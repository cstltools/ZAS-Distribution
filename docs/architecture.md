# Architecture

Full detail: [`../specs/00-system/architecture.md`](../specs/00-system/architecture.md) and
[`../specs/00-system/system-overview.md`](../specs/00-system/system-overview.md). This page summarizes and adds
two traced examples; it introduces no new facts.

## System architecture

ASP.NET **Web Forms** on **.NET Framework 4.0** against **SQL Server**, layered:

```
Browser
  │  full postback, or partial postback inside an UpdatePanel
  ▼
Solution.Web (UI)                — 383 .aspx pages, code-behind = the controller layer
  ▼
Library.BLL (service layer)      — 74 classes, mostly thin pass-through
  ▼
Library.DAL (data access)        — 130 classes, builds SQL by concatenation or SqlParameter
  ▼
Library.DAO (entities)           — plain property bags, no ORM, no attributes
  ▼
SQL Server: SalesrollDB_ZAS (+ ZAS_ACCDB, CSTL_ACCDB, CSTL_ERPDB, ZASHRIS by three-part name)
```

There is **no MVC, no REST controller, no routing table, no Areas**. **Confidence: CONFIRMED.** Source:
`../specs/00-system/architecture.md`.

## Project structure

| Project | Type | Contents |
|---|---|---|
| `Solution.Web` | **Web Site** project (`Project("{E24C65DC-...}")` in `Solution.sln`), not a Web Application — `.aspx.cs` compiles at run time | 383 `.aspx` pages across `SInventory_UI`, `SInventory_RPTVIEW`, `HTMLReport`, `CommonUI`, `Dashboard`, and `Login.aspx` |
| `Library.DAL` | Class library | 130 DAL classes + 5 infrastructure classes (`ClsCommonInternalDAL`, `DB_Manager`, `DB_Connection`/`DB_Authentication`/`DB_Names`, `ClsPrimaryKeyFind`, `ClsApprovalAction`) |
| `Library.BLL` | Class library | 74 BLL classes + 1 panel BLL (`PanalBLL` — login, menu, version) |
| `Library.DAO` | Class library | ~75/119 entity classes — plain property bags |
| `Library.CrystalReports` | Class library | 42 `.rpt` definitions + 114 typed DataSets (`.xsd`) |

`PrecompiledWeb/` is stale `aspnet_compiler` build output, checked into version control — it is not source
(`GAP-STR-004`). **Confidence: CONFIRMED.** Source: `../specs/00-system/system-overview.md`,
`../specs/SPEC-GAPS.md`.

## Controller layer — there isn't one

Web Forms code-behind (`<Page>.aspx.cs`) fills the role a controller would in an MVC app: event handlers
(`OnClick`, `OnSelectedIndexChanged`, etc.) read control values directly out of `.Text`/`.SelectedValue` and
call into the BLL. There is no separation between "controller" and "presenter" — the code-behind class is both.
**Confidence: CONFIRMED.** Source: `../specs/00-system/conventions.md`.

## Service layer (BLL)

`Library.BLL` has 74 classes but is thinner than it looks — the typical method is a one-line pass-through to the
DAL:

```csharp
public DataTable CustomerMaster(string CustomerMasterId)
{
    return aInvoiceDal.LoadCustomerMaster(CustomerMasterId);   // InvoiceBLL.cs
}
```

A handful of classes hold real logic: `InvoiceBLL` (FEFO batch allocation, line-level discount/VAT/net
computation, invoice numbering), `CustPaymentBLL` (payment application), `ChalanBLL`/`RequisitionBLL`
(weighted-average triggering), `PanalBLL` (login, menu, version). Many pages skip the BLL entirely and hold a
DAL instance directly. **Confidence: CONFIRMED.** Source: `../specs/00-system/architecture.md`.

## Repository layer (DAL)

