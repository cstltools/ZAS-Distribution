# WF-001 — Invoice creation and FEFO batch allocation

The single most important algorithm in the application. Every proforma-invoice screen ends here, and stock,
costing, VAT and the invoice total all depend on it.

**Primary evidence**

| | |
|---|---|
| Screen | `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx(.cs)` (the clearest variant) |
| Business logic | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` — `SaveInvoice`, `SaveInvoiceDetails`, `UpdateDCStock`, `InvoiceDetailInsertBLL`, `InvoiceNoGenerator` |
| Data access | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` — `SaveDataForInvoice`, `BatchWiseProductQty`, `UpdateDCStoreQuantity`, `SaveDataForInvoiceDetails`, `ProductInfoDAL`, `InvoiceNoCount`, `LoadCustomerMaster`, `DiscountofProduct` |
| Tables | `tblInvoice`, `tblInvoiceDetail`, `tblDCStore`, `tblUnitPrice`, `tblProduct`, `tblProductDiscount`, `tblProductQuotedPrice`, `tblCustMaster`, `View_DCStoreCurrentStock` |

---

## Stage 1 — Identify the customer

`custCodeTextBox_TextChanged` (or `custNameTextBox_TextChanged`, which splits the autocomplete string on `:`
and takes the code) calls `GetCustInfo(custCode)` → `InvoiceBLL.CustomerMaster` → `InvoiceDAL.LoadCustomerMaster`:

```sql
SELECT * FROM dbo.tblCustMaster
INNER JOIN dbo.tblMarket        ON tblMarket.MarketId        = tblCustMaster.MarketId
INNER JOIN dbo.tblTerritory     ON tblTerritory.TerritoryId  = tblMarket.TerritoryId
INNER JOIN dbo.tblMIOInfo       ON tblMIOInfo.TerritoryId    = tblTerritory.TerritoryId
INNER JOIN dbo.tblEmpGeneralInfo ON tblMIOInfo.EmployeeId    = tblEmpGeneralInfo.EmpInfoId
INNER JOIN dbo.tblArea          ON tblArea.AreaId            = tblTerritory.AreaId
INNER JOIN dbo.tblDcWiseAreaInfo ON tblDcWiseAreaInfo.AreaId = tblArea.AreaId
INNER JOIN dbo.tblCompanyUnit   ON tblDcWiseAreaInfo.DCId    = tblCompanyUnit.ComUnitId
INNER JOIN dbo.tblCompanyInfo   ON tblCompanyInfo.CompanyId  = tblCompanyUnit.CompanyId
INNER JOIN dbo.tblCustCategory  ON tblCustCategory.CategoryId = tblCustMaster.CategoryId
WHERE CustomerCode = @custCode
```

Result fills the read-only header and three hidden fields that drive everything downstream:

| Hidden field | From column | Used for |
|---|---|---|
| `hdComUnitId` | `ComUnitId` | stock lookup, price lookup, invoice numbering |
| `hdCustomerMasterId` | `CustomerMasterId` | discount lookup, invoice header |
| `hdMiaId` | `MiaId` | invoice header |

**All nine joins are INNER.** Any missing link — an area not allocated to a DC, a territory with no MIO, an MIO
whose employee row is gone — returns zero rows, and `GetCustInfo`'s `else` branch is **empty**: the screen
simply shows nothing and gives no message (`BR-CUS-001`).

## Stage 2 — Add a product line

`codeTextBox_TextChanged` → `GetProduct(rowIndex, productCode)`.

1. `ProductCodeValidation(productCode, rowIndex)` scans every other grid row for the same code; a duplicate is
   rejected with `"<code> No: Product Already Inserted!!!"` and both the code and name cells are cleared.
2. `InvoiceBLL.ProductInfo(hdComUnitId, productCode)` → `InvoiceDAL.ProductInfoDAL`:

```sql
SELECT P.ProductCode, (P.ProductName + ':' + P.PackSize) AS ProductName, P.PackSize,
       ISNULL(UP.UnitPrice,0) AS UnitPrice, ISNULL(VCS.TotalQty,0) AS StockQty,
       UP.VATAmountPerUnit AS VAT, ISNULL(UP.CostPrice,0) AS CostPrice,
       ISNULL(UP.VATPercentage,0) VATPercentage, MRPPrice, P.ProductId
FROM dbo.tblProduct P
LEFT JOIN dbo.tblUnitPrice UP ON P.ProductCode = UP.ProductCode
LEFT JOIN (SELECT ComUnitId, ProductCode, TotalQty FROM View_DCStoreCurrentStock
           WHERE ComUnitId = @unit AND ProductCode = @code) VCS ON P.ProductCode = VCS.ProductCode
WHERE UP.ActionStatus = 'Accepted' AND UP.IsActive = 'TRUE' AND P.ProductCode = @code
```

