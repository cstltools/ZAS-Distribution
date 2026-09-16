# Medium and Low Severity Bugs

Scope: Phase 23, MEDIUM and LOW severity. Compact index — full narrative in the linked source document. Titles
are abbreviated from the source; do not treat this table as a substitute for reading the cited file when acting
on an item.

## MEDIUM

| ID | Title | Module | Source |
|---|---|---|---|
| `GAP-SEC-006` | Stored XSS via unquoted `tblMainMenu.URL` in `href=` | Security | `security-audit.md` |
| `BUG-SEC-002` | No systemic audit-log/change-tracking (only login log and one delete-by/date pair exist) | Security | `security-audit.md` |
| `BUG-API-002` | `GetCustomernew` concatenates `Session["MOParam"]` raw into SQL | API | `api-audit.md` |
| `BUG-API-004` | No endpoint returns a controlled 401/403; failures leak as 500 stack traces | API | `api-audit.md` |
| `BUG-PERF-001` | 448 `SELECT *` occurrences across 86 DAL files | Performance | `performance-audit.md` |
| `BUG-PERF-002` | ~1,400 ad hoc SQL strings prevent query-plan cache reuse | Performance | `performance-audit.md` |
| `BUG-PERF-004` | `CustomerPayment.aspx.cs` save loop: 2+ round trips per invoice | Performance | `performance-audit.md` |
| `BUG-PERF-005` | `PrimaryKeyMax` untransacted `MAX(id)+1` scan before every insert | Performance | `performance-audit.md` |
| `BUG-TXN-005` | No `TransactionScope`/`SqlTransaction` spans any multi-call BLL/DAL operation (system-wide restatement) | Transaction integrity | `transaction-integrity-audit.md` |
| `BUG-ACC-002` (database-audit.md) | `tblCompanyInfo`/`tblWearhouse` have no primary key | DB structure | `database-audit.md` |
| `BUG-ACC-003` (database-audit.md) | `tblDCStore` has no concurrency token / non-negative guard at schema level | DB structure | `database-audit.md` |
| `BUG-ACC-006` (database-audit.md) | 17 shared PK names make naming-convention FK inference structurally unreliable | DB structure | `database-audit.md` |
| `BUG-ACC-031` (financial-integrity-audit.md) | Discount percentage/amount rounded independently, no reconciliation | Financial | `financial-integrity-audit.md` |
| `BUG-STOCK-006` | Derived transaction copies date from parent (sales-return freeze `StockRcvDate`; transfer `ReceiveDate` from `ChalanDate`) | Inventory | `inventory-audit.md` / `date-period-audit.md` |
| `BUG-STOCK-009` | `DCStoreBLL.SaveDhakaStock` has inverted return-value logic, discards a freshly generated key | Inventory | `inventory-audit.md` |
| `BUG-STOCK-010` | Central-WH freeze branch skips GL posting that the DC branch performs | Inventory | `inventory-audit.md` |
| `BUG-STOCK-011` | Freeze-release stock guard reads a stale rendered grid cell, not a fresh DB value | Inventory | `inventory-audit.md` |
| `BUG-STOCK-012` | `DirectStockOut` quantity guard bypassable (wired only to a `TextChanged` event) | Inventory | `inventory-audit.md` |
| `BUG-STOCK-013` | `sp_FreezeStockPosting` hard-restricted to `CompanyId=16`; other companies get no GL posting | Inventory/Accounting | `inventory-audit.md` (confirmed via SP body) |
| `BUG-STOCK-016` | FEFO allocator does not exclude already-expired batches from sale | Inventory | `inventory-audit.md` |
| `BUG-STOCK-021` | No `ExpDate`/`MfgDate` cross-validation; hardcoded 2-year shelf-life default | Inventory | `date-period-audit.md` |
| `BUG-STOCK-022` | Stock-monitoring report filters the sales leg by `UpdateDate` instead of `InvoiceDate` | Reporting/Inventory | `date-period-audit.md` |
| `BUG-SALES-003` | No rounding policy; inconsistent client-side formatting | Sales | `sales-audit.md` |
| `BUG-SALES-004` | Dead, non-transactional duplicate save handler left in the file alongside the fixed one | Sales | `sales-audit.md` |
| `BUG-SALES-006` | `ReadOnly` invoice date not server-enforced (SUSPECTED) | Sales | `sales-audit.md` |
| `BUG-SALES-007` | `CalculateGrandTotal()` absolute-value bug | Sales | `sales-audit.md` |
| `BUG-SALES-008` | Consumer-customer discount hardcoded to 0 (intent UNKNOWN) | Sales | `sales-audit.md` |
| `BUG-PUR-005` | Detail-level duplicate checks are structurally dead | Purchase | `purchase-audit.md` |
| `BUG-PUR-007` | Int32/decimal quantity type mismatch crashes total calculation | Purchase | `purchase-audit.md` |
| `BUG-RET-004` | Availability checks ignore `ActionStatus` (rejected returns still reduce available quantity) | Returns | `return-audit.md` |
| `BUG-RET-005` | Balance-side effect limited to an unverified `tblCreditAdjustment` insert; downstream consumption unverified | Returns | `return-audit.md` |
| `BUG-FIN-006` | Payment-type/credit-note routing by display text or a magic literal (`"2"`) (`GAP-APP-019`) | Finance | `collection-audit.md` |
| `BUG-FIN-012` | Overpaid invoices misclassified `'Partial'` forever (rule confirmed; downstream report exposure SUSPECTED) | Finance | `customer-ledger-audit.md` |
| `BUG-FIN-013` | Reversal leaves stale `AIT`/`DiscountOnPayment` on the invoice | Finance | `customer-ledger-audit.md` |
| `GAP-APP-003` | `DiscountofProduct` ignores its date argument; quantity banding removed vs. superseded version | Sales/Pricing | `specs/SPEC-GAPS.md`, re-confirmed `sales-audit.md` |
| `GAP-APP-009` | Composite colon-delimited lookup strings break on `:` or absence | System-wide | `specs/SPEC-GAPS.md` |
| `GAP-APP-010` | Grid cell indexes are positional and unchecked | System-wide | `specs/SPEC-GAPS.md` |
| `GAP-APP-011` | Invoice-number parsing depends on unit-id width (breaks once `ComUnitId > 99`) | Sales | `specs/SPEC-GAPS.md` |
| `GAP-APP-012` | Locale-dependent date concatenation in SQL | System-wide | `specs/SPEC-GAPS.md` |
| `GAP-APP-013` | Unguarded numeric parsing (`Convert.ToDecimal` with no `TryParse`) | System-wide | `specs/SPEC-GAPS.md` |
| `GAP-APP-015` | Hard-coded warehouse id `15` collapses every warehouse to one id | Master Data | `specs/SPEC-GAPS.md` |
| `GAP-APP-020` | Ineffective validation in `InvoiceCreationForCustomer` (`payTypeDDL.Text` never empty) | Sales | `specs/SPEC-GAPS.md`, re-confirmed `sales-audit.md` |
| — (new) | `sp_Process_WeightedAverage` has a leftover full-table debug `SELECT` inside its cursor loop | Inventory/Performance | `accounting-integration-audit-followup.md` §18 |
| — (new) | `sp_Generate_InvoiceNo` builds a dynamic `UPDATE ... IN (...)` by string concatenation of `@InvoiceNo` — SQL-injection surface | Sales/Security | `accounting-integration-audit-followup.md` §23 |

