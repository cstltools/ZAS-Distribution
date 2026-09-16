# Functions

Ground truth: extracted live from `SalesrollDB_ZAS` (`sys.sql_modules` / `OBJECT_DEFINITION`). This database has
**exactly 7 functions** — 3 scalar, 4 table-valued (one inline, three multi-statement) — no more, no fewer.
Application usage below was determined by grepping `Library.DAL`, `Library.BLL`, and `Solution.Web` (whole repo,
excluding `bin`/`obj`/precompiled output) for each function name.

The previous version of this document asserted "0 functions found" and treated their existence as entirely
UNKNOWN. That was correct as far as it went (no function is called *by name* in the C# source for 6 of the 7 —
see below) but is superseded now that the bodies are available directly from the database.

---

## GetAttendanceStatus

```sql
CREATE FUNCTION dbo.GetAttendanceStatus (@pInTime time(0))
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @AttStatus NVARCHAR(50) = '',
            @ShiftId INT = 0,
            @PresentInTime TIME(0),
            @PresentOutTime TIME(0),
            @LetInTime TIME(0),
            @LetOutTime TIME(0);

    SELECT
        @ShiftId = st.ShiftId,
        @PresentInTime = CONVERT(TIME, st.PresentInTime),
        @PresentOutTime = CONVERT(TIME, st.PresentOutTime),
        @LetInTime = CONVERT(TIME, st.LetInTime),
        @LetOutTime = CONVERT(TIME, st.LetOutTime)
    FROM
        tbl_Shift st
    WHERE
        CONVERT(TIME, @pInTime) BETWEEN ShiftInTime AND ShiftOutTime
        AND IsActive = 1;

    IF @pInTime BETWEEN @PresentInTime AND @PresentOutTime
        SET @AttStatus = 'Present';
    ELSE IF @pInTime BETWEEN @LetInTime AND @LetOutTime
        SET @AttStatus = 'Late';
    ELSE
        SET @AttStatus = 'Absent';

    RETURN @AttStatus;
END;
```

- **Type:** `SQL_SCALAR_FUNCTION`.
- **Parameters:** `@pInTime TIME(0)` — an employee's punch-in time.
- **Return type:** `NVARCHAR(50)` — one of `'Present'`, `'Late'`, `'Absent'`.
- **Depends on:** `tbl_Shift` (looks up the active shift whose `ShiftInTime`/`ShiftOutTime` window contains
  `@pInTime`, then classifies against that shift's `PresentInTime`/`PresentOutTime` and `LetInTime`/`LetOutTime`
  windows).
- **Application usage:** `UNUSED — no application caller found`.

---

## GetAttendanceStatusWithShiftId

```sql
CREATE FUNCTION [dbo].[GetAttendanceStatusWithShiftId] (@pInTime TIME(0))
RETURNS TABLE
AS
RETURN
(
    SELECT
        st.ShiftId,
        CASE
            WHEN @pInTime BETWEEN CONVERT(TIME, st.PresentInTime) AND CONVERT(TIME, st.PresentOutTime) THEN 'Present'
            WHEN @pInTime BETWEEN CONVERT(TIME, st.LetInTime) AND CONVERT(TIME, st.LetOutTime) THEN 'Late'
            ELSE 'Absent'
        END AS AttStatus
    FROM
        tbl_Shift st
    WHERE
        CONVERT(TIME, @pInTime) BETWEEN st.PresentInTime AND st.ShiftOutTime
        AND st.IsActive = 1
);
```

- **Type:** `SQL_INLINE_TABLE_VALUED_FUNCTION`.
- **Parameters:** `@pInTime TIME(0)`.
- **Return type:** `TABLE (ShiftId, AttStatus)` — a table-valued sibling of `GetAttendanceStatus`, returning one
  row per matching shift instead of a single scalar. Note the `WHERE` clause here brackets `@pInTime` between
  `st.PresentInTime` and `st.ShiftOutTime` — a different (and inconsistent) range than
  `GetAttendanceStatus`'s `ShiftInTime AND ShiftOutTime`; whether this divergence is intentional is unclear from
  the definition alone.
