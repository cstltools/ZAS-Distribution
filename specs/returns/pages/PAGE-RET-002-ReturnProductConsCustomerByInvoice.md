# RET-002 — ReturnProductConsCustomerByInvoice

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RET-002` |
| Module | `returns` |
| File name | `ReturnProductConsCustomerByInvoice.aspx` |
| Route | `/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ReturnProductConsCustomerByInvoice.aspx.cs` |
| Code-behind class | `SInventory_UI_ReturnProductCustomerByInvoice` |
| Markup lines | 311 |
| Code-behind lines | 239 |
| Server controls | 33 |
| GridViews | 1 |
| Code-behind methods | 10 |

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

`/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 7. UI Control Inventory

33 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `delInvNoTextBox0` | asp:TextBox | Delivery Invoice No |  | True |  |  |  | 34 |
| 5 | `Button2` | asp:Button |  | Search |  |  |  | Button2_Click | 38 |
| 6 | `payamntTextBox` | asp:TextBox | Payment Amount |  | True |  |  |  | 52 |
| 7 | `custCodeTextBox` | asp:TextBox | Customer Code |  | True |  |  |  | 68 |
| 8 | `custNameTextBox` | asp:TextBox | CustomerName |  | True |  |  |  | 74 |
| 9 | `custNameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender | CustomerName |  |  | Enabled=True |  |  | 77 |
| 10 | `custAddressTextBox` | asp:TextBox | Customer Address |  |  | ReadOnly=True |  |  | 94 |
| 11 | `invDateTextBox` | asp:TextBox | Invoice Date |  |  |  |  |  | 102 |
| 12 | `invoiceNoHiddenField` | asp:HiddenField |  |  |  |  |  |  | 141 |
| 13 | `invoiceHiddenField` | asp:HiddenField |  |  |  |  |  |  | 142 |
| 14 | `orderIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 143 |
| 15 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 149 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 152 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 153 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 154 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 155 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 156 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 158 |
| 22 | `retQtyTextBox` | asp:TextBox | loadGridView → col[5] "Return Qty" |  | True |  |  | retQtyTextBox_OnTextChanged | 160 |
| 23 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 164 |
| 24 | `retamountTextBox` | asp:TextBox | loadGridView → col[6] "Return Amount" |  | True | Enabled=False |  |  | 166 |
| 25 | `UpdatePanel3` | asp:UpdatePanel |  |  |  |  |  |  | 257 |
| 26 | `Button1` | asp:Button |  | Save Return |  |  |  | saveButton_Click | 259 |
| 27 | `UpdateProgress2` | asp:UpdateProgress |  |  |  |  |  |  | 262 |
| 28 | `Img2` | asp:Image |  | ~/Images/ajax-loader.gif |  |  |  |  | 266 |
| 29 | `hdCustomerMasterId` | asp:HiddenField |  |  |  |  |  |  | 277 |
| 30 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 278 |
| 31 | `hdMiaId` | asp:HiddenField |  |  |  |  |  |  | 282 |
| 32 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 299 |
| 33 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 302 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceId,InvoiceDetailId,UnitPrice,UnitVatAmount` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Pack Size | PackSize | — | — | — |
| 3 | BoundField | Avialable Qty | AvialableQty | — | — | — |
| 4 | BoundField | Net Amount | NetAmount | — | — | — |
| 5 | TemplateField | Return Qty | — | — | `TextBox:retQtyTextBox` | — |
| 6 | TemplateField | Return Amount | — | — | `TextBox:retamountTextBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CustomerReturnDetailDAO`, `CustomerReturnMasterDAO`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.PaymentTypeLoadBLL(...)` | `InvoiceBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `GetCustInfo` | `aInvoiceBll.LoadConsumerCustomer(...)` | `InvoiceBLL.LoadConsumerCustomer(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadConsumerCustomer` → `InvoiceDAL.LoadConsumerCustomer` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblConsumerCustomer` | — |
| `LoadInvoice` | `aOrderInfoBll.LoadInvoice(...)` | `OrderInfoBLL.LoadInvoice(string invoiceId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — |
| `saveButton_Click` | `aCustomerReturnBll.ApprovalUpdateDAL(...)` | `CustomerReturnBLL.ApprovalUpdateDAL(CustomerReturnMasterDAO aCustomerReturnMasterDao)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.ApprovalUpdateDAL` | — | — | — |
| `saveButton_Click` | `aCustomerReturnBll.SaveCustomerReturn(...)` | `CustomerReturnBLL.SaveCustomerReturn(CustomerReturnMasterDAO aCustomerReturnMasterDao)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.SaveCustomerReturn` → `CustomerReturnDAL.SaveCustomerReturn` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblCustomerReturnMaster` | — |
| `saveButton_Click` | `aCustomerReturnBll.SaveCustomerReturnDetail(...)` | `CustomerReturnBLL.SaveCustomerReturnDetail(CustomerReturnDetailDAO aCustomerReturnDetailDao)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.SaveCustomerReturnDetail` → `CustomerReturnDAL.SaveCustomerReturnDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblCustomerReturnDetail` | — |
| `saveButton_Click` | `aCustomerReturnBll.UpdateDCStoreQty(...)` | `CustomerReturnBLL.UpdateDCStoreQty(string dcstoreId, int qty)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.UpdateDCStoreQty` → `CustomerReturnDAL.UpdateDCStoreQty` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDCStore` | — |
| `saveButton_Click` | `aInvoiceBll.GetDcStoreId(...)` | `InvoiceBLL.GetDcStoreId(string invoicedetailId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetDcStoreId` → `InvoiceDAL.GetDcStoreId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoiceDetail` | — |
| `Button2_Click` | `aCustomerReturnBll.LoadInvoiceNewConsumer(...)` | `CustomerReturnBLL.LoadInvoiceNewConsumer(string delinvno)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.LoadInvoiceNewConsumer` → `CustomerReturnDAL.LoadInvoiceNewConsumer` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustPayDetail`, `tblCustomerReturnDetail`, `tblInvoice`, `tblInvoiceDetail` | — |
| `Button3_Click` | `aCustomerReturnBll.LoadInvoiceNew(...)` | `CustomerReturnBLL.LoadInvoiceNew(string delinvno)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerReturnDetail`, `tblCustomerReturnMaster`, `tblInvoice`, `tblInvoiceDetail` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `Button2` | asp:Button | Search | — | `Button2_Click` | 12 lines | `aCustomerReturnBll.LoadInvoiceNewConsumer` | — |
| `Button1` | asp:Button | Save Return | return confirm('Are you sure you want to Return Invoice ?'); | `saveButton_Click` | 71 lines | `aCustomerReturnBll.ApprovalUpdateDAL`, `aCustomerReturnBll.SaveCustomerReturn`, `aCustomerReturnBll.SaveCustomerReturnDetail`, `aCustomerReturnBll.UpdateDCStoreQty`, `aInvoiceBll.GetDcStoreId` | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 2
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 15 | `aInvoiceBll.PaymentTypeLoadBLL` | — | — | `InvoiceId` |
| 2 | `Todate` | `void Todate()` | 3 | — | — | — | — |
| 3 | `GetCustInfo` | `void GetCustInfo(string custCode)` | 28 | `aInvoiceBll.LoadConsumerCustomer` | — | — | — |
| 4 | `LoadInvoice` | `void LoadInvoice(string invoiceId)` | 21 | `aOrderInfoBll.LoadInvoice` | — | — | — |
| 5 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 71 | `aCustomerReturnBll.ApprovalUpdateDAL`, `aCustomerReturnBll.SaveCustomerReturn`, `aCustomerReturnBll.SaveCustomerReturnDetail`, `aCustomerReturnBll.UpdateDCStoreQty`, `aInvoiceBll.GetDcStoreId` | — | — | `LoginName` |
| 6 | `Clear` | `void Clear()` | 19 | — | — | — | — |
| 7 | `Button2_Click` | `void Button2_Click(object sender, EventArgs e)` | 12 | `aCustomerReturnBll.LoadInvoiceNewConsumer` | — | — | — |
| 8 | `Button3_Click` | `void Button3_Click(object sender, EventArgs e)` | 12 | `aCustomerReturnBll.LoadInvoiceNew` | — | — | — |
| 9 | `retQtyTextBox_OnTextChanged` | `void retQtyTextBox_OnTextChanged(object sender, EventArgs e)` | 20 | — | — | — | — |
| 10 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ReturnProductConsCustomerByInvoice.aspx (markup)
  -> SInventory_UI_ReturnProductCustomerByInvoice (code-behind event handler)
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


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `InvoiceBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetCustInfo` | `InvoiceBLL` | `LoadConsumerCustomer` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoadInvoice` | `OrderInfoBLL` | `LoadInvoice` | `string invoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `saveButton_Click` | `CustomerReturnBLL` | `ApprovalUpdateDAL` | `CustomerReturnMasterDAO aCustomerReturnMasterDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `saveButton_Click` | `CustomerReturnBLL` | `SaveCustomerReturn` | `CustomerReturnMasterDAO aCustomerReturnMasterDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `saveButton_Click` | `CustomerReturnBLL` | `SaveCustomerReturnDetail` | `CustomerReturnDetailDAO aCustomerReturnDetailDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `saveButton_Click` | `CustomerReturnBLL` | `UpdateDCStoreQty` | `string dcstoreId, int qty` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `GetDcStoreId` | `string invoicedetailId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Button2_Click` | `CustomerReturnBLL` | `LoadInvoiceNewConsumer` | `string delinvno` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `Button3_Click` | `CustomerReturnBLL` | `LoadInvoiceNew` | `string delinvno` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyUnit` | `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblConsumerCustomer` | `GetCustInfo()` → `InvoiceBLL.LoadConsumerCustomer` → `InvoiceDAL.LoadConsumerCustomer` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustPayDetail` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNewConsumer` → `CustomerReturnDAL.LoadInvoiceNewConsumer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerReturnDetail` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNewConsumer` → `CustomerReturnDAL.LoadInvoiceNewConsumer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `CustomerReturnBLL.SaveCustomerReturnDetail` → `CustomerReturnDAL.SaveCustomerReturnDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` |
| `tblCustomerReturnMaster` | `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `CustomerReturnBLL.SaveCustomerReturn` → `CustomerReturnDAL.SaveCustomerReturn` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` |
| `tblDCStore` | `saveButton_Click()` → `CustomerReturnBLL.UpdateDCStoreQty` → `CustomerReturnDAL.UpdateDCStoreQty` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblDeliveryManInfo` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNewConsumer` → `CustomerReturnDAL.LoadInvoiceNewConsumer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNewConsumer` → `CustomerReturnDAL.LoadInvoiceNewConsumer` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `InvoiceBLL.GetDcStoreId` → `InvoiceDAL.GetDcStoreId` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentType` | `Page_Load()` → `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-RET-002-001` | {0} | Code | `Solution.Web/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `InvoiceId`, `LoginName`

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

Audit columns written by this page: `EntryBy`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Database object | `tblCompanyUnit` |
| Database object | `tblConsumerCustomer` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerReturnDetail` |
| Database object | `tblCustomerReturnMaster` |
| Database object | `tblDCStore` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblPaymentType` |
| Database object | `tblProduct` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RET-002-001` | Unauthenticated access | Request `/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-RET-002-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-RET-002-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 33 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 10 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 13 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` is **shared with 4 other page(s)** (e.g. `PettyCashApproval.aspx`, `ReturnProductCustomerByInvoice.aspx`, `ReturnustomerByInvoiceApproval.aspx`, `SampleProductIssueApproval.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` is **shared with 43 other page(s)** (e.g. `ProformaHTMLReport.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseDelReturnViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblConsumerCustomer` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerReturnDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerReturnMaster` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblPaymentType` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
