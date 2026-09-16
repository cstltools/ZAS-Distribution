# PaymentTranscationNew2 — Remediation Plan

**Status: PLANNING DOCUMENT ONLY.** No code, stored procedure, schema, or data was changed to produce this
document. Every recommendation below is a proposal for a future implementation phase, never something performed
here. No live database connection was available; anything that would require query execution, execution plans,
or production row data is explicitly labeled **NOT YET RUN / NOT MEASURED**. This plan is strictly derived from
`docs/audit/PaymentTransactionNew2_Investigation.md` and `docs/audit/PaymentTransactionNew2_Performance_Findings.md`
(the "Investigation" and "Performance Findings" docs below), plus direct re-reads of the source files those docs
cite, performed only to verify the architecture facts the plan depends on — no new defects were hunted for, and
no finding from those docs is re-litigated.

---

## 1. Executive Summary

`PaymentTranscationNew2.aspx` → `saveButton_ClickNew` → `PaymentTranscationDal.SaveCustPaymentNew2` has two
confirmed P0 defects (a swapped-field data-corruption bug, and a receipt document that can be lost after money is
already recorded) and four confirmed P1 defects (partial multi-invoice commits, a silently-discarded journal
result, an unwired double-click guard, and TOCTOU duplicate checks). This plan designs fixes for all of them plus
the P2 performance/security items, without performing any of the fixes.

The centerpiece architectural decision is the transaction boundary (§4). Verification against the source (not
just the Investigation doc) confirms: every SP call in the live save chain — master insert, detail insert, the
compensating delete, journal posting, and the receipt insert — routes through `ClsCommonInternalDAL`
(`aInternalDal`/`aCommonInternalDal` in `PaymentTranscationDal`), whose `SaveAction`/`UpdateAction`/`DeleteAction`
→ `ExecuteNonQueryAction` hardcodes `DatabaseFactory.CreateDatabase("SolutionConnectionString" + "SSIDB")`
(`Library.DAL/InternalCls/ClsCommonInternalDAL.cs:826`) — i.e. every call in this specific chain targets the same
connection string / same physical database. Separately, `PaymentTranscationDal` already holds a `DB_Manager
aDbManager` field (`PaymentTranscationDal.cs:20`) that **does** support a shared transaction
(`DB_Manager.CreateConnection` opens one connection and calls `BeginTransaction()`;
`CloseConnection` commits it — `Library.DAL/MAIN_FUNCTION/DB_Manager.cs:24-48`) — but this field is used **only**
by `GenerateBatch()` and `LockProcess()` in the same class, never by `SaveCustPaymentNew2` or any method in its
call chain. So a transaction-capable pattern already exists in this exact codebase, one class away, unused by the
path that needs it — this is a reuse job, not new infrastructure.

**Recommended transaction design (§4): Design B — TransactionScope (or an explicit shared `SqlConnection`/
`SqlTransaction`, mirroring the existing `DB_Manager` pattern) around Master→Detail→Journal-Posting only, with
Money-Receipt-Insert deliberately kept outside that scope and money-receipt failure surfaced as a distinct,
retryable error** — not folded into one all-five-calls transaction. Reasons, in brief: (a) all five calls are
confirmed same-database/same-connection-string, so no MSDTC promotion is expected; (b) the codebase already ships
`sp_Missing_CustomerPaymentPosting`, a cursor-based catch-up job that finds `tblCustPayDetail` rows with no
matching `CSTL_ACCDB..tblDebitCreditVoucherMaster` row and re-runs the posting SP for them
(`docs/database/stored-procedures-part2.md:6559-6620`) — direct evidence this system already treats journal
posting as eventually-consistent/retryable-after-the-fact, not something that must hard-rollback the payment if
it fails; (c) money-receipt generation is the one step furthest from "accounting truth" (§7/§9 of the
Investigation) and its own dedup guard already treats "no receipt this call" as an expected outcome — collapsing
its failure into a full five-step rollback would undo an already-correct financial posting merely because a
document-numbering step raced.

**P0-2 fix in one line:** in `Solution.Web/SInventory_UI/PaymentTranscationNew2.aspx.cs:477-479`, retarget the
second assignment from `detail.CustomerMasterId = ...key["BatchId"]...` to `detail.BatchId = ...key["BatchId"]...`
— see §3.

**Phase 1 scope:** the P0-2 one-line field fix and wiring the already-written `preventDoubleClick()` JS to
`saveButton`'s `OnClientClick` — both zero-behavior-risk, code-only changes deployable independently of the
transaction work in later phases.

---

## 2. Confirmed Root Causes

All items below are restated from the Investigation/Performance Findings docs as given facts, per task
instructions — not re-derived here except where marked "(re-verified)" after a direct source re-read performed
for this plan.

| ID | Root cause | Evidence |
|---|---|---|
| P0-1 | No shared transaction across master/detail/journal-posting/receipt inserts; each is an independent autocommit call | `PaymentTranscationDal.cs:143-164` (re-verified); `.aspx.cs:531-544` (re-verified) |
| P0-2 | `detail.CustomerMasterId` assigned twice; second assignment should target `detail.BatchId` | `.aspx.cs:473-479` (re-verified verbatim, see §3); `CustPaymentDetail.cs:18-19` (re-verified: `BatchId` property exists, unused by this mapping) |
| P1-1 | Mid-loop failure `break`s instead of `continue`; earlier invoices stay committed, later ones never attempted | `PaymentTranscationDal.cs:122-138` (re-verified) |
| P1-2 | `PaymentJournalPosting`'s bool return discarded; `UpdateAction` swallows exceptions internally | `PaymentTranscationDal.cs:134` (re-verified — statement result not assigned); `ClsCommonInternalDAL.cs:656-670` (re-verified: `UpdateAction` catch logs via `ClsErrorLogger.Log`, returns `false`, caller ignores it) |
| P1-3 | `preventDoubleClick()` defined but not wired to `saveButton`'s `OnClientClick` | `PaymentTranscationNew2.aspx:264-272,719-736` |
| P1-4 | SP-level 30s/300s guards are check-then-insert, no `UPDLOCK`/`HOLDLOCK`, no unique constraint | `docs/database/stored-procedures-part2.md:6494-6556,7871-7930,7950-8047` |
| — | Receipt numbering `RCPT-YYMMDD` + same-day `COUNT(*)`, non-atomic | `stored-procedures-part2.md:6517-6527` (re-verified) |
| — | One new `tblCustomerPay` master row per invoice line, not one master shared across N details | `PaymentTranscationDal.cs:99-141` (re-verified) |
| — | `sp_Sales_CustomerPaymentPosting` aggregates `tblCustPayDetail` independent of `@CustPayId`; unconditional unrelated `UPDATE` on every call | Investigation §5 (DB-1), §11 (MD-3) |
| — | Most dropdown/search reads route through generic `ExecuteAllSqlQueryByStoreProcedure` wrapper SP — structural performance concern, NOT MEASURED | `ClsCommonInternalDAL.cs:296-317,608-629` (re-verified: used by `DataContainerDataTable`, `LoadDropDownValue`, `DataContainerDataReader`, and most `*Command(string)` single-string-arg overloads) |
| — | Receipt report lookup has no ownership/company filter on `MoneyReceiptId` | `BatchWiseMoneyReceiptReportViewer.aspx.cs:19-32`; `BatchWiseCollectionReportDal.cs:78-117` |
| — | Print-preview join uses `LIKE '%,...,%'` against a CSV column instead of a real key join | `BatchWiseCollectionReportDal.cs:104` |

### Architecture facts re-verified directly for this plan

1. **Same DB/connection across the whole live save chain (confirmed).** `SaveCustomerPaymentMaster` →
   `sp_PMNT_CustomerPaymentMasterInsert`, `SaveCustomerPaymentDetail` → `sp_PMNT_CustomerPaymentDetailInsert`,
   `DeletePaymentMasterInfoNew` → `sp_MRCP_DeletePayment`, `PaymentJournalPosting` →
   `sp_Sales_CustomerPaymentPosting`, and the final `SaveAction(... "sp_MRCP_MoneyReceiptMasterInsert" ...)` call
   are all issued via `aInternalDal`/`aCommonInternalDal` (both `ClsCommonInternalDAL` instances,
   `PaymentTranscationDal.cs:16-17`), whose `SaveAction`/`UpdateAction`/`DeleteAction` all funnel through the
   private `ExecuteNonQueryAction` (`ClsCommonInternalDAL.cs:816-865`), which hardcodes
   `DatabaseFactory.CreateDatabase("SolutionConnectionString" + "SSIDB")` at line 826 — no parameter varies this.
   All five calls target the identical connection-string key.
