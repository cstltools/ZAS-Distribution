# WF-003 — Customer payment application

Applying a received amount against one or more open invoices, and posting it to the ledger.

**Primary evidence**

| | |
|---|---|
| Screen | `Solution.Web/SInventory_UI/CustomerPayment.aspx(.cs)` (935 lines) |
| Variants | `MarketWiseCustomerPayment.aspx`, `CustomerPaymentProcess.aspx`, `PaymentTranscation(New/New2).aspx`, `Non-TranscationalInvoicePayment.aspx` |
| Business logic | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` — `SaveCustPayment`, `SaveCustDetail`, `UpdateInvoicePaymentAmount` |
| Data access | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` — `SaveCustPayment`, `PaymentJournalPosting`, `UpdateInvoicePaymentAmount`, `DeletePaymentMasterInfo` |
| Tables | `tblCustomerPay`, `tblCustPayDetail`, `tblInvoice`, `tblCreditAdjustment`, `View_InvoiceWiseDue`, plus `ZAS_ACCDB`/`CSTL_ACCDB` bank & cash accounts |
| Procedures | `sp_Sales_CustomerPaymentPosting`, `sp_UD_InvoicePayment` — bodies **UNKNOWN** |

---

## Stage 1 — Select the customer and list open invoices

Company → DC → market/manufacturer dropdowns narrow the list, then the customer is chosen either by
autocomplete (`singleCustomerTextBox`) or from `customerDropDownList`. `orderGridView` is bound with the
customer's open invoices, each row carrying:

| Cell | Content |
|---|---|
| 0/1 | `chkSelect` — include this invoice in the payment |
| 5 | invoice amount due (rendered as literal cell text) |
| 6 | previously paid amount (literal cell text; `"&nbsp;"` when null) |
| 7 | `payAmountTextBox` — the amount to apply to this invoice |

`DataKeys` carry `InvoiceId`, `CustomerMasterId` and the company unit — the code reads them both by name
(`DataKeys[i]["InvoiceId"]`) and by index (`DataKeys[i][1]`, `DataKeys[i][2]`) in different places, which
couples the handler to the declared `DataKeyNames` order. (CONFIRMED.)

`chkSelectAll_CheckedChanged` toggles every row; `ClaculatePaymentAmount()` (sic) re-sums the ticked rows into
`paymentAmountTextBox`.

## Stage 2 — Validation (`Validation()`)

In order, each returning `false` with an alert:

| # | Check | Message |
|---|---|---|
| 1 | `payTypeDDL.SelectedValue == ""` | `Please Select Payment Type!!` |
| 2 | payment type text is `Cash` and `ddlCash.SelectedValue == ""` | `You should select Cash acount!!` |
| 3 | payment type text is `Bank` and `ddlBank.SelectedValue == ""` | `You should select Bank acount!!` |
| 4 | for every ticked row, `InvoiceBLL.CheckChartOfAccount(customerCode)` returns no rows | `Customer does not exist in chart of accounts !!` |
| 5 | no row ticked | `Please Select Invoice from List!!` |
| 6 | a ticked row has an empty `payAmountTextBox` | `Please fill out Pay Amount !!` |
| 7 | `paymentAmountTextBox` empty | `Please Select Payment Amount!!` |
| 8 | sum of ticked row amounts ≠ `paymentAmountTextBox` | `Total Invoice Payment Amount Must Be Equel To Payment Amount` |
| 9 | `paymentDtTextBox` empty | `Please Select Payment Date!!` |
| 10 | `paymentAmountTextBox == 0` | `Payment Amount must be greater then 0!!` |

Checks 2 and 3 compare the payment type by **display text** (`"Cash"`, `"Bank"`) — renaming a
`tblPaymentType` row disables the account requirement silently.

**A financial-year check exists but is commented out:**

```csharp
//DataTable aTable = aInvoiceBll.GetFinancialYearInfo(DateTime.Today.ToString(CultureInfo.InvariantCulture),
//                                                    orderGridView.DataKeys[0][2].ToString());
//if (aTable.Rows.Count == 0) { ShowMessageBox("You should declare financial year !!"); return false; }
```

So a payment can be entered for a date no open financial year covers. What the posting procedure then does is
**UNKNOWN**. (CONFIRMED that the check is disabled; the consequence is UNKNOWN.)

**Not validated:** that the amount applied to an invoice does not exceed its due amount. Over-application is
possible.

## Stage 3 — Credit adjustment (payment type id `2`)

```csharp
if (payTypeDDL.SelectedValue == "2")
{
    var adjustmentDao = new CreditAdjustmentDao {
        CustomerMasterId = DataKeys[i]["CustomerMasterId"],
        CompanyId        = aInvoiceBll.GetCompanyIdByInvoice(DataKeys[i]["InvoiceId"]).Rows[0]["CompanyId"],
        InvoiceId        = DataKeys[i]["InvoiceId"],
        Amount           = Convert.ToDecimal(payAmountTextBox.Text) * -1,   // always negative
        ReturnDate       = paymentDtTextBox,
        EntryBy          = Session["LoginName"], EntryDate = DateTime.Now };
    aInvoiceBll.SaveCreditAdjustmentInfo(adjustmentDao);
}
```

→ `tblCreditAdjustment`.

The payment type is identified by the **hard-coded value `2`**, not by a name or a flag. What type `2` is, is
**UNKNOWN** without `tblPaymentType`. The surrounding `if`/`else` computes `totalamount` two different ways
that produce the same value — dead branching left from an earlier version. (CONFIRMED.)

