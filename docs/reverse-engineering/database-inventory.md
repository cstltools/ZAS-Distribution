# Database Inventory — completeness check

This is the completeness rollup for the database reverse-engineering pass completed 2026-09-05. Unlike the
previous version of this file, every number below comes from **direct inspection of the live SQL Server database**
(`SalesrollDB_ZAS` on `TOWSIF\MSSQLSERVER2019`) — `sys.tables`, `sys.columns`, `sys.indexes`,
`sys.foreign_keys`, `sys.views`, `sys.procedures`, `sys.objects`/`sys.sql_modules`, `sys.triggers` — not
reconstructed from application source. `GAP-DB-001` (no stored-procedure bodies) and `GAP-DB-002` (no schema) from
the prior version of this file are **closed**: a live, queryable copy of the database exists on this machine and
was read directly.

Full detail lives in [`docs/database/`](../database/): [`tables.md`](../database/tables.md),
[`stored-procedures.md`](../database/stored-procedures.md) (index into 4 parts),
[`views.md`](../database/views.md), [`functions.md`](../database/functions.md),
[`triggers.md`](../database/triggers.md), [`relationships.md`](../database/relationships.md).

## Totals

| Category | Found in live DB | Documented this pass | Used by this app's C# source (COVERED) | No app-code caller found (UNUSED) |
|---|---|---|---|---|
| Tables | 213 | 213 (100%) | 112 | 101 |
| Views | 24 | 24 (100%) | 3 | 21 |
| Stored procedures | 515 | 515 (100%) | ~93 | ~422 |
| Scalar/table-valued functions | 7 | 7 (100%) | 1 | 6 |
| Triggers | 0 | 0 (confirmed via `sys.triggers`, not a gap) | — | — |
| Physical foreign keys | 1 | 1 (100%) | 1 (enforced) | — |
| Logical (naming-convention) relationships | 387 (inferred) | 387 | — | — |

