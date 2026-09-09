/*
============================================================================
 PaymentTranscationNew2 — P1 reliability + targeted performance remediation
 docs/audit/PaymentTransactionNew2_*.md

 This script was written against the ACTUAL LIVE definitions of all four
 procedures below, pulled via OBJECT_DEFINITION() from
 TOWSIF\MSSQLSERVER2019 / SalesRollDB_ZAS on 2026-09-09. Two of the four
 procedures were found to differ from the audit docs' recovered bodies:

   - sp_PMNT_CustomerPaymentMasterInsert: live duplicate-guard identity is
     (CreateBy, InvoiceIdForValidation, PaymentAmount) within 120 seconds,
     using WITH (NOLOCK) — NOT the 300-second/6-field version described in
     docs/database/stored-procedures-part2.md.
   - sp_PMNT_CustomerPaymentDetailInsert: live duplicate-guard is an exact
     (CustPayId, InvoiceId) match with NO time window, using WITH (NOLOCK) —
     NOT the 30-second/5-field version described in the docs. Since CustPayId
     is a freshly-generated identity on every call in the current app design,
     this guard essentially never fires today; fixed for correctness anyway.
   - sp_MRCP_MoneyReceiptMasterInsert and sp_Sales_CustomerPaymentPosting
     matched the docs' recovered bodies exactly.

 This script edits the CURRENT live bodies, not the documented ones.

 Implements, in order:
   1. sp_PMNT_CustomerPaymentMasterInsert
        - P1-5: closes the check-then-insert race on the duplicate-payment
          guard (WITH (NOLOCK) -> WITH (UPDLOCK, HOLDLOCK)). Identity fields
          and the 120-second window are UNCHANGED (existing business rule,
          not redesigned).
        - PERF: PaymentCode generation changed from a same-day COUNT(*)+1
          read to a locked MAX(suffix)+1 read, immune to gaps from any
          historically deleted row. Format unchanged (PMNT-yyMMdd####).
   2. sp_PMNT_CustomerPaymentDetailInsert
        - P1-5: same NOLOCK -> UPDLOCK,HOLDLOCK fix on its exact-match
          duplicate guard. Identity (CustPayId, InvoiceId) unchanged. The
          existing tblInvoice payment-status update (already scoped to
          @InvoiceId) is untouched.
   3. sp_MRCP_MoneyReceiptMasterInsert
        - P1-1/P1-3 support: the 30-second (CreateBy, MonyReceiptDate)
          duplicate guard is REMOVED. Rationale: the C# caller (see
          PaymentTranscationDal.SaveCustPaymentNew2) now wraps Master
          insert + Detail insert + Journal posting + this Money Receipt
          insert in ONE transaction, each already protected by its own
          identity guard (steps 1-2 above). The exact same CustPayDetailsId
          set can therefore never be submitted twice successfully, which
          makes this guard redundant. It was also actively harmful: it is
          keyed only on (CreateBy, MonyReceiptDate), so it silently
          discarded the receipt for a user's SECOND legitimate, DIFFERENT
          payment made within 30 seconds of their first — the app then
          displayed an unrelated older receipt with a
          "Data Already Exist" message. Removing it also removes the
          orphan-risk tension flagged in
          docs/audit/PaymentTransactionNew2_MoneyReceipt_Recovery.md — since
          nothing about this insert can legitimately be "a duplicate" any
          more, a receipt can no longer both belong to a successful payment
          and fail to be created.
        - PERF-7: MoneyReceiptNo generation changed from a same-day
          COUNT(MoneyReceiptId)+1 read to a locked MAX(suffix)+1 read
          (WITH (UPDLOCK, HOLDLOCK), scoped to today's rows) — closes the
          receipt-number race that already produced 2 confirmed duplicate
          MoneyReceiptNo values in production (see MoneyReceipt_Recovery.md
          section 8C). Format unchanged (RCPT-yyMMdd####).
   4. sp_Sales_CustomerPaymentPosting
        - P1-2/PERF-8: wrapped in BEGIN TRY/CATCH + RAISERROR so a real
          failure becomes a thrown SqlException the C# caller can react to,
          instead of relying on ExecuteNonQuery's ambiguous rows-affected
          return from a multi-statement, no-SET-NOCOUNT procedure (the task
          explicitly warns not to treat 0/false as a reliable failure
          signal here — this makes the signal reliable by using a real
          exception instead).
        - PERF-9: removed the unconditional
          "UPDATE tblPaymentTranscationMaster SET IsReject = 1 WHERE
          CONVERT(DATE,CreateDate) < CONVERT(DATE,GETDATE()) AND IsProcess=0
          AND IsReject=0" statement that ran on every single posting call
          regardless of @InvoiceId/@CustPayId. Verified live on 2026-09-09:
              SELECT COUNT(*), MAX(CreateDate), SUM(CASE WHEN IsProcess=0
                     AND IsReject=0 THEN 1 ELSE 0 END)
              FROM tblPaymentTranscationMaster
          returned 577987 rows / MAX(CreateDate) = 2025-06-24 / 0 matching
          rows. This table belongs to the legacy, unreachable
          saveButton_Click / SaveCustPayment code path (writes to
          tblPaymentTranscationMaster / tblPaymentTranscationDetail, not
          tblCustomerPay / tblCustPayDetail) and has had zero writes for
          14+ months. The statement has been updating 0 rows on every call
          for that entire period — safe to remove. No other logic in this
          procedure (the invoice aggregate update, the journal-posting
          cursor) is changed.

 No BEGIN TRANSACTION is added inside any of these four procedures. They
 now run under an AMBIENT .NET-managed transaction opened by the C# caller
 (PaymentTranscationDal.SaveCustPaymentNew2, via a new DB_Manager helper) —
 adding a nested BEGIN TRAN/ROLLBACK here would terminate that ambient
 transaction on any inner rollback. TRY/CATCH + RAISERROR is sufficient to
 surface a real error as an exception without touching transaction control.

 NOT changed by this script (explicitly out of scope / preserved as-is):
   - The journal-posting cursor logic and voucher/GL insert statements in
     sp_Sales_CustomerPaymentPosting.
   - The tblInvoice PaymentAmount/PaymentStatus recompute in
     sp_PMNT_CustomerPaymentDetailInsert (already correctly scoped).
   - ExecuteAllSqlQueryByStoreProcedure (PERF-8 wrapper) — confirmed not on
     the write path this script touches; left untouched per the task's
     explicit "only optimize calls directly related to this page's save
     flow" instruction.
   - The CSV LIKE join in BatchWiseCollectionReportDal.MoneyReceiptById
     (PERF-10) — confirmed zero FKs/relational key exist for
     tblMoneyReceipt.CustPayDetailsId; left as documented performance debt,
     not schema-redesigned.
   - No historical rows in tblCustomerPay / tblCustPayDetail / tblMoneyReceipt
     / tblPaymentTranscationMaster are modified, deleted, or backfilled by
     this script.

 Deployment: run against SalesRollDB_ZAS (the "SSIDB" connection; confirmed
 reachable as TOWSIF\MSSQLSERVER2019 / SalesRollDB_ZAS in this environment).
 Idempotent to re-run (ALTER PROCEDURE).
============================================================================
*/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================================
-- 1. sp_PMNT_CustomerPaymentMasterInsert  (edited from the LIVE body)
-- ============================================================================
ALTER PROCEDURE [dbo].[sp_PMNT_CustomerPaymentMasterInsert]
    @CustPayId INT OUT,
    @CustomerMasterId INT NULL,
    @BatchId INT NULL,
    @PaymentDate DATETIME NULL,
    @PaymentAmount DECIMAL(18,2) NULL,
    @PayType NVARCHAR(200) NULL,
    @RefNo  NVARCHAR(200) NULL,
    @RefDate DATETIME NULL,
    @CreateBy NVARCHAR(200) NULL,
    @PaymentRemarks NVARCHAR(MAX) NULL,
    @InvoiceIdForValidation INT = NULL
