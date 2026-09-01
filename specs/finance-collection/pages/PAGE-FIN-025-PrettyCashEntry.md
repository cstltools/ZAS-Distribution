# FIN-025 — PrettyCashEntry

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `FIN-025` |
| Module | `finance-collection` |
| File name | `PrettyCashEntry.aspx` |
| Route | `/SInventory_UI/PrettyCashEntry.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `PrettyCashEntry.aspx.cs` |
| Code-behind class | `SInventory_UI_PrettyCashEntry` |
| Markup lines | 481 |
| Code-behind lines | 771 |
| Server controls | 32 |
| GridViews | 1 |
| Code-behind methods | 27 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/PrettyCashView.aspx`

## 5. Route

`/SInventory_UI/PrettyCashEntry.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `MasID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `../css/Date/jquery.datepick.js` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |

## 7. UI Control Inventory

32 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 55 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 74 |
| 4 | `custIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 89 |
| 5 | `MasterIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 90 |
| 6 | `ImageButton3` | asp:ImageButton | View List : | ~/images/viewList.png |  |  |  | ListImageButton_Click | 104 |
| 7 | `CompanyDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyDropDownList_OnSelectedIndexChanged | 125 |
| 8 | `CashAccDropDownlist` | asp:DropDownList | Cash Account: |  |  |  |  |  | 146 |
| 9 | `txtDate` | asp:TextBox | Posting Date : |  | true |  |  | dateOfBirthTextBox_OnTextChanged | 163 |
| 10 | `imgDatse` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 164 |
| 11 | `Date` | asp:CalendarExtender |  |  |  |  |  |  | 166 |
| 12 | `TextBalance` | asp:TextBox | Balance |  |  |  |  |  | 185 |
| 13 | `TextRemark` | asp:TextBox | Remark: |  |  |  |  |  | 203 |
| 14 | `itemGridView` | asp:GridView |  |  |  |  |  |  | 271 |
| 15 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 274 |
| 16 | `LabelSL` | asp:Label | itemGridView → col[0] "SL" |  |  |  |  |  | 276 |
| 17 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 281 |
| 18 | `txtPerticular` | asp:TextBox | itemGridView → col[1] "Perticular (COA of expense)" |  | True |  |  | PerticularTextBox_TextChanged | 283 |
| 19 | `productNameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 285 |
| 20 | `perticularhiddenId` | asp:HiddenField | itemGridView → col[1] "Perticular (COA of expense)" |  |  |  |  |  | 293 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 298 |
| 22 | `txtCostReason` | asp:TextBox | itemGridView → col[2] "Cost Reason" |  |  |  |  |  | 300 |
| 23 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 307 |
| 24 | `txtAmount` | asp:TextBox | itemGridView → col[3] "Amount" |  |  |  |  |  | 309 |
| 25 | `freqQtyTextBoxs` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 310 |
| 26 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 318 |
| 27 | `ImageButton1` | asp:ImageButton | itemGridView → col[4] "Add" | ~/images/lineAdd.png |  |  |  | itemaddImageButton_Click | 321 |
| 28 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 329 |
| 29 | `ImageButton2` | asp:ImageButton | itemGridView → col[5] "Remove" | ~/images/lineDelete.png |  |  |  | itemdeleteImageButton_Click | 331 |
| 30 | `itemdeleteImageButton` | asp:LinkButton | itemGridView → col[5] "Remove" |  |  |  |  | itemdeleteImageButton_Click | 333 |
| 31 | `btnSave` | asp:Button |  | Save |  |  |  | submitButton_Click1 | 380 |
| 32 | `Button2` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 382 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `itemGridView`

