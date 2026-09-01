# Navigation

## The menu is data, not code

There is no sitemap file, no route table and no hard-coded menu. `MainMasterPage.master.cs` builds the entire
navigation as an HTML string from two tables at every non-postback page load.

```
tblMainMenu (SL, ManuName, URL, ParantId, IsApprovalPage, ModuleId)
tblMenuDistribution (SL, UserId, MenuSL, Status)
```

**Therefore the menu tree of this installation is UNKNOWN.** It cannot be derived from the repository. This
specification instead documents every page that exists, whether or not it appears in someone's menu — see
[../INVENTORY-PAGES.md](../INVENTORY-PAGES.md).

## How it is built

```csharp
// MainMasterPage.master.cs :: Menu()
aDataTableMenu = Session["UserId"] == 1 ? aDal.MainMenu()               // root items, unfiltered
                                        : aDal.MainMenu(userId);        // root items the user was granted
for each root item
    <li><a href="{URL}"><i class="halflings white home"></i>{ManuName}</a>
    aDal.SubItem(SL[, userId])                 // level 2
        for each  -> aDal.SubSubItem(SL[, userId])          // level 3
            for each -> aDal.SubSubChildItem(SL[, userId])  // level 4
    </li>
menu.InnerHtml = <the assembled string>
```

Root items are those with `ParantId IS NULL OR ParantId = ''`. Children are `ParantId = <parent SL>`.

**Depth is capped at four levels** by the four nested loops. A fifth level in the data would simply never
render.

The Dashboard link is prepended outside the loop, and is governed by its own rule
(`LoginName = 'admin'` or `tblUser.IsDashboard = 1`), opening `../SInventory_UI/DashboardPanel.aspx` in a new
tab.

## Known cost

Menu rendering executes **one query per node**. A user with, say, 12 root items, 60 sub items and 200
sub-sub items causes ~270 separate round trips through
`ExecuteAllSqlQueryByStoreProcedure` on **every page load**. There is no caching. (CONFIRMED from the loop
structure; the performance impact is INFERRED.)

## URL handling

`tblMainMenu.URL` is written into the `href` **unquoted**:

```csharp
manurHtml + @" <li><a href=" + row["URL"].ToString().Trim() + "><i class=" + parent + "></i>" + ...
```

A URL containing a space or a quote breaks the markup. A URL containing markup is injected into the page
(stored XSS via the menu table). `SPEC-GAPS.md` → `GAP-SEC-006`.

## Landing page

`Login.aspx` → on success → `Response.Redirect("CommonUI/HomePage.aspx")`.

`CommonUI/` also contains `Default.aspx`, `TemplatePage.aspx`, `JqueryTab.aspx`, `JqueryAccordian.aspx` and
`AdminMenuPanal.aspx`. Which of these are live and which are scaffolding is **UNKNOWN** — they are not
referenced from any other page in the repository.

## Non-menu navigation found in code

Three mechanisms, all documented per page in each spec's section 4:

1. `Response.Redirect("<page>.aspx")` — typically View → Edit, passing the key through `Session`.
2. `window.open(...)` via `ScriptManager.RegisterStartupScript` — every report is opened this way, in a
   centred popup roughly 950×700.
3. `HyperLink` / `<a href>` in markup — rare (8 `asp:HyperLink` in the whole application).

## Recovering the real menu

Run and attach the result:

```sql
SELECT SL, ManuName, URL, ParantId, IsApprovalPage, ModuleId
FROM   dbo.tblMainMenu
ORDER  BY ISNULL(NULLIF(ParantId,''),'0'), SL;
```

With that one result set, the module grouping in this specification (currently INFERRED from page names) can be
replaced with the real one, and section 4 of every page spec can name its actual menu path.
