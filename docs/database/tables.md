# Tables

This file was rewritten on 2026-09-05 against a **live SQL Server schema extraction** of `SalesrollDB_ZAS` (instance `TOWSIF\MSSQLSERVER2019`), superseding the earlier app-code-only version (which covered 126 tables with no column/type/PK/FK detail, all marked UNKNOWN). That assumption no longer holds: every column, type, nullability, identity, default, primary key and index below comes from the live database catalog, not inference.

**Totals: 213 tables, 2438 columns. 112 tables are referenced by this app's C# source (Library.DAL / Library.BLL / Solution.Web); 101 have no app-code reference found (marked UNUSED below — still real tables in the DB, schema fully documented).**

- Confirmed **zero computed columns** exist anywhere in this database (catalog query returned empty) — not repeated per table below.
- Confirmed **zero triggers** exist anywhere in this database.
- Exactly **one physical foreign key** exists in the whole database: `tblPettyCashDetails.PettyCashmasterId` -> `tblPettyCashMaster.PettyCashmasterId` (ON DELETE/UPDATE NO ACTION). Every other FK-looking relationship below is a **logical (inferred), not enforced** relationship, inferred purely from column-name-matches-a-single-column-primary-key-elsewhere naming convention. These are best-effort guesses — verify before relying on them.

---

## `Employee`

**Status:** COVERED (referenced by app source). **Primary key:** `EmployeeID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `EmployeeID` | int | N | Y |  | PK |  |
| 2 | `FirstName` | nvarchar(50) | Y |  |  |  |  |
| 3 | `LastName` | nvarchar(50) | Y |  |  |  |  |
| 4 | `Department` | nvarchar(50) | Y |  |  |  |  |
| 5 | `Position` | nvarchar(50) | Y |  |  |  |  |
| 6 | `Salary` | decimal(10,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK__Employee__7AD04FF1FB548437` | PK | CLUSTERED | EmployeeID |

---

## `PaymentBachMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `BatchMasterId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `BatchMasterId` | int | N | Y |  | PK |  |
| 2 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `BatchDate` | datetime | Y |  | `(getdate())` |  |  |
| 4 | `EntryBy` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `DeliveryManId` | int | Y |  |  |  |  |
| 6 | `PayCustomerId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_PaymentBachMaster` | PK | CLUSTERED | BatchMasterId |

**Logical (inferred, not enforced) foreign keys:**

- `DeliveryManId` -> `tblDeliveryManInfo.DeliveryManId`

---

## `PaymentBatchDetails`

**Status:** COVERED (referenced by app source). **Primary key:** `BatchDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `BatchDetailId` | int | N | Y |  | PK |  |
| 2 | `BatchMasterId` | int | Y |  |  |  |  |
| 3 | `TranscationDetailId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_PaymentBatchDetails` | PK | CLUSTERED | BatchDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `BatchMasterId` -> `PaymentBachMaster.BatchMasterId`

---

## `Table_BadDuesInvoice`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvoiceNo` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `Table_CPM_BonusCampaignDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `CampaignDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CampaignDetailId` | int | N | Y |  | PK |  |
| 2 | `CampgainMasterId` | int | Y |  |  |  |  |
| 3 | `PointProductId` | int | Y |  |  |  |  |
| 4 | `Quantity` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_Table_CPM_BonusCampaignDetail` | PK | CLUSTERED | CampaignDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `CampgainMasterId` -> `Table_CPM_BonusCampaignMaster.CampgainMasterId`, `tblBonusCampaignMaster.CampgainMasterId`

---

## `Table_CPM_BonusCampaignMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `CampgainMasterId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CampgainMasterId` | int | N | Y |  | PK |  |
| 2 | `CampaignCode` | nvarchar(200) | Y |  |  |  |  |
| 3 | `CompanyId` | int | Y |  |  |  |  |
| 4 | `CampaignName` | nvarchar(500) | Y |  |  |  |  |
| 5 | `CampaignDesc` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `FromDate` | datetime | Y |  |  |  |  |
| 7 | `Todate` | datetime | N |  |  |  |  |
| 8 | `IsActive` | bit | Y |  | `((1))` |  | yes |
| 9 | `EntryBy` | nvarchar(200) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(200) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |
| 13 | `BonusProductId` | int | Y |  |  |  |  |
| 14 | `BonusProductQuantity` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_Table_CPM_BonusCampaignMaster` | PK | CLUSTERED | CampgainMasterId |

---

## `Table_CustomerUpload`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustomerCode` | nvarchar(50) | Y |  |  |  |  |
| 2 | `CustomerName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `Address` | nvarchar(500) | Y |  |  |  |  |
| 4 | `CellNo` | nvarchar(50) | Y |  |  |  |  |
| 5 | `MarketId` | int | Y |  |  |  |  |
| 6 | `MarketName` | nvarchar(50) | Y |  |  |  |  |
| 7 | `Address2` | nvarchar(50) | Y |  |  |  |  |
| 8 | `City` | nvarchar(50) | Y |  |  |  |  |
| 9 | `ConPerson` | nvarchar(50) | Y |  |  |  |  |
| 10 | `TermsOfPayment` | nvarchar(50) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `MarketId` -> `tblMarket.MarketId`

---

## `Table_DueInvoice`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvoiceId` | int | Y |  |  |  |  |
| 2 | `InvoiceNo` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `DeliveryInvoNo` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `InvoiceDate` | datetime | Y |  |  |  |  |
| 5 | `CustomerMasterId` | int | Y |  |  |  |  |
| 6 | `CustomerCode` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `CustomerName` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `MarketId` | int | Y |  |  |  |  |
| 9 | `UnitCode` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `UnitId` | int | Y |  |  |  |  |
| 11 | `MiaCode` | nvarchar(MAX) | Y |  |  |  |  |
| 12 | `MiaId` | int | Y |  |  |  |  |
| 13 | `Due` | decimal(18,2) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `MarketId` -> `tblMarket.MarketId`

---

## `Table_EmployeeAsPerHR`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CompanyName` | nvarchar(MAX) | Y |  |  |  |  |
| 2 | `D_EmpCode` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `HR_EmpCode` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `HR_Designation` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `EmpName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `D_Designation` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `HR_Designation_id` | int | Y |  |  |  |  |
| 8 | `CompanyId` | int | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `Table_InstitutionalCustomerUpload`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CostomerCode` | nvarchar(50) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `Table_Inventory_TranscationTypes`

**Status:** UNUSED — no app-code reference found. **Primary key:** `TranscationTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TranscationTypeId` | int | N | Y |  | PK |  |
| 2 | `TranscationType` | nvarchar(200) | Y |  |  |  |  |
| 3 | `RespectiveTable` | nvarchar(500) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_Table_Inventory_TranscationTypes` | PK | CLUSTERED | TranscationTypeId |

---

## `Table_Inventory_Types`

**Status:** UNUSED — no app-code reference found. **Primary key:** `InventoryTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InventoryTypeId` | int | N | Y |  | PK |  |
| 2 | `InventoryType` | nvarchar(500) | Y |  |  |  |  |
| 3 | `FullForm` | nvarchar(500) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_Table_Inventory_Types` | PK | CLUSTERED | InventoryTypeId |

---

## `Table_IVF`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvoiceNo` | nvarchar(50) | Y |  |  |  |  |
| 2 | `MiaCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `UnitCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `MiaId` | int | Y |  |  |  |  |
| 5 | `InvoiceId` | int | Y |  |  |  |  |
| 6 | `UnitId` | int | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`

---

## `Table_Temp_CustPayDetails`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvoiceId` | int | Y |  |  |  |  |
| 2 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 3 | `CustPayId` | int | Y |  |  |  |  |
| 4 | `IsPrint` | bit | Y |  |  |  | yes |
| 5 | `IsPosting` | bit | Y |  |  |  | yes |
| 6 | `CashAccId` | int | Y |  |  |  |  |
| 7 | `BankAccId` | int | Y |  |  |  |  |
| 8 | `Discount` | decimal(18,2) | Y |  |  |  |  |
| 9 | `PaymentRemarks` | nvarchar(500) | Y |  |  |  |  |
| 10 | `TransctionDetailId` | int | Y |  |  |  |  |
| 11 | `ProcessTime` | datetime | Y |  |  |  |  |
| 12 | `AIT` | decimal(18,2) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`
- `CustPayId` -> `tblCustomerPay.CustPayId`, `tblCustomerPayDeleteLog.CustPayId`, `tblPaymentTranscationMaster.CustPayId`

---

## `Table_Temp_DuplicateInvoice`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TempInvoiceId` | int | Y |  |  |  |  |
| 2 | `InvoiceNo` | nvarchar(200) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `Table_test`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `id` | int | Y |  |  |  |  |
| 2 | `invno` | nvarchar(500) | Y |  |  |  |  |
| 3 | `Amount` | decimal(18,2) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `id` -> `tblBalanceSheetMapDetail.id`, `tblBalancesheetMapMaster.id`, `tblProInvoiceReturnTrack.id`

---

## `Table_VersionControl`

**Status:** COVERED (referenced by app source). **Primary key:** `VersionId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `VersionId` | int | N | Y |  | PK |  |
| 2 | `VersionName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `Changes` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `ReleaseDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_Table_VersionControl` | PK | CLUSTERED | VersionId |

---

## `tbl_AppVersion`

**Status:** UNUSED — no app-code reference found. **Primary key:** `AppVersionId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `AppVersionId` | int | N | Y |  | PK |  |
| 2 | `Version` | int | Y |  |  |  |  |
| 3 | `VersionName` | nvarchar(50) | Y |  |  |  |  |
| 4 | `IsActive` | bit | Y |  | `((1))` |  | yes |
| 5 | `InActiveDate` | datetime | Y |  |  |  |  |
| 6 | `DownloadLink` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_AppVersion` | PK | CLUSTERED | AppVersionId |

---

## `tbl_ContactType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ContactTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ContactTypeId` | int | N | Y |  | PK |  |
| 2 | `ContactType` | nvarchar(50) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  | `((1))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_ContactType` | PK | CLUSTERED | ContactTypeId |

---

## `tbl_DcrBrandDetails`

**Status:** UNUSED — no app-code reference found. **Primary key:** `BrandDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `BrandDetailId` | int | N | Y |  | PK |  |
| 2 | `BrandId` | int | Y |  |  |  |  |
| 3 | `DcrId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DcrBrandDetails` | PK | CLUSTERED | BrandDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `DcrId` -> `tbl_DCRInfo.DcrId`

---

## `tbl_DCRCustomerInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DCRCustomerId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DCRCustomerId` | int | N | Y |  | PK |  |
| 2 | `DcrDate` | datetime | Y |  |  |  |  |
| 3 | `EmpInfoId` | int | Y |  |  |  |  |
| 4 | `EntryDate` | datetime | Y |  |  |  |  |
| 5 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `CustomerId` | int | Y |  |  |  |  |
| 7 | `GroupId` | int | Y |  |  |  |  |
| 8 | `RegionId` | int | Y |  |  |  |  |
| 9 | `AreaId` | int | Y |  |  |  |  |
| 10 | `SubTerritoryId` | int | Y |  |  |  |  |
| 11 | `TerritoryId` | int | Y |  |  |  |  |
| 12 | `MarketId` | int | Y |  |  |  |  |
| 13 | `ApprovalStatus` | nvarchar(50) | Y |  |  |  | yes |
| 14 | `Latitude` | nvarchar(MAX) | Y |  |  |  |  |
| 15 | `Longitude` | nvarchar(MAX) | Y |  |  |  |  |
| 16 | `StreetAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 17 | `DoctorProgramypeId` | int | Y |  |  |  |  |
| 18 | `GroupName` | nvarchar(MAX) | Y |  |  |  |  |
| 19 | `RegionName` | nvarchar(MAX) | Y |  |  |  |  |
| 20 | `AreaName` | nvarchar(MAX) | Y |  |  |  |  |
| 21 | `TerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `SubTerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 23 | `MarketName` | nvarchar(MAX) | Y |  |  |  |  |
| 24 | `GroupCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `RegionCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 26 | `AreaCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `TerritoryCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `SubTerritoryCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 29 | `MarketCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `SmcTypeId_DCR` | int | Y |  |  |  |  |
| 31 | `SMCType_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 32 | `CompanyId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DCRCustomerInfo` | PK | CLUSTERED | DCRCustomerId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`

---

## `tbl_DcrDetails`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DcrDetailID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DcrDetailID` | int | N | Y |  | PK |  |
| 2 | `DcrId` | int | Y |  |  |  |  |
| 3 | `ProductId` | int | Y |  |  |  |  |
| 4 | `Type` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ProductQty` | decimal(18,0) | Y |  |  |  |  |
| 6 | `GWPromoQtyId` | int | Y |  |  |  |  |
| 7 | `EmpInfoId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DcrDetails` | PK | CLUSTERED | DcrDetailID |

**Logical (inferred, not enforced) foreign keys:**

- `DcrId` -> `tbl_DCRInfo.DcrId`
- `ProductId` -> `tblProduct.ProductId`
- `GWPromoQtyId` -> `tblGroupWisePromoQty.GWPromoQtyId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tbl_DCRInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DcrId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DcrId` | int | N | Y |  | PK |  |
| 2 | `DcrDate` | datetime | Y |  |  |  |  |
| 3 | `TourTypeId` | int | Y |  |  |  |  |
| 4 | `ChemberId` | int | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `IsApproved` | bit | Y |  | `((0))` |  | yes |
| 10 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `DoctorId` | int | Y |  |  |  |  |
| 12 | `DocTPDetailsId` | int | Y |  |  |  |  |
| 13 | `GroupId` | int | Y |  |  |  |  |
| 14 | `RegionId` | int | Y |  |  |  |  |
| 15 | `AreaId` | int | Y |  |  |  |  |
| 16 | `SubTerritoryId` | int | Y |  |  |  |  |
| 17 | `TerritoryId` | int | Y |  |  |  |  |
| 18 | `MarketId` | int | Y |  |  |  |  |
| 19 | `IsNonEffectiveReason` | bit | Y |  |  |  | yes |
| 20 | `ReasonId` | int | Y |  |  |  |  |
| 21 | `EntryDate_Apps` | datetime | Y |  |  |  |  |
| 22 | `ApprovalStatus` | nvarchar(50) | Y |  |  |  | yes |
| 23 | `Latitude` | nvarchar(MAX) | Y |  |  |  |  |
| 24 | `Longitude` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `StreetAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 26 | `DoctorProgramypeId` | int | Y |  |  |  |  |
| 27 | `GroupName` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `RegionName` | nvarchar(MAX) | Y |  |  |  |  |
| 29 | `AreaName` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `TerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 31 | `SubTerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 32 | `MarketName` | nvarchar(MAX) | Y |  |  |  |  |
| 33 | `GroupCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 34 | `RegionCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 35 | `AreaCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 36 | `TerritoryCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 37 | `SubTerritoryCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 38 | `MarketCode_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 39 | `SmcTypeId_DCR` | int | Y |  |  |  |  |
| 40 | `SMCType_DCR` | nvarchar(MAX) | Y |  |  |  |  |
| 41 | `CompanyId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DCRInfo` | PK | CLUSTERED | DcrId |

**Logical (inferred, not enforced) foreign keys:**

- `TourTypeId` -> `tbl_TourPlanType.TourTypeId`
- `ChemberId` -> `tblDoctorChemberDetail.ChemberId`
- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `DocTPDetailsId` -> `tbl_DoctorTourPlanDetail.DocTPDetailsId`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`
- `ReasonId` -> `tblNonEffectiveReason.ReasonId`

---

## `tbl_DcrVisitedWithDetails`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DcrVisitWithId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DcrVisitWithId` | int | N | Y |  | PK |  |
| 2 | `EmpInfoId` | int | Y |  |  |  |  |
| 3 | `DcrId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DcrVisitedWithDetails` | PK | CLUSTERED | DcrVisitWithId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `DcrId` -> `tbl_DCRInfo.DcrId`

---

## `tbl_DoctorTourPlanDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DocTPDetailsId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DocTPDetailsId` | int | N | Y |  | PK |  |
| 2 | `SMId` | int | Y |  |  |  |  |
| 3 | `DoctorId` | int | Y |  |  |  |  |
| 4 | `ShiftId` | int | Y |  |  |  |  |
| 5 | `TourTypeId` | int | Y |  |  |  |  |
| 6 | `TPId` | int | Y |  |  |  |  |
| 7 | `Comment` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `TourPlanDate` | datetime | Y |  |  |  |  |
| 9 | `EmpInfoId` | int | Y |  |  |  |  |
| 10 | `IsApproved` | bit | Y |  | `((0))` |  | yes |
| 11 | `CreatedBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `CreatedDate` | datetime | Y |  |  |  |  |
| 13 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 14 | `UpdateDate` | datetime | Y |  |  |  |  |
| 15 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 16 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 17 | `DocTPMaster` | int | Y |  |  |  |  |
| 18 | `IsDcrDone` | bit | Y |  | `((0))` |  | yes |
| 19 | `GroupId` | int | Y |  |  |  |  |
| 20 | `RegionId` | int | Y |  |  |  |  |
| 21 | `AreaId` | int | Y |  |  |  |  |
| 22 | `TerritoryId` | int | Y |  |  |  |  |
| 23 | `SubTerritoryId` | int | Y |  |  |  |  |
| 24 | `MarketId` | int | Y |  |  |  |  |
| 25 | `GroupName_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 26 | `RegionName_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `AreaName_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `TerritoryName_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 29 | `SubTerritoryName_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `MarketName_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 31 | `DoctorProgramypeId_DV` | int | Y |  |  |  |  |
| 32 | `GroupCode_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 33 | `RegionCode_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 34 | `AreaCode_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 35 | `TerritoryCode_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 36 | `SubTerritoryCode_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 37 | `MarketCode_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 38 | `SMCTypeId_DV` | int | Y |  |  |  |  |
| 39 | `SMCType_DV` | nvarchar(MAX) | Y |  |  |  |  |
| 40 | `CompanyId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DoctorTourPlanDetail` | PK | CLUSTERED | DocTPDetailsId |

**Logical (inferred, not enforced) foreign keys:**

- `SMId` -> `tbl_SubMarket.SMId`
- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `ShiftId` -> `tbl_Shift.ShiftId`
- `TourTypeId` -> `tbl_TourPlanType.TourTypeId`
- `TPId` -> `tbl_TourPlanPurpose.TPId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `DocTPMaster` -> `tbl_DoctorTourPlanMaster.DocTPMaster`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`

---

## `tbl_DoctorTourPlanMaster`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DocTPMaster`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DocTPMaster` | int | N | Y |  | PK |  |
| 2 | `MonthValue` | int | Y |  |  |  |  |
| 3 | `YearValue` | int | Y |  |  |  |  |
| 4 | `EmpInfoId` | int | Y |  |  |  |  |
| 5 | `IsFinalSubmit` | bit | Y |  | `((0))` |  | yes |
| 6 | `ApprovalStatus` | nvarchar(50) | Y |  | `(N'Pending')` |  | yes |
| 7 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 8 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 9 | `FinalSubmitRemarks` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `ApprovalRemarks` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DoctorTourPlanMaster` | PK | CLUSTERED | DocTPMaster |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tbl_DoctorVisitType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DocVisitTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DocVisitTypeId` | int | N | Y |  | PK |  |
| 2 | `VisitTypeName` | nvarchar(50) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  | `((1))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_DoctorVisitType` | PK | CLUSTERED | DocVisitTypeId |

---

## `tbl_GlobalAccess_User`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `GlobalUserName` | nvarchar(50) | Y |  |  |  |  |
| 2 | `IMEI` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IMEI_Two` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tbl_Group`

**Status:** UNUSED — no app-code reference found. **Primary key:** `GroupId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `GroupId` | int | N | Y |  | PK |  |
| 2 | `GroupName` | nvarchar(500) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `EntryBy` | int | Y |  |  |  |  |
| 5 | `EntryDate` | datetime | Y |  |  |  |  |
| 6 | `UpdateBy` | int | Y |  |  |  |  |
| 7 | `UpdateDate` | datetime | Y |  |  |  |  |
| 8 | `ApproveBy` | int | Y |  |  |  |  |
| 9 | `ApproveDate` | datetime | Y |  |  |  | yes |
| 10 | `InactiveBy` | int | Y |  |  |  |  |
| 11 | `InactiveDate` | datetime | Y |  |  |  |  |
| 12 | `GroupCode` | nvarchar(MAX) | Y |  |  |  |  |
| 13 | `CodeStr` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_Group` | PK | CLUSTERED | GroupId |

---

## `tbl_ImagePath_Setting`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ImagePathId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ImagePathId` | int | N | Y |  | PK |  |
| 2 | `ImageType` | nvarchar(50) | Y |  |  |  |  |
| 3 | `ImagePreName` | nvarchar(50) | Y |  |  |  |  |
| 4 | `IsActive` | bit | Y |  |  |  | yes |
| 5 | `ImagePath` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `Remarks` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_ImagePath` | PK | CLUSTERED | ImagePathId |

---

## `tbl_INV_BatchWiseStock`

**Status:** COVERED (referenced by app source). **Primary key:** `Batch_Stock_Id`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `Batch_Stock_Id` | int | N | Y |  | PK |  |
| 2 | `Weighted_Avg_Id` | int | Y |  |  |  |  |
| 3 | `Batch_No` | nvarchar(200) | Y |  |  |  |  |
| 4 | `Mfg_Date` | datetime | Y |  |  |  |  |
| 5 | `Exp_Date` | datetime | Y |  |  |  |  |
| 6 | `Trans_Qty` | decimal(18,3) | Y |  |  |  |  |
| 7 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `InvoiceDetailId` | int | Y |  | `((0))` |  |  |
| 9 | `SampleOrderDetailId` | int | Y |  | `((0))` |  |  |
| 10 | `DCStoreId` | int | Y |  | `((0))` |  |  |
| 11 | `DestroyId` | int | Y |  | `((0))` |  |  |
| 12 | `B2B_ChallanDetailId` | int | Y |  | `((0))` |  |  |
| 13 | `Issue_Weighted_Avg_Id` | int | Y |  | `((0))` |  | yes |
| 14 | `DCStoreFreezeId` | int | Y |  | `((0))` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_INV_BatchWiseStock` | PK | CLUSTERED | Batch_Stock_Id |

**Logical (inferred, not enforced) foreign keys:**

- `Weighted_Avg_Id` -> `tbl_INV_CentralStore_WeightedAvg.Weighted_Avg_Id`, `tbl_INV_WeightedAvg.Weighted_Avg_Id`
- `InvoiceDetailId` -> `tblInvoiceDetail.InvoiceDetailId`
- `DCStoreId` -> `tblDCStore.DCStoreId`
- `DestroyId` -> `tblDestroyProduct.DestroyId`
- `DCStoreFreezeId` -> `tblDCStoreFreeze.DCStoreFreezeId`

---

## `tbl_INV_Central_BatchWiseStock`

**Status:** UNUSED — no app-code reference found. **Primary key:** `Batch_Stock_Id`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `Batch_Stock_Id` | int | N | Y |  | PK |  |
| 2 | `Weighted_Avg_Id` | int | Y |  |  |  |  |
| 3 | `Batch_No` | nvarchar(200) | Y |  |  |  |  |
| 4 | `Mfg_Date` | datetime | Y |  |  |  |  |
| 5 | `Exp_Date` | datetime | Y |  |  |  |  |
| 6 | `Trans_Qty` | decimal(18,3) | Y |  |  |  |  |
| 7 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `WHStockInDetailId` | int | Y |  | `((0))` |  |  |
| 9 | `StockInTransfarId` | int | Y |  | `((0))` |  |  |
| 10 | `Issue_Weighted_Avg_Id` | int | Y |  | `((0))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_INV_Central_BatchWiseStock` | PK | CLUSTERED | Batch_Stock_Id |

**Logical (inferred, not enforced) foreign keys:**

- `Weighted_Avg_Id` -> `tbl_INV_CentralStore_WeightedAvg.Weighted_Avg_Id`, `tbl_INV_WeightedAvg.Weighted_Avg_Id`
- `WHStockInDetailId` -> `tblWHStockInDetail.WHStockInDetailId`
- `StockInTransfarId` -> `tblStockInTransfar.StockInTransfarId`

