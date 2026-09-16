/*
============================================================================
 Trade Material Transfer — P0/P1 remediation (approved plan)
 docs/audit/TradeMaterialTransfer_Remediation_Plan.md

 IMPORTANT — this script was rewritten against the ACTUAL LIVE definitions
 of both procedures, pulled via OBJECT_DEFINITION() from
 TOWSIF\MSSQLSERVER2019 / SalesRollDB_ZAS on 2026-09-09, after the initial
 draft (based on docs/database/stored-procedures-part2.md) was found to be
 STALE relative to the deployed procedure. The live
 sp_Process_TradeMaterialTransferFullProcess already contains a FIFO
 multi-batch cursor-based deduction loop (ORDER BY ExpDate ASC, DCStoreId
 ASC) and a weighted-average rate fallback chain that do not appear in the
 documentation at all — the docs describe an older/different version of
 this procedure. This script edits the CURRENT live body, not the
 documented one. See docs/audit/TradeMaterialTransfer_Remediation_Implementation.md
 §"Live definition drift" for the full comparison.

 Implements, in order:
   1. sp_Process_TradeMaterialTransferFullProcess
        - P0 (TMT-004/005): atomic approval-idempotency claim gate
        - P0 (TMT-008/009): atomic, guarded per-batch stock deduction inside
          the EXISTING FIFO cursor loop (order preserved, not invented)
        - Removes the unsafe "force-deduct remaining qty from an arbitrary
          row regardless of availability" fallback at the end of the loop
   2. spInsertTradeMaterialTransfer
        - P1 (TMT-006/013): scoped-range-locked TransferCode generation
          (format-preserving: TMT-<YYMM><seq>, monthly bucket unchanged)
        - This procedure's live definition matched the documentation
          exactly (verified via OBJECT_DEFINITION), so no drift concern here.

 NOT changed by this script (explicitly out of scope / preserved as-is):
   - GL voucher posting logic and its existing NOT EXISTS idempotency guard
   - The FIFO batch-selection ORDER BY (ExpDate ASC, DCStoreId ASC) —
     ALREADY ESTABLISHED in the live code, so per instructions this is
     preserved verbatim, not replaced with an invented rule. The earlier
     "BUSINESS RULE REQUIRED — STOCK ROW SELECTION/FIFO" stop-point from
     the plan/first draft is RESOLVED by this discovery: an order already
     exists and is preserved.
   - The weighted-average rate fallback chain (warehouse -> company-wide ->
     tblUnitPrice cost price)
   - spUpdateTradeMaterialTransfer, spDeleteTradeMaterialTransfer
   - Reject-flow behavior (open business question, untouched)
   - No unique index/constraint is created on TransferCode by this script.
     Live check on 2026-09-09 found ZERO existing duplicate TransferCode
     values (precondition satisfied), but the remediation plan explicitly
     deferred creating this constraint rather than approving it outright —
     so it is documented, not created, here.

 Deployment: run against SalesRollDB_ZAS (the "SSIDB" connection; verified
 present as TOWSIF\MSSQLSERVER2019 / SalesRollDB_ZAS in this environment,
 along with CSTL_ERPDB and CSTL_ACCDB on the SAME instance — confirmed by
 querying sys.databases, so the cross-database writes inside procedure #1
 stay local, not MSDTC-promoted, on this instance). Idempotent to re-run
 (ALTER PROCEDURE).
============================================================================
*/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================================
-- 1. sp_Process_TradeMaterialTransferFullProcess  (edited from the LIVE body)
-- ============================================================================
ALTER PROCEDURE dbo.sp_Process_TradeMaterialTransferFullProcess
    @TradeMaterialTransferId INT,
    @ApprovedBy NVARCHAR(100) = 'System'
