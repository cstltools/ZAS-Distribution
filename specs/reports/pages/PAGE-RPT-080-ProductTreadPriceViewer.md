# RPT-080 — ProductTreadPriceViewer

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RPT-080` |
| Module | `reports` |
| File name | `ProductTreadPriceViewer.aspx` |
| Route | `/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `ProductTreadPriceViewer.aspx.cs` |
| Code-behind class | `SInventory_RPTVIEW_ProductTradePriceViewer` |
| Markup lines | 114 |
| Code-behind lines | 129 |
| Server controls | 4 |
| GridViews | 0 |
| Code-behind methods | 5 |

## 2. Purpose

Crystal Report viewer host. It receives report parameters (query string or Session), loads the matching `.rpt` definition from `Solution.Web/Reports/CrystalReports`, binds the result set and renders it in a `CrystalReportViewer` with print and export.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Crystal Report viewer host. It receives report parameters (query string or Session), loads the matching `.rpt` definition from `Solution.Web/Reports/CrystalReports`, binds the result set and renders it in a `CrystalReportViewer` with print and export.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/ProductPriceReport.aspx`

## 5. Route

`/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `reportType`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` |
| Code-behind | `Solution.Web/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WHStockInReportBll.cs` |

## 7. UI Control Inventory

4 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ScriptManager1` | asp:ScriptManager | Edit |  |  |  |  |  | 19 |
| 2 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 21 |
| 3 | `closeButton` | asp:Button |  | Close |  |  |  | closeButton_Click | 39 |
| 4 | `MessageLabel` | asp:Label | sp; |  |  |  |  |  | 59 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Init` | `aOrderReportBll.CompanyInfoBLL(...)` | `StockTransportOrderReportBLL.CompanyInfoBLL()` <br/>`Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs` | `StockTransportOrderReportBLL.CompanyInfoBLL` → `StockTransportOrderReportDAL.CompanyInformation` → `ClsCommonInternalDAL.DataContainerDataTable` → `WHStockInReportBll.CompanyInfoBLL` → `WHStockInReportDal.CompanyInformation` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo` | — |
| `Page_Init` | `aUnitPriceBll.LoadProductPriceReportInfo(...)` | `UnitPriceBLL.LoadProductPriceReportInfo()` <br/>`Library.BLL/SInventory_BLL/UnitPriceBLL.cs` | `UnitPriceBLL.LoadProductPriceReportInfo` → `ProductUnitPriceDAL.GetProductPriceReportInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblUnitPrice` | — |
| `Page_Init` | `aUnitPriceBll.LoadProductUnitPrice(...)` | `UnitPriceBLL.LoadProductUnitPrice()` <br/>`Library.BLL/SInventory_BLL/UnitPriceBLL.cs` | `UnitPriceBLL.LoadProductUnitPrice` → `ProductUnitPriceDAL.LoadUnitPriceView` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblPackSize`, `tblProduct`, `tblUnitPrice` | — |
| `Page_Init` | `aUnitPriceBll.MaterialMasterReport(...)` | `UnitPriceBLL.MaterialMasterReport(string parameter)` <br/>`Library.BLL/SInventory_BLL/UnitPriceBLL.cs` | `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblProCategory`, `tblProType`, `tblProduct`, `tblProductCase`, `tblProductSQ`, `tblStockUOM` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `closeButton` | asp:Button | Close | — | `closeButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Init` | `void Page_Init(object sender, EventArgs e)` | 77 | `aOrderReportBll.CompanyInfoBLL`, `aUnitPriceBll.LoadProductPriceReportInfo`, `aUnitPriceBll.LoadProductUnitPrice`, `aUnitPriceBll.MaterialMasterReport` | — | — | `PRAM` |
| 2 | `ReportPath` | `string ReportPath(string rptName)` | 3 | — | — | — | — |
| 3 | `closeButton_Click` | `void closeButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 4 | `crvSalesRpt_Unload` | `void crvSalesRpt_Unload(object sender, EventArgs e)` | 8 | — | — | — | — |
| 5 | `crvSalesRpt_Disposed` | `void crvSalesRpt_Disposed(object sender, EventArgs e)` | 8 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ProductTreadPriceViewer.aspx (markup)
  -> SInventory_RPTVIEW_ProductTradePriceViewer (code-behind event handler)
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
| `Page_Init` | `StockTransportOrderReportBLL` | `CompanyInfoBLL` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs` |
| `Page_Init` | `WHStockInReportBll` | `CompanyInfoBLL` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/WHStockInReportBll.cs` |
| `Page_Init` | `UnitPriceBLL` | `LoadProductPriceReportInfo` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| `Page_Init` | `UnitPriceBLL` | `LoadProductUnitPrice` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| `Page_Init` | `UnitPriceBLL` | `MaterialMasterReport` | `string parameter` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `Page_Init()` → `StockTransportOrderReportBLL.CompanyInfoBLL` → `StockTransportOrderReportDAL.CompanyInformation` → `ClsCommonInternalDAL.DataContainerDataTable` → `WHStockInReportBll.CompanyInfoBLL`, `Page_Init()` → `UnitPriceBLL.LoadProductUnitPrice` → `ProductUnitPriceDAL.LoadUnitPriceView` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPackSize` | `Page_Init()` → `UnitPriceBLL.LoadProductUnitPrice` → `ProductUnitPriceDAL.LoadUnitPriceView` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProCategory` | `Page_Init()` → `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProType` | `Page_Init()` → `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProduct` | `Page_Init()` → `UnitPriceBLL.LoadProductUnitPrice` → `ProductUnitPriceDAL.LoadUnitPriceView` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductCase` | `Page_Init()` → `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductSQ` | `Page_Init()` → `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblStockUOM` | `Page_Init()` → `UnitPriceBLL.MaterialMasterReport` → `ProductUnitPriceDAL.GetMaterialMasterReport` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `Page_Init()` → `UnitPriceBLL.LoadProductPriceReportInfo` → `ProductUnitPriceDAL.GetProductPriceReportInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `Page_Init()` → `UnitPriceBLL.LoadProductUnitPrice` → `ProductUnitPriceDAL.LoadUnitPriceView` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `Page_Init` | No Data Found!! | No | Yes | UNKNOWN | Business rule |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-RPT-080-001` | No Data Found!! | Code | `Solution.Web/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx.cs` → `Page_Init()` | user action on this page | operation is refused | "No Data Found!!" | CONFIRMED |

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
| Report definition(s) loaded | `crpPoductPriceInfo.rpt`, `crpProductInfo.rpt` |
| Query-string parameters | `reportType` |
| Session parameters | `PRAM` |
| Data layer | `CompanyInfoBLL`, `StockTransportOrderReportBLL`, `UnitPriceBLL`, `aOrderReportBll`, `aUnitPriceBll` |

`.rpt` files are binary, so the report's own field layout, formulas, grouping, sorting and suppression rules are **UNKNOWN** — see `specs/SPEC-GAPS.md` → `GAP-RPT-001`. The full mapping of every viewer to its report is in `specs/reports/REPORT-INVENTORY.md`.

This viewer does **not** use the master page, so it performs no authentication check (`GAP-SEC-002`). Its parameters arrive from the query string and/or `Session`, and are not validated.


## 26. Dependencies

| Kind | Dependency |
|---|---|
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WHStockInReportBll.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblPackSize` |
| Database object | `tblProCategory` |
| Database object | `tblProType` |
| Database object | `tblProduct` |
| Database object | `tblProductCase` |
| Database object | `tblProductSQ` |
| Database object | `tblStockUOM` |
| Database object | `tblUnitPrice` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit, Crystal Reports 13 |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RPT-080-001` | Unauthenticated access | Request `/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-RPT-080-002` | Validation: No Data Found!! | Trigger `Page_Init()` in the state that produces this message | alert "No Data Found!!" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx` |
| Code-behind | `Solution.Web/SInventory_RPTVIEW/ProductTreadPriceViewer.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WHStockInReportBll.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 4 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 5 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 9 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs` is **shared with 23 other page(s)** (e.g. `StockHTMLReport.aspx`, `ChallanReportViewer.aspx`, `CustomerMasterViewer.aspx`, `DCStockReportViewer.aspx`, `DcToDcChalanReportViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` is **shared with 7 other page(s)** (e.g. `ProductCostPriceViewer.aspx`, `PettyCashApproval.aspx`, `ProUnitPriceApprove.aspx`, `ProUnitPriceEdit.aspx`, `ProUnitPriceEntry.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WHStockInReportBll.cs` is **shared with 24 other page(s)** (e.g. `StockHTMLReport.aspx`, `ChallanReportViewer.aspx`, `CustomerMasterViewer.aspx`, `DCStockReportViewer.aspx`, `DcToDcChalanReportViewer.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblPackSize` — any schema change to it breaks this page.
- Writes/reads `tblProCategory` — any schema change to it breaks this page.
- Writes/reads `tblProType` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductCase` — any schema change to it breaks this page.
- Writes/reads `tblProductSQ` — any schema change to it breaks this page.
- Writes/reads `tblStockUOM` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
