# Module: finance-collection

Accounts receivable: customer payments against invoices, payment batches and batch locking, deposit slips, money receipts, payment reversal and duplicate-payment correction, petty cash with approval, customer ledgers, due and outstanding reports, credit limits and AIT.

Every money movement is mirrored into the accounting database by a stored procedure (`sp_Sales_CustomerPaymentPosting`, `sp_Sales_CustomerPaymentReversePosting`, `sp_I_CompanyWiseDeposit`, `sp_MRCP_*`). Those bodies are absent, so the *ledger* effect of each screen is UNKNOWN — only the SalesRoll-side tables are documented.

`PaymentStatus = 'Partial'` is the only payment status literal that appears in C#; the fully-paid value is set inside a procedure.

## Pages (27)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `FIN-001` | `AIT Report.aspx` | `/SInventory_UI/AIT Report.aspx` | Report parameters | 26 | 1 | 12 | yes | [spec](pages/PAGE-FIN-001-AIT-Report.md) |
| `FIN-002` | `BatchWiseCollection.aspx` | `/SInventory_UI/BatchWiseCollection.aspx` | Entry / transaction | 27 | 1 | 13 | yes | [spec](pages/PAGE-FIN-002-BatchWiseCollection.md) |
| `FIN-003` | `BatchWiseCollectionReport.aspx` | `/SInventory_UI/BatchWiseCollectionReport.aspx` | Report parameters | 28 | 1 | 14 | yes | [spec](pages/PAGE-FIN-003-BatchWiseCollectionReport.md) |
| `FIN-004` | `CollectionSheet.aspx` | `/SInventory_UI/CollectionSheet.aspx` | Report parameters | 30 | 1 | 18 | yes | [spec](pages/PAGE-FIN-004-CollectionSheet.md) |
| `FIN-005` | `CompanyWiseDepositSlipReport.aspx` | `/SInventory_UI/CompanyWiseDepositSlipReport.aspx` | Report parameters | 13 | 0 | 8 | yes | [spec](pages/PAGE-FIN-005-CompanyWiseDepositSlipReport.md) |
| `FIN-006` | `CreditCustomerReport.aspx` | `/SInventory_UI/CreditCustomerReport.aspx` | Report parameters | 10 | 1 | 6 | yes | [spec](pages/PAGE-FIN-006-CreditCustomerReport.md) |
| `FIN-007` | `CustomerDueReport.aspx` | `/SInventory_UI/CustomerDueReport.aspx` | Report parameters | 33 | 1 | 19 | yes | [spec](pages/PAGE-FIN-007-CustomerDueReport.md) |
| `FIN-008` | `CustomerLedgerReport.aspx` | `/SInventory_UI/CustomerLedgerReport.aspx` | Report parameters | 5 | 0 | 3 | yes | [spec](pages/PAGE-FIN-008-CustomerLedgerReport.md) |
| `FIN-009` | `CustomerOutstandingReport.aspx` | `/SInventory_UI/CustomerOutstandingReport.aspx` | Report parameters | 16 | 0 | 7 | yes | [spec](pages/PAGE-FIN-009-CustomerOutstandingReport.md) |
| `FIN-010` | `CustomerPayment.aspx` | `/SInventory_UI/CustomerPayment.aspx` | Entry / transaction | 43 | 1 | 28 | yes | [spec](pages/PAGE-FIN-010-CustomerPayment.md) |
| `FIN-011` | `CustomerPaymentBatchReport.aspx` | `/SInventory_UI/CustomerPaymentBatchReport.aspx` | Report parameters | 28 | 1 | 11 | yes | [spec](pages/PAGE-FIN-011-CustomerPaymentBatchReport.md) |
| `FIN-012` | `CustomerPaymentList.aspx` | `/SInventory_UI/CustomerPaymentList.aspx` | List / view | 20 | 1 | 12 | yes | [spec](pages/PAGE-FIN-012-CustomerPaymentList.md) |
| `FIN-013` | `CustomerPaymentProcess.aspx` | `/SInventory_UI/CustomerPaymentProcess.aspx` | Entry / transaction | 48 | 1 | 30 | yes | [spec](pages/PAGE-FIN-013-CustomerPaymentProcess.md) |
| `FIN-014` | `CustomerPaymentReport.aspx` | `/SInventory_UI/CustomerPaymentReport.aspx` | Report parameters | 17 | 0 | 11 | yes | [spec](pages/PAGE-FIN-014-CustomerPaymentReport.md) |
| `FIN-015` | `DepositList.aspx` | `/SInventory_UI/DepositList.aspx` | List / view | 30 | 1 | 12 | yes | [spec](pages/PAGE-FIN-015-DepositList.md) |
| `FIN-016` | `DepositSlipEntry.aspx` | `/SInventory_UI/DepositSlipEntry.aspx` | Entry / transaction | 34 | 0 | 10 | yes | [spec](pages/PAGE-FIN-016-DepositSlipEntry.md) |
| `FIN-017` | `DuplicatePaymentList.aspx` | `/SInventory_UI/DuplicatePaymentList.aspx` | List / view | 16 | 1 | 5 | yes | [spec](pages/PAGE-FIN-017-DuplicatePaymentList.md) |
| `FIN-018` | `MarketWiseCustomerPayment.aspx` | `/SInventory_UI/MarketWiseCustomerPayment.aspx` | Entry / transaction | 43 | 1 | 20 | yes | [spec](pages/PAGE-FIN-018-MarketWiseCustomerPayment.md) |
| `FIN-019` | `MoneyReceiptReport.aspx` | `/SInventory_UI/MoneyReceiptReport.aspx` | Report parameters | 33 | 1 | 19 | yes | [spec](pages/PAGE-FIN-019-MoneyReceiptReport.md) |
| `FIN-020` | `PaymentReverse.aspx` | `/SInventory_UI/PaymentReverse.aspx` | Entry / transaction | 5 | 0 | 5 | yes | [spec](pages/PAGE-FIN-020-PaymentReverse.md) |
| `FIN-021` | `PaymentTranscation.aspx` | `/SInventory_UI/PaymentTranscation.aspx` | Entry / transaction | 61 | 1 | 35 | yes | [spec](pages/PAGE-FIN-021-PaymentTranscation.md) |
| `FIN-022` | `PaymentTranscationNew.aspx` | `/SInventory_UI/PaymentTranscationNew.aspx` | Entry / transaction | 65 | 1 | 38 | yes | [spec](pages/PAGE-FIN-022-PaymentTranscationNew.md) |
| `FIN-023` | `PaymentTranscationNew2.aspx` | `/SInventory_UI/PaymentTranscationNew2.aspx` | Entry / transaction | 78 | 2 | 40 | yes | [spec](pages/PAGE-FIN-023-PaymentTranscationNew2.md) |
| `FIN-024` | `PettyCashApproval.aspx` | `/SInventory_UI/PettyCashApproval.aspx` | Approval | 29 | 1 | 8 | yes | [spec](pages/PAGE-FIN-024-PettyCashApproval.md) |
| `FIN-025` | `PrettyCashEntry.aspx` | `/SInventory_UI/PrettyCashEntry.aspx` | Entry / transaction | 32 | 1 | 27 | yes | [spec](pages/PAGE-FIN-025-PrettyCashEntry.md) |
| `FIN-026` | `PrettyCashView.aspx` | `/SInventory_UI/PrettyCashView.aspx` | List / view | 19 | 1 | 10 | yes | [spec](pages/PAGE-FIN-026-PrettyCashView.md) |
| `FIN-027` | `TotalCustomerDueReport.aspx` | `/SInventory_UI/TotalCustomerDueReport.aspx` | Report parameters | 24 | 1 | 16 | yes | [spec](pages/PAGE-FIN-027-TotalCustomerDueReport.md) |

