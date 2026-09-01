# SAL-010 — InvoiceCreationForCustomer

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-010` |
| Module | `sales-order-invoice` |
| File name | `InvoiceCreationForCustomer.aspx` |
| Route | `/SInventory_UI/InvoiceCreationForCustomer.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `InvoiceCreationForCustomer.aspx.cs` |
| Code-behind class | `SInventory_UI_InvoiceCreationForCustomer` |
| Markup lines | 461 |
| Code-behind lines | 596 |
| Server controls | 76 |
| GridViews | 1 |
| Code-behind methods | 22 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/InvoiceCreationForCustomer.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |

## 7. UI Control Inventory

76 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `custCodeTextBox` | asp:TextBox | Customer Code: |  | True |  |  | custCodeTextBox_TextChanged | 20 |
| 5 | `custNameTextBox` | asp:TextBox | Customer Name: |  | True |  |  | custNameTextBox_TextChanged | 27 |
| 6 | `custNameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 30 |
| 7 | `custAddressTextBox` | asp:TextBox | Customer Address: |  |  | ReadOnly=True |  |  | 47 |
| 8 | `comUnitNameTextBox` | asp:TextBox | Salse Center: |  |  | ReadOnly=True |  |  | 55 |
| 9 | `miaCodeTextBox` | asp:TextBox | MIA Code: |  |  | ReadOnly=True |  |  | 61 |
| 10 | `miaNameTextBox` | asp:TextBox | MIA Name: |  |  | ReadOnly=True |  |  | 67 |
| 11 | `districtNameTextBox` | asp:TextBox | Distirict: |  |  | ReadOnly=True |  |  | 76 |
| 12 | `areaNameTextBox` | asp:TextBox | Area: |  |  | ReadOnly=True |  |  | 82 |
| 13 | `marketNameTextBox` | asp:TextBox | Merket: |  |  | ReadOnly=True |  |  | 88 |
| 14 | `custCategoryTextBox` | asp:TextBox | Customer Category: |  |  | ReadOnly=True |  |  | 96 |
| 15 | `invDateTextBox` | asp:TextBox | Invoice Date: |  |  | ReadOnly=True |  |  | 113 |
| 16 | `orderNoTextBox` | asp:TextBox | Order No: |  |  |  |  |  | 119 |
| 17 | `orderDateTextBox` | asp:TextBox | Order Date: |  |  |  |  |  | 124 |
| 18 | `imgorderDate` | asp:ImageButton | Order Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 125 |
| 19 | `orderDate` | asp:CalendarExtender |  |  |  |  |  |  | 127 |
| 20 | `payTypeDDL` | asp:DropDownList | Payment Type: |  |  |  |  |  | 148 |
| 21 | `gridLineItemGridView` | asp:GridView |  |  |  |  |  |  | 168 |
| 22 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 171 |
| 23 | `slLabel` | asp:Label | gridLineItemGridView → col[0] "SL" | {{Eval('SL')}} |  |  |  |  | 173 |
| 24 | `addImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineAdd.png |  |  |  | addImageButton_Click | 174 |
| 25 | `removeImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineDelete.png |  |  |  | removeImageButton_Click | 176 |
| 26 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 180 |
| 27 | `codeTextBox` | asp:TextBox | gridLineItemGridView → col[1] "Code" | {{Eval('ProductCode')}} | True |  |  | codeTextBox_TextChanged | 182 |
| 28 | `fcodeTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 185 |
| 29 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 191 |
| 30 | `nameTextBox` | asp:TextBox | gridLineItemGridView → col[2] "Name" | {{Eval('ProductName')}} | True |  |  | nameTextBox_TextChanged | 193 |
| 31 | `nameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 196 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 209 |
| 33 | `currentStockTextBox` | asp:TextBox | gridLineItemGridView → col[3] "CStock" | {{Eval('StockQty')}} |  | ReadOnly=True |  |  | 211 |
| 34 | `fcurrentStockTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 213 |
| 35 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 219 |
| 36 | `unitPriceTextBox` | asp:TextBox | gridLineItemGridView → col[4] "UP" | {{Eval('UnitPrice')}} |  | ReadOnly=True |  |  | 221 |
| 37 | `funitPriceTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 223 |
| 38 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 229 |
| 39 | `upVatTextBox` | asp:TextBox | gridLineItemGridView → col[5] "UVAT" | {{Eval('UnitVAT')}} |  | ReadOnly=True |  |  | 231 |
| 40 | `fupVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 233 |
| 41 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 239 |
| 42 | `qtyTextBox` | asp:TextBox | gridLineItemGridView → col[6] "Qty" | {{Eval('Quantity')}} | True |  |  | qtyTextBox_TextChanged | 241 |
| 43 | `fqtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 244 |
| 44 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 251 |
| 45 | `tpTextBox` | asp:TextBox | gridLineItemGridView → col[7] "TP" | {{Eval('TotalPrice')}} |  | ReadOnly=True |  |  | 253 |
| 46 | `ftpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 255 |
| 47 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 261 |
| 48 | `tpVatTextBox` | asp:TextBox | gridLineItemGridView → col[8] "TPVAT" | {{Eval('VAT')}} |  | ReadOnly=True |  |  | 263 |
| 49 | `ftpVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 265 |
| 50 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 272 |
| 51 | `dpTextBox` | asp:TextBox | gridLineItemGridView → col[9] "DP" | {{Eval('DiscountPercentage')}} |  | ReadOnly=True |  |  | 274 |
| 52 | `fdpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 276 |
| 53 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 283 |
| 54 | `dpAmtTextBox` | asp:TextBox | gridLineItemGridView → col[10] "DAmt" | {{Eval('DiscountAmount')}} |  | ReadOnly=True |  |  | 285 |
| 55 | `fdpAmtTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 287 |
| 56 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 293 |
| 57 | `npTextBox` | asp:TextBox | gridLineItemGridView → col[11] "NP" | {{Eval('NetPrice')}} |  | ReadOnly=True |  |  | 295 |
| 58 | `fnpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 297 |
| 59 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 303 |
| 60 | `bQtyTextBox` | asp:TextBox | gridLineItemGridView → col[12] "BQty" | {{Eval('BonusQty')}} | True | ReadOnly=True |  | bQtyTextBox_TextChanged | 305 |
| 61 | `fbQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 308 |
| 62 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 314 |
| 63 | `tQtyTextBox` | asp:TextBox | gridLineItemGridView → col[13] "TQty" | {{Eval('TotalQty')}} |  | ReadOnly=True |  |  | 316 |
| 64 | `ftQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 318 |
| 65 | `tpTptalTextBox` | asp:TextBox | TP Total |  |  | ReadOnly=True |  |  | 337 |
| 66 | `vatTotalTextBox` | asp:TextBox | VAT Total |  |  | ReadOnly=True |  |  | 353 |
| 67 | `disTotalTextBox` | asp:TextBox | Discount Total |  |  | ReadOnly=True |  |  | 369 |
| 68 | `grandTotalTextBox` | asp:TextBox | Grand Total |  |  | ReadOnly=True |  |  | 385 |
| 69 | `saveButton` | asp:Button |  | Invoice Generate |  |  |  | saveButton_Click | 403 |
| 70 | `invTextBox` | asp:TextBox | Print InvNo |  |  |  |  |  | 417 |
| 71 | `printButton` | asp:Button | Print InvNo | Print |  |  |  | printButton_Click | 420 |
| 72 | `hdCustomerMasterId` | asp:HiddenField |  |  |  |  |  |  | 428 |
| 73 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 429 |
| 74 | `hdMiaId` | asp:HiddenField |  |  |  |  |  |  | 433 |
| 75 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 449 |
| 76 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 452 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `gridLineItemGridView`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | SL | — | `SL` | `Label:slLabel`, `ImageButton:addImageButton`, `ImageButton:removeImageButton` | — |
| 1 | TemplateField | Code | — | `ProductCode` | `TextBox:codeTextBox`, `FilteredTextBoxExtender:codeTextBox` | — |
| 2 | TemplateField | Name | — | `ProductName` | `TextBox:nameTextBox`, `AutoCompleteExtender:nameTextBox` | — |
| 3 | TemplateField | CStock | — | `StockQty` | `TextBox:currentStockTextBox`, `FilteredTextBoxExtender:currentStockTextBox` | — |
| 4 | TemplateField | UP | — | `UnitPrice` | `TextBox:unitPriceTextBox`, `FilteredTextBoxExtender:unitPriceTextBox` | — |
| 5 | TemplateField | UVAT | — | `UnitVAT` | `TextBox:upVatTextBox`, `FilteredTextBoxExtender:upVatTextBox` | — |
| 6 | TemplateField | Qty | — | `Quantity` | `TextBox:qtyTextBox`, `FilteredTextBoxExtender:qtyTextBox` | — |
| 7 | TemplateField | TP | — | `TotalPrice` | `TextBox:tpTextBox`, `FilteredTextBoxExtender:tpTextBox` | — |
| 8 | TemplateField | TPVAT | — | `VAT` | `TextBox:tpVatTextBox`, `FilteredTextBoxExtender:tpVatTextBox` | — |
| 9 | TemplateField | DP | — | `DiscountPercentage` | `TextBox:dpTextBox`, `FilteredTextBoxExtender:dpTextBox` | — |
| 10 | TemplateField | DAmt | — | `DiscountAmount` | `TextBox:dpAmtTextBox`, `FilteredTextBoxExtender:dpAmtTextBox` | — |
| 11 | TemplateField | NP | — | `NetPrice` | `TextBox:npTextBox`, `FilteredTextBoxExtender:npTextBox` | — |
| 12 | TemplateField | BQty | — | `BonusQty` | `TextBox:bQtyTextBox`, `FilteredTextBoxExtender:bQtyTextBox` | — |
| 13 | TemplateField | TQty | — | `TotalQty` | `TextBox:tQtyTextBox`, `FilteredTextBoxExtender:tQtyTextBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `Invoice`, `InvoiceDetail`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.PaymentTypeLoadBLL(...)` | `InvoiceBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `GetCustInfo` | `aInvoiceBll.CustomerMaster(...)` | `InvoiceBLL.CustomerMaster(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — |
| `GetProduct` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `qtyTextBox_TextChanged` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.SaveInvoiceDetails(...)` | `InvoiceBLL.SaveInvoiceDetails(List<InvoiceDetail> aInvoiceList,string comUnitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.UpdateDCStoreQuantity` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `InvoiceBLL.InvoiceDetailInsertBLL` → `ClsPrimaryKeyFind.PrimaryKeyMax` | INSERT, SELECT, UPDATE | `tblDCStore`, `tblInvoiceDetail` | — |
| `SaveInvoice` | `aInvoiceBll.SaveInvoice(...)` | `InvoiceBLL.SaveInvoice(Invoice aInvoice, out int invoiceId, out string invoiceNo)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoice` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.InvoiceNoGenerator` → `InvoiceDAL.InvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblInvoice` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `payTypeDDL` | Payment Type: | asp:DropDownList | False | — | `aInvoiceBll.PaymentTypeLoadBLL()` in `Page_Load()` | `tblPaymentType` | set in code (see the DAL method above) |

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
| `imgorderDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `addImageButton` | asp:ImageButton |  | — | `addImageButton_Click` | 74 lines | — | — |
| `removeImageButton` | asp:ImageButton |  | — | `removeImageButton_Click` | 66 lines | — | — |
| `saveButton` | asp:Button | Invoice Generate | — | `saveButton_Click` | 9 lines | — | — |
| `printButton` | asp:Button | Print | — | `printButton_Click` | 6 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 8 | `aInvoiceBll.PaymentTypeLoadBLL` | — | — | — |
| 2 | `Todate` | `void Todate()` | 3 | — | — | — | — |
| 3 | `Validation` | `bool Validation()` | 27 | — | — | — | — |
| 4 | `GetCustInfo` | `void GetCustInfo(string custCode)` | 27 | `aInvoiceBll.CustomerMaster` | — | — | — |
| 5 | `custCodeTextBox_TextChanged` | `void custCodeTextBox_TextChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 6 | `InitialGrid` | `void InitialGrid()` | 51 | — | — | — | — |
| 7 | `addImageButton_Click` | `void addImageButton_Click(object sender, ImageClickEventArgs e)` | 74 | — | — | — | — |
| 8 | `removeImageButton_Click` | `void removeImageButton_Click(object sender, ImageClickEventArgs e)` | 66 | — | — | — | — |
| 9 | `codeTextBox_TextChanged` | `void codeTextBox_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 10 | `GetProduct` | `void GetProduct(int rowindex, string productCode)` | 39 | `aInvoiceBll.ProductInfo` | — | — | — |
| 11 | `ProductCodeValidation` | `bool ProductCodeValidation(string productCode, int rowindex)` | 16 | — | — | — | — |
| 12 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 13 | `qtyTextBox_TextChanged` | `void qtyTextBox_TextChanged(object sender, EventArgs e)` | 43 | `aInvoiceBll.ProductDiscount` | — | — | — |
| 14 | `bQtyTextBox_TextChanged` | `void bQtyTextBox_TextChanged(object sender, EventArgs e)` | 14 | — | — | — | — |
| 15 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 9 | — | — | — | — |
| 16 | `SaveInvoiceDetail` | `bool SaveInvoiceDetail(int invoiceId)` | 29 | `aInvoiceBll.SaveInvoiceDetails` | — | — | — |
| 17 | `SaveInvoice` | `int SaveInvoice()` | 30 | `aInvoiceBll.SaveInvoice` | — | — | `UserId` |
| 18 | `Clear` | `void Clear()` | 22 | — | — | — | — |
| 19 | `TotalValueCalculation` | `void TotalValueCalculation()` | 21 | — | — | — | — |
| 20 | `printButton_Click` | `void printButton_Click(object sender, EventArgs e)` | 6 | — | — | — | — |
| 21 | `custNameTextBox_TextChanged` | `void custNameTextBox_TextChanged(object sender, EventArgs e)` | 15 | — | — | — | — |
| 22 | `nameTextBox_TextChanged` | `void nameTextBox_TextChanged(object sender, EventArgs e)` | 25 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
InvoiceCreationForCustomer.aspx (markup)
  -> SInventory_UI_InvoiceCreationForCustomer (code-behind event handler)
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
| `custNameTextBox_AutoCompleteExtender` | `custNameTextBox` | `GetCustomer` | `SInventoryWebService.asmx` | 1 |
| `nameTextBox_AutoCompleteExtender` | `nameTextBox` | `GetProduct2` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `InvoiceBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetCustInfo` | `InvoiceBLL` | `CustomerMaster` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetProduct` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `qtyTextBox_TextChanged` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `SaveInvoiceDetails` | `List<InvoiceDetail> aInvoiceList,string comUnitId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `SaveInvoice` | `Invoice aInvoice, out int invoiceId, out string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_DCStoreCurrentStock` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblArea` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustCategory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStore` | `SaveInvoiceDetail()` → `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDcWiseAreaInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `SaveInvoice()` → `InvoiceBLL.SaveInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoice` |
| `tblInvoiceDetail` | `SaveInvoiceDetail()` → `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentType` | `Page_Load()` → `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductDiscount` | `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `fcodeTextBox` | `codeTextBox` | Custom, Numbers | . |
| `fcurrentStockTextBox` | `currentStockTextBox` | Custom, Numbers | . |
| `funitPriceTextBox` | `unitPriceTextBox` | Custom, Numbers | . |
| `fupVatTextBox` | `upVatTextBox` | Custom, Numbers | . |
| `fqtyTextBox` | `qtyTextBox` | Custom, Numbers | . |
| `ftpTextBox` | `tpTextBox` | Custom, Numbers | . |
| `ftpVatTextBox` | `tpVatTextBox` | Custom, Numbers | . |
| `fdpTextBox` | `dpTextBox` | Custom, Numbers | . |
| `fdpAmtTextBox` | `dpAmtTextBox` | Custom, Numbers | . |
| `fnpTextBox` | `npTextBox` | Custom, Numbers | . |
| `fbQtyTextBox` | `bQtyTextBox` | Custom, Numbers | . |
| `ftQtyTextBox` | `tQtyTextBox` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-010-001` | {0} | Code | `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `UserId`

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
| Database object | `View_DCStoreCurrentStock` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustCategory` |
| Database object | `tblCustMaster` |
| Database object | `tblDCStore` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblPaymentType` |
| Database object | `tblProduct` |
| Database object | `tblProductDiscount` |
| Database object | `tblTerritory` |
| Database object | `tblUnitPrice` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-010-001` | Unauthenticated access | Request `/SInventory_UI/InvoiceCreationForCustomer.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-010-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-010-003` | Grid loads | Open the page with data present | Grid `gridLineItemGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 76 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 22 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 18 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- Writes/reads `View_DCStoreCurrentStock` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustCategory` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblPaymentType` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductDiscount` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
