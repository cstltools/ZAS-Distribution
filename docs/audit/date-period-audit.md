# Inventory Date / Period Integrity Audit

Scope: date fields on stock/inventory entities in `Library.DAO/SInventory_Entities`, how they are set in
`Library.BLL/SInventory_BLL` and `Library.DAL/SInventory_DAL`, and how they are bound/validated in
`Solution.Web/SInventory_UI/*.aspx.cs`. Read-only audit — no source or data modified.

Companion document: `docs/audit/inventory-audit.md` (stock transaction integrity). Bug IDs continue that
document's `BUG-STOCK-` sequence.

---

## 1. Date-field catalog

| Date field | Entity (file:line) | Set from | Validated against today / period? | Risk |
|---|---|---|---|---|
| `ReceiveDate` (central/DC store) | `DCStore.cs`, `DCStockNew.cs:17`, `DCStoreFreezeDAO.cs:20`, `CentralStore.cs` | UI TextBox + `CalendarExtender`, e.g. `StockReceive.aspx.cs:105` (`Convert.ToDateTime(receiveDateTextBox.Text)`, box pre-filled with `DateTime.Today` but freely editable), `ReceiveProductByChalanByDC.aspx.cs:125`; in the DC-to-DC transfer flow, copied from the **source challan's `ChalanDate`** rather than the actual destination receive event (`ChalanDAL.cs:200-209` → `TransferReceiveProductByChalanByDC.aspx.cs:66`) — see `BUG-STOCK-006` | No `RangeValidator`/`CompareValidator` found on any of these pages; no `> DateTime.Now`/`< DateTime.Today` check in code-behind | Full backdating and future-dating on user-entered paths; date-copy bug on the transfer-receipt path |
| `StockRcvDate` | `DCStoreFreezeDAO.cs:26`, `DCStockNew.cs:24`, `WhStoreFreezeDao.cs:21` | Divergent by page: (a) `DateTime.Now` — `StockConditionFreeze.aspx.cs:220,280,430,510`, `WhStockConditionFreeze.aspx.cs:186` (correct — new event, current date); (b) UI TextBox `rcvDateTextBox.Text` — `ReceiveProductByChalanByDC.aspx.cs:91,126`, `TransferReceiveProductByChalanByDC.aspx.cs:67,98` (user-editable, defaults to today); (c) copied verbatim from the **original pre-sale batch row** on sales-return freeze — `ReturnInvoiceCreationForCustomer.aspx.cs:776`, `ReturnustomerByInvoiceApproval.aspx.cs:130-131`, `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs:1036,1212` — see `BUG-STOCK-006` | No validator on the TextBox paths | Backdating/future-dating on (b); date-copy misstates quarantine aging on (c) |
| `ExpDate` / `MfgDate` | `WharehouseStockInDetailsDao.cs:14-15`, `DCStockNew.cs:16,33`, `StockInTransfar.cs:22,28` | UI TextBox + `CalendarExtender`, e.g. `WarehouseStockIn.aspx.cs:523-524,728-729`, `MaterialConversionReceive.aspx.cs:472-473,563-564` (which auto-defaults `ExpDate = MfgDate.AddYears(2)`, a hardcoded shelf-life assumption, `MaterialConversionReceive.aspx.cs:120-121`), `WarehouseStockOut.aspx.cs:423-424` | None — no check that `ExpDate > MfgDate`, none that either is plausible (not decades in the past/future) | Fully user-controlled batch dating with zero sanity checking; directly feeds `NearlyExpireProductReport`/FEFO ordering, and (per `BUG-STOCK-016`) expired batches are not blocked from sale at all |
| `ChalanDate` | `DCStore.cs:19`, `DCStockNew.cs:19` | UI TextBox `chalanDateTextBox`, `NewStockTransferDcToDc.aspx.cs:202`; `clnDateTextBox`, `ReceiveProductByChalanByDC.aspx.cs:128` | None | Backdating/future-dating; also the value later gets reused as a stand-in for `ReceiveDate` on the receiving side (`BUG-STOCK-006`) |
| `StockOutDate` (Destroy) | `DestroyProductDao.cs:15` | UI TextBox `dateTextBox.Text`, `DestroyProduct.aspx.cs:125` | None | User can record a destruction event as happening on any past/future date, undermining the approval-sequencing/audit trail (destroy is submitted → approved later; a backdated `StockOutDate` can predate the approval date it should logically follow) |
| `StockOutDate` (Direct Stock Out) | `DirectStockOutDao.cs:17` | `DateTime.Now`, `DirectStockOut.aspx.cs:191` | System-set | Low risk |
| `ApproveDate` / `ApprovedDate` (all approval screens: Destroy, Warehouse Stock In, Trade Material Transfer, etc.) | `DestroyProductDao.cs:20`, `WarehouseStockInMasterDao.cs:28`, etc. | `DateTime.Today`/`DateTime.Now`, e.g. `DestroyProductApproval.aspx.cs:81`, `WarehouseStockInApproval.aspx.cs:159,205` | System-set | Low risk — consistently system-set across the module |
| `EntryDate` | Almost every master/detail DAO | `DateTime.Now` at code-behind save time (e.g. `TradeMaterialTransfer.aspx.cs:388`, `WarehouseStockIn.aspx.cs:492`, `WarehouseStockOut.aspx.cs:390`) | System-set | Low risk |
| `ReqDate` / `SubmitDate` / `IssuChalanDate` / `PickingDate` | `Requesition.cs:12,19,21,31` | UI TextBox `clnDateTextBox`, `IssueRequisitionProducts.aspx.cs:172,176` | None | Backdating/future-dating across the requisition → issue → picking chain |
| `ReferenceDate` | `WarehouseStockInMasterDao.cs:20` — typed as `string`, not `DateTime` | Raw UI TextBox text, `WarehouseStockIn.aspx.cs:482,693` | Not even type-validated | Free-text field with no date semantics enforced at all — a data-quality risk independent of backdating |

