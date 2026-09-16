# Financial Integrity Audit

Scope: does any code path in this repository (or any confirmed DB object) guarantee that a financial
posting balances (total debits = total credits), that an account code is valid beyond mere existence, or
that money amounts survive rounding/aggregation without drift? Audit-only; no source file was modified.

## 0. Headline finding — read this first

> **Because 0 of 119 stored procedure bodies are available** (`docs/database/stored-procedures.md`,
> confirmed unavailable as of this audit despite live DB schema access existing for other objects), **and
> because the database itself has ZERO triggers and only ONE enforced foreign key** (confirmed via live
> `sys.triggers`/`sys.foreign_keys` extraction in `docs/database/triggers.md` and `docs/database/tables.md`
> — the sole FK is `tblPettyCashDetails.PettyCashmasterId -> tblPettyCashMaster.PettyCashmasterId`), **there
> is NO discoverable code-level or DB-level guarantee that debit = credit anywhere in this system that this
> audit can see.** Any such guarantee, if it exists, is entirely inside an unavailable procedure body.
>
> **HUMAN VERIFICATION REQUIRED:** run `sp_helptext` against the GL-posting procedures listed in
> `docs/database/stored-procedures.md` (section I) and the posting procedures in sections A/B — notably
> `sp_Sales_ExecuteDeliveryPipeline`, `sp_ProformaPosting`, `sp_ProformaFullReturnPosting`,
> `sp_Sales_DeliverySalesPosting`, `sp_UD_InvoicePayment` — and manually trace whether they insert matching
> debit/credit rows inside a single transaction with a balance check. This document does not, and cannot,
> answer that question. Nothing below should be read as evidence that such a check exists or does not exist
> inside those bodies — only that it does not exist anywhere *outside* them.

## 1. Search for client-side balance/double-entry enforcement — result: NONE FOUND (CONFIRMED)

Grepped the full C# tree (`Library.BLL/**/*.cs`, `Library.DAL/**/*.cs`, `Solution.Web/**/*.cs`) for
`Balance`, `Dr ==`, `Cr ==`, `TotalDebit`, `TotalCredit`, `IsBalanced`, `Reconcil`. No production code hit on
any of these terms in a debit/credit-comparison sense. The only two "financial precondition" calls that
exist anywhere in the app are the ones already named in `specs/00-system/integrations.md`:

### `InvoiceDAL.CheckChartOfAccount` — existence check only (CONFIRMED)

`Library.DAL/SInventory_DAL/InvoiceDAL.cs:1678-1684`:

```csharp
public DataTable CheckChartOfAccount(string customerCode)
{
    string query = @"SELECT AccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE CustomerId IS NOT NULL AND CustomerId IN (
                     SELECT CustomerMasterId FROM dbo.tblCustMaster WHERE CustomerCode = '" + customerCode + "')";
    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
}
```

This does exactly one thing: confirms a row exists in `ZAS_ACCDB..tblChartOfAccounts` linking the customer
to *some* `AccountId`. It does **not** check that the account is active, that it is the correct type
(AR vs. revenue vs. tax), that its balance is valid, or anything about debits/credits. Callers (e.g.
`Solution.Web/SInventory_UI/CustomerPayment.aspx.cs:216`) treat an empty result as "customer has no chart of
accounts row" and block the save — see `WF-customer-payment.md:47`. It is a **referential existence guard**,
not a financial validity or balance check. CONFIRMED by reading the method body directly.

### `InvoiceDAL.GetFinancialYearInfo` — period-open check only (CONFIRMED)

`Library.DAL/SInventory_DAL/InvoiceDAL.cs:1643-1651`:

```csharp
public DataTable GetFinancialYearInfo(string invoiceDate, string unitId)
{
    string query = @"SELECT DISTINCT FinancialYearId FROM dbo.tblCompanyUnit AS UNT
                     INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
                     INNER JOIN CSTL_ACCDB..tblFinancialYear ON tblFinancialYear.CompanyId = CI.CompanyId
                     WHERE UNT.ComUnitId = " + unitId + " AND Status IN ('Active') AND '" + invoiceDate + "' BETWEEN StartDate AND EndDate";
    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
}
```

This confirms the invoice date falls inside an `Active` financial-year period row for the unit's company.
It says nothing about whether any posting within that period balances. It is a **period-window guard**, not
an accounting-balance check. CONFIRMED by reading the method body directly.

