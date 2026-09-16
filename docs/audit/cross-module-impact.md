# Cross-Module Impact Mapping

Scope: Phase 25. For each critical finding, the modules it reaches, traced through actual call chains and data
flow documented elsewhere in `docs/audit/`, not assumed.

## Map 1 — Non-atomic multi-step saves (`BUG-TXN-001`, root cause in `root-cause-analysis.md` RCA-001)

```
Invoice save (Sales)
   │  header committed, detail/stock loop can fail partway (BR-SYS-004)
   ▼
Stock (Inventory) ── tblDCStore left partially decremented ── GAP-APP-005 lost-update overwrite compounds this
   ▼
Delivery confirmation (Delivery/Logistics) ── posts unconditionally regardless of confirmation result (GAP-APP-018)
   ▼
Accounting posting (external ACCDB, via opaque SP) ── posted from a state that may not match what was confirmed
   ▼
Sales / COGS / stock valuation reports (Reporting) ── reflect the same unreconciled state
   ▼
Reconciliation (Sales Register vs Customer Ledger vs GL) ── cannot tie out; the break point is silent, no error logged anywhere (no logging framework configured — system-audit.md)
```

**Modules touched**: Sales/Order/Invoice, Inventory/Warehouse, Delivery/Logistics, Accounting (external),
Reports, Dashboard.

## Map 2 — Receipt master integrity (`BUG-FIN-002`/`GAP-APP-017`, RCA-002)

```
Collection screen (Finance/Collection) ── correct amount shown, correct amount posted to tblCustPayDetail
   │
   ▼
tblCustomerPay master ── N rows per physical receipt, each showing the FULL amount (audit-trail defect)
   ▼
Any direct consumer of tblCustomerPay (not tblCustPayDetail) ── HUMAN VERIFICATION REQUIRED to enumerate — GL
   posting and the checked due/ledger reports were confirmed NOT affected (they read tblCustPayDetail)
   ▼
External audit / bank reconciliation ── if performed against tblCustomerPay directly, will not match physical receipts
```

**Modules touched**: Finance/Collection (direct), potentially Reporting/Dashboard (if any undiscovered
consumer reads the master table — see `reconciliation-audit.md` for the enumeration performed).

## Map 3 — Payment reversal does not reverse (`BUG-FIN-003`, `collection-audit.md`)

```
PaymentReverse.aspx (Finance/Collection) ── issues a raw DELETE, never calls the GL reversal SP
   ▼
tblCustomerPay / tblCustPayDetail ── row(s) removed
   ▼
GL / AR (external ACCDB) ── original posting NEVER reversed — cash/bank and AR remain permanently misstated
   ▼
Customer Ledger / AR aging (Finance/Collection reports) ── customer's due balance is now understated (payment
   removed from due-calc) while accounting still shows the cash as received and posted — a genuine, ongoing
   balance-sheet mismatch, not just a reporting glitch
   ▼
Financial statements (external) ── if consolidated from the ACCDB, carry the stale posting indefinitely
```

**Modules touched**: Finance/Collection, Accounting (external), Reporting — this is the single highest
real-money-impact chain found in this audit, because it is the one place a discrepancy propagates all the way
to the general ledger with no compensating mechanism anywhere in the traced code.

## Map 4 — Duplicate-payment detection can flag and mass-delete legitimate payments (`BUG-FIN-004`)

```
DuplicatePaymentList.aspx (Finance/Collection) ── groups by TransctionDetailId, which defaults to 0 for nearly
   all payments outside one specific workflow
   ▼
Every zero-keyed payment row ── appears identical to every other zero-keyed row ── flagged as a "duplicate" set
   ▼
sp_DEL_DuplicatePayment ── if run against this false-positive set, deletes legitimate payment records
   ▼
Customer Ledger / AR / GL ── legitimate collections silently disappear from the customer's payment history,
   inflating their apparent outstanding balance
```

**Modules touched**: Finance/Collection, Accounting (external), Customer Ledger, and potentially Customer
Service/Collections operations (a customer who genuinely paid gets chased again).

## Map 5 — Absent database-level referential integrity (`BUG-DATA-001` family, RCA-004)

```
Any child table (invoice detail, payment detail, stock movement, order line) ── no enforced FK to its parent
   (1 physical FK in 213 tables, confirmed live-schema fact)
   ▼
Application-level discipline is the ONLY protection ── and that discipline already has confirmed gaps
   (non-atomic saves, Map 1)
   ▼
Every module that joins parent→child in a report or a screen ── silently under-counts or mis-attributes rows if
   an orphan exists, with no database error to surface it
```

**Modules touched**: potentially all — this is systemic, not localized to one workflow.

## Map 6 — Unreachable/unauthenticated surfaces (`GAP-SEC-002/004/009`, `security-audit.md`)

```
127 pages with no auth guard, 18 unauthenticated ASMX methods, no horizontal access control
   ▼
Master data (Customer, Product, Price, Employee) ── leaked to any caller via autocomplete web service
   ▼
Financial reports (Customer Ledger, Due, Payment) ── viewable by URL without login where the report viewer
   doesn't use the master page (confirmed pattern in reporting-audit.md)
   ▼
Reputational / compliance impact ── customer and pricing data exposure; not confined to one module
```

**Modules touched**: System/Admin (root cause), Master Data, Finance/Collection, Reports — essentially every
module that has a page outside the 256 pages using the master page.