2. **`DB_Manager` exists, supports a shared transaction, and is unused by this chain (confirmed).**
   `PaymentTranscationDal` declares `DB_Manager aDbManager = new DB_Manager();` (`PaymentTranscationDal.cs:20`).
   `DB_Manager.CreateConnection` opens a `DbConnection` and immediately calls `dbConnection.BeginTransaction()`
   (`DB_Manager.cs:24-34`); every `*Action` method inside `DB_Manager` executes against that same
   `dbTransaction` (`DB_Manager.cs:65,79,170,189,218,237,258`); `CloseConnection` commits the transaction and
   closes the connection (`DB_Manager.cs:36-48`). Grep of `PaymentTranscationDal.cs` shows `aDbManager` is called
   only from `GenerateBatch()` (`:443,454`) and `LockProcess()` (`:534`) — both unrelated payment-batch-header
   methods, not `SaveCustPaymentNew2`, `SaveCustomerPaymentMaster`, `SaveCustomerPaymentDetail`,
   `PaymentJournalPosting`, or `DeletePaymentMasterInfoNew`. **Conclusion: a transaction-capable DB layer already
   exists in this exact class; wiring the save chain through it is a refactor of existing code, not new
   infrastructure.** Caveat (confirmed by re-read, worth flagging for implementers): `DB_Manager`'s own
   `ExecuteNonQueryAction` catch block (`DB_Manager.cs:91-99`) rolls back and closes the connection but does not
   rethrow or set an error flag the caller can see beyond `pk=0` — if reused verbatim this swallows errors the
   same way `ClsCommonInternalDAL` does today (see P1-2); the recommended reuse in §4 calls for surfacing this,
   not carrying the swallow-forward forward.
3. **No cross-server/linked-server hop found (confirmed for the 5 SPs in this chain, and broadly).** Grep across
   both recovered SP-body documents for `OPENQUERY`, `sp_addlinkedserver`, and "linked server" returned zero
   matches. `sp_Sales_CustomerPaymentPosting` and `sp_Missing_CustomerPaymentPosting` reference
   `CSTL_ACCDB..tblDebitCreditVoucherMaster`/`Detail` using two-part cross-database names, not four-part
   linked-server names. `Solution.Web/Web.config` (the one active, uncommented connection-string block) shows
   `SolutionConnectionStringSSIDB` and `SolutionConnectionStringERPDB` pointing at the **same `Data Source`**
   (e.g. `TOWSIF\MSSQLSERVER2019`), different `Initial Catalog` only — i.e. `CSTL_ACCDB`/`CSTL_ERPDB` access from
   inside these SPs is same-instance, cross-database, not cross-server. Cross-database (same-instance) queries
   executed inside one SQL Server connection do not by themselves require MSDTC; MSDTC promotion under
   `System.Transactions.TransactionScope` is normally triggered by having two or more **separate ADO.NET
   connections** enlisted simultaneously, or a connection to a genuinely different server. This is an informed
   inference from the connection-string and SP-body evidence, not a live-tested fact — see the Open Questions
   in §16 for what should be confirmed in staging before relying on it.
4. **None of the 5 SPs in the chain wrap themselves in a transaction (confirmed, restated from Investigation
   §8).** All five recovered bodies contain no `BEGIN TRAN`/`COMMIT`/`ROLLBACK`/`TRY`/`CATCH`.
5. **Parameter shape confirms Money Receipt takes a set, Journal Posting takes one value (confirmed).**
   `sp_MRCP_MoneyReceiptMasterInsert`'s `@CustPayDetailsId nvarchar(max)` (re-read at
   `stored-procedures-part2.md:6494-6556`) is populated from `string.Join(",", savedDetailIds)`
   (`PaymentTranscationDal.cs:145,152`) — a CSV of every `CustPayDetailId` saved in the current submission's loop,
   i.e. Money Receipt is written once per **submission**, referencing potentially many detail rows.
   `PaymentJournalPosting(int custPayId, int invoiceId)` → `sp_Sales_CustomerPaymentPosting @CustPayId,
   @InvoiceId` (`PaymentTranscationDal.cs:80-89,134`) takes exactly one `CustPayId`/`InvoiceId` pair and is
   called once per loop iteration (once per invoice line, i.e. once per master row created) — Journal Posting
   is per-master, Money Receipt is per-submission. This shape difference is load-bearing for §6.
6. **Journal-posting failure is already treated as eventually-consistent elsewhere in this codebase (confirmed,
   new evidence for this plan).** `sp_Missing_CustomerPaymentPosting` (`stored-procedures-part2.md:6559-6620`) is
   a standalone catch-up procedure: it selects every `tblCustPayDetail` row processed on/after 2025-04-25 with no
   matching `CSTL_ACCDB..tblDebitCreditVoucherMaster` row (`TableID = 2017`), and re-runs
   `sp_Sales_CustomerPaymentPosting @InvoiceId, @CustPayId` for each one in a cursor. This procedure's sole
   purpose is repairing payments whose journal posting never happened — direct evidence the system already
   expects and tolerates "payment committed, journal posting pending/missing," with a reconciliation path already
   built for it (see §5).
7. **BUG-ACC-002 pattern (from `docs/audit/accounting-integration-audit.md`, cross-referenced per task
   instructions).** The audit documents the identical discarded-boolean pattern at three other unrelated posting
   call sites (`DestroyProductApproval.aspx.cs:107`, `SampleProductrIssue.aspx.cs:953`,
   `StockConditionFreeze.aspx.cs:540`) — this page's P1-2 is not an isolated slip but a systemic pattern across
   the codebase's accounting integration, reinforcing that "surface + flag for reconciliation" (matching the
   `sp_Missing_CustomerPaymentPosting` precedent) is more consistent with how this system already operates than
   introducing a first-of-its-kind hard rollback-on-journal-failure behavior for this one screen.

---

## 3. P0 Fix Design

### P0-2 — `CustomerMasterId`/`BatchId` swap

**Confirmed current code** (`Solution.Web/SInventory_UI/PaymentTranscationNew2.aspx.cs:473-479`):

```csharp
detail.CustomerMasterId = !string.IsNullOrWhiteSpace(Convert.ToString(key["CustomerMasterId"]))
   ? Convert.ToInt32(key["CustomerMasterId"])
   : 0;

detail.CustomerMasterId = !string.IsNullOrWhiteSpace(Convert.ToString(key["BatchId"]))
  ? Convert.ToInt32(key["BatchId"])
  : 0;
```

**Proposed minimal correction** — retarget only the left-hand side of the second assignment:

```csharp
detail.CustomerMasterId = !string.IsNullOrWhiteSpace(Convert.ToString(key["CustomerMasterId"]))
   ? Convert.ToInt32(key["CustomerMasterId"])
   : 0;

detail.BatchId = !string.IsNullOrWhiteSpace(Convert.ToString(key["BatchId"]))
  ? Convert.ToInt32(key["BatchId"])
  : 0;
```

One token changed (`detail.CustomerMasterId` → `detail.BatchId` on the left of the second statement). No new
variables, no control-flow change, no other line touched.

**Origin trace (DataKey → grid → property):** `key` is `selectedOrderGridView.DataKeys[i]`
(`.aspx.cs:467`), a `DataKey` populated from the grid's `DataKeyNames` for `selectedOrderGridView` — both
`CustomerMasterId` and `BatchId` are already distinct DataKey fields on that grid (confirmed indirectly: the
first assignment already reads `key["CustomerMasterId"]` and a separate `key["BatchId"]` is read one statement
later — the grid clearly carries both as independent columns/keys; the bug is purely in the C# assignment target,
not in how the grid populates its keys).

**Consumption trace (property → SP parameter → column):** `SaveCustPaymentNew2`
(`PaymentTranscationDal.cs:105-106`) reads `aCustPayment.CustomerMasterId = custPaymentDetail.CustomerMasterId;`
and `aCustPayment.BatchId = custPaymentDetail.BatchId;` per loop iteration, then `SaveCustomerPaymentMaster`
(`:242-263`) binds `@CustomerMasterId` and `@BatchId` as separate `SqlParameter`s into
`sp_PMNT_CustomerPaymentMasterInsert`, which (per Investigation §5, confirmed by the recovered SP body) writes
into `tblCustomerPay.CustomerMasterId` and `tblCustomerPay.BatchId` as separate columns. The fix makes both
columns receive their correctly-named source value for the first time; `BatchId` (the property) exists on
`CustPaymentDetail` today (`Library.DAO/SInventory_Entities/CustPaymentDetail.cs:19`, re-verified) but is never
written by the code-behind before this fix — the fix is exactly "start using an already-declared, already-wired
property correctly," not adding anything new.

**Business-semantics guardrail:** this fix changes exactly one thing — which value lands in
`tblCustomerPay.BatchId` going forward (the real batch id, instead of always 0) — and restores
`tblCustomerPay.CustomerMasterId` to the real customer id (instead of the batch id). It does not touch
`aCustPayment.CustomerMasterId`/`aCustPayment.BatchId` (set earlier and independently from `hdCustomerId.Value`/
`batchDropDownList.SelectedValue` at `.aspx.cs:436-442`, unaffected by this bug), does not touch any SP signature,
and does not change which SP is called, in what order, or with how many parameters. The 300-second dedup guard in
`sp_PMNT_CustomerPaymentMasterInsert` (keyed in part on `CustomerMasterId`/`BatchId`, per Investigation §10) will
start deduplicating on the *correct* customer/batch identity after this fix — a behavior improvement, not a new
behavior.

**Recommended accompanying safeguard (design-only, part of Phase 1 test plan, not a code change performed here):**
add an assertion/unit test that `CustPaymentDetail.CustomerMasterId != CustPaymentDetail.BatchId` after mapping,
for any row where the two source values legitimately differ — this was already recommended in Investigation §15.

### P0-1 — Transaction boundary design

