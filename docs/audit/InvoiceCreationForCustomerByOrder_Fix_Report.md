# InvoiceCreationForCustomerByOrder — Fix Report

## 1. Issues Fixed

| ID      | Issue                  | Fixed? | Files Changed | Summary |
| ------- | ---------------------- | ------ | ------------- | ------- |
| INV-001 | Master without Details | YES | `InvoiceCreationForCustomerByOrder.aspx.cs`, `InvoiceBLL.cs` | `SaveInvoiceDetail`/`SaveInvoiceDetails` now return a real success/failure result (empty detail list, or any failed insert/stock deduction, returns `false`), which the existing `TransactionScope` in `saveButton_Click1` rolls back on. |
| STK-001 | Unsafe Stock Update    | YES | `InvoiceDAL.cs` | `UpdateDCStoreQuantity` changed from an absolute `SET StockQty = <value>` to an atomic `SET StockQty = StockQty - @qty WHERE ... AND StockQty >= @qty`, returning whether a row was actually affected. |
| STK-002 | Insufficient Stock     | YES | `InvoiceBLL.cs` | `UpdateDCStock` now returns `false` if the batch loop runs out of stock/rows before the requested quantity is fully allocated, or if any atomic deduction is rejected by the database. |
| PRT-002 | Blank Print            | YES | `InvoiceReportViewer.aspx.cs` + 5 caller `.aspx.cs` files | Missing/invalid `invoiceId` now shows an explicit error instead of silently defaulting to `"0"`. The 5 callers whose grid already carried `InvoiceId` as a DataKey now pass it. |
| ERR-001 | Swallowed Exceptions   | YES | `ClsCommonInternalDAL.cs`, new `ClsErrorLogger.cs` | `SaveAction`/`UpdateAction` now log the operation, stored procedure name, and full exception to `App_Data\DatabaseErrors.log` before returning the existing failure sentinel. |

```text
ORD-001 — NOT FIXED BY BUSINESS DECISION (remaining-order-quantity validation intentionally not implemented)
```

## 2. Before vs After

**Before:** `saveButton_Click1` already wrapped Master + Details + Stock + OrderStatus in a `TransactionScope`, but `SaveInvoiceDetail()` and `InvoiceBLL.SaveInvoiceDetails()` both unconditionally `return true`, and the raw stock update was an absolute overwrite computed from a stock value read earlier in the same request. A row silently dropped for zero stock, an empty grid, or a lost race on `tblDCStore` all still produced a committed Invoice Master with zero or short details and a "success" message.

**After:** Every step that touches the database (`SaveDataForInvoiceDetails`, `UpdateDCStoreQuantity`) now has its real success/failure propagated up through `InvoiceDetailInsertBLL` → `UpdateDCStock` → `SaveInvoiceDetails` → `SaveInvoiceDetail` → `saveButton_Click1`. Any `false` return (or exception) reaches the existing `TransactionScope` before `scope.Complete()` is called, so Master, Details, Stock and OrderStatus all roll back together, and the user sees a failure message instead of "Proforma Invoice Created Successsfully!!".

## 3. Transaction Flow

Unchanged structurally — the `TransactionScope` (`ReadCommitted`, 5-minute timeout) already wrapped:

```
SaveInvoiceMater → SaveInvoiceDetail → UpdateInvoiceStatus → scope.Complete()
```

What changed is that `SaveInvoiceDetail` can now actually return `false`, which was previously dead code. A `false` (or any exception) causes an early `return` inside the `using` block without calling `scope.Complete()`, so ADO.NET/DTC rolls back everything performed inside the scope, including any `tblInvoiceDetail` rows and `tblDCStore` decrements already issued in that request.

## 4. Stock Flow

- `InvoiceBLL.UpdateDCStock` still walks batches FIFO exactly as before (bonus-qty splitting logic untouched).
- For each batch it allocates from, it now calls `InvoiceDAL.UpdateDCStoreQuantity(dCStoreId, deductQty)` where `deductQty` is the amount being taken from that specific batch (previously the method took the *new absolute stock value* and blindly overwrote it).
- The SQL is now `UPDATE tblDCStore SET StockQty = StockQty - @qty WHERE DCStoreId = @id AND StockQty >= @qty`, executed as a single statement — no separate read-then-write. If a concurrent request already consumed the stock, the `WHERE` clause fails to match, `ExecuteNonQuery` returns 0 rows affected, and the method returns `false`.
- `UpdateDCStock` returns `false` when: (a) no batch rows exist for the product, (b) the loop exhausts every batch without ever reaching the "fully satisfied" branch (total requested quantity exceeds total available stock across all batches), or (c) any individual insert/deduction fails.
- Any `false` propagates through `SaveInvoiceDetails` → `SaveInvoiceDetail`, failing the whole invoice inside the existing transaction (no partial commit, no stock left in a decremented-but-uninvoiced state).

