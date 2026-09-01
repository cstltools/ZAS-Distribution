# MST-001 — ASMInfo

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-001` |
| Module | `master-data` |
| File name | `ASMInfo.aspx` |
| Route | `/SInventory_UI/ASMInfo.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ASMInfo.aspx.cs` |
| Code-behind class | `SInventory_UI_ASMInfo` |
| Markup lines | 463 |
| Code-behind lines | 475 |
| Server controls | 37 |
| GridViews | 1 |
| Code-behind methods | 17 |

## 2. Purpose

Master-data / setup screen. It captures and maintains reference data used by the transactional screens. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Master-data / setup screen. It captures and maintains reference data used by the transactional screens. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

Outbound `Response.Redirect` targets: `ASMInfo.aspx`

## 5. Route

`/SInventory_UI/ASMInfo.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ASMInfo.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ASMInfo.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |

## 7. UI Control Inventory

37 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 41 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 42 |
| 4 | `modeLabel` | asp:Label | Operational Mode: |  |  |  |  |  | 89 |
| 5 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_SelectedIndexChanged | 122 |
| 6 | `asmIdHiddenField` | asp:HiddenField | any Name: |  |  |  |  |  | 124 |
| 7 | `updateStatusHiddenField` | asp:HiddenField |  |  |  |  |  |  | 125 |
| 8 | `regionDropDownList` | asp:DropDownList | Region Name: |  | True |  |  | regionDropDownList_OnTextChanged | 141 |
| 9 | `areaDropDownList` | asp:DropDownList | Area Name: |  | True |  |  |  | 159 |
| 10 | `employeeDropDownList` | asp:DropDownList | Employee Name: |  | True |  |  | employeeDropDownList_OnSelectedIndexChanged | 176 |
| 11 | `vacantCheckBox` | asp:CheckBox | Is Vacant |  | True |  |  | vacantCheckBox_OnCheckedChanged | 195 |
| 12 | `isActiveCheckBox` | asp:CheckBox | Is Active |  | True |  |  | isActiveCheckBox_OnCheckedChanged | 217 |
| 13 | `activeDateTextBox` | asp:TextBox | Active Date |  |  |  |  |  | 238 |
| 14 | `imgDate` | asp:ImageButton | Active Date | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 239 |
| 15 | `Date` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 241 |
| 16 | `inActiveDateTextBox` | asp:TextBox | In Active Date |  |  |  |  |  | 263 |
| 17 | `ImageButton1` | asp:ImageButton | In Active Date | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 264 |
| 18 | `CalendarExtender1` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 266 |
| 19 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click1 | 308 |
| 20 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 309 |
| 21 | `loadGridView` | asp:GridView |  |  |  |  |  | loadGridView_RowCommand | 362 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 365 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 366 |
| 24 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 367 |
| 25 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 368 |
| 26 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 369 |
| 27 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 370 |
| 28 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 371 |
| 29 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 372 |
| 30 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 373 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 374 |
| 32 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 375 |
| 33 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 376 |
| 34 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 377 |
| 35 | `editImageButton` | asp:ImageButton | loadGridView → col[12] "Edit" | ~/images/edit.png |  |  |  |  | 379 |
| 36 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 453 |
| 37 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 456 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `ASMId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Employee Code | EmpMasterCode | — | — | — |
| 1 | BoundField | ASM Name | EmpName | — | — | — |
| 2 | BoundField | Company Name | CompanyName | — | — | — |
| 3 | BoundField | Region Name | RegionName | — | — | — |
| 4 | BoundField | Area Name | AreaName | — | — | — |
| 5 | BoundField | Active Status | IsActive | — | — | — |
| 6 | BoundField | Active Date | ActiveDate | — | — | — |
| 7 | BoundField | In Active Date | InActiveDate | — | — | — |
| 8 | BoundField | Create By | EntryBy | — | — | — |
| 9 | BoundField | Create Date | EntryDate | — | — | — |
| 10 | BoundField | UpdateBy | UpdateBy | — | — | — |
| 11 | BoundField | UpdateDate | UpdateDate | — | — | — |
| 12 | TemplateField | Edit | — | — | `ImageButton:editImageButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `ASMInfoDao`