- **Depends on:** `tbl_Shift`.
- **Application usage:** `UNUSED — no application caller found`.

---

## GetDateWiseAttendence

```sql
CREATE FUNCTION [dbo].[GetDateWiseAttendence]
(
    @EmpCode NVARCHAR(MAX),
	@Year NVARCHAR(MAX)
)
RETURNS 
@MasterTable TABLE 
(
    RowNo int Identity(1,1) Primary Key not null,
		EmpMasterCode NVARCHAR(MAX) NULL,
		EmpName NVARCHAR(MAX) NULL,
		EmpInfoId INT NULL,
		AvailLeaveQty INT NULL,
		LeaveReason NVARCHAR(MAX) NULL,
		LeaveName NVARCHAR(MAX) NULL,
		Status NVARCHAR(MAX) NULL,
		ApprovedBy NVARCHAR(MAX) NULL,
		ApprovedDate DATETIME NULL,
		ActionStatus NVARCHAR(MAX) NULL,
		Date DATETIME NULL,
		DayName NVARCHAR(MAX) NULL,
		LeaveAvailId INT NULL,
		JobLength NVARCHAR(MAX) NULL
)
AS
BEGIN
	DECLARE	@EmpMasterCode NVARCHAR(MAX) 
	DECLARE	@EmpName NVARCHAR(MAX) 
	DECLARE	@EmpInfoId INT 
	DECLARE	@AvailLeaveQty INT 
	DECLARE	@LeaveReason NVARCHAR(MAX) 
	DECLARE	@LeaveName NVARCHAR(MAX) 
	DECLARE	@Status NVARCHAR(MAX) 
	DECLARE	@ApprovedBy NVARCHAR(MAX) 
	DECLARE	@ApprovedDate DATETIME 
	DECLARE	@ActionStatus NVARCHAR(MAX) 
	DECLARE	@Date DATETIME 
	DECLARE	@DayName NVARCHAR(MAX) 
	DECLARE	@JobLength NVARCHAR(MAX) 
	DECLARE @FromDate DATETIME
	DECLARE @ToDate DATETIME
	DECLARE @LeaveAvailId INT 

DECLARE @start DATETIME
DECLARE @end DATETIME
DECLARE @duration INT

SELECT  @end = GETDATE()
SELECT @start=JoiningDate FROM dbo.tblEmpGeneralInfo WHERE EmpMasterCode=@EmpCode
 SELECT @duration = DATEDIFF(mm, @start, @end)

DECLARE @Main CURSOR
SET @Main = CURSOR FAST_FORWARD
FOR
SELECT top 5 EmpMasterCode,EmpName,EmpInfoId,AvailLeaveQty,LeaveReason,LeaveName,Status,ApprovedBy,ApprovedDate,ActionStatus
 ,(CONVERT(NVARCHAR, 5 / 12) + ' Year ' + CONVERT(NVARCHAR, 5 % 12) +' Month'),FromDate,ToDate,LeaveAvailId FROM dbo.tblLeaveAvail WHERE   YEAR(FromDate)=@Year AND ActionStatus='Accepted' ORDER BY FromDate
OPEN @Main
FETCH NEXT FROM @Main
INTO @EmpMasterCode,@EmpName,@EmpInfoId,@AvailLeaveQty,@LeaveReason,@LeaveName,@Status,@ApprovedBy,@ApprovedDate,@ActionStatus,@JobLength,@FromDate,@ToDate,@LeaveAvailId 
WHILE @@FETCH_STATUS=0
BEGIN
INSERT INTO @MasterTable
        ( EmpInfoId ,
          AvailLeaveQty ,
          LeaveReason ,
          EmpMasterCode ,
          EmpName ,
          LeaveName ,
          ActionStatus ,
          Date ,
          DayName ,
          Status ,
          ApprovedBy ,
          ApprovedDate,LeaveAvailId,JobLength
        )
SELECT top 5 @EmpInfoId,1,@LeaveReason,@EmpMasterCode,@EmpName
,@LeaveName,@ActionStatus,ATTDate,DayName,@Status,@ApprovedBy,@ApprovedDate,@LeaveAvailId,@JobLength
 FROM dbo.tblAttendanceRecord --WHERE (ATTDate BETWEEN @FromDate AND @ToDate) AND EmpId=@EmpInfoId

FETCH NEXT FROM @Main
INTO @EmpMasterCode,@EmpName,@EmpInfoId,@AvailLeaveQty,@LeaveReason,@LeaveName,@Status,@ApprovedBy,@ApprovedDate,@ActionStatus,@JobLength,@FromDate,@ToDate,@LeaveAvailId
END
CLOSE @Main
DEALLOCATE @Main

RETURN
END
```

