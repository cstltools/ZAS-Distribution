# Transaction Integrity Audit

> Scope: what is visible in the C# application layer only. Per `specs/SPEC-GAPS.md` (`GAP-DB-001`), all 119
> stored-procedure bodies are absent — any `BEGIN TRAN`/`COMMIT`/`ROLLBACK` that might exist *inside* a
> procedure is **UNKNOWN** and is not guessed at anywhere in this document. Everything below is about whether
> the **C# code** wraps its own multiple SP/SQL calls in one transaction.

## 1. Grep sweep — transaction primitives in Library.DAL / Library.BLL

| Pattern | Library.DAL hits | Library.BLL hits |
|---|---|---|
| `TransactionScope` | 0 | 0 |
| `SqlTransaction` | 0 | 0 |
| `BeginTransaction` | 1 (`Library.DAL/MAIN_FUNCTION/DB_Manager.cs:32`) | 0 |
| `.Commit()` | 1 (`DB_Manager.cs:42`) | 0 |
| `.Rollback()` | 1 (`DB_Manager.cs:96`) | 0 |

**CONFIRMED**: `System.Transactions.TransactionScope` is never used anywhere in the two projects. The only
transaction object in the entire data-access layer is the one inside `DB_Manager` (`Library.DAL/MAIN_FUNCTION/DB_Manager.cs`).

### 1.1 `DB_Manager`'s transaction is per-connection, not per-business-operation

```csharp
// Library.DAL/MAIN_FUNCTION/DB_Manager.cs:24-34
internal void CreateConnection(string DataBaseName)
{
    database = new ... SqlDatabase(DB_Connection.GenerateString(DataBaseName));
    dbConnection = database.CreateConnection();
    if (dbConnection.State != ConnectionState.Open)
    {
        dbConnection.Open();
        dbTransaction = dbConnection.BeginTransaction();   // one transaction per CreateConnection() call
    }
}

// Library.DAL/MAIN_FUNCTION/DB_Manager.cs:36-48
internal void CloseConnection()
{
    if (dbConnection.State == ConnectionState.Open)
    {
        if (dbTransaction != null) { dbTransaction.Commit(); dbTransaction.Dispose(); }
        dbConnection.Close();
    }
}
```

`DB_Manager` is `internal class DB_Manager` — a low-level helper, instantiated fresh per DAL call site that
uses it (grep shows no shared/static instance). A transaction created by `CreateConnection()` covers only the
statements issued between that call and the matching `CloseConnection()` — normally a single stored-procedure
call (see `SaveAction`/`UpdateAction`/`DeleteAction`, `DB_Manager.cs:103-161`). **CONFIRMED**: nothing in the
codebase calls `CreateConnection()` once, issues N stored-procedure calls belonging to one business operation,
and then calls `CloseConnection()` once — grep of `SaveAction|UpdateAction|DeleteAction` call sites shows each
is invoked as an independent, self-contained operation.

### 1.2 The invoice header/detail path does not go through `DB_Manager` at all

**CONFIRMED**: `InvoiceDAL.SaveDataForInvoice` / `SaveDataForInvoiceDetails` (the header/detail insert path used
by every invoice-creation page) resolve to `ClsCommonInternalDAL.SaveDataByInsertCommand(string, List<SqlParameter>, string)`
(`Library.DAL/InternalCls/ClsCommonInternalDAL.cs:400-428`), which does:

```csharp
db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
dbCommand = db.GetSqlStringCommand(queryString);
...
db.ExecuteNonQuery(dbCommand);   // no DbTransaction argument passed — auto-commits as a single statement
```

No `DbTransaction` object is created or passed to `ExecuteNonQuery` here. Each insert/update opens its own
`Database`/connection via `DatabaseFactory.CreateDatabase` and auto-commits individually (ADO.NET default
behavior for a command run without an explicit transaction). This is a **second, independent** data-access path
from `DB_Manager`'s — the codebase has two different "the DAL talks to SQL" mechanisms, neither of which spans
more than one statement per call.

## 2. Traced operation: Invoice save (header + details + stock)

Page: `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs` (page spec `specs/sales-order-invoice/pages/PAGE-SAL-010-InvoiceCreationForCustomer.md`).

### 2.1 Actual call sequence (evidence)