---

## `tbl_INV_CentralStore_WeightedAvg`

**Status:** UNUSED — no app-code reference found. **Primary key:** `Weighted_Avg_Id`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `Weighted_Avg_Id` | bigint | N | Y |  | PK |  |
| 2 | `Mst_Trans_Id` | int | Y |  |  |  |  |
| 3 | `Trans_Id` | int | Y |  |  |  |  |
| 4 | `Item_Code` | nvarchar(200) | Y |  |  |  |  |
| 5 | `Ref_No` | nvarchar(200) | Y |  |  |  |  |
| 6 | `Trans_Date` | datetime | Y |  |  |  |  |
| 7 | `Actual_Date` | datetime | Y |  |  |  |  |
| 8 | `Entry_Date` | datetime | Y |  |  |  |  |
| 9 | `Trans_Type_Id` | int | Y |  |  |  |  |
| 10 | `Inv_Type_Id` | int | Y |  |  |  |  |
| 11 | `Inv_Type` | nvarchar(200) | Y |  |  |  |  |
| 12 | `Company_Id` | int | Y |  |  |  |  |
| 13 | `Warehouse_Id` | int | Y |  |  |  |  |
| 14 | `Item_Id` | int | Y |  |  |  |  |
| 15 | `OP_Qty` | decimal(18,3) | Y |  |  |  |  |
| 16 | `OP_Rate` | decimal(18,4) | Y |  |  |  |  |
| 17 | `OP_Amt` | decimal(18,4) | Y |  |  |  |  |
| 18 | `Curr_Qty` | decimal(18,3) | Y |  |  |  |  |
| 19 | `Curr_Rate` | decimal(18,4) | Y |  |  |  |  |
| 20 | `Curr_Amt` | decimal(18,4) | Y |  |  |  |  |
| 21 | `Close_Qty` | decimal(18,3) | Y |  |  |  |  |
| 22 | `Avg_Rate` | decimal(18,4) | Y |  |  |  |  |
| 23 | `Close_Amt` | decimal(18,4) | Y |  |  |  |  |
| 24 | `Weighted_Avg_Id_Issue` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK__tbl_INV_CentralStore___D5DF2A2D00E4AD4D` | PK | CLUSTERED | Weighted_Avg_Id |

---

## `tbl_INV_WeightedAvg`

**Status:** COVERED (referenced by app source). **Primary key:** `Weighted_Avg_Id`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `Weighted_Avg_Id` | bigint | N | Y |  | PK |  |
| 2 | `Mst_Trans_Id` | int | Y |  |  |  |  |
| 3 | `Trans_Id` | int | Y |  |  |  |  |
| 4 | `Item_Code` | nvarchar(200) | Y |  |  |  |  |
| 5 | `Ref_No` | nvarchar(200) | Y |  |  |  |  |
| 6 | `Trans_Date` | datetime | Y |  |  |  |  |
| 7 | `Actual_Date` | datetime | Y |  |  |  |  |
| 8 | `Entry_Date` | datetime | Y |  |  |  |  |
| 9 | `Trans_Type_Id` | int | Y |  |  |  |  |
| 10 | `Inv_Type_Id` | int | Y |  |  |  |  |
| 11 | `Inv_Type` | nvarchar(200) | Y |  |  |  |  |
| 12 | `Company_Id` | int | Y |  |  |  |  |
| 13 | `Warehouse_Id` | int | Y |  |  |  |  |
| 14 | `Item_Id` | int | Y |  |  |  |  |
| 15 | `OP_Qty` | decimal(18,3) | Y |  |  |  |  |
| 16 | `OP_Rate` | decimal(18,4) | Y |  |  |  |  |
| 17 | `OP_Amt` | decimal(18,4) | Y |  |  |  |  |
| 18 | `Curr_Qty` | decimal(18,3) | Y |  |  |  |  |
| 19 | `Curr_Rate` | decimal(18,4) | Y |  |  |  |  |
| 20 | `Curr_Amt` | decimal(18,4) | Y |  |  |  |  |
| 21 | `Close_Qty` | decimal(18,3) | Y |  |  |  |  |
| 22 | `Avg_Rate` | decimal(18,4) | Y |  |  |  |  |
| 23 | `Close_Amt` | decimal(18,4) | Y |  |  |  |  |
| 24 | `Weighted_Avg_Id_Issue` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK__tbl_INV___D5DF2A2D00E4AD4D` | PK | CLUSTERED | Weighted_Avg_Id |

---

## `tbl_MC_ProductRawMaterialDetails`

**Status:** COVERED (referenced by app source). **Primary key:** `ProductRawMaterialsDetailsId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProductRawMaterialsDetailsId` | int | N | Y |  | PK |  |
| 2 | `ProductId` | int | Y |  |  |  |  |
| 3 | `RawMaterialId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_MC_ProductRawMaterialDetails` | PK | CLUSTERED | ProductRawMaterialsDetailsId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`

---

## `tbl_Notice_MarketDetails`

**Status:** UNUSED — no app-code reference found. **Primary key:** `NoticeDetailsId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `NoticeDetailsId` | int | N | Y |  | PK |  |
| 2 | `NoticeId` | int | N |  |  |  |  |
| 3 | `RegionId` | int | Y |  |  |  |  |
| 4 | `AreaId` | int | Y |  |  |  |  |
| 5 | `TerritoryId` | int | Y |  |  |  |  |
| 6 | `MarketId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_Notice_MarketDetails` | PK | CLUSTERED | NoticeDetailsId |

**Logical (inferred, not enforced) foreign keys:**

- `NoticeId` -> `tbl_Notice_MarketMaster.NoticeId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`

---

## `tbl_Notice_MarketMaster`

**Status:** UNUSED — no app-code reference found. **Primary key:** `NoticeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `NoticeId` | int | N | Y |  | PK |  |
| 2 | `NoticeTitle` | nvarchar(500) | N |  |  |  |  |
| 3 | `Announcement` | nvarchar(500) | Y |  |  |  |  |
| 4 | `FromDate` | datetime | Y |  |  |  |  |
| 5 | `ToDate` | datetime | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `IsActive` | bit | Y |  | `((1))` |  | yes |
| 9 | `IsReaded` | bit | Y |  | `((0))` |  | yes |
| 10 | `CompanyId` | int | Y |  |  |  |  |
| 11 | `IsPublish` | bit | Y |  |  |  | yes |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |
| 13 | `UpdateBy` | nvarchar(500) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_Notice_MarketMaster` | PK | CLUSTERED | NoticeId |

---

## `tbl_Notice_UserDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `NoticeUserId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `NoticeUserId` | int | N | Y |  | PK |  |
| 2 | `MasterId` | int | Y |  |  |  |  |
| 3 | `UserId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_Notice_UserDetail` | PK | CLUSTERED | NoticeUserId |

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`

---

## `tbl_Notification`

**Status:** UNUSED — no app-code reference found. **Primary key:** `NotificationId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `NotificationId` | int | N | Y |  | PK |  |
| 2 | `NotificationType` | nvarchar(50) | Y |  |  |  |  |
| 3 | `NotificationPrimaryText` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `NotificationMainText` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `IsRead` | bit | Y |  | `((0))` |  | yes |
| 8 | `EmpInfoId` | int | Y |  |  |  |  |
| 9 | `NotificationGroupId` | int | Y |  |  |  |  |
| 10 | `PrimaryId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_Notification` | PK | CLUSTERED | NotificationId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tbl_NotificationGroup`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `NotificationGroupId` | int | Y |  |  |  |  |
| 2 | `GroupName` | nvarchar(50) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  | `((1))` |  | yes |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tbl_PrescriptionMaster`

**Status:** UNUSED — no app-code reference found. **Primary key:** `PrescriptionId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PrescriptionId` | int | N | Y |  | PK |  |
| 2 | `PrescriptionDate` | datetime | Y |  |  |  |  |
| 3 | `PrescriptionTypeId` | int | Y |  |  |  |  |
| 4 | `DoctorId` | int | Y |  |  |  |  |
| 5 | `ImagePath` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `ImageName` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `EntryDate` | datetime | Y |  |  |  |  |
| 9 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `UpdateDate` | datetime | Y |  |  |  |  |
| 11 | `ApprovalStatus` | nvarchar(50) | Y |  |  |  | yes |
| 12 | `ApprovedBy` | int | Y |  |  |  | yes |
| 13 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 14 | `ChemberId` | int | Y |  |  |  |  |
| 15 | `TerritoryId` | nchar(10) | Y |  |  |  |  |
| 16 | `SubTerritoryId` | nchar(10) | Y |  |  |  |  |
| 17 | `MarketId` | nchar(10) | Y |  |  |  |  |
| 18 | `GroupId` | int | Y |  |  |  |  |
| 19 | `RegionId` | int | Y |  |  |  |  |
| 20 | `AreaId` | int | Y |  |  |  |  |
| 21 | `DoctorProgramypeId` | int | Y |  |  |  |  |
| 22 | `GroupName` | nvarchar(MAX) | Y |  |  |  |  |
| 23 | `RegionName` | nvarchar(MAX) | Y |  |  |  |  |
| 24 | `AreaName` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `TerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 26 | `SubTerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `MarketName` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `GroupCode_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 29 | `RegionCode_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `AreaCode_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 31 | `TerritoryCode_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 32 | `SubTerritoryCode_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 33 | `MarketCode_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 34 | `SmcTypeId_RX` | int | Y |  |  |  |  |
| 35 | `SMCType_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 36 | `DoctorType_RX` | nvarchar(MAX) | Y |  |  |  |  |
| 37 | `DoctorTypeId_RX` | int | Y |  |  |  |  |
| 38 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 39 | `CompanyId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_PrescriptionMaster` | PK | CLUSTERED | PrescriptionId |

**Logical (inferred, not enforced) foreign keys:**

- `PrescriptionTypeId` -> `tbl_PrescriptionType.PrescriptionTypeId`
- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `ChemberId` -> `tblDoctorChemberDetail.ChemberId`
- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`

---

## `tbl_PrescriptionProductDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `PresDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PresDetailId` | int | N | Y |  | PK |  |
| 2 | `PrescriptionId` | int | Y |  |  |  |  |
| 3 | `ProductId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_PrescriptionProductDetail` | PK | CLUSTERED | PresDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `PrescriptionId` -> `tbl_PrescriptionMaster.PrescriptionId`
- `ProductId` -> `tblProduct.ProductId`

---

## `tbl_PrescriptionType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `PrescriptionTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PrescriptionTypeId` | int | N | Y |  | PK |  |
| 2 | `PrescriptionType` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  | `((1))` |  | yes |
| 4 | `ActiveInactiveDate` | datetime | Y |  |  |  |  |
| 5 | `EntryBy` | int | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | int | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_Prescription` | PK | CLUSTERED | PrescriptionTypeId |

---

## `tbl_Shift`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ShiftId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ShiftId` | int | N | Y |  | PK |  |
| 2 | `ShiftText` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `CreatedBy` | nvarchar(50) | Y |  |  |  |  |
| 4 | `CreatedDate` | datetime | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  | `((1))` |  | yes |
| 6 | `ShiftInTime` | time | Y |  |  |  |  |
| 7 | `ShiftOutTime` | time | Y |  |  |  |  |
| 8 | `PresentInTime` | time | Y |  |  |  |  |
| 9 | `PresentOutTime` | time | Y |  |  |  |  |
| 10 | `LetInTime` | time | Y |  |  |  |  |
| 11 | `LetOutTime` | time | Y |  |  |  |  |
| 12 | `AbsentInTime` | time | Y |  |  |  |  |
| 13 | `AbsentOutTime` | time | Y |  |  |  |  |
| 14 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 15 | `EntryDate` | datetime | Y |  |  |  |  |
| 16 | `Activedate` | datetime | Y |  |  |  |  |
| 17 | `UpdateBy` | nvarchar(MAX) | Y |  |  |  |  |
| 18 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_Shift` | PK | CLUSTERED | ShiftId |

---

## `tbl_SubMarket`

**Status:** UNUSED — no app-code reference found. **Primary key:** `SMId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SMId` | int | N | Y |  | PK |  |
| 2 | `SMCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `SMName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `MarketId` | int | N |  |  |  |  |
| 5 | `IsActive` | bit | Y |  |  |  | yes |
| 6 | `AcOrInAcDate` | datetime | Y |  |  |  |  |
| 7 | `CreatedBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `CreatedDate` | datetime | Y |  |  |  |  |
| 9 | `UpdatedBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `UpdatedDate` | datetime | Y |  |  |  |  |
| 11 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_SubMarket` | PK | CLUSTERED | SMId |

**Logical (inferred, not enforced) foreign keys:**

- `MarketId` -> `tblMarket.MarketId`

---

## `tbl_TEMP_InvoiceWiseCustomerDue`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustomerMasterId` | int | Y |  |  |  |  |
| 2 | `InvoiceNo` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `DueAmount` | numeric(18,2) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`

---

## `tbl_TEMP_TotalDueCustomerWise`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustomerCode` | int | Y |  |  |  |  |
| 2 | `Due` | numeric(18,2) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tbl_TourPlanInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `TourPlanId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TourPlanId` | int | N | Y |  | PK |  |
| 2 | `TerritoryId` | int | Y |  |  |  |  |
| 3 | `MarketId` | int | Y |  |  |  |  |
| 4 | `SMId` | int | Y |  |  |  |  |
| 5 | `CustomerMasterId` | int | Y |  |  |  |  |
| 6 | `ShiftId` | int | Y |  |  |  |  |
| 7 | `TourTypeId` | int | Y |  |  |  |  |
| 8 | `TPId` | int | Y |  |  |  |  |
| 9 | `Comment` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `TourPlanDate` | datetime | Y |  |  |  |  |
| 11 | `EmpInfoId` | int | Y |  |  |  |  |
| 12 | `IsMarketWise` | bit | Y |  | `((1))` |  | yes |
| 13 | `IsApproved` | bit | Y |  | `((0))` |  | yes |
| 14 | `CreatedBy` | nvarchar(50) | Y |  |  |  |  |
| 15 | `CreatedDate` | datetime | Y |  |  |  |  |
| 16 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 17 | `UpdateDate` | datetime | Y |  |  |  |  |
| 18 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 19 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 20 | `TPMaster` | int | Y |  |  |  |  |
| 21 | `GroupId` | int | Y |  |  |  |  |
| 22 | `RegionId` | int | Y |  |  |  |  |
| 23 | `AreaId` | int | Y |  |  |  |  |
| 25 | `SubTerritoryId` | int | Y |  |  |  |  |
| 26 | `GroupName` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `RegionName` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `AreaName` | nvarchar(MAX) | Y |  |  |  |  |
| 29 | `TerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `SubTerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 31 | `MarketName` | nvarchar(MAX) | Y |  |  |  |  |
| 32 | `GroupCode_TP` | nvarchar(MAX) | Y |  |  |  |  |
| 33 | `RegionCode_TP` | nvarchar(MAX) | Y |  |  |  |  |
| 34 | `AreaCode_TP` | nvarchar(MAX) | Y |  |  |  |  |
| 35 | `TerritoryCode_TP` | nvarchar(MAX) | Y |  |  |  |  |
| 36 | `SubTerritoryCode_TP` | nvarchar(MAX) | Y |  |  |  |  |
| 37 | `MarketCode_TP` | nvarchar(MAX) | Y |  |  |  |  |
| 38 | `CompanyId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_TourPlanInfo` | PK | CLUSTERED | TourPlanId |

**Logical (inferred, not enforced) foreign keys:**

- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`
- `SMId` -> `tbl_SubMarket.SMId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `ShiftId` -> `tbl_Shift.ShiftId`
- `TourTypeId` -> `tbl_TourPlanType.TourTypeId`
- `TPId` -> `tbl_TourPlanPurpose.TPId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `TPMaster` -> `tbl_TourPlanMaster.TPMaster`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`

---

## `tbl_TourPlanMaster`

**Status:** UNUSED — no app-code reference found. **Primary key:** `TPMaster`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TPMaster` | int | N | Y |  | PK |  |
| 2 | `MonthValue` | int | Y |  |  |  |  |
| 3 | `YearValue` | int | Y |  |  |  |  |
| 4 | `EmpInfoId` | int | Y |  |  |  |  |
| 5 | `IsFinalSubmit` | bit | Y |  | `((0))` |  | yes |
| 6 | `ApprovalStatus` | nvarchar(50) | Y |  | `(N'pending')` |  | yes |
| 7 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 8 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 9 | `FinalSubmitRemarks` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `ApprovalRemarks` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_TourPlanMaster` | PK | CLUSTERED | TPMaster |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tbl_TourPlanPurpose`

**Status:** UNUSED — no app-code reference found. **Primary key:** `TPId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TPId` | int | N | Y |  | PK |  |
| 2 | `TPName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `Createdby` | nvarchar(50) | Y |  |  |  |  |
| 4 | `CreatedDate` | datetime | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  | `((1))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_TourPlanPurpose` | PK | CLUSTERED | TPId |

---

## `tbl_TourPlanType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `TourTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TourTypeId` | int | N | Y |  | PK |  |
| 2 | `TourTypeName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `CreatedBy` | nvarchar(50) | Y |  |  |  |  |
| 4 | `CreatedDate` | datetime | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  | `((1))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_TourPlanType` | PK | CLUSTERED | TourTypeId |

---

## `tbl_UserRoleInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `UserRoleID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UserRoleID` | int | N | Y |  | PK |  |
| 2 | `RoleName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `ActiveDate` | datetime | Y |  |  |  |  |
| 5 | `ActiveInActiveDate` | datetime | Y |  |  |  |  |
| 6 | `InActiveBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `EntryDate` | datetime | Y |  |  |  |  |
| 9 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `UpdateDate` | datetime | Y |  |  |  |  |
| 11 | `RoleTypeId` | int | Y |  |  |  |  |
| 12 | `IsApprove` | bit | Y |  |  |  | yes |
| 13 | `IsForward` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_UserRoleInfo` | PK | CLUSTERED | UserRoleID |

**Logical (inferred, not enforced) foreign keys:**

- `RoleTypeId` -> `tblRoleType.RoleTypeId`

---

## `tbl_UserTracking`

**Status:** UNUSED — no app-code reference found. **Primary key:** `TrackingId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TrackingId` | int | N | Y |  | PK |  |
| 2 | `EmpInfoId` | int | Y |  |  |  |  |
| 3 | `LatValue` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `LongValue` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `AddressName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `Time` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `TrackDate` | datetime | Y |  |  |  |  |
| 8 | `CompanyId` | int | Y |  |  |  |  |
| 9 | `GroupId` | int | Y |  |  |  |  |
| 10 | `GroupCode` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `GroupName` | nvarchar(MAX) | Y |  |  |  |  |
| 12 | `RegionId` | int | Y |  |  |  |  |
| 13 | `RegionCode` | nvarchar(MAX) | Y |  |  |  |  |
| 14 | `RegionName` | nvarchar(MAX) | Y |  |  |  |  |
| 15 | `AreaId` | int | Y |  |  |  |  |
| 16 | `AreaCode` | nvarchar(MAX) | Y |  |  |  |  |
| 17 | `AreaName` | nvarchar(MAX) | Y |  |  |  |  |
| 18 | `TerritoryId` | int | Y |  |  |  |  |
| 19 | `TerritoryCode` | nvarchar(MAX) | Y |  |  |  |  |
| 20 | `TerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 21 | `AttTypeLoc` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `breakReason` | nvarchar(MAX) | Y |  |  |  |  |
| 23 | `ActiveStatus` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_UserTracking` | PK | CLUSTERED | TrackingId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`

---

## `tblAction`

**Status:** COVERED (referenced by app source). **Primary key:** `ActionId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ActionId` | int | N |  |  | PK |  |
| 2 | `ActionText` | nvarchar(50) | Y |  |  |  |  |
| 3 | `ActionValue` | nvarchar(50) | Y |  |  |  |  |
| 4 | `IsShow` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblAction` | PK | CLUSTERED | ActionId |

---

## `tblActionPageWiseStep`

**Status:** COVERED (referenced by app source). **Primary key:** `PWASId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PWASId` | int | N |  |  | PK |  |
| 2 | `ManuSL` | int | Y |  |  |  |  |
| 3 | `ASId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblActionPageWiseStep` | PK | CLUSTERED | PWASId |

**Logical (inferred, not enforced) foreign keys:**

- `ASId` -> `tblActionSteps.ASId`

---

## `tblActionStatus`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ActionId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ActionId` | int | N | Y |  | PK |  |
| 2 | `ActionStatus` | nvarchar(500) | Y |  |  |  | yes |
| 3 | `SoftwareUseId` | int | Y |  |  |  |  |
| 4 | `WebShow` | nvarchar(500) | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblActionStatus` | PK | CLUSTERED | ActionId |

---

## `tblActionStepDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `ASWLId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ASWLId` | int | N |  |  | PK |  |
| 2 | `ASId` | int | Y |  |  |  |  |
| 3 | `ActionId` | int | Y |  |  |  |  |
| 4 | `ActionCondition` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblActionStepDetail` | PK | CLUSTERED | ASWLId |

**Logical (inferred, not enforced) foreign keys:**

- `ASId` -> `tblActionSteps.ASId`
- `ActionId` -> `tblAction.ActionId`, `tblActionStatus.ActionId`

---

## `tblActionSteps`

**Status:** COVERED (referenced by app source). **Primary key:** `ASId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ASId` | int | N |  |  | PK |  |
| 2 | `ActionSteps` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblActionSteps` | PK | CLUSTERED | ASId |

---

## `tblActionUserWiseApproval`

**Status:** COVERED (referenced by app source). **Primary key:** `UWAId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UWAId` | int | N |  |  | PK |  |
| 2 | `UserId` | int | Y |  |  |  |  |
| 3 | `LoginName` | nvarchar(50) | Y |  |  |  |  |
| 4 | `ManuSL` | int | Y |  |  |  |  |
| 5 | `ActionId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblActionUserWiseApproval` | PK | CLUSTERED | UWAId |

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`
- `ActionId` -> `tblAction.ActionId`, `tblActionStatus.ActionId`

---

## `tblArea`

**Status:** COVERED (referenced by app source). **Primary key:** `AreaId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `AreaId` | int | N | Y |  | PK |  |
| 2 | `AreaCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `AreaName` | nvarchar(250) | Y |  |  |  |  |
| 4 | `AreaShortName` | nvarchar(50) | Y |  |  |  |  |
| 5 | `RegionId` | int | Y |  |  |  |  |
| 6 | `Remarks` | nvarchar(500) | Y |  |  |  |  |
| 7 | `Description` | nvarchar(500) | Y |  |  |  |  |
| 8 | `IsActive` | bit | Y |  |  |  | yes |
| 9 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblArea` | PK | CLUSTERED | AreaId |

**Logical (inferred, not enforced) foreign keys:**

- `RegionId` -> `tblRegion.RegionId`

---

## `tblASMInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `ASMId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ASMId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `AreaId` | int | Y |  |  |  |  |
| 4 | `EmployeeId` | int | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  |  |  | yes |
| 6 | `ActiveDate` | datetime | Y |  |  |  |  |
| 7 | `InActiveDate` | datetime | Y |  |  |  |  |
| 8 | `InActiveBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |
| 13 | `Vacant` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblASMInfo` | PK | CLUSTERED | ASMId |

**Logical (inferred, not enforced) foreign keys:**

- `AreaId` -> `tblArea.AreaId`
- `EmployeeId` -> `Employee.EmployeeId`

---

## `tblBalanceSheetMapDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `Id`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `Id` | int | N | Y |  | PK |  |
| 2 | `MasterId` | int | Y |  |  |  |  |
| 3 | `Particulars` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblBalanceSheetMapDetail` | PK | CLUSTERED | Id |

---

## `tblBalancesheetMapMaster`

**Status:** UNUSED — no app-code reference found. **Primary key:** `Id`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `Id` | int | N | Y |  | PK |  |
| 2 | `Particular` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `ParticularType` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `HasDetail` | bit | Y |  |  |  | yes |
| 5 | `IsNode` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblBalancesheetMapMaster` | PK | CLUSTERED | Id |

