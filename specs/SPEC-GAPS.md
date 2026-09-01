# Specification Gaps

What this specification could **not** determine, why, and exactly what would close it.

Two kinds of entry appear here:

- **Gaps in the specification** — information that exists somewhere but not in this repository.
- **Defects and risks found while reverse engineering** — these are recorded because a maintainer reading a page
  spec must know about them. They are findings, not requests to change code; nothing in this exercise modified
  the application.

Status values: `OPEN` (blocking, needs an external artefact), `KNOWN` (documented, no artefact will change it),
`FINDING` (a defect or risk confirmed in code).

---

## A. Missing artefacts — the big four

### `GAP-DB-001` — Stored procedure bodies are absent · **OPEN** · impact: **critical**

| | |
|---|---|
| Module | all |
| Missing | the T-SQL of **119** stored procedures |
| Source of truth | the SQL Server instance holding `SalesrollDB_ZAS` |
| Impact | Everything the application delegates to the database is undocumentable: all accounting postings, the whole delivery pipeline (`sp_Sales_ExecuteDeliveryPipeline`), stock posting, weighted-average costing, payment posting, batch locking, HR sync, MIGO stock-in, bin cards. That includes their validation, their transactions, their error behaviour, their idempotency and their result sets. Sections 16, 19 and 21 of every page spec stop at the procedure boundary because of this. |
| To close | `EXEC sp_helptext '<name>'` for every name in [00-system/stored-procedures.md](00-system/stored-procedures.md), or a scripted `database/` folder. |

### `GAP-DB-002` — No database schema · **OPEN** · impact: **critical**

| | |
|---|---|
| Missing | DDL for **126 tables** and **6 views**: columns, data types, nullability, defaults, primary keys, foreign keys, indexes, check constraints, triggers |
| Impact | Field-level UI→column mapping (Phase 11) can only be given at the *method* level, not the *column* level. Default status values are unknown. Referential integrity is unknown, so "what breaks if I delete this" is unanswerable. Whether `IsActive` is `bit`, `varchar` or `nvarchar` — which decides whether `BR-SYS-005` is a real bug — is unknown. |
| To close | script the database (`Generate Scripts` → schema only), or supply `INFORMATION_SCHEMA.COLUMNS`, `sys.foreign_keys`, `sys.indexes`, `sys.triggers`, `sys.default_constraints`. |

### `GAP-SEC-001` — Menu and permission rows are absent · **OPEN** · impact: **high**

| | |
|---|---|
| Missing | `tblMainMenu`, `tblMenuDistribution`, `tblUserCompanyUnit`, `tblAction`, `tblActionSteps`, `tblActionStepDetail`, `tblActionPageWiseStep`, `tblActionUserWiseApproval` |
| Impact | The menu tree is unknown, so **module grouping in this specification is INFERRED from page names**, not real. Section 4 of every page spec cannot state a menu path. Section 20 of every page spec is `UNKNOWN`. The approval status vocabulary is unknown (it is `tblAction.ActionText`). |
| To close | the eight `SELECT`s listed at the end of [00-system/role-permission.md](00-system/role-permission.md). |

### `GAP-DOC-001` — No requirement or design source of any kind · **KNOWN** · impact: **medium**

The repository contained no `specs/`, `docs/`, `.claude/` or `database/` directory, no README, no change log,
no ticket references in code, and no XML documentation comments. **Every "Business Objective" (section 3) in
every page spec is therefore marked UNKNOWN**, with an INFERRED objective derived from the implementation. What
the software is *supposed* to do can only come from the business.

---

## B. Missing per-area information

### `GAP-UI-001` — Field labels are heuristic · **KNOWN** · impact: low

No control uses `AssociatedControlID`; labels are loose text in `<td>` cells. Section 7 of each page spec takes
the nearest preceding text, which is right for the standard two-column layout and wrong where the layout
deviates. Grid controls instead take their column `HeaderText`, which is exact.

