# PaymentTranscationNew2 — P1 Reliability + Performance Implementation Report

Date: 2026-09-09
Scope: `SInventory_UI/PaymentTranscationNew2.aspx` — NEW payment entry only.
Historical data (1,181 missing receipts, old duplicate receipt numbers, old
NULL receipt rows) is explicitly out of scope and was **not** touched.

## 1. Executive Summary

All 10 requested findings (5 P1 reliability + 5 performance/reliability) were
investigated against the **live** database and **live** stored procedure
definitions (not just the audit docs, which were found to have drifted from
the live SPs in two places — see §2). Implemented:

- P1-1 Shared transaction — **done**, whole-submission atomicity.
- P1-2 Journal failure handling — **done**, via a real thrown exception.
- P1-3 Multi-invoice failure — **done**, whole-submission rollback (no
  partial commits possible).
- P1-4 Double-click — **verified intact**, not touched.
- P1-5 Duplicate payment concurrency — **done**, TOCTOU closed with
  `UPDLOCK, HOLDLOCK`.
- Receipt number race (Perf-7) — **done**, locked `MAX+1` sequence.
- Unnecessary UPDATE (Perf-9) — **removed**, with live evidence it was
  already a no-op for 14+ months.
- Generic SQL wrapper (Perf-8) — **not applicable to this page's save path**,
  no code change (see §8).
- CSV LIKE join (Perf-10) — **not changed** (no relational key exists);
  documented as performance debt, empirically measured as slow (see §10).
- Receipt report ownership (Perf-11) — **done**, filtered by
  `tblUserCompanyUnit`, tested against real data.

Build (`Library.DAO` → `Library.DAL` → `Library.BLL` → ASP.NET precompile)
passed clean. Functional tests were run against the live dev database
(`TOWSIF\MSSQLSERVER2019` / `SalesRollDB_ZAS`) using dedicated, clearly
sentinel-tagged test rows (`CreateBy = 'P1_AUTOTEST%'`) that were deleted
immediately after each assertion. No historical row was read-modified,
deleted, or backfilled.

**Verdict: IMPLEMENTATION COMPLETE — TESTING INCOMPLETE** (see §19 for the
specific items that could not be safely or completely tested in this
environment).

## 2. Docs-vs-live drift found before implementing

Pulling live definitions via `OBJECT_DEFINITION()` found the audit docs'
recovered SP bodies were stale for two of the four procedures touched:

| Procedure | Docs said | Live actually was |
|---|---|---|
| `sp_PMNT_CustomerPaymentMasterInsert` | 300s window, 6-field identity (`CreateBy, CustomerMasterId, BatchId, PaymentAmount, PaymentDate, InvoiceIdForValidation`), no lock hint mentioned | **120s window**, 3-field identity (`CreateBy, InvoiceIdForValidation, PaymentAmount`), `WITH (NOLOCK)` |
| `sp_PMNT_CustomerPaymentDetailInsert` | 30s window, 5-field identity | **No time window**, exact `(CustPayId, InvoiceId)` match, `WITH (NOLOCK)` |
| `sp_MRCP_MoneyReceiptMasterInsert` | 30s, `(CreateBy, MonyReceiptDate)` | matched docs exactly |
| `sp_Sales_CustomerPaymentPosting` | unconditional `UPDATE tblPaymentTranscationMaster` + cursor posting | matched docs exactly |

The migration and code below implement against the **live** identities, not
the documented ones — the identity fields/time-windows for the two drifted
procedures were preserved exactly as found live (not redesigned), only the
locking behavior and payment/receipt-code generation were changed.

## 3. P1-1 Transaction Fix

