# Specification Coverage

One row per page. Values: `COMPLETE` (fully documented from source, nothing left to discover in this repository), `PARTIAL` (documented as far as the repository allows), `MISSING` (nothing could be documented), `UNKNOWN` (the information lives outside the repository).

**No row is marked COMPLETE for Database, Rules, Permission or Status.** That is deliberate: those four depend on artefacts that are not in the repository (stored-procedure bodies, DDL, and the `tblMenuDistribution` / `tblActionUserWiseApproval` grant rows). Claiming COMPLETE there would be false.

## Pages with an additional deep trace

Four end-to-end workflows were hand-traced beyond the per-page template. For the pages they cover, the
algorithm, the arithmetic and the failure modes are documented — not just the inventory.

| Workflow | Pages it covers in depth |
|---|---|
| [sales-order-invoice/workflows/WF-invoice-batch-allocation.md](sales-order-invoice/workflows/WF-invoice-batch-allocation.md) | `SAL-010 InvoiceCreationForCustomer` (canonical), and by extension every `InvoiceCreation*` / `DelivaryInvoiceCreation*` variant |
| [delivery-logistics/workflows/WF-delivery-confirmation.md](delivery-logistics/workflows/WF-delivery-confirmation.md) | `SAL-001 DelivaryInvoiceCreationAuto`, `SAL-002 DelivaryInvoiceCreationForCustomerByOrderAuto` |
| [finance-collection/workflows/WF-customer-payment.md](finance-collection/workflows/WF-customer-payment.md) | `FIN-* CustomerPayment` and its four variants |
| [00-system/workflows/WF-approval.md](00-system/workflows/WF-approval.md) | all 16 `*Approval` / `*Approve` pages |

## What "COMPLETE" means here

| Dimension | COMPLETE means | Why some rows are not COMPLETE |
|---|---|---|
| UI | every server control in the markup is listed with its type, label, events and line number | 4 pages declare no `runat="server"` control with an id |
| Frontend | the postback model, UpdatePanels, extenders, inline scripts and alerts are documented | 4 pages have no markup controls to describe |
| Backend | every code-behind method is listed with its calls, tables, procedures and session keys | — |
| API | the page's HTTP surface is fully stated (usually "none") | — |
| DB | **never COMPLETE** — table names are recoverable, columns are not (`GAP-DB-002`) | 54 pages expose no resolvable data access at all |
| Rules | **never COMPLETE** — rules inside procedures are invisible (`GAP-DB-001`) | 76 pages carry no message-bearing rule of their own |
| Permission | **never anything but UNKNOWN** — permission is data (`GAP-SEC-001`) | by nature |
| Status | **never COMPLETE** — transitions execute inside procedures | by nature |
| Tests | proposed from code, never executed (`GAP-TEST-001`) | by nature |


