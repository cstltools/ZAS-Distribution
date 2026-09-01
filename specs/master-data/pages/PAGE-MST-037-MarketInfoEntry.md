# MST-037 — MarketInfoEntry

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-037` |
| Module | `master-data` |
| File name | `MarketInfoEntry.aspx` |
| Route | `/SInventory_UI/MarketInfoEntry.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `MarketInfoEntry.aspx.cs` |
| Code-behind class | `SInventory_UI_MarketInfoEntry` |
| Markup lines | 363 |
| Code-behind lines | 149 |
| Server controls | 18 |
| GridViews | 0 |
| Code-behind methods | 14 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/HREmployeeSysnc.aspx`
- referenced from `SInventory_UI/MarketInfoView.aspx`

Outbound `Response.Redirect` targets: `MarketInfoView.aspx`

## 5. Route

`/SInventory_UI/MarketInfoEntry.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MarketInfoEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MarketInfoEntry.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |

## 7. UI Control Inventory

18 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `MarketInfotListImageButton` | asp:ImageButton | View List | ~/images/viewList.png |  |  |  | areaImageButton_Click | 21 |
| 5 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 27 |
| 6 | `regionDropDownList` | asp:DropDownList | Region Name |  | True |  |  | regionDropDownList_OnTextChanged | 84 |
| 7 | `areaDropDownList` | asp:DropDownList | Area Name |  | true |  |  | areaDropDownList_OnSelectedIndexChanged | 104 |
| 8 | `territoryDropDownList` | asp:DropDownList | Territory Name |  |  |  |  |  | 124 |
| 9 | `marketCodeTextBox` | asp:TextBox | Market Code: |  | true |  |  | marketCodeTextBox_OnTextChanged | 144 |
| 10 | `marketnameTextBox` | asp:TextBox | Market Name: |  | true |  |  | marketnameTextBox_OnTextChanged | 165 |
| 11 | `shortNameTextBox` | asp:TextBox | Market Short Name: |  | true |  |  | shortNameTextBox_OnTextChanged | 184 |
| 12 | `descriptionTextBox` | asp:TextBox | Description: |  |  |  |  |  | 202 |
| 13 | `remarksTextBox` | asp:TextBox | Remarks: |  |  |  |  |  | 220 |
| 14 | `isActiveCheckBox` | asp:CheckBox | Is Active: |  |  |  |  |  | 240 |
| 15 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click1 | 296 |
| 16 | `cancelButton` | asp:Button |  | Cancel |  |  |  | cancelButton_Click | 297 |
| 17 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 353 |
| 18 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 356 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropdownList` | `aMarketInfoBll.LoadRegionDropdownList(...)` | `MarketInfoBLL.LoadRegionDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadRegionDropdownList` | — | — | — |
| `submitButton_Click1` | `aInfoBll.SaveMarket(...)` | `MarketInfoBLL.SaveMarket(MarketInfo MarketInfo)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.SaveMarket` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `MarketInfoDAL.HasMarketName` → `ClsCommonInternalDAL.DataContainerDataReader` → `MarketInfoDAL.SaveMarketInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `MarketInfoBLL.MarketCodeGenerator` | SELECT | `tblMarket` | — |
| `regionDropDownList_OnTextChanged` | `aMarketInfoBll.LoadAreaOnDropdownList(...)` | `MarketInfoBLL.LoadAreaOnDropdownList(DropDownList ddl, string regionId)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadAreaOnDropdownList` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `areaDropDownList_OnSelectedIndexChanged` | `aMarketInfoBll.LoadTerritoryOnDropdownList(...)` | `MarketInfoBLL.LoadTerritoryOnDropdownList(DropDownList ddl, string areaId)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadTerritoryOnDropdownList` → `MarketInfoDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblTerritory` | — |
| `marketnameTextBox_OnTextChanged` | `aMarketInfoBll.NameExistOrNot(...)` | `MarketInfoBLL.NameExistOrNot(string name)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.NameExistOrNot` → `MarketInfoDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblMarket` | — |
| `marketCodeTextBox_OnTextChanged` | `aMarketInfoBll.CodeExistOrNot(...)` | `MarketInfoBLL.CodeExistOrNot(string code)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.CodeExistOrNot` → `MarketInfoDAL.CodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblMarket` | — |
| `shortNameTextBox_OnTextChanged` | `aMarketInfoBll.ShortNameExistOrNot(...)` | `MarketInfoBLL.ShortNameExistOrNot(string srtName)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.ShortNameExistOrNot` → `MarketInfoDAL.ShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblMarket` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `regionDropDownList` | Region Name | asp:DropDownList | True | — | `aMarketInfoBll.LoadAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `aMarketInfoBll.LoadRegionDropdownList()` in `LoadDropdownList()` | `tblArea` | set in code (see the DAL method above) |
| `areaDropDownList` | Area Name | asp:DropDownList | true | areaDropDownList_OnSelectedIndexChanged | `aMarketInfoBll.LoadAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `aMarketInfoBll.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblArea`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory Name | asp:DropDownList | False | — | `aMarketInfoBll.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblTerritory` | set in code (see the DAL method above) |

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
| `MarketInfotListImageButton` | asp:ImageButton |  | — | `areaImageButton_Click` | 3 lines | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click1` | 26 lines | `aInfoBll.SaveMarket` | — |
| `cancelButton` | asp:Button | Cancel | — | `cancelButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `LoadDropdownList` | `void LoadDropdownList()` | 3 | `aMarketInfoBll.LoadRegionDropdownList` | — | — | — |
| 3 | `SetActiveStatus` | `void SetActiveStatus()` | 6 | — | — | — | — |
| 4 | `Clear` | `void Clear()` | 11 | — | — | — | — |
| 5 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 6 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 26 | `aInfoBll.SaveMarket` | — | — | `LoginName` |
| 7 | `areaImageButton_Click` | `void areaImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 8 | `regionDropDownList_OnTextChanged` | `void regionDropDownList_OnTextChanged(object sender, EventArgs e)` | 11 | `aMarketInfoBll.LoadAreaOnDropdownList` | — | — | — |
| 9 | `areaDropDownList_OnSelectedIndexChanged` | `void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 10 | `aMarketInfoBll.LoadTerritoryOnDropdownList` | — | — | — |
| 10 | `marketnameTextBox_OnTextChanged` | `void marketnameTextBox_OnTextChanged(object sender, EventArgs e)` | 5 | `aMarketInfoBll.NameExistOrNot` | — | — | — |
| 11 | `GotoPermissionSetUp` | `void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)` | 7 | — | — | — | — |
| 12 | `marketCodeTextBox_OnTextChanged` | `void marketCodeTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `aMarketInfoBll.CodeExistOrNot` | — | — | — |
| 13 | `shortNameTextBox_OnTextChanged` | `void shortNameTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `aMarketInfoBll.ShortNameExistOrNot` | — | — | — |
| 14 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
MarketInfoEntry.aspx (markup)
  -> SInventory_UI_MarketInfoEntry (code-behind event handler)
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
| `LoadDropdownList` | `MarketInfoBLL` | `LoadRegionDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `submitButton_Click1` | `MarketInfoBLL` | `SaveMarket` | `MarketInfo MarketInfo` | `string` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `regionDropDownList_OnTextChanged` | `MarketInfoBLL` | `LoadAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `MarketInfoBLL` | `LoadTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `marketnameTextBox_OnTextChanged` | `MarketInfoBLL` | `NameExistOrNot` | `string name` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `marketCodeTextBox_OnTextChanged` | `MarketInfoBLL` | `CodeExistOrNot` | `string code` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `shortNameTextBox_OnTextChanged` | `MarketInfoBLL` | `ShortNameExistOrNot` | `string srtName` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `regionDropDownList_OnTextChanged()` → `MarketInfoBLL.LoadAreaOnDropdownList` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblMarket` | `marketCodeTextBox_OnTextChanged()` → `MarketInfoBLL.CodeExistOrNot` → `MarketInfoDAL.CodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketnameTextBox_OnTextChanged()` → `MarketInfoBLL.NameExistOrNot` → `MarketInfoDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `shortNameTextBox_OnTextChanged()` → `MarketInfoBLL.ShortNameExistOrNot` → `MarketInfoDAL.ShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `MarketInfoBLL.SaveMarket` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `MarketInfoDAL.HasMarketName` |
| `tblTerritory` | `areaDropDownList_OnSelectedIndexChanged()` → `MarketInfoBLL.LoadTerritoryOnDropdownList` → `MarketInfoDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |

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
| `BR-MST-037-001` | {0} | Code | `Solution.Web/SInventory_UI/MarketInfoEntry.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Database object | `tblArea` |
| Database object | `tblMarket` |
| Database object | `tblTerritory` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-037-001` | Unauthenticated access | Request `/SInventory_UI/MarketInfoEntry.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-037-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MarketInfoEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MarketInfoEntry.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 18 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 14 methods traced | n/a | **MISSING → now documented** | section 12 |
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

- `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` is **shared with 4 other page(s)** (e.g. `CustomerReport.aspx`, `FieldForceSalesSummery.aspx`, `MarketInfoEdit.aspx`, `MarketInfoView.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
