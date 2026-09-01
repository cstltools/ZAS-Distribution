# Architecture — how a click reaches the database

Everything in this document is verified against the named source files. Read it once; every page spec assumes it.

## The layers

```
Browser
  │  full postback, or partial postback inside an UpdatePanel
  ▼
Solution.Web/<folder>/<Page>.aspx           markup: server controls in <table> layout
Solution.Web/<folder>/<Page>.aspx.cs        code-behind: event handlers read control .Text directly
  │
  ├──► Library.DAO/SInventory_Entities/*.cs    plain property bags (no attributes, no validation, no ORM)
  │
  ▼
Library.BLL/SInventory_BLL/<X>BLL.cs        mostly a 1-line pass-through; real logic in a handful of classes
  ▼
Library.DAL/SInventory_DAL/<X>DAL.cs        builds a SQL string by concatenation, or a List<SqlParameter>
  ▼
Library.DAL/InternalCls/ClsCommonInternalDAL.cs     ─┐
Library.DAL/MAIN_FUNCTION/DB_Manager.cs             ─┴─ Enterprise Library Data Access Block
  ▼
SQL Server: SalesrollDB_ZAS  (+ ZAS_ACCDB, CSTL_ERPDB, CSTL_ACCDB, ZASHRIS by three-part name)
```

There is **no service layer, no repository interface, no dependency injection, no unit of work and no ORM.**
Classes are instantiated with `new` at field level in each page.

## The BLL is thinner than it looks

`Library.BLL` has **74** classes; `Library.DAL` has **130**. Many pages skip the BLL entirely and hold a DAL
instance directly — for example `Solution.Web/SInventory_UI/BatchWiseCollection.aspx.cs` uses
`BatchWiseCollectionReportDal`. Where a BLL class does exist, the typical method is:

```csharp
public DataTable CustomerMaster(string CustomerMasterId)
{
    return aInvoiceDal.LoadCustomerMaster(CustomerMasterId);   // InvoiceBLL.cs
}
```

The exceptions — classes that hold real logic — are worth knowing:

| Class | Logic it owns |
|---|---|
| `InvoiceBLL` | FEFO batch allocation (`UpdateDCStock`), line-level discount/VAT/net computation (`InvoiceDetailInsertBLL`), invoice-number generation |
| `CustPaymentBLL` | Payment application across invoices |
| `ChalanBLL`, `RequisitionBLL` | Weighted-average triggering after stock movement |
| `PanalBLL` | Login, menu, version |

## Two ways SQL reaches the server

### 1. Inline SQL through one generic procedure — the majority

`ClsCommonInternalDAL` does **not** send the SQL text as a command. It passes it as a *parameter* to a stored
procedure that executes it:

```csharp
// Library.DAL/InternalCls/ClsCommonInternalDAL.cs
dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
dbCommand.CommandTimeout = 360000000;
db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
dataDDL = db.ExecuteDataSet(dbCommand).Tables[0];
```

So a query written as

```csharp
string query = "select * from tblProduct where ProductCode = '" + aProduct.ProductCode + "'";
```

arrives at SQL Server as `EXEC ExecuteAllSqlQueryByStoreProcedure @Query = '<that whole string>'`.
Consequences, all CONFIRMED:

- **Every inline query is SQL-injectable.** User input is concatenated, never parameterised, and the wrapper
  procedure executes whatever text it is given. See [SPEC-GAPS.md](../SPEC-GAPS.md) → `GAP-SEC-005`.
- Query plans cannot be reused; the plan cache sees one distinct statement per input value.
- `CommandTimeout = 360000000` (≈11 years) means a runaway query never times out.
- Static analysis of "which table does this screen touch" is only possible by parsing strings — which is what
  this specification did.

### 2. Named stored procedures with `SqlParameter` — the minority, used for postings

```csharp
// Library.DAL/SInventory_DAL/InvoiceDAL.cs
public bool PostDeliveryConfFullJournal(int invoiceId)
{
    var p = new List<SqlParameter> { new SqlParameter("@InvoiceId", invoiceId) };
    return aCommonInternalDal.UpdateAction("sp_Sales_ExecuteDeliveryPipeline", p);
}
```

These are properly parameterised. They are used for the operations that matter most — stock posting, accounting
posting, delivery pipeline, weighted average, payment posting — and their bodies are **not in the repository**.

## Transactions

Two different, inconsistent stories:

**`DB_Manager`** (used by a minority of DAL methods) opens a connection *and immediately begins a transaction*:

```csharp
dbConnection.Open();
dbTransaction = dbConnection.BeginTransaction();
```

