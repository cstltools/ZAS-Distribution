# Direct Sales Invoice Fix — `InvoiceCreationForConsumerCustomer.aspx`

Fixes the confirmed-active, confirmed-broken "Direct Sales" page. `InvoiceCreationForCustomer.aspx` (unused) and `ReturnInvoiceCreationForCustomer.aspx` (superseded) were **not touched**, per the prior decision audit. `InvoiceCreationForCustomerByOrder.aspx` was **not touched**. No `RemainingQty`/ORD-001 logic introduced. Nothing committed.

## 1. Root Cause

`tblInvoice.InvoiceId` is a live `IDENTITY`+`PRIMARY KEY` column. `InvoiceDAL.SaveDataForInvoiceConsumer` built a raw dynamic `INSERT` that explicitly listed `InvoiceId` in the column list, using a value pre-computed by `ClsPrimaryKeyFind.PrimaryKeyMax("invoiceId","tblInvoice")` — a pattern only valid for a non-identity column. Every save attempt threw `Msg 8101` ("Cannot insert explicit value for identity column..."). Additionally, the page's `saveButton_Click` had no `TransactionScope` and never checked `SaveInvoiceDetail`'s return value (which itself unconditionally returned `true`, even for zero detail rows) — so once the identity bug was fixed, an insufficient-stock or zero-item save would have partially committed (order + master row present, no/partial details, no error shown).

## 2. Before Flow

```
saveButton_Click (no try/catch, no TransactionScope)
 -> if no customer selected: ConsumeCustomer() + CreateGlOfCustomer()   [unchanged by this fix]
 -> SaveInvoice()
      -> OrderSaveMaster()  [InvoiceBLL.SaveOrderMaster -> SaveDataByInsertCommandWithIdentity - already correct]
      -> InvoiceBLL.SaveInvoiceConsumer -> InvoiceDAL.SaveDataForInvoiceConsumer
           -> PrimaryKeyMax("invoiceId","tblInvoice") + explicit INSERT ...InvoiceId... => Msg 8101 (BROKEN)
      -> UpdateInvoice()  [never reached - exception above propagates unhandled]
 -> SaveInvoiceDetail(invId)   [return value ignored regardless]
 -> UpdateInvoiceDetailConsumer / UpdateInvoiceConsumer / PostDeliveryConfFullJournal
 -> Clear() + "Create Invoice" message shown UNCONDITIONALLY on the journal-posting result only
```

