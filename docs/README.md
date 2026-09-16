# docs/ — System-level understanding and navigation

This folder is the **narrative layer** on top of [`../specs/`](../specs/README.md). It does not re-derive any
fact: everything here is synthesized from what `specs/` already verified against the source code, and every
claim keeps the confidence tag it carries in `specs/` — `CONFIRMED`, `INFERRED`, or `UNKNOWN`. Nothing is
upgraded in confidence just by being repeated here.

**docs/ vs specs/**

| | `docs/` (this folder) | `specs/` |
|---|---|---|
| Purpose | System-level orientation: what the application is, how it's built, what to watch out for | Per-page, per-area detailed specification |
| Grain | One page per topic (architecture, security, a whole module) | One page per screen (383 page specs), plus system documents |
| Use it to | Get oriented, brief a new engineer, scope a change, write a security remediation plan | Look up exactly what one page/control/procedure does |
| Authority | Points into `specs/` for detail; carries no fact `specs/` doesn't already have | The source of truth for reverse-engineered facts |

Start with [`../specs/README.md`](../specs/README.md) and [`../specs/FINAL-REPORT.md`](../specs/FINAL-REPORT.md) if
you want the full audit trail. Start here if you want the short version.

## Contents

| Document | Covers |
|---|---|
| [business.md](business.md) | Business overview, glossary, modules, processes, roles, policies, key calculations |
| [architecture.md](architecture.md) | Layers, project structure, request lifecycle, data access, traced examples |
| [security.md](security.md) | Authentication, authorization, and the 9 confirmed security findings |
| [coding-standard.md](coding-standard.md) | The conventions actually used in this codebase — write new code to match them |
| [testing.md](testing.md) | Current state (no tests), where proposed test cases already live, regression candidates |
| [deployment.md](deployment.md) | Hosting model, configuration, CI/CD — and what is unverifiable from this repo |
| [navigation.md](navigation.md) | How the menu is built at runtime, and why the menu tree itself is unknown |
| [modules.md](modules.md) | The 11 inferred modules, one paragraph each, linking to their specs |
| [integrations.md](integrations.md) | The 4 external databases and what each integration does |
| [workflows.md](workflows.md) | Index of the 4 hand-traced cross-page workflows |

## The one fact to keep in mind everywhere

This application has no prior documentation, no database schema export, and no stored-procedure bodies in this
repository. Four broad areas — the DB schema, the 119 procedure bodies, the real menu/permission data, and
"why" each screen exists — are structurally `UNKNOWN` and cannot be resolved by reading more code. See
[`../specs/SPEC-GAPS.md`](../specs/SPEC-GAPS.md) for the itemized list and what artefact would close each gap.
