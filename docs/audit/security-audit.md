# Security Audit — ZAS Distribution (SalesRoll)

Scope: static source review only. Builds on the prior reverse-engineering audit's confirmed findings
(`specs/FINAL-REPORT.md` §6, `specs/SPEC-GAPS.md` §C, `GAP-SEC-001..009`). Every GAP-SEC-* cited below was
re-verified against the current source during this pass; new findings get `BUG-SEC-*` IDs.

Labels: **CONFIRMED** (read the exact code), **INFERRED** (reasoned from adjacent code, not fully traced),
**SUSPECTED** (pattern seen, not proven exploitable), **UNKNOWN** (needs DB/runtime access).

---

## 1. Re-verification of prior findings

| GAP ID | Re-verified | Evidence |
|---|---|---|
| `GAP-SEC-003` (hardcoded `sa` creds) | **CONFIRMED** | `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs:21-22` — live: `UserId = "sa"`, `Password = "N@fcoZ@s2023#"`. Lines 12-31 also carry five more commented-out `sa`/`sa1234` server/password pairs, i.e. prior real credentials left in history. |
| `GAP-SEC-005` (systemic SQLi via `ExecuteAllSqlQueryByStoreProcedure`) | **CONFIRMED**, mechanism refined | The DAL does **not** literally call a proc named `ExecuteAllSqlQueryByStoreProcedure` in most places checked — the actual pattern is `ClsCommonInternalDAL.DataContainerDataTable(queryString)` (`Library.DAL/InternalCls/ClsCommonInternalDAL.cs`) built from raw concatenated strings, used ~1,400 times per the prior count. The login query is confirmed verbatim in `Library.DAL/PanalCls/PanalClsDAL.cs:14`: `"...where LoginName='" + loginName + "' and Password='" + password + "' and UserStatus='active'"`. |
| `GAP-SEC-007` (plaintext passwords) | **CONFIRMED** | Same line as above — password compared as a literal string in the WHERE clause; no hash/salt anywhere in `PanalClsDAL`/`UserDAL`. |
| `GAP-SEC-002` (127/383 pages no auth guard) | **CONFIRMED (mechanism)**, count not re-run | `Solution.Web/MasterPages/MainMasterPage.master.cs:20-22` is the only session gate in the app: `if (Session["UserId"] == null || Session["UserId"].ToString() == "") Response.Redirect("../Login.aspx");`. Any page not using this master, or not repeating an equivalent check itself, is reachable unauthenticated. Did not re-walk all 383 pages; trusted the prior per-page inventory in `specs/INVENTORY-PAGES.md`. |
| `GAP-SEC-004` (18 unauthenticated ASMX methods) | **CONFIRMED with nuance** | See §3 API cross-check below — the blanket "checks no session" statement is not uniformly true; several methods do read `Session["UserType"]`/`Session["CompanyId"]` for row-scoping, but none of them verify the caller is authenticated first, and several (`HelloWorld`, `GetCustomerInfo`, `GetProduct2`, `GetProduct3`, `GetProductByMenufracturer`) have no session touch at all. Net effect (anonymous access to a full customer/product/price/employee search surface) stands. |
| `GAP-SEC-008` (debug/no customErrors/16-day session) | **CONFIRMED** | `Solution.Web/Web.config:90` `<compilation debug="true" targetFramework="4.0">`; no `<customErrors>` element anywhere in the file (grepped, zero matches); `Web.config:106` `<sessionState mode="InProc" timeout="24000" .../>` = 16.67 days; `Web.config:104` forms auth ticket timeout is a separate, shorter 2880 min (2 days) — the two are inconsistent, session outlives the auth cookie by 8x. |
| `GAP-SEC-009` (no horizontal access control / IDOR) | **CONFIRMED**, concrete instance found | See BUG-SEC-001 below — a specific, traced example of scope bypass. |
| `GAP-SEC-006` (stored XSS via `tblMainMenu.URL`) | not re-traced this pass (menu render code not re-read); no reason to doubt prior finding | — |
| `GAP-SEC-001` (menu is the permission model) | **CONFIRMED**, and the DAL enforces it partially | `PanalClsDAL.cs:33-42` — `MainMenu(int userId)` INNER JOINs `tblMenuDistribution` by `UserId`, so hidden-menu items are also absent from the query result for non-admin users (real gate, not just hidden UI) *for top-level menu rendering*. But `SubItem` overloads (`.cs:43-60`) and page-level access are not re-checked on direct URL request — reaching a page directly bypasses this filter entirely, which is exactly `GAP-SEC-009`'s point. |

