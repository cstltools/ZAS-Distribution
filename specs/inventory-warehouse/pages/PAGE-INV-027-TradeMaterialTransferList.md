# INV-027 — TradeMaterialTransferList

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-027` |
| Module | `inventory-warehouse` |
| File name | `TradeMaterialTransferList.aspx` |
| Route | `/SInventory_UI/TradeMaterialTransferList.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `TradeMaterialTransferList.aspx.cs` |
| Code-behind class | `SInventory_UI_TradeMaterialTransferList` |
| Markup lines | 458 |
| Code-behind lines | 193 |
| Server controls | 53 |
| GridViews | 1 |
| Code-behind methods | 10 |

## 2. Purpose

List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/TradeMaterialTransfer.aspx`

Outbound `Response.Redirect` targets: `Login.aspx`, `TradeMaterialTransfer.aspx`, `TradeMaterialTransfer.aspx?ID=`

## 5. Route

`/SInventory_UI/TradeMaterialTransferList.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TradeMaterialTransferList.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TradeMaterialTransferList.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Client script | `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js` |
| Business / data layer | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |

## 7. UI Control Inventory

53 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 240 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 241 |
| 4 | `newLinkButton` | asp:LinkButton | Existing Transfers |  |  |  |  | newLinkButton_Click | 255 |
| 5 | `fromDateTextBox` | asp:TextBox | From Date |  |  |  |  |  | 264 |
| 6 | `CalendarExtenderFrom` | cc1:CalendarExtender | From Date |  |  |  |  |  | 265 |
| 7 | `toDateTextBox` | asp:TextBox | To Date |  |  |  |  |  | 269 |
| 8 | `CalendarExtenderTo` | cc1:CalendarExtender | To Date |  |  |  |  |  | 270 |
| 9 | `fromBusinessUnitDropDownList` | asp:DropDownList | From Business Unit |  |  |  |  |  | 274 |
| 10 | `toBusinessUnitDropDownList` | asp:DropDownList | To Business Unit |  |  |  |  |  | 278 |
| 11 | `rmNameTextBox` | asp:TextBox | RM Name |  |  |  |  |  | 282 |
| 12 | `searchButton` | asp:Button | RM Name | Search |  |  |  | searchButton_Click | 285 |
| 13 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_Click | 286 |
| 14 | `tradeMaterialTransferGridView` | asp:GridView |  |  |  |  |  | tradeMaterialTransferGridView_RowCommand | 292 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 295 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 296 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 297 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 298 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 299 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 300 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 301 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 302 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 303 |
| 24 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 304 |
| 25 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 305 |
| 26 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 306 |
| 27 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 308 |
| 28 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 316 |
| 29 | `detailsImageButton` | asp:ImageButton | tradeMaterialTransferGridView → col[13] "Actions" | ~/images/viewList.png |  |  |  |  | 319 |
| 30 | `editImageButton` | asp:ImageButton | tradeMaterialTransferGridView → col[13] "Actions" | ~/images/edit.png |  |  |  |  | 323 |
| 31 | `deleteImageButton` | asp:ImageButton | tradeMaterialTransferGridView → col[13] "Actions" | ~/images/lineDelete.png |  |  |  | return confirm('Are you sure you want to delete this transfer?'); | 328 |
| 32 | `dummyButton` | asp:Button | No transfers found matching search criteria. |  |  |  |  |  | 348 |
| 33 | `detailsPanel` | asp:Panel |  |  |  |  |  |  | 349 |
| 34 | `closeLinkButton` | asp:LinkButton | Trade Material Transfer Details |  |  |  |  |  | 352 |
| 35 | `selectedTmtIdHiddenField` | asp:HiddenField | &times; |  |  |  |  |  | 355 |
| 36 | `lblDetailId` | asp:Label | Transfer Code: |  |  |  |  |  | 359 |
| 37 | `lblDetailFromBU` | asp:Label | From Business Unit: |  |  |  |  |  | 363 |
| 38 | `lblDetailFromLoc` | asp:Label | From Location: |  |  |  |  |  | 367 |
| 39 | `lblDetailFGName` | asp:Label | Finished Goods (FG): |  |  |  |  |  | 371 |
| 40 | `lblDetailToBU` | asp:Label | To Business Unit: |  |  |  |  |  | 375 |
| 41 | `lblDetailToLoc` | asp:Label | To Location: |  |  |  |  |  | 379 |
| 42 | `lblDetailRMName` | asp:Label | Raw Material (RM): |  |  |  |  |  | 383 |
| 43 | `lblDetailQty` | asp:Label | Transfer Quantity: |  |  |  |  |  | 387 |
| 44 | `lblDetailRemarks` | asp:Label | Remarks: |  |  |  |  |  | 391 |
| 45 | `lblDetailEntryBy` | asp:Label | Entry User & Date: |  |  |  |  |  | 396 |
| 46 | `lblDetailEntryDate` | asp:Label | on |  |  |  |  |  | 397 |
| 47 | `lblDetailStatus` | asp:Label | Approval Status: |  |  |  |  |  | 403 |
| 48 | `lblDetailApprovedBy` | asp:Label | Approved By & Date: |  |  |  |  |  | 409 |
| 49 | `lblDetailApprovedDate` | asp:Label | on |  |  |  |  |  | 410 |
| 50 | `closeButton` | asp:Button | on | Close |  |  |  |  | 416 |
| 51 | `ModalPopupExtender1` | cc1:ModalPopupExtender |  |  |  |  |  |  | 420 |
| 52 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 431 |
| 53 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 434 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `tradeMaterialTransferGridView`

