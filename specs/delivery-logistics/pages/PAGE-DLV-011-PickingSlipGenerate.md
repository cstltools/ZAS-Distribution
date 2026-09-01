# DLV-011 — PickingSlipGenerate

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `DLV-011` |
| Module | `delivery-logistics` |
| File name | `PickingSlipGenerate.aspx` |
| Route | `/SInventory_UI/PickingSlipGenerate.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `PickingSlipGenerate.aspx.cs` |
| Code-behind class | `SInventory_UI_PickingSlipGenerate` |
| Markup lines | 501 |
| Code-behind lines | 235 |
| Server controls | 25 |
| GridViews | 1 |
| Code-behind methods | 14 |

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

`/SInventory_UI/PickingSlipGenerate.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/PickingSlipGenerate.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/PickingSlipGenerate.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 7. UI Control Inventory

25 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 54 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 56 |
| 4 | `manufacDropDownList` | asp:DropDownList | Company Name: |  | True |  | False | manufacDropDownList_SelectedIndexChanged | 90 |
| 5 | `delDropDownList` | asp:DropDownList | Delivery Man: |  | True |  |  | marketDropDownList_SelectedIndexChanged | 109 |
| 6 | `dateTextBox` | asp:TextBox | Date: |  | True |  |  | dateTextBox_TextChanged | 127 |
| 7 | `imgDate` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 129 |
| 8 | `Date` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 131 |
| 9 | `batchDropDownList` | asp:DropDownList | Batch No : |  |  |  |  |  | 150 |
| 10 | `searchButton` | asp:Button |  | Search |  |  |  | Button1_Click | 188 |
| 11 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 190 |
| 12 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 238 |
| 13 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 241 |
| 14 | `LabelSL` | asp:Label | loadGridView → col[0] "#SL" |  |  |  |  |  | 243 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 246 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 247 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 248 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 249 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 250 |
| 20 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 251 |
| 21 | `chkSelectAll` | asp:CheckBox | loadGridView → col[6] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 253 |
| 22 | `chkSelect` | asp:CheckBox | loadGridView → col[6] "(no header)" |  | True |  |  |  | 256 |
| 23 | `ViewButton` | asp:Button |  | Generate Picking |  |  |  | ViewButton_Click | 297 |
| 24 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 470 |
| 25 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 473 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | #SL | — | — | `Label:LabelSL` | — |
| 1 | BoundField | InvoiceNo | InvoiceNo | — | — | — |
| 2 | BoundField | InvoiceDate | InvoiceDate | — | — | — |
| 3 | BoundField | OrderNo | OrderNo | — | — | — |
| 4 | BoundField | OrderDate | OrderDate | — | — | — |
| 5 | BoundField | Total Amount | TpGrandTotal | — | — | — |
| 6 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aInvoiceBll.ReturnReason(...)` | `InvoiceBLL.ReturnReason(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ReturnReason` → `InvoiceDAL.ReturnReason` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblReturnReason` | — |
| `DropDownlist` | `aInvoiceBatchBll.LoadDeliveryMan(...)` | `InvoiceBatchBLL.LoadDeliveryMan(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` | `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `DropDownlist` | `aOrderInfoBll.LoadDeliveryMan(...)` | `OrderInfoBLL.LoadDeliveryMan(string deliveryId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `DropDownlist` | `aOrderInfoBll.LoadManufac(...)` | `OrderInfoBLL.LoadManufac(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyInfo` | — |
| `manufacDropDownList_SelectedIndexChanged` | `aOrderInfoBll.LoadDC(...)` | `OrderInfoBLL.LoadDC(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadDC` → `OrderInfoDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `marketDropDownList_SelectedIndexChanged` | `aOrderInfoBll.LoadOrderWithInvoice(...)` | `OrderInfoBLL.LoadOrderWithInvoice(string batchId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblMIOInfo`, `tblMarket`, `tblOrder` | — |
| `Button1_Click` | `aInvoiceBll.ReturnReason(...)` | `InvoiceBLL.ReturnReason(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.ReturnReason` → `InvoiceDAL.ReturnReason` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblReturnReason` | — |
| `GridView` | `aInvoiceBatchBll.LoadInvoiceNotNull(...)` | `InvoiceBatchBLL.LoadInvoiceNotNull(string delManId, string invDate, string batchId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` | `InvoiceBatchBLL.LoadInvoiceNotNull` → `InvoiceBatchDAL.LoadInvoiceNotNul` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice` | — |
| `SessionChoose` | `aOrderInfoBll.LoadOrderWithInvoice(...)` | `OrderInfoBLL.LoadOrderWithInvoice(string batchId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblMIOInfo`, `tblMarket`, `tblOrder` | — |
| `dateTextBox_TextChanged` | `aOrderInfoBll.BatchNoLoad(...)` | `OrderInfoBLL.BatchNoLoad(DropDownList aDownList, string companyId, string date, string delmanId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblInvoiceBatch` | — |
| `dateTextBox_TextChanged` | `aOrderInfoBll.BatchNoLoadByUser(...)` | `OrderInfoBLL.BatchNoLoadByUser(DropDownList aDownList, string date, string delmanId, string userId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblInvoice`, `tblInvoiceBatch`, `tblUserCompanyUnit` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `manufacDropDownList` | Company Name: | asp:DropDownList | True | manufacDropDownList_SelectedIndexChanged | `aOrderInfoBll.BatchNoLoad()` in `dateTextBox_TextChanged()`; `aOrderInfoBll.LoadDC()` in `manufacDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadManufac()` in `DropDownlist()`; `aOrderInfoBll.LoadOrderWithInvoice()` in `marketDropDownList_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblInvoiceBatch`, `tblMIOInfo`, `tblMarket`, `tblOrder`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `delDropDownList` | Delivery Man: | asp:DropDownList | True | marketDropDownList_SelectedIndexChanged | `aInvoiceBatchBll.LoadDeliveryMan()` in `DropDownlist()`; `aInvoiceBatchBll.LoadInvoiceNotNull()` in `GridView()`; `aOrderInfoBll.BatchNoLoad()` in `dateTextBox_TextChanged()`; `aOrderInfoBll.BatchNoLoadByUser()` in `dateTextBox_TextChanged()` | `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `batchDropDownList` | Batch No : | asp:DropDownList | False | — | `aInvoiceBatchBll.LoadInvoiceNotNull()` in `GridView()`; `aOrderInfoBll.BatchNoLoad()` in `dateTextBox_TextChanged()`; `aOrderInfoBll.BatchNoLoadByUser()` in `dateTextBox_TextChanged()`; `aOrderInfoBll.LoadOrderWithInvoice()` in `SessionChoose()` | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblInvoiceBatch`, `tblMIOInfo`, `tblMarket`, `tblOrder`, `tblUserCompanyUnit` | set in code (see the DAL method above) |

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
| `searchButton` | asp:Button | Search | — | `Button1_Click` | 9 lines | `aInvoiceBll.ReturnReason` | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |
| `ViewButton` | asp:Button | Generate Picking | — | `ViewButton_Click` | 37 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:CalendarExtender`(dateTextBox)
- External scripts: `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 20 | `aInvoiceBll.ReturnReason` | — | — | `DelMarketId` |
| 2 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 3 | `DropDownlist` | `void DropDownlist()` | 10 | `aInvoiceBatchBll.LoadDeliveryMan`, `aOrderInfoBll.LoadDeliveryMan`, `aOrderInfoBll.LoadManufac` | — | — | — |
| 4 | `manufacDropDownList_SelectedIndexChanged` | `void manufacDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 6 | `aOrderInfoBll.LoadDC` | — | — | — |
| 5 | `marketDropDownList_SelectedIndexChanged` | `void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 10 | `aOrderInfoBll.LoadOrderWithInvoice` | — | — | — |
| 6 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 7 | `Button1_Click` | `void Button1_Click(object sender, EventArgs e)` | 9 | `aInvoiceBll.ReturnReason` | — | — | — |
| 8 | `GridView` | `void GridView()` | 7 | `aInvoiceBatchBll.LoadInvoiceNotNull` | — | — | `DelMarketId` |
| 9 | `SessionChoose` | `void SessionChoose()` | 16 | `aOrderInfoBll.LoadOrderWithInvoice` | — | — | `DelMarketId` |
| 10 | `dateTextBox_TextChanged` | `void dateTextBox_TextChanged(object sender, EventArgs e)` | 17 | `aOrderInfoBll.BatchNoLoad`, `aOrderInfoBll.BatchNoLoadByUser` | — | — | `UserId`, `UserType` |
| 11 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 12 | `Clear` | `void Clear()` | 13 | — | — | — | — |
| 13 | `GetParameter` | `string GetParameter()` | 18 | — | — | — | — |
| 14 | `ViewButton_Click` | `void ViewButton_Click(object sender, EventArgs e)` | 37 | — | — | — | `DelMan`, `Manufac`, `Market`, `MrktRptParameter`, `invoicedate` |

Call chain shape for every persisting action on this page:

```
PickingSlipGenerate.aspx (markup)
  -> SInventory_UI_PickingSlipGenerate (code-behind event handler)
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
| `Page_Load` | `InvoiceBLL` | `ReturnReason` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `DropDownlist` | `InvoiceBatchBLL` | `LoadDeliveryMan` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| `DropDownlist` | `OrderInfoBLL` | `LoadDeliveryMan` | `string deliveryId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `DropDownlist` | `OrderInfoBLL` | `LoadManufac` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `manufacDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `marketDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `LoadOrderWithInvoice` | `string batchId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `Button1_Click` | `InvoiceBLL` | `ReturnReason` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `GridView` | `InvoiceBatchBLL` | `LoadInvoiceNotNull` | `string delManId, string invDate, string batchId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| `SessionChoose` | `OrderInfoBLL` | `LoadOrderWithInvoice` | `string batchId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dateTextBox_TextChanged` | `OrderInfoBLL` | `BatchNoLoad` | `DropDownList aDownList, string companyId, string date, strin` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `dateTextBox_TextChanged` | `OrderInfoBLL` | `BatchNoLoadByUser` | `DropDownList aDownList, string date, string delmanId, string` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `DropDownlist()` → `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyUnit` | `GridView()` → `InvoiceBatchBLL.LoadInvoiceNotNull` → `InvoiceBatchDAL.LoadInvoiceNotNul` → `ClsCommonInternalDAL.DataContainerDataTable`, `SessionChoose()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `manufacDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadDC` → `OrderInfoDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `GridView()` → `InvoiceBatchBLL.LoadInvoiceNotNull` → `InvoiceBatchDAL.LoadInvoiceNotNul` → `ClsCommonInternalDAL.DataContainerDataTable`, `SessionChoose()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `DropDownlist()` → `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue`, `DropDownlist()` → `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `DropDownlist()` → `InvoiceBatchBLL.LoadDeliveryMan` → `InvoiceBatchDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.LoadDropDownValue`, `DropDownlist()` → `OrderInfoBLL.LoadDeliveryMan` → `OrderInfoDAL.LoadDeliveryMan` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `GridView()` → `InvoiceBatchBLL.LoadInvoiceNotNull` → `InvoiceBatchDAL.LoadInvoiceNotNul` → `ClsCommonInternalDAL.DataContainerDataTable`, `SessionChoose()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `dateTextBox_TextChanged()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceBatch` | `dateTextBox_TextChanged()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `dateTextBox_TextChanged()` → `OrderInfoBLL.BatchNoLoad` → `OrderInfoDAL.BatchNoLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblMIOInfo` | `SessionChoose()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `SessionChoose()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `SessionChoose()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderWithInvoice` → `OrderInfoDAL.LoadOrderWithInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblReturnReason` | `Button1_Click()` → `InvoiceBLL.ReturnReason` → `InvoiceDAL.ReturnReason` → `ClsCommonInternalDAL.LoadDropDownValue`, `Page_Load()` → `InvoiceBLL.ReturnReason` → `InvoiceDAL.ReturnReason` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblUserCompanyUnit` | `dateTextBox_TextChanged()` → `OrderInfoBLL.BatchNoLoadByUser` → `OrderInfoDAL.BatchNoLoadByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `manufacDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadDC` → `OrderInfoDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |

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
| `BR-DLV-011-001` | {0} | Code | `Solution.Web/SInventory_UI/PickingSlipGenerate.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `DelMan`, `DelMarketId`, `Manufac`, `Market`, `MrktRptParameter`, `UserId`, `UserType`, `invoicedate`

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
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceBatch` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblReturnReason` |
| Database object | `tblUserCompanyUnit` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-DLV-011-001` | Unauthenticated access | Request `/SInventory_UI/PickingSlipGenerate.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-DLV-011-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-DLV-011-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/PickingSlipGenerate.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/PickingSlipGenerate.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 25 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 14 methods traced | n/a | **MISSING → now documented** | section 12 |
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

- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` is **shared with 43 other page(s)** (e.g. `ProformaHTMLReport.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseDelReturnViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` is **shared with 12 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerPayment.aspx`, `CustomerPaymentBatchReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceBatch` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblReturnReason` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