## 2. New finding: frontend-only permission gate with no backend re-check

### BUG-SEC-001 — "National / all-company" report scope is enabled by a disabled checkbox only — HIGH — CONFIRMED

`Solution.Web/SInventory_UI/AllSalesReport.aspx.cs`:

- `Page_Load` (lines 17-31): for non-Admin users, `CheckBox1.Enabled = false` — pure client-side/rendering
  disablement of the "national report" (all-companies) scope toggle. Admins get it enabled.
- `viewRptButton_Click` (lines 202-228): the click handler **never reads `Session["UserType"]` again**. It
  branches purely on `CheckBox1.Checked` (line 223) and, if checked, opens
  `AllSalesreportViewer.aspx?NationalReport=1&fromDate=...&toDate=...` — a URL with **no company/unit
  scoping parameter at all**.
- A `Visible=false`/`Enabled=false` WebForms control does not stop a crafted raw HTTP POST from posting
  `CheckBox1=on` in the form body; ASP.NET WebForms does not reject postback values for disabled controls by
  itself in all versions, and even if the browser click path is blocked, nothing server-side ever re-validates
  the role before trusting `CheckBox1.Checked`.
- Net effect: a non-Admin user (or anyone who can replay/craft the postback) can reach the same
  all-companies report URL an Admin gets, and that downstream viewer page's own auth/scope posture was not
  re-verified in this pass (cross-reference `GAP-SEC-002`/`GAP-SEC-009`).
- This is the concrete, traced instance of the "hidden button ≠ backend check" pattern the task asked to look
  for. The same `Session["UserType"] != "Admin"` → `control.Enabled = false` shape recurs verbatim (same two
  lines) in at least `ASMReport.aspx.cs:177-179`, `AreaWiseMonthlyInventoryReport.aspx.cs:49-51`,
  `ASMWiseProductSalesReport.aspx.cs:50-52/90-92/190-192`, `B2BTransferChallanReport.aspx.cs:49-51`,
  `B2BTransferChallanRcvReport.aspx.cs:49-51`, `BatchWiseCollectionReport.aspx.cs:54-56` — each needs the
  same check (does the corresponding click/export handler re-verify role before honoring the control's
  state?). Only `AllSalesReport.aspx.cs` was traced end-to-end in this pass; the rest are **SUSPECTED** of the
  same gap and listed for follow-up, not confirmed individually.

## 3. Role/permission enforcement — is it frontend-only elsewhere?

Searched `Solution.Web/SInventory_UI/*.aspx.cs` for `.Visible = false` (420 hits across 88 files) and
`Session["UserType"]`/admin-text comparisons. Findings:

- The overwhelming majority of `.Visible = false` hits are **not** permission-related — they hide/show grid
  columns or template rows based on data state (e.g. `Dashboard.aspx.cs`, `CustomerPayment.aspx.cs`), which is
  normal UI behaviour, not an authorization control. Spot-checked `CustMasterView.aspx.cs:52-57` — that one
  instance is commented-out dead code (`editImageButton.Visible = ...`), not live.
- The genuine permission-flavoured pattern found is the `Session["UserType"] != "Admin"` → `.Enabled = false`
  shape documented in BUG-SEC-001, which is real and repeats across at least 7 files with no accompanying
  backend re-check observed in the one traced end-to-end.
