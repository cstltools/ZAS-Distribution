# SAL-002 — DelivaryInvoiceCreationForCustomerByOrderAuto

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-002` |
| Module | `sales-order-invoice` |
| File name | `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` |
| Route | `/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` |
| Code-behind class | `SInventory_UI_DelivaryInvoiceCreationForCustomerByOrderAuto` |
| Markup lines | 606 |
| Code-behind lines | 2700 |
| Server controls | 93 |
| GridViews | 1 |
| Code-behind methods | 35 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/DelivaryInvoiceCreationAuto.aspx`

## 5. Route

`/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 7. UI Control Inventory

93 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 6 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 8 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 9 |
| 4 | `custCodeTextBox` | asp:Label | Customer Code: |  |  | ReadOnly=True |  |  | 43 |
| 5 | `custNameTextBox` | asp:Label | Customer Name: |  |  | ReadOnly=True |  |  | 49 |
| 6 | `custAddressTextBox` | asp:Label | Customer Address: |  |  | ReadOnly=True |  |  | 62 |
| 7 | `comUnitNameTextBox` | asp:Label | Salse Center: |  |  | ReadOnly=True |  |  | 70 |
| 8 | `miaCodeTextBox` | asp:Label | MIO Code: |  |  | ReadOnly=True |  |  | 76 |
| 9 | `miaNameTextBox` | asp:Label | MIO Name: |  |  | ReadOnly=True |  |  | 82 |
| 10 | `areaNameTextBox` | asp:Label | Territory Name: |  |  | ReadOnly=True |  |  | 90 |
| 11 | `marketNameTextBox` | asp:Label | Merket Name: |  |  | ReadOnly=True |  |  | 96 |
| 12 | `custCategoryTextBox` | asp:Label | Customer Category: |  |  | ReadOnly=True |  |  | 103 |
| 13 | `orderNoTextBox` | asp:Label | Order No: |  |  | ReadOnly=True |  |  | 112 |
| 14 | `orderDateTextBox` | asp:Label | Order Date: |  |  | ReadOnly=True |  |  | 118 |
| 15 | `invDateTextBox` | asp:Label | Invoice Date: |  |  | ReadOnly=True |  |  | 127 |
| 16 | `deliverypersonMobileTextBox` | asp:Label | Delivery Person Mob.No: |  |  | ReadOnly=True |  |  | 136 |
| 17 | `deliverypersonNameTextBox` | asp:Label | Delivery Person Name: |  |  | ReadOnly=True |  |  | 145 |
| 18 | `remarksTextBox` | asp:Label | Remarks: |  |  | ReadOnly=True |  |  | 152 |
| 19 | `payTypeDDL` | asp:DropDownList | Payment Type |  |  |  |  |  | 176 |
| 20 | `invoiceNoHiddenField` | asp:HiddenField |  |  |  |  |  |  | 197 |
| 21 | `invoiceHiddenField` | asp:HiddenField |  |  |  |  |  |  | 198 |
| 22 | `orderIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 199 |
| 23 | `gridLineItemGridView` | asp:GridView |  |  |  |  |  |  | 205 |
| 24 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 208 |
| 25 | `slLabel` | asp:Label | gridLineItemGridView → col[0] "SL" |  |  |  |  |  | 210 |
| 26 | `addImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineAdd.png |  |  |  | addImageButton_Click | 211 |
| 27 | `removeImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineDelete.png |  |  |  | removeImageButton_Click | 213 |
| 28 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 217 |
| 29 | `codeTextBox` | asp:TextBox | gridLineItemGridView → col[1] "Code" |  | True |  |  | codeTextBox_TextChanged | 219 |
| 30 | `orderdetailIdHiddenField` | asp:HiddenField | gridLineItemGridView → col[1] "Code" |  |  |  |  |  | 221 |
| 31 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 224 |
| 32 | `nameTextBox` | asp:TextBox | gridLineItemGridView → col[2] "Name" |  | True |  |  | nameTextBox_TextChanged | 226 |
| 33 | `nameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 228 |
| 34 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 237 |
| 35 | `currentStockTextBox` | asp:TextBox | gridLineItemGridView → col[3] "CStock" |  |  | ReadOnly=True |  |  | 239 |
| 36 | `fcurrentStockTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 241 |
| 37 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 245 |
| 38 | `unitPriceTextBox` | asp:TextBox | gridLineItemGridView → col[4] "UP" |  |  | ReadOnly=True |  |  | 247 |
| 39 | `funitPriceTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 249 |
| 40 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 253 |
| 41 | `upVatTextBox` | asp:TextBox | gridLineItemGridView → col[5] "UVAT" |  |  | ReadOnly=True |  |  | 255 |
| 42 | `fupVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 257 |
| 43 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 261 |
| 44 | `qtyTextBox` | asp:TextBox | gridLineItemGridView → col[6] "Qty" |  | True | ReadOnly=True |  | qtyTextBox_TextChanged | 263 |
| 45 | `fqtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 265 |
| 46 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 269 |
| 47 | `tpTextBox` | asp:TextBox | gridLineItemGridView → col[7] "TP" |  |  | ReadOnly=True |  |  | 271 |
| 48 | `ftpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 273 |
| 49 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 277 |
| 50 | `dpTextBox` | asp:TextBox | gridLineItemGridView → col[8] "DP" |  |  | ReadOnly=True |  |  | 279 |
| 51 | `fdpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 281 |
| 52 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 285 |
| 53 | `dpAmtTextBox` | asp:TextBox | gridLineItemGridView → col[9] "DAmt" |  |  | ReadOnly=True |  |  | 287 |
| 54 | `fdpAmtTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 289 |
| 55 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 293 |
| 56 | `sdTextBox` | asp:TextBox | gridLineItemGridView → col[10] "SD" |  | True | ReadOnly=True |  | sdTextBox_OnTextChanged | 295 |
| 57 | `fsdTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 297 |
| 58 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 301 |
| 59 | `tpVatTextBox` | asp:TextBox | gridLineItemGridView → col[11] "TPVAT" |  |  | ReadOnly=True |  |  | 303 |
| 60 | `ftpVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 305 |
| 61 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 309 |
| 62 | `npTextBox` | asp:TextBox | gridLineItemGridView → col[12] "NP" |  |  | ReadOnly=True |  |  | 311 |
| 63 | `fnpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 313 |
| 64 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 317 |
| 65 | `bQtyTextBox` | asp:TextBox | gridLineItemGridView → col[13] "BQty" |  | True | ReadOnly=True |  | bQtyTextBox_TextChanged | 319 |
| 66 | `fbQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 321 |
| 67 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 325 |
| 68 | `tQtyTextBox` | asp:TextBox | gridLineItemGridView → col[14] "TQty" |  |  | ReadOnly=True |  |  | 327 |
| 69 | `ftQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 329 |
| 70 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 333 |
| 71 | `dQtyTextBox` | asp:TextBox | gridLineItemGridView → col[15] "DQty" |  | True |  |  | dQtyTextBox_TextChanged | 335 |
| 72 | `fdQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 337 |
| 73 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 341 |
| 74 | `statusLabel` | asp:Label | gridLineItemGridView → col[16] "Status" |  |  |  |  |  | 343 |
| 75 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 346 |
| 76 | `reasonReturnDropDownList` | asp:DropDownList | gridLineItemGridView → col[17] "Reason" |  |  | Enabled=False |  |  | 348 |
| 77 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 353 |
| 78 | `dodTextBox` | asp:TextBox | gridLineItemGridView → col[18] "DOD" |  | True | ReadOnly=True |  | dodTextBox_TextChanged | 355 |
| 79 | `dodTextBox11` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 358 |
| 80 | `tpTptalTextBox` | asp:TextBox | TP Total: |  |  | ReadOnly=True |  |  | 400 |
| 81 | `disTotalTextBox` | asp:TextBox | Discount Total: |  |  | ReadOnly=True |  |  | 420 |
| 82 | `HyperLink1` | asp:HyperLink |  |  |  |  |  |  | 434 |
| 83 | `pdTextBox` | asp:TextBox | Special Discount: |  |  | ReadOnly=True |  |  | 440 |
| 84 | `vatTotalTextBox` | asp:TextBox | VAT Total: |  |  | ReadOnly=True |  |  | 460 |
| 85 | `grandTotalTextBox` | asp:TextBox | Grand Total: |  |  | ReadOnly=True |  |  | 480 |
| 86 | `Button1` | asp:Button |  | Invoice Generate |  |  |  | saveButton_Click | 520 |
| 87 | `invTextBox` | asp:Label | Print InvNo: |  |  |  |  |  | 560 |
| 88 | `printButton` | asp:Button |  | Print |  |  |  | printButton_Click | 563 |
| 89 | `hdCustomerMasterId` | asp:HiddenField |  |  |  |  |  |  | 572 |
| 90 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 573 |
| 91 | `hdMiaId` | asp:HiddenField |  |  |  |  |  |  | 576 |
| 92 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 597 |
| 93 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 600 |

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
| 8 | TemplateField | DP | — | `DiscountPercentage` | `TextBox:dpTextBox`, `FilteredTextBoxExtender:dpTextBox` | — |
| 9 | TemplateField | DAmt | — | `DiscountAmount` | `TextBox:dpAmtTextBox`, `FilteredTextBoxExtender:dpAmtTextBox` | — |
| 10 | TemplateField | SD | — | `SpecialAmount` | `TextBox:sdTextBox`, `FilteredTextBoxExtender:sdTextBox` | — |
| 11 | TemplateField | TPVAT | — | `VAT` | `TextBox:tpVatTextBox`, `FilteredTextBoxExtender:tpVatTextBox` | — |
| 12 | TemplateField | NP | — | `NetPrice` | `TextBox:npTextBox`, `FilteredTextBoxExtender:npTextBox` | — |
| 13 | TemplateField | BQty | — | `BonusQty` | `TextBox:bQtyTextBox`, `FilteredTextBoxExtender:bQtyTextBox` | — |
| 14 | TemplateField | TQty | — | `TotalQty` | `TextBox:tQtyTextBox`, `FilteredTextBoxExtender:tQtyTextBox` | — |
| 15 | TemplateField | DQty | — | — | `TextBox:dQtyTextBox`, `FilteredTextBoxExtender:dQtyTextBox` | — |
| 16 | TemplateField | Status | — | — | `Label:statusLabel` | — |
| 17 | TemplateField | Reason | — | — | `DropDownList:reasonReturnDropDownList` | — |
| 18 | TemplateField | DOD | — | — | `TextBox:dodTextBox`, `FilteredTextBoxExtender:dodTextBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `Invoice`, `InvoiceDetail`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.PaymentTypeLoadBLL(...)` | `InvoiceBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `Validation` | `aInvoiceBll.CheckChartOfAccount(...)` | `InvoiceBLL.CheckChartOfAccount(string customerCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CheckChartOfAccount` → `InvoiceDAL.CheckChartOfAccount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster` | — |
| `GetCustInfo` | `aInvoiceBll.CustomerMaster(...)` | `InvoiceBLL.CustomerMaster(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — |
| `LoadInvoice` | `aInvoiceBll.ReturnReason(...)` | `InvoiceBLL.ReturnReason(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ReturnReason` → `InvoiceDAL.ReturnReason` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblReturnReason` | — |
| `LoadInvoice` | `aOrderInfoBll.LoadInvoice(...)` | `OrderInfoBLL.LoadInvoice(string invoiceId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.GetFixedCustomerfromInvoiceTable(...)` | `OrderInfoBLL.GetFixedCustomerfromInvoiceTable(string invoiceid)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetFixedCustomerfromInvoiceTable` → `OrderInfoDAL.GetFixedCustomerfromInvoiceTable` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblInvoice` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.GetTradeTerm(...)` | `OrderInfoBLL.GetTradeTerm(string amount)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblTradePolicyNew` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.GetTradeTermOld(...)` | `OrderInfoBLL.GetTradeTermOld(string amount)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetTradeTermOld` → `OrderInfoDAL.GetTradeTermOld` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblTradePolicy` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.LoadOffer(...)` | `OrderInfoBLL.LoadOffer(string InvoiceId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOffer` → `OrderInfoDAL.LoadOffer` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblInvoice` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.LoadOrderWithDetail(...)` | `OrderInfoBLL.LoadOrderWithDetail(string orderid)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblMarket`, `tblOrder`, `tblOrderDetail`, `tblProduct` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.ProductDiscount(...)` | `OrderInfoBLL.ProductDiscount(string productcode, string customerId, string invoicedate)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `LoadAllDataByOrder` | `aOrderInfoBll.ProductVat(...)` | `OrderInfoBLL.ProductVat(string productcode)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblUnitPrice` | — |
| `GetProduct` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `GetQty` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `qtyTextBox_TextChanged` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `saveButton_Click` | `aInvoiceBll.PostDeliveryConfPartialJournal(...)` | `InvoiceBLL.PostDeliveryConfPartialJournal(int invoiceId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PostDeliveryConfPartialJournal` → `InvoiceDAL.PostDeliveryConfPartialJournal` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_DeliveryConfirmationPartiallPosting`, `sp_Sales_DeliverySalesPosting` |
| `saveButton_Click` | `aInvoiceBll.ResetDeliveryConfirmation(...)` | `InvoiceBLL.ResetDeliveryConfirmation(int invId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ResetDeliveryConfirmation` → `InvoiceDAL.UpdateDeliveryInformation` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoice` | — |
| `DelDelivInvoiceDetail` | `aInvoiceBll.DCInfoWithDCId(...)` | `InvoiceBLL.DCInfoWithDCId(string dcstoreId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.DCInfoWithDCId` → `InvoiceDAL.DCInfoWithDCId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore` | — |
| `DelDelivInvoiceDetail` | `aInvoiceBll.SaveDCStoreFreeze(...)` | `InvoiceBLL.SaveDCStoreFreeze(DCStockNew aStockNew)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `DelDelivInvoiceDetail` | `aInvoiceBll.UpdateInvoiceDetail(...)` | `InvoiceBLL.UpdateInvoiceDetail(InvoiceDetail aInvoiceDetail)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoiceDetail` → `InvoiceDAL.UpdateInvoiceDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoiceDetail` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.DCInfoWithDCId(...)` | `InvoiceBLL.DCInfoWithDCId(string dcstoreId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.DCInfoWithDCId` → `InvoiceDAL.DCInfoWithDCId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.SaveDCStoreFreeze(...)` | `InvoiceBLL.SaveDCStoreFreeze(DCStockNew aStockNew)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `SaveInvoiceDetail` | `aInvoiceBll.UpdateInvoiceDetail(...)` | `InvoiceBLL.UpdateInvoiceDetail(InvoiceDetail aInvoiceDetail)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoiceDetail` → `InvoiceDAL.UpdateInvoiceDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoiceDetail` | — |
| `SaveInvoice` | `aInvoiceBll.SaveInvoice(...)` | `InvoiceBLL.SaveInvoice(Invoice aInvoice, out int invoiceId, out string invoiceNo)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoice` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.InvoiceNoGenerator` → `InvoiceDAL.InvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblInvoice` | — |
| `SaveInvoice` | `aInvoiceBll.UpdateInvoice(...)` | `InvoiceBLL.UpdateInvoice(Invoice aInvoice,out string invoiceno)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoice` → `InvoiceDAL.UpdateInvoice` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `InvoiceBLL.DelInvoiceNoGenerator` → `InvoiceDAL.InvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT, UPDATE | `tblInvoice` | — |
| `SaveInvoice` | `aOrderInfoBll.UpdateInvoiceStatus(...)` | `OrderInfoBLL.UpdateInvoiceStatus(string id)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.UpdateInvoiceStatus` → `OrderInfoDAL.UpdateInvoiceStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblOrder` | — |
| `DelDelivInvoice` | `aInvoiceBll.SaveInvoice(...)` | `InvoiceBLL.SaveInvoice(Invoice aInvoice, out int invoiceId, out string invoiceNo)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoice` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.InvoiceNoGenerator` → `InvoiceDAL.InvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblInvoice` | — |
| `DelDelivInvoice` | `aInvoiceBll.UpdateInvoiceNull(...)` | `InvoiceBLL.UpdateInvoiceNull(Invoice aInvoice)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoiceNull` → `InvoiceDAL.UpdateInvoiceNull` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoice` | — |
| `DelDelivInvoice` | `aOrderInfoBll.UpdateInvoiceStatus(...)` | `OrderInfoBLL.UpdateInvoiceStatus(string id)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.UpdateInvoiceStatus` → `OrderInfoDAL.UpdateInvoiceStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblOrder` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.GetFixedCustomerfromInvoiceTable(...)` | `OrderInfoBLL.GetFixedCustomerfromInvoiceTable(string invoiceid)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetFixedCustomerfromInvoiceTable` → `OrderInfoDAL.GetFixedCustomerfromInvoiceTable` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblInvoice` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.GetTradeTerm(...)` | `OrderInfoBLL.GetTradeTerm(string amount)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblTradePolicyNew` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.GetTradeTermOld(...)` | `OrderInfoBLL.GetTradeTermOld(string amount)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.GetTradeTermOld` → `OrderInfoDAL.GetTradeTermOld` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblTradePolicy` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.LoadInvoice(...)` | `OrderInfoBLL.LoadInvoice(string invoiceId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.LoadOffer(...)` | `OrderInfoBLL.LoadOffer(string InvoiceId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOffer` → `OrderInfoDAL.LoadOffer` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblInvoice` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.ProductDiscount(...)` | `OrderInfoBLL.ProductDiscount(string productcode, string customerId, string invoicedate)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `dQtyTextBox_TextChanged` | `aOrderInfoBll.ProductVat(...)` | `OrderInfoBLL.ProductVat(string productcode)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblUnitPrice` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `payTypeDDL` | Payment Type | asp:DropDownList | False | — | `aInvoiceBll.PaymentTypeLoadBLL()` in `Page_Load()` | `tblPaymentType` | set in code (see the DAL method above) |
| `reasonReturnDropDownList` | gridLineItemGridView → col[17] "Reason" | asp:DropDownList | False | — | `aInvoiceBll.ReturnReason()` in `LoadInvoice()` | `tblReturnReason` | set in code (see the DAL method above) |

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
| `addImageButton` | asp:ImageButton |  | — | `addImageButton_Click` | 82 lines | — | — |
| `removeImageButton` | asp:ImageButton |  | — | `removeImageButton_Click` | 79 lines | — | — |
| `Button1` | asp:Button | Invoice Generate | return confirm('Are you sure you want to Save Delivery Invoice ?'); | `saveButton_Click` | 58 lines | `aInvoiceBll.PostDeliveryConfPartialJournal`, `aInvoiceBll.ResetDeliveryConfirmation` | — |
| `printButton` | asp:Button | Print | — | `printButton_Click` | 9 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 15 | `aInvoiceBll.PaymentTypeLoadBLL` | — | — | `InvoiceId` |
| 2 | `Todate` | `void Todate()` | 3 | — | — | — | — |
| 3 | `Validation` | `bool Validation()` | 46 | `aInvoiceBll.CheckChartOfAccount` | — | — | — |
| 4 | `GetCustInfo` | `void GetCustInfo(string custCode)` | 27 | `aInvoiceBll.CustomerMaster` | — | — | — |
| 5 | `LoadInvoice` | `void LoadInvoice(string invoiceId)` | 49 | `aInvoiceBll.ReturnReason`, `aOrderInfoBll.LoadInvoice` | — | — | `LoginName` |
| 6 | `ChkProductOffer` | `bool ChkProductOffer()` | 12 | — | — | — | — |
| 7 | `ChkProductOfferModi` | `bool ChkProductOfferModi()` | 12 | — | — | — | — |
| 8 | `ChkProductOfferEsomium20` | `bool ChkProductOfferEsomium20()` | 13 | — | — | — | — |
| 9 | `LoadAllDataByOrder` | `void LoadAllDataByOrder(string orderId)` | 128 | `aOrderInfoBll.GetFixedCustomerfromInvoiceTable`, `aOrderInfoBll.GetTradeTerm`, `aOrderInfoBll.GetTradeTermOld`, `aOrderInfoBll.LoadOffer`, `aOrderInfoBll.LoadOrderWithDetail`, `aOrderInfoBll.ProductDiscount` | — | — | — |
| 10 | `custCodeTextBox_TextChanged` | `void custCodeTextBox_TextChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 11 | `InitialGrid` | `void InitialGrid()` | 58 | — | — | — | — |
| 12 | `AddFunc` | `void AddFunc()` | 93 | — | — | — | — |
| 13 | `addImageButton_Click` | `void addImageButton_Click(object sender, ImageClickEventArgs e)` | 82 | — | — | — | — |
| 14 | `removeImageButton_Click` | `void removeImageButton_Click(object sender, ImageClickEventArgs e)` | 79 | — | — | — | — |
| 15 | `codeTextBox_TextChanged` | `void codeTextBox_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 16 | `GetProduct` | `void GetProduct(int rowindex, string productCode)` | 40 | `aInvoiceBll.ProductInfo` | — | — | — |
| 17 | `ProductCodeValidation` | `bool ProductCodeValidation(string productCode, int rowindex)` | 14 | — | — | — | — |
| 18 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 19 | `GetQty` | `void GetQty(decimal qty, int rowindex)` | 41 | `aInvoiceBll.ProductDiscount` | — | — | — |
| 20 | `qtyTextBox_TextChanged` | `void qtyTextBox_TextChanged(object sender, EventArgs e)` | 47 | `aInvoiceBll.ProductDiscount` | — | — | — |
| 21 | `bQtyTextBox_TextChanged` | `void bQtyTextBox_TextChanged(object sender, EventArgs e)` | 14 | — | — | — | — |
| 22 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 58 | `aInvoiceBll.PostDeliveryConfPartialJournal`, `aInvoiceBll.ResetDeliveryConfirmation` | — | — | — |
| 23 | `DelDelivInvoiceDetail` | `bool DelDelivInvoiceDetail(int invoiceId)` | 158 | `aInvoiceBll.DCInfoWithDCId`, `aInvoiceBll.SaveDCStoreFreeze`, `aInvoiceBll.UpdateInvoiceDetail` | — | — | — |
| 24 | `SaveInvoiceDetail` | `bool SaveInvoiceDetail(int invoiceId)` | 174 | `aInvoiceBll.DCInfoWithDCId`, `aInvoiceBll.SaveDCStoreFreeze`, `aInvoiceBll.UpdateInvoiceDetail` | — | — | — |
| 25 | `SaveInvoice` | `int SaveInvoice()` | 67 | `aInvoiceBll.SaveInvoice`, `aInvoiceBll.UpdateInvoice`, `aOrderInfoBll.UpdateInvoiceStatus` | — | — | `LoginName`, `UserId` |
| 26 | `DelDelivInvoice` | `int DelDelivInvoice()` | 67 | `aInvoiceBll.SaveInvoice`, `aInvoiceBll.UpdateInvoiceNull`, `aOrderInfoBll.UpdateInvoiceStatus` | — | — | `LoginName`, `UserId` |
| 27 | `Clear` | `void Clear()` | 25 | — | — | — | — |
| 28 | `TotalValueCalculation` | `void TotalValueCalculation()` | 62 | — | — | — | — |
| 29 | `printButton_Click` | `void printButton_Click(object sender, EventArgs e)` | 9 | — | — | — | — |
| 30 | `custNameTextBox_TextChanged` | `void custNameTextBox_TextChanged(object sender, EventArgs e)` | 15 | — | — | — | — |
| 31 | `nameTextBox_TextChanged` | `void nameTextBox_TextChanged(object sender, EventArgs e)` | 24 | — | — | — | — |
| 32 | `dQtyTextBox_TextChanged` | `void dQtyTextBox_TextChanged(object sender, EventArgs e)` | 931 | `aOrderInfoBll.GetFixedCustomerfromInvoiceTable`, `aOrderInfoBll.GetTradeTerm`, `aOrderInfoBll.GetTradeTermOld`, `aOrderInfoBll.LoadInvoice`, `aOrderInfoBll.LoadOffer`, `aOrderInfoBll.ProductDiscount` | — | — | — |
| 33 | `CalcululateNetPrice` | `void CalcululateNetPrice(int rowindex)` | 77 | — | — | — | — |
| 34 | `sdTextBox_OnTextChanged` | `void sdTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 35 | `dodTextBox_TextChanged` | `void dodTextBox_TextChanged(object sender, EventArgs e)` | 22 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
DelivaryInvoiceCreationForCustomerByOrderAuto.aspx (markup)
  -> SInventory_UI_DelivaryInvoiceCreationForCustomerByOrderAuto (code-behind event handler)
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
| `Page_Load` | `InvoiceBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Validation` | `InvoiceBLL` | `CheckChartOfAccount` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetCustInfo` | `InvoiceBLL` | `CustomerMaster` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoadInvoice` | `InvoiceBLL` | `ReturnReason` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoadInvoice` | `OrderInfoBLL` | `LoadInvoice` | `string invoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `GetFixedCustomerfromInvoiceTable` | `string invoiceid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `GetTradeTerm` | `string amount` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `GetTradeTermOld` | `string amount` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `LoadOffer` | `string InvoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `LoadOrderWithDetail` | `string orderid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `ProductDiscount` | `string productcode, string customerId, string invoicedate` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAllDataByOrder` | `OrderInfoBLL` | `ProductVat` | `string productcode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `GetProduct` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetQty` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `qtyTextBox_TextChanged` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `PostDeliveryConfPartialJournal` | `int invoiceId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `ResetDeliveryConfirmation` | `int invId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DelDelivInvoiceDetail` | `InvoiceBLL` | `DCInfoWithDCId` | `string dcstoreId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DelDelivInvoiceDetail` | `InvoiceBLL` | `SaveDCStoreFreeze` | `DCStockNew aStockNew` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DelDelivInvoiceDetail` | `InvoiceBLL` | `UpdateInvoiceDetail` | `InvoiceDetail aInvoiceDetail` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `DCInfoWithDCId` | `string dcstoreId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `SaveDCStoreFreeze` | `DCStockNew aStockNew` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `UpdateInvoiceDetail` | `InvoiceDetail aInvoiceDetail` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `SaveInvoice` | `Invoice aInvoice, out int invoiceId, out string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `UpdateInvoice` | `Invoice aInvoice,out string invoiceno` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `OrderInfoBLL` | `UpdateInvoiceStatus` | `string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `DelDelivInvoice` | `InvoiceBLL` | `SaveInvoice` | `Invoice aInvoice, out int invoiceId, out string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DelDelivInvoice` | `InvoiceBLL` | `UpdateInvoiceNull` | `Invoice aInvoice` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DelDelivInvoice` | `OrderInfoBLL` | `UpdateInvoiceStatus` | `string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `GetFixedCustomerfromInvoiceTable` | `string invoiceid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `GetTradeTerm` | `string amount` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `GetTradeTermOld` | `string amount` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `LoadInvoice` | `string invoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `LoadOffer` | `string InvoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `ProductDiscount` | `string productcode, string customerId, string invoicedate` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dQtyTextBox_TextChanged` | `OrderInfoBLL` | `ProductVat` | `string productcode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_DeliveryConfirmationPartiallPosting` | saveButton_Click → `InvoiceBLL.PostDeliveryConfPartialJournal → InvoiceDAL.PostDeliveryConfPartialJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Sales_DeliverySalesPosting` | saveButton_Click → `InvoiceBLL.PostDeliveryConfPartialJournal → InvoiceDAL.PostDeliveryConfPartialJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Process_WeightedAverageFreezeStock` | DelDelivInvoiceDetail → `InvoiceBLL.SaveDCStoreFreeze → ChalanDAL.ProcessWeightedAverageFreezeStock → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Process_WeightedAverageFreezeStock` | SaveInvoiceDetail → `InvoiceBLL.SaveDCStoreFreeze → ChalanDAL.ProcessWeightedAverageFreezeStock → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_DCStoreCurrentStock` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblArea` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustCategory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `Validation()` → `InvoiceBLL.CheckChartOfAccount` → `InvoiceDAL.CheckChartOfAccount` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStore` | `DelDelivInvoiceDetail()` → `InvoiceBLL.DCInfoWithDCId` → `InvoiceDAL.DCInfoWithDCId` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.DCInfoWithDCId` → `InvoiceDAL.DCInfoWithDCId` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStoreFreeze` | `DelDelivInvoiceDetail()` → `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction`, `SaveInvoiceDetail()` → `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` |
| `tblDcWiseAreaInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `DelDelivInvoice()` → `InvoiceBLL.SaveInvoice` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoice`, `DelDelivInvoice()` → `InvoiceBLL.UpdateInvoiceNull` → `InvoiceDAL.UpdateInvoiceNull` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `LoadAllDataByOrder()` → `OrderInfoBLL.GetFixedCustomerfromInvoiceTable` → `OrderInfoDAL.GetFixedCustomerfromInvoiceTable` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOffer` → `OrderInfoDAL.LoadOffer` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `DelDelivInvoiceDetail()` → `InvoiceBLL.UpdateInvoiceDetail` → `InvoiceDAL.UpdateInvoiceDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoiceDetail()` → `InvoiceBLL.UpdateInvoiceDetail` → `InvoiceDAL.UpdateInvoiceDetail` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `DelDelivInvoice()` → `OrderInfoBLL.UpdateInvoiceStatus` → `OrderInfoDAL.UpdateInvoiceStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveInvoice()` → `OrderInfoBLL.UpdateInvoiceStatus` → `OrderInfoDAL.UpdateInvoiceStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblOrderDetail` | `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentType` | `Page_Load()` → `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `GetQty()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.LoadOrderWithDetail` → `OrderInfoDAL.LoadOrderWithDetail` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductDiscount` | `GetQty()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.ProductDiscount` → `OrderInfoDAL.ProductDiscount` → `ClsCommonInternalDAL.DataContainerDataTable`, `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblReturnReason` | `LoadInvoice()` → `InvoiceBLL.ReturnReason` → `InvoiceDAL.ReturnReason` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblTerritory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTradePolicy` | `LoadAllDataByOrder()` → `OrderInfoBLL.GetTradeTermOld` → `OrderInfoDAL.GetTradeTermOld` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.GetTradeTermOld` → `OrderInfoDAL.GetTradeTermOld` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTradePolicyNew` | `LoadAllDataByOrder()` → `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.GetTradeTerm` → `OrderInfoDAL.GetTradeTerm` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAllDataByOrder()` → `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable`, `dQtyTextBox_TextChanged()` → `OrderInfoBLL.ProductVat` → `OrderInfoDAL.ProductVat` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `LoadInvoice` | Full | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `dQtyTextBox_TextChanged` | Full | No | Yes | UNKNOWN | Notification |
| `dQtyTextBox_TextChanged` | Partial | No | Yes | UNKNOWN | Notification |
| `dQtyTextBox_TextChanged` | Reject | No | Yes | UNKNOWN | Notification |

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
| `ftQtyTextBox` | `tQtyTextBox` | Custom, Numbers | . |
| `fdQtyTextBox` | `dQtyTextBox` | Custom, Numbers | . |
| `dodTextBox11` | `dodTextBox` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-002-001` | Full | Code | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` → `LoadInvoice()` | user action on this page | operation is refused | "Full" | CONFIRMED |
| `BR-SAL-002-002` | {0} | Code | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-SAL-002-003` | Full | Code | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` → `dQtyTextBox_TextChanged()` | user action on this page | operation is refused | "Full" | CONFIRMED |
| `BR-SAL-002-004` | Partial | Code | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` → `dQtyTextBox_TextChanged()` | user action on this page | operation is refused | "Partial" | CONFIRMED |
| `BR-SAL-002-005` | Reject | Code | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` → `dQtyTextBox_TextChanged()` | user action on this page | operation is refused | "Reject" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `InvoiceId`, `LoginName`, `UserId`

Authentication check: performed by the master page (`MainMasterPage.master.cs` `Page_Load` redirects to `../Login.aspx` when `Session["UserId"]` is null). This page uses that master page, so the check applies.

## 21. Status Lifecycle

Status tokens appearing in this page: `Full`, `Partial`, `Reject`

Application-wide status vocabulary and the transitions that are actually implemented are documented in `specs/00-system/status-lifecycle.md`. Transitions executed inside stored procedures are **UNKNOWN**.

## 22. Error Handling

- Page level: no `try`/`catch` found in the code-behind — an exception surfaces as an ASP.NET yellow screen
- DAL level: `ClsCommonInternalDAL` re-throws (`throw ex;`, which resets the stack trace). `DB_Manager.ExecuteNonQueryAction` **swallows** the exception, rolls back the transaction, closes the connection and returns `0`, so the caller sees "no rows affected" rather than the error.
- Application level: `Global.asax` `Application_Error` is **empty** — there is no global error handler, no logging and no custom error page (`<customErrors>` is not configured in `Web.config`).
- User-visible failures are surfaced as `alert()` boxes injected with `ScriptManager.RegisterStartupScript`.

## 23. Audit

Audit columns written by this page: `UpdateBy`

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
| Database object | `tblReturnReason` |
| Database object | `tblTerritory` |
| Database object | `tblTradePolicy` |
| Database object | `tblTradePolicyNew` |
| Database object | `tblUnitPrice` |
| Stored procedure | `sp_DeliveryConfirmationPartiallPosting` |
| Stored procedure | `sp_Sales_DeliverySalesPosting` |
| Stored procedure | `sp_Process_WeightedAverageFreezeStock` |
| Stored procedure | `sp_Process_WeightedAverageFreezeStock` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-002-001` | Unauthenticated access | Request `/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-002-002` | Validation: Full | Trigger `LoadInvoice()` in the state that produces this message | alert "Full" and the operation is not performed | CONFIRMED |
| `TC-SAL-002-003` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-002-004` | Validation: Full | Trigger `dQtyTextBox_TextChanged()` in the state that produces this message | alert "Full" and the operation is not performed | CONFIRMED |
| `TC-SAL-002-005` | Validation: Partial | Trigger `dQtyTextBox_TextChanged()` in the state that produces this message | alert "Partial" and the operation is not performed | CONFIRMED |
| `TC-SAL-002-006` | Validation: Reject | Trigger `dQtyTextBox_TextChanged()` in the state that produces this message | alert "Reject" and the operation is not performed | CONFIRMED |
| `TC-SAL-002-007` | Grid loads | Open the page with data present | Grid `gridLineItemGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 93 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 35 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 4 names found | bodies **not in repo** | **PARTIAL** | section 16 |
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
- Writes/reads `tblReturnReason` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblTradePolicy` — any schema change to it breaks this page.
- Writes/reads `tblTradePolicyNew` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
