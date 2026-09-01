# SYS-009 — UserWiseApprovalPermission

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SYS-009` |
| Module | `00-system` |
| File name | `UserWiseApprovalPermission.aspx` |
| Route | `/CommonUI/UserWiseApprovalPermission.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `UserWiseApprovalPermission.aspx.cs` |
| Code-behind class | `CommonUI_UserWiseApprovalPermission` |
| Markup lines | 171 |
| Code-behind lines | 180 |
| Server controls | 10 |
| GridViews | 0 |
| Code-behind methods | 11 |

## 2. Purpose

Approval screen. It lists submitted records awaiting a decision and lets an authorised user approve, reject or cancel them, which advances the record status. Which actions the current user may take is resolved at run time by `Library.DAL/InternalCls/ClsApprovalAction.cs`.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Approval screen. It lists submitted records awaiting a decision and lets an authorised user approve, reject or cancel them, which advances the record status. Which actions the current user may take is resolved at run time by `Library.DAL/InternalCls/ClsApprovalAction.cs`.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/CommonUI/UserWiseApprovalPermission.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx` |
| Code-behind | `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/Panal_BLL/PanalBLL.cs` |

## 7. UI Control Inventory

10 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 5 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 6 |
| 4 | `msgLabel` | asp:Label |  |  |  |  |  |  | 25 |
| 5 | `userDropDownList` | asp:DropDownList | User Name : |  | True |  |  | userDropDownList_SelectedIndexChanged | 38 |
| 6 | `mainMenuDropDownList` | asp:DropDownList | Main Menu : |  | True |  |  | mainMenuDropDownList_SelectedIndexChanged | 46 |
| 7 | `appMenuDropDownList` | asp:DropDownList | Menu Option : |  | True |  |  | appMenuDropDownList_SelectedIndexChanged | 54 |
| 8 | `lblMsg` | asp:Label |  |  |  |  |  |  | 68 |
| 9 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 71 |
| 10 | `actionRadioButtonList` | asp:RadioButtonList | Approval Action : |  |  |  |  |  | 88 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aPanalBll.UserDdl(...)` | `PanalBLL.UserDdl(DropDownList userDropDownList)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.UserDdl` → `PanalDAL.UserDdl` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblUser` | — |
| `LoadAction` | `aPanalBll.GetActionPageWiseStepBLL(...)` | `PanalBLL.GetActionPageWiseStepBLL(string manuSL)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.GetActionPageWiseStepBLL` → `PanalDAL.GetActionPageWiseStepDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblActionPageWiseStep` | — |
| `LoadAction` | `aPanalBll.LoadActionBLL(...)` | `PanalBLL.LoadActionBLL(RadioButtonList rdl)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.LoadActionBLL` → `PanalDAL.LoadActionDAL` → `ClsCommonInternalDAL.LoadAction` → `ClsCommonInternalDAL.DataContainerDataTable` → `ClsCommonInternalDAL.DataContainerDataTable` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblAction` | — |
| `userDropDownList_SelectedIndexChanged` | `aPanalBll.MainMenu(...)` | `PanalBLL.MainMenu(DropDownList userDropDownList,string userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblMainMenu`, `tblMenuDistribution` | — |
| `MenuLoad` | `aPanalBll.ApprovalManuDropDownBLL(...)` | `PanalBLL.ApprovalManuDropDownBLL(DropDownList userDropDownList, string userId, string parantId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.ApprovalManuDropDownBLL` → `PanalDAL.ApprovalManuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblActionPageWiseStep`, `tblActionUserWiseApproval`, `tblMainMenu`, `tblMenuDistribution` | — |
| `Save` | `aPanalBll.UserWiseApprovalPermissionBLL(...)` | `PanalBLL.UserWiseApprovalPermissionBLL(ObjUserWiseApprovalPermission approvalPermission)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.UserWiseApprovalPermissionBLL` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.UserWiseApprovalPermissionDAL` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | DELETE, INSERT, SELECT | `tblActionUserWiseApproval` | — |
| `appMenuDropDownList_SelectedIndexChanged` | `aPanalBll.GetManuWiseSelectedActionBLL(...)` | `PanalBLL.GetManuWiseSelectedActionBLL(ObjUserWiseApprovalPermission approvalPermission)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.GetManuWiseSelectedActionBLL` → `PanalDAL.GetManuWiseSelectedActionDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblActionUserWiseApproval` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `userDropDownList` | User Name : | asp:DropDownList | True | userDropDownList_SelectedIndexChanged | `aPanalBll.MainMenu()` in `userDropDownList_SelectedIndexChanged()`; `aPanalBll.UserDdl()` in `Page_Load()` | `tblMainMenu`, `tblMenuDistribution`, `tblUser` | set in code (see the DAL method above) |
| `mainMenuDropDownList` | Main Menu : | asp:DropDownList | True | mainMenuDropDownList_SelectedIndexChanged | `aPanalBll.MainMenu()` in `userDropDownList_SelectedIndexChanged()` | `tblMainMenu`, `tblMenuDistribution` | set in code (see the DAL method above) |
| `appMenuDropDownList` | Menu Option : | asp:DropDownList | True | appMenuDropDownList_SelectedIndexChanged | `aPanalBll.ApprovalManuDropDownBLL()` in `MenuLoad()` | `tblActionPageWiseStep`, `tblActionUserWiseApproval`, `tblMainMenu`, `tblMenuDistribution` | set in code (see the DAL method above) |
| `actionRadioButtonList` | Approval Action : | asp:RadioButtonList | False | — | `aPanalBll.LoadActionBLL()` in `LoadAction()` | `tblAction` | set in code (see the DAL method above) |

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
| `submitButton` | asp:Button | Submit | — | `submitButton_Click` | 7 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | `aPanalBll.UserDdl` | — | — | — |
| 2 | `LoadAction` | `void LoadAction(string ManuSL)` | 30 | `aPanalBll.GetActionPageWiseStepBLL`, `aPanalBll.LoadActionBLL` | — | — | — |
| 3 | `userDropDownList_SelectedIndexChanged` | `void userDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | `aPanalBll.MainMenu` | — | — | — |
| 4 | `MenuLoad` | `void MenuLoad(string userId, string parantId)` | 3 | `aPanalBll.ApprovalManuDropDownBLL` | — | — | — |
| 5 | `mainMenuDropDownList_SelectedIndexChanged` | `void mainMenuDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 5 | — | — | — | — |
| 6 | `Validation` | `bool Validation()` | 31 | — | — | — | — |
| 7 | `Save` | `void Save()` | 13 | `aPanalBll.UserWiseApprovalPermissionBLL` | — | — | — |
| 8 | `Clear` | `void Clear()` | 7 | — | — | — | — |
| 9 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 7 | — | — | — | — |
| 10 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 11 | `appMenuDropDownList_SelectedIndexChanged` | `void appMenuDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 38 | `aPanalBll.GetManuWiseSelectedActionBLL` | — | — | — |

