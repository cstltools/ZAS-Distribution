# PRC-015 — ProductQuotedPriceView

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `PRC-015` |
| Module | `pricing-discount-campaign` |
| File name | `ProductQuotedPriceView.aspx` |
| Route | `/SInventory_UI/ProductQuotedPriceView.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ProductQuotedPriceView.aspx.cs` |
| Code-behind class | `SInventory_UI_ProductDiscountView` |
| Markup lines | 276 |
| Code-behind lines | 161 |
| Server controls | 22 |
| GridViews | 1 |
| Code-behind methods | 12 |

## 2. Purpose

List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: List / browse screen. It lists existing records with search and filter controls and links to the corresponding entry or edit screen. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/ProductQuotedPriceSetup.aspx`

Outbound `Response.Redirect` targets: `ProductQuotedPriceSetup.aspx`

## 5. Route

`/SInventory_UI/ProductQuotedPriceView.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductQuotedPriceView.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductQuotedPriceView.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ASMInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DeliveryPersonDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MIOSetupDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/RSMSetupDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/RequisitionDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SalesAdministrationReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SlaveInformationDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WHBeenCardReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WhStockConditionFreezeDal.cs` |

## 7. UI Control Inventory

22 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 85 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 89 |
| 4 | `productInfoNewImageButton` | asp:ImageButton | Add New | ~/images/Add.png |  |  |  | miaTargetNewImageButton_Click | 109 |
| 5 | `productInfoReloadImageButton` | asp:ImageButton | Reload | ~/images/refresh.png |  |  |  | miaTargetReloadImageButton_Click | 116 |
| 6 | `CompanyDropDown` | asp:DropDownList | Company Name : |  | True |  |  | CompanyDropDown_OnSelectedIndexChanged | 142 |
| 7 | `custCodeTextBox` | asp:TextBox | Customer Code: |  | True |  |  | custCodeTextBox_TextChanged | 158 |
| 8 | `customerTextBox_AutoCompleteExtender` | ajaxToolkit:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 160 |
| 9 | `CustomerIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 167 |
| 10 | `loadGridView` | asp:GridView |  |  |  |  |  | loadGridView_RowCommand | 187 |
| 11 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 191 |
| 12 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 192 |
| 13 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 193 |
| 14 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 194 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 195 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 196 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 197 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 198 |
| 19 | `(no id)` | asp:TemplateField |  |  |  |  | False |  | 199 |
| 20 | `editImageButton` | asp:ImageButton | loadGridView → col[8] "Edit" | ~/images/edit.png |  |  |  |  | 201 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 206 |
| 22 | `ImageButton2` | asp:ImageButton | loadGridView → col[9] "Delete" | ~/images/lineDelete.png |  |  |  |  | 208 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `QuotedPriceId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Company Name | CompanyName | — | — | — |
| 1 | BoundField | Customer Code | CustomerCode | — | — | — |
| 2 | BoundField | Customer Name | CustomerName | — | — | — |
| 3 | BoundField | Product Name | ProductName | — | — | — |
| 4 | BoundField | Quoted Price | QuotedPrice | — | — | — |
| 5 | BoundField | Active Date | ActiveDate | — | — | — |
| 6 | BoundField | Inactive Date | InactiveDate | — | — | — |
| 7 | BoundField | Status | ActionStatus | — | — | — |
| 8 | TemplateField | Edit | — | — | `ImageButton:editImageButton` | — |
| 9 | TemplateField | Delete | — | — | `ImageButton:ImageButton2` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoaddropdownList` | `aCashBll.GetCompanyDropdownList(...)` | `ASMInfoDal.GetCompanyDropdownList(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/ASMInfoDal.cs` | `ASMInfoDal.GetCompanyDropdownList` → `CreditLimitDAL.GetCompanyDropdownList` → `CustPaymentDAL.GetCompanyDropdownList` → `DeliveryPersonDAL.GetCompanyDropdownList` → `DistributionCenterInfoDal.GetCompanyDropdownList` → `EmpGeneralInfoDAL.GetCompanyDropdownList` → `MIOSetupDal.GetCompanyDropdownList` → `OtherStockActionDAL.GetCompanyDropdownList` | SELECT | `tblCompanyInfo` | — |
| `ProductdiscountLoad` | `aProductDiscountBll.ProductQuotedPriceView(...)` | `ProductDiscountBLL.ProductQuotedPriceView(string pram)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.ProductQuotedPriceView` → `ProductDiscountDAL.ProductQuotedprice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCustMaster`, `tblProduct`, `tblProductQuotedPrice` | — |
| `loadGridView_RowCommand` | `aProductDiscountBll.DeleteProductQuotedprice(...)` | `ProductDiscountBLL.DeleteProductQuotedprice(string id)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.DeleteProductQuotedprice` → `ProductDiscountDAL.DeleteProductQuotedprice` → `ClsCommonInternalDAL.DeleteAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.DeleteAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblProductQuotedPrice` | `sp_DEL_ProductDiscount` |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `CompanyDropDown` | Company Name : | asp:DropDownList | True | CompanyDropDown_OnSelectedIndexChanged | `aCashBll.GetCompanyDropdownList()` in `LoaddropdownList()` | `tblCompanyInfo` | set in code (see the DAL method above) |

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
| `productInfoNewImageButton` | asp:ImageButton |  | — | `miaTargetNewImageButton_Click` | 3 lines | — | — |
| `productInfoReloadImageButton` | asp:ImageButton |  | — | `miaTargetReloadImageButton_Click` | 3 lines | — | — |
| `editImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton2` | asp:ImageButton |  | — | `—` | — | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:AutoCompleteExtender`(custCodeTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `LoaddropdownList` | `void LoaddropdownList()` | 6 | `aCashBll.GetCompanyDropdownList` | — | — | — |
| 3 | `custCodeTextBox_TextChanged` | `void custCodeTextBox_TextChanged(object sender, EventArgs e)` | 26 | — | — | — | — |
| 4 | `GenerateParam` | `String GenerateParam()` | 17 | — | — | — | — |
| 5 | `ProductdiscountLoad` | `void ProductdiscountLoad()` | 25 | `aProductDiscountBll.ProductQuotedPriceView` | — | — | — |
| 6 | `PopUp` | `void PopUp(string Id)` | 5 | — | — | — | — |
| 7 | `loadGridView_RowCommand` | `void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 21 | `aProductDiscountBll.DeleteProductQuotedprice` | — | — | `DiscountId` |
| 8 | `miaTargetReloadImageButton_Click` | `void miaTargetReloadImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 9 | `miaTargetNewImageButton_Click` | `void miaTargetNewImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 10 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 11 | `gotoinvoiceButton_Click` | `void gotoinvoiceButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 12 | `CompanyDropDown_OnSelectedIndexChanged` | `void CompanyDropDown_OnSelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ProductQuotedPriceView.aspx (markup)
  -> SInventory_UI_ProductDiscountView (code-behind event handler)
     -> Library.BLL.SInventory_BLL.*BLL          (thin pass-through + calculations)
        -> Library.DAL.SInventory_DAL.*DAL       (builds SQL string / SqlParameter list)
           -> Library.DAL.InternalCls.ClsCommonInternalDAL
              -> EXEC ExecuteAllSqlQueryByStoreProcedure @Query   (for inline SQL)
              -> EXEC <named stored procedure>                    (for parameterised calls)
                 -> SQL Server: SalesrollDB_ZAS