AS
BEGIN
    BEGIN TRY

        -- 1. Duplicate-payment guard (P1-5: race fix only, identity/window
        --    UNCHANGED from the live procedure). UPDLOCK+HOLDLOCK on this
        --    narrow, already-selective predicate (CreateBy + InvoiceId +
        --    Amount + 120s) closes the check-then-insert gap: a concurrent
        --    second call for the same identity blocks here until the first
        --    call's insert (below) commits or rolls back, then re-evaluates
        --    against the now-committed result.
        IF (ISNULL(@InvoiceIdForValidation, 0) > 0)
        BEGIN
            IF EXISTS (
                SELECT 1
                FROM dbo.tblCustomerPay WITH (UPDLOCK, HOLDLOCK)
                WHERE CreateBy = @CreateBy
                  AND ISNULL(InvoiceIdForValidation, 0) = @InvoiceIdForValidation
                  AND ISNULL(PaymentAmount, 0) = ISNULL(@PaymentAmount, 0)
                  AND DATEDIFF(SECOND, CreateDate, GETDATE()) < 120
            )
            BEGIN
                SET @CustPayId = 0;
                RETURN;
            END
        END

        -- 2. PaymentCode generation (PERF: COUNT(*)+1 -> locked MAX+1,
        --    immune to a gap from any historically deleted row for the
        --    same day). Format unchanged: 'PMNT-' + yyMMdd + (1000 + seq).
        DECLARE @SpotPurchaseCode NVARCHAR(500);
        DECLARE @Prefix NVARCHAR(20) = 'PMNT-' + FORMAT(GETDATE(), 'yyMMdd');
        DECLARE @NextSeq INT;

        SELECT @NextSeq = ISNULL(MAX(TRY_CAST(SUBSTRING(PaymentCode, LEN(@Prefix) + 1, 10) AS INT)), 1000) + 1
        FROM dbo.tblCustomerPay WITH (UPDLOCK, HOLDLOCK)
        WHERE CAST(CreateDate AS DATE) = CAST(GETDATE() AS DATE)
          AND PaymentCode LIKE @Prefix + '%';

        SET @SpotPurchaseCode = @Prefix + CAST(@NextSeq AS NVARCHAR(10));

        -- 3. Insert the payment master (unchanged)
        INSERT INTO [dbo].[tblCustomerPay]
        (
            PaymentCode,
            CustomerMasterId,
            PaymentDate,
            PaymentAmount,
            PayType,
            RefNo,
            RefDate,
            CreateBy,
            CreateDate,
            MasterProcessTime,
            BatchId,
            PaymentRemarks,
            InvoiceIdForValidation
        )
        VALUES
        (
            @SpotPurchaseCode,
            @CustomerMasterId,
            @PaymentDate,
            @PaymentAmount,
            @PayType,
            @RefNo,
            @RefDate,
            @CreateBy,
            GETDATE(),
            GETDATE(),
            @BatchId,
            @PaymentRemarks,
            @InvoiceIdForValidation
        );

        SET @CustPayId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage1 NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage1, 16, 1);
    END CATCH
