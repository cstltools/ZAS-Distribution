# Status Lifecycle

## The most important fact

**The approval status vocabulary is data, not code.** Approval screens write the *label of the selected radio
button* straight into the status column:

```csharp
// Solution.Web/SInventory_UI/DestroyProductApproval.aspx.cs
var dao = new DestroyProductDao {
    ActionStatus = RadioButtonList.SelectedItem.Text,   // <-- the text of a tblAction row
    ApproveBy    = Session["LoginName"].ToString(),
    ApproveDate  = DateTime.Today,
};
```

and the radio list is bound from `tblAction` (`SELECT * FROM tblAction WHERE IsShow = 1`, display `ActionText`,
value `ActionId`) by `ClsApprovalAction.LoadActionControlByUser`. The same pattern appears in
`CustomerCreditLimitApproval`, `ProUnitPriceApprove`, `ManualOrderApproval`, `WarehouseStockInApproval`,
`FixedDiscountApproval`, `DiscountApproval`, `SampleProductIssueApproval`, `SlaveInfoApproval`,
`ProductQuotedPriceApproval`, `NonTranscationalInvoiceApproval`, `ReturnustomerByInvoiceApproval`,
`TradeMaterialTransferApproval`, `PettyCashApproval` and `DestroyProductApproval`.

Consequences:

- **The complete set of approval status values is UNKNOWN** — it is whatever rows exist in `tblAction`.
- Editing an `ActionText` value in `tblAction` changes the string written into every future status column while
  leaving historical rows on the old spelling.
- Statuses are compared elsewhere as string literals (`ActionStatus = 'Accepted'` appears 32 times), so an
  `ActionText` that does not match those literals silently makes rows invisible to the rest of the system.

The values observed as literals in the source are listed below. They are **the values the code compares
against**, which is a lower bound on what may exist in the data.

## Status columns found in code

| Column | Tables (INFERRED from the SQL it appears in) | Literals compared/written in code |
|---|---|---|
| `ActionStatus` | `tblUnitPrice`, `tblProduct`, `tblProductDiscount`, `tblProductQuotedPrice`, `tblOrder`, `tblDestroyProduct`, `tblCustomerCreditLimit`, `tblCustomerFixedDiscount`, `tblSampleIssue`, `tblSlaveInfo`, `tblWHStockInMaster`, `tblTradeMaterialTransfer`, `tblPettyCashMaster`, `tblNonTranscationalInvoiceMaster`, and more | `'Accepted'`, `'Posted'`, `'Cancel'`, `'Delete'`, `'Inactive'` |
| `DeliveryInvoiceStatus` | `tblInvoice` | `'Full'`, `'Partial'` |
| `DeliveryStatus` | `tblInvoice`, delivery tables | `'Full'`, `'Partial'`, `'Reject'` |
| `PaymentStatus` | `tblInvoice`, `tblCustomerPay` | `'Partial'` (the only literal in code) |
| `Status` | `tblMenuDistribution`, `tblChalanInfo`, order/requisition tables, `tblPaymentTranscationMaster` | `'Posted'`, `'Full'`, `'Partial'`, `'Active'`, `'Reject'`, `'Approved'`, `'Undelivered'`, `'True'` |
| `UserStatus` | `tblUser` | `'active'` (lower case) |
| `IsActive` | most master tables | `'True'`, `'TRUE'`, `'1'`, `'False'` — **inconsistent, see `BR-SYS-005`** |
| `ApprovalStatus` | `tblWHStockInMaster` (via `sp_Acc_MaterialConversionReceive` parameters) | UNKNOWN (passed as a parameter) |

## Lifecycle 1 — generic master-data approval

Applies to unit price, product discount, quoted price, fixed discount, credit limit, slave info, sample issue,
petty cash, destroy product, trade material transfer, warehouse stock-in.

