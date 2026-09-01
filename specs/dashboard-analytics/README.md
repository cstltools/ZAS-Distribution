# Module: dashboard-analytics

Management screens: the operational dashboard, the ZAS monitoring panel, the accounts panel, cash flow, income statement and total summary.

Architecturally these are the odd ones out. They are the only screens in the application that behave like a modern web page: they expose page-level `[WebMethod]` endpoints (40 of them across `Dashboard.aspx`, `CashFlow.aspx` and `IncomeStatement.aspx`) and fetch JSON with client-side JavaScript instead of posting back. They are also the only heavy users of the `sp_DSB_*` and `sp_GET_*` reporting procedures.

Access to the dashboard is gated by its own rule — `LoginName = 'admin'` or `tblUser.IsDashboard = 1` — not by the menu.

## Pages (8)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `DSB-001` | `Dashboard.aspx` | `/Dashboard/Dashboard.aspx` | Entry / transaction | 4 | 0 | 4 | yes | [spec](pages/PAGE-DSB-001-Dashboard.md) |
| `DSB-002` | `AccountsPanel.aspx` | `/SInventory_UI/AccountsPanel.aspx` | Entry / transaction | 0 | 0 | 1 | **no** | [spec](pages/PAGE-DSB-002-AccountsPanel.md) |
| `DSB-003` | `CashFlow.aspx` | `/SInventory_UI/CashFlow.aspx` | Entry / transaction | 5 | 0 | 11 | **no** | [spec](pages/PAGE-DSB-003-CashFlow.md) |
| `DSB-004` | `Dashboard.aspx` | `/SInventory_UI/Dashboard.aspx` | Entry / transaction | 34 | 0 | 19 | **no** | [spec](pages/PAGE-DSB-004-Dashboard.md) |
| `DSB-005` | `DashboardPanel.aspx` | `/SInventory_UI/DashboardPanel.aspx` | Entry / transaction | 0 | 0 | 1 | **no** | [spec](pages/PAGE-DSB-005-DashboardPanel.md) |
| `DSB-006` | `IncomeStatement.aspx` | `/SInventory_UI/IncomeStatement.aspx` | Report parameters | 5 | 0 | 12 | **no** | [spec](pages/PAGE-DSB-006-IncomeStatement.md) |
| `DSB-007` | `TotalSummary.aspx` | `/SInventory_UI/TotalSummary.aspx` | Entry / transaction | 23 | 1 | 5 | yes | [spec](pages/PAGE-DSB-007-TotalSummary.md) |
| `DSB-008` | `ZASMonitoringPanel.aspx` | `/SInventory_UI/ZASMonitoringPanel.aspx` | Entry / transaction | 38 | 2 | 3 | yes | [spec](pages/PAGE-DSB-008-ZASMonitoringPanel.md) |

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CustomerMaster` | 3 |
| `tblArea` | 2 |
| `tblChalanDetail` | 1 |
| `tblCompanyInfo` | 22 |
| `tblCompanyUnit` | 25 |
| `tblCustMaster` | 1 |
| `tblCustPayDetail` | 8 |
| `tblCustomerPay` | 8 |
| `tblDCStore` | 2 |
| `tblDCStoreFreeze` | 1 |
| `tblDestroyProduct` | 1 |
| `tblDirectStockOut` | 1 |
| `tblEmpGeneralInfo` | 3 |
| `tblInvoice` | 24 |
| `tblInvoiceDetail` | 19 |
| `tblMIATargetProductWise` | 1 |
| `tblMIOInfo` | 3 |
| `tblMarket` | 1 |
| `tblOrder` | 1 |
| `tblProduct` | 2 |
| `tblRSMInfo` | 1 |
| `tblRegion` | 2 |
| `tblSampleIssue` | 1 |
| `tblSampleIssueDetail` | 1 |
| `tblSampleIssueTranscation` | 1 |
| `tblTerritory` | 2 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_DSB_AglingReport` | 3 |
| `sp_DSB_BusinessSummery` | 1 |
| `sp_Dashboard_Top50Customer` | 1 |
| `sp_GET_CashFlowSummaryBDTCurrency` | 2 |
| `sp_GET_CompanyInfo` | 3 |
| `sp_GET_DashboardCardInfo` | 3 |
| `sp_GET_FinancialYear` | 1 |
| `sp_GET_FinancialYearDate` | 2 |
| `sp_GET_FinancialYearWithId` | 2 |
| `sp_GET_IncomeStatementBalance` | 1 |
| `sp_GET_IncomeStatementMapDetailById` | 1 |
| `sp_GET_IncomeStatementMapMaster` | 1 |
| `sp_NationalBinCardPrice` | 1 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