### `GAP-UI-002` — Visible/enabled conditions set at run time are not captured · **KNOWN** · impact: medium

Section 7 reports `Visible`/`Enabled` **as declared in markup**. Many pages flip them in code
(`btn.Visible = false`, `grid.Columns[4].Visible = false`). Section 12 lists the methods that do so, but a
consolidated per-control run-time condition table would require symbolic execution and is not provided.

### `GAP-UI-003` — Page-level CSS is not attributed · **KNOWN** · impact: low

211 `.css` files exist across `css/`, `PopCss/`, `LoginAssets/`, `HTML_Report_css/`. Most are theme files
pulled in by the master page rather than per page. Per-page CSS ownership was not traced.

### `GAP-JS-001` — Shared JavaScript is inventoried, not traced · **PARTIAL** · impact: medium

`Solution.Web/scripts` holds 21 files, of which 18 are third-party (jQuery 1.7.2, jQuery UI 1.8.19, fancybox,
flexslider, isotope, jcarousel, layerslider, modernizr, gmap, twitter, easing, selectnav) plus `custom.js`,
`datepicker.js`, `switcher.js` — all **theme** code from the purchased template, not business logic. Business
behaviour lives in the code-behind, not in JavaScript. Inline `<script>` functions **are** listed per page
(section 11). The 671 `.js` files counted repository-wide are overwhelmingly library files under
`select2/`, `crystalreportviewers13/` and `PrecompiledWeb/`; they were not individually traced, and doing so
would add nothing.

### `GAP-RPT-001` — Crystal report internals are not readable · **OPEN** · impact: medium

`.rpt` files are a binary format. This specification records, for each of the **100** viewer pages, the `.rpt`
file it loads, the parameters it receives and the DAL it queries — but **not** the report's own field layout,
formulas, groups, sorts, suppression rules or embedded SQL. **48 of 146** distinct `.rpt` file names are not referenced by
any viewer page found in the repository and may be dead — see
[reports/REPORT-INVENTORY.md](reports/REPORT-INVENTORY.md).
To close: open each `.rpt` in Crystal Designer and export the definition, or supply the field/formula listing.

### `GAP-TEST-001` — No tests exist to describe · **KNOWN** · impact: medium

There is no test project, no test file and no test data anywhere. Section 27 of each page spec therefore
proposes test cases derived from the code's own validation messages; they have never been run.

---

## C. Findings — security

These are **CONFIRMED defects in the application**, not gaps in the documentation.

### `GAP-SEC-002` — 127 pages have no authentication guard · **FINDING** · severity: **critical**

The only session check in the application is in `MainMasterPage.master.cs`. 256 pages use that master page;
**127 do not**, and most of those are report viewers that accept their parameters from the query string. Any of
them can be requested without logging in. The per-page list, with a "checks session itself?" column, is at the
end of [INVENTORY-PAGES.md](INVENTORY-PAGES.md).

### `GAP-SEC-005` — SQL injection is systemic · **FINDING** · severity: **critical**

User input is concatenated into SQL strings throughout `Library.DAL` and then executed by
`EXEC ExecuteAllSqlQueryByStoreProcedure @Query = '<string>'`. This includes the **login query**:

```csharp
"select * from dbo.tblUser U left join tblUserCompanyUnit UC on U.UserId=UC.UserId " +
"where LoginName='" + loginName + "' and Password='" + password + "' and UserStatus='active'"
```

`Library.DAL` alone declares about **1,400** SQL strings built this way. Parameterised commands are used only for the ~119 named procedures.

### `GAP-SEC-003` — Credentials are hard-coded in source · **FINDING** · severity: **critical**

`Library.DAL/MAIN_FUNCTION/DB_Authentication.cs` contains a live server name, the `sa` login and a plaintext
password, with five more server/password pairs commented out. `Solution.Web/Web.config` contains four more
`sa` passwords in cleartext connection strings. **These are in version control and should be treated as
compromised.**