| Module | Page ID | Page | UI | Frontend | Backend | API | DB | Rules | Permission | Status | Tests | Overall |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 00-system | `SYS-001` | `AdminMenuPanal.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-002` | `ApprovalStepPageWise.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-003` | `Default.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-004` | `HomePage.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-005` | `JqueryAccordian.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-006` | `JqueryTab.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-007` | `TemplatePage.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-008` | `UserPermission.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-009` | `UserWiseApprovalPermission.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-010` | `Login.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-011` | `DBBackup.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-012` | `DCUserPermission.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-013` | `StockConditionPermission.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-014` | `UserEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-015` | `UserEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-016` | `UserMenuPermissionReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-017` | `UserPasswordChangeReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| 00-system | `SYS-018` | `UserView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-001` | `Dashboard.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-002` | `AccountsPanel.aspx` | MISSING | PARTIAL | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-003` | `CashFlow.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-004` | `Dashboard.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-005` | `DashboardPanel.aspx` | MISSING | PARTIAL | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-006` | `IncomeStatement.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-007` | `TotalSummary.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| dashboard-analytics | `DSB-008` | `ZASMonitoringPanel.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-001` | `B2BTransferChallanRcvReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-002` | `B2BTransferChallanReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-003` | `BatchWiseDeliveryTopSheet.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-004` | `BatchWiseTopSheet.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-005` | `ChallanReportList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-006` | `CreatePickingDetailForWareHouse.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-007` | `CreatePickingOnWareHouse.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-008` | `DelivaryTopSheetGenerate.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-009` | `DeliveryReturnReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-010` | `LogisticeRollBackOperation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-011` | `PickingSlipGenerate.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-012` | `ReceiveProductByChalanByDC.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-013` | `ReturnAcknowledgmentNote.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-014` | `ReturnAcknowledgmentNoteView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-015` | `ReturnOnDeliveryReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-016` | `SCPickingGenerate.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-017` | `TopSheetGenerate.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-018` | `TransferReceiveProductByChalanByDC.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| delivery-logistics | `DLV-019` | `WareHousePickingReportList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-001` | `AIT Report.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-002` | `BatchWiseCollection.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-003` | `BatchWiseCollectionReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-004` | `CollectionSheet.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-005` | `CompanyWiseDepositSlipReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-006` | `CreditCustomerReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-007` | `CustomerDueReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-008` | `CustomerLedgerReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-009` | `CustomerOutstandingReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-010` | `CustomerPayment.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-011` | `CustomerPaymentBatchReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-012` | `CustomerPaymentList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-013` | `CustomerPaymentProcess.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-014` | `CustomerPaymentReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-015` | `DepositList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-016` | `DepositSlipEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-017` | `DuplicatePaymentList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-018` | `MarketWiseCustomerPayment.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-019` | `MoneyReceiptReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-020` | `PaymentReverse.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-021` | `PaymentTranscation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-022` | `PaymentTranscationNew.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-023` | `PaymentTranscationNew2.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-024` | `PettyCashApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-025` | `PrettyCashEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-026` | `PrettyCashView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| finance-collection | `FIN-027` | `TotalCustomerDueReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| integrations | `INT-001` | `HREmployeeSysnc.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| integrations | `INT-002` | `MIGOReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-001` | `DCStockInfoReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-002` | `DCStockReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-003` | `DCStockValuationReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-004` | `DestroyProduct.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-005` | `DestroyProductApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-006` | `DestroyProductList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-007` | `DestroyProductReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-008` | `DirectStockOut.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-009` | `DirectStockOutView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-010` | `FreezeStockReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-011` | `IssueRequisitionProducts.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-012` | `MaterialConversionReceive.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-013` | `MaterialConversionReceiveView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-014` | `NationalBeenCardReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-015` | `NationalStockReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-016` | `NearlyExpireProductReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-017` | `NewStockTransferDcToDc.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-018` | `OtherStockAction.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-019` | `StockConditionFreeze.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-020` | `StockReceive.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-021` | `StockReceiveByDC.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-022` | `StockReceiveReportByDc.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-023` | `StockReceiveView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-024` | `StockReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-025` | `TradeMaterialTransfer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-026` | `TradeMaterialTransferApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-027` | `TradeMaterialTransferList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-028` | `TransferStockReceiveByDC.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-029` | `ViewRequisitionForIssue.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-030` | `WHBeenCardReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-031` | `WHStockInList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-032` | `WHStockInReportList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-033` | `WHStockValuationReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-034` | `WarehouseStockIn.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-035` | `WarehouseStockInApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-036` | `WarehouseStockInView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-037` | `WarehouseStockOut.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-038` | `WeaightedAVG_Report.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-039` | `WhFreezeStockRelease.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-040` | `WhStockConditionFreeze.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| inventory-warehouse | `INV-041` | `WhStockMonitoringReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-001` | `ASMInfo.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-002` | `AreaInformation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-003` | `AreaInformationEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-004` | `AreaInformationView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-005` | `CompanyInformation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-006` | `CompanyInformationEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-007` | `CompanyInformationView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-008` | `CustCategoryEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-009` | `CustCategoryView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-010` | `CustMasterEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-011` | `CustMasterEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-012` | `CustMasterView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-013` | `CustomerCategoryEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-014` | `CustomerCreditLimitApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-015` | `CustomerCreditLimitEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-016` | `CustomerCreditView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-017` | `CustomerMasterEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-018` | `DcWiseAreaAlocation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-019` | `DeliveryPersonEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-020` | `DeliveryPersonView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-021` | `DepartmentEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-022` | `DepartmentEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-023` | `DepartmentView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-024` | `DesignationInfo.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-025` | `DesignationView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-026` | `DistributionCenterInfoEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-027` | `DistributionCenterInfoView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-028` | `DistributionCenterInformation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-029` | `EmpGenInfView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-030` | `EmpGeneralInfoEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-031` | `MIOSetup.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-032` | `MIOTargetEntryByProduct.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-033` | `ManufacturerEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-034` | `ManufacturerEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-035` | `ManufacturerView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-036` | `MarketInfoEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-037` | `MarketInfoEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-038` | `MarketInfoView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-039` | `MiaTargetEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-040` | `MiaTargetEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-041` | `MiaTargetView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-042` | `PackSizeEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-043` | `PackSizeEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-044` | `PackSizeView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-045` | `ProCategoryEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-046` | `ProCategoryEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-047` | `ProCategoryView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-048` | `ProTypeEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-049` | `ProTypeEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-050` | `ProTypeView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-051` | `ProUnitPriceApprove.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-052` | `ProUnitPriceEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-053` | `ProUnitPriceEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-054` | `ProUnitPriceView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-055` | `ProductEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-056` | `ProductEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-057` | `ProductSQEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-058` | `ProductSQEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-059` | `ProductSQView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-060` | `ProductView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-061` | `RSMSetup.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-062` | `RegionInformation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-063` | `RegionInformationEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-064` | `RegionInformationViw.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-065` | `ShippingCartonSizeEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-066` | `ShippingCartonSizeEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-067` | `ShippingCartonSizeView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-068` | `SlaveInfoApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-069` | `SlaveInformation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-070` | `SlaveInformationView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-071` | `SupplierInfoView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-072` | `SupplierInformation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-073` | `TerritoryInformation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-074` | `TerritoryInformationEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| master-data | `MST-075` | `TerritoryInformationView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-001` | `BonusCampaignSetup.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-002` | `BonusCampaignSetupView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-003` | `CampaignSetup.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-004` | `CampaignView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-005` | `CustomerFixedDiscount.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-006` | `CustomerFixedDiscountView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-007` | `DiscountApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-008` | `FixedDiscountApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-009` | `FixedDiscountEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-010` | `FixedDiscountView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-011` | `ProductDiscountEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-012` | `ProductDiscountView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-013` | `ProductQuotedPriceApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-014` | `ProductQuotedPriceSetup.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| pricing-discount-campaign | `PRC-015` | `ProductQuotedPriceView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-001` | `AllSalesreportHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-002` | `CustomerCoverageHTMLReportDZSMWise.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-003` | `CustomerCoverageHTMLReportFEWise.aspx` | MISSING | PARTIAL | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-004` | `CustomerCoverageHTMLReportMiaWise.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-005` | `DCStockHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-006` | `DCStockValuationHtmlReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-007` | `DelivaryReturnHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-008` | `InvoicePrintPreview.aspx` | MISSING | PARTIAL | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-009` | `NationalBeenCardHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-010` | `NationalStockReportHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-011` | `ProductWiseNationalSalesHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-012` | `ProformaHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-013` | `SalesPerformanceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-014` | `SalesSummeryForAccHtmlReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-015` | `SalesTrackingHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-016` | `StockHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-017` | `WHBeenCardHTMLReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-018` | `WHStockValuationHtmlReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-019` | `AITReceiptViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-020` | `ASMWiseProductSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-021` | `AllSalesreportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-022` | `AuditReportOneReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-023` | `B2BReceiveViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-024` | `BatchWiseCollectionReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-025` | `BatchWiseCollectionSheetReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-026` | `BatchWiseCollectionViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-027` | `BatchWiseDelReturnViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-028` | `BatchWiseDeliveryTopSheetReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-029` | `BatchWiseMoneyReceiptReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-030` | `BatchWiseTopSheetReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-031` | `BusinessSummaryViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-032` | `ChallanReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-033` | `CreditCustomerViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-034` | `CreditReportAllViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-035` | `CusTPaymentReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-036` | `CustomerDueReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-037` | `CustomerLedgerViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-038` | `CustomerMasterViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-039` | `CustomerPaymentReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-040` | `CustomerPaymentViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-041` | `CustomerReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-042` | `CustomerReturnByMioReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-043` | `CustomerWiseLedgerReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-044` | `CustomerWiseProductSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-045` | `CustomerWiseReturnReportByProductViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-046` | `CustomerWiseReturnReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-047` | `CustomerWiseSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-048` | `DCStockInfoReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-049` | `DCStockReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-050` | `DcToDcChalanReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-051` | `DeleteInvoiceReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-052` | `DelivaryInvoiceReturnViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-053` | `DeliveryReturnReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-054` | `DepositReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-055` | `DestroyProductReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-056` | `DestroyProductStatementViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-057` | `DirectStockOutViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-058` | `EmployeeInformationReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-059` | `EmployeewiseProductSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-060` | `FieldForceSalesSummeryReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-061` | `FreezeStockReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-062` | `GroupInvoiceReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-063` | `InTransitReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-064` | `InvoiceReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-065` | `MIGOReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-066` | `MIOSalesSummeryReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-067` | `MIOWiseProformaReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-068` | `MarketwisePicking.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-069` | `MiaWiseSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-070` | `MioWiseCustomerReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-071` | `MioWiseProductSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-072` | `NTInvoiceReturnReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-073` | `NationalStockReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-074` | `NearlyExpireproductReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-075` | `PaymentReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-076` | `PickingSlipGenerateViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-077` | `ProductCostPriceViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-078` | `ProductDiscountReportAllViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-079` | `ProductDiscountReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-080` | `ProductTreadPriceViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-081` | `ProformaReportPrintViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-082` | `ProformaReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-083` | `RSMWiseProductSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-084` | `ReturnOnDeliveryReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-085` | `SalesAdministrationReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-086` | `SalesHubReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-087` | `SalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-088` | `SalesTransctionReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-089` | `SampleIssueReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-090` | `StockReceiveReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-091` | `TopSheetReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-092` | `TransferStockReceiveReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-093` | `UserCredentialsReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-094` | `UserPermissionReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-095` | `WHStockInReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-096` | `WareHousePickingReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-097` | `WingsSalesAndCollectionReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-098` | `WingsWiseCollectionReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-099` | `WingsWiseDepositReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-100` | `WingsWiseSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-101` | `ASMReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-102` | `ASMWiseProductSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-103` | `AllSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-104` | `AreaWiseMonthlyInventoryReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-105` | `AuditReportOne.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-106` | `BatchWiseDelReturnSummery.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-107` | `BatchWiseTransitReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-108` | `CustomerMasterReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-109` | `CustomerReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-110` | `CustomerWiseLedgerReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-111` | `CustomerWiseProductSalesReportByMio.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-112` | `CustomerWiseReturnReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-113` | `CustomerWiseReturnReportByProduct.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-114` | `CustomerWiseSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-115` | `DeleteInvoiceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-116` | `DeliveryInvoiceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-117` | `EmployeeInformationReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-118` | `FieldForceSalesSummery.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-119` | `InTransitReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-120` | `InvoiceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-121` | `MIAWiseSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-122` | `MIOSalesSummeryReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-123` | `MIOWiseProformaReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-124` | `MiaWiseCustomerCoverageReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-125` | `MioWiseCustomerReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-126` | `MioWiseProductSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-127` | `ProductCostPriceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-128` | `ProductDiscountReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-129` | `ProductPriceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-130` | `ProductWiseNationalSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-131` | `ProformaReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-132` | `RSMWiseProductSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-133` | `SalesHubReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-134` | `SalesHubReport2.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-135` | `SalesPerformanceReportProductWise.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-136` | `SalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-137` | `SalesReports.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-138` | `SalesSummeryReportForAcc.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-139` | `SalesTrackingReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-140` | `SalesTranscationReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-141` | `SampleInvoiceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-142` | `WingsSalesAndCollectionReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-143` | `WingsSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-144` | `WingsWiseCollectionReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-145` | `WingsWiseSalesReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| reports | `RPT-146` | `WingsWiseSalesReportViewer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| returns | `RET-001` | `CustomerReturnByMIOReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| returns | `RET-002` | `ReturnProductConsCustomerByInvoice.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| returns | `RET-003` | `ReturnProductCustomerByInvoice.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| returns | `RET-004` | `ReturnustomerByInvoiceApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| returns | `RET-005` | `SalesRejecionReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-001` | `DelivaryInvoiceCreationAuto.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-002` | `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-003` | `ErrorInvoiceList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-004` | `GroupInvoiceEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-005` | `GroupInvoiceList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-006` | `InvoiceAutoPrint.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-007` | `InvoiceBatchEntry.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-008` | `InvoiceCreationByOrder.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-009` | `InvoiceCreationForConsumerCustomer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-010` | `InvoiceCreationForCustomer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-011` | `InvoiceCreationForCustomerByOrder.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-012` | `ManualOrderApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-013` | `ManualOrderView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-014` | `ManualOrderrCreation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-015` | `Non-TranscationalInvoicePayment.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-016` | `NonTranscationalInvoiceApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | MISSING | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-017` | `NonTranscationalInvoiceReport.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-018` | `NonTranscationalInvoiceReturn.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-019` | `OrderDeleteorEdit.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-020` | `OrderRequisitionCreation.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-021` | `OrderRequisitionView.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | MISSING | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-022` | `OrderStatus.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-023` | `ProformaorInvoiceReturn.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-024` | `ReturnInvoiceCreationForCustomer.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-025` | `SampleInvoiceList.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-026` | `SampleProductIssueApproval.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |
| sales-order-invoice | `SAL-027` | `SampleProductrIssue.aspx` | COMPLETE | COMPLETE | COMPLETE | COMPLETE | PARTIAL | PARTIAL | UNKNOWN | PARTIAL | PARTIAL | PARTIAL |

## Roll-up

| Overall | Pages |
|---|---|
| PARTIAL | 383 |

| Dimension | COMPLETE | PARTIAL | MISSING | UNKNOWN |
|---|---|---|---|---|
| UI | 379 | 0 | 4 | 0 |
| Frontend | 379 | 4 | 0 | 0 |
| Backend | 383 | 0 | 0 | 0 |
| API | 383 | 0 | 0 | 0 |
| DB | 0 | 329 | 54 | 0 |
| Rules | 0 | 307 | 76 | 0 |
| Permission | 0 | 0 | 0 | 383 |
| Status | 0 | 383 | 0 | 0 |
| Tests | 0 | 383 | 0 | 0 |
