# SAL-026 — SampleProductIssueApproval

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-026` |
| Module | `sales-order-invoice` |
| File name | `SampleProductIssueApproval.aspx` |
| Route | `/SInventory_UI/SampleProductIssueApproval.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `SampleProductIssueApproval.aspx.cs` |
| Code-behind class | `SInventory_UI_SampleProductIssueApproval` |
| Markup lines | 335 |
| Code-behind lines | 172 |
| Server controls | 24 |
| GridViews | 1 |
| Code-behind methods | 9 |

## 2. Purpose

Approval screen. It lists submitted records awaiting a decision and lets an authorised user approve, reject or cancel them, which advances the record status. Which actions the current user may take is resolved at run time by `Library.DAL/InternalCls/ClsApprovalAction.cs`. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Approval screen. It lists submitted records awaiting a decision and lets an authorised user approve, reject or cancel them, which advances the record status. Which actions the current user may take is resolved at run time by `Library.DAL/InternalCls/ClsApprovalAction.cs`. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/SampleProductIssueApproval.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/SampleProductIssueApproval.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/SampleProductIssueApproval.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/PettyCashDaL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SlaveApprovalDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs` |

## 7. UI Control Inventory

24 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 45 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 46 |
| 4 | `actionRadioButtonList` | asp:RadioButtonList | Take Action : |  |  |  |  |  | 82 |
| 5 | `btnSubmit` | asp:Button |  | Submit Action |  |  |  | btnSubmit0_Click | 89 |
| 6 | `loadGridView` | asp:GridView |  |  |  |  |  |  | 115 |
| 7 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 118 |
| 8 | `chkSelectAll` | asp:CheckBox | loadGridView → col[0] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 120 |
| 9 | `chkSelect` | asp:CheckBox | loadGridView → col[0] "(no header)" |  |  |  |  |  | 123 |
| 10 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 126 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 127 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 128 |
| 13 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 129 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 130 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 131 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 132 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 133 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 134 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 135 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 136 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 137 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 138 |
| 23 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 326 |
| 24 | `imgWait` | asp:Image |  | ~/images/loading-icon-big.gif |  |  |  |  | 330 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `OrderId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |
| 1 | BoundField | Order Code | OrderCode | — | — | — |
| 2 | BoundField | Company Name | CompanyName | — | — | — |
| 3 | BoundField | Unit Name | ComUnitName | — | — | — |
| 4 | BoundField | Order Date | SubmissionDate | — | — | — |
| 5 | BoundField | Customer Name | CustomerName | — | — | — |
| 6 | BoundField | MIO Name | MIOName | — | — | — |
| 7 | BoundField | Menufacturer Name | ManufacName | — | — | — |
| 8 | BoundField | Market Name | MarketName | — | — | — |
| 9 | BoundField | Territory Name | TerritoryName | — | — | — |
| 10 | BoundField | Region Name | RegionName | — | — | — |
| 11 | BoundField | Area Name | AreaName | — | — | — |
| 12 | BoundField | Delivery Man | DeliveryMan | — | — | — |
| 13 | BoundField | Approval Status | ActionStatus | — | — | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `ManualOrderLoad` | `aOrderListBll.LoadApprovalControlBLL(...)` | `OrderListBLL.LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)` <br/>`Library.BLL/SInventory_BLL/OrderListBLL.cs` | `OrderListBLL.LoadApprovalControlBLL` → `OrderListDAL.LoadApprovalControlDAL` → `ClsApprovalAction.LoadActionControlByUser` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblAction`, `tblActionPageWiseStep`, `tblActionStepDetail`, `tblActionSteps`, `tblActionUserWiseApproval`, `tblMainMenu` | — |
| `ManualOrderCondition` | `aOrderCreationDal.GetSpecialApproveOrder(...)` | `DestroyProductDal.GetSpecialApproveOrder(string actionstatus)` <br/>`Library.DAL/SInventory_DAL/DestroyProductDal.cs` | `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `PettyCashDaL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetSpecialApproveOrder` → `OrderListBLL.GetSpecialApproveOrder` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblDeliveryManInfo`, `tblDestroyProduct`, `tblEmpGeneralInfo`, `tblInvoice`, `tblMIOInfo`, `tblManufacturer`, `tblMarket`, `tblOrder`, `tblOrderDetail`, `tblPettyCashDetails`, `tblPettyCashMaster`, `tblRegion`, `tblSampleIssue`, `tblTerritory`, `tblUser` | — |
| `ManualOrderCondition` | `aOrderListBll.GetSpecialApproveOrder(...)` | `OrderListBLL.GetSpecialApproveOrder(string actionstatus)` <br/>`Library.BLL/SInventory_BLL/OrderListBLL.cs` | `OrderListBLL.GetSpecialApproveOrder` | — | — | — |
| `ManualOrderCondition` | `aOrderListBll.LoadForApprovalConditionBLL(...)` | `OrderListBLL.LoadForApprovalConditionBLL(string pageName, string userName)` <br/>`Library.BLL/SInventory_BLL/OrderListBLL.cs` | `OrderListBLL.LoadForApprovalConditionBLL` → `OrderListDAL.LoadForApprovalConditionDAL` → `ClsApprovalAction.LoadForApprovalByUserCondition` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblActionPageWiseStep`, `tblActionStepDetail`, `tblActionSteps`, `tblActionUserWiseApproval`, `tblMainMenu` | — |
| `SubmitApproval` | `aOrderCreationDal.ApprovalUpdateDAL(...)` | `CreditLimitDAL.ApprovalUpdateDAL(CreditLimitDAO aCreditLimitDao)` <br/>`Library.DAL/SInventory_DAL/CreditLimitDAL.cs` | `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `DestroyProductDal.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `OrderListDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblCustomerCreditLimit`, `tblCustomerReturnMaster`, `tblDestroyProduct`, `tblOrder`, `tblPettyCashMaster`, `tblProductDiscount`, `tblSlaveInfo`, `tblUnitPrice`, `tblWHStockInMaster` | `sp_UD_SampleIssue` |
| `SubmitApproval` | `aOrderCreationDal.CheckAvilAbleQty(...)` | `SampleIssueDal.CheckAvilAbleQty(int orderId)` <br/>`Library.DAL/SInventory_DAL/SampleIssueDal.cs` | `SampleIssueDal.CheckAvilAbleQty` | SELECT | `tblDCStore`, `tblSampleIssue`, `tblSampleIssueDetail` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `actionRadioButtonList` | Take Action : | asp:RadioButtonList | False | — | `aOrderListBll.LoadApprovalControlBLL()` in `ManualOrderLoad()` | `tblAction`, `tblActionPageWiseStep`, `tblActionStepDetail`, `tblActionSteps`, `tblActionUserWiseApproval`, `tblMainMenu` | set in code (see the DAL method above) |

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
| `btnSubmit` | asp:Button | Submit Action | — | `btnSubmit0_Click` | 4 lines | — | — |

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
| 2 | `btnSubmit0_Click` | `void btnSubmit0_Click(object sender, EventArgs e)` | 4 | — | — | — | — |
| 3 | `ManualOrderLoad` | `void ManualOrderLoad()` | 7 | `aOrderListBll.LoadApprovalControlBLL` | — | — | `LoginName` |
| 4 | `ManualOrderCondition` | `void ManualOrderCondition(string pageName, string user)` | 7 | `aOrderCreationDal.GetSpecialApproveOrder`, `aOrderListBll.GetSpecialApproveOrder`, `aOrderListBll.LoadForApprovalConditionBLL` | — | — | — |
| 5 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 6 | `SubmitApproval` | `void SubmitApproval()` | 58 | `aOrderCreationDal.ApprovalUpdateDAL`, `aOrderCreationDal.CheckAvilAbleQty` | — | — | `LoginName` |
| 7 | `Validation` | `bool Validation()` | 26 | — | — | — | — |
| 8 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 9 | `Button41_OnClick` | `void Button41_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
SampleProductIssueApproval.aspx (markup)
  -> SInventory_UI_SampleProductIssueApproval (code-behind event handler)
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
| `ManualOrderLoad` | `OrderListBLL` | `LoadApprovalControlBLL` | `RadioButtonList rdl, string pageName, string userName` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `ManualOrderCondition` | `OrderListBLL` | `GetSpecialApproveOrder` | `string actionstatus` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `ManualOrderCondition` | `PettyCashBLL` | `GetSpecialApproveOrder` | `string actionstatus` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `ManualOrderCondition` | `OrderListBLL` | `LoadForApprovalConditionBLL` | `string pageName, string userName` | `string` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `SubmitApproval` | `CustomerReturnBLL` | `ApprovalUpdateDAL` | `CustomerReturnMasterDAO aCustomerReturnMasterDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| `SubmitApproval` | `OrderListBLL` | `ApprovalUpdateDAL` | `OrderInfoMaster aOrderInfoMaster` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `SubmitApproval` | `PettyCashBLL` | `ApprovalUpdateDAL` | `PettyCashMasterDao aCashMasterDao` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| `SubmitApproval` | `ProductDiscountBLL` | `ApprovalUpdateDAL` | `ProductDiscount aProductDiscount` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `SubmitApproval` | `UnitPriceBLL` | `ApprovalUpdateDAL` | `ProductUnitPrice aProductUnitPrice` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `ManualOrderCondition` | `DestroyProductDal` | `GetSpecialApproveOrder` | `string actionstatus` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblDestroyProduct`, `tblUser` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `ManualOrderCondition` | `OrderListDAL` | `GetSpecialApproveOrder` | `string actionstatus` | `DataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblMIOInfo`, `tblMarket`, `tblOrder`, `tblOrderDetail`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `ManualOrderCondition` | `PettyCashDaL` | `GetSpecialApproveOrder` | `string actionstatus` | `DataTable` | — | `tblPettyCashDetails`, `tblPettyCashMaster` | — | `Library.DAL/SInventory_DAL/PettyCashDaL.cs` |
| `ManualOrderCondition` | `SampleIssueDal` | `GetSpecialApproveOrder` | `string actionstatus` | `DataTable` | SELECT | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblManufacturer`, `tblMarket`, `tblRegion`, `tblSampleIssue`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SubmitApproval` | `CreditLimitDAL` | `ApprovalUpdateDAL` | `CreditLimitDAO aCreditLimitDao` | `bool` | UPDATE | `tblCustomerCreditLimit` | — | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| `SubmitApproval` | `CustomerReturnDAL` | `ApprovalUpdateDAL` | `CustomerReturnMasterDAO aCustomerReturnMasterDao` | `bool` | UPDATE | `tblCustomerReturnMaster` | — | `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs` |
| `SubmitApproval` | `DestroyProductDal` | `ApprovalUpdateDAL` | `DestroyProductDao aOrderInfoMaster` | `bool` | UPDATE | `tblDestroyProduct` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `SubmitApproval` | `OrderListDAL` | `ApprovalUpdateDAL` | `OrderInfoMaster aOrderInfoMaster` | `bool` | UPDATE | `tblOrder` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `SubmitApproval` | `PettyCashDaL` | `ApprovalUpdateDAL` | `PettyCashMasterDao aCashMasterDao` | `bool` | UPDATE | `tblPettyCashMaster` | — | `Library.DAL/SInventory_DAL/PettyCashDaL.cs` |
| `SubmitApproval` | `ProductDiscountDAL` | `ApprovalUpdateDAL` | `ProductDiscount aProductDiscount` | `bool` | UPDATE | `tblProductDiscount` | — | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| `SubmitApproval` | `ProductUnitPriceDAL` | `ApprovalUpdateDAL` | `ProductUnitPrice aProductUnitPrice` | `bool` | UPDATE | `tblUnitPrice` | — | `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs` |
| `SubmitApproval` | `SampleIssueDal` | `ApprovalUpdateDAL` | `OrderInfoMaster aOrderInfoMaster` | `bool` | — | — | `sp_UD_SampleIssue` | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SubmitApproval` | `SlaveApprovalDal` | `ApprovalUpdateDAL` | `SlaveInformation aCreditLimitDao` | `bool` | UPDATE | `tblSlaveInfo` | — | `Library.DAL/SInventory_DAL/SlaveApprovalDal.cs` |
| `SubmitApproval` | `WarehouseStockInApprovalDal` | `ApprovalUpdateDal` | `WarehouseStockInMasterDao aMasterDao` | `void` | UPDATE | `tblWHStockInMaster` | — | `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs` |
| `SubmitApproval` | `SampleIssueDal` | `CheckAvilAbleQty` | `int orderId` | `DataTable` | SELECT | `tblDCStore`, `tblSampleIssue`, `tblSampleIssueDetail` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_UD_SampleIssue` | SubmitApproval → `CreditLimitDAL.ApprovalUpdateDAL → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustomerReturnDAL.ApprovalUpdateDAL → ClsCommonInternalDAL.UpdateDataByUpdateCommand → DestroyProductDal.ApprovalUpdateDAL → ClsCommonInternalDAL.UpdateDataByUpdateCommand` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblAction` | `ManualOrderLoad()` → `OrderListBLL.LoadApprovalControlBLL` → `OrderListDAL.LoadApprovalControlDAL` → `ClsApprovalAction.LoadActionControlByUser` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblActionPageWiseStep` | `ManualOrderCondition()` → `OrderListBLL.LoadForApprovalConditionBLL` → `OrderListDAL.LoadForApprovalConditionDAL` → `ClsApprovalAction.LoadForApprovalByUserCondition` → `ClsCommonInternalDAL.DataContainerDataTable`, `ManualOrderLoad()` → `OrderListBLL.LoadApprovalControlBLL` → `OrderListDAL.LoadApprovalControlDAL` → `ClsApprovalAction.LoadActionControlByUser` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblActionStepDetail` | `ManualOrderCondition()` → `OrderListBLL.LoadForApprovalConditionBLL` → `OrderListDAL.LoadForApprovalConditionDAL` → `ClsApprovalAction.LoadForApprovalByUserCondition` → `ClsCommonInternalDAL.DataContainerDataTable`, `ManualOrderLoad()` → `OrderListBLL.LoadApprovalControlBLL` → `OrderListDAL.LoadApprovalControlDAL` → `ClsApprovalAction.LoadActionControlByUser` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblActionSteps` | `ManualOrderCondition()` → `OrderListBLL.LoadForApprovalConditionBLL` → `OrderListDAL.LoadForApprovalConditionDAL` → `ClsApprovalAction.LoadForApprovalByUserCondition` → `ClsCommonInternalDAL.DataContainerDataTable`, `ManualOrderLoad()` → `OrderListBLL.LoadApprovalControlBLL` → `OrderListDAL.LoadApprovalControlDAL` → `ClsApprovalAction.LoadActionControlByUser` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblActionUserWiseApproval` | `ManualOrderCondition()` → `OrderListBLL.LoadForApprovalConditionBLL` → `OrderListDAL.LoadForApprovalConditionDAL` → `ClsApprovalAction.LoadForApprovalByUserCondition` → `ClsCommonInternalDAL.DataContainerDataTable`, `ManualOrderLoad()` → `OrderListBLL.LoadApprovalControlBLL` → `OrderListDAL.LoadApprovalControlDAL` → `ClsApprovalAction.LoadActionControlByUser` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblArea` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerCreditLimit` | `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblCustomerReturnMaster` | `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblDCStore` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable`, `SubmitApproval()` → `SampleIssueDal.CheckAvilAbleQty` |
| `tblDCStoreFreeze` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDeliveryManInfo` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblDestroyProduct` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable`, `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblEmpGeneralInfo` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblInvoice` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMIOInfo` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMainMenu` | `ManualOrderCondition()` → `OrderListBLL.LoadForApprovalConditionBLL` → `OrderListDAL.LoadForApprovalConditionDAL` → `ClsApprovalAction.LoadForApprovalByUserCondition` → `ClsCommonInternalDAL.DataContainerDataTable`, `ManualOrderLoad()` → `OrderListBLL.LoadApprovalControlBLL` → `OrderListDAL.LoadApprovalControlDAL` → `ClsApprovalAction.LoadActionControlByUser` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblManufacturer` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblMarket` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable`, `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblOrderDetail` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPettyCashDetails` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblPettyCashMaster` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable`, `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblProductDiscount` | `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblRegion` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblSampleIssue` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable`, `SubmitApproval()` → `SampleIssueDal.CheckAvilAbleQty` |
| `tblSampleIssueDetail` | `SubmitApproval()` → `SampleIssueDal.CheckAvilAbleQty` |
| `tblSlaveInfo` | `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblTerritory` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblUser` | `ManualOrderCondition()` → `DestroyProductDal.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderListDAL.GetSpecialApproveOrder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblWHStockInMaster` | `SubmitApproval()` → `CreditLimitDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustomerReturnDAL.ApprovalUpdateDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-026-001` | {0} | Code | `Solution.Web/SInventory_UI/SampleProductIssueApproval.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/PettyCashDaL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SlaveApprovalDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs` |
| Database object | `tblAction` |
| Database object | `tblActionPageWiseStep` |
| Database object | `tblActionStepDetail` |
| Database object | `tblActionSteps` |
| Database object | `tblActionUserWiseApproval` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustomerCreditLimit` |
| Database object | `tblCustomerReturnMaster` |
| Database object | `tblDCStore` |
| Database object | `tblDCStoreFreeze` |
| Database object | `tblDeliveryManInfo` |
| Database object | `tblDestroyProduct` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblMIOInfo` |
| Database object | `tblMainMenu` |
| Database object | `tblManufacturer` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblOrderDetail` |
| Database object | `tblPettyCashDetails` |
| Database object | `tblPettyCashMaster` |
| Database object | `tblProductDiscount` |
| Database object | `tblRegion` |
| Database object | `tblSampleIssue` |
| Database object | `tblSampleIssueDetail` |
| Database object | `tblSlaveInfo` |
| Database object | `tblTerritory` |
| Database object | `tblUnitPrice` |
| Database object | `tblUser` |
| Database object | `tblWHStockInMaster` |
| Stored procedure | `sp_UD_SampleIssue` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-026-001` | Unauthenticated access | Request `/SInventory_UI/SampleProductIssueApproval.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-026-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-026-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/SampleProductIssueApproval.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/SampleProductIssueApproval.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/PettyCashDaL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SlaveApprovalDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 24 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 9 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 33 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` is **shared with 4 other page(s)** (e.g. `PettyCashApproval.aspx`, `ReturnProductConsCustomerByInvoice.aspx`, `ReturnProductCustomerByInvoice.aspx`, `ReturnustomerByInvoiceApproval.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderListBLL.cs` is **shared with 20 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/PettyCashBLL.cs` is **shared with 10 other page(s)** (e.g. `CreditCustomerViewer.aspx`, `CreditReportAllViewer.aspx`, `ProductDiscountReportAllViewer.aspx`, `ProductDiscountReportViewer.aspx`, `CreditCustomerReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` is **shared with 14 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DiscountApproval.aspx`, `DistributionCenterInfoView.aspx`, `ManualOrderrCreation.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` is **shared with 7 other page(s)** (e.g. `ProductCostPriceViewer.aspx`, `ProductTreadPriceViewer.aspx`, `PettyCashApproval.aspx`, `ProUnitPriceApprove.aspx`, `ProUnitPriceEdit.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` is **shared with 6 other page(s)** (e.g. `CustomerCreditLimitApproval.aspx`, `CustomerCreditLimitEntry.aspx`, `CustomerCreditView.aspx`, `CustomerPaymentProcess.aspx`, `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs` is **shared with 1 other page(s)** (e.g. `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DestroyProductDal.cs` is **shared with 55 other page(s)** (e.g. `DestroyProductReportViewer.aspx`, `DestroyProductStatementViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OrderListDAL.cs` is **shared with 17 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/PettyCashDaL.cs` is **shared with 3 other page(s)** (e.g. `CreditCustomerViewer.aspx`, `CreditReportAllViewer.aspx`, `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` is **shared with 8 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`, `ManualOrderrCreation.aspx`, `NonTranscationalInvoiceReturn.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs` is **shared with 1 other page(s)** (e.g. `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SampleIssueDal.cs` is **shared with 17 other page(s)** (e.g. `SampleIssueReportViewer.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SlaveApprovalDal.cs` is **shared with 1 other page(s)** (e.g. `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs` is **shared with 2 other page(s)** (e.g. `PettyCashApproval.aspx`, `SampleProductrIssue.aspx`) — a change there is not local.
- Writes/reads `tblAction` — any schema change to it breaks this page.
- Writes/reads `tblActionPageWiseStep` — any schema change to it breaks this page.
- Writes/reads `tblActionStepDetail` — any schema change to it breaks this page.
- Writes/reads `tblActionSteps` — any schema change to it breaks this page.
- Writes/reads `tblActionUserWiseApproval` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustomerCreditLimit` — any schema change to it breaks this page.
- Writes/reads `tblCustomerReturnMaster` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDCStoreFreeze` — any schema change to it breaks this page.
- Writes/reads `tblDeliveryManInfo` — any schema change to it breaks this page.
- Writes/reads `tblDestroyProduct` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMainMenu` — any schema change to it breaks this page.
- Writes/reads `tblManufacturer` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblOrderDetail` — any schema change to it breaks this page.
- Writes/reads `tblPettyCashDetails` — any schema change to it breaks this page.
- Writes/reads `tblPettyCashMaster` — any schema change to it breaks this page.
- Writes/reads `tblProductDiscount` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblSampleIssue` — any schema change to it breaks this page.
- Writes/reads `tblSampleIssueDetail` — any schema change to it breaks this page.
- Writes/reads `tblSlaveInfo` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.
- Writes/reads `tblUser` — any schema change to it breaks this page.
- Writes/reads `tblWHStockInMaster` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
