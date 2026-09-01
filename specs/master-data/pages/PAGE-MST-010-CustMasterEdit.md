# MST-010 — CustMasterEdit

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-010` |
| Module | `master-data` |
| File name | `CustMasterEdit.aspx` |
| Route | `/SInventory_UI/CustMasterEdit.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `CustMasterEdit.aspx.cs` |
| Code-behind class | `SInventory_UI_CustMasterEdit` |
| Markup lines | 424 |
| Code-behind lines | 303 |
| Server controls | 29 |
| GridViews | 0 |
| Code-behind methods | 12 |

## 2. Purpose

Edit screen. It loads an existing record by its key (usually from Session or query string) and persists the modified values.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Edit screen. It loads an existing record by its key (usually from Session or query string) and persists the modified values.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/CustMasterView.aspx`

Outbound `Response.Redirect` targets: `CustomerMasterView.aspx`

## 5. Route

`/SInventory_UI/CustMasterEdit.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustMasterEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |

## 7. UI Control Inventory

29 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ScriptManager1` | asp:ScriptManager | Edit |  |  |  |  |  | 17 |
| 2 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 19 |
| 3 | `custMastIdHiddenField` | asp:HiddenField | Customer Master Edit |  |  |  |  |  | 29 |
| 4 | `isVatApplicable` | asp:CheckBox |  |  |  |  |  |  | 46 |
| 5 | `customernameTextBox` | asp:TextBox | Customer Name |  |  |  |  |  | 59 |
| 6 | `addressTextBox` | asp:TextBox | Address |  |  |  |  |  | 69 |
| 7 | `addressTextBox2` | asp:TextBox | Address Second |  |  |  |  |  | 81 |
| 8 | `contactTextBox` | asp:TextBox | Contact No |  |  |  |  |  | 94 |
| 9 | `tbxTradeLicenseNo` | asp:TextBox | Drug License No |  |  |  |  |  | 105 |
| 10 | `tbxTradeLicenseExpiryDate` | asp:TextBox | Drug License Expiry Date |  |  |  |  |  | 115 |
| 11 | `imgTradeLicenseExpiryDate` | asp:ImageButton | Drug License Expiry Date | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 116 |
| 12 | `tradeLicenseExpiryDateCalendar` | asp:CalendarExtender |  |  |  |  |  |  | 118 |
| 13 | `cityTextBox` | asp:TextBox | City |  |  |  |  |  | 133 |
| 14 | `contactPersonTextBox` | asp:TextBox | Contact Person |  |  |  |  |  | 148 |
| 15 | `shippingConditionTextBox` | asp:TextBox | Shipping Condition |  |  |  |  |  | 163 |
| 16 | `regionDropDownList` | asp:DropDownList | Region |  | True |  |  | regionDropDownList_SelectedIndexChanged | 178 |
| 17 | `areaDropDownList` | asp:DropDownList | Area |  | True |  |  | areaDropDownList_SelectedIndexChanged | 194 |
| 18 | `territoryDropDownList` | asp:DropDownList | Territory |  | True |  |  | territoryDropDownList_SelectedIndexChanged | 212 |
| 19 | `marketDropDownList` | asp:DropDownList | Market |  | True |  |  |  | 230 |
| 20 | `ddlAIT` | asp:DropDownList | GL Name (AIT) |  |  |  |  |  | 247 |
| 21 | `categoryNameDropDownList` | asp:RadioButtonList | Category Name |  | True |  |  |  | 277 |
| 22 | `paymentTypeDropDownList` | asp:RadioButtonList | Payment Type |  | True |  |  |  | 294 |
| 23 | `(no id)` | asp:ListItem | Payment Type |  |  |  |  |  | 296 |
| 24 | `(no id)` | asp:ListItem | Cash |  |  |  |  |  | 297 |
| 25 | `fbDropDownList` | asp:DropDownList | Fixed Customer |  | True |  |  |  | 309 |
| 26 | `(no id)` | asp:ListItem | Fixed Customer |  |  |  |  |  | 311 |
| 27 | `(no id)` | asp:ListItem | False |  |  |  |  |  | 312 |
| 28 | `updateButton` | asp:Button |  | Update |  |  |  | updateButton_Click | 362 |
| 29 | `closeButton` | asp:Button |  | Close |  |  |  | closeButton_Click | 364 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CustomerMaster`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDown` | `aMasterInfoBll.GetRegion(...)` | `CustomerMasterInfoDal.GetRegion(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SalesDashboardDal.GetRegion` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.GetRegion` | SELECT | `tblRegion` | — |
| `LoadDropDown` | `aMasterInfoBll.LoadAIT(...)` | `CustomerMasterInfoDal.LoadAIT(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.LoadAIT` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.LoadAIT` | SELECT | — | — |
| `LoadDropDown` | `aMasterInfoBll.LoadCategoryName(...)` | `CustomerMasterDAL.LoadCategoryName(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | `CustomerMasterDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoDal.LoadCategoryName` → `ClsCommonInternalDAL.LoadRadioButtonValueWithoutDataBase` → `ProductDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadCategoryName` → `CustomerMasterInfoBll.LoadCategoryName` | SELECT | `tblCustCategory`, `tblProCategory` | — |
| `updateButton_Click` | `aCustomerMasterBLL.Customerorder(...)` | `CustomerMasterBLL.Customerorder(string customerCode)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.Customerorder` → `CustomerMasterDAL.Customerorder` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblOrder` | — |
| `updateButton_Click` | `aMasterInfoBll.Customer(...)` | `CustomerMasterDAL.Customer(string customerId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | `CustomerMasterDAL.Customer` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoDal.Customer` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterBLL.Customer` → `CustomerMasterInfoBll.Customer` | SELECT | `tblInvoice` | — |
| `updateButton_Click` | `aMasterInfoBll.CustomerPayment(...)` | `CustomerMasterDAL.CustomerPayment(string customerId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | `CustomerMasterDAL.CustomerPayment` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoDal.CustomerPayment` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterBLL.CustomerPayment` → `CustomerMasterInfoBll.CustomerPayment` | SELECT | `tblInvoice` | — |
| `updateButton_Click` | `aMasterInfoBll.SaveCoaMasterInfo(...)` | `CustomerMasterInfoDal.SaveCOAMasterInfo(CoaDAO aCustomerMaster)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.SaveCOAMasterInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `CustomerMasterInfoBll.SaveCoaMasterInfo` → `CustomerMasterInfoDal.CreateGlOfCustomer` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | INSERT | — | `sp_I_CustomerGl`, `sp_Integration_CustomerSubsidaryInsert` |
| `updateButton_Click` | `aMasterInfoBll.UpdateDataForCustomerMaster(...)` | `CustomerMasterBLL.UpdateDataForCustomerMaster(CustomerMaster aCustomerMaster)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.UpdateDataForCustomerMaster` → `CustomerMasterDAL.HasCustomerMastername` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterDAL.UpdateCustomerMasterInfo` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | `tblCustMaster` | — |
| `CompanyInfoLoad` | `aMasterInfoBll.CustomerMasterEditLoad(...)` | `CustomerMasterDAL.CustomerMasterEditLoad(string cusmasId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | `CustomerMasterDAL.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterInfoDal.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterBLL.CustomerMasterEditLoad` → `CustomerMasterInfoBll.CustomerMasterEditLoad` | SELECT | `tblArea`, `tblCustCategory`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — |
| `CompanyInfoLoad` | `aMasterInfoBll.GetAreaByRegionId(...)` | `CustomerMasterInfoDal.GetAreaByRegionId(DropDownList ddl, string regionId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetAreaByRegionId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetAreaByRegionId` | SELECT | `tblArea` | — |
| `CompanyInfoLoad` | `aMasterInfoBll.GetMarketNameByTerritoryId(...)` | `CustomerMasterInfoDal.GetMarketNameByTerritoryId(DropDownList ddl,string terrId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetMarketNameByTerritoryId` → `CustPaymentBLL.GetMarketNameByTerritoryId` | SELECT | `tblMarket` | — |
| `CompanyInfoLoad` | `aMasterInfoBll.GetTerritiryByAreaId(...)` | `CustomerMasterInfoDal.GetTerritiryByAreaId(DropDownList ddl,string areaId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetTerritiryByAreaId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetTerritiryByAreaId` | SELECT | `tblTerritory` | — |
| `CompanyInfoLoad` | `aMasterInfoBll.LoadAllMyMarket(...)` | `CustomerMasterInfoDal.LoadAllMyMarket(string id)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.LoadAllMyMarket` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.LoadAllMyMarket` | — | `tblArea`, `tblMarket`, `tblRegion`, `tblTerritory` | — |
| `regionDropDownList_SelectedIndexChanged` | `aMasterInfoBll.GetAreaByRegionId(...)` | `CustomerMasterInfoDal.GetAreaByRegionId(DropDownList ddl, string regionId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetAreaByRegionId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetAreaByRegionId` | SELECT | `tblArea` | — |
| `areaDropDownList_SelectedIndexChanged` | `aMasterInfoBll.GetTerritiryByAreaId(...)` | `CustomerMasterInfoDal.GetTerritiryByAreaId(DropDownList ddl,string areaId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetTerritiryByAreaId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetTerritiryByAreaId` | SELECT | `tblTerritory` | — |
| `territoryDropDownList_SelectedIndexChanged` | `aMasterInfoBll.GetMarketNameByTerritoryId(...)` | `CustomerMasterInfoDal.GetMarketNameByTerritoryId(DropDownList ddl,string terrId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetMarketNameByTerritoryId` → `CustPaymentBLL.GetMarketNameByTerritoryId` | SELECT | `tblMarket` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `regionDropDownList` | Region | asp:DropDownList | True | regionDropDownList_SelectedIndexChanged | `aMasterInfoBll.GetAreaByRegionId()` in `CompanyInfoLoad()`; `aMasterInfoBll.GetAreaByRegionId()` in `regionDropDownList_SelectedIndexChanged()`; `aMasterInfoBll.GetRegion()` in `LoadDropDown()` | `tblArea`, `tblRegion` | set in code (see the DAL method above) |
| `areaDropDownList` | Area | asp:DropDownList | True | areaDropDownList_SelectedIndexChanged | `aMasterInfoBll.GetAreaByRegionId()` in `CompanyInfoLoad()`; `aMasterInfoBll.GetAreaByRegionId()` in `regionDropDownList_SelectedIndexChanged()`; `aMasterInfoBll.GetTerritiryByAreaId()` in `CompanyInfoLoad()`; `aMasterInfoBll.GetTerritiryByAreaId()` in `areaDropDownList_SelectedIndexChanged()` | `tblArea`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory | asp:DropDownList | True | territoryDropDownList_SelectedIndexChanged | `aMasterInfoBll.GetMarketNameByTerritoryId()` in `CompanyInfoLoad()`; `aMasterInfoBll.GetMarketNameByTerritoryId()` in `territoryDropDownList_SelectedIndexChanged()`; `aMasterInfoBll.GetTerritiryByAreaId()` in `CompanyInfoLoad()`; `aMasterInfoBll.GetTerritiryByAreaId()` in `areaDropDownList_SelectedIndexChanged()` | `tblMarket`, `tblTerritory` | set in code (see the DAL method above) |
| `marketDropDownList` | Market | asp:DropDownList | True | — | `aMasterInfoBll.GetMarketNameByTerritoryId()` in `CompanyInfoLoad()`; `aMasterInfoBll.GetMarketNameByTerritoryId()` in `territoryDropDownList_SelectedIndexChanged()` | `tblMarket` | set in code (see the DAL method above) |
| `ddlAIT` | GL Name (AIT) | asp:DropDownList | False | — | `aMasterInfoBll.LoadAIT()` in `LoadDropDown()` | UNKNOWN | set in code (see the DAL method above) |
| `categoryNameDropDownList` | Category Name | asp:RadioButtonList | True | — | `aMasterInfoBll.LoadCategoryName()` in `LoadDropDown()` | `tblCustCategory`, `tblProCategory` | set in code (see the DAL method above) |
| `paymentTypeDropDownList` | Payment Type | asp:RadioButtonList | True | — | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |
| `fbDropDownList` | Fixed Customer | asp:DropDownList | True | — | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |

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
| `imgTradeLicenseExpiryDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `updateButton` | asp:Button | Update | — | `updateButton_Click` | 81 lines | `aCustomerMasterBLL.Customerorder`, `aMasterInfoBll.Customer`, `aMasterInfoBll.CustomerPayment`, `aMasterInfoBll.SaveCoaMasterInfo`, `aMasterInfoBll.UpdateDataForCustomerMaster` | — |
| `closeButton` | asp:Button | Close | — | `closeButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 9 | — | — | — | — |
| 2 | `SetShipingCondition` | `void SetShipingCondition()` | 3 | — | — | — | — |
| 3 | `LoadDropDown` | `void LoadDropDown()` | 6 | `aMasterInfoBll.GetRegion`, `aMasterInfoBll.LoadAIT`, `aMasterInfoBll.LoadCategoryName` | — | — | — |
| 4 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 5 | `Validation` | `bool Validation()` | 75 | — | — | — | — |
| 6 | `updateButton_Click` | `void updateButton_Click(object sender, EventArgs e)` | 81 | `aCustomerMasterBLL.Customerorder`, `aMasterInfoBll.Customer`, `aMasterInfoBll.CustomerPayment`, `aMasterInfoBll.SaveCoaMasterInfo`, `aMasterInfoBll.UpdateDataForCustomerMaster` | — | — | `LoginName` |
| 7 | `CompanyInfoLoad` | `void CompanyInfoLoad(string custMastId)` | 59 | `aMasterInfoBll.CustomerMasterEditLoad`, `aMasterInfoBll.GetAreaByRegionId`, `aMasterInfoBll.GetMarketNameByTerritoryId`, `aMasterInfoBll.GetTerritiryByAreaId`, `aMasterInfoBll.LoadAllMyMarket` | — | — | `LoginName` |
| 8 | `closeButton_Click` | `void closeButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 9 | `custMasterListImageButton_Click` | `void custMasterListImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 10 | `regionDropDownList_SelectedIndexChanged` | `void regionDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | `aMasterInfoBll.GetAreaByRegionId` | — | — | — |
| 11 | `areaDropDownList_SelectedIndexChanged` | `void areaDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | `aMasterInfoBll.GetTerritiryByAreaId` | — | — | — |
| 12 | `territoryDropDownList_SelectedIndexChanged` | `void territoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | `aMasterInfoBll.GetMarketNameByTerritoryId` | — | — | — |

Call chain shape for every persisting action on this page:

```
CustMasterEdit.aspx (markup)
  -> SInventory_UI_CustMasterEdit (code-behind event handler)
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
| `LoadDropDown` | `CustomerMasterInfoBll` | `GetRegion` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `LoadDropDown` | `CustomerMasterInfoBll` | `LoadAIT` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `LoadDropDown` | `CustomerMasterBLL` | `LoadCategoryName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `LoadDropDown` | `CustomerMasterInfoBll` | `LoadCategoryName` | `RadioButtonList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `Customerorder` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `Customer` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterInfoBll` | `Customer` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `CustomerPayment` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterInfoBll` | `CustomerPayment` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `updateButton_Click` | `CustomerMasterInfoBll` | `SaveCoaMasterInfo` | `int customerId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `UpdateDataForCustomerMaster` | `CustomerMaster aCustomerMaster` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterInfoBll` | `UpdateDataForCustomerMaster` | `CustomerMaster aCustomerMaster` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `CompanyInfoLoad` | `CustomerMasterBLL` | `CustomerMasterEditLoad` | `string CustomerMasterId` | `CustomerMaster` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoBll` | `CustomerMasterEditLoad` | `string CustomerMasterId` | `CustomerMaster` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoBll` | `GetAreaByRegionId` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoBll` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `CompanyInfoLoad` | `CustPaymentBLL` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoBll` | `GetTerritiryByAreaId` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoBll` | `LoadAllMyMarket` | `string id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `regionDropDownList_SelectedIndexChanged` | `CustomerMasterInfoBll` | `GetAreaByRegionId` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `areaDropDownList_SelectedIndexChanged` | `CustomerMasterInfoBll` | `GetTerritiryByAreaId` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustomerMasterInfoBll` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropDown` | `CustomerMasterInfoDal` | `GetRegion` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `LoadDropDown` | `SalesDashboardDal` | `GetRegion` | `` | `DataTable` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `LoadDropDown` | `CustomerMasterInfoDal` | `LoadAIT` | `DropDownList ddl` | `void` | SELECT | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `LoadDropDown` | `CustomerMasterDAL` | `LoadCategoryName` | `DropDownList ddl` | `void` | SELECT | `tblCustCategory` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `LoadDropDown` | `CustomerMasterInfoDal` | `LoadCategoryName` | `RadioButtonList ddl` | `void` | SELECT | `tblCustCategory` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `LoadDropDown` | `ProductDAL` | `LoadCategoryName` | `DropDownList ddl` | `void` | — | `tblProCategory` | — | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| `updateButton_Click` | `CustomerMasterDAL` | `Customer` | `string customerId` | `DataTable` | SELECT | `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `updateButton_Click` | `CustomerMasterInfoDal` | `Customer` | `string customerId` | `DataTable` | SELECT | `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `updateButton_Click` | `CustomerMasterDAL` | `CustomerPayment` | `string customerId` | `DataTable` | SELECT | `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `updateButton_Click` | `CustomerMasterInfoDal` | `CustomerPayment` | `string customerId` | `DataTable` | SELECT | `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `updateButton_Click` | `CustomerMasterInfoDal` | `SaveCOAMasterInfo` | `CoaDAO aCustomerMaster` | `bool` | INSERT | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `CompanyInfoLoad` | `CustomerMasterDAL` | `CustomerMasterEditLoad` | `string cusmasId` | `CustomerMaster` | SELECT | `tblArea`, `tblCustCategory`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoDal` | `CustomerMasterEditLoad` | `string cusmasId` | `CustomerMaster` | — | `tblCustMaster` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoDal` | `GetAreaByRegionId` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoDal` | `GetMarketNameByTerritoryId` | `DropDownList ddl,string terrId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `CompanyInfoLoad` | `CustPaymentDAL` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoDal` | `GetTerritiryByAreaId` | `DropDownList ddl,string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `CompanyInfoLoad` | `CustomerMasterInfoDal` | `LoadAllMyMarket` | `string id` | `DataTable` | — | `tblArea`, `tblMarket`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `regionDropDownList_SelectedIndexChanged` | `CustomerMasterInfoDal` | `GetAreaByRegionId` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `areaDropDownList_SelectedIndexChanged` | `CustomerMasterInfoDal` | `GetTerritiryByAreaId` | `DropDownList ddl,string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustomerMasterInfoDal` | `GetMarketNameByTerritoryId` | `DropDownList ddl,string terrId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustPaymentDAL` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_I_CustomerGl` | updateButton_Click → `CustomerMasterInfoDal.SaveCOAMasterInfo → ClsCommonInternalDAL.SaveDataByInsertCommand → CustomerMasterInfoBll.SaveCoaMasterInfo → CustomerMasterInfoDal.CreateGlOfCustomer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction` | **No** — see note |
| `sp_Integration_CustomerSubsidaryInsert` | updateButton_Click → `CustomerMasterInfoDal.SaveCOAMasterInfo → ClsCommonInternalDAL.SaveDataByInsertCommand → CustomerMasterInfoBll.SaveCoaMasterInfo → CustomerMasterInfoDal.CreateGlOfCustomer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `CompanyInfoLoad()` → `CustomerMasterDAL.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterInfoDal.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `CompanyInfoLoad()` → `CustomerMasterInfoDal.GetAreaByRegionId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetAreaByRegionId`, `CompanyInfoLoad()` → `CustomerMasterInfoDal.LoadAllMyMarket` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.LoadAllMyMarket`, `regionDropDownList_SelectedIndexChanged()` → `CustomerMasterInfoDal.GetAreaByRegionId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetAreaByRegionId` |
| `tblCustCategory` | `CompanyInfoLoad()` → `CustomerMasterDAL.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterInfoDal.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `LoadDropDown()` → `CustomerMasterDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoDal.LoadCategoryName` → `ClsCommonInternalDAL.LoadRadioButtonValueWithoutDataBase` |
| `tblCustMaster` | `CompanyInfoLoad()` → `CustomerMasterDAL.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterInfoDal.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `updateButton_Click()` → `CustomerMasterBLL.UpdateDataForCustomerMaster` → `CustomerMasterDAL.HasCustomerMastername` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterDAL.UpdateCustomerMasterInfo` |
| `tblInvoice` | `updateButton_Click()` → `CustomerMasterDAL.CustomerPayment` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoDal.CustomerPayment` → `ClsCommonInternalDAL.DataContainerDataTable`, `updateButton_Click()` → `CustomerMasterDAL.Customer` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoDal.Customer` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `CompanyInfoLoad()` → `CustomerMasterDAL.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterInfoDal.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `CompanyInfoLoad()` → `CustomerMasterInfoDal.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `CompanyInfoLoad()` → `CustomerMasterInfoDal.LoadAllMyMarket` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.LoadAllMyMarket`, `territoryDropDownList_SelectedIndexChanged()` → `CustomerMasterInfoDal.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblOrder` | `updateButton_Click()` → `CustomerMasterBLL.Customerorder` → `CustomerMasterDAL.Customerorder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProCategory` | `LoadDropDown()` → `CustomerMasterDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoDal.LoadCategoryName` → `ClsCommonInternalDAL.LoadRadioButtonValueWithoutDataBase` |
| `tblRegion` | `CompanyInfoLoad()` → `CustomerMasterDAL.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterInfoDal.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `CompanyInfoLoad()` → `CustomerMasterInfoDal.LoadAllMyMarket` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.LoadAllMyMarket`, `LoadDropDown()` → `CustomerMasterInfoDal.GetRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SalesDashboardDal.GetRegion` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `CompanyInfoLoad()` → `CustomerMasterDAL.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterInfoDal.CustomerMasterEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `CompanyInfoLoad()` → `CustomerMasterInfoDal.GetTerritiryByAreaId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetTerritiryByAreaId`, `CompanyInfoLoad()` → `CustomerMasterInfoDal.LoadAllMyMarket` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.LoadAllMyMarket`, `areaDropDownList_SelectedIndexChanged()` → `CustomerMasterInfoDal.GetTerritiryByAreaId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetTerritiryByAreaId` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `SetShipingCondition` | N/A | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-010-001` | N/A | Code | `Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs` → `SetShipingCondition()` | user action on this page | operation is refused | "N/A" | CONFIRMED |
| `BR-MST-010-002` | {0} | Code | `Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| Database object | `tblArea` |
| Database object | `tblCustCategory` |
| Database object | `tblCustMaster` |
| Database object | `tblInvoice` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblProCategory` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Stored procedure | `sp_I_CustomerGl` |
| Stored procedure | `sp_Integration_CustomerSubsidaryInsert` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-010-001` | Unauthenticated access | Request `/SInventory_UI/CustMasterEdit.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-MST-010-002` | Validation: N/A | Trigger `SetShipingCondition()` in the state that produces this message | alert "N/A" and the operation is not performed | CONFIRMED |
| `TC-MST-010-003` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustMasterEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustMasterEdit.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 29 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 12 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 2 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 9 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` is **shared with 10 other page(s)** (e.g. `CustomerMasterViewer.aspx`, `CustMasterEntry.aspx`, `CustMasterView.aspx`, `CustomerMasterEdit.aspx`, `CustomerMasterReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` is **shared with 2 other page(s)** (e.g. `CustMasterEntry.aspx`, `Dashboard.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` is **shared with 3 other page(s)** (e.g. `CustMasterEntry.aspx`, `CustomerReport.aspx`, `FieldForceSalesSummery.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` is **shared with 5 other page(s)** (e.g. `CustMasterEntry.aspx`, `Dashboard.aspx`, `FixedDiscountEntry.aspx`, `InvoiceCreationForConsumerCustomer.aspx`, `SupplierInformation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProductDAL.cs` is **shared with 3 other page(s)** (e.g. `CustMasterEntry.aspx`, `CustomerCreditView.aspx`, `ProductQuotedPriceView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` is **shared with 4 other page(s)** (e.g. `CashFlow.aspx`, `CustMasterEntry.aspx`, `Dashboard.aspx`, `IncomeStatement.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCustCategory` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblProCategory` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
