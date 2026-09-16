# System Audit

Scope: Phase 1 (project discovery) and Phase 3 (module inventory), independently verified against
`specs/00-system/system-overview.md`, `specs/FINAL-REPORT.md` and fresh reads of `Web.config` /
`Library.DAL/MAIN_FUNCTION/DB_Authentication.cs`. This document does not repeat the reverse-engineering detail
in `specs/` — it audits it, spot-checks its claims, and states what is fit for reliance.

## 1. What the system is

CONFIRMED (spot-checked against `Web.config`, `Solution.sln`, `specs/00-system/system-overview.md`):

- Product: "Salesroll", customer ZAS. ASP.NET Web Forms, .NET Framework 4.0, no MVC/API framework, no ORM
  (Enterprise Library Data Access Application Block + ad-hoc SQL).
- Layers: `Solution.Web` (383 `.aspx` pages) → `Library.BLL` (74 classes, mostly pass-through per
  `GAP-STR-005`) → `Library.DAL` (130 classes, ~1,400 inline SQL strings + 119 named stored procedures) →
  SQL Server (`SalesrollDB_ZAS`).
- No test project, no CI, no migrations, no logging (log4net referenced but never configured — CONFIRMED by
  absence of any `log4net.Config` call and an empty `Global.asax.Application_Error`).
- The same codebase serves other deployments (commented-out `Web.config` connection strings for
  `SalesRollDB_FC`, `SalesDisDB_SMC`) — confirms this is a shared product, not a bespoke build for ZAS alone,
  which matters for any fix: a change here likely needs to be safe for those other tenants too.
  **HUMAN VERIFICATION REQUIRED**: whether those other tenants are still live and on the same code branch.

## 2. Baseline reliability

The `specs/` reverse-engineering set (415 files, produced against this same repository) is the most complete
artifact available and is treated here as the Phase 1–4 baseline rather than re-derived from scratch, per its
own statement of accuracy (`specs/FINAL-REPORT.md` §8: every count parsed from source, not estimated). This
audit independently verified a sample rather than accepting it uncritically:

| Claim in `specs/` | Verification performed | Result |
|---|---|---|
| Hard-coded `sa` credentials in `DB_Authentication.cs` | Read the file directly | CONFIRMED — see `docs/audit/security-audit.md` |
| SQL injection via `ExecuteAllSqlQueryByStoreProcedure` | Fresh grep for the pattern in `Library.DAL` | CONFIRMED — see `docs/audit/security-audit.md` |
| 119 stored procedures, 0 bodies available | Searched repository for `.sql`, `CREATE PROCEDURE`, `sp_helptext` output | CONFIRMED — no such artifact exists anywhere in the repo tree |
| No DB schema/DDL anywhere in repo | Searched for `.sql`, `database/`, `*.dacpac`, `*.bak` | CONFIRMED absent |
| GAP-APP-002/003/016/017/020 (correctness findings) | Re-read cited files | See `docs/audit/sales-audit.md`, `collection-audit.md` — confirmed still present |

**Conclusion**: the `specs/` baseline is reliable for Phases 1–4 (structure, inventory, page-level tracing).
It is explicitly *not* a security or correctness sign-off — it says as much itself — and its own "findings"
sections (`GAP-SEC-*`, `GAP-APP-*`) are carried into this audit's bug register with their original evidence,
not restated from memory.

## 3. What this audit cannot do, and why