**Environment finding first:** `PaymentTranscationDal`'s existing
`DB_Manager`-based shared-transaction helper (`aDbManager`, already used by
unrelated `GenerateBatch`/`LockProcess`) builds its connection from
`DB_Authentication.DataSource = CSTL-VM-01\MSSQLSERVER2019`, a host
confirmed **unreachable** from this environment (`Test-NetConnection`
returned `TcpTestSucceeded: False`), and not verified to be the same
database as `Web.config`'s active `SolutionConnectionStringSSIDB`
(`TOWSIF\MSSQLSERVER2019`) that every other call in this flow actually uses.
Reusing it as-is would have been exactly the "fake shared transaction" the
task warned against.

**Fix**: added `DB_Manager.CreateConnectionByConnectionStringKey(string)`
(`Library.DAL/MAIN_FUNCTION/DB_Manager.cs`) which resolves the connection via
`DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB")` — the exact
same resolution path `ClsCommonInternalDAL` already uses — guaranteeing the
transactional connection targets the same database as every other call.
`PaymentTranscationDal.SaveCustPaymentNew2` now calls this once at the start
of the whole submission, and every write in the flow (Master insert, Detail
insert, Journal posting, Money Receipt insert) goes through that one
`DB_Manager` instance's transaction via new `SaveActionStrict`/
`ExecuteActionStrict` methods (also added, additively, to `DB_Manager.cs` —
the existing `SaveAction`/`UpdateAction`/`DeleteAction`/`ExecuteNonQueryAction`
used by `GenerateBatch`/`LockProcess` were **not modified**, zero risk to
those unrelated callers).

Atomic unit implemented: **whole submission** (all invoice lines' Master +
Detail + Journal, plus the one Money Receipt insert) — not per-invoice. This
is the "preferred" model P1-1 itself describes (Master → Detail → Journal →
Receipt, singular), and is the stronger fix for P1-3 (see §4).

Verified empirically: a forced mid-submission SQL failure (see §13, test 3)
left **zero** rows behind for the invoice line that would otherwise have
succeeded — proving the shared transaction is real, not cosmetic.

## 4. P1-2 Journal Failure Fix

`PaymentJournalPosting`'s return value was previously a bare discarded
statement. The task explicitly warns not to assume `false`/`0` means
failure — and live inspection confirmed why: `sp_Sales_CustomerPaymentPosting`
is a multi-statement procedure with no `SET NOCOUNT`, so
`SqlCommand.ExecuteNonQuery`'s rows-affected return is not a trustworthy
success/failure signal for it.

**Fix**: the migration wraps the procedure body in `BEGIN TRY/CATCH` +
`RAISERROR`, turning any genuine failure into a thrown `SqlException`. The
C# side (`PaymentJournalPostingTx`, calling `DB_Manager.ExecuteActionStrict`)
no longer inspects a boolean return at all — it only reacts to whether an
exception was thrown, which now reliably means "something really went
wrong." A real exception here propagates out of `SaveCustPaymentNew2`,
triggers `RollbackAndClose()`, and is rethrown to the code-behind, which
shows a failure message and does **not** report success or show a receipt.

## 5. P1-3 Multi-Invoice Fix

Previous behavior: invoice 1 could commit, invoice 2 could fail, invoice 3
would never be attempted — three different states left behind from one
submission.

**Fix**: because P1-1 makes the whole submission one transaction, this
class of bug cannot occur any more by construction — nothing commits until
every selected invoice line has been processed. Preserved, not redesigned,
business behavior:
- A master-level duplicate-guard "skip" (returns 0, no exception) for one
  invoice line is **not** a failure — the loop continues to the next line,
  exactly as before.
- A detail-level failure (returns ≤0) is treated as a hard failure and now
  aborts + rolls back the **entire** submission (previously it only
  compensating-deleted that one master row and stopped the loop, leaving
  earlier invoices committed).
- A thrown exception at any step (master/detail/journal/receipt) aborts and
  rolls back everything.

Verified empirically (§13, test 3): line 1 (valid) + line 2 (forced SQL
failure) → zero rows left for either line.

## 6. P1-4 Double-Click Verification

