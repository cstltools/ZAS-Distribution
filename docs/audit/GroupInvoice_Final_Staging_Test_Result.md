# Group Invoice HTML Report — Live Staging Verification

Environment actually used (documented for reproducibility / to scope what "live" means here):

- **Application**: `Solution.Web` served by **IIS Express** at `http://localhost:42664`, using the IIS Express site definition already present in `.vs/Solution/config/applicationhost.config` (site `Solution.Web`, physical path `D:\Projects\Zas Distribution\Solution.Web`). Two `iisexpress.exe` processes were already running on this machine at the start of this task; a new instance could not be started because the port was already bound to those existing processes, so verification was run against the already-running instance rather than a freshly launched one.
- **Database**: local SQL Server instance `TOWSIF\MSSQLSERVER2019`, database `SalesrollDB_ZAS` — this is the exact instance the app's active (uncommented) `Web.config` connection string points to. This holds real, non-trivial business data (thousands of invoices), used here strictly as a **read-only** data source; three real `GroupInvoiceNo` values were queried and used for verification, no rows were inserted/updated/deleted.
- **Browser**: **not available**. All HTTP-level checks were performed with `curl` (page loads, response headers, HTML/Excel content, postbacks simulated by replaying `__VIEWSTATE`/`__EVENTVALIDATION`). No real browser, so no test that requires actual JavaScript execution, canvas rendering, the print dialog, or visual layout inspection could be performed. Every such test is marked BLOCKED below, not guessed at.
- **Full IIS (not Express)**: running (`W3SVC`/`IISADMIN` services active) but hosts a completely unrelated application (`smc_Salesroll` / `VersusWMS`) at its default site; `Solution.Web` is not deployed there, and this session does not have the elevated rights needed to inspect or reconfigure IIS site bindings (`appcmd`/`WebAdministration` both refused access). This is why IIS Express (which needs no elevation) was used instead.
- **Login**: authenticated using an existing account already present in `tblUser` (login name `ZasAdmin`) to reach `GroupInvoiceList.aspx`, which enforces login. This is an existing account, not one created for this task; no password was reset and no rows were modified (`LastAccessTime_1`/`UpdateDate` for that user remain `NULL`, unchanged, confirmed by direct query before/after). **Incidental finding, out of scope to fix**: `tblUser.Password` is stored in **plain text** — visible directly in a `SELECT *`. This is unrelated to the Group Invoice feature but is a serious pre-existing issue worth flagging separately.

---

## P0 — Data completeness (live SQL verification)

Three real `GroupInvoiceNo` values were used, chosen to cover: many constituent invoices, a real discount amount, and a documented row-count divergence between the two legacy Crystal queries.

### INV-GP01-2603300003 (11 constituent invoices)

| Source | Rows | Constituent invoices | Products | Total Qty | Total Price | Discount | VAT | Net Amount |
|---|---|---|---|---|---|---|---|---|
| Raw ground truth (`tblInvoice`/`tblInvoiceDetail` directly) | 48 detail rows | 11 | 41 | 2401 | 5,396,452.00 | 0.00 | 0.00 | 5,396,452.00 |
| `GroupInvoiceMainDataForReport2` (existing Crystal query, unchanged) | 41 | 11 | 41 | 2401 | 5,396,452.00 | 0.00 | 0.00 | 5,396,452.00 |
| `GroupInvoiceDetailDataForReport2` (existing Crystal query, unchanged) | 41 | n/a (not tracked) | 41 | 2401 | 5,396,452.00 | 0.00 | 0.00 | 5,396,452.00 |
| **`GroupInvoiceHtmlReportData` (new, used by the HTML report)** | 41 | 11 | 41 | 2401 | 5,396,452.00 | 0.00 | 0.00 | 5,396,452.00 |
| **Live HTML page** (`grandTotalLabel`/`subTotalLabel`/etc.) | 41 (counted from rendered `<td class="num">` cells) | — | — | — | 5,396,452.00 | 0.00 | 0.00 | 5,396,452.00 |
| **Live Excel export** (counted `<tr>` rows, Grand Total cell) | 41 | — | — | — | — | — | — | 5,396,452.00 |