```
(row created by the *Entry* page)
      ActionStatus = <initial>            ← UNKNOWN: the entry pages do not set it explicitly in most cases,
                                            so the default comes from the table DDL, which is not in the repo
      │
      │  *Approval page*: user ticks rows, picks an action from the radio list, clicks Submit
      ▼
      ActionStatus = <tblAction.ActionText of the chosen action>
      ApproveBy    = Session["LoginName"]
      ApproveDate  = DateTime.Today
      │
      ├── if the chosen text is exactly "Cancel"  → status is written, but NO side effect runs
      └── otherwise                                → side effects run (stock update, journal posting)
```

The `"Cancel"` special case is **hard-coded by string comparison** in the approval pages
(`if (RadioButtonList.SelectedItem.Text.Trim() != "Cancel")`) and again in
`ClsApprovalAction.LoadActionControlByUser` (a `"Cancel"` item is left enabled even when the user has no
grant). Renaming that `tblAction` row breaks both. (`BR-SYS-008`, CONFIRMED.)

Downstream consumers filter on `ActionStatus = 'Accepted'` — for example
`InvoiceDAL.ProductInfoDAL` will not return a price unless
`UP.ActionStatus = 'Accepted' AND UP.IsActive = 'TRUE'`. So an approval action whose text is not literally
`Accepted` leaves the record approved-but-invisible.

## Lifecycle 2 — order → proforma invoice → delivery

This is the core order-to-cash path. Everything below the dashed line happens inside stored procedures whose
bodies are **not in the repository**.

```
ORDER          tblOrder / tblOrderDetail
               ManualOrderrCreation.aspx  /  OrderRequisitionCreation.aspx
                     │  ManualOrderApproval.aspx sets ActionStatus
                     ▼
PROFORMA       tblInvoice + tblInvoiceDetail          InvoiceCreationForCustomer(ByOrder).aspx
INVOICE        stock consumed from tblDCStore by expiry order (FEFO)
                     │
                     │  InvoiceDAL.PostJournal(invoiceId)
                     └────────────────────► EXEC sp_ProformaPosting @InvoiceId
                     ▼
DELIVERY       DelivaryInvoiceCreationAuto.aspx / …ForCustomerByOrderAuto.aspx
CONFIRMATION   writes DeliveryTpTotal, DeliveryTpDiscount, DeliveryTpVat, DeliveryTpGrandTotal,
               DelivaryInvoiceNo, DelivarySpecialAmount, UpdateBy, UpdateDate on tblInvoice
                     │
      ┌──────────────┼───────────────────────────────┐
      │ FULL         │ PARTIAL                       │ REJECT
      ▼              ▼                               ▼
 DeliveryInvoiceStatus='Full'   ='Partial'      DeliveryStatus='Reject'
 sp_DeliveryConformationFull    sp_Sales_       sp_DeliveryConformationReject
   ( @InvoiceNo,@UpdateBy,        DeliverySales   ( @InvoiceNo,@UpdateBy,@UpdateDate,
     @UpdateDate )                Posting          @ReturnReason )
        │                         (@InvoiceId)
        ▼
 sp_Sales_ExecuteDeliveryPipeline (@InvoiceId)   ← the live posting call in
                                                   InvoiceDAL.PostDeliveryConfFullJournal;
                                                   sp_DeliveryConfirmationFullPosting and
                                                   sp_Sales_DeliverySalesPosting are the two
                                                   superseded versions, left commented out
────────────────────────────────────────────────────────────────────────────────────────────
everything below this line is UNKNOWN — SP bodies absent
```

Reversal / deletion paths that exist as procedure names:
`sp_DeleteDeliveryInvoice`, `sp_DeleteDeliveryInvoice_FullRollback`, `sp_DeliveryInvoiceDeletePosting`,
`sp_Delete_ProformaInvoice`, `sp_Deletenvoice` (sic), `sp_DeleteOrder`.
`UpdateInvoiceNull` in `InvoiceDAL` nulls every delivery column back out, which is the application-side half of
an un-deliver.

## Lifecycle 3 — returns

```
Proforma-stage return   ProformaorInvoiceReturn.aspx  → sp_ProformaReturnPosting (@InvoiceId)
                        full return                   → sp_ProformaFullReturnPosting (@InvoiceId)
Delivered-stage return  ReturnProductCustomerByInvoice.aspx → tblCustomerReturnMaster / …Detail
                        ReturnustomerByInvoiceApproval.aspx → approval (lifecycle 1)
                                                            → sp_Sales_DeliverySalesReturnPosting
Return acknowledgement  ReturnAcknowledgmentNote.aspx → sp_RAN_MasterInsert / sp_RAN_DetailInsert
                                                       sp_RAN_DeleteById
```

