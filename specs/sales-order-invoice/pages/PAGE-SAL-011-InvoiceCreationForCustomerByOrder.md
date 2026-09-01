# SAL-011 — InvoiceCreationForCustomerByOrder

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-011` |
| Module | `sales-order-invoice` |
| File name | `InvoiceCreationForCustomerByOrder.aspx` |
| Route | `/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `InvoiceCreationForCustomerByOrder.aspx.cs` |
| Code-behind class | `SInventory_UI_InvoiceCreationForCustomer` |
| Markup lines | 654 |
| Code-behind lines | 1938 |
| Server controls | 101 |
| GridViews | 1 |
| Code-behind methods | 43 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `CommonUI/HomePage.aspx`
- referenced from `SInventory_UI/InvoiceCreationByOrder.aspx`

## 5. Route

`/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |

## 7. UI Control Inventory

101 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 42 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 43 |
| 4 | `hdfCustomer` | asp:HiddenField | Proforma Invoice Creation |  |  |  |  |  | 55 |
| 5 | `custCodeTextBox` | asp:Label | Customer Code: |  | True | ReadOnly=True |  | custCodeTextBox_TextChanged | 68 |
| 6 | `custNameTextBox` | asp:Label | Customer Name: |  | True | ReadOnly=True |  | custNameTextBox_TextChanged | 74 |
| 7 | `custAddressTextBox` | asp:Label | Customer Address: |  |  | ReadOnly=True |  |  | 87 |
| 8 | `comUnitNameTextBox` | asp:Label | Salse Center: |  |  | ReadOnly=True |  |  | 94 |
| 9 | `miaCodeTextBox` | asp:Label | MIO Code: |  |  | ReadOnly=True |  |  | 99 |
| 10 | `miaNameTextBox` | asp:Label | MIO Name: |  |  | ReadOnly=True |  |  | 104 |
| 11 | `areaNameTextBox` | asp:Label | Territory Name: |  |  | ReadOnly=True |  |  | 112 |
| 12 | `marketNameTextBox` | asp:Label | Merket: |  |  | ReadOnly=True |  |  | 118 |
| 13 | `invDateTextBox` | asp:Label | Invoice Date: |  |  | ReadOnly=True |  |  | 125 |
| 14 | `orderNoTextBox` | asp:Label | Order No: |  |  | ReadOnly=True |  |  | 137 |
| 15 | `orderDateTextBox` | asp:Label | Order Date: |  |  | ReadOnly=True |  |  | 142 |
| 16 | `custCategoryTextBox` | asp:Label | Customer Category: |  |  | ReadOnly=True |  |  | 153 |
| 17 | `payTypeDDL` | asp:DropDownList | Payment Type: |  |  |  |  |  | 161 |
| 18 | `remarksTextBox` | asp:TextBox | Remarks: |  |  |  |  |  | 167 |
| 19 | `deliverypersonNameTextBox` | asp:DropDownList | Delivery Person Name: |  | True |  |  | deliverypersonNameTextBox_SelectedIndexChanged | 173 |
| 20 | `deliverypersonMobileTextBox` | asp:TextBox | Delivery Person Mob.No: |  |  |  |  |  | 185 |
| 21 | `districtNameTextBox` | asp:Label |  |  |  | ReadOnly=True |  |  | 189 |
| 22 | `txtPONumber` | asp:TextBox | PO Number: |  |  |  |  |  | 195 |
| 23 | `orderIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 200 |
| 24 | `isVatApplicable` | asp:HiddenField |  |  |  |  |  |  | 201 |
| 25 | `gridLineItemGridView` | asp:GridView | nbsp; |  |  |  |  |  | 217 |
| 26 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 220 |
| 27 | `slLabel` | asp:Label | gridLineItemGridView → col[0] "SL" |  |  |  |  |  | 222 |
| 28 | `addImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineAdd.png |  |  |  | addImageButton_Click | 223 |
| 29 | `removeImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineDelete.png |  |  |  | removeImageButton_Click | 225 |
| 30 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 229 |
| 31 | `codeTextBox` | asp:TextBox | gridLineItemGridView → col[1] "Code" |  | True |  |  | codeTextBox_TextChanged | 231 |
| 32 | `orderdetailIdHiddenField` | asp:HiddenField | gridLineItemGridView → col[1] "Code" |  |  |  |  |  | 233 |
| 33 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 236 |
| 34 | `nameTextBox` | asp:TextBox | gridLineItemGridView → col[2] "Name" |  | True |  |  | nameTextBox_TextChanged | 238 |
| 35 | `nameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 240 |
| 36 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 249 |
| 37 | `currentStockTextBox` | asp:TextBox | gridLineItemGridView → col[3] "CStock" |  |  | ReadOnly=True |  |  | 251 |
| 38 | `fcurrentStockTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 253 |
| 39 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 257 |
| 40 | `unitPriceTextBox` | asp:TextBox | gridLineItemGridView → col[4] "UP" |  |  | ReadOnly=True |  |  | 259 |
| 41 | `funitPriceTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 261 |
| 42 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 265 |
| 43 | `upVatTextBox` | asp:TextBox | gridLineItemGridView → col[5] "UVAT" |  |  | ReadOnly=True |  |  | 267 |
| 44 | `fupVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 269 |
| 45 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 273 |
| 46 | `qtyTextBox` | asp:TextBox | gridLineItemGridView → col[6] "Qty" |  | True | ReadOnly=True |  | qtyTextBox_TextChanged | 275 |
| 47 | `fqtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 277 |
| 48 | `hdfActualOrderQuantity` | asp:HiddenField | gridLineItemGridView → col[6] "Qty" |  |  |  |  |  | 280 |
| 49 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 283 |
| 50 | `tpTextBox` | asp:TextBox | gridLineItemGridView → col[7] "TP" |  |  | ReadOnly=True |  |  | 285 |
| 51 | `ftpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 287 |
| 52 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 291 |
| 53 | `dpTextBox` | asp:TextBox | gridLineItemGridView → col[8] "DP" |  |  | ReadOnly=True |  |  | 293 |
| 54 | `fdpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 294 |
| 55 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 298 |
| 56 | `dpAmtTextBox` | asp:TextBox | gridLineItemGridView → col[9] "DAmt" |  |  | ReadOnly=True |  |  | 300 |
| 57 | `fdpAmtTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 302 |
| 58 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 306 |
| 59 | `sdTextBox` | asp:TextBox | gridLineItemGridView → col[10] "SD" |  | True | ReadOnly=True |  | sdTextBox_OnTextChanged | 308 |
| 60 | `fsdTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 309 |
| 61 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 313 |
| 62 | `tpVatTextBox` | asp:TextBox | gridLineItemGridView → col[11] "TPVAT" |  |  | ReadOnly=True |  |  | 315 |
| 63 | `ftpVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 317 |
| 64 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 321 |
| 65 | `npTextBox` | asp:TextBox | gridLineItemGridView → col[12] "NP" |  |  | ReadOnly=True |  |  | 323 |
| 66 | `fnpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 325 |
| 67 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 329 |
| 68 | `bQtyTextBox` | asp:TextBox | gridLineItemGridView → col[13] "BQty" |  | True | ReadOnly=True |  | bQtyTextBox_TextChanged | 331 |
| 69 | `fbQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 333 |
| 70 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 337 |
| 71 | `tQtyTextBox` | asp:TextBox | gridLineItemGridView → col[14] "TQty" |  |  | ReadOnly=True |  |  | 339 |
| 72 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 347 |
| 73 | `addImageButton2` | asp:ImageButton | gridLineItemGridView → col[15] "add" | ~/images/lineAdd.png |  |  |  | addImageButton_Click | 349 |
| 74 | `tpTptalTextBox` | asp:TextBox | TP Total: |  |  | ReadOnly=True |  |  | 399 |
| 75 | `disTotalTextBox` | asp:TextBox | Discount Total: |  |  | ReadOnly=True |  |  | 414 |
| 76 | `HyperLink1` | asp:HyperLink |  |  |  |  |  |  | 425 |
| 77 | `pdTextBox` | asp:TextBox | Special Discount: |  | True | ReadOnly=True |  | pdTextBox_OnTextChanged | 431 |
| 78 | `vatTotalTextBox` | asp:TextBox | VAT Total: |  |  | ReadOnly=True |  |  | 446 |
| 79 | `grandTotalTextBox` | asp:TextBox | Grand Total: |  |  | ReadOnly=True |  |  | 461 |
| 80 | `saveButton` | asp:Button |  | Invoice Generate |  |  |  | saveButton_Click1 | 503 |
| 81 | `ModalPopupExtender10` | asp:ModalPopupExtender |  |  |  |  |  |  | 505 |
| 82 | `HiddenField10` | asp:HiddenField |  |  |  |  |  |  | 510 |
| 83 | `pnlModal` | asp:Panel |  |  |  |  |  |  | 511 |
| 84 | `ImageButton1` | asp:ImageButton | Confirm Message | ../PopCss/Images/close.jpg |  |  |  |  | 515 |
| 85 | `Image2` | asp:Image |  | ~/PopCss/Images/question.png |  |  |  |  | 520 |
| 86 | `invoiceNoTextBox` | asp:TextBox | Are you want to save the invoice ? |  |  |  |  |  | 524 |
| 87 | `yesButton` | asp:Button | invoice ? | Yes |  |  |  | yesButton_Click | 529 |
| 88 | `noButton` | asp:Button |  | No |  |  |  | noButton_Click | 533 |
| 89 | `UpdatePanel3` | asp:UpdatePanel |  |  |  |  |  |  | 539 |
| 90 | `UpdateProgress2` | asp:UpdateProgress |  |  |  |  |  |  | 543 |
| 91 | `Img2` | asp:Image |  | ~/Images/ajax-loader.gif |  |  |  |  | 547 |
| 92 | `invTextBox` | asp:Label | Print InvNo |  |  |  |  |  | 591 |
| 93 | `rptInvoiceId` | asp:Label | Print InvNo |  |  |  | False |  | 592 |
| 94 | `printButton` | asp:Button |  | Print |  |  |  | printButton_Click | 595 |
| 95 | `hdCustomerMasterId` | asp:HiddenField |  |  |  |  |  |  | 602 |
| 96 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 603 |
| 97 | `hdgrossValue` | asp:HiddenField |  |  |  |  |  |  | 604 |
| 98 | `hdMiaId` | asp:HiddenField |  |  |  |  |  |  | 607 |
| 99 | `orderHiddenField` | asp:HiddenField |  |  |  |  |  |  | 608 |
| 100 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 626 |
| 101 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 629 |

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
| 1 | TemplateField | Code | — | `OrderDetailsId`, `ProductCode` | `TextBox:codeTextBox`, `HiddenField:orderdetailIdHiddenField` | — |
| 2 | TemplateField | Name | — | `ProductName` | `TextBox:nameTextBox`, `AutoCompleteExtender:nameTextBox` | — |
| 3 | TemplateField | CStock | — | `StockQty` | `TextBox:currentStockTextBox`, `FilteredTextBoxExtender:currentStockTextBox` | — |
| 4 | TemplateField | UP | — | `UnitPrice` | `TextBox:unitPriceTextBox`, `FilteredTextBoxExtender:unitPriceTextBox` | — |
| 5 | TemplateField | UVAT | — | `UnitVAT` | `TextBox:upVatTextBox`, `FilteredTextBoxExtender:upVatTextBox` | — |
| 6 | TemplateField | Qty | — | `Quantity` | `TextBox:qtyTextBox`, `FilteredTextBoxExtender:qtyTextBox`, `HiddenField:hdfActualOrderQuantity` | — |
| 7 | TemplateField | TP | — | `TotalPrice` | `TextBox:tpTextBox`, `FilteredTextBoxExtender:tpTextBox` | — |
| 8 | TemplateField | DP | — | `DiscountPercentage` | `TextBox:dpTextBox`, `FilteredTextBoxExtender:dpTextBox` | — |
| 9 | TemplateField | DAmt | — | `DiscountAmount` | `TextBox:dpAmtTextBox`, `FilteredTextBoxExtender:dpAmtTextBox` | — |
| 10 | TemplateField | SD | — | `SpecialAmount` | `TextBox:sdTextBox`, `FilteredTextBoxExtender:sdTextBox` | — |
| 11 | TemplateField | TPVAT | — | `VAT` | `TextBox:tpVatTextBox`, `FilteredTextBoxExtender:tpVatTextBox` | — |
| 12 | TemplateField | NP | — | `NetPrice` | `TextBox:npTextBox`, `FilteredTextBoxExtender:npTextBox` | — |
| 13 | TemplateField | BQty | — | `BonusQty` | `TextBox:bQtyTextBox`, `FilteredTextBoxExtender:bQtyTextBox` | — |
| 14 | TemplateField | TQty | — | `TotalQty` | `TextBox:tQtyTextBox` | — |
| 15 | TemplateField | add | — | — | `ImageButton:addImageButton2` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `Invoice`, `InvoiceDetail`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.LoadCustomerPayType(...)` | `InvoiceBLL.LoadCustomerPayType(string value)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadCustomerPayType` → `InvoiceDAL.LoadCustomerPayType` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster` | — |
| `Page_Load` | `aInvoiceBll.PaymentTypeLoadBLL(...)` | `InvoiceBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `DeliveryMan` | `aInvoiceBll.LoadDeliveryMan(...)` | `InvoiceBLL.LoadDeliveryMan(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadDeliveryMan` | — | — | — |
| `Validation` | `aInvoiceBll.GetFinancialYearInfo(...)` | `InvoiceBLL.GetFinancialYearInfo(string invoiceDate,string unitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit` | — |
| `GetCustInfo` | `aInvoiceBll.CustomerMaster(...)` | `InvoiceBLL.CustomerMaster(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — |
| `OrderExists` | `aOrderInfoBll.LoadOrderExistsBll(...)` | `OrderInfoBLL.LoadOrderExistsBll(string orderid)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderExistsBll` → `OrderInfoDAL.LoadOrderExistsDal` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice` | — |
| `LoadAllDataByOrder` | `aInvoiceBll.GetFixedDiscount(...)` | `InvoiceBLL.GetFixedDiscount(string customerId, string unitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustomerFixedDiscount` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.GetTradeTerm(...)` | `OrderInfoBLL.GetTradeTerm(string amount)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblTradePolicyNew` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.LoadOrderWithDetail(...)` | `OrderInfoBLL.LoadOrderWithDetail(string orderid)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblMarket`, `tblOrder`, `tblOrderDetail`, `tblProduct` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.ProductDiscount(...)` | `OrderInfoBLL.ProductDiscount(string productcode, string customerId, string invoicedate)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.ProductVat(...)` | `OrderInfoBLL.ProductVat(string productcode)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblUnitPrice` | — |
| `GetOrderDetailValue` | `aOrderInfoBll.LoadOrderWithDetail(...)` | `OrderInfoBLL.LoadOrderWithDetail(string orderid)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblMarket`, `tblOrder`, `tblOrderDetail`, `tblProduct` | — |
| `addImageButton_Click` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `Qty` | `aInvoiceBll.ProductFocBonusQtyBLL(...)` | `InvoiceBLL.ProductFocBonusQtyBLL(string invoiceDate, string productCode, int qty)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductFocBonusQtyBLL` → `InvoiceDAL.ProductFocBonusQtyDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblFocDetails`, `tblFocMaster` | — |
| `GetProduct` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `GetQty` | `aInvoiceBll.GetFixedDiscount(...)` | `InvoiceBLL.GetFixedDiscount(string customerId, string unitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustomerFixedDiscount` | — |
| `GetQty` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `GetQty` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `qtyTextBox_TextChanged` | `aInvoiceBll.GetBonusCampaignInfo(...)` | `InvoiceBLL.GetBonusCampaignInfo(string productCode, DateTime date, TimeSpan time,Int32 Qty)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetBonusCampaignInfo` → `InvoiceDAL.GetBonusCampaignInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblBonusCampgainDetail`, `tblProduct` | — |
| `qtyTextBox_TextChanged` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `AddCampaignProduct` | `aInvoiceBll.GetBonusCampaignInfoByCompany(...)` | `InvoiceBLL.GetBonusCampaignInfoByCompany(string productCode, DateTime invDate, string orderId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetBonusCampaignInfoByCompany` → `InvoiceDAL.GetBonusCampaignInfoByCompany` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblOrder`, `tblProduct` | — |
| `AddCampaignProduct` | `aInvoiceBll.GetCampaignDetailsByCampaignId(...)` | `InvoiceBLL.GetCampaignDetailsByCampaignId(int campaignId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetCampaignDetailsByCampaignId` → `InvoiceDAL.GetCampaignDetailsByCampaignId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct` | — |
| `AddCampaignProduct` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `saveButton_Click1` | `aInvoiceBll.DeleteInvoice(...)` | `InvoiceBLL.DeleteInvoice(string Invoice)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.DeleteInvoice` → `InvoiceDAL.DeleteInvoice` → `ClsCommonInternalDAL.RunStoreProcedure` | — | — | `sp_Deletenvoice` |
| `saveButton_Click1` | `aInvoiceBll.GetInvoiceNoById(...)` | `InvoiceBLL.GetInvoiceNoById(int invId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetInvoiceNoById` → `InvoiceDAL.GetInvoiceNoById` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice` | — |
| `saveButton_Click1` | `aInvoiceBll.PostJournal(...)` | `InvoiceBLL.PostJournal(int invId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PostJournal` → `InvoiceDAL.PostJournal` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_ProformaPosting` |
| `saveButton_Click1` | `aInvoiceBll.SaveInvoiceMater(...)` | `InvoiceBLL.SaveInvoiceMater(Invoice aInvoice)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoiceMater` → `InvoiceDAL.SaveInvoiceMater` → `ClsCommonInternalDAL.SaveAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.SaveAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_I_InvoiceMaster` |
| `saveButton_Click1` | `aOrderInfoBll.UpdateInvoiceStatus(...)` | `OrderInfoBLL.UpdateInvoiceStatus(string id)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.UpdateInvoiceStatus` → `OrderInfoDAL.UpdateInvoiceStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblOrder` | — |
| `saveButton_Click` | `aInvoiceBll.DeleteInvoice(...)` | `InvoiceBLL.DeleteInvoice(string Invoice)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.DeleteInvoice` → `InvoiceDAL.DeleteInvoice` → `ClsCommonInternalDAL.RunStoreProcedure` | — | — | `sp_Deletenvoice` |
| `saveButton_Click` | `aInvoiceBll.PostJournal(...)` | `InvoiceBLL.PostJournal(int invId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PostJournal` → `InvoiceDAL.PostJournal` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_ProformaPosting` |
| `saveButton_Click` | `aOrderInfoBll.UpdateInvoiceStatus(...)` | `OrderInfoBLL.UpdateInvoiceStatus(string id)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.UpdateInvoiceStatus` → `OrderInfoDAL.UpdateInvoiceStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblOrder` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.LoadProductQty(...)` | `InvoiceBLL.LoadProductQty(string orderid, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblOrder`, `tblOrderDetail` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.SaveInvoiceDetails(...)` | `InvoiceBLL.SaveInvoiceDetails(List<InvoiceDetail> aInvoiceList,string comUnitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.UpdateDCStoreQuantity` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `InvoiceBLL.InvoiceDetailInsertBLL` → `ClsPrimaryKeyFind.PrimaryKeyMax` | INSERT, SELECT, UPDATE | `tblDCStore`, `tblInvoiceDetail` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.UpdateOrder(...)` | `InvoiceBLL.UpdateOrder(string status, string id)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateOrder` → `InvoiceDAL.UpdateOrder` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | UPDATE | `tblOrderDetail` | — |
| `SaveInvoiceDetail` | `aOrderInfoBll.ProductDiscount(...)` | `OrderInfoBLL.ProductDiscount(string productcode, string customerId, string invoicedate)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `SaveInvoice` | `aInvoiceBll.CustomerMaster(...)` | `InvoiceBLL.CustomerMaster(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — |
| `SaveInvoice` | `aInvoiceBll.LoadProductQty(...)` | `InvoiceBLL.LoadProductQty(string orderid, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblOrder`, `tblOrderDetail` | — |
| `SaveInvoice` | `aInvoiceBll.SaveInvoice(...)` | `InvoiceBLL.SaveInvoice(Invoice aInvoice, out int invoiceId, out string invoiceNo)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoice` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.InvoiceNoGenerator` → `InvoiceDAL.InvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblInvoice` | — |
| `SaveInvoice` | `aInvoiceReturnBll.DeleteProforma(...)` | `ProformaOrInvoiceReturnBLL.DeleteProforma(string Invoice, string userId)` <br/>`Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` | `ProformaOrInvoiceReturnBLL.DeleteProforma` → `ProformaOrInvoiceReturn.DeleteProformaDal` → `ClsCommonInternalDAL.RunStoreProcedure` | — | — | `sp_Delete_ProformaInvoice` |
| `SaveInvoice` | `aInvoiceReturnBll.ProformaReturnPosting(...)` | `ProformaOrInvoiceReturn.ProformaReturnPosting(string invoiceNo)` <br/>`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | `ProformaOrInvoiceReturn.ProformaReturnPosting` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ProformaOrInvoiceReturnBLL.ProformaReturnPosting` | — | — | `sp_ProformaReturnPosting` |
| `SaveInvoice` | `aOrderInfoBll.LoadDeliveryMan(...)` | `OrderInfoBLL.LoadDeliveryMan(string deliveryId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `TotalValueCalculation` | `aInvoiceBll.LoadProductQty(...)` | `InvoiceBLL.LoadProductQty(string orderid, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblOrder`, `tblOrderDetail` | — |
| `deliverypersonNameTextBox_SelectedIndexChanged` | `aOrderInfoBll.LoadDeliveryMan(...)` | `OrderInfoBLL.LoadDeliveryMan(string deliveryId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `noButton_Click` | `aInvoiceReturnBll.DeleteProforma(...)` | `ProformaOrInvoiceReturnBLL.DeleteProforma(string Invoice, string userId)` <br/>`Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` | `ProformaOrInvoiceReturnBLL.DeleteProforma` → `ProformaOrInvoiceReturn.DeleteProformaDal` → `ClsCommonInternalDAL.RunStoreProcedure` | — | — | `sp_Delete_ProformaInvoice` |
| `noButton_Click` | `aInvoiceReturnBll.ProformaReturnPosting(...)` | `ProformaOrInvoiceReturn.ProformaReturnPosting(string invoiceNo)` <br/>`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | `ProformaOrInvoiceReturn.ProformaReturnPosting` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ProformaOrInvoiceReturnBLL.ProformaReturnPosting` | — | — | `sp_ProformaReturnPosting` |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `payTypeDDL` | Payment Type: | asp:DropDownList | False | — | `aInvoiceBll.PaymentTypeLoadBLL()` in `Page_Load()` | `tblPaymentType` | set in code (see the DAL method above) |
| `deliverypersonNameTextBox` | Delivery Person Name: | asp:DropDownList | True | deliverypersonNameTextBox_SelectedIndexChanged | `aInvoiceBll.LoadDeliveryMan()` in `DeliveryMan()`; `aOrderInfoBll.LoadDeliveryMan()` in `SaveInvoice()`; `aOrderInfoBll.LoadDeliveryMan()` in `deliverypersonNameTextBox_SelectedIndexChanged()` | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | set in code (see the DAL method above) |

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
| `addImageButton` | asp:ImageButton |  | — | `addImageButton_Click` | 91 lines | `aInvoiceBll.ProductInfo` | — |
| `removeImageButton` | asp:ImageButton |  | — | `removeImageButton_Click` | 66 lines | — | — |
| `addImageButton2` | asp:ImageButton |  | — | `addImageButton_Click` | 91 lines | `aInvoiceBll.ProductInfo` | — |
| `saveButton` | asp:Button | Invoice Generate | — | `saveButton_Click1` | 99 lines | `aInvoiceBll.DeleteInvoice`, `aInvoiceBll.GetInvoiceNoById`, `aInvoiceBll.PostJournal`, `aInvoiceBll.SaveInvoiceMater`, `aOrderInfoBll.UpdateInvoiceStatus` | — |
| `ImageButton1` | asp:ImageButton |  | — | `—` | — | — | — |
| `yesButton` | asp:Button | Yes | — | `yesButton_Click` | 4 lines | — | — |
| `noButton` | asp:Button | No | — | `noButton_Click` | 16 lines | `aInvoiceReturnBll.DeleteProforma`, `aInvoiceReturnBll.ProformaReturnPosting` | — |
| `printButton` | asp:Button | Print | — | `printButton_Click` | 14 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 2
- AJAX Control Toolkit extenders: none
- External scripts: `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 40 | `aInvoiceBll.LoadCustomerPayType`, `aInvoiceBll.PaymentTypeLoadBLL` | — | — | `OrderId` |
| 2 | `DeliveryMan` | `void DeliveryMan()` | 4 | `aInvoiceBll.LoadDeliveryMan` | — | — | — |
| 3 | `Todate` | `void Todate()` | 3 | — | — | — | — |
| 4 | `Validation` | `bool Validation()` | 57 | `aInvoiceBll.GetFinancialYearInfo` | — | — | — |
| 5 | `GetCustInfo` | `void GetCustInfo(string custCode)` | 32 | `aInvoiceBll.CustomerMaster` | — | — | — |
| 6 | `OrderExists` | `bool OrderExists(string orderId)` | 13 | `aOrderInfoBll.LoadOrderExistsBll` | — | — | — |
| 7 | `ChkProductOffer` | `bool ChkProductOffer()` | 15 | — | — | — | — |
| 8 | `LoadAllDataByOrder` | `void LoadAllDataByOrder(string orderId)` | 192 | `aInvoiceBll.GetFixedDiscount`, `aOrderInfoBll.GetTradeTerm`, `aOrderInfoBll.LoadOrderWithDetail`, `aOrderInfoBll.ProductDiscount`, `aOrderInfoBll.ProductVat` | — | — | `LoginName` |
| 9 | `GetOrderDetailValue` | `void GetOrderDetailValue(string orderId)` | 11 | `aOrderInfoBll.LoadOrderWithDetail` | — | — | — |
| 10 | `GetDiscounttotalValue` | `void GetDiscounttotalValue()` | 27 | — | — | — | — |
| 11 | `B` | `bool B()` | 17 | — | — | — | — |
| 12 | `custCodeTextBox_TextChanged` | `void custCodeTextBox_TextChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 13 | `InitialGrid` | `void InitialGrid()` | 49 | — | — | — | — |
| 14 | `sdTextBox_OnTextChanged` | `void sdTextBox_OnTextChanged(object sender, EventArgs e)` | 5 | — | — | — | — |
| 15 | `CalculateGrandTotal` | `void CalculateGrandTotal()` | 20 | — | — | — | — |
| 16 | `TotalProformaValueCalculation` | `void TotalProformaValueCalculation()` | 45 | — | — | — | — |
| 17 | `AddFunc` | `void AddFunc()` | 80 | — | — | — | — |
| 18 | `addImageButton_Click` | `void addImageButton_Click(object sender, ImageClickEventArgs e)` | 91 | `aInvoiceBll.ProductInfo` | — | — | — |
| 19 | `Qty` | `int Qty()` | 20 | `aInvoiceBll.ProductFocBonusQtyBLL` | — | — | — |
| 20 | `removeImageButton_Click` | `void removeImageButton_Click(object sender, ImageClickEventArgs e)` | 66 | — | — | — | — |
| 21 | `codeTextBox_TextChanged` | `void codeTextBox_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 22 | `GetProduct` | `void GetProduct(int rowindex, string productCode)` | 41 | `aInvoiceBll.ProductInfo` | — | — | — |
| 23 | `ProductCodeValidation` | `bool ProductCodeValidation(string productCode, int rowindex)` | 16 | — | — | — | — |
| 24 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 25 | `GetQty` | `void GetQty(decimal qty, int rowindex, decimal discountPerRow, decimal p)` | 147 | `aInvoiceBll.GetFixedDiscount`, `aInvoiceBll.ProductDiscount`, `aInvoiceBll.ProductInfo` | — | — | — |
| 26 | `qtyTextBox_TextChanged` | `void qtyTextBox_TextChanged(object sender, EventArgs e)` | 61 | `aInvoiceBll.GetBonusCampaignInfo`, `aInvoiceBll.ProductDiscount` | — | — | — |
| 27 | `AddCampaignProduct` | `void AddCampaignProduct()` | 172 | `aInvoiceBll.GetBonusCampaignInfoByCompany`, `aInvoiceBll.GetCampaignDetailsByCampaignId`, `aInvoiceBll.ProductInfo` | — | — | — |
| 28 | `TotalQtyCalulation` | `void TotalQtyCalulation(int rowindex)` | 8 | — | — | — | — |
| 29 | `bQtyTextBox_TextChanged` | `void bQtyTextBox_TextChanged(object sender, EventArgs e)` | 14 | — | — | — | — |
| 30 | `saveButton_Click1` | `void saveButton_Click1(object sender, EventArgs e)` | 99 | `aInvoiceBll.DeleteInvoice`, `aInvoiceBll.GetInvoiceNoById`, `aInvoiceBll.PostJournal`, `aInvoiceBll.SaveInvoiceMater`, `aOrderInfoBll.UpdateInvoiceStatus` | — | — | — |
| 31 | `PrepareMasterData` | `Invoice PrepareMasterData()` | 32 | — | — | — | `UserId` |
| 32 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 53 | `aInvoiceBll.DeleteInvoice`, `aInvoiceBll.PostJournal`, `aOrderInfoBll.UpdateInvoiceStatus` | — | — | — |
| 33 | `SaveInvoiceDetail` | `bool SaveInvoiceDetail(int invoiceId)` | 88 | `aInvoiceBll.LoadProductQty`, `aInvoiceBll.SaveInvoiceDetails`, `aInvoiceBll.UpdateOrder`, `aOrderInfoBll.ProductDiscount` | — | — | — |
| 34 | `SaveInvoice` | `int SaveInvoice()` | 262 | `aInvoiceBll.CustomerMaster`, `aInvoiceBll.LoadProductQty`, `aInvoiceBll.SaveInvoice`, `aInvoiceReturnBll.DeleteProforma`, `aInvoiceReturnBll.ProformaReturnPosting`, `aOrderInfoBll.LoadDeliveryMan` | — | — | `LoginName`, `UserId`, `customer` |
| 35 | `Clear` | `void Clear()` | 29 | — | — | — | — |
| 36 | `TotalValueCalculation` | `void TotalValueCalculation()` | 57 | `aInvoiceBll.LoadProductQty` | — | — | — |
| 37 | `printButton_Click` | `void printButton_Click(object sender, EventArgs e)` | 14 | — | — | — | `customer` |
| 38 | `custNameTextBox_TextChanged` | `void custNameTextBox_TextChanged(object sender, EventArgs e)` | 15 | — | — | — | — |
| 39 | `nameTextBox_TextChanged` | `void nameTextBox_TextChanged(object sender, EventArgs e)` | 25 | — | — | — | — |
| 40 | `deliverypersonNameTextBox_SelectedIndexChanged` | `void deliverypersonNameTextBox_SelectedIndexChanged(object sender, EventArgs e)` | 8 | `aOrderInfoBll.LoadDeliveryMan` | — | — | — |
| 41 | `pdTextBox_OnTextChanged` | `void pdTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 42 | `yesButton_Click` | `void yesButton_Click(object sender, EventArgs e)` | 4 | — | — | — | — |
| 43 | `noButton_Click` | `void noButton_Click(object sender, EventArgs e)` | 16 | `aInvoiceReturnBll.DeleteProforma`, `aInvoiceReturnBll.ProformaReturnPosting` | — | — | `LoginName` |

Call chain shape for every persisting action on this page:

```
InvoiceCreationForCustomerByOrder.aspx (markup)
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
| `nameTextBox_AutoCompleteExtender` | `nameTextBox` | `GetProduct2` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `InvoiceBLL` | `LoadCustomerPayType` | `string value` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Page_Load` | `InvoiceBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DeliveryMan` | `InvoiceBLL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Validation` | `InvoiceBLL` | `GetFinancialYearInfo` | `string invoiceDate,string unitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetCustInfo` | `InvoiceBLL` | `CustomerMaster` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `OrderExists` | `OrderInfoBLL` | `LoadOrderExistsBll` | `string orderid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `InvoiceBLL` | `GetFixedDiscount` | `string customerId, string unitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `GetTradeTerm` | `string amount` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `LoadOrderWithDetail` | `string orderid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `ProductDiscount` | `string productcode, string customerId, string invoicedate` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `ProductVat` | `string productcode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `GetOrderDetailValue` | `OrderInfoBLL` | `LoadOrderWithDetail` | `string orderid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `addImageButton_Click` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Qty` | `InvoiceBLL` | `ProductFocBonusQtyBLL` | `string invoiceDate, string productCode, int qty` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetProduct` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetQty` | `InvoiceBLL` | `GetFixedDiscount` | `string customerId, string unitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetQty` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetQty` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `qtyTextBox_TextChanged` | `InvoiceBLL` | `GetBonusCampaignInfo` | `string productCode, DateTime date, TimeSpan time,Int32 Qty` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `qtyTextBox_TextChanged` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `AddCampaignProduct` | `InvoiceBLL` | `GetBonusCampaignInfoByCompany` | `string productCode, DateTime invDate, string orderId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `AddCampaignProduct` | `InvoiceBLL` | `GetCampaignDetailsByCampaignId` | `int campaignId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `AddCampaignProduct` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click1` | `InvoiceBLL` | `DeleteInvoice` | `string Invoice` | `int` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click1` | `InvoiceBLL` | `GetInvoiceNoById` | `int invId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click1` | `InvoiceBLL` | `PostJournal` | `int invId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click1` | `InvoiceBLL` | `SaveInvoiceMater` | `Invoice aInvoice` | `int` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click1` | `OrderInfoBLL` | `UpdateInvoiceStatus` | `string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `DeleteInvoice` | `string Invoice` | `int` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `PostJournal` | `int invId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `OrderInfoBLL` | `UpdateInvoiceStatus` | `string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `LoadProductQty` | `string orderid, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `SaveInvoiceDetails` | `List<InvoiceDetail> aInvoiceList,string comUnitId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `UpdateOrder` | `string status, string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `OrderInfoBLL` | `ProductDiscount` | `string productcode, string customerId, string invoicedate` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `CustomerMaster` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `LoadProductQty` | `string orderid, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `SaveInvoice` | `Invoice aInvoice, out int invoiceId, out string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `ProformaOrInvoiceReturnBLL` | `DeleteProforma` | `string Invoice, string userId` | `int` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `SaveInvoice` | `ProformaOrInvoiceReturnBLL` | `ProformaReturnPosting` | `string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `SaveInvoice` | `OrderInfoBLL` | `LoadDeliveryMan` | `string deliveryId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `TotalValueCalculation` | `InvoiceBLL` | `LoadProductQty` | `string orderid, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `deliverypersonNameTextBox_SelectedIndexChanged` | `OrderInfoBLL` | `LoadDeliveryMan` | `string deliveryId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `noButton_Click` | `ProformaOrInvoiceReturnBLL` | `DeleteProforma` | `string Invoice, string userId` | `int` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `noButton_Click` | `ProformaOrInvoiceReturnBLL` | `ProformaReturnPosting` | `string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `SaveInvoice` | `ProformaOrInvoiceReturn` | `ProformaReturnPosting` | `string invoiceNo` | `bool` | — | — | `sp_ProformaReturnPosting` | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| `noButton_Click` | `ProformaOrInvoiceReturn` | `ProformaReturnPosting` | `string invoiceNo` | `bool` | — | — | `sp_ProformaReturnPosting` | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Deletenvoice` | saveButton_Click1 → `InvoiceBLL.DeleteInvoice → InvoiceDAL.DeleteInvoice → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |
| `sp_ProformaPosting` | saveButton_Click1 → `InvoiceBLL.PostJournal → InvoiceDAL.PostJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_I_InvoiceMaster` | saveButton_Click1 → `InvoiceBLL.SaveInvoiceMater → InvoiceDAL.SaveInvoiceMater → ClsCommonInternalDAL.SaveAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.SaveAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Deletenvoice` | saveButton_Click → `InvoiceBLL.DeleteInvoice → InvoiceDAL.DeleteInvoice → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |
| `sp_ProformaPosting` | saveButton_Click → `InvoiceBLL.PostJournal → InvoiceDAL.PostJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Delete_ProformaInvoice` | SaveInvoice → `ProformaOrInvoiceReturnBLL.DeleteProforma → ProformaOrInvoiceReturn.DeleteProformaDal → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |
| `sp_ProformaReturnPosting` | SaveInvoice → `ProformaOrInvoiceReturn.ProformaReturnPosting → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction → ProformaOrInvoiceReturnBLL.ProformaReturnPosting` | **No** — see note |
| `sp_Delete_ProformaInvoice` | noButton_Click → `ProformaOrInvoiceReturnBLL.DeleteProforma → ProformaOrInvoiceReturn.DeleteProformaDal → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |
| `sp_ProformaReturnPosting` | noButton_Click → `ProformaOrInvoiceReturn.ProformaReturnPosting → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction → ProformaOrInvoiceReturnBLL.ProformaReturnPosting` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_DCStoreCurrentStock` | `AddCampaignProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetQty()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `addImageButton_Click()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblArea` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblBonusCampgainDetail` | `qtyTextBox_TextChanged()` → `InvoiceBLL.GetBonusCampaignInfo` → `InvoiceDAL.GetBonusCampaignInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `AddCampaignProduct()` → `InvoiceBLL.GetBonusCampaignInfoByCompany` → `InvoiceDAL.GetBonusCampaignInfoByCompany` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetQty()` → `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `Validation()` → `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `AddCampaignProduct()` → `InvoiceBLL.GetBonusCampaignInfoByCompany` → `InvoiceDAL.GetBonusCampaignInfoByCompany` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetOrderDetailValue()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetQty()` → `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustCategory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetOrderDetailValue()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `InvoiceBLL.LoadCustomerPayType` → `InvoiceDAL.LoadCustomerPayType` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerFixedDiscount` | `GetQty()` → `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `InvoiceBLL.GetFixedDiscount` → `InvoiceDAL.GetFixedDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStore` | `SaveInvoiceDetail()` → `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDcWiseAreaInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `SaveInvoice()` → `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable`, `deliverypersonNameTextBox_SelectedIndexChanged()` → `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable`, `deliverypersonNameTextBox_SelectedIndexChanged()` → `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblFocDetails` | `Qty()` → `InvoiceBLL.ProductFocBonusQtyBLL` → `InvoiceDAL.ProductFocBonusQtyDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblFocMaster` | `Qty()` → `InvoiceBLL.ProductFocBonusQtyBLL` → `InvoiceDAL.ProductFocBonusQtyDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `GetOrderDetailValue()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `OrderExists()` → `OrderInfoBLL.LoadOrderExistsBll` → `OrderInfoDAL.LoadOrderExistsDal` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.SaveInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoice`, `saveButton_Click1()` → `InvoiceBLL.GetInvoiceNoById` → `InvoiceDAL.GetInvoiceNoById` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `SaveInvoiceDetail()` → `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetOrderDetailValue()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `AddCampaignProduct()` → `InvoiceBLL.GetBonusCampaignInfoByCompany` → `InvoiceDAL.GetBonusCampaignInfoByCompany` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetOrderDetailValue()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable`, `TotalValueCalculation()` → `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrderDetail` | `GetOrderDetailValue()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.UpdateOrder` → `InvoiceDAL.UpdateOrder` → `ClsCommonInternalDAL.SaveDataByInsertCommand`, `TotalValueCalculation()` → `InvoiceBLL.LoadProductQty` → `InvoiceDAL.LoadProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentType` | `Page_Load()` → `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `AddCampaignProduct()` → `InvoiceBLL.GetBonusCampaignInfoByCompany` → `InvoiceDAL.GetBonusCampaignInfoByCompany` → `ClsCommonInternalDAL.DataContainerDataTable`, `AddCampaignProduct()` → `InvoiceBLL.GetCampaignDetailsByCampaignId` → `InvoiceDAL.GetCampaignDetailsByCampaignId` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetOrderDetailValue()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetQty()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductDiscount` | `GetQty()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTradePolicyNew` | `LoadAllDataByOrder()` → `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `AddCampaignProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetQty()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable`, `addImageButton_Click()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `LoadAllDataByOrder` | 0.00 | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `fcurrentStockTextBox` | `currentStockTextBox` | Custom, Numbers | . |
| `funitPriceTextBox` | `unitPriceTextBox` | Custom, Numbers | . |
| `fupVatTextBox` | `upVatTextBox` | Custom, Numbers | . |
| `fqtyTextBox` | `qtyTextBox` | Custom, Numbers | . |
| `ftpTextBox` | `tpTextBox` | Custom, Numbers | . |
| `fdpTextBox` | `dpTextBox` | Custom, Numbers | . |
| `fdpAmtTextBox` | `dpAmtTextBox` | Custom, Numbers | . |
| `fsdTextBox` | `sdTextBox` | Custom, Numbers | . |
| `ftpVatTextBox` | `tpVatTextBox` | Custom, Numbers | . |
| `fnpTextBox` | `npTextBox` | Custom, Numbers | . |
| `fbQtyTextBox` | `bQtyTextBox` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-011-001` | 0.00 | Code | `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` → `LoadAllDataByOrder()` | user action on this page | operation is refused | "0.00" | CONFIRMED |
| `BR-SAL-011-002` | {0} | Code | `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `OrderId`, `UserId`, `customer`

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
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| Database object | `View_DCStoreCurrentStock` |
| Database object | `tblArea` |
| Database object | `tblBonusCampgainDetail` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustCategory` |
| Database object | `tblCustMaster` |
| Database object | `tblCustomerFixedDiscount` |
| Database object | `tblDCStore` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblFocDetails` |
| Database object | `tblFocMaster` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblOrderDetail` |
| Database object | `tblPaymentType` |
| Database object | `tblProduct` |
| Database object | `tblProductDiscount` |
| Database object | `tblTerritory` |
| Database object | `tblTradePolicyNew` |
| Database object | `tblUnitPrice` |
| Stored procedure | `sp_Deletenvoice` |
| Stored procedure | `sp_ProformaPosting` |
| Stored procedure | `sp_I_InvoiceMaster` |
| Stored procedure | `sp_Deletenvoice` |
| Stored procedure | `sp_ProformaPosting` |
| Stored procedure | `sp_Delete_ProformaInvoice` |
| Stored procedure | `sp_ProformaReturnPosting` |
| Stored procedure | `sp_Delete_ProformaInvoice` |
| Stored procedure | `sp_ProformaReturnPosting` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-011-001` | Unauthenticated access | Request `/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-011-002` | Validation: 0.00 | Trigger `LoadAllDataByOrder()` in the state that produces this message | alert "0.00" and the operation is not performed | CONFIRMED |
| `TC-SAL-011-003` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-011-004` | Grid loads | Open the page with data present | Grid `gridLineItemGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 101 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 43 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 9 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 26 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` is **shared with 2 other page(s)** (e.g. `ErrorInvoiceList.aspx`, `ProformaorInvoiceReturn.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` is **shared with 2 other page(s)** (e.g. `ErrorInvoiceList.aspx`, `ProformaorInvoiceReturn.aspx`) — a change there is not local.
- Writes/reads `View_DCStoreCurrentStock` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblBonusCampgainDetail` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustCategory` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustomerFixedDiscount` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblFocDetails` — any schema change to it breaks this page.
- Writes/reads `tblFocMaster` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblOrderDetail` — any schema change to it breaks this page.
- Writes/reads `tblPaymentType` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductDiscount` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblTradePolicyNew` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
