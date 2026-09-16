# Reconciliation audit — Sales / AR / Stock / GL

Scope: a **conceptual** map of what should tie out with what, built from C# code flow, the DAL/BLL
call chains, and the 24 live view bodies in `docs/database/views.md`. This is audit-only, code-and-docs
research — **no live database was queried**. Every claim below is either (a) a code fact with a
file:line citation, or (b) explicitly marked `HUMAN VERIFICATION REQUIRED` with a concrete query to run
once live access exists. Bug IDs referenced (`GAP-APP-016/017`, `GAP-DB-001`, `GAP-APP-005`) are from
`specs/SPEC-GAPS.md`; one new, tentative finding is filed as `BUG-ACC-901` (may be renumbered by the
caller).

---

## 1. End-to-end money/quantity flow (Sales → Cash → AR → GL)

```
Order (tblOrder/tblOrderDetail)
  → Proforma Invoice
  → Delivery Confirmation (Full/Partial) — writes DeliveryNetAmount/DeliveryTpGrandTotal, DeliveryStatus
  → tblInvoice / tblInvoiceDetail                                    [Sales Register source]
  → Customer Payment — TWO parallel, both-live recording paths:
        (A) CustomerPayment.aspx / MarketWiseCustomerPayment.aspx / CustomerPaymentProcess.aspx
            → CustPaymentBLL/CustPaymentDAL → tblCustomerPay (header) + tblCustPayDetail (per-invoice line)
        (B) PaymentTranscation.aspx / PaymentTranscationNew.aspx / PaymentTranscationNew2.aspx
            → PaymentTranscationDal → tblPaymentTranscationMaster (header) + tblPaymentTranscationDetail (line)
  → both paths call the SAME posting procedure per detail row:
        sp_Sales_CustomerPaymentPosting(@CustPayId, @InvoiceId)   -- body UNKNOWN (GAP-DB-001)
  → AR / Dues / Ledger reports read back ONLY path (A)'s tables (see §3.2)
  → GL posting inside ZAS_ACCDB/CSTL_ACCDB                         -- opaque, UNVERIFIABLE
```

**Evidence that both payment paths are live, not superseded:**
- `CustPaymentDAL.cs` (path A) is the DAL behind `CustomerPayment.aspx.cs`, `MarketWiseCustomerPayment.aspx.cs`,
  `CustomerPaymentProcess.aspx.cs`, `CustomerPaymentList.aspx.cs` — writes `tblCustomerPay`/`tblCustPayDetail`.
- `PaymentTranscationDal.cs` (path B) is the DAL behind `PaymentTranscation.aspx.cs`,
  `PaymentTranscationNew.aspx.cs`, `PaymentTranscationNew2.aspx.cs` — writes
  `tblPaymentTranscationMaster`/`tblPaymentTranscationDetail` (`PaymentTranscationDal.cs:33-60`,
  `sp_PMNT_CustomerPaymentMasterInsert`/`sp_PMNT_CustomerPaymentDetailInsert` per
  `docs/database/stored-procedures.md:49-50`).
- Both call `sp_Sales_CustomerPaymentPosting` (`CustPaymentDAL.PaymentJournalPosting`,
  `PaymentTranscationDal.PaymentJournalPosting`, `docs/database/stored-procedures.md:43`) — so the intent
  is that both feed the same GL posting, but the *local* SalesRoll-side bookkeeping tables are entirely
  separate schemas with no shared row.
- The two paths are aware of each other only at invoice-selection time: `CustPaymentDAL.cs:235,392,409,489`
  each exclude invoices already present in
  `tblPaymentTranscationMaster INNER JOIN tblPaymentTranscationDetail ... WHERE IsProcess = 0 AND IsReject = 0`
  from path A's payable-invoice list — i.e. path A stops you re-paying an invoice that has a *pending*
  path-B transaction. This is a UI-level guard against double payment, not a reconciliation between the
  two tables.

---

## 2. Views relevant to reconciliation — status among the 24 live views

Checked against the full 24-view body dump in `docs/database/views.md`:

