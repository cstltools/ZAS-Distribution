-- ============================================================================
-- Fix: "Server took too long or failed" on PaymentTranscationNew2 submit.
--
-- Measured on a restored copy of SalesRollDB_ZAS (801K rows in
-- tblCustomerPay): the tables on the payment-save path have NO indexes
-- except their primary keys. Every save therefore runs, PER INVOICE LINE:
--   1. duplicate-guard EXISTS on tblCustomerPay  -> full scan, ~1.3s,
--      under UPDLOCK+HOLDLOCK = whole-table range lock, so concurrent
--      collectors serialize behind each other
--   2. PaymentCode MAX-seq on tblCustomerPay     -> full scan (~0.1s idle)
--   3. stale-row UPDATE scan on tblPaymentTranscationMaster (~0.15s idle)
-- plus one MoneyReceiptNo MAX-seq scan on tblMoneyReceipt per receipt.
-- A 10-invoice submit on a busy live server exceeds the 60s CommandTimeout.
--
-- These four indexes turn every one of those scans into a seek.
-- Run against the LIVE SalesRollDB_ZAS during a quiet window (each index
-- build reads the whole table once; a few seconds to a minute each).
-- ============================================================================

USE SalesRollDB_ZAS;
GO

-- 1. Duplicate-payment guard: WHERE CreateBy=@x AND InvoiceIdForValidation=@y
--    AND PaymentAmount=@z AND CreateDate within 120s
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblCustomerPay_DupGuard')
    CREATE INDEX IX_tblCustomerPay_DupGuard
        ON dbo.tblCustomerPay (CreateBy, InvoiceIdForValidation, CreateDate)
        INCLUDE (PaymentAmount);
GO

-- 2. PaymentCode sequence: WHERE CAST(CreateDate AS DATE) = today AND
--    PaymentCode LIKE 'PMNT-yyMMdd%'. PaymentCode is NVARCHAR(MAX) so it
--    cannot be an index KEY; key on CreateDate instead (the CAST-to-DATE
--    predicate is sargable) and carry PaymentCode as an included column.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblCustomerPay_CreateDate')
    CREATE INDEX IX_tblCustomerPay_CreateDate
        ON dbo.tblCustomerPay (CreateDate) INCLUDE (PaymentCode);
GO

-- 3. MoneyReceiptNo sequence in sp_MRCP_MoneyReceiptMasterInsert
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblMoneyReceipt_MoneyReceiptNo')
    CREATE INDEX IX_tblMoneyReceipt_MoneyReceiptNo
        ON dbo.tblMoneyReceipt (MoneyReceiptNo) INCLUDE (CreateDate);
GO

-- 4. Unconditional stale-row UPDATE inside sp_Sales_CustomerPaymentPosting:
--    WHERE IsProcess=0 AND IsReject=0 AND CreateDate < today
--    (currently scans 578K rows on EVERY posting call)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblPTM_Stale')
    CREATE INDEX IX_tblPTM_Stale
        ON dbo.tblPaymentTranscationMaster (IsProcess, IsReject, CreateDate);
GO

-- 5. Duplicate receipt-number guard. History already holds duplicates
--    (RCPT-2507131020: two users at the same millisecond under the old
--    COUNT(*)+1 scheme; RCPT-2506301061; two NULLs), so the unique index
--    is FILTERED to rows created from 2026-10-15 onward -- it enforces
--    uniqueness going forward without touching history. Requires
--    QUOTED_IDENTIFIER ON (sqlcmd: -I flag). PaymentCode cannot get the
--    same guard (NVARCHAR(MAX) cannot be an index key); its generation is
--    already serialized by UPDLOCK+HOLDLOCK in the insert SP.
SET QUOTED_IDENTIFIER ON;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_tblMoneyReceipt_MoneyReceiptNo_New')
    CREATE UNIQUE INDEX UX_tblMoneyReceipt_MoneyReceiptNo_New
        ON dbo.tblMoneyReceipt (MoneyReceiptNo)
        WHERE MoneyReceiptNo IS NOT NULL AND CreateDate >= '2026-10-15';
GO

-- Verify
SELECT o.name AS tbl, i.name AS ix
FROM sys.indexes i JOIN sys.objects o ON o.object_id = i.object_id
WHERE i.name IN ('IX_tblCustomerPay_DupGuard','IX_tblCustomerPay_CreateDate',
                 'IX_tblMoneyReceipt_MoneyReceiptNo','IX_tblPTM_Stale',
                 'UX_tblMoneyReceipt_MoneyReceiptNo_New');
GO