- **Type:** `SQL_TABLE_VALUED_FUNCTION` (multi-statement).
- **Parameters:** `@EmpCode NVARCHAR(MAX)`, `@Year NVARCHAR(MAX)`.
- **Return type:** `@MasterTable TABLE (RowNo, EmpMasterCode, EmpName, EmpInfoId, AvailLeaveQty, LeaveReason,
  LeaveName, Status, ApprovedBy, ApprovedDate, ActionStatus, Date, DayName, LeaveAvailId, JobLength)`.
- **Depends on:** `tblEmpGeneralInfo` (to compute tenure via `JoiningDate`), `tblLeaveAvail` (cursor source,
  filtered to accepted leaves in `@Year`), `tblAttendanceRecord` (inner insert source).
- **Notes on the body:** this function appears broken/incomplete as written — the inner `SELECT TOP 5 ... FROM
  dbo.tblAttendanceRecord` has its `WHERE (ATTDate BETWEEN @FromDate AND @ToDate) AND EmpId=@EmpInfoId` clause
  **commented out**, so it pulls the same unfiltered top-5 attendance rows for every iteration of the cursor
  regardless of `@EmpCode`/`@Year`/`EmpInfoId`; `@duration` and `@AvailLeaveQty` are computed but never actually
  used in the inserted row (the insert hardcodes `1` for `AvailLeaveQty`); `JobLength` is computed from literal
  `5/12`/`5%12`, not from `@duration`. This looks like unfinished/dead work-in-progress logic.
- **Application usage:** `UNUSED — no application caller found`.

---

## GetStockValue

```sql
CREATE FUNCTION [dbo].[GetStockValue]  
(  
       @CompanyId int,  
       @FromDate datetime,  
	   @ToDate datetime
)  
RETURNS int  
AS  
BEGIN  
       -- Declare the return variable here  
       DECLARE @Result decimal(18,2)=0  
       SELECT @Result = SUM(CAST(CASE WHEN (ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+
	ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0)) > 
	(ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)) 
	THEN ((ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ 
	ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0))
	-(ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)))
	 ELSE ((ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)) 
	 - (ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0)
	  + ISNULL(vTblsalesInTransit.Value,0))) END AS BIGINT)) + CAST(W.StockValue AS bigint)  
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
GROUP BY P.CompanyId,W.StockValue;  
       -- Return the result of the function  
       RETURN @Result  
END  
```

- **Type:** `SQL_SCALAR_FUNCTION`.
- **Parameters:** `@CompanyId INT`, `@FromDate DATETIME`, `@ToDate DATETIME`.
- **Return type:** declared `RETURNS int`, but the internal accumulator `@Result` is `DECIMAL(18,2)` and every
  branch of the `CASE` is explicitly `CAST(... AS BIGINT)` before being summed and added to `W.StockValue` — the
  final implicit conversion back to `int` on `RETURN @Result` can silently truncate/overflow for large values;
  this is a real risk given the size of the sums being computed, and is worth flagging if this function is ever
  revived.
