# Accounting Integration Audit

Scope: every literal accounting-keyword hit in `Library.BLL/**/*.cs` and `Library.DAL/**/*.cs`, cross-referenced
against the posting-procedure call sites already catalogued in
[`../../specs/00-system/integrations.md`](../../specs/00-system/integrations.md) and
[`../database/stored-procedures.md`](../database/stored-procedures.md), plus the known findings in
[`../../specs/SPEC-GAPS.md`](../../specs/SPEC-GAPS.md) (`GAP-APP-016..020`, `GAP-DB-001`).

**Headline, stated up front and verified below:** every accounting-posting call site found in `Library.BLL` /
`Library.DAL` is a call to an opaque stored procedure taking 0–2 scalar id/amount parameters. With exactly one
partial exception (`sp_PMNT_CustomerPaymentDetailInsert`), no C# code anywhere in these two projects constructs a
Dr/Cr line, an account code, a VAT split, or a COGS figure before calling a posting procedure. `GAP-DB-001` (no
procedure bodies) is confirmed still open, and — as the task brief already flagged — it is trivially closeable:
`docs/database/tables.md`/`views.md`/`functions.md` were regenerated 2026-09-05 against a **live** SQL Server
connection (213 tables, 24 views with bodies, 7 functions with bodies), proving live DB access existed in this
repo's recent history, yet `docs/database/stored-procedures.md` still marks all 119 procedure bodies "NOT
AVAILABLE." Nobody ran `sp_helptext` against the ~35 posting procedures specifically. That single omission is
the reason this document cannot say whether debit ever equals credit anywhere in this system.

---

## 1. Keyword grep results — real accounting call sites only

Grepped case-insensitively for `journal|voucher|GL|ledger|ChartOfAccount|debit|credit|COGS|VAT|tax|Posting` (plus
standalone `Dr`/`Cr`) across `Library.BLL/**/*.cs` and `Library.DAL/**/*.cs`. Coincidental hits (e.g.
`CustomerLedgerBLL`/`DAL` — a read-only statement-of-account screen, not a posting path; `CreditAdjustment`,
`CreditBalance`, `CreditLimit` — customer credit-limit business logic, not GL debit/credit; `LedgerHead`/
`SubsidaryCode` dropdown loaders for master-data screens) are excluded from the table below because they don't
touch a posting procedure. Every row that remained is one of these two shapes:

**Shape A — pure pass-through (the overwhelming majority):**

