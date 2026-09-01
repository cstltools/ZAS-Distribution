# SAL-009 — InvoiceCreationForConsumerCustomer

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-009` |
| Module | `sales-order-invoice` |
| File name | `InvoiceCreationForConsumerCustomer.aspx` |
| Route | `/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `InvoiceCreationForConsumerCustomer.aspx.cs` |
| Code-behind class | `SInventory_UI_InvoiceCreationForCustomer` |
| Markup lines | 547 |
| Code-behind lines | 855 |
| Server controls | 68 |
| GridViews | 1 |
| Code-behind methods | 32 |

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

`/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |

## 7. UI Control Inventory

68 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 6 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 8 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 9 |
| 4 | `companyDropDownList` | asp:DropDownList | Company: |  | True |  |  | companyDropDownList_SelectedIndexChanged | 43 |
| 5 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  | True |  |  | dcDropDownList_SelectedIndexChanged | 51 |
| 6 | `invDateTextBox` | asp:TextBox | Invoice Date: |  |  | ReadOnly=True |  |  | 60 |
| 7 | `custNameTextBox` | asp:TextBox | Customer Name: |  | True |  |  | custNameTextBox_TextChanged | 70 |
| 8 | `custPhoneNoTextBox` | asp:TextBox | Customer Phone No: |  | True | ReadOnly=False |  | custPhoneNoTextBox_OnTextChanged | 77 |
| 9 | `custNameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender | ustomer Phone No: |  |  | Enabled=True |  |  | 79 |
| 10 | `custAddressTextBox` | asp:TextBox | Customer Address: |  |  |  |  |  | 92 |
| 11 | `payTypeDDL` | asp:DropDownList | Payment Type: |  |  |  |  |  | 100 |
| 12 | `gridLineItemGridView` | asp:GridView |  |  |  |  |  |  | 159 |
| 13 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 162 |
| 14 | `slLabel` | asp:Label | gridLineItemGridView → col[0] "SL" |  |  |  |  |  | 164 |
| 15 | `addImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineAdd.png |  |  |  | addImageButton_Click | 165 |
| 16 | `removeImageButton` | asp:ImageButton | gridLineItemGridView → col[0] "SL" | ~/images/lineDelete.png |  |  |  | removeImageButton_Click | 167 |
| 17 | `(no id)` | asp:TemplateField | bsp; |  |  |  |  |  | 171 |
| 18 | `codeTextBox` | asp:TextBox | gridLineItemGridView → col[1] "Product Code" |  | True |  |  | codeTextBox_TextChanged | 173 |
| 19 | `fcodeTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 175 |
| 20 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 179 |
| 21 | `nameTextBox` | asp:TextBox | gridLineItemGridView → col[2] "Product Name" |  | True |  |  | nameTextBox_TextChanged | 181 |
| 22 | `nameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 183 |
| 23 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 192 |
| 24 | `currentStockTextBox` | asp:TextBox | gridLineItemGridView → col[3] "CStock" |  |  | ReadOnly=True |  |  | 194 |
| 25 | `fcurrentStockTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 196 |
| 26 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 200 |
| 27 | `unitPriceTextBox` | asp:TextBox | gridLineItemGridView → col[4] "UP" |  |  | ReadOnly=True |  |  | 202 |
| 28 | `funitPriceTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 204 |
| 29 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 208 |
| 30 | `upVatTextBox` | asp:TextBox | gridLineItemGridView → col[5] "UVAT" |  |  | ReadOnly=True |  |  | 210 |
| 31 | `fupVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 212 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 216 |
| 33 | `qtyTextBox` | asp:TextBox | gridLineItemGridView → col[6] "Qty" |  | True |  |  | qtyTextBox_TextChanged | 218 |
| 34 | `fqtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 220 |
| 35 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 224 |
| 36 | `tpTextBox` | asp:TextBox | gridLineItemGridView → col[7] "TP" |  |  | ReadOnly=True |  |  | 226 |
| 37 | `ftpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 228 |
| 38 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 232 |
| 39 | `tpVatTextBox` | asp:TextBox | gridLineItemGridView → col[8] "TPVAT" |  |  | ReadOnly=True |  |  | 234 |
| 40 | `ftpVatTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 236 |
| 41 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 240 |
| 42 | `dpTextBox` | asp:TextBox | gridLineItemGridView → col[9] "DP" |  |  | ReadOnly=True |  |  | 242 |
| 43 | `fdpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 244 |
| 44 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 248 |
| 45 | `dpAmtTextBox` | asp:TextBox | gridLineItemGridView → col[10] "DAmt" |  | True |  |  | dpAmtTextBox_OnTextChanged | 250 |
| 46 | `fdpAmtTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 252 |
| 47 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 256 |
| 48 | `npTextBox` | asp:TextBox | gridLineItemGridView → col[11] "NP" |  |  | ReadOnly=True |  |  | 258 |
| 49 | `fnpTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 260 |
| 50 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 264 |
| 51 | `bQtyTextBox` | asp:TextBox | gridLineItemGridView → col[12] "BQty" |  | True | ReadOnly=True |  | bQtyTextBox_TextChanged | 266 |
| 52 | `fbQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 268 |
| 53 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 272 |
| 54 | `tQtyTextBox` | asp:TextBox | gridLineItemGridView → col[13] "TQty" |  |  | ReadOnly=True |  |  | 274 |
| 55 | `ftQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 276 |
| 56 | `tpTptalTextBox` | asp:TextBox | TP Total: |  |  | ReadOnly=True |  |  | 318 |
| 57 | `orderIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 329 |
| 58 | `vatTotalTextBox` | asp:TextBox | VAT Total: |  |  | ReadOnly=True |  |  | 338 |
| 59 | `disTotalTextBox` | asp:TextBox | Discount Total: |  |  | ReadOnly=True |  |  | 358 |
| 60 | `grandTotalTextBox` | asp:TextBox | Grand Total: |  |  | ReadOnly=True |  |  | 378 |
| 61 | `saveButton` | asp:Button |  | Invoice Generate |  |  |  | saveButton_Click | 418 |
| 62 | `invTextBox` | asp:TextBox | Print InvNo: |  |  |  |  |  | 460 |
| 63 | `printButton` | asp:Button |  | Print |  |  |  | printButton_Click | 463 |
| 64 | `hdCustomerMasterId` | asp:HiddenField |  |  |  |  |  |  | 471 |
| 65 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 472 |
| 66 | `hdMiaId` | asp:HiddenField |  |  |  |  |  |  | 475 |
| 67 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 538 |
| 68 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 541 |

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
| 1 | TemplateField | Product Code | — | `ProductCode` | `TextBox:codeTextBox`, `FilteredTextBoxExtender:codeTextBox` | — |
| 2 | TemplateField | Product Name | — | `ProductName` | `TextBox:nameTextBox`, `AutoCompleteExtender:nameTextBox` | — |
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

DAO / entity types instantiated by this page: `ConsumerCustomerDAO`, `Invoice`, `InvoiceDetail`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.PaymentTypeLoadBLL(...)` | `InvoiceBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `DropDownList` | `aInvoiceBll.LoadCompany(...)` | `InvoiceBLL.LoadCompany(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadCompany` | — | — | — |
| `GetCustInfo` | `aInvoiceBll.CustomerMaster(...)` | `InvoiceBLL.CustomerMaster(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — |
| `GetProduct` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `qtyTextBox_TextChanged` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |
| `saveButton_Click` | `aCustomerMasterDAL.CreateGlOfCustomer(...)` | `CustomerMasterInfoDal.CreateGlOfCustomer(int supplierId)` <br/>`Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | `CustomerMasterInfoDal.CreateGlOfCustomer` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_I_CustomerGl`, `sp_Integration_CustomerSubsidaryInsert` |
| `saveButton_Click` | `aInvoiceBll.PostDeliveryConfFullJournal(...)` | `InvoiceBLL.PostDeliveryConfFullJournal(int invoiceId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PostDeliveryConfFullJournal` → `InvoiceDAL.PostDeliveryConfFullJournal` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_DeliveryConfirmationFullPosting`, `sp_Sales_DeliverySalesPosting`, `sp_Sales_ExecuteDeliveryPipeline` |
| `saveButton_Click` | `aInvoiceBll.UpdateInvoiceConsumer(...)` | `InvoiceBLL.UpdateInvoiceConsumer(string invoiceID)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoiceConsumer` → `InvoiceDAL.UpdateInvoiceConsumer` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoice` | — |
| `saveButton_Click` | `aInvoiceBll.UpdateInvoiceDetailConsumer(...)` | `InvoiceBLL.UpdateInvoiceDetailConsumer(string invoiceID)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoiceDetailConsumer` → `InvoiceDAL.UpdateInvoiceDetailConsumer` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoiceDetail` | — |
| `OrderSaveDetail` | `aInvoiceBll.ProductInfo(...)` | `InvoiceBLL.ProductInfo(string comUnitId, string productCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_DCStoreCurrentStock`, `tblUnitPrice` | — |
| `OrderSaveDetail` | `aInvoiceBll.SaveOrderDetail(...)` | `InvoiceBLL.SaveOrderDetail(OrderInfoDetail aOrderInfoDetail)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveOrderDetail` → `InvoiceDAL.SaveOrderDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblOrderDetail` | — |
| `OrderSaveMaster` | `aInvoiceBll.GetConsumerCustomerInfo(...)` | `InvoiceBLL.GetConsumerCustomerInfo(string custId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetConsumerCustomerInfo` → `InvoiceDAL.GetConsumerCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblConsumerCustomer` | — |
| `OrderSaveMaster` | `aInvoiceBll.SaveOrderMaster(...)` | `InvoiceBLL.SaveOrderMaster(OrderInfoMaster aOrderInfoMaster)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveOrderMaster` → `InvoiceDAL.SaveOrderMaster` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblOrder` | — |
| `OrderSaveMaster` | `aOrderInfoBll.OrdNo(...)` | `OrderListBLL.OrdNo()` <br/>`Library.BLL/SInventory_BLL/OrderListBLL.cs` | `OrderListBLL.OrdNo` → `OrderListDAL.OrderManualId` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListBLL.OrdnNoGenerator` | SELECT | `tblOrder` | — |
| `ConsumeCustomer` | `aInvoiceBll.SaveConsumerCustomer(...)` | `InvoiceBLL.SaveConsumerCustomer(ConsumerCustomerDAO aConsumerCustomerDao,out int id)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveConsumerCustomer` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveConsumerCustomer` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.CustCodeGenerator` | INSERT, SELECT | `tblConsumerCustomer` | — |
| `SaveInvoiceDetail` | `aInvoiceBll.SaveInvoiceDetails(...)` | `InvoiceBLL.SaveInvoiceDetails(List<InvoiceDetail> aInvoiceList,string comUnitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.UpdateDCStoreQuantity` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `InvoiceBLL.InvoiceDetailInsertBLL` → `ClsPrimaryKeyFind.PrimaryKeyMax` | INSERT, SELECT, UPDATE | `tblDCStore`, `tblInvoiceDetail` | — |
| `SaveInvoice` | `aInvoiceBll.SaveInvoiceConsumer(...)` | `InvoiceBLL.SaveInvoiceConsumer(Invoice aInvoice, out int invoiceId, out string invoiceNo)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveInvoiceConsumer` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoiceConsumer` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.InvoiceNoGenerator` → `InvoiceDAL.InvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblInvoice` | — |
| `SaveInvoice` | `aInvoiceBll.UpdateInvoice(...)` | `InvoiceBLL.UpdateInvoice(Invoice aInvoice,out string invoiceno)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.UpdateInvoice` → `InvoiceDAL.UpdateInvoice` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `InvoiceBLL.DelInvoiceNoGenerator` → `InvoiceDAL.InvoiceNoCount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT, UPDATE | `tblInvoice` | — |
| `companyDropDownList_SelectedIndexChanged` | `aInvoiceBll.LoadDC(...)` | `InvoiceBLL.LoadDC(DropDownList aDropDownList, string companyId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadDC` | — | — | — |
| `ConsumeCustomer` | `aInvoiceBll.GetConsumerCustomerInfo(...)` | `InvoiceBLL.GetConsumerCustomerInfo(string custId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetConsumerCustomerInfo` → `InvoiceDAL.GetConsumerCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblConsumerCustomer` | — |
| `dpAmtTextBox_OnTextChanged` | `aInvoiceBll.ProductDiscount(...)` | `InvoiceBLL.ProductDiscount(string productCode, string customerId,string date)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductDiscount` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyDropDownList` | Company: | asp:DropDownList | True | companyDropDownList_SelectedIndexChanged | `aInvoiceBll.LoadCompany()` in `DropDownList()`; `aInvoiceBll.LoadDC()` in `companyDropDownList_SelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center: | asp:DropDownList | True | dcDropDownList_SelectedIndexChanged | `aInvoiceBll.LoadDC()` in `companyDropDownList_SelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
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
| `addImageButton` | asp:ImageButton |  | — | `addImageButton_Click` | 74 lines | — | — |
| `removeImageButton` | asp:ImageButton |  | — | `removeImageButton_Click` | 66 lines | — | — |
| `saveButton` | asp:Button | Invoice Generate | — | `saveButton_Click` | 27 lines | `aCustomerMasterDAL.CreateGlOfCustomer`, `aInvoiceBll.PostDeliveryConfFullJournal`, `aInvoiceBll.UpdateInvoiceConsumer`, `aInvoiceBll.UpdateInvoiceDetailConsumer` | — |
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
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 9 | `aInvoiceBll.PaymentTypeLoadBLL` | — | — | — |
| 2 | `Todate` | `void Todate()` | 3 | — | — | — | — |
| 3 | `DropDownList` | `void DropDownList()` | 3 | `aInvoiceBll.LoadCompany` | — | — | — |
| 4 | `Validation` | `bool Validation()` | 48 | — | — | — | — |
| 5 | `GetCustInfo` | `void GetCustInfo(string custCode)` | 26 | `aInvoiceBll.CustomerMaster` | — | — | — |
| 6 | `custCodeTextBox_TextChanged` | `void custCodeTextBox_TextChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 7 | `InitialGrid` | `void InitialGrid()` | 51 | — | — | — | — |
| 8 | `addImageButton_Click` | `void addImageButton_Click(object sender, ImageClickEventArgs e)` | 74 | — | — | — | — |
| 9 | `removeImageButton_Click` | `void removeImageButton_Click(object sender, ImageClickEventArgs e)` | 66 | — | — | — | — |
| 10 | `codeTextBox_TextChanged` | `void codeTextBox_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 11 | `GetProduct` | `void GetProduct(int rowindex, string productCode)` | 39 | `aInvoiceBll.ProductInfo` | — | — | — |
| 12 | `ProductCodeValidation` | `bool ProductCodeValidation(string productCode, int rowindex)` | 16 | — | — | — | — |
| 13 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 14 | `qtyTextBox_TextChanged` | `void qtyTextBox_TextChanged(object sender, EventArgs e)` | 43 | `aInvoiceBll.ProductDiscount` | — | — | — |
| 15 | `bQtyTextBox_TextChanged` | `void bQtyTextBox_TextChanged(object sender, EventArgs e)` | 14 | — | — | — | — |
| 16 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 27 | `aCustomerMasterDAL.CreateGlOfCustomer`, `aInvoiceBll.PostDeliveryConfFullJournal`, `aInvoiceBll.UpdateInvoiceConsumer`, `aInvoiceBll.UpdateInvoiceDetailConsumer` | — | — | — |
| 17 | `UpdateDeliveryInfo` | `void UpdateDeliveryInfo()` | 3 | — | — | — | — |
| 18 | `OrderSaveDetail` | `int OrderSaveDetail(int idmain,int i)` | 32 | `aInvoiceBll.ProductInfo`, `aInvoiceBll.SaveOrderDetail` | — | — | — |
| 19 | `OrderSaveMaster` | `int OrderSaveMaster(out string ordercode)` | 37 | `aInvoiceBll.GetConsumerCustomerInfo`, `aInvoiceBll.SaveOrderMaster`, `aOrderInfoBll.OrdNo` | — | — | `LoginName` |
| 20 | `ConsumeCustomer` | `int ConsumeCustomer()` | 12 | `aInvoiceBll.SaveConsumerCustomer` | — | — | — |
| 21 | `SaveInvoiceDetail` | `bool SaveInvoiceDetail(int invoiceId)` | 68 | `aInvoiceBll.SaveInvoiceDetails` | — | — | — |
| 22 | `SaveInvoice` | `int SaveInvoice()` | 41 | `aInvoiceBll.SaveInvoiceConsumer`, `aInvoiceBll.UpdateInvoice` | — | — | `LoginName`, `UserId` |
| 23 | `Clear` | `void Clear()` | 14 | — | — | — | — |
| 24 | `TotalValueCalculation` | `void TotalValueCalculation()` | 21 | — | — | — | — |
| 25 | `printButton_Click` | `void printButton_Click(object sender, EventArgs e)` | 6 | — | — | — | — |
| 26 | `custNameTextBox_TextChanged` | `void custNameTextBox_TextChanged(object sender, EventArgs e)` | 15 | — | — | — | — |
| 27 | `nameTextBox_TextChanged` | `void nameTextBox_TextChanged(object sender, EventArgs e)` | 23 | — | — | — | — |
| 28 | `companyDropDownList_SelectedIndexChanged` | `void companyDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 13 | `aInvoiceBll.LoadDC` | — | — | — |
| 29 | `custPhoneNoTextBox_OnTextChanged` | `void custPhoneNoTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 30 | `ConsumeCustomer` | `void ConsumeCustomer(string custId)` | 8 | `aInvoiceBll.GetConsumerCustomerInfo` | — | — | — |
| 31 | `dcDropDownList_SelectedIndexChanged` | `void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 32 | `dpAmtTextBox_OnTextChanged` | `void dpAmtTextBox_OnTextChanged(object sender, EventArgs e)` | 43 | `aInvoiceBll.ProductDiscount` | — | — | — |

Call chain shape for every persisting action on this page:

```
InvoiceCreationForConsumerCustomer.aspx (markup)
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
| `custNameTextBox_AutoCompleteExtender` | `custPhoneNoTextBox` | `GetConsumerCustomer` | `SInventoryWebService.asmx` | 1 |
| `nameTextBox_AutoCompleteExtender` | `nameTextBox` | `GetProductByMenufracturer` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `InvoiceBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DropDownList` | `InvoiceBLL` | `LoadCompany` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetCustInfo` | `InvoiceBLL` | `CustomerMaster` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetProduct` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `qtyTextBox_TextChanged` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `PostDeliveryConfFullJournal` | `int invoiceId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `UpdateInvoiceConsumer` | `string invoiceID` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `UpdateInvoiceDetailConsumer` | `string invoiceID` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `OrderSaveDetail` | `InvoiceBLL` | `ProductInfo` | `string comUnitId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `OrderSaveDetail` | `InvoiceBLL` | `SaveOrderDetail` | `OrderInfoDetail aOrderInfoDetail` | `int` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `OrderSaveMaster` | `InvoiceBLL` | `GetConsumerCustomerInfo` | `string custId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `OrderSaveMaster` | `InvoiceBLL` | `SaveOrderMaster` | `OrderInfoMaster aOrderInfoMaster` | `int` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `OrderSaveMaster` | `OrderListBLL` | `OrdNo` | `` | `string` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `ConsumeCustomer` | `InvoiceBLL` | `SaveConsumerCustomer` | `ConsumerCustomerDAO aConsumerCustomerDao,out int id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoiceDetail` | `InvoiceBLL` | `SaveInvoiceDetails` | `List<InvoiceDetail> aInvoiceList,string comUnitId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `SaveInvoiceConsumer` | `Invoice aInvoice, out int invoiceId, out string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `SaveInvoice` | `InvoiceBLL` | `UpdateInvoice` | `Invoice aInvoice,out string invoiceno` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `companyDropDownList_SelectedIndexChanged` | `InvoiceBLL` | `LoadDC` | `DropDownList aDropDownList, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `ConsumeCustomer` | `InvoiceBLL` | `GetConsumerCustomerInfo` | `string custId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `dpAmtTextBox_OnTextChanged` | `InvoiceBLL` | `ProductDiscount` | `string productCode, string customerId,string date` | `decimal` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `saveButton_Click` | `CustomerMasterInfoDal` | `CreateGlOfCustomer` | `int supplierId` | `bool` | — | — | `sp_I_CustomerGl`, `sp_Integration_CustomerSubsidaryInsert` | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_I_CustomerGl` | saveButton_Click → `CustomerMasterInfoDal.CreateGlOfCustomer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Integration_CustomerSubsidaryInsert` | saveButton_Click → `CustomerMasterInfoDal.CreateGlOfCustomer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_DeliveryConfirmationFullPosting` | saveButton_Click → `InvoiceBLL.PostDeliveryConfFullJournal → InvoiceDAL.PostDeliveryConfFullJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Sales_DeliverySalesPosting` | saveButton_Click → `InvoiceBLL.PostDeliveryConfFullJournal → InvoiceDAL.PostDeliveryConfFullJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Sales_ExecuteDeliveryPipeline` | saveButton_Click → `InvoiceBLL.PostDeliveryConfFullJournal → InvoiceDAL.PostDeliveryConfFullJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_DCStoreCurrentStock` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `OrderSaveDetail()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblArea` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblConsumerCustomer` | `ConsumeCustomer()` → `InvoiceBLL.GetConsumerCustomerInfo` → `InvoiceDAL.GetConsumerCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `ConsumeCustomer()` → `InvoiceBLL.SaveConsumerCustomer` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveConsumerCustomer`, `OrderSaveMaster()` → `InvoiceBLL.GetConsumerCustomerInfo` → `InvoiceDAL.GetConsumerCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustCategory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStore` | `SaveInvoiceDetail()` → `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDcWiseAreaInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `SaveInvoice()` → `InvoiceBLL.SaveInvoiceConsumer` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceDAL.SaveDataForInvoiceConsumer`, `SaveInvoice()` → `InvoiceBLL.UpdateInvoice` → `InvoiceDAL.UpdateInvoice` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `InvoiceBLL.DelInvoiceNoGenerator`, `saveButton_Click()` → `InvoiceBLL.UpdateInvoiceConsumer` → `InvoiceDAL.UpdateInvoiceConsumer` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblInvoiceDetail` | `SaveInvoiceDetail()` → `InvoiceBLL.SaveInvoiceDetails` → `InvoiceBLL.UpdateDCStock` → `InvoiceDAL.BatchWiseProductQty` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `InvoiceBLL.UpdateInvoiceDetailConsumer` → `InvoiceDAL.UpdateInvoiceDetailConsumer` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblMIOInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `OrderSaveMaster()` → `InvoiceBLL.SaveOrderMaster` → `InvoiceDAL.SaveOrderMaster` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity`, `OrderSaveMaster()` → `OrderListBLL.OrdNo` → `OrderListDAL.OrderManualId` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListBLL.OrdnNoGenerator` |
| `tblOrderDetail` | `OrderSaveDetail()` → `InvoiceBLL.SaveOrderDetail` → `InvoiceDAL.SaveOrderDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` |
| `tblPaymentType` | `Page_Load()` → `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `dpAmtTextBox_OnTextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductDiscount` | `dpAmtTextBox_OnTextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `qtyTextBox_TextChanged()` → `InvoiceBLL.ProductDiscount` → `InvoiceDAL.DiscountofProduct` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `GetProduct()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `OrderSaveDetail()` → `InvoiceBLL.ProductInfo` → `InvoiceDAL.ProductInfoDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |

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
| `BR-SAL-009-001` | {0} | Code | `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `UserId`

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
| Assembly source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Database object | `View_DCStoreCurrentStock` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblConsumerCustomer` |
| Database object | `tblCustCategory` |
| Database object | `tblCustMaster` |
| Database object | `tblDCStore` |
| Database object | `tblDcWiseAreaInfo` |
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
| Database object | `tblTerritory` |
| Database object | `tblUnitPrice` |
| Stored procedure | `sp_I_CustomerGl` |
| Stored procedure | `sp_Integration_CustomerSubsidaryInsert` |
| Stored procedure | `sp_DeliveryConfirmationFullPosting` |
| Stored procedure | `sp_Sales_DeliverySalesPosting` |
| Stored procedure | `sp_Sales_ExecuteDeliveryPipeline` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-009-001` | Unauthenticated access | Request `/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-009-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-009-003` | Grid loads | Open the page with data present | Grid `gridLineItemGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 68 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 32 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 5 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 21 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- `Library.BLL/SInventory_BLL/OrderListBLL.cs` is **shared with 20 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` is **shared with 5 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `Dashboard.aspx`, `FixedDiscountEntry.aspx`, `SupplierInformation.aspx`) — a change there is not local.
- Writes/reads `View_DCStoreCurrentStock` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblConsumerCustomer` — any schema change to it breaks this page.
- Writes/reads `tblCustCategory` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
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
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
