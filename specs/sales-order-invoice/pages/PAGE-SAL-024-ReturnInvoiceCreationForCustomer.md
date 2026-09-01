# SAL-024 — ReturnInvoiceCreationForCustomer

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-024` |
| Module | `sales-order-invoice` |
| File name | `ReturnInvoiceCreationForCustomer.aspx` |
| Route | `/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ReturnInvoiceCreationForCustomer.aspx.cs` |
| Code-behind class | `SInventory_UI_InvoiceCreationForCustomer` |
| Markup lines | 570 |
| Code-behind lines | 1100 |
| Server controls | 91 |
| GridViews | 1 |
| Code-behind methods | 30 |

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

`/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 7. UI Control Inventory

91 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `delinvoicenoTextBox` | asp:TextBox | Invoice No |  | True |  |  | custNameTextBox_TextChanged | 37 |
| 5 | `delinvoicenoTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender | Invoice No |  |  | Enabled=True |  |  | 39 |
| 6 | `salesCenterDropDownList` | asp:DropDownList | Sales Center |  | True |  |  |  | 63 |
| 7 | `Button1` | asp:Button | les Center | Search |  |  |  | Button1_Click | 68 |
| 8 | `custCodeTextBox` | asp:TextBox | Customer Code |  | True |  |  | custCodeTextBox_TextChanged | 105 |
| 9 | `custNameTextBox` | asp:TextBox | CustomerName |  | True |  |  | custNameTextBox_TextChanged | 111 |
| 10 | `custNameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender | CustomerName |  |  | Enabled=True |  |  | 113 |
| 11 | `custAddressTextBox` | asp:TextBox | Customer Address |  |  | ReadOnly=True |  |  | 127 |
| 12 | `comUnitNameTextBox` | asp:TextBox | Salse Center |  |  | ReadOnly=True |  |  | 135 |
| 13 | `miaCodeTextBox` | asp:TextBox | MIA Code |  |  | ReadOnly=True |  |  | 141 |
| 14 | `miaNameTextBox` | asp:TextBox | MIA Name |  |  | ReadOnly=True |  |  | 147 |
| 15 | `districtNameTextBox` | asp:TextBox | Distirict |  |  | ReadOnly=True |  |  | 156 |
| 16 | `areaNameTextBox` | asp:TextBox | Area |  |  | ReadOnly=True |  |  | 162 |
| 17 | `marketNameTextBox` | asp:TextBox | Merket |  |  | ReadOnly=True |  |  | 168 |
| 18 | `custCategoryTextBox` | asp:TextBox | Customer Category |  |  | ReadOnly=True |  |  | 176 |
| 19 | `invDateTextBox` | asp:TextBox | Invoice Date |  |  | ReadOnly=True |  |  | 192 |
| 20 | `orderNoTextBox` | asp:TextBox | Order No |  |  |  |  |  | 198 |
| 21 | `orderDateTextBox` | asp:TextBox | Order Date |  |  |  |  |  | 203 |
| 22 | `imgorderDate` | asp:ImageButton | Order Date | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 204 |
| 23 | `orderDate` | asp:CalendarExtender |  |  |  |  |  |  | 206 |
| 24 | `orderIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 213 |
| 25 | `invoiceHiddenField` | asp:HiddenField |  |  |  |  |  |  | 219 |
| 26 | `payTypeDDL` | asp:DropDownList | Payment Type |  |  |  |  |  | 227 |
| 27 | `gridLineItemGridView` | asp:GridView |  |  |  |  |  |  | 247 |
| 28 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 251 |
| 29 | `slLabel` | asp:Label | gridLineItemGridView → col[0] "SL" | {{Eval('SL')}} |  |  |  |  | 253 |
| 30 | `addImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineAdd.png |  |  |  | addImageButton_Click | 254 |
| 31 | `removeImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineDelete.png |  |  |  | removeImageButton_Click | 256 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 260 |
| 33 | `codeTextBox` | asp:TextBox | gridLineItemGridView → col[1] "Code" | {{Eval('ProductCode')}} | True |  |  | codeTextBox_TextChanged | 262 |
| 34 | `orderdetailIdHiddenField` | asp:HiddenField | gridLineItemGridView → col[1] "Code" |  |  |  |  |  | 265 |
| 35 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 268 |
| 36 | `nameTextBox` | asp:TextBox | gridLineItemGridView → col[2] "Name" | {{Eval('ProductName')}} | True |  |  | nameTextBox_TextChanged | 270 |
| 37 | `nameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 273 |
| 38 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 286 |
| 39 | `currentStockTextBox` | asp:TextBox | gridLineItemGridView → col[3] "CStock" | {{Eval('StockQty')}} |  | ReadOnly=True |  |  | 288 |
| 40 | `fcurrentStockTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 290 |
| 41 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 296 |
| 42 | `unitPriceTextBox` | asp:TextBox | gridLineItemGridView → col[4] "UP" | {{Eval('UnitPrice')}} |  | ReadOnly=True |  |  | 298 |
| 43 | `funitPriceTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 300 |
| 44 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 306 |
| 45 | `upVatTextBox` | asp:TextBox | gridLineItemGridView → col[5] "UVAT" | {{Eval('UnitVAT')}} |  | ReadOnly=True |  |  | 308 |
| 46 | `fupVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 310 |
| 47 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 316 |
| 48 | `qtyTextBox` | asp:TextBox | gridLineItemGridView → col[6] "Qty" | {{Eval('Quantity')}} | True | ReadOnly=True |  | qtyTextBox_TextChanged | 318 |
| 49 | `fqtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 321 |
| 50 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 328 |
| 51 | `tpTextBox` | asp:TextBox | gridLineItemGridView → col[7] "TP" | {{Eval('TotalPrice')}} |  | ReadOnly=True |  |  | 330 |
| 52 | `ftpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 332 |
| 53 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 338 |
| 54 | `tpVatTextBox` | asp:TextBox | gridLineItemGridView → col[8] "TPVAT" | {{Eval('VAT')}} |  | ReadOnly=True |  |  | 340 |
| 55 | `ftpVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 342 |
| 56 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 349 |
| 57 | `dpTextBox` | asp:TextBox | gridLineItemGridView → col[9] "DP" | {{Eval('DiscountPercentage')}} |  | ReadOnly=True |  |  | 351 |
| 58 | `fdpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 353 |
| 59 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 360 |
| 60 | `dpAmtTextBox` | asp:TextBox | gridLineItemGridView → col[10] "DAmt" | {{Eval('DiscountAmount')}} |  | ReadOnly=True |  |  | 362 |
| 61 | `fdpAmtTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 364 |
| 62 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 370 |
| 63 | `npTextBox` | asp:TextBox | gridLineItemGridView → col[11] "NP" | {{Eval('NetPrice')}} |  | ReadOnly=True |  |  | 372 |
| 64 | `fnpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 374 |
| 65 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 380 |
| 66 | `bQtyTextBox` | asp:TextBox | gridLineItemGridView → col[12] "BQty" | {{Eval('BonusQty')}} | True | ReadOnly=True |  | bQtyTextBox_TextChanged | 382 |
| 67 | `fbQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 385 |
| 68 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 391 |
| 69 | `tQtyTextBox` | asp:TextBox | gridLineItemGridView → col[13] "TQty" | {{Eval('TotalQty')}} |  | ReadOnly=True |  |  | 393 |
| 70 | `ftQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 395 |
| 71 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 401 |
| 72 | `prQtyTextBox` | asp:TextBox | gridLineItemGridView → col[14] "PRQty" |  |  | ReadOnly=True |  |  | 403 |
| 73 | `fprQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 404 |
| 74 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 410 |
| 75 | `dQtyTextBox` | asp:TextBox | gridLineItemGridView → col[15] "RQty" |  | True | ReadOnly=True |  | dQtyTextBox_TextChanged | 412 |
| 76 | `fdQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 414 |
| 77 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 420 |
| 78 | `chkSelectAll` | asp:CheckBox | gridLineItemGridView → col[16] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 422 |
| 79 | `chkSelect` | asp:CheckBox | gridLineItemGridView → col[16] "(no header)" |  | True |  |  | chkSelect_OnCheckedChanged | 426 |
| 80 | `tpTptalTextBox` | asp:TextBox | TP Total |  |  | ReadOnly=True |  |  | 443 |
| 81 | `disTotalTextBox` | asp:TextBox | Discount Total |  |  | ReadOnly=True |  |  | 459 |
| 82 | `vatTotalTextBox` | asp:TextBox | VAT Total |  |  | ReadOnly=True |  |  | 475 |
| 83 | `grandTotalTextBox` | asp:TextBox | Grand Total |  |  | ReadOnly=True |  |  | 492 |
| 84 | `saveButton` | asp:Button |  | Invoice Generate |  |  |  | saveButton_Click | 510 |
| 85 | `invTextBox` | asp:TextBox | Print InvNo |  |  |  |  |  | 524 |
| 86 | `printButton` | asp:Button | Print InvNo | Print |  |  |  | printButton_Click | 527 |
| 87 | `hdCustomerMasterId` | asp:HiddenField |  |  |  |  |  |  | 535 |
| 88 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 536 |
| 89 | `hdMiaId` | asp:HiddenField |  |  |  |  |  |  | 540 |
| 90 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 557 |
| 91 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 560 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `gridLineItemGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceDetailId,DCStoreId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | SL | — | `SL` | `Label:slLabel`, `ImageButton:addImageButton`, `ImageButton:removeImageButton` | — |
| 1 | TemplateField | Code | — | `OrderDetailsId`, `ProductCode` | `TextBox:codeTextBox`, `HiddenField:orderdetailIdHiddenField` | — |
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
| 14 | TemplateField | PRQty | — | — | `TextBox:prQtyTextBox`, `FilteredTextBoxExtender:prQtyTextBox` | — |
| 15 | TemplateField | RQty | — | — | `TextBox:dQtyTextBox`, `FilteredTextBoxExtender:dQtyTextBox` | — |
| 16 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `Invoice`, `InvoiceDetail`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.PaymentTypeLoadBLL(...)` | `InvoiceBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `Page_Load` | `aOrderInfoBll.LoadSC(...)` | `OrderInfoBLL.LoadSC(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadSC` → `OrderInfoDAL.LoadSC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit` | — |
| `GetCustInfo` | `aInvoiceBll.CustomerMaster(...)` | `InvoiceBLL.CustomerMaster(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — |
| `LoadInvoice` | `aOrderInfoBll.LoadInvoice(...)` | `OrderInfoBLL.LoadInvoice(string invoiceId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.GetTradeTerm(...)` | `OrderInfoBLL.GetTradeTerm(string amount)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblTradePolicyNew` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.LoadOrderWithDetail(...)` | `OrderInfoBLL.LoadOrderWithDetail(string orderid)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblMarket`, `tblOrder`, `tblOrderDetail`, `tblProduct` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.ProductVat(...)` | `OrderInfoBLL.ProductVat(string productcode)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblUnitPrice` | — |
| `GetProduct` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `GetQty` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `qtyTextBox_TextChanged` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.DCInfoWithDCId(...)` | `InvoiceBLL.DCInfoWithDCId(string dcstoreId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.DCInfoWithDCId` → `InvoiceDAL.DCInfoWithDCId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.SaveDCStoreFreeze(...)` | `InvoiceBLL.SaveDCStoreFreeze(DCStockNew aStockNew)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `SaveInvoiceDetail` | `aInvoiceBll.SaveReturnInvoice(...)` | `InvoiceBLL.SaveReturnInvoice(Invoice aInvoice, out int invoiceId, out string invoiceNo)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveReturnInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForReturnInvoice` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.ReturnInvoiceNoGenerator` → `InvoiceDAL.ReturnInvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblReturnInvoice` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.UpdateInvoiceDetail(...)` | `InvoiceBLL.UpdateInvoiceDetail(InvoiceDetail aInvoiceDetail)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoiceDetail` → `InvoiceDAL.UpdateInvoiceDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoiceDetail` | — |
| `SaveInvoice` | `aInvoiceBll.SaveReturnInvoice(...)` | `InvoiceBLL.SaveReturnInvoice(Invoice aInvoice, out int invoiceId, out string invoiceNo)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveReturnInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForReturnInvoice` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.ReturnInvoiceNoGenerator` → `InvoiceDAL.ReturnInvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblReturnInvoice` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.GetTradeTerm(...)` | `OrderInfoBLL.GetTradeTerm(string amount)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblTradePolicyNew` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.ProductVat(...)` | `OrderInfoBLL.ProductVat(string productcode)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblUnitPrice` | — |
| `Button1_Click` | `aOrderInfoBll.GetQty(...)` | `OrderInfoBLL.GetQty(string invoicedetailId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetQty` → `OrderInfoDAL.GetQty` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblReturnInvoiceDetail` | — |
| `Button1_Click` | `aOrderInfoBll.LoadDelivaryInvoice(...)` | `OrderInfoBLL.LoadDelivaryInvoice(string invoiceno, string comunitId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadDelivaryInvoice` → `OrderInfoDAL.LoadDelivaryInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `salesCenterDropDownList` | Sales Center | asp:DropDownList | True | — | `aInvoiceBll.ProductInfo()` in `SaveInvoiceDetail()`; `aOrderInfoBll.LoadDelivaryInvoice()` in `Button1_Click()`; `aOrderInfoBll.LoadSC()` in `Page_Load()` | `View_DCStoreCurrentStock`, `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct`, `tblUnitPrice` | set in code (see the DAL method above) |
| `payTypeDDL` | Payment Type | asp:DropDownList | False | — | `aInvoiceBll.PaymentTypeLoadBLL()` in `Page_Load()` | `tblPaymentType` | set in code (see the DAL method above) |

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
| `Button1` | asp:Button | Search | — | `Button1_Click` | 29 lines | `aOrderInfoBll.GetQty`, `aOrderInfoBll.LoadDelivaryInvoice` | — |
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
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 15 | `aInvoiceBll.PaymentTypeLoadBLL`, `aOrderInfoBll.LoadSC` | — | — | `InvoiceId` |
| 2 | `Todate` | `void Todate()` | 3 | — | — | — | — |
| 3 | `Validation` | `bool Validation()` | 28 | — | — | — | — |
| 4 | `GetCustInfo` | `void GetCustInfo(string custCode)` | 28 | `aInvoiceBll.CustomerMaster` | — | — | — |
| 5 | `LoadInvoice` | `void LoadInvoice(string invoiceId)` | 15 | `aOrderInfoBll.LoadInvoice` | — | — | — |
| 6 | `LoadAllDataByOrder` | `void LoadAllDataByOrder(string orderId)` | 68 | `aOrderInfoBll.GetTradeTerm`, `aOrderInfoBll.LoadOrderWithDetail`, `aOrderInfoBll.ProductVat` | — | — | — |
| 7 | `custCodeTextBox_TextChanged` | `void custCodeTextBox_TextChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 8 | `InitialGrid` | `void InitialGrid()` | 57 | — | — | — | — |
| 9 | `AddFunc` | `void AddFunc()` | 77 | — | — | — | — |
| 10 | `addImageButton_Click` | `void addImageButton_Click(object sender, ImageClickEventArgs e)` | 74 | — | — | — | — |
| 11 | `removeImageButton_Click` | `void removeImageButton_Click(object sender, ImageClickEventArgs e)` | 66 | — | — | — | — |
| 12 | `codeTextBox_TextChanged` | `void codeTextBox_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 13 | `GetProduct` | `void GetProduct(int rowindex, string productCode)` | 40 | `aInvoiceBll.ProductInfo` | — | — | — |
| 14 | `ProductCodeValidation` | `bool ProductCodeValidation(string productCode, int rowindex)` | 16 | — | — | — | — |
| 15 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 16 | `GetQty` | `void GetQty(decimal qty,int rowindex)` | 37 | `aInvoiceBll.ProductDiscount` | — | — | — |
| 17 | `qtyTextBox_TextChanged` | `void qtyTextBox_TextChanged(object sender, EventArgs e)` | 43 | `aInvoiceBll.ProductDiscount` | — | — | — |
| 18 | `bQtyTextBox_TextChanged` | `void bQtyTextBox_TextChanged(object sender, EventArgs e)` | 14 | — | — | — | — |
| 19 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 9 | — | — | — | — |
| 20 | `SaveInvoiceDetail` | `bool SaveInvoiceDetail(int invoiceId)` | 137 | `aInvoiceBll.DCInfoWithDCId`, `aInvoiceBll.ProductInfo`, `aInvoiceBll.SaveDCStoreFreeze`, `aInvoiceBll.SaveReturnInvoice`, `aInvoiceBll.UpdateInvoiceDetail` | — | — | — |
| 21 | `SaveInvoice` | `int SaveInvoice()` | 35 | `aInvoiceBll.SaveReturnInvoice` | — | — | `UserId` |
| 22 | `Clear` | `void Clear()` | 22 | — | — | — | — |
| 23 | `TotalValueCalculation` | `void TotalValueCalculation()` | 42 | — | — | — | — |
| 24 | `printButton_Click` | `void printButton_Click(object sender, EventArgs e)` | 6 | — | — | — | — |
| 25 | `custNameTextBox_TextChanged` | `void custNameTextBox_TextChanged(object sender, EventArgs e)` | 15 | — | — | — | — |
| 26 | `nameTextBox_TextChanged` | `void nameTextBox_TextChanged(object sender, EventArgs e)` | 25 | — | — | — | — |
| 27 | `dQtyTextBox_TextChanged` | `void dQtyTextBox_TextChanged(object sender, EventArgs e)` | 82 | `aOrderInfoBll.GetTradeTerm`, `aOrderInfoBll.ProductVat` | — | — | — |
| 28 | `Button1_Click` | `void Button1_Click(object sender, EventArgs e)` | 29 | `aOrderInfoBll.GetQty`, `aOrderInfoBll.LoadDelivaryInvoice` | — | — | — |
| 29 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 30 | `chkSelect_OnCheckedChanged` | `void chkSelect_OnCheckedChanged(object sender, EventArgs e)` | 17 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ReturnInvoiceCreationForCustomer.aspx (markup)
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
| `delinvoicenoTextBox_AutoCompleteExtender` | `delinvoicenoTextBox` | `GetCustomer` | `SInventoryWebService.asmx` | 1 |
| `custNameTextBox_AutoCompleteExtender` | `custNameTextBox` | `GetCustomer` | `SInventoryWebService.asmx` | 1 |
| `nameTextBox_AutoCompleteExtender` | `nameTextBox` | `GetProduct2` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `InvoiceBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Page_Load` | `OrderInfoBLL` | `LoadSC` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `GetCustInfo` | `InvoiceBLL` | `CustomerMaster` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoadInvoice` | `OrderInfoBLL` | `LoadInvoice` | `string invoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `GetTradeTerm` | `string amount` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `LoadOrderWithDetail` | `string orderid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `ProductVat` | `string productcode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `GetProduct` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetQty` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `qtyTextBox_TextChanged` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `DCInfoWithDCId` | `string dcstoreId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `SaveDCStoreFreeze` | `DCStockNew aStockNew` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `SaveReturnInvoice` | `Invoice aInvoice, out int invoiceId, out string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `UpdateInvoiceDetail` | `InvoiceDetail aInvoiceDetail` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `SaveReturnInvoice` | `Invoice aInvoice, out int invoiceId, out string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `GetTradeTerm` | `string amount` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `ProductVat` | `string productcode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `Button1_Click` | `OrderInfoBLL` | `GetQty` | `string invoicedetailId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `Button1_Click` | `OrderInfoBLL` | `LoadDelivaryInvoice` | `string invoiceno, string comunitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Process_WeightedAverageFreezeStock` | SaveInvoiceDetail → `InvoiceBLL.SaveDCStoreFreeze → ChalanDAL.ProcessWeightedAverageFreezeStock → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_DCStoreCurrentStock` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblArea` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `OrderInfoBLL.LoadSC` → `OrderInfoDAL.LoadSC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCustCategory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `Button1_Click()` → `OrderInfoBLL.LoadDelivaryInvoice` → `OrderInfoDAL.LoadDelivaryInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStore` | `SaveInvoiceDetail()` → `InvoiceBLL.DCInfoWithDCId` → `InvoiceDAL.DCInfoWithDCId` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStoreFreeze` | `SaveInvoiceDetail()` → `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` |
| `tblDcWiseAreaInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `Button1_Click()` → `OrderInfoBLL.LoadDelivaryInvoice` → `OrderInfoDAL.LoadDelivaryInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `Button1_Click()` → `OrderInfoBLL.LoadDelivaryInvoice` → `OrderInfoDAL.LoadDelivaryInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.UpdateInvoiceDetail` → `InvoiceDAL.UpdateInvoiceDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblMIOInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrderDetail` | `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentType` | `Page_Load()` → `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `Button1_Click()` → `OrderInfoBLL.LoadDelivaryInvoice` → `OrderInfoDAL.LoadDelivaryInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetQty()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductDiscount` | `GetQty()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblReturnInvoice` | `SaveInvoice()` → `InvoiceBLL.SaveReturnInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForReturnInvoice`, `SaveInvoiceDetail()` → `InvoiceBLL.SaveReturnInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForReturnInvoice` |
| `tblReturnInvoiceDetail` | `Button1_Click()` → `OrderInfoBLL.GetQty` → `OrderInfoDAL.GetQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTradePolicyNew` | `LoadAllDataByOrder()` → `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
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
| `fprQtyTextBox` | `prQtyTextBox` | Custom, Numbers | . |
| `fdQtyTextBox` | `dQtyTextBox` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-024-001` | {0} | Code | `Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `InvoiceId`, `UserId`

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
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Database object | `View_DCStoreCurrentStock` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustCategory` |
| Database object | `tblCustMaster` |
| Database object | `tblDCStore` |
| Database object | `tblDCStoreFreeze` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblOrderDetail` |
| Database object | `tblPaymentType` |
| Database object | `tblProduct` |
| Database object | `tblProductDiscount` |
| Database object | `tblReturnInvoice` |
| Database object | `tblReturnInvoiceDetail` |
| Database object | `tblTerritory` |
| Database object | `tblTradePolicyNew` |
| Database object | `tblUnitPrice` |
| Stored procedure | `sp_Process_WeightedAverageFreezeStock` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-024-001` | Unauthenticated access | Request `/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-024-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-024-003` | Grid loads | Open the page with data present | Grid `gridLineItemGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 91 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 30 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 25 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- Writes/reads `View_DCStoreCurrentStock` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustCategory` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDCStoreFreeze` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblOrderDetail` — any schema change to it breaks this page.
- Writes/reads `tblPaymentType` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductDiscount` — any schema change to it breaks this page.
- Writes/reads `tblReturnInvoice` — any schema change to it breaks this page.
- Writes/reads `tblReturnInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblTradePolicyNew` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