| File:Line | Method | What it actually does |
|---|---|---|
| `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1653-1657` | `PostJournal(int invId)` | Builds one `SqlParameter("@InvoiceId", invId)`, calls `aCommonInternalDal.UpdateAction("sp_ProformaPosting", ...)`. No amount, no account, no VAT. |
| `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1660-1665` | `PostProformaReturnJournal(int invId)` | Same shape, `sp_ProformaFullReturnPosting`, `@InvoiceId` only. |
| `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1667-1676` | `PostDeliveryConfFullJournal(int invoiceId)` | Same shape, `sp_Sales_ExecuteDeliveryPipeline`, `@InvoiceId` only. Two alternate procedure calls (`sp_DeliveryConfirmationFullPosting`, `sp_Sales_DeliverySalesPosting`) are commented out immediately above the live call (lines 1673-1674) — the app used to call a different/additional procedure here. |
| `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1686-1694` | `PostDeliveryConfPartialJournal(int invoiceId)` | Same shape, `sp_Sales_DeliverySalesPosting`, `@InvoiceId` only; `sp_DeliveryConfirmationPartiallPosting` commented out above it (line 1692). |
| `Library.DAL/SInventory_DAL/CustPaymentDAL.cs:531-539` | `PaymentJournalPosting(int custPayId, int invoiceId)` | `sp_Sales_CustomerPaymentPosting`, `@CustPayId`+`@InvoiceId` only. `sp_CustomerPaymentPosting` commented out on line 539. |
| `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:80-88` | `PaymentJournalPosting(int custPayId, int invoiceId)` | Identical shape/comment to the above (duplicated logic in a second DAL). |
| `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:273-281` | `PaymentJournalPosting(int custPayId, int invoiceId)` | `sp_Sales_CustomerPaymentReversePosting`, `@CustPayId`+`@InvoiceId` only; `sp_CustomerPaymentPosting` commented out on line 281 — note this DAL uses the *reverse* procedure under the same method name as the two DALs above use the forward procedure, i.e. three DALs, three near-identical methods, two different target procedures. |
| `Library.DAL/SInventory_DAL/DestroyProductDal.cs:227-234` | `PostJournal(int destroyId)` | `sp_Sales_ProductDestroyPosting`, `@DestroyId` only. |
| `Library.DAL/SInventory_DAL/SampleIssueDal.cs:411-416` | `PostJournal(int masterId)` | `sp_SampleInvoicePosting`, one parameter passed **as `@InvoiceId`** even though it is a sample-issue master id, not an invoice id (naming carried over from the invoice posting pattern). |
| `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:115-121` | `PostJournal(int stockInMasterId)` | `sp_WHStockInPosting`, `@MasterId` only. **Dead code** — see §2, no live caller invokes this method (§4/BUG-ACC-004). |
| `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:125-136` | `ApproveWarehouseStockInMaster(...)` | `sp_Acc_MaterialConversionReceive`, 4 scalar params (`@ApprovalStatus`,`@MasterId`,`@ApprovalBy`,`@ApprovalDate`) — an approval-status writer, not a GL construction call; no amount/account passed. |
| `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:133-141` | `PostFreezeStockJournal(int, decimal)` | `sp_FreezeStockPosting`, `@DcStockConditionId`+`@Quantity` — a quantity, not a monetary amount or account code. |
| `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:125-133` | `FreezeStockReleasePosting(int, decimal)` | `sp_FreezeStockReleasePosting`, `@FreezeId`+`@Quantity`, same shape. |
| `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:138-145` | `PostDeliverySalesReturn(int custReturnId)` | `sp_Sales_DeliverySalesReturnPosting`, `@ReturnId` only. |
| `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs:286-293` | `ApproveTradeMaterialTransfer(int, string)` | `sp_Process_TradeMaterialTransferFullProcess`, `@TradeMaterialTransferId`+`@ApprovedBy` — no product cost/account passed; if any inter-unit inventory value transfer happens it happens entirely inside the procedure. |
| `Library.BLL/SInventory_BLL/CustomerMasterInfoDal.cs` (via `CustomerMasterInfoDal.cs:145-`, `sp_I_CustomerGl`/`sp_Integration_CustomerSubsidaryInsert`) | `CreateGlOfCustomer` | Two id-only procedure calls (`@CustomerId`,`@SubsidiaryLId`) to create/link a customer's GL account. No account-type or opening-balance data passed from C#. |

**Shape B — partial exception, the only place the C# picks an account and computes a value:**

| File:Line | Method | What it actually does |
|---|---|---|
| `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:267-280` | `SaveCustomerPaymentDetail(CustPaymentDetail aPaymentDetail)` | Builds parameters for `sp_PMNT_CustomerPaymentDetailInsert`: `@BankAccId` (line 274), `@CashAccId` (line 275), `@AIT` (line 279, tax), `@Discount` (line 280) — all populated from the `CustPaymentDetail` DTO, which is populated in the code-behind from which radio button (`Cash`/`Bank`) the user picked and a computed discount. **This is the one call site where the C# layer chooses the cash-vs-bank account and pre-computes a tax/discount figure before the procedure runs** — everything else in this audit is pure id pass-through. See §3 for detail. |

No other Shape-B call site was found. Grep for standalone `Dr`/`Cr` tokens (as opposed to substrings like
`CreditLimit`) produced no matches referring to debit/credit columns or account sides anywhere in Library.BLL or
Library.DAL — the only place those letters appear is inside identifiers like `CredentialProvider`-style names,
none of which are accounting-related. `COGS` as a literal token: zero matches in either project.

---

## 2. Transaction-type table: does C# construct the entry, or trust an opaque procedure?

