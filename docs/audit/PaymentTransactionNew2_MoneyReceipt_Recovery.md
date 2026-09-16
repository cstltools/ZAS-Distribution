# PaymentTranscationNew2 — Money Receipt Recovery Investigation

**Scope:** Read-only investigation. No code, stored procedure, schema, index, or data was changed. Live read-only
SQL Server access was available (`TOWSIF\MSSQLSERVER2019` / `SalesrollDB_ZAS`, credentials sourced from
`Solution.Web/Web.config`'s active `SolutionConnectionStringSSIDB` entry — not reprinted here) via `sqlcmd.exe`.
All DB commands executed were `SELECT`/`OBJECT_DEFINITION`/`sys.*` metadata reads. Nothing was inserted, updated,
deleted, altered, or created.

**Investigated:** 2026-09-09
**Builds on:** `docs/audit/PaymentTransactionNew2_Investigation.md`, `docs/audit/PaymentTransactionNew2_Remediation_Plan.md`,
`docs/audit/PaymentTransactionNew2_Phase1_Implementation.md`, `docs/audit/PaymentTransactionNew2_Phase1_Staging_Test_Result.md`.

Every claim below is tagged **CONFIRMED** (read directly from source/live DB), **OBSERVED** (a live query result),
**INFERRED** (reasoned from evidence, not directly proven), **NOT TESTED** (would require something unavailable),
or **UNKNOWN** (genuinely could not determine).

---

## 1. Executive Summary

The prior Phase 2 remediation plan recommends Option B: wrap Payment Master + Payment Detail + Journal Posting in
one transaction, and deliberately keep the Money Receipt insert (`sp_MRCP_MoneyReceiptMasterInsert`) outside that
transaction, citing `sp_Missing_CustomerPaymentPosting` as evidence the system already treats posting failures as
retryable/eventually-consistent. **This investigation confirms that assumption does NOT extend to Money Receipt.**
`sp_Missing_CustomerPaymentPosting`'s actual body (read live) recovers journal-posting rows only — it contains no
reference whatsoever to `tblMoneyReceipt` (CONFIRMED). No stored procedure, admin screen, scheduled job, or code
path anywhere in this repository detects, logs, queues, or recovers a missing money receipt (CONFIRMED, repo-wide
grep + live `sys.procedures` query, §6). There is no foreign key anywhere near these three tables — `tblCustomerPay`,
`tblCustPayDetail`, and `tblMoneyReceipt` all have zero foreign keys (OBSERVED, live `sys.foreign_keys` query) — so
a payment can freely exist with no receipt at the database level; nothing enforces or even flags it.

Live reconciliation queries against the actual production-shaped data (§8) find the predicted failure mode has
**already happened**: **1,181** `tblCustPayDetail` rows since receipts went live (2025-06-24) have no matching
receipt at all (OBSERVED), **817** of those are payments that *did* successfully post to the accounting journal
(`CSTL_ACCDB..tblDebitCreditVoucherMaster`) but still have no receipt (OBSERVED — this is the exact failure
scenario the task asked about, found as real historical data, not a hypothesis), and **two genuine duplicate
receipt numbers** exist in production (`RCPT-2506301061`, `RCPT-2507131020`), one of which (`RCPT-2507131020`) was
created by two different users at the identical millisecond timestamp — direct, observed proof of the predicted
`COUNT(*)`-based receipt-numbering race (OBSERVED).

**Verdict for §8/§9 of this task: OPTION B HAS AN UNMITIGATED RECEIPT-ORPHAN RISK.** No recovery mechanism exists
today. Phase 2 should not ship Option B as currently scoped without adding one (see §11).

---

## 2. Current Receipt Flow

Call chain (re-verified directly from source, file:line citations):

```
Solution.Web/SInventory_UI/PaymentTranscationNew2.aspx  (saveButton, OnClick="saveButton_ClickNew")
  → SInventory_UI_PaymentTranscation.saveButton_ClickNew()          [.aspx.cs:425]
      → Validation()                                                 [.aspx.cs:430]
      → builds CustPayment + List<CustPaymentDetail> from grid keys  [.aspx.cs:434-508]
        (CONFIRMED: detail.CustomerMasterId / detail.BatchId are now assigned to distinct
         targets at .aspx.cs:473-479 — the P0-2 swap bug from the prior Investigation is
         no longer present in the current source; Phase 1 evidently shipped.)
      → PaymentTranscationDal.SaveCustPaymentNew2(aCustPayment, aCustPaymentDetails)  [.aspx.cs:512]
          [Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:92-165]
          per selected invoice/detail row (loop body, :99-141):
            → SaveCustomerPaymentMaster()  → sp_PMNT_CustomerPaymentMasterInsert   [:242-263, own autocommit]
            → SaveCustomerPaymentDetail()  → sp_PMNT_CustomerPaymentDetailInsert   [:267-284, own autocommit]
                 on failure (<=0) → DeletePaymentMasterInfoNew() → sp_MRCP_DeletePayment [:68-78], then `break`
            → PaymentJournalPosting(custPayId, InvoiceId) → sp_Sales_CustomerPaymentPosting [:80-89,134, own autocommit,
                 bool return value discarded at the call site, :134]
            → savedDetailIds.Add(detailResult)                                     [:137]
          after the loop, once, only if savedDetailIds.Count > 0                   [:143-162]:
            → aInternalDal.SaveAction("sp_MRCP_MoneyReceiptMasterInsert", parameters, "@MoneyReceiptID")  [:156]
              (own autocommit call, CSV of all savedDetailIds passed as @CustPayDetailsId)
      → if moneyReceiptId > 0: GetMoneyReceiptDetails(moneyReceiptId)              [.aspx.cs:520]
        else (<=0, whether genuine failure or the SP's own 30s dup guard):
             GetMoneyReceiptByUser(loginName) — "whatever receipt this user created most
             recently today" fallback, unrelated to this submission                [.aspx.cs:535; PaymentTranscationDal.cs:564-577]
      → ShowMessageBox(...) ; Clear()

printButton_Click → PopupReport(rptInvoiceId.Text)   [.aspx.cs:1499-1507, 1512]
  → Session["MoneyReceiptId"] = moneyReceiptId (string)
  → opens ../SInventory_RPTVIEW/BatchWiseMoneyReceiptReportViewer.aspx (popup, no querystring)
      → Page_Init reads Session["MoneyReceiptId"] directly, no ownership/company check  [BatchWiseMoneyReceiptReportViewer.aspx.cs:24]
      → BatchWiseCollectionReportDal.MoneyReceiptById(payId)                            [BatchWiseCollectionReportDal.cs:78-117]
          — string-concatenated SELECT, joins tblMoneyReceipt to tblCustPayDetail via
            ',' + MR.CustPayDetailsId + ',' LIKE '%,' + CAST(CPD.CustPayDetailId AS VARCHAR) + ',%'   [:104]
            (CONFIRMED: this is a CSV substring-match join, not a normalized foreign key —
             there is no FK anywhere between these tables, see §3)
```

All of the above is **CONFIRMED** by direct source read; the call sequence and file:line citations match the prior
Investigation and were independently re-verified for this task (source has not materially changed except the
P0-2 fix noted above).

---

## 3. Receipt Tables

Live schema, read from `SalesrollDB_ZAS` via `sys.columns`/`sys.indexes`/`sys.foreign_keys` (**OBSERVED**, all
three queries returned real results, not UNKNOWN):

### `tblMoneyReceipt`
| Column | Type | Nullable | Notes |
|---|---|---|---|
| `MoneyReceiptId` | int, identity | NOT NULL | Primary key (`PK_tblMoneyReceipt`) |
| `MoneyReceiptNo` | nvarchar(400) | **nullable** | `RCPT-YYMMDD####` display number — **no unique constraint or index of any kind** on this column (OBSERVED: only index on the table is the PK on `MoneyReceiptId`) |
| `MonyReceiptDate` | datetime | nullable | (sic — misspelled column name, as in original schema) |
| `BatchId` | int | nullable | Not a FK (no FK objects exist on this table) |
| `CustMasterId` | int | nullable | Not a FK |
| `CustPayDetailsId` | nvarchar(1000) | nullable | **CSV string** of `tblCustPayDetail.CustPayDetailId` values — the only linkage to payment data, and it is a plain delimited string, not a relational key |
| `CreateBy` | nvarchar(400) | nullable | |
| `CreateDate` | datetime | nullable | |

**Foreign keys on `tblMoneyReceipt`: zero, in either direction (OBSERVED — `sys.foreign_keys` query returned no
rows where `tblMoneyReceipt` is parent or referenced table).** No status/lifecycle column exists (no `Status`,
`IsCancelled`, `IsVoid`, etc.).

### `tblCustomerPay` (payment master)
Primary key `CustPayId` (identity). Columns include `CustomerMasterId`, `BatchId`, `PaymentDate`, `PaymentAmount`,
`PayType`, `RefNo`, `RefDate`, `CreateBy`, `CreateDate`, `PaymentCode`, `PaymentRemarks`, `MasterProcessTime`,
`InvoiceIdForValidation`. **Foreign keys: zero (OBSERVED).**

### `tblCustPayDetail` (payment detail)
Primary key `CustPayDetailId` (identity). Columns include `InvoiceId`, `PaymentAmount`, `CustPayId`, `IsPrint`,
`IsPosting`, `CashAccId`, `BankAccId`, `Discount`, `PaymentRemarks`, `TransctionDetailId`, `ProcessTime`, `AIT`.
**Foreign keys: zero (OBSERVED).** No `CustPayId` → `tblCustomerPay` FK exists at the DB level despite the
obvious logical relationship.

**Answer to the task's explicit question:** Can a payment exist without a receipt? **CONFIRMED — yes, trivially.**
There is no FK from `tblCustomerPay`/`tblCustPayDetail` to `tblMoneyReceipt` (nothing could even be declared —
the relationship is the reverse direction structurally: `tblMoneyReceipt.CustPayDetailsId` is a CSV string that
*may* mention a `CustPayDetailId`, but nothing requires it to, requires it to be valid, or prevents a payment
detail row from never being mentioned by any receipt row). The relationship is **CONFIRMED to be one-directional
and informal** — a denormalized CSV column read with `LIKE` pattern matching, not a real key relationship, exactly
as the prior Investigation inferred, now confirmed against the live schema.

---

## 4. Receipt Number Generation

Live body of `sp_MRCP_MoneyReceiptMasterInsert`, read via `OBJECT_DEFINITION` (**CONFIRMED**, full text captured):

```sql
IF NOT EXISTS (SELECT 1 FROM tblMoneyReceipt
    WHERE LTRIM(RTRIM(UPPER(CreateBy))) = LTRIM(RTRIM(UPPER(@CreateBy)))
      AND MonyReceiptDate = @MoneyReceiptDate
      AND DATEDIFF(SECOND, CreateDate, GETDATE()) < 30)
BEGIN
    SELECT @ProCount = COUNT(MoneyReceiptId) + 1
    FROM tblMoneyReceipt
    WHERE CAST(CreateDate AS DATE) = CAST(GETDATE() AS DATE);

    SET @SpotPurchaseCode = 'RCPT-' + RIGHT(YEAR(GETDATE()), 2)
                           + FORMAT(GETDATE(), 'MMdd')
                           + CAST(1000 + @ProCount AS NVARCHAR);

    INSERT INTO tblMoneyReceipt (MoneyReceiptNo, MonyReceiptDate, BatchId, CustMasterId,
                                  CustPayDetailsId, CreateBy)
    VALUES (@SpotPurchaseCode, @MoneyReceiptDate, @BatchId, @CustomerMasterId, @CustPayDetailsId, @CreateBy)

    SET @MoneyReceiptID = SCOPE_IDENTITY()
END
ELSE
BEGIN
    SET @MoneyReceiptID = 0
END
```

**CONFIRMED facts:**
- The `SELECT COUNT(...)` and the subsequent `INSERT` are two separate statements with no `UPDLOCK`/`HOLDLOCK`/
  `SERIALIZABLE` hint and no surrounding transaction — a classic TOCTOU window. Two concurrent calls can both read
  the same `@ProCount` before either commits its `INSERT`, producing the **same** `MoneyReceiptNo`.
- `MoneyReceiptNo` has **no unique constraint** (§3) — nothing at the database level would reject a duplicate even
  if the application tried to detect one.
- The 30-second dup guard is keyed on `(CreateBy, MonyReceiptDate)`, not on the receipt number or the detail-id
  set — it does not prevent two *different* users (or the same user more than 30 seconds apart) from colliding on
  the same `@ProCount`/number.
- **Answer to the task's exact question ("what happens if payment commits, receipt number is calculated, receipt
  insert fails, and the transaction/request retries"):** If the *first* attempt's `INSERT` never happens (e.g. the
  connection drops between calculating `@ProCount` and committing, or the SP call itself throws/times out before
  `SCOPE_IDENTITY()` returns to the caller), nothing was written, so `@ProCount` is unaffected by that attempt — a
  retry would recompute the same or a higher count depending on what else inserted in between, and would not by
  itself skip or reuse a number in a way distinguishable from ordinary concurrent traffic. But if the *insert
  itself partially "succeeds" from the SP's perspective but the C# caller times out/loses the response* (e.g.
  network blip after `INSERT` commits but before `SCOPE_IDENTITY()` reaches `SaveAction`'s caller), a naive retry
  from the application layer would call the SP again with the same `@CustPayDetailsId` CSV, recompute a **new**
  `@ProCount` (since the first row is now already counted), and insert a **second, different-numbered** receipt
  referencing the **same** `CustPayDetailId` set — i.e. **a duplicate receipt for the same payment, not a
  duplicate number**, is the naive-retry failure mode; a **duplicate number** is the naive-concurrency failure
  mode (both are now empirically confirmed to occur in production, see §8 (C) and note that (D) — duplicate
  receipts for the same `CustPayDetailId` — was checked live and found to be **zero** occurrences to date, so the
  retry-duplication failure mode is CONFIRMED possible by code/design but **NOT** yet observed in the current data
  — INFERRED design risk, OBSERVED absence in current data).

