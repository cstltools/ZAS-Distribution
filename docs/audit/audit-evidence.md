# Audit Evidence Index

This audit deliberately does not duplicate evidence already captured in full elsewhere. Use this index to find
where the underlying evidence for any claim lives, rather than searching each `docs/audit/*.md` file
individually.

## Evidence sources, by kind

| Kind of evidence | Where it lives | Notes |
|---|---|---|
| Application source (C#) | `Library.BLL/`, `Library.DAL/`, `Library.DAO/`, `Solution.Web/` | The primary evidence base for every finding in this audit; every `docs/audit/*.md` file cites exact file:line locations |
| Page-by-page trace (UI→DB) | `specs/*/pages/PAGE-*.md` (383 files) | Produced by a prior reverse-engineering pass, independently spot-verified by this audit (see `page-audit.md` §1) |
| Full workflow traces | `specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md`, `specs/delivery-logistics/workflows/WF-delivery-confirmation.md`, `specs/finance-collection/workflows/WF-customer-payment.md`, `specs/00-system/workflows/WF-approval.md` | The four deepest algorithmic traces available; re-verified and built on throughout this audit |
| Prior gap/defect catalog | `specs/SPEC-GAPS.md` (44 `GAP-*` IDs) | Baseline findings, all re-verified by this audit before being carried forward — see each domain audit's "baseline confirmed" section |
| Database schema (tables, views, functions) | `docs/database/tables.md`, `views.md`, `functions.md`, `triggers.md`, `relationships.md` | Live SQL Server extraction, dated 2026-09-05, spot-verified real by this audit (`system-audit.md` §3) |
| Stored procedure bodies | `docs/database/stored-procedures-part1.md` through `-part4.md` | ~515 procedures with full T-SQL, discovered mid-audit; `docs/database/stored-procedures.md` (the file `specs/` originally pointed to) is stale and still says "NOT AVAILABLE" — do not use it |
| Procedure-level reconciliation | `docs/audit/accounting-integration-audit-followup.md` | The 26-of-27 highest-priority procedures read in full, with what each one actually does |
| Every bug found, indexed | `docs/audit/bug-register.md` | Master index; full detail split across `critical-bugs.md`/`high-priority-bugs.md`/`medium-low-bugs.md` |

## How to independently verify any specific claim

1. Find the claim in its domain document (e.g. a stock finding is in `inventory-audit.md`).
2. Open the exact `file:line` cited there — every finding in this audit's own documents (as opposed to the
   `specs/` baseline) was written by an agent that read the file directly in this session, not from memory.
3. For anything citing a stored procedure body, the citation gives a `docs/database/stored-procedures-partN.md`
   line number — open that file at that line to see the actual `CREATE PROCEDURE` text.
4. For anything marked `HUMAN VERIFICATION REQUIRED`, that specific claim could **not** be verified by this
   audit — do not treat it as fact until checked against a live system or an artifact not present in this
   repository.

## What this audit did NOT independently verify

- **Live data.** No query was run against the database in this session (a direct attempt was blocked by
  sandbox policy — see `system-audit.md` §3). Every schema/procedure claim comes from static files already
  present in the repository, not from a fresh query.
- **All 383 pages line-by-line.** Only a sample was independently re-read against `.aspx.cs`; see `page-audit.md`
  §1.
- **Crystal report internals** (`.rpt` binary format) — unreadable by any means available in this session.
- **Runtime behavior.** Nothing in this audit was observed running; every finding is a static-analysis
  conclusion about what the code *would* do, not an observation of it actually happening in production.

## Session/environment note

This audit ran with Bash/PowerShell access to the repository but with `sqlcmd`/live database queries blocked
by the session's own sandbox policy (not a database permission issue — the local SQL Server instance named in
`docs/database/tables.md`, `TOWSIF\MSSQLSERVER2019`, appears reachable in principle). A human running this
audit's unresolved items (Tier 0 #4 in `remediation-plan.md`, and any live-data verification in
`reconciliation-audit.md`/`test-scenarios.md`) should expect this to take under an hour given that live access
clearly existed recently enough to produce the schema and procedure extractions already sitting in
`docs/database/`.
