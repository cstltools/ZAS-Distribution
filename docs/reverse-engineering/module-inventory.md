# Module Inventory

**Module boundaries are INFERRED, not real.** The application has no module concept in code: 272 of its 383
pages sit in one flat folder (`Solution.Web/SInventory_UI`). The 11 modules below were grouped by page name, by
the DAL class each page uses, and by the tables it touches — a stand-in for the real grouping, which lives in
`tblMainMenu.ModuleId`/`ParantId`, a data table not present in this repository. See
[`specs/SPEC-GAPS.md`](../../specs/SPEC-GAPS.md) → `GAP-STR-001`. Once the menu table is supplied, these 11
folders should be re-derived from it, not trusted as-is.

Roles/permissions are marked **UNKNOWN** for every module below because permission is per-user *data*
(`tblMenuDistribution`, `tblUserCompanyUnit`, `tblActionUserWiseApproval`, …), not something expressed in code.
See `GAP-SEC-001`.

| Module ID | Module Name | Pages | Purpose |
|---|---|---|---|
| `00-system` | System | 18 | Login, users, menu permission, approval configuration, DB backup |
| `master-data` | Master Data | 75 | Company/unit, geography, product, price, customer, employee, sales hierarchy |
| `sales-order-invoice` | Sales Order & Invoice | 27 | Manual orders, requisitions, proforma and delivery invoices, sample issue |
| `inventory-warehouse` | Inventory & Warehouse | 41 | Warehouse and DC stock in/out, transfers, freeze, destroy, conversion |
| `delivery-logistics` | Delivery & Logistics | 19 | Picking, top sheets, challans, delivery confirmation, B2B transfer |
| `finance-collection` | Finance & Collection | 27 | Customer payments, batches, deposits, petty cash, ledgers, credit limit |
| `returns` | Returns | 5 | Customer and delivery returns, return approval |
| `pricing-discount-campaign` | Pricing, Discount & Campaign | 15 | Campaigns, bonus campaigns, discounts, quoted prices |
| `dashboard-analytics` | Dashboard & Analytics | 8 | Dashboards, cash flow, income statement, monitoring panels |
| `reports` | Reports | 146 | Report parameter screens, Crystal viewers, HTML reports |
| `integrations` | Integrations | 2 | HR employee sync, MIGO/Excel goods receipt |

---

## `00-system`

- **Purpose:** Owns the security model — login, user administration, menu permission, approval-workflow
  configuration, database backup. `UserId = 1` / `LoginName = 'admin'` are hard-coded superuser identities.
  (Source: [`specs/00-system/README.md`](../../specs/00-system/README.md))
- **Pages:** 18 — [full list](../../specs/00-system/README.md#pages)
- **Workflows:** [WF-approval.md](../../specs/00-system/workflows/WF-approval.md)
- **APIs touched:** none of its own; it defines the authentication/session model every other module's pages rely on.
- **Database objects (rough families):** `tblUser`, `tblMainMenu`, `tblMenuDistribution`, `tblAction*` (approval
  vocabulary and steps), `tblUserCompanyUnit`, `tblLoginLog`.
- **Reports:** 0 dedicated report pages.
- **Integrations:** none directly.
- **Roles:** UNKNOWN (`GAP-SEC-001`) — permission is per-user data in `tblMenuDistribution`, not role-based.
- **Status:** see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md) for `ActionStatus`/approval states used across the whole application.

## `master-data`

- **Purpose:** Reference data for everything else — company/company unit, the geography chain
  Region → Area → Territory → Market, the sales-force hierarchy (RSM → ASM → MIO → MIA), products (category,
  type, brand, pack size, case, UOM), unit prices with approval, customers with category and credit limit, and
  employees. (Source: [`specs/master-data/README.md`](../../specs/master-data/README.md))
