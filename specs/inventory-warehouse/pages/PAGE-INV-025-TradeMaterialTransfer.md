# INV-025 — TradeMaterialTransfer

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-025` |
| Module | `inventory-warehouse` |
| File name | `TradeMaterialTransfer.aspx` |
| Route | `/SInventory_UI/TradeMaterialTransfer.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `TradeMaterialTransfer.aspx.cs` |
| Code-behind class | `SInventory_UI_TradeMaterialTransfer` |
| Markup lines | 348 |
| Code-behind lines | 432 |
| Server controls | 21 |
| GridViews | 0 |
| Code-behind methods | 24 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/TradeMaterialTransferList.aspx`

Outbound `Response.Redirect` targets: `Login.aspx`, `TradeMaterialTransferList.aspx`

## 5. Route

`/SInventory_UI/TradeMaterialTransfer.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js` |
| Business / data layer | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |

## 7. UI Control Inventory

21 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 205 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 206 |
| 4 | `listButton` | asp:Button | Manage Stock Transfers | Transfer List |  |  |  | listButton_Click | 220 |
| 5 | `tradeMaterialTransferIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 225 |
| 6 | `rmIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 226 |
| 7 | `fromBusinessUnitDropDownList` | asp:DropDownList | Business Unit |  | True |  |  | fromBusinessUnitDropDownList_SelectedIndexChanged | 234 |
| 8 | `fromLocationDropDownList` | asp:DropDownList | Location |  | True |  |  | fromLocationDropDownList_SelectedIndexChanged | 241 |
| 9 | `fgDropDownList` | asp:DropDownList | Finished Goods Name |  | True |  |  | fgDropDownList_SelectedIndexChanged | 248 |
| 10 | `fromCurrentStockQtyTextBox` | asp:TextBox | Current Stock Qty |  |  | ReadOnly=True |  |  | 255 |
| 11 | `transferQtyTextBox` | asp:TextBox | Transfer Qty |  |  |  |  |  | 260 |
| 12 | `FilteredTextBoxExtenderQty` | cc1:FilteredTextBoxExtender | Transfer Qty |  |  | Enabled=True |  |  | 261 |
| 13 | `remarksTextBox` | asp:TextBox | Remarks |  |  |  |  |  | 268 |
| 14 | `toBusinessUnitDropDownList` | asp:DropDownList | Business Unit |  | True |  |  | toBusinessUnitDropDownList_SelectedIndexChanged | 278 |
| 15 | `toLocationDropDownList` | asp:DropDownList | Location |  | True |  |  | toLocationDropDownList_SelectedIndexChanged | 285 |
| 16 | `rmNameTextBox` | asp:TextBox | Raw Material Name |  | True |  |  | rmNameTextBox_TextChanged | 292 |
| 17 | `rmNameTextBox_AutoCompleteExtender` | cc1:AutoCompleteExtender | Raw Material Name |  |  | Enabled=True |  |  | 294 |
| 18 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 307 |
| 19 | `cancelButton` | asp:Button |  | Cancel |  |  |  | cancelButton_Click | 310 |
| 20 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 321 |
| 21 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 324 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `TradeMaterialTransferDao`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadBusinessUnits` | `aTradeMaterialTransferDal.LoadBusinessUnit(...)` | `TradeMaterialTransferDal.LoadBusinessUnit(DropDownList ddl, int userId, int moduleId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.LoadBusinessUnit` → `ClsCommonInternalDAL.LoadDropDownListData` | — | — | `sp_Common_GetUnitByPermissionByModule` |
| `LoadFromDependentData` | `aTradeMaterialTransferDal.LoadFgByCompany(...)` | `TradeMaterialTransferDal.LoadFgByCompany(DropDownList ddl, int companyId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.LoadFgByCompany` | SELECT | `tblProduct`, `tblUnitPrice` | — |
| `LoadFromDependentData` | `aTradeMaterialTransferDal.LoadLocation(...)` | `TradeMaterialTransferDal.LoadLocation(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.LoadLocation` → `ClsCommonInternalDAL.LoadDropDownListData` | — | — | `sp_Common_GetUnitWiseLocation_Zas` |
| `LoadToDependentData` | `aTradeMaterialTransferDal.LoadToLocation(...)` | `TradeMaterialTransferDal.LoadToLocation(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.LoadToLocation` → `ClsCommonInternalDAL.LoadDropDownListData` | — | — | `sp_Common_GetUnitWiseLocation_FromErp` |
| `LoadFgCurrentStock` | `aTradeMaterialTransferDal.GetFgCurrentStock(...)` | `TradeMaterialTransferDal.GetFgCurrentStock(int productId, int comUnitId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetFgCurrentStock` | SELECT | `tblDCStore`, `tblProduct` | — |
| `LoadRmCurrentStock` | `aTradeMaterialTransferDal.GetRawMaterialCurrentStock(...)` | `TradeMaterialTransferDal.GetRawMaterialCurrentStock(int companyId, int warehouseId, int itemDescriptionId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetRawMaterialCurrentStock` | SELECT | `tbl_INV_WeightedAvg` | — |
| `ResolveRawMaterial` | `aTradeMaterialTransferDal.GetRawMaterialInfo(...)` | `TradeMaterialTransferDal.GetRawMaterialInfo(string rawMaterialText)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetRawMaterialInfo` | SELECT | — | — |
| `LoadTradeMaterialTransferForEdit` | `aTradeMaterialTransferDal.GetTradeMaterialTransferById(...)` | `TradeMaterialTransferDal.GetTradeMaterialTransferById(int tradeMaterialTransferId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetTradeMaterialTransferById` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — |
| `submitButton_Click` | `aTradeMaterialTransferDal.GetFgInfo(...)` | `TradeMaterialTransferDal.GetFgInfo(int productId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetFgInfo` | SELECT | `tblProduct` | — |
| `submitButton_Click` | `aTradeMaterialTransferDal.GetRawMaterialInfo(...)` | `TradeMaterialTransferDal.GetRawMaterialInfo(string rawMaterialText)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetRawMaterialInfo` | SELECT | — | — |
| `submitButton_Click` | `aTradeMaterialTransferDal.GetTradeMaterialTransferById(...)` | `TradeMaterialTransferDal.GetTradeMaterialTransferById(int tradeMaterialTransferId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetTradeMaterialTransferById` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — |
| `submitButton_Click` | `aTradeMaterialTransferDal.SaveTradeMaterialTransfer(...)` | `TradeMaterialTransferDal.SaveTradeMaterialTransfer(TradeMaterialTransferDao aTradeMaterialTransferDao)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.SaveTradeMaterialTransfer` → `TradeMaterialTransferDal.InsertTradeMaterialTransfer` → `TradeMaterialTransferDal.UpdateTradeMaterialTransfer` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `spInsertTradeMaterialTransfer`, `spUpdateTradeMaterialTransfer` |
| `GetRawMaterial` | `aTradeMaterialTransferDal.SearchRawMaterial(...)` | `TradeMaterialTransferDal.SearchRawMaterial(string prefixText)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.SearchRawMaterial` | SELECT | — | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `fromBusinessUnitDropDownList` | Business Unit | asp:DropDownList | True | fromBusinessUnitDropDownList_SelectedIndexChanged | `aTradeMaterialTransferDal.LoadBusinessUnit()` in `LoadBusinessUnits()`; `aTradeMaterialTransferDal.LoadFgByCompany()` in `LoadFromDependentData()`; `aTradeMaterialTransferDal.LoadLocation()` in `LoadFromDependentData()` | `tblProduct`, `tblUnitPrice` | set in code (see the DAL method above) |
| `fromLocationDropDownList` | Location | asp:DropDownList | True | fromLocationDropDownList_SelectedIndexChanged | `aTradeMaterialTransferDal.GetFgCurrentStock()` in `LoadFgCurrentStock()`; `aTradeMaterialTransferDal.LoadLocation()` in `LoadFromDependentData()` | `tblDCStore`, `tblProduct` | set in code (see the DAL method above) |
| `fgDropDownList` | Finished Goods Name | asp:DropDownList | True | fgDropDownList_SelectedIndexChanged | `aTradeMaterialTransferDal.GetFgCurrentStock()` in `LoadFgCurrentStock()`; `aTradeMaterialTransferDal.GetFgInfo()` in `submitButton_Click()`; `aTradeMaterialTransferDal.LoadFgByCompany()` in `LoadFromDependentData()` | `tblDCStore`, `tblProduct`, `tblUnitPrice` | set in code (see the DAL method above) |
| `toBusinessUnitDropDownList` | Business Unit | asp:DropDownList | True | toBusinessUnitDropDownList_SelectedIndexChanged | `aTradeMaterialTransferDal.GetRawMaterialCurrentStock()` in `LoadRmCurrentStock()`; `aTradeMaterialTransferDal.LoadBusinessUnit()` in `LoadBusinessUnits()`; `aTradeMaterialTransferDal.LoadToLocation()` in `LoadToDependentData()` | `tbl_INV_WeightedAvg` | set in code (see the DAL method above) |
| `toLocationDropDownList` | Location | asp:DropDownList | True | toLocationDropDownList_SelectedIndexChanged | `aTradeMaterialTransferDal.GetRawMaterialCurrentStock()` in `LoadRmCurrentStock()`; `aTradeMaterialTransferDal.LoadToLocation()` in `LoadToDependentData()` | `tbl_INV_WeightedAvg` | set in code (see the DAL method above) |

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
| `listButton` | asp:Button | Transfer List | — | `listButton_Click` | 3 lines | — | — |
| `submitButton` | asp:Button | Submit | return confirm('Are you sure you want to Save ?'); | `submitButton_Click` | 77 lines | `aTradeMaterialTransferDal.GetFgInfo`, `aTradeMaterialTransferDal.GetRawMaterialInfo`, `aTradeMaterialTransferDal.GetTradeMaterialTransferById`, `aTradeMaterialTransferDal.SaveTradeMaterialTransfer` | — |
| `cancelButton` | asp:Button | Cancel | — | `cancelButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:FilteredTextBoxExtender`(transferQtyTextBox), `cc1:AutoCompleteExtender`(rmNameTextBox)
- Inline JavaScript functions declared in the markup: `initTmtSelect2()`
- External scripts: `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 8 | — | — | — | — |
| 2 | `LoadBusinessUnits` | `void LoadBusinessUnits()` | 25 | `aTradeMaterialTransferDal.LoadBusinessUnit` | — | — | `UserId` |
| 3 | `LoadFromDependentData` | `void LoadFromDependentData()` | 11 | `aTradeMaterialTransferDal.LoadFgByCompany`, `aTradeMaterialTransferDal.LoadLocation` | — | — | — |
| 4 | `LoadToDependentData` | `void LoadToDependentData()` | 4 | `aTradeMaterialTransferDal.LoadToLocation` | — | — | — |
| 5 | `LoadFgCurrentStock` | `void LoadFgCurrentStock()` | 17 | `aTradeMaterialTransferDal.GetFgCurrentStock` | — | — | — |
| 6 | `LoadRmCurrentStock` | `void LoadRmCurrentStock()` | 9 | `aTradeMaterialTransferDal.GetRawMaterialCurrentStock` | — | — | — |
| 7 | `ResolveRawMaterial` | `void ResolveRawMaterial()` | 30 | `aTradeMaterialTransferDal.GetRawMaterialInfo` | — | — | — |
| 8 | `Validation` | `bool Validation()` | 54 | — | — | — | — |
| 9 | `ShowValidationMessage` | `void ShowValidationMessage(WebControl control, string message)` | 5 | — | — | — | — |
| 10 | `ClearTransactionFields` | `void ClearTransactionFields()` | 8 | — | — | — | — |
| 11 | `ClearAll` | `void ClearAll()` | 21 | — | — | — | — |
| 12 | `SetSelectedValueIfExists` | `void SetSelectedValueIfExists(ListControl listControl, string selectedValue)` | 6 | — | — | — | — |
| 13 | `LoadTradeMaterialTransferForEdit` | `void LoadTradeMaterialTransferForEdit()` | 56 | `aTradeMaterialTransferDal.GetTradeMaterialTransferById` | — | — | — |
| 14 | `fromBusinessUnitDropDownList_SelectedIndexChanged` | `void fromBusinessUnitDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 15 | `toBusinessUnitDropDownList_SelectedIndexChanged` | `void toBusinessUnitDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 16 | `fromLocationDropDownList_SelectedIndexChanged` | `void fromLocationDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 17 | `toLocationDropDownList_SelectedIndexChanged` | `void toLocationDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 18 | `fgDropDownList_SelectedIndexChanged` | `void fgDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 19 | `rmNameTextBox_TextChanged` | `void rmNameTextBox_TextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 20 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 77 | `aTradeMaterialTransferDal.GetFgInfo`, `aTradeMaterialTransferDal.GetRawMaterialInfo`, `aTradeMaterialTransferDal.GetTradeMaterialTransferById`, `aTradeMaterialTransferDal.SaveTradeMaterialTransfer` | — | — | `LoginName` |
| 21 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 22 | `listButton_Click` | `void listButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 23 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 24 | `GetRawMaterial` | `string[] GetRawMaterial(string prefixText)` | 4 | `aTradeMaterialTransferDal.SearchRawMaterial` | — | — | — |

