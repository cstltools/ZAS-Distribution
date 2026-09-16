# Navigation

Full detail: [`../specs/00-system/navigation.md`](../specs/00-system/navigation.md).

## The menu is data, not code

There is no sitemap file, no route table and no hard-coded menu. `MainMasterPage.master.cs::Menu()` builds the
entire navigation as an HTML string, at every non-postback page load, from two tables:

```
tblMainMenu (SL, ManuName, URL, ParantId, IsApprovalPage, ModuleId)
tblMenuDistribution (SL, UserId, MenuSL, Status)
```

Root items are `ParantId IS NULL OR ParantId = ''`; children are `ParantId = <parent SL>`. Depth is capped at
four levels by four nested loops in the code — a fifth level in the data would never render. A non-`Admin` user
sees only rows granted to them in `tblMenuDistribution`; `Session["UserId"] == 1` sees every root item
unfiltered. Menu rendering executes roughly one query per node — no caching. **Confidence: CONFIRMED** (the
mechanism and the loop structure); **INFERRED** (the performance impact of one-query-per-node at real data
volumes).

**`tblMainMenu.URL` is written into `href=` unquoted**, which is a confirmed stored-XSS finding — see
[security.md](security.md) (`GAP-SEC-006`).

## Why the menu tree itself is unknown

**Because the menu is data, the actual tree — what appears under what, in what order, for which users — is
`UNKNOWN` and cannot be recovered from source.** This specification instead documents every page that exists
in the codebase, whether or not any menu currently links to it — see
[`../specs/INVENTORY-PAGES.md`](../specs/INVENTORY-PAGES.md). To recover the real tree, run:

```sql
SELECT SL, ManuName, URL, ParantId, IsApprovalPage, ModuleId
FROM   dbo.tblMainMenu
ORDER  BY ISNULL(NULLIF(ParantId,''),'0'), SL;
```

With that result set, the module grouping used throughout `specs/` and `docs/` (currently **INFERRED** from
page names, DAL classes and tables — see [modules.md](modules.md), `GAP-STR-001`) could be replaced with the
real one derived from `ModuleId`/`ParantId`.

## Known module groupings used in specs/ and docs/ (INFERRED, not real menu data)

The 11 module folders under `specs/` — and the module table in [modules.md](modules.md) — are **inferred**
groupings based on page name, DAL class, and tables touched, not derived from `tblMainMenu.ModuleId`. See
[modules.md](modules.md) for the full list and one-paragraph summary of each.

## Other navigation mechanisms found in code

1. `Response.Redirect("<page>.aspx")` — typically View → Edit, passing the key through `Session`.
2. `window.open(...)` via `ScriptManager.RegisterStartupScript` — every report opens this way, in a centred
   ~950×700 popup.
3. `HyperLink` / `<a href>` in markup — rare (8 `asp:HyperLink` in the whole application).

## Landing page and dashboard

`Login.aspx` → success → `Response.Redirect("CommonUI/HomePage.aspx")`. The Dashboard link is prepended to the
menu outside the loop and gated by its own rule (`LoginName = 'admin'` or `tblUser.IsDashboard = 1`), opening
`../SInventory_UI/DashboardPanel.aspx` in a new tab — a rule independent of the menu-permission table.

`CommonUI/` also contains `Default.aspx`, `TemplatePage.aspx`, `JqueryTab.aspx`, `JqueryAccordian.aspx` and
`AdminMenuPanal.aspx`, none referenced from any other page in the repository — whether they are live is
**UNKNOWN** (`GAP-STR-002`).
