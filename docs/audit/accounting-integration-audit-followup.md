# Accounting Integration Audit — Follow-up (stored-procedures-part1..4.md cross-check)

Audit-only addendum. No source, schema, or data was modified. This document does not replace or edit any prior
`docs/audit/*.md` file — it is a standalone reference resolving procedures that were previously marked
"HUMAN VERIFICATION REQUIRED" because `docs/database/stored-procedures.md` lists all 119 SP bodies as
"NOT AVAILABLE". It was discovered that `docs/database/stored-procedures-part1.md` through `-part4.md` contain
real, reverse-engineered `CREATE PROCEDURE` bodies for ~515 procedures that most earlier audit agents did not
systematically consult.

Method: grepped each of the 27 procedure names flagged as blocking in prior audits (case-insensitive) across
`stored-procedures-part1.md`–`part4.md`, then read each matched body plus its accompanying
Parameters/Tables-read/Tables-written/Transaction-handling/Business-logic annotations (which are already present
in the part files, written by whoever reverse-engineered them from a live DB export).

**Result: 26 of 27 procedures found with full bodies. 1 not found (still blocked).**

---

## 1. sp_ProformaPosting

- **Found in:** `stored-procedures-part3.md:601` (also driven from a bulk-repost cursor in `stored-procedures-part1.md:5223`)
- **Summary:** Restricted to `CompanyId = 16`. Cursor over one invoice; builds voucher code `JVP-<yy><mm><dd><seq>`, looks up the invoice's financial year, inserts one header row into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster` (`VoucherType='Journal'`, `Particular='ProformaInvoice'`, `TotalAmountBDT`=stock cost), then a Debit/Credit detail pair into `tblDebitCreditVoucherDetail` using GL account IDs from `tblSalesIntegrationMapingNew` (`Particular='ProformaInvoice'`, `BalanceTyp IN ('Dr','Cr')`). Sets `tblInvoice.IsPosting = 1` if a voucher row now exists for that Proforma, else `0`.
- **Transaction/error handling:** None — no `BEGIN TRAN`/`TRY CATCH`/`RAISERROR` anywhere in the body.
- **Resolves/changes:** `accounting-integration-audit.md` and `financial-integrity-audit.md` flagged this as UNKNOWN; also relevant to `root-cause-analysis.md`'s "no rollback if posting fails mid-way" theme.
- **Confidence:** CONFIRMED — real Dr/Cr posting logic, self-consistent with sibling `sp_ProformaReturnPosting`/`sp_ProformaFullReturnPosting`. Correction to note: the procedure is **hard-restricted to `CompanyId = 16`** — for any other company this proc silently does nothing (no error), which prior audits should treat as a real gap, not a hypothetical.

## 2. sp_Sales_ExecuteDeliveryPipeline

