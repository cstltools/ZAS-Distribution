# MST-019 — DeliveryPersonEntry

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-019` |
| Module | `master-data` |
| File name | `DeliveryPersonEntry.aspx` |
| Route | `/SInventory_UI/DeliveryPersonEntry.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `DeliveryPersonEntry.aspx.cs` |
| Code-behind class | `SInventory_UI_DeliveryPersonEntry` |
| Markup lines | 335 |
| Code-behind lines | 333 |
| Server controls | 22 |
| GridViews | 0 |
| Code-behind methods | 15 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/DeliveryPersonView.aspx`

Outbound `Response.Redirect` targets: `DeliveryPersonView.aspx`

## 5. Route

`/SInventory_UI/DeliveryPersonEntry.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DeliveryPersonEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DeliveryPersonEntry.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |

## 7. UI Control Inventory

22 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 15 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 16 |
| 4 | `departmentListImageButton` | asp:ImageButton | View List | ~/images/viewList.png |  |  |  | departmentListImageButton_Click | 44 |
| 5 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 50 |
| 6 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_SelectedIndexChanged | 66 |
| 7 | `empIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 71 |
| 8 | `empDropDownList` | asp:DropDownList | Employee: |  | True |  |  | empDropDownList_OnSelectedIndexChanged | 88 |
| 9 | `delNIDTextBox` | asp:label | Delivery Man NID: |  |  |  |  |  | 104 |
| 10 | `delInfoIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 116 |
| 11 | `delNameTextBox` | asp:label | Delivery Man Name: |  |  |  |  |  | 121 |
| 12 | `deladdTextBox` | asp:label | Present Address: |  |  |  |  |  | 136 |
| 13 | `presentCityLabel` | asp:label | Present City: |  |  |  |  |  | 152 |
| 14 | `delPhoneTextBox` | asp:label | Phone No: |  |  |  |  |  | 168 |
| 15 | `delEmailTextBox` | asp:label | Email: |  |  |  |  |  | 186 |
| 16 | `delContactNoTextBox` | asp:label | Emergencey Contact No: |  |  |  |  |  | 205 |
| 17 | `delrefNameTextBox` | asp:label | Referance Name: |  |  |  |  |  | 222 |
| 18 | `delrefphoneTextBox` | asp:label | Referance Phone: |  |  |  |  |  | 237 |
| 19 | `isActiveCheckBox` | asp:CheckBox | Is Active: |  |  |  |  |  | 257 |
| 20 | `remarksTextBox` | asp:TextBox | Remarks: |  |  |  |  |  | 275 |
| 21 | `Button1` | asp:Button |  | Submit |  |  |  | Button1_Click | 307 |
| 22 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 308 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `DeliveryPersonDAO`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `DropDownList` | `aDeliveryPersonBll.EmpLoad(...)` | `DeliveryPersonBLL.EmpLoad(DropDownList aDropDownList, string companyId)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.EmpLoad` → `DeliveryPersonDAL.EmpLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblEmpGeneralInfo` | — |
| `DropDownList` | `aDeliveryPersonBll.LoadCompanyDropdownList(...)` | `DeliveryPersonBLL.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.LoadCompanyDropdownList` | — | — | — |
| `GetDate` | `aDeliveryPersonBll.EmpLoad(...)` | `DeliveryPersonBLL.EmpLoad(DropDownList aDropDownList, string companyId)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.EmpLoad` → `DeliveryPersonDAL.EmpLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblEmpGeneralInfo` | — |
| `GetDate` | `aDeliveryPersonBll.LoadDelivaryManById(...)` | `DeliveryPersonBLL.LoadDelivaryManById(string delId)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.LoadDelivaryManById` → `DeliveryPersonDAL.LoadDelivaryManById` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — |
| `Save` | `aDeliveryPersonBll.SaveDeliveryPerson(...)` | `DeliveryPersonBLL.SaveDeliveryPerson(DeliveryPersonDAO aDeliveryPersonDao)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.SaveDeliveryPerson` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `DeliveryPersonDAL.SaveDeliveryPerson` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `DeliveryPersonBLL.CodeGenerator` | INSERT, SELECT | `tblDeliveryManInfo` | — |
| `Update` | `aDeliveryPersonBll.UpdateDeliveryInfo(...)` | `DeliveryPersonBLL.UpdateDeliveryInfo(DeliveryPersonDAO aDeliveryPersonDao)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.UpdateDeliveryInfo` → `DeliveryPersonDAL.UpdateDeliveryInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDeliveryManInfo` | — |
| `companyNameDropDownList_SelectedIndexChanged` | `aDeliveryPersonBll.EmpLoad(...)` | `DeliveryPersonBLL.EmpLoad(DropDownList aDropDownList, string companyId)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.EmpLoad` → `DeliveryPersonDAL.EmpLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblEmpGeneralInfo` | — |
| `empDropDownList_OnSelectedIndexChanged` | `aDeliveryPersonBll.LoadEmpInfoById(...)` | `DeliveryPersonBLL.LoadEmpInfoById(string empId)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.LoadEmpInfoById` → `DeliveryPersonDAL.GetEmpInfoById` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblEmpGeneralInfo` | — |
| `ExistOrNot` | `aDeliveryPersonBll.LoadSRInfoById(...)` | `DeliveryPersonBLL.LoadSRInfoById(string empId)` <br/>`Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | `DeliveryPersonBLL.LoadSRInfoById` → `DeliveryPersonDAL.GetSRInfoById` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDeliveryManInfo` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | companyNameDropDownList_SelectedIndexChanged | `aDeliveryPersonBll.EmpLoad()` in `companyNameDropDownList_SelectedIndexChanged()`; `aDeliveryPersonBll.LoadCompanyDropdownList()` in `DropDownList()` | `tblEmpGeneralInfo` | set in code (see the DAL method above) |
| `empDropDownList` | Employee: | asp:DropDownList | True | empDropDownList_OnSelectedIndexChanged | `aDeliveryPersonBll.EmpLoad()` in `DropDownList()`; `aDeliveryPersonBll.EmpLoad()` in `GetDate()`; `aDeliveryPersonBll.EmpLoad()` in `companyNameDropDownList_SelectedIndexChanged()`; `aDeliveryPersonBll.LoadEmpInfoById()` in `empDropDownList_OnSelectedIndexChanged()` | `tblDeliveryManInfo`, `tblEmpGeneralInfo` | set in code (see the DAL method above) |

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
| `departmentListImageButton` | asp:ImageButton |  | — | `departmentListImageButton_Click` | 3 lines | — | — |
| `Button1` | asp:Button | Submit | — | `Button1_Click` | 11 lines | — | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 15 | — | — | — | `DeliveryManId` |
| 2 | `SetActiveStatus` | `void SetActiveStatus()` | 6 | — | — | — | — |
| 3 | `DropDownList` | `void DropDownList()` | 5 | `aDeliveryPersonBll.EmpLoad`, `aDeliveryPersonBll.LoadCompanyDropdownList` | — | — | — |
| 4 | `GetDate` | `void GetDate(string id)` | 38 | `aDeliveryPersonBll.EmpLoad`, `aDeliveryPersonBll.LoadDelivaryManById` | — | — | — |
| 5 | `Clear` | `void Clear()` | 20 | — | — | — | — |
| 6 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 7 | `Validation` | `bool Validation()` | 20 | — | — | — | — |
| 8 | `Save` | `void Save()` | 34 | `aDeliveryPersonBll.SaveDeliveryPerson` | — | — | `LoginName` |
| 9 | `Update` | `void Update()` | 36 | `aDeliveryPersonBll.UpdateDeliveryInfo` | — | — | `LoginName` |
| 10 | `Button1_Click` | `void Button1_Click(object sender, EventArgs e)` | 11 | — | — | — | — |
| 11 | `departmentListImageButton_Click` | `void departmentListImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 12 | `companyNameDropDownList_SelectedIndexChanged` | `void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 11 | `aDeliveryPersonBll.EmpLoad` | — | — | — |
| 13 | `empDropDownList_OnSelectedIndexChanged` | `void empDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 41 | `aDeliveryPersonBll.LoadEmpInfoById` | — | — | — |
| 14 | `ExistOrNot` | `bool ExistOrNot(string mode)` | 45 | `aDeliveryPersonBll.LoadSRInfoById` | — | — | — |
| 15 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
DeliveryPersonEntry.aspx (markup)
  -> SInventory_UI_DeliveryPersonEntry (code-behind event handler)
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
| `DropDownList` | `DeliveryPersonBLL` | `EmpLoad` | `DropDownList aDropDownList, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `DropDownList` | `DeliveryPersonBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `GetDate` | `DeliveryPersonBLL` | `EmpLoad` | `DropDownList aDropDownList, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `GetDate` | `DeliveryPersonBLL` | `LoadDelivaryManById` | `string delId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `Save` | `DeliveryPersonBLL` | `SaveDeliveryPerson` | `DeliveryPersonDAO aDeliveryPersonDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `Update` | `DeliveryPersonBLL` | `UpdateDeliveryInfo` | `DeliveryPersonDAO aDeliveryPersonDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `DeliveryPersonBLL` | `EmpLoad` | `DropDownList aDropDownList, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `empDropDownList_OnSelectedIndexChanged` | `DeliveryPersonBLL` | `LoadEmpInfoById` | `string empId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `ExistOrNot` | `DeliveryPersonBLL` | `LoadSRInfoById` | `string empId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `GetDate()` → `DeliveryPersonBLL.LoadDelivaryManById` → `DeliveryPersonDAL.LoadDelivaryManById` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `ExistOrNot()` → `DeliveryPersonBLL.LoadSRInfoById` → `DeliveryPersonDAL.GetSRInfoById` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetDate()` → `DeliveryPersonBLL.LoadDelivaryManById` → `DeliveryPersonDAL.LoadDelivaryManById` → `ClsCommonInternalDAL.DataContainerDataTable`, `Save()` → `DeliveryPersonBLL.SaveDeliveryPerson` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `DeliveryPersonDAL.SaveDeliveryPerson`, `Update()` → `DeliveryPersonBLL.UpdateDeliveryInfo` → `DeliveryPersonDAL.UpdateDeliveryInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblEmpGeneralInfo` | `DropDownList()` → `DeliveryPersonBLL.EmpLoad` → `DeliveryPersonDAL.EmpLoad` → `ClsCommonInternalDAL.LoadDropDownValue`, `GetDate()` → `DeliveryPersonBLL.EmpLoad` → `DeliveryPersonDAL.EmpLoad` → `ClsCommonInternalDAL.LoadDropDownValue`, `GetDate()` → `DeliveryPersonBLL.LoadDelivaryManById` → `DeliveryPersonDAL.LoadDelivaryManById` → `ClsCommonInternalDAL.DataContainerDataTable`, `companyNameDropDownList_SelectedIndexChanged()` → `DeliveryPersonBLL.EmpLoad` → `DeliveryPersonDAL.EmpLoad` → `ClsCommonInternalDAL.LoadDropDownValue`, `empDropDownList_OnSelectedIndexChanged()` → `DeliveryPersonBLL.LoadEmpInfoById` → `DeliveryPersonDAL.GetEmpInfoById` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `GetDate` | Update | No | Yes | UNKNOWN | Notification |
| `Clear` | Submit | No | Yes | UNKNOWN | Notification |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-MST-019-001` | Update | Code | `Solution.Web/SInventory_UI/DeliveryPersonEntry.aspx.cs` → `GetDate()` | user action on this page | operation is refused | "Update" | CONFIRMED |
| `BR-MST-019-002` | Submit | Code | `Solution.Web/SInventory_UI/DeliveryPersonEntry.aspx.cs` → `Clear()` | user action on this page | operation is refused | "Submit" | CONFIRMED |
| `BR-MST-019-003` | {0} | Code | `Solution.Web/SInventory_UI/DeliveryPersonEntry.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `DeliveryManId`, `LoginName`

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

Audit columns written by this page: `EntryBy`, `UpdateBy`

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
| Assembly source | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblEmpGeneralInfo` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-019-001` | Unauthenticated access | Request `/SInventory_UI/DeliveryPersonEntry.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-019-002` | Validation: Update | Trigger `GetDate()` in the state that produces this message | alert "Update" and the operation is not performed | CONFIRMED |
| `TC-MST-019-003` | Validation: Submit | Trigger `Clear()` in the state that produces this message | alert "Submit" and the operation is not performed | CONFIRMED |
| `TC-MST-019-004` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DeliveryPersonEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DeliveryPersonEntry.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 22 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 15 methods traced | n/a | **MISSING → now documented** | section 12 |
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

- `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` is **shared with 5 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `DeliveryPersonView.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