---

## `tblBankInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `BankId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `BankId` | int | N | Y |  | PK |  |
| 2 | `BankName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `SWIFTCode` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `Website` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ShortName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `Type` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblBankInfo` | PK | CLUSTERED | BankId |

---

## `tblBonusCampaignMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `CampgainMasterId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CampgainMasterId` | int | N | Y |  | PK |  |
| 2 | `CampaignCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 4 | `EntryDate` | datetime | Y |  |  |  |  |
| 5 | `CompanyId` | int | Y |  |  |  |  |
| 6 | `CampaignName` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `CampaignDesc` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `FromDate` | datetime | Y |  |  |  |  |
| 9 | `Todate` | datetime | Y |  |  |  |  |
| 10 | `Type` | nvarchar(50) | Y |  |  |  |  |
| 11 | `IsActive` | bit | Y |  | `((1))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblBonusCampaignMaster` | PK | CLUSTERED | CampgainMasterId |

---

## `tblBonusCampgainDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `CampaignDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CampaignDetailId` | int | N | Y |  | PK |  |
| 2 | `CampgainMasterId` | int | Y |  |  |  |  |
| 3 | `PointProductCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 5 | `BonusProductCode` | nvarchar(50) | Y |  |  |  |  |
| 6 | `BonusQuantity` | decimal(18,0) | Y |  |  |  |  |
| 7 | `IsActive` | bit | Y |  |  |  | yes |
| 8 | `FromDate` | datetime | Y |  |  |  |  |
| 9 | `FromTime` | time | Y |  |  |  |  |
| 10 | `ToDate` | datetime | Y |  |  |  |  |
| 11 | `ToTime` | time | Y |  |  |  |  |
| 12 | `IsSame` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblBonusCampgainDetail` | PK | CLUSTERED | CampaignDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `CampgainMasterId` -> `Table_CPM_BonusCampaignMaster.CampgainMasterId`, `tblBonusCampaignMaster.CampgainMasterId`

---

## `tblCentralStore`

**Status:** COVERED (referenced by app source). **Primary key:** `ReceiveId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ReceiveId` | int | N | Y |  | PK |  |
| 2 | `StorageLocation` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `ProductId` | int | N |  |  |  |  |
| 4 | `ProductCode` | nvarchar(50) | N |  |  |  |  |
| 5 | `ProductName` | nvarchar(MAX) | N |  |  |  |  |
| 6 | `PackSize` | nvarchar(50) | N |  |  |  |  |
| 7 | `BatchNo` | nvarchar(MAX) | N |  |  |  |  |
| 8 | `Quantity` | decimal(18,0) | N |  |  |  |  |
| 9 | `MfgDate` | datetime | N |  |  |  |  |
| 10 | `ExpDate` | datetime | N |  |  |  |  |
| 11 | `ReceiveDate` | datetime | N |  |  |  |  |
| 12 | `ChalanNo` | nvarchar(MAX) | N |  |  |  |  |
| 13 | `ChalanDate` | datetime | N |  |  |  |  |
| 14 | `StockInQty` | decimal(18,0) | N |  |  |  |  |
| 15 | `UnitPrice` | decimal(18,2) | N |  |  |  |  |
| 16 | `TotalPrice` | decimal(18,2) | N |  |  |  |  |
| 17 | `VATPerUnit` | decimal(18,2) | N |  |  |  |  |
| 18 | `TotalVAT` | decimal(18,2) | N |  |  |  |  |
| 19 | `TotalAmount` | decimal(18,2) | N |  |  |  |  |
| 20 | `StockCondition` | nvarchar(50) | N |  |  |  |  |
| 21 | `MigoDetailID` | int | N |  |  |  |  |
| 22 | `DeveloperRemarks` | nvarchar(500) | Y |  |  |  |  |
| 23 | `ProductStockType` | nvarchar(50) | N |  |  |  |  |
| 24 | `InternalNoteNo` | nvarchar(50) | Y |  |  |  |  |
| 25 | `WarehouseId` | int | Y |  |  |  |  |
| 26 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `MaterialConversionMasterId` | int | Y |  |  |  |  |
| 28 | `Company_Id` | int | Y |  |  |  |  |
| 29 | `WarehouseId1` | int | Y |  |  |  |  |
| 30 | `Mst_Trans_Id` | int | Y |  |  |  |  |
| 31 | `Trans_Id` | int | Y |  |  |  |  |
| 32 | `Ref_No` | nvarchar(200) | Y |  |  |  |  |
| 33 | `Trans_Date` | datetime | Y |  |  |  |  |
| 34 | `Actual_Date` | datetime | Y |  |  |  |  |
| 35 | `Entry_Date` | datetime | Y |  |  |  |  |
| 36 | `Trans_Type_Id` | int | Y |  |  |  |  |
| 37 | `Inv_Type_Id` | int | Y |  |  |  |  |
| 38 | `Inv_Type` | nvarchar(200) | Y |  |  |  |  |
| 39 | `OP_Qty` | decimal(18,3) | Y |  |  |  |  |
| 40 | `OP_Rate` | decimal(18,4) | Y |  |  |  |  |
| 41 | `OP_Amt` | decimal(18,4) | Y |  |  |  |  |
| 42 | `Curr_Qty` | decimal(18,3) | Y |  |  |  |  |
| 43 | `Curr_Rate` | decimal(18,4) | Y |  |  |  |  |
| 44 | `Curr_Amt` | decimal(18,4) | Y |  |  |  |  |
| 45 | `Close_Qty` | decimal(18,3) | Y |  |  |  |  |
| 46 | `Avg_Rate` | decimal(18,4) | Y |  |  |  |  |
| 47 | `Close_Amt` | decimal(18,4) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCentralStore` | PK | CLUSTERED | ReceiveId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`

---

## `tblCentralStore_OpeninigBalance`

**Status:** COVERED (referenced by app source). **Primary key:** `CSOpeninigBalanceId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CSOpeninigBalanceId` | int | N | Y |  | PK |  |
| 2 | `CSOpeninigBalanceDate` | datetime | N |  |  |  |  |
| 3 | `ReceiveId` | int | N |  |  |  |  |
| 4 | `StorageLocation` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ProductId` | int | N |  |  |  |  |
| 6 | `ProductCode` | nvarchar(50) | N |  |  |  |  |
| 7 | `ProductName` | nvarchar(MAX) | N |  |  |  |  |
| 8 | `PackSize` | nvarchar(50) | N |  |  |  |  |
| 9 | `BatchNo` | nvarchar(MAX) | N |  |  |  |  |
| 10 | `Quantity` | decimal(18,0) | N |  |  |  |  |
| 11 | `MfgDate` | datetime | N |  |  |  |  |
| 12 | `ExpDate` | datetime | N |  |  |  |  |
| 13 | `ReceiveDate` | datetime | N |  |  |  |  |
| 14 | `ChalanNo` | nvarchar(MAX) | N |  |  |  |  |
| 15 | `ChalanDate` | datetime | N |  |  |  |  |
| 16 | `StockInQty` | decimal(18,0) | N |  |  |  |  |
| 17 | `UnitPrice` | decimal(18,2) | N |  |  |  |  |
| 18 | `TotalPrice` | decimal(18,2) | N |  |  |  |  |
| 19 | `VATPerUnit` | decimal(18,2) | N |  |  |  |  |
| 20 | `TotalVAT` | decimal(18,2) | N |  |  |  |  |
| 21 | `TotalAmount` | decimal(18,2) | N |  |  |  |  |
| 22 | `StockCondition` | nvarchar(50) | N |  |  |  |  |
| 23 | `MigoDetailID` | int | N |  |  |  |  |
| 24 | `DeveloperRemarks` | nvarchar(500) | Y |  |  |  |  |
| 25 | `ProductStockType` | nvarchar(50) | N |  |  |  |  |
| 26 | `InternalNoteNo` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCentralStore_OpeninigBalance` | PK | CLUSTERED | CSOpeninigBalanceId |

**Logical (inferred, not enforced) foreign keys:**

- `ReceiveId` -> `tblCentralStore.ReceiveId`
- `ProductId` -> `tblProduct.ProductId`

---

## `tblChalanDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `ChalanDetailsId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ChalanDetailsId` | int | N |  |  | PK |  |
| 2 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `ProductName` | nvarchar(50) | Y |  |  |  |  |
| 4 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 5 | `BatchNo` | nvarchar(50) | Y |  |  |  |  |
| 6 | `UnitPrice` | decimal(18,2) | Y |  |  |  |  |
| 7 | `Value` | decimal(18,2) | Y |  |  |  |  |
| 8 | `Vat` | decimal(18,2) | Y |  |  |  |  |
| 9 | `ValueWVat` | decimal(18,2) | Y |  |  |  |  |
| 10 | `ChalanId` | int | N |  |  |  |  |
| 11 | `DCStoreId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblChalanDetail` | PK | CLUSTERED | ChalanDetailsId |

**Logical (inferred, not enforced) foreign keys:**

- `ChalanId` -> `tblChalanInfo.ChalanId`
- `DCStoreId` -> `tblDCStore.DCStoreId`

---

## `tblChalanInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `ChalanId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ChalanId` | int | N |  |  | PK |  |
| 2 | `ChalanDate` | date | Y |  |  |  |  |
| 3 | `ChalanNo` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `TrackNo` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `DriverName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `FromComUnitCode` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `FromComUnitName` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `FromComUnitAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `ToComUnitCode` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `ToComUnitName` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `ToComUnitAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 12 | `TotalValue` | decimal(18,2) | Y |  |  |  |  |
| 13 | `TotalVat` | decimal(18,2) | Y |  |  |  |  |
| 14 | `GrandTotal` | decimal(18,2) | Y |  |  |  |  |
| 15 | `ManufacId` | int | Y |  |  |  |  |
| 16 | `IsDeliver` | nvarchar(50) | Y |  |  |  | yes |
| 17 | `FromComUnitId` | int | Y |  |  |  |  |
| 18 | `ChalanBy` | nvarchar(500) | Y |  |  |  |  |
| 19 | `EntryDate` | datetime | Y |  | `(getdate())` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblChalanInfo` | PK | CLUSTERED | ChalanId |

**Logical (inferred, not enforced) foreign keys:**

- `ManufacId` -> `tblManufacturer.ManufacId`

---

## `tblCompanyInfo`

**Status:** COVERED (referenced by app source). **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CompanyId` | int | N | Y |  |  |  |
| 2 | `GroupId` | int | Y |  |  |  |  |
| 3 | `CompanyCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `CompanyName` | nvarchar(500) | Y |  |  |  |  |
| 5 | `CmpShortName` | nvarchar(50) | Y |  |  |  |  |
| 6 | `Address` | nvarchar(250) | Y |  |  |  |  |
| 7 | `ContactNo` | nvarchar(50) | Y |  |  |  |  |
| 8 | `FaxNo` | nvarchar(50) | Y |  |  |  |  |
| 9 | `Remarks` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `EntryDate` | datetime | Y |  |  |  |  |
| 12 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 13 | `UpdateDate` | datetime | Y |  |  |  |  |
| 14 | `CompanyImage` | image | Y |  |  |  |  |
| 15 | `Note` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`

---

## `tblCompanyUnit`

**Status:** COVERED (referenced by app source). **Primary key:** `ComUnitId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ComUnitId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `ComUnitCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `ShortName` | nvarchar(50) | Y |  |  |  |  |
| 5 | `ComUnitName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `Address` | nvarchar(500) | Y |  |  |  |  |
| 7 | `PhoneNo` | nvarchar(50) | Y |  |  |  |  |
| 8 | `MobileNo` | nvarchar(50) | Y |  |  |  |  |
| 9 | `FaxNo` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `EntryDate` | datetime | Y |  |  |  |  |
| 12 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 13 | `UpdateDate` | datetime | Y |  |  |  |  |
| 14 | `DcTypeId` | int | Y |  |  |  |  |
| 15 | `CashGlId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCompanyUnit` | PK | CLUSTERED | ComUnitId |

**Logical (inferred, not enforced) foreign keys:**

- `DcTypeId` -> `tblDistributionType.DcTypeId`

---

## `tblCompanyWiseDeposit`

**Status:** COVERED (referenced by app source). **Primary key:** `DepositId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DepositId` | int | N | Y |  | PK |  |
| 2 | `DepotId` | int | Y |  |  |  |  |
| 3 | `BankId` | int | Y |  |  |  |  |
| 4 | `BranchName` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `DepositType` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `DepositDate` | datetime | Y |  |  |  |  |
| 7 | `Amount` | decimal(18,2) | Y |  |  |  |  |
| 8 | `AccountName` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `CheckNumber` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `CheckDate` | datetime | Y |  |  |  |  |
| 11 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 12 | `EntryBy` | nvarchar(MAX) | Y |  |  |  |  |
| 13 | `EntryDate` | datetime | Y |  |  |  |  |
| 14 | `UpdateBy` | nvarchar(MAX) | Y |  |  |  |  |
| 15 | `UpdateDate` | datetime | Y |  |  |  |  |
| 16 | `IsDelete` | bit | Y |  |  |  | yes |
| 17 | `DeleteBy` | nvarchar(MAX) | Y |  |  |  |  |
| 18 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCompanyWiseDeposit` | PK | CLUSTERED | DepositId |

**Logical (inferred, not enforced) foreign keys:**

- `BankId` -> `tblBankInfo.BankId`

---

## `tblConsumerCustomer`

**Status:** COVERED (referenced by app source). **Primary key:** `ConsumerCustId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ConsumerCustId` | int | N |  |  | PK |  |
| 2 | `CustomerName` | nvarchar(50) | Y |  |  |  |  |
| 3 | `CustomerPhoneNo` | nvarchar(50) | Y |  |  |  |  |
| 4 | `CustomerAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `CustomerCode` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblConsumerCustomer` | PK | CLUSTERED | ConsumerCustId |

---

## `tblCostPriceUpdate`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProductCode` | nvarchar(250) | Y |  |  |  |  |
| 2 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `CostPrice` | decimal(18,2) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tblCreditAdjustment`

**Status:** COVERED (referenced by app source). **Primary key:** `CrditAdjustmentId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CrditAdjustmentId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `CustomerMasterId` | int | Y |  |  |  |  |
| 4 | `InvoiceId` | int | Y |  |  |  |  |
| 5 | `Amount` | decimal(18,2) | Y |  |  |  |  |
| 6 | `ReturnDate` | datetime | Y |  |  |  |  |
| 7 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `EntryDate` | datetime | Y |  |  |  |  |
| 9 | `NTInvoiceId` | int | Y |  |  |  |  |
| 10 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCreditAdjustment` | PK | CLUSTERED | CrditAdjustmentId |

**Logical (inferred, not enforced) foreign keys:**

- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `InvoiceId` -> `tblInvoice.InvoiceId`

---

## `tblCustCategory`

**Status:** COVERED (referenced by app source). **Primary key:** `CategoryId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CategoryId` | int | N |  |  | PK |  |
| 2 | `CategoryCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `CategoryName` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustCategory` | PK | CLUSTERED | CategoryId |

---

## `tblCustMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `CustomerMasterId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustomerMasterId` | int | N | Y |  | PK |  |
| 2 | `CustomerCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `CategoryId` | int | N |  |  |  |  |
| 4 | `CustomerName` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `Address` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `CellNo` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `MarketId` | int | Y |  |  |  |  |
| 8 | `Addrees2` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `City` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `ConPerson` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `ShippingCond` | nvarchar(50) | Y |  |  |  |  |
| 12 | `TermOfPayment` | nvarchar(50) | Y |  |  |  |  |
| 13 | `CustomerCodeOld` | nvarchar(50) | Y |  |  |  |  |
| 14 | `UploadDate` | datetime | Y |  |  |  |  |
| 15 | `ExcelUpload` | bit | Y |  |  |  | yes |
| 16 | `FixedCustomer` | bit | Y |  |  |  | yes |
| 17 | `CreateBy` | nvarchar(50) | Y |  |  |  |  |
| 18 | `CreateDate` | datetime | Y |  |  |  |  |
| 19 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 20 | `UpdateDate` | datetime | Y |  |  |  |  |
| 21 | `IsActive` | bit | Y |  |  |  | yes |
| 22 | `InActiveDate` | datetime | Y |  |  |  |  |
| 23 | `InActiveBy` | nvarchar(50) | Y |  |  |  |  |
| 24 | `IsVatApplicable` | bit | Y |  |  |  | yes |
| 25 | `AITGLId` | int | Y |  |  |  |  |
| 26 | `CustomerTypeId` | int | Y |  |  |  |  |
| 27 | `ProgramTypeId` | int | Y |  |  |  |  |
| 28 | `SMCTypeId` | int | Y |  |  |  |  |
| 29 | `TradelicenseNo` | nvarchar(500) | Y |  |  |  |  |
| 30 | `TradeLicenseExpiryDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `IDX_tblCustMaster_CustomerMasterId` | INDEX | NONCLUSTERED | CustomerMasterId |
| `IDX_tblCustMaster_CustomerMasterId_CustomerCode_CustomerName` | INDEX | NONCLUSTERED | CustomerName,CustomerMasterId,CustomerCode |
| `PK_tblCustMaster` | PK | CLUSTERED | CustomerMasterId |

**Logical (inferred, not enforced) foreign keys:**

- `CategoryId` -> `tblCustCategory.CategoryId`, `tblDoctorCategory.CategoryId`, `tblProCategory.CategoryId`
- `MarketId` -> `tblMarket.MarketId`
- `ProgramTypeId` -> `tblDoctorProgramType.ProgramTypeId`, `tblProgramType.ProgramTypeId`
- `SMCTypeId` -> `tblSMCType.SMCTypeId`

---

## `tblCustomerCreditLimit`

**Status:** COVERED (referenced by app source). **Primary key:** `CreditLimitId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CreditLimitId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | N |  |  |  |  |
| 3 | `CustomerMasterId` | int | N |  |  |  |  |
| 4 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 5 | `LimitAmount` | decimal(18,2) | N |  |  |  |  |
| 6 | `DayLimit` | int | N |  |  |  |  |
| 7 | `IsActive` | bit | N |  |  |  | yes |
| 8 | `EntryBy` | nvarchar(50) | N |  |  |  |  |
| 9 | `EntryDate` | datetime | N |  |  |  |  |
| 10 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `UpdateDate` | datetime | Y |  |  |  |  |
| 12 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 13 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 14 | `StartDate` | datetime | Y |  |  |  |  |
| 15 | `EndDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustomerCreditLimit` | PK | CLUSTERED | CreditLimitId |

**Logical (inferred, not enforced) foreign keys:**

- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`

---

## `tblCustomerFixedDiscount`

**Status:** COVERED (referenced by app source). **Primary key:** `DiscountId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DiscountId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `CustMasterId` | int | Y |  |  |  |  |
| 4 | `DiscountParcentage` | decimal(18,2) | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  |  |  | yes |
| 6 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryDate` | datetime | Y |  |  |  |  |
| 8 | `ActiveDate` | datetime | Y |  |  |  |  |
| 9 | `InactiveDate` | datetime | Y |  |  |  |  |
| 10 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 11 | `ApproveBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `ApproveDate` | datetime | Y |  |  |  | yes |
| 13 | `InactiveBy` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustomerFixedDiscount` | PK | CLUSTERED | DiscountId |

---

## `tblCustomerPay`

**Status:** COVERED (referenced by app source). **Primary key:** `CustPayId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustPayId` | int | N | Y |  | PK |  |
| 2 | `CustomerMasterId` | int | Y |  |  |  |  |
| 3 | `PaymentDate` | datetime | Y |  |  |  |  |
| 4 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `PayType` | nvarchar(50) | Y |  |  |  |  |
| 6 | `RefNo` | nvarchar(50) | Y |  |  |  |  |
| 7 | `RefDate` | datetime | Y |  |  |  |  |
| 8 | `CreateBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `CreateDate` | datetime | Y |  |  |  |  |
| 10 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `UpdateDate` | datetime | Y |  |  |  |  |
| 12 | `PaymentCode` | nvarchar(MAX) | Y |  |  |  |  |
| 13 | `PaymentRemarks` | nvarchar(MAX) | Y |  |  |  |  |
| 14 | `BatchId` | int | Y |  | `((0))` |  |  |
| 15 | `MasterProcessTime` | datetime | Y |  | `(getdate())` |  |  |
| 16 | `InvoiceIdForValidation` | int | Y |  | `((0))` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustomerPay` | PK | CLUSTERED | CustPayId |

**Logical (inferred, not enforced) foreign keys:**

- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `BatchId` -> `tblInvoiceBatch.BatchId`

---

## `tblCustomerPayDeleteLog`

**Status:** UNUSED — no app-code reference found. **Primary key:** `CustPayId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustPayId` | int | N |  |  | PK |  |
| 2 | `CustomerMasterId` | int | Y |  |  |  |  |
| 3 | `PaymentDate` | datetime | Y |  |  |  |  |
| 4 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `PayType` | nvarchar(50) | Y |  |  |  |  |
| 6 | `RefNo` | nvarchar(50) | Y |  |  |  |  |
| 7 | `RefDate` | datetime | Y |  |  |  |  |
| 8 | `CreateBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `CreateDate` | datetime | Y |  |  |  |  |
| 10 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustomerPayDeleteLog` | PK | CLUSTERED | CustPayId |

**Logical (inferred, not enforced) foreign keys:**

- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`

---

## `tblCustomerReturnDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `CustReturnDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustReturnDetailId` | int | N | Y |  | PK |  |
| 2 | `InvoiceDetailId` | int | Y |  |  |  |  |
| 3 | `ReturnQty` | int | Y |  |  |  |  |
| 4 | `ReturnAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `CustReturnId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustomerReturnDetail` | PK | CLUSTERED | CustReturnDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceDetailId` -> `tblInvoiceDetail.InvoiceDetailId`
- `CustReturnId` -> `tblCustomerReturnMaster.CustReturnId`

---

## `tblCustomerReturnMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `CustReturnId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustReturnId` | int | N | Y |  | PK |  |
| 2 | `InvoiceId` | int | Y |  |  |  |  |
| 3 | `TotalQty` | int | Y |  |  |  |  |
| 4 | `TotalAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 10 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 11 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 12 | `IsPosting` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustomerReturnMaster` | PK | CLUSTERED | CustReturnId |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`

---

## `tblCustPayDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `CustPayDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustPayDetailId` | int | N | Y |  | PK |  |
| 2 | `InvoiceId` | int | Y |  |  |  |  |
| 3 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 4 | `CustPayId` | int | Y |  |  |  |  |
| 5 | `IsPrint` | bit | Y |  |  |  | yes |
| 6 | `IsPosting` | bit | Y |  |  |  | yes |
| 7 | `CashAccId` | int | Y |  |  |  |  |
| 8 | `BankAccId` | int | Y |  |  |  |  |
| 9 | `Discount` | decimal(18,2) | Y |  |  |  |  |
| 10 | `PaymentRemarks` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `TransctionDetailId` | int | Y |  |  |  |  |
| 12 | `ProcessTime` | datetime | Y |  |  |  |  |
| 13 | `AIT` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustPayDetail` | PK | CLUSTERED | CustPayDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`
- `CustPayId` -> `tblCustomerPay.CustPayId`, `tblCustomerPayDeleteLog.CustPayId`, `tblPaymentTranscationMaster.CustPayId`

---

## `tblCustPayDetailDeleteLog`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustPayDetailId` | int | N |  |  |  |  |
| 2 | `InvoiceId` | int | Y |  |  |  |  |
| 3 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 4 | `CustPayId` | int | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `CustPayDetailId` -> `tblCustPayDetail.CustPayDetailId`, `tblPaymentTranscationDetail.CustPayDetailId`
- `InvoiceId` -> `tblInvoice.InvoiceId`
- `CustPayId` -> `tblCustomerPay.CustPayId`, `tblCustomerPayDeleteLog.CustPayId`, `tblPaymentTranscationMaster.CustPayId`

