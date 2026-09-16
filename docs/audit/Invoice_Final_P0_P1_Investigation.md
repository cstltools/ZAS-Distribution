# Invoice P0/P1 Follow-up — STK-002 Transaction Fix + Legacy Page Architecture Investigation

Executed 2026-09-08 against local dev SQL Server `TOWSIF\MSSQLSERVER2019` / `SalesRollDB_ZAS`. No production database touched. No code committed. **ORD-001 excluded** — confirmed no `RemainingQty`/`RemainingOrderQty` reference was added anywhere in this pass (see Part I).

---

## 1. STK-002 Root Cause

**There is no defect in `InvoiceCreationForCustomerByOrder.aspx.cs`.** The prior test pass's STK-T03/STK-T08 "FAIL" results were a **test-methodology gap**, not a real bug: those tests called `InvoiceBLL.SaveInvoiceDetails(...)` directly, bypassing the `TransactionScope` that the real page wraps it in. `SaveInvoiceDetails`/`UpdateDCStock` were never designed to be transactional on their own — they rely entirely on the caller providing a transaction, exactly as the ByOrder page does.

The real, complete flow in `saveButton_Click1` (`Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs:1393-1448`):

```csharp
using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
{
    invId = aInvoiceBll.SaveInvoiceMater(PrepareMasterData());
    if (invId <= 0) { showMessageBox(...); return; }        // scope disposed without Complete() -> rollback

    if (!SaveInvoiceDetail(invId)) { showMessageBox(...); return; }  // same -> rollback

    aOrderInfoBll.UpdateInvoiceStatus(orderIdHiddenField.Value);
    scope.Complete();
}
```

This was already correct **before** this session started. Nothing here needed to change.

## 2. STK-002 Fix

**No code change made — none was needed for the ByOrder page.** Verified instead with a new, permanent regression test (see Part 7 below / `STK-REG-001`) that replicates the exact real call sequence (`TransactionScope` → `SaveInvoiceMater` → `SaveInvoiceDetails`) with insufficient stock (10 available, 15 requested) and confirms full rollback.

**The underlying partial-commit risk described in Finding A is real, but only for the 3 legacy pages** (`InvoiceCreationForCustomer`, `InvoiceCreationForConsumerCustomer`, `ReturnInvoiceCreationForCustomer`), whose `saveButton_Click` handlers have **no `TransactionScope` at all**:
```csharp
protected void saveButton_Click(object sender, EventArgs e)
{
    if (Validation() == true)
    {
        int invId = SaveInvoice();
        SaveInvoiceDetail(invId);   // return value not even checked
        Clear();
        showMessageBox("Create Invoice");   // always shown, regardless of detail/stock outcome
    }
}
```
This is currently **unreachable** because `SaveInvoice()`/`SaveInvoiceConsumer()` already throw before any detail/stock work happens (Finding B, Part 4). It would become live the moment Finding B is fixed without also adding a `TransactionScope` here — flagged as a required companion fix, **not implemented in this pass** per the instruction not to modify the legacy pages until their architecture is confirmed (Part 5/6).

## 3. Transaction Boundary — confirmed empirically

```
BEGIN (TransactionScope, ReadCommitted, 5 min timeout)
 ↓
SaveInvoiceMater()  →  EntLib Database.CreateDatabase("...SSIDB") → new SqlConnection → sp_I_InvoiceMaster
                        (opened, used, closed — non-overlapping with the next call)
 ↓
SaveInvoiceDetails() → for each batch: new SqlConnection → SaveDataForInvoiceDetails (or Consumer variant)
                        → new SqlConnection → UpdateDCStoreQuantity (atomic decrement)
                        (each opened, used, closed sequentially)
 ↓
UpdateInvoiceStatus() → new SqlConnection
 ↓
scope.Complete() called ONLY if every step above succeeded
 ↓
COMMIT (on Dispose, if Complete() was called) / ROLLBACK (on Dispose, if not — including on exception or early `return`)
```