END
GO


-- ============================================================================
-- 2. sp_PMNT_CustomerPaymentDetailInsert  (edited from the LIVE body)
-- ============================================================================
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

        -- 3. tblInvoice PaymentAmount / PaymentStatus recompute (unchanged,
        --    already scoped to @InvoiceId only)
        IF (ISNULL(@InvoiceId, 0) > 0)
        BEGIN
            DECLARE @TotalPaid DECIMAL(18, 2) = 0;
            DECLARE @GrandTotal DECIMAL(18, 2) = 0;

            SELECT @TotalPaid = SUM(ISNULL(PaymentAmount, 0) + ISNULL(Discount, 0) + ISNULL(AIT, 0))
            FROM dbo.tblCustPayDetail WITH (NOLOCK)
            WHERE InvoiceId = @InvoiceId;

            SELECT @GrandTotal = ISNULL(DeliveryTpGrandTotal, ISNULL(TpGrandTotal, 0))
            FROM dbo.tblInvoice WITH (NOLOCK)
            WHERE InvoiceId = @InvoiceId;

            UPDATE dbo.tblInvoice
            SET
                PaymentAmount = @TotalPaid,
                PaymentStatus = CASE
                    WHEN @TotalPaid >= @GrandTotal AND @GrandTotal > 0 THEN 'Full'
                    WHEN @TotalPaid > 0 THEN 'Partial'
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