| View (old 6-view list name) | Present in current 24? | Notes |
|---|---|---|
| `View_InvoiceWiseDue` | **Yes** (`views.md:640`) | `UNUSED — no application caller found` (live). Only reference is a commented-out query in `OrderInfoDAL.GetCustomerDueHistory` (`Library.DAL/SInventory_DAL/OrderInfoDAL.cs:400-405`); the live implementation (lines 407-419) uses an inline ad-hoc query instead. |
| `View_CustomerMaster` | **Yes** (`views.md:39`) | Heavily used (21+ DAL files) — denormalized customer+geography join. Not a money view. |
| `View_CentralStoreCurrentStock` | **Yes** (`views.md:13`) | Live caller: `RequisitionDAL.GetPickingDataForIssueDAL` (`Library.DAL/SInventory_DAL/RequisitionDAL.cs:186`). |
| `View_DCStoreCurrentStock` | **Yes** (`views.md:128`) | Live caller: `InvoiceDAL.ProductInfoDAL` (`Library.DAL/SInventory_DAL/InvoiceDAL.cs:708,723`). |
| `View_MIAWiseSalesReport` | **NOT FOUND** among the 24 | Despite being listed as a call target in `specs/reports/README.md` ("`View_MIAWiseSalesReport` — 1" call site), no view of this name exists in the current 24-view dump. Either renamed/dropped since that report inventory was built, or the report inventory's static-analysis pass matched a differently-cased/aliased identifier. **HUMAN VERIFICATION REQUIRED** — run `SELECT * FROM sys.views WHERE name LIKE '%MIAWiseSales%'` against the live DB to confirm whether it exists under another name or was dropped; if dropped, the report page(s) citing it may be broken or already rewritten to an inline query (same pattern as `View_InvoiceWiseDue` above). |
| `View_ProDCStock` | **NOT FOUND** among the 24 | Not present under this or any close name. Likely renamed/removed, or never existed and was an old doc's inference. **HUMAN VERIFICATION REQUIRED** — `SELECT * FROM sys.views WHERE name LIKE '%ProDCStock%' OR name LIKE '%ProductDCStock%'`. |

**Other views discovered in the 24 that are directly relevant to reconciliation and were NOT in the old
6-view list:**

| View | Relevance | Live usage |
|---|---|---|
| `View_InvoiceWisePayment` (`views.md:683`) | `SUM(tblCustPayDetail.PaymentAmount/AIT/Discount)` grouped by invoice — exactly the shape a payment-vs-invoice reconciliation would want | `UNUSED — no application caller found`. |
| `View_InvoiceWiseSales` (`views.md:704`) | `SUM(DeliveryNetAmount)` per invoice from `tblInvoice`/`tblInvoiceDetail` — a canonical "sales register" aggregate | `UNUSED — no application caller found`. |
| `View_MioWiseInvoiceSummary` (`views.md:726`) | invoice summary by field-force MIO | not investigated further (out of AR scope) |

**Key structural fact:** the three views purpose-built for invoice/payment/due reconciliation
(`View_InvoiceWiseDue`, `View_InvoiceWisePayment`, `View_InvoiceWiseSales`) are **all three unused by any
application code path**. Every screen and report that needs "sales this invoice" or "due this invoice"
re-derives it with its own inline, hand-written SQL (see §3 below) — meaning there is no single
authoritative definition of "due" in the codebase; there are as many as there are DAL methods that ask
the question, and they are not guaranteed to agree (see §3.1/3.2).

---

## 3. Reconciliation pairs

### 3.1 Sales Register vs Customer Ledger — RECONCILABLE-IN-PRINCIPLE, with a caveat

| | |
|---|---|
| Sales Register source | `tblInvoice` + `tblInvoiceDetail`, filtered `DelivaryInvoiceNo IS NOT NULL` (e.g. `SalesReportDAl`, `AllSalesreportViewer` chain, `View_InvoiceWiseSales` pattern) |
| Customer Ledger source | `CustomerLedgerDAL.CustomerLedgerDal` (`Library.DAL/SInventory_DAL/CustomerLedgerDAL.cs:17-22`) — `tblInvoice I INNER JOIN View_CustomerMaster M`, selecting `I.TpGrandTotal, I.DeliveryTpGrandTotal, I.PaymentAmount, I.PaymentStatus` straight off the invoice header |
| Customer Wise Ledger (report variant) | `CustomerWiseLedgerDal.GetCustomerWiseLedger` (`Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs:103-119`) — independently re-derives `ActualSales` from `SUM(tblInvoiceDetail.DeliveryNetAmount) WHERE DeliveryStatus IN ('Partial','Full')`, NOT from `tblInvoice.DeliveryTpGrandTotal` |

