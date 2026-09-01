# DLV-009 — DeliveryReturnReport

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `DLV-009` |
| Module | `delivery-logistics` |
| File name | `DeliveryReturnReport.aspx` |
| Route | `/SInventory_UI/DeliveryReturnReport.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `DeliveryReturnReport.aspx.cs` |
| Code-behind class | `SInventory_UI_DeliveryReturnReport` |
| Markup lines | 359 |
| Code-behind lines | 202 |
| Server controls | 15 |
| GridViews | 0 |
| Code-behind methods | 9 |

## 2. Purpose

Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

Outbound `Response.Redirect` targets: `Login.aspx`

## 5. Route

`/SInventory_UI/DeliveryReturnReport.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DeliveryReturnReport.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DeliveryReturnReport.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
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

15 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 5 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 13 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 14 |
| 4 | `CheckBox1` | asp:CheckBox | National Report : |  | True |  |  | CheckBox1_CheckedChanged1 | 52 |
| 5 | `groupDropDownList` | asp:DropDownList | Group Name: |  |  | Enabled=False |  |  | 69 |
| 6 | `companyNameDropDownList` | asp:DropDownList | Company Name: |  | True |  |  | companyNameDropDownList_SelectedIndexChanged | 87 |
| 7 | `dcDropDownList1` | asp:DropDownList | DC Name : |  | True |  |  | dcDropDownList1_SelectedIndexChanged | 105 |
| 8 | `InvoiceDateTextBox` | asp:TextBox | Invoice From Date: |  |  |  |  |  | 124 |
| 9 | `imgDatse` | asp:ImageButton |  | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 125 |
| 10 | `Date` | cc1:CalendarExtender |  |  |  |  |  |  | 127 |
| 11 | `todateTextBox` | asp:TextBox | Invoice To Date: |  |  |  |  |  | 146 |
| 12 | `imgDatsea` | asp:ImageButton | Invoice To Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 147 |
| 13 | `CalendarExtender1` | cc1:CalendarExtender |  |  |  |  |  |  | 149 |
| 14 | `SearchButton` | asp:Button |  | View Report |  |  |  | SearchButton_Click | 188 |
| 15 | `clearButton` | asp:Button |  | Clear |  |  |  | clearButton_OnClick | 189 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropDown` | `aOtherStockActionBLL.DCLoad(...)` | `OtherStockActionBLL.DCLoad(DropDownList aDownList, string userId)` <br/>`Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |
| `LoadDropDown` | `aReportDal.LoadCompanyInfo(...)` | `ASMWiseProductSalesDal.LoadCompanyInfo(DropDownList ddl, string groupId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BonusCampaignDal.LoadCompanyInfo` → `CompanyInfoDAL.LoadCompanyInfo` → `ClsCommonInternalDAL.DataContainerDataTable` → `CustomerDueReportDAL.LoadCompanyInfo` | SELECT | `tblCompanyInfo` | — |
| `LoadDropDown` | `aReportDal.LoadGroupInfo(...)` | `ASMWiseProductSalesDal.LoadGroupInfo(DropDownList groupDropDownList)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerFixedDiscountDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblGroupInformation` | — |
| `companyNameDropDownList_SelectedIndexChanged` | `aReportDal.LoadDcInfo(...)` | `ASMWiseProductSalesDal.LoadDcInfo(DropDownList ddl, string companyId)` <br/>`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerDueReportDAL.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `CustomerPaymentReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit` | — |
| `companyNameDropDownList_SelectedIndexChanged` | `aReportDal.LoadWhInformationByUser(...)` | `MioWiseCustomerDal.LoadWhInformationByUser(DropDownList ddl, string companyId, string userId)` <br/>`Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` | `MioWiseCustomerDal.LoadWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `RequisitionBLL.LoadWhInformationByUser` → `RequisitionDAL.GetWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `StockConditionFreezeBLL.LoadWhInformationByUser` → `BatchWiseTransitReportDal.GetWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `groupDropDownList` | Group Name: | asp:DropDownList | False | — | `aReportDal.LoadCompanyInfo()` in `LoadDropDown()`; `aReportDal.LoadGroupInfo()` in `LoadDropDown()` | `tblCompanyInfo`, `tblGroupInformation` | set in code (see the DAL method above) |
| `companyNameDropDownList` | Company Name: | asp:DropDownList | True | companyNameDropDownList_SelectedIndexChanged | `aReportDal.LoadCompanyInfo()` in `LoadDropDown()`; `aReportDal.LoadDcInfo()` in `companyNameDropDownList_SelectedIndexChanged()`; `aReportDal.LoadWhInformationByUser()` in `companyNameDropDownList_SelectedIndexChanged()` | `tblCompanyInfo`, `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |
| `dcDropDownList1` | DC Name : | asp:DropDownList | True | dcDropDownList1_SelectedIndexChanged | `aOtherStockActionBLL.DCLoad()` in `LoadDropDown()`; `aReportDal.LoadDcInfo()` in `companyNameDropDownList_SelectedIndexChanged()`; `aReportDal.LoadWhInformationByUser()` in `companyNameDropDownList_SelectedIndexChanged()` | `tblCompanyUnit`, `tblUserCompanyUnit` | set in code (see the DAL method above) |

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
| `imgDatse` | asp:ImageButton |  | — | `—` | — | — | — |
| `imgDatsea` | asp:ImageButton |  | — | `—` | — | — | — |
| `SearchButton` | asp:Button | View Report | — | `SearchButton_Click` | 50 lines | — | — |
| `clearButton` | asp:Button | Clear | — | `clearButton_OnClick` | 6 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:CalendarExtender`(InvoiceDateTextBox), `cc1:CalendarExtender`(todateTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 18 | — | — | — | `UserType` |
| 2 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 3 | `LoadDropDown` | `void LoadDropDown()` | 19 | `aOtherStockActionBLL.DCLoad`, `aReportDal.LoadCompanyInfo`, `aReportDal.LoadGroupInfo` | — | — | — |
| 4 | `companyNameDropDownList_SelectedIndexChanged` | `void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)` | 26 | `aReportDal.LoadDcInfo`, `aReportDal.LoadWhInformationByUser` | — | — | `UserId`, `UserType` |
| 5 | `SearchButton_Click` | `void SearchButton_Click(object sender, EventArgs e)` | 50 | — | — | — | `DeliveryReturnRpt` |
| 6 | `dcDropDownList1_SelectedIndexChanged` | `void dcDropDownList1_SelectedIndexChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 7 | `CheckBox1_CheckedChanged1` | `void CheckBox1_CheckedChanged1(object sender, EventArgs e)` | 12 | — | — | — | — |
| 8 | `viewRptButton_Click` | `void viewRptButton_Click(object sender, EventArgs e)` | 33 | — | — | — | `DeliveryReturnRpt` |
| 9 | `clearButton_OnClick` | `void clearButton_OnClick(object sender, EventArgs e)` | 6 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
DeliveryReturnReport.aspx (markup)
  -> SInventory_UI_DeliveryReturnReport (code-behind event handler)
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
| `LoadDropDown` | `CompanyInfoBLL` | `LoadCompanyInfo` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `CustPaymentBLL` | `LoadDcInfo` | `DropDownList ddl, string comId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `RequisitionBLL` | `LoadWhInformationByUser` | `DropDownList ddl, string companyId, string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `companyNameDropDownList_SelectedIndexChanged` | `StockConditionFreezeBLL` | `LoadWhInformationByUser` | `DropDownList ddl, string companyId,string userId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropDown` | `ASMWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `LoadDropDown` | `BatchWiseTransitReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `LoadDropDown` | `BonusCampaignDal` | `LoadCompanyInfo` | `DropDownList ddl` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` |
| `LoadDropDown` | `CompanyInfoDAL` | `LoadCompanyInfo` | `` | `DataTable` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` |
| `LoadDropDown` | `CustomerDueReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `LoadDropDown` | `CustomerFixedDiscountDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `LoadDropDown` | `CustomerPaymentReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `LoadDropDown` | `CustomerReturnBYMioDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `LoadDropDown` | `CustomerWiseLedgerDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `LoadDropDown` | `CustomerWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `LoadDropDown` | `CustomerWiseReturnDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `LoadDropDown` | `CustomerWiseSalesReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `LoadDropDown` | `DCStockInfoReprotDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `LoadDropDown` | `DestroyProductDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `LoadDropDown` | `FieldForceSummeryDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropDown` | `FixedDiscountEntryDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `LoadDropDown` | `MiaTargetDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| `LoadDropDown` | `MIOSalesSummeryReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `LoadDropDown` | `MioWiseCustomerDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `LoadDropDown` | `MioWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `LoadDropDown` | `MIOWiseProformaReportDAL` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `LoadDropDown` | `RSMWiseProductSalesDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `LoadDropDown` | `SalesSummeryReportForAcc` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `LoadDropDown` | `WHStockValuationReprtDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `LoadDropDown` | `WingsWiseSalesReportDal` | `LoadCompanyInfo` | `DropDownList ddl, string groupId` | `void` | SELECT | `tblCompanyInfo` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
| `LoadDropDown` | `ASMWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` |
| `LoadDropDown` | `BatchWiseTransitReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` |
| `LoadDropDown` | `CustomerDueReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` |
| `LoadDropDown` | `CustomerFixedDiscountDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` |
| `LoadDropDown` | `CustomerPaymentReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` |
| `LoadDropDown` | `CustomerReturnBYMioDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` |
| `LoadDropDown` | `CustomerWiseLedgerDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` |
| `LoadDropDown` | `CustomerWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` |
| `LoadDropDown` | `CustomerWiseReturnDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` |
| `LoadDropDown` | `CustomerWiseSalesReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` |
| `LoadDropDown` | `DCStockInfoReprotDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` |
| `LoadDropDown` | `DestroyProductDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` |
| `LoadDropDown` | `FieldForceSummeryDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` |
| `LoadDropDown` | `FixedDiscountEntryDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` |
| `LoadDropDown` | `MiaTargetDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` |
| `LoadDropDown` | `MIOSalesSummeryReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` |
| `LoadDropDown` | `MioWiseCustomerDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |
| `LoadDropDown` | `MioWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` |
| `LoadDropDown` | `MIOWiseProformaReportDAL` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` |
| `LoadDropDown` | `RSMWiseProductSalesDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` |
| `LoadDropDown` | `SalesSummeryReportForAcc` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` |
| `LoadDropDown` | `WHStockValuationReprtDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` |
| `LoadDropDown` | `WingsWiseSalesReportDal` | `LoadGroupInfo` | `DropDownList groupDropDownList` | `void` | SELECT | `tblGroupInformation` | — | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` |
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
| `companyNameDropDownList_SelectedIndexChanged` | `MioWiseCustomerDal` | `LoadWhInformationByUser` | `DropDownList ddl, string companyId, string userId` | `void` | SELECT | `tblCompanyUnit`, `tblUserCompanyUnit` | — | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadDropDown()` → `ASMWiseProductSalesDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadCompanyInfo` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblCompanyUnit` | `LoadDropDown()` → `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `companyNameDropDownList_SelectedIndexChanged()` → `ASMWiseProductSalesDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadDcInfo` → `ClsCommonInternalDAL.LoadDropDownValue`, `companyNameDropDownList_SelectedIndexChanged()` → `MioWiseCustomerDal.LoadWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `RequisitionBLL.LoadWhInformationByUser` → `RequisitionDAL.GetWhInformationByUser` |
| `tblGroupInformation` | `LoadDropDown()` → `ASMWiseProductSalesDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` → `BatchWiseTransitReportDal.LoadGroupInfo` → `ClsCommonInternalDAL.LoadDropDownValue` |
| `tblUserCompanyUnit` | `LoadDropDown()` → `OtherStockActionBLL.DCLoad` → `OtherStockActionDAL.DCLoad` → `ClsCommonInternalDAL.LoadDropDownValue` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`, `companyNameDropDownList_SelectedIndexChanged()` → `MioWiseCustomerDal.LoadWhInformationByUser` → `ClsCommonInternalDAL.LoadDropDownValue` → `RequisitionBLL.LoadWhInformationByUser` → `RequisitionDAL.GetWhInformationByUser` |

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
| `BR-DLV-009-001` | {0} | Code | `Solution.Web/SInventory_UI/DeliveryReturnReport.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `DeliveryReturnRpt`, `UserId`, `UserType`

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
| Assembly source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
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
| Database object | `tblGroupInformation` |
| Database object | `tblUserCompanyUnit` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-DLV-009-001` | Unauthenticated access | Request `/SInventory_UI/DeliveryReturnReport.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-DLV-009-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DeliveryReturnReport.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DeliveryReturnReport.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` |
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
| UI controls | absent | 15 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 9 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
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

- `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` is **shared with 53 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` is **shared with 22 other page(s)** (e.g. `AllSalesReport.aspx`, `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`, `BatchWiseTopSheet.aspx`, `CustomerPayment.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RequisitionBLL.cs` is **shared with 29 other page(s)** (e.g. `Dashboard.aspx`, `B2BReceiveViewer.aspx`, `StockReceiveReportViewer.aspx`, `TransferStockReceiveReportViewer.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` is **shared with 9 other page(s)** (e.g. `AllSalesReport.aspx`, `DcWiseAreaAlocation.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `SCPickingGenerate.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `ASMWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` is **shared with 50 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` is **shared with 49 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` is **shared with 52 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` is **shared with 51 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` is **shared with 58 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` is **shared with 55 other page(s)** (e.g. `CustomerWiseLedgerReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` is **shared with 54 other page(s)** (e.g. `CustomerWiseProductSalesReportViewer.aspx`, `CustomerWiseReturnReportByProductViewer.aspx`, `CustomerWiseReturnReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerDueReportViewer.aspx`, `CustomerWiseSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockInfoReportViewer.aspx`, `SalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DestroyProductDal.cs` is **shared with 55 other page(s)** (e.g. `DestroyProductReportViewer.aspx`, `DestroyProductStatementViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` is **shared with 59 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `FieldForceSalesSummeryReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` is **shared with 59 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` is **shared with 52 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `MIOSalesSummeryReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` is **shared with 52 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` is **shared with 49 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `B2BTransferChallanRcvReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` is **shared with 51 other page(s)** (e.g. `MioWiseCustomerReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` is **shared with 53 other page(s)** (e.g. `CustomerReturnByMioReportViewer.aspx`, `CustomerWiseProductSalesReportViewer.aspx`, `MioWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` is **shared with 51 other page(s)** (e.g. `RSMWiseProductSalesReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` is **shared with 51 other page(s)** (e.g. `SalesSummeryForAccHtmlReport.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` is **shared with 52 other page(s)** (e.g. `DCStockValuationHtmlReport.aspx`, `WHStockValuationHtmlReport.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` is **shared with 57 other page(s)** (e.g. `CustomerPaymentReportViewer.aspx`, `MIOWiseProformaReportViewer.aspx`, `SalesReportViewer.aspx`, `SalesTransctionReportViewer.aspx`, `WingsWiseCollectionReportViewer.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblGroupInformation` — any schema change to it breaks this page.
- Writes/reads `tblUserCompanyUnit` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
