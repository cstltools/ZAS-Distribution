# PRC-013 — ProductQuotedPriceApproval

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `PRC-013` |
| Module | `pricing-discount-campaign` |
| File name | `ProductQuotedPriceApproval.aspx` |
| Route | `/SInventory_UI/ProductQuotedPriceApproval.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ProductQuotedPriceApproval.aspx.cs` |
| Code-behind class | `SInventory_UI_ManualOrderApproval` |
| Markup lines | 455 |
| Code-behind lines | 188 |
| Server controls | 30 |
| GridViews | 2 |
| Code-behind methods | 11 |

## 2. Purpose

Approval screen. It lists submitted records awaiting a decision and lets an authorised user approve, reject or cancel them, which advances the record status. Which actions the current user may take is resolved at run time by `Library.DAL/InternalCls/ClsApprovalAction.cs`. Contains 2 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Approval screen. It lists submitted records awaiting a decision and lets an authorised user approve, reject or cancel them, which advances the record status. Which actions the current user may take is resolved at run time by `Library.DAL/InternalCls/ClsApprovalAction.cs`. Contains 2 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/ProductQuotedPriceApproval.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductQuotedPriceApproval.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductQuotedPriceApproval.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |

## 7. UI Control Inventory

30 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 46 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 47 |
| 4 | `actionRadioButtonList` | asp:RadioButtonList | Take Action : |  |  |  |  |  | 83 |
| 5 | `btnSubmit` | asp:Button |  | Submit Action |  |  |  | btnSubmit0_Click | 90 |
| 6 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 116 |
| 7 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 119 |
| 8 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 121 |
| 9 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 122 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 123 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 124 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 125 |
| 13 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 126 |
| 14 | `chkSelectAll` | asp:CheckBox | loadGridView → col[6] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 128 |
| 15 | `chkSelect` | asp:CheckBox | loadGridView → col[6] "(no header)" |  |  |  |  |  | 131 |
| 16 | `ModalPopupExtender10` | ajaxToolkit:ModalPopupExtender |  |  |  |  |  |  | 328 |
| 17 | `HiddenField10` | asp:HiddenField |  |  |  |  |  |  | 331 |
| 18 | `Panel10` | asp:Panel |  |  |  |  |  |  | 332 |
| 19 | `UpdatePanel11` | asp:UpdatePanel |  |  |  |  |  |  | 334 |
| 20 | `dueGridView` | asp:GridView | Due History |  |  |  |  |  | 347 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 350 |
| 22 | `LabelSL` | asp:Label | dueGridView → col[0] "#SL" |  |  |  |  |  | 352 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 355 |
| 24 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 356 |
| 25 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 357 |
| 26 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 358 |
| 27 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 359 |
| 28 | `Button41` | asp:Button |  | Close |  |  |  | Button41_OnClick | 411 |
| 29 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 446 |
| 30 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 450 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `QuotedPriceId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Customer Name | CustomerName | — | — | — |
| 3 | BoundField | Quoted Price | QuotedPrice | — | — | — |
| 4 | BoundField | Active Date | ActiveDate | — | — | — |
| 5 | BoundField | InActive Date | InActiveDate | — | — | — |
| 6 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `dueGridView`

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


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `ManualOrderLoad` | `aProductDiscountBll.LoadApprovalControlBLL(...)` | `ProductDiscountBLL.LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.LoadApprovalControlBLL` | — | — | — |
| `ManualOrderCondition` | `aProductDiscountBll.GetProductQuotedPriceApproval(...)` | `ProductDiscountBLL.GetProductQuotedPriceApproval(string actionstatus)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.GetProductQuotedPriceApproval` → `ProductDiscountDAL.GetProductQuotedPriceApproval` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster`, `tblProduct`, `tblProductQuotedPrice` | — |
| `ManualOrderCondition` | `aProductDiscountBll.LoadForApprovalConditionBLL(...)` | `ProductDiscountBLL.LoadForApprovalConditionBLL(string pageName, string userName)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.LoadForApprovalConditionBLL` | — | — | — |
| `SubmitApproval` | `aProductDiscountBll.QuotedPriceApproval(...)` | `ProductDiscountBLL.QuotedPriceApproval(QuotedPriceSetup aProductDiscount)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.QuotedPriceApproval` → `ProductDiscountDAL.QuotedPriceApprovalDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblProductQuotedPrice` | — |
| `LoadDueHistory` | `aOrderInfoBll.LoadCustomerDueHistory(...)` | `OrderInfoBLL.LoadCustomerDueHistory(string customerMasterId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CustomerMaster`, `View_InvoiceWiseDue`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblInvoice` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `actionRadioButtonList` | Take Action : | asp:RadioButtonList | False | — | `aProductDiscountBll.LoadApprovalControlBLL()` in `ManualOrderLoad()` | UNKNOWN | set in code (see the DAL method above) |

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
| `btnSubmit` | asp:Button | Submit Action | — | `btnSubmit0_Click` | 4 lines | — | — |
| `Button41` | asp:Button | Close | — | `Button41_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 2
- AJAX Control Toolkit extenders: `ajaxToolkit:ModalPopupExtender`(HiddenField10)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | — | — | — | — |
| 2 | `btnSubmit0_Click` | `void btnSubmit0_Click(object sender, EventArgs e)` | 4 | — | — | — | — |
| 3 | `ManualOrderLoad` | `void ManualOrderLoad()` | 7 | `aProductDiscountBll.LoadApprovalControlBLL` | — | — | `LoginName` |
| 4 | `ManualOrderCondition` | `void ManualOrderCondition(string pageName, string user)` | 6 | `aProductDiscountBll.GetProductQuotedPriceApproval`, `aProductDiscountBll.LoadForApprovalConditionBLL` | — | — | — |
| 5 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 6 | `SubmitApproval` | `void SubmitApproval()` | 49 | `aProductDiscountBll.QuotedPriceApproval` | — | — | `LoginName` |
| 7 | `Validation` | `bool Validation()` | 26 | — | — | — | — |
| 8 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 9 | `invoiceButton_Click` | `void invoiceButton_Click(object sender, EventArgs e)` | 15 | — | — | — | — |
| 10 | `LoadDueHistory` | `void LoadDueHistory(string customerMasterId)` | 14 | `aOrderInfoBll.LoadCustomerDueHistory` | — | — | — |
| 11 | `Button41_OnClick` | `void Button41_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ProductQuotedPriceApproval.aspx (markup)
  -> SInventory_UI_ManualOrderApproval (code-behind event handler)
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
| `ManualOrderLoad` | `ProductDiscountBLL` | `LoadApprovalControlBLL` | `RadioButtonList rdl, string pageName, string userName` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `ManualOrderCondition` | `ProductDiscountBLL` | `GetProductQuotedPriceApproval` | `string actionstatus` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `ManualOrderCondition` | `ProductDiscountBLL` | `LoadForApprovalConditionBLL` | `string pageName, string userName` | `string` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `SubmitApproval` | `ProductDiscountBLL` | `QuotedPriceApproval` | `QuotedPriceSetup aProductDiscount` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `LoadDueHistory` | `OrderInfoBLL` | `LoadCustomerDueHistory` | `string customerMasterId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_CustomerMaster` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `View_InvoiceWiseDue` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable`, `ManualOrderCondition()` → `ProductDiscountBLL.GetProductQuotedPriceApproval` → `ProductDiscountDAL.GetProductQuotedPriceApproval` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `LoadDueHistory()` → `OrderInfoBLL.LoadCustomerDueHistory` → `OrderInfoDAL.GetCustomerDueHistory` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProduct` | `ManualOrderCondition()` → `ProductDiscountBLL.GetProductQuotedPriceApproval` → `ProductDiscountDAL.GetProductQuotedPriceApproval` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductQuotedPrice` | `ManualOrderCondition()` → `ProductDiscountBLL.GetProductQuotedPriceApproval` → `ProductDiscountDAL.GetProductQuotedPriceApproval` → `ClsCommonInternalDAL.DataContainerDataTable`, `SubmitApproval()` → `ProductDiscountBLL.QuotedPriceApproval` → `ProductDiscountDAL.QuotedPriceApprovalDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |

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
| `BR-PRC-013-001` | {0} | Code | `Solution.Web/SInventory_UI/ProductQuotedPriceApproval.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`

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
| Assembly source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Database object | `View_CustomerMaster` |
| Database object | `View_InvoiceWiseDue` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblInvoice` |
| Database object | `tblProduct` |
| Database object | `tblProductQuotedPrice` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-PRC-013-001` | Unauthenticated access | Request `/SInventory_UI/ProductQuotedPriceApproval.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-PRC-013-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-PRC-013-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductQuotedPriceApproval.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductQuotedPriceApproval.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 30 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 11 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 8 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` is **shared with 14 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DiscountApproval.aspx`, `DistributionCenterInfoView.aspx`, `ManualOrderrCreation.aspx`) — a change there is not local.
- Writes/reads `View_CustomerMaster` — any schema change to it breaks this page.
- Writes/reads `View_InvoiceWiseDue` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductQuotedPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
