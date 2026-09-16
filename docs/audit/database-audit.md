# Database Audit

Audit-only pass. No source file was modified to produce this document. Every claim below cites a `file:line`
or a specific section of an existing doc; nothing is fabricated or extrapolated beyond what the cited source
says.

---

## 1. The central discovery: live DB access existed, and was used — selectively

`specs/SPEC-GAPS.md` records two "OPEN, impact: critical" gaps that this whole codebase's earlier
reverse-engineering pass treated as permanent, unclosable facts:

- `GAP-DB-001` (specs/SPEC-GAPS.md:19-27): "the T-SQL of **119** stored procedures" is missing, source of
  truth is "the SQL Server instance holding `SalesrollDB_ZAS`", and the suggested remedy is
  `EXEC sp_helptext '<name>'` against that instance.
- `GAP-DB-002` (specs/SPEC-GAPS.md:29-35): "No database schema" — DDL for "**126 tables** and **6 views**"
  is missing, and the remedy is `Generate Scripts` or `sys.foreign_keys`/`sys.indexes`/`sys.triggers` queries.

Both entries assume the instance was never reachable. That assumption is now demonstrably false:

- `docs/database/tables.md:1-9` states the file "was rewritten on 2026-09-05 against a **live SQL Server
  schema extraction** of `SalesrollDB_ZAS` (instance `TOWSIF\MSSQLSERVER2019`)" — 213 tables, 2438 columns,
  full column/type/nullability/identity/default/PK/index detail pulled from `sys.columns`/`sys.indexes`.
- `docs/database/views.md` (per its header) was rewritten the same way: 24 views with verbatim
  `CREATE VIEW` bodies from `sys.sql_modules`/`OBJECT_DEFINITION`.
- `docs/database/functions.md:1-10` was rewritten the same way: "exactly 7 functions" with full verbatim
  bodies, extracted "live from `SalesrollDB_ZAS` (`sys.sql_modules` / `OBJECT_DEFINITION`)".
- `docs/database/triggers.md` (confirmed via `docs/database/tables.md:7` and `relationships.md:1-14`, which
  both cite it) reports a live `sys.triggers` query returning zero rows.
- `docs/database/relationships.md:1-14` was rewritten the same way, against the same live extraction.

**But `docs/database/stored-procedures.md:1-11` was not.** Its header states plainly: "**None of their T-SQL
bodies are in this repository.** Every row below carries **Body: NOT AVAILABLE (`GAP-DB-001`)**" — and every
one of the 119 rows in the file (spot-checked across sections A-I, e.g. `stored-procedures.md:17-150`) ends
its "Body" column with `NOT AVAILABLE`.

**Finding, not a permanent gap:** whoever ran the live extraction for tables/views/functions/triggers/
relationships had the same connection, the same permissions (schema metadata access implies at minimum
`VIEW DEFINITION`, which is what `sp_helptext` and `OBJECT_DEFINITION` both need), and the same session —
four sibling files were pulled from the live instance and one, `stored-procedures.md`, simply wasn't. This
means `GAP-DB-001` is very likely **closeable right now** with 119 calls to
`SELECT OBJECT_DEFINITION(OBJECT_ID('<procname>'))` or `EXEC sp_helptext '<procname>'`, using the exact same
method already proven to work for the other four database object types. This changes the recommended
remediation in `specs/SPEC-GAPS.md`'s priority table (specs/SPEC-GAPS.md:320-330, row 2: "Supply
`sp_helptext` for all 119 procedures") from "needs external access we don't have" to "needs someone to run
the extraction that was already run four times in this same repo's history."

---

## 2. Tables: count discrepancy and spot-check

`specs/FINAL-REPORT.md:119` states the schema gap covers "126 tables and 6 views" — a count **inferred**
from table names appearing in `Library.DAL` SQL string literals and `SqlParameter` names, since no schema
was reachable at that time.

