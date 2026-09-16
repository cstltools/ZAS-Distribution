# Page-by-Page Audit

Scope: Phase 4. This audit does not re-trace all 383 pages — that trace already exists at full depth
(UI → JS → AJAX → code-behind → BLL → DAL → SP → table → column) in `specs/*/pages/*.md`, one file per page,
and `specs/SPEC-COVERAGE.md` gives a per-page, per-dimension completeness row for every one. Re-doing that work
here would duplicate 383 files for no new evidence. This document instead: (a) states what coverage exists and
its honest limits, (b) spot-verifies a sample, and (c) names the four pages/flows given a full behavioural
trace beyond the template, which is where this audit's own findings concentrate.

## 1. Coverage as it stands

| Dimension | Can ever be COMPLETE? | Current state (from `specs/SPEC-COVERAGE.md`) |
|---|---|---|
| UI controls | Yes | 379/383 COMPLETE, 4 pages have no identifiable server control |
| Frontend behaviour (JS/AJAX/postback) | Yes | 379/383 COMPLETE |
| Backend flow (code-behind → BLL → DAL) | Yes | 383/383 COMPLETE |
| API surface | Yes | 383/383 COMPLETE (most pages: "none") |
| Database mapping (columns) | **No** — blocked by `GAP-DB-002` at the time `specs/` was written | 329 PARTIAL, 54 MISSING. **Now partially unblockable** — see `docs/audit/database-audit.md` re: the live schema extraction found in `docs/database/tables.md` |
| Business rules | **No** — rules inside stored procedures are invisible (`GAP-DB-001`, still open) | 307 PARTIAL, 76 MISSING |
| Permission | **No** — permission is `tblMenuDistribution`/`tblActionUserWiseApproval` data, absent from this repo (`GAP-SEC-001`) | 383/383 UNKNOWN |
| Status lifecycle | **No** — transitions execute inside procedures | 383/383 PARTIAL |
| Test cases | Proposed only, never executed (`GAP-TEST-001`) | 383/383 PARTIAL |

**Spot verification performed for this audit**: re-read 3 page specs in full against their cited `.aspx.cs`
files (`InvoiceCreationForCustomer.aspx.cs`, `DelivaryInvoiceCreationAuto.aspx.cs`,
`CustomerPayment.aspx.cs` — chosen because they are the highest-financial-impact pages) and confirmed the
specs' code citations are accurate line-for-line. No fabricated citation was found in the sample.

## 2. Pages with a full end-to-end behavioural trace

Four pages/flows go beyond the standard per-page template to a fully-worked algorithmic trace, because they
carry the arithmetic and control flow the rest of the audit depends on:

| Page(s) | Workflow doc | What it proves | Consumed by |
|---|---|---|---|
| `InvoiceCreationForCustomer` and all `InvoiceCreation*`/`DelivaryInvoiceCreation*` variants | `specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md` | FEFO stock allocation, discount calculation mismatch, non-atomic save | `docs/audit/distribution-audit.md`, `sales-audit.md`, `inventory-audit.md` |
| `DelivaryInvoiceCreationAuto`, `DelivaryInvoiceCreationForCustomerByOrderAuto` | `specs/delivery-logistics/workflows/WF-delivery-confirmation.md` | status-by-display-text routing, unconditional posting, positional grid-cell invoice number | `docs/audit/distribution-audit.md`, `transaction-integrity-audit.md` |
| `CustomerPayment` and its variants | `specs/finance-collection/workflows/WF-customer-payment.md` | one-receipt-N-masters, discarded amount/status | `docs/audit/collection-audit.md` |
| all 16 `*Approval`/`*Approve` pages | `specs/00-system/workflows/WF-approval.md` | approval matched by `LIKE '%pagename%'`, "Cancel" as a magic literal | `docs/audit/business-rule-audit.md` |

## 3. What remains genuinely page-by-page and not yet centrally audited

The 146 report pages (Phase 19, see `docs/audit/reporting-audit.md`) and the 75 master-data pages are covered
at inventory depth in `specs/` but were not individually re-verified line-by-line by this audit beyond the
samples above — there is no reason from the sampling performed to doubt their accuracy, but this is stated
plainly rather than implied: **full independent re-verification of all 383 pages was not performed in this
audit pass.** Any specific page a reader needs certainty on should be checked against its
`specs/*/pages/PAGE-*.md` file and, for anything financial, against the live `.aspx.cs` directly.
