# FIN-018 — MarketWiseCustomerPayment

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `FIN-018` |
| Module | `finance-collection` |
| File name | `MarketWiseCustomerPayment.aspx` |
| Route | `/SInventory_UI/MarketWiseCustomerPayment.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `MarketWiseCustomerPayment.aspx.cs` |
| Code-behind class | `SInventory_UI_CustomerPayment` |
| Markup lines | 537 |
| Code-behind lines | 429 |
| Server controls | 43 |
| GridViews | 1 |
| Code-behind methods | 20 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/MarketWiseCustomerPayment.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |

## 7. UI Control Inventory

43 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 24 |
| 4 | `companyNameDropDownList` | asp:DropDownList | Company Name |  | True |  |  | companyDropDownList_SelectedIndexChanged | 62 |
| 5 | `distributionDropDownList` | asp:DropDownList | Distribution : |  | True |  |  | distributionDropDownList_SelectedIndexChanged | 83 |
| 6 | `areaDropDownList` | asp:DropDownList | Area Name: |  | true |  |  | areaDropDownList_OnSelectedIndexChanged | 104 |
| 7 | `territoryDropDownList` | asp:DropDownList | Territory |  | True |  |  | territoryDropDownList_SelectedIndexChanged | 123 |
| 8 | `marketDropDownList` | asp:DropDownList | Market |  | True |  |  | marketDropDownList_SelectedIndexChanged | 143 |
| 9 | `customerTextBox` | asp:TextBox | Customer |  | True |  |  | customerTextBox_TextChanged | 162 |
| 10 | `customerDropDownList` | asp:DropDownList | Customer |  | True |  |  | customerDropDownList_SelectedIndexChanged | 180 |
| 11 | `paymentDtTextBox` | asp:TextBox | Payment Date |  | True | ReadOnly=True |  |  | 201 |
| 12 | `paymentAmountTextBox` | asp:TextBox | Payment Amount |  |  |  |  |  | 229 |
| 13 | `FilteredTextBoxExtenderunitValue` | asp:FilteredTextBoxExtender | Payment Amount |  |  | Enabled=True |  |  | 230 |
| 14 | `payTypeDDL` | asp:DropDownList | Payment Type |  |  |  |  |  | 248 |
| 15 | `refNameTextBox` | asp:TextBox | Ref No |  |  |  |  |  | 264 |
| 16 | `refDtTextBox` | asp:TextBox | Ref Date |  | True |  |  |  | 279 |
| 17 | `paymentDtTextBox_CalendarExtender0` | asp:CalendarExtender | Ref Date |  |  |  |  |  | 281 |
| 18 | `ImageButton0` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 285 |
| 19 | `progress` | asp:UpdateProgress |  |  |  |  |  |  | 309 |
| 20 | `imgWait` | asp:Image |  | ~/Images/loading-icon-big.gif |  |  |  |  | 313 |
| 21 | `orderGridView` | asp:GridView |  |  |  |  |  |  | 336 |
| 22 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 339 |
| 23 | `chkSelectAll` | asp:CheckBox | orderGridView → col[0] "(no header)" |  | True |  |  | chkSelectAll_CheckedChanged | 341 |
| 24 | `chkSelect` | asp:CheckBox | orderGridView → col[0] "(no header)" |  | True |  |  | chkSelect_OnCheckedChanged | 345 |
| 25 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 348 |
| 26 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 349 |
| 27 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 350 |
| 28 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 351 |
| 29 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 352 |
| 30 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 353 |
| 31 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 354 |
| 32 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 355 |
| 33 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 356 |
| 34 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 357 |
| 35 | `payAmountTextBox` | asp:TextBox | orderGridView → col[10] "Pay Amount" | {{Eval('Due')}} | True |  |  | payAmountTextBox_TextChanged | 359 |
| 36 | `FilteredTextBoxExtenderunitValue` | asp:FilteredTextBoxExtender |  |  |  | Enabled=True |  |  | 361 |
| 37 | `UpdatePanel3` | asp:UpdatePanel |  |  |  |  |  |  | 393 |
| 38 | `saveButton` | asp:Button |  | Save |  |  |  | saveButton_Click | 395 |
| 39 | `cancelButton` | asp:Button |  | Cancel |  |  |  | cancelButton_Click | 400 |
| 40 | `UpdateProgress2` | asp:UpdateProgress |  |  |  |  |  |  | 402 |
| 41 | `Img2` | asp:Image |  | ~/Images/ajax-loader.gif |  |  |  |  | 406 |
| 42 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 526 |
| 43 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 529 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `orderGridView`

| Attribute | Value |
|---|---|
| datakeynames | `InvoiceId,CustomerMasterId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | TemplateField | _(none)_ | — | — | `CheckBox:chkSelectAll`, `CheckBox:chkSelect` | — |
| 1 | BoundField | Customer Code | CustomerCode | — | — | — |
| 2 | BoundField | Customer Name | CustomerName | — | — | — |
| 3 | BoundField | Pro.Invoice No | InvoiceNo | — | — | — |
| 4 | BoundField | Pro.Invoice Date | InvoiceDate | — | — | — |
| 5 | BoundField | Del Invoice No | DelivaryInvoiceNo | — | — | — |
| 6 | BoundField | Del Invoice Date | UpdateDate | — | — | — |
| 7 | BoundField | Del Inv Amount | DeliveryTpGrandTotal | — | — | — |
| 8 | BoundField | Previous Pay | PaymentAmount | — | — | — |
| 9 | BoundField | Due Amount | Due | — | — | — |
| 10 | TemplateField | Pay Amount | — | `Due` | `TextBox:payAmountTextBox`, `FilteredTextBoxExtender:payAmountTextBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

DAO / entity types instantiated by this page: `CustPayment`, `CustomerMaster`

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `DropDownList` | `aCustPaymentBll.LoadCompanyDropdownList(...)` | `CustPaymentBLL.LoadCompanyDropdownList(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadCompanyDropdownList` → `CustPaymentDAL.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyInfo` | — |
| `DropDownList` | `aCustPaymentBll.LoadSC(...)` | `CustPaymentBLL.LoadSC(DropDownList ddl)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadSC` → `CustPaymentDAL.LoadSC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit` | — |
| `DropDownList` | `aCustPaymentBll.PaymentTypeLoadBLL(...)` | `CustPaymentBLL.PaymentTypeLoadBLL(DropDownList aDropDownList)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.PaymentTypeLoadBLL` → `CustPaymentDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` | — | `tblPaymentType` | — |
| `companyDropDownList_SelectedIndexChanged` | `aCustPaymentBll.LoadDC(...)` | `CustPaymentBLL.LoadDC(DropDownList ddl, string companyId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadDC` | — | — | — |
| `salesCenterDropDownList_SelectedIndexChanged` | `aCustPaymentBll.LoadMarket(...)` | `CustPaymentBLL.LoadMarket(DropDownList ddl, string comunitId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadMarket` → `CustPaymentDAL.LoadMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `View_CustomerMaster`, `tblCompanyUnit`, `tblMarket` | — |
| `customerTextBox_TextChanged` | `aCustPaymentBll.LoadInvoice(...)` | `CustPaymentBLL.LoadInvoice(string comUnitId, string customerId, string marketId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoice` → `CustPaymentDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CustomerMaster`, `tblInvoice` | — |
| `CustomerId` | `aCustPaymentBll.CustomerLoad(...)` | `CustPaymentBLL.CustomerLoad(string aCustomerMaster)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader` | SELECT | `tblCustMaster` | — |
| `marketDropDownList_SelectedIndexChanged` | `aCustPaymentBll.LoadCustomerMaster(...)` | `CustPaymentBLL.LoadCustomerMaster(DropDownList ddl, string marketId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadCustomerMaster` → `CustPaymentDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `View_CustomerMaster`, `tblCustMaster` | — |
| `marketDropDownList_SelectedIndexChanged` | `aCustPaymentBll.LoadInvoice(...)` | `CustPaymentBLL.LoadInvoice(string comUnitId, string customerId, string marketId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadInvoice` → `CustPaymentDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CustomerMaster`, `tblInvoice` | — |
| `saveButton_Click` | `aCustPaymentBll.SaveCustPayment(...)` | `CustPaymentBLL.SaveCustPayment(CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDetailList)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `CustPaymentDAL.GetInvoiceExistOrNot` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.PaymentJournalPosting` | DELETE, INSERT, SELECT | `tblCustPayDetail`, `tblCustomerPay` | `sp_CustomerPaymentPosting`, `sp_Sales_CustomerPaymentPosting`, `sp_Save_SaveCustPayment` |
| `saveButton_Click` | `aCustPaymentBll.UpdateInvoicePaymentAmount(...)` | `CustPaymentBLL.UpdateInvoicePaymentAmount(string amount, string status, string id)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.UpdateInvoicePaymentAmount` → `CustPaymentDAL.UpdateInvoicePaymentAmount` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblInvoice` | `sp_UD_InvoicePayment` |
| `distributionDropDownList_SelectedIndexChanged` | `aCustPaymentBll.LoadDcWiseAreaAloByDc(...)` | `CustPaymentBLL.LoadDcWiseAreaAloByDc(DropDownList ddl,string DcWiseAreaID)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadDcWiseAreaAloByDc` → `CustPaymentDAL.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblArea`, `tblDcWiseAreaInfo` | — |
| `areaDropDownList_OnSelectedIndexChanged` | `aCustPaymentBll.LoadTerritoryOnDropdownList(...)` | `CustPaymentBLL.LoadTerritoryOnDropdownList(DropDownList ddl, string areaId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.LoadTerritoryOnDropdownList` → `CustPaymentDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblTerritory` | — |
| `territoryDropDownList_SelectedIndexChanged` | `aCustPaymentBll.GetMarketNameByTerritoryId(...)` | `CustPaymentBLL.GetMarketNameByTerritoryId(DropDownList ddl, string terrId)` <br/>`Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | `CustPaymentBLL.GetMarketNameByTerritoryId` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblMarket` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `companyNameDropDownList` | Company Name | asp:DropDownList | True | companyDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadCompanyDropdownList()` in `DropDownList()`; `aCustPaymentBll.LoadDC()` in `companyDropDownList_SelectedIndexChanged()` | `tblCompanyInfo` | set in code (see the DAL method above) |
| `distributionDropDownList` | Distribution : | asp:DropDownList | True | distributionDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadDC()` in `companyDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadDcWiseAreaAloByDc()` in `distributionDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadInvoice()` in `customerTextBox_TextChanged()`; `aCustPaymentBll.LoadInvoice()` in `marketDropDownList_SelectedIndexChanged()` | `View_CustomerMaster`, `tblArea`, `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblInvoice`, `tblMarket` | set in code (see the DAL method above) |
| `areaDropDownList` | Area Name: | asp:DropDownList | true | areaDropDownList_OnSelectedIndexChanged | `aCustPaymentBll.LoadDcWiseAreaAloByDc()` in `distributionDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblArea`, `tblDcWiseAreaInfo`, `tblTerritory` | set in code (see the DAL method above) |
| `territoryDropDownList` | Territory | asp:DropDownList | True | territoryDropDownList_SelectedIndexChanged | `aCustPaymentBll.GetMarketNameByTerritoryId()` in `territoryDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadTerritoryOnDropdownList()` in `areaDropDownList_OnSelectedIndexChanged()` | `tblMarket`, `tblTerritory` | set in code (see the DAL method above) |
| `marketDropDownList` | Market | asp:DropDownList | True | marketDropDownList_SelectedIndexChanged | `aCustPaymentBll.GetMarketNameByTerritoryId()` in `territoryDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadCustomerMaster()` in `marketDropDownList_SelectedIndexChanged()`; `aCustPaymentBll.LoadInvoice()` in `customerTextBox_TextChanged()`; `aCustPaymentBll.LoadInvoice()` in `marketDropDownList_SelectedIndexChanged()` | `View_CustomerMaster`, `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblMarket` | set in code (see the DAL method above) |
| `customerDropDownList` | Customer | asp:DropDownList | True | customerDropDownList_SelectedIndexChanged | `aCustPaymentBll.LoadCustomerMaster()` in `marketDropDownList_SelectedIndexChanged()` | `View_CustomerMaster`, `tblCustMaster` | set in code (see the DAL method above) |
| `payTypeDDL` | Payment Type | asp:DropDownList | False | — | `aCustPaymentBll.PaymentTypeLoadBLL()` in `DropDownList()` | `tblPaymentType` | set in code (see the DAL method above) |

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
| `ImageButton0` | asp:ImageButton |  | — | `—` | — | — | — |
| `saveButton` | asp:Button | Save | return confirm('Are you sure you want to Save ?'); | `saveButton_Click` | 74 lines | `aCustPaymentBll.SaveCustPayment`, `aCustPaymentBll.UpdateInvoicePaymentAmount` | — |
| `cancelButton` | asp:Button | Cancel | — | `cancelButton_Click` | 3 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 2
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 7 | — | — | — | — |
| 2 | `Clear` | `void Clear()` | 23 | — | — | — | — |
| 3 | `DropDownList` | `void DropDownList()` | 8 | `aCustPaymentBll.LoadCompanyDropdownList`, `aCustPaymentBll.LoadSC`, `aCustPaymentBll.PaymentTypeLoadBLL` | — | — | `UserId` |
| 4 | `companyDropDownList_SelectedIndexChanged` | `void companyDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 13 | `aCustPaymentBll.LoadDC` | — | — | — |
| 5 | `salesCenterDropDownList_SelectedIndexChanged` | `void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 5 | `aCustPaymentBll.LoadMarket` | — | — | — |
| 6 | `customerTextBox_TextChanged` | `void customerTextBox_TextChanged(object sender, EventArgs e)` | 21 | `aCustPaymentBll.LoadInvoice` | — | — | — |
| 7 | `CustomerId` | `int CustomerId(out CustomerMaster aCustomerMaster)` | 7 | `aCustPaymentBll.CustomerLoad` | — | — | — |
| 8 | `customerDropDownList_SelectedIndexChanged` | `void customerDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 9 | `marketDropDownList_SelectedIndexChanged` | `void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 25 | `aCustPaymentBll.LoadCustomerMaster`, `aCustPaymentBll.LoadInvoice` | — | — | — |
| 10 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 11 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 18 | — | — | — | — |
| 12 | `Validation` | `bool Validation()` | 73 | — | — | — | — |
| 13 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 74 | `aCustPaymentBll.SaveCustPayment`, `aCustPaymentBll.UpdateInvoicePaymentAmount` | — | — | `LoginName` |
| 14 | `payAmountTextBox_TextChanged` | `void payAmountTextBox_TextChanged(object sender, EventArgs e)` | 24 | — | — | — | — |
| 15 | `TotalPaymentAmount` | `void TotalPaymentAmount()` | 17 | — | — | — | — |
| 16 | `chkSelect_OnCheckedChanged` | `void chkSelect_OnCheckedChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 17 | `distributionDropDownList_SelectedIndexChanged` | `void distributionDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 16 | `aCustPaymentBll.LoadDcWiseAreaAloByDc` | — | — | — |
| 18 | `areaDropDownList_OnSelectedIndexChanged` | `void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)` | 16 | `aCustPaymentBll.LoadTerritoryOnDropdownList` | — | — | — |
| 19 | `territoryDropDownList_SelectedIndexChanged` | `void territoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 14 | `aCustPaymentBll.GetMarketNameByTerritoryId` | — | — | — |
| 20 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
MarketWiseCustomerPayment.aspx (markup)
  -> SInventory_UI_CustomerPayment (code-behind event handler)
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
| `DropDownList` | `CustPaymentBLL` | `LoadCompanyDropdownList` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `DropDownList` | `CustPaymentBLL` | `LoadSC` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `DropDownList` | `CustPaymentBLL` | `PaymentTypeLoadBLL` | `DropDownList aDropDownList` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `companyDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDC` | `DropDownList ddl, string companyId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `salesCenterDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadMarket` | `DropDownList ddl, string comunitId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `customerTextBox_TextChanged` | `CustPaymentBLL` | `LoadInvoice` | `string comUnitId, string customerId, string marketId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `CustomerId` | `CustPaymentBLL` | `CustomerLoad` | `string aCustomerMaster` | `CustomerMaster` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `marketDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadCustomerMaster` | `DropDownList ddl, string marketId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `marketDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadInvoice` | `string comUnitId, string customerId, string marketId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `saveButton_Click` | `CustPaymentBLL` | `SaveCustPayment` | `CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDet` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `saveButton_Click` | `CustPaymentBLL` | `UpdateInvoicePaymentAmount` | `string amount, string status, string id` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `distributionDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDcWiseAreaAloByDc` | `DropDownList ddl,string DcWiseAreaID` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `areaDropDownList_OnSelectedIndexChanged` | `CustPaymentBLL` | `LoadTerritoryOnDropdownList` | `DropDownList ddl, string areaId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `territoryDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `GetMarketNameByTerritoryId` | `DropDownList ddl, string terrId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_CustomerPaymentPosting` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_Sales_CustomerPaymentPosting` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_Save_SaveCustPayment` | saveButton_Click → `CustPaymentBLL.SaveCustPayment → ClsPrimaryKeyFind.PrimaryKeyMax → ClsCommonInternalDAL.DataContainerDataTable → CustPaymentDAL.DeletePaymentMasterInfo → ClsCommonInternalDAL.UpdateDataByUpdateCommand → CustPaymentDAL.GetInvoiceExistOrNot` | **No** — see note |
| `sp_UD_InvoicePayment` | saveButton_Click → `CustPaymentBLL.UpdateInvoicePaymentAmount → CustPaymentDAL.UpdateInvoicePaymentAmount → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_CustomerMaster` | `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoice` → `CustPaymentDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadCustomerMaster` → `CustPaymentDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `marketDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadInvoice` → `CustPaymentDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `salesCenterDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadMarket` → `CustPaymentDAL.LoadMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblArea` | `distributionDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadDcWiseAreaAloByDc` → `CustPaymentDAL.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyInfo` | `DropDownList()` → `CustPaymentBLL.LoadCompanyDropdownList` → `CustPaymentDAL.GetCompanyDropdownList` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblCompanyUnit` | `DropDownList()` → `CustPaymentBLL.LoadSC` → `CustPaymentDAL.LoadSC` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `salesCenterDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadMarket` → `CustPaymentDAL.LoadMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCustMaster` | `CustomerId()` → `CustPaymentBLL.CustomerLoad` → `CustPaymentDAL.CustomerLoad` → `ClsCommonInternalDAL.DataContainerDataReader`, `marketDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadCustomerMaster` → `CustPaymentDAL.LoadCustomerMaster` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCustPayDetail` | `saveButton_Click()` → `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo` |
| `tblCustomerPay` | `saveButton_Click()` → `CustPaymentBLL.SaveCustPayment` → `ClsPrimaryKeyFind.PrimaryKeyMax` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustPaymentDAL.DeletePaymentMasterInfo` |
| `tblDcWiseAreaInfo` | `distributionDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadDcWiseAreaAloByDc` → `CustPaymentDAL.LoadDcWiseAreaAloByDc` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblInvoice` | `customerTextBox_TextChanged()` → `CustPaymentBLL.LoadInvoice` → `CustPaymentDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `marketDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadInvoice` → `CustPaymentDAL.LoadInvoice` → `ClsCommonInternalDAL.DataContainerDataTable`, `saveButton_Click()` → `CustPaymentBLL.UpdateInvoicePaymentAmount` → `CustPaymentDAL.UpdateInvoicePaymentAmount` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` |
| `tblMarket` | `salesCenterDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.LoadMarket` → `CustPaymentDAL.LoadMarket` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `territoryDropDownList_SelectedIndexChanged()` → `CustPaymentBLL.GetMarketNameByTerritoryId` → `CustPaymentDAL.GetMarketNameByTerritoryId` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblPaymentType` | `DropDownList()` → `CustPaymentBLL.PaymentTypeLoadBLL` → `CustPaymentDAL.PaymentTypeLoad` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblTerritory` | `areaDropDownList_OnSelectedIndexChanged()` → `CustPaymentBLL.LoadTerritoryOnDropdownList` → `CustPaymentDAL.GetTerritoryOnDropdownList` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Input-character restrictions (AJAX Control Toolkit `FilteredTextBoxExtender`) — client-side only, **not re-checked on the server**:

| Extender | Target | FilterType | ValidChars |
|---|---|---|---|
| `FilteredTextBoxExtenderunitValue` | `paymentAmountTextBox` | Custom | 0123456789. |
| `FilteredTextBoxExtenderunitValue` | `payAmountTextBox` | Custom | 0123456789. |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-FIN-018-001` | {0} | Code | `Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`, `UserId`

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
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Database object | `View_CustomerMaster` |
| Database object | `tblArea` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerPay` |
| Database object | `tblDcWiseAreaInfo` |
| Database object | `tblInvoice` |
| Database object | `tblMarket` |
| Database object | `tblPaymentType` |
| Database object | `tblTerritory` |
| Stored procedure | `sp_CustomerPaymentPosting` |
| Stored procedure | `sp_Sales_CustomerPaymentPosting` |
| Stored procedure | `sp_Save_SaveCustPayment` |
| Stored procedure | `sp_UD_InvoicePayment` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-FIN-018-001` | Unauthenticated access | Request `/SInventory_UI/MarketWiseCustomerPayment.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-FIN-018-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-FIN-018-003` | Grid loads | Open the page with data present | Grid `orderGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 43 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 20 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 4 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 12 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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
- Writes/reads `View_CustomerMaster` — any schema change to it breaks this page.
- Writes/reads `tblArea` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerPay` — any schema change to it breaks this page.
- Writes/reads `tblDcWiseAreaInfo` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblMarket` — any schema change to it breaks this page.
- Writes/reads `tblPaymentType` — any schema change to it breaks this page.
- Writes/reads `tblTerritory` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
