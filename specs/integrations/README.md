# Module: integrations

The two screens whose whole purpose is to move data between databases: HR employee synchronisation from `ZASHRIS`, and MIGO goods-receipt reporting/import from `CSTL_ERPDB`.

The full integration picture — including the accounting postings that are triggered from screens in other modules — is in [../00-system/integrations.md](../00-system/integrations.md). Every integration in this application is database-to-database; there is no HTTP client, no queue, no e-mail and no scheduled job anywhere in the codebase.

## Pages (2)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `INT-001` | `HREmployeeSysnc.aspx` | `/SInventory_UI/HREmployeeSysnc.aspx` | Entry / transaction | 24 | 1 | 13 | yes | [spec](pages/PAGE-INT-001-HREmployeeSysnc.md) |
| `INT-002` | `MIGOReport.aspx` | `/SInventory_UI/MIGOReport.aspx` | Report parameters | 13 | 0 | 3 | yes | [spec](pages/PAGE-INT-002-MIGOReport.md) |

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `tblCompanyInfo` | 1 |
| `tblCompanyUnit` | 1 |
| `tblGroupInformation` | 1 |
| `tblUserCompanyUnit` | 1 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_Employee_EmployeeCompanySyncedWithDistibution` | 1 |
| `sp_Employee_EmployeeListFromHR` | 1 |
| `sp_Employee_EmployeeListSyncedWithDistibution` | 1 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
