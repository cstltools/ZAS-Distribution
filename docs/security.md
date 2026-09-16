# Security

This page pulls every finding from [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) section C (`GAP-SEC-*`)
in full — all 9 confirmed findings, none softened or omitted — plus the mechanism detail from
[`../specs/00-system/role-permission.md`](../specs/00-system/role-permission.md). All of this is `FINDING` /
`CONFIRMED` unless stated otherwise: these are defects verified in code, not hypothetical risks.

## Authentication

**Mechanism:** custom, session-based. `Login.aspx` → `PanalBLL.Login` → `PanalClsDAL.Login` runs:

```csharp
string queryString = @" select * from dbo.tblUser U
                        left join tblUserCompanyUnit UC on U.UserId=UC.UserId
                        where LoginName='" + loginName + "' and Password='" + password +
                     "' and UserStatus='active'";
```

Forms authentication (`<authentication mode="Forms">`) is present in `Web.config` but **commented out** —
protection is session-only. There is no lockout, no attempt counter, no password policy, no expiry, no MFA.
**Confidence: CONFIRMED.** Source: `Library.DAL/PanalCls/PanalClsDAL.cs`, `Solution.Web/Web.config`.

## Authorization

No role model. Four independent layers, none of them roles: menu grant (`tblMenuDistribution`), company unit
(`tblUserCompanyUnit`), approval action (`tblActionUserWiseApproval` chain), and one-off `tblUser` flags. Full
mechanism in `../specs/00-system/role-permission.md`. **Confidence: CONFIRMED** (mechanism); **UNKNOWN** (actual
grants — data not in this repository).

## Roles

There is no role table. Two hard-coded superuser identities exist by literal comparison: `UserId = 1` (menu:
sees everything) and `LoginName = 'admin'` or `tblUser.IsDashboard = 1` (Dashboard link). Every other user's
capability matrix is `UNKNOWN` by nature. See [business.md](business.md) → Roles.

## Permissions

See [business.md](business.md) → Permissions and `../specs/00-system/role-permission.md` for the full matrix
mechanism (`tblMenuDistribution`, `tblUserCompanyUnit`, `tblActionUserWiseApproval`).

## Claims / JWT / Sessions

No claims-based identity, no JWT — this is a classic ASP.NET session model. `Session` is `InProc`, timeout
`24000` minutes (**≈16.6 days**). Session keys used across the app include `UserId`, `LoginName`, `UserType`,
`IsHead`, `ComUnitId`, `CentralWareHouse`, `UserTime`, plus report-parameter keys — an unattended browser stays
logged in for over two weeks, and two report windows opened from two tabs can overwrite each other's session
keys. **Confidence: CONFIRMED.** Source: `../specs/00-system/system-overview.md`, `../specs/00-system/architecture.md`.

## Password handling

**Passwords are stored and compared in plain text.** `tblUser.Password` is written and compared verbatim —
`PanalClsDAL.Login` (comparison), `UserDAL.SaveUserInformation` (insert). No hash, no salt.

- **Finding:** `GAP-SEC-007` · **Severity: critical**
- Source: `../specs/SPEC-GAPS.md`; code: `Library.DAL/PanalCls/PanalClsDAL.cs`, `Library.DAL/.../UserDAL.cs`
- Also recorded as `BR-SEC-003` in `../specs/00-system/business-rules.md`.

## Secrets

**Credentials are hard-coded in source.** `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs` contains a live
server name, the `sa` login and a plaintext password, with five more server/password pairs commented out.
`Solution.Web/Web.config` contains four more `sa` passwords in cleartext connection strings.

- **Finding:** `GAP-SEC-003` · **Severity: critical**
- **These are in version control and should be treated as compromised.**
- Source: `../specs/SPEC-GAPS.md`; code: `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs`, `Solution.Web/Web.config`

## Connection strings

Three connection strings live in `Web.config`: `SalesrollDB_ZAS` (primary), `ZAS_ACCDB` (accounting), and
commented-out alternates for other deployments of the same codebase (`SalesRollDB_FC`, `SalesDisDB_SMC`).
`CSTL_ACCDB` and `ZASHRIS` have **no connection string at all** — reached only via three-part names, so they
must be co-located or linked to the same SQL Server instance. See [deployment.md](deployment.md). **Confidence:
CONFIRMED.**

## Input validation

Server-side, imperative, message-per-rule (`if (x.Text == "") { showMessageBox(...); return false; }`). Almost
no `asp:*Validator` controls (1 in the whole application). Client-side restriction
(`FilteredTextBoxExtender`, 89 declared) is presentation-only and never re-checked on the server. Numeric
conversion is unguarded `Convert.ToDecimal(textBox.Text)` — empty or non-numeric input throws `FormatException`,
which with no global handler becomes a yellow screen (`GAP-APP-013`). **Confidence: CONFIRMED.** Source:
`../specs/00-system/conventions.md`.

## SQL injection protection

**Systemic SQL injection.** User input is concatenated into SQL strings throughout `Library.DAL`, then executed
via `EXEC ExecuteAllSqlQueryByStoreProcedure @Query = '<string>'`. This includes the **login query itself**:

```csharp
"select * from dbo.tblUser U left join tblUserCompanyUnit UC on U.UserId=UC.UserId " +
"where LoginName='" + loginName + "' and Password='" + password + "' and UserStatus='active'"
```

`Library.DAL` alone declares about **1,400** SQL strings built this way. Parameterised commands are used only
for the ~119 named procedures (the minority path).

