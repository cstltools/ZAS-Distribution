# RPT-115 — DeleteInvoiceReport

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `RPT-115` |
| Module | `reports` |
| File name | `DeleteInvoiceReport.aspx` |
| Route | `/SInventory_UI/DeleteInvoiceReport.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `DeleteInvoiceReport.aspx.cs` |
| Code-behind class | `SInventory_UI_DeleteInvoiceReport` |
| Markup lines | 287 |
| Code-behind lines | 214 |
| Server controls | 14 |
| GridViews | 0 |
| Code-behind methods | 15 |

## 2. Purpose

Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Parameterised reporting screen. It collects filter values, queries the database and renders the result (GridView, Crystal Report viewer or HTML table) for viewing, printing or Excel export.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/DeleteInvoiceReport.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DeleteInvoiceReport.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DeleteInvoiceReport.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/AuditReportOneBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |

## 7. UI Control Inventory

14 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 18 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 19 |
| 4 | `MessageLabel` | asp:Label |  |  |  |  |  |  | 51 |
| 5 | `nationalCheckBox` | asp:CheckBox | National Report : |  | True |  |  | nationalCheckBox_OnCheckedChanged | 63 |
| 6 | `comUnitNameDropDownList` | asp:DropDownList | Sales Center : |  |  |  |  |  | 83 |
| 7 | `fromDateTextBox` | asp:TextBox | From Date: |  | True |  |  | fromDateTextBox_OnTextChanged | 103 |
| 8 | `imgDatse` | asp:ImageButton | From Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 104 |
| 9 | `Date` | cc1:CalendarExtender |  |  |  |  |  |  | 106 |
| 10 | `toDateTextBox` | asp:TextBox | To Date: |  | True |  |  | toDateTextBox_OnTextChanged | 125 |
| 11 | `imgDatsea` | asp:ImageButton | To Date: | ~/Images/Calendar_scheduleHS.png |  |  |  |  | 126 |
| 12 | `CalendarExtender1` | cc1:CalendarExtender |  |  |  |  |  |  | 128 |
| 13 | `viewRptButton` | asp:Button |  | View Report |  |  |  | submitButton_Click1 | 165 |
| 14 | `excelButton` | asp:Button |  | Excel |  |  | False | excelButton_OnClick | 168 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `LoadDropdownlist` | `aOrderReportBll.LoadCustomer(...)` | `CustPaymentDAL.LoadCustomer(DropDownList ddl, string batchId)` <br/>`Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | `CustPaymentDAL.LoadCustomer` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DeliveryInvoiceReportDal.LoadCustomer` → `ExcelUpForMIGODAL.LoadCustomer` → `ClsCommonInternalDAL.DataContainerDataTable` → `InvoiceReportDal.LoadCustomer` → `SampleInvoiceDal.LoadCustomer` → `AuditReportOneBll.LoadCustomer` | SELECT | `tblCustMaster`, `tblCustomerMasterExcelFileMaster`, `tblInvoice`, `tblManufacturer` | — |
| `LoadDropdownlist` | `aOrderReportBll.LoadSalesCenter(...)` | `DistributionCenterInfoDal.LoadSalesCenter()` <br/>`Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` | `DistributionCenterInfoDal.LoadSalesCenter` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderInfoDAL.LoadSalesCenter` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `ProductDiscountDAL.LoadSalesCenter` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `AuditReportOneBll.LoadSalesCenter` → `AuditReportOneDal.GetSalesCenter` | SELECT | `tblCompanyInfo`, `tblCompanyUnit` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

