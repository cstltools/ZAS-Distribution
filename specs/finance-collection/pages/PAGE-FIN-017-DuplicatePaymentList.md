# FIN-017 — DuplicatePaymentList

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `FIN-017` |
| Module | `finance-collection` |
| File name | `DuplicatePaymentList.aspx` |
| Route | `/SInventory_UI/DuplicatePaymentList.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `DuplicatePaymentList.aspx.cs` |
| Code-behind class | `SInventory_UI_ErrorInvoiceList` |
| Markup lines | 202 |
| Code-behind lines | 73 |
| Server controls | 16 |
| GridViews | 1 |
| Code-behind methods | 5 |

## 2. Purpose

List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/DuplicatePaymentList.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DuplicatePaymentList.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DuplicatePaymentList.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` |

## 7. UI Control Inventory

16 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `invoiceGridView` | asp:GridView |  |  |  |  |  |  | 76 |
| 5 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 80 |
| 6 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 81 |
| 7 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 82 |
| 8 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 83 |
| 9 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 84 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 85 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 86 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 87 |
| 13 | `removeButton` | asp:Button |  | Remove |  |  |  | removeButton_Click | 122 |
| 14 | `refreshButton` | asp:Button |  | Refress |  |  |  | refreshButton_Click | 123 |
| 15 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 192 |
| 16 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 195 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `invoiceGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceNo` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Unit Name | ComUnitName | — | — | — |
| 1 | BoundField | Customer Code | CustomerCode | — | — | — |
| 2 | BoundField | Customer Name | CustomerName | — | — | — |
| 3 | BoundField | Invoice No | InvoiceNo | — | — | — |
| 4 | BoundField | Invoice Date | InvoiceDate | — | — | — |
| 5 | BoundField | Delivary Invoice No | DelivaryInvoiceNo | — | — | — |
| 6 | BoundField | Payment Date | PaymentDate | — | — | — |
| 7 | BoundField | Payment Amount | PaymentAmount | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadErrorInvoiceList` | `aInvoiceDal.GetDuplicatePaymentList(...)` | `DuplicatePaymentCorrectionDal.GetDuplicatePaymentList()` <br/>`Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` | `DuplicatePaymentCorrectionDal.GetDuplicatePaymentList` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | — |
| `removeButton_Click` | `aInvoiceDal.DeleteDuplicatePayment(...)` | `DuplicatePaymentCorrectionDal.DeleteDuplicatePayment(string Invoice)` <br/>`Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` | `DuplicatePaymentCorrectionDal.DeleteDuplicatePayment` → `ClsCommonInternalDAL.RunStoreProcedure` | — | — | `sp_DEL_DuplicatePayment` |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `removeButton` | asp:Button | Remove | — | `removeButton_Click` | 22 lines | `aInvoiceDal.DeleteDuplicatePayment` | — |
| `refreshButton` | asp:Button | Refress | — | `refreshButton_Click` | 3 lines | — | — |

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
| 2 | `LoadErrorInvoiceList` | `void LoadErrorInvoiceList()` | 14 | `aInvoiceDal.GetDuplicatePaymentList` | — | — | — |
| 3 | `removeButton_Click` | `void removeButton_Click(object sender, EventArgs e)` | 22 | `aInvoiceDal.DeleteDuplicatePayment` | — | — | — |
| 4 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 5 | `refreshButton_Click` | `void refreshButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
DuplicatePaymentList.aspx (markup)
  -> SInventory_UI_ErrorInvoiceList (code-behind event handler)
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
| `LoadErrorInvoiceList` | `DuplicatePaymentCorrectionDal` | `GetDuplicatePaymentList` | `` | `DataTable` | SELECT | `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` |
| `removeButton_Click` | `DuplicatePaymentCorrectionDal` | `DeleteDuplicatePayment` | `string Invoice` | `int` | — | — | `sp_DEL_DuplicatePayment` | `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_DEL_DuplicatePayment` | removeButton_Click → `DuplicatePaymentCorrectionDal.DeleteDuplicatePayment → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyUnit` | `LoadErrorInvoiceList()` → `DuplicatePaymentCorrectionDal.GetDuplicatePaymentList` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `LoadErrorInvoiceList()` → `DuplicatePaymentCorrectionDal.GetDuplicatePaymentList` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustPayDetail` | `LoadErrorInvoiceList()` → `DuplicatePaymentCorrectionDal.GetDuplicatePaymentList` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerPay` | `LoadErrorInvoiceList()` → `DuplicatePaymentCorrectionDal.GetDuplicatePaymentList` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `LoadErrorInvoiceList()` → `DuplicatePaymentCorrectionDal.GetDuplicatePaymentList` → `ClsCommonInternalDAL.DataContainerDataTable` |

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
| `BR-FIN-017-001` | {0} | Code | `Solution.Web/SInventory_UI/DuplicatePaymentList.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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
| Assembly source | `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerPay` |
| Database object | `tblInvoice` |
| Stored procedure | `sp_DEL_DuplicatePayment` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-FIN-017-001` | Unauthenticated access | Request `/SInventory_UI/DuplicatePaymentList.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-FIN-017-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-FIN-017-003` | Grid loads | Open the page with data present | Grid `invoiceGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DuplicatePaymentList.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DuplicatePaymentList.aspx.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 16 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 5 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 5 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerPay` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
