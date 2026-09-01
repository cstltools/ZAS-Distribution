# System Overview

## Identity

| | |
|---|---|
| Product name shown to users | **Salesroll** (login page header, `Solution.Web/Login.aspx`) |
| `Title` directive on the login page | `SMC-IIMS Login` — a leftover from another deployment of the same codebase |
| Customer | ZAS (logo `Solution.Web/css/Logo/zaslogo.png`) |
| Primary database | `SalesrollDB_ZAS` |
| Version / release shown on login | read at run time from a version table via `PanalBLL.GetVersionData()` → `Table_VersionControl` (`VersionName`, `ReleaseDate`) |

The same codebase has clearly been deployed for other customers: `Web.config` carries commented-out connection
strings for `SalesRollDB_FC` ("Faruq Cap") and `SalesDisDB_SMC`, and `DB_Names.cs` names the catalog
`SalesrollDB_ZAS`. Anything customer-specific is therefore **configuration and data**, not code.

## Solution layout

```
Solution.sln
├── Solution.Web              ASP.NET Web Forms web site (383 .aspx pages)   [Website project, not csproj]
├── Library.DAL               Data access:  130 *DAL classes + 5 infrastructure classes
├── Library.BLL               Business logic: 74 *BLL classes + 1 panel BLL
├── Library.DAO               Entities: ~75 plain property bags (no ORM, no attributes)
└── Library.CrystalReports    42 .rpt definitions + 114 typed DataSets (.xsd)
```

`PrecompiledWeb/`, `_ReSharper.Solution/`, `.vs/`, `*/bin`, `*/obj` and `_verify_build/` are build output and
tool caches, not source.

## Runtime

| Concern | Choice |
|---|---|
| Framework | .NET Framework **4.0** (`Web.config` `targetFramework="4.0"`, `Debug=true` in production config) |
| UI | ASP.NET **Web Forms** — no MVC, no Razor, no routing table, no Areas |
| Partial rendering | `UpdatePanel` (369 declared) + AJAX Control Toolkit (`CalendarExtender`, `FilteredTextBoxExtender`, `AutoCompleteExtender`, `ModalPopupExtender`) |
| Data access | Microsoft **Enterprise Library Data Access Application Block** (`Microsoft.Practices.EnterpriseLibrary.Data`) |
| Reporting | **Crystal Reports 13** (`crystalreportviewers13`, `CrystalImageHandler.aspx`) + hand-built HTML reports |
| Session | `InProc`, timeout **24000 minutes** (≈16.6 days) |
| Auth | **Custom session-based.** Forms authentication is present but commented out in `Web.config`. |
| Client libraries | jQuery 1.7.2 / jquery-ui 1.8.19, `select2`, plus a set of theme scripts under `Solution.Web/scripts` |
| Logging | **none.** log4net is listed in `<assemblies>` but is never configured or called. `Global.asax`'s `Application_Error` is empty. |

## Functional scope (order-to-cash)

The implemented flow, reconstructed from the code:

```
Master data (company/unit, region→area→territory→market, product, price, customer, sales force)
        │
        ▼
Order            ManualOrderrCreation / OrderRequisitionCreation → ManualOrderApproval
        │
        ▼
Proforma         InvoiceCreationForCustomer(ByOrder) → tblInvoice (+ tblInvoiceDetail, batch-allocated)
 invoice                 └── sp_ProformaPosting  (accounting)
        │
        ▼
Picking &        PickingSlipGenerate / SCPickingGenerate / CreatePickingOnWareHouse
 top sheet       TopSheetGenerate / DelivaryTopSheetGenerate / BatchWiseTopSheet
        │
        ▼
Delivery         DelivaryInvoiceCreationAuto / …ForCustomerByOrderAuto
 confirmation            ├── full     → sp_DeliveryConformationFull → sp_Sales_ExecuteDeliveryPipeline
                         ├── partial  → sp_Sales_DeliverySalesPosting
                         └── reject   → sp_DeliveryConformationReject
        │
        ├──────────────► Returns   ProformaorInvoiceReturn / ReturnProductCustomerByInvoice
        │                          → sp_ProformaReturnPosting / sp_Sales_DeliverySalesReturnPosting
        ▼
Collection       CustomerPayment / PaymentTranscation(New/New2) / MarketWiseCustomerPayment
                 → sp_Sales_CustomerPaymentPosting, payment batches, deposit slips
```

Stock is tracked in two tiers with batch and expiry:

- **Warehouse / central store** — `tblCentralStore`, `tblWHStockInMaster`/`Detail`, `tblWHStockOutMaster`/`Detail`
- **Distribution centre** — `tblDCStore` (batch rows), `tblDCStock`, `tblDCStoreFreeze`, view `View_DCStoreCurrentStock`

Issue is **FEFO** (first-expiry-first-out), not FIFO: `InvoiceDAL.BatchWiseProductQty` orders candidate batch rows
`ORDER BY ExpDate ASC` and `InvoiceBLL.UpdateDCStock` walks them consuming quantity then bonus quantity. This is
one of the most important behaviours in the system and is documented in
[../sales-order-invoice/workflows/WF-invoice-batch-allocation.md](../sales-order-invoice/workflows/WF-invoice-batch-allocation.md).

Costing is **weighted average**, run by procedure after the fact: `sp_Process_WeightedAverage`,
`sp_Process_WeightedAverageB2BTransfer`, `sp_Process_WeightedAverageFreezeStock`, reported by
`sp_INV_WeightedAverage_Report` into `tbl_INV_WeightedAvg`. A comment in
`Library.BLL/SInventory_BLL/InvoiceBLL.cs::SaveDCStoreFreeze` records that DC-store weighted average was
deliberately switched off ("Disabled as per user request to not maintain DC Store WAC").

## Organisational model

Two hierarchies, both master data:

**Geography / distribution**
`tblCompanyInfo` → `tblCompanyUnit` (a "ComUnit" = sales centre / distribution centre) → `tblDcWiseAreaInfo`
links a unit to areas; `tblRegion` → `tblArea` → `tblTerritory` → `tblMarket` → `tblCustMaster`.

**Sales force**
`tblEmpGeneralInfo` (employee) is referenced by `tblRSMInfo` (regional sales manager, by region),
`tblASMInfo` (area sales manager, by area), `tblMIOInfo` / `tblMioInfo` (market intelligence officer, by
territory) and `tblMIAInfo` (market intelligence assistant). Targets live in `tblMIATarget` and
`tblMIATargetProductWise`. `tblSlaveInfo` holds a subordinate/assistant relationship.

A customer is therefore reachable from a unit through
`tblCustMaster → tblMarket → tblTerritory → tblArea → tblDcWiseAreaInfo → tblCompanyUnit`, which is exactly the
join `InvoiceDAL.LoadCustomerMaster` performs to resolve a customer's owning sales centre and MIO.

## Multi-company

`tblCompanyInfo`, `tblCompanyUnit` and `tblUserCompanyUnit` make the system multi-company. On login, a
non-`Admin` user's company unit is put in `Session["ComUnitId"]` and most screens filter by it. Invoice numbers
are per-unit sequences: `INV-<ComUnitCode>-<9 digits>` (`InvoiceBLL.InvoiceNoGenerator`).

## What is deliberately not here

- No unit tests, integration tests, or test project of any kind.
- No CI configuration, no build script, no Dockerfile.
- No database project, migrations or seed scripts.
- No API documentation, no OpenAPI, no Postman collection.
- No `README`, `CHANGELOG` or architecture note anywhere in the repository before this specification.
