# Module: sales-order-invoice

Order capture through to proforma invoice, plus sample and non-transactional invoices.

The central behaviour of the whole application lives here: an invoice line is expanded into **several** `tblInvoiceDetail` rows by consuming `tblDCStore` batches in expiry order (FEFO). It is documented step by step in [workflows/WF-invoice-batch-allocation.md](workflows/WF-invoice-batch-allocation.md).

There are several near-duplicate creation screens (`InvoiceCreationForCustomer`, `…ByOrder`, `…ForConsumerCustomer`, `DelivaryInvoiceCreationAuto`, `…ForCustomerByOrderAuto`). They share the pattern but not the code; each has its own spec. Which are live is decided by `tblMainMenu` and is UNKNOWN.

Known defects that originate in this module: `GAP-APP-002` (displayed discount ≠ stored discount), `GAP-APP-004` (no stock availability check), `GAP-APP-005` (lost updates on stock), `GAP-APP-007` (non-atomic document save).

## Pages (27)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `SAL-001` | `DelivaryInvoiceCreationAuto.aspx` | `/SInventory_UI/DelivaryInvoiceCreationAuto.aspx` | Entry / transaction | 42 | 1 | 13 | yes | [spec](pages/PAGE-SAL-001-DelivaryInvoiceCreationAuto.md) |
| `SAL-002` | `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` | `/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` | Entry / transaction | 93 | 1 | 35 | yes | [spec](pages/PAGE-SAL-002-DelivaryInvoiceCreationForCustomerByOrderAuto.md) |
| `SAL-003` | `ErrorInvoiceList.aspx` | `/SInventory_UI/ErrorInvoiceList.aspx` | List / view | 16 | 1 | 5 | yes | [spec](pages/PAGE-SAL-003-ErrorInvoiceList.md) |
| `SAL-004` | `GroupInvoiceEntry.aspx` | `/SInventory_UI/GroupInvoiceEntry.aspx` | Entry / transaction | 33 | 1 | 18 | yes | [spec](pages/PAGE-SAL-004-GroupInvoiceEntry.md) |
| `SAL-005` | `GroupInvoiceList.aspx` | `/SInventory_UI/GroupInvoiceList.aspx` | List / view | 25 | 1 | 17 | yes | [spec](pages/PAGE-SAL-005-GroupInvoiceList.md) |
| `SAL-006` | `InvoiceAutoPrint.aspx` | `/SInventory_UI/InvoiceAutoPrint.aspx` | Entry / transaction | 26 | 1 | 14 | yes | [spec](pages/PAGE-SAL-006-InvoiceAutoPrint.md) |
| `SAL-007` | `InvoiceBatchEntry.aspx` | `/SInventory_UI/InvoiceBatchEntry.aspx` | Entry / transaction | 27 | 1 | 12 | yes | [spec](pages/PAGE-SAL-007-InvoiceBatchEntry.md) |
| `SAL-008` | `InvoiceCreationByOrder.aspx` | `/SInventory_UI/InvoiceCreationByOrder.aspx` | Entry / transaction | 27 | 1 | 13 | yes | [spec](pages/PAGE-SAL-008-InvoiceCreationByOrder.md) |
| `SAL-009` | `InvoiceCreationForConsumerCustomer.aspx` | `/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` | Entry / transaction | 68 | 1 | 32 | yes | [spec](pages/PAGE-SAL-009-InvoiceCreationForConsumerCustomer.md) |
| `SAL-010` | `InvoiceCreationForCustomer.aspx` | `/SInventory_UI/InvoiceCreationForCustomer.aspx` | Entry / transaction | 76 | 1 | 22 | yes | [spec](pages/PAGE-SAL-010-InvoiceCreationForCustomer.md) |
| `SAL-011` | `InvoiceCreationForCustomerByOrder.aspx` | `/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` | Entry / transaction | 101 | 1 | 43 | yes | [spec](pages/PAGE-SAL-011-InvoiceCreationForCustomerByOrder.md) |
| `SAL-012` | `ManualOrderApproval.aspx` | `/SInventory_UI/ManualOrderApproval.aspx` | Approval | 39 | 2 | 11 | yes | [spec](pages/PAGE-SAL-012-ManualOrderApproval.md) |
| `SAL-013` | `ManualOrderView.aspx` | `/SInventory_UI/ManualOrderView.aspx` | List / view | 17 | 1 | 6 | yes | [spec](pages/PAGE-SAL-013-ManualOrderView.md) |
| `SAL-014` | `ManualOrderrCreation.aspx` | `/SInventory_UI/ManualOrderrCreation.aspx` | Entry / transaction | 67 | 1 | 33 | yes | [spec](pages/PAGE-SAL-014-ManualOrderrCreation.md) |
| `SAL-015` | `Non-TranscationalInvoicePayment.aspx` | `/SInventory_UI/Non-TranscationalInvoicePayment.aspx` | Entry / transaction | 29 | 1 | 12 | yes | [spec](pages/PAGE-SAL-015-Non-TranscationalInvoicePayment.md) |
| `SAL-016` | `NonTranscationalInvoiceApproval.aspx` | `/SInventory_UI/NonTranscationalInvoiceApproval.aspx` | Approval | 20 | 1 | 8 | yes | [spec](pages/PAGE-SAL-016-NonTranscationalInvoiceApproval.md) |
| `SAL-017` | `NonTranscationalInvoiceReport.aspx` | `/SInventory_UI/NonTranscationalInvoiceReport.aspx` | Report parameters | 30 | 1 | 15 | yes | [spec](pages/PAGE-SAL-017-NonTranscationalInvoiceReport.md) |
| `SAL-018` | `NonTranscationalInvoiceReturn.aspx` | `/SInventory_UI/NonTranscationalInvoiceReturn.aspx` | Entry / transaction | 66 | 1 | 35 | yes | [spec](pages/PAGE-SAL-018-NonTranscationalInvoiceReturn.md) |
| `SAL-019` | `OrderDeleteorEdit.aspx` | `/SInventory_UI/OrderDeleteorEdit.aspx` | Edit | 15 | 1 | 10 | yes | [spec](pages/PAGE-SAL-019-OrderDeleteorEdit.md) |
| `SAL-020` | `OrderRequisitionCreation.aspx` | `/SInventory_UI/OrderRequisitionCreation.aspx` | Entry / transaction | 37 | 1 | 24 | yes | [spec](pages/PAGE-SAL-020-OrderRequisitionCreation.md) |
| `SAL-021` | `OrderRequisitionView.aspx` | `/SInventory_UI/OrderRequisitionView.aspx` | List / view | 14 | 1 | 5 | yes | [spec](pages/PAGE-SAL-021-OrderRequisitionView.md) |
| `SAL-022` | `OrderStatus.aspx` | `/SInventory_UI/OrderStatus.aspx` | Entry / transaction | 54 | 3 | 10 | yes | [spec](pages/PAGE-SAL-022-OrderStatus.md) |
| `SAL-023` | `ProformaorInvoiceReturn.aspx` | `/SInventory_UI/ProformaorInvoiceReturn.aspx` | Entry / transaction | 11 | 0 | 8 | yes | [spec](pages/PAGE-SAL-023-ProformaorInvoiceReturn.md) |
| `SAL-024` | `ReturnInvoiceCreationForCustomer.aspx` | `/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx` | Entry / transaction | 91 | 1 | 30 | yes | [spec](pages/PAGE-SAL-024-ReturnInvoiceCreationForCustomer.md) |
| `SAL-025` | `SampleInvoiceList.aspx` | `/SInventory_UI/SampleInvoiceList.aspx` | List / view | 29 | 1 | 15 | yes | [spec](pages/PAGE-SAL-025-SampleInvoiceList.md) |
| `SAL-026` | `SampleProductIssueApproval.aspx` | `/SInventory_UI/SampleProductIssueApproval.aspx` | Approval | 24 | 1 | 9 | yes | [spec](pages/PAGE-SAL-026-SampleProductIssueApproval.md) |
| `SAL-027` | `SampleProductrIssue.aspx` | `/SInventory_UI/SampleProductrIssue.aspx` | Entry / transaction | 67 | 1 | 36 | yes | [spec](pages/PAGE-SAL-027-SampleProductrIssue.md) |

