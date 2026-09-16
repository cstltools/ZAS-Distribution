# Business Overview

## What the business does

SalesRoll (branded for customer **ZAS**) is a distribution / sales-and-inventory ERP covering order-to-cash for
a multi-company, multi-distribution-centre FMCG-style distributor: master data, ordering, proforma and delivery
invoicing, batch/expiry-tracked warehouse and DC stock, delivery logistics, returns, customer collections, and
roughly a hundred reports. **Confidence: CONFIRMED.** Source: [`../specs/00-system/system-overview.md`](../specs/00-system/system-overview.md).

The same codebase has been deployed for at least two other customers (commented-out connection strings for
"Faruq Cap" and `SalesDisDB_SMC` in `Web.config`), so anything customer-specific is configuration and data, not
code. **Confidence: CONFIRMED.**

## Business terminology

A glossary of domain terms as they actually appear in code, tables and page names — not invented, and not
normalized to "correct" spelling (see [coding-standard.md](coding-standard.md) on misspellings as part of the
contract).

| Term | Meaning | Confidence | Source |
|---|---|---|---|
| **DC** | Distribution Centre — a `tblCompanyUnit` row; the sales/stock centre a customer is assigned to | CONFIRMED | `../specs/00-system/system-overview.md` |
| **ComUnit / Company Unit** | Same as DC — `tblCompanyUnit`; the unit a user and most screens filter by | CONFIRMED | `../specs/00-system/role-permission.md` |
| **MIA** | Market Intelligence Assistant — sales-force role tied to a territory, `tblMIAInfo` | CONFIRMED (role exists) / INFERRED (job meaning) | `../specs/00-system/system-overview.md` |
| **MIO** | Market Intelligence Officer — sales-force role tied to a territory, `tblMIOInfo`/`tblMioInfo` | CONFIRMED / INFERRED | same |
| **ASM** | Area Sales Manager — tied to an area, `tblASMInfo` | CONFIRMED / INFERRED | same |
| **RSM** | Regional Sales Manager — tied to a region, `tblRSMInfo` | CONFIRMED / INFERRED | same |
| **Proforma invoice** | The invoice created at order time (`tblInvoice`), before delivery is confirmed; stock is already allocated (FEFO) at this point | CONFIRMED | `../specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md` |
| **Delivery (confirmation) invoice** | The event that turns a proforma invoice into a confirmed sale — Full / Partial / Reject | CONFIRMED | `../specs/delivery-logistics/workflows/WF-delivery-confirmation.md` |
| **Top sheet** | A picking/delivery-run summary document generated per batch (`TopSheetGenerate`, `BatchWiseTopSheet`) | CONFIRMED (screen exists) / INFERRED (business use) | `../specs/delivery-logistics/README.md` |
| **Challan** | A delivery/transfer document (`tblChalanInfo`/`tblChalanDetail`) — used for DC-to-DC and B2B stock movement paperwork | CONFIRMED | `../specs/delivery-logistics/README.md` |
| **FEFO** | First-Expiry-First-Out — the stock issue order used at invoicing (`BR-INV-001`), not FIFO | CONFIRMED | `../specs/00-system/business-rules.md` |
| **Freeze stock** | A quarantine tier (`tblDCStoreFreeze`/`tblWhStoreFreeze`) that returned or condition-flagged goods land in before they can re-enter sellable stock | CONFIRMED | `../specs/00-system/business-rules.md` (`BR-INV-004`) |
| **Weighted average (WAC)** | The costing method run by procedure after stock movement (`sp_Process_WeightedAverage*`) | CONFIRMED (mechanism) / UNKNOWN (procedure logic) | `../specs/00-system/system-overview.md` |
| **Bin card** | A stock ledger report per product/location (`WHBeenCardReport`, `NationalBeenCardReport`) | CONFIRMED (screens exist) | `../specs/inventory-warehouse/README.md` |
| **B2B transfer** | Business-to-business stock transfer between distribution centres, via a challan | CONFIRMED | `../specs/inventory-warehouse/README.md` |
| **MIGO** | Goods-receipt document imported from the group ERP (`CSTL_ERPDB`) | CONFIRMED | `../specs/00-system/integrations.md` |
| **Action / ActionStatus** | The approval status vocabulary — literally the `ActionText` of a selected `tblAction` row, not a fixed enum | CONFIRMED | `../specs/00-system/status-lifecycle.md` |

## Modules

Module boundaries are **INFERRED** from page names, DAL classes and tables touched — the application has no
module concept in code (see [modules.md](modules.md) and `GAP-STR-001`).

