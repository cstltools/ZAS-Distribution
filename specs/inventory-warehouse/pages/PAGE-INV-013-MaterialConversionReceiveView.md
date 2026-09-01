# INV-013 — MaterialConversionReceiveView

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-013` |
| Module | `inventory-warehouse` |
| File name | `MaterialConversionReceiveView.aspx` |
| Route | `/SInventory_UI/MaterialConversionReceiveView.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `MaterialConversionReceiveView.aspx.cs` |
| Code-behind class | `SInventory_UI_WarehouseStockInView` |
| Markup lines | 159 |
| Code-behind lines | 95 |
| Server controls | 20 |
| GridViews | 1 |
| Code-behind methods | 8 |

## 2. Purpose

List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/MaterialConversionReceive.aspx`

Outbound `Response.Redirect` targets: `BankBranchInformation.aspx`, `MaterialConversionReceive.aspx`, `WarehouseStockIn.aspx`

## 5. Route

`/SInventory_UI/MaterialConversionReceiveView.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MaterialConversionReceiveView.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MaterialConversionReceiveView.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |

## 7. UI Control Inventory

20 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `viewLinkButton` | asp:LinkButton | Warehouse Stock In View |  |  |  |  | addNewLinkButton_OnClick | 22 |
| 5 | `loadGridView` | asp:GridView |  |  |  |  |  | loadGridView_RowCommand | 39 |
| 6 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 42 |
| 7 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 43 |
| 8 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 44 |
| 9 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 45 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 46 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 47 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 48 |
| 13 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 49 |
| 14 | `printButton` | asp:ImageButton | loadGridView → col[7] "Report" | ../images/image/if_paste-clipboard-copy_2931174.png |  |  |  | printButton_Click | 52 |
| 15 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 56 |
| 16 | `editImageButton` | asp:ImageButton | loadGridView → col[8] "Edit" | ~/images/edit.png |  |  |  |  | 58 |
| 17 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 63 |
| 18 | `deleteImageButton` | asp:ImageButton | loadGridView → col[9] "Delete" | ~/images/lineDelete.png |  |  |  | return confirm('Are you sure you want to Delete ?'); | 65 |
| 19 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 149 |
| 20 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 152 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `WHStockInMasterID` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Code | WHStockInCode | — | — | — |
| 1 | BoundField | StockIn Date | WHStockInDate | — | — | — |
| 2 | BoundField | Challan No | ChallanNo | — | — | — |
| 3 | BoundField | Challan Date | ChallanDate | — | — | — |
| 4 | BoundField | Total Qty | TotalQuantity | — | — | — |
| 5 | BoundField | Total Amt | TotalValue | — | — | — |
| 6 | BoundField | Status | Status | — | — | — |
| 7 | TemplateField | Report | — | — | `ImageButton:printButton` | — |
| 8 | TemplateField | Edit | — | — | `ImageButton:editImageButton` | — |
| 9 | TemplateField | Delete | — | — | `ImageButton:deleteImageButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadGridView` | `aWarehouseStockInBll.LoadWarehouseStockInData(...)` | `WarehouseStockInBll.LoadWarehouseStockInData()` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.LoadWarehouseStockInData` → `WarehouseStockInDal.GetWarehouseStockInData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblWHStockInMaster` | — |
| `loadGridView_RowCommand` | `aWarehouseStockInBll.DeleteWhStockInInfoById(...)` | `WarehouseStockInBll.DeleteWhStockInInfoById(string stockInId)` <br/>`Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | `WarehouseStockInBll.DeleteWhStockInInfoById` → `WarehouseStockInDal.DeleteWhStockInInfoById` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` | DELETE | `tblWHStockInDetail`, `tblWHStockInMaster` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `viewLinkButton` | asp:LinkButton |  | — | `addNewLinkButton_OnClick` | 3 lines | — | — |
| `printButton` | asp:ImageButton |  | — | `printButton_Click` | 12 lines | — | — |
| `editImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `deleteImageButton` | asp:ImageButton |  | return confirm('Are you sure you want to Delete ?'); | `—` | — | — | — |

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
| 2 | `LoadGridView` | `void LoadGridView()` | 7 | `aWarehouseStockInBll.LoadWarehouseStockInData` | — | — | — |
| 3 | `PopUp` | `void PopUp(string Id)` | 5 | — | — | — | — |
| 4 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 5 | `printButton_Click` | `void printButton_Click(object sender, EventArgs e)` | 12 | — | — | — | `ReportType` |
| 6 | `loadGridView_RowCommand` | `void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 25 | `aWarehouseStockInBll.DeleteWhStockInInfoById` | — | — | `BankBranchInformation`, `IDInformation` |
| 7 | `addNewLinkButton_OnClick` | `void addNewLinkButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 8 | `reloadLinkButton_OnClick` | `void reloadLinkButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
MaterialConversionReceiveView.aspx (markup)
  -> SInventory_UI_WarehouseStockInView (code-behind event handler)
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
| `LoadGridView` | `WarehouseStockInBll` | `LoadWarehouseStockInData` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `loadGridView_RowCommand` | `WarehouseStockInBll` | `DeleteWhStockInInfoById` | `string stockInId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblWHStockInDetail` | `loadGridView_RowCommand()` → `WarehouseStockInBll.DeleteWhStockInInfoById` → `WarehouseStockInDal.DeleteWhStockInInfoById` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` |
| `tblWHStockInMaster` | `LoadGridView()` → `WarehouseStockInBll.LoadWarehouseStockInData` → `WarehouseStockInDal.GetWarehouseStockInData` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `WarehouseStockInBll.DeleteWhStockInInfoById` → `WarehouseStockInDal.DeleteWhStockInInfoById` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` |

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
| `BR-INV-013-001` | {0} | Code | `Solution.Web/SInventory_UI/MaterialConversionReceiveView.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `BankBranchInformation`, `IDInformation`, `ReportType`

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
| Assembly source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Database object | `tblWHStockInDetail` |
| Database object | `tblWHStockInMaster` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-013-001` | Unauthenticated access | Request `/SInventory_UI/MaterialConversionReceiveView.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-013-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-INV-013-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MaterialConversionReceiveView.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MaterialConversionReceiveView.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 20 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 8 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 2 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` is **shared with 11 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `MaterialConversionReceive.aspx`, `NonTranscationalInvoiceReturn.aspx`) — a change there is not local.
- Writes/reads `tblWHStockInDetail` — any schema change to it breaks this page.
- Writes/reads `tblWHStockInMaster` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
