# Remediation Plan

Prioritized by financial/security exposure and by how cheaply each item closes given what this audit found.
Nothing here has been implemented — this is a plan, not a change log.

## Tier 0 — Do immediately, no code change required

| # | Action | Closes | Effort |
|---|---|---|---|
| 1 | Rotate the `sa` password in `DB_Authentication.cs` and every `Web.config` connection string; purge from git history | `GAP-SEC-003` | Hours |
| 2 | Freeze use of `DuplicatePaymentList.aspx`'s delete action until `BUG-FIN-004` is fixed | `BUG-FIN-004` | Minutes (process control) |
| 3 | Confirm whether `PaymentReverse.aspx` is reachable from the live menu; if so, restrict access immediately | `BUG-FIN-003` | Hours (needs menu/permission data, `GAP-SEC-001`) |
| 4 | Export `sp_helptext` for the one procedure this audit could not recover (`sp_DeleteDeliveryInvoice_FullRollback`) plus any others a human wants double-checked beyond the 26 already recovered | remaining slice of `GAP-DB-001` | Minutes, needs a human with a terminal on the DB host |

## Tier 1 — Fix within the next development cycle (financial/data-integrity exposure)

| # | Action | Closes | Notes |
|---|---|---|---|
| 5 | Route `PaymentReverse.aspx` through `sp_Sales_CustomerPaymentReversePosting` instead of raw deletes; add an approval gate | `BUG-FIN-003` | Highest real-money-impact fix in this audit |
| 6 | Route all invoice-delete paths through `sp_DeliveryInvoiceDeletePosting` or re-enable the commented-out GL cleanup in `sp_DeleteDeliveryInvoice`/`sp_Delete_ProformaInvoice` | `BUG-ACC-902`, RCA-006 | Verify the commented-out code is still correct before re-enabling |
| 7 | Fix `InvoiceDetailInsertBLL`'s discount branch conditions to cover `DiscountPercentage>0 && DiscountAmount==0` | `BUG-SALES-001` | Active overcharge defect on a live save path |
| 8 | Re-enable the warehouse stock-in GL posting call, or replace with the current intended mechanism | `BUG-ACC-004`/`BUG-PUR-002` | Confirm with the business what the intended purchase-side accounting flow is first |
| 9 | Rebuild duplicate-payment detection on a real key (invoice+amount+date, or a generated idempotency token) | `BUG-FIN-004` | Do before Tier 0 item 2 is lifted |
| 10 | Enforce credit limit at invoice save, not only order entry; route overrides through the real approval engine | `BUG-FIN-010`, `BUG-FIN-009` | Business decision on approver role needed |
| 11 | Gate return-approval posting on `SelectedValue == "Accepted"` specifically, not "not null" | `BUG-RET-003` | |
| 12 | Route consumer-customer returns through the same quarantine/approval/posting pipeline as regular returns | `BUG-RET-001` | |
| 13 | Add a stock-availability check before invoice save (reject or explicitly flag under-fulfillment) | `BUG-STOCK-001` | |
| 14 | Convert stock quantity writes from computed-overwrite to atomic `UPDATE ... SET Qty = Qty - @x WHERE Qty >= @x` everywhere identified | `BUG-STOCK-002`/`004`/`005`, `BUG-DATA-005` | System-wide pattern fix, biggest single risk-reduction item after Tier 1 #5 |

## Tier 2 — Structural fixes (transaction/architecture)

| # | Action | Closes |
|---|---|---|
| 15 | Wrap invoice save (header+detail+stock) and the equivalent multi-step operations elsewhere in a `TransactionScope` or move orchestration into one transactional stored procedure | `BUG-TXN-001/002/003`, `RCA-001` |
| 16 | Replace `MAX(id)+1` primary-key/invoice-number generation with `IDENTITY` columns or `sp_getapplock`-guarded sequences | `BUG-DATA-001/002` |
| 17 | Make `DB_Manager`/`ExecuteNonQueryAction` distinguish "no rows matched" from "an error occurred" | `BUG-TXN-004`, `BUG-DATA-004` |
| 18 | Add foreign key constraints for the highest-value relationships (invoice↔detail, invoice↔payment, stock↔product) after an orphan-row audit | RCA-004 |
| 19 | Add explicit transactions/TRY-CATCH to the GL-posting stored procedures themselves (confirmed absent in all 26 read) | `BUG-ACC-030`, cross-cutting finding in `accounting-integration-audit-followup.md` |

## Tier 3 — Security hardening (independent of the above, can run in parallel)

| # | Action | Closes |
|---|---|---|
| 20 | Add a shared base-page session check enforced for every `.aspx`, not opt-in via master page choice | `GAP-SEC-002` |
| 21 | Parameterize the ~1,400 concatenated SQL strings, starting with the login query | `GAP-SEC-005` |
| 22 | Hash passwords (bcrypt/argon2), migrate existing rows on next login | `GAP-SEC-007` |
| 23 | Require authentication on all 18 ASMX autocomplete methods and all 40 static `[WebMethod]`s | `GAP-SEC-004`, `BUG-API-003` |
| 24 | `debug="false"`, add `<customErrors>`, align session/auth-cookie timeouts | `GAP-SEC-008` |
| 25 | Add ownership checks (company unit / user scope) at the DAL boundary for every id-keyed read | `GAP-SEC-009` |

## Tier 4 — Business decisions needed before code changes

| # | Decision | Blocks |
|---|---|---|
| 26 | Is there an intended external purchase-to-pay system, or should this application gain a real PO module? | `BUG-PUR-001`, `TEST-003` |
| 27 | Which of the duplicated screens (`PaymentTranscation`/`New`/`New2`, `SalesReport`/`SalesReports`, five `InvoiceCreation*` variants) are actually live? | `GAP-STR-003`, scoping every fix above correctly |
| 28 | Should the two independent payment pipelines (`tblCustomerPay` vs `tblPaymentTranscationMaster`) be merged, or is one legacy? | `BUG-ACC-901` (reconciliation-audit.md) |
| 29 | Confirm the DC-store weighted-average costing is still deliberately disabled, and whether valuation reports built on it should carry a warning | `BUG-STOCK-015` |

## What to do first if only one thing can be done this week

**Tier 0 item 3** (confirm `PaymentReverse.aspx` reachability) followed by **Tier 1 item 5** (fix its reversal
logic) — this is the single finding in the entire audit with a direct, ongoing, unbounded financial-statement
impact (permanent GL/AR misstatement on every use, with no approval gate). Everything else in Tier 0/1 is
either a one-time data-integrity risk or bounded in scope.
