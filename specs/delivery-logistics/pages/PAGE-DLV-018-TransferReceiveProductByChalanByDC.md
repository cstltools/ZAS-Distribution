# DLV-018 — TransferReceiveProductByChalanByDC

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `DLV-018` |
| Module | `delivery-logistics` |
| File name | `TransferReceiveProductByChalanByDC.aspx` |
| Route | `/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `TransferReceiveProductByChalanByDC.aspx.cs` |
| Code-behind class | `SInventory_UI_ReceiveProductByChalanByDC` |
| Markup lines | 234 |
| Code-behind lines | 202 |
| Server controls | 28 |
| GridViews | 1 |
| Code-behind methods | 11 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/TransferStockReceiveByDC.aspx`

Outbound `Response.Redirect` targets: `TransferStockReceiveByDC.aspx`

## 5. Route

`/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ChalanId`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |

## 7. UI Control Inventory

28 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 7 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `clnNoTextBox` | asp:TextBox | Chalan No: |  |  | ReadOnly=True |  |  | 43 |
| 5 | `clnDateTextBox` | asp:TextBox | Chalan Date : |  |  | ReadOnly=True |  |  | 49 |
| 6 | `rcvDateTextBox` | asp:TextBox | Receive Date : |  |  | ReadOnly=True |  |  | 55 |
| 7 | `truckTextBox` | asp:TextBox | Truck No |  |  | ReadOnly=True |  |  | 63 |
| 8 | `driverNameTextBox` | asp:TextBox | Driver Name : |  |  | ReadOnly=True |  |  | 69 |
| 9 | `rcvGridView` | asp:GridView |  |  |  |  |  |  | 103 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 106 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 107 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 108 |
| 13 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 109 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 110 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 111 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 112 |
| 17 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 113 |
| 18 | `rcvQtyTextBox` | asp:TextBox | rcvGridView → col[7] "Receive Qty" |  | True |  |  | rcvQtyTextBox_TextChanged | 115 |
| 19 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 119 |
| 20 | `damageTextBox` | asp:TextBox | rcvGridView → col[8] "Unreceive Qty" |  | True | ReadOnly=True |  | damageTextBox_TextChanged | 121 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 124 |
| 22 | `backLinkButton` | asp:LinkButton |  |  |  |  |  | backLinkButton_Click | 156 |
| 23 | `UpdatePanel3` | asp:UpdatePanel |  |  |  |  |  |  | 164 |
| 24 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 166 |
| 25 | `hdComUnitId` | asp:HiddenField |  |  |  |  |  |  | 181 |
| 26 | `hdReqId` | asp:HiddenField |  |  |  |  |  |  | 201 |
| 27 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 224 |
| 28 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 228 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `rcvGridView`