**Key question answered**: this codebase opens a **new `SqlConnection` per DAL call** (via `DatabaseFactory.CreateDatabase(...)`), never passing a connection or transaction object explicitly between methods — everything relies on **`System.Transactions` ambient-transaction auto-enlistment**. This raised a real risk: MSDTC is **stopped** on this dev machine (confirmed via `Get-Service MSDTC` → `Stopped`), and .NET promotes an ambient `TransactionScope` to MSDTC if it thinks it needs a full distributed transaction coordinator.

**Empirically confirmed this is NOT a problem here**: `STK-REG-001` opened and closed multiple sequential `SqlConnection`s (master insert, detail insert, stock decrement) inside one `TransactionScope`, all targeting the same SQL Server instance, with each connection fully closed before the next opens. The .NET Lightweight Transaction Manager (LTM) handles this "delegated transaction" case without promoting to MSDTC, and the test observed a clean rollback with **no MSDTC-related exception**. This holds specifically because every DAL call in this flow (a) targets the same connection string / same SQL Server instance, and (b) never holds two connections open at once — if a future change opened two connections concurrently within the same scope, or pointed at two different databases, that guarantee would no longer hold.

## 4. IDENTITY_INSERT Root Cause

### Compatibility matrix

| Component | InvoiceId behavior |
|---|---|
| Current DB (`SalesRollDB_ZAS`, live schema) | `tblInvoice.InvoiceId` is `IDENTITY(seed,increment)` **and** `PRIMARY KEY` (confirmed via `sys.columns.is_identity=1` and `sys.indexes.is_primary_key=1`) |
| `InvoiceDAL.SaveDataForInvoice` / `SaveDataForInvoiceConsumer` (used by `InvoiceCreationForCustomer`/`InvoiceCreationForConsumerCustomer`) | Assumes `InvoiceId` is **manually generated**: computes it via `ClsPrimaryKeyFind.PrimaryKeyMax("invoiceId","tblInvoice")` (`SELECT ISNULL(MAX(InvoiceId),0)+1`) and explicitly lists `InvoiceId` in the `INSERT` column list with that value |
| `InvoiceBLL.SaveInvoice` / `SaveInvoiceConsumer` | Same assumption — calls `PrimaryKeyMax` and passes the computed ID straight through to the DAL insert above |
| `InvoiceCreationForCustomerByOrder.aspx.cs` (working page) | Does **not** compute or supply `InvoiceId` at all — calls `InvoiceDAL.SaveInvoiceMater`, which invokes stored procedure `sp_I_InvoiceMaster` and reads back the value SQL Server generated |
| `sp_I_InvoiceMaster` (stored procedure, live in DB) | Declares `@InvoiceId int OUT` only — never accepts an input value for it. Its `INSERT` statement omits `InvoiceId` from the column list entirely and does `SET @InvoiceId = SCOPE_IDENTITY()` immediately after, i.e. it is written **specifically for an IDENTITY column** |

**Root cause, stated plainly**: two different, mutually incompatible insert strategies coexist in the same codebase for the same table. `sp_I_InvoiceMaster` (and the page that uses it) is written for an `IDENTITY` `InvoiceId` and works correctly. `SaveDataForInvoice`/`SaveDataForInvoiceConsumer` (and the two pages that use them) are written for a **non-identity, manually-numbered** `InvoiceId` and are incompatible with the table as it exists today — every call throws `Msg 8101` (identity insert violation).