```

## 13. API Mapping

AutoComplete extenders bound to the SOAP web service `Solution.Web/SInventory_UI/SInventoryWebService.asmx` (implementation `Solution.Web/App_Code/SInventoryWebService.cs`):

| Extender | TargetControlID | ServiceMethod | ServicePath | MinimumPrefixLength |
|---|---|---|---|---|
| `customerTextBox_AutoCompleteExtender` | `custCodeTextBox` | `GetCustomernew` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `ProductdiscountLoad` | `ProductDiscountBLL` | `ProductQuotedPriceView` | `string pram` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `loadGridView_RowCommand` | `ProductDiscountBLL` | `DeleteProductQuotedprice` | `string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoaddropdownList` | `ASMInfoDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/ASMInfoDal.cs` |
| `LoaddropdownList` | `CreditLimitDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| `LoaddropdownList` | `CustPaymentDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `LoaddropdownList` | `DeliveryPersonDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DeliveryPersonDAL.cs` |
| `LoaddropdownList` | `DistributionCenterInfoDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| `LoaddropdownList` | `EmpGeneralInfoDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs` |
| `LoaddropdownList` | `MIOSetupDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MIOSetupDal.cs` |
| `LoaddropdownList` | `OtherStockActionDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs` |
| `LoaddropdownList` | `ProductDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| `LoaddropdownList` | `RequisitionDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/RequisitionDAL.cs` |
| `LoaddropdownList` | `RSMSetupDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/RSMSetupDal.cs` |
| `LoaddropdownList` | `SalesAdministrationReportDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/SalesAdministrationReportDal.cs` |
| `LoaddropdownList` | `SlaveInformationDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/SlaveInformationDal.cs` |
| `LoaddropdownList` | `StockConditionFreezeDAL` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs` |
| `LoaddropdownList` | `WarehouseStockInDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs` |
| `LoaddropdownList` | `WHBeenCardReportDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WHBeenCardReportDal.cs` |
| `LoaddropdownList` | `WhFreezeStockReleaseDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs` |
| `LoaddropdownList` | `WhStockConditionFreezeDal` | `GetCompanyDropdownList` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WhStockConditionFreezeDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_DEL_ProductDiscount` | loadGridView_RowCommand → `ProductDiscountBLL.DeleteProductQuotedprice → ProductDiscountDAL.DeleteProductQuotedprice → ClsCommonInternalDAL.DeleteAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.DeleteAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoaddropdownList()` → `ASMInfoDal.GetCompanyDropdownList` → `CreditLimitDAL.GetCompanyDropdownList` → `CustPaymentDAL.GetCompanyDropdownList` → `DeliveryPersonDAL.GetCompanyDropdownList`, `ProductdiscountLoad()` → `ProductDiscountBLL.ProductQuotedPriceView` → `ProductDiscountDAL.ProductQuotedprice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `ProductdiscountLoad()` → `ProductDiscountBLL.ProductQuotedPriceView` → `ProductDiscountDAL.ProductQuotedprice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProduct` | `ProductdiscountLoad()` → `ProductDiscountBLL.ProductQuotedPriceView` → `ProductDiscountDAL.ProductQuotedprice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductQuotedPrice` | `ProductdiscountLoad()` → `ProductDiscountBLL.ProductQuotedPriceView` → `ProductDiscountDAL.ProductQuotedprice` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `ProductDiscountBLL.DeleteProductQuotedprice` → `ProductDiscountDAL.DeleteProductQuotedprice` → `ClsCommonInternalDAL.DeleteAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` |

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
| `BR-PRC-015-001` | {0} | Code | `Solution.Web/SInventory_UI/ProductQuotedPriceView.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `DiscountId`

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
| Assembly source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ASMInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DeliveryPersonDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MIOSetupDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/RSMSetupDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/RequisitionDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SalesAdministrationReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SlaveInformationDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WHBeenCardReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WhStockConditionFreezeDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCustMaster` |
| Database object | `tblProduct` |
| Database object | `tblProductQuotedPrice` |
| Stored procedure | `sp_DEL_ProductDiscount` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-PRC-015-001` | Unauthenticated access | Request `/SInventory_UI/ProductQuotedPriceView.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-PRC-015-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-PRC-015-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductQuotedPriceView.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductQuotedPriceView.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ASMInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DeliveryPersonDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MIOSetupDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProductDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/RSMSetupDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/RequisitionDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SalesAdministrationReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SlaveInformationDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WHBeenCardReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WhStockConditionFreezeDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 22 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 12 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
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