and commits in `CloseConnection()`. On error, `ExecuteNonQueryAction` rolls back, closes the connection, and
**swallows the exception**, returning `0`:

```csharp
catch (Exception ex)
{
    dbCommand.Parameters.Clear();
    if (dbTransaction != null) dbTransaction.Rollback();
    dbConnection.Close();
}
return pk;   // 0
```

The caller cannot distinguish "failed" from "affected no rows".

**`ClsCommonInternalDAL`** (used by the majority) uses `DatabaseFactory.CreateDatabase(...)` with **no
transaction at all**. Each statement autocommits.

This matters because multi-statement business operations are written as several separate DAL calls from the
code-behind. `InvoiceCreationForCustomer.aspx.cs::saveButton_Click` is the clearest example:

```csharp
int invId = SaveInvoice();          // INSERT tblInvoice        -- committed
SaveInvoiceDetail(invId);           // N × INSERT tblInvoiceDetail + UPDATE tblDCStore  -- each committed
Clear();
showMessageBox("Create Invoice");
```

If the detail loop fails halfway, the header and the already-written detail rows stay. There is no compensating
action. This is `BR-SYS-004` in [business-rules.md](business-rules.md).

## Primary keys are computed in the application

`Library.DAL/InternalCls/ClsPrimaryKeyFind.cs`:

```csharp
string query = "SELECT (isnull(MAX(" + columnName + "),0)+1) as PKMaxNo FROM " + tableName;
```

The next key is `MAX(id)+1`, read on a separate connection, outside any transaction, then written back. Under
concurrency two users get the same key. A third overload uses `IDENT_CURRENT(tableName)`, which has the same
problem plus reading another session's identity. This is `BR-SYS-003`.

## Document numbering

| Document | Generator | Format |
|---|---|---|
| Invoice | `InvoiceBLL.InvoiceNoGenerator(comUnitId, comUnitCode)` | `INV-<ComUnitCode>-<9 digits, zero-padded>` |
| Return invoice | `InvoiceBLL.ReturnInvoiceNoGenerator` | `<prefix>-<ComUnitCode>-<8 digits>` |
| DC picking | `InvoiceDAL.DcPickingNoCount` | count-based |

The invoice counter is derived, not stored:

```sql
SELECT CASE WHEN ComUnitId > 99
            THEN ISNULL(MAX(CAST(SUBSTRING(InvoiceNo,11,12) AS INT)),0)+1
            ELSE ISNULL(MAX(CAST(SUBSTRING(InvoiceNo,10,11) AS INT)),0)+1 END
FROM dbo.tblInvoice WHERE ComUnitId = @unit AND Remarks != 'Due' GROUP BY ComUnitId
```

Two facts follow. The substring offset depends on whether the unit id is 2 or 3 digits, so a unit crossing 99
changes the parse position. And rows whose `Remarks = 'Due'` are excluded from the maximum, so such rows must
never carry a number in the normal sequence.

## Session as the parameter bus

Report parameters are not passed as query strings in most cases. The parameter screen writes them to `Session`
and opens the viewer with `window.open`:

```csharp
// InvoiceCreationForCustomer.aspx.cs
string url = "../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(invTextBox.Text.Trim());
ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW",
    "window.open('" + url + "', null, 'height=700,width=950,...')", true);
```

Session keys used across the application include `UserId`, `LoginName`, `UserType`, `IsHead`, `ComUnitId`,
`CentralWareHouse`, `UserTime`, plus per-report keys such as `FromDate`, `ToDate`, `PRAM`, `parameter`,
`batchId`, `paydetailId`. Because session is `InProc` with a ~16-day timeout, two report windows opened from two
tabs share and overwrite the same keys.

## Error handling

| Level | Behaviour |
|---|---|
| `Global.asax` `Application_Error` | empty — no global handler |
| `Web.config` `<customErrors>` | not configured; `debug="true"` — full stack traces are shown to the user |
| `DB_Manager` | swallows and returns 0 (see above) |
| `ClsCommonInternalDAL` | `throw ex;` — rethrows but **resets the stack trace** |
| Page | ad-hoc; most save handlers do not wrap in try/catch |
| User feedback | `ScriptManager.RegisterStartupScript(..., "alert('...');", true)`, usually via a local `showMessageBox` |

## Build and deploy

`Solution.Web` is a **Web Site** project (`Project("{E24C65DC-...}")`), not a Web Application, so `.aspx.cs`
files compile at run time. `PrecompiledWeb/Solution.Web` is the output of
`aspnet_compiler` (`Debug.AspNetCompiler.TargetPath`) and is checked in — it is a stale copy of the site and
should not be read as source.
