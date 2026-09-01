# SYS-004 — HomePage

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SYS-004` |
| Module | `00-system` |
| File name | `HomePage.aspx` |
| Route | `/CommonUI/HomePage.aspx` |
| `Title` directive | `Salesroll` |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `HomePage.aspx.cs` |
| Code-behind class | `CommonUI_HomePage` |
| Markup lines | 664 |
| Code-behind lines | 227 |
| Server controls | 61 |
| GridViews | 4 |
| Code-behind methods | 13 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 4 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 4 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `Login.aspx`

Outbound `Response.Redirect` targets: `~/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx`

## 5. Route

`/CommonUI/HomePage.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/CommonUI/HomePage.aspx` |
| Code-behind | `Solution.Web/CommonUI/HomePage.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |

## 7. UI Control Inventory

61 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 42 |
| 3 | `orderDateTextBox` | asp:TextBox | Order Date: |  |  |  |  |  | 79 |
| 4 | `imgDate` | asp:ImageButton | Order Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 80 |
| 5 | `Date` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 82 |
| 6 | `Button1` | asp:Button |  | Search |  |  |  | Button1_Click | 87 |
| 7 | `orderGridView` | asp:GridView |  |  |  |  |  |  | 108 |
| 8 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 111 |
| 9 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 112 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 113 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 114 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 115 |
| 13 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 116 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 117 |
| 15 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 119 |
| 16 | `gotoinvoiceButton` | asp:Button | orderGridView → col[7] "Go To Invoice" | Go To Invoice |  |  |  | gotoinvoiceButton_Click | 121 |
| 17 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 127 |
| 18 | `OrderDetailsV` | asp:LinkButton | orderGridView → col[8] "Order Details" | Order Details View |  |  |  | OrderButton_Click | 129 |
| 19 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 135 |
| 20 | `invoiceButton` | asp:LinkButton | orderGridView → col[9] "Action" | Preview Due History &gt; |  |  |  | invoiceButton_Click | 137 |
| 21 | `ModalPopupExtender1` | ajaxToolkit:ModalPopupExtender |  |  |  |  |  |  | 346 |
| 22 | `HiddenField1` | asp:HiddenField |  |  |  |  |  |  | 349 |
| 23 | `Panel1` | asp:Panel |  |  |  |  |  |  | 350 |
| 24 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 352 |
| 25 | `dueGrid` | asp:GridView | Due History |  |  |  |  |  | 365 |
| 26 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 368 |
| 27 | `LabelSL` | asp:Label | GridView1 → col[0] "#SL" |  |  |  |  |  | 370 |
| 28 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 373 |
| 29 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 374 |
| 30 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 375 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 376 |
| 32 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 377 |
| 33 | `Button2` | asp:Button |  | Close |  |  |  | btnDue_OnClick | 429 |
| 34 | `ModalPopupExtender10` | ajaxToolkit:ModalPopupExtender | bsp; |  |  |  |  |  | 466 |
| 35 | `HiddenField10` | asp:HiddenField |  |  |  |  |  |  | 469 |
| 36 | `Panel10` | asp:Panel |  |  |  |  |  |  | 470 |
| 37 | `UpdatePanel11` | asp:UpdatePanel |  |  |  |  |  |  | 472 |
| 38 | `dueGridView` | asp:GridView |  |  |  |  |  |  | 484 |
| 39 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 487 |
| 40 | `LabelSL` | asp:Label | GridView1 → col[0] "#SL" |  |  |  |  |  | 489 |
| 41 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 492 |
| 42 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 493 |
| 43 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 494 |
| 44 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 495 |
| 45 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 496 |
| 46 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 497 |
| 47 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 498 |
| 48 | `Button41` | asp:Button |  | Close |  |  |  | Button41_OnClick | 535 |
| 49 | `ModalPopupExtender2` | ajaxToolkit:ModalPopupExtender |  |  |  |  |  |  | 567 |
| 50 | `HiddenField11` | asp:HiddenField |  |  |  |  |  |  | 570 |
| 51 | `Panel2` | asp:Panel |  |  |  |  |  |  | 571 |
| 52 | `UpdatePanel2` | asp:UpdatePanel |  |  |  |  |  |  | 573 |
| 53 | `GridView1` | asp:GridView | Details List |  |  |  |  |  | 585 |
| 54 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 588 |
| 55 | `LabelSL` | asp:Label | GridView1 → col[0] "#SL" |  |  |  |  |  | 590 |
| 56 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 593 |
| 57 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 594 |
| 58 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 595 |
| 59 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 596 |
| 60 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 597 |
| 61 | `Button3` | asp:Button |  | Close |  |  |  | Button42_OnClick | 634 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `orderGridView`

