# Module: inventory-warehouse

Two-tier stock: a central warehouse / central store, and per-unit distribution-centre stock, both batch- and expiry-tracked.

Movements covered here: goods receipt (`WarehouseStockIn` + approval, MIGO import from the ERP), issue (`WarehouseStockOut`, `DirectStockOut`), DC-to-DC and B2B transfer, receipt against a challan, requisition/issue, freeze and release (a quarantine tier in `tblDCStoreFreeze` / `tblWhStoreFreeze`), destruction with approval, and material conversion received from `CSTL_ERPDB`.

Costing is weighted average, run *after* the movement by `sp_Process_WeightedAverage` and its variants. `BR-INV-005` records that DC-store weighted average was deliberately switched off.

Almost every posting here ends in a stored procedure whose body is not in the repository, so the accounting effect of each movement is UNKNOWN.

## Pages (41)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `INV-001` | `DCStockInfoReport.aspx` | `/SInventory_UI/DCStockInfoReport.aspx` | Report parameters | 8 | 0 | 11 | yes | [spec](pages/PAGE-INV-001-DCStockInfoReport.md) |
| `INV-002` | `DCStockReport.aspx` | `/SInventory_UI/DCStockReport.aspx` | Report parameters | 9 | 0 | 8 | yes | [spec](pages/PAGE-INV-002-DCStockReport.md) |
| `INV-003` | `DCStockValuationReport.aspx` | `/SInventory_UI/DCStockValuationReport.aspx` | Report parameters | 13 | 0 | 9 | yes | [spec](pages/PAGE-INV-003-DCStockValuationReport.md) |
| `INV-004` | `DestroyProduct.aspx` | `/SInventory_UI/DestroyProduct.aspx` | Entry / transaction | 22 | 1 | 12 | yes | [spec](pages/PAGE-INV-004-DestroyProduct.md) |
| `INV-005` | `DestroyProductApproval.aspx` | `/SInventory_UI/DestroyProductApproval.aspx` | Approval | 23 | 1 | 8 | yes | [spec](pages/PAGE-INV-005-DestroyProductApproval.md) |
| `INV-006` | `DestroyProductList.aspx` | `/SInventory_UI/DestroyProductList.aspx` | List / view | 19 | 1 | 8 | yes | [spec](pages/PAGE-INV-006-DestroyProductList.md) |
| `INV-007` | `DestroyProductReport.aspx` | `/SInventory_UI/DestroyProductReport.aspx` | Report parameters | 31 | 1 | 14 | yes | [spec](pages/PAGE-INV-007-DestroyProductReport.md) |
| `INV-008` | `DirectStockOut.aspx` | `/SInventory_UI/DirectStockOut.aspx` | Entry / transaction | 30 | 1 | 12 | yes | [spec](pages/PAGE-INV-008-DirectStockOut.md) |
| `INV-009` | `DirectStockOutView.aspx` | `/SInventory_UI/DirectStockOutView.aspx` | List / view | 26 | 1 | 8 | yes | [spec](pages/PAGE-INV-009-DirectStockOutView.md) |
| `INV-010` | `FreezeStockReport.aspx` | `/SInventory_UI/FreezeStockReport.aspx` | Report parameters | 8 | 0 | 9 | yes | [spec](pages/PAGE-INV-010-FreezeStockReport.md) |
| `INV-011` | `IssueRequisitionProducts.aspx` | `/SInventory_UI/IssueRequisitionProducts.aspx` | Entry / transaction | 36 | 1 | 11 | yes | [spec](pages/PAGE-INV-011-IssueRequisitionProducts.md) |
| `INV-012` | `MaterialConversionReceive.aspx` | `/SInventory_UI/MaterialConversionReceive.aspx` | Entry / transaction | 64 | 1 | 28 | yes | [spec](pages/PAGE-INV-012-MaterialConversionReceive.md) |
| `INV-013` | `MaterialConversionReceiveView.aspx` | `/SInventory_UI/MaterialConversionReceiveView.aspx` | List / view | 20 | 1 | 8 | yes | [spec](pages/PAGE-INV-013-MaterialConversionReceiveView.md) |
| `INV-014` | `NationalBeenCardReport.aspx` | `/SInventory_UI/NationalBeenCardReport.aspx` | Report parameters | 13 | 0 | 8 | yes | [spec](pages/PAGE-INV-014-NationalBeenCardReport.md) |
| `INV-015` | `NationalStockReport.aspx` | `/SInventory_UI/NationalStockReport.aspx` | Report parameters | 6 | 0 | 5 | yes | [spec](pages/PAGE-INV-015-NationalStockReport.md) |
| `INV-016` | `NearlyExpireProductReport.aspx` | `/SInventory_UI/NearlyExpireProductReport.aspx` | Report parameters | 10 | 0 | 7 | yes | [spec](pages/PAGE-INV-016-NearlyExpireProductReport.md) |
| `INV-017` | `NewStockTransferDcToDc.aspx` | `/SInventory_UI/NewStockTransferDcToDc.aspx` | Entry / transaction | 49 | 2 | 21 | yes | [spec](pages/PAGE-INV-017-NewStockTransferDcToDc.md) |
| `INV-018` | `OtherStockAction.aspx` | `/SInventory_UI/OtherStockAction.aspx` | Entry / transaction | 31 | 1 | 14 | yes | [spec](pages/PAGE-INV-018-OtherStockAction.md) |
| `INV-019` | `StockConditionFreeze.aspx` | `/SInventory_UI/StockConditionFreeze.aspx` | Entry / transaction | 36 | 1 | 16 | yes | [spec](pages/PAGE-INV-019-StockConditionFreeze.md) |
| `INV-020` | `StockReceive.aspx` | `/SInventory_UI/StockReceive.aspx` | Entry / transaction | 21 | 0 | 11 | yes | [spec](pages/PAGE-INV-020-StockReceive.md) |
| `INV-021` | `StockReceiveByDC.aspx` | `/SInventory_UI/StockReceiveByDC.aspx` | Entry / transaction | 16 | 1 | 7 | yes | [spec](pages/PAGE-INV-021-StockReceiveByDC.md) |
| `INV-022` | `StockReceiveReportByDc.aspx` | `/SInventory_UI/StockReceiveReportByDc.aspx` | Report parameters | 28 | 1 | 8 | yes | [spec](pages/PAGE-INV-022-StockReceiveReportByDc.md) |
| `INV-023` | `StockReceiveView.aspx` | `/SInventory_UI/StockReceiveView.aspx` | List / view | 10 | 1 | 5 | yes | [spec](pages/PAGE-INV-023-StockReceiveView.md) |
| `INV-024` | `StockReport.aspx` | `/SInventory_UI/StockReport.aspx` | Report parameters | 10 | 0 | 8 | yes | [spec](pages/PAGE-INV-024-StockReport.md) |
| `INV-025` | `TradeMaterialTransfer.aspx` | `/SInventory_UI/TradeMaterialTransfer.aspx` | Entry / transaction | 21 | 0 | 24 | yes | [spec](pages/PAGE-INV-025-TradeMaterialTransfer.md) |
| `INV-026` | `TradeMaterialTransferApproval.aspx` | `/SInventory_UI/TradeMaterialTransferApproval.aspx` | Approval | 23 | 1 | 6 | yes | [spec](pages/PAGE-INV-026-TradeMaterialTransferApproval.md) |
| `INV-027` | `TradeMaterialTransferList.aspx` | `/SInventory_UI/TradeMaterialTransferList.aspx` | List / view | 53 | 1 | 10 | yes | [spec](pages/PAGE-INV-027-TradeMaterialTransferList.md) |
| `INV-028` | `TransferStockReceiveByDC.aspx` | `/SInventory_UI/TransferStockReceiveByDC.aspx` | Entry / transaction | 16 | 1 | 7 | yes | [spec](pages/PAGE-INV-028-TransferStockReceiveByDC.md) |
| `INV-029` | `ViewRequisitionForIssue.aspx` | `/SInventory_UI/ViewRequisitionForIssue.aspx` | Entry / transaction | 13 | 1 | 2 | yes | [spec](pages/PAGE-INV-029-ViewRequisitionForIssue.md) |
| `INV-030` | `WHBeenCardReport.aspx` | `/SInventory_UI/WHBeenCardReport.aspx` | Report parameters | 13 | 0 | 8 | yes | [spec](pages/PAGE-INV-030-WHBeenCardReport.md) |
| `INV-031` | `WHStockInList.aspx` | `/SInventory_UI/WHStockInList.aspx` | List / view | 15 | 1 | 4 | yes | [spec](pages/PAGE-INV-031-WHStockInList.md) |
| `INV-032` | `WHStockInReportList.aspx` | `/SInventory_UI/WHStockInReportList.aspx` | List / view | 32 | 1 | 12 | yes | [spec](pages/PAGE-INV-032-WHStockInReportList.md) |
| `INV-033` | `WHStockValuationReport.aspx` | `/SInventory_UI/WHStockValuationReport.aspx` | Report parameters | 13 | 0 | 9 | yes | [spec](pages/PAGE-INV-033-WHStockValuationReport.md) |
| `INV-034` | `WarehouseStockIn.aspx` | `/SInventory_UI/WarehouseStockIn.aspx` | Entry / transaction | 63 | 1 | 31 | yes | [spec](pages/PAGE-INV-034-WarehouseStockIn.md) |
| `INV-035` | `WarehouseStockInApproval.aspx` | `/SInventory_UI/WarehouseStockInApproval.aspx` | Approval | 24 | 1 | 12 | yes | [spec](pages/PAGE-INV-035-WarehouseStockInApproval.md) |
| `INV-036` | `WarehouseStockInView.aspx` | `/SInventory_UI/WarehouseStockInView.aspx` | List / view | 20 | 1 | 8 | yes | [spec](pages/PAGE-INV-036-WarehouseStockInView.md) |
| `INV-037` | `WarehouseStockOut.aspx` | `/SInventory_UI/WarehouseStockOut.aspx` | Entry / transaction | 62 | 1 | 31 | yes | [spec](pages/PAGE-INV-037-WarehouseStockOut.md) |
| `INV-038` | `WeaightedAVG_Report.aspx` | `/SInventory_UI/WeaightedAVG_Report.aspx` | Report parameters | 23 | 1 | 18 | yes | [spec](pages/PAGE-INV-038-WeaightedAVG-Report.md) |
| `INV-039` | `WhFreezeStockRelease.aspx` | `/SInventory_UI/WhFreezeStockRelease.aspx` | Entry / transaction | 26 | 1 | 13 | yes | [spec](pages/PAGE-INV-039-WhFreezeStockRelease.md) |
| `INV-040` | `WhStockConditionFreeze.aspx` | `/SInventory_UI/WhStockConditionFreeze.aspx` | Entry / transaction | 33 | 1 | 12 | yes | [spec](pages/PAGE-INV-040-WhStockConditionFreeze.md) |
| `INV-041` | `WhStockMonitoringReport.aspx` | `/SInventory_UI/WhStockMonitoringReport.aspx` | Report parameters | 23 | 1 | 6 | yes | [spec](pages/PAGE-INV-041-WhStockMonitoringReport.md) |

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CentralStoreCurrentStock` | 1 |
| `tblCentralStore` | 11 |
| `tblCentralStore_OpeninigBalance` | 1 |
| `tblChalanDetail` | 1 |
| `tblChalanInfo` | 3 |
| `tblCompanyInfo` | 33 |
| `tblCompanyUnit` | 48 |
| `tblCustMaster` | 4 |
| `tblDCStore` | 12 |
| `tblDCStoreFreeze` | 11 |
| `tblDestroyProduct` | 1 |
| `tblDirectStockOut` | 2 |
| `tblGroupInformation` | 10 |
| `tblInvoice` | 3 |
| `tblInvoiceDetail` | 3 |
| `tblManufacturer` | 1 |
| `tblPackSize` | 1 |
| `tblProduct` | 18 |
| `tblProductCase` | 1 |
| `tblRequisition` | 7 |
| `tblRequsitionChild` | 3 |
| `tblStockCondition` | 3 |
| `tblStockConditionFreeze` | 4 |
| `tblStockConditionPermission` | 3 |
| `tblStockInTransfar` | 3 |
| `tblSupplierInformation` | 2 |
| `tblTradeMaterialTransfer` | 6 |
| `tblUnitPrice` | 13 |
| `tblUserCompanyUnit` | 15 |
| `tblWHStockInDetail` | 11 |
| `tblWHStockInMaster` | 13 |
| `tblWHStockOutDetail` | 1 |
| `tblWHStockOutMaster` | 1 |
| `tblWearhouse` | 13 |
| `tblWhStockConditionFreeze` | 4 |
| `tblWhStoreFreeze` | 4 |
| `tbl_INV_WeightedAvg` | 1 |
| `tbl_MC_ProductRawMaterialDetails` | 1 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `spDeleteTradeMaterialTransfer` | 1 |
| `spInsertTradeMaterialTransfer` | 1 |
| `spUpdateTradeMaterialTransfer` | 1 |
| `sp_Common_GetUnitByPermissionByModule` | 3 |
| `sp_Common_GetUnitWiseLocation_FromErp` | 1 |
| `sp_Common_GetUnitWiseLocation_Zas` | 2 |
| `sp_FreezeStockPosting` | 1 |
| `sp_FreezeStockReleasePosting` | 1 |
| `sp_GET_DCStockInfo` | 1 |
| `sp_INV_WeightedAverage_Report` | 1 |
| `sp_I_DCStockOutInfo` | 1 |
| `sp_Process_TradeMaterialTransferFullProcess` | 1 |
| `sp_Process_WeightedAverageB2BTransfer` | 1 |
| `sp_Process_WeightedAverageFreezeStock` | 4 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
