# Module Audit

Scope: Phase 3. See `docs/audit/system-audit.md` §4 for the module list and the caveat that module boundaries
are **INFERRED** (no `tblMainMenu`/`tblMenuDistribution` data exists in this repository — `GAP-SEC-001`,
`GAP-STR-001`). This document audits each inferred module for completeness and known risk, cross-referencing
the detailed page specs under `specs/*/README.md` rather than re-deriving them.

| Module | Pages | Core tables (INFERRED, no DDL available) | Known critical/high findings | Detail |
|---|---|---|---|---|
| System/Admin | 18 | `tblUser`, `tblMainMenu`, `tblAction*` | `GAP-SEC-002/003/007/008/009` (auth/session/plaintext password/debug config) | `docs/audit/security-audit.md` |
| Master data | 75 | `tblCustMaster`, `tblProductInfo`, `tblEmpGeneralInfo`, `tblCompanyUnit`, pricing masters | `GAP-APP-015` (hard-coded warehouse id 15); credit limit enforcement — see `docs/audit/customer-ledger-audit.md` | `docs/audit/business-rule-audit.md` |
| Sales/Order/Invoice | 27 | `tblInvoice`, `tblInvoiceDetail`, `tblManualOrder` | `GAP-APP-002` (discount shown≠stored), `GAP-APP-003` (date arg ignored), `GAP-APP-020` (dead validation), `BR-SYS-001/003/004` (invoice numbering race, non-atomic saves) | `docs/audit/sales-audit.md`, `docs/audit/transaction-integrity-audit.md` |
| Inventory/Warehouse | 41 | `tblDCStore`, `tblCentralStore`, `tblWHStockInMaster/Detail` | `GAP-APP-004` (no stock availability check), `GAP-APP-005` (lost updates on stock) | `docs/audit/inventory-audit.md` |
| Delivery/Logistics | 19 | delivery/picking/topsheet tables (names UNKNOWN without DDL) | `GAP-APP-018` (misleading success message), `GAP-APP-019` (status routing by display text) | `docs/audit/transaction-integrity-audit.md` |
| Finance/Collection | 27 | `tblCustomerPay`, deposit/payment batch tables | `GAP-APP-016` (payment amount/status discarded), `GAP-APP-017` (N payment masters overstate collections) | `docs/audit/collection-audit.md` |
| Returns | 5 | return tables (names UNKNOWN without DDL) | see return-vs-invoice quantity check | `docs/audit/return-audit.md` |
| Pricing/Discount/Campaign | 15 | discount/campaign tables | `GAP-APP-002`, `GAP-APP-003` | `docs/audit/sales-audit.md` |
| Dashboard/Analytics | 8 | aggregation queries over the above | reconciliation depends on the modules it aggregates | `docs/audit/reconciliation-audit.md` |
| Reports | 146 | read-only over all of the above | date-column correctness — see date/period audit | `docs/audit/date-period-audit.md` |
| Integrations | 2 | external DB three-part names (`ZAS_ACCDB`, `CSTL_ACCDB`, `CSTL_ERPDB`, `ZASHRIS`) | accounting/GL logic lives in these external databases, not in this repository | `docs/audit/accounting-integration-audit.md` |

## Modules present in the generic ERP checklist but absent/thin here

- **Supplier / Purchase Order (transactional)**: `SupplierInformation` exists as master data
  (`specs/master-data/pages/PAGE-MST-071/072`) but no transactional purchase-order or supplier-invoice page was
  found in `specs/` outside inter-DC transfer/receive flows. Confirmed by grep of `specs/*/README.md` module
  lists for "Purchase" — no README lists a purchase module. Marked `UNKNOWN — HUMAN VERIFICATION REQUIRED`
  whether purchasing happens in an external system (one of the 4 integrated databases) instead. See
  `docs/audit/purchase-audit.md`.
- **General Ledger / Journal / Voucher entry screens**: none found under `specs/`. Accounting is delegated to
  stored procedures named `sp_*Posting` and to the external `*_ACCDB` databases. See
  `docs/audit/accounting-integration-audit.md`.
- **Tax/VAT configuration**: VAT is an invoice-line field, not a maintained rate table in any page spec found.
  `UNKNOWN — HUMAN VERIFICATION REQUIRED`.
- **Approval** exists (`specs/00-system/pages/PAGE-SYS-002`, `PAGE-SYS-009`, `role-permission.md`) but its
  vocabulary (`tblAction.ActionText`) is data not in this repository (`GAP-SEC-001`), so the full set of
  approvable states cannot be enumerated here.

## Cross-module coupling risk

Every module above writes through the same non-transactional save pattern (`BR-SYS-004`, confirmed in
`specs/00-system/business-rules.md` and re-verified in `docs/audit/transaction-integrity-audit.md`): header,
detail, stock and payment are separate round-trips with no application-level transaction wrapping them. This
single pattern is the root cause behind several module-specific findings above (`GAP-APP-004/005/007`,
`GAP-APP-017`) rather than independent defects — see `docs/audit/root-cause-analysis.md`.
