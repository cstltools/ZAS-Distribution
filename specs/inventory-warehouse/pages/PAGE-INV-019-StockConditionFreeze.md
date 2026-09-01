# INV-019 — StockConditionFreeze

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `INV-019` |
| Module | `inventory-warehouse` |
| File name | `StockConditionFreeze.aspx` |
| Route | `/SInventory_UI/StockConditionFreeze.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `StockConditionFreeze.aspx.cs` |
| Code-behind class | `SInventory_UI_StockConditionFreeze` |
| Markup lines | 492 |
| Code-behind lines | 561 |
| Server controls | 36 |
| GridViews | 1 |
| Code-behind methods | 16 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

Outbound `Response.Redirect` targets: `Login.aspx`

## 5. Route

`/SInventory_UI/StockConditionFreeze.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/StockConditionFreeze.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/StockConditionFreeze.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |

## 7. UI Control Inventory

36 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 7 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 8 |
| 4 | `centalWHCheckBox` | asp:CheckBox | Central Warehouse : |  | True | Enabled=False |  | centalWHCheckBox_CheckedChanged | 46 |
| 5 | `whDropDownList` | asp:DropDownList | Central Warehouse : |  | True |  |  | whDropDownList_SelectedIndexChanged | 64 |
| 6 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_OnSelectedIndexChanged | 83 |
| 7 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  |  |  |  |  | 102 |
| 8 | `stockInIdHiddenField` | asp:HiddenField | Distribution Center: |  |  |  |  |  | 105 |
| 9 | `manufacturerDropDownList` | asp:DropDownList | Manufacturer: |  | True |  |  | manufacturerDropDownList_SelectedIndexChanged | 122 |
| 10 | `dcDropDownList1` | asp:DropDownList | Distribution Center : |  | True |  |  | dcDropDownList1_SelectedIndexChanged | 141 |
| 11 | `submitButton` | asp:Button |  | Search |  |  |  | submitButton_Click | 177 |
| 12 | `submitButton0` | asp:Button |  | Submit |  |  |  | submitButton0_OnClick | 181 |
| 13 | `loadGridView` | asp:GridView |  |  |  |  |  | loadGridView_RowCommand | 216 |
| 14 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 219 |
| 15 | `LabelSL` | asp:Label | loadGridView → col[0] "#SL" |  |  |  |  |  | 221 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 224 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 225 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 226 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 227 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 228 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 229 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 230 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 231 |
| 24 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 232 |
| 25 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 233 |
| 26 | `StockConditionDropDownList` | asp:DropDownList | loadGridView → col[10] "Stock Condition" |  | True |  |  | myListDropDown_Change | 235 |
| 27 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 240 |
| 28 | `returnQtyTextBox` | asp:TextBox | loadGridView → col[11] "Qty" |  |  |  |  |  | 242 |
| 29 | `FilteredTextBoxExtenderconvRate` | cc1:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 243 |
| 30 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 248 |
| 31 | `remarksTextBox` | asp:TextBox | loadGridView → col[12] "Remarks" |  |  |  |  |  | 250 |
| 32 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 253 |
| 33 | `chkSelectAll` | asp:CheckBox | loadGridView → col[13] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 255 |
| 34 | `chkSelect` | asp:CheckBox | loadGridView → col[13] "(no header)" |  | True |  |  |  | 258 |
| 35 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 482 |
| 36 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 485 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `loadGridView`

