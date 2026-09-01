# SAL-023 — ProformaorInvoiceReturn

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `SAL-023` |
| Module | `sales-order-invoice` |
| File name | `ProformaorInvoiceReturn.aspx` |
| Route | `/SInventory_UI/ProformaorInvoiceReturn.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `ProformaorInvoiceReturn.aspx.cs` |
| Code-behind class | `SInventory_UI_ProformaorInvoiceReturn` |
| Markup lines | 231 |
| Code-behind lines | 189 |
| Server controls | 11 |
| GridViews | 0 |
| Code-behind methods | 8 |

## 2. Purpose

Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers.

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction screen. It captures user input and persists or processes it through the BLL/DAL layers.

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- No other page in the repository references this file; it is reachable only through the database-driven menu or by direct URL. (INFERRED)

## 5. Route

`/SInventory_UI/ProformaorInvoiceReturn.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProformaorInvoiceReturn.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProformaorInvoiceReturn.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| Business / data layer | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |

## 7. UI Control Inventory

11 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 3 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 5 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 6 |
| 4 | `ProformaCheckBox` | asp:CheckBox |  |  | True |  |  | ProformaCheckBox_CheckedChanged | 38 |
| 5 | `InvoiceCheckBox` | asp:CheckBox |  |  | True |  |  | InvoiceCheckBox_CheckedChanged | 56 |
| 6 | `chkSampleInvoice` | asp:CheckBox |  |  | True |  |  | InvoiceCheckBox_CheckedChanged | 74 |
| 7 | `chkWHStockIn` | asp:CheckBox |  |  | True |  |  | chkWHStockIn_CheckedChanged | 93 |
| 8 | `proformaInvoiceTextBox` | asp:TextBox | WH Stock in Code / Invoice No: |  | True |  |  | proformaInvoiceTextBox_TextChanged | 113 |
| 9 | `saveButton` | asp:Button |  | Delete |  |  |  | saveButton_Click | 148 |
| 10 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 219 |
| 11 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 222 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `saveButton_Click` | `aInvoiceReturnBll.DeleteDeliveyInvoice(...)` | `ProformaOrInvoiceReturnBLL.DeleteDeliveyInvoice(string Invoice, string userId)` <br/>`Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` | `ProformaOrInvoiceReturnBLL.DeleteDeliveyInvoice` → `ProformaOrInvoiceReturn.DeleteDeliveyInvoiceDal` → `ClsCommonInternalDAL.RunStoreProcedure` | — | — | `sp_DeleteDeliveryInvoice`, `sp_DeleteDeliveryInvoice_FullRollback` |
| `saveButton_Click` | `aInvoiceReturnBll.DeleteProforma(...)` | `ProformaOrInvoiceReturnBLL.DeleteProforma(string Invoice, string userId)` <br/>`Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` | `ProformaOrInvoiceReturnBLL.DeleteProforma` → `ProformaOrInvoiceReturn.DeleteProformaDal` → `ClsCommonInternalDAL.RunStoreProcedure` | — | — | `sp_Delete_ProformaInvoice` |
| `saveButton_Click` | `aInvoiceReturnBll.InvoiceDeletePosting(...)` | `ProformaOrInvoiceReturn.InvoiceDeletePosting(string invoiceNo)` <br/>`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | `ProformaOrInvoiceReturn.InvoiceDeletePosting` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ProformaOrInvoiceReturnBLL.InvoiceDeletePosting` | — | — | `sp_DeliveryInvoiceDeletePosting` |
| `saveButton_Click` | `aInvoiceReturnBll.LoadDetailID(...)` | `ProformaOrInvoiceReturn.LoadDetailID(string Invoice)` <br/>`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | `ProformaOrInvoiceReturn.LoadDetailID` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProformaOrInvoiceReturnBLL.LoadDetailID` | — | `tblInvoice`, `tblInvoiceDetail` | — |
| `saveButton_Click` | `aInvoiceReturnBll.LoadStock(...)` | `ProformaOrInvoiceReturn.LoadStock(string InvoiceDetailID)` <br/>`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | `ProformaOrInvoiceReturn.LoadStock` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProformaOrInvoiceReturnBLL.LoadStock` | — | `tblDCStoreFreeze` | — |
| `saveButton_Click` | `aInvoiceReturnBll.ProformaReturnPosting(...)` | `ProformaOrInvoiceReturn.ProformaReturnPosting(string invoiceNo)` <br/>`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | `ProformaOrInvoiceReturn.ProformaReturnPosting` → `ClsCommonInternalDAL.UpdateAction` → `ClsCommonInternalDAL.ExecuteNonQueryAction` → `DB_Manager.UpdateAction` → `DB_Manager.ExecuteNonQueryAction` → `ProformaOrInvoiceReturnBLL.ProformaReturnPosting` | — | — | `sp_ProformaReturnPosting` |
| `proformaInvoiceTextBox_TextChanged` | `aInvoiceReturnBll.CheckSampleInvoice(...)` | `ProformaOrInvoiceReturn.CheckSampleInvoice(string sampleinvoiceCode)` <br/>`Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | `ProformaOrInvoiceReturn.CheckSampleInvoice` → `ClsCommonInternalDAL.DataContainerDataReader` → `ProformaOrInvoiceReturnBLL.CheckSampleInvoice` | SELECT | `tblSampleIssue` | — |
| `proformaInvoiceTextBox_TextChanged` | `aInvoiceReturnBll.chkInvoice(...)` | `ProformaOrInvoiceReturnBLL.chkInvoice(string Invoice)` <br/>`Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` | `ProformaOrInvoiceReturnBLL.chkInvoice` → `ProformaOrInvoiceReturn.HasInvoice` → `ClsCommonInternalDAL.DataContainerDataReader` | — | `tblInvoice` | — |
| `proformaInvoiceTextBox_TextChanged` | `aInvoiceReturnBll.chkProforma(...)` | `ProformaOrInvoiceReturnBLL.chkProforma(string Proforma)` <br/>`Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` | `ProformaOrInvoiceReturnBLL.chkProforma` → `ProformaOrInvoiceReturn.HasProforma` → `ClsCommonInternalDAL.DataContainerDataReader` | — | `tblInvoice` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `saveButton` | asp:Button | Delete | return confirm('Are you sure you want to Delete ?'); | `saveButton_Click` | 79 lines | `aInvoiceReturnBll.DeleteDeliveyInvoice`, `aInvoiceReturnBll.DeleteProforma`, `aInvoiceReturnBll.InvoiceDeletePosting`, `aInvoiceReturnBll.LoadDetailID`, `aInvoiceReturnBll.LoadStock` | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 6 | — | — | — | — |
| 2 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 3 | `Clear` | `void Clear()` | 6 | — | — | — | — |
| 4 | `ProformaCheckBox_CheckedChanged` | `void ProformaCheckBox_CheckedChanged(object sender, EventArgs e)` | 6 | — | — | — | — |
| 5 | `InvoiceCheckBox_CheckedChanged` | `void InvoiceCheckBox_CheckedChanged(object sender, EventArgs e)` | 6 | — | — | — | — |
| 6 | `saveButton_Click` | `void saveButton_Click(object sender, EventArgs e)` | 79 | `aInvoiceReturnBll.DeleteDeliveyInvoice`, `aInvoiceReturnBll.DeleteProforma`, `aInvoiceReturnBll.InvoiceDeletePosting`, `aInvoiceReturnBll.LoadDetailID`, `aInvoiceReturnBll.LoadStock`, `aInvoiceReturnBll.ProformaReturnPosting` | — | — | `LoginName` |
| 7 | `proformaInvoiceTextBox_TextChanged` | `void proformaInvoiceTextBox_TextChanged(object sender, EventArgs e)` | 47 | `aInvoiceReturnBll.CheckSampleInvoice`, `aInvoiceReturnBll.chkInvoice`, `aInvoiceReturnBll.chkProforma` | — | — | — |
| 8 | `chkWHStockIn_CheckedChanged` | `void chkWHStockIn_CheckedChanged(object sender, EventArgs e)` | 6 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
ProformaorInvoiceReturn.aspx (markup)
  -> SInventory_UI_ProformaorInvoiceReturn (code-behind event handler)
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
| `saveButton_Click` | `ProformaOrInvoiceReturnBLL` | `DeleteDeliveyInvoice` | `string Invoice, string userId` | `int` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturnBLL` | `DeleteProforma` | `string Invoice, string userId` | `int` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturnBLL` | `InvoiceDeletePosting` | `string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturnBLL` | `LoadDetailID` | `string Invoice` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturnBLL` | `LoadStock` | `string Invoice` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturnBLL` | `ProformaReturnPosting` | `string invoiceNo` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `proformaInvoiceTextBox_TextChanged` | `ProformaOrInvoiceReturnBLL` | `CheckSampleInvoice` | `string sampleinvoiceCode` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `proformaInvoiceTextBox_TextChanged` | `ProformaOrInvoiceReturnBLL` | `chkInvoice` | `string Invoice` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| `proformaInvoiceTextBox_TextChanged` | `ProformaOrInvoiceReturnBLL` | `chkProforma` | `string Proforma` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |

