# Final Report — reverse-engineering audit of SalesRoll (ZAS Distribution)

## 0. Starting position

The repository contained **no `specs/`, no `docs/`, no `.claude/` and no `database/` directory** — verified at
the repository root before any work began. There was therefore nothing to audit against and nothing to trust:
this is a from-scratch reverse engineering, not an update. Every statement below is derived from the source
code, the markup, `Solution.sln` and `Web.config`.

## 1. Totals found

| # | Item | Count | How it was counted |
|---|---|---|---|
| 1 | **Modules** | **11** (INFERRED) | The application has no module concept in code — 272 of its pages sit in one flat folder. Grouping was inferred from page names, DAL classes and tables. The real grouping is `tblMainMenu.ModuleId`, which is data (`GAP-STR-001`). |
| 2 | **Sub-modules** | UNKNOWN | Same reason. |
| 3 | **Pages / screens** | **383** | every `.aspx` outside `PrecompiledWeb`: 272 `SInventory_UI`, 82 `SInventory_RPTVIEW`, 18 `HTMLReport`, 9 `CommonUI`, 1 `Dashboard`, 1 `Login` |
| 4 | **Controllers / actions** | **0 / 0** | Web Forms — no MVC, no Razor Pages, no routing table, no Areas |
| 5 | **Views / code-behind classes** | **383** | one `.aspx.cs` per page; none missing |
| 6 | **Features (wired UI events)** | **1,484** | distinct `OnClick` / `OnSelectedIndexChanged` / `OnTextChanged` / `OnCheckedChanged` / `OnRowCommand` / … handlers declared in markup |
| 7 | **UI controls** | **7,119** | `runat="server"` controls with an id, per page, in section 7 of each spec |
| 8 | **GridViews** | **191** | with their columns, headers, bound expressions and template controls |
| 9 | **Code-behind methods** | **3,659** | brace-matched parse |
| 10 | **APIs** | **60** | 18 ASMX methods (exposed on 2 endpoints) + 40 page `[WebMethod]`s + 2 `.ashx` handlers |
| 11 | **Reports** | **166 `.rpt` files / 146 distinct names**, rendered by **100** viewer + HTML-report pages | `Solution.Web/Reports` (124) and `Library.CrystalReports/SInventory_RPT` (42) |
| 12 | **Services (BLL classes)** | **76** in 77 files, **923** methods | `Library.BLL` |
| 13 | **Repositories (DAL classes)** | **144** in 145 files, **1,554** methods | `Library.DAL` |
| 14 | **DTOs / ViewModels / entities** | **119** classes in 99 files | `Library.DAO` — plain property bags; there is no DTO or ViewModel layer |
| 15 | **Helpers / infrastructure** | **6** | `ClsCommonInternalDAL`, `DB_Manager`, `DB_Connection`/`DB_Authentication`/`DB_Names`, `ClsPrimaryKeyFind`, `ClsApprovalAction`, `clsNum2Word` |
| 16 | **Middleware / filters / background jobs** | **0** | verified absent: no HTTP module, no handler beyond the two `.ashx` + Crystal's, no scheduler, no DI container; all five `Global.asax` event bodies are empty |
| 17 | **Stored procedures** | **119** | called by name from C#; **0 bodies available** |
| 18 | **Tables** | **126** | reconstructed from SQL strings |
| 19 | **Views** | **6** | `View_CentralStoreCurrentStock`, `View_CustomerMaster`, `View_DCStoreCurrentStock`, `View_InvoiceWiseDue`, `View_MIAWiseSalesReport`, `View_ProDCStock` |
| 20 | **Functions** | **0 found** | no `dbo.fn*` / `udf*` call appears in the source; whether any exist is UNKNOWN |
| 21 | **Triggers** | **UNKNOWN** | invisible from application code; requires the schema |
| 22 | **Foreign keys / indexes / constraints** | **UNKNOWN** | same |
| 23 | **Integrations** | **4 databases** | `ZAS_ACCDB`, `CSTL_ACCDB`, `CSTL_ERPDB`, `ZASHRIS` — all database-to-database, 30 three-part-name references |
| 24 | **Configuration** | 3 connection strings + 3 Crystal app settings | `Web.config`; plus hard-coded credentials in `DB_Authentication.cs` |
| 25 | **Authentication** | 1 mechanism | custom, session-based, plain-text password |
| 26 | **Authorisation** | 4 levels | menu grant, company unit, approval action, one-off user flags |

## 2. Specification produced

