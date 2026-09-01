# RET-003 — ReturnProductCustomerByInvoice

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RET-003` |
| Module | `returns` |
| File name | `ReturnProductCustomerByInvoice.aspx` |
| Route | `/SInventory_UI/ReturnProductCustomerByInvoice.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ReturnProductCustomerByInvoice.aspx.cs` |
| Code-behind class | `SInventory_UI_ReturnProductCustomerByInvoice` |
| Markup lines | 478 |
| Code-behind lines | 323 |
| Server controls | 49 |
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

`/SInventory_UI/ReturnProductCustomerByInvoice.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ReturnProductCustomerByInvoice.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ReturnProductCustomerByInvoice.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 7. UI Control Inventory

49 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 15 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 16 |
| 4 | `delInvNoTextBox0` | asp:TextBox | Delivery Invoice No: |  | True |  |  |  | 54 |
| 5 | `paymentTypeRadioButtonList` | asp:RadioButtonList | Payment Type: |  |  |  |  |  | 74 |
| 6 | `(no id)` | asp:ListItem | Payment Type: | Cash |  |  |  |  | 75 |
| 7 | `(no id)` | asp:ListItem |  | Credit |  |  |  |  | 76 |
| 8 | `ddlAdjustmentInv` | asp:DropdownList | Adj. Invoice No: |  |  |  |  |  | 95 |
| 9 | `Button2` | asp:Button |  | Search |  |  |  | Button2_Click | 114 |
| 10 | `custCodeTextBox` | asp:Label | Customer Code: |  | True |  |  |  | 169 |
| 11 | `custNameTextBox` | asp:Label | Customer Name: |  | True |  |  |  | 175 |
| 12 | `custAddressTextBox` | asp:Label | Customer Address: |  |  | ReadOnly=True |  |  | 188 |
| 13 | `comUnitNameTextBox` | asp:Label | Salse Center: |  |  | ReadOnly=True |  |  | 196 |
| 14 | `miaCodeTextBox` | asp:Label | MIO Code: |  |  | ReadOnly=True |  |  | 202 |
| 15 | `miaNameTextBox` | asp:Label | MIO Name: |  |  | ReadOnly=True |  |  | 208 |
| 16 | `payamntTextBox` | asp:Label | Payment Amount: |  | True |  |  |  | 216 |
| 17 | `areaNameTextBox` | asp:Label | Territory Name: |  |  | ReadOnly=True |  |  | 222 |
| 18 | `marketNameTextBox` | asp:Label | Merket Name: |  |  | ReadOnly=True |  |  | 228 |
| 19 | `custCategoryTextBox` | asp:Label | Customer Category: |  |  | ReadOnly=True |  |  | 236 |
| 20 | `orderNoTextBox` | asp:Label | Order No: |  |  |  |  |  | 242 |
| 21 | `orderDateTextBox` | asp:Label | Order Date: |  |  |  |  |  | 248 |
| 22 | `invDateTextBox` | asp:Label | Invoice Date: |  |  | ReadOnly=True |  |  | 258 |
| 23 | `deliverypersonMobileTextBox` | asp:Label | Delivery Person Mob.No: |  |  |  |  |  | 264 |
| 24 | `deliverypersonNameTextBox` | asp:Label |  |  |  |  |  |  | 273 |
| 25 | `remarksTextBox` | asp:Label | Remarks: |  |  |  |  |  | 282 |
| 26 | `payTypeDDL` | asp:DropDownList | Payment Type |  |  |  |  |  | 309 |
| 27 | `invoiceNoHiddenField` | asp:HiddenField | nbsp; |  |  |  |  |  | 331 |
| 28 | `invoiceHiddenField` | asp:HiddenField |  |  |  |  |  |  | 332 |
| 29 | `orderIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 333 |
| 30 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 339 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 342 |
| 32 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 343 |
| 33 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 344 |
| 34 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 345 |
| 35 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 346 |
| 36 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 348 |
| 37 | `retQtyTextBox` | asp:TextBox | loadGridView → col[5] "Return Qty" |  | True |  |  | retQtyTextBox_OnTextChanged | 350 |
| 38 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 353 |
| 39 | `retamountTextBox` | asp:TextBox | loadGridView → col[6] "Return Amount" |  | True | Enabled=False |  |  | 355 |
| 40 | `Button1` | asp:Button |  | Save Return |  |  |  | saveButton_Click | 416 |
| 41 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 419 |
| 42 | `UpdatePanel3` | asp:UpdatePanel |  |  |  |  |  |  | 423 |
| 43 | `UpdateProgress2` | asp:UpdateProgress |  |  |  |  |  |  | 428 |
| 44 | `Img2` | asp:Image |  | ~/Images/ajax-loader.gif |  |  |  |  | 432 |
| 45 | `hdCustomerMasterId` | asp:HiddenField |  |  |  |  |  |  | 443 |
| 46 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 444 |
| 47 | `hdMiaId` | asp:HiddenField |  |  |  |  |  |  | 447 |
| 48 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 468 |
| 49 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 471 |

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

DAO / entity types instantiated by this page: `CreditAdjustmentDao`, `CustomerReturnDetailDAO`, `CustomerReturnMasterDAO`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.PaymentTypeLoadBLL(...)` | `InvoiceBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `GetCustInfo` | `aInvoiceBll.CustomerMaster(...)` | `InvoiceBLL.CustomerMaster(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — |
| `LoadInvoice` | `aOrderInfoBll.LoadInvoice(...)` | `OrderInfoBLL.LoadInvoice(string invoiceId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct` | — |
| `LoadAdjustableInvoiceNo` | `aInvoiceBll.LoadAdjustableInvoiceNo(...)` | `InvoiceBLL.LoadAdjustableInvoiceNo(DropDownList ddl,string customerCode)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.LoadAdjustableInvoiceNo` → `InvoiceDAL.LoadAdjustableInvoiceNo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblArea`, `tblCompanyUnit`, `tblDistrict` | — |
| `saveButton_Click` | `aCustomerReturnBll.SaveCustomerReturn(...)` | `CustomerReturnBLL.SaveCustomerReturn(CustomerReturnMasterDAO aCustomerReturnMasterDao)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.SaveCustomerReturn` → `CustomerReturnDAL.SaveCustomerReturn` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblCustomerReturnMaster` | — |
| `saveButton_Click` | `aCustomerReturnBll.SaveCustomerReturnDetail(...)` | `CustomerReturnBLL.SaveCustomerReturnDetail(CustomerReturnDetailDAO aCustomerReturnDetailDao)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.SaveCustomerReturnDetail` → `CustomerReturnDAL.SaveCustomerReturnDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` | INSERT | `tblCustomerReturnDetail` | — |
| `saveButton_Click` | `aInvoiceBll.GetCompanyIdByInvoice(...)` | `InvoiceBLL.GetCompanyIdByInvoice(string invoiceId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetCompanyIdByInvoice` → `InvoiceDAL.GetCompanyIdByInvoiceId` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice` | — |
| `saveButton_Click` | `aInvoiceBll.SaveCreditAdjustmentInfo(...)` | `InvoiceBLL.SaveCreditAdjustmentInfo(CreditAdjustmentDao adjustmentDao)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.SaveCreditAdjustmentInfo` → `InvoiceDAL.SaveCreditAdjustmentInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT | `tblCreditAdjustment` | — |
| `Button2_Click` | `aCustomerReturnBll.LoadInvoiceNew(...)` | `CustomerReturnBLL.LoadInvoiceNew(string delinvno)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerReturnDetail`, `tblCustomerReturnMaster`, `tblInvoice`, `tblInvoiceDetail` | — |
| `Button3_Click` | `aCustomerReturnBll.LoadInvoiceNew(...)` | `CustomerReturnBLL.LoadInvoiceNew(string delinvno)` <br/>`Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerReturnDetail`, `tblCustomerReturnMaster`, `tblInvoice`, `tblInvoiceDetail` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `paymentTypeRadioButtonList` | Payment Type: | asp:RadioButtonList | False | — | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |
| `ddlAdjustmentInv` | Adj. Invoice No: | asp:DropdownList | False | — | `aInvoiceBll.LoadAdjustableInvoiceNo()` in `LoadAdjustableInvoiceNo()` | `tblArea`, `tblCompanyUnit`, `tblDistrict` | set in code (see the DAL method above) |
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
| `Button2` | asp:Button | Search | — | `Button2_Click` | 20 lines | `aCustomerReturnBll.LoadInvoiceNew` | — |
| `Button1` | asp:Button | Save Return | return confirm('Are you sure you want to Return Invoice ?'); | `saveButton_Click` | 86 lines | `aCustomerReturnBll.SaveCustomerReturn`, `aCustomerReturnBll.SaveCustomerReturnDetail`, `aInvoiceBll.GetCompanyIdByInvoice`, `aInvoiceBll.SaveCreditAdjustmentInfo` | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |

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
| 3 | `GetCustInfo` | `void GetCustInfo(string custCode)` | 27 | `aInvoiceBll.CustomerMaster` | — | — | — |
| 4 | `LoadInvoice` | `void LoadInvoice(string invoiceId)` | 24 | `aOrderInfoBll.LoadInvoice` | — | — | — |
| 5 | `LoadAdjustableInvoiceNo` | `void LoadAdjustableInvoiceNo(string customerCode)` | 6 | `aInvoiceBll.LoadAdjustableInvoiceNo` | — | — | — |
| 6 | `Validation` | `bool Validation()` | 34 | — | — | — | — |
| 7 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 86 | `aCustomerReturnBll.SaveCustomerReturn`, `aCustomerReturnBll.SaveCustomerReturnDetail`, `aInvoiceBll.GetCompanyIdByInvoice`, `aInvoiceBll.SaveCreditAdjustmentInfo` | — | — | `LoginName` |
| 8 | `Clear` | `void Clear()` | 26 | — | — | — | — |
| 9 | `Button2_Click` | `void Button2_Click(object sender, EventArgs e)` | 20 | `aCustomerReturnBll.LoadInvoiceNew` | — | — | — |
| 10 | `Button3_Click` | `void Button3_Click(object sender, EventArgs e)` | 12 | `aCustomerReturnBll.LoadInvoiceNew` | — | — | — |
| 11 | `retQtyTextBox_OnTextChanged` | `void retQtyTextBox_OnTextChanged(object sender, EventArgs e)` | 21 | — | — | — | — |
| 12 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 13 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ReturnProductCustomerByInvoice.aspx (markup)
  -> SInventory_UI_ReturnProductCustomerByInvoice (code-behind event handler)
     -> Library.BLL.SInventory_BLL.*BLL          (thin pass-through + calculations)
        -> Library.DAL.SInventory_DAL.*DAL       (builds SQL string / SqlParameter list)
           -> Library.DAL.InternalCls.ClsCommonInternalDAL
              -> EXEC ExecuteAllSqlQueryByStoreProcedure @Query   (for inline SQL)
              -> EXEC <named stored procedure>                    (for parameterised calls)
                 -> SQL Server: SalesrollDB_ZAS
```

## 13. API Mapping

**None.** This page exposes and consumes no HTTP API; all interaction is full or partial (UpdatePanel) postback. See `specs/00-system/api-inventory.md` for the complete, application-wide API surface.

## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `InvoiceBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GetCustInfo` | `InvoiceBLL` | `CustomerMaster` | `string CustomerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `LoadInvoice` | `OrderInfoBLL` | `LoadInvoice` | `string invoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadAdjustableInvoiceNo` | `InvoiceBLL` | `LoadAdjustableInvoiceNo` | `DropDownList ddl,string customerCode` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `CustomerReturnBLL` | `SaveCustomerReturn` | `CustomerReturnMasterDAO aCustomerReturnMasterDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `saveButton_Click` | `CustomerReturnBLL` | `SaveCustomerReturnDetail` | `CustomerReturnDetailDAO aCustomerReturnDetailDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `GetCompanyIdByInvoice` | `string invoiceId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `saveButton_Click` | `InvoiceBLL` | `SaveCreditAdjustmentInfo` | `CreditAdjustmentDao adjustmentDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `Button2_Click` | `CustomerReturnBLL` | `LoadInvoiceNew` | `string delinvno` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
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
| `tblArea` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAdjustableInvoiceNo()` → `InvoiceBLL.LoadAdjustableInvoiceNo` → `InvoiceDAL.LoadAdjustableInvoiceNo` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblCompanyInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `InvoiceBLL.GetCompanyIdByInvoice` → `InvoiceDAL.GetCompanyIdByInvoiceId` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadAdjustableInvoiceNo()` → `InvoiceBLL.LoadAdjustableInvoiceNo` → `InvoiceDAL.LoadAdjustableInvoiceNo` → `ClsCommonInternalDAL.LoadDropDownValue`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `InvoiceBLL.GetCompanyIdByInvoice` → `InvoiceDAL.GetCompanyIdByInvoiceId` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCreditAdjustment` | `saveButton_Click()` → `InvoiceBLL.SaveCreditAdjustmentInfo` → `InvoiceDAL.SaveCreditAdjustmentInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` |
| `tblCustCategory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustPayDetail` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerReturnDetail` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `CustomerReturnBLL.SaveCustomerReturnDetail` → `CustomerReturnDAL.SaveCustomerReturnDetail` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` |
| `tblCustomerReturnMaster` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `CustomerReturnBLL.SaveCustomerReturn` → `CustomerReturnDAL.SaveCustomerReturn` → `ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity` |
| `tblDcWiseAreaInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDistrict` | `LoadAdjustableInvoiceNo()` → `InvoiceBLL.LoadAdjustableInvoiceNo` → `InvoiceDAL.LoadAdjustableInvoiceNo` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblEmpGeneralInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `InvoiceBLL.GetCompanyIdByInvoice` → `InvoiceDAL.GetCompanyIdByInvoiceId` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `Button2_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `Button3_Click()` → `CustomerReturnBLL.LoadInvoiceNew` → `CustomerReturnDAL.LoadInvoiceNew` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPaymentType` | `Page_Load()` → `InvoiceBLL.PaymentTypeLoadBLL` → `InvoiceDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `LoadInvoice()` → `OrderInfoBLL.LoadInvoice` → `OrderInfoDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `GetCustInfo()` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.DataContainerDataTable` |

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
| `BR-RET-003-001` | {0} | Code | `Solution.Web/SInventory_UI/ReturnProductCustomerByInvoice.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCreditAdjustment` |
| Database object | `tblCustCategory` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerReturnDetail` |
| Database object | `tblCustomerReturnMaster` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblDistrict` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblPaymentType` |
| Database object | `tblProduct` |
| Database object | `tblTerritory` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RET-003-001` | Unauthenticated access | Request `/SInventory_UI/ReturnProductCustomerByInvoice.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-RET-003-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-RET-003-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ReturnProductCustomerByInvoice.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ReturnProductCustomerByInvoice.aspx.cs` |
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
| UI controls | absent | 49 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 13 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 20 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` is **shared with 4 other page(s)** (e.g. `PettyCashApproval.aspx`, `ReturnProductConsCustomerByInvoice.aspx`, `ReturnustomerByInvoiceApproval.aspx`, `SampleProductIssueApproval.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` is **shared with 43 other page(s)** (e.g. `ProformaHTMLReport.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseDelReturnViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCreditAdjustment` — any schema change to it breaks this page.
- Writes/reads `tblCustCategory` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerReturnDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerReturnMaster` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblDistrict` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblPaymentType` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