3. Zero rows → `"No Any Stock or Product of <code>"` and the row is cleared. Note the message conflates two
   different causes: **no approved active price** and **no stock**. A product with stock but an unapproved
   price produces the same message.
4. The overload used by the customer-aware screens adds the quoted-price override (`BR-PRC-002`).

The row's read-only cells are then filled: `ProductName` (`Name:PackSize`), `StockQty`, `UnitPrice`, `UnitVAT`.
Columns 4 and 5 (`UP`, `UVAT`) are **hidden** by `InitialGrid()`:

```csharp
gridLineItemGridView.Columns[4].Visible = false;
gridLineItemGridView.Columns[5].Visible = false;
```

so the user never sees the unit price or unit VAT, only the computed totals.

## Stage 3 — Quantity, discount, line totals (screen side)

`qtyTextBox_TextChanged`:

```
TotalPrice (col 7)   = UnitPrice × Quantity
TotalPriceVat (col 8)= UnitVatAmount × Quantity
DiscountPct (col 9)  = InvoiceBLL.ProductDiscount(productCode, hdCustomerMasterId, invoiceDate)
DiscountAmt (col 10) = pct == 0 ? 0 : (UnitPrice / 100) × pct        ← per UNIT
NetPrice (col 11)    = TotalPrice − DiscountAmt + TotalPriceVat
TotalQty (col 13)    = Quantity
BonusQty (col 12)    = 0                                             ← reset on every quantity change
```

then `TotalValueCalculation()` re-sums columns 7, 8, 10 and 11 into `tpTptalTextBox`, `vatTotalTextBox`,
`disTotalTextBox` and `grandTotalTextBox`.

`bQtyTextBox_TextChanged` adds the bonus quantity **onto the existing** total quantity:

```csharp
tQty = Convert.ToDecimal(bQty) + Convert.ToDecimal(tQty);
```

so entering a bonus twice compounds it, and re-entering the quantity resets the bonus to zero. (CONFIRMED.)

> **`GAP-APP-002`.** `DiscountAmt` here is *per unit*; the value actually stored in stage 5 is *per line*
> (`TotalPrice × pct / 100`). They agree only when `Quantity = 1`. The grand total the user approves is
> therefore not the grand total that ends up on the persisted lines whenever a discounted line has quantity > 1.

## Stage 4 — Save the header

`saveButton_Click` → `Validation()`:

| Check | Message |
|---|---|
| `orderNoTextBox.Text == ""` | `Please Input Order Number!!` |
| `orderDateTextBox.Text == ""` | `Please Input Order Date!!` |
| `payTypeDDL.Text == ""` | `Please Input Payment Type!!` |
| any grid row with empty `codeTextBox` or empty `tQtyTextBox` | `Please Remove Blank Row!!` |

Note `payTypeDDL.Text` reads the **selected item's text**, not its value — it is empty only if the placeholder
item's text is empty, which it is not. This check is therefore ineffective as written. (CONFIRMED.)
There is **no check that the customer was resolved** and **no check that any line exists**.

`SaveInvoice()` builds a `Library.DAO.SInventory_Entities.Invoice`:

| DAO property | UI source | `tblInvoice` column |
|---|---|---|
| `InvoiceDate` | `invDateTextBox` (read-only, defaults to today, `dd-MMM-yyyy`) | `InvoiceDate` |
| `OrderNo` | `orderNoTextBox` | `OrderNo` |
| `OrderDate` | `orderDateTextBox` | `OrderDate` |
| `CustomerMasterId` | `hdCustomerMasterId` | `CustomerMasterId` |
| `ComUnitId` | `hdComUnitId` | `ComUnitId` |
| `MiaId` | `hdMiaId` | `MiaId` |
| `PaymentTypeId` | `payTypeDDL.SelectedValue` | `PaymentTypeId` |
| `TpTotal` | `tpTptalTextBox` | `TpTotal` |
| `TpDiscount` | `disTotalTextBox` | `TpDiscount` |
| `TpVat` | `vatTotalTextBox` | `TpVat` |
| `TpGrandTotal` | `grandTotalTextBox` | `TpGrandTotal` |
| `UserId` | `Session["UserId"]` | `UserId` |
| `ComUnitCode` | `comUnitNameTextBox` split on `:`, `[0]` | (used only for the number) |

`InvoiceBLL.SaveInvoice`:

```csharp
invoiceId  = aClsPrimaryKeyFind.PrimaryKeyMax("invoiceId", "tblInvoice");   // MAX(id)+1  → BR-SYS-003
invoiceNo  = InvoiceNoGenerator(aInvoice.ComUnitId, aInvoice.ComUnitCode);  // INV-<code>-<9 digits>
return aInvoiceDal.SaveDataForInvoice(aInvoice);                            // INSERT, autocommitted
```

`SaveDataForInvoice` inserts 21 columns into `tblInvoice` by string concatenation, including
`OrderId`, `TotalSpecialAmount`, `OldTradePolicy`, `ProductOffer`, `Remarks` and `DeliveryManId`, which this
screen never sets — they are written as the DAO's default (`0` / empty string), **not** as `NULL`.

## Stage 5 — Allocate stock and write the details (the FEFO loop)

`SaveInvoiceDetail(invId)` collects one `InvoiceDetail` per **grid row**, then
`InvoiceBLL.SaveInvoiceDetails` calls `UpdateDCStock` for each.

### The candidate batches

```sql
SELECT * FROM tblDCStore
WHERE ProductCode = @code AND ComUnitId = @unit AND StockQty > 0
ORDER BY ExpDate ASC          -- ← FEFO: nearest expiry first
```

### The loop (`InvoiceBLL.UpdateDCStock`)

```
totalQty = Quantity + BonusQuantity
bonusQty = BonusQuantity

for each batch row, nearest expiry first:
    stockQty = row.StockQty

    if a remainder is carried from the previous batch (restQty1 ≠ 0):
        restQty = stockQty + restQty1            (restQty1 is negative — this is a subtraction)
    else:
        if a bonus remainder is carried:  bonusQty = |restBonusQty1|
        if bonusQty >= stockQty:                  → this whole batch goes to bonus
             restBonusQty      = stockQty − bonusQty
             restBonusQty1     = restBonusQty
             restBonusQtyBool  = true
             restBonusQtyOverflow = (restBonusQty == 0) ? bonusQty : stockQty
        else:
             restQty = stockQty − totalQty
             qty     = totalQty − bonusQty

    if restQty < 0  OR  this batch was fully consumed by bonus:
         restQty1 = restQty                                        (carry the shortfall forward)
         InvoiceDetailInsertBLL(... qty, bonus ...)                → INSERT tblInvoiceDetail
         InvoiceDAL.UpdateDCStoreQuantity(dcStoreId, 0)            → UPDATE tblDCStore SET StockQty = 0
         bonusQty = 0
    else:
         if restQty1 < 0: qty = −restQty1                          (this batch settles the carry)
         InvoiceDetailInsertBLL(... qty, bonus ...)                → INSERT tblInvoiceDetail
         InvoiceDAL.UpdateDCStoreQuantity(dcStoreId, restQty)      → UPDATE tblDCStore SET StockQty = restQty
         break                                                     ← demand satisfied
```

**Facts this establishes**

1. **One grid line becomes 1..N `tblInvoiceDetail` rows**, one per batch consumed. Every downstream report and
   return must expect that.
2. **Batch, receive date and expiry date are copied onto the detail row** from the batch it came from
   (`BatchNo`, `ReceiveDate`, `ExpDate`), which is what makes batch-wise reporting and expiry tracking possible.
3. **Bonus quantity is consumed before chargeable quantity** within a batch, and can span batches on its own
   carry variable.
4. **`StockQty` is overwritten with a value computed in C#**, not decremented in SQL (`BR-INV-003`,
   `GAP-APP-005` — lost update under concurrency).
5. **If total stock is insufficient the loop simply ends.** There is no `else` that raises an error, no
   rollback, and no message. The invoice header is already committed, some detail rows are written, and the
   remaining demand is silently dropped (`BR-INV-002`, `GAP-APP-004`).
6. Each `INSERT`/`UPDATE` is its own autocommitted statement — a failure mid-loop leaves a partial invoice and
   partially decremented stock (`BR-SYS-004`, `GAP-APP-007`).

### The persisted line (`InvoiceBLL.InvoiceDetailInsertBLL`)

```
InvoiceDetailId    = MAX(InvoiceDetailId)+1 on tblInvoiceDetail
ProductCode/Name/PackSize, InvoiceId, OrderDetailsId   copied from the grid line
BatchNo, ReceiveDate, ExpDate                          copied from the batch row
CostPrice, VATPercentage                               re-read from ProductInfo (tblUnitPrice)
UnitPrice                                              taken from the GRID, not re-read
UnitVatAmount                                          taken from the grid
Quantity           = qty allocated to THIS batch
BonusQuantity      = bonus allocated to THIS batch
TotalQuantity      = Quantity + BonusQuantity
TotalPrice         = Quantity × UnitPrice
TotalPriceVatAmount= UnitVatAmount × Quantity
DiscountAmount     = pct>0 && amt>0 → TotalPrice × pct / 100
                     pct=0 && amt>0 → (lineDiscountAmount / lineQuantity) × Quantity
                     pct=0 && amt=0 → 0
SpecialAmount      = lineQuantity > 0 ? (lineSpecialAmount / lineQuantity) × Quantity : 0
NetAmount          = (TotalPrice − DiscountAmount − SpecialAmount) + TotalPriceVatAmount
DCStoreId          = the batch row consumed
```