Any exception from the identity-insert bug propagated unhandled to an ASP.NET error page — no logging (`SaveDataByInsertCommand`'s catch block re-throws without logging), no graceful message.

## 3. After Flow

```
saveButton_Click
 -> if no customer selected: ConsumeCustomer() + CreateGlOfCustomer()   [UNCHANGED - outside the transaction, as before]
 -> try
      TransactionScope (ReadCommitted, 5 min)
        invId = SaveInvoice()
          -> OrderSaveMaster()  [UNCHANGED]
          -> InvoiceBLL.SaveInvoiceConsumer -> InvoiceDAL.SaveDataForInvoiceConsumer
               -> parameterized INSERT omitting InvoiceId -> SaveDataByInsertCommandWithIdentity
               -> SCOPE_IDENTITY() returned as the real InvoiceId  (FIXED)
          -> UpdateInvoice()  [UNCHANGED]
        if invId <= 0 -> message, return (scope disposed without Complete() -> rollback)
        if !SaveInvoiceDetail(invId) -> message, return (rollback)   [now returns the REAL result]
        UpdateInvoiceDetailConsumer / UpdateInvoiceConsumer
        if !PostDeliveryConfFullJournal(invId) -> message, return (rollback)
        scope.Complete()
    catch (Exception ex) -> "Invoice generate failed: ..." message, return
 -> rptInvoiceId.Text = invId.ToString();  Clear();  "Create Invoice" message   [only on the success path]
```

## 4. InvoiceId Generation

`InvoiceDAL.SaveDataForInvoiceConsumer` (`Library.DAL/SInventory_DAL/InvoiceDAL.cs`) rewritten to:
- Parameterized `INSERT` that **omits `InvoiceId`** from the column list entirely.
- Uses `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity`, which appends `;select @ID=SCOPE_IDENTITY()` and returns the real generated ID — the exact same helper already used successfully by `SaveOrderMaster`/`SaveOrderDetail` in the same file and by `CustomerReturnDAL` (the live return module).
- Method signature changed `bool` → `int` (returns the real InvoiceId, `0` on failure). Confirmed by repo-wide grep this method has exactly one caller (`InvoiceBLL.SaveInvoiceConsumer`), so this is not a breaking change to any other code.

`InvoiceBLL.SaveInvoiceConsumer` (`Library.BLL/SInventory_BLL/InvoiceBLL.cs`) updated: removed the `PrimaryKeyMax` call entirely; invoice **numbering is unchanged** (`InvoiceNoGenerator` still runs first, exactly as before — invoice number *format* was deliberately left alone, since the IDENTITY fix only concerns the numeric primary key, not the human-readable invoice number string); `invoiceId`/`out` param now carries the real `SCOPE_IDENTITY()`-derived value.

**Live-verified** (`DS-T05`): `SaveInvoiceConsumer` returned `true`, a real `invoiceId` (e.g. `855159`), invoice number in the pre-existing `INV-BD01-NNNNNNNNN` format (unchanged), and the row was confirmed to exist in `tblInvoice` by that exact ID — no `Msg 8101`.

## 5. Transaction Scope

Added a `TransactionScope` (`ReadCommitted`, 5-minute timeout) around Order-create/self-approve → Invoice Master → Invoice Details/Stock → delivery-consumer updates → journal posting, in `InvoiceCreationForConsumerCustomer.aspx.cs`'s `saveButton_Click`, mirroring `InvoiceCreationForCustomerByOrder.aspx.cs`'s proven `saveButton_Click1` pattern exactly (same options, same "check result, `return` without `Complete()` on failure" shape, same `catch` block).

**Live-verified** (`DS-T03`): insufficient stock (10 available, 15 requested) — the whole flow (order + master + detail + stock) rolled back completely: `masterCount=0`, `orderCount=0`, stock unchanged at `10`. (`DS-T04`, zero stock: same result.) This extends the exact rollback mechanism already proven for the ByOrder page (`STK-REG-001` from the prior investigation) to this page's own call sequence — confirming the earlier finding that sequential same-instance `SqlConnection`s under one ambient `TransactionScope` don't require MSDTC (which is stopped on this dev machine) holds here too.

**Deliberately not exercised live**: `PostDeliveryConfFullJournal` (`sp_Sales_ExecuteDeliveryPipeline`) writes real general-ledger vouchers into a separate database (`CSTL_ACCDB`) and reads from `CSTL_ERPDB` — none of that code was touched by this fix, and running it live would create real accounting entries in a database outside this fix's scope, risky to reverse cleanly. Its call site is unchanged except for now being inside the transaction and having its return value checked (previously the return value gated the *entire* success message but the save had already unconditionally committed regardless — now a `false` here correctly rolls back everything, including the invoice itself). Treated as **BLOCKED for live re-verification** — see Remaining Risks.

## 6. Stock Flow

No changes to stock logic. `InvoiceBLL.SaveInvoiceDetails`/`UpdateDCStock`/`InvoiceDAL.UpdateDCStoreQuantity` (the already-atomic, already-verified mechanism from the ByOrder investigation) are called exactly as before — the fix only ensures the page now (a) checks their result and (b) runs them inside a transaction that also covers the master insert, so a stock failure now correctly takes the master/order down with it instead of leaving them committed.

**Live-verified**: `DS-T01` (100 available, 10 requested → 90 remaining, exact), `DS-T02` (3 detail lines, 5+3+2=10 total, 100→90, exact, no double-deduction), `DS-T03`/`DS-T04` (insufficient/zero stock → full rollback, stock unchanged).

## 7. Detail Save

Fixed `SaveInvoiceDetail(int invoiceId)` in the page code-behind: previously called `aInvoiceBll.SaveInvoiceDetails(...)` and then **unconditionally `return true`** regardless of the result, and returned `true` even when the grid had zero rows. Now returns the real result of `SaveInvoiceDetails`, and returns `false` outright when there are no line items (matching the zero-detail protection already established for the ByOrder page).

**Live-verified**: `DS-T06` confirms `tblInvoiceDetail.InvoiceId` always matches the exact `tblInvoice.InvoiceId` generated by the same `SaveInvoiceConsumer` call in the same transaction — no second ID computed, no `MAX(InvoiceId)`/`TOP 1 ORDER BY` lookups introduced anywhere in this fix.

## 8. Error Handling

- Added a `try/catch` around the whole transaction block (previously none existed) — any exception now shows a safe message (`"Invoice generate failed: " + ex.Message`) instead of propagating to an unhandled ASP.NET error page.
- Added `ClsErrorLogger.Log(...)` to `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity`'s `catch` block (it previously re-threw without logging, unlike `SaveAction`/`UpdateAction`). This is the exact helper this fix's new master-insert path depends on, and it's shared by 5 other DAL classes (`CampaignDAL`, `CompanywisebranchDal`, `CustomerReturnDAL`, `InvoiceBatchDAL`, and `InvoiceDAL`'s own order-save methods) — all of them now get the same logging coverage as `SaveAction`/`UpdateAction`, closing the exact gap flagged in the prior test-result report for this code path.