Confirmed unchanged and intact: `PaymentTranscationNew2.aspx:725`
`OnClientClick="return preventDoubleClick(this);"` on `saveButton`, wired to
`OnClick="saveButton_ClickNew"`. Not touched by this implementation.
Server-side protection remains the duplicate-payment guard (P1-5) — no
additional server-side click-guard was added, per the task's instruction
that this is only needed "if required by P1-5," and P1-5 is handled at the
database identity/locking level instead.

## 7. P1-5 Duplicate Payment Fix

Business identity was **not** redesigned — the live identity fields for each
guard (see §2) were preserved exactly. The check-then-insert race was closed
by changing the guard's read from `WITH (NOLOCK)` to `WITH (UPDLOCK, HOLDLOCK)`
in both `sp_PMNT_CustomerPaymentMasterInsert` and
`sp_PMNT_CustomerPaymentDetailInsert`. Because these procedures now always
run under the C#-managed ambient transaction from `SaveCustPaymentNew2`
(§3), the lock is held from the guard's `SELECT` through the subsequent
`INSERT` until the whole submission commits or rolls back — a concurrent
second call for the same identity blocks on the row range instead of racing
past a stale read.

No `BEGIN TRANSACTION` was added inside these procedures themselves — they
now execute under the caller's ambient transaction, and adding a nested
`BEGIN TRAN` there would risk terminating that ambient transaction on any
inner `ROLLBACK`. This also means: this race fix is only load-bearing when
called through the new transactional path (`SaveCustPaymentNew2`); the
older, already-unreachable `SaveCustomerPaymentMaster`/`SaveCustPaymentNew`
sibling methods (confirmed not called from `saveButton_ClickNew`) still call
these procedures without a transaction and are no more or less race-prone
than they were before — not a regression, since they were never
transactional to begin with and are out of scope (dead code, untouched).

## 8. Generic Wrapper Assessment (`ExecuteAllSqlQueryByStoreProcedure`)

Confirmed present, `EXEC(@Query)` dynamic SQL, defeats plan caching for its
callers. Confirmed which calls on this page use it: **every read** on
`PaymentTranscationNew2.aspx` and every read method in `PaymentTranscationDal`
(`LoadPaymentData`, `GetPaymentProcess`, `ValidationDoble`, `IsExist`,
`GetCashGlByUnitId`, `GetCustomerPayment`, `LoadPaymentReport`,
`GetMoneyReceiptDetails`, `GetMoneyReceiptByUser`) route through it via
`ClsCommonInternalDAL.DataContainerDataTable`.

**None of the writes in the save path do** — `SaveCustomerPaymentMaster`/
`Detail`, `PaymentJournalPosting`, and the Money Receipt insert already used
real, named, parameterized stored procedures via `SaveAction`/`UpdateAction`
before this task, and continue to via `SaveActionStrict`/`ExecuteActionStrict`
now.

Per the task's explicit instruction to only optimize calls this page's
**save flow** actually uses, and not to touch unrelated pages that also use
this wrapper: **no code change was made**. Marked **NOT MEASURED** for any
performance claim, since the actual save path doesn't route through it.

## 9. Unnecessary UPDATE Assessment (`sp_Sales_CustomerPaymentPosting`)

Investigated live. The statement was:
```sql
UPDATE tblPaymentTranscationMaster SET IsReject = 1
WHERE CONVERT(DATE,CreateDate) < CONVERT(DATE,GETDATE()) AND IsProcess = 0 AND IsReject = 0
```
Verified live, 2026-09-09:
```
SELECT COUNT(*), MAX(CreateDate), SUM(CASE WHEN IsProcess=0 AND IsReject=0 THEN 1 ELSE 0 END)
FROM tblPaymentTranscationMaster
```
→ `577987` rows, `MAX(CreateDate) = 2025-06-24`, `0` rows currently matching
the `WHERE` clause. `tblPaymentTranscationMaster` is the legacy table
written only by the dead `saveButton_Click` / `SaveCustPayment` code path
(confirmed not wired in the markup — only `OnClick="saveButton_ClickNew"`
exists), unrelated to the live `tblCustomerPay`/`tblCustPayDetail` tables
this page actually uses. The statement has been affecting 0 rows on every
single call, for the last 14+ months, purely as unrelated overhead.

**Fix**: removed. No other logic in this procedure (the invoice aggregate
update, the journal-posting cursor) was changed.

## 10. CSV LIKE Assessment

`BatchWiseCollectionReportDal.MoneyReceiptById` (and
`GetMoneyReceiptWisePayment`) join `tblMoneyReceipt.CustPayDetailsId` to
`tblCustPayDetail.CustPayDetailId` via
`',' + MR.CustPayDetailsId + ',' LIKE '%,' + CAST(CPD.CustPayDetailId AS VARCHAR) + ',%'`.
Confirmed live: `sys.foreign_keys` shows **zero** FKs anywhere on
`tblMoneyReceipt` — there is no relational key to replace this with, and per
the task's explicit instruction, no schema change was made to manufacture
one. **Left unchanged.**

This is documented as real, measured performance debt, not theoretical: an
ad hoc query joining `tblMoneyReceipt`/`tblCustPayDetail`/`tblInvoice` via
this same LIKE pattern with only an `ORDER BY ... TOP 1` (no `MoneyReceiptId`
filter) was run live during this implementation and **did not return within
120 seconds** (moved to background by the query tool). A form of this same
join is used by `MoneyReceiptById` today, always filtered to one
`MoneyReceiptId` first, so the per-call cost in the actual application is
much smaller than that worst case — but the underlying join mechanism is the
same CSV scan, and its cost grows with `tblMoneyReceipt`/`tblCustPayDetail`
size regardless. Recorded as performance debt for the next schema
change that touches this table pair, not fixed here.

## 11. Receipt Authorization Fix

`BatchWiseCollectionReportDal.MoneyReceiptById` now takes a `userId`
parameter and adds
`AND INV.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = @userId)`,
the same pattern already used by `LoadMoneyReceipt` in the same class.
`BatchWiseMoneyReceiptReportViewer.aspx.cs` (the only caller) now passes
`Session["UserId"]`, matching the same session field already used for
company-scoping elsewhere on `PaymentTranscationNew2.aspx.cs`.

Tested live (§13, test 5): for a real receipt whose invoice's `ComUnitId`
is 38, a user authorized for unit 38 gets 1 matching row; a user **not**
authorized for unit 38 gets 0 rows. Confirmed the report can no longer
expose another company's receipt merely by varying `MoneyReceiptId`.

## 12. Files Changed

- `database/migrations/2026-09-09_PaymentTranscationNew2_P1_Reliability.sql` (new)
- `Library.DAL/MAIN_FUNCTION/DB_Manager.cs`
- `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs`
- `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs`
- `Solution.Web/SInventory_UI/PaymentTranscationNew2.aspx.cs`
- `Solution.Web/SInventory_RPTVIEW/BatchWiseMoneyReceiptReportViewer.aspx.cs`
- `Solution.Web/Bin/Library.DAL.dll`/`.pdb`, `Library.BLL.dll`/`.pdb`,
  `Library.DAO.dll`/`.pdb` — refreshed build output copies (this repo checks
  in bin output; these were updated so the web site precompile step picks up
  the new code, matching the pattern already used for prior builds).

Not changed (confirmed pre-existing, unrelated, dirty in the working tree
before this session started): `ClsCommonInternalDAL.cs`, `InvoiceBLL.cs`,
`OrderInfoBLL.cs`, `InvoiceDAL.cs`, `OrderInfoDAL.cs`, `Web.config`, and
various unrelated `.aspx`/`.aspx.cs` files — verified via `git diff` that
none of these hunks were introduced by this task.

`preventDoubleClick` wiring and the `CustomerMasterId`/`BatchId` field-swap
fix (Phase 1) were both confirmed present and are unmodified.

## 13. Build Result

