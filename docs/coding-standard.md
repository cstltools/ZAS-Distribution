# Coding Standard — how the existing code is actually written

This is a description of **observed practice**, not an aspiration. Its purpose is that new code added to this
application matches what is already here, for consistency's sake, not because these are patterns to be proud
of. Full source: [`../specs/00-system/conventions.md`](../specs/00-system/conventions.md). Known risks in these
patterns are called out below as "avoid repeating this," each cited to its `GAP-APP-*` finding.

## Naming

| Thing | Convention | Example |
|---|---|---|
| Page class | `<Folder>_<PageName>` (Web Site auto-generated) | `SInventory_UI_InvoiceCreationForCustomer` |
| Control | camelCase + type suffix | `custCodeTextBox`, `payTypeDDL`, `saveButton`, `gridLineItemGridView` |
| Hidden field | `hd` prefix | `hdComUnitId`, `hdCustomerMasterId`, `hdMiaId` |
| Filter extender | `f` + target control name | `fqtyTextBox` targets `qtyTextBox` |
| Local field for a layer object | `a` + ClassName | `aInvoiceBll`, `aCommonInternalDal` |
| BLL / DAL class | `<Entity>BLL` / `<Entity>DAL` (casing inconsistent) | `InvoiceBLL`, `SampleIssueDal` |
| Table | `tbl<Entity>` | `tblInvoiceDetail` |
| View | `View_<Name>` | `View_DCStoreCurrentStock` |
| Procedure | `sp_<Area>_<Action>`, also `sp_I_`, `sp_UD_`, `sp_DEL_`, `sp_GET_`, `sp_Save_` | `sp_Sales_DeliverySalesPosting` |

**Misspellings are part of the contract.** They appear in table names, column names, control ids and procedure
names (`Delivary`, `Merket`, `Distirict`, `Salse Center`, `Summery`, `Transcation`, `Perticular`, `Prettycash`,
`Manu`, `Parant`, `Wearhouse`). **Do not "fix" them** in new code that touches these tables/procedures — match
the existing spelling exactly, or you will create a second, disconnected name for the same thing. Source:
`../specs/00-system/conventions.md`.

## The repeated architectural pattern: page → BLL → DAL → SP

Every screen follows the layering in [architecture.md](architecture.md): code-behind reads control values, calls
a BLL method (often a one-line pass-through), which calls a DAL method, which either builds an inline SQL string
executed via `ExecuteAllSqlQueryByStoreProcedure`, or calls a named stored procedure with `SqlParameter`s. New
transactional/posting logic should use the **named-procedure-with-`SqlParameter`** path, not the inline-SQL
path — see "Avoid repeating this" below.

## Page layout

Every screen is an HTML `<table>` grid inside one or more `UpdatePanel`s. No control uses
`AssociatedControlID` — a field's label is the nearest preceding text node. CSS classes come from
`Solution.Web/css`: `TextBox`, `TextBoxMicroMini`, `DropDown`, `gridview`, `TDLeft`, `TDRight`, `Button`. Match
these class names for new fields rather than introducing new ones.

## Building the editable line-item grid

Order, invoice, return, stock-in and stock-out screens all use the same hand-rolled editable grid:

1. `InitialGrid()` builds an in-memory `DataTable` with one blank row and binds it.
2. Each row's first column carries an add (`~/images/lineAdd.png`) and remove (`~/images/lineDelete.png`)
   `ImageButton`.
3. `addImageButton_Click` re-reads every row out of the grid into a new `DataTable`, appends one blank row, and
   rebinds; `removeImageButton_Click` does the same, skipping the clicked row and renumbering `SL`.
4. Cells are addressed **positionally**: `grid.Rows[i].Cells[6].FindControl("qtyTextBox")`.
5. Lookup textboxes have `AutoPostBack="True"` + `OnTextChanged` to fetch and fill read-only cells of that row.
6. Calculation happens server-side on `qtyTextBox_TextChanged` etc., then `TotalValueCalculation()` re-sums the
   whole grid into footer boxes.

If you add this pattern to a new page, follow it exactly — a new/different grid style would be inconsistent
with every other screen in the application.

**Avoid repeating this:** inserting or reordering a grid column silently breaks the code-behind, because
`Cells[n]` indexes are hard-coded with no compile-time link to markup (`GAP-APP-010`). Every keystroke-completing
edit is a full server round trip — there is no client-side calculation.

## Lookup / autocomplete

Product and customer lookups use `AutoCompleteExtender` bound to `SInventoryWebService.asmx`, which returns
**concatenated strings** (`"CODE:Name:(...)"`, `"Name:PackSize"`) that the page splits on `:` and indexes
positionally.

**Avoid repeating this:** a name containing `:` corrupts the parse, and an absent separator throws
`IndexOutOfRangeException` with no handler (`GAP-APP-009` / `BR-SYS-006`). If you add a new lookup, prefer a
proper key/value pair (e.g. two hidden fields) over a colon-delimited string.

## Dropdown loading

