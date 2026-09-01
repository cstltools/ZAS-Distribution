# RPT-007 — DelivaryReturnHTMLReport

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RPT-007` |
| Module | `reports` |
| File name | `DelivaryReturnHTMLReport.aspx` |
| Route | `/HTMLReport/DelivaryReturnHTMLReport.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `DelivaryReturnHTMLReport.aspx.cs` |
| Code-behind class | `HTMLReport_DelivaryReturnHTMLReport` |
| Markup lines | 66 |
| Code-behind lines | 92 |
| Server controls | 23 |
| GridViews | 1 |
| Code-behind methods | 3 |

## 2. Purpose

HTML report screen. It runs a parameterised query and renders the result as an HTML table (printable / exportable) rather than through Crystal Reports. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: HTML report screen. It runs a parameterised query and renders the result as an HTML table (printable / exportable) rather than through Crystal Reports. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/DeliveryReturnReport.aspx`

## 5. Route

`/HTMLReport/DelivaryReturnHTMLReport.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `NationalReport`, `districtId`, `fromDate`, `toDate`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/HTMLReport/DelivaryReturnHTMLReport.aspx` |
| Code-behind | `Solution.Web/HTMLReport/DelivaryReturnHTMLReport.aspx.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` |

## 7. UI Control Inventory

23 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ImageButton1` | asp:ImageButton | Delivary Return Report | ../HTML_Report_css/images/excel.png |  |  |  | ImageButton1_Click | 30 |
| 2 | `loadGridView` | asp:GridView | Import Excel |  |  |  |  |  | 34 |
| 3 | `(no id)` | asp:BoundField | Import Excel |  |  |  |  |  | 36 |
| 4 | `(no id)` | asp:BoundField | Import Excel |  |  |  |  |  | 37 |
| 5 | `(no id)` | asp:BoundField | Import Excel |  |  |  |  |  | 38 |
| 6 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 39 |
| 7 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 40 |
| 8 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 41 |
| 9 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 42 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 43 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 44 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 45 |
| 13 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 47 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 49 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 50 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 51 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 52 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 54 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 55 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 56 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 57 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 59 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 60 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `responstable` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | DC Code | ComUnitCode | — | — | — |
| 1 | BoundField | DC Name | ComUnitName | — | — | — |
| 2 | BoundField | Product Code | ProductCode | — | — | — |
| 3 | BoundField | Product Name | ProductName | — | — | — |
| 4 | BoundField | Pack Size | PackSize | — | — | — |
| 5 | BoundField | Batch No | BatchNo | — | — | — |
| 6 | BoundField | Exp Date | ExpDate | — | — | — |
| 7 | BoundField | Customer Code | CustomerCode | — | — | — |
| 8 | BoundField | Customer Name | CustomerName | — | — | — |
| 9 | BoundField | Order No | OrderNo | — | — | — |
| 10 | BoundField | Invoice Date | InvoiceDate | — | — | — |
| 11 | BoundField | Dlv Inv No | DelivaryInvoiceNo | — | — | — |
| 12 | BoundField | Quantity | Quantity | — | — | — |
| 13 | BoundField | Vat Amount | VatAmount | — | — | — |
| 14 | BoundField | Amount | Amount | — | — | — |
| 15 | BoundField | Region Code | RegionCode | — | — | — |
| 16 | BoundField | Area Code | AreaCode | — | — | — |
| 17 | BoundField | Territory Code | TerritoryCode | — | — | — |
| 18 | BoundField | Mia Code | MiaCode | — | — | — |
| 19 | BoundField | Market Code | MarketCode | — | — | — |
| 20 | BoundField | Return Reason | ReturnReason | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aCompanySalesReportBll.DeliveryReturnReportDAl(...)` | `CompanySalesReportBLL.DeliveryReturnReportDAl(string districtId, DateTime fromDate, DateTime toDate)` <br/>`Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` | `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CustomerMaster`, `tblCompanyUnit`, `tblDCStore`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIOInfo` | — |
| `ImageButton1_Click` | `aCompanySalesReportBll.DeliveryReturnReportDAl(...)` | `CompanySalesReportBLL.DeliveryReturnReportDAl(string districtId, DateTime fromDate, DateTime toDate)` <br/>`Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` | `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CustomerMaster`, `tblCompanyUnit`, `tblDCStore`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIOInfo` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `ImageButton1` | asp:ImageButton |  | — | `ImageButton1_Click` | 41 lines | `aCompanySalesReportBll.DeliveryReturnReportDAl` | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 0
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 27 | `aCompanySalesReportBll.DeliveryReturnReportDAl` | — | — | — |
| 2 | `ImageButton1_Click` | `void ImageButton1_Click(object sender, ImageClickEventArgs e)` | 41 | `aCompanySalesReportBll.DeliveryReturnReportDAl` | — | — | — |
| 3 | `ReportPath` | `string ReportPath(string rptName)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
DelivaryReturnHTMLReport.aspx (markup)
  -> HTMLReport_DelivaryReturnHTMLReport (code-behind event handler)
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
| `Page_Load` | `CompanySalesReportBLL` | `DeliveryReturnReportDAl` | `string districtId, DateTime fromDate, DateTime toDate` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` |
| `ImageButton1_Click` | `CompanySalesReportBLL` | `DeliveryReturnReportDAl` | `string districtId, DateTime fromDate, DateTime toDate` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_CustomerMaster` | `ImageButton1_Click()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `ImageButton1_Click()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStore` | `ImageButton1_Click()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblEmpGeneralInfo` | `ImageButton1_Click()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `ImageButton1_Click()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoiceDetail` | `ImageButton1_Click()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `ImageButton1_Click()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Load()` → `CompanySalesReportBLL.DeliveryReturnReportDAl` → `CompanySalesReportDAL.DeliveryReturnReportDAl` → `ClsCommonInternalDAL.DataContainerDataTable` |

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
| Report definition(s) loaded | `crpDeliveryReturnReport.rpt` |
| Query-string parameters | `NationalReport`, `districtId`, `fromDate`, `toDate` |
| Session parameters | — |
| Data layer | `CompanySalesReportBLL`, `InvoiceBLL`, `StockTransportOrderReportBLL`, `aCompanySalesReportBll`, `aInvoiceBll`, `aOrderReportBll` |

`.rpt` files are binary, so the report's own field layout, formulas, grouping, sorting and suppression rules are **UNKNOWN** — see `specs/SPEC-GAPS.md` → `GAP-RPT-001`. The full mapping of every viewer to its report is in `specs/reports/REPORT-INVENTORY.md`.

This viewer does **not** use the master page, so it performs no authentication check (`GAP-SEC-002`). Its parameters arrive from the query string and/or `Session`, and are not validated.


## 26. Dependencies

| Kind | Dependency |
|---|---|
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` |
| Database object | `View_CustomerMaster` |
| Database object | `tblCompanyUnit` |
| Database object | `tblDCStore` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblMIOInfo` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RPT-007-001` | Unauthenticated access | Request `/HTMLReport/DelivaryReturnHTMLReport.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-RPT-007-002` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/HTMLReport/DelivaryReturnHTMLReport.aspx` |
| Code-behind | `Solution.Web/HTMLReport/DelivaryReturnHTMLReport.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 23 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 3 methods traced | n/a | **MISSING → now documented** | section 12 |
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

- `Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` is **shared with 4 other page(s)** (e.g. `AllSalesreportHTMLReport.aspx`, `AllSalesreportViewer.aspx`, `CustomerPaymentViewer.aspx`, `DeliveryReturnReportViewer.aspx`) — a change there is not local.
- Writes/reads `View_CustomerMaster` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