| Module | Pages | What it covers | Link |
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

## Business processes

**Order-to-cash.** Master data (company/unit, geography, product, price, customer, sales force) feeds an order
(`ManualOrderrCreation` / `OrderRequisitionCreation` → `ManualOrderApproval`), which becomes a proforma invoice
(`InvoiceCreationForCustomer(ByOrder)`), consuming stock by FEFO batch allocation and posting to accounting
(`sp_ProformaPosting`). Delivery confirmation (Full/Partial/Reject) turns the proforma into a confirmed sale and
posts the delivery pipeline. Collection applies customer payments against open invoices. See
[workflows.md](workflows.md) for the two hand-traced steps of this chain (invoice batch allocation, delivery
confirmation) and their known defects. **Confidence: CONFIRMED** for the screen sequence; **UNKNOWN** for what
each posting procedure does. Source: `../specs/00-system/system-overview.md`.

**Procurement / stock-in.** Goods receipt happens two ways: manual warehouse stock-in with approval
(`WarehouseStockIn` → `WarehouseStockInApproval` → `sp_WHStockInPosting`), or import from the group ERP's MIGO
document (`MIGOReport`/`ExcelUpForMIGO` → `sp_StockInMIGOtoCentralStore`). Stock then moves between warehouse,
central store and DC tiers by transfer, and is costed by weighted average after the fact. **Confidence:
CONFIRMED** (mechanism); **UNKNOWN** (procedure bodies). Source: `../specs/inventory-warehouse/README.md`,
`../specs/00-system/status-lifecycle.md` (Lifecycle 5).

**Delivery.** Picking and top-sheet generation prepare a batch for dispatch; delivery confirmation
(`DelivaryInvoiceCreationAuto` and variants) is the state change that turns a proforma invoice into a delivered
sale, branching Full / Partial / Reject by the display text of a status dropdown — with reject as the silent
default for anything unrecognized (`BR-DLV-001`). See
[`../specs/delivery-logistics/workflows/WF-delivery-confirmation.md`](../specs/delivery-logistics/workflows/WF-delivery-confirmation.md)
for the full trace, including `GAP-APP-018` (success/failure message collision) and `GAP-APP-019` (text-based
routing). **Confidence: CONFIRMED.**

**Returns.** Returns are handled at two stages: before delivery (`ProformaorInvoiceReturn`, posted via
`sp_ProformaReturnPosting`/`sp_ProformaFullReturnPosting`) and after delivery
(`ReturnProductCustomerByInvoice` → `ReturnustomerByInvoiceApproval` → `sp_Sales_DeliverySalesReturnPosting`).
Returned goods land in the freeze/quarantine tier, not directly back into sellable stock, and re-insertion is
idempotent per invoice-detail line (`BR-INV-004`). **Confidence: CONFIRMED.** Source: `../specs/returns/README.md`,
`../specs/00-system/status-lifecycle.md` (Lifecycle 3).

**Collections.** `CustomerPayment.aspx` and its variants apply a received amount across one or more open
invoices and post to accounting (`sp_Sales_CustomerPaymentPosting`). See
[`../specs/finance-collection/workflows/WF-customer-payment.md`](../specs/finance-collection/workflows/WF-customer-payment.md)
for the full trace, including two high-severity confirmed defects: `GAP-APP-016` (the screen's Full/Partial
decision and computed amount are discarded — the procedure decides from the invoice id alone) and `GAP-APP-017`
(one receipt across N invoices creates N full-amount payment master rows). **Confidence: CONFIRMED.**

## Roles

There is **no role model** in the technical sense — permission is a per-user, per-menu-item row in
`tblMenuDistribution`, not a role. See [security.md](security.md) and
[`../specs/00-system/role-permission.md`](../specs/00-system/role-permission.md) for the full mechanism. Two
hard-coded superuser identities are CONFIRMED:

| Identity | Grant |
|---|---|
| `UserId = 1` | Sees and may do everything — the literal integer id, not a `UserType` flag |
| `LoginName = 'admin'` (or `tblUser.IsDashboard = 1`) | Sees the Dashboard menu link |

Every other user's effective permissions are `UNKNOWN` by nature — they are rows in
`tblMenuDistribution`, `tblUserCompanyUnit` and `tblActionUserWiseApproval`, which are data not present in this
repository.

## Permissions

Four independent authorization layers exist, none of them a role:

1. **Page access (the menu)** — a row in `tblMenuDistribution` per user per menu item. Hiding a menu item does
   **not** protect the page (`GAP-SEC-001`, `BR-SEC-001`).