- **Pages:** 75 — [full list](../../specs/master-data/README.md#pages-75)
- **Workflows:** none dedicated (Entry/View/Edit pattern documented in [conventions.md](../../specs/00-system/conventions.md) instead).
- **APIs touched:** consumed by the ASMX autocomplete service (`SInventoryWebService.asmx`) for product/customer lookups used elsewhere.
- **Database objects:** `tblCompanyInfo`, `tblCompanyUnit`, `tblRegion`/`tblArea`/`tblDistrict`/`tblMarket`, `tblASMInfo`/`tblEmpGeneralInfo`, `tblProduct`/`tblProCategory`/`tblProType`/`tblProductSQ`, `tblCustMaster`/`tblCustCategory`/`tblCustomerCreditLimit`.
- **Reports:** 0 dedicated report pages (feeds parameters into reports elsewhere).
- **Integrations:** none directly.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** soft delete via `IsActive`, approval via `ActionStatus` — see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md).

## `sales-order-invoice`

- **Purpose:** Order capture through to proforma invoice, plus sample and non-transactional invoices. The
  central behaviour of the application — FEFO batch allocation expanding one invoice line into several
  `tblInvoiceDetail` rows — lives here. (Source: [`specs/sales-order-invoice/README.md`](../../specs/sales-order-invoice/README.md))
- **Pages:** 27 — [full list](../../specs/sales-order-invoice/README.md#pages-27)
- **Workflows:** [WF-invoice-batch-allocation.md](../../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md)
- **APIs touched:** none new; uses the shared autocomplete ASMX for product/customer lookup on entry screens.
- **Database objects:** `tblOrder`/`tblOrderDetail`, `tblInvoice`/`tblInvoiceDetail`, `tblDCStore` (batch consumption), `tblConsumerCustomer`.
- **Reports:** 0 dedicated (report-side coverage is in `reports`).
- **Integrations:** none directly.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** invoice/order lifecycle — see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md). Known correctness defects originating here: `GAP-APP-002`, `GAP-APP-004`, `GAP-APP-005`, `GAP-APP-007` (see [`gaps.md`](gaps.md)).

## `inventory-warehouse`

- **Purpose:** Two-tier batch/expiry-tracked stock — central warehouse/central store and per-unit DC stock.
  Covers goods receipt (incl. MIGO import), issue, DC-to-DC and B2B transfer, receipt against challan,
  requisition/issue, freeze/release quarantine, destruction, and material conversion.
  (Source: [`specs/inventory-warehouse/README.md`](../../specs/inventory-warehouse/README.md))
- **Pages:** 41 — [full list](../../specs/inventory-warehouse/README.md#pages-41)
- **Workflows:** none dedicated in this folder (weighted-average costing is described in the module README and runs entirely inside `sp_Process_WeightedAverage`, body absent).
- **APIs touched:** none new.
- **Database objects:** `tblCentralStore*`, `tblDCStore`/`tblDCStoreFreeze`/`tblDCStore_OpeningBalance`, `tblWhStoreFreeze`, `tblRequisition`/`tblRequsitionChild`, `tblDestroyProduct`, `tblMIGOMaster`/`tblMIGODetail`.
- **Reports:** 0 dedicated report pages (stock reports live in `reports`).
- **Integrations:** reads `CSTL_ERPDB` for material conversion.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md); `BR-INV-005` records DC-store weighted average deliberately switched off.

## `delivery-logistics`

- **Purpose:** Getting goods from a proforma invoice to the customer — picking (warehouse and sales-centre),
  top sheets and batch top sheets, delivery challans, delivery confirmation (full/partial/reject), return
  acknowledgement notes, B2B transfer challans, a logistics rollback screen.
  (Source: [`specs/delivery-logistics/README.md`](../../specs/delivery-logistics/README.md))