Both ledgers and the sales register draw from `tblInvoice`/`tblInvoiceDetail` — the same base tables, so
in principle they tie out. **But** `CustomerLedgerDAL` uses the header-level `tblInvoice.DeliveryTpGrandTotal`
while `CustomerWiseLedgerDal` independently re-sums `tblInvoiceDetail.DeliveryNetAmount`. These are two
different SQL expressions computing what should be the same "actual sales" number, maintained in two
separate files with no shared helper. If `tblInvoice.DeliveryTpGrandTotal` and
`SUM(tblInvoiceDetail.DeliveryNetAmount)` are ever allowed to drift (e.g. a partial-delivery edit that
updates one but not the other — plausible given `GAP-APP-005`'s pattern of unguarded overwrite updates
elsewhere in this codebase), `CustomerLedgerReport.aspx` (FIN-008) and `CustomerWiseLedgerReport.aspx`
(RPT-110) would show different due/sales figures for the same customer.

**Status: RECONCILABLE-IN-PRINCIPLE** (same underlying tables) but **AT-RISK of drift** because the two
report DALs compute the aggregate independently rather than sharing one query/view.
`HUMAN VERIFICATION REQUIRED` — run, per invoice:
`SELECT InvoiceId, DeliveryTpGrandTotal, (SELECT SUM(DeliveryNetAmount) FROM tblInvoiceDetail WHERE InvoiceId = I.InvoiceId) FROM tblInvoice I WHERE DelivaryInvoiceNo IS NOT NULL` and look for any row where the two columns differ.

### 3.2 Customer Ledger / AR aging vs actual cash collected — AT-RISK (scoped)

**What the due/ledger/aging DALs actually read**, confirmed by direct inspection:

| DAL / method | Payment source used |
|---|---|
| `CustomerDueReportDAL.LoadCustomerDueReport` / `LoadCustomerTotalDueReport` (`Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs:93-147`) | `SUM(tblCustPayDetail.PaymentAmount/AIT/Discount)` per invoice, joined to `tblCustomerPay` only for `PaymentDate` filtering |
| `CustomerWiseLedgerDal.GetCustomerWiseLedger` (`:103-119`) | same — `SUM(PD.PaymentAmount) ... FROM tblCustPayDetail AS PD` |
| `SalesDashboardDal.GetDueUpto` (`Library.DAL/SInventory_DAL/SalesDashboardDal.cs:99-108`) | same pattern, `tblCustPayDetail` |
| `SalesDashboardDal.GetAglingReport` (`:145-151`) | delegates entirely to `sp_DSB_AglingReport` — **UNVERIFIABLE**, body unknown (`GAP-DB-001`) |
| `CustomerLedgerDAL.CustomerLedgerDal` (`:17-22`) | `tblInvoice.PaymentAmount` (header column, presumably maintained by the opaque `sp_UD_InvoicePayment`/posting procs) — a THIRD, different source from the two above |

**GAP-APP-017 re-scoped by this reading — narrower than the header suggests.** `GAP-APP-017` says one
receipt across N invoices creates N `tblCustomerPay` header rows each stamped with the *full* receipt
amount. That is confirmed (`WF-customer-payment.md` Stage 5). But the per-invoice **detail** row
(`tblCustPayDetail.PaymentAmount`) is populated correctly with the per-invoice amount, not the full
receipt amount — confirmed at `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs:427`
(`aCustPaymentDetail.PaymentAmount = Convert.ToDecimal(payAmountTextBox.Text);`, the per-row textbox,
inside the same loop that stamps the *header*'s `PaymentAmount` with the full
`paymentAmountTextBox.Text` at line 395).
A repo-wide grep for every DAL that aggregates payment amounts (`CustomerPaymentReportDal`,
`FieldForceSummeryDal`, `WingsWiseSalesReportDal`, `DepositSlipEntryDal`) found **no query that sums
`tblCustomerPay.PaymentAmount` directly** — every one of them sums `tblCustPayDetail.PaymentAmount`
instead (see `docs/database/` search — no `FROM tblCustomerPay` aggregate without a `tblCustPayDetail`
join was found in `Library.DAL`). So **the specific over-count GAP-APP-017 describes (N full-amount
header rows) does not appear to reach the AR/due/ledger/deposit reports that were checked**, because none
of them read `tblCustomerPay.PaymentAmount` in aggregate — they all read the correctly-scoped detail rows.
**This narrowing does not close GAP-APP-017** — the N duplicate header rows still exist in
`tblCustomerPay` and would corrupt any *future* report, ad-hoc query, or the money-receipt/deposit-slip
count logic that trusts `COUNT(*)`/`SUM(PaymentAmount)` on the header table directly (e.g. anyone
reconciling "number of receipts issued today" against `tblCustomerPay` row count would over-count by
(N-1) per multi-invoice receipt). Flag this nuance explicitly rather than assuming the bug is either fully
harmless or fully corrupting.

