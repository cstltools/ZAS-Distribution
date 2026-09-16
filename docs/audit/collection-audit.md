# Collection / Payment Audit

Scope: cash/bank collection, cheque, credit-note/debit-note (`tblCreditAdjustment`), advance, allocation logic,
duplicate-payment detection, and payment reversal. Audit-only — no source or data was modified. All line numbers
verified by direct file reads on 2026-09-05.

Baseline confirmed from `specs/SPEC-GAPS.md`:

- **GAP-APP-016** — `CustPaymentBLL.UpdateInvoicePaymentAmount(amount,status,id)` → `CustPaymentDAL.UpdateInvoicePaymentAmount`
  passes only `@InvoiceId` to `sp_UD_InvoicePayment`; `amount`/`status` params are declared but never added to the
  `SqlParameter` list. **CONFIRMED** — read directly:
  `Library.BLL/SInventory_BLL/CustPaymentBLL.cs:95-98` and `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` (method
  builds `List<SqlParameter>` with `@InvoiceId` only). The screen's Full/Partial decision
  (`CustomerPayment.aspx.cs` saveButton_Click) is dead code.
- **GAP-APP-017** — `CustPaymentBLL.SaveCustPayment(CustPayment, List<CustPaymentDetail>)` at
  `Library.BLL/SInventory_BLL/CustPaymentBLL.cs:18-69` calls `ClsPrimaryKeyFind.PrimaryKeyMax` and
  `aCustPaymentDal.SaveCustPayment(aCustPayment)` **once**, then loops details — this signature itself is not the
  bug. The bug is in the caller: `CustomerPayment.aspx.cs::saveButton_Click` builds a **new** one-element detail
  list inside its per-invoice loop and calls this method **once per ticked invoice**, so each call inserts its own
  `tblCustomerPay` master row, every one carrying the full header `PaymentAmount`. **CONFIRMED** by the workflow
  trace in `specs/finance-collection/workflows/WF-customer-payment.md` (Stage 5) cross-checked against
  `CustPaymentBLL.cs` and `CustPaymentDAL.SaveCustDetail` (`Library.DAL/SInventory_DAL/CustPaymentDAL.cs:99-128`).

**Financial-impact finding (new, this audit):** GAP-APP-017's per-invoice-master duplication does **not** overstate
the amounts booked to the general ledger or shown on the outstanding/due/collection reports, because every one of
those downstream consumers sums `tblCustPayDetail.PaymentAmount` (the correctly allocated per-invoice amount), not
`tblCustomerPay.PaymentAmount` (the inflated per-master header value). Evidence:

- `sp_Sales_CustomerPaymentPosting` (real body recovered in `docs/database/stored-procedures-part3.md:1586-1925`)
  posts the journal from a cursor filtered `WHERE CPD.InvoiceId = @InvoiceId AND CPD.CustPayId = @CustPayId`,
  using `CPD.PaymentAmount`/`AIT`/`Discount` — the per-detail row, not the master.
- `CustomerDueReportDAL.LoadCustomerDueReport`/`LoadCustomerTotalDueReport`
  (`Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs:105-107,138-140`) join and `SUM(CPD.PaymentAmount)`.
- Grep across `Library.DAL` (`BatchWiseCollectionReportDal.cs:35`, `DepositSlipEntryDal.cs:72`,
  `CustomerPaymentReportDal.cs:38-46`, `FieldForceSummeryDal.cs:126`, `SalesDashboardDal.cs:104`,
  `CustomerWiseLedgerDal.cs:111`, `ManualOrderCreationDal.cs:139,341`, `OrderListDAL.cs:158`,
  `SampleIssueDal.cs:139`) shows every collection/due/ledger query aggregates `tblCustPayDetail`, not
  `tblCustomerPay`. **No caller found that sums `tblCustomerPay.PaymentAmount` directly.**

What GAP-APP-017 *does* break, confirmed by this audit:

- `tblCustomerPay` no longer represents "one physical receipt" — a five-invoice receipt creates five master rows,
  each with its own `CustPayId`, `RefNo`/`RefDate` and the full receipt amount. Any manual reconciliation against a
  single cheque/deposit-slip number, or any future report that (reasonably) assumes one row = one receipt, will
  overcount receipts N-fold and misstate the "amount per master row." Confidence: **CONFIRMED** mechanism,
  **SUSPECTED** for which manual/offline processes rely on that 1-receipt-1-row assumption (not enumerable from
  code).

---

