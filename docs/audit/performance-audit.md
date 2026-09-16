# Performance Audit — ZAS Distribution (SalesRoll)

Scope: static grep/read across `Library.DAL` and hot code-behind paths. No profiling, no DB access, no query
plans. Findings are marked **CONFIRMED** (exact pattern read in source) or **POTENTIAL** (pattern present,
performance impact plausible but not measured). Anything requiring DB access (missing indexes, actual execution
plans, row counts, lock contention) is marked **UNKNOWN / HUMAN VERIFICATION REQUIRED** — not claimed as fact.

---

## 1. `SELECT *` / unbounded column retrieval

**CONFIRMED** — `grep -i "select \*"` across `Library.DAL` returns **448 occurrences across 86 files**. Examples
of files with the heaviest concentration: `InvoiceDAL.cs` (27), `OrderInfoDAL.cs` (27), `ProductDiscountDAL.cs`
(20), `PanalDAL.cs` (19), `ChalanDAL.cs` (16), `ProductDAL.cs` (15), `RequisitionDAL.cs` (14),
`CustomerMasterDAL.cs` (14), `MarketInfoDAL.cs` (12).

Impact (POTENTIAL, no DB access to confirm actual column counts or covering indexes): `SELECT *` prevents
covering-index use, pulls unused columns (including any large `text`/`varchar(max)` columns present in the 126
tables), and silently changes result shape if a table gains a column — several `DataTable`/positional-index
consumers downstream (`GAP-APP-010`) would then read the wrong ordinal.

## 2. String-concatenated dynamic SQL — no query plan caching

**CONFIRMED**, mechanism re-verified this pass (see `docs/audit/security-audit.md` §1 for the security angle).
`Library.DAL/InternalCls/ClsCommonInternalDAL.cs` exposes `DataContainerDataTable(queryString)` and equivalents,
which are the terminal call for ~1,400 hand-built SQL strings per the prior audit's count. Each such string is a
distinct literal text to SQL Server (parameter values are baked into the string, not passed as `SqlParameter`s in
the vast majority of DAL methods), so:

- SQL Server cannot reuse a cached plan across calls with different literal values — every invocation with a
  different customer id, date, etc. is a fresh `ad hoc` plan compile (or, worse, contributes to plan-cache
  bloat/eviction of genuinely reusable plans from the ~119 real parameterized stored procedures).
- This is a **CONFIRMED architectural pattern** (the code shape is unambiguous); the *magnitude* of the
  performance cost (CPU spent compiling, cache pollution, whether `optimize for ad hoc workloads` is enabled)
  is **UNKNOWN / HUMAN VERIFICATION REQUIRED** — that needs `sys.dm_exec_query_stats` / plan cache telemetry
  from the live server.

## 3. N+1 query patterns

### CONFIRMED — `InvoiceBLL.SaveInvoiceDetails` (`Library.BLL/SInventory_BLL/InvoiceBLL.cs:194-202`)

```csharp
public bool SaveInvoiceDetails(List<InvoiceDetail> aInvoiceList, string comUnitId)
{
    foreach (var invoiceDetail in aInvoiceList)
    {
        UpdateDCStock(invoiceDetail, comUnitId);   // one call per invoice LINE
    }
    return true;
}
```

`UpdateDCStock` (line 218) itself calls `aInvoiceDal.BatchWiseProductQty(...)` (1 round trip) per line, and the
per-batch-split path calls `InvoiceDetailInsertBLL` (line 322) which calls `ProductInfo(...)` (1 round trip),
`aClsPrimaryKeyFind.PrimaryKeyMax(...)` (1 round trip, see §4), and `aInvoiceDal.SaveDataForInvoiceDetails(...)`
(1 insert round trip) — **per line, potentially per batch split within a line**. An invoice with 20 line items
that each span 2 stock batches is on the order of 20 × (1 + 2×3) = ~140 sequential DB round trips to save one
document, none of them wrapped in a single batch/table-valued-parameter call. This is the DAL-side mechanism
behind `GAP-APP-006`/`GAP-APP-007` (non-transactional, racy saves) and is independently a throughput problem.

### CONFIRMED — `CustomerPayment.aspx.cs::saveButton_Click` (lines ~390-444)

