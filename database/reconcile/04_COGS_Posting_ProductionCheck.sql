-- Run on PRODUCTION (CSTL-VM-01\MSSQLSERVER2019), in SalesRollDB_ZAS. Read-only.
-- Local copy of CSTL_ACCDB is a 2026-08-23 restore, so "no COGS after Aug 22" cannot be judged locally.
USE SalesRollDB_ZAS;
SET NOCOUNT ON;

-- 1. Which accounting DBs exist here, and does the COGS proc point at one of them
SELECT name, state_desc, is_read_only FROM sys.databases WHERE name IN ('CSTL_ACCDB','CSTL_ERP_ACCDB','ZAS_ACCDB');
SELECT o.name, o.modify_date,
       CASE WHEN m.definition LIKE '%CSTL_ERP_ACCDB%' THEN 'CSTL_ERP_ACCDB' WHEN m.definition LIKE '%CSTL_ACCDB%' THEN 'CSTL_ACCDB' ELSE '?' END accdb
FROM sys.sql_modules m JOIN sys.objects o ON o.object_id = m.object_id
WHERE o.name IN ('sp_Sales_ExecuteDeliveryPipeline','sp_Sales_DeliveryCOGSPosting','sp_Process_WeightedAverageSalesConfirmation');

-- 2. Delivered invoices per week vs WAC row (step 1) vs COGS voucher (step 2)
;WITH inv AS (
  SELECT I.InvoiceId, ISNULL(I.UpdateDate, I.InvoiceDate) d,
         CASE WHEN EXISTS (SELECT 1 FROM tbl_INV_WeightedAvg w WITH (NOLOCK) WHERE w.Trans_Type_Id = 4 AND w.Mst_Trans_Id = I.InvoiceId) THEN 1 ELSE 0 END hasWac,
         CASE WHEN EXISTS (SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster v WITH (NOLOCK)
                           WHERE v.TableID = 20 AND v.CommonPK_ID = I.InvoiceId AND v.ActionStatus <> 'Cancelled' AND v.Narration LIKE '%Cost of Goods Sold (COGS)%') THEN 1 ELSE 0 END hasCogs
  FROM tblInvoice I WITH (NOLOCK)
  WHERE I.DeliveryInvoiceStatus IN ('Full','Partial') AND ISNULL(I.UpdateDate, I.InvoiceDate) >= '2026-08-01')
SELECT DATEADD(wk, DATEDIFF(wk, 0, d), 0) wk, COUNT(*) delivered, SUM(hasWac) hasWac, SUM(hasCogs) hasCogs
FROM inv GROUP BY DATEADD(wk, DATEDIFF(wk, 0, d), 0) ORDER BY 1;

-- 3. If hasCogs is 0 after Aug 22: run the proc on one such invoice and read the error (rolled back)
DECLARE @Inv INT = (SELECT TOP 1 I.InvoiceId FROM tblInvoice I WITH (NOLOCK)
                    WHERE I.DeliveryInvoiceStatus IN ('Full','Partial') AND ISNULL(I.UpdateDate, I.InvoiceDate) >= '2026-09-01'
                      AND NOT EXISTS (SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster v WITH (NOLOCK) WHERE v.TableID = 20 AND v.CommonPK_ID = I.InvoiceId)
                    ORDER BY ISNULL(I.UpdateDate, I.InvoiceDate) DESC);
BEGIN TRAN;
BEGIN TRY
    EXEC dbo.sp_Sales_DeliveryCOGSPosting @InvoiceId = @Inv;
    SELECT @Inv invoice, ISNULL((SELECT TOP 1 'inserted ' + VoucherCode FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WHERE TableID = 20 AND CommonPK_ID = @Inv), 'NO INSERT (silent: TotalCost <= 0)') result;
END TRY
BEGIN CATCH
    SELECT @Inv invoice, 'ERROR ' + CAST(ERROR_NUMBER() AS VARCHAR) + ': ' + ERROR_MESSAGE() result;
END CATCH
ROLLBACK TRAN;

-- 4. Also read on the web server: <site root>\App_Data\DatabaseErrors.log (UpdateAction swallows the SP error and logs it there)

-- 5. GL COGS vs inventory ledger per month (after 2026-10-21_DeliveryCOGS_UseLedgerRate.sql, mismatch must be 0 for new postings)
SELECT DATEADD(mm, DATEDIFF(mm, 0, v.PostingDate), 0) mon, COUNT(*) posted,
       SUM(CASE WHEN ABS(v.TotalAmountBDT - w.ledger) > 1 THEN 1 ELSE 0 END) mismatch,
       SUM(v.TotalAmountBDT - w.ledger) gl_minus_ledger
FROM CSTL_ACCDB..tblDebitCreditVoucherMaster v WITH (NOLOCK)
CROSS APPLY (SELECT ISNULL(SUM(-Curr_Amt), 0) ledger FROM tbl_INV_WeightedAvg WITH (NOLOCK) WHERE Trans_Type_Id = 4 AND Mst_Trans_Id = v.CommonPK_ID AND Curr_Qty < 0) w
WHERE v.TableID = 20 AND v.ActionStatus <> 'Cancelled' AND v.Narration LIKE '%Cost of Goods Sold (COGS)%' AND v.PostingDate >= '2026-08-01'
GROUP BY DATEADD(mm, DATEDIFF(mm, 0, v.PostingDate), 0) ORDER BY 1;