-- ============================================================================
-- 3. sp_MRCP_MoneyReceiptMasterInsert  (edited from the LIVE body)
-- ============================================================================
ALTER PROCEDURE [dbo].[sp_MRCP_MoneyReceiptMasterInsert]

         @MoneyReceiptID INT OUT
        ,@MoneyReceiptDate DATETIME
        ,@BatchId INT NULL
        ,@CustomerMasterId INT NULL
        ,@CustPayDetailsId NVARCHAR(MAX) NULL
        ,@CreateBy NVARCHAR(200)
AS
BEGIN
    BEGIN TRY

        -- The prior 30-second (CreateBy, MonyReceiptDate) duplicate guard
        -- is REMOVED here -- see the migration header for the full
        -- rationale (redundant with the payment-level guards now that this
        -- insert runs inside the same transaction as Master/Detail/Journal,
        -- and was suppressing legitimate back-to-back different payments).

        -- MoneyReceiptNo generation (PERF-7: COUNT+1 -> locked MAX+1,
        -- immune to a gap from any historically deleted row). Format
        -- unchanged: 'RCPT-' + yy + MMdd + (1000 + seq).
        DECLARE @SpotPurchaseCode NVARCHAR(500);
        DECLARE @Prefix NVARCHAR(20) = 'RCPT-' + RIGHT(YEAR(GETDATE()), 2) + FORMAT(GETDATE(), 'MMdd');
        DECLARE @NextSeq INT;

        SELECT @NextSeq = ISNULL(MAX(TRY_CAST(SUBSTRING(MoneyReceiptNo, LEN(@Prefix) + 1, 10) AS INT)), 1000) + 1
        FROM tblMoneyReceipt WITH (UPDLOCK, HOLDLOCK)
        WHERE CAST(CreateDate AS DATE) = CAST(GETDATE() AS DATE)
          AND MoneyReceiptNo LIKE @Prefix + '%';

        SET @SpotPurchaseCode = @Prefix + CAST(@NextSeq AS NVARCHAR(10));

        INSERT INTO [dbo].[tblMoneyReceipt]
               (MoneyReceiptNo
               ,MonyReceiptDate
               ,BatchId
               ,CustMasterId
               ,CustPayDetailsId
               ,CreateBy)
         VALUES
               (@SpotPurchaseCode, @MoneyReceiptDate, @BatchId, @CustomerMasterId, @CustPayDetailsId, @CreateBy)

        SET @MoneyReceiptID = SCOPE_IDENTITY()
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage3 NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage3, 16, 1);
    END CATCH
END
GO


-- ============================================================================
-- 4. sp_Sales_CustomerPaymentPosting  (edited from the LIVE body)
-- ============================================================================
ALTER PROCEDURE [dbo].[sp_Sales_CustomerPaymentPosting]

    @InvoiceId INT,
    @CustPayId INT

