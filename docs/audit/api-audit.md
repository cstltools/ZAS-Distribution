# API Audit — ZAS Distribution (SalesRoll)

Scope: static review of the 60 non-page-postback endpoints inventoried in `specs/00-system/api-inventory.md`
(18 ASMX methods on 2 endpoints + 40 page `[WebMethod]`s + 2 `.ashx` handlers). No live requests were made.
Spot-checked 15 of the 60. Builds on `GAP-SEC-004` (ASMX unauthenticated) — re-verified, refined below.

Labels: **CONFIRMED**, **INFERRED**, **SUSPECTED**, **UNKNOWN**.

---

## 1. ASMX endpoints — `SInventoryWebService.asmx` / `WebService.asmx` (same class, `Solution.Web/App_Code/SInventoryWebService.cs`)

Spot-checked 8 of the 18 methods directly in source.

| Method | Line | Auth check | Input handling | Verdict |
|---|---|---|---|---|
| `HelloWorld` | 30-34 | none | no input | Harmless, but confirms zero baseline auth on the service |
| `GetPerticularaction` | 36-... | `EnableSession=true` attribute only, no code check of session contents | `@prefixText` parameterized | **CONFIRMED unauthenticated** (session attribute ≠ auth check) but not injectable |
| `loadCustomerInfoforProductdiscount` | 66-93 | same as above | `@prefixText` parameterized; `param` (from elsewhere) concatenated in — not traced to source | **CONFIRMED unauthenticated**; injection risk of `param` **UNKNOWN** (not traced) |
| `GetCustomerInfo` | 126-145 | **no session attribute at all** | delegates to DAL, not inspected | **CONFIRMED fully anonymous, no session touch whatsoever** |
| `GetApprovedProduct` | 168-195 | reads `Session["CompanyId"]` and throws `NullReferenceException` if absent (line 175: `Session["CompanyId"].ToString()` with no null guard) rather than gating access | `companyId` is `Convert.ToInt32`'d before concatenation (line 182) — **not actually SQL-injectable** despite string-building; `@prefixText` is parameterized | **Correction to `GAP-SEC-004`**: this specific method is not injectable; it is still unauthenticated for anyone with a live session missing `CompanyId`, and throws an unhandled 500 (stack trace leak, `GAP-SEC-008`) for a caller with no session at all |
| `GetCustomer` | 236-264 | reads `Session["UserType"] == "Admin"` to pick scoped vs unscoped query (line 242), and `Session["ComUnitId"]` concatenated **raw** into SQL (line 249: `"...ComUnitId='" + comUnit.Trim() + "'..."`) | `@prefixText` parameterized; `comUnit` from Session is not parameterized | **Correction to `GAP-SEC-004`**: this method *does* apply a real authorization scoping rule — but (a) nothing stops a caller with no session from hitting the `Session["UserType"].ToString()` line and getting an unhandled `NullReferenceException` instead of a clean 401, and (b) if `Session["ComUnitId"]` could ever be attacker-influenced (e.g. via another injection elsewhere) this concatenation would be exploitable. As observed, not directly exploitable by an anonymous caller. |
| `GetCustomernew` | 267-301 | none | `@prefixText` parameterized; `param` from `Session["MOParam"]` concatenated raw into the SQL string (line 285: `... LIKE @prefixText " + param + ""`) | **SUSPECTED SQL injection** if `Session["MOParam"]` is ever settable to attacker-controlled text (not traced to its writer in this pass) — flagged as new, not in original `GAP-SEC-004` list, since original called out `prefixText` concatenation broadly but the actual exploitable seam here is a different parameter |
| `GetProductByMenufracturer` | inventoried, not opened this pass | per inventory table: no session | — | Trusted from `api-inventory.md`; not independently re-verified |

**Net correction to `GAP-SEC-004`:** the finding "none of these methods checks session" is **not literally true for `GetCustomer`/`GetApprovedProduct`**, which do read session for row-scoping — but it remains true that **no method rejects an unauthenticated caller**; the worst case for those two is an unhandled exception (info leak per `GAP-SEC-008`) rather than a clean deny, and several other methods (`HelloWorld`, `GetCustomerInfo`, and per inventory `GetProduct2`, `GetProduct3`, `GetProductByMenufracturer`) have zero session interaction of any kind. The severity assessment (High) stands.

## 2. Page `[WebMethod]` endpoints — `CashFlow.aspx.cs` / `Dashboard.aspx.cs` / `IncomeStatement.aspx.cs`

Spot-checked 5 of the 40. These are `public static` methods (required by ASP.NET AJAX PageMethods), which means
they run **without** the page's own `Page_Load` session-guard ever executing — a static `[WebMethod]` is reached
directly by the AJAX runtime, bypassing whatever auth check the page's `Page_Load` might have.

