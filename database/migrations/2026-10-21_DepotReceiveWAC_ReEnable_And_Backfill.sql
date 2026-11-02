/*
============================================================================
 2026-10-21  Depot receive weighted-average: re-enable + backfill
----------------------------------------------------------------------------
 Problem (found 2026-10-21):
   - Since 2026-08-09 sales deductions (sp_Process_WeightedAverageSalesConfirmation)
     post to SalesRollDB_ZAS..tbl_INV_WeightedAvg, but depot receipts never did
     (ProcessWeightedAverage call was commented out in RequisitionBLL).
     Stock went negative -> Avg_Rate 0 -> every later issue costed at 0.
   - sp_UD_ReceiveIdInDc requires @DCStoreId since 2026-08-09, but the app calls
     it with no parameter -> fails silently -> tblDCStore.TempReceiveId /
     ReceiveId / UnitPrice_CostPrice stay NULL, so sp_Process_WeightedAverage
     (which joins tblCentralStore on TempReceiveId) would find nothing anyway.
   - IDT (depot-to-depot) rows have no StockInTransfarId, so the proc never
     linked them at all: ReceiveId / UnitPrice_CostPrice NULL on every IDT row.
   - sp_Process_WeightedAverage has a stray SELECT * over the whole ledger inside
     its loop, and picks the opening row by Trans_Date with no tiebreaker.

 This script:
   1. sp_UD_ReceiveIdInDc: @DCStoreId optional; also sets TempReceiveId;
      IDT rows inherit link + cost from their source depot row (tblChalanDetail.DCStoreId).
   2. sp_Process_WeightedAverage: drop stray SELECT, order opening row by id.
   3. Link + cost all CLN receipts since 2026-08-09, post the missing WA rows.
   4. Recompute the ledger chain for every row from the cutover onward, in id
      order, carrying the last positive average through negative-stock gaps.
   5. Sanity checks.

 Everything (proc ALTERs included) runs in ONE transaction. Plain T-SQL, runs
 as-is in SSMS (no SQLCMD mode) or sqlcmd -i. Set the two values in #cfg below:
   DryRun 1 = rollback at the end (default), 0 = commit
   FromDate 2026-08-09 = cutover only; 2026-02-16 also posts the 1211 unlinked
   Feb-Jul receipts (recommended, closes more negative items)
 Dry-run results on TOWSIF 2026-10-21 (From=08-09 / From=02-16):
   receipts posted 948 / 2159, negative items 129 / 73, zero-rate sales rows 96 / 96,
   qty chain breaks 0 / 0, amt breaks 6 / 6 (the 6 items negative before cutover,
   stale value intentionally dropped).
 Backup of touched rows: tbl_INV_WeightedAvg_bak_20261021.
============================================================================
*/
USE SalesRollDB_ZAS;
GO
IF OBJECT_ID('tempdb..#cfg') IS NOT NULL DROP TABLE #cfg;
CREATE TABLE #cfg (DryRun BIT, FromDate DATE);
INSERT #cfg VALUES (1, '2026-08-09');   -- <<< EDIT HERE: DryRun 0 to commit, FromDate 2026-02-16 for the wider backfill
GO
SET XACT_ABORT ON;
BEGIN TRAN;
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