AS
BEGIN
    BEGIN TRY

        -- The prior unconditional
        --   UPDATE tblPaymentTranscationMaster SET IsReject = 1
        --   WHERE CONVERT(DATE,CreateDate) < CONVERT(DATE,GETDATE())
        --     AND IsProcess = 0 AND IsReject = 0
        -- statement is REMOVED here (PERF-9). See the migration header for
        -- the live verification evidence (dead table, 0 matching rows,
        -- untouched for 14+ months) -- this ran on every single call
        -- regardless of @InvoiceId/@CustPayId for no effect.

        -- Update Invoice (unchanged)
        UPDATE INV
        SET
            PaymentAmount = AGG.PaymentAmount,
            AIT = AGG.AIT,
            DiscountOnPayment = AGG.Discount
        FROM tblInvoice AS INV
        INNER JOIN (
            SELECT
                InvoiceId,
                SUM(ISNULL(PaymentAmount, 0)) AS PaymentAmount,
                SUM(ISNULL(AIT, 0)) AS AIT,
                SUM(ISNULL(Discount, 0)) AS Discount
            FROM tblCustPayDetail
            WHERE InvoiceId = @InvoiceId
            GROUP BY InvoiceId
        ) AS AGG ON INV.InvoiceId = AGG.InvoiceId
        WHERE INV.InvoiceId = @InvoiceId;

        -- Journal Posting (unchanged below this point)

        DECLARE @VoucherMasterId INT
        DECLARE @VoucherCode NVARCHAR(MAX)
        DECLARE @BiusinessUnitId INT
        DECLARE @FinancialYearId INT
        DECLARE @CurrencyId INT = 1
        DECLARE @Conversionrate DECIMAL(18,4) = 1.00
        DECLARE @VoucherType NVARCHAR(200) ='Journal'
        DECLARE @ApprovalDate DATETIME
        DECLARE @ApproveBy NVARCHAR(200)
        DECLARE @TableId INT
        DECLARE @InvoiceNo  NVARCHAR(200)
        DECLARE @MaterialConversionDate DATETIME
        DECLARE @DeliveryNetAmount  DECIMAL(18,4)

        DECLARE @InvoiceId2 INT
        DECLARE @CustomerName NVARCHAR(500)
        DECLARE @BankId INT
        DECLARE @CashId INT

        DECLARE @SubsidiaryInfoId INT
        DECLARE @ReferenceInfoId INT

        DECLARE @AIT DECIMAL(18,4) = 0
        DECLARE @Discount DECIMAL(18,4) = 0

        DECLARE @MyCursor2 CURSOR
        SET @MyCursor2 = CURSOR FAST_FORWARD
        FOR

        SELECT DISTINCT CI.CompanyId,FY.FinancialYearId,FORMAT(CPY.PaymentDate, 'dd-MMM-yyyy') UpdateDate,
        CPD.PaymentAmount,ISNULL(CPD.Discount, 0) AS Discount,ISNULL(CPD.AIT, 0) AS AIT,TBL.IntegrationTableId,CPY.CustPayId,
        CMS.CustomerName + ' (' + CMS.CustomerCode + ')' AS CustomerName,InvoiceNo,CPD.BankAccId,CPD.CashAccId,
        ISNULL(SB.SubsidiaryInfoId,0) SubsidiaryInfoId,ISNULL(RF.ReferenceInfoId,0) ReferenceInfoId  FROM tblCustPayDetail AS CPD
        LEFT JOIN dbo.tblCustomerPay AS CPY ON CPD.CustPayId = CPY.CustPayId
        LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = CPD.InvoiceId
        INNER JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = tblInvoice.ComUnitId
        INNER JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
        LEFT JOIN tblCustMaster AS CMS ON tblInvoice.CustomerMasterId = CMS.CustomerMasterId

        LEFT JOIN (SELECT S_Info_Id,SubsidiaryInfoId FROM CSTL_ACCDB..tblSubsidiaryInfo WHERE SubsidiaryTypeId = 1) AS SB ON tblInvoice.CustomerMasterId = SB.S_Info_Id

        LEFT JOIN CSTL_ACCDB..tblReferenceInfo AS RF ON RF.ReferenceTypeId = 1016 -- Invoice
        AND R_Info_Id = @InvoiceId

        INNER JOIN (SELECT CompanyId,FinancialYearId,StartDate,EndDate FROM CSTL_ACCDB..tblFinancialYear) AS FY ON CI.CompanyId = FY.CompanyId AND CPY.PaymentDate BETWEEN FY.StartDate AND FY.EndDate
        LEFT JOIN CSTL_ERPDB..tbl_AccIntegrationTable AS TBL ON LTRIM(RTRIM(UPPER(TBL.Table_Name))) = LTRIM(RTRIM(UPPER('tblCustomerPay')))
        LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherMaster AS V ON TableID = 2017 AND  CPY.CustPayId = V.CommonPK_ID
        WHERE

        (CPD.PaymentAmount + ISNULL(CPD.Discount, 0) + ISNULL(CPD.AIT, 0)) > 0
        AND  CPD.InvoiceId = @InvoiceId
        AND CPD.CustPayId = @CustPayId


        OPEN @MyCursor2
        FETCH NEXT FROM @MyCursor2
        INTO @BiusinessUnitId,@FinancialYearId,@ApprovalDate,@DeliveryNetAmount,@Discount,@AIT,@TableId,@InvoiceId2,
        @CustomerName,@InvoiceNo, @BankId, @CashId, @SubsidiaryInfoId,@ReferenceInfoId

        WHILE @@FETCH_STATUS = 0
        BEGIN

         DECLARE @yearText NVARCHAR(MAX)= SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(@ApprovalDate)), 3, 3)
         DECLARE @monthText NVARCHAR(MAX)=(CASE WHEN  LEN(MONTH(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),MONTH(@ApprovalDate)) END)
         DECLARE @dateText NVARCHAR(MAX)=(CASE WHEN  LEN(DAY(@ApprovalDate))=1 THEN '0'+CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) ELSE CONVERT(NVARCHAR(MAX),DAY(@ApprovalDate)) END)
         SELECT @VoucherCode = (@yearText+@monthText+@dateText)+CONVERT(NVARCHAR(MAX),CONVERT(INT,ISNULL(MAX(SUBSTRING(VoucherCode,11,6)),10000))+1)  FROM CSTL_ACCDB..tblDebitCreditVoucherMaster
         WHERE PostingDate = @ApprovalDate

            SET @VoucherType = CASE
                WHEN @BankId IS NOT NULL AND @BankId != 0 THEN 'CreditVoucherBank'
                WHEN @CashId != 0 AND @CashId IS NOT NULL THEN 'CreditVoucherCash'
            END

            IF(@VoucherType='DebitVoucherBank')
            BEGIN
                SET @VoucherCode='DVB-'+@VoucherCode
            END

            IF(@VoucherType='DebitVoucherCash')
            BEGIN
                SET @VoucherCode='DVC-'+@VoucherCode
            END

            IF(@VoucherType='CreditVoucherBank')
            BEGIN
                SET @VoucherCode='CVB-'+@VoucherCode
            END

            IF(@VoucherType='CreditVoucherCash')
            BEGIN
                SET @VoucherCode='CVC-'+@VoucherCode
            END

            IF(@VoucherType='FundTransfer')
            BEGIN
                SET @VoucherCode='FUT-'+@VoucherCode
            END

            IF(@VoucherType='Journal')
            BEGIN
                SET @VoucherCode='JVP-'+@VoucherCode
            END


            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
                    ( VoucherCode ,
                      CompanyInfoId ,
                      FinancialYearId ,
                      PostingDate ,
                      VoucherCategoryID ,
                      CurrencyId ,
                      CurrencyCovRate ,
                      Narration ,
                      PaytoIsSupplier ,
                      IsInvoice ,
                      TotalAmountBDT ,
                      TotalAmount ,
                      Remarks ,
                      EntryBy ,
                      EntryDate ,
                      VoucherType ,
                      ActionStatus,
                      IsCostCenter,
                      IsBuyerCostCenter,
                      IsOtherCostCenter,
                      ApprovedBy,
                      ApprovedDate,
                      CommonPK_ID,
                      TableID
                    )
            VALUES  (
                      @VoucherCode ,
                      @BiusinessUnitId ,
                      @FinancialYearId ,
                      @ApprovalDate ,
                      2 ,
                      @CurrencyId ,
                      @Conversionrate ,

                     'An amount of BDT ' + CONVERT(NVARCHAR(50), @DeliveryNetAmount) +
                        CASE
                            WHEN @Discount > 0 THEN ', including a discount of BDT ' + CONVERT(NVARCHAR(50), @Discount)
                            ELSE ''
                        END +
                        CASE
                            WHEN @AIT > 0 THEN ', and an AIT of BDT ' + CONVERT(NVARCHAR(50), @AIT)
                            ELSE ''
                        END +
                        ' has been received from customer "' + @CustomerName +
                        '" against Invoice No. "' + @InvoiceNo +
                        '" on ' + CONVERT(NVARCHAR(50), @ApprovalDate, 105) + '.' ,


                      0 ,
                      0 ,
                      @DeliveryNetAmount + @Discount + @AIT,
                      @DeliveryNetAmount + @Discount + @AIT,
                      'Auto Posting',
                      @ApproveBy ,
                      @ApprovalDate,
                      @VoucherType,
                      'Approved',
                      0,0,0,'Auto Process',
                      @ApprovalDate,@InvoiceId2,@TableId
                    )

            SET @VoucherMasterId = SCOPE_IDENTITY()

            -- Detail Information Posting

            DECLARE @DrGl INT = 0

            IF (@BankId IS NOT NULL AND @BankId != 0)
            BEGIN
                SELECT @DrGl = AccountId
                FROM CSTL_ACCDB..tblChartOfAccounts
                WHERE BankAccountId = @BankId
            END

            IF(@CashId IS NOT NULL AND @CashId != 0)
            BEGIN
                SELECT @DrGl = AccountId
                FROM CSTL_ACCDB..tblChartOfAccounts
                WHERE CashAccountId = @CashId
            END

            IF(ISNULL(@DeliveryNetAmount,0) > 0)
            BEGIN

                INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks ,RefInfoInd)
                VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @DeliveryNetAmount, 0.00, 'Customer Payment',@ReferenceInfoId);

            END

            IF(@Discount > 0)
            BEGIN

                DECLARE @DrGl1 INT = 0
                SELECT @DrGl1 = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1023 -- Sales Discount

                INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks,RefInfoInd)
                VALUES (@VoucherMasterId, @DrGl1, 'Debit', 0.00, 0.00, @Discount, 0.00, 'Customer Payment',@ReferenceInfoId);


            END


            IF(@AIT > 0)
            BEGIN

                DECLARE @DrGl2 INT = 0
                SELECT @DrGl2 = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 1024 -- Sales Discount

                INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks,RefInfoInd)
                VALUES (@VoucherMasterId, @DrGl2, 'Debit', 0.00, 0.00, @AIT, 0.00, 'Customer Payment',@ReferenceInfoId);


            END


            DECLARE @CrGl INT = 0
            SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 163 AND FlagId = 2  -- Accounts Receivable

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks, SubsidoryLedgerId, RefInfoInd)
            VALUES (@VoucherMasterId, @CrGl, 'Credit', 0, (@DeliveryNetAmount + @AIT + @Discount), 0, 0, 'Customer Payment', @SubsidiaryInfoId, 0);



        FETCH NEXT FROM @MyCursor2
        INTO  @BiusinessUnitId,@FinancialYearId,@ApprovalDate,@DeliveryNetAmount,@Discount,@AIT,@TableId,@InvoiceId2,@CustomerName,@InvoiceNo,
        @BankId, @CashId,@SubsidiaryInfoId,@ReferenceInfoId


        END
        CLOSE @MyCursor2
        DEALLOCATE @MyCursor2

    END TRY
    BEGIN CATCH
        IF CURSOR_STATUS('variable', '@MyCursor2') >= 0
        BEGIN
            CLOSE @MyCursor2;
            DEALLOCATE @MyCursor2;
        END
        DECLARE @ErrorMessage4 NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage4, 16, 1);
    END CATCH
END
GO