**Conclusion for section 1 (CONFIRMED):** across the entire client/BLL/DAL layer, the only two "accounting"
preconditions that exist are (a) does a chart-of-accounts row exist for this customer, and (b) is this date
inside an open financial year. Neither is a debit/credit balance check, and no other candidate code path
(searched via `TotalDebit`/`TotalCredit`/`IsBalanced`/`Dr ==`/`Cr ==`/`Reconcil`/`Balance`) exists anywhere
in the C# source.

## 2. Rounding around money fields (CONFIRMED, no downstream correction found)

`Solution.Web/SInventory_UI/ProductDiscountEntry.aspx.cs:340,346`:

```csharp
discountPerTextBox.Text = System.Math.Round(percentage, 2).ToString(CultureInfo.InvariantCulture);
...
discountAmountTextBox.Text = System.Math.Round(amount, 2).ToString(CultureInfo.InvariantCulture);
```

Percentage and amount are rounded to 2 dp **independently** — rounding the percentage first and the amount
separately from the (unrounded) base means the two displayed/stored numbers are not derived from a single
rounding pass, so `amount ÷ base × 100` need not reproduce the stored percentage exactly. No code anywhere
reconciles the two afterward. This is the only `Math.Round` usage found directly adjacent to
invoice/discount money math in the searched files; other money fields (`InvoiceBLL`, `CustPaymentBLL`,
`CustPaymentDAL`) do arithmetic on unrounded `decimal` values and rely on SQL column precision/scale for
final rounding — that column precision is itself UNKNOWN because `docs/database/tables.md` was not re-read
column-by-column in this pass (schema for `tblInvoice`/`tblCustomerPay` money columns should be checked by a
human against the DDL for scale mismatches). No evidence of a rounding-caused imbalance was found, but no
evidence of a rounding-imbalance *guard* was found either — this is an UNKNOWN, not a clean bill of health.

## 3. Re-verification of known integrity-relevant bugs, connected to financial-integrity consequences

### GAP-APP-005 — Lost updates on stock (CONFIRMED, re-verified)

`Library.DAL/SInventory_DAL/InvoiceDAL.cs:801`:

```csharp
string updateQuery = @"UPDATE tblDCStore SET StockQty='" + Quantity + "' WHERE DCStoreId='" + dCStoreId.Trim() + "'  ";
```

`Quantity` is a value computed in C# (read-then-write with no optimistic-concurrency token, e.g. no
`WHERE ... AND StockQty = @originalStockQty`). Other call sites do the same overwrite pattern:
`Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:113`, `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:97`,
`Library.DAL/SInventory_DAL/ChalanDAL.cs:332`. (Two sites, `CustomerReturnDAL.cs:108` and
`DestroyProductDal.cs:179`, instead do `SET StockQty = StockQty + @qty` / parameterized, which is safe from
lost updates — the vulnerability is not universal, but it is present on the primary invoice-issue path.)
**Financial-integrity consequence:** stock quantity feeds cost-of-goods-sold and inventory valuation
indirectly through whatever the (unavailable) posting procedures read. A lost update under concurrent
issues/receipts silently corrupts the stock-on-hand figure that a GL inventory valuation is presumably
reconciled against, with no error raised and no trigger to catch the drift (confirmed zero triggers).

### GAP-APP-006 — Application-generated PKs race (CONFIRMED, re-verified)

`Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:18,35`:

```csharp
string query = @"SELECT (isnull(MAX(" + columnName + "),0)+1) as PKMaxNo FROM " + tableName;
```

Read outside any transaction, then used by a caller to construct an insert. Concurrent callers can read the
same MAX and collide or silently overwrite/duplicate a key. **Financial-integrity consequence:** if this
pattern generates document/voucher numbers or ledger-row keys used downstream in reconciliation or
duplicate-detection, a race here can produce duplicate or skipped document numbers with no DB constraint to
catch it — confirmed only one enforced FK exists in the whole database, so nothing stops a duplicate ID from
being inserted into most tables.

### GAP-APP-007 — Document saves are not transactional (CONFIRMED, re-verified)