## BUG-FIN-001 — `UpdateInvoicePaymentAmount` discards amount/status (GAP-APP-016, confirmed)

- **Severity:** HIGH **Confidence:** CONFIRMED
- **File:** `Library.BLL/SInventory_BLL/CustPaymentBLL.cs:95-98`, `Library.DAL/SInventory_DAL/CustPaymentDAL.cs`
  (`UpdateInvoicePaymentAmount`)
- The invoice's paid amount/status is decided entirely inside `sp_UD_InvoicePayment` from `@InvoiceId` alone.
  HUMAN VERIFICATION REQUIRED — needs `sp_helptext 'sp_UD_InvoicePayment'` (not present in `docs/database/`) to
  know what it actually does.

## BUG-FIN-002 — One receipt → N `tblCustomerPay` master rows (GAP-APP-017, confirmed, impact scoped)

- **Severity:** HIGH **Confidence:** CONFIRMED (mechanism); financial-statement impact is lower than the raw
  description suggests — see the impact analysis above. Impact: data-integrity / audit-trail corruption of the
  receipt table, not GL/AR overstatement.
- **File:** `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs` (`saveButton_Click`), `Library.BLL/SInventory_BLL/CustPaymentBLL.cs:18-69`

## BUG-FIN-003 — `PaymentReverse.aspx` never reverses the accounting journal — CRITICAL, new finding

- **Severity:** CRITICAL **Confidence:** CONFIRMED
- **Files:** `Solution.Web/SInventory_UI/PaymentReverse.aspx.cs:24-83`, `Library.DAL/SInventory_DAL/PaymentReverseDal.cs:1-59`,
  contrasted with the real `sp_Sales_CustomerPaymentReversePosting` body at
  `docs/database/stored-procedures-part3.md:1956-2314`.

`PaymentReverse.aspx` (`submitButton_Click1` → `DeleteCustomerPaymentInfo`) does:

```csharp
ResetInvoicePaymentStatus(inoiceId);                        // UPDATE tblInvoice SET PaymentStatus=NULL, PaymentAmount=NULL
aDal.InsertIntoDeleteLog(...)                               // sp_I_PaymentDeleteLog (audit only)
aDal.DeletePaymentMaster(CustPayId);                         // DELETE FROM tblCustomerPay
status = aDal.DeletePaymentDetail(CustPayDetailId);          // DELETE FROM tblCustPayDetail
```

— raw string-concatenated `DELETE`s (`PaymentReverseDal.cs:35-45`), **no call anywhere in this page to
`sp_Sales_CustomerPaymentReversePosting`**. That procedure exists and is called by the application, but only from
`Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:280`, reached from a *different* screen's
`invoiceButton_Click` (per `specs/finance-collection/pages/PAGE-FIN-019-MoneyReceiptReport.md`). The real procedure,
when it does run, correctly (a) inserts a reversing debit/credit voucher into
`CSTL_ACCDB..tblDebitCreditVoucherMaster/Detail` reversing the original cash/bank Dr and AR Cr, (b) deletes the
`tblCustomerPay`/`tblCustPayDetail` rows, and (c) **recomputes** `tblInvoice.PaymentAmount/AIT/DiscountOnPayment/PaymentStatus`
from whatever `tblCustPayDetail` rows remain (lines 2291-2312 of the SP body).

`PaymentReverse.aspx` instead:

1. Deletes the SalesRoll rows directly, with **no accounting-side reversal at all** — the original journal entry
   booked by `sp_Sales_CustomerPaymentPosting` (cash/bank debited, AR credited) stays posted in `CSTL_ACCDB`
   permanently. Cash-in-hand / bank ledger and AR remain misstated by the reversed amount indefinitely.
2. Blindly sets `PaymentAmount`/`PaymentStatus` to `NULL` (`PaymentReverseDal.cs:25`) instead of recomputing from
   remaining detail rows — if the invoice ever had more than one `CustPayId` (multiple receipts across time), this
   wipes out payment history for **all** of them, not just the one being reversed, and does not touch the cached
   `AIT`/`DiscountOnPayment` columns at all (stale values survive the "reversal").
3. Has no authorization/approval gate (`ClsApprovalAction` is not used on this page) — any user with menu access to
   `PaymentReverse.aspx` can wipe out a customer's payment history for an arbitrary order by typing its order
   number, with the invoice becoming payable again while the cash already booked in the books is never clawed back.

