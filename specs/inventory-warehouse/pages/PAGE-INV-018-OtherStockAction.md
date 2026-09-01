# INV-018 — OtherStockAction

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-018` |
| Module | `inventory-warehouse` |
| File name | `OtherStockAction.aspx` |
| Route | `/SInventory_UI/OtherStockAction.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `OtherStockAction.aspx.cs` |
| Code-behind class | `SInventory_UI_OtherStockAction` |
| Markup lines | 310 |
| Code-behind lines | 365 |
| Server controls | 31 |
| GridViews | 1 |
| Code-behind methods | 14 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

Outbound `Response.Redirect` targets: `Login.aspx`

## 5. Route

`/SInventory_UI/OtherStockAction.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/OtherStockAction.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/OtherStockAction.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |

## 7. UI Control Inventory

31 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 7 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `dcDropDownList1` | asp:DropDownList | Distribution Center : |  | True |  |  | dcDropDownList_SelectedIndexChanged | 46 |
| 5 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_OnSelectedIndexChanged | 66 |
| 6 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  |  |  |  |  | 85 |
| 7 | `stockInIdHiddenField` | asp:HiddenField | Distribution Center: |  |  |  |  |  | 88 |
| 8 | `manufacturerDropDownList` | asp:DropDownList | Manufacturer: |  | True |  |  | manufacturerDropDownList_SelectedIndexChanged | 106 |
| 9 | `submitButton` | asp:Button |  | Search |  |  |  | submitButton_Click | 142 |
| 10 | `submitButton0` | asp:Button |  | Submit |  |  |  | submitButton0_OnClick | 146 |
| 11 | `loadGridView` | asp:GridView |  |  |  |  |  | loadGridView_RowCommand | 181 |
| 12 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 184 |
| 13 | `LabelSL` | asp:Label | loadGridView → col[0] "#SL" |  |  |  |  |  | 186 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 189 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 190 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 191 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 192 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 193 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 194 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 195 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 196 |
| 22 | `returnQtyTextBox` | asp:TextBox | loadGridView → col[8] "Return Qty" |  |  |  |  |  | 198 |
| 23 | `FilteredTextBoxExtenderconvRate` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 199 |
| 24 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 204 |
| 25 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 205 |
| 26 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 206 |
| 27 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 207 |
| 28 | `chkSelectAll` | asp:CheckBox | loadGridView → col[12] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 209 |
| 29 | `chkSelect` | asp:CheckBox | loadGridView → col[12] "(no header)" |  | True |  |  |  | 212 |
| 30 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 301 |
| 31 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 304 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `DCStoreFreezeId,DCStoreId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | #SL | — | — | `Label:LabelSL` | — |
| 1 | BoundField | ProductCode | ProductCode | — | — | — |
| 2 | BoundField | Product Name | ProductName | — | — | — |
| 3 | BoundField | Batch | BatchNo | — | — | — |
| 4 | BoundField | ExpDate | ExpDate | — | — | — |
| 5 | BoundField | StockQty | StockQty | — | — | — |
| 6 | BoundField | Amount | Amount | — | — | — |
| 7 | BoundField | StockCondition | StockCondition | — | — | — |
| 8 | TemplateField | Return Qty | — | — | `TextBox:returnQtyTextBox` | — |
| 9 | BoundField | Invoice No | InvoiceNo | — | — | — |
| 10 | BoundField | Delivary Invoice No | DelivaryInvoiceNo | — | — | — |
| 11 | BoundField | Remarks | Remarks | — | — | — |
| 12 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDown` | `aOtherStockActionBLL.DCLoad(...)` | `OtherStockActionBLL.DCLoad(DropDownList aDownList, string userId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `LoadDropDown` | `aOtherStockActionBLL.LoadCompanyDropdownList(...)` | `OtherStockActionBLL.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadCompanyDropdownList` | — | — | — |
| `LoadDropDown` | `aOtherStockActionBLL.LoadmanufacturerName(...)` | `OtherStockActionBLL.LoadmanufacturerName(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadmanufacturerName` → `OtherStockActionDAL.LoadmanufacturerName` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyInfo` | — |
| `companyNameDropDownList_OnSelectedIndexChanged` | `aOtherStockActionBLL.LoadDcInformationByUser(...)` | `OtherStockActionBLL.LoadDcInformationByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `companyNameDropDownList_OnSelectedIndexChanged` | `aOtherStockActionBLL.LoadWhInformation(...)` | `OtherStockActionBLL.LoadWhInformation(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |
| `Load` | `aOtherStockActionBLL.LoadData(...)` | `OtherStockActionBLL.LoadData(int DCID,int ManuID)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStoreFreeze`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct`, `tblUnitPrice` | — |
| `Load2` | `aOtherStockActionBLL.LoadData(...)` | `OtherStockActionBLL.LoadData(int DCID,int ManuID)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStoreFreeze`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct`, `tblUnitPrice` | — |
| `loadGridView_RowCommand` | `aOtherStockActionBLL.LoadStockDCStockData(...)` | `OtherStockActionBLL.LoadStockDCStockData(int DCStoreId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadStockDCStockData` → `OtherStockActionDAL.LoadStockDCStockData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore` | — |
| `loadGridView_RowCommand` | `aOtherStockActionBLL.LoadStockData(...)` | `OtherStockActionBLL.LoadStockData(int DCStoreFreezeId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadStockData` → `OtherStockActionDAL.LoadStockData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStoreFreeze` | — |
| `loadGridView_RowCommand` | `aOtherStockActionBLL.UpdateDCStock(...)` | `OtherStockActionBLL.UpdateDCStock(decimal StockQty, int DCStoreId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.UpdateDCStock` → `OtherStockActionDAL.UpdateDCStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDCStore` | — |
| `loadGridView_RowCommand` | `aOtherStockActionBLL.UpdateDCStoreFreezeStock(...)` | `OtherStockActionBLL.UpdateDCStoreFreezeStock(decimal StockQty, int DCStoreFreezeId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.UpdateDCStoreFreezeStock` → `OtherStockActionDAL.UpdateDCStoreFreezeStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDCStoreFreeze` | — |
| `submitButton0_OnClick` | `aOtherStockActionBLL.CheckPermission(...)` | `OtherStockActionBLL.CheckPermission(string condition, string userId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.CheckPermission` → `OtherStockActionDAL.GetCheckPermission` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblStockCondition`, `tblStockConditionPermission` | — |
| `submitButton0_OnClick` | `aOtherStockActionBLL.FreezeStockReleasePosting(...)` | `OtherStockActionBLL.FreezeStockReleasePosting(int dcStoreFreezeId, decimal returnQty)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.FreezeStockReleasePosting` → `OtherStockActionDAL.FreezeStockReleasePosting` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_FreezeStockReleasePosting` |
| `submitButton0_OnClick` | `aOtherStockActionBLL.LoadStockDCStockData(...)` | `OtherStockActionBLL.LoadStockDCStockData(int DCStoreId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadStockDCStockData` → `OtherStockActionDAL.LoadStockDCStockData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore` | — |
| `submitButton0_OnClick` | `aOtherStockActionBLL.LoadStockData(...)` | `OtherStockActionBLL.LoadStockData(int DCStoreFreezeId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadStockData` → `OtherStockActionDAL.LoadStockData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStoreFreeze` | — |
| `submitButton0_OnClick` | `aOtherStockActionBLL.UpdateDCStock(...)` | `OtherStockActionBLL.UpdateDCStock(decimal StockQty, int DCStoreId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.UpdateDCStock` → `OtherStockActionDAL.UpdateDCStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDCStore` | — |
| `submitButton0_OnClick` | `aOtherStockActionBLL.UpdateDCStoreFreezeStock(...)` | `OtherStockActionBLL.UpdateDCStoreFreezeStock(decimal StockQty, int DCStoreFreezeId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.UpdateDCStoreFreezeStock` → `OtherStockActionDAL.UpdateDCStoreFreezeStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDCStoreFreeze` | — |
| `Validation` | `aInvoiceBll.GetFinancialYearInfo(...)` | `InvoiceBLL.GetFinancialYearInfo(string invoiceDate,string unitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `dcDropDownList1` | Distribution Center : | asp:DropDownList | True | dcDropDownList_SelectedIndexChanged | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | companyNameDropDownList_OnSelectedIndexChanged | `aOtherStockActionBLL.LoadCompanyDropdownList()` in `LoadDropDown()`; `aOtherStockActionBLL.LoadData()` in `Load()`; `aOtherStockActionBLL.LoadData()` in `Load2()`; `aOtherStockActionBLL.LoadDcInformationByUser()` in `companyNameDropDownList_OnSelectedIndexChanged()` | `tblCompanyUnit`, `tblDCStoreFreeze`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct`, `tblUnitPrice`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center: | asp:DropDownList | False | — | `aInvoiceBll.GetFinancialYearInfo()` in `Validation()`; `aOtherStockActionBLL.DCLoad()` in `LoadDropDown()`; `aOtherStockActionBLL.LoadData()` in `Load()`; `aOtherStockActionBLL.LoadData()` in `Load2()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStoreFreeze`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct`, `tblUnitPrice`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `manufacturerDropDownList` | Manufacturer: | asp:DropDownList | True | manufacturerDropDownList_SelectedIndexChanged | `aOtherStockActionBLL.LoadmanufacturerName()` in `LoadDropDown()` | `tblCompanyInfo` | set in code (see the DAL method above) |

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
| `submitButton` | asp:Button | Search | — | `submitButton_Click` | 3 lines | — | — |
| `submitButton0` | asp:Button | Submit | — | `submitButton0_OnClick` | 139 lines | `aOtherStockActionBLL.CheckPermission`, `aOtherStockActionBLL.FreezeStockReleasePosting`, `aOtherStockActionBLL.LoadStockDCStockData`, `aOtherStockActionBLL.LoadStockData`, `aOtherStockActionBLL.UpdateDCStock` | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:FilteredTextBoxExtender`(returnQtyTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | — | — | — | — |
| 2 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 3 | `LoadDropDown` | `void LoadDropDown()` | 6 | `aOtherStockActionBLL.DCLoad`, `aOtherStockActionBLL.LoadCompanyDropdownList`, `aOtherStockActionBLL.LoadmanufacturerName` | — | — | `UserId` |
| 4 | `companyNameDropDownList_OnSelectedIndexChanged` | `void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 25 | `aOtherStockActionBLL.LoadDcInformationByUser`, `aOtherStockActionBLL.LoadWhInformation` | — | — | `UserId`, `UserType` |
| 5 | `Load` | `void Load()` | 13 | `aOtherStockActionBLL.LoadData` | — | — | — |
| 6 | `Load2` | `void Load2()` | 9 | `aOtherStockActionBLL.LoadData` | — | — | — |
| 7 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 8 | `refreshButton_Click` | `void refreshButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 9 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 10 | `loadGridView_RowCommand` | `void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 86 | `aOtherStockActionBLL.LoadStockDCStockData`, `aOtherStockActionBLL.LoadStockData`, `aOtherStockActionBLL.UpdateDCStock`, `aOtherStockActionBLL.UpdateDCStoreFreezeStock` | — | — | `LoginName` |
| 11 | `dcDropDownList_SelectedIndexChanged` | `void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 5 | — | — | — | — |
| 12 | `manufacturerDropDownList_SelectedIndexChanged` | `void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 13 | `submitButton0_OnClick` | `void submitButton0_OnClick(object sender, EventArgs e)` | 139 | `aOtherStockActionBLL.CheckPermission`, `aOtherStockActionBLL.FreezeStockReleasePosting`, `aOtherStockActionBLL.LoadStockDCStockData`, `aOtherStockActionBLL.LoadStockData`, `aOtherStockActionBLL.UpdateDCStock`, `aOtherStockActionBLL.UpdateDCStoreFreezeStock` | — | — | `LoginName`, `UserId` |
| 14 | `Validation` | `bool Validation()` | 12 | `aInvoiceBll.GetFinancialYearInfo` | — | — | — |

Call chain shape for every persisting action on this page:

```
OtherStockAction.aspx (markup)
  -> SInventory_UI_OtherStockAction (code-behind event handler)
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
| `LoadDropDown` | `OtherStockActionBLL` | `DCLoad` | `DropDownList aDownList, string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `LoadDropDown` | `OtherStockActionBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `LoadDropDown` | `OtherStockActionBLL` | `LoadmanufacturerName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `OtherStockActionBLL` | `LoadDcInformationByUser` | `DropDownList ddl, string companyId, string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `OtherStockActionBLL` | `LoadWhInformation` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `Load` | `OtherStockActionBLL` | `LoadData` | `int DCID,int ManuID` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `Load2` | `OtherStockActionBLL` | `LoadData` | `int DCID,int ManuID` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `loadGridView_RowCommand` | `OtherStockActionBLL` | `LoadStockDCStockData` | `int DCStoreId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `loadGridView_RowCommand` | `OtherStockActionBLL` | `LoadStockData` | `int DCStoreFreezeId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `loadGridView_RowCommand` | `OtherStockActionBLL` | `UpdateDCStock` | `decimal StockQty, int DCStoreId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `loadGridView_RowCommand` | `OtherStockActionBLL` | `UpdateDCStoreFreezeStock` | `decimal StockQty, int DCStoreFreezeId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `submitButton0_OnClick` | `OtherStockActionBLL` | `CheckPermission` | `string condition, string userId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `submitButton0_OnClick` | `OtherStockActionBLL` | `FreezeStockReleasePosting` | `int dcStoreFreezeId, decimal returnQty` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `submitButton0_OnClick` | `OtherStockActionBLL` | `LoadStockDCStockData` | `int DCStoreId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `submitButton0_OnClick` | `OtherStockActionBLL` | `LoadStockData` | `int DCStoreFreezeId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `submitButton0_OnClick` | `OtherStockActionBLL` | `UpdateDCStock` | `decimal StockQty, int DCStoreId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `submitButton0_OnClick` | `OtherStockActionBLL` | `UpdateDCStoreFreezeStock` | `decimal StockQty, int DCStoreFreezeId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `Validation` | `InvoiceBLL` | `GetFinancialYearInfo` | `string invoiceDate,string unitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_FreezeStockReleasePosting` | submitButton0_OnClick → `OtherStockActionBLL.FreezeStockReleasePosting → OtherStockActionDAL.FreezeStockReleasePosting → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadDropDown()` → `OtherStockActionBLL.LoadmanufacturerName` → `OtherStockActionDAL.LoadmanufacturerName` → `ClsCommonInternalDAL.LoadDropDownValue`, `Validation()` → `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `LoadDropDown()` → `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `Validation()` → `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `companyNameDropDownList_OnSelectedIndexChanged()` → `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `companyNameDropDownList_OnSelectedIndexChanged()` → `OtherStockActionBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblDCStore` | `loadGridView_RowCommand()` → `OtherStockActionBLL.LoadStockDCStockData` → `OtherStockActionDAL.LoadStockDCStockData` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `OtherStockActionBLL.UpdateDCStock` → `OtherStockActionDAL.UpdateDCStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `submitButton0_OnClick()` → `OtherStockActionBLL.LoadStockDCStockData` → `OtherStockActionDAL.LoadStockDCStockData` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton0_OnClick()` → `OtherStockActionBLL.UpdateDCStock` → `OtherStockActionDAL.UpdateDCStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblDCStoreFreeze` | `Load()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable`, `Load2()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `OtherStockActionBLL.LoadStockData` → `OtherStockActionDAL.LoadStockData` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `OtherStockActionBLL.UpdateDCStoreFreezeStock` → `OtherStockActionDAL.UpdateDCStoreFreezeStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `submitButton0_OnClick()` → `OtherStockActionBLL.LoadStockData` → `OtherStockActionDAL.LoadStockData` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton0_OnClick()` → `OtherStockActionBLL.UpdateDCStoreFreezeStock` → `OtherStockActionDAL.UpdateDCStoreFreezeStock` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblInvoice` | `Load()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable`, `Load2()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `Load()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable`, `Load2()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProduct` | `Load()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable`, `Load2()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblStockCondition` | `submitButton0_OnClick()` → `OtherStockActionBLL.CheckPermission` → `OtherStockActionDAL.GetCheckPermission` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblStockConditionPermission` | `submitButton0_OnClick()` → `OtherStockActionBLL.CheckPermission` → `OtherStockActionDAL.GetCheckPermission` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `Load()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable`, `Load2()` → `OtherStockActionBLL.LoadData` → `OtherStockActionDAL.LoadData` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUserCompanyUnit` | `LoadDropDown()` → `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `companyNameDropDownList_OnSelectedIndexChanged()` → `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtenderconvRate` | `returnQtyTextBox` | Custom | 0123456789. |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-INV-018-001` | {0} | Code | `Solution.Web/SInventory_UI/OtherStockAction.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `UserId`, `UserType`

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

- Accounting posting into `ZAS_ACCDB` via `sp_*Posting` / `sp_Sales_*` procedures.

See `specs/00-system/integrations.md` for the full list.

## 25. Reports

_This page opens no report._

## 26. Dependencies

| Kind | Dependency |
|---|---|
| Master page | `MasterPages/MainMasterPage.master` (auth guard + menu) |
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblDCStore` |
| Database object | `tblDCStoreFreeze` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblProduct` |
| Database object | `tblStockCondition` |
| Database object | `tblStockConditionPermission` |
| Database object | `tblUnitPrice` |
| Database object | `tblUserCompanyUnit` |
| Stored procedure | `sp_FreezeStockReleasePosting` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-018-001` | Unauthenticated access | Request `/SInventory_UI/OtherStockAction.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-018-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-INV-018-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/OtherStockAction.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/OtherStockAction.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 31 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 14 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 11 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` is **shared with 43 other page(s)** (e.g. `ProformaHTMLReport.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseDelReturnViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` is **shared with 22 other page(s)** (e.g. `AllSalesReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`, `BatchWiseTopSheet.aspx`, `CustomerPayment.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDCStoreFreeze` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblStockCondition` — any schema change to it breaks this page.
- Writes/reads `tblStockConditionPermission` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
