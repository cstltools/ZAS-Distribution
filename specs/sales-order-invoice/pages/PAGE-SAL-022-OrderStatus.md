# SAL-022 — OrderStatus

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-022` |
| Module | `sales-order-invoice` |
| File name | `OrderStatus.aspx` |
| Route | `/SInventory_UI/OrderStatus.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `OrderStatus.aspx.cs` |
| Code-behind class | `SInventory_UI_OrderStatus` |
| Markup lines | 735 |
| Code-behind lines | 235 |
| Server controls | 54 |
| GridViews | 3 |
| Code-behind methods | 10 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 3 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 3 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/OrderStatus.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/OrderStatus.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/OrderStatus.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |

## 7. UI Control Inventory

54 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `orderNoTextBox` | asp:TextBox | Order No: |  |  |  |  |  | 40 |
| 5 | `searchButton` | asp:Button |  | Search |  |  |  | searchButton_OnClick | 77 |
| 6 | `salesCenterCodeTextBox` | asp:Label | Sales Center Code: |  |  |  |  |  | 135 |
| 7 | `mioCodeTextBox` | asp:Label | MIO Code: |  |  |  |  |  | 141 |
| 8 | `salesCenterNameTextBox` | asp:Label | Sales Center Name: |  |  |  |  |  | 155 |
| 9 | `mioNameTextBox` | asp:Label | MIO Name: |  |  |  |  |  | 161 |
| 10 | `customerCodeTextBox` | asp:Label | Customer Code: |  |  |  |  |  | 174 |
| 11 | `grossValueTextBox` | asp:Label | Gross Value: |  |  |  |  |  | 180 |
| 12 | `CustomerNameTextBox` | asp:Label | Customer Name: |  |  |  |  |  | 192 |
| 13 | `submissionDateTextBox` | asp:Label | Submission Date: |  |  |  |  |  | 198 |
| 14 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 225 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 228 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 229 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 230 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 231 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 232 |
| 20 | `proformaMessageLabel` | asp:Label |  |  |  |  |  |  | 291 |
| 21 | `invoiceNumberLabel` | asp:Label | Invoice Number: |  |  |  |  |  | 312 |
| 22 | `invoiceDateLabel` | asp:Label | Invoice Date: |  |  |  |  |  | 318 |
| 23 | `tpTotalLabel` | asp:Label | TP Total: |  |  |  |  |  | 332 |
| 24 | `tpDiscountLabel` | asp:Label | TP Discount: |  |  |  |  |  | 338 |
| 25 | `tpVatLabel` | asp:Label | TP VAT: |  |  |  |  |  | 351 |
| 26 | `tpGrandTotalLabel` | asp:Label | TP Grand Total: |  |  |  |  |  | 357 |
| 27 | `proformaGridView` | asp:GridView |  |  |  |  |  |  | 388 |
| 28 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 391 |
| 29 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 392 |
| 30 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 393 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 394 |
| 32 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 395 |
| 33 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 396 |
| 34 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 397 |
| 35 | `deliveryInvoiceMsgLabel` | asp:Label |  |  |  |  |  |  | 457 |
| 36 | `deliveryInvoiceNumberLabel` | asp:Label | Delivery Invoice Number: |  |  |  |  |  | 479 |
| 37 | `deliveryTPTotalLabel` | asp:Label | Delivery TP Total: |  |  |  |  |  | 485 |
| 38 | `deliveryTPDiscountLabel` | asp:Label | Delivery TP Discount: |  |  |  |  |  | 499 |
| 39 | `deliveryTPVATLabel` | asp:Label | Delivery TP VAT: |  |  |  |  |  | 505 |
| 40 | `deliveryTPGrandTotalLabel` | asp:Label | Delivery TP Grand Total: |  |  |  |  |  | 518 |
| 41 | `deliveryStatusLabel` | asp:Label | Delivery Status: |  |  |  |  |  | 524 |
| 42 | `deliveryGridView` | asp:GridView |  |  |  |  |  |  | 555 |
| 43 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 558 |
| 44 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 559 |
| 45 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 560 |
| 46 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 561 |
| 47 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 562 |
| 48 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 563 |
| 49 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 564 |
| 50 | `paymentMsgLabel` | asp:Label |  |  |  |  |  |  | 624 |
| 51 | `paymentAmountLabel` | asp:Label | Payment Amount: |  |  |  |  |  | 644 |
| 52 | `paymentStatusLabel` | asp:Label | Payment Status: |  |  |  |  |  | 650 |
| 53 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 725 |
| 54 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 728 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `OrderDetailId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Quantity | Quantity | — | — | — |
| 3 | BoundField | Trade Price | TradePrice | — | — | — |
| 4 | BoundField | Total Trade Price | TotalTradePrice | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `proformaGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceDetailId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Batch No | BatchNo | — | — | — |
| 3 | BoundField | Total Price | TotalPrice | — | — | — |
| 4 | BoundField | Total Price Vat Amount | TotalPriceVatAmount | — | — | — |
| 5 | BoundField | Discount Percentage | DiscountPercentage | — | — | — |
| 6 | BoundField | Net Amount | NetAmount | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `deliveryGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceDetailId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Batch No | BatchNo | — | — | — |
| 3 | BoundField | Total Price | DeliveryTotalPrice | — | — | — |
| 4 | BoundField | Total Price Vat Amount | DeliveryTotalPriceVatAmount | — | — | — |
| 5 | BoundField | Discount Percentage | DeliveryDiscountPercentage | — | — | — |
| 6 | BoundField | Net Amount | DeliveryNetAmount | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `searchButton_OnClick` | `aOrderStatusBll.LoadOrderStatusInfo(...)` | `OrderStatusBll.LoadOrderStatusInfo(string orderNumber)` <br/>`Library.BLL/SInventory_BLL/OrderStatusBll.cs` | `OrderStatusBll.LoadOrderStatusInfo` → `OrderStatusDal.GetOrderStatusInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblOrder`, `tblOrderDetail` | — |
| `PaymentStatusOperation` | `aOrderStatusBll.CheckPaymentStatus(...)` | `OrderStatusBll.CheckPaymentStatus(string orderNo)` <br/>`Library.BLL/SInventory_BLL/OrderStatusBll.cs` | `OrderStatusBll.CheckPaymentStatus` → `OrderStatusDal.GetCheckPaymentStatus` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice` | — |
| `DeliveryInvoiceOperation` | `aOrderStatusBll.CheckDeliveryInvoiceExistOrNot(...)` | `OrderStatusBll.CheckDeliveryInvoiceExistOrNot(string orderNo)` <br/>`Library.BLL/SInventory_BLL/OrderStatusBll.cs` | `OrderStatusBll.CheckDeliveryInvoiceExistOrNot` → `OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice`, `tblInvoiceDetail` | — |
| `DeliveryInvoiceOperation` | `aOrderStatusBll.LoadDeliveryInvoiceInfo(...)` | `OrderStatusBll.LoadDeliveryInvoiceInfo(string orderNo)` <br/>`Library.BLL/SInventory_BLL/OrderStatusBll.cs` | `OrderStatusBll.LoadDeliveryInvoiceInfo` → `OrderStatusDal.GetDeliveryInvoiceInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice`, `tblInvoiceDetail` | — |
| `ProformaInvoiceOperation` | `aOrderStatusBll.CheckInvoiceExistOrNot(...)` | `OrderStatusBll.CheckInvoiceExistOrNot(string orderNo)` <br/>`Library.BLL/SInventory_BLL/OrderStatusBll.cs` | `OrderStatusBll.CheckInvoiceExistOrNot` → `OrderStatusDal.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice` | — |
| `ProformaInvoiceOperation` | `aOrderStatusBll.LoadProformaInvoiceInfo(...)` | `OrderStatusBll.LoadProformaInvoiceInfo(string orderNo)` <br/>`Library.BLL/SInventory_BLL/OrderStatusBll.cs` | `OrderStatusBll.LoadProformaInvoiceInfo` → `OrderStatusDal.GetProformaInvoiceInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblInvoice`, `tblInvoiceDetail` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `searchButton` | asp:Button | Search | — | `searchButton_OnClick` | 34 lines | `aOrderStatusBll.LoadOrderStatusInfo` | — |

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
| 2 | `searchButton_OnClick` | `void searchButton_OnClick(object sender, EventArgs e)` | 34 | `aOrderStatusBll.LoadOrderStatusInfo` | — | — | — |
| 3 | `PaymentStatusOperation` | `void PaymentStatusOperation(string orderNo)` | 16 | `aOrderStatusBll.CheckPaymentStatus` | — | — | — |
| 4 | `PaymentStatusOperationClear` | `void PaymentStatusOperationClear()` | 4 | — | — | — | — |
| 5 | `DeliveryInvoiceOperation` | `void DeliveryInvoiceOperation(string orderNo)` | 32 | `aOrderStatusBll.CheckDeliveryInvoiceExistOrNot`, `aOrderStatusBll.LoadDeliveryInvoiceInfo` | — | — | — |
| 6 | `DeliveryInvoiceOperationClear` | `void DeliveryInvoiceOperationClear()` | 11 | — | — | — | — |
| 7 | `ProformaInvoiceOperation` | `void ProformaInvoiceOperation(string orderNo)` | 34 | `aOrderStatusBll.CheckInvoiceExistOrNot`, `aOrderStatusBll.LoadProformaInvoiceInfo` | — | — | — |
| 8 | `ProformaInvoiceOperationClear` | `void ProformaInvoiceOperationClear()` | 11 | — | — | — | — |
| 9 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 10 | `Clear` | `void Clear()` | 45 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
OrderStatus.aspx (markup)
  -> SInventory_UI_OrderStatus (code-behind event handler)
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
| `searchButton_OnClick` | `OrderStatusBll` | `LoadOrderStatusInfo` | `string orderNumber` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |
| `PaymentStatusOperation` | `OrderStatusBll` | `CheckPaymentStatus` | `string orderNo` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |
| `DeliveryInvoiceOperation` | `OrderStatusBll` | `CheckDeliveryInvoiceExistOrNot` | `string orderNo` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |
| `DeliveryInvoiceOperation` | `OrderStatusBll` | `LoadDeliveryInvoiceInfo` | `string orderNo` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |
| `ProformaInvoiceOperation` | `OrderStatusBll` | `CheckInvoiceExistOrNot` | `string orderNo` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |
| `ProformaInvoiceOperation` | `OrderStatusBll` | `LoadProformaInvoiceInfo` | `string orderNo` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblInvoice` | `DeliveryInvoiceOperation()` → `OrderStatusBll.CheckDeliveryInvoiceExistOrNot` → `OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `DeliveryInvoiceOperation()` → `OrderStatusBll.LoadDeliveryInvoiceInfo` → `OrderStatusDal.GetDeliveryInvoiceInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `PaymentStatusOperation()` → `OrderStatusBll.CheckPaymentStatus` → `OrderStatusDal.GetCheckPaymentStatus` → `ClsCommonInternalDAL.DataContainerDataTable`, `ProformaInvoiceOperation()` → `OrderStatusBll.CheckInvoiceExistOrNot` → `OrderStatusDal.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `ProformaInvoiceOperation()` → `OrderStatusBll.LoadProformaInvoiceInfo` → `OrderStatusDal.GetProformaInvoiceInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `DeliveryInvoiceOperation()` → `OrderStatusBll.CheckDeliveryInvoiceExistOrNot` → `OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `DeliveryInvoiceOperation()` → `OrderStatusBll.LoadDeliveryInvoiceInfo` → `OrderStatusDal.GetDeliveryInvoiceInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `ProformaInvoiceOperation()` → `OrderStatusBll.LoadProformaInvoiceInfo` → `OrderStatusDal.GetProformaInvoiceInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `searchButton_OnClick()` → `OrderStatusBll.LoadOrderStatusInfo` → `OrderStatusDal.GetOrderStatusInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrderDetail` | `searchButton_OnClick()` → `OrderStatusBll.LoadOrderStatusInfo` → `OrderStatusDal.GetOrderStatusInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `PaymentStatusOperation` | No Payment Information found !!! | No | Yes | UNKNOWN | Business rule |
| `DeliveryInvoiceOperation` | No Delivery Invoice found !!! | No | Yes | UNKNOWN | Business rule |
| `ProformaInvoiceOperation` | No Proforma Invoice found!!! | No | Yes | UNKNOWN | Business rule |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-022-001` | No Payment Information found !!! | Code | `Solution.Web/SInventory_UI/OrderStatus.aspx.cs` → `PaymentStatusOperation()` | user action on this page | operation is refused | "No Payment Information found !!!" | CONFIRMED |
| `BR-SAL-022-002` | No Delivery Invoice found !!! | Code | `Solution.Web/SInventory_UI/OrderStatus.aspx.cs` → `DeliveryInvoiceOperation()` | user action on this page | operation is refused | "No Delivery Invoice found !!!" | CONFIRMED |
| `BR-SAL-022-003` | No Proforma Invoice found!!! | Code | `Solution.Web/SInventory_UI/OrderStatus.aspx.cs` → `ProformaInvoiceOperation()` | user action on this page | operation is refused | "No Proforma Invoice found!!!" | CONFIRMED |
| `BR-SAL-022-004` | {0} | Code | `Solution.Web/SInventory_UI/OrderStatus.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

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
| Assembly source | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblOrder` |
| Database object | `tblOrderDetail` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-022-001` | Unauthenticated access | Request `/SInventory_UI/OrderStatus.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-022-002` | Validation: No Payment Information found !!! | Trigger `PaymentStatusOperation()` in the state that produces this message | alert "No Payment Information found !!!" and the operation is not performed | CONFIRMED |
| `TC-SAL-022-003` | Validation: No Delivery Invoice found !!! | Trigger `DeliveryInvoiceOperation()` in the state that produces this message | alert "No Delivery Invoice found !!!" and the operation is not performed | CONFIRMED |
| `TC-SAL-022-004` | Validation: No Proforma Invoice found!!! | Trigger `ProformaInvoiceOperation()` in the state that produces this message | alert "No Proforma Invoice found!!!" and the operation is not performed | CONFIRMED |
| `TC-SAL-022-005` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-022-006` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/OrderStatus.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/OrderStatus.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 54 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 10 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 4 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblOrderDetail` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
