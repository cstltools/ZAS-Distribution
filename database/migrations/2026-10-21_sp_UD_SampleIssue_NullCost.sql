/*
 sp_UD_SampleIssue wrote Close_Amt = NULL whenever CostPerUnit was NULL
 (CRS join needs tblCentralStore via TempReceiveId; unlinked depot rows -> NULL,
 CS.UnitPrice also NULL). The next row for that item reads ISNULL(Close_Amt,0)
 and the whole item value vanishes (34 rows, ~276.6M).

 Fix: an issue is costed at the running weighted average (@Opening_Rate), which
 the proc already computes from the ledger; the DCStore-derived cost is only a
 fallback, and NULL is never written.

 Data: the broken rows sit after the 2026-08-09 cutover, so the chain recompute
 in 2026-10-21_DepotReceiveWAC_DataBackfillOnly.sql (step 4: issues take running
 avg) repairs them. Run that after this ALTER. Verify afterwards:
   SELECT COUNT(*) FROM tbl_INV_WeightedAvg WHERE Close_Amt IS NULL OR Curr_Rate IS NULL;  -- must be 0
*/
USE SalesRollDB_ZAS;
GO
SET ANSI_NULLS ON; SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_UD_SampleIssue] 
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

									-- Issue at running WAC; DCStore cost only if the ledger has no rate. Never NULL (NULL here zeroed the item's value on the next row).
									SET @CostPerUnit = COALESCE(NULLIF(@Opening_Rate, 0), NULLIF(@CostPerUnit, 0), 0)
					
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
GO
