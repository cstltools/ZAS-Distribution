# Views

Ground truth: extracted live from `SalesrollDB_ZAS` (`sys.sql_modules` / `OBJECT_DEFINITION`). This database has
**exactly 24 views**, no more, no fewer. Application usage below was determined by grepping `Library.DAL`,
`Library.BLL`, and `Solution.Web` for each view name (raw SQL strings — this app calls views directly in ad-hoc
SQL, not through stored procedures).

The previous version of this document assumed no DB access existed and listed only 6 views (INFERRED from call
sites, bodies UNKNOWN). That assumption is now obsolete — all 24 view bodies are below, verbatim.

---

## View_CentralStoreCurrentStock

```sql
CREATE VIEW [dbo].[View_CentralStoreCurrentStock]
AS
SELECT        P.ProductCode, P.ProductName, P.PackSize, ISNULL(Vtbl.TotalCurrentStockQty, 0) AS TotalCurrentStockQty, ISNULL(Vtbl.StockCondition, N'Available') AS StockCondition, Vtbl.WarehouseId
FROM            dbo.tblProduct AS P INNER JOIN
                             (SELECT        ProductCode, StockCondition, SUM(Quantity) AS TotalCurrentStockQty, WarehouseId
                               FROM            dbo.tblCentralStore
                               WHERE        (Quantity > 0)
                               GROUP BY ProductCode, StockCondition, WarehouseId) AS Vtbl ON P.ProductCode = Vtbl.ProductCode
```

- **Source tables:** `tblProduct`, `tblCentralStore`.
- **Joins:** `tblProduct` INNER JOIN an aggregated subquery over `tblCentralStore` (grouped by `ProductCode`,
  `StockCondition`, `WarehouseId`), on `ProductCode`.
- **Filter:** the subquery only includes `tblCentralStore` rows with `Quantity > 0`.
- **Derived columns:** `TotalCurrentStockQty` = `SUM(Quantity)` per product/condition/warehouse, defaulted to 0 via
  `ISNULL`; `StockCondition` defaulted to `'Available'` via `ISNULL`.
- **Application usage:** `RequisitionDAL.GetPickingDataForIssueDAL` (`Library.DAL\SInventory_DAL\RequisitionDAL.cs:186`)
  — live usage, joined into the picking-for-issue query filtered to `StockCondition = 'Available'`.
  Note: `RequisitionDAL.GetRequisitionDetailByReqIdDAL` only references this view inside a **commented-out** SQL
  string (line 176) — that call site is dead code, not a live caller.

---

## View_CustomerMaster

```sql
CREATE VIEW [dbo].[View_CustomerMaster]
AS
SELECT        dbo.tblCustMaster.CustomerMasterId, dbo.tblCustMaster.CustomerCode, dbo.tblCustCategory.CategoryCode, dbo.tblCustCategory.CategoryName, dbo.tblCustMaster.CustomerName, dbo.tblCustMaster.Address, 
                         dbo.tblCustMaster.CellNo, dbo.tblCustMaster.MarketId, dbo.tblCustMaster.Addrees2, dbo.tblCustMaster.City, dbo.tblCustMaster.ConPerson, dbo.tblCustMaster.ShippingCond, dbo.tblCustMaster.TermOfPayment, 
                         dbo.tblCustMaster.CustomerCodeOld, dbo.tblCustMaster.UploadDate, dbo.tblCustMaster.ExcelUpload, dbo.tblCustMaster.FixedCustomer, dbo.tblCustMaster.CreateBy, dbo.tblCustMaster.CreateDate, 
                         dbo.tblCustMaster.UpdateBy, dbo.tblCustMaster.UpdateDate, dbo.tblMarket.MarketCode, dbo.tblMarket.MarketName, dbo.tblMarket.MktShortName, dbo.tblTerritory.TerritoryId, dbo.tblTerritory.TerritoryName, 
                         dbo.tblTerritory.TerritoryCode, dbo.tblTerritory.TerShortName, dbo.tblArea.AreaId, dbo.tblArea.AreaCode, dbo.tblArea.AreaName, dbo.tblArea.AreaShortName, dbo.tblRegion.RegionId, dbo.tblRegion.RegionName, 
                         dbo.tblRegion.RegionCode, dbo.tblRegion.RgnShortName
FROM            dbo.tblCustMaster LEFT OUTER JOIN
                         dbo.tblMarket ON dbo.tblMarket.MarketId = dbo.tblCustMaster.MarketId LEFT OUTER JOIN
                         dbo.tblTerritory ON dbo.tblTerritory.TerritoryId = dbo.tblMarket.TerritoryId LEFT OUTER JOIN
                         dbo.tblArea ON dbo.tblArea.AreaId = dbo.tblTerritory.AreaId LEFT OUTER JOIN
                         dbo.tblRegion ON dbo.tblRegion.RegionId = dbo.tblArea.RegionId LEFT OUTER JOIN
                         dbo.tblCustCategory ON dbo.tblCustCategory.CategoryId = dbo.tblCustMaster.CategoryId
```

- **Source tables:** `tblCustMaster`, `tblMarket`, `tblTerritory`, `tblArea`, `tblRegion`, `tblCustCategory`.
- **Joins:** a LEFT OUTER JOIN chain walking the geography hierarchy —
  `tblCustMaster.MarketId = tblMarket.MarketId` → `tblMarket.TerritoryId = tblTerritory.TerritoryId` →
  `tblTerritory.AreaId = tblArea.AreaId` → `tblArea.RegionId = tblRegion.RegionId` — plus a separate LEFT JOIN
  `tblCustMaster.CategoryId = tblCustCategory.CategoryId`.
  This confirms the old doc's inference: it is exactly a denormalized customer master flattening the
  market→territory→area→region hierarchy and category onto each customer row.
- **Filter:** none (no WHERE clause).
- **Derived columns:** none — straight column passthrough, no computed/CASE/aggregate expressions.
- **Application usage:** heavily used — referenced in 21 DAL files under `Library.DAL\SInventory_DAL\`:
  `InvoiceDAL` (`IntransitReportDAl` x2, `InvoiceDetailDataForReport`, `MarketPickinReport` x2, `ProformaReportDAl` x2),
  `CustPaymentDAL` (`LoadInvoice` x2, `LoadCustomerMaster`, `LoadMarket`),
  `OrderInfoDAL` (`GetCustomerDueHistory`, `LoadInvoice`, `LoadOrder`, `LoadOrderForOrderCreation`, `LoadArea`,
  `LoadMarket`, `LoadMarketByInvoice`, `LoadMarketbyArea`),
  `CustomerMasterInfoDal.LoadCustomerMasterView`,
  `CustomerDueReportDAL.LoadCustomerDueReport`,
  `MioWiseCustomerDal.GetMioWiseCustomer`,
  `ProductDiscountDAL` (`LoadArea`, `LoadCustomerMaster`, `LoadMarket`),
  `WingsWiseSalesReportDal.LoadSalesReportInfo`,
  `CustomerPaymentReportDal.LoadSalesReportInfo`,
  `CompanySalesReportDAL` (`CustomerPaymentDAl` x2, `DeliveryReturnReportDAl` x2, `SalesReectionReportDAl` x2,
  `SalesReportDAl` x2, `SalesReportDAlParameter`),
  `CustomerWiseLedgerDal.GetCustomerWiseLedger`,
  `FieldForceSummeryDal.LoadSalesReportInfo`,
  `AuditReportOneDal` (`GetDeleteOrderNationalReport`, `GetDeleteOrderReport`),
  `SalesDashboardDal.GetDueUpto`,
  `StockConditionFreezeDAL.LoadInvoice`,
  `CustomerMasterDAL` (`CustomerMasterReport`, `FixedCustomerMasterReport`, `RegularCustomerMasterReport`,
  `LoadAreaNameById`, `LoadCategoryNameById`, `LoadCompanyUnitById`, `LoadDistrictNameById`, `LoadMarketNameById`,
  `LoadMiaNameById`, `LoadRegionnameById`),
  `CustomerLedgerDAL.CustomerLedgerDal`,
  `CustomerCoverageReportDal.GetMiaWiseCustomerCoverageInfo`,
  `MIOWiseProformaReportDAL.LoadCustomerPaymentInfo`,
  `DCStockInfoReprotDal.LoadSalesReportInfo`,
  `MiaSalesReportDAL` (`MiaWiseReportMainDataDAL`, `LoadMiaByComUnit`).
  By far the most reused view in the schema — matches the old doc's inference that it exists to avoid
  re-deriving the customer/geography join in every report/screen query.

---

## View_DCR

```sql
CREATE VIEW [dbo].[View_DCR]
AS
 select  distinct ISNULL(mas.CompanyId,0) CompanyId, mas.ApprovalStatus,   mas.DoctorId, CONVERT(date,mas.DcrDate)  DcrDate,  doc.DoctorCode [Doctor Code],      doc.DoctorName [Doctor Name],STUFF( (SELECT CONCAT(',', mm.DegreeName , '') FROM tblDoctorDegree mm   WITH (NOLOCK)  INNER JOIN dbo.tblDoctorDegreeDetail mgd   WITH (NOLOCK)  ON mgd.DegId=mm.DegreeId WHERE mgd.DoctorId=mas.DoctorId ORDER BY mgd.DoctorDegId FOR XML PATH ('') ),1,1,'') AS [Degree Name],ISNULL(STUFF( (SELECT CONCAT(',', mm.SpecialityName , '') FROM tblDoctorSpeciality mm   WITH (NOLOCK)  INNER JOIN dbo.tblDoctorSpecialityDetail mgd   WITH (NOLOCK)  ON mgd.SpecialityId=mm.SpecialityId WHERE mgd.DoctorId=mas.DoctorId ORDER BY mgd.DoctorSpId FOR XML PATH ('') ),1,1,''),'') as [Doctor Speciality], ISNULL(pt.ProgramTypeName, 'N/A') [Provider Type] , case when mas.SMCType_DCR is null then 'N/A' else  mas.SMCType_DCR end [Pharma Platform] ,  case when  dt.DoctorTypeName is null then 'N/A' else   dt.DoctorTypeName end  [Doctor Type] ,mas.GroupName  [Group], mas.RegionCode_DCR  [Region],mas.AreaCode_DCR  [Area],mas.TerritoryCode_DCR  [Territory],mas.SubTerritoryCode_DCR +' : '+ mas.SubTerritoryName [Sub-Territory],mas.MarketCode_DCR  [Market] from tbl_DCRInfo mas   WITH (NOLOCK) 
 inner join tblDoctorMaster doc  WITH (NOLOCK)    on mas.DoctorId=doc.DoctorId
 left join tblProgramType pt  WITH (NOLOCK)    on mas.DoctorProgramypeId=pt.ProgramTypeId
 left join tblDoctorType dt  WITH (NOLOCK)    on doc.DoctorTypeId=dt.DoctorTypeId