_No BLL/DAL calls resolved from this page — it either has no persistence, or it is a report-viewer page whose data access lives in its own code-behind._

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | — | `aBll.CheckAssignedOrNot()` in `submitButton_Click1()`; `aBll.LoadAreaDropDownList()` in `regionDropDownList_OnTextChanged()`; `aBll.LoadCompanyDropdownList()` in `LoadDropdownList()`; `aBll.LoadEmployeeDropDownList()` in `companyNameDropDownList_SelectedIndexChanged()` | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblRSMInfo`, `tblRegion` | set in code (see the DAL method above) |
| `regionDropDownList` | Region Name: | asp:DropDownList | True | — | `aBll.LoadAreaDropDownList()` in `GetOneRecord()`; `aBll.LoadAreaDropDownList()` in `regionDropDownList_OnTextChanged()`; `aBll.LoadRegionDropDownList()` in `GetOneRecord()`; `aBll.LoadRegionDropDownList()` in `LoadDropdownList()` | `tblASMInfo`, `tblArea`, `tblRSMInfo`, `tblRegion` | set in code (see the DAL method above) |
| `areaDropDownList` | Area Name: | asp:DropDownList | True | — | `aBll.CheckAssignedOrNot()` in `submitButton_Click1()`; `aBll.LoadAreaDropDownList()` in `GetOneRecord()`; `aBll.LoadAreaDropDownList()` in `regionDropDownList_OnTextChanged()` | `tblASMInfo`, `tblArea`, `tblMIOInfo`, `tblRSMInfo`, `tblRegion` | set in code (see the DAL method above) |
| `employeeDropDownList` | Employee Name: | asp:DropDownList | True | employeeDropDownList_OnSelectedIndexChanged | `aBll.LoadEmployeeDropDownList()` in `GetOneRecord()`; `aBll.LoadEmployeeDropDownList()` in `LoadDropdownList()`; `aBll.LoadEmployeeDropDownList()` in `companyNameDropDownList_SelectedIndexChanged()` | `tblEmpGeneralInfo`, `tblRSMInfo` | set in code (see the DAL method above) |

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
| `imgDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton1` | asp:ImageButton |  | — | `—` | — | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click1` | 71 lines | — | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |
| `editImageButton` | asp:ImageButton |  | — | `—` | — | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:CalendarExtender`(activeDateTextBox), `ajaxToolkit:CalendarExtender`(inActiveDateTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 16 | — | — | — | `ASMId` |
| 2 | `SetPageMode` | `void SetPageMode()` | 11 | — | — | — | — |
| 3 | `GetOneRecord` | `void GetOneRecord(string rsmId)` | 82 | — | — | — | — |
| 4 | `LoadGridView` | `void LoadGridView()` | 9 | — | — | — | — |
| 5 | `SetIsActiveStatus` | `void SetIsActiveStatus()` | 8 | — | — | — | — |
| 6 | `LoadDropdownList` | `void LoadDropdownList()` | 6 | — | — | — | — |
| 7 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 71 | — | — | — | `LoginName` |
| 8 | `Clear` | `void Clear()` | 42 | — | — | — | — |
| 9 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 10 | `Validation` | `bool Validation()` | 55 | — | — | — | — |
| 11 | `companyNameDropDownList_SelectedIndexChanged` | `void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 17 | — | — | — | — |
| 12 | `isActiveCheckBox_OnCheckedChanged` | `void isActiveCheckBox_OnCheckedChanged(object sender, EventArgs e)` | 36 | — | — | — | — |
| 13 | `loadGridView_RowCommand` | `void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 25 | — | — | — | `ASMId` |
| 14 | `vacantCheckBox_OnCheckedChanged` | `void vacantCheckBox_OnCheckedChanged(object sender, EventArgs e)` | 20 | — | — | — | — |
| 15 | `regionDropDownList_OnTextChanged` | `void regionDropDownList_OnTextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 16 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 17 | `employeeDropDownList_OnSelectedIndexChanged` | `void employeeDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 9 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ASMInfo.aspx (markup)
  -> SInventory_UI_ASMInfo (code-behind event handler)
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

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

_No table name was recoverable from the code paths reachable from this page._

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `SetPageMode` | Edit | No | Yes | UNKNOWN | Notification |
| `SetPageMode` | Entry | No | Yes | UNKNOWN | Notification |
| `GetOneRecord` | Update | No | Yes | UNKNOWN | Notification |
| `Clear` | Submit | No | Yes | UNKNOWN | Notification |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-001-001` | Edit | Code | `Solution.Web/SInventory_UI/ASMInfo.aspx.cs` → `SetPageMode()` | user action on this page | operation is refused | "Edit" | CONFIRMED |
| `BR-MST-001-002` | Entry | Code | `Solution.Web/SInventory_UI/ASMInfo.aspx.cs` → `SetPageMode()` | user action on this page | operation is refused | "Entry" | CONFIRMED |
| `BR-MST-001-003` | Update | Code | `Solution.Web/SInventory_UI/ASMInfo.aspx.cs` → `GetOneRecord()` | user action on this page | operation is refused | "Update" | CONFIRMED |
| `BR-MST-001-004` | Submit | Code | `Solution.Web/SInventory_UI/ASMInfo.aspx.cs` → `Clear()` | user action on this page | operation is refused | "Submit" | CONFIRMED |
| `BR-MST-001-005` | {0} | Code | `Solution.Web/SInventory_UI/ASMInfo.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `ASMId`, `LoginName`

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
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-001-001` | Unauthenticated access | Request `/SInventory_UI/ASMInfo.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-001-002` | Validation: Edit | Trigger `SetPageMode()` in the state that produces this message | alert "Edit" and the operation is not performed | CONFIRMED |
| `TC-MST-001-003` | Validation: Entry | Trigger `SetPageMode()` in the state that produces this message | alert "Entry" and the operation is not performed | CONFIRMED |
| `TC-MST-001-004` | Validation: Update | Trigger `GetOneRecord()` in the state that produces this message | alert "Update" and the operation is not performed | CONFIRMED |
| `TC-MST-001-005` | Validation: Submit | Trigger `Clear()` in the state that produces this message | alert "Submit" and the operation is not performed | CONFIRMED |
| `TC-MST-001-006` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-MST-001-007` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ASMInfo.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ASMInfo.aspx.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 37 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 17 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 0 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

_This page has no shared dependency that could be determined._

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
