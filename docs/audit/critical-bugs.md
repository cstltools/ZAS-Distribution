# Critical Bugs — Full Detail

Scope: Phase 23, CRITICAL severity only. 22-field detail is compressed to the fields that carry real
information for each finding (many fields collapse to "see evidence" rather than being padded out). Every
citation below was read directly from the named file during this audit or a sub-agent's pass within it.

---

## GAP-SEC-002 — 127 of 383 pages have no authentication guard
- **Severity:** CRITICAL · **Category:** Security · **Module:** System-wide
- **Description/Evidence:** Only `MainMasterPage.master.cs` checks session; 256 pages use it, 127 do not
  (mostly report viewers reading parameters from the query string). `specs/INVENTORY-PAGES.md` lists all 127.
- **Reproduction:** request any of the 127 listed URLs directly, without a session cookie.
- **Recommended fix:** add a shared base-page session check enforced for every `.aspx`, not opt-in via master
  page choice.
- **Confidence:** CONFIRMED

## GAP-SEC-003 — Hard-coded live `sa` credentials in source control
- **Severity:** CRITICAL · **Category:** Security · **Module:** System-wide
- **Exact file:** `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs:20-22` — `DataSource = "CSTL-VM-01\MSSQLSERVER2019"`,
  `UserId = "sa"`, `Password = "N@fcoZ@s2023#"`; four more `sa` passwords in `Solution.Web/Web.config`
  connection strings (lines 39-87).
- **Business/financial impact:** full database compromise if the repository or its history is ever exposed;
  the credential is **live** (not a placeholder), confirmed by direct read during this audit.
- **Recommended fix:** rotate immediately, move to a secrets store, purge from git history.
- **Confidence:** CONFIRMED

## GAP-SEC-005 — Systemic SQL injection, including the login query
- **Severity:** CRITICAL · **Category:** Security · **Module:** System-wide
- **Exact file:** `Library.DAL/PanalCls/PanalClsDAL.cs:14` (login query, string concatenation of `loginName`/
  `password`) — one of ~1,400 such strings across `Library.DAL`, executed via
  `EXEC ExecuteAllSqlQueryByStoreProcedure @Query = '<string>'`.
- **Recommended fix:** parameterize every dynamic query; the login query first as it is unauthenticated by
  definition.
- **Confidence:** CONFIRMED

## GAP-SEC-007 — Passwords stored and compared in plain text
- **Severity:** CRITICAL · **Category:** Security · **Module:** System-wide
- **Evidence:** `PanalClsDAL.Login`, `UserDAL.SaveUserInformation` — `tblUser.Password` written/compared
  verbatim, no hash or salt.
- **Recommended fix:** hash with a modern algorithm (bcrypt/argon2) and migrate existing rows on next login.
- **Confidence:** CONFIRMED

---

## BUG-ACC-004 (source: `accounting-integration-audit.md`) — Warehouse stock-in GL posting is dead code
- **Severity:** CRITICAL · **Category:** Accounting/Inventory · **Module:** Purchase/Warehouse
- **Page/SP:** `WarehouseStockInApproval.aspx.cs`, `sp_WHStockInPosting`
- **Exact file:** `Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx.cs:176-183` — the call to
  `approvalBll.PostJournal(stockInMasterId)` (which would invoke `sp_WHStockInPosting` via
  `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:115-121`) is **commented out**. Only
  `approvalBll.ApprovalUpdateBLL(aMasterDao)` actually runs.
- **Expected behavior:** approving a warehouse stock-in posts the corresponding accounting journal.
- **Actual behavior:** approval updates status only; no GL entry is ever created for warehouse stock-in, as
  shipped.
- **Root cause:** the posting call was commented out (intent unknown — possibly a deliberate deferral that was
  never re-enabled).
- **Data/financial impact:** goods received into the warehouse never generate a GL/payable entry through this
  path — see `BUG-PUR-002` and `TEST-003` for the compounding effect (no purchase module exists elsewhere
  either).
- **Related bugs:** `BUG-PUR-002` (same finding from the purchase side), `BUG-ACC-002`/`003`/`005` (siblings —
  discarded return values and commented-out preconditions at other posting call sites in the same file family).
- **Recommended fix:** re-enable the call (after confirming `sp_WHStockInPosting`'s current body is still
  correct) or replace it with whatever the business now intends for warehouse-in accounting.