```
saveButton_Click(...)                                    InvoiceCreationForCustomer.aspx.cs:428
  if (Validation())
    int invId = SaveInvoice();                            :432, :470-500
      aClsPrimaryKeyFind.PrimaryKeyMax("invoiceId","tblInvoice")   InvoiceBLL.cs:124   [Step A: read next PK]
      InvoiceNoGenerator(...)  -> InvoiceDAL.InvoiceNoCount(...)   InvoiceBLL.cs:126, :437-486  [Step B: read next inv-no]
      aInvoiceDal.SaveDataForInvoice(aInvoice)             InvoiceBLL.cs:129           [Step C: INSERT tblInvoice — own auto-commit]
    SaveInvoiceDetail(invId);                              :433, :439-468
      aInvoiceBll.SaveInvoiceDetails(list, comUnitId)      InvoiceBLL.cs:194-202
        foreach InvoiceDetail in list:
          UpdateDCStock(detail, comUnitId)                 InvoiceBLL.cs:218-321
            aInvoiceDal.BatchWiseProductQty(...)            :221              [Step D: SELECT stock batches]
            loop over batches:
              InvoiceDetailInsertBLL(...)                   :297/301/316     [Step E: INSERT tblInvoiceDetail — own auto-commit]
              aInvoiceDal.UpdateDCStoreQuantity(dcStoreId, restQty)  :303/317 [Step F: UPDATE tblDCStore — own auto-commit]
    Clear(); showMessageBox("Create Invoice");
```

Each of steps C, E (per line item, per batch) and F (per batch) is its own independent
`SaveDataByInsertCommand` / `UpdateDataByUpdateCommand` call — its own connection, its own implicit one-statement
transaction (§1.2). There is **no** enclosing transaction across C…F.

### 2.2 Error handling around the sequence

**CONFIRMED**: `saveButton_Click` (`InvoiceCreationForCustomer.aspx.cs:428-437`) contains **no `try`/`catch`** —
verified by reading the full method body. Nor does `SaveInvoice()` (:470-500) or `SaveInvoiceDetail()` (:439-468).
This matches `GAP-SEC-008`/page-spec §22: an unhandled exception becomes an ASP.NET yellow-screen (stack trace
shown to the user, `debug="true"` in `Web.config`).

Deeper down, `ClsCommonInternalDAL.SaveDataByInsertCommand` (`ClsCommonInternalDAL.cs:400-428`) does
`catch (Exception ex) { throw ex; }` — it re-throws (resetting the stack trace) rather than swallowing. So a
failure at step E or F **will** propagate up and abort `saveButton_Click`, but nothing along the way undoes the
already-committed steps before it.

### 2.3 Partial-save risk — concrete

If step C (header insert) succeeds and then step D/E/F throws for, say, the 2nd of 3 line items (e.g. a
`NullReferenceException` from `ProductInfoDataTable.Rows[0]` when `ProductInfo` returns no rows — a realistic
case, since `GetProduct()` in the same page already handles "no stock or product" as an expected outcome
elsewhere):

- `tblInvoice` **already has a committed header row** (`InvoiceId`, `InvoiceNo`, totals) — this cannot be undone
  by the C# code because there is no transaction object wrapping it and no compensating `DELETE` call exists in
  the exception path (there is no exception path at all — the exception simply propagates to the ASP.NET runtime).
- `tblInvoiceDetail` has 0 or 1 of the 3 line items actually inserted (steps for line items after the failing
  one never run).
- `tblDCStore` reflects stock decrements for whichever batches were processed before the failure, but not the
  rest.
- The user sees a yellow-screen/error page, not `"Create Invoice"` — so they know *something* failed, but the
  header row (with a consumed invoice number and PK) is left behind as an orphaned/incomplete invoice with no
  matching detail lines or with fewer detail lines than the UI showed.
- **HUMAN VERIFICATION REQUIRED**: whether `tblInvoice` has a `Status`/`IsPosted` column that a report or later
  process could use to filter out such incomplete headers is UNKNOWN — no schema is available (`GAP-DB-002`).

This is the same defect independently flagged as `GAP-APP-007` ("Document saves are not transactional") in
`specs/SPEC-GAPS.md`; this document adds the concrete step-by-step trace and file:line evidence.

## 3. Second traced operation: Invoice cancellation (`ErrorInvoiceList`)

`Solution.Web/SInventory_UI/ErrorInvoiceList.aspx.cs:38-66`, `removeButton_Click`:

