# SAL-014 — ManualOrderrCreation

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-014` |
| Module | `sales-order-invoice` |
| File name | `ManualOrderrCreation.aspx` |
| Route | `/SInventory_UI/ManualOrderrCreation.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ManualOrderrCreation.aspx.cs` |
| Code-behind class | `SInventory_UI_ManualOrderrCreation` |
| Markup lines | 600 |
| Code-behind lines | 1213 |
| Server controls | 67 |
| GridViews | 1 |
| Code-behind methods | 33 |

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

`/SInventory_UI/ManualOrderrCreation.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |

## 7. UI Control Inventory

67 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 48 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 49 |
| 4 | `orderIdHiddenField` | asp:HiddenField | Manual Order Creation |  |  |  |  |  | 61 |
| 5 | `custIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 62 |
| 6 | `paymentHiddenField` | asp:HiddenField |  |  |  |  |  |  | 63 |
| 7 | `dueHiddenField` | asp:HiddenField |  |  |  |  |  |  | 64 |
| 8 | `isVatApplicable` | asp:HiddenField |  |  |  |  |  |  | 65 |
| 9 | `orderNoTextBox` | asp:TextBox |  |  |  | ReadOnly=True | False |  | 68 |
| 10 | `mioIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 73 |
| 11 | `delIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 74 |
| 12 | `custCodeTextBox` | asp:TextBox | Customer Code: |  | True |  |  | custCodeTextBox_TextChanged | 96 |
| 13 | `customerTextBox_AutoCompleteExtender` | ajaxToolkit:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 98 |
| 14 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  | True |  |  | dcDropDownList_SelectedIndexChanged | 133 |
| 15 | `regionDropDownList` | asp:DropDownList | Region: |  | True | Enabled=False |  |  | 140 |
| 16 | `areaDropDownList` | asp:DropDownList | Area: |  | True | Enabled=False |  |  | 147 |
| 17 | `companyDropDownList` | asp:DropDownList | Company: |  | True | Enabled=False |  |  | 156 |
| 18 | `territoryDropDownList` | asp:DropDownList | Territory: |  | True | Enabled=False |  |  | 165 |
| 19 | `marketDropDownList` | asp:DropDownList | Market: |  | True | Enabled=False |  |  | 172 |
| 20 | `discountIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 179 |
| 21 | `mioDropDownList` | asp:DropDownList | MIO: |  | True | Enabled=False |  |  | 183 |
| 22 | `previousDueLabel` | asp:Label | Previous Due: |  |  |  |  |  | 190 |
| 23 | `mioCodeLabel` | asp:Label | MIO Code: |  |  |  |  |  | 215 |
| 24 | `mioNameLabel` | asp:Label | MIO Name: |  |  |  |  |  | 220 |
| 25 | `custNameLabel` | asp:Label | Customer Name: |  |  |  |  |  | 225 |
| 26 | `marketNameLabel` | asp:Label | Market Name: |  |  |  |  |  | 232 |
| 27 | `creditLimitNameLabel` | asp:Label | Credit Limit: |  |  |  |  |  | 237 |
| 28 | `crtakenNameLabel` | asp:Label | Credit Taken: |  |  |  |  |  | 242 |
| 29 | `grossLabel` | asp:Label | Total Gross: |  |  |  |  |  | 251 |
| 30 | `remainingcrLabel` | asp:Label | Remaining: |  |  |  |  |  | 257 |
| 31 | `percentOfUsedCrLabel` | asp:Label | % of Used Credit: |  |  |  |  |  | 263 |
| 32 | `orderDateTextBox` | asp:Label | Order Date: |  | True |  |  |  | 285 |
| 33 | `isSpecialAppCheckBox` | asp:CheckBox | Is Special Order: |  |  |  | False |  | 297 |
| 34 | `productGridView` | asp:GridView |  |  |  |  |  |  | 335 |
| 35 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 337 |
| 36 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 344 |
| 37 | `productCodeTextBox` | asp:TextBox | productGridView → col[1] "Product Code" |  | True |  |  | productCodeTextBox_TextChanged | 346 |
| 38 | `unitpriceHiddenField` | asp:HiddenField | productGridView → col[1] "Product Code" |  |  |  |  |  | 348 |
| 39 | `productidHiddenField` | asp:HiddenField | productGridView → col[1] "Product Code" |  |  |  |  |  | 349 |
| 40 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 352 |
| 41 | `productNameTextBox` | asp:TextBox | productGridView → col[2] "Product Name" |  | True |  |  | productNameTextBox_TextChanged | 354 |
| 42 | `productNameTextBox_AutoCompleteExtender` | ajaxToolkit:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 356 |
| 43 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 365 |
| 44 | `packSizeTextBox` | asp:TextBox | productGridView → col[3] "PackSize" |  |  | ReadOnly=True |  |  | 367 |
| 45 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 371 |
| 46 | `reqQtyTextBox` | asp:TextBox | productGridView → col[4] "Req. Qty" |  | True |  |  | reqQtyTextBox_OnTextChanged | 373 |
| 47 | `freqQtyTextBox` | ajaxToolkit:FilteredTextBoxExtender |  |  |  |  |  |  | 375 |
| 48 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 381 |
| 49 | `vatTextBox` | asp:TextBox | productGridView → col[5] "VAT" |  |  | Enabled=False |  |  | 383 |
| 50 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 386 |
| 51 | `tpTextBox` | asp:TextBox | productGridView → col[6] "TP" |  |  | Enabled=False |  |  | 388 |
| 52 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 391 |
| 53 | `grossTextBox` | asp:TextBox | productGridView → col[7] "TPGross" |  |  | Enabled=False |  |  | 393 |
| 54 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 396 |
| 55 | `ImageButton1` | asp:ImageButton | productGridView → col[8] "Add" | ~/images/lineAdd.png |  |  |  | ImageButton1_Click | 398 |
| 56 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 402 |
| 57 | `ImageButton2` | asp:ImageButton | productGridView → col[9] "Remove" | ~/images/lineDelete.png |  |  |  | ImageButton2_Click | 404 |
| 58 | `submitButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 436 |
| 59 | `ModalPopupExtender10` | ajaxToolkit:ModalPopupExtender |  |  |  |  |  |  | 439 |
| 60 | `HiddenField10` | asp:HiddenField |  |  |  |  |  |  | 443 |
| 61 | `pnlModal` | asp:Panel |  |  |  |  |  |  | 444 |
| 62 | `ImageButton1` | asp:ImageButton | productGridView → col[8] "Add" | ../PopCss/Images/close.jpg |  |  |  |  | 448 |
| 63 | `Image2` | asp:Image |  | ~/PopCss/Images/question.png |  |  |  |  | 453 |
| 64 | `orderCodeTextBox` | asp:TextBox | Generated ordrer code |  |  |  |  |  | 457 |
| 65 | `yesButton` | asp:Button | rdrer code | Proceed next |  |  |  | yesButton_Click | 462 |
| 66 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 469 |
| 67 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 472 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `productGridView`

| Attribute | Value |
|---|---|
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | SL | — | — | — | — |
| 1 | TemplateField | Product Code | — | `ProductCode`, `ProductId`, `UnitPrice` | `TextBox:productCodeTextBox`, `HiddenField:unitpriceHiddenField`, `HiddenField:productidHiddenField` | — |
| 2 | TemplateField | Product Name | — | `ProductName` | `TextBox:productNameTextBox` | — |
| 3 | TemplateField | PackSize | — | `PackSize` | `TextBox:packSizeTextBox` | — |
| 4 | TemplateField | Req. Qty | — | `Quantity` | `TextBox:reqQtyTextBox` | — |
| 5 | TemplateField | VAT | — | `VAT` | `TextBox:vatTextBox` | — |
| 6 | TemplateField | TP | — | `UnitPrice` | `TextBox:tpTextBox` | — |
| 7 | TemplateField | TPGross | — | `GrossValue` | `TextBox:grossTextBox` | — |
| 8 | TemplateField | Add | — | — | `ImageButton:ImageButton1` | — |
| 9 | TemplateField | Remove | — | — | `ImageButton:ImageButton2` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDownList` | `aOrderCreationDal.GetDdlCompany(...)` | `CampaignDAL.GetDdlCompany(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/CampaignDAL.cs` | `CampaignDAL.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `ManualOrderCreationDal.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `ReturnAcknowledgmentNoteDal.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyInfo` | — |
| `LoadDropDownList` | `aOrderCreationDal.GetDdlRegion(...)` | `ManualOrderCreationDal.GetDdlRegion(DropDownList ddl)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDdlRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `ReturnAcknowledgmentNoteDal.GetDdlRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblRegion` | — |
| `custCodeTextBox_TextChanged` | `aOrderCreationDal.GetCustomerCreditLimit(...)` | `ManualOrderCreationDal.GetCustomerCreditLimit(string customerId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetCustomerCreditLimit` → `OrderListDAL.GetCustomerCreditLimit` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerCreditLimit` → `OrderListBLL.GetCustomerCreditLimit` | SELECT | `tblCustMaster`, `tblCustomerCreditLimit` | — |
| `custCodeTextBox_TextChanged` | `aOrderCreationDal.GetCustomerDueInfo(...)` | `ManualOrderCreationDal.GetCustomerDueInfo(string customerId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetCustomerDueInfo` → `SampleIssueDal.GetCustomerDueInfo` | SELECT | `tblCustPayDetail`, `tblInvoice` | — |
| `custCodeTextBox_TextChanged` | `aOrderCreationDal.GetCustomerInfoByCode(...)` | `BatchWiseCollectionReportDal.GetCustomerInfoByCode(string customerCode)` <br/>`Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` | `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `FixedDiscountEntryDal.GetCustomerInfoByCode` → `ManualOrderCreationDal.GetCustomerInfoByCode` | SELECT | `tblArea`, `tblCustMaster`, `tblInvoice`, `tblMIOInfo`, `tblMarket`, `tblRegion`, `tblTerritory` | — |
| `custCodeTextBox_TextChanged` | `aOrderCreationDal.GetCustomerUsedCreditLimit(...)` | `ManualOrderCreationDal.GetCustomerUsedCreditLimit(string customerId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetCustomerUsedCreditLimit` → `OrderListDAL.GetCustomerUsedCreditLimit` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerUsedCreditLimit` → `OrderListBLL.GetCustomerUsedCreditLimit` | SELECT | `tblCustPayDetail`, `tblInvoice`, `tblOrder` | — |
| `custCodeTextBox_TextChanged` | `aOrderCreationDal.GetIsVatApplicable(...)` | `ManualOrderCreationDal.GetIsVatApplicable(string customerId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetIsVatApplicable` | SELECT | `tblCustMaster` | — |
| `SetCusromerDetailInfo` | `aOrderCreationDal.GetCustomerInfo(...)` | `CustPaymentDAL.GetCustomerInfo(string custId)` <br/>`Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | `CustPaymentDAL.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `ManualOrderCreationDal.GetCustomerInfo` → `ProductDiscountDAL.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerInfo` → `WebServiceDal.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblArea`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — |
| `SetCusromerDetailInfo` | `aOrderCreationDal.GetDdlArea(...)` | `ManualOrderCreationDal.GetDdlArea(DropDownList ddl, string regionId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDdlArea` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlArea` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea` | — |
| `SetCusromerDetailInfo` | `aOrderCreationDal.GetDdlDc(...)` | `ManualOrderCreationDal.GetDdlDc(DropDownList ddl, string areaId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDdlDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit`, `tblDcWiseAreaInfo` | — |
| `SetCusromerDetailInfo` | `aOrderCreationDal.GetDdlDcByUser(...)` | `ManualOrderCreationDal.GetDdlDcByUser(DropDownList ddl, string areaId, string userId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblUserCompanyUnit` | — |
| `SetCusromerDetailInfo` | `aOrderCreationDal.GetDdlMarket(...)` | `ManualOrderCreationDal.GetDdlMarket(DropDownList ddl, string territoriId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDdlMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblMarket` | — |
| `SetCusromerDetailInfo` | `aOrderCreationDal.GetDdlMio(...)` | `ManualOrderCreationDal.GetDdlMio(DropDownList ddl, string territoriId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDdlMio` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlMio` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblEmpGeneralInfo`, `tblMIOInfo` | — |
| `SetCusromerDetailInfo` | `aOrderCreationDal.GetDdlTeritory(...)` | `ManualOrderCreationDal.GetDdlTeritory(DropDownList ddl, string areaId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDdlTeritory` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlTeritory` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblTerritory` | — |
| `OrdNo` | `aOrderCreationDal.OrderManualId(...)` | `ManualOrderCreationDal.OrderManualId()` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.OrderManualId` → `OrderListDAL.OrderManualId` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.OrderManualId` | SELECT | `tblOrder` | — |
| `dcDropDownList_SelectedIndexChanged` | `aOrderCreationDal.GetCompanyByDc(...)` | `ManualOrderCreationDal.GetCompanyByDc(string unitId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetCompanyByDc` → `OrderListDAL.GetCompanybydc` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCompanyByDc` → `OrderListBLL.GetCompanybydc` | SELECT | `tblCompanyUnit` | — |
| `dcDropDownList_SelectedIndexChanged` | `aOrderCreationDal.GetMioByCompany(...)` | `ManualOrderCreationDal.GetMioByCompany(string companyId, string territoryId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetMioByCompany` → `OrderListDAL.GetMioByCompany` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetMioByCompany` | SELECT | `tblMIOInfo` | — |
| `submitButton_Click` | `aOrderCreationDal.DeleteOrderMasterInfo(...)` | `ManualOrderCreationDal.DeleteOrderMasterInfo(int masterId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.DeleteOrderMasterInfo` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` → `SampleIssueDal.DeleteOrderMasterInfo` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` | DELETE | `tblOrder` | — |
| `submitButton_Click` | `aOrderCreationDal.GetOrderCode(...)` | `ManualOrderCreationDal.GetOrderCode(int masterId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetOrderCode` | SELECT | `tblOrder` | — |
| `submitButton_Click` | `aOrderCreationDal.SaveOrderInformation(...)` | `ManualOrderCreationDal.SaveOrderInformation(OrderInfoMaster aListMasterDao)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.SaveOrderInformation` → `ClsCommonInternalDAL.SaveAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.SaveAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsCommonInternalDAL.SaveDataByInsertCommandById` → `SampleIssueDal.SaveOrderInformation` → `ClsCommonInternalDAL.SaveAction` | INSERT | `tblOrder` | `sp_I_OrderMaster`, `sp_I_SampleProductIssue` |
| `CheckDataSavedOrNot` | `aOrderCreationDal.GetDataSaveStatus(...)` | `ManualOrderCreationDal.GetDataSaveStatus(int masterId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetDataSaveStatus` → `SampleIssueDal.GetDataSaveStatus` | SELECT | `tblOrderDetail` | — |
| `SaveDetailInformation` | `aOrderCreationDal.SaveOrderDetailInfo(...)` | `ManualOrderCreationDal.SaveOrderDetailInfo(OrderInfoDetail aListMasterDao)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.SaveOrderDetailInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommandById` → `SampleIssueDal.SaveOrderDetailInfo` → `ClsCommonInternalDAL.SaveAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.SaveAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsCommonInternalDAL.SaveDataByInsertCommandById` | INSERT | `tblOrderDetail`, `tblSampleIssueDetail` | `sp_I_SampleProductIssueDetail` |
| `PrepareOrderData` | `aOrderCreationDal.GetAsmInfo(...)` | `ManualOrderCreationDal.GetAsmInfo(string areeaId, string companyId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetAsmInfo` → `SampleIssueDal.GetAsmInfo` | SELECT | `tblASMInfo` | — |
| `PrepareOrderData` | `aOrderCreationDal.GetRsmInfo(...)` | `ManualOrderCreationDal.GetRsmInfo(string areeaId, string companyId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetRsmInfo` → `SampleIssueDal.GetRsmInfo` | SELECT | `tblRSMInfo` | — |
| `Validation` | `aOrderCreationDal.GetAsmInfo(...)` | `ManualOrderCreationDal.GetAsmInfo(string areeaId, string companyId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetAsmInfo` → `SampleIssueDal.GetAsmInfo` | SELECT | `tblASMInfo` | — |
| `Validation` | `aOrderCreationDal.GetRsmInfo(...)` | `ManualOrderCreationDal.GetRsmInfo(string areeaId, string companyId)` <br/>`Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | `ManualOrderCreationDal.GetRsmInfo` → `SampleIssueDal.GetRsmInfo` | SELECT | `tblRSMInfo` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `dcDropDownList` | Distribution Center: | asp:DropDownList | True | dcDropDownList_SelectedIndexChanged | `aOrderCreationDal.GetCompanyByDc()` in `dcDropDownList_SelectedIndexChanged()`; `aOrderCreationDal.GetDdlDc()` in `SetCusromerDetailInfo()`; `aOrderCreationDal.GetDdlDcByUser()` in `SetCusromerDetailInfo()` | `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `regionDropDownList` | Region: | asp:DropDownList | True | — | `aOrderCreationDal.GetDdlArea()` in `SetCusromerDetailInfo()`; `aOrderCreationDal.GetDdlRegion()` in `LoadDropDownList()`; `aOrderCreationDal.GetRsmInfo()` in `PrepareOrderData()`; `aOrderCreationDal.GetRsmInfo()` in `Validation()` | `tblArea`, `tblRSMInfo`, `tblRegion` | set in code (see the DAL method above) |
| `areaDropDownList` | Area: | asp:DropDownList | True | — | `aOrderCreationDal.GetAsmInfo()` in `PrepareOrderData()`; `aOrderCreationDal.GetAsmInfo()` in `Validation()`; `aOrderCreationDal.GetDdlArea()` in `SetCusromerDetailInfo()`; `aOrderCreationDal.GetDdlDc()` in `SetCusromerDetailInfo()` | `tblASMInfo`, `tblArea`, `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblTerritory`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `companyDropDownList` | Company: | asp:DropDownList | True | — | `_aDcStockReceiveBll.ProductInfo()` in `GetProductInGrid()`; `aOrderCreationDal.GetAsmInfo()` in `PrepareOrderData()`; `aOrderCreationDal.GetAsmInfo()` in `Validation()`; `aOrderCreationDal.GetDdlCompany()` in `LoadDropDownList()` | `View_DCStoreCurrentStock`, `tblASMInfo`, `tblCompanyInfo`, `tblMIOInfo`, `tblManufacturer`, `tblPackSize`, `tblProCategory`, `tblProType`, `tblProduct`, `tblProductCase`, `tblProductSQ`, `tblRSMInfo`, `tblStockUOM`, `tblUnitPrice` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory: | asp:DropDownList | True | — | `aOrderCreationDal.GetDdlMarket()` in `SetCusromerDetailInfo()`; `aOrderCreationDal.GetDdlMio()` in `SetCusromerDetailInfo()`; `aOrderCreationDal.GetDdlTeritory()` in `SetCusromerDetailInfo()`; `aOrderCreationDal.GetMioByCompany()` in `dcDropDownList_SelectedIndexChanged()` | `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | set in code (see the DAL method above) |
| `marketDropDownList` | Market: | asp:DropDownList | True | — | `aOrderCreationDal.GetDdlMarket()` in `SetCusromerDetailInfo()` | `tblMarket` | set in code (see the DAL method above) |
| `mioDropDownList` | MIO: | asp:DropDownList | True | — | `aOrderCreationDal.GetDdlMio()` in `SetCusromerDetailInfo()` | `tblEmpGeneralInfo`, `tblMIOInfo` | set in code (see the DAL method above) |

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
| `ImageButton1` | asp:ImageButton |  | — | `ImageButton1_Click` | 4 lines | — | — |
| `ImageButton2` | asp:ImageButton |  | — | `ImageButton2_Click` | 88 lines | — | — |
| `submitButton` | asp:Button | Submit | — | `submitButton_Click` | 27 lines | `aOrderCreationDal.DeleteOrderMasterInfo`, `aOrderCreationDal.GetOrderCode`, `aOrderCreationDal.SaveOrderInformation` | — |
| `ImageButton1` | asp:ImageButton |  | — | `—` | — | — | — |
| `yesButton` | asp:Button | Proceed next | — | `yesButton_Click` | 6 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:AutoCompleteExtender`(custCodeTextBox), `ajaxToolkit:AutoCompleteExtender`(productNameTextBox), `ajaxToolkit:FilteredTextBoxExtender`(reqQtyTextBox), `ajaxToolkit:ModalPopupExtender`(HiddenField10)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 10 | — | — | — | — |
| 2 | `SetOrderDate` | `void SetOrderDate()` | 3 | — | — | — | — |
| 3 | `InitialGrid` | `void InitialGrid()` | 41 | — | — | — | — |
| 4 | `LoadDropDownList` | `void LoadDropDownList()` | 4 | `aOrderCreationDal.GetDdlCompany`, `aOrderCreationDal.GetDdlRegion` | — | — | — |
| 5 | `custCodeTextBox_TextChanged` | `void custCodeTextBox_TextChanged(object sender, EventArgs e)` | 117 | `aOrderCreationDal.GetCustomerCreditLimit`, `aOrderCreationDal.GetCustomerDueInfo`, `aOrderCreationDal.GetCustomerInfoByCode`, `aOrderCreationDal.GetCustomerUsedCreditLimit`, `aOrderCreationDal.GetIsVatApplicable` | — | — | — |
| 6 | `SetDueStatus` | `void SetDueStatus()` | 14 | — | — | — | — |
| 7 | `SetCusromerDetailInfo` | `void SetCusromerDetailInfo()` | 95 | `aOrderCreationDal.GetCustomerInfo`, `aOrderCreationDal.GetDdlArea`, `aOrderCreationDal.GetDdlDc`, `aOrderCreationDal.GetDdlDcByUser`, `aOrderCreationDal.GetDdlMarket`, `aOrderCreationDal.GetDdlMio` | — | — | `UserId`, `UserType` |
| 8 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 9 | `Clear` | `void Clear()` | 41 | — | — | — | — |
| 10 | `SetOderNo` | `void SetOderNo()` | 4 | — | — | — | — |
| 11 | `OrdNo` | `string OrdNo()` | 7 | `aOrderCreationDal.OrderManualId` | — | — | — |
| 12 | `OrdnNoGenerator` | `string OrdnNoGenerator(int id)` | 32 | — | — | — | — |
| 13 | `dcDropDownList_SelectedIndexChanged` | `void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 52 | `aOrderCreationDal.GetCompanyByDc`, `aOrderCreationDal.GetMioByCompany` | — | — | — |
| 14 | `productNameTextBox_TextChanged` | `void productNameTextBox_TextChanged(object sender, EventArgs e)` | 27 | — | — | — | — |
| 15 | `GetProductInGrid` | `void GetProductInGrid(int rowindex, string productCode)` | 77 | — | — | — | — |
| 16 | `ClearRow` | `void ClearRow(int rowindex)` | 26 | — | — | — | — |
| 17 | `ProductExistOrNot` | `bool ProductExistOrNot(string productCode, int rowindex)` | 13 | — | — | — | — |
| 18 | `productCodeTextBox_TextChanged` | `void productCodeTextBox_TextChanged(object sender, EventArgs e)` | 13 | — | — | — | — |
| 19 | `reqQtyTextBox_OnTextChanged` | `void reqQtyTextBox_OnTextChanged(object sender, EventArgs e)` | 41 | — | — | — | — |
| 20 | `CalculateGross` | `void CalculateGross()` | 10 | — | — | — | — |
| 21 | `IncreasetakenLimit` | `void IncreasetakenLimit()` | 22 | — | — | — | — |
| 22 | `CalculateRowGross` | `void CalculateRowGross()` | 15 | — | — | — | — |
| 23 | `ImageButton1_Click` | `void ImageButton1_Click(object sender, ImageClickEventArgs e)` | 4 | — | — | — | — |
| 24 | `AddRowInGrid` | `void AddRowInGrid()` | 93 | — | — | — | — |
| 25 | `ImageButton2_Click` | `void ImageButton2_Click(object sender, ImageClickEventArgs e)` | 88 | — | — | — | — |
| 26 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 27 | `aOrderCreationDal.DeleteOrderMasterInfo`, `aOrderCreationDal.GetOrderCode`, `aOrderCreationDal.SaveOrderInformation` | — | — | — |
| 27 | `CheckDataSavedOrNot` | `bool CheckDataSavedOrNot(int masterId)` | 10 | `aOrderCreationDal.GetDataSaveStatus` | — | — | — |
| 28 | `SaveDetailInformation` | `int SaveDetailInformation(List<OrderInfoDetail> aOrderInfoDetails)` | 10 | `aOrderCreationDal.SaveOrderDetailInfo` | — | — | — |
| 29 | `PrepareOrderDetailData` | `List<OrderInfoDetail> PrepareOrderDetailData(int masterId)` | 28 | — | — | — | — |
| 30 | `PrepareOrderData` | `OrderInfoMaster PrepareOrderData()` | 65 | `aOrderCreationDal.GetAsmInfo`, `aOrderCreationDal.GetRsmInfo` | — | — | `LoginName` |
| 31 | `Validation` | `bool Validation()` | 124 | `aOrderCreationDal.GetAsmInfo`, `aOrderCreationDal.GetRsmInfo` | — | — | — |
| 32 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 33 | `yesButton_Click` | `void yesButton_Click(object sender, EventArgs e)` | 6 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ManualOrderrCreation.aspx (markup)
  -> SInventory_UI_ManualOrderrCreation (code-behind event handler)
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
| `productNameTextBox_AutoCompleteExtender` | `productNameTextBox` | `GetProductByMenufracturer` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `custCodeTextBox_TextChanged` | `OrderListBLL` | `GetCustomerCreditLimit` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `custCodeTextBox_TextChanged` | `CustPaymentBLL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `custCodeTextBox_TextChanged` | `OrderListBLL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `custCodeTextBox_TextChanged` | `OrderListBLL` | `GetCustomerUsedCreditLimit` | `string customerId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| `SetCusromerDetailInfo` | `ProductDiscountBLL` | `GetCustomerInfo` | `string custId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `dcDropDownList_SelectedIndexChanged` | `OrderListBLL` | `GetCompanybydc` | `string id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropDownList` | `CampaignDAL` | `GetDdlCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| `LoadDropDownList` | `ManualOrderCreationDal` | `GetDdlCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `LoadDropDownList` | `ReturnAcknowledgmentNoteDal` | `GetDdlCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs` |
| `LoadDropDownList` | `SampleIssueDal` | `GetDdlCompany` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `LoadDropDownList` | `ManualOrderCreationDal` | `GetDdlRegion` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `LoadDropDownList` | `ReturnAcknowledgmentNoteDal` | `GetDdlRegion` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs` |
| `LoadDropDownList` | `SampleIssueDal` | `GetDdlRegion` | `DropDownList ddl` | `void` | SELECT | `tblRegion` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `custCodeTextBox_TextChanged` | `ManualOrderCreationDal` | `GetCustomerCreditLimit` | `string customerId` | `DataTable` | SELECT | `tblCustMaster`, `tblCustomerCreditLimit` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `custCodeTextBox_TextChanged` | `OrderListDAL` | `GetCustomerCreditLimit` | `string customerId` | `DataTable` | SELECT | `tblCustMaster`, `tblCustomerCreditLimit` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `custCodeTextBox_TextChanged` | `SampleIssueDal` | `GetCustomerCreditLimit` | `string customerId` | `DataTable` | SELECT | `tblCustMaster`, `tblCustomerCreditLimit` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `custCodeTextBox_TextChanged` | `ManualOrderCreationDal` | `GetCustomerDueInfo` | `string customerId` | `DataTable` | SELECT | `tblCustPayDetail`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `custCodeTextBox_TextChanged` | `SampleIssueDal` | `GetCustomerDueInfo` | `string customerId` | `DataTable` | SELECT | `tblInvoice` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `custCodeTextBox_TextChanged` | `BatchWiseCollectionReportDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| `custCodeTextBox_TextChanged` | `CustomerFixedDiscountDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `custCodeTextBox_TextChanged` | `CustPaymentDAL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `custCodeTextBox_TextChanged` | `FixedDiscountEntryDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `custCodeTextBox_TextChanged` | `ManualOrderCreationDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `custCodeTextBox_TextChanged` | `OrderListDAL` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `custCodeTextBox_TextChanged` | `SampleIssueDal` | `GetCustomerInfoByCode` | `string customerCode` | `DataTable` | SELECT | `tblCustMaster`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `custCodeTextBox_TextChanged` | `WebServiceDal` | `GetCustomerInfoByCode` | `string prefixText` | `DataTable` | SELECT | `tblArea`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |
| `custCodeTextBox_TextChanged` | `ManualOrderCreationDal` | `GetCustomerUsedCreditLimit` | `string customerId` | `DataTable` | SELECT | `tblCustPayDetail`, `tblInvoice`, `tblOrder` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `custCodeTextBox_TextChanged` | `OrderListDAL` | `GetCustomerUsedCreditLimit` | `string customerId` | `DataTable` | SELECT | `tblCustPayDetail`, `tblInvoice`, `tblOrder` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `custCodeTextBox_TextChanged` | `SampleIssueDal` | `GetCustomerUsedCreditLimit` | `string customerId` | `DataTable` | SELECT | `tblCustPayDetail`, `tblInvoice`, `tblOrder` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `custCodeTextBox_TextChanged` | `ManualOrderCreationDal` | `GetIsVatApplicable` | `string customerId` | `DataTable` | SELECT | `tblCustMaster` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `CustPaymentDAL` | `GetCustomerInfo` | `string custId` | `DataTable` | SELECT | `tblCustMaster` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `SetCusromerDetailInfo` | `ManualOrderCreationDal` | `GetCustomerInfo` | `string customerCode` | `DataTable` | SELECT | `tblArea`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `ProductDiscountDAL` | `GetCustomerInfo` | `string custId` | `DataTable` | SELECT | `tblCustMaster` | — | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| `SetCusromerDetailInfo` | `SampleIssueDal` | `GetCustomerInfo` | `string customerCode` | `DataTable` | SELECT | `tblArea`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SetCusromerDetailInfo` | `WebServiceDal` | `GetCustomerInfo` | `string prefixText` | `DataTable` | SELECT | `tblArea`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |
| `SetCusromerDetailInfo` | `ManualOrderCreationDal` | `GetDdlArea` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `SampleIssueDal` | `GetDdlArea` | `DropDownList ddl, string regionId` | `void` | SELECT | `tblArea` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SetCusromerDetailInfo` | `ManualOrderCreationDal` | `GetDdlDc` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblCompanyUnit`, `tblDcWiseAreaInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `SampleIssueDal` | `GetDdlDc` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblCompanyUnit`, `tblDcWiseAreaInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SetCusromerDetailInfo` | `ManualOrderCreationDal` | `GetDdlDcByUser` | `DropDownList ddl, string areaId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `SampleIssueDal` | `GetDdlDcByUser` | `DropDownList ddl, string areaId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SetCusromerDetailInfo` | `ManualOrderCreationDal` | `GetDdlMarket` | `DropDownList ddl, string territoriId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `SampleIssueDal` | `GetDdlMarket` | `DropDownList ddl, string territoriId` | `void` | SELECT | `tblMarket` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SetCusromerDetailInfo` | `ManualOrderCreationDal` | `GetDdlMio` | `DropDownList ddl, string territoriId` | `void` | SELECT | `tblEmpGeneralInfo`, `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `SampleIssueDal` | `GetDdlMio` | `DropDownList ddl, string territoriId` | `void` | SELECT | `tblEmpGeneralInfo`, `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SetCusromerDetailInfo` | `ManualOrderCreationDal` | `GetDdlTeritory` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SetCusromerDetailInfo` | `SampleIssueDal` | `GetDdlTeritory` | `DropDownList ddl, string areaId` | `void` | SELECT | `tblTerritory` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `OrdNo` | `ManualOrderCreationDal` | `OrderManualId` | `` | `int` | SELECT | `tblOrder` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `OrdNo` | `OrderListDAL` | `OrderManualId` | `` | `int` | SELECT | `tblOrder` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `OrdNo` | `SampleIssueDal` | `OrderManualId` | `` | `int` | SELECT | `tblOrder` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `dcDropDownList_SelectedIndexChanged` | `ManualOrderCreationDal` | `GetCompanyByDc` | `string unitId` | `DataTable` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `dcDropDownList_SelectedIndexChanged` | `OrderListDAL` | `GetCompanybydc` | `string id` | `DataTable` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `dcDropDownList_SelectedIndexChanged` | `SampleIssueDal` | `GetCompanyByDc` | `string unitId` | `DataTable` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `dcDropDownList_SelectedIndexChanged` | `ManualOrderCreationDal` | `GetMioByCompany` | `string companyId, string territoryId` | `DataTable` | SELECT | `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `dcDropDownList_SelectedIndexChanged` | `OrderListDAL` | `GetMioByCompany` | `string terittoryId, string companyId` | `DataTable` | SELECT | `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| `dcDropDownList_SelectedIndexChanged` | `SampleIssueDal` | `GetMioByCompany` | `string companyId, string territoryId` | `DataTable` | SELECT | `tblMIOInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `submitButton_Click` | `ManualOrderCreationDal` | `DeleteOrderMasterInfo` | `int masterId` | `bool` | DELETE | `tblOrder` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `submitButton_Click` | `SampleIssueDal` | `DeleteOrderMasterInfo` | `int masterId` | `bool` | DELETE | `tblOrder` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `submitButton_Click` | `ManualOrderCreationDal` | `GetOrderCode` | `int masterId` | `DataTable` | SELECT | `tblOrder` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `submitButton_Click` | `ManualOrderCreationDal` | `SaveOrderInformation` | `OrderInfoMaster aListMasterDao` | `int` | INSERT | `tblOrder` | `sp_I_OrderMaster` | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `submitButton_Click` | `SampleIssueDal` | `SaveOrderInformation` | `OrderInfoMaster aListMasterDao` | `int` | INSERT | `tblOrder` | `sp_I_SampleProductIssue` | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `CheckDataSavedOrNot` | `ManualOrderCreationDal` | `GetDataSaveStatus` | `int masterId` | `DataTable` | SELECT | `tblOrderDetail` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `CheckDataSavedOrNot` | `SampleIssueDal` | `GetDataSaveStatus` | `int masterId` | `DataTable` | SELECT | `tblOrderDetail` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `SaveDetailInformation` | `ManualOrderCreationDal` | `SaveOrderDetailInfo` | `OrderInfoDetail aListMasterDao` | `int` | INSERT | `tblOrderDetail` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `SaveDetailInformation` | `SampleIssueDal` | `SaveOrderDetailInfo` | `OrderInfoDetail aListMasterDao` | `int` | INSERT | `tblSampleIssueDetail` | `sp_I_SampleProductIssueDetail` | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `PrepareOrderData` | `ManualOrderCreationDal` | `GetAsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblASMInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `PrepareOrderData` | `SampleIssueDal` | `GetAsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblASMInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `PrepareOrderData` | `ManualOrderCreationDal` | `GetRsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblRSMInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `PrepareOrderData` | `SampleIssueDal` | `GetRsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblRSMInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `Validation` | `ManualOrderCreationDal` | `GetAsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblASMInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `Validation` | `SampleIssueDal` | `GetAsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblASMInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| `Validation` | `ManualOrderCreationDal` | `GetRsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblRSMInfo` | — | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| `Validation` | `SampleIssueDal` | `GetRsmInfo` | `string areeaId, string companyId` | `DataTable` | SELECT | `tblRSMInfo` | — | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_I_OrderMaster` | submitButton_Click → `ManualOrderCreationDal.SaveOrderInformation → ClsCommonInternalDAL.SaveAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.SaveAction → DB_Manager.ExecuteNonQueryAction → ClsCommonInternalDAL.SaveDataByInsertCommandById` | **No** — see note |
| `sp_I_SampleProductIssue` | submitButton_Click → `ManualOrderCreationDal.SaveOrderInformation → ClsCommonInternalDAL.SaveAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.SaveAction → DB_Manager.ExecuteNonQueryAction → ClsCommonInternalDAL.SaveDataByInsertCommandById` | **No** — see note |
| `sp_I_SampleProductIssueDetail` | SaveDetailInformation → `ManualOrderCreationDal.SaveOrderDetailInfo → ClsCommonInternalDAL.SaveDataByInsertCommandById → SampleIssueDal.SaveOrderDetailInfo → ClsCommonInternalDAL.SaveAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.SaveAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblASMInfo` | `PrepareOrderData()` → `ManualOrderCreationDal.GetAsmInfo` → `SampleIssueDal.GetAsmInfo`, `Validation()` → `ManualOrderCreationDal.GetAsmInfo` → `SampleIssueDal.GetAsmInfo` |
| `tblArea` | `SetCusromerDetailInfo()` → `CustPaymentDAL.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `ManualOrderCreationDal.GetCustomerInfo` → `ProductDiscountDAL.GetCustomerInfo`, `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlArea` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlArea` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `custCodeTextBox_TextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `LoadDropDownList()` → `CampaignDAL.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `ManualOrderCreationDal.GetDdlCompany` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyUnit` | `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `dcDropDownList_SelectedIndexChanged()` → `ManualOrderCreationDal.GetCompanyByDc` → `OrderListDAL.GetCompanybydc` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCompanyByDc` |
| `tblCustMaster` | `SetCusromerDetailInfo()` → `CustPaymentDAL.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `ManualOrderCreationDal.GetCustomerInfo` → `ProductDiscountDAL.GetCustomerInfo`, `custCodeTextBox_TextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable`, `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetCustomerCreditLimit` → `OrderListDAL.GetCustomerCreditLimit` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerCreditLimit`, `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetIsVatApplicable` |
| `tblCustPayDetail` | `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetCustomerDueInfo` → `SampleIssueDal.GetCustomerDueInfo`, `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetCustomerUsedCreditLimit` → `OrderListDAL.GetCustomerUsedCreditLimit` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerUsedCreditLimit` |
| `tblCustomerCreditLimit` | `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetCustomerCreditLimit` → `OrderListDAL.GetCustomerCreditLimit` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerCreditLimit` |
| `tblDcWiseAreaInfo` | `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblEmpGeneralInfo` | `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlMio` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlMio` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblInvoice` | `custCodeTextBox_TextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable`, `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetCustomerDueInfo` → `SampleIssueDal.GetCustomerDueInfo`, `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetCustomerUsedCreditLimit` → `OrderListDAL.GetCustomerUsedCreditLimit` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerUsedCreditLimit` |
| `tblMIOInfo` | `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlMio` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlMio` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `custCodeTextBox_TextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable`, `dcDropDownList_SelectedIndexChanged()` → `ManualOrderCreationDal.GetMioByCompany` → `OrderListDAL.GetMioByCompany` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetMioByCompany` |
| `tblMarket` | `SetCusromerDetailInfo()` → `CustPaymentDAL.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `ManualOrderCreationDal.GetCustomerInfo` → `ProductDiscountDAL.GetCustomerInfo`, `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `custCodeTextBox_TextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblOrder` | `OrdNo()` → `ManualOrderCreationDal.OrderManualId` → `OrderListDAL.OrderManualId` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.OrderManualId`, `custCodeTextBox_TextChanged()` → `ManualOrderCreationDal.GetCustomerUsedCreditLimit` → `OrderListDAL.GetCustomerUsedCreditLimit` → `ClsCommonInternalDAL.DataContainerDataTable` → `SampleIssueDal.GetCustomerUsedCreditLimit`, `submitButton_Click()` → `ManualOrderCreationDal.DeleteOrderMasterInfo` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand` → `SampleIssueDal.DeleteOrderMasterInfo` → `ClsCommonInternalDAL.DeleteDataByDeleteCommand`, `submitButton_Click()` → `ManualOrderCreationDal.GetOrderCode`, `submitButton_Click()` → `ManualOrderCreationDal.SaveOrderInformation` → `ClsCommonInternalDAL.SaveAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.SaveAction` |
| `tblOrderDetail` | `CheckDataSavedOrNot()` → `ManualOrderCreationDal.GetDataSaveStatus` → `SampleIssueDal.GetDataSaveStatus`, `SaveDetailInformation()` → `ManualOrderCreationDal.SaveOrderDetailInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommandById` → `SampleIssueDal.SaveOrderDetailInfo` → `ClsCommonInternalDAL.SaveAction` |
| `tblRSMInfo` | `PrepareOrderData()` → `ManualOrderCreationDal.GetRsmInfo` → `SampleIssueDal.GetRsmInfo`, `Validation()` → `ManualOrderCreationDal.GetRsmInfo` → `SampleIssueDal.GetRsmInfo` |
| `tblRegion` | `LoadDropDownList()` → `ManualOrderCreationDal.GetDdlRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `ReturnAcknowledgmentNoteDal.GetDdlRegion` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `SetCusromerDetailInfo()` → `CustPaymentDAL.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `ManualOrderCreationDal.GetCustomerInfo` → `ProductDiscountDAL.GetCustomerInfo`, `custCodeTextBox_TextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblSampleIssueDetail` | `SaveDetailInformation()` → `ManualOrderCreationDal.SaveOrderDetailInfo` → `ClsCommonInternalDAL.SaveDataByInsertCommandById` → `SampleIssueDal.SaveOrderDetailInfo` → `ClsCommonInternalDAL.SaveAction` |
| `tblTerritory` | `SetCusromerDetailInfo()` → `CustPaymentDAL.GetCustomerInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `ManualOrderCreationDal.GetCustomerInfo` → `ProductDiscountDAL.GetCustomerInfo`, `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlTeritory` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlTeritory` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `custCodeTextBox_TextChanged()` → `BatchWiseCollectionReportDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerFixedDiscountDal.GetCustomerInfoByCode` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUserCompanyUnit` | `SetCusromerDetailInfo()` → `ManualOrderCreationDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `SampleIssueDal.GetDdlDcByUser` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `custCodeTextBox_TextChanged` | 0.00 | No | Yes | UNKNOWN | Notification |
| `ShowMessageBox` | {0} | No | Yes | UNKNOWN | Notification |
| `SetOderNo` | 0001 | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `freqQtyTextBox` | `reqQtyTextBox` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-SAL-014-001` | 0.00 | Code | `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx.cs` → `custCodeTextBox_TextChanged()` | user action on this page | operation is refused | "0.00" | CONFIRMED |
| `BR-SAL-014-002` | {0} | Code | `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |
| `BR-SAL-014-003` | 0001 | Code | `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx.cs` → `SetOderNo()` | user action on this page | operation is refused | "0001" | CONFIRMED |

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
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |
| Database object | `tblASMInfo` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerCreditLimit` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblInvoice` |
| Database object | `tblMIOInfo` |
| Database object | `tblMarket` |
| Database object | `tblOrder` |
| Database object | `tblOrderDetail` |
| Database object | `tblRSMInfo` |
| Database object | `tblRegion` |
| Database object | `tblSampleIssueDetail` |
| Database object | `tblTerritory` |
| Database object | `tblUserCompanyUnit` |
| Stored procedure | `sp_I_OrderMaster` |
| Stored procedure | `sp_I_SampleProductIssue` |
| Stored procedure | `sp_I_SampleProductIssueDetail` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-014-001` | Unauthenticated access | Request `/SInventory_UI/ManualOrderrCreation.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-014-002` | Validation: 0.00 | Trigger `custCodeTextBox_TextChanged()` in the state that produces this message | alert "0.00" and the operation is not performed | CONFIRMED |
| `TC-SAL-014-003` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-SAL-014-004` | Validation: 0001 | Trigger `SetOderNo()` in the state that produces this message | alert "0001" and the operation is not performed | CONFIRMED |
| `TC-SAL-014-005` | Grid loads | Open the page with data present | Grid `productGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OrderListBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CampaignDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderListDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/WebServiceDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 67 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 33 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 3 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 19 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OrderListBLL.cs` is **shared with 20 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` is **shared with 14 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DiscountApproval.aspx`, `DistributionCenterInfoView.aspx`, `NonTranscationalInvoiceReturn.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` is **shared with 23 other page(s)** (e.g. `AITReceiptViewer.aspx`, `BatchWiseCollectionReportViewer.aspx`, `BatchWiseCollectionSheetReportViewer.aspx`, `BatchWiseCollectionViewer.aspx`, `BatchWiseMoneyReceiptReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CampaignDAL.cs` is **shared with 6 other page(s)** (e.g. `BonusCampaignSetup.aspx`, `CampaignSetup.aspx`, `CampaignView.aspx`, `NonTranscationalInvoiceReturn.aspx`, `ReturnAcknowledgmentNote.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` is **shared with 58 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` is **shared with 59 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` is **shared with 14 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OrderListDAL.cs` is **shared with 17 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` is **shared with 8 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DistributionCenterInfoView.aspx`, `NonTranscationalInvoiceReturn.aspx`, `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs` is **shared with 3 other page(s)** (e.g. `NonTranscationalInvoiceReturn.aspx`, `ReturnAcknowledgmentNote.aspx`, `SampleProductrIssue.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SampleIssueDal.cs` is **shared with 17 other page(s)** (e.g. `SampleIssueReportViewer.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WebServiceDal.cs` is **shared with 14 other page(s)** (e.g. `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`, `CustomerFixedDiscount.aspx`, `CustomerWiseLedgerReport.aspx`) — a change there is not local.
- Writes/reads `tblASMInfo` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerCreditLimit` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblOrder` — any schema change to it breaks this page.
- Writes/reads `tblOrderDetail` — any schema change to it breaks this page.
- Writes/reads `tblRSMInfo` — any schema change to it breaks this page.
- Writes/reads `tblRegion` — any schema change to it breaks this page.
- Writes/reads `tblSampleIssueDetail` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
