# Sales Return Audit

Audit-only. No source, data, or database was modified. Scope: `ReturnProductConsCustomerByInvoice.aspx.cs`
(RET-002, consumer customer), `ReturnProductCustomerByInvoice.aspx.cs` (RET-003, regular customer),
`ReturnustomerByInvoiceApproval.aspx.cs` (RET-004, approval), plus `Library.BLL/SInventory_BLL/InvoiceBLL.cs`,
`CustomerReturnBLL.cs`, and `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs`, `InvoiceDAL.cs`, `ChalanDAL.cs`.

Prior claim tested: `specs/returns/README.md:5` (`BR-INV-004`) states returned goods land in the freeze/
quarantine tier `tblDCStoreFreeze` via `InvoiceBLL.SaveDCStoreFreeze`, which "inserts only when no row already
exists for that invoice-detail line" (idempotent per line). **This claim only partly holds — see BUG-RET-002,
and does not hold at all for RET-002 — see BUG-RET-001.**

## Q1 — Does return qty verify against sold/remaining qty?

**CONFIRMED, present, but with a data-quality flaw.** Both pages compute `AvailableQty` server-side:
`Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:73` (RET-003, `LoadInvoiceNew`) and `:87` (RET-002,
`LoadInvoiceNewConsumer`) as `Quantity - SUM(ReturnQty already recorded)`. `retQtyTextBox_OnTextChanged`
(`ReturnProductConsCustomerByInvoice.aspx.cs:211-231`, `ReturnProductCustomerByInvoice.aspx.cs:288-309`)
rejects entries greater than that.

**Flaw:** the `SUM(ReturnQty)` subquery is not filtered by `ActionStatus` — a **rejected** prior return still
permanently reduces `AvailableQty`. RET-003 additionally excludes the entire invoice from search once *any*
`tblCustomerReturnMaster` row exists for it (`CustomerReturnDAL.cs:79`, unconditional on status), permanently
blocking a second/legitimate partial return after one rejection. RET-002 has no such invoice-level exclusion —
inconsistent between the two pages.

## Q2 — Is stock restored exactly once?

**CONFIRMED — no, in two distinct ways.**

**RET-002 bypasses the quarantine tier entirely and self-approves.**
`ReturnProductConsCustomerByInvoice.aspx.cs:106-160` (`saveButton_Click`) inserts the return detail, then
calls `CustomerReturnDAL.UpdateDCStoreQty` (`:106-110`):
```csharp
UPDATE dbo.tblDCStore SET StockQty=StockQty+<qty> WHERE DCStoreId='<id>'
```
directly against **sellable** stock — never through `tblDCStoreFreeze` — with no idempotency guard, and then
sets `ActionStatus = "Accepted"` itself in the same request, so RET-004's approval screen never reviews it and
no accounting posting (`sp_Sales_DeliverySalesReturnPosting`, only called from RET-004) ever fires for
consumer-customer returns. A double form submission (no button-disable, no idempotency key) re-runs the loop
and double-credits stock.

**RET-004's "already frozen" check matches the wrong branch and the wrong day.**
`Library.BLL/SInventory_BLL/InvoiceBLL.cs:160-183` (`SaveDCStoreFreeze`) only inserts into
`tblDCStoreFreeze` when `CheckReturnExistOrNotInFreezeStore` (`InvoiceDAL.cs:1448-1472`) finds no existing row.
That method branches on `if (InvoiceDetailId > 0) /* delivery confirmation */ else if (CustReturnDetailId > 0)
/* sales return */`. `ReturnustomerByInvoiceApproval.aspx.cs:110-113` populates **both** fields on the sales-
return object, so the delivery-confirmation branch always wins — the match key becomes
`InvoiceDetailId + ProductCode + BatchNo + TotalQuantity`, restricted to rows frozen **today**
(`CONVERT(DATE,FreezeRcvDate) = CONVERT(DATE,GETDATE())`). Re-running the same approval on a later day inserts
a **second** freeze row for the same return (real double-count); the return value of `SaveDCStoreFreeze` is
also never checked at the call site (`ReturnustomerByInvoiceApproval.aspx.cs:157`), so a failed/skipped freeze
still lets the return proceed to "Accepted" plus journal posting.

