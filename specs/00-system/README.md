# Module: 00-system

Login, user administration, menu permission, approval-workflow configuration and database backup.

This module owns the security model. Read [role-permission.md](role-permission.md) before any page here: authentication is a plain-text password comparison in a concatenated SQL string, the only authentication guard in the application lives in the master page, and permission is a per-user row in `tblMenuDistribution` rather than a role.

`UserId = 1` and `LoginName = 'admin'` are both hard-coded superuser identities.

**Module boundary is INFERRED** — see [../SPEC-GAPS.md](../SPEC-GAPS.md) → `GAP-STR-001`.

## System documents in this folder

| Document | What it covers |
|---|---|
| [system-overview.md](system-overview.md) | Identity, solution layout, runtime, functional scope, organisational model |
| [architecture.md](architecture.md) | The layer-by-layer call chain, the two SQL execution paths, transactions, numbering, error handling |
| [conventions.md](conventions.md) | Naming, page layout, the editable line-item grid, dropdowns, validation, messages, Entry/View/Edit triples |
| [navigation.md](navigation.md) | How the menu is assembled from `tblMainMenu` / `tblMenuDistribution` |
| [role-permission.md](role-permission.md) | Authentication, the four authorisation levels, the permission matrix |
| [status-lifecycle.md](status-lifecycle.md) | Status columns, values, and the five implemented lifecycles |
| [business-rules.md](business-rules.md) | Application-wide rules with IDs and confidence |
| [integrations.md](integrations.md) | Accounting, ERP, HR, Excel — and what is confirmed absent |
| [api-inventory.md](api-inventory.md) | ASMX methods, page `[WebMethod]`s, handlers |
| [database-inventory.md](database-inventory.md) | Catalogs, 127 tables, 6 views, cross-database objects, 119 procedures |
| [code-inventory.md](code-inventory.md) | Assemblies, BLL/DAL/DAO classes, infrastructure, client assets, excluded files |
| [stored-procedures.md](stored-procedures.md) | Every procedure name with the parameters C# passes |

## Pages (18)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `SYS-001` | `AdminMenuPanal.aspx` | `/CommonUI/AdminMenuPanal.aspx` | Entry / transaction | 30 | 0 | 20 | yes | [spec](pages/PAGE-SYS-001-AdminMenuPanal.md) |
| `SYS-002` | `ApprovalStepPageWise.aspx` | `/CommonUI/ApprovalStepPageWise.aspx` | Approval | 9 | 1 | 6 | yes | [spec](pages/PAGE-SYS-002-ApprovalStepPageWise.md) |
| `SYS-003` | `Default.aspx` | `/CommonUI/Default.aspx` | Entry / transaction | 11 | 1 | 4 | yes | [spec](pages/PAGE-SYS-003-Default.md) |
| `SYS-004` | `HomePage.aspx` | `/CommonUI/HomePage.aspx` | Entry / transaction | 61 | 4 | 13 | yes | [spec](pages/PAGE-SYS-004-HomePage.md) |
| `SYS-005` | `JqueryAccordian.aspx` | `/CommonUI/JqueryAccordian.aspx` | Entry / transaction | 3 | 0 | 1 | yes | [spec](pages/PAGE-SYS-005-JqueryAccordian.md) |
| `SYS-006` | `JqueryTab.aspx` | `/CommonUI/JqueryTab.aspx` | Entry / transaction | 3 | 0 | 1 | yes | [spec](pages/PAGE-SYS-006-JqueryTab.md) |
| `SYS-007` | `TemplatePage.aspx` | `/CommonUI/TemplatePage.aspx` | Entry / transaction | 3 | 0 | 1 | yes | [spec](pages/PAGE-SYS-007-TemplatePage.md) |
| `SYS-008` | `UserPermission.aspx` | `/CommonUI/UserPermission.aspx` | Entry / transaction | 22 | 2 | 10 | yes | [spec](pages/PAGE-SYS-008-UserPermission.md) |
| `SYS-009` | `UserWiseApprovalPermission.aspx` | `/CommonUI/UserWiseApprovalPermission.aspx` | Approval | 10 | 0 | 11 | yes | [spec](pages/PAGE-SYS-009-UserWiseApprovalPermission.md) |
| `SYS-010` | `Login.aspx` | `/Login.aspx` | Entry / transaction | 6 | 0 | 3 | **no** | [spec](pages/PAGE-SYS-010-Login.md) |
| `SYS-011` | `DBBackup.aspx` | `/SInventory_UI/DBBackup.aspx` | Entry / transaction | 7 | 0 | 2 | yes | [spec](pages/PAGE-SYS-011-DBBackup.md) |
| `SYS-012` | `DCUserPermission.aspx` | `/SInventory_UI/DCUserPermission.aspx` | Entry / transaction | 9 | 0 | 11 | yes | [spec](pages/PAGE-SYS-012-DCUserPermission.md) |
| `SYS-013` | `StockConditionPermission.aspx` | `/SInventory_UI/StockConditionPermission.aspx` | Entry / transaction | 7 | 0 | 10 | yes | [spec](pages/PAGE-SYS-013-StockConditionPermission.md) |
| `SYS-014` | `UserEdit.aspx` | `/SInventory_UI/UserEdit.aspx` | Edit | 15 | 0 | 6 | **no** | [spec](pages/PAGE-SYS-014-UserEdit.md) |
| `SYS-015` | `UserEntry.aspx` | `/SInventory_UI/UserEntry.aspx` | Entry / transaction | 22 | 0 | 7 | yes | [spec](pages/PAGE-SYS-015-UserEntry.md) |
| `SYS-016` | `UserMenuPermissionReport.aspx` | `/SInventory_UI/UserMenuPermissionReport.aspx` | Report parameters | 5 | 0 | 6 | yes | [spec](pages/PAGE-SYS-016-UserMenuPermissionReport.md) |
| `SYS-017` | `UserPasswordChangeReport.aspx` | `/SInventory_UI/UserPasswordChangeReport.aspx` | Report parameters | 11 | 0 | 6 | yes | [spec](pages/PAGE-SYS-017-UserPasswordChangeReport.md) |
| `SYS-018` | `UserView.aspx` | `/SInventory_UI/UserView.aspx` | List / view | 20 | 1 | 9 | yes | [spec](pages/PAGE-SYS-018-UserView.md) |

## Workflows

End-to-end traces that span several pages and layers. Read these before the individual page specs — they carry the algorithm; the page specs carry the inventory.

- [WF-004 — The generic approval workflow](workflows/WF-approval.md)

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CustomerMaster` | 1 |
| `View_InvoiceWiseDue` | 1 |
| `tblAction` | 1 |
| `tblActionPageWiseStep` | 4 |
| `tblActionSteps` | 1 |
| `tblActionUserWiseApproval` | 3 |
| `tblCompanyInfo` | 2 |
| `tblCompanyUnit` | 5 |
| `tblCustMaster` | 1 |
| `tblEmpGeneralInfo` | 2 |
| `tblInvoice` | 1 |
| `tblLoginLog` | 1 |
| `tblMainMenu` | 19 |
| `tblMarket` | 1 |
| `tblMenuDistribution` | 8 |
| `tblOrder` | 2 |
| `tblOrderDetail` | 1 |
| `tblStockCondition` | 1 |
| `tblStockConditionPermission` | 2 |
| `tblUser` | 9 |
| `tblUserCompanyUnit` | 5 |
| `tblUserType` | 1 |
| `tblWearhouse` | 1 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
