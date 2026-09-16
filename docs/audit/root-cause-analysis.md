# Root Cause Analysis

Scope: Phase 24. For each major finding: symptom → root cause → database impact → business impact →
financial impact → recommended fix. Only findings with a clear causal chain are included here; the full list
of bugs (including ones without a distinct root-cause narrative beyond "the code does X") is in
`bug-register.md`.

---

## RCA-001 — Stock, invoice and delivery state can diverge from what the user saw

**Symptom** (as it would be reported): "the invoice total doesn't match what was on screen", "stock looks
wrong after an error", "a delivery was confirmed but the customer says nothing arrived".

**Root cause**: No operation that spans more than one table — invoice header + detail + stock (`BR-SYS-004`,
`BUG-TXN-001`), or delivery confirmation + accounting posting (`GAP-APP-018`) — runs inside a database or
application transaction. Every step is a separate autocommitted call
(`Library.DAL/InternalCls/ClsCommonInternalDAL` opens no transaction; confirmed no `TransactionScope` or
cross-call `SqlTransaction` exists anywhere in `Library.BLL`/`Library.DAL` — `transaction-integrity-audit.md`).

**Database impact**: a header row can exist with zero, some, or all of its detail rows; `tblDCStore.StockQty`
can be decremented for lines that never got a matching detail row committed, or vice versa, if any exception is
thrown mid-loop.

**Business impact**: an invoice may show a different total than what was actually delivered/charged; a
delivery may be marked posted to accounting when the confirmation itself failed (`GAP-APP-018` — success and
failure show the identical message).

**Financial impact**: understated or overstated revenue/COGS recognition for the affected document, not
detectable from the UI (no error is shown).

**Recommended fix**: wrap each multi-step save in a single stored procedure call (moving the C# orchestration
into one transactional procedure) or a `TransactionScope` around the existing calls if procedure changes are
out of scope. Check every procedure's return/boolean result before proceeding to the next step, and stop the
chain on failure instead of continuing unconditionally.