- `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` is **shared with 14 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DiscountApproval.aspx`, `DistributionCenterInfoView.aspx`, `ManualOrderrCreation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ASMInfoDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` is **shared with 6 other page(s)** (e.g. `CustomerCreditLimitApproval.aspx`, `CustomerCreditLimitEntry.aspx`, `CustomerCreditView.aspx`, `CustomerPaymentProcess.aspx`, `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DeliveryPersonDAL.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` is **shared with 7 other page(s)** (e.g. `AuditReportOne.aspx`, `CustomerCreditView.aspx`, `CustomerReport.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOSetupDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProductDAL.cs` is **shared with 3 other page(s)** (e.g. `CustMasterEdit.aspx`, `CustMasterEntry.aspx`, `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/RSMSetupDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/RequisitionDAL.cs` is **shared with 8 other page(s)** (e.g. `BatchWiseTransitReport.aspx`, `CustomerCreditView.aspx`, `CustomerPaymentReport.aspx`, `CustomerReturnByMIOReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesAdministrationReportDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SlaveInformationDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs` is **shared with 8 other page(s)** (e.g. `BatchWiseTransitReport.aspx`, `CustomerCreditView.aspx`, `CustomerPaymentReport.aspx`, `CustomerReturnByMIOReport.aspx`, `CustomerWiseProductSalesReportByMio.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WHBeenCardReportDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs` is **shared with 1 other page(s)** (e.g. `CustomerCreditView.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WhStockConditionFreezeDal.cs` is **shared with 3 other page(s)** (e.g. `NationalStockReportHTMLReport.aspx`, `NationalStockReportViewer.aspx`, `CustomerCreditView.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductQuotedPrice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
