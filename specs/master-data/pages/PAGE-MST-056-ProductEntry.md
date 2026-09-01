# MST-056 — ProductEntry

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-056` |
| Module | `master-data` |
| File name | `ProductEntry.aspx` |
| Route | `/SInventory_UI/ProductEntry.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ProductEntry.aspx.cs` |
| Code-behind class | `SInventory_UI_ProductEntry` |
| Markup lines | 375 |
| Code-behind lines | 266 |
| Server controls | 24 |
| GridViews | 0 |
| Code-behind methods | 13 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/ProductDiscountView.aspx`
- referenced from `SInventory_UI/ProductQuotedPriceView.aspx`
- referenced from `SInventory_UI/ProductView.aspx`

Outbound `Response.Redirect` targets: `ProductView.aspx`

## 5. Route

`/SInventory_UI/ProductEntry.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductEntry.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductBLL.cs` |

## 7. UI Control Inventory

24 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 58 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 60 |
| 4 | `productListImageButton` | asp:ImageButton | View List | ~/images/viewList.png |  |  |  | productImageButton_Click | 81 |
| 5 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 86 |
| 6 | `companyNameDropDownList` | asp:DropDownList | Company Name |  |  |  |  |  | 97 |
| 7 | `productCodeTextBox` | asp:TextBox | Product Code |  | true |  |  | productCodeTextBox_OnTextChanged | 109 |
| 8 | `productNameTextBox` | asp:TextBox | Product Name |  | true |  |  | productNameTextBox_OnTextChanged | 119 |
| 9 | `descriptionTextBox` | asp:TextBox | Description |  |  |  |  |  | 129 |
| 10 | `categoryNameDropDownList` | asp:DropDownList | Category |  |  |  |  |  | 140 |
| 11 | `manufacDropDownList` | asp:DropDownList | Manufacturer |  |  |  |  |  | 153 |
| 12 | `packSizeDropDownList` | asp:DropDownList | Pack Size |  |  |  |  |  | 167 |
| 13 | `stockUOMDropDownList` | asp:DropDownList | UOM |  |  |  |  |  | 181 |
| 14 | `typeDropDownList` | asp:DropDownList | Product Type |  |  |  |  |  | 195 |
| 15 | `productSQDropDownList` | asp:DropDownList | Product Brand |  |  |  |  |  | 209 |
| 16 | `shippingCartonSizeDropDownList` | asp:DropDownList |  |  |  |  |  |  | 226 |
| 17 | `cbxTradeOrRecipe` | asp:RadioButtonList |  |  | True |  |  | cbxTradeOrRecipe_Clicked | 242 |
| 18 | `(no id)` | asp:ListItem |  |  |  |  |  |  | 251 |
| 19 | `(no id)` | asp:ListItem | Trade |  |  |  |  |  | 252 |
| 20 | `ddlRawMaterials` | asp:DropDownList | Raw Material Name: |  |  |  |  |  | 274 |
| 21 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click1 | 311 |
| 22 | `clearButton` | asp:Button |  | Cancel |  |  |  | clearButton_OnClick | 314 |
| 23 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 344 |
| 24 | `imgWait` | asp:Image | bsp; | ~/images/progress-bar-opt.gif |  |  |  |  | 347 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `Product`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadRawMaterial` | `aProductBLL.LoadRawMaterial(...)` | `ProductBLL.LoadRawMaterial(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadRawMaterial` → `ProductDAL.LoadRawMaterial` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | — | — |
| `LoadProductCategory` | `aProductBLL.LoadCompanyDropdownList(...)` | `ProductBLL.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadCompanyDropdownList` | — | — | — |
| `LoadProductCategory` | `aProductBLL.LoadManufac(...)` | `ProductBLL.LoadManufac(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadManufac` → `ProductDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | — | `tblManufacturer` | — |
| `LoadProductCategory` | `aProductBLL.LoadPackSize(...)` | `ProductBLL.LoadPackSize(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadPackSize` → `ProductDAL.LoadPackSize` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | — | `tblPackSize` | — |
| `LoadProductCategory` | `aProductBLL.LoadProductCategory(...)` | `ProductBLL.LoadProductCategory(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadProductCategory` → `ProductDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | — | `tblProCategory` | — |
| `LoadProductCategory` | `aProductBLL.LoadProductSQ(...)` | `ProductBLL.LoadProductSQ(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadProductSQ` → `ProductDAL.LoadProductSQ` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | — | `tblProductSQ` | — |
| `LoadProductCategory` | `aProductBLL.LoadShippingCartonSize(...)` | `ProductBLL.LoadShippingCartonSize(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadShippingCartonSize` → `ProductDAL.LoadShippingCartonSize` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | — | `tblProductCase` | — |
| `LoadProductCategory` | `aProductBLL.LoadStockUOM(...)` | `ProductBLL.LoadStockUOM(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadStockUOM` → `ProductDAL.LoadStockUOM` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | — | `tblStockUOM` | — |
| `LoadProductCategory` | `aProductBLL.LoadType(...)` | `ProductBLL.LoadType(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.LoadType` → `ProductDAL.LoadType` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | — | `tblProType` | — |
| `submitButton_Click1` | `aProductBLL.CretaeItem(...)` | `ProductBLL.CretaeItem(Product aProduct)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.CretaeItem` → `ProductDAL.CretaeItem` | — | — | `SP_DataMigration_Item` |
| `submitButton_Click1` | `aProductBLL.GetPoductByCode(...)` | `ProductBLL.GetPoductByCode(string productCode)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.GetPoductByCode` → `ProductDAL.GetPoductByCode` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct` | — |
| `submitButton_Click1` | `aProductBLL.SaveProduct(...)` | `ProductBLL.SaveProduct(Product Product)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.SaveProduct` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProductDAL.SaveDataForProduct` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `ProductBLL.ProductCodeGenerator` | SELECT | `tblProduct` | — |
| `submitButton_Click1` | `aProductBLL.SaveRawMaterialDetails(...)` | `ProductBLL.SaveRawMaterialDetails(ProductRawMaterialDetailsDao productRawMaterialDetailsDao)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.SaveRawMaterialDetails` → `ProductDAL.SaveRawMaterialDetails` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT | `tbl_MC_ProductRawMaterialDetails` | — |
| `productCodeTextBox_OnTextChanged` | `aProductBLL.CodeExistOrNot(...)` | `ProductBLL.CodeExistOrNot(string code)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.CodeExistOrNot` → `ProductDAL.CodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct` | — |
| `productNameTextBox_OnTextChanged` | `aProductBLL.NameExistOrNot(...)` | `ProductBLL.NameExistOrNot(string name)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.NameExistOrNot` → `ProductDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyNameDropDownList` | Company Name | asp:DropDownList | False | — | `aProductBLL.LoadCompanyDropdownList()` in `LoadProductCategory()` | UNKNOWN | set in code (see the DAL method above) |
| `categoryNameDropDownList` | Category | asp:DropDownList | False | — | `aProductBLL.LoadProductCategory()` in `LoadProductCategory()` | `tblProCategory` | set in code (see the DAL method above) |
| `manufacDropDownList` | Manufacturer | asp:DropDownList | False | — | `aProductBLL.LoadManufac()` in `LoadProductCategory()` | `tblManufacturer` | set in code (see the DAL method above) |
| `packSizeDropDownList` | Pack Size | asp:DropDownList | False | — | `aProductBLL.LoadPackSize()` in `LoadProductCategory()` | `tblPackSize` | set in code (see the DAL method above) |
| `stockUOMDropDownList` | UOM | asp:DropDownList | False | — | `aProductBLL.LoadStockUOM()` in `LoadProductCategory()` | `tblStockUOM` | set in code (see the DAL method above) |
| `typeDropDownList` | Product Type | asp:DropDownList | False | — | `aProductBLL.LoadType()` in `LoadProductCategory()` | `tblProType` | set in code (see the DAL method above) |
| `productSQDropDownList` | Product Brand | asp:DropDownList | False | — | `aProductBLL.LoadProductSQ()` in `LoadProductCategory()` | `tblProductSQ` | set in code (see the DAL method above) |
| `shippingCartonSizeDropDownList` |  | asp:DropDownList | False | — | `aProductBLL.LoadShippingCartonSize()` in `LoadProductCategory()` | `tblProductCase` | set in code (see the DAL method above) |
| `cbxTradeOrRecipe` |  | asp:RadioButtonList | True | cbxTradeOrRecipe_Clicked | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |
| `ddlRawMaterials` | Raw Material Name: | asp:DropDownList | False | — | `aProductBLL.LoadRawMaterial()` in `LoadRawMaterial()` | UNKNOWN | set in code (see the DAL method above) |

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
| `productListImageButton` | asp:ImageButton |  | — | `productImageButton_Click` | 3 lines | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click1` | 87 lines | `aProductBLL.CretaeItem`, `aProductBLL.GetPoductByCode`, `aProductBLL.SaveProduct`, `aProductBLL.SaveRawMaterialDetails` | — |
| `clearButton` | asp:Button | Cancel | — | `clearButton_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none
- External scripts: `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `LoadRawMaterial` | `void LoadRawMaterial()` | 3 | `aProductBLL.LoadRawMaterial` | — | — | — |
| 3 | `LoadProductCategory` | `void LoadProductCategory()` | 10 | `aProductBLL.LoadCompanyDropdownList`, `aProductBLL.LoadManufac`, `aProductBLL.LoadPackSize`, `aProductBLL.LoadProductCategory`, `aProductBLL.LoadProductSQ`, `aProductBLL.LoadShippingCartonSize` | — | — | — |
| 4 | `Clear` | `void Clear()` | 14 | — | — | — | — |
| 5 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 6 | `Validation` | `bool Validation()` | 66 | — | — | — | — |
| 7 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 87 | `aProductBLL.CretaeItem`, `aProductBLL.GetPoductByCode`, `aProductBLL.SaveProduct`, `aProductBLL.SaveRawMaterialDetails` | — | — | — |
| 8 | `productImageButton_Click` | `void productImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 9 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 10 | `productCodeTextBox_OnTextChanged` | `void productCodeTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `aProductBLL.CodeExistOrNot` | — | — | — |
| 11 | `GotoPermissionSetUp` | `void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)` | 7 | — | — | — | — |
| 12 | `productNameTextBox_OnTextChanged` | `void productNameTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `aProductBLL.NameExistOrNot` | — | — | — |
| 13 | `cbxTradeOrRecipe_Clicked` | `void cbxTradeOrRecipe_Clicked(object sender, EventArgs e)` | 11 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ProductEntry.aspx (markup)
  -> SInventory_UI_ProductEntry (code-behind event handler)
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
| `LoadRawMaterial` | `ProductBLL` | `LoadRawMaterial` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadManufac` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadPackSize` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadProductCategory` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadProductSQ` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadShippingCartonSize` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadStockUOM` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadProductCategory` | `ProductBLL` | `LoadType` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `submitButton_Click1` | `ProductBLL` | `CretaeItem` | `Product aProduct` | `Int32` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `submitButton_Click1` | `ProductBLL` | `GetPoductByCode` | `string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `submitButton_Click1` | `ProductBLL` | `SaveProduct` | `Product Product` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `submitButton_Click1` | `ProductBLL` | `SaveRawMaterialDetails` | `ProductRawMaterialDetailsDao productRawMaterialDetailsDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `productCodeTextBox_OnTextChanged` | `ProductBLL` | `CodeExistOrNot` | `string code` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `productNameTextBox_OnTextChanged` | `ProductBLL` | `NameExistOrNot` | `string name` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `SP_DataMigration_Item` | submitButton_Click1 → `ProductBLL.CretaeItem → ProductDAL.CretaeItem` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblManufacturer` | `LoadProductCategory()` → `ProductBLL.LoadManufac` → `ProductDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblPackSize` | `LoadProductCategory()` → `ProductBLL.LoadPackSize` → `ProductDAL.LoadPackSize` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblProCategory` | `LoadProductCategory()` → `ProductBLL.LoadProductCategory` → `ProductDAL.LoadCategoryName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblProType` | `LoadProductCategory()` → `ProductBLL.LoadType` → `ProductDAL.LoadType` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblProduct` | `productCodeTextBox_OnTextChanged()` → `ProductBLL.CodeExistOrNot` → `ProductDAL.CodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `productNameTextBox_OnTextChanged()` → `ProductBLL.NameExistOrNot` → `ProductDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `ProductBLL.GetPoductByCode` → `ProductDAL.GetPoductByCode` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `ProductBLL.SaveProduct` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProductDAL.SaveDataForProduct` |
| `tblProductCase` | `LoadProductCategory()` → `ProductBLL.LoadShippingCartonSize` → `ProductDAL.LoadShippingCartonSize` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblProductSQ` | `LoadProductCategory()` → `ProductBLL.LoadProductSQ` → `ProductDAL.LoadProductSQ` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblStockUOM` | `LoadProductCategory()` → `ProductBLL.LoadStockUOM` → `ProductDAL.LoadStockUOM` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tbl_MC_ProductRawMaterialDetails` | `submitButton_Click1()` → `ProductBLL.SaveRawMaterialDetails` → `ProductDAL.SaveRawMaterialDetails` → `ClsCommonInternalDAL.SaveDataByInsertCommand` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-056-001` | {0} | Code | `Solution.Web/SInventory_UI/ProductEntry.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

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
| Assembly source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Database object | `tblManufacturer` |
| Database object | `tblPackSize` |
| Database object | `tblProCategory` |
| Database object | `tblProType` |
| Database object | `tblProduct` |
| Database object | `tblProductCase` |
| Database object | `tblProductSQ` |
| Database object | `tblStockUOM` |
| Database object | `tbl_MC_ProductRawMaterialDetails` |
| Stored procedure | `SP_DataMigration_Item` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-056-001` | Unauthenticated access | Request `/SInventory_UI/ProductEntry.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-056-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductEntry.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 24 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 13 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 9 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/ProductBLL.cs` is **shared with 8 other page(s)** (e.g. `CreatePickingDetailForWareHouse.aspx`, `DcWiseAreaAlocation.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `IssueRequisitionProducts.aspx`) — a change there is not local.
- Writes/reads `tblManufacturer` — any schema change to it breaks this page.
- Writes/reads `tblPackSize` — any schema change to it breaks this page.
- Writes/reads `tblProCategory` — any schema change to it breaks this page.
- Writes/reads `tblProType` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductCase` — any schema change to it breaks this page.
- Writes/reads `tblProductSQ` — any schema change to it breaks this page.
- Writes/reads `tblStockUOM` — any schema change to it breaks this page.
- Writes/reads `tbl_MC_ProductRawMaterialDetails` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
