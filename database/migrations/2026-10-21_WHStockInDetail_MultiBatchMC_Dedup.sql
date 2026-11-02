/*
 tblWHStockInDetail: multi-batch Material Conversion receives carried the WHOLE
 MC cost on EVERY batch row (MaterialConversionReceive grid bound
 CM.TotalCostOfThisBatch per row). Approval posted ONE tblCentralStore row per MC
 holding the full qty + full amount under the first batch's MigoDetailID; the
 other batch rows never reached tblCentralStore. Stock is therefore correct,
 but tblWHStockInDetail / tblWHStockInMaster.TotalValue over-state by the
 repeated amounts, and WH stock-in reports / reconciliation disagree with CS.

 Code: already fixed in the 2026-09-01 sync (query now computes
 Trans_Qty * CostPerUnit per batch); rows since then are clean. No GL voucher
 exists for any affected master (sp_WHStockInPosting is CompanyId 16 only).

 Repair (data only, matches CS which is the stock truth):
   - delete each orphan batch row (no CS row) whose sibling row in the same
     master/MC already holds the full MC amount and whose CS row already holds
     the full MC qty;
   - set the surviving row's Qty to the CS StockInQty;
   - recompute master TotalQuantity / TotalValue from the surviving rows.
 Deleted rows are copied to tblWHStockInDetail_MCDup_Backup_20261021 first.

 Local copy 2026-10-20: 8 rows / 7 MCs / 6 masters, 105,038,859.94 removed.
 Run with @DryRun = 1 first; it prints the plan and rolls back.
*/
USE SalesRollDB_ZAS;
GO
SET NOCOUNT ON; SET XACT_ABORT ON;

DECLARE @DryRun BIT = 1;   -- set 0 to apply

BEGIN TRAN;

-- 1. Orphan rows (no CS row) and the sibling that CS actually posted.
IF OBJECT_ID('tempdb..#fix') IS NOT NULL DROP TABLE #fix;
SELECT D.WHStockInDetailID AS DupId, K.WHStockInDetailID AS KeepId, D.WHStockInMasterID, D.MaterialConversionMasterId,
       D.Qty AS DupQty, D.TotalAmount AS DupAmt, K.Qty AS KeepQty, CS.StockInQty AS CsQty, CS.TotalAmount AS CsAmt
INTO #fix
FROM tblWHStockInDetail D
JOIN tblWHStockInMaster M ON M.WHStockInMasterID = D.WHStockInMasterID
JOIN tblWHStockInDetail K ON K.WHStockInMasterID = D.WHStockInMasterID
                          AND K.MaterialConversionMasterId = D.MaterialConversionMasterId
                          AND K.ProductId = D.ProductId
                          AND K.WHStockInDetailID = (SELECT MIN(WHStockInDetailID) FROM tblWHStockInDetail
                                                     WHERE WHStockInMasterID = D.WHStockInMasterID AND MaterialConversionMasterId = D.MaterialConversionMasterId)
JOIN tblCentralStore CS ON CS.MigoDetailID = K.WHStockInDetailID
WHERE ISNULL(D.MaterialConversionMasterId, 0) <> 0
  AND D.WHStockInDetailID <> K.WHStockInDetailID
  AND M.Status = 'Approved'
  AND NOT EXISTS (SELECT 1 FROM tblCentralStore X WHERE X.MigoDetailID = D.WHStockInDetailID)
  AND ABS(D.TotalAmount - K.TotalAmount) < 1          -- same MC total copied onto each batch row
  AND ABS(CS.TotalAmount - K.TotalAmount) < 1         -- CS already carries the full MC amount
  AND CS.StockInQty = (SELECT SUM(Qty) FROM tblWHStockInDetail
                       WHERE WHStockInMasterID = D.WHStockInMasterID AND MaterialConversionMasterId = D.MaterialConversionMasterId); -- and the full MC qty

SELECT * FROM #fix ORDER BY WHStockInMasterID, DupId;
SELECT COUNT(*) AS rowsToDelete, COUNT(DISTINCT MaterialConversionMasterId) AS mcs, COUNT(DISTINCT WHStockInMasterID) AS masters, SUM(DupAmt) AS amountRemoved FROM #fix;

