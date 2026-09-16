# Workflows

Index of the four cross-page workflows hand-traced in `specs/`. Each is worth reading in full before its
module's page specs — the workflow carries the algorithm; the page specs carry the per-control inventory. All
content below is summarized from the linked files, not re-derived.

| Workflow | Module | File |
|---|---|---|
| WF-004 — The generic approval workflow | 00-system | [`../specs/00-system/workflows/WF-approval.md`](../specs/00-system/workflows/WF-approval.md) |
| WF-001 — Invoice creation and FEFO batch allocation | sales-order-invoice | [`../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md`](../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md) |
| WF-002 — Delivery confirmation (Full/Partial/Reject) | delivery-logistics | [`../specs/delivery-logistics/workflows/WF-delivery-confirmation.md`](../specs/delivery-logistics/workflows/WF-delivery-confirmation.md) |
| WF-003 — Customer payment application | finance-collection | [`../specs/finance-collection/workflows/WF-customer-payment.md`](../specs/finance-collection/workflows/WF-customer-payment.md) |

## WF-004 — The generic approval workflow

One mechanism serves all 14 approval screens in the application (destroy product, credit limit, discount,
fixed discount, manual order, non-transactional invoice, petty cash, product quoted price, unit price, customer
return, sample product issue, slave info, trade material transfer, warehouse stock-in). A five-table chain
(`tblAction` → `tblActionStepDetail` → `tblActionSteps` → `tblActionPageWiseStep` → `tblMainMenu`, cross-checked
against `tblActionUserWiseApproval`) resolves, per user per page, the single action they are allowed to take.
The radio list of actions is always fully rendered; actions the user lacks are only **disabled**, not hidden,
and the grant is **not re-checked on postback** — a removed grant does not stop a submission already in flight
(`GAP-SEC-009`, test case `TC-WF004-05`). The literal label `"Cancel"` is a hard-coded escape hatch always left
enabled regardless of grant (`BR-SYS-008`). The worklist itself is filtered by the step's `ActionCondition`,
which is how multi-step chains are expressed — not by an explicit step number on the record, but by matching
its current status text. Approval date is stored as date-only (`DateTime.Today`), so same-day approvals are
indistinguishable by timestamp. None of the underlying vocabulary/steps/grants (`tblAction`,
`tblActionSteps`, `tblActionUserWiseApproval`, etc.) are present as data in this repository — see
[security.md](security.md). **Confidence: CONFIRMED.**

## WF-001 — Invoice creation and FEFO batch allocation

The single most important algorithm in the application — every proforma-invoice screen ends here. Customer
resolution runs a 9-way all-INNER-JOIN geography chain that silently shows nothing if any link is missing
(`BR-CUS-001`). Each grid line is allocated against `tblDCStore` batches ordered by nearest expiry (FEFO, not
FIFO) and can expand into **multiple** `tblInvoiceDetail` rows, one per batch consumed, carrying that batch's
own `BatchNo`/`ReceiveDate`/`ExpDate`. Two confirmed high-severity defects: **`GAP-APP-002`** — the discount
amount shown on screen (computed per unit) differs from the discount amount actually persisted (computed per
line), diverging whenever quantity ≠ 1; and **`GAP-APP-004`**/**`GAP-APP-005`** — there is no stock-availability
check before allocation (a shortfall is written silently, no error) and stock quantity is overwritten rather
than decremented (a lost update under concurrent invoicing of the same batch). Header and detail rows are also
saved as separate, non-transactional statements (`GAP-APP-007`), so a mid-loop failure leaves a partial invoice.
Full stage-by-stage trace, including 10 concrete test cases, in the linked file. **Confidence: CONFIRMED.**

## WF-002 — Delivery confirmation (Full/Partial/Reject)

The state change that turns a proforma invoice into a real, posted sale — everything downstream (collection,
ledger, sales reporting) depends on it. The dispatcher branches on the **display text** of a status dropdown
item: exactly `"Partial"` and `"Full"` are handled explicitly, and **everything else — including an unselected
placeholder — falls to reject** (`BR-DLV-001`). In the Full branch, two confirmed high-severity defects:
**`GAP-APP-018`** — the journal-posting call is made unconditionally without checking whether the confirmation
procedure succeeded, and the **same** success message (`"Delivery Invoice Save Successfully !!"`) is shown on
both the success and the fallback path, with no message at all if both calls fail; and **`GAP-APP-019`** — the
whole Full/Partial/Reject routing depends on the dropdown's display text rather than an id, so renaming the
underlying status label silently reroutes every row to reject. The invoice number for the Full branch is also
read from a positional grid cell (`Cells[7].Text`), not a data key (`GAP-APP-010`). The chart-of-accounts
precondition on this path is commented out in source — whether that was deliberate is **UNKNOWN**. Full trace
and 8 test cases in the linked file. **Confidence: CONFIRMED.**

## WF-003 — Customer payment application

Applying a received amount against one or more open invoices and posting it to the ledger. Two confirmed
high-severity defects dominate this workflow. **`GAP-APP-016`** — the screen computes whether an invoice becomes
`"Full"` or `"Partial"` and the cumulative amount paid, then calls `CustPaymentBLL.UpdateInvoicePaymentAmount`,
but the DAL method underneath passes **only the invoice id** to `sp_UD_InvoicePayment` — the amount and status
arguments are silently discarded, meaning the invoice's real paid amount/status is decided entirely inside a
procedure this repository does not contain. **`GAP-APP-017`** — paying multiple invoices with a single receipt
builds a fresh one-element detail list *inside* the per-invoice loop, so `SaveCustPayment` is called once per
ticked invoice, and **each call creates its own `tblCustomerPay` master row stamped with the full receipt
amount** — paying 5 invoices with one receipt writes 5 master rows, each carrying the whole receipt value, which
means any report summing `tblCustomerPay.PaymentAmount` over-counts. A financial-year check exists in source but
is commented out, so payments can be entered for dates no open financial year covers, with unknown downstream
effect. Full trace and 8 test cases in the linked file. **Confidence: CONFIRMED.**
