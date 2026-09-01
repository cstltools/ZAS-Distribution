# Coding and UI Conventions

These patterns repeat on almost every page. Knowing them makes each page spec short, and makes an unfamiliar
page predictable.

## Naming

| Thing | Convention | Example |
|---|---|---|
| Page class | `<Folder>_<PageName>` (Web Site auto-generated) | `SInventory_UI_InvoiceCreationForCustomer` |
| Control | camelCase + type suffix | `custCodeTextBox`, `payTypeDDL`, `saveButton`, `gridLineItemGridView` |
| Hidden field | `hd` prefix | `hdComUnitId`, `hdCustomerMasterId`, `hdMiaId` |
| Filter extender | `f` + target control name | `fqtyTextBox` targets `qtyTextBox` |
| Local field for a layer object | `a` + ClassName | `aInvoiceBll`, `aCommonInternalDal` |
| BLL / DAL class | `<Entity>BLL` / `<Entity>DAL` — but casing is inconsistent (`Dal`, `DAL`, `Bll`, `BLL`) | `InvoiceBLL`, `SampleIssueDal` |
| Table | `tbl<Entity>` | `tblInvoiceDetail` |
| View | `View_<Name>` | `View_DCStoreCurrentStock` |
| Procedure | `sp_<Area>_<Action>`, also `sp_I_` (insert), `sp_UD_` (update/delete), `sp_DEL_`, `sp_GET_`, `sp_Save_` | `sp_Sales_DeliverySalesPosting` |

Spelling in identifiers and labels is inconsistent and often wrong (`Delivary`, `Merket`, `Distirict`,
`Salse Center`, `Summery`, `Transcation`, `Perticular`, `Prettycash`, `Manu` for menu, `Parant` for parent,
`Wearhouse`). **These misspellings are part of the contract** — they appear in table names, column names,
control ids and procedure names. Do not "fix" them in a spec; quote them exactly.

## Page layout

Every screen is an HTML `<table>` grid inside one or more `UpdatePanel`s:

```html
<td class="TDLeft" style="text-align:right">Customer Code:</td>
<td class="TDRight"><asp:TextBox ID="custCodeTextBox" runat="server" AutoPostBack="True"
                                 CssClass="TextBox" ontextchanged="custCodeTextBox_TextChanged" /></td>
```

No control uses `AssociatedControlID`, so a field's label is the nearest preceding text node. That is how the
"Screen label" column in every page spec's section 7 was derived.

CSS classes come from `Solution.Web/css`: `TextBox`, `TextBoxMicroMini`, `DropDown`, `gridview`, `TDLeft`,
`TDRight`, `Button`.

## The editable line-item grid

Order, invoice, return, stock-in and stock-out screens all use the same hand-rolled editable grid. It is worth
describing once:

1. `InitialGrid()` builds an in-memory `DataTable` with one blank row and binds it.
2. Each row's first column carries an **add** (`~/images/lineAdd.png`) and a **remove**
   (`~/images/lineDelete.png`) `ImageButton`.
3. `addImageButton_Click` **re-reads every row out of the grid into a new `DataTable`**, appends one blank row
   and rebinds. `removeImageButton_Click` does the same, skipping the clicked row index and renumbering `SL`.
4. Cells are addressed **positionally**: `grid.Rows[i].Cells[6].FindControl("qtyTextBox")`.
5. Lookup textboxes (`codeTextBox`, `nameTextBox`) have `AutoPostBack="True"` and an `OnTextChanged` handler
   that fetches the product and fills the read-only cells of that row.
6. Calculation is done on `qtyTextBox_TextChanged` / `bQtyTextBox_TextChanged` on the server, then
   `TotalValueCalculation()` re-sums the whole grid into the footer boxes.

Consequences, all CONFIRMED and reported in each affected page spec:

- **Inserting or reordering a grid column silently breaks the code-behind**, because the `Cells[n]` indexes are
  hard-coded and there is no compile-time link between markup and code.
- Every keystroke-completing edit is a **server round trip**; there is no client-side calculation.
- The grid is the source of truth for line items. Nothing is stored until Save, so a session timeout loses the
  whole document.

## Lookup / autocomplete