```

- **Source tables:** `tbl_DCRInfo` (Daily Call Report info, aliased `mas`), `tblDoctorMaster`, `tblProgramType`,
  `tblDoctorType`, plus correlated subqueries against `tblDoctorDegree`/`tblDoctorDegreeDetail` and
  `tblDoctorSpeciality`/`tblDoctorSpecialityDetail`.
- **Joins:** INNER JOIN `tbl_DCRInfo` to `tblDoctorMaster` on `DoctorId`; LEFT JOIN to `tblProgramType` on
  `DoctorProgramypeId`/`ProgramTypeId`; LEFT JOIN to `tblDoctorType` on `DoctorTypeId`. Region/Area/Territory/Market
  names come pre-denormalized as columns already on `tbl_DCRInfo` (`RegionCode_DCR`, `AreaCode_DCR`, etc.) rather
  than via further joins.
  Also two correlated subqueries per row, each using `STUFF(... FOR XML PATH(''))` to concatenate all of a
  doctor's degrees / specialities into one comma-separated string.
- **Filter:** none beyond the joins; `DISTINCT` on the whole row.
- **Derived columns:** `[Degree Name]` and `[Doctor Speciality]` (string-concatenated via `STUFF`/`FOR XML PATH`),
  `[Provider Type]` (`ISNULL` fallback to `'N/A'`), `[Pharma Platform]` and `[Doctor Type]` (`CASE WHEN ... IS NULL
  THEN 'N/A'`), `[Sub-Territory]` (string concatenation of code + name).
- **Application usage:** `UNUSED — no application caller found` (no match for `View_DCR` anywhere in
  `Library.DAL`, `Library.BLL`, or `Solution.Web`). Likely consumed only by Crystal Reports/SSRS or an external
  reporting tool, or by the ZAS field-force mobile/web API outside this repository.

---

## View_DCStoreCurrentStock

```sql
CREATE VIEW dbo.View_DCStoreCurrentStock
AS
SELECT PD.ProductCode, PD.ProductName, PD.PackSize, D.ComUnitId, SUM(D.StockQty) AS TotalQty
FROM     dbo.tblDCStore AS D LEFT OUTER JOIN
                  dbo.tblProduct AS PD ON D.ProductCode = PD.ProductCode
WHERE  (D.StockQty > 0)
GROUP BY PD.ProductCode, PD.ProductName, PD.PackSize, D.ComUnitId
```

- **Source tables:** `tblDCStore`, `tblProduct`.
- **Joins:** LEFT OUTER JOIN `tblDCStore` to `tblProduct` on `ProductCode`.
- **Filter:** `StockQty > 0`.
- **Derived columns:** `TotalQty` = `SUM(StockQty)` grouped by product and distribution-centre (`ComUnitId`).
- **Application usage:** `InvoiceDAL.ProductInfoDAL` (`Library.DAL\SInventory_DAL\InvoiceDAL.cs:708` and `:723` —
  both overloads: `ProductInfoDAL(comUnitId, productCode)` and `ProductInfoDAL(comUnitId, productCode,
  customerMasterId)`), used to look up current DC-level stock quantity when building the invoice-creation
  product-info row.

---

## View_DoctorMaster

```sql
CREATE VIEW [dbo].[View_DoctorMaster]
 
AS
SELECT DISTINCT CM.ProgramTypeId, CM.DoctorTypeId, CM.SMCTypeId, smct.SMCType, CM.DoctorId ,
                CM.DoctorCode ,
                (CM.DoctorCode + ' : '+CM.DoctorName)AS DoctorName, 
				 
								(
				SELECT TOP 1 Contact FROM dbo.tblDoctorContactDetail WHERE  DoctorId = CM.DoctorId
				) AS DocContact ,dt.DoctorTypeName, pt.ProgramTypeName,  STUFF( (SELECT CONCAT('; ', mm.Name  , '') FROM dbo.tblDoctorChemberDetail mm with (NOLOCK)  WHERE mm.DoctorId=CM.DoctorId ORDER BY mm.ChemberId FOR XML PATH ('') ),1,1,'') AS ChemberName, M.MarketId, M.MarketCode, M.MarketName,   T.TerritoryId, T.TerritoryName, T.TerritoryCode, T.TerShortName, 
             T.Description, A.AreaCode, A.AreaName, A.AreaId, R.RegionId, R.RegionCode, R.RegionName, G.GroupId, G.GroupName, MIO.MIOId, ASM.ASMId, RSM.RSMId, NSM.NSMId, EMIO.EmpName AS MIOEmpName, EMIO.EmpMasterCode AS MIOEmpMastercode, 
             EMIO.EmpInfoId AS MIOEmpInfoId, EASM.EmpName AS ASMEmpName, EASM.EmpMasterCode AS ASMEmpMasterCode, EASM.EmpInfoId AS ASMEmpInfoId, ERSM.EmpName AS RSMEmpName, ERSM.EmpMasterCode AS RSMEmpMasterCode, ERSM.EmpInfoId AS RSMEmpInfoId, 
             ENSM.EmpName AS NSMEmpName, ENSM.EmpMasterCode AS NSMEmpMasterCode, ENSM.EmpInfoId AS NSMEmpInfoId