---

## `tblCustPayForLastcursor`

**Status:** UNUSED — no app-code reference found. **Primary key:** `CustPayForLast`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustPayForLast` | int | N |  |  | PK |  |
| 2 | `InvoiceId` | int | Y |  |  |  |  |
| 3 | `PmtAmt` | decimal(18,2) | Y |  |  |  |  |
| 4 | `VatAmt` | decimal(18,2) | Y |  |  |  |  |
| 5 | `TpAMt` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblCustPayForLastcursor` | PK | CLUSTERED | CustPayForLast |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`

---

## `tblDCStore`

**Status:** COVERED (referenced by app source). **Primary key:** `DCStoreId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DCStoreId` | int | N |  |  | PK |  |
| 2 | `StorageLocation` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `ProductCode` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `PackSize` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `TotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 8 | `ExpDate` | datetime | Y |  |  |  |  |
| 9 | `ReceiveDate` | datetime | Y |  |  |  |  |
| 10 | `ChalanNo` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `ChalanDate` | datetime | Y |  |  |  |  |
| 12 | `ComUnitId` | int | Y |  |  |  |  |
| 13 | `StockQty` | decimal(18,0) | Y |  |  |  |  |
| 14 | `DamageQty` | decimal(18,0) | Y |  |  |  |  |
| 15 | `StockRcvDate` | datetime | Y |  |  |  |  |
| 16 | `ReqId` | int | Y |  |  |  |  |
| 17 | `ReqChildId` | int | Y |  |  |  |  |
| 18 | `StockInTransfarId` | int | Y |  |  |  |  |
| 19 | `StockCondition` | nvarchar(50) | Y |  |  |  |  |
| 20 | `ChalanDetailsId` | int | Y |  |  |  |  |
| 21 | `MfgDate` | datetime | Y |  |  |  |  |
| 22 | `RefDcStoreId` | int | Y |  |  |  |  |
| 23 | `TranscationDate` | datetime | Y |  |  |  |  |
| 24 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `ReceiveId` | int | Y |  |  |  |  |
| 26 | `ReceiveDateTime` | datetime | Y |  |  |  |  |
| 27 | `TempReceiveId` | int | Y |  |  |  |  |
| 28 | `ReceiveBy` | nvarchar(500) | Y |  |  |  |  |
| 29 | `UnitPrice_CostPrice` | decimal(18,3) | Y |  |  |  |  |
| 30 | `IsBadDue` | bit | Y |  |  |  | yes |
| 31 | `NonTranscationalInvReturnDetailID` | int | Y |  | `((0))` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDCStore` | PK | CLUSTERED | DCStoreId |

**Logical (inferred, not enforced) foreign keys:**

- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `ReqId` -> `tblRequisition.ReqId`
- `ReqChildId` -> `tblRequsitionChild.ReqChildId`
- `StockInTransfarId` -> `tblStockInTransfar.StockInTransfarId`
- `ChalanDetailsId` -> `tblChalanDetail.ChalanDetailsId`
- `ReceiveId` -> `tblCentralStore.ReceiveId`

---

## `tblDCStore_OpeningBalance`

**Status:** COVERED (referenced by app source). **Primary key:** `DCOpeningBalanceId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DCOpeningBalanceId` | int | N | Y |  | PK |  |
| 2 | `DCOpeningBalanceDate` | datetime | N |  |  |  |  |
| 3 | `DCStoreId` | int | N |  |  |  |  |
| 4 | `StorageLocation` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ProductCode` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `PackSize` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `TotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 10 | `ExpDate` | datetime | Y |  |  |  |  |
| 11 | `ReceiveDate` | datetime | Y |  |  |  |  |
| 12 | `ChalanNo` | nvarchar(MAX) | Y |  |  |  |  |
| 13 | `ChalanDate` | datetime | Y |  |  |  |  |
| 14 | `ComUnitId` | int | Y |  |  |  |  |
| 15 | `StockQty` | decimal(18,0) | Y |  |  |  |  |
| 16 | `DamageQty` | decimal(18,0) | Y |  |  |  |  |
| 17 | `StockRcvDate` | datetime | Y |  |  |  |  |
| 18 | `ReqId` | int | Y |  |  |  |  |
| 19 | `ReqChildId` | int | Y |  |  |  |  |
| 20 | `StockInTransfarId` | int | Y |  |  |  |  |
| 21 | `StockCondition` | nvarchar(50) | Y |  |  |  |  |
| 22 | `ChalanDetailsId` | int | Y |  |  |  |  |
| 23 | `MfgDate` | datetime | Y |  |  |  |  |
| 24 | `TempReceiveId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDCStore_OpeningBalance` | PK | CLUSTERED | DCOpeningBalanceId |

**Logical (inferred, not enforced) foreign keys:**

- `DCStoreId` -> `tblDCStore.DCStoreId`
- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `ReqId` -> `tblRequisition.ReqId`
- `ReqChildId` -> `tblRequsitionChild.ReqChildId`
- `StockInTransfarId` -> `tblStockInTransfar.StockInTransfarId`
- `ChalanDetailsId` -> `tblChalanDetail.ChalanDetailsId`

---

## `tblDCStoreFreeze`

**Status:** COVERED (referenced by app source). **Primary key:** `DCStoreFreezeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DCStoreFreezeId` | int | N |  |  | PK |  |
| 2 | `DCStoreId` | int | Y |  |  |  |  |
| 3 | `InvoiceDetailId` | int | Y |  |  |  |  |
| 4 | `StorageLocation` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ProductCode` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `PackSize` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `TotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 10 | `ExpDate` | datetime | Y |  |  |  |  |
| 11 | `ReceiveDate` | datetime | Y |  |  |  |  |
| 12 | `ChalanNo` | nvarchar(MAX) | Y |  |  |  |  |
| 13 | `ChalanDate` | datetime | Y |  |  |  |  |
| 14 | `ComUnitId` | int | Y |  |  |  |  |
| 15 | `StockQty` | decimal(18,0) | Y |  |  |  |  |
| 16 | `DamageQty` | decimal(18,0) | Y |  |  |  |  |
| 17 | `StockRcvDate` | datetime | Y |  |  |  |  |
| 18 | `ReqId` | int | Y |  |  |  |  |
| 19 | `ReqChildId` | int | Y |  |  |  |  |
| 20 | `StockInTransfarId` | int | Y |  |  |  |  |
| 21 | `StockCondition` | nvarchar(50) | Y |  |  |  |  |
| 22 | `ReceiveId` | int | Y |  |  |  |  |
| 23 | `StockConditionFreezeID` | int | Y |  |  |  |  |
| 24 | `ChalanDetailsId` | int | Y |  |  |  |  |
| 25 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 26 | `IsPosting` | bit | Y |  |  |  | yes |
| 27 | `RefFreezeId` | int | Y |  |  |  |  |
| 28 | `UnitPrice_CostPrice` | decimal(18,3) | Y |  |  |  |  |
| 29 | `FreezeRcvDate` | datetime | Y |  | `(getdate())` |  |  |
| 30 | `LastFreezeRcvDate` | datetime | Y |  |  |  |  |
| 31 | `CustReturnDetailId` | int | Y |  |  |  |  |
| 32 | `IsDeliveryDelete` | bit | Y |  | `((0))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDCStoreFreeze` | PK | CLUSTERED | DCStoreFreezeId |

**Logical (inferred, not enforced) foreign keys:**

- `DCStoreId` -> `tblDCStore.DCStoreId`
- `InvoiceDetailId` -> `tblInvoiceDetail.InvoiceDetailId`
- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `ReqId` -> `tblRequisition.ReqId`
- `ReqChildId` -> `tblRequsitionChild.ReqChildId`
- `StockInTransfarId` -> `tblStockInTransfar.StockInTransfarId`
- `ReceiveId` -> `tblCentralStore.ReceiveId`
- `StockConditionFreezeID` -> `tblStockConditionFreeze.StockConditionFreezeID`
- `ChalanDetailsId` -> `tblChalanDetail.ChalanDetailsId`
- `CustReturnDetailId` -> `tblCustomerReturnDetail.CustReturnDetailId`

---

## `tblDcWiseAreaInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `DcWiseAreaId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DcWiseAreaId` | int | N | Y |  | PK |  |
| 2 | `DCId` | int | Y |  |  |  |  |
| 3 | `AreaId` | int | Y |  |  |  |  |
| 4 | `IsActive` | bit | Y |  |  |  | yes |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDcWiseAreaInfo` | PK | CLUSTERED | DcWiseAreaId |

**Logical (inferred, not enforced) foreign keys:**

- `AreaId` -> `tblArea.AreaId`

---

## `tblDeliveryManInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `DeliveryManId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DeliveryManId` | int | N | Y |  | PK |  |
| 2 | `EmpInfoId` | int | Y |  |  |  |  |
| 3 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `IsActive` | bit | Y |  |  |  | yes |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDeliveryManInfo` | PK | CLUSTERED | DeliveryManId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tblDepartment`

**Status:** COVERED (referenced by app source). **Primary key:** `DeptId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DeptId` | int | N |  |  | PK |  |
| 2 | `DeptCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `DeptName` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDepartment` | PK | CLUSTERED | DeptId |

---

## `tblDesignation`

**Status:** COVERED (referenced by app source). **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DesignationId` | int | N |  |  |  |  |
| 2 | `DesigName` | nvarchar(500) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tblDestroyProduct`

**Status:** COVERED (referenced by app source). **Primary key:** `DestroyId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DestroyId` | int | N | Y |  | PK |  |
| 2 | `DcFreezeId` | int | Y |  |  |  |  |
| 3 | `StockOutQty` | decimal(18,2) | Y |  |  |  |  |
| 4 | `StockOutAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `StockOutBy` | int | Y |  |  |  |  |
| 6 | `StockOutDate` | datetime | Y |  |  |  |  |
| 7 | `Reason` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 9 | `ApproveBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `ApproveDate` | datetime | Y |  |  |  | yes |
| 11 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `EntryDate` | datetime | Y |  |  |  |  |
| 13 | `IsApproved` | bit | Y |  |  |  | yes |
| 14 | `IsPosting` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDestroyProduct` | PK | CLUSTERED | DestroyId |

---

## `tblDirectStockOut`

**Status:** COVERED (referenced by app source). **Primary key:** `DCStockOutId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DCStockOutId` | int | N | Y |  | PK |  |
| 2 | `StockOutCode` | nvarchar(500) | Y |  |  |  |  |
| 3 | `DCId` | int | Y |  |  |  |  |
| 4 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 5 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `StockOutDate` | datetime | Y |  |  |  |  |
| 7 | `DCStoreId` | int | Y |  |  |  |  |
| 8 | `StockOutQty` | decimal(18,2) | Y |  |  |  |  |
| 9 | `StockOutValue` | decimal(18,2) | Y |  |  |  |  |
| 10 | `StockOutReason` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `EntryDate` | datetime | Y |  |  |  |  |
| 13 | `StockOutType` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDirectStockOut` | PK | CLUSTERED | DCStockOutId |

**Logical (inferred, not enforced) foreign keys:**

- `DCStoreId` -> `tblDCStore.DCStoreId`

---

## `tblDistributionType`

**Status:** COVERED (referenced by app source). **Primary key:** `DcTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DcTypeId` | int | N | Y |  | PK |  |
| 2 | `DcTypeName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDistributionType` | PK | CLUSTERED | DcTypeId |

---

## `tblDoctor_CustomerDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `CustDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustDetailId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `CustomerId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctor_CustomerDetail` | PK | CLUSTERED | CustDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`

---

## `tblDoctorBrandDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DoctorBrandId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DoctorBrandId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `BrandId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorBrandDetail` | PK | CLUSTERED | DoctorBrandId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`

---

## `tblDoctorCategory`

**Status:** UNUSED — no app-code reference found. **Primary key:** `CategoryId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CategoryId` | int | N | Y |  | PK |  |
| 2 | `CategoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `Activedate` | datetime | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `IsDelate` | bit | Y |  |  |  | yes |
| 10 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorCategory` | PK | CLUSTERED | CategoryId |

---

## `tblDoctorChamber`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ChamberId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ChamberId` | int | N | Y |  | PK |  |
| 2 | `ChamberName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `Activedate` | datetime | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `IsDelate` | bit | Y |  |  |  | yes |
| 10 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorChamber` | PK | CLUSTERED | ChamberId |

---

## `tblDoctorChemberDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ChemberId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ChemberId` | int | N | Y |  | PK |  |
| 2 | `ChamberTypeId` | int | Y |  |  |  |  |
| 3 | `DoctorId` | int | Y |  |  |  |  |
| 4 | `Name` | nvarchar(50) | Y |  |  |  |  |
| 5 | `Phone` | nvarchar(50) | Y |  |  |  |  |
| 6 | `Address` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorChemberDetail` | PK | CLUSTERED | ChemberId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`

---

## `tblDoctorContactDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ContactId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ContactId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `ContactType` | nvarchar(50) | Y |  |  |  |  |
| 4 | `Contact` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ContactTypeId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorContactDetail` | PK | CLUSTERED | ContactId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `ContactTypeId` -> `tbl_ContactType.ContactTypeId`

---

## `tblDoctorCustomerDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DocCustomerId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DocCustomerId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `CustomerCode` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `CustomerName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorCustomerDetail` | PK | CLUSTERED | DocCustomerId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`

---

## `tblDoctorDegree`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DegreeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DegreeId` | int | N | Y |  | PK |  |
| 2 | `DoctorTypeId` | int | Y |  |  |  |  |
| 3 | `DegreeName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `IsActive` | bit | Y |  |  |  | yes |
| 5 | `Activedate` | datetime | Y |  |  |  |  |
| 6 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryDate` | datetime | Y |  |  |  |  |
| 8 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `UpdateDate` | datetime | Y |  |  |  |  |
| 10 | `IsDelate` | bit | Y |  |  |  | yes |
| 11 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorDegree` | PK | CLUSTERED | DegreeId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorTypeId` -> `tblDoctorType.DoctorTypeId`

---

## `tblDoctorDegreeDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DoctorDegId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DoctorDegId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `DegId` | int | N |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorDegreeDetail` | PK | CLUSTERED | DoctorDegId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`

---

## `tblDoctorDesignation`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DesignationName` | nvarchar(MAX) | Y |  |  |  |  |
| 2 | `IsActive` | bit | Y |  |  |  | yes |
| 3 | `DesignationId` | int | N | Y |  |  |  |
| 4 | `Activedate` | datetime | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `IsDelate` | bit | Y |  |  |  | yes |
| 10 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tblDoctorInstitutionDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `InstDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InstDetailId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `InstitutionId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorInstitutionDetail` | PK | CLUSTERED | InstDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `InstitutionId` -> `tblInstitutionInfo.InstitutionId`

---

## `tblDoctorMarketDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DoctorMktId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DoctorMktId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `SubmarketId` | int | Y |  |  |  |  |
| 4 | `IsTopDoctor` | bit | Y |  |  |  | yes |
| 5 | `IsDCRAllowed` | bit | Y |  |  |  | yes |
| 6 | `MarketId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorMarketDetail` | PK | CLUSTERED | DoctorMktId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `MarketId` -> `tblMarket.MarketId`

---

## `tblDoctorMaster`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DoctorId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DoctorId` | int | N | Y |  | PK |  |
| 2 | `DoctorCode` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `DoctorName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `DesignationId` | int | Y |  |  |  |  |
| 5 | `DegreeId` | int | Y |  |  |  |  |
| 6 | `Gender` | nvarchar(50) | Y |  |  |  |  |
| 7 | `Speciality` | int | Y |  |  |  |  |
| 8 | `ProgramType` | int | Y |  |  |  |  |
| 9 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `IsActive` | bit | Y |  |  |  | yes |
| 11 | `Activedate` | datetime | Y |  |  |  |  |
| 12 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 13 | `EntryDate` | datetime | Y |  |  |  |  |
| 14 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 15 | `UpdateDate` | datetime | Y |  |  |  |  |
| 16 | `InactiveDate` | datetime | Y |  |  |  |  |
| 17 | `InactiveBy` | nvarchar(50) | Y |  |  |  |  |
| 18 | `IsDelate` | bit | Y |  |  |  | yes |
| 19 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 20 | `DeleteDate` | datetime | Y |  |  |  |  |
| 21 | `DoctorTypeId` | int | Y |  |  |  |  |
| 22 | `SMCTypeId` | int | Y |  |  |  |  |
| 23 | `ProgramTypeId` | int | Y |  |  |  |  |
| 24 | `MarketId` | int | Y |  |  |  |  |
| 25 | `ApprovalStatus` | nvarchar(50) | Y |  |  |  | yes |
| 26 | `UnionName` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `StationTypeId` | int | Y |  |  |  |  |
| 28 | `DoctorCategoryId` | int | Y |  |  |  |  |
| 29 | `DoctorAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `IsFromApp` | bit | Y |  |  |  | yes |
| 31 | `SecondaryCode` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorMaster` | PK | CLUSTERED | DoctorId |

**Logical (inferred, not enforced) foreign keys:**

- `DegreeId` -> `tblDoctorDegree.DegreeId`
- `DoctorTypeId` -> `tblDoctorType.DoctorTypeId`
- `SMCTypeId` -> `tblSMCType.SMCTypeId`
- `ProgramTypeId` -> `tblDoctorProgramType.ProgramTypeId`, `tblProgramType.ProgramTypeId`
- `MarketId` -> `tblMarket.MarketId`

---

## `tblDoctorPatientType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `PatientTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PatientTypeId` | int | N | Y |  | PK |  |
| 2 | `PatientType` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `Activedate` | datetime | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `IsDelate` | bit | Y |  |  |  | yes |
| 10 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorPatientType` | PK | CLUSTERED | PatientTypeId |

---

## `tblDoctorProgramType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ProgramTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProgramTypeId` | int | N | Y |  | PK |  |
| 2 | `ProgramType` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `CreatedBy` | nvarchar(50) | Y |  |  |  |  |
| 4 | `CreatedDate` | datetime | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  | `((1))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorProgramType` | PK | CLUSTERED | ProgramTypeId |

---

## `tblDoctorProgramTypeDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DoctorTypeDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DoctorTypeDetailId` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `ProgramTypeId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorProgramTypeDetail` | PK | CLUSTERED | DoctorTypeDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `ProgramTypeId` -> `tblDoctorProgramType.ProgramTypeId`, `tblProgramType.ProgramTypeId`

---

## `tblDoctorSpecialDay`

**Status:** UNUSED — no app-code reference found. **Primary key:** `SpecialDayId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SpecialDayId` | int | N | Y |  | PK |  |
| 2 | `SpecialDay` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `Activedate` | datetime | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `IsDelate` | bit | Y |  |  |  | yes |
| 10 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorSpecialDay` | PK | CLUSTERED | SpecialDayId |

---

## `tblDoctorSpecialDayDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `SpecialDayInt`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SpecialDayInt` | int | N | Y |  | PK |  |
| 2 | `DoctorId` | int | Y |  |  |  |  |
| 3 | `SpecialDayId` | int | Y |  |  |  |  |
| 4 | `SpecialDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorSpecialDayDetail` | PK | CLUSTERED | SpecialDayInt |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`
- `SpecialDayId` -> `tblDoctorSpecialDay.SpecialDayId`

---

## `tblDoctorSpeciality`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SpecialityId` | int | N | Y |  |  |  |
| 2 | `SpecialityName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `Activedate` | datetime | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |
| 9 | `IsDelate` | bit | Y |  |  |  | yes |
| 10 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tblDoctorSpecialityDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DoctorSpId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DoctorSpId` | int | N | Y |  | PK |  |
| 2 | `SpecialityId` | int | Y |  |  |  |  |
| 3 | `DoctorId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorSpecialityDetail` | PK | CLUSTERED | DoctorSpId |

**Logical (inferred, not enforced) foreign keys:**

- `DoctorId` -> `tblDoctorMaster.DoctorId`

---

## `tblDoctorType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `DoctorTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DoctorTypeId` | int | N | Y |  | PK |  |
| 2 | `DoctorTypeName` | nvarchar(500) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |
| 4 | `DoctorTypeCode` | nvarchar(500) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblDoctorType` | PK | CLUSTERED | DoctorTypeId |

---

## `tblEmpGeneralInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `EmpInfoId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `EmpInfoId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `EmpMasterCode` | nvarchar(200) | Y |  |  |  |  |
| 4 | `EmpName` | nvarchar(200) | Y |  |  |  |  |
| 5 | `ShortName` | nvarchar(200) | Y |  |  |  |  |
| 6 | `FatherName` | nvarchar(200) | Y |  |  |  |  |
| 7 | `MotherName` | nvarchar(200) | Y |  |  |  |  |
| 8 | `Religion` | nvarchar(200) | Y |  |  |  |  |
| 9 | `Nationality` | nvarchar(200) | Y |  |  |  |  |
| 10 | `DateOfBirth` | datetime | Y |  |  |  |  |
| 11 | `PlaceOfBirth` | nvarchar(200) | Y |  |  |  |  |
| 12 | `BloodGroup` | nvarchar(200) | Y |  |  |  |  |
| 13 | `Gender` | nvarchar(50) | Y |  |  |  |  |
| 14 | `AddressPresent` | nvarchar(MAX) | Y |  |  |  |  |
| 15 | `AddressPermanent` | nvarchar(MAX) | Y |  |  |  |  |
| 16 | `PreCity` | nvarchar(200) | Y |  |  |  |  |
| 17 | `PerCity` | nvarchar(200) | Y |  |  |  |  |
| 18 | `EmpImage` | image | Y |  |  |  |  |
| 19 | `SignatureImage` | image | Y |  |  |  |  |
| 20 | `NationalIdNo` | nvarchar(50) | Y |  |  |  |  |
| 21 | `CellNumber` | nvarchar(200) | Y |  |  |  |  |
| 22 | `Email` | nvarchar(200) | Y |  |  |  |  |
| 23 | `EmrgContactNo` | nvarchar(200) | Y |  |  |  |  |
| 24 | `MaritalStatus` | nvarchar(200) | Y |  |  |  | yes |
| 25 | `RefName` | nvarchar(200) | Y |  |  |  |  |
| 26 | `RefContactNo` | nvarchar(200) | Y |  |  |  |  |
| 27 | `DesignationId` | int | N |  |  |  |  |
| 28 | `DepartmentId` | int | N |  |  |  |  |
| 29 | `JoiningDate` | datetime | Y |  |  |  |  |
| 30 | `EntryBy` | nvarchar(200) | Y |  |  |  |  |
| 31 | `EntryDate` | datetime | Y |  |  |  |  |
| 32 | `UpdateBy` | nvarchar(200) | Y |  |  |  |  |
| 33 | `UpdateDate` | datetime | Y |  |  |  |  |
| 34 | `ShiftId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblEmpGeneralInfo` | PK | CLUSTERED | EmpInfoId |

**Logical (inferred, not enforced) foreign keys:**

- `ShiftId` -> `tbl_Shift.ShiftId`

---

## `tblEmpLatLong`

**Status:** UNUSED — no app-code reference found. **Primary key:** `EmpLatLongId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `EmpLatLongId` | int | N | Y |  | PK |  |
| 2 | `Lat` | decimal(18,10) | Y |  |  |  |  |
| 3 | `Lon` | decimal(18,10) | Y |  |  |  |  |
| 4 | `EmpInfoId` | int | Y |  |  |  |  |
| 5 | `CompanyId` | int | Y |  |  |  |  |
| 6 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryDate` | datetime | Y |  |  |  |  |
| 8 | `ShiftId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblEmpLatLong` | PK | CLUSTERED | EmpLatLongId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `ShiftId` -> `tbl_Shift.ShiftId`

---

## `tblGroupInformation`

**Status:** COVERED (referenced by app source). **Primary key:** `GroupId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `GroupId` | int | N | Y |  | PK |  |
| 2 | `GroupName` | nvarchar(500) | Y |  |  |  |  |
| 3 | `GrpShortName` | nvarchar(50) | Y |  |  |  |  |
| 4 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 5 | `EntryDate` | datetime | Y |  |  |  |  |
| 6 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblGroupInformation` | PK | CLUSTERED | GroupId |