| Attribute | Value |
|---|---|
| datakeynames | `nomanslandID` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | #SL | — | — | `Label:LabelSL` | — |
| 1 | BoundField | ProductCode | ProductCode | — | — | — |
| 2 | BoundField | Product Name | ProductName | — | — | — |
| 3 | BoundField | Batch | BatchNo | — | — | — |
| 4 | BoundField | ExpDate | ExpDate | — | — | — |
| 5 | BoundField | Receive Date | ReceiveDate | — | — | — |
| 6 | BoundField | StockInQty | TotalQuantity | — | — | — |
| 7 | BoundField | C.StockQty | StockQty | — | — | — |
| 8 | BoundField | Amount | Amount | — | — | — |
| 9 | BoundField | Stock | StockCondition | — | — | — |
| 10 | TemplateField | Stock Condition | — | — | `DropDownList:StockConditionDropDownList` | — |
| 11 | TemplateField | Qty | — | — | `TextBox:returnQtyTextBox` | — |
| 12 | TemplateField | Remarks | — | — | `TextBox:remarksTextBox` | — |
| 13 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `DCStoreFreezeDAO`, `StockConditionFreezeDAO`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDown` | `aOtherStockActionBLL.DCLoad(...)` | `OtherStockActionBLL.DCLoad(DropDownList aDownList, string userId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `LoadDropDown` | `aOtherStockActionBLL.LoadmanufacturerName(...)` | `OtherStockActionBLL.LoadmanufacturerName(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.LoadmanufacturerName` → `OtherStockActionDAL.LoadmanufacturerName` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyInfo` | — |
| `LoadDropDown` | `aStockConditionFreezeBll.LoadCompanyDropdownList(...)` | `StockConditionFreezeBLL.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadCompanyDropdownList` | — | — | — |
| `companyNameDropDownList_OnSelectedIndexChanged` | `aStockConditionFreezeBll.LoadWhInformation(...)` | `StockConditionFreezeBLL.LoadWhInformation(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` → `RequisitionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` → `StockConditionFreezeDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` → `WarehouseStockInDal.GetWhInformation` | SELECT | `tblCompanyUnit`, `tblWearhouse` | — |
| `companyNameDropDownList_OnSelectedIndexChanged` | `aStockConditionFreezeBll.LoadWhInformationByUser(...)` | `StockConditionFreezeBLL.LoadWhInformationByUser(DropDownList ddl, string companyId,string userId)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadWhInformationByUser` → `BatchWiseTransitReportDal.GetWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerReturnBYMioDal.GetWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerWiseReturnDal.GetWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `FieldForceSummeryDal.GetWhInformationByUser` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `centalWHCheckBox_CheckedChanged` | `aRequisitionBll.WareHouseLoad(...)` | `RequisitionBLL.WareHouseLoad(DropDownList aDownList)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.WareHouseLoad` → `RequisitionDAL.WareHouseLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblWearhouse` | — |
| `LoadDCStoreData` | `aStockConditionFreezeBll.LoadStockConditionBll(...)` | `StockConditionFreezeBLL.LoadStockConditionBll(DropDownList aDownList,string userid)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadStockConditionBll` → `StockConditionFreezeDAL.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeDal.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeBll.LoadStockConditionBll` | SELECT | `tblStockCondition`, `tblStockConditionPermission` | — |
| `LoadDCStoreData` | `aStockConditionFreezeBll.LoadStockDCData(...)` | `StockConditionFreezeBLL.LoadStockDCData(int id)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadStockDCData` → `StockConditionFreezeDAL.LoadStockDCData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore`, `tblUnitPrice` | — |
| `LoadWHData` | `aStockConditionFreezeBll.LoadStockConditionBll(...)` | `StockConditionFreezeBLL.LoadStockConditionBll(DropDownList aDownList,string userid)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadStockConditionBll` → `StockConditionFreezeDAL.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeDal.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeBll.LoadStockConditionBll` | SELECT | `tblStockCondition`, `tblStockConditionPermission` | — |
| `LoadWHData` | `aStockConditionFreezeBll.LoadWHData(...)` | `StockConditionFreezeBLL.LoadWHData(int id)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeBll.LoadWHData` | SELECT | `tblCentralStore`, `tblUnitPrice` | — |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.LoadStockStockQtyDCData(...)` | `StockConditionFreezeBLL.LoadStockStockQtyDCData(int id)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadStockStockQtyDCData` → `StockConditionFreezeDAL.LoadStockStockQtyDCData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore`, `tblUnitPrice` | — |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.LoadWHData(...)` | `StockConditionFreezeBLL.LoadWHData(int id)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeBll.LoadWHData` | SELECT | `tblCentralStore`, `tblUnitPrice` | — |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.SaveDCStoreFreeze(...)` | `StockConditionFreezeBLL.SaveDCStoreFreeze(DCStoreFreezeDAO aDcStoreFreezeDao)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveDCStoreFreeze` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveDCStoreFreeze` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` | INSERT, SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.SaveDCStoreFreeze2(...)` | `StockConditionFreezeBLL.SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStoreFreezeDao)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveDCStoreFreeze2` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.SaveDCStoreFreeze2` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `RequisitionDAL.SaveDCStoreFreeze2` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `StockConditionFreezeDAL.SaveDCStoreFreeze2` | SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.SaveforDC(...)` | `StockConditionFreezeBLL.SaveforDC(StockConditionFreezeDAO aStockConditionFreezeDAO)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveforDC` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforDC` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `CustomerMasterDAL.HasCustomerMastername` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblCustMaster`, `tblStockConditionFreeze` | — |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.SaveforWH(...)` | `StockConditionFreezeBLL.SaveforWH(StockConditionFreezeDAO aStockConditionFreezeDAO)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `WhStockConditionFreezeDal.SaveforWh` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `WhStockConditionFreezeBll.SaveforWh` | SELECT | `tblCustMaster`, `tblStockConditionFreeze`, `tblWhStockConditionFreeze` | — |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.UpdateCentralStore(...)` | `StockConditionFreezeBLL.UpdateCentralStore(decimal StockQty, int ReceiveId)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.UpdateCentralStore` → `StockConditionFreezeDAL.UpdateCentralStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `WhStockConditionFreezeDal.UpdateCentralStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `WhStockConditionFreezeBll.UpdateCentralStore` | UPDATE | `tblCentralStore` | — |
| `loadGridView_RowCommand` | `aStockConditionFreezeBll.UpdateDCStore(...)` | `StockConditionFreezeBLL.UpdateDCStore(decimal StockQty, int DCStoreId)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.UpdateDCStore` → `StockConditionFreezeDAL.UpdateDCStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDCStore` | — |
| `Validation` | `aInvoiceBll.GetFinancialYearInfo(...)` | `InvoiceBLL.GetFinancialYearInfo(string invoiceDate,string unitId)` <br/>`Library.BLL/SInventory_BLL/InvoiceBLL.cs` | `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit` | — |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.LoadStockStockQtyDCData(...)` | `StockConditionFreezeBLL.LoadStockStockQtyDCData(int id)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadStockStockQtyDCData` → `StockConditionFreezeDAL.LoadStockStockQtyDCData` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblDCStore`, `tblUnitPrice` | — |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.LoadWHData(...)` | `StockConditionFreezeBLL.LoadWHData(int id)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeBll.LoadWHData` | SELECT | `tblCentralStore`, `tblUnitPrice` | — |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.PostJournal(...)` | `StockConditionFreezeBLL.PostJournal(int stockConditionFreezeId, decimal quantity)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.PostJournal` → `StockConditionFreezeDAL.PostFreezeStockJournal` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` | — | — | `sp_FreezeStockPosting` |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.SaveDCStoreFreeze(...)` | `StockConditionFreezeBLL.SaveDCStoreFreeze(DCStoreFreezeDAO aDcStoreFreezeDao)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveDCStoreFreeze` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveDCStoreFreeze` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `InvoiceBLL.SaveDCStoreFreeze` → `ChalanDAL.ProcessWeightedAverageFreezeStock` → `ClsCommonInternalDAL.UpdateAction` | INSERT, SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.SaveDCStoreFreeze2(...)` | `StockConditionFreezeBLL.SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStoreFreezeDao)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveDCStoreFreeze2` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.SaveDCStoreFreeze2` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `RequisitionDAL.SaveDCStoreFreeze2` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `StockConditionFreezeDAL.SaveDCStoreFreeze2` | SELECT | `tblDCStoreFreeze` | `sp_Process_WeightedAverageFreezeStock` |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.SaveforDC(...)` | `StockConditionFreezeBLL.SaveforDC(StockConditionFreezeDAO aStockConditionFreezeDAO)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveforDC` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforDC` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `CustomerMasterDAL.HasCustomerMastername` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblCustMaster`, `tblStockConditionFreeze` | — |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.SaveforWH(...)` | `StockConditionFreezeBLL.SaveforWH(StockConditionFreezeDAO aStockConditionFreezeDAO)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `WhStockConditionFreezeDal.SaveforWh` → `ClsCommonInternalDAL.SaveDataByInsertCommand` → `WhStockConditionFreezeBll.SaveforWh` | SELECT | `tblCustMaster`, `tblStockConditionFreeze`, `tblWhStockConditionFreeze` | — |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.UpdateCentralStore(...)` | `StockConditionFreezeBLL.UpdateCentralStore(decimal StockQty, int ReceiveId)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.UpdateCentralStore` → `StockConditionFreezeDAL.UpdateCentralStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `WhStockConditionFreezeDal.UpdateCentralStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `WhStockConditionFreezeBll.UpdateCentralStore` | UPDATE | `tblCentralStore` | — |
| `submitButton0_OnClick` | `aStockConditionFreezeBll.UpdateDCStore(...)` | `StockConditionFreezeBLL.UpdateDCStore(decimal StockQty, int DCStoreId)` <br/>`Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | `StockConditionFreezeBLL.UpdateDCStore` → `StockConditionFreezeDAL.UpdateDCStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblDCStore` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `whDropDownList` | Central Warehouse : | asp:DropDownList | True | whDropDownList_SelectedIndexChanged | `aRequisitionBll.WareHouseLoad()` in `centalWHCheckBox_CheckedChanged()` | `tblWearhouse` | set in code (see the DAL method above) |
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | companyNameDropDownList_OnSelectedIndexChanged | `aStockConditionFreezeBll.LoadCompanyDropdownList()` in `LoadDropDown()`; `aStockConditionFreezeBll.LoadWhInformation()` in `companyNameDropDownList_OnSelectedIndexChanged()`; `aStockConditionFreezeBll.LoadWhInformationByUser()` in `companyNameDropDownList_OnSelectedIndexChanged()` | `tblCompanyUnit`, `tblUserCompanyUnit`, `tblWearhouse` | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center: | asp:DropDownList | False | — | `aInvoiceBll.GetFinancialYearInfo()` in `Validation()`; `aStockConditionFreezeBll.LoadStockDCData()` in `LoadDCStoreData()`; `aStockConditionFreezeBll.LoadWhInformation()` in `companyNameDropDownList_OnSelectedIndexChanged()`; `aStockConditionFreezeBll.LoadWhInformationByUser()` in `companyNameDropDownList_OnSelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblUnitPrice`, `tblUserCompanyUnit`, `tblWearhouse` | set in code (see the DAL method above) |
| `manufacturerDropDownList` | Manufacturer: | asp:DropDownList | True | manufacturerDropDownList_SelectedIndexChanged | `aOtherStockActionBLL.LoadmanufacturerName()` in `LoadDropDown()` | `tblCompanyInfo` | set in code (see the DAL method above) |
| `dcDropDownList1` | Distribution Center : | asp:DropDownList | True | dcDropDownList1_SelectedIndexChanged | `aOtherStockActionBLL.DCLoad()` in `LoadDropDown()` | `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `StockConditionDropDownList` | loadGridView → col[10] "Stock Condition" | asp:DropDownList | True | myListDropDown_Change | `aStockConditionFreezeBll.LoadStockConditionBll()` in `LoadDCStoreData()`; `aStockConditionFreezeBll.LoadStockConditionBll()` in `LoadWHData()` | `tblStockCondition`, `tblStockConditionPermission` | set in code (see the DAL method above) |

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
| `submitButton` | asp:Button | Search | — | `submitButton_Click` | 10 lines | — | — |
| `submitButton0` | asp:Button | Submit | — | `submitButton0_OnClick` | 212 lines | `aStockConditionFreezeBll.LoadStockStockQtyDCData`, `aStockConditionFreezeBll.LoadWHData`, `aStockConditionFreezeBll.PostJournal`, `aStockConditionFreezeBll.SaveDCStoreFreeze`, `aStockConditionFreezeBll.SaveDCStoreFreeze2` | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:FilteredTextBoxExtender`(returnQtyTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 12 | — | — | — | `CentralWareHouse` |
| 2 | `myListDropDown_Change` | `void myListDropDown_Change(object sender, EventArgs e)` | 16 | — | — | — | `LoginName` |
| 3 | `LoadDropDown` | `void LoadDropDown()` | 8 | `aOtherStockActionBLL.DCLoad`, `aOtherStockActionBLL.LoadmanufacturerName`, `aStockConditionFreezeBll.LoadCompanyDropdownList` | — | — | `UserId` |
| 4 | `companyNameDropDownList_OnSelectedIndexChanged` | `void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 26 | `aStockConditionFreezeBll.LoadWhInformation`, `aStockConditionFreezeBll.LoadWhInformationByUser` | — | — | `UserId`, `UserType` |
| 5 | `centalWHCheckBox_CheckedChanged` | `void centalWHCheckBox_CheckedChanged(object sender, EventArgs e)` | 17 | `aRequisitionBll.WareHouseLoad` | — | — | — |
| 6 | `whDropDownList_SelectedIndexChanged` | `void whDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 7 | `dcDropDownList1_SelectedIndexChanged` | `void dcDropDownList1_SelectedIndexChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 8 | `manufacturerDropDownList_SelectedIndexChanged` | `void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | — | — | — | — |
| 9 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 10 | — | — | — | — |
| 10 | `LoadDCStoreData` | `void LoadDCStoreData()` | 12 | `aStockConditionFreezeBll.LoadStockConditionBll`, `aStockConditionFreezeBll.LoadStockDCData` | — | — | `UserId` |
| 11 | `LoadWHData` | `void LoadWHData()` | 11 | `aStockConditionFreezeBll.LoadStockConditionBll`, `aStockConditionFreezeBll.LoadWHData` | — | — | `UserId` |
| 12 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 13 | `loadGridView_RowCommand` | `void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)` | 152 | `aStockConditionFreezeBll.LoadStockStockQtyDCData`, `aStockConditionFreezeBll.LoadWHData`, `aStockConditionFreezeBll.SaveDCStoreFreeze`, `aStockConditionFreezeBll.SaveDCStoreFreeze2`, `aStockConditionFreezeBll.SaveforDC`, `aStockConditionFreezeBll.SaveforWH` | — | — | `LoginName` |
| 14 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 16 | — | — | — | — |
| 15 | `Validation` | `bool Validation()` | 12 | `aInvoiceBll.GetFinancialYearInfo` | — | — | — |
| 16 | `submitButton0_OnClick` | `void submitButton0_OnClick(object sender, EventArgs e)` | 212 | `aStockConditionFreezeBll.LoadStockStockQtyDCData`, `aStockConditionFreezeBll.LoadWHData`, `aStockConditionFreezeBll.PostJournal`, `aStockConditionFreezeBll.SaveDCStoreFreeze`, `aStockConditionFreezeBll.SaveDCStoreFreeze2`, `aStockConditionFreezeBll.SaveforDC` | — | — | `LoginName` |

Call chain shape for every persisting action on this page:

```
StockConditionFreeze.aspx (markup)
  -> SInventory_UI_StockConditionFreeze (code-behind event handler)
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
| `LoadDropDown` | `OtherStockActionBLL` | `DCLoad` | `DropDownList aDownList, string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `LoadDropDown` | `OtherStockActionBLL` | `LoadmanufacturerName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| `LoadDropDown` | `StockConditionFreezeBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `StockConditionFreezeBLL` | `LoadWhInformation` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `companyNameDropDownList_OnSelectedIndexChanged` | `StockConditionFreezeBLL` | `LoadWhInformationByUser` | `DropDownList ddl, string companyId,string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `centalWHCheckBox_CheckedChanged` | `RequisitionBLL` | `WareHouseLoad` | `DropDownList aDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `LoadDCStoreData` | `StockConditionFreezeBLL` | `LoadStockConditionBll` | `DropDownList aDownList,string userid` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `LoadDCStoreData` | `StockConditionFreezeBLL` | `LoadStockDCData` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `LoadWHData` | `StockConditionFreezeBLL` | `LoadStockConditionBll` | `DropDownList aDownList,string userid` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `LoadWHData` | `StockConditionFreezeBLL` | `LoadWHData` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `LoadStockStockQtyDCData` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `LoadWHData` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `SaveDCStoreFreeze` | `DCStoreFreezeDAO aDcStoreFreezeDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `SaveDCStoreFreeze2` | `DCStoreFreezeDAO aDcStoreFreezeDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `SaveforDC` | `StockConditionFreezeDAO aStockConditionFreezeDAO` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `SaveforWH` | `StockConditionFreezeDAO aStockConditionFreezeDAO` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `UpdateCentralStore` | `decimal StockQty, int ReceiveId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `loadGridView_RowCommand` | `StockConditionFreezeBLL` | `UpdateDCStore` | `decimal StockQty, int DCStoreId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `Validation` | `InvoiceBLL` | `GetFinancialYearInfo` | `string invoiceDate,string unitId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `LoadStockStockQtyDCData` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `LoadWHData` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `PostJournal` | `int stockConditionFreezeId, decimal quantity` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `SaveDCStoreFreeze` | `DCStoreFreezeDAO aDcStoreFreezeDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `SaveDCStoreFreeze2` | `DCStoreFreezeDAO aDcStoreFreezeDao` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `SaveforDC` | `StockConditionFreezeDAO aStockConditionFreezeDAO` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `SaveforWH` | `StockConditionFreezeDAO aStockConditionFreezeDAO` | `int` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `UpdateCentralStore` | `decimal StockQty, int ReceiveId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| `submitButton0_OnClick` | `StockConditionFreezeBLL` | `UpdateDCStore` | `decimal StockQty, int DCStoreId` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_Process_WeightedAverageFreezeStock` | loadGridView_RowCommand → `StockConditionFreezeBLL.SaveDCStoreFreeze → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → StockConditionFreezeDAL.SaveDCStoreFreeze → ClsCommonInternalDAL.SaveDataByInsertCommand → InvoiceBLL.SaveDCStoreFreeze` | **No** — see note |
| `sp_FreezeStockPosting` | submitButton0_OnClick → `StockConditionFreezeBLL.PostJournal → StockConditionFreezeDAL.PostFreezeStockJournal → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |
| `sp_Process_WeightedAverageFreezeStock` | submitButton0_OnClick → `StockConditionFreezeBLL.SaveDCStoreFreeze → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → StockConditionFreezeDAL.SaveDCStoreFreeze → ClsCommonInternalDAL.SaveDataByInsertCommand → InvoiceBLL.SaveDCStoreFreeze` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCentralStore` | `LoadWHData()` → `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.UpdateCentralStore` → `StockConditionFreezeDAL.UpdateCentralStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `WhStockConditionFreezeDal.UpdateCentralStore`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.UpdateCentralStore` → `StockConditionFreezeDAL.UpdateCentralStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `WhStockConditionFreezeDal.UpdateCentralStore` |
| `tblCompanyInfo` | `LoadDropDown()` → `OtherStockActionBLL.LoadmanufacturerName` → `OtherStockActionDAL.LoadmanufacturerName` → `ClsCommonInternalDAL.LoadDropDownValue`, `Validation()` → `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `LoadDropDown()` → `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `Validation()` → `InvoiceBLL.GetFinancialYearInfo` → `InvoiceDAL.GetFinancialYearInfo` → `ClsCommonInternalDAL.DataContainerDataTable`, `companyNameDropDownList_OnSelectedIndexChanged()` → `StockConditionFreezeBLL.LoadWhInformationByUser` → `BatchWiseTransitReportDal.GetWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerReturnBYMioDal.GetWhInformationByUser`, `companyNameDropDownList_OnSelectedIndexChanged()` → `StockConditionFreezeBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` → `RequisitionDAL.GetWhInformation` |
| `tblCustMaster` | `loadGridView_RowCommand()` → `StockConditionFreezeBLL.SaveforDC` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforDC`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.SaveforDC` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforDC`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH` |
| `tblDCStore` | `LoadDCStoreData()` → `StockConditionFreezeBLL.LoadStockDCData` → `StockConditionFreezeDAL.LoadStockDCData` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.LoadStockStockQtyDCData` → `StockConditionFreezeDAL.LoadStockStockQtyDCData` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.UpdateDCStore` → `StockConditionFreezeDAL.UpdateDCStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.LoadStockStockQtyDCData` → `StockConditionFreezeDAL.LoadStockStockQtyDCData` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.UpdateDCStore` → `StockConditionFreezeDAL.UpdateDCStore` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblDCStoreFreeze` | `loadGridView_RowCommand()` → `StockConditionFreezeBLL.SaveDCStoreFreeze2` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.SaveDCStoreFreeze2`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.SaveDCStoreFreeze` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveDCStoreFreeze`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.SaveDCStoreFreeze2` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ChalanDAL.SaveDCStoreFreeze2`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.SaveDCStoreFreeze` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveDCStoreFreeze` |
| `tblStockCondition` | `LoadDCStoreData()` → `StockConditionFreezeBLL.LoadStockConditionBll` → `StockConditionFreezeDAL.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeDal.LoadStockConditionBll`, `LoadWHData()` → `StockConditionFreezeBLL.LoadStockConditionBll` → `StockConditionFreezeDAL.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeDal.LoadStockConditionBll` |
| `tblStockConditionFreeze` | `loadGridView_RowCommand()` → `StockConditionFreezeBLL.SaveforDC` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforDC`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.SaveforDC` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforDC`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH` |
| `tblStockConditionPermission` | `LoadDCStoreData()` → `StockConditionFreezeBLL.LoadStockConditionBll` → `StockConditionFreezeDAL.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeDal.LoadStockConditionBll`, `LoadWHData()` → `StockConditionFreezeBLL.LoadStockConditionBll` → `StockConditionFreezeDAL.LoadStockConditionBll` → `ClsCommonInternalDAL.LoadDropDownValue` → `WhStockConditionFreezeDal.LoadStockConditionBll` |
| `tblUnitPrice` | `LoadDCStoreData()` → `StockConditionFreezeBLL.LoadStockDCData` → `StockConditionFreezeDAL.LoadStockDCData` → `ClsCommonInternalDAL.DataContainerDataTable`, `LoadWHData()` → `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.LoadStockStockQtyDCData` → `StockConditionFreezeDAL.LoadStockStockQtyDCData` → `ClsCommonInternalDAL.DataContainerDataTable`, `loadGridView_RowCommand()` → `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.LoadStockStockQtyDCData` → `StockConditionFreezeDAL.LoadStockStockQtyDCData` → `ClsCommonInternalDAL.DataContainerDataTable`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.LoadWHData` → `StockConditionFreezeDAL.LoadWHData` → `ClsCommonInternalDAL.DataContainerDataTable` → `WhStockConditionFreezeDal.LoadWHData` |
| `tblUserCompanyUnit` | `LoadDropDown()` → `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `companyNameDropDownList_OnSelectedIndexChanged()` → `StockConditionFreezeBLL.LoadWhInformationByUser` → `BatchWiseTransitReportDal.GetWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerReturnBYMioDal.GetWhInformationByUser` |
| `tblWearhouse` | `centalWHCheckBox_CheckedChanged()` → `RequisitionBLL.WareHouseLoad` → `RequisitionDAL.WareHouseLoad` → `ClsCommonInternalDAL.LoadDropDownValue`, `companyNameDropDownList_OnSelectedIndexChanged()` → `StockConditionFreezeBLL.LoadWhInformation` → `OtherStockActionDAL.GetWhInformation` → `ClsCommonInternalDAL.LoadDropDownValue` → `RequisitionDAL.GetWhInformation` |
| `tblWhStockConditionFreeze` | `loadGridView_RowCommand()` → `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH`, `submitButton0_OnClick()` → `StockConditionFreezeBLL.SaveforWH` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `StockConditionFreezeDAL.SaveforWH` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtenderconvRate` | `returnQtyTextBox` | Custom | 0123456789 |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-INV-019-001` | {0} | Code | `Solution.Web/SInventory_UI/StockConditionFreeze.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `CentralWareHouse`, `LoginName`, `UserId`, `UserType`

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

