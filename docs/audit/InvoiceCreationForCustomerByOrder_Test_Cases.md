# Invoice / Stock / Print — Test Case Catalog

Scope: `InvoiceCreationForCustomerByOrder.aspx`, `InvoiceCreationForCustomer.aspx`, `InvoiceCreationForConsumerCustomer.aspx`, `ReturnInvoiceCreationForCustomer.aspx`, `InvoiceReportViewer.aspx`, `InvoiceDAL.UpdateDCStoreQuantity`, `ClsCommonInternalDAL.SaveAction`/`UpdateAction`.

**ORD-001 excluded per business decision** — no test in this catalog asserts `InvoiceQty <= RemainingQty` or any Order-remaining-quantity restriction, and none should be added.

Statuses: `PASS` / `FAIL` / `BLOCKED` / `NOT RUN`. See `InvoiceCreationForCustomerByOrder_Test_Result.md` for actual execution results and evidence.

## How these were executed

No test project exists in this repo (confirmed: `Solution.sln` contains only `Library.BLL`/`Library.DAL`/`Library.DAO`/`Library.CrystalReports`/`Solution.Web` — no `*Test*.csproj`, no NUnit/xUnit/MSTest/Selenium/Playwright packages anywhere in `packages.config`). With the user's explicit permission, a throwaway console harness (`Harness.exe`, not part of this repo) was compiled against the built `Library.BLL.dll`/`Library.DAL.dll`/`Library.DAO.dll` and run against the local dev SQL Server instance (`TOWSIF\MSSQLSERVER2019` / `SalesRollDB_ZAS` — confirmed local, not remote/production; matches the machine this session runs on) to call the real BLL/DAL methods directly. All DB-touching tests snapshot and restore exact `StockQty` values and delete only their own tagged test rows — verified zero residual rows after the run. Tests requiring a running IIS site + browser interaction (Print button clicks, full-page postback flows) were **not executable** — no IIS binding is configured for this site in this session and no browser-automation tooling exists in the repo — and are marked `BLOCKED` in the results.

---

## Group A — Invoice Master/Detail