| Attribute | Value |
|---|---|
| datakeynames | `TradeMaterialTransferId` |
| autogeneratecolumns | `False` |
| cssclass | `modern-grid` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Transfer Code | TransferCode | — | — | — |
| 1 | BoundField | From Business Unit | FromCompanyName | — | — | — |
| 2 | BoundField | From Location | FromLocationName | — | — | — |
| 3 | BoundField | FG Name | FromProductName | — | — | — |
| 4 | BoundField | To Business Unit | ToCompanyName | — | — | — |
| 5 | BoundField | To Location | ToLocationName | — | — | — |
| 6 | BoundField | RM Name | ToItemDescription | — | — | — |
| 7 | BoundField | Qty | TransferQty | — | — | — |
| 8 | BoundField | Entry By | EntryBy | — | — | — |
| 9 | BoundField | Entry Date | EntryDate | — | — | — |
| 10 | BoundField | Update By | UpdateBy | — | — | — |
| 11 | BoundField | Update Date | UpdateDate | — | — | — |
| 12 | TemplateField | Status | — | `ApprovalStatus` | — | — |
| 13 | TemplateField | Actions | — | `ApprovalStatus` | `ImageButton:detailsImageButton`, `ImageButton:editImageButton`, `ImageButton:deleteImageButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadBusinessUnits` | `aTradeMaterialTransferDal.LoadBusinessUnit(...)` | `TradeMaterialTransferDal.LoadBusinessUnit(DropDownList ddl, int userId, int moduleId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.LoadBusinessUnit` → `ClsCommonInternalDAL.LoadDropDownListData` | — | — | `sp_Common_GetUnitByPermissionByModule` |
| `LoadGrid` | `aTradeMaterialTransferDal.GetTradeMaterialTransferList(...)` | `TradeMaterialTransferDal.GetTradeMaterialTransferList(string fromDate, string toDate, string fromCompanyId, string toCompany)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetTradeMaterialTransferList` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — |
| `tradeMaterialTransferGridView_RowCommand` | `aTradeMaterialTransferDal.DeleteTradeMaterialTransfer(...)` | `TradeMaterialTransferDal.DeleteTradeMaterialTransfer(int tradeMaterialTransferId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.DeleteTradeMaterialTransfer` → `ClsCommonInternalDAL.DeleteAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.DeleteAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `spDeleteTradeMaterialTransfer` |
| `tradeMaterialTransferGridView_RowCommand` | `aTradeMaterialTransferDal.GetTradeMaterialTransferById(...)` | `TradeMaterialTransferDal.GetTradeMaterialTransferById(int tradeMaterialTransferId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetTradeMaterialTransferById` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — |
| `ShowDetails` | `aTradeMaterialTransferDal.GetTradeMaterialTransferById(...)` | `TradeMaterialTransferDal.GetTradeMaterialTransferById(int tradeMaterialTransferId)` <br/>`Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | `TradeMaterialTransferDal.GetTradeMaterialTransferById` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `fromBusinessUnitDropDownList` | From Business Unit | asp:DropDownList | False | — | `aTradeMaterialTransferDal.LoadBusinessUnit()` in `LoadBusinessUnits()` | UNKNOWN | set in code (see the DAL method above) |
| `toBusinessUnitDropDownList` | To Business Unit | asp:DropDownList | False | — | `aTradeMaterialTransferDal.LoadBusinessUnit()` in `LoadBusinessUnits()` | UNKNOWN | set in code (see the DAL method above) |

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
| `newLinkButton` | asp:LinkButton |  | — | `newLinkButton_Click` | 3 lines | — | — |
| `searchButton` | asp:Button | Search | — | `searchButton_Click` | 8 lines | — | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_Click` | 17 lines | — | — |
| `detailsImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `editImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `deleteImageButton` | asp:ImageButton |  | return confirm('Are you sure you want to delete this transfer?'); | `—` | — | — | — |
| `dummyButton` | asp:Button |  | — | `—` | — | — | — |
| `closeLinkButton` | asp:LinkButton |  | — | `—` | — | — | — |
| `closeButton` | asp:Button | Close | — | `—` | — | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:CalendarExtender`(fromDateTextBox), `cc1:CalendarExtender`(toDateTextBox), `cc1:ModalPopupExtender`(dummyButton)
- Inline JavaScript functions declared in the markup: `initListSelect2()`
- External scripts: `https://cdnjs.cloudflare.com/ajax/libs/select2/3.5.4/select2.min.js`

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `LoadBusinessUnits` | `void LoadBusinessUnits()` | 13 | `aTradeMaterialTransferDal.LoadBusinessUnit` | — | — | `UserId` |
| 3 | `LoadGrid` | `void LoadGrid()` | 11 | `aTradeMaterialTransferDal.GetTradeMaterialTransferList` | — | — | — |
| 4 | `Validation` | `bool Validation()` | 23 | — | — | — | — |
| 5 | `searchButton_Click` | `void searchButton_Click(object sender, EventArgs e)` | 8 | — | — | — | — |
| 6 | `clearButton_Click` | `void clearButton_Click(object sender, EventArgs e)` | 17 | — | — | — | — |
| 7 | `newLinkButton_Click` | `void newLinkButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 8 | `tradeMaterialTransferGridView_RowCommand` | `void tradeMaterialTransferGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 38 | `aTradeMaterialTransferDal.DeleteTradeMaterialTransfer`, `aTradeMaterialTransferDal.GetTradeMaterialTransferById` | — | — | — |
| 9 | `ShowDetails` | `void ShowDetails(int id)` | 36 | `aTradeMaterialTransferDal.GetTradeMaterialTransferById` | — | — | — |
| 10 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
TradeMaterialTransferList.aspx (markup)
  -> SInventory_UI_TradeMaterialTransferList (code-behind event handler)
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

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadBusinessUnits` | `TradeMaterialTransferDal` | `LoadBusinessUnit` | `DropDownList ddl, int userId, int moduleId` | `void` | — | — | `sp_Common_GetUnitByPermissionByModule` | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `LoadGrid` | `TradeMaterialTransferDal` | `GetTradeMaterialTransferList` | `string fromDate, string toDate, string fromCompanyId, string` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `tradeMaterialTransferGridView_RowCommand` | `TradeMaterialTransferDal` | `DeleteTradeMaterialTransfer` | `int tradeMaterialTransferId` | `bool` | — | — | `spDeleteTradeMaterialTransfer` | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `tradeMaterialTransferGridView_RowCommand` | `TradeMaterialTransferDal` | `GetTradeMaterialTransferById` | `int tradeMaterialTransferId` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| `ShowDetails` | `TradeMaterialTransferDal` | `GetTradeMaterialTransferById` | `int tradeMaterialTransferId` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblTradeMaterialTransfer` | — | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Common_GetUnitByPermissionByModule` | LoadBusinessUnits → `TradeMaterialTransferDal.LoadBusinessUnit → ClsCommonInternalDAL.LoadDropDownListData` | **No** — see note |
| `spDeleteTradeMaterialTransfer` | tradeMaterialTransferGridView_RowCommand → `TradeMaterialTransferDal.DeleteTradeMaterialTransfer → ClsCommonInternalDAL.DeleteAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.DeleteAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadGrid()` → `TradeMaterialTransferDal.GetTradeMaterialTransferList`, `ShowDetails()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById`, `tradeMaterialTransferGridView_RowCommand()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById` |
| `tblCompanyUnit` | `LoadGrid()` → `TradeMaterialTransferDal.GetTradeMaterialTransferList`, `ShowDetails()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById`, `tradeMaterialTransferGridView_RowCommand()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById` |
| `tblTradeMaterialTransfer` | `LoadGrid()` → `TradeMaterialTransferDal.GetTradeMaterialTransferList`, `ShowDetails()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById`, `tradeMaterialTransferGridView_RowCommand()` → `TradeMaterialTransferDal.GetTradeMaterialTransferById` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ShowDetails` | Approved | No | Yes | UNKNOWN | Notification |
| `ShowDetails` | Pending | No | Yes | UNKNOWN | Notification |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-INV-027-001` | Approved | Code | `Solution.Web/SInventory_UI/TradeMaterialTransferList.aspx.cs` → `ShowDetails()` | user action on this page | operation is refused | "Approved" | CONFIRMED |
| `BR-INV-027-002` | Pending | Code | `Solution.Web/SInventory_UI/TradeMaterialTransferList.aspx.cs` → `ShowDetails()` | user action on this page | operation is refused | "Pending" | CONFIRMED |
| `BR-INV-027-003` | {0} | Code | `Solution.Web/SInventory_UI/TradeMaterialTransferList.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `UserId`

Authentication check: performed by the master page (`MainMasterPage.master.cs` `Page_Load` redirects to `../Login.aspx` when `Session["UserId"]` is null). This page uses that master page, so the check applies.

## 21. Status Lifecycle

Status tokens appearing in this page: `Approved`, `Pending`

Application-wide status vocabulary and the transitions that are actually implemented are documented in `specs/00-system/status-lifecycle.md`. Transitions executed inside stored procedures are **UNKNOWN**.

## 22. Error Handling

- Page level: no `try`/`catch` found in the code-behind — an exception surfaces as an ASP.NET yellow screen
- DAL level: `ClsCommonInternalDAL` re-throws (`throw ex;`, which resets the stack trace). `DB_Manager.ExecuteNonQueryAction` **swallows** the exception, rolls back the transaction, closes the connection and returns `0`, so the caller sees "no rows affected" rather than the error.
- Application level: `Global.asax` `Application_Error` is **empty** — there is no global error handler, no logging and no custom error page (`<customErrors>` is not configured in `Web.config`).
- User-visible failures are surfaced as `alert()` boxes injected with `ScriptManager.RegisterStartupScript`.

## 23. Audit

Audit columns written by this page: `EntryDate`

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
| Assembly source | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblTradeMaterialTransfer` |
| Stored procedure | `sp_Common_GetUnitByPermissionByModule` |
| Stored procedure | `spDeleteTradeMaterialTransfer` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-027-001` | Unauthenticated access | Request `/SInventory_UI/TradeMaterialTransferList.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-027-002` | Validation: Approved | Trigger `ShowDetails()` in the state that produces this message | alert "Approved" and the operation is not performed | CONFIRMED |
| `TC-INV-027-003` | Validation: Pending | Trigger `ShowDetails()` in the state that produces this message | alert "Pending" and the operation is not performed | CONFIRMED |
| `TC-INV-027-004` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-INV-027-005` | Grid loads | Open the page with data present | Grid `tradeMaterialTransferGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TradeMaterialTransferList.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TradeMaterialTransferList.aspx.cs` |
| Layer source | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 53 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 10 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 2 names found | bodies **not in repo** | **PARTIAL** | section 16 |
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

- `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` is **shared with 2 other page(s)** (e.g. `TradeMaterialTransfer.aspx`, `TradeMaterialTransferApproval.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblTradeMaterialTransfer` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