- **Fix priority:** immediate — this silently breaks a core accounting integration point.
- **Confidence:** CONFIRMED

## BUG-ACC-030 (source: `financial-integrity-audit.md`) — No application-layer control prevents an unbalanced posting from being requested
- **Severity:** CRITICAL · **Category:** Accounting · **Module:** System-wide (all posting call sites)
- **Exact file:** `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1653-1693` (representative) and every other posting
  call site catalogued in `accounting-integration-audit.md` §"Shape A" — each passes only an id/key parameter
  to a stored procedure; no debit/credit line, account code, or amount is constructed or validated in C#.
- **Root cause:** the client tier never computes or sees the debit/credit lines at all — that logic lives
  entirely inside the 119 stored procedures. Combined with 0 DB triggers and 1 enforced FK in 213 tables
  (`database-audit.md`), the *only* place a debit=credit invariant could be enforced is inside procedure logic.
- **Business/financial impact:** this audit cannot confirm debit=credit holds for any posting; that is a
  control weakness independent of whether any specific posting is currently wrong.
- **Recommended fix:** export `sp_helptext` for the ~35 posting procedures (see
  `docs/audit/accounting-integration-audit-followup.md` for what has already been recovered) and add an
  independent balance-check report that sums Dr/Cr by voucher from the GL tables directly.
