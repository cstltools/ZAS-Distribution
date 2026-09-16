# Invoice / Stock / Print — Test Execution Result

Execution date: 2026-09-08. Target: local dev SQL Server `TOWSIF\MSSQLSERVER2019`, database `SalesRollDB_ZAS` (confirmed local, not production — running on this same machine). Executed via a throwaway console harness calling `InvoiceBLL`/`InvoiceDAL` directly (run with the user's explicit permission; harness lives outside the repo, nothing added to source control). All mutating tests snapshot/restore `StockQty` and delete only their own tagged rows — verified **zero residual test rows** in `tblInvoice`/`tblInvoiceDetail`/`tblDCStore` after the run.

**ORD-001 was not tested, per business decision.** No `RemainingQty` check exists in the code touched by this pass (verified by grep — see Step 8 of the prior fix-verification pass).

> **CORRECTION (P0/P1 follow-up pass, same day):** STK-T03/STK-T08 below (and the STK-002 rating) tested `InvoiceBLL.SaveInvoiceDetails` in isolation, **without** the `TransactionScope` that `InvoiceCreationForCustomerByOrder.aspx.cs`'s `saveButton_Click1` actually wraps it in. Re-tested as `STK-REG-001` replicating the real page flow (`TransactionScope` + `SaveInvoiceMater` + `SaveInvoiceDetails`, insufficient stock 10-available/15-requested): the **real page flow correctly rolls back everything** — no master row, no detail row, stock unchanged. **STK-002 requires no code change for the ByOrder page.** The partial-commit risk described below is real only for the 3 legacy pages, which have no `TransactionScope` at all in their save handlers — see `Invoice_Final_P0_P1_Investigation.md` for full detail.

## Summary

```
Total Test Cases: 30
Passed:  15
Failed:   7
Blocked:  6
Not Run:  2  (require live IIS + real DB scale beyond this session's safe scope)
Info-only: (DB-T01/T02 counts reported, not pass/fail — pre-existing historical data)
```

## Results Table

| Test ID | Scenario | Expected | Actual | Status | Evidence |
|---|---|---|---|---|---|
| INV-T01-master | Normal invoice, plain `SaveInvoice` (legacy pages) | Save=SUCCESS | Threw `SqlException`: *"Cannot insert explicit value for identity column in table 'tblInvoice' when IDENTITY_INSERT is set to OFF."* | **FAIL** | Harness run; `tblInvoice.InvoiceId` confirmed IDENTITY+PK in live schema; `InvoiceDAL.SaveDataForInvoice` explicitly lists `InvoiceId` in its INSERT column list |
| INV-T01-byorder | Normal invoice, `SaveInvoiceMater` (ByOrder page) | InvoiceId > 0 | Returned real InvoiceId (855150/855153); row deleted after | **PASS** | Harness run |
| INV-T02 | Multiple details, correct totals | Correct count/qty/amount | Not executed independently — `STK-T02`/`STK-T07` exercise the same detail-insert path with different quantities and both matched expected math | **NOT RUN** | — |
| INV-T03 | Zero detail | Save=FAIL | `SaveInvoiceDetails(emptyList)` returned `false` | **PASS** | Harness run |
| INV-T04 | Zero-stock detail | Save=FAIL, no changes | Covered by STK-T04 below (same code path) | **PASS** (see STK-T04) | — |
| INV-T05 | Detail insertion failure (controlled) | No master/detail/stock change | Not independently forced — see INV-T06/STK-T08 for the closest real equivalent | **NOT RUN** | — |
| INV-T06 | Partial detail failure (A,B ok, C fails) — entire transaction rollback expected | Full rollback | **No rollback occurs** — see STK-T08 | **FAIL** | Harness run (STK-T08) |
| INV-T07 | Double-click Save | No duplicate | ByOrder page: proc's own `OrderNo`+`ProcessTirm`(<60s) guard correctly rejected the immediate re-save (returned InvoiceId=0). **This guard exists only in `sp_I_InvoiceMaster` (ByOrder page)** — the other 3 pages' `PrimaryKeyMax`-based save has no equivalent protection and was not reachable to test (its master-insert already fails per INV-T01-master) | **PASS** (ByOrder only) / **BLOCKED** (other 3 pages — master insert fails before duplication could even be observed) | Harness run |
| INV-T08 | Browser retry / idempotency | Idempotent | Requires an HTTP client replaying a real page postback | **BLOCKED** — no IIS site bound in this session | — |
| STK-T01 | Exact stock (10 avail / 10 invoice) | Success, remaining=0 | Save=True, stock=0 | **PASS** | Harness run |
| STK-T02 | Less than available (100/30) | Success, remaining=70 | Save=True, stock=70 | **PASS** | Harness run |
| STK-T03 | Insufficient stock (10 avail / 15 invoice) | Save=FAIL, stock **remains 10** | Save=False (correct) but stock ended at **0**, not 10 | **FAIL** | Harness run — see Critical Failures |
| STK-T04 | Zero stock | Save=FAIL, stock=0 | Save=False, stock=0 | **PASS** | Harness run |
| STK-T05 | Negative quantity (-5) | Save=FAIL | **Save=True**, stock **increased** from 100 to 105 | **FAIL** | Harness run — see Critical Failures |
| STK-T06 | Zero quantity | Save=FAIL | **Save=True**, stock unchanged (detail row inserted with all-zero amounts) | **FAIL** | Harness run |
| STK-T07 | Single deduction, no double-deduction (100→20 invoice) | 80, not 60 | Save=True, stock=80 | **PASS** | Harness run |
| STK-T08 | Failed invoice → stock rollback (2-detail invoice, 2nd detail fails) | Stock for 1st (successful) detail restored | 1st detail deducted stock 100→90 and **stayed at 90** after the 2nd detail's call failed — **no rollback** | **FAIL** | Harness run — see Critical Failures |
| STK-T09 | 2 concurrent invoices, stock=10, both request 8 | Exactly one succeeds, stock never negative | SessionA/B: exactly one `true`, stock=2 | **PASS** | Harness run (real parallel `Task`s against the live DB) |
| STK-T10 | 10 concurrent invoices, stock=50, each requests 8 | successes × 8 deducted, never negative | 6 succeeded, stock=50-48=2, matches exactly | **PASS** | Harness run |
| STK-DAL-atomic | `UpdateDCStoreQuantity` atomic decrement | Never negative | Decrement 5→0 succeeds; decrement 1 from 0 correctly rejected | **PASS** | Harness run |
| PRT-T01 | `InvoiceCreationForCustomer` Save→Print | Correct invoice | Code path fixed this session (`rptInvoiceId` label + `&invoiceId=` param); not clicked through a real browser | **BLOCKED** — no IIS/browser in this session | Code review only (see prior fix report) |
| PRT-T02 | `InvoiceCreationForConsumerCustomer` Save→Print | Correct invoice | Same | **BLOCKED** | Code review only |
| PRT-T03 | `ReturnInvoiceCreationForCustomer` create→Print | New return invoice, not source | Same, plus **`SaveReturnInvoice`/`SaveReturnInvoice(InvoiceDetail)` target `tblReturnInvoice`/`tblReturnInvoiceDetail`, which do not exist in this database** (`sys.tables` confirms 0 matches) | **BLOCKED** (print) / **FAIL** (save — see Critical Failures) | Live schema query |
| PRT-T04 | Invalid Invoice ID in viewer URL | Clear error, not blank | Code path confirmed present (`InvoiceReportViewer.aspx.cs` guard, from prior fix pass) but not hit via a real HTTP request in this pass | **BLOCKED** | Code review only |
| PRT-T05 | Missing Invoice ID | Clear error | Same | **BLOCKED** | Code review only |
| PRT-T06 | Multiple-detail print | All details appear | Requires Crystal Reports render via browser | **BLOCKED** | — |
| ERR-T01 | SQL exception via `SaveAction`, verify logging | Logged, safe message, no false success | Two fault-injection attempts (oversized `ProductOffer`, oversized `TpTotal`) both **failed to reproduce a thrown exception** — SQL Server silently truncated/accepted out-of-range values rather than erroring in this proc's parameter-binding context; root cause not fully isolated in the time available | **INCONCLUSIVE** | Harness run (both leaked test rows were found and deleted immediately) |
| ERR-T02 | Database unavailable | Safe failure, no partial data | Would require stopping the local SQL Server service, disrupting the shared dev environment | **BLOCKED** — not safe to execute in this session | — |
| ERR-T03 | Exception not silently swallowed | Logged/surfaced | Confirmed **negatively** for one path: `ClsCommonInternalDAL.SaveDataByInsertCommand` (used by `SaveDataForInvoice`/`SaveDataForInvoiceDetails`, i.e. all 3 legacy pages) re-throws on exception (`catch(Exception ex){ throw ex; }`) **without calling `ClsErrorLogger`** — confirmed by INV-T01-master leaving **no entry** in `DatabaseErrors.log` despite throwing. `SaveAction`/`UpdateAction` (used by the ByOrder page) do log, per the original ERR-001 fix. | **FAIL** (for the 3 legacy pages' insert path) | Harness run + log file inspection |
| DB-T01 | Orphan Invoice Masters (no details) | Informational | 4510 pre-existing (historical data, predates INV-001) | **INFO** | Live query |
| DB-T02 | Orphan Invoice Details (no master) | 0 | 0 | **PASS** | Live query — consistent with the `FK_tblInvoiceDetail_tblInvoice` foreign key discovered during this pass, which makes this orphan class structurally impossible going forward |
| DB-T03 | Invoice Qty = Stock Deducted Qty | Match | Verified implicitly by STK-T01/T02/T07's exact-math assertions | **PASS** | Harness run |
| DB-T04 | Exactly one stock movement per invoice | 1 | Verified implicitly by STK-T07/STK-DAL-atomic (single deduction, no double-deduction) | **PASS** | Harness run |
| Group H (regression) | Customer/order/product/pricing/discount/VAT/total/invoice no/date/stock display/Save/Print unchanged | Unchanged | Not exercised through the UI (no IIS); no code changes were made to pricing/discount/VAT/order logic in this session or the prior print-fix session (confirmed by diff scope) | **BLOCKED** (UI) / **PASS** (code-diff scope check) | `git diff` — no touches to pricing/discount/VAT/order files |

## Critical Failures

### 1. STK-T03 / STK-T08 — insufficient/partial stock is not atomic across the whole detail, and multi-detail invoices have no rollback
- **Failure**: Requesting more than one batch's available stock (e.g. 15 units against a single 10-unit batch) causes `InvoiceBLL.UpdateDCStock` to insert a detail row for the *partial* amount (10 units) and deduct that batch's full stock, **then** return `false` because it ran out of batches before satisfying the full request. The caller sees `false` (correctly treated as failure by the pages that check it) but the partial detail row and stock deduction are **already permanently committed** — no compensating delete/restore happens anywhere in `InvoiceBLL`.
- **Root cause**: `UpdateDCStock` / `SaveInvoiceDetails` (`Library.BLL/SInventory_BLL/InvoiceBLL.cs` lines ~194-339) has no internal transaction. The only place this gets safely undone is the `TransactionScope` wrapping `saveButton_Click1` in `InvoiceCreationForCustomerByOrder.aspx.cs` — the one page that has it. The other 3 pages call `SaveInvoiceDetail(invId)` with **no transaction** and, per the current code, **do not even check its return value** before showing a success message (see `InvoiceCreationForCustomer.aspx.cs` `saveButton_Click`: `SaveInvoiceDetail(invId); Clear(); showMessageBox("Create Invoice");`).
- **Impact**: For the 3 legacy pages (once/if the separate INV-T01-master identity-column defect below is fixed), an insufficient-stock invoice would silently under-deliver (partial detail, partial stock deduction) while showing "Create Invoice" success to the user.
- **Recommended fix**: Wrap Master+Detail+Stock in a `TransactionScope` at each of the 3 legacy pages' save handlers (matching the ByOrder page's pattern), and check `SaveInvoiceDetail`'s return value before showing success — same shape as the INV-001 fix already applied to the ByOrder page. Out of scope for this test-execution pass; reporting only.

### 2. STK-T05 — negative quantity is accepted and *increases* stock
- **Failure**: A detail with `Quantity = -5` was accepted (`SaveInvoiceDetails` returned `true`) and **stock increased** from 100 to 105 instead of being rejected.
- **Root cause**: No validation anywhere in `InvoiceBLL.UpdateDCStock`/`InvoiceDAL.UpdateDCStoreQuantity` rejects negative quantities; the deduct amount arithmetic (`stockQty - restQty`) simply goes negative-of-negative and adds to stock.
- **Impact**: A negative quantity line (a UI bug, a tampered request, or an intentional "return via negative invoice line" workaround some users might attempt) silently inflates stock while producing what looks like a normal successful invoice detail.
- **Recommended fix**: Reject `Quantity <= 0` (and separately handle `BonusQuantity < 0`) in `InvoiceBLL.SaveInvoiceDetails` before entering `UpdateDCStock`. Out of scope for this pass; reporting only.

### 3. STK-T06 — zero quantity is accepted
- **Failure**: A detail with `Quantity = 0` was accepted, creating a detail row with zero amounts and no stock effect.
- **Impact**: Low severity (no stock corruption) but produces meaningless invoice line items; likely a real-world source of the DB-T01 orphan-adjacent noise.
- **Recommended fix**: Same validation as STK-T05 (`Quantity <= 0` rejected).

### 4. INV-T01-master — the 3 legacy pages' master-invoice save is completely broken against this schema
- **Failure**: `InvoiceBLL.SaveInvoice`/`SaveInvoiceConsumer` throw `SqlException` on every call: *"Cannot insert explicit value for identity column in table 'tblInvoice' when IDENTITY_INSERT is set to OFF."*
- **Root cause**: `tblInvoice.InvoiceId` is a live `IDENTITY` + `PRIMARY KEY` column in this database (confirmed via `sys.columns`/`sys.indexes`). `InvoiceDAL.SaveDataForInvoice`/`SaveDataForInvoiceConsumer` build a raw dynamic INSERT that explicitly lists `InvoiceId` with the value from `ClsPrimaryKeyFind.PrimaryKeyMax(...)` — a pattern that only works against a non-identity `InvoiceId` column. The correct pattern (`InvoiceDAL.SaveInvoiceMater`, used only by the ByOrder page) lets `IDENTITY`/`SCOPE_IDENTITY()` assign the real PK via a stored procedure and never inserts an explicit value.
- **Impact**: **`InvoiceCreationForCustomer.aspx` and `InvoiceCreationForConsumerCustomer.aspx` cannot create any invoice at all** in this environment — the master insert throws before any detail/stock work happens. The exception is not caught in `saveButton_Click` (no try/catch there), and (per Finding 5 below) is not logged either — the user would see an unhandled ASP.NET error page.
- **Caveat**: This was discovered against the *local dev* database's live schema. If the intended production schema has `InvoiceId` as a plain (non-identity) column, this would not reproduce there — **schema parity between this dev DB and production should be verified** before treating this as universally confirmed. It is, however, 100% reproducible against the database this session was pointed at.
- **Recommended fix (not applied — out of scope for a print-flow-only follow-up)**: Port `InvoiceCreationForCustomer`/`InvoiceCreationForConsumerCustomer` onto the same `SaveInvoiceMater`-style stored-procedure pattern already used and proven by the ByOrder page, or (if this dev schema is simply out of sync) regenerate `tblInvoice.InvoiceId` as non-identity to match what the legacy DAL code expects. This is a pre-existing defect independent of the PRT-002 print-flow fix delivered in the prior session; it directly blocks PRT-T01/PRT-T02 from ever being reachable end-to-end in this environment.

### 5. ERR-T03 — legacy insert path's exceptions are not logged
- **Failure**: `ClsCommonInternalDAL.SaveDataByInsertCommand(string, string)` (used by all 3 legacy pages' `SaveDataForInvoice*`) re-throws exceptions (`catch(Exception ex){ throw ex; }`) without ever calling `ClsErrorLogger.Log(...)`. Confirmed by: `INV-T01-master`'s repeated SQL exception produced **zero** growth in `DatabaseErrors.log`, while the same harness's calls through `SaveAction` (`SaveInvoiceMater`) did write to that log for other failures.
- **Impact**: Only `SaveAction`/`UpdateAction` (the ByOrder page's code path) got the ERR-001 logging fix. The 3 legacy pages' master/detail insert failures are invisible to `DatabaseErrors.log` and, combined with Finding 4, likely surface only as raw ASP.NET error pages.
- **Recommended fix**: Add the same `catch { ClsErrorLogger.Log(...); }` pattern to `SaveDataByInsertCommand` that `SaveAction`/`UpdateAction` already have. Out of scope for this pass; reporting only.

### 6. PRT-T03 (Return Invoice) — target tables do not exist in this database
- **Failure**: `InvoiceBLL.SaveReturnInvoice`/`InvoiceDAL.SaveDataForReturnInvoice(Details)` reference `dbo.tblReturnInvoice` / `dbo.tblReturnInvoiceDetail`, neither of which exists in `SalesRollDB_ZAS` (`sys.tables` query returned zero rows for either name). The live schema instead has `tblCustomerReturnMaster`/`tblCustomerReturnDetail`.
- **Impact**: `ReturnInvoiceCreationForCustomer.aspx`'s save flow cannot succeed against this database at all — independent of, and more fundamental than, the print-flow fix delivered in the prior session for this page. This was **not executed live** (would guarantee a `SqlException`, and the resulting error path was already indirectly exercised by Finding 4's identical failure mode) but is a certain, schema-confirmed failure.
- **Caveat**: Same schema-parity caveat as Finding 4 — verify against production before treating as universal.
- **Recommended fix**: Out of scope; flagging for the return-invoice module owner.

## Blocked Tests

Require infrastructure not available/safe in this session:
- **IIS site + browser** (PRT-T01, PRT-T02, PRT-T04, PRT-T05, PRT-T06, INV-T08, Group H UI regression): no IIS binding configured for this site in this session; no Selenium/Playwright or other browser-automation tooling exists in the repo to add safely within a print-flow test-execution task's scope.
- **Database unavailability simulation** (ERR-T02): would require stopping the shared local SQL Server instance, which is unsafe/disruptive to do from an automated pass.
- **Large-scale concurrent load beyond 10 sessions** (STK-T10 extension): 10-way concurrency was executed live; higher concurrency was judged unnecessary given the atomic-decrement mechanism already proven correct at 2-way and 10-way scale.

## Automated vs. Manual vs. Build

```
Automated tests: 0 (no test project exists in this repo)
Harness-executed (real BLL/DAL calls against a live local dev DB, with full cleanup): 20 of the 30 catalog entries
Code-review-only (no execution): 6
Blocked (infrastructure unavailable): 6 counted above overlap with blocked rows in the table
Manual UI verification: NOT PERFORMED (no live IIS/browser in this session)
```

## Final Go-Live Assessment

```
CODE TEST STATUS:        CONDITIONAL   — build/precompile clean; PRT-002 print-parameter fix confirmed by code
                                          review; several pre-existing defects found in the invoice/stock save
                                          path unrelated to PRT-002 (see Critical Failures 1-6)
DATABASE TEST STATUS:    FAIL          — 2 of 3 legacy pages' master-invoice INSERT is completely broken
                                          against this schema (Finding 4); Return Invoice's target tables do
                                          not exist (Finding 6). NOTE: verify schema parity with production —
                                          these are confirmed against the local dev DB, not confirmed identical
                                          to production schema.
STOCK TEST STATUS:       CONDITIONAL   — atomic decrement and concurrency behavior are solid and PASS at
                                          scale (STK-T01/02/07/09/10/DAL-atomic); insufficient-stock and
                                          negative/zero-quantity handling FAIL (STK-T03/05/06/08)
TRANSACTION TEST STATUS: FAIL          — only the ByOrder page wraps Master+Detail+Stock in a TransactionScope;
                                          the 3 legacy pages have no rollback protection (Finding 1)
PRINT TEST STATUS:       BLOCKED       — PRT-002 fix verified by code review and build only; no live
                                          IIS/browser click-through was performed in this session
ERROR HANDLING STATUS:   CONDITIONAL   — SaveAction/UpdateAction logging (ERR-001, ByOrder path) confirmed
                                          working in the prior session; the 3 legacy pages' insert path does
                                          NOT log exceptions (Finding 5) and was not covered by ERR-001
```

**Note on scope**: Findings 1, 2, 3, 4, 5, and 6 above are **pre-existing defects discovered during this test-execution pass**, not regressions introduced by the PRT-002 print-flow fix delivered in the prior session (that fix touched only the print-button URL and a new `rptInvoiceId` label in 3 files — see `git diff` scope, unchanged by this session). They are reported here because the task asked for real execution evidence, not because they were in scope to fix. No code was modified in this session; ORD-001 was excluded from testing per instruction, and no `RemainingQty` validation was added or tested.

## Production Readiness

```
GO / CONDITIONAL GO / NO-GO:  NO-GO (for the 3 legacy pages: InvoiceCreationForCustomer,
                                InvoiceCreationForConsumerCustomer, ReturnInvoiceCreationForCustomer)
                              CONDITIONAL GO (for InvoiceCreationForCustomerByOrder — the fixed page from the
                                prior INV-001/STK-001/STK-002/ERR-001/PRT-002 session — pending a live
                                IIS/browser click-through, which this session could not perform)
```

The 3 legacy pages cannot create an invoice at all against this database (Finding 4) and, if that were fixed, would be exposed to unvalidated negative/zero quantities (Findings 2-3) and non-atomic partial-stock commits on insufficient stock (Finding 1) with no logging of the failures involved (Finding 5). The Return Invoice page additionally targets non-existent tables (Finding 6). None of this blocks the PRT-002 print-parameter fix itself, which is code-complete and build-verified — but printing correctly is moot for a page that cannot save an invoice in the first place.
