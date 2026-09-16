---
name: salesroll-zas
description: How to work in this specific codebase (SalesRoll / ZAS Distribution ERP) — architecture, conventions, security rules, and the workflow to follow before changing anything. Use for any task touching Solution.Web, Library.BLL, Library.DAL, Library.DAO or Library.CrystalReports in this repository.
---

# Working in SalesRoll (ZAS Distribution)

This file tells you **how** to work on this codebase. It does not describe what any page or feature does — for
that, read the page spec under `specs/<module>/pages/PAGE-*.md` first. This file is a working reference; it
links out to `specs/` and `docs/reverse-engineering/` for detail rather than repeating it.

## Project architecture

ASP.NET **Web Forms** on **.NET Framework 4.0**, `Solution.Web` as a **Web Site** project (`.aspx.cs` compiles
at run time, not a Web Application project) against **SQL Server** (`SalesrollDB_ZAS`, plus three more databases
reached by three-part name: `ZAS_ACCDB`, `CSTL_ERPDB`, `CSTL_ACCDB`, and `ZASHRIS`). Layers: `Solution.Web`
(pages) → `Library.BLL` (business logic, thinner than it looks) → `Library.DAL` (data access, ADO.NET via
Enterprise Library's Data Access Block) → SQL Server, with `Library.DAO` as plain property-bag types crossing
layer boundaries and `Library.CrystalReports` hosting the `.rpt` report definitions. **There is no MVC, no
dependency injection, no repository interfaces, no ORM, and no middleware pipeline** — classes are instantiated
with `new` directly in the page that uses them. Authentication is a single custom, session-based mechanism with
a plain-text password comparison; there is no ASP.NET Membership/Identity. Full detail:
[`specs/00-system/architecture.md`](../specs/00-system/architecture.md), synthesis in
[`docs/reverse-engineering/dependency-map.md`](../docs/reverse-engineering/dependency-map.md).

## Coding conventions

Read [`specs/00-system/conventions.md`](../specs/00-system/conventions.md) once before touching an unfamiliar
page — it documents the repeated patterns (page layout, the hand-rolled editable line-item grid, dropdown
loading helpers, validation shape, message boxes, report-opening pattern, Entry/View/Edit triples) that make
every page predictable. **When adding a new page, match the existing pattern exactly** rather than introducing a
new one — e.g. reuse the existing `Validation()`/`showMessageBox` shape, not `asp:*Validator` controls (only 1
exists in the whole app). **Misspellings in identifiers are part of the contract** (`Delivary`, `Transcation`,
`Wearhouse`, etc.) — never "fix" them; quote them exactly as they appear in code/tables/columns.

## Naming conventions

| Thing | Convention | Example |
|---|---|---|
| Page class | `<Folder>_<PageName>` (Web Site auto-generated) | `SInventory_UI_InvoiceCreationForCustomer` |
| Control | camelCase + type suffix | `custCodeTextBox`, `saveButton`, `gridLineItemGridView` |
| Hidden field | `hd` prefix | `hdComUnitId` |
| Layer object field | `a` + ClassName | `aInvoiceBll`, `aCommonInternalDal` |
| BLL / DAL class | `<Entity>BLL` / `<Entity>DAL` (casing inconsistent: `Dal`/`DAL`/`Bll`/`BLL`) | `InvoiceBLL`, `SampleIssueDal` |
| Table | `tbl<Entity>` | `tblInvoiceDetail` |
| View | `View_<Name>` | `View_DCStoreCurrentStock` |
| Stored procedure | `sp_<Area>_<Action>`, also `sp_I_`/`sp_UD_`/`sp_DEL_`/`sp_GET_`/`sp_Save_` | `sp_Sales_DeliverySalesPosting` |

## Database conventions

Two ways SQL reaches the server, per [`specs/00-system/architecture.md`](../specs/00-system/architecture.md):

1. **Named stored procedures with `SqlParameter`** (the minority, ~119 procedures) — used for postings that
   matter (stock, accounting, delivery, payment). This is the pattern to follow.
2. **A string built by concatenation, passed as one parameter to `EXEC ExecuteAllSqlQueryByStoreProcedure`**
   (the majority, ~1,400 call sites in `Library.DAL`). **This is a confirmed critical SQL-injection defect
   (`GAP-SEC-005`) — it includes the login query.**

**NEW code must never add another instance of pattern 2.** Any new query — however small, however "internal" the
screen — must use a parameterised `SqlParameter` list or a named stored procedure. Do not copy the nearest
existing DAL method if that method concatenates; copy a parameterised one instead, or ask before extending a
concatenating one further. Stored-procedure bodies are **not in this repository** (`GAP-DB-001`) — you cannot
verify what a procedure does beyond its name and the parameters C# passes to it.

## API conventions

No REST/no attribute routing/no Web API controllers. The 60-endpoint surface is: 18 ASMX methods on
`SInventoryWebService.asmx`/`WebService.asmx` (both the same class), 40 page-level `[WebMethod]`s (dashboard
module only), 2 `.ashx` handlers. New client-callable endpoints, if ever needed, should follow the
`[WebMethod]` + `PageMethods`/`$.ajax` pattern already used by the dashboard pages — **and must check
`Session["UserId"]`**, unlike the existing ASMX methods (see Security rules). Detail:
[`docs/reverse-engineering/api-inventory.md`](../docs/reverse-engineering/api-inventory.md).

## UI conventions

Web Forms server controls inside an HTML `<table>` layout, wrapped in `UpdatePanel`s for partial postbacks.
Editable line-item grids are hand-rolled (`InitialGrid()` builds an in-memory `DataTable`; add/remove buttons
re-read the whole grid; cells are addressed **positionally** via `Rows[i].Cells[n].FindControl(...)`) — reordering
a grid column silently breaks the code-behind, so never insert a column without updating every `Cells[n]` index
that follows it. No control uses `AssociatedControlID`; a field's label is the nearest preceding text node.
Detail: [`specs/00-system/conventions.md`](../specs/00-system/conventions.md).

## Frontend conventions

jQuery-era, minimal client logic. Of 21 project-owned scripts, 18 are third-party theme code with no business
logic (`GAP-JS-001`) — business logic belongs in the code-behind (server-side postback), not in JavaScript,
except for the dashboard module's `[WebMethod]` + client-side-JSON pattern. Do not add new client-side business
logic outside that established dashboard pattern; server-side validation is mandatory regardless (client-side
`FilteredTextBoxExtender` is presentation-only and never re-checked on the server today — don't repeat that gap
in new code).

## Security rules

Nine confirmed security findings exist in this application (`GAP-SEC-001` through `GAP-SEC-009`, detail in
[`specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md), index in
[`docs/reverse-engineering/gaps.md`](../docs/reverse-engineering/gaps.md)). **Never introduce a new instance of
any of these patterns:**

1. `GAP-SEC-001` — menu-item visibility is not access control. Never treat "hidden from the menu" as "protected."
2. `GAP-SEC-002` — 127 of 383 pages have no auth guard. **Every new page must use `MainMasterPage` and inherit
   its session check**, or perform the equivalent check itself if it genuinely cannot use that master page
   (e.g. a popup report viewer) — copy the check from `MainMasterPage.master.cs`, the one correct example in
   the codebase, never skip it.
3. `GAP-SEC-003` — hard-coded credentials in `DB_Authentication.cs`/`Web.config`. **Treat everything already
   there as compromised** — never reuse or extend those values, never add new hard-coded credentials anywhere,
   and flag to the user that they should be rotated if the task touches that area.
4. `GAP-SEC-004` — unauthenticated ASMX autocomplete leaking customer/product/price/employee data. Any new
   ASMX/WebMethod must check session.
5. `GAP-SEC-005` — systemic SQL injection via string concatenation. See Database conventions above — never add
   more of this.
6. `GAP-SEC-006` — unquoted/unencoded `href=` from `tblMainMenu.URL` (stored XSS). Always encode output written
   into HTML from data.
7. `GAP-SEC-007` — plaintext password storage/comparison. **Never add new plaintext password handling** — if a
   task touches authentication, flag the existing plaintext scheme rather than silently perpetuating it.
8. `GAP-SEC-008` — `debug="true"`, no `<customErrors>`, 16-day session timeout in production config. Don't widen
   this further; flag if asked to touch `Web.config`.
9. `GAP-SEC-009` — no horizontal access control (ids in URL/Session aren't ownership-checked). Don't assume an
   id from `Session`/query string belongs to the current user without a check, even though existing code mostly
   doesn't check either — don't add a *new* unchecked path if you can avoid it, and flag the gap if the task
   depends on it.

## Testing rules

No test project exists anywhere in the repository (`GAP-TEST-001`). When adding code with non-trivial logic (a
branch, a loop, a parser, anything touching money or stock), leave **one runnable check** behind — the smallest
thing that fails if the logic breaks (an assert-based self-check, or one small test file) — even without a test
framework in place. Page specs already propose test cases per page (derived from the code's own validation
messages, never actually run) — read the relevant page's spec section 27 before inventing new ones.

## Documentation rules

- `specs/` is the **page-level source of truth** — one file per page under `specs/<module>/pages/PAGE-*.md`,
  fixed 31-section template, 383 files, verified against source.
- `docs/` (including `docs/reverse-engineering/`) is the **system-level narrative** — indexes, summaries, and
  synthesis that point back into `specs/` rather than duplicating it.
- Always distinguish **CONFIRMED** (parsed from source) / **INFERRED** (a reasonable reading with no ground
  truth) / **UNKNOWN** (the artefact needed to answer doesn't exist in this repo), and cite the source file for
  any claim.
- When either `specs/` or `docs/` becomes stale relative to the actual code, **correct it, don't leave it
  silently wrong** — but do not touch application code to make a doc "true"; fix the doc.

## Reverse-engineering workflow

When investigating an unfamiliar page or feature:

1. Read `specs/<module>/pages/PAGE-<ID>-<Name>.md` for that page first (find it via
   [`specs/INVENTORY-PAGES.md`](../specs/INVENTORY-PAGES.md)).
2. Then read the actual `.aspx` → `.aspx.cs` → BLL → DAL chain in source, confirming what the spec says still
   matches (specs were correct at the time they were written, not guaranteed to still be correct now).
3. Only then consider making a change.

## Spec-driven development workflow

For any implementation task, follow this pipeline in order:

1. **REQUEST** — restate what's being asked.
2. **READ SPEC** — the relevant `specs/<module>/pages/PAGE-*.md` (or module README) for the page/feature.
3. **READ RULES** — this file.
4. **READ ARCHITECTURE** — [`specs/00-system/architecture.md`](../specs/00-system/architecture.md) and
   [`docs/reverse-engineering/dependency-map.md`](../docs/reverse-engineering/dependency-map.md).
5. **READ DB/API DOCS** — [`docs/reverse-engineering/database-inventory.md`](../docs/reverse-engineering/database-inventory.md),
   [`docs/reverse-engineering/api-inventory.md`](../docs/reverse-engineering/api-inventory.md), and the deeper
   `docs/database.md`/`docs/api.md`/`specs/00-system/stored-procedures.md` they point to.
6. **SEARCH EXISTING IMPL** — grep for the actual classes/methods/tables involved.
7. **FIND SIMILAR FEATURE** — locate the nearest existing page/pattern doing something equivalent; this
   codebase rewards imitation over invention (see Coding conventions).
8. **PLAN** — state the plan before editing.
9. **IMPLEMENT** — following the conventions above, especially the SQL-injection and auth-guard rules.
10. **TEST** — leave the one runnable check (see Testing rules).
11. **VERIFY AGAINST SPEC** — does the change still match what the page spec says the page does?
12. **SPEC VS ACTUAL** — if they now disagree, that's a doc that needs updating, or a bug that needs flagging.
13. **UPDATE DOCS** — correct the relevant `specs/`/`docs/` file if it's now stale.
14. **REPORT** — summarize what changed and why, and flag anything CONFIRMED/INFERRED/UNKNOWN that mattered.

## Change control

- **Never modify a stored-procedure body blind.** Bodies are not in this repository (`GAP-DB-001`). If a task
  requires changing SP-backed behaviour, get the SP script from the user/DBA first — do not guess at what a
  procedure does from its name and parameters.
- **Never silently "fix" a `GAP-APP-*` defect** (e.g. `GAP-APP-002` discount mismatch, `GAP-APP-017` one-receipt
  N-masters) without flagging it to the user first. Some of these are load-bearing behaviour that downstream
  reports or reconciliations may already depend on — nobody has confirmed they're safe to change.

## Database safety

- **Never run destructive operations against the real `SalesrollDB_ZAS`** (or any of `ZAS_ACCDB`, `CSTL_ERPDB`,
  `CSTL_ACCDB`, `ZASHRIS`) from this environment.
- **Treat every credential in `DB_Authentication.cs` and `Web.config` as already compromised** (`GAP-SEC-003`)
  — never reuse or extend them, and tell the user they should be rotated if a task brings you near that file.

## Verification

Before stating a fact about this system, check it against `specs/` or the actual source — never invent a table
name, a procedure's behaviour, or a page's logic. If `specs/` doesn't cover it, say `UNKNOWN` and name what
artefact would answer it (schema, SP body, `tblMainMenu` data), following the pattern already used throughout
`specs/SPEC-GAPS.md`.

## Conflict handling

If `specs/`/`docs/` and the current source code disagree, **trust the source code** and flag the doc as stale —
do not silently trust a doc that no longer matches what's actually deployed.
