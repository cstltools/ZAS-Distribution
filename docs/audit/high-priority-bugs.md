# High-Priority Bugs (Severity: HIGH)

Scope: Phase 23, HIGH severity. Grouped by module. Each entry: title, location, description, impact,
recommended fix, confidence. Full narrative and additional context is in the linked source document.

## Security / API

**`GAP-SEC-004` — 18 unauthenticated ASMX autocomplete methods.** `SInventoryWebService.asmx` (exposed twice).
Leaks customer, product, price and employee masters to any caller; injectable via concatenated `LIKE`. Fix:
require session/API-key auth on every method; parameterize the query. *Source: `security-audit.md`.*

**`GAP-SEC-008` — Debug configuration in production.** `Web.config`: `debug="true"`, no `<customErrors>`,
24,000-minute session timeout vs. a shorter auth-cookie timeout (mismatch noted). Unhandled exceptions render
SQL and stack traces to the browser. Fix: `debug="false"`, custom error pages, align timeouts. *Source:
`security-audit.md`.*

**`GAP-SEC-009` — No horizontal access control.** No page verifies a record belongs to the requesting user's
company unit; ids travel in query string/Session unchecked. Fix: add ownership checks at the DAL boundary for
every id-keyed read. *Source: `security-audit.md`.*

**`GAP-SEC-001` — Menu doubles as the entire permission model.** Hiding a menu item does not protect the page
behind it; direct URL access bypasses it. Fix: enforce authorization server-side per page/action, independent
of menu visibility. *Source: `security-audit.md`.*

**`BUG-SEC-001` — Client-side-only role gating on sensitive reports.** `AllSalesReport.aspx.cs` disables a
"national report" checkbox for non-admins client-side only; the click handler never re-checks role
server-side. Pattern suspected in 6+ sibling report pages. Fix: re-check role server-side before returning
data. *Source: `security-audit.md`.*

**`BUG-API-001` — Unauthenticated, injectable WebMethod.** `CashFlow.aspx.cs::GetAglingReport` builds SQL from
`companyId`/`year`/`month` by concatenation with no auth check. Fix: authenticate, parameterize. *Source:
`api-audit.md`.*

**`BUG-API-003` — All 40 page-level `[WebMethod]`s are `static` and bypass `Page_Load`'s auth guard entirely.**
Every one of them should be treated as unauthenticated regardless of the page's own login check. Fix: add an
explicit session check inside each `[WebMethod]` (a static method cannot rely on instance-level `Page_Load`
logic). *Source: `api-audit.md`.*

## Transaction integrity

**`BUG-TXN-001` — Invoice save has no C#-level transaction.** A mid-sequence failure leaves a committed header
with partial details/stock. *File: `InvoiceCreationForCustomer.aspx.cs::saveButton_Click` and
`Library.BLL/SInventory_BLL/InvoiceBLL.cs`.* Fix: wrap in `TransactionScope` or move orchestration into one
transactional procedure. *Source: `transaction-integrity-audit.md`.*

**`BUG-TXN-002` — Zero try/catch anywhere in the invoice save chain.** An exception mid-save propagates as an
unhandled error with no cleanup of already-written rows. Fix: add try/catch with compensating logic or rely on
the transaction wrap above. *Source: `transaction-integrity-audit.md`.*

**`BUG-TXN-003` — Bulk invoice cancellation loop is unwrapped.** A partial cancel across multiple invoices is
possible if one iteration fails. Fix: same transactional wrap. *Source: `transaction-integrity-audit.md`.*

**`BUG-TXN-004` — `DB_Manager` swallows exceptions and returns `0`.** Indistinguishable from "no rows matched."
Callers cannot tell success-with-no-effect from failure. Fix: rethrow or return a distinguishable error signal.
*Source: `transaction-integrity-audit.md`, also `GAP-APP-008`.*

## Data integrity

**`BUG-DATA-001`/`BUG-DATA-002` — `MAX(id)+1` primary-key and invoice-number generation races.** Read outside
any transaction/lock; two concurrent saves can compute the same next id. Fix: use `IDENTITY` columns or
`sp_getapplock`-guarded sequence generation. *Source: `data-integrity-audit.md`, also `BR-SYS-003`,
`GAP-APP-006`.*

**`BUG-DATA-003` — Header insert's boolean success result is discarded before inserting children.** A failed
header insert does not stop child-row inserts from being attempted. Fix: check the return value before
proceeding. *Source: `data-integrity-audit.md`.*

**`BUG-DATA-004` — `ExecuteNonQueryAction` masks real errors as "0 rows affected."** Same root cause as
`BUG-TXN-004`; listed separately because it was found from the data-integrity angle (silent data loss risk).
*Source: `data-integrity-audit.md`.*

**`BUG-DATA-005` — Stock updated via computed absolute `SET`, not an atomic decrement.** Creates a lost-update
window under concurrent access. Fix: `UPDATE tblDCStore SET StockQty = StockQty - @qty WHERE ... AND StockQty
>= @qty`. *Source: `data-integrity-audit.md`, also `GAP-APP-005`, `BUG-STOCK-002`.*

## Inventory / Stock

**`BUG-STOCK-002` — Stock updates use a C#-computed overwrite instead of an atomic SQL decrement**, confirmed
in 7+ methods across the module (not just the invoice path). Same fix as `BUG-DATA-005`.

**`BUG-STOCK-004` — No negative-stock guard at the SQL layer anywhere.** All quantity checks are client-side
and bypassable via a direct postback. Fix: add a `WHERE StockQty >= @qty` guard (or a CHECK constraint) at the
database layer.

