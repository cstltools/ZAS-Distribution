# FIN-002 — BatchWiseCollection

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `FIN-002` |
| Module | `finance-collection` |
| File name | `BatchWiseCollection.aspx` |
| Route | `/SInventory_UI/BatchWiseCollection.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `BatchWiseCollection.aspx.cs` |
| Code-behind class | `SInventory_UI_BatchWiseCollection` |
| Markup lines | 337 |
| Code-behind lines | 201 |
| Server controls | 27 |
| GridViews | 1 |
| Code-behind methods | 13 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/BatchWiseCollection.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/BatchWiseCollection.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/BatchWiseCollection.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |

## 7. UI Control Inventory

27 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `delDropDownList` | asp:DropDownList | Delivery Man: |  | True |  |  | delDropDownList_OnTextChanged | 38 |
| 5 | `dateTextBox` | asp:TextBox | Batch Creation Date: |  | True |  |  | dateTextBox_TextChanged | 46 |
| 6 | `imgDate` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 48 |
| 7 | `Date` | asp:CalendarExtender |  |  |  |  |  |  | 50 |
| 8 | `batchDropDownList` | asp:DropDownList | Batch No: |  |  |  |  |  | 58 |
| 9 | `customerCheckBox` | asp:CheckBox |  |  | True |  |  | customerCheckBox_OnCheckedChanged | 64 |
| 10 | `singleCustomerTextBox` | asp:TextBox | Customer Code: |  | True | Enabled=False |  | singleCustomerTextBox_OnTextChanged | 74 |
| 11 | `customerTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 76 |
| 12 | `hdCustomerId` | asp:HiddenField |  |  |  |  |  |  | 83 |
| 13 | `nameTextBox` | asp:Label | Customer Name: |  | True | ReadOnly=True |  |  | 89 |
| 14 | `Button1` | asp:Button |  | View Report |  |  |  | submitButton_OnClick | 127 |
| 15 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 129 |
| 16 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 176 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 179 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 180 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 181 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 182 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 183 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 184 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 185 |
| 24 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 187 |
| 25 | `invoiceButton` | asp:LinkButton | loadGridView → col[7] "Action" | Report &gt; &gt; &gt; |  |  |  | invoiceButton_Click | 189 |
| 26 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 326 |
| 27 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 329 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `CustPayDetailId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Invoice Date | InvoiceDate | — | — | — |
| 1 | BoundField | Invoice No | InvoiceNo | — | — | — |
| 2 | BoundField | Customer Name | CustomerName | — | — | — |
| 3 | BoundField | Batch Creation Date | BatchCreationDate | — | — | — |
| 4 | BoundField | Batch | BatchNo | — | — | — |
| 5 | BoundField | Payment Date | PaymentDate | — | — | — |
| 6 | BoundField | Paid Amount | PaidAmount | — | — | — |
| 7 | TemplateField | Action | — | — | `LinkButton:invoiceButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `DropDownList` | `aReportDal.LoadDeliveryMan(...)` | `BatchWiseCollectionReportDal.LoadDeliveryMan(DropDownList aDropDownList)` <br/>`Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` | `BatchWiseCollectionReportDal.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `InvoiceDAL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `FindBatchInfo` | `aOrderInfoBll.BatchNoLoad(...)` | `OrderInfoBLL.BatchNoLoad(DropDownList aDownList, string companyId, string date, string delmanId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblInvoiceBatch` | — |
| `singleCustomerTextBox_OnTextChanged` | `aReportDal.GetCustomerInfoByCode(...)` | `BatchWiseCollectionReportDal.GetCustomerInfoByCode(string customerCode)` <br/>`Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` | `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `FixedDiscountEntryDal.GetCustomerInfoByCode` → `ManualOrderCreationDal.GetCustomerInfoByCode` | SELECT | `tblArea`, `tblCustMaster`, `tblInvoice`, `tblMIOInfo`, `tblMarket`, `tblRegion`, `tblTerritory` | — |
| `searchButton_Click` | `aReportDal.BatchWiseCollection(...)` | `BatchWiseCollectionDal.BatchWiseCollection(int batchId, DateTime batchCreationDate)` <br/>`Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs` | `BatchWiseCollectionDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` → `BatchWiseCollectionReportDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceBatch` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `delDropDownList` | Delivery Man: | asp:DropDownList | True | delDropDownList_OnTextChanged | `aOrderInfoBll.BatchNoLoad()` in `FindBatchInfo()`; `aReportDal.LoadDeliveryMan()` in `DropDownList()` | `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoiceBatch` | set in code (see the DAL method above) |
| `batchDropDownList` | Batch No: | asp:DropDownList | False | — | `aOrderInfoBll.BatchNoLoad()` in `FindBatchInfo()`; `aReportDal.BatchWiseCollection()` in `searchButton_Click()` | `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceBatch` | set in code (see the DAL method above) |

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
| `Button1` | asp:Button | View Report | — | `submitButton_OnClick` | 11 lines | — | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 11 lines | — | — |
| `invoiceButton` | asp:LinkButton | Report &gt; &gt; &gt; | — | `invoiceButton_Click` | 11 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | — | — | — | — |
| 2 | `DropDownList` | `void DropDownList()` | 3 | `aReportDal.LoadDeliveryMan` | — | — | — |
| 3 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 4 | `delDropDownList_OnTextChanged` | `void delDropDownList_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 5 | `FindBatchInfo` | `void FindBatchInfo()` | 28 | `aOrderInfoBll.BatchNoLoad` | — | — | — |
| 6 | `submitButton_OnClick` | `void submitButton_OnClick(object sender, EventArgs e)` | 11 | — | — | — | — |
| 7 | `PopupReport` | `void PopupReport(string batchId)` | 10 | — | — | — | `batchId` |
| 8 | `dateTextBox_TextChanged` | `void dateTextBox_TextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 9 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 11 | — | — | — | — |
| 10 | `singleCustomerTextBox_OnTextChanged` | `void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)` | 35 | `aReportDal.GetCustomerInfoByCode` | — | — | — |
| 11 | `customerCheckBox_OnCheckedChanged` | `void customerCheckBox_OnCheckedChanged(object sender, EventArgs e)` | 15 | — | — | — | — |
| 12 | `searchButton_Click` | `void searchButton_Click(object sender, EventArgs e)` | 18 | `aReportDal.BatchWiseCollection` | — | — | — |
| 13 | `invoiceButton_Click` | `void invoiceButton_Click(object sender, EventArgs e)` | 11 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
BatchWiseCollection.aspx (markup)
  -> SInventory_UI_BatchWiseCollection (code-behind event handler)
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
| `DropDownList` | `InvoiceBatchBLL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| `DropDownList` | `InvoiceBLL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DropDownList` | `OrderInfoBLL` | `LoadDeliveryMan` | `string deliveryId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `FindBatchInfo` | `OrderInfoBLL` | `BatchNoLoad` | `DropDownList aDownList, string companyId, string date, strin` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `singleCustomerTextBox_OnTextChanged` | `CustPaymentBLL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `singleCustomerTextBox_OnTextChanged` | `OrderListBLL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `DropDownList` | `BatchWiseCollectionReportDal` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| `DropDownList` | `InvoiceBatchDAL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — | `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs` |
| `DropDownList` | `InvoiceDAL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| `DropDownList` | `OrderInfoDAL` | `LoadDeliveryMan` | `string deliveryId` | `DataTable` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| `singleCustomerTextBox_OnTextChanged` | `BatchWiseCollectionReportDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| `singleCustomerTextBox_OnTextChanged` | `CustomerFixedDiscountDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `singleCustomerTextBox_OnTextChanged` | `CustPaymentDAL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `singleCustomerTextBox_OnTextChanged` | `FixedDiscountEntryDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `singleCustomerTextBox_OnTextChanged` | `ManualOrderCreationDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `singleCustomerTextBox_OnTextChanged` | `OrderListDAL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `singleCustomerTextBox_OnTextChanged` | `SampleIssueDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `singleCustomerTextBox_OnTextChanged` | `WebServiceDal` | `GetCustomerInfoByCode` | `string prefixText` | `DataTable` | SELECT | `tblArea`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |
| `searchButton_Click` | `BatchWiseCollectionDal` | `BatchWiseCollection` | `int batchId, DateTime batchCreationDate` | `DataTable` | SELECT | `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceBatch` | — | `Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs` |
| `searchButton_Click` | `BatchWiseCollectionReportDal` | `BatchWiseCollection` | `int batchId, DateTime batchCreationDate` | `DataTable` | SELECT | `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceBatch` | — | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `singleCustomerTextBox_OnTextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `searchButton_Click()` → `BatchWiseCollectionDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` → `BatchWiseCollectionReportDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable`, `singleCustomerTextBox_OnTextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustPayDetail` | `searchButton_Click()` → `BatchWiseCollectionDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` → `BatchWiseCollectionReportDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerPay` | `searchButton_Click()` → `BatchWiseCollectionDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` → `BatchWiseCollectionReportDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `DropDownList()` → `BatchWiseCollectionReportDal.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `InvoiceDAL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` |
| `tblEmpGeneralInfo` | `DropDownList()` → `BatchWiseCollectionReportDal.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `InvoiceDAL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` |
| `tblInvoice` | `searchButton_Click()` → `BatchWiseCollectionDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` → `BatchWiseCollectionReportDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable`, `singleCustomerTextBox_OnTextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceBatch` | `FindBatchInfo()` → `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue`, `searchButton_Click()` → `BatchWiseCollectionDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` → `BatchWiseCollectionReportDal.BatchWiseCollection` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `singleCustomerTextBox_OnTextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `singleCustomerTextBox_OnTextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblRegion` | `singleCustomerTextBox_OnTextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `singleCustomerTextBox_OnTextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-FIN-002-001` | {0} | Code | `Solution.Web/SInventory_UI/BatchWiseCollection.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `batchId`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |
| Database object | `tblArea` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerPay` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceBatch` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-FIN-002-001` | Unauthenticated access | Request `/SInventory_UI/BatchWiseCollection.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-FIN-002-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-FIN-002-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/BatchWiseCollection.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/BatchWiseCollection.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 27 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 13 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 12 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` is **shared with 12 other page(s)** (e.g. `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerPayment.aspx`, `CustomerPaymentBatchReport.aspx`, `CustomerPaymentProcess.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderListBLL.cs` is **shared with 20 other page(s)** (e.g. `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs` is **shared with 7 other page(s)** (e.g. `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseMoneyReceiptReportViewer.aspx`, `NTInvoiceReturnReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` is **shared with 23 other page(s)** (e.g. `AITReceiptViewer.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseMoneyReceiptReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` is **shared with 58 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` is **shared with 59 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs` is **shared with 2 other page(s)** (e.g. `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs` is **shared with 9 other page(s)** (e.g. `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CreditCustomerReport.aspx`, `DcWiseAreaAlocation.aspx`, `MaterialConversionReceive.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` is **shared with 14 other page(s)** (e.g. `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` is **shared with 7 other page(s)** (e.g. `HomePage.aspx`, `AuditReportOne.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OrderListDAL.cs` is **shared with 17 other page(s)** (e.g. `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SampleIssueDal.cs` is **shared with 17 other page(s)** (e.g. `SampleIssueReportViewer.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WebServiceDal.cs` is **shared with 14 other page(s)** (e.g. `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerPay` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceBatch` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