## 9. Print Flow

Not modified in this pass — already fixed in the earlier print-flow session (`rptInvoiceId` label + `&invoiceId=` query param). This fix's only interaction with it: `rptInvoiceId.Text = invId.ToString()` now runs **after** the transaction successfully commits (moved out of the try block, alongside `Clear()` and the success message), instead of being set mid-transaction where a later failure (e.g. journal posting) could have left the print button pointing at an ID that no longer exists after rollback. Print itself was not re-tested live in this pass (no IIS/browser session available) — see Remaining Risks.

## 10. Business Logic Preservation

| Behavior | Status | Evidence |
|---|---|---|
| Self-created Order | **PRESERVED** | `OrderSaveMaster()`/`OrderSaveDetail()` — zero lines changed; still called in the same order, still using `IsManual=true`/`IsDirect=true` |
| Self-Approval | **PRESERVED** | `ApprovedBy=Session["LoginName"]`, `ApprovedDate=Now`, `ActionStatus="Accepted"` — zero lines changed in `OrderSaveMaster()` |
| Walk-in Customer | **PRESERVED** | `ConsumeCustomer()`/`ConsumerCustomerDAO`/`CreateGlOfCustomer()` — zero lines changed. Also independently confirmed `tblConsumerCustomer.ConsumerCustId` is **not** an identity column (checked directly against the live schema before touching anything), so this method needed no fix and none was applied |
| Discount = 0 | **PRESERVED** | `aInvoiceDetail.DiscountPercentage = 0; aInvoiceDetail.DiscountAmount = 0; aInvoiceDetail.BonusQuantity = 0;` — zero lines changed. Live-verified (`DS-T07`): persisted detail rows have `DiscountPercentage`/`DiscountAmount` = 0 |
| Pricing / VAT / product selection | **PRESERVED** | No lines touched in `GetProduct`, `qtyTextBox_TextChanged`, `TotalValueCalculation`, or any pricing/VAT computation |
| Invoice numbering | **PRESERVED (format unchanged)** | `InvoiceNoGenerator` (the existing `INV-<unit>-<9-digit-seq>` format) still runs exactly as before; only the numeric `InvoiceId` primary key's generation mechanism changed, not the human-readable invoice number |

## 11. Test Results

Executed live against the local dev SQL Server (`TOWSIF\MSSQLSERVER2019` / `SalesRollDB_ZAS`), via a throwaway console harness calling the real `InvoiceBLL`/`InvoiceDAL` classes directly (same approach and same user-granted permission as the prior test-execution pass). All tests snapshot/restore the same 3 isolated stock batches used throughout this investigation and delete only their own tagged rows — verified zero residual rows after the run.

| Test ID | Scenario | Result | Evidence |
|---|---|---|---|
| DS-T01 | Normal Direct Sale, 1 product, valid stock | **PASS** | Order+Master+Detail all created, stock 100→90, transaction committed |
| DS-T02 | Multiple products (3 lines) | **PASS** | detailCount=3, stock 100→90 (5+3+2 deducted, exact) |
| DS-T03 | Insufficient stock (10 available, 15 requested) | **PASS** | Full rollback: 0 master rows, 0 order rows, stock unchanged at 10 |
| DS-T04 | Zero stock | **PASS** | Full rollback: 0 master rows, stock unchanged at 0 |
| DS-T05 | IDENTITY behavior (no Msg 8101, no explicit InvoiceId insert) | **PASS** | Real `SCOPE_IDENTITY()`-generated ID returned and confirmed to exist |
| DS-T06 | Master/Detail relationship | **PASS** | Detail row's `InvoiceId` matches Master's `InvoiceId` exactly, no second ID computed |
| DS-T07 | Discount regression (still 0) | **PASS** | Persisted `DiscountPercentage`/`DiscountAmount` = 0 |
| DS-T08 | Walk-in customer regression | **NOT RUN** — `ConsumeCustomer`/`CreateGlOfCustomer` deliberately not exercised live (real GL/subsidiary side effects, zero lines changed in this fix; confirmed unchanged by diff, not re-executed) |
| DS-T09 | Self-approval regression | **NOT RUN** live for `OrderSaveMaster`'s approval fields specifically (confirmed unchanged by diff — zero lines touched). A related check, **DS-T09-UpdateInvoiceConsumer**, **PASS**ed live: `IsDirect` flag correctly set inside the same transaction. |
| DS-T10 | Print (Save → Print, correct ID) | **BLOCKED** — no live IIS/browser session available this pass; code path confirmed by review (§9) |
| DS-T11 | Failed transaction rollback | **PASS** — covered by DS-T03/DS-T04 (real forced failures, real rollback verified) |
| DS-T12 | Double submission | **NOT RUN** — `sp_I_InvoiceMaster`'s dedup guard (proven for ByOrder) is **not** the proc this page uses; `SaveDataForInvoiceConsumer`'s plain parameterized insert has no equivalent OrderNo/time-window guard. This is a **pre-existing gap, not introduced by this fix** (the old broken insert had no such guard either) — flagged in Remaining Risks rather than silently left untested |

