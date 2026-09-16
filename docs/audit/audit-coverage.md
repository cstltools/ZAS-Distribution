# Audit Coverage

Maps every phase of the requested audit scope to what was actually performed, what document holds it, and what
is blocked. This is the honesty check for the whole exercise — a phase marked "blocked" here means the
document for that phase says so explicitly and does not fabricate a result.

| Phase | Scope | Status | Document |
|---|---|---|---|
| 1 | Project discovery | Done — baseline reused from `specs/` (415 files), independently spot-verified | `system-audit.md` |
| 2 | Database reverse engineering | Mostly done — live schema extraction found in `docs/database/` (213 tables, 24 views, 7 functions, 0 triggers, 1 FK), spot-verified. SP bodies (119) still unavailable | `database-audit.md`, `system-audit.md` §3 |
| 3 | Module inventory | Done — inferred (no menu/permission table available) but cross-checked against `specs/*/README.md` | `module-audit.md` |
| 4 | Page-by-page audit | Coverage summarized and sampled (3 pages fully re-verified); full 383-page re-trace not repeated (already exists in `specs/`) | `page-audit.md` |
| 5 | Distribution transaction flow audit | Done — reconstructed and re-verified from two existing full workflow traces | `distribution-audit.md` |
| 6 | Inventory/stock audit | Done | `inventory-audit.md` |
| 7 | Sales audit | Done | `sales-audit.md` |
| 8 | Return audit | Done | `return-audit.md` |
| 9 | Collection/receivable audit | Done | `collection-audit.md`, `customer-ledger-audit.md` |
| 10 | Accounting integration audit | Done to the C#/orchestration boundary; internal posting logic blocked by `GAP-DB-001` | `accounting-integration-audit.md` |
| 11 | Accounting reconciliation | Conceptual map only — no live query access in this session | `reconciliation-audit.md` |
| 12 | Financial integrity audit | Done to the extent visible outside stored procedures — no code-level debit=credit guarantee found; procedure-internal guarantee is UNKNOWN | `financial-integrity-audit.md` |
| 13 | Date/period audit | Done | `date-period-audit.md` |
| 14 | Concurrency/duplication audit | Covered inside `transaction-integrity-audit.md` and `data-integrity-audit.md` (MAX(id)+1 races, no optimistic concurrency) | `transaction-integrity-audit.md`, `data-integrity-audit.md` |
| 15 | Transaction/ACID audit | Done — no `TransactionScope`/multi-call `SqlTransaction` found anywhere in `Library.BLL`/`Library.DAL` | `transaction-integrity-audit.md` |
| 16 | Security audit | Done — re-verified all 9 prior `GAP-SEC-*` findings plus 2 new findings | `security-audit.md` |
| 17 | API audit | Done — 13 of 60 endpoints individually spot-checked | `api-audit.md` |
| 18 | Performance audit | Done — pattern-level, CONFIRMED vs POTENTIAL labeled; no query-plan/index-level evidence (needs live DB access) | `performance-audit.md` |
| 19 | Reporting audit | Done to the structural level; `.rpt` internals unreadable (`GAP-RPT-001`) | `reporting-audit.md` |
| 20 | Business rule audit | Done — 11 `BR-AUDIT-*` rules extracted, joining the pre-existing `BR-SYS-*`/`BR-INV-*`/`BR-PRC-*` catalog | `business-rule-audit.md` |
| 21 | Data integrity audit | Done — code-level risk only, framed as risk not confirmed live-data fact | `data-integrity-audit.md` |
| 22 | End-to-end transaction testing | Done conceptually, `TEST-*` scenarios with evidence-based expected outcomes; none executed against a running system | `test-scenarios.md` |
| 23 | Bug classification | Done | `bug-register.md`, `critical-bugs.md`, `high-priority-bugs.md`, `medium-low-bugs.md` |
| 24 | Root cause analysis | Done for the highest-impact findings | `root-cause-analysis.md` |
| 25 | Cross-module impact | Done | `cross-module-impact.md` |
| 26 | Audit documentation | This directory | `README.md` |

## What could not be done in this pass, and why

1. **Stored procedure bodies** — `GAP-DB-001` is **largely closed**, discovered mid-audit:
   `docs/database/stored-procedures-part1.md` through `-part4.md` contain full T-SQL bodies for ~515 procedures
   from a live export, separate from the stale `docs/database/stored-procedures.md` that still says "NOT
   AVAILABLE" for all 119 named-from-C# procedures. A follow-up pass reconciled the highest-value accounting
   procedures against these files — see `docs/audit/accounting-integration-audit-followup.md`. Any remaining
   `HUMAN VERIFICATION REQUIRED` tag on a specific procedure name in this audit means that procedure genuinely
   was not found in the part-files either, not that nobody looked. This session's own direct `sqlcmd` query
   attempt was blocked by sandbox policy, but that turned out not to matter — the bodies were already in the
   repo.
2. **Live reconciliation / live data inspection** (Phases 11, 21 "actual data patterns") — no query execution
   was possible in this session. Everything in this audit that reads as fact about the database schema comes
   from the static files already generated at `docs/database/`, not from a query this session ran itself.
3. **Crystal report internals** (`.rpt` binary format) — `GAP-RPT-001`, unchanged from the `specs/` baseline.
4. **Menu/permission data** (`tblMainMenu`, `tblMenuDistribution`, `tblActionUserWiseApproval`, etc.) —
   `GAP-SEC-001`, unchanged. Every permission-related finding in this audit is about the absence of a backend
   check in code, which is independent of this gap; what remains unknown is the *complete* grant matrix.
5. **Full independent re-trace of all 383 pages** — not repeated; see `page-audit.md` §3.