| Attribute | Value |
|---|---|
| datakeynames | `PerticulerId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | SL | — | — | `Label:LabelSL` | — |
| 1 | TemplateField | Perticular (COA of expense) | — | `ParticularName`, `PerticulerId` | `TextBox:txtPerticular`, `AutoCompleteExtender:txtPerticular`, `HiddenField:perticularhiddenId` | — |
| 2 | TemplateField | Cost Reason | — | `CashReason` | `TextBox:txtCostReason` | — |
| 3 | TemplateField | Amount | — | `Amount` | `TextBox:txtAmount`, `FilteredTextBoxExtender:txtAmount` | — |
| 4 | TemplateField | Add | — | — | `ImageButton:ImageButton1` | — |
| 5 | TemplateField | Remove | — | — | `ImageButton:ImageButton2`, `LinkButton:itemdeleteImageButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `PettyCashDetailsDao`, `PettyCashMasterDao`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `GetOneRecord` | `aCashBll.GetDataMasterByIdBll(...)` | `PettyCashBLL.GetDataMasterByIdBll(int id)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.GetDataMasterByIdBll` → `PettyCashDaL.GetMasterInfoByIdDal` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | — | — | `sp_GET_PretyCashMaster_ById` |
| `GetOneRecord` | `aCashBll.GetDetailsInfoByIdBll(...)` | `PettyCashBLL.GetDetailsInfoByIdBll(int id)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.GetDetailsInfoByIdBll` → `PettyCashDaL.GetDetailsInfoByIdDal` → `ClsCommonInternalDAL.GetDataTableAction` → `DB_Manager.GetDataTableAction` | — | — | `sp_GET_PretyCashDetails_ById` |
| `GetOneRecord` | `aCashBll.LoadCashAcc(...)` | `PettyCashBLL.LoadCashAcc(DropDownList ddl, string id)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.LoadCashAcc` → `PettyCashDaL.LoadCashAccDal` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | — | — |
| `LoaddropdownList` | `aCashBll.LoadCompany(...)` | `CompanywisebranchDal.LoadCompany(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` | `CompanywisebranchDal.LoadCompany` → `ClsCommonInternalDAL.LoadDropDownValue` → `InvoiceDAL.LoadCompany` → `MaterialConversionReceiveDal.LoadCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `InvoiceBLL.LoadCompany` → `PettyCashBLL.LoadCompany` → `PettyCashDaL.LoadCompanydropdown` | SELECT | `tblCompanyInfo` | — |
| `companyDropDownList_OnSelectedIndexChanged` | `aCashBll.LoadCashAcc(...)` | `PettyCashBLL.LoadCashAcc(DropDownList ddl, string id)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.LoadCashAcc` → `PettyCashDaL.LoadCashAccDal` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | — | — |
| `SaveChangesforPettycashDetails` | `aCashBll.SaveDataForDetails(...)` | `PettyCashBLL.SaveDataForDetails(List<PettyCashDetailsDao> aDirectStockInDaoList)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.SaveDataForDetails` → `PettyCashDaL.SaveDataForDetail` | — | — | `sp_I_PrettyCashDetails` |
| `SaveChangesMaster` | `aCashBll.SaveDataBll(...)` | `PettyCashBLL.SaveDataBll(PettyCashMasterDao aCashMasterDao)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.SaveDataBll` → `PettyCashDaL.SavePettyCashMaster` | — | — | `sp_I_PrettyCashMaster` |
| `UpdateChangesMaster` | `aCashBll.UpdateDataBll(...)` | `PettyCashBLL.UpdateDataBll(PettyCashMasterDao aCashMasterDao)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.UpdateDataBll` → `PettyCashDaL.UpdateMasterDataDal` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_UD_prettyCashMaster` |
| `UpdateChangesforPettycashDetails` | `aCashBll.UpdateDetailsDataBll(...)` | `PettyCashBLL.UpdateDetailsDataBll(List<PettyCashDetailsDao> aDirectStockInDaoList)` <br/>`Library.BLL/SInventory_BLL/PettyCashBLL.cs` | `PettyCashBLL.UpdateDetailsDataBll` → `PettyCashDaL.UpdateDetailsDataDal` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_UD_prettyCashDetails` |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `CompanyDropDownList` | Company Name: | asp:DropDownList | True | companyDropDownList_OnSelectedIndexChanged | `aCashBll.LoadCashAcc()` in `GetOneRecord()`; `aCashBll.LoadCashAcc()` in `companyDropDownList_OnSelectedIndexChanged()`; `aCashBll.LoadCompany()` in `LoaddropdownList()` | `tblCompanyInfo` | set in code (see the DAL method above) |
| `CashAccDropDownlist` | Cash Account: | asp:DropDownList | False | — | `aCashBll.LoadCashAcc()` in `GetOneRecord()`; `aCashBll.LoadCashAcc()` in `companyDropDownList_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |

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
| `ImageButton3` | asp:ImageButton |  | — | `ListImageButton_Click` | 8 lines | — | — |
| `imgDatse` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton1` | asp:ImageButton |  | — | `itemaddImageButton_Click` | 50 lines | — | — |
| `ImageButton2` | asp:ImageButton |  | — | `itemdeleteImageButton_Click` | 36 lines | — | — |
| `itemdeleteImageButton` | asp:LinkButton |  | — | `itemdeleteImageButton_Click` | 36 lines | — | — |
| `btnSave` | asp:Button | Save | — | `submitButton_Click1` | 55 lines | — | — |
| `Button2` | asp:Button | Clear | — | `clearButton_OnClick` | 6 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none
- External scripts: `../css/Date/jquery.datepick.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 15 | — | — | — | — |
| 2 | `ListImageButton_Click` | `void ListImageButton_Click(object sender, EventArgs e)` | 8 | — | — | — | — |
| 3 | `Clear` | `void Clear()` | 12 | — | — | — | — |
| 4 | `GetOneRecord` | `void GetOneRecord(Int32 id)` | 64 | `aCashBll.GetDataMasterByIdBll`, `aCashBll.GetDetailsInfoByIdBll`, `aCashBll.LoadCashAcc` | — | — | — |
| 5 | `dateOfBirthTextBox_OnTextChanged` | `void dateOfBirthTextBox_OnTextChanged(object sender, EventArgs e)` | 14 | — | — | — | — |
| 6 | `LoaddropdownList` | `void LoaddropdownList()` | 5 | `aCashBll.LoadCompany` | — | — | — |
| 7 | `LoadInitialGrid` | `void LoadInitialGrid()` | 34 | — | — | — | — |
| 8 | `companyDropDownList_OnSelectedIndexChanged` | `void companyDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 7 | `aCashBll.LoadCashAcc` | — | — | — |
| 9 | `ddlCashACC_OnSelectedIndexChanged` | `void ddlCashACC_OnSelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 10 | `itemaddImageButton_Click` | `void itemaddImageButton_Click(object sender, EventArgs e)` | 50 | — | — | — | — |
| 11 | `itemdeleteImageButton_Click` | `void itemdeleteImageButton_Click(object sender, EventArgs e)` | 36 | — | — | — | — |
| 12 | `AllGridCalc` | `void AllGridCalc()` | 7 | — | — | — | — |
| 13 | `TlayerCalculation_BytxtLayer` | `void TlayerCalculation_BytxtLayer(int rowIndex)` | 53 | — | — | — | — |
| 14 | `IsInteger` | `bool IsInteger(string str)` | 23 | — | — | — | — |
| 15 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 16 | `PrepareDataForSaveMaster` | `PettyCashMasterDao PrepareDataForSaveMaster()` | 21 | — | — | — | `LoginName` |
| 17 | `PrepareMasterForUpdate` | `PettyCashMasterDao PrepareMasterForUpdate()` | 21 | — | — | — | `LoginName` |
| 18 | `PrepareDataForSaveDetails` | `List<PettyCashDetailsDao> PrepareDataForSaveDetails(Int32 Val)` | 29 | — | — | — | — |
| 19 | `PrepareDataUpdateDetails` | `List<PettyCashDetailsDao> PrepareDataUpdateDetails()` | 27 | — | — | — | — |
| 20 | `SaveChangesforPettycashDetails` | `Int32 SaveChangesforPettycashDetails(Int32 Val)` | 14 | `aCashBll.SaveDataForDetails` | — | — | — |
| 21 | `SaveChangesMaster` | `Int32 SaveChangesMaster()` | 15 | `aCashBll.SaveDataBll` | — | — | — |
| 22 | `UpdateChangesMaster` | `bool UpdateChangesMaster()` | 15 | `aCashBll.UpdateDataBll` | — | — | — |
| 23 | `UpdateChangesforPettycashDetails` | `bool UpdateChangesforPettycashDetails()` | 14 | `aCashBll.UpdateDetailsDataBll` | — | — | — |
| 24 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 55 | — | — | — | — |
| 25 | `PerticularTextBox_TextChanged` | `void PerticularTextBox_TextChanged(object sender, EventArgs e)` | 32 | — | — | — | — |
| 26 | `Validation` | `bool Validation()` | 63 | — | — | — | — |
| 27 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 6 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
PrettyCashEntry.aspx (markup)
  -> SInventory_UI_PrettyCashEntry (code-behind event handler)
     -> Library.BLL.SInventory_BLL.*BLL          (thin pass-through + calculations)
        -> Library.DAL.SInventory_DAL.*DAL       (builds SQL string / SqlParameter list)
           -> Library.DAL.InternalCls.ClsCommonInternalDAL
              -> EXEC ExecuteAllSqlQueryByStoreProcedure @Query   (for inline SQL)
              -> EXEC <named stored procedure>                    (for parameterised calls)
                 -> SQL Server: SalesrollDB_ZAS