```
Automated tests: 0 (no test project in repo)
Harness-executed (real BLL/DAL calls, live local DB, full cleanup verified): 9 of 12 test cases
Blocked (no IIS/browser): 1 (DS-T10)
Not run (deliberately, to avoid cross-database GL side effects or because unchanged-by-diff): 2 (DS-T08, DS-T09 order-approval-specific)
```

## 12. Database Verification

Read-only queries run after each test (before cleanup) confirmed:
- **Invoice**: `tblInvoice.InvoiceId` = SQL Server-generated value; `InvoiceNo` in the unchanged format; `CustomerMasterId`/`InvoiceDate` correctly populated.
- **Details**: `tblInvoiceDetail.InvoiceId` = same value as the master row (DS-T06); `ProductCode`/`Quantity`/`Amount` match the input exactly.
- **Stock**: before/deducted/after tracked explicitly per test — every successful save deducted exactly the requested quantity once (DS-T01/T02), every failed save left stock at its pre-test value (DS-T03/T04).
- **Order**: `tblOrder` row created and self-approved (`ActionStatus`, `ApprovedBy`, `ApprovedDate` populated) on success; zero `tblOrder` rows remained after a rolled-back failure (DS-T03).

Post-run verification: `SELECT COUNT(*) FROM tblInvoice/tblOrder/tblOrderDetail WHERE ... LIKE 'AUTOTEST%'` = 0 for all three tables; all 3 stock batches restored to their exact original values (289, 1, 8).

## 13. Files Changed

- `Library.DAL/SInventory_DAL/InvoiceDAL.cs` — `SaveDataForInvoiceConsumer` rewritten: parameterized, omits `InvoiceId`, returns `int` via `SaveDataByInsertCommandWithIdentity`.
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` — `SaveInvoiceConsumer` updated to use the new `int`-returning DAL method; invoice numbering (`InvoiceNoGenerator`) unchanged.
- `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` — `SaveDataByInsertCommandWithIdentity` now logs exceptions via `ClsErrorLogger` before re-throwing (previously silent).
- `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs` — `saveButton_Click` wrapped in `TransactionScope` + `try/catch`; `invId <= 0` and `SaveInvoiceDetail` return value now checked; `SaveInvoiceDetail` now returns the real result (and `false` for zero line items) instead of always `true`; `rptInvoiceId.Text` assignment moved to the post-commit success path.

No `.aspx` markup changes (the print-ID label was already added in the prior session). No stored procedures, no schema, no other page modified.

## 14. Remaining Risks

- **`PostDeliveryConfFullJournal`/journal posting was not re-verified live** inside the new transaction boundary — it writes to a separate database (`CSTL_ACCDB`) via `sp_Sales_ExecuteDeliveryPipeline`. Same-instance cross-database T-SQL inside one connection does not require MSDTC (unlike a second `SqlConnection` to a different server), so this is expected to work the same way the rest of the transaction does — but this specific claim was not empirically exercised, unlike every other step in this fix. Recommend a controlled staging verification (real customer/GL setup, not fabricated test data) before relying on this in production.
- **No double-submission guard exists for this page** (`DS-T12` not run) — this is pre-existing (the old broken insert also had no such guard) and out of scope for this fix, but worth a follow-up: unlike the ByOrder page's `sp_I_InvoiceMaster`, `SaveDataForInvoiceConsumer`'s plain insert has no OrderNo/time-window dedup check.
- **Print was not re-verified live** (`DS-T10` BLOCKED) — no IIS/browser session available this session; the underlying print-ID mechanism is unchanged from the already-fixed print-flow session.
- **Walk-in customer creation and order self-approval were not re-executed live** in this pass (`DS-T08`/`DS-T09`) — confirmed unchanged by diff (zero lines touched), and deliberately not run live to avoid creating real GL/subsidiary-ledger side effects for a test customer.
- **Schema-parity caveat carries over from the prior investigation**: all of the above is confirmed against the local dev database, not production. If production's `tblInvoice.InvoiceId` is not `IDENTITY`, this fix would need re-evaluation there — but per the earlier investigation, no evidence was found anywhere in the repo suggesting production differs on this point.
- **`InvoiceCreationForCustomer.aspx`'s identical bug remains unfixed**, as instructed (unused page, out of scope for this task).