Line-level discount and special amount are **pro-rated across the batches** by allocated quantity. Rounding is
not controlled anywhere: these are `decimal` divisions with no `Math.Round`, so the sum of the per-batch
amounts can differ from the header total by a fraction. (CONFIRMED; the materiality is UNKNOWN.)

## Stage 6 — Finish

```csharp
Clear();                          // resets header, hidden fields and rebuilds a one-row grid
showMessageBox("Create Invoice"); // the only confirmation the user gets
```

`invTextBox` keeps the generated invoice number, and `printButton_Click` opens
`../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=<number>` in a 950×700 popup.

**`Clear()` runs unconditionally, before any success is verified.** `SaveInvoiceDetail` always returns `true`
and its return value is not checked; `SaveInvoice`'s `bool` is discarded. The user sees "Create Invoice" whether
or not anything was written.

## Stage 7 — Accounting

Not performed by this screen. `InvoiceDAL.PostJournal(invoiceId)` → `EXEC sp_ProformaPosting @InvoiceId` is
invoked from the screens that post the proforma. What it writes into `ZAS_ACCDB` is **UNKNOWN** — see
[../../SPEC-GAPS.md](../../SPEC-GAPS.md) → `GAP-DB-001`.

---

## Sequence summary

```
User → custCodeTextBox (postback)
         → InvoiceBLL.CustomerMaster → InvoiceDAL.LoadCustomerMaster → 9-way INNER JOIN
User → codeTextBox (postback, per row)
         → InvoiceBLL.ProductInfo → InvoiceDAL.ProductInfoDAL → tblProduct+tblUnitPrice+View_DCStoreCurrentStock
User → qtyTextBox (postback, per row)
         → InvoiceBLL.ProductDiscount → InvoiceDAL.DiscountofProduct → tblProductDiscount
         → TotalValueCalculation()
User → saveButton
         → Validation()
         → InvoiceBLL.SaveInvoice   → ClsPrimaryKeyFind.PrimaryKeyMax → SELECT MAX(invoiceId)+1
                                    → InvoiceNoGenerator → InvoiceDAL.InvoiceNoCount
                                    → InvoiceDAL.SaveDataForInvoice → INSERT tblInvoice
         → InvoiceBLL.SaveInvoiceDetails
              for each line → UpdateDCStock
                                → InvoiceDAL.BatchWiseProductQty  → SELECT tblDCStore ORDER BY ExpDate
                                → per batch: InvoiceDetailInsertBLL → INSERT tblInvoiceDetail
                                             UpdateDCStoreQuantity  → UPDATE tblDCStore
         → Clear(); alert("Create Invoice")
```

## Test cases this workflow implies

| ID | Scenario | Expected (CONFIRMED from code) |
|---|---|---|
| `TC-WF001-01` | Customer whose area has no `tblDcWiseAreaInfo` row | header stays blank, **no message** |
| `TC-WF001-02` | Product with stock but `ActionStatus != 'Accepted'` on its price | `"No Any Stock or Product of <code>"` |
| `TC-WF001-03` | Same product entered on two rows | `"<code> No: Product Already Inserted!!!"`, row cleared |
| `TC-WF001-04` | Quantity spanning three batches | three `tblInvoiceDetail` rows; first two batches set to `StockQty = 0`; batches consumed in ascending `ExpDate` |
| `TC-WF001-05` | Quantity greater than total stock | invoice is created, fewer units than requested are written, **no error** |
| `TC-WF001-06` | Discounted line with quantity 5 | on-screen discount = `UnitPrice/100 × pct`; stored discount = `TotalPrice × pct/100` — **they differ** |
| `TC-WF001-07` | Enter bonus quantity, then change quantity | bonus resets to 0 |
| `TC-WF001-08` | Enter bonus quantity twice without changing quantity | bonus compounds into `TotalQty` |
| `TC-WF001-09` | Two users invoice the same batch simultaneously | one update overwrites the other; stock is overstated |
| `TC-WF001-10` | Leave payment type on the placeholder and save | passes validation (`payTypeDDL.Text` is never empty), then throws on `Convert.ToInt32("")` |