- **Depends on:** a large fan-out of tables — `tblProduct`, `tblDCStore_OpeningBalance`, `tblCentralStore`,
  `tblDCStore`, `tblInvoiceDetail`, `tblInvoice`, `tblChalanDetail`, `tblChalanInfo`, `tblDCStoreFreeze`,
  `tblSampleIssue`, `tblSampleIssueDetail`, `tblSampleIssueTranscation`, `tblStockInTransfar`, `tblRequisition`,
  `tblCentralStore_OpeninigBalance`, `tblUnitPrice`, `tblCompanyInfo`, `tblWhStoreFreeze`.
- **What it computes:** net stock-value movement for a company between `@FromDate` and `@ToDate` — outbound
  (sales, product bonus, challan delivered/in-transit, samples issued, sales-in-transit) versus inbound (opening
  balance, central-store receive, challan received/in-transit), each side valued at `UnitPrice + VATPerUnit`,
  netted and added to a separately-computed company-wide `StockValue` (`W`) built from opening/receive/Dhaka-
  transfer/transit/freeze deltas.
- **Application usage:** `UNUSED — no application caller found`.

---

## MainMenu2

```sql
create FUNCTION [dbo].[MainMenu2]
(
    @UserId INT,
    @UserRoleID INT
)
RETURNS NVARCHAR(MAX) -- or whatever length you need
AS
BEGIN
    Declare @MenuHTML NVARCHAR(Max)
    SET @MenuHTML='<ul class="metismenu" id="menu" >'

    DECLARE @MainCount INT
	DECLARE @MainStartIndex INT
	DECLARE @MainMenuName NVARCHAR(MAX)
	DECLARE @MainURL NVARCHAR(MAX)
	DECLARE @ParantId INT
	DECLARE @SL nvarchar(max)
	DECLARE @Class NVARCHAR(MAX)
	DECLARE @Icon NVARCHAR(MAX)

	SET @MainStartIndex=1
	SELECT @MainCount=COUNT(*) FROM dbo.tblMainMenuNew where ParantId is null or ParantId=''

	DECLARE @Main CURSOR
	IF(@UserId='1')
	BEGIN
    SET @Main = CURSOR FAST_FORWARD
    FOR
	SELECT ROW,ManuName,URL,Class,Icon,ParantId,SL FROM (select (ROW_NUMBER() OVER (ORDER BY dbo.tblMainMenuNew.SL)) AS ROW,ISNULL(ManuName,'') AS ManuName,   ISNULL(URL,'')   AS URL,ISNULL(Class,'') AS Class,ISNULL(Icon,'') AS Icon,ISNULL(ParantId,'') AS ParantId,SL FROM dbo.tblMainMenuNew where TypeId=1 ) AS TblTemp where ParantId is null or ParantId='' order by SL ASC
	END
	ELSE
	BEGIN
    SET @Main = CURSOR FAST_FORWARD
    FOR
	select '1',ManuName,URL,Class,Icon,ParantId,dbo.tblMainMenuNew.SL from tblMainMenuNew 
                                     WHERE   SL in (select ParantId from tblMenuRole
									 left join tblMainMenuNew on tblMenuRole.SL=tblMainMenuNew.SL
									  where RoleId=@UserRoleID  and TypeId=1) order by tblMainMenuNew.SL asc
	END
	OPEN @Main
    FETCH NEXT FROM @Main
    INTO @MainStartIndex,@MainMenuName,@MainURL,@Class,@Icon,@ParantId,@SL
    WHILE @@FETCH_STATUS=0
    BEGIN
		SET @MenuHTML=@MenuHTML+
		'<li>'+
			'<a href="javascript:;" class="has-arrow">
			'+'<div class="parent-icon"><i class="'+@Icon+'"></i></div>'+
				'<div class="menu-title">'+@MainMenuName+'</div></a>'

				DECLARE @SubCount INT=0
				DECLARE @SubStartIndex INT
				DECLARE @SubMenuName NVARCHAR(MAX)
				DECLARE @SubURL NVARCHAR(MAX)
				DECLARE @SubParantId INT
				DECLARE @SubSL INT
				DECLARE @SubClass NVARCHAR(MAX)
				DECLARE @SubIcon NVARCHAR(MAX)

				IF(@UserId<>'1')
				BEGIN
				SELECT @SubCount=COUNT(*) FROM 				
				(select (ROW_NUMBER() OVER (ORDER BY dbo.tblMainMenuNew.SL)) AS ROW,ISNULL(ManuName,'') AS ManuName,ISNULL(URL,'') AS URL,ISNULL(Class,'') AS Class,ISNULL(Icon,'') AS Icon,ISNULL(ParantId,'') AS ParantId,tblMainMenuNew.SL FROM dbo.tblMainMenuNew
			INNER JOIN dbo.tblMenuRole ON dbo.tblMainMenuNew.SL = dbo.tblMenuRole.SL 
                                     WHERE tblMenuRole.RoleId=@UserRoleID  AND ParantId=@SL ) AS TblTempa 
				END
				ELSE
				BEGIN
					select @SubCount=COUNT(*) from tblMainMenuNew where ParantId=@SL
				END

				IF(@SubCount>0)
				BEGIN
					SET @MenuHTML=@MenuHTML+'<ul>'	
				END

				-- ... (nested identically for Sub, SubSub, SubSubSub levels, 4 levels of cursor/menu nesting total) ...

				FETCH NEXT FROM @Main
				INTO @MainStartIndex,@MainMenuName,@MainURL,@Class,@Icon,@ParantId,@SL
	END
    CLOSE @Main
    DEALLOCATE @Main

	SET @MenuHTML=@MenuHTML+'</ul>'    
    RETURN  @MenuHTML
END
```

