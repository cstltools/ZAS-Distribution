# Module: delivery-logistics

Getting goods from a proforma invoice to the customer: picking (warehouse and sales-centre), top sheets and batch top sheets, delivery challans, delivery confirmation (full / partial / reject), return acknowledgement notes, B2B transfer challans and a rollback screen for logistics.

Delivery confirmation is the state change that turns a proforma invoice into a real sale. It is documented in [workflows/WF-delivery-confirmation.md](workflows/WF-delivery-confirmation.md).

## Pages (19)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `DLV-001` | `B2BTransferChallanRcvReport.aspx` | `/SInventory_UI/B2BTransferChallanRcvReport.aspx` | Report parameters | 24 | 1 | 8 | yes | [spec](pages/PAGE-DLV-001-B2BTransferChallanRcvReport.md) |
| `DLV-002` | `B2BTransferChallanReport.aspx` | `/SInventory_UI/B2BTransferChallanReport.aspx` | Report parameters | 23 | 1 | 8 | yes | [spec](pages/PAGE-DLV-002-B2BTransferChallanReport.md) |
| `DLV-003` | `BatchWiseDeliveryTopSheet.aspx` | `/SInventory_UI/BatchWiseDeliveryTopSheet.aspx` | Report parameters | 23 | 1 | 10 | yes | [spec](pages/PAGE-DLV-003-BatchWiseDeliveryTopSheet.md) |
| `DLV-004` | `BatchWiseTopSheet.aspx` | `/SInventory_UI/BatchWiseTopSheet.aspx` | Report parameters | 23 | 1 | 10 | yes | [spec](pages/PAGE-DLV-004-BatchWiseTopSheet.md) |
| `DLV-005` | `ChallanReportList.aspx` | `/SInventory_UI/ChallanReportList.aspx` | List / view | 18 | 1 | 4 | yes | [spec](pages/PAGE-DLV-005-ChallanReportList.md) |
| `DLV-006` | `CreatePickingDetailForWareHouse.aspx` | `/SInventory_UI/CreatePickingDetailForWareHouse.aspx` | Entry / transaction | 41 | 1 | 15 | yes | [spec](pages/PAGE-DLV-006-CreatePickingDetailForWareHouse.md) |
| `DLV-007` | `CreatePickingOnWareHouse.aspx` | `/SInventory_UI/CreatePickingOnWareHouse.aspx` | Entry / transaction | 13 | 1 | 2 | yes | [spec](pages/PAGE-DLV-007-CreatePickingOnWareHouse.md) |
| `DLV-008` | `DelivaryTopSheetGenerate.aspx` | `/SInventory_UI/DelivaryTopSheetGenerate.aspx` | Report parameters | 23 | 1 | 8 | yes | [spec](pages/PAGE-DLV-008-DelivaryTopSheetGenerate.md) |
| `DLV-009` | `DeliveryReturnReport.aspx` | `/SInventory_UI/DeliveryReturnReport.aspx` | Report parameters | 15 | 0 | 9 | yes | [spec](pages/PAGE-DLV-009-DeliveryReturnReport.md) |
| `DLV-010` | `LogisticeRollBackOperation.aspx` | `/SInventory_UI/LogisticeRollBackOperation.aspx` | Entry / transaction | 34 | 3 | 6 | yes | [spec](pages/PAGE-DLV-010-LogisticeRollBackOperation.md) |
| `DLV-011` | `PickingSlipGenerate.aspx` | `/SInventory_UI/PickingSlipGenerate.aspx` | Entry / transaction | 25 | 1 | 14 | yes | [spec](pages/PAGE-DLV-011-PickingSlipGenerate.md) |
| `DLV-012` | `ReceiveProductByChalanByDC.aspx` | `/SInventory_UI/ReceiveProductByChalanByDC.aspx` | Entry / transaction | 32 | 1 | 12 | yes | [spec](pages/PAGE-DLV-012-ReceiveProductByChalanByDC.md) |
| `DLV-013` | `ReturnAcknowledgmentNote.aspx` | `/SInventory_UI/ReturnAcknowledgmentNote.aspx` | Entry / transaction | 66 | 1 | 35 | yes | [spec](pages/PAGE-DLV-013-ReturnAcknowledgmentNote.md) |
| `DLV-014` | `ReturnAcknowledgmentNoteView.aspx` | `/SInventory_UI/ReturnAcknowledgmentNoteView.aspx` | List / view | 2 | 0 | 1 | yes | [spec](pages/PAGE-DLV-014-ReturnAcknowledgmentNoteView.md) |
| `DLV-015` | `ReturnOnDeliveryReport.aspx` | `/SInventory_UI/ReturnOnDeliveryReport.aspx` | Report parameters | 21 | 1 | 10 | yes | [spec](pages/PAGE-DLV-015-ReturnOnDeliveryReport.md) |
| `DLV-016` | `SCPickingGenerate.aspx` | `/SInventory_UI/SCPickingGenerate.aspx` | Entry / transaction | 22 | 1 | 10 | yes | [spec](pages/PAGE-DLV-016-SCPickingGenerate.md) |
| `DLV-017` | `TopSheetGenerate.aspx` | `/SInventory_UI/TopSheetGenerate.aspx` | Report parameters | 30 | 1 | 15 | yes | [spec](pages/PAGE-DLV-017-TopSheetGenerate.md) |
| `DLV-018` | `TransferReceiveProductByChalanByDC.aspx` | `/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` | Entry / transaction | 28 | 1 | 11 | yes | [spec](pages/PAGE-DLV-018-TransferReceiveProductByChalanByDC.md) |
| `DLV-019` | `WareHousePickingReportList.aspx` | `/SInventory_UI/WareHousePickingReportList.aspx` | List / view | 18 | 1 | 4 | yes | [spec](pages/PAGE-DLV-019-WareHousePickingReportList.md) |

