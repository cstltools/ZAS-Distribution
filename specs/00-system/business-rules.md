# Application-wide Business Rules

Rules that apply across screens. Page-specific rules are in section 19 of each page spec.

**Confidence** is one of:

- `CONFIRMED` — read directly from source; the cited file and method prove it.
- `INFERRED` — a reasonable reading of the code, but the code does not state it outright.
- `UNKNOWN` — the rule is implied by a name or a call, but its logic lives somewhere the repository does not
  contain (almost always a stored procedure).

---

## Identity, numbering and concurrency

### `BR-SYS-001` — Invoice numbers are per company unit, nine digits, zero-padded

- **Description:** an invoice number is `INV-<ComUnitCode>-<9 digits>`. The counter is derived from the largest
  existing number for that unit, not stored.
- **Source:** code · `Library.BLL/SInventory_BLL/InvoiceBLL.cs::InvoiceNoGenerator`, `InvoiceDAL::InvoiceNoCount`
- **Trigger:** saving a new invoice
- **Expected result:** `INV-<code>-000000123`
- **Confidence:** CONFIRMED

### `BR-SYS-002` — Rows whose `Remarks = 'Due'` are excluded from the invoice sequence

- **Source:** `InvoiceDAL.InvoiceNoCount` — `WHERE ComUnitId = @unit AND Remarks != 'Due'`
- **Consequence:** an invoice created with `Remarks = 'Due'` must not participate in the normal number series,
  or numbers will be reused.
- **Confidence:** CONFIRMED (the exclusion). The *purpose* of `'Due'` invoices is INFERRED.

### `BR-SYS-003` — Primary keys are `MAX(id)+1` computed in the application

- **Source:** `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs`
- **Trigger:** every insert that goes through a `*BLL.Save*` method
- **Expected result:** a new key
- **Actual risk:** the read is outside any transaction, so two concurrent saves get the same key; the second
  insert fails on the primary key (or, if there is no PK constraint — UNKNOWN — duplicates are created).
- **Confidence:** CONFIRMED (the mechanism). The failure mode is CONFIRMED-as-possible; whether it has occurred
  is UNKNOWN.

### `BR-SYS-004` — Multi-row saves are not atomic

- **Source:** `ClsCommonInternalDAL` opens no transaction; page code-behinds issue header and detail saves as
  separate calls. Example: `InvoiceCreationForCustomer.aspx.cs::saveButton_Click`.
- **Consequence:** a failure part-way through a document leaves a committed header with partial details, and
  partially decremented stock. No compensating logic exists.
- **Confidence:** CONFIRMED

### `BR-SYS-005` — `IsActive` is written and compared inconsistently

- **Source:** `'True'` (53 sites), `'1'` (27 sites), `'TRUE'` (3 sites), `'False'` (1 site) across
  `Library.DAL/SInventory_DAL/*.cs`
- **Consequence:** a row written by one screen may be invisible to another screen's filter, depending on the
  column's actual type and collation — both UNKNOWN without the DDL.
- **Confidence:** CONFIRMED (the inconsistency). The practical impact is INFERRED.

---

## Lookup and parsing

### `BR-SYS-006` — Lookup values are colon-delimited composite strings

- **Description:** the autocomplete service returns strings such as
  `CODE:Name:( Region- X; Area - Y; Territory - Z; Market - W ):CustomerMasterId` and
  `ProductName:PackSize`. Pages split on `':'` and index fixed positions.
- **Source:** `Solution.Web/App_Code/SInventoryWebService.cs`, `Library.DAL/SInventory_DAL/WebServiceDal.cs`;
  consumers e.g. `InvoiceCreationForCustomer.aspx.cs::SaveInvoiceDetail`
  (`product.Split(':')` then `[0]` and `[1]`)
- **Expected result:** name and pack size are separated
- **Failure:** a product or customer name containing `:` shifts every position; a value with no `:` throws
  `IndexOutOfRangeException`, which with no global error handler becomes a yellow screen.
- **Confidence:** CONFIRMED

