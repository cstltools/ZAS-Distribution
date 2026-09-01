# SAL-008 — InvoiceCreationByOrder

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-008` |
| Module | `sales-order-invoice` |
| File name | `InvoiceCreationByOrder.aspx` |
| Route | `/SInventory_UI/InvoiceCreationByOrder.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `InvoiceCreationByOrder.aspx.cs` |
| Code-behind class | `SInventory_UI_InvoiceCreationByOrder` |
| Markup lines | 434 |
| Code-behind lines | 273 |
| Server controls | 27 |
| GridViews | 1 |
| Code-behind methods | 13 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/InvoiceCreationForCustomerByOrder.aspx`

Outbound `Response.Redirect` targets: `InvoiceCreationForCustomerByOrder.aspx`, `Login.aspx`

## 5. Route

`/SInventory_UI/InvoiceCreationByOrder.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 7. UI Control Inventory

27 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 7 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `orderDateTextBox` | asp:TextBox | Order Date: |  |  |  |  |  | 39 |
| 5 | `imgDate` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 40 |
| 6 | `Date` | asp:CalendarExtender |  |  |  |  |  |  | 42 |
| 7 | `manufacDropDownList` | asp:DropDownList | Company: |  | True |  |  | manufacDropDownList_SelectedIndexChanged | 79 |
| 8 | `salesCenterDropDownList` | asp:DropDownList | Sales Center: |  | True |  |  | salesCenterDropDownList_SelectedIndexChanged | 87 |
| 9 | `areaDropDownList` | asp:DropDownList | Area: |  | True |  |  | areaDropDownList_OnSelectedIndexChanged | 95 |
| 10 | `territoryDropDownList` | asp:DropDownList | Territory: |  | True |  |  | territoryDropDownList_OnSelectedIndexChanged | 105 |
| 11 | `marketDropDownList` | asp:DropDownList | Market: |  | True |  |  | marketDropDownList_SelectedIndexChanged | 113 |
| 12 | `custDropDownList` | asp:DropDownList | Customer: |  | True |  |  |  | 121 |
| 13 | `Button1` | asp:Button |  | Search |  |  |  | Button1_Click | 171 |
| 14 | `orderGridView` | asp:GridView |  |  |  |  |  |  | 230 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 233 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 234 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 235 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 236 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 237 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 238 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 240 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 241 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 242 |
| 24 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 243 |
| 25 | `gotoinvoiceButton` | asp:Button | orderGridView → col[9] "Go To Invoice" | Go To Invoice |  |  |  | gotoinvoiceButton_Click | 245 |
| 26 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 424 |
| 27 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 427 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `orderGridView`