(Full 418-line body preserved in `...\scratchpad\extracted\MainMenu2.sql`; the `-- ... nested identically ...`
elision above collapses three more structurally-identical nesting levels for readability — see that file for the
byte-for-byte original, which is what ships in the database.)

- **Type:** `SQL_SCALAR_FUNCTION`.
- **Parameters:** `@UserId INT`, `@UserRoleID INT`.
- **Return type:** `NVARCHAR(MAX)` — a complete HTML `<ul class="metismenu">` sidebar-menu markup string, up to
  4 levels deep (Main → Sub → SubSub → SubSubSub), built with nested `CURSOR`s.
- **Depends on:** `tblMainMenuNew` (menu item master, self-referencing via `ParantId`), `tblMenuRole`
  (role-based menu visibility for the top 3 levels), `tblMenuDistribution` (user-specific menu visibility for the
  deepest, 4th level).
- **Behavior branch:** when `@UserId = 1` (a hardcoded admin/superuser id), every menu item is included
  unfiltered; for any other user, visibility is filtered by `tblMenuRole`/`tblMenuDistribution` against
  `@UserRoleID`/`@UserId`.
- **Application usage:** `UNUSED — no application caller found`. Given the name and the hardcoded `@UserId='1'`
  admin bypass, this looks like a function-based counterpart to a stored-procedure-driven menu build elsewhere in
  the app (the "2" suffix suggests a second iteration/experiment) — it was not wired into any DAL/BLL call site
  found in this repo.

---

## SplitString

```sql
CREATE FUNCTION dbo.SplitString
(
    @List NVARCHAR(MAX),
    @Delimiter CHAR(1)
)
RETURNS @Result TABLE (Value INT)
AS
BEGIN
    DECLARE @Value NVARCHAR(MAX), @Pos INT;

    WHILE LEN(@List) > 0
    BEGIN
        SET @Pos = CHARINDEX(@Delimiter, @List);
        IF @Pos = 0
        BEGIN
            SET @Value = @List;
            SET @List = '';
        END
        ELSE
        BEGIN
            SET @Value = LEFT(@List, @Pos - 1);
            SET @List = RIGHT(@List, LEN(@List) - @Pos);
        END

        IF ISNUMERIC(@Value) = 1
            INSERT INTO @Result (Value) VALUES (CAST(@Value AS INT));
    END

    RETURN;
END
```