---

## `tblGroupWisePromoQty`

**Status:** UNUSED — no app-code reference found. **Primary key:** `GWPromoQtyId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `GWPromoQtyId` | int | N | Y |  | PK |  |
| 2 | `Year` | int | Y |  |  |  |  |
| 3 | `Month` | nvarchar(500) | Y |  |  |  |  |
| 4 | `PromoGroupId` | int | Y |  |  |  |  |
| 5 | `Qty` | decimal(18,0) | Y |  |  |  |  |
| 6 | `Date` | datetime | Y |  |  |  |  |
| 7 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `EntryDate` | datetime | Y |  |  |  |  |
| 9 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `UpdateDate` | datetime | Y |  |  |  |  |
| 11 | `ProductId` | int | Y |  |  |  |  |
| 12 | `MIOId` | int | Y |  |  |  |  |
| 13 | `EmpInfoId` | int | Y |  |  |  |  |
| 14 | `AllocationCode` | nvarchar(500) | Y |  |  |  |  |
| 15 | `TerritoryId` | int | Y |  |  |  |  |
| 16 | `TransactionQTY` | decimal(18,0) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblGroupWisePromoQty` | PK | CLUSTERED | GWPromoQtyId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`
- `MIOId` -> `tblMIOInfo.MIOId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `TerritoryId` -> `tblTerritory.TerritoryId`

---

## `tblIngridents`

**Status:** COVERED (referenced by app source). **Primary key:** `IngridentsId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `IngridentsId` | int | N |  |  | PK |  |
| 2 | `IngridentsName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IngridentsType` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblIngridents` | PK | CLUSTERED | IngridentsId |

---

## `tblInstitutionInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `InstitutionId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InstitutionId` | int | N | Y |  | PK |  |
| 2 | `Institution` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblInstitutionInfo` | PK | CLUSTERED | InstitutionId |

---

## `tblInvoice`

**Status:** COVERED (referenced by app source). **Primary key:** `InvoiceId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvoiceId` | int | N | Y |  | PK |  |
| 2 | `InvoiceNo` | nvarchar(200) | N |  |  |  |  |
| 3 | `InvoiceDate` | datetime | Y |  |  |  |  |
| 4 | `OrderId` | int | Y |  |  |  |  |
| 5 | `OrderNo` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `OrderDate` | datetime | Y |  |  |  |  |
| 7 | `CustomerMasterId` | int | Y |  |  |  |  |
| 8 | `ComUnitId` | int | Y |  |  |  |  |
| 9 | `MiaId` | int | Y |  |  |  |  |
| 10 | `PaymentTypeId` | int | Y |  |  |  |  |
| 11 | `TpTotal` | decimal(18,2) | Y |  |  |  |  |
| 12 | `TpDiscount` | decimal(18,2) | Y |  |  |  |  |
| 13 | `TpVat` | decimal(18,2) | Y |  |  |  |  |
| 14 | `TpGrandTotal` | decimal(18,2) | Y |  |  |  |  |
| 15 | `UserId` | int | Y |  |  |  |  |
| 16 | `DeliveryTpTotal` | decimal(18,2) | Y |  |  |  |  |
| 17 | `DeliveryTpDiscount` | decimal(18,2) | Y |  |  |  |  |
| 18 | `DeliveryTpVat` | decimal(18,2) | Y |  |  |  |  |
| 19 | `DeliveryTpGrandTotal` | decimal(18,2) | Y |  |  |  |  |
| 20 | `DeliveryInvoiceStatus` | nvarchar(50) | Y |  |  |  | yes |
| 21 | `DelivaryInvoiceNo` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `CreateBy` | nvarchar(50) | Y |  |  |  |  |
| 23 | `CreateDate` | datetime | Y |  |  |  |  |
| 24 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 25 | `UpdateDate` | datetime | Y |  |  |  |  |
| 26 | `TotalSpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 27 | `DelivarySpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 28 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 29 | `PaymentStatus` | nvarchar(50) | Y |  |  |  | yes |
| 30 | `ProductOffer` | nvarchar(50) | Y |  |  |  |  |
| 31 | `OldTradePolicy` | nvarchar(50) | Y |  |  |  |  |
| 32 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 33 | `DeliveryManId` | int | Y |  |  |  |  |
| 34 | `BatchId` | int | Y |  |  |  |  |
| 35 | `IsDirect` | bit | Y |  |  |  | yes |
| 36 | `IsPosting` | bit | Y |  |  |  | yes |
| 37 | `AIT` | decimal(18,2) | Y |  |  |  |  |
| 38 | `DiscountOnPayment` | decimal(18,2) | Y |  |  |  |  |
| 39 | `PONumber` | nvarchar(MAX) | Y |  |  |  |  |
| 40 | `IsBadDue` | bit | Y |  | `((0))` |  | yes |
| 41 | `ProcessTirm` | datetime | Y |  | `(getdate())` |  |  |
| 42 | `GroupInvoiceNo` | nvarchar(200) | Y |  |  |  |  |
| 43 | `GroupInvoiceDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `index_DCWiseInvoice` | INDEX | NONCLUSTERED | ComUnitId,CustomerMasterId |
| `IX_tblInvoice_OrderId` | INDEX | NONCLUSTERED | PaymentStatus,DelivaryInvoiceNo,InvoiceId,OrderId |
| `PK_tblInvoice` | PK | CLUSTERED | InvoiceId |
| `UQ_tblInvoice_InvoiceNo` | UNIQUE | NONCLUSTERED | InvoiceNo |

**Logical (inferred, not enforced) foreign keys:**

- `OrderId` -> `tblOrder.OrderId`, `tblSampleIssue.OrderId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `PaymentTypeId` -> `tblPaymentType.PaymentTypeId`
- `UserId` -> `tblUser.UserId`
- `DeliveryManId` -> `tblDeliveryManInfo.DeliveryManId`
- `BatchId` -> `tblInvoiceBatch.BatchId`

---

## `tblInvoiceBatch`

**Status:** COVERED (referenced by app source). **Primary key:** `BatchId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `BatchId` | int | N | Y |  | PK |  |
| 2 | `BatchCode` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `BatchNo` | int | Y |  |  |  |  |
| 4 | `CompanyId` | int | Y |  |  |  |  |
| 5 | `Date` | datetime | Y |  |  |  |  |
| 6 | `DeliveryManId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblInvoiceBatch` | PK | CLUSTERED | BatchId |

**Logical (inferred, not enforced) foreign keys:**

- `DeliveryManId` -> `tblDeliveryManInfo.DeliveryManId`

---

## `tblInvoiceDeleteLog`

**Status:** COVERED (referenced by app source). **Primary key:** `IDelLogId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `IDelLogId` | int | N | Y |  | PK |  |
| 2 | `InvoiceId` | int | Y |  |  |  |  |
| 3 | `InvoiceNo` | nvarchar(500) | Y |  |  |  |  |
| 4 | `DelInvoiceNo` | nvarchar(500) | Y |  |  |  |  |
| 5 | `DeleteDateTime` | datetime | Y |  |  |  |  |
| 6 | `SampleId` | int | Y |  |  |  |  |
| 7 | `SampleInvoiceNo` | nvarchar(500) | Y |  |  |  |  |
| 8 | `DeleteBy` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblInvoiceDeleteLog` | PK | CLUSTERED | IDelLogId |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`

---

## `tblInvoiceDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `InvoiceDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvoiceDetailId` | int | N | Y |  | PK |  |
| 2 | `ProductCode` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `PackSize` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `ReceiveDate` | datetime | Y |  |  |  |  |
| 7 | `ExpDate` | datetime | Y |  |  |  |  |
| 8 | `CostPrice` | decimal(18,2) | Y |  |  |  |  |
| 9 | `UnitPrice` | decimal(18,2) | Y |  |  |  |  |
| 10 | `UnitVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 11 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 12 | `BonusQuantity` | decimal(18,0) | Y |  |  |  |  |
| 13 | `TotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 14 | `TotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 15 | `TotalPriceVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 16 | `DiscountPercentage` | decimal(18,2) | Y |  |  |  |  |
| 17 | `DiscountAmount` | decimal(18,2) | Y |  |  |  |  |
| 18 | `NetAmount` | decimal(18,2) | Y |  |  |  |  |
| 19 | `InvoiceId` | int | Y |  |  |  |  |
| 20 | `DCStoreId` | int | Y |  |  |  |  |
| 21 | `DeliveryQuantity` | decimal(18,0) | Y |  |  |  |  |
| 22 | `DeliveryBonusQuantity` | decimal(18,0) | Y |  |  |  |  |
| 23 | `DeliveryTotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 24 | `DeliveryTotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 25 | `DeliveryTotalPriceVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 26 | `DeliveryDiscountPercentage` | decimal(18,2) | Y |  |  |  |  |
| 27 | `DeliveryDiscountAmount` | decimal(18,2) | Y |  |  |  |  |
| 28 | `DeliveryNetAmount` | decimal(18,2) | Y |  |  |  |  |
| 29 | `DeliveryStatus` | nvarchar(50) | Y |  |  |  | yes |
| 30 | `OrderDetailsId` | int | Y |  |  |  |  |
| 31 | `SpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 32 | `DelivarySpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 33 | `ReturnReason` | nvarchar(500) | Y |  |  |  |  |
| 34 | `ProcTime` | datetime | Y |  | `(getdate())` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblInvoiceDetail` | PK | CLUSTERED | InvoiceDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`
- `DCStoreId` -> `tblDCStore.DCStoreId`

---

## `tblInvoiceDetail_DeleterRecord`

**Status:** COVERED (referenced by app source). **Primary key:** `InvoiceDetailDR_Id`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvoiceDetailDR_Id` | int | N | Y |  | PK |  |
| 2 | `InvoiceDetailId` | int | N |  |  |  |  |
| 3 | `ProductCode` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `PackSize` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `ReceiveDate` | datetime | Y |  |  |  |  |
| 8 | `ExpDate` | datetime | Y |  |  |  |  |
| 9 | `CostPrice` | decimal(18,2) | Y |  |  |  |  |
| 10 | `UnitPrice` | decimal(18,2) | Y |  |  |  |  |
| 11 | `UnitVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 12 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 13 | `BonusQuantity` | decimal(18,0) | Y |  |  |  |  |
| 14 | `TotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 15 | `TotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 16 | `TotalPriceVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 17 | `DiscountPercentage` | decimal(18,2) | Y |  |  |  |  |
| 18 | `DiscountAmount` | decimal(18,2) | Y |  |  |  |  |
| 19 | `NetAmount` | decimal(18,2) | Y |  |  |  |  |
| 20 | `InvoiceId` | int | Y |  |  |  |  |
| 21 | `DCStoreId` | int | Y |  |  |  |  |
| 22 | `DeliveryQuantity` | decimal(18,0) | Y |  |  |  |  |
| 23 | `DeliveryBonusQuantity` | decimal(18,0) | Y |  |  |  |  |
| 24 | `DeliveryTotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 25 | `DeliveryTotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 26 | `DeliveryTotalPriceVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 27 | `DeliveryDiscountPercentage` | decimal(18,2) | Y |  |  |  |  |
| 28 | `DeliveryDiscountAmount` | decimal(18,2) | Y |  |  |  |  |
| 29 | `DeliveryNetAmount` | decimal(18,2) | Y |  |  |  |  |
| 30 | `DeliveryStatus` | nvarchar(50) | Y |  |  |  | yes |
| 31 | `OrderDetailsId` | int | Y |  |  |  |  |
| 32 | `SpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 33 | `DelivarySpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 34 | `ReturnReason` | nvarchar(500) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblInvoiceDetail_DeleterRecord` | PK | CLUSTERED | InvoiceDetailDR_Id |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceDetailId` -> `tblInvoiceDetail.InvoiceDetailId`
- `InvoiceId` -> `tblInvoice.InvoiceId`
- `DCStoreId` -> `tblDCStore.DCStoreId`

---

## `tblLeaveAvail`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `LeaveAvailId` | int | N | Y |  |  |  |
| 2 | `EmpMasterCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `EmpName` | nvarchar(50) | Y |  |  |  |  |
| 4 | `LeaveInventoryId` | int | N |  |  |  |  |
| 5 | `FromDate` | smalldatetime | Y |  |  |  |  |
| 6 | `ToDate` | smalldatetime | Y |  |  |  |  |
| 7 | `EmpInfoId` | int | N |  |  |  |  |
| 8 | `AvailLeaveQty` | decimal(18,2) | Y |  |  |  |  |
| 9 | `LeaveReason` | nvarchar(50) | Y |  |  |  |  |
| 10 | `LeaveName` | nvarchar(50) | Y |  |  |  |  |
| 11 | `Status` | nvarchar(50) | Y |  |  |  | yes |
| 12 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 13 | `EntryDate` | smalldatetime | Y |  |  |  |  |
| 14 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 15 | `ApprovedDate` | smalldatetime | Y |  |  |  | yes |
| 16 | `DeleteBy` | nvarchar(MAX) | Y |  |  |  |  |
| 17 | `DeleteDate` | datetime | Y |  |  |  |  |
| 18 | `IsActive` | bit | Y |  |  |  | yes |
| 19 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 20 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 21 | `UpdateDate` | smalldatetime | Y |  |  |  |  |
| 22 | `RoleType_L` | nvarchar(50) | Y |  |  |  |  |
| 23 | `NextEmpId` | int | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tblLeaveAvailApp`

**Status:** UNUSED — no app-code reference found. **Primary key:** `LeaveAvailAppId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `LeaveAvailAppId` | int | N | Y |  | PK |  |
| 2 | `LeaveAvailId` | int | Y |  |  |  |  |
| 3 | `PreEmpId` | int | Y |  |  |  |  |
| 4 | `ForEmpId` | int | Y |  |  |  |  |
| 5 | `ActionVersion` | int | Y |  |  |  |  |
| 6 | `ApprovalStatus` | nvarchar(50) | Y |  |  |  | yes |
| 7 | `PreTeriitoryId` | int | Y |  |  |  |  |
| 8 | `ForTeriitoryId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblLeaveAvailApp` | PK | CLUSTERED | LeaveAvailAppId |

---

## `tblLeaveInventory`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `LeaveInventoryId` | int | N |  |  |  |  |
| 2 | `EmpInfoId` | int | N |  |  |  |  |
| 3 | `EmpMasterCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `EmpName` | nvarchar(50) | Y |  |  |  |  |
| 5 | `LeaveName` | nvarchar(50) | Y |  |  |  |  |
| 6 | `DayQty` | decimal(18,0) | Y |  |  |  |  |
| 7 | `LeaveYear` | nvarchar(50) | Y |  |  |  |  |
| 8 | `LeaveId` | int | N |  |  |  |  |
| 9 | `YearDayQty` | decimal(18,0) | Y |  |  |  |  |
| 10 | `DeleteBy` | nvarchar(MAX) | Y |  |  |  |  |
| 11 | `DeleteDate` | datetime | Y |  |  |  |  |
| 12 | `IsActive` | bit | Y |  |  |  | yes |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tblLoginLog`

**Status:** COVERED (referenced by app source). **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `LogId` | int | N | Y |  |  |  |
| 2 | `UserId` | int | Y |  |  |  |  |
| 3 | `LoginName` | nvarchar(500) | Y |  |  |  |  |
| 4 | `LoginTime` | datetime | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`

---

## `tblMainMenu`

**Status:** COVERED (referenced by app source). **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SL` | int | Y |  |  |  |  |
| 2 | `ManuName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `URL` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `ParantId` | nvarchar(50) | Y |  |  |  |  |
| 5 | `IsApprovalPage` | bit | Y |  |  |  | yes |
| 6 | `ModuleId` | int | Y |  |  |  |  |
| 7 | `IsIntegration` | bit | Y |  | `((0))` |  | yes |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `SL` -> `tblMenuDistribution.SL`

---

## `tblMainMenuNew`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SL` | int | N |  |  |  |  |
| 2 | `ManuName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `URL` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `ParantId` | nvarchar(50) | Y |  |  |  |  |
| 5 | `IsApprovalPage` | bit | Y |  |  |  | yes |
| 6 | `Class` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `Icon` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `SystemLink` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `TypeId` | int | Y |  |  |  |  |
| 10 | `Add` | bit | Y |  |  |  | yes |
| 11 | `View` | bit | Y |  |  |  | yes |
| 12 | `Delete` | bit | Y |  |  |  | yes |
| 13 | `Edit` | bit | Y |  |  |  | yes |
| 14 | `IsApp` | bit | Y |  |  |  | yes |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `SL` -> `tblMenuDistribution.SL`

---

## `tblManufacturer`

**Status:** COVERED (referenced by app source). **Primary key:** `ManufacId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ManufacId` | int | N | Y |  | PK |  |
| 2 | `ManufacName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `ManufacAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `ManufacCode` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |
| 7 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblManufacturer` | PK | CLUSTERED | ManufacId |

---

## `tblManuMenuOperations`

**Status:** UNUSED — no app-code reference found. **Primary key:** `SuboperationId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SuboperationId` | int | N | Y |  | PK |  |
| 2 | `MenuId` | int | Y |  |  |  |  |
| 3 | `OperationTitle` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblManuMenuOperations` | PK | CLUSTERED | SuboperationId |

---

## `tblMarket`

**Status:** COVERED (referenced by app source). **Primary key:** `MarketId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `MarketId` | int | N | Y |  | PK |  |
| 2 | `TerritoryId` | int | Y |  |  |  |  |
| 3 | `MarketCode` | nvarchar(500) | Y |  |  |  |  |
| 4 | `MarketName` | nvarchar(500) | Y |  |  |  |  |
| 5 | `MktShortName` | nvarchar(50) | Y |  |  |  |  |
| 6 | `IsActive` | bit | Y |  |  |  | yes |
| 7 | `Description` | nvarchar(500) | Y |  |  |  |  |
| 8 | `Remarks` | nvarchar(500) | Y |  |  |  |  |
| 9 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMarket` | PK | CLUSTERED | MarketId |

**Logical (inferred, not enforced) foreign keys:**

- `TerritoryId` -> `tblTerritory.TerritoryId`

---

## `tblMarketAttendace_Tracking_webapi`

**Status:** UNUSED — no app-code reference found. **Primary key:** `AttendanceDetailsId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `AttendanceDetailsId` | int | N | Y |  | PK |  |
| 2 | `AttendanceId` | int | Y |  |  |  |  |
| 3 | `LatValue` | nvarchar(500) | Y |  |  |  |  |
| 4 | `LongValue` | nvarchar(500) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tbl_MarketAttendace_Tracking` | PK | CLUSTERED | AttendanceDetailsId |

**Logical (inferred, not enforced) foreign keys:**

- `AttendanceId` -> `tblMarketAttendance_Master_webapi.AttendanceId`, `tblMarketAttendance_Master_webapi_backup.AttendanceId`

---

## `tblMarketAttendance_Master_webapi`

**Status:** UNUSED — no app-code reference found. **Primary key:** `AttendanceId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `AttendanceId` | int | N | Y |  | PK |  |
| 2 | `MIOId` | int | Y |  |  |  |  |
| 3 | `EmpInfoId` | int | N |  |  |  |  |
| 4 | `PunchInTime` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `PInLat` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `PInLog` | nvarchar(50) | Y |  |  |  |  |
| 7 | `PunchOutTime` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `POutLat` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `POutLong` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `POutRemarks` | nvarchar(MAX) | Y |  |  |  |  |
| 12 | `AttendanceDate` | datetime | Y |  |  |  |  |
| 13 | `PINCreatedDateTime` | datetime | Y |  |  |  |  |
| 14 | `POUTCreatedDateTime` | datetime | Y |  |  |  |  |
| 15 | `Status` | bit | Y |  | `((0))` |  | yes |
| 16 | `UserRoleID` | int | Y |  |  |  |  |
| 17 | `ShiftId` | int | Y |  |  |  |  |
| 18 | `CompanyId` | int | Y |  |  |  |  |
| 19 | `GroupId` | int | Y |  |  |  |  |
| 20 | `GroupCode` | nvarchar(MAX) | Y |  |  |  |  |
| 21 | `GroupName` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `RegionId` | int | Y |  |  |  |  |
| 23 | `RegionCode` | nvarchar(MAX) | Y |  |  |  |  |
| 24 | `RegionName` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `AreaId` | int | Y |  |  |  |  |
| 26 | `AreaCode` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `AreaName` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `TerritoryId` | int | Y |  |  |  |  |
| 29 | `TerritoryCode` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `TerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 32 | `AttStatus` | nvarchar(MAX) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMarketAttendance_Master` | PK | CLUSTERED | AttendanceId |

**Logical (inferred, not enforced) foreign keys:**

- `MIOId` -> `tblMIOInfo.MIOId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `UserRoleID` -> `tbl_UserRoleInfo.UserRoleID`
- `ShiftId` -> `tbl_Shift.ShiftId`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`

---

## `tblMarketAttendance_Master_webapi_backup`

**Status:** UNUSED — no app-code reference found. **Primary key:** `AttendanceId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `AttendanceId` | int | N |  |  | PK |  |
| 2 | `MIOId` | int | Y |  |  |  |  |
| 3 | `EmpInfoId` | int | N |  |  |  |  |
| 4 | `PunchInTime` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `PInLat` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `PInLog` | nvarchar(50) | Y |  |  |  |  |
| 7 | `PunchOutTime` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `POutLat` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `POutLong` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `POutRemarks` | nvarchar(MAX) | Y |  |  |  |  |
| 12 | `AttendanceDate` | datetime | Y |  |  |  |  |
| 13 | `PINCreatedDateTime` | datetime | Y |  |  |  |  |
| 14 | `POUTCreatedDateTime` | datetime | Y |  |  |  |  |
| 15 | `Status` | bit | Y |  |  |  | yes |
| 16 | `UserRoleID` | int | Y |  |  |  |  |
| 17 | `ShiftId` | int | Y |  |  |  |  |
| 18 | `CompanyId` | int | Y |  |  |  |  |
| 19 | `GroupId` | int | Y |  |  |  |  |
| 20 | `GroupCode` | nvarchar(MAX) | Y |  |  |  |  |
| 21 | `GroupName` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `RegionId` | int | Y |  |  |  |  |
| 23 | `RegionCode` | nvarchar(MAX) | Y |  |  |  |  |
| 24 | `RegionName` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `AreaId` | int | Y |  |  |  |  |
| 26 | `AreaCode` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `AreaName` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `TerritoryId` | int | Y |  |  |  |  |
| 29 | `TerritoryCode` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `TerritoryName` | nvarchar(MAX) | Y |  |  |  |  |
| 31 | `AttStatus` | nvarchar(MAX) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMarketAttendance_Master_backup` | PK | CLUSTERED | AttendanceId |

**Logical (inferred, not enforced) foreign keys:**

- `MIOId` -> `tblMIOInfo.MIOId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `UserRoleID` -> `tbl_UserRoleInfo.UserRoleID`
- `ShiftId` -> `tbl_Shift.ShiftId`
- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`

---

## `tblMarketStationDetail`

**Status:** UNUSED — no app-code reference found. **Primary key:** `MarketStationDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `MarketId` | int | Y |  |  |  |  |
| 2 | `StationTypeId` | int | Y |  |  |  |  |
| 3 | `UserRoleID` | int | Y |  |  |  |  |
| 4 | `MarketStationDetailId` | int | N | Y |  | PK |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMarketStationDetail` | PK | CLUSTERED | MarketStationDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `MarketId` -> `tblMarket.MarketId`
- `UserRoleID` -> `tbl_UserRoleInfo.UserRoleID`

---

## `tblMenuDistribution`