Product and customer lookups use `AutoCompleteExtender` bound to
`SInventory_UI/SInventoryWebService.asmx` (implementation `Solution.Web/App_Code/SInventoryWebService.cs`). The
service returns **concatenated strings**, and the page splits them apart again:

```csharp
// service returns "CODE:Name:( Region- … )" or "Name:PackSize"
string[] proNameAndPackSize = product.Split(':');
aInvoiceDetail.ProductName = proNameAndPackSize[0];
aInvoiceDetail.PackSize    = proNameAndPackSize[1];
```

So a product name or customer name containing a colon corrupts the parse. Several handlers index `[1]`
unconditionally and will throw `IndexOutOfRangeException` if the separator is absent (`BR-SYS-006`).

## Dropdown loading

Three different helpers exist, and they insert three different placeholder texts:

| Helper | Placeholder inserted at index 0 |
|---|---|
| `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase` | `--------Select---------` |
| `ClsCommonInternalDAL.LoadDropDownListData` | `Select from list` |
| `DB_Manager.LoadDropDownListData` | `Select--------------------` |

All three set `Items.Insert(0, new ListItem(text, String.Empty))` and `SelectedIndex = 0`, so **"nothing
selected" is the empty string, not `-1`**. Validation therefore tests `ddl.SelectedValue == ""` (or, in some
pages, `ddl.Text == ""`, which reads the *selected item's text* — a latent bug where a placeholder differs).

Cascading dropdowns are plain postbacks: parent has `AutoPostBack="True"` + `OnSelectedIndexChanged`, and the
handler reloads the child with the parent's key.

Active-record filtering lives in the inline SQL, most commonly `IsActive='True'` and `ActionStatus='Accepted'`.
Note that `IsActive` is compared against the strings `'True'`, `'TRUE'`, `'1'` and `'False'` in different
places — the column is written from `Convert.ToBoolean(...).ToString()` in some DAL methods and from a literal
`'1'` in others (`BR-SYS-005`).

## Validation

The standard shape:

```csharp
private bool Validation()
{
    if (orderNoTextBox.Text == "") { showMessageBox("Please Input Order Number!!"); return false; }
    ...
    return true;
}

protected void saveButton_Click(object sender, EventArgs e)
{
    if (Validation() == true) { ... }
}
```

- Server-side, imperative, message-per-rule. Almost no `asp:*Validator` controls are used (1 in the whole
  application).
- Client-side restriction is `FilteredTextBoxExtender` (89 declared) — **presentation only**, never re-checked
  on the server.
- Numeric conversion is unguarded `Convert.ToDecimal(textBox.Text)`. An empty or non-numeric value throws
  `FormatException`, which — with no global handler — becomes a yellow screen.

## User messages

```csharp
protected void showMessageBox(string message)
{
    message = message.Replace("'", "\'");
    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
        String.Format("alert('{0}');", message), true);
}
```

Copies of this helper (`showMessageBox`, `ShowMessageBox`, `MessageBox`) are duplicated in dozens of pages.
Messages are typically terminated with `!!` or `!!!` and are frequently misspelled — the login failure message
is literally `User name or password incourect !!!!`.

## Opening a report

```csharp
string url = "../SInventory_RPTVIEW/<Name>Viewer.aspx?<params>";
ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW",
    "var Mleft=(screen.width/2)-(950/2);var Mtop=(screen.height/2)-(700/2);" +
    "window.open('" + url + "', null, 'height=700,width=950,...');", true);
```

Parameters travel by query string, by `Session`, or both — see [../reports/REPORT-INVENTORY.md](../reports/REPORT-INVENTORY.md).

## Entry / View / Edit triples

Master data follows a three-page pattern: `<Entity>Entry.aspx` (or `<Entity>Information.aspx`) creates,
`<Entity>View.aspx` lists, `<Entity>Edit.aspx` updates. The View page passes the key to the Edit page — usually
through `Session`, sometimes through the query string. Deletion is generally **soft**: `IsActive` is set to
false, or `ActionStatus` to `'Delete'` / `'Cancel'`.

## Dead code

Large commented-out blocks are everywhere — alternative connection strings in `Web.config`, superseded DAL
methods, whole earlier versions of a query kept above the live one. Where a page spec cites a query, it cites
the **live** one. Superseded variants are ignored, and calling them out individually is out of scope; treat any
commented block as non-authoritative.