-- 3 + 4 + 5. Backfill and chain recompute (same transaction as the ALTERs) ---------
SET NOCOUNT ON;
DECLARE @CutoverId BIGINT = (SELECT MIN(Weighted_Avg_Id) FROM dbo.tbl_INV_WeightedAvg WHERE Ref_No LIKE 'DEL-%'); -- first row of the new sales posting, 2026-08-09
DECLARE @From DATE = (SELECT FromDate FROM #cfg);

PRINT '=== BEFORE ===';
;WITH l AS (SELECT *, ROW_NUMBER() OVER (PARTITION BY Company_Id, Item_Id ORDER BY Weighted_Avg_Id DESC) rn FROM dbo.tbl_INV_WeightedAvg)
SELECT COUNT(*) items, SUM(CASE WHEN Close_Qty<0 THEN 1 ELSE 0 END) negQty, SUM(CASE WHEN Close_Qty>0 AND ISNULL(Avg_Rate,0)=0 THEN 1 ELSE 0 END) posQtyZeroRate, SUM(Close_Amt) ledgerValue FROM l WHERE rn=1;
SELECT COUNT(*) salesRowsZeroRate FROM dbo.tbl_INV_WeightedAvg WHERE Weighted_Avg_Id >= @CutoverId AND Trans_Type_Id=4 AND ISNULL(Curr_Rate,0)=0;

-- backup of every row the recompute may touch
IF OBJECT_ID('dbo.tbl_INV_WeightedAvg_bak_20261021') IS NULL
    SELECT * INTO dbo.tbl_INV_WeightedAvg_bak_20261021 FROM dbo.tbl_INV_WeightedAvg WHERE Weighted_Avg_Id >= @CutoverId;

-- 3a. link + cost the receipts
EXEC dbo.sp_UD_ReceiveIdInDc @DCStoreId = NULL;

-- 3b. post the missing receipt rows (proc has its own duplicate guard)
DECLARE @Id INT, @Posted INT = 0;
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
    SELECT d.DCStoreId FROM dbo.tblDCStore d
    WHERE d.ChalanNo LIKE 'CLN%' AND d.ReceiveDate >= @From AND d.TempReceiveId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.tbl_INV_WeightedAvg w WHERE w.Trans_Type_Id=1 AND w.Trans_Id=d.DCStoreId)
    ORDER BY d.DCStoreId;
OPEN cur; FETCH NEXT FROM cur INTO @Id;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC dbo.sp_Process_WeightedAverage @DCStoreId = @Id;
    SET @Posted += 1;
    FETCH NEXT FROM cur INTO @Id;
END
CLOSE cur; DEALLOCATE cur;
PRINT 'receipt rows posted: ' + CAST(@Posted AS VARCHAR);

-- 4. recompute the chain from the cutover, per company+item, in id order.
--    Receipts keep their own rate; issues take the running average; when stock is
--    <= 0 the last positive average is carried instead of 0.
-- ponytail: id order, not Actual_Date order. Backfilled receipts land after the
--   sales they fed, so those sales are costed at the pre-cutover average. A true
--   chronological rebuild needs re-sequencing ids (BatchWiseStock FKs) - not worth it.
DECLARE @Co INT, @Item INT, @WId BIGINT, @CQty DECIMAL(18,3), @CRate DECIMAL(18,4);
DECLARE @PrevCo INT = -1, @PrevItem INT = -1;
DECLARE @OpQty DECIMAL(18,3), @OpAmt DECIMAL(18,4), @OpRate DECIMAL(18,4), @LastAvg DECIMAL(18,4);
DECLARE @Rate DECIMAL(18,4), @CAmt DECIMAL(18,4), @ClQty DECIMAL(18,3), @ClAmt DECIMAL(18,4), @Avg DECIMAL(18,4);

DECLARE rc CURSOR LOCAL FAST_FORWARD FOR
    SELECT Company_Id, Item_Id, Weighted_Avg_Id, ISNULL(Curr_Qty,0), Curr_Rate
    FROM dbo.tbl_INV_WeightedAvg WHERE Weighted_Avg_Id >= @CutoverId
    ORDER BY Company_Id, Item_Id, Weighted_Avg_Id;
