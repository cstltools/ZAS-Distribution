# API Inventory

This is a **Web Forms postback application**. It has no REST API, no Web API controllers, no OData and no MVC routes. The complete HTTP surface that is not a plain page postback is listed here.

## 1. ASMX SOAP / script service

Two `.asmx` endpoints exist and both point at the *same* implementation class:

| Endpoint | Implementation |
|---|---|
| `/SInventory_UI/SInventoryWebService.asmx` | `Solution.Web/App_Code/SInventoryWebService.cs` (`class SInventoryWebService`) |
| `/SInventory_UI/WebService.asmx` | `Solution.Web/App_Code/SInventoryWebService.cs` (same class — duplicate endpoint) |

It is consumed by AJAX Control Toolkit `AutoCompleteExtender` controls, which POST `{"prefixText":"...","count":N,"contextKey":"..."}` and expect `string[]`.

| # | Method | Signature | Session | Auth | Response |
|---|---|---|---|---|---|
| 1 | `HelloWorld` | `string HelloWorld()` | no | **none** | JSON `string` |
| 2 | `GetPerticularaction` | `string[] GetPerticularaction(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 3 | `loadCustomerInfoforProductdiscount` | `string[] loadCustomerInfoforProductdiscount(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 4 | `loadCustomernew` | `string[] loadCustomernew(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 5 | `GetCustomerInfo` | `string[] GetCustomerInfo(string prefixText)` | no | **none** | JSON `string[]` |
| 6 | `GetCustomerInfoByCode` | `string[] GetCustomerInfoByCode(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 7 | `GetApprovedProduct` | `string[] GetApprovedProduct(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 8 | `GetProduct` | `string[] GetProduct(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 9 | `GetCustomer` | `string[] GetCustomer(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 10 | `GetCustomernew` | `string[] GetCustomernew(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 11 | `GetDelivaryInfo` | `string[] GetDelivaryInfo(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 12 | `GetCustomerWithMIO` | `string[] GetCustomerWithMIO(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 13 | `GetProduct2` | `string[] GetProduct2(string prefixText)` | no | **none** | JSON `string[]` |
| 14 | `GetProduct3` | `string[] GetProduct3(string prefixText)` | no | **none** | JSON `string[]` |
| 15 | `GetEmpInfo` | `string[] GetEmpInfo(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 16 | `GetConsumerCustomer` | `string[] GetConsumerCustomer(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |
| 17 | `GetProductByMenufracturer` | `string[] GetProductByMenufracturer(string prefixText, string contextKey)` | no | **none** | JSON `string[]` |
| 18 | `GetInvoiceCustomer` | `string[] GetInvoiceCustomer(string prefixText)` | EnableSession=true | **none** | JSON `string[]` |

**Security note:** none of these methods checks `Session["UserId"]`, and each builds its SQL with `LIKE '%' + prefixText + '%'` by string concatenation. See `SPEC-GAPS.md` → `GAP-SEC-004`.

## 2. Page-level `[WebMethod]` endpoints

These are static page methods called from the page's own JavaScript (`PageMethods` / `$.ajax` to `Page.aspx/MethodName`).

| Page | Method | Signature |
|---|---|---|
| `/SInventory_UI/CashFlow.aspx` | `GetCompanyWiseSales` | `CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)` |
| `/SInventory_UI/CashFlow.aspx` | `GetFinancialYearDate` | `FinancialYearDao[] GetFinancialYearDate(string companyId, string finyearId)` |
| `/SInventory_UI/CashFlow.aspx` | `GetCashflowData` | `CashFlowDao[] GetCashflowData(string companyId, string finyearId, string startdate, string enddate)` |
| `/SInventory_UI/CashFlow.aspx` | `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)` |
| `/SInventory_UI/CashFlow.aspx` | `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)` |
| `/SInventory_UI/CashFlow.aspx` | `GetWingsSalesTrend` | `WingsSalesTrend[] GetWingsSalesTrend(string year)` |
| `/SInventory_UI/CashFlow.aspx` | `GetDashBoardCardInfo` | `SalesDashboardDao[] GetDashBoardCardInfo(string companyId)` |
| `/SInventory_UI/CashFlow.aspx` | `GetFinancialYear` | `FinancialYearDao[] GetFinancialYear(string companyId)` |
| `/SInventory_UI/CashFlow.aspx` | `GetCompanyInfo` | `CompanyDao[] GetCompanyInfo()` |
| `/SInventory_UI/CashFlow.aspx` | `GetAglingReport` | `AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string month)` |
| `/SInventory_UI/Dashboard.aspx` | `GetCompanyWiseSales` | `CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)` |
| `/SInventory_UI/Dashboard.aspx` | `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)` |
| `/SInventory_UI/Dashboard.aspx` | `GetCompanyWiseInvoice` | `CompanyWiseInvoiceDao[] GetCompanyWiseInvoice(string companyId, string fromDate, string toDate)` |
| `/SInventory_UI/Dashboard.aspx` | `GetMioWiseInvoice` | `MioWiseInvoiceDao[] GetMioWiseInvoice(string companyId, string fromDate, string toDate, string mioId)` |
| `/SInventory_UI/Dashboard.aspx` | `GetTop50Customer` | `Top50CustomerDao[] GetTop50Customer(string companyId, string fromDate, string toDate)` |
| `/SInventory_UI/Dashboard.aspx` | `GetTopPriorityProductsales` | `TopPriorityProductSalesDao[] GetTopPriorityProductsales(string companyId, string depotId, string fromDate, string toDate)` |
| `/SInventory_UI/Dashboard.aspx` | `GetCompanyWiseExpireProduct` | `ExpireProductDao[] GetCompanyWiseExpireProduct(string companyId, string expireIn)` |
| `/SInventory_UI/Dashboard.aspx` | `GetCompanyWiseProductSales` | `CompanyWiseProductSalesDao[] GetCompanyWiseProductSales(string companyId, string fromDate, string toDate)` |
| `/SInventory_UI/Dashboard.aspx` | `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)` |
| `/SInventory_UI/Dashboard.aspx` | `GetRegionInfo` | `RegionDao[] GetRegionInfo()` |
| `/SInventory_UI/Dashboard.aspx` | `GetDepotInfo` | `DepotDao[] GetDepotInfo(string companyId)` |
| `/SInventory_UI/Dashboard.aspx` | `GetWingsSalesTrend` | `WingsSalesTrend[] GetWingsSalesTrend(string year)` |
| `/SInventory_UI/Dashboard.aspx` | `GetDashBoardCardInfo` | `SalesDashboardDao[] GetDashBoardCardInfo(string companyId, string fromDate, string toDate)` |
| `/SInventory_UI/Dashboard.aspx` | `GetFinancialYear` | `FinancialYearDao[] GetFinancialYear(string companyId)` |
| `/SInventory_UI/Dashboard.aspx` | `GetMIOInfo` | `MIODao[] GetMIOInfo(string companyId)` |
| `/SInventory_UI/Dashboard.aspx` | `GetCompanyInfo` | `CompanyDao[] GetCompanyInfo()` |
| `/SInventory_UI/Dashboard.aspx` | `GetAglingReport` | `AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string month)` |
| `/SInventory_UI/Dashboard.aspx` | `GetBusinessSummeryReport` | `BusinessSummeryReportDao[] GetBusinessSummeryReport(string companyId, string timePeriod, string year, string month)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetIncomeStatementData` | `IncomeSatementDao[] GetIncomeStatementData(string companyId, string finyearId, string startdate, string enddate)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetCompanyWiseSales` | `CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetFinancialYearDate` | `FinancialYearDao[] GetFinancialYearDate(string companyId, string finyearId)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetCashflowData` | `CashFlowDao[] GetCashflowData(string companyId, string finyearId, string startdate, string enddate)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetWingsSalesTrend` | `WingsSalesTrend[] GetWingsSalesTrend(string year)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetDashBoardCardInfo` | `SalesDashboardDao[] GetDashBoardCardInfo(string companyId)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetFinancialYear` | `FinancialYearDao[] GetFinancialYear(string companyId)` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetCompanyInfo` | `CompanyDao[] GetCompanyInfo()` |
| `/SInventory_UI/IncomeStatement.aspx` | `GetAglingReport` | `AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string month)` |
| `/SInventory_UI/TradeMaterialTransfer.aspx` | `GetRawMaterial` | `string[] GetRawMaterial(string prefixText)` |

## 3. HTTP handlers (`.ashx`)

| Handler | Behaviour |
|---|---|
| `/PictureHandler.ashx` | Writes `Session["ImageBytes"]` to the response as `image/JPEG`. |
| `/SignatureHandler.ashx` | Guards on `Session["ImageBytes"]` but writes `Session["SigImageBytes"]` — if only the signature is set, the handler returns an empty body. (CONFIRMED bug, see `SPEC-GAPS.md` → `GAP-APP-001`.) |

| `/CrystalImageHandler.aspx` | Registered in `Web.config`; serves images for the Crystal viewer. |

## 4. What does *not* exist

- No REST/JSON API for the business entities.
- No authentication token, API key or CORS configuration.
- No versioning, no rate limiting, no request/response validation layer.
- No Swagger/OpenAPI description.
