# Trade Material Transfer — Remediation Implementation

Implements the P0/P1 items from `docs/audit/TradeMaterialTransfer_Remediation_Plan.md`. No application code was changed or committed. The stored-procedure changes **were deployed and live-tested** against a staging SQL Server instance, with the user's explicit authorization obtained mid-task before any execution — see §9.

## 1. Changes Implemented

| # | File | Type of change |
|---|---|---|
| 1 | `database/migrations/2026-09-09_TradeMaterialTransfer_Idempotency_And_StockSafety.sql` (new) | `ALTER PROCEDURE dbo.sp_Process_TradeMaterialTransferFullProcess` — P0 fixes (§2, §3) |
| 2 | (same file) | `ALTER PROCEDURE dbo.spInsertTradeMaterialTransfer` — P1 fix (§6) |

No `.cs`, `.aspx`, `.csproj`, or any other application file was changed. This repository has no `.sql`/`database/` source for these procedures under version control (confirmed during the original investigation — "no `.sql` files, no `database/` directory and no DDL"), so the two procedures are delivered here as a new, explicit migration script rather than an in-place edit — there was no existing file to edit.

No changes were made to: `Library.DAL`, `Library.DAO`, `Library.BLL`, any invoice/Group Invoice/Direct Sales/ByOrder/ORD-001 page or DAL, `spUpdateTradeMaterialTransfer`, `spDeleteTradeMaterialTransfer`, or the GL voucher logic inside the modified procedure (see §5). Confirmed by `git status` — see §11.

## 1a. Live definition drift — the migration script was rewritten before deployment

The first draft of this script was built against `docs/database/stored-procedures-part2.md`. Before deploying anything, the actual live procedure bodies were pulled via `SELECT OBJECT_DEFINITION(OBJECT_ID(...))` against the staging instance. This revealed that **the deployed `sp_Process_TradeMaterialTransferFullProcess` is materially different from what the documentation describes**:

- The live version already contains a **FIFO multi-batch cursor loop** (`DECLARE dc_cursor CURSOR ... ORDER BY ExpDate ASC, DCStoreId ASC`) that deducts from as many `tblDCStore` batch rows as needed to satisfy the transfer quantity, tracking `@RemainingQty` across iterations — the documented version (and my first draft) assumed a single `TOP 1` row pick with an unsafe "any row" fallback, which is **not** what is actually deployed.
- The live version also has a **three-step weighted-average rate fallback chain** (warehouse-specific → company-wide → `tblUnitPrice.CostPrice`) not mentioned in the documentation at all.
- `spInsertTradeMaterialTransfer`'s live definition was checked the same way and found to match the documentation **exactly** — no drift there.

Per the implementation instructions ("Do NOT blindly implement the examples from the plan... Use the actual... current implementation"), **the migration script was discarded and rewritten from the live body**, not from the documentation. This also **resolves** the "BUSINESS RULE REQUIRED — STOCK ROW SELECTION/FIFO" stop-point raised in the first draft: the live code already has an established selection order (FIFO by `ExpDate`, tie-broken by `DCStoreId`), so it was preserved exactly as-is, not replaced with an invented rule. This is documented here so the discrepancy between `docs/database/` and reality is visible — the documentation for this specific procedure should be treated as stale until someone re-extracts it.

## 2. Idempotency

**Mechanism:** the approval procedure's first write (previously an unconditional `UPDATE ... SET ApprovalStatus='Approved' ...`) is now a **conditional** update:

```sql
UPDATE SalesRollDB_ZAS..tblTradeMaterialTransfer
SET ApprovalStatus = 'Approved', ApprovedBy = @ApprovedBy, ApprovedDate = GETDATE(),
    UpdateBy = @ApprovedBy, UpdateDate = GETDATE()
WHERE TradeMaterialTransferId = @TradeMaterialTransferId
  AND ISNULL(ApprovalStatus, '') <> 'Approved';

IF @@ROWCOUNT = 0
BEGIN
    COMMIT TRANSACTION;
    RETURN;
END
```