OPEN rc; FETCH NEXT FROM rc INTO @Co, @Item, @WId, @CQty, @CRate;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @Co <> @PrevCo OR @Item <> @PrevItem
    BEGIN   -- seed from the last pre-cutover row of this item
        SELECT @OpQty = 0, @OpAmt = 0, @LastAvg = 0;
        SELECT TOP 1 @OpQty = ISNULL(Close_Qty,0), @OpAmt = ISNULL(Close_Amt,0),
               @LastAvg = CASE WHEN ISNULL(Avg_Rate,0) > 0 THEN Avg_Rate WHEN ISNULL(Close_Qty,0) > 0 THEN ISNULL(Close_Amt,0)/Close_Qty ELSE ISNULL(OP_Rate,0) END
        FROM dbo.tbl_INV_WeightedAvg WHERE Company_Id=@Co AND Item_Id=@Item AND Weighted_Avg_Id < @CutoverId ORDER BY Weighted_Avg_Id DESC;
        IF @OpQty <= 0 SET @OpAmt = 0;   -- a negative pre-cutover state carries no value
        SELECT @PrevCo = @Co, @PrevItem = @Item;
    END

    SET @OpRate = CASE WHEN @OpQty > 0 AND @OpAmt > 0 THEN @OpAmt / @OpQty ELSE @LastAvg END;
    SET @Rate   = CASE WHEN @CQty > 0 THEN ISNULL(NULLIF(@CRate,0), @OpRate) ELSE @OpRate END;
    SET @CAmt   = @CQty * @Rate;
    SET @ClQty  = @OpQty + @CQty;
    SET @ClAmt  = CASE WHEN @ClQty > 0 THEN @OpAmt + @CAmt ELSE 0 END;
    SET @Avg    = CASE WHEN @ClQty > 0 AND @ClAmt > 0 THEN @ClAmt / @ClQty ELSE @OpRate END;

    UPDATE dbo.tbl_INV_WeightedAvg
    SET OP_Qty=@OpQty, OP_Rate=@OpRate, OP_Amt=@OpAmt, Curr_Rate=@Rate, Curr_Amt=@CAmt,
        Close_Qty=@ClQty, Avg_Rate=@Avg, Close_Amt=@ClAmt
    WHERE Weighted_Avg_Id = @WId;

    SELECT @OpQty = @ClQty, @OpAmt = @ClAmt, @LastAvg = CASE WHEN @Avg > 0 THEN @Avg ELSE @LastAvg END;
    FETCH NEXT FROM rc INTO @Co, @Item, @WId, @CQty, @CRate;
END
CLOSE rc; DEALLOCATE rc;

PRINT '=== AFTER ===';
;WITH l AS (SELECT *, ROW_NUMBER() OVER (PARTITION BY Company_Id, Item_Id ORDER BY Weighted_Avg_Id DESC) rn FROM dbo.tbl_INV_WeightedAvg)
SELECT COUNT(*) items, SUM(CASE WHEN Close_Qty<0 THEN 1 ELSE 0 END) negQty, SUM(CASE WHEN Close_Qty>0 AND ISNULL(Avg_Rate,0)=0 THEN 1 ELSE 0 END) posQtyZeroRate, SUM(Close_Amt) ledgerValue FROM l WHERE rn=1;
SELECT COUNT(*) salesRowsZeroRate FROM dbo.tbl_INV_WeightedAvg WHERE Weighted_Avg_Id >= @CutoverId AND Trans_Type_Id=4 AND ISNULL(Curr_Rate,0)=0;
-- 5. chain check: must be 0 / 0
;WITH x AS (SELECT Weighted_Avg_Id, OP_Qty, OP_Amt,
    LAG(Close_Qty) OVER (PARTITION BY Company_Id, Item_Id ORDER BY Weighted_Avg_Id) pc,
    LAG(Close_Amt) OVER (PARTITION BY Company_Id, Item_Id ORDER BY Weighted_Avg_Id) pa FROM dbo.tbl_INV_WeightedAvg)
SELECT SUM(CASE WHEN ABS(OP_Qty-pc)>0.001 THEN 1 ELSE 0 END) qtyBreaks, SUM(CASE WHEN ABS(ISNULL(OP_Amt,0)-ISNULL(pa,0))>1 THEN 1 ELSE 0 END) amtBreaks FROM x WHERE Weighted_Avg_Id >= @CutoverId AND pc IS NOT NULL;
SELECT COUNT(*) clnReceiptsStillMissing FROM dbo.tblDCStore d WHERE d.ChalanNo LIKE 'CLN%' AND d.ReceiveDate >= @From AND NOT EXISTS (SELECT 1 FROM dbo.tbl_INV_WeightedAvg w WHERE w.Trans_Type_Id=1 AND w.Trans_Id=d.DCStoreId);
GO

IF (SELECT DryRun FROM #cfg) = 1 BEGIN ROLLBACK TRAN; PRINT 'DRY RUN - rolled back (procs + data untouched)'; END
ELSE BEGIN COMMIT TRAN; PRINT 'COMMITTED'; END
DROP TABLE #cfg;
GO
