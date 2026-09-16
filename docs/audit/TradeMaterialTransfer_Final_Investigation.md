# Trade Material Transfer — Final Investigation

Read-only forensic investigation. No source, schema, stored procedure, or data changes were made.

## 1. Executive Summary

**Overall risk: HIGH**

`TradeMaterialTransferList.aspx` itself is a thin search/delete/view screen with no stock logic. The real risk lives one hop away, in the workflow it is the front door to:

- **Save** (`TradeMaterialTransfer.aspx`) only ever writes the single flat master row (`tblTradeMaterialTransfer`) — no stock movement happens at Save. The human-readable `TransferCode` is generated with an unlocked `MAX(...)+1` pattern and there is **no unique index** on `TransferCode`, so concurrent saves can mint duplicate codes.
- **Approve** (`TradeMaterialTransferApproval.aspx` → `sp_Process_TradeMaterialTransferFullProcess`) is where all the real work happens: it flips `ApprovalStatus`, deducts source stock, writes weighted-average/batch-stock rows in two different databases, and posts GL journal vouchers — all inside one `BEGIN TRAN/COMMIT/ROLLBACK` block, so a SQL-level failure rolls back cleanly. **However, the stock/ledger side of that procedure has no idempotency guard**, unlike the GL-posting side which explicitly checks `NOT EXISTS (...)` before posting a voucher. A duplicate call for the same `TradeMaterialTransferId` (double-click, stale grid, two approvers) will deduct stock and write ledger rows **a second time** while the GL side stays correct — a silent stock/accounting divergence.
- Stock deduction at approval time has **no negative-stock guard**: if the picked `tblDCStore` row doesn't have enough quantity, the procedure falls back to "any row" and deducts anyway, which can drive `StockQty` negative.

None of this is visible from `TradeMaterialTransferList.aspx.cs` alone — it only surfaces when the full chain (List → Transfer entry → Approval → stored procedure) is traced, as done below.

## 2. Complete Execution Flow

There is **no single Save-through-stock-movement chain that both writes the master and moves stock in one user action.** The workflow is split into two independently-triggered stages:

### Stage A — Create/Edit (writes ONLY the master row; no stock effect)

```text
TradeMaterialTransferList.aspx (newLinkButton_Click, TradeMaterialTransferList.aspx.cs:103-106)
        ↓ Response.Redirect("TradeMaterialTransfer.aspx")
TradeMaterialTransfer.aspx  →  submitButton_Click (TradeMaterialTransfer.aspx.cs:328-405)
        ↓
TradeMaterialTransferDal.SaveTradeMaterialTransfer (TradeMaterialTransferDal.cs:194-209)
        ↓ (new)                                   ↓ (existing, TradeMaterialTransferId > 0)
InsertTradeMaterialTransfer (TradeMaterialTransferDal.cs:147-168)   UpdateTradeMaterialTransfer (TradeMaterialTransferDal.cs:170-192)
        ↓ ClsCommonInternalDAL.SaveAction              ↓ ClsCommonInternalDAL.UpdateAction
        ↓ (ClsCommonInternalDAL.cs:801-815, 656-670) → ExecuteNonQueryAction (ClsCommonInternalDAL.cs:816-865)
        ↓
spInsertTradeMaterialTransfer (OUTPUT @TradeMaterialTransferId)   spUpdateTradeMaterialTransfer
        ↓                                                          ↓
INSERT dbo.tblTradeMaterialTransfer (ApprovalStatus hardcoded 'Posted')   UPDATE dbo.tblTradeMaterialTransfer (ApprovalStatus reset to 'Posted')
```

No detail table, no stock table, no other side effects. `TradeMaterialTransferList.aspx.cs:36-47` (`LoadGrid`) simply re-queries this same table for display.

### Stage B — Approve (writes stock, ledger, and GL — the only place stock actually moves)

```text
TradeMaterialTransferApproval.aspx  →  btnSubmit_Click (TradeMaterialTransferApproval.aspx.cs:28-77)
        ↓ loop over checked grid rows, per row:
TradeMaterialTransferDal.ApproveTradeMaterialTransfer(id, approvedBy) (TradeMaterialTransferDal.cs:286-293)
        ↓ ClsCommonInternalDAL.UpdateAction → ExecuteNonQueryAction
sp_Process_TradeMaterialTransferFullProcess (@TradeMaterialTransferId, @ApprovedBy)
        ↓ BEGIN TRY / BEGIN TRANSACTION
        ├─ 1. UPDATE SalesRollDB_ZAS..tblTradeMaterialTransfer  SET ApprovalStatus='Approved', ApprovedBy, ApprovedDate, UpdateBy, UpdateDate
        ├─ 2. UPDATE SalesRollDB_ZAS..tblDCStore  SET StockQty = StockQty - @TransferQty   (source stock deduction — see §5, §11)
        ├─ 3. INSERT SalesRollDB_ZAS..tbl_INV_WeightedAvg   (source FG stock-out ledger row)
        │      INSERT SalesRollDB_ZAS..tbl_INV_BatchWiseStock (source batch row, negative Trans_Qty)
        ├─ 4. INSERT CSTL_ERPDB..tbl_INV_WeightedAvg        (destination RM stock-in ledger row, valued at source WAC)
        │      INSERT CSTL_ERPDB..tbl_INV_BatchWiseStock    (destination batch row)
        ├─ 5. IF NOT EXISTS(voucher already posted for this TradeMaterialTransferId):
        │        INSERT CSTL_ACCDB..tblDebitCreditVoucherMaster ×2 (From company, To company)
        │        INSERT CSTL_ACCDB..tblDebitCreditVoucherDetail ×4 (debit/credit legs)
        └─ COMMIT TRANSACTION
        ↓ BEGIN CATCH → IF @@TRANCOUNT>0 ROLLBACK; RAISERROR (re-thrown)
```

`TradeMaterialTransferList.aspx.cs` has no button that reaches Stage B at all — Approve lives on a separate page (`TradeMaterialTransferApproval.aspx`), reached through the permission-driven approval workflow, not through the List page's UI.

## 3. Master