See §4 (full design, two-plus options, recommendation with reasoning) — kept as its own top-level section per the
required document structure since it is the largest design item in this plan.

---

## 4. Transaction Architecture

### What is actually being asked

Per §2 (verified facts): all five calls in the chain (`sp_PMNT_CustomerPaymentMasterInsert`,
`sp_PMNT_CustomerPaymentDetailInsert`, `sp_MRCP_DeletePayment`, `sp_Sales_CustomerPaymentPosting`,
`sp_MRCP_MoneyReceiptMasterInsert`) hit the same connection string/database, none of the SPs self-wrap in a
transaction, and no cross-server hop was found. This removes "would this need MSDTC/cross-server plumbing" as a
blocker for wrapping some or all of these calls in one transaction. It does **not** by itself answer *which*
calls should share a transaction — that is a business-semantics question, addressed below using the evidence in
§2.6/§2.7 (the existing `sp_Missing_CustomerPaymentPosting` catch-up job and the codebase-wide BUG-ACC-002
pattern of tolerating discarded posting failures).

### Option A — One ambient `TransactionScope` (or one shared `SqlConnection`/`SqlTransaction`) around all five
calls (Master → Detail → Journal Posting → Money Receipt, per invoice line, with Money Receipt inside)

- **Mechanism:** wrap the entire loop body plus the post-loop Money Receipt call in one
  `using (var scope = new TransactionScope(...)) { ... scope.Complete(); }`, or equivalently pass one shared
  `SqlConnection`/`SqlTransaction` into every `ExecuteNonQueryAction`-equivalent call (requires adding a
  transaction-accepting overload to `ClsCommonInternalDAL`, or routing the whole chain through `DB_Manager`
  instead, per §2.2).
- **Pros:** simplest mental model — "if any step fails, nothing about this submission is left half-done,"
  directly closes P0-1's "payment committed but receipt lost" failure mode and P1-1's "some invoices posted, some
  not" failure mode in one stroke.
- **Cons:** money-receipt numbering (`RCPT-YYMMDD`+`COUNT`) is the least safe operation in the chain (own
  unresolved TOCTOU race per P1-4) — folding it into the same transaction as the accounting postings means a
  transient duplicate-receipt-number collision now aborts (and must retry) accounting entries that were otherwise
  fine, holding row locks on `tblCustomerPay`/`tblCustPayDetail` and (inside `sp_Sales_CustomerPaymentPosting`)
  `CSTL_ACCDB..tblDebitCreditVoucherMaster` open for the duration of N invoice postings plus the final receipt
  insert — lock duration grows with basket size N (confirmed unbounded per Performance Findings), increasing
  blocking risk for concurrent submitters. Also least consistent with the `sp_Missing_CustomerPaymentPosting`
  evidence (§2.6) that journal posting is treated as eventually-consistent elsewhere in this system — Option A
  would make this the one place in the codebase where a posting failure hard-blocks the whole submission.

### Option B — TransactionScope (or shared connection/transaction) around Master → Detail → Journal Posting
**per invoice line only**; Money Receipt insert stays a separate, independently-committed call outside that scope
— **RECOMMENDED**

- **Mechanism:** each loop iteration's Master insert + Detail insert + Journal Posting call share one transaction
  (commit together or roll back together); the compensating delete (`sp_MRCP_DeletePayment`) is only needed for
  cases the transaction itself doesn't already cover (see below); the post-loop Money Receipt call remains its
  own autocommit unit, exactly as today, but its failure path is redesigned (§7 of the Investigation's
  recommendation list, formalized here) to surface a distinct, actionable state instead of the current misleading
  "Data Already Exist" fallback.
- **Why per-invoice-line, not per-submission:** this preserves the existing effective atomic unit (Investigation
  Finding MD-1: one master row per invoice line, not one shared master) rather than introducing a new
  submission-level atomic unit that §6 shows is not required by any downstream consumer's parameter shape (Money
  Receipt already accepts a CSV set, unaffected by how many invoice-line transactions preceded it).
- **Why Money Receipt outside the transaction:** (1) its own SP-level dedup guard already anticipates and handles
  "no receipt created this call" as a valid outcome — that's a design already built into the SP, not something
  this plan invents; (2) receipt numbering is the one step with an unresolved, separate TOCTOU defect (P1-4) that
  §8 proposes fixing independently — until that fix lands, folding it into the payment/posting transaction only
  couples an already-broken race into every payment's critical path; (3) it lets a genuinely successful
  payment+posting commit stand on its own even if the receipt step needs to be retried, which is a strictly
  better failure mode than today's (payment stands today too, but the user is shown a wrong receipt and no
  actionable retry path — §9 covers the redesigned failure signaling).
- **Pros:** removes P1-1's partial-invoice-loss entirely (each invoice line's master+detail+posting is now truly
  atomic); removes the worst read of P1-2 (a posting failure no longer leaves an orphaned, already-"successful"-
  looking master/detail pair — the whole line rolls back and can be cleanly retried); keeps money-receipt
  generation decoupled from accounting-posting locking, limiting lock hold time to one invoice line rather than
  the whole loop; consistent with the existing `sp_Missing_CustomerPaymentPosting`-style eventual-consistency
  precedent for anything that still needs post-hoc reconciliation (now scoped only to the receipt-generation step,
  not to journal posting, since journal posting becomes atomic with the payment under this design).
- **Cons:** does not by itself give submission-level atomicity across multiple invoice lines (accepted — see §6:
  no downstream consumer requires it, and Investigation's current implementation is already per-invoice-line, not
  per-submission); requires either (a) a new transaction-aware overload on `ClsCommonInternalDAL`'s
  `SaveAction`/`UpdateAction`/`DeleteAction`, or (b) routing this one chain through the existing `DB_Manager`
  pattern (§2.2) instead — implementation detail for a later phase, not decided here.

### Option C — Keep autocommit-per-step; add a durable outbox / reconciliation-flag column + idempotent retry
(no ambient transaction at all)

- **Mechanism:** add a status column (e.g. `tblCustomerPay.PostingStatus` / `ReceiptStatus`) written after each
  step; a background job (conceptually similar to `sp_Missing_CustomerPaymentPosting`, which already exists for
  the journal-posting case) scans for rows stuck in an intermediate state and completes or compensates them.
- **Pros:** zero risk of introducing a transaction-scoping bug or unexpected lock contention; builds directly on
  a pattern (`sp_Missing_CustomerPaymentPosting`) already proven in this codebase; works even if a future SP in
  this chain turns out to reach a different server (removes the dependency on the "same connection string"
  finding in §2.1 holding forever).
  proceeding=false)
- **Cons:** does not fix P1-1 (partial invoice loss) or the "payment committed, journal posting silently failed"
  half of P1-2 at the point of failure — it only makes those states *detectable and repairable later*, not
  prevented at save time; requires new schema (a status column — explicitly a database-schema change, tagged as
  such in §13); requires a new scheduled job to be built and operated, which is materially more implementation
  and ops surface than Option B for the same practical benefit on this specific defect set, given Option B is
  already safe to do (no MSDTC per §2.3, no self-transacting SPs per §2.4 to conflict with).

### Option D — Compensating saga with an explicit status field per step

- **Mechanism:** explicit state machine (`Created` → `DetailSaved` → `Posted` → `ReceiptIssued`, or a
  `Failed`/`Compensating` branch at each step), with compensating actions defined for each transition, driven
  either synchronously in the C# save method or by a background worker.
- **Pros:** most explicit about failure semantics; most testable in isolation per state transition.
- **Cons:** highest implementation cost of the four options for a screen whose call graph (§2.5) is only 3-4 SP
  calls deep and already-same-database — a saga is designed for cross-service/cross-database consistency, which
  §2.3 shows this is not. Would be revisited only if a future change genuinely moves one of these SPs to a
  different database/server.

### Recommendation