## 5. Print Flow

`InvoiceReportViewer.aspx.cs` `Page_Init` no longer defaults a missing `invoiceId` to `"0"`. It now validates `invoiceId` is a parseable positive integer; if not, it writes an inline error message and completes the request without touching Crystal Reports.

| Caller | Before | After |
|---|---|---|
| `CustomerDueReport.aspx.cs:309` | Passes `InvNo` + `invoiceId` | unchanged |
| `GroupInvoiceEntry.aspx.cs:183` | Passes `InvNo` + `invoiceId` | unchanged |
| `GroupInvoiceList.aspx.cs:189` | Passes `InvNo` + `invoiceId` | unchanged |
| `InvoiceReport.aspx.cs:262` | Passes `InvNo` + `invoiceId` | unchanged |
| `InvoiceCreationForCustomerByOrder.aspx.cs:1821` | Passes `InvNo` + `invoiceId` | unchanged |
| `BatchWiseDeliveryTopSheet.aspx.cs:105` | `InvNo` only → blank report | now also passes `invoiceId` from existing `InvoiceId` DataKey |
| `BatchWiseDelReturnSummery.aspx.cs:93` | `InvNo` only → blank report | now also passes `invoiceId` from existing `InvoiceId` DataKey |
| `BatchWiseTopSheet.aspx.cs:106` | `InvNo` only → blank report | now also passes `invoiceId` from existing `InvoiceId` DataKey |
| `ReturnOnDeliveryReport.aspx.cs:90` | `InvNo` only → blank report | now also passes `invoiceId` from existing `InvoiceId` DataKey |
| `TopSheetGenerate.aspx.cs:109` | `InvNo` only → blank report | now also passes `invoiceId` from existing `InvoiceId` DataKey |
| `InvoiceCreationForConsumerCustomer.aspx.cs:726` | `InvNo` only → blank report | now also passes `invoiceId` from new `rptInvoiceId` Label (see PRT-002 FINAL STATUS) |
| `InvoiceCreationForCustomer.aspx.cs:549` | `InvNo` only → blank report | same as above |
| `ReturnInvoiceCreationForCustomer.aspx.cs:905` | `InvNo` only → blank report | same as above |

## 6. Error Handling

`ClsCommonInternalDAL.SaveAction` and `UpdateAction` still return their existing failure sentinel (`0` / `false`) on any exception — callers' behavior is unchanged — but the exception (operation, stored procedure name, message, stack trace, timestamp) is now written to `App_Data\DatabaseErrors.log` via a new minimal `ClsErrorLogger`. No existing logging framework was present in the repo, so this is a small, self-contained file logger (logging failures are themselves swallowed so a broken log can never mask or crash the original database failure). No SQL details are shown to end users; the invoice page still only shows the generic "Invoice generate failed: ..." message via the outer `catch` in `saveButton_Click1`.

## 7. Regression Test Results

No automated test project exists in this solution. Verified via build + precompilation only (see below); the manual scenarios below need to be exercised in a running environment against a real database before sign-off:

```
Tests Run: 0 (no test project in repo)
Passed: N/A
Failed: N/A
Skipped: All 20 manual regression scenarios listed in the task (normal invoice, zero stock, insufficient stock, partial detail failure, DB exception, double-click, concurrent invoice, all print callers, etc.) — require a live DB/IIS environment to execute and were not run here.
```

## 8. Files Changed

