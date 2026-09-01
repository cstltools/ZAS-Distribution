# DSB-007 — TotalSummary

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `DSB-007` |
| Module | `dashboard-analytics` |
| File name | `TotalSummary.aspx` |
| Route | `/SInventory_UI/TotalSummary.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `TotalSummary.aspx.cs` |
| Code-behind class | `SInventory_UI_TotalSummary` |
| Markup lines | 173 |
| Code-behind lines | 117 |
| Server controls | 23 |
| GridViews | 1 |
| Code-behind methods | 5 |

## 2. Purpose

Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/TotalSummary.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TotalSummary.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TotalSummary.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs` |

## 7. UI Control Inventory

23 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 7 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `fromDateTextBox` | asp:TextBox | From Date |  |  |  |  |  | 46 |
| 5 | `fromDate` | asp:CalendarExtender | From Date |  |  |  |  |  | 47 |
| 6 | `imgDateFrom` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 50 |
| 7 | `toDateTextBox` | asp:TextBox | To Date |  |  |  |  |  | 69 |
| 8 | `toDate` | asp:CalendarExtender | To Date |  |  |  |  |  | 70 |
| 9 | `imgDateTo` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 73 |
| 10 | `viewRptButton` | asp:Button |  | Search |  |  |  | viewRptButton_Click | 94 |
| 11 | `excelButton1` | asp:Button |  | Export to Excel |  |  |  | excelButton1_Click | 101 |
| 12 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 126 |
| 13 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 134 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 135 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 144 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 151 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 156 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 157 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 158 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 159 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 160 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 161 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 162 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Sales Center | ComUnitCode | — | — | — |
| 1 | BoundField | Sales Center Name | ShortName | — | — | — |
| 2 | BoundField | Number of Proforma Invoice | NumberofProformaInvoice | — | — | — |
| 3 | BoundField | Sum of Net Proforma Amount(TP) | SumofNetProformaAmount | — | — | — |
| 4 | BoundField | Proforma Total Vat | ProTpVat | — | — | — |
| 5 | BoundField | Number of Invoices Sold | NumberofInvoiceSold | — | — | — |
| 6 | BoundField | Sum of Net Sales Amount(TP) | SumofNetSalesAmount | — | — | — |
| 7 | BoundField | Sales Total Vat | DelTpVat | — | — | — |
| 8 | BoundField | Number of Returned Invoices | NumberofReturnInvoice | — | — | — |
| 9 | BoundField | Sum of Net Return Amount(TP) | SumofNetReturnAmount | — | — | — |
| 10 | BoundField | Return Total Vat | DelReTpVat | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `viewRptButton_Click` | `aSummaryBll.LoadSummaryBLL(...)` | `TotalSummaryBLL.LoadSummaryBLL(DateTime fromdate, DateTime todate)` <br/>`Library.BLL/SInventory_BLL/TotalSummaryBLL.cs` | `TotalSummaryBLL.LoadSummaryBLL` → `TotalSummaryDAL.LoadSummaryDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `imgDateFrom` | asp:ImageButton |  | — | `—` | — | — | — |
| `imgDateTo` | asp:ImageButton |  | — | `—` | — | — | — |
| `viewRptButton` | asp:Button | Search | — | `viewRptButton_Click` | 64 lines | `aSummaryBll.LoadSummaryBLL` | — |
| `excelButton1` | asp:Button | Export to Excel | — | `excelButton1_Click` | 17 lines | — | — |

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
| 2 | `viewRptButton_Click` | `void viewRptButton_Click(object sender, EventArgs e)` | 64 | `aSummaryBll.LoadSummaryBLL` | — | — | — |
| 3 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 4 | `excelButton1_Click` | `void excelButton1_Click(object sender, EventArgs e)` | 17 | — | — | — | — |
| 5 | `OnPageIndexChanging` | `void OnPageIndexChanging(object sender, GridViewPageEventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
TotalSummary.aspx (markup)
  -> SInventory_UI_TotalSummary (code-behind event handler)
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
| `viewRptButton_Click` | `TotalSummaryBLL` | `LoadSummaryBLL` | `DateTime fromdate, DateTime todate` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyUnit` | `viewRptButton_Click()` → `TotalSummaryBLL.LoadSummaryBLL` → `TotalSummaryDAL.LoadSummaryDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `viewRptButton_Click()` → `TotalSummaryBLL.LoadSummaryBLL` → `TotalSummaryDAL.LoadSummaryDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `viewRptButton_Click()` → `TotalSummaryBLL.LoadSummaryBLL` → `TotalSummaryDAL.LoadSummaryDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `viewRptButton_Click` | Total | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-DSB-007-001` | Total | Code | `Solution.Web/SInventory_UI/TotalSummary.aspx.cs` → `viewRptButton_Click()` | user action on this page | operation is refused | "Total" | CONFIRMED |
| `BR-DSB-007-002` | {0} | Code | `Solution.Web/SInventory_UI/TotalSummary.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs` |
| Database object | `tblCompanyUnit` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-DSB-007-001` | Unauthenticated access | Request `/SInventory_UI/TotalSummary.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-DSB-007-002` | Validation: Total | Trigger `viewRptButton_Click()` in the state that produces this message | alert "Total" and the operation is not performed | CONFIRMED |
| `TC-DSB-007-003` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-DSB-007-004` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TotalSummary.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TotalSummary.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 23 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 5 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 3 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