Call chain shape for every persisting action on this page:

```
UserWiseApprovalPermission.aspx (markup)
  -> CommonUI_UserWiseApprovalPermission (code-behind event handler)
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
| `Page_Load` | `PanalBLL` | `UserDdl` | `DropDownList userDropDownList` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `LoadAction` | `PanalBLL` | `GetActionPageWiseStepBLL` | `string manuSL` | `DataTable` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `LoadAction` | `PanalBLL` | `LoadActionBLL` | `RadioButtonList rdl` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `userDropDownList_SelectedIndexChanged` | `PanalBLL` | `MainMenu` | `DropDownList userDropDownList,string userId` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `MenuLoad` | `PanalBLL` | `ApprovalManuDropDownBLL` | `DropDownList userDropDownList, string userId, string parantI` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `Save` | `PanalBLL` | `UserWiseApprovalPermissionBLL` | `ObjUserWiseApprovalPermission approvalPermission` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `appMenuDropDownList_SelectedIndexChanged` | `PanalBLL` | `GetManuWiseSelectedActionBLL` | `ObjUserWiseApprovalPermission approvalPermission` | `DataTable` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblAction` | `LoadAction()` → `PanalBLL.LoadActionBLL` → `PanalDAL.LoadActionDAL` → `ClsCommonInternalDAL.LoadAction` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblActionPageWiseStep` | `LoadAction()` → `PanalBLL.GetActionPageWiseStepBLL` → `PanalDAL.GetActionPageWiseStepDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `MenuLoad()` → `PanalBLL.ApprovalManuDropDownBLL` → `PanalDAL.ApprovalManuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblActionUserWiseApproval` | `MenuLoad()` → `PanalBLL.ApprovalManuDropDownBLL` → `PanalDAL.ApprovalManuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue`, `Save()` → `PanalBLL.UserWiseApprovalPermissionBLL` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.UserWiseApprovalPermissionDAL`, `appMenuDropDownList_SelectedIndexChanged()` → `PanalBLL.GetManuWiseSelectedActionBLL` → `PanalDAL.GetManuWiseSelectedActionDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMainMenu` | `MenuLoad()` → `PanalBLL.ApprovalManuDropDownBLL` → `PanalDAL.ApprovalManuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue`, `userDropDownList_SelectedIndexChanged()` → `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblMenuDistribution` | `MenuLoad()` → `PanalBLL.ApprovalManuDropDownBLL` → `PanalDAL.ApprovalManuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue`, `userDropDownList_SelectedIndexChanged()` → `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblUser` | `Page_Load()` → `PanalBLL.UserDdl` → `PanalDAL.UserDdl` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `appMenuDropDownList_SelectedIndexChanged` | No Action Selected | No | Yes | UNKNOWN | Required |
| `appMenuDropDownList_SelectedIndexChanged` | One Action Selected | No | Yes | UNKNOWN | Required |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SYS-009-001` | {0} | Code | `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-SYS-009-002` | No Action Selected | Code | `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx.cs` → `appMenuDropDownList_SelectedIndexChanged()` | user action on this page | operation is refused | "No Action Selected" | CONFIRMED |
| `BR-SYS-009-003` | One Action Selected | Code | `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx.cs` → `appMenuDropDownList_SelectedIndexChanged()` | user action on this page | operation is refused | "One Action Selected" | CONFIRMED |

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
| Assembly source | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| Database object | `tblAction` |
| Database object | `tblActionPageWiseStep` |
| Database object | `tblActionUserWiseApproval` |
| Database object | `tblMainMenu` |
| Database object | `tblMenuDistribution` |
| Database object | `tblUser` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SYS-009-001` | Unauthenticated access | Request `/CommonUI/UserWiseApprovalPermission.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SYS-009-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SYS-009-003` | Validation: No Action Selected | Trigger `appMenuDropDownList_SelectedIndexChanged()` in the state that produces this message | alert "No Action Selected" and the operation is not performed | CONFIRMED |
| `TC-SYS-009-004` | Validation: One Action Selected | Trigger `appMenuDropDownList_SelectedIndexChanged()` in the state that produces this message | alert "One Action Selected" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx` |
| Code-behind | `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx.cs` |
| Layer source | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 10 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 11 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 6 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/Panal_BLL/PanalBLL.cs` is **shared with 4 other page(s)** (e.g. `AdminMenuPanal.aspx`, `ApprovalStepPageWise.aspx`, `UserPermission.aspx`, `Login.aspx`) — a change there is not local.
- Writes/reads `tblAction` — any schema change to it breaks this page.
- Writes/reads `tblActionPageWiseStep` — any schema change to it breaks this page.
- Writes/reads `tblActionUserWiseApproval` — any schema change to it breaks this page.
- Writes/reads `tblMainMenu` — any schema change to it breaks this page.
- Writes/reads `tblMenuDistribution` — any schema change to it breaks this page.
- Writes/reads `tblUser` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