### `GAP-SEC-007` — Passwords are stored in plain text · **FINDING** · severity: **critical**

`tblUser.Password` is written and compared verbatim (`PanalClsDAL.Login`, `UserDAL.SaveUserInformation`). No
hash, no salt.

### `GAP-SEC-004` — The autocomplete web service is unauthenticated · **FINDING** · severity: high

All 18 methods on `SInventoryWebService.asmx` (exposed twice, also as `WebService.asmx`) execute a
`LIKE '%' + prefixText + '%'` query built by concatenation and check no session. They leak the full customer
master, product master, price list and employee list to any anonymous caller, and are injectable.

### `GAP-SEC-006` — Menu URLs are written into HTML unquoted · **FINDING** · severity: medium

`MainMasterPage.master.cs` emits `href=` + the raw `tblMainMenu.URL`, with no quoting and no encoding. A URL
value containing markup is stored XSS against every user who can see that menu item.

### `GAP-SEC-008` — Production configuration is debug configuration · **FINDING** · severity: high

`Web.config` has `<compilation debug="true">`, no `<customErrors>`, and a session timeout of 24,000 minutes
(≈16.6 days). Unhandled exceptions render a full stack trace, including the SQL that failed, to the browser.

### `GAP-SEC-009` — No horizontal access control · **FINDING** · severity: high

No page verifies that the record it is loading belongs to the user's company unit, or that the user had the
menu grant for the URL they requested. Keys travel in the query string and in `Session`. Changing an id in the
URL is not prevented.

---

## D. Findings — correctness and data integrity

### `GAP-APP-001` — `SignatureHandler.ashx` guards the wrong session key · **FINDING** · severity: low

```csharp
if ((context.Session["ImageBytes"]) != null) {
    byte[] image = (byte[])(context.Session["SigImageBytes"]);   // reads a different key
```

If only `SigImageBytes` is set the handler returns nothing; if only `ImageBytes` is set it throws on the cast.

### `GAP-APP-002` — Discount shown ≠ discount stored · **FINDING** · severity: high

The screen computes `discountAmount = (UnitPrice / 100) × pct` (per unit); `InvoiceBLL.InvoiceDetailInsertBLL`
stores `TotalPrice × pct / 100` (per line). They agree only when quantity is 1. See `BR-PRC-004`.

### `GAP-APP-003` — Product discount ignores its date argument · **FINDING** · severity: medium

`InvoiceDAL.DiscountofProduct(productCode, customerId, date)` never uses `date`. Quantity banding
(`MinQty`/`MaxQty`) and `Status='Active'`, present in the superseded version directly above it, are also gone.
See `BR-PRC-003`.

### `GAP-APP-004` — No stock availability check before invoicing · **FINDING** · severity: high

`InvoiceBLL.UpdateDCStock` allocates across batches and simply runs out; no error is raised and the invoice is
still written. See `BR-INV-002`.

### `GAP-APP-005` — Lost updates on stock · **FINDING** · severity: high

`UPDATE tblDCStore SET StockQty = '<value computed in C#>'` overwrites rather than decrementing, with no
optimistic-concurrency token. See `BR-INV-003`.

### `GAP-APP-006` — Application-generated primary keys race · **FINDING** · severity: high

`MAX(id)+1` read outside a transaction. See `BR-SYS-003`.

### `GAP-APP-007` — Document saves are not transactional · **FINDING** · severity: high

See `BR-SYS-004`.

### `GAP-APP-008` — `DB_Manager` swallows exceptions and reports success-shaped failure · **FINDING** · severity: high

`ExecuteNonQueryAction` catches, rolls back, closes and returns `0`. The caller cannot distinguish an error from
"no rows matched".

### `GAP-APP-009` — Composite lookup strings break on `:` and on absence · **FINDING** · severity: medium

See `BR-SYS-006`.