FROM   dbo.tblDoctorMaster AS CM with (NOLOCK) 
LEFT JOIN dbo.tblDoctorType dt with (NOLOCK)  ON CM.DoctorTypeId=dt.DoctorTypeId
LEFT JOIN dbo.tblSMCType smct with (NOLOCK)  ON CM.SMCTypeId=smct.SMCTypeId
						LEFT JOIN dbo.tblProgramType pt with (NOLOCK)  ON CM.ProgramTypeId=pt.ProgramTypeId
 INNER JOIN
             dbo.tblMarket AS M with (NOLOCK) ON M.MarketId = REPLACE(CM.MarketId, ' ', '') AND M.IsActive = 1   INNER JOIN
             dbo.tblTerritory AS T with (NOLOCK)  ON T.TerritoryId = M.TerritoryId AND T.IsActive = 1 INNER JOIN
             dbo.tblArea AS A with (NOLOCK)  ON A.AreaId = REPLACE(T.AreaId, ' ', '') AND A.IsActive = 1 INNER JOIN
             dbo.tblRegion AS R with (NOLOCK)  ON R.RegionId = REPLACE(A.RegionId, ' ', '') AND A.IsActive = 1 INNER JOIN
             dbo.tbl_Group AS G with (NOLOCK) ON G.GroupId = R.GroupId AND G.IsActive = 1  
			  LEFT OUTER JOIN
             dbo.tblMIOInfo AS MIO with (NOLOCK)  ON MIO.TerritoryId = T.TerritoryId AND MIO.IsActive = 1 LEFT OUTER JOIN
             dbo.tblEmpGeneralInfo AS EMIO with (NOLOCK)  ON MIO.EmployeeId = EMIO.EmpInfoId LEFT OUTER JOIN
             dbo.tblASMInfo AS ASM with (NOLOCK)  ON ASM.AreaId = A.AreaId AND ASM.IsActive = 1 LEFT OUTER JOIN
             dbo.tblEmpGeneralInfo AS EASM with (NOLOCK)  ON EASM.EmpInfoId = ASM.EmployeeId LEFT OUTER JOIN
             dbo.tblRSMInfo AS RSM with (NOLOCK)  ON RSM.RegionId = R.RegionId AND RSM.IsActive = 1 LEFT OUTER JOIN
             dbo.tblEmpGeneralInfo AS ERSM with (NOLOCK)  ON ERSM.EmpInfoId = RSM.EmployeeId LEFT OUTER JOIN
             dbo.tblNSMInfo AS NSM with (NOLOCK)  ON NSM.GroupId = G.GroupId AND NSM.IsActive = 1 LEFT OUTER JOIN
             dbo.tblEmpGeneralInfo AS ENSM with (NOLOCK)  ON ENSM.EmpInfoId = NSM.EmployeeId