Weighted-average freeze-stock costing is commented out (`InvoiceBLL.cs:172-178`) — dead code, though
`specs/returns/README.md`'s SP table lists it as an active call site; note only, not a bug.

## Q3 — Is customer balance/due adjusted?

**CONFIRMED for RET-003 (Credit payment type only); absent for RET-002.**
`ReturnProductCustomerByInvoice.aspx.cs:172-197` inserts a `tblCreditAdjustment` row
(`InvoiceDAL.cs:1634-1641`, plain INSERT) when the selected payment type is `"Credit"`. There is no update to
`tblCustPayDetail`, `tblInvoice` paid/due columns, or any ledger balance anywhere in this chain — whether
`tblCreditAdjustment` is later netted against displayed due is UNKNOWN from these files (would need a
repo-wide trace of its readers, not performed here). Cash-type returns and all RET-002 (consumer) returns
create no balance-side record at all.

## Q4 — Creation-time vs approval-time adjustment; rejected-return risk

**CONFIRMED — inconsistent, and unsafe on reject.**

| Effect | RET-002 (consumer) | RET-003 (regular) |
|---|---|---|
| `tblCustomerReturnDetail` insert | at creation | at creation |
| Sellable stock (`tblDCStore`) | at creation, self-approved | not touched at creation |
| Quarantine stock (`tblDCStoreFreeze`) | never (BUG-RET-001) | at approval, Accepted only |
| `tblCreditAdjustment` (Credit) | n/a | at creation, before approval |
| Journal posting | never | at approval, for **any** action |

`ReturnustomerByInvoiceApproval.aspx.cs:96-165` (`SubmitApproval`): the outer guard already ensures
`actionRadioButtonList.SelectedValue` is non-empty, so `if (SelectedValue != null)` at line 96 is always true
— it does not gate on "Accepted". Only the `SaveDCStoreFreeze` block is conditioned on Accepted;
`ApprovalUpdateDAL` and, critically, `PostDeliverySalesReturn` (→ `sp_Sales_DeliverySalesReturnPosting`) run
**unconditionally for every action, including Rejected**. Combined with Q3, a rejected RET-003 return leaves a
permanent credit-adjustment in the customer's favor and still fires the journal-posting call, while goods are
never received into any stock table. **HUMAN VERIFICATION REQUIRED — needs `sp_helptext
sp_Sales_DeliverySalesReturnPosting`** to size the actual financial blast radius; the call-site logic (fires
regardless of accept/reject) is confirmed from C# alone.

## Q5 — Return date vs invoice date / cutoff

**Not applicable — no bug.** Neither page exposes a user-editable return-date field; both hard-code
`EntryDate = DateTime.Now` (`ReturnProductConsCustomerByInvoice.aspx.cs:99`,
`ReturnProductCustomerByInvoice.aspx.cs:148`). RET-004's `Validation()` checks only that an active financial
year exists for today (`GetFinancialYearInfo`, `ReturnustomerByInvoiceApproval.aspx.cs:174-183`) — not a
return-vs-invoice-date comparison. No user-controlled date input exists to validate, so there is nothing to
flag.

## Q6 — Duplicate-return prevention

Aggregates Q1/Q2: RET-003's blanket "any prior return blocks re-search" (Q1) incidentally prevents most
double-returns but also blocks legitimate follow-up partial returns; RET-002 has no equivalent and no
save-time re-check, so a resubmitted grid can double-credit stock (reinforces BUG-RET-001). No DB-level unique
constraint is visible from the application code (schema not in repo).

## Bug list

### BUG-RET-001 — Consumer-customer returns (RET-002) bypass quarantine, self-approve, skip posting, no duplicate guard
**Severity:** CRITICAL · **Confidence:** CONFIRMED