**Option B**, for the reasons stated inline above, summarized: it is the smallest change that actually closes
P1-1 (root cause: no shared transaction) and the accounting-integrity half of P1-2, it is provably safe from an
MSDTC/cross-server standpoint given §2.1/§2.3, it reuses an existing in-codebase transaction pattern (§2.2) rather
than inventing new infrastructure, and it deliberately does not over-reach into folding the still-separately-
broken receipt-numbering race (P1-4, fixed independently in §8) into the same transaction. Option C's outbox
pattern is not rejected outright — it is the natural complement for the one thing Option B leaves outside a
transaction (money-receipt generation, §9's "receipt-without-payment" reconciliation query), and Phase 5 (§13)
already proposes read-only reconciliation queries in that spirit without requiring a new scheduled job.

---

## 5. Journal Failure Handling

**Current implementation, traced (confirmed, restated with the call path spelled out):**
`SaveCustPaymentNew2` calls `PaymentJournalPosting(custPayId, custPaymentDetail.InvoiceId)`
(`PaymentTranscationDal.cs:134`) as a bare statement — the `bool` it returns is never assigned to anything.
`PaymentJournalPosting` itself (`:80-89`) returns `aCommonInternalDal.UpdateAction("sp_Sales_CustomerPaymentPosting",
...)`. `UpdateAction` (`ClsCommonInternalDAL.cs:656-670`) calls `ExecuteNonQueryAction` inside a `try`; if that
throws (e.g. the SP errors), the `catch` logs via `ClsErrorLogger.Log("UpdateAction", ...)` and sets
`ActionStatus = false` — so even a hard SQL exception never reaches `SaveCustPaymentNew2` as an exception, only as
a discarded `false`. Net effect: a payment can be fully recorded in `tblCustomerPay`/`tblCustPayDetail` with no
corresponding GL entry, and nothing in this call chain today notices.

**Option tree:**

1. **Journal failure → full rollback of that invoice line's payment.** Under the recommended §4 Option B
   transaction (Master+Detail+Posting share one transaction per invoice line), this becomes a natural side
   effect: if `PaymentJournalPosting` returns `false` (or throws), the transaction is not completed and the
   master+detail rows for that line never commit at all — no compensating delete needed for that case; the
   existing `DeletePaymentMasterInfoNew` compensating delete becomes needed only for whatever failure modes
   remain outside the transaction boundary (in Option B, essentially none for this specific step, since Detail
   failure and Journal failure both now roll back the same transaction).
2. **Journal failure → flagged-for-retry/reconciliation, payment stands.** Keep the payment committed even if
   posting fails, but replace "silently discard the bool" with "check it; on `false`, write a status flag (e.g.
   `tblCustomerPay.PostingStatus = 'Pending'` or equivalent) for a job resembling
   `sp_Missing_CustomerPaymentPosting` to pick up" — effectively formalizing the pattern that procedure already
   implements ad hoc (by scanning for missing `CSTL_ACCDB` voucher rows) into an explicit, queryable flag instead
   of an implicit derived state.
3. **Journal failure → surfaced to user with manual retry.** Return the per-line posting result up through
   `SaveCustPaymentNew2`'s return value/an out-parameter list, and have the UI tell the user "payment recorded,
   accounting posting failed, contact support / retry" instead of the current undifferentiated
   success/"Data Already Exist" binary.

**Recommendation:** **Option 1 for the moment the transaction commits or not** (i.e., adopt §4 Option B, so a
hard journal-posting failure prevents that invoice line's payment from committing in the first place — this is
the cleanest fix for the specific "silent GL gap" failure mode), **combined with Option 2 as the safety net for
anything Option 1 cannot catch** (e.g., the posting SP "succeeds" per its return code but is later found to be
logically wrong — a class of error no transaction boundary can catch). The existing `sp_Missing_CustomerPaymentPosting`
catch-up job (§2.6) is direct evidence the second layer is already an accepted operational pattern in this
system — Phase 2/5 (§13) should extend it (or an equivalent scheduled reconciliation query, §9) to also watch for
the now-rarer case of a payment whose transaction committed but whose posting evidence is missing, rather than
inventing a new mechanism from scratch. Option 3 (user-facing manual retry) is recommended as additional
UX-layer polish, not a substitute for 1+2 — a cashier mid-transaction is not the right actor to resolve a GL
posting failure; ops/accounting reconciliation (Option 2's job) is.

---

## 6. Multi-Invoice Atomicity

**Question posed by the task:** is one-master-per-invoice-line intentional/required by downstream consumers, or
is it a defect that should be restructured?

**Evidence gathered (§2.5, re-verified from source):**

- **Money Receipt (`sp_MRCP_MoneyReceiptMasterInsert`, `@CustPayDetailsId nvarchar(max)`):** already accepts a
  CSV **set** of `CustPayDetailId` values (`PaymentTranscationDal.cs:145,152` builds this from
  `savedDetailIds`, one entry added per successfully-saved detail row across the whole loop). This SP does not
  assume, and is not broken by, N separate master rows feeding it — it was written to aggregate across a
  variable-length set from the start.
- **Journal Posting (`sp_Sales_CustomerPaymentPosting @CustPayId, @InvoiceId`):** takes exactly one
  `CustPayId`/`InvoiceId` pair, called once per loop iteration (`PaymentTranscationDal.cs:134`) — i.e. it is
  explicitly designed around one master row per invoice line, not a shared master. Restructuring to "one master
  row per submission" would require either calling this SP N times against the *same* `CustPayId` (changing its
  semantics — Investigation Finding MD-3 already shows this SP aggregates `tblCustPayDetail` by `@InvoiceId`
  independent of `@CustPayId`, so it may already tolerate that, but this is unverified/UNKNOWN territory not
  covered by the Investigation) or redesigning the SP itself (out of scope for "restructure the C# only").
- **Reports/queries joining on `CustPayId`:** the Investigation did not find any report/query in scope that
  assumes a single master row represents an entire multi-invoice submission's total (Finding MD-1 explicitly
  flags the *opposite* risk — "downstream consumers that assume `tblCustomerPay.PaymentAmount` reflects a full
  multi-invoice receipt total would be wrong" — i.e. the current one-row-per-invoice-line shape is what is
  actually true today; nothing in scope currently reads it the other way).

**Conclusion:** no source evidence in scope proves the one-master-per-invoice-line shape is "actively causing"
any of the P0/P1 defects — P0-1 (receipt can be lost) and P1-1 (partial loop failure) are both caused by the
*absence of a transaction*, not by the master/detail cardinality; fixing §4 does not require touching this
cardinality at all. Per the task's explicit instruction, **this plan does NOT recommend restructuring the
master/detail shape** — it is unusual naming/schema-reuse (Investigation Finding MD-1's own characterization:
"a flattened one-master-per-invoice-line model wearing a master/detail schema"), not a proven defect driver.

**Atomic-unit comparison (Option A/B/C, as requested), for the record:**

| Option | Description | vs. current (effectively C) |
|---|---|---|
| A — entire submission | All N invoice lines' master/detail/posting share one transaction; either all N commit or none do | Pros: strongest guarantee, matches user's mental model of "I submitted one payment." Cons: largest lock footprint (scales with N, unbounded per Performance Findings §"Payment Save"); one bad invoice line (e.g. one with a stale/duplicate SP-level guard hit) blocks every other, otherwise-good invoice line in the same submission — a regression for the common case Investigation §18 calls "presumably its dominant real-world case" (single-invoice submissions are unaffected either way, but this changes multi-invoice UX from "N independent outcomes" to "one outcome," which is a genuine behavior change requiring product sign-off) |
| B — each invoice (line) | Each invoice line's master/detail/posting is its own transaction (this is what §4 Option B recommends) | Matches current effective behavior (one master row per line) but closes the *specific* gap (no transaction at all today) without changing the unit of atomicity — lowest-risk option that still fixes P1-1/P1-2's worst symptom |
| C — each payment master | Identical to B given the current 1:1 master:invoice-line cardinality (there is currently no meaningful difference between "per invoice" and "per payment master" until/unless the cardinality changes) | This is what today's code already does, just without any transaction wrapping it |

**Recommendation: Option B/C (they coincide today)** — matches §4's recommendation and requires no schema or
cardinality change, only adding the transaction boundary. Option A is not recommended without an explicit
product decision to change multi-invoice submission semantics, which is out of scope for a defect-remediation
plan.

---

## 7. Duplicate Protection

**Current state (restated from P1-4):** three SP-level guards — `MasterProcessTime` (5-minute window on
`CreateBy, CustomerMasterId, BatchId, PaymentAmount, PaymentDate, InvoiceIdForValidation`),
`ProcessTime` (30-second window on `CustPayId, InvoiceId, PaymentAmount, AIT, Discount`), and the Money Receipt
30-second `CreateBy, MonyReceiptDate` guard — are all `IF NOT EXISTS(...) BEGIN INSERT ... END`, no
`UPDLOCK`/`HOLDLOCK`, no unique constraint.

**Design options:**

