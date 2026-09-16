# System Inventory

**Status: CONFIRMED counts, sourced verbatim from [`specs/FINAL-REPORT.md`](../../specs/FINAL-REPORT.md) §1.**
This file restates that report's totals table as the canonical entry point for `docs/reverse-engineering/`. It
does not recompute anything — if a number here ever disagrees with `specs/FINAL-REPORT.md`, the specs file is
authoritative and this file is stale (see the Conflict Handling rule in [`.claude/SKILL.md`](../../.claude/SKILL.md)).

Confidence model used throughout `docs/reverse-engineering/`: **CONFIRMED** (parsed from source), **INFERRED**
(a reasonable reading of source with no ground truth to check it against), **UNKNOWN** (the artefact needed to
answer does not exist in this repository).

## Totals

| # | Item | Count | How it was counted |
|---|---|---|---|
| 1 | Modules | 11 (INFERRED) | No module concept exists in code — 272 pages sit in one flat folder. Grouping is inferred from page names, DAL classes and tables. Real grouping is `tblMainMenu.ModuleId`, which is data (`GAP-STR-001`). |
| 2 | Sub-modules | UNKNOWN | Same reason as modules. |
| 3 | Pages / screens | 383 | Every `.aspx` outside `PrecompiledWeb`: 272 `SInventory_UI`, 82 `SInventory_RPTVIEW`, 18 `HTMLReport`, 9 `CommonUI`, 1 `Dashboard`, 1 `Login`. |
| 4 | Controllers / actions | 0 / 0 | Web Forms — no MVC, no Razor Pages, no routing table, no Areas. |
| 5 | Views / code-behind classes | 383 | One `.aspx.cs` per page; none missing. |
| 6 | Features (wired UI events) | 1,484 | Distinct `OnClick` / `OnSelectedIndexChanged` / `OnTextChanged` / `OnCheckedChanged` / `OnRowCommand` / … handlers declared in markup. |
| 7 | UI controls | 7,119 | `runat="server"` controls with an id, per page. |
| 8 | GridViews | 191 | With their columns, headers, bound expressions and template controls. |
| 9 | Code-behind methods | 3,659 | Brace-matched parse. |
| 10 | APIs | 60 | 18 ASMX methods (exposed on 2 endpoints) + 40 page `[WebMethod]`s + 2 `.ashx` handlers. |
| 11 | Reports | 166 `.rpt` files / 146 distinct names, rendered by 100 viewer + HTML-report pages | `Solution.Web/Reports` (124) and `Library.CrystalReports/SInventory_RPT` (42). |
| 12 | Services (BLL classes) | 76 in 77 files, 923 methods | `Library.BLL`. |
| 13 | Repositories (DAL classes) | 144 in 145 files, 1,554 methods | `Library.DAL`. |
| 14 | DTOs / ViewModels / entities | 119 classes in 99 files | `Library.DAO` — plain property bags; there is no DTO or ViewModel layer. |
| 15 | Helpers / infrastructure | 6 | `ClsCommonInternalDAL`, `DB_Manager`, `DB_Connection`/`DB_Authentication`/`DB_Names`, `ClsPrimaryKeyFind`, `ClsApprovalAction`, `clsNum2Word`. |
| 16 | Middleware / filters / background jobs | 0 | Verified absent: no HTTP module, no handler beyond the two `.ashx` + Crystal's, no scheduler, no DI container; all five `Global.asax` event bodies are empty. |
| 17 | Stored procedures | 119 | Called by name from C#; **0 bodies available**. |
| 18 | Tables | 126 | Reconstructed from SQL strings. |
| 19 | Views | 6 | `View_CentralStoreCurrentStock`, `View_CustomerMaster`, `View_DCStoreCurrentStock`, `View_InvoiceWiseDue`, `View_MIAWiseSalesReport`, `View_ProDCStock`. |
| 20 | Functions | 0 found | No `dbo.fn*` / `udf*` call appears in the source; whether any exist is UNKNOWN. |
| 21 | Triggers | UNKNOWN | Invisible from application code; requires the schema. |
| 22 | Foreign keys / indexes / constraints | UNKNOWN | Same reason. |
| 23 | Integrations | 4 databases | `ZAS_ACCDB`, `CSTL_ACCDB`, `CSTL_ERPDB`, `ZASHRIS` — all database-to-database, 30 three-part-name references. |
| 24 | Configuration | 3 connection strings + 3 Crystal app settings | `Web.config`; plus hard-coded credentials in `DB_Authentication.cs`. |
| 25 | Authentication | 1 mechanism | Custom, session-based, plain-text password. |
| 26 | Authorisation | 4 levels | Menu grant, company unit, approval action, one-off user flags. |

## Sources

- [`specs/FINAL-REPORT.md`](../../specs/FINAL-REPORT.md) — origin of every number above (§1 "Totals found").
- [`specs/README.md`](../../specs/README.md) — module table and specification map.
- [`specs/00-system/code-inventory.md`](../../specs/00-system/code-inventory.md), [`specs/00-system/api-inventory.md`](../../specs/00-system/api-inventory.md), [`specs/00-system/database-inventory.md`](../../specs/00-system/database-inventory.md) — per-area detail behind rows 5–24.

## Where this leads next

- Per-module breakdown → [`module-inventory.md`](module-inventory.md)
- Per-page breakdown → [`page-inventory.md`](page-inventory.md)
- API detail → [`api-inventory.md`](api-inventory.md)
- Database detail → [`database-inventory.md`](database-inventory.md)
- How a request actually flows end to end → [`dependency-map.md`](dependency-map.md)
- Everything still missing and why → [`gaps.md`](gaps.md)
