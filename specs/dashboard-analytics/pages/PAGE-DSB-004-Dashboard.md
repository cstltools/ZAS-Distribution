# DSB-004 — Dashboard

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `DSB-004` |
| Module | `dashboard-analytics` |
| File name | `Dashboard.aspx` |
| Route | `/SInventory_UI/Dashboard.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `Dashboard.aspx.cs` |
| Code-behind class | `SInventory_UI_Dashboard` |
| Markup lines | 4140 |
| Code-behind lines | 726 |
| Server controls | 34 |
| GridViews | 0 |
| Code-behind methods | 19 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `Dashboard/Dashboard.aspx`
- referenced from `SInventory_UI/DashboardPanel.aspx`

## 5. Route

`/SInventory_UI/Dashboard.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/Dashboard.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/Dashboard.aspx.cs` |
| Client script | `../Assets/js/vendors/jquery/jquery.min.js` |
| Client script | `../CssFilesAll/bower_components/jquery-ui/js/jquery-ui.min.js` |
| Client script | `../assets/js/horizontal-menu.js` |
| Client script | `../assets/js/pages/ac-datepicker.js` |
| Client script | `../assets/js/pcoded.min.js` |
| Client script | `../assets/js/plugins/bootstrap.min.js` |
| Client script | `../assets/js/plugins/daterangepicker.js` |
| Client script | `../assets/js/plugins/moment.min.js` |
| Client script | `../assets/js/vendor-all.min.js` |
| Client script | `https://code.highcharts.com/highcharts-more.js` |
| Client script | `https://code.highcharts.com/highcharts.js` |
| Client script | `https://code.highcharts.com/modules/accessibility.js` |
| Client script | `https://code.highcharts.com/modules/export-data.js` |
| Client script | `https://code.highcharts.com/modules/exporting.js` |
| Client script | `https://code.highcharts.com/modules/funnel.js` |
| Client script | `https://code.highcharts.com/stock/highstock.js` |
| Client script | `~/assets/js/pcoded.min.js` |
| Client script | `~/assets/js/plugins/bootstrap.min.js` |
| Client script | `~/assets/js/vendor-all.min.js` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |

## 7. UI Control Inventory