- **Found in:** `stored-procedures-part3.md:3823`
- **Summary:** Pure 2-line orchestrator, no tables touched directly: `EXEC sp_Process_WeightedAverageSalesConfirmation @InvoiceId`; `EXEC sp_Sales_DeliveryCOGSPosting @InvoiceId`. `SET NOCOUNT ON`.
- **Transaction/error handling:** None. If the first EXEC fails or partially succeeds, the second still runs (no error check between the two calls) — this is a genuine gap.
- **Resolves/changes:** Confirms the "pipeline" is exactly these two sub-procedures, nothing more (no voucher posting happens directly in this proc — it delegates entirely). Prior audits that assumed this proc itself posts GL entries were WRONG; it posts nothing itself.
- **Confidence:** CONFIRMED (for this proc's own body). The two callees (`sp_Process_WeightedAverageSalesConfirmation`, `sp_Sales_DeliveryCOGSPosting`) were **not** in this 27-item list and were not separately verified — still HUMAN VERIFICATION REQUIRED for their internals if not covered elsewhere.

## 3. sp_Sales_DeliverySalesPosting

- **Found in:** `stored-procedures-part3.md:2725`
- **Summary:** Cursor-driven GL posting keyed off `tblInvoiceDetail` delivery totals (`DeliveryTotalPrice`/`DeliveryDiscountAmount`/`DeliveryNetAmount`). Inserts into `CSTL_ACCDB..tblDebitCreditVoucherMaster/Detail` and `CSTL_ACCDB..tblReferenceInfo`; updates `Invoice`/`INV`.
- **Transaction/error handling:** None.
- **Resolves/changes:** Note the live call site is commented out — `InvoiceDAL.cs:1674` has `//return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesPosting", ...)`. This proc is effectively **dead in the current code path** for that call site; instead the "missing postings" catch-up job (`stored-procedures-part2.md:6633`) calls it directly per invoice, so it is NOT fully dead — it runs via the bulk reposting utility.
- **Confidence:** CONFIRMED.

## 4. sp_DeliveryConfirmationFullPosting

- **Found in:** `stored-procedures-part1.md:10175`
- **Summary:** Restricted to `CompanyId = 16`. Computes net sales/discount and stock cost for `@InvoiceId`, posts a 'Delivery Sales Confirmation Full' Debit(customer GL)/Debit(discount GL if any)/Credit voucher PLUS a separate 'Delivery Confirmation Full, Inventory Valuation' Debit/Credit voucher, both into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster/Detail`; flags `tblInvoice.IsPosting`.
- **Transaction/error handling:** None.
- **Resolves/changes:** **Important correction** — the annotated caller list says this proc is referenced only from **commented-out** code (`InvoiceDAL.cs PostDeliveryConfFullJournal`, `RequisitionDAL.cs ProcessWeightedAvg`) and is "effectively UNUSED in the active application." Any prior audit finding that assumed this proc is actively firing on every full delivery confirmation is WRONG — the active path uses a different, self-contained sibling (`sp_ADJ_DeliveryConfirmationFullPosting`, referenced in the part1 annotation but not itself in this 27-item list).
- **Confidence:** CONFIRMED body; CORRECTED usage status (dead code, not live).

## 5. sp_DeliveryConfirmationPartiallPosting

- **Found in:** `stored-procedures-part1.md:10622`
- **Summary:** Same shape as #4 for partial deliveries; posts three vouchers ('Delivery Sales Confirmation Partial', its inventory valuation, and a 'Return Stock Valuation' for the undelivered remainder).
- **Transaction/error handling:** None.
- **Resolves/changes:** Same correction as #4 — caller (`InvoiceDAL.cs PostDeliveryConfPartialJournal`) is commented out; effectively UNUSED.
- **Confidence:** CONFIRMED body; CORRECTED usage status (dead code).

## 6. sp_ProformaReturnPosting

- **Found in:** `stored-procedures-part3.md:909`
- **Summary:** Same pattern as `sp_ProformaPosting` (Company 16 only, `JVP-` voucher, Dr/Cr pair via `tblSalesIntegrationMapingNew`, `Particular='ProformaInvoice'`), triggered from the return flow.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms `return-audit.md`'s general assumption that returns post a mirroring journal entry; no rollback protection exists if the insert fails partway.
- **Confidence:** CONFIRMED.

## 7. sp_ProformaFullReturnPosting

- **Found in:** `stored-procedures-part3.md:329`
- **Summary:** Same shape, but `Particular='ProformaFullReject'` instead of `'ProformaInvoice'` — a distinct GL mapping row is used for full rejections vs. partial/proforma postings.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms these are genuinely different GL mapping keys (not a copy-paste duplicate of #6) — relevant to `financial-integrity-audit.md` questions about whether full vs. partial returns hit different accounts.
- **Confidence:** CONFIRMED.

## 8. sp_Sales_DeliverySalesReturnPosting

- **Found in:** `stored-procedures-part3.md:3182`
- **Summary:** Cursor-driven, posts Debit/Credit into `CSTL_ACCDB..tblDebitCreditVoucherMaster/Detail` for approved customer returns; reads `tblCustomerReturnMaster/Detail`.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms the "missing postings" catch-up SP (`stored-procedures-part2.md:6699`) calls this per un-posted approved return (`TableID = 2018`) — i.e. there is a known-necessary backfill utility because this proc is not guaranteed to run synchronously/successfully on approval. That corroborates `reconciliation-audit.md` concerns about postings silently not happening.
- **Confidence:** CONFIRMED.

## 9. sp_UD_InvoicePayment — MAJOR CORRECTION

- **Found in:** `stored-procedures-part3.md:8421`
- **Summary:** Cursor over exactly one invoice (`@InvoiceId`). Recomputes, from source tables (not from any caller-supplied amount), `TotalPaid = SUM(tblCustPayDetail.PaymentAmount) + SUM(AIT) + SUM(Discount)` for that invoice, and `DeliveryNetAmount` = `SUM(tblInvoiceDetail.DeliveryNetAmount)`. Then does one `UPDATE tblInvoice SET PaymentAmount = @TotalPaid, AIT = @AIT, DiscountOnPayment = @Discount, PaymentStatus = CASE WHEN @TotalPaid >= @DeliveryTpGrandTotal THEN 'Full' ELSE 'Partial' END`.
- **Transaction/error handling:** None.
- **Resolves/changes:** This directly resolves `collection-audit.md` **BUG-FIN-001** (`docs/audit/collection-audit.md:56-58`), which states: *"The invoice's paid amount/status is decided entirely inside `sp_UD_InvoicePayment` from `@InvoiceId` alone. HUMAN VERIFICATION REQUIRED — needs `sp_helptext 'sp_UD_InvoicePayment'` (not present in `docs/database/`) to know what it actually does."* That sp_helptext **is** available (part3.md) and reading it shows the earlier concern was actually **less severe than implied**: the fact that `CustPaymentBLL.UpdateInvoicePaymentAmount` never passes `amount`/`status` to the DAL (GAP-APP-016) is harmless in practice, because the SP **ignores whatever amount/status the caller would have sent anyway** and always recalculates `PaymentAmount`/`AIT`/`DiscountOnPayment`/`PaymentStatus` fresh from `tblCustPayDetail`/`tblInvoiceDetail` — i.e. the proc self-heals from the correct source-of-truth tables regardless of the discarded C# parameters. GAP-APP-016 is confirmed as dead/unused parameters, but it is **not** a live data-integrity bug for `PaymentAmount`/`PaymentStatus` correctness, since the value shown is always recomputed server-side.
- **Confidence:** CONFIRMED (was UNKNOWN, now CONFIRMED and the underlying concern is downgraded).

## 10. sp_Sales_CustomerPaymentPosting

- **Found in:** `stored-procedures-part3.md:1586`
- **Summary:** Already partially used by `collection-audit.md` (correctly cited at line 29-31). Cursor filtered `WHERE CPD.InvoiceId = @InvoiceId AND CPD.CustPayId = @CustPayId`; posts Dr/Cr GL voucher into `CSTL_ACCDB..tblDebitCreditVoucherMaster/Detail` using `CPD.PaymentAmount`/`AIT`/`Discount` (the per-detail-row amount, not the `tblCustomerPay` master row amount); updates `Invoice`/`INV`, `tblPaymentTranscationMaster`.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms `collection-audit.md`'s finding that GAP-APP-017 (one receipt → N master rows) does not overstate GL postings, because this SP sources amounts from `tblCustPayDetail`, not `tblCustomerPay`. No change to that finding — this is corroboration, not a correction.
- **Confidence:** CONFIRMED (already correctly used by one prior audit; now confirmed independently).

## 11. sp_Save_SaveCustPayment

- **Found in:** `stored-procedures-part3.md:7457`
- **Summary:** Trivial: single `INSERT INTO tblCustomerPay (CustPayId, CustomerMasterId, PaymentDate, PaymentAmount, PayType, RefNo, RefDate, CreateBy, CreateDate, PaymentCode)`. No FROM/JOIN, no accounting logic at all.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms this is purely a receipt-header insert, unrelated to GL posting — consistent with `collection-audit.md`'s framing that the actual journal entry comes from `sp_Sales_CustomerPaymentPosting`, called separately.
- **Confidence:** CONFIRMED.

## 12. sp_DeleteDeliveryInvoice_FullRollback — NOT FOUND

- **Searched:** all of `stored-procedures-part1.md`–`part4.md`, case-insensitive. No match for this exact name (nor any header/EXEC reference to it).
- **Status:** Still **HUMAN VERIFICATION REQUIRED** — this procedure's body remains unavailable in any reviewed source. If any prior audit named this procedure specifically as a rollback mechanism, that claim is unverifiable from the codebase docs and should stay flagged as SUSPECTED/UNKNOWN, not CONFIRMED.

## 13. sp_DeliveryInvoiceDeletePosting

- **Found in:** `stored-procedures-part1.md:11562`
- **Summary:** Despite the `@InvoiceId` parameter name/typical usage, it is actually declared `NVARCHAR(MAX)` and matched against `tblInvoice.DelivaryInvoiceNo` (not the numeric ID) — a naming trap the annotation explicitly flags. Posts a 'Delivery Invoice Delete' reversal voucher (credit customer GL / credit+debit sales-return-mapped GLs) plus a separate 'Delivery Invoice Delete, Inventory Valuation' voucher reversing stock value.
- **Transaction/error handling:** None.
- **Resolves/changes:** Important for `reconciliation-audit.md`/`return-audit.md`: confirms a real reversal-voucher mechanism exists for delivery-invoice deletion (distinct from `sp_DeleteDeliveryInvoice`, which only reverses stock quantities and does NOT touch the GL — see #16 below). If a caller invoked the wrong one of these two by mistake, GL and stock could go out of sync; worth flagging as a real risk area, not hypothetical.
- **Confidence:** CONFIRMED.

## 14. sp_Delete_ProformaInvoice

- **Found in:** `stored-procedures-part1.md:9774`
- **Summary:** Hard-deletes a proforma invoice: for received DC-store lines, adds quantity back to `tblDCStore.StockQty`; archives detail rows to `tblInvoiceDetail_DeleterRecord`; records reversal in `tblProInvoiceReturnTrack`; clears parent order's invoiced flag; logs to `tblInvoiceDeleteLog`; then hard-deletes `tblInvoice`/`tblInvoiceDetail` rows. **The accounting-voucher cleanup block at the end is commented out (dead code)** — GL vouchers are never removed/reversed when a proforma invoice is deleted this way.
- **Transaction/error handling:** None — no explicit transaction wraps the multi-step delete/archive/update sequence, so a mid-failure leaves partial state (e.g., stock restored but header not yet deleted, or vice versa).
- **Resolves/changes:** **Confirms and sharpens** a likely `financial-integrity-audit.md`/`reconciliation-audit.md` concern: deleting a posted proforma invoice leaves its GL voucher orphaned/still-posted while the invoice itself disappears — a genuine, code-confirmed accounting-integrity gap, not speculative.
- **Confidence:** CONFIRMED.

## 15. sp_Deletenvoice

- **Found in:** `stored-procedures-part1.md:10086`
- **Summary:** Given `@OrderID`, reverses and hard-deletes the invoice tied to that order: adds each detail line's `TotalQuantity` back to DC store stock, deletes `tblInvoice`/`tblInvoiceDetail` rows, resets `tblOrder.IsInvoice = 0`.
- **Transaction/error handling:** None.
- **Resolves/changes:** Like #14, this proc **never touches any GL/voucher table at all** — no attempt, not even commented-out code. Any invoice deleted through this path (`InvoiceDAL.cs DeleteInvoice`) that had already been posted to the GL (via `sp_ProformaPosting`/`sp_DeliveryConfirmationFullPosting`/etc.) leaves that posting permanently orphaned. This is a stronger, more concrete version of whatever prior audits flagged as "delete doesn't reverse accounting" — confirmed with zero ambiguity.
- **Confidence:** CONFIRMED.

## 16. sp_DeleteDeliveryInvoice

- **Found in:** `stored-procedures-part1.md:9214`
- **Summary:** Given `@DelivaryInvoiceNo`/`@userid`, and ONLY if no customer payment has ever been recorded against the invoice: reverses delivered-but-not-shipped quantity back onto DC stock, clears freeze rows, blanks delivery columns on the invoice, logs to `tblInvoiceDeleteLog`. The block that would delete matching posted sales/inventory GL vouchers is present in the file but **commented out (dead code)**.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms the same class of gap as #14/#15 — a real, guarded (payment-check) reversal exists for stock, but the accompanying GL cleanup was written and then disabled. This is a stronger finding than "unknown" — it shows the developers were aware GL cleanup was needed (code exists) but it's inert.
- **Confidence:** CONFIRMED.

## 17. sp_WHStockInPosting

- **Found in:** `stored-procedures-part3.md:14340`
- **Summary:** Company-16-restricted GL posting for warehouse stock-in (`Particular='WHStockJournal'`), same cursor/voucher-code/Dr-Cr-via-mapping-table pattern as `sp_ProformaPosting`; sets `tblWHStockInMaster.IsPosting`.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms `distribution-audit.md`/`database-audit.md` assumptions about warehouse-in GL integration pattern being structurally identical to the sales-side posting procs (same `tblSalesIntegrationMapingNew`-driven design, same lack of transaction safety).
- **Confidence:** CONFIRMED.

## 18. sp_Process_WeightedAverage

- **Found in:** `stored-procedures-part2.md:8686`
- **Summary:** Recomputes depot-level weighted-average cost for a single depot stock receipt (`@DCStoreId`). Reads opening qty/amount from the latest `tbl_INV_WeightedAvg` row, adds current receipt, and — duplicate-guarded by `Trans_Type_Id=1 + Trans_Id` — inserts new weighted-average and batch-wise stock rows.
- **Transaction/error handling:** None. **Notable defect found directly in the body:** a leftover debug statement `SELECT * FROM tbl_INV_WeightedAvg` sits inside the cursor loop, returning one full-table result set per loop iteration — this is very likely an unintentional debugging leftover, and depending on the ADO.NET call pattern (`ExecuteNonQuery` vs. `ExecuteReader`) could either be silently ignored or cause unexpected multi-result-set handling issues upstream.
- **Resolves/changes:** New finding not previously flagged in any reviewed `docs/audit/*.md` file — worth adding to `performance-audit.md`/`database-audit.md` as a concrete, code-confirmed defect (not the accounting-correctness kind, but a code-quality/performance one: repeated full-table scans inside a loop).
- **Confidence:** CONFIRMED, with a new defect identified.

## 19. sp_Process_WeightedAverageB2BTransfer

- **Found in:** `stored-procedures-part2.md:8931`
- **Summary:** Records weighted-average cost impact of a branch-to-branch challan issue: computes opening qty/rate, subtracts (negates) issued qty/value, inserts negative weighted-average + batch-wise rows if not already recorded for that challan master+detail pair.
- **Transaction/error handling:** None.
- **Confidence:** CONFIRMED.

## 20. sp_Process_WeightedAverageFreezeStock

- **Found in:** `stored-procedures-part2.md:9258`
- **Summary:** Records weighted-average cost impact of customer-returned stock placed into freeze (`tblDCStoreFreeze`): adds positive receive qty/value, inserts new weighted-average + batch-wise rows tagged with the freeze ID, if not already recorded for that pair.
- **Transaction/error handling:** None.
- **Confidence:** CONFIRMED.

## 21. sp_FreezeStockPosting

- **Found in:** `stored-procedures-part1.md:13121`
- **Summary:** Company-16-restricted. Generates a voucher code and posts a 'Freeze Stock' Debit/Credit pair (value = `UnitPrice * @Quantity`) into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster/Detail`; flags the freeze record's `IsPosting`. Annotated as "the self-contained counterpart to `sp_ADJ_FreezeStockPosting`" and there is a mirror procedure for release (`FreezeStockRelease` GL key, counterpart `sp_ADJ_FreezeStockReleasePosting`) immediately following it in the same file.
- **Transaction/error handling:** None.
- **Confidence:** CONFIRMED.

## 22. sp_DEL_DuplicatePayment

- **Found in:** `stored-procedures-part1.md:7907`
- **Summary:** Given `@InvoiceNo`, finds payment-detail rows sharing a `TransctionDetailId` occurring more than once, keeps the first, and for every OTHER duplicate DELETES its posted GL vouchers (`ZAS_ACCDB..tblDebitCreditVoucherMaster/Detail`) plus the payment/payment-detail rows themselves.
- **Transaction/error handling:** None — a partial failure mid-delete could remove the voucher but leave the payment-detail row, or vice versa.
- **Resolves/changes:** This directly corroborates `cross-module-impact.md:71`'s finding, already noted by a prior audit, that *"sp_DEL_DuplicatePayment — if run against this false-positive set, deletes legitimate payment records."* Reading the body confirms the "duplicate" grouping key is `TransctionDetailId` occurring >1 time with no additional disambiguation (e.g., no check on amount, date, or whether the rows are truly identical vs. coincidentally sharing that key) — the false-positive risk that prior audit called out is architecturally real and directly visible in the WHERE/GROUP BY logic, not speculative.
- **Confidence:** CONFIRMED — corroborates and strengthens an existing finding rather than changing it.

## 23. sp_Generate_InvoiceNo

- **Found in:** `stored-procedures-part2.md:1915`
- **Summary:** Generates a sequential "group invoice number" (`INV-GP01-YYMMDDNNNN`) for the day and updates all `tblInvoice` rows whose `InvoiceNo` is in a caller-supplied list. Builds the `UPDATE ... WHERE InvoiceNo IN (...)` as dynamic SQL via string concatenation and `EXEC`.
- **Transaction/error handling:** None. **Security note directly visible in the body:** `@InvoiceNo` is concatenated straight into the dynamic SQL `IN (...)` clause with no parameterization/escaping — a SQL-injection vector if any caller ever passes unsanitized user input as part of that list. Worth a line in `security-audit.md` if not already present.
- **Confidence:** CONFIRMED.

## 24. sp_I_InvoiceMaster

- **Found in:** `stored-procedures-part2.md:5234`
- **Summary:** Guards against duplicate/rapid submission (same `OrderNo` processed within the last 60 seconds is rejected), generates sequential `InvoiceNo` (`INV-{UnitCode}-{yyMMdd}{seq}`) per company-unit/day, inserts the invoice header. Returns `@InvoiceId = 0` (not an error) if the 60-second guard trips or a duplicate `InvoiceNo` is found.
- **Transaction/error handling:** None — the time-window duplicate check is explicitly **not concurrency-safe** (two simultaneous submissions within the same second could both pass the check and get sequence collisions).
- **Resolves/changes:** Relevant to `sales-audit.md`/`data-integrity-audit.md` concerns about duplicate invoice creation under concurrent submission — confirms the guard exists but is a time-window heuristic, not a real concurrency control (no `SERIALIZABLE`, no unique constraint enforcement mentioned, no `sp_getapplock`).
- **Confidence:** CONFIRMED.

## 25. sp_DeliveryConformationFull

- **Found in:** `stored-procedures-part1.md:11214`
- **Summary:** Given `@InvoiceNo`/`@UpdateBy`/`@UpdateDate`, and only if not already delivery-confirmed: copies every ordered qty/price/discount into the `Delivery*` columns as a full (100%) delivery, marks each line `DeliveryStatus='Full'`, stamps the header `DeliveryInvoiceStatus='Full'` and generates a `DEL-<InvoiceNo>` delivery invoice number. **No GL posting happens in this proc at all** — it only sets delivery status/quantities on the invoice/detail rows.
- **Transaction/error handling:** None.
- **Resolves/changes:** Important structural clarification for `accounting-integration-audit.md`: this is the proc that actually flips an invoice to "delivered," but the corresponding GL posting is a **separate step** (see #2, #4 above) that must be triggered independently — confirms the sales-to-cash pipeline has multiple disjoint stored procedures with no single transactional boundary tying "mark delivered" to "post to GL."
- **Confidence:** CONFIRMED.

## 26. sp_DeliveryConformationReject

- **Found in:** `stored-procedures-part1.md:11345`
- **Summary:** Mirror of #25 for rejected deliveries: zeroes out `Delivery*` columns, marks `DeliveryStatus='Reject'` with reason, and — if no freeze record exists yet — inserts one into `tblDCStoreFreeze` to hold the rejected stock as frozen pending disposition; stamps header rejected with zeroed totals and `DEL-<InvoiceNo>` number.
- **Transaction/error handling:** None.
- **Confidence:** CONFIRMED.

## 27. sp_Acc_MaterialConversionReceive

- **Found in:** `stored-procedures-part1.md:4573`
- **Summary:** On `@ApprovalStatus IN ('ACCEPTED','APPROVED')` for a warehouse stock-in master: if not already posted, creates a voucher header + Debit(Finished Goods 4156)/Credit(Material Conversion Clearing 5882) pair sized to the stock-in's total value; then cursors each stock-in detail line, updating physical central-store stock, the weighted-average ledger, and per-batch stock, computing opening/closing qty and weighted-average rate per product.
- **Transaction/error handling:** None.
- **Resolves/changes:** Confirms the "missing postings" catch-up job (`stored-procedures-part2.md:6958`) exists specifically because this proc's posting is not guaranteed — same class of finding as #8.
- **Confidence:** CONFIRMED.

---

## Summary table

| # | Procedure | Found? | Part file | Confidence |
|---|---|---|---|---|
| 1 | sp_ProformaPosting | Yes | part3 | CONFIRMED |
| 2 | sp_Sales_ExecuteDeliveryPipeline | Yes | part3 | CONFIRMED |
| 3 | sp_Sales_DeliverySalesPosting | Yes | part3 | CONFIRMED |
| 4 | sp_DeliveryConfirmationFullPosting | Yes | part1 | CONFIRMED (dead code) |
| 5 | sp_DeliveryConfirmationPartiallPosting | Yes | part1 | CONFIRMED (dead code) |
| 6 | sp_ProformaReturnPosting | Yes | part3 | CONFIRMED |
| 7 | sp_ProformaFullReturnPosting | Yes | part3 | CONFIRMED |
| 8 | sp_Sales_DeliverySalesReturnPosting | Yes | part3 | CONFIRMED |
| 9 | sp_UD_InvoicePayment | Yes | part3 | CONFIRMED (corrects BUG-FIN-001) |
| 10 | sp_Sales_CustomerPaymentPosting | Yes | part3 | CONFIRMED |
| 11 | sp_Save_SaveCustPayment | Yes | part3 | CONFIRMED |
| 12 | sp_DeleteDeliveryInvoice_FullRollback | **No** | — | HUMAN VERIFICATION REQUIRED |
| 13 | sp_DeliveryInvoiceDeletePosting | Yes | part1 | CONFIRMED |
| 14 | sp_Delete_ProformaInvoice | Yes | part1 | CONFIRMED |
| 15 | sp_Deletenvoice | Yes | part1 | CONFIRMED |
| 16 | sp_DeleteDeliveryInvoice | Yes | part1 | CONFIRMED |
| 17 | sp_WHStockInPosting | Yes | part3 | CONFIRMED |
| 18 | sp_Process_WeightedAverage | Yes | part2 | CONFIRMED (new defect: debug SELECT in loop) |
| 19 | sp_Process_WeightedAverageB2BTransfer | Yes | part2 | CONFIRMED |
| 20 | sp_Process_WeightedAverageFreezeStock | Yes | part2 | CONFIRMED |
| 21 | sp_FreezeStockPosting | Yes | part1 | CONFIRMED |
| 22 | sp_DEL_DuplicatePayment | Yes | part1 | CONFIRMED (corroborates cross-module-impact.md) |
| 23 | sp_Generate_InvoiceNo | Yes | part2 | CONFIRMED (SQL-injection note) |
| 24 | sp_I_InvoiceMaster | Yes | part2 | CONFIRMED |
| 25 | sp_DeliveryConformationFull | Yes | part1 | CONFIRMED |
| 26 | sp_DeliveryConformationReject | Yes | part1 | CONFIRMED |
| 27 | sp_Acc_MaterialConversionReceive | Yes | part1 | CONFIRMED |

## Cross-cutting observations (new, not module-specific)

1. **No stored procedure in this set uses an explicit transaction, TRY/CATCH, or RAISERROR.** Every posting/deletion procedure reviewed here is a sequence of unguarded statements; any mid-procedure failure leaves partial state. This is a single systemic root cause behind many separate "no rollback" findings scattered across `financial-integrity-audit.md`, `reconciliation-audit.md`, and `root-cause-analysis.md` — worth consolidating into one root-cause entry rather than treating each instance as independent.
2. **Multiple GL-posting procedures are dead code at their originally-intended call site** (`sp_DeliveryConfirmationFullPosting`, `sp_DeliveryConfirmationPartiallPosting`) but have self-contained live siblings with an `_ADJ_` naming pattern. Any audit that assumed the plain-named proc is what actually runs in production should be corrected to point at the `sp_ADJ_*` counterpart instead.
3. **Three separate invoice-delete procedures exist** (`sp_Deletenvoice`, `sp_DeleteDeliveryInvoice`, `sp_Delete_ProformaInvoice`) plus a GL-specific one (`sp_DeliveryInvoiceDeletePosting`), and only the last one touches accounting vouchers. Two of the other three have GL-cleanup code physically present but commented out. This is a clear, code-confirmed pattern (not a hypothesis) that deleting an invoice through most of the available code paths orphans its GL postings.
4. **"Missing postings" catch-up/backfill jobs exist** for at least four posting procedures (`sp_Sales_CustomerPaymentPosting`, `sp_Sales_DeliverySalesPosting`, `sp_Sales_DeliverySalesReturnPosting`, `sp_Acc_MaterialConversionReceive`), all scoped to rows updated on/after 2025-04-25. Their mere existence is first-hand evidence (not inference) that these postings have historically failed to fire reliably in production.