| Attribute | Value |
|---|---|
| datakeynames | `ComUnitId,ManufacId,OrderId,Permission,CustomerMasterId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Wings Name | ComUnitName | — | — | — |
| 1 | BoundField | Order No | OrderCode | — | — | — |
| 2 | BoundField | Order Date | SubmissionDate | — | — | — |
| 3 | BoundField | Customer Code | CustomerCode | — | — | — |
| 4 | BoundField | Customer Name | CustomerName | — | — | — |
| 5 | BoundField | Market | MarketName | — | — | — |
| 6 | BoundField | MIO | MIOCode | — | — | — |
| 7 | TemplateField | Go To Invoice | — | — | `Button:gotoinvoiceButton` | — |
| 8 | TemplateField | Order Details | — | — | `LinkButton:OrderDetailsV` | — |
| 9 | TemplateField | Action | — | — | `LinkButton:invoiceButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `dueGrid`

| Attribute | Value |
|---|---|
| datakeynames | `CustomerMasterId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | #SL | — | — | `Label:LabelSL` | — |
| 1 | BoundField | Company Name | CompanyName | — | — | — |
| 2 | BoundField | Unit Name | ComUnitName | — | — | — |
| 3 | BoundField | Invoice No | InvoiceNo | — | — | — |
| 4 | BoundField | Invoice Date | InvoiceDate | — | — | — |
| 5 | BoundField | Due Amount | DueAmount | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `dueGridView`

| Attribute | Value |
|---|---|
| datakeynames | `ProductCode` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | #SL | — | — | `Label:LabelSL` | — |
| 1 | BoundField | Product Code | ProductCode | — | — | — |
| 2 | BoundField | Product Name | ProductName | — | — | — |
| 3 | BoundField | PackSize | PackSize | — | — | — |
| 4 | BoundField | Mfg. Date | MfgDate | — | — | — |
| 5 | BoundField | Exp. Date | ExpDate | — | — | — |
| 6 | BoundField | Batch No | BatchNo | — | — | — |
| 7 | BoundField | StockQty | StockQty | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `GridView1`

| Attribute | Value |
|---|---|
| datakeynames | `ProductCode` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | #SL | — | — | `Label:LabelSL` | — |
| 1 | BoundField | Product Code | ProductCode | — | — | — |
| 2 | BoundField | Product Name | ProductName | — | — | — |
| 3 | BoundField | Quantity | Quantity | — | — | — |
| 4 | BoundField | TP Total | TotalTradePrice | — | — | — |
| 5 | BoundField | Remark | Remarks | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadPendingInvoice` | `aOrderInfoDal.LoadOrderForOrderCreation(...)` | `OrderInfoDAL.LoadOrderForOrderCreation(string comunitId, string manufacId, string marketId)` <br/>`Library.DAL/SInventory_DAL/OrderInfoDAL.cs` | `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblMarket`, `tblOrder` | — |
| `OrderButton_Click` | `aOrderInfoBll.LoadOrderDetails(...)` | `OrderInfoBLL.LoadOrderDetails(string customerMasterId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderDetails` → `OrderInfoDAL.OrderDetails` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblOrder`, `tblOrderDetail` | — |
| `LoadDueHistory` | `aOrderInfoBll.LoadCustomerDueHistory(...)` | `OrderInfoBLL.LoadCustomerDueHistory(string customerMasterId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CustomerMaster`, `View_InvoiceWiseDue`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblInvoice` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `imgDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `Button1` | asp:Button | Search | — | `Button1_Click` | 3 lines | — | — |
| `gotoinvoiceButton` | asp:Button | Go To Invoice | — | `gotoinvoiceButton_Click` | 10 lines | — | — |
| `OrderDetailsV` | asp:LinkButton | Order Details View | — | `OrderButton_Click` | 26 lines | `aOrderInfoBll.LoadOrderDetails` | — |
| `invoiceButton` | asp:LinkButton | Preview Due History &gt; | — | `invoiceButton_Click` | 15 lines | — | — |
| `Button2` | asp:Button | Close | — | `btnDue_OnClick` | 3 lines | — | — |
| `Button41` | asp:Button | Close | — | `Button41_OnClick` | 3 lines | — | — |
| `Button3` | asp:Button | Close | — | `Button42_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 3
- AJAX Control Toolkit extenders: `ajaxToolkit:CalendarExtender`(orderDateTextBox), `ajaxToolkit:ModalPopupExtender`(HiddenField1), `ajaxToolkit:ModalPopupExtender`(HiddenField10), `ajaxToolkit:ModalPopupExtender`(HiddenField11)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 9 | — | — | — | — |
| 2 | `SetOrderDateVisibility` | `void SetOrderDateVisibility()` | 9 | — | — | — | `UserId` |
| 3 | `btnDue_OnClick` | `void btnDue_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 4 | `LoadPendingInvoice` | `void LoadPendingInvoice()` | 40 | `aOrderInfoDal.LoadOrderForOrderCreation` | — | — | `UserId` |
| 5 | `invoiceButton_Click` | `void invoiceButton_Click(object sender, EventArgs e)` | 15 | — | — | — | — |
| 6 | `OrderButton_Click` | `void OrderButton_Click(object sender, EventArgs e)` | 26 | `aOrderInfoBll.LoadOrderDetails` | — | — | — |
| 7 | `LoadDueHistory` | `void LoadDueHistory(string customerMasterId)` | 16 | `aOrderInfoBll.LoadCustomerDueHistory` | — | — | — |
| 8 | `gotoinvoiceButton_Click` | `void gotoinvoiceButton_Click(object sender, EventArgs e)` | 10 | — | — | — | `OrderId` |
| 9 | `GenerateParameter` | `string GenerateParameter()` | 19 | — | `tblCompanyInfo`, `tblCompanyUnit`, `tblUserCompanyUnit` | — | `UserId` |
| 10 | `LoadExpireProduct` | `void LoadExpireProduct()` | 24 | — | — | — | `UserId` |
| 11 | `Button41_OnClick` | `void Button41_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 12 | `Button42_OnClick` | `void Button42_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 13 | `Button1_Click` | `void Button1_Click(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
HomePage.aspx (markup)
  -> CommonUI_HomePage (code-behind event handler)
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
| `OrderButton_Click` | `OrderInfoBLL` | `LoadOrderDetails` | `string customerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `LoadDueHistory` | `OrderInfoBLL` | `LoadCustomerDueHistory` | `string customerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadPendingInvoice` | `OrderInfoDAL` | `LoadOrderForOrderCreation` | `string comunitId, string manufacId, string marketId` | `DataTable` | SELECT | `tblCompanyUnit`, `tblMarket`, `tblOrder` | — | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_CustomerMaster` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `View_InvoiceWiseDue` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable`, code-behind `GenerateParameter()` |
| `tblCompanyUnit` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadPendingInvoice()` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, code-behind `GenerateParameter()` |
| `tblCustMaster` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `LoadPendingInvoice()` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `LoadPendingInvoice()` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `OrderButton_Click()` → `OrderInfoBLL.LoadOrderDetails` → `OrderInfoDAL.OrderDetails` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrderDetail` | `OrderButton_Click()` → `OrderInfoBLL.LoadOrderDetails` → `OrderInfoDAL.OrderDetails` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUserCompanyUnit` | code-behind `GenerateParameter()` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `LoadPendingInvoice` | Generate Invoice | No | Yes | UNKNOWN | Notification |
| `LoadPendingInvoice` | Unable to Access | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SYS-004-001` | Generate Invoice | Code | `Solution.Web/CommonUI/HomePage.aspx.cs` → `LoadPendingInvoice()` | user action on this page | operation is refused | "Generate Invoice" | CONFIRMED |
| `BR-SYS-004-002` | Unable to Access | Code | `Solution.Web/CommonUI/HomePage.aspx.cs` → `LoadPendingInvoice()` | user action on this page | operation is refused | "Unable to Access" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `OrderId`, `UserId`

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
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Database object | `View_CustomerMaster` |
| Database object | `View_InvoiceWiseDue` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblInvoice` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblOrderDetail` |
| Database object | `tblUserCompanyUnit` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SYS-004-001` | Unauthenticated access | Request `/CommonUI/HomePage.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SYS-004-002` | Validation: Generate Invoice | Trigger `LoadPendingInvoice()` in the state that produces this message | alert "Generate Invoice" and the operation is not performed | CONFIRMED |
| `TC-SYS-004-003` | Validation: Unable to Access | Trigger `LoadPendingInvoice()` in the state that produces this message | alert "Unable to Access" and the operation is not performed | CONFIRMED |
| `TC-SYS-004-004` | Grid loads | Open the page with data present | Grid `orderGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/CommonUI/HomePage.aspx` |
| Code-behind | `Solution.Web/CommonUI/HomePage.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 61 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 13 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 10 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` is **shared with 7 other page(s)** (e.g. `AuditReportOne.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- Writes/reads `View_CustomerMaster` — any schema change to it breaks this page.
- Writes/reads `View_InvoiceWiseDue` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblOrderDetail` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
