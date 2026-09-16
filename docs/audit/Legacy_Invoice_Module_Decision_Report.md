# Legacy Invoice Module — Decision Audit

Read-only investigation. No code changed, no database changed, no tables created, `InvoiceCreationForCustomerByOrder.aspx` untouched, ORD-001 untouched, nothing committed.

---

## 1. Executive Summary

Of the 3 pages under review, only **`InvoiceCreationForConsumerCustomer.aspx`** ("Direct Sales") is actually reachable by real users today — it has a live menu entry and is granted to 3 named users — and it is currently broken (every save throws `Msg 8101`, IDENTITY_INSERT violation). **`InvoiceCreationForCustomer.aspx`** and **`ReturnInvoiceCreationForCustomer.aspx`** are **not reachable by anyone**: neither appears in the database-driven menu (`tblMainMenu`), neither is a `Response.Redirect` target anywhere in the codebase, and neither has been touched since the original 2020 import. They are dead code, not "temporarily broken live features."

`InvoiceCreationForCustomerByOrder.aspx` remains the working, actively-maintained page (edited as recently as 6 days before this audit) and was not touched in this pass. Its returns/print flow is separately supported by `ReturnProductCustomerByInvoice.aspx` / `ReturnProductConsCustomerByInvoice.aspx`, both live in the menu, which is why `ReturnInvoiceCreationForCustomer.aspx`'s missing tables (`tblReturnInvoice`/`tblReturnInvoiceDetail`) were never noticed — nothing routes users there.

Recommendation shape (detail in Part 9): fix `InvoiceCreationForConsumerCustomer.aspx` (it's live and broken — real impact), retire `InvoiceCreationForCustomer.aspx` and `ReturnInvoiceCreationForCustomer.aspx` from active scope (dead, unreachable, and — for the latter — targeting tables that don't exist and duplicating a module that already works).

---

## 2. Current Working ByOrder Architecture

`InvoiceCreationForCustomerByOrder.aspx.cs`, `saveButton_Click1` (lines 1393-1448):

```
Validation() + OrderExists(orderId) gate
 ↓
TransactionScope (ReadCommitted, 5 min timeout)
 ↓
InvoiceBLL.SaveInvoiceMater(PrepareMasterData())
   → InvoiceDAL.SaveInvoiceMater → SaveAction("sp_I_InvoiceMaster", params, "@InvoiceId")
   → sp_I_InvoiceMaster: INSERT omits InvoiceId column entirely, SET @InvoiceId = SCOPE_IDENTITY()
   → also enforces its own duplicate-OrderNo-within-60-seconds guard (returns 0 if hit)
 ↓ (invId <= 0 → return without Complete() → rollback)
SaveInvoiceDetail(invId) → InvoiceBLL.SaveInvoiceDetails(list, comUnitId) → UpdateDCStock per line
   → InvoiceDetailInsertBLL (insert detail row) + InvoiceDAL.UpdateDCStoreQuantity (atomic decrement)
 ↓ (false → return without Complete() → rollback)
OrderInfoBLL.UpdateInvoiceStatus(orderId)
 ↓
scope.Complete() → COMMIT on Dispose
```

Reached only via an intermediary redirect page, `InvoiceCreationByOrder.aspx.cs:127` (`Response.Redirect("InvoiceCreationForCustomerByOrder.aspx")`), and from `CommonUI/HomePage.aspx.cs:160`. The menu item "Proforma Invoice Creation" (`tblMainMenu`, 23 users granted) points at that redirect wrapper. This is the single most-used entry point of the four invoice-creation pages by user-grant count.

Live-verified in the prior P0/P1 pass (`STK-REG-001`, `INV-T01-byorder`, `INV-T07-byorder-doubleclick`): master save, detail save, stock deduction, insufficient-stock rejection, and full transaction rollback all confirmed correct. **Not re-touched or re-tested in this pass** per instruction — this section is a restatement of already-verified fact for context.

---

## 3. Legacy Customer Invoice Audit — `InvoiceCreationForCustomer.aspx`

