# RPT-015 — SalesTrackingHTMLReport

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RPT-015` |
| Module | `reports` |
| File name | `SalesTrackingHTMLReport.aspx` |
| Route | `/HTMLReport/SalesTrackingHTMLReport.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `SalesTrackingHTMLReport.aspx.cs` |
| Code-behind class | `HTMLReport_ProformaHTMLReport` |
| Markup lines | 140 |
| Code-behind lines | 93 |
| Server controls | 31 |
| GridViews | 2 |
| Code-behind methods | 4 |

## 2. Purpose

HTML report screen. It runs a parameterised query and renders the result as an HTML table (printable / exportable) rather than through Crystal Reports. Contains 2 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: HTML report screen. It runs a parameterised query and renders the result as an HTML table (printable / exportable) rather than through Crystal Reports. Contains 2 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/SalesTrackingReport.aspx`

## 5. Route

`/HTMLReport/SalesTrackingHTMLReport.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/HTMLReport/SalesTrackingHTMLReport.aspx` |
| Code-behind | `Solution.Web/HTMLReport/SalesTrackingHTMLReport.aspx.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` |

## 7. UI Control Inventory

31 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `FromTextBox` | asp:Label | From: |  |  | ReadOnly=True |  |  | 53 |
| 2 | `toTextBox` | asp:Label | To: |  |  | ReadOnly=True |  |  | 60 |
| 3 | `dcTextBox` | asp:Label |  |  |  | ReadOnly=True |  |  | 67 |
| 4 | `ImageButton1` | asp:ImageButton |  | ../HTML_Report_css/images/excel.png |  |  |  | ImageButton1_Click | 74 |
| 5 | `loadGridView` | asp:GridView | Import Excel |  |  |  |  |  | 80 |
| 6 | `(no id)` | asp:BoundField | Import Excel |  |  |  |  |  | 82 |
| 7 | `(no id)` | asp:BoundField | Import Excel |  |  |  |  |  | 83 |
| 8 | `(no id)` | asp:BoundField | Import Excel |  |  |  |  |  | 84 |
| 9 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 85 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 86 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 87 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 88 |
| 13 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 89 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 90 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 91 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 92 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 93 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 94 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 95 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 96 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 97 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 98 |
| 23 | `GridView2` | asp:GridView |  |  |  |  |  |  | 101 |
| 24 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 103 |
| 25 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 104 |
| 26 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 107 |
| 27 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 108 |
| 28 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 109 |
| 29 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 111 |
| 30 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 112 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 114 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `responstable` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Company Name | CompanyName | — | — | — |
| 1 | BoundField | Unit Name | ComUnitName | — | — | — |
| 2 | BoundField | Customer Code | CustomerCode | — | — | — |
| 3 | BoundField | Customer Name | CustomerName | — | — | — |
| 4 | BoundField | MIO Name | MIO | — | — | — |
| 5 | BoundField | Invoice No | InvoiceNo | — | — | — |
| 6 | BoundField | Invoice Date | InvoiceDate | — | — | — |
| 7 | BoundField | Invoice Value | InvoiceValue | — | — | — |
| 8 | BoundField | Actual Sales | ActualSales | — | — | — |
| 9 | BoundField | Discount | Discount | — | — | — |
| 10 | BoundField | In-Transit | InTransit | — | — | — |
| 11 | BoundField | ReturnValue | ReturnValue | — | — | — |
| 12 | BoundField | Payment Amount | PaymentAmount | — | — | — |
| 13 | BoundField | AIT | AIT | — | — | — |
| 14 | BoundField | Discount On Payment | DOP | — | — | — |
| 15 | BoundField | Due | Due | — | — | — |
| 16 | BoundField | Prevous Collection | PrevousCollection | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `GridView2`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `responstable` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Description | ProductName | — | — | — |
| 2 | BoundField | Pack Size | PackSize | — | — | — |
| 3 | BoundField | Available Qty | AQty | — | — | — |
| 4 | BoundField | Book For Delv | BookFDel | — | — | — |
| 5 | BoundField | Transit Qty | Tqty | — | — | — |
| 6 | BoundField | Restricted Qty | RQty | — | — | — |
| 7 | BoundField | Blocked Qty | BQty | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Init` | `aReportDal.GetSalesTrackingReport(...)` | `NationalSalesProductWiseDal.GetSalesTrackingReport(string parameter)` <br/>`Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` | `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIOInfo` | — |
| `ImageButton1_Click` | `aReportDal.GetSalesTrackingReport(...)` | `NationalSalesProductWiseDal.GetSalesTrackingReport(string parameter)` <br/>`Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` | `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIOInfo` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `ImageButton1` | asp:ImageButton |  | — | `ImageButton1_Click` | 15 lines | `aReportDal.GetSalesTrackingReport` | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 0
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Init` | `void Page_Init(object sender, EventArgs e)` | 31 | `aReportDal.GetSalesTrackingReport` | — | — | `PRAM` |
| 2 | `ReportPath` | `string ReportPath(string rptName)` | 3 | — | — | — | — |
| 3 | `closeButton_Click` | `void closeButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 4 | `ImageButton1_Click` | `void ImageButton1_Click(object sender, ImageClickEventArgs e)` | 15 | `aReportDal.GetSalesTrackingReport` | — | — | `PRAM` |