`Solution.Web/SInventory_UI/CustomerPayment.aspx.cs:334-445` (`saveButton_Click`): the loop over
`orderGridView.Rows` calls, per ticked row and independently: `InvoiceBLL.SaveCreditAdjustmentInfo` (line
386, conditionally), `CustPaymentBLL.UpdateInvoicePaymentAmount` (lines 411/418), and
`CustPaymentBLL.SaveCustPayment` (line 443) — three separate round-trips per row, each its own
implicit-transaction stored-procedure call, with **no ambient `TransactionScope`/`SqlTransaction` wrapping
the whole loop or even a single row's three calls**. `DB_Manager.cs` (see GAP-APP-008 below) does hold a
`dbTransaction` field per `ExecuteNonQueryAction` call, but each `UpdateAction`/`SaveAction` call opens/uses
that independently — there is no evidence in the reviewed code of one transaction spanning multiple stored
procedure calls. **Financial-integrity consequence:** if the page throws, times out, or the process dies
between `UpdateInvoicePaymentAmount` and `SaveCustPayment` for a given row, the invoice can be marked
paid/partial with no corresponding `tblCustomerPay` detail row (or vice versa) — an unbalanced state between
the invoice header and the payment ledger that nothing detects, since there are no triggers and no
reconciliation job found in this codebase.

### GAP-APP-008 — `DB_Manager` swallows exceptions (CONFIRMED, re-verified)

`Library.DAL/MAIN_FUNCTION/DB_Manager.cs:50-116` (`ExecuteNonQueryAction`, `UpdateAction`, `SaveAction`):
every one of these wraps the actual `database.ExecuteNonQuery(...)` call in a `try/catch (Exception ex)`
that sets `ActionStatus = false` (or `pk = 0`) and **does not rethrow, does not log inline, and returns a
value indistinguishable from "the procedure ran and legitimately matched zero rows."** Confirmed at lines
109-116 (`UpdateAction`) and 119-132 (`SaveAction`). **Financial-integrity consequence:** if a GL-posting
stored procedure throws mid-transaction (e.g. a constraint violation, deadlock, or an internal balance check
failing inside the procedure — which per section 0 above we cannot see), the C# caller receives `false`/`0`
and, per GAP-APP-018, several callers show a success message regardless or fail to branch on the return
value at all. A failed posting can therefore look identical to a successful no-op to both the code and the
user, with no distinguishing signal to reconcile against later.

### GAP-APP-016 — Payment amount/status discarded (CONFIRMED, re-verified)

`Library.DAL/SInventory_DAL/CustPaymentDAL.cs:177-188`:

```csharp
public bool UpdateInvoicePaymentAmount(string amount,string status,string id)
{
    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
    aSqlParameterlist.Add(new SqlParameter("@InvoiceId", id));
    return aCommonInternalDal.UpdateAction("sp_UD_InvoicePayment", aSqlParameterlist);
    //string query = @"UPDATE dbo.tblInvoice SET PaymentAmount='" + amount + "',PaymentStatus='"+status+"' WHERE InvoiceId='" + id + "'";
    //return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
}
```

