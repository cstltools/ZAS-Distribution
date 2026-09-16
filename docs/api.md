# API

This is a **Web Forms postback application** — no REST API, no Web API controllers, no MVC routes, no
OData, no Swagger/OpenAPI. The entire non-postback HTTP surface is **60 endpoints**: 18 ASMX methods (exposed
on 2 duplicate endpoints), 40 page-level `[WebMethod]`s, and 2 `.ashx` handlers. Source of truth:
[`../specs/00-system/api-inventory.md`](../specs/00-system/api-inventory.md).

There is **no authentication token, API key, or CORS configuration** anywhere in this surface. Authorization,
where it exists at all, is the same session-based, page-level check used by postbacks (`GAP-SEC-002`) — most of
the endpoints below have none.

## (a) ASMX SOAP / script service — 18 methods, 2 endpoints

Both endpoints resolve to the **same** implementation class — one is dead code but still reachable
(`GAP-APP-014`).

| Endpoint | Implementation |
|---|---|
| `/SInventory_UI/SInventoryWebService.asmx` | `Solution.Web/App_Code/SInventoryWebService.cs` (`class SInventoryWebService`) |
| `/SInventory_UI/WebService.asmx` | same class, duplicate endpoint |

Consumed by AJAX Control Toolkit `AutoCompleteExtender` controls: POST body
`{"prefixText":"...","count":N,"contextKey":"..."}`, response `string[]` as JSON.

**Authentication: NONE on any of the 18 methods** (`CONFIRMED`, `GAP-SEC-004`) — no method checks
`Session["UserId"]` or any other credential. **Authorization: NONE.** Every method builds its SQL with
`LIKE '%' + prefixText + '%'` by raw string concatenation — each is independently SQL-injectable, and
together they leak the full customer, product, price and employee masters to any anonymous caller.

| # | HTTP Method | API ID / .NET signature | Session enabled | Request | Response | Calling chain (INFERRED) | Known issues |
|---|---|---|---|---|---|---|---|
| 1 | POST | `HelloWorld` — `string HelloWorld()` | no | none | JSON `string` | n/a — smoke-test method | none |
| 2 | POST | `GetPerticularaction` — `string[] GetPerticularaction(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblAction` (approval action text, INFERRED) | unauthenticated, injectable |
| 3 | POST | `loadCustomerInfoforProductdiscount` — `string[] loadCustomerInfoforProductdiscount(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblCustMaster` via `ProductDiscountDAL`-adjacent lookup (INFERRED) | unauthenticated, injectable |
| 4 | POST | `loadCustomernew` — `string[] loadCustomernew(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblCustMaster` | unauthenticated, injectable |
| 5 | POST | `GetCustomerInfo` — `string[] GetCustomerInfo(string prefixText)` | no | `prefixText` | JSON `string[]` | `tblCustMaster` | unauthenticated, injectable, no session at all |
| 6 | POST | `GetCustomerInfoByCode` — `string[] GetCustomerInfoByCode(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblCustMaster` (by code) | unauthenticated, injectable |
| 7 | POST | `GetApprovedProduct` — `string[] GetApprovedProduct(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblProduct` (approved/active filter, INFERRED) | unauthenticated, injectable |
| 8 | POST | `GetProduct` — `string[] GetProduct(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblProduct` | unauthenticated, injectable |
| 9 | POST | `GetCustomer` — `string[] GetCustomer(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblCustMaster` | unauthenticated, injectable |
| 10 | POST | `GetCustomernew` — `string[] GetCustomernew(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblCustMaster` | duplicate of #4/#9 in effect; unauthenticated |
| 11 | POST | `GetDelivaryInfo` — `string[] GetDelivaryInfo(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblDeliveryManInfo` | unauthenticated, injectable |
| 12 | POST | `GetCustomerWithMIO` — `string[] GetCustomerWithMIO(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblCustMaster` joined to `tblMIOInfo` (INFERRED) | unauthenticated, injectable |
| 13 | POST | `GetProduct2` — `string[] GetProduct2(string prefixText)` | no | `prefixText` | JSON `string[]` | `tblProduct` | unauthenticated, injectable, no session |
| 14 | POST | `GetProduct3` — `string[] GetProduct3(string prefixText)` | no | `prefixText` | JSON `string[]` | `tblProduct` | unauthenticated, injectable, no session |
| 15 | POST | `GetEmpInfo` — `string[] GetEmpInfo(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblEmpGeneralInfo` | unauthenticated, injectable — leaks employee master |
| 16 | POST | `GetConsumerCustomer` — `string[] GetConsumerCustomer(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblConsumerCustomer` | unauthenticated, injectable |
| 17 | POST | `GetProductByMenufracturer` — `string[] GetProductByMenufracturer(string prefixText, string contextKey)` | no | `prefixText`, `contextKey` (the manufacturer id) | JSON `string[]` | `tblProduct` filtered by `tblManufacturer` | unauthenticated, injectable, no session |
| 18 | POST | `GetInvoiceCustomer` — `string[] GetInvoiceCustomer(string prefixText)` | yes | `prefixText` | JSON `string[]` | `tblCustMaster` joined to `tblInvoice` (INFERRED) | unauthenticated, injectable |

