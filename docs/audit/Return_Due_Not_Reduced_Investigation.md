# Delivery Sales Return done but Due not reduced — Investigation

**Reported on:** 2026-10-21, `PaymentTranscationNew.aspx`, invoice `INV-BD166-2610080003`
(Rangpur - ZAS Bioplasma, Receivable 123,000.00, Previous Pay 0.00, Due 123,000.00).

**Verdict:** the Due column in batch mode cannot reflect a return, no matter how the return was posted.
Two of the four grid queries have a wrong Due formula. Code-level findings are CONFIRMED; the data for this
specific invoice is UNVERIFIED (production reads were not permitted from this session; SQL is at the bottom).

## How Due is supposed to work

Returns never touch `tblInvoice` or `tblInvoiceDetail`. The only bridge between a return and the payment screen
is an accounting voucher:

1. `ReturnProductCustomerByInvoice.aspx` saves `tblCustomerReturnMaster/Detail` (status pending).
2. `ReturnustomerByInvoiceApproval.aspx` sets `ActionStatus = 'Accepted'` and calls
   `sp_Sales_DeliverySalesReturnPosting @ReturnId` (`CustomerReturnDAL.cs:138-145`).
3. The SP inserts one `CSTL_ACCDB..tblDebitCreditVoucherMaster` row with `TableID = 2018`,
   `CommonPK_ID = InvoiceId` (`stored-procedures-part3.md:3403-3428`) and a debit detail line
   `DebitAmountBDT = @ReturnAmount` ("Sales Return").
4. The payment grid is expected to compute
   `Due = DeliveryTotal - (Paid + AIT + Discount) - SUM(DebitAmountBDT of TableID 2018 vouchers for the invoice)`.

## Findings

### F1 — Batch mode, Admin user: returns are never deducted (matches the screenshot)

`PaymentTranscationNew.aspx.cs:885-990` picks the query by mode. Batch dropdown + Admin →
`CustPaymentBLL.LoadInvoiceNew` → `CustPaymentDAL.cs:226-235`:

```sql
(ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0))) AS Due
```

There is no `RTN` join and no return term at all. Receivable = Due whenever nothing has been paid, exactly
what the screenshot shows. `PaymentTranscationNew2.aspx.cs:719,832` calls the same method, so New2 has the same
behaviour.

### F2 — Batch mode, non-Admin user: returns are deducted twice

`LoadInvoiceNewByUser` (`CustPaymentDAL.cs:472` and the `WHERE` at `:489`):

```sql
... - ISNULL(RTN.ReturnAmount,0)  - ISNULL(RTN.ReturnAmount,0) AS Due
```

Copy-paste duplicate. Due is understated by one return amount, and an invoice whose return is more than half
of the remaining balance drops off the list entirely (`>= 1` filter).

### F3 — Customer mode: formula is right, but only if the voucher exists

`GetInvoiceByCustomer` (both overloads, `CustPaymentDAL.cs:398-409` and `:433-443`) deduct the voucher
amount once. Same formula is used by `CustomerDueReportDAL`, `CreditLimitDAL`, `OrderInfoDAL`. If Due is also
wrong in customer mode, the voucher is missing. Known ways it goes missing:

- Return not yet **Accepted** on the approval screen (SP exits silently unless `ActionStatus = 'Accepted'`).
- SP has no TRY/CATCH and the C# discards the result; any failure is silent. The cursor source uses
  `INNER JOIN tblCentralStore`, `INNER JOIN tblCompanyInfo`, `INNER JOIN tblFinancialYear` on `ApprovedDate` —
  a missing financial year for the approval date, or a DC-store row without `TempReceiveId`, yields zero rows
  and no posting.
- SP pre-check bug (`part3.md:3199-3201`): it looks for an existing voucher with `CommonPK_ID = CustReturnId`,
  but the SP stores `CommonPK_ID = InvoiceId`. If this return's `CustReturnId` happens to equal an `InvoiceId`
  that already has a return voucher, the return is silently never posted. The check also does not actually
  prevent double posting of the same return.
- A backfill job `sp_Missing_DeliverySalesReturnPosting` exists in the DB and is not called by the app;
  its existence confirms postings have gone missing before (`accounting-integration-audit-followup.md:75-80`).

### F4 — Not the cause, for completeness

`tblCreditAdjustment` is written only for Credit payment type at return save time
(`ReturnProductCustomerByInvoice.aspx.cs:170-195`). No due query reads it. Irrelevant to the displayed Due.

## Fix (minimal)

One Due expression, used by all four grid queries:

1. `LoadInvoiceNew` (`CustPaymentDAL.cs:226-235`): add the same `RTN` LEFT JOIN used at `:476-479` and
   subtract `ISNULL(RTN.ReturnAmount,0)` in both SELECT and WHERE.
2. `LoadInvoiceNewByUser` (`:472`, `:489`): delete the second `- ISNULL(RTN.ReturnAmount,0)` in both places.
3. Optional, SP: change the pre-check join at `:3200` to match on the invoice, or drop it (the cursor already
   handles per-invoice rows).

After 1 and 2, any invoice whose return was actually posted will show the reduced Due in every mode. If
`INV-BD166-2610080003` still shows 123,000 afterwards, the voucher is missing (F3) and
`EXEC sp_Missing_DeliverySalesReturnPosting` (or re-approval) is the data fix.

## Verified on restored live copy (2026-10-21, TOWSIF\MSSQLSERVER2019, SalesRollDB_ZAS)

| Check | Result |
|---|---|
| Invoice 887989 `INV-BD166-2610080003` | Delivered 123,000.00, no payments |
| Return 3205 | `Accepted` 2026-10-20, 25,000.00, 5 pcs |
| Voucher `TableID 2018` for invoice | **none** → posting never happened |
| Why SP posted nothing for 3205 | `tblDCStore.TempReceiveId IS NULL` for the returned line; the SP's `INNER JOIN tblCentralStore` drops every cursor row. 1,555 of the 1,556 newest DC-stock rows have NULL `TempReceiveId`, so every new return will hit this. |
| Scope | Last return voucher posted 2026-08-22. 25 accepted returns since 2026-08-26 are unposted. 4 fail the central-store join; the other 21 pass every join and post correctly when the SP is run by hand (tested 3201, 3202, 3203, under 1 s each). Why the app's call failed for those on the live server is UNVERIFIED from here. Read `App_Data\DatabaseErrors.log` on the web server. |
| SP fix test | `database/reconcile/02_DeliverySalesReturnPosting_CentralStoreJoin.sql` (INNER → LEFT JOIN) applied locally, SP run for 3205: voucher posted 25,000. Fixed Admin-batch query now returns Due **98,000.00** (was 123,000.00). |

Note: the test copy now holds vouchers for returns 3201, 3202, 3203, 3205 that live does not.

### Deployment order

1. Deploy `Library.DAL.dll` (query fix).
2. Run `database/reconcile/02_DeliverySalesReturnPosting_CentralStoreJoin.sql` on live.
3. Run `EXEC sp_Missing_DeliverySalesReturnPosting` to backfill the 25 unposted returns (it only posts Accepted returns that have no voucher).
4. Re-open the payment page; INV-BD166-2610080003 should show Due 98,000.00.

## Verification SQL (run against SalesrollDB_ZAS, read-only)

```sql
DECLARE @inv INT = (SELECT TOP 1 InvoiceId FROM tblInvoice WHERE InvoiceNo = 'INV-BD166-2610080003');

SELECT 'INVOICE' k, InvoiceId, DeliveryTpGrandTotal, PaymentAmount, AIT, DiscountOnPayment, PaymentStatus
FROM tblInvoice WHERE InvoiceId = @inv;

SELECT 'RETURN' k, M.CustReturnId, M.ActionStatus, M.ApprovedDate, M.TotalAmount, SUM(D.ReturnAmount) DetailAmt
FROM tblCustomerReturnMaster M
LEFT JOIN tblCustomerReturnDetail D ON D.CustReturnId = M.CustReturnId
LEFT JOIN tblInvoiceDetail ID ON ID.InvoiceDetailId = D.InvoiceDetailId
WHERE ID.InvoiceId = @inv
GROUP BY M.CustReturnId, M.ActionStatus, M.ApprovedDate, M.TotalAmount;

SELECT 'VOUCHER' k, V.VoucherMasterId, V.VoucherCode, V.IsInventoryValuation, SUM(D.DebitAmountBDT) Dr
FROM CSTL_ACCDB..tblDebitCreditVoucherMaster V
LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail D ON D.VoucherMasterId = V.VoucherMasterId
WHERE V.TableID = 2018 AND V.CommonPK_ID = @inv
GROUP BY V.VoucherMasterId, V.VoucherCode, V.IsInventoryValuation;
```

Expected: RETURN row with `ActionStatus = 'Accepted'`, and a VOUCHER row whose `Dr` equals the return amount.
No VOUCHER row → F3 (posting missing). VOUCHER row present → F1/F2 alone explain the screenshot.
