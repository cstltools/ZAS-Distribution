# Forensic Audit — `SInventory_UI/InvoiceCreationForCustomerByOrder.aspx`

> Read-only investigation. No source file was modified to produce this report. All claims are
> tagged **CONFIRMED** (directly read in the C# source in this repo), **DOCUMENTED-UNVERIFIED**
> (found in a reverse-engineered `docs/database/*.md` transcript in this repo, believed to be an
> extraction from the live stored procedure in an earlier audit session, but not independently
> re-confirmed against the SQL Server instance in this session), or **UNCONFIRMED** (cannot be
> determined from repository contents at all). Per the audit brief, nothing in the working tree —
> committed or not — was changed to produce this document.

---

## 1. Executive Summary

The live "Save" path for this page (`saveButton_Click1`, wired via `OnClick="saveButton_Click1"` in
`InvoiceCreationForCustomerByOrder.aspx:504`) is materially better engineered than the rest of the
codebase: it is the **only** save flow in the entire repository that wraps Master + Detail + Stock +
Order-status updates in an ADO.NET `System.Transactions.TransactionScope`
(`InvoiceCreationForCustomerByOrder.aspx.cs:1412-1441`). A repo-wide grep for
`SqlTransaction|TransactionScope|BEGIN TRAN|COMMIT TRAN|XACT_ABORT` returns exactly these two lines
and nowhere else in `Library.BLL`, `Library.DAL`, `Library.DAO`, or any other `.aspx.cs` in the
solution.

However, that transaction's safety net is **defeated by a single line of code**: the private helper
`SaveInvoiceDetail(int invoiceId)` (`InvoiceCreationForCustomerByOrder.aspx.cs:1555-1643`) **always
returns `true`** (line 1642), regardless of whether any detail rows were actually built or whether
the underlying save succeeded. The caller's only failure gate —
`if (!SaveInvoiceDetail(invId)) { ...; return; }` (line 1431) — can therefore never fire. Combined
with the fact that any grid line whose displayed stock reads `0` at save time is silently dropped
from the list that gets persisted (line 1629-1636), the transaction can — and, per the evidence
below, will under a specific and reachable condition — commit an **Invoice Master row with zero
Invoice Detail rows**, reported to the user as `"Proforma Invoice Created Successsfully!!"`. When
that invoice is printed, the Crystal Report's detail query (`LEFT JOIN tblInvoiceDetail`) faithfully
returns a null-filled row: the header/company/customer block renders, the line-item table is empty —
i.e. exactly the "blank invoice print" symptom under investigation.

Independently, stock deduction (`InvoiceDAL.UpdateDCStoreQuantity`,
`Library.DAL/SInventory_DAL/InvoiceDAL.cs:799-803`) is a **read-then-blind-overwrite** —
`UPDATE tblDCStore SET StockQty='<value>' WHERE DCStoreId='<id>'` — with no `WHERE StockQty >= @qty`
guard and no decrement expression. The surrounding `TransactionScope` deliberately uses
`ReadCommitted` isolation (a comment at line 1411-1414 explains this was chosen specifically to
avoid range locks on `tblDCStore`), which does not protect this read-then-write pattern from a
classic lost-update race between two concurrent invoice saves on the same batch. Order-remaining-
quantity is never enforced anywhere except as a cosmetic red row color; the value that would be
needed to enforce it (`InvoiceBLL.LoadProductQty`) is loaded and then discarded (its result is never
read — `InvoiceCreationForCustomerByOrder.aspx.cs:1618` is commented out). Double-invoicing of the
same order is guarded only by a check-then-act `SELECT` (`OrderExists`,
`InvoiceCreationForCustomerByOrder.aspx.cs:166-179`) executed **before** the transaction scope opens,
so it is not itself transactionally protected against a second concurrent request.

**Overall risk rating: HIGH** (see §17). The transactional design intent is sound; the
implementation has a specific, low-effort-to-fix defect that silently breaks the guarantee it was
built to provide.

---

## 2. Scope

Investigation is scoped to the complete execution chain reachable from
`SInventory_UI/InvoiceCreationForCustomerByOrder.aspx`: its code-behind, every `Library.BLL` /
`Library.DAL` / `Library.DAO` method it calls (transitively), the shared print/report page it opens
(`InvoiceReportViewer.aspx`), and the Crystal Report it loads. Where a stored procedure body is not
present in the repository as raw SQL, its behavior is reported either as **UNCONFIRMED**, or, where a
prior reverse-engineering pass left a transcript in `docs/database/stored-procedures-part*.md`, as
**DOCUMENTED-UNVERIFIED** with the source cited. No code, database, or git state was modified.

---

## 3. Files Investigated (full list)

**UI / code-behind**
- `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` (654 lines, markup)
- `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` (1907 lines, full read)
- `Solution.Web/SInventory_RPTVIEW/InvoiceReportViewer.aspx.cs` (full read)
- `Solution.Web/SInventory_RPTVIEW/InvoiceReportViewer.aspx`

**Business layer**
- `Library.BLL/SInventory_BLL/InvoiceBLL.cs` (full read)
- `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` (full read)
- `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs`

**Data layer**
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs` (full read — `SaveInvoiceMater`, `SaveDataForInvoice`,
  `SaveDataForInvoiceDetails`, `BatchWiseProductQty`, `UpdateDCStoreQuantity`, `UpdateOrder`,
  `InvoiceNoCount`, `GetInvoiceNoById`, `DeleteInvoice`, `PostJournal`,
  `InvoiceMainDataForReport(string,string)`, `InvoiceDetailDataForReport(string,string)`)
- `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` (current on-disk working-tree version — this file
  shows as modified in `git status`; it was read as-is, not touched)
- `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs`
- `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` (full read — `DataContainerDataTable`,
  `SaveAction`, `UpdateAction`, `DeleteAction`, `ExecuteNonQueryAction`, `RunStoreProcedure`,
  `SaveDataByInsertCommand`, `UpdateDataByUpdateCommand`, `SaveDataByInsertCommandWithIdentity`)
- `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs` (full read — all 3 `PrimaryKeyMax` overloads)
- `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` (full read)

**Entities**
- `Library.DAO/SInventory_Entities/Invoice.cs`
- `Library.DAO/SInventory_Entities/InvoiceDetail.cs`

**Reports**
- `Library.CrystalReports/SInventory_RPT/rptInvoiceForCustomer.rpt` (located; binary Crystal Reports
  design file — internal formula/grouping logic is **UNCONFIRMED**, not human-readable from the repo)

**Reference / prior reverse-engineering artifacts already in the repo (read, not authored by this
audit)**
- `specs/sales-order-invoice/pages/PAGE-SAL-011-InvoiceCreationForCustomerByOrder.md` — an existing,
  independently produced reverse-engineering spec for this exact page. Used as a cross-check; every
  fact taken from it that materially affects a finding below is re-verified against the live source
  in this session and cited by file/line, not by the spec alone.
- `docs/database/stored-procedures-part2.md` (lines ~5218-5330+) — a prior-session transcript
  purporting to be the extracted body of `sp_I_InvoiceMaster`. Cited as **DOCUMENTED-UNVERIFIED**.
- `docs/audit/high-priority-bugs.md`, `docs/audit/sales-audit.md`,
  `docs/audit/accounting-integration-audit-followup.md` — checked for overlap with this report's
  findings; no duplicate of the findings below was found there.

**Other callers of the shared report page** (found via repo-wide grep, relevant to §11)
- `Solution.Web/SInventory_UI/BatchWiseTopSheet.aspx.cs`
- `Solution.Web/SInventory_UI/BatchWiseDelReturnSummery.aspx.cs`
- `Solution.Web/SInventory_UI/BatchWiseDeliveryTopSheet.aspx.cs`
- `Solution.Web/SInventory_UI/InvoiceCreationForConsumerCustomer.aspx.cs`
- `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs`
- `Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx.cs`
- `Solution.Web/SInventory_UI/ReturnOnDeliveryReport.aspx.cs`
- `Solution.Web/SInventory_UI/TopSheetGenerate.aspx.cs`
- `Solution.Web/SInventory_UI/CustomerDueReport.aspx.cs`, `InvoiceReport.aspx.cs`,
  `GroupInvoiceList.aspx.cs`, `GroupInvoiceEntry.aspx.cs` (these do pass `invoiceId`)

---

## 4. Dependency Map

```
InvoiceCreationForCustomerByOrder.aspx  (markup, saveButton OnClick="saveButton_Click1")
  |
  v
SInventory_UI_InvoiceCreationForCustomer  (code-behind class)
  |
  |-- Validation()                                    -- client-supplied totals/date/payment checks only
  |-- OrderExists(orderIdHiddenField.Value)            -- SELECT * FROM tblInvoice WHERE OrderId=@id  (TOCTOU gate)
  |
  |== TransactionScope(ReadCommitted, 5 min) ==========================================
  |     |
  |     |-- PrepareMasterData() -> Invoice entity (UI fields only, no DB call)
  |     |-- InvoiceBLL.SaveInvoiceMater(Invoice)
  |     |     -> InvoiceDAL.SaveInvoiceMater(Invoice)                 [InvoiceDAL.cs:1742-1768]
  |     |          -> ClsCommonInternalDAL.SaveAction("sp_I_InvoiceMaster", params, "@InvoiceId")
  |     |               -> ExecuteNonQueryAction(..., IsPrimaryKey:true)
  |     |                    -> db.AddOutParameter("@InvoiceId") ; db.ExecuteNonQuery(cmd)
  |     |                    -> SQL Server: EXEC sp_I_InvoiceMaster ... @InvoiceId OUT
  |     |                    => tblInvoice (INSERT)                    -- master row
  |     |          NOTE: SaveAction SWALLOWS exceptions -> returns pk=0 on ANY sql error
  |     |     <- invId  (0 on failure/duplicate; scope disposed WITHOUT Complete() => rollback)
  |     |
  |     |-- SaveInvoiceDetail(invId)                          [aspx.cs:1555-1643]
  |     |     for each grid row:
  |     |        if (currentStock == 0) -> InvoiceBLL.UpdateOrder("Undelivered", orderDetailsId)
  |     |                                    -> InvoiceDAL.UpdateOrder -> UPDATE tblOrderDetail SET Status=...
  |     |                                    (row EXCLUDED from the invoice)
  |     |        else                   -> add InvoiceDetail to aInvoiceDetailsList
  |     |     InvoiceBLL.SaveInvoiceDetails(aInvoiceDetailsList, comUnitId)   [InvoiceBLL.cs:194-202]
  |     |        foreach detail -> InvoiceBLL.UpdateDCStock(detail, comUnitId)  [InvoiceBLL.cs:218-321]
  |     |             -> InvoiceDAL.BatchWiseProductQty(productCode, comUnitId)     [InvoiceDAL.cs:938-942]
  |     |                  SELECT * FROM tblDCStore WHERE ProductCode=... AND StockQty>0 ORDER BY ExpDate ASC
  |     |             -> (C# computes restQty/bonusQty split across batches, FEFO)
  |     |             -> InvoiceBLL.InvoiceDetailInsertBLL(...)                    [InvoiceBLL.cs:322-392]
  |     |                  PK = ClsPrimaryKeyFind.PrimaryKeyMax("InvoiceDetailId","tblInvoiceDetail")  -- MAX(id)+1
  |     |                  -> InvoiceDAL.SaveDataForInvoiceDetails(detail)          [InvoiceDAL.cs:379-429]
  |     |                       => tblInvoiceDetail (raw-concatenated INSERT)
  |     |             -> InvoiceDAL.UpdateDCStoreQuantity(dCStoreId, newQty)        [InvoiceDAL.cs:799-803]
  |     |                  UPDATE tblDCStore SET StockQty='<newQty>' WHERE DCStoreId='<id>'   -- absolute overwrite, NOT a decrement
  |     |     return true;   <-- UNCONDITIONAL, line 1642 -- see INV-001
  |     |
  |     |-- OrderInfoBLL.UpdateInvoiceStatus(orderId)
  |     |     -> OrderInfoDAL.UpdateInvoiceStatus         [OrderInfoDAL.cs:269-273]
  |     |          UPDATE tblOrder SET IsInvoice='True' WHERE OrderId=@id   -- unconditional flag flip
  |     |
  |     |-- scope.Complete()   -- reached even if 0 detail rows were inserted
  |== end TransactionScope =============================================================
  |
  |-- InvoiceBLL.GetInvoiceNoById(invId) -> invTextBox.Text, rptInvoiceId.Text = invId
  |-- showMessageBox("Proforma Invoice Created Successsfully!!")

printButton_Click                                             [aspx.cs:1812-1826]
  -> window.open("../SInventory_RPTVIEW/InvoiceReportViewer.aspx
                    ?InvNo=" + invTextBox.Text + "&invoiceId=" + rptInvoiceId.Text)
       |
       v
InvoiceReportViewer.aspx.cs :: Page_Init                       [InvoiceReportViewer.aspx.cs:20-80]
  invoiceId = QueryString["invoiceId"]; if empty -> "0"
  invColl   = InvoiceBLL.InvoiceNoCollectionFormate(InvNo)      -- 'X','Y' quoted list
  mainDataTable   = InvoiceBLL.InvoiceMainDataForReportBLL(invColl, invoiceId)
                       -> InvoiceDAL.InvoiceMainDataForReport    [InvoiceDAL.cs:1240-1262]
                          SELECT ... FROM tblInvoice IV ... WHERE IV.InvoiceId = <invoiceId>
                                                             AND IV.InvoiceNo IN (<invColl>)
  detailDataTable = InvoiceBLL.InvoiceDetailDataForReportBLL(invColl, invoiceId)
                       -> InvoiceDAL.InvoiceDetailDataForReport  [InvoiceDAL.cs:1281-1297]
                          SELECT ... FROM tblInvoice IV LEFT JOIN tblInvoiceDetail IVD ON IVD.InvoiceId=IV.InvoiceId
                                    ... WHERE IV.InvoiceId = <invoiceId> AND IV.InvoiceNo IN (<invColl>)
  rptdoc.Load("Reports/CrystalReports/rptInvoiceForCustomer.rpt")
  rptdoc.SetDataSource(DataSet{mainDataTable, detailDataTable, companyInfoDataTable})
  crvInvoiceReport.ReportSource = rptdoc
```

Stray legacy code path (present in the file, **not wired to any button in the current markup** —
confirmed by grepping `InvoiceCreationForCustomerByOrder.aspx` for `OnClick`, which shows only
`saveButton_Click1` and `printButton_Click`/`yesButton_Click`/`noButton_Click`):

```
saveButton_Click (aspx.cs:1500-1553)  -- DEAD CODE, no <asp:Button> references it
  -> SaveInvoice() -> InvoiceBLL.SaveInvoice(out invoiceId, out invoiceNo)
       -> invoiceId = ClsPrimaryKeyFind.PrimaryKeyMax("invoiceId","tblInvoice")   -- MAX(id)+1, no transaction
       -> InvoiceDAL.SaveDataForInvoice -> raw string-concatenated INSERT INTO tblInvoice
  -> SaveInvoiceDetail(invId)
  -> InvoiceBLL.PostJournal(invId) -> sp_ProformaPosting
  -> OrderInfoBLL.UpdateInvoiceStatus
  -- on any step failing: InvoiceBLL.DeleteInvoice(orderId) -> sp_Deletenvoice
     (deletes ONLY the invoice header/detail rows already inserted by that SP;
      does NOT reverse tblDCStore stock writes or tblOrderDetail "Undelivered" status
      already committed by the preceding, non-transactional SaveInvoiceDetail call)
```

---

## 5. Current Architecture

- ASP.NET Web Forms, .NET Framework 4.x, postback model, `UpdatePanel`-based partial rendering.
- No DTO/ViewModel layer; code-behind reads control values directly into `Library.DAO` entities
  (`Invoice`, `InvoiceDetail`).
- Data access goes through Microsoft Enterprise Library's `Database`/`DbCommand` abstraction via
  `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`; every call opens/closes its own connection —
  there is no shared `SqlConnection`/`SqlTransaction` object passed between calls anywhere in the
  save chain.
- **The one exception**: `saveButton_Click1` wraps the whole save sequence in a
  `System.Transactions.TransactionScope`. Atomicity across the several separate connections opened
  during that scope (one per `SqlDatabase.CreateConnection()` call inside `ClsCommonInternalDAL`)
  relies on ADO.NET's automatic ambient-transaction enlistment (each new `SqlConnection.Open()`
  auto-enlists in `Transaction.Current`), not on an explicit connection/transaction object passed
  down the call chain. This is functionally correct for SQL Server (LTM/DTC promotion handles
  multiple enlistments against the same connection string transparently) but is architecturally
  fragile: nothing in the DAL is transaction-aware by design, so any future refactor that opens a
  connection with `Enlist=false` in its connection string, or that calls out to a different
  database/catalog inside the scope (the codebase references three separate catalogs —
  `SalesrollDB_ZAS`, `ZAS_ACCDB`, `CSTL_ERPDB` — see `specs/.../PAGE-SAL-011...md` §17), would
  silently break the "all or nothing" guarantee without any compiler or runtime warning.
- Stored procedure bodies are **not** part of this repository as compilable/deployable SQL objects.
  A `docs/database/` folder contains prior-session reverse-engineered transcripts of some procedures
  (see §3); these are cited as DOCUMENTED-UNVERIFIED, not as ground truth.

---

## 6. Invoice Master Audit

- **SP used**: `sp_I_InvoiceMaster`, invoked via
  `ClsCommonInternalDAL.SaveAction("sp_I_InvoiceMaster", aSqlParameterlist, "@InvoiceId")`
  (`InvoiceDAL.cs:1767` — CONFIRMED call site).
- **Parameters** (CONFIRMED from `InvoiceDAL.SaveInvoiceMater`, `InvoiceDAL.cs:1742-1768`):
  `@InvoiceDate, @OrderNo, @OrderDate, @CustomerMasterId, @ComUnitId, @MiaId, @PaymentTypeId,
  @TpTotal, @TpDiscount, @TpVat, @TpGrandTotal, @UserId, @OrderId, @TotalSpecialAmount,
  @OldTradePolicy, @ProductOffer, @Remarks, @DeliveryManId, @UnitCode, @PONumber`, plus the OUT
  parameter `@InvoiceId`.
- **ID generation/retrieval**: an ADO.NET **output parameter**
  (`db.AddOutParameter(dbCommand, "@InvoiceId", DbType.Int32, 10)` inside
  `ClsCommonInternalDAL.ExecuteNonQueryAction`), read back after `db.ExecuteNonQuery(dbCommand)`
  returns a positive rows-affected count — **not** `SCOPE_IDENTITY()`/`@@IDENTITY` in the C# layer.
  Whatever the procedure itself does internally to generate `@InvoiceId` (identity column,
  sequence, or a manual MAX+1) is **UNCONFIRMED** from C# alone.
  - **DOCUMENTED-UNVERIFIED** (`docs/database/stored-procedures-part2.md:5218-5226` and body
    following): the transcript describes the live SP as computing the next `InvoiceNo` string via
    `MAX(CAST(SUBSTRING(InvoiceNo,...) AS INT))+1` scoped to `ComUnitId`/date, guarding against
    "the same OrderNo processed within the last 60 seconds", and returning `@InvoiceId = 0` on a
    detected duplicate or guard trip — with **no `BEGIN TRAN`/`TRY-CATCH`** inside the procedure
    itself. If accurate, this means the "atomicity" of the master insert is provided **entirely by
    the outer `TransactionScope`** the C# code added, not by the stored procedure, and the 60-second
    duplicate window is a heuristic, not a hard constraint (see §10, §13).
- **Failure signalling**: `ClsCommonInternalDAL.SaveAction` **swallows all exceptions** and returns
  `pk = 0` (CONFIRMED, `ClsCommonInternalDAL.cs`, `SaveAction`/`ExecuteNonQueryAction`, ~lines
  798-863: `try { pk = ExecuteNonQueryAction(...); } catch (Exception ex) { pk = 0; } return pk;`).
  A genuine SQL error (deadlock, timeout, constraint violation) and a legitimate "rejected by the
  60-second duplicate guard" are **indistinguishable** to the caller — both surface as `invId <= 0`
  and the same generic message
  `"Invoice could not be created. This order may have just been invoiced..."`
  (`InvoiceCreationForCustomerByOrder.aspx.cs:1427`).
- **Is the returned ID correctly used for details?** **Yes, CONFIRMED.** `invId` from
  `SaveInvoiceMater` is passed straight into `SaveInvoiceDetail(invId)`
  (`InvoiceCreationForCustomerByOrder.aspx.cs:1431`), which sets `aInvoiceDetail.InvoiceId =
  invoiceId` on every constructed detail row (`aspx.cs:1592`) before persisting. No ID confusion
  found on this specific hop.

---

## 7. Invoice Detail Audit

- **Table**: `tblInvoiceDetail`, via `InvoiceDAL.SaveDataForInvoiceDetails(InvoiceDetail)`
  (`InvoiceDAL.cs:379-429`), a **raw string-concatenated INSERT** (not parameterized) called from
  `InvoiceBLL.InvoiceDetailInsertBLL` (`InvoiceBLL.cs:322-392`).
- **Master ID passed correctly**: **Yes, CONFIRMED** — see §6.
- **Field completeness**: `InvoiceDetail` (`Library.DAO/SInventory_Entities/InvoiceDetail.cs`)
  carries `ProductCode, ProductName, PackSize, BatchNo, ReceiveDate, ExpDate, CostPrice, UnitPrice,
  UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice, TotalPriceVatAmount,
  DiscountPercentage, DiscountAmount, NetAmount, InvoiceId, DCStoreId, OrderDetailsId, SpecialAmount`
  — product, qty, rate, discount, and VAT/tax are all present (CONFIRMED, populated in
  `InvoiceCreationForCustomerByOrder.aspx.cs:1570-1594`). **Warehouse/UOM**: `DCStoreId` ties the
  detail row to a specific `tblDCStore` batch (which implicitly encodes warehouse/DC), assigned
  inside `InvoiceBLL.InvoiceDetailInsertBLL` from the FEFO batch loop — CONFIRMED present. There is
  **no separate UOM (unit-of-measure) field** on `InvoiceDetail` at all; the page/entity model
  assumes a single fixed unit per product (see §21 note on multi-UOM in the Findings Matrix).
- **VAT conditional zeroing**: `UnitVatAmount` and `TotalPriceVatAmount` are forced to `0` when
  `isVatApplicable.Value == "False"` (`aspx.cs:1579, 1594`) — a client-hidden-field-driven business
  rule, not re-validated server-side.
- **THE CRITICAL DEFECT — `SaveInvoiceDetail` always reports success**
  (`InvoiceCreationForCustomerByOrder.aspx.cs:1555-1643`, **CONFIRMED** by direct read):
  ```csharp
  private bool SaveInvoiceDetail(int invoiceId)
  {
      List<InvoiceDetail> aInvoiceDetailsList = new List<InvoiceDetail>();
      if (gridLineItemGridView.Rows.Count > 0)
      {
          for (int i = 0; i < gridLineItemGridView.Rows.Count; i++)
          {
              ...
              decimal cstock = Convert.ToDecimal(((TextBox)gridLineItemGridView.Rows[i]
                                    .Cells[3].FindControl("currentStockTextBox")).Text);
              ...
              if (cstock == 0)
                  aInvoiceBll.UpdateOrder("Undelivered", aInvoiceDetail.OrderDetailsId.ToString());
              else
                  aInvoiceDetailsList.Add(aInvoiceDetail);
          }
          aInvoiceBll.SaveInvoiceDetails(aInvoiceDetailsList, hdComUnitId.Value);
      }
      return true;   // <-- line 1642: unconditional, ignores list content and the call's own result
  }
  ```
  `InvoiceBLL.SaveInvoiceDetails` (`InvoiceBLL.cs:194-202`) itself also returns nothing actionable —
  it `foreach`es the list calling `UpdateDCStock` per row and its own declared return value is never
  even inspected by the caller above. **If every grid row's displayed stock is `0` at the moment of
  Save (e.g. it changed since the order/grid was loaded, or the row's stock textbox was never
  refreshed), `aInvoiceDetailsList` ends up empty, zero rows are ever handed to
  `SaveInvoiceDetails`, and `SaveInvoiceDetail` still returns `true`.** The caller's gate at
  `InvoiceCreationForCustomerByOrder.aspx.cs:1431` (`if (!SaveInvoiceDetail(invId)) {...return;}`)
  can therefore never observe this failure mode, and `scope.Complete()` commits a master-only
  invoice. This is finding **INV-001** (§15/§16) and is the primary, code-confirmed explanation for
  “Master saves but Details sometimes don’t” (§ “Most Likely Root Cause”, item A).

---

## 8. Transaction Audit

- **Is Master+Details+Stock+Order-status wrapped in one transaction?** **Yes, for the live save
  path** (`saveButton_Click1`) — CONFIRMED, `InvoiceCreationForCustomerByOrder.aspx.cs:1408-1448`,
  `TransactionScope(TransactionScopeOption.Required, options)` with `IsolationLevel.ReadCommitted`
  and a 5-minute timeout, wrapping `SaveInvoiceMater`, `SaveInvoiceDetail`, and
  `OrderInfoBLL.UpdateInvoiceStatus`. `scope.Complete()` is only called after all three succeed
  (or, in the detail case, "succeed" per the broken always-`true` contract in §7); any exception, or
  an explicit early `return` before `Complete()`, rolls the whole scope back on `Dispose()`.
- **Can Invoice Master succeed while Details fail?** **Yes — CONFIRMED, and this is the headline
  defect.** Not because the transaction fails to roll back on a thrown exception (it correctly
  would), but because the detail-save step is designed to never signal failure at all (§7). A
  master-only commit is not a transaction bug; it is a **logic bug that prevents the transaction
  from ever being asked to roll back** for this specific failure mode.
- **Can partial detail rows persist on failure?** For the live path: **No**, in the classic sense —
  if `InvoiceDAL.SaveDataForInvoiceDetails`/`UpdateDCStoreQuantity` genuinely throw (e.g. a SQL
  constraint violation), the exception propagates out of `SaveInvoiceDetail` (nothing in that method
  catches it), is caught by the outer `catch (Exception ex)` at `aspx.cs:1443-1448`, and the
  `TransactionScope` rolls back everything, including the master row and any detail rows already
  inserted earlier in the same loop — CONFIRMED correct behavior for a genuine thrown exception.
  **However**, per the finding in §7, the far more likely failure mode (stock reads as 0 for a line)
  produces **no exception at all**, so this correct rollback path is simply never entered.
- **Legacy `saveButton_Click` path** (dead code, not reachable from the UI, but present in the
  compiled assembly): **No transaction at all.** `SaveInvoice()`, `SaveInvoiceDetail()`,
  `PostJournal()`, `UpdateInvoiceStatus()` are four independent, non-atomic calls; its failure
  handling calls `InvoiceBLL.DeleteInvoice` as a **compensating action**, which only removes rows
  from `tblInvoice`/`tblInvoiceDetail` via `sp_Deletenvoice` — it does **not** reverse
  `tblDCStore` stock writes or `tblOrderDetail` status flips already committed by the preceding
  `SaveInvoiceDetail` call in that same (non-transactional) sequence. This is a real orphaned-state
  bug, moot today only because the button is not wired to it (confirmed by grepping the `.aspx` for
  `OnClick=`).
- **Repo-wide scope**: a grep across the entire solution (excluding `bin`/`obj`) for
  `SqlTransaction|TransactionScope|BEGIN TRAN|COMMIT TRAN|XACT_ABORT` returns matches **only** at
  `InvoiceCreationForCustomerByOrder.aspx.cs:1418` and `:1445`. Every other save flow in the
  application — including this page's own `noButton_Click` "cancel invoice" path
  (`ProformaOrInvoiceReturnBLL.ProformaReturnPosting` then `.DeleteProforma`, two independent calls,
  `aspx.cs:1890-1906`) — is a chain of autocommit statements with no atomicity guarantee at all.

---

## 9. Stock Audit

- **Where stock is read**: `InvoiceDAL.BatchWiseProductQty(productCode, comUnitId)`
  (`InvoiceDAL.cs:938-942`) — `SELECT * FROM tblDCStore WHERE ProductCode='...' AND
  ComUnitId='...' AND StockQty>0 ORDER BY ExpDate ASC` (FEFO ordering). Plain `SELECT`, no locking
  hint (`WITH (UPDLOCK, HOLDLOCK)` or similar) — CONFIRMED.
- **Where stock is deducted**: `InvoiceBLL.UpdateDCStock` (`InvoiceBLL.cs:218-321`) reads the batch
  list above, computes a new remaining quantity **in C# memory** for each batch consumed, then calls
  `InvoiceDAL.UpdateDCStoreQuantity(dCStoreId, newQty)` (`InvoiceDAL.cs:799-803`):
  ```sql
  UPDATE tblDCStore SET StockQty='<newQty>' WHERE DCStoreId='<id>'
  ```
  This is an **absolute overwrite of a value computed client-side from an earlier read**, not an
  atomic decrement (`SET StockQty = StockQty - @qty`), and there is **no `WHERE StockQty >= @qty`**
  guard and no optimistic-concurrency predicate (e.g. `WHERE StockQty = @originalQty`) — CONFIRMED
  by direct read of both methods. This is a textbook **read-then-write (TOCTOU) race**: two
  concurrent invoice saves against the same `DCStoreId` can both read the same starting quantity
  before either writes back, and the second `UPDATE` silently overwrites (loses) the first
  deduction — allowing the DB-recorded stock to end up higher than physically correct, and
  permitting oversell.
- **Is it single-deduction or double-deduction risk?** **Single mechanism** — stock is only ever
  written via `tblDCStore` in this flow; there is no separate stock-ledger/transaction table also
  being written for the same event in the code paths read. Double-deduction in the sense of "two
  different code paths both decrementing the same row for one invoice" was **not found**. The risk
  here is the opposite: **lost deductions** (under-deduction) under concurrency, and **silent
  under-fulfillment** (see next point) — not double-charging stock.
- **Insufficient total stock**: `UpdateDCStock`'s batch loop (`InvoiceBLL.cs:218-321`) walks
  `BatchWiseProductQty`'s result set and simply **stops when it runs out of batch rows** if the sum
  of all available batches is less than the requested quantity. There is **no `SUM(available) >=
  requested` pre-check anywhere** in the C# or in any query read. The shortfall is dropped **without
  any exception, log entry, or return value indicating partial fulfillment** — CONFIRMED. Combined
  with `SaveInvoiceDetail`'s unconditional `return true` (§7), a request for more stock than exists
  across all batches produces a **silently short-shipped, "successful" invoice**.
- **Deliberate isolation-level tradeoff**: the code comment at
  `InvoiceCreationForCustomerByOrder.aspx.cs:1411-1414` explicitly states `ReadCommitted` (not the
  `TransactionScope` default of `Serializable`) was chosen "to avoid range locks on `tblDCStore`" —
  i.e. the race condition above is a **known, accepted tradeoff for throughput**, not an oversight
  of isolation-level defaults. It does not, however, mitigate the underlying non-atomic
  read-then-write pattern; a proper fix (atomic guarded `UPDATE`) would remove the need for the
  tradeoff entirely (see §19).

---

## 10. Order-to-Invoice Audit

- **Remaining-order-qty enforcement**: **Not enforced at all beyond a UI color hint.**
  `InvoiceCreationForCustomerByOrder.aspx.cs:293-297` and `:330-338` set
  `gridLineItemGridView.Rows[i].BackColor = Color.Red` when `cstock < Quantity` during page load —
  purely cosmetic; it does not block the Save postback, and a user can still submit that row.
  `InvoiceBLL.LoadProductQty`/`InvoiceDAL.LoadProductQty` (`InvoiceDAL.cs:736-742`,
  `SELECT SUM(Quantity) FROM tblOrder JOIN tblOrderDetail WHERE OrderId=... AND
  ProductCode=...`) does return the originally ordered quantity for a product, but the call site at
  save time (`aspx.cs:1616-1618`) **assigns the result to `dtdata` and never reads it** — the
  consuming line is commented out: `//tqty = Convert.ToDecimal(dtdata.Rows[0][0].ToString());`
  (CONFIRMED dead code). The only server-side branch that changes behavior at save time is
  `if (cstock == 0)` (`aspx.cs:1629`), which only detects **zero** stock, not "requested qty exceeds
  remaining order qty" or even "requested qty exceeds current stock" for a non-zero shortfall.
- **Can the same order be over-invoiced (quantity)?** **Yes** — nothing compares invoice quantity to
  remaining order quantity across possibly multiple partial invoices; there is no `InvoicedQty` /
  `DeliveredQty` column tracked on `tblOrderDetail` anywhere in the BLL/DAL read for this page —
  the only per-line status write is `InvoiceDAL.UpdateOrder`
  (`UPDATE tblOrderDetail SET Status='<status>' WHERE OrderDetailId='<id>'`,
  `InvoiceDAL.cs:1057-1061`), and the only value it is ever set to from this page is `"Undelivered"`.
- **Can the same order be double-invoiced (whole order)?** **The guard exists but is a
  check-then-act race, not a hard constraint.** `OrderExists(orderIdHiddenField.Value)`
  (`aspx.cs:166-179` → `OrderInfoBLL.LoadOrderExistsBll` → `OrderInfoDAL.LoadOrderExistsDal`,
  `SELECT * FROM dbo.tblInvoice WHERE tblInvoice.OrderId='<id>'`) correctly targets the right
  table/column, and it is invoked twice — once at `aspx.cs:1400` (guarding `saveButton_Click1`) and
  once inside `LoadAllDataByOrder` when the order is first opened. But the check at line 1400 runs
  **before** the `TransactionScope` opens at line 1418 — it is not itself protected by the
  transaction. Two concurrent requests for the same order can both execute this `SELECT`, both see
  zero existing invoice rows, and both proceed to their own `TransactionScope` and insert. No unique
  index/constraint on `tblInvoice.OrderId` is visible from the C# layer (schema DDL is not in the
  repo — **UNCONFIRMED** at the DB level). The only mitigating factor found is
  **DOCUMENTED-UNVERIFIED**: the `sp_I_InvoiceMaster` transcript in
  `docs/database/stored-procedures-part2.md` describes a heuristic "reject if the same OrderNo was
  processed in the last 60 seconds" check with **no transaction/locking of its own** — a
  time-boxed mitigation, not a hard uniqueness guarantee, and unverifiable against the live database
  from this repository.
- **`UpdateInvoiceStatus`** (`OrderInfoDAL.cs:269-273`): `UPDATE dbo.tblOrder SET IsInvoice='True'
  WHERE OrderId='<id>'` — an unconditional flag flip with no `WHERE IsInvoice = 'False'` guard and
  no row-version check, so even if two concurrent transactions both reach this statement they will
  both "succeed" (rows-affected = 1 each, since the `WHERE` only filters on `OrderId`) — it provides
  no additional protection against the double-invoice race above.

---

## 11. Blank Print Audit

**Chain**: `printButton_Click` (`aspx.cs:1812-1826`) → `InvoiceReportViewer.aspx.cs Page_Init`
(lines 20-80) → `InvoiceBLL.InvoiceMainDataForReportBLL`/`InvoiceDetailDataForReportBLL`
(`InvoiceBLL.cs:808-816`) → `InvoiceDAL.InvoiceMainDataForReport`/`InvoiceDetailDataForReport`
(`InvoiceDAL.cs:1240-1262`, `1281-1297`) → `rptInvoiceForCustomer.rpt` via
`CrystalReportViewer`.

| # | Candidate cause | Verdict | Evidence |
|---|---|---|---|
| 1 | Wrong ID param (`OrderId` passed where `InvoiceId` expected) | **NOT FOUND** | `rptInvoiceId.Text = invId.ToString()` is set from the real `tblInvoice.InvoiceId` returned by `SaveInvoiceMater` (`aspx.cs:1458`), and `InvoiceReportViewer.aspx.cs:26` reads the same semantic value under the same query-string key `invoiceId`. Both the main and detail SQL filter on `IV.InvoiceId = <invoiceId>` (`InvoiceDAL.cs:1260, 1294`). |
| 2 | Query-string key-name mismatch between pages | **NOT FOUND** | `InvNo`/`invoiceId` names match exactly on both sides (`aspx.cs:1821` vs `InvoiceReportViewer.aspx.cs:24,26`). |
| 3 | Wrong join / status / date filter excluding a fresh invoice | **NOT FOUND** | No status or date predicate exists in either report query beyond `InvoiceId`/`InvoiceNo` matching; both queries use `LEFT JOIN` from `tblInvoice`, so a freshly committed master row is always returned as one row from the main query. |
| 4 | **Details never committed (master exists, zero detail rows)** | **CONFIRMED** | Direct consequence of §7/§16 INV-001: `SaveInvoiceDetail` can commit a master row with an empty `aInvoiceDetailsList`, and still return `true`. `InvoiceDetailDataForReport`'s `LEFT JOIN tblInvoiceDetail AS IVD ON IVD.InvoiceId = IV.InvoiceId` (`InvoiceDAL.cs:1291-1292`) then returns exactly one row with every `IVD.*`-sourced column `NULL` (`ProductCode`, `Quantity`, `TotalPrice`, etc. all null because the `LEFT JOIN` preserves the master row even with no matching detail). Crystal Report renders the header/company/customer block from `mainDataTable` (non-empty) but the line-item section has nothing to iterate — this is the "blank invoice print" a user would actually observe after a normal-looking successful save. |
| 5 | Print clicked before any Save in the session | **CONFIRMED as a related, secondary failure mode** | `printButton_Click` has **no guard** checking that `invTextBox.Text`/`rptInvoiceId.Text` are non-empty (`aspx.cs:1812-1826`, entire body read). If clicked cold, `rptInvoiceId.Text=""` → `invoiceId` defaults to `"0"` in `Page_Init` (line 30-33) and `invNo=""` → `InvoiceNoCollectionFormate("")` returns `""` (`InvoiceBLL.cs:669-691`, the `if (text != "")` guard is false) → the SQL becomes `... AND IV.InvoiceNo in ()`, which is a SQL Server syntax error. `Page_Init`'s `catch (Exception ex) { throw ex; }` (lines 74-77) rethrows it, producing an ASP.NET error page rather than a silently blank one — a **louder**, differently-shaped failure than the "quiet blank print" symptom, but still a reachable defect from the same missing guard. |
| 6 | **Systemic**: other pages call the same shared report page passing only `InvNo`, no `invoiceId` | **CONFIRMED, out-of-page but same shared report page** | Repo-wide grep of `InvoiceReportViewer.aspx?InvNo=` shows `BatchWiseTopSheet.aspx.cs:106`, `BatchWiseDelReturnSummery.aspx.cs:93`, `BatchWiseDeliveryTopSheet.aspx.cs:105`, `InvoiceCreationForConsumerCustomer.aspx.cs:726`, `InvoiceCreationForCustomer.aspx.cs:549`, `ReturnInvoiceCreationForCustomer.aspx.cs:905`, `ReturnOnDeliveryReport.aspx.cs:90`, `TopSheetGenerate.aspx.cs:109` build the URL with **only** `InvNo`, never `invoiceId`. Every one of those requests hits `InvoiceReportViewer.aspx` with `invoiceId` defaulted to `"0"` (line 30-33) — and because both report queries `AND` on `IV.InvoiceId = 0`, **the main data table is guaranteed to be empty** (there is no invoice with ID 0), so the report renders fully blank (not even the header populates) every single time it is opened from any of those eight other pages. This is a page shared with 43+ other pages per `specs/.../PAGE-SAL-011...md` §31 — the "blank print" complaint pattern may well be dominated by these other, non-target-page entry points rather than by `InvoiceCreationForCustomerByOrder.aspx` itself, whose own call site (line 1821) correctly supplies both parameters. |
| 7 | Wrong report file loaded | **NOT FOUND** | `ReportPath("rptInvoiceForCustomer.rpt")` is a fixed literal (`InvoiceReportViewer.aspx.cs:65,98`); no dynamic report-name selection exists in this chain. |

---

## 12. Error Handling Audit

- **Page level (this page)**: `saveButton_Click1` has a real `catch (Exception ex)` around the
  transaction (`aspx.cs:1443-1448`) that surfaces `ex.Message` via `showMessageBox` — this is
  honest, CONFIRMED. But because `ClsCommonInternalDAL.SaveAction`/`UpdateAction` (the methods
  actually used for `sp_I_InvoiceMaster` and several update calls) **swallow** exceptions internally
  (CONFIRMED, `ClsCommonInternalDAL.cs`, `SaveAction`/`UpdateAction`/`ExecuteNonQueryAction`:
  `catch (Exception ex) { pk = 0; }` / `{ ActionStatus = false; }`, no rethrow, no logging call
  anywhere in the catch blocks), most genuine SQL failures during master-save never reach this outer
  `catch` as exceptions at all — they present only as `invId <= 0`, indistinguishable from a
  legitimate business rejection (e.g. the documented 60-second duplicate guard).
- **DAL level**: two distinct idioms coexist across `ClsCommonInternalDAL.cs` and
  `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` (98 total `catch (Exception` sites sampled across
  `Library.DAL`/`Library.BLL`/`Library.DAO`):
  1. **Rethrow with reset stack trace** (`catch (Exception ex) { throw ex; }`) — the majority
     pattern in read paths (`DataContainerDataTable`, `Get*Action`) and BLL wrapper methods. The
     exception does propagate to the caller, but `throw ex;` (instead of bare `throw;`) discards the
     original stack trace, making the true failure site harder to diagnose from logs.
  2. **Silent swallow-to-zero/false** — used in **every** `SaveAction`/`UpdateAction`/`DeleteAction`
     entry point in both `ClsCommonInternalDAL` and `DB_Manager`, i.e. exactly the write/mutate
     paths used for Invoice, Order, and Stock records. No logging call of any kind appears in any of
     these catch blocks.
- **Application level**: `Global.asax` `Application_Error` is empty and `<customErrors>` is not
  configured (per `specs/.../PAGE-SAL-011...md` §22, cross-checked and consistent with the
  swallow/rethrow pattern found directly above) — there is no global error log, so a swallowed
  exception inside `SaveAction` leaves **zero trace anywhere** in the application; it is
  unrecoverable forensically after the fact from logs (there are none) — only from re-reading the
  code, as this audit had to do.
- **Misleading success messages**: `showMessageBox("Proforma Invoice Created Successsfully!!")`
  (`aspx.cs:1461`) fires unconditionally after the transaction commits, with **no check that any
  detail rows were actually persisted** — directly caused by the INV-001 defect in §7. This is the
  literal "misleading success message after partial failure" the audit brief asks about, and it is
  code-confirmed, not speculative.

---

## 13. Concurrency Audit

| Concern | Status | Evidence |
|---|---|---|
| Stock deduction race (`tblDCStore`) | **CONFIRMED vulnerable** | §9 — read-then-blind-overwrite `UPDATE ... SET StockQty='<value>'`, no guard clause, `ReadCommitted` isolation deliberately chosen over `Serializable`. |
| Double-invoice race (`tblInvoice.OrderId`) | **CONFIRMED vulnerable** | §10 — `OrderExists` check runs before the `TransactionScope` opens; no unique constraint visible from C#; only a DOCUMENTED-UNVERIFIED 60-second SP-side heuristic. |
| Order-status race (`tblOrder.IsInvoice`) | **CONFIRMED vulnerable** | §10 — unconditional `UPDATE ... SET IsInvoice='True' WHERE OrderId=@id`, no optimistic-lock predicate; both concurrent writers "succeed". |
| Detail-row primary key generation | **CONFIRMED vulnerable, in the live path** | `InvoiceBLL.InvoiceDetailInsertBLL` (`InvoiceBLL.cs:322-392`) generates `InvoiceDetailId` via `ClsPrimaryKeyFind.PrimaryKeyMax("InvoiceDetailId","tblInvoiceDetail")` (`ClsPrimaryKeyFind.cs`), which runs `SELECT (ISNULL(MAX(InvoiceDetailId),0)+1) FROM tblInvoiceDetail` with **no locking hint** through `DataContainerDataTable` (itself non-transactional in the sense of not taking any lock). Two concurrent detail inserts can compute the same "next" ID before either commits, risking a duplicate-key failure (if a PK constraint exists at the DB — UNCONFIRMED) or silent data corruption (if it does not). This is **inside** the live, transactional save path (`UpdateDCStock` → `InvoiceDetailInsertBLL`), not just the dead legacy code. |
| Master invoice ID generation | **UNCONFIRMED at the DB level; DOCUMENTED-UNVERIFIED risk noted** | Retrieved via SP output parameter (§6); whether the SP itself uses an atomic `IDENTITY`/`SCOPE_IDENTITY()` mechanism or a manual `MAX+1` pattern under a lock cannot be confirmed from the C# repo. If the DOCUMENTED-UNVERIFIED transcript's description (MAX(SUBSTRING(InvoiceNo,...))+1, no `BEGIN TRAN`) is accurate, this is **also** a race-prone generator. |
| Double-submission (same user, same click) | **Mitigated client-side, CONFIRMED** | `saveButton` has `OnClientClick="if (this.disabled) return false; this.disabled = true; this.value='Processing...';"` and `UseSubmitBehavior="False"` (`InvoiceCreationForCustomerByOrder.aspx:503-505`), plus a `PageRequestManager.add_endRequest` handler that re-enables the button only on an async-postback error (`aspx:640-650`, with an explanatory comment about re-enabling if the `UpdatePanel` request dies server-side). This is a real, reasonably designed guard against accidental double-click, though it depends on client-side JS executing before the second click lands (a narrow race window remains) and offers no protection across two different browser tabs/sessions acting on the same order. |
| Server-side idempotency | **NOT FOUND** | No idempotency token, no unique constraint enforced in the C# layer, no `sp_getapplock`-style serialization anywhere in the reviewed chain. The only server-side duplicate protection is the TOCTOU-vulnerable `OrderExists` check (§10) plus the DOCUMENTED-UNVERIFIED 60-second SP heuristic. |

---

## 14. Database Integrity Audit

- **No DDL/schema files exist in this repository** (confirmed absent by the pre-existing
  `specs/.../PAGE-SAL-011...md` §17/§30, cross-checked with a repo-wide search for `.sql`/`database/`
  content in this session — the only `.sql` artifact found, `docs/database/procedures/
  sp_LoadOrderForOrderCreation.sql`, is an explicitly-labeled hand-authored *replacement* proposal
  for a different page's SQL-injection issue, not an extracted schema). Primary keys, foreign keys,
  indexes, unique constraints, and triggers are therefore **UNCONFIRMED** for every table this page
  touches (`tblInvoice`, `tblInvoiceDetail`, `tblOrder`, `tblOrderDetail`, `tblDCStore`, etc.).
- **Inferred relationships from code** (CONFIRMED as *used*, not as *enforced* by the DB):
  `tblInvoiceDetail.InvoiceId → tblInvoice.InvoiceId`, `tblInvoiceDetail.OrderDetailsId →
  tblOrderDetail.OrderDetailId`, `tblInvoiceDetail.DCStoreId → tblDCStore.DCStoreId`,
  `tblInvoice.OrderId → tblOrder.OrderId`. Because every `LEFT JOIN` used in the report queries
  (§11) tolerates a missing child row without erroring, **the application code itself does not
  depend on any of these being enforced by a real FK constraint** — which is consistent with (and
  arguably enables) the "master with zero details" state described in §7/§16 persisting silently
  rather than being rejected by the database.
- **Raw string-concatenated SQL** (SQL-injection-shaped, not scoped as a security review but
  material to "integrity"): `InvoiceDAL.UpdateDCStoreQuantity` (`:799-803`), `InvoiceDAL.UpdateOrder`
  (`:1057-1061`), `InvoiceDAL.SaveDataForInvoice` (legacy path, `:249-292`),
  `InvoiceDAL.GetInvoiceNoById` (`:1770-1775`), and both report queries
  `InvoiceMainDataForReport`/`InvoiceDetailDataForReport` (`:1240-1297`, which interpolate the
  **user-supplied query-string value** `invoiceId` directly into SQL text with no parameterization
  or type validation — CONFIRMED). This is out of this audit's primary scope but is flagged because
  it means database-side integrity for these fields relies entirely on application-layer discipline,
  with no defense in depth.
- **`ExecuteAllSqlQueryByStoreProcedure`**: nearly every `SELECT` in this chain (all `DataTable`
  loaders via `ClsCommonInternalDAL.DataContainerDataTable`) is executed by wrapping the entire
  ad-hoc SQL string as a single `@Query` parameter to one generic stored procedure — meaning SQL
  Server's own query-plan/parameter-sniffing and any DB-side auditing see one undifferentiated
  "generic query executor" call for the vast majority of this page's reads, not the actual
  statement being run, which further reduces DB-side traceability of what happened during an
  incident.

---

## 15. Findings Matrix

| ID | Module | Finding | Root Cause | Evidence | Severity | Business Impact | Recommended Fix |
|---|---|---|---|---|---|---|---|
| INV-001 | Invoice Detail save | `SaveInvoiceDetail` always returns `true`, even when zero detail rows were built/persisted | Hard-coded `return true;` at end of method, independent of loop/list outcome | `InvoiceCreationForCustomerByOrder.aspx.cs:1555-1643` (return at 1642) | 🔴 Critical | Invoices can post with a header and no line items; downstream accounting/print/reporting all see a "valid" invoice with nothing on it | Make `SaveInvoiceDetail` (and `InvoiceBLL.SaveInvoiceDetails`) return the true outcome: `false`/throw when `aInvoiceDetailsList.Count == 0` after the loop, or when the underlying save call reports fewer rows written than expected; let the existing `TransactionScope` rollback do its job |
| STK-001 | Stock deduction | `UpdateDCStoreQuantity` is a blind absolute overwrite with no `WHERE StockQty >= @qty` guard | Read-then-write pattern split across `BatchWiseProductQty` (read) and `UpdateDCStoreQuantity` (write) with C#-computed value in between, no atomic SQL construct | `InvoiceDAL.cs:799-803`, `InvoiceBLL.cs:218-321` | 🔴 Critical | Concurrent invoice creation for the same product/batch can lose one deduction, resulting in stock levels that don't reflect actual sales — oversell risk | Rewrite as a single atomic guarded UPDATE: `UPDATE tblDCStore SET StockQty = StockQty - @qty WHERE DCStoreId=@id AND StockQty >= @qty`, check rows-affected, and treat 0-rows-affected as insufficient stock (raise, don't silently continue) |
| STK-002 | Stock shortfall | Insufficient total stock across all batches is silently short-shipped, no error/flag | `UpdateDCStock`'s batch loop simply exhausts its input rows with no `SUM(available) >= requested` pre-check | `InvoiceBLL.cs:218-321` | 🟠 High | Customer is invoiced/billed for quantity that was never actually allocated from stock, with no record of the shortfall | Pre-check total available quantity across batches before starting the allocation loop; fail the whole detail row (or the whole invoice) if insufficient, rather than partially allocating and moving on |
| ORD-001 | Order validation | Invoice quantity is never checked against remaining order quantity | The one value that would enable this (`LoadProductQty` result) is loaded and never read (dead/commented-out line) | `InvoiceCreationForCustomerByOrder.aspx.cs:1616-1618` | 🟠 High | Same order line can be invoiced for more than was ordered, with only a cosmetic red-highlight warning that doesn't block save | Compare requested qty to `(OrderedQty - SUM(already invoiced qty for this OrderDetailId))` server-side before adding the row to `aInvoiceDetailsList`; reject or cap and warn |
| ORD-002 | Double invoicing | Same-order double-invoice guard (`OrderExists`) is a check-then-act race, executed before the transaction scope opens | No DB-level unique constraint visible from C#; check and act are two separate round trips outside any lock | `InvoiceCreationForCustomerByOrder.aspx.cs:166-179, 1400, 1418` | 🟠 High | Two concurrent submissions (double-click across tabs, two users) for the same order can both succeed, producing two invoices for one order | Enforce a unique constraint on `tblInvoice.OrderId` at the database (if the business rule is truly "one invoice per order"), or use `sp_getapplock`/a guarded conditional insert inside the SP so the duplicate check and the insert are atomic |
| PRT-001 | Print | `printButton_Click` has no guard requiring a prior successful save; empty `invoiceId` defaults to `"0"` and the report queries a nonexistent `InvoiceId=0` | Missing input validation before building the print URL | `InvoiceCreationForCustomerByOrder.aspx.cs:1812-1826`; `InvoiceReportViewer.aspx.cs:24-33` | 🟡 Medium | Clicking Print before/without Save produces a SQL error page (malformed `IN ()`) rather than a helpful message | Guard `printButton_Click` with `if (string.IsNullOrEmpty(rptInvoiceId.Text)) { showMessageBox("Please save the invoice first."); return; }` |
| PRT-002 | Print (systemic, shared page) | 8 other pages open `InvoiceReportViewer.aspx` passing only `InvNo`, never `invoiceId`, guaranteeing `IV.InvoiceId = 0` and a fully blank report every time | Report page's SQL was extended to filter on `IV.InvoiceId` (presumably to fix a different ambiguity) without updating all existing callers | `BatchWiseTopSheet.aspx.cs:106`, `BatchWiseDelReturnSummery.aspx.cs:93`, `BatchWiseDeliveryTopSheet.aspx.cs:105`, `InvoiceCreationForConsumerCustomer.aspx.cs:726`, `InvoiceCreationForCustomer.aspx.cs:549`, `ReturnInvoiceCreationForCustomer.aspx.cs:905`, `ReturnOnDeliveryReport.aspx.cs:90`, `TopSheetGenerate.aspx.cs:109` | 🔴 Critical (for those 8 pages) / informational for this page | Every print from those 8 entry points is unconditionally blank — likely the dominant source of "blank print" reports across the application, even though it's not this page's own bug | Either make `invoiceId` optional again in the SQL (`AND (IV.InvoiceId = @invoiceId OR @invoiceId = 0)`), or update all 8 callers to also pass `invoiceId` the way `InvoiceCreationForCustomerByOrder.aspx.cs:1821` already correctly does |
| ERR-001 | Error handling | `SaveAction`/`UpdateAction`/`ExecuteNonQueryAction` swallow all SQL exceptions and return `0`/`false` with no logging | `catch (Exception ex) { pk = 0; }` / `{ ActionStatus = false; }`, no rethrow, no log call | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` (`SaveAction`/`UpdateAction`/`ExecuteNonQueryAction`, ~lines 655-863) | 🟠 High | Real DB failures (deadlocks, timeouts, constraint violations) during Invoice/Order/Stock writes are indistinguishable from legitimate business rejections, and leave zero trace anywhere (no logging framework wired up) | At minimum log the swallowed exception (message + stack) before returning the failure sentinel; ideally let it propagate and let the existing `TransactionScope`/outer `catch` in `saveButton_Click1` handle it, which already does the right thing when it gets the chance |
| TRX-001 | Transaction scope | Only one save path in the entire application uses any transaction primitive | Architectural — `TransactionScope` was added only to this one handler | Repo-wide grep, `SqlTransaction\|TransactionScope\|BEGIN TRAN\|COMMIT TRAN\|XACT_ABORT` → 2 matches total, both in `InvoiceCreationForCustomerByOrder.aspx.cs` | 🟡 Medium (architectural) | Every other multi-step save flow in the codebase (including this page's own Cancel/`noButton_Click` path) has the same class of partial-failure exposure this audit was asked to investigate here | Treat the pattern used here (`TransactionScope` + a genuinely-honest inner success/failure contract) as the template to roll out to other multi-step save flows, starting with the highest-value ones identified in a follow-up audit |
| KEY-001 | Primary key generation | `InvoiceDetailInsertBLL` generates `InvoiceDetailId` via unlocked `MAX(id)+1`, inside the live transactional path | `ClsPrimaryKeyFind.PrimaryKeyMax("InvoiceDetailId","tblInvoiceDetail")`, plain `SELECT`, no locking hint | `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs`; called from `InvoiceBLL.cs:322-392` | 🟠 High | Concurrent invoice detail inserts can compute colliding IDs; failure mode is either a constraint violation (if a PK/unique index exists at the DB — UNCONFIRMED) or silent corruption (if not) | Replace with a proper `IDENTITY` column + output parameter/`SCOPE_IDENTITY()`, matching the pattern already used correctly for `tblInvoice.InvoiceId` via `sp_I_InvoiceMaster`'s output parameter |
| DBI-001 | Database integrity | No FK/unique constraints confirmable from the repository for any table this page touches | No DDL in repo; report queries use tolerant `LEFT JOIN`s throughout | §14 | 🟡 Medium (assessment gap, not a proven absence) | Cannot verify whether the DB itself would ever reject the "master with zero details" or "double invoice" states described above, or whether it currently silently allows them | Obtain/commit actual schema DDL (or a `sys.foreign_keys`/`sys.indexes` extract) to this repository so future audits can confirm rather than infer constraint coverage |

---

## 16. Root Cause Analysis

### INV-001 — Invoice can commit with zero detail rows
- **Problem**: A "successfully" saved invoice can have a master row in `tblInvoice` and no rows at
  all in `tblInvoiceDetail`.
- **Evidence**: `InvoiceCreationForCustomerByOrder.aspx.cs:1555-1643`, specifically the unconditional
  `return true;` at line 1642, combined with the `if (cstock == 0)` exclusion at lines 1629-1636 that
  can empty `aInvoiceDetailsList` entirely.
- **Root Cause**: The method's contract (`bool SaveInvoiceDetail(...)`) implies it reports whether
  the detail save succeeded, but its implementation never actually inspects the outcome — it always
  returns `true`. The caller (`saveButton_Click1`) correctly wrote a check against that return value
  (`if (!SaveInvoiceDetail(invId)) {...}`), but the check is structurally dead because the callee
  cannot ever return `false`.
- **Impact**: A committed, user-visible "success", accounting-relevant invoice with no line items —
  financial totals on the master row (computed from the UI grid before save) will not reconcile with
  the (empty) detail rows; the print/report for that invoice is blank in its line-item section.
- **Severity**: 🔴 Critical.
- **Reproduction Scenario**: Load an order whose product's displayed stock is `0` for every line at
  the moment `saveButton` is clicked (e.g., stock was consumed by another transaction between page
  load and save, or the grid was populated from a stale order load). Click Save. The message
  "Proforma Invoice Created Successsfully!!" appears; `tblInvoice` gets one new row; `tblInvoiceDetail`
  gets none.
- **Recommended Fix**: Change `SaveInvoiceDetail` to return `false` (or throw) when, after the loop,
  `aInvoiceDetailsList.Count == 0` while the grid had at least one row that was not legitimately
  empty/blank-removed by the user, and/or when `InvoiceBLL.SaveInvoiceDetails` can report how many
  rows it actually persisted vs. how many were requested. Let the existing, correctly-built
  `TransactionScope` do the rollback it was designed for.

### STK-001 — Non-atomic stock deduction
- **Problem**: `tblDCStore.StockQty` is updated via a blind `SET StockQty = '<value>'` computed from
  an earlier, unlocked `SELECT`.
- **Evidence**: `InvoiceDAL.cs:799-803` (`UpdateDCStoreQuantity`), `InvoiceDAL.cs:938-942`
  (`BatchWiseProductQty`), `InvoiceBLL.cs:218-321` (`UpdateDCStock`).
- **Root Cause**: The deduction was implemented as "read the current quantity, subtract in C#, write
  the new absolute value back" instead of as a single atomic, guarded SQL statement.
- **Impact**: Under concurrent invoice creation touching the same batch/product, one deduction can be
  silently lost (lost update), leaving recorded stock higher than physical/committed reality —
  enabling a subsequent oversell that the system will not itself detect.
- **Severity**: 🔴 Critical.
- **Reproduction Scenario**: Two users (or two browser tabs) simultaneously create invoices that both
  draw from the same `DCStoreId` batch, with combined requested quantity ≤ that batch's stock but
  each individual request's read happening before either write. Both invoices "succeed"; final
  `StockQty` reflects only one of the two deductions.
- **Recommended Fix**: `UPDATE tblDCStore SET StockQty = StockQty - @qty WHERE DCStoreId = @id AND
  StockQty >= @qty`; check `@@ROWCOUNT`/rows-affected == 1; if 0, treat as insufficient stock for
  that batch and either move to the next batch or fail the line. This also removes the motivation
  for the `ReadCommitted`-over-`Serializable` tradeoff noted in the code comment, since the guard
  clause itself provides the necessary correctness without needing broader locking.

### PRT-002 — Systemically blank prints from other pages sharing this report
- **Problem**: Any invoice printed by starting from `BatchWiseTopSheet.aspx`,
  `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`,
  `InvoiceCreationForConsumerCustomer.aspx`, `InvoiceCreationForCustomer.aspx`,
  `ReturnInvoiceCreationForCustomer.aspx`, `ReturnOnDeliveryReport.aspx`, or `TopSheetGenerate.aspx`
  is guaranteed to print fully blank (header and lines both empty).
- **Evidence**: Each of those files' print URL construction passes only `InvNo`
  (grep results in §11 row 6); `InvoiceReportViewer.aspx.cs:30-33` defaults a missing `invoiceId` to
  `"0"`; `InvoiceDAL.cs:1260,1294` both `AND` on `IV.InvoiceId = <invoiceId>`, so `= 0` matches no
  row.
- **Root Cause**: `InvoiceMainDataForReport`/`InvoiceDetailDataForReport` were extended at some point
  to accept and filter on an `invoiceId` parameter (visible from the commented-out single-parameter
  overloads immediately above the current two-parameter versions in `InvoiceDAL.cs`, lines
  944-960/961-975, which show what the query looked like *before* `invoiceId` was added), but not
  all callers across the application were updated to pass it.
- **Impact**: Every print from those 8 pages is unconditionally broken today, independent of any
  save/transaction correctness elsewhere.
- **Severity**: 🔴 Critical (for those pages) — flagged here because it directly bears on diagnosing
  "why print can be blank" for the application as a whole, even though `InvoiceCreationForCustomerByOrder.aspx`'s own print call site is correct.
- **Reproduction Scenario**: From `BatchWiseTopSheet.aspx`, select any row and trigger its print
  action.
- **Recommended Fix**: See PRT-002 row in §15.

---

## 17. Risk Assessment

**Overall Risk: HIGH**

| Dimension | Rating | Justification |
|---|---|---|
| Data integrity | 🔴 High | INV-001 allows a committed, "successful" invoice header with no line items — a structurally invalid business record that nothing in the code or (as far as can be determined) the database currently prevents. |
| Financial accuracy | 🟠 Medium-High | Master-row totals (`TpTotal`, `TpGrandTotal`, etc.) are computed from UI state *before* save and are not derived from the detail rows actually persisted — when INV-001 triggers, the invoice's financial totals will not reconcile with its (empty) detail rows; accounting posting (`sp_ProformaPosting` via `PostJournal`) on the legacy path could then post financial totals with nothing behind them (though that specific call is on the dead code path for this page today). |
| Inventory accuracy | 🔴 High | STK-001 (lost-update race) and STK-002 (silent short-ship on insufficient stock) both allow recorded `tblDCStore.StockQty` to drift from physical/committed reality with no error surfaced. |
| Concurrency | 🟠 Medium-High | Multiple independently-exploitable races identified (§13): stock deduction, double-invoice guard, order-status flag, and detail-row PK generation. None are protected by DB-level locking/constraints that could be confirmed from this repository. |
| Recoverability | 🟠 Medium-High | `SaveAction`/`UpdateAction` swallow exceptions with no logging anywhere in the application (`Global.asax Application_Error` is empty, no logging framework configured) — when something does go wrong, there is no forensic trail beyond re-reading source code, as this audit had to do. |
| Print reliability | 🟠 Medium (for this page) / 🔴 High (application-wide via PRT-002) | This page's own print call path is correctly parameterized; the shared report page it opens is not correctly called by 8 other pages in the application, and this page's own print path silently fails to blank line-items whenever INV-001 has occurred upstream. |

The transactional design added specifically to this page's save handler is a genuine, above-average
mitigation compared to the rest of the codebase — the risk rating is HIGH rather than CRITICAL
specifically because that scaffolding exists and correctly protects against *thrown-exception*
failure modes. It is not CRITICAL-and-unmitigated; it is HIGH because the one failure mode most
likely to occur in normal operation (a stock-driven exclusion emptying the detail list) was left
outside that scaffolding's ability to detect.

---

## 18. Test Cases

**Invoice Save**

| Test ID | Scenario | Expected Result | Actual Implementation Behavior | Risk |
|---|---|---|---|---|
| TC-01 | One product, sufficient stock | Master + 1 detail row committed, stock decremented by qty | CONFIRMED as intended: happy path works as designed (`aInvoiceDetailsList` gets 1 row, `cstock != 0`) | Low |
| TC-02 | Multiple products, all with sufficient stock | Master + N detail rows committed | CONFIRMED as intended (loop adds every non-zero-stock row) | Low |
| TC-03 | Zero quantity on a line | Line should be rejected or excluded with a clear message | **No explicit qty==0 check found**; `qtyTextBox` flows into `InvoiceDetail.Quantity` without a floor validation in `SaveInvoiceDetail`; only `cstock==0` (stock, not requested qty) is checked | 🟠 Medium — a zero-qty line could be silently invoiced as a $0 line item |
| TC-04 | Negative quantity on a line | Should be rejected server-side | **No server-side non-negative check found** in `SaveInvoiceDetail`; client-side `FilteredTextBoxExtender`s only restrict to numeric+`.` characters (per `specs/.../PAGE-SAL-011...md` §18), not sign, and are client-only (not re-checked server-side per the same section) | 🟠 Medium — a crafted/edited postback could submit a negative quantity |
| TC-05 | Quantity > current stock (partial shortfall, stock not zero) | Should be capped/rejected or flagged | **Not enforced** — only `cstock==0` reroutes a line; a qty of e.g. 100 against a stock of 5 is accepted and passed through; the FEFO loop in `UpdateDCStock` will simply allocate what it can across batches with no error if it runs short (STK-002) | 🔴 High |
| TC-06 | Quantity > remaining order quantity | Should be rejected/capped | **Not enforced at all** (ORD-001) — the ordered-qty lookup result is discarded | 🔴 High |
| TC-07 | Duplicate product code within the same invoice grid | Should be rejected or merged | `ProductCodeValidation` (`aspx.cs:953-969`) — its duplicate-check loop is **entirely commented out**, and the method unconditionally `return true;`, so duplicates are allowed | 🟡 Medium |
| TC-08 | Same order invoiced twice (concurrent) | Second attempt should be rejected | TOCTOU race (ORD-002) — both can succeed under concurrency; sequential (non-concurrent) retries are blocked by `OrderExists` | 🟠 High under concurrency, Low sequentially |
| TC-09 | Master succeeds, detail save genuinely throws | Whole transaction rolls back, master row not left orphaned | CONFIRMED correct: `TransactionScope` rolls back on any thrown exception since `scope.Complete()` is never reached | Low |
| TC-10 | Detail succeeds partially then fails (exception mid-loop) | Whole transaction rolls back including earlier detail rows in this invoice | CONFIRMED correct for genuine exceptions (same mechanism as TC-09) | Low |
| TC-11 | DB timeout during save | Should surface a clear error and roll back | `TransactionScope` `Timeout = TimeSpan.FromMinutes(5)` will abort and roll back; the resulting `TransactionAbortedException`/`TimeoutException` is caught by the outer `catch` and shown via `ex.Message` — CONFIRMED handled, though the swallow behavior in `SaveAction` (ERR-001) could mask the specific DB-side cause | 🟡 Medium (handled, but diagnostics are weak) |
| TC-12 | Double-click Save | Second click should be inert | Mitigated: `OnClientClick` disables the button immediately (`aspx.cs:505`); a narrow race remains if the second click physically lands before the JS disables it | 🟢 Low |
| TC-13 | Two users invoice same stock simultaneously | System should serialize or reject the loser cleanly | **Not enforced** — STK-001 lost-update race; both can "succeed" with corrupted resulting stock | 🔴 High |

**Print**

| Test ID | Scenario | Expected Result | Actual Implementation Behavior | Risk |
|---|---|---|---|---|
| TC-14 | Valid invoice, print immediately after save (this page) | Full report renders with header + line items | CONFIRMED works: `invTextBox`/`rptInvoiceId` are populated post-commit and both survive into the Print postback via ViewState (§11 row 1-3) | Low |
| TC-15 | Invoice without details (INV-001 state), print | Should either not be printable or clearly show "no items" | **Blank line-items section** — CONFIRMED per §11 row 4 | 🔴 High |
| TC-16 | Wrong/tampered invoice ID in the print URL | Should show an error, not another invoice's data | The query is `AND`-ed on both `InvoiceId` and `InvoiceNo`, so a mismatched pair returns zero rows (blank), not a different invoice's data — CONFIRMED no cross-invoice data leak, but also no friendly message | 🟡 Medium (fails safe, but unclear to the user) |
| TC-17 | Newly saved invoice, print in the same session without navigating away | Renders correctly | CONFIRMED works (see TC-14) | Low |
| TC-18 | Multi-detail invoice print | All line items render | Works whenever detail rows genuinely exist; CONFIRMED by query shape (`GROUP BY` aggregates per product, `InvoiceDAL.cs:1289-1294`) | Low |
| TC-19 | Cancelled invoice (`noButton_Click` → `DeleteProforma`) print | Should indicate the invoice no longer exists | After `DeleteProforma`, the master row is gone; the report query would return zero rows for `mainDataTable`, but `Page_Init` does **not** check `mainDataTable.Rows.Count` before calling `rptdoc.SetDataSource` — CrystalReportViewer likely shows a template with no data rather than a clear "not found" message; **UNCONFIRMED** exact on-screen behavior (Crystal Reports rendering of an empty DataSet was not executed in this read-only audit) | 🟡 Medium |
| TC-20 | Print with missing master (bad/garbage invoiceId) | Clear "not found" message | No explicit "not found" branch exists; `ShowReport`'s `if (dsDataSet.Tables[0].Rows.Count > 0)` check (`InvoiceReportViewer.aspx.cs:81-95`) is **defined but never called** — `Page_Init` sets `ReportSource` directly without that guard (line 71-72) — CONFIRMED the guard exists in dead code, not on the live path | 🟠 Medium-High |
| TC-21 | Missing detail rows only (master exists) | Same as TC-15 | Same as TC-15 | 🔴 High |

**Stock**

| Test ID | Scenario | Expected Result | Actual Implementation Behavior | Risk |
|---|---|---|---|---|
| TC-22 | Invoice qty exactly equals available stock | Stock becomes 0 after save | Works for the single-batch, single-request case (no concurrency) — CONFIRMED by the arithmetic in `UpdateDCStock` | Low |
| TC-23 | Invoice qty less than available stock | Remaining stock correctly reduced | Works for the non-concurrent case | Low |
| TC-24 | Zero stock at save time | Line excluded, order line marked "Undelivered" | CONFIRMED — the one server-side branch that does work correctly (`cstock==0` at `aspx.cs:1629`) | Low (by design) but feeds INV-001 if it empties the whole grid |
| TC-25 | Negative stock somehow present in `tblDCStore` | Should never occur / should be treated as zero | `BatchWiseProductQty`'s `WHERE StockQty > 0` (`InvoiceDAL.cs:938-942`) would simply exclude such a row from allocation — CONFIRMED it would not be selected, though how a negative value could arise in the first place is a symptom of STK-001, not something this page directly protects against | 🟡 Medium |
| TC-26 | Multiple warehouses/DCs for the same product | Should allocate only from the correct DC | `BatchWiseProductQty` filters `WHERE ... AND ComUnitId='<comUnitId>'` (`InvoiceDAL.cs:938-942`), so it is scoped to the invoicing unit — CONFIRMED correctly scoped, assuming `ComUnitId` uniquely identifies the relevant warehouse/DC in this schema (schema not available to fully confirm) | Low-Medium |
| TC-27 | Multiple UOM for the same product | Quantity/pricing should respect the UOM of the line | **`InvoiceDetail` has no UOM field at all** (§7) — the model assumes a single implicit unit per product; multi-UOM is **not representable**, let alone validated | 🟠 Medium-High (functional gap, not a bug in existing logic) |
| TC-28 | Invoice fails validation/exception *after* stock was already deducted | Stock deduction should be rolled back with everything else | CONFIRMED correct **only if** an exception is actually thrown (the `TransactionScope` covers `UpdateDCStock`/`UpdateDCStoreQuantity` too, since they run inside the same scope, `aspx.cs:1418-1441`) | Low for genuine exceptions; N/A for INV-001's silent-success case (nothing to roll back to, since it doesn't fail) |
| TC-29 | Cancel invoice (`noButton_Click`) after stock was already deducted | Stock should be restored | `noButton_Click` (`aspx.cs:1890-1906`) calls `ProformaReturnPosting` then `DeleteProforma` — **neither call was found, in the code read, to touch `tblDCStore`**; whether stock restoration happens is **UNCONFIRMED** from the C# alone (it would have to happen inside `sp_ProformaReturnPosting`, whose body is not in the repo) | 🟠 High if unconfirmed restoration turns out not to happen |
| TC-30 | Duplicate save (retry after a transient failure) | Should not double-deduct stock or create two invoices | Depends entirely on ORD-002's TOCTOU window and the DOCUMENTED-UNVERIFIED 60-second SP-side heuristic; not a hard guarantee | 🟠 High |

---

## 19. Recommended Fix Plan

*(Described only — no changes were made to the codebase as part of this audit.)*

1. **INV-001**: Change `SaveInvoiceDetail`'s return contract to reflect reality — return `false` (or
   let the underlying save throw) when the resulting detail list is empty or when fewer rows were
   persisted than were attempted. No structural change to the surrounding `TransactionScope` is
   needed; it already does the right thing once given a truthful signal.
2. **STK-001 / STK-002**: Replace the read-then-write `UpdateDCStoreQuantity` call with a single
   atomic, guarded `UPDATE ... SET StockQty = StockQty - @qty WHERE DCStoreId=@id AND StockQty >=
   @qty`, check rows-affected, and add an explicit `SUM(available across batches) >= requested`
   pre-check in `UpdateDCStock` before starting allocation, failing the whole line (not partially
   allocating) if insufficient.
3. **ORD-001**: Wire up the already-fetched (but currently discarded) `LoadProductQty` result (or a
   corrected equivalent that also subtracts previously-invoiced quantity) as a hard, server-side gate
   before a row is added to `aInvoiceDetailsList`.
4. **ORD-002**: Move the `OrderExists` re-check inside the `TransactionScope` (immediately before
   `SaveInvoiceMater`) at minimum; ideally also add a unique constraint on `tblInvoice.OrderId` at
   the database level (schema access required, outside this repo's visibility) so the guarantee does
   not depend solely on application-layer timing.
5. **PRT-001**: Add a simple non-empty guard at the top of `printButton_Click` before constructing
   the report URL.
6. **PRT-002**: Update the 8 identified callers to also pass `invoiceId` (matching what
   `InvoiceCreationForCustomerByOrder.aspx.cs:1821` already does correctly), or relax the report
   query to treat a missing/zero `invoiceId` as "match on `InvoiceNo` alone", restoring the
   pre-`invoiceId`-parameter behavior for those callers.
7. **ERR-001**: Add logging (even a minimal file/EventLog append, since no framework is currently
   wired up) inside the `catch` blocks of `SaveAction`/`UpdateAction`/`ExecuteNonQueryAction` before
   returning the failure sentinel, so a future incident leaves a trace.
8. **KEY-001**: Migrate `tblInvoiceDetail.InvoiceDetailId` (and any other `MAX+1`-generated keys used
   on this page's live path) to a real `IDENTITY` column with output-parameter retrieval, consistent
   with how `tblInvoice.InvoiceId` is already correctly handled.
9. **DBI-001**: Obtain and commit actual schema DDL (or a constraint/index extract) to the repository
   so future audits of this application can confirm rather than infer database-level guarantees.

---

## 20. Priority-wise Remediation Plan

| Priority | Items | Rationale |
|---|---|---|
| P0 (immediate) | INV-001, STK-001 | Both are silent, code-confirmed defects that directly produce financially/operationally incorrect "successful" outcomes with no error surfaced to any user; both have a narrow, well-understood, low-risk fix (a truthful return value; an atomic guarded UPDATE) that does not require schema changes. |
| P1 (near-term) | PRT-002, ORD-001, STK-002 | PRT-002 is a guaranteed, 100%-reproducible defect affecting 8 pages today — trivial to fix and high user-visible value. ORD-001/STK-002 close the two remaining silent-overrun paths that make INV-001/STK-001 worse in combination. |
| P2 (next cycle) | ORD-002, PRT-001, KEY-001 | Real races and gaps, but each requires either DB schema changes (unique constraint) or is a lower-frequency trigger (needs concurrent access or a cold Print click) than P0/P1 items. |
| P3 (hardening / follow-up audit) | ERR-001, DBI-001, TC-03/04/07 (qty/duplicate validation gaps), TC-19/20/29 (Print/Cancel edge cases), UOM data-model gap (TC-27) | Important for long-term maintainability, forensics, and correctness breadth, but none were found to be actively causing the specific "master saves without details / blank print / stock drift" symptoms under investigation the way P0/P1 items are. |

---

# TOP 10 MOST CRITICAL PROBLEMS

1. **INV-001 — Invoice can commit with zero detail rows** (`InvoiceCreationForCustomerByOrder.aspx.cs:1642`)
   - *Root Cause*: `SaveInvoiceDetail` unconditionally `return true;`, defeating the caller's
     `if (!SaveInvoiceDetail(invId))` rollback gate.
   - *Evidence*: `aspx.cs:1555-1643`, `1431`.
   - *Business Impact*: "Successful" invoices with no line items; financial totals don't reconcile;
     blank prints.
   - *Recommended Fix*: Return the true outcome of the detail save; let the existing
     `TransactionScope` roll back.

2. **STK-001 — Non-atomic, unguarded stock deduction** (`InvoiceDAL.cs:799-803`)
   - *Root Cause*: Read-then-blind-overwrite `UPDATE tblDCStore SET StockQty='<value>'`, no `WHERE
     StockQty >= @qty` guard.
   - *Evidence*: `InvoiceDAL.cs:799-803`, `938-942`; `InvoiceBLL.cs:218-321`.
   - *Business Impact*: Lost stock deductions under concurrency; recorded stock drifts above
     physical reality; oversell risk.
   - *Recommended Fix*: Atomic guarded `UPDATE ... SET StockQty = StockQty - @qty WHERE ... AND
     StockQty >= @qty`.

3. **PRT-002 — 8 other pages guarantee a fully blank print via a shared report page**
   (multiple `.aspx.cs` files listed in §11 row 6)
   - *Root Cause*: `InvoiceReportViewer.aspx` was extended to require `invoiceId`; not all callers
     updated.
   - *Evidence*: grep results in §11; `InvoiceReportViewer.aspx.cs:30-33`; `InvoiceDAL.cs:1260,1294`.
   - *Business Impact*: Deterministic, 100%-reproducible blank prints from 8 entry points across the
     application.
   - *Recommended Fix*: Update the 8 callers to pass `invoiceId`, or make the SQL tolerate its
     absence.

4. **STK-002 — Silent short-ship when total available stock is insufficient** (`InvoiceBLL.cs:218-321`)
   - *Root Cause*: Batch-allocation loop has no `SUM(available) >= requested` pre-check; simply stops
     when out of batch rows.
   - *Evidence*: `InvoiceBLL.cs:218-321`.
   - *Business Impact*: Customer billed for quantity never actually allocated, no record of the
     shortfall.
   - *Recommended Fix*: Pre-check total available quantity; fail the line/invoice on shortfall rather
     than partially allocating.

5. **ORD-001 — Invoice quantity never validated against remaining order quantity** (`aspx.cs:1616-1618`)
   - *Root Cause*: The one query result that would enable this check is fetched and then discarded
     (dead/commented-out consuming line).
   - *Evidence*: `InvoiceCreationForCustomerByOrder.aspx.cs:1616-1618`; `InvoiceDAL.cs:736-742`.
   - *Business Impact*: Orders can be invoiced for more than was ordered, with only a cosmetic
     warning.
   - *Recommended Fix*: Enforce `requested qty <= remaining order qty` server-side before adding a
     row to the save list.

6. **ORD-002 — Double-invoice guard is a check-then-act race** (`aspx.cs:166-179, 1400, 1418`)
   - *Root Cause*: `OrderExists` runs before the `TransactionScope` opens; no DB-level unique
     constraint confirmable.
   - *Evidence*: `OrderInfoDAL.cs` (`LoadOrderExistsDal`); `aspx.cs:1400,1418`.
   - *Business Impact*: Concurrent submissions can double-invoice the same order.
   - *Recommended Fix*: Move the check inside the transaction and/or add a DB unique constraint on
     `tblInvoice.OrderId`.

7. **ERR-001 — Write-path exceptions are silently swallowed application-wide** (`ClsCommonInternalDAL.cs`)
   - *Root Cause*: `SaveAction`/`UpdateAction`/`ExecuteNonQueryAction` catch and discard all
     exceptions, with no logging framework configured anywhere in the application to compensate.
   - *Evidence*: `ClsCommonInternalDAL.cs` (`SaveAction`/`UpdateAction`/`ExecuteNonQueryAction`,
     ~lines 655-863); `Global.asax` empty `Application_Error` (per prior spec, cross-checked
     consistent).
   - *Business Impact*: Real DB failures are indistinguishable from legitimate business rejections,
     and leave zero forensic trail.
   - *Recommended Fix*: Log before swallowing, or stop swallowing and let the existing outer `catch`
     in `saveButton_Click1` handle it (it already does correctly when given the chance).

8. **KEY-001 — Detail-row primary key generated via unlocked `MAX+1`, inside the live save path** (`ClsPrimaryKeyFind.cs`)
   - *Root Cause*: `InvoiceDetailInsertBLL` uses `ClsPrimaryKeyFind.PrimaryKeyMax`, a plain
     `SELECT MAX(id)+1`, with no locking, unlike the properly output-parameter-based
     `tblInvoice.InvoiceId`.
   - *Evidence*: `InvoiceBLL.cs:322-392`; `ClsPrimaryKeyFind.cs`.
   - *Business Impact*: Concurrent detail-row inserts risk ID collisions/corruption.
   - *Recommended Fix*: Migrate to a real `IDENTITY` column with output-parameter retrieval.

9. **PRT-001 — Print has no guard against being invoked before a successful save** (`aspx.cs:1812-1826`)
   - *Root Cause*: Missing input validation; an empty `invoiceId` defaults to `"0"`, producing a
     malformed `IN ()` SQL clause and an unhandled error page.
   - *Evidence*: `aspx.cs:1812-1826`; `InvoiceReportViewer.aspx.cs:24-33`.
   - *Business Impact*: A confusing SQL/ASP.NET error page instead of a clear "please save first"
     message.
   - *Recommended Fix*: Add a non-empty check at the top of `printButton_Click`.

10. **TRX-001 — This is the only transactional save flow in the whole application** (repo-wide grep)
    - *Root Cause*: `TransactionScope` was added to this one handler only; every other multi-step
      save flow (including this page's own Cancel path) remains a chain of independent autocommit
      calls.
    - *Evidence*: repo-wide grep for transaction primitives, 2 matches total, both in this file.
    - *Business Impact*: The class of problem this audit was commissioned to investigate here likely
      exists, unmitigated, in every other multi-step save flow in the application.
    - *Recommended Fix*: Use this page's `TransactionScope` + honest-return-value pattern as the
      template for a follow-up, application-wide remediation pass.

---

# MOST LIKELY ROOT CAUSE OF THE CURRENT ISSUES

**(A) Why Invoice Master saves but Details sometimes don't:**
Because `SaveInvoiceDetail` (`InvoiceCreationForCustomerByOrder.aspx.cs:1555-1643`) **always
`return true;` at line 1642**, regardless of whether `aInvoiceDetailsList` ended up with any rows in
it. Any grid line whose displayed `currentStockTextBox` reads `0` at the instant Save is clicked is
excluded from that list (lines 1629-1636) and only triggers a `tblOrderDetail` status flip to
"Undelivered" — it never causes `SaveInvoiceDetail` to report failure. If that happens to every line
in the grid (e.g., stock was consumed by another process between order-load and save-click), the
list handed to `InvoiceBLL.SaveInvoiceDetails` is empty, zero `INSERT`s ever happen against
`tblInvoiceDetail`, and the caller's gate at line 1431
(`if (!SaveInvoiceDetail(invId)) { ...; return; }`) is structurally incapable of catching this,
because the callee it is checking never returns anything but `true`. The surrounding
`TransactionScope` then reaches `scope.Complete()` and commits a master-only invoice.

**(B) Why print can be blank:**
Two independent, code-confirmed mechanisms, both verified by direct read of the SQL:
1. For invoices affected by (A) above: `InvoiceDetailDataForReport`'s `LEFT JOIN tblInvoiceDetail AS
   IVD ON IVD.InvoiceId = IV.InvoiceId` (`InvoiceDAL.cs:1291-1292`) preserves the master row and
   returns every `IVD.*` column as `NULL` when no detail rows exist — the Crystal Report then has a
   populated header and an empty line-item section, i.e. what a user calls "blank."
2. For prints started from 8 *other* pages in the application (`BatchWiseTopSheet.aspx`,
   `BatchWiseDelReturnSummery.aspx`, `BatchWiseDeliveryTopSheet.aspx`,
   `InvoiceCreationForConsumerCustomer.aspx`, `InvoiceCreationForCustomer.aspx`,
   `ReturnInvoiceCreationForCustomer.aspx`, `ReturnOnDeliveryReport.aspx`, `TopSheetGenerate.aspx`):
   each builds the `InvoiceReportViewer.aspx` URL with only `InvNo`, never `invoiceId`
   (§11 row 6). `InvoiceReportViewer.aspx.cs:30-33` defaults the missing `invoiceId` to `"0"`, and
   both report queries `AND` on `IV.InvoiceId = 0` (`InvoiceDAL.cs:1260, 1294`) — which matches no
   row, so the **entire report, header included, is blank**, every single time, for every invoice
   printed from any of those 8 pages. `InvoiceCreationForCustomerByOrder.aspx.cs:1821` itself
   correctly passes both parameters, so this specific page's own print button is not subject to
   mechanism 2 — but any complaint of "blank print" gathered from users across the application as a
   whole would very plausibly be dominated by mechanism 2, not by this page.

**(C) Why stock can become incorrect after invoice creation:**
`InvoiceDAL.UpdateDCStoreQuantity` (`InvoiceDAL.cs:799-803`) performs
`UPDATE tblDCStore SET StockQty='<value>' WHERE DCStoreId='<id>'`, where `<value>` is a **new
absolute quantity computed in C#** from an earlier, separate, unlocked `SELECT`
(`InvoiceDAL.BatchWiseProductQty`, `InvoiceDAL.cs:938-942`) inside `InvoiceBLL.UpdateDCStock`
(`InvoiceBLL.cs:218-321`). There is no `WHERE StockQty >= @qty` guard and no decrement expression —
it is a classic read-then-write race. Two concurrent invoice saves that both read the same starting
`StockQty` for a given `DCStoreId` before either writes back will silently lose one of the two
deductions (last write wins) — with **no exception, no log entry, and no user-visible indication**
that anything went wrong, because both individual `UPDATE` statements succeed from SQL Server's
point of view. A comment at `InvoiceCreationForCustomerByOrder.aspx.cs:1411-1414` confirms this was
a **known, deliberate tradeoff**: `ReadCommitted` isolation was chosen over the `TransactionScope`
default of `Serializable` specifically "to avoid range locks on `tblDCStore`," which sacrifices the
one thing (broader locking) that might otherwise have partially compensated for the missing atomic
guard clause in the `UPDATE` statement itself. Separately, `InvoiceBLL.UpdateDCStock`'s
batch-allocation loop has no check that total available stock across all FEFO batches is sufficient
for the requested quantity before it starts allocating (STK-002) — if it is not, the loop simply
stops early, silently under-allocating without any error, which is a second, independent way
recorded stock and actual fulfilled quantity can diverge after an invoice is created.

---

*End of report. No source file, database object, or git state was modified in the course of this
investigation.*
