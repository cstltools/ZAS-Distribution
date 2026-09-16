# Bug Register

Scope: Phase 23. Every finding produced by this audit, plus the prior `GAP-SEC-*`/`GAP-APP-*` findings from
`specs/SPEC-GAPS.md` that this audit independently re-verified and is carrying forward. This is the master
index — full 22-field detail (root cause, reproduction, fix, etc.) is given only for **CRITICAL** severity
bugs in `critical-bugs.md`, and for **HIGH** severity bugs in `high-priority-bugs.md`, to keep this register
usable as an index rather than a second copy of ~110 findings. Every row below links to the source document
that carries its full narrative and file:line evidence.

**A note on IDs**: two sub-agents independently used the `BUG-ACC-*` prefix for different subject matter
(database-audit.md used it for schema-level findings; accounting-integration-audit.md used it for
posting-flow findings). Rather than renumber the source files after the fact, this register disambiguates by
listing the **source document** alongside every `BUG-ACC-*` ID — always check which file an ID came from
before citing it elsewhere.

## Severity summary

| Severity | Count |
|---|---|
| CRITICAL | 17 (16 original + `BUG-ACC-902`, found in the SP-reconciliation follow-up) |
| HIGH | 45 (`BUG-FIN-001` downgraded — see below; net count unchanged as a new HIGH was not added to replace it, treat as 44 effective) |
| MEDIUM | 40 |
| LOW | 9 |

(Counts are of distinct `BUG-*`/`GAP-*` IDs found across all `docs/audit/*.md` files; a handful of findings
carry a range like "CRITICAL(conditional)" and are counted at their stated worst case.)

## CRITICAL

| ID | Title | Module | Source |
|---|---|---|---|
| `GAP-SEC-002` | 127/383 pages have no authentication guard | Security | `security-audit.md` |
| `GAP-SEC-003` | Hard-coded live `sa` credentials in source | Security | `security-audit.md` |
| `GAP-SEC-005` | Systemic SQL injection incl. the login query | Security | `security-audit.md` |
| `GAP-SEC-007` | Passwords stored/compared in plain text | Security | `security-audit.md` |
| `BUG-ACC-902` | Deleting an invoice orphans its posted GL voucher in 2 of 3 delete stored procedures (bodies confirmed) | Accounting | `accounting-integration-audit-followup.md`, `critical-bugs.md` |
| `BUG-ACC-004` (accounting-integration-audit.md) | Warehouse stock-in GL posting call is dead code, never invoked | Accounting/Inventory | `accounting-integration-audit.md` |
| `BUG-ACC-030` (financial-integrity-audit.md) | No application-layer control prevents an unbalanced posting from being requested | Accounting | `financial-integrity-audit.md` |
| `BUG-FIN-003` | `PaymentReverse.aspx` deletes the row directly, never calls the GL reversal procedure | Finance/Collection | `collection-audit.md` |
| `BUG-FIN-004` | Duplicate-payment detection keys on a column that defaults to 0 for most rows — risk of mass-deleting legitimate payments | Finance/Collection | `collection-audit.md` |
| `BUG-FIN-010` | Credit limit is never checked at invoice creation, only optionally at order creation | Finance/Master Data | `customer-ledger-audit.md` |
| `BUG-STOCK-001` | No stock-availability check before invoice save — silent partial fulfillment | Inventory/Sales | `inventory-audit.md` |
| `BUG-STOCK-003` | No SQL transaction wraps any two-sided stock movement anywhere in the system | Inventory | `inventory-audit.md` |
| `BUG-SALES-001` | Discount is dropped to 0 on insert for the direct-customer invoice screen | Sales | `sales-audit.md` |
| `BUG-PUR-002` | Warehouse stock-in GL posting call is commented out (duplicate confirmation of `BUG-ACC-004` from the purchase side) | Purchase/Inventory | `purchase-audit.md` |
| `BUG-PUR-009` | Unparameterized SQL on receipt/payable fields (severity conditional on reachable user input) | Purchase | `purchase-audit.md` |
| `BUG-RET-001` | Consumer returns bypass quarantine, self-approve, and skip posting entirely | Returns | `return-audit.md` |
| `BUG-RET-003` | Journal posting fires on every approval action including Reject | Returns/Accounting | `return-audit.md` |

Full 22-field detail for every row above: `critical-bugs.md`.

## HIGH (45 — grouped by module, IDs only; full detail in `high-priority-bugs.md`)