**Status:** COVERED (referenced by app source). **Primary key:** `SL`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SL` | int | N |  |  | PK |  |
| 2 | `UserId` | int | Y |  |  |  |  |
| 3 | `MenuSL` | int | Y |  |  |  |  |
| 4 | `Status` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMenuDistribution` | PK | CLUSTERED | SL |

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`

---

## `tblMenuRole`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `MenuRoleId` | int | N | Y |  |  |  |
| 2 | `SL` | int | Y |  |  |  |  |
| 3 | `RoleId` | int | Y |  |  |  |  |
| 4 | `Add` | bit | Y |  |  |  | yes |
| 5 | `View` | bit | Y |  |  |  | yes |
| 6 | `Delete` | bit | Y |  |  |  | yes |
| 7 | `Edit` | bit | Y |  |  |  | yes |
| 8 | `Permission` | bit | Y |  |  |  | yes |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `SL` -> `tblMenuDistribution.SL`

---

## `tblMIATarget`

**Status:** COVERED (referenced by app source). **Primary key:** `MiaTargetId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `MiaTargetId` | int | N |  |  | PK |  |
| 2 | `MiaId` | int | Y |  |  |  |  |
| 3 | `MiaName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `MiaTargetAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `Period` | nvarchar(50) | Y |  |  |  |  |
| 6 | `Year` | nvarchar(50) | Y |  |  |  |  |
| 7 | `CompanyId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMIATarget` | PK | CLUSTERED | MiaTargetId |

---

## `tblMIATargetProductWise`

**Status:** COVERED (referenced by app source). **Primary key:** `MiaTargetId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `MiaTargetId` | int | N | Y |  | PK |  |
| 2 | `MiaId` | int | Y |  |  |  |  |
| 3 | `MiaName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `TargetQty` | int | Y |  |  |  |  |
| 5 | `Period` | nvarchar(50) | Y |  |  |  |  |
| 6 | `Year` | nvarchar(50) | Y |  |  |  |  |
| 7 | `CompanyId` | int | Y |  |  |  |  |
| 8 | `ProductId` | int | Y |  |  |  |  |
| 9 | `TargetAmount` | decimal(18,2) | Y |  | `((0))` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMIATargetProductWise` | PK | CLUSTERED | MiaTargetId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`

---

## `tblMIOInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `MIOId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `MIOId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `TerritoryId` | int | Y |  |  |  |  |
| 4 | `EmployeeId` | int | Y |  |  |  |  |
| 5 | `Vacant` | nvarchar(50) | Y |  |  |  |  |
| 6 | `IsActive` | bit | Y |  |  |  | yes |
| 7 | `ActiveDate` | datetime | Y |  |  |  |  |
| 8 | `InActiveDate` | datetime | Y |  |  |  |  |
| 9 | `InActiveBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `EntryDate` | datetime | Y |  |  |  |  |
| 12 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 13 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMIOInfo` | PK | CLUSTERED | MIOId |

**Logical (inferred, not enforced) foreign keys:**

- `TerritoryId` -> `tblTerritory.TerritoryId`
- `EmployeeId` -> `Employee.EmployeeId`

---

## `tblMoneyReceipt`

**Status:** COVERED (referenced by app source). **Primary key:** `MoneyReceiptId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `MoneyReceiptId` | int | N | Y |  | PK |  |
| 2 | `MoneyReceiptNo` | nvarchar(200) | Y |  |  |  |  |
| 3 | `MonyReceiptDate` | datetime | Y |  |  |  |  |
| 4 | `BatchId` | int | Y |  | `((0))` |  |  |
| 5 | `CustMasterId` | int | Y |  | `((0))` |  |  |
| 6 | `CustPayDetailsId` | nvarchar(500) | Y |  |  |  |  |
| 7 | `CreateBy` | nvarchar(200) | Y |  |  |  |  |
| 8 | `CreateDate` | datetime | Y |  | `(getdate())` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblMoneyReceipt` | PK | CLUSTERED | MoneyReceiptId |

**Logical (inferred, not enforced) foreign keys:**

- `BatchId` -> `tblInvoiceBatch.BatchId`

---

## `tblNonEffectiveReason`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ReasonId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ReasonId` | int | N | Y |  | PK |  |
| 2 | `ReasonName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `IsActive` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblNonEffectiveReason` | PK | CLUSTERED | ReasonId |

---

## `tblNonTranscationalInvoiceDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `InvReturnDetailID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvReturnDetailID` | int | N | Y |  | PK |  |
| 2 | `InvReturnMasterID` | int | Y |  |  |  |  |
| 3 | `ProductId` | int | Y |  |  |  |  |
| 4 | `Batch` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ExpDate` | datetime | Y |  |  |  |  |
| 6 | `MfgDate` | datetime | Y |  |  |  |  |
| 7 | `Qty` | decimal(18,0) | Y |  |  |  |  |
| 8 | `Price` | decimal(18,2) | Y |  |  |  |  |
| 9 | `VAT` | decimal(18,2) | Y |  |  |  |  |
| 10 | `TotalAmount` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblNonTranscationalInvoiceDetail` | PK | CLUSTERED | InvReturnDetailID |

**Logical (inferred, not enforced) foreign keys:**

- `InvReturnMasterID` -> `tblNonTranscationalInvoiceMaster.InvReturnMasterID`
- `ProductId` -> `tblProduct.ProductId`

---

## `tblNonTranscationalInvoiceMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `InvReturnMasterID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `InvReturnMasterID` | int | N | Y |  | PK |  |
| 2 | `ReturnCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `InvoiceId` | int | Y |  |  |  |  |
| 4 | `ReturnDate` | datetime | Y |  |  |  |  |
| 5 | `SalesDate` | datetime | Y |  |  |  |  |
| 6 | `CustomerMasterId` | int | Y |  |  |  |  |
| 7 | `ComUnitId` | int | Y |  |  |  |  |
| 8 | `TotalReturn` | decimal(18,2) | Y |  |  |  |  |
| 9 | `CreateBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `CreateDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |
| 13 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 14 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 15 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 16 | `TotalValue` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblNonTranscationalInvoiceMaster` | PK | CLUSTERED | InvReturnMasterID |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `ComUnitId` -> `tblCompanyUnit.ComUnitId`

---

## `tblNSMInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `NSMId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `NSMName` | nvarchar(MAX) | Y |  |  |  |  |
| 2 | `GroupId` | int | Y |  |  |  |  |
| 3 | `NSMId` | int | N | Y |  | PK |  |
| 4 | `IsActive` | bit | Y |  |  |  | yes |
| 5 | `CompanyId` | int | Y |  |  |  |  |
| 6 | `EmployeeId` | int | Y |  |  |  |  |
| 7 | `ActiveDate` | datetime | Y |  |  |  |  |
| 8 | `InActiveDate` | datetime | Y |  |  |  |  |
| 9 | `InActiveBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `EntryDate` | datetime | Y |  |  |  |  |
| 12 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 13 | `UpdateDate` | datetime | Y |  |  |  |  |
| 14 | `Vacant` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblNSMInfo` | PK | CLUSTERED | NSMId |

**Logical (inferred, not enforced) foreign keys:**

- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`
- `EmployeeId` -> `Employee.EmployeeId`

---

## `tblOrder`

**Status:** COVERED (referenced by app source). **Primary key:** `OrderId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `OrderId` | int | N | Y |  | PK |  |
| 2 | `OrderCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `ComUnitId` | int | Y |  |  |  |  |
| 4 | `ComUnitCode` | nvarchar(50) | Y |  |  |  |  |
| 5 | `ComUnitName` | nvarchar(500) | Y |  |  |  |  |
| 6 | `MIOCode` | nvarchar(50) | Y |  |  |  |  |
| 7 | `MIOName` | nvarchar(50) | Y |  |  |  |  |
| 8 | `ManufacId` | int | Y |  |  |  |  |
| 9 | `CustomerCode` | nvarchar(50) | Y |  |  |  |  |
| 10 | `CustomerName` | nvarchar(500) | Y |  |  |  |  |
| 11 | `GrossValue` | decimal(18,2) | Y |  |  |  |  |
| 12 | `SubmissionDate` | datetime | Y |  |  |  |  |
| 13 | `IsInvoice` | bit | Y |  |  |  | yes |
| 14 | `IsManual` | nvarchar(50) | Y |  |  |  | yes |
| 15 | `RegionId` | int | Y |  |  |  |  |
| 16 | `AreaId` | int | Y |  |  |  |  |
| 17 | `TerritoryId` | int | Y |  |  |  |  |
| 18 | `MarketId` | int | Y |  |  |  |  |
| 19 | `CustomerMasterId` | int | Y |  |  |  |  |
| 20 | `RSMId` | int | Y |  |  |  |  |
| 21 | `ASMId` | int | Y |  |  |  |  |
| 22 | `MIOId` | int | Y |  |  |  |  |
| 23 | `DeliveryPersonId` | int | Y |  |  |  |  |
| 24 | `IsSpecialApproval` | bit | Y |  |  |  | yes |
| 25 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 26 | `EntryDate` | datetime | Y |  |  |  |  |
| 27 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 28 | `UpdateDate` | datetime | Y |  |  |  |  |
| 29 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 30 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 31 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 32 | `IsDirect` | bit | Y |  |  |  | yes |
| 33 | `IsFromApp` | bit | Y |  | `((0))` |  | yes |
| 34 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |
| 35 | `ProcessTime` | datetime | Y |  | `(getdate())` |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `IX_tblOrder_EntryBy` | INDEX | NONCLUSTERED | OrderId,SubmissionDate,EntryBy |
| `IX_tblOrder_IsFromApp` | INDEX | NONCLUSTERED | OrderId,OrderCode,MIOName,CustomerCode,CustomerName,GrossValue,EntryDate,IsFromApp |
| `PK_tblOrder` | PK | CLUSTERED | OrderId |

**Logical (inferred, not enforced) foreign keys:**

- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `ManufacId` -> `tblManufacturer.ManufacId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `RSMId` -> `tblRSMInfo.RSMId`
- `ASMId` -> `tblASMInfo.ASMId`
- `MIOId` -> `tblMIOInfo.MIOId`

---

## `tblOrderDel`

**Status:** COVERED (referenced by app source). **Primary key:** `OrderDelId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `OrderDelId` | int | N | Y |  | PK |  |
| 2 | `OrderId` | int | Y |  |  |  |  |
| 3 | `OrderCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `ComUnitId` | int | Y |  |  |  |  |
| 5 | `ComUnitCode` | nvarchar(50) | Y |  |  |  |  |
| 6 | `ComUnitName` | nvarchar(50) | Y |  |  |  |  |
| 7 | `MIOCode` | nvarchar(50) | Y |  |  |  |  |
| 8 | `MIOName` | nvarchar(50) | Y |  |  |  |  |
| 9 | `ManufacId` | int | Y |  |  |  |  |
| 10 | `CustomerCode` | nvarchar(50) | Y |  |  |  |  |
| 11 | `CustomerName` | nvarchar(50) | Y |  |  |  |  |
| 12 | `GrossValue` | decimal(18,2) | Y |  |  |  |  |
| 13 | `SubmissionDate` | datetime | Y |  |  |  |  |
| 14 | `IsInvoice` | bit | Y |  |  |  | yes |
| 15 | `IsManual` | nvarchar(50) | Y |  |  |  | yes |
| 16 | `DeleteBy` | nvarchar(500) | Y |  |  |  |  |
| 17 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblOrderDel` | PK | CLUSTERED | OrderDelId |

**Logical (inferred, not enforced) foreign keys:**

- `OrderId` -> `tblOrder.OrderId`, `tblSampleIssue.OrderId`
- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `ManufacId` -> `tblManufacturer.ManufacId`

---

## `tblOrderDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `OrderDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `OrderDetailId` | int | N | Y |  | PK |  |
| 2 | `ProductId` | int | Y |  |  |  |  |
| 3 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `ProductName` | nvarchar(500) | Y |  |  |  |  |
| 5 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 6 | `TradePrice` | decimal(18,2) | Y |  |  |  |  |
| 7 | `TotalTradePrice` | decimal(18,2) | Y |  |  |  |  |
| 8 | `OrderId` | int | Y |  |  |  |  |
| 9 | `OrderListDetailId` | int | Y |  |  |  |  |
| 10 | `Status` | nvarchar(MAX) | Y |  |  |  | yes |
| 11 | `UnitVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 12 | `TotalVatAmount` | decimal(18,2) | Y |  |  |  |  |
| 13 | `NetAmount` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblOrderDetail` | PK | CLUSTERED | OrderDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`
- `OrderId` -> `tblOrder.OrderId`, `tblSampleIssue.OrderId`

---

## `tblOrderDetailDel`

**Status:** COVERED (referenced by app source). **Primary key:** `OrderDetailDelId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `OrderDetailDelId` | int | N | Y |  | PK |  |
| 2 | `OrderDetailId` | int | N |  |  |  |  |
| 3 | `ProductId` | int | Y |  |  |  |  |
| 4 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 5 | `ProductName` | nvarchar(50) | Y |  |  |  |  |
| 6 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 7 | `TradePrice` | decimal(18,2) | Y |  |  |  |  |
| 8 | `TotalTradePrice` | decimal(18,2) | Y |  |  |  |  |
| 9 | `OrderId` | int | Y |  |  |  |  |
| 10 | `OrderListDetailId` | int | Y |  |  |  |  |
| 11 | `Status` | nvarchar(MAX) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblOrderDetailDel` | PK | CLUSTERED | OrderDetailDelId |

**Logical (inferred, not enforced) foreign keys:**

- `OrderDetailId` -> `tblOrderDetail.OrderDetailId`, `tblSampleIssueDetail.OrderDetailId`
- `ProductId` -> `tblProduct.ProductId`
- `OrderId` -> `tblOrder.OrderId`, `tblSampleIssue.OrderId`

---

## `tblPackSize`

**Status:** COVERED (referenced by app source). **Primary key:** `PackSizeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PackSizeId` | int | N | Y |  | PK |  |
| 2 | `PackSizeName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblPackSize` | PK | CLUSTERED | PackSizeId |

---

## `tblPaymentReverseDELInvoice`

**Status:** UNUSED — no app-code reference found. **Primary key:** `PaymentReverseDELInvoiceID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PaymentReverseDELInvoiceID` | int | N |  |  | PK |  |
| 2 | `InvoiceId` | int | Y |  |  |  |  |
| 3 | `InvoiceNo` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `InvoiceDate` | datetime | Y |  |  |  |  |
| 5 | `OrderId` | int | Y |  |  |  |  |
| 6 | `OrderNo` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `OrderDate` | datetime | Y |  |  |  |  |
| 8 | `CustomerMasterId` | int | Y |  |  |  |  |
| 9 | `ComUnitId` | int | Y |  |  |  |  |
| 10 | `MiaId` | int | Y |  |  |  |  |
| 11 | `PaymentTypeId` | int | Y |  |  |  |  |
| 12 | `TpTotal` | decimal(18,2) | Y |  |  |  |  |
| 13 | `TpDiscount` | decimal(18,2) | Y |  |  |  |  |
| 14 | `TpVat` | decimal(18,2) | Y |  |  |  |  |
| 15 | `TpGrandTotal` | decimal(18,2) | Y |  |  |  |  |
| 16 | `UserId` | int | Y |  |  |  |  |
| 17 | `DeliveryTpTotal` | decimal(18,2) | Y |  |  |  |  |
| 18 | `DeliveryTpDiscount` | decimal(18,2) | Y |  |  |  |  |
| 19 | `DeliveryTpVat` | decimal(18,2) | Y |  |  |  |  |
| 20 | `DeliveryTpGrandTotal` | decimal(18,2) | Y |  |  |  |  |
| 21 | `DeliveryInvoiceStatus` | nvarchar(50) | Y |  |  |  | yes |
| 22 | `DelivaryInvoiceNo` | nvarchar(MAX) | Y |  |  |  |  |
| 23 | `CreateBy` | nvarchar(50) | Y |  |  |  |  |
| 24 | `CreateDate` | datetime | Y |  |  |  |  |
| 25 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 26 | `UpdateDate` | datetime | Y |  |  |  |  |
| 27 | `TotalSpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 28 | `DelivarySpecialAmount` | decimal(18,2) | Y |  |  |  |  |
| 29 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 30 | `PaymentStatus` | nvarchar(50) | Y |  |  |  | yes |
| 31 | `ProductOffer` | nvarchar(50) | Y |  |  |  |  |
| 32 | `OldTradePolicy` | nvarchar(50) | Y |  |  |  |  |
| 33 | `Remarks` | nvarchar(50) | Y |  |  |  |  |
| 34 | `DeliveryManId` | int | Y |  |  |  |  |
| 35 | `BatchId` | int | Y |  |  |  |  |
| 36 | `IsDirect` | bit | Y |  |  |  | yes |
| 37 | `IsPosting` | bit | Y |  |  |  | yes |
| 38 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 39 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblPaymentReverseDELInvoice` | PK | CLUSTERED | PaymentReverseDELInvoiceID |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`
- `OrderId` -> `tblOrder.OrderId`, `tblSampleIssue.OrderId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `PaymentTypeId` -> `tblPaymentType.PaymentTypeId`
- `UserId` -> `tblUser.UserId`
- `DeliveryManId` -> `tblDeliveryManInfo.DeliveryManId`
- `BatchId` -> `tblInvoiceBatch.BatchId`

---

## `tblPaymentTranscationDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `CustPayDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustPayDetailId` | int | N | Y |  | PK |  |
| 2 | `InvoiceId` | int | Y |  |  |  |  |
| 3 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 4 | `CustPayId` | int | Y |  |  |  |  |
| 5 | `IsPrint` | bit | Y |  |  |  | yes |
| 6 | `Discount` | decimal(18,2) | Y |  |  |  |  |
| 7 | `CashAccId` | int | Y |  |  |  |  |
| 8 | `IsPosting` | bit | Y |  |  |  | yes |
| 9 | `BankAccId` | int | Y |  |  |  |  |
| 10 | `IsDetailProcess` | bit | Y |  |  |  | yes |
| 11 | `PaymentRemarks` | nvarchar(MAX) | Y |  |  |  |  |
| 12 | `AIT` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblPaymentTranscationDetail` | PK | CLUSTERED | CustPayDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `InvoiceId` -> `tblInvoice.InvoiceId`
- `CustPayId` -> `tblCustomerPay.CustPayId`, `tblCustomerPayDeleteLog.CustPayId`, `tblPaymentTranscationMaster.CustPayId`

---

## `tblPaymentTranscationMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `CustPayId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustPayId` | int | N | Y |  | PK |  |
| 2 | `CustomerMasterId` | int | Y |  |  |  |  |
| 3 | `PaymentDate` | datetime | Y |  |  |  |  |
| 4 | `PaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `PayType` | nvarchar(50) | Y |  |  |  |  |
| 6 | `RefNo` | nvarchar(50) | Y |  |  |  |  |
| 7 | `RefDate` | datetime | Y |  |  |  |  |
| 8 | `CreateBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `CreateDate` | datetime | Y |  |  |  |  |
| 10 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `UpdateDate` | datetime | Y |  |  |  |  |
| 12 | `IsProcess` | bit | Y |  |  |  | yes |
| 13 | `InvPaymentAmount` | decimal(18,2) | Y |  |  |  |  |
| 14 | `PaymentStatus` | nvarchar(50) | Y |  |  |  | yes |
| 15 | `IsReject` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblPaymentTranscationMaster` | PK | CLUSTERED | CustPayId |

**Logical (inferred, not enforced) foreign keys:**

- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`

---

## `tblPaymentType`

**Status:** COVERED (referenced by app source). **Primary key:** `PaymentTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PaymentTypeId` | int | N |  |  | PK |  |
| 2 | `PaymentTypeName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblPaymentType` | PK | CLUSTERED | PaymentTypeId |

---

## `tblPettyCashDetails`

**Status:** COVERED (referenced by app source). **Primary key:** `PettyCashDetailsId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PettyCashDetailsId` | int | N | Y |  | PK |  |
| 2 | `PettyCashmasterId` | int | Y |  |  |  |  |
| 3 | `PerticulerId` | int | Y |  |  |  |  |
| 4 | `CashReason` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `Amount` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK__tblPetty__A90888A0293FD2DF` | PK | CLUSTERED | PettyCashDetailsId |

**Physical foreign keys:**

- `PettyCashmasterId` -> `tblPettyCashMaster.PettyCashmasterId` (`FK__tblPettyC__Petty__1FB8AE52`, ON DELETE NO_ACTION, ON UPDATE NO_ACTION)

**Logical (inferred, not enforced) foreign keys:**

- `PettyCashmasterId` -> `tblPettyCashMaster.PettyCashmasterId`

---

## `tblPettyCashMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `PettyCashmasterId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `PettyCashmasterId` | int | N | Y |  | PK |  |
| 2 | `ConpanyId` | int | Y |  |  |  |  |
| 3 | `PostingDate` | datetime | Y |  |  |  |  |
| 4 | `CashAccId` | int | Y |  |  |  |  |
| 5 | `Remark` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryDate` | datetime | Y |  |  |  |  |
| 8 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `UpdateDate` | datetime | Y |  |  |  |  |
| 10 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 11 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 12 | `ActiveStatus` | nvarchar(50) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK__tblPetty__B3D39E3E7F0F437D` | PK | CLUSTERED | PettyCashmasterId |

---

## `tblProCategory`

**Status:** COVERED (referenced by app source). **Primary key:** `CategoryId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CategoryId` | int | N | Y |  | PK |  |
| 2 | `CategoryCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `CategoryName` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProCategory` | PK | CLUSTERED | CategoryId |

---

## `tblProduct`

**Status:** COVERED (referenced by app source). **Primary key:** `ProductId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProductId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `ProductName` | nvarchar(500) | Y |  |  |  |  |
| 5 | `Description` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `PackSize` | nvarchar(50) | Y |  |  |  |  |
| 7 | `ProductBrandId` | int | Y |  |  |  |  |
| 8 | `PackSizeId` | int | Y |  |  |  |  |
| 9 | `ProTypeId` | int | Y |  |  |  |  |
| 10 | `CategoryId` | nvarchar(50) | Y |  |  |  |  |
| 11 | `ManufacId` | int | Y |  |  |  |  |
| 12 | `StockUOMId` | int | Y |  |  |  |  |
| 13 | `CaseId` | int | Y |  |  |  |  |
| 14 | `IsActiveProduct` | bit | Y |  |  |  | yes |
| 15 | `ActiveInactiveBy` | int | Y |  |  |  |  |
| 16 | `ActiveInactiveDate` | datetime | Y |  |  |  |  |
| 17 | `ProductGroupId` | int | Y |  |  |  |  |
| 18 | `IsTrade` | bit | Y |  | `((0))` |  | yes |
| 19 | `IsRecipe` | bit | Y |  | `((0))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProduct` | PK | CLUSTERED | ProductId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductBrandId` -> `tblProductSQ.ProductBrandId`
- `PackSizeId` -> `tblPackSize.PackSizeId`
- `ProTypeId` -> `tblProType.ProTypeId`
- `CategoryId` -> `tblCustCategory.CategoryId`, `tblDoctorCategory.CategoryId`, `tblProCategory.CategoryId`
- `ManufacId` -> `tblManufacturer.ManufacId`
- `StockUOMId` -> `tblStockUOM.StockUOMId`
- `CaseId` -> `tblProductCase.CaseId`

---

## `tblProductCase`

**Status:** COVERED (referenced by app source). **Primary key:** `CaseId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CaseId` | int | N | Y |  | PK |  |
| 2 | `CaseQty` | decimal(18,0) | Y |  |  |  |  |
| 3 | `PcsPerCase` | decimal(18,0) | Y |  |  |  |  |
| 4 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProductCase` | PK | CLUSTERED | CaseId |

---

## `tblProductDiscount`

