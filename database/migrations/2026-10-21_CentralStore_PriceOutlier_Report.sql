/*
 tblCentralStore receipt price outliers (READ-ONLY report).

 Material Conversion receipts post UnitPrice = CM.CostPerUnit from the ERP
 conversion master; a mistyped cost (e.g. product 415002: 27.50 vs running
 average 4,135) is approved unchecked and sp_Acc_MaterialConversionReceive folds
 it into CS Avg_Rate, which then feeds tblDCStore.UnitPrice_CostPrice and the
 depot ledger (tbl_INV_WeightedAvg) on every later transfer.

 Guard (code, same date): WarehouseStockInApproval now refuses to approve a
 master whose detail Price is > 2x or < 1/2 of the product's last CS rate
 unless the approver ticks "Override price check".

 This script only lists the historical rows so they can be corrected by hand:
 the true cost lives in CSTL_ERPDB..tbl_MC_MaterialConversionMaster, and a
 repair must then re-run the CS chain, the depot cost/ledger and the JV
 (DR 4156 / CR 5882) for each master - not something to guess in bulk.

 Reference rate = latest earlier CS row of the same product with UnitPrice > 1
 (skips the 1.00 / 0.50 / 0.01 opening placeholders).
 Local copy 2026-10-21: 2026 alone has 138 of 1,090 receipts outside 2x.
*/
USE SalesRollDB_ZAS;
GO
SET NOCOUNT ON;
DECLARE @Factor DECIMAL(9,2) = 2.0;        -- same threshold as the approval guard
DECLARE @From   DATE = '2026-01-01';

;WITH c AS (
    SELECT CS.ReceiveId, CS.ReceiveDate, CS.ProductId, CS.ProductCode, CS.StockInQty, CS.UnitPrice, CS.TotalAmount,
           CS.Avg_Rate, CS.MigoDetailID, CS.Mst_Trans_Id AS WHStockInMasterID, CS.MaterialConversionMasterId, R.RefRate
    FROM dbo.tblCentralStore CS
    OUTER APPLY (SELECT TOP 1 ISNULL(X.Avg_Rate, X.UnitPrice) RefRate
                 FROM dbo.tblCentralStore X
                 WHERE X.ProductId = CS.ProductId AND X.ReceiveId < CS.ReceiveId AND X.UnitPrice > 1
                 ORDER BY X.ReceiveId DESC) R
    WHERE CS.ReceiveDate >= @From)
SELECT c.*,
       CAST(c.UnitPrice / c.RefRate AS DECIMAL(18,4))                       AS ratio,
       CAST(c.StockInQty * (c.UnitPrice - c.RefRate) AS DECIMAL(18,2))      AS valueDistortion,   -- + over-stated, - under-stated
       (SELECT COUNT(*) FROM dbo.tblDCStore D WHERE D.TempReceiveId = c.ReceiveId) AS depotRowsCostedFromIt
FROM c
WHERE c.RefRate > 0 AND (c.UnitPrice > @Factor * c.RefRate OR c.UnitPrice < c.RefRate / @Factor)
ORDER BY c.ReceiveDate DESC, c.ReceiveId DESC;

-- Summary by product: how often it repeats. A product that is "wrong" every time
-- is more likely a stale reference than an entry error - check those first.
;WITH c AS (
    SELECT CS.ReceiveId, CS.ProductCode, CS.UnitPrice, R.RefRate
    FROM dbo.tblCentralStore CS
    OUTER APPLY (SELECT TOP 1 ISNULL(X.Avg_Rate, X.UnitPrice) RefRate FROM dbo.tblCentralStore X
                 WHERE X.ProductId = CS.ProductId AND X.ReceiveId < CS.ReceiveId AND X.UnitPrice > 1 ORDER BY X.ReceiveId DESC) R
    WHERE CS.ReceiveDate >= @From)
SELECT ProductCode, COUNT(*) receipts,
       SUM(CASE WHEN RefRate > 0 AND (UnitPrice > @Factor * RefRate OR UnitPrice < RefRate / @Factor) THEN 1 ELSE 0 END) outliers,
       MIN(UnitPrice) minPrice, MAX(UnitPrice) maxPrice
FROM c GROUP BY ProductCode
HAVING SUM(CASE WHEN RefRate > 0 AND (UnitPrice > @Factor * RefRate OR UnitPrice < RefRate / @Factor) THEN 1 ELSE 0 END) > 0
ORDER BY outliers DESC, receipts DESC;
GO
