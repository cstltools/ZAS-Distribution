# Sales / Invoice Calculation Audit

Audit-only. No source, data, or database was modified. Scope: Qty × Rate → Subtotal + VAT − Discount = Net
Amount, wherever implemented (client JS/markup, `.aspx.cs` code-behind, `Library.BLL`, `Library.DAL`), plus
duplicate-invoice prevention, invoice-date handling, and discount/VAT consistency between what the user sees
and what is persisted.

Files read: `Library.BLL/SInventory_BLL/InvoiceBLL.cs`, `Library.DAL/SInventory_DAL/InvoiceDAL.cs`,
`Library.DAO/SInventory_Entities/InvoiceDetail.cs`, and the code-behind/markup of
`InvoiceCreationForCustomer.aspx(.cs)`, `InvoiceCreationForCustomerByOrder.aspx(.cs)`,
`InvoiceCreationByOrder.aspx.cs`, `InvoiceCreationForConsumerCustomer.aspx.cs`,
`ReturnInvoiceCreationForCustomer.aspx.cs`, `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs`,
`DelivaryInvoiceCreationAuto.aspx.cs`, `GroupInvoiceEntry.aspx.cs`, `ManualOrderrCreation.aspx.cs`. The last
four do not perform their own Qty×Rate calculation (they aggregate/print already-persisted rows) and are
excluded from the calculation-chain findings below.

## Prior findings (specs/SPEC-GAPS.md) re-verified against current code

- **GAP-APP-002** (discount shown per-unit, stored per-line) — **CONFIRMED**, but narrower than the general
  statement: it reproduces only on `InvoiceCreationForCustomer.aspx.cs:403`
  (`(UnitPrice/100)*discountPer`, no `*qty`), which contradicts the server formula in
  `InvoiceBLL.cs:352`. The order-based flows (`InvoiceCreationForCustomerByOrder.aspx.cs:1009,366`,
  `InvoiceCreationForConsumerCustomer.aspx.cs:430`, `ReturnInvoiceCreationForCustomer.aspx.cs:581`) already
  multiply by qty and are internally consistent with the server. See BUG-SALES-001 for a related, more severe
  defect on the same page.
- **GAP-APP-003** (`DiscountofProduct` unused `date` param) — **CONFIRMED**,
  `Library.DAL/SInventory_DAL/InvoiceDAL.cs:931-937`. The commented-out predecessor immediately above (lines
  926-930) shows the removed `MinQty`/`MaxQty` banding, confirming the regression. Query is also built by
  string concatenation (`ProductCode`, `CustomerMasterId` unparameterized) — SQL-injection surface, noted but
  out of this audit's primary scope.
- **GAP-APP-020** (`payTypeDDL.Text == ""` dead validation) — **CONFIRMED** in two places:
  `InvoiceCreationForCustomer.aspx.cs:44` and `InvoiceCreationForCustomerByOrder.aspx.cs:107`. Both read
  `.Text` (never empty) instead of `.SelectedValue`; `Convert.ToInt32(payTypeDDL.SelectedValue)` downstream
  (`InvoiceCreationForCustomer.aspx.cs:486`, `InvoiceCreationForCustomerByOrder.aspx.cs:1479/1662`) throws if
  nothing was genuinely selected.
- **GAP-APP-004/005/006/007** (no stock check, lost stock updates, racy PK generation, non-transactional save)
  — still **CONFIRMED** for the plain customer-invoice page and for a dead legacy handler in the by-order page
  (see BUG-SALES-004). Note: `InvoiceCreationForCustomerByOrder.aspx.cs` has a newer, actively wired handler
  (`saveButton_Click1`) that has since been hardened with a `TransactionScope` and an order-duplicate check —
  see BUG-SALES-004/005 for the important nuance that the old, unprotected handler is still present as dead
  code in the same file.
- **GAP-APP-011/012/013** (invoice-number parse width, locale-dependent date concatenation, unguarded
  `Convert.ToDecimal`) — not independently re-verified in this pass; no evidence found contradicting them.

## Bug list

### BUG-SALES-001 — Discount silently dropped to zero on line insert (direct customer invoice)
**Severity:** CRITICAL · **Confidence:** CONFIRMED