## Stage 4 — Update the invoice's payment state

```csharp
if (payAmount + previouslyPaid == invoiceAmount)
    aCustPaymentBll.UpdateInvoicePaymentAmount(total.ToString(), "Full",    DataKeys[i][0].ToString());
else
    aCustPaymentBll.UpdateInvoicePaymentAmount(total.ToString(), "Partial", DataKeys[i][0].ToString());
```

**This branch has no effect.** The DAL method it reaches ignores both the amount and the status:

```csharp
public bool UpdateInvoicePaymentAmount(string amount, string status, string id)
{
    var p = new List<SqlParameter> { new SqlParameter("@InvoiceId", id) };   // amount and status unused
    return aCommonInternalDal.UpdateAction("sp_UD_InvoicePayment", p);
}
```

The invoice's paid amount and payment status are therefore decided **entirely inside
`sp_UD_InvoicePayment`**, from the invoice id alone. Whatever the screen computed is discarded.
(CONFIRMED — `GAP-APP-016`. The procedure's actual logic is UNKNOWN.)

The comparison itself is fragile in two further ways: it reads the invoice amount out of
`Cells[5].Text` and the previous amount out of `Cells[6].Text` — **rendered HTML**, guarded only by
`if (Cells[6].Text != "&nbsp;")` — and it compares `decimal`s for exact equality.

## Stage 5 — Persist

```csharp
// CustPaymentBLL.SaveCustPayment(master, details)
aCustPayment.CustPayId = ClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblCustomerPay");   // MAX+1
aCustPaymentDal.SaveCustPayment(aCustPayment);                                             // INSERT master
foreach (detail in details)
{
    detail.CustPayId = aCustPayment.CustPayId;
    if (SaveCustDetail(detail))                                     // INSERT tblCustPayDetail
        aCustPaymentDal.PaymentJournalPosting(CustPayId, InvoiceId);// EXEC sp_Sales_CustomerPaymentPosting
    else
        aCustPaymentDal.DeletePaymentMasterInfo(aCustPayment);      // manual "rollback" of the master
}
return true;                                                        // ← always true
```

**Facts**

1. The compensating delete on failure is the closest thing to a transaction anywhere in the application — and
   it deletes the master while leaving any **already-inserted sibling detail rows** behind.
2. `SaveCustPayment` returns `true` unconditionally; exceptions are rethrown (`throw ex;`, stack trace reset).
3. The page assembles **a fresh one-element `aCustPaymentDetails` list inside the row loop**, so
   `SaveCustPayment` is called **once per ticked invoice**, each call creating its **own `tblCustomerPay`
   master row** carrying the *full* `paymentAmountTextBox` value as `PaymentAmount`. Paying five invoices with
   one receipt therefore creates five master rows, each stamped with the whole receipt amount, and five detail
   rows with the per-invoice amounts. (CONFIRMED from the loop structure — `GAP-APP-017`. Whether that is the
   intended model is UNKNOWN; reports that sum `tblCustomerPay.PaymentAmount` would double-count.)
4. Posting is per detail row: `sp_Sales_CustomerPaymentPosting @CustPayId, @InvoiceId`. The superseded
   `sp_CustomerPaymentPosting` call sits commented out beside it.

## Stage 6 — Finish

```csharp
if (status) { ShowMessageBox("Data Saved Successfully !!!!!"); Clear(); }
```

`status` holds the result of the **last** `SaveCustPayment` call only.

## Downstream

| Step | Screen | Procedure |
|---|---|---|
| Batch the collections | `InvoiceBatchEntry.aspx`, `BatchWiseCollection.aspx` | `sp_Save_PaymentBatchMaster`, `sp_Save_PaymentBatchDetail`, `sp_Save_BatchCode`, `sp_Save_PayCode` |
| Lock (close) a batch | `PaymentTranscation*.aspx` | `sp_PaymentBatchLockProcess` |
| Deposit to bank | `DepositSlipEntry.aspx`, `DepositList.aspx` | `sp_I_CompanyWiseDeposit` |
| Money receipt | | `sp_MRCP_MoneyReceiptMasterInsert`, `sp_MRCP_DeletePayment` |
| Reverse | `PaymentReverse.aspx` | `sp_Sales_CustomerPaymentReversePosting` |
| Remove duplicates | `DuplicatePaymentList.aspx` | `sp_DEL_DuplicatePayment` |

## Test cases

| ID | Scenario | Expected |
|---|---|---|
| `TC-WF003-01` | Pay one invoice in full | one `tblCustomerPay` + one `tblCustPayDetail`; `sp_Sales_CustomerPaymentPosting` runs |
| `TC-WF003-02` | Pay five invoices with one receipt | **five** `tblCustomerPay` rows, each with the full receipt amount — verify against the intended model |
| `TC-WF003-03` | Row amounts do not sum to the header amount | `Total Invoice Payment Amount Must Be Equel To Payment Amount` |
| `TC-WF003-04` | Customer with no chart-of-accounts row | `Customer does not exist in chart of accounts !!`, nothing saved |
| `TC-WF003-05` | Payment date outside any open financial year | **accepted** — the guard is commented out |
| `TC-WF003-06` | Apply more than the due amount | **accepted** — no over-application check |
| `TC-WF003-07` | Detail insert fails on the 2nd of 3 invoices | that master is deleted; the first invoice's master and detail remain |
| `TC-WF003-08` | Rename the `Cash` payment type | the cash-account requirement stops being enforced |
