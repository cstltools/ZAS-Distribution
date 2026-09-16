# Triggers

**Confirmed: zero triggers exist in this database.** A live query against `sys.triggers` on `SalesrollDB_ZAS`
returned 0 rows. This is a verified fact about the schema, not a gap in extraction coverage.

The previous version of this document treated the existence of triggers as entirely UNKNOWN, reasoning that
"nothing in the C# source... can reveal whether one exists." That reasoning was correct given the constraints at
the time (no DB access), but the conclusion is now settled: there is nothing to reveal, because there is nothing
here. The old doc did not claim any trigger positively exists, so there is no CONFLICTING claim to flag — the
prior text is simply superseded by direct confirmation of the negative.

## What this resolves from the old doc

The old triggers.md raised two specific open questions and left them unresolved pending schema access:

- **`GAP-APP-005` — lost updates on stock.** `InvoiceDAL`/`ChalanDAL` overwrite `tblDCStore.StockQty` directly
  from a C#-computed value with no optimistic-concurrency check. The old doc asked whether an undiscovered
  trigger might mitigate this (e.g. rejecting a too-low value, or logging every write). **Now resolved: no such
  trigger exists.** Nothing in the database enforces or audits this write — if the defect is real, it is fully
  exposed; there is no server-side safety net at all for this table.
- **`GAP-APP-006` — application-generated primary keys race.** `ClsPrimaryKeyFind` computes next-id as
  `MAX(id)+1` outside a transaction, so concurrent inserts can collide. The old doc asked whether a trigger might
  enforce a uniqueness rule beyond a plain primary key. **Now resolved: no such trigger exists.** Whatever
  protects against a collision here is limited to the identity/primary-key constraint on the table itself (which
  would raise a hard error on collision) — there is no trigger-based conflict resolution or retry logic.

More generally: audit trails, cascade deletes, and denormalized-column maintenance (e.g. keeping `tblInvoice`
totals in sync with `tblInvoiceDetail`) are *not* implemented via triggers anywhere in this schema. If any of
that behaviour exists, it is implemented in application code or stored procedures instead (see below) — never
transparently at the row level inside the engine.

## The real mechanism: application-side logging, not triggers

Where this app does audit/history-style bookkeeping, it does it explicitly in C#/DAL code (or in a stored
procedure called from C#), not via a DB trigger. Two concrete examples found by grepping for `Log`/`Audit`/
`History`-named tables being written from `Library.DAL`:

- **`tblLoginLog`** — written directly from C# on every login:
  `Library.DAL\Panal_DAL\PanalDAL.cs:51` and `Library.DAL\PanalCls\PanalClsDAL.cs:29`, both executing
  `INSERT INTO dbo.tblLoginLog (UserId, LoginName, LoginTime) VALUES (...)` with values built in the C# method
  itself, not populated by any trigger on `tblUser`.
- **`tblInvoiceDeleteLog`** — read from (not written from) `Library.DAL\SInventory_DAL\DeleteInvoiceReportDAL.cs`
  to build a "deleted invoices" report. The corresponding write happens via an explicit stored procedure call
  (`sp_ADJ_DeliveryInvoiceDeletePosting`, confirmed to exist in the database's stored-procedure set) invoked from
  the delete-invoice workflow — again, application/procedure-driven, not a trigger fired implicitly by a `DELETE`
  statement against the invoice tables.

Both examples confirm the pattern implied by "zero triggers": whenever this system needs history/audit behavior,
it is wired explicitly into the call path (C# insert, or an explicit stored-procedure call) rather than being an
invisible side effect of a plain `INSERT`/`UPDATE`/`DELETE`. Any code path that skips the log-writing call (or
the procedure) — and there is no way to force it via the schema — will simply produce no audit trail, since
nothing at the database level would catch that omission.
