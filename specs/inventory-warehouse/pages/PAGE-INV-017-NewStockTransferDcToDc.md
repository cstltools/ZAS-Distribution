# INV-017 — NewStockTransferDcToDc

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-017` |
| Module | `inventory-warehouse` |
| File name | `NewStockTransferDcToDc.aspx` |
| Route | `/SInventory_UI/NewStockTransferDcToDc.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `NewStockTransferDcToDc.aspx.cs` |
| Code-behind class | `SInventory_UI_NewStockTransferDcToDc` |
| Markup lines | 553 |
| Code-behind lines | 711 |
| Server controls | 49 |
| GridViews | 2 |
| Code-behind methods | 21 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 2 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 2 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

Outbound `Response.Redirect` targets: `Login.aspx`

## 5. Route

`/SInventory_UI/NewStockTransferDcToDc.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/NewStockTransferDcToDc.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/NewStockTransferDcToDc.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |

## 7. UI Control Inventory

49 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 49 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 50 |
| 4 | `chalanDateTextBox` | asp:TextBox | Chalan Date |  |  | ReadOnly=True |  |  | 103 |
| 5 | `chalanNoTextBox` | asp:TextBox | ChalanNo |  |  | ReadOnly=True |  |  | 109 |
| 6 | `ddlFromCmpany` | asp:DropDownList | Company: |  | True |  |  | ddlFromCmpany_SelectedIndexChanged | 123 |
| 7 | `ddlToCompany` | asp:DropDownList | Company: |  | True |  |  | ddlToCompany_SelectedIndexChanged | 131 |
| 8 | `salescenterDropDownList1` | asp:DropDownList | DC (From) |  | True |  |  | salescenterDropDownList1_SelectedIndexChanged | 145 |
| 9 | `salescenterDropDownList2` | asp:DropDownList | DC (To) |  | True |  |  | salescenterDropDownList2_SelectedIndexChanged | 153 |
| 10 | `fromComUnitCodeTextBox` | asp:TextBox | From Sales Center Code |  | True |  |  | fromComUnitCodeTextBox_TextChanged | 167 |
| 11 | `toComUnitCodeTextBox` | asp:TextBox | To Sales Center Code |  | True |  |  | toComUnitCodeTextBox_TextChanged | 174 |
| 12 | `fromComUnitNameTextBox` | asp:TextBox | Name |  |  | ReadOnly=True |  |  | 187 |
| 13 | `toComUnitNameTextBox` | asp:TextBox | Name |  |  |  |  |  | 193 |
| 14 | `fromComUnitAddressTextBox` | asp:TextBox | Address |  |  | ReadOnly=True |  |  | 206 |
| 15 | `toComUnitAddressTextBox` | asp:TextBox | Address |  |  |  |  |  | 213 |
| 16 | `truckNoTextBox` | asp:TextBox | Truck Number |  |  |  |  |  | 227 |
| 17 | `driverNameTextBox` | asp:TextBox | Driver Name |  |  |  |  |  | 233 |
| 18 | `manufacDropDownList` | asp:DropDownList | Manufacturer |  |  |  |  |  | 253 |
| 19 | `prodctCodeTextBox` | asp:TextBox | Product Code |  | True |  |  | productNameTextBox_TextChanged | 288 |
| 20 | `productNameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender | Product Code |  |  | Enabled=True |  |  | 289 |
| 21 | `Button1` | asp:Button |  | Search |  |  |  | Button1_Click | 298 |
| 22 | `productGridView` | asp:GridView |  |  |  |  |  |  | 329 |
| 23 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 332 |
| 24 | `chkSelectAll` | asp:CheckBox | productGridView → col[0] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 334 |
| 25 | `chkSelect` | asp:CheckBox | productGridView → col[0] "(no header)" |  |  |  |  |  | 337 |
| 26 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 340 |
| 27 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 341 |
| 28 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 342 |
| 29 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 343 |
| 30 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 344 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 345 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 346 |
| 33 | `transferQtyTextBox` | asp:TextBox | productGridView → col[7] "Transfer Qty" |  | True |  |  | dQtyTextBox_TextChanged | 348 |
| 34 | `FilteredTextBoxExtenderconvRate` | asp:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 350 |
| 35 | `addButton` | asp:Button |  | Add |  |  |  | addButton_Click | 370 |
| 36 | `chalanGridView` | asp:GridView |  |  |  |  |  |  | 401 |
| 37 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 404 |
| 38 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 405 |
| 39 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 406 |
| 40 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 407 |
| 41 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 408 |
| 42 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 409 |
| 43 | `DeleteImageButton` | asp:ImageButton | chalanGridView → col[5] "Remove Item" | ~/images/lineDelete.png |  |  |  | DeleteImageButton_Click | 411 |
| 44 | `grandTotalWordLabel` | asp:Label | Taka : |  |  |  |  |  | 465 |
| 45 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click1 | 492 |
| 46 | `printChalanNoTextBox` | asp:TextBox | Chalan No |  | True |  |  | fromComUnitCodeTextBox_TextChanged | 509 |
| 47 | `Button2` | asp:Button |  | Print |  |  |  | Button2_Click | 513 |
| 48 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 544 |
| 49 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 548 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `productGridView`