- **Confidence:** CONFIRMED (absence in C#); UNKNOWN (procedure-side enforcement) — partially resolved by the
  followup pass for the procedures whose bodies were recovered.

## BUG-FIN-003 — `PaymentReverse.aspx` never reverses the accounting journal
- **Severity:** CRITICAL · **Category:** Accounting/Finance · **Module:** Collection
- **Page:** `PaymentReverse.aspx` · **Exact file:** `Solution.Web/SInventory_UI/PaymentReverse.aspx.cs:24-83`,
  `Library.DAL/SInventory_DAL/PaymentReverseDal.cs:1-59`
- **Description:** `submitButton_Click1` → `DeleteCustomerPaymentInfo` nulls the invoice's payment status,
  then issues raw `DELETE`s against `tblCustomerPay`/`tblCustPayDetail` (`PaymentReverseDal.cs:35-45`) —
  **it never calls `sp_Sales_CustomerPaymentReversePosting`**, the procedure that exists specifically for this
  (confirmed body at `docs/database/stored-procedures-part3.md:1956-2314`, correctly posts a reversing
  voucher and recomputes invoice payment fields from remaining detail rows). That procedure is only reachable
  from a different screen (`BatchWiseCollectionReportDal.cs:280`).
- **Expected behavior:** reversing a payment reverses both the SalesRoll rows and the GL posting.
- **Actual behavior:** the GL posting (cash/bank Dr, AR Cr) stays permanently in place in `CSTL_ACCDB`; the
  invoice becomes payable again on the SalesRoll side.
- **Data impact:** `tblCustomerPay`/`tblCustPayDetail` rows deleted; `PaymentAmount`/`PaymentStatus` on
  `tblInvoice` set to `NULL` outright rather than recomputed — if the invoice had multiple receipts, **all**
  of them are wiped, not just the one being reversed.
- **Financial impact:** cash/bank and AR remain permanently misstated by the reversed amount; the invoice can
  be collected again, creating a double-collection risk.
- **Business impact:** any user with menu access to this one page can create an unreconcilable accounting
  discrepancy with no approval gate (`ClsApprovalAction` is not used on this page).
- **Recommended fix:** route this screen through `sp_Sales_CustomerPaymentReversePosting` instead of raw
  deletes; add an approval gate.
- **Fix priority:** immediate.
- **Confidence:** CONFIRMED

## BUG-FIN-004 — Duplicate-payment detection defaults nearly every row into one false "duplicate" set
- **Severity:** CRITICAL (via its "fix" — mass deletion risk) · **Category:** Data integrity/Finance · **Module:** Collection
- **Page/SP:** `DuplicatePaymentList.aspx`, `sp_DEL_DuplicatePayment`
- **Exact file:** `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs:15-27`,
  `Library.DAO/SInventory_Entities/CustPaymentDetail.cs:24` (`TransctionDetailId`, non-nullable `int`,
  default `0`), `Solution.Web/SInventory_UI/CustomerPaymentProcess.aspx.cs:495`
- **Description:** the duplicate query groups `tblCustPayDetail` by `TransctionDetailId` and flags any group
  with `COUNT(...) > 1`. That column is populated with a real value in exactly one workflow
  (`CustomerPaymentProcess.aspx.cs:495`); every other payment path (including the main `CustomerPayment.aspx`
  flow) inserts it as `0`. Any two ordinary payments from the majority path are therefore grouped together as
  "duplicates."
- **Expected behavior:** flag genuine double-entries only.
- **Actual behavior:** flags almost all payments outside one narrow workflow as duplicates of each other.
- **Recommended fix:** key duplicate detection on `InvoiceId + Amount + Date (+tolerance)` or a real
  idempotency key generated at save time, not a column that most writers never populate; audit
  `sp_DEL_DuplicatePayment`'s body before this screen is used again.
- **Fix priority:** immediate — freeze use of the "remove duplicates" action on this screen until fixed.
- **Confidence:** CONFIRMED mechanism; SUSPECTED for the real-world false-positive rate (needs live data).

## BUG-FIN-010 — Credit limit is never checked at invoice creation
- **Severity:** CRITICAL · **Category:** Business rule/Finance · **Module:** Sales/Master Data
- **Exact file:** grep for `CreditLimit` across `Solution.Web` (13 files) — only
  `ManualOrderrCreation.aspx.cs`, `SampleProductrIssue.aspx.cs`, `SlaveInfoApproval.aspx.cs` perform an actual
  check. `InvoiceCreationByOrder.aspx.cs` and `InvoiceCreationForCustomerByOrder.aspx.cs` reference no credit
  limit at all.
- **Description:** if any order can reach invoicing without passing through `ManualOrderrCreation.aspx` — or an
  invoice is created directly against a customer with no prior order — no credit-limit gate exists at the
  point cash is actually placed at risk (goods leaving the warehouse).
- **Business impact:** customers can be invoiced past their approved credit limit.
- **Recommended fix:** enforce the credit-limit check at invoice save, not only at order entry, and route the
  override through the real approval engine rather than a self-service checkbox (see `BR-AUDIT` findings in
  `business-rule-audit.md`).
- **Fix priority:** high — this is a direct financial-exposure control gap.
- **Confidence:** CONFIRMED (absence). Menu/permission data (`GAP-SEC-001`) blocks confirming which invoice
  screens are actually reachable in production without going through order creation first — this bounds but
  does not eliminate the risk.

## BUG-STOCK-001 — No stock-availability check before invoice save
- **Severity:** CRITICAL · **Category:** Inventory · **Module:** Sales/Inventory
- **Exact file:** `Library.BLL/SInventory_BLL/InvoiceBLL.cs:218-321` (`UpdateDCStock`)
- **Description:** the FEFO batch-consumption loop simply stops when batches run out; there is no `else`
  branch that raises an error. Matches `GAP-APP-004`.
- **Actual behavior:** the invoice header and any already-processed detail rows are committed; remaining
  demand is silently dropped with no message to the user.
- **Recommended fix:** validate total available stock across batches before committing the header; reject or
  flag the invoice if demand exceeds supply.
- **Fix priority:** immediate.
- **Confidence:** CONFIRMED

## BUG-STOCK-003 — No SQL transaction wraps any two-sided stock movement
- **Severity:** CRITICAL · **Category:** Transaction integrity/Inventory · **Module:** Inventory (system-wide)
- **Evidence:** grep for `SqlTransaction`/`BEGIN TRAN` across `Library.DAL/SInventory_DAL` — zero hits besides
  one unused instance in `DB_Manager.cs`. Every transfer/freeze/release is 2+ independently-committing calls
  (e.g. `OtherStockAction.aspx.cs:141-149`, `WhFreezeStockRelease.aspx.cs:166-222`,
  `StockConditionFreeze.aspx.cs`'s 3-step chain).
- **Recommended fix:** wrap each multi-step stock movement in a single transactional stored procedure or a
  `TransactionScope`.
- **Fix priority:** immediate — this is the schema-level root cause behind several other findings
  (`BUG-STOCK-005/007`, `RCA-001`).
- **Confidence:** CONFIRMED

## BUG-SALES-001 — Discount silently dropped to zero on line insert (direct customer invoice)
- **Severity:** CRITICAL · **Category:** Correctness/Financial · **Module:** Sales
- **Exact file:** `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs:448-462`
  (`SaveInvoiceDetail`) sets only `DiscountPercentage`, never `DiscountAmount`;
  `Library.BLL/SInventory_BLL/InvoiceBLL.cs:348-363` (`InvoiceDetailInsertBLL`) branches on three
  `DiscountPercentage`/`DiscountAmount` combinations, none of which match this page's actual case
  (`DiscountPercentage > 0 && DiscountAmount == 0`) — so the stored discount stays at its constructed default
  of `0` (`InvoiceBLL.cs:330`).
- **Actual behavior:** `tblInvoiceDetail.NetAmount` persists **higher** than what the customer was shown and
  agreed to, even though the invoice-master total field (`TpDiscount`) is still populated with the correct
  discount.
- **Financial impact:** the invoice detail total does not match the invoice header total whenever this specific
  screen applies a percentage discount — a real overcharge relative to what the customer saw, persisted to the
  system of record.
- **Recommended fix:** align `InvoiceDetailInsertBLL`'s branch conditions with the actual `DiscountPercentage>0,
  DiscountAmount==0` case this screen produces (compute `TotalPrice × pct / 100` in that branch, matching the
  other screens' behavior).
- **Fix priority:** immediate — active financial correctness defect on a live save path.
- **Confidence:** CONFIRMED

## BUG-PUR-002 — GL/journal posting for warehouse stock-in is dead code (purchase-side confirmation of `BUG-ACC-004`)
- **Severity:** CRITICAL if `sp_WHStockInPosting` is the only place a supplier payable is booked · **Category:** Accounting/Purchase · **Module:** Purchase/Warehouse
- Same evidence as `BUG-ACC-004` above, examined from the purchase-workflow angle: since no other purchase/
  supplier-payable module exists in this codebase (`BUG-PUR-001`), this dead call may be the **only** intended
  GL-posting point for goods receipt.
- **Recommended fix:** same as `BUG-ACC-004`, prioritized against confirming whether any other mechanism books
  the supplier payable.
- **Confidence:** CONFIRMED (dead code) / SUSPECTED (full financial impact, pending confirmation there is no
  alternate posting path).

## BUG-PUR-009 — Unparameterized SQL throughout the receipt/payable path
- **Severity:** CRITICAL if reachable by a low-trust role · **Category:** Security/Purchase · **Module:** Purchase/Warehouse
- **Exact file:** `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:90-130,274-293`,
  `WarehouseStockInApprovalDal.cs:38-44,63-113` — INSERT/UPDATE built by string concatenation of `ChallanNo`,
  `ReferenceNo`, `Remarks`, `EntryBy` (from `Session["LoginName"]`).
- **Recommended fix:** parameterize; consistent with the system-wide `GAP-SEC-005` remediation.
- **Confidence:** CONFIRMED (pattern) / SUSPECTED (severity — depends on who can reach these fields, which
  needs the permission data this audit does not have, `GAP-SEC-001`).

## BUG-RET-001 — Consumer-customer returns bypass quarantine, self-approve, and skip posting entirely
- **Severity:** CRITICAL · **Category:** Inventory/Accounting · **Module:** Returns
- **Page:** `ReturnProductConsCustomerByInvoice.aspx` (RET-002)
- **Exact file:** `Solution.Web/SInventory_UI/ReturnProductConsCustomerByInvoice.aspx.cs:106-160`
  (`saveButton_Click`) writes directly to **sellable** stock
  (`Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:106-110`, unconditional `UPDATE tblDCStore SET
  StockQty=StockQty+<qty>`), sets `ActionStatus = "Accepted"` itself in the same request (bypassing the RET-004
  approval screen entirely), and never calls `sp_Sales_DeliverySalesReturnPosting` — so no accounting posting
  ever fires for this return type. No duplicate-submission guard exists (no button-disable, no idempotency
  key); a repeated submission double-credits stock.
- **Recommended fix:** route consumer-customer returns through the same quarantine/approval/posting pipeline
  as regular-customer returns (RET-003/004).
- **Fix priority:** immediate.
- **Confidence:** CONFIRMED

## BUG-RET-003 — Return approval posts the journal entry on every action, including Reject
- **Severity:** CRITICAL · **Category:** Accounting · **Module:** Returns
- **Page:** `ReturnustomerByInvoiceApproval.aspx` (RET-004)
- **Exact file:** `Solution.Web/SInventory_UI/ReturnustomerByInvoiceApproval.aspx.cs:96-165` — the guard
  `if (SelectedValue != null)` at line 96 is always true by construction (the outer flow already ensures a
  selection); `PostDeliverySalesReturn` (→ `sp_Sales_DeliverySalesReturnPosting`) runs for **every** selected
  action, not only "Accepted."
- **Actual behavior:** a rejected return can still fire the accounting reversal, while `BUG-RET-006` (credit
  adjustments not reversed on reject) compounds this into a genuine double-benefit for the customer: goods
  never physically returned, but both a credit adjustment and a posted journal reversal exist.
- **Recommended fix:** gate `PostDeliverySalesReturn` on `SelectedValue == "Accepted"` specifically.
- **Fix priority:** immediate.
- **Confidence:** CONFIRMED (code logic) — needs `sp_helptext sp_Sales_DeliverySalesReturnPosting` (or the
  followup doc, if it was recovered there) to size the exact GL blast radius.

## BUG-ACC-902 (new, from `accounting-integration-audit-followup.md`) — Deleting an invoice orphans its posted GL voucher in 2 of 3 delete paths
- **Severity:** CRITICAL · **Category:** Accounting · **Module:** Sales/Accounting
- **Exact evidence:** actual T-SQL bodies recovered and read in full during the follow-up reconciliation pass:
  - `sp_Deletenvoice` (`docs/database/stored-procedures-part1.md:10086`) reverses stock and hard-deletes the
    invoice — **it does not touch any GL/voucher table at all, not even as dead code.**
  - `sp_DeleteDeliveryInvoice` (`docs/database/stored-procedures-part1.md:9214`) contains a block that would
    delete the matching posted GL vouchers, but that block is **commented out** in the procedure body itself.
  - `sp_Delete_ProformaInvoice` (`docs/database/stored-procedures-part1.md:9774`) likewise has its
    accounting-voucher cleanup block commented out.
  - Only `sp_DeliveryInvoiceDeletePosting` (`docs/database/stored-procedures-part1.md:11562`) actually posts a
    reversing voucher — and it is a separate procedure from the three above, meaning a caller must invoke it
    *in addition to* the delete, not automatically.
- **Description:** any invoice deleted through the two silent paths, if it was already posted to the GL (via
  `sp_ProformaPosting`, `sp_DeliveryConfirmationFullPosting`/its live `_ADJ_` sibling, etc.), leaves that
  posting permanently in the general ledger with no corresponding invoice left in SalesRoll to explain it.
- **Financial impact:** phantom, unexplainable GL entries accumulate over time as invoices are deleted through
  the normal (non-GL-aware) delete paths — a direct threat to any GL-to-sales-register reconciliation.
- **Recommended fix:** route every invoice-delete path through (or call in sequence with)
  `sp_DeliveryInvoiceDeletePosting`, or re-enable the commented-out cleanup blocks in the other two procedures
  after verifying they are still correct.
- **Fix priority:** immediate.
- **Confidence:** CONFIRMED (procedure bodies read directly).

## Cross-cutting confirmation across all 26 recovered procedure bodies

The follow-up pass (`accounting-integration-audit-followup.md`) confirms, from actual T-SQL rather than
inference, that **none of the 26 GL-posting/deletion procedures it read use an explicit transaction, TRY/CATCH,
or RAISERROR.** This directly corroborates `BUG-ACC-030`/`RCA-005` from the database side, not just the C#
side: the absence of a debit=credit safety net is confirmed at both layers now, not assumed at one and unknown
at the other. It also independently found "missing postings" backfill/catch-up jobs for four separate posting
procedures scoped to rows from 2025-04-25 onward — first-hand evidence, not inference, that postings have
already failed to fire reliably in this production system at least once before.

---

See `bug-register.md` for the full severity breakdown and `high-priority-bugs.md` / `medium-low-bugs.md` for
everything below CRITICAL. See `accounting-integration-audit-followup.md` for the full procedure-by-procedure
reconciliation, including the `BUG-FIN-001` downgrade (the discarded parameters in
`CustPaymentBLL.UpdateInvoicePaymentAmount` turn out to be harmless dead code, not a live bug, because
`sp_UD_InvoicePayment` recomputes everything from source tables regardless of what it's sent).