- **Reachability**: absent from `tblMainMenu` (0 rows reference its URL, by any label). Zero `Response.Redirect`/`NavigateUrl`/`href` references anywhere in the repo other than the page's own `CodeFile=` self-declaration. Last touched 2020-09-03 (single commit, the initial import) — never revisited since.
- **Save mechanism**: `saveButton_Click` (no `TransactionScope`, return value of `SaveInvoiceDetail` not even checked) → `InvoiceBLL.SaveInvoice` → `InvoiceDAL.SaveDataForInvoice`, which computes `InvoiceId` via `ClsPrimaryKeyFind.PrimaryKeyMax("invoiceId","tblInvoice")` and explicitly lists it in the `INSERT` column list. `tblInvoice.InvoiceId` is a live `IDENTITY`+`PRIMARY KEY` column → every save throws `Msg 8101`.
- **Order dependency**: takes a free-text `OrderNo` (`orderNoTextBox`) that is validated non-empty but **never checked against a real order** — no `OrderExists`, no order-detail load. Structurally simpler (and looser) than ByOrder's order-gated flow.
- **Customer model**: standard `tblCustMaster`-based customer (no on-the-fly customer creation).
- **Discount handling**: discount/bonus fields are populated from grid textboxes, not zeroed — closer to ByOrder's shape than the Consumer page, but this can't be confirmed as *correct* since the page has never successfully executed a save against this schema to observe real behavior.

## 4. Legacy Consumer Invoice Audit — `InvoiceCreationForConsumerCustomer.aspx`

- **Reachability**: **live**. Menu entry "Direct Sales" (`tblMainMenu` SL 25, ParentId 8) is granted to 3 real users via `tblMenuDistribution` (not just the superuser). This page is used today, or at minimum attempted — and fails.
- **Save mechanism**: `saveButton_Click` (no `TransactionScope`) → `InvoiceBLL.SaveInvoiceConsumer` → `InvoiceDAL.SaveDataForInvoiceConsumer`, same `PrimaryKeyMax`+explicit-ID pattern as the plain Customer page → same `Msg 8101` failure on every save.
- **Order dependency — materially different from ByOrder and from the plain Customer page**: this page **auto-creates and self-approves its own order** at save time. `OrderSaveMaster()` inserts a new `OrderInfoMaster` row with `IsDirect=true`, `IsManual=true`, `ActionStatus="Accepted"`, `ApprovedBy=Session["LoginName"]`, `ApprovedDate=Now` — i.e., the same user who creates the sale also auto-approves the order behind it, with no external order ever having existed. `OrderSaveDetail()` writes matching order-detail rows. Nothing here resembles ByOrder's "load an existing approved order" model.
- **Customer model — materially different**: creates a lightweight **`ConsumerCustomerDAO`** (walk-in customer: name/address/phone only, no credit terms or address hierarchy) via `ConsumeCustomer()`, then provisions a GL account for it (`CreateGlOfCustomer`). This is a distinct, simpler customer entity from the standard `tblCustMaster` record ByOrder and the plain Customer page use.
- **Discount handling — deliberately bypassed**: `DiscountPercentage`, `DiscountAmount`, and `BonusQuantity` are hardcoded to `0` at save time; only a flat `SpecialAmount` survives. This looks like an intentional business rule for walk-in/direct sales (no dealer-style bonus/discount schemes apply), not an oversight.

**Conclusion**: this page implements a genuinely different "walk-in direct sale" business flow — self-service order creation, a lighter customer entity, and no discount/bonus pipeline — bolted onto the same broken master-insert bug as the plain Customer page. It is live, used, and currently non-functional.

## 5. Legacy Return Invoice Audit — `ReturnInvoiceCreationForCustomer.aspx`

- **Reachability**: absent from `tblMainMenu`. Zero code references anywhere in the repo besides its own `CodeFile=`. Last touched 2020-09-03, never revisited.
- **Save mechanism**: `InvoiceBLL.SaveReturnInvoice` → `InvoiceDAL.SaveDataForReturnInvoice`/`SaveDataForReturnInvoiceDetails`, targeting `dbo.tblReturnInvoice`/`dbo.tblReturnInvoiceDetail` — **confirmed absent from the live database** (`sys.tables` query, prior pass). This entire module (save AND its 5 associated report/read queries in `InvoiceDAL.cs`) is self-consistent but has no backing schema in this database.
- **Live replacement already exists and is used**: `ReturnProductCustomerByInvoice.aspx` (menu entry "Delivery Sales Return," 3 users granted) and `ReturnProductConsCustomerByInvoice.aspx` (menu entry "Direct Sales Return," 2 users granted) both target `tblCustomerReturnMaster`/`tblCustomerReturnDetail`, which **do exist** and are structurally different: a return is modeled as a claim against an **already-existing, already-delivered invoice/detail row** (tracked via `InvoiceDetailId`+`ReturnQty`), carrying an approval workflow (`ActionStatus`/`ApprovedBy`/`ApprovedDate`), rather than as a brand-new standalone "return invoice" document with its own numbering.