### `BR-SYS-007` — Approval rules are matched to pages by `LIKE '%name%'`

- **Source:** `Library.DAL/InternalCls/ClsApprovalAction.cs` — `WHERE MM.URL LIKE '%' + @pageName + '%'`
- **Consequence:** a page whose file name is a substring of another page's file name inherits that page's
  approval rule.
- **Confidence:** CONFIRMED (the matching). Whether any real pair collides depends on `tblMainMenu` data —
  UNKNOWN.

### `BR-SYS-008` — `"Cancel"` is a magic action label

- **Source:** `ClsApprovalAction.LoadActionControlByUser` (leaves a `"Cancel"` item enabled regardless of
  grant); approval pages (`if (SelectedItem.Text.Trim() != "Cancel")` suppresses the side effect)
- **Expected result:** choosing Cancel records the status but performs no stock or accounting action.
- **Fragility:** the label is compared as a literal; renaming the `tblAction` row breaks both behaviours.
- **Confidence:** CONFIRMED

### `BR-SYS-009` — Control flow keys on display text, not on ids

Several decisive branches compare a list item's **`Text`**, not its value:

| Where | Comparison | If the text changes |
|---|---|---|
| `DelivaryInvoiceCreationAuto.aspx.cs` | `statusDropDownList.SelectedItem.Text == "Partial"` / `"Full"` | every row falls to the `else` branch — **reject** |
| `CustomerPayment.aspx.cs` | `payTypeDDL.SelectedItem.Text == "Cash"` / `"Bank"` | the cash/bank account requirement stops being enforced |
| approval pages | `RadioButtonList.SelectedItem.Text != "Cancel"` | side effects run where they previously did not |
| `ClsApprovalAction` | `rdl.Items[i].Text != "Cancel"` | the Cancel escape hatch disappears |

- **Also:** the reject reason stored by delivery confirmation is the dropdown's display text, not its id.
- **Confidence:** CONFIRMED

### `BR-DLV-001` — Delivery confirmation defaults to reject

`gotoinvoiceButton_Click` handles `"Partial"` and `"Full"` explicitly and treats **everything else** — including
an unselected placeholder — as a rejection, which fires `sp_DeliveryConformationReject` and
`sp_ProformaFullReturnPosting`. There is no "no action" branch.

- **Confidence:** CONFIRMED

---

## Pricing, discount and tax

### `BR-PRC-001` — Only approved, active prices are usable

- **Source:** `InvoiceDAL.ProductInfoDAL` — `WHERE UP.ActionStatus = 'Accepted' AND UP.IsActive = 'TRUE'`
- **Expected result:** a product with no approved active `tblUnitPrice` row returns no price and the screen
  shows `"No Any Stock or Product of <code>"`.
- **Confidence:** CONFIRMED

### `BR-PRC-002` — A customer-specific quoted price overrides the list price

```sql
CASE WHEN QTD.QuotedPrice IS NULL THEN ISNULL(UP.UnitPrice,0) ELSE ISNULL(QTD.QuotedPrice,0) END AS UnitPrice
```
with the quote joined as
```sql
FROM tblProductQuotedPrice Q LEFT JOIN tblCustMaster C ON Q.CustomerMasterId = C.CustomerMasterId
WHERE <today> <= Q.InactiveDate AND ActionStatus = 'Accepted' AND Q.IsActive = 1 AND CustomerCode = @cust
```

- **Source:** `InvoiceDAL.ProductInfoDAL(comUnitId, productCode, customerMasterId)`
- **Note:** the date used is `DateTime.Today` **of the web server**, formatted with the server's current
  culture and concatenated into the SQL string — a locale-dependent comparison. (CONFIRMED)
- **Confidence:** CONFIRMED

### `BR-PRC-003` — Line discount percentage comes from a customer + product + date rule

- **Source:** `InvoiceBLL.ProductDiscount(productCode, customerMasterId, date)` →
  `InvoiceDAL.DiscountofProduct(productCode, customerId, date)`:

```sql
SELECT * FROM dbo.tblProductDiscount
LEFT JOIN dbo.tblProduct ON tblProduct.ProductId = tblProductDiscount.ProductId
WHERE ProductCode = @code AND CustomerMasterId = @cust AND IsActive = '1' AND ActionStatus = 'Accepted'
```

- **Expected result:** `DiscountPercentage` of the first matching row, `0` when no rule matches
- **CONFIRMED defect:** the `date` argument is accepted by both the BLL and the DAL and **is never used in the
  query**. The discount is therefore *not* date-bounded, despite the caller passing the invoice date. An earlier
  version of the method (commented out immediately above it) filtered by `MinQty`/`MaxQty` and
  `Status='Active'` — quantity banding is likewise no longer applied.
- **CONFIRMED risk:** if more than one discount row matches, `Rows[0]` wins with no `ORDER BY` — the winner is
  non-deterministic.
- **Confidence:** CONFIRMED

### `BR-PRC-004` — Line arithmetic

From `InvoiceCreationForCustomer.aspx.cs` (screen-side) and
`InvoiceBLL.InvoiceDetailInsertBLL` (persisted side):

```
TotalPrice        = UnitPrice × Quantity
TotalPriceVat     = UnitVatAmount × Quantity
DiscountAmount    if DiscountPercentage > 0 and DiscountAmount > 0 → TotalPrice × DiscountPercentage / 100
                  if DiscountPercentage = 0 and DiscountAmount > 0 → (DiscountAmount / lineQty) × allocatedQty
                  if both zero                                     → 0
SpecialAmount     = Quantity > 0 ? (SpecialAmount / Quantity) × allocatedQty : 0
NetAmount         = (TotalPrice − DiscountAmount − SpecialAmount) + TotalPriceVat
TotalQuantity     = Quantity + BonusQuantity
```

The **screen** computes the discount amount differently from the **persisted** value:
the screen does `dpAmt = (UnitPrice / 100) × discountPercentage` — per *unit*, not per line — and then
`NetPrice = TotalPrice − dpAmt + TotalPriceVat`. The BLL recomputes it as
`TotalPrice × DiscountPercentage / 100`. **The number the user sees before saving and the number stored can
differ** whenever `Quantity ≠ 1`. This is CONFIRMED from the two code paths; whether it is intentional is
UNKNOWN.

### `BR-PRC-005` — VAT is stored per unit and multiplied out, not derived from a rate at save time

- `tblUnitPrice.VATAmountPerUnit` is fetched as `VAT` and multiplied by quantity.
  `VATPercentage` is read but only used for a (commented-out) alternative calculation.
- **Confidence:** CONFIRMED

---

## Stock

### `BR-INV-001` — Issue is FEFO (first expiry, first out)

```sql
SELECT * FROM tblDCStore
WHERE ProductCode = @code AND ComUnitId = @unit AND StockQty > 0
ORDER BY ExpDate ASC
```

- **Source:** `InvoiceDAL.BatchWiseProductQty`, consumed by `InvoiceBLL.UpdateDCStock`
- **Expected result:** the batch closest to expiry is consumed first; the allocation spans as many batch rows as
  needed, and one invoice line becomes **several `tblInvoiceDetail` rows**, one per batch.
- **Confidence:** CONFIRMED
- **Detail:** see [../sales-order-invoice/workflows/WF-invoice-batch-allocation.md](../sales-order-invoice/workflows/WF-invoice-batch-allocation.md)

### `BR-INV-002` — There is no stock check before allocation

`UpdateDCStock` loops over available batches and `break`s when the remainder is satisfied. If total stock is
less than the requested quantity, the loop simply ends, having written fewer units than requested and set the
last batch to `0`. **No error is raised and the invoice is still created.** The only stock feedback the user
gets is the read-only `CStock` column, which is informational.

- **Source:** `InvoiceBLL.UpdateDCStock` — no `if (available < requested)` branch anywhere
- **Confidence:** CONFIRMED

### `BR-INV-003` — Stock quantity is overwritten, not decremented