**Identity used:** `TradeMaterialTransferId` (the transfer master's clustered PK) — the same parameter the procedure already receives and the same key the pre-existing GL guard already used (`CommonPK_ID = @TradeMaterialTransferId`). `TransferCode` is not used for this or any other idempotency check.

**Why this is safe under concurrency:** an `UPDATE` takes an exclusive row lock while it evaluates and applies. Two sessions calling the procedure for the same `TradeMaterialTransferId` at the same time: whichever session's `UPDATE` acquires the row lock first proceeds; the second session's `UPDATE` blocks on that row until the first commits or rolls back, then re-evaluates its predicate against the now-current row. **This was verified live, not just reasoned about** — see §9/TMT-REG-003: two `sqlcmd` sessions calling the procedure for the same `TradeMaterialTransferId=1016` resulted in exactly one full processing (6 statements, stock deducted once) and one clean no-op (1 statement, `@@ROWCOUNT=0`).

**Status semantics preserved:** live query confirmed all 11 pre-existing `tblTradeMaterialTransfer` rows carry `ApprovalStatus='Approved'` — consistent with the two-value status model (`'Posted'`/`'Approved'`) the investigation and plan were built on. `'Approved'` already meant "fully processed" everywhere else in the application (`TradeMaterialTransfer.aspx.cs` and `TradeMaterialTransferList.aspx.cs` both treat it as "locked, cannot edit/delete"). This implementation does not change that meaning or introduce a new status. Because the claim and every subsequent write (weighted-average, stock, GL) share the same transaction, and any failure after the claim rolls the claim back too (verified live in TMT-REG-004/006, both of which show `ApprovalStatus` reverted to `'Posted'` after a rollback), the invalid state "`Approved` but stock not processed" cannot occur.

**Server-side duplicate-processing protection (P1 item 6):** this claim gate **is** that protection, confirmed live. No separate mechanism was added.

**Live evidence this gap was real, not theoretical:** before deploying the fix, a query for duplicate source-side ledger rows (`tbl_INV_WeightedAvg WHERE Trans_Type_Id=1010 GROUP BY Mst_Trans_Id HAVING COUNT(*)>1`) found `TradeMaterialTransferId=1007` with **two** `-500` qty rows, entered `2026-09-05 10:28:03` and `2026-09-05 16:40:07` — roughly six hours apart, same product. This is exactly the failure mode TMT-004 predicted, already present in this database's history, pre-dating this fix. It was **not** repaired (out of scope — this task only fixes the code path going forward, not historical data; see §10).

## 3. Stock Safety

**Two changes, applied to the ACTUAL live FIFO logic (not the documented single-row logic):**

1. **Per-batch deduction inside the existing cursor loop made atomic and guarded.** The prior loop body did:
   ```sql
   UPDATE SalesRollDB_ZAS..tblDCStore
   SET StockQty = StockQty - @DeductQty
   WHERE DCStoreId = @CurDCStoreId;
   ```
   using `@DeductQty` computed from `@CurStockQty`, a value the cursor had read moments earlier (not locked at read time). This is now:
   ```sql
   UPDATE SalesRollDB_ZAS..tblDCStore
   SET StockQty = StockQty - @DeductQty
   WHERE DCStoreId = @CurDCStoreId
     AND StockQty >= @DeductQty;

   IF @@ROWCOUNT = 0
   BEGIN
       CLOSE dc_cursor; DEALLOCATE dc_cursor;
       RAISERROR('...source stock for DCStoreId %d changed concurrently...', 16, 1, @TradeMaterialTransferId, @CurDCStoreId);
   END
   ```
2. **The unsafe end-of-loop fallback removed.** The prior logic, when the FIFO loop could not source the full quantity from rows that actually had stock (`@RemainingQty > 0` after exhausting all `StockQty > 0` rows), picked **one more row** (`ORDER BY DCStoreId DESC` — unrelated to the FIFO order above) and force-deducted the shortfall from it **unconditionally**, regardless of whether that row had enough — or any — stock. This is now a hard failure:
   ```sql
   IF @RemainingQty > 0
   BEGIN
       DECLARE @RemainingQtyStr NVARCHAR(30) = CONVERT(NVARCHAR(30), @RemainingQty);
       RAISERROR('Trade Material Transfer %d: insufficient aggregate source stock for ProductCode %s at ComUnitId %d (short by %s).',
           16, 1, @TradeMaterialTransferId, @FromProductCode, @FromLocationId, @RemainingQtyStr);
   END
   ```

Both `RAISERROR` calls happen inside the procedure's existing `BEGIN TRY` block, so the existing `BEGIN CATCH ... IF @@TRANCOUNT>0 ROLLBACK ... RAISERROR` unwinds the entire transaction — including the idempotency claim and every batch already deducted earlier in the same loop iteration.

**Live-verified outcomes:**
- **Exact stock (TMT-REG-005):** requesting exactly the remaining balance of a batch row (2 of 2) left it at exactly `0`, never negative.
- **More than available, but some stock exists (TMT-REG-006):** requesting 100 against an aggregate of 5 raised *"insufficient aggregate source stock... (short by 95.000)"* and rolled back completely — the row that had stock (5 units) was confirmed unchanged afterward.
- **Zero stock at all (TMT-REG-004):** a product/location with no `tblDCStore` rows raised *"...short by 1.000"* and rolled back completely.
- In no test did `StockQty` go negative on any row touched by these tests (explicitly re-checked afterward).

**Stock row selection order — NOT changed, and no longer an open question.** The live FIFO order (`ORDER BY ExpDate ASC, DCStoreId ASC`) is an already-established rule in the deployed procedure, discovered during this implementation (§1a) and preserved verbatim. The "BUSINESS RULE REQUIRED — STOCK ROW SELECTION/FIFO" concern from the plan and the first implementation draft is resolved: there was no missing business rule, only stale documentation that didn't reflect it.

## 4. Ledger Safety

No independent guard was added to the weighted-average/batch-stock `INSERT` statements (source or destination). They are single-fire **as a direct consequence of §2**: the procedure now exits at `RETURN` immediately after a failed idempotency claim, and the ledger `INSERT`s appear later in the same procedure body, so a second call for an already-approved (or concurrently-claimed) transfer cannot reach them. **Live-verified**: after TMT-REG-002 (double-click on transfer 1010), the source ledger row count for that transfer stayed at exactly 1, not 2.

The remediation plan's optional secondary `NOT EXISTS` guard on `tbl_INV_WeightedAvg` (defense-in-depth) was **not added**, consistent with "only the minimum change required" — the claim gate alone was sufficient and was proven sufficient live.

## 5. GL

**Not modified.** The existing `IF @TransferValue > 0 AND NOT EXISTS (SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WHERE CommonPK_ID = @TradeMaterialTransferId AND TableID = 2026) BEGIN ... END` block was carried into the migration script byte-for-byte from the live body. Its idempotency key (`CommonPK_ID = TradeMaterialTransferId`) was already aligned with the identity chosen in §2. Live tests naturally exercised the `@TransferValue = 0` path (test product had no cost history), so the GL block's own logic was not directly re-tested by this pass, but its code was not touched.

## 6. TransferCode

**Problem:** `spInsertTradeMaterialTransfer` computed the next sequence number via `SELECT MAX(...)+1` with no lock and no transaction.

**Fix implemented and live-tested:** the number-generation read and the insert are now wrapped in an explicit transaction, with `WITH (UPDLOCK, HOLDLOCK)` on the `MAX()` read, scoped to rows matching the current month's prefix.

**Live-verified (TMT-REG-010):** two `spInsertTradeMaterialTransfer` calls issued back-to-back (as separate OS processes) produced `TMT-26091010` and `TMT-26091011` — distinct, sequential. A full sweep of the entire `tblTradeMaterialTransfer` table (18 rows: 11 pre-existing + 7 created by this test run) for duplicate `TransferCode` values after all testing found **zero** duplicates. See the test-result report's "Concurrency test limitation" note — this demonstrates correct behavior under near-concurrent load but is not a rigorous proof against a true microsecond-level simultaneous race.

**Format preserved exactly:** `'TMT-' + YYMM(@EntryDate) + <4-digit running number starting at 1001>` — confirmed unchanged (the two test codes above follow the exact same shape as the 11 pre-existing ones, e.g. `TMT-26091003`).

## 7. Transaction

**Exact boundary after implementation** (matches what was actually deployed and exercised):

```text
BEGIN TRY
  BEGIN TRANSACTION
    |
    +-- 1. Idempotency claim (conditional UPDATE ApprovalStatus, @@ROWCOUNT check)
    |        └─ IF @@ROWCOUNT = 0: COMMIT (no-op) and RETURN  ← verified live (TMT-REG-002/003/009)
    |
    +-- 2. Source weighted-average rate calc + INSERT (rate fallback chain, unchanged)
    +-- 3. Destination weighted-average calc + INSERT (unchanged)
    +-- 4. FIFO cursor loop over tblDCStore (ORDER BY ExpDate ASC, DCStoreId ASC — unchanged order):
    |        for each batch row: guarded UPDATE (StockQty >= @DeductQty) + @@ROWCOUNT check,
    |        then batch-wise stock INSERT on both sides
    |        └─ IF @RemainingQty > 0 after the loop: RAISERROR (verified live, TMT-REG-004/006)
    |
    +-- 5. GL voucher master+detail INSERT ×2, existing NOT EXISTS guard (unchanged)
    |
  COMMIT TRANSACTION
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION   ← unwinds claim + weighted-avg + stock + GL together, verified live
  RAISERROR (re-throws)
END CATCH
```

`spInsertTradeMaterialTransfer` gained a transaction it did not have before (§6) — additive, verified not to change single-call behavior (still returns the new `TradeMaterialTransferId` via `SCOPE_IDENTITY()` inside the now-explicit transaction).

## 8. Error Handling

- Both new failure points use `RAISERROR(..., 16, 1)`, matching the procedure's existing convention.
- The C# call chain (`ApproveTradeMaterialTransfer` → `UpdateAction` → `ExecuteNonQueryAction`) was reviewed and is already correct: it lets the SQL exception propagate, logs it via `ClsErrorLogger.Log`, and returns `false`; `TradeMaterialTransferApproval.aspx.cs` already checks this per row before counting success. No C# change was needed.
- **One real syntax bug was caught and fixed during deployment, not by static review:** the first version of the "insufficient aggregate stock" `RAISERROR` call passed `CONVERT(NVARCHAR(30), @RemainingQty)` inline as a substitution argument, which SQL Server's `RAISERROR` parser rejects (`Msg 156, Incorrect syntax near the keyword 'CONVERT'`) — substitution arguments must be variables or literals, not expressions. Fixed by assigning the converted value to `@RemainingQtyStr` first, then deployed successfully. This is exactly the kind of thing that would have gone unnoticed without live deployment, and is called out here as a concrete reason the earlier "no SQL execution tool available" pass could not have delivered a trustworthy script on its own.
- The "already processed" no-op path is not distinguished from a genuine failure at the C# layer (same as before) — documented, not changed, per the P2 exclusion on error-message wording.

## 9. Tests

**Authorization:** the user was explicitly asked "There's an active local connection string... Should I use it to deploy the migration and run the TMT-REG live tests?" and answered **"Yes, treat it as staging."** Deployment and live testing proceeded only after this explicit answer.

See `docs/audit/TradeMaterialTransfer_Remediation_Test_Result.md` for full evidence per test.

**Summary:**
- **PASS (8):** TMT-REG-001, 002, 003, 004, 005, 006, 009, 010 — all executed with query-level before/after evidence recorded in the test-result report.
- **NOT RUN (2):** TMT-REG-007, 008 (destination-ledger / GL failure injection) — deliberately not attempted; would require corrupting shared staging schema/data in `CSTL_ERPDB`/`CSTL_ACCDB` beyond what a stock/idempotency test needs, and no isolated fault-injection harness was available.
- **N/A (2):** TMT-REG-011, 012 — no detail table exists in this workflow.
- **Caveat on 003/010:** demonstrated correct behavior under near-concurrent (not rigorously simultaneous) load — see the test-result report's "Concurrency test limitation" section.

**No test is reported as PASS without the query evidence backing it.**

## 10. Remaining Business Questions

1. **FIFO/stock-row selection:** **RESOLVED during this implementation** — the live procedure already implements FIFO-by-`ExpDate`; no business input was actually needed, only accurate information about the deployed code (the documentation was stale).
2. **TransferCode monthly-reset requirement:** unchanged behavior (implicit monthly bucket via the `TMT-YYMM` prefix filter preserved); still no explicit confirmation on whether this is a hard requirement.
3. **Existing duplicate TransferCodes:** checked live — **zero found** as of 2026-09-09. The precondition for a future unique index is satisfied, but this implementation still does not create one (not explicitly approved by the plan — see plan §10, "this plan does not create the constraint").
4. **Reject-flow semantics:** untouched, per explicit instruction. `TradeMaterialTransferApproval.aspx.cs:btnSubmit_Click` still calls `ApproveTradeMaterialTransfer` unconditionally regardless of `actionRadioButtonList.SelectedValue`.
5. **`tblDCStore.StockQty` `decimal(18,0)` vs. `TransferQty` `decimal(18,3)` precision mismatch:** confirmed still present live (`StockQty` column type verified via `sys.columns`); unchanged, outside P0/P1 scope.
6. **Cross-database transaction locality:** **partially resolved for this environment** — `sys.databases` on the staging instance confirms `SalesRollDB_ZAS`, `CSTL_ERPDB`, and `CSTL_ACCDB` are all local databases on the same SQL Server instance, so the procedure's `BEGIN TRAN` stays local there, not MSDTC-promoted. This does **not** confirm the same is true of the actual production topology, which was not accessed or verified by this task.
7. **NEW — historical data divergence found live, not previously known:** `TradeMaterialTransferId=1007` has 2 source-side ledger rows but only 1 destination-side ledger row and 0 GL vouchers (the latter consistent with a zero transfer value, not itself anomalous). The *cause* of the source/destination asymmetry specifically (why the duplicate call produced a second source row but not a second destination row) was not root-caused in this pass — it would require deeper investigation (e.g., application/DB error logs from 2026-09-05) that is out of scope here. **This historical data was left exactly as found — not repaired, not deleted, not modified**, per the instruction against repairing existing data without a separate, explicit authorization for that specific action. Recommend a dedicated reconciliation task.

## 11. Regression

`git status` was reviewed before and after this task (full output, not summary). The only new paths are `database/` and additions under `docs/audit/`. Every other modified/deleted path shown (invoice pages, `Web.config`, build output, `.vs/`, etc.) pre-existed this task and was not touched by it. No invoice, Group Invoice, Direct Sales, ByOrder, ORD-001, or unrelated stock module file was edited at any point. `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` was read (investigation phase) but not modified.

## 12. Build

No C#/.aspx/.csproj file was modified, so no application rebuild was necessary or performed — a direct, verifiable consequence of this change being entirely SQL-side.

The SQL script **was** executed against a live SQL Server instance (staging, per explicit authorization) via `sqlcmd -i`. It deployed successfully on the second attempt after fixing the `RAISERROR`/`CONVERT` syntax error described in §8 — this is the actual, verified build/parse result for the T-SQL, stronger than the static-only review this report originally had to rely on before authorization was given.

---

## FINAL VERDICT

`IMPLEMENTATION COMPLETE — READY FOR LIVE STAGING TEST`

*(Updated from the original `BLOCKED — ENVIRONMENT/ACCESS REQUIRED` verdict once the user explicitly authorized live execution against the local staging instance mid-task.)*

Both procedures were deployed to `TOWSIF\MSSQLSERVER2019` / `SalesRollDB_ZAS` and exercised with real `EXEC` calls covering normal transfer, double-click, near-concurrent approval, zero stock, exact stock, over-request, and near-concurrent `TransferCode` generation — 8 of 12 required scenarios PASS with recorded before/after evidence, 2 are N/A (no detail table), and 2 (fault-injected ledger/GL failure) were deliberately not attempted rather than faked. Live evidence was also found confirming the original defect (TMT-004) had already caused a real duplicate approval in this database's history (`TradeMaterialTransferId=1007`), which strengthens rather than merely theorizes the case for this fix, and that historical data was left untouched.

What is still open, and should not be silently treated as resolved:
- No unique index on `TransferCode` was created (plan explicitly deferred it, precondition now satisfied — a separate decision to actually create it is still needed).
- TMT-REG-007/008 (forced ledger/GL failure) were not run — a more isolated fault-injection setup is recommended before fully signing off on rollback behavior for those two specific failure points, though the shared `BEGIN CATCH`/`ROLLBACK` mechanism they'd exercise was already proven correct by TMT-REG-004/006's rollback evidence.
- The concurrency proof for TMT-REG-003/010 was near-concurrent, not rigorously simultaneous — recommend a proper parallel load test before treating high-concurrency production behavior as fully proven.
- Whether production's actual database topology matches this staging instance's same-server locality for `SalesRollDB_ZAS`/`CSTL_ERPDB`/`CSTL_ACCDB` is still unverified.
- The historical `TradeMaterialTransferId=1007` data divergence found during this pass needs a separate reconciliation decision — it was not touched.

This is a staging result, not a production deployment — the migration has not been run against production, and this task does not claim the system is production-ready as a result of this work.

```text
Files Modified: 0 (existing tracked files)
Files Added: 3 (database/migrations/2026-09-09_TradeMaterialTransfer_Idempotency_And_StockSafety.sql,
                docs/audit/TradeMaterialTransfer_Remediation_Test_Result.md,
                docs/audit/TradeMaterialTransfer_Remediation_Implementation.md)
Database Modified: YES — staging only (TOWSIF\MSSQLSERVER2019 / SalesRollDB_ZAS): 2 stored procedures
                    altered; 7 test rows inserted into tblTradeMaterialTransfer (ids 1010-1016) and
                    their associated weighted-average/batch-stock rows; 2 real tblDCStore rows
                    (70551, 78641) had their StockQty consumed from 5+5 to 0+4 as part of authorized
                    testing.
Data Modified: YES — staging test data only, as above. No production data touched. No pre-existing
               historical data was repaired or altered (including the TMT id 1007 divergence found).
Commit: NO
```
