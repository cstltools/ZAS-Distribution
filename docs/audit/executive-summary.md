# Executive Summary

**System audited**: Salesroll — the ZAS distribution/ERP application (ASP.NET Web Forms, .NET Framework 4.0,
SQL Server, 383 pages, ~600 stored procedures/functions referenced). Audit-only exercise: no source code,
database, or data was modified.

## What this audit found, in one paragraph

The application's core order-to-cash and stock logic works for the common case, but has no transactional
safety net anywhere — not in the C# application layer, and (confirmed by reading 26 actual stored procedure
bodies during this audit) not inside the database procedures either. Combined with a near-total absence of
enforced database constraints (1 physical foreign key in 213 tables) and several specific defects in the
highest-value financial screens, the result is a system where a failure partway through almost any multi-step
operation — an invoice, a stock transfer, a payment, a return — can leave data in a state nothing else in the
system will catch or reconcile. Two findings stand out as immediate, ongoing financial risk rather than
theoretical exposure: payment reversal never undoes the original accounting entry, and two of three
invoice-delete paths orphan posted GL vouchers. Separately, this remains one of the least security-hardened
systems this kind of audit typically encounters: hard-coded live database credentials in source control,
systemic SQL injection including the login query, and plaintext passwords are all still present and were
independently re-verified during this pass.

## Numbers

| | |
|---|---|
| Total findings (bugs + carried-forward gaps) | ~110, spanning `bug-register.md` |
| CRITICAL severity | 17 |
| HIGH severity | 45 (one downgraded on closer inspection — see below) |
| MEDIUM / LOW | 40 / 9 |
| Modules touched by the two biggest root causes (non-atomic saves; no accounting transactions) | effectively all of them — see `cross-module-impact.md` |
| Stored procedure bodies recovered mid-audit | 26 of 27 highest-priority ones, from files already in the repository (`docs/database/stored-procedures-part1..4.md`) that the original documentation baseline did not know to check |

## The five findings leadership should see first

1. **Payment reversal doesn't reverse the accounting.** `PaymentReverse.aspx` deletes the local record but
   never calls the procedure that reverses the GL posting. Every use permanently misstates cash/bank and
   accounts receivable, with no approval gate. (`BUG-FIN-003`, CRITICAL)
2. **Deleting an invoice can leave its GL entry behind.** Two of three delete procedures never clean up the
   posted voucher — confirmed by reading the actual procedure code, including one place where the cleanup
   code exists but was commented out. (`BUG-ACC-902`, CRITICAL)
3. **No stock-availability check before invoicing.** An order for more than what's in stock is still invoiced
   in full, silently, with no error. (`BUG-STOCK-001`, CRITICAL)
4. **A discount can be silently dropped on a live customer-invoice screen**, causing the persisted invoice to
   be higher than what the customer was shown. (`BUG-SALES-001`, CRITICAL)
5. **Credit limits are not enforced at the point goods actually leave the warehouse**, only optionally at order
   entry. (`BUG-FIN-010`, CRITICAL)

## What was corrected during this audit (worth knowing about the audit itself)

- An earlier concern that one receipt paying multiple invoices would overstate collected-cash totals in
  reports and the GL was **downgraded** after tracing the actual posting procedure: the correct, allocated
  amounts are what get posted and reported. The remaining issue is a data-integrity defect in the receipt
  table itself, not a financial misstatement.
- A concern about a discarded payment-amount parameter turned out to be dead code, not a live bug — the
  relevant procedure recomputes the correct value from source tables regardless of what it's sent.
- Both database schema and the majority of stored-procedure logic — originally believed entirely undocumentable
  in this codebase — turned out to already be recovered elsewhere in the repository, just not cross-referenced
  by the documentation that named this audit's starting scope. This audit found and used those files.

## What still cannot be verified without further access

Live data (no query access in this session), Crystal report internals (binary format), the full menu/permission
grant data (needed to know exactly who can reach which screen), and one specific stored procedure
(`sp_DeleteDeliveryInvoice_FullRollback`). See `audit-coverage.md` and `audit-evidence.md` for the complete
list and what would close each gap.

## Where to go next

- **For the full prioritized action list**: `remediation-plan.md`.
- **For every CRITICAL finding in full detail**: `critical-bugs.md`.
- **For the complete bug index**: `bug-register.md`.
- **For how confident to be in any specific claim**: `audit-evidence.md`.
