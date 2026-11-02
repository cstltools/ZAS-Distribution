-- tblInvoice header (PaymentAmount/AIT/DiscountOnPayment/PaymentStatus) disagreed with tblCustPayDetail
-- for delivered invoices: header said paid (due < 1) while detail still has due >= 1 ("Paid But Due").
-- tblCustPayDetail is the source of truth. Re-sync the header for exactly those invoices.
-- Run with @Commit = 0 first (dry run, rolls back), then 1.
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
DECLARE @Commit BIT = 0;

BEGIN TRAN;

IF OBJECT_ID('dbo.tblInvoice_HdrPayBackup_20261102') IS NULL
    CREATE TABLE dbo.tblInvoice_HdrPayBackup_20261102 (InvoiceId INT, InvoiceNo VARCHAR(50), PaymentAmount DECIMAL(18,2), AIT DECIMAL(18,2),
        DiscountOnPayment DECIMAL(18,2), PaymentStatus VARCHAR(20), BackedUpAt DATETIME);

;WITH D AS (
    SELECT InvoiceId, SUM(PaymentAmount) PP, SUM(ISNULL(AIT,0)) AIT, SUM(ISNULL(Discount,0)) Disc
    FROM dbo.tblCustPayDetail GROUP BY InvoiceId
), T AS (
    SELECT I.InvoiceId, I.InvoiceNo, D.PP, D.AIT, D.Disc,
           I.DeliveryTpGrandTotal - (D.PP + D.AIT + D.Disc) DetDue
    FROM dbo.tblInvoice I JOIN D ON D.InvoiceId = I.InvoiceId
    WHERE I.DeliveryInvoiceStatus IN ('Full','Partial')
      AND I.DeliveryTpGrandTotal - (D.PP + D.AIT + D.Disc) >= 1
      AND I.DeliveryTpGrandTotal - (ISNULL(I.PaymentAmount,0) + ISNULL(I.AIT,0) + ISNULL(I.DiscountOnPayment,0)) < 1
)
SELECT I.InvoiceId INTO #Fix FROM dbo.tblInvoice I JOIN T ON T.InvoiceId = I.InvoiceId;

INSERT dbo.tblInvoice_HdrPayBackup_20261102 (InvoiceId, InvoiceNo, PaymentAmount, AIT, DiscountOnPayment, PaymentStatus, BackedUpAt)
SELECT I.InvoiceId, I.InvoiceNo, I.PaymentAmount, I.AIT, I.DiscountOnPayment, I.PaymentStatus, GETDATE()
FROM dbo.tblInvoice I JOIN #Fix F ON F.InvoiceId = I.InvoiceId;

UPDATE I SET PaymentAmount = D.PP, AIT = D.AIT, DiscountOnPayment = D.Disc, PaymentStatus = 'Partial'
FROM dbo.tblInvoice I
JOIN #Fix F ON F.InvoiceId = I.InvoiceId
JOIN (SELECT InvoiceId, SUM(PaymentAmount) PP, SUM(ISNULL(AIT,0)) AIT, SUM(ISNULL(Discount,0)) Disc
      FROM dbo.tblCustPayDetail GROUP BY InvoiceId) D ON D.InvoiceId = I.InvoiceId;

SELECT B.InvoiceNo, B.PaymentAmount oldPay, I.PaymentAmount newPay, B.AIT oldAIT, I.AIT newAIT,
       B.DiscountOnPayment oldDisc, I.DiscountOnPayment newDisc, B.PaymentStatus oldStatus, I.PaymentStatus newStatus
FROM dbo.tblInvoice_HdrPayBackup_20261102 B JOIN dbo.tblInvoice I ON I.InvoiceId = B.InvoiceId;

IF @Commit = 1 COMMIT TRAN ELSE ROLLBACK TRAN;
