# Authentication, Authorisation and Permission

## Summary

There is **no role model**. Permission is a per-user, per-menu-item row in a table. Because those rows are data
and the repository contains no data, the effective permission matrix for this installation is **UNKNOWN** and
cannot be recovered from source. What *can* be documented — and is, below — is the mechanism.

## Authentication

**Where:** `Solution.Web/Login.aspx` + `Login.aspx.cs` → `PanalBLL.Login` → `PanalClsDAL.Login`.

```csharp
// Library.DAL/PanalCls/PanalClsDAL.cs
string queryString = @" select * from dbo.tblUser U
                        left join tblUserCompanyUnit UC on U.UserId=UC.UserId
                        where LoginName='" + loginName + "' and Password='" + password +
                     "' and UserStatus='active'";
```

CONFIRMED facts:

| Fact | Evidence | Consequence |
|---|---|---|
| Passwords are stored and compared **in plain text** | the query above; `UserDAL.SaveUserInformation` inserts `Password` verbatim | anyone with read access to `tblUser` has every password |
| The login query is **string-concatenated** | same query | authentication is SQL-injectable (`' or '1'='1` style) — `GAP-SEC-005` |
| Only `UserStatus='active'` may log in | same query | the status value is lower-case `'active'`, exactly |
| A user joined to more than one `tblUserCompanyUnit` row produces multiple rows; the code takes `Rows[0]` | `Login.aspx.cs` | which unit a multi-unit user lands in is whatever the database returns first — non-deterministic without an `ORDER BY` |
| There is **no lockout, no attempt counter, no password policy, no expiry, no MFA** | absence in `Login.aspx.cs` and `UserDAL` | — |
| Forms authentication is **disabled** | `<authentication mode="Forms">` is commented out in `Web.config` | protection is session-only |

On success the following go into `Session`:

| Key | Source column | Used for |
|---|---|---|
| `UserId` | `tblUser.UserId` | the authentication guard, menu filtering, `UserId` stamped on rows |
| `IsHead` | `tblUser.IsHead` | INFERRED: head-office vs. field distinction |
| `LoginName` | `tblUser.LoginName` | audit stamps, approval permission lookup, dashboard permission |
| `UserType` | `tblUser.UserType` | only ever compared to `"Admin"` |
| `CentralWareHouse` | `tblUser.CentralWareHouse` | central-warehouse screens |
| `ComUnitId` | `tblUserCompanyUnit.CompanyUnitId` — **set only when `UserType != "Admin"`** | company-unit filtering on nearly every screen |
| `UserTime` | `DateTime.Now.ToString("f")` | displayed in the master page header |

`PanalBLL.LoginLog` then inserts into `tblLoginLog (UserId, LoginName, LoginTime)`. There is **no logout log**.

### The guard

The only authentication check in the application is in the master page:

```csharp
// Solution.Web/MasterPages/MainMasterPage.master.cs
protected void Page_Load(object sender, EventArgs e)
{
    if (Session["UserId"] == null || Session["UserId"].ToString() == "")
        Response.Redirect("../Login.aspx");
    ...
}
```

**256 of 383 pages use this master page. 127 do not.** Those 127 are mostly report viewers and HTML reports,
which take their parameters from the query string. A page without the master page and without its own session
check is reachable unauthenticated by URL. `SPEC-GAPS.md` → `GAP-SEC-002` lists them; the per-page list with a
"checks session itself?" column is in [../INVENTORY-PAGES.md](../INVENTORY-PAGES.md).

Logout is `ImageButton1_Click` on the master page: it nulls `UserId`, `LoginName`, `UserType`, `ComUnitId`,
calls `Session.Abandon()` and redirects to `Login.aspx`.

Session is `InProc` with `timeout="24000"` minutes — **about 16.6 days**. An unattended browser stays logged in.

## Authorisation level 1 — page access (the menu)

The menu **is** the permission system. There is no per-page authorisation attribute, no check inside a page,
and no server-side verification that the user was allowed to reach the URL they requested.

Tables:

| Table | Columns seen in code | Meaning |
|---|---|---|
| `tblMainMenu` | `SL`, `ManuName`, `URL`, `ParantId`, `IsApprovalPage`, `ModuleId` | the menu tree; `URL` is the physical `.aspx` path |
| `tblMenuDistribution` | `SL`, `UserId`, `MenuSL`, `Status` | one row = "this user may see this menu item" |

Rendering (`MainMasterPage.master.cs::Menu()`):

```csharp
aDataTableMenu = Convert.ToInt32(Session["UserId"]) == 1
    ? aDal.MainMenu()                                        // UserId 1 sees EVERYTHING
    : aDal.MainMenu(Convert.ToInt32(Session["UserId"]));     // everyone else sees granted rows only
```

The tree is built by four nested loops — root → sub → sub-sub → sub-sub-child — so the menu supports **exactly
four levels**. Each level repeats the same `INNER JOIN tblMenuDistribution` filter, except for `UserId == 1`.

CONFIRMED consequences:

- **`UserId = 1` is a hard-coded superuser.** Not `UserType = 'Admin'` — the literal integer `1`.
- **Hiding a menu item does not protect the page.** Nothing stops a user pasting the URL. This is the single
  most important security finding in this specification (`GAP-SEC-001`).
- A separate check exists only for the dashboard link:
  `LoginName == "admin"` OR `tblUser.IsDashboard = 1` (`PanalClsDAL.GetDashboardPermission`). Note this is a
  *second* hard-coded superuser identity, by login name this time.

Granting is done on `Solution.Web/CommonUI/UserPermission.aspx`: pick a user, tick menu rows, save →
`PanalDAL.SaveMainMenu` inserts into `tblMenuDistribution` with `Status = 'True'`. Revoking
(`PanalDAL.MenuPermissionRemove`) deletes the row **and all descendants**, three levels deep, with four
separate `DELETE` statements.

