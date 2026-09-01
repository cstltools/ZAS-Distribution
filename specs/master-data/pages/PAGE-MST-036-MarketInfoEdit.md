# MST-036 — MarketInfoEdit

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-036` |
| Module | `master-data` |
| File name | `MarketInfoEdit.aspx` |
| Route | `/SInventory_UI/MarketInfoEdit.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `MarketInfoEdit.aspx.cs` |
| Code-behind class | `SInventory_UI_MarketInfoEdit` |
| Markup lines | 450 |
| Code-behind lines | 197 |
| Server controls | 18 |
| GridViews | 0 |
| Code-behind methods | 12 |

## 2. Purpose

Edit screen. It loads an existing record by its key (usually from Session or query string) and persists the modified values.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Edit screen. It loads an existing record by its key (usually from Session or query string) and persists the modified values.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/HREmployeeSysnc.aspx`
- referenced from `SInventory_UI/MarketInfoView.aspx`

## 5. Route

`/SInventory_UI/MarketInfoEdit.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MarketInfoEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MarketInfoEdit.aspx.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |

## 7. UI Control Inventory

18 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ScriptManager1` | asp:ScriptManager | Edit |  |  |  |  |  | 16 |
| 2 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 18 |
| 3 | `marketInfoIdHiddenField` | asp:HiddenField | Market Information Edit |  |  |  |  |  | 29 |
| 4 | `codeHiddenField` | asp:HiddenField | Market Information Edit |  |  |  |  |  | 30 |
| 5 | `nameHiddenField` | asp:HiddenField | Market Information Edit |  |  |  |  |  | 31 |
| 6 | `shortNameHiddenField` | asp:HiddenField |  |  |  |  |  |  | 32 |
| 7 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 39 |
| 8 | `regionDropDownList` | asp:DropDownList | Region Name |  |  |  |  | regionDropDownList_OnTextChanged | 56 |
| 9 | `areaDropDownList` | asp:DropDownList | Area Name |  | true |  |  | areaDropDownList_OnSelectedIndexChanged | 75 |
| 10 | `territoryDropDownList` | asp:DropDownList | Territory Name |  |  |  |  |  | 94 |
| 11 | `shortNameTextBox` | asp:TextBox | Market Short Name: |  | true |  |  | shortNameTextBox_OnTextChanged | 112 |
| 12 | `marketCodeTextBox` | asp:TextBox | Market Code |  | true |  |  | marketCodeTextBox_OnTextChanged | 129 |
| 13 | `marketnameTextBox` | asp:TextBox | Market Name |  | true |  |  | marketnameTextBox_OnTextChanged | 145 |
| 14 | `descriptionTextBox` | asp:TextBox | Description: |  |  |  |  |  | 162 |
| 15 | `remarksTextBox` | asp:TextBox | Remarks: |  |  |  |  |  | 179 |
| 16 | `isActiveCheckBox` | asp:CheckBox | Is Active: |  |  |  |  |  | 199 |
| 17 | `updateButton` | asp:Button |  | Update |  |  |  | updateButton_Click | 296 |
| 18 | `closeButton` | asp:Button |  | Close |  |  |  | closeButton_Click | 298 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropdownList` | `MarketInfoBLL.LoadRegionDropdownList(...)` | `MarketInfoBLL.LoadRegionDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadRegionDropdownList` | — | — | — |
| `updateButton_Click` | `aMarketInfoBLL.UpdateDataForMarketInfo(...)` | `MarketInfoBLL.UpdateDataForMarketInfo(MarketInfo aMarketInfo)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.UpdateDataForMarketInfo` → `MarketInfoDAL.UpdateMarketInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblMarket` | — |
| `MarketInfoLoad` | `MarketInfoBLL.LoadAreaOnDropdownList(...)` | `MarketInfoBLL.LoadAreaOnDropdownList(DropDownList ddl, string regionId)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadAreaOnDropdownList` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `MarketInfoLoad` | `MarketInfoBLL.LoadTerritoryOnDropdownList(...)` | `MarketInfoBLL.LoadTerritoryOnDropdownList(DropDownList ddl, string areaId)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadTerritoryOnDropdownList` → `MarketInfoDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblTerritory` | — |
| `MarketInfoLoad` | `MarketInfoBLL.MarketInfoEditLoad(...)` | `MarketInfoBLL.MarketInfoEditLoad(string MarketInfoId)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.MarketInfoEditLoad` → `MarketInfoDAL.MarketInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblArea`, `tblMarket`, `tblRegion`, `tblTerritory` | — |
| `regionDropDownList_OnTextChanged` | `MarketInfoBLL.LoadAreaOnDropdownList(...)` | `MarketInfoBLL.LoadAreaOnDropdownList(DropDownList ddl, string regionId)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadAreaOnDropdownList` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `areaDropDownList_OnSelectedIndexChanged` | `MarketInfoBLL.LoadTerritoryOnDropdownList(...)` | `MarketInfoBLL.LoadTerritoryOnDropdownList(DropDownList ddl, string areaId)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.LoadTerritoryOnDropdownList` → `MarketInfoDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblTerritory` | — |
| `marketnameTextBox_OnTextChanged` | `MarketInfoBLL.NameExistOrNot(...)` | `MarketInfoBLL.NameExistOrNot(string name)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.NameExistOrNot` → `MarketInfoDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblMarket` | — |
| `marketCodeTextBox_OnTextChanged` | `MarketInfoBLL.CodeExistOrNot(...)` | `MarketInfoBLL.CodeExistOrNot(string code)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.CodeExistOrNot` → `MarketInfoDAL.CodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblMarket` | — |
| `shortNameTextBox_OnTextChanged` | `MarketInfoBLL.ShortNameExistOrNot(...)` | `MarketInfoBLL.ShortNameExistOrNot(string srtName)` <br/>`Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | `MarketInfoBLL.ShortNameExistOrNot` → `MarketInfoDAL.ShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | — | `tblMarket` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `regionDropDownList` | Region Name | asp:DropDownList | False | — | `MarketInfoBLL.LoadAreaOnDropdownList()` in `MarketInfoLoad()`; `MarketInfoBLL.LoadAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `MarketInfoBLL.LoadRegionDropdownList()` in `LoadDropdownList()` | `tblArea` | set in code (see the DAL method above) |
| `areaDropDownList` | Area Name | asp:DropDownList | true | areaDropDownList_OnSelectedIndexChanged | `MarketInfoBLL.LoadAreaOnDropdownList()` in `MarketInfoLoad()`; `MarketInfoBLL.LoadAreaOnDropdownList()` in `regionDropDownList_OnTextChanged()`; `MarketInfoBLL.LoadTerritoryOnDropdownList()` in `MarketInfoLoad()`; `MarketInfoBLL.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblArea`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory Name | asp:DropDownList | False | — | `MarketInfoBLL.LoadTerritoryOnDropdownList()` in `MarketInfoLoad()`; `MarketInfoBLL.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblTerritory` | set in code (see the DAL method above) |

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
| `updateButton` | asp:Button | Update | — | `updateButton_Click` | 40 lines | `aMarketInfoBLL.UpdateDataForMarketInfo` | — |
| `closeButton` | asp:Button | Close | — | `closeButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 9 | — | — | — | — |
| 2 | `LoadDropdownList` | `void LoadDropdownList()` | 3 | `MarketInfoBLL.LoadRegionDropdownList` | — | — | — |
| 3 | `updateButton_Click` | `void updateButton_Click(object sender, EventArgs e)` | 40 | `aMarketInfoBLL.UpdateDataForMarketInfo` | — | — | `LoginName` |
| 4 | `MarketInfoLoad` | `void MarketInfoLoad(string marketId)` | 40 | `MarketInfoBLL.LoadAreaOnDropdownList`, `MarketInfoBLL.LoadTerritoryOnDropdownList`, `MarketInfoBLL.MarketInfoEditLoad` | — | — | — |
| 5 | `regionDropDownList_OnTextChanged` | `void regionDropDownList_OnTextChanged(object sender, EventArgs e)` | 10 | `MarketInfoBLL.LoadAreaOnDropdownList` | — | — | — |
| 6 | `areaDropDownList_OnSelectedIndexChanged` | `void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 10 | `MarketInfoBLL.LoadTerritoryOnDropdownList` | — | — | — |
| 7 | `closeButton_Click` | `void closeButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 8 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 9 | `marketnameTextBox_OnTextChanged` | `void marketnameTextBox_OnTextChanged(object sender, EventArgs e)` | 5 | `MarketInfoBLL.NameExistOrNot` | — | — | — |
| 10 | `marketCodeTextBox_OnTextChanged` | `void marketCodeTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `MarketInfoBLL.CodeExistOrNot` | — | — | — |
| 11 | `GotoUpdatePermissionSetUp` | `void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, )` | 23 | — | — | — | — |
| 12 | `shortNameTextBox_OnTextChanged` | `void shortNameTextBox_OnTextChanged(object sender, EventArgs e)` | 4 | `MarketInfoBLL.ShortNameExistOrNot` | — | — | — |

Call chain shape for every persisting action on this page:

```
MarketInfoEdit.aspx (markup)
  -> SInventory_UI_MarketInfoEdit (code-behind event handler)
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
| `updateButton_Click` | `MarketInfoBLL` | `UpdateDataForMarketInfo` | `MarketInfo aMarketInfo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `MarketInfoLoad` | `MarketInfoBLL` | `LoadAreaOnDropdownList` | `DropDownList ddl, string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `MarketInfoLoad` | `MarketInfoBLL` | `LoadTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| `MarketInfoLoad` | `MarketInfoBLL` | `MarketInfoEditLoad` | `string MarketInfoId` | `MarketInfo` | — | — | — | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
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
| `tblArea` | `MarketInfoLoad()` → `MarketInfoBLL.LoadAreaOnDropdownList` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `MarketInfoLoad()` → `MarketInfoBLL.MarketInfoEditLoad` → `MarketInfoDAL.MarketInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `regionDropDownList_OnTextChanged()` → `MarketInfoBLL.LoadAreaOnDropdownList` → `MarketInfoDAL.GetAreaOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblMarket` | `MarketInfoLoad()` → `MarketInfoBLL.MarketInfoEditLoad` → `MarketInfoDAL.MarketInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `marketCodeTextBox_OnTextChanged()` → `MarketInfoBLL.CodeExistOrNot` → `MarketInfoDAL.CodeExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketnameTextBox_OnTextChanged()` → `MarketInfoBLL.NameExistOrNot` → `MarketInfoDAL.NameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `shortNameTextBox_OnTextChanged()` → `MarketInfoBLL.ShortNameExistOrNot` → `MarketInfoDAL.ShortNameExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `updateButton_Click()` → `MarketInfoBLL.UpdateDataForMarketInfo` → `MarketInfoDAL.UpdateMarketInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblRegion` | `MarketInfoLoad()` → `MarketInfoBLL.MarketInfoEditLoad` → `MarketInfoDAL.MarketInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` |
| `tblTerritory` | `MarketInfoLoad()` → `MarketInfoBLL.LoadTerritoryOnDropdownList` → `MarketInfoDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `MarketInfoLoad()` → `MarketInfoBLL.MarketInfoEditLoad` → `MarketInfoDAL.MarketInfoEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `areaDropDownList_OnSelectedIndexChanged()` → `MarketInfoBLL.LoadTerritoryOnDropdownList` → `MarketInfoDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |

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
| `BR-MST-036-001` | Data Not Update!!! | Code | `Solution.Web/SInventory_UI/MarketInfoEdit.aspx.cs` → `updateButton_Click()` | user action on this page | operation is refused | "Data Not Update!!!" | CONFIRMED |
| `BR-MST-036-002` | {0} | Code | `Solution.Web/SInventory_UI/MarketInfoEdit.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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

Audit columns written by this page: `UpdateBy`

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
| Assembly source | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` |
| Database object | `tblArea` |
| Database object | `tblMarket` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-036-001` | Unauthenticated access | Request `/SInventory_UI/MarketInfoEdit.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-MST-036-002` | Validation: Data Not Update!!! | Trigger `updateButton_Click()` in the state that produces this message | alert "Data Not Update!!!" and the operation is not performed | CONFIRMED |
| `TC-MST-036-003` | Validation: Data Update Successfully!!! Please Reloa | Trigger `updateButton_Click()` in the state that produces this message | alert "Data Update Successfully!!! Please Reload" and the operation is not performed | CONFIRMED |
| `TC-MST-036-004` | Validation: Please input data in all Textbox | Trigger `updateButton_Click()` in the state that produces this message | alert "Please input data in all Textbox" and the operation is not performed | CONFIRMED |
| `TC-MST-036-005` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MarketInfoEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MarketInfoEdit.aspx.cs` |
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
| Backend flow | absent | 12 methods traced | n/a | **MISSING → now documented** | section 12 |
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
- This page does not inherit the master page, so it has **no authentication guard** (`GAP-SEC-002`).

## 31. Change Impact

Changing this page affects:

- `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` is **shared with 4 other page(s)** (e.g. `CustomerReport.aspx`, `FieldForceSalesSummery.aspx`, `MarketInfoEntry.aspx`, `MarketInfoView.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