**Net effect:** every reversal through this screen leaves the books overstating cash/bank received and understating
AR, while the SalesRoll side shows the invoice unpaid and re-collectible — a double-collection / accounting-desync
risk on every use. HUMAN VERIFICATION REQUIRED to confirm whether `PaymentReverse.aspx` is actually reachable from
the live menu (menu data is not in the repository — `GAP-SEC-001`) — if it is wired in, this is the single highest
financial-integrity risk found in the collection module.

## BUG-FIN-004 — `DuplicatePaymentList` root cause: near-universal false positives (PAGE-FIN-017)

- **Severity:** CRITICAL (data-loss risk via the "fix") **Confidence:** CONFIRMED mechanism, SUSPECTED for real-world false-positive rate (needs live data)
- **Files:** `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs:15-27`,
  `Library.DAO/SInventory_Entities/CustPaymentDetail.cs:24`, `Library.DAL/SInventory_DAL/CustPaymentDAL.cs:99-128`,
  `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs:491-502`.

`GetDuplicatePaymentList()` flags a payment as a duplicate with:

```sql
WHERE TransctionDetailId IN (
    SELECT TransctionDetailId FROM tblCustPayDetail
    GROUP BY TransctionDetailId HAVING COUNT(TransctionDetailId) > 1)
```

`CustPaymentDetail.TransctionDetailId` is a non-nullable `int` (`CustPaymentDetail.cs:24`, default `0`). Grepping
every writer of this field across the whole repository, it is populated with a real value in exactly **one**
place: `CustomerPaymentProcess.aspx.cs:495` (the PaymentTranscation → confirm-payment workflow). Every other screen
that inserts into `tblCustPayDetail` — including the main `CustomerPayment.aspx` flow traced above and
`MarketWiseCustomerPayment.aspx` — never sets it, so `CustPaymentDAL.SaveCustDetail` (`CustPaymentDAL.cs:104-127`)
inserts it as `0` (see the raw `INSERT` values list). The instant there are two or more such `0`-valued rows
anywhere in the whole payment history (which will be true almost immediately in any live system), the `GROUP BY
TransctionDetailId HAVING COUNT(...) > 1` clause groups **all of them together as one giant "duplicate" set** —
not genuine double-entries. `DuplicatePaymentList.aspx`'s `removeButton_Click` then calls `sp_DEL_DuplicatePayment`
by invoice number (body UNKNOWN — HUMAN VERIFICATION REQUIRED for `sp_helptext 'sp_DEL_DuplicatePayment'`) against
rows an operator has been shown as "duplicates," most of which are ordinary, legitimate payments that merely share
the same default `TransctionDetailId = 0`.

This explains, from code, why duplicate payments are already a known, named problem in this application (the page
exists specifically to clean them up): true duplicate detection was only ever implemented for the narrow
PaymentTranscation/CustomerPaymentProcess path; the majority of the payment-entry surface (`CustomerPayment.aspx`
and variants) has no duplicate guard, and the "cleanup" screen intended to catch strays instead surfaces (and risks
deleting) ordinary payments.

## BUG-FIN-005 — Over-application / overpayment is never validated

- **Severity:** HIGH **Confidence:** CONFIRMED
- **File:** `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs::Validation()` (per
  `specs/finance-collection/workflows/WF-customer-payment.md` Stage 2) — checks only that the sum of ticked-row
  amounts equals the header `paymentAmountTextBox`; nothing compares a row's amount to that invoice's due amount.
- Consequence, confirmed against the real `sp_Sales_CustomerPaymentReversePosting`/posting SP logic
  (`docs/database/stored-procedures-part3.md:2296-2300`): `PaymentStatus` is only set to `'Full'` on **exact**
  equality `(...) = DeliveryTpGrandTotal`; anything else (including an overpayment) falls into the `ELSE 'Partial'`
  branch. An overpaid invoice is therefore permanently flagged `'Partial'`, not `'Full'` or "credit". Reports that
  branch on `PaymentStatus` (rather than recomputing the due arithmetic themselves) would keep such an invoice as
  "due" even though it has been overpaid. **SUSPECTED** for exactly which report(s) branch on the status flag
  rather than recomputing — most due queries found in this audit recompute the arithmetic directly and would show
  a negative (credit) due instead, which is still wrong presentation but not a missed-collection risk.

## BUG-FIN-006 — Payment-type / credit-adjustment routing by display text or magic literal (GAP-APP-019, confirmed)

