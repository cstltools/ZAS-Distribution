-- ============================================================================
-- Fix: "Execution Timeout Expired" on PaymentTranscationNew2 SEARCH
-- (Is Single Customer -> Search). Measured on SalesRollDB_ZAS
-- (791K tblInvoice, 578K tblPaymentTranscationDetail): 181s elapsed,
-- 142s CPU for one customer with 91 invoices.
--
-- Plan: tblInvoice has no index on CustomerMasterId, so the customer
-- filter walks every invoice through a key lookup; then the
-- "InvoiceId NOT IN (SELECT InvoiceId FROM tblPaymentTranscationMaster
-- JOIN tblPaymentTranscationDetail ...)" anti-join re-scans the whole
-- detail table once PER invoice (~50M row compares).
-- Same shape in CustPaymentDAL.GetInvoiceByCustomer (both overloads),
-- LoadInvoiceNew and LoadInvoiceNewByUser.
--
-- Safe to run on LIVE during a quiet window (one read of each table).
-- Requires QUOTED_IDENTIFIER ON (sqlcmd: -I flag).
-- After: 544ms elapsed / 156ms CPU for the same search.
-- ============================================================================

USE SalesRollDB_ZAS;
GO

-- 1. Customer filter: WHERE INV.CustomerMasterId = @x
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblInvoice_CustomerMasterId')
    CREATE INDEX IX_tblInvoice_CustomerMasterId
        ON dbo.tblInvoice (CustomerMasterId);
GO

-- 2. Pending-payment anti-join: detail rows by InvoiceId, joined to master
--    on CustPayId (exact shape sys.dm_db_missing_index_details suggests)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblPTD_InvoiceId')
    CREATE INDEX IX_tblPTD_InvoiceId
        ON dbo.tblPaymentTranscationDetail (InvoiceId) INCLUDE (CustPayId);
GO