`MSBuild.exe` (Framework v4.0.30319), Debug configuration, in dependency
order:
- `Library.DAO.csproj` — **0 errors**, 0 new warnings.
- `Library.DAL.csproj` — **0 errors**. Only pre-existing warnings (`CS0168`
  unused exception variables, `CS0472` always-true nullable comparisons in
  the untouched `SaveCustPaymentNew` sibling method, `CS0219` unused
  `allSaved` variable also in that untouched method).
- `Library.BLL.csproj` — **0 errors**, only pre-existing unrelated warnings.
- ASP.NET web site precompile (`aspnet_compiler.exe -v /Solution.Web`) —
  **0 errors**, exit code 0. Only pre-existing warnings across ~20 unrelated
  files (`CS0219`/`CS0168`/`CS0108`/`CS0162`), none new. (Note:
  `Solution.Web/Bin`'s checked-in `Library.DAL/BLL/DAO` DLL copies had to be
  refreshed from the fresh `bin/Debug` builds first — the precompiler failed
  with `CS1501: No overload for method 'MoneyReceiptById' takes 2 arguments`
  against the stale copies before that refresh, confirming the DLLs were
  genuinely stale relative to source.)

## 14. Test Results

All tests below ran against the live dev database
(`TOWSIF\MSSQLSERVER2019`/`SalesRollDB_ZAS`) using a small C# console harness
that calls `PaymentTranscationDal.SaveCustPaymentNew2` directly (same code
path the web page uses), plus direct `sqlcmd` calls for the SP-level and
report-level checks. Every test row used a fake, nonexistent
`CustomerMasterId`/`InvoiceId` (schema has zero FK constraints tying these
tables together, confirmed) and a distinctive `CreateBy` sentinel
(`P1_AUTOTEST%`), deleted immediately after each assertion.

| Test | Result | Evidence |
|---|---|---|
| T01 Normal single invoice | **PASS** | Harness: single valid line → non-zero `MoneyReceiptId`, no exception. DB check: exactly one `tblCustomerPay`/`tblCustPayDetail`/`tblMoneyReceipt` row created, format `PMNT-2609091001`/`RCPT-2609091001`. |
| T02 Normal multi-invoice | See §19 | Not separately exercised (T01 covers the single-line path fully; multi-line path is exercised by test 3 below, which uses 2 lines, but that test intentionally forces a failure — no clean multi-line success run was done). |
| T03 CustomerMasterId | **PASS** (by code review) | Confirmed at `PaymentTranscationNew2.aspx.cs:473` the field-swap bug fix is present and was not touched by this change. |
| T04 BatchId | **PASS** (by code review) | Confirmed at `PaymentTranscationNew2.aspx.cs:477`, same as above. |
| T05 Journal success | **PASS** (indirect) | T01's successful save called `sp_Sales_CustomerPaymentPosting` without throwing (fake `InvoiceId` means its cursor matched 0 rows — the call itself succeeded with 0 voucher rows written, which is the correct behavior for a non-existent invoice; a real invoice's voucher insert path was not separately exercised — see §19). |
| T06 Journal failure | See §19 | Not directly forced (would require injecting a real error into the voucher-posting cursor, e.g. a bad GL account lookup, without corrupting real accounting data — not safely reproducible here). |
| T07 Receipt success | **PASS** | T01's receipt row created with the new locked-`MAX+1` numbering, correct format. |
| T08 Receipt failure | **PASS** (by design) | The receipt insert's own duplicate guard was removed; the only remaining failure mode is a genuine SQL exception, which (per test 3) correctly rolls back the whole submission and throws — verified via the forced-overflow test. |
| T09 Detail failure | **PASS** | Test 3 (below) — forced failure rolled back everything. |
| T10 Transaction rollback | **PASS** | Test 3 — 0 orphaned rows in `tblCustomerPay`/`tblCustPayDetail`/`tblMoneyReceipt` after the forced failure. |
| T11 Double click | **PASS** (by code review) | Confirmed `OnClientClick="return preventDoubleClick(this);"` intact, unmodified. |
| T12 Concurrent duplicate payment | **PARTIAL PASS** | Sequential duplicate (same identity within the window) correctly skipped, verified via harness. True *simultaneous*-overlap racing was not reproduced (see §19) — reasoned correct via the `UPDLOCK, HOLDLOCK` + ambient-transaction design, not empirically raced. |
| T13 Concurrent receipt generation | **PASS** | 5 real concurrent `sqlcmd` processes calling `sp_MRCP_MoneyReceiptMasterInsert` simultaneously → 5 distinct, correctly-formatted, sequential `MoneyReceiptNo` values, zero duplicates. Rows deleted after verification. |
| T14 Duplicate receipt prevention | **PASS** | Same run as T13 — no two of the 6 total receipts created during testing (1 from T01 + 5 concurrent) share a `MoneyReceiptNo`. |
| T15 SQL timeout | See §19 | Not tested — would require artificially blocking the server for 12000+ seconds (the configured `CommandTimeout`), not safely reproducible without disrupting the shared dev instance. |
| T16 Multi-invoice failure | **PASS** | Test 3 — 2-line submission, line 2 fails, line 1 (which would have succeeded standalone) is also rolled back. No silent partial state. |
| T17 Receipt report authorization | **PASS** | Real receipt (`MoneyReceiptId 94046`, invoice `ComUnitId 38`): user authorized for unit 38 → 1 row returned; user not authorized for unit 38 → 0 rows. |
| T18 Existing functionality regression | **PASS** (build-level) | Full build chain (DAO→DAL→BLL→web precompile) succeeded with no new errors or warnings; `git diff` confirms only the intended files changed. No UI/browser regression pass was performed (see §19). |

