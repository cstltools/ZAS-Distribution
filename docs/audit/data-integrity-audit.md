# Data Integrity Audit

> Scope: risks visible in the C# application layer. This document does **not** claim any constraint violation
> or duplicate row actually exists in the live database — no SQL was run against it (audit-only). Every risk
> below is framed as "the code does not appear to guard against X" plus what could follow, with
> **HUMAN VERIFICATION REQUIRED** called out where a live-data check would be needed to confirm impact.

## 1. Invoice/document number generation — `MAX(id)+1` race condition

**CONFIRMED (code)**: primary keys for every major transaction table are generated in C# by reading
`MAX(column)+1` with no locking and no uniqueness re-check before the subsequent insert.

```csharp
// Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:12-28
public int PrimaryKeyMax(string columnName, string tableName, string dataBaseName)
{
    string query = @"SELECT (isnull(MAX(" + columnName + "),0)+1) as PKMaxNo FROM " + tableName;
    aTableForPk = aCommonInternalDal.DataContainerDataTable(query, dataBaseName);
    pk = Int32.Parse(aTableForPk.Rows[0][0].ToString().Trim());
    return pk;
}
```

Call sites confirmed by grep (non-exhaustive — pattern `PrimaryKeyMax(` appears throughout `Library.BLL`):
`InvoiceBLL.cs:124` (`tblInvoice`), `:133` (`tblInvoice` again, consumer path), `:147` (`tblReturnInvoice`),
`:156` (`tblReturnInvoiceDetail`), `:169` (`tblDCStoreFreeze`), `:332` (`tblInvoiceDetail`), `:403`
(`tblInvoiceDetail`, return path), plus many more across other BLL classes (`OrderListBLL`, etc. — not
individually enumerated here).

This is the read-then-insert pattern with no transaction and no `UNIQUE` re-check between the read and the
`INSERT` (§ Transaction Integrity Audit confirms no `TransactionScope`/`SqlTransaction` spans this pair of
statements). Two concurrent save requests can both read the same `MAX+1` value before either inserts.

- Code does not appear to guard against two users saving an invoice/order/detail at (nearly) the same instant
  and computing the same next id.
- Risk: duplicate primary key values, an `INSERT` failing with a PK-violation on the *second* request (if
  `tblInvoice.InvoiceId` etc. has a real PK constraint — **UNKNOWN**, no schema available, `GAP-DB-002`), or, if
  there is *no* PK constraint on that column, two invoices silently sharing the same id, which would corrupt
  every downstream join (`tblInvoiceDetail.InvoiceId`, ledger postings, reports).
- **HUMAN VERIFICATION REQUIRED** to confirm (a) whether the affected columns actually have a PK/unique
  constraint in the live schema, and (b) whether any duplicate-id rows already exist in production data.

### 1.1 Invoice-number generation has the same race, plus a display-format bug

```csharp
// Library.BLL/SInventory_BLL/InvoiceBLL.cs:437-486
public string InvoiceNoGenerator(int id, string comCode)
{
    aDataTable = aInvoiceDal.InvoiceNoCount(id.ToString());   // reads current max sequence for this unit
    Id = Convert.ToString(Convert.ToInt32(aDataTable.Rows[0][0].ToString().Trim()) + 1);
    ... // zero-pad to 8 digits
    code = "INV-" + comCode.Trim() + "-" + Id;
    return code;
}
```

Same MAX+1-outside-a-transaction shape as §1 — confirmed by the same evidence (`InvoiceBLL.cs:126` calls this
right after `PrimaryKeyMax`, with no shared transaction across the two reads and the subsequent insert at
`InvoiceBLL.cs:129`). This directly matches `GAP-APP-006` in `specs/SPEC-GAPS.md`.

## 2. Orphan-record risk given the transaction findings

Per `docs/audit/transaction-integrity-audit.md` §2, the invoice save path inserts `tblInvoice` (header) and then
separately, in a loop, inserts `tblInvoiceDetail` rows and updates `tblDCStore` — each insert/update is its own
auto-committing statement with no enclosing transaction and no verification step that "the parent row I'm about
to reference actually made it to disk."

- Code does not appear to guard against a header insert succeeding while none/some of its detail-row inserts
  succeed (see transaction audit §2.3 for the concrete failure trace).
