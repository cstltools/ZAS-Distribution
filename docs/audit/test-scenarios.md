# End-to-End Transaction Test Scenarios

Scope: Phase 22. Conceptual test scenarios with expected outcomes derived from actual code (not executed
against a running system — there is no test environment in this repository, `GAP-TEST-001`). Each scenario
states PASS/FAIL/PARTIAL/UNKNOWN against the *correct* business behavior, with the evidence for that verdict.

| ID | Scenario | Verdict | Evidence |
|---|---|---|---|
| `TEST-001` | Customer → Order → Invoice → Stock → Collection → Accounting, full happy path | **PARTIAL** | Each step works individually, but nothing verifies the *previous* step succeeded before the next runs (`BUG-TXN-001`, `GAP-APP-018`); the accounting step is unauditable (`GAP-DB-001`, though see the followup doc for what's now confirmed) |
| `TEST-002` | Invoice → Return → Stock Reversal → Customer Balance → Accounting Reversal | **FAIL (consumer-return path)** / **PARTIAL (invoice-return path)** | `return-audit.md` `BUG-RET-001`: consumer returns bypass quarantine and self-approve, skipping the normal posting path entirely. `BUG-RET-003`: journal posting fires on every approval action including Reject, meaning a *rejected* return can still post accounting reversal. `BUG-RET-006`: credit adjustment is not reversed when a return is rejected after being applied |
| `TEST-003` | Purchase → Receive → Stock → Supplier Payable → Accounting | **UNKNOWN / largely N/A** | `purchase-audit.md` `BUG-PUR-001`: no real PO/supplier-invoice module exists; the closest analogue (warehouse stock-in) has its GL posting call **commented out** (`BUG-PUR-002` / `BUG-ACC-004` cross-reference in `accounting-integration-audit.md`) — i.e. this scenario as classically defined does not have a live, GL-connected implementation in this codebase |
| `TEST-004` | Invoice → Partial Collection → Remaining Outstanding | **PARTIAL** | The GL-facing calculation (`sp_Sales_CustomerPaymentPosting`, confirmed body per `collection-audit.md`) correctly sums per-invoice allocated amounts (`tblCustPayDetail`), so the *outstanding* figure itself is not wrong; but the receipt audit trail (`tblCustomerPay`) is broken for any multi-invoice receipt (`BUG-FIN-002`/`GAP-APP-017`) and the screen's own Full/Partial decision is discarded before reaching the database (`BUG-FIN-001`/`GAP-APP-016`) |
| `TEST-005` | Backdated Invoice → Historical Stock Report | **FAIL** | `date-period-audit.md` `BUG-STOCK-019`: no future-date/backdating guard exists on any inventory date field anywhere in the traced code; `BUG-STOCK-020`: no period-close/financial-year lock exists anywhere in the system (the one financial-year guard that exists, `InvoiceDAL.GetFinancialYearInfo`, is disabled at its call sites per `financial-integrity-audit.md`) |
| `TEST-006` | Month-End Invoice → Accounting Posting | **FAIL (as a control)** | Same evidence as `TEST-005` — there is no enforced period boundary, so a month-end (or any-date) invoice posts exactly like any other; whatever period-cutoff discipline exists is procedural (human process), not system-enforced |

## Additional scenarios surfaced by this audit's own findings (not in the original six)

| ID | Scenario | Verdict | Evidence |
|---|---|---|---|
| `TEST-007` | Two users invoice the last unit of the same stock batch simultaneously | **FAIL** | `BUG-STOCK-002`/`GAP-APP-005`: stock quantity is overwritten with a C#-computed absolute value, not decremented atomically in SQL — the second writer's update overwrites the first, and total consumption can exceed what was actually on hand |
| `TEST-008` | One receipt pays five invoices in a single `CustomerPayment` save | **PARTIAL** | Correct amounts land in `tblCustPayDetail` and the correct total is posted to GL (per the followup reconciliation), but `tblCustomerPay` gains 5 rows each showing the full receipt amount (`BUG-FIN-002`) |
| `TEST-009` | A user reverses a payment via `PaymentReverse.aspx` | **FAIL** | `BUG-FIN-003`: the page issues a raw `DELETE`, never calling the GL reversal procedure — the accounting side of the original payment is never undone, only the local row disappears |
| `TEST-010` | Run `DuplicatePaymentList` / `sp_DEL_DuplicatePayment` against production-shaped data | **FAIL (high risk)** | `BUG-FIN-004`: grouping key `TransctionDetailId` defaults to 0 for nearly all payments outside one specific workflow, so most legitimate payments look like duplicates of each other and are eligible for mass deletion |
| `TEST-011` | Delivery confirmation posting procedure fails silently | **FAIL** | `GAP-APP-018`: identical success message shown whether the posting call succeeded or failed; no message at all if both calls fail |
| `TEST-012` | A menu item's status-list display text is renamed (e.g. "Full" → "Delivered") | **FAIL** | `BR-SYS-009`/`BR-DLV-001`: every row silently routes to the reject branch, because status routing compares dropdown display text, not a stable id |

## What would be needed to actually execute these

A test/staging environment with a restorable copy of `SalesrollDB_ZAS`, seed data for at least one customer,
product, batch and DC, and a way to inspect `tblDCStore`, `tblInvoice`, `tblCustomerPay`,
`tblCustPayDetail` and the ACCDB tables before/after each scenario. None of this exists in the repository
today (`GAP-TEST-001`). **HUMAN VERIFICATION REQUIRED** to actually run these scenarios and convert PARTIAL/
UNKNOWN verdicts into observed PASS/FAIL.
