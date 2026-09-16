# Trade Material Transfer — Remediation Plan

Planning only. No source, schema, stored procedure, data, index, or configuration changes were made. Nothing was committed.

This plan builds directly on `docs/audit/TradeMaterialTransfer_Final_Investigation.md` (TMT-001 … TMT-014) and the four findings restated in the remediation request (TMT-001…TMT-004 in that message). To avoid ambiguity between the two numbering sets, this document uses the **investigation's IDs** (TMT-004/005/006/008/009/010/013) when citing evidence, and calls out the remediation-request's TMT-001…004 inline where they map.

---

## 1. Confirmed Findings (carried forward, re-verified against schema)

| ID (investigation) | Finding | Re-verified against |
|---|---|---|
| TMT-004/005 | `sp_Process_TradeMaterialTransferFullProcess` deducts source stock and writes destination ledger rows with no idempotency guard, while the GL step has one (`NOT EXISTS` on `CommonPK_ID`+`TableID`) | `docs/database/stored-procedures-part2.md:8398-8517` (steps 1–4 vs. step 5 at line 8522) |
| TMT-008/009 | Source stock deduction (`tblDCStore.StockQty`) has no `WHERE StockQty >= @Qty` guard and no rows-affected check; no locking hint | Same SP, lines 8413-8437; schema confirms `tblDCStore` (`docs/database/tables.md:2481-2533`) has only a clustered PK on `DCStoreId`, no CHECK constraint, no unique key on `(ProductCode, ComUnitId)` |
| TMT-006/013 | `TransferCode` generated via unlocked `MAX(RIGHT(...,4))+1`, no unique index | `docs/database/stored-procedures-part1.md:1990-2000`; `docs/database/tables.md:5732-5767` confirms only the clustered PK exists on `tblTradeMaterialTransfer` |
| TMT-010 | No effective double-click protection on Save/Approve | `TradeMaterialTransfer.aspx:307-309`, `TradeMaterialTransferApproval.aspx` (`btnSubmit`) — `confirm()` dialog only, no disable-on-click, no server-side dedup |

New schema facts gathered for this planning pass (read-only, from `docs/database/tables.md`):

- `tbl_INV_WeightedAvg` (source-side, `SalesRollDB_ZAS`) and its `CSTL_ERPDB` counterpart: PK is `Weighted_Avg_Id` (bigint identity) only. Columns `Mst_Trans_Id`, `Trans_Id`, `Trans_Type_Id` exist and are exactly what the SP already populates with `@TradeMaterialTransferId` / `@TradeMaterialTransferId` / `1010` (source) or `2010` (destination) — **no unique constraint** enforces one-row-per-transfer on this table today.
- `tbl_INV_BatchWiseStock`: PK `Batch_Stock_Id` only, FK-by-convention to `Weighted_Avg_Id`. No uniqueness beyond PK.
- `tblDCStore`: PK `DCStoreId` (not flagged Identity in the doc), `StockQty` is `decimal(18,0)` — **zero decimal places**, while `tblTradeMaterialTransfer.TransferQty` and the SP's `@TransferQty` are `decimal(18,3)`. This is a secondary, pre-existing precision mismatch (fractional transfer quantities silently round when written to `StockQty`) — noted for completeness in §16 but is **not** part of the P0/P1 scope requested; flagged as a P3 follow-up only.
- The SP's initial source-row pick (`SELECT TOP 1 ... FROM tblDCStore WHERE ProductCode=@FromProductCode AND ComUnitId=@FromLocationId AND StockQty >= @TransferQty`) has **no `ORDER BY`** — row selection is non-deterministic when more than one batch row qualifies. This matters for the concurrency design in §5/§12 and is called out there.
- No `ApprovalStatus` values other than `'Posted'` (set by Insert/Update) and `'Approved'` (set by the approval SP) were found anywhere in the reviewed C# (`grep` for `ApprovalStatus == /= '...'` across all `.cs` files returned only comparisons to `"Approved"`). No `'Rejected'`/`'Cancelled'` status exists in code today.

---

## 2. Root Causes

1. **The approval procedure was written assuming it is called exactly once per transfer.** Nothing enforces that assumption — it is a missing precondition, not a logic error in the arithmetic itself.
2. **The status flip (`ApprovalStatus = 'Approved'`) is an unconditional write, not a conditional claim.** It records the outcome but does not *gate* entry into the rest of the procedure.
3. **The stock deduction is a "read intent, then write" pattern spread across a `SELECT TOP 1` (unordered) and a separate unconditional `UPDATE`**, rather than a single atomic conditional write.
4. **`TransferCode` is derived from `MAX()` of existing data with no lock scope and no uniqueness backstop** — a read-then-compute-then-insert pattern with a window for two sessions to compute the same value.
5. **UI-level protections (confirm dialogs) were mistaken for sufficient protection** — they reduce accidental clicks but do not close the server-side race.

None of these are schema-design flaws in the sense of "wrong columns" — the columns needed for a correct fix (`TradeMaterialTransferId`, `ApprovalStatus`, `Mst_Trans_Id`/`Trans_Type_Id`, `TransferCode`) **already exist**. This is a logic/guard gap, not a missing-data-model gap.

---

## 3. Transfer Identity (per instructions, Section 1)

```text
Transfer identity: dbo.tblTradeMaterialTransfer.TradeMaterialTransferId (int, IDENTITY, clustered PK)
```