**Status:** COVERED (referenced by app source). **Primary key:** `DiscountId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `DiscountId` | int | N |  |  | PK |  |
| 2 | `ProductId` | int | Y |  |  |  |  |
| 3 | `CustomerMasterId` | int | Y |  |  |  |  |
| 4 | `DiscountPercentage` | decimal(18,2) | Y |  |  |  |  |
| 5 | `Status` | nvarchar(50) | Y |  |  |  | yes |
| 6 | `ActiveDate` | datetime | Y |  |  |  |  |
| 7 | `InactiveDate` | datetime | Y |  |  |  |  |
| 8 | `IsActive` | bit | Y |  |  |  | yes |
| 9 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |
| 13 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 14 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 15 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProductDiscount` | PK | CLUSTERED | DiscountId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`

---

## `tblProductGroup`

**Status:** UNUSED — no app-code reference found. **Primary key:** `GroupId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `GroupId` | int | N | Y |  | PK |  |
| 2 | `GroupName` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProductGroup` | PK | CLUSTERED | GroupId |

---

## `tblProductQuotedPrice`

**Status:** COVERED (referenced by app source). **Primary key:** `QuotedPriceId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `QuotedPriceId` | int | N |  |  | PK |  |
| 2 | `ProductId` | int | Y |  |  |  |  |
| 3 | `CustomerMasterId` | int | Y |  |  |  |  |
| 4 | `QuotedPrice` | decimal(18,2) | Y |  |  |  |  |
| 5 | `Status` | nvarchar(50) | Y |  |  |  | yes |
| 6 | `ActiveDate` | datetime | Y |  |  |  |  |
| 7 | `InactiveDate` | datetime | Y |  |  |  |  |
| 8 | `IsActive` | bit | Y |  |  |  | yes |
| 9 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |
| 13 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 14 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 15 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProductQuotedPrice` | PK | CLUSTERED | QuotedPriceId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`

---

## `tblProductSQ`

**Status:** COVERED (referenced by app source). **Primary key:** `ProductBrandId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProductBrandId` | int | N | Y |  | PK |  |
| 2 | `IngridentsId` | int | Y |  |  |  |  |
| 3 | `ProductSQName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `MaxValue` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProductSQ` | PK | CLUSTERED | ProductBrandId |

**Logical (inferred, not enforced) foreign keys:**

- `IngridentsId` -> `tblIngridents.IngridentsId`

---

## `tblProductUpload`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProductId` | int | Y |  |  |  |  |
| 2 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `Description` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `Company` | nvarchar(50) | Y |  |  |  |  |
| 6 | `PackSize` | nvarchar(50) | Y |  |  |  |  |
| 7 | `ProductBrand` | nvarchar(50) | Y |  |  |  |  |
| 8 | `ProType` | nvarchar(50) | Y |  |  |  |  |
| 9 | `Category` | nvarchar(50) | Y |  |  |  |  |
| 10 | `Manufacturer` | nvarchar(50) | Y |  |  |  |  |
| 11 | `StockUOM` | nvarchar(50) | Y |  |  |  |  |
| 12 | `ProductCase` | nvarchar(50) | Y |  |  |  |  |
| 13 | `UnitPrice` | decimal(18,2) | Y |  |  |  |  |
| 14 | `MRPPrice` | decimal(18,2) | Y |  |  |  |  |
| 15 | `CompanyId` | nchar(10) | Y |  |  |  |  |
| 16 | `PackSizeId` | nchar(10) | Y |  |  |  |  |
| 17 | `BrandId` | nchar(10) | Y |  |  |  |  |
| 18 | `TypeId` | nchar(10) | Y |  |  |  |  |
| 19 | `CategoryId` | nchar(10) | Y |  |  |  |  |
| 20 | `ManufacturerId` | nchar(10) | Y |  |  |  |  |
| 21 | `UOMId` | nchar(10) | Y |  |  |  |  |
| 22 | `CaseId` | nchar(10) | Y |  |  |  |  |
| 23 | `CostPrice` | decimal(18,2) | Y |  |  |  |  |
| 24 | `StockQuantity` | decimal(18,2) | Y |  |  |  |  |
| 25 | `PurchaseRate` | decimal(18,2) | Y |  |  |  |  |
| 26 | `TotalValue` | decimal(18,2) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`
- `PackSizeId` -> `tblPackSize.PackSizeId`
- `CategoryId` -> `tblCustCategory.CategoryId`, `tblDoctorCategory.CategoryId`, `tblProCategory.CategoryId`
- `CaseId` -> `tblProductCase.CaseId`

---

## `tblProgramType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ProgramTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProgramTypeId` | int | N | Y |  | PK |  |
| 2 | `ProgramTypeName` | nvarchar(50) | Y |  |  |  |  |
| 3 | `PrgmTypeCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `EntryBy` | int | Y |  |  |  |  |
| 5 | `EntryDate` | datetime | Y |  |  |  |  |
| 6 | `UpdateBy` | int | Y |  |  |  |  |
| 7 | `UpdateDate` | datetime | Y |  |  |  |  |
| 8 | `ApproveBy` | int | Y |  |  |  |  |
| 9 | `ApproveDate` | datetime | Y |  |  |  | yes |
| 10 | `IsActive` | bit | Y |  |  |  | yes |
| 11 | `InactiveBy` | int | Y |  |  |  |  |
| 12 | `InactiveDate` | datetime | Y |  |  |  |  |
| 13 | `IsCustomer` | bit | Y |  |  |  | yes |
| 14 | `IsDoctor` | bit | Y |  |  |  | yes |
| 15 | `IsDefault` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_ProgramTypeId` | PK | CLUSTERED | ProgramTypeId |

---

## `tblProInvoiceReturnTrack`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ID` | int | N | Y |  | PK |  |
| 2 | `ReturnTotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 3 | `DCStoreId` | int | Y |  |  |  |  |
| 4 | `ReturnDate` | datetime | Y |  |  |  |  |
| 5 | `ReturnExecutionDateTime` | datetime | Y |  |  |  |  |
| 6 | `InvoiceDetailId` | int | Y |  |  |  |  |
| 7 | `InvoiceId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProInvoiceReturnTrack` | PK | CLUSTERED | ID |

**Logical (inferred, not enforced) foreign keys:**

- `DCStoreId` -> `tblDCStore.DCStoreId`
- `InvoiceDetailId` -> `tblInvoiceDetail.InvoiceDetailId`
- `InvoiceId` -> `tblInvoice.InvoiceId`

---

## `tblProType`

**Status:** COVERED (referenced by app source). **Primary key:** `ProTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ProTypeId` | int | N |  |  | PK |  |
| 2 | `ProTypeName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblProType` | PK | CLUSTERED | ProTypeId |

---

## `tblRegion`

**Status:** COVERED (referenced by app source). **Primary key:** `RegionId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `RegionId` | int | N | Y |  | PK |  |
| 2 | `RegionName` | nvarchar(500) | Y |  |  |  |  |
| 3 | `RegionCode` | nvarchar(500) | Y |  |  |  |  |
| 4 | `RgnShortName` | nvarchar(50) | Y |  |  |  |  |
| 5 | `Description` | nvarchar(500) | Y |  |  |  |  |
| 6 | `Remarks` | nvarchar(500) | Y |  |  |  |  |
| 7 | `IsActive` | bit | Y |  |  |  | yes |
| 8 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `EntryDate` | datetime | Y |  |  |  |  |
| 10 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `UpdateDate` | datetime | Y |  |  |  |  |
| 12 | `GroupId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblRegion` | PK | CLUSTERED | RegionId |

**Logical (inferred, not enforced) foreign keys:**

- `GroupId` -> `tbl_Group.GroupId`, `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`

---

## `tblRequisition`

**Status:** COVERED (referenced by app source). **Primary key:** `ReqId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ReqId` | int | N |  |  | PK |  |
| 2 | `ReqNo` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `ReqDate` | datetime | Y |  |  |  |  |
| 4 | `CompanyId` | int | Y |  |  |  |  |
| 5 | `WarehouseId` | int | Y |  |  |  |  |
| 6 | `WearhouseName` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `ComUnitId` | int | Y |  |  |  |  |
| 8 | `ComUnitCode` | nvarchar(50) | Y |  |  |  |  |
| 9 | `ComUnitName` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `Submit` | nvarchar(50) | Y |  |  |  |  |
| 11 | `SubmitDate` | datetime | Y |  |  |  |  |
| 12 | `IssueChalanNo` | nvarchar(MAX) | Y |  |  |  | yes |
| 13 | `IssuChalanDate` | datetime | Y |  |  |  | yes |
| 14 | `TruckNo` | nvarchar(MAX) | Y |  |  |  |  |
| 15 | `DriverName` | nvarchar(MAX) | Y |  |  |  |  |
| 16 | `TotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 17 | `TotalVAT` | decimal(18,2) | Y |  |  |  |  |
| 18 | `GrandTotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 19 | `ReceiveIssue` | nvarchar(50) | Y |  |  |  |  |
| 20 | `ReceiveIssueDate` | datetime | Y |  |  |  |  |
| 21 | `CreatePicking` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `PickingNo` | nvarchar(MAX) | Y |  |  |  |  |
| 23 | `PickingDate` | datetime | Y |  |  |  |  |
| 24 | `EntryBy` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `EntryDate` | datetime | Y |  |  |  |  |
| 26 | `UpdateBy` | nvarchar(MAX) | Y |  |  |  |  |
| 27 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblRequisition` | PK | CLUSTERED | ReqId |

**Logical (inferred, not enforced) foreign keys:**

- `ComUnitId` -> `tblCompanyUnit.ComUnitId`

---

## `tblRequsitionChild`

**Status:** COVERED (referenced by app source). **Primary key:** `ReqChildId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ReqChildId` | int | N |  |  | PK |  |
| 2 | `ProductCode` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `PackSize` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ReqQty` | decimal(18,0) | Y |  |  |  |  |
| 6 | `ReqId` | int | Y |  |  |  |  |
| 7 | `IssueQty` | decimal(18,0) | Y |  |  |  | yes |
| 8 | `UnitPrice` | decimal(18,2) | Y |  |  |  |  |
| 9 | `PriceAmount` | decimal(18,2) | Y |  |  |  |  |
| 10 | `VATAmount` | decimal(18,2) | Y |  |  |  |  |
| 11 | `TotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 12 | `IsIssue` | nvarchar(MAX) | Y |  |  |  | yes |
| 13 | `CaseQty` | decimal(18,0) | Y |  |  |  |  |
| 14 | `MusakVATAmount` | decimal(18,2) | Y |  |  |  |  |
| 15 | `MusakTotalPrice` | decimal(18,2) | Y |  |  |  |  |
| 16 | `IsPicking` | nvarchar(MAX) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblRequsitionChild` | PK | CLUSTERED | ReqChildId |

**Logical (inferred, not enforced) foreign keys:**

- `ReqId` -> `tblRequisition.ReqId`

---

## `tblReturnReason`

**Status:** COVERED (referenced by app source). **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ReturndReasonId` | int | N |  |  |  |  |
| 2 | `ReaturnReason` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tblRoleType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `RoleTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `RoleTypeId` | int | N | Y |  | PK |  |
| 2 | `RoleType` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblRoleType` | PK | CLUSTERED | RoleTypeId |

---

## `tblRSMInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `RSMId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `RSMId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `RegionId` | int | Y |  |  |  |  |
| 4 | `EmployeeId` | int | Y |  |  |  |  |
| 5 | `IsActive` | bit | Y |  |  |  | yes |
| 6 | `ActiveDate` | datetime | Y |  |  |  |  |
| 7 | `InActiveDate` | datetime | Y |  |  |  |  |
| 8 | `InActiveBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |
| 13 | `Vacant` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblRSMInfo` | PK | CLUSTERED | RSMId |

**Logical (inferred, not enforced) foreign keys:**

- `RegionId` -> `tblRegion.RegionId`
- `EmployeeId` -> `Employee.EmployeeId`

---

## `tblSampleIssue`

**Status:** COVERED (referenced by app source). **Primary key:** `OrderId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `OrderId` | int | N | Y |  | PK |  |
| 2 | `OrderCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `ComUnitId` | int | Y |  |  |  |  |
| 4 | `ComUnitCode` | nvarchar(50) | Y |  |  |  |  |
| 5 | `ComUnitName` | nvarchar(500) | Y |  |  |  |  |
| 6 | `MIOCode` | nvarchar(50) | Y |  |  |  |  |
| 7 | `MIOName` | nvarchar(50) | Y |  |  |  |  |
| 8 | `ManufacId` | int | Y |  |  |  |  |
| 9 | `CustomerCode` | nvarchar(50) | Y |  |  |  |  |
| 10 | `CustomerName` | nvarchar(500) | Y |  |  |  |  |
| 11 | `GrossValue` | decimal(18,2) | Y |  |  |  |  |
| 12 | `SubmissionDate` | datetime | Y |  |  |  |  |
| 13 | `IsInvoice` | bit | Y |  |  |  | yes |
| 14 | `IsManual` | nvarchar(50) | Y |  |  |  | yes |
| 15 | `RegionId` | int | Y |  |  |  |  |
| 16 | `AreaId` | int | Y |  |  |  |  |
| 17 | `TerritoryId` | int | Y |  |  |  |  |
| 18 | `MarketId` | int | Y |  |  |  |  |
| 19 | `CustomerMasterId` | int | Y |  |  |  |  |
| 20 | `RSMId` | int | Y |  |  |  |  |
| 21 | `ASMId` | int | Y |  |  |  |  |
| 22 | `MIOId` | int | Y |  |  |  |  |
| 23 | `DeliveryPersonId` | int | Y |  |  |  |  |
| 24 | `IsSpecialApproval` | bit | Y |  |  |  | yes |
| 25 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 26 | `EntryDate` | datetime | Y |  |  |  |  |
| 27 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 28 | `UpdateDate` | datetime | Y |  |  |  |  |
| 29 | `ApprovedBy` | nvarchar(50) | Y |  |  |  | yes |
| 30 | `ApprovedDate` | datetime | Y |  |  |  | yes |
| 31 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 32 | `IsDirect` | bit | Y |  |  |  | yes |
| 33 | `IsPosting` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblSampleIssue` | PK | CLUSTERED | OrderId |

**Logical (inferred, not enforced) foreign keys:**

- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `ManufacId` -> `tblManufacturer.ManufacId`
- `RegionId` -> `tblRegion.RegionId`
- `AreaId` -> `tblArea.AreaId`
- `TerritoryId` -> `tblTerritory.TerritoryId`
- `MarketId` -> `tblMarket.MarketId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `RSMId` -> `tblRSMInfo.RSMId`
- `ASMId` -> `tblASMInfo.ASMId`
- `MIOId` -> `tblMIOInfo.MIOId`

---

## `tblSampleIssueDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `OrderDetailId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `OrderDetailId` | int | N | Y |  | PK |  |
| 2 | `ProductId` | int | Y |  |  |  |  |
| 3 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `ProductName` | nvarchar(500) | Y |  |  |  |  |
| 5 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 6 | `TradePrice` | decimal(18,2) | Y |  |  |  |  |
| 7 | `TotalTradePrice` | decimal(18,2) | Y |  |  |  |  |
| 8 | `OrderId` | int | Y |  |  |  |  |
| 9 | `OrderListDetailId` | int | Y |  |  |  |  |
| 10 | `Status` | nvarchar(MAX) | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblSampleIssueDetail` | PK | CLUSTERED | OrderDetailId |

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`
- `OrderId` -> `tblOrder.OrderId`, `tblSampleIssue.OrderId`

---

## `tblSampleIssueTranscation`

**Status:** COVERED (referenced by app source). **Primary key:** `CrditAdjustmentId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CrditAdjustmentId` | int | N | Y |  | PK |  |
| 2 | `ComUnitId` | int | Y |  |  |  |  |
| 3 | `CustomerMasterId` | int | Y |  |  |  |  |
| 4 | `IssueId` | int | Y |  |  |  | yes |
| 5 | `IssueDetailId` | int | Y |  |  |  | yes |
| 6 | `DCStoreId` | int | Y |  |  |  |  |
| 7 | `Quantity` | decimal(18,2) | Y |  |  |  |  |
| 8 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 9 | `EntryDate` | datetime | Y |  |  |  |  |
| 10 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblSampleIssueTranscation` | PK | CLUSTERED | CrditAdjustmentId |

**Logical (inferred, not enforced) foreign keys:**

- `ComUnitId` -> `tblCompanyUnit.ComUnitId`
- `CustomerMasterId` -> `tblCustMaster.CustomerMasterId`
- `DCStoreId` -> `tblDCStore.DCStoreId`

---

## `tblShippingCartonSize`

**Status:** UNUSED — no app-code reference found. **Primary key:** `ShippingCartonSizeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `ShippingCartonSizeId` | int | N |  |  | PK |  |
| 2 | `ShippingCartonSizeName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblShippingCartonSize` | PK | CLUSTERED | ShippingCartonSizeId |

---

## `tblSlaveInfo`

**Status:** COVERED (referenced by app source). **Primary key:** `SlaveId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SlaveId` | int | N | Y |  | PK |  |
| 2 | `CompanyId` | int | Y |  |  |  |  |
| 3 | `MaxAmmount` | decimal(18,2) | Y |  |  |  |  |
| 4 | `MinAmmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `DisParcentage` | int | Y |  |  |  |  |
| 6 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 7 | `IsActive` | bit | Y |  |  |  | yes |
| 8 | `ActiveDate` | datetime | Y |  |  |  |  |
| 9 | `InActiveDate` | datetime | Y |  |  |  |  |
| 10 | `InActiveBy` | nvarchar(50) | Y |  |  |  |  |
| 11 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `EntryDate` | datetime | Y |  |  |  |  |
| 13 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 14 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblSlaveInfo` | PK | CLUSTERED | SlaveId |

---

## `tblSMCType`

**Status:** UNUSED — no app-code reference found. **Primary key:** `SMCTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SMCTypeId` | int | N | Y |  | PK |  |
| 2 | `SMCType` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `forCustomer` | bit | Y |  |  |  | yes |
| 4 | `forDotor` | bit | Y |  |  |  | yes |
| 5 | `IsActive` | bit | Y |  |  |  | yes |
| 6 | `EntryBy` | int | Y |  |  |  |  |
| 7 | `EntryDate` | datetime | Y |  |  |  |  |
| 8 | `UpdateBy` | int | Y |  |  |  |  |
| 9 | `UpdateDate` | datetime | Y |  |  |  |  |
| 10 | `ApproveBy` | int | Y |  |  |  |  |
| 11 | `ApproveDate` | datetime | Y |  |  |  | yes |
| 12 | `IsDefault` | bit | Y |  |  |  | yes |
| 13 | `InactiveBy` | int | Y |  |  |  |  |
| 14 | `InactiveDate` | datetime | Y |  |  |  |  |
| 15 | `SMCTypeCode` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblSMCType` | PK | CLUSTERED | SMCTypeId |

---

## `tblStockCondition`

**Status:** COVERED (referenced by app source). **Primary key:** `StockConId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `StockConId` | int | N |  |  | PK |  |
| 2 | `StockCondition` | nvarchar(50) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblStockCondition` | PK | CLUSTERED | StockConId |

---

## `tblStockConditionFreeze`

**Status:** COVERED (referenced by app source). **Primary key:** `StockConditionFreezeID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `StockConditionFreezeID` | int | N |  |  | PK |  |
| 2 | `ReceiveId` | int | Y |  |  |  |  |
| 3 | `DCStoreId` | int | Y |  |  |  |  |
| 4 | `ManufacId` | int | Y |  |  |  |  |
| 5 | `FreezeQty` | decimal(18,0) | Y |  |  |  |  |
| 6 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblStockConditionFreeze` | PK | CLUSTERED | StockConditionFreezeID |

**Logical (inferred, not enforced) foreign keys:**

- `ReceiveId` -> `tblCentralStore.ReceiveId`
- `DCStoreId` -> `tblDCStore.DCStoreId`
- `ManufacId` -> `tblManufacturer.ManufacId`

---

## `tblStockConditionPermission`

**Status:** COVERED (referenced by app source). **Primary key:** `StockCondintionID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `StockCondintionID` | int | N | Y |  | PK |  |
| 2 | `UserId` | int | Y |  |  |  |  |
| 3 | `CompanyUnitId` | int | Y |  |  |  |  |
| 4 | `StockConId` | int | Y |  |  |  |  |
| 5 | `Permission` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblStockConditionPermission` | PK | CLUSTERED | StockCondintionID |

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`
- `StockConId` -> `tblStockCondition.StockConId`

---

## `tblStockInTransfar`

**Status:** COVERED (referenced by app source). **Primary key:** `StockInTransfarId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `StockInTransfarId` | int | N |  |  | PK |  |
| 2 | `ReqId` | int | Y |  |  |  |  |
| 3 | `ReqChildId` | int | Y |  |  |  |  |
| 4 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 5 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `PackSize` | nvarchar(50) | Y |  |  |  |  |
| 7 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `Quantity` | decimal(18,0) | Y |  |  |  |  |
| 9 | `PickingQty` | decimal(18,0) | Y |  |  |  |  |
| 10 | `UnitPrice` | decimal(18,2) | Y |  |  |  |  |
| 11 | `PriceAmount` | decimal(18,2) | Y |  |  |  |  |
| 12 | `VATAmount` | decimal(18,2) | Y |  |  |  |  |
| 13 | `TotalPriceAmount` | decimal(18,2) | Y |  |  |  |  |
| 14 | `ExpDate` | datetime | Y |  |  |  |  |
| 15 | `ReceiveDate` | datetime | Y |  |  |  |  |
| 16 | `IsTransfared` | nvarchar(50) | Y |  |  |  | yes |
| 17 | `IsIssue` | nvarchar(50) | Y |  |  |  | yes |
| 18 | `ReceiveId` | int | Y |  |  |  |  |
| 19 | `MfgDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblStockInTransfar` | PK | CLUSTERED | StockInTransfarId |

**Logical (inferred, not enforced) foreign keys:**

- `ReqId` -> `tblRequisition.ReqId`
- `ReqChildId` -> `tblRequsitionChild.ReqChildId`
- `ReceiveId` -> `tblCentralStore.ReceiveId`

---

## `tblStockUOM`

**Status:** COVERED (referenced by app source). **Primary key:** `StockUOMId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `StockUOMId` | int | N | Y |  | PK |  |
| 2 | `StockUOMName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblStockUOM` | PK | CLUSTERED | StockUOMId |

---

## `tblSupplierInformation`

**Status:** COVERED (referenced by app source). **Primary key:** `SupplierId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SupplierId` | int | N | Y |  | PK |  |
| 2 | `SupplierCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `SupplierName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `SupplierAddress` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ContactNo` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 7 | `EntryDate` | datetime | Y |  |  |  |  |
| 8 | `Updateby` | nvarchar(50) | Y |  |  |  |  |
| 9 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblSupplierInformation` | PK | CLUSTERED | SupplierId |

---

## `tblSynchronizationInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `SynchronizationInfoId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `SynchronizationInfoId` | int | N | Y |  | PK |  |
| 2 | `EmpInfoId` | int | Y |  |  |  |  |
| 3 | `SynchronizationDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblSynchronizationInfo` | PK | CLUSTERED | SynchronizationInfoId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tblTagASMRSMInfo`

**Status:** UNUSED — no app-code reference found. **Primary key:** `TagASMRSMId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TagASMRSMId` | int | N | Y |  | PK |  |
| 2 | `UserRoleId` | int | Y |  |  |  |  |
| 3 | `EmpInfoId` | int | Y |  |  |  |  |
| 4 | `TerritoryAreaId` | int | Y |  |  |  |  |
| 5 | `UserId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblTagASMRSMInfo` | PK | CLUSTERED | TagASMRSMId |

**Logical (inferred, not enforced) foreign keys:**

- `UserRoleId` -> `tbl_UserRoleInfo.UserRoleId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `UserId` -> `tblUser.UserId`

---

## `tblTempCustomer`

