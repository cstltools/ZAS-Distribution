# INV-012 — MaterialConversionReceive

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-012` |
| Module | `inventory-warehouse` |
| File name | `MaterialConversionReceive.aspx` |
| Route | `/SInventory_UI/MaterialConversionReceive.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `MaterialConversionReceive.aspx.cs` |
| Code-behind class | `SInventory_UI_MaterialConversionReceive` |
| Markup lines | 439 |
| Code-behind lines | 692 |
| Server controls | 64 |
| GridViews | 1 |
| Code-behind methods | 28 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/MaterialConversionReceiveView.aspx`

Outbound `Response.Redirect` targets: `MaterialConversionReceiveView.aspx`, `WarehouseStockInView.aspx`

## 5. Route

`/SInventory_UI/MaterialConversionReceive.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |

## 7. UI Control Inventory

64 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 9 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 12 |
| 4 | `viewLinkButton` | asp:LinkButton |  |  |  |  |  | viewLinkButton_OnClick | 35 |
| 5 | `ddlCompany` | asp:DropDownList | Company Name: |  | True |  |  | ddlCompany_OnSelectedIndexChanged | 61 |
| 6 | `warehouseDropDownList` | asp:DropDownList | Warehouse: |  |  |  |  |  | 68 |
| 7 | `stockInIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 71 |
| 8 | `stockInDateTextBox` | asp:TextBox | Stock In Date: |  | True |  |  | stockInDateTextBox_OnTextChanged | 76 |
| 9 | `imgDate` | asp:ImageButton | tock In Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 78 |
| 10 | `manufacturerDate1` | cc1:CalendarExtender |  |  |  |  |  |  | 80 |
| 11 | `challanNoTextBox` | asp:TextBox | Challan No: |  |  |  |  |  | 89 |
| 12 | `challanDateTextBox` | asp:TextBox | Challan Date: |  | True |  |  | challanDateTextBox_OnTextChanged | 94 |
| 13 | `ImageButton4` | asp:ImageButton | Challan Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 96 |
| 14 | `CalendarExtender2` | cc1:CalendarExtender |  |  |  |  |  |  | 98 |
| 15 | `referenceNoTextBox` | asp:TextBox | Reference No: |  |  |  |  |  | 105 |
| 16 | `referenceDateTextBox` | asp:TextBox | Reference Date: |  | True |  |  | referenceDateTextBox_OnTextChanged | 112 |
| 17 | `ImageButton5` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 114 |
| 18 | `CalendarExtender3` | cc1:CalendarExtender |  |  |  |  |  |  | 116 |
| 19 | `remarksTextBox` | asp:TextBox | Remarks: |  |  |  |  |  | 123 |
| 20 | `ddlSupplier` | asp:DropDownList |  |  |  |  | False |  | 129 |
| 21 | `ddlProductionUnit` | asp:DropDownList | Production Unit |  | True |  |  | ddlProductionUnit_OnSelectedIndexChanged | 140 |
| 22 | `loadGridView` | asp:GridView |  |  |  |  |  | loadGridView_RowCommand | 162 |
| 23 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 165 |
| 24 | `LabelSL` | asp:Label | loadGridView → col[0] "SL" |  |  |  |  |  | 167 |
| 25 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 170 |
| 26 | `chkSelectAll` | asp:CheckBox | loadGridView → col[1] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 172 |
| 27 | `chkSelect` | asp:CheckBox | loadGridView → col[1] "(no header)" |  | True |  |  | chkSelect_OnCheckedChanged | 175 |
| 28 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 178 |
| 29 | `productCodeTextBox` | asp:TextBox | loadGridView → col[2] "Product Code" |  |  | ReadOnly=True |  |  | 180 |
| 30 | `productidHiddenField` | asp:HiddenField | loadGridView → col[2] "Product Code" |  |  |  |  |  | 181 |
| 31 | `hdfMaterialConversionMasterId` | asp:HiddenField | loadGridView → col[2] "Product Code" |  |  |  |  |  | 182 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 185 |
| 33 | `productNameTextBox` | asp:TextBox | loadGridView → col[3] "Product Name" |  |  | ReadOnly=True |  |  | 187 |
| 34 | `productNameTextBox_AutoCompleteExtender` | cc1:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 189 |
| 35 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 200 |
| 36 | `packSizeTextBox` | asp:TextBox | loadGridView → col[4] "Pack Size" |  |  | ReadOnly=True |  |  | 202 |
| 37 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 205 |
| 38 | `batchTextBox` | asp:TextBox | loadGridView → col[5] "Batch" |  |  |  |  |  | 207 |
| 39 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 210 |
| 40 | `mfgDateTextBox` | asp:TextBox | loadGridView → col[6] "Mfg. Date" |  | True |  |  | mfgDateTextBox_OnTextChanged | 212 |
| 41 | `ImageButton15` | asp:ImageButton | loadGridView → col[6] "Mfg. Date" | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 214 |
| 42 | `manufacturerDate` | cc1:CalendarExtender |  |  |  |  |  |  | 216 |
| 43 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 221 |
| 44 | `expDateDateTextBox` | asp:TextBox | loadGridView → col[7] "Exp. Date" |  | True |  |  | expDateDateTextBox_OnTextChanged | 223 |
| 45 | `ImageButton25` | asp:ImageButton | loadGridView → col[7] "Exp. Date" | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 226 |
| 46 | `ExpDate` | cc1:CalendarExtender |  |  |  |  |  |  | 228 |
| 47 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 233 |
| 48 | `reqQtyTextBox` | asp:TextBox | loadGridView → col[8] "Qty" |  | True | ReadOnly=True |  | reqQtyTextBox_OnTextChanged | 235 |
| 49 | `FilteredTextBoxExtenderQuantity` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 237 |
| 50 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 242 |
| 51 | `costPriceTextBox` | asp:TextBox | loadGridView → col[9] "Price" |  | True | ReadOnly=True |  | costPriceTextBox_OnTextChanged | 244 |
| 52 | `FilteredTextBoxExtenderPrice` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 246 |
| 53 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 251 |
| 54 | `vatTextBox` | asp:TextBox | loadGridView → col[10] "Vat (Per Unit)" |  | True | ReadOnly=True |  | vatTextBox_OnTextChanged | 253 |
| 55 | `FilteredTextBoxExtenderVat` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 255 |
| 56 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 260 |
| 57 | `totalValueTextBox` | asp:TextBox | loadGridView → col[11] "Total Amt" |  | True | ReadOnly=True |  | totalValueTextBox_OnTextChanged | 262 |
| 58 | `totalQtyTextBox` | asp:TextBox | Total Qty : |  |  | ReadOnly=True |  |  | 310 |
| 59 | `totalVatTextBox` | asp:TextBox | Total Vat: |  |  | ReadOnly=True |  |  | 328 |
| 60 | `grandTotalTextBox` | asp:TextBox | Grand Total: |  |  | ReadOnly=True |  |  | 346 |
| 61 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 377 |
| 62 | `cancelButton` | asp:Button |  | Cancel |  |  |  | cancelButton_Click | 378 |
| 63 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 420 |
| 64 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 423 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `MaterialConversionMasterId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | SL | — | — | `Label:LabelSL` | — |
| 1 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |
| 2 | TemplateField | Product Code | — | `MaterialConversionMasterId`, `ProductCode`, `ProductId` | `TextBox:productCodeTextBox`, `HiddenField:productidHiddenField`, `HiddenField:hdfMaterialConversionMasterId` | — |
| 3 | TemplateField | Product Name | — | `ProductName` | `TextBox:productNameTextBox` | — |
| 4 | TemplateField | Pack Size | — | `PackSize` | `TextBox:packSizeTextBox` | — |
| 5 | TemplateField | Batch | — | `Batch_No` | `TextBox:batchTextBox` | — |
| 6 | TemplateField | Mfg. Date | — | `Mfg_Date` | `TextBox:mfgDateTextBox`, `ImageButton:ImageButton15` | — |
| 7 | TemplateField | Exp. Date | — | `Exp_Date` | `TextBox:expDateDateTextBox`, `ImageButton:ImageButton25` | — |
| 8 | TemplateField | Qty | — | `FinalOutputQuantityInt` | `TextBox:reqQtyTextBox` | — |
| 9 | TemplateField | Price | — | `CostPerUnit` | `TextBox:costPriceTextBox` | — |
| 10 | TemplateField | Vat (Per Unit) | — | `Vat` | `TextBox:vatTextBox` | — |
| 11 | TemplateField | Total Amt | — | `TotalCostOfThisBatch` | `TextBox:totalValueTextBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `WarehouseStockInMasterDao`, `WharehouseStockInDetailsDao`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropdownlist` | `aReceiveDal.LoadCompany(...)` | `CompanywisebranchDal.LoadCompany(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` | `CompanywisebranchDal.LoadCompany` → `ClsCommonInternalDAL.LoadDropDownValue` → `InvoiceDAL.LoadCompany` → `MaterialConversionReceiveDal.LoadCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `InvoiceBLL.LoadCompany` → `PettyCashBLL.LoadCompany` → `PettyCashDaL.LoadCompanydropdown` | SELECT | `tblCompanyInfo` | — |
| `LoadDropdownlist` | `aWarehouseStockInBll.LoadSupplierName(...)` | `WarehouseStockInBll.LoadSupplierName(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadSupplierName` → `WarehouseStockInDal.LoadSupplierName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblSupplierInformation` | — |
| `LoadPendingMaterialConversionList` | `aReceiveDal.LoadMaterialConversionList(...)` | `MaterialConversionReceiveDal.LoadMaterialConversionList(string param)` <br/>`Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` | `MaterialConversionReceiveDal.LoadMaterialConversionList` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblPackSize`, `tblProduct`, `tblWHStockInDetail`, `tblWHStockInMaster`, `tbl_MC_ProductRawMaterialDetails` | — |
| `SaveAllData` | `aWarehouseStockInBll.SaveStockInMasterInfo(...)` | `WarehouseStockInBll.SaveStockInMasterInfo(WarehouseStockInMasterDao aStockInMasterDao, out Int32 maxRowId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.SaveStockInMasterInfo` → `WarehouseStockInDal.CheckEntryWithinlastMinute` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveStockInMasterInformation` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `WarehouseStockInBll.StockInId` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblWHStockInMaster` | — |
| `SaveAllData` | `aWarehouseStockInBll.SaveWarehouseStockInDetail(...)` | `WarehouseStockInBll.SaveWarehouseStockInDetail(List<WharehouseStockInDetailsDao> aDetailList)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.SaveWarehouseStockInDetail` → `WarehouseStockInDal.CheckDetailsExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveWarehouseStockInDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT, SELECT | `tblWHStockInDetail` | — |
| `UpdateAllData` | `aWarehouseStockInBll.DeleteWhStockInDetailById(...)` | `WarehouseStockInBll.DeleteWhStockInDetailById(int stockInId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.DeleteWhStockInDetailById` → `WarehouseStockInDal.DeleteWhStockInDetailById` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` | DELETE | `tblWHStockInDetail` | — |
| `UpdateAllData` | `aWarehouseStockInBll.UpdateStockInMasterInfo(...)` | `WarehouseStockInBll.UpdateStockInMasterInfo(WarehouseStockInMasterDao aStockInMasterDao)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.UpdateStockInMasterInfo` → `WarehouseStockInDal.UpdateStockInMasterInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblWHStockInMaster` | — |
| `UpdateAllData` | `aWarehouseStockInBll.UpdateWarehouseStockInDetail(...)` | `WarehouseStockInBll.UpdateWarehouseStockInDetail(List<WharehouseStockInDetailsDao> aDetailList)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.UpdateWarehouseStockInDetail` → `WarehouseStockInDal.SaveWarehouseStockInDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT | `tblWHStockInDetail` | — |
| `ddlCompany_OnSelectedIndexChanged` | `aReceiveDal.GetProductionUnit(...)` | `MaterialConversionReceiveDal.GetProductionUnit(DropDownList ddl, string businessUnitId)` <br/>`Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` | `MaterialConversionReceiveDal.GetProductionUnit` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | — | — |
| `ddlCompany_OnSelectedIndexChanged` | `aWarehouseStockInBll.LoadWhInformation(...)` | `WarehouseStockInBll.LoadWhInformation(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadWhInformation` → `WarehouseStockInDal.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblWearhouse` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `ddlCompany` | Company Name: | asp:DropDownList | True | ddlCompany_OnSelectedIndexChanged | `aReceiveDal.GetProductionUnit()` in `ddlCompany_OnSelectedIndexChanged()`; `aReceiveDal.LoadCompany()` in `LoadDropdownlist()`; `aWarehouseStockInBll.LoadWhInformation()` in `ddlCompany_OnSelectedIndexChanged()` | `tblCompanyInfo`, `tblWearhouse` | set in code (see the DAL method above) |
| `warehouseDropDownList` | Warehouse: | asp:DropDownList | False | — | `aWarehouseStockInBll.LoadWhInformation()` in `ddlCompany_OnSelectedIndexChanged()` | `tblWearhouse` | set in code (see the DAL method above) |
| `ddlSupplier` |  | asp:DropDownList | False | — | `aWarehouseStockInBll.LoadSupplierName()` in `LoadDropdownlist()` | `tblSupplierInformation` | set in code (see the DAL method above) |
| `ddlProductionUnit` | Production Unit | asp:DropDownList | True | ddlProductionUnit_OnSelectedIndexChanged | `aReceiveDal.GetProductionUnit()` in `ddlCompany_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |

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
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `stockInDateTextBox_OnTextChanged` | `void stockInDateTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 3 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 4 | `challanDateTextBox_OnTextChanged` | `void challanDateTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 5 | `referenceDateTextBox_OnTextChanged` | `void referenceDateTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 6 | `LoadDropdownlist` | `void LoadDropdownlist()` | 5 | `aReceiveDal.LoadCompany`, `aWarehouseStockInBll.LoadSupplierName` | — | — | — |
| 7 | `LoadPendingMaterialConversionList` | `void LoadPendingMaterialConversionList()` | 22 | `aReceiveDal.LoadMaterialConversionList` | — | — | — |
| 8 | `mfgDateTextBox_OnTextChanged` | `void mfgDateTextBox_OnTextChanged(object sender, EventArgs e)` | 34 | — | — | — | — |
| 9 | `expDateDateTextBox_OnTextChanged` | `void expDateDateTextBox_OnTextChanged(object sender, EventArgs e)` | 35 | — | — | — | — |
| 10 | `reqQtyTextBox_OnTextChanged` | `void reqQtyTextBox_OnTextChanged(object sender, EventArgs e)` | 8 | — | — | — | — |
| 11 | `costPriceTextBox_OnTextChanged` | `void costPriceTextBox_OnTextChanged(object sender, EventArgs e)` | 8 | — | — | — | — |
| 12 | `CalculateTotalValue` | `void CalculateTotalValue(int rowindex)` | 27 | — | — | — | — |
| 13 | `CalculateQuantityVatAndValue` | `void CalculateQuantityVatAndValue()` | 51 | — | — | — | — |
| 14 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 14 | — | — | — | — |
| 15 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 16 | `Validation` | `bool Validation()` | 104 | — | — | — | — |
| 17 | `SaveAllData` | `void SaveAllData()` | 86 | `aWarehouseStockInBll.SaveStockInMasterInfo`, `aWarehouseStockInBll.SaveWarehouseStockInDetail` | — | — | `LoginName` |
| 18 | `UpdateAllData` | `void UpdateAllData()` | 85 | `aWarehouseStockInBll.DeleteWhStockInDetailById`, `aWarehouseStockInBll.UpdateStockInMasterInfo`, `aWarehouseStockInBll.UpdateWarehouseStockInDetail` | — | — | `LoginName` |
| 19 | `Clear` | `void Clear()` | 22 | — | — | — | — |
| 20 | `totalValueTextBox_OnTextChanged` | `void totalValueTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 21 | `GenerateParameter` | `string GenerateParameter()` | 16 | — | — | — | — |
| 22 | `vatTextBox_OnTextChanged` | `void vatTextBox_OnTextChanged(object sender, EventArgs e)` | 8 | — | — | — | — |
| 23 | `loadGridView_RowCommand` | `void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 3 | — | — | — | — |
| 24 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 20 | — | — | — | — |
| 25 | `ddlCompany_OnSelectedIndexChanged` | `void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)` | 11 | `aReceiveDal.GetProductionUnit`, `aWarehouseStockInBll.LoadWhInformation` | — | — | — |
| 26 | `ddlProductionUnit_OnSelectedIndexChanged` | `void ddlProductionUnit_OnSelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 27 | `chkSelect_OnCheckedChanged` | `void chkSelect_OnCheckedChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 28 | `viewLinkButton_OnClick` | `void viewLinkButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
MaterialConversionReceive.aspx (markup)
  -> SInventory_UI_MaterialConversionReceive (code-behind event handler)
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
| `LoadDropdownlist` | `InvoiceBLL` | `LoadCompany` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoadDropdownlist` | `PettyCashBLL` | `LoadCompany` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `LoadDropdownlist` | `WarehouseStockInBll` | `LoadSupplierName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `SaveAllData` | `WarehouseStockInBll` | `SaveStockInMasterInfo` | `WarehouseStockInMasterDao aStockInMasterDao, out Int32 maxRo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `SaveAllData` | `WarehouseStockInBll` | `SaveWarehouseStockInDetail` | `List<WharehouseStockInDetailsDao> aDetailList` | `string` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `UpdateAllData` | `WarehouseStockInBll` | `DeleteWhStockInDetailById` | `int stockInId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `UpdateAllData` | `WarehouseStockInBll` | `UpdateStockInMasterInfo` | `WarehouseStockInMasterDao aStockInMasterDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `UpdateAllData` | `WarehouseStockInBll` | `UpdateWarehouseStockInDetail` | `List<WharehouseStockInDetailsDao> aDetailList` | `string` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `ddlCompany_OnSelectedIndexChanged` | `WarehouseStockInBll` | `LoadWhInformation` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropdownlist` | `CompanywisebranchDal` | `LoadCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` |
| `LoadDropdownlist` | `InvoiceDAL` | `LoadCompany` | `DropDownList aDropDownList` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| `LoadDropdownlist` | `MaterialConversionReceiveDal` | `LoadCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |
| `LoadPendingMaterialConversionList` | `MaterialConversionReceiveDal` | `LoadMaterialConversionList` | `string param` | `DataTable` | SELECT | `tblPackSize`, `tblProduct`, `tblWHStockInDetail`, `tblWHStockInMaster`, `tbl_MC_ProductRawMaterialDetails` | — | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |
| `ddlCompany_OnSelectedIndexChanged` | `MaterialConversionReceiveDal` | `GetProductionUnit` | `DropDownList ddl, string businessUnitId` | `void` | SELECT | — | — | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadDropdownlist()` → `CompanywisebranchDal.LoadCompany` → `ClsCommonInternalDAL.LoadDropDownValue` → `InvoiceDAL.LoadCompany` → `MaterialConversionReceiveDal.LoadCompany` |
| `tblPackSize` | `LoadPendingMaterialConversionList()` → `MaterialConversionReceiveDal.LoadMaterialConversionList` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProduct` | `LoadPendingMaterialConversionList()` → `MaterialConversionReceiveDal.LoadMaterialConversionList` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblSupplierInformation` | `LoadDropdownlist()` → `WarehouseStockInBll.LoadSupplierName` → `WarehouseStockInDal.LoadSupplierName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblWHStockInDetail` | `LoadPendingMaterialConversionList()` → `MaterialConversionReceiveDal.LoadMaterialConversionList` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveAllData()` → `WarehouseStockInBll.SaveWarehouseStockInDetail` → `WarehouseStockInDal.CheckDetailsExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveWarehouseStockInDetail`, `UpdateAllData()` → `WarehouseStockInBll.DeleteWhStockInDetailById` → `WarehouseStockInDal.DeleteWhStockInDetailById` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand`, `UpdateAllData()` → `WarehouseStockInBll.UpdateWarehouseStockInDetail` → `WarehouseStockInDal.SaveWarehouseStockInDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommand` |
| `tblWHStockInMaster` | `LoadPendingMaterialConversionList()` → `MaterialConversionReceiveDal.LoadMaterialConversionList` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveAllData()` → `WarehouseStockInBll.SaveStockInMasterInfo` → `WarehouseStockInDal.CheckEntryWithinlastMinute` → `ClsCommonInternalDAL.DataContainerDataTable` → `WarehouseStockInDal.SaveStockInMasterInformation`, `UpdateAllData()` → `WarehouseStockInBll.UpdateStockInMasterInfo` → `WarehouseStockInDal.UpdateStockInMasterInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblWearhouse` | `ddlCompany_OnSelectedIndexChanged()` → `WarehouseStockInBll.LoadWhInformation` → `WarehouseStockInDal.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tbl_MC_ProductRawMaterialDetails` | `LoadPendingMaterialConversionList()` → `MaterialConversionReceiveDal.LoadMaterialConversionList` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
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
| `BR-INV-012-001` | {0} | Code | `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-INV-012-002` | Save | Code | `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx.cs` → `Clear()` | user action on this page | operation is refused | "Save" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblPackSize` |
| Database object | `tblProduct` |
| Database object | `tblSupplierInformation` |
| Database object | `tblWHStockInDetail` |
| Database object | `tblWHStockInMaster` |
| Database object | `tblWearhouse` |
| Database object | `tbl_MC_ProductRawMaterialDetails` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-012-001` | Unauthenticated access | Request `/SInventory_UI/MaterialConversionReceive.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-012-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-INV-012-003` | Validation: Save | Trigger `Clear()` in the state that produces this message | alert "Save" and the operation is not performed | CONFIRMED |
| `TC-INV-012-004` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
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
| UI controls | absent | 64 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 28 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 8 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` is **shared with 11 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `MaterialConversionReceiveView.aspx`, `NonTranscationalInvoiceReturn.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` is **shared with 3 other page(s)** (e.g. `CreditCustomerReport.aspx`, `PrettyCashEntry.aspx`, `ProductDiscountReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs` is **shared with 9 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CreditCustomerReport.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` is **shared with 4 other page(s)** (e.g. `CreditCustomerReport.aspx`, `PrettyCashEntry.aspx`, `ProductDiscountReport.aspx`, `WeaightedAVG_Report.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblPackSize` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblSupplierInformation` — any schema change to it breaks this page.
- Writes/reads `tblWHStockInDetail` — any schema change to it breaks this page.
- Writes/reads `tblWHStockInMaster` — any schema change to it breaks this page.
- Writes/reads `tblWearhouse` — any schema change to it breaks this page.
- Writes/reads `tbl_MC_ProductRawMaterialDetails` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