- **Type:** `SQL_TABLE_VALUED_FUNCTION` (multi-statement).
- **Parameters:** `@List NVARCHAR(MAX)` (delimited list), `@Delimiter CHAR(1)`.
- **Return type:** `@Result TABLE (Value INT)`.
- **Depends on:** no tables — pure string-splitting logic; loops through `@List`, splitting on `@Delimiter`, and
  inserts each numeric token cast to `INT` (non-numeric tokens are silently dropped via the `ISNUMERIC` guard).
- **Application usage:** **Used** — `BatchWiseCollectionReportDal.cs`
  (`Library.DAL\SInventory_DAL\BatchWiseCollectionReportDal.cs:291`):
  `CROSS APPLY dbo.SplitString(MR.CustPayDetailsId, ',') AS Split` — explodes a comma-separated list of
  `CustPayDetailsId`s stored on `tblMoneyReceipt` back into individual ids, then joins each to
  `tblCustPayDetail`/`tblInvoice` to build the batch-wise collection report.

---

## fn_split_string_to_column

```sql
CREATE FUNCTION [dbo].[fn_split_string_to_column] (
    @string NVARCHAR(MAX),
    @delimiter CHAR(1)
    )
RETURNS @out_put TABLE (
    [column_id] INT IDENTITY(1, 1) NOT NULL,
    [value] NVARCHAR(MAX)
    )
AS
BEGIN
    DECLARE @value NVARCHAR(MAX),
        @pos INT = 0,
        @len INT = 0

    SET @string = CASE 
            WHEN RIGHT(@string, 1) != @delimiter
                THEN @string + @delimiter
            ELSE @string
            END

    WHILE CHARINDEX(@delimiter, @string, @pos + 1) > 0
    BEGIN
        SET @len = CHARINDEX(@delimiter, @string, @pos + 1) - @pos
        SET @value = SUBSTRING(@string, @pos, @len)

        INSERT INTO @out_put ([value])
        SELECT LTRIM(RTRIM(@value)) AS [column]

        SET @pos = CHARINDEX(@delimiter, @string, @pos + @len) + 1
    END

    RETURN
END
```

- **Type:** `SQL_TABLE_VALUED_FUNCTION` (multi-statement).
- **Parameters:** `@string NVARCHAR(MAX)`, `@delimiter CHAR(1)`.
- **Return type:** `@out_put TABLE (column_id INT IDENTITY, value NVARCHAR(MAX))`.
- **Depends on:** no tables — a second, independent delimiter-splitter that (unlike `SplitString`) returns
  trimmed **string** values with a generated identity column, rather than casting to `INT`. Functionally
  redundant with `SplitString` for the common case of splitting a numeric id list, but usable for non-numeric
  delimited data that `SplitString` would silently discard.
- **Application usage:** `UNUSED — no application caller found`.

---

## Summary

7/7 functions documented with full verbatim bodies. Application usage:

| Function | Type | Usage |
|---|---|---|
| `SplitString` | Table-valued | Used — `BatchWiseCollectionReportDal.cs` (`CROSS APPLY`) |
| `GetAttendanceStatus` | Scalar | UNUSED |
| `GetAttendanceStatusWithShiftId` | Inline table-valued | UNUSED |
| `GetDateWiseAttendence` | Table-valued | UNUSED (body also appears incomplete/broken — see notes) |
| `GetStockValue` | Scalar | UNUSED |
| `MainMenu2` | Scalar | UNUSED |
| `fn_split_string_to_column` | Table-valued | UNUSED |

Only 1 of the 7 functions has a live caller anywhere in `Library.DAL`/`Library.BLL`/`Solution.Web`. The other 6
are either dead/experimental work (`GetDateWiseAttendence`, `MainMenu2`), redundant with something else in the
app (`fn_split_string_to_column` vs. the used `SplitString`), or plausibly consumed by something outside this
repository (`GetAttendanceStatus`/`GetAttendanceStatusWithShiftId` — HR/attendance logic that may be called from
the separate `ZASHRIS` system; `GetStockValue` — may be intended for a report or dashboard not yet wired up, or
superseded by equivalent logic duplicated inline in DAL query strings).
