# RPT-109 — CustomerReport

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RPT-109` |
| Module | `reports` |
| File name | `CustomerReport.aspx` |
| Route | `/SInventory_UI/CustomerReport.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `CustomerReport.aspx.cs` |
| Code-behind class | `SInventory_UI_CustomerReport` |
| Markup lines | 308 |
| Code-behind lines | 123 |
| Server controls | 9 |
| GridViews | 0 |
| Code-behind methods | 9 |

## 2. Purpose

Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/CustomerReport.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerReport.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerReport.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/TerritoryInfoBll.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |

## 7. UI Control Inventory

9 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 5 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `regionDropDownList` | asp:DropDownList | Region Name: |  | True |  |  | regionDropDownList_OnTextChanged | 67 |
| 5 | `areaDropDownList` | asp:DropDownList | Area Name: |  | true |  |  | areaDropDownList_OnSelectedIndexChanged | 87 |
| 6 | `territoryDropDownList` | asp:DropDownList | Territory Name: |  | True |  |  | territoryDropDownList_OnSelectedIndexChanged | 107 |
| 7 | `marketDropDownList` | asp:DropDownList | Market Name: |  |  |  |  |  | 127 |
| 8 | `submitButton` | asp:Button |  | View Report |  |  |  | submitButton_Click | 169 |
| 9 | `cancelButton` | asp:Button |  | Cancel |  |  |  | cancelButton_Click | 170 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropdownList` | `aReportDal.LoadRegionName(...)` | `AreaDAL.LoadRegionName(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/AreaDAL.cs` | `AreaDAL.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionname` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerReportDal.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblRegion` | — |
| `regionDropDownList_OnTextChanged` | `aReportDal.LoadAreaOnDropdownList(...)` | `CustomerReportDal.LoadAreaOnDropdownList(DropDownList ddl, string regionId)` <br/>`Library.DAL/SInventory_DAL/CustomerReportDal.cs` | `CustomerReportDal.LoadAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadAreaOnDropdownList` → `CustomerMasterDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `MarketInfoBLL.LoadAreaOnDropdownList` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `areaDropDownList_OnSelectedIndexChanged` | `aReportDal.LoadTerritoryOnDropdownList(...)` | `CustomerReportDal.LoadTerritoryOnDropdownList(DropDownList ddl, string areaId)` <br/>`Library.DAL/SInventory_DAL/CustomerReportDal.cs` | `CustomerReportDal.LoadTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadTerritoryOnDropdownList` → `CustomerMasterDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustPaymentBLL.LoadTerritoryOnDropdownList` → `CustPaymentDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblTerritory` | — |
| `territoryDropDownList_OnSelectedIndexChanged` | `aReportDal.LoadMarketOnDropdownList(...)` | `CustomerReportDal.LoadMarketOnDropdownList(DropDownList ddl, string territoryId)` <br/>`Library.DAL/SInventory_DAL/CustomerReportDal.cs` | `CustomerReportDal.LoadMarketOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadMarketOnDropdownList` → `CustomerMasterDAL.GetMarketOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblMarket` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `regionDropDownList` | Region Name: | asp:DropDownList | True | — | `aReportDal.LoadAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `aReportDal.LoadRegionName()` in `LoadDropdownList()` | `tblArea`, `tblRegion` | set in code (see the DAL method above) |
| `areaDropDownList` | Area Name: | asp:DropDownList | true | areaDropDownList_OnSelectedIndexChanged | `aReportDal.LoadAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `aReportDal.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblArea`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory Name: | asp:DropDownList | True | territoryDropDownList_OnSelectedIndexChanged | `aReportDal.LoadMarketOnDropdownList()` in `territoryDropDownList_OnSelectedIndexChanged()`; `aReportDal.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblMarket`, `tblTerritory` | set in code (see the DAL method above) |
| `marketDropDownList` | Market Name: | asp:DropDownList | False | — | `aReportDal.LoadMarketOnDropdownList()` in `territoryDropDownList_OnSelectedIndexChanged()` | `tblMarket` | set in code (see the DAL method above) |

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
| `submitButton` | asp:Button | View Report | — | `submitButton_Click` | 3 lines | — | — |
| `cancelButton` | asp:Button | Cancel | — | `cancelButton_Click` | 6 lines | — | — |

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
| 2 | `LoadDropdownList` | `void LoadDropdownList()` | 3 | `aReportDal.LoadRegionName` | — | — | — |
| 3 | `regionDropDownList_OnTextChanged` | `void regionDropDownList_OnTextChanged(object sender, EventArgs e)` | 12 | `aReportDal.LoadAreaOnDropdownList` | — | — | — |
| 4 | `areaDropDownList_OnSelectedIndexChanged` | `void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 11 | `aReportDal.LoadTerritoryOnDropdownList` | — | — | — |
| 5 | `territoryDropDownList_OnSelectedIndexChanged` | `void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 10 | `aReportDal.LoadMarketOnDropdownList` | — | — | — |
| 6 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 7 | `PopupReport` | `void PopupReport()` | 11 | — | — | — | `PRAM` |
| 8 | `GetPrameterList` | `string GetPrameterList()` | 29 | — | — | — | — |
| 9 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 6 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
CustomerReport.aspx (markup)
  -> SInventory_UI_CustomerReport (code-behind event handler)
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
| `LoadDropdownList` | `CustomerMasterBLL` | `LoadRegionname` | `DropDownList ddl,string id` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `LoadDropdownList` | `MarketInfoBLL` | `LoadRegionName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `regionDropDownList_OnTextChanged` | `CustomerMasterBLL` | `LoadAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `regionDropDownList_OnTextChanged` | `MarketInfoBLL` | `LoadAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `regionDropDownList_OnTextChanged` | `TerritoryInfoBll` | `LoadAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/TerritoryInfoBll.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `CustomerMasterBLL` | `LoadTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `CustPaymentBLL` | `LoadTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `MarketInfoBLL` | `LoadTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `territoryDropDownList_OnSelectedIndexChanged` | `CustomerMasterBLL` | `LoadMarketOnDropdownList` | `DropDownList ddl, string ttrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropdownList` | `AreaDAL` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| `LoadDropdownList` | `CustomerMasterDAL` | `LoadRegionname` | `DropDownList ddl, string id` | `void` | — | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `LoadDropdownList` | `CustomerMasterDAL` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| `LoadDropdownList` | `CustomerReportDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| `LoadDropdownList` | `DistributionCenterInfoDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| `LoadDropdownList` | `FieldForceSummeryDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropdownList` | `MarketInfoDAL` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| `LoadDropdownList` | `TerritoryInfoDal` | `LoadRegionName` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| `regionDropDownList_OnTextChanged` | `CustomerReportDal` | `LoadAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `CustomerReportDal` | `LoadTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| `territoryDropDownList_OnSelectedIndexChanged` | `CustomerReportDal` | `LoadMarketOnDropdownList` | `DropDownList ddl, string territoryId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `regionDropDownList_OnTextChanged()` → `CustomerReportDal.LoadAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadAreaOnDropdownList` → `CustomerMasterDAL.GetAreaOnDropdownList` |
| `tblMarket` | `territoryDropDownList_OnSelectedIndexChanged()` → `CustomerReportDal.LoadMarketOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadMarketOnDropdownList` → `CustomerMasterDAL.GetMarketOnDropdownList` |
| `tblRegion` | `LoadDropdownList()` → `AreaDAL.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionname` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblTerritory` | `areaDropDownList_OnSelectedIndexChanged()` → `CustomerReportDal.LoadTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterBLL.LoadTerritoryOnDropdownList` → `CustomerMasterDAL.GetTerritoryOnDropdownList` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

_No validation logic detected on this page._

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

_No message-bearing rule found in this page's own code._

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `PRAM`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/TerritoryInfoBll.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Database object | `tblArea` |
| Database object | `tblMarket` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RPT-109-001` | Unauthenticated access | Request `/SInventory_UI/CustomerReport.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerReport.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerReport.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/TerritoryInfoBll.cs` |
| Layer source | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 9 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 9 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 4 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` is **shared with 10 other page(s)** (e.g. `CustomerMasterViewer.aspx`, `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `CustMasterView.aspx`, `CustomerMasterEdit.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` is **shared with 4 other page(s)** (e.g. `FieldForceSalesSummery.aspx`, `MarketInfoEdit.aspx`, `MarketInfoEntry.aspx`, `MarketInfoView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/AreaDAL.cs` is **shared with 3 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `FieldForceSalesSummery.aspx`, `WeaightedAVG_Report.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` is **shared with 3 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `FieldForceSalesSummery.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerReportDal.cs` is **shared with 2 other page(s)** (e.g. `CustomerReportViewer.aspx`, `FieldForceSalesSummery.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` is **shared with 7 other page(s)** (e.g. `AuditReportOne.aspx`, `CustomerCreditView.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`, `DistributionCenterInformation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` is **shared with 59 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `FieldForceSalesSummeryReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` is **shared with 1 other page(s)** (e.g. `FieldForceSalesSummery.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` is **shared with 2 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `FieldForceSalesSummery.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