**The real, newly-identified AT-RISK finding here is cross-path, not intra-path — see `BUG-ACC-901` below.**

**Status: AT-RISK** — for two independent reasons:
1. Three different due/ledger DALs use three different source expressions (`tblCustPayDetail` sum,
   `tblInvoice.PaymentAmount` header column, and an opaque procedure) that are not proven to agree.
2. `BUG-ACC-901` (below): payments recorded via the `PaymentTranscation*` pipeline are invisible to every
   due/ledger DAL inspected.

`HUMAN VERIFICATION REQUIRED` —
```sql
-- (a) Does the header-column view of due agree with the detail-sum view of due, per invoice?
SELECT INV.InvoiceId, INV.PaymentAmount AS HeaderPaymentAmount,
       (SELECT SUM(PaymentAmount) FROM tblCustPayDetail WHERE InvoiceId = INV.InvoiceId) AS DetailSumPaymentAmount
FROM tblInvoice INV WHERE DelivaryInvoiceNo IS NOT NULL;
-- (b) How many tblCustomerPay header rows exist vs. how many distinct receipts were actually issued
--     (proxy: distinct (CustomerMasterId, PaymentDate, RefNo) combos, since GAP-APP-017 predicts
--     duplicate headers per multi-invoice receipt):
SELECT CustomerMasterId, PaymentDate, RefNo, COUNT(*) FROM tblCustomerPay GROUP BY CustomerMasterId, PaymentDate, RefNo HAVING COUNT(*) > 1;
```

### 3.3 NEW FINDING — `BUG-ACC-901` (tentative ID): PaymentTranscation-recorded collections are invisible to AR/due/ledger reports

**Severity: high. Confidence: high (direct code evidence, not inference).**

`tblPaymentTranscationMaster`/`tblPaymentTranscationDetail` (path B, §1) is a fully separate schema from
`tblCustomerPay`/`tblCustPayDetail` (path A). A repo-wide grep of `Library.DAL` for
`tblPaymentTranscationDetail`/`tblPaymentTranscationMaster` found exactly **two** files referencing them:
`CustPaymentDAL.cs` (only to *exclude* invoices with a pending path-B transaction from path-A's
payable list — §1) and `PaymentTranscationDal.cs` itself. **No due, ledger, aging, or deposit-reconciliation
DAL reads `tblPaymentTranscationMaster`/`tblPaymentTranscationDetail`** — not
`CustomerDueReportDAL`, not `CustomerWiseLedgerDal`, not `CustomerLedgerDAL`, not
`SalesDashboardDal.GetDueUpto`, not `DepositSlipEntryDal`.

Consequence: a payment collected through `PaymentTranscation.aspx`/`PaymentTranscationNew.aspx`/
`PaymentTranscationNew2.aspx` reduces the invoice's due amount only if/when the opaque
`sp_Sales_CustomerPaymentPosting` procedure also updates `tblInvoice.PaymentAmount` (used by
`CustomerLedgerDAL`) — which is **UNVERIFIABLE** (`GAP-DB-001`). It does **not** appear in
`tblCustPayDetail`, so `CustomerDueReportDAL`, `CustomerWiseLedgerDal.GetCustomerWiseLedger`, and
`SalesDashboardDal.GetDueUpto` — three of the four "customer due" data sources found in this codebase —
would still show the invoice as due (or as due by the wrong amount) after a path-B collection, unless the
posting procedure also inserts into `tblCustPayDetail` on path B's behalf (not evidenced anywhere in the
C# call chain — path B never calls path A's detail-insert code).