1. **Unique business-key constraint.** Add a unique index/constraint on the natural dedup key already used by
   each guard's `WHERE` clause (e.g. `(CustomerMasterId, BatchId, PaymentAmount, PaymentDate,
   InvoiceIdForValidation, CreateBy)` for the master guard, adjusted to a reasonable time-bucket if the guard's
   intent is "within N seconds," not "ever" — a pure unique constraint without a time bucket would block
   legitimate repeat payments days apart with identical amounts, which the current guard does not do). Requires
   care to preserve the *intent* (recent-duplicate suppression) rather than a blanket uniqueness that changes
   business behavior — flagged as a design risk to resolve with a product owner before implementation, not
   assumed away here.
2. **`sp_getapplock`.** Acquire an application lock keyed on a deterministic string (e.g.
   `CONCAT(CreateBy, '|', CustomerMasterId, '|', BatchId, '|', PaymentDate)`) at the start of each guarded SP,
   released at the end/on commit — serializes concurrent attempts with the same key without needing a schema
   change, and composes cleanly with §4 Option B's transaction (the lock is held for the transaction's duration
   if acquired with `@LockOwner = 'Transaction'`).
3. **Atomic `INSERT ... WHERE NOT EXISTS` under `UPDLOCK, HOLDLOCK`.** Replace each guard's plain
   `IF NOT EXISTS(...) BEGIN INSERT` with a single statement pattern that takes the lock and does the existence
   check atomically, e.g. `INSERT INTO tblCustomerPay (...) SELECT ... WHERE NOT EXISTS (SELECT 1 FROM
   tblCustomerPay WITH (UPDLOCK, HOLDLOCK) WHERE <dedup predicate>)` — closes the TOCTOU window without a new
   constraint or lock primitive beyond what SQL Server already provides.
4. **Higher transaction isolation level.** Running the guard check + insert under `SERIALIZABLE` for just that
   statement/transaction achieves a similar effect to option 3 but at coarser (range-lock) granularity and higher
   blocking risk under load — listed for completeness, not preferred over option 3.
5. **Client-supplied idempotency token.** Generate a GUID client-side (or server-side on page load) per "one
   Submit Payment attempt," store it as a hidden field, pass it through to the master-insert SP as a new
   parameter, and give it its own unique constraint. This is the only option in this list that also protects
   against the double-submit scenario at the root (§9's defense-in-depth JS fix reduces *frequency* of double
   POSTs; an idempotency key makes a double POST provably a no-op at the DB layer regardless of timing).

**Recommendation:** **Option 3 (atomic `WHERE NOT EXISTS` under `UPDLOCK, HOLDLOCK`) as the baseline fix for all
three existing guards**, since it directly closes the TOCTOU gap with the smallest change to each SP's existing
shape (same predicate, same table, no new column). **Layer Option 5 (client-supplied idempotency token) on top**
specifically for the master-insert step, since it is the one guard whose corrupted dedup key (P0-2, before the
fix) most concretely demonstrates how fragile a business-key-only guard is — an idempotency token sidesteps
"which business columns should the key include" entirely. Option 1 (hard unique constraint) is not recommended as
a first step because of the business-semantics risk noted above (must confirm with a product owner whether
same-day, same-amount repeat payments from the same customer are legitimate) — worth revisiting once that is
confirmed. Option 2 (`sp_getapplock`) is a reasonable alternative to Option 3 if the team prefers explicit locking
over the `UPDLOCK,HOLDLOCK` pattern; either is acceptable, not both.

---

## 8. Receipt Number Strategy

**Current implementation (confirmed, re-verified body at `stored-procedures-part2.md:6494-6556`):**
`@ProCount = COUNT(MoneyReceiptId) + 1 FROM tblMoneyReceipt WHERE CAST(CreateDate AS DATE) = CAST(GETDATE() AS
DATE)`, then `'RCPT-' + RIGHT(YEAR(GETDATE()),2) + FORMAT(GETDATE(),'MMdd') + CAST(1000 + @ProCount AS
NVARCHAR)` — a same-day `COUNT(*)`-based sequence with no locking hint, computed and used in the same statement
batch as the INSERT but not protected from a concurrent second execution computing the same count before either
commits.

**Design comparison (format `RCPT-YYMMDD####` preserved by all four):**

1. **Per-day SQL `SEQUENCE`.** Create one `SEQUENCE` object per day (or one sequence reset by a scheduled job at
   midnight) and `NEXT VALUE FOR` it inside the SP. Pros: SQL-Server-native, allocation is inherently
   collision-free even under heavy concurrency, no explicit locking needed in the SP body. Cons: `SEQUENCE`
   objects don't auto-reset per calendar day — needs either a nightly job to `ALTER SEQUENCE ... RESTART`, or a
   composite key (`sequence value` is global, day-prefix is derived separately) with a mapping step, adding
   minor operational surface.
2. **Locked-counter table row (`UPDLOCK, HOLDLOCK` or `sp_getapplock`).** Maintain one row per day in a small
   counter table (`tblMoneyReceiptCounter(ReceiptDate, LastSeq)`), and inside the SP:
   `UPDATE tblMoneyReceiptCounter WITH (UPDLOCK, HOLDLOCK) SET @NextSeq = LastSeq = LastSeq + 1 WHERE ReceiptDate
   = CAST(GETDATE() AS DATE)` (inserting the day's row first if absent), then use `@NextSeq` in the format string.
   Pros: closest structural match to the current `COUNT(*)`-based design (same "count so far today" mental
   model), small and easy to reason about, naturally resets per day by construction (one row per date). Cons:
   requires a new small table (schema change) and careful handling of the "first receipt of a new day" insert
   race (mitigated by wrapping the whole read-modify-write in one locked statement, not two).
3. **Identity-column mapping with a computed display format.** Let `tblMoneyReceipt.MoneyReceiptId` (already an
   identity column, confirmed via `SCOPE_IDENTITY()` at the end of the current SP body) be the true collision-free
   sequence, and derive the display number as `'RCPT-' + FORMAT(GETDATE(),'yyMMdd') + RIGHT('0000' +
   CAST(<per-day-rank> AS VARCHAR),4)` where `<per-day-rank>` is computed via `ROW_NUMBER() OVER (PARTITION BY
   CAST(CreateDate AS DATE) ORDER BY MoneyReceiptId)` at *read* time (report/print time), not at insert time —
   i.e. stop trying to pre-compute a same-day sequential display number at insert time at all, and compute it
   only when the receipt is displayed/printed. Pros: completely eliminates the write-time race since nothing
   depends on a same-day count at insert time. Cons: the largest change in behavior/shape of the four options —
   the "display number" becomes a derived/computed value rather than a stored fact, which changes how
   `MoneyReceiptNo` is read everywhere it's currently stored-and-read verbatim (out of scope to audit every such
   read site here).
4. **Transaction-safe `MAX+1` under `UPDLOCK, HOLDLOCK`.** Same idea as option 2 but reading `MAX(sequence part
   of MoneyReceiptNo)` from `tblMoneyReceipt` itself under `UPDLOCK, HOLDLOCK` instead of a separate counter
   table. Pros: no new table. Cons: requires parsing the numeric suffix back out of the existing
   `MoneyReceiptNo` string column on every insert (fragile if the format ever has an exception row), and the
   `UPDLOCK, HOLDLOCK` scan is over a growing table rather than one small counter row — worse locking
   characteristics than option 2 as `tblMoneyReceipt` grows.

**Recommendation: Option 2 (locked-counter table row).** It preserves the existing `RCPT-YYMMDD` + 4-digit
same-day-sequence format exactly as today (no downstream display/consumer change), is the smallest schema
addition (one small table), resets per day for free by keying on `ReceiptDate`, and is the most direct, minimal
fix for the specific race described in P1-4/Investigation §7 — it replaces "count all of today's rows, hope
nothing else counted at the same moment" with "atomically increment one locked row," which is a well-understood,
low-risk SQL Server pattern. Option 1 (`SEQUENCE`) is a reasonable alternative if the team prefers to avoid a new
table and is comfortable adding the daily-reset job; either is acceptable. Option 3 is not recommended as a first
step given its broader blast radius on existing readers of `MoneyReceiptNo`.

---

## 9. Double Submit Protection

**Confirmed current state (restated from P1-3):** `saveButton`'s actual `OnClientClick` is
`return confirm('Are you sure you want to Save ?');` (`PaymentTranscationNew2.aspx:725`) — no disable-on-click,
no re-entrancy guard of any kind. `preventDoubleClick(btn)` is fully defined in the page's own `<script>` block
(`:264-272`, disables the button and changes its text) but is never referenced by any control — confirmed via
grep, the only match for `preventDoubleClick` in the file is its own definition.

**Recommended minimal UI protection:** wire the existing function into the existing handler, combining both
behaviors rather than replacing one with the other:

```
OnClientClick="if (!confirm('Are you sure you want to Save ?')) { return false; } return preventDoubleClick(this);"
```

(exact expression to be finalized against `preventDoubleClick`'s real signature/return value at implementation
time — the point of this plan is the *behavior*: confirm first, then disable-on-click, not a specific string).
This is a zero-new-code change (Phase 1, alongside P0-2) — the function this wires already exists.

**Explicit statement per task instruction:** this JS-layer change is **defense-in-depth only**. It reduces the
*frequency* of accidental double-submits from human double-clicks, but does nothing for network-level retries
(a client or intermediate proxy resubmitting a POST after a timeout, with no JS execution involved), and nothing
for two genuinely different browser tabs/sessions submitting near-simultaneously. **§7's server/DB-side
idempotency design (the atomic `WHERE NOT EXISTS` + optional idempotency token) remains the authoritative
protection** — it is the only layer that is correct regardless of what happens (or fails to happen) in the
browser. Phase 1 should ship the JS fix because it is free and immediate, but it must not be treated as having
closed P1-3/P1-4 on its own.

---

## 10. Performance Plan

Per task instruction, this reuses the Performance Findings doc's own categorization verbatim rather than
inventing new numbers, and adds only a measurement plan — no index creation, no query rewrites.

### Confirmed structural issue (code-level fact, not measured live)

- Payment save (`SaveCustPaymentNew2`) issues up to `3N + 1` sequential, synchronous, unbatched round trips for N
  selected invoices — confirmed by direct code read of the loop shape (`PaymentTranscationDal.cs:99-164`); N is
  unbounded by any code-level cap.
- `sp_Sales_CustomerPaymentPosting` performs an unconditional `UPDATE tblPaymentTranscationMaster SET IsReject=1
  WHERE CreateDate < today AND IsProcess=0 AND IsReject=0` on every single call (confirmed in the recovered SP
  body) — runs once per selected invoice per save, every time, regardless of relevance to the current payment.
- `sp_MRCP_MoneyReceiptMasterInsert`'s receipt-number generation is a same-day `COUNT(*)`-based scan/sequence,
  recalculated from scratch on every call (confirmed in the recovered SP body).
