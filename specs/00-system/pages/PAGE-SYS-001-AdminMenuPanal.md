# SYS-001 — AdminMenuPanal

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SYS-001` |
| Module | `00-system` |
| File name | `AdminMenuPanal.aspx` |
| Route | `/CommonUI/AdminMenuPanal.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `AdminMenuPanal.aspx.cs` |
| Code-behind class | `CommonUI_AdminMenuPanal` |
| Markup lines | 427 |
| Code-behind lines | 397 |
| Server controls | 30 |
| GridViews | 0 |
| Code-behind methods | 20 |

## 2. Purpose

Master-data / setup screen. It captures and maintains reference data used by the transactional screens.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Master-data / setup screen. It captures and maintains reference data used by the transactional screens.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/CommonUI/AdminMenuPanal.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/CommonUI/AdminMenuPanal.aspx` |
| Code-behind | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `../scripts/jquery-ui.js` |
| Client script | `../scripts/jquery.js` |
| Client script | `../scripts/jquery.min.js` |
| Business / data layer | `Library.BLL/Panal_BLL/PanalBLL.cs` |

## 7. UI Control Inventory

30 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 5 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 42 |
| 4 | `msgLabel` | asp:Label |  |  |  |  |  |  | 61 |
| 5 | `hidAccordionIndex` | asp:HiddenField |  |  |  |  |  |  | 69 |
| 6 | `mainMenuNameTextBox` | asp:TextBox | Main Menu Name : |  |  |  |  |  | 82 |
| 7 | `addPageCheckBox` | asp:CheckBox | Add Page : Yes ? |  | True |  |  | addPageCheckBox_CheckedChanged | 87 |
| 8 | `mainMenuDesFolderTextBox` | asp:TextBox | Destination Folder : |  |  |  |  |  | 101 |
| 9 | `mainMenuPageNameTextBox` | asp:TextBox | Page Name : |  |  |  |  |  | 107 |
| 10 | `saveMainMenuButton` | asp:Button |  | Save |  |  |  | saveMainMenuButton_Click | 125 |
| 11 | `ddlMMenuNameMPanal` | asp:DropDownList | Main Menu : |  |  |  |  |  | 144 |
| 12 | `menuNameTextBox` | asp:TextBox | Menu Name : |  |  |  |  |  | 151 |
| 13 | `addPageCheckBoxMenu` | asp:CheckBox | Add Page : Yes ? |  | True |  |  | addPageCheckBoxMenu_CheckedChanged | 158 |
| 14 | `menuDisFolderTextBox` | asp:TextBox | Destination Folder : |  |  |  |  |  | 168 |
| 15 | `menuPageTextBox` | asp:TextBox | Page Name : |  |  |  |  |  | 174 |
| 16 | `menuSaveButton` | asp:Button |  | Save |  |  |  | menuSaveButton_Click | 193 |
| 17 | `ddlSubMMenuNameMPanal` | asp:DropDownList | Main Menu : |  | True |  |  | ddlSubMMenuNameMPanal_SelectedIndexChanged | 212 |
| 18 | `ddlMenuNameForSubMenu` | asp:DropDownList | Menu Name : |  |  |  |  |  | 220 |
| 19 | `subMenuNameTextBox` | asp:TextBox | Sub Menu Name : |  |  |  |  |  | 226 |
| 20 | `addPageCheckBoxSubMenu` | asp:CheckBox | Add Page : Yes ? |  | True |  |  | addPageCheckBoxSubMenu_CheckedChanged | 233 |
| 21 | `submenuDisFolderTextBox` | asp:TextBox | Destination Folder : |  |  |  |  |  | 254 |
| 22 | `submenuPageTextBox` | asp:TextBox | Page Name : |  |  |  |  |  | 259 |
| 23 | `subMenuSaveButton` | asp:Button |  | Save |  |  |  | subMenuSaveButton_Click | 278 |
| 24 | `ddlChMMenuNameMPanal` | asp:DropDownList | Main Menu : |  | True |  |  | ddlChMMenuNameMPanal_SelectedIndexChanged | 298 |
| 25 | `ddlMenuNameForChildMenu` | asp:DropDownList | Menu Name : |  | True |  |  | ddlMenuNameForChildMenu_SelectedIndexChanged | 306 |
| 26 | `ddlsubMenuName` | asp:DropDownList | Sub Menu Name : |  |  |  |  |  | 315 |
| 27 | `childMenuNameTextBox` | asp:TextBox | Child Menu Name : |  |  |  |  |  | 324 |
| 28 | `childDisFolderTextBox` | asp:TextBox | Destination Folder : |  |  |  |  |  | 329 |
| 29 | `childMenuPageTextBox` | asp:TextBox | Page Name : |  |  |  |  |  | 334 |
| 30 | `saveChildMenu` | asp:Button |  | Save |  |  |  | saveChildMenu_Click | 348 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDDL` | `aPanalBll.MainMenuDropdown(...)` | `PanalBLL.MainMenuDropdown(DropDownList aDropDownList)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MainMenuDropdown` → `PanalDAL.MainMenuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblMainMenu` | — |
| `SubMenuSave` | `aPanalBll.MenuSaveBll(...)` | `PanalBLL.MenuSaveBll(ObjPanal aObjPanal)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT, SELECT | `tblMainMenu` | — |
| `MenuSave` | `aPanalBll.MenuSaveBll(...)` | `PanalBLL.MenuSaveBll(ObjPanal aObjPanal)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT, SELECT | `tblMainMenu` | — |
| `MainMenuSave` | `aPanalBll.MenuSaveBll(...)` | `PanalBLL.MenuSaveBll(ObjPanal aObjPanal)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT, SELECT | `tblMainMenu` | — |
| `ChildMenuSave` | `aPanalBll.MenuSaveBll(...)` | `PanalBLL.MenuSaveBll(ObjPanal aObjPanal)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | INSERT, SELECT | `tblMainMenu` | — |
| `ddlSubMMenuNameMPanal_SelectedIndexChanged` | `aPanalBll.MenuDropdown(...)` | `PanalBLL.MenuDropdown(DropDownList aDropDownList,string id)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuDropdown` → `PanalDAL.MenuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblMainMenu` | — |
| `ddlChMMenuNameMPanal_SelectedIndexChanged` | `aPanalBll.MenuDropdown(...)` | `PanalBLL.MenuDropdown(DropDownList aDropDownList,string id)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuDropdown` → `PanalDAL.MenuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblMainMenu` | — |
| `ddlMenuNameForChildMenu_SelectedIndexChanged` | `aPanalBll.MenuDropdown(...)` | `PanalBLL.MenuDropdown(DropDownList aDropDownList,string id)` <br/>`Library.BLL/Panal_BLL/PanalBLL.cs` | `PanalBLL.MenuDropdown` → `PanalDAL.MenuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblMainMenu` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `ddlMMenuNameMPanal` | Main Menu : | asp:DropDownList | False | — | `aPanalBll.MainMenuDropdown()` in `LoadDDL()` | `tblMainMenu` | set in code (see the DAL method above) |
| `ddlSubMMenuNameMPanal` | Main Menu : | asp:DropDownList | True | ddlSubMMenuNameMPanal_SelectedIndexChanged | `aPanalBll.MainMenuDropdown()` in `LoadDDL()`; `aPanalBll.MenuDropdown()` in `ddlSubMMenuNameMPanal_SelectedIndexChanged()` | `tblMainMenu` | set in code (see the DAL method above) |
| `ddlMenuNameForSubMenu` | Menu Name : | asp:DropDownList | False | — | `aPanalBll.MenuDropdown()` in `ddlSubMMenuNameMPanal_SelectedIndexChanged()` | `tblMainMenu` | set in code (see the DAL method above) |
| `ddlChMMenuNameMPanal` | Main Menu : | asp:DropDownList | True | ddlChMMenuNameMPanal_SelectedIndexChanged | `aPanalBll.MainMenuDropdown()` in `LoadDDL()`; `aPanalBll.MenuDropdown()` in `ddlChMMenuNameMPanal_SelectedIndexChanged()` | `tblMainMenu` | set in code (see the DAL method above) |
| `ddlMenuNameForChildMenu` | Menu Name : | asp:DropDownList | True | ddlMenuNameForChildMenu_SelectedIndexChanged | `aPanalBll.MenuDropdown()` in `ddlChMMenuNameMPanal_SelectedIndexChanged()`; `aPanalBll.MenuDropdown()` in `ddlMenuNameForChildMenu_SelectedIndexChanged()` | `tblMainMenu` | set in code (see the DAL method above) |
| `ddlsubMenuName` | Sub Menu Name : | asp:DropDownList | False | — | `aPanalBll.MenuDropdown()` in `ddlMenuNameForChildMenu_SelectedIndexChanged()` | `tblMainMenu` | set in code (see the DAL method above) |

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
| `saveMainMenuButton` | asp:Button | Save | — | `saveMainMenuButton_Click` | 11 lines | — | — |
| `menuSaveButton` | asp:Button | Save | — | `menuSaveButton_Click` | 9 lines | — | — |
| `subMenuSaveButton` | asp:Button | Save | — | `subMenuSaveButton_Click` | 9 lines | — | — |
| `saveChildMenu` | asp:Button | Save | — | `saveChildMenu_Click` | 9 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none
- External scripts: `../scripts/jquery-ui.js`, `../scripts/jquery.js`, `../scripts/jquery.min.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 11 | — | — | — | — |
| 2 | `addPageCheckBox_CheckedChanged` | `void addPageCheckBox_CheckedChanged(object sender, EventArgs e)` | 13 | — | — | — | — |
| 3 | `LoadDDL` | `void LoadDDL()` | 5 | `aPanalBll.MainMenuDropdown` | — | — | — |
| 4 | `addPageCheckBoxMenu_CheckedChanged` | `void addPageCheckBoxMenu_CheckedChanged(object sender, EventArgs e)` | 13 | — | — | — | — |
| 5 | `SubMenuValidation` | `bool SubMenuValidation()` | 31 | — | — | — | — |
| 6 | `SubMenuSave` | `void SubMenuSave()` | 29 | `aPanalBll.MenuSaveBll` | — | — | — |
| 7 | `MenuValidation` | `bool MenuValidation()` | 31 | — | — | — | — |
| 8 | `MenuSave` | `void MenuSave()` | 27 | `aPanalBll.MenuSaveBll` | — | — | — |
| 9 | `MainMenuValidation` | `bool MainMenuValidation()` | 25 | — | — | — | — |
| 10 | `MainMenuSave` | `void MainMenuSave()` | 28 | `aPanalBll.MenuSaveBll` | — | — | — |
| 11 | `ChildMenuValidation` | `bool ChildMenuValidation()` | 30 | — | — | — | — |
| 12 | `ChildMenuSave` | `void ChildMenuSave()` | 25 | `aPanalBll.MenuSaveBll` | — | — | — |
| 13 | `menuSaveButton_Click` | `void menuSaveButton_Click(object sender, EventArgs e)` | 9 | — | — | — | — |
| 14 | `saveMainMenuButton_Click` | `void saveMainMenuButton_Click(object sender, EventArgs e)` | 11 | — | — | — | — |
| 15 | `subMenuSaveButton_Click` | `void subMenuSaveButton_Click(object sender, EventArgs e)` | 9 | — | — | — | — |
| 16 | `ddlSubMMenuNameMPanal_SelectedIndexChanged` | `void ddlSubMMenuNameMPanal_SelectedIndexChanged(object sender, EventArgs e)` | 9 | `aPanalBll.MenuDropdown` | — | — | — |
| 17 | `ddlChMMenuNameMPanal_SelectedIndexChanged` | `void ddlChMMenuNameMPanal_SelectedIndexChanged(object sender, EventArgs e)` | 9 | `aPanalBll.MenuDropdown` | — | — | — |
| 18 | `ddlMenuNameForChildMenu_SelectedIndexChanged` | `void ddlMenuNameForChildMenu_SelectedIndexChanged(object sender, EventArgs e)` | 9 | `aPanalBll.MenuDropdown` | — | — | — |
| 19 | `saveChildMenu_Click` | `void saveChildMenu_Click(object sender, EventArgs e)` | 9 | — | — | — | — |
| 20 | `addPageCheckBoxSubMenu_CheckedChanged` | `void addPageCheckBoxSubMenu_CheckedChanged(object sender, EventArgs e)` | 13 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
AdminMenuPanal.aspx (markup)
  -> CommonUI_AdminMenuPanal (code-behind event handler)
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
| `LoadDDL` | `PanalBLL` | `MainMenuDropdown` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `SubMenuSave` | `PanalBLL` | `MenuSaveBll` | `ObjPanal aObjPanal` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `MenuSave` | `PanalBLL` | `MenuSaveBll` | `ObjPanal aObjPanal` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `MainMenuSave` | `PanalBLL` | `MenuSaveBll` | `ObjPanal aObjPanal` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `ChildMenuSave` | `PanalBLL` | `MenuSaveBll` | `ObjPanal aObjPanal` | `bool` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `ddlSubMMenuNameMPanal_SelectedIndexChanged` | `PanalBLL` | `MenuDropdown` | `DropDownList aDropDownList,string id` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `ddlChMMenuNameMPanal_SelectedIndexChanged` | `PanalBLL` | `MenuDropdown` | `DropDownList aDropDownList,string id` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `ddlMenuNameForChildMenu_SelectedIndexChanged` | `PanalBLL` | `MenuDropdown` | `DropDownList aDropDownList,string id` | `void` | — | — | — | `Library.BLL/Panal_BLL/PanalBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblMainMenu` | `ChildMenuSave()` → `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal`, `LoadDDL()` → `PanalBLL.MainMenuDropdown` → `PanalDAL.MainMenuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue`, `MainMenuSave()` → `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal`, `MenuSave()` → `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal`, `SubMenuSave()` → `PanalBLL.MenuSaveBll` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `PanalDAL.MenuSaveDal`, `ddlChMMenuNameMPanal_SelectedIndexChanged()` → `PanalBLL.MenuDropdown` → `PanalDAL.MenuDropDown` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `SubMenuValidation` | Fill Required TextBox!! | No | Yes | UNKNOWN | Notification |
| `SubMenuValidation` | Select Required DropDown!! | No | Yes | UNKNOWN | Required |
| `SubMenuSave` | Menu Save | No | Yes | UNKNOWN | Notification |
| `MenuValidation` | Fill Required TextBox!! | No | Yes | UNKNOWN | Notification |
| `MenuValidation` | Select Required DropDown!! | No | Yes | UNKNOWN | Required |
| `MenuSave` | Menu Save | No | Yes | UNKNOWN | Notification |
| `MainMenuValidation` | Fill Required TextBox!! | No | Yes | UNKNOWN | Notification |
| `MainMenuSave` | MainMenu Save | No | Yes | UNKNOWN | Notification |
| `ChildMenuValidation` | Fill Required TextBox!! | No | Yes | UNKNOWN | Notification |
| `ChildMenuValidation` | Select Required DropDown!! | No | Yes | UNKNOWN | Required |
| `ChildMenuSave` | MainMenu Save | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SYS-001-001` | Fill Required TextBox!! | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `SubMenuValidation()` | user action on this page | operation is refused | "Fill Required TextBox!!" | CONFIRMED |
| `BR-SYS-001-002` | Select Required DropDown!! | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `SubMenuValidation()` | user action on this page | operation is refused | "Select Required DropDown!!" | CONFIRMED |
| `BR-SYS-001-003` | Menu Save | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `SubMenuSave()` | user action on this page | operation is refused | "Menu Save" | CONFIRMED |
| `BR-SYS-001-004` | Fill Required TextBox!! | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `MenuValidation()` | user action on this page | operation is refused | "Fill Required TextBox!!" | CONFIRMED |
| `BR-SYS-001-005` | Select Required DropDown!! | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `MenuValidation()` | user action on this page | operation is refused | "Select Required DropDown!!" | CONFIRMED |
| `BR-SYS-001-006` | Menu Save | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `MenuSave()` | user action on this page | operation is refused | "Menu Save" | CONFIRMED |
| `BR-SYS-001-007` | Fill Required TextBox!! | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `MainMenuValidation()` | user action on this page | operation is refused | "Fill Required TextBox!!" | CONFIRMED |
| `BR-SYS-001-008` | MainMenu Save | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `MainMenuSave()` | user action on this page | operation is refused | "MainMenu Save" | CONFIRMED |
| `BR-SYS-001-009` | Fill Required TextBox!! | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `ChildMenuValidation()` | user action on this page | operation is refused | "Fill Required TextBox!!" | CONFIRMED |
| `BR-SYS-001-010` | Select Required DropDown!! | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `ChildMenuValidation()` | user action on this page | operation is refused | "Select Required DropDown!!" | CONFIRMED |
| `BR-SYS-001-011` | MainMenu Save | Code | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` → `ChildMenuSave()` | user action on this page | operation is refused | "MainMenu Save" | CONFIRMED |

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
| Database object | `tblMainMenu` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SYS-001-001` | Unauthenticated access | Request `/CommonUI/AdminMenuPanal.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SYS-001-002` | Validation: Fill Required TextBox!! | Trigger `SubMenuValidation()` in the state that produces this message | alert "Fill Required TextBox!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-003` | Validation: Select Required DropDown!! | Trigger `SubMenuValidation()` in the state that produces this message | alert "Select Required DropDown!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-004` | Validation: Menu Save | Trigger `SubMenuSave()` in the state that produces this message | alert "Menu Save" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-005` | Validation: Fill Required TextBox!! | Trigger `MenuValidation()` in the state that produces this message | alert "Fill Required TextBox!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-006` | Validation: Select Required DropDown!! | Trigger `MenuValidation()` in the state that produces this message | alert "Select Required DropDown!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-007` | Validation: Menu Save | Trigger `MenuSave()` in the state that produces this message | alert "Menu Save" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-008` | Validation: Fill Required TextBox!! | Trigger `MainMenuValidation()` in the state that produces this message | alert "Fill Required TextBox!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-009` | Validation: MainMenu Save | Trigger `MainMenuSave()` in the state that produces this message | alert "MainMenu Save" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-010` | Validation: Fill Required TextBox!! | Trigger `ChildMenuValidation()` in the state that produces this message | alert "Fill Required TextBox!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-011` | Validation: Select Required DropDown!! | Trigger `ChildMenuValidation()` in the state that produces this message | alert "Select Required DropDown!!" and the operation is not performed | CONFIRMED |
| `TC-SYS-001-012` | Validation: MainMenu Save | Trigger `ChildMenuSave()` in the state that produces this message | alert "MainMenu Save" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/CommonUI/AdminMenuPanal.aspx` |
| Code-behind | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` |
| Layer source | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 30 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 20 methods traced | n/a | **MISSING → now documented** | section 12 |
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

## 31. Change Impact

Changing this page affects:

- `Library.BLL/Panal_BLL/PanalBLL.cs` is **shared with 4 other page(s)** (e.g. `ApprovalStepPageWise.aspx`, `UserPermission.aspx`, `UserWiseApprovalPermission.aspx`, `Login.aspx`) — a change there is not local.
- Writes/reads `tblMainMenu` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