**Status: AT-RISK**, arguably the single most consequential reconciliation risk found in this pass, because
it means "which AR report do you trust" has a real, different answer depending on which payment screen
collected the cash.

`HUMAN VERIFICATION REQUIRED` —
```sql
-- Any invoice with a completed (IsProcess=1, IsReject=0) path-B payment but no matching tblCustPayDetail row:
SELECT PTD.InvoiceId, PTD.PaymentAmount
FROM tblPaymentTranscationDetail PTD
JOIN tblPaymentTranscationMaster PTM ON PTD.CustPayId = PTM.CustPayId
WHERE PTM.IsProcess = 1 AND PTM.IsReject = 0
  AND NOT EXISTS (SELECT 1 FROM tblCustPayDetail CPD WHERE CPD.InvoiceId = PTD.InvoiceId);
```
If this returns rows, `CustomerDueReport.aspx`, `CustomerWiseLedgerReport.aspx`, and the dashboard aging
report are all overstating that invoice's due amount right now.

### 3.4 AR/collections vs GL subsidiary ledger (ZAS_ACCDB/CSTL_ACCDB) — UNVERIFIABLE

Per `GAP-DB-001`, no procedure body is available, so the actual posting logic cannot be read. What can be
documented is the **call-site contract** — which local objects are handed to which GL-side procedures,
which is the closest thing to a reconciliation map available without a live DB:

| Local object | Passed to | Presumed remote GL object (INFERRED, per `docs/database/stored-procedures.md` §I) |
|---|---|---|
| new customer (`tblCustMaster` row) | `sp_I_CustomerGl(@CustomerId, @SubsidiaryLId)` (`CustomerMasterInfoDal.CreateGlOfCustomer`) | `ZAS_ACCDB..tblSubsidiaryLedger` / `tblSubsubsidiaryLadger` |
| customer↔ledger link | `sp_Integration_CustomerSubsidaryInsert(@CustomerId, @SubsidiaryLId)` | integration/link table (name unknown) |
| `tblCustomerPay` + `tblCustPayDetail` row (path A) | `sp_Sales_CustomerPaymentPosting(@CustPayId, @InvoiceId)` | `tblSubsidiaryLedger`/`tblControlLedger` entries crediting the customer's AR account and debiting cash/bank (INFERRED from naming and parameters only — body NOT AVAILABLE) |
| `tblPaymentTranscationMaster` + `tblPaymentTranscationDetail` row (path B) | same procedure, same parameters | same, in principle — but see `BUG-ACC-901`: whether the procedure treats path A and path B identically is unverifiable, and if it does post identically, that would mean the GL side is consistent while the SalesRoll-side AR reports (§3.3) are not, i.e. **GL and local AR reports could each be internally consistent yet disagree with each other** |
| payment reversal | `sp_Sales_CustomerPaymentReversePosting(@CustPayId, @InvoiceId)` | reversing entries in the same GL tables (INFERRED) |

**Status: UNVERIFIABLE.** Cannot be resolved from code; requires either the procedure source or live-DB
comparison.
`HUMAN VERIFICATION REQUIRED` — with DBA access to `ZAS_ACCDB`/`CSTL_ACCDB`:
```sql
-- Compare total local collections (path A, correctly-scoped detail sum) for a period against
-- the GL subsidiary ledger credit total for the same customers/period:
SELECT SUM(PaymentAmount) FROM tblCustPayDetail WHERE <date filter>;
-- vs., in the accounts DB (table/column names are INFERRED, confirm against actual schema):
SELECT SUM(CreditAmount) FROM ZAS_ACCDB..tblSubsidiaryLedger WHERE <matching date filter, matching customer set>;
```

### 3.5 Sales Register vs GL revenue recognition — UNVERIFIABLE

No stored procedure visibly posts `tblInvoice`/`tblInvoiceDetail` totals to a GL revenue account inside
the C#/DAL layer inspected for this audit (invoice creation code in `InvoiceDAL`/`InvoiceBLL` was not
found to call any `sp_I_*Gl`-style posting procedure at invoice-save time, only at customer/product/supplier
master-creation time and at payment time — see §3.4). Whether/how/when sales revenue is recognized in
`ZAS_ACCDB`/`CSTL_ACCDB` is entirely outside what this codebase makes visible.

