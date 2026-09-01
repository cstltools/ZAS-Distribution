# RPT-118 — FieldForceSalesSummery

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RPT-118` |
| Module | `reports` |
| File name | `FieldForceSalesSummery.aspx` |
| Route | `/SInventory_UI/FieldForceSalesSummery.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `FieldForceSalesSummery.aspx.cs` |
| Code-behind class | `SInventory_UI_FieldForceSalesSummery` |
| Markup lines | 357 |
| Code-behind lines | 268 |
| Server controls | 17 |
| GridViews | 0 |
| Code-behind methods | 12 |

## 2. Purpose

Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

Outbound `Response.Redirect` targets: `Login.aspx`

## 5. Route

`/SInventory_UI/FieldForceSalesSummery.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/FieldForceSalesSummery.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/FieldForceSalesSummery.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |

## 7. UI Control Inventory

17 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 7 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 10 |
| 4 | `groupDropDownList` | asp:DropDownList | Group Name: |  | True | Enabled=False |  |  | 52 |
| 5 | `ddlCompany` | asp:DropDownList | Company Name: |  | True |  |  | ddlCompany_SelectedIndexChanged | 65 |
| 6 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  |  |  |  |  | 78 |
| 7 | `regionDropDownList` | asp:DropDownList | Region Name |  | True |  |  | regionDropDownList_OnTextChanged | 92 |
| 8 | `areaDropDownList` | asp:DropDownList | Area Name |  | true |  |  | areaDropDownList_OnSelectedIndexChanged | 107 |
| 9 | `territoryDropDownList` | asp:DropDownList | Territory Name |  |  |  |  |  | 122 |
| 10 | `fromDateTextBox` | asp:TextBox | From Date: |  |  |  |  |  | 135 |
| 11 | `imgDate` | asp:ImageButton | From Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 136 |
| 12 | `Date` | asp:CalendarExtender |  |  |  |  |  |  | 138 |
| 13 | `toDateTextBox` | asp:TextBox | To Date: |  |  |  |  |  | 151 |
| 14 | `ImageButton1` | asp:ImageButton | To Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 152 |
| 15 | `CalendarExtender1` | asp:CalendarExtender |  |  |  |  |  |  | 154 |
| 16 | `submitButton` | asp:Button |  | View Report |  |  |  | submitButton_Click | 184 |
| 17 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 185 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `regionDropDownList_OnTextChanged` | `aReportDal.GetAreaOnDropdownList(...)` | `CustomerMasterDAL.GetAreaOnDropdownList(DropDownList ddl, string regionId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | `CustomerMasterDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `FieldForceSummeryDal.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `TerritoryInfoDal.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `areaDropDownList_OnSelectedIndexChanged` | `aReportDal.GetTerritoryOnDropdownList(...)` | `CustomerMasterDAL.GetTerritoryOnDropdownList(DropDownList ddl, string areaId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | `CustomerMasterDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `FieldForceSummeryDal.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `MarketInfoDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblTerritory` | — |
| `LoadDropdownList` | `aReportDal.LoadCompanyInfo(...)` | `ASMWiseProductSalesDal.LoadCompanyInfo(DropDownList ddl, string groupId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BonusCampaignDal.LoadCompanyInfo` → `CompanyInfoDAL.LoadCompanyInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerDueReportDAL.LoadCompanyInfo` | SELECT | `tblCompanyInfo` | — |
| `LoadDropdownList` | `aReportDal.LoadGroupInfo(...)` | `ASMWiseProductSalesDal.LoadGroupInfo(DropDownList groupDropDownList)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerFixedDiscountDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblGroupInformation` | — |
| `LoadDropdownList` | `aReportDal.LoadRegionName(...)` | `AreaDAL.LoadRegionName(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/AreaDAL.cs` | `AreaDAL.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionname` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerReportDal.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblRegion` | — |
| `ddlCompany_SelectedIndexChanged` | `aReportDal.LoadDcInfo(...)` | `ASMWiseProductSalesDal.LoadDcInfo(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerPaymentReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |
| `ddlCompany_SelectedIndexChanged` | `aReportDal.LoadDcInfoByUser(...)` | `DestroyProductDal.LoadDcInfoByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.DAL/SInventory_DAL/DestroyProductDal.cs` | `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `SalesSummeryReportForAcc.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `WHStockValuationReprtDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `groupDropDownList` | Group Name: | asp:DropDownList | True | — | `aReportDal.LoadCompanyInfo()` in `LoadDropdownList()`; `aReportDal.LoadGroupInfo()` in `LoadDropdownList()` | `tblCompanyInfo`, `tblGroupInformation` | set in code (see the DAL method above) |
| `ddlCompany` | Company Name: | asp:DropDownList | True | ddlCompany_SelectedIndexChanged | `aReportDal.LoadCompanyInfo()` in `LoadDropdownList()`; `aReportDal.LoadDcInfo()` in `ddlCompany_SelectedIndexChanged()`; `aReportDal.LoadDcInfoByUser()` in `ddlCompany_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center: | asp:DropDownList | False | — | `aReportDal.LoadDcInfo()` in `ddlCompany_SelectedIndexChanged()`; `aReportDal.LoadDcInfoByUser()` in `ddlCompany_SelectedIndexChanged()` | `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `regionDropDownList` | Region Name | asp:DropDownList | True | — | `aReportDal.GetAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `aReportDal.LoadRegionName()` in `LoadDropdownList()` | `tblArea`, `tblRegion` | set in code (see the DAL method above) |
| `areaDropDownList` | Area Name | asp:DropDownList | true | areaDropDownList_OnSelectedIndexChanged | `aReportDal.GetAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `aReportDal.GetTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblArea`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory Name | asp:DropDownList | False | — | `aReportDal.GetTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblTerritory` | set in code (see the DAL method above) |

**Generic dropdown loading contract** (applies to every dropdown in the application):

```
DropDownList
  -> code-behind Page_Load / *_SelectedIndexChanged
     -> <X>BLL.Load<Y>(ddl)                 [Library.BLL/SInventory_BLL]
        -> <X>DAL.Load<Y>(ddl)              [Library.DAL/SInventory_DAL]
           -> ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase(ddl, DisplayField, ValueField, sql)
              -> EXEC ExecuteAllSqlQueryByStoreProcedure @Query = <the inline SELECT>
                 -> SQL Server SalesrollDB_ZAS
```

`ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` inserts a leading item `"--------Select---------"` with an empty value and selects index 0. `DB_Manager.LoadDropDownListData` inserts `"Select--------------------"`, and `ClsCommonInternalDAL.LoadDropDownListData` inserts `"Select from list"`. Which text appears depends on which loader the DAL method used.

Cascading behaviour: a dropdown with `AutoPostBack="True"` and an `OnSelectedIndexChanged` handler is a parent in a dependent-dropdown chain; the handler re-loads the child list with the parent key as a filter. Active/inactive filtering is applied inside the inline SQL (commonly `IsActive='True'` and/or `ActionStatus='Accepted'`).

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `imgDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton1` | asp:ImageButton |  | — | `—` | — | — | — |
| `submitButton` | asp:Button | View Report | — | `submitButton_Click` | 6 lines | — | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | — | — | — | — |
| 2 | `regionDropDownList_OnTextChanged` | `void regionDropDownList_OnTextChanged(object sender, EventArgs e)` | 11 | `aReportDal.GetAreaOnDropdownList` | — | — | — |
| 3 | `areaDropDownList_OnSelectedIndexChanged` | `void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 10 | `aReportDal.GetTerritoryOnDropdownList` | — | — | — |
| 4 | `LoadDropdownList` | `void LoadDropdownList()` | 32 | `aReportDal.LoadCompanyInfo`, `aReportDal.LoadGroupInfo`, `aReportDal.LoadRegionName` | — | — | `UserType` |
| 5 | `ddlCompany_SelectedIndexChanged` | `void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)` | 31 | `aReportDal.LoadDcInfo`, `aReportDal.LoadDcInfoByUser` | — | — | `UserId`, `UserType` |
| 6 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 6 | — | — | — | — |
| 7 | `Validation` | `bool Validation()` | 40 | — | — | — | `IsHead`, `UserType` |
| 8 | `PopUpCustomerPaymentReport` | `void PopUpCustomerPaymentReport()` | 20 | — | — | — | `FDate`, `PRAM`, `TDate`, `pram` |
| 9 | `GenerateParameter` | `string GenerateParameter()` | 42 | — | — | — | — |
| 10 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 11 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 12 | `Clear` | `void Clear()` | 8 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
FieldForceSalesSummery.aspx (markup)
  -> SInventory_UI_FieldForceSalesSummery (code-behind event handler)
     -> Library.BLL.SInventory_BLL.*BLL          (thin pass-through + calculations)
        -> Library.DAL.SInventory_DAL.*DAL       (builds SQL string / SqlParameter list)
           -> Library.DAL.InternalCls.ClsCommonInternalDAL
              -> EXEC ExecuteAllSqlQueryByStoreProcedure @Query   (for inline SQL)
              -> EXEC <named stored procedure>                    (for parameterised calls)
                 -> SQL Server: SalesrollDB_ZAS
```

## 13. API Mapping

**None.** This page exposes and consumes no HTTP API; all interaction is full or partial (UpdatePanel) postback. See `specs/00-system/api-inventory.md` for the complete, application-wide API surface.

## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropdownList` | `CompanyInfoBLL` | `LoadCompanyInfo` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| `LoadDropdownList` | `CustomerMasterBLL` | `LoadRegionname` | `DropDownList ddl,string id` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `LoadDropdownList` | `MarketInfoBLL` | `LoadRegionName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDcInfo` | `DropDownList ddl, string comId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `regionDropDownList_OnTextChanged` | `CustomerMasterDAL` | `GetAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `regionDropDownList_OnTextChanged` | `FieldForceSummeryDal` | `GetAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `regionDropDownList_OnTextChanged` | `MarketInfoDAL` | `GetAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| `regionDropDownList_OnTextChanged` | `TerritoryInfoDal` | `GetAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `CustomerMasterDAL` | `GetTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `CustPaymentDAL` | `GetTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `FieldForceSummeryDal` | `GetTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `MarketInfoDAL` | `GetTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| `LoadDropdownList` | `ASMWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `BatchWiseTransitReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `LoadDropdownList` | `BonusCampaignDal` | `LoadCompanyInfo` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| `LoadDropdownList` | `CompanyInfoDAL` | `LoadCompanyInfo` | `` | `DataTable` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| `LoadDropdownList` | `CustomerDueReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `LoadDropdownList` | `CustomerFixedDiscountDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `LoadDropdownList` | `CustomerPaymentReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `LoadDropdownList` | `CustomerReturnBYMioDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `LoadDropdownList` | `CustomerWiseLedgerDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `LoadDropdownList` | `CustomerWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `LoadDropdownList` | `CustomerWiseReturnDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `LoadDropdownList` | `CustomerWiseSalesReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `LoadDropdownList` | `DCStockInfoReprotDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `LoadDropdownList` | `DestroyProductDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `LoadDropdownList` | `FieldForceSummeryDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropdownList` | `FixedDiscountEntryDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `LoadDropdownList` | `MiaTargetDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| `LoadDropdownList` | `MIOSalesSummeryReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `LoadDropdownList` | `MioWiseCustomerDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `LoadDropdownList` | `MioWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `LoadDropdownList` | `MIOWiseProformaReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `LoadDropdownList` | `RSMWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `SalesSummeryReportForAcc` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `LoadDropdownList` | `WHStockValuationReprtDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `LoadDropdownList` | `WingsWiseSalesReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `LoadDropdownList` | `ASMWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `BatchWiseTransitReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `LoadDropdownList` | `CustomerDueReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `LoadDropdownList` | `CustomerFixedDiscountDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `LoadDropdownList` | `CustomerPaymentReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `LoadDropdownList` | `CustomerReturnBYMioDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `LoadDropdownList` | `CustomerWiseLedgerDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `LoadDropdownList` | `CustomerWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `LoadDropdownList` | `CustomerWiseReturnDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `LoadDropdownList` | `CustomerWiseSalesReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `LoadDropdownList` | `DCStockInfoReprotDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `LoadDropdownList` | `DestroyProductDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `LoadDropdownList` | `FieldForceSummeryDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropdownList` | `FixedDiscountEntryDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `LoadDropdownList` | `MiaTargetDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| `LoadDropdownList` | `MIOSalesSummeryReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `LoadDropdownList` | `MioWiseCustomerDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `LoadDropdownList` | `MioWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `LoadDropdownList` | `MIOWiseProformaReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `LoadDropdownList` | `RSMWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `SalesSummeryReportForAcc` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `LoadDropdownList` | `WHStockValuationReprtDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `LoadDropdownList` | `WingsWiseSalesReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `LoadDropdownList` | `AreaDAL` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| `LoadDropdownList` | `CustomerMasterDAL` | `LoadRegionname` | `DropDownList ddl, string id` | `void` | — | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `LoadDropdownList` | `CustomerMasterDAL` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `LoadDropdownList` | `CustomerReportDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| `LoadDropdownList` | `DistributionCenterInfoDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| `LoadDropdownList` | `FieldForceSummeryDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropdownList` | `MarketInfoDAL` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| `LoadDropdownList` | `TerritoryInfoDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `ASMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `BatchWiseTransitReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustomerDueReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustomerPaymentReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustomerReturnBYMioDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustomerWiseLedgerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustomerWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustomerWiseReturnDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustomerWiseSalesReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `ddlCompany_SelectedIndexChanged` | `CustPaymentDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `ddlCompany_SelectedIndexChanged` | `DCStockInfoReprotDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `DestroyProductDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `FieldForceSummeryDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `MIOSalesSummeryReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `MioWiseCustomerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `MioWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `MIOWiseProformaReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `ddlCompany_SelectedIndexChanged` | `RSMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `SalesSummeryReportForAcc` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `ddlCompany_SelectedIndexChanged` | `WHStockValuationReprtDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `WingsWiseSalesReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `DestroyProductDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `FieldForceSummeryDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `SalesSummeryReportForAcc` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `ddlCompany_SelectedIndexChanged` | `WHStockValuationReprtDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `ddlCompany_SelectedIndexChanged` | `WingsWiseSalesReportDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `regionDropDownList_OnTextChanged()` → `CustomerMasterDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `FieldForceSummeryDal.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyInfo` | `LoadDropdownList()` → `ASMWiseProductSalesDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblCompanyUnit` | `ddlCompany_SelectedIndexChanged()` → `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue`, `ddlCompany_SelectedIndexChanged()` → `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblGroupInformation` | `LoadDropdownList()` → `ASMWiseProductSalesDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblRegion` | `LoadDropdownList()` → `AreaDAL.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionname` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblTerritory` | `areaDropDownList_OnSelectedIndexChanged()` → `CustomerMasterDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblUserCompanyUnit` | `ddlCompany_SelectedIndexChanged()` → `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-RPT-118-001` | {0} | Code | `Solution.Web/SInventory_UI/FieldForceSalesSummery.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `FDate`, `IsHead`, `PRAM`, `TDate`, `UserId`, `UserType`, `pram`

Authentication check: performed by the master page (`MainMasterPage.master.cs` `Page_Load` redirects to `../Login.aspx` when `Session["UserId"]` is null). This page uses that master page, so the check applies.

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
| Master page | `MasterPages/MainMasterPage.master` (auth guard + menu) |
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblGroupInformation` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Database object | `tblUserCompanyUnit` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RPT-118-001` | Unauthenticated access | Request `/SInventory_UI/FieldForceSalesSummery.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-RPT-118-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/FieldForceSalesSummery.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/FieldForceSalesSummery.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Layer source | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 17 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 12 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 7 names found | DDL **not in repo** | **PARTIAL** | section 17 |
| Business rules | absent | messages extracted | rules inside SPs unknown | **PARTIAL** | section 19 |
| Permissions | absent | page-level grant only | grant rows **not in repo** | **UNKNOWN** | section 20 |
| Status lifecycle | absent | tokens extracted | transitions inside SPs unknown | **PARTIAL** | section 21 |

_"Prior spec" is `absent` for every row: the repository contained no `specs/`, `docs/`, `.claude/` or `database/` directory before this rebuild — verified at the repository root._

## 30. Known Gaps

- Stored-procedure bodies are not in the repository → business rules, transactions and result sets executed server-side are UNKNOWN (`GAP-DB-001`).
- No DDL → column names, types, keys, indexes, FKs, triggers UNKNOWN (`GAP-DB-002`).
- Menu path and permission grants live in `tblMainMenu`/`tblMenuDistribution` data → navigation and role matrix UNKNOWN (`GAP-SEC-001`).

## 31. Change Impact

Changing this page affects:

- `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` is **shared with 53 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` is **shared with 10 other page(s)** (e.g. `CustomerMasterViewer.aspx`, `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `CustMasterView.aspx`, `CustomerMasterEdit.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` is **shared with 4 other page(s)** (e.g. `CustomerReport.aspx`, `MarketInfoEdit.aspx`, `MarketInfoEntry.aspx`, `MarketInfoView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `ASMWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/AreaDAL.cs` is **shared with 3 other page(s)** (e.g. `CustomerReport.aspx`, `DcWiseAreaAlocation.aspx`, `WeaightedAVG_Report.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` is **shared with 50 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` is **shared with 49 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` is **shared with 52 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` is **shared with 51 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` is **shared with 58 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` is **shared with 3 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `CustomerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerReportDal.cs` is **shared with 2 other page(s)** (e.g. `CustomerReportViewer.aspx`, `CustomerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` is **shared with 55 other page(s)** (e.g. `CustomerWiseLedgerReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` is **shared with 54 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `CustomerWiseSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockInfoReportViewer.aspx`, `SalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DestroyProductDal.cs` is **shared with 55 other page(s)** (e.g. `DestroyProductReportViewer.aspx`, `DestroyProductStatementViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` is **shared with 7 other page(s)** (e.g. `AuditReportOne.aspx`, `CustomerCreditView.aspx`, `CustomerReport.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` is **shared with 59 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `FieldForceSalesSummeryReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` is **shared with 59 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `MIOSalesSummeryReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` is **shared with 1 other page(s)** (e.g. `CustomerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` is **shared with 49 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` is **shared with 51 other page(s)** (e.g. `MioWiseCustomerReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `RSMWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` is **shared with 51 other page(s)** (e.g. `SalesSummeryForAccHtmlReport.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` is **shared with 2 other page(s)** (e.g. `CustomerReport.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockValuationHtmlReport.aspx`, `WHStockValuationHtmlReport.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` is **shared with 57 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`, `WingsWiseCollectionReportViewer.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblGroupInformation` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