## (b) Page-level `[WebMethod]`s — 40 methods, grouped by page

Static page methods invoked via `PageMethods`/`$.ajax` to `Page.aspx/MethodName`. All are dashboard/reporting
pages; none of the 40 accepts an authentication token — whatever session guard the page's master page provides
(or doesn't — `GAP-SEC-002`) is the only gate.

### `/SInventory_UI/CashFlow.aspx` (10 methods)

| Method | Signature | Response DAO | Calling chain (BLL/DAL → tables, INFERRED) |
|---|---|---|---|
| `GetCompanyWiseSales` | `CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)` | `CompanyWiseSalesDao[]` | `SalesDashboardDal` → `tblInvoice` |
| `GetFinancialYearDate` | `FinancialYearDao[] GetFinancialYearDate(string companyId, string finyearId)` | `FinancialYearDao[]` | `SalesDashboardDal.GetFinancialYearDate` → `sp_GET_FinancialYearDate` → `CSTL_ACCDB..tblFinancialYear` |
| `GetCashflowData` | `CashFlowDao[] GetCashflowData(string companyId, string finyearId, string startdate, string enddate)` | `CashFlowDao[]` | `SalesDashboardDal.GetCashFlowBDTDAL` → `sp_GET_CashFlowSummaryBDTCurrency` → `CSTL_ACCDB` ledger/voucher tables |
| `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)` | `CompanyWiseUnitSalesDao[]` | `SalesDashboardDal` → `tblInvoice`/`tblCompanyUnit` |
| `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)` | `GroupSalesCompanyWiseDao[]` | `SalesDashboardDal` → `tblInvoice`/`tblGroupInformation` |
| `GetWingsSalesTrend` | `WingsSalesTrend[] GetWingsSalesTrend(string year)` | `WingsSalesTrend[]` | `SalesDashboardDal` → `sp_GET_WingsSalesReport*` family |
| `GetDashBoardCardInfo` | `SalesDashboardDao[] GetDashBoardCardInfo(string companyId)` | `SalesDashboardDao[]` | `SalesDashboardDal.GetDashBoardCardInfo` → `sp_GET_DashboardCardInfo` |
| `GetFinancialYear` | `FinancialYearDao[] GetFinancialYear(string companyId)` | `FinancialYearDao[]` | `SalesDashboardDal.GetFinancialYear` → `sp_GET_FinancialYear` |
| `GetCompanyInfo` | `CompanyDao[] GetCompanyInfo()` | `CompanyDao[]` | `SalesDashboardDal.GetCompanyInfo` → `sp_GET_CompanyInfo` → `tblCompanyInfo` |
| `GetAglingReport` | `AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string month)` | `AglingReportDao[]` | `SalesDashboardDal.GetAglingReport` → `sp_DSB_AglingReport` |

### `/SInventory_UI/Dashboard.aspx` (17 methods)

| Method | Signature | Calling chain (INFERRED) |
|---|---|---|
| `GetCompanyWiseSales` | `CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)` | `tblInvoice` |
| `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)` | `tblInvoice`/`tblCompanyUnit` |
| `GetCompanyWiseInvoice` | `CompanyWiseInvoiceDao[] GetCompanyWiseInvoice(string companyId, string fromDate, string toDate)` | `tblInvoice` |
| `GetMioWiseInvoice` | `MioWiseInvoiceDao[] GetMioWiseInvoice(string companyId, string fromDate, string toDate, string mioId)` | `tblInvoice` filtered by `tblMIOInfo` |
| `GetTop50Customer` | `Top50CustomerDao[] GetTop50Customer(string companyId, string fromDate, string toDate)` | `SalesDashboardDal.GetTop50Customer` → `sp_Dashboard_Top50Customer` |
| `GetTopPriorityProductsales` | `TopPriorityProductSalesDao[] GetTopPriorityProductsales(string companyId, string depotId, string fromDate, string toDate)` | `tblInvoiceDetail`/`tblProduct` |
| `GetCompanyWiseExpireProduct` | `ExpireProductDao[] GetCompanyWiseExpireProduct(string companyId, string expireIn)` | `sp_GetExpireproductList`/`sp_GetExpireproductListNational` |
| `GetCompanyWiseProductSales` | `CompanyWiseProductSalesDao[] GetCompanyWiseProductSales(string companyId, string fromDate, string toDate)` | `tblInvoiceDetail` |
| `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)` | `tblInvoice`/`tblGroupInformation` |
| `GetRegionInfo` | `RegionDao[] GetRegionInfo()` | `tblRegion` |
| `GetDepotInfo` | `DepotDao[] GetDepotInfo(string companyId)` | `tblCompanyUnit`/`tblWearhouse` |
| `GetWingsSalesTrend` | `WingsSalesTrend[] GetWingsSalesTrend(string year)` | `sp_GET_WingsSalesReport*` family |
| `GetDashBoardCardInfo` | `SalesDashboardDao[] GetDashBoardCardInfo(string companyId, string fromDate, string toDate)` | `sp_GET_DashboardCardInfo` (note: 3-arg overload vs `CashFlow.aspx`'s 1-arg) |
| `GetFinancialYear` | `FinancialYearDao[] GetFinancialYear(string companyId)` | `sp_GET_FinancialYear` |
| `GetMIOInfo` | `MIODao[] GetMIOInfo(string companyId)` | `tblMIOInfo` |
| `GetCompanyInfo` | `CompanyDao[] GetCompanyInfo()` | `sp_GET_CompanyInfo` |
| `GetAglingReport` | `AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string month)` | `sp_DSB_AglingReport` |
| `GetBusinessSummeryReport` | `BusinessSummeryReportDao[] GetBusinessSummeryReport(string companyId, string timePeriod, string year, string month)` | `SalesDashboardDal.GetBusinesSummeryReport` → `sp_DSB_BusinessSummery` |

### `/SInventory_UI/IncomeStatement.aspx` (12 methods)

| Method | Signature | Calling chain (INFERRED) |
|---|---|---|
| `GetIncomeStatementData` | `IncomeSatementDao[] GetIncomeStatementData(string companyId, string finyearId, string startdate, string enddate)` | `SalesDashboardDal.GetIncomeStatementDetail`/`.GerIncomeStatementBalance` → `sp_GET_IncomeStatementMapDetailById`/`sp_GET_IncomeStatementBalance` |
| `GetCompanyWiseSales` | `CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)` | `tblInvoice` |
| `GetFinancialYearDate` | `FinancialYearDao[] GetFinancialYearDate(string companyId, string finyearId)` | `sp_GET_FinancialYearDate` |
| `GetCashflowData` | `CashFlowDao[] GetCashflowData(string companyId, string finyearId, string startdate, string enddate)` | `sp_GET_CashFlowSummaryBDTCurrency` |
| `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)` | `tblInvoice`/`tblCompanyUnit` |
| `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)` | `tblInvoice`/`tblGroupInformation` |
| `GetWingsSalesTrend` | `WingsSalesTrend[] GetWingsSalesTrend(string year)` | `sp_GET_WingsSalesReport*` family |
| `GetDashBoardCardInfo` | `SalesDashboardDao[] GetDashBoardCardInfo(string companyId)` | `sp_GET_DashboardCardInfo` |
| `GetFinancialYear` | `FinancialYearDao[] GetFinancialYear(string companyId)` | `sp_GET_FinancialYear` |
| `GetCompanyInfo` | `CompanyDao[] GetCompanyInfo()` | `sp_GET_CompanyInfo` |
| `GetAglingReport` | `AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string month)` | `sp_DSB_AglingReport` |

This page and `CashFlow.aspx`/`Dashboard.aspx` share the majority of their `[WebMethod]` names verbatim — the
same dashboard data-access surface is duplicated across three pages rather than factored into one shared
service (consistent with `GAP-STR-005`, business logic sitting in the UI layer, and the wider pattern of
near-duplicate pages noted in `GAP-STR-003`).

### `/SInventory_UI/TradeMaterialTransfer.aspx` (1 method)

| Method | Signature | Calling chain (INFERRED) |
|---|---|---|
| `GetRawMaterial` | `string[] GetRawMaterial(string prefixText)` | `TradeMaterialTransferDal` → `tblProduct`/`CSTL_ERPDB..tblItemDescription` raw-material lookup |

## (c) `.ashx` HTTP handlers — 2

| Handler | HTTP Method | Behaviour | Authentication | Known issues |
|---|---|---|---|---|
| `/PictureHandler.ashx` | GET | Writes `Session["ImageBytes"]` to the response as `image/JPEG` | Whatever session state exists — no explicit login check in the handler itself | Depends entirely on `Session["ImageBytes"]` having been set by a prior page; no content-type/size validation visible |
| `/SignatureHandler.ashx` | GET | Guards on `Session["ImageBytes"] != null` but then casts and writes `Session["SigImageBytes"]` | same as above | **`GAP-APP-001` (CONFIRMED bug):** the null-check and the read target different session keys. If only `SigImageBytes` is set, the guard is false and the handler returns an empty body silently. If only `ImageBytes` is set, the guard passes and the subsequent cast of a `null` `SigImageBytes` throws. The handler only works by accident, when both happen to be set together. |

`/CrystalImageHandler.aspx` is also registered in `Web.config` to serve images for the Crystal Reports viewer,
but it is a `.aspx` handler (not `.ashx`) and is not counted in the "2 handlers" figure from
[`../specs/FINAL-REPORT.md`](../specs/FINAL-REPORT.md) — noted here for completeness per
[`../specs/00-system/api-inventory.md`](../specs/00-system/api-inventory.md) §3.

## What does not exist

- No REST/JSON API for business entities (customers, orders, invoices, etc.) beyond the autocomplete lookups
  above.
- No authentication token, API key, or CORS configuration anywhere in this surface.
- No API versioning, no rate limiting, no request/response schema validation.
- No Swagger/OpenAPI description of any kind.

## Cross-references

- Full endpoint enumeration: [`../specs/00-system/api-inventory.md`](../specs/00-system/api-inventory.md)
- Unauthenticated ASMX finding: [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) → `GAP-SEC-004`
- Duplicate `.asmx` endpoint finding: [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) → `GAP-APP-014`
- `SignatureHandler.ashx` bug: [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) → `GAP-APP-001`
- No page-level auth guard for 127/383 pages generally: [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) →
  `GAP-SEC-002`
- Stored procedures reached indirectly through these BLL/DAL chains:
  [`database/stored-procedures.md`](database/stored-procedures.md)
- Tables reached: [`database/tables.md`](database/tables.md)
