# INV-034 — WarehouseStockIn

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-034` |
| Module | `inventory-warehouse` |
| File name | `WarehouseStockIn.aspx` |
| Route | `/SInventory_UI/WarehouseStockIn.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `WarehouseStockIn.aspx.cs` |
| Code-behind class | `SInventory_UI_WarehouseStockIn` |
| Markup lines | 523 |
| Code-behind lines | 1054 |
| Server controls | 63 |
| GridViews | 1 |
| Code-behind methods | 31 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/MaterialConversionReceiveView.aspx`
- referenced from `SInventory_UI/WarehouseStockInView.aspx`

Outbound `Response.Redirect` targets: `WarehouseStockInView.aspx`

## 5. Route

`/SInventory_UI/WarehouseStockIn.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |

## 7. UI Control Inventory

63 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 47 |
| 3 | `UpdatePanel2` | asp:UpdatePanel |  |  |  |  |  |  | 48 |
| 4 | `viewLinkButton` | asp:LinkButton |  |  |  |  |  | viewLinkButton_OnClick | 82 |
| 5 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_OnSelectedIndexChanged | 119 |
| 6 | `warehouseDropDownList` | asp:DropDownList | Warehouse: |  |  |  |  |  | 127 |
| 7 | `stockInIdHiddenField` | asp:HiddenField | Warehouse: |  |  |  |  |  | 130 |
| 8 | `stockInDateTextBox` | asp:TextBox | Stock In Date: |  | True |  |  | stockInDateTextBox_OnTextChanged | 136 |
| 9 | `imgDate` | asp:ImageButton | tock In Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 138 |
| 10 | `manufacturerDate1` | cc1:CalendarExtender |  |  |  |  |  |  | 140 |
| 11 | `challanNoTextBox` | asp:TextBox | Challan No: |  |  |  |  |  | 150 |
| 12 | `challanDateTextBox` | asp:TextBox | Challan Date: |  | True |  |  | challanDateTextBox_OnTextChanged | 156 |
| 13 | `ImageButton4` | asp:ImageButton | Challan Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 158 |
| 14 | `CalendarExtender2` | cc1:CalendarExtender |  |  |  |  |  |  | 160 |
| 15 | `referenceNoTextBox` | asp:TextBox | Reference No: |  |  |  |  |  | 168 |
| 16 | `referenceDateTextBox` | asp:TextBox | Reference Date: |  | True |  |  | referenceDateTextBox_OnTextChanged | 176 |
| 17 | `ImageButton5` | asp:ImageButton | Reference Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 178 |
| 18 | `CalendarExtender3` | cc1:CalendarExtender |  |  |  |  |  |  | 180 |
| 19 | `remarksTextBox` | asp:TextBox | Remarks: |  |  |  |  |  | 188 |
| 20 | `ddlSupplier` | asp:DropDownList | Supplier: |  |  |  |  |  | 195 |
| 21 | `productGridView` | asp:GridView |  |  |  |  |  |  | 238 |
| 22 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 240 |
| 23 | `LabelSL` | asp:Label | productGridView → col[0] "SL" |  |  |  |  |  | 242 |
| 24 | `productidHiddenField` | asp:HiddenField | productGridView → col[0] "SL" |  |  |  |  |  | 244 |
| 25 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 247 |
| 26 | `productCodeTextBox` | asp:TextBox | productGridView → col[1] "Product Code" |  | True |  |  | productCodeTextBox_TextChanged | 249 |
| 27 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 254 |
| 28 | `productNameTextBox` | asp:TextBox | productGridView → col[2] "Product Name" |  | True |  |  | productNameTextBox_TextChanged | 256 |
| 29 | `productNameTextBox_AutoCompleteExtender` | cc1:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 258 |
| 30 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 267 |
| 31 | `packSizeTextBox` | asp:TextBox | productGridView → col[3] "PackSize" |  |  | ReadOnly=True |  |  | 269 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 273 |
| 33 | `batchTextBox` | asp:TextBox | productGridView → col[4] "Batch" |  |  |  |  |  | 275 |
| 34 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 279 |
| 35 | `mfgDateTextBox` | asp:TextBox | productGridView → col[5] "Mfg.Date" |  | True |  |  | mfgDateTextBox_OnTextChanged | 281 |
| 36 | `ImageButton15` | asp:ImageButton | productGridView → col[5] "Mfg.Date" | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 283 |
| 37 | `manufacturerDate` | cc1:CalendarExtender |  |  |  |  |  |  | 285 |
| 38 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 290 |
| 39 | `expDateDateTextBox` | asp:TextBox | productGridView → col[6] "Exp.Date" |  | True |  |  | expDateDateTextBox_OnTextChanged | 292 |
| 40 | `ImageButton25` | asp:ImageButton | productGridView → col[6] "Exp.Date" | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 295 |
| 41 | `ExpDate` | cc1:CalendarExtender |  |  |  |  |  |  | 297 |
| 42 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 302 |
| 43 | `reqQtyTextBox` | asp:TextBox | productGridView → col[7] "Qty" |  | True |  |  | reqQtyTextBox_OnTextChanged | 304 |
| 44 | `FilteredTextBoxExtenderQuantity` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 306 |
| 45 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 311 |
| 46 | `costPriceTextBox` | asp:TextBox | productGridView → col[8] "Price" |  | True |  |  | costPriceTextBox_OnTextChanged | 313 |
| 47 | `FilteredTextBoxExtenderPrice` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 315 |
| 48 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 320 |
| 49 | `vatTextBox` | asp:TextBox | productGridView → col[9] "Vat (Per Unit)" |  | True |  |  | vatTextBox_OnTextChanged | 322 |
| 50 | `FilteredTextBoxExtenderVat` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 324 |
| 51 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 329 |
| 52 | `totalValueTextBox` | asp:TextBox | productGridView → col[10] "Total Amt" |  | True | ReadOnly=True |  | totalValueTextBox_OnTextChanged | 331 |
| 53 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 335 |
| 54 | `ImageButton1` | asp:ImageButton | productGridView → col[11] "(no header)" | ~/images/lineAdd.png |  |  |  | ImageButton1_Click | 337 |
| 55 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 341 |
| 56 | `ImageButton2` | asp:ImageButton | productGridView → col[12] "(no header)" | ~/images/lineDelete.png |  |  |  | ImageButton2_Click | 343 |
| 57 | `totalQtyTextBox` | asp:TextBox | Total Qty : |  |  | ReadOnly=True |  |  | 381 |
| 58 | `totalVatTextBox` | asp:TextBox | Total Vat: |  |  | ReadOnly=True |  |  | 399 |
| 59 | `grandTotalTextBox` | asp:TextBox | Grand Total: |  |  | ReadOnly=True |  |  | 417 |
| 60 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 456 |
| 61 | `cancelButton` | asp:Button |  | Cancel |  |  |  | cancelButton_Click | 458 |
| 62 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 513 |
| 63 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 516 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `productGridView`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | SL | — | `ProductId` | `Label:LabelSL`, `HiddenField:productidHiddenField` | — |
| 1 | TemplateField | Product Code | — | `ProductCode` | `TextBox:productCodeTextBox` | — |
| 2 | TemplateField | Product Name | — | `ProductName` | `TextBox:productNameTextBox` | — |
| 3 | TemplateField | PackSize | — | `PackSize` | `TextBox:packSizeTextBox` | — |
| 4 | TemplateField | Batch | — | `Batch` | `TextBox:batchTextBox` | — |
| 5 | TemplateField | Mfg.Date | — | `MfgDate` | `TextBox:mfgDateTextBox`, `ImageButton:ImageButton15` | — |
| 6 | TemplateField | Exp.Date | — | `ExpDate` | `TextBox:expDateDateTextBox`, `ImageButton:ImageButton25` | — |
| 7 | TemplateField | Qty | — | `Quantity` | `TextBox:reqQtyTextBox` | — |
| 8 | TemplateField | Price | — | `Price` | `TextBox:costPriceTextBox` | — |
| 9 | TemplateField | Vat (Per Unit) | — | `Vat` | `TextBox:vatTextBox` | — |
| 10 | TemplateField | Total Amt | — | `TotalAmount` | `TextBox:totalValueTextBox` | — |
| 11 | TemplateField | _(none)_ | — | — | `ImageButton:ImageButton1` | — |
| 12 | TemplateField | _(none)_ | — | — | `ImageButton:ImageButton2` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `WarehouseStockInMasterDao`, `WharehouseStockInDetailsDao`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadWarehouseStockInInfo` | `aWarehouseStockInBll.LoadWarehouseStockInDetailInfo(...)` | `WarehouseStockInBll.LoadWarehouseStockInDetailInfo(string stockInId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadWarehouseStockInDetailInfo` → `WarehouseStockInDal.GetWarehouseStockInDetailInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblWHStockInDetail` | — |
| `LoadWarehouseStockInInfo` | `aWarehouseStockInBll.LoadWarehouseStockInMasterInfo(...)` | `WarehouseStockInBll.LoadWarehouseStockInMasterInfo(string stockInId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadWarehouseStockInMasterInfo` → `WarehouseStockInDal.GetWarehouseStockInMasterInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblWHStockInMaster`, `tblWearhouse` | — |
| `LoadWarehouseStockInInfo` | `aWarehouseStockInBll.LoadWhInformation(...)` | `WarehouseStockInBll.LoadWhInformation(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadWhInformation` → `WarehouseStockInDal.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblWearhouse` | — |
| `DropDownLoad` | `aWarehouseStockInBll.LoadCompanyDropdownList(...)` | `WarehouseStockInBll.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadCompanyDropdownList` → `WarehouseStockInDal.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyInfo` | — |
| `DropDownLoad` | `aWarehouseStockInBll.LoadSupplierName(...)` | `WarehouseStockInBll.LoadSupplierName(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadSupplierName` → `WarehouseStockInDal.LoadSupplierName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblSupplierInformation` | — |
| `GetProductInGrid` | `aWarehouseStockInBll.ProductInfo(...)` | `WarehouseStockInBll.ProductInfo(string productCode, string productId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.ProductInfo` → `WarehouseStockInDal.LoadProductCode` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct` | — |
| `SaveAllData` | `aWarehouseStockInBll.SaveStockInMasterInfo(...)` | `WarehouseStockInBll.SaveStockInMasterInfo(WarehouseStockInMasterDao aStockInMasterDao, out Int32 maxRowId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.SaveStockInMasterInfo` → `WarehouseStockInDal.CheckEntryWithinlastMinute` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveStockInMasterInformation` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `WarehouseStockInBll.StockInId` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblWHStockInMaster` | — |
| `SaveAllData` | `aWarehouseStockInBll.SaveWarehouseStockInDetail(...)` | `WarehouseStockInBll.SaveWarehouseStockInDetail(List<WharehouseStockInDetailsDao> aDetailList)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.SaveWarehouseStockInDetail` → `WarehouseStockInDal.CheckDetailsExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveWarehouseStockInDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT, SELECT | `tblWHStockInDetail` | — |
| `UpdateAllData` | `aWarehouseStockInBll.DeleteWhStockInDetailById(...)` | `WarehouseStockInBll.DeleteWhStockInDetailById(int stockInId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.DeleteWhStockInDetailById` → `WarehouseStockInDal.DeleteWhStockInDetailById` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` | DELETE | `tblWHStockInDetail` | — |
| `UpdateAllData` | `aWarehouseStockInBll.UpdateStockInMasterInfo(...)` | `WarehouseStockInBll.UpdateStockInMasterInfo(WarehouseStockInMasterDao aStockInMasterDao)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.UpdateStockInMasterInfo` → `WarehouseStockInDal.UpdateStockInMasterInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblWHStockInMaster` | — |
| `UpdateAllData` | `aWarehouseStockInBll.UpdateWarehouseStockInDetail(...)` | `WarehouseStockInBll.UpdateWarehouseStockInDetail(List<WharehouseStockInDetailsDao> aDetailList)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.UpdateWarehouseStockInDetail` → `WarehouseStockInDal.SaveWarehouseStockInDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT | `tblWHStockInDetail` | — |
| `companyNameDropDownList_OnSelectedIndexChanged` | `aWarehouseStockInBll.LoadWhInformation(...)` | `WarehouseStockInBll.LoadWhInformation(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadWhInformation` → `WarehouseStockInDal.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblWearhouse` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | companyNameDropDownList_OnSelectedIndexChanged | `aWarehouseStockInBll.LoadCompanyDropdownList()` in `DropDownLoad()`; `aWarehouseStockInBll.LoadWhInformation()` in `LoadWarehouseStockInInfo()`; `aWarehouseStockInBll.LoadWhInformation()` in `companyNameDropDownList_OnSelectedIndexChanged()`; `aWarehouseStockInBll.ProductInfo()` in `GetProductInGrid()` | `tblCompanyInfo`, `tblProduct`, `tblWearhouse` | set in code (see the DAL method above) |
| `warehouseDropDownList` | Warehouse: | asp:DropDownList | False | — | `aWarehouseStockInBll.LoadWhInformation()` in `LoadWarehouseStockInInfo()`; `aWarehouseStockInBll.LoadWhInformation()` in `companyNameDropDownList_OnSelectedIndexChanged()` | `tblWearhouse` | set in code (see the DAL method above) |
| `ddlSupplier` | Supplier: | asp:DropDownList | False | — | `aWarehouseStockInBll.LoadSupplierName()` in `DropDownLoad()` | `tblSupplierInformation` | set in code (see the DAL method above) |

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
| `viewLinkButton` | asp:LinkButton |  | — | `viewLinkButton_OnClick` | 3 lines | — | — |
| `imgDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton4` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton5` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton15` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton25` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton1` | asp:ImageButton |  | — | `ImageButton1_Click` | 3 lines | — | — |
| `ImageButton2` | asp:ImageButton |  | — | `ImageButton2_Click` | 82 lines | — | — |
| `submitButton` | asp:Button | Submit | return confirm('Are you sure you want to Save ?'); | `submitButton_Click` | 14 lines | — | — |
| `cancelButton` | asp:Button | Cancel | — | `cancelButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:CalendarExtender`(stockInDateTextBox), `cc1:CalendarExtender`(challanDateTextBox), `cc1:CalendarExtender`(referenceDateTextBox), `cc1:AutoCompleteExtender`(productNameTextBox), `cc1:CalendarExtender`(mfgDateTextBox), `cc1:CalendarExtender`(expDateDateTextBox), `cc1:FilteredTextBoxExtender`(reqQtyTextBox), `cc1:FilteredTextBoxExtender`(costPriceTextBox), `cc1:FilteredTextBoxExtender`(vatTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 25 | — | — | — | `IDInformation` |
| 2 | `SetGridViewStatus` | `void SetGridViewStatus()` | 10 | — | — | — | — |
| 3 | `LoadWarehouseStockInInfo` | `void LoadWarehouseStockInInfo(string value)` | 62 | `aWarehouseStockInBll.LoadWarehouseStockInDetailInfo`, `aWarehouseStockInBll.LoadWarehouseStockInMasterInfo`, `aWarehouseStockInBll.LoadWhInformation` | — | — | `CompanyId` |
| 4 | `DropDownLoad` | `void DropDownLoad()` | 4 | `aWarehouseStockInBll.LoadCompanyDropdownList`, `aWarehouseStockInBll.LoadSupplierName` | — | — | — |
| 5 | `InitialGrid` | `void InitialGrid()` | 49 | — | — | — | — |
| 6 | `AddRowInGrid` | `void AddRowInGrid()` | 97 | — | — | — | — |
| 7 | `ImageButton1_Click` | `void ImageButton1_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 8 | `ImageButton2_Click` | `void ImageButton2_Click(object sender, ImageClickEventArgs e)` | 82 | — | — | — | — |
| 9 | `GetProductInGrid` | `void GetProductInGrid(int rowindex, string productCode)` | 76 | `aWarehouseStockInBll.ProductInfo` | — | — | — |
| 10 | `productCodeTextBox_TextChanged` | `void productCodeTextBox_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 11 | `SaveAllData` | `void SaveAllData()` | 89 | `aWarehouseStockInBll.SaveStockInMasterInfo`, `aWarehouseStockInBll.SaveWarehouseStockInDetail` | — | — | `LoginName` |
| 12 | `Validation` | `bool Validation()` | 98 | — | — | — | — |
| 13 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 14 | — | — | — | — |
| 14 | `UpdateAllData` | `void UpdateAllData()` | 79 | `aWarehouseStockInBll.DeleteWhStockInDetailById`, `aWarehouseStockInBll.UpdateStockInMasterInfo`, `aWarehouseStockInBll.UpdateWarehouseStockInDetail` | — | — | `LoginName` |
| 15 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 16 | `Clear` | `void Clear()` | 19 | — | — | — | — |
| 17 | `productNameTextBox_TextChanged` | `void productNameTextBox_TextChanged(object sender, EventArgs e)` | 37 | — | — | — | — |
| 18 | `reqQtyTextBox_OnTextChanged` | `void reqQtyTextBox_OnTextChanged(object sender, EventArgs e)` | 8 | — | — | — | — |
| 19 | `costPriceTextBox_OnTextChanged` | `void costPriceTextBox_OnTextChanged(object sender, EventArgs e)` | 8 | — | — | — | — |
| 20 | `CalculateTotalValue` | `void CalculateTotalValue(int rowindex)` | 27 | — | — | — | — |
| 21 | `CalculateQuantityVatAndValue` | `void CalculateQuantityVatAndValue()` | 38 | — | — | — | — |
| 22 | `totalValueTextBox_OnTextChanged` | `void totalValueTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 23 | `viewLinkButton_OnClick` | `void viewLinkButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 24 | `vatTextBox_OnTextChanged` | `void vatTextBox_OnTextChanged(object sender, EventArgs e)` | 8 | — | — | — | — |
| 25 | `stockInDateTextBox_OnTextChanged` | `void stockInDateTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 26 | `challanDateTextBox_OnTextChanged` | `void challanDateTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 27 | `referenceDateTextBox_OnTextChanged` | `void referenceDateTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 28 | `mfgDateTextBox_OnTextChanged` | `void mfgDateTextBox_OnTextChanged(object sender, EventArgs e)` | 34 | — | — | — | — |
| 29 | `expDateDateTextBox_OnTextChanged` | `void expDateDateTextBox_OnTextChanged(object sender, EventArgs e)` | 35 | — | — | — | — |
| 30 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 31 | `companyNameDropDownList_OnSelectedIndexChanged` | `void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 20 | `aWarehouseStockInBll.LoadWhInformation` | — | — | `CompanyId` |

Call chain shape for every persisting action on this page:

```
WarehouseStockIn.aspx (markup)
  -> SInventory_UI_WarehouseStockIn (code-behind event handler)
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
| `productNameTextBox_AutoCompleteExtender` | `productNameTextBox` | `GetProduct` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadWarehouseStockInInfo` | `WarehouseStockInBll` | `LoadWarehouseStockInDetailInfo` | `string stockInId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `LoadWarehouseStockInInfo` | `WarehouseStockInBll` | `LoadWarehouseStockInMasterInfo` | `string stockInId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `LoadWarehouseStockInInfo` | `WarehouseStockInBll` | `LoadWhInformation` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `DropDownLoad` | `WarehouseStockInBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `DropDownLoad` | `WarehouseStockInBll` | `LoadSupplierName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `GetProductInGrid` | `WarehouseStockInBll` | `ProductInfo` | `string productCode, string productId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `SaveAllData` | `WarehouseStockInBll` | `SaveStockInMasterInfo` | `WarehouseStockInMasterDao aStockInMasterDao, out Int32 maxRo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `SaveAllData` | `WarehouseStockInBll` | `SaveWarehouseStockInDetail` | `List<WharehouseStockInDetailsDao> aDetailList` | `string` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `UpdateAllData` | `WarehouseStockInBll` | `DeleteWhStockInDetailById` | `int stockInId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `UpdateAllData` | `WarehouseStockInBll` | `UpdateStockInMasterInfo` | `WarehouseStockInMasterDao aStockInMasterDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `UpdateAllData` | `WarehouseStockInBll` | `UpdateWarehouseStockInDetail` | `List<WharehouseStockInDetailsDao> aDetailList` | `string` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `WarehouseStockInBll` | `LoadWhInformation` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `DropDownLoad()` → `WarehouseStockInBll.LoadCompanyDropdownList` → `WarehouseStockInDal.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `GetProductInGrid()` → `WarehouseStockInBll.ProductInfo` → `WarehouseStockInDal.LoadProductCode` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadWarehouseStockInInfo()` → `WarehouseStockInBll.LoadWarehouseStockInDetailInfo` → `WarehouseStockInDal.GetWarehouseStockInDetailInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblSupplierInformation` | `DropDownLoad()` → `WarehouseStockInBll.LoadSupplierName` → `WarehouseStockInDal.LoadSupplierName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblWHStockInDetail` | `LoadWarehouseStockInInfo()` → `WarehouseStockInBll.LoadWarehouseStockInDetailInfo` → `WarehouseStockInDal.GetWarehouseStockInDetailInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveAllData()` → `WarehouseStockInBll.SaveWarehouseStockInDetail` → `WarehouseStockInDal.CheckDetailsExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveWarehouseStockInDetail`, `UpdateAllData()` → `WarehouseStockInBll.DeleteWhStockInDetailById` → `WarehouseStockInDal.DeleteWhStockInDetailById` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand`, `UpdateAllData()` → `WarehouseStockInBll.UpdateWarehouseStockInDetail` → `WarehouseStockInDal.SaveWarehouseStockInDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommand` |
| `tblWHStockInMaster` | `LoadWarehouseStockInInfo()` → `WarehouseStockInBll.LoadWarehouseStockInMasterInfo` → `WarehouseStockInDal.GetWarehouseStockInMasterInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveAllData()` → `WarehouseStockInBll.SaveStockInMasterInfo` → `WarehouseStockInDal.CheckEntryWithinlastMinute` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveStockInMasterInformation`, `UpdateAllData()` → `WarehouseStockInBll.UpdateStockInMasterInfo` → `WarehouseStockInDal.UpdateStockInMasterInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblWearhouse` | `LoadWarehouseStockInInfo()` → `WarehouseStockInBll.LoadWarehouseStockInMasterInfo` → `WarehouseStockInDal.GetWarehouseStockInMasterInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadWarehouseStockInInfo()` → `WarehouseStockInBll.LoadWhInformation` → `WarehouseStockInDal.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue`, `companyNameDropDownList_OnSelectedIndexChanged()` → `WarehouseStockInBll.LoadWhInformation` → `WarehouseStockInDal.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `LoadWarehouseStockInInfo` | Update | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `Clear` | Save | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtenderQuantity` | `reqQtyTextBox` | Custom | 0123456789. |
| `FilteredTextBoxExtenderPrice` | `costPriceTextBox` | Custom | 0123456789. |
| `FilteredTextBoxExtenderVat` | `vatTextBox` | Custom | 0123456789. |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-INV-034-001` | Update | Code | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs` → `LoadWarehouseStockInInfo()` | user action on this page | operation is refused | "Update" | CONFIRMED |
| `BR-INV-034-002` | {0} | Code | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-INV-034-003` | Save | Code | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs` → `Clear()` | user action on this page | operation is refused | "Save" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `CompanyId`, `IDInformation`, `LoginName`

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
| Assembly source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblProduct` |
| Database object | `tblSupplierInformation` |
| Database object | `tblWHStockInDetail` |
| Database object | `tblWHStockInMaster` |
| Database object | `tblWearhouse` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-034-001` | Unauthenticated access | Request `/SInventory_UI/WarehouseStockIn.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-034-002` | Validation: Update | Trigger `LoadWarehouseStockInInfo()` in the state that produces this message | alert "Update" and the operation is not performed | CONFIRMED |
| `TC-INV-034-003` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-INV-034-004` | Validation: Save | Trigger `Clear()` in the state that produces this message | alert "Save" and the operation is not performed | CONFIRMED |
| `TC-INV-034-005` | Grid loads | Open the page with data present | Grid `productGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 63 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 31 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 6 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` is **shared with 11 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `MaterialConversionReceive.aspx`, `MaterialConversionReceiveView.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblSupplierInformation` — any schema change to it breaks this page.
- Writes/reads `tblWHStockInDetail` — any schema change to it breaks this page.
- Writes/reads `tblWHStockInMaster` — any schema change to it breaks this page.
- Writes/reads `tblWearhouse` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
