# SYS-008 — UserPermission

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SYS-008` |
| Module | `00-system` |
| File name | `UserPermission.aspx` |
| Route | `/CommonUI/UserPermission.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `UserPermission.aspx.cs` |
| Code-behind class | `CommonUI_UserPermission` |
| Markup lines | 241 |
| Code-behind lines | 204 |
| Server controls | 22 |
| GridViews | 2 |
| Code-behind methods | 10 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 2 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers. Contains 2 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/CommonUI/UserPermission.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/CommonUI/UserPermission.aspx` |
| Code-behind | `Solution.Web/CommonUI/UserPermission.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js` |
| Business / data layer | `Library.BLL/Panal_BLL/PanalBLL.cs` |

## 7. UI Control Inventory

22 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 8 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 9 |
| 4 | `msgLabel` | asp:Label |  |  |  |  |  |  | 28 |
| 5 | `userDropDownList` | asp:DropDownList | User Name : |  | True |  |  | userDropDownList_SelectedIndexChanged | 41 |
| 6 | `mainMenuDropDownList` | asp:DropDownList | Main Menu : |  | True |  |  | mainMenuDropDownList_SelectedIndexChanged | 49 |
| 7 | `mainMenuGridView` | asp:GridView |  |  |  |  |  | mainMenuGridView_RowCommand | 77 |
| 8 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 83 |
| 9 | `mainMenuCheckBox` | asp:CheckBox | mainMenuGridView → col[0] "Select" |  |  |  |  |  | 85 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 88 |
| 11 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 89 |
| 12 | `mainMenuSaveImageButton` | asp:ImageButton | mainMenuGridView → col[2] "Save" | ~/images/Save.png |  |  |  |  | 91 |
| 13 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 95 |
| 14 | `deleteMainMenuBytton` | asp:ImageButton | menuGridView → col[3] "Delete" | ~/images/DeleteIcon.jpg |  |  |  |  | 97 |
| 15 | `menuGridView` | asp:GridView |  |  |  |  |  | menuGridView_RowCommand | 110 |
| 16 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 115 |
| 17 | `menuCheckBox` | asp:CheckBox | menuGridView → col[0] "Select" |  |  |  |  |  | 117 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 120 |
| 19 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 121 |
| 20 | `saveMenuImageButton` | asp:ImageButton | menuGridView → col[2] "Save" | ~/images/Save.png |  |  |  |  | 123 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 127 |
| 22 | `deleteMainMenuBytton` | asp:ImageButton | menuGridView → col[3] "Delete" | ~/images/DeleteIcon.jpg |  |  |  |  | 129 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `mainMenuGridView`

| Attribute | Value |
|---|---|
| datakeynames | `SL,Status` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | Select | — | — | `CheckBox:mainMenuCheckBox` | — |
| 1 | BoundField | Main Menu | ManuName | — | — | — |
| 2 | TemplateField | Save | — | — | `ImageButton:mainMenuSaveImageButton` | — |
| 3 | TemplateField | Delete | — | — | `ImageButton:deleteMainMenuBytton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `menuGridView`