## Authorisation level 2 — company unit

`tblUserCompanyUnit (UserId, CompanyUnitId, CWHPermission, NationalReportPermission)` is maintained on
`Solution.Web/SInventory_UI/DCUserPermission.aspx` (`DCUserPermissionDal`). Saving **deletes all of the user's
rows first**, then inserts the new set.

`CWHPermission` (central-warehouse access) and `NationalReportPermission` (may see national, i.e. all-unit,
reports) are the only two boolean capability flags in the entire application.

Note `DCUserPermissionDal.GetDCList` unions company units with warehouses using a **hard-coded id of 15** for
warehouse rows:

```sql
SELECT ComUnitId, ComUnitCode + ':' + ComUnitName FROM tblCompanyUnit
UNION ALL
SELECT 15, WearhouseCode + ':' + WearhouseName FROM tblWearhouse
```

Every warehouse therefore collapses to unit 15. Whether that is intentional is **UNKNOWN**.

## Authorisation level 3 — approval actions

Approval screens do not hard-code who may approve. The allowed actions are resolved at run time from four
tables joined to the menu row for the current page:

| Table | Role |
|---|---|
| `tblAction` | the action vocabulary (`ActionId`, `ActionText`, `IsShow`) |
| `tblActionSteps` | a named approval step (`ASId`) |
| `tblActionStepDetail` | which actions belong to a step (`ASId`, `ActionId`) |
| `tblActionPageWiseStep` | which step applies to which menu item (`PWASId`, `ManuSL`, `ASId`) |
| `tblActionUserWiseApproval` | which action a given user may take on a given menu item (`UWAId`, `UserId`, `LoginName`, `ManuSL`, `ActionId`) |

```csharp
// Library.DAL/InternalCls/ClsApprovalAction.cs
SELECT * FROM dbo.tblActionUserWiseApproval UA
INNER JOIN dbo.tblActionPageWiseStep PS ON UA.ManuSL = PS.ManuSL
INNER JOIN dbo.tblActionSteps       SA ON PS.ASId    = SA.ASId
INNER JOIN       tblActionStepDetail AD ON SA.ASId   = AD.ASId AND UA.ActionId = AD.ActionId
INNER JOIN dbo.tblMainMenu          MM ON UA.ManuSL  = MM.SL
WHERE UA.LoginName = @userName AND MM.URL LIKE '%' + @pageName + '%'
```

`LoadActionControlByUser(RadioButtonList rdl, pageName, userName)` then binds every row of `tblAction` where
`IsShow = 1` into the radio list and enables exactly the one matching `ActionId`. Everything else is disabled —
**except an item whose text is `"Cancel"`, which is left enabled**, and the item matching the row's
`ActionCondition`, which is force-disabled. If the query returns nothing, **every action is disabled**, so a
user with no grant sees the screen but can do nothing.

Two facts to be careful about:

- The page is matched by `MM.URL LIKE '%<pageName>%'`. Two pages whose names are substrings of one another
  (for example `SalesReport.aspx` and `AllSalesReport.aspx`) will match each other's rule. (CONFIRMED risk,
  `BR-SYS-007`.)
- This lookup keys on `LoginName`, whereas menu permission keys on `UserId`. Renaming a login silently drops
  approval rights while leaving menu rights intact.

Configuration screens: `CommonUI/ApprovalStepPageWise.aspx` (step ↔ page) and
`CommonUI/UserWiseApprovalPermission.aspx` (user ↔ page ↔ action). Saving the latter deletes the user's
existing rows for that menu item first, then inserts.

## Authorisation level 4 — one-off flags

| Flag | Table | Used for |
|---|---|---|
| `IsCreditApproval` | `tblUser` | credit-limit approval screens |
| `IsHead` | `tblUser` | put in session; INFERRED head-office distinction |
| `IsDashboard` | `tblUser` | shows the Dashboard menu link |
| `IsAppsUser` | `tblUser` | excluded from the user list (`UserDAL`: `ISNULL(IsAppsUser,0) = 0`) — INFERRED: mobile-app service accounts |
| `CWHPermission` | `tblUserCompanyUnit` | central-warehouse screens |
| `NationalReportPermission` | `tblUserCompanyUnit` | national (all-unit) reports |
| `UserType` | `tblUser` | branches only on the literal `"Admin"` (skips setting `Session["ComUnitId"]`) |

## The permission matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| `UserId = 1` | all | all | all | all | all | all | all | all | all | all |
| `LoginName = 'admin'` | + Dashboard | — | — | — | — | — | — | — | — | — |
| every other user | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

The two superuser rows are CONFIRMED from code. Every other cell is UNKNOWN **by nature, not by omission**: it
is determined by rows in `tblMenuDistribution`, `tblUserCompanyUnit` and `tblActionUserWiseApproval`.

**To complete this matrix**, supply the output of:

```sql
SELECT SL, ManuName, URL, ParantId, IsApprovalPage, ModuleId FROM tblMainMenu ORDER BY ParantId, SL;
SELECT UserId, LoginName, UserName, UserType, UserStatus, IsHead, IsCreditApproval, IsDashboard FROM tblUser;
SELECT UserId, MenuSL, Status FROM tblMenuDistribution;
SELECT UserId, CompanyUnitId, CWHPermission, NationalReportPermission FROM tblUserCompanyUnit;
SELECT * FROM tblAction; SELECT * FROM tblActionSteps; SELECT * FROM tblActionStepDetail;
SELECT * FROM tblActionPageWiseStep; SELECT * FROM tblActionUserWiseApproval;
```
