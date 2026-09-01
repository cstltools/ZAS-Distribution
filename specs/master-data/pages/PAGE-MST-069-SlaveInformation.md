# MST-069 — SlaveInformation

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-069` |
| Module | `master-data` |
| File name | `SlaveInformation.aspx` |
| Route | `/SInventory_UI/SlaveInformation.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `SlaveInformation.aspx.cs` |
| Code-behind class | `SInventory_UI_SlaveInformation` |
| Markup lines | 496 |
| Code-behind lines | 426 |
| Server controls | 32 |
| GridViews | 0 |
| Code-behind methods | 22 |

## 2. Purpose

Master-data / setup screen. It captures and maintains reference data used by the transactional screens.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Master-data / setup screen. It captures and maintains reference data used by the transactional screens.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/SlaveInformationView.aspx`

Outbound `Response.Redirect` targets: `SlaveInformationView.aspx`

## 5. Route

`/SInventory_UI/SlaveInformation.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/SlaveInformation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/SlaveInformation.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |

## 7. UI Control Inventory

32 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 43 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 44 |
| 4 | `ListImageButton` | asp:ImageButton | View List | ~/images/viewList.png |  |  |  | ListImageButton_Click | 79 |
| 5 | `modeLabel` | asp:Label | Operational Mode: |  |  |  |  |  | 93 |
| 6 | `slaveIdHiddenField` | asp:HiddenField | Operational Mode: |  |  |  |  |  | 101 |
| 7 | `companyNameDropDownList` | asp:DropDownList | Company Name |  | True |  |  | companyNameDropDownList_OnSelectedIndexChanged | 128 |
| 8 | `hiddenTextBox` | asp:TextBox |  |  |  |  | false |  | 130 |
| 9 | `maxAmountTextBox` | asp:TextBox | Maximum Amount |  | true |  |  | maxAmountTextBox_OnTextChanged | 147 |
| 10 | `fcostPriceTextBox` | ajaxToolkit:FilteredTextBoxExtender |  |  |  |  |  |  | 148 |
| 11 | `minAmmountTextBox` | asp:TextBox | Minimum Amount |  | true |  |  | minAmmountTextBox_OnTextChanged | 168 |
| 12 | `FilteredTextBoxExtender5` | ajaxToolkit:FilteredTextBoxExtender |  |  |  |  |  |  | 169 |
| 13 | `disPerTextBox` | asp:TextBox | Discount Percent % |  |  |  |  |  | 190 |
| 14 | `vatperTextBox_FilteredTextBoxExtender` | ajaxToolkit:FilteredTextBoxExtender | Discount Percent % |  |  |  |  |  | 191 |
| 15 | `isActiveCheckBox` | asp:CheckBox | Is Active |  | true |  |  | isActiveCheckBox_OnCheckedChanged | 213 |
| 16 | `activeDtTextBox` | asp:TextBox | Active Date |  | True |  |  |  | 233 |
| 17 | `activeDtTextBox_CalendarExtender` | ajaxToolkit:CalendarExtender | Active Date |  |  |  |  |  | 234 |
| 18 | `ImageButton` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 237 |
| 19 | `inActiveDateTextBox` | asp:TextBox | InActive Date |  | True |  |  |  | 261 |
| 20 | `CalendarExtender3` | ajaxToolkit:CalendarExtender | InActive Date |  |  |  |  |  | 262 |
| 21 | `ImageButton3` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 265 |
| 22 | `newMaxAmountTextBox` | asp:TextBox | Maximum Amount(New) |  | True |  |  | newMaxAmountTextBox_OnTextChanged | 286 |
| 23 | `FilteredTextBoxExtender1` | ajaxToolkit:FilteredTextBoxExtender | Maximum Amount(New) |  |  |  |  |  | 287 |
| 24 | `newMinAmountTextBox` | asp:TextBox | Minimum Amount(New) |  | True |  |  | newMinAmountTextBox_OnTextChanged | 306 |
| 25 | `FilteredTextBoxExtender2` | ajaxToolkit:FilteredTextBoxExtender | Minimum Amount(New) |  |  |  |  |  | 307 |
| 26 | `newDisPerTextBox` | asp:TextBox | Discount Percent % (New) |  |  |  |  |  | 327 |
| 27 | `FilteredTextBoxExtender3` | ajaxToolkit:FilteredTextBoxExtender | Discount Percent % (New) |  |  |  |  |  | 328 |
| 28 | `newActiveDateTextBox` | asp:TextBox | Active Date (New) |  | True |  |  |  | 349 |
| 29 | `CalendarExtender1` | ajaxToolkit:CalendarExtender | Active Date (New) |  |  |  |  |  | 350 |
| 30 | `ImageButton1` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 353 |
| 31 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 396 |
| 32 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 398 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

