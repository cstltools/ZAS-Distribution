# Report Inventory

Two report technologies are used.

1. **Crystal Reports 13** — `.rpt` definitions under `Solution.Web/Reports/CrystalReports` (123 files) and `Library.CrystalReports/SInventory_RPT` (42 files), rendered by the 82 viewer pages in `Solution.Web/SInventory_RPTVIEW`.
2. **Hand-built HTML** — the 18 pages in `Solution.Web/HTMLReport`, which render an HTML table directly.

Report *parameter entry* happens on a separate screen in `SInventory_UI` (e.g. `SalesReport.aspx`), which stores the filter values in `Session` and/or the query string and then opens the viewer in a popup with `window.open`. That is why most viewers read `Session[...]` rather than the query string.

| Viewer page | `.rpt` used | Query-string params | Session params | Data layer |
|---|---|---|---|---|
| `SInventory_RPTVIEW/AITReceiptViewer.aspx` | `crpAITCollectionReport.rpt` | — | `paydetailId` | `BatchWiseCollectionReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/ASMWiseProductSalesReportViewer.aspx` | `ASMReportExcel.rpt`, `crpASMWiseProductSales.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `ASMWiseProductSalesDal`, `aReportDal` |
| `SInventory_RPTVIEW/AllSalesreportViewer.aspx` | `crpAllSalesReport.rpt`, `rptInvoiceForCustomer.rpt` | `NationalReport`, `districtId`, `fromDate`, `toDate` | `SalesReport` | `CompanySalesReportBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aCompanySalesReportBll`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/AuditReportOneReportViewer.aspx` | `crpAuditReportOne.rpt`, `crpDelOrder.rpt` | `rptType` | `Excel`, `parameter` | `AuditReportOneBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/B2BReceiveViewer.aspx` | `crpB2BStockReceive.rpt` | `ChalanId` | — | `ChalanBLL`, `ComUnitSalesReportBLL`, `StockTransportOrderReportBLL`, `aComUnitSalesReportBll`, `aOrderReportBll`, `aRequisitionBll` |
| `SInventory_RPTVIEW/BatchWiseCollectionReportViewer.aspx` | `crpBatchWiseCollectionReport.rpt` | — | `paydetailId` | `BatchWiseCollectionReportDal`, `aReportDal`, `aSheetBll` |
| `SInventory_RPTVIEW/BatchWiseCollectionSheetReportViewer.aspx` | `crpCollectionSheetReport.rpt` | — | `paydetailId` | `BatchWiseCollectionReportDal`, `aReportDal`, `aSheetBll` |
| `SInventory_RPTVIEW/BatchWiseCollectionViewer.aspx` | `crpBatchWiseCollection.rpt` | — | `batchId` | `BatchWiseCollectionReportDal`, `aReportDal`, `aSheetBll` |
| `SInventory_RPTVIEW/BatchWiseDelReturnViewer.aspx` | `crpBatchWiseReturnTopSheet.rpt` | `rptType` | `BatchId`, `CompanyId`, `DelManId`, `InvoiceDateTextBox` | `BatchWiseTopSheetBll`, `aSheetBll` |
| `SInventory_RPTVIEW/BatchWiseDeliveryTopSheetReportViewer.aspx` | `crpBatchWiseDeliveryTopSheet.rpt` | `rptType` | `BatchId`, `CompanyId`, `DelManId`, `InvoiceDateTextBox` | `BatchWiseTopSheetBll`, `aSheetBll` |
| `SInventory_RPTVIEW/BatchWiseMoneyReceiptReportViewer.aspx` | `crpBatchWiseCollectionReport.rpt` | — | `MoneyReceiptId` | `BatchWiseCollectionReportDal`, `aReportDal`, `aSheetBll` |
| `SInventory_RPTVIEW/BatchWiseTopSheetReportViewer.aspx` | `crpBatchWiseFinalTopSheet.rpt` | `rptType` | `BatchId`, `CompanyId`, `DelManId`, `InvoiceDateTextBox` | `BatchWiseTopSheetBll`, `aSheetBll` |
| `SInventory_RPTVIEW/BusinessSummaryViewer.aspx` | `crpBusinessSummary.rpt` | `fromDate`, `toDate` | — | `CompanySalesReportBLL`, `InvoiceBLL`, `LoadSummaryDAL`, `StockTransportOrderReportBLL`, `TotalSummaryDAL`, `aCompanySalesReportBll`, `aInvoiceBll`, `aOrderReportBll`, `aTotalSummaryDAL` |
| `SInventory_RPTVIEW/ChallanReportViewer.aspx` | `crpChallan.rpt` | `reqId` | — | `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `StockTransportOrderReportDetailDataBLL`, `StockTransportOrderReportMainDataBLL`, `aOrderReportBll` |
| `SInventory_RPTVIEW/CreditCustomerViewer.aspx` | `crpCreditCustomerReport.rpt` | — | `PRAM` | `EmployeeInformationBLL`, `PettyCashBLL`, `PettyCash_BLL`, `aCashBll`, `aEmployeeInformationBLL` |
| `SInventory_RPTVIEW/CreditReportAllViewer.aspx` | `crpCreditCustomerReport.rpt` | — | — | `EmployeeInformationBLL`, `PettyCashBLL`, `PettyCash_BLL`, `aCashBll`, `aEmployeeInformationBLL` |
| `SInventory_RPTVIEW/CusTPaymentReportViewer.aspx` | `rptCusPaymentList.rpt` | — | `jkdj` | `CustPaymentDAL`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerDueReportViewer.aspx` | `crpCustomerDueReport.rpt`, `crpCustomerTotalDueReport.rpt` | `company`, `reportType`, `rptType` | `PRAM`, `PRAM2`, `RPT`, `TODT` | `CustomerDueReportDAL`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerLedgerViewer.aspx` | `crpCustomerLedger.rpt` | `CustomerID` | — | `CustomerLedgerBLL`, `CustomerLedgerBll`, `aCustomerMasterBll` |
| `SInventory_RPTVIEW/CustomerMasterViewer.aspx` | `crpCustomerMaster.rpt` | `rptType` | `AllCheck` | `CompanyInfoBLL`, `CustomerMasterBLL`, `StockTransportOrderReportBLL`, `aCustomerMasterBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/CustomerPaymentReportViewer.aspx` | `ASMReportExcel.rpt`, `crpCustomerPayment.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `CustomerPaymentReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerPaymentViewer.aspx` | `rptCustomerPayment.rpt`, `rptCustomerPaymentDue.rpt` | `fromDate`, `payment`, `salesCenter`, `toDate` | `PaymentReport` | `CompanySalesReportBLL`, `CustomerPaymentBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aCompanySalesReportBll`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/CustomerReportViewer.aspx` | `crpCustomerMasterReport.rpt` | — | `PRAM` | `CustomerReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerReturnByMioReportViewer.aspx` | `ASMReportExcel.rpt`, `crpMioWiseCustomerSalesReturn.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `MioWiseProductSalesDal`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerWiseLedgerReportViewer.aspx` | `ASMReportExcel.rpt`, `rptCustomerLedgerNew.rpt` | `fromDate`, `reportType`, `rptType`, `toDate` | `PRAM` | `CustomerWiseLedgerDal`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerWiseProductSalesReportViewer.aspx` | `ASMReportExcel.rpt`, `crpCustomerWiseProductSales.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `CustomerWiseProductSalesDal`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerWiseReturnReportByProductViewer.aspx` | `ASMReportExcel.rpt`, `crpCustomerWiseReturnByProduct.rpt` | `reportType`, `rptType` | `DateWise`, `FromDate`, `PRAM`, `ToDate` | `CustomerWiseReturnDal`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerWiseReturnReportViewer.aspx` | `ASMReportExcel.rpt`, `crpCustomerWiseReturn.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `CustomerWiseReturnDal`, `aReportDal` |
| `SInventory_RPTVIEW/CustomerWiseSalesReportViewer.aspx` | `crpCustomerWiseSales.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `CustomerWiseSalesReportDAL`, `aReportDal` |
| `SInventory_RPTVIEW/DCStockInfoReportViewer.aspx` | `rptDCStock.rpt` | `reportType`, `rptType` | `PRAM` | `DCStockInfoReprotDal`, `aReportDal` |
| `SInventory_RPTVIEW/DCStockReportViewer.aspx` | `rptDCStock.rpt`, `rptWHDCStock.rpt` | `comUnitId` | `DC`, `ReportView` | `CompanyInfoBLL`, `DCReportDetailDataDAL`, `DCReportMainDataDAL`, `DCStockReportBLL`, `StockTransportOrderReportBLL`, `WHReportDetailDataDAL`, `aDcStockReportBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/DcToDcChalanReportViewer.aspx` | _HTML / none_ | `chalanno` | — | `ChalanBLL`, `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `aChalanBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/DeleteInvoiceReportViewer.aspx` | `crpDelOrder.rpt`, `crpDeleteInvoiceReport.rpt` | `rptType` | `Excel`, `parameter` | `DeleteInvoiceReportBLL`, `aOrderReportBll` |
| `SInventory_RPTVIEW/DelivaryInvoiceReturnViewer.aspx` | `rptInvoiceForCustomerDelivary.rpt` | `InvNo` | — | `CompanyInfoBLL`, `InvoiceBLL`, `InvoiceDetailDataForReportBLL`, `InvoiceMainDataForReportBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/DeliveryReturnReportViewer.aspx` | `crpDeliveryReturnReport.rpt` | `NationalReport`, `districtId`, `fromDate`, `toDate` | `DeliveryReturnRpt` | `CompanySalesReportBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aCompanySalesReportBll`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/DepositReportViewer.aspx` | `crpWingsWiseDepositList.rpt` | — | `ComId`, `FDate`, `TDate`, `UnitId` | `DepositSlipEntryDal`, `aReportDal` |
| `SInventory_RPTVIEW/DestroyProductReportViewer.aspx` | `crpDestroyProductReport.rpt` | — | `PRAM` | `DestroyProductDal`, `aReportDal` |
| `SInventory_RPTVIEW/DestroyProductStatementViewer.aspx` | `crpDestroyProductStatementReport.rpt` | — | `PRAM` | `DestroyProductDal`, `aReportDal` |
| `SInventory_RPTVIEW/DirectStockOutViewer.aspx` | `crpStockOutReport.rpt` | — | `paydetailId` | `DirectStockOutDal`, `aReportDal` |
| `SInventory_RPTVIEW/EmployeeInformationReportViewer.aspx` | `crpDeliveryPerson.rpt`, `crpDeliveryPersonExcel.rpt`, `crpEemployeeInformation.rpt`, `crpEemployeeInformationExcel.rpt` | `reportType`, `rptType` | `PRAM` | `EmployeeInformationBLL`, `aEmployeeInformationBLL` |
| `SInventory_RPTVIEW/EmployeewiseProductSalesReportViewer.aspx` | `crpEmployeewiseProductSales.rpt` | `rptType` | — | `DistributionCenterStockMonitoringBll`, `aStockMonitoringBll` |
| `SInventory_RPTVIEW/FieldForceSalesSummeryReportViewer.aspx` | `ASMReportExcel.rpt`, `crpFieldForceSalesSummery.rpt` | — | `FDate`, `TDate`, `pram` | `CompanyInfoBLL`, `FieldForceSummeryDal`, `StockTransportOrderReportBLL`, `aOrderReportBll`, `aReportDal` |
| `SInventory_RPTVIEW/FreezeStockReportViewer.aspx` | `crpFreezeStockInfo.rpt` | `reportType`, `rptType` | `PRAM` | `FreezeStockReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/GroupInvoiceReportViewer.aspx` | `rptGroupInvoiceReport2.rpt` | — | `paydetailId` | `CompanyInfoBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/InTransitReportViewer.aspx` | `INTransitReport.rpt`, `crpINTransitReport.rpt` | `NationalReport`, `districtId`, `fromDate`, `toDate` | `Excel` | `IntransitBll`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/InvoiceReportViewer.aspx` | `rptInvoiceForCustomer.rpt` | `InvNo`, `invoiceId` | `customer` | `CompanyInfoBLL`, `InvoiceBLL`, `InvoiceDetailDataForReportBLL`, `InvoiceMainDataForReportBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/MIGOReportViewer.aspx` | `crpMainMigoReport.rpt`, `crpMigoDetailsReport.rpt`, `crpOrderDetailsReport.rpt` | `fromDate`, `rpttype`, `toDate` | — | `CompanyInfoBLL`, `ExcelUpForMIGOBLL`, `StockTransportOrderReportBLL`, `aOrderReportBll` |
| `SInventory_RPTVIEW/MIOSalesSummeryReportViewer.aspx` | `ASMReportExcel.rpt`, `crpMioSalesSummery.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `MIOSalesSummeryReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/MIOWiseProformaReportViewer.aspx` | `ASMReportExcel.rpt`, `crpMIOWiseProformaReport.rpt` | `reportType`, `rptType` | `PRAM` | `MIOWiseProformaReportDAL`, `aProformarDAL` |
| `SInventory_RPTVIEW/MarketwisePicking.aspx` | `rptMarketwisePicking.rpt` | `SC` | `Manufac`, `Market`, `MrktRptParameter`, `invoicedate` | `CompanyInfoBLL`, `InvoiceBLL`, `InvoiceDetailDataForReportBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/MiaWiseSalesReportViewer.aspx` | _HTML / none_ | `comunitId`, `fromDate`, `miaId`, `toDate` | — | `CompanyInfoBLL`, `MiaSalesReportBLL`, `MiaWiseReportDetailDataBLL`, `MiaWiseReportMainDataBLL`, `StockTransportOrderReportBLL`, `aMiaSalesReportBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/MioWiseCustomerReportViewer.aspx` | `ASMReportExcel.rpt`, `crpMioWiseCustomer.rpt` | `reportType`, `rptType` | `PRAM` | `MioWiseCustomerDal`, `aReportDal` |
| `SInventory_RPTVIEW/MioWiseProductSalesReportViewer.aspx` | `ASMReportExcel.rpt`, `crpMioWiseProductSales.rpt` | `reportType`, `rptType` | `COMID`, `FromDate`, `PRAM`, `PRAM2`, `PRAM3`, `PRAM4`, `ToDate` | `MioWiseProductSalesDal`, `aReportDal` |
| `SInventory_RPTVIEW/NTInvoiceReturnReportViewer.aspx` | `crpNTInvoiceReturnReport.rpt` | — | `paydetailId` | `BatchWiseCollectionReportDal`, `aReportDal`, `aSheetBll` |
| `SInventory_RPTVIEW/NationalStockReportViewer.aspx` | `crpNationalStockReport.rpt` | `reportType`, `rptType` | `PRAM` | `WhStockConditionFreezeDal`, `aReportDal` |
| `SInventory_RPTVIEW/NearlyExpireproductReportViewer.aspx` | `rptExpireProduct.rpt` | `branchId`, `comId`, `nomberOfMonths` | `National` | `AreaWiseMonthlyInventoryReportDal`, `CompanyInfoBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aDal`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/PaymentReportViewer.aspx` | `crpPaymentTranscation.rpt` | `InvNo` | — | `CompanyInfoBLL`, `InvoiceBLL`, `PaymentTranscationDal`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll`, `aTranscationDal` |
| `SInventory_RPTVIEW/PickingSlipGenerateViewer.aspx` | `rptMarketwisePicking.rpt` | `SC` | `DelMan`, `Manufac`, `Market`, `MrktRptParameter`, `invoicedate` | `CompanyInfoBLL`, `InvoiceBLL`, `InvoiceDetailDataForReportBLL`, `InvoiceNumberBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/ProductCostPriceViewer.aspx` | _HTML / none_ | — | — | `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `UnitPriceBLL`, `aOrderReportBll`, `aUnitPriceBll` |
| `SInventory_RPTVIEW/ProductDiscountReportAllViewer.aspx` | `crpProductDiscount.rpt` | — | `PRAM` | `PettyCashBLL`, `PettyCash_BLL`, `aCashBll` |
| `SInventory_RPTVIEW/ProductDiscountReportViewer.aspx` | `crpProductDiscountReport.rpt` | — | `PRAM` | `PettyCashBLL`, `PettyCash_BLL`, `aCashBll` |
| `SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` | `crpPoductPriceInfo.rpt`, `crpProductInfo.rpt` | `reportType` | `PRAM` | `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `UnitPriceBLL`, `aOrderReportBll`, `aUnitPriceBll` |
| `SInventory_RPTVIEW/ProformaReportPrintViewer.aspx` | `rptInvoiceForCustomer.rpt` | — | `paydetailId` | `CompanyInfoBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/ProformaReportViewer.aspx` | `rptProformaReport.rpt` | `NationalReport`, `districtId`, `fromDate`, `toDate` | `ProformaReport` | `InvoiceBLL`, `ProformaReportBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/RSMWiseProductSalesReportViewer.aspx` | `ASMReportExcel.rpt`, `crpRSMWiseProductSales.rpt` | `reportType`, `rptType` | `FromDate`, `PRAM`, `ToDate` | `RSMWiseProductSalesDal`, `aReportDal` |
| `SInventory_RPTVIEW/ReturnOnDeliveryReportViewer.aspx` | `crpBatchWiseDeliveryTopSheet.rpt` | `rptType` | `BatchId`, `CompanyId`, `DelManId`, `InvoiceDateTextBox` | `BatchWiseTopSheetBll`, `aSheetBll` |
| `SInventory_RPTVIEW/SalesAdministrationReportViewer.aspx` | `ASMReport.rpt`, `ASMReportExcel.rpt`, `MIOReport.rpt`, `MIOReportExcel.rpt`, `crpRSMInformation.rpt`, `crpRSMInformationExcel.rpt` | `reportType`, `rptType` | `PRAM` | `SalesAdministrationReportBll`, `administrationBll` |
| `SInventory_RPTVIEW/SalesHubReportViewer.aspx` | `rptAreaReport.rpt`, `rptMarketReport.rpt`, `rptRegionReport.rpt`, `rptTerritoryReport.rpt` | `rptType` | `PRAM` | `SalesHubBLL`, `aSalesHubBLL` |
| `SInventory_RPTVIEW/SalesReportViewer.aspx` | `crpSalesInformationReport.rpt` | `reportType`, `rptType` | `PRAM` | `CustomerPaymentReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/SalesTransctionReportViewer.aspx` | `ASMReportExcel.rpt`, `crpSalesTranscation.rpt` | — | `PRAM` | `WingsWiseSalesReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/SampleIssueReportViewer.aspx` | `rptSampleIssueInvoice.rpt` | `InvNo` | — | `SampleIssueDal`, `aIssueDal` |
| `SInventory_RPTVIEW/StockReceiveReportViewer.aspx` | `crpStockReceiveByDC.rpt` | `ReqId` | — | `ComUnitSalesReportBLL`, `CompanyInfoBLL`, `RequisitionBLL`, `StockTransportOrderReportBLL`, `aComUnitSalesReportBll`, `aOrderReportBll`, `aRequisitionBll` |
| `SInventory_RPTVIEW/TopSheetReportViewer.aspx` | `rptTopSheet.rpt` | `InvNo` | `BatchId`, `CompanyId`, `DelManId`, `InvoiceDateTextBox`, `ProformaTopSheet` | `CompanyInfoBLL`, `InvoiceBLL`, `InvoiceDetailDataForReportBLL`, `InvoiceMainDataForReportBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `SInventory_RPTVIEW/TransferStockReceiveReportViewer.aspx` | _HTML / none_ | `ChalanId` | — | `ChalanBLL`, `ComUnitSalesReportBLL`, `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `aComUnitSalesReportBll`, `aOrderReportBll`, `aRequisitionBll` |
| `SInventory_RPTVIEW/UserCredentialsReportViewer.aspx` | `ASMReportExcel.rpt`, `crpUserCredentialLog.rpt` | — | `FDate`, `TDate`, `UserId` | `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `UserCredentialChangeDal`, `WingsWiseSalesReportDal`, `aDal`, `aOrderReportBll`, `aReportDal` |
| `SInventory_RPTVIEW/UserPermissionReportViewer.aspx` | `crpUserMenuPermission.rpt` | — | `Pram` | `UserMenuPermissionDal`, `aReportDal` |
| `SInventory_RPTVIEW/WHStockInReportViewer.aspx` | `crpWHStockIn.rpt`, `crpWHStockInStatement.rpt`, `rptWHDCStock.rpt`, `rptWHDCStockExcel.rpt` | `reqId` | `Excel`, `PRAM`, `ProductCode`, `ReportType` | `CompanyInfoBLL`, `DCStockReportBLL`, `WHReportDetailDataDAL`, `WHStockInReportBll`, `aDcStockReportBll`, `aStockInReportBll` |
| `SInventory_RPTVIEW/WareHousePickingReportViewer.aspx` | `rptPickingForWarehouse.rpt` | `reqId` | — | `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `StockTransportOrderReportDetailDataBLL`, `StockTransportOrderReportMainDataBLL`, `aOrderReportBll` |
| `SInventory_RPTVIEW/WingsSalesAndCollectionReportViewer.aspx` | `crpSalesAndCollectionReport.rpt` | `reportType`, `rptType` | `DCId`, `FromDate`, `PRAM`, `ToDate` | `WingsSalesAndCollectionReportBll`, `andCollectionReportBll` |
| `SInventory_RPTVIEW/WingsWiseCollectionReportViewer.aspx` | `ASMReportExcel.rpt`, `crpWingsWiseCollection.rpt` | — | `FromDate`, `PRAM`, `ToDate`, `UserId` | `WingsWiseSalesReportDal`, `aReportDal` |
| `SInventory_RPTVIEW/WingsWiseDepositReportViewer.aspx` | `crpWingsWiseDeposit.rpt` | — | `ComId`, `FDate`, `TDate`, `UnitId` | `DepositSlipEntryDal`, `aReportDal` |
| `SInventory_RPTVIEW/WingsWiseSalesReportViewer.aspx` | `ASMReportExcel.rpt`, `crpWingsWiseSales.rpt` | — | `ComId`, `FDate`, `TDate`, `UnitId`, `UserId` | `WingsWiseSalesReportDal`, `aReportDal` |
| `HTMLReport/AllSalesreportHTMLReport.aspx` | `crpAllSalesReport.rpt`, `rptInvoiceForCustomer.rpt` | `NationalReport`, `districtId`, `fromDate`, `toDate` | `Param` | `CompanySalesReportBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aCompanySalesReportBll`, `aInvoiceBll`, `aOrderReportBll` |
| `HTMLReport/CustomerCoverageHTMLReportDZSMWise.aspx` | `crpMiaWiseCustomerCoverage.rpt` | `fromDate`, `toDate` | — | `CustomerCoverageReportBll`, `aCoverageReportBll` |
| `HTMLReport/CustomerCoverageHTMLReportFEWise.aspx` | _HTML / none_ | — | — | UNKNOWN |
| `HTMLReport/CustomerCoverageHTMLReportMiaWise.aspx` | `crpMiaWiseCustomerCoverage.rpt` | `fromDate`, `toDate` | — | `CustomerCoverageReportBll`, `aCoverageReportBll` |
| `HTMLReport/DCStockHTMLReport.aspx` | `rptAreaWiseMonthlyInventoryReport.rpt` | `DC`, `branchId`, `comId`, `fromDate`, `toDate` | — | `AreaWiseMonthlyInventoryReportBll`, `aReportBll` |
| `HTMLReport/DCStockValuationHtmlReport.aspx` | _HTML / none_ | — | `ComId`, `CompanyName`, `FDate`, `ProductId`, `ProductName`, `TDate`, `WarehouseName` | `WHStockValuationReprtDal`, `aReportBll` |
| `HTMLReport/DelivaryReturnHTMLReport.aspx` | `crpDeliveryReturnReport.rpt` | `NationalReport`, `districtId`, `fromDate`, `toDate` | — | `CompanySalesReportBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aCompanySalesReportBll`, `aInvoiceBll`, `aOrderReportBll` |
| `HTMLReport/InvoicePrintPreview.aspx` | _HTML / none_ | — | — | UNKNOWN |
| `HTMLReport/NationalBeenCardHTMLReport.aspx` | `rptAreaWiseMonthlyInventoryReport.rpt`, `rptNationalBinCardExcel.rpt` | `DC`, `branchId`, `comId`, `fromDate`, `toDate` | — | `NationalBeenCaedDal`, `aReportBll` |
| `HTMLReport/NationalStockReportHTMLReport.aspx` | `crpNationalStockReport.rpt` | — | `CompanyName`, `PRAM` | `WhStockConditionFreezeDal`, `aReportDal` |
| `HTMLReport/ProductWiseNationalSalesHTMLReport.aspx` | `crpNationalSalesProductWise.rpt`, `rptAreaWiseMonthlyInventoryReport.rpt` | — | `ComId`, `FDate`, `PRAM`, `TDate` | `NationalSalesProductWiseDal`, `aReportDal` |
| `HTMLReport/ProformaHTMLReport.aspx` | `rptProformaReport.rpt` | `NationalReport`, `districtId`, `fromDate`, `toDate` | — | `InvoiceBLL`, `ProformaReportBLL`, `StockTransportOrderReportBLL`, `aInvoiceBll`, `aOrderReportBll` |
| `HTMLReport/SalesPerformanceReport.aspx` | _HTML / none_ | `ComUnitId`, `CompanyId`, `FromDate`, `ToDate` | — | `SalesPerformanceReportProductWise_Dal`, `aDal` |
| `HTMLReport/SalesSummeryForAccHtmlReport.aspx` | `rptSalesSummeryForAcc.rpt` | — | `ComId`, `FDate`, `TDate`, `UnitId` | `aReportBll` |
| `HTMLReport/SalesTrackingHTMLReport.aspx` | `crpSalesTrackingReport.rpt`, `rptAreaWiseMonthlyInventoryReport.rpt` | — | `PRAM` | `NationalSalesProductWiseDal`, `aReportDal` |
| `HTMLReport/StockHTMLReport.aspx` | `rptDCStock.rpt`, `rptWHDCStock.rpt`, `rptWHDCStockSum.rpt` | `comUnitId` | `DC`, `SDC` | `CompanyInfoBLL`, `DCReportDetailDataDAL`, `DCReportMainDataDAL`, `DCStockReportBLL`, `StockTransportOrderReportBLL`, `aDcStockReportBll`, `aOrderReportBll` |
| `HTMLReport/WHBeenCardHTMLReport.aspx` | `rptAreaWiseMonthlyInventoryReport.rpt` | `DC`, `branchId`, `comId`, `fromDate`, `toDate` | — | `WHBinCardDal`, `aReportBll` |
| `HTMLReport/WHStockValuationHtmlReport.aspx` | _HTML / none_ | — | `ComId`, `CompanyName`, `FDate`, `ProductId`, `ProductName`, `TDate`, `WarehouseName` | `WHStockValuationReprtDal`, `aReportBll` |

## `.rpt` files with no viewer page found

48 of 146 report definitions are not referenced by any viewer page found in the repository. They are either dead, or bound at run time by a name this scan could not resolve.

- `crpBatchWiseTopSheet.rpt`
- `crpCWMonthlyInventoryReport.rpt`
- `crpCustomerWiseLedger.rpt`
- `crpDCStockMonitoringReport.rpt`
- `crpDayWiseBusinessReport.rpt`
- `crpDcOpeningStockReport.rpt`
- `crpDelOrderReport.rpt`
- `crpDeleteOrder.rpt`
- `crpHistoricalReport.rpt`
- `crpPettyCashReport.rpt`
- `crpProductWiseNationalSalesReport.rpt`
- `crpProductWiseNationalSalesReportZoneWise.rpt`
- `crpSalesRejection.rpt`
- `crpSalesTrendReport.rpt`
- `crpWhOpeningStockReport.rpt`
- `crpWhStockMonitoring.rpt`
- `rptAllProductCountryStock.rpt`
- `rptBatchWiseTopSheet.rpt`
- `rptChalanDCToDC.rpt`
- `rptChalanForWarehouse.rpt`
- `rptChalanReceive.rpt`
- `rptComUnitSales.rpt`
- `rptCompanySales.rpt`
- `rptCountryStock.rpt`
- `rptCustMaster.rpt`
- `rptCustomerSales.rpt`
- `rptDCPicking.rpt`
- `rptDCPickingListReport.rpt`
- `rptDCwiseCountryStock.rpt`
- `rptDcPickingSlip.rpt`
- `rptDcToDcChalan.rpt`
- `rptDelivaryTopSheet.rpt`
- `rptDistrictWiseSales.rpt`
- `rptGroupInvoiceReport.rpt`
- `rptMiaSales.rpt`
- `rptMiaWiseSales.rpt`
- `rptMigoDetail.rpt`
- `rptOrderDetails.rpt`
- `rptProductCostPrice.rpt`
- `rptProductPriceReport.rpt`
- `rptProductTradePrice.rpt`
- `rptRegionSales.rpt`
- `rptSalesComparisionReport.rpt`
- `rptSalesRejection.rpt`
- `rptSpecialDiscount.rpt`
- `rptStockTransfarOrder.rpt`
- `rptWarehouseBinCard.rpt`
- `rptZoneSales.rpt`
