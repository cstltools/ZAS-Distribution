# Gaps — index

This is a thin index over [`specs/SPEC-GAPS.md`](../../specs/SPEC-GAPS.md), which already documents every gap
and finding with full detail, IDs, severities and closure steps — verified and not duplicated here. This file
only summarises the categories and repeats the priority order for closing them, so a reader lands on the right
`GAP-*` ID quickly.

## Categories

| Category | What it covers | Count |
|---|---|---|
| A — Missing artefacts, the big four | Stored-procedure bodies, schema, menu/permission rows, any requirement/design source | 4 (`GAP-DB-001`, `GAP-DB-002`, `GAP-SEC-001`, `GAP-DOC-001`) |
| B — Missing per-area information | Field-label heuristics, run-time visible/enabled conditions, per-page CSS attribution, JS tracing, Crystal report internals, absence of tests | 6 (`GAP-UI-001`, `GAP-UI-002`, `GAP-UI-003`, `GAP-JS-001`, `GAP-RPT-001`, `GAP-TEST-001`) |
| C — Findings, security | Confirmed defects in the running application (`GAP-SEC-001` is listed under A above and not double-counted here) | 8 (`GAP-SEC-002` … `GAP-SEC-009`) |
| D — Findings, correctness / data-integrity | Confirmed logic bugs in the running application | 20 (`GAP-APP-001` … `GAP-APP-020`) |
| E — Structural gaps | Module boundaries, dead/excluded pages, duplicated screens, stale build output, business logic sitting in the UI layer | 6 (`GAP-STR-001` … `GAP-STR-006`) |
| **Total distinct `GAP-*` IDs** | | **44** |

Counts were obtained by counting `### \`GAP-...\`` headers directly in [`specs/SPEC-GAPS.md`](../../specs/SPEC-GAPS.md)
(4 + 6 + 8 + 20 + 6 = 44), grouped by the `## A.`–`## E.` section each falls under. Note this is **44**, not the
"36 gaps" headline figure quoted in `specs/FINAL-REPORT.md` §2 and `specs/README.md`; the file itself is
authoritative for the detail of each finding, and this discrepancy in the summary headline is worth a fix next
time either of those two documents is touched. If any count here disagrees with `specs/SPEC-GAPS.md` itself
(not its own headline text), trust the specs file.

## Priority order for closing the gaps

Reproduced verbatim from the end of `specs/SPEC-GAPS.md` §F:

| # | Do this | Closes |
|---|---|---|
| 1 | Rotate the `sa` password and every credential in `DB_Authentication.cs` and `Web.config` | `GAP-SEC-003` |
| 2 | Supply `sp_helptext` for all 119 procedures | `GAP-DB-001`, most `UNKNOWN`s in sections 16/19/21 |
| 3 | Supply the schema script | `GAP-DB-002`, Phase-11 column mapping |
| 4 | Supply the eight permission/menu `SELECT`s | `GAP-SEC-001`, `GAP-STR-001`, every section 20 |
| 5 | Confirm which of the duplicated pages are live | `GAP-STR-002`, `GAP-STR-003` |
| 6 | Export the Crystal report definitions | `GAP-RPT-001` |
| 7 | Business review of the INFERRED objectives in section 3 of each page spec | `GAP-DOC-001` |

## Full detail

Every individual `GAP-*` ID — its severity, exact code citation, and what closes it — is documented in
[`specs/SPEC-GAPS.md`](../../specs/SPEC-GAPS.md). Do not re-derive or restate gap detail elsewhere; link to the
specific ID instead.