Justification:
- It is the only immutable, database-enforced-unique identifier for a transfer (`PK__tblTrade__06E200F1DBD6F5F0`).
- It is **already** the key the GL idempotency check uses (`CommonPK_ID = @TradeMaterialTransferId AND TableID = 2026`) — reusing it keeps all three idempotency guards (stock, ledger, GL) aligned on the same business key, which is a requirement from §9 of the request ("determine whether its existing idempotency key is aligned with the transfer identity" — it is, so the new guards should match it, not diverge onto `TransferCode`).
- It is already the value the SP writes into `tbl_INV_WeightedAvg.Mst_Trans_Id`/`Trans_Id` on both the source and destination side.
- `TransferCode` is explicitly **unsuitable** as the identity for this purpose: it is nullable, has no uniqueness guarantee today (TMT-006), and is a *display/document* number, not a surrogate key — using it for idempotency would make the idempotency guard only as strong as a value this plan is separately trying to fix.

---

## 4. Idempotency Strategy

### 4.1 Guard location (per request §4)

**Both, with the database as the authoritative layer:**

- **A — Inside `sp_Process_TradeMaterialTransferFullProcess` (PRIMARY, correctness-critical).** This is the only place that can atomically guarantee "exactly once" because it is the only component that can take a lock and hold it for the duration of the whole write set inside one transaction.
- **B — In `TradeMaterialTransferApproval.aspx.cs` (SECONDARY, UX-only).** Re-checking `ApprovalStatus` before calling the SP (e.g. skip rows in the checked set that are no longer `'Posted'`, refresh the grid after each batch) reduces wasted round-trips and gives a better message, but **must never be relied on as the correctness boundary** — the grid can always be stale by the time the click lands (this is exactly TMT-010/TMT-004's root cause).

This satisfies the explicit requirement: *"The primary protection MUST be server/database-side. Client-side protection is only supplementary."*

### 4.2 The atomic claim mechanism (per request §2, Case A/B/C)

Do **not** use `SELECT status` followed by a later `UPDATE` (the request explicitly forbids this — it is exactly the race being fixed). Instead, make the **existing** status-flip statement (already present as step 1 of the procedure) do double duty as the atomic claim, by adding a `WHERE` predicate and checking `@@ROWCOUNT`:

```text
Conceptual replacement for the procedure's current step 1:

  UPDATE dbo.tblTradeMaterialTransfer
  SET ApprovalStatus = 'Approved', ApprovedBy = @ApprovedBy, ApprovedDate = GETDATE(),
      UpdateBy = @ApprovedBy, UpdateDate = GETDATE()
  WHERE TradeMaterialTransferId = @TradeMaterialTransferId
    AND ApprovalStatus <> 'Approved';        -- <-- the guard

  IF @@ROWCOUNT = 0
  BEGIN
      -- Someone already approved this transfer (or it never existed / was concurrently
      -- claimed by another session). Nothing has been written yet in this call.
      -- Roll back cleanly and report "already processed" rather than proceeding.
      ROLLBACK TRANSACTION;
      RETURN;   -- (or RAISERROR with a distinct, catchable message/number so the
                 -- caller can tell "already processed" apart from a real failure)
  END
```

**Why this is safe under concurrency without SERIALIZABLE/table locks (Case C):**

An `UPDATE` statement takes an exclusive row lock on the row(s) it modifies as part of evaluating and applying the write, under any isolation level, including the default READ COMMITTED already in use. If two sessions (User A, User B) issue this `UPDATE` for the same `TradeMaterialTransferId` at the same instant:

1. One of them (say A) acquires the row lock first and proceeds; its predicate (`ApprovalStatus <> 'Approved'`) is true (row is still `'Posted'`), so it updates 1 row and continues into stock/ledger/GL processing, still inside the same open transaction/row lock.
2. B's `UPDATE` blocks on the same row (lock wait) until A's transaction commits or rolls back.
3. If A commits: B's `UPDATE` re-evaluates its predicate against the now-committed row — `ApprovalStatus` is now `'Approved'`, so the predicate is false, B's `UPDATE` affects 0 rows, `@@ROWCOUNT = 0`, and B exits cleanly without ever touching stock/ledger/GL.
4. If A rolls back (e.g. insufficient stock, see §5): B's `UPDATE` re-evaluates against the rolled-back state (`'Posted'` again), succeeds, and becomes the one session that actually processes the transfer.

This gives exactly the required semantics — **"Only ONE must process the transfer. The second execution must safely detect that processing has already occurred"** — using ordinary row-level locking that SQL Server already performs for any `UPDATE`, with **no `SERIALIZABLE`, no table lock, no `sp_getapplock`, no cursor** (satisfies §18's "do not introduce unnecessary locking/serialization").

### 4.3 Existing status model reuse (per request §3)

`'Approved'` already exists and already means "fully processed" everywhere else in the codebase (`TradeMaterialTransfer.aspx.cs:282-294` and `TradeMaterialTransferList.aspx.cs` both treat `ApprovalStatus == "Approved"` as "locked, cannot edit/delete"). **Reuse it as the idempotency gate — no new status value is introduced.** This directly satisfies "prefer reusing an existing status" and "do not invent a new status without evidence": there is no evidence any new status is needed, since the binary `Posted → Approved` model is already sufficient to express "claimed / not yet claimed."

---

## 5. Stock Safety Strategy

### 5.1 Schema actually inspected (per request's explicit instruction not to blindly reuse the example SQL)

`tblDCStore` — relevant columns: `DCStoreId` (PK), `ProductCode`, `ComUnitId`, `StockQty` (decimal(18,0)). The SP currently selects a candidate row via `ProductCode` + `ComUnitId` (+ `StockQty >= @TransferQty` on the first attempt), then updates by `DCStoreId`. The atomic guard therefore needs to key off **`DCStoreId`** (the row actually chosen), not `ProductCode`/`ComUnitId` directly, because multiple `tblDCStore` rows can share the same `ProductCode`/`ComUnitId` (batch rows).

### 5.2 Proposed deduction pattern

```text
Conceptual replacement for the procedure's current step 2:

  -- (row selection unchanged in spirit, but see §5.3 for the ORDER BY fix)
  SELECT TOP 1 @SourceDCStoreId = DCStoreId, @SourceBatchNo = BatchNo, ...
  FROM SalesRollDB_ZAS..tblDCStore
  WHERE ProductCode = @FromProductCode AND ComUnitId = @FromLocationId AND StockQty >= @TransferQty
  ORDER BY ReceiveDate ASC, DCStoreId ASC;   -- deterministic, oldest batch first (FIFO) — see §5.3

  IF @SourceDCStoreId IS NULL
  BEGIN
      -- No single row currently has enough stock. Per request §6/§16, this must be a
      -- hard failure, not a fallback to "any row regardless of quantity" (the current
      -- unsafe fallback is removed).
      RAISERROR('Insufficient source stock for TradeMaterialTransferId %d', 16, 1, @TradeMaterialTransferId);
  END

  UPDATE SalesRollDB_ZAS..tblDCStore
  SET StockQty = StockQty - @TransferQty
  WHERE DCStoreId = @SourceDCStoreId
    AND StockQty >= @TransferQty;             -- re-checked atomically at write time

  IF @@ROWCOUNT = 0
  BEGIN
      RAISERROR('Source stock changed concurrently; transfer not processed for TradeMaterialTransferId %d', 16, 1, @TradeMaterialTransferId);
  END
```

Both `RAISERROR` calls above are inside the same `BEGIN TRY` the procedure already has, so the existing `BEGIN CATCH ... IF @@TRANCOUNT>0 ROLLBACK ... RAISERROR` unwinds everything already written earlier in the same call (the status claim from §4.2 included) — satisfying the requirement: *"Master/processing = rollback, Source stock = unchanged, Destination ledger = rollback, GL = rollback."*

### 5.3 Deterministic row selection — a design decision this plan surfaces but does not resolve unilaterally

The **current** `SELECT TOP 1 ... WHERE StockQty >= @TransferQty` (no `ORDER BY`) already has no defined row order — this is a pre-existing ambiguity, not something this remediation introduces. For the atomic per-row guard in §5.2 to actually deliver "one succeeds, one fails" under concurrency (§7 of the request), **repeated calls for the same product/location must consistently target the same candidate row(s) in the same order** — otherwise two concurrent transfers could each grab a *different* row that individually has enough stock, both succeed, and the *aggregate* available-for-that-product-location stock could still be misrepresented to a human reading the summed total (though no single row would go negative, since each row's own guard holds).

This plan recommends `ORDER BY ReceiveDate ASC, DCStoreId ASC` (oldest-batch-first / FIFO), because:
- It is the natural inventory-accounting convention (and batches carry `ExpDate`, implying FIFO/FEFO intent already exists in the data model).
- It makes row selection deterministic and repeatable, which is a prerequisite for reasoning about concurrent behavior at all.

**This is flagged as an item needing business/inventory-owner confirmation before implementation** — this plan does not have evidence of an existing FIFO/FEFO policy documented elsewhere for `tblDCStore` consumption, only that the columns needed to support one already exist. See §16 "Open Questions."

### 5.4 Concurrency outcome (per request §7)

| Scenario | Outcome under §4.2 + §5.2 |
|---|---|
| Stock 100, A=80 and B=50 concurrently, same row | One `UPDATE` succeeds (100→20), the other's `WHERE StockQty >= @Qty` fails post-commit re-evaluation → `@@ROWCOUNT=0` → `RAISERROR` → full rollback for that session only. Final stock: **20**, never negative. |
| 100 → 100 (exact) | `StockQty >= @TransferQty` → `100 >= 100` true → succeeds, final `StockQty = 0`. |
| 100 → 101 | `100 >= 101` false → the `SELECT TOP 1 ... WHERE StockQty >= @TransferQty` finds no row → hard failure before any write (per §5.2, no unsafe fallback). |
| 0 → 1 | Same as above — no qualifying row, hard failure, no negative stock. |

---

## 6. Transaction Strategy

The procedure's existing `BEGIN TRY / BEGIN TRANSACTION ... COMMIT` / `BEGIN CATCH ... ROLLBACK ... RAISERROR` wrapper is **not weakened or restructured** — it is reused as-is. The only changes are:

1. Step 1 (status update) gains the `WHERE ApprovalStatus <> 'Approved'` guard + `@@ROWCOUNT` check (§4.2), still the first statement inside the same transaction.
2. Step 2 (stock deduction) gains the `WHERE StockQty >= @TransferQty` guard + `@@ROWCOUNT` check (§5.2), still the second statement, still inside the same transaction.
3. Steps 3–4 (ledger inserts) are **unchanged in their own logic** — they become implicitly idempotent as a side effect of step 1's guard (the procedure body physically cannot reach them a second time for an already-approved transfer, because it already returned/rolled back at step 1). Optional defense-in-depth for steps 3–4 is discussed in §7.
4. Step 5 (GL) is **left completely unchanged** (§8 below).

### 6.1 Ordering (per request §10)

```text
1. Idempotency claim (conditional status UPDATE + @@ROWCOUNT)   <-- gate; exits here if already processed
2. Source stock validation/deduction (conditional UPDATE + @@ROWCOUNT)
3. Destination ledger (INSERT, now unreachable twice due to #1)
4. GL posting (INSERT, unchanged NOT EXISTS guard, also now unreachable twice due to #1)
5. COMMIT
```

Order rationale: the claim must be **first**, before any stock is touched, so that "transfer marked processed but stock deduction failed" cannot happen — if stock deduction fails at step 2, the `RAISERROR` there is caught by the existing `CATCH`, which rolls back the *entire* transaction, **including the step-1 status claim**. Status and stock can never diverge because they share one transaction; the claim being first only determines *who gets to try*, not what gets *kept* on failure.

---

## 7. Destination Ledger Idempotency (per request §8)

**Business key inspected:** `tbl_INV_WeightedAvg` (both `SalesRollDB_ZAS` source-side and `CSTL_ERPDB` destination-side) has no unique constraint beyond its own surrogate `Weighted_Avg_Id`. The columns that *identify* a posting are `Mst_Trans_Id` (= `@TradeMaterialTransferId`), `Trans_Id` (= `@TradeMaterialTransferId`, duplicated), and `Trans_Type_Id` (`1010` for source FG-out, `2010` for destination RM-in) — these are exactly the columns the SP already populates, confirming `TradeMaterialTransferId` (not `TransferCode`, not a new "detail id" — there is no detail table, §4 of the investigation) is the correct business key here too, consistent with §3 of this plan.

**Chosen guard:** rather than adding a second, independent `NOT EXISTS` check on `tbl_INV_WeightedAvg` before each insert (which would duplicate logic and could itself race the same way the GL check theoretically could — see §8), this plan relies on the **single upstream claim gate in §4.2** to make steps 3–4 unreachable more than once per transfer. This is simpler and sufficient given the ordering in §6.1.

**Optional defense-in-depth (P1, not required for correctness given §4.2, but cheap insurance against a future refactor that reorders the procedure):** add `AND NOT EXISTS (SELECT 1 FROM tbl_INV_WeightedAvg WHERE Mst_Trans_Id=@TradeMaterialTransferId AND Trans_Type_Id=1010)` immediately before the source insert, and the destination equivalent with `Trans_Type_Id=2010`, mirroring the GL pattern exactly. Recommended as a secondary safety net, not the primary fix.

---

## 8. GL Idempotency (per request §9)

**Inspected, unchanged.** Existing guard:
```sql
IF @TransferValue > 0 AND NOT EXISTS (
    SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster
    WHERE CommonPK_ID = @TradeMaterialTransferId AND TableID = 2026
)
```
(`docs/database/stored-procedures-part2.md:8522-8525`). The key (`CommonPK_ID = TradeMaterialTransferId`) is already aligned with §3's chosen transfer identity. **No change required or recommended.** It is retained verbatim; the new claim gate in §4.2 makes it doubly unreachable on a second call, but the existing guard was already correct on its own and should not be touched.

---

## 9. TransferCode Generation Strategy (per request §11)

**Current format (must be preserved):** `TMT-` + `YYMM` (2-digit year + 2-digit month of `@EntryDate`) + a running number, right-padded/derived from `RIGHT(TransferCode, 4)` of existing codes in that month's prefix, starting at `1001`. Example shape: `TMT-25091001`.

**Options evaluated:**

| Option | Fit | Notes |
|---|---|---|
| Plain `IDENTITY` on `TransferCode` | Poor | `TransferCode` is a formatted string (`TMT-YYMM####`), not a raw integer — an `IDENTITY` column can't produce this format directly. |
| `SEQUENCE` object (per-month or global) | **Good** | A `SEQUENCE` (e.g. `seq_TradeMaterialTransfer_<YYMM>` created on demand, or one global sequence combined with the existing prefix logic) gives a race-free `NEXT VALUE FOR` call with no explicit locking needed — SQL Server serializes sequence allocation internally. Matches the "preserve format" requirement by keeping the `TMT-YYMM` + number construction in the procedure, just replacing the unlocked `MAX()+1` read with a `NEXT VALUE FOR` call for the numeric part. Would need one sequence per `YYMM` prefix (to keep numbers resetting monthly as today), which is a small operational overhead (a sequence must exist before first use each month) — or a single global sequence if monthly reset is not actually a hard business requirement (needs confirmation, see §16). |
| Locking the existing `MAX()+1` read (`UPDLOCK, HOLDLOCK` on the range) | **Good, simplest, format-preserving with zero schema change** | `SELECT @NextSeq = ISNULL(MAX(...), 1000) + 1 FROM dbo.tblTradeMaterialTransfer WITH (UPDLOCK, HOLDLOCK) WHERE TransferCode LIKE @Prefix + '%';` — the `UPDLOCK` takes out an update lock immediately (rather than a shared lock that could deadlock against another session's own `UPDLOCK` read), and `HOLDLOCK` (=`SERIALIZABLE` for this one range scan) holds it until the transaction ends, forcing a second concurrent call to block until the first commits its insert, then recompute against the now-including-the-new-row table. This is a **scoped, single-range lock**, not a full-table or full-transaction `SERIALIZABLE` — satisfies §18's "if locking is required, scope it to the rows actually being processed." Requires wrapping `spInsertTradeMaterialTransfer` in its own explicit transaction (it currently has none) so the lock is held for the duration of the number-compute + insert. |
| Unique constraint + retry loop | Workable but adds application-level retry complexity | Would need the caller (`InsertTradeMaterialTransfer` DAL / `ExecuteNonQueryAction`) to catch a unique-violation and resubmit — more moving parts than the SP-side lock above for the same guarantee. |
| Derive `TransferCode` from `TradeMaterialTransferId` (e.g. `TMT-<Id>`) | Would trivially guarantee uniqueness | **Rejected** — changes the existing human-readable format (loses the `YYMM` grouping that the business format currently encodes), so it does not satisfy "the existing TransferCode format must be preserved if possible." Not recommended unless the business explicitly agrees to change the code format, which is out of scope for this plan to decide. |

**Recommendation:** the `UPDLOCK, HOLDLOCK` scoped-range-lock approach — it is the smallest change (no new schema object), preserves the exact existing format, and satisfies the "no unnecessary locking" constraint because the lock is scoped to rows matching `TransferCode LIKE @Prefix + '%'` (effectively a handful of rows per month), held only for the duration of one INSERT, not the whole approval workflow. The `SEQUENCE`-per-month option is a reasonable alternative if the team prefers to avoid range-lock semantics; both are presented so the decision can be made with the format-preservation constraint already satisfied either way.

This guarantees: "Concurrent Save A, Concurrent Save B cannot create the same TransferCode" — the second session's locked range read blocks until the first commits, then sees the first's new row and computes the next number correctly.

---

## 10. Unique Constraint on TransferCode (per request §12 — investigate only, do not create)

- **Existing duplicates:** **unknown** — could not be queried (no live DB access in this environment, same blocker as investigation §15/TMT-014). Must be checked with `SELECT TransferCode, COUNT(*) FROM dbo.tblTradeMaterialTransfer GROUP BY TransferCode HAVING COUNT(*) > 1;` before a unique index is added, because SQL Server will refuse to create a unique index over data that already violates it.
- **Business requirement:** `TransferCode` is used as the primary human-facing document number for this workflow (shown in the grid, the details modal, and used as `@ReferenceNo` = `'TMT-' + CAST(TradeMaterialTransferId AS NVARCHAR(30))` inside the approval SP — note this `@ReferenceNo` is actually a *second*, `TradeMaterialTransferId`-based reference string distinct from `TransferCode` itself, used only for ledger `Ref_No` values, not for uniqueness). Given it's presented to users as a document number, uniqueness is a reasonable expectation even though nothing currently enforces it.
- **Existing FK references:** none found — `tblTradeMaterialTransfer` has no documented incoming FKs to `TransferCode` specifically (all cross-table references in this workflow go through `TradeMaterialTransferId`, per §3). Adding a unique constraint on `TransferCode` therefore carries **no FK cascade risk**.
- **Recommendation if approved:** `CREATE UNIQUE NONCLUSTERED INDEX UX_tblTradeMaterialTransfer_TransferCode ON dbo.tblTradeMaterialTransfer(TransferCode) WHERE TransferCode IS NOT NULL;` (filtered to allow multiple `NULL`s, since the column is nullable and older/edge-case rows might not have a code) — **only after** the duplicate-check query above returns zero rows, and only after §9's generation fix is deployed and has been running clean for a period, to avoid a migration blocked by legacy duplicates.
- **Migration risk:** low if no duplicates exist; if duplicates are found, they must be manually reconciled (rename one of each duplicate pair, e.g. append `-DUP`) — a data-correction task, out of scope for this read-only plan, requiring business sign-off on which of two same-coded transfers keeps the "canonical" code.

**This plan does not create the constraint.** It documents the exact DDL and the precondition check to run first.

---

## 11. Double-Click / UI Protection (per request §13)

```text
Client-side button disabling = UX protection (reduces accidental repeat submits, improves perceived responsiveness)
Database/SP idempotency      = correctness protection (the §4 claim gate + §5 stock guard are what actually prevent corruption)
```

The remediation's correctness does **not** depend on the UI change. Recommended UI change (P2, cosmetic/UX only):
- `TradeMaterialTransfer.aspx` `submitButton` and `TradeMaterialTransferApproval.aspx` `btnSubmit`: append client-side disable to the existing `OnClientClick="return confirm(...)"` (e.g. `if (confirm('...')) { this.disabled = true; return true; } return false;`), so a second physical click before the async postback returns doesn't even reach the server. This is purely additive UX polish layered on top of the already-correct server-side guard — not a prerequisite for it.

---

## 12. Master/Detail Save Phase (per request §14 — inspected, not changed)

- **Master ID generation:** safe — `SCOPE_IDENTITY()` via `OUTPUT` parameter (investigation §3). No change needed.
- **Detail MasterId:** N/A — no detail table exists (investigation §4); one row *is* the whole transfer.
- **Zero-detail prevention:** N/A for the same reason.
- **Partial detail failure:** N/A for the same reason.
- **Duplicate submission at Save:** possible only through the `TransferCode` race (§9) — a duplicate **master row** (two distinct `TradeMaterialTransferId`s, each internally complete and valid) can be created by a genuine double-click on `submitButton`, since `spInsertTradeMaterialTransfer` has no dedup check of its own (by design — a user is allowed to create multiple distinct transfers). This is not itself a defect; it becomes visible only as a duplicate `TransferCode`, which §9's fix resolves. **No other change to the Save phase is proposed** — it is otherwise correct and is not touched by this remediation.

---

## 13. Approval Flow — Exact Sequence (per request §15, mapped from evidence, not assumed)

```text
Pending Transfer (ApprovalStatus = 'Posted')
      ↓  TradeMaterialTransferApproval.aspx btnSubmit_Click, per checked row
      ↓  TradeMaterialTransferDal.ApproveTradeMaterialTransfer(id, approvedBy)
      ↓  ClsCommonInternalDAL.UpdateAction → ExecuteNonQueryAction
      ↓  sp_Process_TradeMaterialTransferFullProcess  (ONE atomic transaction, post-fix as designed in §4-§8)
      ↓
      ├─ Claim gate (status 'Posted' → 'Approved', conditional, §4.2)
      ├─ Source stock deduction (conditional, §5.2)
      ├─ Destination ledger insert (unchanged logic, now single-fire due to claim gate, §7)
      └─ GL voucher insert (unchanged, already guarded, §8)
      ↓
Processed/Approved (ApprovalStatus = 'Approved', ApprovedBy/ApprovedDate set)
```

This **is** the target architecture described in the request's §15 — confirmed by evidence to already be structurally correct (one atomic transaction covering all four effects), just missing the guard conditions that make repeated entry safe. No restructuring of the sequence is needed, only the guards described above.

**Note (not part of the P0/P1 scope, flagged for awareness):** `TradeMaterialTransferApproval.aspx.cs:btnSubmit_Click` never inspects `actionRadioButtonList.SelectedValue` — it always calls `ApproveTradeMaterialTransfer` regardless of which action the user selected (see investigation §19/test #19). If a "Reject" action is expected to exist and *not* move stock, that is a separate, unaddressed gap outside this remediation's P0/P1 scope; recommend a follow-up investigation before assuming Reject works correctly.

---

## 14. Failure Matrix (post-fix expected behavior)

| Scenario | Expected (post-fix) |
|---|---|
| Normal approval | One successful processing: status flips once, stock deducted once, ledger written once (both sides), GL posted once |
| Double-click approval | First call processes fully; second call's claim gate (§4.2) sees `@@ROWCOUNT=0`, exits cleanly with an "already processed" outcome — no stock/ledger/GL side effects from the second call |
| Two concurrent approvers | Exactly one processes (row-lock ordering per §4.2); the other detects already-processed and exits cleanly |
| Already processed transfer (re-approval attempt later) | Claim gate blocks at step 1; no stock movement |
| Insufficient source stock | Claim gate succeeds (status would flip), stock guard (§5.2) fails → `RAISERROR` → `CATCH` rolls back **everything**, including the status flip — transfer remains `'Posted'` |
| Exact available stock | Succeeds; `StockQty` reaches exactly 0 |
| Zero source stock | No qualifying row found → hard failure before any write → full rollback |
| Detail failure | N/A (no detail table) |
| Destination ledger failure (e.g. constraint violation) | Existing `CATCH` rolls back status + source stock + anything else written so far in that call |
| GL failure | Existing `CATCH` rolls back status + source stock + destination ledger together |
| Exception after stock deduction, before COMMIT | Full rollback via existing `CATCH`; `tblDCStore.StockQty` reverts to its pre-approval value |
| Duplicate TransferCode attempt (concurrent Save) | Blocked at generation time by the scoped range lock (§9); if a unique index is later added (§10), also rejected at the database level as a second line of defense |

---

## 15. Concurrency Strategy Summary

- **Approval idempotency (§4):** single conditional `UPDATE` + `@@ROWCOUNT` on `tblTradeMaterialTransfer.ApprovalStatus`, relying on standard row-level exclusive locking under READ COMMITTED. No `SERIALIZABLE`, no table locks, no application locks.
- **Stock deduction (§5):** single conditional `UPDATE` + `@@ROWCOUNT` on the specific `tblDCStore.DCStoreId` row chosen, same locking mechanism, scoped to one row.
- **TransferCode generation (§9):** a scoped range lock (`UPDLOCK, HOLDLOCK`) limited to rows matching the current month's prefix, held only for the duration of one insert — or a `SEQUENCE` object as an alternative with no explicit locking at all.
- No cursors, no polling loops, no repeated table scans are introduced anywhere in this design. All three guards are single-statement, single-row (or narrow-range) operations, satisfying §18's performance constraint.

---

## 16. Open Questions / Items Requiring Business or DBA Input Before Implementation

These are called out explicitly rather than resolved unilaterally, per the instruction not to invent business rules:

1. **FIFO row selection (§5.3):** should source stock consumption at approval time be oldest-batch-first (`ReceiveDate ASC`)? This plan assumes yes as the safest default but has no documented policy to confirm it against.
2. **Monthly `TransferCode` reset (§9):** does the business require the running number to reset each month (as the current `TMT-YYMM####` format implies), or would a single global sequence be acceptable? Affects whether the `SEQUENCE` option needs one sequence per month (with associated creation/maintenance) or can be a single object.
3. **Unique constraint rollout (§10):** requires a live-DB duplicate check before creation; this plan could not run that check in this environment (no DB access, consistent with investigation §15/TMT-014).
4. **Cross-database transaction locality (investigation TMT-012, carried forward):** still unverified whether `SalesRollDB_ZAS`, `CSTL_ERPDB`, `CSTL_ACCDB` are co-located on one SQL Server instance. This affects whether the existing `BEGIN TRAN` in the SP is a local or (if any DB is linked-server-remote) an MSDTC-promoted distributed transaction — worth confirming before deployment, though it does not change the design above either way.
5. **`StockQty` precision (`decimal(18,0)`) vs. `TransferQty` (`decimal(18,3)`):** noted in §1 as a secondary, pre-existing issue (fractional transfers silently round on write to `tblDCStore`). Not part of the requested P0/P1 scope; flagged as a P3 candidate for a separate, smaller investigation if fractional-quantity transfers are a real business scenario.
6. **"Already processed" signaling back to the UI:** the claim-gate's early exit (§4.2) needs a way to tell `TradeMaterialTransferApproval.aspx.cs` "this one was already done" versus "this one genuinely failed" (e.g. a distinct `RAISERROR` message/number, or a return code). The exact mechanism (message text match vs. custom error number vs. output parameter) is an implementation detail to settle during actual coding, not a blocker to this plan.

None of these block producing this plan — they are implementation-detail decisions to make during the coding phase, not gaps in the diagnosis.

---

## 17. File / Database Impact Summary

No changes were made. This is the inventory of what a future implementation phase would touch.

| # | File / Object | Method / Procedure | Table(s) | Column(s) | Schema change? | Risk |
|---|---|---|---|---|---|---|
| 1 | `sp_Process_TradeMaterialTransferFullProcess` (stored procedure) | whole procedure body, steps 1–2 only | `dbo.tblTradeMaterialTransfer`, `SalesRollDB_ZAS..tblDCStore` | `ApprovalStatus`, `StockQty` | No (logic only — add `WHERE`/`@@ROWCOUNT` guards) | Medium — core correctness fix; must be tested for both success and each rollback path before deployment |
| 2 | `spInsertTradeMaterialTransfer` (stored procedure) | whole procedure body | `dbo.tblTradeMaterialTransfer` | `TransferCode` | No (logic only — add explicit transaction + `UPDLOCK, HOLDLOCK`, or add a `SEQUENCE` object — see §9 for the schema-change variant) | Low–Medium — scoped lock, but changes an existing procedure's locking behavior; needs load testing under concurrent Saves |
| 3 | `dbo.tblTradeMaterialTransfer` (table) | — | — | `TransferCode` | **Optional, P1, deferred:** unique filtered index (§10) — only after duplicate-check and after item #2 is deployed | Low if no existing duplicates; Medium (manual data cleanup) if duplicates are found |
| 4 | `Solution.Web/SInventory_UI/TradeMaterialTransferApproval.aspx.cs` | `btnSubmit_Click` | — | — | No | Low — optional: recognize the "already processed" outcome distinctly in the per-row success/fail tally for a better user-facing message (UX only, not correctness) |
| 5 | `Solution.Web/SInventory_UI/TradeMaterialTransfer.aspx` / `.aspx.cs` | `submitButton` markup | — | — | No | Low — optional client-side disable-on-click (§11), cosmetic |
| 6 | `Solution.Web/SInventory_UI/TradeMaterialTransferApproval.aspx` | `btnSubmit` markup | — | — | No | Low — same as #5 |
| 7 | `tbl_INV_WeightedAvg` (both databases) | — | — | `Mst_Trans_Id`, `Trans_Type_Id` | No schema change; **optional** P1 defense-in-depth `NOT EXISTS` guard added to procedure body (§7), not a new index | Low |

No changes to `Library.DAL`, `Library.DAO`, or `Library.BLL` C# code are required — every guard in this plan lives in the stored procedures, consistent with §4's "primary protection MUST be server/database-side" requirement. The only optional C#/markup touches (#4–#6) are UX polish, not correctness-bearing.

---

## 18. Rollback Plan (for the implementation phase, once approved)

- Every stored-procedure change is a single `ALTER PROCEDURE`; the previous version's definition should be captured (via source control / a pre-change script export) before altering, so a straight `ALTER PROCEDURE` back to the prior body is the rollback path — no data migration is needed to undo a procedure-only change.
- The `TransferCode` unique index (§10), if and when created, can be dropped (`DROP INDEX`) with no data loss, since it enforces a constraint rather than storing new data.
- No table columns are added, renamed, or dropped anywhere in this plan — every change is either procedural logic or an index, both cleanly reversible without a data-loss risk.
- Because the claim-gate and stock-guard changes only add `WHERE`/`@@ROWCOUNT` conditions to statements that already exist, a rollback restores exactly the current (pre-fix) — already-understood — behavior; it does not introduce a new failure mode to reason about.

---

## 19. Deployment Sequence

```text
1. Confirm open questions in §16 that block specific choices (FIFO order, monthly-reset requirement,
   cross-database locality) — or explicitly accept this plan's stated defaults.
2. Run the duplicate-TransferCode read-only check (§10) against a live/replica DB to know current data health.
3. Deploy the sp_Process_TradeMaterialTransferFullProcess fix (§4, §5) to a test environment;
   execute the full test plan in §20 there, including concurrency tests, before touching production.
4. Deploy the spInsertTradeMaterialTransfer fix (§9) to the same test environment; execute
   concurrent-Save tests before production.
5. Deploy both procedure changes to production together (they are independent of each other but
   both are P0/P1 and should not be staggered further once tested).
6. Re-run the §10 duplicate check; if clean, create the unique filtered index on TransferCode.
7. Optional UI polish (§11) can ship independently, before or after the above, since it carries no
   correctness dependency.
```

---

## 20. Test Plan

### TMT-REG-001 — Normal transfer
Approve one `'Posted'` transfer with sufficient stock. Verify: `ApprovalStatus='Approved'` (1 row changed), `tblDCStore.StockQty` reduced by exactly `TransferQty` on the targeted row, exactly one new row in each of source/destination `tbl_INV_WeightedAvg` (and their `tbl_INV_BatchWiseStock` children) for this `TradeMaterialTransferId`, exactly one GL voucher pair.

### TMT-REG-002 — Double-click Approve
Submit the same transfer's approval twice in immediate succession (simulate via two rapid sequential calls to `ApproveTradeMaterialTransfer` with the same id). Verify: first call succeeds with the effects of TMT-REG-001; second call reports "already processed" (via whatever signal is implemented per §16 item 6) and produces **zero** additional stock/ledger/GL rows.

### TMT-REG-003 — Concurrent Approve
Two sessions call the approval SP for the same `TradeMaterialTransferId` at effectively the same time (e.g. two connections issuing the call within the same lock-wait window). Verify: exactly one session's stock/ledger/GL writes land; the other's claim gate reports zero rows affected and it performs no writes; final `StockQty` reflects exactly one deduction.

### TMT-REG-004 — Insufficient stock
Approve a transfer where `TransferQty` exceeds every qualifying `tblDCStore` row's `StockQty`. Verify: `RAISERROR` is raised, full rollback occurs (status remains `'Posted'`, no stock/ledger/GL changes), `StockQty` values unchanged.

### TMT-REG-005 — Exact stock
`TransferQty` equals the chosen row's `StockQty` exactly. Verify: success, resulting `StockQty = 0`, never negative.

### TMT-REG-006 — Destination ledger failure
Force a failure at the destination ledger insert step (e.g. a temporarily invalid FK/type value in a test harness). Verify: source stock deduction and the status claim from the same call are both rolled back — `StockQty` and `ApprovalStatus` both revert to pre-call values.

### TMT-REG-007 — GL failure
Force a failure during GL voucher posting. Verify: status, source stock, and destination ledger writes from the same call are all rolled back together.

### TMT-REG-008 — Duplicate TransferCode concurrency
Two simultaneous Saves in the same `TMT-YYMM` prefix window. Verify: both succeed, with two distinct, sequential `TransferCode` values — no collision.

### TMT-REG-009 — Already-processed approval
Attempt to approve a transfer whose `ApprovalStatus` is already `'Approved'` (from a prior, separate approval run, not a same-instant double-click). Verify: claim gate blocks at step 1, zero stock/ledger/GL changes, clean "already processed" signal.

### TMT-REG-010 — Multiple detail rows
**N/A** — there is no detail table (investigation §4); this scenario does not apply to this workflow. Substituted verification: approve a *batch* of several distinct transfers in one `TradeMaterialTransferApproval.aspx` submission (the page's existing per-row loop, investigation §2 Stage B) and confirm each transfer is processed exactly once independently — i.e. the per-transfer claim gate does not interfere across different `TradeMaterialTransferId` values in the same batch.

---

## 21. Performance Notes (per request §18)

- No `SERIALIZABLE` transaction isolation is introduced at the transaction level — only a scoped `HOLDLOCK` on the narrow `TransferCode` prefix range during insert (§9), and ordinary row-level exclusive locks from the two conditional `UPDATE` statements (§4.2, §5.2), both of which SQL Server already takes for any `UPDATE` regardless of this fix.
- No table-level locks, no cursors, no repeated full scans are introduced. The `SELECT TOP 1 ... ORDER BY ReceiveDate ASC, DCStoreId ASC` (§5.3) is no more expensive than the current unordered `TOP 1` unless it requires a new index to sort efficiently — if `tblDCStore` has no index supporting `(ProductCode, ComUnitId, ReceiveDate)`, this ordering could add a sort operation on however many batch rows match that product/location, which is expected to be a small row count (one company's batches for one product) — not flagged as a performance risk, but worth confirming row-count expectations with whoever owns `tblDCStore` if a given product/location routinely has hundreds of open batch rows.
- Lock hold duration for both conditional `UPDATE`s is bounded by the remaining work inside the same transaction (ledger + GL inserts) — i.e. the same duration the procedure already holds locks for today, since nothing here adds new work, only guard conditions on existing writes.

---

## Recommended Implementation Order

```text
Step 1 — P0 idempotency design   : add the conditional claim-gate UPDATE + @@ROWCOUNT check
                                    to sp_Process_TradeMaterialTransferFullProcess step 1
Step 2 — P0 atomic stock deduction: add WHERE StockQty >= @Qty + @@ROWCOUNT check to step 2,
                                    remove the unsafe "any row" fallback, add deterministic ORDER BY
Step 3 — P0 stock/ledger/GL consistency: verify (via TMT-REG-001/002/003/006/007) that steps 3-5
                                    are now unreachable on a repeat call, purely as a consequence
                                    of Steps 1-2; add the optional NOT EXISTS defense-in-depth (§7)
                                    if the team wants belt-and-suspenders
Step 4 — P1 TransferCode concurrency fix: wrap spInsertTradeMaterialTransfer in an explicit
                                    transaction with UPDLOCK, HOLDLOCK on the prefix range
                                    (or introduce a SEQUENCE object, per §9's decision)
Step 5 — P1 database uniqueness  : run the duplicate-TransferCode check; if clean, create the
                                    filtered unique index from §10
Step 6 — P2 UI duplicate-click protection: disable Submit/Approve buttons on click (§11), cosmetic
Step 7 — Full regression/concurrency testing: execute TMT-REG-001 through TMT-REG-010 (§20) in a
                                    test environment before any production deployment
```

---

## FINAL VERDICT

`REMEDIATION PLAN READY`

The design for every P0 and P1 item (idempotency, stock safety, TransferCode concurrency, uniqueness) is fully specified against verified schema and procedure evidence, reuses the existing status model and business key, and introduces no new locking beyond ordinary row-level locks SQL Server already takes for the statements involved. The items listed in §16 are implementation-detail decisions (FIFO ordering confirmation, monthly-reset policy, live-DB duplicate check, error-signaling mechanism) that should be settled during the coding phase — none of them change the shape of the design above, so they do not block this plan from being considered ready.

```text
Files Modified: 0
Database Modified: 0
Data Modified: 0
Commit: NO
Report: docs/audit/TradeMaterialTransfer_Remediation_Plan.md
```