Returned stock re-enters through `tblDCStoreFreeze` (a quarantine tier), guarded by
`InvoiceBLL.SaveDCStoreFreeze`, which only inserts when
`InvoiceDAL.CheckReturnExistOrNotInFreezeStore` finds no existing row — i.e. **returns are idempotent per
invoice-detail line**. Release from freeze is `WhFreezeStockRelease.aspx` → `sp_FreezeStockReleasePosting`;
putting stock into freeze is `StockConditionFreeze.aspx` / `WhStockConditionFreeze.aspx` →
`sp_FreezeStockPosting`.

## Lifecycle 4 — customer payment

```
CustomerPayment.aspx / MarketWiseCustomerPayment.aspx
   selects open invoices, applies an amount per invoice
   → tblCustomerPay + tblCustPayDetail
   → PaymentStatus = 'Partial' on the invoice while the balance is non-zero
   → sp_Sales_CustomerPaymentPosting (accounting)

Batching        InvoiceBatchEntry.aspx / BatchWiseCollection.aspx
                → sp_Save_PaymentBatchMaster, sp_Save_PaymentBatchDetail, sp_Save_BatchCode,
                  sp_Save_PayCode, sp_PaymentBatchLockProcess          ← "lock" = a closed batch
Deposit         DepositSlipEntry.aspx → sp_I_CompanyWiseDeposit
Money receipt   → sp_MRCP_MoneyReceiptMasterInsert / sp_MRCP_DeletePayment
Reversal        PaymentReverse.aspx → sp_Sales_CustomerPaymentReversePosting
Duplicates      DuplicatePaymentList.aspx → sp_DEL_DuplicatePayment
```

The exact `PaymentStatus` values other than `'Partial'` are **UNKNOWN** — the "fully paid" value is set inside
a stored procedure, not in C#.

## Lifecycle 5 — warehouse / DC stock

```
Goods receipt   WarehouseStockIn.aspx → tblWHStockInMaster/Detail
                WarehouseStockInApproval.aspx → sp_WHStockInPosting, sp_Acc_MaterialConversionReceive
                MIGOReport / ExcelUpForMIGO → sp_StockInMIGOtoCentralStore  (from ERP MIGO)
Issue           WarehouseStockOut.aspx, DirectStockOut.aspx → sp_I_DCStockOutInfo
Transfer        NewStockTransferDcToDc.aspx, TransferStockReceiveByDC.aspx, B2B transfer
                → sp_Process_WeightedAverageB2BTransfer
Freeze          StockConditionFreeze / WhStockConditionFreeze → sp_FreezeStockPosting
Release         WhFreezeStockRelease → sp_FreezeStockReleasePosting
Destroy         DestroyProduct.aspx → DestroyProductApproval.aspx → sp_Sales_ProductDestroyPosting
Conversion      MaterialConversionReceive.aspx → sp_Acc_MaterialConversionReceive (reads CSTL_ERPDB)
Valuation       sp_Process_WeightedAverage, sp_INV_WeightedAverage_Report → tbl_INV_WeightedAvg
```

## What is genuinely UNKNOWN about status

1. **Initial values.** Entry screens mostly do not set a status; the default is in the table DDL, which is
   absent. So "what status does a new record start in" is UNKNOWN for most entities.
2. **The full value set.** Driven by `tblAction` rows.
3. **Transitions performed inside procedures.** 119 procedures, no bodies. Every "→ posted" arrow above stops
   at the procedure boundary.
4. **Who may perform each transition.** Data in `tblActionUserWiseApproval` — see
   [role-permission.md](role-permission.md).

To close 1–3, supply `SELECT * FROM tblAction`, the DDL for the tables above (defaults included), and
`sp_helptext` for the procedures named in [stored-procedures.md](stored-procedures.md).