Three different helpers exist and insert three different placeholder texts (`LoadDropDownValueWithoutDataBase`
→ `--------Select---------`; `LoadDropDownListData` (`ClsCommonInternalDAL`) → `Select from list`;
`LoadDropDownListData` (`DB_Manager`) → `Select--------------------`). All three insert the placeholder as
`ListItem(text, String.Empty)` at index 0 — **"nothing selected" is the empty string, not `-1`**. Validation
should therefore test `ddl.SelectedValue == ""`, not `.Text == ""` (the latter reads the display text and is a
latent bug — see `GAP-APP-020`). Cascading dropdowns are plain postbacks: parent `AutoPostBack="True"` +
`OnSelectedIndexChanged` reloading the child.

**Avoid repeating this:** don't introduce a fourth dropdown-loading helper with yet another placeholder string;
reuse one of the three existing ones for consistency, and always validate on `.SelectedValue`.

## Validation

The standard shape, used almost everywhere:

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

Server-side, imperative, one message per rule. Client-side restriction (`FilteredTextBoxExtender`) is
presentation only and must be paired with a server-side check — it is never sufficient alone.

**Avoid repeating this:** unguarded `Convert.ToDecimal(textBox.Text)` throws `FormatException` on empty/non-numeric
input with no global handler (`GAP-APP-013`); testing a dropdown's `.Text` instead of `.SelectedValue` produces
validation that can never fail (`GAP-APP-020`); comparing list-item **display text** for control flow (e.g.
`"Partial"`, `"Cash"`) breaks silently if the underlying data value's label is ever renamed (`BR-SYS-009`).

## Postbacks and UpdatePanels

Partial rendering uses `UpdatePanel` (369 declared across the app) rather than any SPA framework. There is no
client-side state management; the grid (see above) is the source of truth and nothing is persisted until Save —
a session timeout loses the whole in-progress document. New screens should follow the same full-postback /
`UpdatePanel` pattern rather than introducing a different client-server interaction model, for consistency with
the rest of the application.

## User messages

```csharp
protected void showMessageBox(string message)
{
    message = message.Replace("'", "\'");
    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
        String.Format("alert('{0}');", message), true);
}
```

This helper (`showMessageBox` / `ShowMessageBox` / `MessageBox`) is duplicated per-page rather than shared.
Messages are typically terminated with `!!` or `!!!` and frequently misspelled (`User name or password
incourect !!!!`). Match the existing tone/format for consistency; do not "correct" existing message text in
unrelated tables/procedures for the same reason misspelled identifiers should not be "fixed."

## Entry / View / Edit triples

Master data follows a three-page pattern: `<Entity>Entry.aspx` (or `Information.aspx`) creates,
`<Entity>View.aspx` lists, `<Entity>Edit.aspx` updates. The View page passes the key to Edit, usually through
`Session`, sometimes via query string. Deletion is generally **soft** (`IsActive = false`, or `ActionStatus` set
to `'Delete'`/`'Cancel'`) — new entities should follow the same soft-delete convention rather than hard `DELETE`.

## Error handling (as it stands, not as it should be)

`Global.asax` has no global handler; `DB_Manager` swallows exceptions and returns `0`; `ClsCommonInternalDAL`
rethrows but resets the stack trace; most page save handlers do not wrap in try/catch. **This is not a pattern
to imitate** — see [security.md](security.md) (`GAP-SEC-008`) and [architecture.md](architecture.md) for what
exists today; new code should not add another silent-failure path on top of the existing ones.

## Dead code

Large commented-out blocks are everywhere: alternative connection strings in `Web.config`, superseded DAL
methods, whole earlier versions of a query kept above the live one. When reading a page for context, the **live**
query is authoritative; treat any commented block as historical and non-authoritative, and don't resurrect it
without checking why it was disabled (some disabled checks, like the chart-of-accounts precondition in delivery
confirmation, may have been turned off unintentionally — `GAP-APP-018`).

## Anti-patterns to avoid repeating (summary)

| ID | Anti-pattern | Why it matters |
|---|---|---|
| `GAP-APP-002` | Screen computes a value one way, persistence layer computes it another way | User-visible total can silently differ from what's stored |
| `GAP-APP-003` | A parameter (`date`) is accepted but never used in the query it's passed to | Silent, undocumented no-op — always verify a parameter is actually consumed |
| `GAP-APP-004` | No availability/bounds check before consuming a finite resource | Overdraft happens silently, no error surfaced |
| `GAP-APP-005` | `UPDATE ... SET col = '<value computed in C#>'` instead of `SET col = col - @delta` | Lost updates under any concurrency |
| `GAP-APP-006` | `SELECT MAX(id)+1` for a new key, outside a transaction | Race condition on concurrent inserts |
| `GAP-APP-007` | Header + detail rows saved as separate autocommitted statements | Partial documents on mid-operation failure |
| `GAP-APP-008` | Catch, rollback, and return a "success-shaped" default (e.g. `0`) | Caller cannot tell failure from a valid empty result |
| `GAP-APP-009` | Composite delimited strings split positionally with no escaping | Breaks on the delimiter appearing in real data |
| `GAP-APP-010` | `Cells[n]` / `Rows[i].Cells[n]` positional addressing | A markup change silently breaks code-behind |
| `GAP-APP-013` | Unguarded numeric parsing (`Convert.ToDecimal`) with no `TryParse` | Unhandled exception on bad input |
| `GAP-APP-016`–`GAP-APP-020` | Screen-computed values passed to a call that ignores them; same message on success and failure paths; control flow keyed on display text | See [architecture.md](architecture.md) traced examples and [`../specs/00-system/business-rules.md`](../specs/00-system/business-rules.md) |