### `GAP-APP-010` — Grid cell indexes are positional and unchecked · **FINDING** · severity: medium

Every line-item screen uses `Rows[i].Cells[n].FindControl(...)`. Reordering a column compiles cleanly and fails
at run time.

### `GAP-APP-011` — Invoice-number parsing depends on unit-id width · **FINDING** · severity: medium

`SUBSTRING(InvoiceNo, 11, 12)` when `ComUnitId > 99`, else `SUBSTRING(InvoiceNo, 10, 11)`. A unit crossing 99
changes the parse offset for its whole history. See `BR-SYS-001`.

### `GAP-APP-012` — Locale-dependent date concatenation · **FINDING** · severity: medium

Dates are concatenated into SQL with the server's current culture (e.g.
`"'" + DateTime.Today + "' <= Q.InactiveDate"` in `InvoiceDAL.ProductInfoDAL`). A server culture change silently
changes query semantics.

### `GAP-APP-013` — Unguarded numeric parsing · **FINDING** · severity: medium

`Convert.ToDecimal(textBox.Text)` with no `TryParse` and no server-side numeric validation
(`FilteredTextBoxExtender` is client-side only). Empty or non-numeric input throws.

### `GAP-APP-014` — Two `.asmx` endpoints expose the same class · **FINDING** · severity: low

`SInventoryWebService.asmx` and `WebService.asmx` both declare `Class="SInventoryWebService"`. One is dead but
still reachable.

### `GAP-APP-016` — `UpdateInvoicePaymentAmount` discards its amount and status · **FINDING** · severity: high

`CustomerPayment.aspx.cs` decides `"Full"` vs `"Partial"` and computes the cumulative paid amount, then calls
`CustPaymentBLL.UpdateInvoicePaymentAmount(amount, status, invoiceId)`. The DAL method passes **only
`@InvoiceId`** to `sp_UD_InvoicePayment` and ignores the other two arguments, so the entire screen-side
calculation is dead. The real behaviour lives in the procedure and is UNKNOWN.
See [finance-collection/workflows/WF-customer-payment.md](finance-collection/workflows/WF-customer-payment.md).

### `GAP-APP-017` — One receipt across N invoices creates N payment master rows · **FINDING** · severity: high

`CustomerPayment.aspx.cs::saveButton_Click` builds a fresh single-element detail list **inside** the row loop
and calls `CustPaymentBLL.SaveCustPayment` once per ticked invoice. Each call generates its own
`tblCustomerPay` row stamped with the **full** header payment amount. Paying five invoices with one receipt
therefore writes five masters each carrying the whole receipt value. Any report summing
`tblCustomerPay.PaymentAmount` over-counts. Whether this is the intended data model is UNKNOWN.

### `GAP-APP-018` — Delivery-confirmation success and failure show the same message · **FINDING** · severity: high

`DelivaryInvoiceCreationAuto.aspx.cs::gotoinvoiceButton_Click` posts the journal without checking whether the
confirmation procedure succeeded, and shows `"Delivery Invoice Save Successfully !!"` on both the success and
the fallback path. When both calls fail it shows **nothing at all**. The chart-of-accounts precondition on this
path is commented out. See
[delivery-logistics/workflows/WF-delivery-confirmation.md](delivery-logistics/workflows/WF-delivery-confirmation.md).

### `GAP-APP-019` — Status routing depends on list-item display text · **FINDING** · severity: high

Delivery confirmation branches on `statusDropDownList.SelectedItem.Text == "Partial"` / `"Full"`, with
**reject as the `else`**. Because the list is data-driven, renaming a status value routes every row to reject.
Payment validation similarly branches on the payment type's text (`"Cash"`, `"Bank"`).

### `GAP-APP-020` — Ineffective validation in `InvoiceCreationForCustomer` · **FINDING** · severity: medium

