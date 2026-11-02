-- Delivery Confirmation Full: invoice vs posted vouchers, one row per invoice
-- Run in SalesrollDB_ZAS. Accounting DB: current SPs write to CSTL_ACCDB.
-- To check the old database instead, replace CSTL_ACCDB with CSTL_ERP_ACCDB (find/replace, 4 places).
--
-- Expected per invoice (from SP bodies):
--   Sales voucher  TableID 2014, IsInventoryValuation 0 : Dr AR / Cr Sales = SUM(tblInvoiceDetail.DeliveryNetAmount)
--   COGS voucher   TableID 20,   IsInventoryValuation 1 : Dr COGS / Cr FG  = SUM(DeliveryTotalQuantity * DCStore.UnitPrice_CostPrice)
--
-- Mismatch codes:
--   NO_SALES_VOUCHER     expected (full-delivery pipeline never posts it)
--   NO_COGS_VOUCHER      pipeline failed or cost = 0
--   SALES_AMT_DIFF       voucher total <> invoice net
--   COGS_AMT_DIFF        voucher total <> recomputed cost
--   DUP_SALES / DUP_COGS more than one live voucher for same invoice
--   DR_CR_UNBALANCED     detail Dr <> Cr inside a voucher

DECLARE @FromDate DATE = '2025-04-25';   -- catch-up jobs start here; widen if needed
DECLARE @ToDate   DATE = GETDATE();

;WITH Inv AS (
    SELECT  I.InvoiceId, I.InvoiceNo, I.DelivaryInvoiceNo, I.InvoiceDate,
            ISNULL(I.UpdateDate, I.InvoiceDate) AS DeliveryDate,
            I.DeliveryInvoiceStatus, I.CustomerMasterId, CU.CompanyId,
            CAST(ISNULL(SUM(D.DeliveryNetAmount), 0) AS DECIMAL(18,2)) AS ExpSales,
            CAST(ISNULL(SUM(CAST(ISNULL(D.DeliveryTotalQuantity, D.TotalQuantity) AS DECIMAL(18,4))
                 * ISNULL(S.UnitPrice_CostPrice, ISNULL(UP.UnitPrice, 0))), 0) AS DECIMAL(18,2)) AS ExpCOGS
    FROM dbo.tblInvoice I WITH (NOLOCK)
    JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON CU.ComUnitId = I.ComUnitId
    LEFT JOIN dbo.tblInvoiceDetail D WITH (NOLOCK) ON D.InvoiceId = I.InvoiceId
    LEFT JOIN dbo.tblDCStore S WITH (NOLOCK) ON S.DCStoreId = D.DCStoreId
    LEFT JOIN dbo.tblProduct P WITH (NOLOCK) ON P.ProductCode = D.ProductCode
    LEFT JOIN dbo.tblUnitPrice UP WITH (NOLOCK) ON UP.ProductId = P.ProductId AND UP.IsActive = 1
    WHERE I.DeliveryInvoiceStatus = 'Full'
      AND CAST(ISNULL(I.UpdateDate, I.InvoiceDate) AS DATE) BETWEEN @FromDate AND @ToDate
    GROUP BY I.InvoiceId, I.InvoiceNo, I.DelivaryInvoiceNo, I.InvoiceDate, I.UpdateDate,
             I.DeliveryInvoiceStatus, I.CustomerMasterId, CU.CompanyId
),
V AS (
    SELECT  M.CommonPK_ID AS InvoiceId,
            CASE WHEN M.TableID = 2014 AND ISNULL(M.IsInventoryValuation,0) = 0 THEN 'SALES'
                 WHEN M.TableID = 20 THEN 'COGS' END AS Kind,
            COUNT(DISTINCT M.VoucherMasterId)                 AS VoucherCnt,
            MIN(M.VoucherCode)                                AS VoucherCode,
            MIN(M.PostingDate)                                AS PostingDate,
            CAST(SUM(D.DebitAmountBDT)  AS DECIMAL(18,2))     AS Dr,
            CAST(SUM(D.CreditAmountBDT) AS DECIMAL(18,2))     AS Cr
    FROM CSTL_ACCDB..tblDebitCreditVoucherMaster M WITH (NOLOCK)
    JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail D WITH (NOLOCK) ON D.VoucherMasterId = M.VoucherMasterId
    WHERE M.ActionStatus <> 'Cancelled'
      AND ( (M.TableID = 2014 AND ISNULL(M.IsInventoryValuation,0) = 0) OR M.TableID = 20 )
    GROUP BY M.CommonPK_ID,
             CASE WHEN M.TableID = 2014 AND ISNULL(M.IsInventoryValuation,0) = 0 THEN 'SALES'
                  WHEN M.TableID = 20 THEN 'COGS' END
)
SELECT  Inv.InvoiceId, Inv.InvoiceNo, Inv.DelivaryInvoiceNo, Inv.CompanyId,
        CAST(Inv.DeliveryDate AS DATE) AS DeliveryDate, Inv.CustomerMasterId,
        Inv.ExpSales,  S.VoucherCode AS SalesVoucher, S.PostingDate AS SalesPostDate, S.Dr AS SalesDr, S.Cr AS SalesCr,
        Inv.ExpCOGS,   C.VoucherCode AS COGSVoucher,  C.PostingDate AS COGSPostDate,  C.Dr AS COGSDr,  C.Cr AS COGSCr,
        STUFF(
              CASE WHEN S.InvoiceId IS NULL AND Inv.ExpSales > 0        THEN ',NO_SALES_VOUCHER' ELSE '' END
            + CASE WHEN C.InvoiceId IS NULL AND Inv.ExpCOGS  > 0        THEN ',NO_COGS_VOUCHER'  ELSE '' END
            + CASE WHEN S.InvoiceId IS NOT NULL AND S.Dr <> Inv.ExpSales THEN ',SALES_AMT_DIFF'   ELSE '' END
            + CASE WHEN C.InvoiceId IS NOT NULL AND C.Dr <> Inv.ExpCOGS  THEN ',COGS_AMT_DIFF'    ELSE '' END
            + CASE WHEN S.VoucherCnt > 1                                 THEN ',DUP_SALES'        ELSE '' END
            + CASE WHEN C.VoucherCnt > 1                                 THEN ',DUP_COGS'         ELSE '' END
            + CASE WHEN S.Dr <> S.Cr OR C.Dr <> C.Cr                     THEN ',DR_CR_UNBALANCED' ELSE '' END
        , 1, 1, '') AS Mismatch
FROM Inv
LEFT JOIN V S ON S.InvoiceId = Inv.InvoiceId AND S.Kind = 'SALES'
LEFT JOIN V C ON C.InvoiceId = Inv.InvoiceId AND C.Kind = 'COGS'
WHERE   S.InvoiceId IS NULL AND Inv.ExpSales > 0
     OR C.InvoiceId IS NULL AND Inv.ExpCOGS  > 0
     OR S.Dr <> Inv.ExpSales
     OR C.Dr <> Inv.ExpCOGS
     OR S.VoucherCnt > 1 OR C.VoucherCnt > 1
     OR S.Dr <> S.Cr OR C.Dr <> C.Cr
ORDER BY Inv.DeliveryDate DESC, Inv.InvoiceId;

-- Summary by mismatch type (uncomment to run after the detail)
-- SELECT Mismatch, COUNT(*) Invoices, SUM(ExpSales) Sales, SUM(ExpCOGS) COGS FROM (<query above without ORDER BY>) X GROUP BY Mismatch;
