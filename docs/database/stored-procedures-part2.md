# Stored Procedures — Part 2 (Batch 2 of 4)

Documentation for 131 stored procedures (batch 2 of 4, ~515 total in `SalesrollDB_ZAS`), reverse-engineered directly from live-database T-SQL bodies. Each entry lists parameters, tables read/written, business logic, transaction/error handling, result set, and application caller(s) found via grep across `Library.DAL`, `Library.BLL`, and `Solution.Web`. Procedures with no caller found in this codebase are marked `UNUSED — no application caller found`.

### sp_GET_Employee_Leave_HRAPI

- **Parameters:** @LeaveYear (nvarchar(max), IN), @EmpInfoId (nvarchar(max), IN)
- **Tables read:** tblEmpGeneralInfo, ZASHRIS..tblLeaveInventory
- **Tables written:** none
- **Business logic:** Looks up an employee's master code from `tblEmpGeneralInfo`, then returns leave-balance rows from the HR system's `ZASHRIS..tblLeaveInventory` for the current calendar year where remaining leave quantity (`DayQty`) is greater than zero. Several output columns (LeaveReason, Status, ApprovedBy, etc.) are hard-coded to empty-string placeholders rather than pulled from real data.
- **Transaction/error handling:** none
- **Result set:** One row per available leave type for the employee: EmpMasterCode, EmpName, EmpInfoId, AvailLeaveQty, LeaveName, plus several placeholder/blank columns (LeaveReason, Status, ApprovedBy, ApprovedDate, ActionStatus, DayName, RowNo, a computed "Year/Month" string, FromDate, ToDate, LeaveAvailId).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep across Library.DAL, Library.BLL, Solution.Web found no references).

```sql
CREATE PROCEDURE [dbo].[sp_GET_Employee_Leave_HRAPI] 
	
	@LeaveYear Nvarchar(MAx),
	@EmpInfoId Nvarchar(MAx)
AS
BEGIN


    DECLARE @EmpMasterCode Nvarchar(MAX)

	SELECT @EmpMasterCode=EmpMasterCode FROM tblEmpGeneralInfo WHERE EmpInfoId=@EmpInfoId  
	
	--SELECT LeaveReason,LeaveName,ApprovedBy, CONVERT(nvarchar,ApprovedDate,106) ApprovedDate, DayName, RowNo, CASE WHEN  AvailLeaveQty IS NULL THEN 0 ELSE  AvailLeaveQty END AvailLeaveQty  
	SELECT EmpMasterCode,EmpName,EmpInfoId, DayQty AvailLeaveQty,'' LeaveReason,LeaveName,'' Status,'' ApprovedBy,'' ApprovedDate,'' ActionStatus,'2' DayName,1 RowNo, ''Date
 ,(CONVERT(NVARCHAR, 5 / 12) + ' Year ' + CONVERT(NVARCHAR, 5 % 12) +' Month') ,'' FromDate,'' ToDate,'' LeaveAvailId FROM ZASHRIS..tblLeaveInventory  where EmpMasterCode= @EmpMasterCode and LeaveYear=year(GETDATE()) and DayQty>0
     
END 

 


```

### sp_GET_FinancialYear

- **Parameters:** @CompanyId (nvarchar(max), IN)
- **Tables read:** ZAS_ACCDB..tblFinancialYear
- **Tables written:** none
- **Business logic:** Returns the list of financial years defined for a company in the accounting database, building a composite id string (`StartDate|EndDate`) alongside the year's description.
- **Transaction/error handling:** none
- **Result set:** FinancialYearId (formatted "StartDate|EndDate"), FinancialYear (description) for the given CompanyId.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\SalesDashboardDal.cs:128` via `GetDataTableAction("sp_GET_FinancialYear", ...)`. This DAL is instantiated directly (no BLL wrapper found) from pages such as `Solution.Web\SInventory_UI\Dashboard.aspx.cs`.

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_FinancialYear] 

	@CompanyId NVARCHAR(MAX)

AS
BEGIN
	

	  SELECT CONVERT(VARCHAR(10), StartDate, 111) + '|' + CONVERT(VARCHAR(10), EndDate, 111) FinancialYearId,FinancialYearDesc AS FinancialYear 
	  FROM ZAS_ACCDB..tblFinancialYear WHERE CompanyId = @CompanyId
	  

	  --SELECT * FROM ZAS_ACCDB..tblFinancialYear
	  
	  --SELECT * FROM tblFinancialYear

END




```

### sp_GET_FinancialYearDate

- **Parameters:** @CompanyId (nvarchar(max), IN), @FinYearId (nvarchar(max), IN)
- **Tables read:** ZAS_ACCDB..tblFinancialYear
- **Tables written:** none
- **Business logic:** Returns the start/end date range for a specific financial year of a specific company.
- **Transaction/error handling:** none
- **Result set:** StartDate, EndDate, FinancialYearId for the matching company/financial-year combination.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\SalesDashboardDal.cs:180` via `GetDataTableAction("sp_GET_FinancialYearDate", ...)`, used from the Dashboard/SalesDashboard flow.

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_FinancialYearDate] 

	@CompanyId NVARCHAR(MAX),
	@FinYearId NVARCHAR(MAX)

AS
BEGIN
	

	  SELECT StartDate,EndDate,FinancialYearId
	  FROM ZAS_ACCDB..tblFinancialYear WHERE CompanyId = @CompanyId AND FinancialYearId=@FinYearId
	   

	  --SELECT * FROM ZAS_ACCDB..tblFinancialYear
	  
	  --SELECT * FROM tblFinancialYear

END





```

### sp_GET_FinancialYearWithId

- **Parameters:** @CompanyId (nvarchar(max), IN)
- **Tables read:** ZAS_ACCDB..tblFinancialYear
- **Tables written:** none
- **Business logic:** Returns the raw FinancialYearId (not the composite date-range string used by `sp_GET_FinancialYear`) and description for all financial years of a company — likely feeds a dropdown that needs the underlying id for further lookups.
- **Transaction/error handling:** none
- **Result set:** FinancialYearId, FinancialYear (description) for the given CompanyId.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\SalesDashboardDal.cs:172` via `GetDataTableAction("sp_GET_FinancialYearWithId", ...)`.

```sql


-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_FinancialYearWithId] 

	@CompanyId NVARCHAR(MAX)

AS
BEGIN
	

	  SELECT FinancialYearId,FinancialYearDesc AS FinancialYear 
	  FROM ZAS_ACCDB..tblFinancialYear WHERE CompanyId = @CompanyId
	  

	  --SELECT * FROM ZAS_ACCDB..tblFinancialYear
	  
	  --SELECT * FROM tblFinancialYear

END





```

### sp_GET_LeaveList_HRIS_API

- **Parameters:** @EmployeeId (int, IN), @FromDate (nvarchar(max), IN, default null), @ToDate (nvarchar(max), IN, default null), @Role (nvarchar(max), IN, default null)
- **Tables read:** tblLeaveAvail, tblEmpGeneralInfo, tblDesignation
- **Tables written:** none
- **Business logic:** Returns leave-availment requests for an employee (or requests where the employee is the approving "NextEmpId"), branching on `@Role`: when `@Role='MIO'` and otherwise the same query is run (both branches are effectively identical), filtering to rows where `NextEmpId` is not null — i.e. only surfaces leave requests that are still pending/routed to a next approver.
- **Transaction/error handling:** none
- **Result set:** NextEmpId, LeaveAvailId, EmpMasterCode, EmpName, Designation, LeaveInventoryId, FromDate/ToDate (formatted), EmpInfoId, AvailLeaveQty, LeaveReason, LeaveName, EntryBy, Status, EntryDate, IsActive, ActionStatus, ApprovalStatus.
- **Caller(s):** UNUSED — no application caller found.

```sql

CREATE PROCEDURE [dbo].[sp_GET_LeaveList_HRIS_API] 
	
	@EmployeeId INT,
	@FromDate Nvarchar(MAX)=null,
	@ToDate Nvarchar(MAX)=null,
	@Role Nvarchar(MAX)=null
	
AS
BEGIN
	if(@Role='MIO')
	begin
 select NextEmpId, LeaveAvailId ,
   mas.EmpMasterCode ,
   mas.EmpName ,
   dgs.DesigName Designation,
  LeaveInventoryId ,
   format(FromDate,'dd-MMM-yyyy') FromDate  ,
     format(ToDate,'dd-MMM-yyyy') ToDate,
   mas.EmpInfoId ,
 CONVERT(int,AvailLeaveQty)  AvailLeaveQty,
  LeaveReason ,
  LeaveName  ,
 mas.EntryBy,
 Status ,
 mas.EntryDate ,
  IsActive ,
   CONVERT(int,ActionStatus) ActionStatus, Status ApprovalStatus
 from  tblLeaveAvail  mas
 left join tblEmpGeneralInfo emp  on emp.EmpInfoId=mas.EmpInfoId
 left join tblDesignation dgs on dgs.DesignationId=emp.DesignationId
 where (mas.EmpInfoId=@EmployeeId or NextEmpId=@EmployeeId) and NextEmpId is not null

 end

 else
 begin

 	 
 select NextEmpId, LeaveAvailId ,
   mas.EmpMasterCode ,
   mas.EmpName ,
   dgs.DesigName Designation,
  LeaveInventoryId ,
   format(FromDate,'dd-MMM-yyyy') FromDate  ,
     format(ToDate,'dd-MMM-yyyy') ToDate,
   mas.EmpInfoId ,
 CONVERT(int,AvailLeaveQty)  AvailLeaveQty,
  LeaveReason ,
  LeaveName  ,
 mas.EntryBy,
 Status ,
 mas.EntryDate ,
  IsActive ,
   CONVERT(int,ActionStatus) ActionStatus, Status ApprovalStatus
 from  tblLeaveAvail  mas
 left join tblEmpGeneralInfo emp  on emp.EmpInfoId=mas.EmpInfoId
 left join tblDesignation dgs on dgs.DesignationId=emp.DesignationId where     (mas.EmpInfoId=@EmployeeId or NextEmpId=@EmployeeId) and NextEmpId is not null

 end
 
  
END






```

### sp_GET_LeaveNameDropdown_HRIS_API

- **Parameters:** @EmployeeId (int, IN), @FromDate (nvarchar(max), IN), @ToDate (nvarchar(max), IN)
- **Tables read:** tblEmpGeneralInfo, ZASHRIS..tblLeaveInventory
- **Tables written:** none
- **Business logic:** Resolves an employee's master code, then returns the list of leave types with remaining balance (`DayQty>0`) for the year of `@FromDate`, for use in a leave-name dropdown.
- **Transaction/error handling:** none
- **Result set:** LeaveInventoryId, LeaveName ordered by LeaveName.
- **Caller(s):** UNUSED — no application caller found.

```sql

CREATE PROCEDURE [dbo].[sp_GET_LeaveNameDropdown_HRIS_API] 
	
	@EmployeeId INT,
	@FromDate Nvarchar(MAX),
	@ToDate Nvarchar(MAX)
	
AS
BEGIN


   DECLARE @EmpMasterCode Nvarchar(MAX)

	SELECT @EmpMasterCode=EmpMasterCode FROM tblEmpGeneralInfo WHERE EmpInfoId=@EmployeeId  
	
  SELECT   LeaveInventoryId, LeaveName    FROM ZASHRIS..tblLeaveInventory WHERE EmpMasterCode=@EmpMasterCode 
  and LeaveYear = YEAR(@FromDate)  and DayQty>0
  ORDER BY LeaveName

END






```

### sp_GET_MainMenuRole2

- **Parameters:** @RoleId (int, IN), @TypeId (int, IN)
- **Tables read:** tblMainMenuNew, tblMenuRole
- **Tables written:** none
- **Business logic:** Builds the menu-permission tree for a given role/menu-type by UNIONing (a) all menu items of that type not yet assigned to the role, with default permission flags of 0, and (b) menu items already assigned to the role together with the role's actual Add/View/Delete/Edit/Permission flags, then aggregates via SUM/GROUP BY so each menu item appears once with its effective (possibly zero) permission flags. Used to render a role's menu/permission matrix for editing.
- **Transaction/error handling:** none
- **Result set:** One row per menu item: SL, ManuName, URL, ParantId, Add/View/Delete/Edit (menu-level defaults), ParentName, and the role-specific RoleId, RAdd, RView, RDelete, REdit, Permission (summed/aggregated), ordered by ParantId.
- **Caller(s):** UNUSED — no application caller found.

```sql
CREATE PROCEDURE [dbo].[sp_GET_MainMenuRole2]
	-- Add the parameters for the stored procedure here
   @RoleId INT,@TypeId int

AS
    BEGIN
	---RoleId,tblMenuRole.[Add] AS RAdd,tblMenuRole.[View] AS RView,tblMenuRole.[Delete] AS RDelete,tblMenuRole.Edit AS REdit
	SELECT tblt.SL,
           tblt.ManuName,
           tblt.URL,
           tblt.ParantId,
           ISNULL(tblt.[Add],1)[Add],
           ISNULL(tblt.[View],1)[View],
           ISNULL(tblt.[Delete],1)[Delete],
           ISNULL(tblt.Edit,1)Edit,
           tblt.ParentName,
           SUM(CONVERT(INT,ISNULL(tblt.RoleId,0)))RoleId,
           SUM(CONVERT(INT,ISNULL(tblt.RAdd,0)))RAdd,
           SUM(CONVERT(INT,ISNULL(tblt.RView,0)))RView,
           SUM(CONVERT(INT,ISNULL(tblt.RDelete,0)))RDelete,
           SUM(CONVERT(INT,ISNULL(tblt.REdit,0)))REdit,SUM(CONVERT(INT,ISNULL(tblt.Permission,0)))Permission FROM (SELECT M.SL,M.ManuName,M.URL,M.ParantId,M.[Add],M.[View],M.[Delete],M.Edit,P.ManuName AS ParentName,
	'0'RoleId,'0' AS RAdd,'0' AS RView,'0' AS RDelete,'0' AS REdit,'0' AS  Permission
	 FROM dbo.tblMainMenuNew M
	LEFT JOIN dbo.tblMainMenuNew P ON M.ParantId=P.SL
	
	WHERE M.URL<>'#' and M.SL not in (select SL from tblMenuRole where RoleId=@RoleId) and M.TypeId=@TypeId

	UNION ALL 

	SELECT M.SL,M.ManuName,M.URL,M.ParantId,M.[Add],M.[View],M.[Delete],M.Edit,P.ManuName AS ParentName,
	RoleId,tblMenuRole.[Add] AS RAdd,tblMenuRole.[View] AS RView,tblMenuRole.[Delete] AS RDelete,tblMenuRole.Edit AS REdit
	,tblMenuRole.Permission FROM dbo.tblMenuRole
	LEFT JOIN dbo.tblMainMenuNew M ON M.SL = tblMenuRole.SL
	LEFT JOIN dbo.tblMainMenuNew P ON M.ParantId=P.SL
	WHERE RoleId=@RoleId and M.TypeId in(@TypeId)) AS tblt
	GROUP BY tblt.SL,
           tblt.ManuName,
           tblt.URL,
           tblt.ParantId,
           tblt.[Add],
           tblt.[View],
           tblt.[Delete],
           tblt.Edit,
           tblt.ParentName ORDER BY CONVERT(INT,tblt.ParantId) ASC


			
    END


```

### sp_GET_MaterialConversionPendingList

- **Parameters:** @Param (nvarchar(max), IN) — appended as a raw SQL fragment (extra WHERE conditions)
- **Tables read:** CSTL_ERPDB..tbl_MC_MaterialConversionMaster, CSTL_ERPDB..tbl_MC_ProductionUnit, CSTL_ERPDB..tbl_MC_RecipeMaster, CSTL_ERPDB..tblCompanyInfo, tblCentralStore, tblProduct, tblPackSize
- **Tables written:** none
- **Business logic:** Lists material-conversion (batch production) master records that have not yet been received into the central store (`tblCentralStore.MaterialConversionMasterId IS NULL`), joining in company, production unit, recipe, and finished-good product details. `@Param` is concatenated directly into the dynamic SQL as additional filter criteria, then executed with `EXEC(@Query)`.
- **Transaction/error handling:** none
- **Result set:** MaterialConversionMasterId, RecipeSetupCode, MaterialConversionCode, MaterialConversionDate, CompanyName, ProductionUnitName, FGItemId, ProductName, PackSize, AssessedOutputQuantity, FinalOutputQuantity, LossQuantity, TotalCostOfThisBatch, CostPerUnit — ordered by MaterialConversionMasterId descending.
- **Caller(s):** UNUSED — no application caller found.

```sql

-- Create the stored procedure for getting a single employee record
CREATE PROCEDURE [dbo].[sp_GET_MaterialConversionPendingList]

    @Param NVARCHAR(MAX)

AS
BEGIN


	DECLARE @Query NVARCHAR(MAX) = ''

	SET @Query = 'SELECT CM.MaterialConversionMasterId,RecipeSetupCode,CM.MaterialConversionCode,FORMAT(CM.MaterialConversionDate, ''dd-MMM-yyyy'') MaterialConversionDate,
	CI.CompanyName,PUNT.ProductionUnitName,CM.FGItemId,FG.ProductName,FG.PackSize,CM.AssessedOutputQuantity,FinalOutputQuantity,ISNULL(CM.LossQuantity,0) LossQuantity,CM.TotalCostOfThisBatch,
	CM.CostPerUnit FROM CSTL_ERPDB..tbl_MC_MaterialConversionMaster AS CM
	LEFT JOIN CSTL_ERPDB..tbl_MC_ProductionUnit AS PUNT ON CM.ProductionUnitId = PUNT.ProductionUnitId
	INNER JOIN CSTL_ERPDB..tbl_MC_RecipeMaster AS RCM ON CM.RecipeSetupMasterId = RCM.RecipeSetupMasterId
	LEFT JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON CM.BusinessUnitId = CI.CompanyId
	LEFT JOIN tblCentralStore AS CS ON CM.MaterialConversionMasterId = CS.MaterialConversionMasterId
	LEFT JOIN tblProduct AS FG ON CM.FGItemId = FG.ProductId
	LEFT JOIN tblPackSize AS PKS ON FG.PackSizeId = PKS.PackSizeId
	WHERE CS.MaterialConversionMasterId IS NULL' + @Param + ' ORDER BY CM.MaterialConversionMasterId DESC'

	EXEC (@Query)

END;




```

### sp_GET_ProgramTypeListParm

- **Parameters:** @Parm (nvarchar(max), IN) — appended as a raw SQL fragment
- **Tables read:** tblProgramType
- **Tables written:** none
- **Business logic:** Returns active program-type master records, with `@Parm` concatenated onto the base query and executed dynamically (e.g. to add extra filters/ordering from the caller).
- **Transaction/error handling:** none
- **Result set:** All columns (`SELECT *`) from `tblProgramType` where IsActive=1, plus whatever `@Parm` adds.
- **Caller(s):** UNUSED — no application caller found.

```sql

create PROCEDURE [dbo].[sp_GET_ProgramTypeListParm]
	-- Add the parameters for the stored procedure here
  
  @Parm nvarchar(max)
AS
    BEGIN

 DECLARE @Q NVARCHAR(MAX)='	SELECT *
	FROM dbo.tblProgramType AS GRP WITH (NOLOCK) where IsActive=1  '  +@Parm 

	EXEC sp_executesql @Q
 END


```

### sp_GET_SMCTypeListParm

- **Parameters:** @Parm (nvarchar(max), IN) — appended as a raw SQL fragment
- **Tables read:** tblSMCType
- **Tables written:** none
- **Business logic:** Returns active SMC-type master records, with `@Parm` concatenated onto the base query and executed dynamically.
- **Transaction/error handling:** none
- **Result set:** All columns (`SELECT *`) from `tblSMCType` where IsActive=1, plus whatever `@Parm` adds.
- **Caller(s):** UNUSED — no application caller found.

```sql

create PROCEDURE [dbo].[sp_GET_SMCTypeListParm]
	-- Add the parameters for the stored procedure here
  
  @Parm nvarchar(max)
AS
    BEGIN

 DECLARE @Q NVARCHAR(MAX)='	SELECT *
	FROM dbo.tblSMCType AS GRP WITH (NOLOCK) where IsActive=1  '  +@Parm 

	EXEC sp_executesql @Q
 END


```

### sp_GET_ShiftListDDL

- **Parameters:** none
- **Tables read:** tbl_Shift
- **Tables written:** none
- **Business logic:** Returns active shift definitions for a shift-selection dropdown.
- **Transaction/error handling:** none
- **Result set:** ShiftId, ShiftName (aliased from ShiftText) for active shifts.
- **Caller(s):** UNUSED — no application caller found.

```sql
create PROCEDURE [dbo].[sp_GET_ShiftListDDL]
	-- Add the parameters for the stored procedure here
 

AS
    BEGIN

	SELECT ShiftId, ShiftText ShiftName
	FROM dbo.tbl_Shift AS GRP WITH (NOLOCK) where IsActive=1 


 END


```

### sp_GET_StationTypeList

- **Parameters:** none
- **Tables read:** tblStationType
- **Tables written:** none
- **Business logic:** Returns active station-type master records for a dropdown/list.
- **Transaction/error handling:** none
- **Result set:** All columns (`SELECT *`) from `tblStationType` where IsActive=1.
- **Caller(s):** UNUSED — no application caller found.

```sql

create PROCEDURE [dbo].[sp_GET_StationTypeList]
	-- Add the parameters for the stored procedure here
  

AS
    BEGIN

	SELECT *
	FROM dbo.tblStationType AS GRP WITH (NOLOCK) where IsActive=1 


 END


```

### sp_GET_StockValueForAcc

- **Parameters:** @CompanyId (int, IN), @FromDate (datetime, IN), @ToDate (datetime, IN)
- **Tables read:** tblProduct, tblDCStore_OpeningBalance, tblCentralStore, tblDCStore, tblInvoiceDetail, tblInvoice, tblChalanDetail, tblChalanInfo, tblDCStoreFreeze, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation, tblStockInTransfar, tblCompanyInfo, tblUnitPrice, tblCentralStore_OpeninigBalance, tblRequisition, tblWhStoreFreeze
- **Tables written:** none
- **Business logic:** Computes a single aggregated stock-value figure per company for accounting purposes: it reconciles depot-level opening balance, stock receipts (direct and via challan), sales, bonus, samples, and in-transit movements to derive a net stock value delta, then adds a separately-computed warehouse-level stock value (based on opening balance, central-store receipts, Dhaka transfers, in-transit, and freeze quantities) to produce a combined `StockValue`.
- **Transaction/error handling:** none
- **Result set:** A single row: StockValue (bigint-cast combined depot + warehouse stock value) for the given CompanyId/date range.
- **Caller(s):** UNUSED — no application caller found.

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_StockValueForAcc] 

@CompanyId int,  
       @FromDate datetime,  
	   @ToDate datetime

AS
BEGIN
	

	  

SELECT  SUM(CAST(CASE WHEN (ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+
	ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0)) > 
	(ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)) 
	THEN ((ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ 
	ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0))
	-(ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)))
	 ELSE ((ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)) 
	 - (ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0)
	  + ISNULL(vTblsalesInTransit.Value,0))) END AS BIGINT)) + CAST(W.StockValue AS bigint) as StockValue
	  FROM dbo.tblProduct P with(nolock)  
	  
	  LEFT JOIN (SELECT tblDCStore_OpeningBalance.ProductCode,SUM(StockQty)Quantity,  
	  SUM(((CTRS.UnitPrice*tblDCStore_OpeningBalance.StockQty) + (CTRS.VATPerUnit*tblDCStore_OpeningBalance.StockQty))) AS Value
	  FROM dbo.tblDCStore_OpeningBalance  with(nolock)  
	  INNER JOIN tblCentralStore AS CTRS ON tblDCStore_OpeningBalance.TempReceiveId = CTRS.ReceiveId  
	  WHERE  DCOpeningBalanceDate=@FromDate 
	  GROUP BY tblDCStore_OpeningBalance.ProductCode) vTblOB ON vTblOB.ProductCode = P.ProductCode  
	   
	   LEFT JOIN (SELECT DCS.ProductCode,SUM(TotalQuantity)TotalStockReceiveQty,  SUM(((CTRS.UnitPrice*DCS.StockQty) 
	   + (CTRS.VATPerUnit*DCS.StockQty))) AS Value FROM dbo.tblDCStore AS DCS with(nolock)  
	   INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId WHERE  ChalanDetailsId IS  NULL AND StockRcvDate between @FromDate and @ToDate 
	   GROUP BY DCS.ProductCode) vTblStoReceive ON vTblStoReceive.ProductCode = P.ProductCode  
	    
	   LEFT JOIN (SELECT DCS.ProductCode,SUM(TotalQuantity)TotalStockReceiveQty,  
	   SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty))) AS Value FROM dbo.tblDCStore AS DCS with(nolock)  
	   INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId   WHERE ChalanDetailsId IS NOT NULL AND StockRcvDate between @FromDate and @ToDate 
	   GROUP BY DCS.ProductCode) vTblChallanReceive ON vTblChallanReceive.ProductCode = P.ProductCode 
	    
	   LEFT JOIN (SELECT DCS.ProductCode,SUM(TotalQuantity)TotalStockReceiveQty,  
	   SUM(((CTRS.UnitPrice*TotalQuantity) + (CTRS.VATPerUnit*TotalQuantity) )) AS Value FROM dbo.tblDCStore DCS  with(nolock)   
	   INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId 
	   WHERE StockRcvDate between @FromDate and @ToDate 
	   GROUP BY DCS.ProductCode)  vTblStockReceive ON vTblStockReceive.ProductCode = P.ProductCode
	      
	   LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(DeliveryQuantity)Sales,  
	   SUM(((CTRS.UnitPrice*DeliveryQuantity) + (CTRS.VATPerUnit*DeliveryQuantity) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock)  
	   INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId  
	   INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId  
	   INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId    
	   WHERE DeliveryStatus IN ('Full','Partial') AND  
	   InvoiceDate between @FromDate and @ToDate 
	   GROUP BY tblInvoiceDetail.ProductCode)  vTblsales ON vTblsales.ProductCode = P.ProductCode   
	   
	   LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(BonusQuantity) BonusQty,  
	   SUM(((CTRS.UnitPrice*BonusQuantity) + (CTRS.VATPerUnit*BonusQuantity) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock)  
	   INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId  
	   INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId  
	   INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
	   WHERE DeliveryStatus IN (NULL,'Full','Partial') AND  
	   InvoiceDate between @FromDate and @ToDate 
	   GROUP BY tblInvoiceDetail.ProductCode)  AS vbBonus ON vbBonus.ProductCode = P.ProductCode    
	   
	   LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(DeliveryQuantity)Sales, SUM(((CTRS.UnitPrice*DeliveryQuantity) + 
	   (CTRS.VATPerUnit*DeliveryQuantity) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock)  
	   INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId  
	   INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId  
	   INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId  
	    WHERE OrderDetailsId=0 and InvoiceDate 
		between @FromDate and @ToDate GROUP BY tblInvoiceDetail.ProductCode) 
		vTblProductBonus ON vTblProductBonus.ProductCode = P.ProductCode    
		
		LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(tblInvoiceDetail.TotalQuantity)Sales,  
		SUM(((CTRS.UnitPrice*tblInvoiceDetail.TotalQuantity) + (CTRS.VATPerUnit*tblInvoiceDetail.TotalQuantity) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock)  
		INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId    
		INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId  
		INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId  WHERE DeliveryStatus IS NULL and InvoiceDate between @FromDate and @ToDate 
		GROUP BY tblInvoiceDetail.ProductCode)  vTblsalesInTransit ON vTblsalesInTransit.ProductCode = P.ProductCode  

		LEFT JOIN (SELECT tblChalanDetail.ProductCode,SUM(tblChalanDetail.Quantity)Challan,  
		SUM(((CTRS.UnitPrice*tblChalanDetail.Quantity) + (CTRS.VATPerUnit*tblChalanDetail.Quantity) )) AS Value FROM dbo.tblChalanDetail  with(nolock)  
		INNER JOIN dbo.tblChalanInfo ON tblChalanDetail.ChalanId=tblChalanInfo.ChalanId  
		INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblChalanDetail.DCStoreId  
		INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId  WHERE  
		tblChalanInfo.ChalanDate between @FromDate and @ToDate AND IsDeliver = 'True'
		 GROUP BY tblChalanDetail.ProductCode) vTblChallan ON vTblChallan.ProductCode = P.ProductCode   

		 LEFT JOIN (SELECT tblChalanDetail.ProductCode,SUM(tblChalanDetail.Quantity)Challan,
		 SUM(((CTRS.UnitPrice*tblChalanDetail.Quantity) + (CTRS.VATPerUnit*tblChalanDetail.Quantity) )) AS Value FROM dbo.tblChalanDetail  with(nolock)  
		 INNER JOIN dbo.tblChalanInfo ON tblChalanDetail.ChalanId=tblChalanInfo.ChalanId  
		 INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblChalanDetail.DCStoreId  
		 INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId   
		 WHERE  tblChalanInfo.ChalanDate between @FromDate and @ToDate 
		 AND IsDeliver = 'False' GROUP BY tblChalanDetail.ProductCode) vTblChallanTransit ON vTblChallanTransit.ProductCode = P.ProductCode  
		 
		 LEFT JOIN (SELECT tblDCStoreFreeze.ProductCode,(SUM(tblDCStoreFreeze.StockQty)+SUM(tblDCStoreFreeze.DamageQty))Freeze,  
		 SUM(((CTRS.UnitPrice*tblDCStoreFreeze.StockQty) + (CTRS.VATPerUnit*tblDCStoreFreeze.StockQty) )) 
		 + SUM(((CTRS.UnitPrice*tblDCStoreFreeze.DamageQty) + (CTRS.VATPerUnit*tblDCStoreFreeze.DamageQty) ))  AS Value FROM dbo.tblDCStoreFreeze  with(nolock)   
		 INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblDCStoreFreeze.DCStoreId  
		 INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId     
		 WHERE tblDCStoreFreeze.[StockCondition]='Restricted' and 
		 tblDCStoreFreeze.StockRcvDate between @FromDate and @ToDate 
		 GROUP BY tblDCStoreFreeze.ProductCode) vTblFreez ON vTblFreez.ProductCode = P.ProductCode
		    
		 LEFT JOIN (SELECT tblDCStoreFreeze.ProductCode,(SUM(tblDCStoreFreeze.StockQty)+SUM(tblDCStoreFreeze.DamageQty))Freeze,  
		 SUM(((CTRS.UnitPrice*tblDCStoreFreeze.StockQty) + (CTRS.VATPerUnit*tblDCStoreFreeze.StockQty) )) 
		 + SUM(((CTRS.UnitPrice*tblDCStoreFreeze.DamageQty) + (CTRS.VATPerUnit*tblDCStoreFreeze.DamageQty) )) AS Value FROM dbo.tblDCStoreFreeze  with(nolock)  
		 INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblDCStoreFreeze.DCStoreId  
		 INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId    
		 WHERE  tblDCStoreFreeze.[StockCondition]='Blocked' 
		 and tblDCStoreFreeze.StockRcvDate between @FromDate and @ToDate 
		 GROUP BY tblDCStoreFreeze.ProductCode) vTblFreez2 ON vTblFreez2.ProductCode = P.ProductCode  

		 LEFT JOIN (SELECT SMPD.ProductCode,SUM(ST.Quantity*-1) SampleIssue,
		 SUM(((CTRS.UnitPrice*SMPD.Quantity) + (CTRS.VATPerUnit*SMPD.Quantity) )) AS Value FROm tblSampleIssue AS SMPI  
		 LEFT JOIN tblSampleIssueDetail AS SMPD ON SMPI.OrderId = SMPD.OrderId  
		 LEFT JOIN dbo.tblSampleIssueTranscation AS ST ON ST.IssueDetailId = SMPD.OrderDetailId  
		 INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = ST.DCStoreId  
		 INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
		 WHERE ActionStatus IN ('Accepted')  AND ApprovedDate BETWEEN @FromDate and @ToDate 
		  GROUP BY SMPD.ProductCode)  vTblSample ON vTblSample.ProductCode = P.ProductCode  

		 LEFT JOIN (SELECT sum(StockQty)Closingstock,DCS.ProductCode,
		 SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblDCStore AS DCS  with(nolock)  
		 INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId  
		 INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId   
		 group by DCS.ProductCode) currentStock ON currentStock.ProductCode = P.ProductCode 
		 LEFT JOIN (SELECT PD.CompanyId,sum(((ISNULL(CSOP.OpeningStockValue,0)) + (ISNULL(CSR.ReceiveStockValue,0)) - (ISNULL(DHK.DhkStockValue,0) + ISNULL(TNS.TnsStockValue,0)+ISNULL(WFZ.WfStockValue,0)))) StockValue  FROM tblProduct  AS PD 
LEFT JOIN tblCompanyInfo AS CI ON CI.CompanyId = PD.CompanyId
LEFT JOIN tblUnitPrice AS UP ON UP.ProductId = PD.ProductId 
LEFT JOIN (
SELECT OP.ProductCode,SUM(OP.Quantity) AS OpeningStock,SUM(OP.Quantity*OP.UnitPrice) AS OpeningStockValue FROM tblCentralStore_OpeninigBalance OP 
WHERE OP.CSOpeninigBalanceDate BETWEEN @FromDate and @ToDate  GROUP BY OP.ProductCode ) AS CSOP ON PD.ProductCode = CSOP.ProductCode 
LEFT JOIN (
SELECT ProductCode,SUM(StockInQty) AS StockInQuantity,SUM(StockInQty*UnitPrice) AS ReceiveStockValue FROM tblCentralStore WHERE ReceiveDate BETWEEN @FromDate and @ToDate 
GROUP BY ProductCode) AS CSR ON PD.ProductCode = CSR.ProductCode 
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Dhaka,SUM(ST.Quantity*CS.UnitPrice) AS DhkStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE  ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate and @ToDate 
GROUP BY ST.ProductCode) AS DHK ON PD.ProductCode = DHK.ProductCode
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Transit,SUM(ST.Quantity*CS.UnitPrice) AS TnsStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE  (ReceiveIssue IS NULL OR ReceiveIssue ='') AND RQ.ReqDate BETWEEN @FromDate and @ToDate 
GROUP BY ST.ProductCode) AS TNS ON PD.ProductCode = TNS.ProductCode
LEFT JOIN (
SELECT P.ProductCode,SUM(WF.StockQty) AS Freeze,SUM(WF.StockQty*CS.UnitPrice) AS WfStockValue FROM tblWhStoreFreeze AS WF
LEFT JOIN tblProduct AS P ON P.ProductId = WF.ProductId
LEFT JOIN tblCentralStore AS CS ON WF.ReceiveId = CS.ReceiveId
 WHERE WF.ReceiveDate BETWEEN @FromDate and @ToDate  GROUP BY P.ProductCode) AS WFZ ON PD.ProductCode = WFZ.ProductCode
WHERE UP.IsActive = 1 group by PD.CompanyId) AS W ON P.CompanyId = W.CompanyId
WHERE P.CompanyId = @CompanyId
GROUP BY P.CompanyId,W.StockValue


END



--SELECT LEFT(DATENAME(WEEKDAY,'2020-09-1 00:00:00.000'),3) 





```

### sp_GET_UnitWiseSalesCollectionDue_Report_ZAS_Api

- **Parameters:** @Year (int, IN), @Month (int, IN)
- **Tables read:** tblInvoice, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblDebitCreditVoucherDetail, tblCustPayDetail, tblCustomerPay, tblCustMaster, tblCompanyUnit, tblCompanyInfo, tblMarket, tblTerritory, tblArea
- **Tables written:** none
- **Business logic:** For a given year/month, computes per-customer receivables, collections (payment + AIT + discount), and sales-return amounts for delivered invoices, keeping only customers with an outstanding due greater than 1; then rolls those customer-level rows up to a company-level summary of Receivable/Collection/Due for a monthly sales-collection-due report exposed via the ZAS API.
- **Transaction/error handling:** none (has `SET NOCOUNT ON` only)
- **Result set:** CompanyName, Receiveable, Collection, Due, UptoDate — one row per company, ordered by CompanyName.
- **Caller(s):** UNUSED — no application caller found (name suggests it backs an external API integration not wired into this codebase, or is called by a separate API project not included here).

```sql

CREATE PROCEDURE [dbo].[sp_GET_UnitWiseSalesCollectionDue_Report_ZAS_Api]
    @Year INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FromDate DATE = DATEFROMPARTS(@Year, @Month, 1);
DECLARE @ToDate DATE = EOMONTH(@FromDate);

WITH BaseData AS (
    SELECT 
        CSTMR.CustomerMasterId,
        CI.CompanyName,
        UNT.ComUnitName,
        CSTMR.CustomerCode,
        CSTMR.CustomerName,
        SUM(ISNULL(DeliveryTpGrandTotal,0)) AS Receiveable,
        SUM(ISNULL(PMNT.PaymentAmount,0)) AS Collection,
        SUM(ISNULL(PMNT.AIT,0)) AS AIT,
        SUM(ISNULL(PMNT.Discount,0)) AS Discount,
        SUM(ISNULL(DeliveryTpGrandTotal,0)) 
            - (SUM(ISNULL(PMNT.PaymentAmount,0)) 
            + SUM(ISNULL(PMNT.AIT,0)) 
            + SUM(ISNULL(PMNT.Discount,0))) 
            - SUM(ISNULL(ReturnAmount,0)) AS Due,
        @ToDate AS UptoDate

    FROM tblInvoice AS INV

    LEFT JOIN (
        SELECT 
            M.CommonPK_ID,
            SUM(DebitAmountBDT) AS ReturnAmount
        FROM CSTL_ACCDB..tblDebitCreditVoucherMaster AS M
        LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail AS D 
            ON M.VoucherMasterId = D.VoucherMasterId
        LEFT JOIN tblInvoice AS INV 
            ON M.CommonPK_ID = INV.InvoiceId
        WHERE ISNULL(M.IsInventoryValuation,0) = 0  
          AND M.TableID = 2018
        GROUP BY M.CommonPK_ID
    ) AS RTN 
        ON INV.InvoiceId = RTN.CommonPK_ID

    LEFT JOIN (
        SELECT 
            InvoiceId,
            SUM(CPD.PaymentAmount) AS PaymentAmount,
            SUM(CPD.AIT) AS AIT,
            SUM(CPD.Discount) AS Discount
        FROM tblCustPayDetail AS CPD
        LEFT JOIN tblCustomerPay AS CP 
            ON CPD.CustPayId = CP.CustPayId
        WHERE InvoiceId IS NOT NULL 
          AND PaymentDate <= @ToDate
        GROUP BY InvoiceId
    ) AS PMNT 
        ON PMNT.InvoiceId = INV.InvoiceId

    LEFT JOIN dbo.tblCustMaster AS CSTMR 
        ON CSTMR.CustomerMasterId = INV.CustomerMasterId
    LEFT JOIN dbo.tblCompanyUnit AS UNT 
        ON INV.ComUnitId = UNT.ComUnitId
    LEFT JOIN dbo.tblCompanyInfo AS CI 
        ON UNT.CompanyId = CI.CompanyId
    LEFT JOIN dbo.tblMarket AS MKT 
        ON CSTMR.MarketId = MKT.MarketId
    LEFT JOIN dbo.tblTerritory AS TTR 
        ON MKT.TerritoryId = TTR.TerritoryId
    LEFT JOIN dbo.tblArea AS ARA 
        ON TTR.AreaId = ARA.AreaId

    WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial')
      AND INV.InvoiceDate >= @FromDate
      AND INV.InvoiceDate <= @ToDate

    GROUP BY 
        CSTMR.CustomerMasterId,
        CI.CompanyName,
        UNT.ComUnitName,
        CSTMR.CustomerCode,
        CSTMR.CustomerName

    HAVING 
        SUM(ISNULL(DeliveryTpGrandTotal,0)) 
        - (SUM(ISNULL(PMNT.PaymentAmount,0)) 
        + SUM(ISNULL(PMNT.AIT,0)) 
        + SUM(ISNULL(PMNT.Discount,0))) 
        - SUM(ISNULL(ReturnAmount,0)) > 1
)

SELECT 
    CompanyName,
    SUM(Receiveable) AS Receiveable,
    SUM(Collection + AIT + Discount) AS Collection,
    SUM(Due) AS Due,
    MAX(UptoDate) AS UptoDate
FROM BaseData
GROUP BY CompanyName
ORDER BY CompanyName;
END





```

### sp_GET_UserList

- **Parameters:** @Parameter (nvarchar(max), IN) — appended as a raw SQL fragment
- **Tables read:** tblUser, tbl_UserRoleInfo, tblUserType, tblEmpGeneralInfo, tblCompanyInfo
- **Tables written:** none
- **Business logic:** Returns the application user list (excluding UserId 1, presumably a system/admin seed account) with joined company, employee name, role name, and user type, executed dynamically with `@Parameter` appended for further filtering.
- **Transaction/error handling:** none
- **Result set:** CompanyName, EmpName, RoleName, UserType, UserStatus, a blank ActiveInActiveDate placeholder, plus all columns (`*`) from tblUser and joined tables.
- **Caller(s):** UNUSED — no application caller found.

```sql
CREATE PROCEDURE [dbo].[sp_GET_UserList]
	
	-- Add the parameters for the stored procedure here
	@Parameter NVARCHAR(MAX)

AS
BEGIN
   

   DECLARE @Query NVARCHAR(MAX)

   SET @Query = 'SELECT  com.CompanyName,   emp.EmpName,uRR.RoleName,  uType.UserType UserType, UR.UserStatus,'''' AS ActiveInActiveDate,*   
  FROM tblUser AS UR with (nolock)
   

  left join tbl_UserRoleInfo uRR  with (nolock) on uRR.UserRoleID=UR.UserRoleID
   left join tblUserType uType  with (nolock) on uType.UserTypeId=UR.UserTypeId


     left join tblEmpGeneralInfo emp  with (nolock) on UR.EmpInfoId=emp.EmpInfoId
	     left join tblCompanyInfo com  with (nolock) on com.CompanyId=emp.CompanyId

  WHERE UR.UserId IS NOT NULL   and UR.UserId not in (1)  ' + @Parameter


   --SELECT GP.GroupCode + ':'+ GP.GroupName AS GroupName FROM tbl_Group AS GP
   --SELECT DISTINCT ASMId,COUNT(ASMId) NoOf FROM tblOrder AS INV GROUP BY ASMId
   

   EXEC(@Query)

 
END


```

### sp_GET_UserMaster_ById

- **Parameters:** @id (nvarchar(max), IN)
- **Tables read:** tblUser, tblEmpGeneralInfo, tblUserCompanyUnit, tblTagASMRSMInfo
- **Tables written:** none
- **Business logic:** Returns a single user's master record by UserId, including a comma-concatenated list of the company units (depots) assigned to the user (`UserDCID`) and territory/area tags assigned to the user (`UserASMRSM`), both built via `STUFF`/`FOR XML PATH` string aggregation, plus the linked employee's info.
- **Transaction/error handling:** none
- **Result set:** EmpInfoId, CompanyId, UserDCID (concatenated CompanyUnitIds), UserASMRSM (concatenated TerritoryAreaIds), formatted ActiveInActiveDate, plus all columns (`*`) from tblUser/tblEmpGeneralInfo for the matching UserId.
- **Caller(s):** UNUSED — no application caller found.

```sql
 CREATE PROCEDURE [dbo].[sp_GET_UserMaster_ById]
	-- Add the parameters for the stored procedure here
   @id NVARCHAR(max)

AS
    BEGIN

	 SELECT emp.EmpInfoId, emp.CompanyId, STUFF( (SELECT CONCAT(',', brn.CompanyUnitId , '') FROM dbo.tblUserCompanyUnit brn(NOLOCK)  WHERE brn.UserId=tblUser.UserId ORDER BY brn.CompanyUnitId FOR XML PATH ('') ),1,1,'') AS UserDCID, STUFF( (SELECT CONCAT(',', tg.TerritoryAreaId , '') FROM dbo.tblTagASMRSMInfo tg(NOLOCK)  WHERE tg.UserId=tblUser.UserId  FOR XML PATH ('') ),1,1,'') AS UserASMRSM, FORMAT(ActiveInActiveDate, 'dd MMMM, yyyy')  ActiveInActiveDate,  * from tblUser
	 left join tblEmpGeneralInfo emp on  tblUser.EmpInfoId=emp.EmpInfoId

	  where UserId = @id
      
    END




```

### sp_GET_UserRoleDropDown

- **Parameters:** none
- **Tables read:** tbl_UserRoleInfo
- **Tables written:** none
- **Business logic:** Returns all user-role master records for a role-selection dropdown.
- **Transaction/error handling:** none
- **Result set:** All columns (`SELECT *`) from `tbl_UserRoleInfo`.
- **Caller(s):** UNUSED — no application caller found.

```sql



CREATE PROCEDURE [dbo].[sp_GET_UserRoleDropDown]
	-- Add the parameters for the stored procedure here
   

AS
    BEGIN

	
	select * from tbl_UserRoleInfo
						
    END

	







```

### sp_GET_UserRoleInfo_ById

- **Parameters:** @id (nvarchar(max), IN)
- **Tables read:** tbl_UserRoleInfo
- **Tables written:** none
- **Business logic:** Returns a single user-role master record by UserRoleID, with the ActiveInActiveDate formatted as a display string.
- **Transaction/error handling:** none
- **Result set:** ActiveDateStr (formatted date), plus all columns (`*`) from `tbl_UserRoleInfo` for the matching UserRoleID.
- **Caller(s):** UNUSED — no application caller found.

```sql


 CREATE PROCEDURE [dbo].[sp_GET_UserRoleInfo_ById]
	-- Add the parameters for the stored procedure here
   @id NVARCHAR(max)

AS
    BEGIN

	 Select FORMAT(ActiveInActiveDate,'dd-MMM-yyyy') ActiveDateStr, * from tbl_UserRoleInfo where UserRoleID = @id
      
    END




```

### sp_GET_UserRoleList

- **Parameters:** @Parameter (nvarchar(max), IN) — appended as a raw SQL fragment
- **Tables read:** tbl_UserRoleInfo, tblUser, tblRoleType
- **Tables written:** none
- **Business logic:** Returns the list of user roles with a computed `DeleteStatus` flag ("disabled" if any user currently references that role, otherwise blank) to prevent deleting roles still in use, and joins in the role type description. Executed dynamically with `@Parameter` appended for extra filtering.
- **Transaction/error handling:** none
- **Result set:** UserRoleID, RoleType, RoleName, IsActive, ActiveInActiveDate (formatted), DeleteStatus.
- **Caller(s):** UNUSED — no application caller found.

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GET_UserRoleList]
	
	-- Add the parameters for the stored procedure here
	@Parameter NVARCHAR(MAX)

AS
BEGIN
   

   DECLARE @Query NVARCHAR(MAX)

   SET @Query = 'SELECT UR.UserRoleID, rt.RoleType, UR.RoleName, UR.IsActive, CONVERT(NVARCHAR(50),UR.ActiveInActiveDate,106) AS ActiveInActiveDate,
  CASE WHEN ISNULL(C.NoOf,0) > 0 THEN ''disabled'' ELSE '''' END AS DeleteStatus  
  FROM tbl_UserRoleInfo AS UR
  LEFT JOIN (SELECT DISTINCT UserRoleID,COUNT(UserRoleID) NoOf FROM tblUser WHERE UserRoleID IS NOT NULL GROUP BY UserRoleID) AS C ON UR.UserRoleID = C.UserRoleID

  left join tblRoleType rt on ur.RoleTypeId=rt.RoleTypeId

  WHERE UR.UserRoleID IS NOT NULL ' + @Parameter


   --SELECT GP.GroupCode + ':'+ GP.GroupName AS GroupName FROM tbl_Group AS GP
   --SELECT DISTINCT ASMId,COUNT(ASMId) NoOf FROM tblOrder AS INV GROUP BY ASMId
   

   EXEC(@Query)

 
END



```

### sp_GET_WingsSalesReport

- **Parameters:** @parameter (nvarchar(max), IN, unused in the body), @fDate (datetime, IN), @tdate (datetime, IN), @userId (int, IN)
- **Tables read:** tblCompanyUnit, tblCompanyInfo, tblUserCompanyUnit, tblInvoice, tblInvoiceDetail
- **Tables written:** none (only writes to the local table variable `@Sales`)
- **Business logic:** For each company/depot (business unit) the given user has access to, cursor-iterates and computes per-unit InvoiceValue, ActualSales, InTransit value, Reject value, and DDValue (discount/deduction value) from invoice/invoice-detail rows within the date range, accumulates them into a table variable, then returns only rows where at least one metric is non-zero. This is the "Wings" (multi-company) sales report used for company/depot-wise sales dashboards.
- **Transaction/error handling:** none
- **Result set:** CompanyId, CompanyName, Depot (ComUnitName), InvoiceValue, ActualSales, InTransit, Reject, DDValue, FromDate, ToDate — one row per depot with non-zero activity.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\FieldForceSummeryDal.cs:156` and `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\WingsWiseSalesReportDal.cs:166` via `GetDataTableAction("sp_GET_WingsSalesReport", ...)`; also referenced in a comment only (not actually called) at `WingsWiseSalesReportDal.cs:130`. These DALs are used directly (no BLL layer) from pages such as `Solution.Web\SInventory_UI\WingsSalesReport.aspx.cs`, `WingsWiseSalesReport.aspx.cs`, and `FieldForceSalesSummery.aspx.cs`.

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_WingsSalesReport] 

 @parameter NVARCHAR(MAX),
 @fDate DATETIME,
 @tdate DATETIME,
 @userId INt

AS
BEGIN
	

	  DECLARE @Sales TABLE 
	(
	  CompanyId INT, 
	  ComUnitId INT,
	  CompanyCode NVARCHAR(500),
	  CompanyName NVARCHAR(500),
	  ComUnitName NVARCHAR(500),
	  InvoiceValue DECIMAL(18,2),
	  ActualSales DECIMAL(18,2),
	  InTransit DECIMAL(18,2),
	  Reject DECIMAL(18,2),
	  DDValue DECIMAL(18,2),
	  FromDate DATEtime,
	  ToDate DATEtime
	)


	DECLARE @CompanyId INT
	DECLARE @ComUnitId INT
	DECLARE @CompanyCode NVARCHAR(500)
	DECLARE @CompanyName NVARCHAR(500)
	DECLARE @ComUnitName NVARCHAR(500)
	DECLARE @ActualSales DECIMAL(18,2)
	DECLARE @InTransit DECIMAL(18,2)
	DECLARE @Reject DECIMAL(18,2)
	DECLARE @InvoiceValue DECIMAL(18,2) 
	DECLARE @DDValue DECIMAL(18,2)



	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	SELECT CI.CompanyId,CI.CompanyCode,CI.CompanyName,UNT.ComUnitId,UNT.ComUnitName FROM dbo.tblCompanyUnit AS UNT 
	INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId 
	Where CI.CompanyId IN (SELECT DISTINCT CompanyId FROM tblUserCompanyUnit AS CU
	LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = @userId)
		
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		--PRINT CONVERT(NVARCHAR(max),@CompanyCode)
		--PRINT CONVERT(NVARCHAR(max),@CompanyName)
		--PRINT CONVERT(NVARCHAR(max),@ComUnitName)
		
		SELECT @InvoiceValue =  SUM(INVD.UnitPrice*INVD.Quantity) FROM dbo.tblInvoice AS INV   
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId  
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId   
		WHERE INV.InvoiceNo IS NOT NULL  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
	
		--PRINT CONVERT(NVARCHAR(max),@InvoiceValue)	
	
		SELECT @ActualSales = SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@ActualSales)	
	

		SELECT @Reject = SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)))
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Reject','Partial') AND INVD.Quantity > 0 AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@Reject)	
	
		SELECT @InTransit = SUM(INVD.UnitPrice*INVD.Quantity)  FROM dbo.tblInvoice AS INV 
				LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
				LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
				LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
				WHERE INV.DeliveryInvoiceStatus IS NULL  AND INV.InvoiceDate BETWEEN @fDate AND @tdate  AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@InTransit)
		

		SELECT @DDValue = SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity) + SUM(DeliveryDiscountAmount)
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
		INSERT INTO @Sales
		(
		    CompanyId,
			ComUnitId,
		    CompanyCode,
		    CompanyName,
		    ComUnitName,
		    InvoiceValue,
		    ActualSales,
		    InTransit,
		    Reject,
			DDValue,
			FromDate,
			ToDate
		)
		VALUES
		(   @CompanyId,    -- CompanyId - int
			@ComUnitId,    -- CompanyId - int
		    @CompanyCode,  -- CompanyCode - nvarchar(500)
		    @CompanyName,  -- CompanyName - nvarchar(500)
		    @ComUnitName,  -- ComUnitName - nvarchar(500)
		    ISNULL(@InvoiceValue,0), -- InvoiceValue - decimal(18, 2)
		    ISNULL(@ActualSales,0), -- ActualSales - decimal(18, 2)
		    ISNULL(@InTransit,0), -- InTransit - decimal(18, 2)
		    ISNULL(@Reject,0),  -- Reject - decimal(18, 2)
			ISNULL(@DDValue,0),  -- Reject - decimal(18, 2)
			@fDate,
			@tdate
		)
			
	
	FETCH NEXT FROM @MyCursor
	INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor



	SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
		InTransit,Reject,DDValue,FromDate,ToDate FROM @Sales WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0 OR DDValue > 0) 


	


	--IF(@parameter = '')
	--BEGIN

	--	SET @Query = 'SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
	--	InTransit,Reject FROM ' + @Sales + ' WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0) ' 

	--END
	
	--IF(@parameter != '')
	--BEGIN

	--	SET @Query = 'SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
	--	InTransit,Reject FROM ' + @Sales + ' WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0) ' + @parameter
		
	--END

	--EXEC(@Query)

END





```

### sp_GET_WingsSalesReportByCompany

- **Parameters:** @parameter (nvarchar(max), IN — used as a CompanyId filter value), @fDate (datetime, IN), @tdate (datetime, IN), @userId (int, IN)
- **Tables read:** tblCompanyUnit, tblCompanyInfo, tblUserCompanyUnit, tblInvoice, tblInvoiceDetail
- **Tables written:** none (writes only to local table variable `@Sales`)
- **Business logic:** Same cursor-driven metric computation as `sp_GET_WingsSalesReport`, but restricted to depots belonging to one specific company (`CI.CompanyId = @parameter`) that the user has access to — used for a company-specific drill-down of the Wings sales report.
- **Transaction/error handling:** none
- **Result set:** CompanyId, CompanyName, Depot, InvoiceValue, ActualSales, InTransit, Reject, DDValue, FromDate, ToDate — rows with non-zero activity for the specified company.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\FieldForceSummeryDal.cs:167` and `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\WingsWiseSalesReportDal.cs:178` via `GetDataTableAction("sp_GET_WingsSalesReportByCompany", ...)`, used from `WingsSalesReport.aspx.cs` / `WingsWiseSalesReport.aspx.cs` / `FieldForceSalesSummery.aspx.cs`.

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_WingsSalesReportByCompany] 

 @parameter NVARCHAR(MAX),
 @fDate DATETIME,
 @tdate DATETIME,
 @userId INT

AS
BEGIN
	

	DECLARE @Sales TABLE 
	(
	  CompanyId INT, 
	  ComUnitId INT,
	  CompanyCode NVARCHAR(500),
	  CompanyName NVARCHAR(500),
	  ComUnitName NVARCHAR(500),
	  InvoiceValue DECIMAL(18,2),
	  ActualSales DECIMAL(18,2),
	  InTransit DECIMAL(18,2),
	  Reject DECIMAL(18,2),
	  DDValue DECIMAL(18,2),
	  FromDate DATEtime,
	  ToDate DATEtime
	)


	DECLARE @CompanyId INT
	DECLARE @ComUnitId INT
	DECLARE @CompanyCode NVARCHAR(500)
	DECLARE @CompanyName NVARCHAR(500)
	DECLARE @ComUnitName NVARCHAR(500)
	DECLARE @ActualSales DECIMAL(18,2)
	DECLARE @InTransit DECIMAL(18,2)
	DECLARE @Reject DECIMAL(18,2)
	DECLARE @InvoiceValue DECIMAL(18,2) 
	DECLARE @DDValue DECIMAL(18,2)



	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------

	SELECT CI.CompanyId,CI.CompanyCode,CI.CompanyName,UNT.ComUnitId,UNT.ComUnitName FROM dbo.tblCompanyUnit AS UNT 
	INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId 
	WHERE CI.CompanyId IN (SELECT DISTINCT CompanyId FROM tblUserCompanyUnit AS CU
	LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = @userId) AND CI.CompanyId = @parameter

	

	--SELECT * FROM tblUser WHERE LoginName = 'yearul'
		
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		--PRINT CONVERT(NVARCHAR(max),@CompanyCode)
		--PRINT CONVERT(NVARCHAR(max),@CompanyName)
		--PRINT CONVERT(NVARCHAR(max),@ComUnitName)
		
		SELECT @InvoiceValue =  SUM(INVD.UnitPrice*INVD.Quantity) FROM dbo.tblInvoice AS INV   
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId  
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId   
		WHERE INV.InvoiceNo IS NOT NULL AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
	
		--PRINT CONVERT(NVARCHAR(max),@InvoiceValue)	
	
		SELECT @ActualSales = SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@ActualSales)	
	
		SELECT @Reject = SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)))
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
				WHERE INV.DeliveryInvoiceStatus IN ('Reject','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@Reject)	
	
		SELECT @InTransit = SUM(INVD.UnitPrice*INVD.Quantity)  FROM dbo.tblInvoice AS INV 
				LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
				LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
				LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
				WHERE INV.DeliveryInvoiceStatus IS NULL  AND INV.InvoiceDate BETWEEN @fDate AND @tdate  AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@InTransit)
		

		SELECT @DDValue = SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity) + SUM(DeliveryDiscountAmount)
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0 AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
		INSERT INTO @Sales
		(
		    CompanyId,
			ComUnitId,
		    CompanyCode,
		    CompanyName,
		    ComUnitName,
		    InvoiceValue,
		    ActualSales,
		    InTransit,
		    Reject,
			DDValue,
			FromDate,
			ToDate
		)
		VALUES
		(   @CompanyId,    -- CompanyId - int
			@ComUnitId,    -- CompanyId - int
		    @CompanyCode,  -- CompanyCode - nvarchar(500)
		    @CompanyName,  -- CompanyName - nvarchar(500)
		    @ComUnitName,  -- ComUnitName - nvarchar(500)
		    ISNULL(@InvoiceValue,0), -- InvoiceValue - decimal(18, 2)
		    ISNULL(@ActualSales,0), -- ActualSales - decimal(18, 2)
		    ISNULL(@InTransit,0), -- InTransit - decimal(18, 2)
		    ISNULL(@Reject,0),  -- Reject - decimal(18, 2)
			ISNULL(@DDValue,0),  -- Reject - decimal(18, 2)
			@fDate,
			@tdate
		)
			
	
	FETCH NEXT FROM @MyCursor
	INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor



	SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
		InTransit,Reject,DDValue,FromDate,ToDate FROM @Sales WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0 OR DDValue > 0) 


	


	--IF(@parameter = '')
	--BEGIN

	--	SET @Query = 'SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
	--	InTransit,Reject FROM ' + @Sales + ' WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0) ' 

	--END
	
	--IF(@parameter != '')
	--BEGIN

	--	SET @Query = 'SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
	--	InTransit,Reject FROM ' + @Sales + ' WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0) ' + @parameter
		
	--END

	--EXEC(@Query)

END




```

### sp_GET_WingsSalesReportByUnit

- **Parameters:** @parameter (nvarchar(max), IN — used as a ComUnitId filter value), @fDate (datetime, IN), @tdate (datetime, IN), @userId (int, IN)
- **Tables read:** tblCompanyUnit, tblCompanyInfo, tblUserCompanyUnit, tblInvoice, tblInvoiceDetail
- **Tables written:** none (writes only to local table variable `@Sales`)
- **Business logic:** Same cursor-driven metric computation as `sp_GET_WingsSalesReport`, but restricted to a single depot/company-unit (`UNT.ComUnitId = @parameter`) that the user has access to — used for a unit-specific drill-down of the Wings sales report.
- **Transaction/error handling:** none
- **Result set:** CompanyId, CompanyName, Depot, InvoiceValue, ActualSales, InTransit, Reject, DDValue, FromDate, ToDate — rows with non-zero activity for the specified unit.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\FieldForceSummeryDal.cs:178` and `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\WingsWiseSalesReportDal.cs:190` via `GetDataTableAction("sp_GET_WingsSalesReportByUnit", ...)`, used from `WingsSalesReport.aspx.cs` / `WingsWiseSalesReport.aspx.cs` / `FieldForceSalesSummery.aspx.cs`.

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_WingsSalesReportByUnit] 

 @parameter NVARCHAR(MAX),
 @fDate DATETIME,
 @tdate DATETIME,
 @userId INT

AS
BEGIN
	

	 DECLARE @Sales TABLE 
	(
	  CompanyId INT, 
	  ComUnitId INT,
	  CompanyCode NVARCHAR(500),
	  CompanyName NVARCHAR(500),
	  ComUnitName NVARCHAR(500),
	  InvoiceValue DECIMAL(18,2),
	  ActualSales DECIMAL(18,2),
	  InTransit DECIMAL(18,2),
	  Reject DECIMAL(18,2),
	  DDValue DECIMAL(18,2),
	  FromDate DATEtime,
	  ToDate DATEtime
	)


	DECLARE @CompanyId INT
	DECLARE @ComUnitId INT
	DECLARE @CompanyCode NVARCHAR(500)
	DECLARE @CompanyName NVARCHAR(500)
	DECLARE @ComUnitName NVARCHAR(500)
	DECLARE @ActualSales DECIMAL(18,2)
	DECLARE @InTransit DECIMAL(18,2)
	DECLARE @Reject DECIMAL(18,2)
	DECLARE @InvoiceValue DECIMAL(18,2) 
	DECLARE @DDValue DECIMAL(18,2)


	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------

	SELECT CI.CompanyId,CI.CompanyCode,CI.CompanyName,UNT.ComUnitId,UNT.ComUnitName FROM dbo.tblCompanyUnit AS UNT 
	INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId 
	WHERE  UNT.ComUnitId IN (SELECT DISTINCT CU.CompanyUnitId FROM tblUserCompanyUnit AS CU
	LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = @userId) and  UNT.ComUnitId = @parameter
		
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		--PRINT CONVERT(NVARCHAR(max),@CompanyCode)
		--PRINT CONVERT(NVARCHAR(max),@CompanyName)
		--PRINT CONVERT(NVARCHAR(max),@ComUnitName)
		
		SELECT @InvoiceValue =  SUM(INVD.UnitPrice*INVD.Quantity) FROM dbo.tblInvoice AS INV   
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId  
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId   
		WHERE INV.InvoiceNo IS NOT NULL AND INVD.Quantity > 0 AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
	
		--PRINT CONVERT(NVARCHAR(max),@InvoiceValue)	
	
		SELECT @ActualSales = SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@ActualSales)	
	
		SELECT @Reject = SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)))
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
				WHERE INV.DeliveryInvoiceStatus IN ('Reject','Partial')  AND INVD.Quantity > 0 AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@Reject)	
	
		SELECT @InTransit = SUM(INVD.UnitPrice*INVD.Quantity)  FROM dbo.tblInvoice AS INV 
				LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
				LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
				LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
				WHERE INV.DeliveryInvoiceStatus IS NULL  AND INV.InvoiceDate BETWEEN @fDate AND @tdate  AND INV.ComUnitId = @ComUnitId
	
		--PRINT CONVERT(NVARCHAR(max),@InTransit)
		

		SELECT @DDValue = SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity) + SUM(DeliveryDiscountAmount)
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
		INSERT INTO @Sales
		(
		    CompanyId,
			ComUnitId,
		    CompanyCode,
		    CompanyName,
		    ComUnitName,
		    InvoiceValue,
		    ActualSales,
		    InTransit,
		    Reject,
			DDValue,FromDate,ToDate
		)
		VALUES
		(   @CompanyId,    -- CompanyId - int
			@ComUnitId,    -- CompanyId - int
		    @CompanyCode,  -- CompanyCode - nvarchar(500)
		    @CompanyName,  -- CompanyName - nvarchar(500)
		    @ComUnitName,  -- ComUnitName - nvarchar(500)
		    ISNULL(@InvoiceValue,0), -- InvoiceValue - decimal(18, 2)
		    ISNULL(@ActualSales,0), -- ActualSales - decimal(18, 2)
		    ISNULL(@InTransit,0), -- InTransit - decimal(18, 2)
		    ISNULL(@Reject,0),  -- Reject - decimal(18, 2)
			ISNULL(@DDValue,0),  -- Reject - decimal(18, 2)
			@fDate,
			@tdate
		)
			
	
	FETCH NEXT FROM @MyCursor
	INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor



	SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
		InTransit,Reject,DDValue,FromDate,ToDate FROM @Sales WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0 OR DDValue > 0) 


	


	--IF(@parameter = '')
	--BEGIN

	--	SET @Query = 'SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
	--	InTransit,Reject FROM ' + @Sales + ' WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0) ' 

	--END
	
	--IF(@parameter != '')
	--BEGIN

	--	SET @Query = 'SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
	--	InTransit,Reject FROM ' + @Sales + ' WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0) ' + @parameter
		
	--END

	--EXEC(@Query)

END




```

### sp_GET_WingsSalesReport_ForZas_API

- **Parameters:** @parameter (nvarchar(max), IN, default null, unused in the body), @fDate (datetime, IN), @tdate (datetime, IN), @userId (int, IN)
- **Tables read:** tblCompanyUnit, tblCompanyInfo, tblUserCompanyUnit, tblInvoice, tblInvoiceDetail, tblCustMaster, tblCustCategory
- **Tables written:** none
- **Business logic:** A set-based (non-cursor) rewrite of the Wings sales report restricted to customers in category 1 (`CG.CategoryId=1`, e.g. "Pharmacy"), computing InvoiceValue/ActualSales/InTransit/Reject/DDValue per company/depot via conditional aggregation instead of a cursor, for exposure through an external ZAS API. A large block of the original cursor-based logic is left commented out below the working query.
- **Transaction/error handling:** none
- **Result set:** CompanyId, CompanyName, Depot, InvoiceValue, ActualSales, InTransit, Reject, DDValue, FromDate, ToDate — grouped by company/depot with a HAVING clause requiring at least one non-zero metric.
- **Caller(s):** UNUSED — no application caller found (likely intended for a separate external API integration not present in this codebase).

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_GET_WingsSalesReport_ForZas_API] 

 @parameter NVARCHAR(MAX)=NULL,
 @fDate DATETIME,
 @tdate DATETIME,
 @userId INt

AS
BEGIN
	
	SELECT
    CI.CompanyId,
    CI.CompanyName,
    UNT.ComUnitName AS Depot,
    -- InvoiceValue
    ISNULL(SUM(INVD.UnitPrice * INVD.Quantity), 0) AS InvoiceValue,
    -- ActualSales
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0 
        THEN INVD.DeliveryNetAmount 
        ELSE 0 END), 0) AS ActualSales,
    -- InTransit
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IS NULL 
        THEN INVD.UnitPrice * INVD.Quantity 
        ELSE 0 END), 0) AS InTransit,
    -- Reject
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IN ('Reject','Partial') AND INVD.Quantity > 0 
        THEN ((((INVD.UnitPrice*INVD.Quantity - INVD.DelivarySpecialAmount)/INVD.Quantity) * (INVD.Quantity - INVD.DeliveryQuantity)) 
              + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)))
        ELSE 0 END), 0) AS Reject,
    -- DDValue
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0
        THEN (INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity + ISNULL(INV.DeliveryTpDiscount,0)
        ELSE 0 END), 0) AS DDValue,
    @fDate AS FromDate,
    @tDate AS ToDate
FROM dbo.tblCompanyUnit UNT
INNER JOIN dbo.tblCompanyInfo CI ON CI.CompanyId = UNT.CompanyId
INNER JOIN tblUserCompanyUnit CU ON CU.CompanyUnitId = UNT.ComUnitId AND CU.UserId = @userId
LEFT JOIN dbo.tblInvoice INV ON INV.ComUnitId = UNT.ComUnitId AND INV.InvoiceDate BETWEEN @fDate AND @tDate
LEFT JOIN dbo.tblInvoiceDetail INVD ON INV.InvoiceId = INVD.InvoiceId
LEFT JOIN dbo.tblCustMaster CM ON INV.CustomerMasterId=CM.CustomerMasterId
LEFT JOIN tblCustCategory CG ON CM.CategoryId=CG.CategoryId
Where CG.CategoryId=1
GROUP BY
    CI.CompanyId,
    CI.CompanyName,
    UNT.ComUnitName
HAVING
    SUM(CASE WHEN INV.InvoiceId IS NOT NULL THEN INVD.UnitPrice * INVD.Quantity ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') THEN INVD.DeliveryNetAmount ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IS NULL THEN INVD.UnitPrice * INVD.Quantity ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IN ('Reject','Partial') THEN ((((INVD.UnitPrice*INVD.Quantity - INVD.DelivarySpecialAmount)/INVD.Quantity) * (INVD.Quantity - INVD.DeliveryQuantity)) 
              + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity))) ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0 THEN (INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity + ISNULL(INV.DeliveryTpDiscount,0) ELSE 0 END) > 0;

	--  DECLARE @Sales TABLE 
	--(
	--  CompanyId INT, 
	--  ComUnitId INT,
	--  CompanyCode NVARCHAR(500),
	--  CompanyName NVARCHAR(500),
	--  ComUnitName NVARCHAR(500),
	--  InvoiceValue DECIMAL(18,2),
	--  ActualSales DECIMAL(18,2),
	--  InTransit DECIMAL(18,2),
	--  Reject DECIMAL(18,2),
	--  DDValue DECIMAL(18,2),
	--  FromDate DATEtime,
	--  ToDate DATEtime
	--)


	--DECLARE @CompanyId INT
	--DECLARE @ComUnitId INT
	--DECLARE @CompanyCode NVARCHAR(500)
	--DECLARE @CompanyName NVARCHAR(500)
	--DECLARE @ComUnitName NVARCHAR(500)
	--DECLARE @ActualSales DECIMAL(18,2)
	--DECLARE @InTransit DECIMAL(18,2)
	--DECLARE @Reject DECIMAL(18,2)
	--DECLARE @InvoiceValue DECIMAL(18,2) 
	--DECLARE @DDValue DECIMAL(18,2)



	----------------------------------------------------------
	--DECLARE @MyCursor CURSOR
	--SET @MyCursor = CURSOR FAST_FORWARD
	--FOR
	-----------------
	--SELECT CI.CompanyId,CI.CompanyCode,CI.CompanyName,UNT.ComUnitId,UNT.ComUnitName FROM dbo.tblCompanyUnit AS UNT 
	--INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId 
	--Where CI.CompanyId IN (SELECT DISTINCT CompanyId FROM tblUserCompanyUnit AS CU
	--LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = @userId)
		
	------------
	--OPEN @MyCursor
	--FETCH NEXT FROM @MyCursor
	--INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	--WHILE @@FETCH_STATUS = 0
	--BEGIN
	
	--	--PRINT CONVERT(NVARCHAR(max),@CompanyCode)
	--	--PRINT CONVERT(NVARCHAR(max),@CompanyName)
	--	--PRINT CONVERT(NVARCHAR(max),@ComUnitName)
		
	--	SELECT @InvoiceValue =  SUM(INVD.UnitPrice*INVD.Quantity) FROM dbo.tblInvoice AS INV   
	--	LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId  
	--	LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  
	--	LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId   
	--	WHERE INV.InvoiceNo IS NOT NULL  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
	
	--	--PRINT CONVERT(NVARCHAR(max),@InvoiceValue)	
	
	--	SELECT @ActualSales = SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)
	--	FROM dbo.tblInvoice AS INV 
	--	LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
	--	LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	--	LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
	--	WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
	--	--PRINT CONVERT(NVARCHAR(max),@ActualSales)	
	

	--	SELECT @Reject = SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)))
	--	FROM dbo.tblInvoice AS INV 
	--	LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
	--	LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	--	LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
	--	WHERE INV.DeliveryInvoiceStatus IN ('Reject','Partial') AND INVD.Quantity > 0 AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
	
	--	--PRINT CONVERT(NVARCHAR(max),@Reject)	
	
	--	SELECT @InTransit = SUM(INVD.UnitPrice*INVD.Quantity)  FROM dbo.tblInvoice AS INV 
	--			LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
	--			LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	--			LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
	--			WHERE INV.DeliveryInvoiceStatus IS NULL  AND INV.InvoiceDate BETWEEN @fDate AND @tdate  AND INV.ComUnitId = @ComUnitId
	
	--	--PRINT CONVERT(NVARCHAR(max),@InTransit)
		

	--	SELECT @DDValue = SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity) + SUM(DeliveryDiscountAmount)
	--	FROM dbo.tblInvoice AS INV 
	--	LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
	--	LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	--	LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
	--	WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  AND INV.InvoiceDate BETWEEN @fDate AND @tdate AND INV.ComUnitId = @ComUnitId
		
	--	INSERT INTO @Sales
	--	(
	--	    CompanyId,
	--		ComUnitId,
	--	    CompanyCode,
	--	    CompanyName,
	--	    ComUnitName,
	--	    InvoiceValue,
	--	    ActualSales,
	--	    InTransit,
	--	    Reject,
	--		DDValue,
	--		FromDate,
	--		ToDate
	--	)
	--	VALUES
	--	(   @CompanyId,    -- CompanyId - int
	--		@ComUnitId,    -- CompanyId - int
	--	    @CompanyCode,  -- CompanyCode - nvarchar(500)
	--	    @CompanyName,  -- CompanyName - nvarchar(500)
	--	    @ComUnitName,  -- ComUnitName - nvarchar(500)
	--	    ISNULL(@InvoiceValue,0), -- InvoiceValue - decimal(18, 2)
	--	    ISNULL(@ActualSales,0), -- ActualSales - decimal(18, 2)
	--	    ISNULL(@InTransit,0), -- InTransit - decimal(18, 2)
	--	    ISNULL(@Reject,0),  -- Reject - decimal(18, 2)
	--		ISNULL(@DDValue,0),  -- Reject - decimal(18, 2)
	--		@fDate,
	--		@tdate
	--	)
			
	
	--FETCH NEXT FROM @MyCursor
	--INTO @CompanyId,@CompanyCode,@CompanyName,@ComUnitId,@ComUnitName
	
	--END
	--CLOSE @MyCursor
	--DEALLOCATE @MyCursor



	--SELECT CompanyId,CompanyName,ComUnitName AS Depot,InvoiceValue,ActualSales,
	--	InTransit,Reject,DDValue,FromDate,ToDate FROM @Sales WHERE (ActualSales > 0 OR InTransit > 0 OR InvoiceValue > 0 OR Reject > 0 OR DDValue > 0) 


END



```

### sp_GET_WingsSalesReport_Institute_ForZas_API

- **Parameters:** @parameter (nvarchar(max), IN, default null, unused in the body), @fDate (datetime, IN), @tdate (datetime, IN), @userId (int, IN)
- **Tables read:** tblCompanyUnit, tblCompanyInfo, tblUserCompanyUnit, tblInvoice, tblInvoiceDetail, tblCustMaster, tblCustCategory
- **Tables written:** none
- **Business logic:** Identical to `sp_GET_WingsSalesReport_ForZas_API` except it filters to customer category 2 (`CG.CategoryId=2`, e.g. "Institute") instead of category 1 — a per-customer-segment variant of the Wings sales report for an external ZAS API.
- **Transaction/error handling:** none
- **Result set:** CompanyId, CompanyName, Depot, InvoiceValue, ActualSales, InTransit, Reject, DDValue, FromDate, ToDate — grouped by company/depot with a HAVING clause requiring at least one non-zero metric.
- **Caller(s):** UNUSED — no application caller found.

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE   PROCEDURE [dbo].[sp_GET_WingsSalesReport_Institute_ForZas_API] 

 @parameter NVARCHAR(MAX)=NULL,
 @fDate DATETIME,
 @tdate DATETIME,
 @userId INt

AS
BEGIN
	
	SELECT
    CI.CompanyId,
    CI.CompanyName,
    UNT.ComUnitName AS Depot,
    -- InvoiceValue
    ISNULL(SUM(INVD.UnitPrice * INVD.Quantity), 0) AS InvoiceValue,
    -- ActualSales
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0 
        THEN INVD.DeliveryNetAmount 
        ELSE 0 END), 0) AS ActualSales,
    -- InTransit
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IS NULL 
        THEN INVD.UnitPrice * INVD.Quantity 
        ELSE 0 END), 0) AS InTransit,
    -- Reject
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IN ('Reject','Partial') AND INVD.Quantity > 0 
        THEN ((((INVD.UnitPrice*INVD.Quantity - INVD.DelivarySpecialAmount)/INVD.Quantity) * (INVD.Quantity - INVD.DeliveryQuantity)) 
              + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)))
        ELSE 0 END), 0) AS Reject,
    -- DDValue
    ISNULL(SUM(CASE 
        WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0
        THEN (INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity + ISNULL(INV.DeliveryTpDiscount,0)
        ELSE 0 END), 0) AS DDValue,
    @fDate AS FromDate,
    @tDate AS ToDate
FROM dbo.tblCompanyUnit UNT
INNER JOIN dbo.tblCompanyInfo CI ON CI.CompanyId = UNT.CompanyId
INNER JOIN tblUserCompanyUnit CU ON CU.CompanyUnitId = UNT.ComUnitId AND CU.UserId = @userId
LEFT JOIN dbo.tblInvoice INV ON INV.ComUnitId = UNT.ComUnitId AND INV.InvoiceDate BETWEEN @fDate AND @tDate
LEFT JOIN dbo.tblInvoiceDetail INVD ON INV.InvoiceId = INVD.InvoiceId
LEFT JOIN dbo.tblCustMaster CM ON INV.CustomerMasterId=CM.CustomerMasterId
LEFT JOIN tblCustCategory CG ON CM.CategoryId=CG.CategoryId
Where CG.CategoryId=2
GROUP BY
    CI.CompanyId,
    CI.CompanyName,
    UNT.ComUnitName
HAVING
    SUM(CASE WHEN INV.InvoiceId IS NOT NULL THEN INVD.UnitPrice * INVD.Quantity ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') THEN INVD.DeliveryNetAmount ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IS NULL THEN INVD.UnitPrice * INVD.Quantity ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IN ('Reject','Partial') THEN ((((INVD.UnitPrice*INVD.Quantity - INVD.DelivarySpecialAmount)/INVD.Quantity) * (INVD.Quantity - INVD.DeliveryQuantity)) 
              + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity))) ELSE 0 END) > 0
    OR SUM(CASE WHEN INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0 THEN (INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity + ISNULL(INV.DeliveryTpDiscount,0) ELSE 0 END) > 0;

	


END



```

### sp_Generate_InvoiceNo

- **Parameters:** @InvoiceNo (nvarchar(max), IN) — expected to already be a comma-separated, quoted list of invoice numbers embedded directly into dynamic SQL
- **Tables read:** tblInvoice
- **Tables written:** tblInvoice
- **Business logic:** Generates a new sequential "group invoice number" for the current day (format `INV-GP01-YYMMDDNNNN`, where NNNN is the next 4-digit sequence for the day), then updates all `tblInvoice` rows whose `InvoiceNo` is in the caller-supplied list to stamp them with this new `GroupInvoiceNo` and today's `GroupInvoiceDate`. This groups multiple individual invoices under one consolidated invoice number.
- **Transaction/error handling:** none (no explicit transaction wrapper; the UPDATE is built into a dynamic SQL string and executed via `EXEC`, which is a SQL-injection risk since `@InvoiceNo` is concatenated directly into the `IN (...)` clause).
- **Result set:** A single row/column: the generated GroupInvoiceNo.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\InvoiceDAL.cs:173` via `RunStoreProcedure("sp_Generate_InvoiceNo", ...)`, used from `Solution.Web\SInventory_UI\GroupInvoiceEntry.aspx.cs` (grouping multiple invoices into a single consolidated invoice number).

```sql

CREATE PROCEDURE [dbo].[sp_Generate_InvoiceNo]

    @InvoiceNo NVARCHAR(MAX)

AS
BEGIN


    DECLARE @GroupInvoiceNo NVARCHAR(50);
    DECLARE @MaxSequence INT;

    SELECT @MaxSequence = ISNULL(MAX(CAST(RIGHT(GroupInvoiceNo, 4) AS INT)), 0)
    FROM tblInvoice
    WHERE CAST(GroupInvoiceDate AS DATE) = CAST(GETDATE() AS DATE)
          AND GroupInvoiceNo IS NOT NULL
          AND GroupInvoiceNo <> '';

    SET @GroupInvoiceNo = 'INV-GP01-'
        + RIGHT(CAST(YEAR(GETDATE()) AS VARCHAR(4)), 2)
        + RIGHT('0' + CAST(MONTH(GETDATE()) AS VARCHAR(2)), 2)
        + RIGHT('0' + CAST(DAY(GETDATE()) AS VARCHAR(2)), 2)
        + RIGHT('0000' + CAST(@MaxSequence + 1 AS VARCHAR(4)), 4);

    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = '
        UPDATE tblInvoice
        SET GroupInvoiceNo = ''' + @GroupInvoiceNo + ''',
            GroupInvoiceDate = GETDATE()
        WHERE InvoiceNo IN (' + @InvoiceNo + ')';

    EXEC(@SQL);

    SELECT @GroupInvoiceNo AS GroupInvoiceNo;

END


--SELECT DISTINCT GroupInvoiceNo FROM tblInvoice WHERE InvoiceNo IN ( 'INV-BD53-2601010008'  


```

### sp_GetChamber_ByDoctorId

- **Parameters:** @DoctorId (int, IN)
- **Tables read:** tblDoctorChemberDetail
- **Tables written:** none
- **Business logic:** Returns the list of chambers (visit locations) associated with a given doctor.
- **Transaction/error handling:** none
- **Result set:** ChemberId, Chember (aliased from `name`) for the given DoctorId.
- **Caller(s):** UNUSED — no application caller found.

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetChamber_ByDoctorId]
	-- Add the parameters for the stored procedure here

	@DoctorId INT

AS
BEGIN
	

	SELECT tblDoctorChemberDetail.ChemberId , name   Chember  FROM dbo.tblDoctorChemberDetail with (nolock) WHERE    DoctorId = @DoctorId

END





```

### sp_GetExpireproductList

- **Parameters:** @pram (nvarchar(max), IN — raw SQL fragment for depot-stock branch), @pram2 (nvarchar(max), IN — raw SQL fragment for warehouse-stock branch)
- **Tables read:** tblDCStore, tblCompanyUnit, tblCompanyInfo, tblCentralStore, tblProduct
- **Tables written:** none
- **Business logic:** Returns near/actual expiry stock at the depot level (`tblDCStore`, grouped, with `@pram` appended for extra filters) UNIONed with stock still sitting in the central warehouse (`tblCentralStore`, labeled "Warehouse", with `@pram2` appended), for products with positive on-hand quantity, for the "nearly expiring product" report.
- **Transaction/error handling:** none
- **Result set:** CompanyName, ComUnitName (or "Warehouse"), ProductCode, ProductName, PackSize, MfgDate, ExpDate, BatchNo, StockQty — one row per product batch/location with StockQty > 0.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\AreaWiseMonthlyInventoryReportDal.cs:64` via `GetDataTableAction("sp_GetExpireproductList", ...)`, called through `AreaWiseMonthlyInventoryReportBll` from pages such as `Solution.Web\SInventory_UI\NearlyExpireProductReport.aspx.cs` / `AreaWiseMonthlyInventoryReport.aspx.cs`.

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,0@CompanyId/@CompanyId5/20@CompanyId6,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetExpireproductList] 

	@pram NVARCHAR(MAX),
	@pram2 NVARCHAR(MAX)
	
AS
BEGIN
	
	DECLARE @Query NVARCHAR(MAX)

	SET @Query = 'SELECT CompanyName,ComUnitName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo,SUM(StockQty) AS StockQty FROM dbo.tblDCStore 
	INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = tblDCStore.ComUnitId
	LEFT JOIN tblCompanyInfo AS CI ON CU.CompanyId = CI.CompanyId
	WHERE ProductCode IS NOT NULL' + @pram + ' GROUP BY CompanyName,ComUnitName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(StockQty) > 0
	UNION ALL
	SELECT CompanyName,''Warehouse'' ComUnitName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo,SUM(Quantity) AS StockQty FROM tblCentralStore AS CS
	LEFT JOIN tblProduct AS PD ON CS.ProductCode = PD.ProductCode
	LEFT JOIN tblCompanyInfo AS CI ON PD.CompanyId = CI.CompanyId WHERE PD.ProductCode IS NOT NULL ' + @pram2 + ' GROUP BY CompanyName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(Quantity) > 0'

	--SELECT CompanyName,ComUnitName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo,SUM(StockQty) AS StockQty FROM dbo.tblDCStore 
	--INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = tblDCStore.ComUnitId
	--LEFT JOIN tblCompanyInfo AS CI ON CU.CompanyId = CI.CompanyId
	--WHERE ProductCode IS NOT NULL GROUP BY CompanyName,ComUnitName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(StockQty) > 0
	--UNION ALL
	--SELECT CompanyName,'Warehouse' ComUnitName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo,SUM(Quantity) AS StockQty FROM tblCentralStore AS CS
	--LEFT JOIN tblProduct AS PD ON CS.ProductCode = PD.ProductCode
	--LEFT JOIN tblCompanyInfo AS CI ON PD.CompanyId = CI.CompanyId WHERE PD.ProductCode IS NOT NULL 
	--GROUP BY CompanyName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(Quantity) > 0


	EXEC (@Query)
END






```

### sp_GetExpireproductListNational

- **Parameters:** @pram (nvarchar(max), IN — raw SQL fragment for depot-stock branch), @pram2 (nvarchar(max), IN — raw SQL fragment for warehouse-stock branch)
- **Tables read:** tblDCStore, tblCompanyUnit, tblCompanyInfo, tblCentralStore, tblProduct
- **Tables written:** none
- **Business logic:** Same as `sp_GetExpireproductList` (depot stock UNION warehouse stock for expiring products), but rolled up at a national level — it groups without `ComUnitName` and hard-codes the location label to `'National'` for both branches, i.e. a company/national-level summary rather than per-depot.
- **Transaction/error handling:** none
- **Result set:** CompanyName, ComUnitName ("National"), ProductCode, ProductName, PackSize, MfgDate, ExpDate, BatchNo, StockQty.
- **Caller(s):** `D:\Projects\Zas Distribution\Library.DAL\SInventory_DAL\AreaWiseMonthlyInventoryReportDal.cs:60` via `GetDataTableAction("sp_GetExpireproductListNational", ...)`, called through `AreaWiseMonthlyInventoryReportBll` from the same expiry/inventory report pages as `sp_GetExpireproductList`.

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,0@CompanyId/@CompanyId5/20@CompanyId6,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetExpireproductListNational] 

	@pram NVARCHAR(MAX),
	@pram2 NVARCHAR(MAX)
	
AS
BEGIN
	
	DECLARE @Query NVARCHAR(MAX)

	SET @Query = 'SELECT CompanyName,''National'' ComUnitName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo,SUM(StockQty) AS StockQty FROM dbo.tblDCStore 
	INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = tblDCStore.ComUnitId
	LEFT JOIN tblCompanyInfo AS CI ON CU.CompanyId = CI.CompanyId
	WHERE ProductCode IS NOT NULL' + @pram + ' GROUP BY CompanyName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(StockQty) > 0
	UNION ALL
	SELECT CompanyName,''National'' ComUnitName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo,SUM(Quantity) AS StockQty FROM tblCentralStore AS CS
	LEFT JOIN tblProduct AS PD ON CS.ProductCode = PD.ProductCode
	LEFT JOIN tblCompanyInfo AS CI ON PD.CompanyId = CI.CompanyId WHERE PD.ProductCode IS NOT NULL ' + @pram2 + ' GROUP BY CompanyName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(Quantity) > 0'

	--SELECT CompanyName,'National' ComUnitName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo,SUM(StockQty) AS StockQty FROM dbo.tblDCStore 
	--INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = tblDCStore.ComUnitId
	--LEFT JOIN tblCompanyInfo AS CI ON CU.CompanyId = CI.CompanyId
	--WHERE ProductCode IS NOT NULL GROUP BY CompanyName,ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(StockQty) > 0
	--UNION ALL
	--SELECT CompanyName,'National' ComUnitName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo,SUM(Quantity) AS StockQty FROM tblCentralStore AS CS
	--LEFT JOIN tblProduct AS PD ON CS.ProductCode = PD.ProductCode
	--LEFT JOIN tblCompanyInfo AS CI ON PD.CompanyId = CI.CompanyId WHERE PD.ProductCode IS NOT NULL 
	--GROUP BY CompanyName,PD.ProductCode,PD.ProductName,PD.PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(Quantity) > 0


	EXEC (@Query)
END






```

### sp_Get_CapturedBY_For_DDL

- **Parameters:** none
- **Tables read:** tblEmpGeneralInfo, tblUser
- **Tables written:** none
- **Business logic:** Returns employees who are flagged as "apps users" (`IsAppsUser=1`), formatted as `EmpName(EmpMasterCode)`, for use in a "Captured By" dropdown (likely a CRM/field-force data-capture attribution field).
- **Transaction/error handling:** none
- **Result set:** EmpInfoId, EmpName (formatted as "Name(Code)").
- **Caller(s):** UNUSED — no application caller found.

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_CapturedBY_For_DDL]
	-- Add the parameters for the stored procedure here

AS
BEGIN
    

	SELECT B.UserId EmpInfoId,(A.EmpName+'('+A.EmpMasterCode+')') AS EmpName  FROM dbo.tblEmpGeneralInfo A 
	INNER JOIN dbo.tblUser B ON B.EmpInfoId = A.EmpInfoId
	where B.IsAppsUser=1


	
	--SELECT * FROM tbl_PrescriptionType WHERE IsActive = 1

END





```

### sp_Get_ChamberType_Active

- **Parameters:** none
- **Tables read:** tblDoctorChamber
- **Tables written:** none
- **Business logic:** Returns active doctor-chamber-type master records for a dropdown/list (CRM/doctor-visit module).
- **Transaction/error handling:** none
- **Result set:** All columns (`SELECT *`) from `tblDoctorChamber` where IsActive=1.
- **Caller(s):** UNUSED — no application caller found.

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_ChamberType_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN

		SELECT * FROM dbo.tblDoctorChamber WHERE IsActive = 1

END





```

### sp_Get_ContactType_Active

- **Parameters:** none
- **Tables read:** tbl_ContactType
- **Tables written:** none
- **Business logic:** Returns active contact-type master records for a dropdown/list (CRM/doctor-visit module).
- **Transaction/error handling:** none
- **Result set:** All columns (`SELECT *`) from `tbl_ContactType` where IsActive=1.
- **Caller(s):** UNUSED — no application caller found.

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Get_ContactType_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN

	SELECT * FROM dbo.tbl_ContactType WHERE IsActive=1

END





```

### sp_Get_DCRInfoListForView

- **Parameters:** @param (nvarchar(max), IN) — appended as a raw SQL fragment (extra WHERE/filter conditions)
- **Tables read:** tbl_DCRInfo, tbl_DcrVisitedWithDetails, tblEmpGeneralInfo, tblUser, tbl_UserRoleInfo, tblDesignation, tblDoctorMaster, tblDoctorChemberDetail, tbl_DoctorVisitType
- **Tables written:** none
- **Business logic:** Builds a detailed view of daily call report (DCR) visit records for a CRM/field-force module: for each DCR row it resolves the doctor's address/chamber, the recording user's role/designation, the visit type, and a comma-concatenated list of "visited with" colleagues (via `STUFF`/`FOR XML PATH`), decodes `ApprovalStatus` into a friendly label (Pending/Verified/Approved/Rejected), and appends `@param` as additional dynamic filter criteria before ordering by DCR date descending.
- **Transaction/error handling:** none
- **Result set:** StreetAddress, DoctorAddress, VisitedWith (concatenated names), POutLoc (lat/long), ApprovalStatus (decoded label), TourTypeName, ChamberName, DoctorName, RoleName, DesigName, EmpMasterCode, EmpName, DcrDate (formatted), plus all columns (`*`) from `tbl_DCRInfo`.
- **Caller(s):** UNUSED — no application caller found.

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE
 PROCEDURE [dbo].[sp_Get_DCRInfoListForView]
	-- Add the parameters for the stored procedure here
		@param NVARCHAR(max)
AS
BEGIN
 
    DECLARE @Query NVARCHAR(MAX)

 SET @Query = '
SELECT   DCR.StreetAddress,  docMas.DoctorAddress, STUFF( (SELECT CONCAT('','', mgd.EmpMasterCode +'' : ''+mgd.EmpName , '''') FROM tbl_DcrVisitedWithDetails mm (NOLOCK) INNER JOIN dbo.tblEmpGeneralInfo mgd ON mgd.EmpInfoId=mm.EmpInfoId WHERE mm.DcrId=DCR.DcrId ORDER BY mm.DcrVisitWithId FOR XML PATH ('''') ),1,1,'''') AS VisitedWith, DCR.Latitude  +'',''+ DCR.Longitude AS POutLoc, case when DCR.ApprovalStatus=''0'' then ''Pending''  when DCR.ApprovalStatus=''1'' then ''Verified''  when DCR.ApprovalStatus=''2'' then ''Approved''  WHEN DCR.ApprovalStatus=''3'' then ''Rejected''  else DCR.ApprovalStatus end ApprovalStatus, tt.VisitTypeName TourTypeName,cham.Name ChamberName,    docMas.DoctorCode +'' : ''+  docMas.DoctorName DoctorName,  usr.RoleName, dgs.DesigName, dtl.EmpMasterCode, dtl.EmpName,  FORMAT(DCR.DcrDate,''dd-MMM-yyyy hh:mm tt'')  DcrDate, * FROM  tbl_DCRInfo  DCR WITH (NOLOCK)
 

left JOIN dbo.tblUser us  WITH (NOLOCK) ON us.UserId = DCR.Entryby
left JOIN dbo.tblEmpGeneralInfo dtl  WITH (NOLOCK)  ON dtl.EmpInfoId = us.EmpInfoId
left JOIN dbo.tbl_UserRoleInfo usr  WITH (NOLOCK) ON usr.UserRoleID = us.UserRoleID
left JOIN dbo.tblDesignation dgs  WITH (NOLOCK) ON dgs.DesignationId = dtl.DesignationId
left JOIN dbo.tblDoctorMaster docMas  WITH (NOLOCK) ON docMas.DoctorId = DCR.DoctorId
 
left JOIN   tblDoctorChemberDetail cham  WITH (NOLOCK)    ON cham.ChemberId = DCR.ChemberId		
left JOIN dbo.tbl_DoctorVisitType tt  WITH (NOLOCK)  ON tt.DocVisitTypeId = DCR.TourTypeId
--left JOIN View_Webapi_EmployeeFieldForceInfo on View_Webapi_EmployeeFieldForceInfo.EmpInfoId=dtl.EmpInfoId
where  DCR.DcrId is not null
'+  @param +'  order by DCR.DcrDate desc'
 
END

EXEC (@Query)


```

### sp_Get_DCRReportNew

- **Parameters:** @param (nvarchar(max), IN) — appended as a raw SQL fragment
- **Tables read:** tblEmpGeneralInfo, tblUser, tblDesignation, tbl_UserRoleInfo, tblRoleType, tbl_DoctorTourPlanDetail, tbl_TourPlanInfo, tbl_DCRInfo, tbl_DCRCustomerInfo, tblCompanyInfo
- **Tables written:** none
- **Business logic:** Builds a per-employee, per-day field-force performance report comparing planned targets against actuals: doctor call-plan targets (`tbl_DoctorTourPlanDetail`), pharmacy/customer targets (`tbl_TourPlanInfo`), actual doctor DCR visits (`tbl_DCRInfo`), and actual pharmacy DCR visits (`tbl_DCRCustomerInfo`), joined by employee and matching tour-plan date, then appends `@param` as extra dynamic filters and orders by tour-plan date descending.
- **Transaction/error handling:** none
- **Result set:** EmpMasterCode, EmpName, RoleName, EmpInfoId, DesigName, TourPlanDate (formatted), TerritoryName_DV, AreaName_DV, RegionName_DV, CompanyName, PharmachyTarget, PharmachyVisit, DoctorTarget, DCRTarget.
- **Caller(s):** UNUSED — no application caller found.

```sql
CREATE
 PROCEDURE [dbo].[sp_Get_DCRReportNew]
	-- Add the parameters for the stored procedure here
		@param NVARCHAR(max)
AS
BEGIN
 
    DECLARE @Query NVARCHAR(MAX)

 SET @Query = '
select distinct PM.EmpMasterCode, PM.EmpName, usR.RoleName,PM.EmpInfoId,dgs.DesigName, FORMAT(tblDrTarget.TourPlanDate,''dd-MMM-yyyy'') TourPlanDate , tblDrTarget.TerritoryName_DV, tblDrTarget.AreaName_DV,tblDrTarget.RegionName_DV,  com.CompanyName, ISNULL(tblCsTarget.CustomerTarget,0) PharmachyTarget,  ISNULL(tblCusDCR.DCRCusTarget,0) as PharmachyVisit, ISNULL(tblDrTarget.DoctorTarget,0) DoctorTarget , ISNULL(tblDCR.DCRTarget,0)  DCRTarget  from tblEmpGeneralInfo PM with (nolock)
left join tblUser us  with (nolock) on PM.EmpInfoId=us.EmpInfoId 
   left join tblDesignation dgs  with (nolock) on PM.DesignationId=dgs.DesignationId
   left join tbl_UserRoleInfo usR  with (nolock) on usR.UserRoleId=us.UserRoleId
   left join tblRoleType usRT  with (nolock) on usR.RoleTypeId=usRT.RoleTypeId
   inner join (select    COUNT(distinct DoctorId) DoctorTarget,  TourPlanDate, EmpInfoId, CompanyId, TerritoryName_DV, AreaName_DV,RegionName_DV  from tbl_DoctorTourPlanDetail  group by TourPlanDate, EmpInfoId ,CompanyId, TerritoryName_DV, AreaName_DV,RegionName_DV) tblDrTarget on tblDrTarget.EmpInfoId=PM.EmpInfoId

   
     left join (select distinct    COUNT(distinct CustomerMasterId) CustomerTarget,  TourPlanDate, EmpInfoId   from tbl_TourPlanInfo  group by TourPlanDate, EmpInfoId ) tblCsTarget on tblCsTarget.EmpInfoId=PM.EmpInfoId and CONVERT(Date,tblDrTarget.TourPlanDate)=CONVERT(Date,tblCsTarget.TourPlanDate)

    left join (select   COUNT(distinct dcr.DoctorId) DCRTarget,   CONVERT(Date,dcr.DcrDate) DcrDate, u.EmpInfoId  from tbl_DCRInfo  dcr
	inner join tblUser u on dcr.EntryBy=u.UserId
	 group by CONVERT(Date,dcr.DcrDate), u.EmpInfoId) tblDCR on tblDCR.EmpInfoId=PM.EmpInfoId  and CONVERT(Date,tblDrTarget.TourPlanDate)=CONVERT(Date,tblDCR.DcrDate)

	      left join (select   COUNT(distinct dcr.CustomerId) DCRCusTarget,   CONVERT(Date,dcr.DcrDate) DcrDate, dcr.EmpInfoId  from tbl_DCRCustomerInfo  dcr
	 
	 group by CONVERT(Date,dcr.DcrDate), dcr.EmpInfoId) tblCusDCR on tblCusDCR.EmpInfoId=PM.EmpInfoId  and CONVERT(Date,tblCsTarget.TourPlanDate)=CONVERT(Date,tblCusDCR.DcrDate)

	 left join tblCompanyInfo com on com.CompanyId=tblDrTarget.CompanyId

	 where convert(Date,tblDrTarget.TourPlanDate) is not null
'+  @param +'  order by FORMAT(tblDrTarget.TourPlanDate,''dd-MMM-yyyy'') desc'
 
END

EXEC (@Query)


```

### sp_Get_DCRReportNew_All

- **Parameters:** @param (nvarchar(max), IN) — appended as a raw SQL fragment, @frmDate (nvarchar(max), IN), @toDate (nvarchar(max), IN)
- **Tables read:** tblEmpGeneralInfo, tblUser, tblDesignation, tbl_UserRoleInfo, tblRoleType, tbl_DoctorTourPlanDetail, tbl_TourPlanInfo, tbl_DCRInfo, tbl_DCRCustomerInfo, tblCompanyInfo
- **Tables written:** none
- **Business logic:** An extended version of `sp_Get_DCRReportNew` that scopes doctor and pharmacy tour-plan targets to an explicit date range (`@frmDate`..`@toDate`, embedded directly into the dynamic SQL string) rather than one implicit date, combines doctor and customer tour-plan info with `ISNULL`/`COALESCE`-style fallbacks, and only returns employees who had at least one doctor or customer target in the period, before appending `@param` and ordering by tour-plan date descending.
- **Transaction/error handling:** none
- **Result set:** EmpMasterCode, EmpName, RoleName, EmpInfoId, DesigName, TourPlanDate (formatted), TerritoryName_DV, AreaName_DV, RegionName_DV, CompanyName, PharmachyTarget, PharmachyVisit, DoctorTarget, DCRTarget.
- **Caller(s):** UNUSED — no application caller found.

```sql
CREATE
 PROCEDURE [dbo].[sp_Get_DCRReportNew_All]
	-- Add the parameters for the stored procedure here
		@param NVARCHAR(max),
		@frmDate NVARCHAR(max),
		@toDate NVARCHAR(max) 

AS
BEGIN
 
    DECLARE @Query NVARCHAR(MAX)

 SET @Query = '
select distinct   PM.EmpMasterCode, PM.EmpName, usR.RoleName,PM.EmpInfoId,dgs.DesigName, FORMAT(isnull( tblDrInfo.TourPlanDate,tblCrInfo.TourPlanDate),''dd-MMM-yyyy'') TourPlanDate ,  isnull(tblCrInfo.TerritoryName_DV,tblDrInfo.TerritoryName_DV ) TerritoryName_DV,   isnull(tblCrInfo.AreaName_DV,tblDrInfo.AreaName_DV ) AreaName_DV, isnull(tblCrInfo.RegionName_DV,tblDrInfo.RegionName_DV )  RegionName_DV, com.CompanyName CompanyName,   ISNULL(tblCsTarget.CustomerTarget,0) PharmachyTarget,  ISNULL(tblCusDCR.DCRCusTarget,0) as PharmachyVisit, ISNULL(tblDrTarget.DoctorTarget,0) DoctorTarget , ISNULL(tblDCR.DCRTarget,0)  DCRTarget  from tblEmpGeneralInfo PM with (nolock)
left join tblUser us  with (nolock) on PM.EmpInfoId=us.EmpInfoId 
   left join tblDesignation dgs  with (nolock) on PM.DesignationId=dgs.DesignationId
   left join tbl_UserRoleInfo usR  with (nolock) on usR.UserRoleId=us.UserRoleId
   left join tblRoleType usRT  with (nolock) on usR.RoleTypeId=usRT.RoleTypeId
   left join (select    ISNULL(COUNT(distinct Tdtl.DoctorId),0) DoctorTarget ,Tdtl.EmpInfoId    from tbl_DoctorTourPlanDetail Tdtl where  convert(Date, TourPlanDate)  BETWEEN '''+CAST(convert(Date,@frmDate) as nvarchar(max))+''' AND '''+CAST(convert(Date,@toDate)  as nvarchar(max))+'''   group by Tdtl.EmpInfoId ) tblDrTarget on tblDrTarget.EmpInfoId=PM.EmpInfoId

   
     left join (select distinct    ISNULL(COUNT(distinct CustomerMasterId),0) CustomerTarget,    EmpInfoId   from tbl_TourPlanInfo   where  convert(Date, TourPlanDate)  BETWEEN '''+CAST(convert(Date,@frmDate) as nvarchar(max))+''' AND '''+CAST(convert(Date,@toDate)  as nvarchar(max))+'''   group by   EmpInfoId ) tblCsTarget on tblCsTarget.EmpInfoId=PM.EmpInfoId  


	   left join (select    COUNT(distinct DoctorId) ChkDoctorTarget,  TourPlanDate, EmpInfoId, CompanyId, TerritoryName_DV, AreaName_DV,RegionName_DV  from tbl_DoctorTourPlanDetail    where  convert(Date, TourPlanDate)  BETWEEN '''+CAST(convert(Date,@frmDate) as nvarchar(max))+''' AND '''+CAST(convert(Date,@toDate)  as nvarchar(max))+''' group by TourPlanDate, EmpInfoId ,CompanyId, TerritoryName_DV, AreaName_DV,RegionName_DV  HAVING COUNT(distinct DoctorId)>0 ) tblDrInfo on tblDrInfo.EmpInfoId=PM.EmpInfoId


	   left join (select    COUNT(distinct CustomerMasterId) ChkCustomerTarget,  TourPlanDate, EmpInfoId, CompanyId, TerritoryName TerritoryName_DV, AreaName AreaName_DV, RegionName RegionName_DV  from tbl_TourPlanInfo  where  convert(Date, TourPlanDate)  BETWEEN '''+CAST(convert(Date,@frmDate) as nvarchar(max))+''' AND '''+CAST(convert(Date,@toDate)  as nvarchar(max))+'''     group by TourPlanDate, EmpInfoId ,CompanyId, TerritoryName, AreaName,RegionName  HAVING COUNT(distinct CustomerMasterId)>0 ) tblCrInfo on tblCrInfo.EmpInfoId=PM.EmpInfoId


	   	 left join tblCompanyInfo com on com.CompanyId=isnull(tblDrInfo.CompanyId,tblCrInfo.CompanyId)


		  left join (select   COUNT(distinct dcr.DoctorId) DCRTarget,   CONVERT(Date,dcr.DcrDate) DcrDate, u.EmpInfoId  from tbl_DCRInfo  dcr
	inner join tblUser u on dcr.EntryBy=u.UserId
	 group by CONVERT(Date,dcr.DcrDate), u.EmpInfoId) tblDCR on tblDCR.EmpInfoId=PM.EmpInfoId  and CONVERT(Date,isnull( tblDrInfo.TourPlanDate,tblCrInfo.TourPlanDate))=CONVERT(Date,tblDCR.DcrDate)

	      left join (select   COUNT(distinct dcr.CustomerId) DCRCusTarget,   CONVERT(Date,dcr.DcrDate) DcrDate, dcr.EmpInfoId  from tbl_DCRCustomerInfo  dcr
	 
	 group by CONVERT(Date,dcr.DcrDate), dcr.EmpInfoId) tblCusDCR on tblCusDCR.EmpInfoId=PM.EmpInfoId  and CONVERT(Date,isnull( tblDrInfo.TourPlanDate,tblCrInfo.TourPlanDate))=CONVERT(Date,tblCusDCR.DcrDate)




    where     ISNULL(DoctorTarget,0) + ISNULL(CustomerTarget,0)>0
'+  @param +'  order by FORMAT(isnull( tblDrInfo.TourPlanDate,tblCrInfo.TourPlanDate),''dd-MMM-yyyy'') desc'
 
END

EXEC (@Query)


```

### sp_Get_Degree_All_ActiveByDoctorTypeId

- **Parameters:** @id (int, IN) — accepted but not referenced in the query body
- **Tables read:** tblDoctorDegree
- **Tables written:** none
- **Business logic:** Returns active doctor-degree master records. Despite the name implying filtering "ByDoctorTypeId", the `@id` parameter is not actually used to filter the query — it simply returns all active degrees regardless of doctor type.
- **Transaction/error handling:** none
- **Result set:** All columns (`SELECT *`) from `tblDoctorDegree` where IsActive=1.
- **Caller(s):** UNUSED — no application caller found.

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Get_Degree_All_ActiveByDoctorTypeId]
	-- Add the parameters for the stored procedure here
	@id int 
AS
BEGIN
	

	SELECT * FROM tblDoctorDegree WHERE IsActive = 1  

END




```

### sp_Get_Designation_All_Active

- **Parameters:** none
- **Tables read:** tblDoctorDesignation
- **Tables written:** none
- **Business logic:** Returns all active doctor designation records (IsActive = 1) for populating designation lookups/dropdowns.
- **Transaction/error handling:** none
- **Result set:** All columns of active tblDoctorDesignation rows.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Get_Designation_All_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	

	SELECT * FROM tblDoctorDesignation WHERE IsActive = 1

END
```

### sp_Get_DoctorBrand_Active

- **Parameters:** none
- **Tables read:** tblProductSQ
- **Tables written:** none
- **Business logic:** Returns all rows from tblProductSQ (used as a "doctor brand" lookup source), read with NOLOCK.
- **Transaction/error handling:** none
- **Result set:** All columns from tblProductSQ.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorBrand_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN

		select * from tblProductSQ  with (nolock)

END
```

### sp_Get_DoctorCategoryList

- **Parameters:** none
- **Tables read:** tblDoctorCategory
- **Tables written:** none
- **Business logic:** Lists all non-deleted doctor categories (IsDelate IS NULL) with formatted activation date, for category maintenance/listing screens.
- **Transaction/error handling:** none
- **Result set:** CategoryId, CategoryName, IsActive, Activedate (formatted), EntryBy, EntryDate, UpdateBy, UpdateDate.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Get_DoctorCategoryList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT A.CategoryId ,
          A.CategoryName ,   
          A.IsActive ,
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM dbo.tblDoctorCategory A
		  Where A.IsDelate is NULL
END
```

### sp_Get_DoctorChamberList

- **Parameters:** none
- **Tables read:** tblDoctorChamber
- **Tables written:** none
- **Business logic:** Lists all non-deleted doctor chamber records with formatted activation date, for chamber maintenance/listing screens.
- **Transaction/error handling:** none
- **Result set:** ChamberId, ChamberName, IsActive, Activedate (formatted), EntryBy, EntryDate, UpdateBy, UpdateDate.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Get_DoctorChamberList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT A.ChamberId ,
          A.ChamberName ,   
          A.IsActive ,
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM dbo.tblDoctorChamber A
		  Where A.IsDelate is NULL
END
```

### sp_Get_DoctorChamber_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tblDoctorChamber
- **Tables written:** none
- **Business logic:** Fetches a single doctor chamber record by ChamberId for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** ChamberId, ChamberName, IsActive, Activedate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching chamber.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
Create PROCEDURE [dbo].[sp_Get_DoctorChamber_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.ChamberId ,
          A.ChamberName ,   
          A.IsActive ,
          A.Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM dbo.tblDoctorChamber A
				WHERE A.ChamberId = @id

    END
```

### sp_Get_DoctorCustomer_Active

- **Parameters:** none
- **Tables read:** SalesRollDB_ZAS..tblCustMaster
- **Tables written:** none
- **Business logic:** Returns a concatenated "code:name" label plus id for all active customers, sourced from the cross-database SalesRollDB_ZAS, likely for a customer dropdown.
- **Transaction/error handling:** none
- **Result set:** CustomerMasterId, Customer (CustomerCode:CustomerName concatenation) for active customers.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorCustomer_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN

		SELECT CustomerMasterId,CustomerCode + ':' + CustomerName AS Customer 
		FROM SalesRollDB_ZAS..tblCustMaster WHERE IsActive = 1

END
```

### sp_Get_DoctorDegreeList

- **Parameters:** none
- **Tables read:** tblDoctorDegree, tblDoctorDegreeDetail
- **Tables written:** none
- **Business logic:** Lists all non-deleted doctor degrees with a computed "IsDel" flag (Yes/No) indicating whether the degree has any linked detail rows in tblDoctorDegreeDetail, used to gate deletion in the UI.
- **Transaction/error handling:** none
- **Result set:** DegreeId, DegreeName, IsActive, Activedate (formatted), EntryBy, EntryDate, UpdateBy, UpdateDate, IsDel.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorDegreeList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT A.DegreeId ,
          A.DegreeName ,   
          A.IsActive ,
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate ,
		  CASE WHEN DS.S IS NULL THEN 'Yes' else 'No' END AS IsDel 	  
		  FROM dbo.tblDoctorDegree A
		  LEFT JOIN (SELECT DegId,COUNT(DoctorDegId) AS S FROM tblDoctorDegreeDetail GROUP BY DegId) AS DS 
		  ON DS.DegId = A.DegreeId
		  Where A.IsDelate is NULL


		  --SELECT DegId,COUNT(DoctorDegId) AS S FROM tblDoctorDegreeDetail GROUP BY DegId


END
```

### sp_Get_DoctorDegree_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tblDoctorDegree
- **Tables written:** none
- **Business logic:** Fetches a single doctor degree record by DegreeId for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** DegreeId, DegreeName, IsActive, Activedate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching degree.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
CREATE PROCEDURE [dbo].[sp_Get_DoctorDegree_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.DegreeId ,
          A.DegreeName ,   
          A.IsActive ,
          A.Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM dbo.tblDoctorDegree A
				WHERE A.DegreeId = @id

    END
```

### sp_Get_DoctorDesignationList

- **Parameters:** none
- **Tables read:** tblDoctorDesignation, tblDoctorMaster
- **Tables written:** none
- **Business logic:** Lists all non-deleted doctor designations with a computed "IsDel" flag indicating whether the designation is referenced by any doctor in tblDoctorMaster, used to gate deletion in the UI.
- **Transaction/error handling:** none
- **Result set:** DesignationId, DesignationName, IsActive, Activedate (formatted), EntryBy, EntryDate, UpdateBy, UpdateDate, IsDel.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorDesignationList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT A.DesignationId ,
          A.DesignationName ,   
          A.IsActive ,
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate,
		  CASE WHEN DS.S IS NULL THEN 'Yes' else 'No' END AS IsDel   	  
		  FROM dbo.tblDoctorDesignation A
		  LEFT JOIN (SELECT DesignationId,COUNT(DoctorId) AS S FROM tblDoctorMaster GROUP BY DesignationId) DS ON A.DesignationId = DS.DesignationId
		  Where A.IsDelate is NULL

		  
END
```

### sp_Get_DoctorDesignation_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tblDoctorDesignation
- **Tables written:** none
- **Business logic:** Fetches a single doctor designation record by DesignationId for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** DesignationId, DesignationName, IsActive, Activedate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching designation.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
Create PROCEDURE [dbo].[sp_Get_DoctorDesignation_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.DesignationId ,
          A.DesignationName ,   
          A.IsActive ,
          A.Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM dbo.tblDoctorDesignation A
				WHERE A.DesignationId = @id

    END
```

### sp_Get_DoctorList

- **Parameters:** @Parm (NVARCHAR(MAX), IN) — raw SQL fragment (e.g. additional WHERE/AND conditions) appended into dynamic SQL
- **Tables read:** tblDoctorMaster, tblDoctorContactDetail, tblDoctorType, tblProgramType, tblDoctorDegree, tblDoctorDegreeDetail, tblDoctorSpeciality, tblDoctorSpecialityDetail, tblDoctorProgramType, tblDoctorProgramTypeDetail, tblDoctorDesignation, tblMarket, tblTerritory, tblArea, tblRegion, tbl_Group, tblSMCType, tblUser
- **Tables written:** none
- **Business logic:** Builds and executes a dynamic query returning a full doctor listing with denormalized lookups (designation, program type, doctor type, market/territory/area/region hierarchy, SMC type, entry/update user names) plus comma-concatenated contact numbers, degrees, specialities, and program types per doctor, and a human-readable approval-status label. The caller-supplied @Parm string is concatenated directly into the WHERE clause (SQL-injection risk if not sanitized upstream) before ordering by DoctorCode descending.
- **Transaction/error handling:** none (uses sp_executesql, no TRY/CATCH)
- **Result set:** Wide doctor listing (SELECT * plus derived columns) filtered/ordered per @Parm, one row per doctor.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorList]
	-- Add the parameters for the stored procedure here
	@Parm nvarchar(max)
AS
BEGIN
   
   DECLARE @Q NVARCHAR(MAX)='
   SELECT St.SMCType,STUFF( (SELECT CONCAT('','', mm.Contact , '''') FROM tblDoctorContactDetail mm (NOLOCK)   WHERE mm.DoctorId=DM.DoctorId ORDER BY mm.ContactId FOR XML PATH ('''') ),1,1,'''') AS ContactName,dt.DoctorTypeName, pt.ProgramTypeName,mr.MarketCode, rg.RegionCode, Ar.AreaCode, Tr.TerritoryCode, case when DM.ApprovalStatus=''0'' then ''Pending''  when DM.ApprovalStatus=''1'' then ''Verified'' when DM.ApprovalStatus=''2'' then ''Approved'' when DM.ApprovalStatus=''3'' then ''Rejected''  else DM.ApprovalStatus end ApprovalStatusWeb,  DM.DoctorId, mr.MarketName, CONVERT(NVARCHAR(50),DM.EntryDate,106)AS EntryDate,  dgs.DesignationName DesigName,     DM.DoctorName,   Us.UserName as UserEntryBy,  Up.UserName UserUpdateBy, STUFF( (SELECT CONCAT('','', mm.DegreeName , '''') FROM tblDoctorDegree mm (NOLOCK) INNER JOIN dbo.tblDoctorDegreeDetail mgd ON mgd.DegId=mm.DegreeId WHERE mgd.DoctorId=DM.DoctorId ORDER BY mgd.DoctorDegId FOR XML PATH ('''') ),1,1,'''') AS DegreeName,  STUFF( (SELECT CONCAT('','', mm.SpecialityName , '''') FROM tblDoctorSpeciality mm (NOLOCK) INNER JOIN dbo.tblDoctorSpecialityDetail mgd ON mgd.SpecialityId=mm.SpecialityId WHERE mgd.DoctorId=DM.DoctorId ORDER BY mgd.DoctorSpId FOR XML PATH ('''') ),1,1,'''') as DoctorSpeciality ,  STUFF( (SELECT CONCAT('','', mm.ProgramType , '''') FROM tblDoctorProgramType mm with (NOLOCK) INNER JOIN dbo.tblDoctorProgramTypeDetail mgd ON mgd.ProgramTypeId=mm.ProgramTypeId WHERE mgd.DoctorId=DM.DoctorId ORDER BY mgd. DoctorTypeDetailId FOR XML PATH ('''') ),1,1,'''') as ProgramType, * from tblDoctorMaster DM
   Left Join dbo.tblDoctorDesignation dgs  with (NOLOCK)  ON dgs.DesignationId= DM.DesignationId
   Left Join dbo.tblProgramType pt  with (NOLOCK)  ON pt.ProgramTypeId= DM.ProgramTypeId
    Left Join dbo.tblDoctorType dt  with (NOLOCK)  ON dt.DoctorTypeId= DM.DoctorTypeId
   left join  tblMarket mr on DM.MarketId=mr.MarketId
	 
	 left join  tblTerritory  Tr  WITH (NOLOCK)   on mr.TerritoryId=Tr.TerritoryId
	 left join  tblArea  Ar  WITH (NOLOCK)   on Ar.AreaId=Tr.AreaId
	 left join  tblRegion  rg  WITH (NOLOCK)    on Ar.RegionId=rg.RegionId
	 left join  tbl_Group  gr  WITH (NOLOCK)   on gr.GroupId=rg.GroupId
   left join  tblSMCType  St  WITH (NOLOCK)   on DM.SMCTypeId=St.SMCTypeId
     Left Join tblUser Us  with (NOLOCK)  ON DM.EntryBy= Us.UserId
   Left Join tblUser Up  with (NOLOCK)  ON DM.UpdateBy= Up.UserId  where  DM.DoctorId is not null 
   '+@Parm +'
  


  ORDER BY DM.DoctorCode DESC'


EXEC sp_executesql @Q

END
```

### sp_Get_DoctorPatientList

- **Parameters:** none
- **Tables read:** tblDoctorPatientType
- **Tables written:** none
- **Business logic:** Lists all non-deleted doctor patient-type records with formatted activation date, for maintenance/listing screens.
- **Transaction/error handling:** none
- **Result set:** PatientTypeId, PatientType, IsActive, Activedate (formatted), EntryBy, EntryDate, UpdateBy, UpdateDate.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorPatientList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT A.PatientTypeId ,
          A.PatientType ,   
          A.IsActive ,
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate,
          A.EntryBy,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM [dbo].[tblDoctorPatientType] A
		  Where A.IsDelate is NULL
END
```

### sp_Get_DoctorPatientType_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tblDoctorPatientType
- **Tables written:** none
- **Business logic:** Fetches a single doctor patient-type record by PatientTypeId for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** PatientTypeId, PatientType, IsActive, Activedate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching row.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
Create PROCEDURE [dbo].[sp_Get_DoctorPatientType_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.PatientTypeId ,
          A.PatientType ,   
          A.IsActive ,
          A.Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM [dbo].[tblDoctorPatientType]  A
				WHERE A.PatientTypeId = @id

    END
```

### sp_Get_DoctorPlanDetailsById

- **Parameters:** @id (INT, IN)
- **Tables read:** tbl_DoctorTourPlanMaster, tbl_DoctorTourPlanDetail, tblDoctorMaster, tblEmpGeneralInfo, tblDesignation
- **Tables written:** none
- **Business logic:** Retrieves the detail lines of a specific doctor tour plan (by master id), joined with employee name/designation and the visited doctor's name, ordered by planned tour date.
- **Transaction/error handling:** none
- **Result set:** EmpName, DesigName, TourPlanDate (formatted), DoctorName, Comment, plus all columns from the detail/master join, ordered by TourPlanDate ascending.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorPlanDetailsById]
	-- Add the parameters for the stored procedure here
	@id INT
AS
BEGIN
 
SELECT  emp.EmpMasterCode +' : '+emp.EmpName EmpName,  dgs.DesigName,  CONVERT(NVARCHAR(50),dtl.TourPlanDate,106)AS  TourPlanDate,   cus.DoctorName,  dtl.Comment,  * FROM dbo.tbl_DoctorTourPlanMaster mas
INNER JOIN tbl_DoctorTourPlanDetail dtl ON dtl.DocTPMaster = mas.DocTPMaster
 
LEFT  JOIN dbo.tblDoctorMaster cus ON cus.DoctorId = dtl.DoctorId
 
 left JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = mas.EmpInfoId

left JOIN dbo.tblDesignation dgs ON dgs.DesignationId = emp.DesignationId



 WHERE mas.DocTPMaster=@id
		ORDER BY  dtl.TourPlanDate  asc
END
```

### sp_Get_DoctorProgramType_Active

- **Parameters:** none
- **Tables read:** tblDoctorProgramType
- **Tables written:** none
- **Business logic:** Returns all active doctor program type records for populating program-type lookups/dropdowns.
- **Transaction/error handling:** none
- **Result set:** All columns of active tblDoctorProgramType rows.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorProgramType_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN

		SELECT * FROM tblDoctorProgramType WHERE IsActive = 1

END
```

### sp_Get_DoctorSpeacialDay_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tblDoctorSpecialDay
- **Tables written:** none
- **Business logic:** Fetches a single doctor special-day record by SpecialDayId for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** SpecialDayId, SpecialDay, IsActive, Activedate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching row.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
Create PROCEDURE [dbo].[sp_Get_DoctorSpeacialDay_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.SpecialDayId ,
          A.SpecialDay ,   
          A.IsActive ,
          A.Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM [dbo].[tblDoctorSpecialDay]  A
				WHERE A.SpecialDayId = @id

    END
```

### sp_Get_DoctorSpecailDayList

- **Parameters:** none
- **Tables read:** tblDoctorSpecialDay
- **Tables written:** none
- **Business logic:** Lists all non-deleted doctor special-day records with formatted activation date, for maintenance/listing screens.
- **Transaction/error handling:** none
- **Result set:** SpecialDayId, SpecialDay, IsActive, Activedate (formatted), EntryBy, EntryDate, UpdateBy, UpdateDate.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Get_DoctorSpecailDayList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT A.SpecialDayId ,
          A.SpecialDay ,   
          A.IsActive ,
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM [dbo].[tblDoctorSpecialDay] A
		  Where A.IsDelate is NULL
END
```

### sp_Get_DoctorSpecialDay_All_Active

- **Parameters:** none
- **Tables read:** tblDoctorSpecialDay
- **Tables written:** none
- **Business logic:** Returns active doctor special-day names/ids (read with NOLOCK) for populating a lookup/dropdown.
- **Transaction/error handling:** none
- **Result set:** SpecialDay, SpecialDayId for active rows.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Get_DoctorSpecialDay_All_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	

	SELECT SpecialDay,SpecialDayId FROM tblDoctorSpecialDay  with (nolock) WHERE IsActive = 1

END
```

### sp_Get_DoctorSpecialityList

- **Parameters:** none
- **Tables read:** tblDoctorSpeciality
- **Tables written:** none
- **Business logic:** Lists all non-deleted doctor speciality records with formatted activation date, for maintenance/listing screens.
- **Transaction/error handling:** none
- **Result set:** SpecialityId, SpecialityName, IsActive, Activedate (formatted), EntryBy, EntryDate, UpdateBy, UpdateDate.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Get_DoctorSpecialityList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT A.SpecialityId ,
          A.SpecialityName ,   
          A.IsActive ,
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM [dbo].[tblDoctorSpeciality] A
		  Where A.IsDelate is NULL
END
```

### sp_Get_DoctorSpeciality_Active

- **Parameters:** none
- **Tables read:** tblDoctorSpeciality
- **Tables written:** none
- **Business logic:** Returns all active doctor speciality records for populating speciality lookups/dropdowns.
- **Transaction/error handling:** none
- **Result set:** All columns of active tblDoctorSpeciality rows.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorSpeciality_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN

		SELECT * FROM tblDoctorSpeciality WHERE IsActive = 1

END
```

### sp_Get_DoctorSpeciality_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tblDoctorSpeciality
- **Tables written:** none
- **Business logic:** Fetches a single doctor speciality record by SpecialityId for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** SpecialityId, SpecialityName, IsActive, Activedate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching row.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
Create PROCEDURE [dbo].[sp_Get_DoctorSpeciality_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.SpecialityId ,
          A.SpecialityName ,   
          A.IsActive ,
          A.Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM  [dbo].[tblDoctorSpeciality] A
				WHERE A.SpecialityId = @id

    END
```

### sp_Get_DoctorTourPlanMasteList

- **Parameters:** @param (NVARCHAR(MAX), IN) — raw SQL fragment appended into dynamic SQL
- **Tables read:** tbl_DoctorTourPlanMaster, tblEmpGeneralInfo, tblUser, tbl_UserRoleInfo, tblDesignation
- **Tables written:** none
- **Business logic:** Builds and executes a dynamic query listing doctor tour plan master records with human-readable approval status and month name, joined to employee/role/designation info; the caller-supplied @param string is concatenated directly into the WHERE clause (SQL-injection risk if not sanitized upstream).
- **Transaction/error handling:** none (uses EXEC on a dynamically built string, no TRY/CATCH)
- **Result set:** ApprovalStatus (decoded label), FinalSubmitRemarks, RoleName, DesigName, MonthName1 (decoded), plus all tour-plan-master columns, filtered/ordered per @param.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorTourPlanMasteList]
	-- Add the parameters for the stored procedure here
		@param NVARCHAR(max)
AS
BEGIN
 
    DECLARE @Query NVARCHAR(MAX)

 SET @Query = '
SELECT    case when mas.ApprovalStatus=''0'' then ''Pending''  when mas.ApprovalStatus=''1'' then ''Verified''  when mas.ApprovalStatus=''2'' then ''Approved''  WHEN mas.ApprovalStatus=''3'' then ''Rejected''  else mas.ApprovalStatus end ApprovalStatus,mas.FinalSubmitRemarks,  usr.RoleName, dgs.DesigName,   CASE WHEN mas.MonthValue=1 THEN ''January'' WHEN mas.MonthValue=2 THEN ''February'' WHEN mas.MonthValue=3 THEN ''March'' WHEN mas.MonthValue=4 THEN ''April'' WHEN mas.MonthValue=5 THEN ''May'' WHEN mas.MonthValue=6 THEN ''June''  WHEN mas.MonthValue=7 THEN ''July'' WHEN mas.MonthValue=8 THEN ''August'' WHEN mas.MonthValue=9 THEN ''September''  WHEN mas.MonthValue=10 THEN ''October''  WHEN mas.MonthValue=11 THEN ''November''  WHEN mas.MonthValue=12 THEN ''December'' ELSE '''' END  MonthName1    , * FROM dbo.tbl_DoctorTourPlanMaster mas
left JOIN dbo.tblEmpGeneralInfo dtl ON dtl.EmpInfoId = mas.EmpInfoId
left JOIN dbo.tblUser us ON us.EmpInfoId = mas.EmpInfoId
left JOIN dbo.tbl_UserRoleInfo usr ON usr.UserRoleID = us.UserRoleID
left JOIN dbo.tblDesignation dgs ON dgs.DesignationId = dtl.DesignationId
		
  where   mas.EmpInfoId is not null	 
'+  @param
 
END

EXEC (@Query)
```

### sp_Get_DoctorType_Active

- **Parameters:** none
- **Tables read:** tblDoctorType
- **Tables written:** none
- **Business logic:** Returns all active doctor type records for populating doctor-type lookups/dropdowns.
- **Transaction/error handling:** none
- **Result set:** All columns of active tblDoctorType rows.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_DoctorType_Active]
	-- Add the parameters for the stored procedure here

AS
BEGIN

		SELECT * FROM dbo.tblDoctorType WHERE IsActive = 1

END
```

### sp_Get_Doctor_For_DDL

- **Parameters:** none
- **Tables read:** tblDoctorMaster
- **Tables written:** none
- **Business logic:** Returns all doctor master records, apparently intended for a doctor dropdown list (no active filter is applied despite the "For_DDL" naming).
- **Transaction/error handling:** none
- **Result set:** All columns of tblDoctorMaster.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Get_Doctor_For_DDL]
	-- Add the parameters for the stored procedure here

AS
BEGIN
    
	SELECT * FROM tblDoctorMaster 
	
	--SELECT * FROM tbl_PrescriptionType WHERE IsActive = 1

END
```

### sp_Get_Doctorcategory_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tblDoctorCategory
- **Tables written:** none
- **Business logic:** Fetches a single doctor category record by CategoryId for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** CategoryId, CategoryName, IsActive, Activedate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching row.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
Create PROCEDURE [dbo].[sp_Get_Doctorcategory_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.CategoryId ,
          A.CategoryName ,   
          A.IsActive ,
          A.Activedate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM dbo.tblDoctorCategory A
				WHERE A.CategoryId = @id

    END
```

### sp_Get_Emp_AttendanceInfoDayRow

- **Parameters:** @param (NVARCHAR(MAX), IN) — raw SQL fragment appended into dynamic SQL
- **Tables read:** tblMarketAttendance_Master_webapi, tblEmpGeneralInfo, tbl_UserRoleInfo, tblRoleType, tblDesignation, tbl_Shift, View_Webapi_EmployeeFieldForceInfo
- **Tables written:** none
- **Business logic:** Builds and executes a dynamic query producing one "day row" per employee combining punch-in (AttType=1) and punch-out (AttType=2) attendance records for the same date, along with GPS coordinates, role/designation info, and a role-type-dependent territory/area/region/group code, filtered by the caller-supplied @param fragment and ordered by attendance date descending.
- **Transaction/error handling:** none (uses EXEC on a dynamically built string, no TRY/CATCH)
- **Result set:** AttStatus, TerritoryCode (role-dependent), PInLoc, RoleName, DesigName, EmpMasterCode, EmpName, AttendanceDate, PunchInTime, PInLat, PInLog, PunchOutTime, POutLoc, POutLat, POutLong.
- **Caller(s):** UNUSED — no application caller found

```sql
CREATE
 PROCEDURE [dbo].[sp_Get_Emp_AttendanceInfoDayRow]
	-- Add the parameters for the stored procedure here

	@param NVARCHAR(max)

AS
BEGIN
  

DECLARE @Query NVARCHAR(MAX)

SET @Query = ' 
  
	SELECT  distinct    CASE WHEN att2.PunchInTime  IS NOT NULL THEN ''inline'' ELSE ''none'' END AttStatus, case when usrRT.RoleTypeId=4   then EFF.GroupName when usrRT.RoleTypeId=3  then  EFF.RegionCode when usrRT.RoleTypeId=2  then  EFF.AreaCode   else EFF.TerritoryCode end TerritoryCode, att1.PInLat  +'',''+ att1.PInLog AS PInLoc,   usr.RoleName, dgs.DesigName,dtl.EmpMasterCode,dtl.EmpName, CONVERT(NVARCHAR(50), att1.AttendanceDate, 107)AS  AttendanceDate, 
                att1.PunchInTime ,
              
                att1.PInLat ,
                att1.PInLog  , att2.PunchInTime  PunchOutTime,
               att2.PInLat  +'',''+ att2.PInLog AS POutLoc, 
                att2.PInLat POutLat,
                att2.PInLog  POutLong
        FROM    dbo.tblMarketAttendance_Master_webapi att1 

		left join (select  AttendanceDate, PunchInTime,PInLat,PInLog, EmpInfoId from  tblMarketAttendance_Master_webapi  where  AttType=2 ) att2 on att2.EmpInfoId=att1.EmpInfoId and  CONVERT(DATE,att1.AttendanceDate)=CONVERT(DATE,att2.AttendanceDate)
		 left JOIN dbo.tblEmpGeneralInfo dtl  with (nolock) ON dtl.EmpInfoId = att1.EmpInfoId 
left JOIN dbo.tbl_UserRoleInfo usr  with (nolock) ON usr.UserRoleID = att1.UserRoleID
inner join  tblRoleType usrRT WITH (NOLOCK)    on usrRT.RoleTypeId=usr.RoleTypeId
left JOIN dbo.tblDesignation dgs  with (nolock) ON dgs.DesignationId = dtl.DesignationId
left JOIN dbo.tbl_Shift sft  with (nolock) ON sft.ShiftId=att1.ShiftId
left JOIN View_Webapi_EmployeeFieldForceInfo  EFF  on EFF.EmpInfoId=att1.EmpInfoId
		 
		 
		WHERE att1.AttType=1  '+  @param  +' 	ORDER BY CONVERT(NVARCHAR(50), att1.AttendanceDate, 107) desc   '
 
END

EXEC (@Query)
```

### sp_Get_Emp_AttendanceInfoList

- **Parameters:** @param (NVARCHAR(MAX), IN) — raw SQL fragment appended into dynamic SQL
- **Tables read:** tblMarketAttendance_Master_webapi, tbl_ImagePath_Setting, tblEmpGeneralInfo, tblDesignation, tblUser, tbl_UserRoleInfo, tbl_Shift
- **Tables written:** none
- **Business logic:** Builds and executes a dynamic query listing employee attendance rows with computed image paths for punch-in/out photos, shift text, role/designation info, restricted to app users (IsAppsUser=1), filtered by caller-supplied @param and ordered by attendance date descending.
- **Transaction/error handling:** none (uses EXEC on a dynamically built string, no TRY/CATCH)
- **Result set:** AttStatusP, ImageString, InAddress/OutAddress/ApprovalStatus (placeholders), AttStatus, ImagePreName, POutRemarks, ShiftText, PInLoc, RoleName, DesigName, EmpMasterCode, EmpName, AttendanceDate, PunchInTime, PInLat, PInLog, PunchOutTime, POutLoc, POutLat, POutLong.
- **Caller(s):** UNUSED — no application caller found

```sql

CREATE
 PROCEDURE [dbo].[sp_Get_Emp_AttendanceInfoList]
	-- Add the parameters for the stored procedure here

	@param NVARCHAR(max)

AS
BEGIN
  

DECLARE @Query NVARCHAR(MAX)

SET @Query = ' 
  
	SELECT  distinct   att1.AttStatus AttStatusP, (SELECT LTRIM(RTRIM(ImagePath+''/''+ImagePreName)) FROM dbo.tbl_ImagePath_Setting  with (nolock)  WHERE ImageType=''AttendanceMy'')+CAST(att1.AttendanceId as nvarchar(max))+''.jpg'' AS   ImageString, ''''  InAddress , '''' OutAddress, '''' ApprovalStatus,  CASE WHEN att1.PunchOutTime IS NOT NULL THEN ''inline'' ELSE ''none'' END AttStatus,  (SELECT LTRIM(RTRIM(ImagePath+''\''+ImagePreName)) FROM dbo.tbl_ImagePath_Setting WHERE ImageType=''Attendance'')+CAST(att1.AttendanceId AS NVARCHAR(max))+''.jpg''AS ImagePreName, att1.POutRemarks,  st.ShiftText ShiftText,  att1.PInLat  +'',''+ att1.PInLog AS PInLoc,   usr.RoleName, dgs.DesigName,dtl.EmpMasterCode,dtl.EmpName, CONVERT(NVARCHAR(50), att1.AttendanceDate, 107)AS  AttendanceDate, 
                att1.PunchInTime ,
              
                att1.PInLat ,
                att1.PInLog  , att1.PunchOutTime  PunchOutTime,
               att1.POutLat  +'',''+ att1.POutLat AS POutLoc, 
                 att1.POutLat POutLat,
                att1.POutLat  POutLong
        FROM    dbo.tblMarketAttendance_Master_webapi att1   with (nolock) 

		 
		 left JOIN dbo.tblEmpGeneralInfo dtl  with (nolock) ON dtl.EmpInfoId = att1.EmpInfoId 
 
left JOIN dbo.tblDesignation dgs  with (nolock) ON dgs.DesignationId = dtl.DesignationId
  inner join tblUser ss on ss.EmpInfoId=dtl.EmpInfoId  and ss.IsAppsUser=1
    left JOIN dbo.tbl_UserRoleInfo usr  with (nolock) ON usr.UserRoleID =ss.UserRoleID
		  left join tbl_Shift st on st.ShiftId=att1.ShiftId 
		 
		WHERE   att1.AttendanceId is not null  '+  @param  +' 	ORDER BY CONVERT(NVARCHAR(50), att1.AttendanceDate, 107) desc   '
 
END

EXEC (@Query)
```

### sp_Get_EmployeeFieldForceInfo_EmpId

- **Parameters:** @id (INT, IN)
- **Tables read:** View_Webapi_EmployeeFieldForceInfo
- **Tables written:** none
- **Business logic:** Returns the field-force hierarchy ids (MIO/ASM/RSM/NSM employee ids, group/region/area/territory ids) for a given employee id, from a precomputed view.
- **Transaction/error handling:** none
- **Result set:** MIOEmpId, ASMEMPId, RSMEMPId, NSMEMPId, EmpGroupId, EmpRegionId, EmpAreaId, EmpTerrId for the given employee.
- **Caller(s):** UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Get_EmployeeFieldForceInfo_EmpId]
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
		 
		select  MIOEmpId, ASMEMPId, RSMEMPId,NSMEMPId, EmpGroupId,EmpRegionId,  EmpAreaId,  EmpTerrId  from View_Webapi_EmployeeFieldForceInfo with (nolock) where EmpInfoId=@id

END
```

### sp_Get_EmployeeList

- **Parameters:** none
- **Tables read:** tblEmpGeneralInfo, ZASHRIS..tblEmpGeneralInfo
- **Tables written:** none
- **Business logic:** Returns a "code : name" label plus id for employees whose linked HRIS record shows EmployeeStatus='Active' and who have a non-blank employee code, joined across the local and ZASHRIS databases, ordered by employee code.
- **Transaction/error handling:** none
- **Result set:** EmployeeName (code : name), EmpInfoId, ordered by EmpMasterCode.
- **Caller(s):** UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Get_EmployeeList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
 SELECT        emp.EmpMasterCode+' : '+ emp.EmpName  EmployeeName, emp.EmpInfoId	FROM dbo.tblEmpGeneralInfo  emp with (nolock) --where EmployeeStatus='Active'
				inner join ZASHRIS..tblEmpGeneralInfo hris on hris.EmpMasterCode=emp.EmpMasterCode
				where hris.EmployeeStatus='Active' and emp.EmpMasterCode <>'' 
				order by emp.EmpMasterCode
    

END
```

### sp_Get_Employee_ShiftInfosList

- **Parameters:** none
- **Tables read:** tbl_Shift
- **Tables written:** none
- **Business logic:** Lists all active shift definitions with shift-in/out times formatted as HH:MM, for shift maintenance/listing screens.
- **Transaction/error handling:** none
- **Result set:** ShiftInTime, ShiftOutTime (formatted), Activedate (formatted), plus all other columns of active tbl_Shift rows.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_Employee_ShiftInfosList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
   SELECT   CONVERT(varchar(100),CAST(ShiftInTime AS TIME),100)ShiftInTime, CONVERT(varchar(100),CAST(ShiftOutTime AS TIME),100)ShiftOutTime, 
          CONVERT(NVARCHAR(50),A.Activedate,106)AS Activedate ,*
         
		  FROM dbo.tbl_Shift A
		  WHERE A.IsActive=1
		  


		  --SELECT DegId,COUNT(DoctorDegId) AS S FROM tblDoctorDegreeDetail GROUP BY DegId


END
```

### sp_Get_Employee_ShiftInfos_ById

- **Parameters:** @id (INT, IN)
- **Tables read:** tbl_Shift
- **Tables written:** none
- **Business logic:** Fetches a single shift definition by ShiftId with formatted in/out times, for an edit/detail view.
- **Transaction/error handling:** none
- **Result set:** ShiftInTime, ShiftOutTime (formatted), plus all other columns for the matching shift.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
CREATE
 PROCEDURE [dbo].[sp_Get_Employee_ShiftInfos_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT CONVERT(varchar(100),CAST(ShiftInTime AS TIME),100)   ShiftInTime,CONVERT(varchar(100),CAST(ShiftOutTime AS TIME),100)    ShiftOutTime,  *	  
		  FROM dbo.tbl_Shift A
				WHERE A.ShiftId = @id

    END
```

### sp_Get_LocationTrackList

- **Parameters:** @param (NVARCHAR(MAX), IN) — raw SQL fragment appended into dynamic SQL; @Date (NVARCHAR(MAX), IN) — date filter value interpolated into dynamic SQL
- **Tables read:** tblEmpGeneralInfo, tbl_UserTracking, tblCompanyInfo
- **Tables written:** none
- **Business logic:** Builds and executes a dynamic query returning, per employee, the first and last GPS-tracking rows recorded on @Date (start/end address, company, start/end time), used for a daily location-tracking summary; both @Date and @param are concatenated directly into the SQL string (SQL-injection risk if not sanitized upstream).
- **Transaction/error handling:** none (uses EXEC on a dynamically built string, no TRY/CATCH)
- **Result set:** EmpInfoId, EmpMasterCode, EmpName, AddressName, Company, TrackDate (formatted), StartTime, EndTime, LocationGeo (placeholder), one row per employee tracked on the given date.
- **Caller(s):** UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_LocationTrackList]
	-- Add the parameters for the stored procedure here
		@param NVARCHAR(max),
		@Date NVARCHAR(max)

AS
BEGIN
 
    DECLARE @Query NVARCHAR(MAX)

 SET @Query = '
select distinct emp.EmpInfoId, emp.EmpMasterCode, emp.EmpName, tcStart.AddressName AddressName, com.CompanyName Company, FORMAT(tcStart.TrackDate ,''dd-MMM-yyyy'')  TrackDate, tcStart.Time StartTime, tcEnd.Time EndTime , '''' LocationGeo from tblEmpGeneralInfo  emp 
inner join (select min(TrackingId) TrackingId,   EmpInfoId from  tbl_UserTracking where  CONVERT(Date, TrackDate)='''+cast(CONVERT(date,@Date) as nvarchar(max))+''' group by  EmpInfoId)tblStart on emp.EmpInfoId=tblStart.EmpInfoId

left join (select max(TrackingId) TrackingIdEnd,   EmpInfoId from  tbl_UserTracking where  CONVERT(Date, TrackDate)='''+cast(CONVERT(date,@Date) as nvarchar(max))+''' group by  EmpInfoId)tblEnd on emp.EmpInfoId=tblEnd.EmpInfoId

inner join tbl_UserTracking tcStart on tblStart.TrackingId=tcStart.TrackingId and tblStart.EmpInfoId=tcStart.EmpInfoId

left join tbl_UserTracking tcEnd on tcEnd.TrackingId=tblEnd.TrackingIdEnd and tblEnd.EmpInfoId=tcEnd.EmpInfoId
left join tblCompanyInfo com on com.CompanyId=emp.CompanyId   
  where   tblStart.EmpInfoId is not null	 
'+  @param
 
END

EXEC (@Query)
```

### sp_Get_MonthlyAttendanceHistory

- **Parameters:** @param (NVARCHAR(MAX), IN) — raw SQL fragment appended into dynamic SQL; @DateRange (NVARCHAR(MAX), IN) — raw SQL date-range fragment appended into each subquery's WHERE clause
- **Tables read:** tblEmpGeneralInfo, tblMarketAttendance_Master_webapi, tblCompanyInfo
- **Tables written:** none
- **Business logic:** Builds and executes a dynamic query producing a monthly attendance summary per employee, classifying each punch-in into Present (05:00-09:15), Late (09:16-09:30), or Absent (>=09:31) buckets using hardcoded time-of-day thresholds, then counting occurrences per bucket over the date range supplied via @DateRange/@param, only returning employees with at least one counted attendance event.
- **Transaction/error handling:** none (uses EXEC on a dynamically built string, no TRY/CATCH)
- **Result set:** ItemNo (row number), EmpMasterCode, EmpName, CompanyName, AreaName (placeholder), Present, Late, Absent counts per employee.
- **Caller(s):** UNUSED — no application caller found

```sql

CREATE
 PROCEDURE [dbo].[sp_Get_MonthlyAttendanceHistory]
	-- Add the parameters for the stored procedure here

	@param NVARCHAR(max),
	@DateRange NVARCHAR(max)


AS
BEGIN
  

DECLARE @Query NVARCHAR(MAX)

DECLARE @LastTime NVARCHAR(MAX)
DECLARE @PS NVARCHAR(MAX)
DECLARE @PE NVARCHAR(MAX)

DECLARE @LS NVARCHAR(MAX)
DECLARE @LE NVARCHAR(MAX)


set @LastTime='09:31 am'
set @PS='05:00 am'
set @PE='09:15 am'

set @LS='09:16 am'
set @LE='09:30 am'





SET @Query = ' 
  SELECT  distinct  ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ItemNo, dtl.EmpMasterCode,dtl.EmpName, com.CompanyName, '''' AreaName, ISNUll(tblPre.PreTo,0) Present, ISNUll(tblLate.LateTo,0) Late, ISNUll(tblAbset.AbsentTo,0) [Absent]
        FROM     dbo.tblEmpGeneralInfo dtl  with (nolock) 

		 
		 left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) PreTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime )  between CONVERT(time,'''+@PS+''') and CONVERT(time,'''+@PE+''')  '+  @DateRange  +'   group by att1.EmpInfoId) tblPre     ON dtl.EmpInfoId = tblPre.EmpInfoId 

		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) LateTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime ) between CONVERT(time,'''+@LS+''') and CONVERT(time,'''+@LE+''')  '+  @DateRange  +'  group by att1.EmpInfoId) tblLate     ON dtl.EmpInfoId = tblLate.EmpInfoId 

		  
		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) AbsentTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime )  >= CONVERT(time,'''+@LastTime+''') '+  @DateRange  +'   group by att1.EmpInfoId) tblAbset     ON dtl.EmpInfoId = tblAbset.EmpInfoId 


		 left JOIN dbo.tblCompanyInfo com  with (nolock) ON com.CompanyId = dtl.CompanyId 
		 

		 where (dtl.EmpMasterCode is not null or dtl.EmpMasterCode <>'''')  and (ISNUll(tblPre.PreTo,0) + ISNUll(tblLate.LateTo,0) + ISNUll(tblAbset.AbsentTo,0)>0)  '+  @param  +' 	  '
 
END

EXEC (@Query)
```

### sp_Get_MonthlyAttendanceHistory_Shift

- **Parameters:** @param (NVARCHAR(MAX), IN), @DateRange (NVARCHAR(MAX), IN), @frmDate (NVARCHAR(MAX), IN), @toDate (NVARCHAR(MAX), IN), @ShiftId (NVARCHAR(MAX), IN, default '')
- **Tables read:** ZASHRIS..tblHolidayInformation, sys.all_objects, dbo.tblEmpGeneralInfo, tblUser, ZASHRIS..tblEmpGeneralInfo, dbo.tblMarketAttendance_Master_webapi, dbo.tblCompanyInfo
- **Tables written:** none
- **Business logic:** Builds and executes a fully dynamic SQL string that computes a monthly attendance history per employee (per shift), counting Present/Late/Absent punches within a date range and computing implied Absent/Weekend/Holiday counts, filtered by app-user employees for a given ShiftId.
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH; relies on EXEC of a dynamic string with no error handling)
- **Result set:** Dynamic result set of one row per employee: ItemNo, computed Absent, Holiday, Weekend, EmpMasterCode, EmpName, CompanyName, AreaName (blank literal), Present, Late, Absent.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep across Library.DAL, Library.BLL, Solution.Web found no matches)

```sql
CREATE
 PROCEDURE [dbo].[sp_Get_MonthlyAttendanceHistory_Shift]
	-- Add the parameters for the stored procedure here

	@param NVARCHAR(max),
	@DateRange NVARCHAR(max),
	@frmDate NVARCHAR(max),
	@toDate NVARCHAR(max),
	@ShiftId  NVARCHAR(max)=''




AS
BEGIN
  

DECLARE @Query NVARCHAR(MAX)

DECLARE @LastTime NVARCHAR(MAX)
DECLARE @PS NVARCHAR(MAX)
DECLARE @PE NVARCHAR(MAX)

DECLARE @LS NVARCHAR(MAX)
DECLARE @LE NVARCHAR(MAX)


set @LastTime='09:40 am'
set @PS='05:00 am'
set @PE='09:19 am'

set @LS='09:20 am'
set @LE='09:39 am'






SET @Query = ' 
 select ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS ItemNo,  case when  Present+Late=0 then   (SELECT DATEDIFF(day,CONVERT(Date,'''+@frmDate+'''),CONVERT(Date,'''+@toDate+''')) + 1)-( Weekend+Holiday) else case when ( (SELECT DATEDIFF(day,CONVERT(Date,'''+@frmDate+'''),CONVERT(Date,'''+@toDate+''')) + 1)-((Present+Late+ [Absent])+( Weekend+Holiday)))+ [Absent]>0 then ( (SELECT DATEDIFF(day,CONVERT(Date,'''+@frmDate+'''),CONVERT(Date,'''+@toDate+''')) + 1)-((Present+Late+ [Absent])+( Weekend+Holiday)))+ [Absent] else 0 end end [Absent], * from (
  SELECT  distinct   (select count(*) from ZASHRIS..tblHolidayInformation where  CONVERT(Date, HolidayDate) between    CONVERT(Date,'''+@frmDate+''')   and  CONVERT(Date,'''+@toDate+''')) Holiday,  ( SELECT COUNT(*)
FROM (
    SELECT TOP (DATEDIFF(DAY, CONVERT(Date,'''+@frmDate+''') ,   CONVERT(Date,'''+@toDate+'''))+ 1)
        date = DATEADD(DAY, ROW_NUMBER() OVER (ORDER BY a.object_id) - 1, CONVERT(Date,'''+@frmDate+''') )
    FROM sys.all_objects AS a
    CROSS JOIN sys.all_objects AS b
) AS dates
WHERE DATEPART(WEEKDAY, date) = 6) Weekend, 0 AS ItemNo, dtl.EmpMasterCode,dtl.EmpName, com.CompanyName, '''' AreaName, ISNUll(tblPre.PreTo,0) Present, ISNUll(tblLate.LateTo,0) Late,  case when ISNUll(tblPre.PreTo,0) + ISNUll(tblLate.LateTo,0) = 0 then 0 else  ISNUll(tblAbset.AbsentTo,0) end [Absent]
        FROM     dbo.tblEmpGeneralInfo dtl  with (nolock) 
		 inner join tblUser ss on ss.EmpInfoId=dtl.EmpInfoId 
		 inner join ZASHRIS..tblEmpGeneralInfo hris on hris.EmpMasterCode=dtl.EmpMasterCode
		 left JOIN (select  att1.EmpInfoId, COUNT(att1.PunchInTime) PreTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  att1.AttStatus=''Present'' and att1.ShiftId='''+@ShiftId+''' '+  @DateRange  +'   group by att1.EmpInfoId) tblPre     ON dtl.EmpInfoId = tblPre.EmpInfoId 

		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) LateTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  att1.AttStatus=''Late'' and att1.ShiftId='''+@ShiftId+'''   '+  @DateRange  +' group by att1.EmpInfoId) tblLate     ON dtl.EmpInfoId = tblLate.EmpInfoId 

		  
		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) AbsentTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  att1.AttStatus=''Absent'' and att1.ShiftId='''+@ShiftId+''' '+  @DateRange  +'  group by att1.EmpInfoId) tblAbset     ON dtl.EmpInfoId = tblAbset.EmpInfoId 


		 left JOIN dbo.tblCompanyInfo com  with (nolock) ON com.CompanyId = dtl.CompanyId 
		 

		 where  dtl.EmpInfoId<> 1  and ss.IsAppsUser=1 and  (dtl.EmpMasterCode is not null or dtl.EmpMasterCode <>'''')  '+  @param  +' 	  ) 	   tbl'
 
END

EXEC (@Query)

 --and (ISNUll(tblPre.PreTo,0) + ISNUll(tblLate.LateTo,0) + ISNUll(tblAbset.AbsentTo,0)>0) 

```

### sp_Get_MonthlyAttendanceHistory_new

- **Parameters:** @param (NVARCHAR(MAX), IN), @DateRange (NVARCHAR(MAX), IN), @frmDate (NVARCHAR(MAX), IN), @toDate (NVARCHAR(MAX), IN)
- **Tables read:** ZASHRIS..tblHolidayInformation, sys.all_objects, dbo.tblEmpGeneralInfo, tblUser, ZASHRIS..tblEmpGeneralInfo, dbo.tblMarketAttendance_Master_webapi, dbo.tblCompanyInfo
- **Tables written:** none
- **Business logic:** Similar to sp_Get_MonthlyAttendanceHistory_Shift but without shift filtering; classifies attendance as Present/Late/Absent based on fixed punch-in time windows (@PS/@PE for Present, @LS/@LE for Late, after @LastTime for Absent) and computes derived Weekend/Holiday/Absent counts per employee over a date range via dynamic SQL.
- **Transaction/error handling:** none
- **Result set:** Dynamic result set per employee: computed Absent, Holiday, Weekend, ItemNo, EmpMasterCode, EmpName, CompanyName, AreaName (blank literal), Present, Late, Absent.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

CREATE
 PROCEDURE [dbo].[sp_Get_MonthlyAttendanceHistory_new]
	-- Add the parameters for the stored procedure here

	@param NVARCHAR(max),
	@DateRange NVARCHAR(max),
	@frmDate NVARCHAR(max),
	@toDate NVARCHAR(max)




AS
BEGIN
  

DECLARE @Query NVARCHAR(MAX)

DECLARE @LastTime NVARCHAR(MAX)
DECLARE @PS NVARCHAR(MAX)
DECLARE @PE NVARCHAR(MAX)

DECLARE @LS NVARCHAR(MAX)
DECLARE @LE NVARCHAR(MAX)


set @LastTime='09:40 am'
set @PS='05:00 am'
set @PE='09:19 am'

set @LS='09:20 am'
set @LE='09:39 am'






SET @Query = ' 
 select case when  Present+Late=0 then   (SELECT DATEDIFF(day,CONVERT(Date,'''+@frmDate+'''),CONVERT(Date,'''+@toDate+''')) + 1)-( Weekend+Holiday) else case when ( (SELECT DATEDIFF(day,CONVERT(Date,'''+@frmDate+'''),CONVERT(Date,'''+@toDate+''')) + 1)-((Present+Late+ [Absent])+( Weekend+Holiday)))+ [Absent]>0 then ( (SELECT DATEDIFF(day,CONVERT(Date,'''+@frmDate+'''),CONVERT(Date,'''+@toDate+''')) + 1)-((Present+Late+ [Absent])+( Weekend+Holiday)))+ [Absent] else 0 end end [Absent], * from (
  SELECT  distinct   (select count(*) from ZASHRIS..tblHolidayInformation where  CONVERT(Date, HolidayDate) between    CONVERT(Date,'''+@frmDate+''')   and  CONVERT(Date,'''+@toDate+''')) Holiday,  ( SELECT COUNT(*)
FROM (
    SELECT TOP (DATEDIFF(DAY, CONVERT(Date,'''+@frmDate+''') ,   CONVERT(Date,'''+@toDate+'''))+ 1)
        date = DATEADD(DAY, ROW_NUMBER() OVER (ORDER BY a.object_id) - 1, CONVERT(Date,'''+@frmDate+''') )
    FROM sys.all_objects AS a
    CROSS JOIN sys.all_objects AS b
) AS dates
WHERE DATEPART(WEEKDAY, date) = 6) Weekend,  ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ItemNo, dtl.EmpMasterCode,dtl.EmpName, com.CompanyName, '''' AreaName, ISNUll(tblPre.PreTo,0) Present, ISNUll(tblLate.LateTo,0) Late,  case when ISNUll(tblPre.PreTo,0) + ISNUll(tblLate.LateTo,0) = 0 then 0 else  ISNUll(tblAbset.AbsentTo,0) end [Absent]
        FROM     dbo.tblEmpGeneralInfo dtl  with (nolock) 
		 inner join tblUser ss on ss.EmpInfoId=dtl.EmpInfoId  and ss.IsAppsUser=1
		 inner join ZASHRIS..tblEmpGeneralInfo hris on hris.EmpMasterCode=dtl.EmpMasterCode
		 left JOIN (select  att1.EmpInfoId, COUNT(att1.PunchInTime) PreTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime )  between CONVERT(time,'''+@PS+''') and CONVERT(time,'''+@PE+''')  '+  @DateRange  +'   group by att1.EmpInfoId) tblPre     ON dtl.EmpInfoId = tblPre.EmpInfoId 

		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) LateTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime ) between CONVERT(time,'''+@LS+''') and CONVERT(time,'''+@LE+''')  '+  @DateRange  +'  group by att1.EmpInfoId) tblLate     ON dtl.EmpInfoId = tblLate.EmpInfoId 

		  
		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) AbsentTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime )  >= CONVERT(time,'''+@LastTime+''') '+  @DateRange  +'   group by att1.EmpInfoId) tblAbset     ON dtl.EmpInfoId = tblAbset.EmpInfoId 


		 left JOIN dbo.tblCompanyInfo com  with (nolock) ON com.CompanyId = dtl.CompanyId 
		 

		 where  dtl.EmpInfoId<> 1 and  (dtl.EmpMasterCode is not null or dtl.EmpMasterCode <>'''')  '+  @param  +' 	  ) 	   tbl'
 
END

EXEC (@Query)

 --and (ISNUll(tblPre.PreTo,0) + ISNUll(tblLate.LateTo,0) + ISNUll(tblAbset.AbsentTo,0)>0) 

```

### sp_Get_Order_Info_WebAPI_NEw

- **Parameters:** @customerCode (NVARCHAR(50), IN, default NULL)
- **Tables read:** dbo.tblOrder
- **Tables written:** none
- **Business logic:** Returns the latest 25 orders for a given customer code, with a derived OrderStatus label ('Invoiced'/'Pending') based on the IsInvoice flag.
- **Transaction/error handling:** none
- **Result set:** Top 25 rows: OrderId, OrderCode, MIOName, CustomerName, GrossValue, SubmissionDate (formatted), OrderStatus.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
CREATE PROCEDURE [dbo].[sp_Get_Order_Info_WebAPI_NEw]
	-- Add the parameters for the stored procedure here
@customerCode NVARCHAR(50) = null
AS
BEGIN


SELECT TOP 25  OrderId,
		OrderCode,
		MIOName,
		CustomerName,
		GrossValue,
		CONVERT(NVARCHAR(50),SubmissionDate,106) AS SubmissionDate,

		CASE 
			WHEN IsInvoice = 1 THEN 'Invoiced'
			WHEN IsInvoice = 0 THEN 'Pending'
			END AS OrderStatus
FROM    dbo.tblOrder
WHERE   CustomerCode = @customerCode 


END

```

### sp_Get_Order_Info_WebAPI_NEw_MIo

- **Parameters:** @empId (int, IN, default NULL)
- **Tables read:** dbo.tblEmpGeneralInfo, dbo.tblOrder, dbo.tblInvoice
- **Tables written:** none
- **Business logic:** Resolves an employee's MIOCode from EmpInfoId, then returns the latest 150 app-originated ("IsFromApp = 1") own orders for that MIO, deriving an order status ('Payment Completed' / 'Delivery Completed' / 'Invoiced' / 'Invoice Pending' / 'Pending') via correlated subqueries against tblInvoice.
- **Transaction/error handling:** none
- **Result set:** Top 150 rows: OrderId, OrderCode, MIOName, CustomerName, GrossValue, SubmissionDate (formatted), dsd (invoice PaymentStatus subquery), OrderType ('Own'), OrderStatus, ordered by OrderId descending.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
CREATE PROCEDURE [dbo].[sp_Get_Order_Info_WebAPI_NEw_MIo] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
@empId int = null
AS
BEGIN

		DECLARE @mioCode NVARCHAR(50)

        SELECT  @mioCode = A.EmpMasterCode
        FROM    dbo.tblEmpGeneralInfo A
        WHERE   A.EmpInfoId = @empId 




SELECT TOP 150  
		D.OrderId,
		D.OrderCode,
		D.MIOName,
		D.CustomerName,
		D.GrossValue,
		CONVERT(NVARCHAR(50),D.SubmissionDate,106) AS SubmissionDate,
		( SELECT  PaymentStatus
                            FROM    dbo.tblInvoice
                            WHERE   OrderId = D.OrderId
                          ) AS dsd,
		'Own'  AS OrderType,
		 CASE		WHEN ( SELECT  PaymentStatus
                            FROM    dbo.tblInvoice
                            WHERE   OrderId = D.OrderId
                          ) IS NOT NULL THEN 'Payment Completed'

						  WHEN ( SELECT  DelivaryInvoiceNo
                            FROM    dbo.tblInvoice
                            WHERE   OrderId = D.OrderId
                          ) IS NOT NULL THEN 'Delivery Completed'

					WHEN IsInvoice = 1 THEN 'Invoiced'
					WHEN IsInvoice = 0 THEN 'Invoice Pending'
					WHEN IsInvoice IS NULL THEN 'Pending'
					END AS OrderStatus

FROM    dbo.tblOrder D
			WHERE D.MIOCode = @mioCode 
			AND IsFromApp = 1
			ORDER BY OrderId DESC


END

```

### sp_Get_Order_Info_WebAPI_NEw_MIo_ByMonthYear

- **Parameters:** @empId (int, IN, default NULL), @month (int, IN, default NULL), @year (int, IN, default NULL)
- **Tables read:** dbo.tblEmpGeneralInfo, dbo.tblOrder
- **Tables written:** none
- **Business logic:** Resolves an employee's MIOCode from EmpInfoId, then returns up to 100 app-originated own orders for that MIO filtered to a specific month/year of SubmissionDate, with a derived OrderStatus ('Invoiced'/'Invoice Pending').
- **Transaction/error handling:** none
- **Result set:** Top 100 rows: OrderId, OrderCode, MIOName, CustomerName, GrossValue, SubmissionDate (formatted), OrderType ('Own'), OrderStatus, ordered by OrderId descending.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
CREATE PROCEDURE [dbo].[sp_Get_Order_Info_WebAPI_NEw_MIo_ByMonthYear]
	-- Add the parameters for the stored procedure here
@empId int = NULL,
@month INT = NULL,
@year INT = null
AS
BEGIN

		DECLARE @mioCode NVARCHAR(50)

        SELECT  @mioCode = A.EmpMasterCode
        FROM    dbo.tblEmpGeneralInfo A
        WHERE   A.EmpInfoId = @empId 





SELECT TOP 100  OrderId,
		OrderCode,
		MIOName,
		CustomerName,
		GrossValue,
		CONVERT(NVARCHAR(50),SubmissionDate,106) AS SubmissionDate,
		'Own'  AS OrderType,
		 CASE 
					WHEN IsInvoice = 1 THEN 'Invoiced'
					WHEN IsInvoice = 0 THEN 'Invoice Pending'
					END AS OrderStatus

FROM    dbo.tblOrder
			WHERE MIOCode = @mioCode 
			AND DATEPART(yy,SubmissionDate) = @year AND DATEPART(MM,SubmissionDate) = @month
			AND IsFromApp = 1
			ORDER BY OrderId DESC


END

```

### sp_Get_PrescriptionDetails_ByPrescriptionId

- **Parameters:** @id (int, IN)
- **Tables read:** tbl_PrescriptionProductDetail, tblProduct
- **Tables written:** none
- **Business logic:** Returns the product-level line items of a single prescription (by PrescriptionId), joined to tblProduct for the product name.
- **Transaction/error handling:** none
- **Result set:** PresDetailId, PrescriptionId, ProductId, ProductName for all detail rows of the given prescription.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
Create PROCEDURE [dbo].[sp_Get_PrescriptionDetails_ByPrescriptionId]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN
	      
   Select dt.PresDetailId, dt.PrescriptionId, dt.ProductId, pro.ProductName from tbl_PrescriptionProductDetail dt 
   left join tblProduct pro on pro.ProductId = dt.ProductId
   where PrescriptionId= @id

    END



```

### sp_Get_PrescriptionList

- **Parameters:** @param (NVARCHAR(MAX), IN) — appended filter clause for the dynamic WHERE
- **Tables read:** dbo.tbl_ImagePath_Setting, tbl_PrescriptionMaster, tbl_PrescriptionType, tblDoctorMaster, tblUser, tblEmpGeneralInfo, dbo.tbl_UserRoleInfo
- **Tables written:** none
- **Business logic:** Builds and executes dynamic SQL returning a list of prescriptions with a derived prescription image path, a human-readable approval status label ('Pending'/'Verified'/'Approved'/'Rejected'), doctor name, entry-user role and name, filtered/sorted by the caller-supplied @param fragment and ordered by prescription date descending.
- **Transaction/error handling:** none
- **Result set:** Dynamic result set: ImageString, ApprovalStatus, PrescriptionId, PrescriptionDate, PrescriptionType, DoctorName, RoleName, createBy, SMCType_RX, plus all columns (`*`) from tbl_PrescriptionMaster and joined tables.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_PrescriptionList]
	-- Add the parameters for the stored procedure here

		@param NVARCHAR(max)
AS
BEGIN
 
    DECLARE @Query NVARCHAR(MAX)

 SET @Query = 'Select (SELECT LTRIM(RTRIM(ImagePath+''/''+ImagePreName)) FROM dbo.tbl_ImagePath_Setting  with (nolock)  WHERE ImageType=''PrescriptionMy'')+CAST(PM.PrescriptionId as nvarchar(max))+''.jpg'' AS   ImageString,  case when PM.ApprovalStatus=''0'' then ''Pending''  when PM.ApprovalStatus=''1'' then ''Verified''  when PM.ApprovalStatus=''2'' then ''Approved''  WHEN PM.ApprovalStatus=''3'' then ''Rejected''  else PM.ApprovalStatus end ApprovalStatus,PM.PrescriptionId,FORMAT(PM.EntryDate,''dd-MMM-yyyy hh:mm tt'') PrescriptionDate, PT.PrescriptionType,DM.DoctorCode+'' : ''+ DM.DoctorName DoctorName, usr.RoleName, em.EmpMasterCode+'' : ''+em.EmpName createBy, PM.SMCType_RX, * from tbl_PrescriptionMaster PM with (nolock)
   Left join tbl_PrescriptionType PT  with (nolock) On PM.PrescriptionTypeId= PT.PrescriptionTypeId
   Left join tblDoctorMaster DM  with (nolock) ON PM.DoctorId = DM.DoctorId
   Left Join tblUser Us  with (nolock) ON PM.EntryBy= Us.UserId
   Left Join tblEmpGeneralInfo em  with (nolock) ON em.EmpInfoId= Us.EmpInfoId
  -- left JOIN View_Webapi_EmployeeFieldForceInfo on View_Webapi_EmployeeFieldForceInfo.EmpInfoId=em.EmpInfoId
   
left JOIN dbo.tbl_UserRoleInfo usr  with (nolock) ON usr.UserRoleID = us.UserRoleID
where PM.PrescriptionId is not null  
'+  @param  +' order by  PM.PrescriptionDate desc'
 
END

EXEC (@Query)

 

```

### sp_Get_PrescriptionTypeList

- **Parameters:** none
- **Tables read:** tbl_PrescriptionType
- **Tables written:** none
- **Business logic:** Lists all prescription types with formatted active/inactive date strings.
- **Transaction/error handling:** none
- **Result set:** PrescriptionTypeId, PrescriptionType, IsActive, Activedate, ActivedateString (both formatted from ActiveInactiveDate).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_PrescriptionTypeList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
   
 SELECT A.PrescriptionTypeId,A.PrescriptionType,A.IsActive, CONVERT(NVARCHAR(50),A.ActiveInactiveDate,106)AS Activedate, CONVERT(NVARCHAR(50),A.ActiveInactiveDate,106)AS ActivedateString  from tbl_PrescriptionType A

END



```

### sp_Get_PrescriptionType_ById

- **Parameters:** @id (int, IN)
- **Tables read:** dbo.tbl_PrescriptionType
- **Tables written:** none
- **Business logic:** Returns the full detail row of a single prescription type by id, including audit fields (EntryBy/EntryDate/UpdateBy/UpdateDate).
- **Transaction/error handling:** none
- **Result set:** PrescriptionTypeId, PrescriptionType, IsActive, ActiveInactiveDate, EntryBy, EntryDate, UpdateBy, UpdateDate for the matching row.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
CREATE PROCEDURE [dbo].[sp_Get_PrescriptionType_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

         SELECT A.PrescriptionTypeId ,
          A.PrescriptionType ,   
          A.IsActive ,
          A.ActiveInactiveDate ,
          A.EntryBy ,
          A.EntryDate,
		  A.UpdateBy,
		  A.UpdateDate  	  
		  FROM [dbo].[tbl_PrescriptionType] A
				WHERE A.PrescriptionTypeId = @id
    END



```

### sp_Get_PrescriptionType_For_DDL

- **Parameters:** none
- **Tables read:** tbl_PrescriptionType
- **Tables written:** none
- **Business logic:** Returns all rows of tbl_PrescriptionType (an inactive-filter is present but commented out), intended to populate a dropdown list.
- **Transaction/error handling:** none
- **Result set:** All columns (`*`) from tbl_PrescriptionType.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Get_PrescriptionType_For_DDL]
	-- Add the parameters for the stored procedure here

AS
BEGIN
    
	SELECT * FROM tbl_PrescriptionType 
	
	--SELECT * FROM tbl_PrescriptionType WHERE IsActive = 1

END



```

### sp_Get_Prescription_ByPrescriptionId

- **Parameters:** @id (int, IN)
- **Tables read:** tbl_PrescriptionMaster, tbl_ImagePath_Setting
- **Tables written:** none
- **Business logic:** Returns a single prescription's master record (via a CROSS JOIN-style comma join filtered by ImagePathId = 4) with derived prescription image file path fields.
- **Transaction/error handling:** none
- **Result set:** PrescripImage (derived path), ImagePreName (derived path from tbl_ImagePath_Setting), plus all columns (`*`) from tbl_PrescriptionMaster for the matching PrescriptionId.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_Prescription_ByPrescriptionId]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

        SELECT  '../Prescription' + '/' + ImagePreName
                + CAST(mas.PrescriptionId AS NVARCHAR(MAX)) + '.jpg' PrescripImage ,
				(SELECT LTRIM(RTRIM(ImagePath+'\'+ImagePreName)) FROM dbo.tbl_ImagePath_Setting WHERE ImageType='Prescription')AS ImagePreName,

                *
        FROM    tbl_PrescriptionMaster mas ,
                tbl_ImagePath_Setting img
        WHERE   img.ImagePathId = 4
                AND PrescriptionId = @id

    END



```

### sp_Get_Product_List

- **Parameters:** none
- **Tables read:** dbo.tblProduct
- **Tables written:** none
- **Business logic:** Returns a simple product picklist (ProductId + "Code : Name" label), ordered alphabetically by product name.
- **Transaction/error handling:** none
- **Result set:** ProductId, ProductName (concatenated "ProductCode : ProductName").
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
CREATE PROCEDURE [dbo].[sp_Get_Product_List]
 
AS
BEGIN
	
	SELECT j.ProductId, j.ProductCode+' : '+ j.ProductName  ProductName		  FROM dbo.tblProduct j with (nolock)  order by j.ProductName asc  

END

```

### sp_Get_UserLocationTrackList

- **Parameters:** @param (NVARCHAR(MAX), IN) — appended filter clause, @Date (NVARCHAR(MAX), IN, unused in the query body)
- **Tables read:** tbl_UserTracking
- **Tables written:** none
- **Business logic:** Builds and executes dynamic SQL returning a distinct list of user GPS tracking points with a Google Maps link (LatLong) and a derived AttType label ("Break(reason)"/"Break"/"Active" or the stored AttTypeLoc), filtered by the caller-supplied @param fragment and non-null address, ordered by TrackDate ascending.
- **Transaction/error handling:** none
- **Result set:** Dynamic: Time, LatLong (Google Maps URL), AttType, AddressName, TrackDate.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
-- =============================================
-- Author:		<Author,,Tareq>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_UserLocationTrackList]
	-- Add the parameters for the stored procedure here
		@param NVARCHAR(max),
		@Date NVARCHAR(max)

AS
BEGIN
 
    DECLARE @Query NVARCHAR(MAX)

 SET @Query = 'SELECT DISTINCT Time,  ''http://maps.google.com/?q=''+ tc.LatValue +'',''+tc.LongValue LatLong,CASE WHEN AttTypeLoc IS NULL THEN  
CASE WHEN ActiveStatus=0 THEN CASE WHEN  breakReason IS NULL  THEN ''Break'' ELSE ''Break(''+breakReason+'')'' END ELSE ''Active'' END  ELSE  AttTypeLoc END  AttType, tc.AddressName, TrackDate from tbl_UserTracking tc where EmpInfoId is not null  '+  @param  + ' AND  tc.AddressName IS NOT NULL order by TrackDate asc '
 
END

EXEC (@Query)



```

### sp_Get_UserLocationTracking

- **Parameters:** @empid (int, IN) — actually EmpInfoId, @trackDate (datetime, IN)
- **Tables read:** dbo.tbl_UserTracking
- **Tables written:** none
- **Business logic:** Returns the distinct GPS tracking points recorded for a given employee on a given calendar date.
- **Transaction/error handling:** none
- **Result set:** TrackingId, EmpInfoId, LatValue, LongValue, AddressName, Time (formatted HH:MM:SS), TrackDate.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

CREATE PROCEDURE [dbo].[sp_Get_UserLocationTracking] 
	-- Add the parameters for the stored procedure here
@empid INT, -- this is actully empId
@trackDate DATETIME
AS
BEGIN


--DECLARE @empIdMain INT

--SELECT @empIdMain = EmpInfoId FROM dbo.tblUser WHERE UserId=@empid
	
	SELECT DISTINCT TrackingId ,
           EmpInfoId ,
           LatValue ,
           LongValue ,
           AddressName ,
			 CONVERT(varchar(15),  CAST(Time AS TIME), 100)   AS Time   ,
           TrackDate FROM dbo.tbl_UserTracking
	WHERE EmpInfoId = @empid AND CONVERT(date,TrackDate) = CONVERT(date,@trackDate) 


END

```

### sp_Get_UserRoleInfo

- **Parameters:** none
- **Tables read:** tbl_UserRoleInfo
- **Tables written:** none
- **Business logic:** Returns all active user role records.
- **Transaction/error handling:** none
- **Result set:** All columns (`*`) from tbl_UserRoleInfo where IsActive = 1.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
create PROCEDURE [dbo].[sp_Get_UserRoleInfo]
	-- Add the parameters for the stored procedure here

AS
BEGIN
    

	Select * from tbl_UserRoleInfo A  WITH (NOLOCK) where IsActive=1


END


```

### sp_Get_UserTypeInfo

- **Parameters:** none
- **Tables read:** tblUserType
- **Tables written:** none
- **Business logic:** Returns all user type records (the IsActive filter is present in the code but commented out, so all rows including inactive ones are returned).
- **Transaction/error handling:** none
- **Result set:** All columns (`*`) from tblUserType.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

create PROCEDURE [dbo].[sp_Get_UserTypeInfo]
	-- Add the parameters for the stored procedure here

AS
BEGIN
    

	Select  * from tblUserType A  WITH (NOLOCK) --where IsActive=1


END



```

### sp_Get_Zone_AllByGroup

- **Parameters:** @GroupId (int, IN)
- **Tables read:** dbo.tblRegion
- **Tables written:** none
- **Business logic:** Returns regions ("zones") for a given group, labeling inactive regions with an "(Inactive)" suffix in the display name.
- **Transaction/error handling:** none
- **Result set:** RegionId, RegionName (concatenated "RegionCode : RegionName", with "(Inactive)" suffix when not active).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_Zone_AllByGroup]
	-- Add the parameters for the stored procedure here

	@GroupId INT

AS
BEGIN
	

	SELECT RegionId,RegionCode+' : '+CASE WHEN IsActive=1 THEN   RegionName  ELSE   RegionName+' (Inactive)' END  RegionName  FROM dbo.tblRegion WITH (NOLOCK) WHERE   GroupId = @GroupId

END



```

### sp_Get_Zone_All_Active

- **Parameters:** @GroupId (int, IN)
- **Tables read:** dbo.tblRegion
- **Tables written:** none
- **Business logic:** Returns only active regions ("zones") for a given group.
- **Transaction/error handling:** none
- **Result set:** RegionId, RegionName (concatenated "RegionCode : RegionName") for active regions in the group.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Get_Zone_All_Active]
	-- Add the parameters for the stored procedure here

	@GroupId INT

AS
BEGIN
	

	SELECT RegionId,RegionCode+' : '+ RegionName RegionName FROM dbo.tblRegion  with (nolock) WHERE IsActive = 1 and GroupId = @GroupId

END



```

### sp_Get_doctorCateList

- **Parameters:** none
- **Tables read:** tblDoctorCategory
- **Tables written:** none
- **Business logic:** Returns all active doctor category records.
- **Transaction/error handling:** none
- **Result set:** All columns (`*`) from tblDoctorCategory where IsActive = 1.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Get_doctorCateList]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	

	SELECT * FROM tblDoctorCategory WHERE IsActive = 1

END



```

### sp_INV_DCStockValuationReport

- **Parameters:** @FromDate (DATETIME, IN), @ToDate (DATETIME, IN), @CompanyId (int, IN), @WarehouseId (int, IN, default NULL — NULL/0 = All), @ProductId (int, IN, default NULL — NULL/0 = All)
- **Tables read:** dbo.tbl_INV_WeightedAvg, dbo.tblProduct
- **Tables written:** none
- **Business logic:** Computes a DC (distribution center) stock valuation report per warehouse/product using the weighted-average costing table: derives opening balance (last transaction before the from-date), movement (in/out quantities within the date range), and closing balance (last transaction on/before the to-date), then aggregates the results at the company level and only shows items with non-zero opening, movement, or closing.
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH; SET NOCOUNT ON only; has basic input validation with early RETURN for null/invalid dates)
- **Result set:** Item_Id, ProductCode, ProductName, PackSize, OpeningQty, OpeningRate, OpeningValue, InQty, OutQty, ClosingQty, ClosingRate, ClosingValue, ordered by ProductCode.
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs:121 (GetDataTableAction-style call construction)

```sql

CREATE PROCEDURE [dbo].[sp_INV_DCStockValuationReport]
(
      @FromDate    DATETIME
    , @ToDate      DATETIME
    , @CompanyId   INT
    , @WarehouseId INT = NULL     -- NULL = All
    , @ProductId   INT = NULL     -- NULL = All
)
AS
BEGIN
    SET NOCOUNT ON;

    ---------------------------------------------------------
    -- Basic Validation
    ---------------------------------------------------------
    IF (@FromDate IS NULL OR @ToDate IS NULL OR @CompanyId IS NULL)
        RETURN;

    IF (CONVERT(DATE,@FromDate) > CONVERT(DATE,@ToDate))
        RETURN;

    ---------------------------------------------------------
    -- Convert 0 to NULL (Frontend sends 0)
    ---------------------------------------------------------
    IF @WarehouseId = 0 SET @WarehouseId = NULL;
    IF @ProductId   = 0 SET @ProductId   = NULL;

    DECLARE @FromD DATE = CONVERT(DATE, @FromDate);
    DECLARE @ToD   DATE = CONVERT(DATE, @ToDate);

    ---------------------------------------------------------
    -- CTE Section
    ---------------------------------------------------------
    ;WITH Base AS
    (
        SELECT
              WA.Company_Id
            , WA.Warehouse_Id
            , WA.Item_Id
            , WA.Trans_Date
            , WA.Weighted_Avg_Id
            , WA.Curr_Qty
            , WA.Close_Qty
            , WA.Avg_Rate
            , WA.Close_Amt
        FROM dbo.tbl_INV_WeightedAvg WA
        WHERE WA.Company_Id = @CompanyId
          AND WA.Warehouse_Id IS NOT NULL
          AND WA.Item_Id IS NOT NULL
          AND (@WarehouseId IS NULL OR WA.Warehouse_Id = @WarehouseId)
          AND (@ProductId   IS NULL OR WA.Item_Id      = @ProductId)
          AND WA.Trans_Date < DATEADD(DAY, 1, @ToD)
    ),
    OpeningKey AS
    (
        SELECT
              Warehouse_Id, Item_Id,
              MAX(CAST(Weighted_Avg_Id AS BIGINT)) AS OpeningWAId
        FROM Base
        WHERE Trans_Date < @FromD
        GROUP BY Warehouse_Id, Item_Id
    ),
    ClosingKey AS
    (
        SELECT
              Warehouse_Id, Item_Id,
              MAX(CAST(Weighted_Avg_Id AS BIGINT)) AS ClosingWAId
        FROM Base
        WHERE Trans_Date < DATEADD(DAY, 1, @ToD)
        GROUP BY Warehouse_Id, Item_Id
    ),
    Move AS
    (
        SELECT
              Warehouse_Id, Item_Id,
              SUM(CASE WHEN Curr_Qty > 0 THEN Curr_Qty ELSE 0 END)  AS InQty,
              SUM(CASE WHEN Curr_Qty < 0 THEN -Curr_Qty ELSE 0 END) AS OutQty
        FROM Base
        WHERE Trans_Date >= @FromD
          AND Trans_Date <  DATEADD(DAY, 1, @ToD)
        GROUP BY Warehouse_Id, Item_Id
    ),
    Items AS
    (
        SELECT Warehouse_Id, Item_Id FROM OpeningKey
        UNION
        SELECT Warehouse_Id, Item_Id FROM ClosingKey
        UNION
        SELECT Warehouse_Id, Item_Id FROM Move
    )

    ---------------------------------------------------------
    -- Final Company Wise Aggregated Result
    ---------------------------------------------------------
    SELECT 
          it.Item_Id
        , p.ProductCode
        , p.ProductName
        , p.PackSize

        -- Opening
        , SUM(ISNULL(op.Close_Qty, 0)) AS OpeningQty
        , CASE 
            WHEN SUM(ISNULL(op.Close_Qty, 0)) = 0 THEN 0
            ELSE SUM(ISNULL(op.Close_Amt, 0)) 
                 / SUM(ISNULL(op.Close_Qty, 0))
          END AS OpeningRate
        , SUM(ISNULL(op.Close_Amt, 0)) AS OpeningValue

        -- Movement
        , SUM(ISNULL(mv.InQty,  0)) AS InQty
        , SUM(ISNULL(mv.OutQty, 0)) AS OutQty

        -- Closing
        , SUM(ISNULL(cl.Close_Qty, 0)) AS ClosingQty
        , CASE 
            WHEN SUM(ISNULL(cl.Close_Qty, 0)) = 0 THEN 0
            ELSE SUM(ISNULL(cl.Close_Amt, 0)) 
                 / SUM(ISNULL(cl.Close_Qty, 0))
          END AS ClosingRate
        , SUM(ISNULL(cl.Close_Amt, 0)) AS ClosingValue

    FROM Items it

    LEFT JOIN OpeningKey ok
           ON ok.Warehouse_Id = it.Warehouse_Id
          AND ok.Item_Id      = it.Item_Id

    LEFT JOIN Base op
           ON op.Warehouse_Id    = ok.Warehouse_Id
          AND op.Item_Id         = ok.Item_Id
          AND op.Weighted_Avg_Id = ok.OpeningWAId

    LEFT JOIN Move mv
           ON mv.Warehouse_Id = it.Warehouse_Id
          AND mv.Item_Id      = it.Item_Id

    LEFT JOIN ClosingKey ck
           ON ck.Warehouse_Id = it.Warehouse_Id
          AND ck.Item_Id      = it.Item_Id

    LEFT JOIN Base cl
           ON cl.Warehouse_Id    = ck.Warehouse_Id
          AND cl.Item_Id         = ck.Item_Id
          AND cl.Weighted_Avg_Id = ck.ClosingWAId

    LEFT JOIN dbo.tblProduct p
           ON p.ProductId = it.Item_Id
          AND p.CompanyId = @CompanyId

    WHERE
        ISNULL(op.Close_Qty,0) > 0
        OR ISNULL(mv.InQty,0)  > 0
        OR ISNULL(mv.OutQty,0) > 0
        OR ISNULL(cl.Close_Qty,0) > 0

    GROUP BY 
          it.Item_Id
        , p.ProductCode
        , p.ProductName
        , p.PackSize

    ORDER BY p.ProductCode;

END


```

### sp_INV_WHInventoryValuationReport

- **Parameters:** @FromDate (date, IN), @ToDate (date, IN), @CompanyId (int, IN), @ProductId (int, IN, default NULL)
- **Tables read:** dbo.tblProduct, dbo.tbl_INV_CentralStore_WeightedAvg
- **Tables written:** none
- **Business logic:** Computes a central-store (warehouse) inventory valuation report per product using the central-store weighted-average table: opening balance (last row before FromDate), in/out quantity movement within the range, and closing balance (last row on/before ToDate), returning only items with non-zero opening, in, out, or closing values.
- **Transaction/error handling:** none (SET NOCOUNT ON only; no TRAN/TRY-CATCH)
- **Result set:** ProductId, ProductCode, ProductName, PackSize, OP_Qty, OP_Rate, OP_Amt, TotalIN, TotalOut, Close_Qty, Avg_Rate, Close_Amt, ordered by ProductCode then ProductName.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
CREATE   PROCEDURE [dbo].[sp_INV_WHInventoryValuationReport]
(
      @FromDate  date
    , @ToDate    date
    , @CompanyId int
    , @ProductId int = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    /* ? CHANGE THIS COLUMN NAME if needed */
    /* Replace WG.TranDate with your actual date column in WeightedAvg table */

    ;WITH ProductBase AS
    (
        SELECT
              P.ProductId
            , P.ProductCode
            , P.ProductName
            , P.PackSize
        FROM dbo.tblProduct P
        WHERE (@ProductId IS NULL OR P.ProductId = @ProductId)
    ),
    Opening AS
    (
        SELECT
              PB.ProductId AS Item_Id
            , X.Close_Qty AS OP_Qty
            , X.Avg_Rate  AS OP_Rate
            , X.Close_Amt AS OP_Amt
        FROM ProductBase PB
        OUTER APPLY
        (
            SELECT TOP (1)
                   WG.Close_Qty, WG.Avg_Rate, WG.Close_Amt
            FROM dbo.tbl_INV_CentralStore_WeightedAvg WG
            WHERE WG.Company_Id = @CompanyId
              AND WG.Item_Id    = PB.ProductId
              AND WG.Trans_Date   < @FromDate
            ORDER BY WG.Weighted_Avg_Id DESC
        ) X
    ),
    InQty AS
    (
        SELECT
              WG.Item_Id
            , SUM(WG.Curr_Qty) AS TotalIn
        FROM dbo.tbl_INV_CentralStore_WeightedAvg WG
        WHERE WG.Company_Id = @CompanyId
          AND WG.Trans_Date  >= @FromDate
          AND WG.Trans_Date  <  DATEADD(DAY, 1, @ToDate)
          AND WG.Curr_Qty  >  0
        GROUP BY WG.Item_Id
    ),
    OutQty AS
    (
        SELECT
              WG.Item_Id
            , SUM(WG.Curr_Qty) AS TotalOut  -- negative
        FROM dbo.tbl_INV_CentralStore_WeightedAvg WG
        WHERE WG.Company_Id = @CompanyId
          AND WG.Trans_Date  >= @FromDate
          AND WG.Trans_Date  <  DATEADD(DAY, 1, @ToDate)
          AND WG.Curr_Qty  <  0
        GROUP BY WG.Item_Id
    ),
    Closing AS
    (
        SELECT
              PB.ProductId AS Item_Id
            , X.Close_Qty AS Close_Qty
            , X.Avg_Rate  AS Avg_Rate
            , X.Close_Amt AS Close_Amt
        FROM ProductBase PB
        OUTER APPLY
        (
            SELECT TOP (1)
                   WG.Close_Qty, WG.Avg_Rate, WG.Close_Amt
            FROM dbo.tbl_INV_CentralStore_WeightedAvg WG
            WHERE WG.Company_Id = @CompanyId
              AND WG.Item_Id    = PB.ProductId
              AND WG.Trans_Date  <= @ToDate
            ORDER BY WG.Weighted_Avg_Id DESC
        ) X
    )
    SELECT
          PB.ProductId
        , PB.ProductCode
        , PB.ProductName
        , PB.PackSize

        , ISNULL(OP.OP_Qty, 0)     AS OP_Qty
        , ISNULL(OP.OP_Rate, 0)    AS OP_Rate
        , ISNULL(OP.OP_Amt, 0)     AS OP_Amt

        , ISNULL(TIN.TotalIn, 0)   AS TotalIN
        , ISNULL(TOT.TotalOut * -1, 0) AS TotalOut

        , ISNULL(CS.Close_Qty, 0)  AS Close_Qty
        , ISNULL(CS.Avg_Rate, 0)   AS Avg_Rate
        , ISNULL(CS.Close_Amt, 0)  AS Close_Amt
    FROM ProductBase PB
    LEFT JOIN Opening OP ON PB.ProductId = OP.Item_Id
    LEFT JOIN InQty   TIN ON PB.ProductId = TIN.Item_Id
    LEFT JOIN OutQty  TOT ON PB.ProductId = TOT.Item_Id
    LEFT JOIN Closing CS ON PB.ProductId = CS.Item_Id
    WHERE ISNULL(OP.OP_Qty,0) > 0
       OR ISNULL(TIN.TotalIn,0) > 0
       OR ISNULL(TOT.TotalOut * -1,0) > 0
       OR ISNULL(CS.Close_Qty,0) > 0
    ORDER BY PB.ProductCode ASC, PB.ProductName ASC;
END


```

### sp_INV_WHStockValuationReport

- **Parameters:** @FromDate (DATETIME, IN), @ToDate (DATETIME, IN), @CompanyId (int, IN), @ProductId (int, IN, default NULL — NULL = all)
- **Tables read:** dbo.tbl_INV_CentralStore_WeightedAvg, dbo.tblProduct
- **Tables written:** none
- **Business logic:** Computes a warehouse (central store) stock valuation report per product for a company: opening balance (last transaction before FromDate), in/out movement within the range, and closing balance (last transaction on/before ToDate), returning only products with non-zero opening, movement, or closing values.
- **Transaction/error handling:** none
- **Result set:** ProductCode, ProductName, PackSize, OpeningQty, OpeningRate, OpeningValue, InQty, OutQty, ClosingQty, ClosingRate, ClosingValue, ordered by ProductCode.
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs:75 (GetDataTableAction("sp_INV_WHStockValuationReport", ...))

```sql
CREATE   PROCEDURE [dbo].[sp_INV_WHStockValuationReport]
(
      @FromDate  DATETIME
    , @ToDate    DATETIME
    , @CompanyId INT
    , @ProductId INT = NULL   -- NULL = all
)
AS
BEGIN

    ;WITH Base AS
    (
        SELECT
              WG.Company_Id
            , WG.Item_Id
            , WG.Trans_Date
            , WG.Weighted_Avg_Id
            , WG.Curr_Qty
            , WG.Close_Qty
            , WG.Avg_Rate
            , WG.Close_Amt
        FROM dbo.tbl_INV_CentralStore_WeightedAvg WG
        WHERE WG.Company_Id = @CompanyId
          AND (@ProductId IS NULL OR WG.Item_Id = @ProductId)
          AND WG.Trans_Date < DATEADD(DAY, 1, CONVERT(DATE, @ToDate))  -- inclusive ToDate
    ),
    OpeningKey AS
    (
        SELECT
              Item_Id
            , MAX(Weighted_Avg_Id) AS OpeningWAId
        FROM Base
        WHERE Trans_Date < CONVERT(DATE, @FromDate)                    -- day before FromDate
        GROUP BY Item_Id
    ),
    ClosingKey AS
    (
        SELECT
              Item_Id
            , MAX(Weighted_Avg_Id) AS ClosingWAId
        FROM Base
        WHERE Trans_Date < DATEADD(DAY, 1, CONVERT(DATE, @ToDate))      -- inclusive ToDate
        GROUP BY Item_Id
    ),
    Move AS
    (
        SELECT
              Item_Id
            , SUM(CASE WHEN Curr_Qty > 0 THEN Curr_Qty ELSE 0 END)  AS InQty
            , SUM(CASE WHEN Curr_Qty < 0 THEN -Curr_Qty ELSE 0 END) AS OutQty
        FROM Base
        WHERE Trans_Date >= CONVERT(DATE, @FromDate)
          AND Trans_Date <  DATEADD(DAY, 1, CONVERT(DATE, @ToDate))     -- inclusive ToDate
        GROUP BY Item_Id
    )
    SELECT
          P.ProductCode
        , P.ProductName
        , P.PackSize

        , ISNULL(OP.Close_Qty,0) AS OpeningQty
        , ISNULL(OP.Avg_Rate,0)  AS OpeningRate
        , ISNULL(OP.Close_Amt,0) AS OpeningValue

        , ISNULL(MV.InQty,0)     AS InQty
        , ISNULL(MV.OutQty,0)    AS OutQty

        , ISNULL(CL.Close_Qty,0) AS ClosingQty
        , ISNULL(CL.Avg_Rate,0)  AS ClosingRate
        , ISNULL(CL.Close_Amt,0) AS ClosingValue
    FROM dbo.tblProduct P
    LEFT JOIN OpeningKey OK
           ON OK.Item_Id = P.ProductId
    LEFT JOIN Base OP
           ON OP.Item_Id = OK.Item_Id
          AND OP.Weighted_Avg_Id = OK.OpeningWAId
    LEFT JOIN Move MV
           ON MV.Item_Id = P.ProductId
    LEFT JOIN ClosingKey CK
           ON CK.Item_Id = P.ProductId
    LEFT JOIN Base CL
           ON CL.Item_Id = CK.Item_Id
          AND CL.Weighted_Avg_Id = CK.ClosingWAId
    WHERE P.CompanyId = @CompanyId
      AND (@ProductId IS NULL OR P.ProductId = @ProductId)
      AND (
            ISNULL(OP.Close_Qty,0) > 0
         OR ISNULL(MV.InQty,0)     > 0
         OR ISNULL(MV.OutQty,0)    > 0
         OR ISNULL(CL.Close_Qty,0) > 0
      )
    ORDER BY P.ProductCode;
END


```

### sp_INV_WeightedAverage_Report

- **Parameters:** @Param (NVARCHAR(MAX), IN) — appended filter clause, @OpenParam (NVARCHAR(MAX), IN, unused in active code — only referenced in the commented-out alternate query), @CloseParam (NVARCHAR(MAX), IN, unused in active code — only referenced in the commented-out alternate query)
- **Tables read:** tblProduct, tblUnitPrice, dbo.tblCentralStore
- **Tables written:** none
- **Business logic:** Builds and executes dynamic SQL returning, per product, the unit/MRP price plus the latest weighted-average rate derived from the most recent tblCentralStore receive record per company/product, filtered by the caller-supplied @Param fragment and ordered by ProductCode. A large alternate implementation (opening/in/out/closing valuation using tbl_INV_WeightedAvg) is present but fully commented out and not executed.
- **Transaction/error handling:** none (SET NOCOUNT ON only)
- **Result set:** Dynamic: ProductId, ProductCode, ProductName, UnitPrice, MRPPrice, Avg_Rate.
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/AreaDAL.cs:93 (GetDataTableAction("sp_INV_WeightedAverage_Report", ...))

```sql

CREATE PROCEDURE [dbo].[sp_INV_WeightedAverage_Report] 
    @Param NVARCHAR(MAX),
    @OpenParam NVARCHAR(MAX),
    @CloseParam NVARCHAR(MAX)
AS
BEGIN
    
	SET NOCOUNT ON;

    DECLARE @Query NVARCHAR(MAX);

    SET @Query = 'SELECT PD.ProductId,
	PD.ProductCode,
    PD.ProductName,
	UP.UnitPrice, UP.MRPPrice, ISNULL(Avg_Rate,0) Avg_Rate FROM tblProduct AS PD

LEFT JOIN tblUnitPrice AS UP ON PD.ProductCode = UP.ProductCode AND UP.IsActive = 1
LEFT JOIN (SELECT Company_Id,
    p.ProductId,
    p.ProductName,
    cs.Avg_Rate
FROM dbo.tblProduct p
LEFT JOIN (
    SELECT cs1.*
    FROM dbo.tblCentralStore cs1
    INNER JOIN (
        SELECT Company_Id, ProductCode, MAX(ReceiveId) AS MaxReceiveId
        FROM dbo.tblCentralStore
        --WHERE Company_Id = 29
        GROUP BY Company_Id, ProductCode
    ) x
        ON x.Company_Id = cs1.Company_Id
       AND x.ProductCode = cs1.ProductCode
       AND x.MaxReceiveId = cs1.ReceiveId
) cs
    ON cs.ProductCode = p.ProductCode
	WHERE Company_Id IS NOT NULL) AS LVG ON PD.ProductId = LVG.ProductId AND PD.CompanyId = Company_Id

	WHERE CompanyId IS NOT NULL ' + @Param + ' ORDER BY ProductCode ';

    EXEC (@Query);
END




-- SET @Query = '
--SELECT 
--    ID.ProductId,
--    ID.ProductName,
--	UP.UnitPrice,
--    ISNULL(OP.OP_Qty,0)          AS OP_Qty,
--    ISNULL(OP.OP_Rate, 0)        AS OP_Rate,
--    ISNULL(OP.OP_Amt,0)          AS OP_Amt,
--    ISNULL(TIN.TotalIn,0)        AS TotalIN,
--    ISNULL(TOT.TotalOut*-1,0)    AS TotalOut,
--    ISNULL(CS.Close_Qty,0)       AS Close_Qty,
--    ISNULL(CS.Avg_Rate,0)        AS Avg_Rate,
--    ISNULL(CS.Close_Amt,0)       AS Close_Amt
--FROM tblProduct AS ID

---- Opening
--LEFT JOIN (
--    SELECT 
--        WG.Company_Id, 
--        WG.Item_Id, 
--        WG.Close_Qty AS OP_Qty, 
--        WG.Avg_Rate  AS OP_Rate, 
--        WG.Close_Amt AS OP_Amt
--    FROM dbo.tbl_INV_WeightedAvg AS WG
--    INNER JOIN (
--        SELECT 
--            Company_Id, 
--            Item_Id, 
--            MAX(Weighted_Avg_Id) AS LatestWeighted_Avg_Id
--        FROM dbo.tbl_INV_WeightedAvg AS WG
--        WHERE WG.Company_Id IS NOT NULL ' + @OpenParam + '
--        GROUP BY Company_Id, Item_Id
--    ) AS Latest
--        ON WG.Company_Id = Latest.Company_Id
--       AND WG.Item_Id    = Latest.Item_Id
--       AND WG.Weighted_Avg_Id = Latest.LatestWeighted_Avg_Id
--    LEFT JOIN tblProduct AS ID ON WG.Item_Id = ID.ProductId
--    WHERE WG.Company_Id IS NOT NULL
--) AS OP
--    ON ID.ProductId = OP.Item_Id

---- In
--LEFT JOIN (
--    SELECT 
--        WG.Company_Id, 
--        WG.Item_Id, 
--        SUM(WG.Curr_Qty) AS TotalIn
--    FROM dbo.tbl_INV_WeightedAvg AS WG
--    LEFT JOIN tblProduct AS ID ON WG.Item_Id = ID.ProductId
--    WHERE WG.Curr_Qty > 0 ' + @Param + '
--    GROUP BY Company_Id, Item_Id
--) AS TIN
--    ON ID.ProductId = TIN.Item_Id

---- Out
--LEFT JOIN (
--    SELECT 
--        WG.Company_Id, 
--        WG.Item_Id, 
--        SUM(WG.Curr_Qty) AS TotalOut
--    FROM dbo.tbl_INV_WeightedAvg AS WG
--    LEFT JOIN tblProduct AS ID ON WG.Item_Id = ID.ProductId
--    WHERE WG.Curr_Qty < 0 ' + @Param + '
--    GROUP BY Company_Id, Item_Id
--) AS TOT
--    ON ID.ProductId = TOT.Item_Id
--LEFT JOIN tblUnitPrice AS UP ON ID.ProductId=UP.ProductId AND IsActive=1
---- Closing
--LEFT JOIN (
--    SELECT 
--        WG.Company_Id, 
--        WG.Item_Id, 
--        WG.Close_Qty, 
--        WG.Avg_Rate, 
--        WG.Close_Amt
--    FROM dbo.tbl_INV_WeightedAvg AS WG
--    INNER JOIN (
--        SELECT 
--            Company_Id, 
--            Item_Id, 
--            MAX(Weighted_Avg_Id) AS LatestWeighted_Avg_Id
--        FROM dbo.tbl_INV_WeightedAvg AS WG
--        WHERE WG.Company_Id IS NOT NULL ' + @CloseParam + '
--        GROUP BY Company_Id, Item_Id
--    ) AS Latest
--        ON WG.Company_Id = Latest.Company_Id
--       AND WG.Item_Id    = Latest.Item_Id
--       AND WG.Weighted_Avg_Id = Latest.LatestWeighted_Avg_Id
--    LEFT JOIN tblProduct AS ID ON WG.Item_Id = ID.ProductId
--    WHERE WG.Company_Id IS NOT NULL
--) AS CS
--    ON ID.ProductId = CS.Item_Id

--WHERE 
--    ISNULL(OP.OP_Qty,0) > 0
--    OR ISNULL(TIN.TotalIn,0) > 0
--    OR ISNULL(TOT.TotalOut*-1,0) > 0
--    OR ISNULL(CS.Close_Qty,0) > 0
--ORDER BY 
--    ID.ProductName ASC';


```

### sp_I_CompanyWiseDeposit

- **Parameters:** @DepositId (int, OUT), @IsDelete (bit, IN), @DepotId (int, IN), @BankId (int, IN), @BranchName (NVARCHAR(MAX), IN), @DepositType (NVARCHAR(MAX), IN), @DepositDate (DATETIME, IN), @Amount (DECIMAL(18,2), IN), @AccountName (NVARCHAR(MAX), IN), @CheckNumber (NVARCHAR(MAX), IN), @CheckDate (DATETIME, IN), @Remarks (NVARCHAR(MAX), IN), @EntryBy (NVARCHAR(MAX), IN), @EntryDate (DATETIME, IN)
- **Tables read:** none
- **Tables written:** tblCompanyWiseDeposit (INSERT)
- **Business logic:** Inserts a single bank deposit-slip record (company-wide deposit) and returns its generated identity as @DepositId.
- **Transaction/error handling:** none
- **Result set:** none (write-only procedure); @DepositId returned via OUTPUT parameter
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/DepositSlipEntryDal.cs:61 (SaveAction("sp_I_CompanyWiseDeposit", ..., "@DepositId"))

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,0@CompanyId/@CompanyId5/20@CompanyId6,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_I_CompanyWiseDeposit] 

	 @DepositId INT OUT,
	 @IsDelete BIT
    ,@DepotId INT
    ,@BankId INT
    ,@BranchName NVARCHAR(MAX)
    ,@DepositType NVARCHAR(MAX)
    ,@DepositDate DATEtime	
    ,@Amount DECIMAL(18,2)
    ,@AccountName NVARCHAR(MAX)
    ,@CheckNumber NVARCHAR(MAX)
    ,@CheckDate DATEtime
    ,@Remarks NVARCHAR(MAX)
    ,@EntryBy NVARCHAR(MAX)
    ,@EntryDate DATEtime

AS
BEGIN
	
	INSERT INTO tblCompanyWiseDeposit
           (DepotId
           ,BankId
           ,BranchName
           ,DepositType
           ,DepositDate
           ,Amount
           ,AccountName
           ,CheckNumber
           ,CheckDate
           ,Remarks
           ,EntryBy
           ,EntryDate,
		   IsDelete
           )
     VALUES
           (@DepotId
           ,@BankId
           ,@BranchName
           ,@DepositType
           ,@DepositDate
           ,@Amount
           ,@AccountName
           ,@CheckNumber
           ,@CheckDate
           ,@Remarks
           ,@EntryBy
           ,@EntryDate,
		    @IsDelete)


	SET @DepositId = SCOPE_IDENTITY() 

END



```

### sp_I_CustomerGl

- **Parameters:** @CustomerId (int, IN), @SubsidiaryLId (int, IN)
- **Tables read:** tblCustMaster, ZAS_ACCDB..tblChartOfAccounts, ZAS_ACCDB..tblSubsubsidiaryLadger
- **Tables written:** ZAS_ACCDB..tblChartOfAccounts (INSERT), ZAS_ACCDB..tblCOACompanyPermission (INSERT)
- **Business logic:** For the given customer, if no chart-of-accounts entry with the customer's derived account name exists yet, auto-generates a new GL account code (subsidiary ledger code + running count) and creates a Chart of Accounts entry plus a company-permission row for it, effectively auto-provisioning a customer's GL ledger account. Uses a cursor over a single-row result set (a no-op loop construct since the WHERE clause always returns at most one row).
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH; two separate INSERTs are not wrapped together)
- **Result set:** none (write-only procedure)
- **Caller(s):** referenced in a comment only, not actually called — D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:224 (`//return aCommonInternalDal.UpdateAction("sp_I_CustomerGl", aSqlParameterlist);`)

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_I_CustomerGl] 

 @CustomerId INT,
 @SubsidiaryLId INT

AS
BEGIN
	
	DECLARE @CstmrId INT
    Declare @CstmrCode nvarchar(250)  
    Declare @CstmrName NVARCHAR(MAX)
            
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT CustomerMasterId,CustomerCode,CustomerName FROM tblCustMaster WHERE CustomerMasterId = @CustomerId
    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @CstmrId ,
       @CstmrCode ,
       @CstmrName
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
	DECLARE @AccountCode NVARCHAR(MAX)
	DECLARE @SubsidiryLCode NVARCHAR(MAX)
	DECLARE @AccountName NVARCHAR(MAX)
	DECLARE @Description NVARCHAR(MAX)
	DECLARE @AccountId INT
	DECLARE @HasName INT = 0

	SET @AccountName = 'Code: ' + @CstmrCode + ' ' + @CstmrName
	SET @Description = 'Code: ' + @CstmrCode + ' ' + @CstmrName

	SELECT @HasName=ISNULL(COUNT(*),0) from ZAS_ACCDB..tblChartOfAccounts
	WHERE AccountName = @AccountName

	IF(@HasName =0)
	BEGIN
		SELECT @AccountCode=(CONVERT(NVARCHAR(MAX),(COUNT(AccountId)+10001))) FROM ZAS_ACCDB..tblChartOfAccounts 
		
		SELECT @SubsidiryLCode = SubsubsidiryLCode FROM ZAS_ACCDB..tblSubsubsidiaryLadger WHERE SubSubsidiaryId = @SubsidiaryLId
		SET @AccountCode=@SubsidiryLCode+@AccountCode
		
		insert into ZAS_ACCDB..tblChartOfAccounts (AccountCode,Description,AccountName,SubsidiaryLId,CompanyId,CreateBy,CreateDate,IsActive,InBalanceSheet,InIncomeStatement,CustomerId)
                                   values (@AccountCode,@Description,@AccountName,@SubsidiaryLId,0,'Auto Process',GETDATE(),'True','False','True',@CstmrId)

		SET @AccountId = SCOPE_IDENTITY()

		INSERT INTO ZAS_ACCDB..tblCOACompanyPermission (BankAccountId,CompanyId) VALUES (@AccountId, 0)
	                                   
	END
	
    FETCH NEXT FROM @MyCursor
    INTO @CstmrId ,
       @CstmrCode ,
       @CstmrName  
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END







```

### sp_I_DCStockOutInfo

- **Parameters:** @DestroyId (int, IN)
- **Tables read:** dbo.tblDCStoreFreeze, dbo.tblDestroyProduct
- **Tables written:** dbo.tblDCStore (UPDATE)
- **Business logic:** On approval of a product-destroy request, reduces DC (distribution center) store stock quantity by the stock-out quantity (floored at 0) and increases the damage quantity accordingly, for all DC store rows linked to the given destroy request via the freeze table.
- **Transaction/error handling:** none (SET NOCOUNT ON only; no TRAN/TRY-CATCH)
- **Result set:** none (write-only procedure)
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs:206 and D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs:218 (both UpdateAction("sp_I_DCStockOutInfo", ...))

```sql

-- =======================================================================================
-- 9. sp_I_DCStockOutInfo (Direct Stock Out from tblDCStore on Destroy Approval)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_I_DCStockOutInfo]
    @DestroyId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE DCS
    SET 
        DCS.StockQty = CASE WHEN DCS.StockQty >= DP.StockOutQty THEN DCS.StockQty - DP.StockOutQty ELSE 0 END,
        DCS.DamageQty = ISNULL(DCS.DamageQty, 0) + DP.StockOutQty
    FROM dbo.tblDCStore DCS
    INNER JOIN dbo.tblDCStoreFreeze DCF WITH (NOLOCK) ON DCS.DCStoreId = DCF.DCStoreId
    INNER JOIN dbo.tblDestroyProduct DP WITH (NOLOCK) ON DP.DcFreezeId = DCF.DCStoreFreezeId
    WHERE DP.DestroyId = @DestroyId;
END;


```

### sp_I_DuplicateInvoiceCodeChange

- **Parameters:** none
- **Tables read:** Table_Temp_DuplicateInvoice, tblInvoice
- **Tables written:** tblInvoice (UPDATE)
- **Business logic:** Iterates (via cursor) over a temp staging table of duplicate invoices and, for each, prefixes the invoice's InvoiceNo and DelivaryInvoiceNo with "A" to de-duplicate invoice codes.
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH around the cursor loop of updates)
- **Result set:** none (write-only procedure)
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; targeted grep found no matches)

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_I_DuplicateInvoiceCodeChange] 


AS
BEGIN
	
	DECLARE @TempInvoiceId INT
	DECLARE @InvoiceNo NVARCHAR(200)
	DECLARE @DelivaryInvoiceNo NVARCHAR(200)
            
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT TempInvoiceId,'A' + INV.InvoiceNo, 'A' + INV.DelivaryInvoiceNo FROM Table_Temp_DuplicateInvoice AS TINV
	LEFT JOIN tblInvoice AS INV ON TINV.TempInvoiceId = INV.InvoiceId
	
    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @TempInvoiceId,@InvoiceNo,@DelivaryInvoiceNo
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		UPDATE tblInvoice SET InvoiceNo = @InvoiceNo, DelivaryInvoiceNo = @DelivaryInvoiceNo 
		WHERE InvoiceId = @TempInvoiceId
	
    FETCH NEXT FROM @MyCursor
    INTO @TempInvoiceId,@InvoiceNo,@DelivaryInvoiceNo 
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END









```

### sp_I_InvoiceMaster

- **Parameters:** @InvoiceId (int, OUT), @InvoiceDate (DATETIME, IN, default NULL), @OrderNo (NVARCHAR(MAX), IN, default NULL), @OrderDate (DATETIME, IN, default NULL), @CustomerMasterId (int, IN, default NULL), @ComUnitId (int, IN, default NULL), @MiaId (int, IN, default NULL), @PaymentTypeId (int, IN, default NULL), @TpTotal (DECIMAL(18,2), IN, default NULL), @TpDiscount (DECIMAL(18,2), IN, default NULL), @TpVat (DECIMAL(18,2), IN, default NULL), @TpGrandTotal (DECIMAL(18,2), IN, default NULL), @UserId (int, IN, default NULL), @OrderId (int, IN, default NULL), @TotalSpecialAmount (DECIMAL(18,2), IN, default NULL), @OldTradePolicy (NVARCHAR(50), IN, default NULL), @ProductOffer (NVARCHAR(50), IN, default NULL), @Remarks (NVARCHAR(MAX), IN, default NULL), @DeliveryManId (int, IN, default NULL), @UnitCode (NVARCHAR(50), IN, default NULL), @PONumber (NVARCHAR(MAX), IN, default NULL)
- **Tables read:** tblInvoice
- **Tables written:** dbo.tblInvoice (INSERT)
- **Business logic:** Guards against duplicate/rapid-fire submission (same OrderNo processed within the last 60 seconds is rejected), then generates a new sequential InvoiceNo for the day/unit ("INV-{UnitCode}-{yyMMdd}{seq}") based on the max existing sequence for that ComUnitId/date, and inserts the invoice header row; if a duplicate InvoiceNo is somehow found, or the 60-second guard trips, it returns @InvoiceId = 0 instead of inserting. Large blocks of an older invoice-numbering scheme are commented out.
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH; relies only on the time-window duplicate check, which is not concurrency-safe)
- **Result set:** none (write-only procedure); @InvoiceId returned via OUTPUT parameter (0 signals rejected/duplicate)
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/InvoiceDAL.cs:1767 (SaveAction("sp_I_InvoiceMaster", ..., "@InvoiceId")); two other call sites in the same file (lines 223, 246) are commented out (`//    return aDbManager.SaveAction(...)`)

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_I_InvoiceMaster] 
	(
		 @InvoiceId int OUT,	
		 @InvoiceDate DATETIME = NULL,
         @OrderNo NVARCHAR(MAX) = NULL,
		 @OrderDate DATETIME=NULL,
		 @CustomerMasterId INT = NULL,
		 @ComUnitId INT = NULL,
		 @MiaId INT = NULL,
		 @PaymentTypeId INT = NULL,
		 @TpTotal DECIMAL(18,2)=NULL,
		 @TpDiscount DECIMAL(18,2)=NULL,
		 @TpVat DECIMAL(18,2)=NULL,
		 @TpGrandTotal DECIMAL(18,2)=NULL,
		 @UserId INT = NULL,
         @OrderId INT = NULL,
		 @TotalSpecialAmount DECIMAL(18,2)=NULL,
         @OldTradePolicy NVARCHAR(50) = NULL,
         @ProductOffer NVARCHAR(50) = NULL,
         @Remarks NVARCHAR(MAX) = NULL,
		 @DeliveryManId INT = NULL,
		 @UnitCode NVARCHAR(50) = NULL,
		 @PONumber NVARCHAR(MAX) = NULL
         
	)
AS
BEGIN


	--DECLARE @InvoiceNo NVARCHAR(MAX)
	--DECLARE @NoOfInvoice INT
	--DECLARE @Count INT

	--IF (@ComUnitId > 99)
	--BEGIN
	--		SELECT @NoOfInvoice = (ISNULL(MAX(CAST((SUBSTRING(InvoiceNo,11,12)) AS INT)),0)+1) FROM dbo.tblInvoice 
	--		WHERE ComUnitId = @ComUnitId AND Remarks != 'Due' GROUP BY ComUnitId
	--END
	--ELSE
	--BEGIN
	--		SELECT @NoOfInvoice = (ISNULL(MAX(CAST((SUBSTRING(InvoiceNo,10,11)) AS INT)),0)+1) FROM dbo.tblInvoice 
	--		WHERE ComUnitId = @ComUnitId AND Remarks != 'Due' GROUP BY ComUnitId
	--END
	
	--IF (@NoOfInvoice IS NULL)
	--BEGIN
	--		SET @NoOfInvoice = 1
	--END


	--DECLARE @length INT = 0
	--SET @length = LEN(CONVERT(NVARCHAR(MAX),@NoOfInvoice))

	--IF(@length > 0 OR @length is null)
	--BEGIN

		

	--	IF(@length = 1)
	--	BEGIN
	--		SET @InvoiceNo ='INV-' + @UnitCode + '-' + '00000000' + (CONVERT(NVARCHAR(MAX),@NoOfInvoice))
	--	END

	--	IF(@length = 2)
	--	BEGIN
	--		SET @InvoiceNo ='INV-' + @UnitCode + '-' + '0000000' + (CONVERT(NVARCHAR(MAX),@NoOfInvoice))
	--	END

	--	IF(@length = 3)
	--	BEGIN
	--		SET @InvoiceNo ='INV-' + @UnitCode + '-'+ '000000' + (CONVERT(NVARCHAR(MAX),@NoOfInvoice))
	--	END

	--	IF(@length = 4)
	--	BEGIN
	--		SET @InvoiceNo ='INV-' + @UnitCode + '-' + '00000' + (CONVERT(NVARCHAR(MAX),@NoOfInvoice))
	--	END

	--	IF(@length = 5)
	--	BEGIN
	--		SET @InvoiceNo ='INV-' + @UnitCode + '-' + '0000' + (CONVERT(NVARCHAR(MAX),@NoOfInvoice))
	--	END

	--	IF(@length = 6)
	--	BEGIN
	--		SET @InvoiceNo ='INV-' + @UnitCode + '-' + '000' + (CONVERT(NVARCHAR(MAX),@NoOfInvoice))
	--	END

	--	IF(@length = 7)
	--	BEGIN
	--		SET @InvoiceNo ='INV-' + @UnitCode + '-' + '00' + (CONVERT(NVARCHAR(MAX),@NoOfInvoice))
	--	END
	--END

	--SELECT @Count = COUNT(InvoiceNo) FROM tblInvoice WHERE InvoiceNo = @InvoiceNo


	--SELECT * FROM tblInvoice ORDER BY InvoiceId DESC


	IF NOT EXISTS (SELECT 1 FROM tblInvoice
	WHERE LTRIM(RTRIM(UPPER(OrderNo))) = LTRIM(RTRIM(UPPER(@OrderNo))) AND DATEDIFF(SECOND, ProcessTirm, GETDATE()) < 60 )
	BEGIN


			DECLARE @InvoiceNo NVARCHAR(MAX);
			DECLARE @MaxSequence INT;

			--DECLARE @UnitCode NVARCHAR(200) = 'BD01'
			--DECLARE @ComUnitId INT = 1

			
			-- Retrieve the maximum existing sequence number for Invoice on today's date

			SELECT @MaxSequence = ISNULL(MAX(CAST(RIGHT(InvoiceNo, 4) AS INT)), 0)
			FROM tblInvoice WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE) AND ComUnitId = @ComUnitId

			--PRINT @MaxSequence
			
			-- Generate the new InvoiceNo by incrementing the maximum sequence found

			SET @InvoiceNo = 'INV-' + @UnitCode + '-'
			    + RIGHT(YEAR(GETDATE()), 2) 
			    + FORMAT(GETDATE(), 'MM') 
			    + FORMAT(GETDATE(), 'dd') 
			    + RIGHT('0000' + CONVERT(NVARCHAR(4), @MaxSequence + 1), 4);

			--print @InvoiceNo


			DECLARE @Count INT = 0
			SELECT @Count = COUNT(InvoiceNo) FROM tblInvoice WHERE LTRIM(RTRIM(UPPER(InvoiceNo))) = LTRIM(RTRIM(UPPER(@InvoiceNo)))

			--PRINT @Count


			--PRINT @InvoiceNo

			if (@Count = 0)
			BEGIN

				INSERT INTO dbo.tblInvoice ( 
			       InvoiceNo ,  
			       InvoiceDate ,  
			       OrderNo ,  
			       OrderDate ,  
			       CustomerMasterId ,  
			       ComUnitId ,  
			       MiaId ,  
			       PaymentTypeId ,  
			       TpTotal ,  
			       TpDiscount ,  
			       TpVat ,  
			       TpGrandTotal ,  
			       UserId,  
			       OrderId, 
			       TotalSpecialAmount,  
			       OldTradePolicy,  
			       ProductOffer,
				   Remarks,
				   DeliveryManId,
				   PONumber 
			     ) 
				 VALUES (		 
				   @InvoiceNo ,  
			       @InvoiceDate ,  
			       @OrderNo ,  
			       @OrderDate ,  
			       @CustomerMasterId ,  
			       @ComUnitId ,  
			       @MiaId ,  
			       @PaymentTypeId ,  
			       @TpTotal ,  
			       @TpDiscount ,  
			       @TpVat ,  
			       @TpGrandTotal ,  
			       @UserId,  
			       @OrderId, 
			       @TotalSpecialAmount,  
			       @OldTradePolicy,  
			       @ProductOffer,
				   @Remarks,
				   @DeliveryManId,
				   @PONumber 

				 )
			       
			       
			 SET @InvoiceId = SCOPE_IDENTITY() 


			END
			ELSE
			BEGIN

				SET @InvoiceId = 0

			END

	END
	ELSE
	BEGIN

		SET @InvoiceId = 0

	END

	
	  
         

END


--DECLARE @Count INT = 0

--	SELECT @InvoiceNo = 'INV-' + @UnitCode + '-'
--    + RIGHT(YEAR(GETDATE()), 2) 
--    + FORMAT(GETDATE(), 'MM') 
--    + FORMAT(GETDATE(), 'dd') 
--    + RIGHT('0000' + CONVERT(NVARCHAR(4), 
--         (SELECT COUNT(InvoiceId) + 1 
--          FROM tblInvoice 
--          WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE) AND ComUnitId = @ComUnitId)
--    ), 4) 


--	PRINT @InvoiceNo

--	SELECT * FROM tblInvoice WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE) AND ComUnitId = @ComUnitId ORDER BY InvoiceNo DESC 


```

### sp_I_OrderMaster

- **Parameters:** @OrderId (int, OUT), @ComUnitId (int, IN, default NULL), @ComUnitCode (NVARCHAR(MAX), IN, default NULL), @ComUnitName (NVARCHAR(MAX), IN, default NULL), @MIOCode (NVARCHAR(MAX), IN, default NULL), @MIOName (NVARCHAR(MAX), IN, default NULL), @ManufacId (int, IN, default NULL), @CustomerCode (NVARCHAR(MAX), IN, default NULL), @CustomerName (NVARCHAR(MAX), IN, default NULL), @GrossValue (DECIMAL(18,2), IN, default NULL), @SubmissionDate (DATETIME, IN, default NULL), @IsManual (bit, IN, default NULL), @IsInvoice (bit, IN, default NULL), @RegionId (int, IN, default NULL), @AreaId (int, IN, default NULL), @TerritoryId (int, IN, default NULL), @MarketId (int, IN, default NULL), @CustomerMasterId (int, IN, default NULL), @RSMId (int, IN, default NULL), @ASMId (int, IN, default NULL), @MIOId (int, IN, default NULL), @EntryBy (NVARCHAR(50), IN, default NULL), @EntryDate (DATETIME, IN, default NULL), @ActionStatus (NVARCHAR(50), IN, default NULL), @IsSpecialApproval (bit, IN, default NULL), @ApprovedBy (NVARCHAR(50), IN, default NULL), @ApprovedDate (DATETIME, IN, default NULL)
- **Tables read:** tblOrder
- **Tables written:** dbo.tblOrder (INSERT)
- **Business logic:** Generates a new sequential OrderCode for the day/unit ("ODR-{ComUnitCode}-{yyMMdd}{seq}") based on the max existing sequence for that ComUnitId/date, then inserts a new order header row if no order with the same generated code already exists (duplicate guard returns @OrderId = 0 instead of inserting).
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH; the generate-then-check-then-insert pattern is not concurrency-safe)
- **Result set:** none (write-only procedure); @OrderId returned via OUTPUT parameter (0 signals a generated-code collision)
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs:223 (SaveAction("sp_I_OrderMaster", ..., "@OrderId"))

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_I_OrderMaster] 
	(
		 @OrderId int OUT,	
		 --@OrderNo NVARCHAR(MAX) = NULL,
         @ComUnitId INT = NULL,
         @ComUnitCode NVARCHAR(MAX) = NULL,
         @ComUnitName NVARCHAR(MAX) = NULL,
         @MIOCode NVARCHAR(MAX) = NULL,
         @MIOName NVARCHAR(MAX) = NULL,
         @ManufacId INT = NULL,
         @CustomerCode NVARCHAR(MAX) = NULL,
         @CustomerName NVARCHAR(MAX) = NULL,
         @GrossValue DECIMAL(18,2)=NULL,
         @SubmissionDate DATETIME=NULL,
         @IsManual  BIT=NULL,
         @IsInvoice  BIT=NULL,
         @RegionId INT = NULL,
         @AreaId INT = NULL,
         @TerritoryId INT = NULL,
         @MarketId INT = NULL,
         @CustomerMasterId INT = NULL,
         @RSMId INT = NULL,
         @ASMId INT = NULL,
         @MIOId INT = NULL,
         @EntryBy NVARCHAR(50) = NULL,
         @EntryDate DATETIME=NULL,
         @ActionStatus NVARCHAR(50) = NULL,
         @IsSpecialApproval  BIT=NULL,
         @ApprovedBy NVARCHAR(50) = NULL,
         @ApprovedDate DATETIME=NULL
		
	
	)
AS
BEGIN


	--DECLARE @yearText NVARCHAR(MAX)= SUBSTRING( CONVERT(NVARCHAR(MAX),YEAR(@SubmissionDate)), 3, 3)   
	--DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@SubmissionDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@SubmissionDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@SubmissionDate)) END)
	--DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@SubmissionDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@SubmissionDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@SubmissionDate)) END)

	DECLARE @OrderNo NVARCHAR(MAX);
	DECLARE @MaxSequence INT;
	
	-- Retrieve the maximum existing sequence number for orders on today's date

	SELECT @MaxSequence = ISNULL(MAX(CAST(RIGHT(OrderCode, 4) AS INT)), 0)
	FROM tblOrder WHERE CAST(EntryDate AS DATE) = CAST(GETDATE() AS DATE) AND ComUnitId = @ComUnitId;

	--PRINT @MaxSequence
	

	--DECLARE @ComUnitCode NVARCHAR(200) = 'BD205'

	-- Generate the new OrderNo by incrementing the maximum sequence found

	SET @OrderNo = 'ODR-' + @ComUnitCode + '-'
	    + RIGHT(YEAR(GETDATE()), 2) 
	    + FORMAT(GETDATE(), 'MM') 
	    + FORMAT(GETDATE(), 'dd') 
	    + RIGHT('0000' + CONVERT(NVARCHAR(4), @MaxSequence + 1), 4);

	--print @OrderNo


	DECLARE @Count INT = 0
	SELECT @Count = COUNT(OrderId) FROM tblOrder WHERE LTRIM(RTRIM(UPPER(OrderCode))) = LTRIM(RTRIM(UPPER(@OrderNo)))

	--PRINT @Count


	IF(@Count = 0)
	BEGIN

		INSERT INTO dbo.tblOrder
	  (
	      OrderCode,
	      ComUnitId,
	      ComUnitCode,
	      ComUnitName,
	      MIOCode,
	      MIOName,
	      ManufacId,
	      CustomerCode,
	      CustomerName,
	      GrossValue,
	      SubmissionDate,
	      IsInvoice,
	      IsManual,
	      RegionId,
	      AreaId,
	      TerritoryId,
	      MarketId,
	      CustomerMasterId,
	      RSMId,
	      ASMId,
	      MIOId,
	      EntryBy,
	      EntryDate,
			 ActionStatus,
	      IsSpecialApproval,
	      ApprovedBy,
	      ApprovedDate       
	  )
	  VALUES
	  (   @OrderNo,
	      @ComUnitId ,
	      @ComUnitCode ,
	      @ComUnitName ,
	      @MIOCode ,
	      @MIOName ,
	      @ManufacId ,
	      @CustomerCode ,
	      @CustomerName ,
	      @GrossValue ,
	      @SubmissionDate ,
			 @IsInvoice,
	      @IsManual,       
	      @RegionId,
	      @AreaId,
	      @TerritoryId,
	      @MarketId,
	      @CustomerMasterId,
	      @RSMId,
	      @ASMId,
	      @MIOId,
	      @EntryBy,
	      @EntryDate,
	      @ActionStatus,
	      @IsSpecialApproval,
	      @ApprovedBy,
	      @ApprovedDate
	  )
	        
	        
	        
	  SET @OrderId = SCOPE_IDENTITY()     


	END
	ELSE

	BEGIN
		
		SET @OrderId = 0

	END



	  --DECLARE @OrderNo NVARCHAR(MAX)
	  --SELECT @OrderNo='ODR-'+CONVERT(NVARCHAR(MAX),YEAR(GETDATE()))+ CONVERT(NVARCHAR(MAX),(ISNULL((MAX(CONVERT(INT,SUBSTRING(OrderCode,4,11)) )),0)+1)) FROM dbo.tblOrder WHERE YEAR(EntryDate)=YEAR(GETDATE())
	  

END

--SELECT GETDATE()

--SELECT * FROM tblOrder WHERE OrderCode = 'ODR-BD205-2411040001' ComUnitId = 205 AND SubmissionDate ='2024-11-04'

--SELECT 'ODR-' 
--    + RIGHT(YEAR(GETDATE()), 2) 
--    + FORMAT(GETDATE(), 'MM') 
--    + FORMAT(GETDATE(), 'dd') 
--    + RIGHT('0000' + CONVERT(NVARCHAR(4), 
--         (SELECT COUNT(OrderId) + 1 
--          FROM tblOrder 
--          WHERE CAST(SubmissionDate AS DATE) = CAST(GETDATE() AS DATE))
--    ), 4) AS OrderCode



--SELECT TOP 10 * FROM tblOrder WHERE OrderCode = 'ODR-2410290488' ORDER BY OrderId DESC 

--UPDATE tblOrder SET OrderCode = 'ODR-241012845' WHERE OrderId = 546589


--DELETE tblOrder WHERE OrderId IN (546582,546584,546583)
--DELETE tblOrderDetail WHERE OrderId IN (546582,546584,546583)


```

### sp_I_PaymentDeleteLog

- **Parameters:** @payMaserId (int, IN), @payDetailId (int, IN), @EntryBy (NVARCHAR(MAX), IN), @EntryDate (DATETIME, IN)
- **Tables read:** dbo.tblCustomerPay, dbo.tblCustPayDetail
- **Tables written:** dbo.tblCustomerPayDeleteLog (INSERT), dbo.tblCustPayDetailDeleteLog (INSERT)
- **Business logic:** Archives a customer payment master record (if @payMaserId > 0) and/or a payment detail record (if @payDetailId > 0) into "delete log" audit tables before the corresponding live records are presumably removed elsewhere, recording who/when performed the deletion.
- **Transaction/error handling:** none (two independent cursor loops, no BEGIN TRAN/TRY-CATCH)
- **Result set:** none (write-only procedure)
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PaymentReverseDal.cs:56 (UpdateAction("sp_I_PaymentDeleteLog", ...))

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_I_PaymentDeleteLog] 
	(
		 @payMaserId INT,
		 @payDetailId INT,
		 @EntryBy NVARCHAR(MAX),
		 @EntryDate DATETIME
	)
AS
BEGIN
	 
	IF(@payMaserId > 0) 
	BEGIN
		

		      DECLARE @CustPayId INT
              DECLARE @CustomerMasterId INT
              DECLARE @PaymentDate DATETIME
              DECLARE @PaymentAmount DECIMAL(18,2)
              DECLARE @PayType NVARCHAR(50)
              DECLARE @RefNo NVARCHAR (50)
              DECLARE @RefDate DATETIME
              DECLARE @CreateBy NVARCHAR(50)
              DECLARE @CreateDate DATETIME
		        
		--------------------------------------------------------
		DECLARE @MyCursor CURSOR
		SET @MyCursor = CURSOR FAST_FORWARD
		FOR
		---------------
		
		SELECT CustPayId,
               CustomerMasterId,
               PaymentDate,
               PaymentAmount,
               PayType,
               RefNo,
               RefDate,
               CreateBy,
               CreateDate FROM dbo.tblCustomerPay WHERE CustPayId = @payMaserId
			
		----------
		OPEN @MyCursor
		FETCH NEXT FROM @MyCursor
		INTO   @CustPayId,
               @CustomerMasterId,
               @PaymentDate,
               @PaymentAmount,
               @PayType,
               @RefNo,
               @RefDate,
               @CreateBy,
               @CreateDate
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
		
		INSERT INTO [dbo].[tblCustomerPayDeleteLog]
		(
		    CustPayId,
		    CustomerMasterId,
		    PaymentDate,
		    PaymentAmount,
		    PayType,
		    RefNo,
		    RefDate,
		    CreateBy,
		    CreateDate,
			[DeleteBy],
			[DeleteDate]
		)
		VALUES
		(   @CustPayId,         -- CustPayId - int
		    @CustomerMasterId,         -- CustomerMasterId - int
		    @PaymentDate, -- PaymentDate - datetime
		    @PaymentAmount,      -- PaymentAmount - decimal(18, 2)
		    @PayType,       -- PayType - nvarchar(50)
		    @RefNo,       -- RefNo - nvarchar(50)
		    @RefDate, -- RefDate - datetime
		    @CreateBy,       -- CreateBy - nvarchar(50)
		    @CreateDate, -- CreateDate - datetime
			@EntryBy,       -- UpdateBy - nvarchar(50)
			@EntryDate  -- UpdateDate - datetime
		)
		
		
		
		FETCH NEXT FROM @MyCursor
		INTO @CustPayId,
               @CustomerMasterId,
               @PaymentDate,
               @PaymentAmount,
               @PayType,
               @RefNo,
               @RefDate,
               @CreateBy,
               @CreateDate
		
		END
		CLOSE @MyCursor
		DEALLOCATE @MyCursor  
    END


	-- Pay Detai Info

    IF(@payDetailId > 0) 
	BEGIN


		DECLARE @CustPayDetailId INT
        DECLARE @InvoiceId INT
        DECLARE @PaymentDAmount DECIMAL(18,2) 
        DECLARE @CustPayMId INT

		        
		--------------------------------------------------------
		DECLARE @MyCursor2 CURSOR
		SET @MyCursor2 = CURSOR FAST_FORWARD
		FOR
		---------------
		SELECT CustPayDetailId,
               InvoiceId,
               PaymentAmount,
               CustPayId FROM dbo.tblCustPayDetail WHERE CustPayDetailId = @payDetailId
			
		----------
		OPEN @MyCursor2
		FETCH NEXT FROM @MyCursor2
		INTO   @CustPayDetailId,
               @InvoiceId,
               @PaymentDAmount,
               @CustPayMId
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
		
		INSERT INTO [dbo].[tblCustPayDetailDeleteLog]
		(
		    CustPayDetailId,
		    InvoiceId,
		    PaymentAmount,
		    CustPayId
		)
		VALUES
		(   @CustPayDetailId,    -- CustPayDetailId - int
		    @InvoiceId,    -- InvoiceId - int
		    @PaymentDAmount, -- PaymentAmount - decimal(18, 2)
		    @CustPayMId    -- CustPayId - int
		)
		
		
		
		FETCH NEXT FROM @MyCursor2
		INTO   @CustPayDetailId,
               @InvoiceId,
               @PaymentDAmount,
               @CustPayMId
		
		END
		CLOSE @MyCursor2
		DEALLOCATE @MyCursor2 
    END

END





```

### sp_I_PrettyCashDetails

- **Parameters:** @PettyCashDetailsId (int, OUT), @PettyCashmasterId (int, IN), @PerticulerId (int, IN), @CashReason (NVARCHAR(MAX), IN), @Amount (DECIMAL(18,2), IN)
- **Tables read:** none
- **Tables written:** tblPettyCashDetails (INSERT)
- **Business logic:** Inserts a petty cash detail line item under a given petty cash master record and returns its generated identity.
- **Transaction/error handling:** none
- **Result set:** none (write-only procedure); @PettyCashDetailsId returned via OUTPUT parameter
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PettyCashDaL.cs:100 (SaveAction("sp_I_PrettyCashDetails", ..., "@PettyCashDetailsId"))

```sql
-- =============================================
-- Author: <Author,Tareq>
-- Create date: <Create Date,18/08/2010,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_I_PrettyCashDetails] 

 @PettyCashDetailsId int out,
 @PettyCashmasterId INT,
 @PerticulerId Int,
 @CashReason NVARCHAR(MAX),
 @Amount DECIMAL(18,2)



AS
BEGIN
	
	--Insert into Stock Out

	INSERT INTO	tblPettyCashDetails(PettyCashmasterId,PerticulerId,CashReason,Amount) 
	VALUES (@PettyCashmasterId,@PerticulerId,@CashReason,@Amount)

	  
set @PettyCashDetailsId =SCOPE_IDENTITY()     
	
	
END



```

### sp_I_PrettyCashMaster

- **Parameters:** @PettyCashmasterId (int, OUT), @ConpanyId (int, IN), @PostingDate (DATETIME, IN), @CashAccId (int, IN), @Remark (NVARCHAR(MAX), IN), @EntryBy (NVARCHAR(50), IN), @EntryDate (DATETIME, IN), @ActiveStatus (NVARCHAR(50), IN)
- **Tables read:** none
- **Tables written:** tblPettyCashMaster (INSERT)
- **Business logic:** Inserts a petty cash master (header) record for a company/cash account posting and returns its generated identity.
- **Transaction/error handling:** none
- **Result set:** none (write-only procedure); @PettyCashmasterId returned via OUTPUT parameter
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PettyCashDaL.cs:121 (SaveAction("sp_I_PrettyCashMaster", ..., "@PettyCashmasterId"))

```sql
-- =============================================
-- Author: <Author,Tareq>
-- Create date: <Create Date,18/08/2010,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_I_PrettyCashMaster] 


@PettyCashmasterId int out,
@ConpanyId int ,
@PostingDate DateTime ,
@CashAccId int ,
@Remark Nvarchar(Max) ,
@EntryBy NVARCHAR(50) ,
@EntryDate DateTime ,
@ActiveStatus NVARCHAR(50)




AS
BEGIN
	
	--Insert into Stock Out

	INSERT INTO	tblPettyCashMaster(ConpanyId,PostingDate,CashAccId,Remark,EntryBy,EntryDate,ActiveStatus) 
	VALUES (@ConpanyId,@PostingDate,@CashAccId,@Remark,@EntryBy,@EntryDate,@ActiveStatus)

	  
set @PettyCashmasterId =SCOPE_IDENTITY()     
	
	
END



```

### sp_I_ProductGl

- **Parameters:** none
- **Tables read:** dbo.tblProduct, dbo.tblUnitPrice, ZAS_ACCDB..tblChartOfAccounts
- **Tables written:** ZAS_ACCDB..tblChartOfAccounts (INSERT), ZAS_ACCDB..tblCOACompanyPermission (INSERT)
- **Business logic:** For every active-priced product not yet represented in the Chart of Accounts, auto-generates a GL account code (fixed SubsidiaryLId = 53 + running count) and creates a Chart of Accounts entry plus a company-permission row, effectively bulk auto-provisioning GL ledger accounts for products missing one.
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH; cursor loop of independent inserts)
- **Result set:** none (write-only procedure)
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs:221 (UpdateAction("sp_I_ProductGl", ...))

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_I_ProductGl] 


AS
BEGIN
	
	DECLARE @ProductId INT
	DECLARE @CompanyId INT
    Declare @ProductCode nvarchar(250)  
    Declare @ProductName NVARCHAR(MAX)
            
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT PD.ProductId,PD.CompanyId,PD.ProductCode,PD.ProductName FROM dbo.tblProduct AS PD
	LEFT JOIN dbo.tblUnitPrice AS UP ON UP.ProductId = PD.ProductId
	WHERE UP.IsActive = 1 AND PD.ProductId NOT IN (SELECT GL.ProductId FROM ZAS_ACCDB..tblChartOfAccounts AS GL WHERE GL.ProductId IS NOT NULL)

    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @ProductId,@CompanyId,@ProductCode,@ProductName
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
	DECLARE @SubsidiaryLId INT  
	DECLARE @AccountCode NVARCHAR(MAX)
	DECLARE @SubsidiryLCode NVARCHAR(MAX)
	DECLARE @AccountName NVARCHAR(MAX)
	DECLARE @Description NVARCHAR(MAX)
	DECLARE @AccountId INT
	DECLARE @HasName INT = 0

	SET @AccountName = 'Code: ' + @ProductCode + ' ' + @ProductName
	SET @Description = 'Code: ' + @ProductCode + ' ' + @ProductName
	SET @SubsidiaryLId = 53

	SELECT @HasName=ISNULL(COUNT(*),0) from ZAS_ACCDB..tblChartOfAccounts
	WHERE AccountName = @AccountName

	IF(@HasName =0)
	BEGIN
		SELECT @AccountCode=(CONVERT(NVARCHAR(MAX),(COUNT(AccountId)+10001))) FROM ZAS_ACCDB..tblChartOfAccounts 
		
		SELECT @SubsidiryLCode = SubsubsidiryLCode FROM ZAS_ACCDB..tblSubsubsidiaryLadger WHERE SubSubsidiaryId = @SubsidiaryLId
		SET @AccountCode=@SubsidiryLCode+@AccountCode
		
		insert into ZAS_ACCDB..tblChartOfAccounts (AccountCode,Description,AccountName,SubsidiaryLId,CompanyId,CreateBy,CreateDate,IsActive,InBalanceSheet,InIncomeStatement,ProductId)
                                   values (@AccountCode,@Description,@AccountName,@SubsidiaryLId,0,'Auto Process',GETDATE(),'True','False','True',@ProductId)

		SET @AccountId = SCOPE_IDENTITY()

	     INSERT INTO ZAS_ACCDB..tblCOACompanyPermission (BankAccountId,CompanyId) VALUES (@AccountId, @CompanyId)                              
	END
	
    FETCH NEXT FROM @MyCursor
    INTO @ProductId,@CompanyId,@ProductCode,@ProductName 
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END










```

### sp_I_SampleProductIssue

- **Parameters:** @OrderId (int, OUT), @ComUnitId (int, IN, default NULL), @ComUnitCode (NVARCHAR(MAX), IN, default NULL), @ComUnitName (NVARCHAR(MAX), IN, default NULL), @MIOCode (NVARCHAR(MAX), IN, default NULL), @MIOName (NVARCHAR(MAX), IN, default NULL), @ManufacId (int, IN, default NULL), @CustomerCode (NVARCHAR(MAX), IN, default NULL), @CustomerName (NVARCHAR(MAX), IN, default NULL), @GrossValue (DECIMAL(18,2), IN, default NULL), @SubmissionDate (DATETIME, IN, default NULL), @IsManual (bit, IN, default NULL), @IsInvoice (bit, IN, default NULL), @RegionId (int, IN, default NULL), @AreaId (int, IN, default NULL), @TerritoryId (int, IN, default NULL), @MarketId (int, IN, default NULL), @CustomerMasterId (int, IN, default NULL), @RSMId (int, IN, default NULL), @ASMId (int, IN, default NULL), @MIOId (int, IN, default NULL), @EntryBy (NVARCHAR(50), IN, default NULL), @EntryDate (DATETIME, IN, default NULL), @ActionStatus (NVARCHAR(50), IN, default NULL), @IsSpecialApproval (bit, IN, default NULL), @ApprovedBy (NVARCHAR(50), IN, default NULL), @ApprovedDate (DATETIME, IN, default NULL), @DeliveryPersonId (int, IN, default NULL)
- **Tables read:** tblSampleIssue
- **Tables written:** tblSampleIssue (INSERT)
- **Business logic:** Guards against duplicate/rapid-fire submission (same EntryBy processed within the last 60 seconds is rejected), generates a new sample-issue order code ("SMP-{yyMMdd}{seq}") based on the max existing code for that submission date, and inserts the sample product issue header row.
- **Transaction/error handling:** none (no BEGIN TRAN/TRY-CATCH; time-window duplicate check is not concurrency-safe)
- **Result set:** none (write-only procedure); @OrderId returned via OUTPUT parameter (0 signals the 60-second duplicate guard tripped)
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/SampleIssueDal.cs:222 (SaveAction("sp_I_SampleProductIssue", ..., "@OrderId"))

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_I_SampleProductIssue] 
	(
		 @OrderId int OUT,	
		 --@OrderNo NVARCHAR(MAX) = NULL,
         @ComUnitId INT = NULL,
         @ComUnitCode NVARCHAR(MAX) = NULL,
         @ComUnitName NVARCHAR(MAX) = NULL,
         @MIOCode NVARCHAR(MAX) = NULL,
         @MIOName NVARCHAR(MAX) = NULL,
         @ManufacId INT = NULL,
         @CustomerCode NVARCHAR(MAX) = NULL,
         @CustomerName NVARCHAR(MAX) = NULL,
         @GrossValue DECIMAL(18,2)=NULL,
         @SubmissionDate DATETIME=NULL,
         @IsManual  BIT=NULL,
         @IsInvoice  BIT=NULL,
         @RegionId INT = NULL,
         @AreaId INT = NULL,
         @TerritoryId INT = NULL,
         @MarketId INT = NULL,
         @CustomerMasterId INT = NULL,
         @RSMId INT = NULL,
         @ASMId INT = NULL,
         @MIOId INT = NULL,
         @EntryBy NVARCHAR(50) = NULL,
         @EntryDate DATETIME=NULL,
         @ActionStatus NVARCHAR(50) = NULL,
         @IsSpecialApproval  BIT=NULL,
         @ApprovedBy NVARCHAR(50) = NULL,
         @ApprovedDate DATETIME=NULL,
		 @DeliveryPersonId INT = NULL
		
	
	)
AS
BEGIN
	  

	IF NOT EXISTS (SELECT 1 FROM tblSampleIssue
	WHERE LTRIM(RTRIM(UPPER(EntryBy))) = LTRIM(RTRIM(UPPER(@EntryBy))) AND DATEDIFF(SECOND, EntryDate, GETDATE()) < 60 )
	BEGIN

		DECLARE @yearText NVARCHAR(MAX)= SUBSTRING( CONVERT(NVARCHAR(MAX),YEAR(@SubmissionDate)), 3, 3)   
		DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@SubmissionDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@SubmissionDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@SubmissionDate)) END)
		DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@SubmissionDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@SubmissionDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@SubmissionDate)) END)

		DECLARE @OrderNo NVARCHAR(MAX)
		SELECT @OrderNo ='SMP-'+(@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(OrderCode,11,6)),10000))+1)  FROM tblSampleIssue 
		WHERE SubmissionDate = @SubmissionDate



		  --DECLARE @OrderNo NVARCHAR(MAX)
		  --SELECT @OrderNo='ODR-'+CONVERT(NVARCHAR(MAX),YEAR(GETDATE()))+ CONVERT(NVARCHAR(MAX),(ISNULL((MAX(CONVERT(INT,SUBSTRING(OrderCode,4,11)) )),0)+1)) FROM dbo.tblOrder WHERE YEAR(EntryDate)=YEAR(GETDATE())
		  
	   INSERT INTO tblSampleIssue
	   (
	       OrderCode,
	       ComUnitId,
	       ComUnitCode,
	       ComUnitName,
	       MIOCode,
	       MIOName,
	       ManufacId,
	       CustomerCode,
	       CustomerName,
	       GrossValue,
	       SubmissionDate,
	       IsInvoice,
	       IsManual,
	       RegionId,
	       AreaId,
	       TerritoryId,
	       MarketId,
	       CustomerMasterId,
	       RSMId,
	       ASMId,
	       MIOId,
	       EntryBy,
	       EntryDate,
			 ActionStatus,
	       IsSpecialApproval,
	       ApprovedBy,
	       ApprovedDate,
			 DeliveryPersonId       
	   )
	   VALUES
	   (   @OrderNo ,
	       @ComUnitId ,
	       @ComUnitCode ,
	       @ComUnitName ,
	       @MIOCode ,
	       @MIOName ,
	       @ManufacId ,
	       @CustomerCode ,
	       @CustomerName ,
	       @GrossValue ,
	       @SubmissionDate ,
			 @IsInvoice,
	       @IsManual,       
	       @RegionId,
	       @AreaId,
	       @TerritoryId,
	       @MarketId,
	       @CustomerMasterId,
	       @RSMId,
	       @ASMId,
	       @MIOId,
	       @EntryBy,
	       @EntryDate,
	       @ActionStatus,
	       @IsSpecialApproval,
	       @ApprovedBy,
	       @ApprovedDate,
			 @DeliveryPersonId
	   )
	         
	         
	         
	   SET @OrderId = SCOPE_IDENTITY()   

	END -- 

	ELSE
	BEGIN
		
		 SET @OrderId = 0

	END

	  

END




```

### sp_I_SampleProductIssueDetail

- **Parameters:** @OrderDetailId (int, OUT), @ProductId (int, IN, default NULL), @ProductCode (NVARCHAR(MAX), IN, default NULL), @ProductName (NVARCHAR(MAX), IN, default NULL), @Quantity (DECIMAL(18,3), IN, default 0), @TradePrice (DECIMAL(18,3), IN, default 0), @TotalTradePrice (DECIMAL(18,3), IN, default 0), @OrderId (int, IN, default 0)
- **Tables read:** tblSampleIssueDetail
- **Tables written:** dbo.tblSampleIssueDetail (INSERT)
- **Business logic:** Inserts a sample-issue product line item, guarded by a duplicate check (matching product code, quantity, order id, trade price and total trade price) to avoid inserting the exact same line twice.
- **Transaction/error handling:** none
- **Result set:** none (write-only procedure); @OrderDetailId returned via OUTPUT parameter (0 signals the duplicate guard tripped)
- **Caller(s):** referenced in a comment only, not actually called — D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/SampleIssueDal.cs:303 (`//return aInternalDal.SaveAction("sp_I_SampleProductIssueDetail", aSqlParameters, "@OrderDetailId");`)

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_I_SampleProductIssueDetail] 
	(
		 @OrderDetailId int OUT,	
		 --@OrderNo NVARCHAR(MAX) = NULL,
         @ProductId INT = NULL,
         @ProductCode NVARCHAR(MAX) = NULL,
         @ProductName NVARCHAR(MAX) = NULL,
         @Quantity DECIMAL(18,3) = 0,
		 @TradePrice DECIMAL(18,3) = 0,
         @TotalTradePrice DECIMAL(18,3) = 0,
		 @OrderId INT = 0
		
	
	)
AS
BEGIN
	  

	IF NOT EXISTS (SELECT 1 FROM tblSampleIssueDetail
	WHERE LTRIM(RTRIM(UPPER(@ProductCode))) = LTRIM(RTRIM(UPPER(@ProductCode))) AND Quantity = @Quantity AND OrderId = @OrderId 
	AND TradePrice = @TradePrice AND TotalTradePrice = @TotalTradePrice)
	BEGIN

		
	   INSERT INTO dbo.tblSampleIssueDetail
                                ( ProductId ,
                                  ProductCode ,
                                  ProductName ,
                                  Quantity ,
                                  TradePrice ,
                                  TotalTradePrice ,
                                  OrderId                                     
                                ) VALUES (                                     
                                  @ProductId ,
                                  @ProductCode ,
                                  @ProductName ,
                                  @Quantity ,
                                  @TradePrice ,
                                  @TotalTradePrice ,
                                  @OrderId 
                               )
	         
	         
	   SET @OrderDetailId = SCOPE_IDENTITY()   

	END -- 

	ELSE
	BEGIN
		
		 SET @OrderDetailId = 0

	END

	  

END



```

### sp_I_SupplierGl

- **Parameters:** @SupplierId (int, IN), @SubsidiaryLId (int, IN)
- **Tables read:** tblSupplierInformation, ZAS_ACCDB..tblChartOfAccounts, ZAS_ACCDB..tblSubsubsidiaryLadger
- **Tables written:** ZAS_ACCDB..tblChartOfAccounts
- **Business logic:** For the given supplier, if no Chart of Accounts entry already exists with the composed account name ("Code: <SupplierCode> <SupplierName>"), generates a new account code (subsidiary ledger code + a running count of existing accounts + 10001) and inserts a new GL account row linked to the supplier. Effectively auto-creates a ledger account for a supplier the first time it is needed.
- **Transaction/error handling:** none (no BEGIN TRAN, no TRY/CATCH).
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/SupplierInfoDal.cs:132 via `aCommonInternalDal.UpdateAction("sp_I_SupplierGl", ...)`.

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_I_SupplierGl] 

 @SupplierId INT,
 @SubsidiaryLId INT

AS
BEGIN
	
	DECLARE @SuppId INT
    Declare @SuppCode nvarchar(250)  
    Declare @SuppName NVARCHAR(MAX)
            
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT SupplierId,SupplierCode,SupplierName FROM tblSupplierInformation WHERE SupplierId = @SupplierId
    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @SuppId ,
       @SuppCode ,
       @SuppName
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
	DECLARE @AccountCode NVARCHAR(MAX)
	DECLARE @SubsidiryLCode NVARCHAR(MAX)
	DECLARE @AccountName NVARCHAR(MAX)
	DECLARE @Description NVARCHAR(MAX)
	DECLARE @AccountId INT
	DECLARE @HasName INT = 0

	SET @AccountName = 'Code: ' + @SuppCode + ' ' + @SuppName
	SET @Description = 'Code: ' + @SuppCode + ' ' + @SuppName

	SELECT @HasName=ISNULL(COUNT(*),0) from ZAS_ACCDB..tblChartOfAccounts
	WHERE AccountName = @AccountName

	IF(@HasName =0)
	BEGIN
		SELECT @AccountCode=(CONVERT(NVARCHAR(MAX),(COUNT(AccountId)+10001))) FROM ZAS_ACCDB..tblChartOfAccounts 
		
		SELECT @SubsidiryLCode = SubsubsidiryLCode FROM ZAS_ACCDB..tblSubsubsidiaryLadger WHERE SubSubsidiaryId = @SubsidiaryLId
		SET @AccountCode=@SubsidiryLCode+@AccountCode
		
		insert into ZAS_ACCDB..tblChartOfAccounts (AccountCode,Description,AccountName,SubsidiaryLId,CompanyId,CreateBy,CreateDate,IsActive,InBalanceSheet,InIncomeStatement,SupplierId)
                                   values (@AccountCode,@Description,@AccountName,@SubsidiaryLId,0,'Auto Process',GETDATE(),'True','False','True',@SuppId)

		SET @AccountId = SCOPE_IDENTITY()

		--INSERT INTO ZAS_ACCDB..tblCOACompanyPermission (BankAccountId,CompanyId) VALUES (@AccountId, 0)
	                                   
	END
	
    FETCH NEXT FROM @MyCursor
    INTO @SuppId ,
       @SuppCode ,
       @SuppName  
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

### sp_Integration_CustomerSubsidaryInsert

- **Parameters:** @CustomerId (int, IN) — accepted but not used to filter the query (see business logic note).
- **Tables read:** tblCustMaster, CSTL_ACCDB..tblSubsidiaryInfo
- **Tables written:** CSTL_ACCDB..tblSubsidiaryInfo
- **Business logic:** Finds every customer in tblCustMaster that has no matching subsidiary-ledger record (SubsidiaryTypeId = 1) in CSTL_ACCDB..tblSubsidiaryInfo, and inserts a new subsidiary-info row for each one with a hardcoded EntryBy of 160. Note: the @CustomerId input parameter is declared but the cursor's SELECT does not filter by it, so the procedure processes ALL missing customers regardless of the parameter value — likely a bug/leftover from a per-customer version of this procedure.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:225 via `aCommonInternalDal.UpdateAction("sp_Integration_CustomerSubsidaryInsert", ...)`.

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Integration_CustomerSubsidaryInsert] 

	@CustomerId INT

AS
BEGIN

	DECLARE @CstmrId INT
    Declare @CstmrCode nvarchar(250)  
    Declare @CstmrName NVARCHAR(MAX)
            
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	--SELECT * FROM tblCustMaster ORDER BY CustomerName

	SELECT CustomerMasterId,CustomerCode,CustomerName FROM tblCustMaster AS CMS
	LEFT JOIN CSTL_ACCDB..tblSubsidiaryInfo AS SB ON CMS.CustomerMasterId = SB.S_Info_Id AND SB.SubsidiaryTypeId = 1
	WHERE SB.S_Info_Id IS NULL

    --------------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @CstmrId ,
       @CstmrCode ,
       @CstmrName
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		INSERT INTO CSTL_ACCDB..tblSubsidiaryInfo
		       (SubsidiaryTypeId
		       ,S_Info_Id
		       ,IsSelect
		       ,Description
		       ,EntryBy
		       ,EntryDate)
		 VALUES
		       (1
		       ,@CstmrId
		       ,1
		       ,NULL
		       ,160
		       ,GETDATE()
		       )
	
    FETCH NEXT FROM @MyCursor
    INTO @CstmrId ,
       @CstmrCode ,
       @CstmrName  
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor

	
END
```

### sp_MRCP_DeletePayment

- **Parameters:** @CustPayId (int, IN)
- **Tables read:** none
- **Tables written:** tblCustomerPay, tblCustPayDetail
- **Business logic:** Deletes the payment header row from tblCustomerPay and its associated detail rows from tblCustPayDetail for a given CustPayId — a hard delete of a customer payment.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:76 via `aInternalDal.DeleteAction("sp_MRCP_DeletePayment", ...)`.

```sql
-- =============================================
-- Author: <Author,Nasa>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[sp_MRCP_DeletePayment] 

	@CustPayId INT

AS
BEGIN

	
	DELETE FROM tblCustomerPay WHERE CustPayId = @CustPayId
	DELETE FROM tblCustPayDetail WHERE CustPayId = @CustPayId
	

END
```

### sp_MRCP_MoneyReceiptMasterInsert

- **Parameters:** @MoneyReceiptID (int, OUT), @MoneyReceiptDate (datetime, IN), @BatchId (int, IN, NULL), @CustomerMasterId (int, IN, NULL), @CustPayDetailsId (nvarchar(max), IN, NULL), @CreateBy (nvarchar(200), IN)
- **Tables read:** tblMoneyReceipt
- **Tables written:** tblMoneyReceipt
- **Business logic:** Guards against duplicate double-submits: if no existing tblMoneyReceipt row was created by the same user for the same receipt date within the last 30 seconds, generates a receipt code in the form `RCPT-YYMMDD` + a sequence number based on today's receipt count, inserts the new money receipt, and returns its identity via @MoneyReceiptID. If a likely duplicate is detected, returns @MoneyReceiptID = 0 instead of inserting.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure; result communicated via OUTPUT parameter @MoneyReceiptID).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:156 and :228 via `aInternalDal.SaveAction("sp_MRCP_MoneyReceiptMasterInsert", ..., "@MoneyReceiptID")`.

```sql
-- =============================================
-- Author: <Author,Nasa>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[sp_MRCP_MoneyReceiptMasterInsert] 

	     @MoneyReceiptID int OUT
		,@MoneyReceiptDate DATETIME
        ,@BatchId int NULL
		,@CustomerMasterId INT NULL
        ,@CustPayDetailsId nvarchar(max) NULL
		,@CreateBy nvarchar(200)
AS
BEGIN

	

	IF NOT EXISTS (SELECT 1 FROM tblMoneyReceipt
	WHERE LTRIM(RTRIM(UPPER(CreateBy))) = LTRIM(RTRIM(UPPER(@CreateBy))) AND MonyReceiptDate = @MoneyReceiptDate AND DATEDIFF(SECOND,CreateDate, GETDATE()) < 30 )
		  
	BEGIN

		-- Declare variables

		DECLARE @SpotPurchaseCode NVARCHAR(500);
		DECLARE @ProCount INT;
		
		-- Calculate the sequence for the current date

		SELECT @ProCount = COUNT(MoneyReceiptId) + 1 
		FROM tblMoneyReceipt 
		WHERE CAST(CreateDate AS DATE) = CAST(GETDATE() AS DATE);
		
		-- Generate the ProformaCode in the desired format (YYMMDD + sequence starting at 1000)

		SET @SpotPurchaseCode = 'RCPT-' + RIGHT(YEAR(GETDATE()), 2) 
							+ FORMAT(GETDATE(), 'MMdd') 
							+ CAST(1000 + @ProCount AS NVARCHAR);

		--PRINT @SpotPurchaseCode

		-- Insert

		INSERT INTO [dbo].[tblMoneyReceipt]
		       (MoneyReceiptNo
		       ,MonyReceiptDate
		       ,BatchId
		       ,CustMasterId
		       ,CustPayDetailsId
		       ,CreateBy)
		 VALUES
		       (@SpotPurchaseCode,@MoneyReceiptDate, @BatchId, @CustomerMasterId, @CustPayDetailsId, @CreateBy)


		SET @MoneyReceiptID = SCOPE_IDENTITY()

	END

	ELSE 
	BEGIN
		
		SET @MoneyReceiptID = 0

	END
	

END
```

### sp_Missing_CustomerPaymentPosting

- **Parameters:** none
- **Tables read:** tblCustPayDetail, tblInvoice, tblCustMaster, tblCompanyUnit, CSTL_ACCDB..tblDebitCreditVoucherMaster
- **Tables written:** none directly (delegates all writes to `sp_Sales_CustomerPaymentPosting`, called per row — not in this chunk's file set, so its writes are out of scope here)
- **Business logic:** Bulk-correction/catch-up job: finds customer payment detail rows processed on/after 2025-04-25 that have no corresponding GL posting voucher (TableID = 2017) in CSTL_ACCDB..tblDebitCreditVoucherMaster, and for each one calls `sp_Sales_CustomerPaymentPosting @InvoiceId, @CustPayId` to post the missing accounting entry. This is a "missing postings" catch-up/reposting utility.
- **Transaction/error handling:** none (no BEGIN TRAN/COMMIT/ROLLBACK, no TRY/CATCH) — if `sp_Sales_CustomerPaymentPosting` fails partway through the cursor loop, prior iterations are not rolled back and the cursor is left open/undeallocated.
- **Result set:** none (write-only procedure; relies on the called sub-procedure for any output).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_Missing_CustomerPaymentPosting] 


AS
BEGIN
	
	DECLARE @InvoiceId INT
	DECLARE @CustPayId INT
            
    --------------------------------------------------------

    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR

    ---------------

	--INV.InvoiceNo, CONVERT(DATE,InvoiceDate) InvoiceDate,CustomerName,ISNULL(CPD.PaymentAmount,0) PaymentAmount, ISNULL(CPD.AIT,0) AIT,
	--ISNULL(CPD.Discount,0) Discount, CONVERT(DATE,ProcessTime) ProcessTime

	--SELECT * FROM CSTL_ACCDB..tblDebitCreditVoucherMaster


	SELECT CPD.CustPayId,CPD.InvoiceId FROM tblCustPayDetail CPD 
	LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId
	LEFT JOIN tblCustMaster AS CM ON INV.CustomerMasterId = CM.CustomerMasterId
	LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON CPD.CustPayId = VM.CommonPK_ID AND VM.TableID = 2017
	WHERE CONVERT(DATE, CPD.ProcessTime) >= '2025-04-25' AND VM.CommonPK_ID IS NULL
	
    ----------

    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @CustPayId,@InvoiceId
       
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		EXEC sp_Sales_CustomerPaymentPosting @InvoiceId,@CustPayId
	
    FETCH NEXT FROM @MyCursor
    INTO @CustPayId,@InvoiceId 
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

### sp_Missing_DeliveryConfirmationPosting

- **Parameters:** none
- **Tables read:** tblInvoice, tblCustMaster, tblCompanyUnit, CSTL_ACCDB..tblDebitCreditVoucherMaster
- **Tables written:** none directly (delegates writes to `sp_Sales_DeliverySalesPosting`, called per row — not in this chunk's file set)
- **Business logic:** Bulk catch-up job: finds invoices with a Full/Partial delivery status, updated on/after 2025-04-25, that have no non-inventory-valuation GL posting voucher (TableID = 2014, IsInventoryValuation = 0) yet, and calls `sp_Sales_DeliverySalesPosting @InvoiceId` for each to post the missing delivery/COGS accounting entry.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_Missing_DeliveryConfirmationPosting] 


AS
BEGIN
	
	DECLARE @InvoiceId INT
    Declare @InvoiceNo nvarchar(250)  
	DECLARE @CustomerName nvarchar(500)
	DECLARE @InvoiceDate DATETIME
    Declare @DelivaryInvoiceNo NVARCHAR(200)
	DECLARE @DeliveryInvoiceStatus NVARCHAR(200)
	DECLARE @UpdateDate DATETIME
	DECLARE @DeliveryTpGrandTotal DECIMAL(18,2)
            
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT  INV.InvoiceId,INV.InvoiceNo,CustomerName, CONVERT(DATE,InvoiceDate) InvoiceDate,INV.DelivaryInvoiceNo, DeliveryInvoiceStatus,CONVERT(DATE,INV.UpdateDate) UpdateDate,DeliveryTpGrandTotal FROM tblInvoice AS INV
	LEFT JOIN tblCustMaster AS CM ON INV.CustomerMasterId = CM.CustomerMasterId
	LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON INV.InvoiceId = VM.CommonPK_ID AND VM.TableID = 2014 AND IsInventoryValuation = 0
	WHERE DeliveryInvoiceStatus in ('Full','Partial') AND  CONVERT(DATE, INV.UpdateDate) >= '2025-04-25' AND VM.CommonPK_ID IS NULL

	
    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @InvoiceId,@InvoiceNo,@CustomerName,@InvoiceDate,@DelivaryInvoiceNo,@DeliveryInvoiceStatus,@UpdateDate,@DeliveryTpGrandTotal
       
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
			EXEC sp_Sales_DeliverySalesPosting @InvoiceId; 
	
    FETCH NEXT FROM @MyCursor
    INTO @InvoiceId,@InvoiceNo,@CustomerName,@InvoiceDate,@DelivaryInvoiceNo,@DeliveryInvoiceStatus,@UpdateDate,@DeliveryTpGrandTotal 
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

### sp_Missing_DeliverySalesReturnPosting

- **Parameters:** none
- **Tables read:** tblCustomerReturnMaster, CSTL_ACCDB..tblDebitCreditVoucherMaster
- **Tables written:** none directly (delegates writes to `sp_Sales_DeliverySalesReturnPosting`, called per row — not in this chunk's file set)
- **Business logic:** Bulk catch-up job: finds approved customer returns (tblCustomerReturnMaster, ApprovedDate on/after 2025-04-25) with no matching GL posting voucher (TableID = 2018), and calls `sp_Sales_DeliverySalesReturnPosting @ReturnId` for each to post the missing return accounting entry.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_Missing_DeliverySalesReturnPosting] 


AS
BEGIN
	
	DECLARE @ReturnId INT
	--DECLARE @CustPayId INT
            
    --------------------------------------------------------

    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR

    ---------------

	--INV.InvoiceNo, CONVERT(DATE,InvoiceDate) InvoiceDate,CustomerName,ISNULL(CPD.PaymentAmount,0) PaymentAmount, ISNULL(CPD.AIT,0) AIT,
	--ISNULL(CPD.Discount,0) Discount, CONVERT(DATE,ProcessTime) ProcessTime

	--SELECT * FROM CSTL_ACCDB..tblDebitCreditVoucherMaster

	--SM.OrderCode,SM.ComUnitName, SM.CustomerCode,SM.CustomerName, SM.MIOCode, SM.MIOName, SM.GrossValue,SM.SubmissionDate


	SELECT CustReturnId FROM tblCustomerReturnMaster AS RM
	LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON RM.CustReturnId = VM.CommonPK_ID AND VM.TableID = 2018
	WHERE CONVERT(DATE, RM.ApprovedDate) >= '2025-04-25' AND VM.CommonPK_ID IS NULL


    ----------

    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @ReturnId
       
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		EXEC sp_Sales_DeliverySalesReturnPosting @ReturnId
	
    FETCH NEXT FROM @MyCursor
    INTO @ReturnId 
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

### sp_Missing_ProductDestroyPosting

- **Parameters:** none
- **Tables read:** tblDestroyProduct, CSTL_ACCDB..tblDebitCreditVoucherMaster
- **Tables written:** none directly (delegates writes to `sp_Sales_ProductDestroyPosting`, called per row — not in this chunk's file set)
- **Business logic:** Bulk catch-up job: finds tblDestroyProduct rows with ActionStatus = 'Accepted' and ApproveDate on/after 2025-04-25 that have no matching GL posting voucher (TableID = 2016), and calls `sp_Sales_ProductDestroyPosting @DestroyId` for each to post the missing destroy-write-off accounting entry.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_Missing_ProductDestroyPosting] 


AS
BEGIN
	
	DECLARE @DestroyId INT
	--DECLARE @CustPayId INT
            
    --------------------------------------------------------

    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR

    ---------------

	--INV.InvoiceNo, CONVERT(DATE,InvoiceDate) InvoiceDate,CustomerName,ISNULL(CPD.PaymentAmount,0) PaymentAmount, ISNULL(CPD.AIT,0) AIT,
	--ISNULL(CPD.Discount,0) Discount, CONVERT(DATE,ProcessTime) ProcessTime

	--SELECT * FROM CSTL_ACCDB..tblDebitCreditVoucherMaster

	--SM.OrderCode,SM.ComUnitName, SM.CustomerCode,SM.CustomerName, SM.MIOCode, SM.MIOName, SM.GrossValue,SM.SubmissionDate


	SELECT RM.DestroyId FROM tblDestroyProduct AS RM
	LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON RM.DestroyId = VM.CommonPK_ID AND VM.TableID = 2016
	WHERE LTRIM(RTRIM(UPPER(RM.ActionStatus))) =  LTRIM(RTRIM(UPPER('Accepted'))) AND CONVERT(DATE, RM.ApproveDate) >= '2025-04-25' AND VM.CommonPK_ID IS NULL


    ----------

    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @DestroyId
       
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		EXEC sp_Sales_ProductDestroyPosting @DestroyId
	
    FETCH NEXT FROM @MyCursor
    INTO @DestroyId 
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

### sp_Missing_SampleInvoicePosting

- **Parameters:** @FromDate (datetime, IN, default '2026-07-01'), @ToDate (datetime, IN, default '2026-07-31')
- **Tables read:** dbo.tblSampleIssue, dbo.tblCompanyUnit, CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster
- **Tables written:** none directly (delegates writes to `dbo.sp_Sales_SampleCOGSPosting`, called per row — not in this chunk's file set)
- **Business logic:** Bulk-poster for missing sample COGS vouchers: finds distinct sample-issue orders with ActionStatus IN ('ACCEPTED','APPROVED') approved within the given date window that have no non-cancelled GL posting voucher (TableID = 2015) for their company, and calls `sp_Sales_SampleCOGSPosting @InvoiceId = @OrderId` for each. Prints a summary count of how many were posted.
- **Transaction/error handling:** none (no BEGIN TRAN/COMMIT/ROLLBACK, no TRY/CATCH).
- **Result set:** none (write-only procedure); emits a PRINT message with the count posted, not a result set.
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql

-- =========================================================================
-- sp_Missing_SampleInvoicePosting (Bulk Poster for Missing Samples)
-- =========================================================================
CREATE   PROCEDURE [dbo].[sp_Missing_SampleInvoicePosting]
(
    @FromDate DATETIME = '2026-07-01',
    @ToDate DATETIME = '2026-07-31'
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OrderId INT;
    DECLARE @Count INT = 0;

    DECLARE curSample CURSOR LOCAL FAST_FORWARD FOR
    SELECT DISTINCT SI.OrderId
    FROM dbo.tblSampleIssue SI WITH (NOLOCK)
    INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON SI.ComUnitId = CU.ComUnitId
    WHERE LTRIM(RTRIM(UPPER(SI.ActionStatus))) IN ('ACCEPTED', 'APPROVED')
      AND SI.ApprovedDate BETWEEN @FromDate AND @ToDate
      AND NOT EXISTS (
          SELECT 1 
          FROM CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster VM WITH (NOLOCK)
          WHERE VM.CompanyInfoId = CU.CompanyId
            AND VM.TableID = 2015 
            AND VM.CommonPK_ID = SI.OrderId
            AND VM.ActionStatus <> 'Cancelled'
      );

    OPEN curSample;
    FETCH NEXT FROM curSample INTO @OrderId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.sp_Sales_SampleCOGSPosting @InvoiceId = @OrderId;
        SET @Count = @Count + 1;
        FETCH NEXT FROM curSample INTO @OrderId;
    END;

    CLOSE curSample;
    DEALLOCATE curSample;

    PRINT 'SUCCESS: Posted ' + CAST(@Count AS NVARCHAR(10)) + ' missing sample COGS vouchers!';
END;
```

### sp_Missing_WHReceivePosting

- **Parameters:** none
- **Tables read:** tblWHStockInMaster, CSTL_ACCDB..tblDebitCreditVoucherMaster
- **Tables written:** none directly (delegates writes to `sp_Acc_MaterialConversionReceive`, called per row — not in this chunk's file set)
- **Business logic:** Bulk catch-up job: finds warehouse stock-in master rows with Status = 'Approved' and ApproveDate on/after 2025-04-25 that have no matching GL posting voucher (TableID = 2013), and calls `sp_Acc_MaterialConversionReceive @ApprovalStatus, @ApprovalBy, @ApprovalDate, @MasterId` for each to post the missing warehouse-receive accounting entry.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_Missing_WHReceivePosting] 


AS
BEGIN
	

	DECLARE @ApprovalStatus NVARCHAR(200)
	DECLARE @ApprovalBy NVARCHAR(200)
	DECLARE @ApprovalDate DATETIME
	DECLARE @MasterId INT

	--DECLARE @CustPayId INT
            
    --------------------------------------------------------

    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR

    ---------------

	--INV.InvoiceNo, CONVERT(DATE,InvoiceDate) InvoiceDate,CustomerName,ISNULL(CPD.PaymentAmount,0) PaymentAmount, ISNULL(CPD.AIT,0) AIT,
	--ISNULL(CPD.Discount,0) Discount, CONVERT(DATE,ProcessTime) ProcessTime

	--SELECT * FROM CSTL_ACCDB..tblDebitCreditVoucherMaster

	--SM.OrderCode,SM.ComUnitName, SM.CustomerCode,SM.CustomerName, SM.MIOCode, SM.MIOName, SM.GrossValue,SM.SubmissionDate


	SELECT RM.WHStockInMasterID,Status,ApprovedBy,ApproveDate FROM tblWHStockInMaster AS RM
	LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON RM.WHStockInMasterID = VM.CommonPK_ID AND VM.TableID = 2013
	WHERE LTRIM(RTRIM(UPPER(RM.Status))) =  LTRIM(RTRIM(UPPER('Approved'))) AND CONVERT(DATE, RM.ApproveDate) >= '2025-04-25' AND VM.CommonPK_ID IS NULL

	
	--SELECT RD.WHStockInDetailID FROM tblWHStockInMaster AS RM
	--LEFT JOIN tblWHStockInDetail AS RD ON RM.WHStockInMasterID = RD.WHStockInMasterID
	--LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster VM ON RM.WHStockInMasterID = VM.CommonPK_ID AND VM.TableID = 2013
	--WHERE LTRIM(RTRIM(UPPER(RM.Status))) =  LTRIM(RTRIM(UPPER('Approved'))) AND CONVERT(DATE, RM.ApproveDate) >= '2025-04-25' AND VM.CommonPK_ID IS NULL

    ----------

    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @MasterId,@ApprovalStatus,@ApprovalBy,@ApprovalDate
       
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		EXEC sp_Acc_MaterialConversionReceive 
		 @ApprovalStatus, 
		 @ApprovalBy, 
		 @ApprovalDate, 
		 @MasterId;
	
    FETCH NEXT FROM @MyCursor
    INTO @MasterId,@ApprovalStatus,@ApprovalBy,@ApprovalDate
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

### sp_NationalBinCard

- **Parameters:** @FromDate (datetime, IN), @ToDate (datetime, IN), @CompanyId (int, IN)
- **Tables read:** tblProduct, tblCentralStore_OpeninigBalance, tblCentralStore, tblStockInTransfar, tblRequisition, tblWhStoreFreeze, tblDCStore_OpeningBalance, tblCompanyUnit, tblDCStore, tblInvoiceDetail, tblInvoice, tblChalanDetail, tblChalanInfo, tblDestroyProduct, tblDCStoreFreeze, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation
- **Tables written:** none
- **Business logic:** A national-level bin-card (stock movement) report for a company/date range. Aggregates, per product, warehouse-side opening stock/receive/issue/in-transit/freeze quantities and depot-side opening stock/receive/B2B-receive/issue/bonus/sample/transit/damage/blocked quantities, then computes warehouse stock, depot stock, freeze qty, and a combined closing stock and closing-stock-value (using the latest weighted-average or last unit rate for the company/product). Filters to rows with nonzero movement or nonzero stock and orders by product code.
- **Transaction/error handling:** none.
- **Result set:** one row per product with company/product identifiers, all the WH/Depot quantity breakdowns described above, Avg_Rate, ClosingStockValue, ClosingStock, ClosingStock1, and the input FromDate/ToDate echoed back.
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/NationalBeenCaedDal.cs:25 via `aCommonInternalDal.GetDataTableAction("sp_NationalBinCard", ...)`.

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,0@CompanyId/@CompanyId5/20@CompanyId6,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NationalBinCard] 

	@FromDate DATETIME,
	@ToDate DATETIME,
	@CompanyId INT

	
AS
BEGIN
	
	SELECT DISTINCT PD.ProductCode,PD.ProductName,PD.PackSize,ISNULL(WPS.Quantity,0) WHOpeningStock,ISNULL(WHRCV.StockInQuantity,0) StockInQuantity,
(ISNULL(WPS.Quantity,0) + ISNULL(WHRCV.StockInQuantity,0)) WHReceive,
ISNULL(WHSI.WHStockIssue,0) WHStockIssue,ISNULL(WHST.WHInTransit,0) WHInTransit ,ISNULL(WHFZ.WHFreeze,0) WHFreeze,(ISNULL(WHSI.WHStockIssue,0)+ ISNULL(WHST.WHInTransit,0)+ISNULL(WHFZ.WHFreeze,0)) AS WHTotalIssue,
((ISNULL(WPS.Quantity,0) + ISNULL(WHRCV.StockInQuantity,0)) - (ISNULL(WHSI.WHStockIssue,0)+ ISNULL(WHST.WHInTransit,0)+ISNULL(WHFZ.WHFreeze,0))) AS WHStock,

ISNULL(DPS.StockQty,0) AS DepotOpeningStock,ISNULL(DRCV.TotalStockReceiveQty,0) DepotReceive,ISNULL(B2B.B2BRcvQty,0) B2BRcvQty,
(ISNULL(DPS.StockQty,0) + ISNULL(DRCV.TotalStockReceiveQty,0) + ISNULL(B2B.B2BRcvQty,0) + ISNULL(B2BTN.B2BTransitQty,0)) AS DepotTotalReceive,
ISNULL(B2BI.B2BIssueQty,0) B2BIssueQty,ISNULL(B2BTN.B2BTransitQty,0) B2BTransitQty,ISNULL(DPTS.SalesQuanty,0) SalesQuanty,
ISNULL(DPTBS.BonusQuantity,0) BonusQuantity,ISNULL(DSMPI.SampleIssue,0) SampleIssue,ISNULL(DPTN.SalesTransitQty,0) SalesTransitQty, ISNULL(DFRZ.DamageQty,0) DamageQty,ISNULL(DFRZB.BlockedQty,0) BlockedQty, 

(ISNULL(B2BI.B2BIssueQty,0) + ISNULL(DPTS.SalesQuanty,0) + ISNULL(DPTBS.BonusQuantity,0) + ISNULL(DSMPI.SampleIssue,0) + ISNULL(DPTN.SalesTransitQty,0)
+ ISNULL(DFRZ.DamageQty,0) + ISNULL(DFRZB.BlockedQty,0)) AS DepotTotalIssue,

(ISNULL(DPS.StockQty,0) + ISNULL(DRCV.TotalStockReceiveQty,0) + ISNULL(B2B.B2BRcvQty,0) + ISNULL(DFZ.FreezeQty,0)) - (ISNULL(B2BI.B2BIssueQty,0) + ISNULL(B2BTN.B2BTransitQty,0) + ISNULL(DPTS.SalesQuanty,0) + ISNULL(DPTBS.BonusQuantity,0) + ISNULL(DSMPI.SampleIssue,0) + ISNULL(DPTN.SalesTransitQty,0)
+ ISNULL(DFRZ.DamageQty,0) + ISNULL(DFRZB.BlockedQty,0)) + ISNULL(B2BTN.B2BTransitQty,0) AS DepotStock,ISNULL(DFZ.FreezeQty,0) AS FreezeQty,

((ISNULL(WPS.Quantity,0) + ISNULL(WHRCV.StockInQuantity,0)) - (ISNULL(WHSI.WHStockIssue,0)+ ISNULL(WHST.WHInTransit,0)+ISNULL(WHFZ.WHFreeze,0))) + ((ISNULL(DPS.StockQty,0) + ISNULL(DRCV.TotalStockReceiveQty,0) + ISNULL(B2B.B2BRcvQty,0) + ISNULL(B2BTN.B2BTransitQty,0)) - (ISNULL(B2BI.B2BIssueQty,0) + ISNULL(B2BTN.B2BTransitQty,0) + ISNULL(DPTS.SalesQuanty,0) + ISNULL(DPTBS.BonusQuantity,0) + ISNULL(DSMPI.SampleIssue,0) + ISNULL(DPTN.SalesTransitQty,0)
+ ISNULL(DFRZ.DamageQty,0) + ISNULL(DFRZB.BlockedQty,0)) + ISNULL(B2BTN.B2BTransitQty,0)) + ISNULL(DFZ.FreezeQty,0) AS ClosingStock,

((ISNULL(WPS.Quantity,0) + ISNULL(WHRCV.StockInQuantity,0)) - (ISNULL(WHSI.WHStockIssue,0)+ ISNULL(WHST.WHInTransit,0)+ISNULL(WHFZ.WHFreeze,0))) + ((ISNULL(DPS.StockQty,0) + ISNULL(DRCV.TotalStockReceiveQty,0) + ISNULL(B2B.B2BRcvQty,0)) - (ISNULL(B2BI.B2BIssueQty,0) + ISNULL(B2BTN.B2BTransitQty,0) + ISNULL(DPTS.SalesQuanty,0) + ISNULL(DPTBS.BonusQuantity,0) + ISNULL(DSMPI.SampleIssue,0) + ISNULL(DPTN.SalesTransitQty,0)
+ ISNULL(DFRZ.DamageQty,0) + ISNULL(DFRZB.BlockedQty,0)))  + ISNULL(B2BTN.B2BTransitQty,0) + ISNULL(DFZ.FreezeQty,0) AS ClosingStock1,

	-- Avg_Rate ??? ClosingStockValue ???? ??? ??? ???
	MAX(ISNULL(WA.Avg_Rate, 0)) AS Avg_Rate,
	MAX(ISNULL(WA.Avg_Rate, 0)) * (
		((ISNULL(WPS.Quantity,0) + ISNULL(WHRCV.StockInQuantity,0)) - (ISNULL(WHSI.WHStockIssue,0)+ ISNULL(WHST.WHInTransit,0)+ISNULL(WHFZ.WHFreeze,0))) 
		+ ((ISNULL(DPS.StockQty,0) + ISNULL(DRCV.TotalStockReceiveQty,0) + ISNULL(B2B.B2BRcvQty,0) + ISNULL(B2BTN.B2BTransitQty,0)) 
		   - (ISNULL(B2BI.B2BIssueQty,0) + ISNULL(B2BTN.B2BTransitQty,0) + ISNULL(DPTS.SalesQuanty,0) + ISNULL(DPTBS.BonusQuantity,0) + ISNULL(DSMPI.SampleIssue,0) + ISNULL(DPTN.SalesTransitQty,0) + ISNULL(DFRZ.DamageQty,0) + ISNULL(DFRZB.BlockedQty,0)) 
		   + ISNULL(B2BTN.B2BTransitQty,0)) 
		+ ISNULL(DFZ.FreezeQty,0)
	) AS ClosingStockValue,

	@FromDate AS FromDate,@ToDate AS ToDate  
	FROM tblProduct AS PD
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(Quantity) AS Quantity FROM tblCentralStore_OpeninigBalance AS WOP 
	LEFT JOIN tblProduct AS PD ON WOP.ProductCode = PD.ProductCode
	WHERE Quantity > 0 AND WOP.CSOpeninigBalanceDate = @FromDate GROUP BY PD.CompanyId,PD.ProductCode) AS WPS ON PD.ProductCode = WPS.ProductCode AND PD.CompanyId = WPS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(StockInQty) AS StockInQuantity FROM tblCentralStore AS CTS 
	LEFT JOIN tblProduct AS PD ON CTS.ProductCode = PD.ProductCode
	WHERE ReceiveDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS WHRCV ON PD.ProductCode = WHRCV.ProductCode AND PD.CompanyId = WHRCV.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ST.Quantity) WHStockIssue FROM tblStockInTransfar AS ST
	LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
	LEFT JOIN tblProduct AS PD ON ST.ProductCode = PD.ProductCode
	WHERE ReceiveIssue = 'OK' AND RQ.IssuChalanDate BETWEEN @FromDate and @ToDate
	GROUP BY PD.CompanyId,PD.ProductCode) AS WHSI ON PD.ProductCode = WHSI.ProductCode AND PD.CompanyId = WHSI.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ST.PickingQty) WHInTransit FROM tblStockInTransfar AS ST
	LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
	LEFT JOIN tblProduct AS PD ON ST.ProductCode = PD.ProductCode
	WHERE (ST.IsTransfared is null and ST.IsIssue='OK') AND PD.CompanyId = @CompanyId AND PickingDate BETWEEN @FromDate and @ToDate
	GROUP BY PD.CompanyId,PD.ProductCode) AS WHST ON PD.ProductCode = WHST.ProductCode AND PD.CompanyId = WHST.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(WF.StockQty) AS WHFreeze FROM tblWhStoreFreeze AS WF
	LEFT JOIN tblProduct AS PD ON WF.ProductId = PD.ProductId
	LEFT JOIN tblCentralStore AS CS ON WF.ReceiveId = CS.ReceiveId
	WHERE WF.ReceiveDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS WHFZ ON PD.ProductCode = WHFZ.ProductCode AND PD.CompanyId = WHFZ.CompanyId
	
	LEFT JOIN (SELECT CompanyId,ProductCode,SUM(StockQty) StockQty FROM tblDCStore_OpeningBalance AS DCP
	LEFT JOIN tblCompanyUnit AS UNT ON DCP.ComUnitId = UNT.ComUnitId
	WHERE StockQty > 0 AND DCOpeningBalanceDate = @FromDate  GROUP BY CompanyId,ProductCode) AS DPS ON PD.ProductCode = DPS.ProductCode AND PD.CompanyId = DPS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(TotalQuantity) TotalStockReceiveQty FROM tblDCStore AS DCS with(nolock) 
	INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId LEFT JOIN tblProduct AS PD ON DCS.ProductCode = PD.ProductCode 
	WHERE ChalanDetailsId IS  NULL AND StockRcvDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DRCV ON PD.ProductCode = DRCV.ProductCode AND PD.CompanyId = DRCV.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(TotalQuantity) B2BRcvQty FROM tblDCStore AS DCS with(nolock) 
	LEFT JOIN tblProduct AS PD ON DCS.ProductCode = PD.ProductCode 
	WHERE ChalanDetailsId IS NOT NULL AND ChalanDate BETWEEN @FromDate and @ToDate  GROUP BY PD.CompanyId,PD.ProductCode) AS B2B ON PD.ProductCode = B2B.ProductCode AND PD.CompanyId = B2B.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(DeliveryQuantity) SalesQuanty FROM tblInvoiceDetail AS INVD
	LEFT JOIN tblInvoice AS INV ON INVD.InvoiceId = INV.InvoiceId LEFT JOIN tblProduct AS PD ON INVD.ProductCode = PD.ProductCode
	WHERE  DeliveryStatus IN ('Full','Partial') AND  INV.InvoiceDate BETWEEN @FromDate and @ToDate 
	GROUP BY PD.CompanyId,PD.ProductCode) AS DPTS ON PD.ProductCode = DPTS.ProductCode AND PD.CompanyId = DPTS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(BonusQuantity) BonusQuantity FROM tblInvoiceDetail AS INVD
	LEFT JOIN tblInvoice AS INV ON INVD.InvoiceId = INV.InvoiceId LEFT JOIN tblProduct AS PD ON INVD.ProductCode = PD.ProductCode
	WHERE  DeliveryStatus IN ('Full','Partial') AND  INV.InvoiceDate BETWEEN @FromDate and @ToDate 
	GROUP BY PD.CompanyId,PD.ProductCode) AS DPTBS ON PD.ProductCode = DPTBS.ProductCode AND PD.CompanyId = DPTBS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(TotalQuantity) SalesTransitQty FROM tblInvoiceDetail AS INVD
	LEFT JOIN tblInvoice AS INV ON INVD.InvoiceId = INV.InvoiceId
	LEFT JOIN tblProduct AS PD ON INVD.ProductCode = PD.ProductCode
	WHERE  DeliveryStatus IS NULL AND  INV.InvoiceDate BETWEEN @FromDate and @ToDate 
	GROUP BY PD.CompanyId,PD.ProductCode) AS DPTN ON PD.ProductCode = DPTN.ProductCode AND PD.CompanyId = DPTN.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(CHD.Quantity) B2BIssueQty FROM tblChalanDetail AS CHD with(nolock)
	LEFT JOIN tblChalanInfo AS CHI ON CHD.ChalanId = CHI.ChalanId
	LEFT JOIN tblProduct AS PD ON CHD.ProductCode = PD.ProductCode
	WHERE CHI.ChalanDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS B2BI ON PD.ProductCode = B2BI.ProductCode AND PD.CompanyId = B2BI.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(CHD.Quantity) B2BTransitQty FROM tblChalanDetail AS CHD with(nolock)
	LEFT JOIN tblChalanInfo AS CHI ON CHD.ChalanId = CHI.ChalanId
	LEFT JOIN tblProduct AS PD ON CHD.ProductCode = PD.ProductCode
	WHERE IsDeliver = 'False' AND CHI.ChalanDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS B2BTN ON PD.ProductCode = B2BTN.ProductCode AND PD.CompanyId = B2BTN.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ISNULL(DP.StockOutQty,0)) DamageQty FROM tblDestroyProduct AS DP
LEFT JOIN tblDCStoreFreeze AS DCFZ ON DP.DcFreezeId = DCFZ.DCStoreFreezeId
LEFT JOIN tblProduct AS PD ON PD.ProductCode = DCFZ.ProductCode
WHERE ActionStatus = 'Accepted' AND StockOutDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DFRZ ON PD.ProductCode = DFRZ.ProductCode AND PD.CompanyId = DFRZ.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ISNULL(DCFZ.StockQty,0) + ISNULL(DCFZ.DamageQty,0)) BlockedQty FROM tblDCStoreFreeze AS DCFZ
	LEFT JOIN tblProduct AS PD ON DCFZ.ProductCode = PD.ProductCode
	WHERE DCFZ.StockCondition = 'Blocked' AND CONVERT(VARCHAR(10), FreezeRcvDate, 101) BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DFRZB ON PD.ProductCode = DFRZB.ProductCode AND PD.CompanyId = DFRZB.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ISNULL(DCFZ.StockQty,0)) FreezeQty FROM tblDCStoreFreeze AS DCFZ
	LEFT JOIN tblProduct AS PD ON DCFZ.ProductCode = PD.ProductCode
	WHERE (DCFZ.StockCondition IS NULL OR DCFZ.StockCondition = 'ReturnStock') AND CONVERT(VARCHAR(10), FreezeRcvDate, 101) BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DFZ ON PD.ProductCode = DFZ.ProductCode AND PD.CompanyId = DFZ.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,CAST(SUM(ST.Quantity*-1) AS int) SampleIssue FROM tblSampleIssue AS SMPI 
	LEFT JOIN tblSampleIssueDetail AS SMPD ON SMPI.OrderId = SMPD.OrderId
	LEFT JOIN tblProduct AS PD ON SMPD.ProductCode = PD.ProductCode
	LEFT JOIN dbo.tblSampleIssueTranscation AS ST ON ST.IssueDetailId = SMPD.OrderDetailId
	WHERE ActionStatus IN ('Accepted')  AND SubmissionDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DSMPI ON PD.ProductCode = DSMPI.ProductCode AND PD.CompanyId = DSMPI.CompanyId
	
	-- ????????? ??????? ??? ???? ???? ?????????? ???? (@CompanyId ???? ??????? ??? ?????)
	LEFT JOIN (
		SELECT cs1.ProductCode, cs1.Company_Id, cs1.Avg_Rate
		FROM dbo.tblCentralStore cs1
		INNER JOIN (
			SELECT Company_Id, ProductCode, MAX(ReceiveId) AS MaxReceiveId
			FROM dbo.tblCentralStore
			WHERE Company_Id = @CompanyId
			GROUP BY Company_Id, ProductCode
		) x
			ON x.Company_Id = cs1.Company_Id
		   AND x.ProductCode = cs1.ProductCode
		   AND x.MaxReceiveId = cs1.ReceiveId
		WHERE cs1.Company_Id = @CompanyId
	) AS WA ON PD.ProductCode = WA.ProductCode AND PD.CompanyId = WA.Company_Id
	
	WHERE PD.CompanyId = @CompanyId 
	GROUP BY PD.ProductCode,PD.ProductName,PD.PackSize,ISNULL(WPS.Quantity,0),ISNULL(WHRCV.StockInQuantity,0),ISNULL(WHSI.WHStockIssue,0),ISNULL(WHST.WHInTransit,0),ISNULL(WHFZ.WHFreeze,0),
	ISNULL(DPS.StockQty,0),ISNULL(DRCV.TotalStockReceiveQty,0),ISNULL(B2B.B2BRcvQty,0),ISNULL(B2BI.B2BIssueQty,0),ISNULL(B2BTN.B2BTransitQty,0),ISNULL(DPTS.SalesQuanty,0),DFZ.FreezeQty,
	ISNULL(DPTBS.BonusQuantity,0),ISNULL(DSMPI.SampleIssue,0),ISNULL(DPTN.SalesTransitQty,0), ISNULL(DFRZ.DamageQty,0),ISNULL(DFRZB.BlockedQty,0)
	
	HAVING ISNULL(WPS.Quantity,0) > 0 OR ISNULL(WHRCV.StockInQuantity,0) > 0 OR ISNULL(WHSI.WHStockIssue,0) > 0 OR  ISNULL(WHST.WHInTransit,0) > 0 OR ISNULL(WHFZ.WHFreeze,0) > 0 OR
	ISNULL(DPS.StockQty,0) > 0 OR ISNULL(DRCV.TotalStockReceiveQty,0) > 0 OR ISNULL(B2B.B2BRcvQty,0) > 0 OR ISNULL(B2BI.B2BIssueQty,0) > 0 OR ISNULL(B2BTN.B2BTransitQty,0) > 0 OR 
	ISNULL(DPTS.SalesQuanty,0) > 0 OR ISNULL(DPTBS.BonusQuantity,0) > 0 OR ISNULL(DSMPI.SampleIssue,0) > 0 OR ISNULL(DPTN.SalesTransitQty,0) > 0 OR ISNULL(DFRZ.DamageQty,0) > 0 OR ISNULL(DFRZB.BlockedQty,0) > 0 OR
	((ISNULL(WPS.Quantity,0) + ISNULL(WHRCV.StockInQuantity,0)) - (ISNULL(WHSI.WHStockIssue,0)+ ISNULL(WHST.WHInTransit,0)+ISNULL(WHFZ.WHFreeze,0))) 
	+ ((ISNULL(DPS.StockQty,0) + ISNULL(DRCV.TotalStockReceiveQty,0) + ISNULL(B2B.B2BRcvQty,0)) - (ISNULL(B2BI.B2BIssueQty,0) + ISNULL(B2BTN.B2BTransitQty,0) 
	+ ISNULL(DPTS.SalesQuanty,0) + ISNULL(DPTBS.BonusQuantity,0) + ISNULL(DSMPI.SampleIssue,0) + ISNULL(DPTN.SalesTransitQty,0)
	+ ISNULL(DFRZ.DamageQty,0) + ISNULL(DFRZB.BlockedQty,0))) > 0
	ORDER BY ProductCode

END
```

### sp_NationalBinCardPrice

- **Parameters:** @FromDate (datetime, IN), @ToDate (datetime, IN), @CompanyId (int, IN)
- **Tables read:** tblProduct, tblCentralStore_OpeninigBalance, tblCentralStore, tblStockInTransfar, tblRequisition, tblWhStoreFreeze, tblDCStore_OpeningBalance, tblCompanyUnit, tblStockInTransfar, tblDCStore (implicitly via tblStockInTransfar), tblInvoiceDetail, tblInvoice, tblChalanDetail, tblChalanInfo, tblDCStoreFreeze, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation
- **Tables written:** none
- **Business logic:** Companion to sp_NationalBinCard: instead of quantities, computes the single aggregate monetary value (TotalPrice) of all warehouse and depot stock movements (opening, receive, issue, in-transit, freeze, sales, bonus, B2B, damage, blocked) for a company within a date range, by summing quantity*unit-price for each movement type.
- **Transaction/error handling:** none.
- **Result set:** a single row/column, TotalPrice — the sum of all movement values for the given company and date range.
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/SalesDashboardDal.cs:379 via `_aCommonInternalDal.GetDataTableAction("sp_NationalBinCardPrice", ...)`.

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,0@CompanyId/@CompanyId5/20@CompanyId6,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NationalBinCardPrice] 

	@FromDate DATETIME,
	@ToDate DATETIME,
	@CompanyId INT

	
AS
BEGIN
	
	SELECT DISTINCT sum(ISNULL(WPS.OPPrice,0)+ISNULL(WHRCV.CTSPrice,0)+ISNULL(WHSI.STPrice,0)+ISNULL(WHFZ.WFPrice,0)+ISNULL(DPS.DCOPPrice,0)+ISNULL(DRCV.DCPrice,0)+ISNULL(B2B.B2BRcPrice,0)+ISNULL(DPTS.SalesPrice,0)+ISNULL(DPTBS.BonusPrice,0)+ISNULL(DPTN.SalesTrasitPrice,0)
	+ISNULL(B2BI.B2BIssuePrice,0)+ISNULL(B2BTN.B2BTransitPrice,0)+ISNULL(DFRZ.DamagePrice,0)+ISNULL(DFRZB.BlockedPrice,0))TotalPrice
	FROM tblProduct AS PD
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(Quantity) AS Quantity,SUM(WOP.UnitPrice* WOP.Quantity) AS OPPrice FROM tblCentralStore_OpeninigBalance AS WOP 
	LEFT JOIN tblProduct AS PD ON WOP.ProductCode = PD.ProductCode
	
	WHERE Quantity > 0 AND WOP.CSOpeninigBalanceDate = @FromDate GROUP BY PD.CompanyId,PD.ProductCode) AS WPS ON PD.ProductCode = WPS.ProductCode AND PD.CompanyId = WPS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(StockInQty) AS StockInQuantity,SUM(CTS.StockInQty*CTS.UnitPrice)CTSPrice FROM tblCentralStore AS CTS 
	LEFT JOIN tblProduct AS PD ON CTS.ProductCode = PD.ProductCode
	WHERE ReceiveDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS WHRCV ON PD.ProductCode = WHRCV.ProductCode AND PD.CompanyId = WHRCV.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ST.Quantity) WHStockIssue,SUM(st.Quantity * ST.UnitPrice) STPrice FROM tblStockInTransfar AS ST
	LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
	LEFT JOIN tblProduct AS PD ON ST.ProductCode = PD.ProductCode
	WHERE ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate and @ToDate
	GROUP BY PD.CompanyId,PD.ProductCode) AS WHSI ON PD.ProductCode = WHSI.ProductCode AND PD.CompanyId = WHSI.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ST.PickingQty) WHInTransit,SUM(ST.PickingQty * ST.UnitPrice)STTrnsPrice FROM tblStockInTransfar AS ST
	LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
	LEFT JOIN tblProduct AS PD ON ST.ProductCode = PD.ProductCode
	WHERE (ST.IsTransfared is null and ST.IsIssue='OK') AND PD.CompanyId = @CompanyId
	GROUP BY PD.CompanyId,PD.ProductCode) AS WHST ON PD.ProductCode = WHST.ProductCode AND PD.CompanyId = WHST.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(WF.StockQty) AS WHFreeze,SUM(WF.StockQty * CS.UnitPrice) WFPrice FROM tblWhStoreFreeze AS WF
	LEFT JOIN tblProduct AS PD ON WF.ProductId = PD.ProductId
	LEFT JOIN tblCentralStore AS CS ON WF.ReceiveId = CS.ReceiveId
	WHERE WF.ReceiveDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS WHFZ ON PD.ProductCode = WHFZ.ProductCode AND PD.CompanyId = WHFZ.CompanyId
	
	LEFT JOIN (SELECT CompanyId,DCP.ProductCode,SUM(StockQty) StockQty,SUM(DCP.StockQty * UnitPrice)DCOPPrice FROM tblDCStore_OpeningBalance AS DCP
	LEFT JOIN tblCompanyUnit AS UNT ON DCP.ComUnitId = UNT.ComUnitId
	LEFT JOIN dbo.tblStockInTransfar ON tblStockInTransfar.StockInTransfarId = DCP.StockInTransfarId
	WHERE StockQty > 0 AND DCOpeningBalanceDate = @FromDate  GROUP BY UNT.CompanyId,DCP.ProductCode) AS DPS ON PD.ProductCode = DPS.ProductCode AND PD.CompanyId = DPS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(TotalQuantity) TotalStockReceiveQty,SUM(DCS.TotalQuantity * CTRS.UnitPrice) DCPrice FROM tblDCStore AS DCS with(nolock) 
	INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId LEFT JOIN tblProduct AS PD ON DCS.ProductCode = PD.ProductCode 
	WHERE ChalanDetailsId IS  NULL AND StockRcvDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DRCV ON PD.ProductCode = DRCV.ProductCode AND PD.CompanyId = DRCV.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(TotalQuantity) B2BRcvQty,SUM(DCS.TotalQuantity * CTRS.UnitPrice)B2BRcPrice FROM tblDCStore AS DCS with(nolock) 
	INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId LEFT JOIN tblProduct AS PD ON DCS.ProductCode = PD.ProductCode 
	WHERE ChalanDetailsId IS NOT NULL AND StockRcvDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS B2B ON PD.ProductCode = B2B.ProductCode AND PD.CompanyId = B2B.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(DeliveryQuantity) SalesQuanty,SUM(INVD.DeliveryQuantity * INVD.UnitPrice) SalesPrice FROM tblInvoiceDetail AS INVD
	LEFT JOIN tblInvoice AS INV ON INVD.InvoiceId = INV.InvoiceId LEFT JOIN tblProduct AS PD ON INVD.ProductCode = PD.ProductCode
	WHERE  DeliveryStatus IN ('Full','Partial') AND  INV.InvoiceDate BETWEEN @FromDate and @ToDate 
	GROUP BY PD.CompanyId,PD.ProductCode) AS DPTS ON PD.ProductCode = DPTS.ProductCode AND PD.CompanyId = DPTS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(BonusQuantity) BonusQuantity,SUM(INVD.BonusQuantity * INVD.UnitPrice) AS BonusPrice FROM tblInvoiceDetail AS INVD
	LEFT JOIN tblInvoice AS INV ON INVD.InvoiceId = INV.InvoiceId LEFT JOIN tblProduct AS PD ON INVD.ProductCode = PD.ProductCode
	WHERE  DeliveryStatus IN ('Full','Partial') AND  INV.InvoiceDate BETWEEN @FromDate and @ToDate 
	GROUP BY PD.CompanyId,PD.ProductCode) AS DPTBS ON PD.ProductCode = DPTBS.ProductCode AND PD.CompanyId = DPTBS.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(TotalQuantity) SalesTransitQty,SUM(INVD.TotalQuantity*INVD.UnitPrice)AS SalesTrasitPrice FROM tblInvoiceDetail AS INVD
	LEFT JOIN tblInvoice AS INV ON INVD.InvoiceId = INV.InvoiceId
	LEFT JOIN tblProduct AS PD ON INVD.ProductCode = PD.ProductCode
	WHERE  DeliveryStatus IS NULL AND  INV.InvoiceDate BETWEEN @FromDate and @ToDate 
	GROUP BY PD.CompanyId,PD.ProductCode) AS DPTN ON PD.ProductCode = DPTN.ProductCode AND PD.CompanyId = DPTN.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(CHD.Quantity) B2BIssueQty,SUM(CHD.Quantity * CHD.UnitPrice) AS B2BIssuePrice FROM tblChalanDetail AS CHD with(nolock)
	LEFT JOIN tblChalanInfo AS CHI ON CHD.ChalanId = CHI.ChalanId
	LEFT JOIN tblProduct AS PD ON CHD.ProductCode = PD.ProductCode
	WHERE CHI.ChalanDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS B2BI ON PD.ProductCode = B2BI.ProductCode AND PD.CompanyId = B2BI.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(CHD.Quantity) B2BTransitQty,SUM(CHD.Quantity * CHD.UnitPrice) AS B2BTransitPrice FROM tblChalanDetail AS CHD with(nolock)
	LEFT JOIN tblChalanInfo AS CHI ON CHD.ChalanId = CHI.ChalanId
	LEFT JOIN tblProduct AS PD ON CHD.ProductCode = PD.ProductCode
	WHERE IsDeliver = 'False' AND CHI.ChalanDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS B2BTN ON PD.ProductCode = B2BTN.ProductCode AND PD.CompanyId = B2BTN.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ISNULL(DCFZ.StockQty,0) + ISNULL(DCFZ.DamageQty,0)) DamageQty,SUM((ISNULL(DCFZ.StockQty,0) + ISNULL(DCFZ.DamageQty,0))*DCFZ.UnitPrice_CostPrice )DamagePrice FROM tblDCStoreFreeze AS DCFZ
	LEFT JOIN tblProduct AS PD ON DCFZ.ProductCode = PD.ProductCode
		WHERE DCFZ.StockCondition = 'Restricted' AND DCFZ.StockRcvDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DFRZ ON PD.ProductCode = DFRZ.ProductCode AND PD.CompanyId = DFRZ.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,SUM(ISNULL(DCFZ.StockQty,0) + ISNULL(DCFZ.DamageQty,0)) BlockedQty,SUM((ISNULL(DCFZ.StockQty,0) + ISNULL(DCFZ.DamageQty,0)) * DCFZ.UnitPrice_CostPrice) BlockedPrice FROM tblDCStoreFreeze AS DCFZ
	LEFT JOIN tblProduct AS PD ON DCFZ.ProductCode = PD.ProductCode
	WHERE DCFZ.StockCondition = 'Blocked' AND DCFZ.StockRcvDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DFRZB ON PD.ProductCode = DFRZB.ProductCode AND PD.CompanyId = DFRZB.CompanyId
	
	LEFT JOIN (SELECT PD.CompanyId,PD.ProductCode,CAST(SUM(ST.Quantity*-1) AS int) SampleIssue FROM tblSampleIssue AS SMPI 
	LEFT JOIN tblSampleIssueDetail AS SMPD ON SMPI.OrderId = SMPD.OrderId
	LEFT JOIN tblProduct AS PD ON SMPD.ProductCode = PD.ProductCode
	LEFT JOIN dbo.tblSampleIssueTranscation AS ST ON ST.IssueDetailId = SMPD.OrderDetailId
	WHERE ActionStatus IN ('Accepted')  AND ApprovedDate BETWEEN @FromDate and @ToDate GROUP BY PD.CompanyId,PD.ProductCode) AS DSMPI ON PD.ProductCode = DSMPI.ProductCode AND PD.CompanyId = DSMPI.CompanyId
	
	WHERE PD.CompanyId = @CompanyId 
	


END
```

### sp_NationalStockReport

- **Parameters:** @parameter (nvarchar(max), IN) — a raw fragment of SQL (typically a WHERE/GROUP BY clause) injected verbatim into a dynamically built query and executed with `EXEC`.
- **Tables read:** dbo.tblDCStore, dbo.tblProduct, dbo.tblUnitPrice, tblStockInTransfar, dbo.tblCentralStore, dbo.tblWHStockInDetail, dbo.tblChalanDetail, dbo.tblInvoice, dbo.tblInvoiceDetail, dbo.tblChalanInfo, dbo.tblCompanyUnit, dbo.tblDCStoreFreeze, dbo.tblRequisition — plus whatever tables/columns the caller-supplied `@parameter` fragment references (fully dynamic, not statically resolvable)
- **Tables written:** none
- **Business logic:** Builds a national stock valuation report by UNIONing multiple stock-position sources (available depot stock, B2B-received depot stock, in-process/booked invoice stock, in-transit challan stock, restricted/blocked freeze stock, in-transit warehouse-to-depot transfer stock, and central warehouse stock), maps each product's code prefix to a company/division name (ZAS Dermal, ZAS Critical Care, etc.), and aggregates available/booked/transit/restricted/blocked/warehouse quantities and their monetary values (using cost price) per product. The final WHERE/GROUP-BY portion of the query is supplied by the caller via @parameter and concatenated directly into the dynamic SQL string, so filtering logic is fully caller-controlled (and vulnerable to SQL injection if @parameter isn't tightly controlled by the calling layer).
- **Transaction/error handling:** none.
- **Result set:** one row per product (grouped by ProductCode/ProductName/PackSize) with CompanyName (derived from product code prefix), AQty (available), BookFDel, Tqty (transit), RQty (restricted), BQty (blocked), WHQty (warehouse), TotalQty, and corresponding monetary values (AvialableValue, RestrictedValue, TransitValue, BlockedValue, WhStockValue, GrandTotal).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql

-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,0@CompanyId/@CompanyId5/20@CompanyId6,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NationalStockReport] 

	
	@parameter nvarchar(max)

	
AS
BEGIN
	declare @Query nvarchar(max)

set @Query='
	SELECT  
				CASE 
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''371'' THEN ''ZAS Dermal''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''372'' THEN ''ZAS Critical Care''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''373'' THEN ''ZAS Ophthalmic''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''379'' THEN ''ZAS Active''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''375'' THEN ''ZAS Dental ( Clinic )''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''381'' THEN ''ZAS Dental ( Lab  )''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''377'' THEN ''ZAS Gloves & Instruments''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''376'' THEN ''ZAS Gyno''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''378'' THEN ''ZAS Nutrition''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''374'' THEN ''ZAS Onco''
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''382'' THEN ''ZAS OTC''
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''383'' THEN ''ZAS NOC''
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''384'' THEN ''ZAS Baby Care''
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''385'' THEN ''ZAS Neurology''
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''386'' THEN ''ZAS Hematology''
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = ''387'' THEN ''Aqvida Bangladesh''
				ELSE '''' 
				END 
				
				   CompanyName,
                vTblTotal.ProductCode ,
                vTblTotal.ProductName ,
                vTblTotal.PackSize ,                
                SUM(vTblTotal.AvailableQty)AQty ,
                SUM(vTblTotal.BookFDel)BookFDel ,
                SUM(vTblTotal.Tqty)Tqty ,
                SUM(vTblTotal.RQty)RQty ,
                SUM(vTblTotal.BQty)BQty ,
				SUM(vTblTotal.WHQty)WHQty ,
                SUM(vTblTotal.WHQty+vTblTotal.AvailableQty+vTblTotal.RQty+vTblTotal.BQty)TotalQty ,
				SUM(vTblTotal.AvailableQty * vTblTotal.Price) AS AvialableValue,
				SUM(vTblTotal.RQty * vTblTotal.Price) AS RestrictedValue,
				SUM(vTblTotal.Tqty * vTblTotal.Price) AS TransitValue,
				SUM(vTblTotal.BQty * vTblTotal.Price) AS BlockedValue,
				SUM(vTblTotal.WHQty * vTblTotal.Price) AS WhStockValue,
				(SUM(vTblTotal.RQty * vTblTotal.Price) + SUM(vTblTotal.BQty * vTblTotal.Price) + SUM(vTblTotal.AvailableQty * vTblTotal.Price) + SUM(vTblTotal.WHQty * vTblTotal.Price)) AS GrandTotal

                FROM 
                (SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,DCS.UnitPrice_CostPrice AS Price,
                DCS.ComUnitId,0 AS WHQty  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition=''Available'' AND UP.Isactive = 1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,DCS.UnitPrice_CostPrice AS Price,
                DCS.ComUnitId,0 AS WHQty  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblChalanDetail cd ON dcs.ChalanDetailsId=CD.ChalanDetailsId
                --INNER JOIN tblDCStore dc ON CD.DCStoreId=DC.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
               -- INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = dc.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition=''Available'' AND UP.Isactive = 1
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,ID.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.TotalQuantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,DCS.UnitPrice_CostPrice AS Price,
                I.ComUnitId,0 AS WHQty
                FROM dbo.tblInvoice I 
                INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                --INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE I.DelivaryInvoiceNo IS NULL AND UP.Isactive = 1
                UNION ALL
                  SELECT p.ProductCode,p.ProductName,P.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.Quantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,DCS.UnitPrice_CostPrice AS Price,
                cu.ComUnitId,0 AS WHQty
                FROM dbo.tblChalanInfo I 
                INNER JOIN dbo.tblChalanDetail ID ON ID.ChalanId = I.ChalanId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN dbo.tblCompanyUnit cu ON i.FromComUnitCode = cU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE I.IsDeliver IS NULL AND UP.Isactive = 1
                
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCS.StockQty AS RQty,0 AS BQty,DCS.UnitPrice_CostPrice AS Price,
                DCS.ComUnitId,0 AS WHQty FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
               WHERE DCS.StockQty>0 AND DCS.StockCondition=''Restricted'' AND UP.Isactive = 1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,DCS.StockQty AS BQty,DCS.UnitPrice_CostPrice AS Price,
                DCS.ComUnitId,0 AS WHQty 
                FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition=''Blocked'' AND UP.Isactive = 1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,ST.PackSize,ST.
                BatchNo,ST.MfgDate,ST.ExpDate,0 AS AvailableQty,0 AS BookFDel,ST.Quantity AS Tqty,0 AS RQty,0 AS BQty,ST.UnitPrice AS Price,
                R.ComUnitId,0 AS WHQty FROM dbo.tblRequisition R 
                INNER JOIN dbo.tblStockInTransfar ST ON ST.ReqId = R.ReqId
                INNER JOIN dbo.tblProduct p ON p.ProductCode = ST.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                WHERE  R.ReceiveIssue IS NULL AND UP.Isactive = 1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,D.PackSize,D.
                                BatchNo,D.MfgDate,D.ExpDate,0 AS AvailableQty,0 AS BookFDel,CD.Quantity AS Tqty,0 AS RQty,0 AS BQty,CD.UnitPrice AS Price,
                                CU.ComUnitId,0 AS WHQty FROM dbo.tblChalanInfo C 
                INNER JOIN dbo.tblChalanDetail CD ON CD.ChalanId = C.ChalanId
                INNER JOIN dbo.tblDCStore D ON D.DCStoreId = CD.DCStoreId
                INNER JOIN dbo.tblCompanyUnit CU ON C.ToComUnitCode=CU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON p.ProductCode = CD.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                WHERE C.IsDeliver =''False'' AND UP.Isactive = 1
				UNION ALL
				SELECT ProductCode,ProductName,PackSize,
                BatchNo,MfgDate,ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UnitPrice AS Price,
                0 AS ComUnitId,SUM(Quantity) AS WHQty FROM dbo.tblCentralStore WHERE QUantity > 0 GROUP BY ProductCode,ProductName,PackSize,BatchNo,MfgDate,ExpDate,UnitPrice
				) AS vTblTotal				
				LEFT JOIN dbo.tblCompanyUnit Cu ON Cu.ComUnitId = vTblTotal.ComUnitId 
				
				'+ @parameter +' GROUP BY vTblTotal.ProductCode, vTblTotal.ProductName ,vTblTotal.PackSize'


END         
exec (@Query)
```

### sp_NonTranscationalInvoiceApproval

- **Parameters:** @InvReturnMasterID (int, IN), @ActionStatus (nvarchar(50), IN, NULL), @ApprovedBy (nvarchar(50), IN, NULL), @ApprovedDate (datetime, IN, NULL)
- **Tables read:** dbo.tblNonTranscationalInvoiceDetail, dbo.tblNonTranscationalInvoiceMaster, dbo.tblCompanyUnit, dbo.tblWearhouse, dbo.tblProduct, dbo.tblPackSize, dbo.tblInvoice
- **Tables written:** dbo.tblNonTranscationalInvoiceMaster, dbo.tblCreditAdjustment, dbo.tblInvoice, dbo.tblCentralStore
- **Business logic:** Older/legacy version of the non-transactional invoice return approval process. Marks the return master as Accepted, inserts a pair of offsetting tblCreditAdjustment rows (+Amount and -Amount) tied to the original invoice, reduces the invoice's due/grand-total amounts by the returned amount, and for each returned product line inserts a corresponding stock-in row into tblCentralStore (central/warehouse stock) at the original unit price. Superseded by `sp_NonTranscationalInvoiceApproval_New`, which the application actually calls. **Bug noted in the source:** the master UPDATE statement (`SET ActionStatus = 'Accepted', ApprovedBy = @ApprovedBy, @ApprovedDate = @ApprovedDate`) mistakenly reassigns the local variable `@ApprovedDate` to itself instead of setting the `ApprovedDate` column, so the ApprovedDate column is never actually updated by this procedure.
- **Transaction/error handling:** none (no BEGIN TRAN/COMMIT/ROLLBACK, no TRY/CATCH) — contrast with the `_New` version below.
- **Result set:** none (write-only procedure).
- **Caller(s):** referenced in a comment only, not actually called — D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/DestroyProductDal.cs:223 has `//return aCommonInternalDal.RunStoreProcedure("sp_NonTranscationalInvoiceApproval", ...)` commented out; the live call on the next line (224) invokes `sp_NonTranscationalInvoiceApproval_New` instead.

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NonTranscationalInvoiceApproval] 
	(
		 @InvReturnMasterID INT,	
         @ActionStatus NVARCHAR(50) = NULL,
         @ApprovedBy NVARCHAR(50) = NULL,
         @ApprovedDate DATETIME=NULL
		
	
	)
AS
BEGIN
	  
		DECLARE @ProductId int 
	    DECLARE @ProductCode NVARCHAR(50)
	    DECLARE @ProductName NVARCHAR(MAX)
	    DECLARE @PackSize NVARCHAR(50)
	    DECLARE @BatchNo NVARCHAR(50)
	    DECLARE @MfgDate DATETIME
	    DECLARE @ExpDate DATETIME
	    DECLARE @ReceiveDate DATETIME = @ApprovedDate
	    DECLARE @ChalanNo NVARCHAR(50)
	    DECLARE @ChalanDate DATETIME
	    DECLARE @StockInQty DECIMAL(18,2)
	    DECLARE @UnitPrice DECIMAL(18,2)
	    DECLARE @TotalPrice DECIMAL(18,2)
	    DECLARE @VATPerUnit DECIMAL(18,2)
	    DECLARE @TotalVAT DECIMAL(18,2)
	    DECLARE @TotalAmount DECIMAL(18,2)
	    DECLARE @StockCondition NVARCHAR(50) = 'Available'
	    DECLARE @MigoDetailID INT
	    DECLARE @ProductStockType NVARCHAR(50) = 'Regular'
	    DECLARE @WarehouseId INT
		DECLARE @Remarks NVARCHAR(MAX) = 'Non-transcational Invoice Return'
		DECLARE @TotalValue DECIMAL(18,2)

		--Credit Adjustment Entry

		DECLARE @CompanyId INT
	    DECLARE @CustomerMasterId INT
	    DECLARE @InvoiceId INT
	    DECLARE @Amount DECIMAL(18,2)
	    DECLARE @ReturnDate DATETIME
		DECLARE @NTRemarks NVARCHAR(MAX) = 'Non-transcational Invoice Return'

		--Due Invoice value Adjustment
	    DECLARE @DueAmount DECIMAL(18,2)
	        
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	
	SELECT INVD.ProductId,PD.ProductCode,PD.ProductName,INVD.Batch,INVD.ExpDate,INVD.MfgDate,
	INVM.ReturnCode,INVM.ReturnDate,INVD.Qty,INVD.Price,INVD.VAT,INVD.InvReturnDetailID,WH.WearhouseId,PD.PackSize,INVM.TotalValue
	FROM dbo.tblNonTranscationalInvoiceDetail AS INVD 
	INNER JOIN dbo.tblNonTranscationalInvoiceMaster AS INVM ON INVM.InvReturnMasterID = INVD.InvReturnMasterID
	INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INVM.ComUnitId
	INNER JOIN dbo.tblWearhouse AS WH ON WH.CompanyId = UNT.CompanyId
	INNER JOIN dbo.tblProduct AS PD ON PD.ProductId = INVD.ProductId
	INNER JOIN dbo.tblPackSize AS PKS ON PKS.PackSizeId = PD.PackSizeId
	WHERE INVD.InvReturnMasterID = @InvReturnMasterID


	--MASTER update
	UPDATE dbo.tblNonTranscationalInvoiceMaster 
	SET ActionStatus = 'Accepted', ApprovedBy = @ApprovedBy, @ApprovedDate = @ApprovedDate
	WHERE InvReturnMasterID = @InvReturnMasterID

	--Credit Adjustment Entry

	SELECT @CompanyId = CompanyId FROM tblNonTranscationalInvoiceMaster 
	INNER JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblNonTranscationalInvoiceMaster.ComUnitId WHERE InvReturnMasterID = @InvReturnMasterID
	SELECT @CustomerMasterId = CustomerMasterId FROM tblNonTranscationalInvoiceMaster WHERE InvReturnMasterID = @InvReturnMasterID
	SELECT @InvoiceId = InvoiceId FROM tblNonTranscationalInvoiceMaster WHERE InvReturnMasterID = @InvReturnMasterID
	SELECT @Amount = ISNULL(TotalValue,0) FROM tblNonTranscationalInvoiceMaster WHERE InvReturnMasterID = @InvReturnMasterID
	SELECT @ReturnDate = ReturnDate FROM tblNonTranscationalInvoiceMaster WHERE InvReturnMasterID = @InvReturnMasterID

	-- Plus amount
	INSERT	INTO dbo.tblCreditAdjustment
	(
	    CompanyId,
	    CustomerMasterId,
	    InvoiceId,
	    Amount,
	    ReturnDate,
		NTInvoiceId,
		Remarks,
	    EntryBy,
	    EntryDate
	)
	VALUES
	(   @CompanyId,         -- CompanyId - int
	    @CustomerMasterId,         -- CustomerMasterId - int
	    @InvoiceId,         -- InvoiceId - int
	    @Amount,      -- Amount - decimal(18, 2)
	    @ReturnDate, -- ReturnDate - datetime
		@InvReturnMasterID,
		@NTRemarks,
	    @ApprovedBy,       -- EntryBy - nvarchar(50)
	    @ApprovedDate  -- EntryDate - datetime
	)

	-- Minus amount
	INSERT	INTO dbo.tblCreditAdjustment
	(
	    CompanyId,
	    CustomerMasterId,
	    InvoiceId,
	    Amount,
	    ReturnDate,
		NTInvoiceId,
		Remarks,
	    EntryBy,
	    EntryDate
	)
	VALUES
	(   @CompanyId,         -- CompanyId - int
	    @CustomerMasterId,         -- CustomerMasterId - int
	    @InvoiceId,         -- InvoiceId - int
	    @Amount*-1,      -- Amount - decimal(18, 2)
	    @ReturnDate, -- ReturnDate - datetime
		@InvReturnMasterID,
		@NTRemarks,
	    @ApprovedBy,       -- EntryBy - nvarchar(50)
	    @ApprovedDate  -- EntryDate - datetime
	)

	--- Adjust due invoice value
	SELECT  @DueAmount = DeliveryTpGrandTotal FROM dbo.tblInvoice WHERE InvoiceId = @InvoiceId
	UPDATE dbo.tblInvoice 
	SET DeliveryTpGrandTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END ,
	TpTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END, 
	TpGrandTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END,
	DeliveryTpTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END  
	WHERE InvoiceId = @InvoiceId
	
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @ProductId,@ProductCode,@ProductName,@BatchNo,@ExpDate,@MfgDate,@ChalanNo,@ChalanDate,@StockInQty,@UnitPrice,@VATPerUnit,@MigoDetailID,@WarehouseId,@PackSize,@TotalValue
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	
	INSERT INTO dbo.tblCentralStore
	(	    
	    ProductId,
	    ProductCode,
	    ProductName,
	    PackSize,
	    BatchNo,
	    Quantity,
	    MfgDate,
	    ExpDate,
	    ReceiveDate,
	    ChalanNo,
	    ChalanDate,
	    StockInQty,
	    UnitPrice,
	    TotalPrice,
	    VATPerUnit,
	    TotalVAT,
	    TotalAmount,
	    StockCondition,
	    MigoDetailID,
		ProductStockType,
	    WarehouseId,
		Remarks
	)
	VALUES
	(  
	    @ProductId,         -- ProductId - int
	    @ProductCode,       -- ProductCode - nvarchar(50)
	    @ProductName,       -- ProductName - nvarchar(max)
	    @PackSize,       -- PackSize - nvarchar(50)
	    @BatchNo,       -- BatchNo - nvarchar(max)
	    @StockInQty,      -- Quantity - decimal(18, 0)
	    @MfgDate, -- MfgDate - datetime
	    @ExpDate, -- ExpDate - datetime
	    @ReceiveDate, -- ReceiveDate - datetime
	    @ChalanNo,       -- ChalanNo - nvarchar(max)
	    @ChalanDate, -- ChalanDate - datetime
	    @StockInQty,      -- StockInQty - decimal(18, 0)
	    @UnitPrice,      -- UnitPrice - decimal(18, 2)
	    (@UnitPrice*@StockInQty),      -- TotalPrice - decimal(18, 2)
	    @VATPerUnit,      -- VATPerUnit - decimal(18, 2)
	    (@VATPerUnit * @StockInQty),      -- TotalVAT - decimal(18, 2)
	    (@UnitPrice*@StockInQty) + (@VATPerUnit * @StockInQty) ,      -- TotalAmount - decimal(18, 2)
	    @StockCondition,       -- StockCondition - nvarchar(50)
	    @MigoDetailID,         -- MigoDetailID - int
	    @ProductStockType,       -- ProductStockType - nvarchar(50)
	    @WarehouseId,         -- WarehouseId - int
		@Remarks
	)
	
	
	
	FETCH NEXT FROM @MyCursor
	INTO @ProductId,@ProductCode,@ProductName,@BatchNo,@ExpDate,@MfgDate,@ChalanNo,@ChalanDate,@StockInQty,@UnitPrice,@VATPerUnit,@MigoDetailID,@WarehouseId,@PackSize,@TotalValue
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor    

END
```

### sp_NonTranscationalInvoiceApproval_New

- **Parameters:** @InvReturnMasterID (int, IN), @ActionStatus (nvarchar(50), IN, NULL, defaulted to 'Accepted' inside the body), @ApprovedBy (nvarchar(50), IN, NULL), @ApprovedDate (datetime, IN, NULL, defaulted to GETDATE() inside the body)
- **Tables read:** dbo.tblNonTranscationalInvoiceMaster, dbo.tblCompanyUnit, dbo.tblInvoice, dbo.tblNonTranscationalInvoiceDetail, dbo.tblProduct, dbo.tblPackSize, dbo.tblDCStore
- **Tables written:** dbo.tblNonTranscationalInvoiceMaster, dbo.tblCreditAdjustment, dbo.tblInvoice, dbo.tblDCStore
- **Business logic:** The current, hardened master approval procedure for non-transactional invoice returns. Validates the master record exists and is not already approved, updates its status/approver/date, posts an offsetting credit-adjustment entry against the original invoice and reduces the invoice's due amounts (only if an invoice is linked), then for each returned detail line inserts a new depot stock row (tblDCStore) and calls `sp_Process_WeightedAverage_Non_TransInv_Rcv` to recompute weighted-average cost for that receipt. Finally posts GL vouchers via `sp_Acc_NonTranscationalInvoiceReturnReceive` (finished-goods) and `sp_Acc_NonTranscationalInvoiceReturnPayable` (payable side). Supersedes the older `sp_NonTranscationalInvoiceApproval`.
- **Transaction/error handling:** Yes — wrapped in `BEGIN TRY / BEGIN TRAN ... COMMIT TRAN` with a `BEGIN CATCH` that closes/deallocates the cursor if still open, rolls back the transaction if `@@TRANCOUNT > 0`, and re-raises the original error via `RAISERROR` using captured `ERROR_MESSAGE()/ERROR_SEVERITY()/ERROR_STATE()`. Also explicitly `RAISERROR`s and rolls back/returns early for an invalid or already-approved master ID.
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/DestroyProductDal.cs:224 via `aCommonInternalDal.RunStoreProcedure("sp_NonTranscationalInvoiceApproval_New", ...)`.

```sql

-- =========================================================================
-- 4. sp_NonTranscationalInvoiceApproval_New (Master Approval Procedure)
-- =========================================================================
CREATE   PROCEDURE [dbo].[sp_NonTranscationalInvoiceApproval_New] 
(
    @InvReturnMasterID INT,	
    @ActionStatus NVARCHAR(50) = NULL,
    @ApprovedBy NVARCHAR(50) = NULL,
    @ApprovedDate DATETIME = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @CompanyId INT, @ComUnitId INT, @ProductId INT;
    DECLARE @ProductCode NVARCHAR(50), @ProductName NVARCHAR(MAX), @PackSize NVARCHAR(50), @BatchNo NVARCHAR(50);
    DECLARE @MfgDate DATETIME, @ExpDate DATETIME, @ReceiveDate DATETIME, @ChalanNo NVARCHAR(50), @ChalanDate DATETIME;
    DECLARE @StockInQty DECIMAL(18,2), @UnitPrice DECIMAL(18,2), @VATPerUnit DECIMAL(18,2), @MigoDetailID INT;
    DECLARE @StockCondition NVARCHAR(50) = 'Available', @Remarks NVARCHAR(MAX) = 'Non-transcational Invoice Return';
    DECLARE @TotalValue DECIMAL(18,2), @NewDCStoreId INT;

    DECLARE @CustomerMasterId INT, @InvoiceId INT, @Amount DECIMAL(18,2), @ReturnDate DATETIME, @DueAmount DECIMAL(18,2);

    DECLARE @MyCursor CURSOR;

    SET @ApprovedDate = ISNULL(@ApprovedDate, GETDATE());
    SET @ReceiveDate = @ApprovedDate;
    SET @ActionStatus = ISNULL(@ActionStatus, 'Accepted');

    BEGIN TRY
        BEGIN TRAN;

        -- 1. Validate Master
        IF NOT EXISTS (SELECT 1 FROM dbo.tblNonTranscationalInvoiceMaster WITH (NOLOCK) WHERE InvReturnMasterID = @InvReturnMasterID)
        BEGIN
            RAISERROR('Invalid Non-transcational invoice return ID.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        -- 2. Duplicate Approval Block
        IF EXISTS (SELECT 1 FROM dbo.tblNonTranscationalInvoiceMaster WITH (NOLOCK) WHERE InvReturnMasterID = @InvReturnMasterID AND ActionStatus = 'Accepted')
        BEGIN
            RAISERROR('This Non-transcational invoice return is already approved.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        -- 3. Load Master Info
        SELECT 
            @CompanyId = CU.CompanyId,
            @ComUnitId = M.ComUnitId,
            @CustomerMasterId = M.CustomerMasterId,
            @InvoiceId = M.InvoiceId,
            @Amount = ISNULL(M.TotalValue, 0),
            @ReturnDate = M.ReturnDate
        FROM dbo.tblNonTranscationalInvoiceMaster M WITH (NOLOCK)
        INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON CU.ComUnitId = M.ComUnitId
        WHERE M.InvReturnMasterID = @InvReturnMasterID;

        -- 4. Update Master Status
        UPDATE dbo.tblNonTranscationalInvoiceMaster 
        SET ActionStatus = @ActionStatus, ApprovedBy = @ApprovedBy, ApprovedDate = @ApprovedDate
        WHERE InvReturnMasterID = @InvReturnMasterID;

        IF @ActionStatus <> 'Accepted'
        BEGIN
            COMMIT TRAN;
            RETURN;
        END

        -- 5. Credit Adjustment
        IF ISNULL(@InvoiceId, 0) > 0
        BEGIN
            INSERT INTO dbo.tblCreditAdjustment (CompanyId, CustomerMasterId, InvoiceId, Amount, ReturnDate, NTInvoiceId, Remarks, EntryBy, EntryDate)
            VALUES (@CompanyId, @CustomerMasterId, @InvoiceId, @Amount, @ReturnDate, @InvReturnMasterID, @Remarks, @ApprovedBy, @ApprovedDate);

            SELECT @DueAmount = ISNULL(DeliveryTpGrandTotal, 0) FROM dbo.tblInvoice WITH (NOLOCK) WHERE InvoiceId = @InvoiceId;

            UPDATE dbo.tblInvoice 
            SET DeliveryTpGrandTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END,
                TpTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END,
                TpGrandTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END,
                DeliveryTpTotal = CASE WHEN @DueAmount <= @Amount THEN 0 ELSE @DueAmount - @Amount END
            WHERE InvoiceId = @InvoiceId;
        END

        -- 6. Clean Cursor without Cartesian Join
        SET @MyCursor = CURSOR FAST_FORWARD
        FOR
        SELECT 
            UNT.CompanyId,
            INVM.ComUnitId,
            INVD.ProductId,
            PD.ProductCode,
            PD.ProductName,
            INVD.Batch,
            INVD.ExpDate,
            INVD.MfgDate,
            INVM.ReturnCode,
            INVM.ReturnDate,
            INVD.Qty,
            INVD.Price,
            INVD.VAT,
            INVD.InvReturnDetailID,
            PKS.PackSizeName,
            INVM.TotalValue
        FROM dbo.tblNonTranscationalInvoiceDetail AS INVD WITH (NOLOCK)
        INNER JOIN dbo.tblNonTranscationalInvoiceMaster AS INVM WITH (NOLOCK) ON INVM.InvReturnMasterID = INVD.InvReturnMasterID
        INNER JOIN dbo.tblCompanyUnit AS UNT WITH (NOLOCK) ON UNT.ComUnitId = INVM.ComUnitId
        INNER JOIN dbo.tblProduct AS PD WITH (NOLOCK) ON PD.ProductId = INVD.ProductId
        LEFT JOIN dbo.tblPackSize AS PKS WITH (NOLOCK) ON PKS.PackSizeId = PD.PackSizeId
        WHERE INVD.InvReturnMasterID = @InvReturnMasterID;

        OPEN @MyCursor;
        FETCH NEXT FROM @MyCursor INTO @CompanyId, @ComUnitId, @ProductId, @ProductCode, @ProductName, @BatchNo, @ExpDate, @MfgDate, @ChalanNo, @ChalanDate, @StockInQty, @UnitPrice, @VATPerUnit, @MigoDetailID, @PackSize, @TotalValue;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SELECT @NewDCStoreId = ISNULL(MAX(DCStoreId), 0) + 1 FROM dbo.tblDCStore WITH (NOLOCK);

            INSERT INTO dbo.tblDCStore
            (
                DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo, TotalQuantity,
                ExpDate, ReceiveDate, ChalanNo, ChalanDate, ComUnitId, StockQty, DamageQty, StockRcvDate,
                ReqId, ReqChildId, StockInTransfarId, StockCondition, ChalanDetailsId, MfgDate, RefDcStoreId,
                TranscationDate, Remarks, ReceiveId, ReceiveDateTime, TempReceiveId, ReceiveBy, UnitPrice_CostPrice,
                IsBadDue, NonTranscationalInvReturnDetailID
            )
            VALUES
            (
                @NewDCStoreId, NULL, @ProductCode, @ProductName, @PackSize, @BatchNo, @StockInQty,
                @ExpDate, @ReceiveDate, @ChalanNo, @ChalanDate, @ComUnitId, @StockInQty, 0, @ReceiveDate,
                @InvReturnMasterID, @MigoDetailID, NULL, @StockCondition, NULL, @MfgDate, NULL,
                @ReceiveDate, @Remarks, 0, GETDATE(), 0, @ApprovedBy, @UnitPrice,
                0, @MigoDetailID
            );

            -- Process WAC
            EXEC dbo.sp_Process_WeightedAverage_Non_TransInv_Rcv @DCStoreId = @NewDCStoreId;

            FETCH NEXT FROM @MyCursor INTO @CompanyId, @ComUnitId, @ProductId, @ProductCode, @ProductName, @BatchNo, @ExpDate, @MfgDate, @ChalanNo, @ChalanDate, @StockInQty, @UnitPrice, @VATPerUnit, @MigoDetailID, @PackSize, @TotalValue;
        END;

        CLOSE @MyCursor;
        DEALLOCATE @MyCursor;

        -- 7. Accounts Voucher Posting (GL 4156 Finished Goods)
        EXEC dbo.sp_Acc_NonTranscationalInvoiceReturnReceive
             @ApprovalStatus = @ActionStatus,
             @ApprovalBy = @ApprovedBy,
             @ApprovalDate = @ApprovedDate,
             @MasterId = @InvReturnMasterID;

        -- 8. Accounts Payable Voucher Posting
        EXEC dbo.sp_Acc_NonTranscationalInvoiceReturnPayable
             @ApprovalStatus = @ActionStatus,
             @ApprovalBy = @ApprovedBy,
             @ApprovalDate = @ApprovedDate,
             @MasterId = @InvReturnMasterID;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF CURSOR_STATUS('variable', '@MyCursor') = 1 CLOSE @MyCursor;
        IF CURSOR_STATUS('variable', '@MyCursor') >= -1 DEALLOCATE @MyCursor;
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
```

### sp_PMNT_CustomerPaymentDetailInsert

- **Parameters:** @CustPayDetailId (int, OUTPUT), @CustPayId (int, IN), @InvoiceId (int, IN), @PaymentAmount (decimal(18,2), IN, NULL), @BankAccId (int, IN, NULL), @CashAccId (int, IN, NULL), @Remarks (nvarchar(500), IN, NULL), @AIT (decimal(18,2), IN, NULL), @Discount (decimal(18,2), IN, NULL)
- **Tables read:** tblCustPayDetail
- **Tables written:** tblCustPayDetail
- **Business logic:** Guards against duplicate double-submit of a payment detail line: if no matching row (same CustPayId/InvoiceId/PaymentAmount/AIT/Discount) was inserted in the last 30 seconds, inserts a new tblCustPayDetail row and returns its identity via @CustPayDetailId; otherwise returns @CustPayDetailId = 0 to signal a likely duplicate.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure; result communicated via OUTPUT parameter @CustPayDetailId).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:283 via `aInternalDal.SaveAction("sp_PMNT_CustomerPaymentDetailInsert", ..., "@CustPayDetailId")`.

```sql

CREATE PROCEDURE [dbo].[sp_PMNT_CustomerPaymentDetailInsert]
     
	@CustPayDetailId INT OUTPUT,
	@CustPayId INT,
    @InvoiceId INT,
    @PaymentAmount DECIMAL(18, 2) = NULL,
    @BankAccId INT = NULL,
    @CashAccId INT = NULL,
    @Remarks NVARCHAR(500) = NULL,
    @AIT DECIMAL(18, 2) = NULL,
    @Discount DECIMAL(18, 2) = NULL
   
AS
BEGIN


    IF NOT EXISTS (
        SELECT 1 
        FROM tblCustPayDetail
        WHERE 
            CustPayId = @CustPayId
            AND InvoiceId = @InvoiceId
            AND ISNULL(PaymentAmount, 0) = ISNULL(@PaymentAmount, 0)
            AND ISNULL(AIT, 0) = ISNULL(@AIT, 0)
            AND ISNULL(Discount, 0) = ISNULL(@Discount, 0)
            AND DATEDIFF(SECOND, ProcessTime, GETDATE()) < 30
    )
    BEGIN
        INSERT INTO tblCustPayDetail
        (
            CustPayId,
            InvoiceId,
            PaymentAmount,
            BankAccId,
            CashAccId,
            PaymentRemarks,
            ProcessTime,
            AIT,
            Discount
        )
        VALUES
        (
            @CustPayId,
            @InvoiceId,
            @PaymentAmount,
            @BankAccId,
            @CashAccId,
            @Remarks,
            GETDATE(),
            @AIT,
            @Discount
        );

        SET @CustPayDetailId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        SET @CustPayDetailId = 0;
    END
END
```

### sp_PMNT_CustomerPaymentMasterInsert

- **Parameters:** @CustPayId (int, OUT), @CustomerMasterId (int, IN, NULL), @BatchId (int, IN, NULL), @PaymentDate (datetime, IN, NULL), @PaymentAmount (decimal(18,2), IN, NULL), @PayType (nvarchar(200), IN, NULL), @RefNo (nvarchar(200), IN, NULL), @RefDate (datetime, IN, NULL), @CreateBy (nvarchar(200), IN, NULL), @PaymentRemarks (nvarchar(max), IN, NULL), @InvoiceIdForValidation (int, IN)
- **Tables read:** tblCustomerPay
- **Tables written:** tblCustomerPay
- **Business logic:** Guards against duplicate double-submit of a payment master row: if no matching row (same CreateBy/CustomerMasterId/BatchId/PaymentAmount/PaymentDate/InvoiceIdForValidation) was inserted in the last 300 seconds (5 minutes — note the inline comment says "30 seconds" but the code checks `< 300`), generates a payment code in the form `PMNT-YYMMDD` + a running count of today's payments, inserts the new tblCustomerPay row, and returns its identity via @CustPayId; otherwise returns @CustPayId = 0.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure; result communicated via OUTPUT parameter @CustPayId).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:263 via `aInternalDal.SaveAction("sp_PMNT_CustomerPaymentMasterInsert", ..., "@CustPayId")`.

```sql
-- =============================================
-- Author: <Author,Nasa>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[sp_PMNT_CustomerPaymentMasterInsert] 

	 @CustPayId INT OUT
	,@CustomerMasterId INT NULL
	,@BatchId INT NULL
    ,@PaymentDate DATETIME NULL
    ,@PaymentAmount DECIMAL(18,2) NULL
    ,@PayType NVARCHAR(200) NULL
    ,@RefNo  NVARCHAR(200) NULL
    ,@RefDate DATETIME NULL
	,@CreateBy NVARCHAR(200) NULL
	,@PaymentRemarks NVARCHAR(MAX) NULL
	,@InvoiceIdForValidation INT

AS
BEGIN

	

	IF NOT EXISTS (
	SELECT 1 
	FROM tblCustomerPay
	WHERE CreateBy = @CreateBy 
	  AND CustomerMasterId = @CustomerMasterId 
	  AND BatchId = @BatchId 
	  AND PaymentAmount = @PaymentAmount 
	  AND PaymentDate = @PaymentDate 
	  AND ISNULL(InvoiceIdForValidation,0) = @InvoiceIdForValidation
	  AND DATEDIFF(SECOND, MasterProcessTime, GETDATE()) < 300
	)
	BEGIN

		-- Declare variables

		DECLARE @SpotPurchaseCode NVARCHAR(500);
		DECLARE @ProCount INT;
	
		-- Calculate today's sequence count

		SELECT @ProCount = COUNT(CustPayId)
		FROM tblCustomerPay 
		WHERE CAST(CreateDate AS DATE) = CAST(GETDATE() AS DATE);
	
		IF @ProCount IS NULL SET @ProCount = 0;
	
		-- Generate the payment code: PMNT-YYMMDDXXXX
		SET @SpotPurchaseCode = 'PMNT-' + FORMAT(GETDATE(), 'yyMMdd') 
		                        + CAST(1000 + @ProCount AS NVARCHAR);
	
		-- Insert payment record

		INSERT INTO [dbo].[tblCustomerPay]
		(
			PaymentCode,
			CustomerMasterId,
			PaymentDate,
			PaymentAmount,
			PayType,
			RefNo,
			RefDate,
			CreateBy,
			BatchId,
			PaymentRemarks,
			InvoiceIdForValidation
		)
		VALUES
		(
			@SpotPurchaseCode,
			@CustomerMasterId,
			@PaymentDate,
			@PaymentAmount,
			@PayType,
			@RefNo,
			@RefDate,
			@CreateBy,
			@BatchId,
			@PaymentRemarks,
			@InvoiceIdForValidation
		);
	
		-- Capture the inserted identity

		SET @CustPayId = SCOPE_IDENTITY();

	END
	ELSE
	BEGIN

		-- Duplicate detected within 30 seconds

		SET @CustPayId = 0;


	END



END
```

### sp_PaymentBatchLockProcess

- **Parameters:** @BatchMasterId (int, IN)
- **Tables read:** PaymentBachMaster, PaymentBatchDetails, tblPaymentTranscationDetail, tblPaymentTranscationMaster
- **Tables written:** tblPaymentTranscationMaster
- **Business logic:** For a given payment batch, marks any not-yet-processed and not-yet-rejected payment transaction masters (IsProcess = 0 AND IsReject = 0) belonging to that batch as rejected (sets IsReject = 1) — effectively a bulk "lock/reject remaining unprocessed payments in this batch" operation.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:534 via `aDbManager.SaveAction("sp_PaymentBatchLockProcess", ...)`.

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_PaymentBatchLockProcess]
	-- Add the parameters for the stored procedure here

    @BatchMasterId INT


AS
BEGIN   

	UPDATE tblPaymentTranscationMaster SET IsReject = 1 WHERE CustPayId IN (SELECT PTD.CustPayId FROM PaymentBachMaster AS BM
	LEFT JOIN PaymentBatchDetails AS BD ON BM.BatchMasterId = BD.BatchMasterId
	LEFT JOIN tblPaymentTranscationDetail AS PTD ON BD.TranscationDetailId = PTD.CustPayDetailId
	LEFT JOIN tblPaymentTranscationMaster AS PTM ON PTD.CustPayId = PTM.CustPayId
	WHERE BM.BatchMasterId = @BatchMasterId AND IsProcess = 0 AND IsReject = 0)
		
END


--SELECT PTD.CustPayId FROM PaymentBachMaster AS BM
--	LEFT JOIN PaymentBatchDetails AS BD ON BM.BatchMasterId = BD.BatchMasterId
--	LEFT JOIN tblPaymentTranscationDetail AS PTD ON BD.TranscationDetailId = PTD.CustPayDetailId
--	LEFT JOIN tblPaymentTranscationMaster AS PTM ON PTD.CustPayId = PTM.CustPayId
--	WHERE BM.BatchMasterId = 14142 AND IsProcess = 0 AND IsReject = 0
```

### sp_Process_Error_Update

- **Parameters:** none
- **Tables read:** tblStockInTransfar, tblDCStore
- **Tables written:** tblStockInTransfar
- **Business logic:** A one-off correction/cleanup job: finds tblStockInTransfar rows whose corresponding tblDCStore row exists (matched by ReqId/ReqChildId) but which are still missing the IsTransfared = 'OK' flag, and sets them to 'OK'. Fixes stock-transfer records left in an inconsistent "not marked as transferred" state.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql

-- Create the stored procedure for deleting an employee
CREATE PROCEDURE [dbo].[sp_Process_Error_Update]

AS
BEGIN
    
	
UPDATE tblStockInTransfar SET IsTransfared = 'OK' WHERE StockInTransfarId IN (

SELECT DCS.StockInTransfarId FROM tblStockInTransfar  AS DCS
LEFT JOIN tblDCStore AS STI ON DCS.ReqId = STI.ReqId AND DCS.ReqChildId = STI.ReqChildId
WHERE STI.ReqChildId IS NOT NULL AND DCS.IsTransfared IS NULL)


END;
```

### sp_Process_CentralWeightedAverageOpening

- **Parameters:** none
- **Tables read:** tblCentralStore, tblWHStockInDetail, CSTL_ERPDB..tbl_MC_MaterialConversionDetails, CSTL_ERPDB..tbl_MC_MaterialConversionMaster, tblWHStockInMaster, tblProduct, tblWearhouse, CSTL_ERPDB..Table_Inventory_TranscationTypes, CSTL_ERPDB..Table_Inventory_Types, SalesRollDB_ZAS..tbl_INV_CentralStore_WeightedAvg, dbo.tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock
- **Tables written:** dbo.tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock
- **Business logic:** A bulk/opening-balance job for central-warehouse weighted-average costing of material-conversion (raw-material) receipts. For every tblCentralStore row with positive quantity, joins back through WH stock-in detail and material conversion master/detail records to get company/warehouse/product context, computes an opening quantity/rate/amount from the most recent existing weighted-average record for that company+item, adds the current receipt on top to get closing qty/amount/rate, and (if not already inserted for that transaction) inserts a new tbl_INV_CentralStore_WeightedAvg row plus a corresponding batch-wise stock row. Effectively seeds/backfills the central-store weighted-average ledger from raw material-conversion receipts.
- **Transaction/error handling:** none (no BEGIN TRAN/COMMIT/ROLLBACK, no TRY/CATCH) — if the cursor loop fails partway, prior inserts are not rolled back.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Process_CentralWeightedAverageOpening] 


AS
BEGIN

	--DECLARE @MasterId INT  = 2514

--SELECT * FROM tbl_INV_CentralStore_WeightedAvg

--SELECT * FROM tbl_INV_Central_BatchWiseStock


		--TRUNCATE TABLE tbl_INV_CentralStore_WeightedAvg
		--TRUNCATE TABLE tbl_INV_Central_BatchWiseStock

		 -- Insert Into Central Store

		 DECLARE @StorageLocation nvarchar(max)
         DECLARE @ProductId int
         DECLARE @ProductCode nvarchar(50)
         DECLARE @ProductName nvarchar(max)
         DECLARE @PackSize nvarchar(50)
         --DECLARE @BatchNo nvarchar(max)
         DECLARE @FinalOutputQuantity decimal(18,3)
         --DECLARE @ManufactureDate datetime
         --DECLARE @ExpiryDate datetime
         DECLARE @MaterialConversionDate datetime
         DECLARE @ChallanNo nvarchar(max)
         DECLARE @ChallanDate datetime
         DECLARE @CostPerUnit decimal(18,4)
         DECLARE @TotalPrice decimal(18,4)
         DECLARE @VATPerUnit decimal(18,4) = 0
         DECLARE @TotalVAT decimal(18,4) = 0
         --DECLARE @TotalAmount decimal(18,4) = 0
         DECLARE @StockCondition nvarchar(50) = 'Available'
         DECLARE @MigoDetailID int = 0
         --DECLARE @DeveloperRemarks nvarchar(500)
         DECLARE @ProductStockType nvarchar(50) = 'Regular'
         DECLARE @MaterialConversionCode nvarchar(50)
         DECLARE @WearhouseId int
		 DECLARE @CompanyId INT
         DECLARE @Remarks nvarchar(max) = 'Material Conversion Item Receive'
		 DECLARE @ApproveDate DATETIME

		 DECLARE @TranscationTypeId INT
		 DECLARE @InventoryTypeId INT
		 DECLARE @InventoryType NVARCHAR(200)

		 DECLARE @WHStockInDetailID INT
		 DECLARE @MaterialConversionMasterId INT


		 DECLARE @Batch NVARCHAR(200)
		 DECLARE @MfgDate DATETIME
		 DECLARE @ExpDate DATETIME

		 DECLARE @WHStockInMasterID INT
		
		      
		--------------------------------------------------------
		DECLARE @MyCursor CURSOR
		SET @MyCursor = CURSOR FAST_FORWARD
		FOR
		---------------


		SELECT WM.WHStockInMasterID,PD.ProductId,CS.ProductCode,CS.ProductName,CS.PackSize,CS.Quantity,CS.ReceiveDate,CS.ChalanNo,CS.ChalanDate, ISNULL(CS.Curr_Rate, UnitPrice) AS CostPerUnit,
		CS.Quantity*ISNULL(CS.Curr_Rate, UnitPrice) AS TotalPrice,M.MaterialConversionMasterId,WH.WearhouseId,ISNULL(M.MaterialConversionCode,WM.WHStockInCode) ReferenceCode,PD.CompanyId,
		M.ApproveDate,TSC.TranscationTypeId,ITS.InventoryTypeId,ITS.InventoryType,WD.WHStockInDetailID, WD.MaterialConversionMasterId,
		WD.Batch, ISNULL(WD.MfgDate,GETDATE()) MfgDate,ISNULL(WD.ExpDate,GETDATE()) ExpDate FROM tblCentralStore AS CS 
		LEFT JOIN tblWHStockInDetail AS WD ON CS.MigoDetailID = WD.WHStockInDetailID
		LEFT JOIN CSTL_ERPDB..tbl_MC_MaterialConversionDetails AS MCD ON WD.MaterialConversionMasterId = MCD.MaterialConversionDetailId
		LEFT JOIN CSTL_ERPDB..tbl_MC_MaterialConversionMaster AS M ON MCD.MaterialConversionMasterId = M.MaterialConversionMasterId
		LEFT JOIN tblWHStockInMaster AS WM ON WD.WHStockInMasterID = WM.WHStockInMasterID
		LEFT JOIN tblProduct AS PD ON CS.ProductId = PD.ProductId
		INNER JOIN tblWearhouse AS WH ON PD.CompanyId = WH.CompanyId
		LEFT JOIN CSTL_ERPDB..Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tbl_MC_MaterialConversionMaster')))
		LEFT JOIN CSTL_ERPDB..Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1 -- Default - Raw Material
		WHERE Quantity > 0


		----------
		OPEN @MyCursor
		FETCH NEXT FROM @MyCursor
		INTO @WHStockInMasterID,@ProductId,@ProductCode,@ProductName,@PackSize,@FinalOutputQuantity,
		@MaterialConversionDate,@ChallanNo,@ChallanDate,@CostPerUnit,@TotalPrice,@MaterialConversionMasterId,@WearhouseId,
		@MaterialConversionCode,@CompanyId,@ApproveDate,@TranscationTypeId,@InventoryTypeId,@InventoryType,
		@WHStockInDetailID,@MaterialConversionMasterId, @Batch, @MfgDate, @ExpDate
		
		WHILE @@FETCH_STATUS = 0
		BEGIN


		--PRINT @InsertBy

			-- =========================
			-- Insert Into tbl_INV_CentralStore_WeightedAvg
			-- =========================
			
			DECLARE @WH_Opening_Qty    DECIMAL(18, 3) = 0,
			        @WH_Opening_Rate   DECIMAL(18, 4) = 0,
			        @WH_Opening_Amount DECIMAL(18, 4) = 0,
			        @WH_Closing_Qty    DECIMAL(18, 3) = 0,
			        @WH_Average_Rate   DECIMAL(18, 4) = 0,
			        @WH_Closing_Amount DECIMAL(18, 4) = 0;
			
			SET @WH_Opening_Qty = ISNULL((
			    SELECT TOP 1 Close_Qty
			    FROM SalesRollDB_ZAS..tbl_INV_CentralStore_WeightedAvg
			    WHERE Company_Id = @CompanyId
			      AND Item_Id    = @ProductId
			    ORDER BY Trans_Date DESC, Weighted_Avg_Id DESC
			), 0);
			
			SET @WH_Opening_Amount = ISNULL((
			    SELECT TOP 1 Close_Amt
			    FROM SalesRollDB_ZAS..tbl_INV_CentralStore_WeightedAvg
			    WHERE Company_Id = @CompanyId
			      AND Item_Id    = @ProductId
			    ORDER BY Trans_Date DESC, Weighted_Avg_Id DESC
			), 0);
			
			SET @WH_Opening_Rate = CASE WHEN @WH_Opening_Qty > 0 THEN @WH_Opening_Amount / @WH_Opening_Qty ELSE 0 END;
			
			SET @WH_Closing_Qty    = @WH_Opening_Qty + @FinalOutputQuantity;
			SET @WH_Closing_Amount = @WH_Opening_Amount + @TotalPrice;
			
			SET @WH_Average_Rate = CASE WHEN @WH_Closing_Qty > 0 THEN @WH_Closing_Amount / @WH_Closing_Qty ELSE 0 END;
			
			IF NOT EXISTS
			(
			    SELECT 1
			    FROM dbo.tbl_INV_CentralStore_WeightedAvg
			    WHERE Mst_Trans_Id = @WHStockInMasterID
			      AND Trans_Id     = @WHStockInDetailID
			      AND Ref_No       = @MaterialConversionCode
			      AND Company_Id   = @CompanyId
			      AND Item_Id      = @ProductId
			)
			BEGIN
			    INSERT INTO dbo.tbl_INV_CentralStore_WeightedAvg
			    (
			        Mst_Trans_Id, Trans_Id, Item_Code, Ref_No, Trans_Date, Actual_Date, Entry_Date,
			        Trans_Type_Id, Inv_Type_Id, Inv_Type, Company_Id, Warehouse_Id, Item_Id,
			        OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt,
			        Weighted_Avg_Id_Issue
			    )
			    VALUES
			    (
			        @WHStockInMasterID, @WHStockInDetailID, @ProductCode, @MaterialConversionCode, GETDATE(), @MaterialConversionDate, @ApproveDate,
			        @TranscationTypeId, @InventoryTypeId, @InventoryType, @CompanyId, @WearhouseId, @ProductId,
			        @WH_Opening_Qty, @WH_Opening_Rate, @WH_Opening_Amount,
			        @FinalOutputQuantity, @CostPerUnit, @TotalPrice,
			        @WH_Closing_Qty, @WH_Average_Rate, @WH_Closing_Amount,
			        NULL
			    );


				-- Insert Into Batch Stock Table

			DECLARE @Weighted_Avg_Id_New INT = 0
			SET @Weighted_Avg_Id_New = SCOPE_IDENTITY()

			--PRINT '@Weighted_Avg_Id_Old: ' + CONVERT(NVARCHAR(200), @Weighted_Avg_Id)
			--PRINT '@Weighted_Avg_Id_New: ' + CONVERT(NVARCHAR(200), @Weighted_Avg_Id_New)

			IF(@Weighted_Avg_Id_New > 0)
			BEGIN

				DECLARE @STCount INT = 0

				--PRINT '@Batch_No: ' + CONVERT(NVARCHAR(200), @Batch_No) 
				--PRINT '@Mfg_Date: ' + CONVERT(NVARCHAR(200), @Mfg_Date)  
				--PRINT '@AdjustedStock: ' + CONVERT(NVARCHAR(200), (@AdjustedStock*-1)) 
				--PRINT '@MaterialConversionDetailId: ' + CONVERT(NVARCHAR(200), @MaterialConversionDetailId)  

				SELECT @STCount = COUNT(C.Batch_Stock_Id) FROM tbl_INV_Central_BatchWiseStock AS C WHERE Trans_Qty < 0 AND Weighted_Avg_Id = @Weighted_Avg_Id_New 
				AND Batch_No = @Batch 
				--AND CONVERT(DATE,C.Mfg_Date) = CONVERT(DATE,@MMFGDate)
				AND Trans_Qty = @FinalOutputQuantity AND WHStockInDetailId = @WHStockInDetailID AND ISNULL(WHStockInDetailId,0) > 0 

				--PRINT '@STCount: ' + CONVERT(NVARCHAR(200), @STCount) 

				IF(@STCount = 0) -- If not Exist
				BEGIN

					--PRINT 'Insert Process: tbl_INV_BatchWiseStock'

					INSERT INTO tbl_INV_Central_BatchWiseStock (Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,Trans_Qty, WHStockInDetailId, Remarks )
					VALUES (@Weighted_Avg_Id_New,@Batch,@MfgDate,@EXPDate,@FinalOutputQuantity, @WHStockInDetailId, 'WH Stock Receive')

					-- Insert 


					--SELECT Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,SUM(ISNULL(Trans_Qty,0)) Trans_Qty FROM tbl_INV_BatchWiseStock 
					--WHERE  Weighted_Avg_Id = 10187   --@Weighted_Avg_Id
					--GROUP BY Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date


				END

			END

			END

		FETCH NEXT FROM @MyCursor 
		INTO @WHStockInMasterID,@ProductId,@ProductCode,@ProductName,@PackSize,@FinalOutputQuantity,
		@MaterialConversionDate,@ChallanNo,@ChallanDate,@CostPerUnit,@TotalPrice,@MaterialConversionMasterId,@WearhouseId,
		@MaterialConversionCode,@CompanyId,@ApproveDate,@TranscationTypeId,@InventoryTypeId,@InventoryType,
		@WHStockInDetailID,@MaterialConversionMasterId, @Batch, @MfgDate, @ExpDate
		
		END
		CLOSE @MyCursor
		DEALLOCATE @MyCursor

END
```

### sp_Process_TradeMaterialTransferFullProcess

- **Parameters:** @TradeMaterialTransferId (int, IN), @ApprovedBy (nvarchar(100), IN, default 'System')
- **Tables read:** SalesRollDB_ZAS..tblTradeMaterialTransfer, SalesRollDB_ZAS..tblDCStore, SalesRollDB_ZAS..tbl_INV_WeightedAvg, CSTL_ERPDB..tbl_INV_WeightedAvg, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblChartOfAccounts, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..Table_Integration_MappingByLinkAccounts, CSTL_ERPDB..tblItemCategory, CSTL_ERPDB..tblItemName, CSTL_ERPDB..tblItemDescription, CSTL_ACCDB..tblFinancialYear
- **Tables written:** SalesRollDB_ZAS..tblTradeMaterialTransfer, SalesRollDB_ZAS..tblDCStore, SalesRollDB_ZAS..tbl_INV_WeightedAvg, SalesRollDB_ZAS..tbl_INV_BatchWiseStock, CSTL_ERPDB..tbl_INV_WeightedAvg, CSTL_ERPDB..tbl_INV_BatchWiseStock, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblDebitCreditVoucherDetail
- **Business logic:** End-to-end approval process for an inter-company "trade material transfer" (finished goods moving from one company to another as that company's raw material). Marks the transfer Approved; adjusts physical depot stock at the source (reduces StockQty on a matching/any tblDCStore row); records the source-side weighted-average stock-out (FG) and destination-side weighted-average stock-in (RM, valued at the source's WAC rate) in their respective company databases, with matching batch-wise stock rows; and, if not already posted, creates a pair of GL journal vouchers (one per company) crediting/debiting resolved GL accounts (with several hardcoded fallback account IDs such as 4154/4156/4159) to record the intercompany loan and inventory movement.
- **Transaction/error handling:** Yes — wrapped in `BEGIN TRY / BEGIN TRANSACTION ... COMMIT TRANSACTION` with a `BEGIN CATCH` that rolls back if `@@TRANCOUNT > 0` and re-raises the error via `RAISERROR`.
- **Result set:** none (write-only procedure); `SET NOCOUNT OFF` is used deliberately so the application receives row-count messages back.
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs:292 via `aInternalDal.UpdateAction("sp_Process_TradeMaterialTransferFullProcess", ...)`.

```sql

CREATE   PROCEDURE dbo.sp_Process_TradeMaterialTransferFullProcess
    @TradeMaterialTransferId INT,
    @ApprovedBy NVARCHAR(100) = 'System'
AS
BEGIN
    SET NOCOUNT OFF; -- Enabled to allow row count messages back to Web Application
    
    DECLARE @FromCompanyId INT, @FromLocationId INT, @FromProductId INT, @FromProductCode NVARCHAR(100), @FromProductName NVARCHAR(300);
    DECLARE @ToCompanyId INT, @ToLocationId INT, @ToItemDescriptionId INT, @ToItemDescriptionCode NVARCHAR(100), @ToItemDescription NVARCHAR(300);
    DECLARE @TransferQty DECIMAL(18,3);
    DECLARE @ReferenceNo NVARCHAR(100);
    
    SELECT 
        @FromCompanyId = FromCompanyId, 
        @FromLocationId = FromLocationId, 
        @FromProductId = FromProductId, 
        @FromProductCode = FromProductCode, 
        @FromProductName = FromProductName, 
        @ToCompanyId = ToCompanyId, 
        @ToLocationId = ToLocationId, 
        @ToItemDescriptionId = ToItemDescriptionId, 
        @ToItemDescriptionCode = ToItemDescriptionCode, 
        @ToItemDescription = ToItemDescription, 
        @TransferQty = TransferQty, 
        @ReferenceNo = 'TMT-' + CAST(TradeMaterialTransferId AS NVARCHAR(30)) 
    FROM SalesRollDB_ZAS..tblTradeMaterialTransfer 
    WHERE TradeMaterialTransferId = @TradeMaterialTransferId;
    
    BEGIN TRY 
        BEGIN TRANSACTION;
        
        -- 1. Update status
        UPDATE SalesRollDB_ZAS..tblTradeMaterialTransfer 
        SET ApprovalStatus = 'Approved', 
            ApprovedBy = @ApprovedBy, 
            ApprovedDate = GETDATE(), 
            UpdateBy = @ApprovedBy, 
            UpdateDate = GETDATE() 
        WHERE TradeMaterialTransferId = @TradeMaterialTransferId;
        
        -- 2. Adjust physical stock
        DECLARE @SourceBatchNo NVARCHAR(100) = 'DefaultBatch'; 
        DECLARE @SourceMfgDate DATETIME = GETDATE(); 
        DECLARE @SourceExpDate DATETIME = DATEADD(year, 2, GETDATE()); 
        DECLARE @SourceDCStoreId INT; 
        
        SELECT TOP 1 
            @SourceDCStoreId = DCStoreId, 
            @SourceBatchNo = BatchNo, 
            @SourceMfgDate = MfgDate, 
            @SourceExpDate = ExpDate 
        FROM SalesRollDB_ZAS..tblDCStore 
        WHERE ProductCode = @FromProductCode AND ComUnitId = @FromLocationId AND StockQty >= @TransferQty; 
        
        IF @SourceDCStoreId IS NULL 
        BEGIN 
            SELECT TOP 1 
                @SourceDCStoreId = DCStoreId, 
                @SourceBatchNo = BatchNo, 
                @SourceMfgDate = MfgDate, 
                @SourceExpDate = ExpDate 
            FROM SalesRollDB_ZAS..tblDCStore 
            WHERE ProductCode = @FromProductCode AND ComUnitId = @FromLocationId; 
        END 
        
        IF @SourceDCStoreId IS NOT NULL 
        BEGIN 
            UPDATE SalesRollDB_ZAS..tblDCStore 
            SET StockQty = StockQty - @TransferQty 
            WHERE DCStoreId = @SourceDCStoreId; 
        END 
        
        -- 3. Source Weighted Average calculation
        DECLARE @From_OP_Qty DECIMAL(18,3) = 0, @From_OP_Rate DECIMAL(18,4) = 0, @From_OP_Amt DECIMAL(18,4) = 0; 
        
        SELECT TOP 1 
            @From_OP_Qty = ISNULL(Close_Qty, 0), 
            @From_OP_Rate = ISNULL(Avg_Rate, 0), 
            @From_OP_Amt = ISNULL(Close_Amt, 0) 
        FROM SalesRollDB_ZAS..tbl_INV_WeightedAvg 
        WHERE Company_Id = @FromCompanyId AND Warehouse_Id = @FromLocationId AND Item_Id = @FromProductId 
        ORDER BY Weighted_Avg_Id DESC; 
        
        DECLARE @From_Curr_Qty DECIMAL(18,3) = -@TransferQty; 
        DECLARE @From_Curr_Rate DECIMAL(18,4) = @From_OP_Rate; 
        DECLARE @From_Curr_Amt DECIMAL(18,4) = @From_Curr_Qty * @From_Curr_Rate; 
        DECLARE @From_Close_Qty DECIMAL(18,3) = @From_OP_Qty + @From_Curr_Qty; 
        DECLARE @From_Close_Amt DECIMAL(18,4) = @From_Close_Qty * @From_OP_Rate; 
        
        INSERT INTO SalesRollDB_ZAS..tbl_INV_WeightedAvg 
        (
            Company_Id, Warehouse_Id, Mst_Trans_Id, Trans_Id, Item_Id, Item_Code, Ref_No, 
            Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type, 
            OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt
        ) 
        VALUES 
        (
            @FromCompanyId, @FromLocationId, @TradeMaterialTransferId, @TradeMaterialTransferId, @FromProductId, @FromProductCode, @ReferenceNo, 
            GETDATE(), GETDATE(), GETDATE(), 1010, 1, 'FG', 
            @From_OP_Qty, @From_OP_Rate, @From_OP_Amt, @From_Curr_Qty, @From_Curr_Rate, @From_Curr_Amt, @From_Close_Qty, @From_OP_Rate, @From_Close_Amt
        ); 
        
        DECLARE @NewFromWeightedAvgId INT = SCOPE_IDENTITY(); 
        
        INSERT INTO SalesRollDB_ZAS..tbl_INV_BatchWiseStock 
        (Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty, Remarks, DCStoreId) 
        VALUES 
        (@NewFromWeightedAvgId, @SourceBatchNo, @SourceMfgDate, @SourceExpDate, -@TransferQty, 'Trade Transfer FG Issue', ISNULL(@SourceDCStoreId, 0)); 
        
        -- 4. Destination Weighted Average calculation
        DECLARE @To_OP_Qty DECIMAL(18,3) = 0, @To_OP_Rate DECIMAL(18,4) = 0, @To_OP_Amt DECIMAL(18,4) = 0; 
        
        SELECT TOP 1 
            @To_OP_Qty = ISNULL(Close_Qty, 0), 
            @To_OP_Rate = ISNULL(Avg_Rate, 0), 
            @To_OP_Amt = ISNULL(Close_Amt, 0) 
        FROM CSTL_ERPDB..tbl_INV_WeightedAvg 
        WHERE Company_Id = @ToCompanyId AND Warehouse_Id = @ToLocationId AND Item_Id = @ToItemDescriptionId 
        ORDER BY Weighted_Avg_Id DESC; 
        
        DECLARE @To_Curr_Qty DECIMAL(18,3) = @TransferQty; 
        DECLARE @To_Curr_Rate DECIMAL(18,4) = @From_OP_Rate; -- Receives at From side WAC rate
        DECLARE @To_Curr_Amt DECIMAL(18,4) = @To_Curr_Qty * @To_Curr_Rate; 
        DECLARE @To_Close_Qty DECIMAL(18,3) = @To_OP_Qty + @To_Curr_Qty; 
        DECLARE @To_Close_Amt DECIMAL(18,4) = @To_OP_Amt + @To_Curr_Amt; 
        DECLARE @To_New_Avg_Rate DECIMAL(18,4) = 0; 
        
        IF @To_Close_Qty > 0 
            SET @To_New_Avg_Rate = @To_Close_Amt / @To_Close_Qty; 
        ELSE 
            SET @To_New_Avg_Rate = @To_Curr_Rate; 
            
        INSERT INTO CSTL_ERPDB..tbl_INV_WeightedAvg 
        (
            Company_Id, Warehouse_Id, Mst_Trans_Id, Trans_Id, Item_Id, Item_Code, Ref_No, 
            Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type, 
            OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt
        ) 
        VALUES 
        (
            @ToCompanyId, @ToLocationId, @TradeMaterialTransferId, @TradeMaterialTransferId, @ToItemDescriptionId, @ToItemDescriptionCode, @ReferenceNo, 
            GETDATE(), GETDATE(), GETDATE(), 2010, 2, 'RM', 
            @To_OP_Qty, @To_OP_Rate, @To_OP_Amt, @To_Curr_Qty, @To_Curr_Rate, @To_Curr_Amt, @To_Close_Qty, @To_New_Avg_Rate, @To_Close_Amt
        ); 
        
        DECLARE @NewToWeightedAvgId INT = SCOPE_IDENTITY(); 
        
        INSERT INTO CSTL_ERPDB..tbl_INV_BatchWiseStock 
        (Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty, Remarks) 
        VALUES 
        (@NewToWeightedAvgId, @SourceBatchNo, @SourceMfgDate, @SourceExpDate, @TransferQty, 'Trade Transfer RM Receive'); 
        
        -- 5. JOURNAL POSTING
        DECLARE @TransferValue DECIMAL(18, 4) = @TransferQty * @From_OP_Rate;
        
        IF @TransferValue > 0 AND NOT EXISTS (
            SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
            WHERE CommonPK_ID = @TradeMaterialTransferId AND TableID = 2026
        )
        BEGIN
            -- Retrieve GL accounts from integration mappings
            DECLARE @FromDebitGL INT, @FromCreditGL INT;
            DECLARE @ToDebitGL INT, @ToCreditGL INT;
            
            -- From Company (FG Issue)
            -- Debit: Dynamic SubSubSidiary Party Account (under 01021091001%) matching target company name
            SELECT TOP 1 @FromDebitGL = AccountId 
            FROM CSTL_ACCDB..tblChartOfAccounts 
            WHERE AccountCode LIKE '01021091001%'
              AND (AccountName = (SELECT CompanyName FROM CSTL_ERPDB..tblCompanyInfo WHERE CompanyId = @ToCompanyId)
                OR AccountName LIKE (SELECT CompanyName + '%' FROM CSTL_ERPDB..tblCompanyInfo WHERE CompanyId = @ToCompanyId));
            
            IF @FromDebitGL IS NULL OR @FromDebitGL = 0 
            BEGIN
                SELECT TOP 1 @FromDebitGL = GLId 
                FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity 
                WHERE PageId = 500090 AND FlagId = 1012;
            END
            
            IF @FromDebitGL IS NULL OR @FromDebitGL = 0 SET @FromDebitGL = 4154;
            
            -- Credit: Finished Goods Inventory (FG)
            SELECT TOP 1 @FromCreditGL = GLId 
            FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity 
            WHERE PageId = 500090 AND FlagId = 1014;
            
            IF @FromCreditGL IS NULL OR @FromCreditGL = 0 SET @FromCreditGL = 4156;
            
            -- To Company (RM Receive)
            -- Debit: Raw Material Inventory (RM) (Dynamic based on Category & Type)
            SELECT TOP 1 @ToDebitGL = M.COAId
            FROM CSTL_ERPDB..Table_Integration_MappingByLinkAccounts M
            INNER JOIN CSTL_ERPDB..tblItemCategory C ON M.CategoryId = C.ItemCategoryId
            INNER JOIN CSTL_ERPDB..tblItemName NM ON C.ItemCategoryId = NM.ItemCategoryId
            INNER JOIN CSTL_ERPDB..tblItemDescription ID ON NM.ItemNameId = ID.ItemNameId
            WHERE ID.ItemDescriptionId = @ToItemDescriptionId AND M.LinkAccountId = 1;
            
            IF @ToDebitGL IS NULL OR @ToDebitGL = 0 SET @ToDebitGL = 4159;
            
            -- Credit: Inter Company Material Loans (mapped to FlagId 1013)
            SELECT TOP 1 @ToCreditGL = GLId 
            FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity 
            WHERE PageId = 500090 AND FlagId = 1013;
            
            IF @ToCreditGL IS NULL OR @ToCreditGL = 0 SET @ToCreditGL = 4154;
            
            -- Retrieve Financial Years
            DECLARE @FromFinancialYearId INT, @ToFinancialYearId INT;
            SELECT TOP 1 @FromFinancialYearId = FinancialYearId FROM CSTL_ACCDB..tblFinancialYear WHERE CompanyId = @FromCompanyId AND CONVERT(DATE, GETDATE()) BETWEEN StartDate AND EndDate;
            SELECT TOP 1 @ToFinancialYearId = FinancialYearId FROM CSTL_ACCDB..tblFinancialYear WHERE CompanyId = @ToCompanyId AND CONVERT(DATE, GETDATE()) BETWEEN StartDate AND EndDate;
            
            -- Generate voucher codes sequentially
            DECLARE @FromVoucherCode NVARCHAR(100), @ToVoucherCode NVARCHAR(100);
            DECLARE @yearText NVARCHAR(10), @monthText NVARCHAR(10), @dateText NVARCHAR(10);
            SET @yearText = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(GETDATE())), 3, 2);
            SET @monthText = RIGHT('0' + CONVERT(NVARCHAR(10), MONTH(GETDATE())), 2);
            SET @dateText = RIGHT('0' + CONVERT(NVARCHAR(10), DAY(GETDATE())), 2);
            
            DECLARE @MaxSerial INT = 10000;
            SELECT @MaxSerial = CONVERT(INT, ISNULL(MAX(SUBSTRING(VoucherCode, 11, 5)), 10000))
            FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
            WHERE PostingDate = CONVERT(DATE, GETDATE()) AND VoucherCode LIKE 'JVP-%';
            
            SET @FromVoucherCode = 'JVP-' + (@yearText+@monthText+@dateText) + CONVERT(NVARCHAR(30), @MaxSerial + 1);
            SET @ToVoucherCode = 'JVP-' + (@yearText+@monthText+@dateText) + CONVERT(NVARCHAR(30), @MaxSerial + 2);
            
            -- Insert From Company Voucher Master
            DECLARE @FromVoucherMasterId INT;
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
            (
                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, 
                CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice, 
                TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, 
                VoucherType, ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, 
                ApprovedBy, ApprovedDate, CommonPK_ID, TableID
            )
            VALUES
            (
                @FromVoucherCode, @FromCompanyId, @FromFinancialYearId, CONVERT(DATE, GETDATE()), 2, -- 2 = Journal
                1, 1.0, 'Trade Material Transfer Issue. Ref: ' + @ReferenceNo + ', Product: ' + @FromProductName + ', Qty: ' + CAST(@TransferQty AS NVARCHAR(50)) + ', Value: ' + CONVERT(NVARCHAR(50), @TransferValue), 0, 0,
                @TransferValue, @TransferValue, 'Trade Material Transfer FG Issue', @ApprovedBy, GETDATE(),
                'Journal', 'Approved', 0, 0, 0,
                @ApprovedBy, GETDATE(), @TradeMaterialTransferId, 2026
            );
            SET @FromVoucherMasterId = SCOPE_IDENTITY();
            
            -- Insert From Company Details
            -- Debit Detail
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@FromVoucherMasterId, @FromDebitGL, 'Debit', 0, 0, @TransferValue, 0, 'Intercompany Loan (Receivable)');
            
            -- Credit Detail
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@FromVoucherMasterId, @FromCreditGL, 'Credit', 0, 0, 0, @TransferValue, 'Finished Goods Inventory Stock Out');
            
            -- Insert To Company Voucher Master
            DECLARE @ToVoucherMasterId INT;
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
            (
                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, 
                CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice, 
                TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, 
                VoucherType, ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, 
                ApprovedBy, ApprovedDate, CommonPK_ID, TableID
            )
            VALUES
            (
                @ToVoucherCode, @ToCompanyId, @ToFinancialYearId, CONVERT(DATE, GETDATE()), 2,
                1, 1.0, 'Trade Material Transfer Receive. Ref: ' + @ReferenceNo + ', Product: ' + @ToItemDescription + ', Qty: ' + CAST(@TransferQty AS NVARCHAR(50)) + ', Value: ' + CONVERT(NVARCHAR(50), @TransferValue), 0, 0,
                @TransferValue, @TransferValue, 'Trade Material Transfer RM Receive', @ApprovedBy, GETDATE(),
                'Journal', 'Approved', 0, 0, 0,
                @ApprovedBy, GETDATE(), @TradeMaterialTransferId, 2026
            );
            SET @ToVoucherMasterId = SCOPE_IDENTITY();
            
            -- Insert To Company Details
            -- Debit Detail
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@ToVoucherMasterId, @ToDebitGL, 'Debit', 0, 0, @TransferValue, 0, 'Raw Material Inventory Stock In');
            
            -- Credit Detail
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@ToVoucherMasterId, @ToCreditGL, 'Credit', 0, 0, 0, @TransferValue, 'Intercompany Loan (Payable)');
        END
        
        COMMIT TRANSACTION; 
    END TRY 
    BEGIN CATCH 
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION; 
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(); 
        RAISERROR(@ErrorMessage, 16, 1); 
    END CATCH 
END;
```

### sp_Process_WeightedAverage

- **Parameters:** @DCStoreId (int, IN)
- **Tables read:** tblDCStore, tblCompanyUnit, tblCentralStore, tblProduct, Table_Inventory_TranscationTypes, Table_Inventory_Types, tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Tables written:** tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Business logic:** Recomputes the depot-level weighted-average cost for a single depot stock receipt (@DCStoreId). Reads the opening qty/amount from the most recent tbl_INV_WeightedAvg row for that company/product, adds the current receipt qty/value, and — if no weighted-average row already exists for this DCStore receipt (duplicate-guarded by Trans_Type_Id=1 + Trans_Id) — inserts the new weighted-average row and a matching batch-wise stock row.
- **Transaction/error handling:** none.
- **Result set:** none as designed (write-only), but note the body contains a leftover debug statement `SELECT * FROM tbl_INV_WeightedAvg` inside the cursor loop (line ~101 of the body) that will return one full-table result set per loop iteration — apparent debugging leftover rather than intentional output.
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/ChalanDAL.cs:370 and D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/RequisitionDAL.cs:533, both via `aCommonInternalDal.UpdateAction("sp_Process_WeightedAverage", ...)`.

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Process_WeightedAverage] 

	@DCStoreId INT

AS
BEGIN
	 
	--DECLARE @DCStoreId INT =  59169

	-- Insert Into Waighted Average Table

	DECLARE @TransCompanyId INT
	DECLARE @TransItemDescriptionId INT
	DECLARE @ItemDescriptionCode NVARCHAR(200)
	DECLARE @CurrentQty DECIMAL(18,3)
	DECLARE @CurrentUnitValue DECIMAL(18,4)
	DECLARE @CurrentTotalValue DECIMAL(18,4)
	DECLARE @EntryDate DATETIME
	DECLARE @DeliveryLocationId1 INT
	DECLARE @CursorDCStoreId INT
	DECLARE @ChalanNo NVARCHAR(200)
	DECLARE @ApproveDate DATETIME
	DECLARE @TranscationTypeId INT
	DECLARE @InventoryTypeId INT
	DECLARE @InventoryType NVARCHAR(200)
	DECLARE @BatchNo NVARCHAR(200)
	DECLARE @MMFGDate DATETIME
	DECLARE @EXPDate DATETIME
	
	-- Declare and set the cursor
	DECLARE @TransCursor CURSOR
	SET @TransCursor = CURSOR FAST_FORWARD
	FOR

		SELECT UNT.CompanyId,PD.ProductId,PD.ProductCode,DCS.TotalQuantity,ISNULL(CS.Avg_Rate,CS.UnitPrice) CostPerUnit,DCS.TotalQuantity * ISNULL(CS.Avg_Rate,CS.UnitPrice) AS ReceiveTotalValue,
		DCS.ReceiveDate,DCS.ComUnitId DeliveryLocationId,
		DCS.DCStoreId,DCS.ChalanNo,TSC.TranscationTypeId,ITS.InventoryTypeId,ITS.InventoryType,DCS.BatchNo,DCS.MfgDate, DCS.ExpDate, DCS.ReceiveDate  FROM tblDCStore AS DCS
		INNER JOIN tblCompanyUnit AS UNT ON DCS.ComUnitId = UNT.ComUnitId
		INNER JOIN tblCentralStore AS CS ON DCS.TempReceiveId = CS.ReceiveId
		INNER JOIN tblProduct AS PD ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))
		LEFT JOIN Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tblDCStore')))
		LEFT JOIN Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1
		WHERE PD.ProductCode IS NOT NULL AND DCS.DCStoreId = @DCStoreId -- 59170

	-- Open the cursor and fetch the first row

	OPEN @TransCursor
	FETCH NEXT FROM @TransCursor
	INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
	@EntryDate,@DeliveryLocationId1,@CursorDCStoreId,@ChalanNo,@TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate
	
	-- Process each row

	WHILE @@FETCH_STATUS = 0
	BEGIN

	    -- Set Initial Values to 0

	    DECLARE @Opening_Qty DECIMAL(18, 3) = 0
	    DECLARE @Opening_Rate DECIMAL(18, 4) = 0
	    DECLARE @Opening_Amount DECIMAL(18, 4) = 0
	    DECLARE @Closing_Qty DECIMAL(18, 3) = 0
	    DECLARE @Average_Rate DECIMAL(18, 4) = 0
	    DECLARE @Closing_Amount DECIMAL(18, 4) = 0
	
	    -- Read Values from Weighted Average Page

	    SET @Opening_Qty = ISNULL((SELECT TOP 1 Close_Qty 
	                               FROM tbl_INV_WeightedAvg
	                               WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                               ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Amount = ISNULL((SELECT TOP 1 Close_Amt 
	                                  FROM tbl_INV_WeightedAvg
	                                  WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                                  ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty ELSE 0 END
	
	    SET @Closing_Qty = @Opening_Qty + @CurrentQty
	    SET @Closing_Amount = @Opening_Amount + @CurrentTotalValue
	    SET @Average_Rate = CASE WHEN @Closing_Qty > 0 THEN @Closing_Amount / @Closing_Qty ELSE 0 END
	
	    ---- Print Debug Information

	    --PRINT '@Opening_Qty:' + CONVERT(NVARCHAR, @Opening_Qty)
	    --PRINT '@Opening_Amount:' + CONVERT(NVARCHAR, @Opening_Amount)
	    --PRINT '@Opening_Rate:' + CONVERT(NVARCHAR, @Opening_Rate)
	    --PRINT '@Closing_Qty:' + CONVERT(NVARCHAR, @Closing_Qty)
	    --PRINT '@Closing_Amount:' + CONVERT(NVARCHAR, @Closing_Amount)
	    --PRINT '@Average_Rate:' + CONVERT(NVARCHAR, @Average_Rate)
	
	    -- Insert Operation


		SELECT * FROM tbl_INV_WeightedAvg

		-- Check Duplicate

		DECLARE @DuplicateCount INT = 0

		SELECT @DuplicateCount = COUNT(Weighted_Avg_Id) FROM tbl_INV_WeightedAvg AS WG 
		WHERE Trans_Type_Id = 1 -- Depot Receive
		AND Trans_Id = @CursorDCStoreId -- Receive Master ID

		--PRINT @DuplicateCount
		
		IF(@DuplicateCount = 0) -- Start of Duplicate Check in Weighted Avg
		BEGIN

			INSERT INTO tbl_INV_WeightedAvg
			(
			    Company_Id,
			    Warehouse_Id,
			    Mst_Trans_Id,
			    Trans_Id,
			    Item_Id,
			    Item_Code,
			    Ref_No,
			    Trans_Date,
			    Actual_Date,
			    Entry_Date,
			    Trans_Type_Id,
			    Inv_Type_Id,
			    Inv_Type,
			    OP_Qty,
			    OP_Rate,
			    OP_Amt,
			    Curr_Qty,
			    Curr_Rate,
			    Curr_Amt,
			    Close_Qty,
			    Avg_Rate,
			    Close_Amt
			)
			VALUES
			(
			    @TransCompanyId,
			    @DeliveryLocationId1, -- Warehouse_Id
			    0, -- Mst_Trans_Id
			    @CursorDCStoreId, --@PurchaseOrderLocalReceiveDetailId1, -- Trans_Id
			    @TransItemDescriptionId,
			    @ItemDescriptionCode,
			    @ChalanNo, -- Ref_No
			    GETDATE(), -- Trans_Date
			    @EntryDate, -- Actual_Date
			    @ApproveDate, -- Entry_Date
			    @TranscationTypeId, -- Trans_Type_Id
			    @InventoryTypeId, -- Inv_Type_Id
			    @InventoryType, -- Inv_Type
			    @Opening_Qty, -- OP_Qty
			    @Opening_Rate, -- OP_Rate
			    @Opening_Amount, -- OP_Amt
			    @CurrentQty, -- Curr_Qty
			    @CurrentUnitValue, -- Curr_Rate
			    @CurrentTotalValue, -- Curr_Amt
			    @Closing_Qty, -- Close_Qty
			    @Average_Rate, -- Avg_Rate
			    @Closing_Amount -- Close_Amt
			)



			-- Insert Into Batch Stock Table

			DECLARE @Weighted_Avg_Id_New INT = 0
			SET @Weighted_Avg_Id_New = SCOPE_IDENTITY()

			--PRINT '@Weighted_Avg_Id_Old: ' + CONVERT(NVARCHAR(200), @Weighted_Avg_Id)
			--PRINT '@Weighted_Avg_Id_New: ' + CONVERT(NVARCHAR(200), @Weighted_Avg_Id_New)

			IF(@Weighted_Avg_Id_New > 0)
			BEGIN

				DECLARE @STCount INT = 0

				--PRINT '@Batch_No: ' + CONVERT(NVARCHAR(200), @Batch_No) 
				--PRINT '@Mfg_Date: ' + CONVERT(NVARCHAR(200), @Mfg_Date)  
				--PRINT '@AdjustedStock: ' + CONVERT(NVARCHAR(200), (@AdjustedStock*-1)) 
				--PRINT '@MaterialConversionDetailId: ' + CONVERT(NVARCHAR(200), @MaterialConversionDetailId)  

				SELECT @STCount = COUNT(C.Batch_Stock_Id) FROM tbl_INV_BatchWiseStock AS C WHERE Trans_Qty < 0 AND Weighted_Avg_Id = @Weighted_Avg_Id_New 
				AND Batch_No = @BatchNo 
				--AND CONVERT(DATE,C.Mfg_Date) = CONVERT(DATE,@MMFGDate)
				AND Trans_Qty = @CurrentQty AND DCStoreId = @DCStoreId AND ISNULL(@DCStoreId,0) > 0 

				--PRINT '@STCount: ' + CONVERT(NVARCHAR(200), @STCount) 

				IF(@STCount = 0) -- If not Exist
				BEGIN

					--PRINT 'Insert Process: tbl_INV_BatchWiseStock'

					INSERT INTO tbl_INV_BatchWiseStock (Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,Trans_Qty, DCStoreId, Remarks )
					VALUES (@Weighted_Avg_Id_New,@BatchNo,@MMFGDate,@EXPDate,@CurrentQty, @DCStoreId, 'Stock Receive')

					-- Insert 


					--SELECT Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,SUM(ISNULL(Trans_Qty,0)) Trans_Qty FROM tbl_INV_BatchWiseStock 
					--WHERE  Weighted_Avg_Id = 10187   --@Weighted_Avg_Id
					--GROUP BY Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date


				END

			END
			

		END

	    

	    -- Fetch the next row

	    FETCH NEXT FROM @TransCursor 
	    INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
			 @EntryDate,@DeliveryLocationId1,@CursorDCStoreId,@ChalanNo,@TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate

	END
	
	-- Close and deallocate the cursor

	CLOSE @TransCursor
	DEALLOCATE @TransCursor  

END
```

### sp_Process_WeightedAverageB2BTransfer

- **Parameters:** @ChalanDetailsId (int, IN)
- **Tables read:** tblChalanInfo, tblChalanDetail, tblDCStore, tblCompanyUnit, tblCentralStore, tblProduct, Table_Inventory_TranscationTypes, Table_Inventory_Types, tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Tables written:** tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Business logic:** Records the weighted-average cost impact of a B2B (branch-to-branch/depot-to-depot) challan issue: for the given challan detail line, computes opening qty/rate from the latest weighted-average record for that company/product, subtracts the issued quantity/value (negated) to get the closing balance, and — if not already recorded for this challan master+detail combination — inserts the negative (issue) weighted-average row plus a linked batch-wise stock row (using the latest batch reference tied to the source DCStore).
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/ChalanDAL.cs:388 via `aCommonInternalDal.UpdateAction("sp_Process_WeightedAverageB2BTransfer", ...)`.

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Process_WeightedAverageB2BTransfer] 

	@ChalanDetailsId INT

AS
BEGIN
	  
	-- Insert Into Waighted Average Table

	DECLARE @TransCompanyId INT
	DECLARE @TransItemDescriptionId INT
	DECLARE @ItemDescriptionCode NVARCHAR(200)
	DECLARE @CurrentQty DECIMAL(18,3)
	DECLARE @CurrentUnitValue DECIMAL(18,4)
	DECLARE @CurrentTotalValue DECIMAL(18,4)
	DECLARE @EntryDate DATETIME
	DECLARE @DeliveryLocationId1 INT
	DECLARE @ChalanId INT
	DECLARE @CursorChalanDetailsId INT
	DECLARE @ChalanNo NVARCHAR(200)
	DECLARE @ApproveDate DATETIME
	DECLARE @TranscationTypeId INT
	DECLARE @InventoryTypeId INT
	DECLARE @InventoryType NVARCHAR(200)
	DECLARE @BatchNo NVARCHAR(200)
	DECLARE @MMFGDate DATETIME
	DECLARE @EXPDate DATETIME

	DECLARE @DCStoreId INT
	
	-- Declare and set the cursor

	DECLARE @TransCursor CURSOR
	SET @TransCursor = CURSOR FAST_FORWARD
	FOR
		
		SELECT UNT.CompanyId,PD.ProductId,PD.ProductCode,CD.Quantity,ISNULL(CRS.Avg_Rate,CS.UnitPrice) CostPerUnit,CD.Quantity * ISNULL(CRS.Avg_Rate,CS.UnitPrice) AS ReceiveTotalValue,
		DCS.ReceiveDate,DCS.ComUnitId DeliveryLocationId,
		CD.ChalanDetailsId,CL.ChalanId,DCS.ChalanNo,TSC.TranscationTypeId,ITS.InventoryTypeId,ITS.InventoryType,DCS.BatchNo,DCS.MfgDate, DCS.ExpDate, DCS.ReceiveDate, CD.DCStoreId FROM tblChalanInfo AS CL
		LEFT JOIN tblChalanDetail AS CD ON CL.ChalanId = CD.ChalanId
		LEFT JOIN tblDCStore AS DCS ON CD.DCStoreId = DCS.DCStoreId
		LEFT JOIN tblCompanyUnit AS UNT ON DCS.ComUnitId = UNT.ComUnitId
		LEFT JOIN tblCentralStore AS CS ON DCS.TempReceiveId = CS.ReceiveId
		LEFT JOIN tblProduct AS PD ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))
		LEFT JOIN Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tblChalanInfo')))
		LEFT JOIN Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1

		LEFT JOIN (SELECT WG.Weighted_Avg_Id,WG.Company_Id,ISNULL(WG.Warehouse_Id, 0) AS Warehouse_Id,WG.Item_Id,WG.Close_Qty AS CurrentStock2,WG.Avg_Rate,WG.Curr_Rate,WG.Trans_Date
		FROM dbo.tbl_INV_WeightedAvg WG
		JOIN (SELECT Item_Id,Company_Id,MAX(Weighted_Avg_Id) AS MaxWeighted_Avg_Id FROM dbo.tbl_INV_WeightedAvg GROUP BY Item_Id, Company_Id
		) Latest ON WG.Item_Id = Latest.Item_Id AND WG.Company_Id = Latest.Company_Id AND WG.Weighted_Avg_Id = Latest.MaxWeighted_Avg_Id
		) AS CRS ON UNT.CompanyId = CS.Company_Id AND PD.ProductId = CRS.Item_Id

		WHERE CD.ChalanDetailsId =  @ChalanDetailsId -- 20162 -- CD.ProductCode = '374003' AND

	-- Open the cursor and fetch the first row

	OPEN @TransCursor
	FETCH NEXT FROM @TransCursor
	INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
	@EntryDate,@DeliveryLocationId1,@CursorChalanDetailsId,@ChalanId,@ChalanNo,@TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate, @DCStoreId
	
	-- Process each row

	WHILE @@FETCH_STATUS = 0
	BEGIN

	    -- Set Initial Values to 0

	    DECLARE @Opening_Qty DECIMAL(18, 3) = 0
	    DECLARE @Opening_Rate DECIMAL(18, 4) = 0
	    DECLARE @Opening_Amount DECIMAL(18, 4) = 0
	    DECLARE @Closing_Qty DECIMAL(18, 3) = 0
	    DECLARE @Average_Rate DECIMAL(18, 4) = 0
	    DECLARE @Closing_Amount DECIMAL(18, 4) = 0
	
	    -- Read Values from Weighted Average Page

	    SET @Opening_Qty = ISNULL((SELECT TOP 1 Close_Qty 
	                               FROM tbl_INV_WeightedAvg
	                               WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                               ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Amount = ISNULL((SELECT TOP 1 Close_Amt 
	                                  FROM tbl_INV_WeightedAvg
	                                  WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                                  ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty ELSE 0 END
	
	    SET @Closing_Qty = @Opening_Qty + (@CurrentQty*-1)
	    SET @Closing_Amount = @Opening_Amount + (@CurrentTotalValue*-1)
	    SET @Average_Rate = CASE WHEN @Closing_Qty > 0 THEN @Closing_Amount / @Closing_Qty ELSE 0 END
	
	    ---- Print Debug Information

	    --PRINT '@Opening_Qty:' + CONVERT(NVARCHAR, @Opening_Qty)
	    --PRINT '@Opening_Amount:' + CONVERT(NVARCHAR, @Opening_Amount)
	    --PRINT '@Opening_Rate:' + CONVERT(NVARCHAR, @Opening_Rate)
	    --PRINT '@Closing_Qty:' + CONVERT(NVARCHAR, @Closing_Qty)
	    --PRINT '@Closing_Amount:' + CONVERT(NVARCHAR, @Closing_Amount)
	    --PRINT '@Average_Rate:' + CONVERT(NVARCHAR, @Average_Rate)
	
	    -- Insert Operation

		-- Check Duplicate

		DECLARE @DuplicateCount INT = 0

		SELECT @DuplicateCount = COUNT(Weighted_Avg_Id) FROM tbl_INV_WeightedAvg AS WG 
		WHERE Trans_Type_Id = 2 -- Challan
		AND Mst_Trans_Id = @ChalanId -- Challan Master ID
		AND Trans_Id = @CursorChalanDetailsId -- Challan Details ID
		

		IF(@DuplicateCount = 0) -- Start of Duplicate Check in Weighted Avg
		BEGIN

			INSERT INTO tbl_INV_WeightedAvg
			(
			    Company_Id,
			    Warehouse_Id,
			    Mst_Trans_Id,
			    Trans_Id,
			    Item_Id,
			    Item_Code,
			    Ref_No,
			    Trans_Date,
			    Actual_Date,
			    Entry_Date,
			    Trans_Type_Id,
			    Inv_Type_Id,
			    Inv_Type,
			    OP_Qty,
			    OP_Rate,
			    OP_Amt,
			    Curr_Qty,
			    Curr_Rate,
			    Curr_Amt,
			    Close_Qty,
			    Avg_Rate,
			    Close_Amt
			)
			VALUES
			(
			    @TransCompanyId,
			    @DeliveryLocationId1, -- Warehouse_Id
			    @ChalanId, -- Mst_Trans_Id
			    @CursorChalanDetailsId, --@PurchaseOrderLocalReceiveDetailId1, -- Trans_Id
			    @TransItemDescriptionId,
			    @ItemDescriptionCode,
			    @ChalanNo, -- Ref_No
			    GETDATE(), -- Trans_Date
			    @EntryDate, -- Actual_Date
			    @ApproveDate, -- Entry_Date
			    @TranscationTypeId, -- Trans_Type_Id
			    @InventoryTypeId, -- Inv_Type_Id
			    @InventoryType, -- Inv_Type
			    @Opening_Qty, -- OP_Qty
			    @Opening_Rate, -- OP_Rate
			    @Opening_Amount, -- OP_Amt
			    @CurrentQty*-1, -- Curr_Qty
			    @CurrentUnitValue, -- Curr_Rate
			    @CurrentTotalValue*-1, -- Curr_Amt
			    @Closing_Qty, -- Close_Qty
			    @Average_Rate, -- Avg_Rate
			    @Closing_Amount -- Close_Amt
			)

			-- Insert Into Batch Stock Table

			DECLARE @Weighted_Avg_Id INT = 0
			SET @Weighted_Avg_Id = SCOPE_IDENTITY()

			IF(@Weighted_Avg_Id > 0)
			BEGIN

				-- Check Already Exist or Not

				DECLARE @STCount INT = 0

				--PRINT '@Batch_No: ' + CONVERT(NVARCHAR(200), @Batch_No) 
				--PRINT '@Mfg_Date: ' + CONVERT(NVARCHAR(200), @Mfg_Date)  
				--PRINT '@AdjustedStock: ' + CONVERT(NVARCHAR(200), (@AdjustedStock*-1)) 
				--PRINT '@MaterialConversionDetailId: ' + CONVERT(NVARCHAR(200), @MaterialConversionDetailId)  

				DECLARE @Ref_Weighted_Avg_Id INT = 0
				SELECT @Ref_Weighted_Avg_Id = Weighted_Avg_Id FROM tbl_INV_BatchWiseStock 
				WHERE ISNULL(DCStoreId,0) != 0 AND DCStoreId = @DCStoreId


				SELECT @STCount = COUNT(C.Batch_Stock_Id) FROM tbl_INV_BatchWiseStock AS C WHERE Trans_Qty > 0 AND Batch_No = @BatchNo 
				AND Trans_Qty = @CurrentQty*-1 AND B2B_ChallanDetailId = @ChalanDetailsId 
				AND ISNULL(B2B_ChallanDetailId,0) > 0 

				IF(@STCount = 0) -- If not Exist
				BEGIN

					INSERT INTO tbl_INV_BatchWiseStock (Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,Trans_Qty,Remarks,B2B_ChallanDetailId, Issue_Weighted_Avg_Id)
					VALUES (@Ref_Weighted_Avg_Id,@BatchNo,@MMFGDate,@EXPDate,@CurrentQty*-1,'B2B Chalan Issue',@ChalanDetailsId,@Weighted_Avg_Id)

				END

			END

			

		END

	    

	    -- Fetch the next row

	    FETCH NEXT FROM @TransCursor 
	    INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
	    @EntryDate,@DeliveryLocationId1,@CursorChalanDetailsId,@ChalanId,@ChalanNo,@TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate, @DCStoreId

	END
	
	-- Close and deallocate the cursor

	CLOSE @TransCursor
	DEALLOCATE @TransCursor  

END
```


### sp_Process_WeightedAverageDestroyProduct

- **Parameters:** @DestroyId (int, IN)
- **Tables read:** dbo.tblDestroyProduct, dbo.tblDCStoreFreeze, dbo.tblCompanyUnit, dbo.tblProduct, dbo.tbl_INV_WeightedAvg
- **Tables written:** dbo.tbl_INV_WeightedAvg
- **Business logic:** Records the weighted-average cost impact of destroying (writing off) a quantity of finished-goods product tied to a tblDestroyProduct record. Looks up the company/product/quantity for the destroy record (via its linked freeze record), and — if a Trans_Type_Id=7 stock-out row for this DestroyId/DCStoreId doesn't already exist — computes the new closing quantity/amount at the existing average rate and inserts a negative-quantity weighted-average row (no separate batch-wise-stock insert here, unlike the other Process_WeightedAverage* variants).
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql

-- =======================================================================================
-- 8. sp_Process_WeightedAverageDestroyProduct (WAC Stock Out for Destroyed Products)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Process_WeightedAverageDestroyProduct]
    @DestroyId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT, @ComUnitId INT, @DestroyDate DATETIME;

    SELECT 
        @CompanyId = CU.CompanyId,
        @ComUnitId = DCF.ComUnitId,
        @DestroyDate = ISNULL(DP.ApproveDate, DP.StockOutDate)
    FROM dbo.tblDestroyProduct DP WITH (NOLOCK)
    INNER JOIN dbo.tblDCStoreFreeze DCF WITH (NOLOCK) ON DP.DcFreezeId = DCF.DCStoreFreezeId
    INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON DCF.ComUnitId = CU.ComUnitId
    WHERE DP.DestroyId = @DestroyId;

    DECLARE @ProductId INT, @ProductCode NVARCHAR(50), @Quantity DECIMAL(18,3), @DCStoreId INT;

    SELECT 
        @ProductId = P.ProductId,
        @ProductCode = DCF.ProductCode,
        @Quantity = DP.StockOutQty,
        @DCStoreId = DCF.DCStoreId
    FROM dbo.tblDestroyProduct DP WITH (NOLOCK)
    INNER JOIN dbo.tblDCStoreFreeze DCF WITH (NOLOCK) ON DP.DcFreezeId = DCF.DCStoreFreezeId
    LEFT JOIN dbo.tblProduct P WITH (NOLOCK) ON DCF.ProductCode = P.ProductCode
    WHERE DP.DestroyId = @DestroyId;

    IF (@ProductId IS NOT NULL AND @Quantity > 0)
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM dbo.tbl_INV_WeightedAvg WITH (NOLOCK)
            WHERE Trans_Type_Id = 7 AND Mst_Trans_Id = @DestroyId AND Trans_Id = @DCStoreId AND Curr_Qty < 0
        )
        BEGIN
            DECLARE @Opening_Qty DECIMAL(18,3) = 0, @Opening_Amount DECIMAL(18,4) = 0, @Opening_Rate DECIMAL(18,4) = 0;
            DECLARE @Closing_Qty DECIMAL(18,3) = 0, @Closing_Amount DECIMAL(18,4) = 0;

            SELECT TOP 1 
                @Opening_Qty = ISNULL(Close_Qty, 0),
                @Opening_Amount = ISNULL(Close_Amt, 0),
                @Opening_Rate = ISNULL(Avg_Rate, 0)
            FROM dbo.tbl_INV_WeightedAvg WITH (NOLOCK)
            WHERE Company_Id = @CompanyId AND Item_Id = @ProductId
            ORDER BY Weighted_Avg_Id DESC;

            SET @Closing_Qty = @Opening_Qty - @Quantity;
            SET @Closing_Amount = @Closing_Qty * @Opening_Rate;

            INSERT INTO dbo.tbl_INV_WeightedAvg
            (
                Company_Id, Warehouse_Id, Mst_Trans_Id, Trans_Id, Item_Id, Item_Code, Ref_No,
                Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type,
                OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt
            )
            VALUES
            (
                @CompanyId, @ComUnitId, @DestroyId, @DCStoreId, @ProductId, @ProductCode, 'DESTROY-' + CAST(@DestroyId AS NVARCHAR(20)),
                GETDATE(), @DestroyDate, @DestroyDate, 7, 1, 'Finished Goods',
                @Opening_Qty, @Opening_Rate, @Opening_Amount, -@Quantity, @Opening_Rate, -(@Quantity * @Opening_Rate),
                @Closing_Qty, @Opening_Rate, @Closing_Amount
            );
        END;
    END;
END;
```

### sp_Process_WeightedAverageFreezeStock

- **Parameters:** @DCStoreFreezeId (int, IN)
- **Tables read:** tblCustomerReturnMaster, tblCustomerReturnDetail, tblDCStoreFreeze, tblDCStore, tblCompanyUnit, tblCentralStore, tblProduct, Table_Inventory_TranscationTypes, Table_Inventory_Types, tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Tables written:** tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Business logic:** Records the weighted-average cost impact of stock returned from a customer and placed into freeze/restricted stock (tblDCStoreFreeze) for the given DCStoreFreezeId. Computes opening qty/rate from the latest weighted-average record for the company/product, adds the current (positive) receive quantity/value, and — if not already recorded for this challan master/detail pair — inserts the new weighted-average row plus a linked batch-wise stock row tagged with the freeze ID and remark "Receive From Customer Return Stock".
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/SInventory_DAL/ChalanDAL.cs:379 via `aCommonInternalDal.UpdateAction("sp_Process_WeightedAverageFreezeStock", ...)`.

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Process_WeightedAverageFreezeStock] 

	@DCStoreFreezeId INT

AS
BEGIN

	--DECLARE @DCStoreFreezeId INT = 61830 
	  
	-- Insert Into Waighted Average Table

	DECLARE @TransCompanyId INT
	DECLARE @TransItemDescriptionId INT
	DECLARE @ItemDescriptionCode NVARCHAR(200)
	DECLARE @CurrentQty DECIMAL(18,3)
	DECLARE @CurrentUnitValue DECIMAL(18,4)
	DECLARE @CurrentTotalValue DECIMAL(18,4)
	DECLARE @EntryDate DATETIME
	DECLARE @DeliveryLocationId1 INT
	DECLARE @ChalanId INT
	DECLARE @CursorChalanDetailsId INT
	DECLARE @ChalanNo NVARCHAR(200)
	DECLARE @ApproveDate DATETIME
	DECLARE @TranscationTypeId INT
	DECLARE @InventoryTypeId INT
	DECLARE @InventoryType NVARCHAR(200)
	DECLARE @BatchNo NVARCHAR(200)
	DECLARE @MMFGDate DATETIME
	DECLARE @EXPDate DATETIME
	
	-- Declare and set the cursor

	DECLARE @TransCursor CURSOR
	SET @TransCursor = CURSOR FAST_FORWARD
	FOR

	    --SELECT * FROM tblDCStoreFreeze WHERE DCStoreFreezeId = 61820
		
		SELECT UNT.CompanyId,PD.ProductId,PD.ProductCode,DSF.TotalQuantity,ISNULL(CS.Avg_Rate,CS.UnitPrice) CostPerUnit,DSF.TotalQuantity * ISNULL(CS.Avg_Rate,CS.UnitPrice) AS ReceiveTotalValue,
		RTM.EntryDate,DCS.ComUnitId DeliveryLocationId,
		RTM.CustReturnId,RTD.CustReturnDetailId,DCS.ChalanNo,TSC.TranscationTypeId,ITS.InventoryTypeId,ITS.InventoryType,DCS.BatchNo,DCS.MfgDate, DCS.ExpDate,GETDATE() ApprovedDate FROM tblCustomerReturnMaster AS RTM
		LEFT JOIN tblCustomerReturnDetail AS RTD ON RTM.CustReturnId = RTD.CustReturnId
		LEFT JOIN tblDCStoreFreeze AS DSF ON RTD.CustReturnDetailId = DSF.CustReturnDetailId
		LEFT JOIN tblDCStore AS DCS ON DSF.DCStoreId = DCS.DCStoreId
		INNER JOIN tblCompanyUnit AS UNT ON DCS.ComUnitId = UNT.ComUnitId
		INNER JOIN tblCentralStore AS CS ON DCS.TempReceiveId = CS.ReceiveId
		INNER JOIN tblProduct AS PD ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))
		LEFT JOIN Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tblCustomerReturnMaster')))
		LEFT JOIN Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1
		WHERE DSF.DCStoreFreezeId =  @DCStoreFreezeId -- 20162 -- CD.ProductCode = '374003' AND

	-- Open the cursor and fetch the first row

	OPEN @TransCursor
	FETCH NEXT FROM @TransCursor
	INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
	@EntryDate,@DeliveryLocationId1,@CursorChalanDetailsId,@ChalanId,@ChalanNo,@TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate
	
	-- Process each row

	WHILE @@FETCH_STATUS = 0
	BEGIN

	    -- Set Initial Values to 0

	    DECLARE @Opening_Qty DECIMAL(18, 3) = 0
	    DECLARE @Opening_Rate DECIMAL(18, 4) = 0
	    DECLARE @Opening_Amount DECIMAL(18, 4) = 0
	    DECLARE @Closing_Qty DECIMAL(18, 3) = 0
	    DECLARE @Average_Rate DECIMAL(18, 4) = 0
	    DECLARE @Closing_Amount DECIMAL(18, 4) = 0
	
	    -- Read Values from Weighted Average Page

	    SET @Opening_Qty = ISNULL((SELECT TOP 1 Close_Qty 
	                               FROM tbl_INV_WeightedAvg
	                               WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                               ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Amount = ISNULL((SELECT TOP 1 Close_Amt 
	                                  FROM tbl_INV_WeightedAvg
	                                  WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                                  ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty ELSE 0 END
	
	    SET @Closing_Qty = @Opening_Qty + @CurrentQty
	    SET @Closing_Amount = @Opening_Amount + @CurrentTotalValue
	    SET @Average_Rate = CASE WHEN @Closing_Qty > 0 THEN @Closing_Amount / @Closing_Qty ELSE 0 END
	
	    ---- Print Debug Information

	    --PRINT '@Opening_Qty:' + CONVERT(NVARCHAR, @Opening_Qty)
	    --PRINT '@Opening_Amount:' + CONVERT(NVARCHAR, @Opening_Amount)
	    --PRINT '@Opening_Rate:' + CONVERT(NVARCHAR, @Opening_Rate)
	    --PRINT '@Closing_Qty:' + CONVERT(NVARCHAR, @Closing_Qty)
	    --PRINT '@Closing_Amount:' + CONVERT(NVARCHAR, @Closing_Amount)
	    --PRINT '@Average_Rate:' + CONVERT(NVARCHAR, @Average_Rate)
	
	    -- Insert Operation

		-- Check Duplicate

		DECLARE @DuplicateCount INT = 0

		SELECT @DuplicateCount = COUNT(Weighted_Avg_Id) FROM tbl_INV_WeightedAvg AS WG 
		WHERE Trans_Type_Id = 7 -- Challan
		AND Mst_Trans_Id = @ChalanId -- Challan Master ID
		AND Trans_Id = @CursorChalanDetailsId -- Challan Details ID
		

		IF(@DuplicateCount = 0) -- Start of Duplicate Check in Weighted Avg
		BEGIN

			INSERT INTO tbl_INV_WeightedAvg
			(
			    Company_Id,
			    Warehouse_Id,
			    Mst_Trans_Id,
			    Trans_Id,
			    Item_Id,
			    Item_Code,
			    Ref_No,
			    Trans_Date,
			    Actual_Date,
			    Entry_Date,
			    Trans_Type_Id,
			    Inv_Type_Id,
			    Inv_Type,
			    OP_Qty,
			    OP_Rate,
			    OP_Amt,
			    Curr_Qty,
			    Curr_Rate,
			    Curr_Amt,
			    Close_Qty,
			    Avg_Rate,
			    Close_Amt
			)
			VALUES
			(
			    @TransCompanyId,
			    @DeliveryLocationId1, -- Warehouse_Id
			    @ChalanId, -- Mst_Trans_Id
			    @CursorChalanDetailsId, --@PurchaseOrderLocalReceiveDetailId1, -- Trans_Id
			    @TransItemDescriptionId,
			    @ItemDescriptionCode,
			    @ChalanNo, -- Ref_No
			    GETDATE(), -- Trans_Date
			    @EntryDate, -- Actual_Date
			    @ApproveDate, -- Entry_Date
			    @TranscationTypeId, -- Trans_Type_Id
			    @InventoryTypeId, -- Inv_Type_Id
			    @InventoryType, -- Inv_Type
			    @Opening_Qty, -- OP_Qty
			    @Opening_Rate, -- OP_Rate
			    @Opening_Amount, -- OP_Amt
			    @CurrentQty, -- Curr_Qty
			    @CurrentUnitValue, -- Curr_Rate
			    @CurrentTotalValue, -- Curr_Amt
			    @Closing_Qty, -- Close_Qty
			    @Average_Rate, -- Avg_Rate
			    @Closing_Amount -- Close_Amt
			)


			-- Insert Into Batch Stock Table

			DECLARE @Weighted_Avg_Id INT = 0
			SET @Weighted_Avg_Id = SCOPE_IDENTITY()

			IF(@Weighted_Avg_Id > 0)
			BEGIN

				-- Check Already Exist or Not

				DECLARE @STCount INT = 0

				--PRINT '@Batch_No: ' + CONVERT(NVARCHAR(200), @Batch_No) 
				--PRINT '@Mfg_Date: ' + CONVERT(NVARCHAR(200), @Mfg_Date)  
				--PRINT '@AdjustedStock: ' + CONVERT(NVARCHAR(200), (@AdjustedStock*-1)) 
				--PRINT '@MaterialConversionDetailId: ' + CONVERT(NVARCHAR(200), @MaterialConversionDetailId)  

				SELECT @STCount = COUNT(C.Batch_Stock_Id) FROM tbl_INV_BatchWiseStock AS C WHERE Trans_Qty > 0 AND Batch_No = @BatchNo 
				AND Trans_Qty = @CurrentQty AND DCStoreFreezeId = @DCStoreFreezeId 
				AND ISNULL(DCStoreFreezeId,0) > 0 

				IF(@STCount = 0) -- If not Exist
				BEGIN

					INSERT INTO tbl_INV_BatchWiseStock (Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,Trans_Qty,Remarks,DCStoreFreezeId)
					VALUES (@Weighted_Avg_Id,@BatchNo,@MMFGDate,@EXPDate,@CurrentQty,'Receive From Customer Return Stock',@DCStoreFreezeId)

				END

			END

			

		END

	    

	    -- Fetch the next row

	    FETCH NEXT FROM @TransCursor 
	    INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
	    @EntryDate,@DeliveryLocationId1,@CursorChalanDetailsId,@ChalanId,@ChalanNo,@TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate

	END
	
	-- Close and deallocate the cursor

	CLOSE @TransCursor
	DEALLOCATE @TransCursor  

END
```

### sp_Process_WeightedAverageOpening

- **Parameters:** none
- **Tables read:** tblDCStore, tblCompanyUnit, tblCentralStore, tblProduct, Table_Inventory_TranscationTypes, Table_Inventory_Types, tblDCStoreFreeze, tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Tables written:** tbl_INV_WeightedAvg, tbl_INV_BatchWiseStock
- **Business logic:** A bulk opening-balance job that seeds the depot-level weighted-average ledger from all current tblDCStore rows with positive stock (including any freeze quantity via tblDCStoreFreeze). For every such depot stock row, computes opening qty/rate from the latest existing weighted-average record for the company/product, adds the current quantity/value, and — if not already recorded (Trans_Type_Id=1 + Trans_Id=DCStoreId duplicate guard) — inserts a new weighted-average row plus a matching batch-wise stock row. Effectively a one-time/rerunnable initializer for tbl_INV_WeightedAvg across the whole depot stock table.
- **Transaction/error handling:** none (no BEGIN TRAN/COMMIT/ROLLBACK, no TRY/CATCH) — a large bulk job with no transactional safety net; a mid-run failure leaves partial data committed.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Process_WeightedAverageOpening] 


AS
BEGIN

	--TRUNCATE TABLE tbl_INV_WeightedAvg
	--TRUNCATE TABLE tbl_INV_BatchWiseStock
	 
	 --SELECT * FROM tbl_INV_WeightedAvg
	 --SELECT * FROM tbl_INV_BatchWiseStock

	--DECLARE @DCStoreId INT =  59169

	-- Insert Into Waighted Average Table

	DECLARE @TransCompanyId INT
	DECLARE @TransItemDescriptionId INT
	DECLARE @ItemDescriptionCode NVARCHAR(200)
	DECLARE @CurrentQty DECIMAL(18,3)
	DECLARE @CurrentUnitValue DECIMAL(18,4)
	DECLARE @CurrentTotalValue DECIMAL(18,4)
	DECLARE @EntryDate DATETIME
	DECLARE @DeliveryLocationId1 INT
	DECLARE @CursorDCStoreId INT
	DECLARE @ChalanNo NVARCHAR(200)
	DECLARE @ApproveDate DATETIME
	DECLARE @TranscationTypeId INT
	DECLARE @InventoryTypeId INT
	DECLARE @InventoryType NVARCHAR(200)
	DECLARE @BatchNo NVARCHAR(200)
	DECLARE @MMFGDate DATETIME
	DECLARE @EXPDate DATETIME

	DECLARE @DCStoreId INT
	
	-- Declare and set the cursor
	DECLARE @TransCursor CURSOR
	SET @TransCursor = CURSOR FAST_FORWARD
	FOR


	--SELECT DCStoreId,SUM(StockQty) FZQty FROM tblDCStoreFreeze WHERE StockQty > 0 GROUP BY DCStoreId

		SELECT UNT.CompanyId,PD.ProductId,PD.ProductCode,DCS.StockQty + ISNULL(FZQty,0) TotalQuantity ,ISNULL(CS.Avg_Rate,CS.UnitPrice) CostPerUnit,(DCS.StockQty + ISNULL(FZQty,0)) * ISNULL(CS.Avg_Rate,CS.UnitPrice) AS ReceiveTotalValue,
		DCS.ReceiveDate,DCS.ComUnitId DeliveryLocationId,
		DCS.DCStoreId,DCS.ChalanNo,TSC.TranscationTypeId,ITS.InventoryTypeId,ITS.InventoryType,DCS.BatchNo,DCS.MfgDate, DCS.ExpDate, DCS.ReceiveDate,DCS.DCStoreId  FROM tblDCStore AS DCS
		INNER JOIN tblCompanyUnit AS UNT ON DCS.ComUnitId = UNT.ComUnitId
		INNER JOIN tblCentralStore AS CS ON DCS.TempReceiveId = CS.ReceiveId
		INNER JOIN tblProduct AS PD ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))
		LEFT JOIN Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tblDCStore')))
		LEFT JOIN Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1
		LEFT JOIN (SELECT DCStoreId,SUM(StockQty) FZQty FROM tblDCStoreFreeze WHERE StockQty > 0 GROUP BY DCStoreId) AS FG ON DCS.DCStoreId = FG.DCStoreId
		WHERE PD.ProductCode IS NOT NULL AND (DCS.StockQty + ISNULL(FZQty,0)) > 0


	-- Open the cursor and fetch the first row

	OPEN @TransCursor
	FETCH NEXT FROM @TransCursor
	INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
	@EntryDate,@DeliveryLocationId1,@CursorDCStoreId,@ChalanNo,@TranscationTypeId,
	@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate,@DCStoreId
	
	-- Process each row

	WHILE @@FETCH_STATUS = 0
	BEGIN

	    -- Set Initial Values to 0

	    DECLARE @Opening_Qty DECIMAL(18, 3) = 0
	    DECLARE @Opening_Rate DECIMAL(18, 4) = 0
	    DECLARE @Opening_Amount DECIMAL(18, 4) = 0
	    DECLARE @Closing_Qty DECIMAL(18, 3) = 0
	    DECLARE @Average_Rate DECIMAL(18, 4) = 0
	    DECLARE @Closing_Amount DECIMAL(18, 4) = 0
	
	    -- Read Values from Weighted Average Page

	    SET @Opening_Qty = ISNULL((SELECT TOP 1 Close_Qty 
	                               FROM tbl_INV_WeightedAvg
	                               WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                               ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Amount = ISNULL((SELECT TOP 1 Close_Amt 
	                                  FROM tbl_INV_WeightedAvg
	                                  WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                                  ORDER BY Trans_Date DESC), 0)
	
	    SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty ELSE 0 END
	
	    SET @Closing_Qty = @Opening_Qty + @CurrentQty
	    SET @Closing_Amount = @Opening_Amount + @CurrentTotalValue
	    SET @Average_Rate = CASE WHEN @Closing_Qty > 0 THEN @Closing_Amount / @Closing_Qty ELSE 0 END
	
	    ---- Print Debug Information

	    --PRINT '@Opening_Qty:' + CONVERT(NVARCHAR, @Opening_Qty)
	    --PRINT '@Opening_Amount:' + CONVERT(NVARCHAR, @Opening_Amount)
	    --PRINT '@Opening_Rate:' + CONVERT(NVARCHAR, @Opening_Rate)
	    --PRINT '@Closing_Qty:' + CONVERT(NVARCHAR, @Closing_Qty)
	    --PRINT '@Closing_Amount:' + CONVERT(NVARCHAR, @Closing_Amount)
	    --PRINT '@Average_Rate:' + CONVERT(NVARCHAR, @Average_Rate)
	
	    -- Insert Operation

		-- Check Duplicate

		DECLARE @DuplicateCount INT = 0

		SELECT @DuplicateCount = COUNT(Weighted_Avg_Id) FROM tbl_INV_WeightedAvg AS WG 
		WHERE Trans_Type_Id = 1 -- Depot Receive
		AND Trans_Id = @CursorDCStoreId -- Receive Master ID

		--PRINT @DuplicateCount
		
		IF(@DuplicateCount = 0) -- Start of Duplicate Check in Weighted Avg
		BEGIN

			INSERT INTO tbl_INV_WeightedAvg
			(
			    Company_Id,
			    Warehouse_Id,
			    Mst_Trans_Id,
			    Trans_Id,
			    Item_Id,
			    Item_Code,
			    Ref_No,
			    Trans_Date,
			    Actual_Date,
			    Entry_Date,
			    Trans_Type_Id,
			    Inv_Type_Id,
			    Inv_Type,
			    OP_Qty,
			    OP_Rate,
			    OP_Amt,
			    Curr_Qty,
			    Curr_Rate,
			    Curr_Amt,
			    Close_Qty,
			    Avg_Rate,
			    Close_Amt
			)
			VALUES
			(
			    @TransCompanyId,
			    @DeliveryLocationId1, -- Warehouse_Id
			    0, -- Mst_Trans_Id
			    @CursorDCStoreId, --@PurchaseOrderLocalReceiveDetailId1, -- Trans_Id
			    @TransItemDescriptionId,
			    @ItemDescriptionCode,
			    @ChalanNo, -- Ref_No
			    GETDATE(), -- Trans_Date
			    @EntryDate, -- Actual_Date
			    @ApproveDate, -- Entry_Date
			    @TranscationTypeId, -- Trans_Type_Id
			    @InventoryTypeId, -- Inv_Type_Id
			    @InventoryType, -- Inv_Type
			    @Opening_Qty, -- OP_Qty
			    @Opening_Rate, -- OP_Rate
			    @Opening_Amount, -- OP_Amt
			    @CurrentQty, -- Curr_Qty
			    @CurrentUnitValue, -- Curr_Rate
			    @CurrentTotalValue, -- Curr_Amt
			    @Closing_Qty, -- Close_Qty
			    @Average_Rate, -- Avg_Rate
			    @Closing_Amount -- Close_Amt
			)

			-- Insert Into Batch Stock Table

			DECLARE @Weighted_Avg_Id_New INT = 0
			SET @Weighted_Avg_Id_New = SCOPE_IDENTITY()

			--PRINT '@Weighted_Avg_Id_Old: ' + CONVERT(NVARCHAR(200), @Weighted_Avg_Id)
			--PRINT '@Weighted_Avg_Id_New: ' + CONVERT(NVARCHAR(200), @Weighted_Avg_Id_New)

			IF(@Weighted_Avg_Id_New > 0)
			BEGIN

				DECLARE @STCount INT = 0

				--PRINT '@Batch_No: ' + CONVERT(NVARCHAR(200), @Batch_No) 
				--PRINT '@Mfg_Date: ' + CONVERT(NVARCHAR(200), @Mfg_Date)  
				--PRINT '@AdjustedStock: ' + CONVERT(NVARCHAR(200), (@AdjustedStock*-1)) 
				--PRINT '@MaterialConversionDetailId: ' + CONVERT(NVARCHAR(200), @MaterialConversionDetailId)  

				SELECT @STCount = COUNT(C.Batch_Stock_Id) FROM tbl_INV_BatchWiseStock AS C WHERE Trans_Qty < 0 AND Weighted_Avg_Id = @Weighted_Avg_Id_New 
				AND Batch_No = @BatchNo 
				--AND CONVERT(DATE,C.Mfg_Date) = CONVERT(DATE,@MMFGDate)
				AND Trans_Qty = @CurrentQty AND DCStoreId = @DCStoreId AND ISNULL(@DCStoreId,0) > 0 

				--PRINT '@STCount: ' + CONVERT(NVARCHAR(200), @STCount) 

				IF(@STCount = 0) -- If not Exist
				BEGIN

					--PRINT 'Insert Process: tbl_INV_BatchWiseStock'

					INSERT INTO tbl_INV_BatchWiseStock (Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,Trans_Qty, DCStoreId, Remarks )
					VALUES (@Weighted_Avg_Id_New,@BatchNo,@MMFGDate,@EXPDate,@CurrentQty, @DCStoreId, 'Stock Receive')

					-- Insert 


					--SELECT Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,SUM(ISNULL(Trans_Qty,0)) Trans_Qty FROM tbl_INV_BatchWiseStock 
					--WHERE  Weighted_Avg_Id = 10187   --@Weighted_Avg_Id
					--GROUP BY Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date


				END

			END

		END

	    

	    -- Fetch the next row

	    FETCH NEXT FROM @TransCursor 
	    INTO @TransCompanyId, @TransItemDescriptionId, @ItemDescriptionCode, @CurrentQty, @CurrentUnitValue, @CurrentTotalValue,
			 @EntryDate,@DeliveryLocationId1,@CursorDCStoreId,@ChalanNo,
			 @TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate,@DCStoreId

	END
	
	-- Close and deallocate the cursor

	CLOSE @TransCursor
	DEALLOCATE @TransCursor
	


END
```

### sp_Process_WeightedAverageSalesConfirmation

- **Parameters:** @InvoiceId (int, IN)
- **Tables read:** dbo.tblInvoice, dbo.tblCompanyUnit, dbo.tblInvoiceDetail, dbo.tblProduct, dbo.tbl_INV_WeightedAvg
- **Tables written:** dbo.tbl_INV_WeightedAvg
- **Business logic:** Records the weighted-average cost impact of a confirmed sales delivery (invoice). For each product line on the invoice, if a stock-out row (Trans_Type_Id=4) for this invoice/DCStore doesn't already exist, computes the closing quantity at the current average rate after deducting the delivered quantity and inserts a negative-quantity weighted-average row for it. Similar pattern to sp_Process_WeightedAverageDestroyProduct but for sales delivery instead of destruction.
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** UNUSED — no application caller found (no hit in callers.txt; confirmed via grep across Library.DAL, Library.BLL, Solution.Web).

```sql

-- =======================================================================================
-- 4. sp_Process_WeightedAverageSalesConfirmation (Sales Delivery Stock-out from WAC)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Process_WeightedAverageSalesConfirmation]
    @InvoiceId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT, @ComUnitId INT, @InvoiceNo NVARCHAR(100), @InvoiceDate DATETIME;

    SELECT 
        @CompanyId = CU.CompanyId,
        @ComUnitId = I.ComUnitId,
        @InvoiceNo = I.DelivaryInvoiceNo,
        @InvoiceDate = ISNULL(I.UpdateDate, I.InvoiceDate)
    FROM dbo.tblInvoice I WITH (NOLOCK)
    INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON I.ComUnitId = CU.ComUnitId
    WHERE I.InvoiceId = @InvoiceId;

    DECLARE @ProductId INT, @ProductCode NVARCHAR(50), @Quantity DECIMAL(18,3), @DCStoreId INT;

    DECLARE curDetails CURSOR LOCAL FAST_FORWARD FOR
    SELECT P.ProductId, ID.ProductCode, ISNULL(ID.DeliveryTotalQuantity, ID.TotalQuantity), ID.DCStoreId
    FROM dbo.tblInvoiceDetail ID WITH (NOLOCK)
    INNER JOIN dbo.tblProduct P WITH (NOLOCK) ON ID.ProductCode = P.ProductCode
    WHERE ID.InvoiceId = @InvoiceId;

    OPEN curDetails;
    FETCH NEXT FROM curDetails INTO @ProductId, @ProductCode, @Quantity, @DCStoreId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM dbo.tbl_INV_WeightedAvg WITH (NOLOCK)
            WHERE Trans_Type_Id = 4 AND Mst_Trans_Id = @InvoiceId AND Trans_Id = @DCStoreId AND Curr_Qty < 0
        )
        BEGIN
            DECLARE @Opening_Qty DECIMAL(18,3) = 0, @Opening_Amount DECIMAL(18,4) = 0, @Opening_Rate DECIMAL(18,4) = 0;
            DECLARE @Closing_Qty DECIMAL(18,3) = 0, @Closing_Amount DECIMAL(18,4) = 0;

            SELECT TOP 1 
                @Opening_Qty = ISNULL(Close_Qty, 0),
                @Opening_Amount = ISNULL(Close_Amt, 0),
                @Opening_Rate = ISNULL(Avg_Rate, 0)
            FROM dbo.tbl_INV_WeightedAvg WITH (NOLOCK)
            WHERE Company_Id = @CompanyId AND Item_Id = @ProductId
            ORDER BY Weighted_Avg_Id DESC;

            SET @Closing_Qty = @Opening_Qty - @Quantity;
            SET @Closing_Amount = @Closing_Qty * @Opening_Rate;

            INSERT INTO dbo.tbl_INV_WeightedAvg
            (
                Company_Id, Warehouse_Id, Mst_Trans_Id, Trans_Id, Item_Id, Item_Code, Ref_No,
                Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type,
                OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt
            )
            VALUES
            (
                @CompanyId, @ComUnitId, @InvoiceId, @DCStoreId, @ProductId, @ProductCode, @InvoiceNo,
                GETDATE(), @InvoiceDate, @InvoiceDate, 4, 1, 'Finished Goods',
                @Opening_Qty, @Opening_Rate, @Opening_Amount, -@Quantity, @Opening_Rate, -(@Quantity * @Opening_Rate),
                @Closing_Qty, @Opening_Rate, @Closing_Amount
            );
        END;

        FETCH NEXT FROM curDetails INTO @ProductId, @ProductCode, @Quantity, @DCStoreId;
    END;

    CLOSE curDetails;
    DEALLOCATE curDetails;
END;
```

### sp_Process_WeightedAverage_Non_TransInv_Rcv

- **Parameters:** @DCStoreId (int, IN)
- **Tables read:** dbo.tblDCStore, dbo.tblCompanyUnit, dbo.tblProduct, dbo.tblNonTranscationalInvoiceDetail, dbo.tblNonTranscationalInvoiceMaster, dbo.tbl_INV_WeightedAvg
- **Tables written:** dbo.tbl_INV_WeightedAvg, dbo.tbl_INV_BatchWiseStock
- **Business logic:** Records the weighted-average cost impact of a depot stock receipt originating from a non-transactional invoice return (called from `sp_NonTranscationalInvoiceApproval_New` for each new tblDCStore row it inserts). If the received quantity is positive and no positive-quantity weighted-average row already exists for this DCStoreId (Trans_Type_Id=1), computes the new closing qty/amount from the existing average rate for that company/product, inserts the weighted-average row, and (if not already present) a matching batch-wise stock row remarked "Non-transcational Invoice Return Receive".
- **Transaction/error handling:** none.
- **Result set:** none (write-only procedure).
- **Caller(s):** D:/Projects/Zas Distribution/Library.DAL/... not directly called from the DAL layer — invoked internally from T-SQL by `sp_NonTranscationalInvoiceApproval_New` (`EXEC dbo.sp_Process_WeightedAverage_Non_TransInv_Rcv @DCStoreId = @NewDCStoreId;`, see that procedure's body above). No direct C#/.aspx.cs caller found.

```sql

-- =========================================================================
-- 1. sp_Process_WeightedAverage_Non_TransInv_Rcv (WAC Calculation)
-- =========================================================================
CREATE   PROCEDURE [dbo].[sp_Process_WeightedAverage_Non_TransInv_Rcv]
    @DCStoreId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT, @ProductId INT, @ProductCode NVARCHAR(50), @ProductName NVARCHAR(200);
    DECLARE @StockInQty DECIMAL(18,3), @UnitPrice DECIMAL(18,4), @TotalValue DECIMAL(18,4);
    DECLARE @ReceiveDate DATETIME, @ComUnitId INT, @ChalanNo NVARCHAR(100), @BatchNo NVARCHAR(100);
    DECLARE @MfgDate DATETIME, @ExpDate DATETIME, @InvReturnMasterID INT, @InvReturnDetailID INT;

    SELECT 
        @CompanyId = CU.CompanyId,
        @ProductId = PD.ProductId,
        @ProductCode = PD.ProductCode,
        @ProductName = PD.ProductName,
        @StockInQty = ISNULL(DCS.TotalQuantity, 0),
        @UnitPrice = ISNULL(DCS.UnitPrice_CostPrice, ISNULL(NTID.Price, 0)),
        @TotalValue = ISNULL(DCS.TotalQuantity, 0) * ISNULL(DCS.UnitPrice_CostPrice, ISNULL(NTID.Price, 0)),
        @ReceiveDate = ISNULL(DCS.ReceiveDateTime, DCS.ReceiveDate),
        @ComUnitId = DCS.ComUnitId,
        @ChalanNo = DCS.ChalanNo,
        @BatchNo = DCS.BatchNo,
        @MfgDate = DCS.MfgDate,
        @ExpDate = DCS.ExpDate,
        @InvReturnMasterID = NTM.InvReturnMasterID,
        @InvReturnDetailID = DCS.NonTranscationalInvReturnDetailID
    FROM dbo.tblDCStore AS DCS WITH (NOLOCK)
    INNER JOIN dbo.tblCompanyUnit AS CU WITH (NOLOCK) ON DCS.ComUnitId = CU.ComUnitId
    INNER JOIN dbo.tblProduct AS PD WITH (NOLOCK) ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))
    LEFT JOIN dbo.tblNonTranscationalInvoiceDetail AS NTID WITH (NOLOCK) ON NTID.InvReturnDetailID = DCS.NonTranscationalInvReturnDetailID
    LEFT JOIN dbo.tblNonTranscationalInvoiceMaster AS NTM WITH (NOLOCK) ON NTM.InvReturnMasterID = NTID.InvReturnMasterID
    WHERE DCS.DCStoreId = @DCStoreId;

    IF (@StockInQty > 0 AND @ProductId IS NOT NULL)
    BEGIN
        IF NOT EXISTS
        (
            SELECT 1 FROM dbo.tbl_INV_WeightedAvg WITH (NOLOCK)
            WHERE Trans_Type_Id = 1 AND Trans_Id = @DCStoreId AND Curr_Qty > 0
        )
        BEGIN
            DECLARE @Opening_Qty DECIMAL(18,3) = 0, @Opening_Amount DECIMAL(18,4) = 0, @Opening_Rate DECIMAL(18,4) = 0;
            DECLARE @Closing_Qty DECIMAL(18,3) = 0, @Closing_Amount DECIMAL(18,4) = 0, @Average_Rate DECIMAL(18,4) = 0;

            SELECT TOP 1 
                @Opening_Qty = ISNULL(Close_Qty, 0),
                @Opening_Amount = ISNULL(Close_Amt, 0)
            FROM dbo.tbl_INV_WeightedAvg WITH (NOLOCK)
            WHERE Company_Id = @CompanyId AND Item_Id = @ProductId
            ORDER BY Weighted_Avg_Id DESC;

            SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty ELSE 0 END;

            SET @Closing_Qty = @Opening_Qty + @StockInQty;
            SET @Closing_Amount = @Opening_Amount + @TotalValue;
            SET @Average_Rate = CASE WHEN @Closing_Qty > 0 THEN @Closing_Amount / @Closing_Qty ELSE 0 END;

            INSERT INTO dbo.tbl_INV_WeightedAvg
            (
                Company_Id, Warehouse_Id, Mst_Trans_Id, Trans_Id, Item_Id, Item_Code, Ref_No,
                Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type,
                OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt
            )
            VALUES
            (
                @CompanyId, @ComUnitId, @InvReturnMasterID, @DCStoreId, @ProductId, @ProductCode, @ChalanNo,
                GETDATE(), @ReceiveDate, @ReceiveDate, 1, 1, 'Finished Goods',
                @Opening_Qty, @Opening_Rate, @Opening_Amount, @StockInQty, @UnitPrice, @TotalValue,
                @Closing_Qty, @Average_Rate, @Closing_Amount
            );

            DECLARE @Weighted_Avg_Id_New INT = SCOPE_IDENTITY();

            IF (@Weighted_Avg_Id_New > 0)
            BEGIN
                IF NOT EXISTS
                (
                    SELECT 1 FROM dbo.tbl_INV_BatchWiseStock WITH (NOLOCK)
                    WHERE DCStoreId = @DCStoreId AND Batch_No = @BatchNo AND Trans_Qty = @StockInQty
                )
                BEGIN
                    INSERT INTO dbo.tbl_INV_BatchWiseStock
                    (
                        Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty, DCStoreId, Remarks
                    )
                    VALUES
                    (
                        @Weighted_Avg_Id_New, @BatchNo, @MfgDate, @ExpDate, @StockInQty, @DCStoreId, 'Non-transcational Invoice Return Receive'
                    );
                END;
            END;
        END;
    END;
END;
```