2. **Company unit** — `tblUserCompanyUnit` (plus two boolean flags, `CWHPermission`, `NationalReportPermission`).
3. **Approval action** — `tblActionUserWiseApproval` resolves, per user per page, the single action they may
   take; see [workflows.md](workflows.md) → WF-approval.
4. **One-off flags** — `IsCreditApproval`, `IsHead`, `IsDashboard`, `IsAppsUser` on `tblUser`.

**Confidence: CONFIRMED** (the mechanism). **UNKNOWN** (the actual grants for this installation — data, not code).
Source: `../specs/00-system/role-permission.md`.

## Status lifecycle

The approval status vocabulary is **data, not code**: approval screens write the display text of a selected
`tblAction` row straight into a status column. Five lifecycles are documented in full in
[`../specs/00-system/status-lifecycle.md`](../specs/00-system/status-lifecycle.md): generic master-data approval,
order → proforma → delivery, returns, customer payment, and warehouse/DC stock. Every "what status does a record
start in" and "what does the procedure transition to" question is `UNKNOWN` — the defaults live in table DDL and
procedure bodies not present in this repository. **Confidence: CONFIRMED** (the mechanism and the literals
observed in code); **UNKNOWN** (the complete value set and initial defaults).

## Business policies (rules) and exceptions

Every rule below reuses its `BR-*` ID from
[`../specs/00-system/business-rules.md`](../specs/00-system/business-rules.md) — no new IDs are invented here.

| Rule ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| `BR-SYS-001` | Invoice numbers are per company unit: `INV-<ComUnitCode>-<9 digits>`, derived from the max existing number, not stored | code | `Library.BLL/SInventory_BLL/InvoiceBLL.cs::InvoiceNoGenerator` | CONFIRMED |
| `BR-SYS-002` | Rows with `Remarks = 'Due'` are excluded from the invoice numbering sequence | code | `Library.DAL/SInventory_DAL/InvoiceDAL.cs::InvoiceNoCount` | CONFIRMED (exclusion) / INFERRED (purpose) |
| `BR-SYS-003` | Primary keys are `MAX(id)+1`, computed in the application outside any transaction | code | `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs` | CONFIRMED |
| `BR-SYS-004` | Multi-row document saves (header + details) are not atomic — each insert autocommits separately | code | `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs::saveButton_Click` | CONFIRMED |
| `BR-SYS-005` | `IsActive` is written/compared inconsistently (`'True'`, `'1'`, `'TRUE'`, `'False'`) | code | `Library.DAL/SInventory_DAL/*.cs` | CONFIRMED (inconsistency) / INFERRED (impact) |
| `BR-SYS-006` | Lookup values from the autocomplete service are colon-delimited composite strings, split positionally | code | `Solution.Web/App_Code/SInventoryWebService.cs` | CONFIRMED |
| `BR-SYS-007` | Approval rules are matched to pages by `LIKE '%pageName%'` | code | `Library.DAL/InternalCls/ClsApprovalAction.cs` | CONFIRMED (matching) / UNKNOWN (whether a real collision exists) |
| `BR-SYS-008` | `"Cancel"` is a magic, hard-coded action label exempt from grant checks | code | `ClsApprovalAction.LoadActionControlByUser` | CONFIRMED |
| `BR-SYS-009` | Several decisive branches compare list-item display **text**, not id | code | `DelivaryInvoiceCreationAuto.aspx.cs`, `CustomerPayment.aspx.cs` | CONFIRMED |
| `BR-DLV-001` | Delivery confirmation defaults to reject for anything not exactly `"Partial"`/`"Full"` | code | `DelivaryInvoiceCreationAuto.aspx.cs::gotoinvoiceButton_Click` | CONFIRMED |
| `BR-PRC-001` | Only an `ActionStatus='Accepted' AND IsActive='TRUE'` price is usable | code | `InvoiceDAL.ProductInfoDAL` | CONFIRMED |
| `BR-PRC-002` | A customer-specific quoted price overrides the list price (date-bounded, approval-gated) | code | `InvoiceDAL.ProductInfoDAL(comUnitId, productCode, customerMasterId)` | CONFIRMED |
| `BR-PRC-003` | Line discount % comes from a customer+product rule; the date argument is accepted but never used | code | `InvoiceBLL.ProductDiscount` / `InvoiceDAL.DiscountofProduct` | CONFIRMED |
| `BR-PRC-004` | Line arithmetic (see Important Calculations below) — discount shown ≠ discount stored | code | `InvoiceCreationForCustomer.aspx.cs` vs `InvoiceBLL.InvoiceDetailInsertBLL` | CONFIRMED |
| `BR-PRC-005` | VAT is stored per unit and multiplied out, not derived from a rate at save time | code | `tblUnitPrice.VATAmountPerUnit` usage | CONFIRMED |
| `BR-INV-001` | Stock issue is FEFO (nearest expiry first), not FIFO | code | `InvoiceDAL.BatchWiseProductQty`, `InvoiceBLL.UpdateDCStock` | CONFIRMED |
| `BR-INV-002` | No stock-availability check before allocation — a shortfall is silently written | code | `InvoiceBLL.UpdateDCStock` | CONFIRMED |
| `BR-INV-003` | Stock quantity is overwritten (not decremented) — lost updates under concurrency | code | `InvoiceDAL.UpdateDCStoreQuantity` | CONFIRMED |
| `BR-INV-004` | Returned goods enter a freeze tier idempotently per invoice-detail line | code | `InvoiceBLL.SaveDCStoreFreeze` | CONFIRMED |
| `BR-INV-005` | DC-store weighted average is deliberately not maintained ("disabled as per user request") | code comment | `InvoiceBLL.SaveDCStoreFreeze` | CONFIRMED (code state) / UNKNOWN (current business intent) |
| `BR-CUS-001` | A customer's owning sales centre is derived through an all-INNER-JOIN geography chain — any break hides the customer from invoicing | code | `InvoiceDAL.LoadCustomerMaster` | CONFIRMED |
| `BR-CUS-002` | Accounting requires a chart-of-accounts row per customer | code | `InvoiceDAL.CheckChartOfAccount` | CONFIRMED (check exists) / UNKNOWN (failure behavior, inside procedure) |
| `BR-CUS-003` | A financial year must cover the invoice date, checked against the group accounting DB | code | `InvoiceDAL.GetFinancialYearInfo` | CONFIRMED |