- `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs:448-462` (`SaveInvoiceDetail`) sets only
  `aInvoiceDetail.DiscountPercentage`; `aInvoiceDetail.DiscountAmount` is never assigned and stays at the
  C# default `0` (`Library.DAO/SInventory_Entities/InvoiceDetail.cs:27`).
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs:348-363` (`InvoiceDetailInsertBLL`) branches only on:
  - `DiscountPercentage > 0 && DiscountAmount > 0`
  - `DiscountPercentage == 0 && DiscountAmount > 0`
  - `DiscountPercentage == 0 && DiscountAmount == 0` → sets stored discount to 0

  The actual case produced by this page — `DiscountPercentage > 0 && DiscountAmount == 0` — matches **none**
  of the three branches, so `aDetailForInsert.DiscountAmount` stays at its freshly-constructed default of `0`
  (`InvoiceBLL.cs:330`) even though a nonzero discount was shown and applied on screen.
- Effect: `tblInvoiceDetail.NetAmount = (TotalPrice - 0 - SpecialAmount) + VAT` is persisted **higher** than
  what the user saw. The invoice-master total (`TpDiscount`, from `disTotalTextBox`) is still populated
  correctly from the UI aggregate, so master and detail become internally inconsistent — the master shows a
  discount total that the detail rows don't individually add up to.

### BUG-SALES-002 — VAT is a flat per-unit amount, computed independently of discount
**Severity:** HIGH · **Confidence:** CONFIRMED (behavior) / SUSPECTED (whether it's a defect vs. intended tax rule)

- `Library.BLL/SInventory_BLL/InvoiceBLL.cs:371,383-384`: `TotalPriceVatAmount = UnitVatAmount * qty`; then
  `NetAmount = (TotalPrice - DiscountAmount - SpecialAmount) + TotalPriceVatAmount`.
- `UnitVatAmount` comes from the product master's `VATAmountPerUnit`
  (`InvoiceDAL.cs:704`, `ProductInfoDAL`) — a fixed currency figure, not a percentage of a taxable base. VAT
  is therefore computed on the **pre-discount** unit price; discounting a line has zero effect on its VAT.
  Client and server agree with each other here (no client/server mismatch), so this is a tax-computation
  question, not an inconsistency bug.
- **HUMAN VERIFICATION REQUIRED** — confirm with Finance whether VAT should be assessed net-of-discount, and
  whether `VATAmountPerUnit` is already discount-aware upstream (no visibility into how that column is
  populated/maintained from this code).

### BUG-SALES-003 — No rounding/precision policy in the calculation chain
**Severity:** MEDIUM · **Confidence:** CONFIRMED

- `Math.Round` does not appear anywhere in `Library.BLL` (full-directory search) — every money field
  (`TotalPrice`, `DiscountAmount`, `TotalPriceVatAmount`, `NetAmount`, `SpecialAmount`,
  `InvoiceBLL.cs:347,352,371,375-377,383-384`) is unrounded `decimal` arithmetic.
- Client-side formatting is inconsistent between fields on the *same* row: discount amounts use `.ToString("F")`
  (2-decimal) — `InvoiceCreationForCustomerByOrder.aspx.cs:1009,366`,
  `InvoiceCreationForConsumerCustomer.aspx.cs:430`, `ReturnInvoiceCreationForCustomer.aspx.cs:581,1016` — while
  `TotalPrice`/`TotalPriceVAT`/`NetPrice`/grand totals (`TotalValueCalculation()`, e.g.
  `InvoiceCreationForCustomer.aspx.cs:536-545`) use plain `.ToString()` (full precision).
- The one `Math.Round` found across all `*Invoice*.aspx.cs` files is commented out
  (`DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs:2576`) — tried once, then abandoned.
- Effect: displayed column totals (rounded) can disagree by fractions of a currency unit with the row-sum a
  user gets by adding what they see on screen — a "totals don't foot" defect, compounding BUG-SALES-001 in
  discount scenarios.

### BUG-SALES-004 — Dead, non-transactional duplicate-save handler left in the file alongside the fixed one
**Severity:** MEDIUM (not currently reachable, high blast radius if reintroduced) · **Confidence:** CONFIRMED

- `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` contains two save handlers:
  - `saveButton_Click1` (lines 1393-1462, **live** — wired via
    `InvoiceCreationForCustomerByOrder.aspx:503-505`): wraps master+detail+status-update in a
    `TransactionScope` (`ReadCommitted`, 5-minute timeout), calls `OrderExists()` first, and its comments
    reference a 60-second duplicate-invoice guard inside `sp_I_InvoiceMaster`.
  - `saveButton_Click` (lines 1500-1553, **dead**, not wired to any control): calls `SaveInvoice()` then
    `SaveInvoiceDetail()` sequentially with **no transaction**; on exception calls
    `aInvoiceBll.DeleteInvoice(orderIdHiddenField.Value)` — passing an **Order ID** into a method whose
    parameter is named/documented for an Invoice ID (`InvoiceBLL.DeleteInvoice(string Invoice)`), and which in
    any case does not roll back already-inserted `tblInvoiceDetail` rows or already-decremented `tblDCStore`
    stock. GAP-APP-005/006/007 apply in full to this dead path.
- Recommendation: delete `saveButton_Click` (and `SaveInvoice()` at line 1645 if otherwise unused) rather than
  leave an unsafe, identically-patterned method sitting next to the safe one.

### BUG-SALES-005 — No duplicate-submit protection on `InvoiceCreationForCustomer.aspx`
**Severity:** HIGH · **Confidence:** CONFIRMED

- `InvoiceCreationForCustomerByOrder.aspx` mitigates double-submit: `OrderExists()` check
  (`.aspx.cs:1400`), client-side button self-disable on click (`.aspx:505`), re-enabled only on AJAX error
  (`.aspx:643-649`), plus (per code comment, `.aspx.cs:1424-1426`) a DB-side 60-second window inside
  `sp_I_InvoiceMaster` — **HUMAN VERIFICATION REQUIRED — needs `sp_helptext sp_I_InvoiceMaster`** to confirm
  the exact predicate.
- `InvoiceCreationForCustomer.aspx.cs` `saveButton_Click` (lines 428-437) has **no** equivalent check; its
  markup (`InvoiceCreationForCustomer.aspx:403-404`) wires a plain button with no `OnClientClick` disable and
  no `UseSubmitBehavior="False"`. A double-click or resubmit after a slow round trip is not blocked anywhere
  in this file — `InvoiceBLL.SaveInvoice`'s `ClsPrimaryKeyFind.PrimaryKeyMax` (GAP-APP-006) will mint a second,
  distinct invoice with no additional guard.

### BUG-SALES-006 — Invoice date is `ReadOnly`, not server-enforced
**Severity:** MEDIUM · **Confidence:** SUSPECTED (mechanism confirmed; exploit path not attempted against a live instance)

- `InvoiceCreationForCustomer.aspx.cs:27-29` / markup `:113-114` and
  `InvoiceCreationForCustomerByOrder.aspx.cs:68-71`: `invDateTextBox` is `ReadOnly="True"`, populated once on
  `!IsPostBack` via `Todate()`.
- `SaveInvoice()`/`PrepareMasterData()` read `invDateTextBox.Text` directly
  (`InvoiceCreationForCustomer.aspx.cs:480`, `InvoiceCreationForCustomerByOrder.aspx.cs:1473,1656`) with no
  check against `DateTime.Today`. ASP.NET's `ReadOnly` attribute is HTML-only; `TextBox.Text` is restored from
  posted form data regardless. A crafted POST, or a session left open across a midnight boundary (since
  `Todate()` runs only once per session), can persist an `InvoiceDate` that doesn't match the real save time.

### BUG-SALES-007 — `CalculateGrandTotal()` uses absolute difference instead of subtraction
**Severity:** MEDIUM · **Confidence:** CONFIRMED

`InvoiceCreationForCustomerByOrder.aspx.cs:564-584`:
```csharp
if (grandTtl > ttldiscount)
    total = grandTtl - ttldiscount;