---

## 6. Database Schema Comparison

| | ByOrder master (`tblInvoice`) | Legacy Customer/Consumer master (`tblInvoice`) | Return Invoice (`tblReturnInvoice`/Detail) | Live Return module (`tblCustomerReturnMaster`/Detail) |
|---|---|---|---|---|
| Table exists in current DB | Yes | Yes (same table) | **No** | Yes |
| `InvoiceId`/PK generation expected by code | `SCOPE_IDENTITY()` via proc | Manually computed via `PrimaryKeyMax`, inserted explicitly | N/A (table doesn't exist) | Own PK, independent of `tblInvoice`'s ID scheme |
| Structural shape | Full order+customer+pricing invoice | Same table, same shape — only the insert mechanism differs | New standalone document (own `ReturnInvoiceId`/`ReturnInvoiceNo`) | Return-against-existing-invoice claim (`InvoiceId` FK + `InvoiceDetailId` FK + `ReturnQty` + approval fields) |

The plain Customer and Consumer pages don't have a schema mismatch — they target the *same, existing* `tblInvoice` table ByOrder uses successfully. Their problem is purely in the DAL's insert *mechanism* (Part 7). The Return Invoice page has an actual schema mismatch — its tables never existed in this database (or existed once and were dropped in favor of the `tblCustomerReturnMaster` design — no migration evidence either way was found in the repo, which has no SQL/migration folder to check).

## 7. InvoiceId Architecture Comparison

| Component | InvoiceId behavior |
|---|---|
| Current DB (`tblInvoice.InvoiceId`) | `IDENTITY` + `PRIMARY KEY` (confirmed via `sys.columns`/`sys.indexes`) |
| `sp_I_InvoiceMaster` (used by ByOrder) | Declares `@InvoiceId INT OUT` only; INSERT omits the column; `SET @InvoiceId = SCOPE_IDENTITY()` — written correctly for an identity column |
| `SaveDataForInvoice` (plain Customer) | Computes ID via `PrimaryKeyMax`, inserts it explicitly — written for a **non**-identity column; incompatible with the live schema |
| `SaveDataForInvoiceConsumer` (Consumer) | Identical pattern/incompatibility as above |
| Every other module that references `InvoiceId` (details FK, `CustomerReturnDAL`, payment posting procs, report DALs) | Treats it as an opaque join key only — no code found anywhere that parses, formats, or derives meaning from the numeric value itself | No dependency on *how* the ID was generated, only that it's a stable, real integer |

No evidence anywhere in the repo that any consumer depends on `InvoiceId` being manually/sequentially computed rather than `IDENTITY`-assigned. The two legacy pages' public method signatures (`bool SaveInvoice(Invoice, out int invoiceId, out string invoiceNo)`) are unaffected by *how* the ID is produced internally — callers receive the real final ID either way.

## 8. Return Module Comparison

| | `ReturnInvoiceCreationForCustomer.aspx` | `ReturnProductCustomerByInvoice.aspx` / `...ConsCustomerByInvoice.aspx` |
|---|---|---|
| Menu presence | None | "Delivery Sales Return" (3 users) / "Direct Sales Return" (2 users) |
| Last touched | 2020-09-03 (import only) | 2026-09-01 (`ByInvoice` variant — actively maintained); Cons variant also 2020-only but still linked live |
| Backing tables | `tblReturnInvoice`/`tblReturnInvoiceDetail` — **do not exist** | `tblCustomerReturnMaster`/`tblCustomerReturnDetail` — **exist and are queried by multiple modules** (report DALs also reference them) |
| Data model | Return is its own new "invoice" document (own ID/number sequence) | Return is a claim against a specific existing `InvoiceId`/`InvoiceDetailId`, with `ReturnQty` and an approval workflow |
| Functional overlap | Would duplicate the same business capability ("customer returns product") | Already covers this capability, live, in production use |

---

## 9. Active vs Legacy Classification

| Page | Status | Evidence | Recommended Action |
|---|---|---|---|
| `InvoiceCreationForCustomer.aspx` | **UNUSED** (dead code — not merely "legacy," genuinely unreachable) | Absent from `tblMainMenu`; zero code redirects anywhere; untouched since the 2020 import; currently throws on every save attempt regardless | Exclude from active fix scope; confirm with stakeholders before any removal, but do not invest fix effort here without a stated business need to revive it |
| `InvoiceCreationForConsumerCustomer.aspx` | **ACTIVE BUT BROKEN** | Live "Direct Sales" menu entry, granted to 3 real users (`tblMenuDistribution`); every save currently throws `Msg 8101` | Needs a real fix — see Part 6/9 Decision B |
| `ReturnInvoiceCreationForCustomer.aspx` | **SUPERSEDED** (and separately broken) | Absent from `tblMainMenu`; zero code references; targets tables that don't exist; a live, actively-maintained replacement (`ReturnProductCustomerByInvoice.aspx`) already covers the same capability with a different, working data model | Do not fix or resurrect its schema; treat as retired |
| `ReturnProductCustomerByInvoice.aspx` | **ACTIVE** | Live "Delivery Sales Return" menu entry (3 users); targets existing `tblCustomerReturnMaster`/`tblCustomerReturnDetail`; most recently edited file in this comparison set (2026-09-01) | No action — out of scope, working |

---

## 10. Recommended Migration Strategy (for your decision — not implemented)

**For `InvoiceCreationForConsumerCustomer.aspx` (the one page that actually needs fixing):**

The ByOrder master-insert pattern (`SaveInvoiceMater`/`sp_I_InvoiceMaster`, `SCOPE_IDENTITY()`) is safe to reuse **for the master-row mechanics only** — swap `SaveDataForInvoiceConsumer`'s raw dynamic INSERT for a `SaveAction`+stored-procedure call that lets `IDENTITY` assign the row, exactly as `SaveInvoiceMater` already does (same or a near-identical sibling stored procedure, since `sp_I_InvoiceMaster`'s parameter set is a superset of what this page needs — the proc doesn't care who its caller is). Pair this with adding a `TransactionScope` around Master+Detail+Stock in this page's `saveButton_Click`, mirroring ByOrder's `saveButton_Click1`, so the STK-002-style partial-commit risk (Part 2 of the prior report) doesn't reappear the moment the insert starts succeeding.

**What must NOT be mechanically copied from ByOrder**: the order-dependency logic. ByOrder *requires* a pre-existing approved order (`OrderExists` gate). The Consumer page's entire reason for existing is walk-in sales with **no** pre-existing order — it self-creates and self-approves one (`OrderSaveMaster`/`OrderSaveDetail`) as an internal bookkeeping step, and zeroes out discount/bonus fields deliberately. **This business logic must be preserved as-is** (or explicitly redesigned with product-owner sign-off) — migrating "the save pattern" means fixing *only* the master-insert mechanism and the transaction boundary, not re-deriving the whole page onto ByOrder's order-centric flow.

**For `InvoiceCreationForCustomer.aspx`**: since it's unreachable and unused, there is no urgency to fix it at all. If a future business need requires reviving it, the same master-insert fix would apply (it has no order-auto-creation or discount-zeroing quirks to preserve — closer to a straight swap than the Consumer page), but this should be a deliberate "do we still need this page" conversation, not an automatic fix alongside the Consumer page.

**For `ReturnInvoiceCreationForCustomer.aspx`**: no migration recommended. Reviving it would mean either (a) creating `tblReturnInvoice`/`tblReturnInvoiceDetail` from scratch with no schema specification to build from (this repo has no migration/SQL-script history to source one from), duplicating the `tblCustomerReturnMaster`-based module that already does this job, or (b) rewriting the page onto `tblCustomerReturnMaster`'s data model — which is really "delete this page and point its use case at `ReturnProductCustomerByInvoice.aspx`," not a fix to the existing page.

---

## 11. Risks

| Proposed change | Risk | Why |
|---|---|---|
| Fix `InvoiceCreationForConsumerCustomer.aspx`'s master insert (swap to `SaveAction`+proc, `SCOPE_IDENTITY()`) | **LOW** | Public method signature (`out int invoiceId`) unchanged; no other code calls `PrimaryKeyMax("invoiceId","tblInvoice")` besides this page and the equally-broken plain Customer page (confirmed by repo-wide grep); no code anywhere derives meaning from the ID's numeric value, only uses it as an opaque join key |
| Add `TransactionScope` to `InvoiceCreationForConsumerCustomer.aspx`'s save handler | **LOW** | Directly mirrors ByOrder's already-proven, live-tested pattern (`STK-REG-001`); same connection string, same SQL Server instance, sequential non-overlapping connections — confirmed empirically not to require MSDTC in this environment |
| Preserve the self-created-order and zeroed-discount logic exactly as-is during the fix | **MEDIUM** if skipped | This is the page's actual differentiator from ByOrder — silently dropping or "simplifying" it during a fix would change real business behavior for the 3 users who rely on Direct Sales working the way it currently (partially) does |
| Fix `InvoiceCreationForCustomer.aspx` | **LOW technical risk, but likely unnecessary work** | Page is unreachable; fixing it changes nothing for any current user; only worth doing if a business owner confirms it should be revived |
| Create `tblReturnInvoice`/`tblReturnInvoiceDetail` and fix `ReturnInvoiceCreationForCustomer.aspx` | **HIGH** | Would stand up a second, parallel, unintegrated return-tracking system alongside the one already live (`tblCustomerReturnMaster`) — duplicate reporting, no shared approval workflow, unclear relationship between the two return histories for the same customer/invoice, and no schema spec exists anywhere in the repo to build the new tables from (pure guesswork) |
| Any change to `InvoiceId` generation strategy itself (e.g., changing the IDENTITY seed/increment, or reverting the schema to non-identity) | **CRITICAL if ever considered** | `InvoiceId` is referenced by `tblInvoiceDetail` (FK), `tblCustomerReturnMaster`/`Detail`, payment-posting procs, and multiple report DALs across the whole invoice ecosystem — not evaluated in this pass because no such change is being proposed, but flagged because Part D of the prior investigation raised the question explicitly: the answer is **the schema should not change**, only the two legacy pages' DAL code should be brought in line with it |

---

## 12. Regression Requirements

If `InvoiceCreationForConsumerCustomer.aspx` is fixed:
- Re-run the equivalent of `STK-REG-001`/`INV-T01-byorder` against this page's specific save path (master insert via the new mechanism, wrapped in `TransactionScope`, insufficient-stock scenario) to confirm the same atomic-rollback guarantee holds here too — do not assume it transfers just because the pattern is copied.
- Confirm `OrderSaveMaster`/`OrderSaveDetail` (the self-created-order logic) still runs correctly inside the same transaction boundary as the invoice master/detail/stock work — if it currently runs *outside* any transaction (it does today, since there is none), moving it inside one is itself a behavior change worth its own explicit test.
- Confirm the 3 users' actual expected end-to-end scenario (Direct Sales: walk-in customer + free product selection + no discount) still produces correct totals after the master-insert fix — the fix should touch only ID generation and the transaction wrapper, not the pricing/discount logic, but this must be verified, not assumed.
- Re-verify `INV-T07`-style double-click protection: `sp_I_InvoiceMaster`'s dedup guard is keyed on `OrderNo` — if the Consumer page's self-created order uses a different `OrderNo` value than what that guard expects (e.g. always blank, or always freshly generated), the guard may not protect this page the way it protects ByOrder. This needs explicit investigation before assuming double-click safety carries over.

If `InvoiceCreationForCustomer.aspx` or `ReturnInvoiceCreationForCustomer.aspx` are ever revived: full new regression suite, since neither has any live traffic or recent testing history to regress against.

---

## 13. Final Decisions

### Decision A — `InvoiceCreationForCustomer.aspx`

```
RECOMMENDATION: Do not fix as part of active work. Confirm with the business/product owner
                whether this page is still needed at all before investing further effort.
REASON:         Unreachable — absent from the live menu, zero code references, unused since
                the 2020 import. Fixing it changes nothing for any current user.
EVIDENCE:       tblMainMenu contains no row for this URL; repo-wide grep for Response.Redirect/
                NavigateUrl/href found zero references outside the page's own CodeFile; git log
                shows a single commit (2020-09-03), never revisited.
IMPLEMENTATION RISK: LOW (if fixed later) — but the real risk is spending effort on a page
                nobody uses instead of confirming intent first.
```

### Decision B — `InvoiceCreationForConsumerCustomer.aspx`

```
RECOMMENDATION: Fix. This is the one legacy page with real, current business impact.
REASON:         Live "Direct Sales" menu entry, granted to 3 real users, currently broken on
                every save (Msg 8101). The underlying business flow (walk-in customer,
                self-created order, no discount/bonus) is real and distinct — it needs its
                own careful fix, not a mechanical copy of ByOrder.
EVIDENCE:       tblMenuDistribution grants this page to UserId 2, 7, 25; SaveInvoiceConsumer/
                SaveDataForInvoiceConsumer explicitly insert into the live IDENTITY column
                tblInvoice.InvoiceId, reproducible on every call (confirmed live in the prior
                pass); ConsumerCustomerDAO/OrderSaveMaster/zeroed-discount logic confirmed by
                code read to be materially different from ByOrder.
IMPLEMENTATION RISK: LOW for the ID-generation/transaction fix itself; MEDIUM if the
                self-created-order and discount-zeroing business logic is not carefully
                preserved during the change (see Part 11).
```

### Decision C — `ReturnInvoiceCreationForCustomer.aspx`

```
RECOMMENDATION: Deprecate / remove from active scope. Do not create the missing tables.
                If users need a return-invoice-shaped feature to replace this page's intended
                purpose, treat it as new-feature design work pointed at the existing
                ReturnProductCustomerByInvoice.aspx model, not a fix to this page.
REASON:         Unreachable (no menu entry, no code references, untouched since 2020) AND
                targets tables that do not exist in this database AND a fully-featured,
                actively-maintained, live replacement (ReturnProductCustomerByInvoice.aspx /
                ReturnProductConsCustomerByInvoice.aspx, using tblCustomerReturnMaster/Detail)
                already covers the "customer return" business capability today.
EVIDENCE:       tblMainMenu has no entry for this page but does have "Delivery Sales Return"
                (3 users) and "Direct Sales Return" (2 users) pointing at the ByInvoice pages;
                sys.tables confirms tblReturnInvoice/tblReturnInvoiceDetail do not exist;
                repo-wide search found these table names referenced only inside this one
                dead module.
IMPLEMENTATION RISK: HIGH if "fixed" by creating the missing tables — would create a second,
                parallel, unintegrated return-tracking system running alongside the one
                actually in use, per Part 11.
```

---

# FINAL OUTPUT

```
LEGACY MODULE DECISION AUDIT COMPLETE

InvoiceCreationForCustomer:
STATUS: UNUSED (dead — no menu entry, no code references, untouched since 2020)
RECOMMENDATION: Do not fix without confirmed business need; not urgent regardless of IDENTITY_INSERT bug

InvoiceCreationForConsumerCustomer:
STATUS: ACTIVE BUT BROKEN (live "Direct Sales" menu entry, 3 users granted, every save fails)
RECOMMENDATION: Fix — port master-insert to SaveInvoiceMater/SCOPE_IDENTITY() pattern + add
                TransactionScope, while preserving its self-created-order and zeroed-discount logic

ReturnInvoiceCreationForCustomer:
STATUS: SUPERSEDED (no menu entry, no code references, targets nonexistent tables, replaced by
        ReturnProductCustomerByInvoice.aspx which is live and working)
RECOMMENDATION: Deprecate / remove from active scope; do not create tblReturnInvoice/tblReturnInvoiceDetail

ReturnProductCustomerByInvoice:
STATUS: ACTIVE (live "Delivery Sales Return" menu entry, 3 users granted, targets existing schema,
        most recently edited file in this comparison set)

ByOrder:
UNCHANGED — VERIFIED WORKING (not modified or re-tested in this pass)

ORD-001:
UNCHANGED (no RemainingQty/RemainingOrderQty reference introduced or touched)

Code Modified: NO
Database Modified: NO
Commit: NO

Report:
docs/audit/Legacy_Invoice_Module_Decision_Report.md
```