```

## 13. API Mapping

AutoComplete extenders bound to the SOAP web service `Solution.Web/SInventory_UI/SInventoryWebService.asmx` (implementation `Solution.Web/App_Code/SInventoryWebService.cs`):

| Extender | TargetControlID | ServiceMethod | ServicePath | MinimumPrefixLength |
|---|---|---|---|---|
| `productNameTextBox_AutoCompleteExtender` | `txtPerticular` | `GetPerticularaction` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `GetOneRecord` | `PettyCashBLL` | `GetDataMasterByIdBll` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `GetOneRecord` | `PettyCashBLL` | `GetDetailsInfoByIdBll` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `GetOneRecord` | `PettyCashBLL` | `LoadCashAcc` | `DropDownList ddl, string id` | `void` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `LoaddropdownList` | `InvoiceBLL` | `LoadCompany` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoaddropdownList` | `PettyCashBLL` | `LoadCompany` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `companyDropDownList_OnSelectedIndexChanged` | `PettyCashBLL` | `LoadCashAcc` | `DropDownList ddl, string id` | `void` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `SaveChangesforPettycashDetails` | `PettyCashBLL` | `SaveDataForDetails` | `List<PettyCashDetailsDao> aDirectStockInDaoList` | `Int32` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `SaveChangesMaster` | `PettyCashBLL` | `SaveDataBll` | `PettyCashMasterDao aCashMasterDao` | `Int32` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `UpdateChangesMaster` | `PettyCashBLL` | `UpdateDataBll` | `PettyCashMasterDao aCashMasterDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `UpdateChangesforPettycashDetails` | `PettyCashBLL` | `UpdateDetailsDataBll` | `List<PettyCashDetailsDao> aDirectStockInDaoList` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoaddropdownList` | `CompanywisebranchDal` | `LoadCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` |
| `LoaddropdownList` | `InvoiceDAL` | `LoadCompany` | `DropDownList aDropDownList` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| `LoaddropdownList` | `MaterialConversionReceiveDal` | `LoadCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_GET_PretyCashMaster_ById` | GetOneRecord → `PettyCashBLL.GetDataMasterByIdBll → PettyCashDaL.GetMasterInfoByIdDal → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_GET_PretyCashDetails_ById` | GetOneRecord → `PettyCashBLL.GetDetailsInfoByIdBll → PettyCashDaL.GetDetailsInfoByIdDal → ClsCommonInternalDAL.GetDataTableAction → DB_Manager.GetDataTableAction` | **No** — see note |
| `sp_I_PrettyCashDetails` | SaveChangesforPettycashDetails → `PettyCashBLL.SaveDataForDetails → PettyCashDaL.SaveDataForDetail` | **No** — see note |
| `sp_I_PrettyCashMaster` | SaveChangesMaster → `PettyCashBLL.SaveDataBll → PettyCashDaL.SavePettyCashMaster` | **No** — see note |
| `sp_UD_prettyCashMaster` | UpdateChangesMaster → `PettyCashBLL.UpdateDataBll → PettyCashDaL.UpdateMasterDataDal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_UD_prettyCashDetails` | UpdateChangesforPettycashDetails → `PettyCashBLL.UpdateDetailsDataBll → PettyCashDaL.UpdateDetailsDataDal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoaddropdownList()` → `CompanywisebranchDal.LoadCompany` → `ClsCommonInternalDAL.LoadDropDownValue` → `InvoiceDAL.LoadCompany` → `MaterialConversionReceiveDal.LoadCompany` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `GetOneRecord` | Update | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `submitButton_Click1` | Opearation Successfully Done... | No | Yes | UNKNOWN | Notification |
| `submitButton_Click1` | Opearation Successfully Done... | No | Yes | UNKNOWN | Notification |
| `clearButton_OnClick` | Opearation Successfully Done... | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `freqQtyTextBoxs` | `txtAmount` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-FIN-025-001` | Update | Code | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx.cs` → `GetOneRecord()` | user action on this page | operation is refused | "Update" | CONFIRMED |
| `BR-FIN-025-002` | {0} | Code | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-FIN-025-003` | Opearation Successfully Done... | Code | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx.cs` → `submitButton_Click1()` | user action on this page | operation is refused | "Opearation Successfully Done..." | CONFIRMED |
| `BR-FIN-025-004` | Opearation Successfully Done... | Code | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx.cs` → `submitButton_Click1()` | user action on this page | operation is refused | "Opearation Successfully Done..." | CONFIRMED |
| `BR-FIN-025-005` | Opearation Successfully Done... | Code | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx.cs` → `clearButton_OnClick()` | user action on this page | operation is refused | "Opearation Successfully Done..." | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |
| Database object | `tblCompanyInfo` |
| Stored procedure | `sp_GET_PretyCashMaster_ById` |
| Stored procedure | `sp_GET_PretyCashDetails_ById` |
| Stored procedure | `sp_I_PrettyCashDetails` |
| Stored procedure | `sp_I_PrettyCashMaster` |
| Stored procedure | `sp_UD_prettyCashMaster` |
| Stored procedure | `sp_UD_prettyCashDetails` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-FIN-025-001` | Unauthenticated access | Request `/SInventory_UI/PrettyCashEntry.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-FIN-025-002` | Validation: Update | Trigger `GetOneRecord()` in the state that produces this message | alert "Update" and the operation is not performed | CONFIRMED |
| `TC-FIN-025-003` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-FIN-025-004` | Validation: Opearation Successfully Done... | Trigger `submitButton_Click1()` in the state that produces this message | alert "Opearation Successfully Done..." and the operation is not performed | CONFIRMED |
| `TC-FIN-025-005` | Validation: Opearation Successfully Done... | Trigger `submitButton_Click1()` in the state that produces this message | alert "Opearation Successfully Done..." and the operation is not performed | CONFIRMED |
| `TC-FIN-025-006` | Validation: Opearation Successfully Done... | Trigger `clearButton_OnClick()` in the state that produces this message | alert "Opearation Successfully Done..." and the operation is not performed | CONFIRMED |
| `TC-FIN-025-007` | Grid loads | Open the page with data present | Grid `itemGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/PrettyCashEntry.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 32 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 27 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 6 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 1 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- `Library.BLL/SInventory_BLL/PettyCashBLL.cs` is **shared with 10 other page(s)** (e.g. `CreditCustomerViewer.aspx`, `CreditReportAllViewer.aspx`, `ProductDiscountReportAllViewer.aspx`, `ProductDiscountReportViewer.aspx`, `CreditCustomerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` is **shared with 3 other page(s)** (e.g. `CreditCustomerReport.aspx`, `MaterialConversionReceive.aspx`, `ProductDiscountReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs` is **shared with 9 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CreditCustomerReport.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` is **shared with 4 other page(s)** (e.g. `CreditCustomerReport.aspx`, `MaterialConversionReceive.aspx`, `ProductDiscountReport.aspx`, `WeaightedAVG_Report.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