## 15. Repository (DAL) Mapping

| Caller (code-behind) | DAL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `saveButton_Click` | `ProformaOrInvoiceReturn` | `InvoiceDeletePosting` | `string invoiceNo` | `bool` | — | — | `sp_DeliveryInvoiceDeletePosting` | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturn` | `LoadDetailID` | `string Invoice` | `DataTable` | — | `tblInvoice`, `tblInvoiceDetail` | — | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturn` | `LoadStock` | `string InvoiceDetailID` | `DataTable` | — | `tblDCStoreFreeze` | — | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| `saveButton_Click` | `ProformaOrInvoiceReturn` | `ProformaReturnPosting` | `string invoiceNo` | `bool` | — | — | `sp_ProformaReturnPosting` | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| `proformaInvoiceTextBox_TextChanged` | `ProformaOrInvoiceReturn` | `CheckSampleInvoice` | `string sampleinvoiceCode` | `bool` | SELECT | `tblSampleIssue` | — | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_DeleteDeliveryInvoice` | saveButton_Click → `ProformaOrInvoiceReturnBLL.DeleteDeliveyInvoice → ProformaOrInvoiceReturn.DeleteDeliveyInvoiceDal → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |
| `sp_DeleteDeliveryInvoice_FullRollback` | saveButton_Click → `ProformaOrInvoiceReturnBLL.DeleteDeliveyInvoice → ProformaOrInvoiceReturn.DeleteDeliveyInvoiceDal → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |
| `sp_Delete_ProformaInvoice` | saveButton_Click → `ProformaOrInvoiceReturnBLL.DeleteProforma → ProformaOrInvoiceReturn.DeleteProformaDal → ClsCommonInternalDAL.RunStoreProcedure` | **No** — see note |
| `sp_DeliveryInvoiceDeletePosting` | saveButton_Click → `ProformaOrInvoiceReturn.InvoiceDeletePosting → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction → ProformaOrInvoiceReturnBLL.InvoiceDeletePosting` | **No** — see note |
| `sp_ProformaReturnPosting` | saveButton_Click → `ProformaOrInvoiceReturn.ProformaReturnPosting → ClsCommonInternalDAL.UpdateAction → ClsCommonInternalDAL.ExecuteNonQueryAction → DB_Manager.UpdateAction → DB_Manager.ExecuteNonQueryAction → ProformaOrInvoiceReturnBLL.ProformaReturnPosting` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `tblDCStoreFreeze` | `saveButton_Click()` → `ProformaOrInvoiceReturn.LoadStock` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProformaOrInvoiceReturnBLL.LoadStock` |
| `tblInvoice` | `proformaInvoiceTextBox_TextChanged()` → `ProformaOrInvoiceReturnBLL.chkInvoice` → `ProformaOrInvoiceReturn.HasInvoice` → `ClsCommonInternalDAL.DataContainerDataReader`, `proformaInvoiceTextBox_TextChanged()` → `ProformaOrInvoiceReturnBLL.chkProforma` → `ProformaOrInvoiceReturn.HasProforma` → `ClsCommonInternalDAL.DataContainerDataReader`, `saveButton_Click()` → `ProformaOrInvoiceReturn.LoadDetailID` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProformaOrInvoiceReturnBLL.LoadDetailID` |
| `tblInvoiceDetail` | `saveButton_Click()` → `ProformaOrInvoiceReturn.LoadDetailID` → `ClsCommonInternalDAL.DataContainerDataTable` → `ProformaOrInvoiceReturnBLL.LoadDetailID` |
| `tblSampleIssue` | `proformaInvoiceTextBox_TextChanged()` → `ProformaOrInvoiceReturn.CheckSampleInvoice` → `ClsCommonInternalDAL.DataContainerDataReader` → `ProformaOrInvoiceReturnBLL.CheckSampleInvoice` |

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
| `BR-SAL-023-001` | {0} | Code | `Solution.Web/SInventory_UI/ProformaorInvoiceReturn.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Session keys this page reads/writes: `LoginName`

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
| Assembly source | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| Assembly source | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| Database object | `tblDCStoreFreeze` |
| Database object | `tblInvoice` |
| Database object | `tblInvoiceDetail` |
| Database object | `tblSampleIssue` |
| Stored procedure | `sp_DeleteDeliveryInvoice` |
| Stored procedure | `sp_DeleteDeliveryInvoice_FullRollback` |
| Stored procedure | `sp_Delete_ProformaInvoice` |
| Stored procedure | `sp_DeliveryInvoiceDeletePosting` |
| Stored procedure | `sp_ProformaReturnPosting` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-SAL-023-001` | Unauthenticated access | Request `/SInventory_UI/ProformaorInvoiceReturn.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-SAL-023-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/ProformaorInvoiceReturn.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/ProformaorInvoiceReturn.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` |
| Layer source | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 11 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 8 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 5 names found | bodies **not in repo** | **PARTIAL** | section 16 |
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

- `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` is **shared with 2 other page(s)** (e.g. `ErrorInvoiceList.aspx`, `InvoiceCreationForCustomerByOrder.aspx`) — a change there is not local.
- `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` is **shared with 2 other page(s)** (e.g. `ErrorInvoiceList.aspx`, `InvoiceCreationForCustomerByOrder.aspx`) — a change there is not local.
- Writes/reads `tblDCStoreFreeze` — any schema change to it breaks this page.
- Writes/reads `tblInvoice` — any schema change to it breaks this page.
- Writes/reads `tblInvoiceDetail` — any schema change to it breaks this page.
- Writes/reads `tblSampleIssue` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
