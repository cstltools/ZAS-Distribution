# Module: returns

Returns at two stages: before delivery (proforma return) and after delivery (customer return, return on delivery, sales rejection).

Returned goods do not go straight back into sellable stock — they land in the freeze/quarantine tier `tblDCStoreFreeze`, guarded by `InvoiceBLL.SaveDCStoreFreeze`, which inserts only when no row already exists for that invoice-detail line (`BR-INV-004`, returns are idempotent per line). Release back to sellable stock is a separate screen in the inventory module.

## Pages (5)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `RET-001` | `CustomerReturnByMIOReport.aspx` | `/SInventory_UI/CustomerReturnByMIOReport.aspx` | Report parameters | 17 | 0 | 10 | yes | [spec](pages/PAGE-RET-001-CustomerReturnByMIOReport.md) |
| `RET-002` | `ReturnProductConsCustomerByInvoice.aspx` | `/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx` | Entry / transaction | 33 | 1 | 10 | yes | [spec](pages/PAGE-RET-002-ReturnProductConsCustomerByInvoice.md) |
| `RET-003` | `ReturnProductCustomerByInvoice.aspx` | `/SInventory_UI/ReturnProductCustomerByInvoice.aspx` | Entry / transaction | 49 | 1 | 13 | yes | [spec](pages/PAGE-RET-003-ReturnProductCustomerByInvoice.md) |
| `RET-004` | `ReturnustomerByInvoiceApproval.aspx` | `/SInventory_UI/ReturnustomerByInvoiceApproval.aspx` | Approval | 16 | 1 | 8 | yes | [spec](pages/PAGE-RET-004-ReturnustomerByInvoiceApproval.md) |
| `RET-005` | `SalesRejecionReport.aspx` | `/SInventory_UI/SalesRejecionReport.aspx` | Report parameters | 13 | 0 | 8 | yes | [spec](pages/PAGE-RET-005-SalesRejecionReport.md) |

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `tblAction` | 1 |
| `tblActionPageWiseStep` | 2 |
| `tblActionStepDetail` | 2 |
| `tblActionSteps` | 2 |
| `tblActionUserWiseApproval` | 2 |
| `tblArea` | 4 |
| `tblCompanyInfo` | 6 |
| `tblCompanyUnit` | 13 |
| `tblConsumerCustomer` | 1 |
| `tblCreditAdjustment` | 1 |
| `tblCustCategory` | 1 |
| `tblCustMaster` | 5 |
| `tblCustPayDetail` | 4 |
| `tblCustomerReturnDetail` | 7 |
| `tblCustomerReturnMaster` | 7 |
| `tblDCStore` | 2 |
| `tblDCStoreFreeze` | 1 |
| `tblDcWiseAreaInfo` | 3 |
| `tblDeliveryManInfo` | 2 |
| `tblDistrict` | 1 |
| `tblEmpGeneralInfo` | 5 |
| `tblGroupInformation` | 1 |
| `tblInvoice` | 8 |
| `tblInvoiceDetail` | 8 |
| `tblMIOInfo` | 3 |
| `tblMainMenu` | 2 |
| `tblMarket` | 1 |
| `tblPaymentType` | 2 |
| `tblProduct` | 2 |
| `tblRegion` | 1 |
| `tblTerritory` | 3 |
| `tblUserCompanyUnit` | 3 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_Process_WeightedAverageFreezeStock` | 1 |
| `sp_Sales_DeliverySalesReturnPosting` | 1 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
