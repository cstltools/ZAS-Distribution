# Integrations

Full detail: [`../specs/00-system/integrations.md`](../specs/00-system/integrations.md).

Every integration in this application is **database-to-database**, inside one SQL Server instance, using
three-part names or a second connection string. There is **no message queue, no HTTP client, no file drop, no
scheduled job and no outbound web-service call anywhere in the codebase** — verified by the absence of
`HttpClient`, `WebRequest`, `WebClient`, `SmtpClient`, `ServiceReference`, and any
`Quartz`/`Hangfire`/`TaskScheduler` reference. **Confidence: CONFIRMED.**

## Map

```
                    SalesrollDB_ZAS   (the application's own database)
                            │
   ┌────────────────────────┼─────────────────────────┬──────────────────────┐
   │                        │                         │                      │
   ▼                        ▼                         ▼                      ▼
ZAS_ACCDB              CSTL_ACCDB               CSTL_ERPDB              ZASHRIS
(ZAS accounting)     (group accounting)        (group ERP)            (group HR)
 write: postings      read: chart of accounts,  read: items, batch      read: employees,
 read: COA, ledgers         vouchers, financial       stock, material         job locations
                            year, bank/cash            conversion, recipes
```

## 1. Accounting — `ZAS_ACCDB` and `CSTL_ACCDB`

**Direction:** SalesRoll → accounting (write, via procedures); accounting → SalesRoll (read, via three-part
names). **Connection:** `SolutionConnectionStringACCDB` for `ZAS_ACCDB`; `CSTL_ACCDB` has **no connection
string** — reached only as `CSTL_ACCDB..<table>`, so it must be co-located or linked to the same instance.

**What it does:** posts every accounting-relevant event — proforma invoice save/return, delivery confirmation
(full/partial), delivery invoice delete, customer payment and reversal, product destroy approval, warehouse
stock-in approval, sample invoice, freeze/release, trade material transfer, and GL row creation for
customer/supplier/product — each via a named stored procedure (`sp_ProformaPosting`,
`sp_Sales_ExecuteDeliveryPipeline`, `sp_Sales_CustomerPaymentPosting`, etc.). Two preconditions are checked
application-side before posting: an **Active** financial year in `CSTL_ACCDB..tblFinancialYear` covering the
document date, and a `ZAS_ACCDB..tblChartOfAccounts` row for the customer.

**Confidence: CONFIRMED** that these checks and calls exist. **UNKNOWN**: what each posting procedure actually
does, and whether any of them is idempotent (re-approving/re-posting could double-post — no evidence either
way, since procedure bodies are absent).

## 2. Group ERP — `CSTL_ERPDB`

**Direction:** ERP → SalesRoll, read-only; nothing in this repository writes to `CSTL_ERPDB`. Used for mapping
SalesRoll products to ERP items, batch stock lookup, and receiving converted/produced material
(`MaterialConversionReceive.aspx` and related screens). Also includes `SP_DataMigration_Item`, which pushes a
SalesRoll product **into** the ERP item master (body UNKNOWN), and `sp_StockInMIGOtoCentralStore` for goods
receipt from an ERP MIGO document. **Confidence: CONFIRMED** (the calls and screens exist); **UNKNOWN** (what
the procedures do with the data).

## 3. HR — `ZASHRIS`

**Direction:** HR → SalesRoll, read then copy. Screen: `HREmployeeSysnc.aspx` ("Sysnc" — spelling preserved
per convention, see [coding-standard.md](coding-standard.md)). It is **manual and one employee at a time** — the
user picks rows and presses sync; there is no schedule, no delta detection, and no deletion propagation. What
the sync procedures actually copy field-by-field is **UNKNOWN** (procedure bodies absent). **Confidence:
CONFIRMED** (the manual, one-at-a-time mechanism); **UNKNOWN** (procedure internals).

## 4. Excel

File-based, through the browser, in both directions. **Import:** the only `asp:FileUpload` control in the
entire application loads MIGO goods-receipt lines and customer-master rows into staging tables. **Export:**
several Crystal reports have an "Excel" variant selected by query string or session flag. **Confidence:
CONFIRMED.**

## 5. Reporting engine

Crystal Reports 13 is an in-process integration (not a network integration) — `crystalreportviewers13/` ships
client resources, `CrystalImageHandler.aspx` is a registered HTTP handler, and typed datasets define report
schemas. **Confidence: CONFIRMED.**

## 6. What is *not* integrated

Explicitly checked and absent: e-mail, SMS gateway, payment gateway, SAP/e-Pharma/e-Sales/LMS integration,
mobile/app API (despite `tblUser.IsAppsUser` implying app accounts exist, no endpoint serves them in this
repository), single sign-on/LDAP/AD, file-based EDI/FTP/SFTP. **Confidence: CONFIRMED absent.**

## Integration risk summary

| Risk | Evidence | Confidence |
|---|---|---|
| Cross-database writes are not in a distributed transaction | postings are separate `EXEC`s issued after the local insert has already committed | CONFIRMED |
| A failed posting is silent | return value frequently ignored; `DB_Manager` swallows exceptions | CONFIRMED |
| `CSTL_ACCDB` and `ZASHRIS` have no connection string — must be co-located or linked | three-part names only | CONFIRMED |
| Re-approving may double-post | idempotency lives in the absent procedures | UNKNOWN |
| Coupling is by table name across four databases | 30 three-part-name references | CONFIRMED |