Already identified functionally as `GAP-APP-017` (N payment masters for one receipt); the same loop is also a
performance N+1: for each ticked invoice row, the loop calls `aCustPaymentBll.UpdateInvoicePaymentAmount(...)`
(1 round trip, line 411/418) then builds a fresh 1-row detail list and calls
`aCustPaymentBll.SaveCustPayment(...)` (at least 1 more round trip, line 443) — 2+ sequential DB calls per
ticked invoice, in a UI-thread loop, instead of one batched save.

### POTENTIAL — widespread `for (i = 0; i < grid.Rows.Count; i++)` loops in code-behind

Grepped `Solution.Web/SInventory_UI/*.aspx.cs` for `for (... Rows.Count ...)`: **420 occurrences across 88
files** (e.g. `ManualOrderrCreation.aspx.cs` 15, `InvoiceCreationForCustomerByOrder.aspx.cs` 23,
`InvoiceCreationForCustomer.aspx.cs` 6). Not all of these call the DAL per iteration — many just read
already-bound `GridViewRow` cell values with no DB call. Only `CustomerPayment.aspx.cs` and the
`InvoiceBLL.SaveInvoiceDetails` chain above were traced to a confirmed per-row DB round trip; the rest are
**POTENTIAL** N+1 candidates that would need per-file tracing to confirm or rule out.

## 4. Application-generated primary keys (`MAX(id)+1`)

**CONFIRMED**, restating `GAP-APP-006` with the performance angle added:
`Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:12-45` —

```csharp
string query = @"SELECT (isnull(MAX(" + columnName + "),0)+1) as PKMaxNo FROM " + tableName;
```

Every insert path using this (which includes the hot invoice-line insert path in §3) issues a full
`MAX()` aggregate scan of the target table as a separate round trip immediately before every insert, with no
transaction spanning the read and the subsequent insert. Performance impact: an unindexed or poorly-indexed
`MAX()` scan cost grows with table size (this is a running sales/ERP system — `tblInvoiceDetail` is plausibly
large); correctness impact (race condition under concurrent inserts) is the separately-tracked `GAP-APP-006`.
Whether the scanned column has a supporting index is **UNKNOWN / HUMAN VERIFICATION REQUIRED** (needs
`sys.indexes`).

## 5. Cursors

**Not found.** Grepped `Library.DAL` for `DECLARE ... CURSOR` / `FETCH NEXT` (case-insensitive): zero matches.
No T-SQL cursor usage is visible from the application layer — but recall `GAP-DB-001`: 119 stored procedure
bodies are not in this repository, so cursor usage **inside** those procedures is **UNKNOWN / HUMAN
VERIFICATION REQUIRED**, not ruled out.

## 6. `NOLOCK` usage

Noted in passing while reading `AuditReportOneDal.cs`: `FROM tblOrderDel AS OD WITH(NOLOCK)`. Only two call
sites opened this pass; broader use was not grepped. **POTENTIAL** dirty-read risk on reporting queries, not
sized further — flagging as a pattern to watch rather than a counted finding.

## 7. Bug register (performance)

| ID | Title | Severity | Confidence |
|---|---|---|---|
| `BUG-PERF-001` | 448 `SELECT *` occurrences across 86 `Library.DAL` files — no column-level projection anywhere | Medium | Confirmed pattern; magnitude of impact Potential |
| `BUG-PERF-002` | ~1,400 string-concatenated ad hoc SQL statements (via `ClsCommonInternalDAL`) prevent SQL Server plan-cache reuse | Medium | Confirmed pattern; measured impact Unknown/Human verification required |
| `BUG-PERF-003` | `InvoiceBLL.SaveInvoiceDetails` → `UpdateDCStock` → `InvoiceDetailInsertBLL` performs multiple sequential DB round trips per invoice line (and per batch split within a line), fully serial, no batching | High | Confirmed |
| `BUG-PERF-004` | `CustomerPayment.aspx.cs::saveButton_Click` issues 2+ sequential DB calls per ticked invoice inside a UI-thread loop | Medium | Confirmed |
| `BUG-PERF-005` | `ClsPrimaryKeyFind.PrimaryKeyMax` issues a `SELECT MAX(col)+1 FROM table` round trip immediately before every insert on this path, untransacted with the insert | Medium | Confirmed (pattern); index support Unknown/Human verification required |
| — | Missing indexes / actual query plans / lock contention / cursors inside the 119 unseen stored procedures | — | **UNKNOWN / HUMAN VERIFICATION REQUIRED** — requires live DB access (`GAP-DB-001`, `GAP-DB-002`) |
