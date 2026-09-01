# FIN-013 — CustomerPaymentProcess

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `FIN-013` |
| Module | `finance-collection` |
| File name | `CustomerPaymentProcess.aspx` |
| Route | `/SInventory_UI/CustomerPaymentProcess.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `CustomerPaymentProcess.aspx.cs` |
| Code-behind class | `SInventory_UI_CustomerPaymentProcess` |
| Markup lines | 547 |
| Code-behind lines | 731 |
| Server controls | 48 |
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

- referenced from `SInventory_UI/CustomerPaymentList.aspx`

Outbound `Response.Redirect` targets: `CustomerPaymentList.aspx`, `Login.aspx`

## 5. Route

`/SInventory_UI/CustomerPaymentProcess.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderStatusDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |

## 7. UI Control Inventory

48 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 58 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 61 |
| 4 | `paymentBatchId` | asp:HiddenField |  |  |  |  |  |  | 89 |
| 5 | `manufacDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | ddlCompany_OnSelectedIndexChanged | 106 |
| 6 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  | True |  |  | dcDropDownList_SelectedIndexChanged | 114 |
| 7 | `customerCheckBox` | asp:CheckBox |  |  | True |  |  | customerCheckBox_OnCheckedChanged | 127 |
| 8 | `refNameTextBox` | asp:TextBox | Reference No: |  |  |  |  |  | 140 |
| 9 | `totalBalance` | asp:Label | Customer Name: |  |  |  | False |  | 146 |
| 10 | `singleCustomerTextBox` | asp:TextBox | Customer Name: |  | True | Enabled=False |  | singleCustomerTextBox_OnTextChanged | 147 |
| 11 | `customerTextBox_AutoCompleteExtender` | ajaxToolkit:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 148 |
| 12 | `hdCustomerId` | asp:HiddenField |  |  |  |  |  |  | 155 |
| 13 | `delDropDownList` | asp:DropDownList | Delivery Man: |  | True |  |  | delDropDownList_OnTextChanged | 169 |
| 14 | `dateTextBox` | asp:TextBox | Batch Creation Date: |  | True |  |  | dateTextBox_TextChanged | 177 |
| 15 | `imgDate` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 179 |
| 16 | `Date` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 181 |
| 17 | `batchDropDownList` | asp:DropDownList | Batch No: |  | True |  |  | batchDropDownList_SelectedIndexChanged | 189 |
| 18 | `customerDropDownList` | asp:DropDownList |  |  | True |  |  | customerDropDownList_SelectedIndexChanged | 200 |
| 19 | `paymentDtTextBox` | asp:Label | Payment Date: |  | True | ReadOnly=True |  |  | 225 |
| 20 | `paymentAmountTextBox` | asp:Label | Payment Amount: |  |  | ReadOnly=True |  |  | 232 |
| 21 | `payTypeDDL` | asp:DropDownList | Payment Type: |  | True |  |  | payTypeDDL_OnSelectedIndexChanged | 245 |
| 22 | `ddlBank` | asp:DropDownList | Bank Account: |  | True |  |  |  | 253 |
| 23 | `ddlCash` | asp:DropDownList | Cash Account: |  | True |  |  |  | 260 |
| 24 | `customerTextBox` | asp:TextBox | Customer |  | True |  |  | customerTextBox_TextChanged | 275 |
| 25 | `Button1` | asp:Button |  | Search |  |  |  | Button1_Click2 | 297 |
| 26 | `saveBatchButton` | asp:Button |  | Generate Batch |  |  |  | saveBatchButton_Click | 299 |
| 27 | `Button2` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 302 |
| 28 | `HiddenField1` | asp:HiddenField |  |  |  |  |  |  | 320 |
| 29 | `loadGridView` | asp:GridView | nbsp; |  |  |  |  |  | 334 |
| 30 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 338 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 339 |
| 32 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 340 |
| 33 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 341 |
| 34 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 342 |
| 35 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 343 |
| 36 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 344 |
| 37 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 345 |
| 38 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 346 |
| 39 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 348 |
| 40 | `chkSelectAll` | asp:CheckBox | loadGridView → col[9] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 350 |
| 41 | `chkSelect` | asp:CheckBox | loadGridView → col[9] "(no header)" |  | True |  |  | chkSelect_CheckedChanged | 353 |
| 42 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 363 |
| 43 | `invoiceButton` | asp:LinkButton | loadGridView → col[10] "Action" | Reject &gt; |  |  |  | invoiceButton_Click | 365 |
| 44 | `totalReceiveableLabel` | asp:Label | Total Amount: |  |  |  |  |  | 396 |
| 45 | `submitButton` | asp:Button |  | Confirm |  |  |  | saveButton_Click | 425 |
| 46 | `clearButton` | asp:Button |  | Reload |  |  |  | clearButton_OnClick | 429 |
| 47 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 536 |
| 48 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 540 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `CustPayDetailId,CustomerMasterId,CustPayId,IsBatchCreated` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Unit Name | ComUnitName | — | — | — |
| 1 | BoundField | Customer Name | CustomerName | — | — | — |
| 2 | BoundField | InvoiceNo | InvoiceNo | — | — | — |
| 3 | BoundField | Delivery Man | DeliveryMan | — | — | — |
| 4 | BoundField | Invoice Batch | InvoiceBatch | — | — | — |
| 5 | BoundField | Payment Date | PaymentDate | — | — | — |
| 6 | BoundField | Payment Amount | PaymentAmount | — | — | — |
| 7 | BoundField | AIT | AIT | — | — | — |
| 8 | BoundField | Discount | Discount | — | — | — |
| 9 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |
| 10 | TemplateField | Action | — | — | `LinkButton:invoiceButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CustPayment`, `CustomerMaster`, `PaymentBachMasterDao`, `PaymentBatchDetailDao`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDownList` | `aCustPaymentBll.PaymentTypeLoadBLL(...)` | `CustPaymentBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.PaymentTypeLoadBLL` → `CustPaymentDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `LoadDropDownList` | `aInvoiceBatchBll.LoadDeliveryMan(...)` | `InvoiceBatchBLL.LoadDeliveryMan(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` | `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `LoadDropDownList` | `aOrderInfoBll.LoadManufac(...)` | `OrderInfoBLL.LoadManufac(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyInfo` | — |
| `customerTextBox_TextChanged` | `aCustPaymentBll.LoadInvoiceNew(...)` | `CustPaymentBLL.LoadInvoiceNew(string batchId, string customerId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType` | — |
| `singleCustomerTextBox_OnTextChanged` | `aCustPaymentBll.GetCustomerInfoByCode(...)` | `CustPaymentBLL.GetCustomerInfoByCode(string customerCode)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.GetCustomerInfoByCode` | — | — | — |
| `SetBalanceInfo` | `aCustPaymentBll.GetCreditBalanceInfo(...)` | `CustPaymentBLL.GetCreditBalanceInfo(string value)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.GetCreditBalanceInfo` → `CustPaymentDAL.GetCreditBalanceInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCreditAdjustment` | — |
| `payTypeDDL_OnSelectedIndexChanged` | `aCustPaymentBll.LoadBankAccount(...)` | `CustPaymentBLL.LoadBankAccount(DropDownList ddlBank, string companyId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadBankAccount` → `CustPaymentDAL.LoadBankAccount` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | — | — |
| `payTypeDDL_OnSelectedIndexChanged` | `aCustPaymentBll.LoadCashAccount(...)` | `CustPaymentBLL.LoadCashAccount(DropDownList ddlCash, string companyId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadCashAccount` → `CustPaymentDAL.LoadCashAccount` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | — | — |
| `ddlCompany_OnSelectedIndexChanged` | `aOtherStockActionBLL.LoadDcInformationByUser(...)` | `OtherStockActionBLL.LoadDcInformationByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `ddlCompany_OnSelectedIndexChanged` | `aOtherStockActionBLL.LoadWhInformation(...)` | `OtherStockActionBLL.LoadWhInformation(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |
| `CustomerId` | `aCustPaymentBll.CustomerLoad(...)` | `CustPaymentBLL.CustomerLoad(string aCustomerMaster)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblCustMaster` | — |
| `batchDropDownList_SelectedIndexChanged` | `aCustPaymentBll.LoadCustomer(...)` | `CustPaymentBLL.LoadCustomer(DropDownList ddl, string batchId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadCustomer` | — | — | — |
| `FindBatchInfo` | `aOrderInfoBll.BatchNoLoad(...)` | `OrderInfoBLL.BatchNoLoad(DropDownList aDownList, string companyId, string date, string delmanId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblInvoiceBatch` | — |
| `FindBatchInfo` | `aOrderInfoBll.BatchNoLoadByUser(...)` | `OrderInfoBLL.BatchNoLoadByUser(DropDownList aDownList, string date, string delmanId, string userId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblInvoice`, `tblInvoiceBatch`, `tblUserCompanyUnit` | — |
| `LoadPaymentData` | `aTranscationDal.LoadPaymentData(...)` | `PaymentTranscationDal.LoadPaymentData(string userID,string pram)` <br/>`Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblUserCompanyUnit` | — |
| `saveButton_Click` | `aCustPaymentBll.SaveCustPayment(...)` | `CustPaymentBLL.SaveCustPayment(CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDetailList)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustPaymentDAL.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.PaymentJournalPosting` | DELETE, INSERT, SELECT | `tblCustPayDetail`, `tblCustomerPay` | `sp_CustomerPaymentPosting`, `sp_Sales_CustomerPaymentPosting`, `sp_Save_SaveCustPayment` |
| `saveButton_Click` | `aCustPaymentBll.UpdateInvoicePaymentAmount(...)` | `CustPaymentBLL.UpdateInvoicePaymentAmount(string amount, string status, string id)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.UpdateInvoicePaymentAmount` → `CustPaymentDAL.UpdateInvoicePaymentAmount` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoice` | `sp_UD_InvoicePayment` |
| `saveButton_Click` | `aCustPaymentDal.SaveCode(...)` | `CustPaymentDAL.SaveCode()` <br/>`Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | `CustPaymentDAL.SaveCode` → `ClsCommonInternalDAL.SaveAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.SaveAction` → `DB_Manager.ExecuteNonQueryAction` → `DB_Manager.CloseConnection` → `DB_Manager.CreateConnection` → `DB_Connection.GenerateString` | — | — | `sp_Save_PayCode`, `sp_Save_SaveCustPayment` |
| `saveButton_Click` | `aPaymentDal.GetInvoiceExistOrNot(...)` | `CustPaymentDAL.GetInvoiceExistOrNot(int invoiceId)` <br/>`Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | `CustPaymentDAL.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderStatusDal.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | — |
| `saveButton_Click` | `aTranscationDal.GetPaymentProcess(...)` | `PaymentTranscationDal.GetPaymentProcess(string payId)` <br/>`Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | `PaymentTranscationDal.GetPaymentProcess` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster` | — |
| `saveButton_Click` | `aTranscationDal.IsExist(...)` | `PaymentTranscationDal.IsExist(string custPayId)` <br/>`Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | `PaymentTranscationDal.IsExist` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustPayDetail` | — |
| `saveButton_Click` | `aTranscationDal.UpdatePaymentTranscation(...)` | `PaymentTranscationDal.UpdatePaymentTranscation(string custPayId)` <br/>`Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | `PaymentTranscationDal.UpdatePaymentTranscation` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblPaymentTranscationMaster` | — |
| `saveButton_Click` | `aTranscationDal.UpdatePaymentTranscationStatus(...)` | `PaymentTranscationDal.UpdatePaymentTranscationStatus(string payId)` <br/>`Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | `PaymentTranscationDal.UpdatePaymentTranscationStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblPaymentTranscationMaster` | — |
| `UpdatePaymentTranscation` | `aTranscationDal.UpdatePaymentTranscation(...)` | `PaymentTranscationDal.UpdatePaymentTranscation(string custPayId)` <br/>`Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | `PaymentTranscationDal.UpdatePaymentTranscation` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblPaymentTranscationMaster` | — |
| `saveBatchButton_Click` | `aTranscationDal.CheckExistOrNot(...)` | `CreditLimitDAL.CheckExistorNot(int companyId, int customerid)` <br/>`Library.DAL/SInventory_DAL/CreditLimitDAL.cs` | `CreditLimitDAL.CheckExistorNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `PaymentTranscationDal.CheckExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCustMaster`, `tblCustomerCreditLimit` | — |
| `saveBatchButton_Click` | `aTranscationDal.GenerateBatch(...)` | `PaymentTranscationDal.GenerateBatch(PaymentBachMasterDao master, List<PaymentBatchDetailDao> detailsList)` <br/>`Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | `PaymentTranscationDal.GenerateBatch` → `ClsCommonInternalDAL.SaveAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.SaveAction` → `DB_Manager.ExecuteNonQueryAction` → `PaymentTranscationDal.CloseAllConnection` → `DB_Manager.CloseConnection` → `PaymentTranscationDal.CreateConnection` | — | — | `sp_Save_PaymentBatchDetail`, `sp_Save_PaymentBatchMaster` |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `manufacDropDownList` | Company Name: | asp:DropDownList | True | ddlCompany_OnSelectedIndexChanged | `aCustPaymentBll.LoadBankAccount()` in `payTypeDDL_OnSelectedIndexChanged()`; `aCustPaymentBll.LoadCashAccount()` in `payTypeDDL_OnSelectedIndexChanged()`; `aOrderInfoBll.BatchNoLoad()` in `FindBatchInfo()`; `aOrderInfoBll.LoadManufac()` in `LoadDropDownList()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoiceBatch`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center: | asp:DropDownList | True | dcDropDownList_SelectedIndexChanged | `aOtherStockActionBLL.LoadDcInformationByUser()` in `ddlCompany_OnSelectedIndexChanged()`; `aOtherStockActionBLL.LoadWhInformation()` in `ddlCompany_OnSelectedIndexChanged()` | `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `delDropDownList` | Delivery Man: | asp:DropDownList | True | delDropDownList_OnTextChanged | `aInvoiceBatchBll.LoadDeliveryMan()` in `LoadDropDownList()`; `aOrderInfoBll.BatchNoLoad()` in `FindBatchInfo()`; `aOrderInfoBll.BatchNoLoadByUser()` in `FindBatchInfo()` | `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `batchDropDownList` | Batch No: | asp:DropDownList | True | batchDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadCustomer()` in `batchDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadInvoiceNew()` in `customerTextBox_TextChanged()`; `aOrderInfoBll.BatchNoLoad()` in `FindBatchInfo()`; `aOrderInfoBll.BatchNoLoadByUser()` in `FindBatchInfo()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblInvoice`, `tblInvoiceBatch`, `tblInvoiceDetail`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblPaymentType`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `customerDropDownList` |  | asp:DropDownList | True | customerDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadCustomer()` in `batchDropDownList_SelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `payTypeDDL` | Payment Type: | asp:DropDownList | True | payTypeDDL_OnSelectedIndexChanged | `aCustPaymentBll.PaymentTypeLoadBLL()` in `LoadDropDownList()` | `tblPaymentType` | set in code (see the DAL method above) |
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
| `Button1` | asp:Button | Search | — | `Button1_Click2` | 3 lines | — | — |
| `saveBatchButton` | asp:Button | Generate Batch | return confirm('Are you sure you want to Save ?'); | `saveBatchButton_Click` | 46 lines | `aTranscationDal.CheckExistOrNot`, `aTranscationDal.GenerateBatch` | — |
| `Button2` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |
| `invoiceButton` | asp:LinkButton | Reject &gt; | return confirm('Are you sure you want to Reject ?') | `invoiceButton_Click` | 11 lines | — | — |
| `submitButton` | asp:Button | Confirm | return confirm('Are you sure you want to Save ?') | `saveButton_Click` | 132 lines | `aCustPaymentBll.SaveCustPayment`, `aCustPaymentBll.UpdateInvoicePaymentAmount`, `aCustPaymentDal.SaveCode`, `aPaymentDal.GetInvoiceExistOrNot`, `aTranscationDal.GetPaymentProcess` | — |
| `clearButton` | asp:Button | Reload | — | `clearButton_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:AutoCompleteExtender`(singleCustomerTextBox), `ajaxToolkit:CalendarExtender`(dateTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `LoadDropDownList` | `void LoadDropDownList()` | 9 | `aCustPaymentBll.PaymentTypeLoadBLL`, `aInvoiceBatchBll.LoadDeliveryMan`, `aOrderInfoBll.LoadManufac` | — | — | — |
| 3 | `CustMasterNewImageButton_Click` | `void CustMasterNewImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 4 | `customerTextBox_TextChanged` | `void customerTextBox_TextChanged(object sender, EventArgs e)` | 18 | `aCustPaymentBll.LoadInvoiceNew` | — | — | — |
| 5 | `singleCustomerTextBox_OnTextChanged` | `void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)` | 39 | `aCustPaymentBll.GetCustomerInfoByCode` | — | — | — |
| 6 | `SetBalanceInfo` | `void SetBalanceInfo()` | 17 | `aCustPaymentBll.GetCreditBalanceInfo` | — | — | — |
| 7 | `customerCheckBox_OnCheckedChanged` | `void customerCheckBox_OnCheckedChanged(object sender, EventArgs e)` | 18 | — | — | — | — |
| 8 | `payTypeDDL_OnSelectedIndexChanged` | `void payTypeDDL_OnSelectedIndexChanged(object sender, EventArgs e)` | 48 | `aCustPaymentBll.LoadBankAccount`, `aCustPaymentBll.LoadCashAccount` | — | — | — |
| 9 | `ddlCompany_OnSelectedIndexChanged` | `void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)` | 35 | `aOtherStockActionBLL.LoadDcInformationByUser`, `aOtherStockActionBLL.LoadWhInformation` | — | — | `UserId`, `UserType` |
| 10 | `dcDropDownList_SelectedIndexChanged` | `void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 13 | — | — | — | — |
| 11 | `CustomerId` | `int CustomerId(out CustomerMaster aCustomerMaster)` | 7 | `aCustPaymentBll.CustomerLoad` | — | — | — |
| 12 | `delDropDownList_OnTextChanged` | `void delDropDownList_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 13 | `customerDropDownList_SelectedIndexChanged` | `void customerDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 14 | `batchDropDownList_SelectedIndexChanged` | `void batchDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 11 | `aCustPaymentBll.LoadCustomer` | — | — | — |
| 15 | `dateTextBox_TextChanged` | `void dateTextBox_TextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 16 | `FindBatchInfo` | `void FindBatchInfo()` | 40 | `aOrderInfoBll.BatchNoLoad`, `aOrderInfoBll.BatchNoLoadByUser` | — | — | `UserId`, `UserType` |
| 17 | `LoadPaymentData` | `void LoadPaymentData()` | 39 | `aTranscationDal.LoadPaymentData` | — | — | `UserId` |
| 18 | `GenerateParameter` | `string GenerateParameter()` | 22 | — | — | — | — |
| 19 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 20 | — | — | — | — |
| 20 | `CalculateTotalReceiveable` | `void CalculateTotalReceiveable()` | 19 | — | — | — | — |
| 21 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 132 | `aCustPaymentBll.SaveCustPayment`, `aCustPaymentBll.UpdateInvoicePaymentAmount`, `aCustPaymentDal.SaveCode`, `aPaymentDal.GetInvoiceExistOrNot`, `aTranscationDal.GetPaymentProcess`, `aTranscationDal.IsExist` | — | — | `LoginName` |
| 22 | `Validation` | `bool Validation()` | 28 | — | — | — | — |
| 23 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 24 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 25 | `invoiceButton_Click` | `void invoiceButton_Click(object sender, EventArgs e)` | 11 | — | — | — | — |
| 26 | `UpdatePaymentTranscation` | `void UpdatePaymentTranscation(string custPayId, int rowindex)` | 13 | `aTranscationDal.UpdatePaymentTranscation` | — | — | — |
| 27 | `chkSelect_CheckedChanged` | `void chkSelect_CheckedChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 28 | `Button1_Click2` | `void Button1_Click2(object sender, EventArgs e)` | 3 | — | — | — | — |
| 29 | `CheckBatchUpdate` | `void CheckBatchUpdate()` | 34 | — | — | — | — |
| 30 | `saveBatchButton_Click` | `void saveBatchButton_Click(object sender, EventArgs e)` | 46 | `aTranscationDal.CheckExistOrNot`, `aTranscationDal.GenerateBatch` | — | — | `UserId` |

Call chain shape for every persisting action on this page:

```
CustomerPaymentProcess.aspx (markup)
  -> SInventory_UI_CustomerPaymentProcess (code-behind event handler)
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
| `LoadDropDownList` | `CustPaymentBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `LoadDropDownList` | `InvoiceBatchBLL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| `LoadDropDownList` | `OrderInfoBLL` | `LoadManufac` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `customerTextBox_TextChanged` | `CustPaymentBLL` | `LoadInvoiceNew` | `string batchId, string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `singleCustomerTextBox_OnTextChanged` | `CustPaymentBLL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `SetBalanceInfo` | `CustPaymentBLL` | `GetCreditBalanceInfo` | `string value` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `payTypeDDL_OnSelectedIndexChanged` | `CustPaymentBLL` | `LoadBankAccount` | `DropDownList ddlBank, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `payTypeDDL_OnSelectedIndexChanged` | `CustPaymentBLL` | `LoadCashAccount` | `DropDownList ddlCash, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `ddlCompany_OnSelectedIndexChanged` | `OtherStockActionBLL` | `LoadDcInformationByUser` | `DropDownList ddl, string companyId, string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `ddlCompany_OnSelectedIndexChanged` | `OtherStockActionBLL` | `LoadWhInformation` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `CustomerId` | `CustPaymentBLL` | `CustomerLoad` | `string aCustomerMaster` | `CustomerMaster` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `batchDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadCustomer` | `DropDownList ddl, string batchId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `FindBatchInfo` | `OrderInfoBLL` | `BatchNoLoad` | `DropDownList aDownList, string companyId, string date, strin` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `FindBatchInfo` | `OrderInfoBLL` | `BatchNoLoadByUser` | `DropDownList aDownList, string date, string delmanId, string` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `saveButton_Click` | `CustPaymentBLL` | `SaveCustPayment` | `CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDet` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `saveButton_Click` | `CustPaymentBLL` | `UpdateInvoicePaymentAmount` | `string amount, string status, string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadPaymentData` | `PaymentTranscationDal` | `LoadPaymentData` | `string userID,string pram` | `DataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch`, `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| `saveButton_Click` | `CustPaymentDAL` | `SaveCode` | `` | `int` | — | — | `sp_Save_PayCode`, `sp_Save_SaveCustPayment` | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `saveButton_Click` | `CustPaymentDAL` | `GetInvoiceExistOrNot` | `int invoiceId` | `DataTable` | SELECT | `tblCustPayDetail`, `tblCustomerPay` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `saveButton_Click` | `OrderStatusDal` | `GetInvoiceExistOrNot` | `string orderNo` | `DataTable` | SELECT | `tblInvoice` | — | `Library.DAL/SInventory_DAL/OrderStatusDal.cs` |
| `saveButton_Click` | `PaymentTranscationDal` | `GetPaymentProcess` | `string payId` | `DataTable` | SELECT | `tblPaymentTranscationDetail`, `tblPaymentTranscationMaster` | — | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| `saveButton_Click` | `PaymentTranscationDal` | `IsExist` | `string custPayId` | `DataTable` | SELECT | `tblCustPayDetail` | — | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| `saveButton_Click` | `PaymentTranscationDal` | `UpdatePaymentTranscation` | `string custPayId` | `bool` | UPDATE | `tblPaymentTranscationMaster` | — | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| `saveButton_Click` | `PaymentTranscationDal` | `UpdatePaymentTranscationStatus` | `string payId` | `bool` | UPDATE | `tblPaymentTranscationMaster` | — | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| `UpdatePaymentTranscation` | `PaymentTranscationDal` | `UpdatePaymentTranscation` | `string custPayId` | `bool` | UPDATE | `tblPaymentTranscationMaster` | — | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| `saveBatchButton_Click` | `CreditLimitDAL` | `CheckExistorNot` | `int companyId, int customerid` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCustMaster`, `tblCustomerCreditLimit` | — | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| `saveBatchButton_Click` | `PaymentTranscationDal` | `CheckExistOrNot` | `string custPayDetailId` | `DataTable` | SELECT | — | — | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| `saveBatchButton_Click` | `PaymentTranscationDal` | `GenerateBatch` | `PaymentBachMasterDao master, List<PaymentBatchDetailDao> det` | `Int32` | — | — | `sp_Save_PaymentBatchDetail`, `sp_Save_PaymentBatchMaster` | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_CustomerPaymentPosting` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_Sales_CustomerPaymentPosting` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_Save_SaveCustPayment` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_UD_InvoicePayment` | saveButton_Click → `CustPaymentBLL.UpdateInvoicePaymentAmount → CustPaymentDAL.UpdateInvoicePaymentAmount → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Save_PayCode` | saveButton_Click → `CustPaymentDAL.SaveCode → ClsCommonInternalDAL.SaveAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.SaveAction → DB_Manager.ExecuteNonQueryAction → DB_Manager.CloseConnection` | **No** — see note |
| `sp_Save_PaymentBatchDetail` | saveBatchButton_Click → `PaymentTranscationDal.GenerateBatch → ClsCommonInternalDAL.SaveAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.SaveAction → DB_Manager.ExecuteNonQueryAction → PaymentTranscationDal.CloseAllConnection` | **No** — see note |
| `sp_Save_PaymentBatchMaster` | saveBatchButton_Click → `PaymentTranscationDal.GenerateBatch → ClsCommonInternalDAL.SaveAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.SaveAction → DB_Manager.ExecuteNonQueryAction → PaymentTranscationDal.CloseAllConnection` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadDropDownList()` → `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveBatchButton_Click()` → `CreditLimitDAL.CheckExistorNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `PaymentTranscationDal.CheckExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `ddlCompany_OnSelectedIndexChanged()` → `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `ddlCompany_OnSelectedIndexChanged()` → `OtherStockActionBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblCreditAdjustment` | `SetBalanceInfo()` → `CustPaymentBLL.GetCreditBalanceInfo` → `CustPaymentDAL.GetCreditBalanceInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `CustomerId()` → `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveBatchButton_Click()` → `CreditLimitDAL.CheckExistorNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `PaymentTranscationDal.CheckExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustPayDetail` | `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo`, `saveButton_Click()` → `CustPaymentDAL.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderStatusDal.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `PaymentTranscationDal.IsExist` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerCreditLimit` | `saveBatchButton_Click()` → `CreditLimitDAL.CheckExistorNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `PaymentTranscationDal.CheckExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerPay` | `saveButton_Click()` → `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo`, `saveButton_Click()` → `CustPaymentDAL.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderStatusDal.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `LoadDropDownList()` → `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue`, `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `LoadDropDownList()` → `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue`, `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `CustPaymentBLL.UpdateInvoicePaymentAmount` → `CustPaymentDAL.UpdateInvoicePaymentAmount` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction`, `saveButton_Click()` → `CustPaymentDAL.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderStatusDal.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceBatch` | `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue`, `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentTranscationDetail` | `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `PaymentTranscationDal.GetPaymentProcess` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentTranscationMaster` | `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable`, `UpdatePaymentTranscation()` → `PaymentTranscationDal.UpdatePaymentTranscation` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `PaymentTranscationDal.GetPaymentProcess` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `PaymentTranscationDal.UpdatePaymentTranscationStatus` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `saveButton_Click()` → `PaymentTranscationDal.UpdatePaymentTranscation` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblPaymentType` | `LoadDropDownList()` → `CustPaymentBLL.PaymentTypeLoadBLL` → `CustPaymentDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue`, `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUserCompanyUnit` | `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `LoadPaymentData()` → `PaymentTranscationDal.LoadPaymentData` → `ClsCommonInternalDAL.DataContainerDataTable`, `ddlCompany_OnSelectedIndexChanged()` → `OtherStockActionBLL.LoadDcInformationByUser` → `OtherStockActionDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `SetBalanceInfo` | 0.00 | No | Yes | UNKNOWN | Notification |
| `LoadPaymentData` | TK. | No | Yes | UNKNOWN | Notification |
| `CalculateTotalReceiveable` | TK. | No | Yes | UNKNOWN | Notification |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-FIN-013-001` | 0.00 | Code | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs` → `SetBalanceInfo()` | user action on this page | operation is refused | "0.00" | CONFIRMED |
| `BR-FIN-013-002` | TK. | Code | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs` → `LoadPaymentData()` | user action on this page | operation is refused | "TK." | CONFIRMED |
| `BR-FIN-013-003` | TK. | Code | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs` → `CalculateTotalReceiveable()` | user action on this page | operation is refused | "TK." | CONFIRMED |
| `BR-FIN-013-004` | {0} | Code | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderStatusDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCreditAdjustment` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerCreditLimit` |
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
| Stored procedure | `sp_Save_PayCode` |
| Stored procedure | `sp_Save_PaymentBatchDetail` |
| Stored procedure | `sp_Save_PaymentBatchMaster` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-FIN-013-001` | Unauthenticated access | Request `/SInventory_UI/CustomerPaymentProcess.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-FIN-013-002` | Validation: 0.00 | Trigger `SetBalanceInfo()` in the state that produces this message | alert "0.00" and the operation is not performed | CONFIRMED |
| `TC-FIN-013-003` | Validation: TK. | Trigger `LoadPaymentData()` in the state that produces this message | alert "TK." and the operation is not performed | CONFIRMED |
| `TC-FIN-013-004` | Validation: TK. | Trigger `CalculateTotalReceiveable()` in the state that produces this message | alert "TK." and the operation is not performed | CONFIRMED |
| `TC-FIN-013-005` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-FIN-013-006` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderStatusDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 48 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 30 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 7 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 16 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` is **shared with 12 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerPayment.aspx`, `CustomerPaymentBatchReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` is **shared with 22 other page(s)** (e.g. `AllSalesReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`, `BatchWiseTopSheet.aspx`, `CustomerPayment.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` is **shared with 6 other page(s)** (e.g. `CustomerCreditLimitApproval.aspx`, `CustomerCreditLimitEntry.aspx`, `CustomerCreditView.aspx`, `PettyCashApproval.aspx`, `ProductQuotedPriceView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` is **shared with 6 other page(s)** (e.g. `PaymentReportViewer.aspx`, `CustomerPaymentBatchReport.aspx`, `MoneyReceiptReport.aspx`, `PaymentTranscation.aspx`, `PaymentTranscationNew.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCreditAdjustment` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerCreditLimit` — any schema change to it breaks this page.
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
