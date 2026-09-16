# Direct Sales Invoice — Final Go-Live Audit

Read-only audit. No files modified, no database data changed, no accounting vouchers created, nothing committed. Independently re-verified against source code and live (but read-only) database schema queries — not merely restated from `DirectSales_Invoice_Fix_Report.md`.

## 1. Overall Go-Live Recommendation

**SAFE WITH STAGING VERIFICATION.**

The transaction mechanism protecting `PostDeliveryConfFullJournal` is architecturally sound and consistent with everything already empirically proven in this investigation (the identical mechanism was live-tested three separate times: `STK-REG-001` for the ByOrder page, and `DS-T03`/`DS-T04` for this page's own order+master+detail+stock rollback). The reasons it isn't rated an unqualified SAFE are stated in full in §8 — chiefly, that the journal-posting step itself was never executed live (by design, to avoid creating real GL vouchers), so this section's conclusion rests on structural/architectural analysis rather than an observed test result for that one step specifically.

## 2. Journal Transaction Analysis

Call chain, traced from source:

```
InvoiceCreationForConsumerCustomer.aspx.cs : saveButton_Click
  -> TransactionScope (ReadCommitted, 5 min)     [System.Transactions, ambient]
     -> SaveInvoice() -> InvoiceBLL.SaveInvoiceConsumer -> InvoiceDAL.SaveDataForInvoiceConsumer
          -> ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity
               -> DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB")  [new SqlConnection #1, opened+closed]
     -> SaveInvoiceDetail(invId) -> InvoiceBLL.SaveInvoiceDetails -> UpdateDCStock (per line)
          -> ClsCommonInternalDAL.SaveDataByInsertCommand / UpdateDataByUpdateCommand
               -> DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB")  [new SqlConnection #2..N, opened+closed]
     -> aInvoiceBll.UpdateInvoiceDetailConsumer / UpdateInvoiceConsumer
          -> ClsCommonInternalDAL.UpdateDataByUpdateCommand -> [new SqlConnection, opened+closed]
     -> aInvoiceBll.PostDeliveryConfFullJournal(invId) -> InvoiceDAL.PostDeliveryConfFullJournal
          -> ClsCommonInternalDAL.UpdateAction("sp_Sales_ExecuteDeliveryPipeline", ...)
               -> ExecuteNonQueryAction -> DatabaseFactory.CreateDatabase("SolutionConnectionString" + "SSIDB")
                    [connection string suffix is a HARDCODED LITERAL "SSIDB" in ExecuteNonQueryAction,
                     Library.DAL/InternalCls/ClsCommonInternalDAL.cs:826 - confirmed by direct source read,
                     not derived from any caller parameter - UpdateAction has no dataBaseName parameter at all]
               -> stored procedure runs entirely inside THIS ONE connection/session, still pointed at
                  Initial Catalog=SalesRollDB_ZAS
     -> scope.Complete()  [only if every step above succeeded]
```

**Where the transaction starts**: the .NET `TransactionScope` constructor, in `saveButton_Click`. This creates an *ambient* transaction (`System.Transactions`), not a literal SQL `BEGIN TRAN` — no code anywhere in this call chain issues `BEGIN TRAN`/`COMMIT`/`ROLLBACK` T-SQL explicitly. Every `SqlConnection` opened while that ambient transaction is active auto-enlists in it (this is standard ADO.NET/`System.Transactions` behavior, not something this codebase configures explicitly).

**Which connection owns the transaction**: none of them individually — the ambient `TransactionScope` is the owner. Each DAL call (`SaveDataForInvoiceConsumer`, `SaveInvoiceDetails`, `UpdateInvoiceDetailConsumer`, `UpdateInvoiceConsumer`, `PostDeliveryConfFullJournal`) opens its **own**, separate `SqlConnection` via `DatabaseFactory.CreateDatabase(...)`, uses it, and lets it close — none of them are reused or passed between calls. This was independently re-confirmed by reading `ExecuteNonQueryAction`, `SaveDataByInsertCommandWithIdentity`, and `UpdateDataByUpdateCommand` — all three follow the identical "open fresh `Database`/`SqlConnection`, execute, implicitly close" pattern.

**Does the journal operation use the SAME connection or SAME SqlTransaction as the invoice/stock work?** No — it uses a **different `SqlConnection` object**, opened fresh by `UpdateAction`'s call into `ExecuteNonQueryAction`. It does **not** use the same `SqlTransaction` object either, because no code here creates an explicit `SqlTransaction` at all — everything relies on ambient enlistment. What it **does** share is: the same *ambient `System.Transactions` transaction* (because it's still opened while `saveButton_Click`'s `TransactionScope` is on the call stack) and the same *connection string* (`SolutionConnectionStringSSIDB`, i.e. `SalesRollDB_ZAS`).

## 3. CSTL_ACCDB Transaction Analysis

Re-confirmed directly against the live database (read-only queries):
- `sys.databases` lists `CSTL_ACCDB` and `CSTL_ERPDB` as `ONLINE` **on the same SQL Server instance** (`TOWSIF\MSSQLSERVER2019`) that hosts `SalesRollDB_ZAS`.
- `sys.servers WHERE is_linked=1` returned **zero rows** — there is no linked-server configuration involved. `CSTL_ACCDB..tblDebitCreditVoucherMaster` and `CSTL_ERPDB..Table_Integration_AccountsConnectivity` are plain **same-instance, cross-database** T-SQL references (three-part naming), not linked-server or remote references.
- `sp_Sales_ExecuteDeliveryPipeline` (the proc `PostDeliveryConfFullJournal` calls) itself contains no `BEGIN TRAN`/`COMMIT`/`ROLLBACK`/`SET XACT_ABORT`. Neither do the two procs it calls in turn (`sp_Process_WeightedAverageSalesConfirmation`, `sp_Sales_DeliveryCOGSPosting`) — re-read in full from `sys.sql_modules` this session, no transaction-control statements found anywhere in any of the three.

**What this means concretely**: the `INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster`/`...Detail` statements inside `sp_Sales_DeliveryCOGSPosting` execute over the **exact same SQL Server session/connection** that `UpdateAction` opened against `SalesRollDB_ZAS`. SQL Server natively treats same-instance cross-database access within one session as part of that session's current transaction context — this is standard SQL Server behavior and requires no special configuration. Since that connection is enlisted in the ambient `TransactionScope`, the `CSTL_ACCDB` writes are, transactionally, no different from a write to `SalesRollDB_ZAS` on the same connection: if the ambient transaction is never completed, they roll back with everything else.

## 4. Distributed Transaction / MSDTC Risk

**Risk assessment: LOW, and structurally the same risk already proven safe elsewhere in this investigation.**

- MSDTC is confirmed **stopped** on this dev machine (re-checked, `Get-Service MSDTC` → `Stopped`) — so if any part of this chain required distributed-transaction promotion, it would fail outright with an MSDTC error.
- Cross-database access via three-part naming (`CSTL_ACCDB..table`) within **one single `SqlConnection`/session** does **not** trigger DTC promotion — DTC promotion in `System.Transactions` is driven by the *number and overlap of `SqlConnection` objects* enlisted in the ambient transaction, not by which databases those connections' T-SQL happens to touch. Since the `CSTL_ACCDB` access happens entirely inside server-side T-SQL on a connection whose ADO.NET-visible target is only ever `SalesRollDB_ZAS`, ADO.NET/`System.Transactions` never "sees" a second database at all.
- The genuine risk category for DTC promotion in this codebase is **the number of sequential `SqlConnection` objects opened against `SalesRollDB_ZAS` within one `TransactionScope`** (since each DAL call opens its own). This was the exact scenario already tested and proven safe: `STK-REG-001` (2 sequential connections, ByOrder page) and `DS-T03`/`DS-T04` (3+ sequential connections: order, master, detail, all rolled back together, live-verified, no MSDTC error). `PostDeliveryConfFullJournal`'s connection is simply one more connection of the identical shape (same connection string, opened and closed sequentially, never overlapping with another open connection) — there is no structural reason it would behave differently from the connections already proven safe.
- **What was not directly observed**: an actual live run reaching `PostDeliveryConfFullJournal` inside the transaction and either succeeding or being forced to fail. This is the one gap between "architecturally sound, consistent with proven behavior" and "empirically confirmed for this exact step" — see §8.

## 5. Journal Failure/Rollback Analysis

Traced precisely through `ClsCommonInternalDAL.UpdateAction` (`Library.DAL/InternalCls/ClsCommonInternalDAL.cs:656-669`, read directly this session):

```csharp
internal bool UpdateAction(string StoreProcedureName, List<SqlParameter> SqlParameterlist)
{
    try
    {
        ActionStatus = Convert.ToBoolean(ExecuteNonQueryAction(StoreProcedureName, SqlParameterlist, string.Empty, false));
    }
    catch (Exception ex)
    {
        ClsErrorLogger.Log("UpdateAction", StoreProcedureName, ex);
        ActionStatus = false;
    }
    return ActionStatus;
}
```

**Key finding, confirmed by direct source read**: `UpdateAction` **catches and swallows** any exception thrown during journal posting — it does **not** re-throw. This means:
- A genuine SQL error deep inside `sp_Sales_DeliveryCOGSPosting` (e.g. a constraint violation on the `CSTL_ACCDB` voucher insert) does **not** propagate as an unhandled exception up through `PostDeliveryConfFullJournal` — it is logged (via `ClsErrorLogger`, confirmed this exact method already logs correctly) and the method returns `false`.
- The Direct Sales fix's `saveButton_Click` (`Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs`, re-read this session) explicitly checks this return value: `if (!aInvoiceBll.PostDeliveryConfFullJournal(invId)) { showMessageBox("Invoice could not be posted..."); return; }` — this `return` happens **inside** the `using (TransactionScope scope = ...)` block, **before** `scope.Complete()` is ever reached.
- Disposing a `TransactionScope` without calling `Complete()` rolls back **every enlisted resource** — Order, Invoice Master, Invoice Details, Stock deduction, `UpdateInvoiceDetailConsumer`/`UpdateInvoiceConsumer`, and any partial work `sp_Sales_DeliveryCOGSPosting` had already done (e.g., a `tblDebitCreditVoucherMaster` row inserted before a later `tblDebitCreditVoucherDetail` insert failed) — **all roll back together**, because all of it ran on connections enlisted in that one ambient transaction.
- **Conclusion**: no accounting data can remain partially committed if journal posting fails, and no Invoice/Order/Stock data can remain committed without its corresponding journal entry either — both directions of atomicity hold, by the same mechanism already proven for the invoice/stock side.

**One caveat worth naming explicitly**: `UpdateAction` also returns `false` for the *ordinary, non-error* case of "0 rows affected" (e.g., if the pipeline runs successfully but the COGS proc's own business logic decides `@TotalCost <= 0` and skips posting entirely — see `sp_Sales_DeliveryCOGSPosting`'s `IF (@TotalCost > 0)` guard). In that case, the page fix will *also* roll back the entire invoice with a generic "Invoice could not be posted" message, even though nothing was actually wrong — this is a **pre-existing ambiguity in `UpdateAction`'s return contract** (it conflates "zero rows affected because nothing needed posting" with "zero rows affected because of a real failure"), not something introduced by this fix, but worth flagging: a zero-cost Direct Sales line (unlikely but possible, e.g. all-bonus/zero-price items) could now cause a previously-silent no-op to instead block the entire invoice. Recommend confirming with the business whether a $0 COGS Direct Sales invoice is a realistic scenario before go-live.

## 6. Print Verification Analysis

Re-read directly from source this session (`Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx` and `.aspx.cs`):

- **Print button/event**: `<asp:Button ID="printButton" runat="server" OnClick="printButton_Click" Text="Print" />` (aspx line 464) → `printButton_Click` (`.aspx.cs:769-776`).
- **Report/page URL**: `../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=<invoice number>&invoiceId=<invoice id>`.
- **Required parameters**: `InvNo` (from `invTextBox.Text`, set inside `SaveInvoice()` from the value returned by `InvoiceBLL.SaveInvoiceConsumer`'s `out invoiceNo`) and `invoiceId` (from `rptInvoiceId.Text`, set in `saveButton_Click` **after** the transaction commits — `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs`, the line immediately following the `try/catch` block).
- **Invoice ID propagation**: `rptInvoiceId` is a server-side `asp:Label` (`Visible="False"`, aspx line 461) — plain WebForms controls retain their `.Text` across postbacks via ViewState by default; no `EnableViewState="false"` was found anywhere in this page (grepped, zero matches) or the master page reference that would break this. Since Save and Print are two separate button postbacks on the same page lifecycle, the label correctly carries the just-committed `InvoiceId` from the Save postback into the Print postback.
- **Whether the new `SCOPE_IDENTITY()`-generated `InvoiceId` is correctly passed**: yes — `invId` (the real, committed ID returned by `SaveInvoice()` → `SaveInvoiceConsumer`) is assigned to `rptInvoiceId.Text` only after `scope.Complete()` succeeds; there is no other code path that could populate this label with a stale or fabricated value.
- **Whether any old IDENTITY_INSERT assumption remains**: none — confirmed by grep (§ Regression Audit below) that no `PrimaryKeyMax("invoiceId","tblInvoice")` or manual-ID logic remains anywhere in this page's call chain.
- **Report data source**: not traced further in this pass (Crystal Reports binding inside `InvoiceReportViewer.aspx.cs`, unchanged since the earlier print-flow fix session and outside this audit's stated focus on the journal-transaction risk).

### Manual browser/IIS test procedure for Print

Requires a running IIS instance pointed at this site with a working `SalesRollDB_ZAS` connection — not available in this automated session.

1. Log in as one of the 3 users granted the "Direct Sales" menu item (per the earlier menu audit: `UserId` 2, 7, or 25).
2. Navigate to Direct Sales, fill in company/DC, a customer (existing, matched by phone, to avoid triggering `ConsumeCustomer`/GL creation during this manual test unless that's also being verified), payment type, and one product line with available stock.
3. Click **Save**. Expect: "Create Invoice" success message, no ASP.NET error page.
4. Click **Print** (same page, same session, no reload in between). Expect: a new browser window/tab opens `InvoiceReportViewer.aspx?InvNo=...&invoiceId=...` with the `invoiceId` query parameter populated with a real positive integer (visible in the browser's address bar/dev tools network tab).
5. Confirm the rendered report shows the correct customer, product line(s), quantity, and total matching what was just saved — not blank, not an "Invoice ID is missing or invalid" error.
6. Re-run steps 2-5 a second time in the same browser session (without reloading the page) to confirm repeated Save→Print cycles each produce their own distinct, correct invoice — not a stale ID from the first save.
7. Separately, open `InvoiceReportViewer.aspx` directly with `invoiceId=0`, `invoiceId=abc`, and no `invoiceId` param at all — confirm each shows the "Invoice ID is missing or invalid" message, not a blank report (regression check on the existing `InvoiceReportViewer.aspx.cs` guard, unmodified by this fix).

## 7. Regression Findings

Confirmed via direct `git diff` inspection of the actual changed hunks in `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs` (4 hunks total: the `using System.Transactions;` addition, `saveButton_Click`, `SaveInvoiceDetail`'s return-value fix, and the `printButton_Click` URL — the last one confirmed to be a **pre-existing** change from the earlier print-flow session, not touched again in this fix):

| Item | Status | Evidence |
|---|---|---|
| `OrderSaveMaster` | **UNCHANGED** | Zero lines in the diff touch this method |
| `OrderSaveDetail` | **UNCHANGED** | Zero lines in the diff touch this method |
| ORD-001 | **NOT TOUCHED** | Repo-wide grep for `RemainingQty`/`RemainingOrderQty` across the full diff returns nothing |
| ByOrder page | **NOT MODIFIED** | `git status` confirms `InvoiceCreationForCustomerByOrder.aspx(.cs)` carry only their pre-existing (prior-session) modifications; no Edit/Write touched them in this or the previous fix session |
| `ConsumeCustomer` | **UNCHANGED** | Zero lines in the diff touch this method |
| `ConsumerCustomerDAO` | **UNCHANGED** | Not referenced in the diff at all; separately re-confirmed `tblConsumerCustomer.ConsumerCustId` is **not** an identity column (`sys.columns.is_identity=0`, re-checked this session), so this class never needed a fix |
| Self-Approval logic | **UNCHANGED** | `ApprovedBy`/`ApprovedDate`/`ActionStatus="Accepted"` assignments inside `OrderSaveMaster` — zero lines touched |
| Discount handling | **UNCHANGED** | `DiscountPercentage = 0; DiscountAmount = 0; BonusQuantity = 0;` inside `SaveInvoiceDetail` — zero lines touched (only the *return statement* after this block was changed, not the discount assignment itself) |

**Stale IDENTITY_INSERT / manual InvoiceId check** (re-run this session, not merely cited from the prior report):
```
grep "PrimaryKeyMax" Library.BLL/SInventory_BLL/InvoiceBLL.cs
```
→ 3 matches remain: `SaveInvoice` (line 124, the *unused* plain Customer page — correctly left broken, out of scope), `SaveReturnInvoice` (line 147, the *superseded* Return Invoice page — correctly left broken, out of scope), and `SaveConsumerCustomer` (line 51, `tblConsumerCustomer` — correctly retained, since that table is genuinely not an identity column). **Zero matches remain for the Direct Sales invoice-master path** (`SaveInvoiceConsumer`/`SaveDataForInvoiceConsumer`) — confirmed clean.

## 8. Exact Remaining Risks

1. **Journal posting inside the transaction was never executed live**, by explicit instruction (to avoid creating real accounting vouchers). Everything in §2-5 is a structural/architectural analysis backed by direct source and schema reads, and by extrapolation from three *other* live-tested instances of the identical multi-connection-under-one-`TransactionScope` pattern — but it is not itself an observed test result. This is the single largest gap between "sound by design" and "proven."
2. **`UpdateAction`'s ambiguous return contract** (§5): a legitimate zero-cost posting (no error, nothing to post) and a genuine SQL failure both return `false` and are now both treated as "roll back the whole invoice" by this fix. Whether this is ever a realistic scenario for Direct Sales needs a business-side answer before go-live.
3. **No double-submission guard** on this save path (already flagged in the prior fix report, re-confirmed unchanged — `sp_I_InvoiceMaster`'s dedup guard is specific to the ByOrder page's stored procedure and has no equivalent here).
4. **Schema-parity caveat** carries forward unchanged from every prior report in this investigation: all findings are against the local dev database; production schema parity for `tblInvoice.InvoiceId` (identity) and the `CSTL_ACCDB`/`CSTL_ERPDB` cross-database layout should be confirmed before relying on any of this in production.
5. **Print was not re-verified live** in this audit either (no IIS/browser session available) — the manual procedure in §6 is provided for staging execution.

## 9. Staging/UAT Test Plan

All scenarios use a **staging or UAT database — never production** — with the exact same isolated-batch technique already used throughout this investigation (a dedicated test product/`DCStoreId` whose stock is snapshotted before and restored after each run, so no real inventory or accounting history is disturbed). A designated **staging-only test customer and product** should be set up in advance so no real customer/GL account is touched.

### Scenario A — Successful Direct Sales Invoice
**Setup**: staging test product with known stock (e.g. 100 units), a staging test customer already present in `tblCustMaster` (to avoid exercising `ConsumeCustomer`/GL-creation in this scenario — see Scenario-specific customer-creation test separately if needed).
**Steps**: Save one line item within available stock; click Print.
**Verify**:
- `tblOrder`: exactly one new row, `ActionStatus='Accepted'`, `ApprovedBy`/`ApprovedDate` populated, `IsDirect=1`, `IsManual=1`.
- `tblOrderDetail`: exactly one new row linked to that `OrderId`.
- `tblInvoice`: exactly one new row, `InvoiceId` matches what was passed to Print, `IsDirect='True'`.
- `tblInvoiceDetail`: exactly one new row, `InvoiceId` matches the master, `DiscountPercentage=0`, `DiscountAmount=0`.
- `tblDCStore`: `StockQty` reduced by exactly the sold quantity.
- `CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster`/`tblDebitCreditVoucherDetail`: a new voucher pair exists, `CommonPK_ID` = the invoice's `InvoiceId`, `TableID=20`, debit/credit amounts balance and match the COGS calculation.
- Print opens a report window showing correct invoice data with the same `InvoiceId`.

### Scenario B — Insufficient Stock
**Setup**: staging test product with stock deliberately set below the intended sale quantity (e.g. 10 units, sell 15).
**Steps**: Save.
**Verify**:
- User sees "Invoice items could not be saved..." message, not a success message.
- `tblOrder`: **no** new row for this attempt.
- `tblInvoice`/`tblInvoiceDetail`: **no** new rows.
- `tblDCStore.StockQty`: unchanged.
- `CSTL_ACCDB.tblDebitCreditVoucherMaster`: **no** new voucher (nothing to roll back, since the flow never reached journal posting).

### Scenario C — Journal Posting Failure
**Setup**: the hardest scenario to stage safely without touching real accounting config — recommend one of: (a) a staging-only test `CompanyId` intentionally **not** present in `Table_Integration_AccountsConnectivity` (so `@DrGl`/`@CrGl` fall back to their hardcoded defaults `4162`/`4156` — confirm whether those default GL accounts exist in the staging `CSTL_ACCDB`; if not, the insert into `tblDebitCreditVoucherDetail` should fail on an FK/lookup constraint), or (b) temporarily and reversibly renaming/disabling a constraint in the **staging** `CSTL_ACCDB` copy only (never production) to force a failure, restoring it immediately after.
**Steps**: Save a normal, sufficient-stock invoice under the condition set up above.
**Verify**:
- User sees "Invoice could not be posted..." message.
- `tblOrder`: **no** row remains (rolled back).
- `tblInvoice`/`tblInvoiceDetail`: **no** rows remain (rolled back).
- `tblDCStore.StockQty`: unchanged (rolled back).
- `CSTL_ACCDB.tblDebitCreditVoucherMaster`: **no** row remains, even if the pipeline got as far as inserting the voucher master before failing on the detail insert — this is the specific claim this scenario exists to prove.
- `DatabaseErrors.log` (`App_Data\DatabaseErrors.log`) contains a new entry for `Operation=UpdateAction`, `StoredProcedure=sp_Sales_ExecuteDeliveryPipeline`.

### Scenario D — Zero Stock
**Setup**: staging test product with `StockQty=0`.
**Steps**: Save.
**Verify**: identical checklist to Scenario B — no order, no invoice, no detail, stock unchanged at 0, no journal voucher.

### Scenario E — Discount = 0
**Setup**: any normal Direct Sales save (Scenario A's setup is sufficient).
**Steps**: Save with a product that would carry a non-zero discount on the ByOrder page (to confirm Direct Sales does NOT pick up that discount).
**Verify**:
- `tblInvoiceDetail.DiscountPercentage=0`, `DiscountAmount=0`, `BonusQuantity=0` for every line.
- `tblInvoiceDetail.NetAmount`/`TotalPrice` reflect full undiscounted price.
- `CSTL_ACCDB` COGS voucher amount matches `SUM(Quantity * UnitPrice_CostPrice)` from `tblDCStore` — i.e., the accounting entry reflects the actual (undiscounted-sale) cost basis, not a discounted figure.

## 10. Production Deployment Checklist

- [ ] Confirm production `tblInvoice.InvoiceId` is `IDENTITY` (matches the assumption this entire fix is built on) — do **not** deploy if production's schema differs on this point without re-evaluating.
- [ ] Confirm production has `CSTL_ACCDB`/`CSTL_ERPDB` on the **same SQL Server instance** as the main application database (required for the cross-database T-SQL in `sp_Sales_DeliveryCOGSPosting` to work at all, transactionally or otherwise).
- [ ] Confirm production's default GL accounts (`4162`, `4156`) exist and are valid, or that `Table_Integration_AccountsConnectivity` is correctly populated for every `CompanyId`/`PageId=20` combination that will use Direct Sales.
- [ ] Run Scenario A through E (§9) in a staging environment that mirrors production schema before enabling this fix for the 3 real users.
- [ ] Execute the manual Print procedure (§6) in staging.
- [ ] Confirm MSDTC's status in the **production** environment (this audit's confidence about avoiding MSDTC promotion was partly informed by observing this dev machine's behavior with MSDTC stopped — if production has MSDTC running, that removes this specific risk category entirely regardless; if production also has it stopped, the findings in §4 apply directly).
- [ ] Communicate to the business the answer needed for Remaining Risk #2 (§8) before go-live: is a $0-COGS Direct Sales invoice a real scenario, and if so, should it be allowed to succeed without a journal entry rather than being rolled back?
- [ ] After deployment, monitor `App_Data\DatabaseErrors.log` for the first few days of real Direct Sales usage — this is now the first and only visibility into any `UpdateAction`-swallowed failure in this flow.