| Transaction type | (a) constructs entry / (b) opaque call | Evidence (file:line) |
|---|---|---|
| Sales Invoice — Proforma save | (b) | `InvoiceDAL.cs:1653-1657` → `sp_ProformaPosting(@InvoiceId)` |
| Sales Invoice — Delivery confirmation (full) | (b) | `InvoiceDAL.cs:1667-1676` → `sp_Sales_ExecuteDeliveryPipeline(@InvoiceId)` |
| Sales Invoice — Delivery confirmation (partial) | (b) | `InvoiceDAL.cs:1686-1694` → `sp_Sales_DeliverySalesPosting(@InvoiceId)` |
| Purchase / Warehouse stock-in | (b), and in practice **not called at all** | `WarehouseStockInApprovalDal.cs:115-121` (`sp_WHStockInPosting(@MasterId)`) exists but its only caller, `WarehouseStockInApproval.aspx.cs:179`, is commented out (§4/BUG-ACC-004). `ApproveWarehouseStockInMaster` (`WarehouseStockInApprovalDal.cs:125-136`, `sp_Acc_MaterialConversionReceive`) is called and passes only approval-status scalars, no amount/account. |
| Collection / Customer payment | (b) for the journal, **partial (a)** for account/tax selection | Journal: `CustPaymentDAL.cs:531-539` / `PaymentTranscationDal.cs:80-88` → `sp_Sales_CustomerPaymentPosting(@CustPayId,@InvoiceId)`. Account/tax selection: `PaymentTranscationDal.cs:267-280` → `sp_PMNT_CustomerPaymentDetailInsert` (see Shape B above). |
| Payment reversal | (b) | `BatchWiseCollectionReportDal.cs:273-281` → `sp_Sales_CustomerPaymentReversePosting(@CustPayId,@InvoiceId)` |
| Return (proforma) | (b) | `InvoiceDAL.cs:1660-1665` → `sp_ProformaFullReturnPosting(@InvoiceId)`; `ProformaOrInvoiceReturnBLL.cs:72-79` → `sp_ProformaReturnPosting`/`sp_DeliveryInvoiceDeletePosting`, both id-only |
| Return (delivery/customer) | (b) | `CustomerReturnDAL.cs:138-145` → `sp_Sales_DeliverySalesReturnPosting(@ReturnId)` |
| Return Acknowledgment Note (RAN) | **no posting call exists at all** | `ReturnAcknowledgmentNoteDal.cs:63-64` — the posting call is commented out (`// PaymentJournalPosting(ranMasterId, detail.ProductId);`) with the comment "Optional: ... if needed." No procedure is ever invoked for a RAN. |
| Product Destroy / write-off — request | **no posting call exists at all** | `DestroyProduct.aspx.cs:95` — `//aDal.PostJournal();` is commented out, yet the very next lines show `ShowMessageBox("Stock Out Successfully Done !!!!")`. |
| Product Destroy / write-off — approval | (b), return value discarded | `DestroyProductApproval.aspx.cs:107` → `aDal.PostJournal(...)` (`DestroyProductDal.cs:227-234` → `sp_Sales_ProductDestroyPosting(@DestroyId)`), called with no `if`/`else` around the return `bool` (§4/BUG-ACC-002). |
| Sample Issue | (b), return value discarded | `SampleProductrIssue.aspx.cs:953` → `aOrderCreationDal.PostJournal(masterId)` (`SampleIssueDal.cs:411-416` → `sp_SampleInvoicePosting`), return `bool` discarded (§4/BUG-ACC-002). |
| Trade Material Transfer | (b) | `TradeMaterialTransferDal.cs:286-293` → `sp_Process_TradeMaterialTransferFullProcess(@TradeMaterialTransferId,@ApprovedBy)` |
| Freeze / release stock | (b), return value discarded on release | `StockConditionFreeze.aspx.cs:540` → `aStockConditionFreezeBll.PostJournal(...)` (`StockConditionFreezeDAL.cs:133-141` → `sp_FreezeStockPosting`), discarded (§4/BUG-ACC-002); `OtherStockAction.aspx.cs:276` correctly checks the `bool` from `FreezeStockReleasePosting`. |

**Conclusion for §2:** the brief's expectation is confirmed. Every transaction type in the app is (b) —
opaque, id-only procedure calls — with the single Shape-B exception documented above for cash/bank account
choice and AIT/discount on customer payments. Nothing constructs a Dr/Cr pair, a GL account code (beyond
choosing cash-vs-bank), a VAT-account split, or a COGS figure anywhere visible in C#.

---

## 3. Is debit=credit, VAT-account mapping, or COGS-at-sale checkable? — confirmed UNKNOWN, with the one exception documented precisely

Checked `docs/database/stored-procedures.md` §I ("GL / subsidiary-ledger integration") and §A/§B for any
procedure whose parameter list contains both an amount and an account code (which would prove the C# — not the
procedure — decides where money posts):

- **§I** (`sp_I_CustomerGl`, `sp_UD_CustomerGl`, `sp_Integration_CustomerSubsidaryInsert`, `sp_I_SupplierGl`,
  `sp_I_ProductGl`) — every parameter list is `@CustomerId`/`@SupplierId`/`@SubsidiaryLId`, i.e. id-to-id links.
  No amount parameter anywhere in this section. These create/link GL *accounts*, they do not post *entries*.
