# WF-002 — Delivery confirmation (Full / Partial / Reject)

The state change that turns a proforma invoice into a real, posted sale. Everything after this point in
order-to-cash — collection, ledger, sales reporting — depends on it.

**Primary evidence**

| | |
|---|---|
| Screen | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationAuto.aspx(.cs)` (the dispatcher) |
| Partial screen | `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx(.cs)` (2,700 lines) |
| Business logic | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` — `SaveFullInvoice`, `SaveRejectInvoice`, `PostDeliveryConfFullJournal`, `PostDeliveryConfPartialJournal`, `PostProformaReturnJournal`, `UpdateInvoice`, `UpdateInvoiceNull` |
| Data access | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` |
| Procedures | `sp_DeliveryConformationFull`, `sp_DeliveryConformationReject`, `sp_Sales_ExecuteDeliveryPipeline`, `sp_Sales_DeliverySalesPosting`, `sp_ProformaFullReturnPosting` — **all bodies UNKNOWN** |

---

## The screen

A grid of proforma invoices for a chosen batch (`OrderInfoBLL.LoadOrderWithInvoice(batchId)`), each row
carrying a `statusDropDownList` and a `reasonReturnDropDownList`, plus a `gotoinvoiceButton`. The dispatcher is
`gotoinvoiceButton_Click`, and it branches on the **text** of the selected status item:

```csharp
if (statusDropDownList.SelectedItem.Text == "Partial")        → redirect to the partial screen
else if (statusDropDownList.SelectedItem.Text == "Full")      → confirm in full, here
else                                                          → reject
```

`"Partial"` and `"Full"` are **hard-coded string literals compared against list-item text**. The list is loaded
from data, so renaming a status value silently routes every row down the `else` branch — i.e. **reject**. That
is the failure mode: the default is not "do nothing", it is "reject". (CONFIRMED, `BR-DLV-001`.)

## Branch 1 — Partial

```csharp
Session["InvoiceId"] = orderGridView.DataKeys[rowindex]["InvoiceId"].ToString();
Response.Redirect("DelivaryInvoiceCreationForCustomerByOrderAuto.aspx");
```

The invoice id travels in `Session`, not the URL. The partial screen re-opens the invoice, lets the user reduce
per-line quantities, and writes the delivery columns on `tblInvoice`:

```sql
UPDATE tblInvoice SET DeliveryTpTotal=…, DeliveryTpDiscount=…, DeliveryTpVat=…, DeliveryTpGrandTotal=…,
                      DeliveryInvoiceStatus='Partial', DelivaryInvoiceNo=…, DelivarySpecialAmount=…,
                      UpdateBy=…, UpdateDate=…
WHERE InvoiceId=…
```

(column list confirmed from `InvoiceDAL.UpdateInvoiceNull`, which nulls exactly these back out).

Posting: `InvoiceBLL.PostDeliveryConfPartialJournal(invoiceId)` →
`EXEC sp_Sales_DeliverySalesPosting @InvoiceId`.

## Branch 2 — Full

```csharp
int status = aInvoiceBll.SaveFullInvoice(
                 orderGridView.Rows[rowindex].Cells[7].Text,     // ← the invoice number, read out of a CELL
                 Session["LoginName"].ToString(),
                 DateTime.Now.ToString("dd-MMM-yyyy"));
```

→ `InvoiceDAL.SaveFullInvoice` →
`EXEC sp_DeliveryConformationFull @InvoiceNo, @UpdateBy, @UpdateDate`.

Then, **unconditionally — the return value of the previous call is not tested**:

```csharp
if (aInvoiceBll.PostDeliveryConfFullJournal(Convert.ToInt32(orderGridView.DataKeys[rowindex]["InvoiceId"])))
{
    ShowMessageBox("Delivery Invoice Save Successfully !!");
    GridView();
}
else if (status == 1)
{
    ShowMessageBox("Delivery Invoice Save Successfully !!");   // ← same message on the failure path
    GridView();
}
```

→ `InvoiceDAL.PostDeliveryConfFullJournal` → `EXEC sp_Sales_ExecuteDeliveryPipeline @InvoiceId`.

Two superseded posting calls sit commented out immediately above it, recording the evolution of this step:

```csharp
//return aCommonInternalDal.UpdateAction("sp_DeliveryConfirmationFullPosting", aSqlParameterList);
//return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesPosting", aSqlParameterList);
return aCommonInternalDal.UpdateAction("sp_Sales_ExecuteDeliveryPipeline", aSqlParameterList);
```

**CONFIRMED problems in this branch**

| # | Problem | Consequence |
|---|---|---|
| 1 | The invoice number is taken from `Cells[7].Text` — a **positional grid cell**, HTML-encoded | inserting a grid column breaks delivery confirmation; a number containing an encodable character arrives corrupted |
| 2 | The result of the confirmation (`status`) is never checked before posting | the journal is posted even if the confirmation failed |
| 3 | Both the success and the failure path show `"Delivery Invoice Save Successfully !!"` | the user cannot tell a failed posting from a successful one |
| 4 | Nothing happens when both fail | silent no-op, no message at all |
| 5 | The chart-of-accounts precondition is **commented out** (see below) | invoices for customers with no `ZAS_ACCDB..tblChartOfAccounts` row are confirmed anyway |

The disabled precondition, still present in the source:

```csharp
//DataTable aTable = aInvoiceBll.CheckChartOfAccount(orderGridView.Rows[rowindex].Cells[2].Text.Trim());
//if (aTable.Rows.Count > 0) { … }
//else { ShowMessageBox("Delivery Confirmation is not pssible because Customer does not exist in chart of accounts !!"); }
```

Whether it was disabled deliberately is **UNKNOWN**. What the posting procedure does when the account is
missing is also UNKNOWN.

## Branch 3 — Reject (the `else`)

```csharp
int status = aInvoiceBll.SaveRejectInvoice(
                 orderGridView.Rows[rowindex].Cells[7].Text,
                 Session["LoginName"].ToString(),
                 DateTime.Now.ToString("dd-MMM-yyyy"),
                 ((DropDownList)orderGridView.Rows[rowindex].FindControl("reasonReturnDropDownList")).SelectedItem.Text);