| Control | Screen label | Type | AutoPostBack | OnSelectedIndexChanged | Loaded by (code-behind call, matched on the control name passed as an argument) | Tables read | DisplayField / ValueField |
|---|---|---|---|---|---|---|---|
| `comUnitNameDropDownList` | Sales Center : | asp:DropDownList | False | — | `aOrderReportBll.LoadSalesCenter()` in `LoadDropdownlist()` | `tblCompanyInfo`, `tblCompanyUnit` | set in code (see the DAL method above) |

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
| `viewRptButton` | asp:Button | View Report | — | `submitButton_Click1` | 14 lines | — | — |
| `excelButton` | asp:Button | Excel | — | `excelButton_OnClick` | 7 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: `cc1:CalendarExtender`(fromDateTextBox), `cc1:CalendarExtender`(toDateTextBox)

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 8 | — | — | — | — |
| 2 | `LoadDropdownlist` | `void LoadDropdownlist()` | 4 | `aOrderReportBll.LoadCustomer`, `aOrderReportBll.LoadSalesCenter` | — | — | — |
| 3 | `ShowMessageBox` | `void ShowMessageBox(string message)` | 6 | — | — | — | — |
| 4 | `Popup` | `void Popup()` | 21 | — | — | — | `Excel`, `parameter` |
| 5 | `GenerateParameter` | `string GenerateParameter()` | 42 | — | — | — | — |
| 6 | `cancelButton_OnClick` | `void cancelButton_OnClick(object sender, EventArgs e)` | 3 | — | — | — | — |
| 7 | `Clear` | `void Clear()` | 3 | — | — | — | — |
| 8 | `DateValidationCheck` | `void DateValidationCheck(TextBox dateTextBox)` | 8 | — | — | — | — |
| 9 | `submitButton_Click1` | `void submitButton_Click1(object sender, EventArgs e)` | 14 | — | — | — | — |
| 10 | `Validation` | `bool Validation()` | 20 | — | — | — | — |
| 11 | `nationalCheckBox_OnCheckedChanged` | `void nationalCheckBox_OnCheckedChanged(object sender, EventArgs e)` | 11 | — | — | — | — |
| 12 | `excelButton_OnClick` | `void excelButton_OnClick(object sender, EventArgs e)` | 7 | — | — | — | — |
| 13 | `ExcelPopUp` | `void ExcelPopUp()` | 18 | — | — | — | `Excel`, `parameter` |
| 14 | `fromDateTextBox_OnTextChanged` | `void fromDateTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |
| 15 | `toDateTextBox_OnTextChanged` | `void toDateTextBox_OnTextChanged(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
DeleteInvoiceReport.aspx (markup)
  -> SInventory_UI_DeleteInvoiceReport (code-behind event handler)
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
| `LoadDropdownlist` | `AuditReportOneBll` | `LoadCustomer` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/AuditReportOneBll.cs` |
| `LoadDropdownlist` | `CustPaymentBLL` | `LoadCustomer` | `DropDownList ddl, string batchId` | `void` | — | — | — | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| `LoadDropdownlist` | `DeleteInvoiceReportBLL` | `LoadCustomer` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs` |
| `LoadDropdownlist` | `ExcelUpForMIGOBLL` | `LoadCustomer` | `int id` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs` |
| `LoadDropdownlist` | `AuditReportOneBll` | `LoadSalesCenter` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/AuditReportOneBll.cs` |
| `LoadDropdownlist` | `DeleteInvoiceReportBLL` | `LoadSalesCenter` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs` |
| `LoadDropdownlist` | `DistributionCeneterInfoBll` | `LoadSalesCenter` | `` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| `LoadDropdownlist` | `ProductDiscountBLL` | `LoadSalesCenter` | `DropDownList ddl` | `void` | — | — | — | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `LoadDropdownlist` | `CustPaymentDAL` | `LoadCustomer` | `DropDownList ddl, string batchId` | `void` | SELECT | `tblCustMaster`, `tblInvoice` | — | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| `LoadDropdownlist` | `DeliveryInvoiceReportDal` | `LoadCustomer` | `DropDownList aDownList, string marketId` | `void` | SELECT | `tblCustMaster` | — | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| `LoadDropdownlist` | `ExcelUpForMIGODAL` | `LoadCustomer` | `string parameter` | `DataTable` | SELECT | `tblCustomerMasterExcelFileMaster`, `tblManufacturer` | — | `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` |
| `LoadDropdownlist` | `InvoiceReportDal` | `LoadCustomer` | `DropDownList aDownList, string marketId` | `void` | SELECT | `tblCustMaster` | — | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| `LoadDropdownlist` | `SampleInvoiceDal` | `LoadCustomer` | `DropDownList aDownList, string marketId` | `void` | SELECT | `tblCustMaster` | — | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |
| `LoadDropdownlist` | `DistributionCenterInfoDal` | `LoadSalesCenter` | `` | `DataTable` | SELECT | `tblCompanyInfo`, `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| `LoadDropdownlist` | `OrderInfoDAL` | `LoadSalesCenter` | `DropDownList ddl` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| `LoadDropdownlist` | `ProductDiscountDAL` | `LoadSalesCenter` | `DropDownList ddl` | `void` | SELECT | `tblCompanyUnit` | — | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |

## 16. Stored Procedure Mapping

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblCompanyInfo` | `LoadDropdownlist()` → `DistributionCenterInfoDal.LoadSalesCenter` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderInfoDAL.LoadSalesCenter` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCompanyUnit` | `LoadDropdownlist()` → `DistributionCenterInfoDal.LoadSalesCenter` → `ClsCommonInternalDAL.DataContainerDataTable` → `OrderInfoDAL.LoadSalesCenter` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` |
| `tblCustMaster` | `LoadDropdownlist()` → `CustPaymentDAL.LoadCustomer` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DeliveryInvoiceReportDal.LoadCustomer` → `ExcelUpForMIGODAL.LoadCustomer` |
| `tblCustomerMasterExcelFileMaster` | `LoadDropdownlist()` → `CustPaymentDAL.LoadCustomer` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DeliveryInvoiceReportDal.LoadCustomer` → `ExcelUpForMIGODAL.LoadCustomer` |
| `tblInvoice` | `LoadDropdownlist()` → `CustPaymentDAL.LoadCustomer` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DeliveryInvoiceReportDal.LoadCustomer` → `ExcelUpForMIGODAL.LoadCustomer` |
| `tblManufacturer` | `LoadDropdownlist()` → `CustPaymentDAL.LoadCustomer` → `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` → `DeliveryInvoiceReportDal.LoadCustomer` → `ExcelUpForMIGODAL.LoadCustomer` |

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
| `BR-RPT-115-001` | {0} | Code | `Solution.Web/SInventory_UI/DeleteInvoiceReport.aspx.cs` → `ShowMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `Excel`, `parameter`

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

- Excel file import/export.

See `specs/00-system/integrations.md` for the full list.

## 25. Reports

_This page opens no report._

## 26. Dependencies

| Kind | Dependency |
|---|---|
| Master page | `MasterPages/MainMasterPage.master` (auth guard + menu) |
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/AuditReportOneBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |
| Database object | `tblCompanyInfo` |
| Database object | `tblCompanyUnit` |
| Database object | `tblCustMaster` |
| Database object | `tblCustomerMasterExcelFileMaster` |
| Database object | `tblInvoice` |
| Database object | `tblManufacturer` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-RPT-115-001` | Unauthenticated access | Request `/SInventory_UI/DeleteInvoiceReport.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-RPT-115-002` | Validation: {0} | Trigger `ShowMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/DeleteInvoiceReport.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/DeleteInvoiceReport.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/AuditReportOneBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` |
| Layer source | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 14 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 15 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 0 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 6 names found | DDL **not in repo** | **PARTIAL** | section 17 |
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

- `Library.BLL/SInventory_BLL/AuditReportOneBll.cs` is **shared with 4 other page(s)** (e.g. `AuditReportOneReportViewer.aspx`, `DeleteInvoiceReportViewer.aspx`, `AuditReportOne.aspx`, `DistributionCenterInfoView.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` is **shared with 67 other page(s)** (e.g. `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`, `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs` is **shared with 4 other page(s)** (e.g. `AuditReportOneReportViewer.aspx`, `DeleteInvoiceReportViewer.aspx`, `AuditReportOne.aspx`, `DistributionCenterInfoView.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` is **shared with 6 other page(s)** (e.g. `AuditReportOne.aspx`, `DcWiseAreaAlocation.aspx`, `DistributionCenterInfoView.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs` is **shared with 1 other page(s)** (e.g. `AuditReportOne.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` is **shared with 14 other page(s)** (e.g. `AuditReportOne.aspx`, `DiscountApproval.aspx`, `DistributionCenterInfoView.aspx`, `ManualOrderrCreation.aspx`, `NonTranscationalInvoiceReturn.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` is **shared with 64 other page(s)** (e.g. `CusTPaymentReportViewer.aspx`, `AIT Report.aspx`, `ASMWiseProductSalesReport.aspx`, `AllSalesReport.aspx`, `AreaWiseMonthlyInventoryReport.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` is **shared with 2 other page(s)** (e.g. `AuditReportOne.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` is **shared with 7 other page(s)** (e.g. `AuditReportOne.aspx`, `CustomerCreditView.aspx`, `CustomerReport.aspx`, `DistributionCenterInfoView.aspx`, `DistributionCenterInformation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` is **shared with 1 other page(s)** (e.g. `AuditReportOne.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` is **shared with 2 other page(s)** (e.g. `AuditReportOne.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` is **shared with 7 other page(s)** (e.g. `HomePage.aspx`, `AuditReportOne.aspx`, `BatchWiseCollection.aspx`, `BatchWiseCollectionReport.aspx`, `CollectionSheet.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` is **shared with 8 other page(s)** (e.g. `AuditReportOne.aspx`, `DistributionCenterInfoView.aspx`, `ManualOrderrCreation.aspx`, `NonTranscationalInvoiceReturn.aspx`, `PettyCashApproval.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` is **shared with 2 other page(s)** (e.g. `AuditReportOne.aspx`, `DcWiseAreaAlocation.aspx`) — a change there is not local.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblCompanyUnit` — any schema change to it breaks this page.
- Writes/reads `tblCustMaster` — any schema change to it breaks this page.
- Writes/reads `tblCustomerMasterExcelFileMaster` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblManufacturer` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