**Correction to the `specs/SPEC-GAPS.md` baseline, found during this audit**: `GAP-DB-002` ("no database
schema") is **no longer accurate**. `docs/database/tables.md` (dated 2026-09-05, the day of this audit) states
it was rewritten against a **live SQL Server schema extraction** of `SalesrollDB_ZAS` on instance
`TOWSIF\MSSQLSERVER2019` — 213 tables, 2,438 columns, full column/type/nullability/identity/default/PK/index
detail, plus a confirmed **zero computed columns**, **zero triggers**, and only **one physical foreign key in
the entire database** (`tblPettyCashDetails.PettyCashmasterId → tblPettyCashMaster.PettyCashmasterId`). This
audit spot-checked `docs/database/tables.md` directly (read the `Employee` and other table entries) and
confirms the extraction is real and detailed, not a restatement of the old UNKNOWN. **`docs/database/views.md`
and `docs/database/functions.md` were likewise rewritten with full definitions (24 views, 7 functions).**
This means Phase 2 (database structure) is now largely CONFIRMED rather than UNKNOWN — see
`docs/audit/database-audit.md`. The near-total absence of enforced foreign keys (1 of an estimated 100+ logical
parent-child relationships) is itself one of this audit's most important findings — see `BUG-DATA-001` in
`docs/audit/data-integrity-audit.md`.

**Second correction, found later in this same audit pass**: `GAP-DB-001` is **also substantially closed**,
just not where the earlier part of this audit looked. `docs/database/stored-procedures.md` (the file named by
`specs/`) still marks all 119 bodies "NOT AVAILABLE" — but four sibling files exist alongside it,
`docs/database/stored-procedures-part1.md` through `-part4.md` (dated the same day), containing **full T-SQL
bodies for roughly 515 procedure entries**, explicitly reverse-engineered "directly from the live
`SalesrollDB_ZAS` T-SQL definitions" via a prior `sqlcmd` export. This audit spot-verified the claim by reading
`stored-procedures-part1.md` directly — it contains real `CREATE PROCEDURE` bodies, not placeholders. This
session's own attempt to query the database directly was blocked by sandbox policy (below), but that no longer
matters for most procedures: **the bodies already exist in the repository, in files the original `specs/`
baseline and this audit's first pass did not know to check.**

Practical effect: every finding elsewhere in this audit marked `HUMAN VERIFICATION REQUIRED — needs sp_helptext
for <name>` should be re-checked against `docs/database/stored-procedures-part{1,2,3,4}.md` before being
treated as genuinely blocked — several already were, mid-audit, by the inventory/stock and collection/ledger
sub-audits once this was discovered (see `docs/audit/inventory-audit.md` and `docs/audit/collection-audit.md`
for examples of findings resolved this way). A follow-up pass reconciled the highest-value remaining ones — see
`docs/audit/accounting-integration-audit-followup.md`. Some procedures genuinely remain absent even from the
part-files (they cover ~515 of the ~600+ procedure/function names referenced across the app); those keep the
`HUMAN VERIFICATION REQUIRED` label.

This is recorded once here so every downstream document can point back to it instead of repeating the
explanation, and so a reader does not mistake "this audit said UNKNOWN" for "this is permanently unknowable" —
in most cases it was knowable from files already sitting in this repository.

## 4. Module inventory (Phase 3)

There is no module table or module concept in code or database (menu/permission tables — `tblMainMenu`,
`tblMenuDistribution` — are themselves absent from the repository, `GAP-SEC-001`). Module boundaries below are
**INFERRED** from the folder structure already built in `specs/` (grouped by page name and DAL class, not by
real menu data) — carried forward, not re-derived, because there is no better source available without DB
access.

| Module (inferred) | Pages | Confirmed present in this codebase? |
|---|---|---|
| System/Admin (users, roles, menu, approval, backup) | 18 | Yes — `specs/00-system/pages/` |
| Master data (customer, product, territory, employee, pricing masters) | 75 | Yes — `specs/master-data/` |
| Sales / Order / Invoice | 27 | Yes — `specs/sales-order-invoice/` |
| Inventory / Warehouse (stock receive/issue/transfer/adjustment/destroy) | 41 | Yes — `specs/inventory-warehouse/` |
| Delivery / Logistics (picking, top sheet, delivery confirmation, B2B transfer) | 19 | Yes — `specs/delivery-logistics/` |
| Finance / Collection (payment, deposit, customer ledger, dues) | 27 | Yes — `specs/finance-collection/` |
| Returns | 5 | Yes — `specs/returns/` |
| Pricing / Discount / Campaign | 15 | Yes — `specs/pricing-discount-campaign/` |
| Dashboard / Analytics | 8 | Yes — `specs/dashboard-analytics/` |
| Reports | 146 | Yes — `specs/reports/` |
| Integrations (HR sync, MIGO) | 2 | Yes — `specs/integrations/` |

Modules explicitly **not found** despite being named in the generic audit checklist that scoped this exercise:
a distinct "Supplier"/"Purchase Order" transactional module (only `SupplierInformation` master data exists —
see `docs/audit/purchase-audit.md`), a "General Ledger"/"Journal"/"Voucher" UI module (accounting is delegated
entirely to stored procedures and the 4 external databases — see `docs/audit/accounting-integration-audit.md`),
and a distinct "Tax/VAT" configuration module (VAT appears as invoice-line fields, not as a maintained tax
table — **HUMAN VERIFICATION REQUIRED**).

## 5. Cross-reference

Page-by-page tracing (Phase 4) is not repeated here — it already exists at page-spec granularity across the
383 files under `specs/*/pages/*.md`, each following UI → JS → AJAX → code-behind → BLL → DAL → SP → table →
column. This audit's page-level contribution is `docs/audit/page-audit.md`, which summarizes coverage and
flags where that trace stops at the stored-procedure boundary.