```

→ `EXEC sp_DeliveryConformationReject @InvoiceNo, @UpdateBy, @UpdateDate, @ReturnReason`,
followed by `InvoiceBLL.PostProformaReturnJournal(invoiceId)` → `EXEC sp_ProformaFullReturnPosting @InvoiceId`.

The reason is stored as the **display text** of the reason dropdown (sourced from `tblReturnReason`), not its
id — so renaming a reason breaks historical comparability.

The "please choose a reason" guard is **commented out**, so a rejection with the placeholder still selected
stores the placeholder text as the reason. (CONFIRMED.)

## State after each branch

| Branch | `tblInvoice` columns written | Procedure | Accounting |
|---|---|---|---|
| Full | `DeliveryInvoiceStatus='Full'`, delivery totals, `DelivaryInvoiceNo`, `UpdateBy`, `UpdateDate` (**set inside the SP — exact list UNKNOWN**) | `sp_DeliveryConformationFull` | `sp_Sales_ExecuteDeliveryPipeline` |
| Partial | as above with `DeliveryInvoiceStatus='Partial'` and reduced delivery totals | screen-side `UPDATE` + `sp_Sales_DeliverySalesPosting` | `sp_Sales_DeliverySalesPosting` |
| Reject | `DeliveryStatus='Reject'` + return reason (**inside the SP — UNKNOWN**) | `sp_DeliveryConformationReject` | `sp_ProformaFullReturnPosting` |

## Reversal

| Action | Mechanism |
|---|---|
| Un-deliver an invoice | `InvoiceDAL.UpdateInvoiceNull` nulls every delivery column back out — the application-side half only |
| Delete a delivery invoice | `sp_DeleteDeliveryInvoice`, `sp_DeleteDeliveryInvoice_FullRollback`, `sp_DeliveryInvoiceDeletePosting` |
| Logistics rollback screen | `Solution.Web/SInventory_UI/LogisticeRollBackOperation.aspx` → `Library.DAL/SInventory_DAL/RollBackDAL.cs` |
| Delete a proforma | `sp_Delete_ProformaInvoice`, `sp_Deletenvoice` (sic) |

Whether any of these are idempotent, or restore stock, is **UNKNOWN** — all logic is inside the procedures.

## Where the stock actually moves

**Not here.** Stock was already consumed from `tblDCStore` when the proforma invoice was created
([../../sales-order-invoice/workflows/WF-invoice-batch-allocation.md](../../sales-order-invoice/workflows/WF-invoice-batch-allocation.md)).
Delivery confirmation is a financial and status event. Whether `sp_Sales_ExecuteDeliveryPipeline` performs any
further stock movement is UNKNOWN.

## Test cases

| ID | Scenario | Expected |
|---|---|---|
| `TC-WF002-01` | Status = `Full`, everything healthy | `sp_DeliveryConformationFull` then `sp_Sales_ExecuteDeliveryPipeline`; alert `"Delivery Invoice Save Successfully !!"` |
| `TC-WF002-02` | Status = `Full`, posting procedure fails | **same success alert** if the confirmation returned 1 — indistinguishable |
| `TC-WF002-03` | Status = `Full`, both fail | **no message at all**, grid not refreshed |
| `TC-WF002-04` | Status = `Partial` | redirect to the partial screen with `Session["InvoiceId"]` set |
| `TC-WF002-05` | Status list item renamed from `Full` to `Delivered` | row falls into the `else` → **rejected**, not confirmed |
| `TC-WF002-06` | Reject with the placeholder reason still selected | the placeholder text is stored as the reason |
| `TC-WF002-07` | Customer has no chart-of-accounts row | confirmation proceeds — the guard is commented out; the posting outcome is UNKNOWN |
| `TC-WF002-08` | A column is inserted before index 7 in the grid | the wrong value is passed as the invoice number |