## Workflows

End-to-end traces that span several pages and layers. Read these before the individual page specs — they carry the algorithm; the page specs carry the inventory.

- [WF-003 — Customer payment application](workflows/WF-customer-payment.md)

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CustomerMaster` | 5 |
| `tblAction` | 1 |
| `tblActionPageWiseStep` | 2 |
| `tblActionStepDetail` | 2 |
| `tblActionSteps` | 2 |
| `tblActionUserWiseApproval` | 2 |
| `tblArea` | 8 |
| `tblCompanyInfo` | 48 |
| `tblCompanyUnit` | 60 |
| `tblCreditAdjustment` | 9 |
| `tblCustMaster` | 55 |
| `tblCustPayDetail` | 44 |
| `tblCustomerCreditLimit` | 2 |
| `tblCustomerPay` | 19 |
| `tblCustomerReturnMaster` | 1 |
| `tblDCStore` | 1 |
| `tblDCStoreFreeze` | 1 |
| `tblDcWiseAreaInfo` | 3 |
| `tblDeliveryManInfo` | 14 |
| `tblDestroyProduct` | 2 |
| `tblEmpGeneralInfo` | 16 |
| `tblGroupInformation` | 2 |
| `tblInvoice` | 66 |
| `tblInvoiceBatch` | 25 |
| `tblInvoiceDetail` | 25 |
| `tblMIOInfo` | 7 |
| `tblMainMenu` | 2 |
| `tblManufacturer` | 1 |
| `tblMarket` | 8 |
| `tblMoneyReceipt` | 6 |
| `tblOrder` | 3 |
| `tblOrderDetail` | 2 |
| `tblPaymentTranscationDetail` | 34 |
| `tblPaymentTranscationMaster` | 37 |
| `tblPaymentType` | 32 |
| `tblPettyCashDetails` | 2 |
| `tblPettyCashMaster` | 3 |
| `tblProductDiscount` | 1 |
| `tblRegion` | 7 |
| `tblSampleIssue` | 1 |
| `tblSlaveInfo` | 1 |
| `tblTerritory` | 8 |
| `tblUnitPrice` | 1 |
| `tblUser` | 2 |
| `tblUserCompanyUnit` | 22 |
| `tblWHStockInMaster` | 1 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_CustomerPaymentPosting` | 9 |
| `sp_DEL_DuplicatePayment` | 1 |
| `sp_DEL_PettyCashDetails` | 1 |
| `sp_DEL_PettyCashMaster` | 1 |
| `sp_GET_PretyCashDetails_ById` | 1 |
| `sp_GET_PretyCashMaster_ById` | 1 |
| `sp_I_PrettyCashDetails` | 1 |
| `sp_I_PrettyCashMaster` | 1 |
| `sp_MRCP_DeletePayment` | 2 |
| `sp_MRCP_MoneyReceiptMasterInsert` | 2 |
| `sp_PMNT_CustomerPaymentDetailInsert` | 2 |
| `sp_PMNT_CustomerPaymentMasterInsert` | 2 |
| `sp_PaymentBatchLockProcess` | 1 |
| `sp_Sales_CustomerPaymentPosting` | 9 |
| `sp_Sales_CustomerPaymentReversePosting` | 1 |
| `sp_Save_PayCode` | 1 |
| `sp_Save_PaymentBatchDetail` | 1 |
| `sp_Save_PaymentBatchMaster` | 1 |
| `sp_Save_SaveCustPayment` | 7 |
| `sp_UD_InvoicePayment` | 6 |
| `sp_UD_SampleIssue` | 1 |
| `sp_UD_prettyCashDetails` | 1 |
| `sp_UD_prettyCashMaster` | 1 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