| Attribute | Value |
|---|---|
| datakeynames | `DCStoreId,VATAmountPerUnit,UnitPrice` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |
| 1 | BoundField | Product Code | PCode | — | — | — |
| 2 | BoundField | Product Name | PName | — | — | — |
| 3 | BoundField | Stock Qty | StockQty | — | — | — |
| 4 | BoundField | Batch No | BatchNo | — | — | — |
| 5 | BoundField | ExpDate | ExpDate | — | — | — |
| 6 | BoundField | ReceiveDate | ReceiveDate | — | — | — |
| 7 | TemplateField | Transfer Qty | — | — | `TextBox:transferQtyTextBox`, `FilteredTextBoxExtender:transferQtyTextBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.

#### `chalanGridView`

| Attribute | Value |
|---|---|
| datakeynames | `DCStoreId,VATAmountPerUnit,UnitPrice` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Product Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Transfer Qty | TransferQty | — | — | — |
| 3 | BoundField | Batch No | BatchNo | — | — | — |
| 4 | BoundField | ExpDate | ExpDate | — | — | — |
| 5 | TemplateField | Remove Item | — | — | `ImageButton:DeleteImageButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aChalanBll.LoadManufac(...)` | `ChalanBLL.LoadManufac(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.LoadManufac` → `ChalanDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblManufacturer` | — |
| `submitButton_Click1` | `aChalanBll.LoadChalanById(...)` | `ChalanBLL.LoadChalanById(string id)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.LoadChalanById` → `ChalanDAL.LoadChalanById` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblChalanInfo` | — |
| `submitButton_Click1` | `aChalanBll.SaveDataForChalanDetail(...)` | `ChalanBLL.SaveDataForChalanDetail(List<ChalanDetail> aIChalanList)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.SaveDataForChalanDetail` → `ChalanDAL.CheckDetailsExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.ProcessWeightedAverageB2BTransfer` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | SELECT, UPDATE | `tblChalanDetail`, `tblDCStore` | `sp_Process_WeightedAverageB2BTransfer` |
| `submitButton_Click1` | `aChalanBll.SaveDataForChalanInfo(...)` | `ChalanBLL.SaveDataForChalanInfo(ChalanInfo aChalanInfo, out int ChalanId)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.SaveDataForChalanInfo` → `ChalanDAL.CheckEntryWithinlastMinute` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.SaveDataForChalanInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanBLL.ChalanNoGenerator` | SELECT | `tblChalanInfo` | — |
| `ChalanId` | `aChalanBll.ChalanNoForShow(...)` | `ChalanBLL.ChalanNoForShow()` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.ChalanNoForShow` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanBLL.ChalanNoGenerator` | SELECT | — | — |
| `fromComUnitCodeTextBox_TextChanged` | `aChalanBll.ComUnit(...)` | `ChalanBLL.ComUnit(string ComUnitId)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.ComUnit` → `ChalanDAL.LoadComunit` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.LoadComUnit` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit` | — |
| `toComUnitCodeTextBox_TextChanged` | `aChalanBll.ComUnit(...)` | `ChalanBLL.ComUnit(string ComUnitId)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.ComUnit` → `ChalanDAL.LoadComunit` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.LoadComUnit` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit` | — |
| `Button1_Click` | `aChalanBll.GetProductDcStore(...)` | `ChalanBLL.GetProductDcStore(string productCode, string comunitId)` <br/>`Library.BLL/SInventory_BLL/ChalanBLL.cs` | `ChalanBLL.GetProductDcStore` → `ChalanDAL.GetProductDcStore` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyUnit`, `tblDCStore`, `tblProduct`, `tblUnitPrice` | — |
| `ddlFromCmpany_SelectedIndexChanged` | `aReportDal.LoadDcInfo(...)` | `ASMWiseProductSalesDal.LoadDcInfo(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerPaymentReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |
| `ddlFromCmpany_SelectedIndexChanged` | `aReportDal.LoadDcInfoByUser(...)` | `DestroyProductDal.LoadDcInfoByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.DAL/SInventory_DAL/DestroyProductDal.cs` | `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `SalesSummeryReportForAcc.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `WHStockValuationReprtDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `ddlToCompany_SelectedIndexChanged` | `aReportDal.LoadDcInfo(...)` | `ASMWiseProductSalesDal.LoadDcInfo(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerPaymentReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |
| `ddlToCompany_SelectedIndexChanged` | `aReportDal.LoadDcInfoByUser(...)` | `DestroyProductDal.LoadDcInfoByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.DAL/SInventory_DAL/DestroyProductDal.cs` | `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `SalesSummeryReportForAcc.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `WHStockValuationReprtDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `ddlFromCmpany` | Company: | asp:DropDownList | True | ddlFromCmpany_SelectedIndexChanged | `aDal.DCLoad()` in `ddlFromCmpany_SelectedIndexChanged()`; `aDal.LoadManufac()` in `Page_Load()`; `aReportDal.LoadDcInfo()` in `ddlFromCmpany_SelectedIndexChanged()`; `aReportDal.LoadDcInfoByUser()` in `ddlFromCmpany_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblManufacturer`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `ddlToCompany` | Company: | asp:DropDownList | True | ddlToCompany_SelectedIndexChanged | `aDal.DCLoad()` in `ddlToCompany_SelectedIndexChanged()`; `aDal.LoadManufac()` in `Page_Load()`; `aReportDal.LoadDcInfo()` in `ddlToCompany_SelectedIndexChanged()`; `aReportDal.LoadDcInfoByUser()` in `ddlToCompany_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblManufacturer`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `salescenterDropDownList1` | DC (From) | asp:DropDownList | True | salescenterDropDownList1_SelectedIndexChanged | `aDal.DCLoad()` in `Page_Load()`; `aDal.DCLoad()` in `ddlFromCmpany_SelectedIndexChanged()`; `aReportDal.LoadDcInfo()` in `ddlFromCmpany_SelectedIndexChanged()`; `aReportDal.LoadDcInfoByUser()` in `ddlFromCmpany_SelectedIndexChanged()` | `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `salescenterDropDownList2` | DC (To) | asp:DropDownList | True | salescenterDropDownList2_SelectedIndexChanged | `aDal.DCLoad()` in `ddlToCompany_SelectedIndexChanged()`; `aReportDal.LoadDcInfo()` in `ddlToCompany_SelectedIndexChanged()`; `aReportDal.LoadDcInfoByUser()` in `ddlToCompany_SelectedIndexChanged()` | `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `manufacDropDownList` | Manufacturer | asp:DropDownList | False | — | `aChalanBll.LoadManufac()` in `Page_Load()` | `tblManufacturer` | set in code (see the DAL method above) |

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
| `Button1` | asp:Button | Search | — | `Button1_Click` | 12 lines | `aChalanBll.GetProductDcStore` | — |
| `addButton` | asp:Button | Add | — | `addButton_Click` | 54 lines | — | — |
| `DeleteImageButton` | asp:ImageButton |  | — | `DeleteImageButton_Click` | 43 lines | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click1` | 112 lines | `aChalanBll.LoadChalanById`, `aChalanBll.SaveDataForChalanDetail`, `aChalanBll.SaveDataForChalanInfo` | — |
| `Button2` | asp:Button | Print | — | `Button2_Click` | 7 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 13 | `aChalanBll.LoadManufac` | — | — | — |
| 2 | `Todate` | `void Todate()` | 3 | — | — | — | — |
| 3 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 4 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 5 | `salescenterDropDownList1_SelectedIndexChanged` | `void salescenterDropDownList1_SelectedIndexChanged(object sender, EventArgs e)` | 68 | — | — | — | — |
| 6 | `Clear` | `void Clear()` | 17 | — | — | — | — |
| 7 | `Validation` | `bool Validation()` | 44 | — | — | — | — |
| 8 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 112 | `aChalanBll.LoadChalanById`, `aChalanBll.SaveDataForChalanDetail`, `aChalanBll.SaveDataForChalanInfo` | — | — | `LoginName` |
| 9 | `ChalanId` | `void ChalanId()` | 4 | `aChalanBll.ChalanNoForShow` | — | — | — |
| 10 | `fromComUnitCodeTextBox_TextChanged` | `void fromComUnitCodeTextBox_TextChanged(object sender, EventArgs e)` | 20 | `aChalanBll.ComUnit` | — | — | — |
| 11 | `toComUnitCodeTextBox_TextChanged` | `void toComUnitCodeTextBox_TextChanged(object sender, EventArgs e)` | 20 | `aChalanBll.ComUnit` | — | — | — |
| 12 | `Button1_Click` | `void Button1_Click(object sender, EventArgs e)` | 12 | `aChalanBll.GetProductDcStore` | — | — | — |
| 13 | `HasDCStoreId` | `bool HasDCStoreId(int dcstoreId)` | 12 | — | — | — | — |
| 14 | `addButton_Click` | `void addButton_Click(object sender, EventArgs e)` | 54 | — | — | — | — |
| 15 | `DeleteImageButton_Click` | `void DeleteImageButton_Click(object sender, ImageClickEventArgs e)` | 43 | — | — | — | — |
| 16 | `Button2_Click` | `void Button2_Click(object sender, EventArgs e)` | 7 | — | — | — | — |
| 17 | `dQtyTextBox_TextChanged` | `void dQtyTextBox_TextChanged(object sender, EventArgs e)` | 32 | — | — | — | — |
| 18 | `ddlFromCmpany_SelectedIndexChanged` | `void ddlFromCmpany_SelectedIndexChanged(object sender, EventArgs e)` | 61 | `aReportDal.LoadDcInfo`, `aReportDal.LoadDcInfoByUser` | — | — | `UserId`, `UserType` |
| 19 | `ddlToCompany_SelectedIndexChanged` | `void ddlToCompany_SelectedIndexChanged(object sender, EventArgs e)` | 45 | `aReportDal.LoadDcInfo`, `aReportDal.LoadDcInfoByUser` | — | — | `UserId`, `UserType` |
| 20 | `salescenterDropDownList2_SelectedIndexChanged` | `void salescenterDropDownList2_SelectedIndexChanged(object sender, EventArgs e)` | 55 | — | — | — | — |
| 21 | `productNameTextBox_TextChanged` | `void productNameTextBox_TextChanged(object sender, EventArgs e)` | 17 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
NewStockTransferDcToDc.aspx (markup)
  -> SInventory_UI_NewStockTransferDcToDc (code-behind event handler)
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
| `productNameTextBox_AutoCompleteExtender` | `prodctCodeTextBox` | `GetProductByMenufracturer` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `ChalanBLL` | `LoadManufac` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `submitButton_Click1` | `ChalanBLL` | `LoadChalanById` | `string id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `submitButton_Click1` | `ChalanBLL` | `SaveDataForChalanDetail` | `List<ChalanDetail> aIChalanList` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `submitButton_Click1` | `ChalanBLL` | `SaveDataForChalanInfo` | `ChalanInfo aChalanInfo, out int ChalanId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `ChalanId` | `ChalanBLL` | `ChalanNoForShow` | `` | `string` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `fromComUnitCodeTextBox_TextChanged` | `ChalanBLL` | `ComUnit` | `string ComUnitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `toComUnitCodeTextBox_TextChanged` | `ChalanBLL` | `ComUnit` | `string ComUnitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `Button1_Click` | `ChalanBLL` | `GetProductDcStore` | `string productCode, string comunitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDcInfo` | `DropDownList ddl, string comId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDcInfo` | `DropDownList ddl, string comId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `ddlFromCmpany_SelectedIndexChanged` | `ASMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `BatchWiseTransitReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustomerDueReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustomerPaymentReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustomerReturnBYMioDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustomerWiseLedgerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustomerWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustomerWiseReturnDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustomerWiseSalesReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `CustPaymentDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `DCStockInfoReprotDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `DestroyProductDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `FieldForceSummeryDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `MIOSalesSummeryReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `MioWiseCustomerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `MioWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `MIOWiseProformaReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `RSMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `SalesSummeryReportForAcc` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `WHStockValuationReprtDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `WingsWiseSalesReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `DestroyProductDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `FieldForceSummeryDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `SalesSummeryReportForAcc` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `WHStockValuationReprtDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `ddlFromCmpany_SelectedIndexChanged` | `WingsWiseSalesReportDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `ASMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `BatchWiseTransitReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustomerDueReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustomerPaymentReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustomerReturnBYMioDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustomerWiseLedgerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustomerWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustomerWiseReturnDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustomerWiseSalesReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `ddlToCompany_SelectedIndexChanged` | `CustPaymentDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `ddlToCompany_SelectedIndexChanged` | `DCStockInfoReprotDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `DestroyProductDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `FieldForceSummeryDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `MIOSalesSummeryReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `MioWiseCustomerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `MioWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `MIOWiseProformaReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `ddlToCompany_SelectedIndexChanged` | `RSMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `SalesSummeryReportForAcc` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `ddlToCompany_SelectedIndexChanged` | `WHStockValuationReprtDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `WingsWiseSalesReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `DestroyProductDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `FieldForceSummeryDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `SalesSummeryReportForAcc` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `ddlToCompany_SelectedIndexChanged` | `WHStockValuationReprtDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `ddlToCompany_SelectedIndexChanged` | `WingsWiseSalesReportDal` | `LoadDcInfoByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Process_WeightedAverageB2BTransfer` | submitButton_Click1 → `ChalanBLL.SaveDataForChalanDetail → ChalanDAL.CheckDetailsExistOrNot → ClsCommonInternalDAL.DataContainerDataTable → ChalanDAL.ProcessWeightedAverageB2BTransfer → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblChalanDetail` | `submitButton_Click1()` → `ChalanBLL.SaveDataForChalanDetail` → `ChalanDAL.CheckDetailsExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.ProcessWeightedAverageB2BTransfer` |
| `tblChalanInfo` | `submitButton_Click1()` → `ChalanBLL.LoadChalanById` → `ChalanDAL.LoadChalanById` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `ChalanBLL.SaveDataForChalanInfo` → `ChalanDAL.CheckEntryWithinlastMinute` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.SaveDataForChalanInfo` |
| `tblCompanyUnit` | `Button1_Click()` → `ChalanBLL.GetProductDcStore` → `ChalanDAL.GetProductDcStore` → `ClsCommonInternalDAL.DataContainerDataTable`, `ddlFromCmpany_SelectedIndexChanged()` → `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue`, `ddlFromCmpany_SelectedIndexChanged()` → `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `ddlToCompany_SelectedIndexChanged()` → `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue`, `ddlToCompany_SelectedIndexChanged()` → `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `fromComUnitCodeTextBox_TextChanged()` → `ChalanBLL.ComUnit` → `ChalanDAL.LoadComunit` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.LoadComUnit` |
| `tblDCStore` | `Button1_Click()` → `ChalanBLL.GetProductDcStore` → `ChalanDAL.GetProductDcStore` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton_Click1()` → `ChalanBLL.SaveDataForChalanDetail` → `ChalanDAL.CheckDetailsExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.ProcessWeightedAverageB2BTransfer` |
| `tblManufacturer` | `Page_Load()` → `ChalanBLL.LoadManufac` → `ChalanDAL.LoadManufac` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `Button1_Click()` → `ChalanBLL.GetProductDcStore` → `ChalanDAL.GetProductDcStore` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `Button1_Click()` → `ChalanBLL.GetProductDcStore` → `ChalanDAL.GetProductDcStore` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUserCompanyUnit` | `ddlFromCmpany_SelectedIndexChanged()` → `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue`, `ddlToCompany_SelectedIndexChanged()` → `DestroyProductDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.LoadDcInfoByUser` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtenderconvRate` | `transferQtyTextBox` | Custom | 0123456789 |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-INV-017-001` | {0} | Code | `Solution.Web/SInventory_UI/NewStockTransferDcToDc.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `UserId`, `UserType`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| Database object | `tblChalanDetail` |
| Database object | `tblChalanInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblDCStore` |
| Database object | `tblManufacturer` |
| Database object | `tblProduct` |
| Database object | `tblUnitPrice` |
| Database object | `tblUserCompanyUnit` |
| Stored procedure | `sp_Process_WeightedAverageB2BTransfer` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-017-001` | Unauthenticated access | Request `/SInventory_UI/NewStockTransferDcToDc.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-017-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-INV-017-003` | Grid loads | Open the page with data present | Grid `productGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/NewStockTransferDcToDc.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/NewStockTransferDcToDc.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ChalanBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 49 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 21 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 8 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/ChalanBLL.cs` is **shared with 6 other page(s)** (e.g. `DcToDcChalanReportViewer.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`, `StockReceiveReportByDc.aspx`, `TransferReceiveProductByChalanByDC.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `ASMWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` is **shared with 50 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` is **shared with 51 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` is **shared with 55 other page(s)** (e.g. `CustomerWiseLedgerReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` is **shared with 54 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `CustomerWiseSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockInfoReportViewer.aspx`, `SalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DestroyProductDal.cs` is **shared with 55 other page(s)** (e.g. `DestroyProductReportViewer.aspx`, `DestroyProductStatementViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` is **shared with 59 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `FieldForceSalesSummeryReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `MIOSalesSummeryReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` is **shared with 51 other page(s)** (e.g. `MioWiseCustomerReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `RSMWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` is **shared with 51 other page(s)** (e.g. `SalesSummeryForAccHtmlReport.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockValuationHtmlReport.aspx`, `WHStockValuationHtmlReport.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` is **shared with 57 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`, `WingsWiseCollectionReportViewer.aspx`) — a change there is not local.
- Writes/reads `tblChalanDetail` — any schema change to it breaks this page.
- Writes/reads `tblChalanInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblManufacturer` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
