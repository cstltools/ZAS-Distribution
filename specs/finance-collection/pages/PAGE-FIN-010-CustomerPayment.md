# FIN-010 — CustomerPayment

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `FIN-010` |
| Module | `finance-collection` |
| File name | `CustomerPayment.aspx` |
| Route | `/SInventory_UI/CustomerPayment.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `CustomerPayment.aspx.cs` |
| Code-behind class | `SInventory_UI_CustomerPayment` |
| Markup lines | 556 |
| Code-behind lines | 935 |
| Server controls | 43 |
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

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

Outbound `Response.Redirect` targets: `Login.aspx`

## 5. Route

`/SInventory_UI/CustomerPayment.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerPayment.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |

## 7. UI Control Inventory

43 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 60 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 61 |
| 4 | `manufacDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | ddlCompany_OnSelectedIndexChanged | 91 |
| 5 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  | True |  |  | dcDropDownList_SelectedIndexChanged | 99 |
| 6 | `customerCheckBox` | asp:CheckBox |  |  | True |  |  | customerCheckBox_OnCheckedChanged | 112 |
| 7 | `refNameTextBox` | asp:TextBox | Reference No: |  |  |  |  |  | 123 |
| 8 | `totalBalance` | asp:Label | Total Balance: |  |  |  |  |  | 129 |
| 9 | `delDropDownList` | asp:DropDownList | Delivery Man: |  | True |  |  | delDropDownList_OnTextChanged | 138 |
| 10 | `dateTextBox` | asp:TextBox | Batch Creation Date: |  | True |  |  | dateTextBox_TextChanged | 146 |
| 11 | `imgDate` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 148 |
| 12 | `Date` | asp:CalendarExtender |  |  |  |  |  |  | 150 |
| 13 | `batchDropDownList` | asp:DropDownList | Batch No: |  | True |  |  | batchDropDownList_SelectedIndexChanged | 158 |
| 14 | `customerDropDownList` | asp:DropDownList |  |  | True |  |  | customerDropDownList_SelectedIndexChanged | 169 |
| 15 | `singleCustomerTextBox` | asp:TextBox | Customer Name: |  | True | Enabled=False |  | singleCustomerTextBox_OnTextChanged | 177 |
| 16 | `customerTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 178 |
| 17 | `hdCustomerId` | asp:HiddenField |  |  |  |  |  |  | 185 |
| 18 | `paymentDtTextBox` | asp:Label | Payment Date: |  | True | ReadOnly=True |  |  | 191 |
| 19 | `paymentAmountTextBox` | asp:Label | Payment Amount: |  |  | ReadOnly=True |  |  | 198 |
| 20 | `payTypeDDL` | asp:DropDownList | Payment Type: |  | True |  |  | payTypeDDL_OnSelectedIndexChanged | 212 |
| 21 | `ddlBank` | asp:DropDownList | Bank Account: |  | True |  |  |  | 220 |
| 22 | `ddlCash` | asp:DropDownList | Cash Account: |  | True |  |  |  | 227 |
| 23 | `customerTextBox` | asp:TextBox | Customer |  | True |  |  | customerTextBox_TextChanged | 259 |
| 24 | `submitButton` | asp:Button |  | Search |  |  |  | Button1_Click2 | 295 |
| 25 | `saveButton` | asp:Button |  | Submit |  | Enabled=False |  | saveButton_Click | 297 |
| 26 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 300 |
| 27 | `orderGridView` | asp:GridView |  |  |  |  |  |  | 329 |
| 28 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 332 |
| 29 | `chkSelectAll` | asp:CheckBox | orderGridView → col[0] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 334 |
| 30 | `chkSelect` | asp:CheckBox | orderGridView → col[0] "(no header)" |  | True |  |  | chkSelect_CheckedChanged | 337 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 340 |
| 32 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 341 |
| 33 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 342 |
| 34 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 343 |
| 35 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 344 |
| 36 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 345 |
| 37 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 346 |
| 38 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 347 |
| 39 | `payAmountTextBox` | asp:TextBox | orderGridView → col[8] "Pay Amount" |  | True |  |  | payAmountTextBox_TextChanged | 349 |
| 40 | `FilteredTextBoxExtenderunitValue` | asp:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 350 |
| 41 | `refDtTextBox` | asp:TextBox | Ref Date |  | True |  |  |  | 389 |
| 42 | `paymentDtTextBox_CalendarExtender0` | asp:CalendarExtender | Ref Date |  |  |  |  |  | 390 |
| 43 | `ImageButton0` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 393 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `orderGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceId,CustomerMasterId,CustomerCode` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |
| 1 | BoundField | Pro.Invoice No | InvoiceNo | — | — | — |
| 2 | BoundField | Pro.Invoice Date | InvoiceDate | — | — | — |
| 3 | BoundField | Del Invoice No | DelivaryInvoiceNo | — | — | — |
| 4 | BoundField | Del Invoice Date | UpdateDate | — | — | — |
| 5 | BoundField | Del Inv Amount | DeliveryTpGrandTotal | — | — | — |
| 6 | BoundField | Previous Pay | PaymentAmount | — | — | — |
| 7 | BoundField | Due Amount | Due | — | — | — |
| 8 | TemplateField | Pay Amount | — | — | `TextBox:payAmountTextBox`, `FilteredTextBoxExtender:payAmountTextBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CreditAdjustmentDao`, `CustPayment`, `CustomerMaster`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `DropDownList` | `aCustPaymentBll.PaymentTypeLoadBLL(...)` | `CustPaymentBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.PaymentTypeLoadBLL` → `CustPaymentDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `DropDownList` | `aInvoiceBatchBll.LoadDeliveryMan(...)` | `InvoiceBatchBLL.LoadDeliveryMan(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` | `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `DropDownList` | `aOrderInfoBll.LoadManufac(...)` | `OrderInfoBLL.LoadManufac(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyInfo` | — |
| `customerTextBox_TextChanged` | `aCustPaymentBll.LoadInvoiceNew(...)` | `CustPaymentBLL.LoadInvoiceNew(string batchId, string customerId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType` | — |
| `CustomerId` | `aCustPaymentBll.CustomerLoad(...)` | `CustPaymentBLL.CustomerLoad(string aCustomerMaster)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblCustMaster` | — |
| `Validation` | `aInvoiceBll.CheckChartOfAccount(...)` | `InvoiceBLL.CheckChartOfAccount(string customerCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CheckChartOfAccount` → `InvoiceDAL.CheckChartOfAccount` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster` | — |
| `Validation` | `aInvoiceBll.GetFinancialYearInfo(...)` | `InvoiceBLL.GetFinancialYearInfo(string invoiceDate,string unitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit` | — |
| `saveButton_Click` | `aCustPaymentBll.SaveCustPayment(...)` | `CustPaymentBLL.SaveCustPayment(CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDetailList)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustPaymentDAL.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.PaymentJournalPosting` | DELETE, INSERT, SELECT | `tblCustPayDetail`, `tblCustomerPay` | `sp_CustomerPaymentPosting`, `sp_Sales_CustomerPaymentPosting`, `sp_Save_SaveCustPayment` |
| `saveButton_Click` | `aCustPaymentBll.UpdateInvoicePaymentAmount(...)` | `CustPaymentBLL.UpdateInvoicePaymentAmount(string amount, string status, string id)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.UpdateInvoicePaymentAmount` → `CustPaymentDAL.UpdateInvoicePaymentAmount` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoice` | `sp_UD_InvoicePayment` |
| `saveButton_Click` | `aInvoiceBll.GetCompanyIdByInvoice(...)` | `InvoiceBLL.GetCompanyIdByInvoice(string invoiceId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetCompanyIdByInvoice` → `InvoiceDAL.GetCompanyIdByInvoiceId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice` | — |
| `saveButton_Click` | `aInvoiceBll.SaveCreditAdjustmentInfo(...)` | `InvoiceBLL.SaveCreditAdjustmentInfo(CreditAdjustmentDao adjustmentDao)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveCreditAdjustmentInfo` → `InvoiceDAL.SaveCreditAdjustmentInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT | `tblCreditAdjustment` | — |
| `batchDropDownList_SelectedIndexChanged` | `aCustPaymentBll.LoadCustomer(...)` | `CustPaymentBLL.LoadCustomer(DropDownList ddl, string batchId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadCustomer` | — | — | — |
| `Button1_Click1` | `aCustPaymentBll.LoadInvoiceNew(...)` | `CustPaymentBLL.LoadInvoiceNew(string batchId, string customerId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType` | — |
| `Button1_Click2` | `aCustPaymentBll.LoadInvoiceByCustomer(...)` | `CustPaymentBLL.LoadInvoiceByCustomer(string custmerId, string dcId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType` | — |
| `Button1_Click2` | `aCustPaymentBll.LoadInvoiceByCustomerByUser(...)` | `CustPaymentBLL.LoadInvoiceByCustomerByUser(string custmerId, string userId, string dc)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType` | — |
| `Button1_Click2` | `aCustPaymentBll.LoadInvoiceNew(...)` | `CustPaymentBLL.LoadInvoiceNew(string batchId, string customerId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType` | — |
| `Button1_Click2` | `aCustPaymentBll.LoadInvoiceNewByUser(...)` | `CustPaymentBLL.LoadInvoiceNewByUser(string batchId, string customerId, string userId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType`, `tblUserCompanyUnit` | — |
| `FindBatchInfo` | `aOrderInfoBll.BatchNoLoad(...)` | `OrderInfoBLL.BatchNoLoad(DropDownList aDownList, string companyId, string date, string delmanId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblInvoiceBatch` | — |
| `FindBatchInfo` | `aOrderInfoBll.BatchNoLoadByUser(...)` | `OrderInfoBLL.BatchNoLoadByUser(DropDownList aDownList, string date, string delmanId, string userId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblInvoice`, `tblInvoiceBatch`, `tblUserCompanyUnit` | — |
| `singleCustomerTextBox_OnTextChanged` | `aCustPaymentBll.GetCustomerInfoByCode(...)` | `CustPaymentBLL.GetCustomerInfoByCode(string customerCode)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.GetCustomerInfoByCode` | — | — | — |
| `SetBalanceInfo` | `aCustPaymentBll.GetCreditBalanceInfo(...)` | `CustPaymentBLL.GetCreditBalanceInfo(string value)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.GetCreditBalanceInfo` → `CustPaymentDAL.GetCreditBalanceInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCreditAdjustment` | — |
| `payTypeDDL_OnSelectedIndexChanged` | `aCustPaymentBll.LoadBankAccount(...)` | `CustPaymentBLL.LoadBankAccount(DropDownList ddlBank, string companyId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadBankAccount` → `CustPaymentDAL.LoadBankAccount` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | — | — |
| `payTypeDDL_OnSelectedIndexChanged` | `aCustPaymentBll.LoadCashAccount(...)` | `CustPaymentBLL.LoadCashAccount(DropDownList ddlCash, string companyId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadCashAccount` → `CustPaymentDAL.LoadCashAccount` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | — | — |
| `ddlCompany_OnSelectedIndexChanged` | `aOtherStockActionBLL.LoadDcInformationByUser(...)` | `OtherStockActionBLL.LoadDcInformationByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `ddlCompany_OnSelectedIndexChanged` | `aOtherStockActionBLL.LoadWhInformation(...)` | `OtherStockActionBLL.LoadWhInformation(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `manufacDropDownList` | Company Name: | asp:DropDownList | True | ddlCompany_OnSelectedIndexChanged | `aCustPaymentBll.LoadBankAccount()` in `payTypeDDL_OnSelectedIndexChanged()`; `aCustPaymentBll.LoadCashAccount()` in `payTypeDDL_OnSelectedIndexChanged()`; `aOrderInfoBll.BatchNoLoad()` in `FindBatchInfo()`; `aOrderInfoBll.LoadManufac()` in `DropDownList()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoiceBatch`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center: | asp:DropDownList | True | dcDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadInvoiceByCustomer()` in `Button1_Click2()`; `aCustPaymentBll.LoadInvoiceByCustomerByUser()` in `Button1_Click2()`; `aOtherStockActionBLL.LoadDcInformationByUser()` in `ddlCompany_OnSelectedIndexChanged()`; `aOtherStockActionBLL.LoadWhInformation()` in `ddlCompany_OnSelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `delDropDownList` | Delivery Man: | asp:DropDownList | True | delDropDownList_OnTextChanged | `aInvoiceBatchBll.LoadDeliveryMan()` in `DropDownList()`; `aOrderInfoBll.BatchNoLoad()` in `FindBatchInfo()`; `aOrderInfoBll.BatchNoLoadByUser()` in `FindBatchInfo()` | `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `batchDropDownList` | Batch No: | asp:DropDownList | True | batchDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadCustomer()` in `batchDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadInvoiceNew()` in `Button1_Click1()`; `aCustPaymentBll.LoadInvoiceNew()` in `Button1_Click2()`; `aCustPaymentBll.LoadInvoiceNew()` in `customerTextBox_TextChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceBatch`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `customerDropDownList` |  | asp:DropDownList | True | customerDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadCustomer()` in `batchDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadInvoiceNew()` in `Button1_Click2()`; `aCustPaymentBll.LoadInvoiceNewByUser()` in `Button1_Click2()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `payTypeDDL` | Payment Type: | asp:DropDownList | True | payTypeDDL_OnSelectedIndexChanged | `aCustPaymentBll.PaymentTypeLoadBLL()` in `DropDownList()` | `tblPaymentType` | set in code (see the DAL method above) |
| `ddlBank` | Bank Account: | asp:DropDownList | True | — | `aCustPaymentBll.LoadBankAccount()` in `payTypeDDL_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `ddlCash` | Cash Account: | asp:DropDownList | True | — | `aCustPaymentBll.LoadCashAccount()` in `payTypeDDL_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |

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
| `imgDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `submitButton` | asp:Button | Search | — | `Button1_Click2` | 117 lines | `aCustPaymentBll.LoadInvoiceByCustomer`, `aCustPaymentBll.LoadInvoiceByCustomerByUser`, `aCustPaymentBll.LoadInvoiceNew`, `aCustPaymentBll.LoadInvoiceNewByUser` | — |
| `saveButton` | asp:Button | Submit | return confirm('Are you sure you want to Save ?'); | `saveButton_Click` | 143 lines | `aCustPaymentBll.SaveCustPayment`, `aCustPaymentBll.UpdateInvoicePaymentAmount`, `aInvoiceBll.GetCompanyIdByInvoice`, `aInvoiceBll.SaveCreditAdjustmentInfo` | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |
| `ImageButton0` | asp:ImageButton |  | — | `—` | — | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 9 | — | — | — | — |
| 2 | `Clear` | `void Clear()` | 28 | — | — | — | — |
| 3 | `DropDownList` | `void DropDownList()` | 9 | `aCustPaymentBll.PaymentTypeLoadBLL`, `aInvoiceBatchBll.LoadDeliveryMan`, `aOrderInfoBll.LoadManufac` | — | — | — |
| 4 | `customerTextBox_TextChanged` | `void customerTextBox_TextChanged(object sender, EventArgs e)` | 18 | `aCustPaymentBll.LoadInvoiceNew` | — | — | — |
| 5 | `CustomerId` | `int CustomerId(out CustomerMaster aCustomerMaster)` | 7 | `aCustPaymentBll.CustomerLoad` | — | — | — |
| 6 | `customerDropDownList_SelectedIndexChanged` | `void customerDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 7 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 8 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 20 | — | — | — | — |
| 9 | `SaveButtonVisibility` | `void SaveButtonVisibility()` | 27 | — | — | — | — |
| 10 | `dateTextBox_TextChanged` | `void dateTextBox_TextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 11 | `Validation` | `bool Validation()` | 137 | `aInvoiceBll.CheckChartOfAccount`, `aInvoiceBll.GetFinancialYearInfo` | — | — | — |
| 12 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 143 | `aCustPaymentBll.SaveCustPayment`, `aCustPaymentBll.UpdateInvoicePaymentAmount`, `aInvoiceBll.GetCompanyIdByInvoice`, `aInvoiceBll.SaveCreditAdjustmentInfo` | — | — | `LoginName` |
| 13 | `payAmountTextBox_TextChanged` | `void payAmountTextBox_TextChanged(object sender, EventArgs e)` | 26 | — | — | — | — |
| 14 | `batchDropDownList_SelectedIndexChanged` | `void batchDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 11 | `aCustPaymentBll.LoadCustomer` | — | — | — |
| 15 | `Button1_Click1` | `void Button1_Click1(object sender, EventArgs e)` | 20 | `aCustPaymentBll.LoadInvoiceNew` | — | — | — |
| 16 | `Button1_Click2` | `void Button1_Click2(object sender, EventArgs e)` | 117 | `aCustPaymentBll.LoadInvoiceByCustomer`, `aCustPaymentBll.LoadInvoiceByCustomerByUser`, `aCustPaymentBll.LoadInvoiceNew`, `aCustPaymentBll.LoadInvoiceNewByUser` | — | — | `UserId`, `UserType` |
| 17 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 18 | `delDropDownList_OnTextChanged` | `void delDropDownList_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 19 | `FindBatchInfo` | `void FindBatchInfo()` | 40 | `aOrderInfoBll.BatchNoLoad`, `aOrderInfoBll.BatchNoLoadByUser` | — | — | `UserId`, `UserType` |
| 20 | `manufacDropDownList_OnSelectedIndexChanged` | `void manufacDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 21 | `chkSelect_CheckedChanged` | `void chkSelect_CheckedChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 22 | `ClaculatePaymentAmount` | `void ClaculatePaymentAmount()` | 32 | — | — | — | — |
| 23 | `singleCustomerTextBox_OnTextChanged` | `void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)` | 39 | `aCustPaymentBll.GetCustomerInfoByCode` | — | — | — |
| 24 | `SetBalanceInfo` | `void SetBalanceInfo()` | 17 | `aCustPaymentBll.GetCreditBalanceInfo` | — | — | — |
| 25 | `customerCheckBox_OnCheckedChanged` | `void customerCheckBox_OnCheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 26 | `payTypeDDL_OnSelectedIndexChanged` | `void payTypeDDL_OnSelectedIndexChanged(object sender, EventArgs e)` | 48 | `aCustPaymentBll.LoadBankAccount`, `aCustPaymentBll.LoadCashAccount` | — | — | — |
| 27 | `ddlCompany_OnSelectedIndexChanged` | `void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)` | 35 | `aOtherStockActionBLL.LoadDcInformationByUser`, `aOtherStockActionBLL.LoadWhInformation` | — | — | `UserId`, `UserType` |
| 28 | `dcDropDownList_SelectedIndexChanged` | `void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 13 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
CustomerPayment.aspx (markup)
  -> SInventory_UI_CustomerPayment (code-behind event handler)
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
| `customerTextBox_AutoCompleteExtender` | `singleCustomerTextBox` | `GetCustomernew` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `DropDownList` | `CustPaymentBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `DropDownList` | `InvoiceBatchBLL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| `DropDownList` | `OrderInfoBLL` | `LoadManufac` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `customerTextBox_TextChanged` | `CustPaymentBLL` | `LoadInvoiceNew` | `string batchId, string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `CustomerId` | `CustPaymentBLL` | `CustomerLoad` | `string aCustomerMaster` | `CustomerMaster` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `Validation` | `InvoiceBLL` | `CheckChartOfAccount` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Validation` | `InvoiceBLL` | `GetFinancialYearInfo` | `string invoiceDate,string unitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `CustPaymentBLL` | `SaveCustPayment` | `CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDet` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `saveButton_Click` | `CustPaymentBLL` | `UpdateInvoicePaymentAmount` | `string amount, string status, string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `GetCompanyIdByInvoice` | `string invoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `SaveCreditAdjustmentInfo` | `CreditAdjustmentDao adjustmentDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `batchDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadCustomer` | `DropDownList ddl, string batchId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `Button1_Click1` | `CustPaymentBLL` | `LoadInvoiceNew` | `string batchId, string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `Button1_Click2` | `CustPaymentBLL` | `LoadInvoiceByCustomer` | `string custmerId, string dcId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `Button1_Click2` | `CustPaymentBLL` | `LoadInvoiceByCustomerByUser` | `string custmerId, string userId, string dc` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `Button1_Click2` | `CustPaymentBLL` | `LoadInvoiceNew` | `string batchId, string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `Button1_Click2` | `CustPaymentBLL` | `LoadInvoiceNewByUser` | `string batchId, string customerId, string userId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `FindBatchInfo` | `OrderInfoBLL` | `BatchNoLoad` | `DropDownList aDownList, string companyId, string date, strin` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `FindBatchInfo` | `OrderInfoBLL` | `BatchNoLoadByUser` | `DropDownList aDownList, string date, string delmanId, string` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `singleCustomerTextBox_OnTextChanged` | `CustPaymentBLL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `SetBalanceInfo` | `CustPaymentBLL` | `GetCreditBalanceInfo` | `string value` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `payTypeDDL_OnSelectedIndexChanged` | `CustPaymentBLL` | `LoadBankAccount` | `DropDownList ddlBank, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `payTypeDDL_OnSelectedIndexChanged` | `CustPaymentBLL` | `LoadCashAccount` | `DropDownList ddlCash, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `ddlCompany_OnSelectedIndexChanged` | `OtherStockActionBLL` | `LoadDcInformationByUser` | `DropDownList ddl, string companyId, string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `ddlCompany_OnSelectedIndexChanged` | `OtherStockActionBLL` | `LoadWhInformation` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_CustomerPaymentPosting` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_Sales_CustomerPaymentPosting` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_Save_SaveCustPayment` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_UD_InvoicePayment` | saveButton_Click → `CustPaymentBLL.UpdateInvoicePaymentAmount → CustPaymentDAL.UpdateInvoicePaymentAmount → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `DropDownList()` → `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyUnit` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Validation()` → `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCreditAdjustment` | `SetBalanceInfo()` → `CustPaymentBLL.GetCreditBalanceInfo` → `CustPaymentDAL.GetCreditBalanceInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `InvoiceBLL.SaveCreditAdjustmentInfo` → `InvoiceDAL.SaveCreditAdjustmentInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` |
| `tblCustMaster` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `CustomerId()` → `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader` |
| `tblCustPayDetail` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerPay` | `saveButton_Click()` → `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo` |
| `tblDeliveryManInfo` | `DropDownList()` → `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblEmpGeneralInfo` | `DropDownList()` → `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblInvoice` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblInvoiceBatch` | `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblInvoiceDetail` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentTranscationDetail` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentTranscationMaster` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentType` | `Button1_Click1()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomerByUser` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceByCustomer` → `CustPaymentDAL.GetInvoiceByCustomer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `DropDownList()` → `CustPaymentBLL.PaymentTypeLoadBLL` → `CustPaymentDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblUserCompanyUnit` | `Button1_Click2()` → `CustPaymentBLL.LoadInvoiceNewByUser` → `CustPaymentDAL.LoadInvoiceNewByUser` → `ClsCommonInternalDAL.DataContainerDataTable`, `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `ddlCompany_OnSelectedIndexChanged()` → `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `SetBalanceInfo` | 0.00 | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtenderunitValue` | `payAmountTextBox` | Custom | 0123456789. |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-FIN-010-001` | {0} | Code | `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-FIN-010-002` | 0.00 | Code | `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs` → `SetBalanceInfo()` | user action on this page | operation is refused | "0.00" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `UserId`, `UserType`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCreditAdjustment` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerPay` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceBatch` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblPaymentTranscationDetail` |
| Database object | `tblPaymentTranscationMaster` |
| Database object | `tblPaymentType` |
| Database object | `tblUserCompanyUnit` |
| Stored procedure | `sp_CustomerPaymentPosting` |
| Stored procedure | `sp_Sales_CustomerPaymentPosting` |
| Stored procedure | `sp_Save_SaveCustPayment` |
| Stored procedure | `sp_UD_InvoicePayment` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-FIN-010-001` | Unauthenticated access | Request `/SInventory_UI/CustomerPayment.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-FIN-010-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-FIN-010-003` | Validation: 0.00 | Trigger `SetBalanceInfo()` in the state that produces this message | alert "0.00" and the operation is not performed | CONFIRMED |
| `TC-FIN-010-004` | Grid loads | Open the page with data present | Grid `orderGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerPayment.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 43 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 28 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 4 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 15 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` is **shared with 43 other page(s)** (e.g. `ProformaHTMLReport.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseDelReturnViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` is **shared with 12 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerPaymentBatchReport.aspx`, `CustomerPaymentProcess.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` is **shared with 22 other page(s)** (e.g. `AllSalesReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`, `BatchWiseTopSheet.aspx`, `CustomerPaymentProcess.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCreditAdjustment` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerPay` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceBatch` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblPaymentTranscationDetail` — any schema change to it breaks this page.
- Writes/reads `tblPaymentTranscationMaster` — any schema change to it breaks this page.
- Writes/reads `tblPaymentType` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