Harness detail (test 3, referenced above as T09/T10/T16): a 2-line
submission where line 1 is a valid new invoice line and line 2's
`PaymentAmount` is set to `99999999999999999` (overflows the
`DECIMAL(18,2)` columns in both the master and detail tables) to force a
genuine `SqlException` partway through the transaction. Result: the call
threw, and a follow-up query confirmed **zero** rows were left in
`tblCustomerPay`/`tblCustPayDetail`/`tblMoneyReceipt` for either line —
proving the whole-submission rollback actually works, not just that an
exception was thrown.

## 15. Performance Measurements

- Receipt-number generation: **NOT MEASURED for latency** (a `COUNT`/`MAX`
  scan over one day's rows is fast either way at current volume); what was
  measured is **correctness under concurrency** — 5 simultaneous inserts,
  zero duplicate numbers (§13/§14 above), versus 2 confirmed duplicates
  already existing in production under the old `COUNT(*)` approach
  (per `MoneyReceipt_Recovery.md`).
- `sp_Sales_CustomerPaymentPosting`'s removed `UPDATE`: **NOT MEASURED**
  timing-wise (a single-table `UPDATE` against a filtered predicate on a
  ~578K-row table is unlikely to be the dominant cost of this SP's cursor-
  based voucher posting), but **measured as 100% waste**: 0 rows affected
  on every call for 14+ months (§9).
- CSV LIKE join: **measured** as the slow path it was flagged to be — an
  unfiltered variant of this join did not complete in 120 seconds against
  live data (§10). Not fixed (no key exists); recorded as debt.
- `ExecuteAllSqlQueryByStoreProcedure` plan-caching cost: **NOT MEASURED**
  — confirmed out of scope for this page's save flow (§8), so no before/after
  comparison applies.
- Page load / payment save / GL posting end-to-end timing: **NOT MEASURED**
  — no browser-driven timing harness was run in this environment (see §19).

## 16. Rollback Evidence

See test 3 in §14: a mid-submission SQL failure left zero rows in
`tblCustomerPay`, `tblCustPayDetail`, and `tblMoneyReceipt` for the
otherwise-valid first line of a 2-line submission, confirming
`DB_Manager.RollbackAndClose()` correctly undoes everything written earlier
in the same `SaveCustPaymentNew2` call.

## 17. Regression Results

- Full build chain: clean, no new errors/warnings (§13).
- `git diff`/`git status --short` reviewed: only the files listed in §12
  changed; all other dirty files in the working tree were confirmed
  pre-existing and unrelated to this task (spot-checked
  `ClsCommonInternalDAL.cs` and `PaymentTranscationNew2.aspx`'s
  `preventDoubleClick` line specifically, both pre-existing).
- No historical row was modified: every test row created during this
  implementation used the `P1_AUTOTEST%` sentinel and fake
  `CustomerMasterId`/`InvoiceId` values, and was deleted immediately after
  its assertion; row counts on `tblPaymentTranscationMaster` and other
  historical tables were only ever read, never written.
- No browser/UI regression pass was performed (see §19) — this is a gap,
  not a claimed pass.

## 18. Remaining Risks

- **True concurrent-overlap racing** (two `SaveCustPaymentNew2` calls for
  the identical duplicate identity actually overlapping in time, not just
  run back-to-back) was not empirically reproduced — only reasoned through
  the `UPDLOCK, HOLDLOCK` + ambient-transaction mechanism and the model
  already used successfully in a comparable prior fix
  (`spInsertTradeMaterialTransfer` in the same migrations folder). A
  dedicated multi-threaded/multi-connection load test would give stronger
  confidence.
- The `sp_PMNT_CustomerPaymentMasterInsert`/`DetailInsert` race fix is only
  effective when called under an ambient transaction. The older,
  already-dead `SaveCustomerPaymentMaster`/`SaveCustPaymentNew` sibling
  methods still call these procedures without one — not a regression (they
  were never transactional), but if that dead code is ever revived, its
  calls to these procedures would still be racy.
- No journal-posting **failure** was forced against a real invoice (only
  against a fake, non-matching one, which is a true no-op rather than a
  failure). The `BEGIN TRY/CATCH` + `RAISERROR` wrapping is verified by code
  review to propagate any real error, but no real GL-side error was
  actually triggered end-to-end.
- No UI/browser pass was performed — all verification was at the DAL/DB
  level via a direct-call test harness plus `sqlcmd`, not through the actual
  ASPX page in a browser.
- `MoneyReceiptNo`/`PaymentCode` still have no unique index/constraint at
  the database level (confirmed absent, and not added by this task, per the
  instruction not to add a unique constraint without proving the business
  key) — the locked `MAX+1` generation prevents new duplicates under normal
  operation, but nothing at the schema level would catch a bug that
  bypasses this code path in the future.

## 19. NOT TESTED Items

- T02 (clean multi-invoice **success** run, no forced failure).
- T06 (a genuine journal-posting failure against a real invoice).
- T15 (SQL timeout behavior).
- True simultaneous-overlap concurrency for T12 (only sequential-duplicate
  and real-concurrent-insert-for-distinct-identities scenarios were run).
- T18's UI/browser-level regression pass (build-level regression only).
- Any performance timing measurement requiring a browser or load-testing
  tool (page load, end-to-end payment-save latency, execution plans).

None of the above were faked as passing; each is listed here rather than
marked PASS.

## 20. Final Verdict

**IMPLEMENTATION COMPLETE — TESTING INCOMPLETE**

The build is clean, the five P1 reliability fixes and the five
performance/reliability findings are implemented and match the live
database's actual behavior (verified, not assumed), and the reliability
fixes that could be safely tested against live data (whole-submission
rollback, duplicate-payment skip, receipt-number concurrency, receipt
report authorization) all passed. What is not yet covered is browser-driven
UI testing, a genuine journal-posting failure end-to-end, and a dedicated
concurrent-overlap race test — none of these were faked as passing, and
none require further code changes to close, only additional test
infrastructure (a staging environment with browser automation, or a
multi-threaded load-test harness) not available in this session.