| Test ID | Scenario | Precondition | Test Data | Steps | Expected Result |
|---|---|---|---|---|---|
| INV-T01-master | Normal invoice via plain `SaveInvoice` (legacy pages) | Real `SalesRollDB_ZAS`, real CustomerMasterId/ComUnitId | 1 detail line, valid customer/comunit | Call `InvoiceBLL.SaveInvoice` | Master=1 record, Save=SUCCESS |
| INV-T01-byorder | Normal invoice via `SaveInvoiceMater` (ByOrder page's stored-proc pattern) | Same | Same | Call `InvoiceDAL.SaveInvoiceMater` | Returns InvoiceId > 0 |
| INV-T02 | Multiple details (3+) | Sufficient stock across products | 3 products, distinct qty/price | Call `SaveInvoiceDetails` with 3-item list | Correct detail count, quantities, amounts, total |
| INV-T03 | Zero detail | — | Empty `List<InvoiceDetail>` | Call `SaveInvoiceDetails(emptyList, comUnitId)` | Save=FAIL, no orphan master created by this call |
| INV-T04 | Zero-stock detail | Product batch StockQty=0 | 1 detail, qty=5 | Call `SaveInvoiceDetails` | Save=FAIL, no master/detail/stock change |
| INV-T05 | Detail insertion failure (controlled) | — | Malformed/oversized detail data | Force a detail insert failure | Master/Details/Stock all unaffected (no built-in rollback exists outside `TransactionScope`, see INV/STK-T08) |
| INV-T06 | Partial detail failure (A ok, B ok, C fails) | — | 3 details, C references insufficient stock | Call `SaveInvoiceDetails` with all 3 | Expected: entire transaction rolls back. **Actually**: `SaveInvoiceDetails` has no internal transaction — see STK-T08 |
| INV-T07 | Double-click Save | — | Same OrderNo submitted twice, back-to-back | Call `SaveInvoiceMater` twice with identical OrderNo | No duplicate invoice; proc's own `ProcessTirm`/`OrderNo` guard (ByOrder page only) rejects the second call |
| INV-T08 | Browser retry / repeat save | — | — | N/A without a browser/HTTP client | Idempotent per existing business behavior |

## Group B — Stock

| Test ID | Scenario | Test Data | Expected Result |
|---|---|---|---|
| STK-T01 | Exact stock | Available=10, Invoice=10 | Save=SUCCESS, Remaining=0 |
| STK-T02 | Less than available | Available=100, Invoice=30 | Save=SUCCESS, Remaining=70 |
| STK-T03 | Insufficient stock | Available=10, Invoice=15 | Save=FAIL, stock remains 10, no incomplete invoice |
| STK-T04 | Zero stock | Available=0, Invoice>0 | Save=FAIL, stock remains 0 |
| STK-T05 | Negative quantity | Invoice Qty=-5 | Save=FAIL |
| STK-T06 | Zero quantity | Invoice Qty=0 | Save=FAIL |
| STK-T07 | Single deduction | Before=100, Invoice=20 | After=80, not 60 |
| STK-T08 | Failed invoice → stock rollback | Multi-detail invoice where a later detail fails | Earlier successful detail's stock deduction is restored |
| STK-DAL-atomic | `InvoiceDAL.UpdateDCStoreQuantity` atomicity | Decrement exact amount, then decrement again from 0 | First succeeds to 0; second is rejected (WHERE clause), never negative |

## Group C — Concurrency

| Test ID | Scenario | Test Data | Expected Result |
|---|---|---|---|
| STK-T09 | Two concurrent invoices, same batch | Stock=10, SessionA=8, SessionB=8 (parallel) | Exactly one succeeds; stock never goes negative |
| STK-T10 | Many concurrent invoices, same product/store | Stock=50, 10 parallel requests of 8 each | Successes × 8 == stock deducted; stock never negative |

## Group D — Print

| Test ID | Scenario | Expected Result |
|---|---|---|
| PRT-T01 | `InvoiceCreationForCustomer` Save → Print | Correct invoice, correct Invoice ID, details visible |
| PRT-T02 | `InvoiceCreationForConsumerCustomer` Save → Print | Correct invoice |
| PRT-T03 | `ReturnInvoiceCreationForCustomer` create → Print | NEW return invoice printed, not the source invoice |
| PRT-T04 | Invalid Invoice ID in viewer URL | Clear error, not a blank report |
| PRT-T05 | Missing Invoice ID in viewer URL | Clear validation/error, no blank invoice |
| PRT-T06 | Multiple-detail invoice print | All details appear in the report |

## Group E — Error Handling

| Test ID | Scenario | Expected Result |
|---|---|---|
| ERR-T01 | SQL exception during a `SaveAction`-routed insert | Operation FAILED, exception logged, safe user message, no false success |
| ERR-T02 | Database unavailable | No false success, no partial data, useful error/log |
| ERR-T03 | Detail failure not silently swallowed | Exception surfaces / is logged, not lost |

## Group F — Save Duplication

Covered by INV-T07/INV-T08 above.

## Group G — Data Integrity

| Test ID | Scenario | Expected Result |
|---|---|---|
| DB-T01 | Orphan Invoice Masters (no details) | 0 unexpected orphans (post-INV-001 baseline) |
| DB-T02 | Orphan Invoice Details (no master) | 0 orphans |
| DB-T03 | Stock transaction mismatch | Invoice Qty = Stock Deducted Qty where applicable |
| DB-T04 | Duplicate stock deduction | Exactly one valid stock movement per invoice |

## Group H — Regression

Customer selection, order selection, product loading, pricing, discount, VAT/tax, invoice total, invoice number, invoice date, existing stock display, Save, Print — verify unchanged. **No RemainingQty restriction added or tested (ORD-001 excluded).**