Only `@InvoiceId` is passed to `sp_UD_InvoicePayment`; `amount` and `status` (computed at
`CustomerPayment.aspx.cs:395,406-420` from the screen's Full/Partial logic) are silently dropped — the
commented-out direct-UPDATE line confirms this used to write them and no longer does. **Financial-integrity
consequence:** whatever `sp_UD_InvoicePayment` actually does to `tblInvoice.PaymentAmount`/`PaymentStatus` is
entirely procedure-internal and UNKNOWN (per section 0); the C# layer's own Full/Partial and cumulative-paid
calculation is provably dead code, so any spec or report that assumes the UI's payment-status logic reflects
what's stored is assuming something this repository cannot confirm.

### GAP-APP-017 — One receipt across N invoices creates N full-amount payment rows (CONFIRMED, re-verified)

`Solution.Web/SInventory_UI/CustomerPayment.aspx.cs:334-443`: inside the `for` loop over grid rows, for each
ticked row a **new** `CustPayment aCustPayment` is constructed (line 350) but its `PaymentAmount` field is
set at line 395 to `Convert.ToDecimal(paymentAmountTextBox.Text)` — **the single header-level receipt amount
textbox, not a per-row/per-invoice amount** (the per-row amount is `payAmountTextBox`, used only for the
`CustPaymentDetail.PaymentAmount` at line 427, and for the Full/Partial comparison, but never assigned to
`aCustPayment.PaymentAmount`). `CustPaymentBLL.SaveCustPayment(aCustPayment, aCustPaymentDetails)` is then
called once per ticked row (line 443), each time inserting a new `tblCustomerPay` master row (per
`specs/finance-collection/workflows/WF-customer-payment.md`) stamped with that same full header amount.
**Financial-integrity consequence:** AR aging, cash-collected, and collections-summary reports that sum
`tblCustomerPay.PaymentAmount` will overstate cash collected by a factor of N whenever one receipt is applied
across N invoices in a single save — this is a direct double/N-counting risk on any downstream financial
report, not merely a data-modeling oddity.

## 4. New financial-integrity-specific findings

### BUG-ACC-030 — No application-layer control exists to prevent an unbalanced posting from being requested · CRITICAL · CONFIRMED (absence), UNKNOWN (procedure-side behavior)

Every "Save"/"Post" action that results in a financial posting (invoice save, delivery-confirmation posting
via `sp_Sales_ExecuteDeliveryPipeline`, proforma posting via `sp_ProformaPosting`/`sp_ProformaFullReturnPosting`,
payment posting via `sp_UD_InvoicePayment`) hands the request straight to a stored procedure with only an
`@InvoiceId`/similar key parameter (see `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1653-1693`). No amount, no
account list, and no debit/credit breakdown is constructed or validated in C# before that call — there is
nothing in the client tier that *could* reject an unbalanced posting even in principle, because the client
tier never computes or sees the debit/credit lines at all; it exists only inside the procedure. Combined with
zero DB triggers and effectively no enforced FKs (section 0), this means the *only* place a debit=credit
invariant could be enforced, if it is enforced anywhere, is inside procedure logic this audit cannot read.
This is worth recording as its own finding distinct from GAP-DB-001 because it establishes that the absence
of a visible guarantee is not merely a documentation gap — it is a structural fact about where control could
possibly live in this architecture. CONFIRMED: no such control exists in any reachable C#/BLL/DAL code.
UNKNOWN: whether the procedures compensate for this.

### BUG-ACC-031 — Rounding of percentage and amount is not derived from a single computation · MEDIUM · CONFIRMED (as coded), UNKNOWN (financial impact)

`Solution.Web/SInventory_UI/ProductDiscountEntry.aspx.cs:340,346` rounds a discount percentage and a discount
amount independently to 2 dp from what appear to be separately-computed inputs, with no reconciliation step
verifying `Round(amount,2) == Round(base * Round(percentage,2) / 100, 2)`. Given GAP-APP-002 (discount
shown ≠ discount stored: per-unit vs. per-line), this rounding independence compounds an existing
inconsistency rather than being a new root cause. Severity kept at MEDIUM because no evidence of an actual
mismatched total reaching a ledger was found — only that no downstream check would catch one if it occurred.

### BUG-ACC-032 — Multi-call payment save sequence has no transactional or compensating-action boundary · HIGH · CONFIRMED

Restated from GAP-APP-007 with the specific financial framing: `CustomerPayment.aspx.cs` performs up to three
independent DB round trips per invoice row (`SaveCreditAdjustmentInfo`, `UpdateInvoicePaymentAmount`,
`SaveCustPayment`) with no shared transaction and no compensating rollback if a later call in the sequence
fails after an earlier one succeeded (see section 3, GAP-APP-007 above for line numbers). Filed separately
here because GAP-APP-007 was originally recorded as a generic "not transactional" finding; this entry
records its specific financial-integrity shape (partial payment-application state) for this audit's purposes.

## 5. What this audit did NOT find (explicitly, to avoid false comfort)

- No code path anywhere sums debit and credit sides of a transaction and compares them.
- No code path validates an account code's type, status, or balance beyond "does a chart-of-accounts row
  exist for this customer" (`CheckChartOfAccount`) — UNKNOWN whether the procedures do more.
- No reconciliation job, scheduled task, or admin report that cross-checks `tblCustomerPay` totals against
  `tblInvoice` payment status was found in the searched trees; its existence is UNKNOWN, not ruled out —
  Crystal Reports (`.rpt`, binary, per GAP-RPT-001) were not opened as part of this audit and could contain
  such a check as a report-level formula, which this audit cannot see either.
- Whether `sp_UD_InvoicePayment`, `sp_Sales_ExecuteDeliveryPipeline`, `sp_ProformaPosting`, and
  `sp_ProformaFullReturnPosting` internally maintain double-entry balance is **UNKNOWN** and requires
  `sp_helptext` extraction — this is the single most important open item from this audit.

## Confidence legend

- **CONFIRMED** — read directly from the cited file:line in this pass.
- **INFERRED** — reasonably follows from confirmed facts but not itself directly observed.
- **SUSPECTED** — plausible based on pattern but not verified.
- **UNKNOWN / HUMAN VERIFICATION REQUIRED** — cannot be determined from available artifacts; explicitly not guessed.