Call chain shape for every persisting action on this page:

```
SalesTrackingHTMLReport.aspx (markup)
  -> HTMLReport_ProformaHTMLReport (code-behind event handler)
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

_No BLL method resolved for this page._

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Init` | `NationalSalesProductWiseDal` | `GetSalesTrackingReport` | `string parameter` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` |
| `ImageButton1_Click` | `NationalSalesProductWiseDal` | `GetSalesTrackingReport` | `string parameter` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `ImageButton1_Click()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `ImageButton1_Click()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `ImageButton1_Click()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `ImageButton1_Click()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `ImageButton1_Click()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `ImageButton1_Click()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `ImageButton1_Click()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `NationalSalesProductWiseDal.GetSalesTrackingReport` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

_No validation logic detected on this page._

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

_No message-bearing rule found in this page's own code._

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `PRAM`

Authentication check: performed by the master page (`MainMasterPage.master.cs` `Page_Load` redirects to `../Login.aspx` when `Session["UserId"]` is null). **This page does not use the master page — it therefore performs no session check unless its own code-behind does. See `specs/SPEC-GAPS.md` (`GAP-SEC-002`).**

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

- Excel file import/export.

See `specs/00-system/integrations.md` for the full list.

## 25. Reports

**This page is a report host.**

| Aspect | Value |
|---|---|
| Report definition(s) loaded | `crpSalesTrackingReport.rpt`, `rptAreaWiseMonthlyInventoryReport.rpt` |
| Query-string parameters | — |
| Session parameters | `PRAM` |
| Data layer | `NationalSalesProductWiseDal`, `aReportDal` |

`.rpt` files are binary, so the report's own field layout, formulas, grouping, sorting and suppression rules are **UNKNOWN** — see `specs/SPEC-GAPS.md` → `GAP-RPT-001`. The full mapping of every viewer to its report is in `specs/reports/REPORT-INVENTORY.md`.

This viewer does **not** use the master page, so it performs no authentication check (`GAP-SEC-002`). Its parameters arrive from the query string and/or `Session`, and are not validated.


## 26. Dependencies

| Kind | Dependency |
|---|---|
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblMIOInfo` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RPT-015-001` | Unauthenticated access | Request `/HTMLReport/SalesTrackingHTMLReport.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-RPT-015-002` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/HTMLReport/SalesTrackingHTMLReport.aspx` |
| Code-behind | `Solution.Web/HTMLReport/SalesTrackingHTMLReport.aspx.cs` |
| Layer source | `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 31 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 4 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 7 names found | DDL **not in repo** | **PARTIAL** | section 17 |
| Business rules | absent | messages extracted | rules inside SPs unknown | **PARTIAL** | section 19 |
| Permissions | absent | page-level grant only | grant rows **not in repo** | **UNKNOWN** | section 20 |
| Status lifecycle | absent | tokens extracted | transitions inside SPs unknown | **PARTIAL** | section 21 |

_"Prior spec" is `absent` for every row: the repository contained no `specs/`, `docs/`, `.claude/` or `database/` directory before this rebuild — verified at the repository root._

## 30. Known Gaps

- Stored-procedure bodies are not in the repository → business rules, transactions and result sets executed server-side are UNKNOWN (`GAP-DB-001`).
- No DDL → column names, types, keys, indexes, FKs, triggers UNKNOWN (`GAP-DB-002`).
- Menu path and permission grants live in `tblMainMenu`/`tblMenuDistribution` data → navigation and role matrix UNKNOWN (`GAP-SEC-001`).
- This page does not inherit the master page, so it has **no authentication guard** (`GAP-SEC-002`).

## 31. Change Impact

Changing this page affects:

- `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` is **shared with 1 other page(s)** (e.g. `ProductWiseNationalSalesHTMLReport.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