| Attribute | Value |
|---|---|
| datakeynames | `ComUnitId,ManufacId,OrderId,Permission` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Order No | OrderCode | — | — | — |
| 1 | BoundField | Order Date | SubmissionDate | — | — | — |
| 2 | BoundField | Customer Code | CustomerCode | — | — | — |
| 3 | BoundField | Customer Name | CustomerName | — | — | — |
| 4 | BoundField | Market | MarketName | — | — | — |
| 5 | BoundField | MIO | MIOCode | — | — | — |
| 6 | BoundField | Remarks | Remarks | — | — | — |
| 7 | BoundField | Value(Due + Current Order) | ReceiveAble | — | — | — |
| 8 | BoundField | Credit Limit | LimitAmount | — | — | — |
| 9 | TemplateField | Go To Invoice | — | — | `Button:gotoinvoiceButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `DropDownlist` | `aOrderInfoBll.LoadManufac(...)` | `OrderInfoBLL.LoadManufac(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyInfo` | — |
| `DropDownlist` | `aOrderInfoBll.LoadSC(...)` | `OrderInfoBLL.LoadSC(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadSC` → `OrderInfoDAL.LoadSC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit` | — |
| `salesCenterDropDownList_SelectedIndexChanged` | `aOrderInfoBll.AreaLoad(...)` | `OrderInfoBLL.AreaLoad(DropDownList aDownList, string dcId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.AreaLoad` | — | — | — |
| `salesCenterDropDownList_SelectedIndexChanged` | `aOrderInfoBll.LoadMarketOrderWise(...)` | `OrderInfoBLL.LoadMarketOrderWise(DropDownList ddl, string comunitId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadMarketOrderWise` → `OrderInfoDAL.LoadMarketOrderWise` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea`, `tblDcWiseAreaInfo`, `tblMarket`, `tblOrder`, `tblTerritory` | — |
| `manufacDropDownList_SelectedIndexChanged` | `aOrderInfoBll.LoadDC(...)` | `OrderInfoBLL.LoadDC(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadDC` → `OrderInfoDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `manufacDropDownList_SelectedIndexChanged` | `aOrderInfoBll.LoadDcInformationByUser(...)` | `OrderInfoBLL.LoadDcInformationByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadDcInformationByUser` → `OrderInfoDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `marketDropDownList_SelectedIndexChanged` | `aOrderInfoBll.CustomerLoad(...)` | `OrderInfoBLL.CustomerLoad(DropDownList aDownList, string marketId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.CustomerLoad` → `OrderInfoDAL.CustomerLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCustMaster` | — |
| `marketDropDownList_SelectedIndexChanged` | `aOrderInfoBll.LoadOrderForOrderCreation(...)` | `OrderInfoBLL.LoadOrderForOrderCreation(string comunitId, string manufacId, string marketId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblMarket`, `tblOrder` | — |
| `SessionChoose` | `aOrderInfoBll.LoadOrderForOrderCreation(...)` | `OrderInfoBLL.LoadOrderForOrderCreation(string comunitId, string manufacId, string marketId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblMarket`, `tblOrder` | — |
| `GridView` | `aOrderInfoBll.LoadOrderForOrderCreation(...)` | `OrderInfoBLL.LoadOrderForOrderCreation(string comunitId, string manufacId, string marketId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblMarket`, `tblOrder` | — |
| `areaDropDownList_OnSelectedIndexChanged` | `aOrderInfoBll.TerritoryLoad(...)` | `OrderInfoBLL.TerritoryLoad(DropDownList aDownList, string areaId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.TerritoryLoad` | — | — | — |
| `territoryDropDownList_OnSelectedIndexChanged` | `aOrderInfoBll.MarketLoad(...)` | `OrderInfoBLL.MarketLoad(DropDownList aDownList, string territoryId)` <br/>`Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | `OrderInfoBLL.MarketLoad` | — | — | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `manufacDropDownList` | Company: | asp:DropDownList | True | manufacDropDownList_SelectedIndexChanged | `aOrderInfoBll.LoadDC()` in `manufacDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadDcInformationByUser()` in `manufacDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadManufac()` in `DropDownlist()`; `aOrderInfoBll.LoadOrderForOrderCreation()` in `marketDropDownList_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblMarket`, `tblOrder`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `salesCenterDropDownList` | Sales Center: | asp:DropDownList | True | salesCenterDropDownList_SelectedIndexChanged | `aOrderInfoBll.AreaLoad()` in `salesCenterDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadDC()` in `manufacDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadDcInformationByUser()` in `manufacDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadMarketOrderWise()` in `salesCenterDropDownList_SelectedIndexChanged()` | `tblArea`, `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblMarket`, `tblOrder`, `tblTerritory`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `areaDropDownList` | Area: | asp:DropDownList | True | areaDropDownList_OnSelectedIndexChanged | `aOrderInfoBll.AreaLoad()` in `salesCenterDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.TerritoryLoad()` in `areaDropDownList_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory: | asp:DropDownList | True | territoryDropDownList_OnSelectedIndexChanged | `aOrderInfoBll.MarketLoad()` in `territoryDropDownList_OnSelectedIndexChanged()`; `aOrderInfoBll.TerritoryLoad()` in `areaDropDownList_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `marketDropDownList` | Market: | asp:DropDownList | True | marketDropDownList_SelectedIndexChanged | `aOrderInfoBll.CustomerLoad()` in `marketDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadMarketOrderWise()` in `salesCenterDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.LoadOrderForOrderCreation()` in `marketDropDownList_SelectedIndexChanged()`; `aOrderInfoBll.MarketLoad()` in `territoryDropDownList_OnSelectedIndexChanged()` | `tblArea`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblMarket`, `tblOrder`, `tblTerritory` | set in code (see the DAL method above) |
| `custDropDownList` | Customer: | asp:DropDownList | True | — | `aOrderInfoBll.CustomerLoad()` in `marketDropDownList_SelectedIndexChanged()` | `tblCustMaster` | set in code (see the DAL method above) |

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
| `imgDate` | asp:ImageButton |  | — | `—` | — | — | — |
| `Button1` | asp:Button | Search | — | `Button1_Click` | 43 lines | — | — |
| `gotoinvoiceButton` | asp:Button | Go To Invoice | — | `gotoinvoiceButton_Click` | 10 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 15 | — | — | — | `MarketId` |
| 2 | `DropDownlist` | `void DropDownlist()` | 9 | `aOrderInfoBll.LoadManufac`, `aOrderInfoBll.LoadSC` | — | — | `UserId` |
| 3 | `salesCenterDropDownList_SelectedIndexChanged` | `void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 6 | `aOrderInfoBll.AreaLoad`, `aOrderInfoBll.LoadMarketOrderWise` | — | — | — |
| 4 | `manufacDropDownList_SelectedIndexChanged` | `void manufacDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 28 | `aOrderInfoBll.LoadDC`, `aOrderInfoBll.LoadDcInformationByUser` | — | — | `UserId`, `UserType` |
| 5 | `marketDropDownList_SelectedIndexChanged` | `void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 8 | `aOrderInfoBll.CustomerLoad`, `aOrderInfoBll.LoadOrderForOrderCreation` | — | — | — |
| 6 | `gotoinvoiceButton_Click` | `void gotoinvoiceButton_Click(object sender, EventArgs e)` | 10 | — | — | — | `OrderId` |
| 7 | `SessionChoose` | `void SessionChoose()` | 16 | `aOrderInfoBll.LoadOrderForOrderCreation` | — | — | `MarketId` |
| 8 | `GridView` | `void GridView(string parametr)` | 35 | `aOrderInfoBll.LoadOrderForOrderCreation` | — | — | `MarketId` |
| 9 | `GenerateParameter` | `string GenerateParameter()` | 58 | — | — | — | — |
| 10 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 11 | `Button1_Click` | `void Button1_Click(object sender, EventArgs e)` | 43 | — | — | — | `UserType` |
| 12 | `areaDropDownList_OnSelectedIndexChanged` | `void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 3 | `aOrderInfoBll.TerritoryLoad` | — | — | — |
| 13 | `territoryDropDownList_OnSelectedIndexChanged` | `void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 3 | `aOrderInfoBll.MarketLoad` | — | — | — |

Call chain shape for every persisting action on this page:

```
InvoiceCreationByOrder.aspx (markup)
  -> SInventory_UI_InvoiceCreationByOrder (code-behind event handler)
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
| `DropDownlist` | `OrderInfoBLL` | `LoadManufac` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `DropDownlist` | `OrderInfoBLL` | `LoadSC` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `salesCenterDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `AreaLoad` | `DropDownList aDownList, string dcId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `salesCenterDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `LoadMarketOrderWise` | `DropDownList ddl, string comunitId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `manufacDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `manufacDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `LoadDcInformationByUser` | `DropDownList ddl, string companyId, string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `marketDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `CustomerLoad` | `DropDownList aDownList, string marketId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `marketDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `LoadOrderForOrderCreation` | `string comunitId, string manufacId, string marketId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `SessionChoose` | `OrderInfoBLL` | `LoadOrderForOrderCreation` | `string comunitId, string manufacId, string marketId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `GridView` | `OrderInfoBLL` | `LoadOrderForOrderCreation` | `string comunitId, string manufacId, string marketId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `OrderInfoBLL` | `TerritoryLoad` | `DropDownList aDownList, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `territoryDropDownList_OnSelectedIndexChanged` | `OrderInfoBLL` | `MarketLoad` | `DropDownList aDownList, string territoryId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `salesCenterDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadMarketOrderWise` → `OrderInfoDAL.LoadMarketOrderWise` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyInfo` | `DropDownlist()` → `OrderInfoBLL.LoadManufac` → `OrderInfoDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyUnit` | `DropDownlist()` → `OrderInfoBLL.LoadSC` → `OrderInfoDAL.LoadSC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `GridView()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `SessionChoose()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `manufacDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadDC` → `OrderInfoDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `manufacDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadDcInformationByUser` → `OrderInfoDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.CustomerLoad` → `OrderInfoDAL.CustomerLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblDcWiseAreaInfo` | `salesCenterDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadMarketOrderWise` → `OrderInfoDAL.LoadMarketOrderWise` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblMarket` | `GridView()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `SessionChoose()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `salesCenterDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadMarketOrderWise` → `OrderInfoDAL.LoadMarketOrderWise` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblOrder` | `GridView()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `SessionChoose()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadOrderForOrderCreation` → `OrderInfoDAL.LoadOrderForOrderCreation` → `ClsCommonInternalDAL.DataContainerDataTable`, `salesCenterDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadMarketOrderWise` → `OrderInfoDAL.LoadMarketOrderWise` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblTerritory` | `salesCenterDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadMarketOrderWise` → `OrderInfoDAL.LoadMarketOrderWise` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblUserCompanyUnit` | `manufacDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadDC` → `OrderInfoDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `manufacDropDownList_SelectedIndexChanged()` → `OrderInfoBLL.LoadDcInformationByUser` → `OrderInfoDAL.GetDcInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `GridView` | Generate Invoice | No | Yes | UNKNOWN | Notification |
| `GridView` | Unable to Access | No | Yes | UNKNOWN | Notification |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-008-001` | Generate Invoice | Code | `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs` → `GridView()` | user action on this page | operation is refused | "Generate Invoice" | CONFIRMED |
| `BR-SAL-008-002` | Unable to Access | Code | `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs` → `GridView()` | user action on this page | operation is refused | "Unable to Access" | CONFIRMED |
| `BR-SAL-008-003` | {0} | Code | `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `MarketId`, `OrderId`, `UserId`, `UserType`

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
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblTerritory` |
| Database object | `tblUserCompanyUnit` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-008-001` | Unauthenticated access | Request `/SInventory_UI/InvoiceCreationByOrder.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-008-002` | Validation: Generate Invoice | Trigger `GridView()` in the state that produces this message | alert "Generate Invoice" and the operation is not performed | CONFIRMED |
| `TC-SAL-008-003` | Validation: Unable to Access | Trigger `GridView()` in the state that produces this message | alert "Unable to Access" and the operation is not performed | CONFIRMED |
| `TC-SAL-008-004` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-008-005` | Grid loads | Open the page with data present | Grid `orderGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 27 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 13 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 9 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