---

## 2. Period-close / financial-year gating

**CONFIRMED: no period-closing or financial-year lock exists anywhere in the system, not just in the inventory
module.** Grepped `PeriodClose`, `ClosingDate`, `LockDate`, `IsClosed` across the entire repository — zero
matches. `FinancialYear` exists as a concept (`Library.DAL/SInventory_DAL/InvoiceDAL.cs:1643-1647`,
`SalesDashboardDal.cs` financial-year lookups) but only as a **reporting filter dropdown** — it is never read
back to validate or block a transaction date at save time.

**CONFIRMED: no future-date guard exists on any inventory transaction date field.** Grepped
`> DateTime.Now` / `> DateTime.Today` across `Solution.Web/SInventory_UI/*.aspx.cs` and
`Library.BLL/SInventory_BLL/*.cs` — zero matches for any inventory transaction save path. No markup-level
`RangeValidator`/`CompareValidator` was found on any of the date TextBoxes listed above either.

---

## 3. Historical-report "as of" date-column risk

- `Library.DAL/SInventory_DAL/WhStockMonitoringReportDal.cs` filters the sales/issue leg of the warehouse
  stock-monitoring report by `tblInvoice.UpdateDate BETWEEN @FromDate AND @ToDate` rather than
  `tblInvoice.InvoiceDate`. `UpdateDate` changes whenever the invoice row is later touched (status change,
  correction), so a report run "as of" a date range can silently include/exclude invoices whose real business
  date (`InvoiceDate`) falls outside the requested window. The same query correctly uses `ReceiveDate`
  (stock-in leg) and `PickingDate` (issue leg elsewhere), which makes the `UpdateDate` choice on the sales leg
  look like an inconsistency rather than a deliberate design choice. **SUSPECTED** (plausible reporting defect,
  not exhaustively cross-checked against every report in the module).
- `DCStockReportDAL.cs` and `FreezeStockReportDal.cs` (current-stock reports) filter only on `StockQty > 0`
  with **no date parameter at all** — they cannot be run "as of" a historical date, correct or otherwise. This
  is a functional gap (no historical stock snapshot capability for these two report types), distinct from a
  wrong-column bug.
- Other stock reports that do carry a date range (`sp_DCBinCard`, `sp_NationalBinCard`, `sp_WearhouseBinCard`,
  per `docs/database/stored-procedures.md` §E) delegate entirely to stored procedures whose bodies were not
  found in the recovered `docs/database/*` batches — **HUMAN VERIFICATION REQUIRED** to confirm which date
  column each actually filters on internally.