**Exceptions the code does not enforce** (documented because their absence is itself a rule a maintainer must
know — `BR-SEC-001` through `BR-SEC-005`): no page verifies menu grant before serving; no page verifies
company-unit ownership; passwords are not hashed; inline SQL is not parameterised; the autocomplete web service
does not check the session. See [security.md](security.md) for the full security posture.

## Integrations

Every integration is database-to-database inside SQL Server (three-part names or a second connection string) —
there is no message queue, HTTP client, file drop, scheduled job, or outbound web-service call anywhere in the
codebase. Four external databases: `ZAS_ACCDB` and `CSTL_ACCDB` (accounting), `CSTL_ERPDB` (group ERP), `ZASHRIS`
(HR). See [integrations.md](integrations.md) for the full map. **Confidence: CONFIRMED.** Source:
`../specs/00-system/integrations.md`.

## Important calculations

**Discount (per line, `BR-PRC-004`).**

```
TotalPrice        = UnitPrice × Quantity
DiscountAmount     if pct > 0 and amt > 0 → TotalPrice × pct / 100
                    if pct = 0 and amt > 0 → (DiscountAmount / lineQty) × allocatedQty
                    if both zero           → 0
NetAmount          = (TotalPrice − DiscountAmount − SpecialAmount) + TotalPriceVat
```

**CONFIRMED defect (`GAP-APP-002`):** the screen computes the discount shown to the user *per unit*
(`(UnitPrice / 100) × pct`) while `InvoiceBLL.InvoiceDetailInsertBLL` persists it *per line*
(`TotalPrice × pct / 100`). They agree only when `Quantity = 1`.

**Weighted average costing.** Run by procedure after the movement (`sp_Process_WeightedAverage`,
`…WeightedAverageB2BTransfer`, `…WeightedAverageFreezeStock`), reported into `tbl_INV_WeightedAvg`. The actual
formula is inside the procedure body and is **UNKNOWN**. DC-store weighted average is deliberately disabled
(`BR-INV-005`). Source: `../specs/00-system/system-overview.md`.

**Invoice numbering (`BR-SYS-001`, `BR-SYS-002`, `GAP-APP-011`).** `INV-<ComUnitCode>-<9 digits>`, with the
counter derived by parsing the largest existing number for the unit via `SUBSTRING`, at an offset that depends
on whether the unit id is 2 or 3 digits — a unit crossing id 99 changes the parse position for its whole
history. **Confidence: CONFIRMED.** Source: `../specs/00-system/architecture.md`.