**Evidence found and how it was weighed**:
1. A **commented-out** earlier version of `SaveDataForInvoice`/`SaveDataForInvoiceDetails` (`InvoiceDAL.cs:202-247`) already attempted to call `SaveAction("sp_I_InvoiceMaster", ...)` — but that attempt itself incorrectly included `@InvoiceId` as an **input** parameter alongside the proc's own `OUT` parameter of the same name, which would collide/throw. This looks like an **abandoned, already-broken attempt**, not proof of a validated design — noted but not treated as strong evidence either way.
2. A repo-wide search for stored procedures containing `INSERT INTO tblInvoice` found **only `sp_I_InvoiceMaster`** as a genuine master-row inserter; no consumer-specific or legacy-specific insert procedure exists.
3. A repo-wide search confirmed `PrimaryKeyMax("invoiceId","tblInvoice")` is called **only** from the two broken methods (`InvoiceBLL.cs:124,133`) — nothing else in the codebase depends on `InvoiceId` being pre-computed before insert, so switching to a `SCOPE_IDENTITY()`-based approach would not require touching any other caller (the public method signatures `bool SaveInvoice(Invoice, out int invoiceId, out string invoiceNo)` / `SaveInvoiceConsumer(...)` are unaffected either way — the `out invoiceId` still receives the real final ID regardless of how it's generated internally).
4. `tblInvoiceDetail.InvoiceId` is referenced only via `FK_tblInvoiceDetail_tblInvoice` (a plain foreign key, no dependency on numbering scheme) and by report/payment/accounting queries that all join on `InvoiceId` as an opaque integer key — none of the searched modules (`CustomerReturnDAL`, payment posting procs, report DALs) parse or derive meaning from the `InvoiceId` value itself; they only use it as a join key. **No evidence found that switching `InvoiceId` generation from manual-max to identity/`SCOPE_IDENTITY()` would break any downstream consumer.**

**Conclusion — architecture appears to point clearly at one answer**: the current database schema (`IDENTITY`) is authoritative (it's real, live, and already successfully used by the ByOrder page and its stored procedure), and `SaveDataForInvoice`/`SaveDataForInvoiceConsumer` are the outdated side needing a fix, not the schema. **However, per the explicit instruction not to implement this without your confirmation, no code change was made.** Recommended next step if you approve: port `SaveDataForInvoice`/`SaveDataForInvoiceConsumer` onto the same `SaveAction` + stored-procedure + `SCOPE_IDENTITY()` pattern already proven by `SaveInvoiceMater`/`sp_I_InvoiceMaster` (either reusing that same proc, since its parameter set is a superset of what these two pages need, or a near-identical sibling proc if `Consumer`-specific columns differ) — plus adding the same `TransactionScope` protection at both pages' save handlers (Part 2) as part of the same change, since fixing the insert without also fixing the transaction boundary would reintroduce Finding A for these two pages.

## 5. Legacy Page Classification

| Page | Status | Root Cause | Required Action |
|---|---|---|---|
| `InvoiceCreationForCustomer.aspx` | **SUPPORTED + FIXABLE** | `SaveDataForInvoice` inserts an explicit value into the live `IDENTITY` column `tblInvoice.InvoiceId` (Msg 8101) | Adopt the `SaveInvoiceMater`/`sp_I_InvoiceMaster` pattern (proven working) + wrap save handler in `TransactionScope` (Part 2). Schema itself needs no change. |
| `InvoiceCreationForConsumerCustomer.aspx` | **SUPPORTED + FIXABLE** | Identical root cause via `SaveDataForInvoiceConsumer` | Same as above. |
| `ReturnInvoiceCreationForCustomer.aspx` | **LEGACY / DEPRECATED** | Targets `tblReturnInvoice`/`tblReturnInvoiceDetail`, which do not exist in this database (Part 6). A **separate, fully-featured, currently-referenced return module already exists** — `ReturnProductCustomerByInvoice.aspx` / `ReturnProductConsCustomerByInvoice.aspx`, backed by `CustomerReturnBLL`/`CustomerReturnDAL` against `tblCustomerReturnMaster`/`tblCustomerReturnDetail`, which models a return as a claim against an **existing** invoice/delivery (with an approval workflow: `ActionStatus`, `ApprovedBy`, `ApprovedDate`) rather than as a brand-new "return invoice" document. | Do **not** resurrect `tblReturnInvoice`/`tblReturnInvoiceDetail`. Confirm with the business/product owner whether `ReturnInvoiceCreationForCustomer.aspx` is still meant to be reachable at all — if the intent is genuinely a return-as-a-new-document workflow distinct from the existing by-invoice return, that is a new feature requiring a real schema design, not a bug fix. |

## 6. Return Invoice Schema

`tblReturnInvoice`/`tblReturnInvoiceDetail` are **genuinely absent** from `SalesRollDB_ZAS` (`sys.tables` query returns zero rows for either name — re-confirmed in this pass). This is not a naming mismatch or a case-sensitivity issue.

Repository-wide search for `tblReturnInvoice`/`tblReturnInvoiceDetail`: **only** `InvoiceDAL.cs` references them (save methods at lines 340/628, and 5 report/read queries at lines 1007, 1022, 1124, 1311, 1328-1329) — a fully self-consistent module that simply has no backing table in this database. No trace of these table names anywhere else in the repository (no SQL scripts, no migration files — this repo has no `.sql`/migrations folder at all to check against).

Search for `ReturnInvoice`, `InvoiceReturn`, `SalesReturn`, `Return` (broader terms) surfaced the real, active alternative: `CustomerReturnDAL.cs` (`tblCustomerReturnMaster`/`tblCustomerReturnDetail`), consumed by `ReturnProductCustomerByInvoice.aspx.cs` and `ReturnProductConsCustomerByInvoice.aspx.cs` via `CustomerReturnBLL.cs`. This module is structurally different: it links each return row directly to an `InvoiceId`/`InvoiceDetailId` from an invoice that was already saved and delivered (`ReturnQty` tracked per `InvoiceDetailId`, with `ApprovedBy`/`ApprovedDate`/`ActionStatus` for a review workflow) — it is not a drop-in rename of `tblReturnInvoice`.

**Conclusion**: this is case **2 is ruled out** (not a rename — column/table shapes differ meaningfully) and **case 3 fits** (replaced by another table/module) far better than case 1 (never existed — implausible, since fully-formed save/report code exists for it) or case 4 (missing migration — no migration evidence exists anywhere in the repo, and a live replacement module already covers the same business need). **Do not create these tables based on this evidence alone** — doing so would resurrect a second, parallel, un-integrated return-invoice system running alongside the one actually in use, with no shared approval workflow, no shared reporting, and an unknown relationship to the real (`tblCustomerReturnMaster`-based) return data already in production. If `ReturnInvoiceCreationForCustomer.aspx` must be revived, treat it as "design a real return-as-new-document flow" (a business/architecture decision), not "fix a missing table."

## 7. Regression Results

| Item | Status | Evidence |
|---|---|---|
| STK-001 (atomic stock decrement) | **PASS** | Unchanged this pass; re-confirmed by `STK-REG-001` incidentally (stock never went negative, decrement was exact) |
| STK-002 (insufficient stock, no partial commit) | **PASS** — for the ByOrder page (see Part 1/2). Not applicable / latent-only for the 3 legacy pages (currently unreachable, see Part 2) | `STK-REG-001`, executed live: `completedScope=False; masterCount before/after=0/0; detailCount before/after=2344/2344; stock before/after=10/10` |
| INV-001 (no master without details) | **Unaffected by this pass.** Still confirmed fine for the ByOrder page (unchanged code). Still **FAIL** for the 2 fixable legacy pages, purely as a downstream consequence of Finding B (they cannot create *any* master row right now, fixable or not) | No new evidence needed — Finding B is the blocking issue |
| PRT-002 (print parameter) | **BLOCKED** — unchanged; still requires a live IIS/browser session this environment cannot provide | Code review only, from the prior session's fix |
| ERR-001 (exceptions logged) | **CONDITIONAL** — unchanged from the prior pass: `SaveAction`/`UpdateAction` (ByOrder path) log correctly; `SaveDataByInsertCommand` (the 2 fixable legacy pages' insert path) does not log at all | Confirmed again by `INV-T01-master`-style exception during this pass producing no `DatabaseErrors.log` growth |

**ByOrder page regression check** (Part H requirement): master save, detail save, stock deduction, insufficient-stock rejection, and rollback were all re-exercised live in this pass (`INV-T01-byorder`, `STK-REG-001`) with **no code changes** to `InvoiceCreationForCustomerByOrder.aspx.cs`, `InvoiceBLL.cs`, or `InvoiceDAL.cs` — all passed. Print and error-handling for this page were not re-executed live (same IIS/browser limitation as before) but their code paths are untouched since the prior session.

## 8. Remaining Risks

- **Finding A/STK-002 for the 2 fixable legacy pages is latent, not resolved** — it will become a live data-integrity issue the moment Finding B (IDENTITY_INSERT) is fixed unless a `TransactionScope` is added to those pages' save handlers in the *same* change. Flagging this explicitly so the two fixes aren't done separately.
- **Schema-parity caveat stands**: all of the above is confirmed against the local dev database (`TOWSIF\MSSQLSERVER2019` / `SalesRollDB_ZAS`), not production. If production's `tblInvoice.InvoiceId` is not `IDENTITY`, or if production still has `tblReturnInvoice`/`tblReturnInvoiceDetail`, these specific findings would not reproduce there — verify schema parity before treating this as universally confirmed.
- **`ClsPrimaryKeyFind.PrimaryKeyMax`'s own race condition** (documented in the prior test pass) becomes irrelevant for `tblInvoice`/`tblInvoiceDetail` if the recommended `SCOPE_IDENTITY()`-based fix is adopted, since it would no longer be called for either table — a secondary benefit of that fix, not counted as "solving" the general `PrimaryKeyMax` race for other tables that still use it.
- **ERR-001 gap for `SaveDataByInsertCommand`** (Part 7) remains open regardless of the IDENTITY_INSERT decision — worth fixing independently since it affects visibility into *any* future failure on that code path, not just this one.
- **ERR-T01 fault injection remains unresolved and leaks rows if re-run.** Both a 500-char string into a `NVARCHAR(50)` parameter and a `9999999999999999.99` value into a `DECIMAL(18,2)` parameter were silently accepted by `sp_I_InvoiceMaster` without error (the column stored the out-of-range decimal value as-is — confirmed via direct query, itself a minor anomaly worth separate attention), each leaking a real `tblInvoice` row that had to be found and deleted manually after the run. This was cleaned up both times before finishing this session, and no `RemainingQty`-related tables were affected, but it means `ERR-T01` is still genuinely **not exercised** — a reliable way to force `SaveAction` to hit a real, hard SQL exception was not found in the time available.
- **No test project exists in this repo.** `STK-REG-001` lives only in a throwaway harness outside source control (per the "do not commit" instruction) — if this fix path is approved later, this test should be ported into something durable (even a simple script) so the regression stays checked going forward.

---

# FINAL OUTPUT

```
P0/P1 INVESTIGATION COMPLETE

STK-002:
Root Cause: Prior test called SaveInvoiceDetails() directly, outside the TransactionScope the real
            ByOrder page wraps it in. The real page flow already rolls back correctly.
Fixed: NO CODE CHANGE NEEDED (ByOrder page was already correct)
Regression Test: PASS (STK-REG-001, executed live — see Part 1/2/7)

IDENTITY_INSERT:
Root Cause Confirmed: YES — tblInvoice.InvoiceId is a live IDENTITY+PK column; SaveDataForInvoice/
                      SaveDataForInvoiceConsumer explicitly insert a manually-computed value into it
Fix Implemented: NO (per instruction — architecture evidence gathered and presented in Part 4/5 for
                 your decision; recommended fix described but not applied)
Reason: Strong evidence points to porting the legacy pages onto the proven SaveInvoiceMater/
        sp_I_InvoiceMaster pattern, but this is an architectural decision affecting 2 live pages and
        was explicitly excluded from auto-implementation by this task's rules.

RETURN INVOICE:
Schema Issue Confirmed: YES — tblReturnInvoice/tblReturnInvoiceDetail do not exist in this database;
                        a separate, currently-used replacement module (tblCustomerReturnMaster/Detail,
                        via ReturnProductCustomerByInvoice.aspx) already covers returns
Fix Implemented: NO
Reason: Creating the missing tables would resurrect a superseded, unintegrated return-invoice system
        running in parallel with the one actually in use — a business/architecture decision, not a
        bug fix, and explicitly excluded from auto-implementation by this task's rules.

ByOrder Regression:
PASS (master save, detail save, stock deduction, insufficient-stock rejection/rollback all
      re-verified live with zero code changes to this page)

ORD-001:
NOT TOUCHED (grep-confirmed: no RemainingQty/RemainingOrderQty reference exists anywhere in the
             files touched across this and the prior session)

Build:
PASS (no code changed this session, so unchanged from the prior session's clean aspnet_compiler run)

Commit:
NO

Report:
docs/audit/Invoice_Final_P0_P1_Investigation.md
```