```csharp
for (int i = 0; i < invoiceGridView.Rows.Count; i++)
{
    ...
    aInvoiceReturnBll.ProformaReturnPosting(invoiceNo.Trim());          // Step 1: posting
    aInvoiceReturnBll.DeleteProforma(invoiceNo.Trim(), Session["LoginName"].ToString());  // Step 2: delete
}
ShowMessageBox("Invoice Canceled Successfully !!");
```

**CONFIRMED**: no `try`/`catch` anywhere in this method (full method body read). The loop processes N selected
invoices; each invoice does a "post" call followed by a "delete" call, with **no** transaction spanning even the
two steps for a single invoice, let alone across the N invoices in the loop. If invoice 3 of 5 throws inside
`ProformaReturnPosting` or `DeleteProforma`:
- Invoices 1–2 are fully posted-and-deleted (committed).
- Invoice 3 may be posted but not deleted (or neither, depending on which call threw) — an inconsistent
  intermediate state with no automatic reconciliation.
- Invoices 4–5 are untouched.
- The success message is never shown (the exception aborts the page), so the user has no way to tell from the UI
  which of the 5 actually got cancelled — they must re-open the list and compare.

## 4. Bugs

| ID | Title | Severity | Confidence |
|---|---|---|---|
| `BUG-TXN-001` | Invoice save (header + line-items + stock) has no C#-level transaction; a mid-sequence exception leaves a committed `tblInvoice` header with partial/missing `tblInvoiceDetail` rows and partial `tblDCStore` updates, and no code path attempts to undo the header. Evidence: `InvoiceCreationForCustomer.aspx.cs:428-500`, `InvoiceBLL.cs:122-129,194-321`, `ClsCommonInternalDAL.cs:400-428`. | High | CONFIRMED (code-level; whether it manifests depends on runtime failure conditions not observable statically) |
| `BUG-TXN-002` | `saveButton_Click` and every method it calls on the main invoice-creation page have zero `try`/`catch` blocks; any exception (numeric parse failure, missing dictionary row, etc.) surfaces as an unhandled exception mid-save with no rollback and no clean user message. Evidence: `InvoiceCreationForCustomer.aspx.cs:428-500` (full method bodies read, no `try` keyword present). | High | CONFIRMED |
| `BUG-TXN-003` | Bulk invoice cancellation (`ErrorInvoiceList.removeButton_Click`) posts+deletes each selected invoice with two unwrapped calls inside an untried loop; a failure partway leaves a mix of cancelled/not-cancelled invoices with no per-row success/failure feedback to the user. Evidence: `ErrorInvoiceList.aspx.cs:38-66`. | High | CONFIRMED |
| `BUG-TXN-004` | `DB_Manager.ExecuteNonQueryAction` (the second, stored-procedure-oriented data path) catches all exceptions, rolls back its own single-statement transaction, closes the connection and returns `0` — indistinguishable from a legitimate "0 rows affected" result. Any caller using this path cannot tell a DB error from a no-op. Evidence: `DB_Manager.cs:90-100` (matches `GAP-APP-008`). | High | CONFIRMED |
| `BUG-TXN-005` | No `TransactionScope`/`SqlTransaction` spans multiple DAL calls anywhere in `Library.BLL` or `Library.DAL` (0 hits for `TransactionScope`, 0 for ad-hoc multi-call `SqlTransaction` outside `DB_Manager`'s single-connection scope). All true cross-table atomicity, if it exists, can only live inside the 119 stored procedures this repository cannot read (`GAP-DB-001`) — application code provides none of its own. | Medium (structural finding, not a single defect) | CONFIRMED |

## 5. What remains genuinely unknown

- Whether any of the 119 stored procedures (e.g. a hypothetical `sp_Sales_SaveInvoice`) wrap their *own* internal
  work in `BEGIN TRAN`/`COMMIT`/`ROLLBACK` is **UNKNOWN** — no procedure body is in the repository (`GAP-DB-001`).
  This audit only shows that **the C# call sequence above does not use any procedure that bundles header+detail+stock
  into one call** — each is a separate named DAL method hitting a separate table via a separate ad-hoc SQL statement,
  not a single `EXEC sp_Sales_SaveInvoice` — so even if individual procedures are transactional internally, nothing
  observed here calls one covering the whole operation.
- Whether SQL Server's default `READ COMMITTED` isolation plus per-statement auto-commit has ever actually produced
  an orphaned header row in production data is **UNKNOWN** — this requires querying the live database, which is out
  of scope (audit-only, no SQL execution performed). **HUMAN VERIFICATION REQUIRED.**
