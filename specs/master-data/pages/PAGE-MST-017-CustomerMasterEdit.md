# MST-017 — CustomerMasterEdit

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `MST-017` |
| Module | `master-data` |
| File name | `CustomerMasterEdit.aspx` |
| Route | `/SInventory_UI/CustomerMasterEdit.aspx` |
| `Title` directive | _not set_ |
| Master page | _none (standalone)_ |
| Code-behind | `CustomerMasterEdit.aspx.cs` |
| Code-behind class | `SInventory_UI_CustomerMasterEdit` |
| Markup lines | 371 |
| Code-behind lines | 302 |
| Server controls | 26 |
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

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/CustomerMasterEdit.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ID`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerMasterEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |

## 7. UI Control Inventory

26 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `ScriptManager1` | asp:ScriptManager | Edit |  |  |  |  |  | 16 |
| 2 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 18 |
| 3 | `custMastIdHiddenField` | asp:HiddenField | Customer Master Edit |  |  |  |  |  | 28 |
| 4 | `customerCodeTextBox` | asp:TextBox | Customer Code |  |  | ReadOnly=true |  |  | 46 |
| 5 | `isVatApplicable` | asp:CheckBox |  |  |  |  |  |  | 63 |
| 6 | `customernameTextBox` | asp:TextBox | Customer Name |  |  | ReadOnly=true |  |  | 77 |
| 7 | `addressTextBox` | asp:TextBox | Address |  |  | ReadOnly=true |  |  | 87 |
| 8 | `addressTextBox2` | asp:TextBox | Address Second |  |  | ReadOnly=true |  |  | 99 |
| 9 | `contactTextBox` | asp:TextBox | ContactNo |  |  | ReadOnly=true |  |  | 112 |
| 10 | `cityTextBox` | asp:TextBox | City |  |  | ReadOnly=true |  |  | 124 |
| 11 | `contactPersonTextBox` | asp:TextBox | Contact Person |  |  | ReadOnly=true |  |  | 139 |
| 12 | `shippingConditionTextBox` | asp:TextBox | Shipping Condition |  |  | ReadOnly=true |  |  | 154 |
| 13 | `feNameTextBox` | asp:TextBox | FE Person Name |  |  | ReadOnly=true |  |  | 169 |
| 14 | `dzsmNameTextBox` | asp:TextBox | DZSM Person Name |  |  | ReadOnly=true |  |  | 184 |
| 15 | `regionNameDropDownList` | asp:DropDownList | DZSM Name |  | True | Enabled=False |  | regionNameDropDownList_SelectedIndexChanged | 196 |
| 16 | `comUnitNameDropDownList` | asp:DropDownList | Distribution Center |  | True |  |  | comUnitNameDropDownList_SelectedIndexChanged | 208 |
| 17 | `districtNameDropDownList` | asp:DropDownList | FE Name |  | True | Enabled=False |  | districtNameDropDownList_SelectedIndexChanged | 221 |
| 18 | `areaNameDropDownList` | asp:DropDownList | Territory Name |  | True | Enabled=False |  | areaNameDropDownList_SelectedIndexChanged | 234 |
| 19 | `miaNameDropDownList` | asp:DropDownList | MIO Name |  | True | Enabled=False |  | miaNameDropDownList_SelectedIndexChanged | 246 |
| 20 | `marketNameDropDownList` | asp:DropDownList | Market Name |  | True |  |  |  | 259 |
| 21 | `categoryNameDropDownList` | asp:DropDownList | Category Name |  | True | Enabled=False |  |  | 271 |
| 22 | `paymentTypeDropDownList` | asp:DropDownList | Payment Type |  | True | Enabled=False |  |  | 283 |
| 23 | `(no id)` | asp:ListItem | Payment Type |  |  |  |  |  | 285 |
| 24 | `(no id)` | asp:ListItem |  |  |  |  |  |  | 286 |
| 25 | `updateButton` | asp:Button |  | Update |  |  |  | updateButton_Click | 309 |
| 26 | `closeButton` | asp:Button |  | Close |  |  |  | closeButton_Click | 311 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CustomerMaster`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDown` | `aCustomerMasterBLL.LoadCategoryName(...)` | `CustomerMasterBLL.LoadCategoryName(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadCategoryName` | — | — | — |
| `LoadDropDown` | `aCustomerMasterBLL.LoadRegionname(...)` | `CustomerMasterBLL.LoadRegionname(DropDownList ddl,string id)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadRegionname` → `CustomerMasterDAL.LoadRegionname` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblRegion` | — |
| `updateButton_Click` | `aCustomerMasterBLL.Customer(...)` | `CustomerMasterBLL.Customer(string customerId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.Customer` | — | — | — |
| `updateButton_Click` | `aCustomerMasterBLL.CustomerPayment(...)` | `CustomerMasterBLL.CustomerPayment(string customerId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.CustomerPayment` | — | — | — |
| `updateButton_Click` | `aCustomerMasterBLL.Customerorder(...)` | `CustomerMasterBLL.Customerorder(string customerCode)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.Customerorder` → `CustomerMasterDAL.Customerorder` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblOrder` | — |
| `updateButton_Click` | `aCustomerMasterBLL.UpdateDataForCustomerMaster(...)` | `CustomerMasterBLL.UpdateDataForCustomerMaster(CustomerMaster aCustomerMaster)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.UpdateDataForCustomerMaster` → `CustomerMasterDAL.HasCustomerMastername` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterDAL.UpdateCustomerMasterInfo` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | `tblCustMaster` | — |
| `CompanyInfoLoad` | `aCustomerMasterBLL.CustomerMasterEditLoad(...)` | `CustomerMasterBLL.CustomerMasterEditLoad(string CustomerMasterId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.CustomerMasterEditLoad` | — | — | — |
| `CompanyInfoLoad` | `aCustomerMasterBLL.LoadAreaName(...)` | `CustomerMasterBLL.LoadAreaName(DropDownList ddl,string regionId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadAreaName` → `CustomerMasterDAL.LoadAreaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `CompanyInfoLoad` | `aCustomerMasterBLL.LoadCompanyUnit(...)` | `CustomerMasterBLL.LoadCompanyUnit(DropDownList ddl,string regionId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadCompanyUnit` → `CustomerMasterDAL.LoadCompanyUnit` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit` | — |
| `CompanyInfoLoad` | `aCustomerMasterBLL.LoadDistrictName(...)` | `CustomerMasterBLL.LoadDistrictName(DropDownList ddl,string comUnitId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadDistrictName` → `CustomerMasterDAL.LoadDistrictName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblDistrict` | — |
| `CompanyInfoLoad` | `aCustomerMasterBLL.LoadMarketName(...)` | `CustomerMasterBLL.LoadMarketName(DropDownList ddl,string areaId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadMarketName` → `CustomerMasterDAL.LoadMarketName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblMarket` | — |
| `CompanyInfoLoad` | `aCustomerMasterBLL.LoadMiaName(...)` | `CustomerMasterBLL.LoadMiaName(DropDownList ddl,string areaId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadMiaName` → `CustomerMasterDAL.LoadMiaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblMIAInfo` | — |
| `regionNameDropDownList_SelectedIndexChanged` | `aCustomerMasterBLL.LoadCompanyUnit(...)` | `CustomerMasterBLL.LoadCompanyUnit(DropDownList ddl,string regionId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadCompanyUnit` → `CustomerMasterDAL.LoadCompanyUnit` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit` | — |
| `areaNameDropDownList_SelectedIndexChanged` | `aCustomerMasterBLL.LoadMiaName(...)` | `CustomerMasterBLL.LoadMiaName(DropDownList ddl,string areaId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadMiaName` → `CustomerMasterDAL.LoadMiaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblMIAInfo` | — |
| `comUnitNameDropDownList_SelectedIndexChanged` | `aCustomerMasterBLL.LoadDistrictName(...)` | `CustomerMasterBLL.LoadDistrictName(DropDownList ddl,string comUnitId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadDistrictName` → `CustomerMasterDAL.LoadDistrictName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblDistrict` | — |
| `districtNameDropDownList_SelectedIndexChanged` | `aCustomerMasterBLL.LoadAreaName(...)` | `CustomerMasterBLL.LoadAreaName(DropDownList ddl,string regionId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadAreaName` → `CustomerMasterDAL.LoadAreaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `miaNameDropDownList_SelectedIndexChanged` | `aCustomerMasterBLL.LoadMarketName(...)` | `CustomerMasterBLL.LoadMarketName(DropDownList ddl,string areaId)` <br/>`Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | `CustomerMasterBLL.LoadMarketName` → `CustomerMasterDAL.LoadMarketName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblMarket` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `regionNameDropDownList` | DZSM Name | asp:DropDownList | True | regionNameDropDownList_SelectedIndexChanged | `aCustomerMasterBLL.LoadCompanyUnit()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadCompanyUnit()` in `regionNameDropDownList_SelectedIndexChanged()`; `aCustomerMasterBLL.LoadRegionname()` in `LoadDropDown()` | `tblCompanyUnit`, `tblRegion` | set in code (see the DAL method above) |
| `comUnitNameDropDownList` | Distribution Center | asp:DropDownList | True | comUnitNameDropDownList_SelectedIndexChanged | `aCustomerMasterBLL.LoadCompanyUnit()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadCompanyUnit()` in `regionNameDropDownList_SelectedIndexChanged()`; `aCustomerMasterBLL.LoadDistrictName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadDistrictName()` in `comUnitNameDropDownList_SelectedIndexChanged()` | `tblCompanyUnit`, `tblDistrict` | set in code (see the DAL method above) |
| `districtNameDropDownList` | FE Name | asp:DropDownList | True | districtNameDropDownList_SelectedIndexChanged | `aCustomerMasterBLL.LoadAreaName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadAreaName()` in `districtNameDropDownList_SelectedIndexChanged()`; `aCustomerMasterBLL.LoadDistrictName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadDistrictName()` in `comUnitNameDropDownList_SelectedIndexChanged()` | `tblArea`, `tblDistrict` | set in code (see the DAL method above) |
| `areaNameDropDownList` | Territory Name | asp:DropDownList | True | areaNameDropDownList_SelectedIndexChanged | `aCustomerMasterBLL.LoadAreaName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadAreaName()` in `districtNameDropDownList_SelectedIndexChanged()`; `aCustomerMasterBLL.LoadMiaName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadMiaName()` in `areaNameDropDownList_SelectedIndexChanged()` | `tblArea`, `tblMIAInfo` | set in code (see the DAL method above) |
| `miaNameDropDownList` | MIO Name | asp:DropDownList | True | miaNameDropDownList_SelectedIndexChanged | `aCustomerMasterBLL.LoadMarketName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadMarketName()` in `miaNameDropDownList_SelectedIndexChanged()`; `aCustomerMasterBLL.LoadMiaName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadMiaName()` in `areaNameDropDownList_SelectedIndexChanged()` | `tblMIAInfo`, `tblMarket` | set in code (see the DAL method above) |
| `marketNameDropDownList` | Market Name | asp:DropDownList | True | — | `aCustomerMasterBLL.LoadMarketName()` in `CompanyInfoLoad()`; `aCustomerMasterBLL.LoadMarketName()` in `miaNameDropDownList_SelectedIndexChanged()` | `tblMarket` | set in code (see the DAL method above) |
| `categoryNameDropDownList` | Category Name | asp:DropDownList | True | — | `aCustomerMasterBLL.LoadCategoryName()` in `LoadDropDown()` | UNKNOWN | set in code (see the DAL method above) |
| `paymentTypeDropDownList` | Payment Type | asp:DropDownList | True | — | UNKNOWN — no code-behind call names this control; it may be filled from markup, from a shared helper, or left empty | UNKNOWN | set in code (see the DAL method above) |

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
| `updateButton` | asp:Button | Update | — | `updateButton_Click` | 70 lines | `aCustomerMasterBLL.Customer`, `aCustomerMasterBLL.CustomerPayment`, `aCustomerMasterBLL.Customerorder`, `aCustomerMasterBLL.UpdateDataForCustomerMaster` | — |
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
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 8 | — | — | — | — |
| 2 | `LoadDropDown` | `void LoadDropDown()` | 5 | `aCustomerMasterBLL.LoadCategoryName`, `aCustomerMasterBLL.LoadRegionname` | — | — | — |
| 3 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 4 | `Validation` | `bool Validation()` | 119 | — | — | — | — |
| 5 | `updateButton_Click` | `void updateButton_Click(object sender, EventArgs e)` | 70 | `aCustomerMasterBLL.Customer`, `aCustomerMasterBLL.CustomerPayment`, `aCustomerMasterBLL.Customerorder`, `aCustomerMasterBLL.UpdateDataForCustomerMaster` | — | — | — |
| 6 | `CompanyInfoLoad` | `void CompanyInfoLoad(string custMastId)` | 39 | `aCustomerMasterBLL.CustomerMasterEditLoad`, `aCustomerMasterBLL.LoadAreaName`, `aCustomerMasterBLL.LoadCompanyUnit`, `aCustomerMasterBLL.LoadDistrictName`, `aCustomerMasterBLL.LoadMarketName`, `aCustomerMasterBLL.LoadMiaName` | — | — | — |
| 7 | `closeButton_Click` | `void closeButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 8 | `regionNameDropDownList_SelectedIndexChanged` | `void regionNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | `aCustomerMasterBLL.LoadCompanyUnit` | — | — | — |
| 9 | `areaNameDropDownList_SelectedIndexChanged` | `void areaNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | `aCustomerMasterBLL.LoadMiaName` | — | — | — |
| 10 | `comUnitNameDropDownList_SelectedIndexChanged` | `void comUnitNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | `aCustomerMasterBLL.LoadDistrictName` | — | — | — |
| 11 | `districtNameDropDownList_SelectedIndexChanged` | `void districtNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | `aCustomerMasterBLL.LoadAreaName` | — | — | — |
| 12 | `miaNameDropDownList_SelectedIndexChanged` | `void miaNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | `aCustomerMasterBLL.LoadMarketName` | — | — | — |

Call chain shape for every persisting action on this page:

```
CustomerMasterEdit.aspx (markup)
  -> SInventory_UI_CustomerMasterEdit (code-behind event handler)
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
| `LoadDropDown` | `CustomerMasterBLL` | `LoadCategoryName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `LoadDropDown` | `CustomerMasterBLL` | `LoadRegionname` | `DropDownList ddl,string id` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `Customer` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `CustomerPayment` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `Customerorder` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `updateButton_Click` | `CustomerMasterBLL` | `UpdateDataForCustomerMaster` | `CustomerMaster aCustomerMaster` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterBLL` | `CustomerMasterEditLoad` | `string CustomerMasterId` | `CustomerMaster` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterBLL` | `LoadAreaName` | `DropDownList ddl,string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterBLL` | `LoadCompanyUnit` | `DropDownList ddl,string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterBLL` | `LoadDistrictName` | `DropDownList ddl,string comUnitId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterBLL` | `LoadMarketName` | `DropDownList ddl,string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `CompanyInfoLoad` | `CustomerMasterBLL` | `LoadMiaName` | `DropDownList ddl,string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `regionNameDropDownList_SelectedIndexChanged` | `CustomerMasterBLL` | `LoadCompanyUnit` | `DropDownList ddl,string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `areaNameDropDownList_SelectedIndexChanged` | `CustomerMasterBLL` | `LoadMiaName` | `DropDownList ddl,string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `comUnitNameDropDownList_SelectedIndexChanged` | `CustomerMasterBLL` | `LoadDistrictName` | `DropDownList ddl,string comUnitId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `districtNameDropDownList_SelectedIndexChanged` | `CustomerMasterBLL` | `LoadAreaName` | `DropDownList ddl,string regionId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| `miaNameDropDownList_SelectedIndexChanged` | `CustomerMasterBLL` | `LoadMarketName` | `DropDownList ddl,string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblArea` | `CompanyInfoLoad()` → `CustomerMasterBLL.LoadAreaName` → `CustomerMasterDAL.LoadAreaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `districtNameDropDownList_SelectedIndexChanged()` → `CustomerMasterBLL.LoadAreaName` → `CustomerMasterDAL.LoadAreaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyUnit` | `CompanyInfoLoad()` → `CustomerMasterBLL.LoadCompanyUnit` → `CustomerMasterDAL.LoadCompanyUnit` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `regionNameDropDownList_SelectedIndexChanged()` → `CustomerMasterBLL.LoadCompanyUnit` → `CustomerMasterDAL.LoadCompanyUnit` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCustMaster` | `updateButton_Click()` → `CustomerMasterBLL.UpdateDataForCustomerMaster` → `CustomerMasterDAL.HasCustomerMastername` → `ClsCommonInternalDAL.DataContainerDataReader` → `CustomerMasterDAL.UpdateCustomerMasterInfo` |
| `tblDistrict` | `CompanyInfoLoad()` → `CustomerMasterBLL.LoadDistrictName` → `CustomerMasterDAL.LoadDistrictName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `comUnitNameDropDownList_SelectedIndexChanged()` → `CustomerMasterBLL.LoadDistrictName` → `CustomerMasterDAL.LoadDistrictName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblMIAInfo` | `CompanyInfoLoad()` → `CustomerMasterBLL.LoadMiaName` → `CustomerMasterDAL.LoadMiaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `areaNameDropDownList_SelectedIndexChanged()` → `CustomerMasterBLL.LoadMiaName` → `CustomerMasterDAL.LoadMiaName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblMarket` | `CompanyInfoLoad()` → `CustomerMasterBLL.LoadMarketName` → `CustomerMasterDAL.LoadMarketName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `miaNameDropDownList_SelectedIndexChanged()` → `CustomerMasterBLL.LoadMarketName` → `CustomerMasterDAL.LoadMarketName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblOrder` | `updateButton_Click()` → `CustomerMasterBLL.Customerorder` → `CustomerMasterDAL.Customerorder` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblRegion` | `LoadDropDown()` → `CustomerMasterBLL.LoadRegionname` → `CustomerMasterDAL.LoadRegionname` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `CustomerMasterDAL.LoadRegionName` |

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
| `BR-MST-017-001` | {0} | Code | `Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

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
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Database object | `tblArea` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblDistrict` |
| Database object | `tblMIAInfo` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblRegion` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-MST-017-001` | Unauthenticated access | Request `/SInventory_UI/CustomerMasterEdit.aspx` with no session | Page renders — **no guard**; this is a defect, see GAP-SEC-002 | CONFIRMED |
| `TC-MST-017-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CustomerMasterEdit.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CustomerMasterEdit.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 26 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 12 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 8 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` is **shared with 10 other page(s)** (e.g. `CustomerMasterViewer.aspx`, `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `CustMasterView.aspx`, `CustomerMasterReport.aspx`) — a change there is not local.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblDistrict` — any schema change to it breaks this page.
- Writes/reads `tblMIAInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
