# MST-028 — DistributionCenterInformation

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-028` |
| Module | `master-data` |
| File name | `DistributionCenterInformation.aspx` |
| Route | `/SInventory_UI/DistributionCenterInformation.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `DistributionCenterInformation.aspx.cs` |
| Code-behind class | `SInventory_UI_DistributionCenterInformation` |
| Markup lines | 310 |
| Code-behind lines | 160 |
| Server controls | 20 |
| GridViews | 0 |
| Code-behind methods | 12 |

## 2. Purpose

Master-data / setup screen. It captures and maintains reference data used by the transactional screens.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Master-data / setup screen. It captures and maintains reference data used by the transactional screens.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/DistributionCenterInfoView.aspx`

Outbound `Response.Redirect` targets: `DistributionCenterInfoView.aspx`

## 5. Route

`/SInventory_UI/DistributionCenterInformation.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DistributionCenterInformation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DistributionCenterInformation.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |

## 7. UI Control Inventory

20 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `unitViewImageButton` | asp:ImageButton | View List | ~/images/viewList11.png |  |  |  | unitViewImageButton_Click | 41 |
| 5 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_OnSelectedIndexChanged | 80 |
| 6 | `ddlDcType` | asp:DropDownList | Distribution Type: |  |  |  |  |  | 98 |
| 7 | `salesCenternameTextBox` | asp:TextBox | Distribution Center Name: |  | true |  |  | salesCenternameTextBox_OnTextChanged | 115 |
| 8 | `dcShortNameTextBox` | asp:TextBox | DC Short Name: |  | true |  |  | dcShortNameTextBox_OnTextChanged | 132 |
| 9 | `addressTextBox` | asp:TextBox | Address: |  |  |  |  |  | 148 |
| 10 | `phoneNoTextBox` | asp:TextBox | Phone No: |  |  |  |  |  | 163 |
| 11 | `FilteredTextBoxExtender2` | cc1:FilteredTextBoxExtender | Phone No: |  |  | Enabled=True |  |  | 164 |
| 12 | `mobileNoTextBox` | asp:TextBox | Mobile No: |  |  |  |  |  | 181 |
| 13 | `FilteredTextBoxExtenderqty` | cc1:FilteredTextBoxExtender | Mobile No: |  |  | Enabled=True |  |  | 182 |
| 14 | `faxNoTextBox` | asp:TextBox | Fax-No: |  |  |  |  |  | 198 |
| 15 | `FilteredTextBoxExtender1` | cc1:FilteredTextBoxExtender | Fax-No: |  |  | Enabled=True |  |  | 199 |
| 16 | `ddlCashAccount` | asp:DropDownList | Cash-GL Account: |  |  |  |  |  | 216 |
| 17 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click1 | 252 |
| 18 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 255 |
| 19 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 299 |
| 20 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 302 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDown` | `aCompanyUnitBll.LoadCompanyDropdownList(...)` | `ASMInfoBll.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ASMInfoBll.cs` | `ASMInfoBll.LoadCompanyDropdownList` → `ASMInfoDal.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `CreditLimitDAL.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustPaymentDAL.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `DeliveryPersonDAL.GetCompanyDropdownList` | SELECT | `tblCompanyInfo` | — |
| `LoadDropDown` | `aCompanyUnitBll.LoadDcType(...)` | `DistributionCenterInfoDal.LoadDcType(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` | `DistributionCenterInfoDal.LoadDcType` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DistributionCeneterInfoBll.LoadDcType` | SELECT | `tblDistributionType` | — |
| `submitButton_Click1` | `aCompanyUnitBll.SaveSalesCenter(...)` | `DistributionCeneterInfoBll.SaveSalesCenter(CompanyUnit companyUnit)` <br/>`Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` | `DistributionCeneterInfoBll.SaveSalesCenter` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `DistributionCenterInfoDal.CheckCodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `DistributionCenterInfoDal.SaveDataForSalesCenter` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `DistributionCeneterInfoBll.SalesCodeGenerator` | SELECT | `tblCompanyUnit` | — |
| `salesCenternameTextBox_OnTextChanged` | `aCompanyUnitBll.CheckDCInfoExistOrNot(...)` | `DistributionCeneterInfoBll.CheckDCInfoExistOrNot(string dcName)` <br/>`Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` | `DistributionCeneterInfoBll.CheckDCInfoExistOrNot` → `DistributionCenterInfoDal.DCInfoExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit` | — |
| `dcShortNameTextBox_OnTextChanged` | `aCompanyUnitBll.CheckDCShortNameExistOrNot(...)` | `DistributionCeneterInfoBll.CheckDCShortNameExistOrNot(string shortName)` <br/>`Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` | `DistributionCeneterInfoBll.CheckDCShortNameExistOrNot` → `DistributionCenterInfoDal.DCShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit` | — |
| `companyNameDropDownList_OnSelectedIndexChanged` | `aCompanyUnitBll.LoadCashAccount(...)` | `CustPaymentDAL.LoadCashAccount(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | `CustPaymentDAL.LoadCashAccount` → `DistributionCenterInfoDal.LoadCashAccount` → `CustPaymentBLL.LoadCashAccount` → `CustPaymentDAL.LoadCashAccount` → `ClsCommonInternalDAL.LoadDropDownValue` → `DistributionCeneterInfoBll.LoadCashAccount` | SELECT | — | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | companyNameDropDownList_OnSelectedIndexChanged | `aCompanyUnitBll.LoadCashAccount()` in `companyNameDropDownList_OnSelectedIndexChanged()`; `aCompanyUnitBll.LoadCompanyDropdownList()` in `LoadDropDown()` | `tblCompanyInfo` | set in code (see the DAL method above) |
| `ddlDcType` | Distribution Type: | asp:DropDownList | False | — | `aCompanyUnitBll.LoadDcType()` in `LoadDropDown()` | `tblDistributionType` | set in code (see the DAL method above) |
| `ddlCashAccount` | Cash-GL Account: | asp:DropDownList | False | — | `aCompanyUnitBll.LoadCashAccount()` in `companyNameDropDownList_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |

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
| `unitViewImageButton` | asp:ImageButton |  | — | `unitViewImageButton_Click` | 3 lines | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click1` | 35 lines | `aCompanyUnitBll.SaveSalesCenter` | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:FilteredTextBoxExtender`(phoneNoTextBox), `cc1:FilteredTextBoxExtender`(mobileNoTextBox), `cc1:FilteredTextBoxExtender`(faxNoTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | — | — | — | — |
| 2 | `LoadDropDown` | `void LoadDropDown()` | 5 | `aCompanyUnitBll.LoadCompanyDropdownList`, `aCompanyUnitBll.LoadDcType` | — | — | — |
| 3 | `Clear` | `void Clear()` | 10 | — | — | — | — |
| 4 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 5 | `Validation` | `bool Validation()` | 28 | — | — | — | — |
| 6 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 35 | `aCompanyUnitBll.SaveSalesCenter` | — | — | `LoginName` |
| 7 | `unitViewImageButton_Click` | `void unitViewImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 8 | `salesCenternameTextBox_OnTextChanged` | `void salesCenternameTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `aCompanyUnitBll.CheckDCInfoExistOrNot` | — | — | — |
| 9 | `GotoPermissionSetUp` | `void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)` | 7 | — | — | — | — |
| 10 | `dcShortNameTextBox_OnTextChanged` | `void dcShortNameTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `aCompanyUnitBll.CheckDCShortNameExistOrNot` | — | — | — |
| 11 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 12 | `companyNameDropDownList_OnSelectedIndexChanged` | `void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 8 | `aCompanyUnitBll.LoadCashAccount` | — | — | — |

Call chain shape for every persisting action on this page:

```
DistributionCenterInformation.aspx (markup)
  -> SInventory_UI_DistributionCenterInformation (code-behind event handler)
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
| `LoadDropDown` | `ASMInfoBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| `LoadDropDown` | `CustPaymentBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `LoadDropDown` | `DcWiseAreaAloBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| `LoadDropDown` | `DeliveryPersonBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `LoadDropDown` | `DistributionCeneterInfoBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| `LoadDropDown` | `EmpGeneralInfoBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| `LoadDropDown` | `MIOSetupBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| `LoadDropDown` | `OtherStockActionBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `LoadDropDown` | `ProductBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadDropDown` | `RequisitionBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `LoadDropDown` | `RSMSetupBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| `LoadDropDown` | `SalesAdministrationReportBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| `LoadDropDown` | `SlaveinformationBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| `LoadDropDown` | `StockConditionFreezeBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `LoadDropDown` | `WarehouseStockInBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `LoadDropDown` | `WhFreezeStockReleaseBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| `LoadDropDown` | `WhStockConditionFreezeBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| `LoadDropDown` | `WingsSalesAndCollectionReportBll` | `LoadCompanyDropdownList` | `DropDownList ddl, string groupId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| `LoadDropDown` | `DistributionCeneterInfoBll` | `LoadDcType` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| `submitButton_Click1` | `DistributionCeneterInfoBll` | `SaveSalesCenter` | `CompanyUnit companyUnit` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| `salesCenternameTextBox_OnTextChanged` | `DistributionCeneterInfoBll` | `CheckDCInfoExistOrNot` | `string dcName` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| `dcShortNameTextBox_OnTextChanged` | `DistributionCeneterInfoBll` | `CheckDCShortNameExistOrNot` | `string shortName` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `CustPaymentBLL` | `LoadCashAccount` | `DropDownList ddlCash, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `DistributionCeneterInfoBll` | `LoadCashAccount` | `DropDownList ddlCashAccount, int companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropDown` | `DistributionCenterInfoDal` | `LoadDcType` | `DropDownList ddl` | `void` | SELECT | `tblDistributionType` | — | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `CustPaymentDAL` | `LoadCashAccount` | `DropDownList ddl, string companyId` | `void` | SELECT | — | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `DistributionCenterInfoDal` | `LoadCashAccount` | `DropDownList ddl, int companyId` | `void` | SELECT | — | — | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadDropDown()` → `ASMInfoBll.LoadCompanyDropdownList` → `ASMInfoDal.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `CreditLimitDAL.GetCompanyDropdownList` |
| `tblCompanyUnit` | `dcShortNameTextBox_OnTextChanged()` → `DistributionCeneterInfoBll.CheckDCShortNameExistOrNot` → `DistributionCenterInfoDal.DCShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `salesCenternameTextBox_OnTextChanged()` → `DistributionCeneterInfoBll.CheckDCInfoExistOrNot` → `DistributionCenterInfoDal.DCInfoExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `DistributionCeneterInfoBll.SaveSalesCenter` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `DistributionCenterInfoDal.CheckCodeExistOrNot` |
| `tblDistributionType` | `LoadDropDown()` → `DistributionCenterInfoDal.LoadDcType` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DistributionCeneterInfoBll.LoadDcType` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtender2` | `phoneNoTextBox` | Custom | 0123456789 |
| `FilteredTextBoxExtenderqty` | `mobileNoTextBox` | Custom | 0123456789+ |
| `FilteredTextBoxExtender1` | `faxNoTextBox` | Custom | 0123456789+ |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-028-001` | {0} | Code | `Solution.Web/SInventory_UI/DistributionCenterInformation.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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

Audit columns written by this page: `EntryBy`

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
| Assembly source | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblDistributionType` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-028-001` | Unauthenticated access | Request `/SInventory_UI/DistributionCenterInformation.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-028-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DistributionCenterInformation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DistributionCenterInformation.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 20 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 12 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 3 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/ASMInfoBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` is **shared with 5 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `DeliveryPersonEntry.aspx`, `DeliveryPersonView.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` is **shared with 6 other page(s)** (e.g. `AuditReportOne.aspx`, `DcWiseAreaAlocation.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`, `EmpGeneralInfoEntry.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` is **shared with 4 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGenInfView.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/MIOSetupBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` is **shared with 22 other page(s)** (e.g. `AllSalesReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`, `BatchWiseTopSheet.aspx`, `CustomerPayment.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ProductBLL.cs` is **shared with 8 other page(s)** (e.g. `CreatePickingDetailForWareHouse.aspx`, `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `IssueRequisitionProducts.aspx`, `ProductEdit.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RSMSetupBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RequisitionBLL.cs` is **shared with 29 other page(s)** (e.g. `Dashboard.aspx`, `B2BReceiveViewer.aspx`, `StockReceiveReportViewer.aspx`, `TransferStockReceiveReportViewer.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` is **shared with 16 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `CustomerPaymentReport.aspx`, `CustomerReturnByMIOReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` is **shared with 9 other page(s)** (e.g. `AllSalesReport.aspx`, `DcWiseAreaAlocation.aspx`, `DeliveryReturnReport.aspx`, `EmpGeneralInfoEntry.aspx`, `SCPickingGenerate.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` is **shared with 11 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `MaterialConversionReceive.aspx`, `MaterialConversionReceiveView.aspx`, `NonTranscationalInvoiceReturn.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` is **shared with 7 other page(s)** (e.g. `AuditReportOne.aspx`, `CustomerCreditView.aspx`, `CustomerReport.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblDistributionType` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