- No RBAC/claims middleware, no `[Authorize]`-equivalent attribute, and no page-level permission filter exists
  anywhere in the codebase (confirmed absent in `GAP-STR-*`/prior audit and not contradicted here). Enforcement
  is: (a) whether a page uses `MainMasterPage` (session-exists check only, not role-based), and (b) ad hoc
  `Session["UserType"]`/`Session["LoginName"] == "admin"` string comparisons scattered per-page, each one a
  candidate for the same bypass as BUG-SEC-001 unless proven otherwise. **INFERRED**: this is a systemic
  pattern, not an isolated bug, but only one instance was traced to a concrete bypass.

## 4. Audit-log / who-changed-what mechanism

- **No general audit-log table or interceptor exists.** No DAL/BLL class wraps writes with a generic
  before/after change log; no `Global.asax` hook records mutations (confirmed empty event bodies, per prior
  audit).
- What partially exists instead, all ad hoc, per-table, manually populated by whichever screen wrote the row:
  - `tblLoginLog` — login events only. `PanalClsDAL.LoginLog(userId, LoginName, loginTime)`
    (`Library.DAL/PanalCls/PanalClsDAL.cs:26-32`) does a raw concatenated `INSERT INTO dbo.tblLoginLog`.
    Records *that* someone logged in, not what they did afterward.
  - `tblOrderDel` carries `DeleteBy`/`DeleteDate` columns, read back by
    `Library.DAL/SInventory_DAL/AuditReportOneDal.cs` (`GetDeleteOrderReport`/`GetDeleteOrderNationalReport`)
    — this is a manually-maintained "who deleted this order" trail for exactly one table, not a general
    mechanism. Whether every code path that deletes an order populates these columns wasn't traced.
  - Files named `UserCredentialChangeDal.cs` and `PaymentReverseDal.cs` (both matched a `Log`/`Audit` grep)
    were not opened this pass — **UNKNOWN** whether they log anything general-purpose or are similarly
    narrow/manual. Flagging for follow-up rather than asserting.
- **Conclusion: CONFIRMED — there is no systemic audit trail.** Any "who changed this row" question for the
  ~120 other tables cannot be answered from the application; it would require row-level `INSERT`/`UPDATE`
  triggers or CDC in the database itself, which is outside this repo (`GAP-DB-002`).

## 5. Bug register (security)

| ID | Title | Severity | Confidence |
|---|---|---|---|
| `GAP-SEC-003` | Hardcoded `sa` credentials, live in source | Critical | Confirmed |
| `GAP-SEC-005` | Systemic SQL injection via string-concatenated queries incl. login | Critical | Confirmed |
| `GAP-SEC-007` | Plaintext password storage/comparison | Critical | Confirmed |
| `GAP-SEC-002` | 127/383 pages with no authentication guard | Critical | Confirmed (mechanism; count trusted from prior inventory) |
| `GAP-SEC-004` | Unauthenticated/anonymous-reachable ASMX search methods | High | Confirmed, with nuance (some do read session for scoping, none gate on it; several have zero check) |
| `GAP-SEC-008` | `debug=true`, no `customErrors`, 16-day session vs 2-day auth cookie mismatch | High | Confirmed |
| `GAP-SEC-009` | No horizontal access control (IDOR) | High | Confirmed |
| `BUG-SEC-001` | Role-gated report scope enforced only by `Control.Enabled=false`, no server-side re-check on submit | High | Confirmed (`AllSalesReport.aspx.cs`); pattern suspected in 6+ sibling report pages, not individually traced |
| `GAP-SEC-006` | Stored XSS via unquoted `tblMainMenu.URL` in menu render | Medium | Not re-traced this pass, no reason to doubt |
| `GAP-SEC-001` | Menu table doubles as the permission model; direct-URL access bypasses it | High | Confirmed |
| `BUG-SEC-002` | No systemic audit-log/change-tracking mechanism anywhere in the app; only 2 narrow, manually-populated exceptions (`tblLoginLog`, `tblOrderDel.DeleteBy/DeleteDate`) | Medium | Confirmed |