## LOW

| ID | Title | Module | Source |
|---|---|---|---|
| `GAP-APP-001` | `SignatureHandler.ashx` guards the wrong session key | System | `specs/SPEC-GAPS.md` |
| `GAP-APP-014` | Two `.asmx` endpoints expose the same class (one dead but reachable) | Security | `specs/SPEC-GAPS.md` |
| `BUG-ACC-001` (database-audit.md) | `GetStockValue` declares `RETURNS int` over a decimal/bigint accumulation | DB structure | `database-audit.md` |
| `BUG-ACC-005` (database-audit.md) | `Employee` demo table name-collides with the real `EmployeeId` FK pattern | DB structure | `database-audit.md` |
| `BUG-ACC-007` (database-audit.md) | Dead shadow tables (`Table_DueInvoice`, `Table_IVF`, `Table_Temp_CustPayDetails`) duplicate live data with no PK | DB structure | `database-audit.md` |
| `BUG-STOCK-015` | DC-store weighted average is deliberately disabled (informational, re-confirms `BR-INV-005`) | Inventory | `inventory-audit.md` |
| `BUG-STOCK-023` | `ReferenceDate` stored as an unvalidated free-text string, not a date type | Inventory | `date-period-audit.md` |
| `BUG-PUR-003` | Approval screen reuses the Material-Conversion stored procedure (SUSPECTED — may be intentional reuse) | Purchase | `purchase-audit.md` |

## Structural notes carried forward (not bugs, but relevant context)

`GAP-STR-001` through `GAP-STR-006` (module boundaries inferred, dead/excluded pages, duplicated near-identical
screens, stale build output in version control, business logic sitting in the UI layer) are unchanged from
`specs/SPEC-GAPS.md` and were not re-litigated by this audit — see that file for detail.

---

See `bug-register.md` for the index and `critical-bugs.md` / `high-priority-bugs.md` for everything CRITICAL
and HIGH.
