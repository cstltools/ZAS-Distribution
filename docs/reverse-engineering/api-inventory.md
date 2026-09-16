# API Inventory — index

This is a thin index. Full per-endpoint detail lives in [`docs/api.md`](../api.md) (system-level narrative,
maintained separately) and the original extraction is [`specs/00-system/api-inventory.md`](../../specs/00-system/api-inventory.md)
(page-spec-linked, verified against source). Read one of those two for detail; this file exists only to state
the totals and surface the one security-relevant fact every reader of this index needs to see first.

This is a **Web Forms postback application**. It has no REST API, no Web API controllers, no OData, and no MVC
routing. Every HTTP endpoint that is *not* a plain page postback is one of the following 60.

## Totals

| Category | Count |
|---|---|
| ASMX SOAP/script methods | 18 (exposed on 2 endpoints — `SInventoryWebService.asmx` and `WebService.asmx`, both pointing at the same implementation class) |
| Page-level `[WebMethod]` endpoints | 40 (across `CashFlow.aspx`, `Dashboard.aspx`, `IncomeStatement.aspx`, `TradeMaterialTransfer.aspx`) |
| `.ashx` HTTP handlers | 2 (`PictureHandler.ashx`, `SignatureHandler.ashx`) |
| **Total** | **60** |

(Source: `specs/FINAL-REPORT.md` §1 row 10; full breakdown in `specs/00-system/api-inventory.md`.)

## `GAP-SEC-004` — unauthenticated ASMX autocomplete endpoints (surfaced prominently, on purpose)

All **18** methods on `SInventoryWebService.asmx` (duplicated as `WebService.asmx`) check **no session** and
build their query as `LIKE '%' + prefixText + '%'` by string concatenation — so they are both anonymously
callable and SQL-injectable. Between them they leak the full **customer master, product master, price list, and
employee list** to any unauthenticated caller. Severity: **high**.

- Full method-by-method table (name, signature, session flag, auth flag): [`specs/00-system/api-inventory.md`](../../specs/00-system/api-inventory.md) §1.
- Finding detail: [`specs/SPEC-GAPS.md`](../../specs/SPEC-GAPS.md) → `GAP-SEC-004`.
- Related: `GAP-SEC-005` (the same concatenation pattern is systemic across `Library.DAL`), `GAP-APP-014` (the two `.asmx` endpoints are literally the same class — one is dead but still reachable).

## What does not exist

No REST/JSON API for business entities, no auth token/API key/CORS, no versioning or rate limiting, no
request/response validation layer, no OpenAPI/Swagger description. (Source: `specs/00-system/api-inventory.md` §4.)

## Sources

- [`docs/api.md`](../api.md) — full per-endpoint narrative (maintained outside `docs/reverse-engineering/`).
- [`specs/00-system/api-inventory.md`](../../specs/00-system/api-inventory.md) — original, verified extraction; every method signature, session flag and auth flag.
- [`specs/SPEC-GAPS.md`](../../specs/SPEC-GAPS.md) — `GAP-SEC-004`, `GAP-SEC-005`, `GAP-APP-014`.
