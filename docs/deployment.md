# Deployment

This document states only what is verifiable from this repository. Where the repository is silent, that is
recorded as `UNKNOWN` with the artefact that would resolve it — consistent with
[`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md).

## Hosting model

**IIS-hosted ASP.NET Web Forms site**, `.NET Framework 4.0` (`Web.config` `targetFramework="4.0"`).
`Solution.Web` is declared in `Solution.sln` as a **Web Site** project
(`Project("{E24C65DC-7377-472B-9ABA-BC803B73C61A}")`), not a Web Application project — `.aspx.cs` files compile
at run time rather than being pre-built into a single assembly. **Confidence: CONFIRMED.** Source:
`Solution.sln`, `Solution.Web/Web.config`, `../specs/00-system/system-overview.md`.

The other four projects (`Library.DAL`, `Library.BLL`, `Library.DAO`, `Library.CrystalReports`) are standard
class libraries (`Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}")`). **Confidence: CONFIRMED.**

### `PrecompiledWeb/`

`PrecompiledWeb/Solution.Web` exists in the repository and is the output of `aspnet_compiler`
(`Debug.AspNetCompiler.TargetPath` in the `.sln`/build config). It is a **stale, checked-in copy of the whole
site** — it should not be read as source, and its presence suggests a precompile-and-deploy step was used at
some point, but no script or documentation of that step is present.

- **Finding:** `GAP-STR-004` · Status: `KNOWN`
- Source: [`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md), `../specs/00-system/architecture.md`
- **Confidence: CONFIRMED** that the folder is stale build output; **UNKNOWN** what process produced or
  consumes it in an actual deployment (no deployment script accompanies it in this repository).

## Connection strings / environment configuration

`Web.config` carries the live connection strings plus many commented-out historical ones for other machines and
other customer deployments of the same codebase (`SalesRollDB_FC`, `SalesDisDB_SMC`, plus a series of
developer-machine connection strings with hard-coded `sa` credentials, several still active). Three catalogs
have a connection string: `SalesrollDB_ZAS` (primary application DB), `ZAS_ACCDB` (accounting). `CSTL_ACCDB` and
`ZASHRIS` have **no connection string** — they are reached only via three-part SQL names, so they must live on
the same SQL Server instance (or a linked server) as the application database. **Confidence: CONFIRMED.** See
[security.md](security.md) (`GAP-SEC-003`) — several of these connection strings contain live, cleartext `sa`
passwords committed to version control and must be treated as compromised. Source:
`Solution.Web/Web.config`, `../specs/00-system/integrations.md`.

## CI/CD

**Confirmed absent.** No `.github/` directory, no `azure-pipelines.yml`, no build script, and no Dockerfile
exist anywhere in this repository (checked at the repository root and via a full-repo search).

- **Confidence: CONFIRMED absent.**

## What is genuinely unknown

| Question | Why it's unknown here |
|---|---|
| Which IIS site bindings, host headers, and TLS configuration are used in production | Not present in this repository — IIS site configuration lives on the server, not in source control |
| Application pool settings (identity, recycling, .NET version pinning beyond `targetFramework="4.0"`) | Same |
| The actual deployment procedure (manual copy, `aspnet_compiler` + xcopy, MSDeploy, other) | No deployment script or runbook exists in the repository; `PrecompiledWeb/` is a clue but not a documented process |
| Which of the many commented-out connection strings reflects the *current* production environment | Only the uncommented block in `Web.config` is live at HEAD; which physical server that resolves to in production is deployment-environment knowledge, not source |
| Whether a load balancer, reverse proxy, or CDN sits in front of the site | Not discoverable from application code |
| Backup/restore procedure for `SalesrollDB_ZAS` beyond the in-app `DBBackup.aspx` screen | The screen exists (`specs/00-system` `SYS-011`) but what it actually invokes and where backups are stored is a page-level detail, not a deployment-pipeline fact |

To close any of the above, supply the IIS configuration export, the application pool settings, and whatever
script or runbook (if any) is used to publish a new build to production.

## Summary table

| Aspect | Status |
|---|---|
| Hosting model | IIS, ASP.NET Web Forms, Web Site project — CONFIRMED |
| Target framework | .NET Framework 4.0 — CONFIRMED |
| Connection strings | In `Web.config`, several containing live cleartext credentials — CONFIRMED |
| Precompiled output in repo | `PrecompiledWeb/` present, stale — CONFIRMED, treat as non-source |
| CI/CD | Absent — CONFIRMED |
| IIS bindings / app pool / TLS | UNKNOWN — needs server configuration export |
| Deployment procedure/runbook | UNKNOWN — needs whatever process the operations team currently uses |