- **Pages:** 19 — [full list](../../specs/delivery-logistics/README.md#pages-19)
- **Workflows:** [WF-delivery-confirmation.md](../../specs/delivery-logistics/workflows/WF-delivery-confirmation.md)
- **APIs touched:** none new.
- **Database objects:** `tblChalanInfo`/`tblChalanDetail`, `tblDCPicking`/`tblDCPickingDetail`, `tblRequsitionChild`, `tblDCStoreFreeze` (returns landing tier).
- **Reports:** report-parameter/viewer pages counted separately under `reports`, but several delivery-logistics pages themselves are report-parameter screens (see [`page-inventory.md`](page-inventory.md)).
- **Integrations:** none directly.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** delivery confirmation is the state change that turns a proforma invoice into a real sale — see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md). Known defect: `GAP-APP-018`, `GAP-APP-019`.

## `finance-collection`

- **Purpose:** Accounts receivable — customer payments against invoices, payment batches/locking, deposit
  slips, money receipts, payment reversal/correction, petty cash with approval, customer ledgers, due/outstanding
  reports, credit limits, AIT. (Source: [`specs/finance-collection/README.md`](../../specs/finance-collection/README.md))
- **Pages:** 27 — [full list](../../specs/finance-collection/README.md#pages-27)
- **Workflows:** [WF-customer-payment.md](../../specs/finance-collection/workflows/WF-customer-payment.md)
- **APIs touched:** none new.
- **Database objects:** `tblCustomerPay`/`tblCustPayDetail`, `tblPaymentTranscationMaster`/`Detail`, `tblPettyCashMaster`/`Details`, `tblCompanyWiseDeposit`, `tblCreditAdjustment`, `tblCustomerCreditLimit`.
- **Reports:** several report-parameter pages counted under `reports` operate on this module's data (batch collection, AIT, due reports).
- **Integrations:** every money movement is mirrored into the accounting database via a stored procedure (`sp_Sales_CustomerPaymentPosting`, etc.); bodies absent, so the ledger effect is UNKNOWN.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** `PaymentStatus = 'Partial'` is the only literal visible in C#; the fully-paid value is set inside a procedure — see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md). Known defects: `GAP-APP-016`, `GAP-APP-017`.

## `returns`

- **Purpose:** Returns at two stages — before delivery (proforma return) and after delivery (customer return,
  return on delivery, sales rejection). Returned goods land in the freeze/quarantine tier `tblDCStoreFreeze`
  rather than going straight back to sellable stock. (Source: [`specs/returns/README.md`](../../specs/returns/README.md))
- **Pages:** 5 — [full list](../../specs/returns/README.md#pages-5)
- **Workflows:** none dedicated (see module README for the idempotency rule `BR-INV-004`).
- **APIs touched:** none new.
- **Database objects:** `tblCustomerReturnMaster`/`Detail`, `tblDCStoreFreeze`, `tblReturnReason`.
- **Reports:** `SalesRejecionReport.aspx` is a report-parameter screen counted here.
- **Integrations:** none directly.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md).

## `pricing-discount-campaign`

- **Purpose:** Commercial policy — campaigns and bonus campaigns (buy-X-get-Y), product discounts, customer
  fixed discounts, customer-specific quoted prices, each with its own approval screen.
  (Source: [`specs/pricing-discount-campaign/README.md`](../../specs/pricing-discount-campaign/README.md))
- **Pages:** 15 — [full list](../../specs/pricing-discount-campaign/README.md#pages-15)
- **Workflows:** none dedicated (rules `BR-PRC-002`, `BR-PRC-003` documented in the module README instead).
- **APIs touched:** none new.
- **Database objects:** `tblFocMaster`/`tblFocDetails`, `tblBonusCampaignMaster`/`tblBonusCampgainDetail`, `tblProductDiscount`, `tblCustomerFixedDiscount`, `tblProductQuotedPrice`.
- **Reports:** 0 dedicated report pages.
- **Integrations:** none directly.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** approval-gated (`ActionStatus`) — see [status-lifecycle.md](../../specs/00-system/status-lifecycle.md). Known defect: `GAP-APP-003`.

## `dashboard-analytics`

- **Purpose:** Management screens — operational dashboard, ZAS monitoring panel, accounts panel, cash flow,
  income statement, total summary. The only pages in the application that behave like a modern SPA page: 40 of
  the application's 60 API endpoints ([`[WebMethod]`s](api-inventory.md)) belong to just three pages here.
  (Source: [`specs/dashboard-analytics/README.md`](../../specs/dashboard-analytics/README.md))
- **Pages:** 8 — [full list](../../specs/dashboard-analytics/README.md#pages-8)
- **Workflows:** none dedicated.
- **APIs touched:** 40 `[WebMethod]` endpoints across `Dashboard.aspx`, `CashFlow.aspx`, `IncomeStatement.aspx` — see [`api-inventory.md`](api-inventory.md).
- **Database objects:** reads via `sp_DSB_*` and `sp_GET_*` reporting procedures across most of the schema; no tables owned exclusively by this module.
- **Reports:** these pages are themselves the reporting surface for management; not counted in the 146 `reports` module pages.
- **Integrations:** none directly.
- **Roles:** access is gated by its own rule — `LoginName = 'admin'` or `tblUser.IsDashboard = 1` — not by the menu. Otherwise UNKNOWN (`GAP-SEC-001`).
- **Status:** not applicable — these are read-only analytical views.

## `reports`

- **Purpose:** The reporting surface — parameter-entry screens (`SInventory_UI`), Crystal Report viewers
  (`SInventory_RPTVIEW`), hand-built HTML reports (`HTMLReport`). Usual flow: parameter screen → `Session`/query
  string → `window.open` viewer → `.rpt` bound from a `*ReportDal`. (Source: [`specs/reports/README.md`](../../specs/reports/README.md))
- **Pages:** 146 — [full list](../../specs/reports/README.md#pages-146), report-to-`.rpt` mapping in [`specs/reports/REPORT-INVENTORY.md`](../../specs/reports/REPORT-INVENTORY.md)
- **Workflows:** none dedicated.
- **APIs touched:** none of its own.
- **Database objects:** spans nearly the whole schema; no tables owned exclusively by this module.
- **Reports:** 166 `.rpt` files / 146 distinct names, 100 viewer/HTML pages; 48 `.rpt` definitions appear unreferenced by any viewer (`GAP-RPT-001`).
- **Integrations:** none directly.
- **Roles:** UNKNOWN (`GAP-SEC-001`). **Most viewer pages do not use the master page and therefore have no authentication guard** — see `GAP-SEC-002` and [`page-inventory.md`](page-inventory.md).
- **Status:** not applicable.

## `integrations`

- **Purpose:** The two screens whose whole purpose is moving data between databases — HR employee sync from
  `ZASHRIS`, MIGO goods-receipt reporting/import from `CSTL_ERPDB`. Every integration in this application is
  database-to-database; there is no HTTP client, no queue, no e-mail, no scheduled job anywhere in the codebase.
  (Source: [`specs/integrations/README.md`](../../specs/integrations/README.md))
- **Pages:** 2 — [full list](../../specs/integrations/README.md#pages-2)
- **Workflows:** none dedicated.
- **APIs touched:** none.
- **Database objects:** `tblCustomerMasterExcelFile*` (MIGO import staging), `tblMIGOMaster`/`tblMIGODetail`, reads `ZASHRIS` and `CSTL_ERPDB` by three-part name.
- **Reports:** `MIGOReport.aspx` is a report-parameter screen counted here.
- **Integrations:** 2 of the application's 4 cross-database integrations (`ZASHRIS`, `CSTL_ERPDB`); see [`specs/00-system/integrations.md`](../../specs/00-system/integrations.md) for the full picture including postings triggered from other modules.
- **Roles:** UNKNOWN (`GAP-SEC-001`).
- **Status:** not applicable.
