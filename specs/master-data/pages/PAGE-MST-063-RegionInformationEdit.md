# MST-063 — RegionInformationEdit

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-063` |
| Module | `master-data` |
| File name | `RegionInformationEdit.aspx` |
| Route | `/SInventory_UI/RegionInformationEdit.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `RegionInformationEdit.aspx.cs` |
| Code-behind class | `SInventory_UI_RegionInformationEdit` |
| Markup lines | 349 |
| Code-behind lines | 146 |
| Server controls | 15 |
| GridViews | 0 |
| Code-behind methods | 9 |

## 2. Purpose

Edit screen. It loads an existing record by its key (usually from Session or query string) and persists the modified values.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Edit screen. It loads an existing record by its key (usually from Session or query string) and persists the modified values.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/RegionInformationViw.aspx`

## 5. Route

`/SInventory_UI/RegionInformationEdit.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/RegionInformationEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/RegionInformationEdit.aspx.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |

## 7. UI Control Inventory

15 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ScriptManager1` | asp:ScriptManager | Edit |  |  |  |  |  | 16 |
| 2 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 18 |
| 3 | `regionInfoIdHiddenField` | asp:HiddenField | DZSM Edit |  |  |  |  |  | 29 |
| 4 | `codeHiddenField` | asp:HiddenField | DZSM Edit |  |  |  |  |  | 30 |
| 5 | `nameHiddenField` | asp:HiddenField |  |  |  |  |  |  | 31 |
| 6 | `shortNameHiddenField` | asp:HiddenField |  |  |  |  |  |  | 32 |
| 7 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 39 |
| 8 | `regioncodeTextBox` | asp:TextBox | Region Code: |  | true |  |  | regioncodeTextBox_OnTextChanged | 54 |
| 9 | `regionnameTextBox` | asp:TextBox | Region Name: |  | true |  |  | regionnameTextBox_OnTextChanged | 70 |
| 10 | `shortNameTextBox` | asp:TextBox | Region Short Name: |  | True |  |  | shortNameTextBox_OnTextChanged | 87 |
| 11 | `descriptionTextBox` | asp:TextBox | Description: |  |  |  |  |  | 104 |
| 12 | `remarksTextBox` | asp:TextBox | Remarks: |  |  |  |  |  | 121 |
| 13 | `isActiveCheckBox` | asp:CheckBox | Is Active: |  |  |  |  |  | 141 |
| 14 | `updateButton` | asp:Button |  | Update |  |  |  | updateButton_Click | 195 |
| 15 | `closeButton` | asp:Button |  | Close |  |  |  | closeButton_Click | 197 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `updateButton_Click` | `RegionInfoBLL.UpdateDataForRegionInfo(...)` | `RegionInfoBLL.UpdateDataForRegionInfo(RegionInfo aRegionInfo)` <br/>`Library.BLL/SInventory_BLL/RegionInfoBLL.cs` | `RegionInfoBLL.UpdateDataForRegionInfo` → `RegionDAL.UpdateRegionInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblRegion` | — |
| `RegionInfoLoad` | `RegionInfoBLL.RegionInfoEditLoad(...)` | `RegionInfoBLL.RegionInfoEditLoad(string RegionInfoId)` <br/>`Library.BLL/SInventory_BLL/RegionInfoBLL.cs` | `RegionInfoBLL.RegionInfoEditLoad` → `RegionDAL.RegionInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | — | `tblRegion` | — |
| `regioncodeTextBox_OnTextChanged` | `RegionInfoBLL.CheckRegionCodeExistOrNot(...)` | `RegionInfoBLL.CheckRegionCodeExistOrNot(string code)` <br/>`Library.BLL/SInventory_BLL/RegionInfoBLL.cs` | `RegionInfoBLL.CheckRegionCodeExistOrNot` → `RegionDAL.CheckRegionCodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblRegion` | — |
| `regionnameTextBox_OnTextChanged` | `RegionInfoBLL.NameExistOrNot(...)` | `RegionInfoBLL.NameExistOrNot(string name)` <br/>`Library.BLL/SInventory_BLL/RegionInfoBLL.cs` | `RegionInfoBLL.NameExistOrNot` → `RegionDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblRegion` | — |
| `shortNameTextBox_OnTextChanged` | `RegionInfoBLL.ShortNameExistOrNot(...)` | `RegionInfoBLL.ShortNameExistOrNot(string shortname)` <br/>`Library.BLL/SInventory_BLL/RegionInfoBLL.cs` | `RegionInfoBLL.ShortNameExistOrNot` → `RegionDAL.ShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblRegion` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `updateButton` | asp:Button | Update | — | `updateButton_Click` | 38 lines | `RegionInfoBLL.UpdateDataForRegionInfo` | — |
| `closeButton` | asp:Button | Close | — | `closeButton_Click` | 4 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 10 | — | — | — | — |
| 2 | `updateButton_Click` | `void updateButton_Click(object sender, EventArgs e)` | 38 | `RegionInfoBLL.UpdateDataForRegionInfo` | — | — | `LoginName` |
| 3 | `RegionInfoLoad` | `void RegionInfoLoad(string marketId)` | 17 | `RegionInfoBLL.RegionInfoEditLoad` | — | — | — |
| 4 | `closeButton_Click` | `void closeButton_Click(object sender, EventArgs e)` | 4 | — | — | — | — |
| 5 | `regioncodeTextBox_OnTextChanged` | `void regioncodeTextBox_OnTextChanged(object sender, EventArgs e)` | 5 | `RegionInfoBLL.CheckRegionCodeExistOrNot` | — | — | — |
| 6 | `regionnameTextBox_OnTextChanged` | `void regionnameTextBox_OnTextChanged(object sender, EventArgs e)` | 5 | `RegionInfoBLL.NameExistOrNot` | — | — | — |
| 7 | `shortNameTextBox_OnTextChanged` | `void shortNameTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `RegionInfoBLL.ShortNameExistOrNot` | — | — | — |
| 8 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 9 | `GotoUpdatePermissionSetUp` | `void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, )` | 23 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
RegionInformationEdit.aspx (markup)
  -> SInventory_UI_RegionInformationEdit (code-behind event handler)
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
| `updateButton_Click` | `RegionInfoBLL` | `UpdateDataForRegionInfo` | `RegionInfo aRegionInfo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |
| `RegionInfoLoad` | `RegionInfoBLL` | `RegionInfoEditLoad` | `string RegionInfoId` | `RegionInfo` | — | — | — | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |
| `regioncodeTextBox_OnTextChanged` | `RegionInfoBLL` | `CheckRegionCodeExistOrNot` | `string code` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |
| `regionnameTextBox_OnTextChanged` | `RegionInfoBLL` | `NameExistOrNot` | `string name` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |
| `shortNameTextBox_OnTextChanged` | `RegionInfoBLL` | `ShortNameExistOrNot` | `string shortname` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblRegion` | `RegionInfoLoad()` → `RegionInfoBLL.RegionInfoEditLoad` → `RegionDAL.RegionInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `regioncodeTextBox_OnTextChanged()` → `RegionInfoBLL.CheckRegionCodeExistOrNot` → `RegionDAL.CheckRegionCodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `regionnameTextBox_OnTextChanged()` → `RegionInfoBLL.NameExistOrNot` → `RegionDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `shortNameTextBox_OnTextChanged()` → `RegionInfoBLL.ShortNameExistOrNot` → `RegionDAL.ShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `updateButton_Click()` → `RegionInfoBLL.UpdateDataForRegionInfo` → `RegionDAL.UpdateRegionInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `updateButton_Click` | Data Not Update!!! | No | Yes | UNKNOWN | Business rule |
| `updateButton_Click` | Data Update Successfully!!! Please Reload | No | Yes | UNKNOWN | Required |
| `updateButton_Click` | Please input data in all Textbox | No | Yes | UNKNOWN | Required |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-063-001` | Data Not Update!!! | Code | `Solution.Web/SInventory_UI/RegionInformationEdit.aspx.cs` → `updateButton_Click()` | user action on this page | operation is refused | "Data Not Update!!!" | CONFIRMED |
| `BR-MST-063-002` | {0} | Code | `Solution.Web/SInventory_UI/RegionInformationEdit.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`

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

_No integration touchpoint detected on this page._

See `specs/00-system/integrations.md` for the full list.

## 25. Reports

_This page opens no report._

## 26. Dependencies

| Kind | Dependency |
|---|---|
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |
| Database object | `tblRegion` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-063-001` | Unauthenticated access | Request `/SInventory_UI/RegionInformationEdit.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-MST-063-002` | Validation: Data Not Update!!! | Trigger `updateButton_Click()` in the state that produces this message | alert "Data Not Update!!!" and the operation is not performed | CONFIRMED |
| `TC-MST-063-003` | Validation: Data Update Successfully!!! Please Reloa | Trigger `updateButton_Click()` in the state that produces this message | alert "Data Update Successfully!!! Please Reload" and the operation is not performed | CONFIRMED |
| `TC-MST-063-004` | Validation: Please input data in all Textbox | Trigger `updateButton_Click()` in the state that produces this message | alert "Please input data in all Textbox" and the operation is not performed | CONFIRMED |
| `TC-MST-063-005` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/RegionInformationEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/RegionInformationEdit.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 15 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 9 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 1 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` is **shared with 2 other page(s)** (e.g. `RegionInformation.aspx`, `RegionInformationViw.aspx`) — a change there is not local.
- Writes/reads `tblRegion` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