AS
BEGIN
    SET NOCOUNT OFF;

    DECLARE @FromCompanyId INT, @FromLocationId INT, @FromProductId INT, @FromProductCode NVARCHAR(100), @FromProductName NVARCHAR(300);
    DECLARE @ToCompanyId INT, @ToLocationId INT, @ToItemDescriptionId INT, @ToItemDescriptionCode NVARCHAR(100), @ToItemDescription NVARCHAR(300);
    DECLARE @TransferQty DECIMAL(18,3);
    DECLARE @ReferenceNo NVARCHAR(100);

    SELECT
        @FromCompanyId = FromCompanyId,
        @FromLocationId = FromLocationId,
        @FromProductId = FromProductId,
        @FromProductCode = FromProductCode,
        @FromProductName = FromProductName,
        @ToCompanyId = ToCompanyId,
        @ToLocationId = ToLocationId,
        @ToItemDescriptionId = ToItemDescriptionId,
        @ToItemDescriptionCode = ToItemDescriptionCode,
        @ToItemDescription = ToItemDescription,
        @TransferQty = TransferQty,
        @ReferenceNo = 'TMT-' + CAST(TradeMaterialTransferId AS NVARCHAR(30))
    FROM SalesRollDB_ZAS..tblTradeMaterialTransfer
    WHERE TradeMaterialTransferId = @TradeMaterialTransferId;

    IF @TradeMaterialTransferId IS NULL OR @TransferQty IS NULL OR @TransferQty <= 0
    BEGIN
        RAISERROR('Invalid Trade Material Transfer ID or Transfer Quantity.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        -- =====================================================================
        -- 1. ATOMIC IDEMPOTENCY CLAIM (TMT-004/005/009 remediation)
        --    Conditional UPDATE + @@ROWCOUNT check replaces the prior
        --    unconditional status UPDATE. Under SQL Server's normal row-level
        --    locking (no SERIALIZABLE/table lock needed), a concurrent second
        --    call for the same TradeMaterialTransferId blocks on this row,
        --    then re-evaluates the WHERE clause against the committed/rolled-
        --    back result and finds 0 rows to update — so it is guaranteed to
        --    exit here without ever reaching the weighted-average, stock, or
        --    GL processing below.
        --    Live evidence found on 2026-09-09 that this gap is not
        --    theoretical: TradeMaterialTransferId 1007 already has two
        --    source-side tbl_INV_WeightedAvg rows (-500 qty each, ~6 hours
        --    apart, same day) — see the implementation report.
        -- =====================================================================
        UPDATE SalesRollDB_ZAS..tblTradeMaterialTransfer
        SET ApprovalStatus = 'Approved',
            ApprovedBy = @ApprovedBy,
            ApprovedDate = GETDATE(),
            UpdateBy = @ApprovedBy,
            UpdateDate = GETDATE()
        WHERE TradeMaterialTransferId = @TradeMaterialTransferId
          AND ISNULL(ApprovalStatus, '') <> 'Approved';

        IF @@ROWCOUNT = 0
        BEGIN
            -- Already approved by a prior call, or claimed a moment ago by a
            -- concurrent call. Nothing else has been written in this
            -- invocation, so there is nothing to roll back — commit the
            -- (no-op) transaction and stop before touching weighted-average,
            -- stock, or GL processing.
            COMMIT TRANSACTION;
            RETURN;
        END

        -- 2. Source Weighted Average Rate & Calculation (UNCHANGED)
        DECLARE @From_OP_Qty DECIMAL(18,3) = 0, @From_OP_Rate DECIMAL(18,4) = 0, @From_OP_Amt DECIMAL(18,4) = 0;

        SELECT TOP 1
            @From_OP_Qty = ISNULL(Close_Qty, 0),
            @From_OP_Rate = ISNULL(Avg_Rate, 0),
            @From_OP_Amt = ISNULL(Close_Amt, 0)
        FROM SalesRollDB_ZAS..tbl_INV_WeightedAvg
        WHERE Company_Id = @FromCompanyId AND Warehouse_Id = @FromLocationId AND Item_Id = @FromProductId
        ORDER BY Weighted_Avg_Id DESC;

        -- Rate fallback if not found at warehouse
        IF @From_OP_Rate IS NULL OR @From_OP_Rate = 0
        BEGIN
            SELECT TOP 1 @From_OP_Rate = ISNULL(Avg_Rate, 0)
            FROM SalesRollDB_ZAS..tbl_INV_WeightedAvg
            WHERE Company_Id = @FromCompanyId AND Item_Id = @FromProductId AND Avg_Rate > 0
            ORDER BY Weighted_Avg_Id DESC;
        END

        IF @From_OP_Rate IS NULL OR @From_OP_Rate = 0
        BEGIN
            SELECT TOP 1 @From_OP_Rate = ISNULL(CostPrice, 0)
            FROM SalesRollDB_ZAS..tblUnitPrice
            WHERE ProductId = @FromProductId AND IsActive = 1;
        END

        DECLARE @From_Curr_Qty DECIMAL(18,3) = -@TransferQty;
        DECLARE @From_Curr_Rate DECIMAL(18,4) = @From_OP_Rate;
        DECLARE @From_Curr_Amt DECIMAL(18,4) = @From_Curr_Qty * @From_Curr_Rate;
        DECLARE @From_Close_Qty DECIMAL(18,3) = @From_OP_Qty + @From_Curr_Qty;
        DECLARE @From_Close_Amt DECIMAL(18,4) = @From_Close_Qty * @From_OP_Rate;

        INSERT INTO SalesRollDB_ZAS..tbl_INV_WeightedAvg
        (
            Company_Id, Warehouse_Id, Mst_Trans_Id, Trans_Id, Item_Id, Item_Code, Ref_No,
            Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type,
            OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt
        )
        VALUES
        (
            @FromCompanyId, @FromLocationId, @TradeMaterialTransferId, @TradeMaterialTransferId, @FromProductId, @FromProductCode, @ReferenceNo,
            GETDATE(), GETDATE(), GETDATE(), 1010, 1, 'FG',
            @From_OP_Qty, @From_OP_Rate, @From_OP_Amt, @From_Curr_Qty, @From_Curr_Rate, @From_Curr_Amt, @From_Close_Qty, @From_OP_Rate, @From_Close_Amt
        );

        DECLARE @NewFromWeightedAvgId INT = SCOPE_IDENTITY();

        -- 3. Destination Weighted Average Calculation (UNCHANGED)
        DECLARE @To_OP_Qty DECIMAL(18,3) = 0, @To_OP_Rate DECIMAL(18,4) = 0, @To_OP_Amt DECIMAL(18,4) = 0;

        SELECT TOP 1
            @To_OP_Qty = ISNULL(Close_Qty, 0),
            @To_OP_Rate = ISNULL(Avg_Rate, 0),
            @To_OP_Amt = ISNULL(Close_Amt, 0)
        FROM CSTL_ERPDB..tbl_INV_WeightedAvg
        WHERE Company_Id = @ToCompanyId AND Warehouse_Id = @ToLocationId AND Item_Id = @ToItemDescriptionId
        ORDER BY Weighted_Avg_Id DESC;

        DECLARE @To_Curr_Qty DECIMAL(18,3) = @TransferQty;
        DECLARE @To_Curr_Rate DECIMAL(18,4) = @From_OP_Rate; -- Receives at From side WAC rate
        DECLARE @To_Curr_Amt DECIMAL(18,4) = @To_Curr_Qty * @To_Curr_Rate;
        DECLARE @To_Close_Qty DECIMAL(18,3) = @To_OP_Qty + @To_Curr_Qty;
        DECLARE @To_Close_Amt DECIMAL(18,4) = @To_OP_Amt + @To_Curr_Amt;
        DECLARE @To_New_Avg_Rate DECIMAL(18,4) = 0;

        IF @To_Close_Qty > 0
            SET @To_New_Avg_Rate = @To_Close_Amt / @To_Close_Qty;
        ELSE
            SET @To_New_Avg_Rate = @To_Curr_Rate;

        INSERT INTO CSTL_ERPDB..tbl_INV_WeightedAvg
        (
            Company_Id, Warehouse_Id, Mst_Trans_Id, Trans_Id, Item_Id, Item_Code, Ref_No,
            Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type,
            OP_Qty, OP_Rate, OP_Amt, Curr_Qty, Curr_Rate, Curr_Amt, Close_Qty, Avg_Rate, Close_Amt
        )
        VALUES
        (
            @ToCompanyId, @ToLocationId, @TradeMaterialTransferId, @TradeMaterialTransferId, @ToItemDescriptionId, @ToItemDescriptionCode, @ReferenceNo,
            GETDATE(), GETDATE(), GETDATE(), 2010, 2, 'RM',
            @To_OP_Qty, @To_OP_Rate, @To_OP_Amt, @To_Curr_Qty, @To_Curr_Rate, @To_Curr_Amt, @To_Close_Qty, @To_New_Avg_Rate, @To_Close_Amt
        );

        DECLARE @NewToWeightedAvgId INT = SCOPE_IDENTITY();

        -- 4. FIFO Multi-Batch Stock Deduction from tblDCStore & Batch Tracking
        --    ORDER BY ExpDate ASC, DCStoreId ASC is the EXISTING, established
        --    selection order in the live procedure — PRESERVED UNCHANGED.
        --    Per the implementation instructions, this order is not replaced
        --    or second-guessed; only the per-row write is made atomic below.
        DECLARE @RemainingQty DECIMAL(18,3) = @TransferQty;
        DECLARE @CurDCStoreId INT, @CurBatchNo NVARCHAR(100), @CurMfgDate DATETIME, @CurExpDate DATETIME, @CurStockQty DECIMAL(18,3), @DeductQty DECIMAL(18,3);

        DECLARE dc_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT DCStoreId, ISNULL(BatchNo, 'DefaultBatch'), ISNULL(MfgDate, GETDATE()), ISNULL(ExpDate, DATEADD(year, 2, GETDATE())), StockQty
        FROM SalesRollDB_ZAS..tblDCStore
        WHERE ProductCode = @FromProductCode AND ComUnitId = @FromLocationId AND StockQty > 0
        ORDER BY ExpDate ASC, DCStoreId ASC;

        OPEN dc_cursor;
        FETCH NEXT FROM dc_cursor INTO @CurDCStoreId, @CurBatchNo, @CurMfgDate, @CurExpDate, @CurStockQty;

        WHILE @@FETCH_STATUS = 0 AND @RemainingQty > 0
        BEGIN
            IF @CurStockQty >= @RemainingQty
                SET @DeductQty = @RemainingQty;
            ELSE
                SET @DeductQty = @CurStockQty;

            -- =================================================================
            -- TMT-008/009 remediation: atomic, guarded per-batch deduction.
            -- @CurStockQty was read moments earlier by the cursor's own
            -- SELECT (not locked at read time) — this re-checks the quantity
            -- condition at write time and confirms the write actually
            -- happened via @@ROWCOUNT, closing the gap between "read" and
            -- "write" for this specific row under concurrent approvals that
            -- target the same batch.
            -- =================================================================
            UPDATE SalesRollDB_ZAS..tblDCStore
            SET StockQty = StockQty - @DeductQty
            WHERE DCStoreId = @CurDCStoreId
              AND StockQty >= @DeductQty;

            IF @@ROWCOUNT = 0
            BEGIN
                CLOSE dc_cursor;
                DEALLOCATE dc_cursor;
                RAISERROR('Trade Material Transfer %d: source stock for DCStoreId %d changed concurrently and no longer has sufficient quantity for this batch.',
                    16, 1, @TradeMaterialTransferId, @CurDCStoreId);
            END

            -- Insert into From Company BatchWiseStock
            INSERT INTO SalesRollDB_ZAS..tbl_INV_BatchWiseStock
            (Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty, Remarks, DCStoreId)
            VALUES
            (@NewFromWeightedAvgId, @CurBatchNo, @CurMfgDate, @CurExpDate, -@DeductQty, 'Trade Transfer FG Issue', @CurDCStoreId);

            -- Insert into To Company BatchWiseStock
            INSERT INTO CSTL_ERPDB..tbl_INV_BatchWiseStock
            (Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty, Remarks)
            VALUES
            (@NewToWeightedAvgId, @CurBatchNo, @CurMfgDate, @CurExpDate, @DeductQty, 'Trade Transfer RM Receive');

            SET @RemainingQty = @RemainingQty - @DeductQty;

            FETCH NEXT FROM dc_cursor INTO @CurDCStoreId, @CurBatchNo, @CurMfgDate, @CurExpDate, @CurStockQty;
        END

        CLOSE dc_cursor;
        DEALLOCATE dc_cursor;

        -- =====================================================================
        -- TMT-008 remediation: the previous fallback here picked one more
        -- row (ORDER BY DCStoreId DESC, no relation to the FIFO order above)
        -- and force-deducted the remaining quantity from it REGARDLESS of
        -- whether that row had enough — or any — stock, which could and did
        -- drive StockQty negative. That fallback is REMOVED. If the FIFO
        -- loop above could not source the full requested quantity from rows
        -- that actually had stock, this is now a hard failure — full
        -- rollback via the existing CATCH block below, including every
        -- batch already deducted earlier in this same loop and the
        -- idempotency claim from step 1.
        -- =====================================================================
        IF @RemainingQty > 0
        BEGIN
            DECLARE @RemainingQtyStr NVARCHAR(30) = CONVERT(NVARCHAR(30), @RemainingQty);
            RAISERROR('Trade Material Transfer %d: insufficient aggregate source stock for ProductCode %s at ComUnitId %d (short by %s).',
                16, 1, @TradeMaterialTransferId, @FromProductCode, @FromLocationId, @RemainingQtyStr);
        END

        -- =====================================================================
        -- 5. JOURNAL POSTING — UNCHANGED. Existing NOT EXISTS idempotency
        --    guard (CommonPK_ID = @TradeMaterialTransferId AND TableID = 2026)
        --    is preserved verbatim: it was already correct and already
        --    aligned with the same transfer identity used by the new claim
        --    gate in step 1.
        -- =====================================================================
        DECLARE @TransferValue DECIMAL(18, 4) = @TransferQty * @From_OP_Rate;

        IF @TransferValue > 0 AND NOT EXISTS (
            SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster
            WHERE CommonPK_ID = @TradeMaterialTransferId AND TableID = 2026
        )
        BEGIN
            DECLARE @FromDebitGL INT, @FromCreditGL INT;
            DECLARE @ToDebitGL INT, @ToCreditGL INT;

            -- From Company (FG Issue)
            -- Debit: Inter-company party account matching destination company
            SELECT TOP 1 @FromDebitGL = AccountId
            FROM CSTL_ACCDB..tblChartOfAccounts
            WHERE AccountCode LIKE '01021091001%'
              AND (AccountName = (SELECT CompanyName FROM CSTL_ERPDB..tblCompanyInfo WHERE CompanyId = @ToCompanyId)
                OR AccountName LIKE (SELECT CompanyName + '%' FROM CSTL_ERPDB..tblCompanyInfo WHERE CompanyId = @ToCompanyId));

            IF @FromDebitGL IS NULL OR @FromDebitGL = 0
            BEGIN
                SELECT TOP 1 @FromDebitGL = GLId
                FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity
                WHERE PageId = 500090 AND FlagId = 1012;
            END

            IF @FromDebitGL IS NULL OR @FromDebitGL = 0 SET @FromDebitGL = 4154;

            -- Credit: Finished Goods Inventory (FG)
            SELECT TOP 1 @FromCreditGL = GLId
            FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity
            WHERE PageId = 500090 AND FlagId = 1014;

            IF @FromCreditGL IS NULL OR @FromCreditGL = 0 SET @FromCreditGL = 4156;

            -- To Company (RM Receive)
            -- Debit: Raw Material Inventory (RM)
            SELECT TOP 1 @ToDebitGL = M.COAId
            FROM CSTL_ERPDB..Table_Integration_MappingByLinkAccounts M
            INNER JOIN CSTL_ERPDB..tblItemCategory C ON M.CategoryId = C.ItemCategoryId
            INNER JOIN CSTL_ERPDB..tblItemName NM ON C.ItemCategoryId = NM.ItemCategoryId
            INNER JOIN CSTL_ERPDB..tblItemDescription ID ON NM.ItemNameId = ID.ItemNameId
            WHERE ID.ItemDescriptionId = @ToItemDescriptionId AND M.LinkAccountId = 1;

            IF @ToDebitGL IS NULL OR @ToDebitGL = 0 SET @ToDebitGL = 4159;

            -- Credit: Inter Company Material Loans
            SELECT TOP 1 @ToCreditGL = GLId
            FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity
            WHERE PageId = 500090 AND FlagId = 1013;

            IF @ToCreditGL IS NULL OR @ToCreditGL = 0 SET @ToCreditGL = 4154;

            -- Retrieve Financial Years
            DECLARE @FromFinancialYearId INT, @ToFinancialYearId INT;
            SELECT TOP 1 @FromFinancialYearId = FinancialYearId FROM CSTL_ACCDB..tblFinancialYear WHERE CompanyId = @FromCompanyId AND CONVERT(DATE, GETDATE()) BETWEEN StartDate AND EndDate;
            SELECT TOP 1 @ToFinancialYearId = FinancialYearId FROM CSTL_ACCDB..tblFinancialYear WHERE CompanyId = @ToCompanyId AND CONVERT(DATE, GETDATE()) BETWEEN StartDate AND EndDate;

            -- Generate voucher codes sequentially
            DECLARE @FromVoucherCode NVARCHAR(100), @ToVoucherCode NVARCHAR(100);
            DECLARE @yearText NVARCHAR(10), @monthText NVARCHAR(10), @dateText NVARCHAR(10);
            SET @yearText = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(GETDATE())), 3, 2);
            SET @monthText = RIGHT('0' + CONVERT(NVARCHAR(10), MONTH(GETDATE())), 2);
            SET @dateText = RIGHT('0' + CONVERT(NVARCHAR(10), DAY(GETDATE())), 2);

            DECLARE @MaxSerial INT = 10000;
            SELECT @MaxSerial = CONVERT(INT, ISNULL(MAX(SUBSTRING(VoucherCode, 11, 5)), 10000))
            FROM CSTL_ACCDB..tblDebitCreditVoucherMaster
            WHERE PostingDate = CONVERT(DATE, GETDATE()) AND VoucherCode LIKE 'JVP-%';

            SET @FromVoucherCode = 'JVP-' + (@yearText+@monthText+@dateText) + CONVERT(NVARCHAR(30), @MaxSerial + 1);
            SET @ToVoucherCode = 'JVP-' + (@yearText+@monthText+@dateText) + CONVERT(NVARCHAR(30), @MaxSerial + 2);

            -- Insert From Company Voucher Master
            DECLARE @FromVoucherMasterId INT;
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
            (
                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID,
                CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,
                TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate,
                VoucherType, ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter,
                ApprovedBy, ApprovedDate, CommonPK_ID, TableID
            )
            VALUES
            (
                @FromVoucherCode, @FromCompanyId, @FromFinancialYearId, CONVERT(DATE, GETDATE()), 2, -- 2 = Journal
                1, 1.0, 'Trade Material Transfer Issue. Ref: ' + @ReferenceNo + ', Product: ' + @FromProductName + ', Qty: ' + CAST(@TransferQty AS NVARCHAR(50)) + ', Value: ' + CONVERT(NVARCHAR(50), @TransferValue), 0, 0,
                @TransferValue, @TransferValue, 'Trade Material Transfer FG Issue', @ApprovedBy, GETDATE(),
                'Journal', 'Approved', 0, 0, 0,
                @ApprovedBy, GETDATE(), @TradeMaterialTransferId, 2026
            );
            SET @FromVoucherMasterId = SCOPE_IDENTITY();

            -- Insert From Company Details
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@FromVoucherMasterId, @FromDebitGL, 'Debit', 0, 0, @TransferValue, 0, 'Intercompany Loan (Receivable)');

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@FromVoucherMasterId, @FromCreditGL, 'Credit', 0, 0, 0, @TransferValue, 'Finished Goods Inventory Stock Out');

            -- Insert To Company Voucher Master
            DECLARE @ToVoucherMasterId INT;
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
            (
                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID,
                CurrencyId, CurrencyCovRate, Narration, PaytoIsSupplier, IsInvoice,
                TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate,
                VoucherType, ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter,
                ApprovedBy, ApprovedDate, CommonPK_ID, TableID
            )
            VALUES
            (
                @ToVoucherCode, @ToCompanyId, @ToFinancialYearId, CONVERT(DATE, GETDATE()), 2,
                1, 1.0, 'Trade Material Transfer Receive. Ref: ' + @ReferenceNo + ', Product: ' + @ToItemDescription + ', Qty: ' + CAST(@TransferQty AS NVARCHAR(50)) + ', Value: ' + CONVERT(NVARCHAR(50), @TransferValue), 0, 0,
                @TransferValue, @TransferValue, 'Trade Material Transfer RM Receive', @ApprovedBy, GETDATE(),
                'Journal', 'Approved', 0, 0, 0,
                @ApprovedBy, GETDATE(), @TradeMaterialTransferId, 2026
            );
            SET @ToVoucherMasterId = SCOPE_IDENTITY();

            -- Insert To Company Details
            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@ToVoucherMasterId, @ToDebitGL, 'Debit', 0, 0, @TransferValue, 0, 'Raw Material Inventory Stock In');

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail
            (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES
            (@ToVoucherMasterId, @ToCreditGL, 'Credit', 0, 0, 0, @TransferValue, 'Intercompany Loan (Payable)');
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO


-- ============================================================================
-- 2. spInsertTradeMaterialTransfer
--    Live definition verified IDENTICAL to docs/database/stored-procedures-part1.md
--    on 2026-09-09 — no drift here.
--    TMT-006/013 remediation: TransferCode generation race fix.
--    Format preserved exactly: 'TMT-' + YYMM (of @EntryDate) + running 4-digit
--    sequence starting at 1001, scoped per-month via the existing LIKE prefix
--    filter (no change to the monthly-bucket behavior).
-- ============================================================================
ALTER PROCEDURE dbo.spInsertTradeMaterialTransfer
    @FromCompanyId INT,
    @FromLocationId INT,
    @FromProductId INT,
    @FromProductCode NVARCHAR(100),
    @FromProductName NVARCHAR(300),
    @FromCurrentStockQty DECIMAL(18,3),
    @ToCompanyId INT,
    @ToLocationId INT,
    @ToItemDescriptionId INT,
    @ToItemDescriptionCode NVARCHAR(100),
    @ToItemDescription NVARCHAR(300),
    @ToCurrentStockQty DECIMAL(18,3),
    @TransferQty DECIMAL(18,3),
    @Remarks NVARCHAR(500),
    @EntryBy NVARCHAR(100),
    @EntryDate DATETIME,
    @TradeMaterialTransferId INT OUTPUT
AS
BEGIN
    -- DO NOT set SET NOCOUNT ON to allow row count messages back to Web Application
    DECLARE @Prefix NVARCHAR(10);
    DECLARE @NextSeq INT;
    DECLARE @GeneratedCode NVARCHAR(50);

    SET @Prefix = 'TMT-' + SUBSTRING(CONVERT(NVARCHAR(8), @EntryDate, 112), 3, 4);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- =====================================================================
        -- TMT-006 remediation: scoped range lock on rows matching this
        -- month's prefix only (typically a handful of rows), held only for
        -- the duration of this one INSERT. UPDLOCK avoids the classic
        -- shared-lock conversion deadlock when two sessions both want to
        -- read-then-write the same range; HOLDLOCK (=SERIALIZABLE for this
        -- one range scan) keeps the lock until COMMIT/ROLLBACK, so a second
        -- concurrent caller blocks here and recomputes @NextSeq against the
        -- first caller's just-committed row rather than racing it.
        -- No table-wide or workflow-wide lock is introduced.
        -- =====================================================================
        SELECT @NextSeq = ISNULL(MAX(CAST(RIGHT(TransferCode, 4) AS INT)), 1000) + 1
        FROM dbo.tblTradeMaterialTransfer WITH (UPDLOCK, HOLDLOCK)
        WHERE TransferCode LIKE @Prefix + '%';

        SET @GeneratedCode = @Prefix + CAST(@NextSeq AS NVARCHAR(4));

        INSERT INTO dbo.tblTradeMaterialTransfer
        (
            TransferCode,
            FromCompanyId,
            FromLocationId,
            FromProductId,
            FromProductCode,
            FromProductName,
            FromCurrentStockQty,
            ToCompanyId,
            ToLocationId,
            ToItemDescriptionId,
            ToItemDescriptionCode,
            ToItemDescription,
            ToCurrentStockQty,
            TransferQty,
            Remarks,
            EntryBy,
            EntryDate,
            ApprovalStatus -- Auto-set to 'Posted' on Insert
        )
        VALUES
        (
            @GeneratedCode,
            @FromCompanyId,
            @FromLocationId,
            @FromProductId,
            @FromProductCode,
            @FromProductName,
            @FromCurrentStockQty,
            @ToCompanyId,
            @ToLocationId,
            @ToItemDescriptionId,
            @ToItemDescriptionCode,
            @ToItemDescription,
            @ToCurrentStockQty,
            @TransferQty,
            @Remarks,
            @EntryBy,
            @EntryDate,
            'Posted'
        );

        SET @TradeMaterialTransferId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO
