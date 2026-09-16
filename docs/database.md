# Database — Overview

> **No schema DDL and no stored-procedure body is present in this repository.** Everything in this file and
> in `docs/database/*` is **reconstructed from the C# application code** — the SQL strings, stored-procedure
> names and parameters it builds and executes. A claim marked `CONFIRMED` means "confirmed as referenced by
> the code," never "confirmed as the actual live schema." Column lists, data types, nullability, defaults,
> primary keys, foreign keys, indexes, check constraints and triggers are **UNKNOWN** for every object in this
> database, full stop. See [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) → `GAP-DB-001` (procedure bodies)
> and `GAP-DB-002` (schema). Source of truth for every table/procedure/view name below:
> [`../specs/00-system/database-inventory.md`](../specs/00-system/database-inventory.md) and
> [`../specs/00-system/stored-procedures.md`](../specs/00-system/stored-procedures.md).

## Platform

SQL Server. The application is ASP.NET Web Forms (.NET Framework) using plain ADO.NET — there is no ORM, no
migrations framework, no `.sql` files, no `.dacpac` in the repository.

## Connection strings (`Solution.Web/Web.config`)

| Name | Catalog | Purpose (INFERRED) |
|---|---|---|
| `SolutionConnectionStringSSIDB` | `SalesrollDB_ZAS` | The application's own database. Nearly every screen reads/writes here. |
| `SolutionConnectionStringACCDB` | `ZAS_ACCDB` | Accounting/GL database, reached both by this connection string and by three-part name. |
| `SolutionConnectionStringERPDB` | `CSTL_ERPDB` | Group ERP database (items, material conversion, batch stock). Read-only from this app. |

Two further catalogs — `CSTL_ACCDB` (group accounting: chart of accounts, vouchers, financial year) and
`ZASHRIS` (HR database, source of the employee sync) — have **no connection string entry** and are reached only
by hard-coded three-part name (`Catalog..dbo.Table`) inside SQL strings. `Library.DAL/MAIN_FUNCTION/DB_Names.cs`
additionally hard-codes `SALESDB = "SalesrollDB_ZAS"`, and `DB_Authentication.cs` hard-codes a server name, the
`sa` login and a plaintext password in source (`CONFIRMED`, see
[`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) → `GAP-SEC-003` — treat these credentials as compromised).

## Multi-database integration model

This application talks to **5 catalogs across (at least) 4 separate SQL Server databases**, all via
database-to-database three-part-name references or by opening a second connection string — there is no service
layer, message queue or API between them. See [`integrations.md`](integrations.md) for the full picture (owned
by a sibling documentation pass) and
[`database/relationships.md`](database/relationships.md) for the specific tables involved. Summary:

| Catalog | Reached via | Role (INFERRED) |
|---|---|---|
| `SalesrollDB_ZAS` | connection string | this application's own data |
| `ZAS_ACCDB` | connection string + 3-part name | accounting/GL |
| `CSTL_ERPDB` | connection string + 3-part name | group ERP (items, batch stock, material conversion) |
| `CSTL_ACCDB` | 3-part name only | group accounting (chart of accounts, vouchers, financial year) |
| `ZASHRIS` | 3-part name only | HR (employee master sync) |

## Object counts (from [`../specs/FINAL-REPORT.md`](../specs/FINAL-REPORT.md) §1)

| Object | Count | Confidence |
|---|---|---|
| Tables | 126 | CONFIRMED referenced by code |
| Views | 6 | CONFIRMED referenced by code |
| Stored procedures | 119 | CONFIRMED called by name from code; 0 bodies available |
| Functions (`dbo.fn*`/`udf*`) | 0 found | CONFIRMED absent from call sites — existence in the DB itself UNKNOWN |
| Triggers | UNKNOWN | invisible from application code |
| Foreign keys / indexes / constraints | UNKNOWN | requires the live schema |

## Data-access pattern

Two parallel styles are used everywhere, both through `Library.DAL`:

1. **Ad-hoc SQL routed through one generic procedure.** `ClsCommonInternalDAL` (and equivalents) build a SQL
   string in C# — typically with string concatenation of user input, filter values and dates — and execute it
   via `EXEC ExecuteAllSqlQueryByStoreProcedure @Query = '<string>'`. `Library.DAL` alone contains **about
   1,400** SQL strings built this way, including the **login query**. This is a **CONFIRMED, systemic SQL
   injection** finding — see [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) → `GAP-SEC-005`.
2. **119 named stored procedures**, called with genuine `SqlParameter` lists (not concatenated), covering
   accounting postings, the delivery pipeline, stock posting, weighted-average costing, payment posting, batch
   locking, HR sync, MIGO stock-in and bin cards. Full list, parameters and call sites:
   [`database/stored-procedures.md`](database/stored-procedures.md).

`DB_Manager` (`Library.DAL/MAIN_FUNCTION/DB_Manager.cs`) opens a transaction on connect and commits on close,
but swallows exceptions and returns `0` on failure — the caller cannot distinguish an error from "no rows
matched" (`GAP-APP-008`). `ClsCommonInternalDAL`'s query path has **no transaction** at all.

## See also

| Document | Covers |
|---|---|
| [`database/tables.md`](database/tables.md) | All 126 tables, grouped by subject area, with referencing modules/DAL classes |
| [`database/stored-procedures.md`](database/stored-procedures.md) | All 119 procedures: parameters, callers, inferred purpose, body status |
| [`database/views.md`](database/views.md) | All 6 views |
| [`database/functions.md`](database/functions.md) | Functions (none found in code) |
| [`database/triggers.md`](database/triggers.md) | Triggers (UNKNOWN) |
| [`database/relationships.md`](database/relationships.md) | Inferred logical relationships between major tables |
| [`api.md`](api.md) | The 60 non-postback HTTP endpoints and the DB objects they reach |
| [`integrations.md`](integrations.md) | The cross-database integration model in full (sibling document) |