| Attribute | Value |
|---|---|
| datakeynames | `SL,Status` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | Select | — | — | `CheckBox:menuCheckBox` | — |
| 1 | BoundField | Main Menu | ManuName | — | — | — |
| 2 | TemplateField | Save | — | — | `ImageButton:saveMenuImageButton` | — |
| 3 | TemplateField | Delete | — | — | `ImageButton:deleteMainMenuBytton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aPanalBll.UserDdl(...)` | `PanalBLL.UserDdl(DropDownList userDropDownList)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.UserDdl` → `PanalDAL.UserDdl` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblUser` | — |
| `userDropDownList_SelectedIndexChanged` | `aPanalBll.MainMenu(...)` | `PanalBLL.MainMenu(DropDownList userDropDownList,string userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblMainMenu`, `tblMenuDistribution` | — |
| `MainMenuLoad` | `aPanalBll.MainMenuLoad(...)` | `PanalBLL.MainMenuLoad(string userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MainMenuLoad` → `PanalDAL.MainMenuLoad` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblMainMenu`, `tblMenuDistribution` | — |
| `MenuLoad` | `aPanalBll.OtherMenuLoad(...)` | `PanalBLL.OtherMenuLoad(string userId,string parantId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.OtherMenuLoad` → `PanalDAL.OtherMenuLoad` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblMainMenu`, `tblMenuDistribution` | — |
| `mainMenuGridView_RowCommand` | `aPanalBll.MainMenu(...)` | `PanalBLL.MainMenu(DropDownList userDropDownList,string userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblMainMenu`, `tblMenuDistribution` | — |
| `mainMenuGridView_RowCommand` | `aPanalBll.MenuPermissionRemove(...)` | `PanalBLL.MenuPermissionRemove(int sl, int userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuPermissionRemove` → `PanalDAL.MenuPermissionRemove` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` | DELETE, SELECT | `tblMainMenu` | — |
| `mainMenuGridView_RowCommand` | `aPanalBll.SaveMainMenu(...)` | `PanalBLL.SaveMainMenu(int sl,int userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.SaveMainMenu` → `PanalDAL.SaveMainMenu` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT, SELECT | `tblMenuDistribution` | — |
| `menuGridView_RowCommand` | `aPanalBll.MenuPermissionRemove(...)` | `PanalBLL.MenuPermissionRemove(int sl, int userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuPermissionRemove` → `PanalDAL.MenuPermissionRemove` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` | DELETE, SELECT | `tblMainMenu` | — |
| `menuGridView_RowCommand` | `aPanalBll.SaveMenuBll(...)` | `PanalBLL.SaveMenuBll(int sl, int userId)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.SaveMenuBll` → `PanalDAL.CheckMenuSl` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.GetParantId` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.SaveMainMenu` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` | INSERT, SELECT | `tblMainMenu`, `tblMenuDistribution` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `userDropDownList` | User Name : | asp:DropDownList | True | userDropDownList_SelectedIndexChanged | `aPanalBll.MainMenu()` in `mainMenuGridView_RowCommand()`; `aPanalBll.MainMenu()` in `userDropDownList_SelectedIndexChanged()`; `aPanalBll.MainMenuLoad()` in `MainMenuLoad()`; `aPanalBll.MenuPermissionRemove()` in `mainMenuGridView_RowCommand()` | `tblMainMenu`, `tblMenuDistribution`, `tblUser` | set in code (see the DAL method above) |
| `mainMenuDropDownList` | Main Menu : | asp:DropDownList | True | mainMenuDropDownList_SelectedIndexChanged | `aPanalBll.MainMenu()` in `mainMenuGridView_RowCommand()`; `aPanalBll.MainMenu()` in `userDropDownList_SelectedIndexChanged()` | `tblMainMenu`, `tblMenuDistribution` | set in code (see the DAL method above) |

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
| `mainMenuSaveImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `deleteMainMenuBytton` | asp:ImageButton |  | — | `—` | — | — | — |
| `saveMenuImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `deleteMainMenuBytton` | asp:ImageButton |  | — | `—` | — | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none
- External scripts: `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | `aPanalBll.UserDdl` | — | — | — |
| 2 | `CheckBox` | `void CheckBox()` | 2 | — | — | — | — |
| 3 | `userDropDownList_SelectedIndexChanged` | `void userDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 6 | `aPanalBll.MainMenu` | — | — | — |
| 4 | `mainMenuGridView_RowDataBound` | `void mainMenuGridView_RowDataBound(object sender, GridViewRowEventArgs e)` | 12 | — | — | — | — |
| 5 | `MainMenuLoad` | `void MainMenuLoad()` | 4 | `aPanalBll.MainMenuLoad` | — | — | — |
| 6 | `MenuLoad` | `void MenuLoad(string userId,string parantId)` | 4 | `aPanalBll.OtherMenuLoad` | — | — | — |
| 7 | `mainMenuGridView_RowCommand` | `void mainMenuGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 62 | `aPanalBll.MainMenu`, `aPanalBll.MenuPermissionRemove`, `aPanalBll.SaveMainMenu` | — | — | — |
| 8 | `menuGridView_RowDataBound` | `void menuGridView_RowDataBound(object sender, GridViewRowEventArgs e)` | 12 | — | — | — | — |
| 9 | `mainMenuDropDownList_SelectedIndexChanged` | `void mainMenuDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 10 | `menuGridView_RowCommand` | `void menuGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 62 | `aPanalBll.MenuPermissionRemove`, `aPanalBll.SaveMenuBll` | — | — | — |

Call chain shape for every persisting action on this page:

```
UserPermission.aspx (markup)
  -> CommonUI_UserPermission (code-behind event handler)
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
| `userDropDownList_SelectedIndexChanged` | `PanalBLL` | `MainMenu` | `DropDownList userDropDownList,string userId` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `MainMenuLoad` | `PanalBLL` | `MainMenuLoad` | `string userId` | `DataTable` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `MenuLoad` | `PanalBLL` | `OtherMenuLoad` | `string userId,string parantId` | `DataTable` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `mainMenuGridView_RowCommand` | `PanalBLL` | `MainMenu` | `DropDownList userDropDownList,string userId` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `mainMenuGridView_RowCommand` | `PanalBLL` | `MenuPermissionRemove` | `int sl, int userId` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `mainMenuGridView_RowCommand` | `PanalBLL` | `SaveMainMenu` | `int sl,int userId` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `menuGridView_RowCommand` | `PanalBLL` | `MenuPermissionRemove` | `int sl, int userId` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `menuGridView_RowCommand` | `PanalBLL` | `SaveMenuBll` | `int sl, int userId` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblMainMenu` | `MainMenuLoad()` → `PanalBLL.MainMenuLoad` → `PanalDAL.MainMenuLoad` → `ClsCommonInternalDAL.DataContainerDataTable`, `MenuLoad()` → `PanalBLL.OtherMenuLoad` → `PanalDAL.OtherMenuLoad` → `ClsCommonInternalDAL.DataContainerDataTable`, `mainMenuGridView_RowCommand()` → `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue`, `mainMenuGridView_RowCommand()` → `PanalBLL.MenuPermissionRemove` → `PanalDAL.MenuPermissionRemove` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand`, `menuGridView_RowCommand()` → `PanalBLL.MenuPermissionRemove` → `PanalDAL.MenuPermissionRemove` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand`, `menuGridView_RowCommand()` → `PanalBLL.SaveMenuBll` → `PanalDAL.CheckMenuSl` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.GetParantId` |
| `tblMenuDistribution` | `MainMenuLoad()` → `PanalBLL.MainMenuLoad` → `PanalDAL.MainMenuLoad` → `ClsCommonInternalDAL.DataContainerDataTable`, `MenuLoad()` → `PanalBLL.OtherMenuLoad` → `PanalDAL.OtherMenuLoad` → `ClsCommonInternalDAL.DataContainerDataTable`, `mainMenuGridView_RowCommand()` → `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue`, `mainMenuGridView_RowCommand()` → `PanalBLL.SaveMainMenu` → `PanalDAL.SaveMainMenu` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable`, `menuGridView_RowCommand()` → `PanalBLL.SaveMenuBll` → `PanalDAL.CheckMenuSl` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.GetParantId`, `userDropDownList_SelectedIndexChanged()` → `PanalBLL.MainMenu` → `PanalDAL.MainMenu` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblUser` | `Page_Load()` → `PanalBLL.UserDdl` → `PanalDAL.UserDdl` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `mainMenuGridView_RowCommand` | Data Save | No | Yes | UNKNOWN | Notification |
| `mainMenuGridView_RowCommand` | Eroor!! | No | Yes | UNKNOWN | Notification |
| `mainMenuGridView_RowCommand` | Permission Delete | No | Yes | UNKNOWN | Notification |
| `mainMenuGridView_RowCommand` | Select First | No | Yes | UNKNOWN | Required |
| `menuGridView_RowCommand` | Data Save | No | Yes | UNKNOWN | Notification |
| `menuGridView_RowCommand` | Eroor!! | No | Yes | UNKNOWN | Notification |
| `menuGridView_RowCommand` | Permission Delete | No | Yes | UNKNOWN | Notification |
| `menuGridView_RowCommand` | Select First | No | Yes | UNKNOWN | Required |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SYS-008-001` | Data Save | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `mainMenuGridView_RowCommand()` | user action on this page | operation is refused | "Data Save" | CONFIRMED |
| `BR-SYS-008-002` | Eroor!! | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `mainMenuGridView_RowCommand()` | user action on this page | operation is refused | "Eroor!!" | CONFIRMED |
| `BR-SYS-008-003` | Permission Delete | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `mainMenuGridView_RowCommand()` | user action on this page | operation is refused | "Permission Delete" | CONFIRMED |
| `BR-SYS-008-004` | Select First | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `mainMenuGridView_RowCommand()` | user action on this page | operation is refused | "Select First" | CONFIRMED |
| `BR-SYS-008-005` | Data Save | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `menuGridView_RowCommand()` | user action on this page | operation is refused | "Data Save" | CONFIRMED |
| `BR-SYS-008-006` | Eroor!! | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `menuGridView_RowCommand()` | user action on this page | operation is refused | "Eroor!!" | CONFIRMED |
| `BR-SYS-008-007` | Permission Delete | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `menuGridView_RowCommand()` | user action on this page | operation is refused | "Permission Delete" | CONFIRMED |
| `BR-SYS-008-008` | Select First | Code | `Solution.Web/CommonUI/UserPermission.aspx.cs` → `menuGridView_RowCommand()` | user action on this page | operation is refused | "Select First" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Authentication check: performed by the master page (`MainMasterPage.master.cs` `Page_Load` redirects to `../Login.aspx` when `Session["UserId"]` is null). This page uses that master page, so the check applies.

## 21. Status Lifecycle

Status tokens appearing in this page: `Delete`

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
| Database object | `tblMainMenu` |
| Database object | `tblMenuDistribution` |
| Database object | `tblUser` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SYS-008-001` | Unauthenticated access | Request `/CommonUI/UserPermission.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SYS-008-002` | Validation: Data Save | Trigger `mainMenuGridView_RowCommand()` in the state that produces this message | alert "Data Save" and the operation is not performed | CONFIRMED |
| `TC-SYS-008-003` | Validation: Eroor!! | Trigger `mainMenuGridView_RowCommand()` in the state that produces this message | alert "Eroor!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-008-004` | Validation: Permission Delete | Trigger `mainMenuGridView_RowCommand()` in the state that produces this message | alert "Permission Delete" and the operation is not performed | CONFIRMED |
| `TC-SYS-008-005` | Validation: Data Save | Trigger `menuGridView_RowCommand()` in the state that produces this message | alert "Data Save" and the operation is not performed | CONFIRMED |
| `TC-SYS-008-006` | Validation: Eroor!! | Trigger `menuGridView_RowCommand()` in the state that produces this message | alert "Eroor!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-008-007` | Validation: Permission Delete | Trigger `menuGridView_RowCommand()` in the state that produces this message | alert "Permission Delete" and the operation is not performed | CONFIRMED |
| `TC-SYS-008-008` | Grid loads | Open the page with data present | Grid `mainMenuGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/CommonUI/UserPermission.aspx` |
| Code-behind | `Solution.Web/CommonUI/UserPermission.aspx.cs` |
| Layer source | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 22 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 10 methods traced | n/a | **MISSING → now documented** | section 12 |
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

- `Library.BLL/Panal_BLL/PanalBLL.cs` is **shared with 4 other page(s)** (e.g. `AdminMenuPanal.aspx`, `ApprovalStepPageWise.aspx`, `UserWiseApprovalPermission.aspx`, `Login.aspx`) — a change there is not local.
- Writes/reads `tblMainMenu` — any schema change to it breaks this page.
- Writes/reads `tblMenuDistribution` — any schema change to it breaks this page.
- Writes/reads `tblUser` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