**Status:** UNUSED — no app-code reference found. **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `CustomerId` | int | Y |  |  |  |  |
| 2 | `CustomerCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `CustomerName` | nvarchar(500) | Y |  |  |  |  |
| 4 | `MarketId` | int | Y |  |  |  |  |
| 5 | `MarketCode` | nvarchar(50) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `MarketId` -> `tblMarket.MarketId`

---

## `tblTerritory`

**Status:** COVERED (referenced by app source). **Primary key:** `TerritoryId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TerritoryId` | int | N | Y |  | PK |  |
| 2 | `AreaId` | int | Y |  |  |  |  |
| 3 | `TerritoryName` | nvarchar(500) | Y |  |  |  |  |
| 4 | `TerritoryCode` | nvarchar(500) | Y |  |  |  |  |
| 5 | `TerShortName` | nvarchar(50) | Y |  |  |  |  |
| 6 | `Description` | nvarchar(500) | Y |  |  |  |  |
| 7 | `Remarks` | nvarchar(500) | Y |  |  |  |  |
| 8 | `IsActive` | bit | Y |  |  |  | yes |
| 9 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 10 | `EntryDate` | datetime | Y |  |  |  |  |
| 11 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 12 | `UpdateDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblTerritory` | PK | CLUSTERED | TerritoryId |

**Logical (inferred, not enforced) foreign keys:**

- `AreaId` -> `tblArea.AreaId`

---

## `tblTradeMaterialTransfer`

**Status:** COVERED (referenced by app source). **Primary key:** `TradeMaterialTransferId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TradeMaterialTransferId` | int | N | Y |  | PK |  |
| 2 | `TransferCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `FromCompanyId` | int | Y |  |  |  |  |
| 4 | `FromLocationId` | int | Y |  |  |  |  |
| 5 | `FromProductId` | int | Y |  |  |  |  |
| 6 | `FromProductCode` | nvarchar(100) | Y |  |  |  |  |
| 7 | `FromProductName` | nvarchar(300) | Y |  |  |  |  |
| 8 | `FromCurrentStockQty` | decimal(18,3) | Y |  |  |  |  |
| 9 | `ToCompanyId` | int | Y |  |  |  |  |
| 10 | `ToLocationId` | int | Y |  |  |  |  |
| 11 | `ToItemDescriptionId` | int | Y |  |  |  |  |
| 12 | `ToItemDescriptionCode` | nvarchar(100) | Y |  |  |  |  |
| 13 | `ToItemDescription` | nvarchar(300) | Y |  |  |  |  |
| 14 | `ToCurrentStockQty` | decimal(18,3) | Y |  |  |  |  |
| 15 | `TransferQty` | decimal(18,3) | Y |  |  |  |  |
| 16 | `Remarks` | nvarchar(500) | Y |  |  |  |  |
| 17 | `EntryBy` | nvarchar(100) | Y |  |  |  |  |
| 18 | `EntryDate` | datetime | Y |  |  |  |  |
| 19 | `UpdateBy` | nvarchar(100) | Y |  |  |  |  |
| 20 | `UpdateDate` | datetime | Y |  |  |  |  |
| 21 | `ApprovalStatus` | nvarchar(50) | Y |  |  |  | yes |
| 22 | `ApprovedBy` | nvarchar(100) | Y |  |  |  | yes |
| 23 | `ApprovedDate` | datetime | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK__tblTrade__06E200F1DBD6F5F0` | PK | CLUSTERED | TradeMaterialTransferId |

---

## `tblTradePolicyNew`

**Status:** COVERED (referenced by app source). **Primary key:** `TradePolicyId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `TradePolicyId` | int | N |  |  | PK |  |
| 2 | `ManufacId` | int | Y |  |  |  |  |
| 3 | `MinAmount` | decimal(18,2) | Y |  |  |  |  |
| 4 | `MaxAmount` | decimal(18,2) | Y |  |  |  |  |
| 5 | `DiscountPerc` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblNewTradePolicy` | PK | CLUSTERED | TradePolicyId |

**Logical (inferred, not enforced) foreign keys:**

- `ManufacId` -> `tblManufacturer.ManufacId`

---

## `tblUnitPrice`

**Status:** COVERED (referenced by app source). **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UnitPriceId` | int | Y |  |  |  |  |
| 2 | `ProductId` | int | N |  |  |  |  |
| 3 | `ProductCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `ProductName` | nvarchar(500) | Y |  |  |  |  |
| 5 | `PackSize` | nvarchar(500) | Y |  |  |  |  |
| 6 | `CostPrice` | decimal(18,2) | Y |  |  |  |  |
| 7 | `UnitPrice` | decimal(18,2) | Y |  |  |  |  |
| 8 | `MRPPrice` | decimal(18,2) | Y |  |  |  |  |
| 9 | `VATPercentage` | decimal(18,2) | Y |  |  |  |  |
| 10 | `VATAmountPerUnit` | decimal(18,2) | Y |  |  |  |  |
| 11 | `MusakVATPercentage` | decimal(18,2) | Y |  |  |  |  |
| 12 | `MusakVATAmountPerUnit` | decimal(18,2) | Y |  |  |  |  |
| 13 | `TPVat` | decimal(18,2) | Y |  |  |  |  |
| 14 | `MusakVat` | decimal(18,2) | Y |  |  |  |  |
| 15 | `IsActive` | bit | Y |  |  |  | yes |
| 16 | `ActiveDate` | datetime | Y |  |  |  |  |
| 17 | `InActiveDate` | datetime | Y |  |  |  |  |
| 18 | `ActionStatus` | nvarchar(50) | Y |  |  |  | yes |
| 19 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 20 | `EntryDate` | datetime | Y |  |  |  |  |
| 21 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 22 | `UpdateDate` | datetime | Y |  |  |  |  |
| 23 | `ApproveBy` | nvarchar(50) | Y |  |  |  |  |
| 24 | `ApproveDate` | datetime | Y |  |  |  | yes |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

**Logical (inferred, not enforced) foreign keys:**

- `ProductId` -> `tblProduct.ProductId`

---

## `tblUser`

**Status:** COVERED (referenced by app source). **Primary key:** `UserId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UserId` | int | N | Y |  | PK |  |
| 2 | `UserName` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `UserType` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `UserCode` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `LoginName` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `Password` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `UserStatus` | nvarchar(50) | Y |  |  |  | yes |
| 8 | `Email` | nvarchar(MAX) | Y |  |  |  |  |
| 9 | `ContactNo` | nvarchar(50) | Y |  |  |  |  |
| 10 | `CentralWareHouse` | bit | Y |  |  |  | yes |
| 11 | `IsExpProduct` | bit | Y |  |  |  | yes |
| 12 | `IsHead` | bit | Y |  |  |  | yes |
| 13 | `IsDashboard` | bit | Y |  |  |  | yes |
| 14 | `IsCreditApproval` | bit | Y |  |  |  | yes |
| 15 | `EmpInfoId` | int | Y |  |  |  |  |
| 16 | `IsAppsUser` | bit | Y |  |  |  | yes |
| 17 | `IMEI_One` | nvarchar(MAX) | Y |  |  |  |  |
| 18 | `IMEI_Two` | nvarchar(MAX) | Y |  |  |  |  |
| 19 | `DeviceInfo_1` | nvarchar(MAX) | Y |  |  |  |  |
| 20 | `DeviceInfo_2` | nvarchar(MAX) | Y |  |  |  |  |
| 21 | `OS_1` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `OS_2` | nvarchar(MAX) | Y |  |  |  |  |
| 23 | `OS_Version_1` | nvarchar(MAX) | Y |  |  |  |  |
| 24 | `OS_Version_2` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `LastAccessTime_1` | datetime | Y |  |  |  |  |
| 26 | `LastAccessTime_2` | datetime | Y |  |  |  |  |
| 27 | `AppVer_1` | nvarchar(MAX) | Y |  |  |  |  |
| 28 | `AppVer_2` | nvarchar(MAX) | Y |  |  |  |  |
| 29 | `UserRoleID` | int | Y |  |  |  |  |
| 30 | `IsMainDashboard` | bit | Y |  |  |  | yes |
| 31 | `IsDepotDashboard` | bit | Y |  |  |  | yes |
| 32 | `ActiveInActiveDate` | datetime | Y |  |  |  |  |
| 33 | `UserTypeId` | int | Y |  |  |  |  |
| 34 | `UpdateBy` | int | Y |  |  |  |  |
| 35 | `UpdateDate` | datetime | Y |  |  |  |  |
| 36 | `EntryBy` | int | Y |  |  |  |  |
| 37 | `EntryDate` | datetime | Y |  |  |  |  |
| 38 | `IsRXUser` | bit | Y |  |  |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblUser` | PK | CLUSTERED | UserId |

**Logical (inferred, not enforced) foreign keys:**

- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `UserRoleID` -> `tbl_UserRoleInfo.UserRoleID`
- `UserTypeId` -> `tblUserType.UserTypeId`

---

## `tblUserCompanyUnit`

**Status:** COVERED (referenced by app source). **Primary key:** `UserComId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UserId` | int | Y |  |  |  |  |
| 2 | `CompanyUnitId` | int | Y |  |  |  |  |
| 3 | `CWHPermission` | bit | Y |  |  |  | yes |
| 4 | `NationalReportPermission` | bit | Y |  |  |  | yes |
| 5 | `UserComId` | int | N | Y |  | PK |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblUserCompanyUnit` | PK | CLUSTERED | UserComId |

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`

---

## `tblUserDeviceToken`

**Status:** UNUSED — no app-code reference found. **Primary key:** `UserDeviceTokenId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UserDeviceTokenId` | int | N | Y |  | PK |  |
| 2 | `UserId` | int | Y |  |  |  |  |
| 3 | `EmpInfoId` | int | Y |  |  |  |  |
| 4 | `DeviceToken` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `EntryDate` | datetime | Y |  |  |  |  |
| 6 | `Device` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblUserDeviceToken` | PK | CLUSTERED | UserDeviceTokenId |

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`

---

## `tblUserLog`

**Status:** COVERED (referenced by app source). **Primary key:** `UserLogId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UserLogId` | int | N | Y |  | PK |  |
| 2 | `UserId` | int | N |  |  |  |  |
| 3 | `UserName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `UserType` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `UserCode` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `LoginName` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `Password` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `UserStatus` | nvarchar(50) | Y |  |  |  | yes |
| 9 | `Email` | nvarchar(MAX) | Y |  |  |  |  |
| 10 | `ContactNo` | nvarchar(50) | Y |  |  |  |  |
| 11 | `CentralWareHouse` | bit | Y |  |  |  | yes |
| 12 | `IsExpProduct` | bit | Y |  |  |  | yes |
| 13 | `IsHead` | bit | Y |  |  |  | yes |
| 14 | `IsDashboard` | bit | Y |  |  |  | yes |
| 15 | `IsCreditApproval` | bit | Y |  |  |  | yes |
| 16 | `EmpInfoId` | int | Y |  |  |  |  |
| 17 | `IsAppsUser` | bit | Y |  |  |  | yes |
| 18 | `IMEI_One` | nvarchar(MAX) | Y |  |  |  |  |
| 19 | `IMEI_Two` | nvarchar(MAX) | Y |  |  |  |  |
| 20 | `DeviceInfo_1` | nvarchar(MAX) | Y |  |  |  |  |
| 21 | `DeviceInfo_2` | nvarchar(MAX) | Y |  |  |  |  |
| 22 | `OS_1` | nvarchar(MAX) | Y |  |  |  |  |
| 23 | `OS_2` | nvarchar(MAX) | Y |  |  |  |  |
| 24 | `OS_Version_1` | nvarchar(MAX) | Y |  |  |  |  |
| 25 | `OS_Version_2` | nvarchar(MAX) | Y |  |  |  |  |
| 26 | `LastAccessTime_1` | datetime | Y |  |  |  |  |
| 27 | `LastAccessTime_2` | datetime | Y |  |  |  |  |
| 28 | `AppVer_1` | nvarchar(MAX) | Y |  |  |  |  |
| 29 | `AppVer_2` | nvarchar(MAX) | Y |  |  |  |  |
| 30 | `UserRoleID` | int | Y |  |  |  |  |
| 31 | `IsMainDashboard` | bit | Y |  |  |  | yes |
| 32 | `IsDepotDashboard` | bit | Y |  |  |  | yes |
| 33 | `ActiveInActiveDate` | datetime | Y |  |  |  |  |
| 34 | `UserTypeId` | int | Y |  |  |  |  |
| 35 | `UpdateBy` | int | Y |  |  |  |  |
| 36 | `UpdateDate` | datetime | Y |  |  |  |  |
| 37 | `EntryBy` | int | Y |  |  |  |  |
| 38 | `EntryDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblUserLog_1` | PK | CLUSTERED | UserLogId |

**Logical (inferred, not enforced) foreign keys:**

- `UserId` -> `tblUser.UserId`
- `EmpInfoId` -> `tblEmpGeneralInfo.EmpInfoId`
- `UserRoleID` -> `tbl_UserRoleInfo.UserRoleID`
- `UserTypeId` -> `tblUserType.UserTypeId`

---

## `tblUserType`

**Status:** COVERED (referenced by app source). **Primary key:** `UserTypeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `UserTypeId` | int | N |  |  | PK |  |
| 2 | `UserType` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblUserType` | PK | CLUSTERED | UserTypeId |

---

## `tblWearhouse`

**Status:** COVERED (referenced by app source). **Primary key:** NONE FOUND. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WearhouseId` | int | N |  |  |  |  |
| 2 | `WearhouseCode` | nvarchar(MAX) | Y |  |  |  |  |
| 3 | `WearhouseName` | nvarchar(MAX) | Y |  |  |  |  |
| 4 | `Address` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `PhoneNo` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `MobileNo` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `FaxNo` | nvarchar(MAX) | Y |  |  |  |  |
| 8 | `CompanyId` | int | Y |  |  |  |  |
| 9 | `CompanyName` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:** none found (no PK, no unique/non-unique index catalog rows).

---

## `tblWhStockConditionFreeze`

**Status:** COVERED (referenced by app source). **Primary key:** `WhStockConditionFreezeID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WhStockConditionFreezeID` | int | N |  |  | PK |  |
| 2 | `ReceiveId` | int | Y |  |  |  |  |
| 3 | `WarehouseId` | int | Y |  |  |  |  |
| 4 | `FreezeQty` | decimal(18,0) | Y |  |  |  |  |
| 5 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 6 | `EntryDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWhStockConditionFreeze` | PK | CLUSTERED | WhStockConditionFreezeID |

**Logical (inferred, not enforced) foreign keys:**

- `ReceiveId` -> `tblCentralStore.ReceiveId`

---

## `tblWHStockInDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `WHStockInDetailID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WHStockInDetailID` | int | N | Y |  | PK |  |
| 2 | `WHStockInMasterID` | int | Y |  |  |  |  |
| 3 | `ProductId` | int | Y |  |  |  |  |
| 4 | `Batch` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `ExpDate` | datetime | Y |  |  |  |  |
| 6 | `MfgDate` | datetime | Y |  |  |  |  |
| 7 | `Qty` | decimal(18,0) | Y |  |  |  |  |
| 8 | `Price` | decimal(18,2) | Y |  |  |  |  |
| 9 | `VAT` | decimal(18,2) | Y |  |  |  |  |
| 10 | `TotalAmount` | decimal(18,2) | Y |  |  |  |  |
| 11 | `MaterialConversionMasterId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWHStockInDetail` | PK | CLUSTERED | WHStockInDetailID |

**Logical (inferred, not enforced) foreign keys:**

- `WHStockInMasterID` -> `tblWHStockInMaster.WHStockInMasterID`
- `ProductId` -> `tblProduct.ProductId`

---

## `tblWHStockInDetailDeleteLog`

**Status:** UNUSED — no app-code reference found. **Primary key:** `WHStockInDetailDeleteLogID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WHStockInDetailDeleteLogID` | int | N | Y |  | PK |  |
| 2 | `WHStockInDetailID` | int | Y |  |  |  |  |
| 3 | `WHStockInMasterID` | int | Y |  |  |  |  |
| 4 | `ProductId` | int | Y |  |  |  |  |
| 5 | `Batch` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `ExpDate` | datetime | Y |  |  |  |  |
| 7 | `MfgDate` | datetime | Y |  |  |  |  |
| 8 | `Qty` | decimal(18,0) | Y |  |  |  |  |
| 9 | `Price` | decimal(18,2) | Y |  |  |  |  |
| 10 | `VAT` | decimal(18,2) | Y |  |  |  |  |
| 11 | `TotalAmount` | decimal(18,2) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWHStockInDetailDeleteLog` | PK | CLUSTERED | WHStockInDetailDeleteLogID |

**Logical (inferred, not enforced) foreign keys:**

- `WHStockInDetailID` -> `tblWHStockInDetail.WHStockInDetailID`
- `WHStockInMasterID` -> `tblWHStockInMaster.WHStockInMasterID`
- `ProductId` -> `tblProduct.ProductId`

---

## `tblWHStockInMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `WHStockInMasterID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WHStockInMasterID` | int | N |  |  | PK |  |
| 2 | `WHStockInCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `WarehouseId` | int | Y |  |  |  |  |
| 4 | `WHStockInDate` | datetime | Y |  |  |  |  |
| 5 | `TotalQuantity` | int | Y |  |  |  |  |
| 6 | `TotalVat` | decimal(18,2) | Y |  |  |  |  |
| 7 | `TotalValue` | decimal(18,2) | Y |  |  |  |  |
| 8 | `ChallanNo` | nvarchar(50) | Y |  |  |  |  |
| 9 | `ChallanDate` | datetime | Y |  |  |  |  |
| 10 | `ReferenceNo` | nchar(50) | Y |  |  |  |  |
| 11 | `ReferenceDate` | nvarchar(500) | Y |  |  |  |  |
| 12 | `Remarks` | nvarchar(500) | Y |  |  |  |  |
| 13 | `Status` | nvarchar(50) | Y |  |  |  | yes |
| 14 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 15 | `EntryDate` | datetime | Y |  |  |  |  |
| 16 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 17 | `UpdateDate` | datetime | Y |  |  |  |  |
| 18 | `ApproveBy` | nvarchar(50) | Y |  |  |  |  |
| 19 | `ApproveDate` | datetime | Y |  |  |  | yes |
| 20 | `SupplierId` | int | Y |  |  |  |  |
| 21 | `AccountsApproval` | bit | Y |  |  |  | yes |
| 22 | `IsPosting` | bit | Y |  | `((0))` |  | yes |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWHStockInMaster` | PK | CLUSTERED | WHStockInMasterID |

**Logical (inferred, not enforced) foreign keys:**

- `SupplierId` -> `tblSupplierInformation.SupplierId`

---

## `tblWHStockInMasterDeleteLog`

**Status:** UNUSED — no app-code reference found. **Primary key:** `WHStockInMasterDeleteLogID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WHStockInMasterDeleteLogID` | int | N | Y |  | PK |  |
| 2 | `WHStockInMasterID` | int | N |  |  |  |  |
| 3 | `WHStockInCode` | nvarchar(50) | Y |  |  |  |  |
| 4 | `WarehouseId` | int | Y |  |  |  |  |
| 5 | `WHStockInDate` | datetime | Y |  |  |  |  |
| 6 | `TotalQuantity` | int | Y |  |  |  |  |
| 7 | `TotalVat` | decimal(18,2) | Y |  |  |  |  |
| 8 | `TotalValue` | decimal(18,2) | Y |  |  |  |  |
| 9 | `ChallanNo` | nvarchar(50) | Y |  |  |  |  |
| 10 | `ChallanDate` | datetime | Y |  |  |  |  |
| 11 | `ReferenceNo` | nchar(50) | Y |  |  |  |  |
| 12 | `ReferenceDate` | nvarchar(500) | Y |  |  |  |  |
| 13 | `Remarks` | nvarchar(500) | Y |  |  |  |  |
| 14 | `Status` | nvarchar(50) | Y |  |  |  | yes |
| 15 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 16 | `EntryDate` | datetime | Y |  |  |  |  |
| 17 | `UpdateBy` | nvarchar(50) | Y |  |  |  |  |
| 18 | `UpdateDate` | datetime | Y |  |  |  |  |
| 19 | `ApproveBy` | nvarchar(50) | Y |  |  |  |  |
| 20 | `ApproveDate` | datetime | Y |  |  |  | yes |
| 21 | `SupplierId` | int | Y |  |  |  |  |
| 22 | `AccountsApproval` | bit | Y |  |  |  | yes |
| 23 | `DeleteBy` | nvarchar(50) | Y |  |  |  |  |
| 24 | `DeleteDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWHStockInMasterDeleteLog` | PK | CLUSTERED | WHStockInMasterDeleteLogID |

**Logical (inferred, not enforced) foreign keys:**

- `WHStockInMasterID` -> `tblWHStockInMaster.WHStockInMasterID`
- `SupplierId` -> `tblSupplierInformation.SupplierId`

---

## `tblWHStockOutDetail`

**Status:** COVERED (referenced by app source). **Primary key:** `WHStockOutDetailID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WHStockOutDetailID` | int | N | Y |  | PK |  |
| 2 | `WHStockOutMasterID` | int | Y |  |  |  |  |
| 3 | `ProductId` | int | Y |  |  |  |  |
| 4 | `Qty` | decimal(18,0) | Y |  |  |  |  |
| 5 | `WHStockInDetailID` | int | Y |  |  |  |  |
| 6 | `ReceiveId` | int | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWHStockOutDetail` | PK | CLUSTERED | WHStockOutDetailID |

**Logical (inferred, not enforced) foreign keys:**

- `WHStockOutMasterID` -> `tblWHStockOutMaster.WHStockOutMasterID`
- `ProductId` -> `tblProduct.ProductId`
- `WHStockInDetailID` -> `tblWHStockInDetail.WHStockInDetailID`
- `ReceiveId` -> `tblCentralStore.ReceiveId`

---

## `tblWHStockOutMaster`

**Status:** COVERED (referenced by app source). **Primary key:** `WHStockOutMasterID`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WHStockOutMasterID` | int | N |  |  | PK |  |
| 2 | `WHStockOutCode` | nvarchar(50) | Y |  |  |  |  |
| 3 | `WearhouseId` | int | Y |  |  |  |  |
| 4 | `WHStockOutDate` | datetime | Y |  |  |  |  |
| 5 | `WHStockInMasterID` | int | Y |  |  |  |  |
| 6 | `Reason` | nvarchar(500) | Y |  |  |  |  |
| 7 | `EntryBy` | nvarchar(50) | Y |  |  |  |  |
| 8 | `EntryDate` | datetime | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWHStockOutMaster` | PK | CLUSTERED | WHStockOutMasterID |

**Logical (inferred, not enforced) foreign keys:**

- `WHStockInMasterID` -> `tblWHStockInMaster.WHStockInMasterID`

---

## `tblWhStoreFreeze`

**Status:** COVERED (referenced by app source). **Primary key:** `WhStoreFreezeId`. **Computed columns:** 0.

| # | Column | Type | Nullable | Identity | Default | PK | Business/status field |
|---|---|---|---|---|---|---|---|
| 1 | `WhStoreFreezeId` | int | N | Y |  | PK |  |
| 2 | `ReceiveId` | int | Y |  |  |  |  |
| 3 | `ProductId` | int | Y |  |  |  |  |
| 4 | `ProductName` | nvarchar(MAX) | Y |  |  |  |  |
| 5 | `PackSize` | nvarchar(MAX) | Y |  |  |  |  |
| 6 | `BatchNo` | nvarchar(MAX) | Y |  |  |  |  |
| 7 | `TotalQuantity` | decimal(18,0) | Y |  |  |  |  |
| 8 | `ExpDate` | datetime | Y |  |  |  |  |
| 9 | `ReceiveDate` | datetime | Y |  |  |  |  |
| 10 | `StockQty` | decimal(18,0) | Y |  |  |  |  |
| 11 | `DamageQty` | decimal(18,0) | Y |  |  |  |  |
| 12 | `StockRcvDate` | datetime | Y |  |  |  |  |
| 13 | `StockCondition` | nvarchar(50) | Y |  |  |  |  |
| 14 | `WhStockConditionFreezeID` | int | Y |  |  |  |  |
| 15 | `Remarks` | nvarchar(MAX) | Y |  |  |  |  |

**Indexes:**

| Index | Type | Clustering | Columns |
|---|---|---|---|
| `PK_tblWhStoreFreeze` | PK | CLUSTERED | WhStoreFreezeId |

**Logical (inferred, not enforced) foreign keys:**

- `ReceiveId` -> `tblCentralStore.ReceiveId`
- `ProductId` -> `tblProduct.ProductId`
- `WhStockConditionFreezeID` -> `tblWhStockConditionFreeze.WhStockConditionFreezeID`

---
