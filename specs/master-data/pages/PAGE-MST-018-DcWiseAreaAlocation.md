# MST-018 — DcWiseAreaAlocation

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-018` |
| Module | `master-data` |
| File name | `DcWiseAreaAlocation.aspx` |
| Route | `/SInventory_UI/DcWiseAreaAlocation.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `DcWiseAreaAlocation.aspx.cs` |
| Code-behind class | `SInventory_UI_DcWiseAreaAlocation` |
| Markup lines | 232 |
| Code-behind lines | 226 |
| Server controls | 10 |
| GridViews | 0 |
| Code-behind methods | 12 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/DcWiseAreaAlocation.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DcWiseAreaAlocation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DcWiseAreaAlocation.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WingsSalesAndCollectionReportDal.cs` |

## 7. UI Control Inventory

10 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 15 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 17 |
| 4 | `companyDropDownList` | asp:DropDownList | Company : |  | True |  |  | companyDropDownList_SelectedIndexChanged | 65 |
| 5 | `distributionDropDownList` | asp:DropDownList | Distribution : |  | True |  |  | distributionDropDownList_SelectedIndexChanged | 86 |
| 6 | `areaCheckBoxList` | asp:CheckBoxList | Area Name: |  |  |  |  |  | 104 |
| 7 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click1 | 175 |
| 8 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 177 |
| 9 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 221 |
| 10 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 224 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `DcWiseAreaAloDao`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropdownList` | `aDcWiseBll.LoadCompanyDropdownList(...)` | `ASMInfoBll.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ASMInfoBll.cs` | `ASMInfoBll.LoadCompanyDropdownList` → `ASMInfoDal.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `CreditLimitDAL.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustPaymentDAL.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `DeliveryPersonDAL.GetCompanyDropdownList` | SELECT | `tblCompanyInfo` | — |
| `LoadArea` | `aDcWiseBll.LoadAreaView(...)` | `AreaDAL.LoadAreaView()` <br/>`Library.DAL/SInventory_DAL/AreaDAL.cs` | `AreaDAL.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` → `DcWiseAreaAloDal.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` → `TerritoryInfoDal.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` → `DcWiseAreaAloBll.LoadAreaView` | SELECT | `tblArea`, `tblRegion`, `tblTerritory` | — |
| `submitButton_Click1` | `aDcWiseBll.LoadDcWiseAreaAloInfo(...)` | `DcWiseAreaAloBll.LoadDcWiseAreaAloInfo(int DCId)` <br/>`Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` | `DcWiseAreaAloBll.LoadDcWiseAreaAloInfo` → `DcWiseAreaAloDal.GetDcWiseAreaAloInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDcWiseAreaInfo` | — |
| `submitButton_Click1` | `aDcWiseBll.SaveDcWiseAreaInfo(...)` | `DcWiseAreaAloBll.SaveDcWiseAreaInfo(List<DcWiseAreaAloDao> areaListForSave)` <br/>`Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` | `DcWiseAreaAloBll.SaveDcWiseAreaInfo` → `DcWiseAreaAloDal.SaveDcWiseAreaAlo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | — | `tblDcWiseAreaInfo` | — |
| `submitButton_Click1` | `aDcWiseBll.UpdateDcWiseAreaInfo(...)` | `DcWiseAreaAloBll.UpdateDcWiseAreaInfo(List<DcWiseAreaAloDao> areaListForUpdate)` <br/>`Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` | `DcWiseAreaAloBll.UpdateDcWiseAreaInfo` → `DcWiseAreaAloDal.UpdateDcWiseAreaAlo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDcWiseAreaInfo` | — |
| `companyDropDownList_SelectedIndexChanged` | `aDcWiseBll.LoadDC(...)` | `CustPaymentDAL.LoadDC(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | `CustPaymentDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DcWiseAreaAloDal.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DeliveryInvoiceReportDal.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `InvoiceDAL.LoadDC` → `InvoiceReportDal.LoadDC` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `distributionDropDownList_SelectedIndexChanged` | `aDcWiseBll.LoadDcWiseAreaAloByDc(...)` | `CustPaymentDAL.LoadDcWiseAreaAloByDc(DropDownList ddl,string DCId)` <br/>`Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | `CustPaymentDAL.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DcWiseAreaAloDal.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentBLL.LoadDcWiseAreaAloByDc` → `CustPaymentDAL.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DcWiseAreaAloBll.LoadDcWiseAreaAloByDc` | SELECT | `tblArea`, `tblDcWiseAreaInfo` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyDropDownList` | Company : | asp:DropDownList | True | companyDropDownList_SelectedIndexChanged | `aDcWiseBll.LoadCompanyDropdownList()` in `LoadDropdownList()`; `aDcWiseBll.LoadDC()` in `companyDropDownList_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `distributionDropDownList` | Distribution : | asp:DropDownList | True | distributionDropDownList_SelectedIndexChanged | `aDcWiseBll.LoadDC()` in `companyDropDownList_SelectedIndexChanged()`; `aDcWiseBll.LoadDcWiseAreaAloByDc()` in `distributionDropDownList_SelectedIndexChanged()`; `aDcWiseBll.LoadDcWiseAreaAloInfo()` in `submitButton_Click1()` | `tblArea`, `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `areaCheckBoxList` | Area Name: | asp:CheckBoxList | False | — | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |

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
| `submitButton` | asp:Button | Submit | — | `submitButton_Click1` | 66 lines | `aDcWiseBll.LoadDcWiseAreaAloInfo`, `aDcWiseBll.SaveDcWiseAreaInfo`, `aDcWiseBll.UpdateDcWiseAreaInfo` | — |
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
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `LoadDropdownList` | `void LoadDropdownList()` | 3 | `aDcWiseBll.LoadCompanyDropdownList` | — | — | — |
| 3 | `LoadArea` | `void LoadArea()` | 8 | `aDcWiseBll.LoadAreaView` | — | — | — |
| 4 | `Clear` | `void Clear()` | 5 | — | — | — | — |
| 5 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 6 | `Validation` | `bool Validation()` | 30 | — | — | — | — |
| 7 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 66 | `aDcWiseBll.LoadDcWiseAreaAloInfo`, `aDcWiseBll.SaveDcWiseAreaInfo`, `aDcWiseBll.UpdateDcWiseAreaInfo` | — | — | `LoginName` |
| 8 | `AllocatedOrNot` | `bool AllocatedOrNot(string value, DataTable aDataTable)` | 17 | — | — | — | — |
| 9 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 10 | `companyDropDownList_SelectedIndexChanged` | `void companyDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | `aDcWiseBll.LoadDC` | — | — | — |
| 11 | `distributionDropDownList_SelectedIndexChanged` | `void distributionDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 23 | `aDcWiseBll.LoadDcWiseAreaAloByDc` | — | — | — |
| 12 | `UncheckAllCheckBox` | `void UncheckAllCheckBox()` | 7 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
DcWiseAreaAlocation.aspx (markup)
  -> SInventory_UI_DcWiseAreaAlocation (code-behind event handler)
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
| `LoadDropdownList` | `ASMInfoBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| `LoadDropdownList` | `CustPaymentBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `LoadDropdownList` | `DcWiseAreaAloBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| `LoadDropdownList` | `DeliveryPersonBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| `LoadDropdownList` | `DistributionCeneterInfoBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| `LoadDropdownList` | `EmpGeneralInfoBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| `LoadDropdownList` | `MIOSetupBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| `LoadDropdownList` | `OtherStockActionBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `LoadDropdownList` | `ProductBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `LoadDropdownList` | `RequisitionBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `LoadDropdownList` | `RSMSetupBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| `LoadDropdownList` | `SalesAdministrationReportBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| `LoadDropdownList` | `SlaveinformationBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| `LoadDropdownList` | `StockConditionFreezeBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `LoadDropdownList` | `WarehouseStockInBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| `LoadDropdownList` | `WhFreezeStockReleaseBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| `LoadDropdownList` | `WhStockConditionFreezeBll` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| `LoadDropdownList` | `WingsSalesAndCollectionReportBll` | `LoadCompanyDropdownList` | `DropDownList ddl, string groupId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| `LoadArea` | `DcWiseAreaAloBll` | `LoadAreaView` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| `submitButton_Click1` | `DcWiseAreaAloBll` | `LoadDcWiseAreaAloInfo` | `int DCId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| `submitButton_Click1` | `DcWiseAreaAloBll` | `SaveDcWiseAreaInfo` | `List<DcWiseAreaAloDao> areaListForSave` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| `submitButton_Click1` | `DcWiseAreaAloBll` | `UpdateDcWiseAreaInfo` | `List<DcWiseAreaAloDao> areaListForUpdate` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| `companyDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `companyDropDownList_SelectedIndexChanged` | `DcWiseAreaAloBll` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| `companyDropDownList_SelectedIndexChanged` | `InvoiceBLL` | `LoadDC` | `DropDownList aDropDownList, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `companyDropDownList_SelectedIndexChanged` | `OrderInfoBLL` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| `distributionDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDcWiseAreaAloByDc` | `DropDownList ddl,string DcWiseAreaID` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `distributionDropDownList_SelectedIndexChanged` | `DcWiseAreaAloBll` | `LoadDcWiseAreaAloByDc` | `string DcWiseAreaID` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadArea` | `AreaDAL` | `LoadAreaView` | `` | `DataTable` | SELECT | `tblArea`, `tblRegion` | — | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| `LoadArea` | `DcWiseAreaAloDal` | `LoadAreaView` | `` | `DataTable` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs` |
| `LoadArea` | `TerritoryInfoDal` | `LoadAreaView` | `` | `DataTable` | SELECT | `tblArea`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| `companyDropDownList_SelectedIndexChanged` | `CustPaymentDAL` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `companyDropDownList_SelectedIndexChanged` | `DcWiseAreaAloDal` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs` |
| `companyDropDownList_SelectedIndexChanged` | `DeliveryInvoiceReportDal` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| `companyDropDownList_SelectedIndexChanged` | `InvoiceDAL` | `LoadDC` | `DropDownList aDropDownList,string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| `companyDropDownList_SelectedIndexChanged` | `InvoiceReportDal` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| `companyDropDownList_SelectedIndexChanged` | `OrderInfoDAL` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| `companyDropDownList_SelectedIndexChanged` | `SampleInvoiceDal` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |
| `companyDropDownList_SelectedIndexChanged` | `WingsSalesAndCollectionReportDal` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsSalesAndCollectionReportDal.cs` |
| `distributionDropDownList_SelectedIndexChanged` | `CustPaymentDAL` | `LoadDcWiseAreaAloByDc` | `DropDownList ddl,string DCId` | `void` | SELECT | `tblArea`, `tblDcWiseAreaInfo` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `distributionDropDownList_SelectedIndexChanged` | `DcWiseAreaAloDal` | `LoadDcWiseAreaAloByDc` | `string DCId` | `DataTable` | SELECT | `tblDcWiseAreaInfo` | — | `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `LoadArea()` → `AreaDAL.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` → `DcWiseAreaAloDal.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable`, `distributionDropDownList_SelectedIndexChanged()` → `CustPaymentDAL.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DcWiseAreaAloDal.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `LoadDropdownList()` → `ASMInfoBll.LoadCompanyDropdownList` → `ASMInfoDal.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` → `CreditLimitDAL.GetCompanyDropdownList` |
| `tblCompanyUnit` | `companyDropDownList_SelectedIndexChanged()` → `CustPaymentDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DcWiseAreaAloDal.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblDcWiseAreaInfo` | `distributionDropDownList_SelectedIndexChanged()` → `CustPaymentDAL.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DcWiseAreaAloDal.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `DcWiseAreaAloBll.LoadDcWiseAreaAloInfo` → `DcWiseAreaAloDal.GetDcWiseAreaAloInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `DcWiseAreaAloBll.SaveDcWiseAreaInfo` → `DcWiseAreaAloDal.SaveDcWiseAreaAlo` → `ClsCommonInternalDAL.SaveDataByInsertCommand`, `submitButton_Click1()` → `DcWiseAreaAloBll.UpdateDcWiseAreaInfo` → `DcWiseAreaAloDal.UpdateDcWiseAreaAlo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblRegion` | `LoadArea()` → `AreaDAL.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` → `DcWiseAreaAloDal.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblTerritory` | `LoadArea()` → `AreaDAL.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` → `DcWiseAreaAloDal.LoadAreaView` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUserCompanyUnit` | `companyDropDownList_SelectedIndexChanged()` → `CustPaymentDAL.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DcWiseAreaAloDal.LoadDC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |

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
| `BR-MST-018-001` | {0} | Code | `Solution.Web/SInventory_UI/DcWiseAreaAlocation.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WingsSalesAndCollectionReportDal.cs` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblRegion` |
| Database object | `tblTerritory` |
| Database object | `tblUserCompanyUnit` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-018-001` | Unauthenticated access | Request `/SInventory_UI/DcWiseAreaAlocation.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-MST-018-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DcWiseAreaAlocation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DcWiseAreaAlocation.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` |
| Layer source | `Library.DAL/SInventory_DAL/AreaDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WingsSalesAndCollectionReportDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 10 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 12 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 7 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/ASMInfoBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` is **shared with 5 other page(s)** (e.g. `DeliveryPersonEntry.aspx`, `DeliveryPersonView.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` is **shared with 6 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` is **shared with 4 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGenInfView.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` is **shared with 43 other page(s)** (e.g. `ProformaHTMLReport.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseDelReturnViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/MIOSetupBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` is **shared with 32 other page(s)** (e.g. `HomePage.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `BatchWiseDelReturnSummery.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` is **shared with 22 other page(s)** (e.g. `AllSalesReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`, `BatchWiseTopSheet.aspx`, `CustomerPayment.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ProductBLL.cs` is **shared with 8 other page(s)** (e.g. `CreatePickingDetailForWareHouse.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `IssueRequisitionProducts.aspx`, `ProductEdit.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RSMSetupBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RequisitionBLL.cs` is **shared with 29 other page(s)** (e.g. `Dashboard.aspx`, `B2BReceiveViewer.aspx`, `StockReceiveReportViewer.aspx`, `TransferStockReceiveReportViewer.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` is **shared with 16 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `CustomerPaymentReport.aspx`, `CustomerReturnByMIOReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`, `DistributionCenterInformation.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` is **shared with 9 other page(s)** (e.g. `AllSalesReport.aspx`, `DeliveryReturnReport.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `SCPickingGenerate.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` is **shared with 11 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `MaterialConversionReceive.aspx`, `MaterialConversionReceiveView.aspx`, `NonTranscationalInvoiceReturn.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` is **shared with 3 other page(s)** (e.g. `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `WhFreezeStockRelease.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/AreaDAL.cs` is **shared with 3 other page(s)** (e.g. `CustomerReport.aspx`, `FieldForceSalesSummery.aspx`, `WeaightedAVG_Report.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` is **shared with 2 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs` is **shared with 9 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CreditCustomerReport.aspx`, `MaterialConversionReceive.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` is **shared with 2 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` is **shared with 7 other page(s)** (e.g. `HomePage.aspx`, `AuditReportOne.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` is **shared with 2 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` is **shared with 2 other page(s)** (e.g. `CustomerReport.aspx`, `FieldForceSalesSummery.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
