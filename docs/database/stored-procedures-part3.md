# Stored Procedures — Batch 3

Documentation for the 131 stored procedures in batch 3 of 4, reverse-engineered directly from the live `SalesrollDB_ZAS` T-SQL definitions (ground truth: `procs_batch3.txt`, verified complete — every block carries a `DEF-END` marker and a full body). Application callers were located by grepping the literal procedure name across `Library.DAL`, `Library.BLL`, and `Solution.Web`; where none was found the procedure is marked `UNUSED — no application caller found` (confirmed with a repo-wide grep, not just the three call-site folders). Tables read/written and transaction/error handling are read directly out of each body. Business-logic sentences are derived only from constructs actually present in the body (tables touched, cursor usage, literal filter/tag values); where the body's intent could not be safely reduced to a sentence, it is marked `UNKNOWN — HUMAN VERIFICATION REQUIRED` and the full body is given immediately below for manual review.

## `sp_ProductDestroyPosting`

**Parameters:**
- `@DestroyId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster, CSTL_ERP_ACCDB..tblFinancialYear, CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew, tblArea, tblCentralStore, tblCompanyUnit, tblCustMaster, tblDCStore, tblDCStoreFreeze, tblDcWiseAreaInfo, tblDeliveryManInfo, tblDestroyProduct, tblEmpGeneralInfo, tblInvoice, tblMarket, tblProduct, tblStockConditionFreeze, tblTerritory, tblUnitPrice
**Tables written:** INSERT → CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster, ZAS_ACCDB..tblDebitCreditVoucherDetail; UPDATE → tblDestroyProduct

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** GL-posting procedure restricted to `CompanyId = 16`: for each qualifying row (cursor-driven), it builds a voucher code and financial-year id, inserts a header row into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster` (`VoucherType='Journal'`, `Particular='ProductDestroy'`) and a matching debit/credit line pair into `tblDebitCreditVoucherDetail`, using GL account ids looked up from `tblSalesIntegrationMapingNew` for that `Particular`; it then sets `tblDestroyProduct.IsPosting` to 1 if a voucher now exists for the record, else 0.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProductDestroyPosting] 

	@DestroyId INT
	
AS
BEGIN
	

	DECLARE @VoucherMasterId INT
	
	DECLARE @TempCompanyId INT = 0
	
	SELECT @TempCompanyId = CompanyId FROM tblDestroyProduct AS DSP
	INNER JOIN tblDCStoreFreeze AS DFZS ON DSP.DcFreezeId = DFZS.DCStoreFreezeId
	INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = DFZS.DCStoreId
	INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
	INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = DFZS.ComUnitId
	WHERE DSP.DestroyId = @DestroyId  



	IF(@TempCompanyId = 16)
	BEGIN
			
			DECLARE @DCFreezeId INT
			DECLARE @InvDate DATETIME 
			DECLARE @CompanyId INT
			DECLARE @CostPrice DECIMAL (18,2)
			DECLARE @StockOutQty DECIMAL (18,2)


			DECLARE @StockOutDate DATETIME 
			DECLARE @Reason NVARCHAR(500)
			        
			--------------------------------------------------------
			DECLARE @MyCursor CURSOR
			SET @MyCursor = CURSOR FAST_FORWARD
			FOR
			---------------


			SELECT DFZS.DCStoreFreezeId,DSP.StockOutQty,UNT.CompanyId,CTRS.UnitPrice,DSP.StockOutDate,DSP.Reason FROM tblDestroyProduct AS DSP
			INNER JOIN tblDCStoreFreeze AS DFZS ON DSP.DcFreezeId = DFZS.DCStoreFreezeId
			INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = DFZS.DCStoreId
			INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
			INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = DFZS.ComUnitId
			WHERE DSP.DestroyId = @DestroyId  

			--DCS.ProductCode = '372107'  


			----------
			OPEN @MyCursor
			FETCH NEXT FROM @MyCursor
			INTO @DCFreezeId,@StockOutQty,@CompanyId,@CostPrice,@StockOutDate,@Reason
			
			WHILE @@FETCH_STATUS = 0
			BEGIN
			

			-- Master Variable Info
			-------------------------------------

			DECLARE @VoucherType NVARCHAR(MAX) ='Journal'
			DECLARE @VoucherCode NVARCHAR(MAX)

			DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@StockOutDate)), 3, 3)   
			DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@StockOutDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@StockOutDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@StockOutDate)) END)
			DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@StockOutDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@StockOutDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@StockOutDate)) END)
			SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster 
			WHERE @StockOutDate = @StockOutDate
			
				IF(@VoucherType='DebitVoucherBank')
				BEGIN
					SET @VoucherCode='DVB-'+@VoucherCode	
				END
				IF(@VoucherType='DebitVoucherCash')
				BEGIN
					SET @VoucherCode='DVC-'+@VoucherCode
				END
				IF(@VoucherType='CreditVoucherBank')
				BEGIN
					SET @VoucherCode='CVB-'+@VoucherCode	
				END
				IF(@VoucherType='CreditVoucherCash')
				BEGIN
					SET @VoucherCode='CVC-'+@VoucherCode
				END
				IF(@VoucherType='FundTransfer')
				BEGIN
					SET @VoucherCode='FUT-'+@VoucherCode
				END

				IF(@VoucherType='Journal')
				BEGIN
					SET @VoucherCode='JVP-'+@VoucherCode
				END


			DECLARE @FinYearId NVARCHAR(MAX) = ''

			SELECT @FinYearId = FinancialYearId FROM CSTL_ERP_ACCDB..tblFinancialYear 
			WHERE CompanyId = @CompanyId AND Status IN ('Active') AND @StockOutDate BETWEEN StartDate AND EndDate

			----------------------------------

			-- Master Information Posting

			INSERT	INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster
			(
			    VoucherCode,
			    CompanyInfoId,
			    FinancialYearId,
			    PostingDate,
			    VoucherCategoryID,
			    CurrencyId,
			    CurrencyCovRate,
			    Narration,
			    TotalAmount,
			    TotalAmountBDT,
			    EntryBy,
			    EntryDate,
			    VoucherType, 
			    JournalType,
			    DestroyId,
				IsCostCenter,
				IsInvoice,
				isManualVCRNo,
				IsExportInvoice,
				IsImportInvoice

			)
			VALUES
			(   
			    @VoucherCode,       -- VoucherCode - nvarchar(max)
			    @CompanyId,         -- CompanyInfoId - int
			    ISNULL(@FinYearId,0),         -- FinancialYearId - int
			    @StockOutDate, -- PostingDate - datetime
			    2,         -- VoucherCategoryID - int
			    1,         -- CurrencyId - int
			    1.00,      -- CurrencyCovRate - decimal(18, 2)
			    'Destroy Product - ( Destroy Date: ' + CONVERT(NVARCHAR(50), GETDATE(), 101) + ', Stock Value: ' + CONVERT(NVARCHAR(50), (@CostPrice*@StockOutQty)) + ' )',       -- Narration - nvarchar(max)
			    0.00,      -- TotalAmount - decimal(18, 2)
			    @CostPrice*@StockOutQty,      -- TotalAmountBDT - decimal(18, 2)
			    'Auto Process',       -- EntryBy - nvarchar(max)
			    CURRENT_TIMESTAMP, -- EntryDate - datetime	  
			    'Journal',       -- VoucherType - nvarchar(50)
			    'Receivable',       -- JournalType - nvarchar(50)
			    @DestroyId ,      -- ProformaId -int
				0,0,0,0,0
			)


			-- Detail Information Posting

			SET @VoucherMasterId = SCOPE_IDENTITY()

			DECLARE	@DrGl INT 
			SELECT @DrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProductDestroy') AND BalanceType IN ('Dr')

			-- Debit

			INSERT INTO ZAS_ACCDB..tblDebitCreditVoucherDetail
			(
			    VoucherMasterId,
			    AccountId,
			    DebitorCredit,
			    DebitAmount,
			    CreditAmount,
			    DebitAmountBDT,
			    CreditAmountBDT
			)
			VALUES
			(   @VoucherMasterId,    -- VoucherMasterId - int
			    @DrGl,    -- AccountId - int
			    'Debit',  -- DebitorCredit - nvarchar(50)
			    0.00, -- DebitAmount - decimal(18, 2)
			    0.00, -- CreditAmount - decimal(18, 2)
			    @CostPrice*@StockOutQty, -- DebitAmountBDT - decimal(18, 2)
			    0.00 -- CreditAmountBDT - decimal(18, 2)

			)

			--- Credit

			DECLARE	@CrGl INT 
			SELECT @CrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProductDestroy') AND BalanceType IN ('Cr')
			
			INSERT INTO ZAS_ACCDB..tblDebitCreditVoucherDetail
			(
			    VoucherMasterId,
			    AccountId,
			    DebitorCredit,
			    DebitAmount,
			    CreditAmount,
			    DebitAmountBDT,
			    CreditAmountBDT
			)
			VALUES
			(   @VoucherMasterId,    -- VoucherMasterId - int
			    @CrGl,    -- AccountId - int
			    'Credit',  -- DebitorCredit - nvarchar(50)
			    0.00, -- DebitAmount - decimal(18, 2)
			    0.00, -- CreditAmount - decimal(18, 2)
			    0.00, -- DebitAmountBDT - decimal(18, 2)
			    @CostPrice*@StockOutQty -- CreditAmountBDT - decimal(18, 2)
			)


			DECLARE @Count INT = 0 

			SELECT @Count = COUNT(DestroyId) FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster WHERE DestroyId = @DestroyId

			IF (@Count > 0)
				BEGIN
				   
				   UPDATE dbo.tblDestroyProduct SET IsPosting = 1 WHERE DestroyId = @DestroyId

				END

			ELSE

				BEGIN

				UPDATE tblDestroyProduct SET IsPosting = 0 WHERE DestroyId = @DestroyId

				END
			
			
			FETCH NEXT FROM @MyCursor
			INTO @DCFreezeId,@StockOutQty,@CompanyId,@CostPrice,@StockOutDate,@Reason

			
			END
			CLOSE @MyCursor
			DEALLOCATE @MyCursor

	END


	

	
END


--SELECT * FROM tblInvoice

--SELECT * FROM dbo.tblDeliveryManInfo
--INNER JOIN dbo.tblEmpGeneralInfo ON tblEmpGeneralInfo.EmpInfoId = tblDeliveryManInfo.EmpInfoId
--WHERE DeliveryManId = 4


--SELECT * FROM dbo.tblDCStoreFreeze


--SELECT DISTINCT * FROM dbo.tblCustMaster AS CSTMR
--LEFT JOIN dbo.tblMarket AS MKT ON MKT.MarketId = CSTMR.MarketId
--LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MKT.TerritoryId
--LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId
--INNER JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARA.AreaId


--SELECT * FROM dbo.tblStockConditionFreeze


--SELECT F.DCStoreFreezeId,F.DCStoreId,F.ProductCode,F.ProductName,F.BatchNo,F.ExpDate,F.StockQty,
--(U.UnitPrice*F.StockQty) AS Amount,F.StockCondition,F.Remarks
--FROM dbo.tblDCStoreFreeze F
--INNER JOIN dbo.tblUnitPrice U ON F.ProductCode = U.ProductCode
--INNER JOIN dbo.tblProduct P ON F.ProductCode = P.ProductCode
--INNER JOIN dbo.tblCompanyUnit AS IUNT ON IUNT.ComUnitId = F.ComUnitId
--WHERE StockQty > 0 AND U.IsActive = 1   AND F.DCStoreFreezeId = 5621 ORDER BY F.ExpDate 
```

---

## `sp_ProformaFullReturnPosting`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster, CSTL_ERP_ACCDB..tblFinancialYear, CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew, ZAS_ACCDB..tblDebitCreditVoucherMaster, tblCentralStore, tblCompanyUnit, tblCustMaster, tblDCStore, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail
**Tables written:** INSERT → CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail, CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster; UPDATE → tblInvoice

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** GL-posting procedure restricted to `CompanyId = 16`: for each qualifying row (cursor-driven), it builds a voucher code and financial-year id, inserts a header row into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster` (`VoucherType='Journal'`, `Particular='ProformaFullReject'`) and a matching debit/credit line pair into `tblDebitCreditVoucherDetail`, using GL account ids looked up from `tblSalesIntegrationMapingNew` for that `Particular`; it then sets `tblInvoice.IsPosting` to 1 if a voucher now exists for the record, else 0.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1664` — `return aCommonInternalDal.UpdateAction("sp_ProformaFullReturnPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProformaFullReturnPosting] 

	@InvoiceId INT
	
AS
BEGIN


	DECLARE @TempCompanyId INT = 0
	
	SELECT @TempCompanyId = CompanyId FROM tblInvoice AS INV 
	LEFT JOIN tblCompanyUnit AS CU ON INV.ComUnitId = CU.ComUnitId
	WHERE InvoiceId = @InvoiceId


	IF(@TempCompanyId = 16)
	BEGIN

		DECLARE @ProformaInvId INT
		DECLARE @InvDate DATETIME
		DECLARE @CompanyId INT
		DECLARE @CostPrice NVARCHAR(500)
		DECLARE @InvoiceNo NVARCHAR(500)
		
		DECLARE @VoucherMasterId INT      
		--------------------------------------------------------
		DECLARE @MyCursor CURSOR
		SET @MyCursor = CURSOR FAST_FORWARD
		FOR
		---------------
		
		SELECT INV.InvoiceId,INV.UpdateDate,UNT.CompanyId,
		SUM(INVD.TotalPrice) + SUM(INVD.TotalPriceVatAmount) AS TotalPrice,INV.InvoiceNo FROM tblInvoice AS INV
		LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
		INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = INVD.DCStoreId
		INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
		INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
		WHERE INV.InvoiceId = @InvoiceId 
		GROUP BY INV.InvoiceId,UNT.CompanyId,INV.UpdateDate,INV.InvoiceNo
			
		----------
		OPEN @MyCursor
		FETCH NEXT FROM @MyCursor
		INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
		

		-- Master Variable Info
		-------------------------------------

		DECLARE @VoucherType NVARCHAR(MAX) ='Journal'
		DECLARE @VoucherCode NVARCHAR(MAX)

		DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@InvDate)), 3, 3)   
		DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) END)
		DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@InvDate)) END)
		SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster 
		WHERE @InvDate = @InvDate
		
			IF(@VoucherType='DebitVoucherBank')
			BEGIN
				SET @VoucherCode='DVB-'+@VoucherCode	
			END
			IF(@VoucherType='DebitVoucherCash')
			BEGIN
				SET @VoucherCode='DVC-'+@VoucherCode
			END
			IF(@VoucherType='CreditVoucherBank')
			BEGIN
				SET @VoucherCode='CVB-'+@VoucherCode	
			END
			IF(@VoucherType='CreditVoucherCash')
			BEGIN
				SET @VoucherCode='CVC-'+@VoucherCode
			END
			IF(@VoucherType='FundTransfer')
			BEGIN
				SET @VoucherCode='FUT-'+@VoucherCode
			END

			IF(@VoucherType='Journal')
			BEGIN
				SET @VoucherCode='JVP-'+@VoucherCode
			END

		DECLARE @FinYearId NVARCHAR(MAX) = ''

		SELECT @FinYearId = FinancialYearId FROM CSTL_ERP_ACCDB..tblFinancialYear 
		WHERE CompanyId = @CompanyId AND Status IN ('Active') AND @InvDate BETWEEN StartDate AND EndDate

		----------------------------------

		-- Master Information Posting

		INSERT	INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster
		(
		    VoucherCode,
		    CompanyInfoId,
		    FinancialYearId,
		    PostingDate,
		    VoucherCategoryID,
		    CurrencyId,
		    CurrencyCovRate,
		    Narration,
		    TotalAmount,
		    TotalAmountBDT,
		    EntryBy,
		    EntryDate,
		    VoucherType, 
		    JournalType,
		    ProformaId,
			IsCostCenter,
			IsInvoice,
			isManualVCRNo,
			IsExportInvoice,
			IsImportInvoice

		)
		VALUES
		(   
		    @VoucherCode,       -- VoucherCode - nvarchar(max)
		    @CompanyId,         -- CompanyInfoId - int
		    @FinYearId,         -- FinancialYearId - int
		    @InvDate, -- PostingDate - datetime
		    2,         -- VoucherCategoryID - int
		    1,         -- CurrencyId - int
		    1.00,      -- CurrencyCovRate - decimal(18, 2)
		    'Proforma Full Return - ( Invoice No:' + @InvoiceNo + ' ,  Invoice Date: ' + CONVERT(NVARCHAR(50), @InvDate, 101) + ', Stock Value: ' + CONVERT(NVARCHAR(50), @CostPrice) + ' )',       -- Narration - nvarchar(max)
		    0.00,      -- TotalAmount - decimal(18, 2)
		    @CostPrice,      -- TotalAmountBDT - decimal(18, 2)
		    'Auto Process',       -- EntryBy - nvarchar(max)
		    @InvDate, -- EntryDate - datetime	  
		    'Journal',       -- VoucherType - nvarchar(50)
		    'Adjustable',       -- JournalType - nvarchar(50)
		    @ProformaInvId,      -- ProformaId -int
			0,0,0,0,0
		)


		-- Detail Information Posting

		SET @VoucherMasterId = SCOPE_IDENTITY()

		--DECLARE	@VoucherMasterId INT 
		--SELECT @VoucherMasterId = VoucherMasterId FROM ZAS_ACCDB..tblDebitCreditVoucherMaster WHERE VoucherCode = @VoucherCode

		DECLARE	@DrGl INT 
		SELECT @DrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProformaFullReject') AND BalanceType IN ('Dr')

		--SELECT * FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProformaFullReject') AND BalanceType IN ('Cr')

		-- Debit

		INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
		(
		    VoucherMasterId,
		    AccountId,
		    DebitorCredit,
		    DebitAmount,
		    CreditAmount,
		    DebitAmountBDT,
		    CreditAmountBDT
		)
		VALUES
		(   @VoucherMasterId,    -- VoucherMasterId - int
		    @DrGl,    -- AccountId - int
		    'Debit',  -- DebitorCredit - nvarchar(50)
		    0.00, -- DebitAmount - decimal(18, 2)
		    0.00, -- CreditAmount - decimal(18, 2)
		    @CostPrice, -- DebitAmountBDT - decimal(18, 2)
		    0.00 -- CreditAmountBDT - decimal(18, 2)

		)

		--- Credit

		DECLARE	@CrGl INT 
		SELECT @CrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProformaFullReject') AND BalanceType IN ('Cr')
		
		INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
		(
		    VoucherMasterId,
		    AccountId,
		    DebitorCredit,
		    DebitAmount,
		    CreditAmount,
		    DebitAmountBDT,
		    CreditAmountBDT
		)
		VALUES
		(   @VoucherMasterId,    -- VoucherMasterId - int
		    @CrGl,    -- AccountId - int
		    'Credit',  -- DebitorCredit - nvarchar(50)
		    0.00, -- DebitAmount - decimal(18, 2)
		    0.00, -- CreditAmount - decimal(18, 2)
		    0.00, -- DebitAmountBDT - decimal(18, 2)
		    @CostPrice -- CreditAmountBDT - decimal(18, 2)
		)


		DECLARE @Count INT = 0 

		SELECT @Count = COUNT(ProformaId) FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster WHERE ProformaId = @ProformaInvId

		IF (@Count > 0)
			BEGIN
			   
			   UPDATE tblInvoice SET IsPosting = 1 WHERE InvoiceId = @ProformaInvId

			END
		ELSE
			BEGIN

			UPDATE tblInvoice SET IsPosting = 0 WHERE InvoiceId = @ProformaInvId

			END
		
		
		FETCH NEXT FROM @MyCursor
		INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
		
		END
		CLOSE @MyCursor
		DEALLOCATE @MyCursor
		
	END
	
	

	
END


--SELECT * FROM tblInvoice

--SELECT * FROM dbo.tblDeliveryManInfo
--INNER JOIN dbo.tblEmpGeneralInfo ON tblEmpGeneralInfo.EmpInfoId = tblDeliveryManInfo.EmpInfoId
--WHERE DeliveryManId = 4



--SELECT * FROM dbo.tblCustMaster WHERE CustomerCode
```

---

## `sp_ProformaPosting`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster, CSTL_ERP_ACCDB..tblFinancialYear, CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew, tblCentralStore, tblCompanyUnit, tblDCStore, tblDeb, tblInvoice, tblInvoiceDetail
**Tables written:** INSERT → CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail, CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster; UPDATE → tblInvoice

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** GL-posting procedure restricted to `CompanyId = 16`: for each qualifying row (cursor-driven), it builds a voucher code and financial-year id, inserts a header row into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster` (`VoucherType='Journal'`, `Particular='ProformaInvoice'`) and a matching debit/credit line pair into `tblDebitCreditVoucherDetail`, using GL account ids looked up from `tblSalesIntegrationMapingNew` for that `Particular`; it then sets `tblInvoice.IsPosting` to 1 if a voucher now exists for the record, else 0.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1657` — `return aCommonInternalDal.UpdateAction("sp_ProformaPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProformaPosting] 

	@InvoiceId INT
	
AS
BEGIN

	DECLARE @TempCompanyId INT = 0
	
	SELECT @TempCompanyId = CompanyId FROM tblInvoice AS INV 
	LEFT JOIN tblCompanyUnit AS CU ON INV.ComUnitId = CU.ComUnitId
	WHERE InvoiceId = @InvoiceId


	IF(@TempCompanyId = 16)
	BEGIN


		DECLARE @ProformaInvId INT
		DECLARE @InvDate DATETIME
		DECLARE @CompanyId INT
		DECLARE @CostPrice NVARCHAR(500)
		DECLARE @InvoiceNo NVARCHAR(500)
		DECLARE @VoucherMasterId INT      
		--------------------------------------------------------
		DECLARE @MyCursor CURSOR
		SET @MyCursor = CURSOR FAST_FORWARD
		FOR
		---------------
		
		SELECT INV.InvoiceId,INV.InvoiceDate,UNT.CompanyId,SUM(((INVD.UnitPrice*INVD.TotalQuantity) + (INVD.UnitVatAmount*INVD.TotalQuantity))) AS TotalPrice,
		INV.InvoiceNo FROM tblInvoice AS INV
		LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
		INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = INVD.DCStoreId
		INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
		INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
		WHERE INV.InvoiceId = @InvoiceId
		GROUP BY INV.InvoiceId,UNT.CompanyId,INV.InvoiceDate,INV.InvoiceNo

		----------
		OPEN @MyCursor
		FETCH NEXT FROM @MyCursor
		INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
		

		-- Master Variable Info
		-------------------------------------

		DECLARE @VoucherType NVARCHAR(MAX) ='Journal'
		DECLARE @VoucherCode NVARCHAR(MAX)

		DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@InvDate)), 3, 3)   
		DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) END)
		DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@InvDate)) END)
		SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster 
		WHERE @InvDate = @InvDate
		
			IF(@VoucherType='DebitVoucherBank')
			BEGIN
				SET @VoucherCode='DVB-'+@VoucherCode	
			END
			IF(@VoucherType='DebitVoucherCash')
			BEGIN
				SET @VoucherCode='DVC-'+@VoucherCode
			END
			IF(@VoucherType='CreditVoucherBank')
			BEGIN
				SET @VoucherCode='CVB-'+@VoucherCode	
			END
			IF(@VoucherType='CreditVoucherCash')
			BEGIN
				SET @VoucherCode='CVC-'+@VoucherCode
			END
			IF(@VoucherType='FundTransfer')
			BEGIN
				SET @VoucherCode='FUT-'+@VoucherCode
			END

			IF(@VoucherType='Journal')
			BEGIN
				SET @VoucherCode='JVP-'+@VoucherCode
			END

		

		--DECLARE @ProCount INT = 0
		
		--SELECT @ProCount = COUNT(VoucherMasterId)+1  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster WHERE MONTH(PostingDate) = MONTH(GETDATE())  AND YEAR(PostingDate) = YEAR(GETDATE())

		--DECLARE @length INT = 0
		--SET @length = LEN(CONVERT(NVARCHAR(MAX),@ProCount))

		--IF(@length = 1)
		--BEGIN
		--   SET @VoucherCode = 'JVP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '000000' + (CONVERT(NVARCHAR(MAX),@ProCount))
		--END
		--ELSE IF (@length = 2)
		--BEGIN
		--	SET @VoucherCode = 'JVP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '00000' + (CONVERT(NVARCHAR(MAX),@ProCount))
		--END
		--ELSE IF (@length = 3)
		--BEGIN
		--	SET @VoucherCode = 'JVP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '0000' + (CONVERT(NVARCHAR(MAX),@ProCount))
		--END
		--ELSE IF (@length = 4)
		--BEGIN
		--	SET @VoucherCode = 'JVP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '000' + (CONVERT(NVARCHAR(MAX),@ProCount))
		--END
		--ELSE IF (@length = 5)
		--BEGIN
		--	SET @VoucherCode = 'JVP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '00' + (CONVERT(NVARCHAR(MAX),@ProCount))
		--END
		--ELSE
		--BEGIN
		--   SET @VoucherCode ='JVP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + (CONVERT(NVARCHAR(MAX),@ProCount))
		--END

		--DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(CURRENT_TIMESTAMP)), 3, 3)   
		--DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(CURRENT_TIMESTAMP))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(CURRENT_TIMESTAMP)) ELSE CONVERT(NVARCHAR(MAX),MONTH(CURRENT_TIMESTAMP)) END)
		--DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(CURRENT_TIMESTAMP))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(CURRENT_TIMESTAMP)) ELSE CONVERT(NVARCHAR(MAX),DAY(CURRENT_TIMESTAMP)) END)
 --		  SELECT @VoucherCode = 'JVP-' +(@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster 
		--WHERE CURRENT_TIMESTAMP = CURRENT_TIMESTAMP

		

		DECLARE @FinYearId NVARCHAR(MAX) = ''

		SELECT @FinYearId = FinancialYearId FROM CSTL_ERP_ACCDB..tblFinancialYear 
		WHERE CompanyId = @CompanyId AND Status IN ('Active') AND @InvDate BETWEEN StartDate AND EndDate

		----------------------------------


		--SELECT * FROM CSTL_ERP_ACCDB..tblFinancialYear 


		-- Master Information Posting

		INSERT	INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster
		(
		    VoucherCode,
		    CompanyInfoId,
		    FinancialYearId,
		    PostingDate,
		    VoucherCategoryID,
		    CurrencyId,
		    CurrencyCovRate,
		    Narration,
		    TotalAmount,
		    TotalAmountBDT,
		    EntryBy,
		    EntryDate,
		    VoucherType, 
		    JournalType,
		    ProformaId,
			IsCostCenter,
			IsInvoice,
			isManualVCRNo,
			IsExportInvoice,
			IsImportInvoice

		)
		VALUES
		(   
		    @VoucherCode,       -- VoucherCode - nvarchar(max)
		    @CompanyId,         -- CompanyInfoId - int
		    @FinYearId,         -- FinancialYearId - int
		    @InvDate, -- PostingDate - datetime
		    2,         -- VoucherCategoryID - int
		    1,         -- CurrencyId - int
		    1.00,      -- CurrencyCovRate - decimal(18, 2)
		    'Proforma Invoice, Inventory Valuation - ( Invoice No:' + @InvoiceNo + ' , Invoice Date:' + CONVERT(NVARCHAR(50), @InvDate, 101) + ', Stock Value:' + CONVERT(NVARCHAR(50), @CostPrice) + ')',       -- Narration - nvarchar(max)
		    0.00,      -- TotalAmount - decimal(18, 2)
		    @CostPrice,      -- TotalAmountBDT - decimal(18, 2)
		    'Auto Process',       -- EntryBy - nvarchar(max)
		    CURRENT_TIMESTAMP, -- EntryDate - datetime	  
		    'Journal',       -- VoucherType - nvarchar(50)
		    'Adjustable',       -- JournalType - nvarchar(50)
		    @ProformaInvId,      -- ProformaId -int
			0,0,0,0,0
		)


		-- Detail Information Posting

		
		SET @VoucherMasterId = SCOPE_IDENTITY()

		DECLARE	@DrGl INT 
		SELECT @DrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProformaInvoice') AND BalanceTyp IN ('Dr')

		-- Debit

		INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
		(
		    VoucherMasterId,
		    AccountId,
		    DebitorCredit,
		    DebitAmount,
		    CreditAmount,
		    DebitAmountBDT,
		    CreditAmountBDT
		)
		VALUES
		(   @VoucherMasterId,    -- VoucherMasterId - int
		    @DrGl,    -- AccountId - int
		    'Debit',  -- DebitorCredit - nvarchar(50)
		    0.00, -- DebitAmount - decimal(18, 2)
		    0.00, -- CreditAmount - decimal(18, 2)
		    @CostPrice, -- DebitAmountBDT - decimal(18, 2)
		    0.00 -- CreditAmountBDT - decimal(18, 2)

		)

		--- Credit

		DECLARE	@CrGl INT 
		SELECT @CrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProformaInvoice') AND BalanceTyp IN ('Cr')
		
		INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
		(
		    VoucherMasterId,
		    AccountId,
		    DebitorCredit,
		    DebitAmount,
		    CreditAmount,
		    DebitAmountBDT,
		    CreditAmountBDT
		)
		VALUES
		(   @VoucherMasterId,    -- VoucherMasterId - int
		    @CrGl,    -- AccountId - int
		    'Credit',  -- DebitorCredit - nvarchar(50)
		    0.00, -- DebitAmount - decimal(18, 2)
		    0.00, -- CreditAmount - decimal(18, 2)
		    0.00, -- DebitAmountBDT - decimal(18, 2)
		    @CostPrice -- CreditAmountBDT - decimal(18, 2)
		)


		DECLARE @Count INT = 0 

		SELECT @Count = COUNT(ProformaId) FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster WHERE ProformaId = @ProformaInvId

		IF (@Count > 0)
			BEGIN
			   
			   UPDATE tblInvoice SET IsPosting = 1 WHERE InvoiceId = @ProformaInvId

			END
		ELSE
			BEGIN

			UPDATE tblInvoice SET IsPosting = 0 WHERE InvoiceId = @ProformaInvId

			END
		
		
		FETCH NEXT FROM @MyCursor
		INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
		
		END
		CLOSE @MyCursor
		DEALLOCATE @MyCursor


	END

	
	

	
END



--SELECT * FROM tblDeb
```

---

## `sp_ProformaReturnPosting`

**Parameters:**
- `@InvoiceId` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster, CSTL_ERP_ACCDB..tblFinancialYear, CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew, ZAS_ACCDB..tblDebitCreditVoucherMaster, tblCentralStore, tblCompanyUnit, tblDCStore, tblInvoice, tblInvoiceDetail
**Tables written:** INSERT → CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail, CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster; UPDATE → tblInvoice

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** GL-posting procedure restricted to `CompanyId = 16`: for each qualifying row (cursor-driven), it builds a voucher code and financial-year id, inserts a header row into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster` (`VoucherType='Journal'`, `Particular='ProformaInvoice'`) and a matching debit/credit line pair into `tblDebitCreditVoucherDetail`, using GL account ids looked up from `tblSalesIntegrationMapingNew` for that `Particular`; it then sets `tblInvoice.IsPosting` to 1 if a voucher now exists for the record, else 0.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs:94` — `return aCommonInternalDal.UpdateAction("sp_ProformaReturnPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProformaReturnPosting] 

	@InvoiceId NVARCHAR(MAX)
	
AS
BEGIN
	
	DECLARE @ProformaInvId INT
	DECLARE @InvDate DATETIME
	DECLARE @CompanyId INT
	DECLARE @CostPrice NVARCHAR(500)
	DECLARE @InvoiceNo NVARCHAR(500)
	DECLARE @VoucherMasterId INT         
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	
	SELECT INV.InvoiceId,INV.InvoiceDate,UNT.CompanyId,SUM(((INVD.UnitPrice*INVD.TotalQuantity) + (INVD.UnitVatAmount*INVD.TotalQuantity))) AS TotalPrice,
	INV.InvoiceNo FROM tblInvoice AS INV
	LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
	INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = INVD.DCStoreId
	INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
	INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
	WHERE INV.InvoiceNo = @InvoiceId
	GROUP BY INV.InvoiceId,UNT.CompanyId,INV.InvoiceDate,INV.InvoiceNo
		

	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	

	-- Master Variable Info
    -------------------------------------

	DECLARE @VoucherType NVARCHAR(MAX) ='Journal'
	DECLARE @VoucherCode NVARCHAR(MAX)

	DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@InvDate)), 3, 3)   
	DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) END)
	DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@InvDate)) END)
    SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster 
	WHERE @InvDate = @InvDate
	
		IF(@VoucherType='DebitVoucherBank')
		BEGIN
			SET @VoucherCode='DVB-'+@VoucherCode	
		END
		IF(@VoucherType='DebitVoucherCash')
		BEGIN
			SET @VoucherCode='DVC-'+@VoucherCode
		END
		IF(@VoucherType='CreditVoucherBank')
		BEGIN
			SET @VoucherCode='CVB-'+@VoucherCode	
		END
		IF(@VoucherType='CreditVoucherCash')
		BEGIN
			SET @VoucherCode='CVC-'+@VoucherCode
		END
		IF(@VoucherType='FundTransfer')
		BEGIN
			SET @VoucherCode='FUT-'+@VoucherCode
		END

		IF(@VoucherType='Journal')
		BEGIN
			SET @VoucherCode='JVP-'+@VoucherCode
		END

	DECLARE @FinYearId NVARCHAR(MAX) = ''

	SELECT @FinYearId = FinancialYearId FROM CSTL_ERP_ACCDB..tblFinancialYear 
	WHERE CompanyId = @CompanyId AND Status IN ('Active') AND @InvDate BETWEEN StartDate AND EndDate

	----------------------------------


	-- Master Information Posting

	INSERT	INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster
	(
	    VoucherCode,
	    CompanyInfoId,
	    FinancialYearId,
	    PostingDate,
	    VoucherCategoryID,
	    CurrencyId,
	    CurrencyCovRate,
	    Narration,
	    TotalAmount,
	    TotalAmountBDT,
	    EntryBy,
	    EntryDate,
	    VoucherType, 
	    JournalType,
	    ProformaId,
		IsCostCenter,
		IsInvoice,
		isManualVCRNo,
		IsExportInvoice,
		IsImportInvoice

	)
	VALUES
	(   
	    @VoucherCode,       -- VoucherCode - nvarchar(max)
	    @CompanyId,         -- CompanyInfoId - int
	    @FinYearId,         -- FinancialYearId - int
	    CONVERT(NVARCHAR(50), GETDATE(), 101), -- PostingDate - datetime
	    2,         -- VoucherCategoryID - int
	    1,         -- CurrencyId - int
	    1.00,      -- CurrencyCovRate - decimal(18, 2)
	    'Proforma Invoice Return ( Invoice No:' + @InvoiceNo + ' , Invoice Date:' + CONVERT(NVARCHAR(50), @InvDate, 101) + ', Stock Value:' + CONVERT(NVARCHAR(50), @CostPrice) + ')',       -- Narration - nvarchar(max)
	    0.00,      -- TotalAmount - decimal(18, 2)
	    @CostPrice,      -- TotalAmountBDT - decimal(18, 2)
	    'Auto Process',       -- EntryBy - nvarchar(max)
	    CURRENT_TIMESTAMP, -- EntryDate - datetime	  
	    'Journal',       -- VoucherType - nvarchar(50)
	    'Adjustable',       -- JournalType - nvarchar(50)
	    @ProformaInvId,      -- ProformaId -int
		0,0,0,0,0
	)


	-- Detail Information Posting

	SET @VoucherMasterId = SCOPE_IDENTITY()

	DECLARE	@DrGl INT 
	SELECT @DrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProformaInvoice') AND BalanceType IN ('Dr')

	-- Credit

	INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
	(
	    VoucherMasterId,
	    AccountId,
	    DebitorCredit,
	    DebitAmount,
	    CreditAmount,
	    DebitAmountBDT,
	    CreditAmountBDT
	)
	VALUES
	(   @VoucherMasterId,    -- VoucherMasterId - int
	    @DrGl,    -- AccountId - int
	    'Credit',  -- DebitorCredit - nvarchar(50)
	    0.00, -- DebitAmount - decimal(18, 2)
	    0.00, -- CreditAmount - decimal(18, 2)
	    @CostPrice, -- DebitAmountBDT - decimal(18, 2)
	    0.00 -- CreditAmountBDT - decimal(18, 2)

	)

	--- Debit

	DECLARE	@CrGl INT 
	SELECT @CrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('ProformaInvoice') AND BalanceType IN ('Cr')


	
	INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
	(
	    VoucherMasterId,
	    AccountId,
	    DebitorCredit,
	    DebitAmount,
	    CreditAmount,
	    DebitAmountBDT,
	    CreditAmountBDT
	)
	VALUES
	(   @VoucherMasterId,    -- VoucherMasterId - int
	    @CrGl,    -- AccountId - int
	    'Debit',  -- DebitorCredit - nvarchar(50)
	    0.00, -- DebitAmount - decimal(18, 2)
	    0.00, -- CreditAmount - decimal(18, 2)
	    0.00, -- DebitAmountBDT - decimal(18, 2)
	    @CostPrice -- CreditAmountBDT - decimal(18, 2)
	)


	--DECLARE @Count INT = 0 

	--SELECT @Count = COUNT(ProformaId) FROM ZAS_ACCDB..tblDebitCreditVoucherMaster WHERE ProformaId = @ProformaInvId

	--IF (@Count > 0)
	--	BEGIN
		   
	--	   UPDATE tblInvoice SET IsPosting = 1 WHERE InvoiceId = @ProformaInvId

	--	END
	--ELSE
	--	BEGIN

	--	UPDATE tblInvoice SET IsPosting = 0 WHERE InvoiceId = @ProformaInvId

	--	END
	
	
	FETCH NEXT FROM @MyCursor
	INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor

	
END
```

---

## `sp_Report_TargetvsAcheivement`

**Parameters:**
- `@DelivaryInvoiceNo` — nvarchar(500), input

**Tables read (FROM/JOIN):** View_MioWiseInvoiceSummary, View_SalesMonthYearWise, tblArea, tblCompanyInfo, tblCompanyUnit, tblDcWiseAreaInfo, tblEmpGeneralInfo, tblMIATargetProductWise, tblMIOInfo, tblProduct, tblTerritory, tblproduct
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_MioWiseInvoiceSummary, View_SalesMonthYearWise, tblArea, tblCompanyInfo, tblCompanyUnit, tblDcWiseAreaInfo, tblEmpGeneralInfo, tblMIATargetProductWise, tblMIOInfo, tblProduct, tblTerritory, tblproduct.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SLS.CompanyId,CompanyCode,CompanyName,SLS.ComUnitId,ComUnitCode ,ComUnitName,SLS.MiaId,MIO,AreaName, TerritoryName,ProductCode,ProductName,PackSize,0 AS TargetQty, SUM(ISNULL(SLS.DeliveryTotalQuantity,0)) AS Qty FROM View_MioWiseInvoiceSummary AS SLS LEFT JOIN tblMIATargetProductWise AS TGRT ON SLS.MiaId = TGRT.MiaId AND SLS.ProductId = TG`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:	<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Report_TargetvsAcheivement]

	--@DelivaryInvoiceNo nvarchar(500),
 --   @userid nvarchar(500)

AS
BEGIN

	--SELECT GETDATE()


	DECLARE @FromDate DATETIME = '2024-03-01'
	DECLARE @ToDate DATETIME = '2024-04-30'


	--CASE WHEN SUM(ISNULL(TGRT.TargetQty,0)) > 0 THEN (SUM(ISNULL(TotalSalesQuantity,0))* 100) / SUM(ISNULL(TGRT.TargetQty,0)) ELSE 0 END AS Achivment


	SELECT CompanyId,CompanyCode,CompanyName,ComUnitId,ComUnitCode ,ComUnitName,MIO,AreaName,MiaId,MIO,
	TerritoryName,ProductCode,ProductName,PackSize,SUM(ISNULL(SLVST.TargetQty,0)) TargetQty, SUM(ISNULL(SLVST.Qty,0)) AS Qty,

	CASE WHEN SUM(ISNULL(SLVST.TargetQty,0)) > 0 THEN (SUM(ISNULL(SLVST.Qty,0))* 100) / SUM(ISNULL(SLVST.TargetQty,0)) ELSE 0 END AS Achivment

	FROM (SELECT CI.CompanyId,CI.CompanyCode,CI.CompanyName,ISNULL(SL.ComUnitId,UNT.ComUnitId) ComUnitId,ISNULL(SL.ComUnitCode,UNT.ComUnitCode) ComUnitCode ,ISNULL(SL.ComUnitName,UNT.ComUnitName) ComUnitName,TGRT.MiaId,EmpName AS MIO,ARA.AreaName,
	TTR.TerritoryName,PD.ProductCode,PD.ProductName,PD.PackSize, SUM(ISNULL(TargetQty,0)) AS TargetQty, SUM(ISNULL(TotalSalesQuantity,0)) AS Qty 
	 FROM tblMIATargetProductWise AS TGRT 
	LEFT JOIN tblProduct AS PD ON TGRT.ProductId = PD.ProductId 
	LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=TGRT.MiaId 
	LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId 
	LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
	LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId  
	LEFT JOIN tblCompanyInfo AS CI ON TGRT.CompanyId = CI.CompanyId
	LEFT JOIN (SELECT ComUnitId,AreaId,CompanyId FROM tblDcWiseAreaInfo AS DC 
	LEFT JOIN tblCompanyUnit AS UNT ON DC.DCId = UNT.ComUnitId
	WHERE IsActive = 1) AS DCA ON ARA.AreaId = DCA.AreaId AND TGRT.CompanyId = DCA.CompanyId AND ARA.IsActive = 1
	LEFT JOIN tblCompanyUnit AS UNT ON DCA.ComUnitId = UNT.ComUnitId
	
	-- Sales
	
	INNER JOIN (SELECT ComUnitId,ComUnitCode,ComUnitName,MNS.ProductId,PD.ProductCode,MiaId,PD.ProductName,PD.PackSize,MonthName,Year,SUM(Qty) AS TotalSalesQuantity,SUM(Value) TotalSalesValue FROM View_SalesMonthYearWise AS MNS 
	LEFT JOIN tblproduct AS PD ON MNS.ProductId = PD.ProductId
	--WHERE MiaId = 418 AND MonthName IN ('February','March') AND Year = '2024'
	GROUP BY ComUnitId,ComUnitCode,ComUnitName,MNS.ProductId,PD.ProductCode,MiaId,PD.ProductName,PD.PackSize,MonthName,Year
	) AS SL ON TGRT.ProductId = SL.ProductId AND TGRT.MiaId = SL.MiaId AND TGRT.Period = SL.MonthName AND TGRT.Year = SL.Year
	
	WHERE TGRT.MiaId = 469 AND Period IN ('April','March') AND TGRT.Year = '2024'
	
	GROUP BY CI.CompanyId,CI.CompanyCode,CI.CompanyName,SL.ComUnitId,UNT.ComUnitId,SL.ComUnitCode,UNT.ComUnitCode,SL.ComUnitName,UNT.ComUnitName,TGRT.MiaId,EmpName,ARA.AreaName,
	TTR.TerritoryName,PD.ProductCode,PD.ProductName,PD.PackSize
	
	UNION ALL
	
	SELECT SLS.CompanyId,CompanyCode,CompanyName,SLS.ComUnitId,ComUnitCode ,ComUnitName,SLS.MiaId,MIO,AreaName,
	TerritoryName,ProductCode,ProductName,PackSize,0 AS TargetQty, SUM(ISNULL(SLS.DeliveryTotalQuantity,0)) AS Qty FROM View_MioWiseInvoiceSummary AS SLS
	LEFT JOIN tblMIATargetProductWise AS TGRT ON SLS.MiaId = TGRT.MiaId AND SLS.ProductId = TGRT.ProductId 
	AND SLS.CompanyId = TGRT.CompanyId AND SLS.MONTH = TGRT.Period AND SLS.Year = TGRT.Year
	WHERE TGRT.ProductId IS NULL AND SLS.MiaId = 469 AND MONTH IN ('April','March') AND SLS.Year = '2024' 
	GROUP BY SLS.CompanyId,CompanyCode,CompanyName,SLS.ComUnitId,ComUnitCode ,ComUnitName,SLS.MiaId,MIO,AreaName,TerritoryName,ProductCode,ProductName,PackSize) AS SLVST

	WHERE SLVST.MiaId = 469

	GROUP BY  CompanyId,CompanyCode,CompanyName,ComUnitId,ComUnitCode ,ComUnitName,MIO,AreaName,MiaId,MIO,TerritoryName,ProductCode,ProductName,PackSize

	


END






	--SELECT CI.CompanyCode,CI.CompanyName,ISNULL(SL.ComUnitCode,UNT.ComUnitCode) ComUnitCode ,ISNULL(SL.ComUnitName,UNT.ComUnitName) ComUnitName,EmpName AS MIO,ARA.AreaName,
	--TTR.TerritoryName,PD.ProductCode,PD.ProductName,PD.PackSize, SUM(ISNULL(TargetQty,0)) AS TargetQty, SUM(ISNULL(TotalSalesQuantity,0)) AS Qty 
	-- FROM tblMIATargetProductWise AS TGRT 
	--LEFT JOIN tblProduct AS PD ON TGRT.ProductId = PD.ProductId 
	--LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=TGRT.MiaId 
	--LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId 
	--LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
	--LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId  
	--LEFT JOIN tblCompanyInfo AS CI ON TGRT.CompanyId = CI.CompanyId
	--LEFT JOIN (SELECT ComUnitId,AreaId,CompanyId FROM tblDcWiseAreaInfo AS DC 
	--LEFT JOIN tblCompanyUnit AS UNT ON DC.DCId = UNT.ComUnitId
	--WHERE IsActive = 1) AS DCA ON ARA.AreaId = DCA.AreaId AND TGRT.CompanyId = DCA.CompanyId AND ARA.IsActive = 1
	--LEFT JOIN tblCompanyUnit AS UNT ON DCA.ComUnitId = UNT.ComUnitId
	
	---- Sales
	
	--INNER JOIN (SELECT ComUnitId,ComUnitCode,ComUnitName,MNS.ProductId,PD.ProductCode,MiaId,PD.ProductName,PD.PackSize,MonthName,Year,SUM(Qty) AS TotalSalesQuantity,SUM(Value) TotalSalesValue FROM View_SalesMonthYearWise AS MNS 
	--LEFT JOIN tblproduct AS PD ON MNS.ProductId = PD.ProductId
	----WHERE MiaId = 418 AND MonthName IN ('February','March') AND Year = '2024'
	--GROUP BY ComUnitId,ComUnitCode,ComUnitName,MNS.ProductId,PD.ProductCode,MiaId,PD.ProductName,PD.PackSize,MonthName,Year
	--) AS SL ON TGRT.ProductId = SL.ProductId AND TGRT.MiaId = SL.MiaId AND TGRT.Period = SL.MonthName AND TGRT.Year = SL.Year
	
	--WHERE TGRT.MiaId = 469 AND Period IN ('April','March') AND TGRT.Year = '2024'
	
	--GROUP BY CI.CompanyCode,CI.CompanyName,SL.ComUnitCode,UNT.ComUnitCode,SL.ComUnitName,UNT.ComUnitName,EmpName,ARA.AreaName,
	--TTR.TerritoryName,PD.ProductCode,PD.ProductName,PD.PackSize
	
	--UNION ALL
	
	--SELECT CompanyCode,CompanyName,ComUnitCode ,ComUnitName,MIO,AreaName,
	--TerritoryName,ProductCode,ProductName,PackSize,0 AS TargetQty, SUM(ISNULL(SLS.DeliveryTotalQuantity,0)) AS Qty FROM View_MioWiseInvoiceSummary AS SLS
	--LEFT JOIN tblMIATargetProductWise AS TGRT ON SLS.MiaId = TGRT.MiaId AND SLS.ProductId = TGRT.ProductId 
	--AND SLS.CompanyId = SLS.CompanyId AND SLS.MONTH = TGRT.Period AND SLS.Year = TGRT.Year
	--WHERE TGRT.ProductId IS NULL AND SLS.MiaId = 469 AND MONTH IN ('April','March') AND SLS.Year = '2024' 
	--GROUP BY CompanyCode,CompanyName,ComUnitCode ,ComUnitName,MIO,AreaName,TerritoryName,ProductCode,ProductName,PackSize
```

---

## `sp_RPT_CusVisitListByEmp`

**Parameters:**
- `@EmpInfoId` — int, input = `null`
- `@DcrDate` — nvarchar(max), input

**Tables read (FROM/JOIN):** tblCustMaster, tbl_DCRCustomerInfo, tbl_TourPlanInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblCustMaster, tbl_DCRCustomerInfo, tbl_TourPlanInfo, filtered by @EmpInfoId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select distinct dcr.CustomerId from tbl_DCRCustomerInfo dcr with (nolock) where dcr.EmpInfoId=@EmpInfoId and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_RPT_CusVisitListByEmp]
	-- Add the parameters for the stored procedure here
	  
	  @EmpInfoId  int =null ,

	  @DcrDate nvarchar(max)

	   
AS
BEGIN
select mas.CustomerCode, mas.CustomerName, dcr.StreetAddress, FORMAT(dcr.DcrDate, 'dd-MMM-yyyy') DcrDate, FORMAT(dcr.EntryDate, 'hh:mm:ss tt') VisitTime, 'yes' Status, dcr.Remarks,  'http://maps.google.com/?q='+dcr.Latitude+','+dcr.Longitude MapShow from tbl_DCRCustomerInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerId


where dcr.EmpInfoId=@EmpInfoId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)


union all

select mas.CustomerCode, mas.CustomerName,'' StreetAddress, '' DcrDate, '' VisitTime, 'no' Status, '' Remarks,  '' MapShow from tbl_TourPlanInfo dcr with (nolock)
inner join tblCustMaster mas  with (nolock) on mas.CustomerMasterId=dcr.CustomerMasterId
 

where dcr.EmpInfoId=@EmpInfoId  and CONVERT(Date,dcr.TourPlanDate)=CONVERT(Date, @DcrDate) --and (dcr.IsDcrDone is null  or dcr.IsDcrDone=0)
 and dcr.CustomerMasterId not in (select distinct dcr.CustomerId  from tbl_DCRCustomerInfo dcr with (nolock)
 
 

where dcr.EmpInfoId=@EmpInfoId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate))


 END
```

---

## `sp_Rpt_DCRInfo_ById`

**Parameters:**
- `@id` — INT, input

**Tables read (FROM/JOIN):** tblDesignation, tblDoctorChemberDetail, tblDoctorMaster, tblEmpGeneralInfo, tblProduct, tblUser, tbl_DCRInfo, tbl_DcrDetails, tbl_DcrVisitedWithDetails, tbl_DoctorVisitType, tbl_UserRoleInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblDesignation, tblDoctorChemberDetail, tblDoctorMaster, tblEmpGeneralInfo, tblProduct, tblUser, tbl_DCRInfo, tbl_DcrDetails, tbl_DcrVisitedWithDetails, tbl_DoctorVisitType, tbl_UserRoleInfo, filtered by @id.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT dtl.EmpMasterCode+' : '+dtl.EmpName empInfo, dgs.DesigName, mas.Remarks, cham.Name ChamberName, docMas.DoctorCode+' : '+docMas.DoctorName DoctorName, tt.VisitTypeName TourTypeName, CASE WHEN prodtl.Type='Product' THEN 'Commercial' ELSE prodtl.Type END Type, pro.ProductCode+' : '+pro.ProductName ProductName, emp.EmpMasterCode+' : '+emp.Emp`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
CREATE PROCEDURE [dbo].[sp_Rpt_DCRInfo_ById]
	-- Add the parameters for the stored procedure here
    @id INT
AS
    BEGIN

      SELECT dtl.EmpMasterCode+' : '+dtl.EmpName empInfo,  dgs.DesigName,  mas.Remarks, cham.Name ChamberName, docMas.DoctorCode+' : '+docMas.DoctorName DoctorName, tt.VisitTypeName TourTypeName, CASE WHEN prodtl.Type='Product' THEN 'Commercial' ELSE  prodtl.Type END Type, pro.ProductCode+' : '+pro.ProductName ProductName, emp.EmpMasterCode+' : '+emp.EmpName EmpName, convert(varchar,mas.DcrDate, 0)  DcrDate,  * FROM dbo.tbl_DCRInfo mas WITH (NOLOCK)
LEFT JOIN tbl_DcrDetails prodtl ON  mas.DcrId=prodtl.DcrId
left JOIN dbo.tbl_DoctorVisitType tt  WITH (NOLOCK)  ON tt.DocVisitTypeId = mas.TourTypeId
LEFT JOIN dbo.tblDoctorChemberDetail cham ON  cham.ChemberId=mas.ChemberId
LEFT JOIN dbo.tblProduct pro ON pro.ProductId = prodtl.ProductId
LEFT JOIN dbo.tblDoctorMaster docMas ON docMas.DoctorId = mas.DoctorId
LEFT JOIN dbo.tbl_DcrVisitedWithDetails vis ON  vis.DcrId=mas.DcrId
LEFT JOIN dbo.tblEmpGeneralInfo emp ON  vis.EmpInfoId=emp.EmpInfoId


left JOIN dbo.tblUser us ON us.UserId = mas.Entryby
left JOIN dbo.tblEmpGeneralInfo dtl ON dtl.EmpInfoId = us.EmpInfoId
left JOIN dbo.tbl_UserRoleInfo usr ON usr.UserRoleID = us.UserRoleID
left JOIN dbo.tblDesignation dgs ON dgs.DesignationId = dtl.DesignationId

 where mas.DcrId = @id
    END
```

---

## `sp_RPT_DocotVisitListByEmp`

**Parameters:**
- `@EmpInfoId` — int, input = `null`
- `@DcrDate` — nvarchar(max), input

**Tables read (FROM/JOIN):** tblDoctorMaster, tblUser, tbl_DCRInfo, tbl_DoctorTourPlanDetail
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblDoctorMaster, tblUser, tbl_DCRInfo, tbl_DoctorTourPlanDetail, filtered by @EmpInfoId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select dcr.DoctorId from tbl_DCRInfo dcr with (nolock) inner join tblUser us with (nolock) on us.UserId=dcr.EntryBy where us.EmpInfoId=@EmpInfoId and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_RPT_DocotVisitListByEmp]
	-- Add the parameters for the stored procedure here
	  
	  @EmpInfoId  int =null ,

	  @DcrDate nvarchar(max)

	   
AS
BEGIN
select mas.DoctorCode, mas.DoctorName, dcr.StreetAddress, FORMAT(dcr.DcrDate, 'dd-MMM-yyyy') DcrDate, FORMAT(dcr.EntryDate, 'hh:mm:ss tt') VisitTime, 'yes' Status, dcr.Remarks,  'http://maps.google.com/?q='+dcr.Latitude  MapShow from tbl_DCRInfo dcr with (nolock)
inner join tblDoctorMaster mas  with (nolock) on mas.DoctorId=dcr.DoctorId
inner join tblUser us  with (nolock) on us.UserId=dcr.EntryBy

where us.EmpInfoId=@EmpInfoId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate)


union all

select mas.DoctorCode, mas.DoctorName,'' StreetAddress, '' DcrDate, '' VisitTime, 'no' Status, '' Remarks,  '' MapShow from tbl_DoctorTourPlanDetail dcr with (nolock)
inner join tblDoctorMaster mas  with (nolock) on mas.DoctorId=dcr.DoctorId
 

where dcr.EmpInfoId=@EmpInfoId  and CONVERT(Date,dcr.TourPlanDate)=CONVERT(Date, @DcrDate) --and (dcr.IsDcrDone is null  or dcr.IsDcrDone=0)
 and dcr.DoctorId not in (select dcr.DoctorId  from tbl_DCRInfo dcr with (nolock)
 
inner join tblUser us  with (nolock) on us.UserId=dcr.EntryBy

where us.EmpInfoId=@EmpInfoId  and CONVERT(Date,dcr.DcrDate)=CONVERT(Date, @DcrDate))


 END
```

---

## `sp_Run_Existing_Inventory_Voucher_Posting`

**Parameters:**
- `@CompanyId` — INT, input = `16`
- `@FromDate` — DATE, input = `'2026-07-01'`
- `@ToDate` — DATE, input = `'2026-07-31'`
- `@PostingType` — VARCHAR(20), input = `'ALL'`

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, cur_del, cur_return, cur_sample, tblCompanyUnit, tblCustomerReturnMaster, tblInvoice, tblInvoiceDetail, tblSampleIssue
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining CSTL_ACCDB..tblDebitCreditVoucherMaster, cur_del, cur_return, cur_sample, tblCompanyUnit, tblCustomerReturnMaster, tblInvoice, tblInvoiceDetail, tblSampleIssue, filtered by @CompanyId, @FromDate, @ToDate, @SampleOrderId, @CustReturnId, @InvoiceId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT @CompanyId AS CompanyId, @FromDate AS FromDate, @ToDate AS ToDate, @SampleCount AS Existing_SampleSP_Executed, @ReturnCount AS Existing_ReturnSP_Executed, @DeliveryCount AS Existing_DeliverySP_Executed; END;`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE   PROCEDURE dbo.sp_Run_Existing_Inventory_Voucher_Posting
    @CompanyId INT = 16,
    @FromDate DATE = '2026-07-01',
    @ToDate DATE = '2026-07-31',
    @PostingType VARCHAR(20) = 'ALL' -- 'ALL', 'SAMPLE', 'RETURN', 'DELIVERY'
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SampleCount INT = 0;
    DECLARE @ReturnCount INT = 0;
    DECLARE @DeliveryCount INT = 0;

    -- =========================================================================
    -- ?. ???????? SP ???? ???????? COGS ???????: dbo.sp_Sales_SampleCOGSPosting
    -- =========================================================================
    IF @PostingType IN ('ALL', 'SAMPLE')
    BEGIN
        DECLARE @SampleOrderId INT;
        DECLARE cur_sample CURSOR LOCAL FAST_FORWARD FOR
            SELECT DISTINCT SI.OrderId
            FROM tblSampleIssue SI WITH (NOLOCK)
            INNER JOIN tblCompanyUnit CU WITH (NOLOCK) ON SI.ComUnitId = CU.ComUnitId
            LEFT JOIN CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster V WITH (NOLOCK)
                ON V.TableID = 2015 AND SI.OrderId = V.CommonPK_ID AND V.ActionStatus <> 'Cancelled'
            WHERE CU.CompanyId = @CompanyId
              AND SI.ActionStatus IN ('Accepted', 'Approved')
              AND SI.ApprovedDate >= @FromDate AND SI.ApprovedDate <= @ToDate
              AND ISNULL(V.VoucherMasterId, 0) = 0;

        OPEN cur_sample;
        FETCH NEXT FROM cur_sample INTO @SampleOrderId;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            EXEC dbo.sp_Sales_SampleCOGSPosting @InvoiceId = @SampleOrderId;
            SET @SampleCount += 1;
            FETCH NEXT FROM cur_sample INTO @SampleOrderId;
        END;
        CLOSE cur_sample;
        DEALLOCATE cur_sample;
    END;

    -- =========================================================================
    -- ?. ???????? SP ???? ???? ??????? ???????: dbo.sp_Sales_DeliverySalesReturnCOGSPosting
    -- =========================================================================
    IF @PostingType IN ('ALL', 'RETURN')
    BEGIN
        DECLARE @CustReturnId INT;
        DECLARE cur_return CURSOR LOCAL FAST_FORWARD FOR
            SELECT DISTINCT CRM.CustReturnId
            FROM tblCustomerReturnMaster CRM WITH (NOLOCK)
            INNER JOIN tblInvoice INV WITH (NOLOCK) ON CRM.InvoiceId = INV.InvoiceId
            INNER JOIN tblCompanyUnit CU WITH (NOLOCK) ON INV.ComUnitId = CU.ComUnitId
            LEFT JOIN CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster V WITH (NOLOCK)
                ON V.TableID = 23 AND CRM.CustReturnId = V.CommonPK_ID AND V.ActionStatus <> 'Cancelled'
            WHERE CU.CompanyId = @CompanyId
              AND CRM.ActionStatus IN ('Accepted', 'Approved')
              AND CRM.ApprovedDate >= @FromDate AND CRM.ApprovedDate <= @ToDate
              AND ISNULL(V.VoucherMasterId, 0) = 0;

        OPEN cur_return;
        FETCH NEXT FROM cur_return INTO @CustReturnId;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            EXEC dbo.sp_Sales_DeliverySalesReturnCOGSPosting @ReturnId = @CustReturnId;
            SET @ReturnCount += 1;
            FETCH NEXT FROM cur_return INTO @CustReturnId;
        END;
        CLOSE cur_return;
        DEALLOCATE cur_return;
    END;

    -- =========================================================================
    -- ?. ???????? SP ???? ???????? COGS ???????: dbo.sp_Sales_DeliveryCOGSPosting
    -- =========================================================================
    IF @PostingType IN ('ALL', 'DELIVERY')
    BEGIN
        DECLARE @InvoiceId INT;
        DECLARE cur_del CURSOR LOCAL FAST_FORWARD FOR
            SELECT DISTINCT INV.InvoiceId
            FROM tblInvoice INV WITH (NOLOCK)
            INNER JOIN tblCompanyUnit CU WITH (NOLOCK) ON INV.ComUnitId = CU.ComUnitId
            INNER JOIN tblInvoiceDetail INVD WITH (NOLOCK) ON INV.InvoiceId = INVD.InvoiceId
            LEFT JOIN CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster V WITH (NOLOCK)
                ON V.TableID = 2014 AND ISNULL(V.IsInventoryValuation, 0) = 1 AND INV.InvoiceId = V.CommonPK_ID AND V.ActionStatus <> 'Cancelled'
            WHERE CU.CompanyId = @CompanyId
              AND INVD.DeliveryStatus IN ('Full', 'Partial')
              AND INV.DeliveryInvoiceStatus <> 'Reject'
              AND INV.InvoiceDate >= @FromDate AND INV.InvoiceDate <= @ToDate
              AND ISNULL(V.VoucherMasterId, 0) = 0;

        OPEN cur_del;
        FETCH NEXT FROM cur_del INTO @InvoiceId;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            EXEC dbo.sp_Sales_DeliveryCOGSPosting @InvoiceId = @InvoiceId;
            SET @DeliveryCount += 1;
            FETCH NEXT FROM cur_del INTO @InvoiceId;
        END;
        CLOSE cur_del;
        DEALLOCATE cur_del;
    END;

    -- Execution Summary
    SELECT 
        @CompanyId AS CompanyId,
        @FromDate AS FromDate,
        @ToDate AS ToDate,
        @SampleCount AS Existing_SampleSP_Executed,
        @ReturnCount AS Existing_ReturnSP_Executed,
        @DeliveryCount AS Existing_DeliverySP_Executed;
END;
```

---

## `sp_Sales_CustomerPaymentPosting`

**Parameters:**
- `@InvoiceId` — INT, input
- `@CustPayId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblChartOfAccounts, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, customer, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice, tblPaymentTranscationMaster
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster; UPDATE → INV, Invoice, tblPaymentTranscationMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblChartOfAccounts, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, customer, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice, tblPaymentTranscationMaster, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster; updates INV, Invoice, tblPaymentTranscationMaster.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT InvoiceId, SUM(ISNULL(PaymentAmount, 0)) AS PaymentAmount, SUM(ISNULL(AIT, 0)) AS AIT, SUM(ISNULL(Discount, 0)) AS Discount FROM tblCustPayDetail WHERE InvoiceId = @InvoiceId GROUP BY InvoiceId ) AS AGG ON INV.InvoiceId = AGG.InvoiceId WHERE INV.InvoiceId = @InvoiceId;`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs:538` — `return aCommonInternalDal.UpdateAction("sp_Sales_CustomerPaymentPosting", aSqlParameterList);`
- `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:87` — `return aCommonInternalDal.UpdateAction("sp_Sales_CustomerPaymentPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Sales_CustomerPaymentPosting] 

	@InvoiceId INT,
	@CustPayId INT

AS
BEGIN


		--SELECT * FROM dbo.tblCustPayDetail AS CPD 
		--LEFT JOIN dbo.tblCustomerPay AS CPY ON CPD.CustPayId = CPY.CustPayId
		--WHERE CPD.PaymentAmount > 0 AND ISNULL(CPD.AIT, 0) > 0 AND ISNULL(CPD.Discount, 0) > 0

		-- Reject Previous Not Processd Payment

		UPDATE tblPaymentTranscationMaster SET IsReject = 1
		WHERE CONVERT(DATE,CreateDate) < CONVERT(DATE,GETDATE()) AND IsProcess = 0 AND IsReject = 0


		--SELECT * FROM tblPaymentTranscationMaster  WHERE CONVERT(DATE,CreateDate) < CONVERT(DATE,GETDATE()) AND IsProcess = 0 AND IsReject = 0

		--UPdate Invoice

		UPDATE INV
		SET 
		    PaymentAmount = AGG.PaymentAmount,
		    AIT = AGG.AIT,
		    DiscountOnPayment = AGG.Discount
		FROM tblInvoice AS INV
		INNER JOIN (
		    SELECT 
		        InvoiceId,
		        SUM(ISNULL(PaymentAmount, 0)) AS PaymentAmount,
		        SUM(ISNULL(AIT, 0)) AS AIT,
		        SUM(ISNULL(Discount, 0)) AS Discount
		    FROM tblCustPayDetail
		    WHERE InvoiceId = @InvoiceId
		    GROUP BY InvoiceId
		) AS AGG ON INV.InvoiceId = AGG.InvoiceId
		WHERE INV.InvoiceId = @InvoiceId;


		

		

		-- Journal Posting 

		DECLARE @VoucherMasterId INT
		DECLARE @VoucherCode NVARCHAR(MAX)
		DECLARE @BiusinessUnitId INT
		--DECLARE @SupplierId INT
		--DECLARE @BankAccountId INT
		--DECLARE @CashAccountId INT
		DECLARE @FinancialYearId INT
 --		  DECLARE @DateOfPayment DATETIME
		DECLARE @CurrencyId INT = 1
		DECLARE @Conversionrate DECIMAL(18,4) = 1.00
 --		  DECLARE @Narration NVARCHAR(MAX)
 --		  DECLARE @TotalPayAmountBDT DECIMAL(18,4)
 --		  DECLARE @TotalAmount DECIMAL(18,4)
 --		  DECLARE @VoucherCategory INT
 --		  DECLARE @ChecqueNo  NVARCHAR(MAX)
 --		  DECLARE @ChecqueDate DATETIME
 --		  DECLARE @ReferenceNo NVARCHAR(MAX)
 --		  DECLARE @ReferenceDate DATETIME
		--DECLARE @PurchaseOrderLocalCode2 NVARCHAR(500)
		DECLARE @VoucherType NVARCHAR(200) ='Journal'
		DECLARE @ApprovalDate DATETIME
		DECLARE @ApproveBy NVARCHAR(200)
		DECLARE @TableId INT
		DECLARE @InvoiceNo  NVARCHAR(200)
		DECLARE @MaterialConversionDate DATETIME
		DECLARE @DeliveryNetAmount  DECIMAL(18,4)

		DECLARE @InvoiceId2 INT
		DECLARE @CustomerName NVARCHAR(500)
		DECLARE @BankId INT
		DECLARE @CashId INT

		DECLARE @SubsidiaryInfoId INT
		DECLARE @ReferenceInfoId INT

		-- For Credit Amount

		DECLARE @AIT DECIMAL(18,4) = 0
		DECLARE @Discount DECIMAL(18,4) = 0
		        
		--------------------------------------------------------

		DECLARE @MyCursor2 CURSOR
		SET @MyCursor2 = CURSOR FAST_FORWARD
		FOR
		-----------

		--DECLARE @InvoiceId INT = 546854
	 --   DECLARE @CustPayId INT = 540816

		SELECT DISTINCT CI.CompanyId,FY.FinancialYearId,FORMAT(CPY.PaymentDate, 'dd-MMM-yyyy') UpdateDate,
		CPD.PaymentAmount,ISNULL(CPD.Discount, 0) AS Discount,ISNULL(CPD.AIT, 0) AS AIT,TBL.IntegrationTableId,CPY.CustPayId,
		CMS.CustomerName + ' (' + CMS.CustomerCode + ')' AS CustomerName,InvoiceNo,CPD.BankAccId,CPD.CashAccId,
		ISNULL(SB.SubsidiaryInfoId,0) SubsidiaryInfoId,ISNULL(RF.ReferenceInfoId,0) ReferenceInfoId  FROM tblCustPayDetail AS CPD 
        LEFT JOIN dbo.tblCustomerPay AS CPY ON CPD.CustPayId = CPY.CustPayId
        LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = CPD.InvoiceId
        INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = tblInvoice.ComUnitId
		INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		LEFT JOIN tblCustMaster AS CMS ON tblInvoice.CustomerMasterId = CMS.CustomerMasterId

		-- Subsidary

		LEFT JOIN (SELECT S_Info_Id,SubsidiaryInfoId FROM CSTL_ACCDB..tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1) AS SB ON tblInvoice.CustomerMasterId = SB.S_Info_Id

		-- Reference

		LEFT JOIN CSTL_ACCDB..tblReferenceInfo AS RF ON RF.ReferenceTypeId = 1016 -- Invoice
		AND R_Info_Id = @InvoiceId 

		INNER JOIN (SELECT CompanyId,FinancialYearId,StartDate,EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND CPY.PaymentDate BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblCustomerPay'))) 	
		LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2017 AND  CPY.CustPayId = V.CommonPK_ID
		WHERE 
		--ISNULL(V.VoucherMasterId,0) = 0 AND 
		
		(CPD.PaymentAmount + ISNULL(CPD.Discount, 0) + ISNULL(CPD.AIT, 0)) > 0 
		--AND CI.CompanyId IN (16) -- 2- Critical Care, 16 - Sales & Marketing 
		AND  CPD.InvoiceId = @InvoiceId -- 275680
		AND CPD.CustPayId = @CustPayId -- 274967


		----------

		OPEN @MyCursor2
		FETCH NEXT FROM @MyCursor2                               
		INTO @BiusinessUnitId,@FinancialYearId,@ApprovalDate,@DeliveryNetAmount,@Discount,@AIT,@TableId,@InvoiceId2,
		@CustomerName,@InvoiceNo, @BankId, @CashId, @SubsidiaryInfoId,@ReferenceInfoId
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

		--SELECT * FROM CSTL_ACCDB..tblVoucherCategory

		 --- Master Information Posting

		 DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)   
		 DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
		 DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
		 SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
		 WHERE PostingDate = @ApprovalDate

			SET @VoucherType = CASE 
                WHEN @BankId IS NOT NULL AND @BankId != 0 THEN 'CreditVoucherBank'
                WHEN @CashId != 0 AND @CashId IS NOT NULL THEN 'CreditVoucherCash'
            END
		
			IF(@VoucherType='DebitVoucherBank')
			BEGIN
				SET @VoucherCode='DVB-'+@VoucherCode	
			END

			IF(@VoucherType='DebitVoucherCash')
			BEGIN
				SET @VoucherCode='DVC-'+@VoucherCode
			END

			IF(@VoucherType='CreditVoucherBank')
			BEGIN
				SET @VoucherCode='CVB-'+@VoucherCode	
			END

			IF(@VoucherType='CreditVoucherCash')
			BEGIN
				SET @VoucherCode='CVC-'+@VoucherCode
			END

			IF(@VoucherType='FundTransfer')
			BEGIN
				SET @VoucherCode='FUT-'+@VoucherCode
			END

			IF(@VoucherType='Journal')
			BEGIN
				SET @VoucherCode='JVP-'+@VoucherCode
			END

			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
			        ( VoucherCode ,
			          CompanyInfoId , 
			          FinancialYearId ,
			          PostingDate ,
			          VoucherCategoryID ,
			          CurrencyId ,
			          CurrencyCovRate ,
			          Narration ,        
			          PaytoIsSupplier ,
			          IsInvoice ,
			          TotalAmountBDT ,
			          TotalAmount ,
			          Remarks ,
			          EntryBy ,
			          EntryDate ,
			          VoucherType ,
			          ActionStatus,
					  IsCostCenter,
					  IsBuyerCostCenter,
					  IsOtherCostCenter,
					  ApprovedBy,
					  ApprovedDate,
					  CommonPK_ID,
					  TableID
			        )
			VALUES  ( 
					  @VoucherCode ,
			          @BiusinessUnitId , 
			          @FinancialYearId ,
			          @ApprovalDate ,
			          2 ,
			          @CurrencyId ,
			          @Conversionrate ,

					 'An amount of BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + 
						CASE 
						    WHEN @Discount > 0 THEN ', including a discount of BDT ' + CONVERT(NVARCHAR(50), @Discount) 
						    ELSE '' 
						END + 
						CASE 
						    WHEN @AIT > 0 THEN ', and an AIT of BDT ' + CONVERT(NVARCHAR(50), @AIT) 
						    ELSE '' 
						END + 
						' has been received from customer "' + @CustomerName + 
						'" against Invoice No. "' + @InvoiceNo + 
						'" on ' + CONVERT(NVARCHAR(50), @ApprovalDate, 105) + '.' , 
						

			          0 ,
			          0 ,
			          @DeliveryNetAmount + @Discount + @AIT,
			          @DeliveryNetAmount + @Discount + @AIT,
			          'Auto Posting',
			          @ApproveBy ,
			          @ApprovalDate,
			          @VoucherType,
			          'Approved',
					  0,0,0,'Auto Process',
					  @ApprovalDate,@InvoiceId2,@TableId
			        )
			
			SET @VoucherMasterId = SCOPE_IDENTITY()

			-- Detail Information Posting

			-- Bank / Cash GL

			DECLARE	@DrGl INT = 0 

            IF (@BankId IS NOT NULL AND @BankId != 0)
            BEGIN 
                SELECT @DrGl = AccountId 
                FROM CSTL_ACCDB..tblChartOfAccounts 
                WHERE BankAccountId = @BankId
            END

            IF(@CashId IS NOT NULL AND @CashId != 0)
            BEGIN 
                SELECT @DrGl = AccountId 
                FROM CSTL_ACCDB..tblChartOfAccounts 
                WHERE CashAccountId = @CashId
            END

			-- Debit Side 1

			IF(ISNULL(@DeliveryNetAmount,0) > 0)
			BEGIN

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks ,RefInfoInd) 
				VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Customer Payment',@ReferenceInfoId);

			END

			-- Debit Side 2 -- Discount Amount

			IF(@Discount > 0)
			BEGIN

				DECLARE	@DrGl1 INT = 0 
				SELECT @DrGl1 = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1023 -- Sales Discount

				--PRINT @DrGl1

				-- Debit Side

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks,RefInfoInd) 
				VALUES (@VoucherMasterId, @DrGl1, 'Debit', 0.00, 0.00, @Discount, 0.00, 'Customer Payment',@ReferenceInfoId);

				
			END

			
			-- Debit Side 2 -- AIT Amount

			IF(@AIT > 0)
			BEGIN

				DECLARE	@DrGl2 INT = 0 
				SELECT @DrGl2 = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1024 -- Sales Discount

				--PRINT @DrGl2

				-- Debit Side

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks,RefInfoInd) 
				VALUES (@VoucherMasterId, @DrGl2, 'Debit', 0.00, 0.00, @AIT, 0.00, 'Customer Payment',@ReferenceInfoId);

				
			END
			

			-- Credit Side

			DECLARE	@CrGl INT = 0 
			SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2  -- Accounts Receivable
			--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

			--PRINT @CrGl
			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks, SubsidoryLedgerId, RefInfoInd)
			VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, (@DeliveryNetAmount + @AIT + @Discount), 0, 0, 'Customer Payment', @SubsidiaryInfoId, 0);

		
		
		FETCH NEXT FROM @MyCursor2
		INTO  @BiusinessUnitId,@FinancialYearId,@ApprovalDate,@DeliveryNetAmount,@Discount,@AIT,@TableId,@InvoiceId2,@CustomerName,@InvoiceNo,
		@BankId, @CashId,@SubsidiaryInfoId,@ReferenceInfoId

		
		END
		CLOSE @MyCursor2
		DEALLOCATE @MyCursor2

END
	
```

---

## `sp_Sales_CustomerPaymentReversePosting`

**Parameters:**
- `@InvoiceId` — INT, input
- `@CustPayId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblChartOfAccounts, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice, tblPaymentTranscationMaster
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster; UPDATE → INV, Invoice, tblPaymentTranscationMaster; DELETE → Payment, tblCustPayDetail, tblCustomerPay

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblChartOfAccounts, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice, tblPaymentTranscationMaster, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster; updates INV, Invoice, tblPaymentTranscationMaster; deletes from Payment, tblCustPayDetail, tblCustomerPay.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT InvoiceId, SUM(ISNULL(PaymentAmount, 0)) AS PaymentAmount, SUM(ISNULL(AIT, 0)) AS AIT, SUM(ISNULL(Discount, 0)) AS Discount FROM tblCustPayDetail WHERE InvoiceId = @InvoiceId GROUP BY InvoiceId ) AS AGG ON INV.InvoiceId = AGG.InvoiceId WHERE INV.InvoiceId = @InvoiceId; END`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:280` — `return aCommonInternalDal.UpdateAction("sp_Sales_CustomerPaymentReversePosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Sales_CustomerPaymentReversePosting] 

	@InvoiceId INT,
	@CustPayId INT

AS
BEGIN


		--SELECT * FROM dbo.tblCustPayDetail AS CPD 
		--LEFT JOIN dbo.tblCustomerPay AS CPY ON CPD.CustPayId = CPY.CustPayId
		--WHERE CPD.PaymentAmount > 0 AND ISNULL(CPD.AIT, 0) > 0 AND ISNULL(CPD.Discount, 0) > 0

		-- Reject Previous Not Processd Payment

		UPDATE tblPaymentTranscationMaster SET IsReject = 1
		WHERE CONVERT(DATE,CreateDate) < CONVERT(DATE,GETDATE()) AND IsProcess = 0 AND IsReject = 0


		--SELECT * FROM tblPaymentTranscationMaster  WHERE CONVERT(DATE,CreateDate) < CONVERT(DATE,GETDATE()) AND IsProcess = 0 AND IsReject = 0

		--UPdate Invoice

		

		

		-- Journal Posting 

		DECLARE @VoucherMasterId INT
		DECLARE @VoucherCode NVARCHAR(MAX)
		DECLARE @BiusinessUnitId INT
		--DECLARE @SupplierId INT
		--DECLARE @BankAccountId INT
		--DECLARE @CashAccountId INT
		DECLARE @FinancialYearId INT
 --		  DECLARE @DateOfPayment DATETIME
		DECLARE @CurrencyId INT = 1
		DECLARE @Conversionrate DECIMAL(18,4) = 1.00
 --		  DECLARE @Narration NVARCHAR(MAX)
 --		  DECLARE @TotalPayAmountBDT DECIMAL(18,4)
 --		  DECLARE @TotalAmount DECIMAL(18,4)
 --		  DECLARE @VoucherCategory INT
 --		  DECLARE @ChecqueNo  NVARCHAR(MAX)
 --		  DECLARE @ChecqueDate DATETIME
 --		  DECLARE @ReferenceNo NVARCHAR(MAX)
 --		  DECLARE @ReferenceDate DATETIME
		--DECLARE @PurchaseOrderLocalCode2 NVARCHAR(500)
		DECLARE @VoucherType NVARCHAR(200) ='Journal'
		DECLARE @ApprovalDate DATETIME
		DECLARE @ApproveBy NVARCHAR(200)
		DECLARE @TableId INT
		DECLARE @InvoiceNo  NVARCHAR(200)
		DECLARE @MaterialConversionDate DATETIME
		DECLARE @DeliveryNetAmount  DECIMAL(18,4)

		DECLARE @InvoiceId2 INT
		DECLARE @CustomerName NVARCHAR(500)
		DECLARE @BankId INT
		DECLARE @CashId INT

		DECLARE @SubsidiaryInfoId INT
		DECLARE @ReferenceInfoId INT

		-- For Credit Amount

		DECLARE @AIT DECIMAL(18,4) = 0
		DECLARE @Discount DECIMAL(18,4) = 0
		        
		--------------------------------------------------------

		DECLARE @MyCursor2 CURSOR
		SET @MyCursor2 = CURSOR FAST_FORWARD
		FOR
		-----------

		--DECLARE @InvoiceId INT = 546854
	 --   DECLARE @CustPayId INT = 540816

		SELECT DISTINCT CI.CompanyId,FY.FinancialYearId,FORMAT(GETDATE(), 'dd-MMM-yyyy') UpdateDate,
		CPD.PaymentAmount,ISNULL(CPD.Discount, 0) AS Discount,ISNULL(CPD.AIT, 0) AS AIT,TBL.IntegrationTableId,CPY.CustPayId,
		CMS.CustomerName + ' (' + CMS.CustomerCode + ')' AS CustomerName,InvoiceNo,CPD.BankAccId,CPD.CashAccId,
		ISNULL(SB.SubsidiaryInfoId,0) SubsidiaryInfoId,ISNULL(RF.ReferenceInfoId,0) ReferenceInfoId  FROM tblCustPayDetail AS CPD 
        LEFT JOIN dbo.tblCustomerPay AS CPY ON CPD.CustPayId = CPY.CustPayId
        LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = CPD.InvoiceId
        INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = tblInvoice.ComUnitId
		INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		LEFT JOIN tblCustMaster AS CMS ON tblInvoice.CustomerMasterId = CMS.CustomerMasterId

		-- Subsidary

		LEFT JOIN (SELECT S_Info_Id,SubsidiaryInfoId FROM CSTL_ACCDB..tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1) AS SB ON tblInvoice.CustomerMasterId = SB.S_Info_Id

		-- Reference

		LEFT JOIN CSTL_ACCDB..tblReferenceInfo AS RF ON RF.ReferenceTypeId = 1016 -- Invoice
		AND R_Info_Id = @InvoiceId 

		INNER JOIN (SELECT CompanyId,FinancialYearId,StartDate,EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND GETDATE() BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblCustomerPay'))) 	
		LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2017 AND  CPY.CustPayId = V.CommonPK_ID
		WHERE 
		--ISNULL(V.VoucherMasterId,0) = 0 AND 
		
		(CPD.PaymentAmount + ISNULL(CPD.Discount, 0) + ISNULL(CPD.AIT, 0)) > 0 
		--AND CI.CompanyId IN (16) -- 2- Critical Care, 16 - Sales & Marketing 
		AND  CPD.InvoiceId = @InvoiceId -- 275680
		AND CPD.CustPayId = @CustPayId -- 274967


		----------

		OPEN @MyCursor2
		FETCH NEXT FROM @MyCursor2                               
		INTO @BiusinessUnitId,@FinancialYearId,@ApprovalDate,@DeliveryNetAmount,@Discount,@AIT,@TableId,@InvoiceId2,
		@CustomerName,@InvoiceNo, @BankId, @CashId, @SubsidiaryInfoId,@ReferenceInfoId
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

		--SELECT * FROM CSTL_ACCDB..tblVoucherCategory

		 --- Master Information Posting

		 DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)   
		 DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
		 DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
		 SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
		 WHERE PostingDate = @ApprovalDate

			SET @VoucherType = CASE 
                WHEN @BankId IS NOT NULL AND @BankId != 0 THEN 'CreditVoucherBank'
                WHEN @CashId != 0 AND @CashId IS NOT NULL THEN 'CreditVoucherCash'
            END
		
			IF(@VoucherType='DebitVoucherBank')
			BEGIN
				SET @VoucherCode='DVB-'+@VoucherCode	
			END

			IF(@VoucherType='DebitVoucherCash')
			BEGIN
				SET @VoucherCode='DVC-'+@VoucherCode
			END

			IF(@VoucherType='CreditVoucherBank')
			BEGIN
				SET @VoucherCode='CVB-'+@VoucherCode	
			END

			IF(@VoucherType='CreditVoucherCash')
			BEGIN
				SET @VoucherCode='CVC-'+@VoucherCode
			END

			IF(@VoucherType='FundTransfer')
			BEGIN
				SET @VoucherCode='FUT-'+@VoucherCode
			END

			IF(@VoucherType='Journal')
			BEGIN
				SET @VoucherCode='JVP-'+@VoucherCode
			END

			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
			        ( VoucherCode ,
			          CompanyInfoId , 
			          FinancialYearId ,
			          PostingDate ,
			          VoucherCategoryID ,
			          CurrencyId ,
			          CurrencyCovRate ,
			          Narration ,        
			          PaytoIsSupplier ,
			          IsInvoice ,
			          TotalAmountBDT ,
			          TotalAmount ,
			          Remarks ,
			          EntryBy ,
			          EntryDate ,
			          VoucherType ,
			          ActionStatus,
					  IsCostCenter,
					  IsBuyerCostCenter,
					  IsOtherCostCenter,
					  ApprovedBy,
					  ApprovedDate,
					  CommonPK_ID,
					  TableID
			        )
			VALUES  ( 
					  @VoucherCode ,
			          @BiusinessUnitId , 
			          @FinancialYearId ,
			          @ApprovalDate ,
			          2 ,
			          @CurrencyId ,
			          @Conversionrate ,

					 'An earlier received amount of BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + 
		CASE 
		    WHEN @Discount > 0 THEN ', which included a discount of BDT ' + CONVERT(NVARCHAR(50), @Discount) 
		    ELSE '' 
		END + 
		CASE 
		    WHEN @AIT > 0 THEN ', and an AIT of BDT ' + CONVERT(NVARCHAR(50), @AIT) 
		    ELSE '' 
		END + 
		' has been **reversed** for customer "' + @CustomerName + 
		'" against Invoice No. "' + @InvoiceNo + 
		'" on ' + CONVERT(NVARCHAR(50), @ApprovalDate, 105) + '.',

						

			          0 ,
			          0 ,
			          @DeliveryNetAmount ,
			          @DeliveryNetAmount ,
			          'Auto Posting',
			          @ApproveBy ,
			          @ApprovalDate,
			          @VoucherType,
			          'Approved',
					  0,0,0,'Auto Process',
					  @ApprovalDate,@InvoiceId2,@TableId
			        )
			
			SET @VoucherMasterId = SCOPE_IDENTITY()

			-- Detail Information Posting


			-- Debit Side

			DECLARE	@CrGl INT = 0 
			SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2  -- Accounts Receivable
			--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

			--PRINT @CrGl
			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks, SubsidoryLedgerId, RefInfoInd)
			VALUES (@VoucherMasterId, @CrGl, 'Debit', (@DeliveryNetAmount + @AIT + @Discount), 0, 0, 0, 'Customer Payment', 0, @ReferenceInfoId);




			-- Bank / Cash GL

			DECLARE	@DrGl INT = 0 

            IF (@BankId IS NOT NULL AND @BankId != 0)
            BEGIN 
                SELECT @DrGl = AccountId 
                FROM CSTL_ACCDB..tblChartOfAccounts 
                WHERE BankAccountId = @BankId
            END

            IF(@CashId IS NOT NULL AND @CashId != 0)
            BEGIN 
                SELECT @DrGl = AccountId 
                FROM CSTL_ACCDB..tblChartOfAccounts 
                WHERE CashAccountId = @CashId
            END

			-- Debit Side 1

			IF(ISNULL(@DeliveryNetAmount,0) > 0)
			BEGIN

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks ,SubsidoryLedgerId) 
				VALUES (@VoucherMasterId, @DrGl, 'Credit', 0.00, 0.00, 0, @DeliveryNetAmount, 'Customer Payment',@SubsidiaryInfoId);

			END

			-- Debit Side 2 -- Discount Amount

			IF(@Discount > 0)
			BEGIN

				DECLARE	@DrGl1 INT = 0 
				SELECT @DrGl1 = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1023 -- Sales Discount

				--PRINT @DrGl1

				-- Debit Side

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks,SubsidoryLedgerId) 
				VALUES (@VoucherMasterId, @DrGl1, 'Credit', 0.00, 0.00,0.00 , @Discount, 'Customer Payment',@SubsidiaryInfoId);

				
			END

			
			-- Debit Side 2 -- AIT Amount

			IF(@AIT > 0)
			BEGIN

				DECLARE	@DrGl2 INT = 0 
				SELECT @DrGl2 = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1024 -- Sales Discount

				--PRINT @DrGl2

				-- Debit Side

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks,SubsidoryLedgerId) 
				VALUES (@VoucherMasterId, @DrGl2, 'Credit', 0.00, 0.00, 0.00,@AIT, 'Customer Payment',@SubsidiaryInfoId);

				
			END
			

			
		
		
		FETCH NEXT FROM @MyCursor2
		INTO  @BiusinessUnitId,@FinancialYearId,@ApprovalDate,@DeliveryNetAmount,@Discount,@AIT,@TableId,@InvoiceId2,@CustomerName,@InvoiceNo,
		@BankId, @CashId,@SubsidiaryInfoId,@ReferenceInfoId

		
		END
		CLOSE @MyCursor2
		DEALLOCATE @MyCursor2

		--Delete Payment

		DELETE FROM tblCustomerPay WHERE CustPayId = @CustPayId
		DELETE FROM tblCustPayDetail WHERE CustPayId = @CustPayId

		-- Update Invoice

		--DECLARE @InvoiceId INT = 616722

		UPDATE INV
		SET 
		    PaymentAmount = ISNULL(AGG.PaymentAmount,0) ,
		    AIT = ISNULL(AGG.AIT,0) ,
		    DiscountOnPayment = ISNULL(AGG.Discount,0),
			PaymentStatus = CASE 
								WHEN (ISNULL(AGG.PaymentAmount,0) + ISNULL(AGG.AIT,0) + ISNULL(AGG.Discount,0)) = 0 THEN NULL
								WHEN (ISNULL(AGG.PaymentAmount,0) + ISNULL(AGG.AIT,0) + ISNULL(AGG.Discount,0)) > 0 AND  (ISNULL(AGG.PaymentAmount,0) + ISNULL(AGG.AIT,0) + ISNULL(AGG.Discount,0)) = ISNULL(DeliveryTpGrandTotal,0) THEN 'Full'
								ELSE 'Partial'
							END
		FROM tblInvoice AS INV
		LEFT JOIN (
		    SELECT 
		        InvoiceId,
		        SUM(ISNULL(PaymentAmount, 0)) AS PaymentAmount,
		        SUM(ISNULL(AIT, 0)) AS AIT,
		        SUM(ISNULL(Discount, 0)) AS Discount
		    FROM tblCustPayDetail
		    WHERE InvoiceId = @InvoiceId
		    GROUP BY InvoiceId
		) AS AGG ON INV.InvoiceId = AGG.InvoiceId
		WHERE INV.InvoiceId = @InvoiceId;

END
	
```

---

## `sp_Sales_DeliveryCOGSPosting`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, tblCompanyUnit, tblCustMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tblProduct, tblUnitPrice
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, tblCompanyUnit, tblCustMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tblProduct, tblUnitPrice, then inserts into `CSTL_ACCDB..tblDebitCreditVoucherDetail` (columns: VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks); inserts into `CSTL_ACCDB..tblDebitCreditVoucherMaster` (columns: VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,...).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK) WHERE CommonPK_ID = @InvoiceId AND TableID = 20 AND ActionStatus <> 'Cancelled' AND Narration LIKE '%Cost of Goods Sold (COGS)%' ) BEGIN DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@DeliveryDate)), 3, 2`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =======================================================================================
-- 5. sp_Sales_DeliveryCOGSPosting (Delivery COGS Posting using UnitPrice_CostPrice)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Sales_DeliveryCOGSPosting]
    @InvoiceId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT, @FinancialYearId INT, @InvoiceNo NVARCHAR(100), @DeliveryDate DATETIME;
    DECLARE @TotalCost DECIMAL(18,4) = 0, @CustomerName NVARCHAR(200);

    SELECT 
        @CompanyId = CU.CompanyId,
        @InvoiceNo = I.DelivaryInvoiceNo,
        @DeliveryDate = ISNULL(I.UpdateDate, I.InvoiceDate),
        @CustomerName = ISNULL(CM.CustomerName, 'Customer')
    FROM dbo.tblInvoice I WITH (NOLOCK)
    INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON I.ComUnitId = CU.ComUnitId
    LEFT JOIN dbo.tblCustMaster CM WITH (NOLOCK) ON I.CustomerMasterId = CM.CustomerMasterId
    WHERE I.InvoiceId = @InvoiceId;

    SELECT TOP 1 @FinancialYearId = FinancialYearId 
    FROM CSTL_ACCDB..tblFinancialYear WITH (NOLOCK)
    WHERE CompanyId = @CompanyId AND @DeliveryDate BETWEEN StartDate AND EndDate;

    -- Calculate total COGS using tblDCStore.UnitPrice_CostPrice
    SELECT 
        @TotalCost = ISNULL(SUM(CAST(ISNULL(ID.DeliveryTotalQuantity, ID.TotalQuantity) AS DECIMAL(18,4)) * ISNULL(DCS.UnitPrice_CostPrice, ISNULL(UP.UnitPrice, 0))), 0)
    FROM dbo.tblInvoiceDetail ID WITH (NOLOCK)
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK) ON ID.DCStoreId = DCS.DCStoreId
    LEFT JOIN dbo.tblProduct P WITH (NOLOCK) ON ID.ProductCode = P.ProductCode
    LEFT JOIN dbo.tblUnitPrice UP WITH (NOLOCK) ON P.ProductId = UP.ProductId AND UP.IsActive = 1
    WHERE ID.InvoiceId = @InvoiceId;

    IF (@TotalCost > 0)
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK)
            WHERE CommonPK_ID = @InvoiceId AND TableID = 20 AND ActionStatus <> 'Cancelled'
              AND Narration LIKE '%Cost of Goods Sold (COGS)%'
        )
        BEGIN
            DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@DeliveryDate)), 3, 2);
            DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CAST(MONTH(@DeliveryDate) AS NVARCHAR(2)), 2);
            DECLARE @dateText NVARCHAR(10) = RIGHT('0' + CAST(DAY(@DeliveryDate) AS NVARCHAR(2)), 2);

            DECLARE @MaxNum INT = 10000;
            SELECT @MaxNum = ISNULL(MAX(VoucherMasterId), 10000) FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK);

            DECLARE @VoucherCode NVARCHAR(50) = 'JVP-' + @yearText + @monthText + @dateText + CAST((@MaxNum % 89999) + 10001 AS NVARCHAR(10));

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
            (
                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, CurrencyId, CurrencyCovRate,
                Narration, PaytoIsSupplier, IsInvoice, TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
                ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy, ApprovedDate, CommonPK_ID, TableID, IsInventoryValuation
            )
            VALUES
            (
                @VoucherCode, @CompanyId, @FinancialYearId, @DeliveryDate, 2, 1, 1.00,
                'Cost of Goods Sold (COGS) for Delivery Invoice: ' + @InvoiceNo + ' to ' + @CustomerName + '.',
                0, 0, @TotalCost, @TotalCost, 'Auto COGS Posting', 'System', @DeliveryDate, 'Journal',
                'Approved', 0, 0, 0, 'Auto Process', @DeliveryDate, @InvoiceId, 20, 1
            );

            DECLARE @VoucherMasterId INT = SCOPE_IDENTITY();

            -- DR: Cost of Goods Sold (4162)
            DECLARE @DrGl INT = 0;
            SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1021 AND IsInventory = 1;
            IF ISNULL(@DrGl, 0) = 0 SET @DrGl = 4162;

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @TotalCost, 0.00, 'Cost of Goods Sold COGS');

            -- CR: Finished Goods Inventory (4156)
            DECLARE @CrGl INT = 0;
            SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1014 AND IsInventory = 1;
            IF ISNULL(@CrGl, 0) = 0 SET @CrGl = 4156;

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks)
            VALUES (@VoucherMasterId, @CrGl, 'Credit', 0.00, @TotalCost, 0.00, 0.00, 'Finished Goods Inventory Stock Out');
        END;
    END;
END;
```

---

## `sp_Sales_DeliveryCOGSReversePosting`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tblProduct, tbl_INV_WeightedAvg, tbl_PTP_AdavnecPayToSupplierMaster
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tblProduct, tbl_INV_WeightedAvg, tbl_PTP_AdavnecPayToSupplierMaster, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Sales_DeliveryCOGSReversePosting] 

	@InvoiceId INT

AS
BEGIN

	--SELECT * FROM tblInvoice AS INV
	--LEFT JOIN tblInvoiceDetail AS IVD ON INV.InvoiceId = IVD.InvoiceId
	--LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	--WHERE CompanyId = 16 AND INV.InvoiceId = 548554


	-- Approve or Reject

	DECLARE @DeliveyInvoiceStatus NVARCHAR(200) = NULL
	SELECT @DeliveyInvoiceStatus = DeliveryInvoiceStatus FROM tblInvoice WHERE  InvoiceId = @InvoiceId

	--PRINT @DeliveyInvoiceStatus



	-- Insert 

	IF(LTRIM(RTRIM(UPPER(@DeliveyInvoiceStatus))) = LTRIM(RTRIM(UPPER('Full'))) OR LTRIM(RTRIM(UPPER(@DeliveyInvoiceStatus))) = LTRIM(RTRIM(UPPER('Partial'))))
	BEGIN

		-- Journal Posting 

		DECLARE @VoucherMasterId INT
		DECLARE @VoucherCode NVARCHAR(MAX)
		DECLARE @BiusinessUnitId INT
		--DECLARE @SupplierId INT
		--DECLARE @BankAccountId INT
		--DECLARE @CashAccountId INT
		DECLARE @FinancialYearId INT
 --		  DECLARE @DateOfPayment DATETIME
		DECLARE @CurrencyId INT = 1
		DECLARE @Conversionrate DECIMAL(18,4) = 1.00
 --		  DECLARE @Narration NVARCHAR(MAX)
 --		  DECLARE @TotalPayAmountBDT DECIMAL(18,4)
 --		  DECLARE @TotalAmount DECIMAL(18,4)
 --		  DECLARE @VoucherCategory INT
 --		  DECLARE @ChecqueNo  NVARCHAR(MAX)
 --		  DECLARE @ChecqueDate DATETIME
 --		  DECLARE @ReferenceNo NVARCHAR(MAX)
 --		  DECLARE @ReferenceDate DATETIME
		--DECLARE @PurchaseOrderLocalCode2 NVARCHAR(500)
		DECLARE @VoucherType NVARCHAR(200) ='Journal'
		DECLARE @ApprovalDate DATETIME
		DECLARE @ApproveBy NVARCHAR(200)
		DECLARE @TableId INT
		DECLARE @InvoiceNo  NVARCHAR(200)
		DECLARE @MaterialConversionDate DATETIME
		DECLARE @DeliveryNetAmount  DECIMAL(18,4)

		DECLARE @InvoiceId2 INT
		DECLARE @CustomerName NVARCHAR(500)

		-- For Credit Amount

		DECLARE @ToatlAdvanceAmountFc DECIMAL(18,4) = 0
		        
		--------------------------------------------------------
		DECLARE @MyCursor2 CURSOR
		SET @MyCursor2 = CURSOR FAST_FORWARD
		FOR
		---------------

		SELECT DISTINCT CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,FORMAT(INV.UpdateDate, 'dd-MMM-yyyy') UpdateDate,
		SUM(ISNULL(INVD.DeliveryTotalQuantity,0) * ISNULL(CS.Avg_Rate,0)) CostAmount,TBL.IntegrationTableId,INV.InvoiceId, 
		CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' END CustomerName  FROM tblInvoice AS INV WITH(NOLOCK)
		LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
		LEFT JOIN tblCustMaster AS CMS ON INV.CustomerMasterId = CMS.CustomerMasterId
		INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = INVD.DCStoreId
		LEFT JOIN tblProduct AS PD ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))
		INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
		INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
		INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId

		-- Current Rate

		LEFT JOIN (SELECT WG.Weighted_Avg_Id,WG.Company_Id,ISNULL(WG.Warehouse_Id, 0) AS Warehouse_Id,WG.Item_Id,WG.Close_Qty AS CurrentStock2,WG.Avg_Rate,WG.Curr_Rate,WG.Trans_Date
		FROM dbo.tbl_INV_WeightedAvg WG
		JOIN (SELECT Item_Id,Company_Id,MAX(Weighted_Avg_Id) AS MaxWeighted_Avg_Id FROM dbo.tbl_INV_WeightedAvg GROUP BY Item_Id, Company_Id
		) Latest ON WG.Item_Id = Latest.Item_Id AND WG.Company_Id = Latest.Company_Id AND WG.Weighted_Avg_Id = Latest.MaxWeighted_Avg_Id
		) AS CS ON CI.CompanyId = CS.Company_Id AND PD.ProductId = CS.Item_Id

		INNER JOIN (SELECT CompanyId,FinancialYearId,StartDate,EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND INV.UpdateDate BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblInvoice'))) 	
		--LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2014 AND INV.InvoiceId = V.CommonPK_ID
		WHERE  INV.InvoiceId = @InvoiceId --548554
		GROUP BY CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,INV.UpdateDate,TBL.IntegrationTableId,INV.InvoiceId, CMS.CustomerCode, CustomerName
		HAVING SUM(ISNULL(INVD.DeliveryTotalQuantity,0) * ISNULL(CS.Avg_Rate,0)) > 0


		--WM.WHStockInMasterID,WHStockInCode,WM.WHStockInDate,TotalQuantity,
		--TotalValue,ChallanNo,ChallanDate,WH.CompanyId
  
		 --SELECT * FROM tbl_PTP_AdavnecPayToSupplierMaster  M.ApprovalStatus = 'Accepted' AND 

		----------
		OPEN @MyCursor2
		FETCH NEXT FROM @MyCursor2                               
		INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,@CustomerName
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

		--SELECT * FROM CSTL_ACCDB..tblVoucherCategory

		 --- Master Information Posting

		 DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)   
		 DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
		 DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
		 SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
		 WHERE PostingDate = @ApprovalDate
		
			IF(@VoucherType='DebitVoucherBank')
			BEGIN
				SET @VoucherCode='DVB-'+@VoucherCode	
			END

			IF(@VoucherType='DebitVoucherCash')
			BEGIN
				SET @VoucherCode='DVC-'+@VoucherCode
			END

			IF(@VoucherType='CreditVoucherBank')
			BEGIN
				SET @VoucherCode='CVB-'+@VoucherCode	
			END

			IF(@VoucherType='CreditVoucherCash')
			BEGIN
				SET @VoucherCode='CVC-'+@VoucherCode
			END

			IF(@VoucherType='FundTransfer')
			BEGIN
				SET @VoucherCode='FUT-'+@VoucherCode
			END

			IF(@VoucherType='Journal')
			BEGIN
				SET @VoucherCode='JVP-'+@VoucherCode
			END

			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
			        ( VoucherCode ,
			          CompanyInfoId , 
			          FinancialYearId ,
			          PostingDate ,
			          VoucherCategoryID ,
			          CurrencyId ,
			          CurrencyCovRate ,
			          Narration ,        
			          PaytoIsSupplier ,
			          IsInvoice ,
			          TotalAmountBDT ,
			          TotalAmount ,
			          Remarks ,
			          EntryBy ,
			          EntryDate ,
			          VoucherType ,
			          ActionStatus,
					  IsCostCenter,
					  IsBuyerCostCenter,
					  IsOtherCostCenter,
					  ApprovedBy,
					  ApprovedDate,
					  CommonPK_ID,
					  TableID
			        )
			VALUES  ( 
					  @VoucherCode ,
			          @BiusinessUnitId , 
			          @FinancialYearId ,
			          @ApprovalDate ,
			          2 ,
			          @CurrencyId ,
			          @Conversionrate ,
					  'An expense of BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + ' for the Cost of Goods Sold to ' + @CustomerName + ' under invoice ' + @InvoiceNo + ' has been reversed.',    
			          0 ,
			          0 ,
			          @DeliveryNetAmount ,
			          @DeliveryNetAmount ,
			          'Auto Posting' ,
			          @ApproveBy ,
			          @ApprovalDate ,
			          @VoucherType ,
			          'Approved',
					  0,0,0,'Auto Process',
					  @ApprovalDate,@InvoiceId2,@TableId
			        )
			
			SET @VoucherMasterId = SCOPE_IDENTITY()



			-- Detail Information Posting

			DECLARE	@DrGl INT = 0 
			SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1014 AND IsInventory = 1 -- Finished Goods Inventory (FG) 

			PRINT @DrGl

			-- Debit Side

			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks) 
			VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Cost of Goods Sold');

			-- Credit Side

			DECLARE	@CrGl INT = 0 
			SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1021 AND IsInventory = 1 -- Cost of Goods Sold (COGS)
			--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

			PRINT @CrGl
			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, RefInfoInd, Remarks)
			VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, @DeliveryNetAmount, 0, 0, 0, 'Cost of Goods Sold');

		
		
		FETCH NEXT FROM @MyCursor2
		INTO  @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,@CustomerName

		
		END
		CLOSE @MyCursor2
		DEALLOCATE @MyCursor2

	END
	
	-- Process Stock

	DECLARE @DeliveyInvoiceNo NVARCHAR(200) = NULL
	SELECT @DeliveyInvoiceNo = DelivaryInvoiceNo FROM tblInvoice WHERE InvoiceId = @InvoiceId

	EXEC sp_Process_DeleteDeliveryInvoice @DelivaryInvoiceNo = @DeliveyInvoiceNo; 

	
END
```

---

## `sp_Sales_DeliverySalesPosting`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tblPurchaseOrderLocalMaster, tbl_PTP_AdavnecPayToSupplierMaster
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblReferenceInfo; UPDATE → INV, Invoice

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tblPurchaseOrderLocalMaster, tbl_PTP_AdavnecPayToSupplierMaster, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblReferenceInfo; updates INV, Invoice.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT InvoiceId, SUM(DeliveryTotalPrice) AS DeliveryTotalPrice, SUM(DeliveryDiscountAmount) AS DeliveryDiscountAmount, SUM(DeliveryNetAmount) AS DeliveryNetAmount FROM tblInvoiceDetail WHERE InvoiceId = @InvoiceId GROUP BY InvoiceId ) AS AGG ON INV.InvoiceId = AGG.InvoiceId WHERE INV.InvoiceId = @InvoiceId;`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1674` — `//return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Sales_DeliverySalesPosting] 

	@InvoiceId INT

AS
BEGIN

	--SELECT * FROM tblInvoice AS INV
	--LEFT JOIN tblInvoiceDetail AS IVD ON INV.InvoiceId = IVD.InvoiceId
	--LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	--WHERE CompanyId = 16 AND INV.InvoiceId = 509565

	-- Update Invoice

	UPDATE INV
	SET 
	    DeliveryTpDiscount = AGG.DeliveryDiscountAmount,
	    DeliveryTpGrandTotal = AGG.DeliveryNetAmount,
	    DeliveryTpTotal = AGG.DeliveryTotalPrice
	FROM tblInvoice AS INV
	INNER JOIN (
	    SELECT InvoiceId,
	        SUM(DeliveryTotalPrice) AS DeliveryTotalPrice,
	        SUM(DeliveryDiscountAmount) AS DeliveryDiscountAmount,
	        SUM(DeliveryNetAmount) AS DeliveryNetAmount
	    FROM tblInvoiceDetail WHERE InvoiceId = @InvoiceId GROUP BY InvoiceId
	) AS AGG ON INV.InvoiceId = AGG.InvoiceId
	WHERE INV.InvoiceId = @InvoiceId;



	-- Approve or Reject

	DECLARE @DeliveyInvoiceStatus NVARCHAR(200) = NULL
	SELECT @DeliveyInvoiceStatus = DeliveryInvoiceStatus FROM tblInvoice WHERE  InvoiceId = @InvoiceId

	--PRINT @DeliveyInvoiceStatus

	-- Insert 

	IF(LTRIM(RTRIM(UPPER(@DeliveyInvoiceStatus))) = LTRIM(RTRIM(UPPER('Full'))) OR LTRIM(RTRIM(UPPER(@DeliveyInvoiceStatus))) = LTRIM(RTRIM(UPPER('Partial'))))
	BEGIN

		-- Reference Posting

		DECLARE @Count INT = 0
		SELECT @Count = COUNT(ReferenceInfoId) FROM CSTL_ACCDB.. tblReferenceInfo 
		WHERE ReferenceTypeId = 1016 AND R_Info_Id = @InvoiceId


		DECLARE @DeliveyInvoiceNo NVARCHAR(200) = NULL
		SELECT @DeliveyInvoiceNo = DelivaryInvoiceNo FROM tblInvoice WHERE  InvoiceId = @InvoiceId

		--PRINT @Count

		--SELECT DISTINCT ApprovalStatus FROM tblPurchaseOrderLocalMaster
		
		IF(@Count = 0)
		BEGIN
			INSERT INTO CSTL_ACCDB..tblReferenceInfo
			   (ReferenceTypeId
			   ,R_Info_Id
			   ,IsSelect
			   ,Description
			   ,EntryBy
			   ,EntryDate)
			VALUES
           (1016,@InvoiceId,1,@DeliveyInvoiceNo,0,GETDATE())
		END


		-- Journal Posting 

		DECLARE @VoucherMasterId INT
		DECLARE @VoucherCode NVARCHAR(MAX)
		DECLARE @BiusinessUnitId INT
		--DECLARE @SupplierId INT
		--DECLARE @BankAccountId INT
		--DECLARE @CashAccountId INT
		DECLARE @FinancialYearId INT
 --		  DECLARE @DateOfPayment DATETIME
		DECLARE @CurrencyId INT = 1
		DECLARE @Conversionrate DECIMAL(18,4) = 1.00
 --		  DECLARE @Narration NVARCHAR(MAX)
 --		  DECLARE @TotalPayAmountBDT DECIMAL(18,4)
 --		  DECLARE @TotalAmount DECIMAL(18,4)
 --		  DECLARE @VoucherCategory INT
 --		  DECLARE @ChecqueNo  NVARCHAR(MAX)
 --		  DECLARE @ChecqueDate DATETIME
 --		  DECLARE @ReferenceNo NVARCHAR(MAX)
 --		  DECLARE @ReferenceDate DATETIME
		--DECLARE @PurchaseOrderLocalCode2 NVARCHAR(500)
		DECLARE @VoucherType NVARCHAR(200) ='Journal'
		DECLARE @ApprovalDate DATETIME
		DECLARE @ApproveBy NVARCHAR(200)
		DECLARE @TableId INT
		DECLARE @InvoiceNo  NVARCHAR(200)
		DECLARE @MaterialConversionDate DATETIME
		DECLARE @DeliveryNetAmount  DECIMAL(18,4)

		DECLARE @InvoiceId2 INT
		DECLARE @VoucherMasterId2 INT
		DECLARE @CustomerName NVARCHAR(500)

		DECLARE @SubsidiaryInfoId INT
		DECLARE @ReferenceInfoId INT

		-- For Credit Amount

		DECLARE @ToatlAdvanceAmountFc DECIMAL(18,4) = 0
		        
		--------------------------------------------------------
		DECLARE @MyCursor2 CURSOR
		SET @MyCursor2 = CURSOR FAST_FORWARD
		FOR
		---------------

		--SELECT * FROM tblInvoice


		--DECLARE @InvoiceId INT = 550556


		SELECT CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,FORMAT(INV.UpdateDate, 'dd-MMM-yyyy') UpdateDate,
		SUM(ISNULL(INVD.DeliveryNetAmount,0)) DeliveryNetAmount,TBL.IntegrationTableId,INV.InvoiceId, 
		CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' END CustomerName,V.VoucherMasterId,
		ISNULL(SB.SubsidiaryInfoId,0) SubsidiaryInfoId,ISNULL(RF.ReferenceInfoId,0) ReferenceInfoId
		FROM tblInvoice AS INV WITH(NOLOCK)
		LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
		LEFT JOIN tblCustMaster AS CMS ON INV.CustomerMasterId = CMS.CustomerMasterId
		INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = INVD.DCStoreId
		INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
		INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
		INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId

		-- Subsidary

		LEFT JOIN (SELECT S_Info_Id,SubsidiaryInfoId FROM CSTL_ACCDB..tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1) AS SB ON INV.CustomerMasterId = SB.S_Info_Id

		-- Reference

		LEFT JOIN CSTL_ACCDB..tblReferenceInfo AS RF ON RF.ReferenceTypeId = 1016 -- Invoice
		AND R_Info_Id = @InvoiceId 

		--LTRIM(RTRIM(UPPER(INV.InvoiceNo))) = LTRIM(RTRIM(UPPER(RF.Description))) 

		-- Table

		INNER JOIN (SELECT CompanyId,FinancialYearId,StartDate,EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND INV.UpdateDate BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblInvoice'))) 
		LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2014 AND INV.InvoiceId = V.CommonPK_ID AND ISNULL(IsInventoryValuation,0) = 0 -- For Sales Voucher
		WHERE ISNULL(V.VoucherMasterId,0) = 0 AND INV.InvoiceId =  @InvoiceId -- 548554
		--AND UNT.CompanyId = 16 -- Sales & Marketing
		GROUP BY CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,INV.UpdateDate,TBL.IntegrationTableId,
		INV.InvoiceId, CMS.CustomerCode, CustomerName,V.VoucherMasterId,SB.SubsidiaryInfoId,RF.ReferenceInfoId
		HAVING SUM(ISNULL(INVD.DeliveryNetAmount,0)) > 0


		--WM.WHStockInMasterID,WHStockInCode,WM.WHStockInDate,TotalQuantity,
		--TotalValue,ChallanNo,ChallanDate,WH.CompanyId
  
		 --SELECT * FROM tbl_PTP_AdavnecPayToSupplierMaster  M.ApprovalStatus = 'Accepted' AND 

		----------
		OPEN @MyCursor2
		FETCH NEXT FROM @MyCursor2                               
		INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,
		@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId,@ReferenceInfoId
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

		--SELECT * FROM CSTL_ACCDB..tblVoucherCategory

		 --- Master Information Posting

		 DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)   
		 DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
		 DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
		 SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
		 WHERE PostingDate = @ApprovalDate
		
			IF(@VoucherType='DebitVoucherBank')
			BEGIN
				SET @VoucherCode='DVB-'+@VoucherCode	
			END

			IF(@VoucherType='DebitVoucherCash')
			BEGIN
				SET @VoucherCode='DVC-'+@VoucherCode
			END

			IF(@VoucherType='CreditVoucherBank')
			BEGIN
				SET @VoucherCode='CVB-'+@VoucherCode	
			END

			IF(@VoucherType='CreditVoucherCash')
			BEGIN
				SET @VoucherCode='CVC-'+@VoucherCode
			END

			IF(@VoucherType='FundTransfer')
			BEGIN
				SET @VoucherCode='FUT-'+@VoucherCode
			END

			IF(@VoucherType='Journal')
			BEGIN
				SET @VoucherCode='JVP-'+@VoucherCode
			END

			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
			        ( VoucherCode ,
			          CompanyInfoId , 
			          FinancialYearId ,
			          PostingDate ,
			          VoucherCategoryID ,
			          CurrencyId ,
			          CurrencyCovRate ,
			          Narration ,        
			          PaytoIsSupplier ,
			          IsInvoice ,
			          TotalAmountBDT ,
			          TotalAmount ,
			          Remarks ,
			          EntryBy ,
			          EntryDate ,
			          VoucherType ,
			          ActionStatus,
					  IsCostCenter,
					  IsBuyerCostCenter,
					  IsOtherCostCenter,
					  ApprovedBy,
					  ApprovedDate,
					  CommonPK_ID,
					  TableID
			        )
			VALUES  ( 
					  @VoucherCode ,
			          @BiusinessUnitId , 
			          @FinancialYearId ,
			          @ApprovalDate ,
			          2 ,
			          @CurrencyId ,
			          @Conversionrate ,
					 'Sale to ' + @CustomerName + ' under invoice ' + @InvoiceNo + ' for BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + ', recorded as Accounts Receivable.'  ,     
			          0 ,
			          0 ,
			          @DeliveryNetAmount ,
			          @DeliveryNetAmount ,
			          'Auto Posting' ,
			          @ApproveBy ,
			          @ApprovalDate ,
			          @VoucherType ,
			          'Approved',
					  0,0,0,'Auto Process',
					  @ApprovalDate,@InvoiceId2,@TableId
			        )
			
			SET @VoucherMasterId = SCOPE_IDENTITY()



			-- Detail Information Posting

			DECLARE	@DrGl INT = 0 
			SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 2 -- Accounts Receivable

			--PRINT @DrGl

			-- Debit Side

			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit,
			DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks, SubsidoryLedgerId,RefInfoInd) 
			VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Sales', @SubsidiaryInfoId, 0);

			-- Credit Side

			DECLARE	@CrGl INT = 0 
			SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1020  -- Sales
			--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

			PRINT @CrGl
			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks, SubsidoryLedgerId,RefInfoInd)
			VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, @DeliveryNetAmount, 0, 0, 'Sales', 0, @ReferenceInfoId);

		
		
		FETCH NEXT FROM @MyCursor2
		INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,
		@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId,@ReferenceInfoId

		
		END
		CLOSE @MyCursor2
		DEALLOCATE @MyCursor2

	END
	
	---- COGS Posting

	EXEC sp_Sales_DeliveryCOGSPosting  @InvoiceId = @InvoiceId; 

	
END
```

---

## `sp_Sales_DeliverySalesReturnCOGSPosting`

**Parameters:**
- `@ReturnId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCompanyUnit, tblCustMaster, tblCustomerReturnDetail, tblCustomerReturnMaster, tblDCStore, tblInvoice, tblInvoiceDetail
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCompanyUnit, tblCustMaster, tblCustomerReturnDetail, tblCustomerReturnMaster, tblDCStore, tblInvoice, tblInvoiceDetail, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =======================================================================================
-- 10. sp_Sales_DeliverySalesReturnCOGSPosting (Sales Return COGS Reversal: DR 4156 / CR 4162)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Sales_DeliverySalesReturnCOGSPosting] 
	@ReturnId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ApprovalStatus NVARCHAR(200) = NULL;
	SELECT @ApprovalStatus = ActionStatus FROM tblCustomerReturnMaster WHERE CustReturnId = @ReturnId;

	IF(LTRIM(RTRIM(UPPER(@ApprovalStatus))) IN ('ACCEPTED', 'APPROVED'))
	BEGIN
		DECLARE @VoucherMasterId INT, @VoucherCode NVARCHAR(MAX), @BiusinessUnitId INT, @FinancialYearId INT;
		DECLARE @CurrencyId INT = 1, @Conversionrate DECIMAL(18,4) = 1.00, @VoucherType NVARCHAR(200) = 'Journal';
		DECLARE @ApprovalDate DATETIME, @ApproveBy NVARCHAR(200), @TableId INT, @DeliveryNetAmount DECIMAL(18,4);
		DECLARE @InvoiceNo NVARCHAR(200), @ReturnId2 INT, @CustomerName NVARCHAR(500);

		DECLARE @MyCursor2 CURSOR;
		SET @MyCursor2 = CURSOR FAST_FORWARD FOR
		SELECT 
			CI.CompanyId,
			FY.FinancialYearId,
			INV.DelivaryInvoiceNo,
			FORMAT(CRM.ApprovedDate, 'dd-MMM-yyyy') AS UpdateDate,
			SUM(ISNULL(CRD.ReturnQty, 0) * ISNULL(DCS.UnitPrice_CostPrice, 0)) AS CostAmount,
			TBL.IntegrationTableId,
			CRM.CustReturnId,
			CMS.CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' ELSE '' END AS CustomerName
		FROM tblCustomerReturnMaster AS CRM
		LEFT JOIN tblCustomerReturnDetail AS CRD ON CRM.CustReturnId = CRD.CustReturnId
		LEFT JOIN tblInvoiceDetail AS ID ON CRD.InvoiceDetailId = ID.InvoiceDetailId
		LEFT JOIN tblInvoice AS INV ON INV.InvoiceId = CRM.InvoiceId
		LEFT JOIN tblDCStore AS DCS ON DCS.DCStoreId = ID.DCStoreId
		INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
		INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		LEFT JOIN tblCustMaster AS CMS ON INV.CustomerMasterId = CMS.CustomerMasterId
		INNER JOIN (SELECT CompanyId, FinancialYearId, StartDate, EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY 
			ON CI.CompanyId = FY.CompanyId AND CRM.ApprovedDate BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = 'TBLCUSTOMERRETURNMASTER'
		LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON V.TableID = 23 AND CRM.CustReturnId = V.CommonPK_ID AND V.ActionStatus <> 'Cancelled'
		WHERE ISNULL(V.VoucherMasterId, 0) = 0 AND CRM.CustReturnId = @ReturnId
		GROUP BY CI.CompanyId, FY.FinancialYearId, INV.DelivaryInvoiceNo, CRM.ApprovedDate, TBL.IntegrationTableId, CRM.CustReturnId, CMS.CustomerCode, CMS.CustomerName
		HAVING SUM(ISNULL(CRD.ReturnQty, 0) * ISNULL(DCS.UnitPrice_CostPrice, 0)) > 0;

		OPEN @MyCursor2;
		FETCH NEXT FROM @MyCursor2 INTO @BiusinessUnitId, @FinancialYearId, @InvoiceNo, @ApprovalDate, @DeliveryNetAmount, @TableId, @ReturnId2, @CustomerName;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@ApprovalDate)), 3, 2);
			DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CAST(MONTH(@ApprovalDate) AS NVARCHAR(2)), 2);
			DECLARE @dateText NVARCHAR(10) = RIGHT('0' + CAST(DAY(@ApprovalDate) AS NVARCHAR(2)), 2);

			DECLARE @MaxNum INT = 10000;
			SELECT @MaxNum = ISNULL(MAX(VoucherMasterId), 10000) FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK);

			SET @VoucherCode = 'JVP-' + @yearText + @monthText + @dateText + CAST((@MaxNum % 89999) + 10001 AS NVARCHAR(10));

			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
			(
				VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, CurrencyId, CurrencyCovRate,
				Narration, PaytoIsSupplier, IsInvoice, TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
				ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy, ApprovedDate, CommonPK_ID, TableID, IsInventoryValuation
			)
			VALUES
			(
				@VoucherCode, @BiusinessUnitId, @FinancialYearId, @ApprovalDate, 2, @CurrencyId, @Conversionrate,
				'COGS adjusted. BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + ' returned by "' + @CustomerName + '" for Invoice "' + @InvoiceNo + '" on ' + CONVERT(NVARCHAR(50), @ApprovalDate, 105) + '.',     
				0, 0, @DeliveryNetAmount, @DeliveryNetAmount, 'Auto Posting', 'System', @ApprovalDate, @VoucherType,
				'Approved', 0, 0, 0, 'Auto Process', @ApprovalDate, @ReturnId2, @TableId, 1
			);

			SET @VoucherMasterId = SCOPE_IDENTITY();

			-- Debit: Finished Goods Inventory (4156)
			DECLARE @DrGl INT = 0;
			SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1016 AND IsInventory = 1;
			IF ISNULL(@DrGl, 0) = 0 SET @DrGl = 4156;

			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks) 
			VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Sales Return COGS Adjustment');

			-- Credit: Cost of Goods Sold (4162)
			DECLARE @CrGl INT = 0;
			SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1021 AND IsInventory = 1;
			IF ISNULL(@CrGl, 0) = 0 SET @CrGl = 4162;

			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, RefInfoInd, Remarks)
			VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, @DeliveryNetAmount, 0, 0, 0, 'Sales Return COGS Adjustment');

			FETCH NEXT FROM @MyCursor2 INTO @BiusinessUnitId, @FinancialYearId, @InvoiceNo, @ApprovalDate, @DeliveryNetAmount, @TableId, @ReturnId2, @CustomerName;
		END;

		CLOSE @MyCursor2;
		DEALLOCATE @MyCursor2;
	END;
END;
```

---

## `sp_Sales_DeliverySalesReturnPosting`

**Parameters:**
- `@ReturnId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblCustomerReturnDetail, tblCustomerReturnMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tbl_PTP_AdavnecPayToSupplierMaster
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblCustomerReturnDetail, tblCustomerReturnMaster, tblDCStore, tblInvoice, tblInvoiceDetail, tbl_PTP_AdavnecPayToSupplierMaster, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:144` — `return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesReturnPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Sales_DeliverySalesReturnPosting] 

	@ReturnId INT

AS
BEGIN

	--SELECT * FROM tblCustomerReturnMaster AS RTNM
	--LEFT JOIN dbo.tblCustomerReturnDetail AS RTND ON RTND.CustReturnId = RTNM.CustReturnId
	--LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceDetailId = RTND.InvoiceDetailId
	--WHERE RTNM.CustReturnId = 8

	-- Approve or Reject

	DECLARE @ApprovalStatus NVARCHAR(200) = NULL
	DECLARE @VoucherMasterIdTemp INT = 0

	SELECT @ApprovalStatus = R.ActionStatus, @VoucherMasterIdTemp =ISNULL( V.VoucherMasterId,0) FROM tblCustomerReturnMaster AS R
	LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2018 AND R.CustReturnId = V.CommonPK_ID
	WHERE CustReturnId = @ReturnId  -- 1793

	--PRINT @ApprovalStatus
	--PRINT @VoucherMasterIdTemp

	-- Insert 

	IF(LTRIM(RTRIM(UPPER(@ApprovalStatus))) = LTRIM(RTRIM(UPPER('Accepted'))) AND @VoucherMasterIdTemp = 0 )
	BEGIN

		-- Journal Posting 

		DECLARE @VoucherMasterId INT
		DECLARE @VoucherCode NVARCHAR(MAX)
		DECLARE @BiusinessUnitId INT
		--DECLARE @SupplierId INT
		--DECLARE @BankAccountId INT
		--DECLARE @CashAccountId INT
		DECLARE @FinancialYearId INT
 --		  DECLARE @DateOfPayment DATETIME
		DECLARE @CurrencyId INT = 1
		DECLARE @Conversionrate DECIMAL(18,4) = 1.00
 --		  DECLARE @Narration NVARCHAR(MAX)
 --		  DECLARE @TotalPayAmountBDT DECIMAL(18,4)
 --		  DECLARE @TotalAmount DECIMAL(18,4)
 --		  DECLARE @VoucherCategory INT
 --		  DECLARE @ChecqueNo  NVARCHAR(MAX)
 --		  DECLARE @ChecqueDate DATETIME
 --		  DECLARE @ReferenceNo NVARCHAR(MAX)
 --		  DECLARE @ReferenceDate DATETIME
		--DECLARE @PurchaseOrderLocalCode2 NVARCHAR(500)
		DECLARE @VoucherType NVARCHAR(200) ='Journal'
		DECLARE @ApprovalDate DATETIME
		DECLARE @ApproveBy NVARCHAR(200)
		DECLARE @TableId INT
		DECLARE @InvoiceNo  NVARCHAR(200)
		DECLARE @MaterialConversionDate DATETIME
		DECLARE @DeliveryNetAmount  DECIMAL(18,4)
		DECLARE @ReturnAmount  DECIMAL(18,4)

		

		DECLARE @TotalPaid  DECIMAL(18,4)
		DECLARE @Due DECIMAL(18,4)
		DECLARE @SalesAdjustment DECIMAL(18,4)
		DECLARE @AccountsPayable DECIMAL(18,4)

		DECLARE @InvoiceId2 INT
		DECLARE @VoucherMasterId2 INT
		DECLARE @CustomerName NVARCHAR(500)

		-- For Credit Amount

		DECLARE @ToatlAdvanceAmountFc DECIMAL(18,4) = 0

		DECLARE @SubsidiaryInfoId INT
		DECLARE @ReferenceInfoId INT
		        
		--------------------------------------------------------
		DECLARE @MyCursor2 CURSOR
		SET @MyCursor2 = CURSOR FAST_FORWARD
		FOR
		---------------


		--SELECT * FROM tblCustomerReturnMaster


		--SELECT INV.InvoiceId,SUM(ISNULL(DeliveryNetAmount,0)) DeliveryNetAmount, (ISNULL(INV.PaymentAmount,0) + ISNULL(INV.DiscountOnPayment,0) + ISNULL(INV.AIT,0)) AS Totalpaid FROM tblInvoice AS INV
		--LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		--WHERE INV.InvoiceId = 540576 
		--GROUP BY INV.InvoiceId,INV.PaymentAmount,INV.AIT,INV.DiscountOnPayment



		SELECT DISTINCT CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,FORMAT(RTNM.ApprovedDate, 'dd-MMM-yyyy') UpdateDate,
		IVD.DeliveryNetAmount,ISNULL(IVD.TotalPaid,0) TotalPaid , (IVD.DeliveryNetAmount - ISNULL(IVD.TotalPaid,0)) AS Due,

		CASE WHEN (IVD.DeliveryNetAmount - ISNULL(IVD.TotalPaid,0)) >=  SUM(ISNULL(RTND.ReturnAmount,0)) THEN SUM(ISNULL(RTND.ReturnAmount,0)) 
		ELSE (IVD.DeliveryNetAmount - ISNULL(IVD.TotalPaid,0)) END AS SalesAdjustment, 

		CASE WHEN (IVD.DeliveryNetAmount - ISNULL(IVD.TotalPaid,0)) >=  SUM(ISNULL(RTND.ReturnAmount,0)) THEN 0 
		ELSE  SUM(ISNULL(RTND.ReturnAmount,0)) - (IVD.DeliveryNetAmount - ISNULL(IVD.TotalPaid,0)) END AS AccountsPayable,

		SUM(ISNULL(RTND.ReturnAmount,0)) ReturnAmount,TBL.IntegrationTableId,INV.InvoiceId, 
		CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' END CustomerName,V.VoucherMasterId,
		ISNULL(SB.SubsidiaryInfoId,0) SubsidiaryInfoId
		,ISNULL(RF.ReferenceInfoId,0) ReferenceInfoId 
		FROM tblCustomerReturnMaster AS RTNM
		LEFT JOIN dbo.tblCustomerReturnDetail AS RTND ON RTND.CustReturnId = RTNM.CustReturnId
		LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceDetailId = RTND.InvoiceDetailId
		LEFT JOIN dbo.tblInvoice AS INV ON INVD.InvoiceId = INV.InvoiceId

		LEFT JOIN (SELECT INV.InvoiceId,SUM(ISNULL(DeliveryNetAmount,0)) DeliveryNetAmount, (ISNULL(INV.PaymentAmount,0) + ISNULL(INV.DiscountOnPayment,0) + ISNULL(INV.AIT,0)) AS Totalpaid FROM tblInvoice AS INV
		LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		--WHERE INV.InvoiceId = 540576 
		GROUP BY INV.InvoiceId,INV.PaymentAmount,INV.AIT,INV.DiscountOnPayment) AS IVD ON INV.InvoiceId = IVD.InvoiceId

		LEFT JOIN tblDCStore AS DCS ON DCS.DCStoreId = INVD.DCStoreId
		INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
		LEFT JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
		LEFT JOIN tblCustMaster AS CMS ON INV.CustomerMasterId = CMS.CustomerMasterId
		INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		INNER JOIN (SELECT CompanyId,FinancialYearId,StartDate,EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND RTNM.ApprovedDate BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblCustomerReturnMaster'))) 
		LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2018 AND INV.InvoiceId = V.CommonPK_ID

		-- Subsidary

		LEFT JOIN (SELECT S_Info_Id,SubsidiaryInfoId FROM CSTL_ACCDB..tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1) AS SB ON Inv.CustomerMasterId = SB.S_Info_Id

		-- Reference

		LEFT JOIN (SELECT R_Info_Id,ReferenceInfoId FROM CSTL_ACCDB..tblReferenceInfo WHERE ReferenceInfoId =1016) AS RF ON INV.InvoiceId = RF.R_Info_Id

		WHERE  RTNM.CustReturnId = @ReturnId -- @ReturnId -- 548554
		GROUP BY CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,RTNM.ApprovedDate,IVD.DeliveryNetAmount,IVD.TotalPaid,
		TBL.IntegrationTableId,INV.InvoiceId, CMS.CustomerCode, CustomerName,V.VoucherMasterId,ISNULL(SB.SubsidiaryInfoId,0), ISNULL(RF.ReferenceInfoId,0) 

		
		HAVING SUM(ISNULL(RTND.ReturnAmount,0)) > 0


		--WM.WHStockInMasterID,WHStockInCode,WM.WHStockInDate,TotalQuantity,
		--TotalValue,ChallanNo,ChallanDate,WH.CompanyId
  
		 --SELECT * FROM tbl_PTP_AdavnecPayToSupplierMaster  M.ApprovalStatus = 'Accepted' AND 

		----------
		OPEN @MyCursor2
		FETCH NEXT FROM @MyCursor2                               
		INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,
		@DeliveryNetAmount,@TotalPaid,@Due,@SalesAdjustment,@AccountsPayable,
		@ReturnAmount,@TableId,@InvoiceId2,@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId, @ReferenceInfoId
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

		--SELECT * FROM CSTL_ACCDB..tblVoucherCategory

		 --- Master Information Posting

		 DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)   
		 DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
		 DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
		 SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
		 WHERE PostingDate = @ApprovalDate
		
			IF(@VoucherType='DebitVoucherBank')
			BEGIN
				SET @VoucherCode='DVB-'+@VoucherCode	
			END

			IF(@VoucherType='DebitVoucherCash')
			BEGIN
				SET @VoucherCode='DVC-'+@VoucherCode
			END

			IF(@VoucherType='CreditVoucherBank')
			BEGIN
				SET @VoucherCode='CVB-'+@VoucherCode	
			END

			IF(@VoucherType='CreditVoucherCash')
			BEGIN
				SET @VoucherCode='CVC-'+@VoucherCode
			END

			IF(@VoucherType='FundTransfer')
			BEGIN
				SET @VoucherCode='FUT-'+@VoucherCode
			END

			IF(@VoucherType='Journal')
			BEGIN
				SET @VoucherCode='JVP-'+@VoucherCode
			END

			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
			        ( VoucherCode ,
			          CompanyInfoId , 
			          FinancialYearId ,
			          PostingDate ,
			          VoucherCategoryID ,
			          CurrencyId ,
			          CurrencyCovRate ,
			          Narration ,        
			          PaytoIsSupplier ,
			          IsInvoice ,
			          TotalAmountBDT ,
			          TotalAmount ,
			          Remarks ,
			          EntryBy ,
			          EntryDate ,
			          VoucherType ,
			          ActionStatus,
					  IsCostCenter,
					  IsBuyerCostCenter,
					  IsOtherCostCenter,
					  ApprovedBy,
					  ApprovedDate,
					  CommonPK_ID,
					  TableID
			        )
			VALUES  ( 
					  @VoucherCode ,
			          @BiusinessUnitId , 
			          @FinancialYearId ,
			          @ApprovalDate ,
			          2 ,
			          @CurrencyId ,
			          @Conversionrate ,
					 'An amount of BDT ' + CONVERT(NVARCHAR(50), @ReturnAmount) + 
					 ' has been returned by customer "' + @CustomerName + 
					 '" against Invoice No. "' + @InvoiceNo + 
					 '" on ' + CONVERT(NVARCHAR(50), @ApprovalDate, 105) + '.',     
			          0 ,
			          0 ,
			          @DeliveryNetAmount ,
			          @DeliveryNetAmount ,
			          'Auto Posting' ,
			          @ApproveBy ,
			          @ApprovalDate ,
			          @VoucherType ,
			          'Approved',
					  0,0,0,'Auto Process',
					  @ApprovalDate,@InvoiceId2,@TableId
			        )
			
			SET @VoucherMasterId = SCOPE_IDENTITY()



			-- Detail Information Posting

			DECLARE	@DrGl INT = 0 
			SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1020 -- Sales Return

			-- PRINT @DrGl

			-- Debit Side

			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks,RefInfoInd, SubsidoryLedgerId) 
			VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @ReturnAmount, 0.00, 'Sales Return', @ReferenceInfoId, @SubsidiaryInfoId);

			-- Credit Side

			IF (@SalesAdjustment > 0)
			BEGIN

				DECLARE	@CrGl INT = 0 
				SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 2 -- Accounts Receivable
				--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

				--PRINT @CrGl
				
				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount,  Remarks)
				VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, @SalesAdjustment, 0, 0,'Sales Return');

			END


			IF (@AccountsPayable > 0)
			BEGIN

				-- Credit Side

				DECLARE	@CrGl2 INT = 0 
				SELECT @CrGl2 = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 23 AND FlagId = 1 -- Accounts Payable
				--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

				--PRINT @CrGl
				
				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount,  Remarks)
				VALUES (@VoucherMasterId, @CrGl2, 'Credit', 0, @AccountsPayable, 0, 0,'Sales Return');
		

			END

			
		
		FETCH NEXT FROM @MyCursor2
		INTO  @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,
		@DeliveryNetAmount,@TotalPaid,@Due,@SalesAdjustment,@AccountsPayable,
		@ReturnAmount,@TableId,@InvoiceId2,@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId, @ReferenceInfoId

		
		END
		CLOSE @MyCursor2
		DEALLOCATE @MyCursor2

		-- COGS Posting

		EXEC sp_Sales_DeliverySalesReturnCOGSPosting  @ReturnId = @ReturnId; 


	END -- End of If
	
	
	
END
```

---

## `sp_Sales_DeliverySalesReversePosting`

**Parameters:**
- `@DelivaryInvoiceNo` — nvarchar(500), input
- `@userid` — nvarchar(500), input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblDCStore, tblInvoice, tblInvoiceDetail, tbl_PTP_AdavnecPayToSupplierMaster
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblReferenceInfo, CSTL_ACCDB..tblSubsidiaryInfo, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCentralStore, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblDCStore, tblInvoice, tblInvoiceDetail, tbl_PTP_AdavnecPayToSupplierMaster, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DelivaryInvoiceNo FROM tblCustPayDetail AS CP LEFT JOIN tblInvoice AS I ON CP.InvoiceId = I.InvoiceId WHERE LTRIM(RTRIM(UPPER(DelivaryInvoiceNo))) = LTRIM(RTRIM(UPPER(@DelivaryInvoiceNo)))) BEGIN DECLARE @InvoiceId INT = 0 DECLARE @DeliveyInvoiceStatus NVARCHAR(200) = NULL SELECT @InvoiceId = InvoiceId, @DeliveyInvoiceStatus =`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Sales_DeliverySalesReversePosting] 

	@DelivaryInvoiceNo nvarchar(500),
    @userid nvarchar(500)

AS
BEGIN

	--SELECT * FROM tblInvoice AS INV
	--LEFT JOIN tblInvoiceDetail AS IVD ON INV.InvoiceId = IVD.InvoiceId
	--LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	--WHERE CompanyId = 16 AND INV.InvoiceId = 509565


	IF NOT EXISTS(SELECT DelivaryInvoiceNo FROM tblCustPayDetail AS CP
	LEFT JOIN tblInvoice AS I ON CP.InvoiceId = I.InvoiceId WHERE LTRIM(RTRIM(UPPER(DelivaryInvoiceNo))) = LTRIM(RTRIM(UPPER(@DelivaryInvoiceNo))))	
	BEGIN

		DECLARE @InvoiceId INT = 0
		DECLARE @DeliveyInvoiceStatus NVARCHAR(200) = NULL

		-- Approve or Reject

		SELECT @InvoiceId = InvoiceId, @DeliveyInvoiceStatus = DeliveryInvoiceStatus 
		FROM tblInvoice WHERE  LTRIM(RTRIM(UPPER(DelivaryInvoiceNo))) = LTRIM(RTRIM(UPPER(@DelivaryInvoiceNo)))

		-- Insert 

		IF(LTRIM(RTRIM(UPPER(@DeliveyInvoiceStatus))) = LTRIM(RTRIM(UPPER('Full'))) OR LTRIM(RTRIM(UPPER(@DeliveyInvoiceStatus))) = LTRIM(RTRIM(UPPER('Partial'))))
		BEGIN

			-- Journal Posting 

			DECLARE @VoucherMasterId INT
			DECLARE @VoucherCode NVARCHAR(MAX)
			DECLARE @BiusinessUnitId INT
			--DECLARE @SupplierId INT
			--DECLARE @BankAccountId INT
			--DECLARE @CashAccountId INT
			DECLARE @FinancialYearId INT
 --			  DECLARE @DateOfPayment DATETIME
			DECLARE @CurrencyId INT = 1
			DECLARE @Conversionrate DECIMAL(18,4) = 1.00
 --			  DECLARE @Narration NVARCHAR(MAX)
 --			  DECLARE @TotalPayAmountBDT DECIMAL(18,4)
 --			  DECLARE @TotalAmount DECIMAL(18,4)
 --			  DECLARE @VoucherCategory INT
 --			  DECLARE @ChecqueNo  NVARCHAR(MAX)
 --			  DECLARE @ChecqueDate DATETIME
 --			  DECLARE @ReferenceNo NVARCHAR(MAX)
 --			  DECLARE @ReferenceDate DATETIME
			--DECLARE @PurchaseOrderLocalCode2 NVARCHAR(500)
			DECLARE @VoucherType NVARCHAR(200) ='Journal'
			DECLARE @ApprovalDate DATETIME
			DECLARE @ApproveBy NVARCHAR(200)
			DECLARE @TableId INT
			DECLARE @InvoiceNo  NVARCHAR(200)
			DECLARE @MaterialConversionDate DATETIME
			DECLARE @DeliveryNetAmount  DECIMAL(18,4)
			DECLARE @InvoiceId2 INT
			DECLARE @VoucherMasterId2 INT
			DECLARE @CustomerName NVARCHAR(500)
			DECLARE @SubsidiaryInfoId INT
			DECLARE @ReferenceInfoId INT

			-- For Credit Amount

			DECLARE @ToatlAdvanceAmountFc DECIMAL(18,4) = 0
			        
			--------------------------------------------------------
			DECLARE @MyCursor2 CURSOR
			SET @MyCursor2 = CURSOR FAST_FORWARD
			FOR
			---------------

			--SELECT * FROM tblInvoice

				SELECT CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,FORMAT(INV.UpdateDate, 'dd-MMM-yyyy') UpdateDate,
				SUM(ISNULL(INVD.DeliveryNetAmount,0)) DeliveryNetAmount,TBL.IntegrationTableId,INV.InvoiceId, 
				CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' END CustomerName,V.VoucherMasterId,
				ISNULL(SB.SubsidiaryInfoId,0) SubsidiaryInfoId,ISNULL(RF.ReferenceInfoId,0) ReferenceInfoId
				FROM tblInvoice AS INV WITH(NOLOCK)
				LEFT JOIN tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
				LEFT JOIN tblCustMaster AS CMS ON INV.CustomerMasterId = CMS.CustomerMasterId
				INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = INVD.DCStoreId
				INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
				INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
				INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId

				-- Subsidary

				LEFT JOIN (SELECT S_Info_Id,SubsidiaryInfoId FROM CSTL_ACCDB..tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1) AS SB ON INV.CustomerMasterId = SB.S_Info_Id

				-- Reference

				LEFT JOIN CSTL_ACCDB..tblReferenceInfo AS RF ON RF.ReferenceTypeId = 1016 -- Invoice
				AND R_Info_Id = @InvoiceId 

				--LTRIM(RTRIM(UPPER(INV.InvoiceNo))) = LTRIM(RTRIM(UPPER(RF.Description))) 

				-- Table

				INNER JOIN (SELECT CompanyId,FinancialYearId,StartDate,EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND INV.UpdateDate BETWEEN FY.StartDate AND FY.EndDate
				LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblInvoice'))) 
				LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2014 AND INV.InvoiceId = V.CommonPK_ID
				WHERE 
				
				--ISNULL(V.VoucherMasterId,0) = 0 AND 
				
				INV.InvoiceId = @InvoiceId -- 548554
				AND UNT.CompanyId = 16 -- Sales & Marketing
				GROUP BY CI.CompanyId,FY.FinancialYearId,INV.InvoiceNo,INV.UpdateDate,TBL.IntegrationTableId,
				INV.InvoiceId, CMS.CustomerCode, CustomerName,V.VoucherMasterId,SB.SubsidiaryInfoId,RF.ReferenceInfoId
				HAVING SUM(ISNULL(INVD.DeliveryNetAmount,0)) > 0


			--WM.WHStockInMasterID,WHStockInCode,WM.WHStockInDate,TotalQuantity,
			--TotalValue,ChallanNo,ChallanDate,WH.CompanyId
  
			 --SELECT * FROM tbl_PTP_AdavnecPayToSupplierMaster  M.ApprovalStatus = 'Accepted' AND 

			----------
			OPEN @MyCursor2
			FETCH NEXT FROM @MyCursor2                               
			INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,
			@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId,@ReferenceInfoId
			
			WHILE @@FETCH_STATUS = 0
			BEGIN

			--SELECT * FROM CSTL_ACCDB..tblVoucherCategory

			 --- Master Information Posting

			 DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)   
			 DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
			 DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
			 SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
			 WHERE PostingDate = @ApprovalDate
			
				IF(@VoucherType='DebitVoucherBank')
				BEGIN
					SET @VoucherCode='DVB-'+@VoucherCode	
				END

				IF(@VoucherType='DebitVoucherCash')
				BEGIN
					SET @VoucherCode='DVC-'+@VoucherCode
				END

				IF(@VoucherType='CreditVoucherBank')
				BEGIN
					SET @VoucherCode='CVB-'+@VoucherCode	
				END

				IF(@VoucherType='CreditVoucherCash')
				BEGIN
					SET @VoucherCode='CVC-'+@VoucherCode
				END

				IF(@VoucherType='FundTransfer')
				BEGIN
					SET @VoucherCode='FUT-'+@VoucherCode
				END

				IF(@VoucherType='Journal')
				BEGIN
					SET @VoucherCode='JVP-'+@VoucherCode
				END

				
				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
				        ( VoucherCode ,
				          CompanyInfoId , 
				          FinancialYearId ,
				          PostingDate ,
				          VoucherCategoryID ,
				          CurrencyId ,
				          CurrencyCovRate ,
				          Narration ,        
				          PaytoIsSupplier ,
				          IsInvoice ,
				          TotalAmountBDT ,
				          TotalAmount ,
				          Remarks ,
				          EntryBy ,
				          EntryDate ,
				          VoucherType ,
				          ActionStatus,
						  IsCostCenter,
						  IsBuyerCostCenter,
						  IsOtherCostCenter,
						  ApprovedBy,
						  ApprovedDate,
						  CommonPK_ID,
						  TableID
				        )
				VALUES  ( 
						  @VoucherCode ,
				          @BiusinessUnitId , 
				          @FinancialYearId ,
				          @ApprovalDate ,
				          2 ,
				          @CurrencyId ,
				          @Conversionrate ,
						 'Accounts Receivable has been reversed because Sale to ' + @CustomerName + ' under invoice ' + @InvoiceNo + ' for BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + ' has been deleted.',    
				          0 ,
				          0 ,
				          @DeliveryNetAmount ,
				          @DeliveryNetAmount ,
				          'Auto Posting' ,
				          @ApproveBy ,
				          @ApprovalDate ,
				          @VoucherType ,
				          'Approved',
						  0,0,0,'Auto Process',
						  @ApprovalDate,@InvoiceId2,@TableId
				        )
				
				SET @VoucherMasterId = SCOPE_IDENTITY()

				-- Detail Information Posting

				DECLARE	@DrGl INT = 0 
				SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1020 -- Sales

				--PRINT @DrGl

				-- Debit Side

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit,
				DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks, SubsidoryLedgerId,RefInfoInd) 
				VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Sales', @SubsidiaryInfoId, @ReferenceInfoId);

				-- Credit Side

				DECLARE	@CrGl INT = 0 
				SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 2  -- Accounts Receivable 
				--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

				--PRINT @CrGl
				
				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, RefInfoInd, Remarks)
				VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, @DeliveryNetAmount, 0, 0, 0, 'Sales');

			
			
			FETCH NEXT FROM @MyCursor2
			INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,
			@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId,@ReferenceInfoId

			
			END
			CLOSE @MyCursor2
			DEALLOCATE @MyCursor2

			END -- End Of Cursor
			
		---- COGS Posting

		EXEC sp_Sales_DeliveryCOGSReversePosting  @InvoiceId = @InvoiceId; 


	END -- End of Exist Check

END
```

---

## `sp_Sales_ExecuteDeliveryPipeline`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure orchestrator/pipeline procedure — no tables touched directly. It sequentially calls `sp_Process_WeightedAverageSalesConfirmation`, `sp_Sales_DeliveryCOGSPosting` (each executed as a nested stored procedure) to run the full delivery-confirmation pipeline in one call.

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1675` — `return aCommonInternalDal.UpdateAction("sp_Sales_ExecuteDeliveryPipeline", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =======================================================================================
-- 6. sp_Sales_ExecuteDeliveryPipeline (Master Pipeline for Delivery Confirmation)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Sales_ExecuteDeliveryPipeline]
    @InvoiceId INT
AS
BEGIN
    SET NOCOUNT ON;

    EXEC dbo.sp_Process_WeightedAverageSalesConfirmation @InvoiceId = @InvoiceId;
    EXEC dbo.sp_Sales_DeliveryCOGSPosting @InvoiceId = @InvoiceId;
END;
```

---

## `sp_Sales_ProductDestroyPosting`

**Parameters:**
- `@DestroyId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, tblCompanyUnit, tblDCStore, tblDCStoreFreeze, tblDestroyProduct, tblProduct, tblUnitPrice
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, tblCompanyUnit, tblDCStore, tblDCStoreFreeze, tblDestroyProduct, tblProduct, tblUnitPrice, then inserts into `CSTL_ACCDB..tblDebitCreditVoucherDetail` (columns: VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks); inserts into `CSTL_ACCDB..tblDebitCreditVoucherMaster` (columns: VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,...).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK) WHERE CommonPK_ID = @DestroyId AND TableID = 144 AND ActionStatus <> 'Cancelled' ) BEGIN DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@DestroyDate)), 3, 2); DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CA`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/DestroyProductDal.cs:233` — `return aCommonInternalDal.UpdateAction("sp_Sales_ProductDestroyPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =======================================================================================
-- 7. sp_Sales_ProductDestroyPosting (DR Destroy Expense 4164 / CR Finished Goods FG 4156)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Sales_ProductDestroyPosting]
    @DestroyId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT, @FinancialYearId INT, @DestroyDate DATETIME;
    DECLARE @TotalDestroyCost DECIMAL(18,4) = 0;

    SELECT 
        @CompanyId = CU.CompanyId,
        @DestroyDate = ISNULL(DP.ApproveDate, DP.StockOutDate)
    FROM dbo.tblDestroyProduct DP WITH (NOLOCK)
    INNER JOIN dbo.tblDCStoreFreeze DCF WITH (NOLOCK) ON DP.DcFreezeId = DCF.DCStoreFreezeId
    INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON DCF.ComUnitId = CU.ComUnitId
    WHERE DP.DestroyId = @DestroyId;

    SELECT TOP 1 @FinancialYearId = FinancialYearId 
    FROM CSTL_ACCDB..tblFinancialYear WITH (NOLOCK)
    WHERE CompanyId = @CompanyId AND @DestroyDate BETWEEN StartDate AND EndDate;

    SELECT 
        @TotalDestroyCost = ISNULL(SUM(CAST(DP.StockOutQty AS DECIMAL(18,4)) * ISNULL(DCS.UnitPrice_CostPrice, ISNULL(UP.UnitPrice, 0))), 0)
    FROM dbo.tblDestroyProduct DP WITH (NOLOCK)
    INNER JOIN dbo.tblDCStoreFreeze DCF WITH (NOLOCK) ON DP.DcFreezeId = DCF.DCStoreFreezeId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK) ON DCF.DCStoreId = DCS.DCStoreId
    LEFT JOIN dbo.tblProduct P WITH (NOLOCK) ON DCF.ProductCode = P.ProductCode
    LEFT JOIN dbo.tblUnitPrice UP WITH (NOLOCK) ON P.ProductId = UP.ProductId AND UP.IsActive = 1
    WHERE DP.DestroyId = @DestroyId;

    IF (@TotalDestroyCost > 0)
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK)
            WHERE CommonPK_ID = @DestroyId AND TableID = 144 AND ActionStatus <> 'Cancelled'
        )
        BEGIN
            DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@DestroyDate)), 3, 2);
            DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CAST(MONTH(@DestroyDate) AS NVARCHAR(2)), 2);
            DECLARE @dateText NVARCHAR(10) = RIGHT('0' + CAST(DAY(@DestroyDate) AS NVARCHAR(2)), 2);

            DECLARE @MaxNum INT = 10000;
            SELECT @MaxNum = ISNULL(MAX(VoucherMasterId), 10000) FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK);

            DECLARE @VoucherCode NVARCHAR(50) = 'JVP-' + @yearText + @monthText + @dateText + CAST((@MaxNum % 89999) + 10001 AS NVARCHAR(10));

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
            (
                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, CurrencyId, CurrencyCovRate,
                Narration, PaytoIsSupplier, IsInvoice, TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
                ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy, ApprovedDate, CommonPK_ID, TableID
            )
            VALUES
            (
                @VoucherCode, @CompanyId, @FinancialYearId, @DestroyDate, 2, 1, 1.00,
                'Cost of Goods Destroyed for Destroy ID: ' + CAST(@DestroyId AS NVARCHAR(20)) + '.',
                0, 0, @TotalDestroyCost, @TotalDestroyCost, 'Auto Destroy Posting', 'System', @DestroyDate, 'Journal',
                'Approved', 0, 0, 0, 'Auto Process', @DestroyDate, @DestroyId, 144
            );

            DECLARE @VoucherMasterId INT = SCOPE_IDENTITY();

            -- DR: Cost of Goods Destroyed (GL 4164)
            DECLARE @DrGl INT = 0;
            SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 144 AND FlagId = 1022 AND IsInventory = 1;
            IF ISNULL(@DrGl, 0) = 0 SET @DrGl = 4164;

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @TotalDestroyCost, 0.00, 'Cost of Goods Destroy');

            -- CR: Finished Goods Inventory (4156)
            DECLARE @CrGl INT = 0;
            SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 144 AND FlagId = 1014 AND IsInventory = 1;
            IF ISNULL(@CrGl, 0) = 0 SET @CrGl = 4156;

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks)
            VALUES (@VoucherMasterId, @CrGl, 'Credit', 0.00, @TotalDestroyCost, 0.00, 0.00, 'Finished Goods Stock Out');
        END;
    END;
END;
```

---

## `sp_Sales_SampleCOGSPosting`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCompanyUnit, tblCustMaster, tblDCStore, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, tblCompanyUnit, tblCustMaster, tblDCStore, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation, and per row inserts into CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =======================================================================================
-- 11. sp_Sales_SampleCOGSPosting (Sample COGS Posting: DR 4164 / CR 4156)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Sales_SampleCOGSPosting] 
	@InvoiceId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ApprovalStatus NVARCHAR(200) = NULL;
	SELECT @ApprovalStatus = ActionStatus FROM tblSampleIssue WHERE OrderId = @InvoiceId;

	IF(LTRIM(RTRIM(UPPER(@ApprovalStatus))) IN ('ACCEPTED', 'APPROVED'))
	BEGIN
		DECLARE @VoucherMasterId INT, @VoucherCode NVARCHAR(MAX), @BiusinessUnitId INT, @FinancialYearId INT;
		DECLARE @CurrencyId INT = 1, @Conversionrate DECIMAL(18,4) = 1.00, @VoucherType NVARCHAR(200) ='Journal';
		DECLARE @ApprovalDate DATETIME, @ApproveBy NVARCHAR(200), @TableId INT, @DeliveryNetAmount DECIMAL(18,4);
		DECLARE @InvoiceNo NVARCHAR(200), @InvoiceId2 INT, @CustomerName NVARCHAR(500);
		        
		DECLARE @MyCursor2 CURSOR;
		SET @MyCursor2 = CURSOR FAST_FORWARD FOR
		SELECT 
			CI.CompanyId,
			FY.FinancialYearId,
			SI.OrderCode,
			FORMAT(SI.ApprovedDate, 'dd-MMM-yyyy') AS UpdateDate,
			SUM(ISNULL(SIT.Quantity * -1, 0) * ISNULL(DCS.UnitPrice_CostPrice, 0)) AS CostAmount,
			TBL.IntegrationTableId,
			SI.OrderId, 
			CMS.CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' ELSE '' END AS CustomerName
		FROM tblSampleIssue AS SI
		LEFT JOIN tblSampleIssueDetail AS SD ON SI.OrderId = SD.OrderId
		LEFT JOIN tblSampleIssueTranscation AS SIT ON SIT.IssueId = SI.OrderId AND IssueDetailId = SD.OrderDetailId
		INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = SIT.DCStoreId
		INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = SI.ComUnitId
		INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		LEFT JOIN tblCustMaster AS CMS ON SI.CustomerMasterId = CMS.CustomerMasterId
		INNER JOIN (SELECT CompanyId, FinancialYearId, StartDate, EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY 
		       ON CI.CompanyId = FY.CompanyId AND SI.ApprovedDate BETWEEN FY.StartDate AND FY.EndDate
		LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = 'TBLSAMPLEISSUE' 	
		LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2015 AND SI.OrderId = V.CommonPK_ID AND V.ActionStatus <> 'Cancelled'
		WHERE ISNULL(V.VoucherMasterId, 0) = 0 AND SI.OrderId = @InvoiceId
		GROUP BY CI.CompanyId, FY.FinancialYearId, SI.OrderCode, SI.ApprovedDate, TBL.IntegrationTableId, SI.OrderId, CMS.CustomerCode, CMS.CustomerName
		HAVING SUM(ISNULL(SIT.Quantity * -1, 0) * ISNULL(DCS.UnitPrice_CostPrice, 0)) > 0;

		OPEN @MyCursor2;
		FETCH NEXT FROM @MyCursor2 INTO @BiusinessUnitId, @FinancialYearId, @InvoiceNo, @ApprovalDate, @DeliveryNetAmount, @TableId, @InvoiceId2, @CustomerName;
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@ApprovalDate)), 3, 2);
			DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CAST(MONTH(@ApprovalDate) AS NVARCHAR(2)), 2);
			DECLARE @dateText NVARCHAR(10) = RIGHT('0' + CAST(DAY(@ApprovalDate) AS NVARCHAR(2)), 2);
			
			DECLARE @MaxNum INT = 10000;
			SELECT @MaxNum = ISNULL(MAX(VoucherMasterId), 10000) FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK);

			SET @VoucherCode = 'JVP-' + @yearText + @monthText + @dateText + CAST((@MaxNum % 89999) + 10001 AS NVARCHAR(10));
			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
			(
				VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, CurrencyId, CurrencyCovRate,
				Narration, PaytoIsSupplier, IsInvoice, TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
				ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy, ApprovedDate, CommonPK_ID, TableID
			)
			VALUES
			(
				@VoucherCode, @BiusinessUnitId, @FinancialYearId, @ApprovalDate, 2, @CurrencyId, @Conversionrate,
				'An expense of BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) + ' has been recorded for the Sample Expense to ' + @CustomerName + ' under Sample Invoice No ' + @InvoiceNo + '.',    
				0, 0, @DeliveryNetAmount, @DeliveryNetAmount, 'Auto Posting', 'System', @ApprovalDate, @VoucherType,
				'Approved', 0, 0, 0, 'Auto Process', @ApprovalDate, @InvoiceId2, @TableId
			);
			
			SET @VoucherMasterId = SCOPE_IDENTITY();

			-- DR: Sample Expense (4164)
			DECLARE @DrGl INT = 0;
			SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 160 AND FlagId = 1022 AND IsInventory = 1;
			IF ISNULL(@DrGl, 0) = 0 SET @DrGl = 4164;

			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks) 
			VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Sample Expense');

			-- CR: Finished Goods Inventory (4156)
			DECLARE @CrGl INT = 0;
			SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 160 AND FlagId = 1014 AND IsInventory = 1;
			IF ISNULL(@CrGl, 0) = 0 SET @CrGl = 4156;
			
			INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, RefInfoInd, Remarks)
			VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, @DeliveryNetAmount, 0, 0, 0, 'Sample Expense Stock Out');

			FETCH NEXT FROM @MyCursor2 INTO @BiusinessUnitId, @FinancialYearId, @InvoiceNo, @ApprovalDate, @DeliveryNetAmount, @TableId, @InvoiceId2, @CustomerName;
		END;
		CLOSE @MyCursor2;
		DEALLOCATE @MyCursor2;
	END;
END;
```

---

## `sp_SalesAPI_doLogin`

**Parameters:**
- `@username` — NVARCHAR(50), input
- `@password` — NVARCHAR(50), input
- `@Imei` — NVARCHAR(max), input

**Tables read (FROM/JOIN):** tblEmpGeneralInfo, tblUser, tbl_AppVersion, tbl_GlobalAccess_User
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Field-force mobile login: looks up the user in `tbl_GlobalAccess_User`/related employee tables by username/password (and IMEI/device info where present) and returns the matching login/employee record, used to authenticate the mobile Sales/DCR app.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT TOP 1 VersionName FROM dbo.tbl_AppVersion WHERE IsActive = 1 ORDER BY Version DESC) AS VersionName FROM tblUser A INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId WHERE A.LoginName = @username AND A.Password = @password AND A.UserStatus = 'Active' END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_SalesAPI_doLogin]
    @username NVARCHAR(50) ,
    @password NVARCHAR(50),
	@Imei NVARCHAR(max)
AS
    BEGIN

	IF(EXISTS(SELECT * FROM tbl_GlobalAccess_User WHERE IMEI = @Imei OR IMEI_Two = @Imei))
	BEGIN
		        SELECT  A.UserId ,
                B.EmpName AS UserName ,
                A.UserType ,
                A.UserCode ,
                A.LoginName ,
                A.Email ,
                A.ContactNo ,
                A.CentralWareHouse ,
                A.IsExpProduct ,
                A.EmpInfoId ,
                A.Password AS UserCo,
				1 AS IsImeiMatched,
				(SELECT TOP 1 VersionName FROM dbo.tbl_AppVersion WHERE IsActive = 1 ORDER BY Version DESC) AS VersionName
        FROM    tblUser A
                INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
        WHERE   A.LoginName = @username
                AND A.Password = @password
                AND A.UserStatus = 'Active'
	END
	ELSE
    BEGIN
    	        SELECT  A.UserId ,
                B.EmpName AS UserName ,
                A.UserType ,
                A.UserCode ,
                A.LoginName ,
                A.Email ,
                A.ContactNo ,
                A.CentralWareHouse ,
                A.IsExpProduct ,
                A.EmpInfoId ,
                A.Password AS UserCo,
				ISNULL((SELECT COUNT(*) AS Dsas FROM dbo.tblUser WHERE IMEI_One = @Imei OR IMEI_Two = @Imei),0)AS IsImeiMatched,
				(SELECT TOP 1 VersionName FROM dbo.tbl_AppVersion WHERE IsActive = 1 ORDER BY Version DESC) AS VersionName



        FROM    tblUser A
                INNER JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
        WHERE   A.LoginName = @username
                AND A.Password = @password
                AND A.UserStatus = 'Active'
    END
	

    END
```

---

## `sp_SalesAPI_doLogin_New`

**Parameters:**
- `@username` — NVARCHAR(50), input
- `@password` — NVARCHAR(50), input
- `@Imei` — NVARCHAR(max), input
- `@DeviceInfo` — NVARCHAR(max), input
- `@OS` — NVARCHAR(max), input
- `@OS_Version` — NVARCHAR(max), input
- `@AppVersion` — NVARCHAR(max), input
- `@Lat` — NVARCHAR(max), input
- `@Long` — NVARCHAR(max), input
- `@SAddress` — NVARCHAR(max), input

**Tables read (FROM/JOIN):** ZASHRIS..tblDesignation, ZASHRIS..tblEmpGeneralInfo, tblASMInfo, tblEmpGeneralInfo, tblMIOInfo, tblRSMInfo, tblRoleType, tblUser, tbl_AppVersion, tbl_Shift, tbl_UserRoleInfo
**Tables written:** UPDATE → tblUser

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Field-force mobile login: looks up the user in `tbl_GlobalAccess_User`/related employee tables by username/password (and IMEI/device info where present) and returns the matching login/employee record, used to authenticate the mobile Sales/DCR app.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT TOP 1 VersionName FROM dbo.tbl_AppVersion WHERE IsActive = 1 ORDER BY Version DESC) AS VersionName, shft.ShiftInTime AS ShiftStartTime, shft.ShiftOutTime AS ShiftEndTime, 1 AS TrackEnable,tblRoleType.RoleTypeId,RoleType,IsApprove,IsForward, @Per permission FROM tblUser A LEFT JOIN dbo.tblEmpGeneralInfo`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_SalesAPI_doLogin_New]
    @username NVARCHAR(50) ,
    @password NVARCHAR(50),
	@Imei NVARCHAR(max),
	@DeviceInfo NVARCHAR(max),
	 
	 
	@OS NVARCHAR(max),
	@OS_Version NVARCHAR(max) ,
	@AppVersion NVARCHAR(max) ,

	@Lat NVARCHAR(max) ,
	@Long NVARCHAR(max) ,
	@SAddress NVARCHAR(max) 






AS
    BEGIN

	DECLARE @islogin BIT=0

	DECLARE @UserId INT=0
	DECLARE @FirstIMEI NVARCHAR(MAX)=NULL
	DECLARE @SecIMEI NVARCHAR(MAX)=NULL
	Declare @Roltp nvarchar(max)=''
 declare @ComId int=0
 declare @EMPId int=0
 declare @Per  nvarchar(max)='3'

 select @Roltp=RoleType, @EMPId=  A.EmpInfoId, @UserId=ISNULL(UserId,0) FROM dbo.tblUser  A
	 LEFT JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 
LEFT join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=B.EmpMasterCode
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE LoginName=@username AND Password=@password  and UserStatus = 'Active' and IsAppsUser=1

				--AND empHR.EmployeeStatus='Active'

				if(@Roltp='Wings Head')
				begin
				set @Per='2'
				end


				
				if(@Roltp='Admin')
				begin
				set @Per='1'
				end

	IF(@FirstIMEI IS NULL AND @SecIMEI IS NULL)
	BEGIN
	    
		UPDATE dbo.tblUser SET LastAccessTime_1=GETDATE(), AppVer_1=@AppVersion , OS_Version_1=@OS_Version, OS_1=@OS, DeviceInfo_1=@DeviceInfo, IMEI_One=@Imei WHERE UserId=@UserId
		SET @islogin=1
	END
	ELSE IF(@FirstIMEI IS NOT NULL AND @SecIMEI IS NOT NULL)
	BEGIN
	    IF(@FirstIMEI=@Imei OR @SecIMEI=@Imei)
		BEGIN
		    SET @islogin=1
		END
		ELSE
        BEGIN
            SET @islogin=0
        END

	END
	ELSE IF(@FirstIMEI IS NULL AND  @SecIMEI IS NOT NULL)
	BEGIN
	    IF(@SecIMEI=@Imei)
		BEGIN
		    SET @islogin=1
		END
		ELSE
        BEGIN
            UPDATE dbo.tblUser SET LastAccessTime_1=GETDATE(), AppVer_1=@AppVersion , OS_Version_1=@OS_Version, OS_1=@OS,  DeviceInfo_1=@DeviceInfo,IMEI_One=@Imei WHERE UserId=@UserId
			SET @islogin=1
        END
	END
	ELSE IF(@FirstIMEI IS NOT NULL AND  @SecIMEI IS NULL)
	BEGIN
	    IF(@FirstIMEI=@Imei)
		BEGIN
		    SET @islogin=1
		END
		ELSE
        BEGIN
            UPDATE dbo.tblUser SET  LastAccessTime_2=GETDATE(), AppVer_2=@AppVersion , OS_Version_2=@OS_Version,OS_2=@OS, DeviceInfo_2=@DeviceInfo, IMEI_Two=@Imei WHERE UserId=@UserId
			SET @islogin=1
        END
	END
	
	IF(@islogin=0)
	BEGIN
	    SET @UserId=0
	END
	
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


		if(@Roltp='Wings Head')
		begin 
		select @ComId=CompanyId from tblEmpGeneralInfo where EmpInfoId=@EMPId 
		end

  	        SELECT @ComId CompanyId,  case when  ISNULL(A.EmpInfoId,0)=0 then RoleType else    DSG.DesigName end  DesigName, B.EmpMasterCode, A.UserId ,
            case when  ISNULL(A.EmpInfoId,0)=0 then  A.LoginName else     B.EmpName end  AS UserName ,
				RoleType as EmpRole,
                A.UserType ,
                A.UserCode ,
                A.LoginName ,
                A.Email ,
                A.ContactNo ,
                A.CentralWareHouse ,
                ISNULL(A.EmpInfoId,0) EmpInfoId , A.Password,
                A.Password AS UserCo,
				ISNULL((SELECT COUNT(*) AS Dsas FROM dbo.tblUser WHERE IMEI_One = @Imei OR IMEI_Two = @Imei),0)AS IsImeiMatched,
				(SELECT TOP 1 VersionName FROM dbo.tbl_AppVersion WHERE IsActive = 1 ORDER BY Version DESC) AS VersionName,
				shft.ShiftInTime AS ShiftStartTime,
				shft.ShiftOutTime AS ShiftEndTime,
				1 AS TrackEnable,tblRoleType.RoleTypeId,RoleType,IsApprove,IsForward, @Per permission



        FROM    tblUser A
              LEFT JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
                LEFT JOIN ZASHRIS..tblEmpGeneralInfo AS EGI  ON B.EmpMasterCode = EGI.EmpMasterCode

				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId = tbl_UserRoleInfo.RoleTypeId
                LEFT JOIN dbo.tbl_Shift shft ON B.ShiftId = shft.ShiftId
              LEFT JOIN ZASHRIS..tblDesignation AS DSG ON EGI.DesigId = DSG.DesigId
        --WHERE   A.LoginName = @username
        --        AND A.Password = @password
        --        AND A.UserStatus = 'Active'

		WHERE
        A.UserId=@UserId


		if(ISNULL(@UserId,0)>0)
begin
exec [dbo].[sp_Webapi_Save_UserTracking] @empId = @empId,
		@latv = @Lat,
		@longv = @Long,
		@addressName = @SAddress

end
	

    END
```

---

## `sp_SalesAPI_doLoginCheckEMI`

**Parameters:**
- `@username` — NVARCHAR(50), input
- `@password` — NVARCHAR(50), input
- `@Imei` — NVARCHAR(max), input
- `@DeviceInfo` — NVARCHAR(max), input
- `@OS` — NVARCHAR(max), input
- `@OS_Version` — NVARCHAR(max), input
- `@AppVersion` — NVARCHAR(max), input

**Tables read (FROM/JOIN):** tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Field-force mobile login: looks up the user in `tbl_GlobalAccess_User`/related employee tables by username/password (and IMEI/device info where present) and returns the matching login/employee record, used to authenticate the mobile Sales/DCR app.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select * from tblUser WHERE LoginName=@username AND Password=@password AND UserStatus = 'Active' and ((IMEI_One=@Imei) or (IMEI_Two=@Imei)) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_SalesAPI_doLoginCheckEMI]
    @username NVARCHAR(50) ,
    @password NVARCHAR(50),
	@Imei NVARCHAR(max),
	@DeviceInfo NVARCHAR(max),
	 
	 
	@OS NVARCHAR(max),
	@OS_Version NVARCHAR(max) ,
	@AppVersion NVARCHAR(max) 
AS
    BEGIN

 select * from tblUser WHERE LoginName=@username AND Password=@password AND UserStatus = 'Active' and ((IMEI_One=@Imei) or (IMEI_Two=@Imei))
	

    END
```

---

## `sp_SalesAPI_doLoginMatch`

**Parameters:**
- `@username` — NVARCHAR(50), input
- `@password` — NVARCHAR(50), input
- `@Imei` — NVARCHAR(max), input
- `@DeviceInfo` — NVARCHAR(max), input
- `@OS` — NVARCHAR(max), input
- `@OS_Version` — NVARCHAR(max), input
- `@AppVersion` — NVARCHAR(max), input

**Tables read (FROM/JOIN):** tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Field-force mobile login: looks up the user in `tbl_GlobalAccess_User`/related employee tables by username/password (and IMEI/device info where present) and returns the matching login/employee record, used to authenticate the mobile Sales/DCR app.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select * from tblUser WHERE LoginName=@username AND Password=@password AND UserStatus = 'Active' END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_SalesAPI_doLoginMatch]
    @username NVARCHAR(50) ,
    @password NVARCHAR(50),
	@Imei NVARCHAR(max),
	@DeviceInfo NVARCHAR(max),
	 
	 
	@OS NVARCHAR(max),
	@OS_Version NVARCHAR(max) ,
	@AppVersion NVARCHAR(max) 
AS
    BEGIN

 select * from tblUser WHERE LoginName=@username AND Password=@password AND UserStatus = 'Active' 
	

    END
```

---

## `sp_SalesAPI_FieldForceArea`

**Parameters:**
- `@EmpRole` — NVARCHAR(max), input
- `@EmpMasterCode` — NVARCHAR(max), input

**Tables read (FROM/JOIN):** View_FieldForceArea, tblEmpGeneralInfo, tblTagASMRSMInfo, tblTerritory
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceArea, tblEmpGeneralInfo, tblTagASMRSMInfo, tblTerritory, filtered by @empId, @Parrameter.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT ARA.AreaId,ARA.AreaName,ARA.RegionId FROM View_FieldForceArea AS ARA WHERE ARA.AreaId IS NOT NULL ' + @Parrameter EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 



CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceArea]
    
	@EmpRole NVARCHAR(max) ,
    @EmpMasterCode NVARCHAR(max)

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);
  	        DECLARE @TRID NVARCHAR(MAX)='';
  	        DECLARE @empId NVARCHAR(MAX)='';;

			SELECT @empId = EmpInfoId 
FROM tblEmpGeneralInfo 
WHERE EmpMasterCode in (CAST(@EmpMasterCode AS NVARCHAR(MAX))) ;

			if(@EmpRole = 'DZSM')
			BEGIN

				--SET @Parrameter = 'AND RSMCode =  '''+ @EmpMasterCode +'''';

				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')
				--SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';
					SET @Parrameter=@Parrameter+ ' and ARA.AreaId in ( '+ @TRID +')';

			END

			else if(@EmpRole = 'AM')
			BEGIN
			select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')
				--SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';
					SET @Parrameter=@Parrameter+ ' and ARA.AreaId in ( select AreaId from tblTerritory where TerritoryId in ( '+ @TRID +'))';
					--ARA.AreaId in ( select AreaId from tblTerritory where TerritoryId in (630,631,239))
			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode =  '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode =  '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT ARA.AreaId,ARA.AreaName,ARA.RegionId FROM View_FieldForceArea AS ARA WHERE ARA.AreaId IS NOT NULL ' + @Parrameter
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceAsm`

**Parameters:**
- `@EmpRole` — NVARCHAR(50), input
- `@EmpMasterCode` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceAsm, tblEmpGeneralInfo, tblTagASMRSMInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceAsm, tblEmpGeneralInfo, tblTagASMRSMInfo, filtered by @EmpMasterCode.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT ASM.ASMEmpId,ASM.AreaId,ASM.ASMId,ASM.EmpName,ASM.EmpMasterCode FROM View_FieldForceAsm AS ASM WHERE ASM.ASMId IS NOT NULL ' + @Parrameter +' order by ASM.EmpName ' EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 


CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceAsm]
    
	@EmpRole NVARCHAR(50) ,
    @EmpMasterCode NVARCHAR(50)

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);
			 	        DECLARE @TRID NVARCHAR(MAX);

			if(@EmpRole = 'DZSM')
			BEGIN


				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where empMasterCode=@EmpMasterCode) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')

				SET @Parrameter = 'AND ASM.AreaId in ( '+ @TRID +')';
			--	SET @Parrameter = 'AND RSMCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'AM')
			BEGIN

				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where empMasterCode=@EmpMasterCode) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @Parrameter = 'AND ASM.TerritoryId in ( '+ @TRID +')';

			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode = '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT ASM.ASMEmpId,ASM.AreaId,ASM.ASMId,ASM.EmpName,ASM.EmpMasterCode FROM View_FieldForceAsm AS ASM WHERE ASM.ASMId IS NOT NULL ' + @Parrameter +' order by ASM.EmpName ' 
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceGroup`

**Parameters:**
- `@EmpRole` — NVARCHAR(50), input
- `@EmpMasterCode` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceGroup
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceGroup.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT GP.GroupId,GP.GroupName FROM View_FieldForceGroup AS GP WHERE GP.GroupId IS NOT NULL ' + @Parrameter EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 


CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceGroup]
    
	@EmpRole NVARCHAR(50) ,
    @EmpMasterCode NVARCHAR(50)

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);


			if(@EmpRole = 'RSM')
			BEGIN

				SET @Parrameter = 'AND RSMCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'AM')
			BEGIN

				SET @Parrameter = 'AND ASMCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode =  '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT GP.GroupId,GP.GroupName FROM View_FieldForceGroup AS GP WHERE GP.GroupId IS NOT NULL ' + @Parrameter
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceMarket`

**Parameters:**
- `@EmpMasterCode` — NVARCHAR(500), input
- `@EmpRole` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceMarket, tblEmpGeneralInfo, tblTagASMRSMInfo, tblTerritory
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceMarket, tblEmpGeneralInfo, tblTagASMRSMInfo, tblTerritory, filtered by @empId, @Parrameter.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT distinct MKT.MarketId,MKT.MarketName,MKT.TerritoryId FROM View_FieldForceMarket AS MKT with (nolock) WHERE MKT.MarketId IS NOT NULL ' + @Parrameter EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 

CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceMarket]
    
	
    @EmpMasterCode NVARCHAR(500),
	@EmpRole NVARCHAR(50) 

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX); DECLARE @TRID NVARCHAR(MAX)='';
  	        DECLARE @empId NVARCHAR(MAX)='';;

			SELECT @empId = EmpInfoId 
FROM tblEmpGeneralInfo 
WHERE EmpMasterCode in (CAST(@EmpMasterCode AS NVARCHAR(MAX))) ;



			if(@EmpRole = 'DZSM')
			BEGIN

				--SET @Parrameter = 'AND RSMCode = '''+ @EmpMasterCode +'''';

				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')
				--SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';
					SET @Parrameter=@Parrameter+ ' and MKT.TerritoryId in (  select AreaId from tblTerritory where TerritoryId in ( '+ @TRID +'))';

			END

			else if(@EmpRole = 'AM')
			BEGIN

			--SET @Parrameter = 'AND ASMCode ='''+ @EmpMasterCode +'''';
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')
				--SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';
					SET @Parrameter=@Parrameter+ ' and MKT.TerritoryId in ( '+ @TRID +')';

			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode = = '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT distinct MKT.MarketId,MKT.MarketName,MKT.TerritoryId FROM View_FieldForceMarket AS MKT with (nolock) WHERE MKT.MarketId IS NOT NULL ' + @Parrameter
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceMio`

**Parameters:**
- `@EmpRole` — NVARCHAR(50), input
- `@EmpMasterCode` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceMio, ZASHRIS..tblEmpGeneralInfo, tblEmpGeneralInfo, tblMIOInfo, tblTagASMRSMInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceMio, ZASHRIS..tblEmpGeneralInfo, tblEmpGeneralInfo, tblMIOInfo, tblTagASMRSMInfo, filtered by @EmpMasterCode.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT MIO.MIOEmpId,MIO.MIOId,MIO.TerritoryId,MIO.MIOName AS EmpName,MIO.MIOCode AS EmpMasterCode FROM View_FieldForceMio AS MIO WHERE MIO.MioId IS NOT NULL ' + @Parrameter +' order by MIO.MIOName asc' EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 


CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceMio]
    
	@EmpRole NVARCHAR(50) ,
    @EmpMasterCode NVARCHAR(50)

AS
    BEGIN

	declare @comId int


	
	select @comId=e.CompanyId from tblEmpGeneralInfo e 
 
	where     e.EmpMasterCode=@EmpMasterCode  
	 
			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);
  	        DECLARE @TRID NVARCHAR(MAX);


			if(@EmpRole = 'DZSM')
			BEGIN

			select @comId=m.CompanyId from tblMIOInfo m
	
	inner JOIN dbo.tblEmpGeneralInfo e ON m.EmployeeId = e.EmpInfoId
	 
	--inner join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=e.EmpMasterCode 
	  where  --empHR.EmployeeStatus='Active'  and 
	   e.EmpMasterCode=@EmpMasterCode and m.IsActive=1
			select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where empMasterCode=@EmpMasterCode) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')

				SET @Parrameter = 'AND MIO.AreaId in ( '+ @TRID +') and MIO.CompanyId='+CAST(@comId as nvarchar(max));

	
		 
			--	SET @Parrameter = 'AND RSMCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'AM')
			BEGIN
		 

				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where empMasterCode=@EmpMasterCode) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @Parrameter = 'AND MIO.TerritoryId in ( '+ @TRID +') and MIO.CompanyId='+CAST(@comId as nvarchar(max));

			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode =  '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode = '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT MIO.MIOEmpId,MIO.MIOId,MIO.TerritoryId,MIO.MIOName AS EmpName,MIO.MIOCode AS EmpMasterCode FROM View_FieldForceMio AS MIO WHERE MIO.MioId IS NOT NULL ' + @Parrameter +' order by MIO.MIOName asc'
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceNsm`

**Parameters:**
- `@EmpRole` — NVARCHAR(50), input
- `@EmpMasterCode` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceNsm
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceNsm.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT NSM.NSMEmpId, NSM.NSMId,NSM.GroupId,NSM.EmpName,NSM.EmpMasterCode FROM View_FieldForceNsm AS NSM WHERE NSM.NSMId IS NOT NULL ' + @Parrameter EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 

CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceNsm]
    
	@EmpRole NVARCHAR(50) ,
    @EmpMasterCode NVARCHAR(50)

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);


			if(@EmpRole = 'DZSM')
			BEGIN

				SET @Parrameter = 'AND RSMCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'AM')
			BEGIN

				SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode =  '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode = '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT NSM.NSMEmpId, NSM.NSMId,NSM.GroupId,NSM.EmpName,NSM.EmpMasterCode FROM View_FieldForceNsm AS NSM WHERE NSM.NSMId IS NOT NULL ' + @Parrameter
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceRegion`

**Parameters:**
- `@EmpRole` — NVARCHAR(50), input
- `@EmpMasterCode` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceRegion
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceRegion.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT RGN.RegionId,RGN.RegionName,RGn.GroupId FROM View_FieldForceRegion AS RGN WHERE RGN.RegionId IS NOT NULL ' + @Parrameter EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 


CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceRegion]
    
	@EmpRole NVARCHAR(50) ,
    @EmpMasterCode NVARCHAR(50)

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);


			if(@EmpRole = 'DZSM')
			BEGIN

				SET @Parrameter = 'AND RSMCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'AM')
			BEGIN

				SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode =  '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode =  '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT RGN.RegionId,RGN.RegionName,RGn.GroupId FROM View_FieldForceRegion AS RGN WHERE RGN.RegionId IS NOT NULL ' + @Parrameter
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceRsm`

**Parameters:**
- `@EmpRole` — NVARCHAR(50), input
- `@EmpMasterCode` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceRsm
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceRsm.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT RSM.RSMEmpId, RSM.RSMId,RSM.RegionId,RSM.EmpName,RSM.EmpMasterCode FROM View_FieldForceRsm AS RSM WHERE RSM.RSMId IS NOT NULL ' + @Parrameter EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 

CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceRsm]
    
	@EmpRole NVARCHAR(50) ,
    @EmpMasterCode NVARCHAR(50)

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);


			if(@EmpRole = 'DZSM')
			BEGIN

				SET @Parrameter = 'AND RSMCode ='''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'AM')
			BEGIN

				SET @Parrameter = 'AND ASMCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode = '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT RSM.RSMEmpId, RSM.RSMId,RSM.RegionId,RSM.EmpName,RSM.EmpMasterCode FROM View_FieldForceRsm AS RSM WHERE RSM.RSMId IS NOT NULL ' + @Parrameter
			
	
			EXEC (@Query)
    END
```

---

## `sp_SalesAPI_FieldForceTerritory`

**Parameters:**
- `@EmpRole` — NVARCHAR(50), input
- `@EmpMasterCode` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** View_FieldForceTerritory, tblEmpGeneralInfo, tblTagASMRSMInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_FieldForceTerritory, tblEmpGeneralInfo, tblTagASMRSMInfo, filtered by @empId, @Parrameter.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT TTR.TerritoryId,TTR.TerritoryName,TTR.AreaId FROM View_FieldForceTerritory AS TTR WHERE TTR.TerritoryId IS NOT NULL ' + @Parrameter EXEC (@Query) END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 



CREATE PROCEDURE [dbo].[sp_SalesAPI_FieldForceTerritory]
    
	@EmpRole NVARCHAR(50) ,
    @EmpMasterCode NVARCHAR(50)

AS
    BEGIN

			DECLARE @Parrameter NVARCHAR(MAX)='';
  	        DECLARE @Query NVARCHAR(MAX);
			   DECLARE @TRID NVARCHAR(MAX)='';
  	        DECLARE @empId NVARCHAR(MAX)='';;

			SELECT @empId = EmpInfoId 
FROM tblEmpGeneralInfo 
WHERE EmpMasterCode in (CAST(@EmpMasterCode AS NVARCHAR(MAX))) ;

			if(@EmpRole = 'DZSM')
			BEGIN

				--SET @Parrameter = 'AND RSMCode ='''+ @EmpMasterCode +'''';
				
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')
				--SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';
					SET @Parrameter=@Parrameter+ ' and TTR.AreaId in ( '+ @TRID +')';
			END

			else if(@EmpRole = 'AM')
			BEGIN

				--SET @Parrameter = 'AND ASMCode ='''+ @EmpMasterCode +'''';
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')
				--SET @Parrameter = 'AND ASMCode =  '''+ @EmpMasterCode +'''';
					SET @Parrameter=@Parrameter+ ' and TTR.TerritoryId in ( '+ @TRID +')';
			END

			else if(@EmpRole = 'MIO')
			BEGIN

				SET @Parrameter = 'AND MIOCode = '''+ @EmpMasterCode +'''';

			END

			else if(@EmpRole = 'NSM')
			BEGIN

				SET @Parrameter = 'AND NSMCode = '''+ @EmpMasterCode +'''';

			END
			
			SET @Query = 'SELECT DISTINCT TTR.TerritoryId,TTR.TerritoryName,TTR.AreaId FROM View_FieldForceTerritory AS TTR WHERE TTR.TerritoryId IS NOT NULL ' + @Parrameter
			
	
			EXEC (@Query)
    END
```

---

## `sp_SampleInvoicePosting`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ACCDB..tblFinancialYear, CSTL_ACCDB..tblSalesIntegrationMapingNew, CSTL_ACCDB..tblVoucherCategory, CSTL_ERPDB..Table_Integration_AccountsConnectivity, CSTL_ERPDB..tblCompanyInfo, CSTL_ERPDB..tbl_AccIntegrationTable, CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster, CSTL_ERP_ACCDB..tblFinancialYear, tblCentralStore, tblCompanyUnit, tblCustMaster, tblDCStore, tblInvoice, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation, tbl_INV_WeightedAvg, tbl_PTP_AdavnecPayToSupplierMaster
**Tables written:** INSERT → CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster, CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail; UPDATE → tblSampleIssue

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** GL-posting procedure restricted to `CompanyId = 16`: for each qualifying row (cursor-driven), it builds a voucher code and financial-year id, inserts a header row into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster` (`VoucherType='Journal'`, `Particular='SampleInvoice'`) and a matching debit/credit line pair into `tblDebitCreditVoucherDetail`, using GL account ids looked up from `tblSalesIntegrationMapingNew` for that `Particular`; it then sets `tblSampleIssue.IsPosting` to 1 if a voucher now exists for the record, else 0.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/SampleIssueDal.cs:415` — `return aInternalDal.UpdateAction("sp_SampleInvoicePosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SampleInvoicePosting] 

	@InvoiceId INT
	
AS
BEGIN

	--DECLARE @InvoiceId INT = 8665


	DECLARE @TempCompanyId INT = 0
	DECLARE @ActionStatus NVARCHAR(200) = 0

	SELECT @TempCompanyId = CompanyId, @ActionStatus = ActionStatus FROM tblSampleIssue AS SI 
	INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = SI.ComUnitId
	WHERE OrderId = @InvoiceId


	IF(@TempCompanyId IS NOT NULL)
	BEGIN

		

		-- Journal Posting 

			DECLARE @VoucherMasterId INT
			DECLARE @VoucherCode NVARCHAR(MAX)
			DECLARE @BiusinessUnitId INT
			--DECLARE @SupplierId INT
			--DECLARE @BankAccountId INT
			--DECLARE @CashAccountId INT
			DECLARE @FinancialYearId INT
 --			  DECLARE @DateOfPayment DATETIME
			DECLARE @CurrencyId INT = 1
			DECLARE @Conversionrate DECIMAL(18,4) = 1.00
 --			  DECLARE @Narration NVARCHAR(MAX)
 --			  DECLARE @TotalPayAmountBDT DECIMAL(18,4)
 --			  DECLARE @TotalAmount DECIMAL(18,4)
 --			  DECLARE @VoucherCategory INT
 --			  DECLARE @ChecqueNo  NVARCHAR(MAX)
 --			  DECLARE @ChecqueDate DATETIME
 --			  DECLARE @ReferenceNo NVARCHAR(MAX)
 --			  DECLARE @ReferenceDate DATETIME
			--DECLARE @PurchaseOrderLocalCode2 NVARCHAR(500)
			DECLARE @VoucherType NVARCHAR(200) ='Journal'
			DECLARE @ApprovalDate DATETIME
			DECLARE @ApproveBy NVARCHAR(200)
			DECLARE @TableId INT
			DECLARE @InvoiceNo  NVARCHAR(200)
			DECLARE @MaterialConversionDate DATETIME
			DECLARE @DeliveryNetAmount  DECIMAL(18,4)
			DECLARE @InvoiceId2 INT
			DECLARE @VoucherMasterId2 INT
			DECLARE @CustomerName NVARCHAR(500)
			DECLARE @SubsidiaryInfoId INT
			DECLARE @ReferenceInfoId INT

			-- For Credit Amount

			DECLARE @ToatlAdvanceAmountFc DECIMAL(18,4) = 0
			        
			--------------------------------------------------------
			DECLARE @MyCursor2 CURSOR
			SET @MyCursor2 = CURSOR FAST_FORWARD
			FOR
			---------------

			--SELECT * FROM tblInvoice

				--DECLARE @InvoiceId INT = 8665

				--SELECT MsT_Trans_Id,SUM(Curr_Amt*-1) AS TotalValue FROM tbl_INV_WeightedAvg 
				--WHERE Trans_Type_Id = 1008 AND MsT_Trans_Id = @InvoiceId GROUP BY MsT_Trans_Id



				SELECT DISTINCT CI.CompanyId,FY.FinancialYearId,INV.OrderCode InvoiceNo,FORMAT(INV.SubmissionDate, 'dd-MMM-yyyy') UpdateDate,
				TV.TotalValue DeliveryNetAmount,TBL.IntegrationTableId,INV.OrderId, 
				CMS.CustomerName + CASE WHEN CMS.CustomerCode IS NOT NULL THEN ' (' + CMS.CustomerCode + ')' END CustomerName,V.VoucherMasterId,
				0 SubsidiaryInfoId,0 ReferenceInfoId FROM tblSampleIssue AS INV WITH(NOLOCK)
				LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
				LEFT JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
				INNER JOIN CSTL_ACCDB..tblFinancialYear AS FY ON CI.CompanyId = FY.CompanyId AND INV.SubmissionDate BETWEEN FY.StartDate AND FY.EndDate
				LEFT JOIN tblCustMaster AS CMS ON INV.CustomerMasterId = CMS.CustomerMasterId
				LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblSampleIssue'))) 
				LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2015 AND INV.OrderId = V.CommonPK_ID

				LEFT JOIN (SELECT MsT_Trans_Id,SUM(Curr_Amt*-1) AS TotalValue FROM tbl_INV_WeightedAvg 
				WHERE Trans_Type_Id = 1008 AND MsT_Trans_Id = @InvoiceId GROUP BY MsT_Trans_Id) AS TV ON INV.OrderId = TV.MsT_Trans_Id

				WHERE ISNULL(VoucherMasterId,0) = 0 AND ISNULL( TV.TotalValue,0) > 0 AND INV.OrderId = @InvoiceId


			--WM.WHStockInMasterID,WHStockInCode,WM.WHStockInDate,TotalQuantity,
			--TotalValue,ChallanNo,ChallanDate,WH.CompanyId
  
			 --SELECT * FROM tbl_PTP_AdavnecPayToSupplierMaster  M.ApprovalStatus = 'Accepted' AND 

			----------
			OPEN @MyCursor2
			FETCH NEXT FROM @MyCursor2                               
			INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,
			@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId,@ReferenceInfoId
			
			WHILE @@FETCH_STATUS = 0
			BEGIN

			--SELECT * FROM CSTL_ACCDB..tblVoucherCategory

			 --- Master Information Posting

			 DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)   
			 DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
			 DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
			 SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster 
			 WHERE PostingDate = @ApprovalDate
			
				IF(@VoucherType='DebitVoucherBank')
				BEGIN
					SET @VoucherCode='DVB-'+@VoucherCode	
				END

				IF(@VoucherType='DebitVoucherCash')
				BEGIN
					SET @VoucherCode='DVC-'+@VoucherCode
				END

				IF(@VoucherType='CreditVoucherBank')
				BEGIN
					SET @VoucherCode='CVB-'+@VoucherCode	
				END

				IF(@VoucherType='CreditVoucherCash')
				BEGIN
					SET @VoucherCode='CVC-'+@VoucherCode
				END

				IF(@VoucherType='FundTransfer')
				BEGIN
					SET @VoucherCode='FUT-'+@VoucherCode
				END

				IF(@VoucherType='Journal')
				BEGIN
					SET @VoucherCode='JVP-'+@VoucherCode
				END

				
				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
				        ( VoucherCode ,
				          CompanyInfoId , 
				          FinancialYearId ,
				          PostingDate ,
				          VoucherCategoryID ,
				          CurrencyId ,
				          CurrencyCovRate ,
				          Narration ,        
				          PaytoIsSupplier ,
				          IsInvoice ,
				          TotalAmountBDT ,
				          TotalAmount ,
				          Remarks ,
				          EntryBy ,
				          EntryDate ,
				          VoucherType ,
				          ActionStatus,
						  IsCostCenter,
						  IsBuyerCostCenter,
						  IsOtherCostCenter,
						  ApprovedBy,
						  ApprovedDate,
						  CommonPK_ID,
						  TableID
				        )
				VALUES  ( 
						  @VoucherCode ,
				          @BiusinessUnitId , 
				          @FinancialYearId ,
				          @ApprovalDate ,
				          2 ,
				          @CurrencyId ,
				          @Conversionrate,
						   
						 'Sample product delivered to ' 
						 + @CustomerName 
						 + ' under invoice number ' 
						 + @InvoiceNo 
						 + ' for BDT ' 
						 + CONVERT(NVARCHAR(50), @DeliveryNetAmount) 
						 + ' as part of promotional expense.',

				          0 ,
				          0 ,
				          @DeliveryNetAmount ,
				          @DeliveryNetAmount ,
				          'Auto Posting' ,
				          @ApproveBy ,
				          @ApprovalDate ,
				          @VoucherType ,
				          'Approved',
						  0,0,0,'Auto Process',
						  @ApprovalDate,@InvoiceId2,@TableId
				        )
				
				SET @VoucherMasterId = SCOPE_IDENTITY()

				-- Detail Information Posting

				DECLARE	@DrGl INT = 0 
				SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 160 AND FlagId = 1022 -- Sample Expense

				--PRINT @DrGl

				-- Debit Side

				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit,
				DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks, SubsidoryLedgerId,RefInfoInd) 
				VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Sample Expense', @SubsidiaryInfoId, @ReferenceInfoId);

				-- Credit Side

				DECLARE	@CrGl INT = 0 
				SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 160 AND FlagId = 1014  -- FG Inventory 
				--AND LTRIM(RTRIM(UPPER(BalanceType))) = LTRIM(RTRIM(UPPER('CR')))

				--PRINT @CrGl
				
				INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, RefInfoInd, Remarks)
				VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, @DeliveryNetAmount, 0, 0, 0, 'Sample Expense');

			
			
			FETCH NEXT FROM @MyCursor2
			INTO @BiusinessUnitId,@FinancialYearId,@InvoiceNo,@ApprovalDate,@DeliveryNetAmount,@TableId,@InvoiceId2,
			@CustomerName,@VoucherMasterId2,@SubsidiaryInfoId,@ReferenceInfoId

			
			END
			CLOSE @MyCursor2
			DEALLOCATE @MyCursor2

			END -- End Of Cursor







	--	DECLARE @ProformaInvId INT
	--	DECLARE @InvDate DATETIME
	--	DECLARE @CompanyId INT
	--	DECLARE @CostPrice NVARCHAR(500)
	--	DECLARE @InvoiceNo NVARCHAR(500)
	--	DECLARE @VoucherMasterId INT 
		     
	--	--------------------------------------------------------
	--	DECLARE @MyCursor CURSOR
	--	SET @MyCursor = CURSOR FAST_FORWARD
	--	FOR
	--	---------------
		
		
	--	SELECT SI.OrderId,SI.SubmissionDate,UNT.CompanyId,SUM(((CTRS.UnitPrice*(SIT.Quantity*-1)) + (CTRS.VATPerUnit*(SIT.Quantity*-1)))) AS TotalPrice,
	--	SI.OrderCode FROM tblSampleIssue AS SI
	--	LEFT JOIN tblSampleIssueDetail AS SD ON SI.OrderId = SD.OrderId
	--	LEFT JOIN tblSampleIssueTranscation AS SIT ON SIT.IssueId = SI.OrderId AND IssueDetailId = SD.OrderDetailId
	--	INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = SIT.DCStoreId
	--	INNER JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
	--	INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = SI.ComUnitId
	--	--INNER JOIN CSTL_ACCDB..tblFinancialYear AS FY ON CI.
	--	WHERE SI.OrderId = @InvoiceId
	--	GROUP BY SI.OrderId,SI.SubmissionDate,UNT.CompanyId,SI.OrderCode 
		
	--	----------
	--	OPEN @MyCursor
	--	FETCH NEXT FROM @MyCursor
	--	INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
		
	--	WHILE @@FETCH_STATUS = 0
	--	BEGIN
		

	--	-- Master Variable Info
	--	-------------------------------------

	--	DECLARE @VoucherType NVARCHAR(MAX) ='Journal'
	--	DECLARE @VoucherCode NVARCHAR(MAX)

	--	DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@InvDate)), 3, 3)   
	--	DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@InvDate)) END)
	--	DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@InvDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@InvDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@InvDate)) END)
	--	SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster 
	--	WHERE @InvDate = @InvDate
		
	--		IF(@VoucherType='DebitVoucherBank')
	--		BEGIN
	--			SET @VoucherCode='DVB-'+@VoucherCode	
	--		END
	--		IF(@VoucherType='DebitVoucherCash')
	--		BEGIN
	--			SET @VoucherCode='DVC-'+@VoucherCode
	--		END
	--		IF(@VoucherType='CreditVoucherBank')
	--		BEGIN
	--			SET @VoucherCode='CVB-'+@VoucherCode	
	--		END
	--		IF(@VoucherType='CreditVoucherCash')
	--		BEGIN
	--			SET @VoucherCode='CVC-'+@VoucherCode
	--		END
	--		IF(@VoucherType='FundTransfer')
	--		BEGIN
	--			SET @VoucherCode='FUT-'+@VoucherCode
	--		END

	--		IF(@VoucherType='Journal')
	--		BEGIN
	--			SET @VoucherCode='JVP-'+@VoucherCode
	--		END


	--	DECLARE @FinYearId INT = 0

	--	SELECT @FinYearId = FinancialYearId FROM CSTL_ERP_ACCDB..tblFinancialYear 
	--	WHERE CompanyId = @CompanyId AND Status IN ('Active') AND @InvDate BETWEEN StartDate AND EndDate



	--	--SELECT * FROM CSTL_ERP_ACCDB..tblFinancialYear  WHERE CompanyId = 1 AND '2024-12-07' BETWEEN StartDate AND EndDate

	--	----------------------------------


	--	-- Master Information Posting

	--	INSERT	INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
	--	(
	--	    VoucherCode,
	--	    CompanyInfoId,
	--	    FinancialYearId,
	--	    PostingDate,
	--	    VoucherCategoryID,
	--	    CurrencyId,
	--	    CurrencyCovRate,
	--	    Narration,
	--	    TotalAmount,
	--	    TotalAmountBDT,
	--	    EntryBy,
	--	    EntryDate,
	--	    VoucherType, 
	--	    JournalType,
	--	    SampleInvoiceId,
	--		IsCostCenter,
	--		IsInvoice,
	--		isManualVCRNo,
	--		IsExportInvoice,
	--		IsImportInvoice

	--	)
	--	VALUES
	--	(   
	--	    @VoucherCode,       -- VoucherCode - nvarchar(max)
	--	    @CompanyId,         -- CompanyInfoId - int
	--	    @FinYearId,         -- FinancialYearId - int
	--	    @InvDate, -- PostingDate - datetime
	--	    2,         -- VoucherCategoryID - int
	--	    1,         -- CurrencyId - int
	--	    1.00,      -- CurrencyCovRate - decimal(18, 2)
	--	    'Sample Invoice - ( Invoice No:' + @InvoiceNo + ' , Invoice Date:' + CONVERT(NVARCHAR(50), @InvDate, 101) + ', Stock Value:' + CONVERT(NVARCHAR(50), @CostPrice) + ')',       -- Narration - nvarchar(max)
	--	    0.00,      -- TotalAmount - decimal(18, 2)
	--	    @CostPrice,      -- TotalAmountBDT - decimal(18, 2)
	--	    'Auto Process',       -- EntryBy - nvarchar(max)
	--	    CURRENT_TIMESTAMP, -- EntryDate - datetime	  
	--	    'Journal',       -- VoucherType - nvarchar(50)
	--	    'Adjustable',       -- JournalType - nvarchar(50)
	--	    @ProformaInvId,      -- ProformaId -int
	--		0,0,0,0,0
	--	)


	--	-- Detail Information Posting

		
	--	SET @VoucherMasterId = SCOPE_IDENTITY()

	--	DECLARE	@DrGl INT 
	--	SELECT @DrGl = GlId FROM CSTL_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('SampleInvoice') AND BalanceType IN ('Dr')

	--	-- Debit

	--	INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
	--	(
	--	    VoucherMasterId,
	--	    AccountId,
	--	    DebitorCredit,
	--	    DebitAmount,
	--	    CreditAmount,
	--	    DebitAmountBDT,
	--	    CreditAmountBDT
	--	)
	--	VALUES
	--	(   @VoucherMasterId,    -- VoucherMasterId - int
	--	    @DrGl,    -- AccountId - int
	--	    'Debit',  -- DebitorCredit - nvarchar(50)
	--	    0.00, -- DebitAmount - decimal(18, 2)
	--	    0.00, -- CreditAmount - decimal(18, 2)
	--	    @CostPrice, -- DebitAmountBDT - decimal(18, 2)
	--	    0.00 -- CreditAmountBDT - decimal(18, 2)

	--	)

	--	--- Credit

	--	DECLARE	@CrGl INT 
	--	SELECT @CrGl = GlId FROM CSTL_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('SampleInvoice') AND BalanceType IN ('Cr')
		
	--	INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
	--	(
	--	    VoucherMasterId,
	--	    AccountId,
	--	    DebitorCredit,
	--	    DebitAmount,
	--	    CreditAmount,
	--	    DebitAmountBDT,
	--	    CreditAmountBDT
	--	)
	--	VALUES
	--	(   @VoucherMasterId,    -- VoucherMasterId - int
	--	    @CrGl,    -- AccountId - int
	--	    'Credit',  -- DebitorCredit - nvarchar(50)
	--	    0.00, -- DebitAmount - decimal(18, 2)
	--	    0.00, -- CreditAmount - decimal(18, 2)
	--	    0.00, -- DebitAmountBDT - decimal(18, 2)
	--	    @CostPrice -- CreditAmountBDT - decimal(18, 2)
	--	)


	--	DECLARE @Count INT = 0 

	--	SELECT @Count = COUNT(ProformaId) FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WHERE SampleInvoiceId = @ProformaInvId

	--	IF (@Count > 0)
	--		BEGIN
			   
	--		   UPDATE tblSampleIssue SET IsPosting = 1 WHERE OrderId = @ProformaInvId

	--		END
	--	ELSE
	--		BEGIN

	--		UPDATE tblSampleIssue SET IsPosting = 0 WHERE OrderId = @ProformaInvId

	--		END
		
		
	--	FETCH NEXT FROM @MyCursor
	--	INTO @ProformaInvId,@InvDate,@CompanyId,@CostPrice,@InvoiceNo
		
	--	END
	--	CLOSE @MyCursor
	--	DEALLOCATE @MyCursor
		

	--END
	
	

	
END
```

---

## `sp_Save_ASMRSMTag`

**Parameters:**
- `@UserId` — INT, input
- `@TerritoryAreaId` — int, input = `NULL`
- `@UserRoleID` — int, input = `NULL`
- `@EmpInfoId` — int, input = `NULL`

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblTagASMRSMInfo

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblTagASMRSMInfo` (columns: UserRoleId, EmpInfoId, TerritoryAreaId, UserId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 create PROCEDURE [dbo].[sp_Save_ASMRSMTag]
	-- Add the parameters for the stored procedure here
	@UserId INT,
    @TerritoryAreaId int=NULL,
    @UserRoleID int=NULL,
    @EmpInfoId int=NULL 
AS
    BEGIN
 INSERT INTO [dbo].[tblTagASMRSMInfo]
           ([UserRoleId]
           ,[EmpInfoId]
           ,[TerritoryAreaId]
           ,[UserId])
     VALUES
           (@UserRoleId 
           ,@EmpInfoId 
           ,@TerritoryAreaId 
           ,@UserId)
 
    END
```

---

## `sp_Save_BatchCode`

**Parameters:**
- `@BatchId` — INT, input

**Tables read (FROM/JOIN):** tblDeb, tblInvoiceBatch
**Tables written:** UPDATE → tblInvoiceBatch

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblDeb, tblInvoiceBatch, then updates `tblInvoiceBatch` (sets: BatchCode).

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs:119` — `return aCommonInternalDal.UpdateAction("sp_Save_BatchCode",aSqlParameterlist);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_BatchCode] 

	@BatchId INT
	
AS
BEGIN
	
	DECLARE @ProformaCode  NVARCHAR(500)
	DECLARE @ProCount INT = 0
	
	SELECT @ProCount = ISNULL(COUNT(BatchId),0)+1  FROM tblInvoiceBatch WHERE MONTH(Date) = MONTH(GETDATE()) AND Year(Date) = Year(GETDATE())

	DECLARE @length INT = 0
	SET @length = LEN(CONVERT(NVARCHAR(MAX),@ProCount))

	IF(@length = 1)
	BEGIN
	   SET @ProformaCode = Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '000' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 2)
	BEGIN
		SET @ProformaCode = Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '00' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 3)
	BEGIN
		SET @ProformaCode = Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '0' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE
	BEGIN
	   SET @ProformaCode = Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + (CONVERT(NVARCHAR(MAX),@ProCount))
	END

	--PRINT @ProformaCode

	UPDATE tblInvoiceBatch SET BatchCode = @ProformaCode WHERE BatchId = @BatchId

END



--SELECT * FROM tblDeb
```

---

## `sp_Save_BDoctorChemberDetail`

**Parameters:**
- `@ChamberTypeId` — INT, input
- `@DoctorId` — INT, input
- `@Name` — nvarchar(max), input = `null`
- `@Phone` — nvarchar(max), input = `null`
- `@Address` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblDoctorChemberDetail
**Tables written:** INSERT → tblDoctorChemberDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblDoctorChemberDetail, then inserts into `tblDoctorChemberDetail` (columns: ChamberTypeId, DoctorId, Name, Phone, Address).

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_BDoctorChemberDetail]
	-- Add the parameters for the stored procedure here
	 

@ChamberTypeId  INT,
@DoctorId  INT,
@Name   nvarchar(max)=null,
@Phone   nvarchar(max)=null,
@Address   nvarchar(max)=null
 
AS
    BEGIN
	declare @coun  INT=0
	if(@Name is not null)

select @coun=ISNULL(count(*),0) from [tblDoctorChemberDetail] where ChamberTypeId=@ChamberTypeId and  DoctorId=@DoctorId and Name=@Name

if(@coun=0)

BEGIN
	INSERT INTO [dbo].[tblDoctorChemberDetail]
           ([ChamberTypeId]
           ,[DoctorId]
           ,[Name]
           ,[Phone]
           ,[Address])
     VALUES
           (@ChamberTypeId 
           ,@DoctorId 
           ,@Name 
           ,@Phone
           ,@Address)

 END

END
```

---

## `sp_Save_DoctorBrandDetail`

**Parameters:**
- `@DoctorId` — INT, input
- `@BrandId` — int, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorBrandDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorBrandDetail` (columns: DoctorId, BrandId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorBrandDetail]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@BrandId int

AS
BEGIN
	
	INSERT INTO [dbo].[tblDoctorBrandDetail]
           (DoctorId
           ,BrandId)
     VALUES
           (@DoctorId,
		   @BrandId)
END
```

---

## `sp_Save_DoctorCategory`

**Parameters:**
- `@CategoryId` — INT, input
- `@CategoryName` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorCategory

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorCategory` (columns: CategoryName, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[sp_Save_DoctorCategory]
	-- Add the parameters for the stored procedure here
	@CategoryId INT,
    @CategoryName NVARCHAR(MAX) ,
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)

AS
    BEGIN
	
        INSERT  INTO dbo.tblDoctorCategory
                ( CategoryName ,                 
                  IsActive ,
                  Activedate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @CategoryName ,              
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )

SELECT SCOPE_IDENTITY()





END
```

---

## `sp_Save_DoctorChamber`

**Parameters:**
- `@ChamberId` — INT, input
- `@ChamberName` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorChamber

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorChamber` (columns: ChamberName, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create  PROCEDURE [dbo].[sp_Save_DoctorChamber]
	-- Add the parameters for the stored procedure here
	@ChamberId INT,
    @ChamberName NVARCHAR(MAX) ,
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)

AS
    BEGIN
	
        INSERT  INTO dbo.tblDoctorChamber
                ( ChamberName ,                  
                  IsActive ,
                  Activedate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @ChamberName ,              
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )

SELECT SCOPE_IDENTITY()

END
```

---

## `sp_Save_DoctorChemberDetail`

**Parameters:**
- `@DoctorId` — INT, input
- `@ChamberTypeId` — INT, input
- `@Name` — NVARCHAR(500), input
- `@Phone` — NVARCHAR(500), input
- `@Address` — NVARCHAR(500), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorChemberDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorChemberDetail` (columns: DoctorId, ChamberTypeId, Name, Phone, Address).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorChemberDetail]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@ChamberTypeId INT,
	@Name NVARCHAR(500),
	@Phone NVARCHAR(500),
	@Address NVARCHAR(500)

AS
BEGIN
	
	INSERT INTO [dbo].[tblDoctorChemberDetail]
           (DoctorId,
		    ChamberTypeId
           ,Name
           ,Phone
           ,Address)
     VALUES
           (@DoctorId,
		   @ChamberTypeId
           ,@Name
           ,@Phone
           ,@Address)
END
```

---

## `sp_Save_DoctorContactDetail`

**Parameters:**
- `@DoctorId` — INT, input
- `@ContactType` — NVARCHAR(500), input
- `@Contact` — NVARCHAR(500), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorContactDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorContactDetail` (columns: DoctorId, ContactType, Contact).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorContactDetail]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@ContactType NVARCHAR(500),
	@Contact NVARCHAR(500)

AS
BEGIN
	
	INSERT INTO [dbo].[tblDoctorContactDetail]
           (DoctorId
           ,ContactType
           ,Contact)
     VALUES
           (@DoctorId
           ,@ContactType
           ,@Contact)
END
```

---

## `sp_Save_DoctorContactDetail_New`

**Parameters:**
- `@DoctorId` — INT, input
- `@ContactTypeId` — INT, input
- `@ContactType` — NVARCHAR(500), input
- `@Contact` — NVARCHAR(500), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorContactDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorContactDetail` (columns: DoctorId, ContactType, ContactTypeId, Contact).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Save_DoctorContactDetail_New]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@ContactTypeId INT,
	@ContactType NVARCHAR(500),
	@Contact NVARCHAR(500)

AS
BEGIN
	
	INSERT INTO [dbo].[tblDoctorContactDetail]
           (DoctorId
           ,ContactType
           ,ContactTypeId
           ,Contact)
     VALUES
           (@DoctorId
           ,@ContactType,@ContactTypeId
           ,@Contact)
END
```

---

## `sp_Save_DoctorCustomer`

**Parameters:**
- `@DoctorId` — INT, input
- `@CustomerCode` — NVARCHAR(MAX), input
- `@CustomerName` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorCustomerDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorCustomerDetail` (columns: DoctorId, CustomerCode, CustomerName).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorCustomer]
	-- Add the parameters for the stored procedure here
	@DoctorId INT,
    @CustomerCode NVARCHAR(MAX) ,
    @CustomerName NVARCHAR(MAX)


AS
    BEGIN
	

        INSERT INTO tblDoctorCustomerDetail
           (DoctorId
           ,CustomerCode
           ,CustomerName)
     VALUES
           (@DoctorId
           ,@CustomerCode
           ,@CustomerName)

SELECT SCOPE_IDENTITY()
		
END
```

---

## `sp_Save_DoctorDegree`

**Parameters:**
- `@DegreeId` — INT, input
- `@DoctorTypeId` — int, input = `null`
- `@DegreeName` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** tblDoctorDegree
**Tables written:** INSERT → tblDoctorDegree

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblDoctorDegree, then inserts into `tblDoctorDegree` (columns: DegreeName, DoctorTypeId, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() End else Return 0 END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorDegree]
	-- Add the parameters for the stored procedure here
	@DegreeId INT,
	@DoctorTypeId int=null,
    @DegreeName NVARCHAR(MAX) ,
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)


AS
    BEGIN

 
	if not exists (select DegreeName from tblDoctorDegree where DegreeName=@DegreeName )
begin 
 INSERT  INTO dbo.tblDoctorDegree
                ( DegreeName , DoctorTypeId,               
                  IsActive ,
                  Activedate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @DegreeName ,@DoctorTypeId,
                
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )
SELECT SCOPE_IDENTITY()
End
else  Return 0
	
 
		
END
```

---

## `sp_Save_DoctorDegreeDetail`

**Parameters:**
- `@DoctorId` — INT, input
- `@DegId` — INT, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorDegreeDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorDegreeDetail` (columns: DoctorId, DegId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorDegreeDetail]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@DegId INT

AS
BEGIN
	
	INSERT INTO [dbo].[tblDoctorDegreeDetail]
           (
           DoctorId
           ,DegId)
     VALUES
           (
            @DoctorId
           ,@DegId)
END
```

---

## `sp_Save_DoctorDesignation`

**Parameters:**
- `@DesignationId` — INT, input
- `@DesignationName` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorDesignation

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorDesignation` (columns: DesignationName, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[sp_Save_DoctorDesignation]
	-- Add the parameters for the stored procedure here
	@DesignationId INT,
    @DesignationName NVARCHAR(MAX) ,
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)

AS
    BEGIN
	
        INSERT  INTO dbo.tblDoctorDesignation
                (  DesignationName,                  
                  IsActive ,
                  Activedate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @DesignationName ,              
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )

SELECT SCOPE_IDENTITY()

END
```

---

## `sp_Save_DoctorInistitutionDetail`

**Parameters:**
- `@DoctorId` — INT, input
- `@InstitutionId` — INT, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorInstitutionDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorInstitutionDetail` (columns: DoctorId, InstitutionId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorInistitutionDetail]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@InstitutionId INT

AS
BEGIN
	
	
	INSERT INTO tblDoctorInstitutionDetail
           (DoctorId
           ,InstitutionId)
     VALUES
           (@DoctorId
           ,@InstitutionId)

END
```

---

## `sp_Save_DoctorMarketDetail`

**Parameters:**
- `@DoctorId` — INT, input
- `@SubmarketId` — INT, input
- `@IsTopDoctor` — BIT, input
- `@IsDCRAllowed` — BIT, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorMarketDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorMarketDetail` (columns: DoctorId, SubmarketId, IsTopDoctor, IsDCRAllowed).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorMarketDetail]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@SubmarketId INT,
	@IsTopDoctor BIT ,@IsDCRAllowed BIT

AS
BEGIN
	
	
	INSERT INTO [dbo].[tblDoctorMarketDetail]
           (DoctorId
           ,SubmarketId,IsTopDoctor,IsDCRAllowed)
     VALUES
           (@DoctorId
           ,@SubmarketId,@IsTopDoctor,@IsDCRAllowed)

END
```

---

## `sp_Save_DoctorMaster`

**Parameters:**
- `@DoctorId` — int, input
- `@SecondaryCode` — nvarchar(max), input = `null`
- `@DesignationId` — int, input = `null`
- `@Gender` — nvarchar(max), input = `null`
- `@DoctorName` — nvarchar(max), input = `null`
- `@IsActive` — bit, input = `null`
- `@Activedate` — datetime, input = `null`
- `@InactiveDate` — datetime, input = `null`
- `@EntryBy` — int, input = `null`
- `@DivisionId` — int, input = `null`
- `@DistrictId` — int, input = `null`
- `@ThanaId` — int, input = `null`
- `@UPCode` — nvarchar(max), input = `null`
- `@DoctorTypeId` — int, input = `null`
- `@MarketId` — int, input = `null`
- `@UnionName` — nvarchar(max), input = `null`
- `@Reamrks` — nvarchar(max), input = `null`
- `@StationTypeId` — int, input = `null`
- `@ProgramTypeId` — int, input = `null`
- `@DoctorCategoryId` — int, input = `null`
- `@SMCTypeId` — int, input = `null`

**Tables read (FROM/JOIN):** tblDoctorMaster
**Tables written:** INSERT → tblDoctorMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblDoctorMaster, then inserts into `tblDoctorMaster`.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorMaster]
	-- Add the parameters for the stored procedure here

		 

		   @DoctorId int 
      ,@SecondaryCode  nvarchar(max)=null
      ,@DesignationId int=null
	  ,@Gender  nvarchar(max)=null
      ,@DoctorName nvarchar(max)=null
	  ,@IsActive  bit=null
      ,@Activedate   datetime=null
      ,@InactiveDate   datetime=null
      ,@EntryBy  int=null
	  ,@DivisionId  int=null
      ,@DistrictId  int=null
      ,@ThanaId  int=null
	  ,@UPCode nvarchar(max)=null
      ,@DoctorTypeId  int=null
      
      ,@MarketId  int=null
      ,@UnionName  nvarchar(max)=null
      ,@Reamrks  nvarchar(max)=null
      ,@StationTypeId   int=null,
	  @ProgramTypeId   int=null,
	  @DoctorCategoryId   int=null,
	  @SMCTypeId   int=null


AS
BEGIN
	

	  DECLARE @CustCode NVARCHAR(MAX)
	DECLARE @CustCodeint INT
SELECT  @CustCodeint=Max(CONVERT(INT,SUBSTRING(DoctorCode,2,LEN(DoctorCode)+1)))+1 FROM dbo.tblDoctorMaster WHERE ApprovalStatus='2' 
--ORDER BY DoctorId DESC
SET @CustCode='D'+CONVERT(NVARCHAR(MAX),@CustCodeint)

PRINT @CustCode
	   
 --     declare @CustomerCode nvarchar(max)
	--select @CustomerCode=MAX(ISNULL(DoctorCode,1000))+1 from tblDoctorMaster where  ApprovalStatus='2'


	--INSERT INTO tblDoctorMaster
 --          (  DoctorName
 --     ,[DoctorCode]
      
     
 --     ,[DesignationId]
    
 --     ,[Gender]
   
   
       
 --     ,[IsActive]
 --     ,[Activedate]
 --     ,[EntryBy]
 --     ,[EntryDate]
    
 --     ,[IsFromApp]
 --     ,[ApprovalStatus]
       
     
 --     ,[DoctorTypeId]
      
 --     ,[MarketId]
 --     ,[UnionName]
    
 --     ,[StationTypeId],ProgramTypeId,DoctorCategoryId,InactiveDate, SMCTypeId)
 --    VALUES
 --          (  @DoctorName 
 --     ,@CustCode 
      
    
 --     ,@DesignationId 
    
 --     ,@Gender 
   
   
       
 --     ,@IsActive 
 --     ,@Activedate 
 --     ,@EntryBy 
 --     ,GETDATE() 
      
     
 --     ,0 
 --     ,'2' 
       
   
 --     ,@DoctorTypeId 
     
 --     ,@MarketId 
 --     ,@UnionName 
 --     ,@Reamrks 
 --     ,@StationTypeId ,@ProgramTypeId,@DoctorCategoryId,@InactiveDate,@SMCTypeId
 --          )

	  SELECT SCOPE_IDENTITY()

END
```

---

## `sp_Save_DoctorPatientType`

**Parameters:**
- `@PatientTypeId` — INT, input
- `@PatientType` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorPatientType

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorPatientType` (columns: PatientType, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[sp_Save_DoctorPatientType]
	-- Add the parameters for the stored procedure here
	@PatientTypeId INT,
    @PatientType  NVARCHAR(MAX) ,
	@IsActive  BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)

AS
    BEGIN
	
        INSERT  INTO [dbo].[tblDoctorPatientType]
                ( PatientType,                  
                  IsActive ,
                  Activedate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @PatientType ,              
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )

SELECT SCOPE_IDENTITY()



END
```

---

## `sp_Save_DoctorSpecialDay`

**Parameters:**
- `@SpecialDayId` — INT, input
- `@SpecialDay` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorSpecialDay

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorSpecialDay` (columns: SpecialDay, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Save_DoctorSpecialDay]
	-- Add the parameters for the stored procedure here
	@SpecialDayId  INT,
    @SpecialDay NVARCHAR(MAX) ,
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)

AS
    BEGIN
	
        INSERT  INTO [dbo].[tblDoctorSpecialDay] 
                ( SpecialDay ,                   
                  IsActive ,
                  Activedate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @SpecialDay ,              
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )

SELECT SCOPE_IDENTITY()

END
```

---

## `sp_Save_DoctorSpecialDayDetail`

**Parameters:**
- `@DoctorId` — INT, input
- `@SpecialDayId` — INT, input
- `@SpecialDate` — Datetime, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorSpecialDayDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorSpecialDayDetail` (columns: DoctorId, SpecialDayId, SpecialDate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorSpecialDayDetail]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@SpecialDayId INT,
	@SpecialDate Datetime

AS
BEGIN
	
	
	INSERT INTO [dbo].[tblDoctorSpecialDayDetail]
           (DoctorId
           ,SpecialDayId
           ,SpecialDate)
     VALUES
           (@DoctorId
           ,@SpecialDayId
           ,@SpecialDate)

END
```

---

## `sp_Save_DoctorSpecialDayDetails`

**Parameters:**
- `@DoctorId` — INT, input
- `@SpecialDayId` — INT, input
- `@SpecialDate` — datetime, input = `null`

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorSpecialDayDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorSpecialDayDetail` (columns: DoctorId, SpecialDayId, SpecialDate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_Save_DoctorSpecialDayDetails]
	-- Add the parameters for the stored procedure here

	@DoctorId INT,
	@SpecialDayId INT,
	@SpecialDate datetime=null 

AS
BEGIN
	
	INSERT INTO [dbo].tblDoctorSpecialDayDetail
           (DoctorId
           ,SpecialDayId
           ,SpecialDate )
     VALUES
           (@DoctorId
           ,@SpecialDayId,@SpecialDate)
END
```

---

## `sp_Save_DoctorSpeciality`

**Parameters:**
- `@SpecialityId` — INT, input
- `@SpecialityName` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorSpeciality

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorSpeciality` (columns: SpecialityName, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Save_DoctorSpeciality]
	-- Add the parameters for the stored procedure here
	@SpecialityId   INT,
    @SpecialityName  NVARCHAR(MAX) ,
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)

AS
    BEGIN
	
        INSERT  INTO [dbo].[tblDoctorSpeciality]
                ( SpecialityName ,                   
                  IsActive ,
                  Activedate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @SpecialityName ,              
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )

SELECT SCOPE_IDENTITY()

END
```

---

## `sp_Save_DoctorSpecialityDetail`

**Parameters:**
- `@DoctorId` — INT, input = `NULL`
- `@SpecialityId` — NVARCHAR(max), input = `NULL`

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblDoctorSpecialityDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblDoctorSpecialityDetail` (columns: DoctorId, SpecialityId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_DoctorSpecialityDetail]
	-- Add the parameters for the stored procedure here
@DoctorId INT = NULL,
@SpecialityId NVARCHAR(max) = NULL
AS
BEGIN

 


		INSERT INTO dbo.tblDoctorSpecialityDetail
		        ( DoctorId, SpecialityId )
		VALUES  ( @DoctorId,@SpecialityId)


END
```

---

## `sp_Save_Leave_HRIS_API`

**Parameters:**
- `@LeaveAvailId` — int, input = `0`
- `@LeaveInventoryId` — nvarchar (max), input = `NULL`
- `@FromDate` — Datetime NULL, input
- `@ToDate` — Datetime NULL, input
- `@EmpInfoId` — int, input = `NULL`
- `@AvailLeaveQty` — decimal(18,2) NUll, input
- `@LeaveReason` — nvarchar (max), input = `NULL`
- `@LeaveName` — nvarchar (max), input = `NULL`
- `@EntryBy` — nvarchar (max), input = `NULL`
- `@IsActive` — bit, input = `NULL`
- `@Status` — nvarchar (max), input = `NULL`
- `@ActionStatus` — nvarchar (max), input = `NULL`

**Tables read (FROM/JOIN):** ZASHRIS..tblEmpGeneralInfo, tblASMInfo, tblEmpGeneralInfo, tblLeaveAvail, tblLeaveAvailApp, tblMIOInfo, tblRoleType, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo
**Tables written:** INSERT → tblLeaveAvail, tblLeaveAvailApp; UPDATE → ZASHRIS..tblLeaveInventory, tblLeaveAvail; DELETE → tblLeaveAvail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from ZASHRIS..tblEmpGeneralInfo, tblASMInfo, tblEmpGeneralInfo, tblLeaveAvail, tblLeaveAvailApp, tblMIOInfo, tblRoleType, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo, then inserts into `tblLeaveAvail` (columns: EmpMasterCode, EmpName, LeaveInventoryId, FromDate, ToDate, EmpInfoId, AvailLeaveQty, LeaveReason, LeaveName, Status,...); inserts into `tblLeaveAvailApp` (columns: LeaveAvailId, PreEmpId, ForEmpId, ActionVersion, ApprovalStatus); updates `ZASHRIS..tblLeaveInventory` (sets: DayQty); updates `tblLeaveAvail` (sets: LeaveInventoryId, FromDate, ToDate, AvailLeaveQty, LeaveReason, LeaveName, Status, UpdateBy); deletes from `tblLeaveAvail`.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select ISNULL(max(ActionVersion),0)+1 from tblLeaveAvailApp where LeaveAvailId=@LeaveAvailId) ,@Status) end END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE  PROCEDURE [dbo].[sp_Save_Leave_HRIS_API]
	-- Add the parameters for the stored procedure here
	@LeaveAvailId int=0,
    @LeaveInventoryId  nvarchar (max)=NULL,
    @FromDate  Datetime NULL,
    @ToDate    Datetime NULL,
    @EmpInfoId int=NULL,
    @AvailLeaveQty decimal(18,2) NUll,  
	@LeaveReason nvarchar (max)=NULL,
	@LeaveName  nvarchar (max)=NULL,
	@EntryBy   nvarchar (max)=NULL,
	@IsActive  bit=NULL,
	@Status  nvarchar (max)=NULL,
	@ActionStatus nvarchar (max)=NULL


AS
    BEGIN
	  DECLARE @EmpMasterCode Nvarchar(MAX)

	SELECT @EmpMasterCode=EmpMasterCode FROM tblEmpGeneralInfo WHERE EmpInfoId=@EmpInfoId  
	declare @Roltp nvarchar(max)=null
	DECLARE @NextEmpId int =0
	DECLARE @TrId int =0

 if(@LeaveAvailId>0)
 begin
 --if(@IsActive=0)
 --begin
 --delete from [dbo].[tblLeaveAvail]
   
 --WHERE LeaveAvailId=@LeaveAvailId
 --end

 
	 select @Roltp=RoleType  FROM dbo.tblUser  A
	 LEFT JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 
LEFT join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=B.EmpMasterCode
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE B.EmpInfoId=@EntryBy  and UserStatus = 'Active' and IsAppsUser=1

     

	 
	if(@Roltp='AM')
	begin
	select @TrId=AreaId from tblASMInfo where IsActive=1 and EmployeeId=@EntryBy
		select  @NextEmpId= mm.EmpInfoId  FROM tblTagASMRSMInfo  mm WITH (NOLOCK)
INNER JOIN tblUser rr WITH (NOLOCK) ON mm.EmpInfoId = rr.EmpInfoId
WHERE TerritoryAreaId = @TrId AND mm.UserRoleId = 5 AND rr.UserStatus = 'Active'

	  set @Status='Verified'
				 
	end

	
	if(@Roltp='DZSM')
	begin
	select @TrId=CompanyId from tblEmpGeneralInfo where  EmpInfoId=@EntryBy
	 
	 
	  select top 1  @NextEmpId=B.EmpInfoId FROM dbo.tblUser  A
	 LEFT JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 
LEFT join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=B.EmpMasterCode
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE B.CompanyId= @TrId and  tblRoleType.RoleType='Wings Head'    and UserStatus = 'Active' and IsAppsUser=1
				  set @Status='Verified'
	end

	if(@Roltp='Wings Head')
	begin
	 set @Status='Approved'
 end
 
 UPDATE [dbo].[tblLeaveAvail]
   SET  [LeaveInventoryId] = @LeaveInventoryId 
      ,[FromDate] = @FromDate 
      ,[ToDate] = @ToDate 
     
      ,[AvailLeaveQty] = @AvailLeaveQty 
      ,[LeaveReason] = @LeaveReason 
      ,[LeaveName] = @LeaveName 
      ,[Status] = @Status 
    
      ,UpdateBy = @EntryBy 
      ,UpdateDate = GETDATE() 
    
      ,[IsActive] = @IsActive 
      ,[ActionStatus] = @ActionStatus , NextEmpId=@NextEmpId
 WHERE LeaveAvailId=@LeaveAvailId



			 INSERT INTO [dbo].[tblLeaveAvailApp]
           ([LeaveAvailId]
           ,[PreEmpId]
           ,[ForEmpId]
           ,[ActionVersion]
           ,[ApprovalStatus])
     VALUES
           (@LeaveAvailId
           ,@EntryBy
           ,@NextEmpId
           ,(select ISNULL(max(ActionVersion),0)+1 from  tblLeaveAvailApp where LeaveAvailId=@LeaveAvailId) 
           ,@Status)



		   if(@Status='Approved')
		   begin
		   
		     update     ZASHRIS..tblLeaveInventory set DayQty =DayQty-CONVERT(int,@AvailLeaveQty) WHERE EmpMasterCode=@EmpMasterCode  and LeaveYear=YEAR(@FromDate) and LeaveName=@LeaveName
 
		   end



end
	    else

		 begin


	 select @Roltp=RoleType  FROM dbo.tblUser  A
	 LEFT JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 
LEFT join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=B.EmpMasterCode
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE B.EmpInfoId=@EmpInfoId  and UserStatus = 'Active' and IsAppsUser=1

		  
	DECLARE @EmpName NVARCHAR(MAX)  
	

	if(@Roltp='MIO')
	begin
	select @TrId=TerritoryId from tblMIOInfo where IsActive=1 and EmployeeId=@EmpInfoId
		select  @NextEmpId= mm.EmpInfoId  FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE TerritoryAreaId=@TrId and  [UserRoleId]=3  

				 
	end


	if(@Roltp='AM')
	begin
	select @TrId=AreaId from tblASMInfo where IsActive=1 and EmployeeId=@EmpInfoId
		select  @NextEmpId= mm.EmpInfoId  FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE TerritoryAreaId=@TrId and  [UserRoleId]=5

	 

				 
	end

	
	if(@Roltp='DZSM')
	begin
	select @TrId=CompanyId from tblEmpGeneralInfo where  EmpInfoId=@EmpInfoId
	 
	 
	  select top 1  @NextEmpId=B.EmpInfoId FROM dbo.tblUser  A
	 LEFT JOIN dbo.tblEmpGeneralInfo B ON B.EmpInfoId = A.EmpInfoId
	 
LEFT join ZASHRIS..tblEmpGeneralInfo empHR on empHR.EmpMasterCode=B.EmpMasterCode
				LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = A.UserRoleID
				LEFT JOIN dbo.tblRoleType ON tblRoleType.RoleTypeId =tbl_UserRoleInfo.RoleTypeId  WHERE B.CompanyId= @TrId and  tblRoleType.RoleType='Wings Head'    and UserStatus = 'Active' and IsAppsUser=1
				  set @Status='Verified'
	end
 
 
  
 

	SELECT @EmpMasterCode=EmpMasterCode, @EmpName=EmpName from tblEmpGeneralInfo Where EmpInfoId=@EmpInfoId

	
		   insert into tblLeaveAvail ( EmpMasterCode,EmpName,LeaveInventoryId,FromDate,ToDate,EmpInfoId,AvailLeaveQty,LeaveReason,LeaveName,Status,EntryBy,EntryDate,IsActive,ActionStatus, RoleType_L,NextEmpId) 
            values (@EmpMasterCode,@EmpName,@LeaveInventoryId,@FromDate,@ToDate,@EmpInfoId,@AvailLeaveQty,@LeaveReason,@LeaveName,@Status,@EntryBy,GETDATE(),@IsActive,@ActionStatus, @Roltp,@NextEmpId)

			set @LeaveAvailId= SCOPE_IDENTITY()


			 INSERT INTO [dbo].[tblLeaveAvailApp]
           ([LeaveAvailId]
           ,[PreEmpId]
           ,[ForEmpId]
           ,[ActionVersion]
           ,[ApprovalStatus])
     VALUES
           (@LeaveAvailId
           ,@EmpInfoId
           ,@NextEmpId
           ,(select ISNULL(max(ActionVersion),0)+1 from  tblLeaveAvailApp where LeaveAvailId=@LeaveAvailId) 
           ,@Status)
		end

END
	
```

---

## `sp_Save_MenuRole`

**Parameters:**
- `@SL` — INT NULL, input
- `@RoleId` — INT NULL, input
- `@Add` — BIT NULL, input
- `@View` — BIT NULL, input
- `@Delete` — BIT NULL, input
- `@Edit` — BIT NULL, input
- `@Permission` — BIT NULL, input

**Tables read (FROM/JOIN):** tblMenuRole
**Tables written:** INSERT → tblMenuRole; DELETE → BIT, tblMenuRole

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblMenuRole, then inserts into `tblMenuRole` (columns: SL, RoleId, Add, View, Delete, Edit, Permission); deletes from `BIT`; deletes from `tblMenuRole`.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 CREATE PROCEDURE [dbo].[sp_Save_MenuRole]
	-- Add the parameters for the stored procedure here
    @SL INT NULL,
	    @RoleId INT NULL,
	    @Add BIT NULL,
	    @View BIT NULL,
	    @Delete BIT NULL,
	    @Edit BIT NULL,
		@Permission BIT NULL


AS
    BEGIN
	
		
	--IF EXISTS (SELECT SL FROM dbo.tblMenuRole WHERE RoleId=@RoleId AND SL=@SL)
	--BEGIN
	--    DELETE FROM dbo.tblMenuRole WHERE RoleId=@RoleId AND SL=@SL
	--END


	INSERT INTO dbo.tblMenuRole
	(
	    SL,
	    RoleId,
	    [Add],
	    [View],
	    [Delete],
	    Edit,Permission
	)
	VALUES
	(   @SL,
	    @RoleId,
	    @Add,
	    @View,
	    @Delete,
	    @Edit,@Permission
	    )
        

		SELECT SCOPE_IDENTITY()
END
  
    
```

---

## `sp_Save_PaymentBatchDetail`

**Parameters:**
- `@BatchMasterId` — INT, input
- `@TranscationDetailId` — INT, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → PaymentBatchDetails

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `PaymentBatchDetails` (columns: BatchMasterId, TranscationDetailId).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY(); END`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:454` — `aDbManager.SaveAction("sp_Save_PaymentBatchDetail", aSQL);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_PaymentBatchDetail]
	-- Add the parameters for the stored procedure here

    @BatchMasterId INT,
	@TranscationDetailId INT


AS
BEGIN   
	
	INSERT INTO [dbo].[PaymentBatchDetails]
           ([BatchMasterId]
           ,[TranscationDetailId])
     VALUES
           (@BatchMasterId
           ,@TranscationDetailId)

	SELECT SCOPE_IDENTITY();
		
END
```

---

## `sp_Save_PaymentBatchMaster`

**Parameters:**
- `@BatchMasterId` — INT, OUTPUT
- `@PayCustomerId` — INT, input
- `@DeliveryManId` — INT, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** PaymentBachMaster
**Tables written:** INSERT → PaymentBachMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from PaymentBachMaster, then inserts into `PaymentBachMaster` (columns: BatchNo, EntryBy, DeliveryManId, PayCustomerId).

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs:443` — `pk = aDbManager.SaveAction("sp_Save_PaymentBatchMaster", gSqlParameterList, "@BatchMasterId");`

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_PaymentBatchMaster]
	-- Add the parameters for the stored procedure here

	@BatchMasterId INT OUT,
	@PayCustomerId INT,
	@DeliveryManId INT,
    @EntryBy NVARCHAR(MAX)


AS
BEGIN   
	
	DECLARE @ProformaCode  NVARCHAR(500)
	DECLARE @ProCount INT = 0

	--DECLARE @BatchMasterId INT = 0
	
	SELECT @ProCount = ISNULL(MAX(BatchMasterId),0)+1  FROM PaymentBachMaster WHERE YEAR(BatchDate) = YEAR(GETDATE()) AND MONTH(BatchDate) = MONTH(GETDATE())

	--PRINT @ProCount

	DECLARE @length INT = 0
	SET @length = LEN(CONVERT(NVARCHAR(MAX),@ProCount))

	IF(@length = 1)
	BEGIN
	   SET @ProformaCode = 'PB-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '000' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 2)
	BEGIN
		SET @ProformaCode = 'PB-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '00' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 3)
	BEGIN
		SET @ProformaCode = 'PB-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '0' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE
	BEGIN
	   SET @ProformaCode ='PB-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + (CONVERT(NVARCHAR(MAX),@ProCount))
	END


	INSERT INTO [dbo].[PaymentBachMaster]
           ([BatchNo]
           ,[EntryBy],DeliveryManId,PayCustomerId)
     VALUES
           (@ProformaCode
           ,@EntryBy,@DeliveryManId,@PayCustomerId)

	SET @BatchMasterId = SCOPE_IDENTITY();

	--SELECT @BatchMasterId;
		
END
```

---

## `sp_Save_PrescriptionType`

**Parameters:**
- `@PrescriptionTypeId` — INT, input
- `@PrescriptionType` — NVARCHAR(MAX), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** tbl_PrescriptionType
**Tables written:** INSERT → tbl_PrescriptionType

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tbl_PrescriptionType, then inserts into `tbl_PrescriptionType` (columns: PrescriptionType, IsActive, ActiveInactiveDate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() End else Return 0 END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_PrescriptionType]
	-- Add the parameters for the stored procedure here
	@PrescriptionTypeId INT,
    @PrescriptionType NVARCHAR(MAX) ,
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)
AS
    BEGIN

	if not exists (select PrescriptionType from tbl_PrescriptionType where PrescriptionType=@PrescriptionType)
begin 
       INSERT  INTO [dbo].[tbl_PrescriptionType]
                ( PrescriptionType ,                
                  IsActive ,
                  ActiveInactiveDate ,
                  EntryBy ,
                  EntryDate 
	            )
        VALUES  ( @PrescriptionType ,              
                  @IsActive ,
                  @Activedate,
                  @EntryBy ,
                  GETDATE() 	
	            )

SELECT SCOPE_IDENTITY()


End
else  Return 0
	
        
END
```

---

## `sp_Save_SaveCustPayment`

**Parameters:**
- `@CustPayId` — NVARCHAR(MAX), input
- `@CustomerMasterId` — NVARCHAR(MAX), input
- `@PaymentDate` — DATETIME, input
- `@PaymentAmount` — decimal(18,2), input
- `@PayType` — NVARCHAR(MAX), input
- `@RefNo` — NVARCHAR(MAX), input
- `@RefDate` — NVARCHAR(MAX), input
- `@CreateBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblCustomerPay

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblCustomerPay` (columns: CustPayId, CustomerMasterId, PaymentDate, PaymentAmount, PayType, RefNo, RefDate, CreateBy, CreateDate, PaymentCode).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs:68` — `return aCommonInternalDal.RunStoreProcedure("sp_Save_SaveCustPayment", aSqlParameterList, "SSIDB");`

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_SaveCustPayment] ---- sp_DeliveryConformationFull 'INV-BD32-00001394','Test','29-Oct-2018'
	@CustPayId  NVARCHAR(MAX),
	@CustomerMasterId  NVARCHAR(MAX),
	@PaymentDate   DATETIME,
	@PaymentAmount decimal(18,2),
	@PayType  NVARCHAR(MAX),
	@RefNo NVARCHAR(MAX),
	@RefDate NVARCHAR(MAX),
	@CreateBy NVARCHAR(MAX)

AS
BEGIN


DECLARE @PlanCode  NVARCHAR(500)
	DECLARE @ProCount INT = 0
	SET @ProCount=@CustPayId

	DECLARE @length INT = 0
	SET @length = LEN(CONVERT(NVARCHAR(MAX),@ProCount))

	IF(@length = 1)
	BEGIN
	   SET @PlanCode = 'CP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '00000' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 2)
	BEGIN
		SET @PlanCode = 'CP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '0000' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 3)
	BEGIN
		SET @PlanCode = 'CP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '000' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 4)
	BEGIN
		SET @PlanCode = 'CP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '00' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE IF (@length = 5)
	BEGIN
		SET @PlanCode = 'CP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + '0' + (CONVERT(NVARCHAR(MAX),@ProCount))
	END
	ELSE
	BEGIN
	   SET @PlanCode ='CP-' + Right(Year(getDate()),2) + FORMAT(GETDATE(),'MM') + (CONVERT(NVARCHAR(MAX),@ProCount))
	END


	INSERT INTO dbo.tblCustomerPay
        ( CustPayId ,     
          CustomerMasterId ,       
          PaymentDate ,
          PaymentAmount ,
          PayType ,
          RefNo ,
          RefDate ,
          CreateBy ,
          CreateDate,
		  PaymentCode   
        )Values(
		
		@CustPayId  ,
	@CustomerMasterId  ,
	@PaymentDate   ,
	@PaymentAmount,
	@PayType  ,
	@RefNo ,
	@RefDate ,
	@CreateBy ,
	GETDATE(),
		@PlanCode
		)

END
```

---

## `sp_Save_ShiftInfos`

**Parameters:**
- `@ShiftId` — INT, input
- `@ShiftText` — NVARCHAR(MAX), input
- `@ShiftInTime` — TIME, input
- `@ShiftOutTime` — TIME, input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@EntryBy` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** tbl_Shift
**Tables written:** INSERT → tbl_Shift

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tbl_Shift, then inserts into `tbl_Shift` (columns: ShiftText, ShiftInTime, ShiftOutTime, IsActive, Activedate, EntryBy, EntryDate).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() End else Return 0 END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Save_ShiftInfos]
	-- Add the parameters for the stored procedure here
	@ShiftId INT,
    @ShiftText NVARCHAR(MAX) ,
    @ShiftInTime TIME ,
    @ShiftOutTime TIME ,
   
	@IsActive BIT,
	@Activedate DATETIME,
    @EntryBy NVARCHAR(MAX)


AS
    BEGIN

 
	if not exists (select ShiftText from tbl_Shift where ShiftText=@ShiftText)
begin 

INSERT INTO [dbo].[tbl_Shift]
           (ShiftText
           ,[ShiftInTime]
           ,[ShiftOutTime]
           ,[IsActive]
           ,[Activedate]
           ,[EntryBy]
           ,[EntryDate]
           )
     VALUES
           (@ShiftText 
           ,@ShiftInTime 
           ,@ShiftOutTime
           ,@IsActive
           ,@Activedate
           ,@EntryBy
           ,GETDATE() 	
           )
 
SELECT SCOPE_IDENTITY()
End
else  Return 0
	
 
		
END
```

---

## `sp_Save_UserCompanyUnit`

**Parameters:**
- `@UserId` — INT, input
- `@ComUnitId` — INT, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblUserCompanyUnit

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblUserCompanyUnit` (columns: UserId, CompanyUnitId, CWHPermission, NationalReportPermission).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_Save_UserCompanyUnit]
	-- Add the parameters for the stored procedure here

	@UserId INT,
	@ComUnitId INT

AS
BEGIN
	
	INSERT INTO [dbo].tblUserCompanyUnit
           (UserId
           ,CompanyUnitId,CWHPermission,NationalReportPermission)
     VALUES
           (@UserId
           ,@ComUnitId,0,0)
END
```

---

## `sp_Save_UserMaster_New`

**Parameters:**
- `@UserId` — INT, input
- `@UserName` — nvarchar (max), input = `NULL`
- `@UserType` — nvarchar (max), input = `NULL`
- `@LoginName` — nvarchar (max), input = `NULL`
- `@Password` — nvarchar (max), input = `NULL`
- `@IMEI_One` — nvarchar (max), input = `NULL`
- `@IMEI_Two` — nvarchar (max), input = `NULL`
- `@EmpInfoId` — int, input = `NULL`
- `@UserRoleID` — int, input = `NULL`
- `@UserTypeId` — int, input = `NULL`
- `@IsAppsUser` — bit, input = `NULL`
- `@ActiveInActiveDate` — datetime, input = `NULL`
- `@EntryBy` — nvarchar(50), input = `NULL`
- `@UserStatus` — nvarchar (max), input = `NULL`
- `@IsMainDashboard` — bit, input = `NULL`
- `@IsDepotDashboard` — bit, input = `NULL`
- `@IsHead` — bit, input = `NULL`
- `@IsRXUser` — bit, input = `NULL`
- `@IsCreditApproval` — bit, input = `NULL`

**Tables read (FROM/JOIN):** tblUser
**Tables written:** INSERT → tblUser

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblUser, then inserts into `tblUser` (columns: UserName, UserType, UserCode, LoginName, Password, EmpInfoId, IsAppsUser, IMEI_One, IMEI_Two, UserRoleID,...).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Save_UserMaster_New]
	-- Add the parameters for the stored procedure here
	@UserId INT,
    @UserName nvarchar (max)=NULL,
    @UserType nvarchar (max)=NULL,
    @LoginName nvarchar (max)=NULL,
    @Password nvarchar (max)=NULL,
    @IMEI_One nvarchar (max)=NULL,
    @IMEI_Two nvarchar (max)=NULL,
    @EmpInfoId int=NULL,
    @UserRoleID int=NULL,
    @UserTypeId int=NULL,
    
    
	@IsAppsUser bit=NULL,
    @ActiveInActiveDate datetime=NULL,
	@EntryBy nvarchar(50)=NULL,


	@UserStatus nvarchar (max)=NULL,
	@IsMainDashboard bit=NULL,
	@IsDepotDashboard bit=NULL,
	@IsHead bit=NULL,
	@IsRXUser bit=NULL,
	@IsCreditApproval bit=NULL

AS
    BEGIN
	
       DECLARE  @CampaignCode NVARCHAR(max)

SELECT @CampaignCode='U_'+ CAST(ISNULL(MAX(ISNULL(UserId,0)),0)+1001 AS NVARCHAR(max)) FROM dbo.[tblUser]

   
  INSERT INTO [dbo].[tblUser]
           ([UserName]
           ,[UserType]
           ,[UserCode]
           ,[LoginName]
           ,[Password]
         
          
           ,[EmpInfoId]
           ,[IsAppsUser]
           ,[IMEI_One]
           ,[IMEI_Two]
           ,[UserRoleID]
           ,[ActiveInActiveDate]
           ,[UserTypeId],EntryBy,EntryDate, UserStatus,IsMainDashboard,IsDepotDashboard, IsHead,IsCreditApproval, IsRXUser)
     VALUES
           ( @UserName 
           ,@UserType 
           ,@CampaignCode 
           ,@LoginName 
           ,@Password 
          
          
          
           ,@EmpInfoId 
           ,@IsAppsUser 
           ,@IMEI_One 
           ,@IMEI_Two 
           ,@UserRoleID 
         
           ,@ActiveInActiveDate
           ,@UserTypeId,@EntryBy,GETDATE(),@UserStatus,@IsMainDashboard,@IsDepotDashboard,@IsHead,@IsCreditApproval, @IsRXUser )

SELECT SCOPE_IDENTITY()
END
	
```

---

## `sp_Save_UserRoleInfo`

**Parameters:**
- `@UserRoleID` — INT, input
- `@RoleName` — NVARCHAR(500), input
- `@entryBy` — INT, input
- `@RoleTypeId` — INT, input
- `@isActive` — BIT, input
- `@acInAcDate` — DATETIME, input

**Tables read (FROM/JOIN):** tbl_UserRoleInfo
**Tables written:** INSERT → tbl_UserRoleInfo

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tbl_UserRoleInfo, then inserts into `tbl_UserRoleInfo` (columns: RoleName, IsActive, ActiveInActiveDate, EntryBy, EntryDate, RoleTypeId).

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT SCOPE_IDENTITY() END ELSE Return 0 END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Save_UserRoleInfo]
	
	-- Add the parameters for the stored procedure here

	@UserRoleID INT,
    @RoleName NVARCHAR(500),
    @entryBy INT,
    @RoleTypeId INT,
	@isActive BIT,
	@acInAcDate DATETIME

 AS
    BEGIN
	
	IF NOT EXISTS (select UserRoleID from tbl_UserRoleInfo where   RoleName = @RoleName )
    BEGIN 
        INSERT INTO tbl_UserRoleInfo
           (RoleName
           ,IsActive
           ,ActiveInActiveDate
           ,EntryBy
           ,EntryDate,RoleTypeId)
     VALUES
           (@RoleName,
			@isActive,
			@acInAcDate,
			@entryBy,
			GETDATE(),@RoleTypeId)

		SELECT SCOPE_IDENTITY()

			END
		ELSE  	
		Return 0
    END
```

---

## `sp_SaveEmpLatLong`

**Parameters:**
- `@Lat` — FLOAT, input
- `@Lon` — FLOAT, input
- `@EmpInfoId` — INT, input
- `@CompanyId` — INT, input
- `@EntryBy` — VARCHAR(50), input
- `@ShiftId` — INT, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** INSERT → tblEmpLatLong

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: inserts a row into `tblEmpLatLong` (columns: Lat, Lon, EmpInfoId, CompanyId, EntryBy, EntryDate, ShiftId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_SaveEmpLatLong]
(
     
    @Lat FLOAT,
    @Lon FLOAT,
    @EmpInfoId INT,
    @CompanyId INT,
    @EntryBy VARCHAR(50) ,
    @ShiftId INT  
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[tblEmpLatLong]
    (
        
        [Lat],
        [Lon],
        [EmpInfoId],
        [CompanyId],
        [EntryBy],
        [EntryDate], ShiftId
    )
    VALUES
    (
         
        @Lat,
        @Lon,
        @EmpInfoId,
        @CompanyId,
        @EntryBy,
       GETDATE(),@ShiftId
    );
END
```

---

## `sp_Summery_SalesSummerReportForAccounts`

**Parameters:**
- `@CompanyId` — INT, input = `NULL`
- `@fDate` — DATETIME, input
- `@tDate` — DATETIME, input
- `@ComUnitId` — INT, input = `NULL`

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster, RawReportData, tblCompanyInfo, tblCompanyUnit, tblCustPayDetail, tblCustomerReturnDetail, tblCustomerReturnMaster, tblInvoice, tblInvoiceDetail, tblSampleIssue
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining CSTL_ACCDB..tblDebitCreditVoucherDetail, CSTL_ACCDB..tblDebitCreditVoucherMaster, RawReportData, tblCompanyInfo, tblCompanyUnit, tblCustPayDetail, tblCustomerReturnDetail, tblCustomerReturnMaster, tblInvoice, tblInvoiceDetail, tblSampleIssue, filtered by @CompanyId, @ComUnitId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT CompanyId, ComUnitId, CompanyCode, CompanyName, ComUnitCode, ComUnitName, FromDate, ToDate, OpeningDue, SalesAmount, ReturnAmount, PaymentAmount, AIT, Discount, (OpeningDue + SalesAmount - ReturnAmount - PaymentAm`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs:59` — `return aCommonInternalDAL.GetDataTableAction("sp_Summery_SalesSummerReportForAccounts", aSqlParameterlist, "SSIDB");`

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Summery_SalesSummerReportForAccounts]
    @CompanyId INT = NULL,
    @fDate DATETIME,
    @tDate DATETIME,
    @ComUnitId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- CTE block to calculate Opening/Closing balances and period transactions
    WITH RawReportData AS (
        SELECT 
            CI.CompanyId,
            UNT.ComUnitId,
            CI.CompanyCode,
            CI.CompanyName,
            UNT.ComUnitCode,
            UNT.ComUnitName,
            @fDate AS FromDate,
            @tDate AS ToDate,
            
            -- ?. ?????????? ????? (Opening Due before @fDate)
            (
                ISNULL((SELECT SUM(IVD.DeliveryNetAmount)
                 FROM tblInvoice AS INV WITH (NOLOCK)
                 INNER JOIN tblInvoiceDetail AS IVD WITH (NOLOCK) ON INV.InvoiceId = IVD.InvoiceId
                 WHERE INV.ComUnitId = UNT.ComUnitId AND CONVERT(DATE, INV.UpdateDate) < @fDate AND INV.DeliveryInvoiceStatus IN ('Full','Partial') AND IVD.Quantity > 0), 0)
                -
                ISNULL((SELECT SUM(RTD.ReturnAmount)
                 FROM tblCustomerReturnMaster AS RTN WITH (NOLOCK)
                 INNER JOIN tblCustomerReturnDetail AS RTD WITH (NOLOCK) ON RTN.CustReturnId = RTD.CustReturnId
                 INNER JOIN tblInvoice AS INV WITH (NOLOCK) ON RTN.InvoiceId = INV.InvoiceId
                 WHERE INV.ComUnitId = UNT.ComUnitId AND CONVERT(DATE, RTN.ApprovedDate) < @fDate), 0)
                -
                ISNULL((SELECT SUM(CPD.PaymentAmount + ISNULL(CPD.Discount, 0) + ISNULL(CPD.AIT, 0))
                 FROM tblCustPayDetail AS CPD WITH (NOLOCK)
                 INNER JOIN tblInvoice AS INV WITH (NOLOCK) ON CPD.InvoiceId = INV.InvoiceId
                 WHERE INV.ComUnitId = UNT.ComUnitId AND CONVERT(DATE, CPD.ProcessTime) < @fDate), 0)
            ) AS OpeningDue,

            ISNULL(SLS.SalesAmount, 0) AS SalesAmount,
            ISNULL(RTN.ReturnAmount, 0) AS ReturnAmount,
            ISNULL(PM.PaymentAmount, 0) AS PaymentAmount,
            ISNULL(PM.AIT, 0) AS AIT,
            ISNULL(PM.Discount, 0) AS Discount,
            ISNULL(SMP.SampleExpense, 0) AS SampleExpense
        FROM tblCompanyUnit AS UNT
        LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId

        -- ??? ??????? ?????? ??? ???? (Sales during range)
        LEFT JOIN (
            SELECT INV.ComUnitId, SUM(DeliveryNetAmount) AS SalesAmount
            FROM tblInvoice AS INV WITH (NOLOCK)
            INNER JOIN tblInvoiceDetail AS IVD WITH (NOLOCK) ON INV.InvoiceId = IVD.InvoiceId
            WHERE CONVERT(DATE, INV.UpdateDate) BETWEEN @fDate AND @tDate
              AND INV.DeliveryInvoiceStatus IN ('Full','Partial')
              AND IVD.Quantity > 0
            GROUP BY INV.ComUnitId
        ) AS SLS ON UNT.ComUnitId = SLS.ComUnitId

        -- ??? ??????? ?????? ??? ???????/??????? (Payment during range)
        LEFT JOIN (
            SELECT INV.ComUnitId, 
                   SUM(CPD.PaymentAmount) AS PaymentAmount,
                   SUM(ISNULL(CPD.AIT, 0)) AS AIT,
                   SUM(ISNULL(CPD.Discount, 0)) AS Discount
            FROM tblCustPayDetail AS CPD WITH (NOLOCK)
            INNER JOIN tblInvoice AS INV WITH (NOLOCK) ON CPD.InvoiceId = INV.InvoiceId
            WHERE CONVERT(DATE, CPD.ProcessTime) BETWEEN @fDate AND @tDate
            GROUP BY INV.ComUnitId
        ) AS PM ON UNT.ComUnitId = PM.ComUnitId

        -- ??? ??????? ?????? ??? ??????? (Returns during range)
        LEFT JOIN (
            SELECT INV.ComUnitId, SUM(RTD.ReturnAmount) AS ReturnAmount
            FROM tblCustomerReturnMaster AS RTN WITH (NOLOCK)
            INNER JOIN tblCustomerReturnDetail AS RTD WITH (NOLOCK) ON RTN.CustReturnId = RTD.CustReturnId
            INNER JOIN tblInvoice AS INV WITH (NOLOCK) ON RTN.InvoiceId = INV.InvoiceId
            WHERE CONVERT(DATE, RTN.ApprovedDate) BETWEEN @fDate AND @tDate
            GROUP BY INV.ComUnitId
        ) AS RTN ON UNT.ComUnitId = RTN.ComUnitId

        -- ???????? ????????? (?? ?????? ??????? ?????/?????? ??????????? ???? ?????)
        LEFT JOIN (
            SELECT SI.ComUnitId, SUM(ISNULL(VD.DebitAmountBDT, 0)) AS SampleExpense
            FROM CSTL_ACCDB.dbo.tblDebitCreditVoucherMaster VM WITH (NOLOCK)
            INNER JOIN CSTL_ACCDB.dbo.tblDebitCreditVoucherDetail VD WITH (NOLOCK) ON VM.VoucherMasterId = VD.VoucherMasterId
            INNER JOIN tblSampleIssue SI WITH (NOLOCK) ON VM.CommonPK_ID = SI.OrderId
            WHERE VD.AccountId = 4164
              AND VM.TableID = 2015
              AND CONVERT(DATE, VM.PostingDate) BETWEEN @fDate AND @tDate
              AND VM.ActionStatus IN ('Accepted', 'Approved')
            GROUP BY SI.ComUnitId
        ) AS SMP ON UNT.ComUnitId = SMP.ComUnitId

        WHERE CI.CompanyId IS NOT NULL 
          AND (ISNULL(@CompanyId, 0) = 0 OR CI.CompanyId = @CompanyId)
          AND (ISNULL(@ComUnitId, 0) = 0 OR UNT.ComUnitId = @ComUnitId)
    )
    SELECT 
        CompanyId,
        ComUnitId,
        CompanyCode,
        CompanyName,
        ComUnitCode,
        ComUnitName,
        FromDate,
        ToDate,
        OpeningDue,
        SalesAmount,
        ReturnAmount,
        PaymentAmount,
        AIT,
        Discount,
        -- ?. ?????? ????? (Closing Due = Opening + Sales - Returns - Payments - Discounts)
        (OpeningDue + SalesAmount - ReturnAmount - PaymentAmount - AIT - Discount) AS ClosingDue,
        SampleExpense
    FROM RawReportData
    WHERE (
        ABS(OpeningDue) +
        SalesAmount +
        ReturnAmount +
        PaymentAmount +
        AIT +
        Discount +
        SampleExpense
    ) > 0
    ORDER BY CompanyId, ComUnitName;
END
```

---

## `sp_Suport_IsInvoiceCorrectionOnOrder`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** tblInvoice, tblOrder
**Tables written:** UPDATE → tblOrder

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from tblInvoice, tblOrder, and per row updates tblOrder.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Suport_IsInvoiceCorrectionOnOrder] 

	
	
AS
BEGIN
	
	DECLARE @OrderId INT
        
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	
	SELECT OrderId FROM tblOrder WHERE IsInvoice = 0 AND OrderId IN (SELECT OrderId FROM tblInvoice WHERE OrderId is not null)
		
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @OrderId
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	
	UPDATE tblOrder SET IsInvoice = 1 WHERE OrderId = @OrderId
	
	
	
	FETCH NEXT FROM @MyCursor
	INTO @OrderId
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor




	
END
```

---

## `sp_Test_GenerateC#ModelFromTable`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** sys..columns, sys..types
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining sys..columns, sys..types, filtered by @Result.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT REPLACE(col.name, ' ', '_') ColumnName , column_id ColumnId , CASE typ.name WHEN 'bigint' THEN 'long' WHEN 'binary' THEN 'byte[]' WHEN 'bit' THEN 'bool' WHEN 'char' THEN 'string' WHEN 'date' THEN 'DateTime' WHEN 'datetime' THEN 'DateTime' WHEN 'datetime2' THEN 'DateTime' WHEN 'datetimeoffset' THEN 'DateTimeOffset' WHEN 'decimal' THEN 'decima`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_Test_GenerateC#ModelFromTable]  -- exec sp_Test_GenerateC#ModelFromTable
-- Add the parameters for the stored procedure here
AS
BEGIN


DECLARE @TableName sysname = 'tblDcWiseTerritoryMaster'
DECLARE @className NVARCHAR(200)= SUBSTRING(@TableName,
LEN(RIGHT(@TableName,
CHARINDEX('_',
@TableName)))
+ 1, LEN(@TableName));
DECLARE @Result VARCHAR(MAX) = 'public class ' + @className + '
{'

SELECT @Result = @Result + '
public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }
'
FROM ( SELECT REPLACE(col.name, ' ', '_') ColumnName ,
column_id ColumnId ,
CASE typ.name
WHEN 'bigint' THEN 'long'
WHEN 'binary' THEN 'byte[]'
WHEN 'bit' THEN 'bool'
WHEN 'char' THEN 'string'
WHEN 'date' THEN 'DateTime'
WHEN 'datetime' THEN 'DateTime'
WHEN 'datetime2' THEN 'DateTime'
WHEN 'datetimeoffset' THEN 'DateTimeOffset'
WHEN 'decimal' THEN 'decimal'
WHEN 'float' THEN 'double'
WHEN 'image' THEN 'byte[]'
WHEN 'int' THEN 'int'
WHEN 'money' THEN 'decimal'
WHEN 'nchar' THEN 'string'
WHEN 'ntext' THEN 'string'
WHEN 'numeric' THEN 'decimal'
WHEN 'nvarchar' THEN 'string'
WHEN 'real' THEN 'float'
WHEN 'smalldatetime' THEN 'DateTime'
WHEN 'smallint' THEN 'short'
WHEN 'smallmoney' THEN 'decimal'
WHEN 'text' THEN 'string'
WHEN 'time' THEN 'TimeSpan'
WHEN 'timestamp' THEN 'long'
WHEN 'tinyint' THEN 'byte'
WHEN 'uniqueidentifier' THEN 'Guid'
WHEN 'varbinary' THEN 'byte[]'
WHEN 'varchar' THEN 'string'
ELSE 'UNKNOWN_' + typ.name
END ColumnType ,
CASE WHEN col.is_nullable = 1
AND typ.name IN ( 'bigint', 'bit',
'date', 'datetime',
'datetime2',
'datetimeoffset',
'decimal', 'float',
'int', 'money',
'numeric', 'real',
'smalldatetime',
'smallint',
'smallmoney', 'time',
'tinyint',
'uniqueidentifier' )
THEN '?'
ELSE ''
END NullableSign
FROM sys.columns col
JOIN sys.types typ ON col.system_type_id = typ.system_type_id
AND col.user_type_id = typ.user_type_id
WHERE object_id = OBJECT_ID(@TableName)
) t
ORDER BY ColumnId

SET @Result = @Result + '
}'

PRINT @Result



END
```

---

## `sp_UD_CustomerCreditLimitExtension`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** tblCompanyInfo, tblCompanyUnit, tblCustomerCreditLimit, tblInvoice
**Tables written:** UPDATE → tblCustomerCreditLimit

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from tblCompanyInfo, tblCompanyUnit, tblCustomerCreditLimit, tblInvoice, and per row updates tblCustomerCreditLimit.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UD_CustomerCreditLimitExtension] 

AS
BEGIN
	  

	DECLARE @CustomerId INT
	DECLARE @CompanyId INT
	DECLARE @CreditLimitId INT
	DECLARE @EndDate DATETIME
	        
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	
	SELECT CD.CreditLimitId,CD.CompanyId,CD.CustomerMasterId,CD.EndDate 
	FROM dbo.tblCustomerCreditLimit AS CD WHERE CD.ActionStatus IN ('Accepted') AND CD.EndDate IS NOT NULL 
	AND CONVERT(DATE,GETDATE()) = CD.EndDate
	
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @CreditLimitId,@CompanyId,@CustomerId,@EndDate
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		DECLARE @Due DECIMAL(18,2)
		SET @Due = 0
	    
		-- Check Due Status

		SELECT @Due = SUM(DeliveryTpGrandTotal-ISNULL(PaymentAmount,0)) FROM dbo.tblInvoice AS IV
	    LEFT JOIN dbo.tblCompanyUnit ON  tblCompanyUnit.ComUnitId = IV.ComUnitId 
	    LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
	    WHERE DeliveryInvoiceStatus IN ('FULL','Partial') AND IV.CustomerMasterId = @CustomerId AND tblCompanyInfo.CompanyId = @CompanyId
	
		IF(@Due = 0)
			
			BEGIN
				UPDATE tblCustomerCreditLimit SET EndDate = DATEADD(DAY,30,@EndDate) WHERE CreditLimitId = @CreditLimitId
			END
	
	
	FETCH NEXT FROM @MyCursor
	INTO @CreditLimitId,@CompanyId,@CustomerId,@EndDate
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor

END
```

---

## `sp_UD_CustomerGl`

**Parameters:**
- `@CustomerId` — INT, input
- `@SubsidiaryLId` — INT, input

**Tables read (FROM/JOIN):** ZAS_ACCDB..tblChartOfAccounts, ZAS_ACCDB..tblSubsubsidiaryLadger, tblCustMaster
**Tables written:** INSERT → ZAS_ACCDB..tblCOACompanyPermission, ZAS_ACCDB..tblChartOfAccounts; UPDATE → ZAS_ACCDB..tblChartOfAccounts

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from ZAS_ACCDB..tblChartOfAccounts, ZAS_ACCDB..tblSubsubsidiaryLadger, tblCustMaster, and per row inserts into ZAS_ACCDB..tblCOACompanyPermission, ZAS_ACCDB..tblChartOfAccounts; updates ZAS_ACCDB..tblChartOfAccounts.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT AccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE CustomerId = @CustomerId ) END FETCH NEXT FROM @MyCursor INTO @CstmrId , @CstmrCode , @CstmrName END CLOSE @MyCursor DEALLOCATE @MyCursor END`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:441` — `return aCommonInternalDal.UpdateAction("sp_UD_CustomerGl", aSqlParameterlist);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_UD_CustomerGl] 

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

	--SELECT @HasName=ISNULL(COUNT(*),0) from ZAS_ACCDB..tblChartOfAccounts
	--WHERE AccountName = @AccountName

	IF(@HasName =0)
	BEGIN
		--SELECT @AccountCode=(CONVERT(NVARCHAR(MAX),(COUNT(AccountId)+10001))) FROM ZAS_ACCDB..tblChartOfAccounts 
		
		--SELECT @SubsidiryLCode = SubsubsidiryLCode FROM ZAS_ACCDB..tblSubsubsidiaryLadger WHERE SubSubsidiaryId = @SubsidiaryLId
		--SET @AccountCode=@SubsidiryLCode+@AccountCode
		
		--insert into ZAS_ACCDB..tblChartOfAccounts (AccountCode,Description,AccountName,SubsidiaryLId,CompanyId,CreateBy,CreateDate,IsActive,InBalanceSheet,InIncomeStatement,CustomerId)
  --                                 values (@AccountCode,@Description,@AccountName,@SubsidiaryLId,0,'Auto Process',GETDATE(),'True','False','True',@CstmrId)

		--SET @AccountId = SCOPE_IDENTITY()

		--INSERT INTO ZAS_ACCDB..tblCOACompanyPermission (BankAccountId,CompanyId) VALUES (@AccountId, 0)

		UPDATE ZAS_ACCDB..tblChartOfAccounts SET AccountName = @AccountName, Description = @Description WHERE AccountId IN (
			SELECT AccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE CustomerId =  @CustomerId
		) 
	                                   
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

---

## `sp_UD_InvoicePayment`

**Parameters:**
- `@InvoiceId` — INT, input

**Tables read (FROM/JOIN):** tblCustPayDetail, tblInvoice, tblInvoiceDetail
**Tables written:** UPDATE → tblInvoice

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from tblCustPayDetail, tblInvoice, tblInvoiceDetail, and per row updates tblInvoice.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/CustPaymentDAL.cs:183` — `return aCommonInternalDal.UpdateAction("sp_UD_InvoicePayment", aSqlParameterlist);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_UD_InvoicePayment] 

	@InvoiceId INT

AS
BEGIN


	--DECLARE @InvoiceId INT = 583392
	
    Declare @DeliveryTpGrandTotal DECIMAL(18,2)
	DECLARE @PaymentAmount DECIMAL(18,2)
	Declare @AIT DECIMAL(18,2)
	Declare @Discount DECIMAL(18,2)
	DECLARE @TotalPaid DECIMAL(18,2)
    Declare @CursorInvoiceId INT
	        
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT INV.InvoiceId,ISNULL(DeliveryNetAmount,0) DeliveryNetAmount,ISNULL(CustPaymentAmount,0) CustPaymentAmount,
	ISNULL(CP.AIT,0) AIT,ISNULL(Discount,0) Discount,ISNULL(CustPaymentAmount,0) + ISNULL(CP.AIT,0) + ISNULL(Discount,0) AS TotalPaid FROM tblInvoice AS INV
	LEFT JOIN (SELECT InvoiceId,SUM(DeliveryNetAmount) DeliveryNetAmount FROM tblInvoiceDetail GROUP BY InvoiceId) AS IVD ON INV.InvoiceId = IVD.InvoiceId
	LEFT JOIN (SELECT InvoiceId,SUM(CPD.PaymentAmount) AS CustPaymentAmount, SUM(Isnull(CPD.AIT,0)) AIT, SUM(Isnull(CPD.Discount,0)) Discount 
	FROM tblCustPayDetail AS CPD GROUP BY InvoiceId) AS CP ON INV.InvoiceId = CP.InvoiceId
	WHERE INV.InvoiceId IS NOT NULL 
	
	--AND InvoiceDate BETWEEN '2025-02-01' AND '2025-03-06'
	
	AND INV.InvoiceId = @InvoiceId -- 583392

    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @CursorInvoiceId,@DeliveryTpGrandTotal,@PaymentAmount, @AIT, @Discount,@TotalPaid
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		UPDATE dbo.tblInvoice SET PaymentAmount = @TotalPaid, AIT = @AIT, DiscountOnPayment = @Discount, 
		PaymentStatus = CASE WHEN @TotalPaid >= @DeliveryTpGrandTotal THEN 'Full' ELSE 'Partial' END  WHERE InvoiceId = @CursorInvoiceId

    FETCH NEXT FROM @MyCursor
    INTO @CursorInvoiceId,@DeliveryTpGrandTotal,@PaymentAmount, @AIT, @Discount,@TotalPaid 
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor


	
END
```

---

## `sp_UD_OrderIsSpecialApproval`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerCreditLimit, tblInvoice, tblOrder
**Tables written:** UPDATE → tblOrder

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerCreditLimit, tblInvoice, tblOrder, and per row updates tblOrder.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT InvoiceId,ISNULL(SUM(PaymentAmount),0)Amount FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp ON tbltemp.InvoiceId = tblInvoice.InvoiceId WHERE CustomerMasterId=@CstmrId AND PaymentStatus<>'Full')AS tbltemp IF(ISNull(@Due,0) > 0) BEGIN UPDATE tblOrder SET IsSpecialApproval = 1 WHERE OrderId = @OrderId`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_UD_OrderIsSpecialApproval] 

AS
BEGIN
	
	DECLARE @CstmrId INT
	DECLARE @OrderId INT
    Declare @GrossValue DECIMAL
	DECLARE @TermOfPayment NVARCHAR(MAX)
            
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT ODR.OrderId,CST.CustomerMasterId,GrossValue,TermOfPayment FROM tblOrder AS ODR 
	LEFT JOIN tblCustMaster AS CST ON ODR.CustomerCode = CST.CustomerCode
	LEFT JOIN tblCompanyUnit AS UNT ON ODR.ComUnitId = UNT.ComUnitId
	LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
	WHERE IsFromApp = 1 and IsInvoice = 0 

    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @OrderId,@CstmrId ,
       @GrossValue,
	   @TermOfPayment 
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
	DECLARE @LimitAmount DECIMAL
	DECLARE @Due DECIMAL
	DECLARE @RemainAmount DECIMAL



	SELECT @LimitAmount = LimitAmount FROM dbo.tblCustomerCreditLimit
    LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
    WHERE tblCustMaster.CustomerMasterId = @CstmrId

	SELECT @Due = ISNULL(SUM(DeliveryTpGrandTotal-ISNULL(PaymentAmount,0)),0) FROM dbo.tblInvoice 
    WHERE (DeliveryInvoiceStatus IS NOT NULL OR DeliveryInvoiceStatus != 'Reject') AND CustomerMasterId = @CstmrId


	SELECT @RemainAmount = SUM(tbltemp.RemainAmount) FROM (SELECT ISNULL(SUM(GrossValue),0)RemainAmount FROM dbo.tblOrder WHERE (IsInvoice IS NULL OR IsInvoice='0') AND CustomerMasterId=@CstmrId
	UNION ALL 
	SELECT SUM(DeliveryTpGrandTotal-ISNULL(tbltemp.Amount,0))RemainAmount FROM dbo.tblInvoice 
	LEFT JOIN  
	(SELECT InvoiceId,ISNULL(SUM(PaymentAmount),0)Amount FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp ON tbltemp.InvoiceId = tblInvoice.InvoiceId 
	WHERE CustomerMasterId=@CstmrId AND PaymentStatus<>'Full')AS tbltemp


	IF(ISNull(@Due,0) > 0)
	BEGIN	

		UPDATE tblOrder SET IsSpecialApproval = 1 WHERE OrderId = @OrderId  
		                                 
	END

	IF(@TermOfPayment = 'Credit' AND (ISNULL(@GrossValue,0) + ISNULL(@Due,0)) > @RemainAmount)
	BEGIN	

		UPDATE tblOrder SET IsSpecialApproval = 1 WHERE OrderId = @OrderId  
		                                 
	END
	
    FETCH NEXT FROM @MyCursor
    INTO @OrderId,@CstmrId ,
       @GrossValue,
	   @TermOfPayment
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

---

## `sp_UD_ReceiveIdInDc`

**Parameters:**
- `@DCStoreId` — INT, input

**Tables read (FROM/JOIN):** tblCentralStore, tblDCStore, tblStockInTransfar
**Tables written:** UPDATE → DCS

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblCentralStore, tblDCStore, tblStockInTransfar, then updates `DCS` (sets: DCS.ReceiveId, DCS.UnitPrice_CostPrice).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/RequisitionDAL.cs:519` — `return aCommonInternalDal.UpdateAction("sp_UD_ReceiveIdInDc", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =======================================================================================
-- 3. sp_UD_ReceiveIdInDc (Inherits CS WAC Rate into tblDCStore.UnitPrice_CostPrice)
-- =======================================================================================
CREATE   PROCEDURE [dbo].[sp_UD_ReceiveIdInDc]
    @DCStoreId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE DCS
    SET 
        DCS.ReceiveId = ST.ReceiveId,
        DCS.UnitPrice_CostPrice = ISNULL(CS.Avg_Rate, ISNULL(CS.UnitPrice, DCS.UnitPrice_CostPrice))
    FROM dbo.tblDCStore DCS
    INNER JOIN dbo.tblStockInTransfar ST WITH (NOLOCK) ON DCS.StockInTransfarId = ST.StockInTransfarId
    INNER JOIN dbo.tblCentralStore CS WITH (NOLOCK) ON ST.ReceiveId = CS.ReceiveId
    WHERE DCS.DCStoreId = @DCStoreId;
END;
```

---

## `sp_UD_ReceiveIdInDcOpeningBalance`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** tblDCStore
**Tables written:** UPDATE → tblDCStore_OpeningBalance

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from tblDCStore, and per row updates tblDCStore_OpeningBalance.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UD_ReceiveIdInDcOpeningBalance] 

AS
BEGIN
	  
	--FOR Stock
	DECLARE @DcStoreId INT
	DECLARE @TempReceiveId INT
	        
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	
	SELECT DCStoreId,TempReceiveId FROM dbo.tblDCStore WHERE TempReceiveId IS NOT NULL

	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @DcStoreId,@TempReceiveId
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	

	  UPDATE tblDCStore_OpeningBalance SET TempReceiveId = @TempReceiveId WHERE DCStoreId = @DcStoreId
	
	FETCH NEXT FROM @MyCursor
	INTO @DcStoreId,@TempReceiveId 
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor    

END




--SELECT TempReceiveId FROM tblDCStore WHERE DCStoreId = 9237
```

---

## `sp_UD_ResetPaymentExtra`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** tblCustPayDetail, tblInvoice
**Tables written:** UPDATE → tblInvoice

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from tblCustPayDetail, tblInvoice, and per row updates tblInvoice.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,02/08/2016,>
-- Description:	<Description,,>
-- ============================================

CREATE PROCEDURE [dbo].[sp_UD_ResetPaymentExtra] 


AS
BEGIN
	
	DECLARE @InvoiceId INT
	DECLARE @Payment DECIMAL(18,2)
	DECLARE @DeliveryTpGrandTotal DECIMAL(18,2)
	        
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	
	SELECT InvoiceId,DeliveryTpGrandTotal FROM tblInvoice WHERE DeliveryTpGrandTotal < ISnull(PaymentAmount,0) 
	AND DeliveryInvoiceStatus IN ('Full','Partial') AND DeliveryTpGrandTotal > 0
		
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @InvoiceId,@DeliveryTpGrandTotal
	
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
			SET @Payment = 0;
			SELECT @Payment = ISNULL(SUM(PaymentAmount),0) FROm tblCustPayDetail WHERE InvoiceId = @InvoiceId
			
			UPDATE tblInvoice SET PaymentAmount = @Payment, 
			PaymentStatus = (CASE WHEN @Payment =  @DeliveryTpGrandTotal THEN 'Full' ELSE 'Partial' END) 
			WHERE InvoiceId = @InvoiceId
		
		FETCH NEXT FROM @MyCursor
		INTO @InvoiceId,@DeliveryTpGrandTotal
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor
	
END
```

---

## `sp_UD_SampleIssue`

**Parameters:**
- `@MasterId` — int, input
- `@ActionStatus` — NVARCHAR(500), input
- `@ApprovedBy` — NVARCHAR(MAX), input
- `@ApprovedDate` — DATETIME, input

**Tables read (FROM/JOIN):** CSTL_ACCDB..tblFinancialYear, Table_Inventory_TranscationTypes, Table_Inventory_Types, Weighted, tblCentralStore, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblDCStore, tblProduct, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation, tbl_INV_BatchWiseStock, tbl_INV_WeightedAvg
**Tables written:** INSERT → Batch, tblSampleIssueTranscation, tblStockTrannsctionForSample, tbl_INV_BatchWiseStock, tbl_INV_WeightedAvg; UPDATE → UPDATE, tblDCStore

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from CSTL_ACCDB..tblFinancialYear, Table_Inventory_TranscationTypes, Table_Inventory_Types, Weighted, tblCentralStore, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblDCStore, tblProduct, tblSampleIssue, tblSampleIssueDetail, tblSampleIssueTranscation, tbl_INV_BatchWiseStock, tbl_INV_WeightedAvg, and per row inserts into Batch, tblSampleIssueTranscation, tblStockTrannsctionForSample, tbl_INV_BatchWiseStock, tbl_INV_WeightedAvg; updates UPDATE, tblDCStore.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT TOP 1 Close_Amt FROM tbl_INV_WeightedAvg WHERE Company_Id = @CompanyId AND Item_Id = @ProductId ORDER BY Trans_Date DESC), 0) SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty E`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/SampleIssueDal.cs:449` — `return aInternalDal.UpdateAction("sp_UD_SampleIssue", aSqlParameterlist);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UD_SampleIssue] 
(
		@MasterId int,
		@ActionStatus NVARCHAR(500),
		@ApprovedBy NVARCHAR(MAX),
		@ApprovedDate DATETIME
)
AS
BEGIN
	

	--declare @MasterId int = 9571
	--declare @ActionStatus NVARCHAR(500) = 'Accepted'
	--declare @ApprovedBy NVARCHAR(MAX) = 'Rezaul'
	--declare @ApprovedDate DATETIME = '2025-11-06'
	
	DECLARE @CurrentApprovalStatus NVARCHAR(300) = ''
	SELECT @CurrentApprovalStatus = ISNULL(ActionStatus,'Posted') FROM tblSampleIssue WHERE  OrderId=@MasterId
 

	IF(LTRIM(RTRIM(UPPER(@CurrentApprovalStatus))) = LTRIM(RTRIM(UPPER('Posted')))) -- Chek Is Posted or Not
	BEGIN

			-- Update 

		UPDATE tblSampleIssue 
		SET ActionStatus=@ActionStatus,
			ApprovedBy=@ApprovedBy,
			ApprovedDate=@ApprovedDate 
		WHERE OrderId=@MasterId

		

		IF(LTRIM(RTRIM(UPPER(@ActionStatus))) = LTRIM(RTRIM(UPPER('Accepted')))) -- Accepted Ststus Check Stsrt
		BEGIN

			-- First Cursor Start

			DECLARE @CompanyId INT
			DECLARE @FinancialYearId INT
			DECLARE @CustomerName NVARCHAR(MAX)
			DECLARE @ProductId INT
			DECLARE @Quantity DECIMAL(18, 3) 
			DECLARE	@ComUnitId int
			DECLARE	@OrderDetailsId int
			DECLARE @CustomerMasterId INT

			DECLARE @SubmissionDate DATETIME
			
			DECLARE @TranscationTypeId INT
			DECLARE @InventoryTypeId INT
			DECLARE @InventoryType NVARCHAR(200)

			DECLARE @OrderCode NVARCHAR(MAX)


			DECLARE @MyCursor CURSOR
			SET @MyCursor = CURSOR FAST_FORWARD
			FOR
			---------------


			SELECT CI.CompanyId, FY.FinancialYearId,CSTMR.CustomerName + ' (' + CSTMR.CustomerCode + ')' AS Customer,ISSM.ComUnitId,ProductId,ISSD.Quantity,
			ISSD.OrderDetailId,TSC.TranscationTypeId,ITS.InventoryTypeId,ITS.InventoryType,ISSM.OrderCode,CSTMR.CustomerMasterId, SubmissionDate FROM tblSampleIssue AS ISSM 
			LEFT JOIN tblSampleIssueDetail AS ISSD ON ISSM.OrderId = ISSD.OrderId
			LEFT JOIN tblCustMaster AS CSTMR ON ISSM.CustomerMasterId = CSTMR.CustomerMasterId
			LEFT JOIN tblCompanyUnit AS UNT ON ISSM.ComUnitId = UNT.ComUnitId
			LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
			INNER JOIN CSTL_ACCDB..tblFinancialYear AS FY ON CI.CompanyId = FY.CompanyId AND ISSM.SubmissionDate BETWEEN FY.StartDate AND FY.EndDate
			LEFT JOIN Table_Inventory_TranscationTypes AS TSC ON LTRIM(RTRIM(UPPER(RespectiveTable))) = LTRIM(RTRIM(UPPER('tblSampleIssue')))
			LEFT JOIN Table_Inventory_Types AS ITS ON ITS.InventoryTypeId = 1 -- Finished Good (FG)
			WHERE ISSM.OrderId = @MasterId -- 8664

			
			----------

			OPEN @MyCursor -- Start
			FETCH NEXT FROM @MyCursor
			INTO @CompanyId, @FinancialYearId, @CustomerName,@ComUnitId,@ProductId,@Quantity,@OrderDetailsId,
			@TranscationTypeId,@InventoryTypeId,@InventoryType,@OrderCode,@CustomerMasterId, @SubmissionDate


			-------------

			WHILE @@FETCH_STATUS = 0
			BEGIN



				-- Second Courser Start

				DECLARE @TempQty DECIMAL(18,3)
				DECLARE @CostPerUnit DECIMAL(18,4)
				DECLARE @BatchNo NVARCHAR(200)
				DECLARE @MMFGDate DATETIME
				DECLARE @EXPDate DATETIME

				DECLARE @ProductCode NVARCHAR(300)

				--FOR Stock

				DECLARE @DcStoreId INT
				DECLARE @StockQuantity DECIMAL(18,2)
				SET @TempQty = @Quantity
				        
				--------------------------------------------------------
				DECLARE @MyCursor2 CURSOR
				SET @MyCursor2 = CURSOR FAST_FORWARD
				FOR
				---------------

				SELECT DCStoreId,StockQty,DCS.BatchNo,DCS.MfgDate, DCS.ExpDate, ISNULL(CRS.Avg_Rate,CS.UnitPrice) CostPerUnit, PD.ProductCode FROM dbo.tblDCStore AS DCS
				LEFT JOIN tblCompanyUnit AS UNT ON DCS.ComUnitId = UNT.ComUnitId
				LEFT JOIN tblCentralStore AS CS ON DCS.TempReceiveId = CS.ReceiveId
				LEFT JOIN tblProduct AS PD ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(PD.ProductCode)))
				LEFT JOIN (SELECT WG.Weighted_Avg_Id,WG.Company_Id,ISNULL(WG.Warehouse_Id, 0) AS Warehouse_Id,WG.Item_Id,WG.Close_Qty AS CurrentStock2,WG.Avg_Rate,WG.Curr_Rate,WG.Trans_Date
				FROM dbo.tbl_INV_WeightedAvg WG
				JOIN (SELECT Item_Id,Company_Id,MAX(Weighted_Avg_Id) AS MaxWeighted_Avg_Id FROM dbo.tbl_INV_WeightedAvg GROUP BY Item_Id, Company_Id
				) Latest ON WG.Item_Id = Latest.Item_Id AND WG.Company_Id = Latest.Company_Id AND WG.Weighted_Avg_Id = Latest.MaxWeighted_Avg_Id
				) AS CRS ON UNT.CompanyId = CS.Company_Id AND PD.ProductId = CRS.Item_Id
				WHERE StockQty > 0 AND DCS.ComUnitId = @ComUnitId 
				AND PD.ProductId = @ProductId

				----------
				OPEN @MyCursor2
				FETCH NEXT FROM @MyCursor2
				INTO @DcStoreId,@StockQuantity,@BatchNo,@MMFGDate,@EXPDate,@CostPerUnit,@ProductCode
				
				WHILE @@FETCH_STATUS = 0
				BEGIN
				
						DECLARE @AdjustedQty DECIMAL(18,3) = 0

						-------------------------------------------------------------
						----------------------- Temp Quantity Check Stsrt ----------------------
						-------------------------------------------------------------

						IF (@TempQty > 0 ) 
						BEGIN

							IF(@StockQuantity >= @TempQty) -- If @StockQuantity = 50 & @TempQty = 40
							BEGIN

								-- Minus Stock 

								UPDATE dbo.tblDCStore SET StockQty = StockQty - @TempQty
								WHERE DCStoreId = @DcStoreId

								-- Insert into tblStockTrannsctionForSample (-)

								--SELECT * FROM tblSampleIssueTranscation

								INSERT INTO [dbo].[tblSampleIssueTranscation] ([ComUnitId],[CustomerMasterId],[IssueId],[IssueDetailId],[DCStoreId],[Quantity]) 
								VALUES(@ComUnitId,@CustomerMasterId,@MasterId,@OrderDetailsId,@DcStoreId,@TempQty*-1)

								SET @AdjustedQty = @TempQty
								SET @TempQty = 0
								

							END

							ELSE -- Else block 

							BEGIN

								-- Minus Stock 

								UPDATE tblDCStore SET StockQty = 0
								WHERE DCStoreId = @DcStoreId

								-- Insert into tblStockTrannsctionForSample (-)

								INSERT INTO [dbo].[tblSampleIssueTranscation] ([ComUnitId],[CustomerMasterId],[IssueId],[IssueDetailId],[DCStoreId],[Quantity]) 
								VALUES(@ComUnitId,@CustomerMasterId,@MasterId,@OrderDetailsId,@DcStoreId,@StockQuantity*-1)

								SET @AdjustedQty = @StockQuantity
								SET @TempQty = @TempQty - @StockQuantity
								

							END -- End of Adjustable Stock Avilable


							print '@AdjustedQty: ' +  CONVERT(NVARCHAR, @AdjustedQty)

									 --Set Initial Values to 0 (Weighted Average Check)

									DECLARE @Opening_Qty DECIMAL(18, 3) = 0
									DECLARE @Opening_Rate DECIMAL(18, 4) = 0
									DECLARE @Opening_Amount DECIMAL(18, 4) = 0
									DECLARE @Closing_Qty DECIMAL(18, 3) = 0
									DECLARE @Average_Rate DECIMAL(18, 4) = 0
									DECLARE @Closing_Amount DECIMAL(18, 4) = 0
					
									-- Read Values from Weighted Average Page

									SET @Opening_Qty = ISNULL((SELECT TOP 1 Close_Qty 
									                           FROM tbl_INV_WeightedAvg
									                           WHERE Company_Id = @CompanyId AND Item_Id = @ProductId
									                           ORDER BY Trans_Date DESC), 0)
					
									SET @Opening_Amount = ISNULL((SELECT TOP 1 Close_Amt 
									                              FROM tbl_INV_WeightedAvg
									                              WHERE Company_Id = @CompanyId AND Item_Id = @ProductId
									                              ORDER BY Trans_Date DESC), 0)
					
									SET @Opening_Rate = CASE WHEN @Opening_Qty > 0 THEN @Opening_Amount / @Opening_Qty ELSE 0 END
					
									SET @Closing_Qty = @Opening_Qty + (@AdjustedQty*-1)
									SET @Closing_Amount = @Opening_Amount + ((@AdjustedQty*@CostPerUnit)*-1)
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

									--SELECT @DuplicateCount = COUNT(Weighted_Avg_Id) FROM tbl_INV_WeightedAvg AS WG 
									--WHERE Trans_Type_Id = 4 -- Challan
									--AND Mst_Trans_Id = @ChalanId -- Invoice Master ID
									--AND Trans_Id = @CursorChalanDetailsId -- Invoice Details ID

									-- Commented because of Same OrderDetailsid can insert multiple time
									

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
										    @CompanyId,
										    @ComUnitId, -- Warehouse_Id
										    @MasterId, -- Mst_Trans_Id
										    @OrderDetailsId, --@PurchaseOrderLocalReceiveDetailId1, -- Trans_Id
										    @ProductId,
										    @ProductCode,
										    @OrderCode, -- Ref_No
										    GETDATE(), -- Trans_Date
										    @SubmissionDate, -- Actual_Date
										    @ApprovedDate, -- Entry_Date
										    @TranscationTypeId, -- Trans_Type_Id
										    @InventoryTypeId, -- Inv_Type_Id
										    @InventoryType, -- Inv_Type
										    @Opening_Qty, -- OP_Qty
										    @Opening_Rate, -- OP_Rate
										    @Opening_Amount, -- OP_Amt
										    @AdjustedQty*-1, -- Curr_Qty
										    @CostPerUnit, -- Curr_Rate
										    (@AdjustedQty*@CostPerUnit)*-1, -- Curr_Amt
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

											SELECT @STCount = COUNT(C.Batch_Stock_Id) FROM tbl_INV_BatchWiseStock AS C WHERE Trans_Qty < 0 AND Weighted_Avg_Id = @Weighted_Avg_Id_New AND Batch_No = @BatchNo 
											AND Trans_Qty = @AdjustedQty*-1 AND SampleOrderDetailId = @OrderDetailsId 
											AND ISNULL(SampleOrderDetailId,0) > 0 

											--PRINT '@STCount: ' + CONVERT(NVARCHAR(200), @STCount) 

											IF(@STCount = 0) -- If not Exist
											BEGIN

												--PRINT 'Insert Process: tbl_INV_BatchWiseStock'

												DECLARE @Ref_Weighted_Avg_Id INT = 0
												SELECT @Ref_Weighted_Avg_Id = Weighted_Avg_Id FROM tbl_INV_BatchWiseStock 
												WHERE ISNULL(DCStoreId,0) != 0 AND DCStoreId = @DCStoreId

												INSERT INTO tbl_INV_BatchWiseStock (Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,Trans_Qty, SampleOrderDetailId, Remarks,Issue_Weighted_Avg_Id )
												VALUES (@Ref_Weighted_Avg_Id,@BatchNo,@MMFGDate,@EXPDate,@AdjustedQty*-1, @OrderDetailsId, 'Sample Issue', @Weighted_Avg_Id_New)

												-- Insert 


												--SELECT Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date,SUM(ISNULL(Trans_Qty,0)) Trans_Qty FROM tbl_INV_BatchWiseStock 
												--WHERE  Weighted_Avg_Id = 10187   --@Weighted_Avg_Id
												--GROUP BY Weighted_Avg_Id,Batch_No,Mfg_Date,Exp_Date


											END

										END

									END -- End of Duplicate Check in Weighted Avg

					END  --Temp Quantity Check End

					-------------------------------------------------------------
				    ----------------------- Temp Quantity Check Stsrt ----------------------
					-------------------------------------------------------------
				
				
				FETCH NEXT FROM @MyCursor2
				INTO @DcStoreId,@StockQuantity,@BatchNo,@MMFGDate,@EXPDate,@CostPerUnit,@ProductCode
				
				END
				CLOSE @MyCursor2
				DEALLOCATE @MyCursor2 -- Second Courser End




			FETCH NEXT FROM @MyCursor 
			INTO @CompanyId, @FinancialYearId, @CustomerName,@ComUnitId,@ProductId,@Quantity,@OrderDetailsId,@TranscationTypeId,
			@InventoryTypeId,@InventoryType, @OrderCode, @CustomerMasterId, @SubmissionDate
			
			END
			CLOSE @MyCursor
			DEALLOCATE @MyCursor -- First Courser End


			

		END -- Accepted Ststus Check End


	END -- End of Is Posted or Not

END


--SELECT * FROM tbl_INV_BatchWiseStock
```

---

## `sp_UD_SampleStockIssue`

**Parameters:**
- `@MasterId` — int, input
- `@OrderDetailsId` — int, input
- `@DcId` — int, input
- `@Quantity` — DECIMAL(18,2), input
- `@ProductCode` — NVARCHAR(MAX), input

**Tables read (FROM/JOIN):** tblDCStore, tblSampleIssue
**Tables written:** INSERT → tblSampleIssueTranscation, tblStockTrannsctionForSample; UPDATE → tblDCStore

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from tblDCStore, tblSampleIssue, and per row inserts into tblSampleIssueTranscation, tblStockTrannsctionForSample; updates tblDCStore.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs:364` — `return aInternalDal.UpdateAction("sp_UD_SampleStockIssue", aSqlParameters);`
- `Library.DAL/SInventory_DAL/SampleIssueDal.cs:363` — `return aInternalDal.UpdateAction("sp_UD_SampleStockIssue", aSqlParameters);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UD_SampleStockIssue] 
	(
		 @MasterId int,
		 @OrderDetailsId int,
		 @DcId int,
		 @Quantity DECIMAL(18,2),
		 @ProductCode NVARCHAR(MAX)
	)
AS
BEGIN
	  
	DECLARE @ComUnitId INT=@DcId
	DECLARE @CustomerMasterId INT
	DECLARE @TempQty DECIMAL(18,2)

	--FOR Stock
	DECLARE @DcStoreId INT
	DECLARE @StockQuantity DECIMAL(18,2)

	SET @TempQty = @Quantity
	        
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------
	
	SELECT DCStoreId,StockQty FROM dbo.tblDCStore WHERE StockQty > 0 AND ComUnitId = @ComUnitId AND ProductCode = @ProductCode

	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @DcStoreId,@StockQuantity
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	SET @CustomerMasterId = NULL
	SET @ComUnitId = NULL
	
	SELECT @CustomerMasterId = CustomerMasterId FROM tblSampleIssue WHERE OrderId = @MasterId
	SELECT @ComUnitId = ComUnitId FROM tblSampleIssue WHERE OrderId = @MasterId


	IF (@TempQty > 0 )
	BEGIN

		IF(@StockQuantity >= @TempQty)
		BEGIN

			-- Minus Stock 
			UPDATE dbo.tblDCStore SET StockQty = (@StockQuantity - @TempQty)
			WHERE DCStoreId = @DcStoreId

			-- Insert into tblStockTrannsctionForSample (-)
			INSERT INTO [dbo].[tblSampleIssueTranscation] ([ComUnitId],[CustomerMasterId],[IssueId],[IssueDetailId],[DCStoreId],[Quantity]) 
			VALUES(@ComUnitId,@CustomerMasterId,@MasterId,@OrderDetailsId,@DcStoreId,@TempQty*-1)

			SET @TempQty = 0

		END

		ELSE -- Else block 

		BEGIN

			-- Minus Stock 
			UPDATE tblDCStore SET StockQty = 0
			WHERE DCStoreId = @DcStoreId

			-- Insert into tblStockTrannsctionForSample (-)
			INSERT INTO [dbo].[tblSampleIssueTranscation] ([ComUnitId],[CustomerMasterId],[IssueId],[IssueDetailId],[DCStoreId],[Quantity]) 
			VALUES(@ComUnitId,@CustomerMasterId,@MasterId,@OrderDetailsId,@DcStoreId,@StockQuantity*-1)

			SET @TempQty = @TempQty - @StockQuantity

		END

	END

	
	FETCH NEXT FROM @MyCursor
	INTO @DcStoreId,@StockQuantity
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor    

END
```

---

## `sp_UD_UserRoleInfo`

**Parameters:**
- `@UserRoleID` — INT, input
- `@RoleName` — NVARCHAR(500), input
- `@entryBy` — INT, input
- `@RoleTypeId` — INT, input
- `@isActive` — BIT, input
- `@acInAcDate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tbl_UserRoleInfo

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tbl_UserRoleInfo` (sets: RoleName, IsActive, ActiveInActiveDate, UpdateBy, UpdateDate, RoleTypeId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_UD_UserRoleInfo]
	-- Add the parameters for the stored procedure here
@UserRoleID INT,
    @RoleName NVARCHAR(500),
    @entryBy INT,
    @RoleTypeId INT,
	@isActive BIT,
	@acInAcDate DATETIME

AS
    BEGIN

	UPDATE tbl_UserRoleInfo
	SET RoleName=@RoleName
           ,IsActive=@IsActive
           ,ActiveInActiveDate=@acInAcDate
           ,UpdateBy=@entryBy
           ,UpdateDate=GETDATE(),RoleTypeId=@RoleTypeId
     
	WHERE UserRoleID = @UserRoleID

 END
```

---

## `sp_Update_DoctorCategory`

**Parameters:**
- `@CategoryId` — INT, input = `0`
- `@CategoryName` — NVARCHAR(MAX), input
- `@UpdateBy` — NVARCHAR(50), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblDoctorCategory

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblDoctorCategory` (sets: CategoryName, UpdateBy, UpdateDate, IsActive, Activedate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Update_DoctorCategory]
	-- Add the parameters for the stored procedure here
    @CategoryId INT = 0 ,
    @CategoryName NVARCHAR(MAX) ,
    @UpdateBy NVARCHAR(50) ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN

        UPDATE  dbo.tblDoctorCategory
        SET     CategoryName = @CategoryName ,
                UpdateBy = @UpdateBy ,
                UpdateDate = GETDATE() ,
                IsActive = @isActive ,      
                Activedate = @Activedate
        WHERE   CategoryId = @CategoryId

    END
```

---

## `sp_Update_DoctorChamber`

**Parameters:**
- `@ChamberId` — INT, input = `0`
- `@ChamberName` — NVARCHAR(MAX), input
- `@UpdateBy` — NVARCHAR(50), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblDoctorChamber

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblDoctorChamber` (sets: ChamberName, UpdateBy, UpdateDate, IsActive, Activedate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Update_DoctorChamber]
	-- Add the parameters for the stored procedure here
    @ChamberId INT = 0 ,
    @ChamberName NVARCHAR(MAX) ,
    @UpdateBy NVARCHAR(50) ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN

        UPDATE  dbo.tblDoctorChamber
        SET     ChamberName = @ChamberName,
                UpdateBy = @UpdateBy ,
                UpdateDate = GETDATE() ,
                IsActive = @isActive ,      
                Activedate = @Activedate
        WHERE   ChamberId = @ChamberId

    END
```

---

## `sp_Update_DoctorDegree`

**Parameters:**
- `@DegreeId` — INT, input = `0`
- `@DegreeName` — NVARCHAR(MAX), input
- `@DoctorTypeId` — int, input = `null`
- `@UpdateBy` — NVARCHAR(50), input
- `@remarks` — NVARCHAR(MAX), input = `NULL`
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblDoctorDegree

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblDoctorDegree` (sets: DegreeName, UpdateBy, UpdateDate, IsActive, Activedate, DoctorTypeId).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE
 PROCEDURE [dbo].[sp_Update_DoctorDegree]
	-- Add the parameters for the stored procedure here
    @DegreeId INT = 0 ,
    @DegreeName NVARCHAR(MAX) ,
	@DoctorTypeId int=null,

    @UpdateBy NVARCHAR(50) ,
    @remarks NVARCHAR(MAX) = NULL ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN

        UPDATE  dbo.tblDoctorDegree
        SET     DegreeName = @DegreeName ,
                UpdateBy = @UpdateBy ,
                UpdateDate = GETDATE() ,
                IsActive = @isActive ,      
                Activedate = @Activedate, DoctorTypeId=0
        WHERE   DegreeId = @DegreeId


    END
```

---

## `sp_Update_DoctorDesignation`

**Parameters:**
- `@DesignationId` — INT, input = `0`
- `@DesignationName` — NVARCHAR(MAX), input
- `@UpdateBy` — NVARCHAR(50), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblDoctorDesignation

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblDoctorDesignation` (sets: DesignationName, UpdateBy, UpdateDate, IsActive, Activedate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Update_DoctorDesignation]
	-- Add the parameters for the stored procedure here
    @DesignationId INT = 0 ,
    @DesignationName  NVARCHAR(MAX) ,
    @UpdateBy NVARCHAR(50) ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN

        UPDATE  dbo.tblDoctorDesignation
        SET     DesignationName = @DesignationName ,
                UpdateBy = @UpdateBy ,
                UpdateDate = GETDATE() ,
                IsActive = @isActive ,      
                Activedate = @Activedate
        WHERE   DesignationId = @DesignationId

    END
```

---

## `sp_Update_DoctorMaster`

**Parameters:**
- `@DoctorId` — int, input
- `@SecondaryCode` — nvarchar(max), input = `null`
- `@DesignationId` — int, input = `null`
- `@Gender` — nvarchar(max), input = `null`
- `@DoctorName` — nvarchar(max), input = `null`
- `@IsActive` — bit, input = `null`
- `@Activedate` — datetime, input = `null`
- `@InactiveDate` — datetime, input = `null`
- `@DivisionId` — int, input = `null`
- `@DistrictId` — int, input = `null`
- `@ThanaId` — int, input = `null`
- `@UPCode` — nvarchar(max), input = `null`
- `@DoctorTypeId` — int, input = `null`
- `@MarketId` — int, input = `null`
- `@UnionName` — nvarchar(max), input = `null`
- `@Reamrks` — nvarchar(max), input = `null`
- `@StationTypeId` — int, input = `null`
- `@ProgramTypeId` — int, input = `null`
- `@DoctorCategoryId` — int, input = `null`
- `@UpdateBy` — int, input
- `@SMCTypeId` — int, input = `null`

**Tables read (FROM/JOIN):** tblDoctorBrandDetail, tblDoctorChemberDetail, tblDoctorContactDetail, tblDoctorDegreeDetail, tblDoctorSpecialDayDetail, tblDoctorSpecialityDetail, tbl_DCRInfo, tbl_PrescriptionMaster
**Tables written:** UPDATE → tblDoctorMaster; DELETE → tblDoctorBrandDetail, tblDoctorChemberDetail, tblDoctorContactDetail, tblDoctorDegreeDetail, tblDoctorSpecialDayDetail, tblDoctorSpecialityDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblDoctorBrandDetail, tblDoctorChemberDetail, tblDoctorContactDetail, tblDoctorDegreeDetail, tblDoctorSpecialDayDetail, tblDoctorSpecialityDetail, tbl_DCRInfo, tbl_PrescriptionMaster, then updates `tblDoctorMaster` (sets: DoctorName, DesignationId, Gender, IsActive, Activedate, UpdateBy, UpdateDate, DoctorTypeId); deletes from `tblDoctorBrandDetail`; deletes from `tblDoctorChemberDetail`; deletes from `tblDoctorContactDetail`; deletes from `tblDoctorDegreeDetail`; deletes from `tblDoctorSpecialDayDetail`; deletes from `tblDoctorSpecialityDetail`.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select ChemberId from tbl_DCRInfo where DoctorId=@DoctorId) tbl) and DoctorId=@DoctorId END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 CREATE PROCEDURE [dbo].[sp_Update_DoctorMaster]
	-- Add the parameters for the stored procedure here
   
		   @DoctorId int 
      ,@SecondaryCode  nvarchar(max)=null
      ,@DesignationId int=null
	  ,@Gender  nvarchar(max)=null
      ,@DoctorName nvarchar(max)=null
	  ,@IsActive  bit=null
      ,@Activedate   datetime=null
      ,@InactiveDate   datetime=null
      
	  ,@DivisionId  int=null
      ,@DistrictId  int=null
      ,@ThanaId  int=null
	  ,@UPCode nvarchar(max)=null
      ,@DoctorTypeId  int=null
      
      ,@MarketId  int=null
      ,@UnionName  nvarchar(max)=null
      ,@Reamrks  nvarchar(max)=null
      ,@StationTypeId   int=null,
	  @ProgramTypeId   int=null,
	  @DoctorCategoryId   int=null,

	@UpdateBy int,
	  @SMCTypeId   int=null

   
AS
    BEGIN
 

        UPDATE  dbo.tblDoctorMaster
        SET     DoctorName=@DoctorName
       
       
      ,[DesignationId]=@DesignationId
    
      ,[Gender]=@Gender
   
   
       
      ,[IsActive]=@IsActive
      ,[Activedate]=@Activedate
      ,UpdateBy=@UpdateBy
      ,UpdateDate=GETDATE()
      
     
      ,[DoctorTypeId]=@DoctorTypeId
      
      ,[MarketId]=@MarketId
      ,[UnionName]=@UnionName 
      ,[StationTypeId]=@StationTypeId,ProgramTypeId=@ProgramTypeId,DoctorCategoryId=@DoctorCategoryId,InactiveDate=@InactiveDate , SMCTypeId  =@SMCTypeId                    
        WHERE   DoctorId = @DoctorId


	 
		 Delete From dbo.tblDoctorBrandDetail where DoctorId = @DoctorId

		 Delete From dbo.tblDoctorDegreeDetail where DoctorId = @DoctorId
		 Delete From dbo.tblDoctorSpecialityDetail where DoctorId = @DoctorId

		  Delete From dbo.tblDoctorSpecialDayDetail where DoctorId = @DoctorId
		
		 Delete From dbo.tblDoctorContactDetail where DoctorId = @DoctorId


		 declare @CountPres int =0
		 declare @CountDcr int =0
		 select @CountPres=ISNULL(COUNT(*),0) from tbl_PrescriptionMaster where DoctorId=@DoctorId
		 select @CountDcr=ISNULL(COUNT(*),0)  from tbl_DCRInfo where DoctorId=@DoctorId


		 print @CountPres
		 print @CountDcr
 

		 --if(@CountPres=0 and @CountDcr=0)
		 --begin
		 delete From dbo.tblDoctorChemberDetail where 	ChemberId not in( select ChemberId from (
		  select   ChemberId from tbl_PrescriptionMaster where DoctorId=@DoctorId
		  union all
		 select ChemberId   from tbl_DCRInfo where DoctorId=@DoctorId) tbl) and  DoctorId=@DoctorId
		  --end

    END
```

---

## `sp_Update_DoctorPatientType`

**Parameters:**
- `@PatientTypeId` — INT, input = `0`
- `@PatientType` — NVARCHAR(MAX), input
- `@UpdateBy` — NVARCHAR(50), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblDoctorPatientType

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblDoctorPatientType` (sets: PatientType, UpdateBy, UpdateDate, IsActive, Activedate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Update_DoctorPatientType]
	-- Add the parameters for the stored procedure here
    @PatientTypeId  INT = 0 ,
    @PatientType   NVARCHAR(MAX) ,
    @UpdateBy NVARCHAR(50) ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN

        UPDATE  [dbo].[tblDoctorPatientType]
        SET     PatientType = @PatientType ,
                UpdateBy = @UpdateBy ,
                UpdateDate = GETDATE(),
                IsActive = @isActive ,      
                Activedate = @Activedate
        WHERE   PatientTypeId = @PatientTypeId 

    END
```

---

## `sp_Update_DoctorSpecialDay`

**Parameters:**
- `@SpecialDayId` — INT, input = `0`
- `@SpecialDay` — NVARCHAR(MAX), input
- `@UpdateBy` — NVARCHAR(50), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblDoctorSpecialDay

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblDoctorSpecialDay` (sets: SpecialDay, UpdateBy, UpdateDate, IsActive, Activedate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Update_DoctorSpecialDay]
	-- Add the parameters for the stored procedure here
    @SpecialDayId INT = 0 ,
    @SpecialDay  NVARCHAR(MAX) ,
    @UpdateBy NVARCHAR(50) ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN

        UPDATE  [dbo].[tblDoctorSpecialDay] 
        SET     SpecialDay = @SpecialDay ,
                UpdateBy = @UpdateBy ,
                UpdateDate = GETDATE() ,
                IsActive = @isActive ,      
                Activedate = @Activedate
        WHERE   SpecialDayId = @SpecialDayId

    END
```

---

## `sp_Update_DoctorSpeciality`

**Parameters:**
- `@SpecialityId` — INT, input = `0`
- `@SpecialityName` — NVARCHAR(MAX), input
- `@UpdateBy` — NVARCHAR(50), input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblDoctorSpeciality

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblDoctorSpeciality` (sets: SpecialityName, UpdateBy, UpdateDate, IsActive, Activedate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Update_DoctorSpeciality]
	-- Add the parameters for the stored procedure here
    @SpecialityId INT = 0 ,
    @SpecialityName  NVARCHAR(MAX) ,
    @UpdateBy NVARCHAR(50) ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN

        UPDATE [dbo].[tblDoctorSpeciality] 
        SET     SpecialityName = @SpecialityName,
                UpdateBy = @UpdateBy,
                UpdateDate = GETDATE(),
                IsActive = @isActive,      
                Activedate = @Activedate
        WHERE   SpecialityId = @SpecialityId 

    END
```

---

## `sp_Update_Employee_ShiftInfos`

**Parameters:**
- `@ShiftId` — INT, input = `0`
- `@ShiftText` — NVARCHAR(MAX), input
- `@ShiftInTime` — TIME, input
- `@ShiftOutTime` — TIME, input
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input
- `@UpdateBy` — NVARCHAR(50), input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tbl_Shift

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tbl_Shift` (sets: ShiftText, ShiftInTime, ShiftOutTime, IsActive, Activedate, UpdateBy, UpdateDate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Update_Employee_ShiftInfos]
	-- Add the parameters for the stored procedure here
 

	@ShiftId INT = 0 ,
    @ShiftText NVARCHAR(MAX) ,
    @ShiftInTime TIME ,
    @ShiftOutTime TIME ,
   
	@IsActive BIT,
	@Activedate DATETIME,
	@UpdateBy NVARCHAR(50) 
AS
    BEGIN

      UPDATE [dbo].[tbl_Shift]
   SET [ShiftText] = @ShiftText 
      ,[ShiftInTime] = @ShiftInTime 
      ,[ShiftOutTime] = @ShiftOutTime 
      ,[IsActive] = @IsActive 
      ,[Activedate] = @Activedate 
     
      ,[UpdateBy] = @UpdateBy 
      ,[UpdateDate] = GETDATE()
        WHERE    ShiftId = @ShiftId


    END
```

---

## `sp_Update_MIO_VacantUpdateProcess`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tblOrder

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tblOrder` (sets: MIOCode, MIOName).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CReate PROCEDURE [dbo].[sp_Update_MIO_VacantUpdateProcess]
	

AS
    BEGIN

      Update tblOrder SEt MIOCode='ZAS-1001' , MIOName='Vacant' Where MIOCode IS NULL AND MIOName IS NULL

    END
```

---

## `sp_Update_PrescriptionType`

**Parameters:**
- `@PrescriptionTypeId` — INT, input = `0`
- `@PrescriptionType` — NVARCHAR(MAX), input
- `@UpdateBy` — NVARCHAR(50), input
- `@remarks` — NVARCHAR(MAX), input = `NULL`
- `@IsActive` — BIT, input
- `@Activedate` — DATETIME, input

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tbl_PrescriptionType

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tbl_PrescriptionType` (sets: PrescriptionType, UpdateBy, UpdateDate, IsActive, ActiveInactiveDate).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Update_PrescriptionType]
	-- Add the parameters for the stored procedure here
    @PrescriptionTypeId  INT = 0 ,
    @PrescriptionType NVARCHAR(MAX) ,
    @UpdateBy NVARCHAR(50) ,
    @remarks NVARCHAR(MAX) = NULL ,
    @IsActive BIT ,
    @Activedate DATETIME
AS
    BEGIN
        UPDATE  dbo.tbl_PrescriptionType
        SET     PrescriptionType = @PrescriptionType,
                UpdateBy = @UpdateBy,
                UpdateDate = GETDATE(),
                IsActive = @isActive,      
                ActiveInactiveDate = @Activedate
        WHERE   PrescriptionTypeId = @PrescriptionTypeId
    END
```

---

## `sp_Update_UserMaster_new`

**Parameters:**
- `@UserId` — INT, input
- `@UserName` — nvarchar (max), input = `NULL`
- `@UserType` — nvarchar (max), input = `NULL`
- `@LoginName` — nvarchar (max), input = `NULL`
- `@Password` — nvarchar (max), input = `NULL`
- `@IMEI_One` — nvarchar (max), input = `NULL`
- `@IMEI_Two` — nvarchar (max), input = `NULL`
- `@EmpInfoId` — int, input = `NULL`
- `@UserRoleID` — int, input = `NULL`
- `@UserTypeId` — int, input = `NULL`
- `@IsAppsUser` — bit, input = `NULL`
- `@ActiveInActiveDate` — datetime, input = `NULL`
- `@EntryBy` — nvarchar(50), input = `NULL`
- `@UserStatus` — nvarchar (max), input = `NULL`
- `@IsMainDashboard` — bit, input = `NULL`
- `@IsDepotDashboard` — bit, input = `NULL`
- `@IsHead` — bit, input = `NULL`
- `@IsCreditApproval` — bit, input = `NULL`

**Tables read (FROM/JOIN):** tblTagASMRSMInfo, tblUserCompanyUnit, tbl_UserMarketDetail
**Tables written:** UPDATE → tblUser; DELETE → tblTagASMRSMInfo, tblUserCompanyUnit, tbl_UserMarketDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tblTagASMRSMInfo, tblUserCompanyUnit, tbl_UserMarketDetail, then updates `tblUser` (sets: IsMainDashboard, IsDepotDashboard, UserName, UserType, LoginName, Password, UserStatus, EmpInfoId); deletes from `tblTagASMRSMInfo`; deletes from `tblUserCompanyUnit`; deletes from `tbl_UserMarketDetail`.

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 CREATE PROCEDURE [dbo].[sp_Update_UserMaster_new]
	-- Add the parameters for the stored procedure here
@UserId INT,
    @UserName nvarchar (max)=NULL,
    @UserType nvarchar (max)=NULL,
    @LoginName nvarchar (max)=NULL,
    @Password nvarchar (max)=NULL,
    @IMEI_One nvarchar (max)=NULL,
    @IMEI_Two nvarchar (max)=NULL,
    @EmpInfoId int=NULL,
    @UserRoleID int=NULL,
    @UserTypeId int=NULL,
    
    
	@IsAppsUser bit=NULL,
    @ActiveInActiveDate datetime=NULL,
	@EntryBy nvarchar(50)=NULL,


	@UserStatus nvarchar (max)=NULL,
	@IsMainDashboard bit=NULL,
	@IsDepotDashboard bit=NULL,
	@IsHead bit=NULL,
	@IsCreditApproval bit=NULL
   
AS
    BEGIN

       UPDATE [dbo].[tblUser] set  IsMainDashboard=@IsMainDashboard,IsDepotDashboard=@IsDepotDashboard,
     [UserName]=@UserName
           ,[UserType]=@UserType
           
           ,[LoginName]=@LoginName
           ,[Password]=@Password,
         
      UserStatus=@UserStatus
           ,[EmpInfoId]=@EmpInfoId
           ,[IsAppsUser]=@IsAppsUser
           ,[IMEI_One]=@IMEI_One
           ,[IMEI_Two]=@IMEI_Two
           ,[UserRoleID]=@UserRoleID
           ,[ActiveInActiveDate]=@ActiveInActiveDate
           ,[UserTypeId]=@UserTypeId,IsCreditApproval=@IsCreditApproval, IsHead=@IsHead,  UpdateBy=@EntryBy,UpdateDate =GETDATE()               
        WHERE    UserId = @UserId
-- delete from [tbl_UserMarketDetail]         WHERE    UserId = @UserId
 delete from tblUserCompanyUnit         WHERE    UserId = @UserId

 delete from tblTagASMRSMInfo         WHERE    UserId = @UserId

 
    END
```

---

## `sp_Upload_Customer`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** Table_CustomerUpload, tblCustMaster
**Tables written:** INSERT → tblCustMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from Table_CustomerUpload, tblCustMaster, and per row inserts into tblCustMaster.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Upload_Customer] 


AS
BEGIN
	
	DECLARE @CustomerCode NVARCHAR(500)
	DECLARE @CustomerName NVARCHAR(MAX)
	DECLARE @Address NVARCHAR(MAX)
	DECLARE @MarketId INT
	DECLARE @TermsOfPayment NVARCHAR(200)
	DECLARE @CellNo NVARCHAR(200)
	
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT CUD.CustomerCode,CUD.CustomerName,CUD.Address,CUD.MarketId,CUD.TermsOfPayment,CUD.CellNo FROM Table_CustomerUpload AS CUD
	LEFT JOIN tblCustMaster AS CS ON CUD.CustomerCode = CS.CustomerCode
	WHERE CS.CustomerCode IS NULL
	
    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @CustomerCode,@CustomerName,@Address,@MarketId,@TermsOfPayment,@CellNo
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		INSERT INTO [dbo].[tblCustMaster]
           (CustomerCode
           ,CategoryId
           ,CustomerName
           ,Address
           ,CellNo
           ,MarketId
           ,Addrees2
           ,City
           ,ConPerson
           ,TermOfPayment
           ,FixedCustomer
           ,CreateBy
           ,CreateDate
           ,IsActive
           ,IsVatApplicable
           ,AITGLId)
     VALUES
           (@CustomerCode
           ,1
           ,@CustomerName
           ,@Address
           ,@CellNo
           ,@MarketId
           ,@Address
           ,'Dhaka'
           ,'Mr.'
           ,@TermsOfPayment
           ,0
           ,'rezaul'
           ,GETDATE()
           ,1
           ,0
           ,0)
	
    FETCH NEXT FROM @MyCursor
    INTO @CustomerCode,@CustomerName,@Address,@MarketId,@TermsOfPayment,@CellNo
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor
	
END
```

---

## `sp_Upload_CustomerWiseDue`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** Table_DueInvoice, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo
**Tables written:** INSERT → tblInvoice

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Iterates row-by-row via a cursor over data from Table_DueInvoice, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, and per row inserts into tblInvoice.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Upload_CustomerWiseDue] 


AS
BEGIN
	
	DECLARE @InvoiceNo NVARCHAR(500)
	DECLARE @DeliveryInvoNo NVARCHAR(500)
	DECLARE @InvoiceDate DATETIME
	DECLARE @CustomerMasterId INT
	DECLARE @ComUnitId INT
	DECLARE @MiaId int
	declare @Due decimal(18,2)
	
    --------------------------------------------------------
    DECLARE @MyCursor CURSOR
    SET @MyCursor = CURSOR FAST_FORWARD
    FOR
    ---------------

	SELECT DI.InvoiceNo,DeliveryInvoNo,DI.InvoiceDate,CM.CustomerMasterId,UNT.ComUnitId,MIO.MIOId MiaId,di.Due FROM Table_DueInvoice AS DI
	INNER JOIN tblCompanyUnit AS UNT ON LTRIM(RTRIM(UPPER(DI.UnitCode))) = LTRIM(RTRIM(UPPER(UNT.ComUnitCode)))
	INNER JOIN tblCustMaster AS CM ON LTRIM(RTRIM(UPPER(CM.CustomerCode))) = LTRIM(RTRIM(UPPER(DI.CustomerCode)))
	LEFT JOIN tblEmpGeneralInfo AS EGI ON DI.MiaCode = EGI.EmpMasterCode
	LEFT JOIN tblMIOInfo AS MIO ON EGI.EmpInfoId = MIO.EmployeeId
	LEFT JOIN tblInvoice AS INV ON DI.InvoiceNo = INV.InvoiceNo
	WHERE INV.InvoiceNo IS NULL
	
    ----------
    OPEN @MyCursor
    FETCH NEXT FROM @MyCursor
    INTO @InvoiceNo,@DeliveryInvoNo,@InvoiceDate,@CustomerMasterId,@ComUnitId,@MiaId,@Due
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
		INSERT INTO [dbo].[tblInvoice]
           (InvoiceNo
           ,InvoiceDate
           ,CustomerMasterId
           ,ComUnitId
           ,MiaId
           ,PaymentTypeId
           ,TpTotal
           ,TpDiscount
           ,TpVat
           ,TpGrandTotal
           ,UserId
           ,DeliveryTpTotal
           ,DeliveryTpDiscount
           ,DeliveryTpVat
           ,DeliveryTpGrandTotal
           ,DeliveryInvoiceStatus
           ,DelivaryInvoiceNo
           ,UpdateDate
           ,TotalSpecialAmount
           ,DelivarySpecialAmount
           ,ProductOffer
           ,OldTradePolicy
           ,Remarks
           ,DeliveryManId
           ,BatchId
           ,IsDirect
           ,ProcessTirm)
     VALUES
           (@InvoiceNo
           ,@InvoiceDate
           ,@CustomerMasterId
           ,@ComUnitId
           ,@MiaId
           ,2
           ,@Due
           ,0
           ,0
           ,@Due
           ,0
           ,@Due
           ,0
           ,0
           ,@Due
           ,'Full'
           ,@DeliveryInvoNo
           ,GETDATE()
           ,0
           ,0
           ,0
           ,0
           ,'Due'
           ,0
           ,0
           ,0
           ,GETDATE())
	
    FETCH NEXT FROM @MyCursor
    INTO @InvoiceNo,@DeliveryInvoNo,@InvoiceDate,@CustomerMasterId,@ComUnitId,@MiaId,@Due
    
    END
    CLOSE @MyCursor
    DEALLOCATE @MyCursor


END
```

---

## `sp_WearhouseBinCard`

**Parameters:**
- `@FromDate` — DATETIME, input
- `@ToDate` — DATETIME, input
- `@CompanyId` — INT, input

**Tables read (FROM/JOIN):** tblCentralStore, tblCentralStore_OpeninigBalance, tblCompanyInfo, tblProduct, tblRequisition, tblStockInTransfar, tblUnitPrice, tblWhStoreFreeze
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblCentralStore, tblCentralStore_OpeninigBalance, tblCompanyInfo, tblProduct, tblRequisition, tblStockInTransfar, tblUnitPrice, tblWhStoreFreeze, filtered by @CompanyId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT P.ProductCode,SUM(WF.StockQty) AS Freeze,SUM(WF.StockQty*CS.UnitPrice) AS WfStockValue FROM tblWhStoreFreeze AS WF LEFT JOIN tblProduct AS P ON P.ProductId = WF.ProductId LEFT JOIN tblCentralStore AS CS ON WF.ReceiveId = CS.ReceiveId WHERE WF.ReceiveDate BETWEEN @FromDate AND @ToDate GROUP BY P.ProductCode) AS WFZ ON PD.ProductCode = WFZ.Pr`

**Application caller(s):**
- `Library.DAL/SInventory_DAL/WHBinCardDal.cs:24` — `return aCommonInternalDal.GetDataTableAction("sp_WearhouseBinCard", aSqlParameterList, "SSIDB");`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,0@CompanyId/@CompanyId5/20@CompanyId6,>
-- Description:	<Description,,>
-- =============================================
CREATE
 PROCEDURE [dbo].[sp_WearhouseBinCard] 

	@FromDate DATETIME,
	@ToDate DATETIME,
	@CompanyId INT
	
AS
BEGIN
	
	SELECT CI.CompanyName,PD.ProductId,PD.ProductCode,PD.ProductName,PD.PackSize,ISNULL(CSOP.OpeningStock,0) AS OpeningStock,ISNULL(CSR.StockInQuantity,0) AS StockReceive,
(ISNULL(CSOP.OpeningStock,0) + ISNULL(CSR.StockInQuantity,0)) AS TotalReceive,ISNULL(DHK.Dhaka,0) AS Dhaka,ISNULL(BG.Bogra,0) AS Bogra,
ISNULL(CTG.Chittagong,0) AS Chittagong,ISNULL(NK.Noakhali,0) AS Noakhali,ISNULL(SYL.Sylhet,0) AS Sylhet,ISNULL(KLN.Khulna,0) AS Khulna,ISNULL(MYM.Mymensingh,0) AS Mymensingh,
ISNULL(RNG.Rangpur,0) AS Rangpur,ISNULL(RAJ.Rajshahi,0) AS Rajshahi,ISNULL(CML.Cumilla,0) AS Cumilla, 
ISNULL(TNS.Transit,0) AS Transit,ISNULL(WFZ.Freeze,0) AS Freeze, @FromDate AS FromDate,@ToDate AS ToDate,
(ISNULL(DHK.Dhaka,0)+ISNULL(BG.Bogra,0)+ISNULL(CTG.Chittagong,0)+ISNULL(NK.Noakhali,0)+ISNULL(SYL.Sylhet,0)+ISNULL(KLN.Khulna,0)++ISNULL(KLN.Khulna,0)+ISNULL(MYM.Mymensingh,0)+ISNULL(RNG.Rangpur,0)+ISNULL(RAJ.Rajshahi,0)+ISNULL(CML.Cumilla,0)+
ISNULL(TNS.Transit,0)+ISNULL(WFZ.Freeze,0)) AS TotalIssue,((ISNULL(CSOP.OpeningStock,0) + ISNULL(CSR.StockInQuantity,0)) - (ISNULL(DHK.Dhaka,0)+ISNULL(BG.Bogra,0)+ISNULL(CTG.Chittagong,0)+ISNULL(NK.Noakhali,0)+ISNULL(SYL.Sylhet,0)+ISNULL(KLN.Khulna,0)+ISNULL(MYM.Mymensingh,0)+ISNULL(RNG.Rangpur,0)+ISNULL(RAJ.Rajshahi,0)+ISNULL(CML.Cumilla,0)+
ISNULL(TNS.Transit,0)+ISNULL(WFZ.Freeze,0))) AS ClosingStock,((ISNULL(CSOP.OpeningStockValue,0)) + (ISNULL(CSR.ReceiveStockValue,0)) - (ISNULL(DHK.DhkStockValue,0)+ISNULL(BG.BgrStockValue,0)+ISNULL(CTG.CtgStockValue,0)+ISNULL(NK.NklStockValue,0)+ISNULL(SYL.Sylhet,0)+ISNULL(KLN.KlnStockValue,0)+ISNULL(CML.CumlStockValue,0)
+ISNULL(MYM.MymStockValue,0)+ISNULL(RNG.RngStockValue,0)+ISNULL(RAJ.RajStockValue,0) + ISNULL(TNS.TnsStockValue,0)+ISNULL(WFZ.WfStockValue,0))) StockValue  FROM tblProduct  AS PD 
LEFT JOIN tblCompanyInfo AS CI ON CI.CompanyId = PD.CompanyId
LEFT JOIN tblUnitPrice AS UP ON UP.ProductId = PD.ProductId 
LEFT JOIN (
SELECT OP.ProductCode,SUM(OP.Quantity) AS OpeningStock,SUM(OP.Quantity*OP.UnitPrice) AS OpeningStockValue FROM tblCentralStore_OpeninigBalance OP 
WHERE OP.CSOpeninigBalanceDate BETWEEN @FromDate AND @FromDate GROUP BY OP.ProductCode ) AS CSOP ON PD.ProductCode = CSOP.ProductCode 
LEFT JOIN (
SELECT ProductCode,SUM(StockInQty) AS StockInQuantity,SUM(StockInQty*UnitPrice) AS ReceiveStockValue FROM tblCentralStore WHERE ReceiveDate BETWEEN @FromDate AND @ToDate
GROUP BY ProductCode) AS CSR ON PD.ProductCode = CSR.ProductCode 
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Dhaka,SUM(ST.Quantity*CS.UnitPrice) AS DhkStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Dhaka%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS DHK ON PD.ProductCode = DHK.ProductCode
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Bogra,SUM(ST.Quantity*CS.UnitPrice) AS BgrStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Bogra%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS BG ON PD.ProductCode = BG.ProductCode
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Chittagong,SUM(ST.Quantity*CS.UnitPrice) AS CtgStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Chattogram%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS CTG ON PD.ProductCode = CTG.ProductCode
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Noakhali,SUM(ST.Quantity*CS.UnitPrice) AS NklStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Noakhali%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS NK ON PD.ProductCode = NK.ProductCode
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Sylhet,SUM(ST.Quantity*CS.UnitPrice) AS SylStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Sylhet%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS SYL ON PD.ProductCode = SYL.ProductCode
LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Khulna,SUM(ST.Quantity*CS.UnitPrice) AS KlnStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Khulna%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS KLN ON PD.ProductCode = KLN.ProductCode

LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Rajshahi,SUM(ST.Quantity*CS.UnitPrice) AS RajStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Rajshahi%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS RAJ ON PD.ProductCode = RAJ.ProductCode

LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Mymensingh,SUM(ST.Quantity*CS.UnitPrice) AS MymStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Mymensingh%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS MYM ON PD.ProductCode = MYM.ProductCode

LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Rangpur,SUM(ST.Quantity*CS.UnitPrice) AS RngStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Rangpur%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS RNG ON PD.ProductCode = RNG.ProductCode

LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Cumilla,SUM(ST.Quantity*CS.UnitPrice) AS CumlStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND ComUnitName LIKE '%Cumilla%' AND ReceiveIssue = 'OK' AND RQ.ReceiveIssueDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS CML ON PD.ProductCode = CML.ProductCode

LEFT JOIN (
SELECT ST.ProductCode,SUM(ST.Quantity) AS Transit,SUM(ST.Quantity*CS.UnitPrice) AS TnsStockValue FROM tblStockInTransfar AS ST
LEFT JOIN tblRequisition AS RQ ON ST.ReqId = RQ.ReqId
LEFT JOIN tblCentralStore AS CS ON ST.ReceiveId = CS.ReceiveId
WHERE CompanyId = @CompanyId AND (ReceiveIssue IS NULL OR ReceiveIssue ='') AND RQ.ReqDate BETWEEN @FromDate AND @ToDate
GROUP BY ST.ProductCode) AS TNS ON PD.ProductCode = TNS.ProductCode
LEFT JOIN (
SELECT P.ProductCode,SUM(WF.StockQty) AS Freeze,SUM(WF.StockQty*CS.UnitPrice) AS WfStockValue FROM tblWhStoreFreeze AS WF
LEFT JOIN tblProduct AS P ON P.ProductId = WF.ProductId
LEFT JOIN tblCentralStore AS CS ON WF.ReceiveId = CS.ReceiveId
 WHERE WF.ReceiveDate BETWEEN @FromDate AND @ToDate GROUP BY P.ProductCode) AS WFZ ON PD.ProductCode = WFZ.ProductCode
WHERE PD.CompanyId = @CompanyId  AND UP.IsActive = 1 ORDER BY PD.ProductCode

END
```

---

## `sp_Webapi_CHK_PROMOProductQty`

**Parameters:**
- `@EmpInfoId` — INT, input
- `@ProductId` — INT, input
- `@Qty` — int, input

**Tables read (FROM/JOIN):** tblGroupWisePromoQty
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblGroupWisePromoQty, filtered by @EmpInfoId, @ProductId, @Qty.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT ISNULL(SUM(TransactionQTY) ,0)TransactionQTY FROM dbo.tblGroupWisePromoQty WHERE EmpInfoId=@EmpInfoId AND ProductId=@ProductId HAVING ISNULL(SUM(TransactionQTY) ,0)>=@Qty END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
 
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_CHK_PROMOProductQty]
	-- Add the parameters for the stored procedure here
    @EmpInfoId INT,
    @ProductId INT,
    @Qty int


AS
    BEGIN
	
        SELECT  ISNULL(SUM(TransactionQTY) ,0)TransactionQTY FROM dbo.tblGroupWisePromoQty WHERE EmpInfoId=@EmpInfoId AND ProductId=@ProductId HAVING ISNULL(SUM(TransactionQTY) ,0)>=@Qty


    END
```

---

## `sp_Webapi_Delete_Dcr`

**Parameters:**
- `@id` — int, input

**Tables read (FROM/JOIN):** tbl_DCRInfo, tbl_DcrDetails
**Tables written:** DELETE → tbl_DCRInfo, tbl_DcrDetails

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tbl_DCRInfo, tbl_DcrDetails, then deletes from `tbl_DCRInfo`; deletes from `tbl_DcrDetails`.

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Delete_Dcr]
	-- Add the parameters for the stored procedure here
@id int
AS
BEGIN
	
	DELETE FROM dbo.tbl_DCRInfo WHERE DcrId = @id
	DELETE FROM dbo.tbl_DcrDetails WHERE DcrId = @id


END
```

---

## `sp_Webapi_Delete_DoctorTourPlan`

**Parameters:**
- `@id` — int, input

**Tables read (FROM/JOIN):** tbl_DoctorTourPlanDetail
**Tables written:** DELETE → tbl_DoctorTourPlanDetail

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Reads from tbl_DoctorTourPlanDetail, then deletes from `tbl_DoctorTourPlanDetail`.

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_Delete_DoctorTourPlan]
	-- Add the parameters for the stored procedure here
@id int
AS
BEGIN
		
		DELETE FROM dbo.tbl_DoctorTourPlanDetail WHERE DocTPDetailsId = @id


END
```

---

## `sp_Webapi_FinalSubmitSend_TourPlan`

**Parameters:**
- `@empId` — INT, input = `null`
- `@month` — INT, input = `NULL`
- `@year` — INT, input = `NULL`
- `@remarks` — NVARCHAR(max), input = `NULL`

**Tables read (FROM/JOIN):** (none — no FROM/JOIN detected)
**Tables written:** UPDATE → tbl_TourPlanMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Pure write operation: updates `tbl_TourPlanMaster` (sets: IsFinalSubmit, FinalSubmitRemarks, ApprovalStatus).

**Result set:** No SELECT anywhere in the body — pure write/void procedure, no result set.

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_FinalSubmitSend_TourPlan]
	-- Add the parameters for the stored procedure here
@empId INT=null,
@month INT = NULL,
@year INT = NULL,
@remarks NVARCHAR(max) = NULL
As
BEGIN
			

			UPDATE dbo.tbl_TourPlanMaster 
			SET IsFinalSubmit = 1 , FinalSubmitRemarks = @remarks,
			ApprovalStatus='pending' 
			WHERE EmpInfoId = @empId AND MonthValue = @month AND YearValue = @year

END
```

---

## `sp_WebAPI_Get_AdminTopBerDashboard`

**Parameters:**
- `@empId` — INT, input
- `@CompanyId` — int, input
- `@currentDate` — DATETIME, input

**Tables read (FROM/JOIN):** View_CustomerMaster, date, tblCompanyInfo, tblCompanyUnit, tblCustPayDetail, tblCustomerPay, tblInvoice, tblInvoiceDetail, tblOrder
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_CustomerMaster, date, tblCompanyInfo, tblCompanyUnit, tblCustPayDetail, tblCustomerPay, tblInvoice, tblInvoiceDetail, tblOrder, filtered by @CompanyId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT cast( CONVERT(int, @NoOfOrder) as nvarchar(max)) Order_received, cast( CONVERT(int, @NoOfInvoice) as nvarchar(max)) Total_Invoice, cast( CONVERT(int,@DeliveryConfirmed) as nvarchar(max)) Delivery_Confirmed, cast( @TotalCollection as nvarchar(max)) Total_Collection FROM @Sales END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_WebAPI_Get_AdminTopBerDashboard]  -- sp_WebAPI_Get_MioDashboardTopBarData_New 363,'24-Feb-2021'
	-- Add the parameters for the stored procedure here
    @empId INT ,
	@CompanyId int, 
    @currentDate DATETIME
AS
    BEGIN
	
	 
	
	  DECLARE @Sales TABLE 
	(

	  NoOfOrder DECIMAL(18,2),
	  NoOfInvoice DECIMAL(18,2),
	  DeliveryConfirmed DECIMAL(18,2),
	  ActualSales DECIMAL(18,2),
	  TotalCollection DECIMAL(18,2),
	  TotalDue DECIMAL(18,2)
	)


	DECLARE @NoOfOrder DECIMAL(18,2)
	DECLARE @NoOfInvoice DECIMAL(18,2)
	DECLARE @DeliveryConfirmed DECIMAL(18,2)
	DECLARE @ActualSales DECIMAL(18,2) 
	DECLARE @TotalCollection DECIMAL(18,2)
	DECLARE @TotalDue DECIMAL(18,2)


	-- In case of only from date

	IF(@currentDate IS NOT NULL)
	BEGIN 

		SELECT @NoOfOrder =  COUNT(OrderCode) FROM tblOrder 
	    WHERE ManufacId = @CompanyId AND CONVERT(date,  SubmissionDate) = CONVERT(date, @currentDate) AND ActionStatus IN ('Accepted')

		SELECT @NoOfInvoice = COUNT(INV.InvoiceNo) FROM tblInvoice AS INV 
		LEFT JOIN  tblCompanyUnit AS UT ON UT.ComUnitId = INV.ComUnitId
		LEFT JOIN tblCompanyInfo AS CI ON Ci.CompanyId = UT.CompanyId
		WHERE  CONVERT(date,INV.InvoiceDate) =  CONVERT(date,@currentDate) AND INV.InvoiceNo IS NOT NULL AND CI.CompanyId = @CompanyId

		SELECT @DeliveryConfirmed = COUNT(INV.DelivaryInvoiceNo) FROM tblInvoice AS INV 
		LEFT JOIN  tblCompanyUnit AS UT ON UT.ComUnitId = INV.ComUnitId
		LEFT JOIN tblCompanyInfo AS CI ON Ci.CompanyId = UT.CompanyId
		WHERE CONVERT(date,  INV.UpdateDate) = CONVERT(date,@currentDate) AND INV.DelivaryInvoiceNo IS NOT NULL AND INV.DeliveryInvoiceStatus IN ('Full','Partial') AND CI.CompanyId = @CompanyId

		SELECT @ActualSales = SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) 
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  
		AND  CONVERT(date,INV.UpdateDate) =  CONVERT(date,@currentDate) AND CI.CompanyId = @CompanyId

		SELECT @TotalCollection = SUM(CPD.PaymentAmount) FROM tblCustPayDetail AS CPD
		LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId
		LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE CONVERT(date,PaymentDate) =  CONVERT(date,@currentDate)  AND CI.CompanyId = @CompanyId


		SELECT @TotalDue = SUM(DeliveryTpGrandTotal- (ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0)))  FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
		LEFT JOIN ( SELECT InvoiceId,SUM(CPD.PaymentAmount) AS PaymentAmount,SUM(CPD.AIT) AIT, SUM(CPD.Discount) Discount FROM tblCustPayDetail AS CPD  
		LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId 
		WHERE CONVERT(date,CP.PaymentDate) <=  CONVERT(date,@currentDate) GROUP BY InvoiceId ) AS PMNT ON PMNT.InvoiceId = inv.InvoiceId 
		LEFT JOIN dbo.View_CustomerMaster AS VC ON INV.CustomerMasterId = VC.CustomerMasterId WHERE DeliveryInvoiceStatus IN ('Full','Partial') 
		AND CONVERT(date,INV.InvoiceDate) <=  CONVERT(date,@currentDate) AND CI.CompanyId = @CompanyId


	END

	-- -- In case of only to date

	 


	INSERT INTO @Sales
		(
		    NoOfOrder,
			NoOfInvoice,
			DeliveryConfirmed,
			ActualSales,
			TotalCollection,
			TotalDue
		)
		VALUES
		(   
		    ISNULL(@NoOfOrder,0), -- InvoiceValue - decimal(18, 2)
		    ISNULL(@NoOfInvoice,0), -- ActualSales - decimal(18, 2)
		    ISNULL(@DeliveryConfirmed,0), -- InTransit - decimal(18, 2)
		    ISNULL(@ActualSales,0),  -- Reject - decimal(18, 2)
			ISNULL(@TotalCollection,0),  -- Reject - decimal(18, 2)
			ISNULL(@TotalDue,0) 
			
		)


	--SELECT * FROM @Sales 
				 
        SELECT    cast( CONVERT(int, @NoOfOrder) as nvarchar(max)) Order_received,
   cast(  CONVERT(int,  @NoOfInvoice) as nvarchar(max)) Total_Invoice,
    cast( CONVERT(int,@DeliveryConfirmed) as nvarchar(max)) Delivery_Confirmed,
    cast( @TotalCollection as nvarchar(max)) Total_Collection   FROM @Sales 






    END
```

---

## `sp_Webapi_GET_EmpLatLong`

**Parameters:**
- `@empId` — int, input

**Tables read (FROM/JOIN):** GetAttendanceStatusWithShiftId, tblEmpLatLong
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining GetAttendanceStatusWithShiftId, tblEmpLatLong, filtered by @empId, @ShiftId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT [Lat] ,[Lon] FROM dbo.tblEmpLatLong with(nolock) where EmpInfoId=@empId and ShiftId=@ShiftId and [Lat] is not null and [Lon] is not null END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GET_EmpLatLong]
	-- Add the parameters for the stored procedure here
	@empId int 
AS
BEGIN
		
		declare @pInTime time(0)=null , @ShiftId   int=0, @AttStatus nvarchar(max)=''
set @pInTime=FORMAT(getdate(),'hh:mm tt')
		select @ShiftId=ShiftId, @AttStatus= AttStatus from  dbo.GetAttendanceStatusWithShiftId(@pInTime);
		
		SELECT  [Lat] 
      ,[Lon] FROM dbo.tblEmpLatLong  with(nolock)  where EmpInfoId=@empId and  ShiftId=@ShiftId and [Lat] is not null and  [Lon] is not null
END
```

---

## `sp_WebAPI_Get_LeaveAppCount`

**Parameters:**
- `@empId` — INT, input

**Tables read (FROM/JOIN):** tblDesignation, tblEmpGeneralInfo, tblLeaveAvail
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblDesignation, tblEmpGeneralInfo, tblLeaveAvail, filtered by @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT @LeaveCount AS LeaveAppCount END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_WebAPI_Get_LeaveAppCount]  -- sp_WebAPI_Get_MioDashboardTopBarData_New 363,'24-Feb-2021'
	-- Add the parameters for the stored procedure here
    @empId INT  
AS
    BEGIN
	
	  declare @LeaveCount nvarchar(max)='0'

	  
 select  @LeaveCount= ISNULL(count(*),'0')
 from  tblLeaveAvail  mas
 left join tblEmpGeneralInfo emp  on emp.EmpInfoId=mas.EmpInfoId
 left join tblDesignation dgs on dgs.DesignationId=emp.DesignationId where       NextEmpId=@empId  and NextEmpId is not null

        SELECT  @LeaveCount AS LeaveAppCount 





    END
```

---

## `sp_WebAPI_Get_MioDashboardTopBarData_New`

**Parameters:**
- `@empId` — INT, input
- `@currentDate` — DATETIME, input

**Tables read (FROM/JOIN):** ZASHRIS..tblEmpGeneralInfo, tblASMInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIATarget, tblMIOInfo, tblMarketAttendance_Master_webapi, tblOrder, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo, tbl_UserTracking
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining ZASHRIS..tblEmpGeneralInfo, tblASMInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIATarget, tblMIOInfo, tblMarketAttendance_Master_webapi, tblOrder, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo, tbl_UserTracking, filtered by @empId, @mioCode, @year, @month, @comId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT ISNULL(@puninTime, 0) AS PunchInTime , ISNULL(@weeklyAmount, 0) AS WeeklySaleAmount , ISNULL(@monthlyAmount, 0) AS MonthlySaleAmount , ISNULL(@orderCountToday, 0) AS OrderSubmitedToday, 'Target: '+ISNULL(@Target,0)+Char(10)+'Achieve:'+ISNULL(@Achievement,0)+'%' Targetvsachievement, CONVERT(bit`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_WebAPI_Get_MioDashboardTopBarData_New]  -- sp_WebAPI_Get_MioDashboardTopBarData_New 363,'24-Feb-2021'
	-- Add the parameters for the stored procedure here
    @empId INT ,
    @currentDate DATETIME
AS
    BEGIN
	
	 
	
        DECLARE @weekStartDate DATETIME ,
            @weekEndDate DATETIME
        DECLARE @month INT ,
            @year INT

        SET DATEFIRST 6
        SET @weekStartDate = DATEADD(DAY, 1 - DATEPART(dw, @currentDate),
                                     @currentDate)
        SET @weekEndDate = DATEADD(DAY, 7 - DATEPART(dw, @currentDate),
                                   @currentDate);

        SET @year = DATEPART(yy, @currentDate)
        SET @month = DATEPART(MM, @currentDate)



        DECLARE @mioCode NVARCHAR(50)
        SELECT  @mioCode = A.EmpMasterCode
        FROM    dbo.tblEmpGeneralInfo A
        WHERE   A.EmpInfoId = @empId 



        DECLARE @puninTime NVARCHAR(50) ,
          @ActiveStatus bit ,
            @weeklyAmount NVARCHAR(50) ,
            @monthlyAmount NVARCHAR(50) ,
            @orderCountToday NVARCHAR(50)


			
        SELECT  @puninTime = ISNULL(PunchInTime, 0)
        FROM    dbo.tblMarketAttendance_Master_webapi
        WHERE   EmpInfoId = @empId
                AND CONVERT(date, AttendanceDate) = CONVERT(date,@currentDate)



        SELECT top 1  @ActiveStatus=   ISNULL(ActiveStatus,0)   
        FROM    dbo.tbl_UserTracking   
        WHERE ActiveStatus is not null and  EmpInfoId = @empId
                AND CONVERT(date, TrackDate) = CONVERT(date,@currentDate)  order by TrackDate desc


        SELECT  @weeklyAmount = SUM(B.DeliveryNetAmount)
        FROM    dbo.tblInvoice A
                INNER JOIN dbo.tblInvoiceDetail B ON B.InvoiceId = A.InvoiceId
                INNER JOIN dbo.tblOrder C ON C.OrderId = A.OrderId
        WHERE   C.MIOCode = @mioCode
                AND DelivaryInvoiceNo IS NOT NULL
                AND DeliveryInvoiceStatus != 'Reject'
                AND A.UpdateDate BETWEEN @weekStartDate
                                 AND     @weekEndDate



       
        SELECT  @monthlyAmount = SUM(B.DeliveryNetAmount)
        FROM    dbo.tblInvoice A
                INNER JOIN dbo.tblInvoiceDetail B ON B.InvoiceId = A.InvoiceId
                INNER JOIN dbo.tblOrder C ON C.OrderId = A.OrderId
        WHERE   C.MIOCode = @mioCode
                AND DelivaryInvoiceNo IS NOT NULL
                AND DeliveryInvoiceStatus != 'Reject'
                AND DATEPART(yy, SubmissionDate) = @year
                AND DATEPART(MM, SubmissionDate) = @month

        SELECT  @orderCountToday = COUNT(*)
        FROM    dbo.tblOrder
        WHERE   CONVERT(date,SubmissionDate) = CONVERT(date,@currentDate)
                AND MIOCode = @mioCode

				declare @Target nvarchar(max)
				
  declare @Achievement nvarchar(max)


  DECLARE @Roletype NVARCHAR(MAX)=NULL
	SELECT @Roletype=RoleTypeId FROM dbo.tblUser 
	LEFT JOIN dbo.tbl_UserRoleInfo ON tbl_UserRoleInfo.UserRoleID = tblUser.UserRoleID
	WHERE EmpInfoId=@empId
IF(@Roletype='1')
	BEGIN


	select @Target= 
				cast(mas.MiaTargetAmount as int) from tblMIATarget  mas
				inner join  tblMIOInfo mio on mio.MIOId=mas.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode   
				 where mas.Year = year(@currentDate) AND  (mas.Period = format(@currentDate,'MMMM')) and EGI.EmpMasterCode=@mioCode

		select	@Achievement= cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(@Target as decimal(18,2)),0) as decimal(18,2))   FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where EGI.EmpMasterCode=@mioCode and MONTH(INV.InvoiceDate)=month(@currentDate) and year(INV.InvoiceDate)=year(@currentDate)


				 end

				 	IF(@Roletype='2')
	BEGIN

	declare @comId nvarchar(max)
	select @comId=CompanyId from tblASMInfo where EmployeeId=@empId and IsActive=1
	select @Target= 
				cast(sum(mas.MiaTargetAmount) as int) from tblMIATarget  mas
				inner join  tblMIOInfo mio on mio.MIOId=mas.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode   
				 where mas.Year = year(@currentDate) AND  (mas.Period = format(@currentDate,'MMMM')) and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )

		select	@Achievement= cast(ISNULL((SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))*100)/cast(@Target as decimal(18,2)),0) as decimal(18,2))   FROM tblInvoice AS INV  
                LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
				inner join  tblMIOInfo mio on mio.MIOId=INV.MiaId
				 inner JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId 
				 inner JOIN ZASHRIS..tblEmpGeneralInfo AS EGIHR ON EGIHR.EmpMasterCode = EGI.EmpMasterCode 
				 where   MONTH(INV.InvoiceDate)=month(@currentDate) and year(INV.InvoiceDate)=year(@currentDate) and    mio.CompanyId=@comId and  mio.TerritoryId  in (SELECT   mm.[TerritoryAreaId]   FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId ) and  [UserRoleId]=3  )

	end
				 
        SELECT  ISNULL(@puninTime, 0) AS PunchInTime ,
                ISNULL(@weeklyAmount, 0) AS WeeklySaleAmount ,
                ISNULL(@monthlyAmount, 0) AS MonthlySaleAmount ,
                ISNULL(@orderCountToday, 0) AS OrderSubmitedToday, 'Target: '+ISNULL(@Target,0)+Char(10)+'Achieve:'+ISNULL(@Achievement,0)+'%' Targetvsachievement, CONVERT(bit,ISNULL(@ActiveStatus,0))  Status,   10 Order_received,
    20 Total_Invoice,
    30 Delivery_Confirmed,
    40 Total_Collection






    END
```

---

## `sp_WebAPI_Get_OrderinfoData_ById`

**Parameters:**
- `@orderId` — INT, input

**Tables read (FROM/JOIN):** tblInvoice, tblOrder
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblInvoice, tblOrder, filtered by @orderId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT PaymentStatus FROM dbo.tblInvoice WHERE OrderId = @orderId ) IS NOT NULL THEN 1 ELSE 0 END AS IsPayment, 'No' AS IsFromCustomer FROM dbo.tblOrder WHERE OrderId = @orderId END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_WebAPI_Get_OrderinfoData_ById]
	-- Add the parameters for the stored procedure here
    @orderId INT
AS
    BEGIN
			
        SELECT  OrderId ,
                OrderCode ,
                MIOName ,
                CustomerCode ,
                CustomerName ,
                GrossValue ,
				1 AS IsMioApproved,
                CONVERT(NVARCHAR(50), SubmissionDate, 106) AS SubmissionDate ,
                1 AS IsPending ,

                1 AS IsConfirm ,

                CASE WHEN IsInvoice = 0 THEN 0
                     ELSE 1
                END AS IsInvoiced ,
                CASE WHEN ( SELECT  DelivaryInvoiceNo
                            FROM    dbo.tblInvoice
                            WHERE   OrderId = @orderId
                          ) IS NOT NULL THEN 1
                     ELSE 0
                END AS IsDelivered ,
                CASE WHEN ( SELECT  PaymentStatus
                            FROM    dbo.tblInvoice
                            WHERE   OrderId = @orderId
                          ) IS NOT NULL THEN 1
                     ELSE 0
                END AS IsPayment,
				'No' AS IsFromCustomer
        FROM    dbo.tblOrder
        WHERE   OrderId = @orderId

    END
```

---

## `sp_WebAPi_Get_PrescriptionType`

**Parameters:**
- `@empId` — int, input

**Tables read (FROM/JOIN):** tbl_PrescriptionType
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tbl_PrescriptionType.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT * FROM dbo.tbl_PrescriptionType WHERE IsActive = 1 END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_WebAPi_Get_PrescriptionType]
	-- Add the parameters for the stored procedure here
@empId int
AS
BEGIN
		
		SELECT * FROM dbo.tbl_PrescriptionType WHERE IsActive = 1

END
```

---

## `sp_Webapi_GET_ShiftEmpLatLong`

**Parameters:**
- `@empId` — int, input

**Tables read (FROM/JOIN):** GetAttendanceStatusWithShiftId, tblEmpLatLong
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining GetAttendanceStatusWithShiftId, tblEmpLatLong, filtered by @empId, @ShiftId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT [Lat] ,[Lon] FROM dbo.tblEmpLatLong with(nolock) where EmpInfoId=@empId and ShiftId=@ShiftId END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_Webapi_GET_ShiftEmpLatLong]
	-- Add the parameters for the stored procedure here
	@empId int 
AS
BEGIN

declare @pInTime time(0)=null , @ShiftId   int=0, @AttStatus nvarchar(max)=''
set @pInTime=FORMAT(getdate(),'hh:mm tt')
		select @ShiftId=ShiftId, @AttStatus= AttStatus from  dbo.GetAttendanceStatusWithShiftId(@pInTime);
		
		SELECT  [Lat]
      ,[Lon] FROM dbo.tblEmpLatLong  with(nolock)  where EmpInfoId=@empId and  ShiftId=@ShiftId
END
```

---

## `sp_WebAPI_GetAttendanceData_New`

**Parameters:**
- `@empId` — INT, input
- `@fromDate` — NVARCHAR(50), input = `null`
- `@toDate` — NVARCHAR(50), input = `null`
- `@Role` — NVARCHAR(MAX), input = `NULL`

**Tables read (FROM/JOIN):** tblASMInfo, tblEmpGeneralInfo, tblMarketAttendance_Master_webapi, tblTagASMRSMInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblEmpGeneralInfo, tblMarketAttendance_Master_webapi, tblTagASMRSMInfo, filtered by @empId, @params.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT distinct e.EmpMasterCode+'' : ''+e.EmpName EntryBy, CONVERT(NVARCHAR(50), att1.AttendanceDate, 107)AS AttendanceDate, att1.PunchInTime , att1.PInLat , att1.PInLog , att1.PunchOutTime PunchOutTime, att1.POutLat POutLat, att1.POu`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_WebAPI_GetAttendanceData_New]
	-- Add the parameters for the stored procedure here
    @empId INT ,
    @fromDate  NVARCHAR(50)=null,
    @toDate  NVARCHAR(50)=null,
@Role   NVARCHAR(MAX)= NULL
AS
    BEGIN
	
		IF @FromDate IS NOT NULL
    BEGIN
        DECLARE @FromDateString VARCHAR(20) = CONVERT(VARCHAR, @FromDate, 106);
        SET @FromDateString = REPLACE(@FromDateString, 'Sept', 'Sep');
        SET @FromDate = CONVERT(DATETIME, @FromDateString);
    END
	

		IF @ToDate IS NOT NULL
    BEGIN
        DECLARE @ToDateString VARCHAR(20) = CONVERT(VARCHAR, @ToDate, 106);
        SET @ToDateString = REPLACE(@ToDateString, 'Sept', 'Sep');
        SET @ToDate = CONVERT(DATETIME, @ToDateString);
    END

		
    DECLARE @TRID NVARCHAR(MAX),
		  @params NVARCHAR(max) =''

			  if(@Role = 'Team')
			BEGIN

			declare @comId NVARCHAR(max) 
			select @comId= CompanyId from tblASMInfo where EmployeeId=@empId
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND att1.TerritoryId in ( '+ @TRID +')'+ ' AND att1.CompanyId in ( '+ @comId +')' ;

			END
			else
			BEGIN
				 

				SET @params=@params+ ' and att1.EmpInfoId='+CAST(@empId as nvarchar(max)) ;

			END

			IF(@fromDate IS NOT NULL AND @toDate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND convert(Date, att1.AttendanceDate) between '''+CAST(CONVERT(DATE,@fromDate) AS NVARCHAR(max))+''' AND '''+CAST(CONVERT(DATE,@toDate) AS NVARCHAR(max))+''' '
		END

		  
	 	DECLARE @Q NVARCHAR(MAX)
	SET @Q='SELECT  distinct  e.EmpMasterCode+'' : ''+e.EmpName EntryBy, CONVERT(NVARCHAR(50), att1.AttendanceDate, 107)AS  AttendanceDate, 
                att1.PunchInTime ,
              
                att1.PInLat ,
                att1.PInLog  , att1.PunchOutTime  PunchOutTime,
              
                att1.POutLat POutLat,
                att1.POutLong  POutLong
        FROM    dbo.tblMarketAttendance_Master_webapi att1 
		left join tblEmpGeneralInfo e on e.EmpInfoId=att1.EmpInfoId
		 
	 	WHERE  att1.EmpInfoId  is not null '  +@params 

EXEC sys.sp_executesql @Q
    
    END
```

---

## `sp_Webapi_GetCustWiseOrderReport`

**Parameters:**
- `@empId` — int, input = `null`
- `@Role` — nvarchar(max), input = `null`
- `@Month` — int, input = `null`
- `@Year` — int, input = `null`

**Tables read (FROM/JOIN):** tblEmpGeneralInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblEmpGeneralInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT ROW_NUMBER() Over (Order by D.CustomerCode) As [SN], ISNULL(Count(D.CustomerMasterId),0) ordered_qty, D.CustomerCode+ '' : '' + D.CustomerName customer_name, convert(decimal(18,2),iSNULL(SUM(D.GrossValue),0)) as ordered_value FROM dbo.tblOrder D with (nolock) left join tblUser u with (nolock) on D.EntryBy=u.LoginName left join tblE`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetCustWiseOrderReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
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
		    SET @params=@params+ ' AND MONTH(convert(Date,  D.SubmissionDate))='''+CAST(@Month AS NVARCHAR(max))+''''
		END
		IF(@Year IS NOT NULL )
		BEGIN
		    SET @params=@params+ ' AND   year(convert(Date,  D.SubmissionDate))= '''+CAST(@Year AS NVARCHAR(max))+''''
		END

	  
	     declare @companyId NVARCHAR(50)='' 
		DECLARE @mioCode NVARCHAR(50)

        SELECT  @mioCode = usr.LoginName,@companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

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
				 

				SET @params=@params+ ' and D.EntryBy='''+convert(nvarchar(max),@mioCode)+''''

			END


DECLARE @Q NVARCHAR(MAX)
	SET @Q='SELECT ROW_NUMBER() Over (Order by D.CustomerCode) As [SN],  ISNULL(Count(D.CustomerMasterId),0) ordered_qty, D.CustomerCode+ '' : '' +	D.CustomerName customer_name,
	convert(decimal(18,2),iSNULL(SUM(D.GrossValue),0))	 as ordered_value

FROM    dbo.tblOrder D with (nolock) 
 
left join tblUser u with (nolock) on D.EntryBy=u.LoginName
left join tblEmpGeneralInfo emp with (nolock) on emp.EmpInfoId=u.EmpInfoId 

WHERE   IsFromApp = 1   '+@params+'
		
group by   D.CustomerCode,	D.CustomerName  
			 '

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetCustWiseOrderReport_N`

**Parameters:**
- `@empId` — int, input = `null`
- `@Role` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblEmpGeneralInfo, tblMIOInfo, tblOrder, tblRSMInfo, tblRoleType, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblEmpGeneralInfo, tblMIOInfo, tblOrder, tblRSMInfo, tblRoleType, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT ROW_NUMBER() Over (Order by D.CustomerCode) As [SN], ISNULL(Count(D.CustomerMasterId),0) ordered_qty, D.CustomerCode+ '' : '' + D.CustomerName customer_name, convert(decimal(18,2),iSNULL(SUM(D.GrossValue),0)) as ordered_value FROM dbo.tblOrder D with (nolock) left join tblUser u with (nolock) on D.EntryBy=u.LoginName left join tblE`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetCustWiseOrderReport_N] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
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
	SET @Q='SELECT ROW_NUMBER() Over (Order by D.CustomerCode) As [SN],  ISNULL(Count(D.CustomerMasterId),0) ordered_qty, D.CustomerCode+ '' : '' +	D.CustomerName customer_name,
	convert(decimal(18,2),iSNULL(SUM(D.GrossValue),0))	 as ordered_value

FROM    dbo.tblOrder D with (nolock) 
 
left join tblUser u with (nolock) on D.EntryBy=u.LoginName
left join tblEmpGeneralInfo emp with (nolock) on emp.EmpInfoId=u.EmpInfoId 

WHERE   IsFromApp = 1   '+@params+'
		
group by   D.CustomerCode,	D.CustomerName  
			 '

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetCustWiseSalesReport`

**Parameters:**
- `@empId` — int, input = `null`
- `@Role` — nvarchar(max), input = `null`
- `@Month` — int, input = `null`
- `@Year` — int, input = `null`

**Tables read (FROM/JOIN):** tblArea, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblTerritory, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblArea, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblTerritory, tblUser, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT ROW_NUMBER() Over (Order by CSTMR.CustomerCode) As [SN], ISNULL(Count(INV.CustomerMasterId),0) ordered_qty, CSTMR.CustomerCode+ '' : '' + CSTMR.CustomerName customer_name, SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN INVD.DeliveryNetAmount ELSE 0 END)) as ordered_value FROM dbo.tblInvoice AS INV inner join dbo.tblO`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetCustWiseSalesReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
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

	  
	  
	      declare @companyId NVARCHAR(50)='' 
	  DECLARE @mioCode NVARCHAR(50)
	  DECLARE @mioId NVARCHAR(50)

        SELECT  @mioCode = usr.LoginName,@companyId=A.CompanyId
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
	SET @Q='SELECT ROW_NUMBER() Over (Order by CSTMR.CustomerCode) As [SN],  ISNULL(Count(INV.CustomerMasterId),0) ordered_qty, CSTMR.CustomerCode+ '' : '' +	CSTMR.CustomerName customer_name,
	SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))	 as ordered_value

FROM         dbo.tblInvoice AS INV
							 	 inner join  dbo.tblOrder D  with (nolock) on INV.OrderId=D.OrderId
							   LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
							  
							  LEFT JOIN dbo.tblCustMaster AS CSTMR ON  INV.CustomerMasterId = CSTMR.CustomerMasterId
							   LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=INV.MiaId 
							   LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId 
							   LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
							   LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
							    LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId WHERE DeliveryInvoiceStatus IN (''Full'',''Partial'')

  '+@params+'
 
		
	group by CSTMR.CustomerCode,CSTMR.CustomerName,INV.CustomerMasterId 
			 '

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetCustWiseSalesReport_N`

**Parameters:**
- `@empId` — int, input = `null`
- `@Role` — nvarchar(max), input = `''`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIOInfo, tblOrder, tblRSMInfo, tblRoleType, tblTagASMRSMInfo, tblTerritory, tblUser, tbl_UserRoleInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIOInfo, tblOrder, tblRSMInfo, tblRoleType, tblTagASMRSMInfo, tblTerritory, tblUser, tbl_UserRoleInfo, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT ROW_NUMBER() Over (Order by CSTMR.CustomerCode) As [SN], ISNULL(Count(INV.CustomerMasterId),0) ordered_qty, CSTMR.CustomerCode+ '' : '' + CSTMR.CustomerName customer_name, SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN INVD.DeliveryNetAmount ELSE 0 END)) as ordered_value FROM dbo.tblInvoice AS INV inner join dbo.tblO`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetCustWiseSalesReport_N] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@Role  nvarchar(max)='',
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

 

AS
BEGIN
 

			 
		DECLARE @params NVARCHAR(max)='  '

		IF(@fromdate IS NOT NULL and @todate IS NOT NULL )
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate))  between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
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
        WHERE   A.EmployeeId = @empId  and IsActive=1

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
		else	  if(@Role = 'Team')
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
	SET @Q='SELECT ROW_NUMBER() Over (Order by CSTMR.CustomerCode) As [SN],  ISNULL(Count(INV.CustomerMasterId),0) ordered_qty, CSTMR.CustomerCode+ '' : '' +	CSTMR.CustomerName customer_name,
	SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END))	 as ordered_value

FROM         dbo.tblInvoice AS INV
							 	 inner join  dbo.tblOrder D  with (nolock) on INV.OrderId=D.OrderId
							   LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
							  
							  LEFT JOIN dbo.tblCustMaster AS CSTMR ON  INV.CustomerMasterId = CSTMR.CustomerMasterId
							   LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=INV.MiaId 
							   LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId 
							   LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId 
							   LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
							    LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId WHERE DeliveryInvoiceStatus IN (''Full'',''Partial'')

  '+@params+'
 
		
	group by CSTMR.CustomerCode,CSTMR.CustomerName,INV.CustomerMasterId 
			 '

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetEmpInfoRoleID`

**Parameters:**
- `@empId` — int, input

**Tables read (FROM/JOIN):** View_Webapi_EmployeeFieldForceInfo, View_webapi_FieldForce, tblEmpGeneralInfo, tblUser, tbl_UserRoleInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining View_Webapi_EmployeeFieldForceInfo, View_webapi_FieldForce, tblEmpGeneralInfo, tblUser, tbl_UserRoleInfo, filtered by @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT case when u.EmpInfoId is null then LoginName else EmpName end EmpName, emp.EmpMasterCode, emp.EmpInfoId, u.UserRoleID FROM dbo.tblUser u with (nolock) inner join tblEmpGeneralInfo emp with (nolock) on u.EmpInfoId= emp.EmpInfoId END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Webapi_GetEmpInfoRoleID]
	-- Add the parameters for the stored procedure here
	@empId int
AS
BEGIN



--SELECT case when  u.EmpInfoId is null then LoginName else   EmpName end EmpName, emp.EmpMasterCode, emp.EmpInfoId, u.UserRoleID FROM dbo.tblUser u with (nolock) 
--inner  join tblEmpGeneralInfo emp with (nolock)  on u.EmpInfoId=    emp.EmpInfoId
--inner  join tbl_UserRoleInfo uri with (nolock)  on u.UserRoleID=    uri.UserRoleID
--where u.UserStatus='Active'  and uri.RoleTypeId<>'2' and uri.RoleTypeId<>'3' and uri.RoleTypeId<>'4'
-- AND emp.EmpInfoId<>@empId

 SELECT case when  u.EmpInfoId is null then LoginName else   EmpName end EmpName, emp.EmpMasterCode, emp.EmpInfoId, u.UserRoleID FROM dbo.tblUser u with (nolock) 
inner  join tblEmpGeneralInfo emp with (nolock)  on u.EmpInfoId=    emp.EmpInfoId
inner join View_Webapi_EmployeeFieldForceInfo fs on emp.EmpInfoId=fs.EmpInfoId
inner  join tbl_UserRoleInfo uri with (nolock)  on u.UserRoleID=    uri.UserRoleID
where u.UserStatus='Active'  --and uri.RoleTypeId<>'2' and uri.RoleTypeId<>'3' and uri.RoleTypeId<>'4'
 --AND emp.EmpInfoId<>@empId and (fs.MIOEmpId=@empId  or fs.ASMEMPId=@empId  or fs.RSMEMPId=@empId  or fs.NSMEMPId=@empId)


union all

select distinct ASMName EmpName, ASM EmpMasterCode,ASMEMPId EmpInfoId, u.UserRoleID from View_webapi_FieldForce   vvv
inner join tblUser u on u.EmpInfoId=vvv.ASMEMPId
 --where MIOEmpId=@empId

union all

select distinct RSMName EmpName, RSMCode EmpMasterCode,RSMEMPId EmpInfoId, u.UserRoleID from View_webapi_FieldForce   vvv
inner join tblUser u on u.EmpInfoId=vvv.RSMEMPId
-- where  MIOEmpId=@empId

union all


select distinct NSMName EmpName, NSMCode EmpMasterCode,NSMEMPId EmpInfoId, u.UserRoleID from View_webapi_FieldForce   vvv
inner join tblUser u on u.EmpInfoId=vvv.NSMEMPId
 --where  MIOEmpId=@empId


UNION ALL 

SELECT case when  u.EmpInfoId is null then LoginName else   EmpName end EmpName, emp.EmpMasterCode, emp.EmpInfoId, u.UserRoleID FROM dbo.tblUser u with (nolock) 
inner  join tblEmpGeneralInfo emp with (nolock)  on u.EmpInfoId=    emp.EmpInfoId
--where u.UserStatus='Active' AND emp.EmpInfoId=@empId

END
```

---

## `sp_Webapi_GetInvoiceDetailsReport`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId, @mioCode.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT isnull(ODR.TerritoryId,0) TerritoryId , inv.InvoiceNo InvNo, ODR.CustomerCode +' : '+ODR.CustomerName Chemist , sum(INV.TpGrandTotal) invAmount FROM dbo.tblInvoice AS INV INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId INNER JOIN db`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetInvoiceDetailsReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

 

AS
BEGIN
 
 DECLARE @Q NVARCHAR(MAX)
		DECLARE @params NVARCHAR(max)='  '

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		DECLARE @companyId NVARCHAR(50)
        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 


			
		declare @TRID nvarchar(max)
			  if(@roletype = 'AM')
			BEGIN


			select     tblOrd.InvNo  ,tblOrd.Chemist,  CONVERT(decimal(18, 2),ISNULL(invAmount,0))  Amount from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   inv.InvoiceNo InvNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , sum(INV.TpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0), inv.InvoiceNo ,ODR.CustomerCode ,ODR.CustomerName   ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1   and isnull(tblOrd.invAmount,0)>0 AND   emp.CompanyId=ISNULL(@companyId,0) AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   )
							 
 
			 

			END

			  if(@roletype = 'DZSM')
			BEGIN

			 
	select     tblOrd.InvNo  ,tblOrd.Chemist,  CONVERT(decimal(18, 2),ISNULL(invAmount,0))  Amount from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		     		    INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
  isnull(ODR.RegionId,0) RegionId  ,   inv.InvoiceNo InvNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , sum(INV.TpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0), inv.InvoiceNo ,ODR.CustomerCode ,ODR.CustomerName   ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1   and isnull(tblOrd.invAmount,0)>0 AND   emp.CompanyId=ISNULL(@companyId,0) and ar.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5   )
		

		  

			END

			 if(@roletype = 'MIO')
			BEGIN
				 
		 
				select     tblOrd.InvNo  ,tblOrd.Chemist,  CONVERT(decimal(18, 2),ISNULL(invAmount,0))  Amount from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   inv.InvoiceNo InvNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , sum(INV.TpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0), inv.InvoiceNo ,ODR.CustomerCode ,ODR.CustomerName   ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1   and isnull(tblOrd.invAmount,0)>0 AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)
   
	 
			END





--EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetInvoiceDetailsReportff`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIOInfo, tblOrder, tblRoleType, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblMIOInfo, tblOrder, tblRoleType, tblTagASMRSMInfo, tblUser, tbl_UserRoleInfo, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT inv.InvoiceNo InvNo, D.CustomerCode +'' : ''+D.CustomerName Chemist , CONVERT(decimal(18, 2), (SUM((CASE WHEN INVD.NetAmount > 0 THEN INVD.NetAmount ELSE 0 END)))) AS Amount FROM tblInvoice inv WITH (NOLOCK) INNER JOIN dbo.tblOrder D WITH (NOLOCK) ON D.OrderId = inv.OrderId LEFT JOIN dbo.tblInvoiceDetail AS INVD WITH (NOLOCK`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_Webapi_GetInvoiceDetailsReportff] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

 

AS
BEGIN
 
 DECLARE @Q NVARCHAR(MAX)
		DECLARE @params NVARCHAR(max)='  '

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		DECLARE @companyId NVARCHAR(50)
        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 


			
		declare @TRID nvarchar(max)
			  if(@roletype = 'AM')
			BEGIN
			SET @params=@params+ ' AND emp.CompanyId = '+ ISNULL(@companyId,0) +' ';
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND ff.TerritoryId in ( '+ ISNULL(@TRID,0) +')';

					SET @Q='
SELECT     inv.InvoiceNo InvNo,
 D.CustomerCode +'' : ''+D.CustomerName Chemist ,
   CONVERT(decimal(18, 2), (SUM((CASE WHEN INVD.NetAmount > 0 THEN INVD.NetAmount ELSE 0 END)))) AS Amount 
FROM
       dbo.tblMIOInfo ff WITH (NOLOCK)
	  LEFT JOIN dbo.tblOrder D  WITH (NOLOCK)   ON D.TerritoryId = ff.TerritoryId   and D.IsFromApp=1
	  LEFT JOIN tblInvoice inv  WITH (NOLOCK) ON D.OrderId = inv.OrderId

LEFT JOIN dbo.tblInvoiceDetail AS INVD  WITH (NOLOCK)  ON INV.InvoiceId = INVD.InvoiceId

LEFT JOIN tblEmpGeneralInfo emp WITH (NOLOCK) ON emp.EmpInfoId = ff.EmployeeId 
LEFT JOIN tblUser u WITH (NOLOCK) ON ff.EmployeeId = u.EmpInfoId 
		left join tbl_UserRoleInfo ur  with (nolock) on ur.UserRoleID=u.UserRoleID
				left join tblRoleType urp  with (nolock) on ur.RoleTypeId=urp.RoleTypeId
WHERE     ff.isactive=1 and   isnull( u.EmpInfoId,0)>0  


   '+@params+'
group by inv.InvoiceNo,  D.CustomerCode +'' : ''+D.CustomerName

	order by D.CustomerCode +'' : ''+D.CustomerName'

			END

			  if(@roletype = 'DZSM')
			BEGIN

			SET @params=@params+ ' AND emp.CompanyId = '+ ISNULL(@companyId,0) +' ';
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')

				--SET @params=@params+ ' AND D.AreaId in ( '+ ISNULL(@TRID,0) +')';
			SET @params=@params+ '	AND  ff.AreaId in( '+ ISNULL(@TRID,0) +')';

					SET @Q='
SELECT   inv.InvoiceNo InvNo,
 D.CustomerCode +'' : ''+D.CustomerName Chemist ,
   CONVERT(decimal(18, 2), (SUM((CASE WHEN INVD.NetAmount > 0 THEN INVD.NetAmount ELSE 0 END)))) AS Amount 
FROM   tblASMInfo ff WITH (NOLOCK)
	  LEFT JOIN dbo.tblOrder D  WITH (NOLOCK)   ON D.areaId = ff.areaId   and D.IsFromApp=1
	  LEFT JOIN tblInvoice inv  WITH (NOLOCK) ON D.OrderId = inv.OrderId

LEFT JOIN dbo.tblInvoiceDetail AS INVD  WITH (NOLOCK)  ON INV.InvoiceId = INVD.InvoiceId

LEFT JOIN tblEmpGeneralInfo emp WITH (NOLOCK) ON emp.EmpInfoId = ff.EmployeeId 
LEFT JOIN tblUser u WITH (NOLOCK) ON ff.EmployeeId = u.EmpInfoId 
		left join tbl_UserRoleInfo ur  with (nolock) on ur.UserRoleID=u.UserRoleID
				left join tblRoleType urp  with (nolock) on ur.RoleTypeId=urp.RoleTypeId
WHERE     ff.isactive=1 and   isnull( u.EmpInfoId,0)>0  

   '+@params+'
		
group by inv.InvoiceNo,  D.CustomerCode +'' : ''+D.CustomerName

	order by D.CustomerCode +'' : ''+D.CustomerName'

			END

			 if(@roletype = 'MIO')
			BEGIN
				 

				SET @params=@params+ ' and D.EntryBy='''+convert(nvarchar(max),@mioCode)+''''


					SET @Q='
SELECT    inv.InvoiceNo InvNo,
 D.CustomerCode +'' : ''+D.CustomerName Chemist ,
   CONVERT(decimal(18, 2), (SUM((CASE WHEN INVD.NetAmount > 0 THEN INVD.NetAmount ELSE 0 END)))) AS Amount 
FROM
     tblInvoice inv WITH (NOLOCK)
INNER JOIN dbo.tblOrder D   WITH (NOLOCK) ON D.OrderId = inv.OrderId
LEFT JOIN dbo.tblInvoiceDetail AS INVD   WITH (NOLOCK) ON INV.InvoiceId = INVD.InvoiceId
LEFT JOIN tblUser u WITH (NOLOCK) ON D.EntryBy = u.LoginName
LEFT JOIN tblEmpGeneralInfo emp WITH (NOLOCK) ON emp.EmpInfoId = u.EmpInfoId 
		left join tbl_UserRoleInfo ur  with (nolock) on ur.UserRoleID=u.UserRoleID
				left join tblRoleType urp  with (nolock) on ur.RoleTypeId=urp.RoleTypeId
WHERE
    D.OrderId IS NOT NULL and   D.IsFromApp=1  and isnull( u.EmpInfoId,0)>0


   '+@params+'
		
group by inv.InvoiceNo,  D.CustomerCode +'' : ''+D.CustomerName

	order by D.CustomerCode +'' : ''+D.CustomerName'
			END





EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetInvoiceDetailsReportForWings`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId, @mioCode.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT isnull(ODR.TerritoryId,0) TerritoryId , inv.InvoiceNo InvNo, ODR.CustomerCode +' : '+ODR.CustomerName Chemist , sum(INV.TpGrandTotal) invAmount FROM dbo.tblInvoice AS INV INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId INNER JOIN db`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetInvoiceDetailsReportForWings] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

 

AS
BEGIN
 
 DECLARE @Q NVARCHAR(MAX)
		DECLARE @params NVARCHAR(max)='  '

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		DECLARE @companyId NVARCHAR(50)
        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 


			
		declare @TRID nvarchar(max)
			  if(@roletype = 'AM')
			BEGIN


		SELECT  INV.InvoiceNo InvNo,CSTMR.CustomerCode +' : '+ CSTMR.CustomerName Chemist,SUM(ISNULL(DeliveryTpGrandTotal,0)) AS Amount FROM tblInvoice AS INV
		LEFT JOIN tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
		LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId
		LEFT JOIN tblMIOInfo AS MIO ON ODR.MIOId = MIO.MIOId
		LEFT JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
		LEFT JOIN tblASMInfo AS ASM ON ODR.ASMId = ASM.ASMId
		LEFT JOIN tblEmpGeneralInfo AS EGIA ON ASM.EmployeeId = EGIA.EmpInfoId
		LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND UNT.CompanyId = ISNULL(@companyId,0) 
		AND InvoiceDate BETWEEN CONVERT(Date, @fromdate) AND CONVERT(Date,@todate) AND EGIA.EmpInfoId = @empId 
		GROUP BY INV.InvoiceNo,CSTMR.CustomerCode,CSTMR.CustomerName 
			 

			END

			  if(@roletype = 'DZSM')
			BEGIN

			 
	select     tblOrd.InvNo  ,tblOrd.Chemist,  CONVERT(decimal(18, 2),ISNULL(invAmount,0))  Amount from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		     		    INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
  isnull(ODR.RegionId,0) RegionId  ,   inv.InvoiceNo InvNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , sum(INV.TpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0), inv.InvoiceNo ,ODR.CustomerCode ,ODR.CustomerName   ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1   and isnull(tblOrd.invAmount,0)>0 AND   emp.CompanyId=ISNULL(@companyId,0) and ar.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5   )
		

		  

			END

			 if(@roletype = 'MIO')
			BEGIN
				 
		 
				select     tblOrd.InvNo  ,tblOrd.Chemist,  CONVERT(decimal(18, 2),ISNULL(invAmount,0))  Amount from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   inv.InvoiceNo InvNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , sum(INV.TpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0), inv.InvoiceNo ,ODR.CustomerCode ,ODR.CustomerName   ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1   and isnull(tblOrd.invAmount,0)>0 AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)
   
	 
			END





--EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetInvoiceStatusMasterReport`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT isnull(ODR.TerritoryId,0) TerritoryId,EGI.EmpMasterCode, count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount FROM dbo.tblInvoice AS INV INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId INNER JOIN dbo.tblCompanyUnit A`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetInvoiceStatusMasterReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

 

AS
BEGIN
 
     DECLARE @Q NVARCHAR(MAX)
     DECLARE @params NVARCHAR(max)='  '

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		DECLARE @companyId NVARCHAR(50)
        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 


			
		declare @TRID nvarchar(max)
			  if(@roletype = 'AM')
			BEGIN


		--	SELECT DISTINCT 0 id,ROW_NUMBER() OVER (ORDER BY EGI.EmpMasterCode) AS sn,'MIO' roletype,EGI.EmpMasterCode+' : '+EGI.EmpName name,
		--UNT.CompanyId,EGI.EmpInfoId AS empid,COUNT(InvoiceId) AS invNumber,SUM(ISNULL(DeliveryTpGrandTotal,0)) AS invAmount FROM tblInvoice AS INV
		--LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId
		--LEFT JOIN tblMIOInfo AS MIO ON ODR.MIOId = MIO.MIOId
		--LEFT JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
		--LEFT JOIN tblASMInfo AS ASM ON ODR.ASMId = ASM.ASMId
		--LEFT JOIN tblEmpGeneralInfo AS EGIA ON ASM.EmployeeId = EGIA.EmpInfoId
		--LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		--WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND UNT.CompanyId = ISNULL(@companyId,0) 
		--AND InvoiceDate BETWEEN CONVERT(Date, @fromdate) AND CONVERT(Date,@todate) AND EGIA.EmpInfoId = @empId 
		--GROUP BY UNT.CompanyId,EGI.EmpInfoId,EGI.EmpMasterCode,EGI.EmpName
							 
	select    'MIO' RoleType,
    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
    0 AS id,
     isnull( emp.EmpInfoId,0) AS empid,
    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    
			--and isnull(tblOrd.invAmount,0)>0 
		 where   B.IsActive=1   AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and B.CompanyId=ISNULL(@companyId,0)

   
	group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
ORDER BY
    emp.EmpMasterCode; 

			 

			END

			  if(@roletype = 'DZSM')
			BEGIN

			 


		

		 

				 
	select    'AM' RoleType,
    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
    0 AS id,
     isnull( emp.EmpInfoId,0) AS empid,
    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		    INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
    isnull(ODR.RegionId,0) RegionId  ,   count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount,EGI.EmpMasterCode  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0),EGI.EmpMasterCode) tblOrd on tblOrd.RegionId=ar.RegionId AND EMP.EmpMasterCode = tblOrd.EmpMasterCode
		  


		 where   B.IsActive=1    AND ar.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5 ) and B.CompanyId=ISNULL(@companyId,0) 

   
	group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
ORDER BY
    emp.EmpMasterCode; 

			END

			 if(@roletype = 'MIO')
			BEGIN
				 

				SELECT  'MIO' RoleType,
    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
    0 AS id,
     isnull( emp.EmpInfoId,0) AS empid,
    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   FROM tblMIOInfo B
INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
LEFT JOIN (SELECT isnull(ODR.TerritoryId,0) TerritoryId,EGI.EmpMasterCode, count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
							 where  ODR.ManufacId = ISNULL(@companyId,0) and  (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0),EGI.EmpMasterCode) tblOrd on tblOrd.TerritoryId=b.TerritoryId 
							 --AND EMP.EmpMasterCode = tblOrd.EmpMasterCode
							 where   B.IsActive=1  AND  emp.EmpMasterCode=@empId   and b.CompanyId=ISNULL(@companyId,0) group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
ORDER BY
    emp.EmpMasterCode; 
			END





--EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetInvoiceStatusMasterReportForASMClick`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT isnull(ODR.TerritoryId,0) TerritoryId,EGI.EmpMasterCode, count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount FROM dbo.tblInvoice AS INV INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId INNER JOIN dbo.tblCompanyUnit A`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetInvoiceStatusMasterReportForASMClick] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

 

AS
BEGIN
 
     DECLARE @Q NVARCHAR(MAX)
     DECLARE @params NVARCHAR(max)='  '

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		DECLARE @companyId NVARCHAR(50)
        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 


			
		declare @TRID nvarchar(max)
			  if(@roletype = 'AM')
			BEGIN


			SELECT DISTINCT 0 id,ROW_NUMBER() OVER (ORDER BY EGI.EmpMasterCode) AS sn,'MIO' roletype,EGI.EmpMasterCode+' : '+EGI.EmpName name,
		UNT.CompanyId,EGI.EmpInfoId AS empid,COUNT(InvoiceId) AS invNumber,SUM(ISNULL(DeliveryTpGrandTotal,0)) AS invAmount FROM tblInvoice AS INV
		LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId
		LEFT JOIN tblMIOInfo AS MIO ON ODR.MIOId = MIO.MIOId
		LEFT JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
		LEFT JOIN tblASMInfo AS ASM ON ODR.ASMId = ASM.ASMId
		LEFT JOIN tblEmpGeneralInfo AS EGIA ON ASM.EmployeeId = EGIA.EmpInfoId
		LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND UNT.CompanyId = ISNULL(@companyId,0) 
		AND InvoiceDate BETWEEN CONVERT(Date, @fromdate) AND CONVERT(Date,@todate) AND EGIA.EmpInfoId = @empId 
		GROUP BY UNT.CompanyId,EGI.EmpInfoId,EGI.EmpMasterCode,EGI.EmpName 
							 
	--select    'MIO' RoleType,
 --   ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
 --   0 AS id,
 --    isnull( emp.EmpInfoId,0) AS empid,
 --   isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   from tblMIOInfo B
	--	   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

	--	      left join ( SELECT    
 --   isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
 --                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
 --                            INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
 --                            INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
	--						 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
 --                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
	--						 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    
	--		--and isnull(tblOrd.invAmount,0)>0 
	--	 where   B.IsActive=1   AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and B.CompanyId=ISNULL(@companyId,0)

   
	--group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
--ORDER BY
--    emp.EmpMasterCode; 

			 

			END

			  if(@roletype = 'DZSM')
			BEGIN

			 


		

		 

				 
	select    'AM' RoleType,
    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
    0 AS id,
     isnull( emp.EmpInfoId,0) AS empid,
    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		    INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
    isnull(ODR.RegionId,0) RegionId  ,   count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount,EGI.EmpMasterCode  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0),EGI.EmpMasterCode) tblOrd on tblOrd.RegionId=ar.RegionId AND EMP.EmpMasterCode = tblOrd.EmpMasterCode
		  


		 where   B.IsActive=1    AND ar.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5 ) and B.CompanyId=ISNULL(@companyId,0) 

   
	group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
ORDER BY
    emp.EmpMasterCode; 

			END

			 if(@roletype = 'MIO')
			BEGIN
				 

				SELECT  'MIO' RoleType,
    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
    0 AS id,
     isnull( emp.EmpInfoId,0) AS empid,
    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   FROM tblMIOInfo B
INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
LEFT JOIN (SELECT isnull(ODR.TerritoryId,0) TerritoryId,EGI.EmpMasterCode, count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
							 where  ODR.ManufacId = ISNULL(@companyId,0) and  (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0),EGI.EmpMasterCode) tblOrd on tblOrd.TerritoryId=b.TerritoryId 
							 --AND EMP.EmpMasterCode = tblOrd.EmpMasterCode
							 where   B.IsActive=1  AND  emp.EmpMasterCode=@empId   and b.CompanyId=ISNULL(@companyId,0) group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
ORDER BY
    emp.EmpMasterCode; 
			END





--EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetInvoiceStatusMasterReportNew`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`
- `@CompanyId` — int, input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblRSMInfo, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblRSMInfo, tblTagASMRSMInfo, tblUser, filtered by @empId, @params.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT DISTINCT 0 id,ROW_NUMBER() OVER (ORDER BY EGI.EmpMasterCode) AS sn,'MIO' roletype,EGI.EmpMasterCode+' : '+EGI.EmpName name, UNT.CompanyId,EGI.EmpInfoId AS empid,COUNT(InvoiceId) AS invNumber,SUM(ISNULL(DeliveryTpGrandTotal,0)) AS invAmount FROM tblInvoice AS INV LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId LEFT JOIN tblMIOInf`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetInvoiceStatusMasterReportNew] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	
	@empId int = null,
	@roletype  nvarchar(max)=null, 
	@fromdate  nvarchar(max)=null,
	@todate   nvarchar(max)=null,
	@CompanyId int = null
 

AS
BEGIN
 

	-- DZSM Role (Will get AM Sales)

	IF(UPPER(RTRIM(LTRIM(@roletype))) = UPPER(RTRIM(LTRIM('DZSM'))))
	BEGIN
		
		SELECT DISTINCT 0 id,ROW_NUMBER() OVER (ORDER BY EGI.EmpMasterCode) AS sn,'AM' roletype,EGI.EmpMasterCode+' : '+EGI.EmpName name,
		UNT.CompanyId,EGI.EmpInfoId AS empid,COUNT(InvoiceId) AS invNumber,SUM(ISNULL(DeliveryTpGrandTotal,0)) AS invAmount FROM tblInvoice AS INV
		LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId
		LEFT JOIN tblASMInfo AS ASM ON ODR.ASMId = ASM.ASMId
		LEFT JOIN tblEmpGeneralInfo AS EGI ON ASM.EmployeeId = EGI.EmpInfoId
		LEFT JOIN tblRSMInfo AS RSM ON ODR.RSMId = RSM.RSMId
		LEFT JOIN tblEmpGeneralInfo AS EGIR ON RSM.EmployeeId = EGIR.EmpInfoId
		LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND UNT.CompanyId = ISNULL(@companyId,0) 
		AND InvoiceDate BETWEEN CONVERT(Date, @fromdate) AND CONVERT(Date,@todate) AND EGIR.EmpInfoId = @empId  
		GROUP BY UNT.CompanyId,EGI.EmpInfoId,EGI.EmpMasterCode,EGI.EmpName

	END

	-- AM Role (Will get MIO Sales)

	IF(UPPER(RTRIM(LTRIM(@roletype))) = UPPER(RTRIM(LTRIM('AM'))))
	BEGIN
		
		SELECT DISTINCT 0 id,ROW_NUMBER() OVER (ORDER BY EGI.EmpMasterCode) AS sn,'MIO' roletype,EGI.EmpMasterCode+' : '+EGI.EmpName name,
		UNT.CompanyId,EGI.EmpInfoId AS empid,COUNT(InvoiceId) AS invNumber,SUM(ISNULL(DeliveryTpGrandTotal,0)) AS invAmount FROM tblInvoice AS INV
		LEFT JOIN tblOrder AS ODR ON INV.OrderId = ODR.OrderId
		LEFT JOIN tblMIOInfo AS MIO ON ODR.MIOId = MIO.MIOId
		LEFT JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
		LEFT JOIN tblASMInfo AS ASM ON ODR.ASMId = ASM.ASMId
		LEFT JOIN tblEmpGeneralInfo AS EGIA ON ASM.EmployeeId = EGIA.EmpInfoId
		LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND UNT.CompanyId = ISNULL(@companyId,0) 
		AND InvoiceDate BETWEEN CONVERT(Date, @fromdate) AND CONVERT(Date,@todate) AND EGIA.EmpInfoId = @empId 
		GROUP BY UNT.CompanyId,EGI.EmpInfoId,EGI.EmpMasterCode,EGI.EmpName

	END


END


--     DECLARE @Q NVARCHAR(MAX)
--     DECLARE @params NVARCHAR(max)='  '

--	IF(@fromdate IS NOT NULL)
--		BEGIN
--		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
--		END
		 

	  
	  
--		DECLARE @mioCode NVARCHAR(50)
--		DECLARE @companyId NVARCHAR(50)
--        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
--        FROM    dbo.tblEmpGeneralInfo A with (nolock)

--		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
--        WHERE   A.EmpInfoId = @empId 


			
--		declare @TRID nvarchar(max)
--			  if(@roletype = 'AM')
--			BEGIN
							 
--	select    'MIO' RoleType,
--    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
--    0 AS id,
--     isnull( emp.EmpInfoId,0) AS empid,
--    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   from tblMIOInfo B
--		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

--		      left join ( SELECT    
--    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
--                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
--                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
--                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
--							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
--                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
--							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

--		 where   B.IsActive=1   AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and B.CompanyId=ISNULL(@companyId,0)

   
--	group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
--ORDER BY
--    emp.EmpMasterCode; 

			 

--			END

--			  if(@roletype = 'DZSM')
--			BEGIN

			 


		

		 

				 
--	select    'AM' RoleType,
--    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
--    0 AS id,
--     isnull( emp.EmpInfoId,0) AS empid,
--    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   from tblASMInfo B
--		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
--		    INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
--		      left join ( SELECT    
--    isnull(ODR.RegionId,0) RegionId  ,   count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount,EGI.EmpMasterCode  FROM dbo.tblInvoice AS INV 
--                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
--                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
--                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
--							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
--                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
--							 where   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0),EGI.EmpMasterCode) tblOrd on tblOrd.RegionId=ar.RegionId AND EMP.EmpMasterCode = tblOrd.EmpMasterCode
		  


--		 where   B.IsActive=1    AND ar.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5 ) and B.CompanyId=ISNULL(@companyId,0) 

   
--	group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
--ORDER BY
--    emp.EmpMasterCode; 

--			END

--			 if(@roletype = 'MIO')
--			BEGIN
				 

--				SELECT  'MIO' RoleType,
--    ROW_NUMBER() OVER (ORDER BY emp.EmpMasterCode) AS sn,
--    0 AS id,
--     isnull( emp.EmpInfoId,0) AS empid,
--    isnull( emp.EmpName + ' : ' + emp.EmpMasterCode,'N/A' )    AS name,     isnull(sum(tblOrd.invAmount),0) invAmount, isnull(sum(tblOrd.invNumber),0) invNumber   FROM tblMIOInfo B
--INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
--LEFT JOIN (SELECT isnull(ODR.TerritoryId,0) TerritoryId,EGI.EmpMasterCode, count(distinct INV.InvoiceNo) invNumber, sum(INV.DeliveryTpGrandTotal) invAmount  FROM dbo.tblInvoice AS INV 
--                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
--                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
--                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
--							 INNER JOIN tblMIOInfo AS DLV ON DLV.MIOId = INV.MiaId
--                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmployeeId
--							 where  ODR.ManufacId = ISNULL(@companyId,0) and  (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0),EGI.EmpMasterCode) tblOrd on tblOrd.TerritoryId=b.TerritoryId 

--							 where   B.IsActive=1  AND  emp.EmpMasterCode=@empId   and b.CompanyId=ISNULL(@companyId,0) group by emp.EmpInfoId ,  emp.EmpMasterCode,emp.EmpName
--ORDER BY
--    emp.EmpMasterCode; 
--			END





--EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')
```

---

## `sp_Webapi_GetNotification`

**Parameters:**
- `@empId` — INT, input

**Tables read (FROM/JOIN):** tbl_Notification
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tbl_Notification.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT NotificationId , NotificationType , NotificationPrimaryText , NotificationMainText AS NotificationRestText , EntryBy , EntryDate , IsRead , EmpInfoId,PrimaryId FROM dbo.tbl_Notification ORDER BY IsRead ASC END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetNotification]
	-- Add the parameters for the stored procedure here
@empId INT
AS
BEGIN
	
	SELECT NotificationId ,
           NotificationType ,
           NotificationPrimaryText ,
          NotificationMainText AS  NotificationRestText ,
           EntryBy ,
           EntryDate ,
           IsRead ,
           EmpInfoId,PrimaryId FROM dbo.tbl_Notification ORDER BY IsRead ASC

END
```

---

## `sp_Webapi_GetNotificationCount`

**Parameters:**
- `@empId` — int, input

**Tables read (FROM/JOIN):** tbl_Notification
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tbl_Notification, filtered by @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT COUNT(*) AS NotificationCount FROM dbo.tbl_Notification (NOLOCK) WHERE IsRead = 0 AND EmpInfoId = @empId END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetNotificationCount]
	-- Add the parameters for the stored procedure here
@empId int
AS
BEGIN
		

		SELECT COUNT(*) AS NotificationCount FROM dbo.tbl_Notification (NOLOCK) WHERE IsRead = 0 
		
		AND EmpInfoId = @empId




END
```

---

## `sp_Webapi_GetOrder_TrackingList`

**Parameters:**
- `@empId` — int, input = `null`
- `@FromDate` — NVARCHAR(50), input = `null`
- `@ToDate` — NVARCHAR(50), input = `null`
- `@Role` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblCustomerReturnMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblCustomerReturnMaster, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @empId, @params, @EMPId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT inv.InvoiceId FROM dbo. tblCustomerReturnMaster with (nolock) inner join tblInvoice inv with (nolock) on tblCustomerReturnMaster.InvoiceId=inv.InvoiceId WHERE inv.OrderId = D.OrderId ) IS NOT NULL THEN ''Returned'' WHEN I`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetOrder_TrackingList] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
@empId int = null,
@FromDate   NVARCHAR(50)=null,
@ToDate    NVARCHAR(50)=null ,
@Role  nvarchar(max)=null




AS
BEGIN
 

		DECLARE @mioCode NVARCHAR(50),
		  @EmpCode NVARCHAR(50),
		  @ComId NVARCHAR(50)



        SELECT  @mioCode = usr.LoginName, @ComId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

		IF @FromDate IS NOT NULL
    BEGIN
        DECLARE @FromDateString VARCHAR(20) = CONVERT(VARCHAR, @FromDate, 106);
        SET @FromDateString = REPLACE(@FromDateString, 'Sept', 'Sep');
        SET @FromDate = CONVERT(DATETIME, @FromDateString);
    END
	

		IF @ToDate IS NOT NULL
    BEGIN
        DECLARE @ToDateString VARCHAR(20) = CONVERT(VARCHAR, @ToDate, 106);
        SET @ToDateString = REPLACE(@ToDateString, 'Sept', 'Sep');
        SET @ToDate = CONVERT(DATETIME, @ToDateString);
    END

		DECLARE @params NVARCHAR(max)='  '

		IF(@FromDate IS NOT NULL AND @ToDate IS NULL)
		BEGIN
		    SET @params=@params+ ' AND  convert(Date,  D.SubmissionDate)='''+CAST(CONVERT(DATE,@FromDate) AS NVARCHAR(max))+''''
		END
		IF(@FromDate IS NOT NULL AND @ToDate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  convert(Date,  D.SubmissionDate) between '''+CAST(CONVERT(DATE,@FromDate) AS NVARCHAR(max))+''' AND '''+ CAST(CONVERT(DATE,@ToDate) AS NVARCHAR(max))+''' '
		END

		declare @TRID nvarchar(max)
			  if(@Role = 'Team')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ @TRID +')';
				 SET @params=@params+ ' and D.ManufacId='''+convert(nvarchar(max),@ComId)+''''
			END
			else
			BEGIN


			select 
                @mioCode = B.MIOId   from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo A ON A.EmpInfoId = B.EmployeeId
		    INNER JOIN dbo.tblUser ur ON A.EmpInfoId = ur.EmpInfoId
		 where EmployeeId=@EMPId and IsActive=1  

		  --SET @params=@params+ ' and D.MIOId='''+convert(nvarchar(max),@mioCode)+''''
		
		 if(@mioCode is not null)
		 begin 
		   SET @params=@params+ ' and D.MIOId='''+convert(nvarchar(max),@mioCode)+''''
		 end
		 else
		 begin
		 	 SET @params=@params+ ' and D.MIOId='''+convert(nvarchar(max),@EMPId)+''''
		 	

		 end
		 
		  end
DECLARE @Q NVARCHAR(MAX)
	SET @Q='SELECT  distinct     ''''   ApprovalStatus, '''' CustomerType,  
		D.OrderId,
		D.OrderCode,
		isnull(D.MIOName,''N/A'') MIOName,
		D.CustomerCode+ '' : '' +	D.CustomerName CustomerName,
		isnull(D.GrossValue,0) as GrossValue,
		FORMAT(D.EntryDate,''dd MMM, yyyy hh:mm tt'') CreatedAt,

	  emp.empmastercode +'' : ''+ emp.empname    CreatedBy,
		CONVERT(NVARCHAR(50),D.SubmissionDate,106) AS SubmissionDate,
		isnull(( SELECT  isnull(PaymentStatus,'''')
                            FROM    dbo.tblInvoice with (nolock)
                            WHERE   OrderId = D.OrderId
                          ),'''') AS dsd,
		''Own''  AS OrderType,
		 CASE		WHEN ( SELECT  PaymentStatus
                            FROM    dbo.tblInvoice with (nolock)
                            WHERE   OrderId = D.OrderId
                          ) IS NOT NULL THEN ''Payment Completed''

						  WHEN ( SELECT  DelivaryInvoiceNo
                            FROM    dbo.tblInvoice with (nolock)
                            WHERE   OrderId = D.OrderId
                          ) IS NOT NULL THEN ''Payment Completed''

						  WHEN ( SELECT  inv.InvoiceId
                            FROM    dbo. tblCustomerReturnMaster with (nolock)
							inner join tblInvoice  inv  with (nolock) on tblCustomerReturnMaster.InvoiceId=inv.InvoiceId
							 
                            WHERE   inv.OrderId = D.OrderId
                          ) IS NOT NULL THEN ''Returned''
						   

					WHEN IsInvoice = 1 THEN ''Invoiced''
					WHEN IsInvoice = 0 THEN ''Invoice Pending''
					WHEN IsInvoice IS NULL THEN ''Pending''
					END AS OrderStatus

FROM    dbo.tblOrder D with (nolock) 
  
left join tblUser u with (nolock) on D.EntryBy=u.LoginName
left join tblEmpGeneralInfo emp with (nolock) on emp.EmpInfoId=u.EmpInfoId 

where D.IsFromApp=1 '+@params+'
			ORDER BY OrderId DESC

			 '

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_WebAPi_GetOrderDetails_New`

**Parameters:**
- `@orderId` — int, input

**Tables read (FROM/JOIN):** tblOrderDetail, tblProduct
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblOrderDetail, tblProduct, filtered by @orderId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT (B.ProductCode +' : ' + B.ProductName) AS ProductName, CAST(Quantity AS INT) Quantity, ISNULL(TradePrice,0) AS TotalTradePrice, ISNULL(TotalVatAmount,0) AS TotalVatAmount, ISNULL(NetAmount,0) AS NetAmount FROM dbo.tblOrderDetail LEFT JOIN dbo.tblProduct B ON B.ProductId = tblOrderDetail.ProductId WHERE OrderId = @`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_WebAPi_GetOrderDetails_New]  -- sp_WebAPi_GetOrderDetails_New 90672
	-- Add the parameters for the stored procedure here
@orderId int
AS
BEGIN
		
		  SELECT  (B.ProductCode +' : ' + B.ProductName) AS  ProductName,
        CAST(Quantity AS INT) Quantity,
       ISNULL(TradePrice,0) AS  TotalTradePrice,
		ISNULL(TotalVatAmount,0) AS TotalVatAmount,
		ISNULL(NetAmount,0) AS NetAmount
FROM    dbo.tblOrderDetail
	LEFT JOIN dbo.tblProduct B ON B.ProductId = tblOrderDetail.ProductId
WHERE   OrderId = @orderId
END
```

---

## `sp_Webapi_GetOrderStatusDetailsReport`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`
- `@Menutype` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId, @mioCode.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT isnull(ODR.TerritoryId,0) TerritoryId , ODR.OrderCode OrdNo, ODR.CustomerCode +' : '+ODR.CustomerName Chemist , ODR.MIOCode +' : '+ODR.MIOName CreatedBy, CONVERT(decimal(18, 2), SUM(INV.TpGrandTotal)) AS Amount FROM dbo.tblInvoice AS INV INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.C`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetOrderStatusDetailsReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null,
@Menutype   nvarchar(max)=null

 

AS
BEGIN
 

		DECLARE @params NVARCHAR(max)='  '

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		DECLARE @companyId NVARCHAR(50)
        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

		declare @InvDone nvarchar(max)='0' , @PendingDelivery nvarchar(max)='0' , @DeliveryDone nvarchar(max)='0', @InvReturn  nvarchar(max)='0'
		
		  
			  if(@roletype = 'AM')
			BEGIN
			  
 
  
  if(@Menutype='Pending')
		begin
     

	SELECT  distinct   OrdNo ,Chemist, CreatedBy,Amount      from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,    ODR.OrderCode OrdNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , ODR.MIOCode +' : '+ODR.MIOName CreatedBy,
   CONVERT(decimal(18, 2),  SUM(INV.TpGrandTotal)) AS Amount    FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where inv.DeliveryInvoiceStatus is null and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)  , ODR.OrderCode  ,
 ODR.CustomerCode +' : '+ODR.CustomerName   , ODR.MIOCode +' : '+ODR.MIOName   ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and emp.CompanyId=ISNULL(@companyId,0)

     end

	 
	  	if(@Menutype='Return')
		begin

		 
		SELECT  distinct OrdNo ,Chemist, CreatedBy,Amount   from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId   ,    ODR.OrderCode OrdNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , ODR.MIOCode +' : '+ODR.MIOName CreatedBy,
   CONVERT(decimal(18, 2),  SUM(INV.TpGrandTotal)) AS Amount   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Reject') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0) , ODR.OrderCode  ,
 ODR.CustomerCode +' : '+ODR.CustomerName   , ODR.MIOCode +' : '+ODR.MIOName    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and emp.CompanyId=ISNULL(@companyId,0)

			END
			END

			 if(@roletype = 'DZSM')
			BEGIN
			 
			    if(@Menutype='Pending')
		begin
     

	SELECT distinct  OrdNo ,Chemist, CreatedBy,Amount      from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		       INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId

		      left join ( SELECT    
    isnull(ODR.RegionId,0) RegionId  ,         ODR.OrderCode OrdNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , ODR.MIOCode +' : '+ODR.MIOName CreatedBy,
   CONVERT(decimal(18, 2),  SUM(INV.TpGrandTotal)) AS Amount     FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where inv.DeliveryInvoiceStatus is null and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0)  , ODR.OrderCode  ,
 ODR.CustomerCode +' : '+ODR.CustomerName   , ODR.MIOCode +' : '+ODR.MIOName   ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1  AND b.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  ) and emp.CompanyId=ISNULL(@companyId,0)

     end

	 

		 	if(@Menutype='Return')
		begin

		 
		SELECT  distinct  OrdNo ,Chemist, CreatedBy,Amount        from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		       INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
    isnull(ODR.RegionId,0) RegionId  ,          ODR.OrderCode OrdNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , ODR.MIOCode +' : '+ODR.MIOName CreatedBy,
   CONVERT(decimal(18, 2),  SUM(INV.TpGrandTotal)) AS Amount    FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Reject') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by isnull(ODR.RegionId,0)   , ODR.OrderCode  ,
 ODR.CustomerCode +' : '+ODR.CustomerName   , ODR.MIOCode +' : '+ODR.MIOName   ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1  AND b.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5   ) and emp.CompanyId=ISNULL(@companyId,0)
			 
			  
			END
			END

			 if(@roletype = 'MIO')
			BEGIN
				 

				     if(@Menutype='Pending')
		begin
     

	SELECT  distinct
  OrdNo ,Chemist, CreatedBy,Amount          from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId   ,          ODR.OrderCode OrdNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , ODR.MIOCode +' : '+ODR.MIOName CreatedBy,
   CONVERT(decimal(18, 2),  SUM(INV.TpGrandTotal)) AS Amount    FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where inv.DeliveryInvoiceStatus is null and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)   , ODR.OrderCode  ,
 ODR.CustomerCode +' : '+ODR.CustomerName   , ODR.MIOCode +' : '+ODR.MIOName   ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)

     

	 end

	 
	 	 	if(@Menutype='Return')
		begin
		 
		SELECT  distinct
   OrdNo ,Chemist, CreatedBy,Amount          from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId   ,          ODR.OrderCode OrdNo,
 ODR.CustomerCode +' : '+ODR.CustomerName Chemist , ODR.MIOCode +' : '+ODR.MIOName CreatedBy,
   CONVERT(decimal(18, 2),  SUM(INV.TpGrandTotal)) AS Amount    FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Reject') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    , ODR.OrderCode  ,
 ODR.CustomerCode +' : '+ODR.CustomerName   , ODR.MIOCode +' : '+ODR.MIOName  ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)

			END
			END


 
 




 

--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetOrderStatusDetailsReportff`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`
- `@Menutype` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT D.OrderCode OrdNo, D.CustomerCode +'' : ''+D.CustomerName Chemist , D.MIOCode +'' : ''+d.MIOName CreatedBy, CONVERT(decimal(18, 2), SUM(INV.TpGrandTotal)) AS Amount FROM dbo.tblInvoice AS INV INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_Webapi_GetOrderStatusDetailsReportff] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null,
@Menutype   nvarchar(max)=null


 

AS
BEGIN
 



		DECLARE @params NVARCHAR(max)='  '


		if(@Menutype='Pending')
		begin
			SET @params=@params+ '  and inv.DeliveryInvoiceStatus is null  ';

			IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,   INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		end


		if(@Menutype='Return')
		begin
			SET @params=@params+ '  and  inv.DeliveryInvoiceStatus=''Reject'' ';

			IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,   INV.UpdateDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		end

	
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		
		DECLARE @companyId NVARCHAR(50)
            SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

			SET @params=@params+ ' AND D.ManufacId = '+ ISNULL(@companyId,0) +' ';

		declare @TRID nvarchar(max)
			  if(@roletype = 'AM')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ ISNULL(@TRID,0) +')';

			END

			 if(@roletype = 'DZSM')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.AreaId in ( '+ ISNULL(@TRID,0) +')';

			END

			 if(@roletype = 'MIO')
			BEGIN
				 

				SET @params=@params+ ' and D.EntryBy='''+convert(nvarchar(max),@mioCode)+''''

			END


DECLARE @Q NVARCHAR(MAX)
	SET @Q='

SELECT  
  
    D.OrderCode OrdNo,
 D.CustomerCode +'' : ''+D.CustomerName Chemist , D.MIOCode +'' : ''+d.MIOName CreatedBy,
   CONVERT(decimal(18, 2),  SUM(INV.TpGrandTotal)) AS Amount  
FROM  dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS D ON INV.OrderId = D.OrderId

   '+@params+'  
	group by D.OrderCode
	,  D.CustomerCode  +'' : ''+ D.CustomerName ,  D.MIOCode,d.MIOName

	order by D.CustomerCode  +'' : ''+ D.CustomerName
		
  '

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetOrderStatusDetailsReportfff`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`
- `@Menutype` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT D.OrderCode OrdNo, D.CustomerCode +'' : ''+D.CustomerName Chemist ,emp.EmpName +'' : ''+emp.EmpMasterCode CreatedBy, CONVERT(decimal(18, 2), (SUM((CASE WHEN INVD.NetAmount > 0 THEN INVD.NetAmount ELSE 0 END)))) AS Amount FROM tblInvoice inv WITH (NOLOCK) left JOIN dbo.tblOrder D WITH (NOLOCK) ON D.OrderId = inv.Orde`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
create PROCEDURE [dbo].[sp_Webapi_GetOrderStatusDetailsReportfff] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null,
@Menutype   nvarchar(max)=null


 

AS
BEGIN
 



		DECLARE @params NVARCHAR(max)='  '


		if(@Menutype='Pending')
		begin
			SET @params=@params+ '  and inv.DeliveryInvoiceStatus is null  ';
		end


		if(@Menutype='Return')
		begin
			SET @params=@params+ '  and  inv.DeliveryInvoiceStatus=''Reject'' ';
		end

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,   INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		
		DECLARE @companyId NVARCHAR(50)
            SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

			SET @params=@params+ ' AND D.ManufacId = '+ ISNULL(@companyId,0) +' ';

		declare @TRID nvarchar(max)
			  if(@roletype = 'AM')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.TerritoryId in ( '+ ISNULL(@TRID,0) +')';

			END

			 if(@roletype = 'DZSM')
			BEGIN
				select @TRID=	STUFF( (SELECT CONCAT(',', mm.[TerritoryAreaId] , '') FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  FOR XML PATH ('') ),1,1,'')

				SET @params=@params+ ' AND D.AreaId in ( '+ ISNULL(@TRID,0) +')';

			END

			 if(@roletype = 'MIO')
			BEGIN
				 

				SET @params=@params+ ' and D.EntryBy='''+convert(nvarchar(max),@mioCode)+''''

			END


DECLARE @Q NVARCHAR(MAX)
	SET @Q='

SELECT  
 

    D.OrderCode OrdNo,
 D.CustomerCode +'' : ''+D.CustomerName Chemist ,emp.EmpName  +'' : ''+emp.EmpMasterCode CreatedBy,
   CONVERT(decimal(18, 2), (SUM((CASE WHEN INVD.NetAmount > 0 THEN INVD.NetAmount ELSE 0 END)))) AS Amount 


FROM
    tblInvoice inv WITH (NOLOCK) 
left JOIN dbo.tblOrder D    WITH (NOLOCK)  ON D.OrderId = inv.OrderId
LEFT JOIN dbo.tblInvoiceDetail AS INVD  WITH (NOLOCK)  ON INV.InvoiceId = INVD.InvoiceId
LEFT JOIN tblUser u WITH (NOLOCK) ON D.EntryBy = u.LoginName
LEFT JOIN tblEmpGeneralInfo emp WITH (NOLOCK) ON emp.EmpInfoId = u.EmpInfoId
WHERE
    D.OrderId IS NOT NULL  and   D.IsFromApp=1


   '+@params+'  
	group by D.OrderCode
	,  D.CustomerCode  +'' : ''+ D.CustomerName,emp.EmpName  +'' : ''+ emp.EmpMasterCode 

	order by D.CustomerCode  +'' : ''+ D.CustomerName
		
  '

EXEC sys.sp_executesql @Q


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetOrderStatusReport`

**Parameters:**
- `@empId` — int, input = `null`
- `@roletype` — nvarchar(max), input = `null`
- `@fromdate` — nvarchar(max), input = `null`
- `@todate` — nvarchar(max), input = `null`

**Tables read (FROM/JOIN):** tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblASMInfo, tblArea, tblCompanyUnit, tblCustMaster, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblMIOInfo, tblOrder, tblTagASMRSMInfo, tblUser, filtered by @params, @empId, @mioCode.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT @InvDone InvDone, @PendingDelivery PendingDelivery, @DeliveryDone DeliveryDone, @InvReturn InvReturn END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetOrderStatusReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
	-- Add the parameters for the stored procedure here
	@empId int = null,
 
 
@roletype  nvarchar(max)=null,
 
@fromdate  nvarchar(max)=null,
@todate   nvarchar(max)=null

 

AS
BEGIN
 

		DECLARE @params NVARCHAR(max)='  '

	IF(@fromdate IS NOT NULL)
		BEGIN
		    SET @params=@params+ ' AND  (convert(Date,  INV.InvoiceDate)) between  '''+CAST(@fromdate AS NVARCHAR(max))+''' and  '''+CAST(@todate AS NVARCHAR(max))+''' '
		END
		 

	  
	  
		DECLARE @mioCode NVARCHAR(50)
		DECLARE @companyId NVARCHAR(50)
        SELECT  @mioCode = usr.LoginName ,  @companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 

		declare @InvDone nvarchar(max)='0' , @PendingDelivery nvarchar(max)='0' , @DeliveryDone nvarchar(max)='0', @InvReturn  nvarchar(max)='0'
		
		  
			  if(@roletype = 'AM')
			BEGIN
			  
 


 select    @InvDone= ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where    ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and emp.CompanyId=ISNULL(@companyId,0)

     

	SELECT  
   @PendingDelivery= ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where inv.DeliveryInvoiceStatus is null and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and emp.CompanyId=ISNULL(@companyId,0)

     

	 

		SELECT  
   @DeliveryDone=  ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Full','Partial') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and emp.CompanyId=ISNULL(@companyId,0)



		 
		SELECT  
   @InvReturn=  ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Reject') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND b.TerritoryId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=3   ) and emp.CompanyId=ISNULL(@companyId,0)

			END

			 if(@roletype = 'DZSM')
			BEGIN
			 

 select    @InvDone= ISNULL(invNumber,0)       from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		       INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
        isnull(ODR.RegionId,0) RegionId  ,    count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where    ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0)    ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1  AND b.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5   ) and emp.CompanyId=ISNULL(@companyId,0)

     

	SELECT  
   @PendingDelivery= ISNULL(invNumber,0)       from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		       INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId

		      left join ( SELECT    
    isnull(ODR.RegionId,0) RegionId  ,     count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where inv.DeliveryInvoiceStatus is null and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0)    ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1  AND b.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5  ) and emp.CompanyId=ISNULL(@companyId,0)

     

	 

		SELECT  
   @DeliveryDone=  ISNULL(invNumber,0)        from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		       INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
    isnull(ODR.RegionId,0) RegionId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Full','Partial') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.RegionId,0)    ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1  AND b.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5   ) and emp.CompanyId=ISNULL(@companyId,0)



		 
		SELECT  
   @InvReturn=  ISNULL(invNumber,0)        from tblASMInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId
		       INNER JOIN dbo.tblArea ar ON ar.AreaId = B.AreaId
		      left join ( SELECT    
    isnull(ODR.RegionId,0) RegionId  ,  count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Reject') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by isnull(ODR.RegionId,0)   ) tblOrd on tblOrd.RegionId=ar.RegionId
		    

		 where   B.IsActive=1  AND b.AreaId IN (SELECT  	  mm.TerritoryAreaId FROM tblTagASMRSMInfo mm (NOLOCK)   WHERE [EmpInfoId] in  (select EmpInfoId from tblEmpGeneralInfo where EmpInfoId=@empId) and  [UserRoleId]=5   ) and emp.CompanyId=ISNULL(@companyId,0)
			 
			  
			END

			 if(@roletype = 'MIO')
			BEGIN
				 

				

 select    @InvDone= ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where    ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)

     

	SELECT  
   @PendingDelivery= ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where inv.DeliveryInvoiceStatus is null and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.InvoiceDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)

     

	 

		SELECT  
   @DeliveryDone=  ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Full','Partial') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)



		 
		SELECT  
   @InvReturn=  ISNULL(invNumber,0)       from tblMIOInfo B
		   INNER JOIN dbo.tblEmpGeneralInfo emp ON emp.EmpInfoId = B.EmployeeId

		      left join ( SELECT    
    isnull(ODR.TerritoryId,0) TerritoryId  ,   count(distinct INV.InvoiceNo) invNumber   FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId

							 where   inv.DeliveryInvoiceStatus in  ('Reject') and   ODR.ManufacId = ISNULL(@companyId,0) and     (convert(Date,  INV.UpdateDate)) between convert(Date, @fromdate) and convert(Date,@todate ) group by  isnull(ODR.TerritoryId,0)    ) tblOrd on tblOrd.TerritoryId=b.TerritoryId
		    

		 where   B.IsActive=1  AND  emp.EmpMasterCode=@mioCode and emp.CompanyId=ISNULL(@companyId,0)

			END


 
 




	SELECT  
    @InvDone InvDone,
@PendingDelivery  PendingDelivery,
@DeliveryDone  DeliveryDone,
@InvReturn   InvReturn


--(D.EntryBy = '+CAST(@mioCode as nvarchar(max))+'  OR
--			D.AreaId='+CAST(@AreaId as nvarchar(max))+' OR 
--			D.RegionId='+CAST(@ZoneId as nvarchar(max))+' OR 
--			D.GroupId='+CAST(@GroupId as nvarchar(max))+')


    END
```

---

## `sp_Webapi_GetProductWiseOrderReport`

**Parameters:**
- `@empId` — int, input = `null`
- `@Role` — nvarchar(max), input = `null`
- `@Month` — int, input = `null`
- `@Year` — int, input = `null`

**Tables read (FROM/JOIN):** tblEmpGeneralInfo, tblOrder, tblOrderDetail, tblProduct, tblTagASMRSMInfo, tblUser
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tblEmpGeneralInfo, tblOrder, tblOrderDetail, tblProduct, tblTagASMRSMInfo, tblUser, filtered by @params, @empId.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select ISNULL(count(D.OrderId),0) order_times, convert(decimal(18,2),iSNULL(SUM(dtl.TotalTradePrice),0)) as ordered_value, SUM(dtl.Quantity) Quantity, dtl.ProductId from tblOrderDetail dtl inner join dbo.tblOrder D with (nolock) on dtl.OrderId=D.OrderId left join tblUser u with (nolock) on D.EntryBy=u.LoginName left join tblEmpGeneralInfo emp`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE PROCEDURE [dbo].[sp_Webapi_GetProductWiseOrderReport] -- sp_Get_Order_Info_WebAPI_NEw_MIo 313
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
		    SET @params=@params+ ' AND MONTH(convert(Date,  D.SubmissionDate))='''+CAST(@Month AS NVARCHAR(max))+''''
		END
		IF(@Year IS NOT NULL )
		BEGIN
		    SET @params=@params+ ' AND   year(convert(Date,  D.SubmissionDate))= '''+CAST(@Year AS NVARCHAR(max))+''''
		END

	  
	  
	  	   declare @companyId NVARCHAR(50)='' 
	  DECLARE @mioCode NVARCHAR(50)

        SELECT  @mioCode = usr.LoginName,@companyId=A.CompanyId
        FROM    dbo.tblEmpGeneralInfo A with (nolock)

		inner join tblUser usr  with (nolock) on a.EmpInfoId=usr.EmpInfoId
        WHERE   A.EmpInfoId = @empId 
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

---

## `sp_WebApi_GetVersion`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** tbl_AppVersion
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tbl_AppVersion.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT TOP 1 * FROM tbl_AppVersion WHERE IsActive =1 ORDER by AppVersionId desc END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE sp_WebApi_GetVersion 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		

		SELECT TOP 1 * FROM tbl_AppVersion WHERE IsActive =1 ORDER by AppVersionId desc


END
```

---

## `sp_WebApi_GetVersion_check`

**Parameters:**
- none (or not parseable from signature)

**Tables read (FROM/JOIN):** tbl_AppVersion
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining tbl_AppVersion.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `SELECT TOP 1 * FROM tbl_AppVersion WHERE IsActive =1 ORDER by AppVersionId desc END`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_WebApi_GetVersion_check] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
		

		SELECT TOP 1 * FROM tbl_AppVersion WHERE IsActive =1 ORDER by AppVersionId desc


END
```

---

## `sp_WebAPI_MonthlyAttendanceHistory`

**Parameters:**
- `@wings_id` — NVARCHAR(max), input
- `@frmDate` — NVARCHAR(max), input
- `@toDate` — NVARCHAR(max), input

**Tables read (FROM/JOIN):** ZASHRIS..tblHolidayInformation, sys..all_objects, tblCompanyInfo, tblEmpGeneralInfo, tblMarketAttendance_Master_webapi
**Tables written:** (none — read-only procedure)

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** Read-only report/lookup query joining ZASHRIS..tblHolidayInformation, sys..all_objects, tblCompanyInfo, tblEmpGeneralInfo, tblMarketAttendance_Master_webapi.

**Result set:** Final `SELECT` in the body (truncated to ~350 chars): `select att1.EmpInfoId, COUNT(att1.PunchInTime) AbsentTo from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where CONVERT(time,att1.PunchInTime ) >= CONVERT(time,'''+@LastTime+''') AND CONVERT(date,att1.AttendanceDate) BETWEEN CONVERT(Date,'''+@frmDate+''') AND CONVERT(Date,'''+@toDate+''') group by att1.EmpInfoId) tblAbset ON`

**Application caller(s):** UNUSED — no application caller found

**Full T-SQL body:**
```sql
CREATE
 PROCEDURE [dbo].[sp_WebAPI_MonthlyAttendanceHistory]
	-- Add the parameters for the stored procedure here

	@wings_id NVARCHAR(max),
	@frmDate  NVARCHAR(max),
	@toDate  NVARCHAR(max)



AS
BEGIN
  
  IF @frmDate IS NOT NULL
    BEGIN
        DECLARE @FromDateString VARCHAR(20) = CONVERT(VARCHAR, @frmDate, 106);
        SET @FromDateString = REPLACE(@FromDateString, 'Sept', 'Sep');
        SET @frmDate = CONVERT(DATETIME, @FromDateString);
    END
	

		IF @ToDate IS NOT NULL
    BEGIN
        DECLARE @ToDateString VARCHAR(20) = CONVERT(VARCHAR, @ToDate, 106);
        SET @ToDateString = REPLACE(@ToDateString, 'Sept', 'Sep');
        SET @ToDate = CONVERT(DATETIME, @ToDateString);
    END

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
SELECT  (select count(*) from ZASHRIS..tblHolidayInformation where  CONVERT(Date, HolidayDate) between    CONVERT(Date,'''+@frmDate+''')   and  CONVERT(Date,'''+@toDate+''')) Holiday,  ( SELECT COUNT(*)
FROM (
    SELECT TOP (DATEDIFF(DAY, CONVERT(Date,'''+@frmDate+''') ,   CONVERT(Date,'''+@toDate+'''))+ 1)
        date = DATEADD(DAY, ROW_NUMBER() OVER (ORDER BY a.object_id) - 1, CONVERT(Date,'''+@frmDate+''') )
    FROM sys.all_objects AS a
    CROSS JOIN sys.all_objects AS b
) AS dates
WHERE DATEPART(WEEKDAY, date) = 6) Weekend,    ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ItemNo,     dtl.EmpMasterCode,dtl.EmpName, com.CompanyName, '''' AreaName, ISNUll(tblPre.PreTo,0) Present, ISNUll(tblLate.LateTo,0) Late, ISNUll(tblAbset.AbsentTo,0) [Absent]
        FROM     dbo.tblEmpGeneralInfo dtl  with (nolock) 

		 
		 left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) PreTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime )  between CONVERT(time,'''+@PS+''') and CONVERT(time,'''+@PE+''')  AND CONVERT(date,att1.AttendanceDate)  BETWEEN CONVERT(Date,'''+@frmDate+''') AND  CONVERT(Date,'''+@toDate+''')    group by att1.EmpInfoId) tblPre     ON dtl.EmpInfoId = tblPre.EmpInfoId 

		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) LateTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime ) between CONVERT(time,'''+@LS+''') and CONVERT(time,'''+@LE+''')  AND CONVERT(date,att1.AttendanceDate)  BETWEEN CONVERT(Date,'''+@frmDate+''') AND  CONVERT(Date,'''+@toDate+''')    group by att1.EmpInfoId) tblLate     ON dtl.EmpInfoId = tblLate.EmpInfoId 

		  
		  left JOIN (select att1.EmpInfoId, COUNT(att1.PunchInTime) AbsentTo  from dbo.tblMarketAttendance_Master_webapi att1 with (nolock) where  CONVERT(time,att1.PunchInTime )  >= CONVERT(time,'''+@LastTime+''') AND CONVERT(date,att1.AttendanceDate)  BETWEEN CONVERT(Date,'''+@frmDate+''') AND  CONVERT(Date,'''+@toDate+''')    group by att1.EmpInfoId) tblAbset     ON dtl.EmpInfoId = tblAbset.EmpInfoId 


		 left JOIN dbo.tblCompanyInfo com  with (nolock) ON com.CompanyId = dtl.CompanyId 
		 

		 where (dtl.EmpMasterCode is not null or dtl.EmpMasterCode <>'''')  and (ISNUll(tblPre.PreTo,0) + ISNUll(tblLate.LateTo,0) + ISNUll(tblAbset.AbsentTo,0)>0)   AND dtl.CompanyId  ='+@wings_id
 
END

EXEC (@Query)
```

---

## `sp_WHStockInPosting`

**Parameters:**
- `@MasterId` — INT, input

**Tables read (FROM/JOIN):** CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster, CSTL_ERP_ACCDB..tblFinancialYear, CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew, tblCompanyInfo, tblWHStockInDetail, tblWHStockInMaster, tblWearhouse
**Tables written:** INSERT → CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail, CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster; UPDATE → tblWHStockInMaster

**Transaction / error handling:** no explicit transaction, no TRY/CATCH, no RAISERROR.

**Business logic:** GL-posting procedure restricted to `CompanyId = 16`: for each qualifying row (cursor-driven), it builds a voucher code and financial-year id, inserts a header row into `CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster` (`VoucherType='Journal'`, `Particular='WHStockJournal'`) and a matching debit/credit line pair into `tblDebitCreditVoucherDetail`, using GL account ids looked up from `tblSalesIntegrationMapingNew` for that `Particular`; it then sets `tblWHStockInMaster.IsPosting` to 1 if a voucher now exists for the record, else 0.

**Result set:** No caller-facing result set — every SELECT in the body is either a scalar `SELECT @var = ...` variable assignment or the source query of a cursor, both consumed internally, not returned to the caller.

**Application caller(s):**
- `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:120` — `return aCommonInternalDal.UpdateAction("sp_WHStockInPosting", aSqlParameterList);`

**Full T-SQL body:**
```sql
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_WHStockInPosting] 

	@MasterId INT
	
AS
BEGIN
	
	DECLARE @WHStockInMasterID INT
	DECLARE @WHStockInCode NVARCHAR(500)
	DECLARE @WHStockInDate DATETIME
	DECLARE @CompanyId INT
	DECLARE @TotalQuantity INT
	DECLARE @TotalValue DECIMAL(18,2)
	DECLARE @ChallanNo NVARCHAR(500)
	DECLARE @ChallanDate DATETIME 
	DECLARE @VoucherMasterId INT  
	 
	--------------------------------------------------------
	DECLARE @MyCursor CURSOR
	SET @MyCursor = CURSOR FAST_FORWARD
	FOR
	---------------

	SELECT DISTINCT WM.WHStockInMasterID,WHStockInCode,WM.WHStockInDate,TotalQuantity,TotalValue,ChallanNo,ChallanDate,WH.CompanyId FROM tblWHStockInMaster AS WM WITH(NOLOCK)
	LEFT JOIN tblWHStockInDetail AS WSD ON WM.WHStockInMasterID = WSD.WHStockInMasterID
	LEFT JOIN tblWearhouse AS WH ON WM.WarehouseId = WH.WearhouseId
	LEFT JOIN tblCompanyInfo AS CI ON WH.CompanyId = CI.CompanyId
	WHERE WM.WHStockInMasterID = @MasterId
	
	----------
	OPEN @MyCursor
	FETCH NEXT FROM @MyCursor
	INTO @WHStockInMasterID,@WHStockInCode,@WHStockInDate,@TotalQuantity,@TotalValue,@ChallanNo,@ChallanDate,@CompanyId
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	

	-- Master Variable Info
    -------------------------------------

	DECLARE @VoucherType NVARCHAR(MAX) ='Journal'
	DECLARE @VoucherCode NVARCHAR(MAX)

	DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@WHStockInDate)), 3, 3)   
	DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@WHStockInDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@WHStockInDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@WHStockInDate)) END)
	DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@WHStockInDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@WHStockInDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@WHStockInDate)) END)
    SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster 
	WHERE @WHStockInDate = @WHStockInDate
	
		IF(@VoucherType='DebitVoucherBank')
		BEGIN
			SET @VoucherCode='DVB-'+@VoucherCode	
		END
		IF(@VoucherType='DebitVoucherCash')
		BEGIN
			SET @VoucherCode='DVC-'+@VoucherCode
		END
		IF(@VoucherType='CreditVoucherBank')
		BEGIN
			SET @VoucherCode='CVB-'+@VoucherCode	
		END
		IF(@VoucherType='CreditVoucherCash')
		BEGIN
			SET @VoucherCode='CVC-'+@VoucherCode
		END
		IF(@VoucherType='FundTransfer')
		BEGIN
			SET @VoucherCode='FUT-'+@VoucherCode
		END

		IF(@VoucherType='Journal')
		BEGIN
			SET @VoucherCode='JVP-'+@VoucherCode
		END

	DECLARE @FinYearId NVARCHAR(MAX) = ''

	SELECT @FinYearId = FinancialYearId FROM CSTL_ERP_ACCDB..tblFinancialYear 
	WHERE CompanyId = @CompanyId AND Status IN ('Active') AND @WHStockInDate BETWEEN StartDate AND EndDate

	----------------------------------


	-- Master Information Posting

	INSERT	INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster
	(
	    VoucherCode,
	    CompanyInfoId,
	    FinancialYearId,
	    PostingDate,
	    VoucherCategoryID,
	    CurrencyId,
	    CurrencyCovRate,
	    Narration,
	    TotalAmount,
	    TotalAmountBDT,
	    EntryBy,
	    EntryDate,
	    VoucherType, 
	    JournalType,
	    WHStockInMasterId,
		IsCostCenter,
		IsInvoice,
		isManualVCRNo,
		IsExportInvoice,
		IsImportInvoice

	)
	VALUES
	(   
	    @VoucherCode,       -- VoucherCode - nvarchar(max)
	    @CompanyId,         -- CompanyInfoId - int
	    @FinYearId,         -- FinancialYearId - int
	    CONVERT(NVARCHAR(50), GETDATE(), 101), -- PostingDate - datetime
	    2,         -- VoucherCategoryID - int
	    1,         -- CurrencyId - int
	    1.00,      -- CurrencyCovRate - decimal(18, 2)
	    'Warehouse Stock In - ( Stock-in Code:' + @WHStockInCode + ' , Stock-in Approve Date:' + CONVERT(NVARCHAR(50), @WHStockInDate, 101) + ', Stock Value:' + CONVERT(NVARCHAR(50), @TotalValue) + ')',       -- Narration - nvarchar(max)
	    0.00,      -- TotalAmount - decimal(18, 2)
	    @TotalValue,      -- TotalAmountBDT - decimal(18, 2)
	    'Auto Process',       -- EntryBy - nvarchar(max)
	    CURRENT_TIMESTAMP, -- EntryDate - datetime	  
	    'Journal',       -- VoucherType - nvarchar(50)
	    'Adjustable',       -- JournalType - nvarchar(50)
	    @WHStockInMasterID,      -- ProformaId -int
		0,0,0,0,0
	)


	-- Detail Information Posting

	SET @VoucherMasterId = SCOPE_IDENTITY()

	DECLARE	@DrGl INT 
	SELECT @DrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('WHStockJournal') AND BalanceType IN ('Dr')

	-- Credit

	INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
	(
	    VoucherMasterId,
	    AccountId,
	    DebitorCredit,
	    DebitAmount,
	    CreditAmount,
	    DebitAmountBDT,
	    CreditAmountBDT
	)
	VALUES
	(   @VoucherMasterId,    -- VoucherMasterId - int
	    @DrGl,    -- AccountId - int
	    'Debit',  -- DebitorCredit - nvarchar(50)
	    0.00, -- DebitAmount - decimal(18, 2)
	    0.00, -- CreditAmount - decimal(18, 2)
	    @TotalValue, -- DebitAmountBDT - decimal(18, 2)
	    0.00 -- CreditAmountBDT - decimal(18, 2)

	)

	--- Debit

	DECLARE	@CrGl INT 
	SELECT @CrGl = GlId FROM CSTL_ERP_ACCDB..tblSalesIntegrationMapingNew WHERE Particular IN ('WHStockJournal') AND BalanceType IN ('Cr')


	
	INSERT INTO CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail
	(
	    VoucherMasterId,
	    AccountId,
	    DebitorCredit,
	    DebitAmount,
	    CreditAmount,
	    DebitAmountBDT,
	    CreditAmountBDT
	)
	VALUES
	(   @VoucherMasterId,    -- VoucherMasterId - int
	    @CrGl,    -- AccountId - int
	    'Credit',  -- DebitorCredit - nvarchar(50)
	    0.00, -- DebitAmount - decimal(18, 2)
	    0.00, -- CreditAmount - decimal(18, 2)
	    0.00, -- DebitAmountBDT - decimal(18, 2)
	    @TotalValue -- CreditAmountBDT - decimal(18, 2)
	)


	DECLARE @Count INT = 0 

	SELECT @Count = COUNT(WHStockInMasterId) FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster WHERE WHStockInMasterId = @MasterId

	IF (@Count > 0)
		BEGIN
		   
		   UPDATE tblWHStockInMaster SET IsPosting = 1 WHERE WHStockInMasterId = @MasterId

		END
	ELSE
		BEGIN

		UPDATE tblWHStockInMaster SET IsPosting = 0 WHERE WHStockInMasterId = @MasterId

		END
	
	
	FETCH NEXT FROM @MyCursor
	INTO @WHStockInMasterID,@WHStockInCode,@WHStockInDate,@TotalQuantity,@TotalValue,@ChallanNo,@ChallanDate,@CompanyId
	
	END
	CLOSE @MyCursor
	DEALLOCATE @MyCursor

	
END
```

---
