# Stored Procedures — Part 4 (Batch 4 of 4)

Reverse-engineered documentation for 124 stored procedures extracted verbatim from the live `SalesrollDB_ZAS` database. This batch is dominated by the `sp_Webapi_*` / `sp_webapi_*` family — a mobile field-force ("MIO"/medical rep) API surface (attendance, DCR/doctor-call-report, tour plan, orders, prescriptions) plus a handful of `sp_check_*` validation procs and two `tbl_*`-named inventory procs.

**Caller methodology**: literal, case-insensitive search of each exact procedure name across `Library.DAL`, `Library.BLL`, and `Solution.Web` (`*.cs`, `*.aspx`, excluding `bin`/`obj`). Only `tbl_INV_CentralStoreStoIssue` (called from `RequisitionDAL.cs`) and `tbl_MC_MaterialConversionCentralStoreRcv` (search separately, see below) had matches. Every other procedure in this batch is invoked, if at all, only by an external mobile/WebAPI backend project that is not part of this repository — so it is marked `UNUSED — no application caller found` with respect to this codebase.

---

## sp_Webapi_GetProductWiseOrderReport_N

- **Parameters**: `@empId INT = NULL`, `@Role NVARCHAR(MAX) = NULL`, `@fromdate NVARCHAR(MAX) = NULL`, `@todate NVARCHAR(MAX) = NULL`
- **Tables read**: `tblEmpGeneralInfo`, `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`, `tblMIOInfo`, `tblASMInfo`, `tblRSMInfo`, `tblTagASMRSMInfo`, `tblProduct`, `tblOrderDetail`, `tblOrder`
- **Tables written**: none
- **Business logic**: Resolves the caller's role (MIO/AM/DZSM) and manufacturer/territory scope for `@empId`, builds a dynamic-SQL filter string, and returns a product-wise order count/quantity/value report from `tblOrder`/`tblOrderDetail` filtered to app-originated orders (`IsFromApp = 1`), optionally by date range and by "Team" territory scope.
- **Transaction/error handling**: none (no explicit transaction, no TRY/CATCH)
- **Result set**: one dynamic result set — per product: `SN`, `order_times`, `product_name`, `ordered_qty`, `ordered_value`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_GetProductWiseOrderReport_N] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@Role  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

AS
BEGIN

 

 
 DECLARE @params NVARCHAR(max)='  '

			IF(@fromdate IS NOT NULL and @todate IS NOT NULL )
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  D.SubmissionDate))   between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
	 
		 

	  
	  
	  
	  DECLARE @mioCode NVARCHAR(50)

        SELECT  @mioCode = usr.LoginName
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 


		 Declare @Roltp nvarchar(max)=''
		   declare @companyId NVARCHAR(50)=0 
	  
		  select @Roltp=RoleType   FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId
		if(@Roltp='MIO')
		begin 
		select @companyId=B.CompanyId from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@empId and IsActive=1

		  

		end

			if(@Roltp='AM')
		begin 
		 

		select @companyId=B.CompanyId from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@empId and IsActive=1

	 


		end
 	if(@Roltp='DZSM')

	begin 
	 
			select @companyId=B.CompanyId  from tblRSMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@empId and IsActive=1

		end

		SET @params=@params+ ' AND D.ManufacId ='+ @companyId +'';




		declare @TRID nvarchar(max)
			  if(@Role = 'Team')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ @TRID +')';

			END

			  if(@Roltp = 'AM')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ @TRID +')';

			END


			 if(@Roltp = 'MIO')
			BEGIN
				 

				SET @params=@params+ ' and D.EntryBy='''+convert(nvarchar(max),@mioCode)+''''

			END


DECLARE @Q NVARCHAR(MAX)
	SET @Q='select  ROW_NUMBER() Over (Order by pro.ProductCode) As [SN], order_times,  pro.ProductCode+'' : ''+pro.ProductName product_name,   (tblDtl.Quantity) ordered_qty, ordered_value from tblProduct pro
inner join (select ISNULL(count(D.OrderId),0) order_times, convert(decimal(18,2),iSNULL(SUM(dtl.TotalTradePrice),0))	 as ordered_value,  SUM(dtl.Quantity) Quantity, dtl.ProductId from tblOrderDetail dtl
inner join  dbo.tblOrder D  with (nolock) on dtl.OrderId=D.OrderId
left join tblUser u with (nolock) on D.EntryBy=u.LoginName

left join tblEmpGeneralInfo emp with (nolock) on emp.EmpInfoId=u.EmpInfoId  WHERE  IsFromApp = 1 '+@params+'
 group by  ProductId) tblDtl on tblDtl.ProductId=pro.ProductId    
 
		'

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END

```

## sp_Webapi_GetProductWiseSalesReport

- **Parameters**: `@empId INT = NULL`, `@Role NVARCHAR(MAX) = NULL`, `@Month INT = NULL`, `@Year INT = NULL`
- **Tables read**: `tblEmpGeneralInfo`, `tblUser`, `tblMIOInfo`, `tblTagASMRSMInfo`, `tblInvoice`, `tblOrder`, `tblInvoiceDetail`, `tblCustMaster`, `tblTerritory`, `tblArea`, `tblCompanyUnit`, `tblCompanyInfo`
- **Tables written**: none
- **Business logic**: Same role/scope-resolution pattern as above, but reports actual invoiced sales (product-wise order count, delivered quantity, delivered net value) from `tblInvoice`/`tblInvoiceDetail` for invoices with delivery status `Full` or `Partial`, filterable by month/year and MIO/territory scope.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — per product: `SN`, `order_times`, `product_name`, `ordered_qty`, `ordered_value`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_GetProductWiseSalesReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@Role  nvarchar(max)=null,
 
@Month  int=null,
@Year   int=null

AS
BEGIN

 

 

		 
		DECLARE @params NVARCHAR(max)='  '

		IF(@Month IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND MONTH(convert(Date,  INV.InvoiceDate))='''+CAST(@Month AS NVARCHAR(max))+''''
		END
		IF(@Year IS NOT NULL )
		BEGIN
		    SET @params=@params+ ' AND   year(convert(Date,  INV.InvoiceDate))= '''+CAST(@Year AS NVARCHAR(max))+''''
		END

	  
	  
	  
	  DECLARE @mioCode NVARCHAR(50)
	  DECLARE @mioId NVARCHAR(50)
	   declare @companyId NVARCHAR(50)='' 
        SELECT  @mioCode = usr.LoginName ,@companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

		  SELECT  @mioId = a.MIOId
        FROM    dbo.tblMIOInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmployeeId=usr.EmpInfoId
        WHERE   A.EmployeeId = @empId 

		if(@companyId<>'')
		begin
			SET @params=@params+ ' AND D.ManufacId ='+ @companyId +'';
			end

		declare @TRID nvarchar(max)
			  if(@Role = 'Team')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ @TRID +')';

			END
			else
			BEGIN
				 

			SET @params=@params+ ' and INV.MiaId='''+convert(nvarchar(max),@mioId)+''''

			END






DECLARE @Q NVARCHAR(MAX)
	SET @Q='SELECT   ROW_NUMBER() Over (Order by INVD.ProductCode) As [SN],ISNULL(count(D.OrderId),0) order_times,  INVD.ProductCode+'' : ''+INVD.ProductName product_name,   ISNULL(SUM(DeliveryTotalQuantity),0)  ordered_qty, SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) AS ordered_value   FROM dbo.tblInvoice AS INV
							 	 inner join  dbo.tblOrder D  with (nolock) on INV.OrderId=D.OrderId
							  LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
							  
							  LEFT JOIN dbo.tblCustMaster AS CSTMR ON  INV.CustomerMasterId = CSTMR.CustomerMasterId
							   LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=INV.MiaId 
							   LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId 
							   LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
							   LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
							    LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId WHERE DeliveryInvoiceStatus IN (''Full'',''Partial'') 
   '   +@params+'
 GROUP BY   INVD.ProductCode,INVD.ProductName HAVING SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) > 0  
 
		'
		--AND    MONTH(INV.InvoiceDate) =5 AND    year(INV.InvoiceDate) =2023 AND INV.MiaId = '649' 

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END

```

## sp_Webapi_GetProductWiseSalesReport_N

- **Parameters**: `@empId INT = NULL`, `@Role NVARCHAR(MAX) = NULL`, `@fromdate NVARCHAR(MAX) = NULL`, `@todate NVARCHAR(MAX) = NULL`
- **Tables read**: same as `sp_Webapi_GetProductWiseSalesReport` plus `tblMIATargetProductWise`
- **Tables written**: none
- **Business logic**: Extended version of `sp_Webapi_GetProductWiseSalesReport` that also joins the MIO's product-wise monthly target (`tblMIATargetProductWise`) for the `@fromdate`'s year/month and computes an `Achivment` percentage (`delivered qty * 100 / TargetQty`) alongside the sales figures, using a from/to date range instead of month/year.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — per product: `SN`, `order_times`, `product_name`, `ordered_qty`, `ordered_value`, `TargetQty`, `Achivment`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_GetProductWiseSalesReport_N] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@Role  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

AS
BEGIN

 

 

		 
		DECLARE @params NVARCHAR(max)='  '

		IF(@fromdate IS NOT NULL and @todate IS NOT NULL )
		BEGIN
		    SET @params=@params+ '  and   (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
	 
	  
	  
	  
	  DECLARE @mioCode NVARCHAR(50)
	  DECLARE @mioId NVARCHAR(50)

        SELECT  @mioCode = usr.LoginName
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

		  SELECT  @mioId = a.MIOId
        FROM    dbo.tblMIOInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmployeeId=usr.EmpInfoId
        WHERE   A.EmployeeId = @empId  and A.IsActive=1


		 Declare @Roltp nvarchar(max)=''
		   declare @companyId NVARCHAR(50)=0 
	  
		  select @Roltp=RoleType   FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId
		if(@Roltp='MIO')
		begin 
		select @companyId=B.CompanyId from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@empId and IsActive=1

		  

		end

			if(@Roltp='AM')
		begin 
		 

		select @companyId=B.CompanyId from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@empId and IsActive=1

	 


		end
 	if(@Roltp='DZSM')

	begin 
	 
			select @companyId=B.CompanyId  from tblRSMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@empId and IsActive=1

		end

		SET @params=@params+ ' AND D.ManufacId ='+ @companyId +'';




		declare @TRID nvarchar(max)

			 if(isnull(@Role,'') = '')
			BEGIN
				SET @params=@params+ ' and INV.MiaId='''+convert(nvarchar(max),@mioId)+''''
			end

			 else if(@Role = 'Team')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ @TRID +')';

			END
			else  if(@Roltp = 'AM')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ @TRID +')';

			END

			else if(@Roltp = 'MIO')
			BEGIN
				 

			SET @params=@params+ ' and INV.MiaId='''+convert(nvarchar(max),@mioId)+''''

			END






DECLARE @Q NVARCHAR(MAX)
	SET @Q='SELECT   ROW_NUMBER() Over (Order by INVD.ProductCode) As [SN],ISNULL(count(D.OrderId),0) order_times,  INVD.ProductCode+'' : ''+INVD.ProductName product_name,   ISNULL(SUM(DeliveryTotalQuantity),0)  ordered_qty, SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) AS ordered_value , ISNULL(TG.TargetQty,0) TargetQty, isnull(convert(int,  ROUND(CASE WHEN TG.TargetQty IS NOT NULL THEN (ISNULL(SUM(DeliveryTotalQuantity),0) * 100) / NULLIF(TG.TargetQty,0) ELSE 0 END ,0)),0) AS Achivment  FROM dbo.tblInvoice AS INV
							 	 inner join  dbo.tblOrder D  with (nolock) on INV.OrderId=D.OrderId
							  LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
							  
							  LEFT JOIN dbo.tblCustMaster AS CSTMR ON  INV.CustomerMasterId = CSTMR.CustomerMasterId
							   LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=INV.MiaId 
							   LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId 
							   LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
							   LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
							    LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId 
								LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
								 LEFT JOIN (SELECT TGRT.MiaId,TGRT.TargetQty,PD.ProductCode FROM tblMIATargetProductWise AS TGRT 
						  LEFT JOIN tblProduct AS PD ON TGRT.ProductId = PD.ProductId 
						  WHERE TGRT.Year = DATENAME(year,'''+CAST(@fromdate AS NVARCHAR(max))+''' ) AND (TGRT.Period = DATENAME(month,'''+CAST(@fromdate AS NVARCHAR(max))+'''))) AS TG 
						  ON tblMIOInfo.MIOId = TG.MiaId AND INVD.ProductCode = TG.ProductCode 
								WHERE DeliveryInvoiceStatus IN (''Full'',''Partial'') 
   '   +@params+'
 GROUP BY    TG.TargetQty,INVD.ProductCode,INVD.ProductName HAVING SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) > 0  
 
		'
		--AND    MONTH(INV.InvoiceDate) =5 AND    year(INV.InvoiceDate) =2023 AND INV.MiaId = '649' 

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END

```

## sp_Webapi_GetTeamList

- **Parameters**: `@role NVARCHAR(50) = NULL`, `@id INT = NULL`, `@aparam NVARCHAR(MAX)` (no default; treated as raw SQL fragment appended to the dynamic query)
- **Tables read**: `tblASMInfo`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblTerritory`, `tblArea`, `View_Webapi_EmployeeFieldForceInfo` (referenced via `@aparam`/`@param` text, not directly joined in the base query)
- **Tables written**: none
- **Business logic**: Returns the list of field-force employees reporting under a given manager, scoped by role (`AM` scopes by company+territory tag list, `RSM`/`DZSM` by `RSMEMPId`, `NSM` by `NSMEMPId`), via fully dynamic SQL built from string concatenation (including the caller-supplied `@aparam` appended verbatim — a SQL-injection-prone pattern).
- **Transaction/error handling**: none
- **Result set**: one dynamic result set of employee/territory columns (`EmpRole`, `TerritoryName`, `TerritoryCode`, `AreaCode`, `AreaName`, `EmpMasterCode`, `EmpName`, `ShiftId`, `EmpInfoId`, `Gender`, `AddressPresent`, `CellNumber`, `Email`, `EmrgContactNo`)
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE
 PROCEDURE [dbo].[sp_Webapi_GetTeamList]
	-- Add the parameters for the stored procedure here
    @role NVARCHAR(50) = NULL ,
    @id INT = NULL,
	@aparam NVARCHAR(MAX) NULL
AS
    BEGIN
		 DECLARE @TRID NVARCHAR(MAX)
	DECLARE @param NVARCHAR(MAX)=''
	IF(@role='AM')
	BEGIN
	declare @comId int
	select @comId=CompanyId from tblASMInfo where EmployeeId=@id and IsActive=1

	select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@id) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')
	    SET @Param=@Param+' and m.TerritoryId in   ('+@TRID+')  and m.CompanyId='+CAST(@comId as nvarchar(max))
	END
	IF(@role='RSM')
	BEGIN
	    SET @param=@param+' Where RSMEMPId='+CONVERT(NVARCHAR(MAX),@id)+' AND View_Webapi_EmployeeFieldForceInfo.EmpInfoId<>'+CONVERT(NVARCHAR(MAX),@id)
	END
	IF(@role='DZSM')
	BEGIN
	    SET @param=@param+' Where RSMEMPId='+CONVERT(NVARCHAR(MAX),@id)+' AND View_Webapi_EmployeeFieldForceInfo.EmpInfoId<>'+CONVERT(NVARCHAR(MAX),@id)
	END
	IF(@role='NSM')
	BEGIN
	    SET @param=@param+' Where NSMEMPId='+CONVERT(NVARCHAR(MAX),@id)+' AND View_Webapi_EmployeeFieldForceInfo.EmpInfoId<>'+CONVERT(NVARCHAR(MAX),@id)
	END


	DECLARE @q NVARCHAR(MAX)=''

	SET @q='
	
   SELECT distinct e.EmpName EmpRole,
           TerritoryName,
           TerritoryCode,
           AreaCode,
           AreaName,
          '''' RegionCode,
         ''''  RegionName,      
          '''' GroupName,
           e.EmpMasterCode,
            e.EmpName,
            e.ShiftId,
           
           e.EmpInfoId,
            e.Gender,
           e. AddressPresent,
 
           e. CellNumber,
            e.Email,
           
           
 
            e.EmrgContactNo
           FROM dbo.tblMIOInfo m
	inner JOIN dbo.tblEmpGeneralInfo e ON m.EmployeeId = e.EmpInfoId
	inner JOIN dbo.tblTerritory  tr ON m.TerritoryId = tr.TerritoryId
	inner JOIN dbo.tblArea  ar ON ar.AreaId = tr.AreaId
	--inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=e.EmpMasterCode 
	  where   m.IsActive=1 and tr.IsActive=1 

	   --empHR.EmployeeStatus=''Active''
	'+@param + ' ' +@aparam
	

	EXEC sp_executesql @q

    END



```

## sp_Webapi_GetTerritoryByEmpId

- **Parameters**: `@empId INT = NULL`
- **Tables read**: `tblTerritory`, `tblMIOInfo`
- **Tables written**: none
- **Business logic**: Returns the active territories assigned to the given employee (via `tblMIOInfo`).
- **Transaction/error handling**: none
- **Result set**: `TerritoryId`, `TerritoryName`, `TerritoryCode`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetTerritoryByEmpId]
	-- Add the parameters for the stored procedure here
@empId INT = NULL
AS
BEGIN
		
		SELECT TerritoryId ,
               TerritoryName ,
               TerritoryCode 
			   FROM dbo.tblTerritory WHERE IsActive = 1 AND TerritoryId IN (
		SELECT TerritoryId FROM dbo.tblMIOInfo WHERE EmployeeId = @empId AND IsActive = 1
		)
		 

END
```

## sp_Webapi_Get_AMMultipleArea

- **Parameters**: `@AreId NVARCHAR(MAX) = NULL`
- **Tables read**: `tblASMInfo`
- **Tables written**: none
- **Business logic**: Given an area id, looks up the Area Manager (ASM) assigned to it, then returns all other active areas assigned to that same ASM (i.e. the ASM's full multi-area coverage list).
- **Transaction/error handling**: none
- **Result set**: `AreaId` (list)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_AMMultipleArea]
	-- Add the parameters for the stored procedure here
	@AreId NVARCHAR(MAX)= NULL
	AS
    BEGIN


	declare    @PersonalEmpId nvarchar(max)
	select @PersonalEmpId=EmployeeId from tblASMInfo where areaid=@AreId
	select     AreaId from tblASMInfo where EmployeeId=@PersonalEmpId  and IsActive=1 order by ASMId asc
	 

	end
```

## sp_Webapi_Get_ASM_FS

- **Parameters**: `@AreId NVARCHAR(MAX) = NULL`
- **Tables read**: `tblASMInfo`, `tblTagASMRSMInfo`
- **Tables written**: none
- **Business logic**: Resolves the ASM employee assigned to `@AreId`, then returns that ASM's tagged territory-area ids from `tblTagASMRSMInfo` (field-force scope lookup).
- **Transaction/error handling**: none
- **Result set**: `TerritoryAreaId` (list)
- **Caller**: UNUSED — no application caller found

```sql
create PROCEDURE [dbo].[sp_Webapi_Get_ASM_FS]
	-- Add the parameters for the stored procedure here
	@AreId NVARCHAR(MAX)= NULL
	AS
    BEGIN

	 declare @EmpId int=0
	select   @EmpId=  EmployeeId from tblASMInfo where AreaId=@AreId  and IsActive=1 order by ASMId asc


select  [TerritoryAreaId] from	 [dbo].[tblTagASMRSMInfo] where  [EmpInfoId]= @EmpId

	end
```

## sp_Webapi_Get_AttendanceInfo

- **Parameters**: `@empId INT`
- **Tables read**: `dbo.GetAttendanceStatusWithShiftId(@pInTime)` (table-valued function), `tblMarketAttendance_Master_webapi`
- **Tables written**: none
- **Business logic**: Determines the current shift from the current time via the `GetAttendanceStatusWithShiftId` function, then returns whether the "Punch In" / "Punch Out" buttons should be enabled (`ON`/`OFF`) for the employee's mobile attendance screen today, based on whether punch-in/out already recorded for that shift; if no shift is active both buttons are forced `OFF`.
- **Transaction/error handling**: none
- **Result set**: single row — `PunchInBtn`, `PunchOUTBtn`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_AttendanceInfo]
 
 @empId int

 as
 
 
BEGIN
declare @pInTime time(0)=null , @ShiftId   int=0, @AttStatus nvarchar(max)=''
set @pInTime=FORMAT(getdate(),'hh:mm tt')
		select @ShiftId=ShiftId, @AttStatus= AttStatus from  dbo.GetAttendanceStatusWithShiftId(@pInTime);

		if(@ShiftId>0)
		begin
		
	select CASE WHEN att1.PunchInTime  is not null THEN 'OFF' ELSE 'ON'   END PunchInBtn , CASE WHEN att1.PunchOutTime  is not null THEN 'OFF'  ELSE 'ON' END PunchOUTBtn FROM tblMarketAttendance_Master_webapi  att1
 

	WHERE att1.EmpInfoId = @empId AND CONVERT(DATE,att1.AttendanceDate) = CONVERT(DATE,GETDATE())    and  ShiftId=@ShiftId
		end
		else
		begin
		select  'OFF'      PunchInBtn ,  'OFF'  PunchOUTBtn
		end


END
```

## sp_Webapi_Get_AttendanceInformation2

- **Parameters**: `@param NVARCHAR(MAX) = NULL`, `@Role NVARCHAR(MAX) = NULL`, `@AppStatus NVARCHAR(MAX) = NULL`, `@AttType INT = NULL`, `@FromDt DATETIME = NULL`, `@ToDt DATETIME = NULL`, `@EmpId INT = NULL`
- **Tables read**: `tblMarketAttendance_Master_webapi`, `tblApprovalLog`, `tblEmpGeneralInfo`, `View_Webapi_EmployeeFieldForceInfo`, `tblRoleType`
- **Tables written**: none
- **Business logic**: Normalizes `@FromDt`/`@ToDt` (fixing a `Sept`→`Sep` locale quirk from `CONVERT(...,106)`), builds a dynamic filter (approval status, date/date-range, attendance type, employee), and returns attendance records joined to their approval-workflow log (latest step only) and the employee's field-force hierarchy, excluding rows whose current-step approver role matches `@Role` (i.e. items already actioned by the caller's role) — an attendance approval worklist/queue.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set of attendance + approval-log + employee/hierarchy + a computed attendance-photo `ImageString` path
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_AttendanceInformation2]
	-- Add the parameters for the stored procedure here
	@param NVARCHAR(MAX)= NULL,
	@Role NVARCHAR(MAX) =NULL,
	@AppStatus NVARCHAR(maX)= NULL,
	@AttType INT =NULL,
	@FromDt DATETIME =NULL,
	@ToDt DATETIME =NULL,
	@EmpId INT =NULL
AS
    BEGIN
	
  IF @FromDt IS NOT NULL
    BEGIN
        DECLARE @FromDateString VARCHAR(20) = CONVERT(VARCHAR, @FromDt, 106);
        SET @FromDateString = REPLACE(@FromDateString, 'Sept', 'Sep');
        SET @FromDt = CONVERT(DATETIME, @FromDateString);
    END
	

		IF @ToDt IS NOT NULL
    BEGIN
        DECLARE @ToDtString VARCHAR(20) = CONVERT(VARCHAR, @ToDt, 106);
        SET @ToDtString = REPLACE(@ToDtString, 'Sept', 'Sep');
        SET @ToDt = CONVERT(DATETIME, @ToDtString);
    END

	DECLARE @params NVARCHAR(max)='   '
	IF(@AppStatus IS NOT NULL)
	BEGIN
	    SET @params=' and tblMarketAttendance_Master_webapi.ApprovalStatus ='''+@AppStatus+''''
	IF(@FromDt IS NOT NULL AND @ToDt IS NULL)
		BEGIN
		    SET @params=@params+ ' AND Convert(Date,tblMarketAttendance_Master_webapi.AttendanceDate)='''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''''
		END
		IF(@FromDt IS NOT NULL AND @ToDt IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND Convert(Date,tblMarketAttendance_Master_webapi.AttendanceDate) between '''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''' AND '''+ CAST(CONVERT(DATE,@ToDt) AS NVARCHAR(max))+''' '
		END
	END
	ELSE
    BEGIN
        IF(@FromDt IS NOT NULL AND @ToDt IS NULL)
		BEGIN
		    SET @params=@params+ ' AND Convert(Date,tblMarketAttendance_Master_webapi.AttendanceDate)='''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''''
		END
		IF(@FromDt IS NOT NULL AND @ToDt IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND Convert(Date,tblMarketAttendance_Master_webapi.AttendanceDate) between '''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''' AND '''+ CAST(CONVERT(DATE,@ToDt) AS NVARCHAR(max))+''' '
		END
    END
	IF(@AttType IS NOT NULL)
	BEGIN
	    SET @params= @params + ' AND tblMarketAttendance_Master_webapi.AttType='+convert(nvarchar(max),@AttType)+' '
	END
	IF(@EmpId IS NOT NULL)
	BEGIN
	    SET @params= @params + ' AND tblMarketAttendance_Master_webapi.EmpInfoId='+convert(nvarchar(max),@EmpId)+' '
	END

	DECLARE @Q NVARCHAR(MAX)
	SET @Q='

	SELECT  tblMarketAttendance_Master_webapi.AttAddress, AttendanceId,
       --tblMarketAttendance_Master_webapi.MIOId,
       tblMarketAttendance_Master_webapi.EmpInfoId,
       PunchInTime,
       PInLat,
       PInLog,
       --PunchOutTime,
       --POutLat,
       --POutLong,
       --tblMarketAttendance_Master_webapi.EntryDate,
       POutRemarks,
       AttendanceDate,
       PINCreatedDateTime,
       POUTCreatedDateTime,
       tblMarketAttendance_Master_webapi.ApprovalStatus,
       tblMarketAttendance_Master_webapi.ShiftId,
	   tblMarketAttendance_Master_webapi.AttType,
       
       tblApprovalLog.ApprovalId,
       Date,
       FromEmpId,
       ToEmpId,
       tblApprovalLog.TableId,
       tblApprovalLog.Status,
       Comments,
       Type,
       Step,
       tblApprovalLog.GroupId,
       tblApprovalLog.RegionId,
       tblApprovalLog.AreaId,
       tblApprovalLog.TerritoryId,
       
       tblApprovalLog.RoleTypeId,ToRoleTypeId,
       
       
       tblEmpGeneralInfo.EmpMasterCode,
       tblEmpGeneralInfo.EmpName,
	   View_Webapi_EmployeeFieldForceInfo.TerritoryId,
                                 View_Webapi_EmployeeFieldForceInfo.AreaId,
                                 View_Webapi_EmployeeFieldForceInfo.RegionId,
                                 View_Webapi_EmployeeFieldForceInfo.GroupId,
                                 TerritoryName,
                                 TerritoryCode,
                                 AreaCode,
                                 AreaName,
                                 RegionCode,
                                 RegionName,
                                 GroupName,
                                 MIOEmpId,
                                 ASMEMPId,
                                 RSMEMPId,
                                 NSMEMPId,LogMax.MaxStep
	   ,(SELECT LTRIM(RTRIM(ImagePath+''/''+ImagePreName)) FROM dbo.tbl_ImagePath_Setting  with (nolock)  WHERE ImageType=''AttendanceMy'')+CAST(tblMarketAttendance_Master_webapi.AttendanceId as nvarchar(max))+''.jpg'' AS   ImageString 
	   
	   
	   FROM dbo.tblMarketAttendance_Master_webapi with (nolock)
LEFT JOIN dbo.tblApprovalLog  with (nolock) ON dbo.tblApprovalLog.TableId=dbo.tblMarketAttendance_Master_webapi.AttendanceId
LEFT JOIN (SELECT TableId,MAX(Step)MaxStep FROM dbo.tblApprovalLog  with (nolock)  GROUP BY TableId) AS LogMax ON LogMax.TableId=dbo.tblApprovalLog.TableId
LEFT JOIN dbo.tblEmpGeneralInfo  with (nolock) ON tblEmpGeneralInfo.EmpInfoId = tblMarketAttendance_Master_webapi.EmpInfoId
LEFT JOIN dbo.View_Webapi_EmployeeFieldForceInfo  with (nolock) ON View_Webapi_EmployeeFieldForceInfo.EmpInfoId = tblEmpGeneralInfo.EmpInfoId
left join (select TableId,RoleTypeId from tblApprovalLog  with (nolock) where Step=1) as tblrole on tblMarketAttendance_Master_webapi.AttendanceId=tblrole.TableId
left join tblRoleType  with (nolock) on tblRoleType.RoleTypeId=tblrole.RoleTypeId
WHERE tblMarketAttendance_Master_webapi.AttendanceId is not null AND   tblRoleType.RoleType<>'''+@Role+'''  '+@params+'  AND Step=LogMax.MaxStep '+@param

EXEC sys.sp_executesql @Q


    END


```

## sp_Webapi_Get_CampaingData

- **Parameters**: `@empId INT`
- **Tables read**: `tblBonusCampaignMaster`, `tblCompanyInfo`, `tblEmpGeneralInfo`
- **Tables written**: none
- **Business logic**: Returns active bonus/incentive campaigns for the company the given employee belongs to.
- **Transaction/error handling**: none
- **Result set**: `CampaignCode`, `CampaignName`, `CampaignDesc`, `FromDate`, `Todate`, `Type`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_CampaingData]
	-- Add the parameters for the stored procedure here
@empId int
AS
BEGIN
		
SELECT
       A.CampaignCode ,
       A.CampaignName ,
       A.CampaignDesc ,
       CONVERT(NVARCHAR(50),A.FromDate,107)AS FromDate ,
       CONVERT(NVARCHAR(50),A.Todate,107)AS Todate,
       A.Type  FROM dbo.tblBonusCampaignMaster A 
INNER JOIN dbo.tblCompanyInfo B ON B.CompanyId = A.CompanyId
INNER JOIN dbo.tblEmpGeneralInfo C ON B.CompanyId = C.CompanyId
WHERE C.EmpInfoId = @empId
AND A.IsActive = 1

END

```

## sp_Webapi_Get_ChamberName

- **Parameters**: `@empid INT`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblDoctorChemberDetail`, `View_DoctorMaster`, `tblDoctorMaster`, `tblMIOInfo`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `View_Webapi_EmployeeFieldForceInfo`
- **Tables written**: none
- **Business logic**: Resolves the caller's role type (1=Territory/MIO, 2=Area/AM-team, 3=Region, 4=Group) and appends a matching dynamic-SQL WHERE clause, then returns distinct doctor chamber names scoped to that hierarchy level.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — `ChemberId`, `ChemberName`, `DoctorId`
- **Caller**: UNUSED — no application caller found

```sql
 






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE 
[dbo].[sp_Webapi_Get_ChamberName]
	-- Add the parameters for the stored procedure here
  @empid INT 

AS
BEGIN
		    DECLARE @Q NVARCHAR(MAX)=''
	DECLARE @Param NVARCHAR(MAX)=''
	DECLARE @Id NVARCHAR(MAX)=0
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId

	SET @Q='
	 

--SELECT 
--    100 AS ChemberId,  
--    ''N/A'' AS ChemberName,  
--    1 AS DoctorId

--	UNION ALL 
	SELECT distinct tblDoctorChemberDetail.ChemberId  ChemberId,ltrim(rtrim(name))   ChemberName, tblDoctorChemberDetail.DoctorId FROM dbo.tblDoctorChemberDetail with (nolock)
	inner JOIN dbo.View_DoctorMaster CV  with (nolock) ON CV.DoctorId=tblDoctorChemberDetail.DoctorId 
	inner JOIN dbo.tblDoctorMaster doc  with (nolock) ON doc.DoctorId=tblDoctorChemberDetail.DoctorId 
		
		
		'

	--SELECT * FROM dbo.View_CustomerMaster WHERE GroupId
		 DECLARE @TRID NVARCHAR(MAX)
	IF(@Roletype='1')
	BEGIN
		SELECT @TRID=	STUFF( (SELECT CONCAT(',',TerritoryId , '') FROM dbo.tblMIOInfo WHERE EmployeeId=@empId and IsActive=1 FOR XML PATH ('') ),1,1,'')

	    SET @Param=@Param+' WHERE CV.TerritoryId  in ( '+ @TRID +')';
	END
	IF(@Roletype='2')
	BEGIN
	  	select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @Param=@Param+ ' WHERE  CV.TerritoryId in ( '+ @TRID +')';
	END
	IF(@Roletype='3')
	BEGIN
	   SELECT   @Id=  STUFF( (SELECT CONCAT(',', mm.EmpRegionId , '')   FROM View_Webapi_EmployeeFieldForceInfo mm (NOLOCK)   WHERE EmpInfoId=@empId   FOR XML PATH ('') ),1,1,'')  
	   

	    SET @Param=@Param+' WHERE CV.RegionId in ('+@Id+')'
	END
	IF(@Roletype='4')
	BEGIN
	    SELECT @Id=EmpGroupId FROM dbo.View_Webapi_EmployeeFieldForceInfo WHERE EmpInfoId=@empId

	    SET @Param=@Param+' WHERE CV.GroupId='+@Id
	END

	SET @Q=@Q+@Param

    EXEC sys.sp_executesql @Q
END



```

## sp_Webapi_Get_ClientVisitReportDtl

- **Parameters**: `@DocTPMaster NVARCHAR(MAX)`, `@wings_id NVARCHAR(MAX)`, `@date NVARCHAR(MAX)`, `@role NVARCHAR(MAX)`
- **Tables read**: `tblEmpGeneralInfo`, `tblUser`, `tblDesignation`, `tbl_UserRoleInfo`, `tblRoleType`, `tbl_DoctorTourPlanDetail`, `tbl_TourPlanInfo`, `tblCompanyInfo`, `tbl_DCRInfo`, `tbl_DCRCustomerInfo`
- **Tables written**: none
- **Business logic**: For a given company/area/date/role, computes a per-employee client-visit-report detail row: planned doctor visits vs. actual DCR (doctor call report) visits, and planned customer visits vs. actual customer-DCR visits, filtered to employees who had any planned target that day.
- **Transaction/error handling**: none
- **Result set**: `DetailsId`, `EmployeeId`, `EmployeeName`, `DailyPlan`, `DailyVisit`, `PlannedVisit`, `ActualVisit`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_ClientVisitReportDtl]
   @DocTPMaster nvarchar(max) ,
    @wings_id nvarchar(max),
   @date nvarchar(max),
   @role nvarchar(max)

AS
BEGIN

 

select  distinct  PM.EmpInfoId DetailsId, PM.EmpInfoId EmployeeId,   PM.EmpName EmployeeName, ISNULL(tblDrTarget.DoctorTarget,0)  DailyPlan,  ISNULL(tblDCR.DCRTarget,0)  DailyVisit, ISNULL(tblCsTarget.CustomerTarget,0) PlannedVisit,ISNULL(tblCusDCR.DCRCusTarget,0) ActualVisit   from   tblEmpGeneralInfo PM with (nolock)
left join tblUser us  with (nolock) on PM.EmpInfoId=us.EmpInfoId 
   left join tblDesignation dgs  with (nolock) on PM.DesignationId=dgs.DesignationId
   left join tbl_UserRoleInfo usR  with (nolock) on usR.UserRoleId=us.UserRoleId
   left join tblRoleType usRT  with (nolock) on usR.RoleTypeId=usRT.RoleTypeId
   left join (select    ISNULL(COUNT(distinct Tdtl.DoctorId),0) DoctorTarget ,Tdtl.EmpInfoId    from tbl_DoctorTourPlanDetail Tdtl where  convert(Date, TourPlanDate)  = convert(Date, @date) group by Tdtl.EmpInfoId ) tblDrTarget on tblDrTarget.EmpInfoId=PM.EmpInfoId

   
     left join (select distinct    ISNULL(COUNT(distinct CustomerMasterId),0) CustomerTarget,    EmpInfoId   from tbl_TourPlanInfo   where  convert(Date, TourPlanDate) = convert(Date, @date)   group by   EmpInfoId ) tblCsTarget on tblCsTarget.EmpInfoId=PM.EmpInfoId  


	   left join (select    COUNT(distinct DoctorId) ChkDoctorTarget,  TourPlanDate, EmpInfoId, CompanyId, AreaId  from tbl_DoctorTourPlanDetail    where  convert(Date, TourPlanDate)= convert(Date, @date) group by TourPlanDate, EmpInfoId ,CompanyId, AreaId  HAVING COUNT(distinct DoctorId)>0 ) tblDrInfo on tblDrInfo.EmpInfoId=PM.EmpInfoId


	   left join (select    COUNT(distinct CustomerMasterId) ChkCustomerTarget,  TourPlanDate, EmpInfoId, CompanyId, AreaId from tbl_TourPlanInfo  where  convert(Date, TourPlanDate) = convert(Date, @date)     group by TourPlanDate, EmpInfoId ,CompanyId, AreaId  HAVING COUNT(distinct CustomerMasterId)>0 ) tblCrInfo on tblCrInfo.EmpInfoId=PM.EmpInfoId


	   	 left join tblCompanyInfo com on com.CompanyId=isnull(tblDrInfo.CompanyId,tblCrInfo.CompanyId)


		  left join (select   COUNT(distinct dcr.DoctorId) DCRTarget,   CONVERT(Date,dcr.DcrDate) DcrDate, u.EmpInfoId  from tbl_DCRInfo  dcr
	inner join tblUser u on dcr.EntryBy=u.UserId
	 group by CONVERT(Date,dcr.DcrDate), u.EmpInfoId) tblDCR on tblDCR.EmpInfoId=PM.EmpInfoId  and CONVERT(Date,isnull(tblCrInfo.TourPlanDate,tblDrInfo.TourPlanDate ))=CONVERT(Date,tblDCR.DcrDate)

	      left join (select   COUNT(distinct dcr.CustomerId) DCRCusTarget,   CONVERT(Date,dcr.DcrDate) DcrDate, dcr.EmpInfoId  from tbl_DCRCustomerInfo  dcr
	 
	 group by CONVERT(Date,dcr.DcrDate), dcr.EmpInfoId) tblCusDCR on tblCusDCR.EmpInfoId=PM.EmpInfoId  and CONVERT(Date,isnull(tblCrInfo.TourPlanDate,tblDrInfo.TourPlanDate ))=CONVERT(Date,tblCusDCR.DcrDate)




    where     ISNULL(DoctorTarget,0) + ISNULL(CustomerTarget,0)>0

 
and   isnull(tblDrInfo.AreaId,tblCrInfo.AreaId)=@DocTPMaster    and com.CompanyId=@wings_id and usRT.RoleTypeId=@role  
 
 
 
END
 


```

## sp_Webapi_Get_ClientVisitReportMaster

- **Parameters**: `@wings_id NVARCHAR(MAX)`, `@date NVARCHAR(MAX)`, `@role NVARCHAR(MAX)`
- **Tables read**: `tbl_DoctorTourPlanDetail`, `tbl_DoctorTourPlanMaster`, `tbl_TourPlanInfo`, `tbl_TourPlanMaster`, `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`
- **Tables written**: none
- **Business logic**: Returns the distinct list of areas that had a doctor tour plan or a customer tour plan on the given date for the given company/role, as a master/filter list feeding the detail report above.
- **Transaction/error handling**: none
- **Result set**: `DocTPMaster` (area id), `Area` (area name)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_ClientVisitReportMaster]
   @wings_id nvarchar(max),
   @date nvarchar(max),
   @role nvarchar(max)


AS
BEGIN

 select distinct  * from (

select  distinct  dtl.AreaId DocTPMaster,  AreaName_DV Area from tbl_DoctorTourPlanDetail  dtl  with (nolock)
inner join tbl_DoctorTourPlanMaster mas  with (nolock) on mas.DocTPMaster=dtl.DocTPMaster
	LEFT JOIN dbo.tblUser u ON u.EmpInfoId = dtl.EmpInfoId
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = u.UserRoleID
				LEFT JOIN dbo.tblRoleType rt ON rt.RoleTypeId =tbl_UserRoleInfo.RoleTypeId
 
  
 
where dtl.CompanyId=@wings_id and rt.RoleTypeId=@role and convert(Date,dtl.TourPlanDate)= convert(Date, @date  )
 


 union all

select  distinct  dtl.AreaId DocTPMaster,  AreaName  Area from tbl_TourPlanInfo  dtl  with (nolock)
inner join tbl_TourPlanMaster mas  with (nolock) on mas.TPMaster=dtl.TPMaster
	LEFT JOIN dbo.tblUser u ON u.EmpInfoId = dtl.EmpInfoId
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = u.UserRoleID
				LEFT JOIN dbo.tblRoleType rt ON rt.RoleTypeId =tbl_UserRoleInfo.RoleTypeId
 
  
 
where dtl.CompanyId=@wings_id and rt.RoleTypeId=@role and convert(Date,dtl.TourPlanDate)= convert(Date, @date  )
 
 )tbl 
 
 
END
 


```

## sp_Webapi_Get_CustomerByMarketId

- **Parameters**: `@marketId INT = NULL`
- **Tables read**: `tblCustMaster`
- **Tables written**: none
- **Business logic**: Returns active customers belonging to the given market, with a combined "code : name" display label.
- **Transaction/error handling**: none
- **Result set**: `CustomerMasterId`, `CustomerName` (code : name), `Address`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_CustomerByMarketId]
	-- Add the parameters for the stored procedure here
@marketId INT = NULL
AS
BEGIN
		
		SELECT CustomerMasterId ,
              (CustomerCode +' : '+ CustomerName)AS CustomerName  ,
               Address 
			    FROM dbo.tblCustMaster WHERE MarketId = @marketId AND IsActive = 1
		 

END
```

## sp_Webapi_Get_DICcheck

- **Parameters**: `@EmployeeId NVARCHAR(MAX) = NULL`
- **Tables read**: `tbluser`, `tbl_UserRoleInfo`, `tblRoleType`
- **Tables written**: none
- **Business logic**: Looks up the role type of the given employee. Contains a commented-out block that would have computed a next CustomerApprovalId for DZSM role; that logic is currently dead/disabled.
- **Transaction/error handling**: none
- **Result set**: `UserType`, `NewEmpId` (currently always NULL since the assignment is commented out)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DICcheck]
	-- Add the parameters for the stored procedure here
	 @EmployeeId NVARCHAR(MAX)= NULL
	AS
    BEGIN
  

 
 
  declare @UserType nvarchar(max)
  declare @NewEmpId nvarchar(max)


	select  @UserType= usrT.RoleType from tbluser us
	inner join tbl_UserRoleInfo usr ON usr.UserRoleID=us.UserRoleID
	inner join tblRoleType usrT ON usr.RoleTypeId=usrT.RoleTypeId

	 where EmpInfoId=@EmployeeId

	 print @UserType

	  --if(@UserType='DZSM')
	  --begin
	  --select @NewEmpId= max(CustomerApprovalId)+1 from tblCustomerApprovalLog 
	  --end


	  select @UserType   UserType,@NewEmpId  NewEmpId
	end
```

## sp_Webapi_Get_DZSMMultipleArea

- **Parameters**: `@AreId NVARCHAR(MAX) = NULL`
- **Tables read**: `tblRSMInfo`
- **Tables written**: none
- **Business logic**: Given a region id, resolves the RSM/DZSM assigned to it, then returns all other active regions assigned to that same RSM (multi-region coverage list), mirroring sp_Webapi_Get_AMMultipleArea at the regional level.
- **Transaction/error handling**: none
- **Result set**: `RegionId` (list)
- **Caller**: UNUSED — no application caller found

```sql
create PROCEDURE [dbo].[sp_Webapi_Get_DZSMMultipleArea]
	-- Add the parameters for the stored procedure here
	@AreId NVARCHAR(MAX)= NULL
	AS
    BEGIN


	declare    @PersonalEmpId nvarchar(max)
	select @PersonalEmpId=EmployeeId from tblRSMInfo where RegionId=@AreId
	select    RegionId from tblRSMInfo where EmployeeId=@PersonalEmpId  and IsActive=1 order by RSMId asc
	 

	end
```

## sp_Webapi_Get_DcrListById

- **Parameters**: `@empId NVARCHAR(50)`, `@FromDt NVARCHAR(MAX) = NULL`, `@ToDt NVARCHAR(MAX) = NULL`, `@providertype NVARCHAR(MAX) = NULL` (unused in body), `@pharmatype NVARCHAR(MAX) = NULL` (unused in body), `@doctortype NVARCHAR(MAX) = NULL` (unused in body), `@Role NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `tbl_DCRInfo`, `tbl_TourPlanType`, `tblDoctorChemberDetail`, `tblDoctorMaster`
- **Tables written**: none
- **Business logic**: Returns a list of doctor-call-report (DCR) entries, filtered by date range and scoped either by territory (Role='Team') or by the entering user (EntryBy), joined to tour type, chamber, employee and doctor names.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — `DcrId`, `DcrDate`, `TourTypeName`, `ChamberAddress`, `EmpName`, `IsApproved`, `Name` (doctor), `ApprovalStatus`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DcrListById]
	-- Add the parameters for the stored procedure here
    @empId NVARCHAR(50),
	
	@FromDt NVARCHAR(MAX) =NULL,
	@ToDt NVARCHAR(MAX) =NULL,
	@providertype NVARCHAR(MAX) =NULL,
	@pharmatype NVARCHAR(MAX) =NULL,
	@doctortype NVARCHAR(MAX) =NULL,
@Role   NVARCHAR(MAX)= NULL


AS
    BEGIN

	DECLARE @userId NVARCHAR(max) ,   @params NVARCHAR(max) =''
	SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @empId
	 IF(@FromDt IS NOT NULL AND @ToDt IS NULL)
		BEGIN
		    SET @params=@params+ ' AND convert(Date, A.DcrDate)='''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''''
		END
		IF(@FromDt IS NOT NULL AND @ToDt IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND convert(Date,A.DcrDate) between '''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''' AND '''+CAST(CONVERT(DATE,@ToDt) AS NVARCHAR(max))+''' '
		END


		 DECLARE @TRID NVARCHAR(MAX);
		  SELECT @UserId=UserId FROM dbo.tblUser WHERE EmpInfoId=@empId

			  if(@Role = 'Team')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND A.TerritoryId in ( '+ @TRID +')';

			END
			else
			BEGIN
				 

				SET @params=@params+ ' and A.EntryBy='+@userId ;

			END



	
		DECLARE @Q NVARCHAR(MAX)
	SET @Q='
	
        SELECT  A.DcrId ,
                FORMAT(A.DcrDate, ''MMMM dd, yyyy hh:mm tt'') AS DcrDate ,
                B.TourTypeName ,
                C.Name AS ChamberAddress ,
                D.EmpName ,
                A.IsApproved ,
                M.DoctorName AS Name, A.ApprovalStatus
        FROM    dbo.tbl_DCRInfo A
                LEFT JOIN dbo.tbl_TourPlanType B ON B.TourTypeId = A.TourTypeId
                LEFT JOIN dbo.tblDoctorChemberDetail C ON C.ChemberId = A.ChemberId
				LEFT JOIN dbo.tblUser us ON us.UserId = A.EntryBy
                LEFT JOIN dbo.tblEmpGeneralInfo D ON D.EmpInfoId = us.EmpInfoId
                LEFT JOIN dbo.tblDoctorMaster M ON M.DoctorId = A.DoctorId
             

WHERE A.DcrId is not null '  +@params 

EXEC sys.sp_executesql @Q


    END

```

## sp_Webapi_Get_DcrListByIdNew

- **Parameters**: `@empId NVARCHAR(50)`, `@FromDt NVARCHAR(MAX) = NULL`, `@ToDt NVARCHAR(MAX) = NULL`, `@providertype NVARCHAR(MAX) = NULL` (unused), `@pharmatype NVARCHAR(MAX) = NULL` (unused), `@doctortype NVARCHAR(MAX) = NULL` (unused), `@Role NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `tbl_DCRCustomerInfo`, `tblCustMaster`, `tbl_DCRInfo`, `tblDoctorMaster`
- **Tables written**: none
- **Business logic**: Newer version of sp_Webapi_Get_DcrListById that unions customer-DCR visits and doctor-DCR visits into one combined visit list (name, address, visit date/time, lat/lon split out of a combined lat,lon string, and a decoded approval-status label), scoped by territory or by EmpInfoId.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — `EmpInfoId`, `TerritoryId`, `Name`, `Address`, `DcrDate`, `VisitTime`, `Status`, `feedback`, `Lat`, `Lon`, `ApprovalStatus`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DcrListByIdNew]
	-- Add the parameters for the stored procedure here
    @empId NVARCHAR(50),
	
	@FromDt NVARCHAR(MAX) =NULL,
	@ToDt NVARCHAR(MAX) =NULL,
	@providertype NVARCHAR(MAX) =NULL,
	@pharmatype NVARCHAR(MAX) =NULL,
	@doctortype NVARCHAR(MAX) =NULL,
@Role   NVARCHAR(MAX)= NULL


AS
    BEGIN

	DECLARE @userId NVARCHAR(max) ,   @params NVARCHAR(max) =''
	SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @empId
	 IF(@FromDt IS NOT NULL AND @ToDt IS NULL)
		BEGIN
		    SET @params=@params+ ' AND convert(Date, A.DcrDate)='''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''''
		END
		IF(@FromDt IS NOT NULL AND @ToDt IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND convert(Date,A.DcrDate) between '''+CAST(CONVERT(DATE,@FromDt) AS NVARCHAR(max))+''' AND '''+CAST(CONVERT(DATE,@ToDt) AS NVARCHAR(max))+''' '
		END


		 DECLARE @TRID NVARCHAR(MAX);
		  SELECT @UserId=UserId FROM dbo.tblUser WHERE EmpInfoId=@empId

			  if(@Role = 'Team')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND A.TerritoryId in ( '+ @TRID +')';

			END
			else
			BEGIN
				 

				SET @params=@params+ ' and A.EmpInfoId='+@empId ;

			END



	
		DECLARE @Q NVARCHAR(MAX)
	SET @Q='
	select * from (

select D.EmpInfoId, dcr.TerritoryId, mas.CustomerCode + '' : '' +mas.CustomerName Name, dcr.StreetAddress Address, FORMAT(dcr.DcrDate, ''dd-MMM-yyyy'') DcrDate, FORMAT(dcr.EntryDate, ''hh:mm:ss tt'') VisitTime, ''yes'' Status, dcr.Remarks feedback, dcr.Latitude Lat, dcr.Longitude Lon, case when dcr.ApprovalStatus=''0'' then ''Pending''  when dcr.ApprovalStatus=''1'' then ''Verified''  when dcr.ApprovalStatus=''2'' then ''Approved''  WHEN dcr.ApprovalStatus=''3'' then ''Rejected''  else dcr.ApprovalStatus end ApprovalStatus from tbl_DCRCustomerInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerId
   LEFT JOIN dbo.tblEmpGeneralInfo D ON D.EmpInfoId = dcr.EmpInfoId

--where dcr.EmpInfoId=@EmpInfoId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)


union all

select D.EmpInfoId,dcr.TerritoryId, mas.DoctorCode  + '' : '' + mas.DoctorName Name, dcr.StreetAddress Address, FORMAT(dcr.DcrDate, ''dd-MMM-yyyy'') DcrDate, FORMAT(dcr.EntryDate, ''hh:mm:ss tt'') VisitTime, ''yes'' Status, dcr.Remarks feedback,  CASE
        WHEN CHARINDEX('','', dcr.Latitude) > 0 THEN
            SUBSTRING(dcr.Latitude, 1, CHARINDEX('','', dcr.Latitude) - 1)
        ELSE
            dcr.Latitude
    END AS Lat,
    CASE
        WHEN CHARINDEX('','', dcr.Latitude) > 0 THEN
            SUBSTRING(dcr.Latitude, CHARINDEX('','', dcr.Latitude) + 1, LEN(dcr.Latitude))
        ELSE
            NULL -- or an appropriate default value for longitude when there is no comma
    END AS Lon, case when dcr.ApprovalStatus=''0'' then ''Pending''  when dcr.ApprovalStatus=''1'' then ''Verified''  when dcr.ApprovalStatus=''2'' then ''Approved''  WHEN dcr.ApprovalStatus=''3'' then ''Rejected''  else dcr.ApprovalStatus end  ApprovalStatus from tbl_DCRInfo dcr with (nolock)
inner join tblDoctorMaster mas  with (nolock) on mas.DoctorId=dcr.DoctorId
inner join tblUser us  with (nolock) on us.UserId=dcr.EntryBy
   LEFT JOIN dbo.tblEmpGeneralInfo D ON D.EmpInfoId = us.EmpInfoId

   ) A  where A.EmpInfoId is not null'  +@params 

EXEC sys.sp_executesql @Q


    END

```

## sp_Webapi_Get_DcrProductByType

- **Parameters**: `@empId INT`, `@type NVARCHAR(50) = NULL`
- **Tables read**: `tblUser`, `tblEmpGeneralInfo`, `tbl_UserRoleInfo`, `tblRoleType`, `tblMIOInfo`, `tblASMInfo`, `tblRSMInfo`, `tblProductGroup`, `tblCompanyInfo`, `tblProduct`
- **Tables written**: none
- **Business logic**: Resolves the employee's company via role (MIO/AM/DZSM), resolves @type to a product group id, and returns the active products for that company (the @type→GroupId lookup result is computed but not actually applied as a filter on tblProduct).
- **Transaction/error handling**: none
- **Result set**: `ProductId`, `ProductCode`, `ProductName`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DcrProductByType]
	-- Add the parameters for the stored procedure here
	@empId INT,
	@type NVARCHAR(50) =null
AS
BEGIN
		Declare @Roltp nvarchar(max)=''
 declare @ComId int=0 
 select @Roltp=RoleType   FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId


if(@Roltp='MIO')
		begin 
		select @ComId=CompanyId from tblMIOInfo where EmployeeId=@EMPId and IsActive=1
		end

			if(@Roltp='AM')
		begin 
		select @ComId=CompanyId from tblASMInfo where EmployeeId=@EMPId and IsActive=1
		end
 	if(@Roltp='DZSM')

	begin 
		select @ComId=CompanyId from tblRSMInfo where EmployeeId=@EMPId and IsActive=1
		end
		DECLARE @typeid INT
        SELECT @typeid = GroupId FROM dbo.tblProductGroup WHERE LTRIM(RTRIM(GroupName))= @type

	SELECT distinct  C.ProductId,C.ProductCode,C.ProductName FROM dbo.tblEmpGeneralInfo A 
	
   
INNER JOIN dbo.tblCompanyInfo B ON B.CompanyId = @ComId
INNER JOIN dbo.tblProduct C ON C.CompanyId = B.CompanyId 
WHERE A.EmpInfoId = @empId AND C.IsActiveProduct = 1  


END

 
```

## sp_Webapi_Get_DoctorBrand

- **Parameters**: `@empId INT`
- **Tables read**: `tblUser`, `tblEmpGeneralInfo`, `tbl_UserRoleInfo`, `tblRoleType`, `tblMIOInfo`, `tblASMInfo`, `tblRSMInfo`, `tblCompanyInfo`, `tblProduct`, `tblProductSQ`
- **Tables written**: none
- **Business logic**: Resolves the employee's company by role, then returns the distinct product brands (SQ) available for that company (excluding brand id 15 in the main branch, then adding brand 15 back in with a forced MaxValue of 0) — a brand picklist for DCR/prescription entry.
- **Transaction/error handling**: none
- **Result set**: `MaxValue`, `ProductBrandId`, `ProductSQName`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorBrand] 
	-- Add the parameters for the stored procedure here
	@empId int
AS
BEGIN

	Declare @Roltp nvarchar(max)=''
 declare @ComId int=0 
 select @Roltp= RoleType   FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId

				if(@Roltp='MIO')
		begin 
		select @ComId= CompanyId from tblMIOInfo where EmployeeId=@EMPId and IsActive=1
		end

			if(@Roltp='AM')
		begin 
		select @ComId=CompanyId from tblASMInfo where EmployeeId=@EMPId and IsActive=1
		end
 	if(@Roltp='DZSM')

	begin 
		select @ComId=CompanyId from tblRSMInfo where EmployeeId=@EMPId and IsActive=1
		end

		 

		select distinct ISNULL(MAX(MaxValue),0) as MaxValue, sq.ProductBrandId, sq.ProductSQName  


		FROM dbo.tblEmpGeneralInfo A 
INNER JOIN dbo.tblCompanyInfo B ON B.CompanyId = @ComId
INNER JOIN dbo.tblProduct C ON C.CompanyId = B.CompanyId
INNER JOIN dbo.tblProductSQ sq ON sq.ProductBrandId = C.ProductBrandId

WHERE   C.IsActiveProduct = 1   and sq.ProductBrandId not in(15)
		group by sq.ProductBrandId, sq.ProductSQName

		union all 	select distinct 0 as MaxValue, sq.ProductBrandId, sq.ProductSQName  from  tblProductSQ sq  where   sq.ProductBrandId   in(15)
		order by  sq.ProductSQName asc


END


```

## sp_Webapi_Get_DoctorBrandByDoctorId

- **Parameters**: `@DoctorId INT`
- **Tables read**: `tblDoctorBrandDetail`, `tblProductSQ`
- **Tables written**: none
- **Business logic**: Returns the brands already tagged against a specific doctor.
- **Transaction/error handling**: none
- **Result set**: `DoctorId`, `BrandId`, `BrandName`
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
create PROCEDURE [dbo].[sp_Webapi_Get_DoctorBrandByDoctorId]
	-- Add the parameters for the stored procedure here
	@DoctorId int
AS
BEGIN 

select mas.DoctorId DoctorId, mas.BrandId BrandId, pr.ProductSQName BrandName FROM dbo.tblDoctorBrandDetail mas WITH (NOLOCK)

		LEFT JOIN dbo.tblProductSQ pr  WITH (NOLOCK) ON mas.BrandId=pr.ProductBrandId 

	 

where mas.DoctorId=@DoctorId
END






```

## sp_Webapi_Get_DoctorCategory

- **Parameters**: none
- **Tables read**: `tblDoctorCategory`
- **Tables written**: none
- **Business logic**: Returns all active doctor categories (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: all columns of tblDoctorCategory where IsActive = 1
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Webapi_Get_DoctorCategory] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		select * from tblDoctorCategory  with (nolock) where IsActive=1


END


```

## sp_Webapi_Get_DoctorChamber

- **Parameters**: none
- **Tables read**: `tblDoctorChamber`
- **Tables written**: none
- **Business logic**: Returns all active chamber types (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `ChamberTypeId`, `ChamberTypeName`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorChamber]
	-- Add the parameters for the stored procedure here

AS
BEGIN
		


		SELECT ChamberId ChamberTypeId,ChamberName ChamberTypeName
			    FROM dbo.tblDoctorChamber WHERE IsActive = 1


END


```

## sp_Webapi_Get_DoctorChamberByDocId

- **Parameters**: `@doctorId INT`
- **Tables read**: `tblDoctorChemberDetail`
- **Tables written**: none
- **Business logic**: Returns the named chambers (with phone/address) recorded for a specific doctor, excluding blank-named rows.
- **Transaction/error handling**: none
- **Result set**: `ChamberId`, `ChamberName`, `Phone`, `Address`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorChamberByDocId]
	-- Add the parameters for the stored procedure here
	@doctorId INT 
AS
BEGIN
	SELECT ChemberId AS ChamberId ,
           Name AS ChamberName,
          isnull(Phone,'') Phone ,
         isnull(Address,'')   Address FROM dbo.tblDoctorChemberDetail WHERE DoctorId = @doctorId AND Name<>'' 
END


```

## sp_Webapi_Get_DoctorChamberByDoctorId

- **Parameters**: `@DoctorId INT`
- **Tables read**: `tblDoctorChemberDetail`, `tblDoctorChamber`
- **Tables written**: none
- **Business logic**: Returns the chamber-type assignments for a specific doctor, resolving the chamber-type name (defaulting to N/A when unmatched) — a near-duplicate of sp_Webapi_Get_DoctorChamberByDocId with a different join/shape.
- **Transaction/error handling**: none
- **Result set**: `DoctorId`, `ChemberId`, `ChemberName`
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorChamberByDoctorId]
	-- Add the parameters for the stored procedure here
	@DoctorId int
AS
BEGIN 

select mas.DoctorId DoctorId, mas.ChemberId ChemberId, isnull(pr.ChamberName,'N/A') ChemberName FROM dbo.tblDoctorChemberDetail mas WITH (NOLOCK)

		LEFT JOIN dbo.tblDoctorChamber pr  WITH (NOLOCK) ON mas.ChemberId=pr.ChamberId 

	 

where mas.DoctorId=@DoctorId
END





--select * from tblDoctorChemberDetail
```

## sp_Webapi_Get_DoctorContactType

- **Parameters**: none
- **Tables read**: `tbl_ContactType`
- **Tables written**: none
- **Business logic**: Returns all active contact types (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: all columns of tbl_ContactType where IsActive = 1
- **Caller**: UNUSED — no application caller found

```sql
 
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorContactType]
	-- Add the parameters for the stored procedure here

AS
BEGIN
		


		SELECT *
			    FROM tbl_ContactType WHERE IsActive = 1


END


```

## sp_Webapi_Get_DoctorDegree

- **Parameters**: none
- **Tables read**: `tblDoctorDegree`
- **Tables written**: none
- **Business logic**: Returns active doctor degrees, each tagged with its associated doctor type (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `DegreeId`, `DegreeName`, `DoctorTypeId`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorDegree] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		
		SELECT DegreeId,DegreeName, DoctorTypeId FROM dbo.tblDoctorDegree WHERE IsActive = 1
		 



END


```

## sp_Webapi_Get_DoctorDesignation

- **Parameters**: none
- **Tables read**: `tblDoctorDesignation`
- **Tables written**: none
- **Business logic**: Returns active doctor designations (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `DesignationId`, `DesignationName`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorDesignation]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	
	SELECT DesignationId ,
           DesignationName  FROM dbo.tblDoctorDesignation WHERE IsActive = 1

END


```

## sp_Webapi_Get_DoctorInstitute

- **Parameters**: none
- **Tables read**: `tblInstitutionInfo`
- **Tables written**: none
- **Business logic**: Returns active institutions (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `InstitutionId`, `Institution`
- **Caller**: UNUSED — no application caller found

```sql
 
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorInstitute]
	-- Add the parameters for the stored procedure here

AS
BEGIN

SELECT  InstitutionId,  Institution  FROM dbo.tblInstitutionInfo with(nolock)  WHERE IsActive = 1
		
		--SELECT 1 AS InstitutionId,'Ins -1' AS Institution



END


```

## sp_Webapi_Get_DoctorList

- **Parameters**: `@empId INT`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `View_DoctorMaster`, `tblMIOInfo`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `View_Webapi_EmployeeFieldForceInfo`
- **Tables written**: none
- **Business logic**: Resolves the caller's role type (territory/area-team/region/group) and returns the doctor master list (from `View_DoctorMaster`) filtered to that scope — the main doctor picklist for the mobile app.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — doctor code/name/contact/type/program/chamber plus hierarchy ids (`GroupId`, `RegionId`, `AreaId`, `TerritoryId`, `MarketId`), `SMCTypeId`/`SMCType`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorList]
	-- Add the parameters for the stored procedure here
    @empId INT
AS
    BEGIN
		    DECLARE @Q NVARCHAR(MAX)=''
	DECLARE @Param NVARCHAR(MAX)=''
	DECLARE @Id NVARCHAR(MAX)=0
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId

	SET @Q=' select DISTINCT 0 SubTerritoryId, 0 IsSelect, [MarketCode] MarketCode, [DoctorId]
      ,[DoctorCode]
      ,[DoctorName]
      ,[DocContact]
      ,[DoctorTypeName]
      ,isnull(ProgramTypeName,'''') ProgramTypeName
      ,ltrim(rtrim([ChemberName])) [ChemberName],View_DoctorMaster.GroupId, View_DoctorMaster.RegionId,View_DoctorMaster.AreaId, View_DoctorMaster.TerritoryId,   View_DoctorMaster.MarketId ,   MarketName MarketName, SMCTypeId, SMCType, ProgramTypeId, DoctorTypeId from View_DoctorMaster     with (nolock)     '

	--SELECT * FROM dbo.View_CustomerMaster WHERE GroupId
		 DECLARE @TRID NVARCHAR(MAX)
	IF(@Roletype='1')
	BEGIN
		SELECT @Id=STUFF(
    (SELECT ',' + CAST(TerritoryId AS VARCHAR)
     FROM dbo.tblMIOInfo 
     WHERE EmployeeId = @empId AND IsActive = 1
     FOR XML PATH('')), 
    1, 1, '');
	   -- SET @Param=@Param+' WHERE TerritoryId='+@Id
		SET @Param=@Param+ ' WHERE  TerritoryId in ( '+ @Id +')';
	END
	IF(@Roletype='2')
	BEGIN
	  	select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @Param=@Param+ ' WHERE  TerritoryId in ( '+ @TRID +')';
	END
	IF(@Roletype='3')
	BEGIN
	   SELECT   @Id=  STUFF( (SELECT CONCAT(',', mm.EmpRegionId , '')   FROM View_Webapi_EmployeeFieldForceInfo mm (NOLOCK)   WHERE EmpInfoId=@empId   FOR XML PATH ('') ),1,1,'')  
	   

	    SET @Param=@Param+' WHERE RegionId in ('+@Id+')'
	END
	IF(@Roletype='4')
	BEGIN
	    SELECT @Id=EmpGroupId FROM dbo.View_Webapi_EmployeeFieldForceInfo WHERE EmpInfoId=@empId

	    SET @Param=@Param+' WHERE GroupId='+@Id
	END

	SET @Q=@Q+@Param

    EXEC sys.sp_executesql @Q

	end
```

## sp_Webapi_Get_DoctorListDate

- **Parameters**: `@empId INT`, `@Date NVARCHAR(MAX)`
- **Tables read**: same as `sp_Webapi_Get_DoctorList` plus `tbl_DoctorTourPlanDetail`
- **Tables written**: none
- **Business logic**: Same scoped doctor list as `sp_Webapi_Get_DoctorList`, but additionally flags (`IsSelect`) which doctors already have a tour-plan entry for `@empId` on `@Date` — used to pre-tick already-planned doctors when building a new tour plan.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — same shape as `sp_Webapi_Get_DoctorList` plus `IsSelect`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorListDate]
	-- Add the parameters for the stored procedure here
    @empId INT,
	@Date nvarchar(max)
AS
    BEGIN
		    DECLARE @Q NVARCHAR(MAX)=''
	DECLARE @Param NVARCHAR(MAX)=''
	DECLARE @Id NVARCHAR(MAX)=0
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId

	SET @Q=' select DISTINCT 0 SubTerritoryId, case when  tblDtp.dtpDocId is null then 0 else 1 end  IsSelect, [MarketCode] MarketCode, [DoctorId]
      ,[DoctorCode]
      ,[DoctorName]
      ,[DocContact]
      ,[DoctorTypeName]
      ,isnull(ProgramTypeName,'''') ProgramTypeName
      ,[ChemberName],View_DoctorMaster.GroupId, View_DoctorMaster.RegionId,View_DoctorMaster.AreaId, View_DoctorMaster.TerritoryId,   View_DoctorMaster.MarketId ,   MarketName MarketName, SMCTypeId, SMCType, ProgramTypeId, DoctorTypeId from View_DoctorMaster     with (nolock)     
	  
	   left join (select  distinct dtp.DoctorId dtpDocId   from tbl_DoctorTourPlanDetail dtp  with (nolock) where dtp.EmpInfoId='+CAST(@empId as nvarchar(max))+' and CONVERT(date,dtp.TourPlanDate)=CONVERT(date,'''+CAST(@Date as nvarchar(max))+''')) tblDtp on  View_DoctorMaster.DoctorId=tblDtp.dtpDocId
	  
	  '

	--SELECT * FROM dbo.View_CustomerMaster WHERE GroupId
	 DECLARE @TRID NVARCHAR(MAX)
	IF(@Roletype='1')
	BEGIN
		SELECT @Id=STUFF(
    (SELECT ',' + CAST(TerritoryId AS VARCHAR)
     FROM dbo.tblMIOInfo 
     WHERE EmployeeId = @empId AND IsActive = 1
     FOR XML PATH('')), 
    1, 1, '');
	SET @Param=@Param+ ' WHERE  TerritoryId in ( '+ @Id +')';
	   -- SET @Param=@Param+' WHERE TerritoryId='+@Id
	END
	IF(@Roletype='2')
	BEGIN
	   	select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @Param=@Param+ ' WHERE  TerritoryId in ( '+ @TRID +')';
	END
	IF(@Roletype='3')
	BEGIN
	   SELECT   @Id=  STUFF( (SELECT CONCAT(',', mm.EmpRegionId , '')   FROM View_Webapi_EmployeeFieldForceInfo mm (NOLOCK)   WHERE EmpInfoId=@empId   FOR XML PATH ('') ),1,1,'')  
	   

	    SET @Param=@Param+' WHERE RegionId in ('+@Id+')'
	END
	IF(@Roletype='4')
	BEGIN
	    SELECT @Id=EmpGroupId FROM dbo.View_Webapi_EmployeeFieldForceInfo WHERE EmpInfoId=@empId

	    SET @Param=@Param+' WHERE GroupId='+@Id
	END

	SET @Q=@Q+@Param

    EXEC sys.sp_executesql @Q

	end
```

## sp_Webapi_Get_DoctorPlanByDate

- **Parameters**: `@cDate DATETIME`, `@empId INT`
- **Tables read**: `tbl_DoctorTourPlanMaster`, `tbl_DoctorTourPlanDetail`, `tblDoctorMaster`
- **Tables written**: none
- **Business logic**: Returns the doctors an employee has planned to visit on a specific date, restricted to approved tour plans where the DCR (call report) hasn't been completed yet (`IsDcrDone = 0`) — the "today's planned doctors still pending a visit report" list.
- **Transaction/error handling**: none
- **Result set**: `DocTPDetailsId`, `TpDate`, `DoctorCode`, `DoctorName`, `DoctorId`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorPlanByDate]
	-- Add the parameters for the stored procedure here
    @cDate DATETIME ,
    @empId INT
AS
    BEGIN
	

        SELECT  B.DocTPDetailsId ,
                CONVERT(NVARCHAR(50), B.TourPlanDate,106) AS TpDate,
				C.DoctorCode,
				C.DoctorName,
				C.DoctorId
        FROM    dbo.tbl_DoctorTourPlanMaster A
                INNER JOIN dbo.tbl_DoctorTourPlanDetail B ON B.DocTPMaster = A.DocTPMaster
                INNER JOIN dbo.tblDoctorMaster C ON C.DoctorId = B.DoctorId
        WHERE   A.ApprovalStatus = 'Approved'
                AND B.TourPlanDate = @cDate
                AND A.EmpInfoId = @empId
				AND B.IsDcrDone = 0


    END


```

## sp_Webapi_Get_DoctorProgramType

- **Parameters**: none
- **Tables read**: `tblProgramType`
- **Tables written**: none
- **Business logic**: Returns active program types (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `ProgramTypeId`, `ProgramType`
- **Caller**: UNUSED — no application caller found

```sql
 
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorProgramType] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		
		SELECT ProgramTypeId, ProgramTypeName ProgramType
			    FROM dbo.tblProgramType WHERE IsActive = 1


END


```

## sp_Webapi_Get_DoctorSpecialDay

- **Parameters**: none
- **Tables read**: `tblDoctorSpecialDay`
- **Tables written**: none
- **Business logic**: Returns active "special day" lookup values for doctors (a static picklist, e.g. weekly off day).
- **Transaction/error handling**: none
- **Result set**: `SpecialDayId`, `SpecialDay`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Webapi_Get_DoctorSpecialDay] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		
		SELECT SpecialDayId,SpecialDay FROM dbo.tblDoctorSpecialDay WHERE IsActive = 1
		 



END


```

## sp_Webapi_Get_DoctorSpeciality

- **Parameters**: none
- **Tables read**: `tblDoctorSpeciality`
- **Tables written**: none
- **Business logic**: Returns active doctor specialities (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `SpecialityId`, `SpecialityName`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorSpeciality] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		
		SELECT SpecialityId ,
               SpecialityName 
			    FROM dbo.tblDoctorSpeciality WHERE IsActive = 1


END


```

## sp_Webapi_Get_DoctorType

- **Parameters**: none
- **Tables read**: `tblDoctorType`
- **Tables written**: none
- **Business logic**: Returns active doctor types (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `DoctorTypeId`, `DoctorTypeName`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Webapi_Get_DoctorType] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		
		SELECT DoctorTypeId,DoctorTypeName
			    FROM dbo.tblDoctorType with (nolock) WHERE IsActive = 1


END


```

## sp_Webapi_Get_DoctorVisitPlanMasterData

- **Parameters**: `@empId INT = NULL`, `@month INT = NULL`, `@year INT = NULL`
- **Tables read**: `tbl_DoctorTourPlanMaster`
- **Tables written**: none
- **Business logic**: Returns the monthly doctor-visit tour-plan master record (submission/approval status and remarks) for an employee/month/year.
- **Transaction/error handling**: none
- **Result set**: `DocTPMaster`, `MonthValue`, `YearValue`, `EmpInfoId`, `IsFinalSubmit`, `ApprovalStatus`, `ApprovedBy`, `ApprovedDate`, `FinalSubmitRemarks`, `ApprovalRemarks`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctorVisitPlanMasterData]
	-- Add the parameters for the stored procedure here
@empId INT=null,@month INT=null,@year INT=null
AS
BEGIN
	
	SELECT DocTPMaster ,
           MonthValue ,
           YearValue ,
           EmpInfoId ,
           IsFinalSubmit ,
           ApprovalStatus ,
           ApprovedBy ,
           ApprovedDate ,
           FinalSubmitRemarks ,
           ApprovalRemarks FROM dbo.tbl_DoctorTourPlanMaster WHERE EmpInfoId = @empId AND MonthValue = @month AND YearValue = @year


END


```

## sp_Webapi_Get_DoctoreVisitTypeForDcr

- **Parameters**: none
- **Tables read**: `tbl_DoctorVisitType`
- **Tables written**: none
- **Business logic**: Returns active doctor-visit types for use on the DCR entry screen (a static lookup/picklist); contains a commented-out alternative that would have read `tbl_TourPlanType` instead.
- **Transaction/error handling**: none
- **Result set**: `TourTypeId`, `TourTypeName`
- **Caller**: UNUSED — no application caller found

```sql
 

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_DoctoreVisitTypeForDcr]
	-- Add the parameters for the stored procedure here

AS
BEGIN

	SELECT DocVisitTypeId AS TourTypeId ,
         VisitTypeName AS  TourTypeName  FROM tbl_DoctorVisitType WHERE IsActive =1


	--SELECT TourTypeId ,
 --          TourTypeName  FROM dbo.tbl_TourPlanType WHERE IsActive =1
END


```

## sp_Webapi_Get_ImagePath

- **Parameters**: `@type NVARCHAR(50)`
- **Tables read**: `tbl_ImagePath_Setting`
- **Tables written**: none
- **Business logic**: Returns the configured image storage path/prefix settings for a given image type (used to build photo URLs, e.g. attendance photos).
- **Transaction/error handling**: none
- **Result set**: `ImagePathId`, `ImageType`, `ImagePreName`, `IsActive`, `ImagePath`
- **Caller**: UNUSED — no application caller found

```sql
create PROCEDURE [dbo].[sp_Webapi_Get_ImagePath]
	-- Add the parameters for the stored procedure here
@type NVARCHAR(50)
AS
BEGIN
	SELECT ImagePathId ,
           ImageType ,
           ImagePreName ,
           IsActive ,
           ImagePath FROM dbo.tbl_ImagePath_Setting WHERE ImageType = @type
END
```

## sp_Webapi_Get_LocationTrackDetailsList

- **Parameters**: `@EmpMasterId NVARCHAR(MAX)`, `@date NVARCHAR(MAX)`
- **Tables read**: `tbl_UserTracking`, `tblEmpGeneralInfo`
- **Tables written**: none
- **Business logic**: Returns an employee's GPS location-tracking breadcrumb trail for a given date (address, time, active/break status, break reason, lat/long), most recent first.
- **Transaction/error handling**: none
- **Result set**: `date`, `address`, `time`, `status`, `break_reson`, `Lat`, `Long`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_LocationTrackDetailsList] 
 
 
 @EmpMasterId NVARCHAR(max),
		@date NVARCHAR(max)

AS
BEGIN
 
 
 select  FORMAT(urt.TrackDate ,'dd-MMM-yyyy')  [date], urt.AddressName [address] , urt.Time [time], case when urt.ActiveStatus=1 then 'Active' when urt.ActiveStatus=0 then 'Break'  else ''  end  [status],ISNULL(urt.breakReason,'')  break_reson, urt.LatValue Lat, urt.LongValue Long from tbl_UserTracking urt   with (nolock)
 inner join  tblEmpGeneralInfo  emp  with (nolock)  on urt.EmpInfoId=emp.EmpInfoId
 
  
  where    urt.EmpInfoId=@EmpMasterId and CONVERT(date,urt.TrackDate)=CONVERT(date,@date)	 

  order by urt.TrackDate desc

   
END

```

## sp_Webapi_Get_LocationTrackList

- **Parameters**: `@wings_id NVARCHAR(MAX)`, `@date NVARCHAR(MAX)`
- **Tables read**: `tblEmpGeneralInfo`, `tbl_UserTracking`
- **Tables written**: none
- **Business logic**: For every employee in a company, returns their first and last GPS tracking points for the given date (start/end time and start latitude/longitude) — a company-wide daily "first seen / last seen" location summary.
- **Transaction/error handling**: none
- **Result set**: `date`, `EmpMasterId`, `emp_id`, `name`, `s_time`, `e_time`, `S_Lat`, `S_Long`, `E_Lat` (always blank), `E_Long` (always blank)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_LocationTrackList] 

 
 @wings_id NVARCHAR(max),
		@date NVARCHAR(max)

AS
BEGIN

 

 

select distinct FORMAT(tcStart.TrackDate ,'dd-MMM-yyyy')  [date], emp.EmpInfoId EmpMasterId, emp.EmpMasterCode emp_id, emp.EmpName name,  tcStart.Time s_time, tcEnd.Time e_time ,  tcEnd.LatValue S_Lat, tcEnd.LongValue  S_Long, '' E_Lat,'' E_Long from tblEmpGeneralInfo  emp  with (nolock)
inner join (select min(TrackingId) TrackingId,   EmpInfoId from  tbl_UserTracking  with (nolock) where  CONVERT(Date, TrackDate)= cast(CONVERT(date,@date) as nvarchar(max))  group by  EmpInfoId)tblStart on emp.EmpInfoId=tblStart.EmpInfoId

left join (select max(TrackingId) TrackingIdEnd,   EmpInfoId from  tbl_UserTracking  with (nolock) where  CONVERT(Date, TrackDate)= cast(CONVERT(date,@date) as nvarchar(max))  group by  EmpInfoId)tblEnd on emp.EmpInfoId=tblEnd.EmpInfoId

inner join tbl_UserTracking tcStart  with (nolock) on tblStart.TrackingId=tcStart.TrackingId and tblStart.EmpInfoId=tcStart.EmpInfoId

left join tbl_UserTracking tcEnd  with (nolock) on tcEnd.TrackingId=tblEnd.TrackingIdEnd and tblEnd.EmpInfoId=tcEnd.EmpInfoId
 
  where   tblStart.EmpInfoId is not null and CONVERT(date,tcStart.TrackDate)=CONVERT(date,@date)	  AND emp.CompanyId = @wings_id

   
END

```

## sp_Webapi_Get_MarketByTerritoryId

- **Parameters**: `@territoryId INT = NULL`
- **Tables read**: `tblMarket`
- **Tables written**: none
- **Business logic**: Returns active markets belonging to a territory (picklist for cascading territory→market dropdowns).
- **Transaction/error handling**: none
- **Result set**: all columns of tblMarket
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_MarketByTerritoryId]
	-- Add the parameters for the stored procedure here
@territoryId INT = NULL
AS
BEGIN
		
		SELECT * FROM dbo.tblMarket WHERE TerritoryId = @territoryId AND IsActive = 1
		 

END
```

## sp_Webapi_Get_NonEffectiveReason

- **Parameters**: none
- **Tables read**: `tblNonEffectiveReason`
- **Tables written**: none
- **Business logic**: Returns active "non-effective visit" reason codes (a static lookup/picklist, e.g. why a planned doctor call wasn't completed).
- **Transaction/error handling**: none
- **Result set**: `ReasonId`, `ReasonName`
- **Caller**: UNUSED — no application caller found

```sql
 
create PROCEDURE [dbo].[sp_Webapi_Get_NonEffectiveReason] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		
		SELECT ReasonId, ReasonName  
			    FROM dbo.tblNonEffectiveReason WHERE IsActive = 1


END


```

## sp_Webapi_Get_PlannedDoctorList

- **Parameters**: `@empId INT`, `@date NVARCHAR(MAX)` (overwritten internally to `GETDATE()`, so the passed-in value is ignored)
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `View_DoctorMaster`, `tbl_DoctorTourPlanDetail`, `tbl_DCRInfo`, `tblASMInfo`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `View_Webapi_EmployeeFieldForceInfo`
- **Tables written**: none
- **Business logic**: Returns today's tour-plan doctor list scoped by role (MIO sees own plan; AM/team sees the whole team's planned doctors annotated with "Planned By: <planner>"), flagging each doctor with `isDCRDone` based on whether a DCR already exists today for that doctor/scope.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — `isDCRDone`, `SubTerritoryId`, `MarketCode` (or planner-annotated), doctor code/name/contact/type/program/chamber, hierarchy ids
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_PlannedDoctorList]
	-- Add the parameters for the stored procedure here
    @empId INT,
	@date nvarchar(max)
AS
    BEGIN
		    DECLARE @Q NVARCHAR(MAX)=''
	DECLARE @Param NVARCHAR(MAX)=''
	DECLARE @Id NVARCHAR(MAX)=0
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId
	set @date=GETDATE()

	IF(@Roletype='1')
	BEGIN

	SET @Q=' select DISTINCT case when tblDCrDone.DoctorId is null then 0 else 1 end  isDCRDone, 0 SubTerritoryId, View_DoctorMaster.MarketCode MarketCode, View_DoctorMaster.DoctorId 
      ,View_DoctorMaster.DoctorCode 
      ,View_DoctorMaster.DoctorName 
      ,View_DoctorMaster.DocContact 
      ,View_DoctorMaster.DoctorTypeName 
      ,isnull(View_DoctorMaster.ProgramTypeName,'''') ProgramTypeName
      ,View_DoctorMaster.ChemberName,View_DoctorMaster.GroupId, View_DoctorMaster.RegionId,View_DoctorMaster.AreaId, View_DoctorMaster.TerritoryId,   View_DoctorMaster.MarketId ,   View_DoctorMaster.MarketName MarketName, View_DoctorMaster.SMCTypeId, View_DoctorMaster.SMCType, View_DoctorMaster.ProgramTypeId, View_DoctorMaster.DoctorTypeId from View_DoctorMaster     with (nolock)   
	   INNER JOIN dbo.tbl_DoctorTourPlanDetail B   with (nolock)   ON B.DoctorId = View_DoctorMaster.DoctorId   and b.EmpInfoId='''+CAST(@empId as nvarchar(max))+'''

	   
	    left join ( select distinct  dcr.DoctorId  from tbl_DCRInfo dcr
		
		inner join tblUser us on us.UserId=dcr.EntryBy
		  where convert(Date,dcr.DcrDate)='''+CAST(convert( Date,@date ) as nvarchar(max))+''' and  us.EmpInfoId='''+CAST(@empId as nvarchar(max))+''' ) tblDCrDone ON tblDCrDone.DoctorId = View_DoctorMaster.DoctorId   


	    '
		end


		IF(@Roletype='2')
	BEGIN

		declare @comId nvarchar(max)
	select @comId=CompanyId from tblASMInfo where EmployeeId=@empId and IsActive=1
	declare @TRID_dd nvarchar(max)
	select @TRID_dd=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')
	SET @Q=' select DISTINCT case when tblDCrDone.DoctorId is null then 0 else 1 end  isDCRDone, 0 SubTerritoryId, View_DoctorMaster.MarketCode   +CHAR(10)+  CHAR(10)+''Planned By: ''+  CHAR(10)+ eb.EmpMasterCode+'', '' +eb.EmpName MarketCode, View_DoctorMaster.DoctorId 
      ,View_DoctorMaster.DoctorCode 
      ,View_DoctorMaster.DoctorName  DoctorName
      ,View_DoctorMaster.DocContact 
      ,View_DoctorMaster.DoctorTypeName 
      ,isnull(View_DoctorMaster.ProgramTypeName,'''') ProgramTypeName
      ,View_DoctorMaster.ChemberName,View_DoctorMaster.GroupId, View_DoctorMaster.RegionId,View_DoctorMaster.AreaId, View_DoctorMaster.TerritoryId,   View_DoctorMaster.MarketId ,   View_DoctorMaster.MarketName MarketName, View_DoctorMaster.SMCTypeId, View_DoctorMaster.SMCType, View_DoctorMaster.ProgramTypeId, View_DoctorMaster.DoctorTypeId from View_DoctorMaster     with (nolock)   
	   INNER JOIN dbo.tbl_DoctorTourPlanDetail B   with (nolock)   ON B.DoctorId = View_DoctorMaster.DoctorId   and b.TerritoryId in ('+CAST(@TRID_dd as nvarchar(max))+')  and  b.CompanyId in ('+CAST(@comId as nvarchar(max))+')

	   INNER JOIN dbo.tblEmpGeneralInfo eb  with (nolock)   ON eb.EmpInfoId = B.EmpInfoId 
	    left join ( select distinct  dcr.DoctorId  from tbl_DCRInfo dcr
		
		inner join tblUser us on us.UserId=dcr.EntryBy
		  where convert(Date,dcr.DcrDate)='''+CAST(convert( Date,@date ) as nvarchar(max))+''' and  dcr.TerritoryId in ('+CAST(@TRID_dd as nvarchar(max))+') ) tblDCrDone ON tblDCrDone.DoctorId = View_DoctorMaster.DoctorId   


	    '
		end

	--SELECT * FROM dbo.View_CustomerMaster WHERE GroupId
	 DECLARE @TRID NVARCHAR(MAX)
	IF(@Roletype='1')
	BEGIN
		SELECT @Id=STUFF(
    (SELECT ',' + CAST(TerritoryId AS VARCHAR)
     FROM dbo.tblMIOInfo 
     WHERE EmployeeId = @empId AND IsActive = 1
     FOR XML PATH('')), 
    1, 1, '');

	    SET @Param=@Param+' WHERE View_DoctorMaster.TerritoryId in  ( '+ @Id +')';
	END
	IF(@Roletype='2')
	BEGIN
	select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')
	    SET @Param=@Param+' WHERE View_DoctorMaster.TerritoryId in ('+@TRID+')'
	END
	IF(@Roletype='3')
	BEGIN
	   SELECT   @Id=  STUFF( (SELECT CONCAT(',', mm.EmpRegionId , '')   FROM View_Webapi_EmployeeFieldForceInfo mm (NOLOCK)   WHERE EmpInfoId=@empId   FOR XML PATH ('') ),1,1,'')  
	   

	    SET @Param=@Param+' WHERE View_DoctorMaster.RegionId in ('+@Id+')'
	END
	IF(@Roletype='4')
	BEGIN
	    SELECT @Id=EmpGroupId FROM dbo.View_Webapi_EmployeeFieldForceInfo WHERE EmpInfoId=@empId

	    SET @Param=@Param+' WHERE View_DoctorMaster.GroupId='+@Id
	END

	SET @Q=@Q+@Param +'  and convert(Date,B.TourPlanDate)='''+CAST(convert( Date,@date ) as nvarchar(max))+''''

    EXEC sys.sp_executesql @Q

	end
```

## sp_Webapi_Get_PrescriptionList

- **Parameters**: `@empId INT`, `@FromDt NVARCHAR(MAX) = NULL`, `@ToDt NVARCHAR(MAX) = NULL`, `@providertype NVARCHAR(MAX) = NULL`, `@pharmatype NVARCHAR(MAX) = NULL`, `@doctortype NVARCHAR(MAX) = NULL`, `@Role NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `tbl_PrescriptionMaster`, `tblDoctorMaster`
- **Tables written**: none
- **Business logic**: Returns a prescription-entry list filtered by date range, provider/pharma/doctor type, and territory (Team) or entering user, joined to employee and doctor names.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — `PrescriptionDate`, `ApprovalStatus`, `EmpName`, `DoctorName`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_PrescriptionList]
	-- Add the parameters for the stored procedure here
@empId int,
	
	@FromDt NVARCHAR(MAX) =NULL,
	@ToDt NVARCHAR(MAX) =NULL,
@providertype   NVARCHAR(MAX)= NULL,
@pharmatype   NVARCHAR(MAX)= NULL,
@doctortype   NVARCHAR(MAX)= NULL,
@Role   NVARCHAR(MAX)= NULL

AS
BEGIN
 
 DECLARE @UserId NVARCHAR(max),  @params NVARCHAR(max)=''

	 IF(@FromDt IS NOT NULL AND @ToDt IS NULL)
		BEGIN
		    SET @params=@params+ ' AND A.PrescriptionDate='''+@FromDt+''''
		END
		IF(@FromDt IS NOT NULL AND @ToDt IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND A.PrescriptionDate between '''+@FromDt+''' AND '''+@ToDt+''' '
		END


		IF(@providertype<>0 )
		BEGIN

		SET @params=@params+ ' AND  convert(Int,A.DoctorProgramypeId)='''+CAST(CONVERT(Int,@providertype) AS NVARCHAR(max))+''''
		    
		END

			IF(@pharmatype<>0 )
		BEGIN

		SET @params=@params+ ' AND  convert(Int,A.SmcTypeId_RX)='''+CAST(CONVERT(Int,@pharmatype) AS NVARCHAR(max))+''''
		    
		END

			IF(@doctortype<>0 )
		BEGIN

		SET @params=@params+ ' AND  convert(Int,M.Doctortypeid)='''+CAST(CONVERT(Int,@doctortype) AS NVARCHAR(max))+''''
		    
		END

		  DECLARE @TRID NVARCHAR(MAX);
		  SELECT @UserId=UserId FROM dbo.tblUser WHERE EmpInfoId=@empId

			  if(@Role = 'Team')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND A.TerritoryId in ( '+ @TRID +')';

			END
			else
			BEGIN
				 

				SET @params=@params+ ' and A.EntryBy='+@userId ;

			END



	

		DECLARE @Q NVARCHAR(MAX)
	SET @Q='
 SELECT FORMAT(A.EntryDate, ''MMMM dd, yyyy hh:mm tt'') PrescriptionDate,A.ApprovalStatus,  d.EmpMasterCode+'' - ''+D.EmpName EmpName , M.DoctorCode+'' - ''+M.DoctorName DoctorName FROM dbo.tbl_PrescriptionMaster A   with (NOLOCK)  
	LEFT JOIN dbo.tblUser us   with (NOLOCK)   ON us.UserId = A.EntryBy
                LEFT JOIN dbo.tblEmpGeneralInfo D   with (NOLOCK)   ON D.EmpInfoId = us.EmpInfoId
                LEFT JOIN dbo.tblDoctorMaster M   with (NOLOCK)   ON M.DoctorId = A.DoctorId
 WHERE A.DoctorId is not null '   +@params 

EXEC sys.sp_executesql @Q


    END
```

## sp_Webapi_Get_ProviderType

- **Parameters**: none
- **Tables read**: `tblProgramType`
- **Tables written**: none
- **Business logic**: Returns active program types re-labelled as "provider types" with boolean flags for whether each applies to customers and/or doctors (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `ProviderTypeId`, `ProviderType`, `forCustomer`, `forDoctor`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_ProviderType] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		
		SELECT d.ProgramTypeId ProviderTypeId, d.ProgramTypeName ProviderType,case when   d.IsCustomer=1 then 1 else 0 end forCustomer,   case when d.isDoctor=1 then 1 else 0 end forDoctor
			    FROM tblProgramType d with (nolock) WHERE IsActive = 1


END


```

## sp_Webapi_Get_RSMByWings

- **Parameters**: `@empId INT = NULL`, `@roletype NVARCHAR(MAX) = NULL`, `@fromdate NVARCHAR(MAX) = NULL`, `@todate NVARCHAR(MAX) = NULL`
- **Tables read**: `tblEmpGeneralInfo`, `tblASMInfo`, `tblTagASMRSMInfo`, `tblOrder`, `tblInvoice`, `tblInvoiceDetail`, `tblRSMInfo`, `tblCompanyUnit`
- **Tables written**: none
- **Business logic**: Sales-performance-by-manager report: for `@roletype = 'DZSM'`, lists the ASMs under the caller's company/area-tag scope with their invoiced amount/count for the date range; for `@roletype = 'Wings Head'`, lists RSMs company-wide with invoiced amount/count. Contains a large commented-out third branch for a DZSM-level rollup that is not active.
- **Transaction/error handling**: none
- **Result set**: `id`, `sn`, `empid`, `name`, `roletype`, `invAmount`, `invNumber` (columns vary slightly per branch)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_RSMByWings]
 @empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null


AS
BEGIN

		   declare @companyId NVARCHAR(50)=0 
select   @companyId=A.CompanyId  from   dbo.tblEmpGeneralInfo A  
		  where A.EmpInfoId=@empId
		 

		 if(@roletype ='DZSM')
		 begin

		 		declare @TRID nvarchar(max)

		 
			 

		   select B.ASMId  id,     ROW_NUMBER() OVER (ORDER BY A.EmpMasterCode) AS sn, A.EmpInfoId empid, A.EmpMasterCode+' : '+A.EmpName name, 'AM' roletype, isnull(tblOrd.invAmount,0) invAmount, isnull( tblOrd.invNumber,0) invNumber  from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId

		   left join ( SELECT    
    isnull(D.AreaId,0) AreaId  ,
      
     
    CONVERT(decimal(18, 2), (SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN INVD.DeliveryNetAmount ELSE 0 END)))) AS invAmount,
    COUNT(distinct inv.InvoiceId) AS invNumber
FROM
    dbo.tblOrder D WITH (NOLOCK)
INNER JOIN tblInvoice inv ON D.OrderId = inv.OrderId
LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
 
WHERE
    D.OrderId IS NOT NULL and   D.IsFromApp=1    AND D.ManufacId = ISNULL(@companyId,0) AND  (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(D.AreaId,0)    ) tblOrd on tblOrd.AreaId=b.AreaId

		 where B.CompanyId=@companyId and IsActive=1 and A.EmpInfoId<>1 and   B.AreaId in (   SELECT  mm.[TerritoryAreaId] FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5    )

		 end


	 if(UPPER(RTRIM(LTRIM(@roletype))) = UPPER(RTRIM(LTRIM('Wings Head'))) )
	 BEGIN


		SELECT DISTINCT 0 id,ROW_NUMBER() OVER (ORDER BY EGI.EmpMasterCode) AS sn,'DZSM' roletype,EGI.EmpMasterCode+' : '+EGI.EmpName name,
	    UNT.CompanyId,EGI.EmpInfoId AS empid,COUNT(InvoiceId) AS invNumber,SUM(DeliveryTpGrandTotal) AS invAmount FROM tblInvoice AS INV
	    LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId
	    LEFT JOIN tblRSMInfo AS RSM ON ODR.RSMId = RSM.RSMId
	    LEFT JOIN tblEmpGeneralInfo AS EGI ON RSM.EmployeeId = EGI.EmpInfoId
	    LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	    WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND UNT.CompanyId = ISNULL(@companyId,0) 
		AND InvoiceDate BETWEEN CONVERT(Date, @fromdate) AND CONVERT(Date,@todate)  
	    GROUP BY UNT.CompanyId,EGI.EmpInfoId,EGI.EmpMasterCode,EGI.EmpName
		

	 END
		 		 


END
 
 

 --select distinct 0  id,ROW_NUMBER() OVER (ORDER BY A.EmpMasterCode) AS sn, A.EmpInfoId empid, A.EmpMasterCode+' : '+A.EmpName name, 'DZSM' roletype, isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   from tblRSMInfo B
	--	INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
	--	left join ( SELECT ODR.RegionId,COUNT(InvoiceId) invNumber,SUM(ISNULL(INV.DeliveryTpGrandTotal,0)) invAmount FROM tblInvoice AS INV	--	LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId	--	LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId	--	where   ODR.ManufacId = ISNULL(@companyId,0) and  (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  ODR.RegionId) tblOrd on tblOrd.RegionId=b.RegionId
	--	where B.CompanyId=ISNULL(@companyId,0)  and isnull(tblOrd.invAmount,0) > 0
	--	group by A.EmpInfoId ,  A.EmpMasterCode,A.EmpName


 --Wings Head

 --SELECT    
 --   isnull(ODR.RegionId,0) RegionId  ,   count(distinct INV.InvoiceNo) invNumber, sum(INV.TpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV  --                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId --                            INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId --                            INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId --                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId --                            INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId	--						 where   ODR.ManufacId = ISNULL(@companyId,0) and  (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0)   






```

## sp_Webapi_Get_RSM_DZSMcheck

- **Parameters**: `@EmployeeId NVARCHAR(MAX) = NULL`
- **Tables read**: `tbluser`, `tbl_UserRoleInfo`, `View_Webapi_EmployeeFieldForceInfo`
- **Tables written**: none
- **Business logic**: Checks whether the given employee's assigned ASM manager and RSM manager (from the field-force hierarchy view) are the same person; if so returns those ids, otherwise returns the literal string `'nai'` for both (a "is this employee both ASM and RSM" sanity check).
- **Transaction/error handling**: none
- **Result set**: `ASMEMPId`, `RSMEMPId`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_RSM_DZSMcheck]
	-- Add the parameters for the stored procedure here
	 @EmployeeId NVARCHAR(MAX)= NULL
	AS
    BEGIN
  

 
 

 declare @RoleTypeId int 
 declare @ASMEMPId int 

 declare @RSMEMPId int 

	select @RoleTypeId= usr.RoleTypeId from tbluser us
	inner join tbl_UserRoleInfo usr ON usr.UserRoleID=us.UserRoleID
	 where EmpInfoId=@EmployeeId

	 
	SELECT distinct @ASMEMPId= ASMEMPId,@RSMEMPId= RSMEMPId FROM dbo.View_Webapi_EmployeeFieldForceInfo WHERE EmpInfoId=@EmployeeId
	 

	if(@ASMEMPId=@RSMEMPId )
	 begin
	  select @ASMEMPId  ASMEMPId, @RSMEMPId RSMEMPId
	 end
	 else 
	  begin
	 select  'nai' ASMEMPId,'nai' RSMEMPId
	 end
	 

	end
```

## sp_Webapi_Get_RptAttendanceList

- **Parameters**: `@wings_id NVARCHAR(MAX)`, `@date NVARCHAR(MAX)`
- **Tables read**: `tblMarketAttendance_Master_webapi`, `tblEmpGeneralInfo`, `tbl_UserTracking`
- **Tables written**: none
- **Business logic**: Company-wide daily attendance report: for each attendance row on the given date, computes a Present/Late/Absent status from the punch-in time band (05:00-09:19 Present, 09:20-09:39 Late, else Absent) and shows whether/when the employee last went on break.
- **Transaction/error handling**: none
- **Result set**: `name`, `w_starttime`, `t_breaktime`, `t_time`, `status`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_RptAttendanceList] 

 
 @wings_id NVARCHAR(max),
		@date NVARCHAR(max)

AS
BEGIN

 

 
 select   e.EmpMasterCode+' : '+e.EmpName name,  mas.PunchInTime w_starttime,ISNULL( case when tblTrck.t_time is not null then 'Yes' else 'N/A' end,'N/A') t_breaktime, ISNULL(FORMAT(tblTrck.t_time, 'hh:mm tt'),'') t_time,case when CONVERT(time,mas.PunchInTime )  between CONVERT(time,'05:00 am') and CONVERT(time,'09:19 am') then 'Present' when  CONVERT(time,mas.PunchInTime )  between CONVERT(time,'09:20 am') and CONVERT(time,'09:39 am')  then 'Late' else 'Absent'  end  [status] from tblMarketAttendance_Master_webapi   mas with (nolock)
 inner join tblEmpGeneralInfo e on e.EmpInfoId=mas.EmpInfoId
  left join (select ut.EmpInfoId, MAX(ut.TrackDate  ) t_time from  tbl_UserTracking ut where ut.ActiveStatus=0 group by ut.EmpInfoId)tblTrck on mas.EmpInfoId=tblTrck.EmpInfoId
  where    CONVERT(date,mas.AttendanceDate)=CONVERT(date,@date)	  AND mas.CompanyId = @wings_id


  order by mas.AttendanceDate desc

   
END

```

## sp_Webapi_Get_RptAttendanceListLate

- **Parameters**: `@wings_id NVARCHAR(MAX)`, `@date NVARCHAR(MAX)`
- **Tables read**: same as `sp_Webapi_Get_RptAttendanceList`
- **Tables written**: none
- **Business logic**: Same report as `sp_Webapi_Get_RptAttendanceList` but filtered to only Late/Absent punch-ins (punch-in time >= 09:20 am).
- **Transaction/error handling**: none
- **Result set**: `name`, `w_starttime`, `t_breaktime`, `t_time`, `status` (Late/Absent only)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_RptAttendanceListLate] 

 
 @wings_id NVARCHAR(max),
		@date NVARCHAR(max)

AS
BEGIN

 

 
 select    e.EmpMasterCode+' : '+e.EmpName name,  mas.PunchInTime w_starttime, ISNULL( case when tblTrck.t_time is not null then 'Yes' else 'N/A' end,'N/A') t_breaktime, ISNULL(FORMAT(tblTrck.t_time, 'hh:mm tt'),'') t_time, case when  CONVERT(time,mas.PunchInTime )  between CONVERT(time,'09:20 am') and CONVERT(time,'09:39 am')  then 'Late' when CONVERT(time,mas.PunchInTime )>= CONVERT(time,'09:40 am') then  'Absent' end [status] from tblMarketAttendance_Master_webapi   mas with (nolock)
 inner join tblEmpGeneralInfo e on e.EmpInfoId=mas.EmpInfoId
  left join (select ut.EmpInfoId, MAX(ut.TrackDate)    t_time from  tbl_UserTracking ut where ut.ActiveStatus=0 group by ut.EmpInfoId)tblTrck on mas.EmpInfoId=tblTrck.EmpInfoId
  where   CONVERT(date,mas.AttendanceDate)=CONVERT(date,@date)	  AND mas.CompanyId = @wings_id  
    
	 and    CONVERT(time,mas.PunchInTime )  >=CONVERT(time,'09:20 am')


  order by mas.AttendanceDate desc

 
   
END

```

## sp_Webapi_Get_RptAttendanceListPresent

- **Parameters**: `@wings_id NVARCHAR(MAX)`, `@date NVARCHAR(MAX)`
- **Tables read**: same as `sp_Webapi_Get_RptAttendanceList`
- **Tables written**: none
- **Business logic**: Same report filtered to only "Present" punch-ins (punch-in time between 05:00 and 09:19 am).
- **Transaction/error handling**: none
- **Result set**: `name`, `w_starttime`, `t_breaktime`, `t_time`, `status` (always `Present`)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_RptAttendanceListPresent] 

 
 @wings_id NVARCHAR(max),
		@date NVARCHAR(max)

AS
BEGIN

 

 
 select  e.EmpMasterCode+' : '+e.EmpName name,  mas.PunchInTime w_starttime, ISNULL( case when tblTrck.t_time is not null then 'Yes' else 'N/A' end,'N/A') t_breaktime, ISNULL(FORMAT(tblTrck.t_time, 'hh:mm tt'),'') t_time, 'Present'   [status] from tblMarketAttendance_Master_webapi   mas with (nolock)
 inner join tblEmpGeneralInfo e on e.EmpInfoId=mas.EmpInfoId

 left join (select ut.EmpInfoId, MAX(ut.TrackDate  ) t_time from  tbl_UserTracking ut where ut.ActiveStatus=0 group by ut.EmpInfoId)tblTrck on mas.EmpInfoId=tblTrck.EmpInfoId

  where    CONVERT(date,mas.AttendanceDate)=CONVERT(date,@date)	  AND mas.CompanyId = @wings_id
  and    CONVERT(time,mas.PunchInTime )  between CONVERT(time,'05:00 am') and CONVERT(time,'09:19 am')  


  order by mas.AttendanceDate desc

   
END

```

## sp_Webapi_Get_SMCType

- **Parameters**: none
- **Tables read**: `tblSMCType`
- **Tables written**: none
- **Business logic**: Returns a "Select" placeholder row unioned with active SMC types, each flagged for whether it applies to customers and/or doctors (a static dropdown picklist).
- **Transaction/error handling**: none
- **Result set**: `SMCTypeId`, `SMCType`, `forCustomer`, `forDoctor`
- **Caller**: UNUSED — no application caller found

```sql
 
CREATE PROCEDURE [dbo].[sp_Webapi_Get_SMCType] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		 
		 SELECT top 1 0 SMCTypeId,'Select' SMCType,     1 forCustomer,       1   forDoctor
			    FROM tblSMCType d 
				union all
		SELECT d.SMCTypeId SMCTypeId, d.SMCType SMCType,case when   d.forCustomer=1 then 1 else 0 end forCustomer,   case when d.forDotor=1 then 1 else 0 end forDoctor
			    FROM tblSMCType d with (nolock) WHERE IsActive = 1


END


```

## sp_Webapi_Get_Shift

- **Parameters**: `@empId INT = NULL` (unused in body)
- **Tables read**: `tbl_Shift`
- **Tables written**: none
- **Business logic**: Returns active shifts (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `ShiftId`, `ShiftText`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_Shift]
	-- Add the parameters for the stored procedure here
@empId INT = NULL
AS
BEGIN
		
		SELECT ShiftId ,
               ShiftText 
			    FROM dbo.tbl_Shift WHERE IsActive = 1
		 

END
```

## sp_Webapi_Get_Shift_AttendanceInfo

- **Parameters**: `@empId INT`
- **Tables read**: `dbo.GetAttendanceStatusWithShiftId(@pInTime)` (function), `tblMarketAttendance_Master_webapi`
- **Tables written**: none
- **Business logic**: Same punch-in/out button-state check as `sp_Webapi_Get_AttendanceInfo`, but without the "no active shift" fallback branch — if no shift resolves it returns no rows instead of forcing both buttons OFF.
- **Transaction/error handling**: none
- **Result set**: single row (if a shift is active) — `PunchInBtn`, `PunchOUTBtn`
- **Caller**: UNUSED — no application caller found

```sql

create PROCEDURE [dbo].[sp_Webapi_Get_Shift_AttendanceInfo]
 
 @empId int

 as
 
BEGIN
declare @pInTime time(0)=null , @ShiftId   int=0, @AttStatus nvarchar(max)=''
set @pInTime=FORMAT(getdate(),'hh:mm tt')
		select @ShiftId=ShiftId, @AttStatus= AttStatus from  dbo.GetAttendanceStatusWithShiftId(@pInTime);

	select CASE WHEN att1.PunchInTime  is not null THEN 'OFF' ELSE 'ON'   END PunchInBtn , CASE WHEN att1.PunchOutTime  is not null THEN 'OFF'  ELSE 'ON' END PunchOUTBtn FROM tblMarketAttendance_Master_webapi  att1
 

	WHERE att1.EmpInfoId = @empId AND CONVERT(DATE,att1.AttendanceDate) = CONVERT(DATE,GETDATE())    and  ShiftId=@ShiftId

END
```

## sp_Webapi_Get_SubMarketByMarketId

- **Parameters**: `@marketId INT = NULL`
- **Tables read**: `tbl_SubMarket`
- **Tables written**: none
- **Business logic**: Returns active sub-markets belonging to a market (picklist for cascading market→sub-market dropdowns).
- **Transaction/error handling**: none
- **Result set**: `SMId`, `SMCode`, `SMName`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_SubMarketByMarketId]
	-- Add the parameters for the stored procedure here
@marketId INT = NULL
AS
BEGIN
		
		SELECT SMId ,
               SMCode ,
               SMName 
			    FROM dbo.tbl_SubMarket WHERE MarketId = @marketId AND IsActive = 1
		 

END
```

## sp_Webapi_Get_TargetVsAcchivementData

- **Parameters**: `@empId INT`, `@month INT = NULL`, `@year INT = NULL`
- **Tables read**: `tblMIOInfo`, `tblEmpGeneralInfo`, `tblMIATargetProductWise`, `tblCompanyInfo`, `tblProduct`, `tblTerritory`, `tblArea`, `tblInvoice`, `tblInvoiceDetail`, `tblCompanyUnit`
- **Tables written**: none
- **Business logic**: Target-vs-achievement report for an MIO's month/year: unions (a) each targeted product with its delivered qty/value and computed achievement percentage, with (b) any additionally-sold products that had no target at all (shown with `TargetQty = 0`).
- **Transaction/error handling**: none
- **Result set**: `ProductCode`, `ProductName`, `Qty`, `Value`, `TargetQty`, `Achivment`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_TargetVsAcchivementData] -- [sp_Webapi_Get_TargetVsAcchivementData] 151,4,2021
	-- Add the parameters for the stored procedure here
    @empId INT ,
    @month INT = NULL ,
    @year INT = NULL
AS
    BEGIN

		-- Note:Target Quantity, Sales Quantity , Achivement(%)

        DECLARE @mioId INT ,
            @companyId INT

        SELECT  @mioId = MIOId
        FROM    dbo.tblMIOInfo
        WHERE   EmployeeId = @empId

        SELECT  @companyId = CompanyId
        FROM    dbo.tblEmpGeneralInfo
        WHERE   EmpInfoId = @empId
	

        SELECT  PD.ProductCode ,
                PD.ProductName ,
                ISNULL(SL.Qty, 0) Qty ,
                ISNULL(SL.Value, 0) Value ,
                ISNULL(TGRT.TargetQty, 0) TargetQty ,
                CASE WHEN TGRT.TargetQty IS NOT NULL
                     THEN ( ISNULL(SL.Qty, 0) * 100 ) / NULLIF(TGRT.TargetQty,
                                                              0)
                     ELSE 0
                END AS Achivment
        FROM    tblMIATargetProductWise AS TGRT
                LEFT JOIN tblCompanyInfo AS CI ON TGRT.CompanyId = CI.CompanyId
                LEFT JOIN tblProduct AS PD ON TGRT.ProductId = PD.ProductId
                LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId = TGRT.MiaId
                LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId
                LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId
                LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId
                LEFT JOIN ( SELECT  MiaId ,
                                    UNT.ComUnitCode ,
                                    UNT.ComUnitName ,
                                    ProductCode ,
                                    SUM(INVD.DeliveryNetAmount) AS Value ,
                                    ISNULL(SUM(DeliveryTotalQuantity), 0) AS Qty
                            FROM    tblInvoice AS INV
                                    INNER JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                                    INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
                            WHERE   DeliveryInvoiceStatus IN ( 'Full',
                                                              'Partial' )
                                    AND YEAR(INV.InvoiceDate) = @year
                                    AND MONTH(INV.InvoiceDate) = @month
                                    AND UNT.CompanyId = @companyId
                                    AND INV.MiaId = @mioId
                            GROUP BY UNT.ComUnitCode ,
                                    UNT.ComUnitName ,
                                    ProductCode ,
                                    MiaId
                          ) AS SL ON SL.ProductCode = PD.ProductCode
                                     AND TGRT.MiaId = SL.MiaId
        WHERE   TGRT.Year = @year
                AND ( TGRT.Period = DATENAME(MONTH,
                                             DATEADD(MONTH, @month, 0) - 1) )
                AND CI.CompanyId = @companyId
                AND TGRT.MiaId = @mioId
        UNION ALL
        SELECT  INVD.ProductCode ,
                INVD.ProductName ,
                ISNULL(SUM(DeliveryTotalQuantity), 0) Qty ,
                SUM(INVD.DeliveryNetAmount) Value ,
                0 TargetQty ,
                0 AS Achivment
        FROM    tblInvoice AS INV
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                LEFT JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
                LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId = INV.MiaId
                LEFT JOIN dbo.tblEmpGeneralInfo ON tblMIOInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId
                LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId
                LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId
        WHERE   DeliveryInvoiceStatus IN ( 'Full', 'Partial' )
                AND YEAR(INV.InvoiceDate) = @year
                AND MONTH(INV.InvoiceDate) = @month
                AND UNT.CompanyId = @companyId
                AND INV.MiaId = @mioId
                AND INVD.ProductCode NOT IN (
                SELECT  PD.ProductCode
                FROM    tblMIATargetProductWise AS TGRT
                        LEFT JOIN tblProduct AS PD ON TGRT.ProductId = PD.ProductId
                WHERE   Year = @year
                        AND Period = DATENAME(MONTH,
                                              DATEADD(MONTH, @month, 0) - 1)
                        AND CI.CompanyId = @companyId
                        AND TGRT.MiaId = @mioId )
        GROUP BY INVD.ProductCode ,
                INVD.ProductName 
      



    END

```

## sp_Webapi_Get_TodaysTask

- **Parameters**: `@empId INT`
- **Tables read**: `tbl_TourPlanInfo`, `tbl_TourPlanMaster`, `tblMarket`, `tblTerritory`, `tbl_TourPlanPurpose`, `tblCustMaster`, `tbl_DoctorTourPlanDetail`, `tbl_DoctorTourPlanMaster`, `tblDoctorMaster`, `tbl_DWSPMaster`, `tbl_DWSPDetail`
- **Tables written**: none
- **Business logic**: Builds the mobile app's "today's task" list for an employee: today's approved customer-visit tour plan items, today's approved doctor-visit tour plan items, and today's DWSP (a promotional-spend record) entries — all unioned into one worklist tagged by `TourType` (`mtp`/`dtp`/`DWSP`).
- **Transaction/error handling**: none
- **Result set**: `MarketName`, `TerritoryName`, `SMName`, `TPName`, `IsMarketWise`, `MName`, `TourType`, `Id`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_TodaysTask]
	-- Add the parameters for the stored procedure here
    
    @empId INT
AS
    BEGIN



		SELECT  B.MarketName ,
        C.TerritoryName ,
       tp.TPName  SMName ,
      'Customer visit' TPName ,
        A.IsMarketWise ,
        ( cus.CustomerCode + ' : ' + cus.CustomerName ) AS MName ,
        'mtp' AS TourType,A.TourPlanId Id
FROM    dbo.tbl_TourPlanInfo A
        inner JOIN dbo.tbl_TourPlanMaster mas ON mas.TPMaster = A.TPMaster
 
        LEFT JOIN dbo.tblMarket B ON B.MarketId = A.MarketId
        LEFT JOIN dbo.tblTerritory C ON C.TerritoryId = A.TerritoryId
    
      
        LEFT JOIN dbo.tbl_TourPlanPurpose tp ON tp.TPId = A.TPId
        LEFT JOIN dbo.tblCustMaster cus ON cus.CustomerMasterId = A.CustomerMasterId
WHERE   A.EmpInfoId = @empId
        AND CONVERT(Date, A.TourPlanDate) = CONVERT(Date,GETDATE() ) 
        AND mas.ApprovalStatus = '2' 
UNION ALL
SELECT  '' AS MarketName ,
        '' AS TerritoryName ,
        tp.TPName AS SMName ,
        'Doctor Visit' AS TPName ,
		  0 AS IsMarketWise ,
        ( B.DoctorCode + ' : ' + B.DoctorName ) AS MName ,
        'dtp' AS TourType, A.DocTPDetailsId Id
FROM    dbo.tbl_DoctorTourPlanDetail A
        inner JOIN dbo.tbl_DoctorTourPlanMaster mas ON mas.DocTPMaster = A.DocTPMaster

        LEFT JOIN dbo.tblDoctorMaster B ON B.DoctorId = A.DoctorId
        LEFT JOIN dbo.tbl_TourPlanPurpose tp ON tp.TPId = A.TPId

WHERE   A.EmpInfoId = @empId
        AND CONVERT(Date,A.TourPlanDate) =CONVERT(Date,GETDATE() )
        
	AND mas.ApprovalStatus = '2' 
	 



	 UNION ALL
SELECT  '' AS MarketName ,
        '' AS TerritoryName ,
        '' AS SMName ,
        'DWSP Reccord' AS TPName ,
		  0 AS IsMarketWise ,
           'Campaign'+' :'+ cast(ISNULL(dtl.CampaignAmount,0) as nvarchar(max)) +', ' +'General'+' :'+ cast(ISNULL(dtl.GeneralAmount,0) as nvarchar(max)) +', '   +'FCB'+' :'+ cast(ISNULL(dtl.FCBAmount,0) as nvarchar(max))  MName ,
        'DWSP' AS TourType, 0 Id
 

	  from tbl_DWSPMaster mas  with (nolock)
left join tbl_DWSPDetail dtl   with (nolock) on mas.DWSPMasterId=dtl.DWSPMasterId
where  CONVERT(Date,dtl.DWSPDate)=CONVERT(Date,GETDATE() ) and mas.EmpInfoId=@empId  
	 
	   and  mas.ApprovalStatus = '2' 

    END


```

## sp_Webapi_Get_TourPlanInfo

- **Parameters**: `@month INT = NULL`, `@year INT = NULL`, `@empId INT = NULL`
- **Tables read**: `tbl_TourPlanInfo`, `tblMarket`, `tblCustMaster`, `tbl_Shift`, `tbl_TourPlanType`, `tbl_TourPlanPurpose`, `tbl_TourPlanMaster`
- **Tables written**: none
- **Business logic**: Returns an employee's customer/market tour-plan entries for a given month/year, joined to market, customer, shift, tour type and purpose names, plus the plan master's final-submit flag.
- **Transaction/error handling**: none
- **Result set**: `TourPlanId`, `Comment`, `IsMarketWise`, `IsApproved`, `MarketName`, `CustomerName`, `ShiftText`, `TourTypeName`, `TPName`, `MonthValue`, `YearValue`, `DayValue`, `TourPlanDate`, `IsFinalSubmit`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_TourPlanInfo] -- sp_Webapi_Get_TourPlanInfo 2,2021,0
	-- Add the parameters for the stored procedure here
@month INT = NULL,
@year INT = NULL,
@empId INT = NULL
AS
BEGIN
	
	SELECT A.TourPlanId ,    
           A.Comment ,
           A.IsMarketWise ,
           A.IsApproved,
		   B.MarketName,
		   (C.CustomerCode + ' : '+C.CustomerName)AS CustomerName,
		   D.ShiftText,
		   E.TourTypeName,
		   F.TPName,
		   MONTH(A.TourPlanDate) AS MonthValue,
		   YEAR(A.TourPlanDate) AS YearValue,
		   DAY(A.TourPlanDate) AS DayValue,
		   	Convert(varchar(10), A.TourPlanDate,120) AS TourPlanDate,
			tpM.IsFinalSubmit
			
		    
		    FROM dbo.tbl_TourPlanInfo A
			LEFT JOIN dbo.tblMarket B ON B.MarketId = A.MarketId
			LEFT JOIN dbo.tblCustMaster C ON C.CustomerMasterId = A.CustomerMasterId
			LEFT JOIN dbo.tbl_Shift D ON D.ShiftId = A.ShiftId
			LEFT JOIN dbo.tbl_TourPlanType E ON E.TourTypeId = A.TourTypeId
			LEFT JOIN dbo.tbl_TourPlanPurpose F ON F.TPId = A.TPId
			INNER JOIN dbo.tbl_TourPlanMaster tpM ON tpM.TPMaster = A.TPMaster
			WHERE A.EmpInfoId = @empId AND MONTH(A.TourPlanDate) = @month AND YEAR(A.TourPlanDate) = @year
END
```

## sp_Webapi_Get_TourPlanPurpose

- **Parameters**: `@empId INT = NULL` (unused in body)
- **Tables read**: `tbl_TourPlanPurpose`
- **Tables written**: none
- **Business logic**: Returns active tour-plan purposes (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `TPId`, `TPName`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_TourPlanPurpose]
	-- Add the parameters for the stored procedure here
@empId INT = NULL
AS
BEGIN
		
	SELECT TPId ,
           TPName 
		   FROM dbo.tbl_TourPlanPurpose WHERE IsActive =1
		 

END
```

## sp_Webapi_Get_TourPlanType

- **Parameters**: `@empId INT = NULL` (unused in body)
- **Tables read**: `tbl_TourPlanType`
- **Tables written**: none
- **Business logic**: Returns active tour-plan types (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `TourTypeId`, `TourTypeName`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_TourPlanType]
	-- Add the parameters for the stored procedure here
@empId INT = NULL
AS
BEGIN
		
		SELECT TourTypeId ,
               TourTypeName 
			    FROM dbo.tbl_TourPlanType WHERE IsActive = 1
		 

END
```

## sp_Webapi_Get_TourTourPlanDetails

- **Parameters**: `@month INT = NULL`, `@year INT = NULL`, `@empId INT = NULL`
- **Tables read**: `tbl_DoctorTourPlanMaster`, `tbl_DoctorTourPlanDetail`, `tblDoctorMaster`
- **Tables written**: none
- **Business logic**: Returns an employee's doctor tour-plan detail rows for a given month/year, joined to doctor code/name.
- **Transaction/error handling**: none
- **Result set**: `DocTPDetailsId`, `DoctorName`, `MonthValue`, `YearValue`, `DayValue`, `TourPlanDate`, `IsFinalSubmit`
- **Caller**: UNUSED — no application caller found

```sql
 
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_TourTourPlanDetails]
	-- Add the parameters for the stored procedure here
@month INT = NULL,
@year INT = NULL,
@empId INT = NULL
AS
BEGIN
		
		SELECT  B.DocTPDetailsId,
		(C.DoctorCode+':'+C.DoctorName)AS DoctorName,
		B.DocTPDetailsId,
				   MONTH(B.TourPlanDate) AS MonthValue,
		   YEAR(B.TourPlanDate) AS YearValue,
		   DAY(b.TourPlanDate) AS DayValue,
		   	Convert(varchar(10), b.TourPlanDate,120) AS TourPlanDate,
			A.IsFinalSubmit
		
		 FROM dbo.tbl_DoctorTourPlanMaster A 
		INNER JOIN dbo.tbl_DoctorTourPlanDetail B ON B.DocTPMaster = A.DocTPMaster
		LEFT JOIN dbo.tblDoctorMaster C ON C.DoctorId = B.DoctorId
		WHERE A.EmpInfoId = @empId AND MONTH(B.TourPlanDate) = @month AND YEAR(B.TourPlanDate) = @year

END


```

## sp_Webapi_Get_UserByRoleId

- **Parameters**: `@id INT`, `@empId INT`
- **Tables read**: `tblUser`, `tblEmpGeneralInfo`
- **Tables written**: none
- **Business logic**: Returns employees assigned a given user role, excluding the calling employee (e.g. building an "assign to" picklist of peers/subordinates for approvals).
- **Transaction/error handling**: none
- **Result set**: `EmpInfoId`, `EmpName`, `EmpMasterCode`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_UserByRoleId]
	-- Add the parameters for the stored procedure here
	@id INT,
	@empId int
AS
BEGIN


		SELECT B.EmpInfoId,B.EmpName,B.EmpMasterCode FROM dbo.tblUser A 
		INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
		WHERE A.UserRoleID = @id AND A.EmpInfoId !=@empId



END


```

## sp_Webapi_Get_Userrole

- **Parameters**: none
- **Tables read**: `tbl_UserRoleInfo`
- **Tables written**: none
- **Business logic**: Returns a fixed, hardcoded subset of user roles (ids 1, 3, 5, 7 and 17) — a role picklist restricted to specific field-force roles.
- **Transaction/error handling**: none
- **Result set**: all columns of tbl_UserRoleInfo
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_Userrole]
	-- Add the parameters for the stored procedure here

AS
BEGIN

SELECT * FROM dbo.tbl_UserRoleInfo  with (nolock)
WHERE   UserRoleID in (1,3,5,7)
union all SELECT * FROM dbo.tbl_UserRoleInfo   with (nolock)
WHERE UserRoleID IN ('17')


--select * from tblRoleType

END
```

## sp_Webapi_Get_VisitTypeForDcr

- **Parameters**: none
- **Tables read**: `tbl_TourPlanType`
- **Tables written**: none
- **Business logic**: Returns active tour/visit types for use on the DCR screen — identical query to `sp_Webapi_Get_TourPlanType`.
- **Transaction/error handling**: none
- **Result set**: `TourTypeId`, `TourTypeName`
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Get_VisitTypeForDcr]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	SELECT TourTypeId ,
           TourTypeName  FROM dbo.tbl_TourPlanType WHERE IsActive =1
END


```

## sp_Webapi_Get_Wings

- **Parameters**: none
- **Tables read**: `tblCompanyInfo`
- **Tables written**: none
- **Business logic**: Returns all companies ("wings"/manufacturers) ordered by name (a static lookup/picklist).
- **Transaction/error handling**: none
- **Result set**: `WingsId`, `WingsName`
- **Caller**: UNUSED — no application caller found

```sql

create PROCEDURE [dbo].[sp_Webapi_Get_Wings] 

 


AS
BEGIN

 select CompanyId WingsId, CompanyName WingsName from tblCompanyInfo with (nolock)
 order by CompanyName asc
   
END

```

## sp_Webapi_Get_cvSelectedReportDtl

- **Parameters**: `@DocTPDetailsId NVARCHAR(MAX)` (used as an employee id)
- **Tables read**: `tbl_DCRInfo`, `tblDoctorMaster`, `tblUser`, `tbl_DoctorTourPlanDetail`, `tbl_DCRCustomerInfo`, `tblCustMaster`, `tbl_TourPlanInfo`
- **Tables written**: none
- **Business logic**: For today's date, returns a combined "client visit report" detail list for an employee: completed doctor DCR visits, planned-but-not-yet-visited doctors, completed customer DCR visits, and planned-but-not-yet-visited customers — each row prefixed `D-`/`P-` and labeled with time/address/remarks/location.
- **Transaction/error handling**: none
- **Result set**: `emp_id`, `client_name`, `time`, `fromt`, `remarks`, `location`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Get_cvSelectedReportDtl]
   @DocTPDetailsId nvarchar(max)
    --, @DcrDate nvarchar(max)

AS
BEGIN
declare @DcrDate nvarchar(max)
set @DcrDate=GETDATE()
 select * from (

select  distinct us.EmpInfoId  emp_id, 'D- '+ mas.DoctorCode +' : '+ mas.DoctorName client_name, FORMAT(dcr.EntryDate, 'hh:mm:ss tt')   time,dcr.StreetAddress fromt,  dcr.Remarks remarks,dcr.Latitude location   from tbl_DCRInfo dcr with (nolock)
inner join tblDoctorMaster mas  with (nolock) on mas.DoctorId=dcr.DoctorId
inner join tblUser us  with (nolock) on us.UserId=dcr.EntryBy

where us.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)


union all

select dcr.EmpInfoId emp_id, 'D- '+ mas.DoctorCode +' : '+ mas.DoctorName client_name, FORMAT(dcr.TourPlanDate, 'hh:mm:ss tt')   time, '' fromt,  '' remarks,'' location   from tbl_DoctorTourPlanDetail dcr with (nolock)
inner join tblDoctorMaster mas  with (nolock) on mas.DoctorId=dcr.DoctorId
 

where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.TourPlanDate)=CONVERT(Date, @DcrDate) --and (dcr.IsDcrDone is null  or dcr.IsDcrDone=0)
 and dcr.DoctorId not in (select dcr.DoctorId  from tbl_DCRInfo dcr with (nolock)
 
inner join tblUser us  with (nolock) on us.UserId=dcr.EntryBy

where us.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate))


 


 union all

 select dcr.EmpInfoId  emp_id, 'P- '+ mas.CustomerCode +' : '+ mas.CustomerName client_name, FORMAT(dcr.EntryDate, 'hh:mm:ss tt')   time,dcr.StreetAddress fromt,  dcr.Remarks remarks,dcr.Latitude location from tbl_DCRCustomerInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerId


where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)


union all

select dcr.EmpInfoId  emp_id, 'P- '+ mas.CustomerCode +' : '+ mas.CustomerName client_name, FORMAT(dcr.TourPlanDate, 'hh:mm:ss tt')   time,'' fromt,  '' remarks,'' location from tbl_TourPlanInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerMasterId
 

where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.TourPlanDate)=CONVERT(Date, @DcrDate) --and (dcr.IsDcrDone is null  or dcr.IsDcrDone=0)
 and dcr.CustomerMasterId not in (select distinct dcr.CustomerId  from tbl_DCRCustomerInfo dcr with (nolock)
 
 

where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)))tbl


 
 
END
 

```

## sp_Webapi_Get_cvSelectedReportDtl_new

- **Parameters**: `@DocTPDetailsId NVARCHAR(MAX)`, `@DcrDate NVARCHAR(MAX)`
- **Tables read**: same as `sp_Webapi_Get_cvSelectedReportDtl`
- **Tables written**: none
- **Business logic**: Newer version of `sp_Webapi_Get_cvSelectedReportDtl` that takes an explicit date parameter (instead of always today), splits the combined "lat,long" string into separate `Lat`/`Long` columns, and adds a `Yes`/`No` visited-status flag per row.
- **Transaction/error handling**: none
- **Result set**: `emp_id`, `client_name`, `time`, `fromt`, `remarks`, `Lat`, `Long`, `status`
- **Caller**: UNUSED — no application caller found

```sql

CREATE

 PROCEDURE [dbo].[sp_Webapi_Get_cvSelectedReportDtl_new]
   @DocTPDetailsId nvarchar(max)
    , @DcrDate nvarchar(max)

AS
BEGIN
 
 select distinct * from (

select  distinct us.EmpInfoId  emp_id, 'D- '+ mas.DoctorCode +' : '+ mas.DoctorName client_name, FORMAT(dcr.EntryDate, 'hh:mm:ss tt')   time,dcr.StreetAddress fromt,  dcr.Remarks remarks,SUBSTRING(dcr.Latitude, 1, CHARINDEX(',', dcr.Latitude + ',') - 1) AS Lat, 
    CASE WHEN CHARINDEX(',', dcr.Latitude + ',') = 0 THEN '' ELSE SUBSTRING(dcr.Latitude, CHARINDEX(',', dcr.Latitude) + 1, LEN(dcr.Latitude) - CHARINDEX(',', dcr.Latitude)) END AS Long, 'Yes' status   from tbl_DCRInfo dcr with (nolock)
inner join tblDoctorMaster mas  with (nolock) on mas.DoctorId=dcr.DoctorId
inner join tblUser us  with (nolock) on us.UserId=dcr.EntryBy

where us.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)


union all

select dcr.EmpInfoId emp_id, 'D- '+ mas.DoctorCode +' : '+ mas.DoctorName client_name, FORMAT(dcr.TourPlanDate, 'hh:mm:ss tt')   time, '' fromt,  '' remarks,'' Lat,'' Long,  'No' status     from tbl_DoctorTourPlanDetail dcr with (nolock)
inner join tblDoctorMaster mas  with (nolock) on mas.DoctorId=dcr.DoctorId
 

where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.TourPlanDate)=CONVERT(Date, @DcrDate) --and (dcr.IsDcrDone is null  or dcr.IsDcrDone=0)
 and dcr.DoctorId not in (select dcr.DoctorId  from tbl_DCRInfo dcr with (nolock)
 
inner join tblUser us  with (nolock) on us.UserId=dcr.EntryBy

where us.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate))


 


 union all

 select dcr.EmpInfoId  emp_id, 'P- '+ mas.CustomerCode +' : '+ mas.CustomerName client_name, FORMAT(dcr.EntryDate, 'hh:mm:ss tt')   time,dcr.StreetAddress fromt,  dcr.Remarks remarks,dcr.Latitude Lat,  dcr.Longitude Long,  'Yes' status   from tbl_DCRCustomerInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerId


where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)


union all

select dcr.EmpInfoId  emp_id, 'P- '+ mas.CustomerCode +' : '+ mas.CustomerName client_name, FORMAT(dcr.TourPlanDate, 'hh:mm:ss tt')   time,'' fromt,  '' remarks, '' Lat,'' Long, 'No' status   from tbl_TourPlanInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerMasterId
 

where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.TourPlanDate)=CONVERT(Date, @DcrDate) --and (dcr.IsDcrDone is null  or dcr.IsDcrDone=0)
 and dcr.CustomerMasterId not in (select distinct dcr.CustomerId  from tbl_DCRCustomerInfo dcr with (nolock)
 
 

where dcr.EmpInfoId=@DocTPDetailsId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)))tbl


 
 
END
 

```

## sp_Webapi_Get_cvSixtyDaysReportDtl

- **Parameters**: `@emp_Id NVARCHAR(MAX)`
- **Tables read**: `tbl_DCRInfo`, `tblUser`, `tblEmpGeneralInfo`, `tbl_UserRoleInfo`, `tblRoleType`, `tbl_DCRCustomerInfo`, `tblCustMaster`
- **Tables written**: none
- **Business logic**: Returns an employee's doctor-DCR and customer-DCR visit history for the trailing 60 days, splitting the combined lat/long string for doctor visits.
- **Transaction/error handling**: none
- **Result set**: `date`, `visit_time`, `v_from`, `Lat`, `Long`, `EmpInfoId`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Get_cvSixtyDaysReportDtl]
   @emp_Id nvarchar(max) 

AS
BEGIN

 select distinct * from (

select  distinct FORMAT(dtl.DcrDate, 'dd-MMM-yy')  date, FORMAT(dtl.DcrDate, 'hh:mm tt')  visit_time,dtl.StreetAddress v_from, SUBSTRING(dtl.Latitude, 1, CHARINDEX(',', dtl.Latitude + ',') - 1) AS Lat, 
    CASE WHEN CHARINDEX(',', dtl.Latitude + ',') = 0 THEN '' ELSE SUBSTRING(dtl.Latitude, CHARINDEX(',', dtl.Latitude) + 1, LEN(dtl.Latitude) - CHARINDEX(',', dtl.Latitude)) END AS Long, emp.EmpInfoId EmpInfoId   from tbl_DCRInfo  dtl  with (nolock)
 	LEFT JOIN dbo.tblUser u ON u.UserId = dtl.EntryBy
	LEFT JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = u.EmpInfoId

	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = u.UserRoleID
				LEFT JOIN dbo.tblRoleType rt ON rt.RoleTypeId =tbl_UserRoleInfo.RoleTypeId




 
  
 
where emp.EmpInfoId=@emp_Id  and convert(date, dtl.DcrDate)  >= DATEADD(day, -60, GETDATE())


union all select  FORMAT(dcr.DcrDate, 'dd-MMM-yy')  date, FORMAT(dcr.DcrDate, 'hh:mm tt')  visit_time,+dcr.StreetAddress v_from,dcr.Latitude Lat,   dcr.Longitude Long, dcr.EmpInfoId EmpInfoId  from tbl_DCRCustomerInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerId


where dcr.EmpInfoId=@emp_Id  and convert(date, dcr.DcrDate)  >= DATEADD(day, -60, GETDATE())
)tbl
 
 
 
END
 


```

## sp_Webapi_Save_CustomerDcrInfo

- **Parameters**: `@CustomerId INT`, `@sessionUser NVARCHAR(50)`, `@remarks NVARCHAR(50)`, `@DcrDate DATETIME = NULL`, `@Latitude NVARCHAR(MAX) = NULL`, `@Longitude NVARCHAR(MAX) = NULL`, `@StreetAddress NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tbl_DCRCustomerInfo` (duplicate check), `ZAS_UserFS_MIO`/`ZAS_UserFS_AM`/`ZAS_UserFS_RSM`, `tblCustMaster`, `tblMarket`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`
- **Tables written**: `tbl_DCRCustomerInfo` (INSERT)
- **Business logic**: Records a customer DCR (call report) visit — first checks for an existing same-day entry for this employee/customer to avoid duplicates, then resolves the caller's company (by role: MIO/AM/DZSM) and the customer's full org hierarchy (group/region/area/territory/market, including denormalized names/codes), and inserts one `tbl_DCRCustomerInfo` row with `ApprovalStatus = '2'`. A large trailing block that would post an approval-log entry via `sp_webapi_SaveDCRAppLog` is entirely commented out (dead code). Note: the procedure body has an extra, unmatched trailing `END` beyond the one closing the main `BEGIN`.
- **Transaction/error handling**: none (no explicit transaction; relies on the pre-INSERT duplicate-count check for idempotency, not a constraint)
- **Result set**: single scalar — the new `tbl_DCRCustomerInfo` identity (`@Id`), only when the duplicate check passes; no result set at all if a same-day entry already exists
- **Caller**: UNUSED — no application caller found

```sql
create PROCEDURE [dbo].[sp_Webapi_Save_CustomerDcrInfo]
	-- Add the parameters for the stored procedure here
@CustomerId INT ,
 
@sessionUser NVARCHAR(50),
@remarks NVARCHAR(50),
 
 @DcrDate datetime=null,

    @Latitude NVARCHAR(MAX) = NULL ,
    @Longitude NVARCHAR(MAX) = NULL ,

	@StreetAddress NVARCHAR(MAX) = NULL
AS
BEGIN



declare @Count int=0
DECLARE @Roltp nvarchar(max)
	DECLARE @userId nvarchar(max)
SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @sessionUser
select @Count= ISNULL(count(*),0) from tbl_DCRCustomerInfo  where  CONVERT(Date,dcrDate)=CONVERT(Date,@dcrDate)  and EmpInfoId= @sessionUser and CustomerId=@CustomerId

	
	if(@Count=0)
	begin
 
	declare @CompanyId int=null

	


	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@sessionUser



				if(@Roltp='MIO')
				begin
				 select  @CompanyId=CompanyId  from 	ZAS_UserFS_MIO  where EmpInfoId= @sessionUser
				end


				if(@Roltp='MIO')
				begin
				 select  @CompanyId=CompanyId   from 	ZAS_UserFS_MIO  where EmpInfoId= @sessionUser
				end

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_AM  where EmpInfoId= @sessionUser
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_RSM  where EmpInfoId= @sessionUser
		end


		 
	DECLARE    @GroupId INT,@RegionId INT,@AreaId INT, @TerritoryId INT,@SubTerritoryId INT,@marketId INT
		SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @sessionUser


			declare @GroupName nvarchar(max),@RegionName nvarchar(max),@AreaName nvarchar(max),@TerritoryName nvarchar(max),@SubTerritoryName nvarchar(max),@MarketName nvarchar(max), @GroupCode_ord nvarchar(max),@RegionCode_ord nvarchar(max),@AreaCode_ord nvarchar(max),@TerritoryCode_ord nvarchar(max),@SubTerritoryCode_ord nvarchar(max),@MarketCode_ord nvarchar(max)



		select   @GroupId=gr.GroupId, @RegionId=rg.RegionId, @AreaId=Ar.AreaId, @TerritoryId=Tr.TerritoryId, @marketId=cus.MarketId,@GroupName=gr.GroupName, @RegionName=rg.RegionName, @AreaName=Ar.AreaName,@TerritoryName=Tr.TerritoryName ,  @MarketName=mr.MarketName ,@GroupCode_ord=gr.GroupCode, @RegionCode_ord=rg.RegionCode, @AreaCode_ord=Ar.AreaCode,@TerritoryCode_ord=Tr.TerritoryCode , @MarketCode_ord=mr.MarketCode from tblCustMaster cus  with (nolock)
		 left join  tblMarket mr on cus.MarketId=mr.MarketId

	  
	 left join  tblTerritory  Tr  WITH (NOLOCK)   on mr.TerritoryId=Tr.TerritoryId
	 left join  tblArea  Ar  WITH (NOLOCK)   on Ar.AreaId=Tr.AreaId
	 left join  tblRegion  rg  WITH (NOLOCK)    on Ar.RegionId=rg.RegionId
	 left join  tbl_Group  gr  WITH (NOLOCK)   on gr.GroupId=rg.GroupId 
		 where   cus.CustomerMasterId = @CustomerId
	 

		
		INSERT INTO dbo.tbl_DCRCustomerInfo
		        ( DcrDate ,
		           
		          EmpInfoId ,
		          EntryDate ,
		          Remarks,
				  CustomerId, GroupId,RegionId,AreaId,TerritoryId,SubTerritoryId, MarketId, ApprovalStatus, Latitude, Longitude, StreetAddress, [GroupName]
           ,[RegionName]
           ,[AreaName]
           ,[TerritoryName]
           ,[SubTerritoryName]
           ,[MarketName],[GroupCode_DCR]
           ,[RegionCode_DCR]
           ,[AreaCode_DCR]
           ,[TerritoryCode_DCR]
           ,[SubTerritoryCode_DCR]
           ,[MarketCode_DCR] , [CompanyId]
		        )
		VALUES  ( 
					@dcrDate,
				 
					@sessionUser,
					GETDATE(),
					@remarks,
					@CustomerId, @GroupId,@RegionId,@AreaId,@TerritoryId,@SubTerritoryId, @MarketId, '2',@Latitude,@Longitude,@StreetAddress, @GroupName 
           ,@RegionName 
           ,@AreaName 
           ,@TerritoryName 
           ,@SubTerritoryName 
           ,@MarketName ,@GroupCode_Ord 
           ,@RegionCode_Ord 
           ,@AreaCode_Ord 
           ,@TerritoryCode_Ord 
           ,@SubTerritoryCode_Ord 
           ,@MarketCode_Ord, @CompanyId 
		        )

				DECLARE @Id INT
				SELECT @Id=SCOPE_IDENTITY()

			
--DECLARE @empId INT
----set @Id=@doctorId
		 


--DECLARE @Role NVARCHAR(MAX)
--SELECT @Role=usR.RoleName, @empId=emp.EmpInfoId
--FROM dbo.tblEmpGeneralInfo  emp  with (nolock) 
--left join  tblUser us  with (nolock)  on us.EmpInfoId=emp.EmpInfoId
--left join tbl_UserRoleInfo usR  with (nolock)  on usR.UserRoleID=us.UserRoleID
 
-- WHERE us.UserId=@userId


--DECLARE @ToEmpId1 INT
--DECLARE @GroupId1 INT
--DECLARE @RegionId1 INT
--DECLARE @AreaId1 INT
--DECLARE @TerrId1 INT

--DECLARE @EntryTime TIME(7)=cast(GETDATE() as time)
----SELECT * FROM dbo.View_webapi_FieldForce
----LEFT JOIN dbo.tblMIOInfo ON 

--IF(@Role='MIO')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE MIOEmpId=@empId
--END
--IF(@Role='ASM')
--BEGIN
--SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE ASMEMPId=@empId
--END
--IF(@Role='RSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE RSMEMPId=@empId
--END
--IF(@Role='NSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE NSMEMPId=@empId
--END

	
--EXECUTE dbo.sp_webapi_SaveDCRAppLog @DCRApprovalId = 0,                         -- int
--                                 @Date = @EntryDate_Apps,           -- datetime
--                                 @FromEmpId = @empId,                          -- int
--                                 @ToEmpId = 0,                            -- int
--                                 @TableId = @Id,                            -- int
--                                 @Status = N'Posted',                           -- nvarchar(max)
--                                 @Comments = N'',                         -- nvarchar(max)
--                                 @Type = N'DCR',                             -- nvarchar(max)
--                                 @Step = 1,                               -- int
--                                 @GroupId = @GroupId1,                            -- int
--                                 @RegionId = @RegionId1,                           -- int
--                                 @AreaId = @AreaId1,                             -- int
--                                 @TerritoryId = @TerrId1,                        -- int
--                                 @ToGroupId = 0,                          -- int
--                                 @ToRegionId = 0,                         -- int
--                                 @ToAreaId = 0,                           -- int
--                                 @ToTerritoryId = 0,                      -- int
--                                 @EntryByS = @empId,                         -- nvarchar(max)
--                                 @EntryDateS = @EntryDate_Apps,     -- datetime
--                                 @EntryTimeS = @EntryTime,                -- time(7)
--                                 @ApproveByS = NULL,                       -- nvarchar(max)
--                                 @ApproveDateS = NULL,   -- datetime
--                                 @ApproveTimeS = NULL,              -- time(7)
--                                 @EntryByApp = @empId,                       -- nvarchar(max)
--                                 @EntryDateApp = @EntryDate_Apps,   -- datetime
--                                 @EntryTimeApp = @EntryTime,              -- time(7)
--                                 @ApproveByApp = NULL,                     -- nvarchar(max)
--                                 @ApproveDateApp = NULL, -- datetime
--                                 @ApproveTimeApp = NULL,            -- time(7)
--                                 @MenuId = 382                            -- int


    SELECT  @Id



			


END

 
END

```

## sp_Webapi_Save_DcrBrand

- **Parameters**: `@BrandId INT`, `@pk INT` (DCR id)
- **Tables read**: none
- **Tables written**: `tbl_DcrBrandDetails` (INSERT)
- **Business logic**: Records one promoted-brand line item against a DCR (doctor call report) entry.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
 


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DcrBrand]
	-- Add the parameters for the stored procedure here
    @BrandId INT, @pk INT
AS
    BEGIN
	
        INSERT  INTO dbo.tbl_DcrBrandDetails
                ( BrandId, DcrId )
        VALUES  ( @BrandId, @pk )


    END


```

## sp_Webapi_Save_DcrInfo

- **Parameters**: `@doctorId INT`, `@dcrDate NVARCHAR(50)`, `@visitTypeId INT`, `@chamberId INT`, `@IsNonEffectiveReason BIT = NULL`, `@ReasonId INT`, `@sessionUser NVARCHAR(50)`, `@remarks NVARCHAR(50)`, `@DocTPDetailsId INT = 0`, `@EntryDate_Apps DATETIME = NULL`, `@Latitude NVARCHAR(MAX) = NULL`, `@Longitude NVARCHAR(MAX) = NULL`, `@StreetAddress NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tbl_DCRInfo` (duplicate check), `tbl_UserRoleInfo`, `tblRoleType`, `ZAS_UserFS_MIO`/`ZAS_UserFS_AM`/`ZAS_UserFS_RSM`, `tblDoctorMaster`, `tblMarket`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`, `tblSMCType`
- **Tables written**: `tbl_DoctorTourPlanDetail` (UPDATE IsDcrDone = 1), `tbl_DCRInfo` (INSERT)
- **Business logic**: Records a doctor DCR (call report) visit — only proceeds if `@remarks` is non-empty and no same-day DCR already exists for this doctor/user; marks the corresponding tour-plan detail row as done, resolves the caller's company and the doctor's full org hierarchy/SMC/program type (denormalized), and inserts the DCR row with `ApprovalStatus = '2'`. Contains a large commented-out approval-log posting block (dead code) and normalizes a 'Sept'->'Sep' date-format quirk on input.
- **Transaction/error handling**: none (no explicit transaction; relies on the pre-INSERT duplicate-count check, not a constraint)
- **Result set**: single scalar `@Id` (new tbl_DCRInfo identity) if the remarks/duplicate checks pass; the final unconditional `SELECT @Id` still returns 0 when the insert was skipped
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DcrInfo]
	-- Add the parameters for the stored procedure here
@doctorId INT ,
@dcrDate NVARCHAR(50),
@visitTypeId INT,
@chamberId INT,
@IsNonEffectiveReason bit=null,
@ReasonId INT,
@sessionUser NVARCHAR(50),
@remarks NVARCHAR(50),
@DocTPDetailsId INT =0,
@EntryDate_Apps datetime=NULL,

    @Latitude NVARCHAR(MAX) = NULL ,
    @Longitude NVARCHAR(MAX) = NULL ,

	@StreetAddress NVARCHAR(MAX) = NULL
AS
BEGIN

 SET @dcrDate = REPLACE(@dcrDate, 'Sept', 'Sep');
DECLARE @Id INT=0
if(@remarks<>'')
begin

declare @Count int=0
DECLARE @Roltp nvarchar(max)
	DECLARE @userId nvarchar(max)
SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @sessionUser
select @Count= ISNULL(count(*),0) from tbl_DCRInfo  where  CONVERT(Date,dcrDate)=CONVERT(Date,@dcrDate)  and EntryBy= @userId and DoctorId=@doctorId

	
	if(@Count=0)
	begin
 
	declare @CompanyId int=null

	


	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@sessionUser



				if(@Roltp='MIO')
				begin
				 select  @CompanyId=CompanyId  from 	ZAS_UserFS_MIO  where EmpInfoId= @sessionUser
				end


				if(@Roltp='MIO')
				begin
				 select  @CompanyId=CompanyId   from 	ZAS_UserFS_MIO  where EmpInfoId= @sessionUser
				end

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_AM  where EmpInfoId= @sessionUser
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_RSM  where EmpInfoId= @sessionUser
		end




	UPDATE dbo.tbl_DoctorTourPlanDetail SET IsDcrDone = 1 WHERE DocTPDetailsId = @DocTPDetailsId

	DECLARE  @SmcTypeId INT=null ,@SMCType NVARCHAR(500), @DoctorProgramypeId int, @GroupId INT,@RegionId INT,@AreaId INT, @TerritoryId INT,@SubTerritoryId INT,@marketId INT
		SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @sessionUser


			declare @GroupName nvarchar(max),@RegionName nvarchar(max),@AreaName nvarchar(max),@TerritoryName nvarchar(max),@SubTerritoryName nvarchar(max),@MarketName nvarchar(max), @GroupCode_ord nvarchar(max),@RegionCode_ord nvarchar(max),@AreaCode_ord nvarchar(max),@TerritoryCode_ord nvarchar(max),@SubTerritoryCode_ord nvarchar(max),@MarketCode_ord nvarchar(max)



		select @SMCType=smcT.SMCType, @SmcTypeId= ISNULL(tblDoctorMaster.SmcTypeId,NULL), @DoctorProgramypeId=tblDoctorMaster.ProgramTypeId, @GroupId=gr.GroupId, @RegionId=rg.RegionId, @AreaId=Ar.AreaId, @TerritoryId=Tr.TerritoryId, @marketId=tblDoctorMaster.MarketId,@GroupName=gr.GroupName, @RegionName=rg.RegionName, @AreaName=Ar.AreaName,@TerritoryName=Tr.TerritoryName ,  @MarketName=mr.MarketName ,@GroupCode_ord=gr.GroupCode, @RegionCode_ord=rg.RegionCode, @AreaCode_ord=Ar.AreaCode,@TerritoryCode_ord=Tr.TerritoryCode , @MarketCode_ord=mr.MarketCode from tblDoctorMaster   with (nolock)
		 left join  tblMarket mr on tblDoctorMaster.MarketId=mr.MarketId

	  
	 left join  tblTerritory  Tr  WITH (NOLOCK)   on mr.TerritoryId=Tr.TerritoryId
	 left join  tblArea  Ar  WITH (NOLOCK)   on Ar.AreaId=Tr.AreaId
	 left join  tblRegion  rg  WITH (NOLOCK)    on Ar.RegionId=rg.RegionId
	 left join  tbl_Group  gr  WITH (NOLOCK)   on gr.GroupId=rg.GroupId
	  left JOIN dbo.tblSMCType smcT   with (nolock) ON smcT.SmcTypeId = tblDoctorMaster.SmcTypeId
		 where   DoctorId = @doctorId
	 

		
		INSERT INTO dbo.tbl_DCRInfo
		        ( DcrDate ,
		          TourTypeId ,
		          ChemberId ,
		          EntryBy ,
		          EntryDate ,
		          Remarks,
				  DoctorId,
				  DocTPDetailsId, IsNonEffectiveReason,ReasonId,EntryDate_Apps,GroupId,RegionId,AreaId,TerritoryId,SubTerritoryId, MarketId, ApprovalStatus, Latitude, Longitude, StreetAddress,DoctorProgramypeId,[GroupName]
           ,[RegionName]
           ,[AreaName]
           ,[TerritoryName]
           ,[SubTerritoryName]
           ,[MarketName],[GroupCode_DCR]
           ,[RegionCode_DCR]
           ,[AreaCode_DCR]
           ,[TerritoryCode_DCR]
           ,[SubTerritoryCode_DCR]
           ,[MarketCode_DCR],  SmcTypeId_DCR, SMCType_DCR , CompanyId
		        )
		VALUES  ( 
					@dcrDate,
					@visitTypeId,
					@chamberId,
					@userId,
					GETDATE(),
					@remarks,
					@doctorId,
					@DocTPDetailsId,
					@IsNonEffectiveReason,@ReasonId,@EntryDate_Apps,@GroupId,@RegionId,@AreaId,@TerritoryId,@SubTerritoryId, @MarketId, '2',@Latitude,@Longitude,@StreetAddress,@DoctorProgramypeId,@GroupName 
           ,@RegionName 
           ,@AreaName 
           ,@TerritoryName 
           ,@SubTerritoryName 
           ,@MarketName ,@GroupCode_Ord 
           ,@RegionCode_Ord 
           ,@AreaCode_Ord 
           ,@TerritoryCode_Ord 
           ,@SubTerritoryCode_Ord 
           ,@MarketCode_Ord,  isnull(@SmcTypeId,0), isnull( @SMCType , 'N/A') , @CompanyId 
		        )

				
				SELECT @Id=SCOPE_IDENTITY()

			
--DECLARE @empId INT
----set @Id=@doctorId
		 


--DECLARE @Role NVARCHAR(MAX)
--SELECT @Role=usR.RoleName, @empId=emp.EmpInfoId
--FROM dbo.tblEmpGeneralInfo  emp  with (nolock) 
--left join  tblUser us  with (nolock)  on us.EmpInfoId=emp.EmpInfoId
--left join tbl_UserRoleInfo usR  with (nolock)  on usR.UserRoleID=us.UserRoleID
 
-- WHERE us.UserId=@userId


--DECLARE @ToEmpId1 INT
--DECLARE @GroupId1 INT
--DECLARE @RegionId1 INT
--DECLARE @AreaId1 INT
--DECLARE @TerrId1 INT

--DECLARE @EntryTime TIME(7)=cast(GETDATE() as time)
----SELECT * FROM dbo.View_webapi_FieldForce
----LEFT JOIN dbo.tblMIOInfo ON 

--IF(@Role='MIO')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE MIOEmpId=@empId
--END
--IF(@Role='ASM')
--BEGIN
--SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE ASMEMPId=@empId
--END
--IF(@Role='RSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE RSMEMPId=@empId
--END
--IF(@Role='NSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  with (nolock)  WHERE NSMEMPId=@empId
--END

	
--EXECUTE dbo.sp_webapi_SaveDCRAppLog @DCRApprovalId = 0,                         -- int
--                                 @Date = @EntryDate_Apps,           -- datetime
--                                 @FromEmpId = @empId,                          -- int
--                                 @ToEmpId = 0,                            -- int
--                                 @TableId = @Id,                            -- int
--                                 @Status = N'Posted',                           -- nvarchar(max)
--                                 @Comments = N'',                         -- nvarchar(max)
--                                 @Type = N'DCR',                             -- nvarchar(max)
--                                 @Step = 1,                               -- int
--                                 @GroupId = @GroupId1,                            -- int
--                                 @RegionId = @RegionId1,                           -- int
--                                 @AreaId = @AreaId1,                             -- int
--                                 @TerritoryId = @TerrId1,                        -- int
--                                 @ToGroupId = 0,                          -- int
--                                 @ToRegionId = 0,                         -- int
--                                 @ToAreaId = 0,                           -- int
--                                 @ToTerritoryId = 0,                      -- int
--                                 @EntryByS = @empId,                         -- nvarchar(max)
--                                 @EntryDateS = @EntryDate_Apps,     -- datetime
--                                 @EntryTimeS = @EntryTime,                -- time(7)
--                                 @ApproveByS = NULL,                       -- nvarchar(max)
--                                 @ApproveDateS = NULL,   -- datetime
--                                 @ApproveTimeS = NULL,              -- time(7)
--                                 @EntryByApp = @empId,                       -- nvarchar(max)
--                                 @EntryDateApp = @EntryDate_Apps,   -- datetime
--                                 @EntryTimeApp = @EntryTime,              -- time(7)
--                                 @ApproveByApp = NULL,                     -- nvarchar(max)
--                                 @ApproveDateApp = NULL, -- datetime
--                                 @ApproveTimeApp = NULL,            -- time(7)
--                                 @MenuId = 382                            -- int

end
    SELECT  @Id



			


END

 
END

```

## sp_Webapi_Save_DcrProduct

- **Parameters**: `@productId INT`, `@pk INT` (DCR id), `@type NVARCHAR(50)`, `@qty DECIMAL(18,0)` (unused in body), `@empid INT`
- **Tables read**: none
- **Tables written**: `tbl_DcrDetails` (INSERT)
- **Business logic**: Records one promoted-product line item against a DCR entry (the quantity parameter is accepted but never actually inserted anywhere).
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Save_DcrProduct]
	-- Add the parameters for the stored procedure here
@productId INT,
@pk INT,
@type NVARCHAR(50),
@qty DECIMAL(18,0),
@empid INT
AS
BEGIN

 


 
 
 
 

		INSERT INTO dbo.tbl_DcrDetails
		        ( DcrId, ProductId, Type ,EmpInfoId)
		VALUES  (
				@pk,@productId,@type,@empid
		          )
    
	 

   





 











END

```

## sp_Webapi_Save_DcrVisitedWith

- **Parameters**: `@empId INT`, `@pk INT` (DCR id)
- **Tables read**: none
- **Tables written**: `tbl_DcrVisitedWithDetails` (INSERT)
- **Business logic**: Records a colleague who accompanied the caller on a DCR visit.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
 

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DcrVisitedWith]
	-- Add the parameters for the stored procedure here
    @empId INT, @pk INT
AS
    BEGIN
	
        INSERT  INTO dbo.tbl_DcrVisitedWithDetails
                ( EmpInfoId, DcrId )
        VALUES  ( @empId, @pk )


    END


```

## sp_Webapi_Save_DoctorBrandDetail

- **Parameters**: `@masterId INT = NULL` (doctor id), `@itemName NVARCHAR(MAX) = NULL` (brand name)
- **Tables read**: `tblProductSQ`
- **Tables written**: `tblDoctorBrandDetail` (INSERT, conditional)
- **Business logic**: Resolves a brand name to its id and, if found, tags that brand against the doctor.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DoctorBrandDetail]
	-- Add the parameters for the stored procedure here
@masterId INT = NULL,
@itemName NVARCHAR(max) = NULL
AS
BEGIN


		DECLARE @dgId INT 


	SELECT   @dgId= ProductBrandId FROM dbo.tblProductSQ WHERE ProductSQName = @itemName

	IF(@dgId IS NOT NULL)
	BEGIN 
		INSERT INTO dbo.tblDoctorBrandDetail
		        ( DoctorId, BrandId )
		VALUES  ( @masterId,@dgId
		          )
	END




END


```

## sp_Webapi_Save_DoctorChemberDetail

- **Parameters**: `@pk INT` (doctor id), `@ChamberTypeId INT = NULL`, `@Name NVARCHAR(MAX) = NULL`
- **Tables read**: none
- **Tables written**: `tblDoctorChemberDetail` (INSERT)
- **Business logic**: Adds a chamber (clinic/visiting location) record for a doctor.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Webapi_Save_DoctorChemberDetail]
	-- Add the parameters for the stored procedure here
@pk INT,
@ChamberTypeId INT = NULL,
@Name NVARCHAR(max) = NULL
AS
BEGIN
	
	INSERT INTO dbo.tblDoctorChemberDetail
	        ( DoctorId ,
	          ChamberTypeId ,
	          Name
	        )
	VALUES  ( @pk,
	@ChamberTypeId,
	@Name
	        )
END


```

## sp_Webapi_Save_DoctorContactDetail

- **Parameters**: `@pk INT` (doctor id), `@ContactTypeId INT = NULL`, `@Contact NVARCHAR(MAX) = NULL`
- **Tables read**: none
- **Tables written**: `tblDoctorContactDetail` (INSERT)
- **Business logic**: Adds a contact-detail record (e.g. phone) for a doctor.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Webapi_Save_DoctorContactDetail]
	-- Add the parameters for the stored procedure here
@pk INT,
@ContactTypeId INT = NULL,
@Contact NVARCHAR(max) = NULL
AS
BEGIN
	
	INSERT INTO dbo.tblDoctorContactDetail
	        ( DoctorId ,
	          ContactTypeId ,
	          Contact
	        )
	VALUES  ( @pk,
	@ContactTypeId,
	@Contact
	        )
END


```

## sp_Webapi_Save_DoctorDegreeDetail

- **Parameters**: `@masterId INT = NULL` (doctor id), `@itemName NVARCHAR(MAX) = NULL` (degree name)
- **Tables read**: `tblDoctorDegree`
- **Tables written**: `tblDoctorDegreeDetail` (INSERT, conditional)
- **Business logic**: Resolves a degree name to its id and, if found, tags that degree against the doctor.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DoctorDegreeDetail]
	-- Add the parameters for the stored procedure here
@masterId INT = NULL,
@itemName NVARCHAR(max) = NULL
AS
BEGIN


		DECLARE @dgId INT 


	SELECT @dgId =  DegreeId FROM dbo.tblDoctorDegree WHERE DegreeName = @itemName

	IF(@dgId IS NOT NULL)
	BEGIN
		
		INSERT INTO dbo.tblDoctorDegreeDetail
		        ( DoctorId, DegId )
		VALUES  ( @masterId,@dgId
		          )
	END




END


```

## sp_Webapi_Save_DoctorEntry

- **Parameters**: `@MasterId INT = NULL` (unused), `@doctorName NVARCHAR(MAX) = NULL`, `@remarks NVARCHAR(MAX) = NULL`, `@entryBy INT = NULL`, `@EntryDate DATETIME = NULL` (unused), `@subMarketId INT = NULL` (unused), `@designaionId INT = NULL`, `@marketId INT = NULL`, `@TerritoryId INT = NULL` (unused), `@SubTerritoryId INT = NULL` (unused), `@DoctorCategoryId INT = NULL`, `@ProgramTypeId INT = NULL`, `@UnionName NVARCHAR(MAX) = NULL`, `@StationTypeId INT = NULL` (unused — a local variable of the same purpose is derived instead), `@DoctorTypeId INT = NULL`, `@SMCTypeId INT = NULL`, `@DoctorAddress NVARCHAR(MAX) = NULL`
- **Tables read**: `tblDoctorMaster` (for next code), `tblUser`, `tblMarketStationDetail`
- **Tables written**: `tblDoctorMaster` (INSERT, only when `@marketId > 0`), `tblDoctorMarketDetail` (INSERT)
- **Business logic**: Creates a new doctor master record from the mobile app: generates a sequential DoctorCode ('D' + MAX(DoctorId)+10001), looks up the market's station type, inserts the doctor with `ApprovalStatus = '2'` and `IsFromApp = 1`, then links the doctor to the market. Several alternate insert blocks (special day, chamber, contact detail) and an approval-log post are commented out (dead code). If `@marketId` is not `> 0`, nothing is inserted and `SELECT @doctorId` returns NULL. Note: the procedure body has three extra, unmatched trailing `END`s beyond the ones closing its BEGIN blocks.
- **Transaction/error handling**: none
- **Result set**: single scalar `@doctorId` (new identity, or NULL if `@marketId` was not `> 0`)
- **Caller**: UNUSED — no application caller found

```sql
 


CREATE PROCEDURE [dbo].[sp_Webapi_Save_DoctorEntry]
	-- Add the parameters for the stored procedure here

	  @MasterId INT=NULL,
    @doctorName NVARCHAR(MAX) = NULL ,
   
    @remarks NVARCHAR(MAX) = NULL ,
    @entryBy INT = NULL ,
    @EntryDate DATETIME = NULL ,
   
    --@contactTypeId INT = NULL ,
    ----@contactInfo NVARCHAR(MAX) = NULL ,
    @subMarketId INT = NULL,
	@designaionId INT =NULL,
	@marketId INT = NULL,
	@TerritoryId  INT = NULL,
	@SubTerritoryId  INT = NULL,
	@DoctorCategoryId  INT = NULL,
	@ProgramTypeId  INT = NULL,
	@UnionName NVARCHAR(MAX) = NULL ,
	@StationTypeId  INT = NULL ,
	@DoctorTypeId  INT = NULL  ,
	--@SpecialDayId  INT = NULL ,
	 
    @SMCTypeId INT = NULL ,
	@DoctorAddress NVARCHAR(MAX) = NULL

AS
    BEGIN




        DECLARE @doctorId INT,@userId int
        DECLARE @DoctorCode NVARCHAR(MAX)

        SELECT  @DoctorCode = 'D'
                + ( CONVERT(NVARCHAR(MAX), ( max(DoctorId) + 10001 )) )
        FROM    tblDoctorMaster


		SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @entryBy

		--IF(NOT EXISTS (SELECT * FROM dbo.tblDoctorMaster WHERE DoctorName = @DoctorName  ))
	BEGIN



	if(@marketId >0)
	begin
	 DECLARE @RouteInformationMasterId int ,	@StationTypeId_ INT
		

	 select @StationTypeId_= ISNULL(StationTypeId,null) from tblMarketStationDetail where MarketId=@marketId

		  

		 


        INSERT  INTO dbo.tblDoctorMaster
                ( DoctorName ,
                  DoctorCode ,
                  Remarks ,
                  IsActive ,
                  EntryBy ,
                  EntryDate ,
                
				  IsFromApp,
				  DesignationId,
				  ApprovalStatus   ,
				     MarketId
       ,[UnionName]
      
      ,[StationTypeId]
      ,[ProgramTypeId]
      ,[DoctorCategoryId], DoctorTypeId, SMCTypeId, DoctorAddress
	            )
        VALUES  ( @doctorName ,
                  @DoctorCode ,
                  @remarks ,
                  1,
                  @userId ,
                  GETDATE() ,
                  
				  1,
				  @designaionId,
				  '2'
				 ,   @marketId
      ,@UnionName 
      
      ,1 
      ,@ProgramTypeId 
      ,@DoctorCategoryId ,@DoctorTypeId,@SMCTypeId,@DoctorAddress
	            )

				 
  select    @doctorId=       SCOPE_IDENTITY()

        IF ( @marketId IS NOT NULL )
            BEGIN
                INSERT  INTO dbo.tblDoctorMarketDetail
                        ( DoctorId, MarketId )
                VALUES  ( @doctorId, @marketId )
            END

			   --IF (@SpeciaDateStr IS  not NULL )
      --      BEGIN
      --          INSERT  INTO dbo.tblDoctorSpecialDayDetail
      --                  ( DoctorId, SpecialDayId,SpecialDate )
      --          VALUES  ( @doctorId, @SpecialDayId,@SpeciaDateStr )
      --      END
		 


--        --IF ( @chamberId IS NOT NULL )
--        --    BEGIN
--        --        INSERT  INTO dbo.tblDoctorChemberDetail
--        --                ( ChamberTypeId ,
--        --                  DoctorId ,
--        --                  Name 
--        --                )
--        --        VALUES  ( @chamberId ,
--        --                  @doctorId ,
--        --                  @chamberAddress
--        --                )
	
--        --    END

        --IF ( @contactTypeId IS NOT NULL )
        --    BEGIN
        --        INSERT  INTO dbo.tblDoctorContactDetail
        --                ( DoctorId ,
        --                  ContactTypeId ,
        --                  Contact 
	       --             )
        --        VALUES  ( @doctorId ,
        --                  @contactTypeId ,
        --                  @contactInfo
	       --             )
        --    END

--			 --IF ( @contactTypeId IS NOT NULL )
--    --        BEGIN
--    --            INSERT  INTO dbo.tblDoctorContactDetail
--    --                    ( DoctorId ,
--    --                      ContactTypeId ,
--    --                      Contact 
--	   --                 )
--    --            VALUES  ( @doctorId ,
--    --                      @contactTypeId ,
--    --                      @contactInfo
--	   --                 )
--    --        END

		
		
--    END
	
--DECLARE @Id INT
--DECLARE @empId INT
--set @Id=@doctorId
		 


--DECLARE @Role NVARCHAR(MAX)
--SELECT @Role=usR.RoleName, @empId=emp.EmpInfoId
--FROM dbo.tblEmpGeneralInfo  emp
--left join  tblUser us on us.EmpInfoId=emp.EmpInfoId
--left join tbl_UserRoleInfo usR on usR.UserRoleID=us.UserRoleID
 
-- WHERE us.UserId=@userId


--DECLARE @ToEmpId1 INT
--DECLARE @GroupId1 INT
--DECLARE @RegionId1 INT
--DECLARE @AreaId1 INT
--DECLARE @TerrId1 INT

--DECLARE @EntryTime TIME(7)=cast(GETDATE() as time)
----SELECT * FROM dbo.View_webapi_FieldForce
----LEFT JOIN dbo.tblMIOInfo ON 

--IF(@Role='MIO')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE MIOEmpId=@empId
--END
--IF(@Role='ASM')
--BEGIN
--SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE ASMEMPId=@empId
--END
--IF(@Role='RSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE RSMEMPId=@empId
--END
--IF(@Role='NSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE NSMEMPId=@empId
--END

	
--EXECUTE dbo.sp_webapi_SaveDoctorAppLog @DoctorApprovalId = 0,                         -- int
--                                 @Date = @EntryDate,           -- datetime
--                                 @FromEmpId = @empId,                          -- int
--                                 @ToEmpId = 0,                            -- int
--                                 @TableId = @Id,                            -- int
--                                 @Status = N'Posted',                           -- nvarchar(max)
--                                 @Comments = N'',                         -- nvarchar(max)
--                                 @Type = N'Doctor',                             -- nvarchar(max)
--                                 @Step = 1,                               -- int
--                                 @GroupId = @GroupId1,                            -- int
--                                 @RegionId = @RegionId1,                           -- int
--                                 @AreaId = @AreaId1,                             -- int
--                                 @TerritoryId = @TerrId1,                        -- int
--                                 @ToGroupId = 0,                          -- int
--                                 @ToRegionId = 0,                         -- int
--                                 @ToAreaId = 0,                           -- int
--                                 @ToTerritoryId = 0,                      -- int
--                                 @EntryByS = @empId,                         -- nvarchar(max)
--                                 @EntryDateS = @EntryDate,     -- datetime
--                                 @EntryTimeS = @EntryTime,                -- time(7)
--                                 @ApproveByS = NULL,                       -- nvarchar(max)
--                                 @ApproveDateS = NULL,   -- datetime
--                                 @ApproveTimeS = NULL,              -- time(7)
--                                 @EntryByApp = @empId,                       -- nvarchar(max)
--                                 @EntryDateApp = @EntryDate,   -- datetime
--                                 @EntryTimeApp = @EntryTime,              -- time(7)
--                                 @ApproveByApp = NULL,                     -- nvarchar(max)
--                                 @ApproveDateApp = NULL, -- datetime
--                                 @ApproveTimeApp = NULL,            -- time(7)
--                                 @MenuId = 303                            -- int


    SELECT  @doctorId

	--END
	 END
	 END
	 END

```

## sp_Webapi_Save_DoctorInstitutionDetail

- **Parameters**: `@masterId INT = NULL` (doctor id), `@itemName NVARCHAR(MAX) = NULL` (institution name)
- **Tables read**: `tblInstitutionInfo`
- **Tables written**: `tblDoctorInstitutionDetail` (INSERT, conditional)
- **Business logic**: Resolves an institution name to its id and, if found, tags that institution against the doctor.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DoctorInstitutionDetail]
	-- Add the parameters for the stored procedure here
    @masterId INT = NULL ,
    @itemName NVARCHAR(MAX) = NULL
AS
    BEGIN


        DECLARE @dgId INT 


        SELECT  @dgId = InstitutionId
        FROM    dbo.tblInstitutionInfo
        WHERE   Institution = @itemName


		IF(@dgId IS NOT NULL)
		BEGIN
			    INSERT  INTO dbo.tblDoctorInstitutionDetail
                ( DoctorId, InstitutionId )
        VALUES  ( @masterId, @dgId )
		END






    END


```

## sp_Webapi_Save_DoctorSpecialDayDetail

- **Parameters**: `@pk INT` (doctor id), `@SpecialDayId INT = NULL`, `@SpeciaDateStr DATETIME = NULL`
- **Tables read**: none
- **Tables written**: `tblDoctorSpecialDayDetail` (INSERT)
- **Business logic**: Adds a "special day" (e.g. weekly off) record for a doctor.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DoctorSpecialDayDetail]
	-- Add the parameters for the stored procedure here
@pk INT,
@SpecialDayId INT = NULL,
@SpeciaDateStr datetime = NULL
AS
BEGIN
	
	INSERT INTO dbo.tblDoctorSpecialDayDetail
	        ( DoctorId ,
	          SpecialDayId ,
	          SpecialDate
	        )
	VALUES  ( @pk,
	@SpecialDayId,
	@SpeciaDateStr
	        )
END


```

## sp_Webapi_Save_DoctorSpecialityDetail

- **Parameters**: `@masterId INT = NULL` (doctor id), `@itemName NVARCHAR(MAX) = NULL` (speciality name)
- **Tables read**: `tblDoctorSpeciality`
- **Tables written**: `tblDoctorSpecialityDetail` (INSERT, unconditional)
- **Business logic**: Resolves a speciality name to its id and tags it against the doctor; unlike the sibling Save_Doctor*Detail procs, this one inserts even when the name doesn't resolve (inserting a NULL SpecialityId).
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DoctorSpecialityDetail]
	-- Add the parameters for the stored procedure here
@masterId INT = NULL,
@itemName NVARCHAR(max) = NULL
AS
BEGIN


		DECLARE @dgId INT 


	SELECT @dgId =  SpecialityId FROM dbo.tblDoctorSpeciality WHERE SpecialityName = @itemName


		INSERT INTO dbo.tblDoctorSpecialityDetail
		        ( DoctorId, SpecialityId )
		VALUES  ( @masterId,@dgId
		          )


END


```

## sp_Webapi_Save_DoctorTypeDetail

- **Parameters**: `@masterId INT = NULL` (doctor id), `@itemName NVARCHAR(MAX) = NULL` (program-type name)
- **Tables read**: `tblDoctorProgramType`
- **Tables written**: `tblDoctorProgramTypeDetail` (INSERT, unconditional)
- **Business logic**: Resolves a program-type name to its id and tags it against the doctor (name misleadingly says "DoctorType" but the lookup/target tables are program-type tables); inserts unconditionally even if the name doesn't resolve.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Save_DoctorTypeDetail]
	-- Add the parameters for the stored procedure here
    @masterId INT = NULL ,
    @itemName NVARCHAR(MAX) = NULL
AS
    BEGIN


        DECLARE @dgId INT 


        SELECT  @dgId = ProgramTypeId
        FROM    dbo.tblDoctorProgramType
        WHERE   ProgramType = @itemName


        INSERT  INTO dbo.tblDoctorProgramTypeDetail
                ( DoctorId, ProgramTypeId )
        VALUES  ( @masterId, @dgId )


    END


```

## sp_Webapi_Save_Prescription

- **Parameters**: `@doctorId INT = NULL`, `@presTypeId INT = NULL`, `@ChemberId INT = NULL`, `@presDate DATETIME = NULL`, `@sessionUser NVARCHAR(50) = NULL`, `@Remarks NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`, `ZAS_UserFS_MIO`/`ZAS_UserFS_AM`/`ZAS_UserFS_RSM`, `tblDoctorMaster`, `tblMarket`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`, `tblSMCType`, `tblDoctorType`
- **Tables written**: `tbl_PrescriptionMaster` (INSERT)
- **Business logic**: Records a new prescription master row — resolves the caller's company by role and the doctor's full denormalized org hierarchy/SMC/doctor-type, then inserts with `ApprovalStatus = '2'`. Contains a large commented-out approval-log posting block (dead code).
- **Transaction/error handling**: none
- **Result set**: single scalar `@Id` (new tbl_PrescriptionMaster identity)
- **Caller**: UNUSED — no application caller found

```sql



CREATE PROCEDURE [dbo].[sp_Webapi_Save_Prescription]
	-- Add the parameters for the stored procedure here
@doctorId INT =NULL,
@presTypeId INT = NULL,
@ChemberId INT = NULL,
@presDate DATETIME = NULL,
@sessionUser NVARCHAR(50)=null,
@Remarks NVARCHAR(max)=null

AS
BEGIN
	
	DECLARE @Roltp nvarchar(max)
	DECLARE @userId INT 
	declare @CompanyId int=null
	SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @sessionUser


	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@sessionUser


					if(@Roltp='MIO')
				begin
				 select  @CompanyId=CompanyId  from 	ZAS_UserFS_MIO  where EmpInfoId= @sessionUser
				end


				if(@Roltp='MIO')
				begin
				 select  @CompanyId=CompanyId   from 	ZAS_UserFS_MIO  where EmpInfoId= @sessionUser
				end

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_AM  where EmpInfoId= @sessionUser
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_RSM  where EmpInfoId= @sessionUser
		end



	DECLARE @DoctorTypeId int,  @DoctorType NVARCHAR(max),  @SmcTypeId INT=null ,@SMCType NVARCHAR(500), @DoctorProgramypeId int, @GroupId INT,@RegionId INT,@AreaId INT, @TerritoryId INT,@SubTerritoryId INT,@marketId INT

	declare @GroupName nvarchar(max),@RegionName nvarchar(max),@AreaName nvarchar(max),@TerritoryName nvarchar(max),@SubTerritoryName nvarchar(max),@MarketName nvarchar(max), @GroupCode_ord nvarchar(max),@RegionCode_ord nvarchar(max),@AreaCode_ord nvarchar(max),@TerritoryCode_ord nvarchar(max),@SubTerritoryCode_ord nvarchar(max),@MarketCode_ord nvarchar(max)

	select @DoctorTypeId=ISNULL(tblDoctorMaster.DoctorTypeId,0), @DoctorType=ISNULL(dtT.DoctorTypeName,'N/A'),  @SMCType=smcT.SMCType, @SmcTypeId= ISNULL(tblDoctorMaster.SmcTypeId,NULL), @DoctorProgramypeId=tblDoctorMaster.ProgramTypeId,  @GroupId=gr.GroupId, @RegionId=rg.RegionId, @AreaId=Ar.AreaId,@TerritoryId=Tr.TerritoryId, @marketId=tblDoctorMaster.MarketId,@GroupName=gr.GroupName, @RegionName=rg.RegionName, @AreaName=Ar.AreaName,@TerritoryName=Tr.TerritoryName , @MarketName=mr.MarketName  ,@GroupCode_ord=gr.GroupCode, @RegionCode_ord=rg.RegionCode, @AreaCode_ord=Ar.AreaCode,@TerritoryCode_ord=Tr.TerritoryCode , @MarketCode_ord=mr.MarketCode from 
	

	tblDoctorMaster   with (nolock)
  left join  tblMarket mr on tblDoctorMaster.MarketId=mr.MarketId

	 
	 left join  tblTerritory  Tr  WITH (NOLOCK)   on mr.TerritoryId=Tr.TerritoryId
	 left join  tblArea  Ar  WITH (NOLOCK)   on Ar.AreaId=Tr.AreaId
	 left join  tblRegion  rg  WITH (NOLOCK)    on Ar.RegionId=rg.RegionId
	 left join  tbl_Group  gr  WITH (NOLOCK)   on gr.GroupId=rg.GroupId
	 
		 left JOIN dbo.tblSMCType smcT   with (nolock) ON smcT.SmcTypeId = tblDoctorMaster.SmcTypeId

		  left JOIN dbo.tblDoctorType dtT   with (nolock) ON dtT.DoctorTypeId = tblDoctorMaster.DoctorTypeId
		  
		 where   DoctorId = @doctorId
		 
	 
	INSERT INTO dbo.tbl_PrescriptionMaster
	        ( PrescriptionDate ,
	          PrescriptionTypeId ,
	          DoctorId ,
	          EntryBy ,
	          EntryDate,
			  ApprovalStatus,ChemberId,GroupId,RegionId,AreaId,TerritoryId,SubTerritoryId, MarketId,DoctorProgramypeId,[GroupName]
           ,[RegionName]
           ,[AreaName]
           ,[TerritoryName]
           ,[SubTerritoryName]
           ,[MarketName] ,[GroupCode_RX]
           ,[RegionCode_RX]
           ,[AreaCode_RX]
           ,[TerritoryCode_RX]
           ,[SubTerritoryCode_RX]
           ,[MarketCode_RX],  SmcTypeId_RX, SMCType_RX,DoctorType_RX,DoctorTypeId_RX, Remarks, CompanyId
	        )
	VALUES  ( 
	@presDate,
	@presTypeId,
	@doctorId,
	@userId,
	GETDATE(),
	'2',@ChemberId,@GroupId,@RegionId,@AreaId,@TerritoryId,@SubTerritoryId, @MarketId,@DoctorProgramypeId ,@GroupName 
           ,@RegionName 
           ,@AreaName 
           ,@TerritoryName 
           ,@SubTerritoryName 
           ,@MarketName 
		   ,@GroupCode_Ord 
           ,@RegionCode_Ord 
           ,@AreaCode_Ord 
           ,@TerritoryCode_Ord 
           ,@SubTerritoryCode_Ord 
           ,@MarketCode_Ord,  isnull(@SmcTypeId,0),isnull( @SMCType , 'N/A'), @DoctorType ,@DoctorTypeId , @Remarks, @CompanyId
	        )


DECLARE @Id INT
				SELECT @Id=SCOPE_IDENTITY()

			
--DECLARE @empId INT
----set @Id=@doctorId
		 


--DECLARE @Role NVARCHAR(MAX)
--SELECT @Role=usR.RoleName, @empId=emp.EmpInfoId
--FROM dbo.tblEmpGeneralInfo  emp  WITH (NOLOCK)  
--left join  tblUser us  WITH (NOLOCK)   on us.EmpInfoId=emp.EmpInfoId
--left join tbl_UserRoleInfo usR  WITH (NOLOCK)  on usR.UserRoleID=us.UserRoleID
 
-- WHERE us.UserId=@userId


--DECLARE @ToEmpId1 INT
--DECLARE @GroupId1 INT
--DECLARE @RegionId1 INT
--DECLARE @AreaId1 INT
--DECLARE @TerrId1 INT

--DECLARE @EntryTime TIME(7)=cast(GETDATE() as time)
----SELECT * FROM dbo.View_webapi_FieldForce
----LEFT JOIN dbo.tblMIOInfo ON 

--IF(@Role='MIO')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  WITH (NOLOCK)   WHERE MIOEmpId=@empId
--END
--IF(@Role='ASM')
--BEGIN
--SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  WITH (NOLOCK)   WHERE ASMEMPId=@empId
--END
--IF(@Role='RSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  WITH (NOLOCK)   WHERE RSMEMPId=@empId
--END
--IF(@Role='NSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce  WITH (NOLOCK)   WHERE NSMEMPId=@empId
--END

--DECLARE @EDate DATETIME=GETDATE()
	
--EXECUTE dbo.sp_webapi_SavePrescriptionAppLog @PrescriptionApprovalId = 0,                         -- int
--                                 @Date = @EDate,           -- datetime
--                                 @FromEmpId = @empId,                          -- int
--                                 @ToEmpId = 0,                            -- int
--                                 @TableId = @Id,                            -- int
--                                 @Status = N'Posted',                           -- nvarchar(max)
--                                 @Comments = N'',                         -- nvarchar(max)
--                                 @Type = N'Prescription',                             -- nvarchar(max)
--                                 @Step = 1,                               -- int
--                                 @GroupId = @GroupId1,                            -- int
--                                 @RegionId = @RegionId1,                           -- int
--                                 @AreaId = @AreaId1,                             -- int
--                                 @TerritoryId = @TerrId1,                        -- int
--                                 @ToGroupId = 0,                          -- int
--                                 @ToRegionId = 0,                         -- int
--                                 @ToAreaId = 0,                           -- int
--                                 @ToTerritoryId = 0,                      -- int
--                                 @EntryByS = @empId,                         -- nvarchar(max)
--                                 @EntryDateS = @EDate,     -- datetime
--                                 @EntryTimeS = @EntryTime,                -- time(7)
--                                 @ApproveByS = NULL,                       -- nvarchar(max)
--                                 @ApproveDateS = NULL,   -- datetime
--                                 @ApproveTimeS = NULL,              -- time(7)
--                                 @EntryByApp = @empId,                       -- nvarchar(max)
--                                 @EntryDateApp = @EDate,   -- datetime
--                                 @EntryTimeApp = @EntryTime,              -- time(7)
--                                 @ApproveByApp = NULL,                     -- nvarchar(max)
--                                 @ApproveDateApp = NULL, -- datetime
--                                 @ApproveTimeApp = NULL,            -- time(7)
--                                 @MenuId = 379                           -- int


    SELECT  @Id


END
```

## sp_Webapi_Save_PrescriptionDetail

- **Parameters**: `@productId INT = NULL`, `@pk INT` (prescription id)
- **Tables read**: none
- **Tables written**: `tbl_PrescriptionProductDetail` (INSERT)
- **Business logic**: Records one prescribed-product line item against a prescription.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
 

CREATE PROCEDURE [dbo].[sp_Webapi_Save_PrescriptionDetail]
	-- Add the parameters for the stored procedure here
	@productId INT = NULL,
	@pk int

AS
BEGIN
		
		INSERT INTO dbo.tbl_PrescriptionProductDetail
		        ( 
		          PrescriptionId ,
		          ProductId
		        )
		VALUES  (
		@pk,@productId

		        )


END


```

## sp_Webapi_Save_SynchronizationInfo

- **Parameters**: `@EmpInfoId INT = NULL`
- **Tables read**: none
- **Tables written**: `tblSynchronizationInfo` (INSERT)
- **Business logic**: Logs a timestamp each time the mobile app performs a data synchronization for an employee.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
create PROCEDURE [dbo].[sp_Webapi_Save_SynchronizationInfo]
	-- Add the parameters for the stored procedure here
  @EmpInfoId int = NULL  
  
AS
    BEGIN
 
    INSERT INTO [dbo].[tblSynchronizationInfo]
           ( [EmpInfoId]
           ,[SynchronizationDate])
     VALUES
           (  @EmpInfoId 
           ,GETDATE())
    End
 

```

## sp_Webapi_Save_TourPlanInfo

- **Parameters**: `@customerId INT = NULL`, `@TourDate DATETIME = NULL`, `@empId INT = NULL`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`, `ZAS_UserFS_MIO`/`ZAS_UserFS_AM`/`ZAS_UserFS_RSM`, `tbl_TourPlanMaster`, `tblCustMaster`, `tblmarket`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`
- **Tables written**: `tbl_TourPlanMaster` (INSERT, if none exists for month/year/employee), `tbl_TourPlanInfo` (INSERT)
- **Business logic**: Records a new customer/market tour-plan entry — resolves the caller's company by role, gets-or-creates the month/year `tbl_TourPlanMaster` header for the employee, resolves the customer's full org hierarchy (denormalized), and inserts a `tbl_TourPlanInfo` detail row. Note: `@StationTypeId` is used in the INSERT column list but never assigned, so it is always inserted as NULL.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_Save_TourPlanInfo]
	-- Add the parameters for the stored procedure here
   
    @customerId INT =null,
    
    @TourDate DATETIME = NULL ,
    @empId INT = null
AS
    BEGIN

	DECLARE @tpMasterId int


		DECLARE @Roltp nvarchar(max)
	declare @CompanyId int =null



	
	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId



			 

				if(@Roltp='MIO')
				begin
				 select  @CompanyId=CompanyId   from 	ZAS_UserFS_MIO  where EmpInfoId= @empId
				end

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_AM  where EmpInfoId= @empId
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_RSM  where EmpInfoId= @empId
		end






	IF(NOT EXISTS(SELECT * FROM dbo.tbl_TourPlanMaster WHERE MonthValue = MONTH(@TourDate) AND YearValue = YEAR(@TourDate) AND EmpInfoId = @empId))
	BEGIN

	INSERT INTO dbo.tbl_TourPlanMaster
	        ( MonthValue ,
	          YearValue ,
	          EmpInfoId 
	          
	        )
	VALUES  ( 
	MONTH(@TourDate),
	YEAR(@TourDate),
	@empId

	        )


			SET @tpMasterId = SCOPE_IDENTITY()

		
	END
	ELSE
	BEGIN

	SET @tpMasterId = (SELECT  TOP 1 TPMaster FROM dbo.tbl_TourPlanMaster WHERE MonthValue = MONTH(@TourDate) AND YearValue = YEAR(@TourDate) AND EmpInfoId = @empId ORDER BY TPMaster DESC)
		
	END
    

	declare @GroupName nvarchar(max),@RegionName nvarchar(max),@AreaName nvarchar(max),@TerritoryName nvarchar(max), @MarketName nvarchar(max), @GroupCode_ord nvarchar(max),@RegionCode_ord nvarchar(max),@AreaCode_ord nvarchar(max),@TerritoryCode_ord nvarchar(max) ,@MarketCode_ord nvarchar(max)

	DECLARE @marketId int, @StationTypeId INT, @GroupId INT,@RegionId INT,@AreaId INT,@TerritoryId_ INT  


	select @marketId=mr.MarketId, @TerritoryId_=tr.TerritoryId,@AreaId=ar.AreaId,@RegionId=rg.RegionId,@GroupId=rg.GroupId,@GroupName=gr.GroupName, @RegionName=rg.RegionName, @AreaName=Ar.AreaName,@TerritoryName=Tr.TerritoryName , @MarketName=mr.MarketName ,@GroupCode_ord=gr.GroupCode, @RegionCode_ord=rg.RegionCode, @AreaCode_ord=Ar.AreaCode,@TerritoryCode_ord=Tr.TerritoryCode   , @MarketCode_ord=mr.MarketCode from tblCustMaster cus with (nolock)
		inner join tblmarket mr  with (nolock) on cus.MarketId=mr.MarketId
		inner join tblTerritory tr  with (nolock) on mr.TerritoryId=tr.TerritoryId
		inner join tblArea ar   with (nolock)  on ar.AreaId=tr.AreaId
		inner join tblRegion rg  with (nolock) on ar.RegionId=rg.RegionId
		 inner join  tbl_Group  gr  WITH (NOLOCK)   on gr.GroupId=rg.GroupId
		  where  cus.CustomerMasterId=@customerId


        INSERT  INTO dbo.tbl_TourPlanInfo
                (  
                  CustomerMasterId ,
                 
                  TourPlanDate ,
                  EmpInfoId ,
                  
                  CreatedBy ,
                  CreatedDate,
				  TPMaster,GroupId,RegionId,AreaId,TerritoryId,  MarketId,TourTypeId, [GroupName]
           ,[RegionName]
           ,[AreaName]
           ,[TerritoryName]
            
           ,[MarketName],[GroupCode_TP]
           ,[RegionCode_TP]
           ,[AreaCode_TP]
           ,[TerritoryCode_TP]
           
           ,[MarketCode_TP],CompanyId
	            )
        VALUES  (  
                  @customerId ,
                  
                  @TourDate ,
                  @empId ,
                 
                  @empId ,
                  GETDATE(),
				  @tpMasterId,@GroupId,@RegionId,@AreaId,@TerritoryId_,  @marketId,@StationTypeId, @GroupName 
           ,@RegionName 
           ,@AreaName 
           ,@TerritoryName 
           
           ,@MarketName ,@GroupCode_Ord 
           ,@RegionCode_Ord 
           ,@AreaCode_Ord 
           ,@TerritoryCode_Ord 
           
           ,@MarketCode_Ord,@CompanyId
                )


    END
```

## sp_Webapi_Save_TourPlanInfo_Doctor

- **Parameters**: `@TourDate DATETIME = NULL`, `@doctorId INT = NULL`, `@empId INT = NULL`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`, `ZAS_UserFS_MIO`/`ZAS_UserFS_AM`/`ZAS_UserFS_RSM`, `tblEmpGeneralInfo`, `tbl_DoctorTourPlanDetail` (duplicate check), `tbl_DoctorTourPlanMaster`, `tblDoctorMaster`, `tblMarket`, `tblSMCType`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`
- **Tables written**: `tbl_DoctorTourPlanMaster` (INSERT, if none exists for month/year/employee), `tbl_DoctorTourPlanDetail` (INSERT, only if no plan for that doctor/date exists yet)
- **Business logic**: Records a new doctor tour-plan entry — resolves the caller's company by role (falling back to the employee's own company if unresolved), gets-or-creates the month/year plan master, and — only if the doctor isn't already planned for that date — resolves the doctor's full denormalized org hierarchy/SMC/program type and inserts the detail row. Contains a large commented-out approval-log posting block (dead code); resets `@tpMasterId = 0` at the very end regardless of outcome.
- **Transaction/error handling**: none
- **Result set**: none (the `@Id` output is only ever assigned inside a commented-out `SELECT @Id`)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Save_TourPlanInfo_Doctor]
	-- Add the parameters for the stored procedure here
  @TourDate DATETIME = NULL ,
  @doctorId INT = NULL,
  @empId INT =null
  
AS
    BEGIN

	DECLARE @Roltp nvarchar(max)
	declare @CompanyId int =0



	
	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId



			 

				if(@Roltp='MIO')
				begin
				 select  @CompanyId=ISNULL(CompanyId,0)   from 	ZAS_UserFS_MIO  where EmpInfoId= @empId
				end

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=CompanyId  from 	ZAS_UserFS_AM  where EmpInfoId= @empId
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=ISNULL(CompanyId,0)  from 	ZAS_UserFS_RSM  where EmpInfoId= @empId
		end

			if(@CompanyId=0)
		begin
		select  @CompanyId=e.CompanyId   FROM dbo.tblEmpGeneralInfo e
	
			 
			  				
				  WHERE e.EmpInfoId=@empId
		end


	DECLARE @tpMasterId int
	IF(NOT EXISTS(SELECT * FROM dbo.tbl_DoctorTourPlanDetail WHERE TourPlanDate = @TourDate  AND DoctorId = @doctorId))
	BEGIN

	IF(NOT EXISTS(SELECT * FROM dbo.tbl_DoctorTourPlanMaster WHERE MonthValue = MONTH(@TourDate) AND YearValue = YEAR(@TourDate) AND EmpInfoId = @empId))
	BEGIN

		
		INSERT INTO dbo.tbl_DoctorTourPlanMaster
		        ( MonthValue ,
		          YearValue ,
		          EmpInfoId 
		        )
		VALUES  ( 
		MONTH(@TourDate),
		YEAR(@TourDate),
		@empId

		        )


			SET @tpMasterId = SCOPE_IDENTITY()

		
	END
	ELSE
	BEGIN

	SET @tpMasterId = (SELECT  TOP 1 DocTPMaster FROM dbo.tbl_DoctorTourPlanMaster WHERE MonthValue = MONTH(@TourDate) AND YearValue = YEAR(@TourDate) AND EmpInfoId = @empId ORDER BY DocTPMaster DESC)
		
	END

	DECLARE @StationTypeId int
 SELECT  @StationTypeId=StationTypeId FROM dbo.tblDoctorMaster WHERE DoctorId=@doctorId

 	DECLARE  @DoctorProgramypeId int,@userId INT, @GroupId INT,@RegionId INT,@AreaId INT, @TerritoryId INT,@SubTerritoryId INT,@marketId INT

	declare @GroupName nvarchar(max),@RegionName nvarchar(max),@AreaName nvarchar(max),@TerritoryName nvarchar(max),@SubTerritoryName nvarchar(max),@MarketName nvarchar(max), @GroupCode_ord nvarchar(max),@RegionCode_ord nvarchar(max),@AreaCode_ord nvarchar(max),@TerritoryCode_ord nvarchar(max),@SubTerritoryCode_ord nvarchar(max),@MarketCode_ord nvarchar(max),@SMCTypeId_DV int ,@SMCType_DV nvarchar(max)


 	select @SMCTypeId_DV=tblDoctorMaster.SMCTypeId, @SMCType_DV= st.SmcType,  @DoctorProgramypeId=tblDoctorMaster.ProgramTypeId, @GroupId=gr.GroupId, @RegionId=rg.RegionId, @AreaId=Ar.AreaId,@TerritoryId=Tr.TerritoryId, @marketId=tblDoctorMaster.MarketId,@GroupName=gr.GroupName, @RegionName=rg.RegionName, @AreaName=Ar.AreaName,@TerritoryName=Tr.TerritoryName  , @MarketName=mr.MarketName,@GroupCode_ord=gr.GroupCode, @RegionCode_ord=rg.RegionCode, @AreaCode_ord=Ar.AreaCode,@TerritoryCode_ord=Tr.TerritoryCode , @MarketCode_ord=mr.MarketCode  from tblDoctorMaster   with (nolock)
		 left join  tblMarket mr on tblDoctorMaster.MarketId=mr.MarketId
		 left join  tblSMCType st on tblDoctorMaster.SMCTypeId=st.SMCTypeId

	  
	 left join  tblTerritory  Tr  WITH (NOLOCK)   on mr.TerritoryId=Tr.TerritoryId
	 left join  tblArea  Ar  WITH (NOLOCK)   on Ar.AreaId=Tr.AreaId
	 left join  tblRegion  rg  WITH (NOLOCK)    on Ar.RegionId=rg.RegionId
	 left join  tbl_Group  gr  WITH (NOLOCK)   on gr.GroupId=rg.GroupId
		 where   DoctorId = @doctorId
	 

INSERT INTO dbo.tbl_DoctorTourPlanDetail
        ( 
          DoctorId ,
          TourPlanDate ,
          EmpInfoId ,
          DocTPMaster, TourTypeId,GroupId,RegionId,AreaId,TerritoryId,SubTerritoryId, MarketId ,DoctorProgramypeId_DV,[GroupName_DV]
           ,[RegionName_DV]
           ,[AreaName_DV]
           ,[TerritoryName_DV]
           ,[SubTerritoryName_DV]
           ,[MarketName_DV],[GroupCode_DV]
           ,[RegionCode_DV]
           ,[AreaCode_DV]
           ,[TerritoryCode_DV]
           ,[SubTerritoryCode_DV]
           ,[MarketCode_DV], SMCTypeId_DV,SMCType_DV,  CompanyId
        )
VALUES  (
		@doctorId,
		@TourDate,
		@empId,
		@tpMasterId ,@StationTypeId,@GroupId,@RegionId,@AreaId,@TerritoryId,@SubTerritoryId, @MarketId, @DoctorProgramypeId,@GroupName 
           ,@RegionName 
           ,@AreaName 
           ,@TerritoryName 
           ,@SubTerritoryName 
           ,@MarketName ,@GroupCode_Ord 
           ,@RegionCode_Ord 
           ,@AreaCode_Ord 
           ,@TerritoryCode_Ord 
           ,@SubTerritoryCode_Ord 
           ,@MarketCode_Ord, @SMCTypeId_DV, @SMCType_DV,@CompanyId


        )


 		DECLARE @Id INT
 set @Id=@tpMasterId
		 


--DECLARE @Role NVARCHAR(MAX)
--SELECT @Role=usR.RoleName, @empId=emp.EmpInfoId
--FROM dbo.tblEmpGeneralInfo  emp
--left join  tblUser us on us.EmpInfoId=emp.EmpInfoId
--left join tbl_UserRoleInfo usR on usR.UserRoleID=us.UserRoleID
 
-- WHERE us.EmpInfoId=@empId


--DECLARE @ToEmpId1 INT
--DECLARE @GroupId1 INT
--DECLARE @RegionId1 INT
--DECLARE @AreaId1 INT
--DECLARE @TerrId1 INT

--DECLARE @EntryTime TIME(7)=cast(GETDATE() as time)
----SELECT * FROM dbo.View_webapi_FieldForce
----LEFT JOIN dbo.tblMIOInfo ON 

--IF(@Role='MIO')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE MIOEmpId=@empId
--END
--IF(@Role='ASM')
--BEGIN
--SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE ASMEMPId=@empId
--END
--IF(@Role='RSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE RSMEMPId=@empId
--END
--IF(@Role='NSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE NSMEMPId=@empId
--END

--DECLARE @EDate DATETIME=GETDATE()
	
--EXECUTE dbo.sp_webapi_SaveVisitPlanAppLog @VisitPlanApprovalId = 0,                         -- int
--                                 @Date = @EDate,           -- datetime
--                                 @FromEmpId = @empId,                          -- int
--                                 @ToEmpId = 0,                            -- int
--                                 @TableId = @Id,                            -- int
--                                 @Status = N'0',                           -- nvarchar(max)
--                                 @Comments = N'',                         -- nvarchar(max)
--                                 @Type = N'TourPlan',                             -- nvarchar(max)
--                                 @Step = 1,                               -- int
--                                 @GroupId = @GroupId1,                            -- int
--                                 @RegionId = @RegionId1,                           -- int
--                                 @AreaId = @AreaId1,                             -- int
--                                 @TerritoryId = @TerrId1,                        -- int
--                                 @ToGroupId = 0,                          -- int
--                                 @ToRegionId = 0,                         -- int
--                                 @ToAreaId = 0,                           -- int
--                                 @ToTerritoryId = 0,                      -- int
--                                 @EntryByS = @empId,                         -- nvarchar(max)
--                                 @EntryDateS = @EDate,     -- datetime
--                                 @EntryTimeS = @EntryTime,                -- time(7)
--                                 @ApproveByS = NULL,                       -- nvarchar(max)
--                                 @ApproveDateS = NULL,   -- datetime
--                                 @ApproveTimeS = NULL,              -- time(7)
--                                 @EntryByApp = @empId,                       -- nvarchar(max)
--                                 @EntryDateApp = @EDate,   -- datetime
--                                 @EntryTimeApp = @EntryTime,              -- time(7)
--                                 @ApproveByApp = NULL,                     -- nvarchar(max)
--                                 @ApproveDateApp = NULL, -- datetime
--                                 @ApproveTimeApp = NULL,            -- time(7)
--                                 @MenuId = 377                           -- int


--    SELECT  @Id



    End
set	@tpMasterId=0
    End

```

## sp_Webapi_Save_UserDeviceToken

- **Parameters**: `@username NVARCHAR(50)`, `@password NVARCHAR(50)`, `@DeviceToken NVARCHAR(MAX) = NULL`, `@Device NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tblUserDeviceToken` (duplicate check)
- **Tables written**: `tblUserDeviceToken` (INSERT, conditional)
- **Business logic**: Mobile-app login/device-registration: validates username/password against `tblUser` (plaintext password comparison, active status only); if valid and the device token isn't already registered for that user, inserts a new device-token row (for push notifications).
- **Transaction/error handling**: none
- **Result set**: single scalar (new `tblUserDeviceToken` identity) only if credentials are valid and the token is new; no result set otherwise
- **Caller**: UNUSED — no application caller found

```sql

create PROCEDURE [dbo].[sp_Webapi_Save_UserDeviceToken]
	-- Add the parameters for the stored procedure here
    @username NVARCHAR(50) ,
    @password NVARCHAR(50),
    @DeviceToken NVARCHAR(MAX)=NULL,
	@Device NVARCHAR(MAX)=NULL
AS
    BEGIN
	
	
	DECLARE @UserId INT , @EmpInfoId INT , @Count INT ,
	   @CountData int
SELECT @CountData=COUNT(*) FROM tblUser A
	WHERE   A.LoginName = @username
                AND A.Password = @password
                AND A.UserStatus = 'Active'

print @CountData
 IF(@CountData=1)
 BEGIN 

 
SELECT @UserId=A.UserId, @EmpInfoId= A.EmpInfoId	 FROM    tblUser A
	WHERE   A.LoginName = @username
                AND A.Password = @password
                AND A.UserStatus = 'Active'
  SELECT @Count=ISNULL(COUNT(*),0) FROM dbo.tblUserDeviceToken WHERE UserId = @UserId     AND  DeviceToken=@DeviceToken
  IF(@Count=0)
	BEGIN

       INSERT INTO [dbo].[tblUserDeviceToken]
           ([UserId]
           ,[EmpInfoId]
           ,[DeviceToken], EntryDate, Device)
     VALUES
           (@UserId 
           ,@EmpInfoId 
           ,@DeviceToken,GETDATE(), @Device)

SELECT SCOPE_IDENTITY()

    END
    END

	end	
```

## sp_Webapi_Save_UserTracking

- **Parameters**: `@empId INT = NULL`, `@latv NVARCHAR(MAX) = NULL`, `@longv NVARCHAR(MAX) = NULL`, `@addressName NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`, `ZAS_UserFS_MIO`/`ZAS_UserFS_AM`/`ZAS_UserFS_RSM`, `tblEmpGeneralInfo`
- **Tables written**: `tbl_UserTracking` (INSERT)
- **Business logic**: Records one GPS breadcrumb point for an employee (lat/long/address/time), resolving and denormalizing the employee's company and org hierarchy by role; always inserted with `ActiveStatus = 1` (active/moving).
- **Transaction/error handling**: none
- **Result set**: single scalar (new identity)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Save_UserTracking]
	-- Add the parameters for the stored procedure here
    @empId INT =NULL,
    @latv NVARCHAR(MAX) =NULL,
    @longv NVARCHAR(MAX) =NULL,
    @addressName NVARCHAR(MAX)=NULL
AS
    BEGIN


	declare @CompanyId int=0, @Roltp nvarchar(max),
	@GroupId int=null,  @GroupCode   nvarchar(max),  @GroupName  nvarchar(max),@RegionId int=null, @RegionCode  nvarchar(max), @RegionName  nvarchar(max),@AreaId int=null, @AreaCode  nvarchar(max), @AreaName  nvarchar(max), @TerritoryId int=null, @TerritoryCode  nvarchar(max),@TerritoryName  nvarchar(max) 
	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId

				if(@Roltp='MIO')
				begin
				 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_MIO  where EmpInfoId= @empId
				end


			 

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_AM  where EmpInfoId= @empId
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_RSM  where EmpInfoId= @empId
		end

			if(@CompanyId=0)
		begin
		select  @CompanyId=e.CompanyId   FROM dbo.tblEmpGeneralInfo e
	
			 
			  				
				  WHERE e.EmpInfoId=@empId
		end

        INSERT  INTO dbo.tbl_UserTracking
                ( EmpInfoId ,
                  LatValue ,
                  LongValue ,
                  AddressName ,
                  Time ,
                  TrackDate,[CompanyId]
      ,[GroupId]
      ,[GroupCode]
      ,[GroupName]
      ,[RegionId]
      ,[RegionCode]
      ,[RegionName]
      ,[AreaId]
      ,[AreaCode]
      ,[AreaName]
      ,[TerritoryId]
      ,[TerritoryCode]
      ,[TerritoryName], ActiveStatus
	            )
        VALUES  ( @empId ,
                  @latv ,
                  @longv ,
                  @addressName ,
                  LTRIM(RIGHT(CONVERT(VARCHAR(50), GETDATE(), 100), 7)) ,
                  GETDATE(), @CompanyId 
      ,@GroupId 
      ,@GroupCode 
      ,@GroupName 
      ,@RegionId 
      ,@RegionCode 
      ,@RegionName 
      ,@AreaId 
      ,@AreaCode 
      ,@AreaName 
      ,@TerritoryId 
      ,@TerritoryCode 
      ,@TerritoryName ,1
                )

SELECT SCOPE_IDENTITY()

    END

```

## sp_Webapi_Save_UserTrackingPunch

- **Parameters**: `@empId INT = NULL`, `@latv NVARCHAR(MAX) = NULL`, `@longv NVARCHAR(MAX) = NULL`, `@addressName NVARCHAR(MAX) = NULL`, `@AttTypeLoc NVARCHAR(MAX) = NULL`
- **Tables read**: same as `sp_Webapi_Save_UserTracking`
- **Tables written**: `tbl_UserTracking` (INSERT)
- **Business logic**: Same as `sp_Webapi_Save_UserTracking`, but tags the tracking point with an `AttTypeLoc` (attendance-type location, e.g. punch-in/out location) instead of a fixed active status.
- **Transaction/error handling**: none
- **Result set**: single scalar (new identity)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Save_UserTrackingPunch]
	-- Add the parameters for the stored procedure here
    @empId INT =NULL,
    @latv NVARCHAR(MAX) =NULL,
    @longv NVARCHAR(MAX) =NULL,
    @addressName NVARCHAR(MAX)=NULL,
	@AttTypeLoc NVARCHAR(MAX)=NULL
AS
    BEGIN


	declare @CompanyId int=0, @Roltp nvarchar(max),
	@GroupId int=null,  @GroupCode   nvarchar(max),  @GroupName  nvarchar(max),@RegionId int=null, @RegionCode  nvarchar(max), @RegionName  nvarchar(max),@AreaId int=null, @AreaCode  nvarchar(max), @AreaName  nvarchar(max), @TerritoryId int=null, @TerritoryCode  nvarchar(max),@TerritoryName  nvarchar(max) 
	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId

				if(@Roltp='MIO')
				begin
				 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_MIO  where EmpInfoId= @empId
				end

 

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_AM  where EmpInfoId= @empId
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_RSM  where EmpInfoId= @empId
		end
			if(@CompanyId=0)
		begin
		select  @CompanyId=e.CompanyId   FROM dbo.tblEmpGeneralInfo e
	
			 
			  				
				  WHERE e.EmpInfoId=@empId
		end


        INSERT  INTO dbo.tbl_UserTracking
                ( EmpInfoId ,
                  LatValue ,
                  LongValue ,
                  AddressName ,
                  Time ,
                  TrackDate,[CompanyId]
      ,[GroupId]
      ,[GroupCode]
      ,[GroupName]
      ,[RegionId]
      ,[RegionCode]
      ,[RegionName]
      ,[AreaId]
      ,[AreaCode]
      ,[AreaName]
      ,[TerritoryId]
      ,[TerritoryCode]
      ,[TerritoryName],  AttTypeLoc
	            )
        VALUES  ( @empId ,
                  @latv ,
                  @longv ,
                  @addressName ,
                  LTRIM(RIGHT(CONVERT(VARCHAR(50), GETDATE(), 100), 7)) ,
                  GETDATE(), @CompanyId 
      ,@GroupId 
      ,@GroupCode 
      ,@GroupName 
      ,@RegionId 
      ,@RegionCode 
      ,@RegionName 
      ,@AreaId 
      ,@AreaCode 
      ,@AreaName 
      ,@TerritoryId 
      ,@TerritoryCode 
      ,@TerritoryName , @AttTypeLoc
                )

SELECT SCOPE_IDENTITY()

    END

```

## sp_Webapi_Save_UserTrackingStatus

- **Parameters**: `@empId INT = NULL`, `@latv NVARCHAR(MAX) = NULL`, `@longv NVARCHAR(MAX) = NULL`, `@addressName NVARCHAR(MAX) = NULL`, `@breakReason NVARCHAR(MAX) = NULL`, `@ActiveStatus BIT = NULL`
- **Tables read**: same as `sp_Webapi_Save_UserTracking`
- **Tables written**: `tbl_UserTracking` (INSERT)
- **Business logic**: Same as `sp_Webapi_Save_UserTracking`, but records an explicit active/break status and break reason (used for the mobile app's "take a break" / "resume" tracking events).
- **Transaction/error handling**: none
- **Result set**: single scalar (new identity)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_Webapi_Save_UserTrackingStatus]
	-- Add the parameters for the stored procedure here
    @empId INT =NULL,
    @latv NVARCHAR(MAX) =NULL,
    @longv NVARCHAR(MAX) =NULL,
    @addressName NVARCHAR(MAX)=NULL,
    @breakReason NVARCHAR(MAX)=NULL,
    @ActiveStatus bit=NULL


AS
    BEGIN


	declare @CompanyId int=0, @Roltp nvarchar(max),
	@GroupId int=null,  @GroupCode   nvarchar(max),  @GroupName  nvarchar(max),@RegionId int=null, @RegionCode  nvarchar(max), @RegionName  nvarchar(max),@AreaId int=null, @AreaCode  nvarchar(max), @AreaName  nvarchar(max), @TerritoryId int=null, @TerritoryCode  nvarchar(max),@TerritoryName  nvarchar(max) 
	select @Roltp=RoleType   FROM dbo.tblUser  A
	
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId

				if(@Roltp='MIO')
				begin
				 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_MIO  where EmpInfoId= @empId
				end

 

					if(@Roltp='AM')
		begin 
		 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_AM  where EmpInfoId= @empId
		end
 	if(@Roltp='DZSM')

	begin 
		 select  @CompanyId=ISNULL(CompanyId,0), @TerritoryId= TerritoryId,@TerritoryCode= TerritoryCode, @TerritoryName=TerritoryName,  @AreaId=AreaId, @AreaCode= AreaCode, @AreaName= AreaName, @RegionId= RegionId, @RegionCode= RegionCode, @RegionName= RegionName,@GroupId=  GroupId, @GroupCode= GroupCode,@GroupName=  GroupName from 	ZAS_UserFS_RSM  where EmpInfoId= @empId
		end

		if(@CompanyId=0)
		begin
		select  @CompanyId=e.CompanyId   FROM dbo.tblEmpGeneralInfo e
	
			 
			  				
				  WHERE e.EmpInfoId=@empId
		end
        INSERT  INTO dbo.tbl_UserTracking
                ( EmpInfoId ,
                  LatValue ,
                  LongValue ,
                  AddressName ,
                  Time ,
                  TrackDate,[CompanyId]
      ,[GroupId]
      ,[GroupCode]
      ,[GroupName]
      ,[RegionId]
      ,[RegionCode]
      ,[RegionName]
      ,[AreaId]
      ,[AreaCode]
      ,[AreaName]
      ,[TerritoryId]
      ,[TerritoryCode]
      ,[TerritoryName],  breakReason, ActiveStatus
	            )
        VALUES  ( @empId ,
                  @latv ,
                  @longv ,
                  @addressName ,
                  LTRIM(RIGHT(CONVERT(VARCHAR(50), GETDATE(), 100), 7)) ,
                  GETDATE(), @CompanyId 
      ,@GroupId 
      ,@GroupCode 
      ,@GroupName 
      ,@RegionId 
      ,@RegionCode 
      ,@RegionName 
      ,@AreaId 
      ,@AreaCode 
      ,@AreaName 
      ,@TerritoryId 
      ,@TerritoryCode 
      ,@TerritoryName,  @breakReason, @ActiveStatus
                )

SELECT SCOPE_IDENTITY()

    END

```

## sp_Webapi_UD_DoctorEntry

- **Parameters**: `@MasterId INT = NULL` (doctor id), `@doctorName NVARCHAR(MAX) = NULL`, `@remarks NVARCHAR(MAX) = NULL`, `@entryBy INT = NULL`, `@EntryDate DATETIME = NULL` (unused), `@contactTypeId INT = NULL` (unused), `@contactInfo NVARCHAR(MAX) = NULL` (unused), `@subMarketId INT = NULL` (unused), `@designaionId INT = NULL`, `@marketId INT = NULL`, `@TerritoryId INT = NULL` (unused), `@SubTerritoryId INT = NULL` (unused), `@DoctorCategoryId INT = NULL`, `@ProgramTypeId INT = NULL`, `@UnionName NVARCHAR(MAX) = NULL`, `@StationTypeId INT = NULL`, `@DoctorTypeId INT = NULL`, `@SMCTypeId INT = NULL`, `@DoctorAddress NVARCHAR(MAX) = NULL`
- **Tables read**: `tblUser`, `tblmarket`, `tblTerritory`, `tblArea`, `tblRegion`, `tblMarketStationDetail`
- **Tables written**: `tblDoctorMaster` (UPDATE), `tblDoctorMarketDetail`/`tblDoctorSpecialDayDetail`/`tblDoctorContactDetail`/`tblDoctorDegreeDetail`/`tblDoctorBrandDetail`/`tblDoctorSpecialityDetail`/`tblDoctorInstitutionDetail`/`tblDoctorProgramTypeDetail`/`tblDoctorChemberDetail` (all DELETE, then `tblDoctorMarketDetail` re-INSERT only)
- **Business logic**: "Update Doctor" (UD = update/delete) entry: updates the doctor master row, then wipes every doctor child-detail table for that doctor and re-inserts only the market link — the other child tables (special-day, contact, degree, brand, speciality, institution, program-type, chamber) are cleared but never repopulated in this procedure, since all their would-be re-insert blocks are commented out (dead code) — meaning a call to this proc silently drops those child details.
- **Transaction/error handling**: none (destructive delete/re-insert with no transaction wrapping it)
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
 
CREATE PROCEDURE [dbo].[sp_Webapi_UD_DoctorEntry]
	-- Add the parameters for the stored procedure here
   @MasterId INT=NULL,
   @doctorName NVARCHAR(MAX) = NULL ,
   
    @remarks NVARCHAR(MAX) = NULL ,
    @entryBy INT = NULL ,
    @EntryDate DATETIME = NULL ,
   
    @contactTypeId INT = NULL ,
    @contactInfo NVARCHAR(MAX) = NULL ,
    @subMarketId INT = NULL,
	@designaionId INT =NULL,
	@marketId INT = NULL,
	@TerritoryId  INT = NULL,
	@SubTerritoryId  INT = NULL,
	@DoctorCategoryId  INT = NULL,
	@ProgramTypeId  INT = NULL,
	@UnionName NVARCHAR(MAX) = NULL ,
	@StationTypeId  INT = NULL ,
	@DoctorTypeId  INT = NULL  ,
    @SMCTypeId INT = NULL ,
	@DoctorAddress NVARCHAR(MAX) = NULL
	--@SpecialDayId  INT = NULL ,
	--@SpeciaDateStr date = NULL 
AS
    BEGIN



	 DECLARE @userId int
		SELECT @userId = UserId FROM dbo.tblUser WHERE EmpInfoId = @entryBy
	 DECLARE @RouteInformationMasterId int ,	@StationTypeId_ INT, @GroupId INT,@RegionId INT,@AreaId INT,@TerritoryId_ INT,@SubTerritoryId_ INT, @divId int ,@disId int ,@thanaId int 
		SELECT   @TerritoryId_=tr.TerritoryId,@AreaId=ar.AreaId,@RegionId=rg.RegionId,@GroupId=rg.GroupId from tblmarket mr with (nolock)
	 
		inner join tblTerritory tr  with (nolock) on mr.TerritoryId=tr.TerritoryId
		inner join tblArea ar   with (nolock)  on ar.AreaId=tr.AreaId
		inner join tblRegion rg  with (nolock) on ar.RegionId=rg.RegionId
		 
		 
		  where  MarketId=@MarketId

	 select @StationTypeId_= ISNULL(StationTypeId,null) from tblMarketStationDetail where MarketId=@marketId

		  

		 

	UPDATE tblDoctorMaster
	SET DoctorName=@DoctorName ,
               
                  Remarks =@remarks,
                
                  UpdateBy=@userId ,
                  UpdateDate=GETDATE() ,
                
				 
				  DesignationId=@designaionId,
				 
				      MarketId=@MarketId
       ,[UnionName]=@UnionName
      
      ,[StationTypeId]=@StationTypeId
      ,[ProgramTypeId]=@ProgramTypeId
      ,[DoctorCategoryId]=@DoctorCategoryId, DoctorTypeId=@DoctorTypeId,SMCTypeId=@SMCTypeId,DoctorAddress=@DoctorAddress
      
     
	WHERE DoctorId = @MasterId


 
	DELETE FROM tblDoctorMarketDetail WHERE DoctorId=@MasterId
	DELETE FROM tblDoctorSpecialDayDetail WHERE DoctorId=@MasterId

	DELETE FROM tblDoctorContactDetail WHERE DoctorId=@MasterId
	DELETE FROM dbo.tblDoctorDegreeDetail WHERE DoctorId=@MasterId
	DELETE FROM dbo.tblDoctorBrandDetail WHERE DoctorId=@MasterId
	DELETE FROM dbo.tblDoctorSpecialityDetail WHERE DoctorId=@MasterId
	DELETE FROM dbo.tblDoctorInstitutionDetail WHERE DoctorId=@MasterId
	DELETE FROM dbo.tblDoctorProgramTypeDetail WHERE DoctorId=@MasterId
	DELETE FROM dbo.tblDoctorChemberDetail WHERE DoctorId=@MasterId







	 IF ( @marketId IS NOT NULL )
            BEGIN
                INSERT  INTO dbo.tblDoctorMarketDetail
                        ( DoctorId, MarketId )
                VALUES  ( @MasterId, @marketId )
            END

			   --IF (@SpeciaDateStr IS  not NULL )
      --      BEGIN
      --          INSERT  INTO dbo.tblDoctorSpecialDayDetail
      --                  ( DoctorId, SpecialDayId,SpecialDate )
      --          VALUES  ( @MasterId, @SpecialDayId,@SpeciaDateStr )
      --      END
		 


--        --IF ( @chamberId IS NOT NULL )
--        --    BEGIN
--        --        INSERT  INTO dbo.tblDoctorChemberDetail
--        --                ( ChamberTypeId ,
--        --                  DoctorId ,
--        --                  Name 
--        --                )
--        --        VALUES  ( @chamberId ,
--        --                  @doctorId ,
--        --                  @chamberAddress
--        --                )
	
--        --    END

        --IF ( @contactTypeId IS NOT NULL )
        --    BEGIN
        --        INSERT  INTO dbo.tblDoctorContactDetail
        --                ( DoctorId ,
        --                  ContactTypeId ,
        --                  Contact 
	       --             )
        --        VALUES  ( @MasterId ,
        --                  @contactTypeId ,
        --                  @contactInfo
	       --             )
        --    END


 END

```

## sp_Webapi_UpdateNotificationRead

- **Parameters**: `@id INT`
- **Tables read**: none
- **Tables written**: `tbl_Notification` (UPDATE)
- **Business logic**: Marks a mobile-app notification as read.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_UpdateNotificationRead]
	-- Add the parameters for the stored procedure here
@id int
AS
BEGIN
		
		UPDATE dbo.tbl_Notification SET IsRead = 1 WHERE NotificationId= @id

END
```

## sp_Webapi_Update_DoctorFinalSubmit

- **Parameters**: `@empId INT = NULL`, `@month INT = NULL`, `@year INT = NULL`, `@remarks NVARCHAR(MAX) = NULL`
- **Tables read**: `tbl_DoctorTourPlanMaster`
- **Tables written**: `tbl_DoctorTourPlanMaster` (UPDATE)
- **Business logic**: Finalizes an employee's monthly doctor tour plan — sets `IsFinalSubmit = 0` (note: sets it to 0, not 1, despite the name), records remarks, and sets `ApprovalStatus = '2'`. Contains a large commented-out approval-log posting block (dead code).
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Update_DoctorFinalSubmit]
	-- Add the parameters for the stored procedure here
    @empId INT = NULL ,
    @month INT = NULL ,
    @year INT = NULL ,
    @remarks NVARCHAR(MAX) = NULL
AS
    BEGIN
        UPDATE  dbo.tbl_DoctorTourPlanMaster
        SET     IsFinalSubmit = 0 ,
                FinalSubmitRemarks = @remarks ,
                ApprovalStatus = '2'
        WHERE   EmpInfoId = @empId
                AND MonthValue = @month
                AND YearValue = @year

					DECLARE @MasterId INT=0
			SELECT @MasterId=DocTPMaster FROM dbo.tbl_DoctorTourPlanMaster WHERE EmpInfoId = @empId AND MonthValue = @month AND YearValue = @year
				 
		 


--DECLARE @Role NVARCHAR(MAX)
--SELECT @Role=usR.RoleName, @empId=emp.EmpInfoId
--FROM dbo.tblEmpGeneralInfo  emp
--left join  tblUser us on us.EmpInfoId=emp.EmpInfoId
--left join tbl_UserRoleInfo usR on usR.UserRoleID=us.UserRoleID
 
-- WHERE us.EmpInfoId=@empId


--DECLARE @ToEmpId1 INT
--DECLARE @GroupId1 INT
--DECLARE @RegionId1 INT
--DECLARE @AreaId1 INT
--DECLARE @TerrId1 INT

--DECLARE @EntryTime TIME(7)=cast(GETDATE() as time)
----SELECT * FROM dbo.View_webapi_FieldForce
----LEFT JOIN dbo.tblMIOInfo ON 

--IF(@Role='MIO')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE MIOEmpId=@empId
--END
--IF(@Role='ASM')
--BEGIN
--SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE ASMEMPId=@empId
--END
--IF(@Role='RSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE RSMEMPId=@empId
--END
--IF(@Role='NSM')
--BEGIN
--    SELECT @GroupId1=GroupId,@AreaId1=AreaId,@TerrId1=TerritoryId,@RegionId1=RegionId FROM dbo.View_webapi_FieldForce WHERE NSMEMPId=@empId
--END

--DECLARE @EDate DATETIME=GETDATE()
	
--EXECUTE dbo.sp_webapi_SaveVisitPlanAppLog @VisitPlanApprovalId = 0,                         -- int
--                                 @Date = @EDate,           -- datetime
--                                 @FromEmpId = @empId,                          -- int
--                                 @ToEmpId = 0,                            -- int
--                                 @TableId = @MasterId,                            -- int
--                                 @Status = N'0',                           -- nvarchar(max)
--                                 @Comments = N'',                         -- nvarchar(max)
--                                 @Type = N'TourPlan',                             -- nvarchar(max)
--                                 @Step = 1,                               -- int
--                                 @GroupId = @GroupId1,                            -- int
--                                 @RegionId = @RegionId1,                           -- int
--                                 @AreaId = @AreaId1,                             -- int
--                                 @TerritoryId = @TerrId1,                        -- int
--                                 @ToGroupId = 0,                          -- int
--                                 @ToRegionId = 0,                         -- int
--                                 @ToAreaId = 0,                           -- int
--                                 @ToTerritoryId = 0,                      -- int
--                                 @EntryByS = @empId,                         -- nvarchar(max)
--                                 @EntryDateS = @EDate,     -- datetime
--                                 @EntryTimeS = @EntryTime,                -- time(7)
--                                 @ApproveByS = NULL,                       -- nvarchar(max)
--                                 @ApproveDateS = NULL,   -- datetime
--                                 @ApproveTimeS = NULL,              -- time(7)
--                                 @EntryByApp = @empId,                       -- nvarchar(max)
--                                 @EntryDateApp = @EDate,   -- datetime
--                                 @EntryTimeApp = @EntryTime,              -- time(7)
--                                 @ApproveByApp = NULL,                     -- nvarchar(max)
--                                 @ApproveDateApp = NULL, -- datetime
--                                 @ApproveTimeApp = NULL,            -- time(7)
--                                 @MenuId = 377                           -- int



    END


```

## sp_Webapi_delete_TourPlanData

- **Parameters**: `@id INT`
- **Tables read**: none
- **Tables written**: `tbl_TourPlanInfo` (DELETE)
- **Business logic**: Deletes a customer/market tour-plan entry by id.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_Webapi_delete_TourPlanData]
	-- Add the parameters for the stored procedure here
@id int
AS
BEGIN
	
		DELETE FROM dbo.tbl_TourPlanInfo WHERE TourPlanId = @id
END
```

## sp_check_DoctorCategory

- **Parameters**: `@CategoryId INT = 0`, `@CategoryName NVARCHAR(MAX)`
- **Tables read**: `tblDoctorCategory`
- **Tables written**: none
- **Business logic**: Duplicate-name check for doctor categories — returns any other category row with the same name (excluding the current id), for use as an "already exists" validation before insert/update.
- **Transaction/error handling**: none
- **Result set**: matching `tblDoctorCategory` rows
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_check_DoctorCategory]
	-- Add the parameters for the stored procedure here
	  @CategoryId INT = 0 ,
    @CategoryName NVARCHAR(MAX) 
AS
BEGIN
		 
		SELECT * FROM dbo.tblDoctorCategory WHERE CategoryName=@CategoryName AND     CategoryId NOT IN ( @CategoryId)

END



```

## sp_check_DoctorDegree

- **Parameters**: `@DegreeId INT = 0`, `@DoctorTypeId INT = NULL` (unused in body), `@DegreeName NVARCHAR(MAX)`
- **Tables read**: `tblDoctorDegree`
- **Tables written**: none
- **Business logic**: Duplicate-name check for doctor degrees (same pattern as `sp_check_DoctorCategory`).
- **Transaction/error handling**: none
- **Result set**: matching `tblDoctorDegree` rows
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_check_DoctorDegree]
	-- Add the parameters for the stored procedure here
	  @DegreeId INT = 0 ,
	@DoctorTypeId int=null,

    @DegreeName NVARCHAR(MAX) 
AS
BEGIN
		 
		SELECT * FROM dbo.tblDoctorDegree WHERE DegreeName=@DegreeName   and   DegreeId NOT IN ( @DegreeId)

END



```

## sp_check_DoctorDesignation

- **Parameters**: `@DesignationId INT = 0`, `@DesignationName NVARCHAR(MAX)`
- **Tables read**: `tblDoctorDesignation`
- **Tables written**: none
- **Business logic**: Duplicate-name check for doctor designations.
- **Transaction/error handling**: none
- **Result set**: matching `tblDoctorDesignation` rows
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_check_DoctorDesignation]
	-- Add the parameters for the stored procedure here
	  @DesignationId INT = 0 ,
    @DesignationName NVARCHAR(MAX) 
AS
BEGIN
		 
		SELECT * FROM dbo.tblDoctorDesignation WHERE DesignationName=@DesignationName AND    DesignationId NOT IN ( @DesignationId)

END



```

## sp_check_DoctorSpeciality

- **Parameters**: `@SpecialityId INT = 0`, `@SpecialityName NVARCHAR(MAX)`
- **Tables read**: `tblDoctorSpeciality`
- **Tables written**: none
- **Business logic**: Duplicate-name check for doctor specialities.
- **Transaction/error handling**: none
- **Result set**: matching `tblDoctorSpeciality` rows
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_check_DoctorSpeciality]
	-- Add the parameters for the stored procedure here
	  @SpecialityId INT = 0 ,
    @SpecialityName NVARCHAR(MAX) 
AS
BEGIN
		 
		SELECT * FROM dbo.tblDoctorSpeciality WHERE SpecialityName=@SpecialityName AND     SpecialityId NOT IN ( @SpecialityId)

END



```

## sp_check_Employee_ShiftInfos

- **Parameters**: `@ShiftId INT = 0`, `@ShiftText NVARCHAR(MAX)`
- **Tables read**: `tbl_Shift`
- **Tables written**: none
- **Business logic**: Duplicate-name check for shift definitions.
- **Transaction/error handling**: none
- **Result set**: matching `tbl_Shift` rows
- **Caller**: UNUSED — no application caller found

```sql

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_check_Employee_ShiftInfos]
	-- Add the parameters for the stored procedure here
	  @ShiftId INT = 0 ,
    @ShiftText NVARCHAR(MAX) 
AS
BEGIN
		 
		SELECT * FROM dbo.tbl_Shift WHERE ShiftText=@ShiftText AND    ShiftId NOT IN ( @ShiftId)

END



```

## sp_check_UserInfo

- **Parameters**: `@UserId INT`, `@LoginName NVARCHAR(MAX) = NULL`, `@EmpInfoId INT = NULL`
- **Tables read**: `tblUser`
- **Tables written**: none
- **Business logic**: Duplicate-user check when editing an existing user — returns any other active user with the same login name, unioned with any other active user already linked to the same employee (excluding the current `@UserId`), so the caller can block saving a conflicting login/employee link.
- **Transaction/error handling**: none
- **Result set**: matching `tblUser` rows
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_check_UserInfo]
	-- Add the parameters for the stored procedure here
	  @UserId  INT ,
	  --@UserName  nvarchar(max) =null,
	  @LoginName  nvarchar(max) =null ,
	  @EmpInfoId  int =null 

	   
AS
BEGIN
		 
	--SELECT * FROM dbo.tblUser WHERE UserName=@UserName AND  UserId NOT IN ( @UserId)

 	SELECT * FROM dbo.tblUser WHERE  LoginName=@LoginName AND  UserId NOT IN ( @UserId) and UserStatus='Active'


union all	SELECT * FROM dbo.tblUser WHERE   EmpInfoId=@EmpInfoId AND EmpInfoId is not null and  UserId NOT IN ( @UserId) and UserStatus='Active'



END

```

## sp_check_UserInfo_Save

- **Parameters**: `@UserId INT` (unused in body), `@LoginName NVARCHAR(MAX) = NULL`, `@EmpInfoId INT = NULL`
- **Tables read**: `tblUser`
- **Tables written**: none
- **Business logic**: Same duplicate-user check as `sp_check_UserInfo` but for a new-user save flow — omits the `UserId NOT IN` exclusion since there's no existing user id yet.
- **Transaction/error handling**: none
- **Result set**: matching `tblUser` rows
- **Caller**: UNUSED — no application caller found

```sql
create PROCEDURE [dbo].[sp_check_UserInfo_Save]
	-- Add the parameters for the stored procedure here
	  @UserId  INT ,
	  --@UserName  nvarchar(max) =null,
	  @LoginName  nvarchar(max) =null ,
	  @EmpInfoId  int =null 

	   
AS
BEGIN
		 
	--SELECT * FROM dbo.tblUser WHERE UserName=@UserName 

 	SELECT * FROM dbo.tblUser WHERE  LoginName=@LoginName and UserStatus='Active' 


union all	SELECT * FROM dbo.tblUser WHERE   EmpInfoId=@EmpInfoId AND EmpInfoId is not null   and UserStatus='Active' 



END



```

## sp_check_UserInfo_Save_new

- **Parameters**: `@UserType NVARCHAR(MAX) = NULL`, `@LoginName NVARCHAR(MAX) = NULL`, `@EmpInfoId INT = NULL`
- **Tables read**: `tblUser`
- **Tables written**: none
- **Business logic**: Variant of `sp_check_UserInfo_Save` that only checks the employee-link conflict when `@UserType = 'Admin'` (admin users can be created without a linked employee, everyone else's check is login-name only).
- **Transaction/error handling**: none
- **Result set**: matching `tblUser` rows
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_check_UserInfo_Save_new]
	-- Add the parameters for the stored procedure here
	  @UserType   nvarchar(max) =null ,
	  --@UserName  nvarchar(max) =null,
	  @LoginName  nvarchar(max) =null ,
	  @EmpInfoId  int =null 

	   
AS
BEGIN
		 
	--SELECT * FROM dbo.tblUser WHERE UserName=@UserName 

	if(@UserType= 'Admin' )
	begin

	SELECT * FROM dbo.tblUser WHERE  LoginName=@LoginName and UserStatus='Active' 


union all	SELECT * FROM dbo.tblUser WHERE   EmpInfoId=@EmpInfoId AND EmpInfoId is not null   and UserStatus='Active' 

	end
else

 	begin

	SELECT * FROM dbo.tblUser WHERE  LoginName=@LoginName and UserStatus='Active' 

	 

	end


END



```

## sp_check_UserRoleInfo

- **Parameters**: `@UserRoleID INT`, `@RoleName NVARCHAR(MAX)`
- **Tables read**: `tbl_UserRoleInfo`
- **Tables written**: none
- **Business logic**: Duplicate-name check for user roles.
- **Transaction/error handling**: none
- **Result set**: matching `tbl_UserRoleInfo` rows
- **Caller**: UNUSED — no application caller found

```sql


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_check_UserRoleInfo]
	-- Add the parameters for the stored procedure here
	  @UserRoleID  INT ,
	  @RoleName  nvarchar(max)  
AS
BEGIN
		 
	SELECT * FROM dbo.tbl_UserRoleInfo WHERE RoleName=@RoleName AND  UserRoleID NOT IN ( @UserRoleID)

END




```

## sp_webapi_GetAllProducts

- **Parameters**: `@empId INT`
- **Tables read**: `tblUser`, `tblEmpGeneralInfo`, `tbl_UserRoleInfo`, `tblRoleType`, `tblMIOInfo`, `tblASMInfo`, `tblRSMInfo`, `tblCompanyInfo`, `tblProduct`, `tblUnitPrice`, `tblProductQuotedPrice`
- **Tables written**: none
- **Business logic**: Resolves the employee's company by role (MIO/AM/DZSM) and returns all active-priced products for that company, including any customer-specific quoted price if one exists.
- **Transaction/error handling**: none
- **Result set**: `CompanyId`, `ProductId`, `ProductCode`, `ProductName`, `Description`, `PackSize`, `UnitPrice`, `VATPercentage`, `VATAmountPerUnit`, `QuotedPrice`, `CustomerMasterId`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetAllProducts]
	-- Add the parameters for the stored procedure here
	@empId int
AS
BEGIN

	Declare @Roltp nvarchar(max)=''
 declare @ComId int=0 
 select @Roltp=RoleType   FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId


if(@Roltp='MIO')
		begin 
		select @ComId=CompanyId from tblMIOInfo where EmployeeId=@EMPId and IsActive=1
		end

			if(@Roltp='AM')
		begin 
		select @ComId=CompanyId from tblASMInfo where EmployeeId=@EMPId and IsActive=1
		end
 	if(@Roltp='DZSM')

	begin 
		select @ComId=CompanyId from tblRSMInfo where EmployeeId=@EMPId and IsActive=1
		end

SELECT  @ComId CompanyId, c.ProductId,
		C.ProductCode,
		C.ProductName,
		C.Description,
		D.PackSize,
		D.UnitPrice,
		D.VATPercentage,
		D.VATAmountPerUnit,
		ISNULL((SELECT TOP 1 QuotedPrice FROM dbo.tblProductQuotedPrice WHERE ProductId = C.ProductId AND IsActive = 1 ORDER BY QuotedPriceId DESc),0) AS QuotedPrice,
		ISNULL((SELECT TOP 1 CustomerMasterId FROM dbo.tblProductQuotedPrice WHERE ProductId = C.ProductId AND IsActive = 1 ORDER BY QuotedPriceId DESc),0) AS CustomerMasterId


 FROM dbo.tblEmpGeneralInfo A 
INNER JOIN dbo.tblCompanyInfo B ON B.CompanyId = @ComId
INNER JOIN dbo.tblProduct C ON C.CompanyId = B.CompanyId
INNER JOIN dbo.tblUnitPrice D ON C.ProductId = D.ProductId
WHERE A.EmpInfoId = @empId AND D.IsActive = 1
		
END
```

## sp_webapi_GetAllSampleProducts

- **Parameters**: `@empId INT`
- **Tables read**: `tblUser`, `tblEmpGeneralInfo`, `tbl_UserRoleInfo`, `tblRoleType`, `tblMIOInfo`, `tblASMInfo`, `tblRSMInfo`, `tblCompanyInfo`, `tblProduct`
- **Tables written**: none
- **Business logic**: Same company-resolution pattern as `sp_webapi_GetAllProducts`, but returns a simplified "sample" product list with pricing/pack fields hardcoded to 0 (used for free-sample distribution rather than sale).
- **Transaction/error handling**: none
- **Result set**: `IsActiveProduct`, `ProductId`, `ProductCode`, `ProductName`, `Description`, `PackSize` (0), `UnitPrice` (0), `VATPercentage` (0), `VATAmountPerUnit` (0), `QuotedPrice` (0), `CustomerMasterId` (0)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetAllSampleProducts] -- sp_webapi_GetAllSampleProducts 151
	-- Add the parameters for the stored procedure here
	@empId int
AS
BEGIN


	Declare @Roltp nvarchar(max)=''
 declare @ComId int=0 
 select @Roltp=RoleType   FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId


if(@Roltp='MIO')
		begin 
		select @ComId=CompanyId from tblMIOInfo where EmployeeId=@EMPId and IsActive=1
		end

			if(@Roltp='AM')
		begin 
		select @ComId=CompanyId from tblASMInfo where EmployeeId=@EMPId and IsActive=1
		end
 	if(@Roltp='DZSM')

	begin 
		select @ComId=CompanyId from tblRSMInfo where EmployeeId=@EMPId and IsActive=1
		end

SELECT distinct C.IsActiveProduct, c.ProductId,
		C.ProductCode,
		C.ProductName,
		C.Description,
		0 AS  PackSize,
		0 as UnitPrice,
		0 AS  VATPercentage,
		 0 VATAmountPerUnit,
		0 AS QuotedPrice,
	0 AS CustomerMasterId


 FROM dbo.tblEmpGeneralInfo A 

   
INNER JOIN dbo.tblCompanyInfo B ON B.CompanyId = @ComId
INNER JOIN dbo.tblProduct C  ON C .CompanyId = B.CompanyId 
 
WHERE A.EmpInfoId = @empId and C.IsActiveProduct = 1  
--AND A.EmpInfoId = @empId

		
END

```

## sp_webapi_GetCustomerbyUser

- **Parameters**: `@empId INT`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblCustMaster`, `tblMarket`, `tblTerritory`, `tblArea`, `tblRegion`, `tblCustCategory`, `tblInvoice`, `tblMIOInfo`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `View_Webapi_EmployeeFieldForceInfo`
- **Tables written**: none
- **Business logic**: Returns the active-customer picklist scoped by the caller's role hierarchy (territory/area-team/region/group), including each customer's most recent invoice date as a display note.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — market/customer identity and hierarchy ids, `CustomerName`, `CustomerCode`, `Address`, `MarketName`, `CellNo`, `CustomerType`, plus several always-NULL/0 placeholder columns (`CustomerStation`, `Route`, `Balance`, `CreditLimit`)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetCustomerbyUser]
	-- Add the parameters for the stored procedure here
    @empId INT
AS
    BEGIN

		    DECLARE @Q NVARCHAR(MAX)=''
	DECLARE @Param NVARCHAR(MAX)=''
	DECLARE @Id NVARCHAR(MAX)=0
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId

	SET @Q='

        SELECT distinct C.MarketCode, 0 IsSelect, D.CustomerTypeId, D.ProgramTypeId, D.SMCTypeId, 0 as CustomerCheck,format(tbInvo.InvoiceDate,''dd MMMM, yyyy'') Note, rg.GroupId GroupId, rg.RegionId RegionId, ar.AreaId as AreaId, tr.TerritoryId TerritoryId, 0 SubTerritoryId,C.MarketId MarketId, D.CustomerMasterId ,
                D.CustomerName ,
                D.CustomerCode ,
                D.Address ,
                C.MarketName ,
                D.CellNo ,
                E.CategoryName +ISNULL( '' [Payment Type: ''+d.TermOfPayment+'']'','''') AS CustomerType ,
                NULL AS ''CustomerStation'' ,
                NULL AS ''Route'' ,
                0 AS ''Balance'' ,
                0 AS ''CreditLimit'' ,
                NULL AS ''Route''
        FROM       dbo.tblCustMaster D
                INNER JOIN dbo.tblMarket C ON D.MarketId = C.MarketId
                INNER JOIN tblTerritory tr  ON C.TerritoryId = tr.TerritoryId
              left JOIN tblArea ar  ON ar.AreaId = tr.AreaId
                left JOIN tblRegion rg  ON ar.RegionId = rg.RegionId
                INNER JOIN dbo.tblCustCategory E ON E.CategoryId = D.CategoryId
					left join (
				select max(InvoiceDate) InvoiceDate, tblInvoice.CustomerMasterId from tblInvoice with (nolock)   group by CustomerMasterId) tbInvo on  tbInvo.CustomerMasterId=D.CustomerMasterId
        WHERE     D.IsActive = 1 '
		
		
			 DECLARE @TRID NVARCHAR(MAX)
	IF(@Roletype='1')
	BEGIN
		SELECT @Id=TerritoryId FROM dbo.tblMIOInfo WHERE EmployeeId=@empId and IsActive=1

	    SET @Param=@Param+' and tr.TerritoryId='+@Id
	END
	IF(@Roletype='2')
	BEGIN
	  	select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @Param=@Param+ ' and  tr.TerritoryId in ( '+ @TRID +')';
	END
	IF(@Roletype='3')
	BEGIN
	  select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')

	    SET @Param=@Param+' and tr.AreaId in ('+@Id+')'
	END
	IF(@Roletype='4')
	BEGIN
	    SELECT @Id=EmpGroupId FROM dbo.View_Webapi_EmployeeFieldForceInfo WHERE EmpInfoId=@empId

	    SET @Param=@Param+' and GroupId='+@Id
	END

	SET @Q=@Q+@Param

    EXEC sys.sp_executesql @Q

	end
```

## sp_webapi_GetCustomerbyUserDate

- **Parameters**: `@empId INT`, `@Date NVARCHAR(MAX)` (accepted but unused in the query itself)
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblMIOInfo`, `tblTerritory`, `tblMarket`, `tblCustMaster`, `tblCustCategory`, `tblInvoice`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`, `View_Webapi_EmployeeFieldForceInfo`
- **Tables written**: none
- **Business logic**: A variant customer picklist joined from the MIO's assigned territory rather than a direct market/territory join, scoped by role hierarchy; despite the name, `@Date` is not actually used to filter anything.
- **Transaction/error handling**: none
- **Result set**: one dynamic result set — similar shape to `sp_webapi_GetCustomerbyUser` (hierarchy ids hardcoded to 0 in this version)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetCustomerbyUserDate]
	-- Add the parameters for the stored procedure here
    @empId INT,
	@Date nvarchar(max)
AS
    BEGIN


	   DECLARE @Q NVARCHAR(MAX)=''
	DECLARE @Param NVARCHAR(MAX)=''
	DECLARE @Id NVARCHAR(MAX)=0
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId

	SET @Q='
        SELECT distinct C.MarketCode, 0 IsSelect, D.CustomerTypeId, D.ProgramTypeId, D.SMCTypeId, 0 as CustomerCheck,format(tbInvo.InvoiceDate,''dd MMMM, yyyy'') Note, 0 GroupId, 0 RegionId, 0 as AreaId, 0 TerritoryId, 0 SubTerritoryId,0 MarketId,  D.CustomerMasterId ,
                D.CustomerName ,
                D.CustomerCode ,
                D.Address ,
                C.MarketName ,
                D.CellNo ,
                E.CategoryName AS CustomerType ,
                NULL AS ''CustomerStation'' ,
                NULL AS ''Route'' ,
                0 AS ''Balance'' ,
                0 AS ''CreditLimit'' ,
                NULL AS ''Route''
        FROM    dbo.tblMIOInfo A
                INNER JOIN dbo.tblTerritory B ON B.TerritoryId = A.TerritoryId
                INNER JOIN dbo.tblMarket C ON C.TerritoryId = B.TerritoryId
                INNER JOIN dbo.tblCustMaster D ON D.MarketId = C.MarketId
                INNER JOIN dbo.tblCustCategory E ON E.CategoryId = D.CategoryId
					left join (
				select max(InvoiceDate) InvoiceDate, tblInvoice.CustomerMasterId from tblInvoice with (nolock)   group by CustomerMasterId) tbInvo on  tbInvo.CustomerMasterId=D.CustomerMasterId
        WHERE    A.IsActive = 1'

			 DECLARE @TRID NVARCHAR(MAX)
	IF(@Roletype='1')
	BEGIN
		SELECT @Id=TerritoryId FROM dbo.tblMIOInfo WHERE EmployeeId=@empId and IsActive=1

	    SET @Param=@Param+' and TerritoryId='+@Id
	END
	IF(@Roletype='2')
	BEGIN
	  	select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @Param=@Param+ ' and  TerritoryId in ( '+ @TRID +')';
	END
	IF(@Roletype='3')
	BEGIN
	   SELECT   @Id=  STUFF( (SELECT CONCAT(',', mm.EmpRegionId , '')   FROM View_Webapi_EmployeeFieldForceInfo mm (NOLOCK)   WHERE EmpInfoId=@empId   FOR XML PATH ('') ),1,1,'')  
	   

	    SET @Param=@Param+' and RegionId in ('+@Id+')'
	END
	IF(@Roletype='4')
	BEGIN
	    SELECT @Id=EmpGroupId FROM dbo.View_Webapi_EmployeeFieldForceInfo WHERE EmpInfoId=@empId

	    SET @Param=@Param+' and GroupId='+@Id
	END

	SET @Q=@Q+@Param

    EXEC sys.sp_executesql @Q

	end
```

## sp_webapi_GetCustomerbyUserPlanned

- **Parameters**: `@empId INT`, `@Date NVARCHAR(MAX)` (overwritten internally to `GETDATE()`, so the passed-in value is ignored)
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblMIOInfo`, `tblTerritory`, `tblMarket`, `tblCustMaster`, `tblCustCategory`, `tbl_TourPlanInfo`, `tbl_DCRCustomerInfo`, `tblASMInfo`, `tblTagASMRSMInfo`, `tblEmpGeneralInfo`
- **Tables written**: none
- **Business logic**: Returns today's planned customers for a role (MIO's own plan, or AM/team's whole territory-tagged plan annotated "Planned By: <planner>"), flagging each with `isDCRDone` based on whether a customer-DCR already exists today.
- **Transaction/error handling**: none
- **Result set**: one dynamic-branch result set — `isDCRDone`, `MarketCode`/`MarketName`, `IsSelect`, customer type/program/SMC ids, `CustomerMasterId`, `CustomerName`, `CustomerCode`, `Address`, `CellNo`, `CustomerType`, placeholder columns
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetCustomerbyUserPlanned]
	-- Add the parameters for the stored procedure here
    @empId INT,
	@Date nvarchar(max)
AS
    BEGIN
	set @date=GETDATE()
	DECLARE @Id NVARCHAR(MAX)=0
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId
IF(@Roletype='1')
	BEGIN

        SELECT distinct case when tblDCrDone.CustomerId is null then 0 else 1 end isDCRDone,  C.MarketCode, 0 IsSelect, D.CustomerTypeId, D.ProgramTypeId, D.SMCTypeId, 0 as CustomerCheck,'' Note, 0 GroupId, 0 RegionId, 0 as AreaId, 0 TerritoryId, 0 SubTerritoryId,0 MarketId,  D.CustomerMasterId ,
                D.CustomerName ,
                D.CustomerCode ,
                D.Address ,
                C.MarketName ,
                D.CellNo ,
                E.CategoryName AS CustomerType ,
                NULL AS 'CustomerStation' ,
                NULL AS 'Route' ,
                0 AS 'Balance' ,
                0 AS 'CreditLimit' ,
                NULL AS 'Route'
        FROM    dbo.tblMIOInfo A  with (nolock)
                INNER JOIN dbo.tblTerritory B  with (nolock) ON B.TerritoryId = A.TerritoryId
                INNER JOIN dbo.tblMarket C  with (nolock) ON C.TerritoryId = B.TerritoryId
                INNER JOIN dbo.tblCustMaster D  with (nolock) ON D.MarketId = C.MarketId
                INNER JOIN dbo.tblCustCategory E  with (nolock) ON E.CategoryId = D.CategoryId
			 INNER JOIN tbl_TourPlanInfo tp   with (nolock)   ON tp.CustomerMasterId = D.CustomerMasterId  and tp.EmpInfoId=A.EmployeeId

			 left join ( select distinct  CustomerId , EmpInfoId from tbl_DCRCustomerInfo dcr  where convert(Date,dcr.DcrDate)=  CAST(convert( Date,@date ) as nvarchar(max))  ) tblDCrDone ON tblDCrDone.CustomerId = D.CustomerMasterId  and tblDCrDone.EmpInfoId=A.EmployeeId
	  


        WHERE   A.EmployeeId = @empId AND A.IsActive = 1  and convert(Date,tp.TourPlanDate)= CAST(convert( Date,@date ) as nvarchar(max))


    END   
	


	IF(@Roletype='2')
	BEGIN
		declare @comId nvarchar(max)
	select @comId=CompanyId from tblASMInfo where EmployeeId=@empId and IsActive=1
	
        SELECT distinct case when tblDCrDone.CustomerId is null then 0 else 1 end isDCRDone,  C.MarketCode, 0 IsSelect, D.CustomerTypeId, D.ProgramTypeId, D.SMCTypeId, 0 as CustomerCheck,'' Note, 0 GroupId, 0 RegionId, 0 as AreaId, 0 TerritoryId, 0 SubTerritoryId,0 MarketId,  D.CustomerMasterId ,
                D.CustomerName ,
                D.CustomerCode ,
                D.Address ,
                C.MarketName  +CHAR(10)+  CHAR(10)+ 'Planned By:  '+  CHAR(10)+ eb.EmpMasterCode+' , '  +eb.EmpName   MarketName,
                D.CellNo ,
                E.CategoryName AS CustomerType ,
                NULL AS 'CustomerStation' ,
                NULL AS 'Route' ,
                0 AS 'Balance' ,
                0 AS 'CreditLimit' ,
                NULL AS 'Route'
        FROM    dbo.tblMIOInfo A  with (nolock)
                INNER JOIN dbo.tblTerritory B  with (nolock) ON B.TerritoryId = A.TerritoryId
                INNER JOIN dbo.tblMarket C  with (nolock) ON C.TerritoryId = B.TerritoryId
                INNER JOIN dbo.tblCustMaster D  with (nolock) ON D.MarketId = C.MarketId
                INNER JOIN dbo.tblCustCategory E  with (nolock) ON E.CategoryId = D.CategoryId
			 INNER JOIN tbl_TourPlanInfo tp   with (nolock)   ON tp.CustomerMasterId = D.CustomerMasterId  and tp.TerritoryId=A.TerritoryId and tp.CompanyId=@comId
			  INNER JOIN dbo.tblEmpGeneralInfo eb  with (nolock)   ON eb.EmpInfoId = tp.EmpInfoId 
			 left join ( select distinct  CustomerId , TerritoryId from tbl_DCRCustomerInfo dcr  where convert(Date,dcr.DcrDate)=  CAST(convert( Date,@date ) as nvarchar(max))  ) tblDCrDone ON tblDCrDone.CustomerId = D.CustomerMasterId  and tblDCrDone.TerritoryId=A.TerritoryId
	  


        WHERE  a.CompanyId=@comId and  A.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  ) AND A.IsActive = 1  and convert(Date,tp.TourPlanDate)= CAST(convert( Date,@date ) as nvarchar(max))


    END  
	
	 END
```

## sp_webapi_GetTourPlanStatus

- **Parameters**: `@empId INT = NULL`, `@month INT = NULL`, `@year INT = NULL`
- **Tables read**: `tbl_TourPlanMaster`
- **Tables written**: none
- **Business logic**: Returns the customer/market tour-plan master (submission/approval status) for an employee/month/year.
- **Transaction/error handling**: none
- **Result set**: all columns of `tbl_TourPlanMaster`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetTourPlanStatus]
	-- Add the parameters for the stored procedure here
@empId INT=null,@month INT=null,@year INT=null
AS
BEGIN
			
			SELECT * FROM dbo.tbl_TourPlanMaster WHERE EmpInfoId = @empId AND MonthValue = @month AND YearValue = @year

END
```

## sp_webapi_GetVPList

- **Parameters**: `@empId INT`, `@Month NVARCHAR(MAX)`, `@Year NVARCHAR(MAX)`, `@plnDate NVARCHAR(MAX)`
- **Tables read**: `tbl_TourPlanInfo`, `tblCustMaster`, `tbl_DoctorTourPlanDetail`, `tblDoctorMaster`
- **Tables written**: none
- **Business logic**: Returns a combined "visit plan" list (customer visits + doctor visits) for an employee on a specific planned date, labeled with market and formatted day/date/month/year.
- **Transaction/error handling**: none
- **Result set**: `Vplist`, `Market`, `DayName`, `DateValue`, `MonthName`, `YearValue`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetVPList]
	-- Add the parameters for the stored procedure here
    @empId INT,
	@Month nvarchar(max),
	@Year nvarchar(max),
	@plnDate nvarchar(max)
AS
    BEGIN
	 select * from (
	select 'Customer: '+ cus.CustomerCode+', '+ CustomerName Vplist,  ISNULL(mas.MarketCode_TP+' : '+mas.MarketName,'') Market,  datename(dw,mas.TourPlanDate) [DayName], FORMAT(mas.TourPlanDate,'dd') DateValue , FORMAT(mas.TourPlanDate,'MMMM') [MonthName], FORMAT(mas.TourPlanDate,'yyyy')  YearValue from tbl_TourPlanInfo mas with (nolock)
	inner join tblCustMaster cus  with (nolock) on mas.CustomerMasterId=cus.CustomerMasterId

	where mas.EmpInfoId=@empId and month(mas.TourPlanDate)=  @Month and year(mas.TourPlanDate)=  @Year and convert(Date,mas.TourPlanDate)=  convert(Date,@plnDate)
	 union all

	select 'Doctor: '+doc.DoctorCode+', '+ doc.DoctorName Vplist, ISNULL(mas.MarketCode_DV+' : '+mas.MarketName_DV,'') Market,  datename(dw,mas.TourPlanDate) [DayName], FORMAT(mas.TourPlanDate,'dd') DateValue , FORMAT(mas.TourPlanDate,'MMMM') [MonthName], FORMAT(mas.TourPlanDate,'yyyy')  YearValue from tbl_DoctorTourPlanDetail
	mas with (nolock)
	inner join tblDoctorMaster doc  with (nolock) on mas.DoctorId=doc.DoctorId
	
	where mas.EmpInfoId=@empId  and month(mas.TourPlanDate)=  @Month and year(mas.TourPlanDate)=  @Year   and convert(Date,mas.TourPlanDate)=  convert(Date,@plnDate)
	) tbl

        
    END
```

## sp_webapi_GetVPListDate

- **Parameters**: `@empId INT`, `@Month NVARCHAR(MAX)`, `@Year NVARCHAR(MAX)`
- **Tables read**: `tbl_TourPlanInfo`, `tblCustMaster`, `tbl_DoctorTourPlanDetail`, `tblDoctorMaster`
- **Tables written**: none
- **Business logic**: Returns the distinct set of dates in a month/year that have any customer or doctor visit plan for the employee — likely used to highlight plan dates on a calendar control.
- **Transaction/error handling**: none
- **Result set**: `PlanDate`, `DayName`, `DateValue`, `MonthName`, `YearValue`, ordered by date
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetVPListDate]
	-- Add the parameters for the stored procedure here
    @empId INT,
	@Month nvarchar(max),
	@Year nvarchar(max)
AS
    BEGIN
	 select distinct * from (
	select CONVERT(date, mas.TourPlanDate) PlanDate,  datename(dw,mas.TourPlanDate) [DayName], FORMAT(mas.TourPlanDate,'dd') DateValue , FORMAT(mas.TourPlanDate,'MMMM') [MonthName], FORMAT(mas.TourPlanDate,'yyyy')  YearValue from tbl_TourPlanInfo mas with (nolock)
	inner join tblCustMaster cus  with (nolock) on mas.CustomerMasterId=cus.CustomerMasterId

	where mas.EmpInfoId=@empId and month(mas.TourPlanDate)=  @Month and year(mas.TourPlanDate)=  @Year
	 union all

	select  CONVERT(date, mas.TourPlanDate) PlanDate,   datename(dw,mas.TourPlanDate) [DayName], FORMAT(mas.TourPlanDate,'dd') DateValue , FORMAT(mas.TourPlanDate,'MMMM') [MonthName], FORMAT(mas.TourPlanDate,'yyyy')  YearValue from tbl_DoctorTourPlanDetail
	mas with (nolock)
	inner join tblDoctorMaster doc  with (nolock) on mas.DoctorId=doc.DoctorId
	
	where mas.EmpInfoId=@empId  and month(mas.TourPlanDate)=  @Month and year(mas.TourPlanDate)=  @Year
	) tbl

        order by PlanDate asc
    END
```

## sp_webapi_Get_MIOTargetAcheve

- **Parameters**: `@empId INT`, `@Month INT`, `@Year INT`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblMIATarget`, `tblMIOInfo`, `tblEmpGeneralInfo`, `ZASHRIS..tblEmpGeneralInfo` (cross-database), `tblInvoice`, `tblInvoiceDetail`, `tblASMInfo`, `tblTagASMRSMInfo`
- **Tables written**: none
- **Business logic**: Target-vs-achievement summary (single aggregate, not per-product): for role `1` (MIO) computes the MIO's own monetary target/achievement/percentage; for role `2` (AM) computes the same but summed via a per-invoice loop rather than aggregation (note: the AM branch's achievement/percentage queries do not actually `GROUP BY`/`SUM` correctly across multiple MIOs — they compute one scalar per query, so only the last-matching row's amount effectively applies if multiple invoices exist; this looks like an incomplete/buggy aggregation). Joins to an external `ZASHRIS` HR database for employee code cross-reference.
- **Transaction/error handling**: none
- **Result set**: single row — `AchivmentValue`, `TargetValue`, `ActualSates` (hardcoded `'0'`), `Achivment` (only for roles 1 and 2; no rows for other roles)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_Get_MIOTargetAcheve]
	-- Add the parameters for the stored procedure here
    @empId INT,
    @Month int,
    @Year INT


AS
    BEGIN
declare @Month_val nvarchar(max)
	 
 
	 
	 
set	@Month_val =DATENAME( MONTH, DATEADD( MONTH, @Month, -1)) 


	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId

		declare @Target nvarchar(max)
				
  declare @Achievement nvarchar(max)
  declare @AchievementPer nvarchar(max)

IF(@Roletype='1')
	BEGIN




	select @Target= 
				cast( cast(ISNULL( mas.MiaTargetAmount,0)  as decimal(18,2)) as nvarchar(max))   from tblMIATarget  mas
				inner join  tblMIOInfo mio on mio.MIOId=mas.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode   
				 where mas.Year = @Year AND  (mas.Period = @Month_val) and EGI.EmpInfoId=@empId

		select	@Achievement= cast( cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))),0)  as decimal(18,2)) as nvarchar(max))   FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where EGI.EmpInfoId=@empId and MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year

				 	select	@AchievementPer= cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(@Target as decimal(18,2)),0) as decimal(18,2))   FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where EGI.EmpInfoId=@empId and MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year


				 select  @Achievement AchivmentValue,  @Target TargetValue, '0' ActualSates, @AchievementPer Achivment
 
  


  END   
	


	IF(@Roletype='2')
	BEGIN
		declare @comId nvarchar(max)
	select @comId=CompanyId from tblASMInfo where EmployeeId=@empId and IsActive=1
 
	select @Target= 
				cast( cast(ISNULL( mas.MiaTargetAmount,0)  as decimal(18,2)) as nvarchar(max))   from tblMIATarget  mas
				inner join  tblMIOInfo mio on mio.MIOId=mas.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode   
				 where mas.Year = @Year AND  (mas.Period = @Month_val) and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )

		select	@Achievement= cast( cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))),0)  as decimal(18,2)) as nvarchar(max))   FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where  MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )

				 	select	@AchievementPer= cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(@Target as decimal(18,2)),0) as decimal(18,2))   FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where   MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )


				 select  @Achievement AchivmentValue,  @Target TargetValue, '0' ActualSates, @AchievementPer Achivment
 
    END
    END

```

## sp_webapi_Get_MIOTargetAcheve_New

- **Parameters**: `@empId INT`, `@Month INT`, `@Year INT`
- **Tables read**: `tblUser`, `tbl_UserRoleInfo`, `tblMIATargetProductWise`, `tblProduct`, `tblMIOInfo`, `tblTerritory`, `tblArea`, `tblEmpGeneralInfo`, `tblCompanyInfo`, `tblDcWiseAreaInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail`, `tblASMInfo`, `tblTagASMRSMInfo`, `tblMIATarget`, `ZASHRIS..tblEmpGeneralInfo`, `tblRSMInfo`, `tblTerritory` (again, for RSM scope)
- **Tables written**: none
- **Business logic**: Newer, per-role target-vs-achievement report: role `1` (MIO) aggregates product-wise target quantity vs. delivered value/qty properly grouped; roles `2` (AM) and `3` (DZSM/RSM) instead compute a monetary target (`tblMIATarget.MiaTargetAmount`) vs. summed invoice net amount, scoped by territory-tag membership. Contains several large commented-out alternate query blocks (dead code).
- **Transaction/error handling**: none
- **Result set**: one result set per matched role branch — column names/shapes differ slightly per branch (`Mio`/`EmpName`, `RoleType`, `TargetValue`, `AchivmentValue`, `Achivment`)
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_Get_MIOTargetAcheve_New]
	-- Add the parameters for the stored procedure here
    @empId INT,
    @Month int,
    @Year INT


AS
    BEGIN
declare @Month_val nvarchar(max)
	 
 
	 
	 
set	@Month_val =DATENAME( MONTH, DATEADD( MONTH, @Month, -1)) 

declare @comId nvarchar(max)
	DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId
	 
				
 

IF(@Roletype='1')
	BEGIN
	select @comId=CompanyId from tblMIOInfo where EmployeeId=@empId and IsActive=1
	 
   
				 SELECT  EmpMasterCode + ' : '+ EmpName Mio, 'MIO' RoleType,   cast(SUM(TargetQty) as decimal(18,2)) TargetValue,   cast(ISNULL(Value,0)  as decimal(18,2))  AS AchivmentValue, CASE WHEN SUM(ISNULL(TGRT.TargetQty,0)) > 0 THEN (ISNULL(SL.Qty,0)* 100) / SUM(ISNULL(TGRT.TargetQty,0)) ELSE 0 END AS Achivment  FROM tblMIATargetProductWise AS TGRT  
LEFT JOIN tblProduct AS PD ON TGRT.ProductId = PD.ProductId 
LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=TGRT.MiaId 
	 
LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId  
LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId 
LEFT JOIN tblCompanyInfo AS CI ON TGRT.CompanyId = CI.CompanyId   
LEFT JOIN (SELECT ComUnitId,AreaId,CompanyId FROM tblDcWiseAreaInfo AS DC 
LEFT JOIN tblCompanyUnit AS UNT ON DC.DCId = UNT.ComUnitId 
WHERE IsActive = 1) AS DCA ON ARA.AreaId = DCA.AreaId AND TGRT.CompanyId = DCA.CompanyId 
LEFT JOIN tblCompanyUnit AS UNT ON DCA.ComUnitId = UNT.ComUnitId 
LEFT JOIN (SELECT INV.ComUnitId,MiaId,UNT.ComUnitCode,UNT.ComUnitName,ProductCode,SUM(INVD.DeliveryNetAmount) AS Value, ISNULL(SUM(DeliveryTotalQuantity),0) AS Qty FROM tblInvoice AS INV 
LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId  
LEFT JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId 
------  
where    DeliveryInvoiceStatus IN ('Full','Partial')   AND month(INV.InvoiceDate)=@Month and  year(INV.InvoiceDate)=@Year
GROUP BY INV.ComUnitId,UNT.ComUnitCode,UNT.ComUnitName,ProductCode,MiaId) AS SL ON Sl.ProductCode = PD.ProductCode AND TGRT.MiaId = SL.MiaId  
--
WHERE TGRT.Year = @Year  and TGRT.Period=@Month_val
and  tblEmpGeneralInfo.EmpInfoId=@empId-- and   ISNULL(SL.ComUnitId,UNT.ComUnitId) = @comId

GROUP BY SL.Value, SL.Qty, EmpMasterCode + ' : '+ EmpName 

  END   
	


	IF(@Roletype='2')
	BEGIN
		
	select @comId=CompanyId from tblASMInfo where EmployeeId=@empId and IsActive=1
 
	--select  
	--			cast( cast(ISNULL( mas.MiaTargetAmount,0)  as decimal(18,2)) as nvarchar(max)) Target,    from tblMIATarget  mas
	--			inner join  tblMIOInfo mio on mio.MIOId=mas.MiaId
	--			 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
	--			 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode   
	--			 where mas.Year = @Year AND  (mas.Period = @Month_val) and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )

		select	 EGI.EmpMasterCode + ' : '+EGI.EmpName Mio, 'MIO' RoleType,  cast( (ISNULL( mioTar.MiaTargetAmount,0))  as decimal(18,2) ) TargetValue,    cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))),0)  as decimal(18,2) ) AchivmentValue,   cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(ISNULL( mioTar.MiaTargetAmount,0) as decimal(18,2)),0) as decimal(18,2))    Achivment  FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				inner join  tblMIATarget mioTar on mio.MIOId=mioTar.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where  MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  ) and  mioTar.Year = @Year AND  (mioTar.Period = @Month_val)  
				 	 group by EGI.EmpMasterCode + ' : '+EGI.EmpName, ISNULL( mioTar.MiaTargetAmount,0)
				-- 	select	@AchievementPer= cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(@Target as decimal(18,2)),0) as decimal(18,2))   FROM tblInvoice AS INV  
    --            LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				--inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				-- inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				-- inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				-- where   MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )


			
 
    END



		IF(@Roletype='3')
	BEGIN
		
	select @comId=CompanyId from tblRSMInfo where EmployeeId=@empId and IsActive=1
 
	--select  
	--			cast( cast(ISNULL( mas.MiaTargetAmount,0)  as decimal(18,2)) as nvarchar(max)) Target,    from tblMIATarget  mas
	--			inner join  tblMIOInfo mio on mio.MIOId=mas.MiaId
	--			 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
	--			 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode   
	--			 where mas.Year = @Year AND  (mas.Period = @Month_val) and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )

		select	 EGI.EmpMasterCode + ' : '+EGI.EmpName EmpName, 'MIO' RoleType, cast( cast( (ISNULL( mioTar.MiaTargetAmount,0))  as decimal(18,2)) as nvarchar(max)) TargetValue,  cast( cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))),0)  as decimal(18,2)) as nvarchar(max)) AchivmentValue, cast(  cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(ISNULL( mioTar.MiaTargetAmount,0) as decimal(18,2)),0) as decimal(18,2)) as nvarchar(max))  Achivment  FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				inner join  tblMIATarget mioTar on mio.MIOId=mioTar.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where  MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year and    mio.CompanyId=@comId and  mio.TerritoryId  in (select TerritoryId from tblTerritory where AreaId in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=5 ))
				 	 group by EGI.EmpMasterCode + ' : '+EGI.EmpName,ISNULL( mioTar.MiaTargetAmount,0)
				-- 	select	@AchievementPer= cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(@Target as decimal(18,2)),0) as decimal(18,2))   FROM tblInvoice AS INV  
    --            LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				--inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				-- inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				-- inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				-- where   MONTH(INV.InvoiceDate)=@Month and year(INV.InvoiceDate)=@Year and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )


			
 
    END
    END

```

## sp_webapi_Get_StockReport

- **Parameters**: `@empId INT`
- **Tables read**: `View_DCStoreCurrentStock`, `tblCompanyUnit`, `tblCompanyInfo`, `tblEmpGeneralInfo`
- **Tables written**: none
- **Business logic**: Returns current distributor-center stock quantities (aggregated by product) for the company the calling employee belongs to, excluding zero/negative stock.
- **Transaction/error handling**: none
- **Result set**: `ProductCode`, `ProductName`, `StockQty`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE sp_webapi_Get_StockReport
	-- Add the parameters for the stored procedure here
    @empId INT
AS
    BEGIN
		
        SELECT  A.ProductCode ,
                A.ProductName ,
                SUM(A.TotalQty) AS StockQty
        FROM    dbo.View_DCStoreCurrentStock A
                INNER JOIN dbo.tblCompanyUnit B ON B.ComUnitId = A.ComUnitId
                INNER JOIN dbo.tblCompanyInfo C ON C.CompanyId = B.CompanyId
                INNER JOIN dbo.tblEmpGeneralInfo D ON C.CompanyId = D.CompanyId
        WHERE   D.EmpInfoId = @empId
                AND A.TotalQty > 0
        GROUP BY A.ProductCode ,
                A.ProductName
        ORDER BY A.ProductName

    END
```

## sp_webapi_GetmarketNotice_ByEmpId

- **Parameters**: `@empId INT`
- **Tables read**: `tblMIOInfo`, `tblTerritory`, `tblMarket`, `tblArea`, `tblRegion`, `tbl_Notice_MarketDetails`, `tbl_Notice_MarketMaster`
- **Tables written**: none
- **Business logic**: Returns the 20 most recent market-targeted notices/announcements visible to the markets covered by the employee's assigned territory.
- **Transaction/error handling**: none
- **Result set**: `NoticeId`, `NoticeTitle`, `Announcement`, `FromDate`, `ToDate`, `EntryDate`, `EntryBy`, `IsActive`, `IsReaded`
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_GetmarketNotice_ByEmpId]
	-- Add the parameters for the stored procedure here
@empId INT 
AS
BEGIN
	
	SELECT  DISTINCT TOP 20 NM.NoticeId ,
                    NM.NoticeTitle ,
                    NM.Announcement ,
					CONVERT(VARCHAR(10),NM.FromDate,100) AS FromDate,
             	CONVERT(VARCHAR(10),NM.ToDate,100) AS ToDate,
  
                    NM.EntryDate ,
                    NM.EntryBy ,
                    NM.IsActive ,
                    NM.IsReaded
FROM    dbo.tblMIOInfo A
        INNER JOIN dbo.tblTerritory B ON B.TerritoryId = A.TerritoryId
		INNER JOIN dbo.tblMarket C ON c.TerritoryId = B.TerritoryId
		LEFT JOIN dbo.tblArea Ar ON Ar.AreaId = B.AreaId
		LEFT JOIN dbo.tblRegion Re ON Re.RegionId = Ar.RegionId
		INNER JOIN dbo.tbl_Notice_MarketDetails NB ON NB.MarketId = C.MarketId
		INNER JOIN dbo.tbl_Notice_MarketMaster NM ON NM.NoticeId = NB.NoticeId
		WHERE A.EmployeeId = @empId
		ORDER BY NM.NoticeId DESC
	

END
```

## sp_webapi_SaveOrderDetail

- **Parameters**: `@orderid INT`, `@productid INT`, `@quantity DECIMAL(18,2)`, `@unitprice DECIMAL(18,2)`, `@unitvatPercentage DECIMAL(18,2)`
- **Tables read**: `tblProduct`
- **Tables written**: `tblOrderDetail` (INSERT), `tblOrder` (UPDATE `GrossValue`)
- **Business logic**: Adds one order line item (computing VAT-per-unit, total amount, total VAT, and net amount from quantity/price/VAT%), then rolls the line's total up into the parent order's running `GrossValue`.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_SaveOrderDetail]
	-- Add the parameters for the stored procedure here
@orderid INT,
@productid INT,
@quantity DECIMAL(18,2),
@unitprice DECIMAL(18,2),
@unitvatPercentage DECIMAL(18,2)


AS
BEGIN
		
		DECLARE @totalVatAmount DECIMAL(18,2),@totalAmount DECIMAL(18,2),@NetAmount DECIMAL(18,2)

		
		DECLARE @vatAmtPerUnit DECIMAL(18,2)

		
		SET @vatAmtPerUnit = ((@unitprice*@unitvatPercentage)/100)
	
		SET @totalAmount = @quantity*@unitprice	
		SET @totalVatAmount = ((@totalAmount * @unitvatPercentage)/100)


		SET @NetAmount = @totalAmount + @totalVatAmount

		DECLARE @productCode NVARCHAR(500)
		DECLARE @productN NVARCHAR(500)

		SELECT @productCode = ProductCode, @productN=ProductName  FROM dbo.tblProduct WHERE ProductId = @productid


		INSERT INTO dbo.tblOrderDetail
		        ( ProductId ,
		          Quantity ,
		          TradePrice ,
		          TotalTradePrice ,
		          OrderId ,
		          UnitVatAmount ,
		          TotalVatAmount ,
		          NetAmount,
				  ProductCode, ProductName
		        )
		VALUES  ( 
					
					@productid,
					@quantity,
					@unitprice,
					@totalAmount,
					@orderid,
					@vatAmtPerUnit,
					@totalVatAmount,
					@NetAmount,
					@productCode, @productN
		        )


				UPDATE dbo.tblOrder SET GrossValue = ISNULL(GrossValue,0) + @totalAmount WHERE OrderId = @orderid

END
```

## sp_webapi_SaveOrderMaster

- **Parameters**: `@empId INT`, `@comUnitId INT` (accepted but unused — company unit is instead derived from the customer's DC mapping), `@CustomerCode NVARCHAR(50)`, `@SubmittedDate NVARCHAR(MAX) = NULL` (overwritten to today), `@CollectionDate NVARCHAR(MAX) = NULL` (overwritten to today, then never used), `@EntryDate NVARCHAR(MAX) = NULL` (overwritten to `GETDATE()`), `@Remarks NVARCHAR(MAX) = NULL`, `@orderType NVARCHAR(MAX) = NULL` (unused in body)
- **Tables read**: `tblOrder` (duplicate/sequence checks), `tblUser`, `tblEmpGeneralInfo`, `tbl_UserRoleInfo`, `tblRoleType`, `tblMIOInfo`, `tblCustMaster`, `tblMarket`, `tblTerritory`, `tblArea`, `tblDcWiseAreaInfo`, `tblCompanyUnit`, `tblCompanyInfo`, `tblASMInfo`, `tblRSMInfo`, `tblRegion`
- **Tables written**: `tblOrder` (INSERT)
- **Business logic**: Creates a new sales order from the mobile app — guards against rapid duplicate submits (same customer/date within a ~3.5 minute window), resolves the caller's company/MIO identity by role, resolves the customer's full distribution hierarchy (region/area/territory/market/DC/company-unit/RSM/ASM), generates a sequential `OrderCode` (`ODR-<unitcode>-<yy><mm><dd><seq>`) with a uniqueness re-check, and inserts the order with `ActionStatus = 'Accepted'` and `IsFromApp = 1`.
- **Transaction/error handling**: wrapped in `BEGIN TRY / END TRY ... BEGIN CATCH / END CATCH`; on any error the CATCH block returns a scalar `0` and exits (no `RAISERROR`, no explicit `BEGIN TRAN`/`ROLLBACK` — errors are swallowed rather than surfaced)
- **Result set**: single scalar — new `tblOrder` identity on success, `0` if a duplicate submit was detected, a generated `OrderCode` collision was found, or an error was caught
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_SaveOrderMaster]  --[dbo].[sp_webapi_SaveOrderMaster] 'shaon20','20245','11/25/2020 5:04:16 PM'
    @empId INT ,
    @comUnitId INT ,
    @CustomerCode NVARCHAR(50) ,
    @SubmittedDate nvarchar(max)=null ,
    @CollectionDate nvarchar(max)=null ,
    @EntryDate nvarchar(max) = NULL,
	@Remarks NVARCHAR(max) =null

	,
	@orderType NVARCHAR(max) =null
AS
    BEGIN
	 BEGIN TRY
	declare @mioCode NVARCHAR(50) ,
            @mioName NVARCHAR(50) ,
              @mioId INT 
	set @SubmittedDate= CONVERT(date, GETDATE())
	set @CollectionDate= CONVERT(date, GETDATE())
	set @EntryDate=  GETDATE() 

	DECLARE @CountDataOrd INT=0
SELECT @CountDataOrd = COUNT(*)
FROM dbo.tblOrder
WHERE CustomerCode = @CustomerCode
  AND CONVERT(DATE, SubmissionDate) = CONVERT(DATE, @SubmittedDate)
  AND EntryDate BETWEEN DATEADD(MINUTE, -3, GETDATE())   -- ???? ? ?????
                     AND DATEADD(SECOND, 30, GETDATE());  
IF(@CountDataOrd=0)
BEGIN


	Declare @Roltp nvarchar(max)=''
 declare @companyId int=0 
 select @Roltp=RoleType   FROM dbo.tblUser  A
	 INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE A.EmpInfoId=@empId


if(@Roltp='MIO')
		begin 
		select @companyId=B.CompanyId,@mioCode = A.EmpMasterCode ,
                @mioName = A.EmpName ,
                @mioId = B.MIOId from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@EMPId and IsActive=1

		  

		end

			if(@Roltp='AM')
		begin 
		--select @companyId=CompanyId from tblASMInfo where EmployeeId=@EMPId and IsActive=1
		declare @terrId  int

		 SELECT  
                @terrId = TTR.TerritoryId  
                
        FROM    tblCustMaster AS CSTMR
                INNER JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
                INNER JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId
                 
        WHERE   CSTMR.MarketId IS NOT NULL
                AND CSTMR.CustomerCode = @CustomerCode

		select @companyId=B.CompanyId from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@EMPId and IsActive=1

		 select  @mioCode = A.EmpMasterCode ,
                @mioName = A.EmpName ,
                @mioId = B.MIOId from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where b.TerritoryId=@terrId and IsActive=1  and b.CompanyId=@companyId


		end
 	if(@Roltp='DZSM')

	begin 
		--select @companyId=CompanyId from tblRSMInfo where EmployeeId=@EMPId and IsActive=1

			select @companyId=B.CompanyId,@mioCode = A.EmpMasterCode ,
                @mioName = A.EmpName ,
                @mioId = B.RSMId from tblRSMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		 where EmployeeId=@EMPId and IsActive=1

		end

        DECLARE @comUnitCode NVARCHAR(50) ,
            @comunitName NVARCHAR(50) ,
           
            @customerName NVARCHAR(50) ,
            @regionId INT ,
            @areaId INT ,
            @territoryId INT ,
            @marketId INT ,
            @customerMasterId INT ,
            @rsmId INT ,
            @asmId INT ,
          
            @userIdMaster  NVARCHAR(50)

		

        SELECT  @userIdMaster = LoginName
        FROM    dbo.tblUser
        WHERE   EmpInfoId = @empId

      

        SELECT  @customerName = CustomerName
        FROM    dbo.tblCustMaster
        WHERE   CustomerCode = @CustomerCode


        DECLARE @loginName NVARCHAR(50)

		--select @loginName  =  LoginName FROM dbo.tblUser WHERE EmpInfoId = @empId

		DECLARE @depId int
		
        SELECT  @comUnitCode = F.ComUnitCode ,
                @comunitName = F.ComUnitName ,
               -- @companyId = @comUnitId,
				@depId = F.ComUnitId
        FROM    dbo.tblCustMaster A
                INNER JOIN dbo.tblMarket B ON B.MarketId = A.MarketId
                INNER JOIN dbo.tblTerritory C ON C.TerritoryId = B.TerritoryId
                INNER JOIN dbo.tblArea D ON D.AreaId = C.AreaId
                INNER JOIN dbo.tblDcWiseAreaInfo E ON E.AreaId = D.AreaId
                INNER JOIN dbo.tblCompanyUnit F ON F.ComUnitId = E.DCId
                INNER JOIN dbo.tblCompanyInfo com ON com.CompanyId = F.CompanyId
        WHERE   A.CustomerCode = @CustomerCode
                AND com.CompanyId = @companyId and E.IsActive=1


        SELECT  @regionId = RGN.RegionId ,
                @areaId = ARA.AreaId ,
                @territoryId = TTR.TerritoryId ,
                @marketId = MKT.MarketId ,
                @customerMasterId = CSTMR.CustomerMasterId ,
                @rsmId = RS.RSMId ,
                @asmId = ASm.ASMId
        FROM    tblCustMaster AS CSTMR
                INNER JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
                INNER JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId
                INNER JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId
                INNER JOIN dbo.tblRegion AS RGN ON ARA.RegionId = RGN.RegionId

                INNER JOIN dbo.tblRSMInfo RS ON RGN.RegionId = RS.RegionId
                INNER JOIN dbo.tblASMInfo ASm ON ASm.AreaId = ARA.AreaId
        WHERE   CSTMR.MarketId IS NOT NULL
                AND CSTMR.CustomerCode = @CustomerCode
                AND RS.IsActive = 1
                AND RS.CompanyId = @companyId
                AND ASm.IsActive = 1
				AND ASm.CompanyId = @companyId



				
        --SELECT  @comUnitCode = ComUnitCode ,
        --        @comunitName = ComUnitName ,
        --        @companyId = CompanyId
        --FROM    dbo.tblCompanyUnit
        --WHERE   ComUnitId = @comUnitId



	DECLARE @OrderNo NVARCHAR(MAX);
	DECLARE @MaxSequence INT;
	
	-- Retrieve the maximum existing sequence number for orders on today's date

	SELECT @MaxSequence = ISNULL(MAX(CAST(RIGHT(OrderCode, 4) AS INT)), 0)
	FROM tblOrder WHERE CAST(EntryDate AS DATE) = CAST(GETDATE() AS DATE) AND ComUnitId = @depId;

	--PRINT @MaxSequence
	

	--DECLARE @ComUnitCode NVARCHAR(200) = 'BD53'

	-- Generate the new OrderNo by incrementing the maximum sequence found

	SET @OrderNo = 'ODR-' + @ComUnitCode + '-'
	    + RIGHT(YEAR(GETDATE()), 2) 
	    + FORMAT(GETDATE(), 'MM') 
	    + FORMAT(GETDATE(), 'dd') 
	    + RIGHT('0000' + CONVERT(NVARCHAR(4), @MaxSequence + 1), 4);

	--PRINT @OrderNo

	DECLARE @Count INT = 0
	SELECT @Count = COUNT(OrderId) FROM tblOrder WHERE LTRIM(RTRIM(UPPER(OrderCode))) = LTRIM(RTRIM(UPPER(@OrderNo)))

	--PRINT @Count

	IF(@Count = 0)
	BEGIN

		INSERT INTO dbo.tblOrder
		        (OrderCode,
		         ComUnitId,
		         ComUnitCode,
		         ComUnitName,
		         MIOCode,
		         MIOName,
		         ManufacId,
		         CustomerCode,
		         CustomerName,
		         SubmissionDate,
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
		         IsInvoice,
		         IsManual,
		         IsSpecialApproval,
		         ActionStatus,
		         IsFromApp,
		         Remarks
		         )
			VALUES      
			(	 @OrderNo,
		         @depId,
		         @comUnitCode,
		         @comunitName,
		         @mioCode,
		         @mioName,
		         @companyId,
		         @CustomerCode,
		         @customerName,
		         @SubmittedDate,
		         @regionId,
		         @areaId,
		         @territoryId,
		         @marketId,
		         @customerMasterId,
		         @rsmId,
		         @asmId,
		         @mioId,
		         @userIdMaster,
		         GETDATE(),
		         0,
		         0,
		         0,
		         'Accepted',
		         1,
		         @Remarks
		         );

		SELECT SCOPE_IDENTITY();

	END

	ELSE

	BEGIN
		
		SELECT 0

	END

	

	-- Insert Process End


        

    END    
   END TRY
    BEGIN CATCH

       SELECT 0

        RETURN;

    END CATCH
END
```

## sp_webapi_SavePunchInInfo

- **Parameters**: `@attendaceDate DATETIME`, `@pInTime NVARCHAR(50)`, `@pInLat NVARCHAR(50)`, `@pInLong NVARCHAR(50)`, `@empId INT`, `@EntryDate DATETIME`
- **Tables read**: `tblMIOInfo`
- **Tables written**: `tblMarketAttendance_Master_webapi` (INSERT)
- **Business logic**: Records a mobile "punch in" attendance row for an employee. Older/simpler sibling of `sp_webapi_SavePunchTotalInfo`, with no role/company/shift resolution.
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_SavePunchInInfo]
	-- Add the parameters for the stored procedure here
@attendaceDate DATETIME,
@pInTime NVARCHAR(50),
@pInLat NVARCHAR(50),
@pInLong NVARCHAR(50),
@empId INT,
@EntryDate DATETIME
AS
BEGIN


DECLARE @mioId INT

SELECT @mioId = MIOId FROM dbo.tblMIOInfo WHERE EmployeeId = @empId

INSERT INTO dbo.tblMarketAttendance_Master_webapi
        ( MIOId ,
          EmpInfoId ,
          PunchInTime ,
          PInLat ,
          PInLog ,
          AttendanceDate,
		  PINCreatedDateTime
        )
VALUES  ( 
			@mioId,
			@empId,
			@pInTime,
			@pInLat,
			@pInLong,
			@attendaceDate,
			@EntryDate
        )





END
```

## sp_webapi_SavePunchTotalInfo

- **Parameters**: `@attendaceDate NVARCHAR(MAX) = NULL` (overwritten to `GETDATE()`), `@pInTime NVARCHAR(50) = NULL`, `@pInLat NVARCHAR(50) = NULL`, `@pInLong NVARCHAR(50) = NULL`, `@pOutTime NVARCHAR(50) = NULL` (unused), `@poutLat NVARCHAR(50) = NULL` (unused), `@poutLong NVARCHAR(50) = NULL` (unused), `@remarks NVARCHAR(500) = NULL`, `@empId INT`, `@EntryDate DATETIME`, `@AttType INT`, `@AttAddress NVARCHAR(MAX) = NULL` (unused)
- **Tables read**: `tblMIOInfo`, `dbo.GetAttendanceStatusWithShiftId(@pInTime)` (function), `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`, `ZAS_UserFS_MIO`/`ZAS_UserFS_AM`, `tblEmpGeneralInfo`, `tblMarketAttendance_Master_webapi`
- **Tables written**: `tblMarketAttendance_Master_webapi` (INSERT for `@AttType = 1` punch-in, UPDATE for `@AttType = 2` punch-out)
- **Business logic**: Unified punch-in/punch-out proc driven by `@AttType`: on punch-in, resolves the current shift, the caller's role/company/org hierarchy (denormalized), and inserts a new attendance row; on punch-out, locates today's open attendance row for the resolved shift and updates its punch-out fields. Also calls out to the external `ZASHRIS..sp_ATT_AppsAttendenceSyncProcess` to sync the punch into the separate HR system. Contains a large commented-out block referencing an `ZASHRIS` HR proximity cross-check (dead code).
- **Transaction/error handling**: none
- **Result set**: single scalar `@AttId` (the attendance row id, whether inserted or updated)
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_webapi_SavePunchTotalInfo]

    @attendaceDate NVARCHAR(MAX) = NULL,
    @pInTime NVARCHAR(50) = NULL,
    @pInLat NVARCHAR(50) = NULL,
    @pInLong NVARCHAR(50) = NULL,
    @pOutTime NVARCHAR(50) = NULL,
    @poutLat NVARCHAR(50) = NULL,
    @poutLong NVARCHAR(50) = NULL,
    @remarks NVARCHAR(500) = NULL,
    @empId INT,
    @EntryDate DATETIME,
    @AttType INT,
    @AttAddress NVARCHAR(MAX) = NULL


AS
BEGIN

    -- Convert attendaceDate to current date

	--DECLARE @HREmployeeId INT = 0 -- HR Employee Id
	
	--SELECT HEGI.* FROM tblEmpGeneralInfo AS EGI 
	--INNER JOIN ZASHRIS..tblEmpGeneralInfo AS HEGI ON EGi.EmpMasterCode = HEGI.EmpMasterCode
	--WHERE EGI.EmpInfoId =  904 --@empId -- Salesroll Employee Id

	--PRINT @HREmployeeId
   

    SET @attendaceDate =   GETDATE() ;

    DECLARE @AttId INT = NULL;
    DECLARE @mioId INT;

    SELECT @mioId = MIOId 
    FROM dbo.tblMIOInfo 
    WHERE EmployeeId = @empId;

    DECLARE @ShiftId INT = 0, @AttStatus NVARCHAR(MAX) = '';

    IF (@AttType = 1)
    BEGIN
        SET @pInTime = FORMAT(GETDATE(), 'hh:mm tt');

        SELECT @ShiftId = ShiftId, 
               @AttStatus = AttStatus 
        FROM dbo.GetAttendanceStatusWithShiftId(@pInTime);

        PRINT @ShiftId;
        PRINT @AttStatus;

        DECLARE @CompanyId INT = 0,
                @Roltp NVARCHAR(MAX),
                @GroupId INT = NULL,  
                @GroupCode NVARCHAR(MAX),
                @GroupName NVARCHAR(MAX),
                @RegionId INT = NULL,
                @RegionCode NVARCHAR(MAX),
                @RegionName NVARCHAR(MAX),
                @AreaId INT = NULL,
                @AreaCode NVARCHAR(MAX),
                @AreaName NVARCHAR(MAX),
                @TerritoryId INT = NULL,
                @TerritoryCode NVARCHAR(MAX),
                @TerritoryName NVARCHAR(MAX);

        SELECT @Roltp = RoleType
        FROM dbo.tblUser A
        LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
        LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId = tbl_UserRoleInfo.RoleTypeId
        WHERE A.EmpInfoId = @empId;

        IF (@Roltp = 'MIO')
        BEGIN
            SELECT @CompanyId = ISNULL(CompanyId, 0),
                   @TerritoryId = TerritoryId, @TerritoryCode = TerritoryCode, @TerritoryName = TerritoryName,
                   @AreaId = AreaId, @AreaCode = AreaCode, @AreaName = AreaName,
                   @RegionId = RegionId, @RegionCode = RegionCode, @RegionName = RegionName,
                   @GroupId = GroupId, @GroupCode = GroupCode, @GroupName = GroupName
            FROM ZAS_UserFS_MIO 
            WHERE EmpInfoId = @empId;
        END

        IF (@Roltp = 'AM')
        BEGIN
            SELECT @CompanyId = ISNULL(CompanyId, 0),
                   @TerritoryId = TerritoryId, @TerritoryCode = TerritoryCode, @TerritoryName = TerritoryName,
                   @AreaId = AreaId, @AreaCode = AreaCode, @AreaName = AreaName,
                   @RegionId = RegionId, @RegionCode = RegionCode, @RegionName = RegionName,
                   @GroupId = GroupId, @GroupCode = GroupCode, @GroupName = GroupName
            FROM ZAS_UserFS_AM
            WHERE EmpInfoId = @empId;
        END

        IF (@CompanyId = 0)
        BEGIN
            SELECT @CompanyId = e.CompanyId
            FROM dbo.tblEmpGeneralInfo e
            WHERE e.EmpInfoId = @empId;
        END

        INSERT INTO dbo.tblMarketAttendance_Master_webapi
        (
            MIOId, EmpInfoId, PunchInTime, PInLat, PInLog, AttendanceDate,
            PINCreatedDateTime, CompanyId, GroupId, GroupCode, GroupName,
            RegionId, RegionCode, RegionName, AreaId, AreaCode, AreaName,
            TerritoryId, TerritoryCode, TerritoryName, ShiftId, AttStatus
        )
        VALUES
        (
            @mioId, @empId, @pInTime, @pInLat, @pInLong, @attendaceDate,
            @EntryDate, @CompanyId, @GroupId, @GroupCode, @GroupName,
            @RegionId, @RegionCode, @RegionName, @AreaId, @AreaCode, @AreaName,
            @TerritoryId, @TerritoryCode, @TerritoryName, @ShiftId, @AttStatus
        );

        SET @AttId = SCOPE_IDENTITY();


    END

    IF (@AttType = 2)
    BEGIN
        DECLARE @count INT;
        SELECT @ShiftId = ShiftId, 
               @AttStatus = AttStatus 
        FROM dbo.GetAttendanceStatusWithShiftId(@pInTime);

        SELECT @AttId = AttendanceId, 
               @CompanyId = ISNULL(CompanyId, 0) 
        FROM dbo.tblMarketAttendance_Master_webapi 
        WHERE CONVERT(DATE, AttendanceDate) = CONVERT(DATE, @attendaceDate)
          AND EmpInfoId = @empId AND ShiftId = @ShiftId;

        IF (@CompanyId = 0)
        BEGIN
            SELECT @CompanyId = e.CompanyId
            FROM dbo.tblEmpGeneralInfo e
            WHERE e.EmpInfoId = @empId;
        END

        IF (@AttId > 0)
        BEGIN
            UPDATE dbo.tblMarketAttendance_Master_webapi 
            SET PunchOutTime = @pInTime,
                POutLat = @pInLat,
                POutLong = @pInLong,
                POutRemarks = @remarks,
                POUTCreatedDateTime = @EntryDate,  
                CompanyId = @CompanyId
            WHERE AttendanceId = @AttId;
        END
    END
		
	--========================================
	-- Insert Into HR Proximity
	--========================================

	EXEC ZASHRIS..sp_ATT_AppsAttendenceSyncProcess 
	@StartDate = @attendaceDate,
	@EndDate = @attendaceDate,
	@EmployeeId = @empId;



    SELECT @AttId;


END

```

## sp_webapi_SavePunchoutInfo

- **Parameters**: `@attendaceDate DATETIME`, `@pOutTime NVARCHAR(50)`, `@pOutLat NVARCHAR(50)`, `@pOutLong NVARCHAR(50)`, `@remarks NVARCHAR(500)`, `@empId INT`, `@EntryDate DATETIME`
- **Tables read**: `tblMarketAttendance_Master_webapi`
- **Tables written**: `tblMarketAttendance_Master_webapi` (UPDATE, conditional)
- **Business logic**: Records a punch-out against today's existing attendance row for the employee, only if one exists. Older/simpler sibling of the punch-out branch in `sp_webapi_SavePunchTotalInfo` (no shift resolution).
- **Transaction/error handling**: none
- **Result set**: none
- **Caller**: UNUSED — no application caller found

```sql
CREATE PROCEDURE [dbo].[sp_webapi_SavePunchoutInfo]
	-- Add the parameters for the stored procedure here
@attendaceDate DATETIME,
@pOutTime NVARCHAR(50),
@pOutLat NVARCHAR(50),
@pOutLong NVARCHAR(50),
@remarks NVARCHAR(500),
@empId INT,
@EntryDate DATETIME
AS
BEGIN

DECLARE @count INT

SELECT @count =  COUNT(*) FROM dbo.tblMarketAttendance_Master_webapi WHERE AttendanceDate = @attendaceDate AND EmpInfoId = @empId

IF(@count > 0)
BEGIN
		
		UPDATE dbo.tblMarketAttendance_Master_webapi 
		SET PunchOutTime =@pOutTime,
			POutLat = @pOutLat,
			POutLong = @pOutLong,
			POutRemarks = @remarks,
			POUTCreatedDateTime = @EntryDate
			WHERE AttendanceDate = @attendaceDate AND EmpInfoId = @empId
		
END




END
```

## sp_webapi_Shift_SavePunchTotalInfo

- **Parameters**: `@attendaceDate NVARCHAR(MAX) = NULL` (overwritten to `GETDATE()`), `@pInTime NVARCHAR(50) = NULL`, `@pInLat NVARCHAR(50) = NULL`, `@pInLong NVARCHAR(50) = NULL`, `@pOutTime NVARCHAR(50) = NULL` (unused), `@poutLat NVARCHAR(50) = NULL` (unused), `@poutLong NVARCHAR(50) = NULL` (unused), `@remarks NVARCHAR(500) = NULL`, `@empId INT`, `@EntryDate DATETIME`, `@AttType INT`, `@AttAddress NVARCHAR(MAX) = NULL` (unused)
- **Tables read**: `tblMIOInfo`, `dbo.GetAttendanceStatusWithShiftId(@pInTime)` (function), `tblUser`, `tbl_UserRoleInfo`, `tblRoleType`, `ZAS_UserFS_MIO`, `tblMarketAttendance_Master_webapi`
- **Tables written**: `tblMarketAttendance_Master_webapi` (INSERT for `@AttType = 1`, UPDATE for `@AttType = 2`)
- **Business logic**: Near-identical shift-aware punch-in/out proc to `sp_webapi_SavePunchTotalInfo`, but only resolves org hierarchy for role `MIO` (not `AM`), and does not call the `ZASHRIS` HR sync proc — it instead contains two large commented-out cursor-based blocks (dead code) that would have pushed attendance into a `ZASHRIS..tblAttendanceRecord` table via a `FAST_FORWARD` cursor per punch-in/out.
- **Transaction/error handling**: none
- **Result set**: single scalar `@AttId`
- **Caller**: UNUSED — no application caller found

```sql

CREATE PROCEDURE [dbo].[sp_webapi_Shift_SavePunchTotalInfo]
    -- Parameters
    @attendaceDate NVARCHAR(MAX) = NULL,
    @pInTime NVARCHAR(50) = NULL,
    @pInLat NVARCHAR(50) = NULL,
    @pInLong NVARCHAR(50) = NULL,
    @pOutTime NVARCHAR(50) = NULL,
    @poutLat NVARCHAR(50) = NULL,
    @poutLong NVARCHAR(50) = NULL,
    @remarks NVARCHAR(500) = NULL,
    @empId INT,
    @EntryDate DATETIME,
    @AttType INT,
    @AttAddress NVARCHAR(MAX) = NULL
AS
BEGIN

    -- Set the attendance date to the current date

    SET @attendaceDate =  GETDATE() ;

    -- Variables for internal usage

    DECLARE @AttId INT = NULL, 
            @mioId INT, 
            @ShiftId INT = 0, 
            @AttStatus NVARCHAR(MAX) = '';

    -- Fetch MIOId for the given employee

    SELECT @mioId = MIOId 
    FROM dbo.tblMIOInfo 
    WHERE EmployeeId = @empId;

    -- Process for Punch-In

    IF (@AttType = 1)
    BEGIN

        SET @pInTime = FORMAT(GETDATE(), 'hh:mm tt');

        -- Get Shift ID and Attendance Status

        SELECT @ShiftId = ShiftId, 
               @AttStatus = AttStatus 
        FROM dbo.GetAttendanceStatusWithShiftId(@pInTime);

        -- Variables for user role and location data

        DECLARE @CompanyId INT = 0, 
                @Roltp NVARCHAR(MAX),
                @GroupId INT = NULL, 
                @GroupCode NVARCHAR(MAX), 
                @GroupName NVARCHAR(MAX),
                @RegionId INT = NULL, 
                @RegionCode NVARCHAR(MAX), 
                @RegionName NVARCHAR(MAX),
                @AreaId INT = NULL, 
                @AreaCode NVARCHAR(MAX), 
                @AreaName NVARCHAR(MAX),
                @TerritoryId INT = NULL, 
                @TerritoryCode NVARCHAR(MAX), 
                @TerritoryName NVARCHAR(MAX);

        -- Fetch user role type

        SELECT @Roltp = RoleType 
        FROM dbo.tblUser A
        LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
        LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId = tbl_UserRoleInfo.RoleTypeId
        WHERE A.EmpInfoId = @empId;

        -- Fetch location details based on role type

        IF (@Roltp = 'MIO')
        BEGIN
            SELECT @CompanyId = ISNULL(CompanyId, 0), 
                   @TerritoryId = TerritoryId, 
                   @TerritoryCode = TerritoryCode, 
                   @TerritoryName = TerritoryName,  
                   @AreaId = AreaId, 
                   @AreaCode = AreaCode, 
                   @AreaName = AreaName, 
                   @RegionId = RegionId, 
                   @RegionCode = RegionCode, 
                   @RegionName = RegionName, 
                   @GroupId = GroupId, 
                   @GroupCode = GroupCode, 
                   @GroupName = GroupName 
            FROM ZAS_UserFS_MIO 
            WHERE EmpInfoId = @empId;
        END

        -- Insert punch-in data

        INSERT INTO dbo.tblMarketAttendance_Master_webapi (
            MIOId, EmpInfoId, PunchInTime, PInLat, PInLog, AttendanceDate,
            PINCreatedDateTime, CompanyId, GroupId, GroupCode, GroupName,
            RegionId, RegionCode, RegionName, AreaId, AreaCode, AreaName,
            TerritoryId, TerritoryCode, TerritoryName, ShiftId, AttStatus
        )
        VALUES (
            @mioId, @empId, @pInTime, @pInLat, @pInLong, @attendaceDate,
            @EntryDate, @CompanyId, @GroupId, @GroupCode, @GroupName,
            @RegionId, @RegionCode, @RegionName, @AreaId, @AreaCode, @AreaName,
            @TerritoryId, @TerritoryCode, @TerritoryName, @ShiftId, @AttStatus
        );

        SET @AttId = SCOPE_IDENTITY();

		----------------------------------------------------------------------------------------
		----------------------------------------------------------------------------------------
		--                          Update to HR System Attendance
		----------------------------------------------------------------------------------------
		----------------------------------------------------------------------------------------


		--DECLARE @EmployeeId INT
  --      DECLARE @ATTDate DATETIME
  --      DECLARE @DayName NVARCHAR(500)
  --      DECLARE @ShiftId2 INT
  --      DECLARE @ShiftStart time(7)
  --      DECLARE @ShiftEnd time(7)
  --      DECLARE @InTime time(7)
  --      DECLARE @OutTime time(7)
  --      DECLARE @ComOutTime time(7)
  --      DECLARE @OverTimeDuration time(7)
  --      DECLARE @ComOverTimeDuration time(7)
  --      DECLARE @DutyDuration time(7)
  --      DECLARE @ComDutyDuration time(7)
  --      DECLARE @ATTStatus2 NVARCHAR(15)
  --      DECLARE @Remarks2 NVARCHAR(500)
		--DECLARE @AttBit NVARCHAR(15)

		--DECLARE @HRAttendancedate DATE

		----------------------------------------------------------
		--DECLARE @MyCursor CURSOR
		--SET @MyCursor = CURSOR FAST_FORWARD
		--FOR
		-----------------

		
		--SELECT TOP 1 HGI.EmpInfoId,ATT.AttendanceDate,DATENAME(WEEKDAY, @attendaceDate) AS DayName,ATT.ShiftId,ShiftInTime,ShiftOutTime,ATT.PunchInTime,ATT.AttStatus,
		--CASE 
		--	WHEN LTRIM(RTRIM(UPPER(ATT.AttStatus))) = LTRIM(RTRIM(UPPER('Present'))) THEN 'P' 
		--	WHEN LTRIM(RTRIM(UPPER(ATT.AttStatus))) = LTRIM(RTRIM(UPPER('Absent'))) THEN 'A'
		--	WHEN LTRIM(RTRIM(UPPER(ATT.AttStatus))) = LTRIM(RTRIM(UPPER('Late'))) THEN 'L'
		--END AttBit
		--,HRATT.ATTDate AS HRAttendancedate FROM tblMarketAttendance_Master_webapi AS ATT
		--LEFT JOIN tbl_Shift AS SFT ON ATT.ShiftId = SFT.ShiftId
		--LEFT JOIN tblEmpGeneralInfo AS EGI ON ATT.EmpInfoId = EGI.EmpInfoId
		--LEFT JOIN ZASHRIS..tblEmpGeneralInfo AS HGI ON LTRIM(RTRIM(UPPER(EGI.EmpMasterCode))) = LTRIM(RTRIM(UPPER(HGI.EmpMasterCode)))
		--LEFT JOIN ZASHRIS..tblAttendanceRecord AS HRATT ON ATT.AttendanceDate = HRATT.ATTDate AND HGI.EmpInfoId = HRATT.EmpId
		--WHERE 
		--	AttendanceDate = @attendaceDate 
		--	AND ATT.EmpInfoId = @empId 
		--ORDER BY ATT.AttendanceId ASC


		------------
		--OPEN @MyCursor
		--FETCH NEXT FROM @MyCursor
		--INTO @EmployeeId,@ATTDate,@DayName,@ShiftId2,@ShiftStart,@ShiftEnd,@InTime,@ATTStatus2,@AttBit,@HRAttendancedate
		
		--WHILE @@FETCH_STATUS = 0
		--BEGIN
		
		--	  -- Case 1: HR Attendance Not Found 
		--	  -- Insert

		--	  IF(@HRAttendancedate IS NULL)
		--	  BEGIN

		--		INSERT INTO ZASHRIS..tblAttendanceRecord
		--		(EmpId, ATTDate, DayName, ShiftId, ShiftStart, ShiftEnd, InTime, OutTime, ComOutTime, OverTimeDuration, ComOverTimeDuration, DutyDuration, ComDutyDuration, ATTStatus, Remarks) 
		--		VALUES (@EmployeeId, @ATTDate, @DayName,0, @ShiftStart,@ShiftEnd,@InTime,'00:00:00.0000000','00:00:00.0000000','00:00:00.0000000','00:00:00.0000000','00:00:00.0000000','00:00:00.0000000',@AttBit, @ATTStatus2);

		--	  END

		--	  -- Case 2: HR Attendance Found 
		--	  -- Update

		--	  ELSE
		--	  BEGIN

		--			UPDATE ZASHRIS..tblAttendanceRecord SET ShiftStart = @ShiftStart,ShiftEnd = @ShiftEnd, InTime = @InTime, ATTStatus = @AttBit, Remarks = @ATTStatus2  WHERE EmpId = @EmployeeId AND ATTDate = @ATTDate

		--	  END

			  
		--FETCH NEXT FROM @MyCursor 
		--INTO @EmployeeId,@ATTDate,@DayName,@ShiftId2,@ShiftStart,@ShiftEnd,@InTime,@ATTStatus2,@AttBit,@HRAttendancedate
		
		--END
		--CLOSE @MyCursor
		--DEALLOCATE @MyCursor



    END

    -- Process for Punch-Out

    IF (@AttType = 2)
    BEGIN

        SELECT @ShiftId = ShiftId, 
               @AttStatus = AttStatus 
        FROM dbo.GetAttendanceStatusWithShiftId(@pInTime);

        SELECT @AttId = AttendanceId, 
               @CompanyId = ISNULL(CompanyId, 0) 
        FROM dbo.tblMarketAttendance_Master_webapi 
        WHERE CONVERT(DATE, AttendanceDate) = CONVERT(DATE, @attendaceDate) 
              AND EmpInfoId = @empId 
              AND ShiftId = @ShiftId;

        IF (@AttId > 0)
        BEGIN
            UPDATE dbo.tblMarketAttendance_Master_webapi
            SET PunchOutTime = @pInTime,
                POutLat = @pInLat,
                POutLong = @pInLong,
                POutRemarks = @remarks,
                POUTCreatedDateTime = @EntryDate,
                CompanyId = @CompanyId
            WHERE AttendanceId = @AttId;
        END



		----------------------------------------------------------------------------------------
		----------------------------------------------------------------------------------------
		--                          Update to HR System Attendance
		----------------------------------------------------------------------------------------
		----------------------------------------------------------------------------------------


		--DECLARE @EmployeeId2 INT
  --      DECLARE @ATTDate2 DATETIME
  --      DECLARE @DayName2 NVARCHAR(500)
  --      DECLARE @ShiftId21 INT
  --      DECLARE @ShiftStart2 time(7)
  --      DECLARE @ShiftEnd2 time(7)
  --      DECLARE @InTime2 time(7)
  --      DECLARE @OutTime2 time(7)
  --      DECLARE @ComOutTime2 time(7)
  --      DECLARE @OverTimeDuration2 time(7)
  --      DECLARE @ComOverTimeDuration2 time(7)
  --      DECLARE @DutyDuration2 time(7)
  --      DECLARE @ComDutyDuration2 time(7)
  --      DECLARE @ATTStatus21 NVARCHAR(15)
  --      DECLARE @Remarks21 NVARCHAR(500)
		--DECLARE @AttBit2 NVARCHAR(15)

		--DECLARE @HRAttendancedate2 DATE

		----------------------------------------------------------
		--DECLARE @MyCursor2 CURSOR
		--SET @MyCursor2 = CURSOR FAST_FORWARD
		--FOR
		-----------------

		
		--SELECT TOP 1 HGI.EmpInfoId,ATT.AttendanceDate,DATENAME(WEEKDAY, @attendaceDate) AS DayName,ATT.ShiftId,ShiftInTime,ShiftOutTime,ATT.PunchInTime,ATT.AttStatus,
		--CASE 
		--	WHEN LTRIM(RTRIM(UPPER(ATT.AttStatus))) = LTRIM(RTRIM(UPPER('Present'))) THEN 'P' 
		--	WHEN LTRIM(RTRIM(UPPER(ATT.AttStatus))) = LTRIM(RTRIM(UPPER('Absent'))) THEN 'A'
		--	WHEN LTRIM(RTRIM(UPPER(ATT.AttStatus))) = LTRIM(RTRIM(UPPER('Late'))) THEN 'L'
		--END AttBit
		--,HRATT.ATTDate AS HRAttendancedate FROM tblMarketAttendance_Master_webapi AS ATT
		--LEFT JOIN tbl_Shift AS SFT ON ATT.ShiftId = SFT.ShiftId
		--LEFT JOIN tblEmpGeneralInfo AS EGI ON ATT.EmpInfoId = EGI.EmpInfoId
		--LEFT JOIN ZASHRIS..tblEmpGeneralInfo AS HGI ON LTRIM(RTRIM(UPPER(EGI.EmpMasterCode))) = LTRIM(RTRIM(UPPER(HGI.EmpMasterCode)))
		--LEFT JOIN ZASHRIS..tblAttendanceRecord AS HRATT ON ATT.AttendanceDate = HRATT.ATTDate AND HGI.EmpInfoId = HRATT.EmpId
		--WHERE 
		--	AttendanceDate = @attendaceDate
		--	AND ATT.EmpInfoId = @empId 
		--ORDER BY ATT.AttendanceId DESC


		------------
		--OPEN @MyCursor2
		--FETCH NEXT FROM @MyCursor2
		--INTO @EmployeeId2,@ATTDate2,@DayName2,@ShiftId21,@ShiftStart2,@ShiftEnd2,@InTime2,@ATTStatus21,@AttBit2,@HRAttendancedate2
		
		--WHILE @@FETCH_STATUS = 0
		--BEGIN
		
		--	  -- Case 1: HR Attendance Not Found 
		--	  -- Insert

		--	  IF(@HRAttendancedate2 IS NULL)
		--	  BEGIN

		--		INSERT INTO ZASHRIS..tblAttendanceRecord
		--		(EmpId, ATTDate, DayName, ShiftId, ShiftStart, ShiftEnd, InTime, OutTime, ComOutTime, OverTimeDuration, ComOverTimeDuration, DutyDuration, ComDutyDuration, ATTStatus, Remarks) 
		--		VALUES (@EmployeeId2, @ATTDate2, @DayName2,0, @ShiftStart2,@ShiftEnd2,'00:00:00.0000000',@InTime2,'00:00:00.0000000','00:00:00.0000000','00:00:00.0000000','00:00:00.0000000','00:00:00.0000000',@AttBit2, @ATTStatus21);

		--	  END

		--	  -- Case 2: HR Attendance Found 
		--	  -- Update

		--	  ELSE
		--	  BEGIN

		--			UPDATE ZASHRIS..tblAttendanceRecord SET OutTime = @InTime2  WHERE EmpId = @EmployeeId2 AND ATTDate = @ATTDate2

		--	  END

			  
		--FETCH NEXT FROM @MyCursor2 
		--INTO @EmployeeId2,@ATTDate2,@DayName2,@ShiftId21,@ShiftStart2,@ShiftEnd2,@InTime2,@ATTStatus21,@AttBit2,@HRAttendancedate2
		
		--END
		--CLOSE @MyCursor2
		--DEALLOCATE @MyCursor2



    END

    -- Return the Attendance ID

    SELECT @AttId;

END;



```

## tbl_INV_CentralStoreStoIssue

- **Parameters**: `@StockInTransfarId INT`
- **Tables read**: `tblStockInTransfar`, `tblCentralStore`, `tbl_INV_CentralStore_WeightedAvg`
- **Tables written**: `tbl_INV_CentralStore_WeightedAvg` (INSERT)
- **Business logic**: Records a weighted-average-cost stock-out from the central store for an STO (stock transfer order) issue: looks up the product/quantity/batch from the stock-in-transfer row, reads the product's latest weighted-average snapshot (opening qty/rate/value), computes the issue value (`picked qty * opening rate`), the new closing quantity (`opening qty - picked qty`) and closing value, and inserts a new weighted-average ledger row recording the issue as a negative quantity/value movement (hardcoded `Company_Id`/`Warehouse_Id = 16`).
- **Transaction/error handling**: `SET NOCOUNT ON`; no explicit transaction and no TRY/CATCH
- **Result set**: none
- **Caller**: `Library.DAL/SInventory_DAL/RequisitionDAL.cs`, method `ProcessWeightedAvg(int stockInTransferId)`, called via `aCommonInternalDal.UpdateAction("tbl_INV_CentralStoreStoIssue", aSqlParameterList)` — a commented-out line directly above it shows the call used to target a different proc (`sp_DeliveryConfirmationFullPosting`), suggesting this was swapped in as a replacement

```sql

-- =======================================================================================
-- 2. tbl_INV_CentralStoreStoIssue (STO Issue: Central Store WAC Stock Out)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[tbl_INV_CentralStoreStoIssue] 
	@StockInTransfarId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ProductId INT, @PickingQty DECIMAL(18,3), @ReceiveId INT, @BatchNo NVARCHAR(100), @ProductCode NVARCHAR(100);
	DECLARE @WH_Current_Stock DECIMAL(18,3) = 0, @WH_Opening_Rate DECIMAL(18,4) = 0, @WH_Opening_Value DECIMAL(18,4) = 0;
	DECLARE @WH_Current_Issue_Value DECIMAL(18,4) = 0, @WH_Closing_Stock DECIMAL(18,3) = 0, @WH_Closing_Rate DECIMAL(18,4) = 0, @WH_Closing_Value DECIMAL(18,4) = 0;
	DECLARE @StockInDate DATETIME;

	SELECT 
		@ProductId = CS.ProductId,
		@PickingQty = SIT.PickingQty,
		@ReceiveId = SIT.ReceiveId,
		@BatchNo = SIT.BatchNo,
		@ProductCode = SIT.ProductCode,
		@StockInDate = GETDATE()
	FROM dbo.tblStockInTransfar SIT WITH (NOLOCK)
	INNER JOIN dbo.tblCentralStore CS WITH (NOLOCK) ON SIT.ReceiveId = CS.ReceiveId
	WHERE SIT.StockInTransfarId = @StockInTransfarId;

	SELECT TOP 1 
		@WH_Current_Stock = Close_Qty,
		@WH_Opening_Rate = Avg_Rate,
		@WH_Opening_Value = Close_Amt
	FROM dbo.tbl_INV_CentralStore_WeightedAvg WITH (NOLOCK)
	WHERE Item_Id = @ProductId
	ORDER BY Weighted_Avg_Id DESC;

	SET @WH_Current_Issue_Value = ISNULL(@PickingQty, 0) * ISNULL(@WH_Opening_Rate, 0);
	SET @WH_Closing_Stock = ISNULL(@WH_Current_Stock, 0) - ISNULL(@PickingQty, 0);
	SET @WH_Closing_Rate = ISNULL(@WH_Opening_Rate, 0);
	SET @WH_Closing_Value = @WH_Closing_Stock * @WH_Closing_Rate;

	INSERT INTO dbo.tbl_INV_CentralStore_WeightedAvg
	(
		Item_Id, Trans_Date, Curr_Qty, Curr_Rate, Curr_Amt,
		Close_Qty, Avg_Rate, Close_Amt, Company_Id, Warehouse_Id
	)
	VALUES
	(
		@ProductId, @StockInDate, -@PickingQty, @WH_Opening_Rate, -@WH_Current_Issue_Value,
		@WH_Closing_Stock, @WH_Closing_Rate, @WH_Closing_Value, 16, 16
	);
END;

```

## tbl_MC_MaterialConversionCentralStoreRcv

- **Parameters**: `@MasterId INT` (a `tblWHStockInMaster` id)
- **Tables read**: `tblWHStockInMaster`, `tblWHStockInDetail`, `CSTL_ERPDB..tbl_MC_MaterialConversionMaster` (cross-database), `CSTL_ERPDB..Table_Inventory_TranscationTypes`, `CSTL_ERPDB..Table_Inventory_Types`, `tblProduct`, `tblWearhouse`, `tblCentralStore` (existence check), `SalesRollDB_ZAS..tblCentralStore`, `SalesRollDB_ZAS..tbl_INV_CentralStore_WeightedAvg`, `dbo.tbl_INV_CentralStore_WeightedAvg`, `dbo.tbl_INV_Central_BatchWiseStock`
- **Tables written**: `tblCentralStore` (INSERT, conditional on not already migrated), `tbl_INV_CentralStore_WeightedAvg` (INSERT, conditional on not already present), `tbl_INV_Central_BatchWiseStock` (INSERT, conditional on not already present)
- **Business logic**: Receives finished goods from a material-conversion (raw-material-to-product) batch into the central store, one warehouse-stock-in-detail line at a time via a cursor: for each line, if it hasn't already been migrated into `tblCentralStore` (checked by `MigoDetailID`), computes a weighted-average opening/closing quantity and rate and inserts the stock-in row plus a legacy weighted-average snapshot on the same row; separately (and idempotently, via an `IF NOT EXISTS`) inserts a normalized weighted-average ledger row and a batch-wise stock row. Reads several tables from external databases (`CSTL_ERPDB`, `SalesRollDB_ZAS`) alongside the current one.
- **Transaction/error handling**: none (no explicit transaction; each cursor iteration's inserts are unguarded by TRY/CATCH)
- **Result set**: none
- **Caller**: UNUSED — no application caller found (the sibling procedure `tbl_INV_CentralStoreStoIssue`, in this same batch, IS called from `Library.DAL/SInventory_DAL/RequisitionDAL.cs`, but this material-conversion receive counterpart has no matching caller in the app source)
```sql
CREATE PROCEDURE [dbo].[tbl_MC_MaterialConversionCentralStoreRcv] 
	
	@MasterId INT

AS
BEGIN
		
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
		
		      
		--------------------------------------------------------
		DECLARE @MyCursor CURSOR
		SET @MyCursor = CURSOR FAST_FORWARD
		FOR
		---------------


		SELECT 
            PD.ProductId,
            PD.ProductCode,
            PD.ProductName,
            PD.PackSize,
            WD.Qty AS FinalOutputQuantity,
            M.MaterialConversionDate,
            WM.ChallanNo,
            WM.ChallanDate, 
            WD.Price AS CostPerUnit,
		    WD.Qty*WD.Price AS TotalPrice,
            WD.MaterialConversionMasterId,
            WH.WearhouseId,
            M.MaterialConversionCode,
            PD.CompanyId,
		    M.ApproveDate,
            TSC.TranscationTypeId,
            ITS.InventoryTypeId,
            ITS.InventoryType,
            WD.WHStockInDetailID, 
            WD.MaterialConversionMasterId, 
            WD.Batch, 
            ISNULL(WD.MfgDate,GETDATE()),
            ISNULL(WD.ExpDate,GETDATE())  
        FROM tblWHStockInMaster AS WM
		LEFT JOIN tblWHStockInDetail AS WD ON WM.WHStockInMasterID = WD.WHStockInMasterID
		LEFT JOIN CSTL_ERPDB..tbl_MC_MaterialConversionMaster AS M ON WD.MaterialConversionMasterId = M.MaterialConversionMasterId
		LEFT JOIN CSTL_ERPDB..Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tbl_MC_MaterialConversionMaster')))
		LEFT JOIN CSTL_ERPDB..Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1 -- Default - Raw Material
		INNER JOIN tblProduct AS PD ON WD.ProductId = PD.ProductId
		INNER JOIN tblWearhouse AS WH ON PD.CompanyId = WH.CompanyId
		WHERE WM.WHStockInMasterID = @MasterId


		----------
		OPEN @MyCursor
		FETCH NEXT FROM @MyCursor
		INTO @ProductId,@ProductCode,@ProductName,@PackSize,@FinalOutputQuantity,
		@MaterialConversionDate,@ChallanNo,@ChallanDate,@CostPerUnit,@TotalPrice,@MaterialConversionMasterId,@WearhouseId,
		@MaterialConversionCode,@CompanyId,@ApproveDate,@TranscationTypeId,@InventoryTypeId,@InventoryType,
		@WHStockInDetailID,@MaterialConversionMasterId, @Batch, @MfgDate, @ExpDate
		
		WHILE @@FETCH_STATUS = 0
		BEGIN


		--PRINT @InsertBy
		

		DECLARE @CheckCount int = 0
		SELECT @CheckCount = COUNT(*) from tblCentralStore WHERE MigoDetailID = @WHStockInDetailID
		--MigoDetailID = 18055

		PRINT @CheckCount
		
		
		IF (@CheckCount = 0)
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
			                           FROM SalesRollDB_ZAS..tblCentralStore
			                           WHERE Company_Id = @CompanyId AND ProductId = @ProductId
			                           ORDER BY Trans_Date DESC), 0)
			
			SET @Opening_Amount = ISNULL((SELECT TOP 1 Close_Amt 
			                              FROM SalesRollDB_ZAS..tblCentralStore
			                              WHERE Company_Id = @CompanyId AND ProductId = @ProductId
			                              ORDER BY Trans_Date DESC), 0)
			
			SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty ELSE 0 END
			
			SET @Closing_Qty = @Opening_Qty + @FinalOutputQuantity
			SET @Closing_Amount = @Opening_Amount + @TotalPrice
			SET @Average_Rate = CASE WHEN @Closing_Qty > 0 THEN @Closing_Amount / @Closing_Qty ELSE 0 END

			-- Insert Quary

			INSERT INTO tblCentralStore
			       (ProductId
			       ,ProductCode
			       ,ProductName
			       ,PackSize
			       ,BatchNo
			       ,Quantity
			       ,MfgDate
			       ,ExpDate
			       ,ReceiveDate
			       ,ChalanNo
			       ,ChalanDate
			       ,StockInQty
			       ,UnitPrice
			       ,TotalPrice
			       ,VATPerUnit
			       ,TotalVAT
			       ,TotalAmount
			       ,StockCondition
			       ,MigoDetailID
			       --,DeveloperRemarks
			       ,ProductStockType
			       ,InternalNoteNo
			       ,WarehouseId
			       ,Remarks,MaterialConversionMasterId,
				   
				   -- Weighted Average

					Company_Id,
			        --Warehouse_Id,
			        Mst_Trans_Id,
			        Trans_Id,
			        --Item_Id,
			        --Item_Code,
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
			       (@ProductId
			       ,@ProductCode
			       ,@ProductName
			       ,@PackSize
			       ,@Batch
			       ,@FinalOutputQuantity
			       ,@MfgDate
			       ,@ExpDate
			       ,@MaterialConversionDate
			       ,@ChallanNo
			       ,@ChallanDate
			       ,@FinalOutputQuantity
			       ,@CostPerUnit
			       ,@TotalPrice
			       ,@VATPerUnit
			       ,@TotalVAT
			       ,@TotalPrice
			       ,@StockCondition
			       ,@WHStockInDetailID
			       --,@DeveloperRemarks
			       ,@ProductStockType
			       ,@MaterialConversionCode
			       ,@WearhouseId
			       ,@Remarks,@MaterialConversionMasterId,
				   
				   -- Weighted Average

					@CompanyId,
			        --@DeliveryLocationId1, -- Warehouse_Id
			        @MasterId, -- Mst_Trans_Id
			        @WHStockInDetailID, -- Trans_Id
			        --@TransItemDescriptionId,
			        --@ItemDescriptionCode,
			        @MaterialConversionCode, -- Ref_No
			        GETDATE(), -- Trans_Date
			        @MaterialConversionDate, -- Actual_Date
			        @ApproveDate, -- Entry_Date
			        @TranscationTypeId, -- Trans_Type_Id
			        @InventoryTypeId, -- Inv_Type_Id
			        @InventoryType, -- Inv_Type
			        @Opening_Qty, -- OP_Qty
			        @Opening_Rate, -- OP_Rate
			        @Opening_Amount, -- OP_Amt
			        @FinalOutputQuantity, -- Curr_Qty
			        @CostPerUnit, -- Curr_Rate
			        @TotalPrice, -- Curr_Amt
			        @Closing_Qty, -- Close_Qty
			        @Average_Rate, -- Avg_Rate
			        @Closing_Amount -- Close_Amt)
					)

				END


			-- Insert Into tbl_INV_CentralStore_WeightedAvg

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
			    WHERE Mst_Trans_Id = @MasterId
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
			        @MasterId, @WHStockInDetailID, @ProductCode, @MaterialConversionCode, GETDATE(), @MaterialConversionDate, @ApproveDate,
			        @TranscationTypeId, @InventoryTypeId, @InventoryType, @CompanyId, @WearhouseId, @ProductId,
			        @WH_Opening_Qty, @WH_Opening_Rate, @WH_Opening_Amount,
			        @FinalOutputQuantity, @CostPerUnit, @TotalPrice,
			        @WH_Closing_Qty, @WH_Average_Rate, @WH_Closing_Amount,
			        NULL
			    );

				DECLARE @Weighted_Avg_Id_New INT = 0
				SET @Weighted_Avg_Id_New = SCOPE_IDENTITY()

				-- Insert Into Batch Stock Table

				IF (@Weighted_Avg_Id_New > 0)
				BEGIN

				    DECLARE @STCount INT = 0;
				
				    SELECT @STCount = COUNT(C.Batch_Stock_Id)
				    FROM dbo.tbl_INV_Central_BatchWiseStock AS C
				    WHERE C.Trans_Qty > 0
				      AND C.Weighted_Avg_Id = @Weighted_Avg_Id_New
				      AND C.Batch_No        = @Batch
				      AND ISNULL(C.Trans_Qty,0) = @FinalOutputQuantity
				      AND ISNULL(C.WHStockInDetailId,0) = ISNULL(@WHStockInDetailID,0)
				
				    IF (@STCount = 0)
				    BEGIN
				        INSERT INTO dbo.tbl_INV_Central_BatchWiseStock
				        (
				            Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty,
				            Remarks, WHStockInDetailId
				        )
				        VALUES
				        (
				            @Weighted_Avg_Id_New, @Batch, @MfgDate, @ExpDate, @FinalOutputQuantity,
				            N'WH Stock In', @WHStockInDetailId
				        );
				    END
				END

			END

		FETCH NEXT FROM @MyCursor 
		INTO @ProductId,@ProductCode,@ProductName,@PackSize,@FinalOutputQuantity,
		@MaterialConversionDate,@ChallanNo,@ChallanDate,@CostPerUnit,@TotalPrice,@MaterialConversionMasterId,@WearhouseId,
		@MaterialConversionCode,@CompanyId,@ApproveDate,@TranscationTypeId,@InventoryTypeId,@InventoryType,
		@WHStockInDetailID,@MaterialConversionMasterId, @Batch, @MfgDate, @ExpDate
		
		END
		CLOSE @MyCursor
		DEALLOCATE @MyCursor

END
```
