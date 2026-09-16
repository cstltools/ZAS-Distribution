# Purchase / Stock-Receive Audit

Audit-only. No source, data, or database was modified.

## Executive finding

**CONFIRMED — there is no external-supplier purchase-order / procurement module in this codebase.** A
repo-wide case-insensitive search for `PurchaseOrder`, `GoodsReceipt`, `GRN`, and the bare word `purchase`
across every `.cs`/`.aspx` file returned zero matches. Goods enter the system exclusively through:

1. `WarehouseStockIn.aspx` — a warehouse goods-receipt screen recording a supplier, challan, qty, price and
   VAT directly against a `tblSupplierInformation` row. This is the closest analogue to a "purchase"
   transaction, but it is **receipt-only**: no purchase order is ever created, referenced, or matched against.
2. DC-to-DC / B2B transfer-challan receipt (`ReceiveProductByChalanByDC.aspx`,
   `TransferReceiveProductByChalanByDC.aspx`, `StockReceiveByDC.aspx`) — internal movement between the
   company's own distribution centres, not an external-supplier transaction.
3. `MaterialConversionReceive.aspx` — receipt from an internal material-conversion/manufacturing process; its
   supplier dropdown is explicitly commented out (`MaterialConversionReceive.aspx.cs:73`).

`tblSupplierInformation` is **not** dead master data — it is read by a real transaction
(`WarehouseStockIn.aspx`) — but the transaction that reads it has no purchase-order concept, no
ordered-vs-received matching, and no three-way match. Per specs/master-data, this confirms SupplierInformation
is live but the "purchase" flow around it is a bare goods-receipt form, not a procurement module.

## Bug list

### BUG-PUR-001 — No purchase order exists; receipt is unconstrained against any authorization document
**Severity:** HIGH · **Confidence:** CONFIRMED (module absence — architectural gap, not a code defect per se)

`Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs:548-646` (`Validation()`) and `:457-546` (`SaveAllData()`)
require Warehouse, Supplier, Stock-In Date, Challan No/Date and line items — no `PurchaseOrderId`/`POId`
field, no lookup against an expected/ordered quantity. A user can post any quantity of any product against any
supplier with no upstream authorization.

### BUG-PUR-002 — GL/journal posting for warehouse stock-in is dead code
**Severity:** CRITICAL if `sp_WHStockInPosting` is the only place a supplier payable is booked · **Confidence:** CONFIRMED (dead code) / SUSPECTED (financial impact)

- `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:115-121` — `PostJournal(int stockInMasterId)`
  calls `sp_WHStockInPosting`.
- Only call site, `Solution.Web/SInventory_UI/WarehouseStockInApproval.aspx.cs:177-180`, is commented out:
  ```csharp
  // if (approvalBll.InsertDataIntoCenterlWarehouse(PrepareData(stockInMasterId)))
  // {
  //     approvalBll.PostJournal(stockInMasterId);
  // }
  approvalBll.ApprovalUpdateBLL(aMasterDao);
  ```
