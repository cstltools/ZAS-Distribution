# MST-006 — CompanyInformationEdit

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-006` |
| Module | `master-data` |
| File name | `CompanyInformationEdit.aspx` |
| Route | `/SInventory_UI/CompanyInformationEdit.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `CompanyInformationEdit.aspx.cs` |
| Code-behind class | `SInventory_UI_CompanyInformationEdit` |
| Markup lines | 425 |
| Code-behind lines | 148 |
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

- referenced from `SInventory_UI/CompanyInformationView.aspx`

## 5. Route

`/SInventory_UI/CompanyInformationEdit.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CompanyInformationEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CompanyInformationEdit.aspx.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |

## 7. UI Control Inventory

15 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ScriptManager1` | asp:ScriptManager | Edit |  |  |  |  |  | 16 |
| 2 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 18 |
| 3 | `companyInfoIdHiddenField` | asp:HiddenField | Company Information Edit |  |  |  |  |  | 30 |
| 4 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 37 |
| 5 | `groupDropDownList` | asp:DropDownList | Group Name |  |  | Enabled=false |  |  | 69 |
| 6 | `companyNameTextBox` | asp:TextBox | Company Name |  | True |  |  | companyNameTextBox_OnTextChanged | 87 |
| 7 | `cmpNameHiddenField` | asp:HiddenField | Company Name |  |  |  |  |  | 88 |
| 8 | `cmpShortHiddenField` | asp:HiddenField |  |  |  |  |  |  | 89 |
| 9 | `cmpShortNameTextBox` | asp:TextBox | Company Short Name |  | true |  |  | cmpShortNameTextBox_OnTextChanged | 105 |
| 10 | `addressNameTextBox` | asp:TextBox | Address |  |  |  |  |  | 123 |
| 11 | `contactNoTextBox` | asp:TextBox | Contact No |  |  |  |  |  | 142 |
| 12 | `faxNoTextBox` | asp:TextBox | Fax No |  |  |  |  |  | 161 |
| 13 | `remarksTextBox` | asp:TextBox | Remarks |  |  |  |  |  | 177 |
| 14 | `updateButton` | asp:Button |  | Update |  |  |  | updateButton_Click | 227 |
| 15 | `closeButton` | asp:Button |  | Close |  |  |  | closeButton_Click | 228 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDownList` | `aCompanyInfoBll.LoadGroupDropdownList(...)` | `CompanyInfoBLL.LoadGroupDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` | `CompanyInfoBLL.LoadGroupDropdownList` → `CompanyInfoDAL.GetGroupDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblGroupInformation` | — |
| `updateButton_Click` | `aCompanyInfoBll.UpdateDataForCompanyInfo(...)` | `CompanyInfoBLL.UpdateDataForCompanyInfo(CompanyInformation aCompanyInfo)` <br/>`Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` | `CompanyInfoBLL.UpdateDataForCompanyInfo` → `CompanyInfoDAL.UpdateCompanyInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblCompanyInfo` | — |
| `CompanyInfoLoad` | `aCompanyInfoBll.CompanyInfoEditLoad(...)` | `CompanyInfoBLL.CompanyInfoEditLoad(string companyInfoId)` <br/>`Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` | `CompanyInfoBLL.CompanyInfoEditLoad` → `CompanyInfoDAL.CompanyInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | — | `tblCompanyInfo` | — |
| `companyNameTextBox_OnTextChanged` | `aCompanyInfoBll.CheckCompanyInfoExistOrNot(...)` | `CompanyInfoBLL.CheckCompanyInfoExistOrNot(string companyName)` <br/>`Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` | `CompanyInfoBLL.CheckCompanyInfoExistOrNot` → `CompanyInfoDAL.GetCompanyInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo` | — |
| `cmpShortNameTextBox_OnTextChanged` | `aCompanyInfoBll.CheckCompanyShortNameExistOrNot(...)` | `CompanyInfoBLL.CheckCompanyShortNameExistOrNot(string cmpShortName)` <br/>`Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` | `CompanyInfoBLL.CheckCompanyShortNameExistOrNot` → `CompanyInfoDAL.GetCompanyShortNameInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `groupDropDownList` | Group Name | asp:DropDownList | False | — | `aCompanyInfoBll.LoadGroupDropdownList()` in `LoadDropDownList()` | `tblGroupInformation` | set in code (see the DAL method above) |

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
| `updateButton` | asp:Button | Update | — | `updateButton_Click` | 39 lines | `aCompanyInfoBll.UpdateDataForCompanyInfo` | — |
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
| 2 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 3 | `LoadDropDownList` | `void LoadDropDownList()` | 4 | `aCompanyInfoBll.LoadGroupDropdownList` | — | — | — |
| 4 | `updateButton_Click` | `void updateButton_Click(object sender, EventArgs e)` | 39 | `aCompanyInfoBll.UpdateDataForCompanyInfo` | — | — | `LoginName` |
| 5 | `CompanyInfoLoad` | `void CompanyInfoLoad(string comapnyInfoId)` | 15 | `aCompanyInfoBll.CompanyInfoEditLoad` | — | — | — |
| 6 | `closeButton_Click` | `void closeButton_Click(object sender, EventArgs e)` | 4 | — | — | — | — |
| 7 | `companyNameTextBox_OnTextChanged` | `void companyNameTextBox_OnTextChanged(object sender, EventArgs e)` | 9 | `aCompanyInfoBll.CheckCompanyInfoExistOrNot` | — | — | — |
| 8 | `cmpShortNameTextBox_OnTextChanged` | `void cmpShortNameTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `aCompanyInfoBll.CheckCompanyShortNameExistOrNot` | — | — | — |
| 9 | `GotoUpdatePermissionSetUp` | `void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, )` | 23 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
CompanyInformationEdit.aspx (markup)
  -> SInventory_UI_CompanyInformationEdit (code-behind event handler)
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
| `LoadDropDownList` | `CompanyInfoBLL` | `LoadGroupDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| `updateButton_Click` | `CompanyInfoBLL` | `UpdateDataForCompanyInfo` | `CompanyInformation aCompanyInfo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| `CompanyInfoLoad` | `CompanyInfoBLL` | `CompanyInfoEditLoad` | `string companyInfoId` | `CompanyInformation` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| `companyNameTextBox_OnTextChanged` | `CompanyInfoBLL` | `CheckCompanyInfoExistOrNot` | `string companyName` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| `cmpShortNameTextBox_OnTextChanged` | `CompanyInfoBLL` | `CheckCompanyShortNameExistOrNot` | `string cmpShortName` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `CompanyInfoLoad()` → `CompanyInfoBLL.CompanyInfoEditLoad` → `CompanyInfoDAL.CompanyInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `cmpShortNameTextBox_OnTextChanged()` → `CompanyInfoBLL.CheckCompanyShortNameExistOrNot` → `CompanyInfoDAL.GetCompanyShortNameInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `companyNameTextBox_OnTextChanged()` → `CompanyInfoBLL.CheckCompanyInfoExistOrNot` → `CompanyInfoDAL.GetCompanyInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `updateButton_Click()` → `CompanyInfoBLL.UpdateDataForCompanyInfo` → `CompanyInfoDAL.UpdateCompanyInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblGroupInformation` | `LoadDropDownList()` → `CompanyInfoBLL.LoadGroupDropdownList` → `CompanyInfoDAL.GetGroupDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `updateButton_Click` | Data Not Update!!! | No | Yes | UNKNOWN | Business rule |
| `updateButton_Click` | Data Update Successfully!!! Please Reload | No | Yes | UNKNOWN | Required |
| `updateButton_Click` | Please input data in all Textbox | No | Yes | UNKNOWN | Required |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-006-001` | {0} | Code | `Solution.Web/SInventory_UI/CompanyInformationEdit.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-MST-006-002` | Data Not Update!!! | Code | `Solution.Web/SInventory_UI/CompanyInformationEdit.aspx.cs` → `updateButton_Click()` | user action on this page | operation is refused | "Data Not Update!!!" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblGroupInformation` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-006-001` | Unauthenticated access | Request `/SInventory_UI/CompanyInformationEdit.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-MST-006-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-MST-006-003` | Validation: Data Not Update!!! | Trigger `updateButton_Click()` in the state that produces this message | alert "Data Not Update!!!" and the operation is not performed | CONFIRMED |
| `TC-MST-006-004` | Validation: Data Update Successfully!!! Please Reloa | Trigger `updateButton_Click()` in the state that produces this message | alert "Data Update Successfully!!! Please Reload" and the operation is not performed | CONFIRMED |
| `TC-MST-006-005` | Validation: Please input data in all Textbox | Trigger `updateButton_Click()` in the state that produces this message | alert "Please input data in all Textbox" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CompanyInformationEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CompanyInformationEdit.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
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
| Tables | absent | 2 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` is **shared with 53 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblGroupInformation` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