- **Severity:** MEDIUM **Confidence:** CONFIRMED
- Cash/Bank account requirement is enforced by comparing `payTypeDDL`'s **display text** to `"Cash"`/`"Bank"`; the
  credit-adjustment branch (`tblCreditAdjustment` insert, always a negative amount) is selected by the **hard-coded
  value `"2"`** (`CustomerPayment.aspx.cs`, Stage 3 of the WF spec). Renaming either lookup row silently disables
  validation or misroutes a credit note. `tblPaymentType` data is not in this repository — HUMAN VERIFICATION
  REQUIRED to confirm what id `2` currently means in production.

## BUG-FIN-007 — Financial-year guard is commented out

- **Severity:** MEDIUM **Confidence:** CONFIRMED (the guard is disabled in source; the SP-side consequence is UNKNOWN)
- A payment can be entered for a date outside any open financial year; whether `sp_Sales_CustomerPaymentPosting`
  itself guards against this is UNKNOWN from its body (no such check appears in the recovered SQL at
  `docs/database/stored-procedures-part3.md:1586-1925` — it joins `tblFinancialYear` only to resolve
  `@FinancialYearId`, via an `INNER JOIN` that would simply **exclude the row from the cursor** if no financial
  year covers the payment date, silently skipping the journal post while still leaving the `tblCustomerPay`/
  `tblCustPayDetail` rows saved). **This means a payment dated outside any financial year is saved on the
  SalesRoll side but never posted to the GL at all**, with no error surfaced to the user. Escalated from MEDIUM to
  effectively HIGH-impact once the SP body is read — the C# side shows "Data Saved Successfully" regardless
  (`CustomerPayment.aspx.cs`, Stage 6).

## BUG-FIN-008 — No transaction across a multi-invoice receipt; partial failure is silently swallowed

- **Severity:** HIGH **Confidence:** CONFIRMED
- **File:** `Library.BLL/SInventory_BLL/CustPaymentBLL.cs:18-69`
- Each ticked invoice is its own `SaveCustPayment` call with its own `INSERT`/compensating `DELETE` — there is no
  ambient transaction spanning the receipt. `SaveCustPayment` always `return true`s regardless of how many of the N
  invoices actually succeeded (line 60); the compensating delete on a detail-insert failure removes the master but
  leaves any already-inserted sibling detail rows from the **same** master behind (dead orphan rows), and says
  nothing about invoices already fully committed in earlier loop iterations. Combined with `DB_Manager
  .ExecuteNonQueryAction` swallowing exceptions and returning `0` (GAP-APP-008), a partial failure midway through a
  5-invoice receipt is indistinguishable, to the user, from full success.

---

## Duplicate-collection / invoice-paid-twice risk — summary

| Question | Answer | Confidence |
|---|---|---|
| Can a collection be entered with no customer reference? | No screen path found that skips `CustomerMasterId`/`InvoiceId` — every write path requires an invoice/customer key. | CONFIRMED (code) |
| Can the same invoice be paid twice (double-posting)? | Yes in effect: no idempotency check exists on the `CustomerPayment.aspx` insert path (`SaveCustDetail`'s only duplicate guard is `CheckCustomerPaymentExistOrNot`, which is `InvoiceId`+`Amount`+`Date` keyed — a second receipt for a different amount or date sails through), and `DuplicatePaymentList`'s own detector is broken (BUG-FIN-004), so a genuine double-entry is no more likely to be caught than a false positive. | CONFIRMED |
| Partial vs overpayment handling | Partial works (aggregation-based); overpayment is silently accepted and misclassified (BUG-FIN-005). | CONFIRMED |
| Does `PaymentReverse.aspx` reverse stock/ledger/accounting, or just flag status? | Neither, precisely — it does not "flag" status, it deletes the SalesRoll rows outright, but it never reverses the accounting journal (BUG-FIN-003). Stock is not applicable to a payment reversal (no stock movement is tied to `tblCustomerPay`). | CONFIRMED |

## Known gaps carried from `specs/SPEC-GAPS.md`

- `GAP-DB-001` — most stored-procedure bodies remain unavailable; `sp_UD_InvoicePayment`, `sp_DEL_DuplicatePayment`,
  `sp_I_PaymentDeleteLog`, `sp_Save_SaveCustPayment`, `sp_CustomerPaymentPosting` (legacy) were **not** found in
  `docs/database/` during this pass — HUMAN VERIFICATION REQUIRED for each via `sp_helptext`.
- `sp_Sales_CustomerPaymentPosting` and `sp_Sales_CustomerPaymentReversePosting` bodies **were** available in
  `docs/database/stored-procedures-part3.md` and were read in full for this audit.