- The print-preview join (`BatchWiseCollectionReportDal.cs:104`) uses `LIKE '%,' + CAST(...) + ',%'` against a
  CSV string column instead of a normalized join key — confirmed directly in the DAL source, precludes a standard
  index seek on that join leg.

### Likely issue (structural, plausible impact, not measured)

- Nearly every dropdown/search read on this page routes through the generic `ExecuteAllSqlQueryByStoreProcedure`
  wrapper SP (confirmed at `ClsCommonInternalDAL.cs:296-317,608-629`, re-verified this plan reads it is used by
  `DataContainerDataTable`, `LoadDropDownValue`/`LoadDropDownValueWithoutDataBase`, and most `*Command(string)`
  single-string overloads) — this structurally defeats SQL Server's per-logical-query plan caching since the
  optimizer sees one ad-hoc batch string per call, not a stable parameterized shape. Likely (not confirmed)
  contributor to variable/degraded query-plan reuse, especially on the invoice-search path, which the Performance
  Findings doc rates its highest-frequency read (P2).

### Not measured (no data, no live DB access — flagged, not estimated)

- Any actual millisecond latency figure for any stage in either document.
- Whether the SP-level 30s/300s duplicate guards (P1-4) actually collide under real concurrent load.
- Execution plans, logical reads, or lock/blocking behavior for any of the above.
- The cumulative cost of `sp_Sales_CustomerPaymentPosting`'s internal cursor over `CSTL_ACCDB`/`CSTL_ERPDB`
  joins.
- Connection-pool exhaustion/wait behavior under concurrent multi-invoice submissions.

### Staging measurement plan (what to measure, not what was measured)

1. **Page-load and search-postback duration** — capture via browser dev tools / server-side stopwatch around
   `Page_Load` and the invoice-search click handlers, for a representative dataset size; compare wrapper-SP
   (`ExecuteAllSqlQueryByStoreProcedure`) calls vs. any dedicated-SP calls on the same page to isolate the
   wrapper's actual overhead.
2. **Per-call SQL duration in the save chain** — instrument (e.g. `SqlConnection.StatisticsEnabled` or SQL
   Server Extended Events/Query Store) each of the `3N+1` round trips individually for N=1, N=5, N=20 selected
   invoices, to confirm/refute the assumed linear-in-N scaling and to get a first real number for typical basket
   sizes.
3. **Logical reads / execution plans** for `ExecuteAllSqlQueryByStoreProcedure`-routed dropdown/search queries vs.
   an equivalent dedicated parameterized SP, to quantify the plan-cache-defeat concern instead of leaving it
   qualitative.
4. **Blocking/locking** — run the save chain concurrently from two sessions in staging (same customer/batch, and
   different customer/batch) and observe `sys.dm_exec_requests`/`sys.dm_tran_locks` for lock waits, both under
   today's no-transaction code and under the §4 Option B transaction once implemented, to confirm the transaction
   doesn't introduce unexpected blocking.
5. **Connection-pool wait time** under a simulated burst of concurrent multi-invoice submissions, given the
   current one-connection-per-SP-call pattern (§2.1) — relevant regardless of which transaction option is chosen,
   since even Option B still opens a connection per invoice-line transaction, not one per whole submission.
6. **Transaction duration** once §4's chosen design is implemented — measure actual hold time of the
   Master+Detail+Posting transaction per invoice line in staging, to confirm it stays short (expected, since it's
   scoped to one invoice line, not the whole loop) before rolling out to production.

---

## 11. Historical Reconciliation Plan

All queries below are **example, proposed, READ-ONLY diagnostic T-SQL — NOT YET RUN.** No database connection was
available in this environment; these are for a future staging/production run by someone with DB access, to
quantify each defect's real blast radius before or after a code fix ships. Every query is a `SELECT` only —
nothing here inserts, updates, deletes, or creates an index/constraint.

```sql
-- (1) P0-2 blast radius: how many tblCustomerPay rows plausibly have CustomerMasterId
--     corrupted with a BatchId (or 0) instead of a real customer id, before the code fix ships.
--     Heuristic: CustomerMasterId that doesn't correspond to any real tblCustMaster row it should
--     match via the invoice on the linked detail, OR CustomerMasterId == BatchId (bug's literal signature).
SELECT CP.CustPayId, CP.CustomerMasterId, CP.BatchId, CP.CreateDate, CP.CreateBy
FROM tblCustomerPay CP
WHERE CP.CustomerMasterId = CP.BatchId AND CP.BatchId <> 0
   OR CP.BatchId = 0; -- BatchId always 0 today per the bug's second effect

-- (2) Payment without a receipt: tblCustomerPay/tblCustPayDetail rows whose CustPayDetailId
--     never appears in any tblMoneyReceipt.CustPayDetailsId CSV list.
SELECT CPD.CustPayDetailId, CPD.CustPayId, CPD.InvoiceId, CPD.PaymentAmount
FROM tblCustPayDetail CPD
WHERE NOT EXISTS (
    SELECT 1 FROM tblMoneyReceipt MR
    WHERE ',' + MR.CustPayDetailsId + ',' LIKE '%,' + CAST(CPD.CustPayDetailId AS VARCHAR) + ',%'
);

-- (3) Receipt without a matching payment detail (orphaned/garbage CSV reference).
SELECT MR.MoneyReceiptId, MR.MoneyReceiptNo, value AS OrphanCustPayDetailId
FROM tblMoneyReceipt MR
CROSS APPLY STRING_SPLIT(MR.CustPayDetailsId, ',') AS s
WHERE NOT EXISTS (
    SELECT 1 FROM tblCustPayDetail CPD WHERE CPD.CustPayDetailId = TRY_CAST(value AS INT)
);

-- (4) Payment without a journal entry (mirrors sp_Missing_CustomerPaymentPosting's own query,
--     restated here as a read-only diagnostic rather than a fix-it job).
SELECT CPD.CustPayId, CPD.InvoiceId, CPD.CustPayDetailId
FROM tblCustPayDetail CPD
LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM
    ON CPD.CustPayId = VM.CommonPK_ID AND VM.TableID = 2017
WHERE VM.CommonPK_ID IS NULL;

-- (5) Duplicate payments: same invoice/customer/amount within a short window (candidate P1-4 collisions).
SELECT InvoiceId, CustomerMasterId, PaymentAmount, COUNT(*) AS DupCount,
       MIN(CreateDate) AS FirstSeen, MAX(CreateDate) AS LastSeen
FROM tblCustomerPay CP
JOIN tblCustPayDetail CPD ON CPD.CustPayId = CP.CustPayId
GROUP BY InvoiceId, CustomerMasterId, PaymentAmount
HAVING COUNT(*) > 1
   AND DATEDIFF(SECOND, MIN(CP.CreateDate), MAX(CP.CreateDate)) < 60;

-- (6) Duplicate receipt numbers (candidate collisions in the COUNT(*)-based sequence, P1-4/§8).
SELECT MoneyReceiptNo, COUNT(*) AS DupCount
FROM tblMoneyReceipt
GROUP BY MoneyReceiptNo
HAVING COUNT(*) > 1;

-- (7) Invoice allocation mismatch: tblInvoice.PaymentAmount vs. the SUM of its tblCustPayDetail rows
--     (per Finding MD-3, sp_Sales_CustomerPaymentPosting is supposed to keep these in sync).
SELECT INV.InvoiceId, INV.PaymentAmount AS InvoiceRecordedAmount,
       SUM(CPD.PaymentAmount) AS DetailSum,
       INV.PaymentAmount - SUM(CPD.PaymentAmount) AS Mismatch
FROM tblInvoice INV
JOIN tblCustPayDetail CPD ON CPD.InvoiceId = INV.InvoiceId
GROUP BY INV.InvoiceId, INV.PaymentAmount
HAVING INV.PaymentAmount <> SUM(CPD.PaymentAmount);

-- (8) Customer ledger mismatch: total customer payments recorded in tblCustomerPay vs. total
--     Dr/Cr posted for that customer in CSTL_ACCDB (coarse sanity check, not a full reconciliation).
SELECT CP.CustomerMasterId,
       SUM(CPD.PaymentAmount) AS TotalRecordedPayment,
       (SELECT SUM(VD.Amount) FROM CSTL_ACCDB..tblDebitCreditVoucherDetail VD
        JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON VD.VoucherMasterId = VM.VoucherMasterId
        WHERE VM.TableID = 2017 AND VM.CommonPK_ID IN (
            SELECT CP2.CustPayId FROM tblCustomerPay CP2 WHERE CP2.CustomerMasterId = CP.CustomerMasterId
        )) AS TotalPostedVoucherAmount
FROM tblCustomerPay CP
JOIN tblCustPayDetail CPD ON CPD.CustPayId = CP.CustPayId
GROUP BY CP.CustomerMasterId;
```

Query (8) references `tblDebitCreditVoucherDetail`'s exact column names (`Amount`, `VoucherMasterId`) as a
best-effort example based on the naming pattern already confirmed for `tblDebitCreditVoucherMaster`
(`CommonPK_ID`, `TableID`) — these exact column names were **not independently re-verified** against a recovered
schema for the Detail table in this plan and should be checked against `docs/database/tables.md` before running.

---