| Field | Value |
|---|---|
| Table | `dbo.tblTradeMaterialTransfer` (single flat table — see §4) |
| Primary key | `TradeMaterialTransferId`, `int`, **Identity**, clustered PK `PK__tblTrade__06E200F1DBD6F5F0` |
| Document/transfer number | `TransferCode`, `nvarchar(50)`, nullable, **no unique index** |
| Transfer date | `EntryDate` (create) / `UpdateDate` (edit) — there is no separate "transfer date" distinct from entry date |
| From location | `FromCompanyId` + `FromLocationId` (this system's own `tblCompanyUnit`) |
| To location | `ToCompanyId` + `ToLocationId` (resolved against `CSTL_ERPDB..tblCompanyUnit`, i.e. the external ERP's location space) |
| Status | `ApprovalStatus` — values observed in code: `'Posted'` (set by Insert/Update SPs) and `'Approved'` (set by the approval SP). The UI (`TradeMaterialTransferList.aspx.cs:166-180`) treats *anything not literally "Approved"* as "Pending" for display — `'Posted'` is shown as a "Pending" badge, which is a labeling mismatch worth noting but not a data bug. |
| CreatedBy/Date | `EntryBy`, `EntryDate` |
| UpdatedBy/Date | `UpdateBy`, `UpdateDate` (also touched by the approval SP) |
| Approval fields | `ApprovedBy`, `ApprovedDate` |

**ID generation:**
- Surrogate key `TradeMaterialTransferId`: safe pattern — `SCOPE_IDENTITY()` via an `OUTPUT` parameter (`spInsertTradeMaterialTransfer`, `ClsCommonInternalDAL.SaveAction`/`ExecuteNonQueryAction`, `ClsCommonInternalDAL.cs:833-843`). No `MAX(Id)+1` risk here.
- Human-readable `TransferCode`: **unsafe pattern.** `spInsertTradeMaterialTransfer` (docs/database/stored-procedures-part1.md:1990-2000) computes it as:
  ```sql
  SELECT @NextSeq = ISNULL(MAX(CAST(RIGHT(TransferCode, 4) AS INT)), 1000) + 1
  FROM dbo.tblTradeMaterialTransfer
  WHERE TransferCode LIKE @Prefix + '%';
  SET @GeneratedCode = @Prefix + CAST(@NextSeq AS NVARCHAR(4));
  ```
  This `SELECT MAX(...)+1` runs with no `BEGIN TRAN`, no `UPDLOCK`/`HOLDLOCK`, and the whole procedure has no explicit transaction. Two inserts landing in the same window (double-click Submit, or two users creating transfers concurrently in the same `TMT-YYMM` prefix) can both read the same `@NextSeq` and insert two rows with the **identical `TransferCode`**. There is no unique constraint on the column to catch this at the database level (§18) — a duplicate `TransferCode` will simply be persisted. Classified as a **concurrency/ID-generation risk — TMT-006/TMT-009 P1, Confirmed by code+schema.**

## 4. Detail

**There is no separate detail table.** `dbo.tblTradeMaterialTransfer` is a flat, single-row-per-transfer table: one FG (from) line and one RM (to) line live in the same row (`FromProductId`/`FromProductCode`/... and `ToItemDescriptionId`/`ToItemDescriptionCode`/...). This is a deliberate 1-master-row = 1-line-item design, not a master/detail schema. Consequences:

- Detail-without-master / master-without-detail (§8/§9 in the template) are **not applicable** — there is only one row per transfer, so this class of defect cannot occur here.
- "Duplicate detail rows" for the same transfer are likewise not applicable (one row = the whole transfer).
- What *does* function as the "detail writes" are the side-effect rows created by the approval stored procedure (`tbl_INV_WeightedAvg`, `tbl_INV_BatchWiseStock`, GL voucher rows) — those are analyzed under §5/§6/§17 because they are the place actual multi-row, multi-table writes occur.
- **Duplicate *master* rows** are possible under the `TransferCode` race described in §3 and under the double-submit scenario in §14 (two full rows, each independently valid, describing the same transfer).

## 5. Source Stock

| Field | Value |
|---|---|
| Table | `SalesRollDB_ZAS..tblDCStore` |
| Row selection key | `ProductCode` + `ComUnitId` (no direct FK to `tblTradeMaterialTransfer`; matched by value) |
| Quantity column | `StockQty` |
| Where deducted | `sp_Process_TradeMaterialTransferFullProcess`, step 2 (docs/database/stored-procedures-part2.md:8407-8437) |
| DAL/BLL | `TradeMaterialTransferDal.ApproveTradeMaterialTransfer` → `ClsCommonInternalDAL.UpdateAction` (no BLL layer between DAL and stored procedure for this call) |

Exact logic:
```sql
SELECT TOP 1 @SourceDCStoreId=DCStoreId, ... FROM tblDCStore
WHERE ProductCode=@FromProductCode AND ComUnitId=@FromLocationId AND StockQty >= @TransferQty;

IF @SourceDCStoreId IS NULL
    SELECT TOP 1 @SourceDCStoreId=DCStoreId, ... FROM tblDCStore
    WHERE ProductCode=@FromProductCode AND ComUnitId=@FromLocationId;  -- no qty condition this time

IF @SourceDCStoreId IS NOT NULL
    UPDATE tblDCStore SET StockQty = StockQty - @TransferQty WHERE DCStoreId=@SourceDCStoreId;
```
See §11/§12 for why this is unsafe.

## 6. Destination Stock

There is **no `tblDCStore`-equivalent physical stock row created on the destination side.** "Destination stock" here means the RM's **weighted-average inventory ledger**, not a discrete stock/bin table:

| Field | Value |
|---|---|
| Table | `CSTL_ERPDB..tbl_INV_WeightedAvg` (running qty/value ledger) + `CSTL_ERPDB..tbl_INV_BatchWiseStock` (batch trail) |
| Key | `Company_Id` + `Warehouse_Id` + `Item_Id` (= `ToCompanyId`/`ToLocationId`/`ToItemDescriptionId`) |
| Quantity column | `Curr_Qty` (= `+@TransferQty`), rolled into a new `Close_Qty`/`Avg_Rate` row |
| Where added | Same stored procedure, step 4 (stored-procedures-part2.md:8476-8517), always **INSERT**, never UPDATE — it's an append-only ledger, so "current stock" is always `TOP 1 ... ORDER BY Weighted_Avg_Id DESC`. |
| Valued at | The **source's** current weighted-average rate (`@From_OP_Rate`), not any price entered on the destination side — `ToCurrentStockQty` captured by the UI is written to the master row but never used in the approval math. |

This confirms source deduction (step 2/3) and destination addition (step 4) are **not** independent asynchronous steps — they execute sequentially inside the same `sp_Process_TradeMaterialTransferFullProcess` transaction, so from a transactional-atomicity standpoint they are as tightly coupled as SQL Server allows in one batch (see §7). The risk is not "one happens without the other" — it's "both happen twice" (§4/§9/§10, idempotency).

## 7. Transaction Boundary

**Inside `sp_Process_TradeMaterialTransferFullProcess` (the only place with an explicit transaction):**

```text
BEGIN TRY
  BEGIN TRANSACTION
    |
    +-- 1. Master status UPDATE (tblTradeMaterialTransfer.ApprovalStatus='Approved')
    +-- 2. Source stock deduction (tblDCStore)
    +-- 3. Source weighted-avg + batch INSERT (SalesRollDB_ZAS)
    +-- 4. Destination weighted-avg + batch INSERT (CSTL_ERPDB)
    +-- 5. GL voucher master+detail INSERT ×2 (CSTL_ACCDB), guarded by NOT EXISTS
    |
  COMMIT TRANSACTION
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
  RAISERROR (re-throws to caller)
END CATCH
```
Everything a *single successful approval* touches is genuinely atomic at the SQL level — a mid-procedure SQL error (constraint violation, deadlock, etc.) rolls the whole thing back. This is the one part of the flow that is well-built.

**Outside any transaction (Stage A, the Save/Update flow):**

```text
No transaction anywhere
    |
    +-- spInsertTradeMaterialTransfer / spUpdateTradeMaterialTransfer  <-- single statement, no BEGIN TRAN, no stock touched
```
This is fine on its own (one INSERT/UPDATE statement is atomic by itself), but it means Stage A and Stage B are two **separate, non-atomic units of work with an arbitrary time gap between them** — by design (approval is a distinct workflow step), not a defect, but it's the reason the stock-validation staleness in §9/§11 is possible.

**Cross-database boundary:** step 2–3 write to `SalesRollDB_ZAS`, step 4 writes to `CSTL_ERPDB`, step 5 writes to `CSTL_ACCDB` — three different databases referenced by three-part names (`DB..table`), all inside one `BEGIN TRAN`. This only stays a **local** transaction (no MSDTC) if all three databases live on the *same* SQL Server instance. That could not be confirmed read-only from this repository (no connection strings resolved to actual server names in scope of this review) — **flagged Suspected/Unknown, not Confirmed.** If any of the three is actually a linked server, SQL Server would auto-promote to a distributed transaction (MSDTC), which changes failure semantics and is worth a follow-up DBA check.

## 8. Failure/Rollback Analysis

| Scenario | Actual Behavior | Expected Behavior | Status |
|---|---|---|---|
| Master (Insert/Update) failure | `SaveAction`/`UpdateAction` catch the exception, log it (`ClsErrorLogger.Log`), return `0`/`false`. `SaveTradeMaterialTransfer` returns a "...failed." string; `submitButton_Click` shows it via `showMessageBox` and does not navigate away. No stock/detail exists yet at this stage, so nothing to roll back. | Full rollback, no side effects | **Matches expectation** |
| Detail failure | N/A — no separate detail table (§4) | N/A | **N/A** |
| Source stock failure (inside approval SP) | Whole SP wrapped in TRY/CATCH + explicit transaction; any failure rolls back status change, stock deduction, ledger inserts and GL inserts together. | Full rollback | **Matches expectation** |
| Destination stock failure (inside approval SP) | Same transaction as above — rolls back with everything else. | Full rollback | **Matches expectation** |
| Exception after stock update, before COMMIT | Caught by `BEGIN CATCH`, rolled back via `IF @@TRANCOUNT>0 ROLLBACK`, re-raised with `RAISERROR`. C# side (`ExecuteNonQueryAction`) catches the re-raised exception, logs it via `ClsErrorLogger.Log` (`UpdateAction`, `ClsCommonInternalDAL.cs:663-667`), returns `false`. UI (`TradeMaterialTransferApproval.aspx.cs:51-69`) just counts it as "not successful" in the batch tally — the underlying SQL error text/detail never reaches the screen. | Full rollback (SQL-level: yes); user sees only a generic count, not why it failed | **Rollback matches; error visibility is degraded (see §12/§16)** |
| **Duplicate approval call for the same `TradeMaterialTransferId`** (double-click, stale grid row, two approvers) | Step 1 re-applies harmlessly. **Step 2 deducts `StockQty` a second time. Steps 3–4 insert a second set of weighted-avg/batch-stock rows.** Step 5 is guarded (`NOT EXISTS`) and does **not** duplicate the GL voucher. Net effect: stock and inventory ledgers are double-counted while the GL stays correct — the two subsystems now disagree. | Idempotent — a second approval call for an already-approved transfer should be a no-op | **DEFECT — see TMT-004/TMT-009/TMT-010, P0** |

## 9. Stock Validation

Two different, disconnected checks exist, and neither is authoritative at the moment stock is actually deducted:

1. **UI/entry-time only** — `TradeMaterialTransfer.aspx.cs:173-186` (`Validation()`), on Save: `transferQty > currentStockQty` where `currentStockQty` comes from `GetFgCurrentStock` (`TradeMaterialTransferDal.cs:63-78`), a `SUM(StockQty) ... HAVING SUM(StockQty) > 0` **aggregate across all `tblDCStore` rows** for that product/location. This is a point-in-time, client-round-trip check with no re-validation — it can be stale by the time of Approve (which may happen minutes, hours, or days later, after other transfers/sales have consumed the same stock).
2. **Approval-time (the only check that matters for what actually gets deducted)** — the stored procedure picks a **single** `tblDCStore` row (`TOP 1 ... WHERE StockQty >= @TransferQty`, falling back to *any* row if none qualifies) and deducts from that one row only, with **no re-check against a live aggregate** and **no atomic `UPDATE ... WHERE StockQty >= @Qty` + rows-affected check** pattern.

Confirmed unsafe pattern (`sp_Process_TradeMaterialTransferFullProcess`, stored-procedures-part2.md:8413-8437):
```sql
SELECT TOP 1 @SourceDCStoreId = DCStoreId ... WHERE ... AND StockQty >= @TransferQty;
IF @SourceDCStoreId IS NULL
    SELECT TOP 1 @SourceDCStoreId = DCStoreId ...  -- no qty condition
IF @SourceDCStoreId IS NOT NULL
    UPDATE tblDCStore SET StockQty = StockQty - @TransferQty WHERE DCStoreId = @SourceDCStoreId;
```
No `WHERE StockQty >= @Qty` on the actual `UPDATE`, no rows-affected check, no `UPDLOCK`/`HOLDLOCK`. Classified **TMT-008, P0, Confirmed by code.**

## 10. Concurrency

**Scenario as specified:** stock = 100, User A transfers 80, User B transfers 50, concurrently.

- The UI-side check (`transferQty > currentStockQty`) happens independently per session against a `SELECT SUM(...)` snapshot — both A and B can see "100 available" and both pass entry-time validation.
- At **approval** time, the deduction (`UPDATE tblDCStore SET StockQty = StockQty - @TransferQty WHERE DCStoreId=@SourceDCStoreId`) has no `WHERE StockQty >= @TransferQty` guard and no locking hint. SQL Server's default read-committed isolation does take a row/page lock for the duration of the `UPDATE` itself, so the two `UPDATE` statements will not interleave *mid-write* — but nothing stops **both** from executing to completion regardless of the resulting value, because neither checks the precondition against the row it is about to modify.
- Net result: **`100 → -30` is possible** if both approvals target the same `tblDCStore` row (same product/location) and are approved back-to-back — the procedure has no mechanism that would refuse the second one. Classified **TMT-009, P0, Confirmed by code (no test executed against a live DB — logic-level confirmation only).**
- No `SERIALIZABLE`, `UPDLOCK`, or `HOLDLOCK` hint appears anywhere in the reviewed stored procedures (checked both `docs/database/stored-procedures-part1.md` and `-part2.md` for this workflow).

## 11. Duplicate Submission

- **Save (Stage A):** `submitButton` (`TradeMaterialTransfer.aspx:307-309`) has `OnClientClick="return confirm(...)"` but **no client-side disable-after-click and no server-side duplicate/idempotency check.** Combined with the unlocked `TransferCode` generation (§3), a double-click (confirm dialog clicked, then a second physical click lands before the async postback returns) can create two master rows with duplicate `TransferCode` values and no way for the database to reject it (no unique index, §18).
- **Approve (Stage B):** `btnSubmit` (spec: `PAGE-INV-026-TradeMaterialTransferApproval.md:154`) also only has a `confirm(...)` dialog, no disable-on-click. As shown in §8/§9/§10, a duplicate call for the same row is **not** rejected by the stored procedure for the stock/ledger side (only the GL side is idempotency-guarded). This is the same defect as §8's last row, restated here as the "duplicate submission" trigger for it. Classified **TMT-010, P0, Confirmed by code.**
- No unique constraint exists on `TransferCode`, and no application-level check (e.g., "does an identical From/To/Product/Qty/EntryBy transfer already exist in the last N seconds") is performed anywhere in `TradeMaterialTransferDal` or the two code-behind files.

## 12. Error Handling

Exception path traced through all layers actually used by this workflow:

```text
Page (TradeMaterialTransferList.aspx.cs / TradeMaterialTransfer.aspx.cs / TradeMaterialTransferApproval.aspx.cs)
   ↓ calls DAL method, checks only a bool/string/int return value — never an exception
TradeMaterialTransferDal (thin pass-through, no try/catch of its own)
   ↓
ClsCommonInternalDAL.SaveAction / UpdateAction / DeleteAction
   ↓ try { ExecuteNonQueryAction(...) } catch (Exception ex) { ClsErrorLogger.Log(...); return false/0; }
ClsCommonInternalDAL.ExecuteNonQueryAction
   ↓ try { ... } catch (Exception ex) { throw ex; }   <-- rethrows (loses original stack trace: `throw ex`, not `throw;`)
SQL Server / ADO.NET
```
Findings:
- `SaveAction`/`UpdateAction` **swallow** every exception from `ExecuteNonQueryAction` and convert it to a boolean/int `false`/`0` after logging via `ClsErrorLogger.Log` (`ClsCommonInternalDAL.cs:806-815`, `658-667`). `DeleteAction` (`ClsCommonInternalDAL.cs:206-218`) does the same but **without even calling `ClsErrorLogger.Log`** — a delete failure (SQL error, timeout, constraint violation) is silently converted to `false` with **zero logging**, and `TradeMaterialTransferList.aspx.cs:134-137` just shows a generic "delete failed" message.
- `ExecuteNonQueryAction` itself uses `throw ex;` instead of `throw;` (`ClsCommonInternalDAL.cs:860-863`), which resets the stack trace to this rethrow point — makes root-causing a production error harder, though it doesn't change program behavior.
- Every caller (`TradeMaterialTransferList.aspx.cs`, `TradeMaterialTransfer.aspx.cs`, `TradeMaterialTransferApproval.aspx.cs`) checks the returned bool/string, so failures are *not* ignored at the call site — the user does get a "failed" message. The gap is **loss of detail**: the actual SQL error (constraint violation, RAISERROR text from `sp_Process_TradeMaterialTransferFullProcess`, etc.) never reaches the UI, only "Approval failed." / "...update failed." This makes diagnosing *why* a real production failure happened dependent entirely on `ClsErrorLogger.Log` output existing and being checked.
- Classified **TMT-011, P2, Confirmed by code** (error handling degrades diagnosability; it does not by itself cause data corruption since the underlying SP transaction still rolls back correctly on failure).

## 13. Stored Procedures

| SP | Caller | Tables Read | Tables Modified | Own Transaction? | TRY/CATCH? | Stock Op | Idempotent? |
|---|---|---|---|---|---|---|---|
| `spInsertTradeMaterialTransfer` | `TradeMaterialTransferDal.InsertTradeMaterialTransfer` | `dbo.tblTradeMaterialTransfer` (for `TransferCode` MAX) | INSERT `dbo.tblTradeMaterialTransfer` | No | No | None | N/A (a second call always creates a new row — by design) |
| `spUpdateTradeMaterialTransfer` | `TradeMaterialTransferDal.UpdateTradeMaterialTransfer` | none | UPDATE `dbo.tblTradeMaterialTransfer` | No | No | None | Yes (pure overwrite by PK) |
| `spDeleteTradeMaterialTransfer` | `TradeMaterialTransferDal.DeleteTradeMaterialTransfer` | `dbo.tblTradeMaterialTransfer` | DELETE `dbo.tblTradeMaterialTransfer` | No | No | None | Yes (delete of an already-deleted id is a no-op, 0 rows affected) |
| `sp_Process_TradeMaterialTransferFullProcess` | `TradeMaterialTransferDal.ApproveTradeMaterialTransfer` | `SalesRollDB_ZAS..tblTradeMaterialTransfer`, `..tblDCStore`, `..tbl_INV_WeightedAvg`; `CSTL_ERPDB..tbl_INV_WeightedAvg`, `..tblCompanyInfo`, `..Table_Integration_AccountsConnectivity`, `..Table_Integration_MappingByLinkAccounts`, `..tblItemCategory/tblItemName/tblItemDescription`; `CSTL_ACCDB..tblDebitCreditVoucherMaster/Detail`, `..tblChartOfAccounts`, `..tblFinancialYear` | `SalesRollDB_ZAS..tblTradeMaterialTransfer`, `..tblDCStore`, `..tbl_INV_WeightedAvg`, `..tbl_INV_BatchWiseStock`; `CSTL_ERPDB..tbl_INV_WeightedAvg`, `..tbl_INV_BatchWiseStock`; `CSTL_ACCDB..tblDebitCreditVoucherMaster/Detail` | **Yes** (`BEGIN TRAN...COMMIT`) | **Yes** (rolls back + `RAISERROR`) | Deducts source `StockQty`, adds destination weighted-avg qty | **No for stock/ledger steps (2–4); Yes for GL step (5)** — this asymmetry is the P0 finding (§8/§9/§10) |

No page-level (.NET `TransactionScope`/`SqlTransaction`) transaction wraps any of these calls — the only transaction boundary in the whole chain is the one inside `sp_Process_TradeMaterialTransferFullProcess`.

## 14. Database Constraints

From `docs/database/tables.md:5732-5767` (`tblTradeMaterialTransfer`):

- PK: `TradeMaterialTransferId` (clustered, identity) — the only index on the table.
- **No unique index/constraint on `TransferCode`.**
- **No FK constraints** documented from `tblTradeMaterialTransfer` to `tblCompanyInfo`/`tblCompanyUnit`/`tblProduct`/`tblItemDescription` — all cross-references (`FromCompanyId`, `ToCompanyId`, `FromProductId`, `ToItemDescriptionId`, etc.) are logical/value-matched only, consistent with `docs/database/relationships.md:230`'s note that several of these columns point at the external `CSTL_ERPDB` database, which cannot carry a same-database FK anyway.
- No CHECK constraint on `TransferQty` (nothing prevents a negative or zero value from being *stored*, though the entry-page UI does reject `<= 0`, see §15).
- No unique constraint tying `(FromCompanyId, FromLocationId, FromProductId, ToCompanyId, ToLocationId, ToItemDescriptionId, EntryDate)` or similar — nothing at the DB layer prevents duplicate transfer rows.
- `tblDCStore` schema/constraints were not in scope of this read-only pass beyond what the SP body already reveals (no CHECK `StockQty >= 0` is exercised by the procedure, since it deducts unconditionally once a row is chosen).

## 15. Existing Data Integrity

**Blocked for this pass** — no live/read-only SQL Server connection was available in this investigation environment (no DB tool, connection string, or credentials were exercised). This section could not be executed as `SELECT` queries against the actual database; it is based entirely on static analysis of the C#/stored-procedure source and the schema documentation in `docs/database/`.

If DB access becomes available, the following read-only queries would directly confirm/refute the P0 findings above and should be run before any remediation:

```sql
-- A. Duplicate TransferCode values (confirms §3 TMT-006)
SELECT TransferCode, COUNT(*) AS Cnt
FROM dbo.tblTradeMaterialTransfer
GROUP BY TransferCode
HAVING COUNT(*) > 1;

-- B. Negative stock in the table this workflow deducts from (confirms §9/§10 TMT-008/TMT-009)
SELECT * FROM SalesRollDB_ZAS..tblDCStore WHERE StockQty < 0;

-- C. Approved transfers with more than one matching weighted-avg ledger entry
--    (Mst_Trans_Id / Trans_Id = TradeMaterialTransferId is set by the SP itself; >1 row per id = duplicate approval, confirms §8/§9 TMT-004)
SELECT Mst_Trans_Id, COUNT(*) AS Cnt
FROM SalesRollDB_ZAS..tbl_INV_WeightedAvg
WHERE Trans_Type_Id = 1010
GROUP BY Mst_Trans_Id
HAVING COUNT(*) > 1;

SELECT Mst_Trans_Id, COUNT(*) AS Cnt
FROM CSTL_ERPDB..tbl_INV_WeightedAvg
WHERE Trans_Type_Id = 2010
GROUP BY Mst_Trans_Id
HAVING COUNT(*) > 1;

-- D. Approved transfers where the GL voucher count and the ledger-entry count disagree
--    (would show the accounting/inventory divergence predicted in §8)
SELECT TMT.TradeMaterialTransferId,
       (SELECT COUNT(*) FROM CSTL_ACCDB..tblDebitCreditVoucherMaster V WHERE V.CommonPK_ID = TMT.TradeMaterialTransferId AND V.TableID = 2026) AS VoucherCount,
       (SELECT COUNT(*) FROM SalesRollDB_ZAS..tbl_INV_WeightedAvg W WHERE W.Mst_Trans_Id = TMT.TradeMaterialTransferId AND W.Trans_Type_Id = 1010) AS LedgerCount
FROM dbo.tblTradeMaterialTransfer TMT
WHERE TMT.ApprovalStatus = 'Approved';
```

## 16. Related Module Comparison

`TradeMaterialTransferDal`/`ClsCommonInternalDAL` are shared infrastructure used across dozens of modules (the same `SaveAction`/`UpdateAction`/`DeleteAction` swallow-and-log pattern, and the same lack of `UPDLOCK`/`HOLDLOCK` anywhere in the reviewed stored-procedure docs, is systemic, not unique to this page). A grep across both stored-procedure doc files found **no** use of `UPDLOCK`, `HOLDLOCK`, or `SERIALIZABLE` in any procedure, and the only `StockQty = StockQty ± @Qty` pattern found across the two files is the one inside `sp_Process_TradeMaterialTransferFullProcess` itself — i.e. this review did not find a "safer" sibling stock-transfer procedure in the same documentation set to contrast against. `DepoToWHTransfer.aspx`, `NewStockTransferDcToDc.aspx`, and `StockTransferOrder.aspx` were not independently re-audited in this pass (out of scope per the task's focus on `TradeMaterialTransferList.aspx`); their stored procedures were not located in the two stored-procedure doc files searched. No claim is made about their behavior — flagged **Not Compared / Unknown** rather than assumed similar or different.

## 17. Findings

### TMT-001 — Transaction boundary
- **Severity:** P2
- **Status:** Confirmed
- **Evidence:** No `TransactionScope`/`SqlTransaction` anywhere in `TradeMaterialTransferList.aspx.cs`, `TradeMaterialTransfer.aspx.cs`, `TradeMaterialTransferApproval.aspx.cs`, or `TradeMaterialTransferDal.cs`. The only transaction is inside `sp_Process_TradeMaterialTransferFullProcess`.
- **Root Cause:** By design — Save and Approve are separate business steps, not a single atomic operation, so a page-spanning transaction was never intended.
- **Impact:** Low on its own; it is the reason the entry-time stock check (§9) can go stale before Approve executes.
- **Recommended Fix:** No change required for Save/Approve being separate steps; ensure any future automation that chains Save→Approve wraps both DAL calls' *outcomes* (not the SQL itself) in compensating logic.
- **Code Change Required:** NO
- **Database Change Required:** NO

### TMT-002 — Master save
- **Severity:** P3
- **Status:** Confirmed
- **Evidence:** `spInsertTradeMaterialTransfer`/`spUpdateTradeMaterialTransfer` are single-statement, no TRY/CATCH; failures surface as `SaveAction`/`UpdateAction` returning `0`/`false`, correctly checked in `TradeMaterialTransfer.aspx.cs:391-404`.
- **Root Cause:** N/A — behaves as expected for a single-statement operation.
- **Impact:** None observed.
- **Recommended Fix:** None required.
- **Code Change Required:** NO
- **Database Change Required:** NO

### TMT-003 — Detail save
- **Severity:** N/A
- **Status:** Not Found (no detail table exists — §4)
- **Evidence:** `TradeMaterialTransferDao`/`tblTradeMaterialTransfer` schema — one row encodes both FG and RM sides.
- **Root Cause:** N/A
- **Impact:** N/A
- **Recommended Fix:** N/A
- **Code Change Required:** NO
- **Database Change Required:** NO

### TMT-004 — Source stock deduction (duplicate-approval double-deduction)
- **Severity:** P0
- **Status:** Confirmed (by code; not exercised against a live DB — see §15)
- **Evidence:** `sp_Process_TradeMaterialTransferFullProcess` steps 2–3 (docs/database/stored-procedures-part2.md:8407-8474) have no `NOT EXISTS`/idempotency guard, unlike step 5 (GL posting, line 8522) which explicitly has one.
- **Root Cause:** The stock-deduction and ledger-insert steps assume the procedure is called exactly once per transfer; nothing enforces that assumption.
- **Impact:** A duplicate call (double-click, stale approval grid, concurrent approvers) deducts `tblDCStore.StockQty` a second time and inserts a second weighted-average/batch-stock row pair, silently overstating the stock movement while the GL stays correct — physical/ledger stock and accounting diverge.
- **Recommended Fix:** Add a guard at the top of the procedure: if `ApprovalStatus` is already `'Approved'` for `@TradeMaterialTransferId`, exit without repeating steps 2–4 (mirroring the pattern already used for step 5).
- **Code Change Required:** NO (page/DAL code is fine as-is)
- **Database Change Required:** YES (stored procedure)

### TMT-005 — Destination stock addition
- **Severity:** P0 (same root cause as TMT-004)
- **Status:** Confirmed
- **Evidence:** Step 4 (stored-procedures-part2.md:8476-8517), same missing guard as step 2–3.
- **Root Cause:** Same as TMT-004.
- **Impact:** Same as TMT-004, on the destination (`CSTL_ERPDB`) side.
- **Recommended Fix:** Same fix as TMT-004 covers this too (single guard at top of procedure).
- **Code Change Required:** NO
- **Database Change Required:** YES

### TMT-006 — Master ID / TransferCode generation
- **Severity:** P1
- **Status:** Confirmed
- **Evidence:** `spInsertTradeMaterialTransfer` (stored-procedures-part1.md:1990-2000), unlocked `SELECT MAX(...)+1`; `docs/database/tables.md:5762-5766` confirms no unique index on `TransferCode`.
- **Root Cause:** Sequence number derived from existing data via `MAX()+1` without a lock, serializable isolation, or a database sequence/unique constraint to catch collisions.
- **Impact:** Two concurrent inserts in the same `TMT-YYMM` prefix can produce duplicate `TransferCode` values, which are user-facing document numbers — confusing for search/audit/reconciliation, and would corrupt any future logic that assumes `TransferCode` is unique.
- **Recommended Fix:** Add a unique index on `TransferCode`, and either use a `SEQUENCE` object or serialize the `MAX()+1` read with `UPDLOCK, HOLDLOCK` on the range scan (or a dedicated counter table with `UPDLOCK`).
- **Code Change Required:** NO
- **Database Change Required:** YES

### TMT-007 — Master/detail integrity
- **Severity:** N/A
- **Status:** Not Found (no detail table — §4)
- **Evidence:** Same as TMT-003.
- **Root Cause / Impact / Fix:** N/A
- **Code Change Required:** NO
- **Database Change Required:** NO

### TMT-008 — Stock validation
- **Severity:** P0
- **Status:** Confirmed
- **Evidence:** `sp_Process_TradeMaterialTransferFullProcess` (stored-procedures-part2.md:8413-8437) — the actual `UPDATE tblDCStore SET StockQty = StockQty - @TransferQty` has no `WHERE StockQty >= @TransferQty` and no rows-affected check; the fallback branch explicitly picks a row **without** a quantity condition when no row has enough stock.
- **Root Cause:** Stock sufficiency is only checked client-side at Save time (`TradeMaterialTransfer.aspx.cs:180-186`), against a stale aggregate, and never re-validated atomically at the point of deduction.
- **Impact:** `tblDCStore.StockQty` can be driven negative by a single approval alone (no concurrency even required) if stock was consumed elsewhere between Save and Approve.
- **Recommended Fix:** Change the deduction to `UPDATE tblDCStore SET StockQty = StockQty - @TransferQty WHERE DCStoreId=@SourceDCStoreId AND StockQty >= @TransferQty;` and check `@@ROWCOUNT = 1`; if 0, `RAISERROR` and let the existing `CATCH`/rollback handle it.
- **Code Change Required:** NO
- **Database Change Required:** YES

### TMT-009 — Concurrency/race condition
- **Severity:** P0
- **Status:** Confirmed (by code inspection; §15 queries would give live confirmation)
- **Evidence:** §10 above; no `UPDLOCK`/`HOLDLOCK`/`SERIALIZABLE` anywhere in the reviewed procedures; no rows-affected re-check on the stock `UPDATE`.
- **Root Cause:** Same missing guard as TMT-008, compounded by the lack of any locking hint.
- **Impact:** Two simultaneous approvals against the same `tblDCStore` row can both succeed and jointly overdraw stock (e.g. `100 → -30`).
- **Recommended Fix:** Same as TMT-008 (conditional `UPDATE` + rows-affected check achieves both goals at once).
- **Code Change Required:** NO
- **Database Change Required:** YES

### TMT-010 — Duplicate submission
- **Severity:** P0
- **Status:** Confirmed
- **Evidence:** §11 — `submitButton`/`btnSubmit` have only a `confirm()` dialog, no disable-on-click, on both `TradeMaterialTransfer.aspx` and `TradeMaterialTransferApproval.aspx`; no server-side idempotency check backs either.
- **Root Cause:** No client-side re-entrancy guard and no server-side duplicate-request detection.
- **Impact:** Compounds TMT-004/005/006/009 — this is the practical trigger for all of them.
- **Recommended Fix:** Disable the button via `OnClientClick` (e.g. append `this.disabled=true;` after the `confirm()`), and add the TMT-004/005 approval-status guard as the authoritative server-side backstop (client-side disabling alone is not sufficient defense).
- **Code Change Required:** YES (client-side button disable) — the authoritative fix is still the DB-side guard in TMT-004/005.
- **Database Change Required:** YES (see TMT-004/005/006/008/009)

### TMT-011 — Error handling
- **Severity:** P2
- **Status:** Confirmed
- **Evidence:** §12 — `SaveAction`/`UpdateAction` swallow exceptions after logging; `DeleteAction` swallows **without logging**; `ExecuteNonQueryAction` uses `throw ex;` (resets stack trace) instead of `throw;`.
- **Root Cause:** Shared DAL helper methods designed to convert all failures into a boolean/int return rather than propagate typed exceptions.
- **Impact:** No data-corruption impact (the SP-level transaction still protects data integrity on failure) but materially slows down diagnosing *why* an approval or delete failed in production, since only a generic message reaches the user and `DeleteAction` failures aren't logged at all.
- **Recommended Fix:** Add `ClsErrorLogger.Log(...)` to `DeleteAction`'s catch block; change `throw ex;` to `throw;` in `ExecuteNonQueryAction`.
- **Code Change Required:** YES
- **Database Change Required:** NO

### TMT-012 — Stored procedure transaction behavior
- **Severity:** P3 (informational / Suspected)
- **Status:** Suspected
- **Evidence:** §7 — `sp_Process_TradeMaterialTransferFullProcess` writes across `SalesRollDB_ZAS`, `CSTL_ERPDB`, `CSTL_ACCDB` inside one `BEGIN TRAN`, using three-part names only (no four-part linked-server syntax visible in the SP body).
- **Root Cause:** Whether this stays a local transaction or escalates to MSDTC depends on whether all three databases are on the same physical/logical SQL Server instance — not verifiable read-only from source alone.
- **Impact:** If any database is remote (linked server), MSDTC requirements/availability become a dependency for every approval; a misconfigured or unavailable DTC would turn every approval into a hard failure (safe, but disruptive) rather than silent corruption.
- **Recommended Fix:** Confirm with a DBA that `SalesRollDB_ZAS`, `CSTL_ERPDB`, and `CSTL_ACCDB` are co-located on one instance; no code change implied either way.
- **Code Change Required:** NO
- **Database Change Required:** NO (verification only)

### TMT-013 — Database constraints
- **Severity:** P1
- **Status:** Confirmed
- **Evidence:** §14 — only a clustered PK exists on `tblTradeMaterialTransfer`; no unique index on `TransferCode`, no FKs, no CHECK constraints.
- **Root Cause:** Table was created without secondary constraints beyond the PK.
- **Impact:** Nothing at the database layer prevents the duplicate-`TransferCode` (TMT-006) or duplicate-row (TMT-010) scenarios from persisting once they occur; recovery is manual/forensic only.
- **Recommended Fix:** Add unique index on `TransferCode`; consider a CHECK constraint `TransferQty > 0` as defense-in-depth (the UI already enforces this, but nothing stops a future direct-SQL or alternate-caller insert from violating it).
- **Code Change Required:** NO
- **Database Change Required:** YES

### TMT-014 — Existing data integrity
- **Severity:** Unknown (would drive re-prioritization if confirmed)
- **Status:** Blocked — no read-only SQL access available in this environment
- **Evidence:** N/A
- **Root Cause:** N/A
- **Impact:** Unknown until the §15 queries are run.
- **Recommended Fix:** Run the four queries in §15 against a read-only replica/reporting copy before scheduling remediation, to know whether TMT-004/006/008/009 have already produced bad data that needs separate data-correction (out of scope for this investigation, which is read-only-forensic only).
- **Code Change Required:** NO
- **Database Change Required:** NO (read-only verification only)

## 18. Recommended Remediation Order

```text
P0 — Stock/data corruption
     - TMT-008: add WHERE StockQty >= @Qty + @@ROWCOUNT check to the source deduction UPDATE
     - TMT-004/TMT-005: add an ApprovalStatus='Approved' short-circuit guard at the top of
       sp_Process_TradeMaterialTransferFullProcess (mirrors the existing GL NOT EXISTS pattern)
     - TMT-009: covered by the same conditional UPDATE fix as TMT-008

P1 — Transaction atomicity / Master-detail integrity / Stock validation
     - TMT-006/TMT-013: unique index on TransferCode; serialize the TransferCode sequence
       generation (UPDLOCK/HOLDLOCK or a SEQUENCE object)
     - TMT-012: confirm cross-database co-location with DBA (verification, no code change)

P2 — Error handling / Duplicate submission
     - TMT-010: disable Submit/Approve buttons on click (client-side defense-in-depth;
       the P0 DB-side guard above is the real fix)
     - TMT-011: log DeleteAction failures; fix throw ex; → throw;

P3 — Refactoring / cleanup
     - Labeling: 'Posted' status displayed as "Pending" on the List page is confusing but
       not incorrect — consider aligning the badge text with the actual stored value.
```

## 19. Remediation Test Plan

1. Normal transfer: Save → Approve once → verify one `tblDCStore` deduction, one destination weighted-avg row, one GL voucher pair.
2. Multiple products: repeat #1 for several different `FromProductId`/`ToItemDescriptionId` pairs in sequence; verify no cross-contamination of ledger rows (`Mst_Trans_Id` scoping).
3. Exact-stock transfer: `TransferQty == StockQty` → resulting `StockQty == 0`, not negative, not blocked.
4. Insufficient stock at approval time (stock consumed by another operation between Save and Approve): approval should fail cleanly (post-fix) instead of deducting into negative.
5. Zero stock at approval time: same as #4, should refuse rather than fall back to an unconditional deduction.
6. Zero transfer quantity: confirm UI (`Validation()`) still blocks `<= 0`; confirm DB layer also rejects it after a CHECK constraint is added (defense-in-depth).
7. Negative transfer quantity: same as #6.
8. Master save failure (simulate SP exception): confirm no stock/ledger/GL rows are created, and the UI shows a failure message.
9. N/A — no detail table.
10. N/A — no detail table.
11. Source-stock-deduction failure inside approval SP (simulate a constraint violation mid-procedure): confirm full rollback of status/stock/ledger/GL together.
12. Destination-stock failure inside approval SP: same as #11, confirm full rollback.
13. Exception raised after stock deduction but before COMMIT: confirm `tblDCStore` reverts to its pre-approval value (rollback), and `ApprovalStatus` stays `'Posted'`.
14. Concurrent transfer: two sessions approve transfers against the same product/location simultaneously with combined quantity > available stock — confirm (post-fix) the second is rejected rather than allowed to overdraw.
15. Double-click submission: click Save/Approve twice in rapid succession — confirm (post-fix) only one master row / one stock deduction results, not two.
16. Duplicate transfer number: attempt to force two inserts in the same `TMT-YYMM` window — confirm (post-fix) the unique index rejects the second with a clean error, not a silently duplicated code.
17. Master/detail relationship: N/A (no detail table) — instead verify one `tblTradeMaterialTransfer` row consistently maps to exactly one set of ledger/GL rows post-approval (no duplicates, no orphans).
18. Stock before/after reconciliation: sum `tbl_INV_WeightedAvg.Curr_Qty` movements attributable to a `TradeMaterialTransferId` against the corresponding `tblDCStore` delta — should match exactly, both before and after the TMT-004/005 fix.
19. Cancel/reversal: **no Cancel/Reject stock-reversal path currently exists in the traced code** — `TradeMaterialTransferApproval.aspx.cs:btnSubmit_Click` always calls `ApproveTradeMaterialTransfer` regardless of which `actionRadioButtonList` option is selected (the selected value is never inspected). If a "Reject" action is expected to exist and *not* move stock, this needs explicit verification/build — currently, selecting anything and clicking Submit runs the same approve-and-move-stock procedure. Flagged as a **Suspected** gap worth a dedicated follow-up, separate from the P0 list above since it wasn't the primary target of this investigation.
20. Approval flow: exercise `TradeMaterialTransferApproval.aspx` end-to-end per §2 Stage B, confirming the `ApprovalStatus` transition `'Posted' → 'Approved'` and all five side-effect groups occur exactly once.

## 20. FINAL VERDICT

**CRITICAL — STOCK/TRANSACTION CORRUPTION RISK**

The List page itself is inert with respect to stock movement, but tracing its "Add/Edit" and downstream "Approve" actions to their actual stored procedures surfaces a P0, code-confirmed defect: `sp_Process_TradeMaterialTransferFullProcess` can deduct source stock and write destination ledger rows more than once for the same transfer (no idempotency guard on the stock/ledger steps, unlike the GL-posting step which has one), and its stock deduction has no atomic sufficiency check — both are independently capable of producing negative or double-counted stock without any unusual or malicious input, just an ordinary double-click or two overlapping approvals.

```text
Files Modified: 0
Database Modified: 0
Data Modified: 0
Commit: NO
Report: docs/audit/TradeMaterialTransfer_Final_Investigation.md
```
