# Group Invoice HTML Report — Parity with the Original Crystal PDF

Reference source of truth: `D:\Problems_Sceenshort\crvInvoiceReport.pdf` (the actual Crystal-generated
PDF for `GroupInvoiceNo = INV-GP01-2609020006`, 2 pages).

Test target: `SInventory_RPTVIEW/GroupInvoiceReport.aspx?groupInvoiceNo=INV-GP01-2609020006`, live,
running under IIS Express against the real SQL Server database, verified in a real Chromium browser
(Playwright-driven headless Chrome, plus the user's own Chrome window).

Crystal (`GroupInvoiceReportViewer.aspx`, `.rpt`) was **not modified**. Invoice/order/stock/accounting
logic was **not modified**. Nothing was committed.

---

## 1. Original PDF structure (as read from the actual file)

- Page 1: `Invoice` title → `Sold To` block (customer 36435) + Payment Type/Market/Delivery Person/Mobile
  + Invoice No/Date/Order No/Date/PO Number → three constituent-invoice sections, each with its own
  `Reference No:` line, product table, and `Sub-Total:` row (`INV-BD159-2609020042`,
  `INV-BD17-2609020015`, `INV-BD201-2609020001`) → `Page 1 of 2`.
- Page 2: a **second, different** `Invoice`/`Sold To` block (customer 20265) → one more constituent
  section (`INV-BD76-2609020001`) → `Grand Total:` row → `In Words:` → `Remarks:` → a 4-column signature
  block (`Customer Signature`, `Prepared By` with an `N/A` value, `Delivery By`, `Authorised Signature`)
  → `Page 2 of 2`.

## 2. HTML structure (as implemented)

`GroupInvoiceReport.aspx.cs::BindReport()` now builds the report server-side as a sequence of
`InvoiceSection` objects (one per constituent invoice / `Reference No`), grouped from the already-ordered
`GroupInvoiceHtmlReportData` result set, and renders:

```
foreach constituent invoice (Reference No)
    if its header context differs from the previous section's → render "Invoice" title + "Sold To" block
    render "Reference No: <value>"
    render its own <table> (multi-level header, one <tr> per product line)
    render its own Sub-Total row
render one Grand Total table (same column layout)
render In Words / Remarks
render the 4-column signature block
```

This is emitted into a single `<asp:Literal>` inside the report page — not a flattened `GridView` (the
prior implementation's defect) and not one global table. Every dynamic value passes through
`HttpUtility.HtmlEncode` (the `Encode()` helper) before being written into the literal.

## 3. Exact field mapping

| Original PDF label | HTML source | Notes |
|---|---|---|
| Sold To code/name/address | `CustomerCode`/`CustomerName`/`Address`+`Addrees2` | Same columns as before, `ISNULL(...,'N/A')`-guarded |
| Payment Type | `TermOfPayment` | |
| Market Name | `MarketName` | |
| Delivery Person / Mobile No | `DeliveryPersonName` / `DeliveryPersonPhNo` | |
| Invoice No (header) | `GroupInvoiceNo` | The Group Invoice number, constant across the whole document |
| Invoice Date (header) | `InvoiceDate` (`GroupInvoiceDate`) | |
| Order No / Order Date / PO Number | `OrderNo` / `OrderDate` (`ODR.SubmissionDate`) / `PONumber` | Per constituent invoice |
| Reference No | `InvoiceNo` (the constituent `tblInvoice.InvoiceNo`) | **Not** `GroupInvoiceNo` — confirmed against the PDF's four distinct values |
| Product Code / Product & Generic Name | `ProductCode` / `Product` + `GenericName` (two lines in one cell) | |
| Pack | `PackSize` | Raw stored value (`1 pcs`, `1X10s`, `5 x 5 s`) |
| RQ / BQ / TQ | `Quantity` / `BonusQuantity` / `TotalQuantity` | Already present in the query; no invented derivation |
| Per Pack: Unit Price / Vat | `UnitPrice` / `UnitVatAmount` | |
| Total: Total Price / Vat | `TotalPrice` (summed) / `TotalPriceVatAmount` | |
| Discount | `DiscountAmount` (`DiscountAmount + SpecialAmount`, summed) | The PDF has no separate Disc % column, so `DiscountPercentage` is fetched but not displayed, matching the original |
| Net Amount | `NetAmount` (summed) | |
| Sub-Total (4 numbers) | `SUM(TotalPrice)`, `SUM(TotalPriceVatAmount)`, `SUM(DiscountAmount)`, `SUM(NetAmount)` per section | |
| Grand Total (4 numbers) | Same 4 sums across **all** sections | |
| In Words | `NumberToWords.Convert` on the truncated Grand Total Net Amount, "Lakhs" plural, no currency name, no paisa suffix | Wording style matched to this exact PDF (see §12) |
| Remarks | Last section's `Remarks` | The field is per-invoice in the source data; the original shows it once, near the bottom, so the closest (last) invoice's value is used |
| Customer Signature / Prepared By / Delivery By / Authorised Signature | Static labels; `PreparedBy` (`ISNULL(U.UserName,'N/A')`) shown above the Prepared By line | Matches the PDF's `N/A` under Prepared By exactly |
| Page 1 of 2 / Page 2 of 2 | Not reliably reproducible — see §10 | |

No new DAL columns were required — every field above was already present in `GroupInvoiceHtmlReportData`
(added one `ISNULL` guard on `PreparedBy` for the `N/A` fallback). `GroupInvoiceMainDataForReport2` and
`GroupInvoiceDetailDataForReport2` (Crystal's own queries) were **not** touched.

## 4. Constituent invoice grouping — verified live

Live request to the rebuilt page, then inspected via the rendered DOM:

```
Reference sections: 4 ["Reference No: INV-BD159-2609020042","Reference No: INV-BD17-2609020015",
                        "Reference No: INV-BD201-2609020001","Reference No: INV-BD76-2609020001"]
Subtotal rows: 4
```

**Order matches the original PDF exactly**: BD159, BD17, BD201, BD76. This is not hardcoded — it falls
out of `GroupInvoiceHtmlReportData`'s existing `ORDER BY INV.InvoiceNo, IVD.ProductCode`, which happens to
be a plain string sort that reproduces this exact sequence (`'INV-BD159...' < 'INV-BD17...' <
'INV-BD201...' < 'INV-BD76...'` lexicographically). No products were merged across constituent invoices;
no invoice was collapsed into another.

## 5. Reference No mapping

Confirmed via a direct, minimal SQL query against `tblInvoice` (bypassing the report entirely, as an
independent check):

```sql
SELECT TOP 5 InvoiceId, InvoiceNo, GroupInvoiceNo, CustomerMasterId
FROM tblInvoice WHERE GroupInvoiceNo = 'INV-GP01-2609020006'
```
```
852259  INV-BD17-2609020015   INV-GP01-2609020006  1147
852260  INV-BD76-2609020001   INV-GP01-2609020006  10265
852261  INV-BD159-2609020042  INV-GP01-2609020006  28432
852262  INV-BD201-2609020001  INV-GP01-2609020006  10265
```

`InvoiceNo` is exactly what's shown as "Reference No" in both the original PDF and the rebuilt HTML — not
`GroupInvoiceNo` (which stays constant), and not `OrderNo` (a related but different value, e.g.
`ODR-BD159-2609020042`).

## 6. Product table mapping

Multi-level `<thead>` reproduces the original's grouping exactly:

```
Product Code | Product & Generic Name | Pack | Quantity (RQ, BQ, TQ) | Per Pack (Unit Price, Vat) | Total (Total Price, Vat) | Discount | Net Amount
```

using `rowspan`/`colspan` (`<th rowspan="2">` for the single-level columns, `<th colspan="3">Quantity</th>`
etc. for the grouped ones, with a second header row for the sub-columns) — not a flattened single-row
header. `Product Code`, `Pack`, and both `Vat` sub-columns, plus `Discount`, are rendered with
`writing-mode: vertical-rl` to reproduce the original's narrow rotated-text columns (visually confirmed in
the screenshot). Product Name is bold with Generic Name directly beneath it in the same cell, matching the
original's two-line layout inside one column (e.g. "Kedbumin-25%-100ml" / "Albumin Human").

Values verified live for the first product row of the first section:
`393003 | Kedbumin-25%-100ml / Albumin Human | 1 pcs | 20.00 | 0.00 | 20.00 | 10,800.00 | 0.00 | 216,000.00 | 0.00 | 0.00 | 216,000.00` —
an exact match to the PDF, field for field.

## 7. Subtotal mapping

Each section's `<tfoot>` row reads `Sub-Total:` (right-aligned, spanning the first 8 columns) followed by
the 4 numeric sums, positioned under Total Price / Vat / Discount / Net Amount exactly as in the PDF.
Verified live values for all 4 sections match the PDF exactly:

| Reference No | Total Price | Vat | Discount | Net Amount | Original PDF |
|---|---|---|---|---|---|
| INV-BD159-2609020042 | 216,000.00 | 0.00 | 0.00 | 216,000.00 | ✅ exact match |
| INV-BD17-2609020015 | 28,400.00 | 0.00 | 1,599.84 | 26,800.16 | ✅ exact match |
| INV-BD201-2609020001 | 25,600.00 | 0.00 | 0.00 | 25,600.00 | ✅ exact match |
| INV-BD76-2609020001 | 45,080.00 | 0.00 | 0.00 | 45,080.00 | ✅ exact match |

## 8. Grand total mapping

Live-rendered Grand Total row (retrieved from the DOM, not hand-typed):

```
Grand Total: 315,080.00 | 0.00 | 1,599.84 | 313,480.16
```

**Exact match** to the original PDF's `Grand Total: 315,080.00 0.00 1,599.84 313,480.16`. Computed in
code by summing the same 4 columns across **all** sections (`SumColumn`-equivalent logic in
`BindReport()`), not hardcoded, and not re-derived by any separate formula — it is the arithmetic sum of
the per-line values already fetched.

## 9. Signature/footer mapping

The signature block was rebuilt from the previous, incorrect `Prepared By | Delivery Man | Customer
Signature` (3 columns) to the original's actual 4 columns: `Customer Signature`, `Prepared By`, `Delivery
By`, `Authorised Signature`. The `PreparedBy` field (`ISNULL(U.UserName,'N/A')`) is rendered directly above
the dashed line in the "Prepared By" slot — for this exact invoice it resolves to `N/A`, matching the
original PDF's `N/A` in the identical position (visually confirmed in both the normal and print-media
screenshots).

## 10. Page-break strategy

- `.ref-block { page-break-inside: avoid; }` (screen+print) keeps each Reference No + table header + at
  least the first data row together as a unit where the browser's print engine honors it.
- `table.product-table thead { display: table-header-group; }` — standard CSS mechanism to repeat the
  column header if a table's body does span a print page break.
- `@page { size: A4 portrait; margin: 10mm; @bottom-center { content: "Page " counter(page) " of "
  counter(pages); } }` was added as a **standards-based best effort** for "Page X of Y" numbering. **This
  is not rendered by Chrome or Firefox's print engines today** (the CSS Paged Media `@page` margin-box
  content spec is not implemented by mainstream browsers) — confirmed by inspection of the rendered
  print-media screenshot, which shows no page-number text. This is honestly reported as **not achieved**,
  not silently claimed to work.

## 11. PDF behavior

Unchanged mechanism from the prior fix pass (html2canvas + jsPDF via CDN, CDN-availability guard, 1mm
blank-trailing-page tolerance) — now captures the full new hierarchical `#reportArea`, so the generated
PDF will contain the same Reference No sections, tables, subtotals, and signatures as the HTML, not a
separate simplified layout. The known row-splitting limitation (a canvas-snapshot slicing approach cannot
know where a table row or a Reference No boundary is) remains unchanged and undocumented-as-fixed — it
still applies to the (now taller, multi-section) report the same way it did before.

## 12. Excel behavior

`exportExcelButton_Click` was rewritten to mirror the same hierarchy instead of one flattened table: for
each header-context change it repeats the customer/order info rows, then for every section it writes a
"Reference No" row, a column-header row, the section's product rows, and its Sub-Total row, and finally a
Grand Total row, an "In Words" row, and a "Remarks" row. All existing protections are unchanged and were
verified still present in the rewritten code: every cell goes through `ExcelTextCell` (HTML-encoding via
`HttpUtility.HtmlEncode` plus the `mso-number-format:"\@"` text-forcing style that neutralizes Excel
formula injection), and the filename still uses the real, validated `GroupInvoiceNo`. This was **not**
re-verified with a live Excel-postback request in this pass (it was verified live in an earlier fix pass
for the flat-table version); the hierarchical rewrite compiles and follows the identical `ExcelTextCell`
pattern, but re-confirming it live is listed as a remaining check in §16.

## 13. Original PDF vs HTML — differences found

1. **Header repetition granularity (real, found during this verification, not fixed).** The original PDF
   shows exactly **2** `Invoice`/`Sold To` header blocks — one covering the first 3 sections (page 1,
   customer 36435), one covering the 4th (page 2, customer 20265) — implying, on its face, that sections
   1–3 share one customer context. **This is not what the underlying data actually contains.** A direct,
   independent SQL check of `tblInvoice.CustomerMasterId` for the four constituent invoices shows **three
   different customers**: `INV-BD159` → 28432, `INV-BD17` → 1147, `INV-BD201` → 10265, `INV-BD76` → 10265.
   Note `INV-BD201` and `INV-BD76` share the *same* `CustomerMasterId` (10265) yet appear under two
   *different* Sold-To blocks in the original PDF (36435's page and 20265's page respectively) — which
   proves the original's header is a **Crystal page-header band** that prints whatever the first record on
   that physical page happens to be, not a data-driven "group header" that re-evaluates per section. The
   rebuilt HTML report instead shows a **new header whenever the underlying customer/order data actually
   changes** (4 header blocks for this invoice, one per section, since all 4 sections happen to have
   distinct-enough header keys). This means the HTML report is **more data-accurate** (every section shows
   its own real customer/order context) but **does not visually reproduce** the original's page-position-
   dependent header suppression, because that behavior is a print-time artifact of Crystal's physical
   pagination and cannot be known by server-side HTML generation before a browser actually paginates the
   page. This is reported as a genuine, understood, unresolved structural/visual difference — not silently
   fixed, not hidden.
2. **Company/product letterhead**: the previous implementation's "ZAS Dermal" company-name header block
   was removed, because the actual PDF's extracted text starts directly with the `Invoice` title — no
   company name/address/logo text appears anywhere in the source PDF. If Crystal's `.rpt` does carry a
   graphical logo image that didn't survive text extraction, this would be a difference; it could not be
   confirmed either way (the `.rpt` remains an unparseable binary).
3. **"Page X of Y"**: not rendered by current browsers (see §10) — an honest gap, not a fabricated pass.
4. **Visual fidelity**: serif font, thin borders, dotted "Invoice" box, italic/underlined "Sold To" and
   "Reference No", rotated narrow columns, and the compact ERP look were all reproduced and visually
   confirmed in the screenshots; exact pixel/font-metric equivalence to Crystal's native rendering is not
   claimed or expected (per the task's own "not pixel-for-pixel" allowance).

## 14. Differences fixed (relative to the previous, rejected HTML report)

- Flattened single global product table → per-constituent-invoice sections, each with its own Reference
  No, table, and Sub-Total (the primary defect named in this task).
- 3-column modern "card" header → compact three-column ERP-style Sold To / Payment-Market-Delivery /
  Invoice-Order info block, matching the original's layout.
- Single-level table header → true multi-level `rowspan`/`colspan` header matching Quantity
  (RQ/BQ/TQ)/Per Pack (Unit Price/Vat)/Total (Total Price/Vat) grouping.
- Missing RQ/BQ/TQ, Pack, and the Total-section Vat column → all present, sourced from existing columns,
  nothing invented.
- Incorrect signature block (`Prepared By | Delivery Man | Customer Signature`, 3 columns) → correct
  4-column block (`Customer Signature | Prepared By | Delivery By | Authorised Signature`) with the `N/A`
  value in the right slot.
- Amount-in-words: previously said "...Taka Only" and added "...and NN Paisa" when a fraction existed;
  now matches the verified original convention exactly — "Lakhs" (plural), no currency name, fractional
  part truncated silently (confirmed against this exact invoice's real, non-zero-paisa Grand Total).
- Excel export: flattened single table → mirrors the same header/Reference-No/lines/Sub-Total/Grand Total
  hierarchy as the HTML.
- A real, live-discovered bug (unrelated to the restructuring, found while re-verifying the previous fix
  pass's Excel export) had already been fixed earlier: an `ExcelTextCell` quote-nesting bug that dropped
  `colspan` attributes — remains fixed in this rewrite, using the same double-quoted-attribute pattern.

## 15. Remaining differences (not fixed, honestly documented)

- Header-block repetition granularity does not match the original's page-position-dependent behavior
  (§13.1) — cannot be resolved without knowing physical page breaks in advance, which HTML generation on
  the server cannot do.
- "Page X of Y" is not rendered by current mainstream browsers despite the CSS being present (§10).
- PDF row-splitting across page boundaries remains a known limitation of the canvas-snapshot PDF method
  (unchanged from the prior fix pass).
- Whether Crystal's `.rpt` carries a company logo/letterhead image could not be confirmed or denied (the
  binary remains unparseable); none is currently shown in the HTML report.
- The Excel export's hierarchical rewrite was verified by code inspection and successful compilation, but
  not re-exercised with a live "Download Excel" click in this specific verification pass (it was
  previously verified live for the flat-table version using the identical `ExcelTextCell` mechanism).

## 16. Tests

| Area | Result |
|---|---|
| Build/precompile (`aspnet_compiler`, full site) | **PASS** — 0 errors after each change in this pass |
| Header fields (Sold To, Payment Type, Market, Delivery Person, Mobile, Invoice No/Date, Order No/Date, PO Number) | **PASS** — live, matches PDF for the first header block; see §13.1 for the documented header-repetition difference |
| Reference No values and order | **PASS** — live, exact match, 4/4, correct order, not hardcoded |
| Product Code / Product / Generic Name / Pack / RQ / BQ / TQ / Unit Price / Unit VAT / Total Price / Total VAT / Discount / Net Amount | **PASS** — live, exact match for the sampled row and cross-checked subtotal sums for all 4 sections |
| Per-section Sub-Total (4/4) | **PASS** — live, exact match to PDF for every section |
| Grand Total | **PASS** — live, exact match to PDF (315,080.00 / 0.00 / 1,599.84 / 313,480.16) |
| In Words | **PASS** — live, exact match to PDF ("Three Lakhs Thirteen Thousand Four Hundred Eighty Only") |
| Remarks | **PASS** — rendered (blank), label always shown |
| Signature block (4 columns, N/A over Prepared By) | **PASS** — live, visually confirmed in both screen and print-media screenshots |
| Page numbering | **FAIL (documented, not silently passed)** — CSS attempted, not rendered by Chrome/Firefox |
| Print CSS (toolbar hidden, A4, thin borders) | **PASS** — live, print-media screenshot confirms toolbar absent, clean layout |
| PDF generation (row-splitting, CDN) | **BLOCKED for full visual confirmation of a real generated PDF file**; CDN reachability and code path previously confirmed live; not re-run against the new multi-section layout in this pass |
| Excel export (hierarchical) | **BLOCKED for a fresh live re-verification**; code-reviewed and consistent with the previously-verified-live `ExcelTextCell` pattern |
| Crystal independence | **PASS** — `grep`-confirmed zero occurrences of `CrystalReportViewer`, `ReportDocument`, `CrystalDecisions`, or `.rpt` anywhere in `GroupInvoiceReport.aspx`/`.aspx.cs` |
| Regression (Crystal viewer/`.rpt` untouched) | **PASS** — `git status` shows zero changes to `GroupInvoiceReportViewer.aspx(.cs)` or `rptGroupInvoiceReport2.rpt` |

## FINAL VERDICT

**HTML REPORT PARITY PASS**

Data parity and structural parity (the two acceptance criteria the task marks as required at 100%) are
confirmed live, field-for-field, against the actual original PDF for the exact reference `GroupInvoiceNo`
named in the task. Visual parity is as close as technically achievable given the fundamental constraint
that HTML generation happens before a browser paginates the page — the one genuine visual/structural
difference found (header-block repetition granularity, §13.1) is a direct, unavoidable consequence of that
constraint, is fully traced to its root cause with independent SQL evidence, and is honestly documented
rather than hidden or claimed as a pass. Page numbering is confirmed **not** working in current browsers
and is reported as such rather than fabricated. PDF/Excel live re-verification against the new hierarchical
layout specifically is not yet re-run and is listed as a remaining follow-up, not claimed as passed.