`Library.DAL` (130 classes) is where SQL is actually built and sent, through two distinct paths (see "Data
access" below). There is no repository *interface*, no unit of work, and no dependency injection — classes are
instantiated with `new` at the field level in each page. **Confidence: CONFIRMED.**

## Data access

Two ways SQL reaches the server:

**1. Inline SQL through one generic procedure (the majority).** `ClsCommonInternalDAL` passes a whole SQL
string as a parameter to `ExecuteAllSqlQueryByStoreProcedure`, which executes it:

```csharp
dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
dbCommand.CommandTimeout = 360000000;
db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
dataDDL = db.ExecuteDataSet(dbCommand).Tables[0];
```

Every such query is SQL-injectable (see [security.md](security.md), `GAP-SEC-005`); about 1,400 strings in
`Library.DAL` are built this way. **Confidence: CONFIRMED.**

**2. Named stored procedures with `SqlParameter` (the minority, used for postings).** Properly parameterised,
used for stock/accounting postings, delivery pipeline, weighted average, payment posting — bodies not in this
repository.

Data access uses the Microsoft **Enterprise Library Data Access Application Block**
(`Microsoft.Practices.EnterpriseLibrary.Data`) — **no ORM**. Primary keys are computed in the application as
`MAX(id)+1` (`ClsPrimaryKeyFind`), outside any transaction (`BR-SYS-003`). Two inconsistent transaction stories
exist: `DB_Manager` opens a transaction per connection and swallows exceptions on error (returns `0`);
`ClsCommonInternalDAL` (majority) uses no transaction at all — every statement autocommits. **Confidence:
CONFIRMED.** Source: `../specs/00-system/architecture.md`.

## Frontend

Web Forms server controls in an HTML `<table>` layout, `UpdatePanel` (369 declared) for partial rendering, AJAX
Control Toolkit extenders (`CalendarExtender`, `FilteredTextBoxExtender`, `AutoCompleteExtender`,
`ModalPopupExtender`), jQuery 1.7.2 / jQuery UI 1.8.19 / select2. No control uses `AssociatedControlID` — a
field's label is the nearest preceding text node. Validation is server-side and imperative
(`if (x.Text == "") { showMessageBox(...); return false; }`) — almost no `asp:*Validator` controls (1 in the
whole application). **Confidence: CONFIRMED.** Source: `../specs/00-system/conventions.md`.

## API

**60 HTTP endpoints that are not page postbacks**: 18 ASMX methods (exposed on 2 endpoints,
`SInventoryWebService.asmx` + `WebService.asmx`, both declaring the same class), 40 page `[WebMethod]`s (mostly
on `Dashboard.aspx`/`CashFlow.aspx`/`IncomeStatement.aspx`), and 2 `.ashx` handlers. See
[`../specs/00-system/api-inventory.md`](../specs/00-system/api-inventory.md) for the full list. **Confidence:
CONFIRMED.**

## Authentication and authorization

Custom, session-based; Forms authentication is present in `Web.config` but commented out. Login is a
string-concatenated, plaintext-password query (see [security.md](security.md)). The only authentication guard in
the whole application is in `MainMasterPage.master.cs`; 127 of 383 pages don't use that master page and have no
guard of their own. Authorization is four independent layers (menu grant, company unit, approval action, one-off
flags) — not a role model. **Confidence: CONFIRMED.** Full detail: [security.md](security.md),
`../specs/00-system/role-permission.md`.

## External integrations

All database-to-database, inside SQL Server. See [integrations.md](integrations.md) for the map of the 4
external databases.

## Background services

**None found — confirmed absent.** No HTTP module, no handler beyond the two `.ashx` + Crystal's own, no
scheduler (`Quartz`/`Hangfire`/`TaskScheduler`), no DI container. All five `Global.asax` event bodies are empty.
**Confidence: CONFIRMED.** Source: `../specs/FINAL-REPORT.md`.

## Logging

**None, effectively.** `log4net` is listed in `Web.config`'s `<assemblies>` but is never configured or called
anywhere in the codebase. `Global.asax`'s `Application_Error` is empty. **Confidence: CONFIRMED.** Source:
`../specs/00-system/system-overview.md`.

## Error handling

| Level | Behaviour |
|---|---|
| `Global.asax` `Application_Error` | empty — no global handler |
| `Web.config` `<customErrors>` | not configured; `debug="true"` — full stack traces (including the failing SQL) are shown to the user |
| `DB_Manager` | swallows exceptions, rolls back, returns `0` — caller cannot distinguish an error from "no rows matched" |
| `ClsCommonInternalDAL` | `throw ex;` — rethrows but resets the stack trace |
| Page | ad hoc; most save handlers do not wrap in try/catch |
| User feedback | `ScriptManager.RegisterStartupScript(..., "alert('...')", true)` via a locally duplicated `showMessageBox` helper |

**Confidence: CONFIRMED.** See [security.md](security.md), `GAP-SEC-008`.

## Configuration

`Web.config` carries 3 connection strings (`SalesrollDB_ZAS`, `ZAS_ACCDB`, and commented-out alternates for
other deployments — see [deployment.md](deployment.md)) plus 3 Crystal Reports app settings. Hard-coded
credentials also live in `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs` — see [security.md](security.md),
`GAP-SEC-003`. **Confidence: CONFIRMED.**

## Deployment architecture

IIS-hosted ASP.NET Web Forms site, Web Site project (compiles at run time, not pre-built into an assembly except
via the stale `PrecompiledWeb/` checked-in copy). No deployment scripts, Dockerfile, or CI configuration exist in
this repository. See [deployment.md](deployment.md) for the full breakdown of what is verifiable vs. `UNKNOWN`.

## Two traced examples

### Example 1 — Invoice creation and FEFO batch allocation

UI control → code-behind → BLL → DAL → SP → table, traced end to end in
[`../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md`](../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md):

```
custCodeTextBox (UI, InvoiceCreationForCustomer.aspx)
  → custCodeTextBox_TextChanged (code-behind)
    → InvoiceBLL.CustomerMaster (BLL)
      → InvoiceDAL.LoadCustomerMaster (DAL) — 9-way INNER JOIN, inline SQL
        → tblCustMaster, tblMarket, tblTerritory, tblMIOInfo, tblEmpGeneralInfo,
          tblArea, tblDcWiseAreaInfo, tblCompanyUnit, tblCompanyInfo, tblCustCategory

saveButton_Click (code-behind)
  → InvoiceBLL.SaveInvoice (BLL)
    → ClsPrimaryKeyFind.PrimaryKeyMax (MAX(invoiceId)+1)
    → InvoiceBLL.InvoiceNoGenerator → InvoiceDAL.InvoiceNoCount
    → InvoiceDAL.SaveDataForInvoice → INSERT tblInvoice
  → InvoiceBLL.SaveInvoiceDetails (BLL)
    → InvoiceBLL.UpdateDCStock, per grid line
      → InvoiceDAL.BatchWiseProductQty → SELECT tblDCStore ORDER BY ExpDate ASC  (FEFO)
      → per batch: InvoiceBLL.InvoiceDetailInsertBLL → INSERT tblInvoiceDetail
                   InvoiceDAL.UpdateDCStoreQuantity → UPDATE tblDCStore SET StockQty = <computed>
```

Note that stock posting to accounting (`sp_ProformaPosting`) is a separate call from a different screen, not
part of this trace — see the workflow doc for the full stage-by-stage breakdown, including the confirmed defects
(`GAP-APP-002`, `GAP-APP-004`, `GAP-APP-005`, `GAP-APP-007`). **Confidence: CONFIRMED.**

### Example 2 — Delivery confirmation (Full branch)

Traced in
[`../specs/delivery-logistics/workflows/WF-delivery-confirmation.md`](../specs/delivery-logistics/workflows/WF-delivery-confirmation.md):

```
statusDropDownList (UI, DelivaryInvoiceCreationAuto.aspx)
  → gotoinvoiceButton_Click (code-behind) — branches on .SelectedItem.Text == "Full"
    → InvoiceBLL.SaveFullInvoice (BLL)
      → InvoiceDAL.SaveFullInvoice (DAL)
        → EXEC sp_DeliveryConformationFull @InvoiceNo, @UpdateBy, @UpdateDate  (SP body UNKNOWN)
    → InvoiceBLL.PostDeliveryConfFullJournal (BLL) — called unconditionally, result of the
                                                       previous call is NOT checked (GAP-APP-018)
      → InvoiceDAL.PostDeliveryConfFullJournal (DAL)
        → EXEC sp_Sales_ExecuteDeliveryPipeline @InvoiceId  (SP body UNKNOWN)
```

Two superseded posting calls (`sp_DeliveryConfirmationFullPosting`, `sp_Sales_DeliverySalesPosting`) sit
commented out immediately above the live call, recording the evolution of this step. The invoice number is read
from a positional grid cell (`Cells[7].Text`), not from a data key — a confirmed fragility (`GAP-APP-010`).
**Confidence: CONFIRMED** for everything above the SP boundary; **UNKNOWN** for what the procedures do.
