# FIN-001 — AIT Report

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `FIN-001` |
| Module | `finance-collection` |
| File name | `AIT Report.aspx` |
| Route | `/SInventory_UI/AIT Report.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `AIT Report.aspx.cs` |
| Code-behind class | `SInventory_UI_AIT_Report` |
| Markup lines | 538 |
| Code-behind lines | 185 |
| Server controls | 26 |
| GridViews | 1 |
| Code-behind methods | 12 |

## 2. Purpose

Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/AIT Report.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/AIT Report.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/AIT Report.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |

## 7. UI Control Inventory

26 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 49 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 52 |
| 4 | `groupDropDownList` | asp:DropDownList | Group Name: |  | True | Enabled=False |  |  | 111 |
| 5 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_SelectedIndexChanged | 129 |
| 6 | `dcDropDownList` | asp:DropDownList | Distribution Center: |  |  |  |  |  | 148 |
| 7 | `customerNameTextBox1` | asp:TextBox |  |  | True |  |  | customerNameTextBox1_TextChanged | 170 |
| 8 | `customerTextBox_AutoCompleteExtender` | ajaxToolkit:AutoCompleteExtender |  |  |  | Enabled=True |  |  | 171 |
| 9 | `fromDateTextBox` | asp:TextBox | (Payment) From Date: |  |  |  |  |  | 196 |
| 10 | `imgDate` | asp:ImageButton | (Payment) From Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 197 |
| 11 | `Date` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 199 |
| 12 | `toDateTextBox` | asp:TextBox | (Payment) To Date: |  |  |  |  |  | 217 |
| 13 | `ImageButton1` | asp:ImageButton | (Payment) To Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 218 |
| 14 | `CalendarExtender1` | ajaxToolkit:CalendarExtender |  |  |  |  |  |  | 220 |
| 15 | `submitButton` | asp:Button |  | Search |  |  |  | submitButton_Click | 264 |
| 16 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 265 |
| 17 | `invoiceGridView` | asp:GridView |  |  |  |  |  |  | 301 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 304 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 305 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 306 |
| 21 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 307 |
| 22 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 308 |
| 23 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 309 |
| 24 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 310 |
| 25 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 312 |
| 26 | `invoiceButton` | asp:LinkButton | invoiceGridView → col[7] "Action" | Preview Receipt &gt; &gt; &gt; |  |  |  | invoiceButton_Click | 314 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `invoiceGridView`

| Attribute | Value |
|---|---|
| datakeynames | `CustPayDetailId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Company Name | CompanyName | — | — | — |
| 1 | BoundField | Unit Name | ComUnitName | — | — | — |
| 2 | BoundField | Customer Code | CustomerCode | — | — | — |
| 3 | BoundField | Customer Name | CustomerName | — | — | — |
| 4 | BoundField | Invoice No | InvoiceNo | — | — | — |
| 5 | BoundField | Payment Date | PaymentDate | — | — | — |
| 6 | BoundField | Paid AIT | PaidAmount | — | — | — |
| 7 | TemplateField | Action | — | — | `LinkButton:invoiceButton` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropdownList` | `aReportDal.LoadCompanyInfo(...)` | `ASMWiseProductSalesDal.LoadCompanyInfo(DropDownList ddl, string groupId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BonusCampaignDal.LoadCompanyInfo` → `CompanyInfoDAL.LoadCompanyInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerDueReportDAL.LoadCompanyInfo` | SELECT | `tblCompanyInfo` | — |
| `LoadDropdownList` | `aReportDal.LoadGroupInfo(...)` | `ASMWiseProductSalesDal.LoadGroupInfo(DropDownList groupDropDownList)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerFixedDiscountDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblGroupInformation` | — |
| `companyNameDropDownList_SelectedIndexChanged` | `aReportDal.LoadDcInfo(...)` | `ASMWiseProductSalesDal.LoadDcInfo(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerPaymentReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |
| `submitButton_Click` | `aReportDal.LoadAitInvoice(...)` | `CustomerPaymentReportDal.LoadAitInvoice(string parameter)` <br/>`Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` | `CustomerPaymentReportDal.LoadAitInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `groupDropDownList` | Group Name: | asp:DropDownList | True | — | `aReportDal.LoadCompanyInfo()` in `LoadDropdownList()`; `aReportDal.LoadGroupInfo()` in `LoadDropdownList()` | `tblCompanyInfo`, `tblGroupInformation` | set in code (see the DAL method above) |
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | companyNameDropDownList_SelectedIndexChanged | `aReportDal.LoadCompanyInfo()` in `LoadDropdownList()`; `aReportDal.LoadDcInfo()` in `companyNameDropDownList_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit` | set in code (see the DAL method above) |
| `dcDropDownList` | Distribution Center: | asp:DropDownList | False | — | `aReportDal.LoadDcInfo()` in `companyNameDropDownList_SelectedIndexChanged()` | `tblCompanyUnit` | set in code (see the DAL method above) |

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
| `ImageButton1` | asp:ImageButton |  | — | `—` | — | — | — |
| `submitButton` | asp:Button | Search | — | `submitButton_Click` | 17 lines | `aReportDal.LoadAitInvoice` | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 3 lines | — | — |
| `invoiceButton` | asp:LinkButton | Preview Receipt &gt; &gt; &gt; | — | `invoiceButton_Click` | 10 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `ajaxToolkit:AutoCompleteExtender`(customerNameTextBox1), `ajaxToolkit:CalendarExtender`(fromDateTextBox), `ajaxToolkit:CalendarExtender`(toDateTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | — | — | — | — |
| 2 | `LoadDropdownList` | `void LoadDropdownList()` | 17 | `aReportDal.LoadCompanyInfo`, `aReportDal.LoadGroupInfo` | — | — | — |
| 3 | `companyNameDropDownList_SelectedIndexChanged` | `void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 10 | `aReportDal.LoadDcInfo` | — | — | — |
| 4 | `submitButton_Click` | `void submitButton_Click(object sender, EventArgs e)` | 17 | `aReportDal.LoadAitInvoice` | — | — | — |
| 5 | `Validation` | `bool Validation()` | 29 | — | — | — | — |
| 6 | `PopUpCustomerPaymentReport` | `void PopUpCustomerPaymentReport(string id)` | 8 | — | — | — | `paydetailId` |
| 7 | `GetPrameterList` | `String GetPrameterList()` | 23 | — | — | — | — |
| 8 | `customerNameTextBox1_TextChanged` | `void customerNameTextBox1_TextChanged(object sender, EventArgs e)` | 9 | — | — | — | — |
| 9 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 5 | — | — | — | — |
| 10 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 11 | `Clear` | `void Clear()` | 7 | — | — | — | — |
| 12 | `invoiceButton_Click` | `void invoiceButton_Click(object sender, EventArgs e)` | 10 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
AIT Report.aspx (markup)
  -> SInventory_UI_AIT_Report (code-behind event handler)
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


## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropdownList` | `CompanyInfoBLL` | `LoadCompanyInfo` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDcInfo` | `DropDownList ddl, string comId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropdownList` | `ASMWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `BatchWiseTransitReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `LoadDropdownList` | `BonusCampaignDal` | `LoadCompanyInfo` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| `LoadDropdownList` | `CompanyInfoDAL` | `LoadCompanyInfo` | `` | `DataTable` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| `LoadDropdownList` | `CustomerDueReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `LoadDropdownList` | `CustomerFixedDiscountDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `LoadDropdownList` | `CustomerPaymentReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `LoadDropdownList` | `CustomerReturnBYMioDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `LoadDropdownList` | `CustomerWiseLedgerDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `LoadDropdownList` | `CustomerWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `LoadDropdownList` | `CustomerWiseReturnDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `LoadDropdownList` | `CustomerWiseSalesReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `LoadDropdownList` | `DCStockInfoReprotDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `LoadDropdownList` | `DestroyProductDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `LoadDropdownList` | `FieldForceSummeryDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropdownList` | `FixedDiscountEntryDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `LoadDropdownList` | `MiaTargetDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| `LoadDropdownList` | `MIOSalesSummeryReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `LoadDropdownList` | `MioWiseCustomerDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `LoadDropdownList` | `MioWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `LoadDropdownList` | `MIOWiseProformaReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `LoadDropdownList` | `RSMWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `SalesSummeryReportForAcc` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `LoadDropdownList` | `WHStockValuationReprtDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `LoadDropdownList` | `WingsWiseSalesReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `LoadDropdownList` | `ASMWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `BatchWiseTransitReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `LoadDropdownList` | `CustomerDueReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `LoadDropdownList` | `CustomerFixedDiscountDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `LoadDropdownList` | `CustomerPaymentReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `LoadDropdownList` | `CustomerReturnBYMioDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `LoadDropdownList` | `CustomerWiseLedgerDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `LoadDropdownList` | `CustomerWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `LoadDropdownList` | `CustomerWiseReturnDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `LoadDropdownList` | `CustomerWiseSalesReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `LoadDropdownList` | `DCStockInfoReprotDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `LoadDropdownList` | `DestroyProductDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `LoadDropdownList` | `FieldForceSummeryDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropdownList` | `FixedDiscountEntryDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `LoadDropdownList` | `MiaTargetDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| `LoadDropdownList` | `MIOSalesSummeryReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `LoadDropdownList` | `MioWiseCustomerDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `LoadDropdownList` | `MioWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `LoadDropdownList` | `MIOWiseProformaReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `LoadDropdownList` | `RSMWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `LoadDropdownList` | `SalesSummeryReportForAcc` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `LoadDropdownList` | `WHStockValuationReprtDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `LoadDropdownList` | `WingsWiseSalesReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `ASMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `BatchWiseTransitReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustomerDueReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustomerPaymentReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustomerReturnBYMioDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustomerWiseLedgerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustomerWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustomerWiseReturnDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustomerWiseSalesReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustPaymentDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `DCStockInfoReprotDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `DestroyProductDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `FieldForceSummeryDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `MIOSalesSummeryReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `MioWiseCustomerDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `MioWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `MIOWiseProformaReportDAL` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `RSMWiseProductSalesDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `SalesSummeryReportForAcc` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `WHStockValuationReprtDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `WingsWiseSalesReportDal` | `LoadDcInfo` | `DropDownList ddl, string companyId` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `submitButton_Click` | `CustomerPaymentReportDal` | `LoadAitInvoice` | `string parameter` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadDropdownList()` → `ASMWiseProductSalesDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue`, `submitButton_Click()` → `CustomerPaymentReportDal.LoadAitInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyUnit` | `companyNameDropDownList_SelectedIndexChanged()` → `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue`, `submitButton_Click()` → `CustomerPaymentReportDal.LoadAitInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustMaster` | `submitButton_Click()` → `CustomerPaymentReportDal.LoadAitInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustPayDetail` | `submitButton_Click()` → `CustomerPaymentReportDal.LoadAitInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCustomerPay` | `submitButton_Click()` → `CustomerPaymentReportDal.LoadAitInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblGroupInformation` | `LoadDropdownList()` → `ASMWiseProductSalesDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblInvoice` | `submitButton_Click()` → `CustomerPaymentReportDal.LoadAitInvoice` → `ClsCommonInternalDAL.DataContainerDataTable` |

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
| `BR-FIN-001-001` | {0} | Code | `Solution.Web/SInventory_UI/AIT Report.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `paydetailId`

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
| Assembly source | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblCustPayDetail` |
| Database object | `tblCustomerPay` |
| Database object | `tblGroupInformation` |
| Database object | `tblInvoice` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-FIN-001-001` | Unauthenticated access | Request `/SInventory_UI/AIT Report.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-FIN-001-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-FIN-001-003` | Grid loads | Open the page with data present | Grid `invoiceGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/AIT Report.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/AIT Report.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
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
| UI controls | absent | 26 controls enumerated | n/a | **MISSING → now documented** | section 7 |
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

- `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` is **shared with 53 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `ASMWiseProductSalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` is **shared with 50 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` is **shared with 49 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` is **shared with 52 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` is **shared with 51 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` is **shared with 58 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` is **shared with 55 other page(s)** (e.g. `CustomerWiseLedgerReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` is **shared with 54 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `CustomerWiseSalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockInfoReportViewer.aspx`, `SalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DestroyProductDal.cs` is **shared with 55 other page(s)** (e.g. `DestroyProductReportViewer.aspx`, `DestroyProductStatementViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` is **shared with 59 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `FieldForceSalesSummeryReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` is **shared with 59 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `MIOSalesSummeryReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` is **shared with 49 other page(s)** (e.g. `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`, `B2BTransferChallanReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` is **shared with 51 other page(s)** (e.g. `MioWiseCustomerReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `RSMWiseProductSalesReportViewer.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` is **shared with 51 other page(s)** (e.g. `SalesSummeryForAccHtmlReport.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockValuationHtmlReport.aspx`, `WHStockValuationHtmlReport.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` is **shared with 57 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`, `WingsWiseCollectionReportViewer.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustPayDetail` — any schema change to it breaks this page.
- Writes/reads `tblCustomerPay` — any schema change to it breaks this page.
- Writes/reads `tblGroupInformation` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
