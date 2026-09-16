# Dependency Map — how a click reaches the database

**Status: CONFIRMED.** Synthesised from [`specs/00-system/architecture.md`](../../specs/00-system/architecture.md)
and [`specs/00-system/conventions.md`](../../specs/00-system/conventions.md), which trace this pattern against
source for every one of the 383 pages. This is the one file in `docs/reverse-engineering/` that is new synthesis
rather than an index — everything else here restates or points at `specs/`.

## The generic chain

Every page in this system, without exception, follows the same layering. There is no MVC controller/action
split, no dependency injection, and no service interface — every class is concrete and instantiated with `new`
directly in the page that uses it (`specs/00-system/architecture.md` — "There is no service layer, no repository
interface, no dependency injection, no unit of work and no ORM").

```
Browser
  │  full postback, or a partial postback inside an UpdatePanel
  ▼
Solution.Web/<folder>/<Page>.aspx            markup: server controls in a <table> layout
  │  server control event fires (OnClick / OnSelectedIndexChanged / OnTextChanged / OnRowCommand / …)
  ▼
Solution.Web/<folder>/<Page>.aspx.cs         code-behind: the event handler IS the "action" —
  │                                           reads control .Text/.SelectedValue directly, does most
  │                                           validation and calculation itself (business logic lives
  │                                           here more than in the BLL — see GAP-STR-005)
  ▼
Library.BLL/SInventory_BLL/<X>BLL.cs         often a 1-line pass-through; a handful of classes
  │                                           (InvoiceBLL, CustPaymentBLL, ChalanBLL, RequisitionBLL,
  │                                           PanalBLL) hold real logic
  ▼
Library.DAL/SInventory_DAL/<X>DAL.cs         either:
  │
  ├──(a) builds a SQL string by concatenation ──► Library.DAL/InternalCls/ClsCommonInternalDAL.cs
  │                                                 ──► EXEC ExecuteAllSqlQueryByStoreProcedure @Query='<string>'
  │                                                       (the "majority" path — ~1,400 call sites, all
  │                                                        SQL-injectable, see GAP-SEC-005)
  │
  └──(b) builds a List<SqlParameter> ──────────► a named stored procedure (sp_Sales_*, sp_I_*, sp_UD_*, …)
                                                    (the "minority" path — properly parameterised, used
                                                     for postings: stock, accounting, delivery, payment)
  ▼
SQL Server: SalesrollDB_ZAS  (+ ZAS_ACCDB, CSTL_ERPDB, CSTL_ACCDB, ZASHRIS reached by three-part name)
  ▼
table / view
```

`Library.DAO/*` plain property-bag classes (no attributes, no validation, no ORM mapping) carry data between
these layers — they are the closest thing to a DTO but are not tied to the database by any framework.

Two facts worth stating explicitly because they surprise anyone who has worked on a modern layered app:

- **The BLL is thinner than the layer diagram suggests.** `Library.BLL` has 76 classes; `Library.DAL` has 144.
  Many pages skip the BLL entirely and hold a DAL instance directly — e.g.
  `Solution.Web/SInventory_UI/BatchWiseCollection.aspx.cs` calls `BatchWiseCollectionReportDal` with no BLL in
  between at all.
- **The JS/AJAX layer is thin, not primary.** Per `GAP-JS-001`, of the 21 project-owned scripts, 18 are
  third-party theme code (jQuery 1.7.2, jQuery UI, fancybox, etc.) with no business logic; the remaining 3
  (`custom.js`, `datepicker.js`, `switcher.js`) are also theme code. Business logic executes **server-side in
  postbacks**, not via client-side AJAX — the only exception is the dashboard-analytics module's 40
  `[WebMethod]` endpoints (`CashFlow.aspx`, `Dashboard.aspx`, `IncomeStatement.aspx`, `TradeMaterialTransfer.aspx`),
  which really do fetch JSON with client-side JavaScript. See [`api-inventory.md`](api-inventory.md) and
  [`module-inventory.md`](module-inventory.md) → `dashboard-analytics`.

## Worked example 1 — saving a customer-invoice line (`InvoiceCreationForCustomer.aspx`)

This is the central transactional path of the whole application: turning grid rows into `tblInvoiceDetail` rows
by allocating stock batches in FEFO (first-expiry-first-out) order.

```
Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx
  saveButton (server control, OnClick="saveButton_Click")
  ▼
InvoiceCreationForCustomer.aspx.cs :: saveButton_Click
  ├─ Validation()                       -- imperative per-field checks (see GAP-APP-020: payTypeDDL.Text
  │                                        check is inert because the placeholder's Text is never "")
  ├─ int invId = SaveInvoice();         -- INSERT tblInvoice, committed on its own
  └─ SaveInvoiceDetail(invId)           -- loops the grid, per row calls:
        ▼
     Library.BLL/SInventory_BLL/InvoiceBLL.cs
        InvoiceBLL.InvoiceDetailInsertBLL(...)   -- computes line-level discount/VAT/net
        InvoiceBLL.UpdateDCStock(...)            -- FEFO batch allocation across tblDCStore rows;
                                                     no stock-availability check (GAP-APP-004), and the
                                                     stock write is a raw UPDATE, not a decrement, with
                                                     no concurrency token (GAP-APP-005)
        ▼
     Library.DAL/SInventory_DAL/InvoiceDAL.cs
        -- one INSERT tblInvoiceDetail per allocated batch, each its own autocommitted statement via
           ClsCommonInternalDAL (no surrounding transaction — GAP-APP-007: if the detail loop fails
           halfway, the already-written header and detail rows are not rolled back)
        ▼
     tblInvoice, tblInvoiceDetail, tblDCStore
```

Sources: `specs/00-system/architecture.md` ("Transactions" section, citing this exact method by name) and
`specs/sales-order-invoice/workflows/WF-invoice-batch-allocation.md`.

## Worked example 2 — a parameterised posting: delivery pipeline (`sp_Sales_ExecuteDeliveryPipeline`)

This is the minority, properly-parameterised path — used for the operations that matter most.

```
some delivery-confirmation page (e.g. Solution.Web/SInventory_UI/DelivaryInvoiceCreationAuto.aspx.cs)
  gotoinvoiceButton (server control, OnClick="gotoinvoiceButton_Click")
  ▼
DelivaryInvoiceCreationAuto.aspx.cs :: gotoinvoiceButton_Click
  -- posts the journal WITHOUT checking whether the confirmation procedure succeeded, and shows the
     same "Delivery Invoice Save Successfully !!" message on both the success and the fallback path
     (GAP-APP-018); the chart-of-accounts precondition on this path is commented out
  ▼
Library.DAL/SInventory_DAL/InvoiceDAL.cs
  public bool PostDeliveryConfFullJournal(int invoiceId)
  {
      var p = new List<SqlParameter> { new SqlParameter("@InvoiceId", invoiceId) };
      return aCommonInternalDal.UpdateAction("sp_Sales_ExecuteDeliveryPipeline", p);
  }
  ▼
EXEC sp_Sales_ExecuteDeliveryPipeline @InvoiceId = <invoiceId>     -- SqlParameter, not concatenation
  ▼
[body not in this repository — GAP-DB-001]
  UNKNOWN: what tables it touches, what it validates, its transaction boundary, its error behaviour
```

Source: `specs/00-system/architecture.md` §"2. Named stored procedures with `SqlParameter`" (cites this exact
method and procedure name), `specs/delivery-logistics/workflows/WF-delivery-confirmation.md`.

## Worked example 3 — the login query itself is the injectable, unparameterised path

Included because it shows the "majority" SQL path at its most consequential — this is not a report screen, it
is authentication.

```
Solution.Web/Login.aspx
  loginButton (server control, OnClick)
  ▼
Login.aspx.cs
  ▼
Library.BLL / Library.DAL (PanalCls / PanalClsDAL.Login-family methods)
  ▼
Library.DAL/InternalCls/ClsCommonInternalDAL.cs
  string query =
      "select * from dbo.tblUser U left join tblUserCompanyUnit UC on U.UserId=UC.UserId " +
      "where LoginName='" + loginName + "' and Password='" + password + "' and UserStatus='active'"
  dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
  db.AddInParameter(dbCommand, "Query", DbType.String, query);   -- the whole string is one parameter
  ▼
EXEC ExecuteAllSqlQueryByStoreProcedure @Query = '<concatenated string above>'
  ▼
tblUser, tblUserCompanyUnit
```

Consequence, CONFIRMED: this is SQL-injectable at the login screen itself, and `tblUser.Password` is compared in
plain text (`GAP-SEC-005`, `GAP-SEC-007`). Source: `specs/SPEC-GAPS.md` → `GAP-SEC-005` (quotes this exact
string), `specs/00-system/architecture.md` §"1. Inline SQL through one generic procedure."

## Notes for anyone extending this system

- **No controller/action layer exists to intercept a request** — the code-behind event handler is the
  request's only entry point. Adding "middleware"-style behaviour (auth check, logging) means adding it to
  every page's `Page_Load`/master page, not to a central pipeline. `MainMasterPage.master.cs` is the one place
  a session check already exists; see [`.claude/SKILL.md`](../../.claude/SKILL.md) → Security rules for why new
  pages must reuse that pattern.
- **New SQL must never take the concatenation path** — see `GAP-SEC-005` and the Database conventions section
  of [`.claude/SKILL.md`](../../.claude/SKILL.md).
- Full architecture narrative (transactions, primary-key generation, document numbering, session as a parameter
  bus, error handling, build/deploy): [`specs/00-system/architecture.md`](../../specs/00-system/architecture.md).