- **§A** (delivery/invoice posting) — every posting procedure (`sp_ProformaPosting`, `sp_ProformaFullReturnPosting`,
  `sp_Sales_ExecuteDeliveryPipeline`, `sp_Sales_DeliverySalesPosting`, `sp_DeliveryInvoiceDeletePosting`,
  `sp_UD_InvoicePayment`) takes only `@InvoiceId` (`sp_DeliveryConfirmationFullPosting` additionally takes
  `@StockInTransfarId`). No amount, no account code, no VAT/COGS figure is passed from C# for any of these — the
  procedure must look up the invoice's stored amounts and VAT itself.
- **§B** (payment posting) — `sp_CustomerPaymentPosting`/`sp_Sales_CustomerPaymentPosting`/
  `sp_Sales_CustomerPaymentReversePosting` take only `@CustPayId`+`@InvoiceId`. The **one exception** is exactly
  as the task brief predicted: **`sp_PMNT_CustomerPaymentDetailInsert`** takes `@AIT` (tax), `@BankAccId`,
  `@CashAccId`, `@Discount`, `@PaymentAmount`, confirmed against the live call site at
  `PaymentTranscationDal.cs:267-280`. This means: **for customer payments only**, the C# layer (a) decides
  whether the payment is Cash or Bank and passes the corresponding account id, and (b) pre-computes an AIT
  (advance income tax) figure and a discount figure as plain decimals before the insert. It does **not** compute
  which GL account those decimals land in beyond the cash/bank choice — the procedure still owns the actual
  ledger-posting mechanics (which account AIT and Discount post to, whether a corresponding Dr/Cr pair is
  balanced) — so this is a partial, not a full, exception to the opaque-black-box pattern.

No other exception was found. Therefore, restating the brief's required claim, now verified rather than assumed:
**whether debit=credit, whether any line-level GL account mapping happens (beyond the cash/bank choice above),
whether VAT is posted to a separate account, and whether COGS is computed and posted at time of sale are ALL
UNKNOWN, blocked by `GAP-DB-001`**, because every other visible posting call site in `Library.BLL`/`Library.DAL`
passes 0–2 scalar id/quantity parameters and never inline Dr/Cr construction in C#.

---

## 4. External DB write-side: read (precondition) vs. write (posting) call sites, and the commented-out guards

**Reads — chart-of-accounts / financial-year precondition checks:**

| Call site | What it reads |
|---|---|
| `InvoiceDAL.cs:1643-1651` `GetFinancialYearInfo(invoiceDate, unitId)` | `CSTL_ACCDB..tblFinancialYear` joined through `dbo.tblCompanyUnit` |
| `InvoiceDAL.cs:1678-1685` `CheckChartOfAccount(customerCode)` | `ZAS_ACCDB..tblChartOfAccounts` filtered by customer id |
| `CustomerMasterInfoDal.cs:105,447` | `ZAS_ACCDB..tblChartOfAccounts` (account dropdowns) |
| `CustPaymentDAL.cs:507,524` | `CSTL_ACCDB..tblChartOfAccounts` (bank/cash account lists for the payment screen) |
| `PettyCashDaL.cs:31,136-137` | `ZAS_ACCDB..tblChartOfAccounts` (cash/particulars account lookups) |
| `WarehouseStockInDal.cs:34` | `ZAS_ACCDB..tblChartOfAccounts` (supplier account lookup) |