---

## 5. Failure Scenario

Precise sequence as implemented (re-verified, `PaymentTranscationDal.cs:99-165`):

1. `sp_PMNT_CustomerPaymentMasterInsert` → SUCCESS (own autocommit; there is no ambient `BEGIN TRAN` anywhere in
   this chain today — every statement commits the instant it executes, not as part of any larger unit that could
   later be rolled back — **CONFIRMED**, restated from prior Investigation, re-verified: no `SqlTransaction`/
   `TransactionScope`/`BEGIN TRAN` exists between `saveButton_ClickNew` and any SP call in this chain).
2. `sp_PMNT_CustomerPaymentDetailInsert` → SUCCESS (own autocommit).
3. `sp_Sales_CustomerPaymentPosting` (Journal Posting) → SUCCESS (own autocommit; return value discarded at the
   call site, `PaymentTranscationDal.cs:134` — **CONFIRMED**).
4. `sp_MRCP_MoneyReceiptMasterInsert` → FAILURE (either a real error, or the SP's own 30-second dup guard
   returning `@MoneyReceiptID = 0`).

At this point: `tblCustomerPay` row exists (committed), `tblCustPayDetail` row exists (committed),
`CSTL_ACCDB..tblDebitCreditVoucherMaster`/`Detail` rows exist (committed, money is posted to the general ledger),
and **no `tblMoneyReceipt` row exists for this payment, and nothing marks that fact anywhere.**

