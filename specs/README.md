# SalesRoll (ZAS Distribution) — Specification

Reverse-engineered specification of the application in this repository. Written from the source code, not from
prior documentation — **there was none**. Before this rebuild the repository contained no `specs/`, no `docs/`,
no `.claude/` and no `database/` directory. That is a verified fact, not an assumption, and it shapes what this
specification can and cannot claim (see [SPEC-GAPS.md](SPEC-GAPS.md)).

## What this application is

A distribution / sales-and-inventory ERP for **ZAS**, branded **SalesRoll**, built as an **ASP.NET Web Forms**
site on **.NET Framework 4.0** against **SQL Server**. It covers order-to-cash for a multi-company,
multi-distribution-centre FMCG-style distributor: master data, ordering, proforma and delivery invoicing,
batch/expiry-tracked warehouse and DC stock, delivery logistics, returns, customer collections, and roughly a
hundred reports.

| | |
|---|---|
| Pages (`.aspx`) | **383** |
| Server controls declared in markup | **7,119** |
| GridViews | **191** |
| Code-behind methods | **3,659** |
| BLL + DAL + DAO methods | **2,477** |
| Distinct tables referenced in code | **126** |
| Distinct views referenced in code | **6** |
| Stored procedures called by name | **119** |
| Crystal Report definitions (`.rpt`) | **166** files / **146** distinct names (124 in `Solution.Web/Reports`, 42 in `Library.CrystalReports`) |
| Report viewer / HTML report pages | **100** |
| HTTP endpoints that are not page postbacks | **60** — 18 ASMX methods + 40 page `[WebMethod]`s + 2 `.ashx` handlers |

## How to read this specification

Start here, then go to the module you care about, then to the page.

| Document | What it answers |
|---|---|
| [00-system/system-overview.md](00-system/system-overview.md) | What the system is, its modules, its runtime |
| [00-system/architecture.md](00-system/architecture.md) | How a click reaches the database, layer by layer |
| [00-system/conventions.md](00-system/conventions.md) | The repeated patterns — read this once and every page spec becomes shorter |
| [00-system/navigation.md](00-system/navigation.md) | How the menu is built (and why the menu tree is UNKNOWN) |
| [00-system/role-permission.md](00-system/role-permission.md) | Authentication, page permission, approval permission |
| [00-system/status-lifecycle.md](00-system/status-lifecycle.md) | Every status value found, and which transitions are implemented |
| [00-system/business-rules.md](00-system/business-rules.md) | Application-wide rules with IDs and confidence levels |
| [00-system/integrations.md](00-system/integrations.md) | ERP, accounting, HR, Excel |
| [00-system/api-inventory.md](00-system/api-inventory.md) | The complete non-postback HTTP surface |
| [00-system/database-inventory.md](00-system/database-inventory.md) | Catalogs, tables, views, cross-database objects |
| [00-system/code-inventory.md](00-system/code-inventory.md) | Every class outside the pages: BLL, DAL, DAO, infrastructure |
| [00-system/stored-procedures.md](00-system/stored-procedures.md) | All 119 procedure names + the parameters C# passes |
| [reports/REPORT-INVENTORY.md](reports/REPORT-INVENTORY.md) | Every report viewer → its `.rpt`, parameters and data layer |
| [INVENTORY-PAGES.md](INVENTORY-PAGES.md) | All 383 pages in one table, with links to their specs |
| [SPEC-COVERAGE.md](SPEC-COVERAGE.md) | Per page, how completely each area is specified |
| [SPEC-GAPS.md](SPEC-GAPS.md) | Everything that is still missing, and why |
| [FINAL-REPORT.md](FINAL-REPORT.md) | The audit result: totals found, what is complete, what is not |

## Modules

Module boundaries are **INFERRED**. The application has no module concept in code — every page sits in one flat
folder, `Solution.Web/SInventory_UI`, and the real grouping lives in `tblMainMenu`, which is data and is not in
the repository. Pages were grouped by name, by the DAL class they use and by the tables they touch.

| Module | Pages | What it covers |
|---|---|---|
| [00-system](00-system/README.md) | 18 | Login, users, menu permission, approval configuration, DB backup |
| [master-data](master-data/README.md) | 75 | Company/unit, geography, product, price, customer, employee, sales hierarchy |
| [sales-order-invoice](sales-order-invoice/README.md) | 27 | Manual orders, requisitions, proforma and delivery invoices, sample issue |
| [inventory-warehouse](inventory-warehouse/README.md) | 41 | Warehouse and DC stock in/out, transfers, freeze, destroy, conversion |
| [delivery-logistics](delivery-logistics/README.md) | 19 | Picking, top sheets, challans, delivery confirmation, B2B transfer |
| [finance-collection](finance-collection/README.md) | 27 | Customer payments, batches, deposits, petty cash, ledgers, credit limit |
| [returns](returns/README.md) | 5 | Customer and delivery returns, return approval |
| [pricing-discount-campaign](pricing-discount-campaign/README.md) | 15 | Campaigns, bonus campaigns, discounts, quoted prices |
| [dashboard-analytics](dashboard-analytics/README.md) | 8 | Dashboards, cash flow, income statement, monitoring panels |
| [reports](reports/README.md) | 146 | Report parameter screens, Crystal viewers, HTML reports |
| [integrations](integrations/README.md) | 2 | HR employee sync, MIGO/Excel goods receipt |

Every page has one spec file under `<module>/pages/PAGE-<ID>-<Name>.md` following a fixed 31-section template.

## Honest statement of completeness

This specification is **complete for what the repository contains** and **explicitly incomplete for what it does
not**. Specifically:

- **UI, frontend behaviour, backend call chains, table and stored-procedure *names*: documented for all 383 pages.**
  These were extracted from the actual markup and C#, not sampled.
- **Stored-procedure bodies: absent.** 119 procedures — including every accounting posting, every stock posting
  and the whole delivery pipeline — run logic this repository does not contain. Anything they do is `UNKNOWN`.
- **Database schema: absent.** No columns, types, keys, indexes, constraints or triggers.
- **Role/permission matrix: absent.** Permission is per-user data in `tblMenuDistribution`, not code.
- **Menu tree: absent.** Same reason.

Nothing in this specification is marked COMPLETE in those four areas. See [SPEC-GAPS.md](SPEC-GAPS.md) for the
full list and for what to supply in order to close each gap.