- Approving a stock-in inserts stock directly into `tblCentralStore` via `InsertDataIntoCenterlWarehouse`
  (raw C# INSERT), but the accounting journal that should book the inventory debit / supplier payable is
  never invoked. **HUMAN VERIFICATION REQUIRED — needs `sp_helptext sp_WHStockInPosting`** to confirm goods
  can be received into stock with no corresponding GL/AP liability.

### BUG-PUR-003 — Approval reuses a Material-Conversion stored procedure for ordinary stock-in
**Severity:** LOW · **Confidence:** SUSPECTED

`Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:125-136`
(`ApproveWarehouseStockInMaster` → `sp_Acc_MaterialConversionReceive`) is correctly gated to
`MaterialConversionCount > 0` (`WarehouseStockInApproval.aspx.cs:167`); ordinary supplier receipts use the
`InsertDataIntoCenterlWarehouse` branch instead. Not a functional bug as read, but SP reuse across unrelated
business processes is fragile. **HUMAN VERIFICATION REQUIRED — needs `sp_helptext sp_Acc_MaterialConversionReceive`**
to confirm it makes no conversion-specific assumptions wrong for a genuine supplier receipt.

### BUG-PUR-004 — Duplicate-receipt prevention is a time-window heuristic, not a uniqueness constraint
**Severity:** HIGH · **Confidence:** CONFIRMED (no atomic enforcement in code) — DB constraint unknown

- Supplier receipt: `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:82-88`
  (`CheckEntryWithinlastMinute`) keys only on `EntryBy` + a 60-second window — **`ChallanNo` is never part of
  the check**, so the same physical delivery challan can be keyed twice by a different user, or by the same
  user after 61 seconds (e.g. a retried timeout), as two independent `tblWHStockInMaster` rows.
- DC transfer receipt: `Library.DAL/SInventory_DAL/RequisitionDAL.cs:510-514` (`CheckDuplicateEntry`, ±5-minute
  window) plus a second, inconsistent check `CheckStoExistOrNotInDcStore` (`RequisitionDAL.cs:396-402`,
  "today + exact qty match") stacked on top — a resubmission outside either window, or with a slightly
  different quantity, is not caught.
- All are "SELECT then INSERT" (TOCTOU race), with no visible DB unique constraint (schema not in repo —
  **HUMAN VERIFICATION REQUIRED**).

### BUG-PUR-005 — Detail-row "duplicate" checks are structurally dead
**Severity:** MEDIUM · **Confidence:** CONFIRMED

- `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:103-119` (`SaveWarehouseStockInDetail`) checks
  `CheckDetailsExistOrNot` (`WarehouseStockInDal.cs:194-201`) keyed on a `WHStockInMasterID` that
  `StockInId()` (`WarehouseStockInBll.cs:59-64`) always mints fresh — no prior row can ever match on the save
  path.
- The update path (`UpdateWarehouseStockInDetail`, `WarehouseStockInBll.cs:142-149`) doesn't call the check at
  all; it deletes all existing detail rows then blind-inserts.
- `WarehouseStockInApprovalDal.CheckDetailsExistOrNot` (`:55-61`) keys on `MigoDetailID`
  (= `WHStockInDetailID`), unique per row by definition — also structurally incapable of matching.

### BUG-PUR-006 — Non-transactional multi-statement posting
**Severity:** HIGH · **Confidence:** CONFIRMED

- Save: `WarehouseStockIn.aspx.cs:495-535` — one master INSERT, then N detail INSERTs one row at a time in a
  loop (`WarehouseStockInBll.cs:103-119`); no `SqlTransaction`/`TransactionScope` wraps master+details.
- Update: `WarehouseStockIn.aspx.cs:665-744` — UPDATE, then DELETE ALL existing details
  (`DeleteWhStockInDetailById`), then a loop of fresh inserts — three-plus independent round trips; a
  mid-loop failure leaves the DELETE already committed with a partial/empty new detail set (real data loss,
  not just inconsistency).
- Approval posting: `WarehouseStockInApprovalBll.InsertDataIntoCenterlWarehouse` (`:43-62`) loops per detail
  row, again not transactional.
- Per the app-wide pattern already documented in specs (`DB_Manager.ExecuteNonQueryAction` swallows exceptions
  and returns 0), a partial failure here looks like "0 rows affected" rather than a visible error.

### BUG-PUR-007 — Int32/decimal quantity mismatch can crash total-quantity calculation
**Severity:** MEDIUM · **Confidence:** CONFIRMED (code) / SUSPECTED (business impact — depends on whether fractional receipts occur)

`WarehouseStockIn.aspx.cs:862-899` (`CalculateQuantityVatAndValue`) does
`int qty = Convert.ToInt32(...reqQtyTextBox.Text...)`, but the client-side filter explicitly allows decimal
input (`ValidChars="0123456789."`) and the per-line DAO field is `decimal`
(`WarehouseStockIn.aspx.cs:525`: `Qty = Convert.ToDecimal(quantityTextBox.Text)`). A fractional quantity
(e.g. `12.5`) throws `FormatException` here, with no `try/catch`, aborting the whole entry. The master total
field is also `Int32` throughout (`:474`), so even a workaround would silently truncate fractional totals.

### BUG-PUR-008 — `SELECT MAX(id)+1` primary-key race condition
**Severity:** HIGH · **Confidence:** CONFIRMED (pattern) — DB constraint unknown

`Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:12-28` (`PrimaryKeyMax`) is used for both the supplier-receipt
master ID (`WarehouseStockInBll.cs:59-64` → `tblWHStockInMaster`) and the DC-transfer-receipt ID
(`RequisitionBLL.cs:403,435` → `tblDCStore`), as a separate round trip before the INSERT. Two concurrent
submissions can read the same `MAX(id)+1`, producing either a duplicate-key failure or (if no PK constraint
exists) two rows silently sharing an ID, corrupting joins to detail tables/reports. **HUMAN VERIFICATION
REQUIRED** — confirm whether these ID columns carry a DB-enforced PK/unique constraint (no DDL in repo,
consistent with `GAP-DB-002`).

### BUG-PUR-009 — Unparameterized SQL throughout the receipt/payable path
**Severity:** CRITICAL if reachable by a low-trust role · **Confidence:** CONFIRMED (pattern) / SUSPECTED (severity — role/permission data not in repo)

`WarehouseStockInDal.cs:90-130,274-293` and `WarehouseStockInApprovalDal.cs:38-44,63-113` build INSERT/UPDATE
statements by string concatenation of `ChallanNo`, `ReferenceNo`, `Remarks`, `EntryBy` (from
`Session["LoginName"]`), with no `SqlParameter` usage — consistent with the application-wide pattern already
flagged in `specs/SPEC-GAPS.md` (`GAP-SEC-005`), re-confirmed here specifically on the goods-receipt/payable
path.

## Summary table

| ID | Title | Severity | Confidence |
|---|---|---|---|
| BUG-PUR-001 | No PO exists; unconstrained goods receipt | HIGH | CONFIRMED (module absence) |
| BUG-PUR-002 | GL posting SP call commented out | CRITICAL | CONFIRMED code / SUSPECTED impact — needs `sp_helptext sp_WHStockInPosting` |
| BUG-PUR-003 | Approval reuses Material-Conversion SP | LOW | SUSPECTED — needs `sp_helptext sp_Acc_MaterialConversionReceive` |
| BUG-PUR-004 | Duplicate-receipt check is time-window heuristic | HIGH | CONFIRMED — DB constraint unknown |
| BUG-PUR-005 | Detail-level "duplicate" checks are structurally dead | MEDIUM | CONFIRMED |
| BUG-PUR-006 | Non-transactional multi-statement posting | HIGH | CONFIRMED |
| BUG-PUR-007 | Int32/decimal qty mismatch crashes total calc | MEDIUM | CONFIRMED code / SUSPECTED impact |
| BUG-PUR-008 | `SELECT MAX(id)+1` PK race condition | HIGH | CONFIRMED pattern — DB constraint unknown |
| BUG-PUR-009 | Unparameterized SQL on receipt/payable fields | CRITICAL (conditional) | CONFIRMED pattern / SUSPECTED severity |

**Purchase-order calculation logic (cost matching, PO-vs-received variance, etc.): NOT APPLICABLE** — no such
document/logic exists in code. The Qty × Price + Qty × VAT-per-unit → line total → master total calculation
that does exist lives inside the goods-receipt screen (`WarehouseStockIn.aspx.cs:833-899`) and is covered
under BUG-PUR-006/007.

**Needs human/DB verification:**
- `sp_WHStockInPosting`, `sp_Acc_MaterialConversionReceive` — SP bodies not available.
- Whether `tblWHStockInMasterID`/`DCStoreId` carry DB-enforced PK/unique constraints (schema not in repo).
