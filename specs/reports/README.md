# Module: reports

The reporting surface: 146 pages made up of parameter-entry screens in `SInventory_UI`, Crystal Report viewers in `SInventory_RPTVIEW`, and hand-built HTML reports in `HTMLReport`.

The usual flow is: a parameter screen collects filters, writes them into `Session` (and sometimes the query string), then opens a viewer in a `window.open` popup. The viewer loads a `.rpt` from `Solution.Web/Reports/CrystalReports` and binds a `DataTable` from a `*ReportDal`.

[REPORT-INVENTORY.md](REPORT-INVENTORY.md) maps every viewer to its `.rpt`, its parameters and its data layer, and lists the 48 report definitions no viewer appears to reference.

**Most viewer pages do not use the master page and therefore have no authentication guard** (`GAP-SEC-002`). Report internals — layout, formulas, grouping, suppression — are not readable from a binary `.rpt` (`GAP-RPT-001`).

## Pages (146)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `RPT-001` | `AllSalesreportHTMLReport.aspx` | `/HTMLReport/AllSalesreportHTMLReport.aspx` | HTML report | 22 | 1 | 3 | **no** | [spec](pages/PAGE-RPT-001-AllSalesreportHTMLReport.md) |
| `RPT-002` | `CustomerCoverageHTMLReportDZSMWise.aspx` | `/HTMLReport/CustomerCoverageHTMLReportDZSMWise.aspx` | HTML report | 12 | 1 | 3 | **no** | [spec](pages/PAGE-RPT-002-CustomerCoverageHTMLReportDZSMWise.md) |
| `RPT-003` | `CustomerCoverageHTMLReportFEWise.aspx` | `/HTMLReport/CustomerCoverageHTMLReportFEWise.aspx` | HTML report | 0 | 0 | 1 | **no** | [spec](pages/PAGE-RPT-003-CustomerCoverageHTMLReportFEWise.md) |
| `RPT-004` | `CustomerCoverageHTMLReportMiaWise.aspx` | `/HTMLReport/CustomerCoverageHTMLReportMiaWise.aspx` | HTML report | 11 | 1 | 3 | **no** | [spec](pages/PAGE-RPT-004-CustomerCoverageHTMLReportMiaWise.md) |
| `RPT-005` | `DCStockHTMLReport.aspx` | `/HTMLReport/DCStockHTMLReport.aspx` | HTML report | 35 | 2 | 4 | **no** | [spec](pages/PAGE-RPT-005-DCStockHTMLReport.md) |
| `RPT-006` | `DCStockValuationHtmlReport.aspx` | `/HTMLReport/DCStockValuationHtmlReport.aspx` | HTML report | 19 | 1 | 8 | **no** | [spec](pages/PAGE-RPT-006-DCStockValuationHtmlReport.md) |
| `RPT-007` | `DelivaryReturnHTMLReport.aspx` | `/HTMLReport/DelivaryReturnHTMLReport.aspx` | HTML report | 23 | 1 | 3 | **no** | [spec](pages/PAGE-RPT-007-DelivaryReturnHTMLReport.md) |
| `RPT-008` | `InvoicePrintPreview.aspx` | `/HTMLReport/InvoicePrintPreview.aspx` | HTML report | 0 | 0 | 2 | **no** | [spec](pages/PAGE-RPT-008-InvoicePrintPreview.md) |
| `RPT-009` | `NationalBeenCardHTMLReport.aspx` | `/HTMLReport/NationalBeenCardHTMLReport.aspx` | HTML report | 42 | 2 | 4 | **no** | [spec](pages/PAGE-RPT-009-NationalBeenCardHTMLReport.md) |
| `RPT-010` | `NationalStockReportHTMLReport.aspx` | `/HTMLReport/NationalStockReportHTMLReport.aspx` | HTML report | 23 | 1 | 4 | **no** | [spec](pages/PAGE-RPT-010-NationalStockReportHTMLReport.md) |
| `RPT-011` | `ProductWiseNationalSalesHTMLReport.aspx` | `/HTMLReport/ProductWiseNationalSalesHTMLReport.aspx` | HTML report | 20 | 2 | 4 | **no** | [spec](pages/PAGE-RPT-011-ProductWiseNationalSalesHTMLReport.md) |
| `RPT-012` | `ProformaHTMLReport.aspx` | `/HTMLReport/ProformaHTMLReport.aspx` | HTML report | 25 | 1 | 3 | **no** | [spec](pages/PAGE-RPT-012-ProformaHTMLReport.md) |
| `RPT-013` | `SalesPerformanceReport.aspx` | `/HTMLReport/SalesPerformanceReport.aspx` | HTML report | 2 | 0 | 9 | **no** | [spec](pages/PAGE-RPT-013-SalesPerformanceReport.md) |
| `RPT-014` | `SalesSummeryForAccHtmlReport.aspx` | `/HTMLReport/SalesSummeryForAccHtmlReport.aspx` | HTML report | 17 | 1 | 5 | **no** | [spec](pages/PAGE-RPT-014-SalesSummeryForAccHtmlReport.md) |
| `RPT-015` | `SalesTrackingHTMLReport.aspx` | `/HTMLReport/SalesTrackingHTMLReport.aspx` | HTML report | 31 | 2 | 4 | **no** | [spec](pages/PAGE-RPT-015-SalesTrackingHTMLReport.md) |
| `RPT-016` | `StockHTMLReport.aspx` | `/HTMLReport/StockHTMLReport.aspx` | HTML report | 44 | 3 | 3 | **no** | [spec](pages/PAGE-RPT-016-StockHTMLReport.md) |
| `RPT-017` | `WHBeenCardHTMLReport.aspx` | `/HTMLReport/WHBeenCardHTMLReport.aspx` | HTML report | 35 | 2 | 4 | **no** | [spec](pages/PAGE-RPT-017-WHBeenCardHTMLReport.md) |
| `RPT-018` | `WHStockValuationHtmlReport.aspx` | `/HTMLReport/WHStockValuationHtmlReport.aspx` | HTML report | 19 | 1 | 8 | **no** | [spec](pages/PAGE-RPT-018-WHStockValuationHtmlReport.md) |
| `RPT-019` | `AITReceiptViewer.aspx` | `/SInventory_RPTVIEW/AITReceiptViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-019-AITReceiptViewer.md) |
| `RPT-020` | `ASMWiseProductSalesReportViewer.aspx` | `/SInventory_RPTVIEW/ASMWiseProductSalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-020-ASMWiseProductSalesReportViewer.md) |
| `RPT-021` | `AllSalesreportViewer.aspx` | `/SInventory_RPTVIEW/AllSalesreportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-021-AllSalesreportViewer.md) |
| `RPT-022` | `AuditReportOneReportViewer.aspx` | `/SInventory_RPTVIEW/AuditReportOneReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-022-AuditReportOneReportViewer.md) |
| `RPT-023` | `B2BReceiveViewer.aspx` | `/SInventory_RPTVIEW/B2BReceiveViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-023-B2BReceiveViewer.md) |
| `RPT-024` | `BatchWiseCollectionReportViewer.aspx` | `/SInventory_RPTVIEW/BatchWiseCollectionReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-024-BatchWiseCollectionReportViewer.md) |
| `RPT-025` | `BatchWiseCollectionSheetReportViewer.aspx` | `/SInventory_RPTVIEW/BatchWiseCollectionSheetReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-025-BatchWiseCollectionSheetReportViewer.md) |
| `RPT-026` | `BatchWiseCollectionViewer.aspx` | `/SInventory_RPTVIEW/BatchWiseCollectionViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-026-BatchWiseCollectionViewer.md) |
| `RPT-027` | `BatchWiseDelReturnViewer.aspx` | `/SInventory_RPTVIEW/BatchWiseDelReturnViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-027-BatchWiseDelReturnViewer.md) |
| `RPT-028` | `BatchWiseDeliveryTopSheetReportViewer.aspx` | `/SInventory_RPTVIEW/BatchWiseDeliveryTopSheetReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-028-BatchWiseDeliveryTopSheetReportViewer.md) |
| `RPT-029` | `BatchWiseMoneyReceiptReportViewer.aspx` | `/SInventory_RPTVIEW/BatchWiseMoneyReceiptReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-029-BatchWiseMoneyReceiptReportViewer.md) |
| `RPT-030` | `BatchWiseTopSheetReportViewer.aspx` | `/SInventory_RPTVIEW/BatchWiseTopSheetReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-030-BatchWiseTopSheetReportViewer.md) |
| `RPT-031` | `BusinessSummaryViewer.aspx` | `/SInventory_RPTVIEW/BusinessSummaryViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-031-BusinessSummaryViewer.md) |
| `RPT-032` | `ChallanReportViewer.aspx` | `/SInventory_RPTVIEW/ChallanReportViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-032-ChallanReportViewer.md) |
| `RPT-033` | `CreditCustomerViewer.aspx` | `/SInventory_RPTVIEW/CreditCustomerViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-033-CreditCustomerViewer.md) |
| `RPT-034` | `CreditReportAllViewer.aspx` | `/SInventory_RPTVIEW/CreditReportAllViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-034-CreditReportAllViewer.md) |
| `RPT-035` | `CusTPaymentReportViewer.aspx` | `/SInventory_RPTVIEW/CusTPaymentReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-035-CusTPaymentReportViewer.md) |
| `RPT-036` | `CustomerDueReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerDueReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-036-CustomerDueReportViewer.md) |
| `RPT-037` | `CustomerLedgerViewer.aspx` | `/SInventory_RPTVIEW/CustomerLedgerViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-037-CustomerLedgerViewer.md) |
| `RPT-038` | `CustomerMasterViewer.aspx` | `/SInventory_RPTVIEW/CustomerMasterViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-038-CustomerMasterViewer.md) |
| `RPT-039` | `CustomerPaymentReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerPaymentReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-039-CustomerPaymentReportViewer.md) |
| `RPT-040` | `CustomerPaymentViewer.aspx` | `/SInventory_RPTVIEW/CustomerPaymentViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-040-CustomerPaymentViewer.md) |
| `RPT-041` | `CustomerReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-041-CustomerReportViewer.md) |
| `RPT-042` | `CustomerReturnByMioReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerReturnByMioReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-042-CustomerReturnByMioReportViewer.md) |
| `RPT-043` | `CustomerWiseLedgerReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerWiseLedgerReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-043-CustomerWiseLedgerReportViewer.md) |
| `RPT-044` | `CustomerWiseProductSalesReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerWiseProductSalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-044-CustomerWiseProductSalesReportViewer.md) |
| `RPT-045` | `CustomerWiseReturnReportByProductViewer.aspx` | `/SInventory_RPTVIEW/CustomerWiseReturnReportByProductViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-045-CustomerWiseReturnReportByProductViewer.md) |
| `RPT-046` | `CustomerWiseReturnReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerWiseReturnReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-046-CustomerWiseReturnReportViewer.md) |
| `RPT-047` | `CustomerWiseSalesReportViewer.aspx` | `/SInventory_RPTVIEW/CustomerWiseSalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-047-CustomerWiseSalesReportViewer.md) |
| `RPT-048` | `DCStockInfoReportViewer.aspx` | `/SInventory_RPTVIEW/DCStockInfoReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-048-DCStockInfoReportViewer.md) |
| `RPT-049` | `DCStockReportViewer.aspx` | `/SInventory_RPTVIEW/DCStockReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-049-DCStockReportViewer.md) |
| `RPT-050` | `DcToDcChalanReportViewer.aspx` | `/SInventory_RPTVIEW/DcToDcChalanReportViewer.aspx` | Report viewer | 4 | 0 | 4 | **no** | [spec](pages/PAGE-RPT-050-DcToDcChalanReportViewer.md) |
| `RPT-051` | `DeleteInvoiceReportViewer.aspx` | `/SInventory_RPTVIEW/DeleteInvoiceReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-051-DeleteInvoiceReportViewer.md) |
| `RPT-052` | `DelivaryInvoiceReturnViewer.aspx` | `/SInventory_RPTVIEW/DelivaryInvoiceReturnViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-052-DelivaryInvoiceReturnViewer.md) |
| `RPT-053` | `DeliveryReturnReportViewer.aspx` | `/SInventory_RPTVIEW/DeliveryReturnReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-053-DeliveryReturnReportViewer.md) |
| `RPT-054` | `DepositReportViewer.aspx` | `/SInventory_RPTVIEW/DepositReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-054-DepositReportViewer.md) |
| `RPT-055` | `DestroyProductReportViewer.aspx` | `/SInventory_RPTVIEW/DestroyProductReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-055-DestroyProductReportViewer.md) |
| `RPT-056` | `DestroyProductStatementViewer.aspx` | `/SInventory_RPTVIEW/DestroyProductStatementViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-056-DestroyProductStatementViewer.md) |
| `RPT-057` | `DirectStockOutViewer.aspx` | `/SInventory_RPTVIEW/DirectStockOutViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-057-DirectStockOutViewer.md) |
| `RPT-058` | `EmployeeInformationReportViewer.aspx` | `/SInventory_RPTVIEW/EmployeeInformationReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-058-EmployeeInformationReportViewer.md) |
| `RPT-059` | `EmployeewiseProductSalesReportViewer.aspx` | `/SInventory_RPTVIEW/EmployeewiseProductSalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-059-EmployeewiseProductSalesReportViewer.md) |
| `RPT-060` | `FieldForceSalesSummeryReportViewer.aspx` | `/SInventory_RPTVIEW/FieldForceSalesSummeryReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-060-FieldForceSalesSummeryReportViewer.md) |
| `RPT-061` | `FreezeStockReportViewer.aspx` | `/SInventory_RPTVIEW/FreezeStockReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-061-FreezeStockReportViewer.md) |
| `RPT-062` | `GroupInvoiceReportViewer.aspx` | `/SInventory_RPTVIEW/GroupInvoiceReportViewer.aspx` | Report viewer | 4 | 0 | 6 | **no** | [spec](pages/PAGE-RPT-062-GroupInvoiceReportViewer.md) |
| `RPT-063` | `InTransitReportViewer.aspx` | `/SInventory_RPTVIEW/InTransitReportViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-063-InTransitReportViewer.md) |
| `RPT-064` | `InvoiceReportViewer.aspx` | `/SInventory_RPTVIEW/InvoiceReportViewer.aspx` | Report viewer | 3 | 0 | 6 | **no** | [spec](pages/PAGE-RPT-064-InvoiceReportViewer.md) |
| `RPT-065` | `MIGOReportViewer.aspx` | `/SInventory_RPTVIEW/MIGOReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-065-MIGOReportViewer.md) |
| `RPT-066` | `MIOSalesSummeryReportViewer.aspx` | `/SInventory_RPTVIEW/MIOSalesSummeryReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-066-MIOSalesSummeryReportViewer.md) |
| `RPT-067` | `MIOWiseProformaReportViewer.aspx` | `/SInventory_RPTVIEW/MIOWiseProformaReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-067-MIOWiseProformaReportViewer.md) |
| `RPT-068` | `MarketwisePicking.aspx` | `/SInventory_RPTVIEW/MarketwisePicking.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-068-MarketwisePicking.md) |
| `RPT-069` | `MiaWiseSalesReportViewer.aspx` | `/SInventory_RPTVIEW/MiaWiseSalesReportViewer.aspx` | Report viewer | 4 | 0 | 4 | **no** | [spec](pages/PAGE-RPT-069-MiaWiseSalesReportViewer.md) |
| `RPT-070` | `MioWiseCustomerReportViewer.aspx` | `/SInventory_RPTVIEW/MioWiseCustomerReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-070-MioWiseCustomerReportViewer.md) |
| `RPT-071` | `MioWiseProductSalesReportViewer.aspx` | `/SInventory_RPTVIEW/MioWiseProductSalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-071-MioWiseProductSalesReportViewer.md) |
| `RPT-072` | `NTInvoiceReturnReportViewer.aspx` | `/SInventory_RPTVIEW/NTInvoiceReturnReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-072-NTInvoiceReturnReportViewer.md) |
| `RPT-073` | `NationalStockReportViewer.aspx` | `/SInventory_RPTVIEW/NationalStockReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-073-NationalStockReportViewer.md) |
| `RPT-074` | `NearlyExpireproductReportViewer.aspx` | `/SInventory_RPTVIEW/NearlyExpireproductReportViewer.aspx` | Report viewer | 3 | 0 | 6 | **no** | [spec](pages/PAGE-RPT-074-NearlyExpireproductReportViewer.md) |
| `RPT-075` | `PaymentReportViewer.aspx` | `/SInventory_RPTVIEW/PaymentReportViewer.aspx` | Report viewer | 3 | 0 | 6 | **no** | [spec](pages/PAGE-RPT-075-PaymentReportViewer.md) |
| `RPT-076` | `PickingSlipGenerateViewer.aspx` | `/SInventory_RPTVIEW/PickingSlipGenerateViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-076-PickingSlipGenerateViewer.md) |
| `RPT-077` | `ProductCostPriceViewer.aspx` | `/SInventory_RPTVIEW/ProductCostPriceViewer.aspx` | Report viewer | 4 | 0 | 4 | **no** | [spec](pages/PAGE-RPT-077-ProductCostPriceViewer.md) |
| `RPT-078` | `ProductDiscountReportAllViewer.aspx` | `/SInventory_RPTVIEW/ProductDiscountReportAllViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-078-ProductDiscountReportAllViewer.md) |
| `RPT-079` | `ProductDiscountReportViewer.aspx` | `/SInventory_RPTVIEW/ProductDiscountReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-079-ProductDiscountReportViewer.md) |
| `RPT-080` | `ProductTreadPriceViewer.aspx` | `/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-080-ProductTreadPriceViewer.md) |
| `RPT-081` | `ProformaReportPrintViewer.aspx` | `/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx` | Report viewer | 4 | 0 | 6 | **no** | [spec](pages/PAGE-RPT-081-ProformaReportPrintViewer.md) |
| `RPT-082` | `ProformaReportViewer.aspx` | `/SInventory_RPTVIEW/ProformaReportViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-082-ProformaReportViewer.md) |
| `RPT-083` | `RSMWiseProductSalesReportViewer.aspx` | `/SInventory_RPTVIEW/RSMWiseProductSalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-083-RSMWiseProductSalesReportViewer.md) |
| `RPT-084` | `ReturnOnDeliveryReportViewer.aspx` | `/SInventory_RPTVIEW/ReturnOnDeliveryReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-084-ReturnOnDeliveryReportViewer.md) |
| `RPT-085` | `SalesAdministrationReportViewer.aspx` | `/SInventory_RPTVIEW/SalesAdministrationReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-085-SalesAdministrationReportViewer.md) |
| `RPT-086` | `SalesHubReportViewer.aspx` | `/SInventory_RPTVIEW/SalesHubReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-086-SalesHubReportViewer.md) |
| `RPT-087` | `SalesReportViewer.aspx` | `/SInventory_RPTVIEW/SalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-087-SalesReportViewer.md) |
| `RPT-088` | `SalesTransctionReportViewer.aspx` | `/SInventory_RPTVIEW/SalesTransctionReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-088-SalesTransctionReportViewer.md) |
| `RPT-089` | `SampleIssueReportViewer.aspx` | `/SInventory_RPTVIEW/SampleIssueReportViewer.aspx` | Report viewer | 3 | 0 | 6 | **no** | [spec](pages/PAGE-RPT-089-SampleIssueReportViewer.md) |
| `RPT-090` | `StockReceiveReportViewer.aspx` | `/SInventory_RPTVIEW/StockReceiveReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-090-StockReceiveReportViewer.md) |
| `RPT-091` | `TopSheetReportViewer.aspx` | `/SInventory_RPTVIEW/TopSheetReportViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-091-TopSheetReportViewer.md) |
| `RPT-092` | `TransferStockReceiveReportViewer.aspx` | `/SInventory_RPTVIEW/TransferStockReceiveReportViewer.aspx` | Report viewer | 4 | 0 | 4 | **no** | [spec](pages/PAGE-RPT-092-TransferStockReceiveReportViewer.md) |
| `RPT-093` | `UserCredentialsReportViewer.aspx` | `/SInventory_RPTVIEW/UserCredentialsReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-093-UserCredentialsReportViewer.md) |
| `RPT-094` | `UserPermissionReportViewer.aspx` | `/SInventory_RPTVIEW/UserPermissionReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-094-UserPermissionReportViewer.md) |
| `RPT-095` | `WHStockInReportViewer.aspx` | `/SInventory_RPTVIEW/WHStockInReportViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-095-WHStockInReportViewer.md) |
| `RPT-096` | `WareHousePickingReportViewer.aspx` | `/SInventory_RPTVIEW/WareHousePickingReportViewer.aspx` | Report viewer | 3 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-096-WareHousePickingReportViewer.md) |
| `RPT-097` | `WingsSalesAndCollectionReportViewer.aspx` | `/SInventory_RPTVIEW/WingsSalesAndCollectionReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-097-WingsSalesAndCollectionReportViewer.md) |
| `RPT-098` | `WingsWiseCollectionReportViewer.aspx` | `/SInventory_RPTVIEW/WingsWiseCollectionReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-098-WingsWiseCollectionReportViewer.md) |
| `RPT-099` | `WingsWiseDepositReportViewer.aspx` | `/SInventory_RPTVIEW/WingsWiseDepositReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-099-WingsWiseDepositReportViewer.md) |
| `RPT-100` | `WingsWiseSalesReportViewer.aspx` | `/SInventory_RPTVIEW/WingsWiseSalesReportViewer.aspx` | Report viewer | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-100-WingsWiseSalesReportViewer.md) |
| `RPT-101` | `ASMReport.aspx` | `/SInventory_UI/ASMReport.aspx` | Report parameters | 14 | 0 | 13 | yes | [spec](pages/PAGE-RPT-101-ASMReport.md) |
| `RPT-102` | `ASMWiseProductSalesReport.aspx` | `/SInventory_UI/ASMWiseProductSalesReport.aspx` | Report parameters | 17 | 0 | 11 | yes | [spec](pages/PAGE-RPT-102-ASMWiseProductSalesReport.md) |
| `RPT-103` | `AllSalesReport.aspx` | `/SInventory_UI/AllSalesReport.aspx` | Report parameters | 17 | 0 | 10 | yes | [spec](pages/PAGE-RPT-103-AllSalesReport.md) |
| `RPT-104` | `AreaWiseMonthlyInventoryReport.aspx` | `/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx` | Report parameters | 13 | 0 | 6 | yes | [spec](pages/PAGE-RPT-104-AreaWiseMonthlyInventoryReport.md) |
| `RPT-105` | `AuditReportOne.aspx` | `/SInventory_UI/AuditReportOne.aspx` | Report parameters | 14 | 0 | 15 | yes | [spec](pages/PAGE-RPT-105-AuditReportOne.md) |
| `RPT-106` | `BatchWiseDelReturnSummery.aspx` | `/SInventory_UI/BatchWiseDelReturnSummery.aspx` | Report parameters | 23 | 1 | 10 | yes | [spec](pages/PAGE-RPT-106-BatchWiseDelReturnSummery.md) |
| `RPT-107` | `BatchWiseTransitReport.aspx` | `/SInventory_UI/BatchWiseTransitReport.aspx` | Report parameters | 27 | 1 | 13 | yes | [spec](pages/PAGE-RPT-107-BatchWiseTransitReport.md) |
| `RPT-108` | `CustomerMasterReport.aspx` | `/SInventory_UI/CustomerMasterReport.aspx` | Report parameters | 11 | 0 | 5 | yes | [spec](pages/PAGE-RPT-108-CustomerMasterReport.md) |
| `RPT-109` | `CustomerReport.aspx` | `/SInventory_UI/CustomerReport.aspx` | Report parameters | 9 | 0 | 9 | yes | [spec](pages/PAGE-RPT-109-CustomerReport.md) |
| `RPT-110` | `CustomerWiseLedgerReport.aspx` | `/SInventory_UI/CustomerWiseLedgerReport.aspx` | Report parameters | 18 | 0 | 12 | yes | [spec](pages/PAGE-RPT-110-CustomerWiseLedgerReport.md) |
| `RPT-111` | `CustomerWiseProductSalesReportByMio.aspx` | `/SInventory_UI/CustomerWiseProductSalesReportByMio.aspx` | Report parameters | 22 | 0 | 12 | yes | [spec](pages/PAGE-RPT-111-CustomerWiseProductSalesReportByMio.md) |
| `RPT-112` | `CustomerWiseReturnReport.aspx` | `/SInventory_UI/CustomerWiseReturnReport.aspx` | Report parameters | 15 | 0 | 11 | yes | [spec](pages/PAGE-RPT-112-CustomerWiseReturnReport.md) |
| `RPT-113` | `CustomerWiseReturnReportByProduct.aspx` | `/SInventory_UI/CustomerWiseReturnReportByProduct.aspx` | Report parameters | 18 | 0 | 11 | yes | [spec](pages/PAGE-RPT-113-CustomerWiseReturnReportByProduct.md) |
| `RPT-114` | `CustomerWiseSalesReport.aspx` | `/SInventory_UI/CustomerWiseSalesReport.aspx` | Report parameters | 27 | 1 | 16 | yes | [spec](pages/PAGE-RPT-114-CustomerWiseSalesReport.md) |
| `RPT-115` | `DeleteInvoiceReport.aspx` | `/SInventory_UI/DeleteInvoiceReport.aspx` | Report parameters | 14 | 0 | 15 | yes | [spec](pages/PAGE-RPT-115-DeleteInvoiceReport.md) |
| `RPT-116` | `DeliveryInvoiceReport.aspx` | `/SInventory_UI/DeliveryInvoiceReport.aspx` | Report parameters | 27 | 1 | 14 | yes | [spec](pages/PAGE-RPT-116-DeliveryInvoiceReport.md) |
| `RPT-117` | `EmployeeInformationReport.aspx` | `/SInventory_UI/EmployeeInformationReport.aspx` | Report parameters | 13 | 0 | 12 | yes | [spec](pages/PAGE-RPT-117-EmployeeInformationReport.md) |
| `RPT-118` | `FieldForceSalesSummery.aspx` | `/SInventory_UI/FieldForceSalesSummery.aspx` | Report parameters | 17 | 0 | 12 | yes | [spec](pages/PAGE-RPT-118-FieldForceSalesSummery.md) |
| `RPT-119` | `InTransitReport.aspx` | `/SInventory_UI/InTransitReport.aspx` | Report parameters | 13 | 0 | 7 | yes | [spec](pages/PAGE-RPT-119-InTransitReport.md) |
| `RPT-120` | `InvoiceReport.aspx` | `/SInventory_UI/InvoiceReport.aspx` | Report parameters | 32 | 1 | 18 | yes | [spec](pages/PAGE-RPT-120-InvoiceReport.md) |
| `RPT-121` | `MIAWiseSalesReport.aspx` | `/SInventory_UI/MIAWiseSalesReport.aspx` | Report parameters | 17 | 0 | 4 | yes | [spec](pages/PAGE-RPT-121-MIAWiseSalesReport.md) |
| `RPT-122` | `MIOSalesSummeryReport.aspx` | `/SInventory_UI/MIOSalesSummeryReport.aspx` | Report parameters | 15 | 0 | 11 | yes | [spec](pages/PAGE-RPT-122-MIOSalesSummeryReport.md) |
| `RPT-123` | `MIOWiseProformaReport.aspx` | `/SInventory_UI/MIOWiseProformaReport.aspx` | Report parameters | 15 | 0 | 11 | yes | [spec](pages/PAGE-RPT-123-MIOWiseProformaReport.md) |
| `RPT-124` | `MiaWiseCustomerCoverageReport.aspx` | `/SInventory_UI/MiaWiseCustomerCoverageReport.aspx` | Report parameters | 11 | 0 | 7 | yes | [spec](pages/PAGE-RPT-124-MiaWiseCustomerCoverageReport.md) |
| `RPT-125` | `MioWiseCustomerReport.aspx` | `/SInventory_UI/MioWiseCustomerReport.aspx` | Report parameters | 9 | 0 | 9 | yes | [spec](pages/PAGE-RPT-125-MioWiseCustomerReport.md) |
| `RPT-126` | `MioWiseProductSalesReport.aspx` | `/SInventory_UI/MioWiseProductSalesReport.aspx` | Report parameters | 15 | 0 | 15 | yes | [spec](pages/PAGE-RPT-126-MioWiseProductSalesReport.md) |
| `RPT-127` | `ProductCostPriceReport.aspx` | `/SInventory_UI/ProductCostPriceReport.aspx` | Report parameters | 4 | 0 | 3 | yes | [spec](pages/PAGE-RPT-127-ProductCostPriceReport.md) |
| `RPT-128` | `ProductDiscountReport.aspx` | `/SInventory_UI/ProductDiscountReport.aspx` | Report parameters | 10 | 1 | 9 | yes | [spec](pages/PAGE-RPT-128-ProductDiscountReport.md) |
| `RPT-129` | `ProductPriceReport.aspx` | `/SInventory_UI/ProductPriceReport.aspx` | Report parameters | 10 | 0 | 5 | yes | [spec](pages/PAGE-RPT-129-ProductPriceReport.md) |
| `RPT-130` | `ProductWiseNationalSalesReport.aspx` | `/SInventory_UI/ProductWiseNationalSalesReport.aspx` | Report parameters | 13 | 0 | 9 | yes | [spec](pages/PAGE-RPT-130-ProductWiseNationalSalesReport.md) |
| `RPT-131` | `ProformaReport.aspx` | `/SInventory_UI/ProformaReport.aspx` | Report parameters | 12 | 0 | 7 | yes | [spec](pages/PAGE-RPT-131-ProformaReport.md) |
| `RPT-132` | `RSMWiseProductSalesReport.aspx` | `/SInventory_UI/RSMWiseProductSalesReport.aspx` | Report parameters | 15 | 0 | 11 | yes | [spec](pages/PAGE-RPT-132-RSMWiseProductSalesReport.md) |
| `RPT-133` | `SalesHubReport.aspx` | `/SInventory_UI/SalesHubReport.aspx` | Report parameters | 14 | 0 | 14 | yes | [spec](pages/PAGE-RPT-133-SalesHubReport.md) |
| `RPT-134` | `SalesHubReport2.aspx` | `/SInventory_UI/SalesHubReport2.aspx` | Report parameters | 15 | 0 | 11 | yes | [spec](pages/PAGE-RPT-134-SalesHubReport2.md) |
| `RPT-135` | `SalesPerformanceReportProductWise.aspx` | `/SInventory_UI/SalesPerformanceReportProductWise.aspx` | Report parameters | 14 | 0 | 7 | yes | [spec](pages/PAGE-RPT-135-SalesPerformanceReportProductWise.md) |
| `RPT-136` | `SalesReport.aspx` | `/SInventory_UI/SalesReport.aspx` | Report parameters | 15 | 0 | 11 | yes | [spec](pages/PAGE-RPT-136-SalesReport.md) |
| `RPT-137` | `SalesReports.aspx` | `/SInventory_UI/SalesReports.aspx` | Report parameters | 2 | 0 | 1 | yes | [spec](pages/PAGE-RPT-137-SalesReports.md) |
| `RPT-138` | `SalesSummeryReportForAcc.aspx` | `/SInventory_UI/SalesSummeryReportForAcc.aspx` | Report parameters | 14 | 0 | 9 | yes | [spec](pages/PAGE-RPT-138-SalesSummeryReportForAcc.md) |
| `RPT-139` | `SalesTrackingReport.aspx` | `/SInventory_UI/SalesTrackingReport.aspx` | Report parameters | 33 | 1 | 11 | yes | [spec](pages/PAGE-RPT-139-SalesTrackingReport.md) |
| `RPT-140` | `SalesTranscationReport.aspx` | `/SInventory_UI/SalesTranscationReport.aspx` | Report parameters | 25 | 1 | 11 | yes | [spec](pages/PAGE-RPT-140-SalesTranscationReport.md) |
| `RPT-141` | `SampleInvoiceReport.aspx` | `/SInventory_UI/SampleInvoiceReport.aspx` | Report parameters | 26 | 1 | 14 | yes | [spec](pages/PAGE-RPT-141-SampleInvoiceReport.md) |
| `RPT-142` | `WingsSalesAndCollectionReport.aspx` | `/SInventory_UI/WingsSalesAndCollectionReport.aspx` | Report parameters | 14 | 0 | 9 | yes | [spec](pages/PAGE-RPT-142-WingsSalesAndCollectionReport.md) |
| `RPT-143` | `WingsSalesReport.aspx` | `/SInventory_UI/WingsSalesReport.aspx` | Report parameters | 14 | 0 | 9 | yes | [spec](pages/PAGE-RPT-143-WingsSalesReport.md) |
| `RPT-144` | `WingsWiseCollectionReport.aspx` | `/SInventory_UI/WingsWiseCollectionReport.aspx` | Report parameters | 14 | 0 | 10 | yes | [spec](pages/PAGE-RPT-144-WingsWiseCollectionReport.md) |
| `RPT-145` | `WingsWiseSalesReport.aspx` | `/SInventory_UI/WingsWiseSalesReport.aspx` | Report parameters | 14 | 0 | 9 | yes | [spec](pages/PAGE-RPT-145-WingsWiseSalesReport.md) |
| `RPT-146` | `WingsWiseSalesReportViewer.aspx` | `/SInventory_UI/WingsWiseSalesReportViewer.aspx` | Report parameters | 4 | 0 | 5 | **no** | [spec](pages/PAGE-RPT-146-WingsWiseSalesReportViewer.md) |

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CustomerMaster` | 36 |
| `View_MIAWiseSalesReport` | 1 |
| `tblASMInfo` | 7 |
| `tblArea` | 36 |
| `tblBankInfo` | 2 |
| `tblCentralStore` | 15 |
| `tblChalanDetail` | 13 |
| `tblChalanInfo` | 10 |
| `tblCompanyInfo` | 130 |
| `tblCompanyUnit` | 155 |
| `tblCompanyWiseDeposit` | 2 |
| `tblCustCategory` | 6 |
| `tblCustMaster` | 59 |
| `tblCustPayDetail` | 21 |
| `tblCustomerCreditLimit` | 2 |
| `tblCustomerMasterExcelFileMaster` | 2 |
| `tblCustomerPay` | 19 |
| `tblCustomerReturnMaster` | 2 |
| `tblDCStore` | 27 |
| `tblDCStoreFreeze` | 12 |
| `tblDCStore_OpeningBalance` | 2 |
| `tblDcWiseAreaInfo` | 24 |
| `tblDeliveryManInfo` | 36 |
| `tblDepartment` | 1 |
| `tblDesignation` | 1 |
| `tblDestroyProduct` | 2 |
| `tblDirectStockOut` | 1 |
| `tblEmpGeneralInfo` | 85 |
| `tblGroupInformation` | 25 |
| `tblInvoice` | 106 |
| `tblInvoiceBatch` | 37 |
| `tblInvoiceDeleteLog` | 4 |
| `tblInvoiceDetail` | 72 |
| `tblInvoiceDetail_DeleterRecord` | 4 |
| `tblMIAInfo` | 4 |
| `tblMIATarget` | 2 |
| `tblMIATargetProductWise` | 2 |
| `tblMIGODetail` | 3 |
| `tblMIOInfo` | 47 |
| `tblMainMenu` | 1 |
| `tblManufacturer` | 3 |
| `tblMarket` | 27 |
| `tblMenuDistribution` | 1 |
| `tblMoneyReceipt` | 1 |
| `tblNonTranscationalInvoiceDetail` | 7 |
| `tblNonTranscationalInvoiceMaster` | 1 |
| `tblOrder` | 27 |
| `tblOrderDel` | 4 |
| `tblOrderDetail` | 22 |
| `tblOrderDetailDel` | 4 |
| `tblPackSize` | 5 |
| `tblPaymentTranscationDetail` | 1 |
| `tblPaymentTranscationMaster` | 1 |
| `tblPaymentType` | 6 |
| `tblProCategory` | 1 |
| `tblProType` | 1 |
| `tblProduct` | 39 |
| `tblProductCase` | 1 |
| `tblProductDiscount` | 6 |
| `tblProductSQ` | 1 |
| `tblRSMInfo` | 9 |
| `tblRegion` | 25 |
| `tblRequisition` | 9 |
| `tblRequsitionChild` | 2 |
| `tblSampleIssue` | 3 |
| `tblSampleIssueDetail` | 3 |
| `tblSampleIssueTranscation` | 3 |
| `tblStockInTransfar` | 17 |
| `tblStockUOM` | 3 |
| `tblTerritory` | 37 |
| `tblUnitPrice` | 13 |
| `tblUser` | 11 |
| `tblUserCompanyUnit` | 46 |
| `tblWHStockInDetail` | 11 |
| `tblWHStockInMaster` | 2 |
| `tblWearhouse` | 6 |
| `tblWhStoreFreeze` | 2 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_DCBinCard` | 2 |
| `sp_Deletenvoice` | 9 |
| `sp_GET_EmployeeWiseProductSale` | 1 |
| `sp_GET_WingsSalesReport` | 2 |
| `sp_GET_WingsSalesReportByCompany` | 1 |
| `sp_GET_WingsSalesReportByUnit` | 1 |
| `sp_INV_DCStockValuationReport` | 2 |
| `sp_INV_WHStockValuationReport` | 2 |
| `sp_NationalBinCard` | 4 |
| `sp_Summery_SalesSummerReportForAccounts` | 2 |
| `sp_WearhouseBinCard` | 4 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