_No BLL/DAL calls resolved from this page — it either has no persistence, or it is a report-viewer page whose data access lives in its own code-behind._

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyNameDropDownList` | Company Name | asp:DropDownList | True | companyNameDropDownList_OnSelectedIndexChanged | `aBll.LoadCompanyDropdownList()` in `LoadDropDown()` | `tblCompanyInfo` | set in code (see the DAL method above) |

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
| `ListImageButton` | asp:ImageButton |  | — | `ListImageButton_Click` | 3 lines | — | — |
| `ImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton3` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton1` | asp:ImageButton |  | — | `—` | — | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click` | 14 lines | — | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:FilteredTextBoxExtender`(maxAmountTextBox), `ajaxToolkit:FilteredTextBoxExtender`(minAmmountTextBox), `ajaxToolkit:FilteredTextBoxExtender`(disPerTextBox), `ajaxToolkit:CalendarExtender`(activeDtTextBox), `ajaxToolkit:CalendarExtender`(inActiveDateTextBox), `ajaxToolkit:FilteredTextBoxExtender`(newMaxAmountTextBox), `ajaxToolkit:FilteredTextBoxExtender`(newMinAmountTextBox), `ajaxToolkit:FilteredTextBoxExtender`(newDisPerTextBox), `ajaxToolkit:CalendarExtender`(newActiveDateTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 16 | — | — | — | `SlaveId` |
| 2 | `GetOneRecord` | `void GetOneRecord(string slaveId)` | 21 | — | — | — | — |
| 3 | `SetPageMode` | `void SetPageMode()` | 12 | — | — | — | — |
| 4 | `LoadDropDown` | `void LoadDropDown()` | 3 | — | — | — | — |
| 5 | `SetActiveStatus` | `void SetActiveStatus()` | 11 | — | — | — | — |
| 6 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 7 | `Validation` | `bool Validation()` | 75 | — | — | — | — |
| 8 | `Clear` | `void Clear()` | 19 | — | — | — | — |
| 9 | `isActiveCheckBox_OnCheckedChanged` | `void isActiveCheckBox_OnCheckedChanged(object sender, EventArgs e)` | 10 | — | — | — | — |
| 10 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 11 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 14 | — | — | — | — |
| 12 | `SaveSlaveInformation` | `void SaveSlaveInformation(string msg)` | 33 | — | — | — | `LoginName` |
| 13 | `UpdateSlaveInformation` | `void UpdateSlaveInformation()` | 19 | — | — | — | `LoginName` |
| 14 | `maxAmountTextBox_OnTextChanged` | `void maxAmountTextBox_OnTextChanged(object sender, EventArgs e)` | 18 | — | — | — | — |
| 15 | `CheckMAxMin` | `void CheckMAxMin()` | 33 | — | — | — | — |
| 16 | `CheckRange` | `bool CheckRange(string companyId, decimal value, TextBox ammountTextBox)` | 27 | — | — | — | — |
| 17 | `minAmmountTextBox_OnTextChanged` | `void minAmmountTextBox_OnTextChanged(object sender, EventArgs e)` | 19 | — | — | — | — |
| 18 | `ListImageButton_Click` | `void ListImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 19 | `newMinAmountTextBox_OnTextChanged` | `void newMinAmountTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 20 | `newMaxAmountTextBox_OnTextChanged` | `void newMaxAmountTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 21 | `companyNameDropDownList_OnSelectedIndexChanged` | `void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 22 | `SetCompanyWiseStatus` | `void SetCompanyWiseStatus()` | 12 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
SlaveInformation.aspx (markup)
  -> SInventory_UI_SlaveInformation (code-behind event handler)
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
| `SetPageMode` | Update | No | Yes | UNKNOWN | Notification |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `Clear` | Submit | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `fcostPriceTextBox` | `maxAmountTextBox` | Custom, Numbers | .0123456789 |
| `FilteredTextBoxExtender5` | `minAmmountTextBox` | Custom, Numbers | .0123456789 |
| `vatperTextBox_FilteredTextBoxExtender` | `disPerTextBox` | Custom, Numbers | 0123456789 |
| `FilteredTextBoxExtender1` | `newMaxAmountTextBox` | Custom, Numbers | .0123456789 |
| `FilteredTextBoxExtender2` | `newMinAmountTextBox` | Custom, Numbers | .0123456789 |
| `FilteredTextBoxExtender3` | `newDisPerTextBox` | Custom, Numbers | 0123456789 |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-069-001` | Edit | Code | `Solution.Web/SInventory_UI/SlaveInformation.aspx.cs` → `SetPageMode()` | user action on this page | operation is refused | "Edit" | CONFIRMED |
| `BR-MST-069-002` | Entry | Code | `Solution.Web/SInventory_UI/SlaveInformation.aspx.cs` → `SetPageMode()` | user action on this page | operation is refused | "Entry" | CONFIRMED |
| `BR-MST-069-003` | Update | Code | `Solution.Web/SInventory_UI/SlaveInformation.aspx.cs` → `SetPageMode()` | user action on this page | operation is refused | "Update" | CONFIRMED |
| `BR-MST-069-004` | {0} | Code | `Solution.Web/SInventory_UI/SlaveInformation.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-MST-069-005` | Submit | Code | `Solution.Web/SInventory_UI/SlaveInformation.aspx.cs` → `Clear()` | user action on this page | operation is refused | "Submit" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `SlaveId`

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
| `TC-MST-069-001` | Unauthenticated access | Request `/SInventory_UI/SlaveInformation.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-069-002` | Validation: Edit | Trigger `SetPageMode()` in the state that produces this message | alert "Edit" and the operation is not performed | CONFIRMED |
| `TC-MST-069-003` | Validation: Entry | Trigger `SetPageMode()` in the state that produces this message | alert "Entry" and the operation is not performed | CONFIRMED |
| `TC-MST-069-004` | Validation: Update | Trigger `SetPageMode()` in the state that produces this message | alert "Update" and the operation is not performed | CONFIRMED |
| `TC-MST-069-005` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-MST-069-006` | Validation: Submit | Trigger `Clear()` in the state that produces this message | alert "Submit" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/SlaveInformation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/SlaveInformation.aspx.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 32 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 22 methods traced | n/a | **MISSING → now documented** | section 12 |
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