## Workflows

End-to-end traces that span several pages and layers. Read these before the individual page specs — they carry the algorithm; the page specs carry the inventory.

- [WF-001 — Invoice creation and FEFO batch allocation](workflows/WF-invoice-batch-allocation.md)

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CustomerMaster` | 1 |
| `View_DCStoreCurrentStock` | 10 |
| `View_InvoiceWiseDue` | 1 |
| `tblASMInfo` | 4 |
| `tblAction` | 2 |
| `tblActionPageWiseStep` | 4 |
| `tblActionStepDetail` | 4 |
| `tblActionSteps` | 4 |
| `tblActionUserWiseApproval` | 4 |
| `tblArea` | 20 |
| `tblBonusCampgainDetail` | 1 |
| `tblCompanyInfo` | 21 |
| `tblCompanyUnit` | 49 |
| `tblConsumerCustomer` | 3 |
| `tblCustCategory` | 6 |
| `tblCustMaster` | 38 |
| `tblCustPayDetail` | 7 |
| `tblCustomerCreditLimit` | 3 |
| `tblCustomerFixedDiscount` | 2 |
| `tblCustomerPay` | 1 |
| `tblCustomerReturnMaster` | 1 |
| `tblDCStore` | 9 |
| `tblDCStoreFreeze` | 5 |
| `tblDcWiseAreaInfo` | 14 |
| `tblDeliveryManInfo` | 10 |
| `tblDestroyProduct` | 2 |
| `tblEmpGeneralInfo` | 19 |
| `tblFocDetails` | 1 |
| `tblFocMaster` | 1 |
| `tblInvoice` | 54 |
| `tblInvoiceBatch` | 4 |
| `tblInvoiceDetail` | 16 |
| `tblMIOInfo` | 21 |
| `tblMainMenu` | 4 |
| `tblManufacturer` | 2 |
| `tblMarket` | 30 |
| `tblNonTranscationalInvoiceMaster` | 1 |
| `tblOrder` | 35 |
| `tblOrderDetail` | 15 |
| `tblPaymentType` | 5 |
| `tblPettyCashDetails` | 1 |
| `tblPettyCashMaster` | 2 |
| `tblProduct` | 26 |
| `tblProductDiscount` | 14 |
| `tblRSMInfo` | 4 |
| `tblRegion` | 12 |
| `tblRequisition` | 3 |
| `tblRequsitionChild` | 3 |
| `tblReturnInvoice` | 2 |
| `tblReturnInvoiceDetail` | 1 |
| `tblReturnReason` | 4 |
| `tblSampleIssue` | 4 |
| `tblSampleIssueDetail` | 3 |
| `tblSlaveInfo` | 1 |
| `tblTerritory` | 20 |
| `tblTradePolicy` | 2 |
| `tblTradePolicyNew` | 5 |
| `tblUnitPrice` | 16 |
| `tblUser` | 1 |
| `tblUserCompanyUnit` | 11 |
| `tblWHStockInDetail` | 3 |
| `tblWHStockInMaster` | 3 |
| `tblWearhouse` | 5 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_DeleteDeliveryInvoice` | 1 |
| `sp_DeleteDeliveryInvoice_FullRollback` | 1 |
| `sp_DeleteOrder` | 1 |
| `sp_Delete_ProformaInvoice` | 4 |
| `sp_Deletenvoice` | 2 |
| `sp_DeliveryConfirmationFullPosting` | 2 |
| `sp_DeliveryConfirmationPartiallPosting` | 1 |
| `sp_DeliveryConformationFull` | 1 |
| `sp_DeliveryConformationReject` | 1 |
| `sp_DeliveryInvoiceDeletePosting` | 1 |
| `sp_FreezeStockPosting` | 1 |
| `sp_I_CustomerGl` | 1 |
| `sp_I_InvoiceMaster` | 1 |
| `sp_I_OrderMaster` | 2 |
| `sp_I_SampleProductIssue` | 2 |
| `sp_I_SampleProductIssueDetail` | 2 |
| `sp_Integration_CustomerSubsidaryInsert` | 1 |
| `sp_Process_WeightedAverageFreezeStock` | 3 |
| `sp_ProformaFullReturnPosting` | 1 |
| `sp_ProformaPosting` | 3 |
| `sp_ProformaReturnPosting` | 4 |
| `sp_Sales_DeliverySalesPosting` | 3 |
| `sp_Sales_ExecuteDeliveryPipeline` | 2 |
| `sp_Sales_ProductDestroyPosting` | 1 |
| `sp_SampleInvoicePosting` | 1 |
| `sp_Save_BatchCode` | 1 |
| `sp_UD_SampleIssue` | 1 |
| `sp_UD_SampleStockIssue` | 1 |
| `sp_WHStockInPosting` | 1 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