34 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ddlCardCompany` | select | Wings name |  |  |  |  |  | 449 |
| 2 | `txtCardFromDate` | input | From Date: |  |  |  |  |  | 459 |
| 3 | `txtCardToDate` | input | To Date: |  |  |  |  |  | 469 |
| 4 | `ddlSalesTrendFinancialYear` | select | Financial Year |  |  |  |  |  | 667 |
| 5 | `ddlInvCompany` | select | Wings name |  |  |  |  |  | 754 |
| 6 | `txtInvFromDate` | input | From Date |  |  |  |  |  | 765 |
| 7 | `txtInvToDate` | input | To Date |  |  |  |  |  | 775 |
| 8 | `ddlMioInvCompany` | select | Wings name |  |  |  |  |  | 851 |
| 9 | `ddlMio` | select | MIO name |  |  |  |  |  | 860 |
| 10 | `txtMioInvFromDate` | input | From Date |  |  |  |  |  | 870 |
| 11 | `txtMioInvToDate` | input | To Date |  |  |  |  |  | 880 |
| 12 | `ddlExpiredProductCompany` | select | Wings name |  |  |  |  |  | 957 |
| 13 | `ddlExpireInMonth` | select | Expired In (Month) |  |  |  |  |  | 966 |
| 14 | `ddlTopCustomerCompany` | select | Wings name |  |  |  |  |  | 1050 |
| 15 | `txtTopFromDate` | input | From Date |  |  |  |  |  | 1060 |
| 16 | `txtTopToDate` | input | To Date |  |  |  |  |  | 1070 |
| 17 | `ddlTopPriorityCompany` | select | Wings |  |  |  |  |  | 1143 |
| 18 | `ddlDepot` | select | Depot |  |  |  |  |  | 1152 |
| 19 | `txtTopPriorityFromDate` | input | From Date |  |  |  |  |  | 1163 |
| 20 | `txtTopPriorityToDate` | input | To Date |  |  |  |  |  | 1173 |
| 21 | `ddlSalesCompany` | select | Wings name |  |  |  |  |  | 1245 |
| 22 | `ddlSalesFinancialYear` | select | Financial Year |  |  |  |  |  | 1254 |
| 23 | `ddlSalesMonth` | select | Month name |  |  |  |  |  | 1271 |
| 24 | `ddlProductSalesCompany` | select | Wings name |  |  |  |  |  | 1370 |
| 25 | `txtFromDate` | input | From Date |  |  |  |  |  | 1380 |
| 26 | `txtToDate` | input | To Date |  |  |  |  |  | 1390 |
| 27 | `ddlAglingCompany` | select | Wings |  |  |  |  |  | 1466 |
| 28 | `ageing` | input |  |  |  |  |  |  | 1476 |
| 29 | `ageing` | input | Daily |  |  |  |  |  | 1480 |
| 30 | `ddlAglingFinancialYear` | select | Year |  |  |  |  |  | 1491 |
| 31 | `ddlAglingMonth` | select | Month |  |  |  |  |  | 1510 |
| 32 | `ddlBusinessSummeryCompany` | select | Wings |  |  |  |  |  | 1590 |
| 33 | `ddlBusinessSummeryFinancialYear` | select | Year |  |  |  |  |  | 1615 |
| 34 | `ddlBusinessSummeryMonth` | select | Month |  |  |  |  |  | 1634 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `AglingReportDao`, `BusinessSummeryReportDao`, `CompanyDao`, `CompanyWiseInvoiceDao`, `CompanyWiseProductSalesDao`, `CompanyWiseSalesDao`, `CompanyWiseUnitSalesDao`, `DepotDao`, `ExpireProductDao`, `FinancialYearDao`, `GroupSalesCompanyWiseDao`, `MIODao`, `MioWiseInvoiceDao`, `RegionDao`, `SalesDashboardDao`, `Top50CustomerDao`, `TopPriorityProductSalesDao`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `GetCompanyWiseSales` | `aDashboardDal.GetCompanyWiseSales(...)` | `SalesDashboardDal.GetCompanyWiseSales(string companyId, string year, string month)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetCompanyWiseSales` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceDetail` | — |
| `GetCompanyWiseSales` | `aDashboardDal.GetDueUpto(...)` | `SalesDashboardDal.GetDueUpto(DateTime toDateTime, string companyId)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetDueUpto` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CustomerMaster`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | — |
| `GetCompanyWiseDcSales` | `aDashboardDal.GetCompanyWiseDcSales(...)` | `SalesDashboardDal.GetCompanyWiseDcSales(string companyId, DateTime fromDate, DateTime toDate)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetCompanyWiseDcSales` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — |
| `GetCompanyWiseInvoice` | `aDashboardDal.GetCompanyInvoice(...)` | `SalesDashboardDal.GetCompanyInvoice(string companyId, DateTime fromDate, DateTime toDate)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetCompanyInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblInvoice` | — |
| `GetMioWiseInvoice` | `aDashboardDal.GetMioWiseInvoice(...)` | `SalesDashboardDal.GetMioWiseInvoice(string companyId, DateTime fromDate, DateTime toDate, string mioId)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetMioWiseInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblInvoice`, `tblMIOInfo` | — |
| `GetTop50Customer` | `aDashboardDal.GetTop50Customer(...)` | `SalesDashboardDal.GetTop50Customer(string companyId, DateTime fromDate, DateTime toDate)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceDetail`, `tblMarket`, `tblTerritory` | `sp_Dashboard_Top50Customer` |
| `GetTopPriorityProductsales` | `aDashboardDal.GetTopPriorityProductsales(...)` | `SalesDashboardDal.GetTopPriorityProductsales(string companyId, string depotId, DateTime fromDate, DateTime toDate)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIATargetProductWise`, `tblMIOInfo`, `tblOrder`, `tblProduct`, `tblRSMInfo`, `tblRegion`, `tblTerritory` | — |
| `GetCompanyWiseExpireProduct` | `aDashboardDal.GetCompanyWiseExpireProduct(...)` | `SalesDashboardDal.GetCompanyWiseExpireProduct(string companyId, Int32 expireIn)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetCompanyWiseExpireProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblDCStore` | — |
| `GetCompanyWiseProductSales` | `aDashboardDal.GetCompanyWiseProductSales(...)` | `SalesDashboardDal.GetCompanyWiseProductSales(string companyId, string prameter)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetCompanyWiseProductSales` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — |
| `GetGroupSalesCompanyWise` | `aDashboardDal.GetGroupSalesCompanyWise(...)` | `SalesDashboardDal.GetGroupSalesCompanyWise(DateTime fromDate, DateTime toDate)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetGroupSalesCompanyWise` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — |
| `GetRegionInfo` | `aDashboardDal.GetRegion(...)` | `CustomerMasterInfoDal.GetRegion(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SalesDashboardDal.GetRegion` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.GetRegion` | SELECT | `tblRegion` | — |
| `GetDepotInfo` | `aDashboardDal.GetDepot(...)` | `SalesDashboardDal.GetDepot(string companyId)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetDepot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit` | — |
| `GetWingsSalesTrend` | `aDashboardDal.GetWingsSalesTrend(...)` | `SalesDashboardDal.GetWingsSalesTrend(string year)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetWingsSalesTrend` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — |
| `GetDashBoardCardInfo` | `aDashboardDal.GetDashBoardCardInfo(...)` | `SalesDashboardDal.GetDashBoardCardInfo(string companyId,string fromDate, string toDate,DateTime currentDate)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetDashBoardCardInfo` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | — | — | `sp_GET_DashboardCardInfo` |
| `GetDashBoardCardInfo` | `aDashboardDal.GetDashBoardStockCardInfo(...)` | `SalesDashboardDal.GetDashBoardStockCardInfo(string companyId, string fromDate, string toDate, DateTime today)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetDashBoardStockCardInfo` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | — | — | `sp_NationalBinCardPrice` |
| `GetFinancialYear` | `aDashboardDal.GetFinancialYear(...)` | `SalesDashboardDal.GetFinancialYear(string companyId)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetFinancialYear` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | — | — | `sp_GET_FinancialYear` |
| `GetMIOInfo` | `aDashboardDal.GetMIOInfo(...)` | `SalesDashboardDal.GetMIOInfo(string companyId)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetMIOInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblEmpGeneralInfo`, `tblMIOInfo` | — |
| `GetCompanyInfo` | `aDashboardDal.GetCompanyInfo(...)` | `CompanyInfoDAL.GetCompanyInfo(string companyName)` <br/>`Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` | `CompanyInfoDAL.GetCompanyInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `SalesDashboardDal.GetCompanyInfo` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | SELECT | `tblCompanyInfo` | `sp_GET_CompanyInfo` |
| `GetAglingReport` | `aDashboardDal.GetAglingReport(...)` | `SalesDashboardDal.GetAglingReport(string parameter)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetAglingReport` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | — | — | `sp_DSB_AglingReport` |
| `GetBusinessSummeryReport` | `aDashboardDal.GetBusinesSummeryReport(...)` | `SalesDashboardDal.GetBusinesSummeryReport(string parameter1, string parameter2, string parameter3, string parame)` <br/>`Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | `SalesDashboardDal.GetBusinesSummeryReport` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | — | — | `sp_DSB_BusinessSummery` |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `txtCardFromDate` | input |  | — | `—` | — | — | — |
| `txtCardToDate` | input |  | — | `—` | — | — | — |
| `txtInvFromDate` | input |  | — | `—` | — | — | — |
| `txtInvToDate` | input |  | — | `—` | — | — | — |
| `txtMioInvFromDate` | input |  | — | `—` | — | — | — |
| `txtMioInvToDate` | input |  | — | `—` | — | — | — |
| `txtTopFromDate` | input |  | — | `—` | — | — | — |
| `txtTopToDate` | input |  | — | `—` | — | — | — |
| `txtTopPriorityFromDate` | input |  | — | `—` | — | — | — |
| `txtTopPriorityToDate` | input |  | — | `—` | — | — | — |
| `txtFromDate` | input |  | — | `—` | — | — | — |
| `txtToDate` | input |  | — | `—` | — | — | — |
| `ageing` | input |  | — | `—` | — | — | — |
| `ageing` | input |  | — | `—` | — | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 0
- AJAX Control Toolkit extenders: none
- Inline JavaScript functions declared in the markup: `LoadAglingReport()`, `LoadBusinessSummeryReport()`, `LoadCompanyWisePerDayInvoice()`, `LoadCompanyWisePerDayinvDataTable()`, `LoadDcWiseSalesChart()`, `LoadDropDownlist()`, `LoadExpireProduct()`, `LoadFinancialYear()`, `LoadFinancialYearMonth()`, `LoadMIOInfo()`, `LoadMIOWiseInvoiceDataTable()`, `LoadMioWisePerDayInvoice()`, `LoadProductSalesDataTable()`, `LoadProductWiseSalesChart()`, `LoadSalesFunnel()`, `LoadSalesFunnelDataTable()`, `LoadSalesTrendDataTable()`, `LoadTop50Customer()`, `LoadTopPriorityProductSales()`, `LoadWingsSalesStatusDataTable()`, `LoadWingsSalesTrendChart()`, `LoadWingsWiseDepot()`, `LoadWingsWisePerDaySales()`, `SetDashBoardCardInfo()`, `formatDate()`, `numberWithCommas()`
- External scripts: `../Assets/js/vendors/jquery/jquery.min.js`, `../CssFilesAll/bower_components/jquery-ui/js/jquery-ui.min.js`, `../assets/js/horizontal-menu.js`, `../assets/js/pages/ac-datepicker.js`, `../assets/js/pcoded.min.js`, `../assets/js/plugins/bootstrap.min.js`, `../assets/js/plugins/daterangepicker.js`, `../assets/js/plugins/moment.min.js`, `../assets/js/vendor-all.min.js`, `https://code.highcharts.com/highcharts-more.js`, `https://code.highcharts.com/highcharts.js`, `https://code.highcharts.com/modules/accessibility.js`, `https://code.highcharts.com/modules/export-data.js`, `https://code.highcharts.com/modules/exporting.js`, `https://code.highcharts.com/modules/funnel.js`, `https://code.highcharts.com/stock/highstock.js`, `~/assets/js/pcoded.min.js`, `~/assets/js/plugins/bootstrap.min.js`, `~/assets/js/vendor-all.min.js`
- Client-side `alert()` messages: "505 - Request Denied !!"; "Failed"; "You are "

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 3 | — | — | — | — |
| 2 | `GetCompanyWiseSales` | `CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)` | 33 | `aDashboardDal.GetCompanyWiseSales`, `aDashboardDal.GetDueUpto` | — | — | — |
| 3 | `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)` | 29 | `aDashboardDal.GetCompanyWiseDcSales` | — | — | — |
| 4 | `GetCompanyWiseInvoice` | `CompanyWiseInvoiceDao[] GetCompanyWiseInvoice(string companyId, string fromDate, string toDate)` | 30 | `aDashboardDal.GetCompanyInvoice` | — | — | — |
| 5 | `GetMioWiseInvoice` | `MioWiseInvoiceDao[] GetMioWiseInvoice(string companyId, string fromDate, string toDate, string mio)` | 32 | `aDashboardDal.GetMioWiseInvoice` | — | — | — |
| 6 | `GetTop50Customer` | `Top50CustomerDao[] GetTop50Customer(string companyId, string fromDate, string toDate)` | 36 | `aDashboardDal.GetTop50Customer` | — | — | — |
| 7 | `GetTopPriorityProductsales` | `TopPriorityProductSalesDao[] GetTopPriorityProductsales(string companyId, string depotId, string fromDate, string to)` | 36 | `aDashboardDal.GetTopPriorityProductsales` | — | — | — |
| 8 | `GetCompanyWiseExpireProduct` | `ExpireProductDao[] GetCompanyWiseExpireProduct(string companyId, string expireIn)` | 33 | `aDashboardDal.GetCompanyWiseExpireProduct` | — | — | — |
| 9 | `GetCompanyWiseProductSales` | `CompanyWiseProductSalesDao[] GetCompanyWiseProductSales(string companyId, string fromDate, string toDate)` | 43 | `aDashboardDal.GetCompanyWiseProductSales` | — | — | — |
| 10 | `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)` | 28 | `aDashboardDal.GetGroupSalesCompanyWise` | — | — | — |
| 11 | `GetRegionInfo` | `RegionDao[] GetRegionInfo()` | 28 | `aDashboardDal.GetRegion` | — | — | — |
| 12 | `GetDepotInfo` | `DepotDao[] GetDepotInfo(string companyId)` | 28 | `aDashboardDal.GetDepot` | — | — | — |
| 13 | `GetWingsSalesTrend` | `WingsSalesTrend[] GetWingsSalesTrend(string year)` | 29 | `aDashboardDal.GetWingsSalesTrend` | — | — | — |
| 14 | `GetDashBoardCardInfo` | `SalesDashboardDao[] GetDashBoardCardInfo(string companyId, string fromDate, string toDate)` | 44 | `aDashboardDal.GetDashBoardCardInfo`, `aDashboardDal.GetDashBoardStockCardInfo` | — | — | — |
| 15 | `GetFinancialYear` | `FinancialYearDao[] GetFinancialYear(string companyId)` | 28 | `aDashboardDal.GetFinancialYear` | — | — | — |
| 16 | `GetMIOInfo` | `MIODao[] GetMIOInfo(string companyId)` | 26 | `aDashboardDal.GetMIOInfo` | — | — | — |
| 17 | `GetCompanyInfo` | `CompanyDao[] GetCompanyInfo()` | 28 | `aDashboardDal.GetCompanyInfo` | — | — | — |
| 18 | `GetAglingReport` | `AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string mon)` | 60 | `aDashboardDal.GetAglingReport` | — | — | — |
| 19 | `GetBusinessSummeryReport` | `BusinessSummeryReportDao[] GetBusinessSummeryReport(string companyId, string timePeriod, string year, string mon)` | 70 | `aDashboardDal.GetBusinesSummeryReport` | — | — | — |

Call chain shape for every persisting action on this page:

```
Dashboard.aspx (markup)
  -> SInventory_UI_Dashboard (code-behind event handler)
     -> Library.BLL.SInventory_BLL.*BLL          (thin pass-through + calculations)
        -> Library.DAL.SInventory_DAL.*DAL       (builds SQL string / SqlParameter list)
           -> Library.DAL.InternalCls.ClsCommonInternalDAL
              -> EXEC ExecuteAllSqlQueryByStoreProcedure @Query   (for inline SQL)
              -> EXEC <named stored procedure>                    (for parameterised calls)
                 -> SQL Server: SalesrollDB_ZAS
```

## 13. API Mapping

Page-level `[WebMethod]` endpoints (called by page JavaScript via `PageMethods` / `$.ajax`):

| HTTP | Route | Method | Returns |
|---|---|---|---|
| POST | `/SInventory_UI/Dashboard.aspx/GetCompanyWiseSales` | `GetCompanyWiseSales` | `CompanyWiseSalesDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetCompanyWiseDcSales` | `GetCompanyWiseDcSales` | `CompanyWiseUnitSalesDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetCompanyWiseInvoice` | `GetCompanyWiseInvoice` | `CompanyWiseInvoiceDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetMioWiseInvoice` | `GetMioWiseInvoice` | `MioWiseInvoiceDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetTop50Customer` | `GetTop50Customer` | `Top50CustomerDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetTopPriorityProductsales` | `GetTopPriorityProductsales` | `TopPriorityProductSalesDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetCompanyWiseExpireProduct` | `GetCompanyWiseExpireProduct` | `ExpireProductDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetCompanyWiseProductSales` | `GetCompanyWiseProductSales` | `CompanyWiseProductSalesDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetGroupSalesCompanyWise` | `GetGroupSalesCompanyWise` | `GroupSalesCompanyWiseDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetRegionInfo` | `GetRegionInfo` | `RegionDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetDepotInfo` | `GetDepotInfo` | `DepotDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetWingsSalesTrend` | `GetWingsSalesTrend` | `WingsSalesTrend[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetDashBoardCardInfo` | `GetDashBoardCardInfo` | `SalesDashboardDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetFinancialYear` | `GetFinancialYear` | `FinancialYearDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetMIOInfo` | `GetMIOInfo` | `MIODao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetCompanyInfo` | `GetCompanyInfo` | `CompanyDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetAglingReport` | `GetAglingReport` | `AglingReportDao[]` |
| POST | `/SInventory_UI/Dashboard.aspx/GetBusinessSummeryReport` | `GetBusinessSummeryReport` | `BusinessSummeryReportDao[]` |

AJAX URLs referenced in markup: `Dashboard.aspx/GetAglingReport`, `Dashboard.aspx/GetBusinessSummeryReport`, `Dashboard.aspx/GetCompanyInfo`, `Dashboard.aspx/GetCompanyWiseDcSales`, `Dashboard.aspx/GetCompanyWiseExpireProduct`, `Dashboard.aspx/GetCompanyWiseInvoice`, `Dashboard.aspx/GetCompanyWiseProductSales`, `Dashboard.aspx/GetCompanyWiseSales`, `Dashboard.aspx/GetDashBoardCardInfo`, `Dashboard.aspx/GetDepotInfo`, `Dashboard.aspx/GetFinancialYear`, `Dashboard.aspx/GetGroupSalesCompanyWise`, `Dashboard.aspx/GetMIOInfo`, `Dashboard.aspx/GetMioWiseInvoice`, `Dashboard.aspx/GetRegionInfo`, `Dashboard.aspx/GetTop50Customer`, `Dashboard.aspx/GetTopPriorityProductsales`, `Dashboard.aspx/GetWingsSalesTrend`


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `GetRegionInfo` | `CustomerMasterInfoBll` | `GetRegion` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `GetCompanyWiseSales` | `SalesDashboardDal` | `GetCompanyWiseSales` | `string companyId, string year, string month` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceDetail` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetCompanyWiseSales` | `SalesDashboardDal` | `GetDueUpto` | `DateTime toDateTime, string companyId` | `DataTable` | SELECT | `View_CustomerMaster`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetCompanyWiseDcSales` | `SalesDashboardDal` | `GetCompanyWiseDcSales` | `string companyId, DateTime fromDate, DateTime toDate` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetCompanyWiseInvoice` | `SalesDashboardDal` | `GetCompanyInvoice` | `string companyId, DateTime fromDate, DateTime toDate` | `DataTable` | SELECT | `tblCompanyUnit`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetMioWiseInvoice` | `SalesDashboardDal` | `GetMioWiseInvoice` | `string companyId, DateTime fromDate, DateTime toDate, string` | `DataTable` | SELECT | `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblInvoice`, `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetTop50Customer` | `SalesDashboardDal` | `GetTop50Customer` | `string companyId, DateTime fromDate, DateTime toDate` | `DataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceDetail`, `tblMarket`, `tblTerritory` | `sp_Dashboard_Top50Customer` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetTopPriorityProductsales` | `SalesDashboardDal` | `GetTopPriorityProductsales` | `string companyId, string depotId, DateTime fromDate, DateTim` | `DataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIATargetProductWise`, `tblMIOInfo`, `tblOrder`, `tblProduct`, `tblRSMInfo`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetCompanyWiseExpireProduct` | `SalesDashboardDal` | `GetCompanyWiseExpireProduct` | `string companyId, Int32 expireIn` | `DataTable` | SELECT | `tblCompanyUnit`, `tblDCStore` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetCompanyWiseProductSales` | `SalesDashboardDal` | `GetCompanyWiseProductSales` | `string companyId, string prameter` | `DataTable` | SELECT | `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetGroupSalesCompanyWise` | `SalesDashboardDal` | `GetGroupSalesCompanyWise` | `DateTime fromDate, DateTime toDate` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetRegionInfo` | `CustomerMasterInfoDal` | `GetRegion` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `GetRegionInfo` | `SalesDashboardDal` | `GetRegion` | `` | `DataTable` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetDepotInfo` | `SalesDashboardDal` | `GetDepot` | `string companyId` | `DataTable` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetWingsSalesTrend` | `SalesDashboardDal` | `GetWingsSalesTrend` | `string year` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetDashBoardCardInfo` | `SalesDashboardDal` | `GetDashBoardCardInfo` | `string companyId,string fromDate, string toDate,DateTime cur` | `DataTable` | — | — | `sp_GET_DashboardCardInfo` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetDashBoardCardInfo` | `SalesDashboardDal` | `GetDashBoardStockCardInfo` | `string companyId, string fromDate, string toDate, DateTime t` | `DataTable` | — | — | `sp_NationalBinCardPrice` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetFinancialYear` | `SalesDashboardDal` | `GetFinancialYear` | `string companyId` | `DataTable` | — | — | `sp_GET_FinancialYear` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetMIOInfo` | `SalesDashboardDal` | `GetMIOInfo` | `string companyId` | `DataTable` | SELECT | `tblEmpGeneralInfo`, `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetCompanyInfo` | `CompanyInfoDAL` | `GetCompanyInfo` | `string companyName` | `DataTable` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| `GetCompanyInfo` | `SalesDashboardDal` | `GetCompanyInfo` | `` | `DataTable` | — | — | `sp_GET_CompanyInfo` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetAglingReport` | `SalesDashboardDal` | `GetAglingReport` | `string parameter` | `DataTable` | — | — | `sp_DSB_AglingReport` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `GetBusinessSummeryReport` | `SalesDashboardDal` | `GetBusinesSummeryReport` | `string parameter1, string parameter2, string parameter3, str` | `DataTable` | — | — | `sp_DSB_BusinessSummery` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Dashboard_Top50Customer` | GetTop50Customer → `SalesDashboardDal.GetTop50Customer → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_GET_DashboardCardInfo` | GetDashBoardCardInfo → `SalesDashboardDal.GetDashBoardCardInfo → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_NationalBinCardPrice` | GetDashBoardCardInfo → `SalesDashboardDal.GetDashBoardStockCardInfo → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_GET_FinancialYear` | GetFinancialYear → `SalesDashboardDal.GetFinancialYear → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_GET_CompanyInfo` | GetCompanyInfo → `CompanyInfoDAL.GetCompanyInfo → ClsCommonInternalDAL.DataContainerDataTable → SalesDashboardDal.GetCompanyInfo → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_DSB_AglingReport` | GetAglingReport → `SalesDashboardDal.GetAglingReport → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_DSB_BusinessSummery` | GetBusinessSummeryReport → `SalesDashboardDal.GetBusinesSummeryReport → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_CustomerMaster` | `GetCompanyWiseSales()` → `SalesDashboardDal.GetDueUpto` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblArea` | `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction`, `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `GetCompanyInfo()` → `CompanyInfoDAL.GetCompanyInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `SalesDashboardDal.GetCompanyInfo` → `ClsCommonInternalDAL.GetDataTableAction`, `GetCompanyWiseDcSales()` → `SalesDashboardDal.GetCompanyWiseDcSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetCompanyWiseSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetDueUpto` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetGroupSalesCompanyWise()` → `SalesDashboardDal.GetGroupSalesCompanyWise` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` |
| `tblCompanyUnit` | `GetCompanyWiseDcSales()` → `SalesDashboardDal.GetCompanyWiseDcSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseExpireProduct()` → `SalesDashboardDal.GetCompanyWiseExpireProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseInvoice()` → `SalesDashboardDal.GetCompanyInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetCompanyWiseSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetDueUpto` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetDepotInfo()` → `SalesDashboardDal.GetDepot` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` |
| `tblCustPayDetail` | `GetCompanyWiseSales()` → `SalesDashboardDal.GetCompanyWiseSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetDueUpto` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` |
| `tblCustomerPay` | `GetCompanyWiseSales()` → `SalesDashboardDal.GetCompanyWiseSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetDueUpto` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` |
| `tblDCStore` | `GetCompanyWiseExpireProduct()` → `SalesDashboardDal.GetCompanyWiseExpireProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `GetMIOInfo()` → `SalesDashboardDal.GetMIOInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetMioWiseInvoice()` → `SalesDashboardDal.GetMioWiseInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `GetCompanyWiseDcSales()` → `SalesDashboardDal.GetCompanyWiseDcSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseInvoice()` → `SalesDashboardDal.GetCompanyInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseProductSales()` → `SalesDashboardDal.GetCompanyWiseProductSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetCompanyWiseSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetDueUpto` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetGroupSalesCompanyWise()` → `SalesDashboardDal.GetGroupSalesCompanyWise` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `GetCompanyWiseDcSales()` → `SalesDashboardDal.GetCompanyWiseDcSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseProductSales()` → `SalesDashboardDal.GetCompanyWiseProductSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCompanyWiseSales()` → `SalesDashboardDal.GetCompanyWiseSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetGroupSalesCompanyWise()` → `SalesDashboardDal.GetGroupSalesCompanyWise` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction`, `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIATargetProductWise` | `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `GetMIOInfo()` → `SalesDashboardDal.GetMIOInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetMioWiseInvoice()` → `SalesDashboardDal.GetMioWiseInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` |
| `tblOrder` | `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProduct` | `GetCompanyWiseProductSales()` → `SalesDashboardDal.GetCompanyWiseProductSales` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblRSMInfo` | `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblRegion` | `GetRegionInfo()` → `CustomerMasterInfoDal.GetRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SalesDashboardDal.GetRegion` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `GetTop50Customer()` → `SalesDashboardDal.GetTop50Customer` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction`, `GetTopPriorityProductsales()` → `SalesDashboardDal.GetTopPriorityProductsales` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

_No validation logic detected on this page._

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

_No message-bearing rule found in this page's own code._

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Authentication check: performed by the master page (`MainMasterPage.master.cs` `Page_Load` redirects to `../Login.aspx` when `Session["UserId"]` is null). **This page does not use the master page — it therefore performs no session check unless its own code-behind does. See `specs/SPEC-GAPS.md` (`GAP-SEC-002`).**

## 21. Status Lifecycle

No status token appears in this page's own code.

Application-wide status vocabulary and the transitions that are actually implemented are documented in `specs/00-system/status-lifecycle.md`. Transitions executed inside stored procedures are **UNKNOWN**.

## 22. Error Handling

- Page level: no `try`/`catch` found in the code-behind — an exception surfaces as an ASP.NET yellow screen
- DAL level: `ClsCommonInternalDAL` re-throws (`throw ex;`, which resets the stack trace). `DB_Manager.ExecuteNonQueryAction` **swallows** the exception, rolls back the transaction, closes the connection and returns `0`, so the caller sees "no rows affected" rather than the error.
- Application level: `Global.asax` `Application_Error` is **empty** — there is no global error handler, no logging and no custom error page (`<customErrors>` is not configured in `Web.config`).
- User-visible failures are surfaced as `alert()` boxes injected with `ScriptManager.RegisterStartupScript`.

## 23. Audit

No audit column is written by this page (INFERRED — none appears in its code-behind).

Application-wide auditing: only `tblLoginLog (UserId, LoginName, LoginTime)`, written on successful login (`Library.DAL/PanalCls/PanalClsDAL.cs::LoginLog`). There is no logout log, no change log, no row-version history and no application logging framework wired up (log4net is referenced in `Web.config` `<assemblies>` but never configured or called).

## 24. Integration

_No integration touchpoint detected on this page._

See `specs/00-system/integrations.md` for the full list.

## 25. Reports

_This page opens no report._

## 26. Dependencies

| Kind | Dependency |
|---|---|
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| Database object | `View_CustomerMaster` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerPay` |
| Database object | `tblDCStore` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblMIATargetProductWise` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblProduct` |
| Database object | `tblRSMInfo` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Stored procedure | `sp_Dashboard_Top50Customer` |
| Stored procedure | `sp_GET_DashboardCardInfo` |
| Stored procedure | `sp_NationalBinCardPrice` |
| Stored procedure | `sp_GET_FinancialYear` |
| Stored procedure | `sp_GET_CompanyInfo` |
| Stored procedure | `sp_DSB_AglingReport` |
| Stored procedure | `sp_DSB_BusinessSummery` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-DSB-004-001` | Unauthenticated access | Request `/SInventory_UI/Dashboard.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/Dashboard.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/Dashboard.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 34 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 19 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | 18 WebMethods | n/a | **MISSING → now documented** | section 13 |
| Stored procedures | absent | 7 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 19 names found | DDL **not in repo** | **PARTIAL** | section 17 |
| Business rules | absent | messages extracted | rules inside SPs unknown | **PARTIAL** | section 19 |
| Permissions | absent | page-level grant only | grant rows **not in repo** | **UNKNOWN** | section 20 |
| Status lifecycle | absent | tokens extracted | transitions inside SPs unknown | **PARTIAL** | section 21 |

_"Prior spec" is `absent` for every row: the repository contained no `specs/`, `docs/`, `.claude/` or `database/` directory before this rebuild — verified at the repository root._

## 30. Known Gaps

- Stored-procedure bodies are not in the repository → business rules, transactions and result sets executed server-side are UNKNOWN (`GAP-DB-001`).
- No DDL → column names, types, keys, indexes, FKs, triggers UNKNOWN (`GAP-DB-002`).
- Menu path and permission grants live in `tblMainMenu`/`tblMenuDistribution` data → navigation and role matrix UNKNOWN (`GAP-SEC-001`).
- This page does not inherit the master page, so it has **no authentication guard** (`GAP-SEC-002`).

## 31. Change Impact

Changing this page affects:

- `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` is **shared with 2 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustMasterEntry.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` is **shared with 52 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` is **shared with 5 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `FixedDiscountEntry.aspx`, `InvoiceCreationForConsumerCustomer.aspx`, `SupplierInformation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` is **shared with 4 other page(s)** (e.g. `CashFlow.aspx`, `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `IncomeStatement.aspx`) — a change there is not local.
- Writes/reads `View_CustomerMaster` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerPay` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblMIATargetProductWise` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblRSMInfo` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
