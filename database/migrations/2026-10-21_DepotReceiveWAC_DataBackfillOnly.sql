/*
 Data backfill ONLY. Run AFTER 2026-10-21_DepotReceiveWAC_ProcsOnly.sql
 (needs the fixed sp_UD_ReceiveIdInDc + sp_Process_WeightedAverage).
 Plain T-SQL, runs in SSMS as-is. Idempotent (safe to re-run).

   #cfg DryRun   : 1 = rollback at the end (run this first), 0 = commit
   #cfg FromDate : 2026-02-16 recommended (ledger start, posts all unlinked
                   CLN receipts); 2026-08-09 = cutover only

 What it does, in one transaction:
   1. backup rows >= cutover into tbl_INV_WeightedAvg_bak_20261021
   2. link + cost CLN receipts (sp_UD_ReceiveIdInDc NULL)
   3. post missing depot-receive WA rows (sp_Process_WeightedAverage per DCStoreId)
   4. recompute OP/Curr/Close/Avg chain for every row >= cutover (id order,
      receipts keep own rate, issues take running avg, last positive avg
      carried through negative stock)
   5. checks: qtyBreaks must be 0, clnReceiptsStillMissing must be 0
*/
USE SalesRollDB_ZAS;
GO
IF OBJECT_ID('tempdb..#cfg') IS NOT NULL DROP TABLE #cfg;
CREATE TABLE #cfg (DryRun BIT, FromDate DATE);
INSERT #cfg VALUES (1, '2026-02-16');   -- <<< EDIT HERE: DryRun 0 to commit
GO
SET XACT_ABORT ON;
BEGIN TRAN;
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