WHERE (CM.IsActive = '1') AND CM.ApprovalStatus='2'  
```

- **Source tables:** `tblDoctorMaster`, `tblDoctorType`, `tblSMCType`, `tblProgramType`, `tblMarket`, `tblTerritory`,
  `tblArea`, `tblRegion`, `tbl_Group`, `tblMIOInfo`/`tblASMInfo`/`tblRSMInfo`/`tblNSMInfo`, `tblEmpGeneralInfo`
  (joined 4x for MIO/ASM/RSM/NSM employee names), plus correlated subqueries against `tblDoctorContactDetail` and
  `tblDoctorChemberDetail`.
- **Joins:** LEFT JOINs to type/SMC/program lookups; then an INNER JOIN chain up the geography hierarchy
  (`Market` → `Territory` → `Area` → `Region` → `Group`, each filtered `IsActive = 1`, with `REPLACE(..., ' ',
  '')` defensively stripping stray spaces from the FK columns); then LEFT OUTER JOINs to each field-force role
  (MIO/ASM/RSM/RSM/NSM) and their employee name.
- **Filter:** `CM.IsActive = '1' AND CM.ApprovalStatus = '2'` (only active, approved doctors).
- **Derived columns:** `DoctorName` = `DoctorCode + ' : ' + DoctorName`; `DocContact` = `TOP 1 Contact` from
  `tblDoctorContactDetail` (correlated subquery, so only the first contact number is surfaced); `ChemberName` =
  semicolon-concatenated list of chamber names via `STUFF`/`FOR XML PATH`.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceArea

```sql
CREATE VIEW [dbo].[View_FieldForceArea]
AS
SELECT      distinct  ARA.AreaId, ARA.AreaName, ARA.RegionId, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode
FROM            dbo.tbl_Group AS GP
 LEFT OUTER JOIN  dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId 
						  LEFT OUTER JOIN dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId 
						  LEFT OUTER JOIN
                             (SELECT        N.GroupId, EGI.EmpMasterCode, EGI.EmpName
                               FROM            dbo.tblNSMInfo AS N LEFT OUTER JOIN
                                                         dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
                               WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             ( SELECT        RM.RegionId, EGI.EmpMasterCode, EGI.EmpName,urR.RoleName EmpRole
                               FROM            dbo.tblRSMInfo AS RM
							    LEFT  JOIN dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
								  LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (  SELECT        AM.AreaId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblASMInfo AS AM
							    LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
								  LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (
							   SELECT        M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblMIOInfo AS M 
							   LEFT  JOIN dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
							    LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1) AND (RGN.IsActive = 1) AND (ARA.IsActive = 1)
```

- **Source tables:** `tbl_Group`, `tblRegion`, `tblArea`, `tblTerritory`, `tblNSMInfo`, `tblRSMInfo`, `tblASMInfo`,
  `tblMIOInfo`, `tblEmpGeneralInfo`, `tblUser`, `tbl_UserRoleInfo`.
- **Joins:** walks the org hierarchy `tbl_Group` → `tblRegion` → `tblArea` → `tblTerritory`, then LEFT JOINs four
  independent subqueries (one per field-force role: NSM by GroupId, RSM by RegionId, ASM by AreaId, MIO by
  TerritoryId), each resolving the role's employee code/name (and, for RSM/ASM/MIO, the assigned user's role name
  via `tblUser`/`tbl_UserRoleInfo`), filtered to `IsActive = 1` rows only.
- **Filter:** `GP.IsActive = 1 AND RGN.IsActive = 1 AND ARA.IsActive = 1`. `DISTINCT` on the result set.
- **Derived columns:** none beyond the resolved NSM/RSM/ASM/MIO employee codes.
- **Application usage:** `UNUSED — no application caller found`.

This is the first of nine near-identical "field-force org chart" views (`View_FieldForceArea/Asm/Group/Market/
Mio/Nsm/Region/Rsm/Territory`) that all join the same `tbl_Group → tblRegion → tblArea → tblTerritory →
tblMarket` hierarchy plus the same four role-lookup subqueries, differing only in which hierarchy level is the
view's primary grain. None of the nine has a live application caller (see each below) — they appear to exist for
an external consumer (mobile field-force app / reporting) outside this repository.

---

## View_FieldForceAsm

```sql
CREATE VIEW [dbo].[View_FieldForceAsm]
AS
SELECT        ASM.ASMId, ASM.AreaId, ASM.EmpMasterCode, ASM.EmpName, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode, 
                         NSM.EmpInfoId AS NSMEmpId, RSM.EmpInfoId AS RSMEmpId, ASM.EmpInfoId AS ASMEmpId, MIO.EmpInfoId AS MIOEmpId
FROM            dbo.tbl_Group AS GP LEFT OUTER JOIN
                         dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId   LEFT OUTER JOIN
                         dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                             (SELECT        N.NSMId, N.GroupId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName  EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblNSMInfo AS N  
							    LEFT   JOIN   dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
                               LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
								 WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             (	 SELECT        RM.RSMId, RM.RegionId, EGI.EmpMasterCode, EGI.EmpName,  urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblRSMInfo AS RM
							    LEFT  JOIN dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
								   inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=EGI.EmpMasterCode
								     LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        empHR.EmployeeStatus='Active' and    (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             ( SELECT        AM.ASMId, AM.AreaId, EGI.EmpMasterCode, EGI.EmpName,  urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblASMInfo AS AM
							    LEFT   JOIN  dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
								   inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=EGI.EmpMasterCode
								   LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE         empHR.EmployeeStatus='Active' and     (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (SELECT        M.MIOId, M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName,  urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblMIOInfo AS M
							    LEFT   JOIN  dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
								   inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=EGI.EmpMasterCode
								  LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE          empHR.EmployeeStatus='Active' and     (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1) AND (RGN.IsActive = 1) AND (ARA.IsActive = 1)
```

- **Source tables:** same hierarchy as `View_FieldForceArea`, plus a cross-database INNER JOIN to
  `ZASHRIS..tblEmpGeneralInfo` (the separate HR system database) for RSM/ASM/MIO, gating each on
  `EmployeeStatus = 'Active'` in HR — i.e. an employee must be active in the HRIS, not just `IsActive=1` in this
  app's own tables, to appear.
- **Joins:** identical hierarchy walk to `View_FieldForceArea`, grain is ASM (Area Sales Manager) level.
- **Filter:** `GP/RGN/ARA.IsActive = 1`; each role subquery additionally requires `empHR.EmployeeStatus = 'Active'`
  (except NSM's subquery, which has no HRIS join/filter — an inconsistency versus the RSM/ASM/MIO subqueries).
- **Derived columns:** none beyond the resolved employee codes/ids per role.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceGroup

```sql
CREATE VIEW [dbo].[View_FieldForceGroup]
AS
SELECT        GP.GroupId, GP.GroupName, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode
FROM            dbo.tbl_Group AS GP LEFT OUTER JOIN
                         dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId  LEFT OUTER JOIN
                         dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                             (SELECT        N.GroupId, EGI.EmpMasterCode, EGI.EmpName
                               FROM            dbo.tblNSMInfo AS N LEFT OUTER JOIN
                                                         dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
                               WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             (   SELECT        RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblRSMInfo AS RM 
							   LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
							  LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 

                               WHERE        (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (  SELECT        AM.AreaId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblASMInfo AS AM
							    LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
								  LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (	   SELECT        M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblMIOInfo AS M 
							   LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
							    LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1)
```

- **Source tables/joins:** same hierarchy/subquery pattern as `View_FieldForceArea`, grain is Group level (no
  HRIS cross-database join here, unlike `View_FieldForceAsm`).
- **Filter:** `GP.IsActive = 1` only (looser than the Area/Asm views, which also require `RGN`/`ARA` active).
- **Derived columns:** none.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceMarket

```sql
CREATE VIEW [dbo].[View_FieldForceMarket]
AS
SELECT     MKT.TerritoryId,   MKT.MarketId, MKT.MarketName,  NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode
FROM            dbo.tbl_Group AS GP  with (nolock)  LEFT OUTER JOIN
                         dbo.tblRegion AS RGN  with (nolock)   ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA  with (nolock)  ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR  with (nolock)  ON ARA.AreaId = TTR.AreaId   LEFT OUTER JOIN
                         dbo.tblMarket AS MKT  with (nolock)  ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                             (SELECT        N.GroupId, EGI.EmpMasterCode, EGI.EmpName
                               FROM            dbo.tblNSMInfo AS N  with (nolock)  LEFT OUTER JOIN
                                                         dbo.tblEmpGeneralInfo AS EGI  with (nolock)  ON N.EmployeeId = EGI.EmpInfoId
                               WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             (SELECT        RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblRSMInfo AS RM  with (nolock) 
							   LEFT JOIN   dbo.tblEmpGeneralInfo AS EGI  with (nolock)  ON RM.EmployeeId = EGI.EmpInfoId
							    LEFT   JOIN  dbo.tblUser AS ur  with (nolock)  ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR  with (nolock)  ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (   SELECT        AM.AreaId, EGI.EmpMasterCode, EGI.EmpName,  urR.RoleName  EmpRole
                               FROM            dbo.tblASMInfo AS AM   with (nolock) 
							   LEFT   JOIN  dbo.tblEmpGeneralInfo AS EGI  with (nolock)  ON AM.EmployeeId = EGI.EmpInfoId
							     LEFT   JOIN  dbo.tblUser AS ur  with (nolock)  ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR  with (nolock)  ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (   SELECT        M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName  EmpRole
                               FROM            dbo.tblMIOInfo AS M  with (nolock) 
							    LEFT   JOIN dbo.tblEmpGeneralInfo AS EGI  with (nolock)  ON M.EmployeeId = EGI.EmpInfoId
								 LEFT   JOIN  dbo.tblUser AS ur  with (nolock)  ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR  with (nolock)  ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1) AND (RGN.IsActive = 1) AND (ARA.IsActive = 1) AND (TTR.IsActive = 1)   AND (MKT.IsActive = 1)
```

- **Source tables/joins:** same hierarchy/subquery pattern, grain is Market level; every base table hinted
  `WITH (NOLOCK)`.
- **Filter:** all five hierarchy levels required active (`GP`, `RGN`, `ARA`, `TTR`, `MKT`).
- **Derived columns:** none.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceMio

```sql
CREATE VIEW [dbo].[View_FieldForceMio]
AS
SELECT     MIO.CompanyId,    MIO.MIOId, MIO.TerritoryId, MIO.EmpName AS MIOName, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode, 
                         NSM.EmpInfoId AS NSMEmpId, RSM.EmpInfoId AS RSMEmpId, ASM.EmpInfoId AS ASMEmpId, MIO.EmpInfoId AS MIOEmpId, RGN.RegionId, ARA.AreaId
FROM            dbo.tbl_Group AS GP LEFT OUTER JOIN
                         dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId   LEFT OUTER JOIN
                         dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                             ( SELECT        N.NSMId, N.GroupId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblNSMInfo AS N 
							   LEFT OUTER JOIN dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
							     LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (N.IsActive = 1)  ) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             (	   SELECT        RM.RSMId, RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblRSMInfo AS RM
							    LEFT OUTER JOIN  dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
								  --inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=EGI.EmpMasterCode
								 LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE    --    empHR.EmployeeStatus='Active' and 
							     (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (  SELECT        AM.ASMId, AM.AreaId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblASMInfo AS AM 
							   LEFT OUTER JOIN  dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
							      --inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=EGI.EmpMasterCode
							    inner   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE     --  empHR.EmployeeStatus='Active' and
							     (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (   SELECT    m.CompanyId,    M.MIOId, M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, ISNULL(urR.RoleName,'Vacant')  EmpRole, case when ISNULL(urR.RoleName,'Vacant')='Vacant' then M.MIOId else  EGI.EmpInfoId end  EmpInfoId
                               FROM            dbo.tblMIOInfo AS M 
							   LEFT OUTER JOIN dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
							--   inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=EGI.EmpMasterCode
							    LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE     --   empHR.EmployeeStatus='Active' and  
							    (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE     (GP.IsActive = 1) AND (RGN.IsActive = 1) AND (ARA.IsActive = 1) AND (TTR.IsActive = 1)
```

- **Source tables/joins:** same hierarchy/subquery pattern, grain is MIO level. Notably the ASM subquery uses
  `INNER JOIN dbo.tblUser` (not LEFT, unlike every sibling view) — an ASM with no `tblUser` row is silently
  dropped from this specific subquery. The `ZASHRIS..tblEmpGeneralInfo` HRIS join is commented out for RSM/ASM/MIO
  here (present and live in `View_FieldForceAsm`) — an inconsistency between the two views.
- **Filter:** `GP/RGN/ARA/TTR.IsActive = 1`.
- **Derived columns:** MIO subquery's `EmpRole` defaults to `'Vacant'` via `ISNULL`, and `EmpInfoId` is a `CASE`
  that falls back to `M.MIOId` itself when the role is vacant (i.e. no employee assigned) rather than a real
  employee id — a modeling quirk worth flagging if this view is ever revived for use.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceNsm

```sql
CREATE VIEW [dbo].[View_FieldForceNsm]
AS
SELECT        NSM.NSMId, NSM.GroupId, NSM.EmpMasterCode, NSM.EmpName, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode, 
                         NSM.EmpInfoId AS NSMEmpId, RSM.EmpInfoId AS RSMEmpId, ASM.EmpInfoId AS ASMEmpId, MIO.EmpInfoId AS MIOEmpId
FROM            dbo.tbl_Group AS GP LEFT OUTER JOIN
                         dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId   LEFT OUTER JOIN
                         dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                             ( SELECT        N.NSMId, N.GroupId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblNSMInfo AS N 
							   LEFT OUTER JOIN  dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
							    LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             (
							   SELECT        RM.RSMId, RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblRSMInfo AS RM 
							   LEFT OUTER JOIN   dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
							      LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (  SELECT        AM.ASMId, AM.AreaId, EGI.EmpMasterCode, EGI.EmpName,  urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblASMInfo AS AM
							    LEFT OUTER JOIN dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
								 LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (
							   SELECT        M.MIOId, M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblMIOInfo AS M 
							   LEFT OUTER JOIN  dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
						  LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1)
```

- **Source tables/joins:** same pattern, grain is NSM level.
- **Filter:** `GP.IsActive = 1` only.
- **Derived columns:** none.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceRegion

```sql
CREATE VIEW [dbo].[View_FieldForceRegion]
AS
SELECT        RGN.RegionId, RGN.RegionName, RGN.GroupId, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode
FROM            dbo.tbl_Group AS GP LEFT OUTER JOIN
                         dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId  LEFT OUTER JOIN
                         dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                             (SELECT        N.GroupId, EGI.EmpMasterCode, EGI.EmpName
                               FROM            dbo.tblNSMInfo AS N LEFT OUTER JOIN
                                                         dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
                               WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             (	   SELECT        RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblRSMInfo AS RM 
							   LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
							    LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (  SELECT        AM.AreaId, EGI.EmpMasterCode, EGI.EmpName,  urR.RoleName EmpRole
                               FROM            dbo.tblASMInfo AS AM
							    LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
								LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (			   SELECT        M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblMIOInfo AS M 
							   LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
								LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1) AND (RGN.IsActive = 1)
```

- **Source tables/joins:** same pattern, grain is Region level.
- **Filter:** `GP.IsActive = 1 AND RGN.IsActive = 1`.
- **Derived columns:** none.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceRsm

```sql
CREATE VIEW [dbo].[View_FieldForceRsm]
AS
SELECT        RSM.RSMId, RSM.RegionId, RSM.EmpMasterCode, RSM.EmpName, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode, 
                         NSM.EmpInfoId AS NSMEmpId, RSM.EmpInfoId AS RSMEmpId, ASM.EmpInfoId AS ASMEmpId, MIO.EmpInfoId AS MIOEmpId
FROM            dbo.tbl_Group AS GP LEFT OUTER JOIN
                         dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId  LEFT OUTER JOIN
                         dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                             (  SELECT        N.NSMId, N.GroupId, EGI.EmpMasterCode, EGI.EmpName,urR.RoleName   EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblNSMInfo AS N 
							   LEFT OUTER JOIN   dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
							       LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             ( SELECT        RM.RSMId, RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblRSMInfo AS RM 
							   LEFT OUTER JOIN  dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
							     LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (  SELECT        AM.ASMId, AM.AreaId, EGI.EmpMasterCode, EGI.EmpName,  urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblASMInfo AS AM
							    LEFT OUTER JOIN  dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
								 LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (   SELECT        M.MIOId, M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName,urR.RoleName EmpRole, EGI.EmpInfoId
                               FROM            dbo.tblMIOInfo AS M 
							   LEFT OUTER JOIN  dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
							    LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1) AND (RGN.IsActive = 1)
```

- **Source tables/joins:** same pattern, grain is RSM level.
- **Filter:** `GP.IsActive = 1 AND RGN.IsActive = 1`.
- **Derived columns:** none.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_FieldForceTerritory

```sql
CREATE VIEW [dbo].[View_FieldForceTerritory]
AS
SELECT        TTR.TerritoryId,TTR.TerritoryCode+' : ' + TTR.TerritoryName TerritoryName, TTR.AreaId, NSM.EmpMasterCode AS NSMCode, RSM.EmpMasterCode AS RSMCode, ASM.EmpMasterCode AS ASMCode, MIO.EmpMasterCode AS MIOCode
FROM            dbo.tbl_Group AS GP LEFT OUTER JOIN
                         dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
                         dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
                         dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId 
						   LEFT OUTER JOIN
                             (SELECT        N.GroupId, EGI.EmpMasterCode, EGI.EmpName
                               FROM            dbo.tblNSMInfo AS N LEFT OUTER JOIN
                                                         dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
                               WHERE        (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId LEFT OUTER JOIN
                             (SELECT        RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblRSMInfo AS RM
							    LEFT   JOIN dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
								 LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                             (	   SELECT        AM.AreaId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblASMInfo AS AM 
							   LEFT  JOIN  dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
							    LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                             (SELECT        M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, urR.RoleName EmpRole
                               FROM            dbo.tblMIOInfo AS M 
							   LEFT   JOIN  dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
							      LEFT   JOIN  dbo.tblUser AS ur ON ur.EmpInfoId = EGI.EmpInfoId
							    LEFT   JOIN dbo.tbl_UserRoleInfo AS urR ON urR.UserRoleID = ur.UserRoleID 
                               WHERE        (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId
WHERE        (GP.IsActive = 1) AND (RGN.IsActive = 1) AND (ARA.IsActive = 1) AND (TTR.IsActive = 1)
```

- **Source tables/joins:** same pattern, grain is Territory level.
- **Filter:** `GP/RGN/ARA/TTR.IsActive = 1`.
- **Derived columns:** `TerritoryName` = `TerritoryCode + ' : ' + TerritoryName`.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_InvoiceWiseDue

```sql
CREATE VIEW [dbo].[View_InvoiceWiseDue]
AS
SELECT INV.InvoiceId, INV.CustomerMasterId, INV.ComUnitId, SUM(ISNULL(IVD.DeliveryNetAmount, INV.DeliveryTpGrandTotal)) AS TotalDelivery, ISNULL(PD.ActualPayment, 0) AS ActualPayment, ISNULL(PD.AIT, 0) AS AIT, 
                  ISNULL(PD.DiscountAmount, 0) AS DiscountAmount, ISNULL(PD.TotalPayment, 0) AS TotalPayment, SUM(ISNULL(IVD.DeliveryNetAmount, INV.DeliveryTpGrandTotal)) - ISNULL(PD.TotalPayment, 0) - ISNULL(RTN.ReturnAmount, 0) AS Due, 
                  ISNULL(RTN.ReturnAmount, 0) AS ReturnAmount
FROM     dbo.tblInvoice AS INV LEFT OUTER JOIN
                  dbo.tblInvoiceDetail AS IVD ON INV.InvoiceId = IVD.InvoiceId LEFT OUTER JOIN
                      (SELECT M.CommonPK_ID, SUM(D.DebitAmountBDT) AS ReturnAmount
                       FROM      CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster AS M LEFT OUTER JOIN
                                         CSTL_ACCDB.dbo.tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId LEFT OUTER JOIN
                                         dbo.tblInvoice AS INV ON M.CommonPK_ID = INV.InvoiceId
                       WHERE   (ISNULL(M.IsInventoryValuation, 0) = 0) AND (ISNULL(INV.PaymentAmount, 0) = 0) AND (M.TableID = 2018)
                       GROUP BY M.CommonPK_ID) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID LEFT OUTER JOIN
                      (SELECT InvoiceId, SUM(PaymentAmount) AS ActualPayment, SUM(ISNULL(AIT, 0)) AS AIT, SUM(ISNULL(Discount, 0)) AS DiscountAmount, SUM(PaymentAmount) + SUM(ISNULL(AIT, 0)) + SUM(ISNULL(Discount, 0)) 
                                         AS TotalPayment
                       FROM      dbo.tblCustPayDetail AS CPD
                       GROUP BY InvoiceId
                       HAVING  (SUM(PaymentAmount) + SUM(ISNULL(AIT, 0)) + SUM(ISNULL(Discount, 0)) > 0)) AS PD ON INV.InvoiceId = PD.InvoiceId
WHERE  (INV.DelivaryInvoiceNo IS NOT NULL)
GROUP BY INV.InvoiceId, INV.CustomerMasterId, INV.ComUnitId, PD.TotalPayment, PD.ActualPayment, PD.AIT, PD.DiscountAmount, RTN.ReturnAmount
```

- **Source tables:** `tblInvoice`, `tblInvoiceDetail`, `tblCustPayDetail`, plus a cross-database join to
  `CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster`/`tblDebitCreditVoucherDetail` (the accounts DB) for returns.
- **Joins:** LEFT OUTER JOIN `tblInvoice` to `tblInvoiceDetail` on `InvoiceId`; LEFT JOIN to a `RTN` subquery
  (return/debit-note amount from the accounts DB, filtered to non-inventory-valuation vouchers of `TableID = 2018`
  with zero payment already recorded); LEFT JOIN to a `PD` subquery (aggregated payments from `tblCustPayDetail`).
- **Filter:** `DelivaryInvoiceNo IS NOT NULL` (only delivered invoices).
- **Derived columns:** `Due` = `SUM(delivery net amount) - TotalPayment - ReturnAmount`; `TotalPayment` =
  `PaymentAmount + AIT + Discount` summed.
- **Application usage:** `UNUSED — no application caller found` (live). The only reference in the codebase is a
  **commented-out** query in `OrderInfoDAL.GetCustomerDueHistory`
  (`Library.DAL\SInventory_DAL\OrderInfoDAL.cs:400-405`) — that method's live implementation
  (lines 407-419) has been rewritten to compute due amount with an inline query against `tblInvoice`/
  `tblCustMaster`/`tblCompanyUnit`/`tblCompanyInfo` directly, no longer using this view. The old spec's claim that
  `OrderInfoDAL.GetCustomerDueHistory` is a live caller of this view is **stale** — it was true of an earlier
  version of the code.

---

## View_InvoiceWisePayment

```sql
CREATE VIEW dbo.View_InvoiceWisePayment
AS
SELECT InvoiceId, SUM(PaymentAmount) AS PP, SUM(ISNULL(AIT, 0)) AS AIT, SUM(ISNULL(Discount, 0)) AS DiscountAmount
FROM     dbo.tblCustPayDetail AS CPD
GROUP BY InvoiceId
HAVING (SUM(PaymentAmount) + SUM(ISNULL(AIT, 0)) + SUM(ISNULL(Discount, 0)) > 0)
```

- **Source tables:** `tblCustPayDetail`.
- **Joins:** none — single-table aggregate.
- **Filter:** `HAVING SUM(PaymentAmount) + SUM(AIT) + SUM(Discount) > 0` (excludes invoices with net-zero
  payment activity).
- **Derived columns:** `PP` = `SUM(PaymentAmount)`; `AIT` = `SUM(ISNULL(AIT,0))`; `DiscountAmount` =
  `SUM(ISNULL(Discount,0))`.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_InvoiceWiseSales

```sql
CREATE VIEW dbo.View_InvoiceWiseSales
AS
SELECT INV.InvoiceId, INV.CustomerMasterId, INV.ComUnitId, SUM(ISNULL(IVD.DeliveryNetAmount, INV.DeliveryTpGrandTotal)) AS TotalDelivery
FROM     dbo.tblInvoice AS INV LEFT OUTER JOIN
                  dbo.tblInvoiceDetail AS IVD ON INV.InvoiceId = IVD.InvoiceId
WHERE  (INV.DelivaryInvoiceNo IS NOT NULL)
GROUP BY INV.InvoiceId, INV.CustomerMasterId, INV.ComUnitId
HAVING (SUM(ISNULL(IVD.DeliveryNetAmount, INV.DeliveryTpGrandTotal)) > 0)
```

- **Source tables:** `tblInvoice`, `tblInvoiceDetail`.
- **Joins:** LEFT OUTER JOIN on `InvoiceId`.
- **Filter:** `DelivaryInvoiceNo IS NOT NULL`; `HAVING SUM(delivery net amount) > 0`.
- **Derived columns:** `TotalDelivery` = `SUM(ISNULL(DeliveryNetAmount, DeliveryTpGrandTotal))` (falls back to the
  invoice-header grand total when the line-level net amount is null).
- **Application usage:** `UNUSED — no application caller found`.

---

## View_MioWiseInvoiceSummary

```sql
CREATE VIEW View_MioWiseInvoiceSummary AS
SELECT TOP (100) PERCENT 
    dbo.tblEmpGeneralInfo.EmpName AS MIO, 
    DATENAME(YEAR, INV.InvoiceDate) AS Year, 
    DATENAME(MONTH, INV.InvoiceDate) AS MONTH, 
    INV.ComUnitId, 
    INV.MiaId, 
    UNT.CompanyId,
    CI.CompanyCode,
    CI.CompanyName,
    UNT.ComUnitCode, 
    UNT.ComUnitName, 
    ARA.AreaName, 
    TTR.TerritoryName, 
    PD.ProductId, 
    PD.ProductCode, 
    PD.ProductName, 
    PD.PackSize, 
    SUM(ISNULL(INVD.DeliveryTotalQuantity, 0)) AS DeliveryTotalQuantity, 
    SUM(ISNULL(INVD.DeliveryNetAmount, 0)) AS DeliveryNetAmount, 
    ISNULL(TRGT.TargetQty, 0) AS TargetQty
FROM 
    dbo.tblInvoice AS INV 
LEFT OUTER JOIN 
    dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
LEFT OUTER JOIN 
    dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId 
LEFT OUTER JOIN 
    dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
LEFT OUTER JOIN 
    dbo.tblProduct AS PD ON INVD.ProductCode = PD.ProductCode 
LEFT OUTER JOIN 
    dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId = INV.MiaId 
LEFT OUTER JOIN 
    dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId 
LEFT OUTER JOIN 
    dbo.tblTerritory AS TTR ON TTR.TerritoryId = dbo.tblMIOInfo.TerritoryId 
LEFT OUTER JOIN 
    dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
LEFT OUTER JOIN 
    (SELECT MiaId, ProductId, CompanyId, Year, Period, TargetQty
     FROM dbo.tblMIATargetProductWise) AS TRGT 
     ON INV.MiaId = TRGT.MiaId 
     AND UNT.CompanyId = TRGT.CompanyId 
     AND DATENAME(YEAR, INV.InvoiceDate) = TRGT.Year 
     AND DATENAME(MONTH, INV.InvoiceDate) = TRGT.Period 
     AND PD.ProductId = TRGT.ProductId
GROUP BY 
    INV.InvoiceDate, 
    dbo.tblEmpGeneralInfo.EmpName, 
    UNT.CompanyId,
    INV.ComUnitId,
    CI.CompanyCode,
    CI.CompanyName, 
    INV.MiaId, 
    UNT.ComUnitCode, 
    UNT.ComUnitName, 
    ARA.AreaName, 
    TTR.TerritoryName, 
    PD.ProductId, 
    PD.ProductCode, 
    PD.ProductName, 
    PD.PackSize, 
    TRGT.TargetQty
HAVING 
    SUM(ISNULL(INVD.DeliveryTotalQuantity, 0)) > 0 OR ISNULL(TRGT.TargetQty, 0) > 0
ORDER BY 
    MONTH DESC;
```

- **Source tables:** `tblInvoice`, `tblInvoiceDetail`, `tblCompanyUnit`, `tblCompanyInfo`, `tblProduct`,
  `tblMIOInfo`, `tblEmpGeneralInfo`, `tblTerritory`, `tblArea`, `tblMIATargetProductWise`.
  Sales-vs-target reporting view for the MIO (Medical/Marketing Information Officer) role.
- **Joins:** invoice → invoice-detail → company unit → company info → product, all LEFT OUTER; MIO info →
  employee name → territory → area, all LEFT OUTER; a target-quantity subquery joined on the composite key
  `(MiaId, CompanyId, Year, Period, ProductId)` — matching each invoice's month/year textually against
  `tblMIATargetProductWise.Year`/`.Period`.
- **Filter:** `HAVING SUM(delivered qty) > 0 OR target qty > 0` (drops rows with neither sales nor a target).
- **Derived columns:** `Year`/`MONTH` via `DATENAME`; `DeliveryTotalQuantity`/`DeliveryNetAmount` summed with
  `ISNULL` zero-fallback; `TargetQty` defaulted to 0. Uses the deprecated `TOP (100) PERCENT ... ORDER BY` pattern
  (an ORDER BY on a view has no guaranteed effect once the view itself is queried — a pre-SQL2005-era idiom that
  no longer does anything useful here).
- **Application usage:** `UNUSED — no application caller found`.

---

## View_SalesMonthYearWise

```sql
CREATE VIEW dbo.View_SalesMonthYearWise
AS
SELECT        INV.ComUnitId, INV.MiaId, UNT.ComUnitCode, UNT.ComUnitName, PD.ProductId, SUM(INVD.DeliveryNetAmount) AS Value, ISNULL(SUM(INVD.DeliveryTotalQuantity), 0) AS Qty, FORMAT(INV.InvoiceDate, 'MMMM') 
                         AS MonthName, YEAR(INV.InvoiceDate) AS Year
FROM            dbo.tblInvoice AS INV LEFT OUTER JOIN
                         dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId LEFT OUTER JOIN
                         dbo.tblProduct AS PD ON INVD.ProductCode = PD.ProductCode LEFT OUTER JOIN
                         dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
WHERE        (INV.DeliveryInvoiceStatus IN ('Full', 'Partial'))
GROUP BY INV.ComUnitId, UNT.ComUnitCode, UNT.ComUnitName, PD.ProductId, INV.MiaId, FORMAT(INV.InvoiceDate, 'MMMM'), YEAR(INV.InvoiceDate)
```

- **Source tables:** `tblInvoice`, `tblInvoiceDetail`, `tblProduct`, `tblCompanyUnit`.
- **Joins:** LEFT OUTER JOIN chain invoice → invoice-detail → product, and invoice → company unit.
- **Filter:** `DeliveryInvoiceStatus IN ('Full', 'Partial')`.
- **Derived columns:** `Value` = `SUM(DeliveryNetAmount)`; `Qty` = `ISNULL(SUM(DeliveryTotalQuantity), 0)`;
  `MonthName` = `FORMAT(InvoiceDate, 'MMMM')`; `Year` = `YEAR(InvoiceDate)`.
- **Application usage:** `UNUSED — no application caller found`.

---

## View_Webapi_EmployeeFieldForceInfo

```sql
CREATE VIEW [dbo].[View_Webapi_EmployeeFieldForceInfo]
AS
SELECT distinct dbo.tblEmpGeneralInfo.EmpInfoId, dbo.tblEmpGeneralInfo.EmpMasterCode, dbo.tblEmpGeneralInfo.EmpName AS EmpRole, dbo.tblMIOInfo.TerritoryId, dbo.tblASMInfo.AreaId, dbo.tblRSMInfo.RegionId, dbo.tblNSMInfo.GroupId, dbo.tblTerritory.TerritoryId AS EmpTerrId, 
             dbo.tblTerritory.TerritoryName, dbo.tblTerritory.TerritoryCode, dbo.tblArea.AreaCode, dbo.tblArea.AreaName, dbo.tblArea.AreaId AS EmpAreaId, dbo.tblRegion.RegionId AS EmpRegionId, dbo.tblRegion.RegionCode, dbo.tblRegion.RegionName, 
             dbo.tbl_Group.GroupId AS EmpGroupId, dbo.tbl_Group.GroupName, tbl_Group.GroupCode, MIO.MIOEmpId, ASM.ASMEMPId, RSM.RSMEMPId, NSM.NSMEMPId
FROM   dbo.tblEmpGeneralInfo   with (nolock)
inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=tblEmpGeneralInfo.EmpMasterCode
  LEFT OUTER JOIN
             dbo.tblMIOInfo   with (nolock)  ON  dbo.tblMIOInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId AND dbo.tblMIOInfo.IsActive = 1 LEFT OUTER JOIN
             dbo.tblASMInfo   with (nolock)  ON dbo.tblASMInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId AND dbo.tblASMInfo.IsActive = 1  LEFT OUTER JOIN
             dbo.tblRSMInfo   with (nolock)  ON dbo.tblRSMInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId  AND dbo.tblRSMInfo.IsActive = 1  LEFT OUTER JOIN
             dbo.tblNSMInfo   with (nolock)  ON dbo.tblNSMInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId AND dbo.tblNSMInfo.IsActive = 1 LEFT OUTER JOIN
             dbo.tblTerritory   with (nolock)  ON dbo.tblTerritory.TerritoryId = dbo.tblMIOInfo.TerritoryId LEFT OUTER JOIN
             dbo.tblArea   with (nolock)  ON dbo.tblArea.AreaId = dbo.tblASMInfo.AreaId OR dbo.tblArea.AreaId = dbo.tblTerritory.AreaId LEFT OUTER JOIN
             dbo.tblRegion   with (nolock)  ON dbo.tblRegion.RegionId = dbo.tblRSMInfo.RegionId OR dbo.tblRegion.RegionId = dbo.tblArea.RegionId LEFT OUTER JOIN
             dbo.tbl_Group   with (nolock)  ON dbo.tbl_Group.GroupId = dbo.tblNSMInfo.GroupId   LEFT OUTER JOIN
                 (SELECT M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS MIOEmpId, M.MIOId
                 FROM    dbo.tblMIOInfo AS M   with (nolock)  LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI   with (nolock)  ON M.EmployeeId = EGI.EmpInfoId
                 WHERE (M.IsActive = 1)) AS MIO ON MIO.MIOId = dbo.tblMIOInfo.MIOId LEFT OUTER JOIN
                 (SELECT AM.AreaId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS ASMEMPId
                 FROM    dbo.tblASMInfo AS AM   with (nolock)  LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI   with (nolock)  ON AM.EmployeeId = EGI.EmpInfoId
                 WHERE (AM.IsActive = 1)) AS ASM ON ASM.AreaId = dbo.tblArea.AreaId LEFT OUTER JOIN
                 (SELECT RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS RSMEMPId
                 FROM    dbo.tblRSMInfo AS RM   with (nolock)  LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI   with (nolock)  ON RM.EmployeeId = EGI.EmpInfoId
                 WHERE (RM.IsActive = 1)) AS RSM ON RSM.RegionId = dbo.tblRegion.RegionId LEFT OUTER JOIN
                 (SELECT N.GroupId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS NSMEMPId
                 FROM    dbo.tblNSMInfo AS N    with (nolock) LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI   with (nolock)  ON N.EmployeeId = EGI.EmpInfoId
                 WHERE (N.IsActive = 1)) AS NSM ON NSM.GroupId = dbo.tbl_Group.GroupId

			   where empHR.EmployeeStatus='Active'
```

- **Source tables:** `tblEmpGeneralInfo` (joined to the HRIS `ZASHRIS..tblEmpGeneralInfo`), `tblMIOInfo`,
  `tblASMInfo`, `tblRSMInfo`, `tblNSMInfo`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`.
- **Joins:** an employee-centric variant of the org-chart pattern — one employee row joined out to whichever of
  MIO/ASM/RSM/NSM role tables they hold, then resolves the org node upward (territory/area/region/group), with a
  peculiar `OR`-based join for `tblArea` (`AreaId = tblASMInfo.AreaId OR AreaId = tblTerritory.AreaId`) and
  `tblRegion` (`RegionId = tblRSMInfo.RegionId OR RegionId = tblArea.RegionId`) so an employee resolves their
  region/area whether they hold that role directly or one level below it.
- **Filter:** `empHR.EmployeeStatus = 'Active'` (HRIS-side status only — no `IsActive` check on
  `tblEmpGeneralInfo` itself). `DISTINCT`.
- **Derived columns:** none beyond the resolved role ids/names.
- **Application usage:** `UNUSED — no application caller found`. The `Webapi` naming strongly suggests this
  backs a REST API for the field-force mobile app that lives outside this repository.

---

## View_webapi_FieldForce

```sql
CREATE VIEW [dbo].[View_webapi_FieldForce]
AS
SELECT GP.GroupId, GP.GroupCode, GP.GroupName, NSM.EmpName AS NSMName, NSM.EmpMasterCode AS NSMCode, RGN.RegionId, RGN.RegionCode, RGN.RegionName, RSM.EmpName AS RSMName, RSM.EmpMasterCode AS RSMCode, ARA.AreaId, ARA.AreaCode, ARA.AreaName, 
             ASM.EmpName AS ASMName, ASM.EmpMasterCode AS ASM, TTR.TerritoryId, TTR.TerritoryCode, TTR.TerritoryName, MIO.EmpName AS MIOName, MIO.EmpMasterCode AS MIOCode,   MKT.MarketId, 
             MKT.MarketCode, MKT.MarketName, ASM.ASMEMPId, RSM.RSMEMPId, NSM.NSMEMPId, MIO.MIOEmpId, MIO.MIOId
FROM   dbo.tbl_Group AS GP LEFT OUTER JOIN
             dbo.tblRegion AS RGN ON GP.GroupId = RGN.GroupId LEFT OUTER JOIN
             dbo.tblArea AS ARA ON RGN.RegionId = ARA.RegionId LEFT OUTER JOIN
             dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId   LEFT OUTER JOIN
             dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId LEFT OUTER JOIN
                 (SELECT RM.RegionId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS RSMEMPId
                 FROM    dbo.tblRSMInfo AS RM LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI ON RM.EmployeeId = EGI.EmpInfoId
                 WHERE (RM.IsActive = 1)) AS RSM ON RGN.RegionId = RSM.RegionId LEFT OUTER JOIN
                 (SELECT AM.AreaId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS ASMEMPId
                 FROM    dbo.tblASMInfo AS AM LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI ON AM.EmployeeId = EGI.EmpInfoId
                 WHERE (AM.IsActive = 1)) AS ASM ON ARA.AreaId = ASM.AreaId LEFT OUTER JOIN
                 (SELECT M.TerritoryId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS MIOEmpId, M.MIOId
                 FROM    dbo.tblMIOInfo AS M LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI ON M.EmployeeId = EGI.EmpInfoId
                 WHERE (M.IsActive = 1)) AS MIO ON TTR.TerritoryId = MIO.TerritoryId LEFT OUTER JOIN
                 (SELECT N.GroupId, EGI.EmpMasterCode, EGI.EmpName, EGI.EmpInfoId AS NSMEMPId
                 FROM    dbo.tblNSMInfo AS N LEFT OUTER JOIN
                              dbo.tblEmpGeneralInfo AS EGI ON N.EmployeeId = EGI.EmpInfoId
                 WHERE (N.IsActive = 1)) AS NSM ON NSM.GroupId = GP.GroupId
WHERE (GP.IsActive = 1) AND (RGN.IsActive = 1) AND (ARA.IsActive = 1) AND (TTR.IsActive = 1) AND  (MKT.IsActive = 1)
```

- **Source tables/joins:** the org-chart hierarchy again (`tbl_Group → tblRegion → tblArea → tblTerritory →
  tblMarket`) with the four role subqueries, but no HRIS join — a market-grain, lowercase-named sibling of
  `View_webapi_FieldForce`'s uppercase cousins.
- **Filter:** all five hierarchy levels required active.
- **Derived columns:** none.
- **Application usage:** `UNUSED — no application caller found`.

---

## ZAS_UserFS_AM

```sql
CREATE View  [dbo].[ZAS_UserFS_AM]
as
 select  A.EmpInfoId, com.CompanyId, 0 TerritoryId,   ''TerritoryCode, '' TerritoryName, ar.AreaId, ar.AreaCode, ar.AreaName, rg.RegionId, rg.RegionCode, rg.RegionName, gp.GroupId, gp.GroupCode, gp.GroupName  FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 INNER JOIN ZASHRIS..tblEmpGeneralInfo HRE ON B.EmpMasterCode = HRE.EmpMasterCode 
	 INNER JOIN dbo.tblASMInfo asm ON asm.EmployeeId = A.EmpInfoId 
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId 
	    INNER JOIN dbo.tblArea ar   with (nolock) ON ar.AreaId = asm.AreaId
        INNER JOIN dbo.tblRegion rg    with (nolock)ON rg.RegionId = ar.RegionId
        INNER JOIN dbo.tbl_Group gp   with (nolock) ON gp.GroupId = rg.GroupId
				 INNER JOIN dbo.tblCompanyInfo com ON com.CompanyId = asm.CompanyId 
				  where HRE.EmployeeStatus='Active' and asm.IsActive=1
```

- **Source tables:** `tblUser`, `tblEmpGeneralInfo`, `ZASHRIS..tblEmpGeneralInfo` (HRIS), `tblASMInfo`,
  `tbl_UserRoleInfo`, `tblRoleType`, `tblArea`, `tblRegion`, `tbl_Group`, `tblCompanyInfo`. "FS" = field-sales;
  "AM" = Area Manager (ASM).
  Resolves the logged-in user's org placement for an ASM-level user.
- **Joins:** `tblUser` → `tblEmpGeneralInfo` → HRIS `tblEmpGeneralInfo` (must match by `EmpMasterCode`) → INNER
  JOIN `tblASMInfo` (so only users who are also an ASM appear) → area → region → group → company info; LEFT JOIN
  to role/role-type (informational only, not filtered on).
- **Filter:** `HRE.EmployeeStatus = 'Active' AND asm.IsActive = 1`.
- **Derived columns:** `TerritoryId` is hardcoded to literal `0`, `TerritoryCode`/`TerritoryName` hardcoded to
  empty strings `''` — because an ASM spans multiple territories, so no single territory applies; this is a
  deliberate placeholder rather than a real join result.
- **Application usage:** `UNUSED — no application caller found`.

---

## ZAS_UserFS_MIO

```sql
CREATE View  ZAS_UserFS_MIO
as
 select  A.EmpInfoId, com.CompanyId, terry.TerritoryId, terry.TerritoryCode, terry.TerritoryName, ar.AreaId, ar.AreaCode, ar.AreaName, rg.RegionId, rg.RegionCode, rg.RegionName, gp.GroupId, gp.GroupCode, gp.GroupName  FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 INNER JOIN ZASHRIS..tblEmpGeneralInfo HRE ON B.EmpMasterCode = HRE.EmpMasterCode 
	 INNER JOIN dbo.tblMIOInfo mio ON mio.EmployeeId = A.EmpInfoId 
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId 
				  INNER JOIN dbo.tblTerritory terry   with (nolock) ON terry.TerritoryId = mio.TerritoryId
        INNER JOIN dbo.tblArea ar   with (nolock) ON ar.AreaId = terry.AreaId
        INNER JOIN dbo.tblRegion rg    with (nolock)ON rg.RegionId = ar.RegionId
        INNER JOIN dbo.tbl_Group gp   with (nolock) ON gp.GroupId = rg.GroupId
				 INNER JOIN dbo.tblCompanyInfo com ON com.CompanyId = mio.CompanyId 
				  where HRE.EmployeeStatus='Active' and mio.IsActive=1
```

- **Source tables/joins:** the MIO-level sibling of `ZAS_UserFS_AM` — INNER JOIN `tblMIOInfo` instead of
  `tblASMInfo`, then resolves the real single territory → area → region → group upward, since an MIO is assigned
  to exactly one territory (unlike the ASM view, `TerritoryId`/`Code`/`Name` here come from a real join, not a
  placeholder).
- **Filter:** `HRE.EmployeeStatus = 'Active' AND mio.IsActive = 1`.
- **Derived columns:** none (real values throughout, no placeholders).
- **Application usage:** `UNUSED — no application caller found`.

---

## ZAS_UserFS_RSM

```sql
CREATE View  [dbo].[ZAS_UserFS_RSM]
as
 select  A.EmpInfoId, com.CompanyId, 0 TerritoryId,   ''TerritoryCode, '' TerritoryName, ''AreaId, ''AreaCode, ''AreaName, rg.RegionId, rg.RegionCode, rg.RegionName, gp.GroupId, gp.GroupCode, gp.GroupName  FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 INNER JOIN ZASHRIS..tblEmpGeneralInfo HRE ON B.EmpMasterCode = HRE.EmpMasterCode 
	 INNER JOIN dbo.tblRSMInfo rsm ON rsm.EmployeeId = A.EmpInfoId 
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId 
        INNER JOIN dbo.tblRegion rg    with (nolock)ON rg.RegionId = rsm.RegionId
        INNER JOIN dbo.tbl_Group gp   with (nolock) ON gp.GroupId = rg.GroupId
				 INNER JOIN dbo.tblCompanyInfo com ON com.CompanyId = rsm.CompanyId 
				  where HRE.EmployeeStatus='Active' and rsm.IsActive=1
```

- **Source tables/joins:** the RSM-level sibling — INNER JOIN `tblRSMInfo`, resolves region → group directly
  (no territory/area join at all, since an RSM spans a whole region across many areas/territories).
- **Filter:** `HRE.EmployeeStatus = 'Active' AND rsm.IsActive = 1`.
- **Derived columns:** `TerritoryId` hardcoded `0`; `TerritoryCode`/`TerritoryName`/`AreaId`/`AreaCode`/`AreaName`
  all hardcoded empty-string placeholders (an RSM has neither a single territory nor a single area).
- **Application usage:** `UNUSED — no application caller found`.

---

## Summary

24/24 views documented with full verbatim SQL. Application usage:

| View | Usage |
|---|---|
| `View_CentralStoreCurrentStock` | Used — `RequisitionDAL.GetPickingDataForIssueDAL` |
| `View_CustomerMaster` | Used — 21 DAL classes (see above) |
| `View_DCStoreCurrentStock` | Used — `InvoiceDAL.ProductInfoDAL` (both overloads) |
| `View_InvoiceWiseDue` | UNUSED (live caller removed; only a commented-out reference remains) |
| all other 21 views | `UNUSED — no application caller found` |

The 21 unused views split into two families: nine near-identical "field-force org chart" views
(`View_FieldForce*`) and their `Webapi`/`ZAS_UserFS_*` cousins (11 more), and a handful of financial rollups
(`View_InvoiceWiseDue/Payment/Sales`, `View_SalesMonthYearWise`, `View_MioWiseInvoiceSummary`, `View_DCR`,
`View_DoctorMaster`). None of these are called from `Library.DAL`/`Library.BLL`/`Solution.Web` — they most likely
back an external consumer (Crystal Reports/SSRS, a separate WebAPI project, or the field-force mobile app) not
present in this repository, or are leftover from an earlier version of the application.