| Module | IDs |
|---|---|
| Security | `GAP-SEC-004`, `GAP-SEC-008`, `GAP-SEC-009`, `GAP-SEC-001`, `BUG-SEC-001` |
| API | `BUG-API-001`, `BUG-API-003` |
| Accounting/DB structure | `BUG-ACC-002` (database-audit.md — no PK on `tblCompanyInfo`/`tblWearhouse`), `BUG-ACC-003` (database-audit.md — no concurrency token / negative-stock guard at schema level), `BUG-ACC-004` (financial-integrity-audit.md numbering — see note below), `BUG-ACC-001` (accounting-integration-audit.md — misleading success/failure message), `BUG-ACC-002` (accounting-integration-audit.md — posting return value discarded), `BUG-ACC-003` (accounting-integration-audit.md — posting call sites commented out), `BUG-ACC-005` (accounting-integration-audit.md — chart-of-accounts/financial-year checks commented out at all 7 call sites), `BUG-ACC-032` (financial-integrity-audit.md — payment save has no transactional boundary) |
| Transaction integrity | `BUG-TXN-001`, `BUG-TXN-002`, `BUG-TXN-003`, `BUG-TXN-004` |
| Data integrity | `BUG-DATA-001`, `BUG-DATA-002`, `BUG-DATA-003`, `BUG-DATA-004`, `BUG-DATA-005` |
| Inventory/Stock | `BUG-STOCK-002`, `BUG-STOCK-004`, `BUG-STOCK-005`, `BUG-STOCK-007`, `BUG-STOCK-014`, `BUG-STOCK-019`, `BUG-STOCK-020` |
| Sales | `BUG-SALES-002`, `BUG-SALES-005` |
| Purchase | `BUG-PUR-001`, `BUG-PUR-004`, `BUG-PUR-006`, `BUG-PUR-008` |
| Returns | `BUG-RET-002`, `BUG-RET-006` |
| Finance/Collection | `BUG-FIN-001`, `BUG-FIN-002`, `BUG-FIN-005`, `BUG-FIN-007`, `BUG-FIN-008`, `BUG-FIN-009`, `BUG-FIN-011` |
| Performance | `BUG-PERF-003` |

**Note on ID collision**: `financial-integrity-audit.md` did not actually reuse `BUG-ACC-004` — its IDs are
`BUG-ACC-030/031/032` (already namespaced above 030 by that agent to avoid collision). The only true collision
is `database-audit.md` vs `accounting-integration-audit.md`, both starting their own `BUG-ACC-001..005`
sequence independently — always cite the source file.

## MEDIUM (40) and LOW (9)

Full list in `medium-low-bugs.md`, compact table form (ID, title, module, source, confidence) — not repeated
here to keep this index short. Notable clusters: `BUG-STOCK-006/008–013/015/016/021–023` (inventory edge
cases), `BUG-DATA-*` schema-inference caveats, `BUG-PERF-001/002/004/005` (pattern-level performance),
`GAP-APP-001/009/010/011/012/013/014/015` (carried forward from `specs/SPEC-GAPS.md`, all still MEDIUM/LOW).

## Superseded / corrected during this audit

- `GAP-APP-017`'s originally-feared financial impact (collections overstated in reports) was **downgraded**
  after tracing the actual GL-posting procedure body: reports and GL correctly sum `tblCustPayDetail`, not the
  inflated `tblCustomerPay` master rows. The remaining defect is an audit-trail/data-integrity one
  (`BUG-FIN-002`), not a reported-balance misstatement. See `root-cause-analysis.md` RCA-002.
- Several `HUMAN VERIFICATION REQUIRED — needs sp_helptext` tags across `accounting-integration-audit.md`,
  `financial-integrity-audit.md`, `inventory-audit.md` and `collection-audit.md` were resolved mid-audit once
  `docs/database/stored-procedures-part1..4.md` (full T-SQL bodies from a live export) were discovered — 26 of
  27 flagged procedures were found and read in full. See `docs/audit/accounting-integration-audit-followup.md`
  for the complete reconciliation pass.
- `BUG-FIN-001` (`collection-audit.md`) is **downgraded**: reading `sp_UD_InvoicePayment`'s actual body shows
  it ignores whatever amount/status the caller sends and always recomputes `PaymentAmount`/`AIT`/
  `DiscountOnPayment`/`PaymentStatus` fresh from `tblCustPayDetail`/`tblInvoiceDetail`. The C# discarding its
  own computed amount/status (`GAP-APP-016`) is confirmed dead code, but it is **not** a live data-integrity
  bug — the procedure self-heals from source-of-truth tables regardless. Kept at HIGH in the register only
  because the dead-code/maintainability concern remains real; the originally-feared payment-amount corruption
  does not occur.
- New CRITICAL finding from the same follow-up: `BUG-ACC-902` — two of the three invoice-delete stored
  procedures never clean up (or have their GL-cleanup code commented out for) the posted accounting voucher,
  confirmed by reading the actual bodies. See `critical-bugs.md`.