**Status: UNVERIFIABLE.**
`HUMAN VERIFICATION REQUIRED` — ask the DBA/finance team directly which procedure (if any, possibly a
scheduled job rather than an interactive-screen call) posts sales revenue, then compare
`SUM(tblInvoiceDetail.DeliveryNetAmount)` for a period against the GL revenue account balance for the
same period.

### 3.6 Stock: DC store vs central store vs weighted-average cost vs bin cards — AT-RISK

| Source | Object | Scope |
|---|---|---|
| DC current stock | `View_DCStoreCurrentStock` (`views.md:128-147`) | `SUM(tblDCStore.StockQty) WHERE StockQty > 0`, grouped by product+DC. Live caller: `InvoiceDAL.ProductInfoDAL`. |
| Central store current stock | `View_CentralStoreCurrentStock` (`views.md:13-36`) | `SUM(tblCentralStore.Quantity) WHERE Quantity > 0`, grouped by product+condition+warehouse. Live caller: `RequisitionDAL.GetPickingDataForIssueDAL`. |
| Weighted-average cost | `tbl_INV_WeightedAvg` (referenced in task background; not re-verified here — out of the views/DAL files read this pass) | presumably updated on every stock-in/stock-out event |
| Bin card (DC) | `sp_DCBinCard` (`docs/reports` §"Stored procedures this module calls": `sp_DCBinCard` 2 call sites) | opaque procedure — body NOT AVAILABLE |
| Bin card (national) | `sp_NationalBinCard` (4 call sites) | opaque procedure — body NOT AVAILABLE |

`View_DCStoreCurrentStock` and `View_CentralStoreCurrentStock` both read straight off their respective
base tables with a simple `SUM(...) WHERE qty > 0 GROUP BY`, so **in principle** they should already tie
out with a manual `SELECT SUM(StockQty) FROM tblDCStore` / `tblCentralStore` at any instant — there is no
separate cached/materialized total to drift from the detail rows *within* each store. The risk is not
"view vs. table" (those are definitionally consistent, being simple aggregates) but **table vs. reality**:

- `GAP-APP-005` (`specs/SPEC-GAPS.md:185-189`): `tblDCStore.StockQty` is updated via
  `UPDATE tblDCStore SET StockQty = '<value computed in C#>'` — an **overwrite**, not a decrement, with
  **no optimistic-concurrency token**. Two concurrent stock-affecting operations (e.g. an invoice delivery
  confirmation and a stock transfer-in) on the same DC/product can race: the second `UPDATE` silently
  clobbers the first's result with a stale computed value. This means `tblDCStore.StockQty` — and
  therefore everything built on it (`View_DCStoreCurrentStock`, and by extension any invoice-creation
  stock check via `InvoiceDAL.ProductInfoDAL`) — can silently diverge from what actually moved through
  bin-card-tracked transactions (`sp_DCBinCard`), because bin cards are (presumably, UNVERIFIED — opaque
  procedure) an append-only ledger of movements that isn't vulnerable to the same last-write-wins race.
- The weighted-average cost table (`tbl_INV_WeightedAvg`) was not independently re-derived in this pass;
  whether its costing keeps pace with the same racy `tblDCStore` updates is unknown.

**Status: AT-RISK** — not because the views misrepresent their base tables, but because `GAP-APP-005`
gives a concrete, code-confirmed mechanism (unguarded overwrite update) by which the base table itself can
drift from the true movement history that bin cards are supposed to represent.

`HUMAN VERIFICATION REQUIRED` —
```sql
-- Compare current DC stock per product against what the bin card's running balance says it should be
-- (requires sp_DCBinCard's actual movement table, since the procedure body is unavailable):
SELECT ProductCode, ComUnitId, SUM(StockQty) FROM tblDCStore GROUP BY ProductCode, ComUnitId
-- vs. the last running-balance row per product/DC from whatever table sp_DCBinCard reads (identify via
-- SQL Profiler/extended events while running the report, since the proc body cannot be read statically).
```

---

## 4. Reconciliation-report inventory — which screen shows which side of each pair

