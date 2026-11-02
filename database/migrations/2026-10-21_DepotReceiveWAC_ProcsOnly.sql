-- Procs only (no data backfill). Same two ALTERs as 2026-10-21_DepotReceiveWAC_ReEnable_And_Backfill.sql
USE SalesRollDB_ZAS;
GO
SET ANSI_NULLS ON; SET QUOTED_IDENTIFIER ON;
GO
-- 1. sp_UD_ReceiveIdInDc ------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_UD_ReceiveIdInDc]
    @DCStoreId INT = NULL   -- NULL = every row still unlinked (what the app passes)
AS
BEGIN
    SET NOCOUNT ON;

    -- CLN (central -> depot): tblStockInTransfar -> tblCentralStore
    UPDATE DCS
    SET DCS.ReceiveId = ST.ReceiveId,
        DCS.TempReceiveId = ST.ReceiveId,
        DCS.UnitPrice_CostPrice = ISNULL(CS.Avg_Rate, ISNULL(CS.UnitPrice, DCS.UnitPrice_CostPrice))
    FROM dbo.tblDCStore DCS
    INNER JOIN dbo.tblStockInTransfar ST WITH (NOLOCK) ON DCS.StockInTransfarId = ST.StockInTransfarId
    INNER JOIN dbo.tblCentralStore CS WITH (NOLOCK) ON ST.ReceiveId = CS.ReceiveId
    WHERE (@DCStoreId IS NULL AND (DCS.TempReceiveId IS NULL OR DCS.ReceiveId IS NULL OR ISNULL(DCS.UnitPrice_CostPrice,0) = 0))
       OR DCS.DCStoreId = @DCStoreId;

    -- IDT (depot -> depot): tblChalanDetail.DCStoreId -> source depot row; inherit its link + cost.
    -- ponytail: loop because a transfer can be sourced from another transfer; cap 10 hops.
    DECLARE @n INT = 1, @hop INT = 0;
    WHILE @n > 0 AND @hop < 10
    BEGIN
        UPDATE DCS
        SET DCS.ReceiveId = SRC.ReceiveId,
            DCS.TempReceiveId = SRC.TempReceiveId,
            DCS.UnitPrice_CostPrice = ISNULL(NULLIF(SRC.UnitPrice_CostPrice,0), DCS.UnitPrice_CostPrice)
        FROM dbo.tblDCStore DCS
        INNER JOIN dbo.tblChalanDetail CD WITH (NOLOCK) ON DCS.ChalanDetailsId = CD.ChalanDetailsId
        INNER JOIN dbo.tblDCStore SRC WITH (NOLOCK) ON CD.DCStoreId = SRC.DCStoreId
        WHERE DCS.StockInTransfarId IS NULL
          AND ((@DCStoreId IS NULL AND (DCS.TempReceiveId IS NULL OR DCS.ReceiveId IS NULL OR ISNULL(DCS.UnitPrice_CostPrice,0) = 0))
               OR DCS.DCStoreId = @DCStoreId)
          AND (   ISNULL(DCS.ReceiveId,-1)     <> ISNULL(SRC.ReceiveId,-1)
               OR ISNULL(DCS.TempReceiveId,-1) <> ISNULL(SRC.TempReceiveId,-1)
               OR ISNULL(DCS.UnitPrice_CostPrice,0) <> ISNULL(NULLIF(SRC.UnitPrice_CostPrice,0), ISNULL(DCS.UnitPrice_CostPrice,0)));
        SELECT @n = @@ROWCOUNT, @hop += 1;
    END
END;
GO

-- 2. sp_Process_WeightedAverage (live body, two-line change) -------------------
-- =============================================
-- Author:		<Author,JEWEL>
-- Create date: <Create Date,01-04-2019,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_Process_WeightedAverage] 

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
	                               ORDER BY Weighted_Avg_Id DESC), 0)
	
	    SET @Opening_Amount = ISNULL((SELECT TOP 1 Close_Amt 
	                                  FROM tbl_INV_WeightedAvg
	                                  WHERE Company_Id = @TransCompanyId AND Item_Id = @TransItemDescriptionId
	                                  ORDER BY Weighted_Avg_Id DESC), 0)
	
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
			 @EntryDate,@DeliveryLocationId1,@CursorDCStoreId,@ChalanNo,@TranscationTypeId,@InventoryTypeId,@InventoryType,@BatchNo,@MMFGDate,@EXPDate, @ApproveDate

	END
	
	-- Close and deallocate the cursor

	CLOSE @TransCursor
	DEALLOCATE @TransCursor  

END
GO