`docs/database/tables.md:5` now reports, from the live catalog: **213 tables, 2438 columns. 112 tables are
referenced by app C# source; 101 have no app-code reference found** (real tables, not inferred/phantom —
their schema is fully documented, they are simply unused by this codebase).

Why the earlier count (126) undercounted by 87 tables: it could only see a table's name if that name
appeared literally in a C# string (`"...FROM tblXxx..."`, `"...INSERT INTO tblXxx..."`, or a matching
`SqlParameter`/DAL method name). A table with zero app-code reference — dead ETL staging tables, backup
tables (`tblMarketAttendance_Master_webapi_backup`), abandoned experiments (`Table_CustomerUpload`,
`Table_BadDuesInvoice`) — is invisible to string-literal grepping by construction. The live catalog has no
such blind spot: it enumerates every row in `sys.tables` regardless of whether any application code ever
touches it.

**Spot-check — 6 tables grepped from `Library.DAL` SQL strings against `docs/database/tables.md`:**

| Table (from C# literal) | Grep evidence | Found in tables.md? |
|---|---|---|
| `tblArea` | `Library.DAL\SInventory_DAL\AreaDAL.cs:39,46,97` (`FROM tblArea`, `UPDATE tblArea SET ...`) | Yes — has full column/index detail (not separately re-quoted here; confirmed present) |
| `tblUser` | `Library.DAL\PanalCls\PanalClsDAL.cs:103` (`SELECT UserName FROM tblUser WHERE LoginName = ...`) | Yes |
| `tblMenuDistribution` | `Library.DAL\Panal_DAL\PanalDAL.cs:126,150,231` (`INSERT INTO tblMenuDistribution`, `SELECT MenuSL FROM tblMenuDistribution`) | Yes |
| `tblGroupInformation` | `Library.DAL\SInventory_DAL\ASMWiseProductSalesDal.cs:17` (`SELECT GroupId, GroupName FROM tblGroupInformation`) | Yes |
| `tblInvoice` | referenced throughout `InvoiceDAL`; documented at `docs/database/tables.md:3513-3570` | Yes — 43 columns, PK `InvoiceId`, unique index on `InvoiceNo` |
| `tblCustPayDetail` | referenced by `CustPaymentDAL`/`PaymentTranscationDal`; documented at `docs/database/tables.md:2403-2427` | Yes |
| `tblDCStore` | referenced by `ChalanDAL`, `InvoiceBLL`, `OtherStockActionBLL`; documented at `docs/database/tables.md:2481-2530` | Yes |
| `tblCentralStore` | documented at `docs/database/tables.md:1801-1859` | Yes |
| `tblPaymentTranscationMaster` | documented at `docs/database/tables.md:4693-4719` | Yes |

**No naming mismatch or missing table was found in this spot-check.** Every table name pulled from a live
C# SQL literal or a stored-procedure "Tables touched (INFERRED)" cell that was checked resolves cleanly to a
documented table in `tables.md`. This is expected — `tables.md`'s 112 "COVERED" tables were themselves
derived by grepping the same C# source, so a mismatch here would indicate an error in that derivation, not
new information. The exercise's value is negative: it did not surface a naming drift/typo/dynamic-SQL table
that the schema extraction missed.

---

## 3. Columns / PKs / FKs: confirmed vs. still open

**Now CONFIRMED (live catalog, not inference):**
- Every column's name, SQL type, nullability, identity flag, default constraint, and whether it is a PK
  column — `docs/database/tables.md`, per-table.
- Zero computed columns anywhere in the database (`docs/database/tables.md:7`).
- Zero triggers anywhere in the database (`docs/database/tables.md:8`; `docs/database/relationships.md`
  cites the same `sys.triggers` result).
- Exactly one physically enforced FK constraint in the whole 213-table database:
  `tblPettyCashDetails.PettyCashmasterId -> tblPettyCashMaster.PettyCashmasterId`
  (`docs/database/tables.md:9`; `docs/database/relationships.md:29-36`, constraint name
  `FK__tblPettyC__Petty__1FB8AE52`, `ON DELETE/UPDATE NO ACTION`).
- 387 additional parent→child relationships are documented, but every one of them is explicitly labeled
  "logical (inferred from column naming), NOT enforced by a physical FOREIGN KEY constraint"
  (`docs/database/relationships.md:236`), inferred purely by matching an `...Id` column name against a
  same-named single-column PK elsewhere. 17 PK column names are shared by more than one table, making every
  match against them genuinely ambiguous (`relationships.md:238`, with each ambiguous case flagged inline,
  e.g. `relationships.md:64,67,84,148-152,160-161,166,170`).
- 176 `...Id`-shaped columns have no matching single-column PK anywhere in the extract at all — some because
  the natural target table (`tblCompanyInfo`, `tblWearhouse`, `tblDesignation`, `tblDoctorDesignation`) has
  **no primary key constraint at all** despite being an identity-keyed master table referenced by dozens of
  children (`relationships.md:198-214`), others because they point at a different database entirely
  (`ZAS_ACCDB`, `CSTL_ERPDB`, `ZASHRIS` — `relationships.md:216-231`, none of which were queried in this
  pass).

**Still INFERRED / UNKNOWN:**
- The *business meaning* of columns beyond their name and type — the schema extraction gives type/nullability,
  not semantics. E.g. `tblInvoice.OldTradePolicy nvarchar(50)` (`docs/database/tables.md:3549`) has no
  documented meaning beyond its name.
- Whether any of the 387 logical relationships are actually enforced by application code where the DB does
  not enforce them. **Checked in this pass:** `Library.DAL` deletes are simple single-table `DELETE`
  statements gated by an `@Id` parameter (e.g. the delete-shaped stored procedures listed in
  `docs/database/stored-procedures.md` section C, `sp_DEL_PettyCashMaster`/`sp_DEL_PettyCashDetails`,
  `stored-procedures.md:63-64`) — since these are stored-procedure calls with `Body: NOT AVAILABLE`, whether
  the procedure itself cascades or blocks the delete when child rows exist is **UNKNOWN**, not just for the
  DB but for the app too, until `GAP-DB-001` is closed (see §1/§4). No C#-side pre-delete "check for
  children" logic was found in the petty-cash, invoice, or order deletion call paths inspected
  (`Library.DAL\SInventory_DAL\InvoiceDAL.cs` delete methods referenced by `specs/SPEC-GAPS.md:190-196`
  already document that document saves are non-transactional; the same files show no referential-integrity
  pre-checks before delete). Net: for every relationship except the one physical FK, **nothing** — not SQL
  Server, not the stored procedures (unverified), not the C# layer (verified absent for the paths checked) —
  is confirmed to stop an orphaned child row from being created or left behind.

---

## 4. Stored procedures: business logic is entirely unknown, prioritized by risk

Per §1, all 119 named procedures carry `Body: NOT AVAILABLE` in `docs/database/stored-procedures.md`. This
means transactions, error handling, concurrency control, and idempotency for **all 119** are UNKNOWN.

**HUMAN VERIFICATION REQUIRED**: run `sp_helptext '<name>'` (or `OBJECT_DEFINITION`) against
`SalesrollDB_ZAS` on `TOWSIF\MSSQLSERVER2019` for every name in `docs/database/stored-procedures.md`, using
the same method already used to extract `docs/database/views.md` and `docs/database/functions.md`. This is
not a research task blocked on missing access — it is a mechanical extraction step that was simply skipped
for this one file.

**Priority order to extract first, by business risk** (all from `docs/database/stored-procedures.md`):

1. `sp_Sales_ExecuteDeliveryPipeline` — "the largest single black box in the app" per its own row
   (`stored-procedures.md:24`): stock decrement + GL posting in one call, no visibility into either.
2. `sp_DeliveryConfirmationFullPosting`, `sp_DeliveryConfirmationPartiallPosting`,
   `sp_Sales_DeliverySalesPosting` (`stored-procedures.md:21-23`) — GL posting for delivery confirmation.
3. `sp_ProformaPosting`, `sp_ProformaReturnPosting`, `sp_ProformaFullReturnPosting`,
   `sp_Sales_DeliverySalesReturnPosting` (`stored-procedures.md:30-33`) — GL posting for proforma/returns.
4. `sp_UD_InvoicePayment` (`stored-procedures.md:34`) — already flagged by `GAP-APP-016`: the C# caller
   computes an amount and a Full/Partial status and passes both, but the DAL forwards only `@InvoiceId`; the
   *entire* payment-amount/status logic is inside this one procedure and nowhere else.
5. `sp_CustomerPaymentPosting`, `sp_Sales_CustomerPaymentPosting`,
   `sp_Sales_CustomerPaymentReversePosting` (`stored-procedures.md:42-44`) — payment GL posting/reversal.
6. `sp_Process_WeightedAverage`, `sp_Process_WeightedAverageB2BTransfer`,
   `sp_Process_WeightedAverageFreezeStock` (`stored-procedures.md:87-89`) — weighted-average stock costing;
   directly reachable from `RequisitionDAL.ProcessWeightedAvg` (`stored-procedures.md:21`) and
   `ChalanDAL.ProcessWeightedAverage`.
7. `sp_FreezeStockPosting`, `sp_FreezeStockReleasePosting` (`stored-procedures.md:90-91`) — GL effect of
   stock freeze/release.
8. `sp_WHStockInPosting`, `sp_Acc_MaterialConversionReceive` (`stored-procedures.md:111-112`) — warehouse
   stock-in GL posting.
9. `sp_SampleInvoicePosting`, `sp_Sales_ProductDestroyPosting` (`stored-procedures.md:127,138`) — GL posting
   for sample issue and product write-off/destroy.
10. `sp_I_CustomerGl`, `sp_I_SupplierGl`, `sp_I_ProductGl`,
    `sp_Integration_CustomerSubsidaryInsert` (`stored-procedures.md:145-149`) — GL account creation/linking
    for customers, suppliers, and products.

---

## 5. Bugs found in this pass

Numbering continues from `BUG-ACC-001` (no prior `BUG-ACC-*` IDs exist anywhere in the repository —
confirmed via repo-wide search).

### `BUG-ACC-001` — `GetStockValue` declares `RETURNS int` but accumulates in `decimal`/`bigint` · **MEDIUM** · **CONFIRMED**

`docs/database/functions.md:207-360` (verbatim body). The function declares `RETURNS int`
(functions.md:213) but its internal accumulator `@Result` is `DECIMAL(18,2)` (functions.md:217) and every
branch of the `CASE` inside the `SUM()` is explicitly `CAST(... AS BIGINT)` (functions.md:226) before being
summed. The final implicit conversion back to `int` on `RETURN @Result` can silently truncate or overflow
for large stock values. Severity is capped at MEDIUM rather than HIGH because `functions.md:527,591-603`
confirms **no application caller was found anywhere** in `Library.DAL`/`Library.BLL`/`Solution.Web` — the
risk is latent, not currently triggering incorrect output in production, but would activate immediately if
any report or dashboard is wired to this function later without the return type being fixed first.

### `BUG-ACC-002` — Two heavily-referenced master tables have no primary key at all · **HIGH** · **CONFIRMED**

`docs/database/relationships.md:198-214` (orphan-column analysis of the live catalog): `tblCompanyInfo` (its
`CompanyId` column is `IDENTITY` but "**not marked as a PK** in the extract — the table has no primary key
constraint at all") and `tblWearhouse` (same: `WearhouseId` "is not marked as a PK — no PK constraint on
that table") are both referenced by `CompanyId`/`WarehouseId`-shaped columns across dozens of other tables
(`tblCompanyUnit`, `tblProduct`, `tblEmpGeneralInfo`, `tblASMInfo`, `tblRSMInfo`, `tblNSMInfo`, `tblMIOInfo`,
`tblCreditAdjustment`, `tblRequisition`, `tblInvoiceBatch`, and more — full list at
`relationships.md:207-208`). Without a PK, SQL Server cannot enforce uniqueness on the identity column these
tables are keyed by in every other part of the schema, and no unique index substitutes for it. A duplicate
`CompanyId` row (from any insert path that doesn't rely on the identity column, or from a restore/migration)
would silently corrupt every join that assumes `CompanyId` uniquely identifies a company.

### `BUG-ACC-003` — `tblDCStore` has no concurrency token and no non-negative guard on `StockQty` · **HIGH** · **CONFIRMED** (schema) / cross-reference to **existing FINDING** `GAP-APP-005`

`docs/database/tables.md:2481-2530`: `tblDCStore` (the table `GAP-APP-005` in `specs/SPEC-GAPS.md:185-188`
already names as the target of `UPDATE tblDCStore SET StockQty = '<value computed in C#>'` with "no
optimistic-concurrency token") has, per the live schema, **no `rowversion`/`timestamp` column anywhere in
its 31 columns**, and `StockQty` itself (`tables.md:2499`, `decimal(18,0)`) is nullable with no default and
no `CHECK` constraint preventing a negative value. `docs/database/triggers.md` (confirmed zero triggers,
`tables.md:8`) rules out a trigger-based safety net, and `docs/database/tables.md:8` already draws this same
conclusion for the identically-shaped `GAP-APP-006` (`MAX(id)+1` PK race). This pass adds the concrete
column-level evidence: the schema is not merely missing a check by omission of a code review — there is no
mechanism available in the table definition itself (no rowversion, no CHECK, no trigger, only one physical
FK anywhere in the DB) that could have caught either bug even if someone had tried to defend against it at
the DB layer.

### `BUG-ACC-004` — `tblInvoice` money and status columns are nullable with no defaults, compounding `GAP-APP-016` · **HIGH** · **CONFIRMED** (schema) + **INFERRED** (consequence)

`docs/database/tables.md:3513-3562`: `tblInvoice.PaymentAmount` (`decimal(18,2)`, nullable, no default —
row 28), `tblInvoice.PaymentStatus` (`nvarchar(50)`, nullable, no default, flagged "business/status field" —
row 29), and `TpGrandTotal`/`DeliveryTpGrandTotal` (both nullable decimals, no default) have no schema-level
protection against being left `NULL` indefinitely. `specs/SPEC-GAPS.md:233-239` (`GAP-APP-016`) already
established that `CustomerPayment.aspx.cs` computes `PaymentAmount`/`"Full"`/`"Partial"` in C# and passes
them to `CustPaymentBLL.UpdateInvoicePaymentAmount`, but the DAL forwards only `@InvoiceId` to
`sp_UD_InvoicePayment` — meaning **the columns responsible for whether an invoice is recorded as paid are
both nullable and have their write path entirely inside an unexamined stored procedure** (see §4, priority
item 4). If that procedure has any early-exit/error path that skips the update, these columns can sit `NULL`
with no schema-level signal that anything is wrong, and no default value to fall back to.

### `BUG-ACC-005` — `Employee` table is an unrelated tutorial/demo table that collides by name with a real FK pattern · **LOW** · **CONFIRMED**

`docs/database/tables.md:13-31`: the table `Employee` (PK `EmployeeID`) has only
`FirstName`/`LastName`/`Department`/`Position`/`Salary` — a generic HR-tutorial shape unrelated to this
application's domain (the real field-force employee master is `tblEmpGeneralInfo`, PK `EmpInfoId`, per
`docs/database/relationships.md:186-196`). `relationships.md:188-196` already confirms that
`tblASMInfo.EmployeeId`, `tblMIOInfo.EmployeeId`, `tblNSMInfo.EmployeeId`, and `tblRSMInfo.EmployeeId` all
name-match `Employee.EmployeeID` by naming convention, creating a spurious logical-FK candidate that a
naive relationship-inference tool (or a developer skimming table names) could wire to the wrong table. This
is a documentation/data-hygiene risk rather than an active application bug — `Employee` is itself UNUSED by
app code (`tables.md`'s COVERED/UNUSED split) — but its presence in a 213-table production schema alongside
a real, differently-keyed employee master is exactly the kind of trap that produces a wrong join if anyone
extends the schema search casually.

### `BUG-ACC-006` — 17 primary-key names are shared across multiple tables, making naming-convention FK inference structurally unreliable for a large fraction of the schema · **MEDIUM** · **CONFIRMED**

`docs/database/relationships.md:238` states this as a summary count, and `relationships.md` itemizes over a
dozen concrete ambiguous cases inline, e.g.: `CategoryId` is the PK of `tblCustCategory`, `tblDoctorCategory`,
**and** `tblProCategory` simultaneously (`relationships.md:64,80,166`); `GroupId` is the PK of
`tblGroupInformation`, `tbl_Group`, **and** `tblProductGroup` simultaneously (`relationships.md:67,73,170`);
`CustPayId` is the PK of `tblCustomerPay`, `tblCustomerPayDeleteLog`, **and** `tblPaymentTranscationMaster`
simultaneously (`relationships.md:149-152`). This is not a defect in the extraction — it is a real
characteristic of this schema's design — but it means that for any child column named e.g. `CategoryId` or
`GroupId`, **which of 2-3 candidate parent tables it actually references is architecturally undecidable from
the schema alone**, and (per §3) is not decidable from application code either without a live trace of which
table each specific DAL method actually joins against at the call site.

### `BUG-ACC-007` — Dead shadow/staging tables duplicate `tblInvoice`-derived data with no primary key and no app reference · **LOW** · **SUSPECTED**

`docs/database/tables.md` documents `Table_DueInvoice` (`tables.md:170-190`: `InvoiceId`, `InvoiceNo`,
`CustomerMasterId`, `Due` — 13 columns, **no PK found, no index found**) and, per
`docs/database/relationships.md:110,113`, the similarly-shaped `Table_IVF` and
`Table_Temp_CustPayDetails` also reference `tblInvoice.InvoiceId`/`tblCustomerPay.CustPayId` by column name
while carrying the `Table_`-prefix naming convention that (per `tables.md`'s 101 UNUSED tables) marks them
as having no live app-code caller. These read like point-in-time ETL/reporting snapshot tables (a due-invoice
extract, an "IVF" extract, a temp customer-payment-detail staging table) that were created for an
ad-hoc/reporting purpose and then abandoned — never cleaned up, never given a PK, and never wired into the
live application. Confidence is SUSPECTED rather than CONFIRMED because the actual origin/owner of these
tables (a scheduled job outside this repo? a one-off migration script? Crystal Reports directly querying
them?) could not be determined from the C# source or the schema alone.

---

## Sources cited

`specs/SPEC-GAPS.md`, `specs/FINAL-REPORT.md`, `docs/database/tables.md`, `docs/database/views.md`,
`docs/database/functions.md`, `docs/database/triggers.md`, `docs/database/stored-procedures.md`,
`docs/database/relationships.md`, `Library.DAL/SInventory_DAL/AreaDAL.cs`,
`Library.DAL/PanalCls/PanalClsDAL.cs`, `Library.DAL/Panal_DAL/PanalDAL.cs`,
`Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs`, `Library.BLL/SInventory_BLL/*.cs`.