| Reconciliation pair | "Left side" report | "Right side" report |
|---|---|---|
| Sales Register vs Customer Ledger (§3.1) | `SalesReport.aspx` (RPT-136, `SalesReportDAl`), `AllSalesReport.aspx` (RPT-103), `InvoiceReport.aspx` (RPT-120) | `CustomerLedgerReport.aspx` (FIN-008, `CustomerLedgerDAL`), `CustomerWiseLedgerReport.aspx` (RPT-110, `CustomerWiseLedgerDal`) |
| Customer Ledger/AR aging vs cash collected (§3.2, §3.3) | `CustomerDueReport.aspx` (FIN-007, `CustomerDueReportDAL`), `TotalCustomerDueReport.aspx` (FIN-027) | `CustomerPaymentReport.aspx` (FIN-014, `CustomerPaymentReportDal` — reads `tblCustPayDetail`, path A only), `MoneyReceiptReport.aspx` (FIN-019), `PaymentTranscation*.aspx` list views (path B, no dedicated "due impact" report found) |
| AR vs GL subsidiary ledger (§3.4) | any of the above local AR reports | none — no GL-side viewer exists in this codebase; would have to be pulled directly from `ZAS_ACCDB`/`CSTL_ACCDB` by finance tooling outside this repo |
| Sales Register vs GL revenue (§3.5) | `SalesSummeryReportForAcc.aspx` (RPT-138, `sp_Summery_SalesSummerReportForAccounts` — name suggests an accounts-facing summary but body is opaque) | none found locally |
| DC vs Central stock vs bin card (§3.6) | `DCStockHTMLReport.aspx`/`DCStockReportViewer.aspx` (`View_DCStoreCurrentStock`-adjacent), `NationalStockReportViewer.aspx` | `NationalBeenCardHTMLReport.aspx` (RPT-009, `sp_NationalBinCard`), `WHBeenCardHTMLReport.aspx` (RPT-017, `sp_WearhouseBinCard`) |

`SalesSummeryReportForAcc.aspx` is the one report in the inventory whose name explicitly signals it exists
*for* accounts-team reconciliation, but its data comes from `sp_Summery_SalesSummerReportForAccounts`, an
opaque procedure — so even the report built for this purpose cannot be verified from code (`GAP-DB-001`).

---

## 5. Summary table

| # | Pair | Status | Key evidence |
|---|---|---|---|
| 3.1 | Sales Register vs Customer Ledger | RECONCILABLE-IN-PRINCIPLE (AT-RISK of drift) | Same base tables (`tblInvoice`/`tblInvoiceDetail`) but two DALs independently re-derive "actual sales" with different expressions (header total vs. detail sum) |
| 3.2 | Customer Ledger/AR aging vs cash collected | AT-RISK | Three different due-calc DALs use three different payment sources; `GAP-APP-017`'s header-row duplication doesn't reach these DALs directly (they use detail rows) but still corrupts any header-level (`tblCustomerPay`) aggregate elsewhere |
| 3.3 | (new) Path-A AR reports vs Path-B (`PaymentTranscation*`) collections | AT-RISK — `BUG-ACC-901` (tentative) | No due/ledger DAL reads `tblPaymentTranscationMaster/Detail`; path-B collections may not reduce reported due |
| 3.4 | AR/collections vs GL subsidiary ledger | UNVERIFIABLE | `GAP-DB-001` — posting procedure bodies absent |
| 3.5 | Sales Register vs GL revenue recognition | UNVERIFIABLE | No visible posting call at invoice-save time; procedure bodies absent |
| 3.6 | DC stock vs Central stock vs bin card | AT-RISK | `GAP-APP-005` — unguarded overwrite update with no concurrency token on `tblDCStore.StockQty` |

New tentative finding: **`BUG-ACC-901`** — collections recorded through the `PaymentTranscation*` payment
pipeline are not read by any customer-due/ledger/aging DAL found in `Library.DAL`, so those reports can
understate collections/overstate due whenever that pipeline is the one actually used to record a payment.
Confidence: high (direct absence-of-reference grep across `Library.DAL`); real-world impact depends on how
often branches/users actually use the `PaymentTranscation*` screens vs. `CustomerPayment.aspx` —
**HUMAN VERIFICATION REQUIRED** to establish usage volume and to run the query in §3.3.
