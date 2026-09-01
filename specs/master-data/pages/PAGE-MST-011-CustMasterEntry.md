# MST-011 — CustMasterEntry

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-011` |
| Module | `master-data` |
| File name | `CustMasterEntry.aspx` |
| Route | `/SInventory_UI/CustMasterEntry.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `CustMasterEntry.aspx.cs` |
| Code-behind class | `SInventory_UI_CustMasterEntry` |
| Markup lines | 550 |
| Code-behind lines | 404 |
| Server controls | 37 |
| GridViews | 0 |
| Code-behind methods | 18 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/CustMasterView.aspx`
- referenced from `SInventory_UI/OrderDeleteorEdit.aspx`

Outbound `Response.Redirect` targets: `CustMasterView.aspx`

## 5. Route

`/SInventory_UI/CustMasterEntry.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustMasterEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustMasterEntry.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |

## 7. UI Control Inventory

37 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 34 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 36 |
| 4 | `custMastListImageButton` | asp:ImageButton | View List | ~/images/viewList11.png |  |  |  | custMasterListImageButton_Click | 62 |
| 5 | `isVatApplicable` | asp:CheckBox |  |  |  |  |  |  | 82 |
| 6 | `customernameTextBox` | asp:TextBox | Customer Name |  |  |  |  |  | 96 |
| 7 | `addressTextBox` | asp:TextBox | Address |  |  |  |  |  | 106 |
| 8 | `addressTextBox2` | asp:TextBox | Address Second |  |  |  |  |  | 118 |
| 9 | `contactTextBox` | asp:TextBox | Contact No |  |  |  |  |  | 131 |
| 10 | `tbxTradeLicenseNo` | asp:TextBox | Drug License No |  |  |  |  |  | 142 |
| 11 | `tbxTradeLicenseExpiryDate` | asp:TextBox | Drug License Expiry Date |  |  |  |  |  | 152 |
| 12 | `imgTradeLicenseExpiryDate` | asp:ImageButton | Drug License Expiry Date | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 153 |
| 13 | `tradeLicenseExpiryDateCalendar` | asp:CalendarExtender |  |  |  |  |  |  | 155 |
| 14 | `cityTextBox` | asp:TextBox | City |  |  |  |  |  | 172 |
| 15 | `contactPersonTextBox` | asp:TextBox | Contact Person |  |  |  |  |  | 187 |
| 16 | `shippingConditionTextBox` | asp:TextBox | Shipping Condition |  |  |  |  |  | 202 |
| 17 | `regionDropDownList` | asp:DropDownList | Region |  | True |  |  | regionDropDownList_SelectedIndexChanged | 217 |
| 18 | `areaDropDownList` | asp:DropDownList | Area |  | True |  |  | areaDropDownList_SelectedIndexChanged | 233 |
| 19 | `territoryDropDownList` | asp:DropDownList | Territory |  | True |  |  | territoryDropDownList_SelectedIndexChanged | 251 |
| 20 | `marketDropDownList` | asp:DropDownList | Market |  | True |  |  |  | 269 |
| 21 | `ddlAIT` | asp:DropDownList | GL Name (AIT) |  |  |  |  |  | 287 |
| 22 | `categoryNameDropDownList` | asp:RadioButtonList | Category Name |  | True |  |  |  | 317 |
| 23 | `paymentTypeDropDownList` | asp:RadioButtonList | Payment Type |  | True |  |  |  | 332 |
| 24 | `(no id)` | asp:ListItem | Payment Type |  |  |  |  |  | 334 |
| 25 | `(no id)` | asp:ListItem | Cash |  |  |  |  |  | 335 |
| 26 | `fbDropDownList` | asp:DropDownList | Fixed Customer |  | True |  |  |  | 349 |
| 27 | `(no id)` | asp:ListItem | Fixed Customer |  |  |  |  |  | 351 |
| 28 | `(no id)` | asp:ListItem | False |  |  |  |  |  | 352 |
| 29 | `ddlLedgerHead` | asp:DropDownList | Ledger Head |  | True |  |  | ddlLedgerHead_OnSelectedIndexChanged | 368 |
| 30 | `ddlControlLedger` | asp:DropDownList | Control Ledger |  | True |  |  | ddlControlLedger_OnSelectedIndexChanged | 387 |
| 31 | `ddlSubsidiaryLedger` | asp:DropDownList | Subsidiary Ledger |  | True |  |  | ddlSubsidiaryLedger_OnSelectedIndexChanged | 407 |
| 32 | `ddlSubSubsidiaryLedger` | asp:DropDownList | Sub-Subsidiary Ledger |  | True |  |  | ddlSubSubsidiaryLedger_OnSelectedIndexChanged | 427 |
| 33 | `ddlAccountName` | asp:DropDownList | Account Name |  |  |  |  |  | 447 |
| 34 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click1 | 494 |
| 35 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 497 |
| 36 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 539 |
| 37 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 542 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CoaDAO`, `CustomerMaster`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `SetCustomerCode` | `aMasterInfoBll.LoadCustomerMasterId(...)` | `CustomerMasterInfoBll.LoadCustomerMasterId()` <br/>`Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` | `CustomerMasterInfoBll.LoadCustomerMasterId` → `CustomerMasterInfoDal.GetCustomerMasterId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster` | — |
| `LoadDropDown` | `aMasterInfoBll.GetLedgerHead(...)` | `CustomerMasterInfoDal.GetLedgerHead(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetLedgerHead` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetLedgerHead` | — | — | — |
| `LoadDropDown` | `aMasterInfoBll.GetRegion(...)` | `CustomerMasterInfoDal.GetRegion(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SalesDashboardDal.GetRegion` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterInfoBll.GetRegion` | SELECT | `tblRegion` | — |
| `LoadDropDown` | `aMasterInfoBll.LoadAIT(...)` | `CustomerMasterInfoDal.LoadAIT(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.LoadAIT` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.LoadAIT` | SELECT | — | — |
| `LoadDropDown` | `aMasterInfoBll.LoadCategoryName(...)` | `CustomerMasterDAL.LoadCategoryName(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | `CustomerMasterDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoDal.LoadCategoryName` → `ClsCommonInternalDAL.LoadRadioButtonValueWithoutDataBase` → `ProductDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadCategoryName` → `CustomerMasterInfoBll.LoadCategoryName` | SELECT | `tblCustCategory`, `tblProCategory` | — |
| `submitButton_Click1` | `aMasterInfoBll.SaveCoaMasterInfo(...)` | `CustomerMasterInfoDal.SaveCOAMasterInfo(CoaDAO aCustomerMaster)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.SaveCOAMasterInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `CustomerMasterInfoBll.SaveCoaMasterInfo` → `CustomerMasterInfoDal.CreateGlOfCustomer` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | INSERT | — | `sp_I_CustomerGl`, `sp_Integration_CustomerSubsidaryInsert` |
| `submitButton_Click1` | `aMasterInfoBll.SaveCustMasterInfo(...)` | `CustomerMasterBLL.SaveCustMasterInfo(CustomerMaster CustomerMaster)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.SaveCustMasterInfo` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterDAL.HasCustomerMastername` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterDAL.SaveCustometMasterInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `CustomerMasterBLL.CustMasterCodeGenerator` | INSERT, SELECT | `tblCustMaster` | — |
| `regionDropDownList_SelectedIndexChanged` | `aMasterInfoBll.GetAreaByRegionId(...)` | `CustomerMasterInfoDal.GetAreaByRegionId(DropDownList ddl, string regionId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetAreaByRegionId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetAreaByRegionId` | SELECT | `tblArea` | — |
| `areaDropDownList_SelectedIndexChanged` | `aMasterInfoBll.GetTerritiryByAreaId(...)` | `CustomerMasterInfoDal.GetTerritiryByAreaId(DropDownList ddl,string areaId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetTerritiryByAreaId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetTerritiryByAreaId` | SELECT | `tblTerritory` | — |
| `territoryDropDownList_SelectedIndexChanged` | `aMasterInfoBll.GetMarketNameByTerritoryId(...)` | `CustomerMasterInfoDal.GetMarketNameByTerritoryId(DropDownList ddl,string terrId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetMarketNameByTerritoryId` → `CustPaymentBLL.GetMarketNameByTerritoryId` | SELECT | `tblMarket` | — |
| `ddlLedgerHead_OnSelectedIndexChanged` | `aMasterInfoBll.LoadControlCode(...)` | `CustomerMasterInfoDal.LoadControlCode(DropDownList ddl, string LedgerHead)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.LoadControlCode` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.LoadControlCode` | — | — | — |
| `ddlControlLedger_OnSelectedIndexChanged` | `aMasterInfoBll.LoadSubsidaryCode(...)` | `CustomerMasterInfoDal.LoadSubsidaryCode(DropDownList ddl, string ControlLedger)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.LoadSubsidaryCode` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.LoadSubsidaryCode` | — | — | — |
| `ddlSubsidiaryLedger_OnSelectedIndexChanged` | `aMasterInfoBll.LoadSUBSubsidaryCode(...)` | `CustomerMasterInfoDal.LoadSUBSubsidaryCode(DropDownList ddl, string ControlLedger)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.LoadSUBSubsidaryCode` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.LoadSUBSubsidaryCode` | SELECT | — | — |
| `ddlSubSubsidiaryLedger_OnSelectedIndexChanged` | `aMasterInfoBll.LoadAccountName(...)` | `CustomerMasterInfoDal.LoadAccountName(DropDownList ddl, string ControlLedger)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.LoadAccountName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.LoadAccountName` | SELECT | — | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `regionDropDownList` | Region | asp:DropDownList | True | regionDropDownList_SelectedIndexChanged | `aMasterInfoBll.GetAreaByRegionId()` in `regionDropDownList_SelectedIndexChanged()`; `aMasterInfoBll.GetRegion()` in `LoadDropDown()` | `tblArea`, `tblRegion` | set in code (see the DAL method above) |
| `areaDropDownList` | Area | asp:DropDownList | True | areaDropDownList_SelectedIndexChanged | `aMasterInfoBll.GetAreaByRegionId()` in `regionDropDownList_SelectedIndexChanged()`; `aMasterInfoBll.GetTerritiryByAreaId()` in `areaDropDownList_SelectedIndexChanged()` | `tblArea`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory | asp:DropDownList | True | territoryDropDownList_SelectedIndexChanged | `aMasterInfoBll.GetMarketNameByTerritoryId()` in `territoryDropDownList_SelectedIndexChanged()`; `aMasterInfoBll.GetTerritiryByAreaId()` in `areaDropDownList_SelectedIndexChanged()` | `tblMarket`, `tblTerritory` | set in code (see the DAL method above) |
| `marketDropDownList` | Market | asp:DropDownList | True | — | `aMasterInfoBll.GetMarketNameByTerritoryId()` in `territoryDropDownList_SelectedIndexChanged()` | `tblMarket` | set in code (see the DAL method above) |
| `ddlAIT` | GL Name (AIT) | asp:DropDownList | False | — | `aMasterInfoBll.LoadAIT()` in `LoadDropDown()` | UNKNOWN | set in code (see the DAL method above) |
| `categoryNameDropDownList` | Category Name | asp:RadioButtonList | True | — | `aMasterInfoBll.LoadCategoryName()` in `LoadDropDown()` | `tblCustCategory`, `tblProCategory` | set in code (see the DAL method above) |
| `paymentTypeDropDownList` | Payment Type | asp:RadioButtonList | True | — | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |
| `fbDropDownList` | Fixed Customer | asp:DropDownList | True | — | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |
| `ddlLedgerHead` | Ledger Head | asp:DropDownList | True | ddlLedgerHead_OnSelectedIndexChanged | `aMasterInfoBll.GetLedgerHead()` in `LoadDropDown()`; `aMasterInfoBll.LoadControlCode()` in `ddlLedgerHead_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `ddlControlLedger` | Control Ledger | asp:DropDownList | True | ddlControlLedger_OnSelectedIndexChanged | `aMasterInfoBll.LoadControlCode()` in `ddlLedgerHead_OnSelectedIndexChanged()`; `aMasterInfoBll.LoadSubsidaryCode()` in `ddlControlLedger_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `ddlSubsidiaryLedger` | Subsidiary Ledger | asp:DropDownList | True | ddlSubsidiaryLedger_OnSelectedIndexChanged | `aMasterInfoBll.LoadSUBSubsidaryCode()` in `ddlSubsidiaryLedger_OnSelectedIndexChanged()`; `aMasterInfoBll.LoadSubsidaryCode()` in `ddlControlLedger_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `ddlSubSubsidiaryLedger` | Sub-Subsidiary Ledger | asp:DropDownList | True | ddlSubSubsidiaryLedger_OnSelectedIndexChanged | `aMasterInfoBll.LoadAccountName()` in `ddlSubSubsidiaryLedger_OnSelectedIndexChanged()`; `aMasterInfoBll.LoadSUBSubsidaryCode()` in `ddlSubsidiaryLedger_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `ddlAccountName` | Account Name | asp:DropDownList | False | — | `aMasterInfoBll.LoadAccountName()` in `ddlSubSubsidiaryLedger_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |

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
| `custMastListImageButton` | asp:ImageButton |  | — | `custMasterListImageButton_Click` | 3 lines | — | — |
| `imgTradeLicenseExpiryDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click1` | 79 lines | `aMasterInfoBll.SaveCoaMasterInfo`, `aMasterInfoBll.SaveCustMasterInfo` | — |
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
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 9 | — | — | — | — |
| 2 | `SetNA` | `void SetNA()` | 6 | — | — | — | — |
| 3 | `SetCustomerCode` | `void SetCustomerCode()` | 11 | `aMasterInfoBll.LoadCustomerMasterId` | — | — | — |
| 4 | `SetShipingCondition` | `void SetShipingCondition()` | 3 | — | — | — | — |
| 5 | `LoadDropDown` | `void LoadDropDown()` | 7 | `aMasterInfoBll.GetLedgerHead`, `aMasterInfoBll.GetRegion`, `aMasterInfoBll.LoadAIT`, `aMasterInfoBll.LoadCategoryName` | — | — | — |
| 6 | `Clear` | `void Clear()` | 24 | — | — | — | — |
| 7 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 8 | `Validation` | `bool Validation()` | 99 | — | — | — | — |
| 9 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 79 | `aMasterInfoBll.SaveCoaMasterInfo`, `aMasterInfoBll.SaveCustMasterInfo` | — | — | `LoginName` |
| 10 | `custMasterListImageButton_Click` | `void custMasterListImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 11 | `regionDropDownList_SelectedIndexChanged` | `void regionDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 14 | `aMasterInfoBll.GetAreaByRegionId` | — | — | — |
| 12 | `areaDropDownList_SelectedIndexChanged` | `void areaDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 13 | `aMasterInfoBll.GetTerritiryByAreaId` | — | — | — |
| 13 | `territoryDropDownList_SelectedIndexChanged` | `void territoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 13 | `aMasterInfoBll.GetMarketNameByTerritoryId` | — | — | — |
| 14 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 15 | `ddlLedgerHead_OnSelectedIndexChanged` | `void ddlLedgerHead_OnSelectedIndexChanged(object sender, EventArgs e)` | 18 | `aMasterInfoBll.LoadControlCode` | — | — | — |
| 16 | `ddlControlLedger_OnSelectedIndexChanged` | `void ddlControlLedger_OnSelectedIndexChanged(object sender, EventArgs e)` | 15 | `aMasterInfoBll.LoadSubsidaryCode` | — | — | — |
| 17 | `ddlSubsidiaryLedger_OnSelectedIndexChanged` | `void ddlSubsidiaryLedger_OnSelectedIndexChanged(object sender, EventArgs e)` | 13 | `aMasterInfoBll.LoadSUBSubsidaryCode` | — | — | — |
| 18 | `ddlSubSubsidiaryLedger_OnSelectedIndexChanged` | `void ddlSubSubsidiaryLedger_OnSelectedIndexChanged(object sender, EventArgs e)` | 10 | `aMasterInfoBll.LoadAccountName` | — | — | — |

Call chain shape for every persisting action on this page:

```
CustMasterEntry.aspx (markup)
  -> SInventory_UI_CustMasterEntry (code-behind event handler)
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
| `SetCustomerCode` | `CustomerMasterInfoBll` | `LoadCustomerMasterId` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `LoadDropDown` | `CustomerMasterInfoBll` | `GetLedgerHead` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `LoadDropDown` | `CustomerMasterInfoBll` | `GetRegion` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `LoadDropDown` | `CustomerMasterInfoBll` | `LoadAIT` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `LoadDropDown` | `CustomerMasterBLL` | `LoadCategoryName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `LoadDropDown` | `CustomerMasterInfoBll` | `LoadCategoryName` | `RadioButtonList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `submitButton_Click1` | `CustomerMasterInfoBll` | `SaveCoaMasterInfo` | `int customerId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `submitButton_Click1` | `CustomerMasterBLL` | `SaveCustMasterInfo` | `CustomerMaster CustomerMaster` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `submitButton_Click1` | `CustomerMasterInfoBll` | `SaveCustMasterInfo` | `CustomerMaster CustomerMaster` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `regionDropDownList_SelectedIndexChanged` | `CustomerMasterInfoBll` | `GetAreaByRegionId` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `areaDropDownList_SelectedIndexChanged` | `CustomerMasterInfoBll` | `GetTerritiryByAreaId` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustomerMasterInfoBll` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `ddlLedgerHead_OnSelectedIndexChanged` | `CustomerMasterInfoBll` | `LoadControlCode` | `DropDownList ddl, string LedgerHead` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `ddlControlLedger_OnSelectedIndexChanged` | `CustomerMasterInfoBll` | `LoadSubsidaryCode` | `DropDownList ddl, string ControlLedger` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `ddlSubsidiaryLedger_OnSelectedIndexChanged` | `CustomerMasterInfoBll` | `LoadSUBSubsidaryCode` | `DropDownList ddl, string ControlLedger` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |
| `ddlSubSubsidiaryLedger_OnSelectedIndexChanged` | `CustomerMasterInfoBll` | `LoadAccountName` | `DropDownList ddl, string ControlLedger` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropDown` | `CustomerMasterInfoDal` | `GetLedgerHead` | `DropDownList ddl` | `void` | — | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `LoadDropDown` | `CustomerMasterInfoDal` | `GetRegion` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `LoadDropDown` | `SalesDashboardDal` | `GetRegion` | `` | `DataTable` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` |
| `LoadDropDown` | `CustomerMasterInfoDal` | `LoadAIT` | `DropDownList ddl` | `void` | SELECT | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `LoadDropDown` | `CustomerMasterDAL` | `LoadCategoryName` | `DropDownList ddl` | `void` | SELECT | `tblCustCategory` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `LoadDropDown` | `CustomerMasterInfoDal` | `LoadCategoryName` | `RadioButtonList ddl` | `void` | SELECT | `tblCustCategory` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `LoadDropDown` | `ProductDAL` | `LoadCategoryName` | `DropDownList ddl` | `void` | — | `tblProCategory` | — | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| `submitButton_Click1` | `CustomerMasterInfoDal` | `SaveCOAMasterInfo` | `CoaDAO aCustomerMaster` | `bool` | INSERT | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `regionDropDownList_SelectedIndexChanged` | `CustomerMasterInfoDal` | `GetAreaByRegionId` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `areaDropDownList_SelectedIndexChanged` | `CustomerMasterInfoDal` | `GetTerritiryByAreaId` | `DropDownList ddl,string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustomerMasterInfoDal` | `GetMarketNameByTerritoryId` | `DropDownList ddl,string terrId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustPaymentDAL` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `ddlLedgerHead_OnSelectedIndexChanged` | `CustomerMasterInfoDal` | `LoadControlCode` | `DropDownList ddl, string LedgerHead` | `void` | — | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `ddlControlLedger_OnSelectedIndexChanged` | `CustomerMasterInfoDal` | `LoadSubsidaryCode` | `DropDownList ddl, string ControlLedger` | `void` | — | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `ddlSubsidiaryLedger_OnSelectedIndexChanged` | `CustomerMasterInfoDal` | `LoadSUBSubsidaryCode` | `DropDownList ddl, string ControlLedger` | `void` | SELECT | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| `ddlSubSubsidiaryLedger_OnSelectedIndexChanged` | `CustomerMasterInfoDal` | `LoadAccountName` | `DropDownList ddl, string ControlLedger` | `void` | SELECT | — | — | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_I_CustomerGl` | submitButton_Click1 → `CustomerMasterInfoDal.SaveCOAMasterInfo → ClsCommonInternalDAL.SaveDataByInsertCommand → CustomerMasterInfoBll.SaveCoaMasterInfo → CustomerMasterInfoDal.CreateGlOfCustomer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction` | **No** — see note |
| `sp_Integration_CustomerSubsidaryInsert` | submitButton_Click1 → `CustomerMasterInfoDal.SaveCOAMasterInfo → ClsCommonInternalDAL.SaveDataByInsertCommand → CustomerMasterInfoBll.SaveCoaMasterInfo → CustomerMasterInfoDal.CreateGlOfCustomer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `regionDropDownList_SelectedIndexChanged()` → `CustomerMasterInfoDal.GetAreaByRegionId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetAreaByRegionId` |
| `tblCustCategory` | `LoadDropDown()` → `CustomerMasterDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoDal.LoadCategoryName` → `ClsCommonInternalDAL.LoadRadioButtonValueWithoutDataBase` |
| `tblCustMaster` | `SetCustomerCode()` → `CustomerMasterInfoBll.LoadCustomerMasterId` → `CustomerMasterInfoDal.GetCustomerMasterId` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `CustomerMasterBLL.SaveCustMasterInfo` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerMasterDAL.HasCustomerMastername` |
| `tblMarket` | `territoryDropDownList_SelectedIndexChanged()` → `CustomerMasterInfoDal.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblProCategory` | `LoadDropDown()` → `CustomerMasterDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoDal.LoadCategoryName` → `ClsCommonInternalDAL.LoadRadioButtonValueWithoutDataBase` |
| `tblRegion` | `LoadDropDown()` → `CustomerMasterInfoDal.GetRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SalesDashboardDal.GetRegion` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `areaDropDownList_SelectedIndexChanged()` → `CustomerMasterInfoDal.GetTerritiryByAreaId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterInfoBll.GetTerritiryByAreaId` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `SetNA` | N/A | No | Yes | UNKNOWN | Notification |
| `SetShipingCondition` | N/A | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-011-001` | N/A | Code | `Solution.Web/SInventory_UI/CustMasterEntry.aspx.cs` → `SetNA()` | user action on this page | operation is refused | "N/A" | CONFIRMED |
| `BR-MST-011-002` | N/A | Code | `Solution.Web/SInventory_UI/CustMasterEntry.aspx.cs` → `SetShipingCondition()` | user action on this page | operation is refused | "N/A" | CONFIRMED |
| `BR-MST-011-003` | {0} | Code | `Solution.Web/SInventory_UI/CustMasterEntry.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`

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
| Database object | `tblMarket` |
| Database object | `tblProCategory` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Stored procedure | `sp_I_CustomerGl` |
| Stored procedure | `sp_Integration_CustomerSubsidaryInsert` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-011-001` | Unauthenticated access | Request `/SInventory_UI/CustMasterEntry.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-011-002` | Validation: N/A | Trigger `SetNA()` in the state that produces this message | alert "N/A" and the operation is not performed | CONFIRMED |
| `TC-MST-011-003` | Validation: N/A | Trigger `SetShipingCondition()` in the state that produces this message | alert "N/A" and the operation is not performed | CONFIRMED |
| `TC-MST-011-004` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustMasterEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustMasterEntry.aspx.cs` |
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
| UI controls | absent | 37 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 18 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 2 names found | bodies **not in repo** | **PARTIAL** | section 16 |
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

- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` is **shared with 10 other page(s)** (e.g. `CustomerMasterViewer.aspx`, `CustMasterEdit.aspx`, `CustMasterView.aspx`, `CustomerMasterEdit.aspx`, `CustomerMasterReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` is **shared with 2 other page(s)** (e.g. `CustMasterEdit.aspx`, `Dashboard.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` is **shared with 3 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustomerReport.aspx`, `FieldForceSalesSummery.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` is **shared with 5 other page(s)** (e.g. `CustMasterEdit.aspx`, `Dashboard.aspx`, `FixedDiscountEntry.aspx`, `InvoiceCreationForConsumerCustomer.aspx`, `SupplierInformation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProductDAL.cs` is **shared with 3 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustomerCreditView.aspx`, `ProductQuotedPriceView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` is **shared with 4 other page(s)** (e.g. `CashFlow.aspx`, `CustMasterEdit.aspx`, `Dashboard.aspx`, `IncomeStatement.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCustCategory` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblProCategory` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