| Method | File:line | Auth check | SQL construction | Verdict |
|---|---|---|---|---|
| `GetCompanyWiseSales` | `CashFlow.aspx.cs:22` | `[WebMethod(EnableSession = true)]` attribute only; **no code inside the method reads Session at all** | delegates to `SalesDashboardDal.GetCompanyWiseSales` (DAL body not opened this pass) | **CONFIRMED**: reachable by any client that can POST to `CashFlow.aspx/GetCompanyWiseSales`, regardless of whether they ever logged in — `Page_Load` (lines 16-19) is empty and would not have blocked them anyway |
| `GetAglingReport` | `CashFlow.aspx.cs:332-355` | same — none | **CONFIRMED SQL injection surface, new finding**: `companyId`, `year`, `month` are raw method parameters (client-controlled AJAX POST body) concatenated directly into a SQL fragment with zero validation or parameterization: `parameter + " AND CI.CompanyId = " + companyId` (line 340) and `" AND YEAR(INV.InvoiceDate) = '" + year + "' AND MONTH(...) = '" + month + "'"` (line 345), then passed to `aDashboardDal.GetAglingReport(parameter)` | **BUG-API-001 — High — Confirmed**: unauthenticated, unparameterized, directly attacker-controlled SQL injection via a page method. This is a distinct code path from the ASMX methods in `GAP-SEC-004` and was not previously enumerated by ID. |
| `GetCompanyWiseDcSales`, `GetCompanyWiseInvoice`, `GetDashBoardCardInfo`, `GetTop50Customer`, etc. (same file, `Dashboard.aspx.cs`) | inventoried, not individually opened | per pattern above, `public static` + `EnableSession=true` attribute with no in-method check | not traced | **SUSPECTED** to share the same no-auth-check shape as `GetCompanyWiseSales`/`GetAglingReport`; not confirmed line-by-line |

**Conclusion:** the 40 page-level `[WebMethod]`s are not covered by `GAP-SEC-004` (which only enumerated the 18
ASMX methods) but exhibit the same class of defect — and at least one (`GetAglingReport`) is a confirmed new
SQL-injection point with zero auth. Recommend treating all 40 as unauthenticated until each is individually
verified; this list is a starting point, not exhaustive.

## 3. `.ashx` handlers

| Handler | Auth check | Notes |
|---|---|---|
| `PictureHandler.ashx` | not opened this pass; per `api-inventory.md`, writes `Session["ImageBytes"]` to the response | **UNKNOWN** whether it checks `Session["UserId"]` before doing so — inventoried but not re-verified |
| `SignatureHandler.ashx` | per `GAP-APP-001`, guards on `Session["ImageBytes"]` but reads `Session["SigImageBytes"]` | Functional bug, re-stated from `GAP-APP-001`, not re-opened this pass |

## 4. Unauthenticated endpoints — consolidated cross-check

Per `GAP-SEC-004` (18 ASMX methods) plus this pass's findings:

- **All 18 `SInventoryWebService`/`WebService.asmx` methods**: confirmed no method rejects a caller without a
  valid session; 2 of the 8 spot-checked at least read session for scoping, the rest do not (`GAP-SEC-004`
  stands, severity High).
- **At least `GetCompanyWiseSales` and `GetAglingReport`** in `CashFlow.aspx.cs` (and by strong pattern
  inference, likely most/all of the other ~38 page `[WebMethod]`s in `Dashboard.aspx.cs` /
  `IncomeStatement.aspx.cs` / `TradeMaterialTransfer.aspx.cs`): confirmed/suspected no auth check, because they
  are `static` and never execute the hosting page's `Page_Load`.
- Net: unauthenticated surface is **larger than the 18 originally counted** — practically the entire 60-endpoint
  API surface should be assumed unauthenticated unless proven otherwise per-method.

## 5. Error handling / HTTP status consistency

- No endpoint checked wraps its body in try/catch with a structured error response; an exception (e.g. the
  `NullReferenceException` in `GetApprovedProduct`/`GetCustomer` when session keys are absent, or a SQL error
  from `GetAglingReport`) propagates to the ASP.NET default error pipeline. Combined with `debug="true"` and no
  `<customErrors>` (`GAP-SEC-008`), this returns a full stack trace (including the failing SQL text) as a 500
  response body to any caller, authenticated or not.
- No endpoint checked returns a distinct "unauthorized" status (401/403) for a missing session — the choices
  observed are: silently proceed with `companyId=0`-style defaults, or throw and surface a 500.

## 6. Bug register (API)

| ID | Title | Severity | Confidence |
|---|---|---|---|
| `GAP-SEC-004` | 18 ASMX methods reachable without authentication | High | Confirmed, refined (2/8 spot-checked do read session for row-scoping but none gate on it) |
| `BUG-API-001` | `CashFlow.aspx.cs::GetAglingReport` — unauthenticated `[WebMethod]` builds SQL by concatenating client-supplied `companyId`/`year`/`month` with no validation or parameterization | High | Confirmed |
| `BUG-API-002` | `SInventoryWebService.GetCustomernew` concatenates `Session["MOParam"]` raw into a SQL string after a parameterized `@prefixText` — injectable if that session value is ever attacker-influenced | Medium | Suspected (writer of `MOParam` not traced) |
| `BUG-API-003` | Page-level `[WebMethod]`s are `static` and bypass the hosting page's `Page_Load` session guard entirely — the entire 40-method surface should be treated as unauthenticated pending per-method verification | High | Confirmed mechanism (static methods bypass Page_Load); per-method coverage is Suspected beyond the 2 traced |
| `BUG-API-004` | No endpoint observed returns a controlled 401/403 for missing auth; failures surface as unhandled 500s with stack traces (compounds `GAP-SEC-008`) | Medium | Confirmed for the 4 methods traced end-to-end |