---

## 4. Bugs (continuing the `BUG-STOCK-` sequence from `inventory-audit.md`)

| ID | Title | Severity | Confidence |
|---|---|---|---|
| BUG-STOCK-019 | No future-date or backdating guard on any inventory transaction date field system-wide — every date-entry TextBox across `WarehouseStockIn.aspx`, `MaterialConversionReceive.aspx`, `WarehouseStockOut.aspx`, `DestroyProduct.aspx`, `StockConditionFreeze.aspx`, `ReceiveProductByChalanByDC.aspx`, `TransferReceiveProductByChalanByDC.aspx`, `NewStockTransferDcToDc.aspx`, `IssueRequisitionProducts.aspx` binds to a `CalendarExtender` with no server-side or markup validator and no `> DateTime.Now` check anywhere in the corresponding code-behind. | HIGH | CONFIRMED |
| BUG-STOCK-020 | No period-close / financial-year lock exists anywhere in the system (not inventory-specific) — even a validated date could still post into an already-closed accounting period, because no such concept is enforced at write time anywhere in the codebase. `FinancialYear` is reporting-only. | HIGH | CONFIRMED |
| BUG-STOCK-021 | `ExpDate`/`MfgDate` have no cross-field sanity check (`ExpDate` is never validated to be after `MfgDate`, neither is bounded to a plausible range), and `MaterialConversionReceive.aspx.cs:120-121` silently defaults `ExpDate = MfgDate.AddYears(2)` regardless of the actual product's real shelf life (user can override, so not silently wrong, but a latent data-entry trap). | MEDIUM | CONFIRMED |
| BUG-STOCK-022 | `WhStockMonitoringReportDal` filters the sales leg of the stock-monitoring report by `tblInvoice.UpdateDate` instead of `InvoiceDate`, which can mis-scope "as of a date range" results whenever an invoice row was edited after its original business date. | MEDIUM | SUSPECTED |
| BUG-STOCK-023 | `WarehouseStockInMasterDao.ReferenceDate` is declared as `string` (not `DateTime`) and bound directly from free-text TextBox input with no date parsing/validation at all (`WarehouseStockIn.aspx.cs:482,693`). | LOW | CONFIRMED |
| (see also BUG-STOCK-006 in `inventory-audit.md`) | Date copied from parent/source transaction into a derived transaction (sales-return freeze `StockRcvDate`; DC-to-DC transfer `ReceiveDate` sourced from `ChalanDate`) | MEDIUM | CONFIRMED |

---

## 5. Files read for this audit (key ones)

`Library.DAO/SInventory_Entities/DCStore.cs`, `DCStockNew.cs`, `DCStoreFreezeDAO.cs`, `WhStoreFreezeDao.cs`,
`DestroyProductDao.cs`, `DirectStockOutDao.cs`, `WarehouseStockInMasterDao.cs`, `WharehouseStockInDetailsDao.cs`,
`Requesition.cs`, `CentralStore.cs`
`Library.DAL/SInventory_DAL/InvoiceDAL.cs`, `ChalanDAL.cs`, `WhStockMonitoringReportDal.cs`,
`DCStockReportDAL.cs`, `FreezeStockReportDal.cs`, `SalesDashboardDal.cs`
`Solution.Web/SInventory_UI/StockReceive.aspx.cs`, `ReceiveProductByChalanByDC.aspx.cs`,
`TransferReceiveProductByChalanByDC.aspx.cs`, `NewStockTransferDcToDc.aspx.cs`, `WarehouseStockIn.aspx.cs`,
`WarehouseStockOut.aspx.cs`, `MaterialConversionReceive.aspx.cs`, `DestroyProduct.aspx.cs`,
`DestroyProductApproval.aspx.cs`, `StockConditionFreeze.aspx.cs`, `WhStockConditionFreeze.aspx.cs`,
`IssueRequisitionProducts.aspx.cs`, `ReturnInvoiceCreationForCustomer.aspx.cs`,
`ReturnustomerByInvoiceApproval.aspx.cs`, `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs`
`docs/database/stored-procedures.md` (financial-year procedures, §J)