- Accounting posting into `ZAS_ACCDB` via `sp_*Posting` / `sp_Sales_*` procedures.

See `specs/00-system/integrations.md` for the full list.

## 25. Reports

_This page opens no report._

## 26. Dependencies

| Kind | Dependency |
|---|---|
| Master page | `MasterPages/MainMasterPage.master` (auth guard + menu) |
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Database object | `tblCentralStore` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblDCStore` |
| Database object | `tblDCStoreFreeze` |
| Database object | `tblStockCondition` |
| Database object | `tblStockConditionFreeze` |
| Database object | `tblStockConditionPermission` |
| Database object | `tblUnitPrice` |
| Database object | `tblUserCompanyUnit` |
| Database object | `tblWearhouse` |
| Database object | `tblWhStockConditionFreeze` |
| Stored procedure | `sp_Process_WeightedAverageFreezeStock` |
| Stored procedure | `sp_FreezeStockPosting` |
| Stored procedure | `sp_Process_WeightedAverageFreezeStock` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-INV-019-001` | Unauthenticated access | Request `/SInventory_UI/StockConditionFreeze.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-INV-019-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-INV-019-003` | Grid loads | Open the page with data present | Grid `loadGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/StockConditionFreeze.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/StockConditionFreeze.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 36 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 16 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 3 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 13 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` is **shared with 43 other page(s)** (e.g. `ProformaHTMLReport.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseDelReturnViewer.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` is **shared with 22 other page(s)** (e.g. `AllSalesReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`, `BatchWiseTopSheet.aspx`, `CustomerPayment.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RequisitionBLL.cs` is **shared with 29 other page(s)** (e.g. `Dashboard.aspx`, `B2BReceiveViewer.aspx`, `StockReceiveReportViewer.aspx`, `TransferStockReceiveReportViewer.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` is **shared with 9 other page(s)** (e.g. `AllSalesReport.aspx`, `DcWiseAreaAlocation.aspx`, `DeliveryReturnReport.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`) — a change there is not local.
- Writes/reads `tblCentralStore` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblDCStore` — any schema change to it breaks this page.
- Writes/reads `tblDCStoreFreeze` — any schema change to it breaks this page.
- Writes/reads `tblStockCondition` — any schema change to it breaks this page.
- Writes/reads `tblStockConditionFreeze` — any schema change to it breaks this page.
- Writes/reads `tblStockConditionPermission` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblWearhouse` — any schema change to it breaks this page.
- Writes/reads `tblWhStockConditionFreeze` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
