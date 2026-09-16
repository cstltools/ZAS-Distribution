# SQL RESOURCE_SEMAPHORE Diagnosis

Read-only diagnosis. No application code, database schema, or data was modified. No SQL Server
configuration was changed. No index was created. No statistics were updated. Nothing was committed.

---

## 1. Stuck request — identified

Read from `sys.dm_exec_requests` (session-level detail):

| Field | Value |
|---|---|
| session_id | **57** |
| request_id | 0 |
| status | suspended |
| command | SELECT |
| wait_type | **RESOURCE_SEMAPHORE** |
| wait_time | 459,352 ms at last check (kept climbing — the request had been queued since one of this task's earlier retries) |
| wait_resource | (none — RESOURCE_SEMAPHORE waits don't populate this column; the wait is on a memory grant, not a lock) |
| blocking_session_id | 0 (not blocked by another session's lock — this is a pure memory-grant queue wait) |
| cpu_time | 120 ms (negligible — confirms the query never actually started executing; all elapsed time was spent waiting for a memory grant) |
| total_elapsed_time | 459,497 ms (≈ 7.7 minutes at the moment of this specific check; earlier checks across the session's multiple retries showed this same session/pattern recurring for 15+ minutes cumulatively, per the user's report) |
| database_name | SalesRollDB_ZAS |

## 2. Memory grant pressure — `sys.dm_exec_query_memory_grants`

The single row for session 57:

| Field | Value |
|---|---|
| requested_memory_kb | **151,704 KB** (~148 MB) |
| granted_memory_kb | **NULL** — never granted; this is exactly why it was stuck |
| required_memory_kb | **6,144 KB** (~6 MB) — the actual minimum the query needs |
| ideal_memory_kb | **692,328 KB** (~676 MB) — what the optimizer would grant if memory were unconstrained |
| query_cost | 458.3 (estimated optimizer cost — moderately high for what should be a single-`GroupInvoiceNo` lookup) |
| timeout_sec | 11,457 (~3.2 hours) — SQL Server was prepared to let this sit in the memory-grant queue for over three hours before giving up on its own |
| wait_time_ms | 507,656 at that check |

**Who else held large grants?** At the time of inspection, session 57 was the only row in `sys.dm_exec_query_memory_grants` — no other session was holding a competing large grant from *this* database's workload. Memory clerks (`sys.dm_os_memory_clerks`) showed nothing unusual: the largest clerks were SQL Server's own node/lock-manager/buffer-pool structures, all in the tens-of-MB range, not hundreds of MB. This means the semaphore wait was **not** caused by another SQL query hoarding memory — it was caused by **overall Windows physical memory being critically low** (see §6), leaving the SQL Server memory grant semaphore unable to satisfy even a 148 MB request.

`sys.dm_os_sys_memory.available_physical_memory_kb` was observed oscillating between roughly **280 MB and 1,000 MB** free across repeated checks during this diagnosis — on a 16 GB machine. That is the real constraint.

## 3. Exact query

Retrieved via `sys.dm_exec_sql_text(sql_handle)` for session 57 — this is `InvoiceDAL.GroupInvoiceHtmlReportData`, the dedicated data source added for the Group Invoice HTML report (already includes the `OPTION (MAXDOP 1)` hint added earlier in this same task):

```sql
SELECT
    INV.GroupInvoiceNo, CONVERT(DATE, INV.GroupInvoiceDate, 101) AS InvoiceDate,
    ISNULL(CSTMR.CustomerCode, 'N/A') AS CustomerCode, ISNULL(CSTMR.CustomerName, 'N/A') AS CustomerName,
    ISNULL(CSTMR.Address, '') AS Address, ISNULL(CSTMR.Addrees2, '') AS Addrees2,
    ISNULL(CSTMR.TermOfPayment, 'N/A') AS TermOfPayment, ISNULL(MKT.MarketName, 'N/A') AS MarketName,
    ISNULL(EGI.EmpName, 'N/A') AS DeliveryPersonName, ISNULL(EGI.CellNumber, 'N/A') AS DeliveryPersonPhNo,
    INV.Remarks, INV.PONumber, ISNULL(U.UserName, 'N/A') AS PreparedBy,
    INV.InvoiceNo, INV.OrderNo, CONVERT(DATE, ODR.SubmissionDate, 101) AS OrderDate,
    IVD.ProductCode, IVD.ProductName AS Product, PD.Description AS GenericName,
    SUM(IVD.Quantity) AS Quantity, SUM(IVD.BonusQuantity) AS BonusQuantity,
    SUM(IVD.Quantity) + SUM(IVD.BonusQuantity) AS TotalQuantity,
    IVD.UnitPrice, IVD.UnitVatAmount, SUM(IVD.TotalPrice) AS TotalPrice, IVD.TotalPriceVatAmount,
    (IVD.DiscountPercentage + ISNULL(DS.DiscountPercentage, 0)) AS DiscountPercentage,
    SUM(IVD.DiscountAmount + IVD.SpecialAmount) AS DiscountAmount, SUM(IVD.NetAmount) AS NetAmount,
    IVD.PackSize
FROM tblInvoice AS INV
LEFT JOIN tblUser AS U ON INV.UserId = U.UserId
LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId
LEFT JOIN tblInvoiceDetail AS IVD ON INV.InvoiceId = IVD.InvoiceId
LEFT JOIN tblProduct AS PD ON IVD.ProductCode = PD.ProductCode
LEFT JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
LEFT JOIN tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
LEFT JOIN (SELECT CustomerMasterId, ProductId, DiscountPercentage FROM tblProductDiscount
           WHERE IsActive = 1 AND ActionStatus = 'Accepted') AS DS
    ON DS.CustomerMasterId = INV.CustomerMasterId AND DS.ProductId = PD.ProductId
WHERE INV.GroupInvoiceNo = 'INV-GP01-2609020006'
GROUP BY INV.GroupInvoiceNo, CONVERT(DATE, INV.GroupInvoiceDate, 101), CSTMR.CustomerCode, CSTMR.CustomerName,
    CSTMR.Address, CSTMR.Addrees2, CSTMR.TermOfPayment, MKT.MarketName, EGI.EmpName, EGI.CellNumber,
    INV.Remarks, INV.PONumber, U.UserName, INV.InvoiceNo, INV.OrderNo, CONVERT(DATE, ODR.SubmissionDate, 101),
    IVD.ProductCode, IVD.ProductName, PD.Description, IVD.UnitPrice, IVD.UnitVatAmount, IVD.TotalPriceVatAmount,
    IVD.DiscountPercentage, DS.DiscountPercentage, IVD.PackSize
ORDER BY INV.InvoiceNo, IVD.ProductCode
OPTION (MAXDOP 1)
```

- **Database**: `SalesRollDB_ZAS`
- **Execution duration**: never actually executed — 100% of its elapsed time (7.7+ minutes at last check, 15+ minutes cumulative across this task's retries per the user's report) was spent waiting for a memory grant, not running.
- **Requested memory**: 151,704 KB (~148 MB)
- **Granted memory**: none (NULL)
- **Wait duration**: 459–508 seconds at the two points checked, still climbing when cancelled.

## 4. Common causes — checked against this query

| Possible cause | Present? | Notes |
|---|---|---|
| Large JOINs | Yes | 9 joined tables/subqueries, but all `LEFT JOIN` on indexed or small lookup tables (`tblUser`, `tblOrder`, `tblProduct`, `tblMarket`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblCustMaster`, a small filtered subquery on `tblProductDiscount`) — not inherently pathological. |
| Missing index | **Yes — confirmed, see §5** | `tblInvoice.GroupInvoiceNo`, the column in the `WHERE` clause, has no index of any kind. |
| GROUP BY on a large dataset | Partially | The `GROUP BY` list is long (23 columns) but should apply to a tiny row set once `GroupInvoiceNo` is filtered — the *optimizer's estimate* of that row set is the actual problem, not the real row count. |
| ORDER BY on a large dataset | No | `ORDER BY InvoiceNo, ProductCode` operates on the same tiny actual row set; not the driver here. |
| DISTINCT | No | Not used. |
| Hash joins / hash aggregates | Likely, by the optimizer's choice | With no index to drive a nested-loop seek on `GroupInvoiceNo`, the optimizer's cheapest plan for filtering a 765K-row table is a scan feeding a hash match, which reserves memory proportional to its (inflated) row estimate. |
| Large SORT operations | Possible | The `GROUP BY`/`ORDER BY` combination can be satisfied by a sort; same inflated-estimate mechanism applies. |
| `SELECT *` | No | Explicit column list throughout. |
| Implicit conversions | Not identified | `GroupInvoiceNo` is compared to a string literal of the same apparent type; no obvious type mismatch found in this pass. |
| Poor cardinality estimates / stale statistics | **Yes — contributing** | No dedicated statistics object exists for `GroupInvoiceNo` specifically (only auto-created single-column `_WA_Sys_*` stats on various other columns, see §5) — the optimizer has no good density information for this specific predicate and, absent an index, falls back to a full-table-scan-based estimate. |
| Parameter sniffing | Not applicable | This is inline literal SQL (`GroupInvoiceNo = '...'` built via string concatenation), not a parameterized stored procedure/query — no parameter-sniffing mechanism is in play. |
| Cartesian/multiplication joins | No | Every join has an explicit, correct `ON` condition; no unintended cross joins observed. |
| Unnecessary table scans | **Yes — the root mechanism** | Without an index on `GroupInvoiceNo`, `tblInvoice` (765,658 rows / 341 MB) must be scanned to find matching rows, and depending on the join order this can cascade into a scan-driven hash join against `tblInvoiceDetail` (1,264,526 rows / 695 MB) too. |

## 5. Table size / index / statistics findings

| Table | Row count | Size |
|---|---|---|
| tblInvoice | 765,658 | 340.96 MB |
| tblInvoiceDetail | 1,264,526 | 694.96 MB |
| tblCustMaster | 28,249 | 19.85 MB |
| tblProduct | 2,999 | 1.36 MB |

**Indexes on `tblInvoice`** (`sys.indexes` / `sys.index_columns`):

- `PK_tblInvoice` — CLUSTERED on `InvoiceId`
- `UQ_tblInvoice_InvoiceNo` — NONCLUSTERED, unique, on `InvoiceNo`
- `IX_tblInvoice_OrderId` — NONCLUSTERED, includes `PaymentStatus`, `DelivaryInvoiceNo`, `InvoiceId`, keyed on `OrderId`
- `index_DCWiseInvoice` — NONCLUSTERED, keyed on `ComUnitId`, `CustomerMasterId`

**No index exists on `GroupInvoiceNo`** — the exact column every Group Invoice report query (both the pre-existing Crystal queries and the new HTML report's dedicated query) filters on. This was not introduced by this task; it is a pre-existing schema characteristic. Statistics are present (a mix of index-backed stats last updated 2026-01-01, and several `_WA_Sys_*` auto-created single-column stats updated as recently as 2026-06-29), but none of the inspected statistics objects are specifically the `GroupInvoiceNo` column's density/histogram — consistent with there being no index to anchor one, and no evidence a manual `CREATE STATISTICS` was ever run for it. No statistics were updated during this diagnosis, per instructions.

## 6. Relationship to the Group Invoice task

**Answer: A — directly related, but only as this task's own diagnostic/test traffic, not as a defect in the delivered feature.**

The stuck session's SQL text is an exact match for `GroupInvoiceDAL.GroupInvoiceHtmlReportData` filtered on `INV-GP01-2609020006` — the reference invoice from the original PDF used to verify the HTML report rebuild earlier in this task. This was one of this task's own repeated live-verification attempts (the same request re-sent several times while diagnosing why the app was slow to respond), left sitting in SQL Server's memory-grant wait queue.

However, the **root cause is not specific to this query or this task**:
- The missing `GroupInvoiceNo` index equally affects the pre-existing, untouched Crystal queries (`GroupInvoiceMainDataForReport2`, `GroupInvoiceDetailDataForReport2`), which filter on the identical column and would produce the identical oversized-memory-grant risk under the same low-memory conditions.
- While diagnosing, an **entirely unrelated, pre-existing script** — its own comment header reads "Dashboard performance fix — Phase B + Phase 2 (card-group independence)" — was independently observed also stuck on `RESOURCE_SEMAPHORE` (session 71, wait climbing past 150 seconds) at the same time, with no connection to the Group Invoice work at all. This confirms category **C** applies simultaneously to a second, unrelated session: the memory-grant starvation is a **machine-wide condition** (Windows physical memory observed oscillating between ~280 MB and ~1,000 MB free on a 16 GB box, with heavy concurrent load from Visual Studio, multiple browsers, SQL Server, and IIS Express all running on the same machine), not something caused by the Group Invoice query's design alone.

So: this specific stuck session is category A (this task's own retry), but the underlying vulnerability it exposed — a missing index on a large, frequently-filtered column, made acute by system-wide memory pressure — is a pre-existing condition (category B/C in effect) that was simply never surfaced before because the machine had more free memory during the original implementation and audit passes earlier in this engagement.

## 7. Safe action taken

- **Confirmed** session 57 was this task's own test query (exact SQL text match, exact test invoice number) before touching it.
- **Cancelled only session 57** via `KILL 57` — no other session was killed. (Two SQL Server sessions had been killed by the user's own request during earlier troubleshooting in this same task, before this diagnosis was requested; no additional sessions beyond 57 were killed during this diagnosis itself.)
- **Verified** the kill succeeded: `sys.dm_exec_sessions` for session_id 57 now shows a completely different, unrelated internal task (SQL Server's own full-text "FT CRAWL MON" background job), confirming the original connection was torn down and the session_id was recycled — the stuck query no longer exists.
- **Confirmed** session 71 (the unrelated dashboard script also seen waiting on `RESOURCE_SEMAPHORE`) was left untouched, per "do not kill arbitrary sessions."
- **Did not rerun** the same heavy query afterward.

### Recommended lightweight replacement query (diagnosis-only, not a code change)

For any future check of `INV-GP01-2609020006` (or any single `GroupInvoiceNo`) without risking another large memory grant:

```sql
-- Cheap existence/shape check: no joins, no GROUP BY, no ORDER BY, capped row count.
SELECT TOP 5 InvoiceId, InvoiceNo, GroupInvoiceNo, CustomerMasterId, DeliveryManId
FROM tblInvoice WITH (NOLOCK)
WHERE GroupInvoiceNo = 'INV-GP01-2609020006'
OPTION (MAXDOP 1, FAST 5);
```

This avoids the join fan-out and `GROUP BY`/`ORDER BY` that drove the large memory-grant estimate, and `FAST 5` tells the optimizer to prefer a plan that returns the first few rows quickly over one optimized for total throughput. This is offered purely as a lower-risk way to probe the database interactively during future diagnosis — it is **not** a proposed change to `GroupInvoiceHtmlReportData` or any application code, and no such change was made.

**This lightweight query was run as part of this diagnosis and confirms the machine-wide-pressure conclusion**: even this trivial, single-table, no-join, `TOP 5` query took **3 minutes 2 seconds** to return — for a query that should complete in milliseconds under normal conditions. That rules out the join/`GROUP BY` complexity of the original query as the primary cause of the multi-minute delays observed throughout this task; the bottleneck is the shared machine's available physical memory, not this query's shape. (As a side confirmation of data correctness, its result — the four constituent invoices `INV-BD17-2609020015`, `INV-BD76-2609020001`, `INV-BD159-2609020042`, `INV-BD201-2609020001` under `GroupInvoiceNo = INV-GP01-2609020006` — exactly matches the four Reference No values shown in the original Crystal PDF used as the parity reference.)

**Real fix (not applied — flagged for a separate, explicitly-scoped task)**: a non-clustered index on `tblInvoice.GroupInvoiceNo` (optionally including commonly-selected columns) would let every Group Invoice query — old Crystal and new HTML alike — seek directly to the matching handful of rows instead of scanning 765K rows and letting the optimizer guess at a memory grant. This is a schema change and was explicitly out of scope for this diagnosis ("do not create indexes automatically").

## 8. Is application code affected?

**No.** `GroupInvoiceHtmlReportData`'s SQL text, as retrieved directly from `sys.dm_exec_sql_text`, matches the deployed code exactly (including the `OPTION (MAXDOP 1)` hint already added earlier in this task) — there is no bug in the query logic itself. The query was simply waiting in a memory-grant queue caused by machine-wide resource pressure combined with a pre-existing missing index; it was never executing, never returned wrong data, and never corrupted or blocked any other request's data. No further code changes were made as part of this diagnosis.

---

## FINAL STATUS

**QUERY CANCELLED — ROOT CAUSE IDENTIFIED**
