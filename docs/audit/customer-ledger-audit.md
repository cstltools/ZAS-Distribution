# Customer Ledger / Credit Limit Audit

Scope: customer ledger balance / outstanding computation, credit-limit entry/approval and whether it is enforced,
and aging-report correctness given `GAP-APP-017`. Audit-only — no source or data was modified. Line numbers
verified by direct file reads on 2026-09-05.

## Outstanding / due computation — is it correct given GAP-APP-017?

**Finding: yes, the aging/outstanding reports are arithmetically correct despite GAP-APP-017.** Every due/aging
query found sums `tblCustPayDetail.PaymentAmount` (the properly allocated per-invoice amount), never
`tblCustomerPay.PaymentAmount` (the per-master, inflated-by-N header amount):

- `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs:93-120` (`LoadCustomerDueReport`, backs `PAGE-FIN-007
  CustomerDueReport`) — `DueAmount = DeliveryTpGrandTotal - (SUM(PMNT.PaymentAmount)+AIT+Discount) - ReturnAmount`,
  where `PMNT` is `SELECT InvoiceId, SUM(CPD.PaymentAmount) ... FROM tblCustPayDetail AS CPD LEFT JOIN
  tblCustomerPay AS CP ... GROUP BY InvoiceId` (line 105-107).
- `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs:126-147` (`LoadCustomerTotalDueReport`, backs `PAGE-FIN-027
  TotalCustomerDueReport`) — identical pattern (line 138-140), aggregated per customer.
- `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs:111` — same `SUM(PD.PaymentAmount)` from
  `tblCustPayDetail`.

**Confidence: CONFIRMED** by direct read of both files.

## `CustomerLedgerReport` is not a ledger

- **File:** `Library.DAL/SInventory_DAL/CustomerLedgerDAL.cs:15-23`
- **Severity:** MEDIUM **Confidence:** CONFIRMED

The entire query behind `PAGE-FIN-008 CustomerLedgerReport`:

```sql
SELECT M.CustomerCode, M.CustomerName, I.OrderNo, I.OrderDate, I.InvoiceNo, I.InvoiceDate,
       M.ComUnitCode, M.ComUnitName, M.MarketCode, M.MarketName,
       I.TpGrandTotal, I.DeliveryTpGrandTotal, I.DelivaryInvoiceNo, I.UpdateDate,
       I.PaymentAmount, I.PaymentStatus
FROM dbo.tblInvoice I
INNER JOIN dbo.View_CustomerMaster M ON I.CustomerMasterId = M.CustomerMasterId
WHERE M.CustomerCode = '<CustomerID>'
```

This is a flat per-invoice list read from `tblInvoice.PaymentAmount` — the **cached rollup column** that
`sp_Sales_CustomerPaymentPosting` writes (`UPDATE INV SET PaymentAmount = AGG.PaymentAmount ...`,
`docs/database/stored-procedures-part3.md:1609-1625`), not a transaction-by-transaction ledger of debits (invoices)
and credits (each receipt, credit note, discount, AIT). There is:

- no opening-balance carry-forward,
- no running balance column,
- no line for each individual payment/receipt (only the invoice's current cumulative `PaymentAmount`),
- no inclusion of `tblCreditAdjustment` (credit notes) rows at all.

A user calling this "the customer ledger" is looking at a due-list with the invoice's net paid-to-date, not an
auditable ledger. This is a naming/functionality gap, not a numeric-correctness bug (the underlying `PaymentAmount`
column it reads is written correctly per invoice by the posting SP), but it means **no screen in this module
reconstructs a true running-balance ledger** — reconciling "why is this customer's balance X" requires manually
joining `tblCustPayDetail`+`tblCreditAdjustment`+`tblInvoice`, which no report does today.

## BUG-FIN-009 — Credit-limit enforcement exists on exactly one screen, and is self-overridable

- **Severity:** HIGH **Confidence:** CONFIRMED
- **File:** `Solution.Web/SInventory_UI/ManualOrderrCreation.aspx.cs:1186-1196` (the live, non-`.exclude`d order
  screen — see `GAP-STR-006`)

```csharp
if (Convert.ToDecimal(previousDueLabel.Text.Trim()) > 0 && isSpecialAppCheckBox.Checked == false)
{ ShowMessageBox("Customer has previous due. Special Approval required !!!"); return false; }

if (Convert.ToDecimal(remainingcrLabel.Text) < 0 && paymentHiddenField.Value != "Cash" && isSpecialAppCheckBox.Checked == false)
{ ShowMessageBox("Credit Limit Crossed !!!"); return false; }
```

This is the **only** blocking credit-limit check found anywhere in the codebase (`InvoiceCreationByOrder.aspx.cs`
and `InvoiceCreationForCustomerByOrder.aspx.cs` — the two invoice-creation screens that actually book AR exposure —
contain **zero** references to `CreditLimit`, confirmed by grep across `Solution.Web`). The block is lifted the
instant `isSpecialAppCheckBox` is checked (line 1024: `aListMasterDao.IsSpecialApproval =
isSpecialAppCheckBox.Checked`) — a checkbox on the **same page**, filled in and submitted by the **same user**
creating the order, with no second-person/approver sign-off gate at save time. `IsSpecialApproval` is persisted as
a flag on the order for later reporting, but nothing in this code path routes the order to
`CustomerCreditLimitApproval.aspx` or blocks it pending approval before it can be saved. **Credit-limit enforcement
is therefore advisory, not preventive**, wherever it exists at all.

## BUG-FIN-010 — Credit limit is not checked at invoice creation, only (optionally) at order creation

- **Severity:** CRITICAL **Confidence:** CONFIRMED (absence)

Grep for `CreditLimit` across `Solution.Web` returns 13 files; the only ones performing an actual limit *check*
(not just a report/entry/approval/view screen) are `ManualOrderrCreation.aspx.cs`,
`SampleProductrIssue.aspx.cs`, and `SlaveInfoApproval.aspx.cs`. **Neither `InvoiceCreationByOrder.aspx.cs` nor
`InvoiceCreationForCustomerByOrder.aspx.cs`** (both were modified in the working tree per the initial `git status`,
and both are on the module's finding-list from `specs/SPEC-GAPS.md`, `GAP-APP-002`/`GAP-APP-020`) reference credit
limit at all. If any order can reach invoicing without having passed through `ManualOrderrCreation.aspx` (e.g. an
order created via a different screen, or an invoice created directly against a customer without a prior order —
`InvoiceCreationForCustomerByOrder.aspx` suggests a direct-invoice path exists), that invoice is raised with **no
credit-limit check whatsoever**, blocking or advisory. HUMAN VERIFICATION REQUIRED to enumerate every order/invoice
entry path against the live menu (`tblMainMenu`/`tblMenuDistribution`, `GAP-SEC-001`) to know how many of them
bypass `ManualOrderrCreation.aspx` entirely.

## BUG-FIN-011 — Credit-limit lookup ignores approval status and row selection is unordered

- **Severity:** MEDIUM-HIGH **Confidence:** CONFIRMED
- **Files:** `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs:117-127` (`GetCustomerCreditLimit`), identical
  copies in `Library.DAL/SInventory_DAL/OrderListDAL.cs:140-149` and `Library.DAL/SInventory_DAL/SampleIssueDal.cs:117-127`

```sql
SELECT * FROM dbo.tblCustomerCreditLimit
LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
WHERE tblCustMaster.CustomerMasterId = '<customerId>'
```

No `WHERE ActionStatus = 'Approved'`, no `IsActive = 1` filter, and no `ORDER BY` — yet the caller
(`ManualOrderrCreation.aspx.cs:158-161`) unconditionally reads `dtcrlimit.Rows[0]["LimitAmount"]`. Cross-checked
against `Library.DAL/SInventory_DAL/CreditLimitDAL.cs`, a credit-limit row's lifecycle is
`INSERT ... ActionStatus` (initial, likely `'Posted'`) → `CustomerCreditLimitApproval.aspx` flips it via
`UPDATE tblCustomerCreditLimit SET ActionStatus=@ActionStatus, ApprovedBy=..., ApprovedDate=...`
(`CreditLimitDAL.cs:44`). Because the *read* side never filters on this column:

1. An entered-but-not-yet-approved limit is used to gate orders identically to an approved one — the approval step
   has no effect on enforcement.
2. If a customer has more than one `tblCustomerCreditLimit` row (a revision history, or a rejected-then-resubmitted
   entry), which row lands in `Rows[0]` is whatever order SQL Server's query plan happens to return — not
   guaranteed to be "the current approved limit."

HUMAN VERIFICATION REQUIRED — needs the DDL for `tblCustomerCreditLimit` (`GAP-DB-002`) to confirm whether a
uniqueness/one-active-row constraint exists at the database level that would make this moot; nothing in the C# or
recovered SQL enforces it.

## BUG-FIN-012 (shared with collection audit, BUG-FIN-005) — Overpayment is not blocked, and skews `PaymentStatus`

Repeated here because it directly affects aging-report correctness: `sp_Sales_CustomerPaymentPosting`/`...
ReversePosting`'s status recompute (`docs/database/stored-procedures-part3.md:2296-2300`) sets `PaymentStatus =
'Full'` **only** on exact equality to `DeliveryTpGrandTotal`; an overpaid invoice is left `'Partial'` forever.
The due-amount **arithmetic** used by `CustomerDueReportDAL`/`CustomerLedgerDAL` is computed independently
(`DeliveryTpGrandTotal - PaymentAmount - AIT - Discount`) and would correctly show a negative number (a credit) for
an overpaid invoice rather than hiding it — so the *aging total* is not wrong, but any downstream logic that reads
`PaymentStatus` as a proxy for "still open" (rather than recomputing the arithmetic) would incorrectly continue to
treat an overpaid invoice as open/due. **Confidence: CONFIRMED** for the exact-equality status rule;
**SUSPECTED/UNKNOWN** for which additional screens (outside this module) branch on the status flag.

## BUG-FIN-013 — `PaymentReverse.aspx` leaves stale `AIT`/`DiscountOnPayment` on the invoice (cross-ref BUG-FIN-003)

- **Severity:** MEDIUM **Confidence:** CONFIRMED
- **File:** `Library.DAL/SInventory_DAL/PaymentReverseDal.cs:23-27`

`ResetInvoicePaymentStatus` only nulls `PaymentStatus` and `PaymentAmount`:

```sql
UPDATE dbo.tblInvoice SET PaymentStatus = NULL, PaymentAmount = NULL WHERE InvoiceId = <id>
```

It does not reset `AIT` or `DiscountOnPayment`, both of which are also cached rollup columns written by the
posting SP and both of which several due/aging queries add back into the outstanding calculation (e.g.
`CustomerDueReportDAL.cs:95`: `DeliveryTpGrandTotal - (PaymentAmount + AIT + Discount) - ReturnAmount`, when a
query path reads the cached `tblInvoice` columns instead of re-aggregating `tblCustPayDetail`). After a reversal
through this screen, a reader of the cached columns would still subtract the stale AIT/Discount, understating the
customer's true outstanding balance by that amount. The due-report queries actually verified in this audit
(`CustomerDueReportDAL`) re-aggregate from `tblCustPayDetail` (which the reversal does correctly delete), so they
are **not** affected — this risk applies only to any other, unverified code path that trusts the cached
`tblInvoice.AIT`/`DiscountOnPayment` columns directly. **SUSPECTED** scope; flagged for completeness.

---

## Bug list summary

| ID | Title | Severity | Confidence |
|---|---|---|---|
| BUG-FIN-001 | `UpdateInvoicePaymentAmount` discards amount/status (GAP-APP-016) | HIGH | CONFIRMED |
| BUG-FIN-002 | One receipt → N `tblCustomerPay` masters (GAP-APP-017); GL/reports NOT overstated | HIGH (data-integrity, not $-overstatement) | CONFIRMED |
| BUG-FIN-003 | `PaymentReverse.aspx` never reverses the GL journal | CRITICAL | CONFIRMED |
| BUG-FIN-004 | `DuplicatePaymentList` flags near-all `CustomerPayment.aspx` rows as false-positive duplicates | CRITICAL (data-loss via "fix") | CONFIRMED mechanism / SUSPECTED scale |
| BUG-FIN-005 | Overpayment/over-application never validated | HIGH | CONFIRMED |
| BUG-FIN-006 | Payment-type/credit-adjustment routed by display text / magic literal `"2"` | MEDIUM | CONFIRMED |
| BUG-FIN-007 | Financial-year guard disabled → payment saved but silently never posted to GL | HIGH | CONFIRMED |
| BUG-FIN-008 | No transaction across multi-invoice receipt; partial failure hidden | HIGH | CONFIRMED |
| BUG-FIN-009 | Credit-limit block is self-overridable by the same user (no approver gate) | HIGH | CONFIRMED |
| BUG-FIN-010 | Credit limit never checked at invoice creation, only optionally at order creation | CRITICAL | CONFIRMED (absence) |
| BUG-FIN-011 | Credit-limit lookup ignores `ActionStatus`, unordered row pick | MEDIUM-HIGH | CONFIRMED |
| BUG-FIN-012 | Overpaid invoices misclassified `'Partial'` forever | MEDIUM | CONFIRMED (rule) / SUSPECTED (downstream use) |
| BUG-FIN-013 | Reversal leaves stale `AIT`/`DiscountOnPayment` on invoice | MEDIUM | CONFIRMED (code) / SUSPECTED (report exposure) |

## Known gaps

- `GAP-DB-002` (no DDL) blocks confirming any DB-level constraint on `tblCustomerCreditLimit` uniqueness (BUG-FIN-011).
- `sp_UD_InvoicePayment`, `sp_DEL_DuplicatePayment` bodies remain unavailable — HUMAN VERIFICATION REQUIRED.
- Menu/permission data (`GAP-SEC-001`) blocks confirming which order/invoice screens are actually reachable in
  production, needed to close BUG-FIN-010's blast radius.
