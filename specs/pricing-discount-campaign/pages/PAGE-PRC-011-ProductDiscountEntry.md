# PRC-011 — ProductDiscountEntry

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `PRC-011` |
| Module | `pricing-discount-campaign` |
| File name | `ProductDiscountEntry.aspx` |
| Route | `/SInventory_UI/ProductDiscountEntry.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ProductDiscountEntry.aspx.cs` |
| Code-behind class | `SInventory_UI_ProductDiscountEntry` |
| Markup lines | 532 |
| Code-behind lines | 349 |
| Server controls | 33 |
| GridViews | 0 |
| Code-behind methods | 26 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/ProductDiscountView.aspx`

Outbound `Response.Redirect` targets: `ProductDiscountView.aspx`

## 5. Route

`/SInventory_UI/ProductDiscountEntry.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductDiscountEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductDiscountEntry.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |

## 7. UI Control Inventory

33 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 48 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 49 |
| 4 | `ListImageButton` | asp:ImageButton | View Details | ~/images/viewList.png |  |  |  | ListImageButton_Click | 83 |
| 5 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 89 |
| 6 | `manufacturerDropDownList` | asp:DropDownList | Manufacturer: |  | True |  |  | manufacturerDropDownList_SelectedIndexChanged | 121 |
| 7 | `dcDropDownList` | asp:DropDownList | Distribution Center : |  | True |  |  | dcDropDownList_SelectedIndexChanged | 129 |
| 8 | `areaDropDownList` | asp:DropDownList | Area: |  | True |  |  | areaDropDownList_OnSelectedIndexChanged | 137 |
| 9 | `discountIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 144 |
| 10 | `mioDropDownList` | asp:DropDownList | MIO: |  | True |  |  | mioDropDownList_SelectedIndexChanged | 148 |
| 11 | `territoryDropDownList` | asp:DropDownList | Territory: |  | True |  |  | territoryDropDownList_OnSelectedIndexChanged | 156 |
| 12 | `marketDropDownList` | asp:DropDownList | Market: |  | True |  |  | marketDropDownList_SelectedIndexChanged | 164 |
| 13 | `customerNameTextBox1` | asp:TextBox | Customer Name: |  | True |  |  | customerNameTextBox1_TextChanged | 202 |
| 14 | `customerTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 203 |
| 15 | `customerTextBox` | asp:TextBox | Customer Code: |  | True |  |  |  | 226 |
| 16 | `productNameTextBox2` | asp:TextBox | Product Name: |  | True |  |  | productNameTextBox2_TextChanged | 245 |
| 17 | `productNameTextBox_AutoCompleteExtender` | asp:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 247 |
| 18 | `custIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 267 |
| 19 | `productIdHiddenField` | asp:HiddenField |  |  |  |  |  |  | 268 |
| 20 | `productCodeTextBox` | asp:TextBox | Product Code: |  | True |  |  | productCodeTextBox_TextChanged | 277 |
| 21 | `txtIsactive` | asp:TextBox | Unit Price: |  |  | ReadOnly=True |  |  | 294 |
| 22 | `discountPerTextBox` | asp:TextBox | Discount Percentage: |  | True |  |  | discountPerTextBox_OnTextChanged | 313 |
| 23 | `freqQtyTextBox` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 314 |
| 24 | `discountAmountTextBox` | asp:TextBox | Discount Amount: |  | True |  |  | discountAmountTextBox_OnTextChanged | 339 |
| 25 | `FilteredTextBoxExtender1` | asp:FilteredTextBoxExtender |  |  |  |  |  |  | 340 |
| 26 | `activeDtTextBox` | asp:TextBox | Active Date: |  | True |  |  |  | 363 |
| 27 | `admissionDtTextBox_CalendarExtender` | asp:CalendarExtender |  |  |  |  |  |  | 364 |
| 28 | `ImageButton` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 367 |
| 29 | `inactiveDtTextBox` | asp:TextBox | Inactive Date: |  | True |  |  |  | 388 |
| 30 | `admissionDtTextBox_CalendarExtender0` | asp:CalendarExtender |  |  |  |  |  |  | 389 |
| 31 | `ImageButton0` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 392 |
| 32 | `searchButton` | asp:Button |  | Submit |  |  |  | submitButton_Click | 434 |
| 33 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 436 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CustomerMaster`, `Product`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `GetData` | `aProductDiscountBll.GetCustomerInfo(...)` | `ProductDiscountBLL.GetCustomerInfo(string custId)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.GetCustomerInfo` | — | — | — |
| `GetData` | `aProductDiscountBll.GetProduct(...)` | `ProductDiscountBLL.GetProduct(string productId)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.GetProduct` → `ProductDiscountDAL.GetProduct` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct` | — |
| `GetData` | `aProductDiscountBll.ProductDiscountEditLoad(...)` | `ProductDiscountBLL.ProductDiscountEditLoad(string ID)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.ProductDiscountEditLoad` → `ProductDiscountDAL.ProductDiscountEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | — | `tblProductDiscount` | — |
| `DropDownLoad` | `aProductDiscountBll.LoadmanufacturerName(...)` | `ProductDiscountBLL.LoadmanufacturerName(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.LoadmanufacturerName` → `ProductDiscountDAL.LoadmanufacturerName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyInfo` | — |
| `SaveData` | `aProductDiscountBll.SaveProductDiscount(...)` | `ProductDiscountBLL.SaveProductDiscount(ProductDiscount aProductDiscount)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.SaveProductDiscount` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProductDiscountDAL.SaveProductDiscount` → `ClsCommonInternalDAL.SaveDataByInsertCommand` | SELECT | `tblProductDiscount` | — |
| `UpdateData` | `aProductDiscountBll.UpdateProductDiscountInfo(...)` | `ProductDiscountBLL.UpdateProductDiscountInfo(ProductDiscount aProductDiscount)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.UpdateProductDiscountInfo` → `ProductDiscountDAL.UpdateProductDiscountInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblProductDiscount` | — |
| `CustomerId` | `aCustPaymentBll.CustomerLoad(...)` | `CustPaymentBLL.CustomerLoad(string aCustomerMaster)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblCustMaster` | — |
| `productCodeTextBox_TextChanged` | `aCustPaymentBll.DetailProductLoad(...)` | `CustPaymentBLL.DetailProductLoad(string aProduct)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.DetailProductLoad` → `CustPaymentDAL.DetailProductLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblProduct` | — |
| `manufacturerDropDownList_SelectedIndexChanged` | `aProductDiscountBll.DCLoad(...)` | `ProductDiscountBLL.DCLoad(DropDownList aDownList, string companyId)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.DCLoad` | — | — | — |
| `dcDropDownList_SelectedIndexChanged` | `aProductDiscountBll.AreaLoad(...)` | `ProductDiscountBLL.AreaLoad(DropDownList aDownList, string DcId)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.AreaLoad` | — | — | — |
| `areaDropDownList_OnSelectedIndexChanged` | `aProductDiscountBll.MIOLoad(...)` | `ProductDiscountBLL.MIOLoad(DropDownList aDownList, string areaId)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.MIOLoad` → `ProductDiscountDAL.MIOLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblEmpGeneralInfo`, `tblMIOInfo`, `tblTerritory` | — |
| `mioDropDownList_SelectedIndexChanged` | `aProductDiscountBll.TerritoryLoad(...)` | `ProductDiscountBLL.TerritoryLoad(DropDownList aDownList, string mioId)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.TerritoryLoad` | — | — | — |
| `territoryDropDownList_OnSelectedIndexChanged` | `aProductDiscountBll.MarketLoad(...)` | `ProductDiscountBLL.MarketLoad(DropDownList aDownList, string territoryId)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.MarketLoad` | — | — | — |
| `CheckDiscountExistOrNot` | `aProductDiscountBll.CheckDiscountExistOrNot(...)` | `ProductDiscountBLL.CheckDiscountExistOrNot(int customerId, string productCode)` <br/>`Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | `ProductDiscountBLL.CheckDiscountExistOrNot` → `ProductDiscountDAL.CheckDiscountExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCustMaster`, `tblProduct`, `tblProductDiscount` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `manufacturerDropDownList` | Manufacturer: | asp:DropDownList | True | manufacturerDropDownList_SelectedIndexChanged | `aProductDiscountBll.DCLoad()` in `manufacturerDropDownList_SelectedIndexChanged()`; `aProductDiscountBll.LoadmanufacturerName()` in `DropDownLoad()` | `tblCompanyInfo` | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center : | asp:DropDownList | True | dcDropDownList_SelectedIndexChanged | `aProductDiscountBll.AreaLoad()` in `dcDropDownList_SelectedIndexChanged()`; `aProductDiscountBll.DCLoad()` in `manufacturerDropDownList_SelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `areaDropDownList` | Area: | asp:DropDownList | True | areaDropDownList_OnSelectedIndexChanged | `aProductDiscountBll.AreaLoad()` in `dcDropDownList_SelectedIndexChanged()`; `aProductDiscountBll.MIOLoad()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblEmpGeneralInfo`, `tblMIOInfo`, `tblTerritory` | set in code (see the DAL method above) |
| `mioDropDownList` | MIO: | asp:DropDownList | True | mioDropDownList_SelectedIndexChanged | `aProductDiscountBll.MIOLoad()` in `areaDropDownList_OnSelectedIndexChanged()`; `aProductDiscountBll.TerritoryLoad()` in `mioDropDownList_SelectedIndexChanged()` | `tblEmpGeneralInfo`, `tblMIOInfo`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory: | asp:DropDownList | True | territoryDropDownList_OnSelectedIndexChanged | `aProductDiscountBll.MarketLoad()` in `territoryDropDownList_OnSelectedIndexChanged()`; `aProductDiscountBll.TerritoryLoad()` in `mioDropDownList_SelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |
| `marketDropDownList` | Market: | asp:DropDownList | True | marketDropDownList_SelectedIndexChanged | `aProductDiscountBll.MarketLoad()` in `territoryDropDownList_OnSelectedIndexChanged()` | UNKNOWN | set in code (see the DAL method above) |

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
| `ListImageButton` | asp:ImageButton |  | — | `ListImageButton_Click` | 3 lines | — | — |
| `ImageButton` | asp:ImageButton |  | — | `—` | — | — | — |
| `ImageButton0` | asp:ImageButton |  | — | `—` | — | — | — |
| `searchButton` | asp:Button | Submit | — | `submitButton_Click` | 11 lines | — | — |
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
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 13 | — | — | — | `DiscountId` |
| 2 | `Clear` | `void Clear()` | 13 | — | — | — | — |
| 3 | `GetData` | `void GetData(string id)` | 21 | `aProductDiscountBll.GetCustomerInfo`, `aProductDiscountBll.GetProduct`, `aProductDiscountBll.ProductDiscountEditLoad` | — | — | — |
| 4 | `DropDownLoad` | `void DropDownLoad()` | 6 | `aProductDiscountBll.LoadmanufacturerName` | — | — | — |
| 5 | `SaveData` | `void SaveData()` | 39 | `aProductDiscountBll.SaveProductDiscount` | — | — | `LoginName` |
| 6 | `UpdateData` | `void UpdateData()` | 29 | `aProductDiscountBll.UpdateProductDiscountInfo` | — | — | `LoginName` |
| 7 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 11 | — | — | — | — |
| 8 | `CustomerId` | `int CustomerId(out CustomerMaster aCustomerMaster)` | 7 | `aCustPaymentBll.CustomerLoad` | — | — | — |
| 9 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 10 | `productCodeTextBox_TextChanged` | `void productCodeTextBox_TextChanged(object sender, EventArgs e)` | 8 | `aCustPaymentBll.DetailProductLoad` | — | — | — |
| 11 | `GenerateParameter` | `void GenerateParameter()` | 30 | — | — | — | `MOParam` |
| 12 | `manufacturerDropDownList_SelectedIndexChanged` | `void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 6 | `aProductDiscountBll.DCLoad` | — | — | — |
| 13 | `dcDropDownList_SelectedIndexChanged` | `void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | `aProductDiscountBll.AreaLoad` | — | — | — |
| 14 | `areaDropDownList_OnSelectedIndexChanged` | `void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 4 | `aProductDiscountBll.MIOLoad` | — | — | — |
| 15 | `mioDropDownList_SelectedIndexChanged` | `void mioDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 4 | `aProductDiscountBll.TerritoryLoad` | — | — | — |
| 16 | `territoryDropDownList_OnSelectedIndexChanged` | `void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 4 | `aProductDiscountBll.MarketLoad` | — | — | — |
| 17 | `marketDropDownList_SelectedIndexChanged` | `void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 18 | `customerNameTextBox1_TextChanged` | `void customerNameTextBox1_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 19 | `productNameTextBox2_TextChanged` | `void productNameTextBox2_TextChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 20 | `CheckDiscountExistOrNot` | `bool CheckDiscountExistOrNot()` | 15 | `aProductDiscountBll.CheckDiscountExistOrNot` | — | — | — |
| 21 | `GetProductInGrid` | `void GetProductInGrid(string productCode)` | 14 | — | — | — | — |
| 22 | `ListImageButton_Click` | `void ListImageButton_Click(object sender, ImageClickEventArgs e)` | 3 | — | — | — | — |
| 23 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 24 | `discountAmountTextBox_OnTextChanged` | `void discountAmountTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 25 | `discountPerTextBox_OnTextChanged` | `void discountPerTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 26 | `AmountOrPercantageCalculation` | `void AmountOrPercantageCalculation(string flag)` | 16 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ProductDiscountEntry.aspx (markup)
  -> SInventory_UI_ProductDiscountEntry (code-behind event handler)
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
| `customerTextBox_AutoCompleteExtender` | `customerNameTextBox1` | `GetCustomernew` | `SInventoryWebService.asmx` | 1 |
| `productNameTextBox_AutoCompleteExtender` | `productNameTextBox2` | `GetProductByMenufracturer` | `SInventoryWebService.asmx` | 1 |


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `GetData` | `ProductDiscountBLL` | `GetCustomerInfo` | `string custId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `GetData` | `ProductDiscountBLL` | `GetProduct` | `string productId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `GetData` | `ProductDiscountBLL` | `ProductDiscountEditLoad` | `string ID` | `ProductDiscount` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `DropDownLoad` | `ProductDiscountBLL` | `LoadmanufacturerName` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `SaveData` | `ProductDiscountBLL` | `SaveProductDiscount` | `ProductDiscount aProductDiscount` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `UpdateData` | `ProductDiscountBLL` | `UpdateProductDiscountInfo` | `ProductDiscount aProductDiscount` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `CustomerId` | `CustPaymentBLL` | `CustomerLoad` | `string aCustomerMaster` | `CustomerMaster` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `productCodeTextBox_TextChanged` | `CustPaymentBLL` | `DetailProductLoad` | `string aProduct` | `Product` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `manufacturerDropDownList_SelectedIndexChanged` | `ProductDiscountBLL` | `DCLoad` | `DropDownList aDownList, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `dcDropDownList_SelectedIndexChanged` | `ProductDiscountBLL` | `AreaLoad` | `DropDownList aDownList, string DcId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `ProductDiscountBLL` | `MIOLoad` | `DropDownList aDownList, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `mioDropDownList_SelectedIndexChanged` | `ProductDiscountBLL` | `TerritoryLoad` | `DropDownList aDownList, string mioId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `territoryDropDownList_OnSelectedIndexChanged` | `ProductDiscountBLL` | `MarketLoad` | `DropDownList aDownList, string territoryId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| `CheckDiscountExistOrNot` | `ProductDiscountBLL` | `CheckDiscountExistOrNot` | `int customerId, string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `DropDownLoad()` → `ProductDiscountBLL.LoadmanufacturerName` → `ProductDiscountDAL.LoadmanufacturerName` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCustMaster` | `CheckDiscountExistOrNot()` → `ProductDiscountBLL.CheckDiscountExistOrNot` → `ProductDiscountDAL.CheckDiscountExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `CustomerId()` → `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader` |
| `tblEmpGeneralInfo` | `areaDropDownList_OnSelectedIndexChanged()` → `ProductDiscountBLL.MIOLoad` → `ProductDiscountDAL.MIOLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblMIOInfo` | `areaDropDownList_OnSelectedIndexChanged()` → `ProductDiscountBLL.MIOLoad` → `ProductDiscountDAL.MIOLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblProduct` | `CheckDiscountExistOrNot()` → `ProductDiscountBLL.CheckDiscountExistOrNot` → `ProductDiscountDAL.CheckDiscountExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetData()` → `ProductDiscountBLL.GetProduct` → `ProductDiscountDAL.GetProduct` → `ClsCommonInternalDAL.DataContainerDataTable`, `productCodeTextBox_TextChanged()` → `CustPaymentBLL.DetailProductLoad` → `CustPaymentDAL.DetailProductLoad` → `ClsCommonInternalDAL.DataContainerDataReader` |
| `tblProductDiscount` | `CheckDiscountExistOrNot()` → `ProductDiscountBLL.CheckDiscountExistOrNot` → `ProductDiscountDAL.CheckDiscountExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable`, `GetData()` → `ProductDiscountBLL.ProductDiscountEditLoad` → `ProductDiscountDAL.ProductDiscountEditLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `SaveData()` → `ProductDiscountBLL.SaveProductDiscount` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProductDiscountDAL.SaveProductDiscount`, `UpdateData()` → `ProductDiscountBLL.UpdateProductDiscountInfo` → `ProductDiscountDAL.UpdateProductDiscountInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblTerritory` | `areaDropDownList_OnSelectedIndexChanged()` → `ProductDiscountBLL.MIOLoad` → `ProductDiscountDAL.MIOLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `UpdateData` | Please input data in all Textbox | No | Yes | UNKNOWN | Required |
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `freqQtyTextBox` | `discountPerTextBox` | Custom, Numbers | . |
| `FilteredTextBoxExtender1` | `discountAmountTextBox` | Custom, Numbers | . |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-PRC-011-001` | {0} | Code | `Solution.Web/SInventory_UI/ProductDiscountEntry.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `DiscountId`, `LoginName`, `MOParam`

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

Audit columns written by this page: `UpdateBy`

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
| Assembly source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCustMaster` |
| Database object | `tblEmpGeneralInfo` |
| Database object | `tblMIOInfo` |
| Database object | `tblProduct` |
| Database object | `tblProductDiscount` |
| Database object | `tblTerritory` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-PRC-011-001` | Unauthenticated access | Request `/SInventory_UI/ProductDiscountEntry.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-PRC-011-002` | Validation: Please input data in all Textbox | Trigger `UpdateData()` in the state that produces this message | alert "Please input data in all Textbox" and the operation is not performed | CONFIRMED |
| `TC-PRC-011-003` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProductDiscountEntry.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProductDiscountEntry.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 33 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 26 methods traced | n/a | **MISSING → now documented** | section 12 |
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

- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` is **shared with 14 other page(s)** (e.g. `AuditReportOne.aspx`, `DeleteInvoiceReport.aspx`, `DiscountApproval.aspx`, `DistributionCenterInfoView.aspx`, `ManualOrderrCreation.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblEmpGeneralInfo` — any schema change to it breaks this page.
- Writes/reads `tblMIOInfo` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductDiscount` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