```
specs/                              415 markdown files
├── README.md                       entry point
├── FINAL-REPORT.md                 this document
├── SPEC-COVERAGE.md                per-page, per-area completeness (383 rows)
├── SPEC-GAPS.md                    36 gaps and findings with IDs
├── INVENTORY-PAGES.md              all 383 pages + the 127 with no auth guard
├── 00-system/                      12 system documents + 18 page specs + 1 workflow
├── master-data/                    README + 75 page specs
├── sales-order-invoice/            README + 27 page specs + 1 workflow
├── inventory-warehouse/            README + 41 page specs
├── delivery-logistics/             README + 19 page specs + 1 workflow
├── finance-collection/             README + 27 page specs + 1 workflow
├── returns/                        README + 5 page specs
├── pricing-discount-campaign/      README + 15 page specs
├── dashboard-analytics/            README + 8 page specs
├── reports/                        README + REPORT-INVENTORY.md + 146 page specs
└── integrations/                   README + 2 page specs
```

Every one of the **383** page specs carries all **31** required sections — verified programmatically. All
internal links resolve — verified programmatically. No duplicate page IDs.

## 3. Completeness — honestly

| | Pages |
|---|---|
| Completed specs (every area COMPLETE) | **0** |
| Partial specs | **383** |
| Missing specs | **0** |

**No page is 100% specified, and no page can be**, because four areas depend on artefacts that are not in this
repository. Per dimension:

| Dimension | COMPLETE | PARTIAL | MISSING | UNKNOWN |
|---|---|---|---|---|
| UI controls | 379 | 0 | 4 | 0 |
| Frontend behaviour | 379 | 4 | 0 | 0 |
| Backend flow | 383 | 0 | 0 | 0 |
| API surface | 383 | 0 | 0 | 0 |
| Database mapping | 0 | 329 | 54 | 0 |
| Business rules | 0 | 307 | 76 | 0 |
| Permissions | 0 | 0 | 0 | 383 |
| Status lifecycle | 0 | 383 | 0 | 0 |
| Test cases | 0 | 383 | 0 | 0 |

The 4 `MISSING` UI rows are pages that declare no identified server control. The 54 `MISSING` DB rows are pages
with no resolvable data access (mostly thin report hosts). The 76 `MISSING` rules rows are pages that carry no
message-bearing rule of their own.

## 4. Spec vs code vs database conflicts

**Spec/code conflicts: none possible** — there was no prior spec. Every row of every section-29 table reads
`MISSING → now documented`.

**Spec/database conflicts: none assertable** — there is no database artefact to compare against. Every
database claim in this specification is derived from application code and is labelled as such.

**Code-internal conflicts found: 20**, recorded as `GAP-APP-*` findings. The material ones:

| ID | Conflict |
|---|---|
| `GAP-APP-002` | The discount the user sees is computed per *unit*; the discount stored is computed per *line*. They differ whenever quantity ≠ 1. |
| `GAP-APP-003` | `DiscountofProduct` takes a date and never uses it; quantity banding present in the superseded version is gone. |
| `GAP-APP-016` | `UpdateInvoicePaymentAmount(amount, status, id)` passes only `@InvoiceId` — the screen's Full/Partial decision is dead code. |
| `GAP-APP-017` | One receipt across N invoices writes N `tblCustomerPay` masters, each stamped with the full receipt amount. |
| `GAP-APP-018` | Delivery confirmation shows the same success message on the failure path, and nothing at all when both calls fail. |
| `GAP-APP-020` | `InvoiceCreationForCustomer.Validation()` tests `payTypeDDL.Text`, which can never be empty — the check is inert, and the save then throws. |

## 5. What is still missing — the exhaustive list

### Blocking (needs an artefact from outside the repository)

1. **119 stored-procedure bodies.** Every accounting posting, the delivery pipeline, stock posting, weighted
   average, payment posting, batch locking, HR sync, MIGO stock-in, all bin cards. → `GAP-DB-001`
2. **The database schema.** Columns, types, nullability, defaults, primary keys, foreign keys, indexes, check
   constraints, triggers, for 126 tables and 6 views. → `GAP-DB-002`
3. **The menu tree** (`tblMainMenu`). Without it: no menu path in any spec, and module boundaries stay
   inferred. → `GAP-SEC-001`, `GAP-STR-001`
4. **Permission grants** (`tblMenuDistribution`, `tblUserCompanyUnit`, `tblActionUserWiseApproval`,
   `tblActionPageWiseStep`, `tblActionSteps`, `tblActionStepDetail`). Without them, section 20 of all 383 specs
   stays `UNKNOWN`. → `GAP-SEC-001`