| Attribute | Value |
|---|---|
| datakeynames | `ChalanId,ChalanDetailsId,DCStoreId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Pack Size | PackSize | — | — | — |
| 3 | BoundField | Batch No | BatchNo | — | — | — |
| 4 | BoundField | Quantity | Quantity | — | — | — |
| 5 | BoundField | Exp. Date | ExpDate | — | — | — |
| 6 | BoundField | Receive Date | ReceiveDate | — | — | — |
| 7 | TemplateField | Receive Qty | — | `Quantity` | `TextBox:rcvQtyTextBox` | — |
| 8 | TemplateField | Unreceive Qty | — | — | `TextBox:damageTextBox` | — |
| 9 | BoundField | Mfg. date | mfgdate | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `DCStoreFreezeDAO`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadInfo` | `aChalanBll.GetChalanReceieve(...)` | `ChalanBLL.GetChalanReceieve(string id)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblChalanDetail`, `tblChalanInfo`, `tblCompanyUnit`, `tblDCStore`, `tblProduct` | — |
| `LoadGrid` | `aChalanBll.GetChalanReceieve(...)` | `ChalanBLL.GetChalanReceieve(string id)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblChalanDetail`, `tblChalanInfo`, `tblCompanyUnit`, `tblDCStore`, `tblProduct` | — |
| `Save` | `aChalanBll.DCStockIn2(...)` | `ChalanBLL.DCStockIn2(DCStockNew aDcStockNew)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.DCStockIn2` → `ChalanDAL.ChalanUpdate` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `ChalanDAL.CheckB2BExistOrNotInDcStore` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.DCStockInDALMain` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `ChalanDAL.ProcessWeightedAverage` | INSERT, SELECT, UPDATE | `tblChalanInfo`, `tblDCStore` | `sp_Process_WeightedAverage`, `sp_UD_ReceiveIdInDc` |
| `Save` | `aChalanBll.SaveDCStoreFreeze2(...)` | `ChalanBLL.SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStockNew)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.SaveDCStoreFreeze2` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ChalanDAL.SaveDCStoreFreeze2` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `Save` | `aRequisitionBll.CheckDuplicateEntry(...)` | `RequisitionBLL.CheckDuplicateEntry(int? id)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.CheckDuplicateEntry` → `RequisitionDAL.CheckDuplicateEntry` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore` | — |
| `Save` | `aRequisitionBll.UpdateTempReceiveId(...)` | `RequisitionBLL.UpdateTempReceiveId()` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.UpdateTempReceiveId` → `RequisitionDAL.UpdateTempReceiveId` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_UD_ReceiveIdInDc` |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `backLinkButton` | asp:LinkButton |  | — | `backLinkButton_Click` | 3 lines | — | — |
| `submitButton` | asp:Button | Submit | return confirm('Are you sure you want to Save ?'); | `submitButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 2
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 8 | — | — | — | — |
| 2 | `LoadInfo` | `void LoadInfo(string reqId)` | 10 | `aChalanBll.GetChalanReceieve` | — | — | — |
| 3 | `LoadGrid` | `void LoadGrid(string reqId)` | 5 | `aChalanBll.GetChalanReceieve` | — | — | — |
| 4 | `Save` | `void Save()` | 98 | `aChalanBll.DCStockIn2`, `aChalanBll.SaveDCStoreFreeze2`, `aRequisitionBll.CheckDuplicateEntry`, `aRequisitionBll.UpdateTempReceiveId` | — | — | — |
| 5 | `Popup` | `void Popup(string reqId)` | 6 | — | — | — | — |
| 6 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 7 | `Clear` | `void Clear()` | 12 | — | — | — | — |
| 8 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 9 | `damageTextBox_TextChanged` | `void damageTextBox_TextChanged(object sender, EventArgs e)` | 10 | — | — | — | — |
| 10 | `rcvQtyTextBox_TextChanged` | `void rcvQtyTextBox_TextChanged(object sender, EventArgs e)` | 10 | — | — | — | — |
| 11 | `backLinkButton_Click` | `void backLinkButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
TransferReceiveProductByChalanByDC.aspx (markup)
  -> SInventory_UI_ReceiveProductByChalanByDC (code-behind event handler)
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
| `LoadInfo` | `ChalanBLL` | `GetChalanReceieve` | `string id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `LoadGrid` | `ChalanBLL` | `GetChalanReceieve` | `string id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `Save` | `ChalanBLL` | `DCStockIn2` | `DCStockNew aDcStockNew` | `int` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `Save` | `ChalanBLL` | `SaveDCStoreFreeze2` | `DCStoreFreezeDAO aDcStockNew` | `string` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `Save` | `RequisitionBLL` | `CheckDuplicateEntry` | `int? id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `Save` | `RequisitionBLL` | `UpdateTempReceiveId` | `` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Process_WeightedAverage` | Save → `ChalanBLL.DCStockIn2 → ChalanDAL.ChalanUpdate → ClsCommonInternalDAL.UpdateDataByUpdateCommand → ChalanDAL.CheckB2BExistOrNotInDcStore → ClsCommonInternalDAL.DataContainerDataTable → ChalanDAL.DCStockInDALMain` | **No** — see note |
| `sp_UD_ReceiveIdInDc` | Save → `ChalanBLL.DCStockIn2 → ChalanDAL.ChalanUpdate → ClsCommonInternalDAL.UpdateDataByUpdateCommand → ChalanDAL.CheckB2BExistOrNotInDcStore → ClsCommonInternalDAL.DataContainerDataTable → ChalanDAL.DCStockInDALMain` | **No** — see note |
| `sp_Process_WeightedAverageFreezeStock` | Save → `ChalanBLL.SaveDCStoreFreeze2 → ChalanDAL.ProcessWeightedAverageFreezeStock → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblChalanDetail` | `LoadGrid()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInfo()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblChalanInfo` | `LoadGrid()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInfo()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable`, `Save()` → `ChalanBLL.DCStockIn2` → `ChalanDAL.ChalanUpdate` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `ChalanDAL.CheckB2BExistOrNotInDcStore` |
| `tblCompanyUnit` | `LoadGrid()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInfo()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStore` | `LoadGrid()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInfo()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable`, `Save()` → `ChalanBLL.DCStockIn2` → `ChalanDAL.ChalanUpdate` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `ChalanDAL.CheckB2BExistOrNotInDcStore`, `Save()` → `RequisitionBLL.CheckDuplicateEntry` → `RequisitionDAL.CheckDuplicateEntry` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDCStoreFreeze` | `Save()` → `ChalanBLL.SaveDCStoreFreeze2` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` |
| `tblProduct` | `LoadGrid()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadInfo()` → `ChalanBLL.GetChalanReceieve` → `ChalanDAL.GetChalanReceieve` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-DLV-018-001` | {0} | Code | `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Database object | `tblChalanDetail` |
| Database object | `tblChalanInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblDCStore` |
| Database object | `tblDCStoreFreeze` |
| Database object | `tblProduct` |
| Stored procedure | `sp_Process_WeightedAverage` |
| Stored procedure | `sp_UD_ReceiveIdInDc` |
| Stored procedure | `sp_Process_WeightedAverageFreezeStock` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-DLV-018-001` | Unauthenticated access | Request `/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-DLV-018-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-DLV-018-003` | Grid loads | Open the page with data present | Grid `rcvGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 28 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 11 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 3 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 6 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/ChalanBLL.cs` is **shared with 6 other page(s)** (e.g. `DcToDcChalanReportViewer.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`, `NewStockTransferDcToDc.aspx`, `StockReceiveReportByDc.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RequisitionBLL.cs` is **shared with 29 other page(s)** (e.g. `Dashboard.aspx`, `B2BReceiveViewer.aspx`, `StockReceiveReportViewer.aspx`, `TransferStockReceiveReportViewer.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- Writes/reads `tblChalanDetail` — any schema change to it breaks this page.
- Writes/reads `tblChalanInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDCStoreFreeze` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