**Writes — posting procedures only** (all catalogued in §1/§2 above; none of them also read chart-of-accounts or
financial-year data from C# immediately beforehand at the same call site).

**Commented-out chart-of-accounts / financial-year guards — confirmed, quoted verbatim.** Grep for
`//.*ChartOfAccount` and `//.*FinancialYear` across `*.cs` found the precondition calls disabled at seven
call sites (this directly resolves open question #11 in `specs/FINAL-REPORT.md` §5 — the guards were disabled,
not merely never implemented):

- `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs:43`
  `//DataTable aTable = aInvoiceBll.CheckChartOfAccount(custCodeTextBox.Text.Trim());`
- `Solution.Web/SInventory_UI/DelivaryInvoiceCreationAuto.aspx.cs:118`
  `//DataTable aTable = aInvoiceBll.CheckChartOfAccount(orderGridView.Rows[rowindex].Cells[2].Text.Trim());`
  — this is the exact call site named in `GAP-APP-018` ("the chart-of-accounts precondition on this path is
  commented out"); confirmed directly in the source immediately after the double-success-message bug (§5/BUG-ACC-001).
- `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs:175`
  `//DataTable aTable = aInvoiceBll.GetFinancialYearInfo(DateTime.Today.ToString(CultureInfo.InvariantCulture), orderGridView.DataKeys[0][2].ToString());`
- `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs:82`
  `//DataTable aTable = aInvoiceBll.GetFinancialYearInfo(invDateTextBox.Text, hdComUnitId.Value);`
- `Solution.Web/SInventory_UI/ReturnustomerByInvoiceApproval.aspx.cs:186`
  `//DataTable aTable1 = aInvoiceBll.CheckChartOfAccount(loadGridView.DataKeys[0][3].ToString());`
- `Solution.Web/SInventory_UI/PaymentTranscationNew.aspx.cs:225,266`
  `//DataTable aTable = aInvoiceBll.GetFinancialYearInfo(...)` and `//DataTable aTable1 = aInvoiceBll.CheckChartOfAccount(...)`
- `Solution.Web/SInventory_UI/PaymentTranscation.aspx.cs:224,265`
  same pair of guards, same pattern

**Net effect:** `InvoiceDAL.GetFinancialYearInfo` and `InvoiceDAL.CheckChartOfAccount` are defined, and their
underlying `ZAS_ACCDB`/`CSTL_ACCDB` reads are real, but **no live UI call site invokes them any more** — every
call site that once did has the call commented out. So in the shipped code, invoices, deliveries and payments
are posted with **zero application-level precondition check** that an active financial year or a GL account
exists for the customer; if such a check exists at all today, it must be inside the posting procedures
themselves (unknown, `GAP-DB-001`).

---

## 5. Bugs found (accounting-integration, visible in C# only)

### BUG-ACC-001 — Delivery-confirmation posting: identical success message on success and failure, silent on double failure
- **Severity:** HIGH
- **Confidence:** CONFIRMED (formalizes `GAP-APP-018` with the exact code and line numbers)
- **Evidence:** `Solution.Web/SInventory_UI/DelivaryInvoiceCreationAuto.aspx.cs:97-115`. The `if` branch (line 99-105,
  `PostDeliveryConfFullJournal` returned `true`) shows `"Delivery Invoice Save Successfully !!"`. The `else`
  branch (line 106-115) re-checks an unrelated earlier `status` flag from `SaveFullInvoice` (line 94) and, if
  that happened to equal `1`, shows the **same** `"Delivery Invoice Save Successfully !!"` message even though
  the journal posting call just returned `false`. If both `PostDeliveryConfFullJournal` returns `false` and
  `status != 1`, no message is shown at all and the grid is not refreshed — the user gets no feedback that
  posting failed. The chart-of-accounts precondition that used to run before this code (§4) is commented out at
  line 118, directly below.
- **A near-identical pattern exists at** `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs:878-884` for the
  partial-delivery path (`PostDeliveryConfPartialJournal`); the `else` branch there was not fully inspected in
  this audit and should be checked if this bug is triaged for a fix.

### BUG-ACC-002 — Posting procedure's boolean return value discarded at three call sites
- **Severity:** HIGH
- **Confidence:** CONFIRMED
- **Evidence:**
  - `Solution.Web/SInventory_UI/DestroyProductApproval.aspx.cs:107` — `aDal.PostJournal(...)` called with no
    `if`/assignment; `DestroyProductDal.PostJournal` (`Library.DAL/SInventory_DAL/DestroyProductDal.cs:227-234`)
    returns `bool` from `sp_Sales_ProductDestroyPosting`. The approval flow proceeds and (per the surrounding
    code at lines 85-113) shows no success/failure branching tied to this specific call.
  - `Solution.Web/SInventory_UI/SampleProductrIssue.aspx.cs:953` — `aOrderCreationDal.PostJournal(masterId)`
    called with no `if`/assignment; `SampleIssueDal.PostJournal` (`Library.DAL/SInventory_DAL/SampleIssueDal.cs:411-416`)
    returns `bool` from `sp_SampleInvoicePosting`. Lines 957-963 immediately show
    `ShowMessageBox("Sample Issued Successfully !!")` regardless of whether the journal posted.
  - `Solution.Web/SInventory_UI/StockConditionFreeze.aspx.cs:540` — `aStockConditionFreezeBll.PostJournal(DCID, ...)`
    called with no `if`/assignment; `StockConditionFreezeDAL.PostFreezeStockJournal`
    (`Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:133-141`) returns `bool` from `sp_FreezeStockPosting`.
  - Contrast: `Solution.Web/SInventory_UI/OtherStockAction.aspx.cs:276-282` **does** check the return value of
    the sibling method `FreezeStockReleasePosting` — proving the codebase knows how to check this return value
    and simply didn't at the three sites above.

### BUG-ACC-003 — Two accounting-posting call sites are fully commented out, with success shown or no compensating check
- **Severity:** HIGH
- **Confidence:** CONFIRMED
- **Evidence:**
  - `Solution.Web/SInventory_UI/DestroyProduct.aspx.cs:93-98` — the comment `// Post Journal` is immediately
    followed by `//aDal.PostJournal();` (commented out), then unconditionally `ShowMessageBox("Stock Out
    Successfully Done !!!!")`. No accounting entry is attempted at all on this screen; if this screen is meant to
    post independently of the separate `DestroyProductApproval.aspx` step, a stock write-off can be recorded in
    `SalesRoll` with no corresponding GL effect until (if ever) approved.
  - `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs:63-64` — `// Optional: PaymentJournalPosting or
    other post-processing here if needed` / `// PaymentJournalPosting(ranMasterId, detail.ProductId);` — no
    posting procedure is called anywhere in the Return Acknowledgment Note flow. There is no
    `sp_*Posting` procedure listed for RAN in `docs/database/stored-procedures.md` §H either, so this may be
    intentional (RAN may be a non-financial acknowledgment step), but it is asserted here as SUSPECTED, not
    CONFIRMED, pending product-owner confirmation — flagging it because the surrounding comment ("if needed")
    shows the original author was unsure too.

### BUG-ACC-004 — Warehouse stock-in GL posting procedure is dead code: never invoked in the shipped path
- **Severity:** CRITICAL
- **Confidence:** CONFIRMED
- **Evidence:** `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:115-121` defines
  `PostJournal(int stockInMasterId)` → `sp_WHStockInPosting(@MasterId)`. Its only call site anywhere in
  `Solution.Web` is `Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx.cs:176-183`, where it is commented
  out (`//     approvalBll.PostJournal(stockInMasterId);` at line 179), alongside a second commented-out call
  (`//approvalBll.InsertDataIntoCenterlWarehouse(...)` at line 177). Only `approvalBll.ApprovalUpdateBLL(aMasterDao)`
  (line 182) actually runs. This means **warehouse stock-in approval, as shipped, never posts a GL journal at
  all** — the entire "post the accounting journal for a warehouse stock-in" behavior documented as INFERRED
  purpose in `docs/database/stored-procedures.md` §F is unreachable code. Rated CRITICAL rather than HIGH because
  (unlike BUG-ACC-002/003) this is not a discarded-return-value slip but a wholesale disabling of a financial
  posting path for an entire transaction type, with no comment explaining why or any alternate posting path found
  in this audit.

### BUG-ACC-005 — Chart-of-accounts / financial-year preconditions disabled at every live call site
- **Severity:** HIGH
- **Confidence:** CONFIRMED (formalizes open question #11 in `specs/FINAL-REPORT.md` §5, resolving it from
  "whether ... was disabled deliberately" to "confirmed disabled, reason UNKNOWN")
- **Evidence:** the seven call sites quoted verbatim in §4 above — `InvoiceDAL.CheckChartOfAccount` and
  `InvoiceDAL.GetFinancialYearInfo` are fully implemented, real-data-backed methods, but every UI call to either
  one across the delivery, invoice, and payment screens is commented out. No replacement guard was found in the
  C# in their place. Whether the underlying posting procedures re-implement this check internally is UNKNOWN
  (`GAP-DB-001`) — but as visible from the application layer, invoices/deliveries/payments can be posted for a
  customer with no chart-of-accounts row, or on a date outside any active financial year, and the application
  will not stop them.

---

## Not claimed

Per the task instructions, this audit does **not** assert or infer any stored-procedure-internal behavior (Dr/Cr
balancing, VAT account routing, COGS calculation, idempotency) beyond what is directly visible in the C# call
sites above. All such behavior remains `GAP-DB-001` / UNKNOWN and requires `sp_helptext` extraction of the ~35
procedures named in §1/§2 to resolve.