## 12. Test Matrix

Design only — no test below was executed. Every row describes what a future tester/QA should do once the Phase 1
(and later) fixes are implemented, in a staging environment with DB access.

| ID | Scenario | Precondition | Action | Expected result | Database checks | Rollback checks |
|---|---|---|---|---|---|---|
| T01 | Normal single invoice | Valid customer, one open invoice, no recent duplicate submissions | Select 1 invoice, fill payment fields, click Submit, confirm dialog | Success message; receipt number shown matches the newly created row | `tblCustomerPay` has 1 new row with correct `CustomerMasterId`/`BatchId`; `tblCustPayDetail` has 1 matching row; `tblMoneyReceipt` has 1 new row whose `CustPayDetailsId` contains the detail id | N/A (happy path) |
| T02 | Normal multi-invoice | Valid customer, 3 open invoices | Select 3 invoices, submit once | Success message; one receipt referencing all 3 detail ids | 3 `tblCustomerPay` rows (one per invoice line, per §6), 3 matching `tblCustPayDetail` rows, 1 `tblMoneyReceipt` row with a 3-entry CSV | N/A (happy path) |
| T03 | `CustomerMasterId` correctness | Post-P0-2-fix build | Submit any payment | `tblCustomerPay.CustomerMasterId` equals the real customer id, not the batch id | Assert `CustomerMasterId != BatchId` when the two source values differ (per §3's recommended safeguard) | N/A |
| T04 | `BatchId` correctness | Post-P0-2-fix build, submission made from a specific selected batch | Submit a payment against a chosen batch | `tblCustomerPay.BatchId` equals the selected batch, not 0 | Assert `BatchId` matches `batchDropDownList.SelectedValue` at submit time | N/A |
| T05 | Receipt generation success | Normal conditions | Submit a valid payment | Receipt number displayed matches the DB row | `tblMoneyReceipt` row exists, `MoneyReceiptNo` format `RCPT-YYMMDD####` | N/A |
| T06 | Receipt insertion failure | Force `sp_MRCP_MoneyReceiptMasterInsert`'s 30s guard to fire (e.g. two submissions by the same user within 30s at the same `MoneyReceiptDate`), or simulate a hard failure | Submit twice quickly | Post-fix (§4 Option B + §7/§9): payment/posting for the second submission still commits (transaction scoped to Master+Detail+Posting only); receipt step surfaces a distinct "receipt pending, retry" state instead of showing an unrelated prior receipt | `tblCustomerPay`/`tblCustPayDetail` rows exist for both submissions; confirm whether a receipt row exists for the second submission or is flagged pending, per whichever §7/§9-informed design is implemented | Confirm no `tblCustomerPay` rollback occurred for the second submission (receipt failure should not roll back the payment under Option B) |
| T07 | Journal failure | Force `sp_Sales_CustomerPaymentPosting` to fail/throw (e.g. bad data forcing a cursor error) | Submit a payment that trips the forced failure | Post-fix (§5 Option 1): the whole invoice-line transaction (master+detail+posting) rolls back; user sees an explicit failure, not a false success | `tblCustomerPay`/`tblCustPayDetail` show no row for the failed line; no orphaned master-without-posting row | Confirm the transaction actually rolled back (no partial commit for that line) |
| T08 | Detail failure | Force `sp_PMNT_CustomerPaymentDetailInsert` to fail/dedup-guard-fire for one line in a multi-invoice submission | Submit 3 invoices where the 2nd is engineered to fail detail insert | Under §4 Option B: only that invoice line rolls back; per §6 the current per-line atomic unit is preserved — the other 2 lines are unaffected (this is the existing/expected behavior, not a regression) | Lines 1 and 3 committed normally; line 2 has no master/detail row | Confirm line 2's transaction rolled back cleanly, no partial master-without-detail row |
| T09 | Multi-invoice mid-loop failure | 3 invoices selected, 2nd engineered to fail | Submit | Per §6/current-implementation semantics: line 1 and 3 succeed independently of line 2's failure (matches T08); UI should report per-line results rather than a single pass/fail (P1-1 fix) | 2 of 3 lines committed; 1 shows a clear failure reason surfaced to the UI | Confirm no `break` causes line 3 to be skipped (P1-1's specific defect) |
| T10 | Double-click | Post-§9-fix build | Rapidly double-click Submit Payment | Second click is a no-op (button disabled after first click) or is safely absorbed by the §7 idempotency/guard layer if it does reach the server | At most one set of rows created for the submission | N/A |
| T11 | Concurrent duplicate submission | Two sessions submit the identical payment (same customer/invoice/amount) within the same second | Fire both submissions near-simultaneously (e.g. via two browser sessions or a script) | Exactly one submission results in committed rows; the other is rejected/deduped cleanly, not silently double-inserted | Only one `tblCustomerPay`/`tblCustPayDetail` set exists for the pair | Confirm the rejected attempt made no partial writes |
| T12 | Concurrent receipt generation | Two sessions each complete a valid, distinct payment at nearly the same moment | Fire both near-simultaneously | Two distinct, non-colliding receipt numbers are generated (validates §8's fix) | `tblMoneyReceipt` has 2 rows with different `MoneyReceiptNo` values, no duplicate | N/A |
| T13 | SQL timeout | Simulate a slow/blocked SP call (e.g. an artificial delay or lock held by another session) during the save chain | Submit while the simulated block is active | Under §4 Option B: the transaction times out/fails cleanly and rolls back rather than leaving a partial commit | No partial `tblCustomerPay`/`tblCustPayDetail` row for the timed-out attempt | Confirm rollback, not partial commit |
| T14 | Browser retry | Simulate a client-side timeout/retry that resends the same POST | Submit, then force a duplicate POST of the same form state | With §7's idempotency token (or the atomic guard), the retried POST is recognized as a duplicate and does not create a second set of rows | Only one set of rows exists for the two POSTs | N/A |
| T15 | Refresh after success | Successful submission completed, page refreshed (F5) | Refresh the page after a successful save | No re-submission occurs (standard ASP.NET postback-vs-refresh semantics); if the browser prompts to resubmit and the user accepts, §7's guard should still prevent a duplicate | No new rows created by the refresh alone | N/A |
| T16 | Already processed duplicate | An invoice already has a committed payment | Attempt to submit a payment for the same invoice again | Guard rejects or clearly flags as already-paid, distinct from a "recent duplicate" message (addresses the P0-1 misleading-message finding) | No new `tblCustomerPay`/`tblCustPayDetail` row; UI message is accurate about *why* (already paid, not "data already exists" ambiguity) | N/A |
| T17 | Rollback verification | Any of T06/T07/T08/T13's forced-failure scenarios | Trigger the failure, then query the DB immediately after | For every forced-failure case, confirm zero partial rows (no master without detail, no detail without posting evidence, under the new transaction boundary) exist for the failed unit | Direct row-count/existence checks per the specific scenario's tables | Confirms §4's transaction design actually holds under real failure injection, not just the happy path |

---

## 13. Deployment Strategy

Phased, each phase independently deployable and separately verifiable in staging before promotion, per the
Implementation Phasing structure required by the task (§ numbering below matches that required section).

### Phase 1 — P0 fixes (code-only)
- P0-2: retarget `detail.BatchId` assignment (`.aspx.cs:477-479`) — **code-only**.
- P1-3: wire `preventDoubleClick()` to `saveButton`'s `OnClientClick` — **code-only**.
- Rationale for bundling P1-3 into Phase 1: zero new code, zero business-logic risk, ships alongside P0-2 for a
  fast, low-risk first release.
- Deployment: standard code deploy; no DB downtime, no schema change, no data migration. Safe to deploy ahead of
  every later phase.

### Phase 2 — Transaction / reliability (code-only, plus verification against existing stored procedures)
- Implement §4 Option B (Master+Detail+Posting share a transaction per invoice line) — **code-only** (adds a
  transaction-aware overload to `ClsCommonInternalDAL`, or routes the chain through `DB_Manager`, per §2.2 — no
  SP signature changes required, since all three SPs already accept the same parameters they do today).
- Implement §5's journal-failure propagation (surfaced via the transaction's own rollback, per Option 1) —
  **code-only**.
- Redesign the money-receipt failure UI message (§9 of the Investigation, formalized in §4/§9 here) to
  distinguish "genuine recent duplicate" from "this submission's receipt needs retry" — **code-only**.
- Deployment: requires staging verification of T06/T07/T08/T13/T17 (§12) before promotion, specifically to
  confirm no unexpected MSDTC promotion or blocking under concurrent load (§10's staging measurement plan,
  items 4-6).

