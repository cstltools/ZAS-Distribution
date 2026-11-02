-- ============================================================================
-- Fix: the same order can get two invoices (e.g. ODR-BD25-2610240040 ->
-- INV-BD25-2610240024 and INV-BD25-2610240027).
--
-- Root cause (audit ORD-002): the only guards are check-then-act -
--   * InvoiceCreationForCustomerByOrder.OrderExists() SELECTs before the
--     transaction opens,
--   * sp_I_InvoiceMaster rejects only if the same OrderNo was invoiced in
--     the last 60 seconds.
-- Two users / two tabs / a retry after 60s all slip through. Nothing in the
-- schema says "one invoice per order".
--
-- Fix: a UNIQUE filtered index on tblInvoice(OrderId). The second insert
-- fails with error 2601, SaveAction logs it and returns 0, and the page's
-- existing invId <= 0 branch rolls back and tells the user the order was
-- already invoiced. No application change required.
--
-- Historic duplicates already exist (40 orders on the restored copy), so
-- the index is filtered to InvoiceId > MAX(InvoiceId) at the moment this
-- script runs: old rows are left alone, every new row is enforced.
-- Run against LIVE SalesRollDB_ZAS. Builds in seconds (filter is empty).
--
-- Filtered indexes require QUOTED_IDENTIFIER ON on every writer. ADO.NET and
-- SSMS default it ON, and all 66 procs that write tblInvoice were checked
-- (sys.sql_modules.uses_quoted_identifier = 1, no triggers on the table).
-- Ad-hoc scripts run via sqlcmd need the -I flag.
-- ============================================================================

USE SalesRollDB_ZAS;
GO
SET QUOTED_IDENTIFIER ON;  -- required for filtered indexes (sqlcmd defaults it OFF)
GO

-- 0. Report the legacy duplicates so someone can review them by hand.
SELECT OrderId, OrderNo, COUNT(*) AS Invoices,
       STRING_AGG(InvoiceNo, ', ') AS InvoiceNos
FROM dbo.tblInvoice
WHERE OrderId IS NOT NULL
GROUP BY OrderId, OrderNo
HAVING COUNT(*) > 1
ORDER BY OrderId;
GO

-- 1. One invoice per order, enforced for everything created from now on.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_tblInvoice_OrderId_OneInvoice')
BEGIN
    DECLARE @cutoff INT = (SELECT ISNULL(MAX(InvoiceId), 0) FROM dbo.tblInvoice);
    DECLARE @sql NVARCHAR(MAX) = N'
        CREATE UNIQUE INDEX UX_tblInvoice_OrderId_OneInvoice
            ON dbo.tblInvoice (OrderId)
            WHERE OrderId IS NOT NULL AND InvoiceId > ' + CAST(@cutoff AS NVARCHAR(20)) + N';';
    EXEC sp_executesql @sql;
    PRINT 'Created UX_tblInvoice_OrderId_OneInvoice, enforced for InvoiceId > ' + CAST(@cutoff AS NVARCHAR(20));
END
GO

-- Rollback:
-- DROP INDEX UX_tblInvoice_OrderId_OneInvoice ON dbo.tblInvoice;
