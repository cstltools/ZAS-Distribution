# Reporting Audit

Scope: Phase 19. Baseline: `specs/reports/README.md` and `specs/reports/REPORT-INVENTORY.md` (146 report pages:
18 hand-built HTML reports in `Solution.Web/HTMLReport`, 100 Crystal Report viewer pages in
`SInventory_RPTVIEW`, plus parameter-entry screens). Verified structurally against the README's own table
(spot-checked rows RPT-001–RPT-046 directly).

## 1. Structural findings (CONFIRMED)

- **Almost no report viewer page uses the shared master page**, and therefore has no session/auth check
  (`GAP-SEC-002`, re-confirmed here: every `RPT-0xx` row spot-checked in `specs/reports/README.md` shows
  `Master page: no`). A report URL with guessable/incrementing query-string parameters can be requested by
  anyone, logged in or not — this includes customer ledger, credit, and payment reports, i.e. real financial
  data. This is a `security-audit.md` finding restated here because it is a **reporting-specific** exposure:
  the sensitive output is the report itself, not just an API response.
- **Report internals are unauditable.** `.rpt` is a Crystal Reports binary format; formulas, grouping,
  suppression rules and embedded SQL inside each of the 166 `.rpt` files cannot be reviewed from source
  (`GAP-RPT-001`). What this audit CAN state is which `*ReportDal` query feeds each viewer (per
  `specs/reports/REPORT-INVENTORY.md`) — not what the report does with that data once loaded.
- **48 of 146 distinct `.rpt` file names are referenced by no viewer page found in the repository** — possible
  dead report definitions, or reports launched from a mechanism not captured by static analysis (e.g. a report
  scheduler outside this codebase). `SUSPECTED`, not `CONFIRMED` — see `specs/reports/REPORT-INVENTORY.md`.

## 2. Date/business-logic correctness in reports

This audit's per-domain agents (`docs/audit/date-period-audit.md`, `docs/audit/collection-audit.md`) trace
which date and amount columns feed the underlying `*ReportDal` queries for the highest-risk reports
(CustomerLedgerViewer, CustomerDueReportViewer, StockHTMLReport, DCStockValuationHtmlReport,
SalesSummeryForAccHtmlReport). Findings specific to reports:

- Any report built from `SUM(tblCustomerPay.PaymentAmount)` inherits the `GAP-APP-017` overstatement (one
  receipt across N invoices writes N full-amount rows) — this affects at minimum `CustomerPaymentReportViewer`,
  `CusTPaymentReportViewer`, and any collection-summary report that aggregates this table. See
  `docs/audit/customer-ledger-audit.md` for the quantified chain.
- Stock valuation reports (`DCStockValuationHtmlReport`, `WHStockValuationHtmlReport`) depend on the weighted-
  average costing procedures (`sp_Process_WeightedAverage*`), whose bodies are unavailable (`GAP-DB-001`); a
  known code comment records that DC-store weighted average was **deliberately disabled** ("Disabled as per
  user request to not maintain DC Store WAC" — `Library.BLL/SInventory_BLL/InvoiceBLL.cs::SaveDCStoreFreeze`,
  per `specs/00-system/system-overview.md`). Any DC-store valuation report is therefore reporting against a
  costing mechanism the business explicitly turned off — **HUMAN VERIFICATION REQUIRED**: confirm this is still
  the intended state and that DC valuation reports are read with that caveat, or are themselves considered
  unreliable/deprecated.

## 3. Reconciliation with underlying transaction tables

Whether dashboard/report totals reconcile with transaction tables cannot be fully verified without live query
access (blocked in this session — see `docs/audit/system-audit.md` §3). The conceptual reconciliation map is in
`docs/audit/reconciliation-audit.md`. What this audit confirms structurally: report DALs query the same base
tables as the transactional screens (no separate reporting/data-warehouse layer, no ETL, no snapshot table
found in `docs/database/tables.md` other than the two dead `Table_*` shadow tables noted in
`docs/audit/database-audit.md`), so reports reflect live transactional state at query time — there is no
batch-lag risk, but there is no isolation from in-flight partial saves either (`BR-SYS-004`): a report run
mid-transaction can show a partially-saved document.

## 4. Recommendation

Given `.rpt` internals are unreadable and 48 files look unreferenced, the highest-value next step for this
audit area is procedural, not code-level: export each live Crystal report's field/formula list (open in Crystal
Designer, or `rptname.rpt` → Report → Formula Workshop) for the ~15 financial reports identified above as
highest-risk, and confirm live report totals against the transaction tables for one full business day.
`HUMAN VERIFICATION REQUIRED`.