else
    total = ttldiscount - grandTtl;   // absolute value, not a real subtraction
```
If the running special-discount total (`pdTextBox`) ever meets or exceeds the grand total (over-entry, or a
rounding artifact from BUG-SALES-003), the grand total is silently overwritten with
`discount − old grand total` — an unrelated positive number — instead of being clamped to zero or flagged.

### BUG-SALES-008 — Consumer-customer flow hardcodes discount to zero
**Severity:** LOW/MEDIUM (business-intent dependent) · **Confidence:** CONFIRMED (code) / UNKNOWN (whether it's a bug)

`InvoiceCreationForConsumerCustomer.aspx.cs:610`: `aInvoiceDetail.DiscountAmount = 0;` unconditionally, while
the grid still renders discount columns/values to the operator (`InitialGrid()` lines 141,160,194,217).
**HUMAN VERIFICATION REQUIRED** — confirm with the business whether consumer-customer invoices are meant to
always suppress discounts; if not, this is the same class of defect as BUG-SALES-001.

## Summary table

| ID | Title | Severity | Confidence |
|---|---|---|---|
| BUG-SALES-001 | Discount dropped to 0 on insert (direct customer invoice) | CRITICAL | CONFIRMED |
| BUG-SALES-002 | VAT flat-per-unit, unaffected by discount | HIGH | CONFIRMED / SUSPECTED (as defect) |
| BUG-SALES-003 | No rounding policy; inconsistent client formatting | MEDIUM | CONFIRMED |
| BUG-SALES-004 | Dead non-transactional save handler left in file | MEDIUM | CONFIRMED |
| BUG-SALES-005 | No duplicate-submit protection on plain customer invoice | HIGH | CONFIRMED |
| BUG-SALES-006 | `ReadOnly` invoice date not server-enforced | MEDIUM | SUSPECTED |
| BUG-SALES-007 | `CalculateGrandTotal()` absolute-value bug | MEDIUM | CONFIRMED |
| BUG-SALES-008 | Consumer-customer discount hardcoded to 0 | LOW/MEDIUM | CONFIRMED / UNKNOWN intent |

**Needs human/DB verification:**
- `sp_I_InvoiceMaster` — exact duplicate-invoice predicate (BUG-SALES-005).
- Whether `tblProduct`/unit-price VAT-per-unit figures are already net-of-discount upstream (BUG-SALES-002).
