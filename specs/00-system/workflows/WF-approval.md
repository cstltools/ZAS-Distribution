# WF-004 — The generic approval workflow

One mechanism serves every approval screen in the application. Learn it once and all 14 approval pages become
readable.

**Primary evidence**

| | |
|---|---|
| Engine | `Library.DAL/InternalCls/ClsApprovalAction.cs` — `LoadActionControlByUser`, `LoadForApprovalByUserCondition` |
| Configuration screens | `Solution.Web/CommonUI/ApprovalStepPageWise.aspx`, `Solution.Web/CommonUI/UserWiseApprovalPermission.aspx` |
| Configuration DAL | `Library.DAL/Panal_DAL/PanalDAL.cs` |
| Canonical consumer | `Solution.Web/SInventory_UI/DestroyProductApproval.aspx(.cs)` |
| Tables | `tblAction`, `tblActionSteps`, `tblActionStepDetail`, `tblActionPageWiseStep`, `tblActionUserWiseApproval`, `tblMainMenu` |

---

## The data model

```
tblAction              (ActionId, ActionText, IsShow)          the action vocabulary — e.g. Accept, Reject, Cancel
   ▲
   │
tblActionStepDetail    (ASId, ActionId)                        which actions belong to a step
   ▲
   │
tblActionSteps         (ASId, …)                               a named approval step
   ▲
   │
tblActionPageWiseStep  (PWASId, ManuSL, ASId)                  which step governs which menu item (= page)
   ▲
   │
tblMainMenu            (SL, ManuName, URL, …)                  the page
   ▲
   │
tblActionUserWiseApproval (UWAId, UserId, LoginName, ManuSL, ActionId)
                                                               which action a given user may take on that page
```

**None of these rows are in the repository.** The action vocabulary, the steps, and every grant are data —
see [../../SPEC-GAPS.md](../../SPEC-GAPS.md) → `GAP-SEC-001`.

## Configuration

**Step ↔ page** (`CommonUI/ApprovalStepPageWise.aspx`): pick a menu item, pick a step, save.
`PanalDAL` deletes `tblActionPageWiseStep WHERE ManuSL = @sl` first, then inserts — so a page has at most one
step.

**User ↔ page ↔ action** (`CommonUI/UserWiseApprovalPermission.aspx`): pick a user, pick a menu item, pick one
action from a radio list, save. Again delete-then-insert:

```sql
DELETE tblActionUserWiseApproval WHERE UserId = @UserId AND ManuSL = @ManuSL;
INSERT INTO dbo.tblActionUserWiseApproval (UWAId, UserId, LoginName, ManuSL, ActionId) VALUES (…);
```

**A user therefore has exactly one permitted action per page.** Multi-action grants are not representable.
(CONFIRMED.)

## Run time — what an approval page does

### 1. Work out what this user may do

```csharp
string filename = Path.GetFileName(Request.Path);        // e.g. "DestroyProductApproval.aspx"
string userName = Session["LoginName"].ToString();
aDal.LoadApprovalControlDAL(RadioButtonList, filename, userName);   // → ClsApprovalAction.LoadActionControlByUser
```

```sql
SELECT * FROM dbo.tblActionUserWiseApproval UA
INNER JOIN dbo.tblActionPageWiseStep PS ON UA.ManuSL  = PS.ManuSL
INNER JOIN dbo.tblActionSteps        SA ON PS.ASId    = SA.ASId
INNER JOIN       tblActionStepDetail AD ON SA.ASId    = AD.ASId AND UA.ActionId = AD.ActionId
INNER JOIN dbo.tblMainMenu           MM ON UA.ManuSL  = MM.SL
WHERE UA.LoginName = @userName AND MM.URL LIKE '%' + @pageName + '%'
```

Then the radio list is bound from **all** visible actions and selectively disabled:

```csharp
rdl.DataSource = SELECT * FROM tblAction WHERE IsShow = 1;   // every action is shown
if (rows returned) {
    for each item:
        item.Enabled = (item.Value == row["ActionId"]);       // only the granted action is enabled
        if (!item.Enabled && item.Text != "Cancel")           // ...except "Cancel", always left enabled
            item.Enabled = false;
        if (item.Text == row["ActionCondition"])              // the step's current condition is disabled
            item.Enabled = false;
} else {
    for each item: item.Enabled = false;                      // no grant → nothing is enabled
}
```

Three behaviours worth stating plainly:

- Actions the user may **not** take are still **rendered**, only disabled. Disabled radio buttons are a
  client-side affordance; the page does **not** re-check the grant on postback. (CONFIRMED — `GAP-SEC-009`.)
- **`"Cancel"` is always available**, by string comparison, regardless of grant (`BR-SYS-008`).
- The page is matched by `LIKE '%<filename>%'`, so `SalesReport.aspx` matches any menu URL containing that
  substring (`BR-SYS-007`).

### 2. List the records that are at this user's step

```csharp
string actionStatus = aDal.LoadForApprovalConditionDAL(filename, user);   // = row["ActionCondition"]
aDataTable = aDal.GetSpecialApproveOrder(actionStatus);                   // rows whose status matches
loadGridView.DataSource = aDataTable;
```

So the **worklist itself is filtered by the step's `ActionCondition`**: a user sees only records currently in
the status their step acts on. That is how multi-step approval chains are expressed — not by an explicit step
number on the record, but by matching its current status text.

### 3. Submit

```csharp
if (RadioButtonList.SelectedValue == "")   → alert "Please Choose any of the operation !!!"
else if (Validation())
    for each grid row:
        if (chkSelect.Checked):
            dao.ActionStatus = RadioButtonList.SelectedItem.Text;    // ← the status IS the action's label
            dao.ApproveBy    = Session["LoginName"];
            dao.ApproveDate  = DateTime.Today;
            if (aDal.ApprovalUpdateDAL(dao))                          // UPDATE the record's status
                if (RadioButtonList.SelectedItem.Text.Trim() != "Cancel")
                    <side effects>                                   // stock update, journal posting
```

For `DestroyProductApproval` the side effects are:

```csharp
DataTable t = aDal.FetchDcFreezeQty(dao.DcFreezeId);
decimal qty = t.Rows[0].Field<decimal>("StockQty") - Convert.ToDecimal(DataKeys[i][2]);
aDal.UpdateFreezeStockQty(dao.DcFreezeId, qty);      // absolute overwrite, computed in C#
aDal.PostJournal(DataKeys[i][0]);                    // EXEC sp_Sales_ProductDestroyPosting
```

`ApproveDate` is `DateTime.Today` — **date only, no time**. Two approvals on the same day are
indistinguishable by timestamp. (CONFIRMED.)

## Screens that use this engine

`CustomerCreditLimitApproval`, `DestroyProductApproval`, `DiscountApproval`, `FixedDiscountApproval`,
`ManualOrderApproval`, `NonTranscationalInvoiceApproval`, `PettyCashApproval`, `ProductQuotedPriceApproval`,
`ProUnitPriceApprove`, `ReturnustomerByInvoiceApproval` (sic), `SampleProductIssueApproval`,
`SlaveInfoApproval`, `TradeMaterialTransferApproval`, `WarehouseStockInApproval`.

Each has its own `LoadApprovalControlDAL` / `LoadForApprovalConditionDAL` wrapper in its own DAL class, all
delegating to the same `ClsApprovalAction`.

## What is UNKNOWN

| Question | Why it cannot be answered here |
|---|---|
| What are the available actions? | `tblAction` rows |
| What are the approval steps, and their order? | `tblActionSteps` + `tblActionStepDetail` rows |
| Which step governs which page? | `tblActionPageWiseStep` rows |
| Who may approve what? | `tblActionUserWiseApproval` rows |
| What status does a record start in? | table DDL defaults |
| What does each posting procedure do, and is it idempotent? | procedure bodies |

## Test cases

| ID | Scenario | Expected |
|---|---|---|
| `TC-WF004-01` | User with no grant opens an approval page | every radio item disabled; worklist filtered by an empty `ActionCondition` |
| `TC-WF004-02` | User with a grant, submits without choosing | `Please Choose any of the operation !!!` |
| `TC-WF004-03` | User chooses their granted action on 3 ticked rows | 3 status updates + 3 postings |
| `TC-WF004-04` | User chooses `Cancel` | status updated, **no** stock or journal side effect |
| `TC-WF004-05` | Grant is removed while the page is open, then submit | **the action still succeeds** — no re-check on postback |
| `TC-WF004-06` | `tblAction.ActionText` renamed from `Accepted` to `Approved` | new records become invisible to every `ActionStatus = 'Accepted'` filter |
| `TC-WF004-07` | Two pages whose names are substrings of one another | both match the same `LIKE` rule |