**`BUG-STOCK-005` — DC-to-DC transfer deducts source stock at challan creation, before destination confirms.**
No in-transit bucket exists; a lost/abandoned transfer permanently loses stock with only an unconfirmed
document as evidence. Fix: introduce an in-transit stock state, decrement source only on confirmed receipt.

**`BUG-STOCK-007` — Destroy approval can drive frozen stock negative** (`qty = currentFreezeStockQty -
StockOutQty` with no `>= 0` check; confirmed the paired GL procedure doesn't guard it either). Fix: validate
before writing.

**`BUG-STOCK-014` — Possible double stock deduction via `sp_Process_WeightedAverageSalesConfirmation`**, called
from `sp_Sales_ExecuteDeliveryPipeline`. UNKNOWN — HUMAN VERIFICATION REQUIRED (this specific callee's body was
not part of the 27 recovered in the follow-up pass).

**`BUG-STOCK-019`/`BUG-STOCK-020` — No future-date/backdating guard, and no period-close/financial-year lock,
anywhere in the traced inventory code.** The one financial-year guard in the codebase is disabled at its call
sites (see `financial-integrity-audit.md`). Fix: add both controls; requires a business decision on where the
period boundary should live.

## Sales / Purchase / Returns

**`BUG-SALES-002` — VAT is flat-per-unit and unaffected by discount**, an inconsistency between how discount
and VAT interact on paper vs. in the persisted calculation. Confidence: CONFIRMED as coded / SUSPECTED as a
defect (may be the intended tax treatment — needs business confirmation).

**`BUG-SALES-005` — No duplicate-submit guard on the plain customer-invoice screen.** A double form submission
can create two invoices for the same intent. Fix: disable the save button on click, add a server-side
idempotency check.

**`BUG-PUR-001` — No real purchase-order module exists**; the closest analogue (warehouse stock-in) has an
unconstrained receipt with no PO to match against. Business decision needed on whether this is intentional
(purchasing lives in an external system) — see `purchase-audit.md`/`module-audit.md`.

**`BUG-PUR-004` — Duplicate-receipt detection is a time-window heuristic**, not a real constraint (confirmed:
`sp_I_InvoiceMaster`'s equivalent 60-second guard on the sales side is explicitly not concurrency-safe per the
follow-up pass — the same class of weakness applies here).

**`BUG-PUR-006` — Non-transactional multi-statement posting** on the goods-receipt path — same root cause as
`BUG-TXN-001`, different module.

**`BUG-PUR-008` — `SELECT MAX(id)+1` primary-key race**, same as `BUG-DATA-001`, on the purchase/receipt
tables.

**`BUG-RET-002` — Freeze-store idempotency check matches the wrong branch and is scoped to "today" only.**
Re-running an approval on a later day double-counts a freeze row. Fix: correct the branch condition in
`CheckReturnExistOrNotInFreezeStore` and remove the same-day restriction.

**`BUG-RET-006` — Credit adjustment granted at return creation, never reversed if the return is later
rejected.** Fix: add a compensating reversal on reject.

## Finance / Collection

**`BUG-FIN-001` — `UpdateInvoicePaymentAmount` discards its amount/status arguments (`GAP-APP-016`).**
**Downgraded during this audit**: reading `sp_UD_InvoicePayment`'s actual body shows it recomputes everything
from source tables regardless of what it's sent — the discarded parameters are dead code, not a live
correctness bug. Still worth cleaning up for maintainability. *Source: `collection-audit.md`,
`accounting-integration-audit-followup.md`.*

**`BUG-FIN-002` — One receipt produces N `tblCustomerPay` master rows (`GAP-APP-017`).** Confirmed NOT to
overstate GL/report totals (they read the correctly-allocated `tblCustPayDetail`), but it does corrupt the
receipt table's 1-row-per-receipt assumption — a real audit-trail defect. Fix: restructure to one master + N
allocation-detail rows.

**`BUG-FIN-005` — Overpayment is never validated**; an overpaid invoice is permanently misclassified
`'Partial'` rather than `'Full'` or credited, per the confirmed `sp_UD_InvoicePayment` logic (`>=` vs `=`
comparison). Fix: handle the overpayment case explicitly (credit note / `'Full'` classification with a
recorded overage).

**`BUG-FIN-007` — Financial-year guard is commented out; a payment dated outside any financial year saves on
the SalesRoll side but is silently excluded from GL posting** (confirmed: the posting SP's `INNER JOIN` to
`tblFinancialYear` would simply drop the row from its cursor). The user sees "Data Saved Successfully"
regardless. Fix: re-enable the guard, or make the SP raise a detectable error/flag instead of silently
skipping.

**`BUG-FIN-008` — No transaction across a multi-invoice receipt; partial failure is silently swallowed**,
compounding `BUG-TXN-004`'s exception-swallowing in the same call chain.

**`BUG-FIN-009` — Credit-limit block is self-overridable by the same user**, with no approver gate — routed
through a self-service checkbox rather than the application's real approval engine (`ClsApprovalAction`). Fix:
require a distinct approver role.

**`BUG-FIN-011` — Credit-limit lookup ignores `ActionStatus` and picks an unordered row** when multiple credit
limit records exist for a customer. Fix: filter to `Approved`/active status and pick the most recent explicitly.

## Performance

**`BUG-PERF-003` — `InvoiceBLL.SaveInvoiceDetails` issues multiple sequential round trips per invoice line**
(one per batch consumed, each its own DB call). Fix: batch the per-line writes into a single call (table-valued
parameter or a single stored-procedure invocation per invoice, not per line/batch). *Source:
`performance-audit.md`.*

---

See `bug-register.md` for the complete index and `medium-low-bugs.md` for everything below HIGH.
