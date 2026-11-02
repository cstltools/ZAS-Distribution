-- Fix: sp_Sales_DeliverySalesReturnPosting silently posted nothing when tblDCStore.TempReceiveId is NULL
-- (INNER JOIN tblCentralStore dropped every cursor row). Run on SalesrollDB_ZAS.
USE SalesrollDB_ZAS;
GO
-- =============================================
-- Author: <Author,JEWEL>
-- Create date: <Create Date,01/15/2016,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_Sales_DeliverySalesReturnPosting] 

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
		-- ponytail: was INNER JOIN; CTRS is never selected and new DC stock has NULL TempReceiveId, which silently blocked posting
		LEFT JOIN tblCentralStore AS CTRS ON DCS.TempReceiveId = CTRS.ReceiveId
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
GO