- **Finding:** `GAP-SEC-005` · **Severity: critical**
- Also `BR-SEC-004` in `../specs/00-system/business-rules.md`.
- Source: `../specs/SPEC-GAPS.md`; code: `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` + every `*DAL` class

## XSS

**Menu URLs are written into HTML unquoted.** `MainMasterPage.master.cs` emits `href=` + the raw
`tblMainMenu.URL`, with no quoting and no encoding:

```csharp
manurHtml + @" <li><a href=" + row["URL"].ToString().Trim() + "><i class=" + parent + "></i>" + ...
```

A URL value containing markup is stored XSS against every user who can see that menu item.

- **Finding:** `GAP-SEC-006` · **Severity: medium**
- Source: `../specs/SPEC-GAPS.md`, `../specs/00-system/navigation.md`; code: `Solution.Web/MasterPages/MainMasterPage.master.cs`

## CSRF

`Web.config` does not explicitly disable ViewState or EventValidation, so ASP.NET's default per-page ViewState
MAC / EventValidation protections are presumably in effect for postback pages (this reduces classic
form-replay CSRF against postback controls) — **INFERRED**, since no custom CSRF token or `[ValidateAntiForgeryToken]`-equivalent
mechanism was found anywhere in the codebase, and Web Forms' built-in protections are not a CSRF defense by
design (they don't bind a request to an origin or a fresh per-request token issued for that purpose). Given
`GAP-SEC-002` (127 pages with no auth guard) and `GAP-SEC-009` (no re-check of permission on postback), an
attacker who can get an authenticated session to submit a request has few additional server-side barriers.
**Confidence: INFERRED** (no explicit CSRF configuration found; ASP.NET Web Forms defaults were not overridden
in either direction).

## File upload security

There is exactly **one** `asp:FileUpload` control in the entire application, on the Excel MIGO/customer-master
import screen (`Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` + `ExcelUpForMIGOBLL`, staging tables
`tblCustomerMasterExcelFileMaster`/`Detail`, `tbltempCustMaster`). No file-type, size, or content validation was
found in the reverse-engineered code path. **Confidence: CONFIRMED** (the control exists and is the only one);
**UNKNOWN** (whether any validation happens inside a stored procedure it calls). Source:
`../specs/00-system/integrations.md`.

## Sensitive data handling

- Plaintext passwords (`GAP-SEC-007`, above).
- Hard-coded `sa` credentials in version control (`GAP-SEC-003`, above).
- Production configuration runs with debug diagnostics on:

**Production configuration is debug configuration.** `Web.config` has `<compilation debug="true">`, no
`<customErrors>`, and a session timeout of 24,000 minutes (≈16.6 days). Unhandled exceptions render a full
stack trace, including the SQL that failed, to the browser.

- **Finding:** `GAP-SEC-008` · **Severity: high**
- Source: `../specs/SPEC-GAPS.md`, `../specs/00-system/architecture.md`; code: `Solution.Web/Web.config`

## The full list of 9 confirmed findings

| ID | Severity | Finding |
|---|---|---|
| `GAP-SEC-003` | **critical** | Live server, `sa` login and plaintext password hard-coded in `DB_Authentication.cs`; four more `sa` passwords in `Web.config`. In version control — treat as compromised. |
| `GAP-SEC-005` | **critical** | Systemic SQL injection — ~1,400 concatenated queries executed via `EXEC ExecuteAllSqlQueryByStoreProcedure @Query`, including the login query. |
| `GAP-SEC-007` | **critical** | Passwords stored and compared in plain text. |
| `GAP-SEC-002` | **critical** | 127 of 383 pages have no authentication guard — reachable by URL without logging in. |
| `GAP-SEC-004` | high | 18 unauthenticated ASMX methods leak the customer, product, price and employee masters, and are themselves injectable. |
| `GAP-SEC-008` | high | `debug="true"`, no `<customErrors>`, 16-day session timeout — stack traces with SQL are shown to users. |
| `GAP-SEC-009` | high | No horizontal access control; ids in URL/Session are not ownership-checked, and approval grants are not re-checked on postback. |
| `GAP-SEC-006` | medium | `tblMainMenu.URL` written into `href=` unquoted — stored XSS via the menu table. |
| `GAP-SEC-001` | high | Hiding a menu item does not protect the page — the menu **is** the permission model. |

## Additional detail: `GAP-SEC-002` and `GAP-SEC-004`

**127 of 383 pages have no authentication guard.** The only session check in the application is in
`MainMasterPage.master.cs`. 256 pages use that master page; 127 do not, and most of those are report viewers
that accept their parameters from the query string — reachable unauthenticated by URL. Per-page list in
[`../specs/INVENTORY-PAGES.md`](../specs/INVENTORY-PAGES.md).

**The autocomplete web service is unauthenticated.** All 18 methods on `SInventoryWebService.asmx` (exposed
twice, also as `WebService.asmx`) execute a `LIKE '%' + prefixText + '%'` query built by concatenation and check
no session. They leak the full customer master, product master, price list and employee list to any anonymous
caller, and are themselves injectable. Also recorded as `BR-SEC-005`.

## `GAP-SEC-009` in detail — no horizontal access control

No page verifies that the record it is loading belongs to the user's company unit, or that the user had the
menu grant for the URL they requested. Keys travel in the query string and in `Session`, unchecked. Approval
pages render disabled controls for actions the user lacks, but **do not re-check the grant on postback** — see
[`../specs/00-system/workflows/WF-approval.md`](../specs/00-system/workflows/WF-approval.md), test case
`TC-WF004-05`. Also recorded as `BR-SEC-001`, `BR-SEC-002`.