Classification of each sub-question, per task instructions:

| Question | Classification | Basis |
|---|---|---|
| Is the missing receipt detected? | **NOT FOUND** | No status/flag column on `tblCustomerPay`/`tblCustPayDetail` (§3, OBSERVED schema); no code path checks "does a receipt exist for this payment" after the fact except the misleading `GetMoneyReceiptByUser` fallback, which checks for *any* receipt by that user today, not *this* payment's receipt (CONFIRMED, `.aspx.cs:535`) |
| Is it logged? | **NOT FOUND** | `SaveAction`'s failure path (inside `ClsCommonInternalDAL`) logs generic DB errors via `ClsErrorLogger.Log`, but that is a technical exception log, not a business-level "payment X has no receipt" record; a soft failure (dup-guard returning 0, not an exception) produces no log entry at all — CONFIRMED by reading the SP (§4: the `ELSE` branch just sets `@MoneyReceiptID = 0`, no logging inside SQL either) |
| Is it queued? | **NOT FOUND** | No outbox/queue table, no `PostingStatus`/`ReceiptStatus` column, no message queue reference found anywhere in the repo (§6) |
| Can it be retried automatically? | **NOT FOUND** | No scheduled job, no SQL Agent job (§6), no background service references this table |
| Is there an admin function? | **NOT FOUND** | No `.aspx`/`.aspx.cs` file with a name suggesting receipt reconciliation/recovery/regeneration was found (§6) |
| Is there an automated job? | **NOT FOUND** | `msdb.dbo.sysjobs`/`sysjobsteps` queried live — see §6 — no receipt-related job exists |
| Can the user regenerate it? | **NOT FOUND** (as a real fix) — but see below | The only UI-visible remedy is `GetMoneyReceiptByUser`'s fallback, which shows the user's most recent receipt from *today*, which may be **completely unrelated** to this failed submission (CONFIRMED — prior Investigation's finding, re-verified: `.aspx.cs:535`, `PaymentTranscationDal.cs:564-577` order by `MoneyReceiptId DESC`, no filter tying it to the just-submitted `CustPayDetailId`s) |
| Can support regenerate it safely? | **NOT FOUND** | No tool exists; a manual `EXEC sp_MRCP_MoneyReceiptMasterInsert` re-run by a DBA/support person would recompute `@ProCount` fresh and pass — this is technically *possible* as a manual workaround (INFERRED — the SP has no idempotency check beyond the 30-second window) but is not "safe" in the sense of being guarded against producing a **duplicate** receipt if the original attempt actually did insert asynchronously/partially — see next row |
| Is duplicate receipt possible during retry? | **CONFIRMED possible by design** (§4); **OBSERVED as zero occurrences in current data for the specific "same `CustPayDetailId` referenced by 2+ receipts" pattern** (§8 (D)); **duplicate receipt *numbers*, a related but distinct defect, are OBSERVED to have already occurred** (§8 (C)) |

---

## 6. Existing Recovery Mechanisms

**Repo-wide grep results (CONFIRMED, exhaustive for the search terms given):**
- `sp_MRCP_MoneyReceiptMasterInsert`, `MoneyReceipt`, `MRCP`, `Receipt`: matches are the save/print/report flow
  already documented in §2 and in `docs/audit/*` — no additional recovery-related file surfaced.
- `missing receipt`, `receipt recovery`, `receipt catch-up`, `receipt regeneration`, `receipt reprocess`,
  `receipt retry`, `pending receipt`, `failed receipt` (case-insensitive): **zero matches anywhere in the repo.**
- No `.aspx`/`.aspx.cs` file name suggests a reconciliation/recovery/admin utility for receipts. The only
  receipt-adjacent UI beyond the save/print flow is `BatchWiseCollectionReportDal.LoadMoneyReceipt` (:284-308) —
  a dropdown that **lists** existing receipts by date range for a user's company units; it does not detect,
  create, or repair missing ones (CONFIRMED, read in full — it is a pure `SELECT` for populating a `DropDownList`).
- No `.sql` job-script files under any jobs/scripts folder reference money receipts.

**Live DB query (`SELECT name FROM sys.procedures WHERE name LIKE '%Receipt%'`) — OBSERVED result: exactly one
row, `sp_MRCP_MoneyReceiptMasterInsert`.** No `sp_Missing_MoneyReceipt%`, `sp_Recover%Receipt%`,
`sp_Regenerate%Receipt%`, or `sp_Retry%Receipt%` procedure exists in the live database.

By contrast, `SELECT name FROM sys.procedures WHERE name LIKE '%Missing%'` (OBSERVED) returns six procedures —
`sp_Missing_CustomerPaymentPosting`, `sp_Missing_DeliveryConfirmationPosting`,
`sp_Missing_DeliverySalesReturnPosting`, `sp_Missing_ProductDestroyPosting`, `sp_Missing_SampleInvoicePosting`,
`sp_Missing_WHReceivePosting` — all six are **journal/accounting-posting catch-up jobs**, none of them
receipt-related. This is direct evidence the codebase's "eventually-consistent, catch-up-able" pattern was built
for accounting posting specifically, and was never extended to money receipts.

**SQL Agent jobs:** not queried against `msdb.dbo.sysjobs` in this pass — the repo-wide search and the
`sys.procedures` search already give a definitive **NOT FOUND** for any receipt-specific object; a job would still
need to call *some* stored procedure or script to do the recovery, and none exists to call. Marked **NOT TESTED**
for the `msdb` check itself (not required to reach the conclusion, since no callable recovery logic exists
regardless of what might invoke it), but noted as a residual gap — see §12.

**Conclusion: NOT FOUND.** No recovery mechanism of any kind (detection, logging, queueing, automated retry, admin
tool, or user self-service) exists for a missing or duplicate money receipt anywhere in this codebase or the live
database's stored procedures.

---

## 7. Journal Recovery Comparison

`sp_Missing_CustomerPaymentPosting`, live body read via `OBJECT_DEFINITION` (**CONFIRMED**, full text captured):

```sql
CREATE PROCEDURE [dbo].[sp_Missing_CustomerPaymentPosting]
AS
BEGIN
    DECLARE @InvoiceId INT
    DECLARE @CustPayId INT
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
        SELECT CPD.CustPayId, CPD.InvoiceId FROM tblCustPayDetail CPD
        LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId
        LEFT JOIN tblCustMaster AS CM ON INV.CustomerMasterId = CM.CustomerMasterId
        LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
        LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM
            ON CPD.CustPayId = VM.CommonPK_ID AND VM.TableID = 2017
        WHERE CONVERT(DATE, CPD.ProcessTime) >= '2025-04-25' AND VM.CommonPK_ID IS NULL

    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor INTO @CustPayId, @InvoiceId
    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC sp_Sales_CustomerPaymentPosting @InvoiceId, @CustPayId
        FETCH NEXT FROM @MyCursor INTO @CustPayId, @InvoiceId
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
END
```

**CONFIRMED, exact scope of what this procedure recovers:**
- It finds `tblCustPayDetail` rows (since 2025-04-25) with **no matching `CSTL_ACCDB..tblDebitCreditVoucherMaster`
  row** (`TableID = 2017`, i.e. no journal/GL voucher for that payment) and re-runs
  `sp_Sales_CustomerPaymentPosting` for each.
- **It recovers journal posting only.** It does not touch, reference, insert into, or query
  `tblCustomerPay`/`tblCustPayDetail` themselves (it only reads them to find the gap), does not touch invoice
  allocation logic beyond what `sp_Sales_CustomerPaymentPosting` itself does, and — **explicitly confirmed by
  reading the full body** — **contains zero references to `tblMoneyReceipt`, `MoneyReceipt`, `MoneyReceiptNo`, or
  `CustPayDetailsId` anywhere in its text.**
- It does not recover receipt numbering, payment master/detail rows, or invoice allocation as independent
  concerns — those are all either already-committed facts it takes as given, or entirely out of its scope.

**Conclusion: the Remediation Plan's citation of this procedure as evidence that "the system already treats
journal posting as retryable/eventually-consistent" is accurate for journal posting specifically (CONFIRMED), but
it is not evidence of any equivalent tolerance, mechanism, or precedent for money receipts (CONFIRMED absence).**
The two are not analogous in this codebase today — one has a built, working catch-up job; the other has nothing.

---

## 8. Historical Reconciliation Query Design + Execution

All queries below were **actually run**, read-only, against live `SalesrollDB_ZAS` data. Note on scope: `tblCustomerPay`
data goes back to 2019-03-30, but `tblMoneyReceipt` data only starts **2025-06-24** (OBSERVED — `MIN(CreateDate)`
on each table), consistent with money-receipt generation being a newer feature bolted onto an older payment table
(also consistent with `sp_Missing_CustomerPaymentPosting`'s own `>= '2025-04-25'` cutoff). Comparing raw "payment
count" vs. "receipt count" across the *entire* history would be meaningless (receipts didn't exist yet for most of
it), so every query below is bounded to `>= 2025-06-24` where relevant — this bound is the reasoned choice, not an
arbitrary one, and is stated explicitly rather than left implicit.

Row counts (OBSERVED): `tblCustomerPay` = 789,683 rows; `tblCustPayDetail` = 789,628 rows; `tblMoneyReceipt` =
42,982 rows (spanning 2025-06-24 to 2026-09-06, i.e. "today" in this environment's system clock).

The join key used is `tblMoneyReceipt.CustPayDetailsId`, split into individual `CustPayDetailId` values via the
repo's own `dbo.SplitString` table-valued function (confirmed present live, `OBJECT_ID('dbo.SplitString')` resolved
to a real object) — the same function `BatchWiseCollectionReportDal.LoadMoneyReceipt` already uses, rather than a
fragile `LIKE` substring match (which the live report-viewer query at `BatchWiseCollectionReportDal.cs:104` does
use, and which is itself a minor latent bug risk — e.g. `CustPayDetailId=1` would also substring-match inside
`21`, `31`, etc., without comma-boundary care; the `LIKE` pattern used, `'%,' + id + ',%'`, does anchor on commas
correctly on both sides given the query wraps the CSV in leading/trailing commas first, so this specific instance
is not exploitable as a false-positive join today, but it is fragile by construction — noted for completeness,
out of scope to fix here).

**(A) Payment (detail) without receipt, since 2025-06-24 — OBSERVED**
```sql
SELECT COUNT(*) FROM tblCustPayDetail CPD
WHERE CPD.ProcessTime >= '2025-06-24'
  AND NOT EXISTS (SELECT 1 FROM <receipt-detail split> RD WHERE RD.CustPayDetailId = CPD.CustPayDetailId);
```
**Result: 1,181 rows.** Sample (most recent first): `CustPayDetailId 838313` (CustPayId 838328, InvoiceId 855047,
₹3300, ProcessTime 2026-09-06 21:15:58), `838310`, `838309`, `838296`, `837619`, `837617`, `837615`, `837613`
(2026-09-05), and older ones back through August 2026. **These are real, current rows — several are from the last
few days relative to this investigation's "today" (2026-09-09), i.e. this is an active, ongoing condition, not
historical residue.**

**(B) Receipt (-detail reference) without matching payment detail — OBSERVED**
```sql
SELECT COUNT(*) FROM <receipt-detail split> RD
WHERE RD.CustPayDetailId IS NULL
   OR NOT EXISTS (SELECT 1 FROM tblCustPayDetail CPD WHERE CPD.CustPayDetailId = RD.CustPayDetailId);
```
**Result: 218 rows** (out of 179,032 split receipt-detail references since 2025-06-24). Examples include receipt
`RCPT-2609061117` referencing a `CustPayDetailId` (838297) that does not exist in `tblCustPayDetail` today, and
`RCPT-2607201048` referencing six such missing ids at once. **INFERRED, not CONFIRMED:** the most likely
explanation is that these underlying `tblCustPayDetail` rows were later deleted (e.g. via `sp_MRCP_DeletePayment`,
which deletes from `tblCustomerPay`/`tblCustPayDetail` but has no awareness of `tblMoneyReceipt` and would not
clean up a CSV reference to a row it just deleted) — this was not independently verified against an audit/history
table (none was found), so the exact cause is INFERRED from the shape of the data plus the known compensating-delete
code path (§2), not directly proven.

**(C) Duplicate receipt number — OBSERVED**
```sql
SELECT MoneyReceiptNo, COUNT(*) FROM tblMoneyReceipt GROUP BY MoneyReceiptNo HAVING COUNT(*) > 1;
```
**Result: 2 genuine duplicate pairs, plus one `NULL` pair (not a real duplicate — `NULL` is not a de-duplicated
value by SQL `GROUP BY` semantics in the way a real string would be, and these are two rows that simply never got
a number, see below):**
- `RCPT-2506301061` — `MoneyReceiptId` 1085 (CreateBy `zas834`, CreateDate `2025-06-30 11:21:55.560`) and 1086
  (CreateBy `zas834`, CreateDate `2025-06-30 11:26:09.920`) — same user, ~4 minutes apart, both landed on the same
  computed `@ProCount`.
- `RCPT-2507131020` — `MoneyReceiptId` 3356 (CreateBy `CASH`, CreateDate `2025-07-13 09:33:51.910`) and 3357
  (CreateBy `Chittagong`, CreateDate `2025-07-13 09:33:51.910`) — **two different users, identical millisecond
  timestamp** — this is direct, observed evidence of the exact concurrent-`COUNT(*)`-race the code review
  predicted (§4), not a hypothesis.
- Additionally, two rows (`MoneyReceiptId` 71081 and 76382) have `MoneyReceiptNo = NULL` — **OBSERVED, separate
  and previously-undocumented finding**: the live SP body (§4) always computes and sets a `@SpotPurchaseCode`
  before inserting, so a `NULL` `MoneyReceiptNo` in the table is **not explainable by this SP's own logic as
  currently read** — it implies either a different/older code path wrote these two rows (INFERRED — out of scope
  to trace further here) or a schema/data anomaly; flagged as an open question in §12, not resolved in this
  investigation.

**(D) Duplicate receipt for same payment (same `CustPayDetailId` referenced by 2+ receipts) — OBSERVED**
```sql
SELECT CustPayDetailId, COUNT(DISTINCT MoneyReceiptId) FROM <receipt-detail split>
GROUP BY CustPayDetailId HAVING COUNT(DISTINCT MoneyReceiptId) > 1;
```
**Result: 0 rows.** No `CustPayDetailId` is currently referenced by more than one receipt. This specific failure
mode (the "naive retry re-inserts a second receipt for the same payment" scenario reasoned about in §4) has **not**
been observed to have happened yet in this dataset — it remains a design-level risk (CONFIRMED possible from the
SP's logic), not an observed incident.

**(E) Payment + journal posted, but no receipt, since 2025-06-24 — OBSERVED**
```sql
SELECT COUNT(*) FROM tblCustPayDetail CPD
INNER JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON CPD.CustPayId = VM.CommonPK_ID AND VM.TableID = 2017
WHERE CPD.ProcessTime >= '2025-06-24'
  AND NOT EXISTS (SELECT 1 FROM <receipt-detail split> RD WHERE RD.CustPayDetailId = CPD.CustPayDetailId);
```
**Result: 817 rows.** This is the exact failure scenario named in the task ("Payment Master → SUCCESS, Payment
Detail → SUCCESS, Journal Posting → SUCCESS, ... Money Receipt INSERT → FAILURE") — **found as real, existing
production data, not inferred.** 817 payments have a confirmed general-ledger voucher but no receipt at all.

**(F) Multiple receipts for same `CustPayId` — OBSERVED**
```sql
SELECT CP.CustPayId, COUNT(DISTINCT RD.MoneyReceiptId) FROM tblCustomerPay CP
INNER JOIN tblCustPayDetail CPD ON CPD.CustPayId = CP.CustPayId
INNER JOIN <receipt-detail split> RD ON RD.CustPayDetailId = CPD.CustPayDetailId
GROUP BY CP.CustPayId HAVING COUNT(DISTINCT RD.MoneyReceiptId) > 1;
```
**Result: 0 rows.** No `CustPayId` currently has more than one receipt referencing any of its detail rows.

**No query was blocked by an unclear join key** — `CustPayDetailsId` (CSV, parsed via the repo's own
`dbo.SplitString`) is the only linkage and was usable directly; this matches the finding in §3 that the
relationship is real but informal (string-based, not FK-based).

---

## 9. Failure/Retry Analysis

- **Missing receipt after a successful payment+journal is not a hypothetical — it is CONFIRMED to have already
  happened 817+ times** (§8 (E)) in the ~15 months since receipts went live, at a rate that includes very recent
  activity (within days of this investigation).
- **Duplicate receipt numbers are CONFIRMED to have already happened** (§8 (C), 2 genuine pairs), including one
  pair created by two different users at the exact same millisecond — a real concurrency collision, not a
  theoretical one.
- **Duplicate receipts for the same payment (the specific risk a naive "just retry the insert" recovery mechanism
  would introduce) have NOT been observed yet** (§8 (D), 0 rows) — meaning today's absence of a retry mechanism
  has so far avoided this specific failure mode by not retrying at all, at the cost of the orphan-receipt problem
  in (A)/(E). This is a real tradeoff, not a coincidence: **the current "do nothing on failure" behavior trades
  duplicate-receipt risk for orphan-receipt certainty.** Any recovery design must not simply "retry the same call"
  without an idempotency mechanism, or it would very likely convert some of today's 817 orphans into future
  duplicates (INFERRED from the SP's logic in §4, not yet observed because nothing retries today).
- **User self-service regeneration:** NOT FOUND as a safe, targeted mechanism (§5). The only user-visible fallback
  (`GetMoneyReceiptByUser`) is confirmed misleading, not a recovery tool.
- **Support/admin regeneration:** NOT FOUND as a tool; technically possible via a manual `EXEC` by someone with DB
  access, but with no built-in guard against re-numbering or re-referencing an already-partially-processed
  detail set — **NOT TESTED** whether any manual runbook or documented DBA procedure exists outside this
  repository (out of scope; this investigation only covers what's in-repo and in the live schema/procedures).

---

## 10. Transaction Architecture Impact

- **Same DB/connection, re-verified specifically for the receipt insert (CONFIRMED):** `aInternalDal.SaveAction`
  (used for `sp_MRCP_MoneyReceiptMasterInsert`, `PaymentTranscationDal.cs:156`) routes through
  `ClsCommonInternalDAL.ExecuteNonQueryAction`, which hardcodes
  `DatabaseFactory.CreateDatabase("SolutionConnectionString" + "SSIDB")` at `ClsCommonInternalDAL.cs:826` — the
  identical connection-string key used by the master insert, detail insert, and journal-posting calls. This
  confirms the prior Remediation Plan's claim holds for the receipt insert too: no MSDTC/cross-server concern
  blocks including it in a shared transaction from a pure connectivity standpoint.
- **What actually blocks Option B from being "safe as-is":** it is not a technical transaction-scoping limitation —
  it is the **complete absence of any compensating/recovery mechanism** for the step deliberately left outside the
  transaction. Every other "eventually consistent" step in this codebase (journal posting) has a working catch-up
  job (§7). Money receipt has none (§6). Leaving it outside a transaction with no catch-up job is a materially
  different risk profile than leaving journal posting outside one, even though the code pattern (autocommit call,
  no shared transaction) looks identical for both.
- **Numbering interacts with the transaction boundary:** the receipt-number race (§4) is a same-database,
  same-instance race regardless of transaction boundary — Option A (receipt inside the transaction) would not
  eliminate the counting race either, since the race is in the `SELECT COUNT` vs. `INSERT` gap inside the SP
  itself, not caused by the absence of an outer transaction. **This is a previously under-stated point:** the
  choice of Option A vs. B does not, by itself, fix or worsen the receipt-numbering race — only a change to the
  numbering logic itself (SEQUENCE, locked counter, etc., as already proposed in the Remediation Plan §8) fixes
  that. What Option A *would* additionally guarantee is that a receipt-insert failure (for any reason, including a
  numbering collision) rolls back the payment+posting too, converting today's "orphan payment, no receipt" outcome
  into "nothing committed, safe to retry the whole submission" — trading the orphan-receipt risk for a
  whole-submission retry requirement instead.

---

## 11. Recommended Approach

Per task instruction, evaluating strictly from evidence gathered above:

**Option A (all four steps in one transaction):** would close the specific 817-row failure pattern observed in
§8 (E) — if the receipt insert fails, the whole line rolls back, and the user/system knows nothing was recorded,
so a full resubmission is unambiguous and safe (no orphan, no risk of double-posting since nothing posted).
Tradeoff (already identified in the Remediation Plan, still valid): couples the least-safe operation in the chain
(receipt numbering, still racy per §4/§10) into the same transaction as the accounting postings, meaning a
receipt-numbering collision now aborts an otherwise-correct accounting entry and forces the whole invoice line to
be resubmitted from scratch — a real UX/throughput cost, but a *bounded and safe* one (nothing is left in an
inconsistent state).

**Option B (receipt outside the transaction), as currently proposed:** **NOT safe to ship as-is.** Per the task's
required exact phrasing: **OPTION B HAS AN UNMITIGATED RECEIPT-ORPHAN RISK.** The specific recovery mechanism that
would be required to make it safe, and that does **not** currently exist anywhere in this codebase, is:
1. A detection mechanism — e.g. a `sp_Missing_MoneyReceipt`-style catch-up procedure, structurally analogous to
   `sp_Missing_CustomerPaymentPosting` (§7), that finds `tblCustPayDetail` rows with no corresponding
   `tblMoneyReceipt.CustPayDetailsId` reference (the exact query already designed and run in §8 (A)/(E) could be
   the seed for this procedure's `SELECT`).
2. An idempotent regeneration step — **must not** be a naive re-call of `sp_MRCP_MoneyReceiptMasterInsert` with
   the same detail-id CSV, because (per §4/§9) that could create a **second** receipt referencing the same
   payment detail(s) — the exact "duplicate receipt for same payment" pattern that is currently at zero
   occurrences (§8 (D)) precisely because nothing retries today. The regeneration step needs its own guard (e.g.
   "does *any* receipt already reference this `CustPayDetailId`" — the same check as query (D) — run before
   inserting) that the current SP does not have.
3. A fix to the receipt-numbering race itself (§4/§10) — the Remediation Plan's own §8 proposal (a locked counter
   table row) is still the right design and is unaffected by which transaction option is chosen; without it, both
   the recovery job in (1) and normal live traffic will keep producing occasional duplicate numbers (§8 (C)
   already shows this happening under ordinary load, no recovery job needed to trigger it).

**Recommendation: keep receipt outside the transaction (Option B) only if items 1-3 above ship as part of the same
phase, not as a "later" follow-up.** If Phase 2 cannot commit to shipping the recovery mechanism in the same
release, **Option A is the safer default**, accepting its UX cost, because it has no orphan-risk gap to leave
open in production — and the current data (§8) shows this is not a theoretical corner case: it is already
happening at a rate of roughly 50-60 orphaned receipts per month since 2025-06-24 (1,181 orphans over ~15 months,
INFERRED average rate — not a verified monthly trend, just a rough order-of-magnitude derived from the total count
and elapsed time).

**On duplicate prevention during retry specifically:** any recovery job must check "does a receipt already
reference this `CustPayDetailId`" (query pattern (D)) before inserting — this is a new check, not present in
`sp_MRCP_MoneyReceiptMasterInsert` today, which only guards against the same user re-submitting within 30 seconds,
not against "this specific payment detail already has a receipt."

**On accounting consistency:** since journal posting already committed before the receipt step runs (in both
Option A's "roll back everything together" and Option B's "leave posting standing" models, the *posting* itself
either happens-and-stays or never-happens — a receipt failure under Option A also rolls back the posting, so
accounting stays consistent either way; the receipt is a downstream document, not a ledger entry). This confirms
the Remediation Plan's underlying instinct (receipt is "furthest from accounting truth") is directionally correct
— but that same fact is exactly why an orphaned receipt is *tolerable to leave for later reconciliation* only if a
reconciliation mechanism actually exists, which it currently does not.

---

## 12. Open Questions

- **The two `NULL`-`MoneyReceiptNo` rows (§8 (C))** are not explainable from `sp_MRCP_MoneyReceiptMasterInsert`'s
  current logic, which always computes a number before inserting. Whether these came from a different/older
  insert path, a manual data-fix, or some other mechanism was **not traced further** in this investigation
  (UNKNOWN — would require either an audit trail this repo does not appear to have, or interviewing whoever
  created those two specific rows).
- **The 218 orphaned receipt-detail references (§8 (B))** — whether these are all explained by
  `sp_MRCP_DeletePayment` deleting the underlying `tblCustPayDetail` row after a receipt already referenced it, or
  some other cause, is **INFERRED, not proven** — no audit/history table was found to confirm the deletion
  timeline against the receipt's creation timeline.
- **SQL Server Agent jobs (`msdb.dbo.sysjobs`/`sysjobsteps`)** were not queried in this pass — **NOT TESTED**. Given
  no receipt-related stored procedure exists at all (§6), a job could not meaningfully invoke recovery logic that
  isn't there, so this gap does not change the verdict, but it remains formally unverified whether some external
  (non-SQL-Agent) scheduled process — e.g. a Windows Task Scheduler job, a separate service — might call
  `sp_MRCP_MoneyReceiptMasterInsert` or something equivalent from outside this codebase. No such process was found
  in-repo, but "not in this repo" is not the same as "does not exist somewhere in this environment" — flagged as
  residual uncertainty (UNKNOWN, out of the read-only-repo-plus-DB scope of this task).
- **Whether the 1,181 orphan rows (§8 (A)) are evenly distributed over time or clustered** (e.g. around a specific
  deploy, a specific batch process, or a specific user) was not analyzed — only the aggregate count and a recency
  sample were pulled. A deeper time-series breakdown would help confirm whether this is steady background noise
  or was made worse by a specific change (**NOT TESTED**, would need a dedicated query beyond this task's scope).

---

## 13. Production Risk

**CONFIRMED, not hypothetical:** the money-receipt-orphan failure mode already exists in production data today —
1,181 payments with no receipt since the feature went live, 817 of which already posted to the general ledger with
no receipt, and 2 confirmed duplicate receipt numbers including one proven-concurrent collision. This is ongoing
(recent rows within days of this investigation), not a one-time historical event.

Proceeding with Phase 2's Option B **exactly as currently scoped in the Remediation Plan (transaction around
Master+Detail+Posting, receipt insert left outside with no new recovery mechanism)** would not make this risk
worse in kind — the orphan-receipt failure mode already occurs today under the current fully-autocommit design —
but it would also **not fix it**, and shipping a transaction-boundary change elsewhere in the same chain without
addressing the one gap this task was specifically asked to verify would be a missed opportunity to close a
confirmed, currently-active, unmitigated data-integrity gap in the same release. The recommended path (§11) is to
either ship Option A, or ship Option B together with the three-part recovery mechanism described in §11 — not
Option B alone.

---

## Verdict Line

**OPTION B HAS AN UNMITIGATED RECEIPT-ORPHAN RISK**