```csharp
// InvoiceDAL.UpdateDCStoreQuantity
UPDATE tblDCStore SET StockQty = '<computed value>' WHERE DCStoreId = '<id>'
```

The new value is computed in C# from a value read earlier. Two concurrent invoices reading the same batch will
each write their own absolute result and the second overwrites the first — a lost update.

- **Confidence:** CONFIRMED

### `BR-INV-004` — Returned goods land in a freeze (quarantine) tier, idempotently

- **Source:** `InvoiceBLL.SaveDCStoreFreeze` inserts into `tblDCStoreFreeze` only when
  `InvoiceDAL.CheckReturnExistOrNotInFreezeStore` returns no rows.
- **Confidence:** CONFIRMED

### `BR-INV-005` — DC-store weighted average is deliberately not maintained

- **Source:** commented-out block in `InvoiceBLL.SaveDCStoreFreeze` with the note
  *"Weighted Average (Disabled as per user request to not maintain DC Store WAC)"*
- **Confidence:** CONFIRMED (the code state and the stated reason). Whether it is still the business's wish is
  UNKNOWN.

---

## Customer and credit

### `BR-CUS-001` — A customer's sales centre is derived, not stored on the customer

The owning company unit is resolved through the geography chain:

```
tblCustMaster → tblMarket → tblTerritory → tblArea → tblDcWiseAreaInfo → tblCompanyUnit
```

all **INNER JOIN**s (`InvoiceDAL.LoadCustomerMaster`). A break anywhere — an area not allocated to a DC, a
territory with no MIO, an MIO with no employee row — makes the customer **invisible to invoicing**, and the
screen silently shows nothing (`GetCustInfo` has an empty `else` branch).

- **Confidence:** CONFIRMED

### `BR-CUS-002` — Accounting requires a chart-of-accounts row per customer

- **Source:** `InvoiceDAL.CheckChartOfAccount(customerCode)` reads
  `ZAS_ACCDB..tblChartOfAccounts WHERE CustomerId IS NOT NULL AND CustomerId IN (SELECT CustomerMasterId FROM tblCustMaster WHERE CustomerCode = @code)`;
  `sp_Integration_CustomerSubsidaryInsert` and `sp_I_CustomerGl` create the ledger side.
- **Confidence:** CONFIRMED that the check exists. What happens when it fails is UNKNOWN (inside the procedure).

### `BR-CUS-003` — A financial year must cover the invoice date

```sql
SELECT DISTINCT FinancialYearId FROM tblCompanyUnit UNT
INNER JOIN tblCompanyInfo CI ON CI.CompanyId = UNT.CompanyId
INNER JOIN CSTL_ACCDB..tblFinancialYear ON tblFinancialYear.CompanyId = CI.CompanyId
WHERE UNT.ComUnitId = @unit AND Status IN ('Active') AND @invoiceDate BETWEEN StartDate AND EndDate
```

- **Source:** `InvoiceDAL.GetFinancialYearInfo`
- **Consequence:** posting depends on an **Active** financial year in the *group* accounting database
  `CSTL_ACCDB` covering the date.
- **Confidence:** CONFIRMED

---

## Security rules that the code does not enforce

These are stated here because their *absence* is itself a rule a maintainer must know.

| ID | Rule that does **not** exist | Evidence |
|---|---|---|
| `BR-SEC-001` | No page verifies that the user was granted the menu item before serving the page | no check in any code-behind; only the menu is filtered |
| `BR-SEC-002` | No page verifies company-unit ownership of the record it is editing | no `ComUnitId` check on load-by-key paths |
| `BR-SEC-003` | Passwords are not hashed | `PanalClsDAL.Login`, `UserDAL.SaveUserInformation` |
| `BR-SEC-004` | User input is not parameterised in inline SQL | `ClsCommonInternalDAL` + every `*DAL` string concatenation |
| `BR-SEC-005` | The autocomplete web service does not check the session | `SInventoryWebService.cs` |

See [../SPEC-GAPS.md](../SPEC-GAPS.md) for the full security gap list.
