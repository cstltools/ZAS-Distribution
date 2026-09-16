# Modules

**Module boundaries are INFERRED**, not derived from real menu data. The application has no module concept in
code — all 383 pages sit in a small set of flat folders (`SInventory_UI`, `SInventory_RPTVIEW`, `HTMLReport`,
`CommonUI`, `Dashboard`), and the real grouping lives in `tblMainMenu.ModuleId`/`ParantId`, which is data not
present in this repository. The 11 groupings below were built by grouping pages by name, by the DAL class they
use, and by the tables they touch.

- **Finding:** `GAP-STR-001` · Status: `OPEN` (needs `tblMainMenu` export to close)
- Source: [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md), [`../specs/README.md`](../specs/README.md)
- See also [navigation.md](navigation.md) for why the real menu tree can't be reconstructed from source.

## The module table

| Module | Pages | What it covers | Spec |
|---|---|---|---|
| 00-system | 18 | Login, users, menu permission, approval configuration, DB backup | [`../specs/00-system/README.md`](../specs/00-system/README.md) |
| master-data | 75 | Company/unit, geography, product, price, customer, employee, sales hierarchy | [`../specs/master-data/README.md`](../specs/master-data/README.md) |
| sales-order-invoice | 27 | Manual orders, requisitions, proforma and delivery invoices, sample issue | [`../specs/sales-order-invoice/README.md`](../specs/sales-order-invoice/README.md) |
| inventory-warehouse | 41 | Warehouse and DC stock in/out, transfers, freeze, destroy, conversion | [`../specs/inventory-warehouse/README.md`](../specs/inventory-warehouse/README.md) |
| delivery-logistics | 19 | Picking, top sheets, challans, delivery confirmation, B2B transfer | [`../specs/delivery-logistics/README.md`](../specs/delivery-logistics/README.md) |
| finance-collection | 27 | Customer payments, batches, deposits, petty cash, ledgers, credit limit | [`../specs/finance-collection/README.md`](../specs/finance-collection/README.md) |
| returns | 5 | Customer and delivery returns, return approval | [`../specs/returns/README.md`](../specs/returns/README.md) |
| pricing-discount-campaign | 15 | Campaigns, bonus campaigns, discounts, quoted prices | [`../specs/pricing-discount-campaign/README.md`](../specs/pricing-discount-campaign/README.md) |
| dashboard-analytics | 8 | Dashboards, cash flow, income statement, monitoring panels | [`../specs/dashboard-analytics/README.md`](../specs/dashboard-analytics/README.md) |
| reports | 146 | Report parameter screens, Crystal viewers, HTML reports | [`../specs/reports/README.md`](../specs/reports/README.md) |
| integrations | 2 | HR employee sync, MIGO/Excel goods receipt | [`../specs/integrations/README.md`](../specs/integrations/README.md) |

## One paragraph per module

**00-system.** Owns the security model: login, user administration, menu-permission grants, approval-workflow
step configuration, and database backup. Authentication is a plain-text password comparison in a concatenated
SQL string; the only authentication guard in the application lives in the master page; permission is a per-user
row in `tblMenuDistribution` rather than a role. `UserId = 1` and `LoginName = 'admin'` are both hard-coded
superuser identities. See [`../specs/00-system/README.md`](../specs/00-system/README.md).

**master-data.** Reference data for everything else: company/unit (sales/distribution centre), the geography
chain Region → Area → Territory → Market, the sales-force hierarchy (RSM → ASM → MIO → MIA plus slave/assistant
records and targets), products with category/type/brand/pack size/UOM, unit prices and their approval,
customers with category and credit limit, and employees. Most entities follow the Entry/View/Edit triple. See
[`../specs/master-data/README.md`](../specs/master-data/README.md).

**sales-order-invoice.** Order capture through to proforma invoice, plus sample and non-transactional invoices.
The central algorithm of the whole application lives here: an invoice line expands into several
`tblInvoiceDetail` rows by consuming stock batches in expiry order (FEFO) — see
[workflows.md](workflows.md). Several near-duplicate invoice-creation screens coexist; which are live is
decided by `tblMainMenu` and is UNKNOWN. See [`../specs/sales-order-invoice/README.md`](../specs/sales-order-invoice/README.md).

**inventory-warehouse.** Two-tier stock — central warehouse/central store, and per-unit DC stock — both batch-
and expiry-tracked. Covers goods receipt (manual and ERP MIGO import), issue, DC-to-DC/B2B transfer, freeze and
release (a quarantine tier), destruction with approval, and material conversion received from the group ERP.
Costing is weighted average, run after the movement by procedure; almost every posting here ends in a
procedure whose body is not in this repository. See [`../specs/inventory-warehouse/README.md`](../specs/inventory-warehouse/README.md).

**delivery-logistics.** Getting goods from a proforma invoice to the customer: picking, top sheets and batch top
sheets, delivery challans, delivery confirmation (Full/Partial/Reject — the state change that turns a proforma
invoice into a real sale), return acknowledgement notes, B2B transfer challans, and a logistics rollback screen.
See [`../specs/delivery-logistics/README.md`](../specs/delivery-logistics/README.md) and [workflows.md](workflows.md).

**finance-collection.** Accounts receivable: customer payments against invoices, payment batches and batch
locking, deposit slips, money receipts, payment reversal and duplicate-payment correction, petty cash with
approval, customer ledgers, due/outstanding reports, credit limits and AIT. Every money movement is mirrored
into the accounting database by a stored procedure whose body is absent, so only the SalesRoll-side effect is
documented. See [`../specs/finance-collection/README.md`](../specs/finance-collection/README.md) and
[workflows.md](workflows.md).

**returns.** Returns at two stages: before delivery (proforma return) and after delivery (customer return,
return-on-delivery, sales rejection). Returned goods land in a freeze/quarantine tier rather than straight back
into sellable stock, guarded to be idempotent per invoice-detail line. See
[`../specs/returns/README.md`](../specs/returns/README.md).

**pricing-discount-campaign.** Commercial policy: campaigns and bonus campaigns (buy-X-get-Y), product
discounts, customer fixed discounts, and customer-specific quoted prices, each with its own approval screen.
How these apply at invoicing time is documented as business rules `BR-PRC-002` and `BR-PRC-003` (the latter
carrying a confirmed defect — the discount rule's date argument is never used). See
[`../specs/pricing-discount-campaign/README.md`](../specs/pricing-discount-campaign/README.md).

**dashboard-analytics.** Management screens: the operational dashboard, ZAS monitoring panel, accounts panel,
cash flow, income statement, and total summary. Architecturally the odd ones out — the only screens that expose
page-level `[WebMethod]` endpoints and fetch JSON client-side instead of posting back. Access is gated by its
own admin/`IsDashboard` rule, not by the menu. See [`../specs/dashboard-analytics/README.md`](../specs/dashboard-analytics/README.md).

**reports.** The reporting surface: 146 pages — parameter-entry screens, Crystal Report viewers, and hand-built
HTML reports. Most viewer pages don't use the master page and therefore have no authentication guard
(`GAP-SEC-002`). Report internals (layout, formulas, grouping) are unreadable from the binary `.rpt` format
(`GAP-RPT-001`). See [`../specs/reports/README.md`](../specs/reports/README.md).

**integrations.** The two screens whose whole purpose is moving data between databases: HR employee sync from
`ZASHRIS`, and MIGO goods-receipt reporting/import from `CSTL_ERPDB`. The fuller integration picture — including
accounting postings triggered from screens in other modules — is in [integrations.md](integrations.md). See
[`../specs/integrations/README.md`](../specs/integrations/README.md).