5. **`tblAction` rows.** The approval status vocabulary *is* this table's `ActionText` column, so the full set
   of status values is unknown. → `GAP-SEC-001`
6. **Crystal report internals** for 166 `.rpt` files — field layout, formulas, grouping, sorting, suppression,
   embedded SQL. Binary format. → `GAP-RPT-001`
7. **Business intent.** Section 3 of every page spec is `UNKNOWN`; only the business can supply what each
   screen is *meant* to achieve. → `GAP-DOC-001`

### Needs a decision from the business

8. Which of the duplicated screens are live: `PaymentTranscation` / `New` / `New2`, `SalesReport` /
   `SalesReports`, `SalesHubReport` / `SalesHubReport2`, and the five `InvoiceCreation*` variants.
   → `GAP-STR-003`
9. Whether the 9 unreferenced `CommonUI` pages are live. → `GAP-STR-002`
10. Whether the **48** unreferenced `.rpt` definitions are dead. → `GAP-RPT-001`
11. Whether the commented-out chart-of-accounts and financial-year guards were disabled deliberately.
12. Whether the hard-coded warehouse id `15` is intentional. → `GAP-APP-015`
13. Whether one-receipt-per-invoice masters (`GAP-APP-017`) is the intended data model.

### Deliberately out of scope of this specification

14. Per-control run-time visibility/enabled conditions set in code — would require symbolic execution.
    → `GAP-UI-002`
15. Per-page CSS attribution across 211 stylesheets (almost all theme). → `GAP-UI-003`
16. File-by-file trace of the 671 `.js` files — 18 of the 21 project scripts are third-party theme code and
    none carries business logic. → `GAP-JS-001`

## 6. Security findings

Reverse engineering surfaced nine confirmed security defects. They are documented so a maintainer cannot miss
them; nothing was changed.

| ID | Severity | Finding |
|---|---|---|
| `GAP-SEC-003` | critical | Live server, `sa` login and plaintext password hard-coded in `DB_Authentication.cs`; four more `sa` passwords in `Web.config`. **In version control — treat as compromised.** |
| `GAP-SEC-005` | critical | Systemic SQL injection. ~1,400 concatenated queries executed via `EXEC ExecuteAllSqlQueryByStoreProcedure @Query`, **including the login query**. |
| `GAP-SEC-007` | critical | Passwords stored and compared in plain text. |
| `GAP-SEC-002` | critical | 127 of 383 pages have no authentication guard. |
| `GAP-SEC-004` | high | 18 unauthenticated ASMX methods leak the customer, product, price and employee masters. |
| `GAP-SEC-008` | high | `debug="true"`, no `<customErrors>`, 16-day session timeout — stack traces with SQL are shown to users. |
| `GAP-SEC-009` | high | No horizontal access control; ids in URL/Session are not ownership-checked, and approval grants are not re-checked on postback. |
| `GAP-SEC-006` | medium | `tblMainMenu.URL` written into `href=` unquoted — stored XSS via the menu table. |
| `GAP-SEC-001` | high | Hiding a menu item does not protect the page; the menu **is** the permission model. |

## 7. Recommended order of work

| # | Action | Effect |
|---|---|---|
| 1 | Rotate every credential in `DB_Authentication.cs` and `Web.config`; purge them from history | closes `GAP-SEC-003` |
| 2 | Supply `sp_helptext` for all 119 procedures | closes `GAP-DB-001`; unlocks sections 16, 19 and 21 of all 383 specs |
| 3 | Supply the schema script | closes `GAP-DB-002`; unlocks column-level field mapping |
| 4 | Supply the eight menu/permission `SELECT`s | closes `GAP-SEC-001` and `GAP-STR-001`; unlocks section 20 of all 383 specs and re-derives the module tree from real data |
| 5 | Confirm which duplicated pages and reports are live | closes `GAP-STR-002`, `GAP-STR-003`, part of `GAP-RPT-001` |
| 6 | Export the Crystal report definitions | closes `GAP-RPT-001` |
| 7 | Business review of the INFERRED objectives | closes `GAP-DOC-001` |

Items 2–4 are the high-leverage ones: three data exports would convert the majority of the remaining
`UNKNOWN`s in this specification into documented fact.

## 8. Statement of accuracy

- Every count in this report was produced by parsing the source, not by estimation.
- Every code claim cites a file and a method, and can be checked.
- Nothing was invented. Where the repository does not answer a question, the specification says `UNKNOWN` and
  names the artefact that would answer it.
- Inferences are labelled `INFERRED` and are never presented as confirmed rules.
- **No application code, business logic, database object or architecture was changed.** The only files created
  are under `specs/`.