- Risk: an invoice header (`tblInvoice`) with fewer `tblInvoiceDetail` child rows than the UI's grid showed —
  which corrupts any report or total (`TpGrandTotal` etc., stored on the header from the *UI's* computed totals)
  that assumes header totals reconcile with the sum of detail rows.
- Conversely, nothing in `InvoiceDetailInsertBLL` (`InvoiceBLL.cs:322-391`) checks that `aInvoiceDetail.InvoiceId`
  (passed straight through to `aDetailForInsert.InvoiceId`, line 388) corresponds to a header row that is
  actually committed — it trusts the `invoiceId` returned by the earlier `SaveInvoice()` call without
  re-querying `tblInvoice`. If `SaveDataForInvoice` silently returned `false` (its return value is a `bool` —
  see `InvoiceBLL.cs:129`, `return aInvoiceDal.SaveDataForInvoice(aInvoice);`) the caller
  (`InvoiceCreationForCustomer.aspx.cs:496`, `aInvoiceBll.SaveInvoice(aInvoice, out invoiceId, out invoiceNo);`)
  **discards that boolean entirely** and proceeds to `SaveInvoiceDetail(invId)` regardless.

```csharp
// InvoiceCreationForCustomer.aspx.cs:428-437
protected void saveButton_Click(object sender, EventArgs e)
{
    if (Validation() == true)
    {
        int invId = SaveInvoice();          // return value of the underlying bool SaveInvoice(...) is never checked
        SaveInvoiceDetail(invId);           // proceeds unconditionally
        Clear();
        showMessageBox("Create Invoice");
    }
}
```

- Code does not appear to guard against the header insert failing (returning `false`/0 rows affected) and detail
  rows still being written against an `invId` whose header row does not exist.
- Risk: `tblInvoiceDetail` rows referencing a non-existent `tblInvoice.InvoiceId` — a true orphan child record —
  **if** there is no enforced foreign key from `tblInvoiceDetail.InvoiceId` to `tblInvoice.InvoiceId` in the live
  schema. **HUMAN VERIFICATION REQUIRED**: whether such an FK exists is UNKNOWN (`GAP-DB-002`); if it does exist,
  the detail insert would instead fail loudly (a different, but still uncontrolled, failure mode — see the
  transaction audit for what happens to the header in that case, since nothing rolls it back either).

## 3. Missing pre-insert validation, more generally

- **No duplicate/existence check before number generation**: neither `SaveInvoice` nor `InvoiceNoGenerator`
  queries for an existing row with the about-to-be-used id/number before inserting (confirmed — full method
  bodies read, `InvoiceBLL.cs:122-129,437-486`). The only "uniqueness" mechanism is whatever constraint may or
  may not exist at the database layer, which this audit cannot see.
- **No parent-existence check before detail insert**: confirmed above (§2) — `SaveInvoiceDetail`/`UpdateDCStock`/
  `InvoiceDetailInsertBLL` never re-select `tblInvoice` for `invId` before writing children.
- **`DB_Manager.ExecuteNonQueryAction` masks failures as `0` rows**: (`DB_Manager.cs:90-100`) a genuine SQL error
  (constraint violation, timeout, deadlock) is caught, the transaction rolled back, and `0` returned — identical
  to what a legitimate "no rows matched" `UPDATE`/`DELETE` would return. Any caller on this code path that treats
  `0` as merely "nothing to update" rather than "an error occurred" cannot distinguish the two. This is the same
  finding as `GAP-APP-008`, cited here for its data-integrity angle: a caller might proceed as if a
  stock-decrement `UPDATE` succeeded when it actually failed and rolled back.
- **Stock decrement is an overwrite, not an atomic decrement**: `UpdateDCStoreQuantity` (called at
  `InvoiceBLL.cs:303,317`) is documented in `specs/SPEC-GAPS.md` (`GAP-APP-005`) as
  `UPDATE tblDCStore SET StockQty = '<value computed in C#>'` — the new value is computed by reading the current
  stock into C#, subtracting in C#, and writing the result back, with no `WHERE StockQty = <the value just read>`
  optimistic-concurrency guard and no atomic `SET StockQty = StockQty - @qty`. Between the read and the write,
  another concurrent sale against the same batch can also read the pre-decrement value.
  - Code does not appear to guard against two concurrent invoices against the same batch both computing their
    decrement from the same starting stock figure.
  - Risk: a lost update — one sale's decrement overwrites the other's, so `tblDCStore.StockQty` ends up higher
    than it should be (effectively "un-consuming" one of the two sales' stock) or, depending on ordering, stock
    can be driven negative. **HUMAN VERIFICATION REQUIRED** to confirm whether negative or inflated `StockQty`
    values exist in the live `tblDCStore` table.

## 4. Bugs

| ID | Title | Severity | Confidence |
|---|---|---|---|
| `BUG-DATA-001` | `ClsPrimaryKeyFind.PrimaryKeyMax` computes every major PK (`tblInvoice`, `tblInvoiceDetail`, `tblReturnInvoice`, `tblDCStoreFreeze`, etc.) via an untransacted `MAX(col)+1` read; concurrent saves can compute the same id. Evidence: `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:12-28`, call sites `InvoiceBLL.cs:124,133,147,156,169,332,403`. | High | CONFIRMED (code pattern); live-data collision unconfirmed — HUMAN VERIFICATION REQUIRED |
| `BUG-DATA-002` | `InvoiceNoGenerator` derives the human-readable invoice number the same untransacted way, so two concurrent invoices for the same company unit can be assigned the identical invoice number string. Evidence: `InvoiceBLL.cs:437-486`. | High | CONFIRMED (code pattern) |
| `BUG-DATA-003` | `saveButton_Click` discards the `bool` result of the header insert (`SaveInvoice`/`SaveDataForInvoice`) and unconditionally proceeds to insert detail rows against the generated `invId`, with no re-check that the header row exists. Evidence: `InvoiceCreationForCustomer.aspx.cs:428-437`, `InvoiceBLL.cs:122-129`. | High | CONFIRMED |
| `BUG-DATA-004` | `DB_Manager.ExecuteNonQueryAction` returns `0` both for "no rows matched" and for a genuine SQL exception (after silently rolling back), so callers cannot detect real failures on this code path. Evidence: `DB_Manager.cs:90-100`. Same root cause as `GAP-APP-008`. | High | CONFIRMED |
| `BUG-DATA-005` | Stock quantity is updated by a C#-computed absolute `SET StockQty = <value>` rather than an atomic decrement, creating a lost-update window under concurrent sales against the same batch. Evidence: call sites `InvoiceBLL.cs:303,317` into `InvoiceDAL.UpdateDCStoreQuantity` (per `GAP-APP-005`). | High | CONFIRMED (code pattern); live impact — HUMAN VERIFICATION REQUIRED |

## 5. Explicitly not claimed

- No claim is made that any duplicate key, orphaned row, or negative stock value currently exists in
  `SalesrollDB_ZAS` — this would require querying the live database, which this audit does not do.
- No claim is made about the presence or absence of database-level constraints (PK/FK/CHECK/UNIQUE) — the
  schema is not in the repository (`GAP-DB-002`). Every risk above is conditioned on "if no such constraint
  exists" and is marked for human verification against the live schema.