`ReturnProductConsCustomerByInvoice.aspx.cs:106-160` writes directly to sellable `tblDCStore.StockQty`
(`CustomerReturnDAL.cs:106-110`, unconditional `+=`), sets `ActionStatus = "Accepted"` in the same request
(bypassing RET-004 review entirely), never calls `sp_Sales_DeliverySalesReturnPosting`, and has no
duplicate-submission guard of any kind.

### BUG-RET-002 — Freeze-store idempotency check matches the wrong branch and date window
**Severity:** HIGH · **Confidence:** CONFIRMED (code logic)

`CheckReturnExistOrNotInFreezeStore` (`InvoiceDAL.cs:1448-1472`) always takes the "delivery confirmation"
branch for sales-return calls because `InvoiceDetailId` is always populated
(`ReturnustomerByInvoiceApproval.aspx.cs:110-113`), and restricts matching to rows frozen "today" — the
BR-INV-004 idempotency guarantee does not hold across day boundaries. `SaveDCStoreFreeze`'s return value is
never checked at the call site (`ReturnustomerByInvoiceApproval.aspx.cs:157`).

### BUG-RET-003 — Approval posts the journal entry on every action, including Reject
**Severity:** CRITICAL · **Confidence:** CONFIRMED (code logic) — needs SP verification for financial impact

`ReturnustomerByInvoiceApproval.aspx.cs:96-165`: the `if (SelectedValue != null)` guard at line 96 is
always true by construction; `PostDeliverySalesReturn` runs for every selected action, not only "Accepted".
**HUMAN VERIFICATION REQUIRED — needs `sp_helptext sp_Sales_DeliverySalesReturnPosting`.**

### BUG-RET-004 — Availability checks are not `ActionStatus`-aware
**Severity:** MEDIUM · **Confidence:** CONFIRMED

Rejected returns still count against `AvailableQty` (both pages), and RET-003 permanently blocks re-returning
an invoice after one rejection (`CustomerReturnDAL.cs:79`).

### BUG-RET-005 — Only `tblCreditAdjustment` insert found as a balance-side effect; downstream consumption unverified
**Severity:** MEDIUM · **Confidence:** CONFIRMED (insert exists) / UNKNOWN (downstream effect)

RET-003, Credit payment type only (`InvoiceDAL.cs:1634-1641`). RET-002 has no balance/due adjustment of any
kind. Whether `tblCreditAdjustment` is later netted into displayed customer due needs a separate trace of its
readers — not performed in this pass.

### BUG-RET-006 — Credit adjustment granted at creation, never reversed on reject
**Severity:** HIGH · **Confidence:** CONFIRMED

RET-003's `tblCreditAdjustment` row (Q3) is written before RET-004 approval and no code path reverses it if
the return is subsequently rejected — see Q4.

## Summary table

| ID | Title | Severity | Confidence |
|---|---|---|---|
| BUG-RET-001 | Consumer returns bypass quarantine, self-approve, skip posting | CRITICAL | CONFIRMED |
| BUG-RET-002 | Freeze-store idempotency check wrong branch/day-scoped | HIGH | CONFIRMED |
| BUG-RET-003 | Journal posting fires on every approval action incl. Reject | CRITICAL | CONFIRMED — needs `sp_helptext sp_Sales_DeliverySalesReturnPosting` |
| BUG-RET-004 | Availability checks ignore `ActionStatus` | MEDIUM | CONFIRMED |
| BUG-RET-005 | Balance-side effect limited to unverified `tblCreditAdjustment` insert | MEDIUM | CONFIRMED / UNKNOWN downstream |
| BUG-RET-006 | Credit adjustment not reversed on reject | HIGH | CONFIRMED |

**Needs human/DB verification:**
- `sp_Sales_DeliverySalesReturnPosting` — confirm GL/ledger effect (BUG-RET-003).
- `sp_Process_WeightedAverageFreezeStock` — confirmed dead/commented out in code; spec/README discrepancy
  noted only.
- Schema for `tblCustomerReturnDetail`/`tblDCStoreFreeze` — whether any unique constraint mitigates
  BUG-RET-001/002 at the DB layer (no DDL in repo).