Call chain shape for every persisting action on this page:

```
TradeMaterialTransfer.aspx (markup)
  -> SInventory_UI_TradeMaterialTransfer (code-behind event handler)
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
| POST | `/SInventory_UI/TradeMaterialTransfer.aspx/GetRawMaterial` | `GetRawMaterial` | `string[]` |

AutoComplete extenders bound to the SOAP web service `Solution.Web/SInventory_UI/SInventoryWebService.asmx` (implementation `Solution.Web/App_Code/SInventoryWebService.cs`):

| Extender | TargetControlID | ServiceMethod | ServicePath | MinimumPrefixLength |
|---|---|---|---|---|
| `rmNameTextBox_AutoCompleteExtender` | `rmNameTextBox` | `GetRawMaterial` | `` | 1 |


## 14. Service (BLL) Mapping

_No BLL method resolved for this page._

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadBusinessUnits` | `TradeMaterialTransferDal` | `LoadBusinessUnit` | `DropDownList ddl, int userId, int moduleId` | `void` | — | — | `sp_Common_GetUnitByPermissionByModule` | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `LoadFromDependentData` | `TradeMaterialTransferDal` | `LoadFgByCompany` | `DropDownList ddl, int companyId` | `void` | SELECT | `tblProduct`, `tblUnitPrice` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `LoadFromDependentData` | `TradeMaterialTransferDal` | `LoadLocation` | `DropDownList ddl, string companyId` | `void` | — | — | `sp_Common_GetUnitWiseLocation_Zas` | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `LoadToDependentData` | `TradeMaterialTransferDal` | `LoadToLocation` | `DropDownList ddl, string companyId` | `void` | — | — | `sp_Common_GetUnitWiseLocation_FromErp` | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `LoadFgCurrentStock` | `TradeMaterialTransferDal` | `GetFgCurrentStock` | `int productId, int comUnitId` | `DataTable` | SELECT | `tblDCStore`, `tblProduct` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `LoadRmCurrentStock` | `TradeMaterialTransferDal` | `GetRawMaterialCurrentStock` | `int companyId, int warehouseId, int itemDescriptionId` | `DataTable` | SELECT | `tbl_INV_WeightedAvg` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `ResolveRawMaterial` | `TradeMaterialTransferDal` | `GetRawMaterialInfo` | `string rawMaterialText` | `DataTable` | SELECT | — | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `LoadTradeMaterialTransferForEdit` | `TradeMaterialTransferDal` | `GetTradeMaterialTransferById` | `int tradeMaterialTransferId` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `submitButton_Click` | `TradeMaterialTransferDal` | `GetFgInfo` | `int productId` | `DataTable` | SELECT | `tblProduct` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `submitButton_Click` | `TradeMaterialTransferDal` | `GetRawMaterialInfo` | `string rawMaterialText` | `DataTable` | SELECT | — | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `submitButton_Click` | `TradeMaterialTransferDal` | `GetTradeMaterialTransferById` | `int tradeMaterialTransferId` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `submitButton_Click` | `TradeMaterialTransferDal` | `SaveTradeMaterialTransfer` | `TradeMaterialTransferDao aTradeMaterialTransferDao` | `string` | — | — | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `GetRawMaterial` | `TradeMaterialTransferDal` | `SearchRawMaterial` | `string prefixText` | `string[]` | SELECT | — | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Common_GetUnitByPermissionByModule` | LoadBusinessUnits → `TradeMaterialTransferDal.LoadBusinessUnit → ClsCommonInternalDAL.LoadDropDownListData` | **No** — see note |
| `sp_Common_GetUnitWiseLocation_Zas` | LoadFromDependentData → `TradeMaterialTransferDal.LoadLocation → ClsCommonInternalDAL.LoadDropDownListData` | **No** — see note |
| `sp_Common_GetUnitWiseLocation_FromErp` | LoadToDependentData → `TradeMaterialTransferDal.LoadToLocation → ClsCommonInternalDAL.LoadDropDownListData` | **No** — see note |
| `spInsertTradeMaterialTransfer` | submitButton_Click → `TradeMaterialTransferDal.SaveTradeMaterialTransfer → TradeMaterialTransferDal.InsertTradeMaterialTransfer → TradeMaterialTransferDal.UpdateTradeMaterialTransfer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction` | **No** — see note |
| `spUpdateTradeMaterialTransfer` | submitButton_Click → `TradeMaterialTransferDal.SaveTradeMaterialTransfer → TradeMaterialTransferDal.InsertTradeMaterialTransfer → TradeMaterialTransferDal.UpdateTradeMaterialTransfer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadTradeMaterialTransferForEdit()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById`, `submitButton_Click()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById` |
| `tblCompanyUnit` | `LoadTradeMaterialTransferForEdit()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById`, `submitButton_Click()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById` |
| `tblDCStore` | `LoadFgCurrentStock()` → `TradeMaterialTransferDal.GetFgCurrentStock` |
| `tblProduct` | `LoadFgCurrentStock()` → `TradeMaterialTransferDal.GetFgCurrentStock`, `LoadFromDependentData()` → `TradeMaterialTransferDal.LoadFgByCompany`, `submitButton_Click()` → `TradeMaterialTransferDal.GetFgInfo` |
| `tblTradeMaterialTransfer` | `LoadTradeMaterialTransferForEdit()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById`, `submitButton_Click()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById` |
| `tblUnitPrice` | `LoadFromDependentData()` → `TradeMaterialTransferDal.LoadFgByCompany` |
| `tbl_INV_WeightedAvg` | `LoadRmCurrentStock()` → `TradeMaterialTransferDal.GetRawMaterialCurrentStock` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ClearAll` | Submit | No | Yes | UNKNOWN | Notification |
| `LoadTradeMaterialTransferForEdit` | Update | No | Yes | UNKNOWN | Notification |
| `submitButton_Click` | " + msg.Replace(" | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtenderQty` | `transferQtyTextBox` | Custom | 0123456789. |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-INV-025-001` | Submit | Code | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx.cs` → `ClearAll()` | user action on this page | operation is refused | "Submit" | CONFIRMED |
| `BR-INV-025-002` | Update | Code | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx.cs` → `LoadTradeMaterialTransferForEdit()` | user action on this page | operation is refused | "Update" | CONFIRMED |
| `BR-INV-025-003` | " + msg.Replace(" | Code | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx.cs` → `submitButton_Click()` | user action on this page | operation is refused | "" + msg.Replace("" | CONFIRMED |
| `BR-INV-025-004` | {0} | Code | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `UserId`

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
| Assembly source | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblDCStore` |
| Database object | `tblProduct` |
| Database object | `tblTradeMaterialTransfer` |
| Database object | `tblUnitPrice` |
| Database object | `tbl_INV_WeightedAvg` |
| Stored procedure | `sp_Common_GetUnitByPermissionByModule` |
| Stored procedure | `sp_Common_GetUnitWiseLocation_Zas` |
| Stored procedure | `sp_Common_GetUnitWiseLocation_FromErp` |
| Stored procedure | `spInsertTradeMaterialTransfer` |
| Stored procedure | `spUpdateTradeMaterialTransfer` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-025-001` | Unauthenticated access | Request `/SInventory_UI/TradeMaterialTransfer.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-025-002` | Validation: Submit | Trigger `ClearAll()` in the state that produces this message | alert "Submit" and the operation is not performed | CONFIRMED |
| `TC-INV-025-003` | Validation: Update | Trigger `LoadTradeMaterialTransferForEdit()` in the state that produces this message | alert "Update" and the operation is not performed | CONFIRMED |
| `TC-INV-025-004` | Validation: " + msg.Replace(" | Trigger `submitButton_Click()` in the state that produces this message | alert "" + msg.Replace("" and the operation is not performed | CONFIRMED |
| `TC-INV-025-005` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx.cs` |
| Layer source | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 21 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 24 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | 1 WebMethods | n/a | **MISSING → now documented** | section 13 |
| Stored procedures | absent | 5 names found | bodies **not in repo** | **PARTIAL** | section 16 |
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

- `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` is **shared with 2 other page(s)** (e.g. `TradeMaterialTransferApproval.aspx`, `TradeMaterialTransferList.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblTradeMaterialTransfer` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.
- Writes/reads `tbl_INV_WeightedAvg` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
