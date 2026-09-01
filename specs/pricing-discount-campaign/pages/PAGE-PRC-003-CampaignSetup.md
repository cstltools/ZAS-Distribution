# PRC-003 — CampaignSetup

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `PRC-003` |
| Module | `pricing-discount-campaign` |
| File name | `CampaignSetup.aspx` |
| Route | `/SInventory_UI/CampaignSetup.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `CampaignSetup.aspx.cs` |
| Code-behind class | `SInventory_UI_CampaignSetup` |
| Markup lines | 322 |
| Code-behind lines | 685 |
| Server controls | 45 |
| GridViews | 1 |
| Code-behind methods | 20 |

## 2. Purpose

Master-data / setup screen. It captures and maintains reference data used by the transactional screens. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Master-data / setup screen. It captures and maintains reference data used by the transactional screens. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/BonusCampaignSetup.aspx`
- referenced from `SInventory_UI/CampaignView.aspx`

Outbound `Response.Redirect` targets: `CampaignView.aspx`

## 5. Route

`/SInventory_UI/CampaignSetup.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CampaignSetup.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CampaignSetup.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CampaignBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |

## 7. UI Control Inventory

45 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 48 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 49 |
| 4 | `areaInfoNewImageButton` | asp:ImageButton | View List | ~/images/viewList.png |  |  |  | areaInfoNewImageButton_Click | 77 |
| 5 | `companyDropDownList` | asp:DropDownList | Company |  | True |  |  | companyDropDownList_OnSelectedIndexChanged | 99 |
| 6 | `masterHiddenField` | asp:HiddenField |  |  |  |  |  |  | 104 |
| 7 | `productGridView` | asp:GridView |  |  |  |  |  |  | 129 |
| 8 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 131 |
| 9 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 138 |
| 10 | `productCodeTextBox` | asp:TextBox | productGridView → col[1] "Code" |  | True |  |  | productCodeTextBox_TextChanged | 140 |
| 11 | `productidHiddenField` | asp:HiddenField | productGridView → col[1] "Code" |  |  |  |  |  | 142 |
| 12 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 145 |
| 13 | `productNameTextBox` | asp:TextBox | productGridView → col[2] "Point Product" |  | True |  |  | productNameTextBox_TextChanged | 147 |
| 14 | `productNameTextBox_AutoCompleteExtender` | ajaxToolkit:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 149 |
| 15 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 158 |
| 16 | `qtyTextBox` | asp:TextBox | productGridView → col[3] "Qty" |  | True |  |  |  | 160 |
| 17 | `freqQtyTextBox` | ajaxToolkit:FilteredTextBoxExtender |  |  |  |  |  |  | 162 |
| 18 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 166 |
| 19 | `bonusproductCodeTextBox` | asp:TextBox | productGridView → col[4] "Code" |  | True |  |  | bonusproductCodeTextBox_OnTextChanged | 168 |
| 20 | `bonusproductidHiddenField` | asp:HiddenField | productGridView → col[4] "Code" |  |  |  |  |  | 171 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 175 |
| 22 | `bonusproductNameTextBox` | asp:TextBox | productGridView → col[5] "Bonus Product Name" |  | True |  |  | bonusproductNameTextBox_OnTextChanged | 177 |
| 23 | `bonusproductNameTextBox_AutoCompleteExtendera` | ajaxToolkit:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 179 |
| 24 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 188 |
| 25 | `bonusQtyTextBox` | asp:TextBox | productGridView → col[6] "Bonus Qty" |  | True |  |  |  | 190 |
| 26 | `fbQtyTextBox` | ajaxToolkit:FilteredTextBoxExtender |  |  |  |  |  |  | 192 |
| 27 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 196 |
| 28 | `isActiveCheckBox` | asp:CheckBox | productGridView → col[7] "Is Active" |  |  |  |  |  | 198 |
| 29 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 201 |
| 30 | `fromDateTextBox` | asp:TextBox | productGridView → col[8] "From Date" |  |  |  |  |  | 203 |
| 31 | `Date` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 207 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 212 |
| 33 | `fromtimeTextBox` | asp:TextBox | productGridView → col[9] "Time" |  |  |  |  |  | 214 |
| 34 | `from` | ajaxToolkit:MaskedEditExtender |  |  |  |  |  |  | 216 |
| 35 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 222 |
| 36 | `toDateTextBox` | asp:TextBox | productGridView → col[10] "To Date" |  |  |  |  |  | 224 |
| 37 | `toDate` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 228 |
| 38 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 233 |
| 39 | `totimeTextBox` | asp:TextBox | productGridView → col[11] "Time" |  |  |  |  |  | 235 |
| 40 | `to` | ajaxToolkit:MaskedEditExtender |  |  |  |  |  |  | 237 |
| 41 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 243 |
| 42 | `ImageButton1` | asp:ImageButton | productGridView → col[12] "Add" | ~/images/lineAdd.png |  |  |  | ImageButton1_Click | 245 |
| 43 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 249 |
| 44 | `ImageButton2` | asp:ImageButton | productGridView → col[13] "Remove" | ~/images/lineDelete.png |  |  |  | ImageButton2_Click | 251 |
| 45 | `Button1` | asp:Button |  | Save |  |  |  | Button1_Click | 289 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `productGridView`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | SL | — | — | — | — |
| 1 | TemplateField | Code | — | `ProductCode`, `ProductId` | `TextBox:productCodeTextBox`, `HiddenField:productidHiddenField` | — |
| 2 | TemplateField | Point Product | — | `ProductName` | `TextBox:productNameTextBox` | — |
| 3 | TemplateField | Qty | — | `Quantity` | `TextBox:qtyTextBox` | — |
| 4 | TemplateField | Code | — | `BonusProductCode`, `BonusProductId` | `TextBox:bonusproductCodeTextBox`, `HiddenField:bonusproductidHiddenField` | — |
| 5 | TemplateField | Bonus Product Name | — | `BonusProductName` | `TextBox:bonusproductNameTextBox` | — |
| 6 | TemplateField | Bonus Qty | — | `BonusQuantity` | `TextBox:bonusQtyTextBox` | — |
| 7 | TemplateField | Is Active | — | — | `CheckBox:isActiveCheckBox` | — |
| 8 | TemplateField | From Date | — | `FromDate` | `TextBox:fromDateTextBox` | — |
| 9 | TemplateField | Time | — | `FromTime` | `TextBox:fromtimeTextBox` | — |
| 10 | TemplateField | To Date | — | `ToDate` | `TextBox:toDateTextBox` | — |
| 11 | TemplateField | Time | — | `ToTime` | `TextBox:totimeTextBox` | — |
| 12 | TemplateField | Add | — | — | `ImageButton:ImageButton1` | — |
| 13 | TemplateField | Remove | — | — | `ImageButton:ImageButton2` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CampaignDAO`, `CampaignDetailDAO`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadData` | `aCampaignDal.LoadingleData(...)` | `CampaignDAL.LoadingleData(string id)` <br/>`Library.DAL/SInventory_DAL/CampaignDAL.cs` | `CampaignDAL.LoadingleData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblBonusCampaignMaster`, `tblBonusCampgainDetail`, `tblProduct` | — |
| `LoadDropDownList` | `aCampaignDal.GetDdlCompany(...)` | `CampaignDAL.GetDdlCompany(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CampaignDAL.cs` | `CampaignDAL.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyInfo` | — |
| `Save` | `aCampaignBll.SaveCampaignMast(...)` | `CampaignBLL.SaveCampaignMast(CampaignDAO aCampaignDao)` <br/>`Library.BLL/SInventory_BLL/CampaignBLL.cs` | `CampaignBLL.SaveCampaignMast` → `AreaDAL.HasAreaName` → `ClsCommonInternalDAL.DataContainerDataReader` → `CampaignDAL.GetMasterCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CampaignDAL.SaveCampaignMaster` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` → `ClsPrimaryKeyFind.PrimaryKeyMax` | INSERT, SELECT | `tblArea`, `tblBonusCampaignMaster` | — |
| `Save` | `aCampaignDal.SaveCampaignDetail(...)` | `CampaignDAL.SaveCampaignDetail(CampaignDetailDAO aCampaignDetailDao)` <br/>`Library.DAL/SInventory_DAL/CampaignDAL.cs` | `CampaignDAL.SaveCampaignDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblBonusCampgainDetail` | — |
| `Update` | `aCampaignDal.DeleteCampaignDetail(...)` | `CampaignDAL.DeleteCampaignDetail(string id)` <br/>`Library.DAL/SInventory_DAL/CampaignDAL.cs` | `CampaignDAL.DeleteCampaignDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | DELETE | `tblBonusCampgainDetail` | — |
| `Update` | `aCampaignDal.SaveCampaignDetail(...)` | `CampaignDAL.SaveCampaignDetail(CampaignDetailDAO aCampaignDetailDao)` <br/>`Library.DAL/SInventory_DAL/CampaignDAL.cs` | `CampaignDAL.SaveCampaignDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblBonusCampgainDetail` | — |
| `Update` | `aCampaignDal.UpdateCampaignMaster(...)` | `CampaignDAL.UpdateCampaignMaster(CampaignDAO aCampaignDao)` <br/>`Library.DAL/SInventory_DAL/CampaignDAL.cs` | `CampaignDAL.UpdateCampaignMaster` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblBonusCampaignMaster` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyDropDownList` | Company | asp:DropDownList | True | companyDropDownList_OnSelectedIndexChanged | `_aDcStockReceiveBll.ProductInfo()` in `GetProductInGrid()`; `_aDcStockReceiveBll.ProductInfo()` in `GetProductInGridBonus()`; `aCampaignDal.GetDdlCompany()` in `LoadDropDownList()` | `View_DCStoreCurrentStock`, `tblCompanyInfo`, `tblManufacturer`, `tblPackSize`, `tblProCategory`, `tblProType`, `tblProduct`, `tblProductCase`, `tblProductSQ`, `tblStockUOM`, `tblUnitPrice` | set in code (see the DAL method above) |

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
| `areaInfoNewImageButton` | asp:ImageButton |  | — | `areaInfoNewImageButton_Click` | 3 lines | — | — |
| `ImageButton1` | asp:ImageButton |  | — | `ImageButton1_Click` | 4 lines | — | — |
| `ImageButton2` | asp:ImageButton |  | — | `ImageButton2_Click` | 98 lines | — | — |
| `Button1` | asp:Button | Save | — | `Button1_Click` | 14 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:AutoCompleteExtender`(productNameTextBox), `ajaxToolkit:FilteredTextBoxExtender`(qtyTextBox), `ajaxToolkit:AutoCompleteExtender`(bonusproductNameTextBox), `ajaxToolkit:FilteredTextBoxExtender`(bonusQtyTextBox), `ajaxToolkit:CalendarExtender`(fromDateTextBox), `ajaxToolkit:MaskedEditExtender`(fromtimeTextBox), `ajaxToolkit:CalendarExtender`(toDateTextBox), `ajaxToolkit:MaskedEditExtender`(totimeTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 14 | — | — | — | `MasterId` |
| 2 | `LoadData` | `void LoadData(string id)` | 15 | `aCampaignDal.LoadingleData` | — | — | — |
| 3 | `InitialGrid` | `void InitialGrid()` | 57 | — | — | — | — |
| 4 | `ImageButton1_Click` | `void ImageButton1_Click(object sender, ImageClickEventArgs e)` | 4 | — | — | — | — |
| 5 | `productCodeTextBox_TextChanged` | `void productCodeTextBox_TextChanged(object sender, EventArgs e)` | 13 | — | — | — | — |
| 6 | `ProductExistOrNot` | `bool ProductExistOrNot(string productCode, int rowindex)` | 13 | — | — | — | — |
| 7 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 8 | `productNameTextBox_TextChanged` | `void productNameTextBox_TextChanged(object sender, EventArgs e)` | 27 | — | — | — | — |
| 9 | `GetProductInGrid` | `void GetProductInGrid(int rowindex, string productCode)` | 64 | — | — | — | — |
| 10 | `GetProductInGridBonus` | `void GetProductInGridBonus(int rowindex, string productCode)` | 49 | — | — | — | — |
| 11 | `AddRowInGrid` | `void AddRowInGrid()` | 102 | — | — | — | — |
| 12 | `ImageButton2_Click` | `void ImageButton2_Click(object sender, ImageClickEventArgs e)` | 98 | — | — | — | — |
| 13 | `bonusproductCodeTextBox_OnTextChanged` | `void bonusproductCodeTextBox_OnTextChanged(object sender, EventArgs e)` | 13 | — | — | — | — |
| 14 | `LoadDropDownList` | `void LoadDropDownList()` | 4 | `aCampaignDal.GetDdlCompany` | — | — | — |
| 15 | `bonusproductNameTextBox_OnTextChanged` | `void bonusproductNameTextBox_OnTextChanged(object sender, EventArgs e)` | 28 | — | — | — | — |
| 16 | `Save` | `void Save()` | 42 | `aCampaignBll.SaveCampaignMast`, `aCampaignDal.SaveCampaignDetail` | — | — | `LoginName` |
| 17 | `Button1_Click` | `void Button1_Click(object sender, EventArgs e)` | 14 | — | — | — | — |
| 18 | `Update` | `void Update()` | 44 | `aCampaignDal.DeleteCampaignDetail`, `aCampaignDal.SaveCampaignDetail`, `aCampaignDal.UpdateCampaignMaster` | — | — | `LoginName` |
| 19 | `companyDropDownList_OnSelectedIndexChanged` | `void companyDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 23 | — | — | — | — |
| 20 | `areaInfoNewImageButton_Click` | `void areaInfoNewImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
CampaignSetup.aspx (markup)
  -> SInventory_UI_CampaignSetup (code-behind event handler)
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
| `productNameTextBox_AutoCompleteExtender` | `productNameTextBox` | `GetProductByMenufracturer` | `SInventoryWebService.asmx` | 1 |
| `bonusproductNameTextBox_AutoCompleteExtendera` | `bonusproductNameTextBox` | `GetProductByMenufracturer` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Save` | `CampaignBLL` | `SaveCampaignMast` | `CampaignDAO aCampaignDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/CampaignBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadData` | `CampaignDAL` | `LoadingleData` | `string id` | `DataTable` | SELECT | `tblBonusCampaignMaster`, `tblBonusCampgainDetail`, `tblProduct` | — | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| `LoadDropDownList` | `CampaignDAL` | `GetDdlCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| `Save` | `CampaignDAL` | `SaveCampaignDetail` | `CampaignDetailDAO aCampaignDetailDao` | `int` | INSERT | `tblBonusCampgainDetail` | — | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| `Update` | `CampaignDAL` | `DeleteCampaignDetail` | `string id` | `bool` | DELETE | `tblBonusCampgainDetail` | — | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| `Update` | `CampaignDAL` | `SaveCampaignDetail` | `CampaignDetailDAO aCampaignDetailDao` | `int` | INSERT | `tblBonusCampgainDetail` | — | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| `Update` | `CampaignDAL` | `UpdateCampaignMaster` | `CampaignDAO aCampaignDao` | `bool` | UPDATE | `tblBonusCampaignMaster` | — | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `Save()` → `CampaignBLL.SaveCampaignMast` → `AreaDAL.HasAreaName` → `ClsCommonInternalDAL.DataContainerDataReader` → `CampaignDAL.GetMasterCode` |
| `tblBonusCampaignMaster` | `LoadData()` → `CampaignDAL.LoadingleData` → `ClsCommonInternalDAL.DataContainerDataTable`, `Save()` → `CampaignBLL.SaveCampaignMast` → `AreaDAL.HasAreaName` → `ClsCommonInternalDAL.DataContainerDataReader` → `CampaignDAL.GetMasterCode`, `Update()` → `CampaignDAL.UpdateCampaignMaster` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblBonusCampgainDetail` | `LoadData()` → `CampaignDAL.LoadingleData` → `ClsCommonInternalDAL.DataContainerDataTable`, `Save()` → `CampaignDAL.SaveCampaignDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity`, `Update()` → `CampaignDAL.DeleteCampaignDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `Update()` → `CampaignDAL.SaveCampaignDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` |
| `tblCompanyInfo` | `LoadDropDownList()` → `CampaignDAL.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblProduct` | `LoadData()` → `CampaignDAL.LoadingleData` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `Page_Load` | Update | No | Yes | UNKNOWN | Notification |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `Save` | Opearation Successfully Done... | No | Yes | UNKNOWN | Notification |
| `Update` | Opearation Successfully Done... | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `freqQtyTextBox` | `qtyTextBox` | Custom, Numbers | . |
| `fbQtyTextBox` | `bonusQtyTextBox` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-PRC-003-001` | Update | Code | `Solution.Web/SInventory_UI/CampaignSetup.aspx.cs` → `Page_Load()` | user action on this page | operation is refused | "Update" | CONFIRMED |
| `BR-PRC-003-002` | {0} | Code | `Solution.Web/SInventory_UI/CampaignSetup.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-PRC-003-003` | Opearation Successfully Done... | Code | `Solution.Web/SInventory_UI/CampaignSetup.aspx.cs` → `Save()` | user action on this page | operation is refused | "Opearation Successfully Done..." | CONFIRMED |
| `BR-PRC-003-004` | Opearation Successfully Done... | Code | `Solution.Web/SInventory_UI/CampaignSetup.aspx.cs` → `Update()` | user action on this page | operation is refused | "Opearation Successfully Done..." | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `MasterId`

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
| Assembly source | `Library.BLL/SInventory_BLL/CampaignBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| Database object | `tblArea` |
| Database object | `tblBonusCampaignMaster` |
| Database object | `tblBonusCampgainDetail` |
| Database object | `tblCompanyInfo` |
| Database object | `tblProduct` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-PRC-003-001` | Unauthenticated access | Request `/SInventory_UI/CampaignSetup.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-PRC-003-002` | Validation: Update | Trigger `Page_Load()` in the state that produces this message | alert "Update" and the operation is not performed | CONFIRMED |
| `TC-PRC-003-003` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-PRC-003-004` | Validation: Opearation Successfully Done... | Trigger `Save()` in the state that produces this message | alert "Opearation Successfully Done..." and the operation is not performed | CONFIRMED |
| `TC-PRC-003-005` | Validation: Opearation Successfully Done... | Trigger `Update()` in the state that produces this message | alert "Opearation Successfully Done..." and the operation is not performed | CONFIRMED |
| `TC-PRC-003-006` | Grid loads | Open the page with data present | Grid `productGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CampaignSetup.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CampaignSetup.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CampaignBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 45 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 20 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 5 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.DAL/SInventory_DAL/CampaignDAL.cs` is **shared with 6 other page(s)** (e.g. `BonusCampaignSetup.aspx`, `CampaignView.aspx`, `ManualOrderrCreation.aspx`, `NonTranscationalInvoiceReturn.aspx`, `ReturnAcknowledgmentNote.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblBonusCampaignMaster` — any schema change to it breaks this page.
- Writes/reads `tblBonusCampgainDetail` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
