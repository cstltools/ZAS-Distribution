-- Fix: sp_PMNT_CustomerPaymentDetailInsert wrote tblInvoice.PaymentAmount = cash+discount+AIT while
-- sp_Sales_CustomerPaymentPosting writes cash only (AIT/Discount in own columns). When the posting step
-- did not run, the header double counted discount/AIT and the invoice showed "paid" while
-- tblCustPayDetail still had a due. Only section 3 (header recompute) changed; the rest is the live body.
-- Rollback: re-run the CREATE of this proc from 2026-09-09_PaymentTranscationNew2_P1_Reliability.sql (section 2).
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO
ALTER PROCEDURE [dbo].[sp_PMNT_CustomerPaymentDetailInsert]
    @CustPayDetailId INT OUTPUT,
    @CustPayId INT,
    @InvoiceId INT,
    @PaymentAmount DECIMAL(18, 2) = NULL,
    @BankAccId INT = NULL,
    @CashAccId INT = NULL,
    @Remarks NVARCHAR(500) = NULL,
    @AIT DECIMAL(18, 2) = NULL,
    @Discount DECIMAL(18, 2) = NULL
AS
BEGIN
    BEGIN TRY

        -- 1. Duplicate guard (P1-5: race fix only, identity UNCHANGED --
        --    exact CustPayId + InvoiceId match, no time window).
        IF EXISTS (
            SELECT 1
            FROM dbo.tblCustPayDetail WITH (UPDLOCK, HOLDLOCK)
            WHERE CustPayId = @CustPayId
              AND InvoiceId = @InvoiceId
        )
        BEGIN
            SET @CustPayDetailId = 0;
            RETURN;
        END

        -- 2. Insert the payment detail (unchanged)
        INSERT INTO dbo.tblCustPayDetail
        (
            CustPayId,
            InvoiceId,
            PaymentAmount,
            BankAccId,
            CashAccId,
            PaymentRemarks,
            ProcessTime,
            AIT,
            Discount
        )
        VALUES
        (
            @CustPayId,
            @InvoiceId,
            ISNULL(@PaymentAmount, 0),
            ISNULL(@BankAccId, 0),
            ISNULL(@CashAccId, 0),
            @Remarks,
            GETDATE(),
            ISNULL(@AIT, 0),
            ISNULL(@Discount, 0)
        );

        SET @CustPayDetailId = SCOPE_IDENTITY();

        -- 3. tblInvoice header recompute from tblCustPayDetail (the source of truth).
        --    Same meaning as sp_Sales_CustomerPaymentPosting: PaymentAmount = CASH only,
        --    AIT and DiscountOnPayment kept in their own columns. Previously PaymentAmount
        --    held cash+discount+AIT, so if the posting step did not run, the header double
        --    counted discount/AIT ("Paid But Due" invoices).
        IF (ISNULL(@InvoiceId, 0) > 0)
        BEGIN
            DECLARE @Cash DECIMAL(18, 2) = 0, @TotAIT DECIMAL(18, 2) = 0, @TotDisc DECIMAL(18, 2) = 0;
            DECLARE @GrandTotal DECIMAL(18, 2) = 0;

            SELECT @Cash = SUM(ISNULL(PaymentAmount, 0)),
                   @TotAIT = SUM(ISNULL(AIT, 0)),
                   @TotDisc = SUM(ISNULL(Discount, 0))
            FROM dbo.tblCustPayDetail WITH (NOLOCK)
            WHERE InvoiceId = @InvoiceId;

            SELECT @GrandTotal = ISNULL(DeliveryTpGrandTotal, ISNULL(TpGrandTotal, 0))
            FROM dbo.tblInvoice WITH (NOLOCK)
            WHERE InvoiceId = @InvoiceId;

            UPDATE dbo.tblInvoice
            SET
                PaymentAmount = @Cash,
                AIT = @TotAIT,
                DiscountOnPayment = @TotDisc,
                PaymentStatus = CASE
                    WHEN @Cash + @TotAIT + @TotDisc >= @GrandTotal AND @GrandTotal > 0 THEN 'Full'
                    WHEN @Cash + @TotAIT + @TotDisc > 0 THEN 'Partial'
                    ELSE NULL
                END,
                UpdateDate = GETDATE()
            WHERE InvoiceId = @InvoiceId;
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage2 NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage2, 16, 1);
    END CATCH
END
GO