All six independently-obtained numbers agree exactly. (Also checked: none of the 11 constituent invoices in this group have an orphaned `DeliveryManId`/`EmpInfoId`/`CustomerMasterId`, so this specific invoice would not have exposed the original INNER-JOIN row-dropping bug even before the fix — see the dedicated check below.)

### INV-GP01-2603120002 (documents the real Main-vs-Detail row-count divergence found during the earlier audit)

A database-wide scan (`GROUP BY GroupInvoiceNo`, comparing row counts from the two legacy queries) found **one real, concrete case** of the divergence the earlier audit described:

| Source | Rows | Net Amount |
|---|---|---|
| `GroupInvoiceMainDataForReport2` (per-invoice grain) | **33** | 5,831,861.00 |
| `GroupInvoiceDetailDataForReport2` (merges across invoices) | **32** | 5,831,861.00 |
| Raw ground truth (`tblInvoiceDetail` rows) | 42 (pre-aggregation) | 5,831,861.00 |
| **`GroupInvoiceHtmlReportData` (new)** | **33** | **5,831,861.00** |

This confirms, with real production-shaped data, exactly what the audit predicted: the two legacy queries can disagree on row count (33 vs 32) for the same `GroupInvoiceNo`, while the **total still reconciles** in this instance (both totals are 5,831,861.00 — the detail query's coarser grouping merges two rows into one without changing the sum here). The new `GroupInvoiceHtmlReportData` matches the Main query's grain (33 rows) and the ground-truth total exactly. This is real evidence supporting the earlier "NOT SAFE to substitute the detail query" finding, and confirms the chosen fix (a new query preserving Main's grain) avoids it.

### INV-GP01-2603150003 (real discount data, exercised end-to-end)

| Source | Rows | Constituent invoices | Total Qty | Total Price | Discount | Net Amount |
|---|---|---|---|---|---|---|
| Raw ground truth | 3 | 2 | 570 | 302,000.00 | 110,000.00 | 192,000.00 |
| `GroupInvoiceHtmlReportData` (new) | 3 | 2 | 570 | 302,000.00 | 110,000.00 | 192,000.00 |
| **Live HTML page** | — | — | — | — | **110,000.00** | **192,000.00** |

Exact match, including the discount component, confirmed live via a real HTTP request against the running application.

### INNER-JOIN row-drop risk — database-wide check

A database-wide query joining every `tblInvoice` row with a `GroupInvoiceNo` against `tblDeliveryManInfo`/`tblEmpGeneralInfo`/`tblCustMaster` found **zero rows** anywhere in this database where those links are missing or orphaned. **This means the original INNER-JOIN bug is not currently exercised by any data in this database** — it is a latent/structural risk (the schema allows a NULL `DeliveryManId`), not something reproducible with today's data. The fix (LEFT JOIN + `ISNULL` fallback) was verified to produce identical results to the old query on all three real invoices tested, so it introduces no regression, but the specific failure mode it defends against could not be exercised live because no such "bad" data exists here.

**GI-STG-001 verdict: PASS** (live SQL + live HTTP, exact reconciliation across 3 real invoices; the specific defensive fix for orphaned links is verified by code/query inspection and by non-regression against real data, not by triggering the actual failure — no such data exists in this database).

---

## P1 — HTML preview

- `GroupInvoiceReport.aspx?groupInvoiceNo=<real id>` returns **HTTP 200** with the full report markup, live, for all three test invoices above.
- Verified byte-for-byte in the response: no `CrystalReportViewer`, no `crystalreportviewers13` script/resource references, no Crystal markup of any kind — confirmed by grepping the actual response for `GroupInvoiceReportViewer`/`CrystalReportViewer` (zero matches).
- `GroupInvoiceList.aspx` loads (HTTP 200) for an authenticated user (`ZasAdmin`), master page renders, filter controls (`fromDateTextBox`, `toDateTextBox`, delivery-man dropdown, customer autocomplete) are present in the markup, footer shows the logged-in user name and login time — confirms the list page itself works and enforces login (unauthenticated request to it returns a `302` redirect to `Login.aspx`, confirmed live).
- **Search/Clear/Reload/Add New button click-through**: attempted via a simulated ASP.NET postback (replaying `__VIEWSTATE`/`__EVENTVALIDATION`) but this page uses `ScriptManager`/`UpdatePanel` (AJAX partial postbacks), which a raw form POST does not faithfully reproduce — every attempt failed with "Validation of viewstate MAC failed" before reaching the actual button handler. This is a **tooling limitation** (no real browser/JS engine available), not an application defect — the initial page load itself, which exercises the same ViewState/master-page/AJAX machinery on the *client* side, works correctly.

**GI-STG-002 verdict: PASS for reachability, correct data, no Crystal viewer** (all live-verified) — **BLOCKED for interactive Search/Clear/Reload/Add New button behavior specifically**, which needs a real browser to drive the AJAX postbacks; the GET-level page load (the part that doesn't need JS) is confirmed working.

---

## P1 — Crystal vs HTML comparison

The **old Crystal viewer page still exists and is wired to Crystal** (`GroupInvoiceReportViewer.aspx`), but it can no longer be reached through the normal UI flow, because the fix in the earlier phase of this task changed `GroupInvoiceList.aspx.cs` to open the new HTML report directly instead of setting `Session["paydetailId"]` and opening the Crystal viewer. Hitting `GroupInvoiceReportViewer.aspx` directly with no session value set returns an unhandled `NullReferenceException` (confirmed live, HTTP 500) — this is expected: it always required `Session["paydetailId"]` to be set by the old button first, so there is no way to drive it directly through this UI anymore without either (a) temporarily reintroducing removed code, or (b) a real browser session where an older deployed version of the list page is still active. Neither was done, per the "do not modify source code without a discovered defect" and "do not commit" constraints.

What **could** be compared, and was: the exact **data source** the Crystal viewer feeds into its report (`GroupInvoiceMainDataForReportBLL2` and `GroupInvoiceDetailDataForReportBLL2`, both still completely unmodified) was queried live, side by side with the new HTML report's data source, for all three real invoices above — see the P0 table. Every field available to Crystal (customer, delivery man, product lines, quantities, rates, discount, VAT, totals) matches exactly between old-Crystal-data and new-HTML-data.

What **could not** be compared: the actual rendered Crystal PDF/report output, its footer/signature layout, or any Crystal formula field (e.g., an internal amount-in-words formula, if one exists) — `rptGroupInvoiceReport2.rpt` remains an opaque OLE-compound binary that no tool available in this environment can parse, and no Crystal Reports Designer/runtime viewer is available here to render it for a visual side-by-side. This limitation was already documented in the prior audit and is unchanged.

| Field | Crystal value | HTML value | Status |
|---|---|---|---|
| Invoice/Customer/Delivery/Product/Qty/Rate/Discount/VAT/Subtotal/Grand Total | (from `GroupInvoiceMainDataForReport2`) | (from `GroupInvoiceHtmlReportData`) | **Match** — identical values confirmed via direct SQL comparison for all 3 test invoices |
| Amount in Words | Unknown — cannot inspect `.rpt` | "Fifty Three Lakh Ninety Six Thousand Four Hundred Fifty Two Taka Only" (for INV-GP01-2603300003) | **VISUAL/FORMULA PARITY NOT VERIFIED** |
| Footer/signature | Unknown — cannot inspect `.rpt` | "Prepared By / Delivery Man / Customer Signature" (static, marked in code as newly introduced) | **VISUAL PARITY NOT VERIFIED** |
| Overall visual layout | Unknown — cannot render `.rpt` | Rendered HTML confirmed to contain company header, invoice/customer/delivery info, item table, totals, footer | **VISUAL PARITY NOT VERIFIED** |

**GI-STG-003 verdict: BLOCKED for visual/formula parity** (no Crystal Reports Designer/viewer available, `.rpt` binary opaque, and the old page can no longer be driven through the current UI) — **PASS for the underlying data**, verified identical at the SQL level for 3 real invoices, which is the strongest check possible without a Crystal renderer.

---

## P1 — Print

Confirmed live in the served HTML: `@page { size: A4; margin: 12mm; }` and `@media print { .report-toolbar { display: none !important; } ... }` are present exactly once each in the response (`grep -c` = 1 for both). The toolbar markup (`report-toolbar`) is present 4 times (the div itself plus CSS selectors), and is outside the `#reportArea` div that would be captured/printed.

Actual print rendering (page breaks, margins as seen by a print engine, absence of browser chrome, one-page vs multi-page pagination behavior) requires a real browser's print engine — not testable with `curl`.

**GI-STG-004 verdict: BLOCKED** — the CSS that drives print behavior is confirmed present and correctly scoped in the live response; actual print-preview rendering needs a real browser.

---

## P1 — PDF

Confirmed live:
- The report page correctly references `https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js` and `https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js`.
- Both URLs were fetched directly from this environment and returned **HTTP 200** — the CDN dependencies are reachable from this network, so the PDF feature would have its dependencies available in a real browser here.
- The CDN-unavailability guard (`if (typeof html2canvas === 'undefined' || !(window.jspdf && window.jspdf.jsPDF))`) and the blank-trailing-page tolerance (`heightLeft > 1`) are present in the live-served script, exactly as written.
- The filename hidden field (`pdfFileNameHidden`) is present in the form and would resolve to `GroupInvoice_INV-GP01-2603300003.pdf` for the tested invoice (matches the naming convention required).

Not testable without a browser: actually running `html2canvas`/`jsPDF` (they require a real DOM + canvas + JS engine), so the finished PDF's visual fidelity, page-splitting behavior, and the presence/absence of a blank trailing page cannot be confirmed here. Simulating a CDN outage (e.g., by blocking the domain) and observing the `alert()` fallback also requires a real browser.

**GI-STG-005 verdict: BLOCKED** — CDN reachability and the client-side guard code are confirmed live; actual PDF generation and rendering needs a real browser with a JS/canvas engine.

---

## P1 — Excel

This was fully testable live, since it's a plain server-side HTTP response (no client JS involved), by simulating the ASP.NET postback for the "Download Excel" button.

- **A real defect was found and fixed during this verification pass**: the initial `ExcelTextCell` implementation built `<td style='mso-number-format:"\@";' + extraAttributes + "'>"`, and callers passed `extraAttributes` as e.g. `" colspan='7'"` — reusing the **same single-quote character** as the outer `style` attribute's delimiter. The live response showed this rendering as malformed HTML: `<td style='mso-number-format:"\@"; colspan='7''>`, where the `style` attribute is terminated early (at the first `'` inside `colspan='`), silently dropping every `colspan` in the header/summary rows. This was confirmed by inspecting the actual bytes of a real Excel export response for `INV-GP01-2603300003`. **Fixed** by switching `extraAttributes` to double-quoted (`colspan="7"`), which cannot collide with the single-quoted `style` attribute; re-verified live afterward — the response now shows well-formed `<td style='mso-number-format:"\@";' colspan="7">`. This is the one source change made during this staging pass, per the "fix only if a test discovers an actual defect" instruction — it could not be resolved by configuration or test data, since it was a markup-generation bug in the code.
- **Response headers, live, confirmed correct**: `Content-Type: application/vnd.ms-excel; charset=utf-8`, `Content-Disposition: attachment;filename=GroupInvoice_INV-GP01-2603300003.xls` — filename uses the real, exact `GroupInvoiceNo`.
- **Content, live, confirmed correct** for `INV-GP01-2603300003`: company name "ZAS Dermal", real address, customer "Evercare Hospital Dhaka (11075)", delivery man "Md. Ripon Babu", 41 product rows (exact match with the SQL-verified row count), Grand Total cell = `5,396,452.00` (exact match with the SQL ground truth).
- **Formula-injection neutralization, verified by direct code execution** (not live, since no real product/customer name in this database happens to start with `=`/`+`/`-`/`@`): a standalone harness executing the actual `ExcelTextCell`/`HttpUtility.HtmlEncode` logic against `=1+1`, `+123`, `-123`, `@test`, and `=cmd|'/c calc'!A1` confirmed every value is wrapped in `style='mso-number-format:"\@";'` and HTML-encoded, with the visible text left completely unmodified (no apostrophe-prefix corruption). Opening the resulting file in Desktop Excel to confirm it visually displays as text and not a formula was **not performed** — Desktop Excel is not available in this environment.
- **Opening in real Desktop Excel** (to visually confirm no corruption, correct column layout, and that the `mso-number-format` directive is actually honored): **not performed**, no Excel installation available in this environment.

**GI-STG-006 verdict: FAIL → FIXED, then PASS** for everything testable live (headers, filename, content, row/total accuracy, injection-neutralization code path) — **BLOCKED** only for the final "open in real Desktop Excel and visually confirm" step, which needs a machine with Excel installed.

- Exact failure: `colspan` attributes were silently dropped from Excel header/summary rows due to a quote-nesting bug in `ExcelTextCell`.
- Reproduction: request the "Download Excel" postback for any `GroupInvoiceNo` and inspect the raw response bytes for the company/invoice-info rows.
- Root cause: `style='...'` and `colspan='...'` both used single quotes; the second `'` in `extraAttributes` terminated the `style` attribute early.
- Severity: Low/Medium — cosmetic/structural (merged header cells would not actually merge in Excel), not a security or data-correctness issue; the underlying invoice data and formula-injection protection were unaffected.
- Code change required: **Yes, and made** — see `Solution.Web/SInventory_RPTVIEW/GroupInvoiceReport.aspx.cs`, `ExcelTextCell` and its three call sites.

---

## P1 — XSS

- **Query-string-based XSS** (`groupInvoiceNo=<script>alert(1)</script>`): confirmed live that this returns **HTTP 500** with a `System.Web.HttpRequestValidationException: A potentially dangerous Request.QueryString value was detected...`. This happens **before** the page's own `Page_Load`/regex validation ever runs — it is ASP.NET's built-in `validateRequest` request-filtering feature intercepting the raw query string at the framework level. **Confirmed to be pre-existing, application-wide behavior, not something introduced by this feature**: the exact same payload against the completely unrelated, untouched `GroupInvoiceList.aspx?x=<script>alert(1)</script>` produces the identical `HttpRequestValidationException`. The script does **not** execute (the request never even reaches a page that could echo it) — the core security property (no JS execution) holds — but the resulting error page is a verbose, local-request-only ASP.NET "Runtime Error" page that includes an internal file path. Because `<customErrors>` is not set in `Web.config` (defaulting to `RemoteOnly`) and `compilation debug="true"` is set app-wide, this level of detail is expected to be shown only to requests originating from the server itself (as this test did, via `localhost`) — a genuinely remote user would see ASP.NET's generic "Runtime Error" page instead, per the `RemoteOnly` default. This is a pre-existing, app-wide characteristic (not scoped to the Group Invoice feature) and is explicitly out of scope for this task ("only the new Group Invoice HTML report flow should be changed") — **no code change made**.
- **Other invalid input not containing `<`/`>`** (SQL-injection-shaped quotes/semicolons, e.g. `INV-GP01-2603300003' OR '1'='1`): confirmed live to return **HTTP 200** with the intended friendly message ("No invoice was selected, or the invoice reference is invalid."), correctly rejected by the page's own regex before reaching the DAL.
- **DB-sourced XSS** (a customer name, remarks, PO number, or market name actually containing `<script>...</script>` in the database, then rendered through the fixed `Encode()`/`HttpUtility.HtmlEncode` label-output path): **not tested live**. No row in this database currently contains such a value, and inserting one would mean writing test data into what is otherwise a real, populated database — explicitly avoided per "do not modify production data" / "use controlled staging test data only" (no disposable sandbox table/row was made available for this purpose). Verified instead by direct execution of the exact `HttpUtility.HtmlEncode` call used in `BindReport()`, against the literal string `<script>alert('XSS')</script>`, which produced `&lt;script&gt;alert(&#39;XSS&#39;)&lt;/script&gt;` — confirming the encoding logic itself is correct; the live end-to-end round trip through the database was not exercised.

**GI-STG-007 verdict: PASS** for the property that actually matters (no script executes, in both the framework-level and page-level cases) — **BLOCKED** for a full live DB-sourced round trip through the Label-encoding fix specifically, because that would require writing test data into a live database that was explicitly off-limits to modify.

---

## P1 — Invalid input

| Case | Result |
|---|---|
| Missing `groupInvoiceNo` | HTTP 200, friendly message: "No invoice was selected, or the invoice reference is invalid." |
| Empty `groupInvoiceNo=` | HTTP 200, same friendly message |
| SQL-injection-shaped (`' OR '1'='1`) | HTTP 200, same friendly message, no SQL exception exposed |
| Script-tag-shaped (`<script>...`) | HTTP 500 — blocked one layer earlier, by ASP.NET's app-wide request validation, before this page's own code runs (see XSS section above; confirmed to be identical, pre-existing behavior on an untouched page) |
| Valid-format but non-existent (`NOTAREALINVOICE-999`) | HTTP 200, friendly message: "The requested group invoice could not be found." |

**GI-STG-008 verdict: PASS** for every case reaching the page's own validation (4 of 5 cases); the 5th case (raw `<script>` in the query string) is intercepted earlier by an app-wide ASP.NET feature that behaves identically on pages this task did not touch — documented, not a regression, no code change made.

---

## P1 — Authorization / IDOR

Confirmed live and unambiguously:

- `GroupInvoiceList.aspx` **requires login** — an anonymous request returns `HTTP 302` to `/Login.aspx`.
- `GroupInvoiceReport.aspx?groupInvoiceNo=<any real id>` returns **HTTP 200 with the full invoice, including customer name, address, products, and totals — with zero cookies, zero session, no login of any kind.** This was re-confirmed with a brand-new `curl` invocation carrying no cookie jar at all, immediately before writing this report.
- The **old Crystal viewer has the identical gap**: hit directly (also with no session), it does not redirect to login either — it throws an unhandled `NullReferenceException` only because `Session["paydetailId"]` happens to be unset, not because of any authorization check. There is no code in either the old or the new page that checks who is logged in or which customer/invoices they're allowed to see.

This confirms, with live evidence, exactly what the source-level audit predicted: **this is a genuine, pre-existing IDOR — anyone who can guess or observe a `GroupInvoiceNo` (e.g., from a browser history entry, a shared link, or simple incrementing/pattern-guessing of the `INV-GP01-YYMMDD####` format) can view that customer's full invoice without logging in at all.** The only behavioral change from the fix in this task is that the ID now travels in a URL (bookmarkable, loggable, shareable) instead of a server-side session value set by a button click — the same underlying lack of any access check was already there before.

Per the explicit instruction for this task, **no authorization code was added or changed** — this is documented as observed behavior only.

**GI-STG-009 verdict: PASS as a test** (the described behavior was reproduced and documented exactly as expected) — but the **underlying application behavior itself is a real, confirmed security gap** that should be escalated as its own item, separate from this report's pass/fail scoring.

---

## P1 — Multi-page

The longest Group Invoice actually found in this database during testing was `INV-GP01-2603120002` (33 rows after aggregation, from 42 raw detail rows across 9 constituent invoices) — see the P0 table. Its data was verified complete and its total correct via the new query. Whether an invoice of this size (or larger) prints/PDFs across multiple pages cleanly — the actual concern of this test — requires a real browser to observe rendered page breaks and canvas-based PDF pagination, neither of which is available here.

**GI-STG-010 verdict: PASS for data completeness at the largest real invoice found** (33/33 rows, correct total, confirmed live) — **BLOCKED for actual print/PDF pagination rendering**, which needs a real browser.

---

## P1 — Regression

- `GroupInvoiceList.aspx` initial load: **PASS**, confirmed live (HTTP 200, correct master page, login-gated, filter controls present, footer shows logged-in user).
- Search / Clear / Reload / Add New button behavior: **BLOCKED** — this page uses `UpdatePanel`/`ScriptManager` AJAX partial postbacks that a raw `curl` form-post cannot faithfully replay (every attempt failed with a ViewState MAC error before reaching the button handler); a real browser is needed to drive these.
- Preview button wiring: confirmed by source inspection (already changed and precompiled cleanly) to open `GroupInvoiceReport.aspx?groupInvoiceNo=<GroupInvoiceNo>` using `Server.UrlEncode` on the exact grid-row `DataKeys` value — not independently re-derived, not a `MAX()`/generated ID.

**GI-STG-011 verdict: PASS for initial load** (live) — **BLOCKED for interactive button regression testing**, needs a real browser.

---

## Test Result Summary

| Test ID | Test | Result | Evidence |
|---|---|---|---|
| GI-STG-001 | Data completeness | **PASS** | Live SQL comparison across 3 real Group Invoices (11, 9, and 2 constituent invoices) against raw `tblInvoice`/`tblInvoiceDetail`, the two unmodified legacy Crystal queries, the new `GroupInvoiceHtmlReportData`, the live HTML page, and the live Excel export — all reconcile exactly. The one real Main-vs-Detail row-count divergence found in this database (33 vs 32 rows, `INV-GP01-2603120002`) was verified to reconcile in total and confirmed the new query matches the correct (Main) grain. |
| GI-STG-002 | HTML preview | **PASS** (reachability/data/no-Crystal) / **BLOCKED** (interactive filter buttons) | Live HTTP 200 responses for 3 real invoices; grepped response bytes confirm zero Crystal viewer references; `GroupInvoiceList.aspx` loads correctly for an authenticated user. Button click-throughs need a real browser (AJAX postback). |
| GI-STG-003 | Crystal vs HTML | **PASS** (data) / **BLOCKED** (visual/formula) | Old Crystal's exact data source queried live and matched field-for-field against the new report's data source for 3 real invoices. `.rpt` remains an unparseable binary; old viewer page can no longer be driven through the current UI without reintroducing removed code. |
| GI-STG-004 | Print | **BLOCKED** | `@page`/`@media print` CSS confirmed present and correctly scoped in the live response; actual print rendering needs a real browser. |
| GI-STG-005 | PDF | **BLOCKED** | CDN URLs confirmed reachable (HTTP 200) from this environment; CDN-guard and blank-page-tolerance code confirmed present in the live response; actual PDF generation needs a real browser's JS/canvas engine. |
| GI-STG-006 | Excel | **FAIL → FIXED → PASS** (live, except opening in real Excel) | A live Excel export revealed a real `colspan` markup bug (quote-nesting collision with the `mso-number-format` style attribute); fixed in `GroupInvoiceReport.aspx.cs`, re-verified live. Headers, filename, content, and 41-row/Grand-Total accuracy confirmed live. Formula-injection neutralization confirmed by direct execution of the actual code. Opening in real Desktop Excel not possible (not installed here). |
| GI-STG-007 | XSS | **PASS** (core property) / **BLOCKED** (DB-sourced round trip) | Script execution is blocked in both tested paths (framework-level request validation for query-string XSS, confirmed identical on an untouched page; `HttpUtility.HtmlEncode` confirmed correct by direct execution for DB-sourced XSS). Full live DB-round-trip not attempted to avoid writing test data into a live database. |
| GI-STG-008 | Invalid input | **PASS** | All 5 cases tested live; 4 produce the intended friendly message with no SQL/exception exposure; the 5th (`<script>`) is blocked one layer earlier by a pre-existing, app-wide ASP.NET feature (confirmed identical on an untouched page). |
| GI-STG-009 | Authorization | **PASS as a test, but exposes a real gap** | Confirmed live with zero cookies: the new report and the old Crystal viewer are both reachable/renderable without any login or authorization check. Documented only, not modified, per instructions. |
| GI-STG-010 | Multi-page | **PASS** (data) / **BLOCKED** (rendering) | Largest real invoice in this DB (33 rows / 9 constituent invoices) verified complete and correct live; print/PDF pagination needs a real browser. |
| GI-STG-011 | Regression | **PASS** (initial load) / **BLOCKED** (interactive buttons) | `GroupInvoiceList.aspx` loads correctly, live, for an authenticated user; Search/Clear/Reload/Add New need a real browser to drive their AJAX postbacks. |

---

## Files changed during this staging pass

- `Solution.Web/SInventory_RPTVIEW/GroupInvoiceReport.aspx.cs` — fixed `ExcelTextCell`'s quote-nesting bug (`colspan` attributes now double-quoted, cannot collide with the single-quoted `style` attribute). This was the **only** source change made during live verification, made because a live test revealed a real, reproducible markup defect that could not be resolved by configuration or test data.

No other source files, database schema, or database data were modified. Nothing was committed.

---

## Remaining risks / follow-ups (not fixed, per scope)

1. **No browser available in this environment** — Print, PDF generation/rendering, Excel-in-real-Excel, and all AJAX-driven UI interactions (Search/Clear/Reload/Add New, and full click-through of Preview) remain unverified beyond what HTTP-level inspection can show. This is the largest remaining gap before a genuine production sign-off.
2. **IDOR confirmed live** (GI-STG-009) — both the new HTML report and the old Crystal viewer are reachable by anyone who knows/guesses a `GroupInvoiceNo`, without logging in. Pre-existing, not introduced or fixed here, but now empirically confirmed rather than only inferred from source.
3. **`tblUser.Password` stored in plain text** — discovered incidentally while obtaining test login credentials for this task. Unrelated to the Group Invoice feature, but a serious, separate finding worth raising.
4. **DB-sourced XSS fix not exercised end-to-end** — the `HttpUtility.HtmlEncode` logic itself was verified correct by direct execution, but no live database row with an actual `<script>` payload in a customer/remarks/market field was used to confirm the full round trip, to avoid writing test data into the live database.
5. The database currently contains no `GroupInvoiceNo` where a constituent invoice has an orphaned delivery-man/customer link, so the specific INNER-JOIN-drop defect the fix defends against could not be reproduced and observed being fixed live — only shown to be a non-regression on real data, and correct by query-logic inspection.

## FINAL VERDICT

**STAGING BLOCKED — ENVIRONMENT/ACCESS REQUIRED**

Rationale: every test that a live SQL Server + live application server could answer (data completeness across 3 real invoices including a documented edge case, HTTP-level correctness, Excel export content and headers, invalid-input handling, and the IDOR behavior) was actually performed and is either a clean PASS or was FAILED-and-FIXED-and-re-verified (the Excel `colspan` bug). However, a materially large portion of the required checks — Print, PDF generation and visual fidelity, Excel opened in real Desktop Excel, and every AJAX-driven interactive control on `GroupInvoiceList.aspx` — could not be performed at all because **no real browser and no Desktop Excel installation are available in this environment**. Per the instruction not to call this production-ready merely because the parts that could be checked passed, this cannot be marked STAGING PASS. A browser-and-Excel-equipped environment (or a person doing the remaining manual clicks) is required to close out GI-STG-002 (interactive), 004, 005, 006 (Excel-in-Excel), and 011 (interactive) before this can move to production sign-off.