`Validation()` tests `payTypeDDL.Text == ""`, which reads the *selected item's text*. The placeholder item's
text is never empty, so the check can never fail; `Convert.ToInt32(payTypeDDL.SelectedValue)` then throws on
save. The same method does not verify that a customer was resolved or that any line exists.

### `GAP-APP-015` — Hard-coded warehouse id `15` · **FINDING** · severity: medium

`DCUserPermissionDal.GetDCList` unions warehouses in with a literal `15` as the unit id, collapsing every
warehouse to one id. Intent UNKNOWN.

---

## E. Structural gaps

### `GAP-STR-001` — Module boundaries are inferred · **OPEN**

See `GAP-SEC-001`. Once `tblMainMenu` is available, the eleven module folders in this specification should be
re-derived from `ModuleId`/`ParantId` and the page specs moved accordingly.

### `GAP-STR-002` — Nine pages in `CommonUI` may be dead · **OPEN**

`Default.aspx`, `TemplatePage.aspx`, `JqueryTab.aspx`, `JqueryAccordian.aspx`, `AdminMenuPanal.aspx` are
referenced by nothing in the repository. Whether they are in `tblMainMenu` is UNKNOWN.

### `GAP-STR-003` — Duplicated near-identical pages · **KNOWN**

`PaymentTranscation.aspx`, `PaymentTranscationNew.aspx` and `PaymentTranscationNew2.aspx` (1,375 / 1,541 /
1,514 lines) coexist, as do `SalesReport.aspx` / `SalesReports.aspx` and `SalesHubReport.aspx` /
`SalesHubReport2.aspx`. Which is live is decided by `tblMainMenu` — UNKNOWN. Each is specified separately;
none was assumed dead.

### `GAP-STR-006` — Seven pages are excluded from the build · **KNOWN**

`AreaWiseMonthlySalesReportViewer`, `CompanyWiseBranch`, `CustomerDueReport` (the `SInventory_RPTVIEW` copy),
`MIOWiseCustomer`, `ManualOrderCreation`, `SalesTrendReportViewer` and `WarehouseStockInEdit` exist only as
`.exclude` files, meaning they were removed from the Web Site project and are neither compiled nor served. Two
report artefacts (`crpCustomerDueReport.cs`, `rptInvoiceForCustomer.rpt`) are excluded the same way. They are
**not** counted in the 383 pages and have no spec. Full list in
[00-system/code-inventory.md](00-system/code-inventory.md).

Note `ManualOrderCreation.aspx` is excluded while `ManualOrderrCreation.aspx` (double *r*) is live — the
excluded one is the superseded original.

### `GAP-STR-004` — `PrecompiledWeb/` is stale build output in version control · **KNOWN**

It contains an older copy of the whole site. It was excluded from every count and every trace in this
specification. It should not be read as source.

### `GAP-STR-005` — Business logic sits in the UI layer · **KNOWN**

The BLL is largely pass-through; calculation and orchestration live in `.aspx.cs`. This is why sections 11 and
12 of each page spec carry most of the behavioural detail, and why the same logic is duplicated across the
several invoice-creation variants.

---

## F. Priority order for closing the gaps

| # | Do this | Closes |
|---|---|---|
| 1 | Rotate the `sa` password and every credential in `DB_Authentication.cs` and `Web.config` | `GAP-SEC-003` |
| 2 | Supply `sp_helptext` for all 119 procedures | `GAP-DB-001`, most `UNKNOWN`s in sections 16/19/21 |
| 3 | Supply the schema script | `GAP-DB-002`, Phase-11 column mapping |
| 4 | Supply the eight permission/menu `SELECT`s | `GAP-SEC-001`, `GAP-STR-001`, every section 20 |
| 5 | Confirm which of the duplicated pages are live | `GAP-STR-002`, `GAP-STR-003` |
| 6 | Export the Crystal report definitions | `GAP-RPT-001` |
| 7 | Business review of the INFERRED objectives in section 3 of each page spec | `GAP-DOC-001` |