-- 2. Backup, delete, fold qty into the kept row.
IF OBJECT_ID('dbo.tblWHStockInDetail_MCDup_Backup_20261021') IS NULL
    SELECT ISNULL(D.WHStockInDetailID, 0) AS WHStockInDetailID, D.WHStockInMasterID, D.ProductId, D.Batch, D.ExpDate, D.MfgDate, D.Qty, D.Price, D.VAT, D.TotalAmount, D.MaterialConversionMasterId
    INTO dbo.tblWHStockInDetail_MCDup_Backup_20261021 FROM tblWHStockInDetail D WHERE 1 = 0;   -- ISNULL() strips IDENTITY
INSERT INTO dbo.tblWHStockInDetail_MCDup_Backup_20261021 (WHStockInDetailID, WHStockInMasterID, ProductId, Batch, ExpDate, MfgDate, Qty, Price, VAT, TotalAmount, MaterialConversionMasterId)
SELECT D.WHStockInDetailID, D.WHStockInMasterID, D.ProductId, D.Batch, D.ExpDate, D.MfgDate, D.Qty, D.Price, D.VAT, D.TotalAmount, D.MaterialConversionMasterId
FROM tblWHStockInDetail D JOIN #fix F ON F.DupId = D.WHStockInDetailID
WHERE NOT EXISTS (SELECT 1 FROM dbo.tblWHStockInDetail_MCDup_Backup_20261021 B WHERE B.WHStockInDetailID = D.WHStockInDetailID);

DELETE D FROM tblWHStockInDetail D JOIN #fix F ON F.DupId = D.WHStockInDetailID;

UPDATE K SET Qty = F.CsQty
FROM tblWHStockInDetail K JOIN (SELECT DISTINCT KeepId, CsQty FROM #fix) F ON F.KeepId = K.WHStockInDetailID;

-- 3. Master totals from surviving rows.
UPDATE M SET TotalQuantity = S.q, TotalValue = S.a, TotalVat = S.v
FROM tblWHStockInMaster M
JOIN (SELECT WHStockInMasterID, SUM(Qty) q, SUM(TotalAmount) a, SUM(Qty * VAT) v FROM tblWHStockInDetail
      WHERE WHStockInMasterID IN (SELECT WHStockInMasterID FROM #fix) GROUP BY WHStockInMasterID) S ON S.WHStockInMasterID = M.WHStockInMasterID;

-- 4. Verify: every touched master now equals its CS rows.
SELECT M.WHStockInMasterID, M.TotalQuantity, M.TotalValue, C.q AS csQty, C.a AS csAmt
FROM tblWHStockInMaster M
JOIN (SELECT D.WHStockInMasterID, SUM(CS.StockInQty) q, SUM(CS.TotalAmount) a FROM tblCentralStore CS
      JOIN tblWHStockInDetail D ON D.WHStockInDetailID = CS.MigoDetailID GROUP BY D.WHStockInMasterID) C ON C.WHStockInMasterID = M.WHStockInMasterID
WHERE M.WHStockInMasterID IN (SELECT WHStockInMasterID FROM #fix);

IF EXISTS (SELECT 1 FROM tblWHStockInMaster M
           JOIN (SELECT D.WHStockInMasterID, SUM(CS.StockInQty) q, SUM(CS.TotalAmount) a FROM tblCentralStore CS
                 JOIN tblWHStockInDetail D ON D.WHStockInDetailID = CS.MigoDetailID GROUP BY D.WHStockInMasterID) C ON C.WHStockInMasterID = M.WHStockInMasterID
           WHERE M.WHStockInMasterID IN (SELECT WHStockInMasterID FROM #fix) AND (M.TotalQuantity <> C.q OR ABS(M.TotalValue - C.a) > 1))
BEGIN
    ROLLBACK; RAISERROR('Verification failed: master != central store after repair. Rolled back.', 16, 1); RETURN;
END

IF @DryRun = 1 BEGIN ROLLBACK; PRINT 'DRY RUN - rolled back.'; END
ELSE           BEGIN COMMIT;   PRINT 'Applied.'; END
GO