**Confidence**: CONFIRMED (the absence of transaction handling and of return-value checks is directly readable
in the C#). The frequency/materiality of actual divergence in production data is UNKNOWN — HUMAN VERIFICATION
REQUIRED against live data.

---

## RCA-002 — Receipt master table integrity broken, though downstream $ totals are not overstated

**Symptom**: "the payment master table has more rows than physical receipts", "an audit of `tblCustomerPay`
doesn't tie to the deposit book."

**Root cause**: `CustomerPayment.aspx.cs::saveButton_Click` calls `CustPaymentBLL.SaveCustPayment` once per
ticked invoice inside a loop, and each call is stamped with the **full** header receipt amount rather than the
amount allocated to that specific invoice (`GAP-APP-017`, `BUG-FIN-002`). One receipt across N invoices
therefore writes N `tblCustomerPay` rows, each carrying the whole receipt value.

**Correction made during this audit**: the collection/ledger deep-dive (`docs/audit/collection-audit.md`)
traced the actual GL posting procedure (`sp_Sales_CustomerPaymentPosting`, body recovered from
`docs/database/stored-procedures-part3.md`) and every due/ledger/collection report, and found they all sum
`tblCustPayDetail.PaymentAmount` — the correctly-allocated per-invoice detail table — **not** the inflated
`tblCustomerPay.PaymentAmount` master column. **GL and aging totals are therefore not overstated.** The real
damage is narrower but still real: `tblCustomerPay` itself — the master receipt record, the thing an auditor
would reconcile against physical receipts/deposit slips — carries N rows per physical receipt, each showing the
full amount. Anyone auditing collections directly from that table (as opposed to the detail table) would be
misled, and the true count of "how many receipts were taken" is unrecoverable from it.

**Database impact**: `tblCustomerPay` row count and per-row amounts do not represent physical receipts 1:1.

**Business impact**: audit trail / reconciliation-from-first-principles is compromised even though the
downstream financial reports happen to sum the correct table.

**Financial impact**: lower than originally assessed — not a reported-balance misstatement, but a data
integrity/audit defect that would fail a receipts-based audit test.

**Recommended fix**: restructure the save so one receipt writes one master row with N allocation-detail rows
each carrying its own allocated amount, not the full receipt amount duplicated N times.

**Confidence**: CONFIRMED (both the code pattern and the corrected downstream impact, re-traced against actual
SP body and report DAL code in `collection-audit.md`).

---

## RCA-003 — Two independent, unreconciled payment-recording pipelines

**Symptom**: "a customer paid but their due amount didn't go down."

**Root cause**: `docs/audit/reconciliation-audit.md` found a second, fully-functional payment pipeline
(`tblPaymentTranscationMaster`/`Detail`, written by the `PaymentTranscation*.aspx` screens — recall
`GAP-STR-003` already flagged three near-duplicate variants of this screen) that is **never read** by any of
the due/ledger/dashboard DALs checked (`CustomerDueReportDAL`, `CustomerWiseLedgerDal`, `CustomerLedgerDAL`,
`SalesDashboardDal.GetDueUpto`) — confirmed by grep across `Library.DAL`.

**Database impact**: two payment tables exist for what appears to be the same business event, with no join or
reconciliation between them found in application code.

**Business impact**: a payment recorded through `PaymentTranscation*` may not reduce the customer's due balance
as shown on standard due/aging reports and the dashboard.

**Financial impact**: potential double-chasing of customers who already paid via the second pipeline, and/or an
inflated total-outstanding figure company-wide.

**Recommended fix**: confirm with the business which pipeline is authoritative (this may be the same "which
duplicated screen is live" open question as `GAP-STR-003`), and either merge the two tables' reporting or
retire the pipeline that is not live.

**Confidence**: SUSPECTED (`BUG-ACC-901` in `reconciliation-audit.md`) — the absence of a join was confirmed by
grep, but whether the two pipelines are actually both live in production, or one is legacy/dead, is UNKNOWN.

---

## RCA-004 — No enforced referential integrity anywhere except one table

**Symptom**: "a child record exists pointing at a parent that isn't there", "deleting X broke Y in a way
nobody could predict."

**Root cause**: the live schema extraction (`docs/database/tables.md`) confirms only **one** physical foreign
key exists in the entire 213-table database. Every other parent-child relationship the application relies on
(invoice→customer, detail→invoice, stock→product, payment→invoice, etc.) is enforced, if at all, only by
application code discipline — and that code already has confirmed gaps (`BUG-TXN-001`, non-atomic saves).

**Database impact**: orphaned child rows are structurally possible anywhere in the schema; nothing at the
database level prevents them.

**Business impact**: any report or process that assumes a foreign key trail is unbroken (e.g. "every invoice
detail has a live invoice header") is trusting application discipline alone, not the database.

**Financial impact**: an orphaned financial detail row (payment allocation, invoice line) would be invisible to
parent-based reports (undercounting) while still consuming/allocating resources (stock, cash) — a silent
mismatch, not a crash.

**Recommended fix**: add foreign key constraints for the highest-value relationships (invoice↔detail,
invoice↔payment, stock movement↔product) at minimum, after first auditing existing data for orphans (adding a
constraint to a table with existing orphans will fail and must be planned around).

**Confidence**: CONFIRMED (the schema fact: 1 physical FK in 213 tables). The consequence (actual orphan rows
existing today) is UNKNOWN without a live query.

---

## RCA-005 — Delegated, unreadable accounting logic

**Symptom**: "we can't tell why the GL doesn't match the sales register."

**Root cause**: every accounting-posting call site found in `Library.BLL`/`Library.DAL` (~20-35 sites, per
`accounting-integration-audit.md`) passes only scalar IDs to an opaque stored procedure — there is no inline
Dr/Cr construction, account-code selection, VAT-split or COGS calculation in C# anywhere except a partial
exception in `PaymentTranscationDal.cs` (which picks Cash vs Bank account and pre-computes AIT/discount, but
still delegates the ledger mechanics). The entire double-entry guarantee, if one exists, lives inside the 119
stored procedures whose bodies are unavailable (`GAP-DB-001`).

**Database impact**: none directly attributable — this is an audit-visibility problem, not a code defect by
itself.

**Business/financial impact**: this audit cannot confirm or deny that debit=credit holds for any posting in
this system. That is not a finding that the system is broken — it is a finding that **the system's correctness
in this area is unverifiable from source control**, which is itself a control weakness (no independently
reviewable accounting logic).

**Recommended fix**: export `sp_helptext` for the ~35 posting procedures listed in
`docs/audit/accounting-integration-audit.md` and `docs/database/stored-procedures.md`; this audit could not do
so itself (blocked by sandbox policy, not by the database — see `system-audit.md` §3) but a human with a
terminal on the same machine that already has live schema access can very likely do this in under an hour.

**Update — largely resolved mid-audit**: `docs/audit/accounting-integration-audit-followup.md` found that 26
of the ~35 posting/deletion procedures already have full bodies sitting in
`docs/database/stored-procedures-part1..4.md`, discovered too late for the original agents to consult
systematically. Reading them confirms real Dr/Cr voucher posting logic keyed off a GL account-mapping table
(`tblSalesIntegrationMapingNew`) — the accounting logic is not arbitrary, it is a consistent, traceable
pattern. But it also confirms the worst-case assumption above: **none of the 26 procedures read use an
explicit transaction, TRY/CATCH, or RAISERROR.** The debit=credit safety net this RCA worried might exist
inside the procedures does not — the absence is now CONFIRMED at the database layer, not just inferred from
its absence in C#.

**Confidence**: CONFIRMED (the absence of inline accounting logic in C#, and now the absence of transactional
safety inside the procedures themselves). The correctness of any single posting's Dr/Cr account selection is
still UNKNOWN without a live balance query — this audit read the *shape* of the logic, not its output.

---

## RCA-006 — Deleting an invoice does not reliably reverse its accounting posting

**Symptom**: "the general ledger has entries for invoices that no longer exist in SalesRoll."

**Root cause**: three separate stored procedures can delete an invoice
(`sp_Deletenvoice`, `sp_DeleteDeliveryInvoice`, `sp_Delete_ProformaInvoice`), plus a fourth,
`sp_DeliveryInvoiceDeletePosting`, that is the *only* one of the four which actually posts a reversing GL
voucher — and it is a separate call a caller must remember to also make. Reading all four bodies
(`accounting-integration-audit-followup.md` §12-16): `sp_Deletenvoice` never touches accounting tables at all;
`sp_DeleteDeliveryInvoice` and `sp_Delete_ProformaInvoice` both contain a GL-cleanup block that is **commented
out** in the procedure source itself — the developers clearly intended to handle this and then disabled it.

**Database impact**: `CSTL_ACCDB`/`CSTL_ERP_ACCDB` voucher tables accumulate entries whose source invoice no
longer exists in `tblInvoice` whenever a delete goes through one of the three non-cleaning paths.

**Business impact**: any reconciliation between the sales register and the GL will show unexplainable phantom
postings that cannot be traced back to a live document.

**Financial impact**: overstated historical revenue/receivables in the GL relative to what SalesRoll can show
as still existing, growing with every invoice deleted through the affected paths.

**Recommended fix**: make every invoice-delete path call `sp_DeliveryInvoiceDeletePosting` (or re-enable the
commented-out cleanup blocks, after verifying they are still correct against the current voucher schema)
before or as part of the delete.

**Confidence**: CONFIRMED — read directly from the procedure bodies, not inferred.
