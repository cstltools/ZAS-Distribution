# Accounts Integration Verification — Zas_Sales_Roll_Journal_Entries_By_Page_Full.xlsx

Date: 2026-10-20
Input: `D:\Problems_Sceenshort\Zas_Sales_Roll_Journal_Entries_By_Page_Full.xlsx` (3 sheets: Journal Entries, Summary, Other Integrations)
Verified against: SP bodies in `docs/database/stored-procedures-part1..3.md` (dumped from live SalesrollDB_ZAS) and C# call sites in `Library.DAL` / `Solution.Web`.
Not done: live DB query (permission denied in this session). Mapping-table contents (`Table_Integration_AccountsConnectivity`) therefore unverified; fallback GLs are taken from SP bodies.

## Verdict on the Excel

The Excel is accurate. Every voucher shape (Dr/Cr lines, reference, subsidiary, TableID, guard conditions) matches the SP bodies. The reviewer's six comments in column G are all legitimate; four are real defects, two are design questions.

## Reviewer comments, verified

| # | Excel row | Reviewer comment | Finding | Severity |
|---|-----------|------------------|---------|----------|
| 1 | Delivery Confirmation Full | Sales Journal Missing | **CONFIRMED DEFECT.** `sp_Sales_ExecuteDeliveryPipeline` only runs `sp_Process_WeightedAverageSalesConfirmation` + `sp_Sales_DeliveryCOGSPosting`. No Dr AR / Cr Sales voucher (TableID 2014) is posted on the full-delivery path. `InvoiceDAL.cs:1767` has the `sp_Sales_DeliverySalesPosting` call commented out. The partial path (`sp_Sales_DeliverySalesPosting`) posts Sales **and** calls COGS internally, so partial is complete and full is not. Only the bulk catch-up job (part2 line 6633) backfills TableID 2014 for Full invoices, and nothing in the app calls it. | **P1** — AR and Sales understated for every full delivery since the pipeline went live |
| 2 | Customer Payment Reversal | not under stand | **CONFIRMED DEFECT.** Subsidiary is on the wrong side. Forward posting (`sp_Sales_CustomerPaymentPosting`) puts `SubsidoryLedgerId` on the Cr AR line. Reversal (`sp_Sales_CustomerPaymentReversePosting`) writes Dr AR with `SubsidoryLedgerId = 0` and puts the customer subsidiary on the Cr Cash/Bank, Discount and AIT lines. Customer sub-ledger never gets the reversal; cash/discount/AIT accounts get a customer tag they should not have. | **P1** — customer ledger wrong after any receipt reject |
| 3 | Product Destroy | Gl Head wrong | **CONFIRMED.** `sp_Sales_ProductDestroyPosting` looks up PageId 144 / FlagId 1022; on miss falls back to GL 4164. GL 4164 is the same fallback `sp_Sales_SampleCOGSPosting` uses for Sample Expense, and `sp_SampleInvoicePosting` maps FlagId 1022 as "Sample Expense". So destroy write-offs land in Sample Expense unless the 144/1022 mapping row exists. Fix is data: insert the mapping row pointing to a Destroy / Inventory Write-off GL. | **P2** — misclassified expense, trial balance still balances |
| 4 | Trade Material Transfer, To Company | Not Cleare why RM? | **CONFIRMED BY DESIGN.** `sp_Process_TradeMaterialTransferFullProcess` resolves the To-company Dr GL from `Table_Integration_MappingByLinkAccounts` by item category (LinkAccountId 1 = raw material), fallback 4159. The receiving company is treated as consuming the goods as raw material (manufacturing side). If the receiver is also a trading company this is the wrong head and should map to FG. Needs a business decision, not a code bug. | **P3** — question for accounts |
| 5 | Proforma Save / Full Return / Freeze | GL not under stand | **LEGACY, NOT ACTIVE.** All read `CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew` and are gated `CompanyId = 16`; that database is not on the current server. GL 514/515/501/16 are the old chart. These paths cannot post anything today. Recommend removing the calls or leaving as dead code; no accounting impact. | Info |
| 6 | Proforma Return | Not under stand why same journal | **CORRECT.** `sp_ProformaReturnPosting` takes the 'ProformaInvoice' Dr GL and posts it as **Credit**, and the Cr GL as **Debit**: it is the exact reversal of `sp_ProformaPosting`. Same two accounts, swapped sides. Same legacy caveat as #5. | Info |

## Additional findings not flagged in the Excel

| # | Where | Finding | Severity |
|---|-------|---------|----------|
| 7 | Delivery Sales Return (`sp_Sales_DeliverySalesReturnPosting`) | Customer subsidiary is on the **Dr Sales Return** line, not on the Cr AR line. Every other proc tags AR. The customer's sub-ledger will not show the return credit. Excel row documents it this way, which is correct as-is, but it is inconsistent with payment posting. | P2 |
| 8 | Trade Material Transfer | Posting date, financial year and voucher serial all use `GETDATE()`; voucher serial is `MAX(...) + 1 / + 2` with no lock. Two approvals in the same second can collide on VoucherCode. | P2 |
| 9 | Full delivery pages | `DelivaryInvoiceCreationAuto.aspx.cs:100` ignores a posting failure when `SaveFullInvoice` returned 1 and still shows "Save Successfully". `InvoiceCreationForConsumerCustomer.aspx.cs:507` correctly rolls back. | P2 |
| 10 | Catch-up jobs | Four backfill SPs exist (payment, delivery sales, sales return, material conversion), all scoped `>= 2025-04-25`, none called by the app. Their existence confirms postings have been missed in production before. | Info |

## Recommended order of fixes

1. Full delivery sales voucher (#1): add `EXEC dbo.sp_Sales_DeliverySalesPosting @InvoiceId` to the pipeline **instead of** the direct COGS call (SalesPosting already calls COGS and has the duplicate guard on TableID 2014). Then run the TableID 2014 catch-up once for Full invoices.
2. Payment reversal subsidiary (#2): move `@SubsidiaryInfoId` to the Dr AR line, set 0 on the other three lines. Repair existing reversal vouchers with an UPDATE on `tblDebitCreditVoucherDetail` by VoucherMasterId.
3. Destroy GL (#3): insert `Table_Integration_AccountsConnectivity` row PageId 144 / FlagId 1022 / IsInventory 1 pointing to the correct write-off GL. Reclassify posted destroy vouchers that hit 4164.
4. Decide #4 and #7 with accounts; then fix in SP.
5. #8, #9 as hardening.

## Things I could not verify

- Actual rows in `Table_Integration_AccountsConnectivity` (which GL each PageId/FlagId maps to, whether 144/1022 really is missing). Needs a DB read.
- Whether any Full-delivery invoice currently has a TableID 2014 voucher (would prove or disprove that the catch-up was run manually).