Stored-procedure used/unused counts are aggregated from four independently-produced parts (see
[`stored-procedures.md`](../database/stored-procedures.md)) and carry ±a few procedures of counting fuzziness at
the boundaries (a handful are reachable only through commented-out call sites, or call each other internally
rather than being called from C#); the total of 515 is exact and reconciled against `sys.procedures`.

## COVERED / MISSING / UNUSED / CONFLICTING / UNKNOWN

**COVERED** — schema/definition fully documented from the live DB *and* a real application caller was found:
112 tables, 3 views, ~93 stored procedures, 1 function (`SplitString`), the 1 physical FK.

**MISSING (closed this pass)** — existed in the real database but were entirely absent from the prior
documentation:
- **87 tables** (213 live − 126 previously listed).
- **18 views** (24 live − 6 previously listed: `View_CentralStoreCurrentStock`, `View_CustomerMaster`,
  `View_DCStoreCurrentStock`, `View_InvoiceWiseDue`, `View_MIAWiseSalesReport`, `View_ProDCStock` were the only
  ones previously known).
- **7 functions** (previous doc stated "0 found" — all 7 existed all along; the app's C# source genuinely never
  calls 6 of them by name, which is *why* the app-code-only method found none).
- **396 stored procedures** (515 live − 119 previously listed).
- Every column, data type, nullable/default/identity flag, index, and the 1 physical FK for every table — none
  of this existed in any prior version of the docs.
- Full T-SQL bodies for all 515 procedures, 24 views, and 7 functions — the prior docs had procedure *names and
  C#-side call parameters only*, zero bodies.

**UNUSED** (exists in the live DB, real schema/definition now documented, but no reference found anywhere in
`Library.DAL`, `Library.BLL`, or `Solution.Web`): 101 tables, 21 views, ~422 stored procedures, 6 functions. Most
of the unused stored procedures fall into recognizable families — `sp_Webapi_*`/`sp_webapi_*` (mobile/field-force
API), `sp_SalesAPI_*`, `sp_ADJ_*` (accounting-adjustment jobs called only from each other), `sp_CS_Get*`/dropdown
helpers, `Dynamic*` ad-hoc pivot reports — consistent with this repository being one of several applications
against a shared database, not with dead/abandoned code inside this repo. This is a real, load-bearing finding,
not a documentation gap: **do not delete or "clean up" any of these tables/procedures based on this doc** without
checking for other consumers of `SalesrollDB_ZAS` (other apps, Crystal Reports, SQL Agent jobs, ad-hoc BI queries)
— this pass only proves "not called from *this* repository's C#/aspx code."

**CONFLICTING** — prior documentation asserted something the live database contradicts:
- Old `tables.md` marked 126 tables "CONFIRMED referenced" by app code; this pass's grep-based check confirms only
  112 of those as having a live caller. The ~14-table gap is **UNKNOWN — HUMAN VERIFICATION REQUIRED** (could be
  looser matching in the earlier manual pass, DAL classes that build table names dynamically, or genuine
  drift/removal of call sites since the earlier pass was written — not independently re-investigated here).
- Old `functions.md` stated "No `dbo.fn*`/`udf*` call appears in source; existence otherwise is UNKNOWN" — worded
  cautiously enough not to be a hard conflict, but the practical implication ("no functions" as a working
  assumption) is wrong: 7 exist, they're just barely used.
- Old `views.md` listed `View_InvoiceWiseDue` as used by `OrderInfoDAL.GetCustomerDueHistory`; this pass found
  that call site is commented-out dead code — the live implementation uses an inline query instead. Flagged in
  [`views.md`](../database/views.md) as CONFLICTING/superseded, not corrected retroactively in the old doc.
- Old `triggers.md` treated trigger existence as fully UNKNOWN but did not positively assert any trigger exists —
  so this is a resolved-UNKNOWN, not a true conflict (see [`triggers.md`](../database/triggers.md)).

**UNKNOWN — HUMAN VERIFICATION REQUIRED** (genuinely unresolved by this pass, not guessed):
- The ~14-table discrepancy between the old "confirmed referenced" list and this pass's grep results (above).
- 17 ambiguous parent-table matches in [`relationships.md`](../database/relationships.md) where a PK column name
  (e.g. `CategoryId`, `GroupId`, `OrderId`/`OrderDetailId`, `CustPayId`) is shared by 2–3 different tables, so the
  "logical FK" target can't be picked mechanically.
- 176 `...Id`-shaped columns in [`relationships.md`](../database/relationships.md) with no matching PK found
  anywhere in `SalesrollDB_ZAS` — the business-significant ones (`CompanyId`, `WarehouseId`, `BankAccId`,
  `CashAccId`, etc.) most likely reference `ZAS_ACCDB`/`CSTL_ERPDB`/`ZASHRIS` via the cross-database three-part
  names described in the old `relationships.md`, but this was not directly re-verified against those other
  databases this pass (only `SalesrollDB_ZAS` was inspected live).
- Cross-database objects (`ZAS_ACCDB`, `CSTL_ACCDB`, `CSTL_ERPDB`, `ZASHRIS`) — connection strings for these exist
  in `Web.config` and the databases are reachable on the same local SQL Server instance, but this pass scoped all
  live inspection to `SalesrollDB_ZAS` only; the old app-code-only description of cross-database usage in
  `tables.md`/`relationships.md` was left as-is and is still UNKNOWN-quality (not verified against those
  databases' live schemas).
- Business meaning of tables/procedures whose T-SQL is generic/ambiguous or whose only clue is the name (flagged
  individually as `UNKNOWN — HUMAN VERIFICATION REQUIRED` inline in `tables.md`/`stored-procedures-part*.md` where
  encountered).

## Notable defects surfaced as a side effect (not the primary goal of this pass, but found along the way)

Several real code-quality/correctness issues turned up while reading full T-SQL bodies and app callers. Recorded
here so they aren't lost; none were "fixed" as part of this documentation pass:
- `GetDateWiseAttendence` (function) has a commented-out `WHERE` clause, so it silently returns unfiltered data
  regardless of the parameters passed — see [`functions.md`](../database/functions.md).
- Several `sp_Get_*`/reporting procedures build dynamic SQL by directly concatenating caller-supplied parameters
  (`EXEC`/`sp_executesql` over unparameterized string concatenation) — SQL-injection-shaped, though currently
  UNUSED from this app. See `stored-procedures-part1.md`/`part2.md`.
- Legacy `sp_NonTranscationalInvoiceApproval` reassigns a variable instead of a column (`SET @ApprovedDate =
  @ApprovedDate`); superseded by `sp_NonTranscationalInvoiceApproval_New`, which is the one actually called.
- `sp_Process_WeightedAverage` contains a leftover debug `SELECT * FROM tbl_INV_WeightedAvg` inside its cursor
  loop.
- `sp_Integration_CustomerSubsidaryInsert` ignores its `@CustomerId` parameter and processes all customers.
- `sp_Webapi_Save_UserDeviceToken` contains a plaintext-password login check.
- None of the bulk `sp_Missing_*Posting`/opening-balance/GL-posting jobs use an explicit transaction or TRY/CATCH
  except `sp_NonTranscationalInvoiceApproval_New` and `sp_Process_TradeMaterialTransferFullProcess` — a failure
  partway through any of the others leaves partial writes with no rollback.
- `Library.DAL` mixes raw string-concatenated SQL (e.g. `ProductDAL.SaveDataForProduct`) with parameterized
  stored-procedure calls throughout — the raw-SQL paths are SQL-injection-shaped wherever user input reaches them
  unescaped. Not re-audited exhaustively here; flagged because it directly affects how reliable "tables
  read/written" static analysis can be (dynamic SQL can reference tables not visible to simple string search).

## Sources

- Live database: `SalesrollDB_ZAS` on `TOWSIF\MSSQLSERVER2019` (connection string active in
  `Solution.Web\Web.config`), queried directly via `sqlcmd`/`osql` against `sys.*` catalog views and
  `sys.sql_modules.definition`.
- [`docs/database/tables.md`](../database/tables.md) — all 213 tables, full column/index/FK detail.
- [`docs/database/stored-procedures.md`](../database/stored-procedures.md) → parts 1–4 — all 515 procedures,
  full verbatim bodies.
- [`docs/database/views.md`](../database/views.md) — all 24 views, full verbatim SQL.
- [`docs/database/functions.md`](../database/functions.md) — all 7 functions, full verbatim bodies.
- [`docs/database/triggers.md`](../database/triggers.md) — confirms 0 triggers exist.
- [`docs/database/relationships.md`](../database/relationships.md) — 1 physical FK + 387 logical relationships.
- Application cross-reference: `Library.DAL`, `Library.BLL`, `Solution.Web` (`.cs`/`.aspx`), grepped per object
  name for callers.
