# Testing

## Current state

**No test project exists.** There is no test file, no test data, and no test framework reference anywhere in
the repository.

- **Finding:** `GAP-TEST-001` · Confidence: **CONFIRMED** · Status: `KNOWN` (no artefact will change this fact)
- Source: [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md)

No CI configuration exists either to run tests automatically even if they existed — see
[deployment.md](deployment.md).

## What should be tested — where the proposed cases already live

Every one of the 383 page specs under `specs/*/pages/*.md` already carries a **section 27 ("Test Cases")**
proposing test cases derived directly from the page's own validation messages and control-flow branches — these
were never run, since no test project exists, but they are grounded in real code (the exact `if` conditions and
message strings in each page's `Validation()` method), not invented scenarios. **Do not re-derive these here** —
this document exists to point at them, not duplicate them.

To find the proposed tests for a given screen: open its page spec under `specs/<module>/pages/PAGE-<ID>-<Name>.md`
and read section 27. For example, `specs/sales-order-invoice/pages/PAGE-SAL-010-InvoiceCreationForCustomer.md`
carries validation-derived cases for the order-number/date/payment-type checks described in
[coding-standard.md](coding-standard.md).

The four hand-traced cross-page workflows also carry their own test-case tables, which are a better starting
point than individual page specs for anything spanning more than one screen — see [workflows.md](workflows.md)
and the `TC-WF00*` tables in:

- [`../specs/00-system/workflows/WF-approval.md`](../specs/00-system/workflows/WF-approval.md) — `TC-WF004-*`
- [`../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md`](../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md) — `TC-WF001-*`
- [`../specs/delivery-logistics/workflows/WF-delivery-confirmation.md`](../specs/delivery-logistics/workflows/WF-delivery-confirmation.md) — `TC-WF002-*`
- [`../specs/finance-collection/workflows/WF-customer-payment.md`](../specs/finance-collection/workflows/WF-customer-payment.md) — `TC-WF003-*`

## Known correctness defects as regression-test candidates

These are `GAP-APP-*` findings — confirmed defects in the running code, not hypothetical bugs — reframed as the
specific input that would trigger each one. A regression test suite built to protect against reintroducing these
defects (or to characterize current behavior before refactoring) should start here.

| ID | Trigger input | Observed (defective) behavior | Source workflow |
|---|---|---|---|
| `GAP-APP-002` | Save a proforma invoice line with `Quantity > 1` and a non-zero discount percentage | The discount total shown on screen before saving differs from the discount amount actually persisted to `tblInvoiceDetail` | [WF-invoice-batch-allocation.md](../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md) `TC-WF001-06` |
| `GAP-APP-003` | Set up a product discount rule, then invoice the same product/customer on a date it should no longer apply | The discount still applies — the `date` parameter passed to `DiscountofProduct` is never used in the query | `../specs/00-system/business-rules.md` `BR-PRC-003` |
| `GAP-APP-004` | Invoice a quantity greater than total available stock across all batches | The invoice is created anyway with fewer units allocated than requested; no error is raised | [WF-invoice-batch-allocation.md](../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md) `TC-WF001-05` |
| `GAP-APP-005` | Two concurrent invoices allocate from the same `tblDCStore` batch | The second `UPDATE` overwrites the first — stock ends up overstated | [WF-invoice-batch-allocation.md](../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md) `TC-WF001-09` |
| `GAP-APP-007` | Force a failure partway through `SaveInvoiceDetail`'s per-line loop (e.g. a bad batch row) | The invoice header and prior detail rows stay committed; no compensating rollback | `../specs/00-system/business-rules.md` `BR-SYS-004` |
| `GAP-APP-011` | Create an invoice for a company unit whose id crosses from 2 digits to 3 digits (e.g. id 99 → 100) | The `SUBSTRING` offset used to parse the running invoice number changes, corrupting the sequence for that unit's history | `../specs/00-system/architecture.md` |
| `GAP-APP-016` | Apply a customer payment and observe the invoice's resulting `PaymentStatus`/paid amount | The screen's computed Full/Partial decision and cumulative amount are silently discarded — `sp_UD_InvoicePayment` receives only `@InvoiceId` | [WF-customer-payment.md](../specs/finance-collection/workflows/WF-customer-payment.md) stage 4 |
| `GAP-APP-017` | Pay 5 open invoices with a single receipt on `CustomerPayment.aspx` | 5 separate `tblCustomerPay` master rows are created, **each** stamped with the full receipt amount — any report summing `PaymentAmount` over-counts by 5× | [WF-customer-payment.md](../specs/finance-collection/workflows/WF-customer-payment.md) `TC-WF003-02` |
| `GAP-APP-018` | Force `sp_DeliveryConformationFull` to fail while `sp_Sales_ExecuteDeliveryPipeline` succeeds (or vice versa) | The same `"Delivery Invoice Save Successfully !!"` message shows on both the success and the fallback path; if both fail, no message shows at all | [WF-delivery-confirmation.md](../specs/delivery-logistics/workflows/WF-delivery-confirmation.md) `TC-WF002-02`/`03` |
| `GAP-APP-019` | Rename the `Full`/`Partial`/`Cash`/`Bank` display text of the underlying data-driven list item | Every row routes to reject (delivery) or the account requirement stops being enforced (payment) — because control flow keys on display text, not id | [WF-delivery-confirmation.md](../specs/delivery-logistics/workflows/WF-delivery-confirmation.md) `TC-WF002-05` |
| `GAP-APP-020` | Save `InvoiceCreationForCustomer` with the payment-type placeholder still selected | Validation passes (`payTypeDDL.Text` is never empty), then `Convert.ToInt32(payTypeDDL.SelectedValue)` throws on save | [WF-invoice-batch-allocation.md](../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md) `TC-WF001-10` |
| `GAP-APP-009` | Enter a customer or product name containing a `:` character in an autocomplete lookup | The colon-delimited composite string parses incorrectly, shifting every subsequent field; a value with no `:` throws `IndexOutOfRangeException` | `../specs/00-system/business-rules.md` `BR-SYS-006` |

## What a maintainer should do before writing new tests

1. Read [`../specs/00-system/conventions.md`](../specs/00-system/conventions.md) first — a large share of "test
   cases" for any given page are just its validation messages, and that document explains the shape they take.
2. Prefer the workflow-level test cases (`TC-WF00*`) over single-page cases when the behavior spans more than
   one screen — the workflows already show the full call chain and where it breaks.
3. Treat everything below the stored-procedure boundary as untestable from this repository alone — 119
   procedure bodies are absent (`GAP-DB-001`), so any test that depends on procedure-internal logic (e.g. what
   `PaymentStatus` value means "fully paid," or what `sp_Sales_ExecuteDeliveryPipeline` actually posts) needs the
   procedure source or a live database to verify against.