## Workflows

End-to-end traces that span several pages and layers. Read these before the individual page specs — they carry the algorithm; the page specs carry the inventory.

- [WF-002 — Delivery confirmation (Full / Partial / Reject)](workflows/WF-delivery-confirmation.md)

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CentralStoreCurrentStock` | 1 |
| `View_CustomerMaster` | 5 |
| `tblArea` | 3 |
| `tblCentralStore` | 4 |
| `tblChalanDetail` | 2 |
| `tblChalanInfo` | 5 |
| `tblCompanyInfo` | 21 |
| `tblCompanyUnit` | 33 |
| `tblCustMaster` | 9 |
| `tblDCStore` | 7 |
| `tblDCStoreFreeze` | 2 |
| `tblDcWiseAreaInfo` | 2 |
| `tblDeliveryManInfo` | 10 |
| `tblEmpGeneralInfo` | 11 |
| `tblGroupInformation` | 3 |
| `tblInvoice` | 12 |
| `tblInvoiceBatch` | 8 |
| `tblInvoiceDetail` | 3 |
| `tblMIOInfo` | 5 |
| `tblManufacturer` | 1 |
| `tblMarket` | 10 |
| `tblOrder` | 2 |
| `tblProduct` | 9 |
| `tblProductCase` | 2 |
| `tblRegion` | 3 |
| `tblRequisition` | 17 |
| `tblRequsitionChild` | 9 |
| `tblReturnReason` | 2 |
| `tblStockInTransfar` | 10 |
| `tblTerritory` | 3 |
| `tblUnitPrice` | 4 |
| `tblUserCompanyUnit` | 15 |
| `tblWHStockInDetail` | 4 |
| `tblWHStockInMaster` | 3 |
| `tblWearhouse` | 5 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_DeliveryConfirmationFullPosting` | 1 |
| `sp_Process_WeightedAverage` | 2 |
| `sp_Process_WeightedAverageFreezeStock` | 1 |
| `sp_UD_ReceiveIdInDc` | 4 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