### Phase 3 — Concurrency / idempotency (stored-procedure, plus optional database-schema for the idempotency
token)
- §7: rewrite the three existing TOCTOU guards (`MasterProcessTime`, `ProcessTime`, Money Receipt's 30s guard) to
  the atomic `WHERE NOT EXISTS (... WITH (UPDLOCK, HOLDLOCK))` pattern — **stored-procedure**.
- §7: add a client-supplied idempotency token parameter to `sp_PMNT_CustomerPaymentMasterInsert` plus a backing
  unique index — **stored-procedure** + **database-schema**.
- §8: implement the locked-counter table for receipt numbering — **database-schema** (new small table) +
  **stored-procedure** (rewrite `sp_MRCP_MoneyReceiptMasterInsert`'s numbering logic).
- Deployment: requires T11/T12/T14/T16 (§12) verification under real concurrent load in staging.

### Phase 4 — Performance (measurement only in this phase; any resulting rewrite is a later, separately-scoped
phase)
- Execute §10's staging measurement plan (items 1-6) — no code/SQL/schema change in this phase itself, purely
  instrumentation and data collection.
- Any structural fix that measurement justifies (e.g. migrating specific hot dropdown/search reads off
  `ExecuteAllSqlQueryByStoreProcedure` to dedicated parameterized SPs) is explicitly **out of scope for this
  plan** and would be its own future phase, scoped only after real numbers exist — consistent with the task's
  "no query rewrites" constraint for this document.

### Phase 5 — Historical reconciliation (data-repair, informed by read-only queries; no query in §11 is itself a
repair)
- Run §11's 8 read-only diagnostic queries in staging (and, with appropriate access controls, production) to
  quantify each defect's real blast radius — **read-only, no tag needed (no change performed)**.
- Any resulting data correction (e.g. re-deriving `CustomerMasterId` for rows corrupted by P0-2 before the fix
  shipped, where derivable from the linked invoice's `tblInvoice.CustomerMasterId`) is **data-repair**, planned
  and executed only after Phase 5's diagnostic queries confirm scope and a safe derivation exists — not designed
  in detail here since it depends on Phase 5's actual findings, which don't exist yet (no live DB access).

---

## 14. Rollback Strategy

- **Phase 1:** trivially revertible — a single-file code change (P0-2) and a markup attribute (P1-3); standard
  redeploy of the previous build reverts both instantly. No data was touched, so no data rollback is needed.
- **Phase 2:** revertible by redeploying the pre-Phase-2 build; because the transaction wraps only Master+Detail+
  Posting (not Money Receipt), rolling back the code does not orphan any data differently than today's already-
  existing autocommit behavior — worst case reverts to the current (already-known) partial-commit risk profile,
  not a new one. Recommend keeping the T17 rollback-verification tests (§12) in the regression suite going
  forward so any future change to this chain re-proves the transaction still rolls back cleanly.
- **Phase 3:** the SP rewrites (`UPDLOCK, HOLDLOCK` pattern) are logically equivalent to the current guards under
  non-concurrent conditions, so reverting to the previous SP body is safe if a problem is found; the idempotency-
  token unique index should be added as `WITH (ONLINE = ON)` if the edition supports it, and can be dropped
  without data loss if rolled back (it is an added constraint on already-existing columns, not a new column that
  other code depends on, unless the idempotency-token column itself is added — in which case the column can
  remain unused/nullable after a rollback with no harm). The receipt-numbering counter table can be dropped
  without touching `tblMoneyReceipt` if rolled back, at the cost of reverting to the old `COUNT(*)` logic in the
  SP.
- **Phase 4:** measurement-only; nothing to roll back.
- **Phase 5:** diagnostic queries are read-only, nothing to roll back. Any later data-repair step (out of this
  plan's scope, per §13) would need its own explicit before/after snapshot and rollback script at the time it is
  actually designed — not fabricated here.

---

## 15. Risks

- **Transaction-scope risk (Phase 2):** although §2.1/§2.3 give strong same-database/no-linked-server evidence
  against MSDTC promotion, this was not confirmed against a live SQL Server instance in this environment — Phase
  2 must re-verify this in staging (§16 Open Questions) before production rollout; if promotion does occur
  unexpectedly, `TransactionScope` should be swapped for the explicit shared-`SqlConnection`/`SqlTransaction`
  variant of Option B, which cannot promote to MSDTC by construction.
- **Business-semantics risk (§7 Option 1, unique constraint):** a hard unique constraint on the dedup key could
  reject legitimate same-day, same-amount repeat payments from the same customer if that is a real business
  scenario — this plan recommends the atomic `WHERE NOT EXISTS` pattern specifically to avoid this risk, but any
  future move to Option 1 needs product sign-off first.
- **Multi-invoice atomicity change risk (§6):** if a future decision is made to move from Option B/C
  (per-invoice-line) to Option A (whole-submission) atomicity, that is a genuine user-facing behavior change
  (one bad invoice line now blocks all others in the same submission) requiring explicit product sign-off — not
  a pure technical refactor.
- **`DB_Manager` reuse risk (§2.2):** `DB_Manager`'s own error handling swallows exceptions similarly to
  `ClsCommonInternalDAL` today (catch, rollback, close, no rethrow) — if Phase 2 reuses `DB_Manager` verbatim
  instead of building a corrected transaction-aware path, it would silently reintroduce a version of P1-2 at the
  transaction-management layer. The recommended Phase 2 implementation should explicitly surface failures rather
  than copy this existing swallow-pattern.
- **Receipt-numbering migration risk (§8):** introducing a new counter table (Option 2) needs a one-time seed
  value derived from the current max sequence number already issued for "today," to avoid generating a
  colliding receipt number on the very first call after deployment — this seeding step is a data-repair/setup
  action for Phase 3, not designed in exhaustive detail here since it depends on the exact deployment moment.
- **Unverified column names in §11 query (8):** flagged inline in §11 — `tblDebitCreditVoucherDetail`'s exact
  schema was not independently confirmed in this plan and must be checked before running.
- **Scope boundary risk:** this plan, per its own instructions, does not address P2/P4 items (report-viewer
  ownership check, generic wrapper-SP migration, dead legacy `saveButton_Click` path) in the same depth as the P0/
  P1 items — they remain valid follow-up work per the Investigation's own §16 Recommended Remediation Plan, not
  re-scoped here.

---

## 16. Open Questions

1. **MSDTC behavior under real load:** confirm in staging that wrapping Master+Detail+Posting in one
   `TransactionScope` does not promote to MSDTC in practice, under both sequential and (if applicable) any
   parallelized future use of these calls — §2.3's inference is strong but not live-tested.
2. **Does `sp_Sales_CustomerPaymentPosting` tolerate being called multiple times against the same `@CustPayId`
   safely** (Finding MD-3 suggests its `tblInvoice` update is idempotent per `@InvoiceId`, but this was not fully
   proven for every side effect the procedure has, e.g. the `CSTL_ACCDB` voucher insert) — relevant if a future
   retry mechanism (§5 Option 2) ever needs to safely re-invoke it, beyond the existing
   `sp_Missing_CustomerPaymentPosting` job which already assumes this is safe.
3. **What should "already processed duplicate" (T16) actually communicate to the user** — this plan assumes a
   clearer message than today's "Data Already Exist" is desirable, but the exact wording/UX is a product decision
   not made here.
4. **Same-day, same-amount repeat payment legitimacy** (blocks a confident recommendation for §7 Option 1) —
   needs a product-owner answer before a hard unique constraint is considered.
5. **Historical corruption scope** — genuinely unknown until §11's queries are actually run in staging/production;
   this plan cannot estimate row counts without DB access.
6. **Owner/timeline for the P2/P4 items** (report-viewer ownership filter, wrapper-SP migration, dead legacy path)
   — out of this plan's phase sequence (§17) but flagged as needing its own decision, since they were rated P2/P4
   by the Investigation and are not blocking for the P0/P1 phases here.
7. **Whether any other page/screen besides `PaymentTranscationNew2.aspx` calls
   `PaymentTranscationDal.SaveCustomerPaymentMaster`/`SaveCustomerPaymentDetail`/`PaymentJournalPosting`
   directly** — if so, those callers would also need the Phase 2 transaction wiring to get the same protection;
   this plan's source re-read did not exhaustively search for other callers beyond confirming the ones the
   Investigation already named.

---

## 17. Recommended Implementation Sequence

1. **Phase 1 (P0 fixes)** — ship first, independently: P0-2 field-swap fix + P1-3 double-click JS wiring.
   Lowest risk, highest urgency (P0-2 corrupts data on every normal save today), zero dependency on any other
   phase.
2. **Phase 2 (transaction/reliability)** — §4 Option B transaction boundary + §5's journal-failure propagation +
   the receipt-failure-message redesign. Depends on Phase 1 only in the sense that both touch the same class of
   files; no hard technical dependency, but recommended to land after Phase 1 so the transaction work is being
   tested against already-correct `CustomerMasterId`/`BatchId` data rather than still-corrupting writes.
3. **Phase 3 (concurrency/idempotency)** — §7's atomic guards + idempotency token, §8's receipt-numbering fix.
   Recommended after Phase 2 because the locked-counter/idempotency work is easiest to validate once the
   transaction boundary (Phase 2) is already in place and stable — testing concurrent duplicate submission
   (T11/T14) is more meaningful once T06/T07/T08/T17 (Phase 2's tests) are already green.
4. **Phase 4 (performance)** — measurement only; can run in parallel with Phases 2-3 in staging, since it does
   not touch the save-chain code path itself, only instruments it. Any resulting structural fix is future,
   separately-scoped work.
5. **Phase 5 (historical reconciliation)** — run last (or in parallel, since the queries are read-only and
   non-blocking) specifically because §11's most valuable query (P0-2 blast radius) is most informative once
   Phase 1 has shipped and the corruption has stopped growing — quantifying a still-growing problem is less
   useful than quantifying a now-capped one.
