# Distribution Transaction Flow Audit

Scope: Phase 5. Traces the order-to-cash lifecycle end to end using the two fully-worked reference workflows
already produced in `specs/` (`WF-invoice-batch-allocation.md`, `WF-delivery-confirmation.md`), re-verified
here and connected to the accounting/stock/collection audits produced elsewhere in `docs/audit/`.

## 1. The confirmed lifecycle

```
Customer resolved (9-way INNER JOIN — any missing link silently blanks the header, BR-CUS-001)
   │
   ▼
Proforma invoice created  — InvoiceCreationForCustomer(ByOrder).aspx.cs / InvoiceBLL.SaveInvoice+SaveInvoiceDetails
   │   - stock is allocated HERE, FEFO, batch-by-batch (tblDCStore), not at delivery confirmation
   │   - BUG-SALES-* (see docs/audit/sales-audit.md): discount shown ≠ discount stored (GAP-APP-002),
   │     no stock-shortage error (GAP-APP-004), lost-update stock overwrite (GAP-APP-005)
   │   - accounting: sp_ProformaPosting — body UNKNOWN (GAP-DB-001)
   ▼
Delivery confirmation — DelivaryInvoiceCreationAuto.aspx.cs, branches on dropdown DISPLAY TEXT
   ├── "Full"    → sp_DeliveryConformationFull → sp_Sales_ExecuteDeliveryPipeline (accounting; body UNKNOWN)
   ├── "Partial" → redirect, screen-side UPDATE tblInvoice + sp_Sales_DeliverySalesPosting
   └── else      → REJECT (the default branch — renaming the status list breaks this silently, BR-DLV-001)
   │   - confirmation result is not checked before posting the journal (GAP-APP-018)
   │   - success and failure show the identical message; both-fail shows nothing (GAP-APP-018)
   │   - chart-of-accounts precondition is commented out (unknown intent)
   ▼
Returns — ProformaorInvoiceReturn / ReturnProductCustomerByInvoice → sp_ProformaReturnPosting /
          sp_Sales_DeliverySalesReturnPosting (accounting; body UNKNOWN)
   ▼
Collection — CustomerPayment.aspx.cs → CustPaymentBLL.SaveCustPayment (per invoice, not per receipt —
             GAP-APP-017) → sp_UD_InvoicePayment (amount/status args discarded — GAP-APP-016)
   ▼
Customer ledger / AR / GL — entirely inside stored procedures and the external *_ACCDB databases (UNKNOWN,
             see docs/audit/accounting-integration-audit.md)
```

## 2. What is CONFIRMED at each boundary

| Boundary | Confirmed | Evidence |
|---|---|---|
| Customer → Proforma | Stock is consumed at proforma-save time, not at delivery. FEFO order (`ORDER BY ExpDate ASC`). One grid line can produce 1..N `tblInvoiceDetail` rows (one per batch). | `specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md` Stage 5, re-read directly |
| Proforma → Delivery confirmation | The invoice number that identifies which invoice to confirm is read from `orderGridView.Rows[rowindex].Cells[7].Text` — a **positional grid cell**, not a data key. Inserting/reordering a grid column silently confirms the wrong invoice. | `specs/delivery-logistics/workflows/WF-delivery-confirmation.md`, `DelivaryInvoiceCreationAuto.aspx.cs::gotoinvoiceButton_Click` |
| Delivery confirmation → Accounting | The posting call (`sp_Sales_ExecuteDeliveryPipeline`) fires **unconditionally**, without checking whether the confirmation procedure that ran immediately before it succeeded. | same file, confirmed line-by-line in the workflow doc |
| Return → Stock/Accounting | Reversal procedures (`sp_DeleteDeliveryInvoice_FullRollback`, `sp_ProformaFullReturnPosting`) exist and are named as if they reverse stock+GL together, but their internals are UNKNOWN — whether they are idempotent or symmetric with the original posting cannot be verified without `sp_helptext` | `GAP-DB-001` |
| Collection → Ledger | One receipt paying N invoices calls the payment-save procedure N times, each stamped with the **full** receipt amount, not the per-invoice allocation (`GAP-APP-017`). Any downstream ledger/AR total built from `SUM(tblCustomerPay.PaymentAmount)` over-states cash collected by a factor of up to N. | `docs/audit/collection-audit.md`, `CustomerPayment.aspx.cs::saveButton_Click` |

## 3. Purchase-side flow

No distinct external-supplier purchase-to-pay module was found (see `docs/audit/module-audit.md` §"absent
modules" and `docs/audit/purchase-audit.md`). The closest analogues are inter-DC/warehouse transfer and
receive flows (`specs/inventory-warehouse/pages/PAGE-INV-017` NewStockTransferDcToDc,
`PAGE-INV-034`/`036` WarehouseStockIn/View, `specs/delivery-logistics` B2B transfer challan pages), which move
stock between the company's own warehouses/DCs rather than recording a supplier payable. **HUMAN VERIFICATION
REQUIRED**: confirm with the business whether supplier purchasing genuinely happens in one of the four external
databases (`ZAS_ACCDB`, `CSTL_ACCDB`, `CSTL_ERPDB`) rather than in this application at all.

## 4. Cross-module impact of the two biggest confirmed defects

**A. Non-atomic, non-transactional multi-step saves (`BR-SYS-004`, confirmed system-wide in
`docs/audit/transaction-integrity-audit.md`)** — a single root cause with impact across:
Invoice (partial header+detail) → Stock (partially decremented, `GAP-APP-005`) → Delivery confirmation
(posted on unconfirmed state, `GAP-APP-018`) → Sales/COGS reports (reflect the partial state) →
Accounting (posted from a state that may not match what the user saw) → Reconciliation (cannot tie out, since
the break point is silent).

**B. One-receipt-produces-N-full-amount-rows (`GAP-APP-017`)** — impact across:
Collection screen (correct amount shown to the user) → `tblCustomerPay` (N rows, each overstated) →
Customer ledger / outstanding / aging reports (overstated collections → understated outstanding, opposite-
direction misstatement from what a collections shortfall would cause) → AR reconciliation (will not tie to
bank deposits) → GL (if GL posting sums this same table, the misstatement propagates into the general ledger).
See `docs/audit/customer-ledger-audit.md` for the quantified version of this chain.

## 5. What remains UNKNOWN

Everything that happens **inside** `sp_ProformaPosting`, `sp_Sales_ExecuteDeliveryPipeline`,
`sp_Sales_DeliverySalesPosting`, `sp_ProformaReturnPosting`, `sp_Sales_DeliverySalesReturnPosting`,
`sp_UD_InvoicePayment`, and every write these procedures make into `ZAS_ACCDB`/`CSTL_ACCDB`/`CSTL_ERPDB` is
**HUMAN VERIFICATION REQUIRED — needs sp_helptext for each name**, per `GAP-DB-001` (see
`docs/audit/system-audit.md` §3 for why this audit could not pull them itself).