- `Library.DAL/SInventory_DAL/InvoiceDAL.cs` — `UpdateDCStoreQuantity` made atomic, returns `bool`.
- `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` — `SaveAction`/`UpdateAction` log exceptions before swallowing.
- `Library.DAL/InternalCls/ClsErrorLogger.cs` (new) — minimal file-based error logger.
- `Library.DAL/Library.DAL.csproj` — added `ClsErrorLogger.cs` to compile list.
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` — `SaveInvoiceDetails`, `UpdateDCStock`, `InvoiceDetailInsertBLL` now propagate real success/failure and enforce stock sufficiency.
- `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` — `SaveInvoiceDetail` returns real success/failure instead of always `true`.
- `Solution.Web/SInventory_RPTVIEW/InvoiceReportViewer.aspx.cs` — rejects missing/invalid `invoiceId` instead of defaulting to `"0"`.
- `Solution.Web/SInventory_UI/BatchWiseDeliveryTopSheet.aspx.cs`, `BatchWiseDelReturnSummery.aspx.cs`, `BatchWiseTopSheet.aspx.cs`, `ReturnOnDeliveryReport.aspx.cs`, `TopSheetGenerate.aspx.cs` — pass `invoiceId` (already available as a grid DataKey) to `InvoiceReportViewer.aspx`.

## 9. Database/SP Changes

No stored procedures were modified. `UpdateDCStoreQuantity` builds inline SQL through the existing `ExecuteAllSqlQueryByStoreProcedure` generic-query mechanism (same pattern the method already used) — only the query text changed from an absolute `SET` to a conditional decrement. No schema change, no new SP, no signature change visible to other modules (only the two existing callers, both in `InvoiceBLL.UpdateDCStock`, needed updating — confirmed via full-repo grep that no other code calls `UpdateDCStoreQuantity`).

## PRT-002 FINAL STATUS

The 3 remaining callers flagged in section 10 as "still lack a stored invoice ID" are now wired up, following the exact pattern already used by the working reference page (`InvoiceCreationForCustomerByOrder.aspx`): a hidden `asp:Label ID="rptInvoiceId" runat="server" Visible="False"` next to `invTextBox`, set to the real generated `InvoiceId` right after save (before `Clear()` resets the form), and read back in `printButton_Click` to append `&invoiceId=...` to the `InvoiceReportViewer.aspx` URL.

No second Invoice ID is generated — each page's existing `SaveInvoice()`/`SaveInvoiceConsumer()`/`SaveReturnInvoice()` `out invoiceId` (the same value already used for `SaveInvoiceDetail`) is reused. `ReturnInvoiceCreationForCustomer.aspx` already had a `HiddenField` named `invoiceHiddenField`, but it holds the *source* invoice being returned against — it was left untouched and a new `rptInvoiceId` Label was added instead, so its existing meaning isn't disturbed.

| Page | Invoice ID Available After Save | Print Parameter Correct | Print Result |
| ---- | -------------------------------- | ------------------------ | ------------- |
| InvoiceCreationForCustomer | YES — `rptInvoiceId.Text` set from `SaveInvoice()`'s `out invoiceId` | YES — `&invoiceId=` appended in `printButton_Click` | Correct invoice (pending live manual verification) |
| InvoiceCreationForConsumerCustomer | YES — `rptInvoiceId.Text` set from `SaveInvoiceConsumer()`'s `out invoiceId` | YES — `&invoiceId=` appended in `printButton_Click` | Correct invoice (pending live manual verification) |
| ReturnInvoiceCreationForCustomer | YES — `rptInvoiceId.Text` set from `SaveReturnInvoice()`'s `out invoiceId` (the new Return Invoice ID, not the source `invoiceHiddenField`) | YES — `&invoiceId=` appended in `printButton_Click` | Correct invoice (pending live manual verification) |

Files changed for this follow-up (6, no commit made):
- `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx` / `.aspx.cs`
- `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` / `.aspx.cs`
- `Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx` / `.aspx.cs`

Verification performed: full-site `aspnet_compiler` precompile (exit code 0, no `error CS...` lines — only pre-existing warnings unrelated to these files). No automated tests exist in this repo (unchanged from section 7). Live click-through (Save → Print → correct report) was not performed — no running IIS/SQL Server environment available in this session — so the "Print Result" cells above reflect code-path verification only, not an observed report render.

## 10. Remaining Risks

- **3 print callers previously lacked a stored invoice ID** (`InvoiceCreationForConsumerCustomer.aspx.cs`, `InvoiceCreationForCustomer.aspx.cs`, `ReturnInvoiceCreationForCustomer.aspx.cs`) — **now resolved**, see "PRT-002 FINAL STATUS" below.
- **No automated tests exist** for this module; the 20 manual regression scenarios in the task need to be run against a real SQL Server/IIS environment.
- **`PrecompiledWeb` folder** mirrors `Solution.Web` as a build artifact and was not updated — if that folder is what's actually deployed, it needs to be regenerated from source before deployment.
- **Legacy dead code**: `saveButton_Click`/`SaveInvoice()` (no `TransactionScope`, manual `DeleteInvoice` compensation) remains unreferenced by any control and was left untouched per "smallest safe change" — flagged as pre-existing risk if ever rewired.
- **ORD-001 explicitly not implemented** per business decision — verified no `InvoiceQty <= RemainingOrderQty` check or equivalent was added anywhere in this change set.
