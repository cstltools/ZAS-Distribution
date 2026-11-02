-- COGS voucher must equal the inventory ledger stock-out it mirrors.
-- Before: sp_Sales_DeliveryCOGSPosting priced lines at tblDCStore.UnitPrice_CostPrice and, when
-- that was NULL, at tblUnitPrice.UnitPrice (the SELLING price). sp_Process_WeightedAverageSalesConfirmation
-- (step 1 of sp_Sales_ExecuteDeliveryPipeline) already books the same lines into tbl_INV_WeightedAvg at
-- Avg_Rate. Oct-2026: 244/277 delivered invoices had GL <> ledger.
-- After: TotalCost = SUM(-Curr_Amt) of this invoice's Trans_Type_Id=4 ledger rows. One source of truth.
-- Run as-is in SSMS or: sqlcmd -S <server> -d SalesRollDB_ZAS -i <this file>
USE SalesRollDB_ZAS;
GO
SET ANSI_NULLS ON; SET QUOTED_IDENTIFIER ON;
GO
ALTER PROCEDURE [dbo].[sp_Sales_DeliveryCOGSPosting]
    @InvoiceId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT, @FinancialYearId INT, @InvoiceNo NVARCHAR(100), @DeliveryDate DATETIME;
    DECLARE @TotalCost DECIMAL(18,4) = 0, @CustomerName NVARCHAR(200);

    SELECT
        @CompanyId = CU.CompanyId,
        @InvoiceNo = I.DelivaryInvoiceNo,
        @DeliveryDate = ISNULL(I.UpdateDate, I.InvoiceDate),
        @CustomerName = ISNULL(CM.CustomerName, 'Customer')
    FROM dbo.tblInvoice I WITH (NOLOCK)
    INNER JOIN dbo.tblCompanyUnit CU WITH (NOLOCK) ON I.ComUnitId = CU.ComUnitId
    LEFT JOIN dbo.tblCustMaster CM WITH (NOLOCK) ON I.CustomerMasterId = CM.CustomerMasterId
    WHERE I.InvoiceId = @InvoiceId;

    SELECT TOP 1 @FinancialYearId = FinancialYearId
    FROM CSTL_ACCDB..tblFinancialYear WITH (NOLOCK)
    WHERE CompanyId = @CompanyId AND @DeliveryDate BETWEEN StartDate AND EndDate;

    -- COGS = what the inventory ledger took out for this invoice (written by
    -- sp_Process_WeightedAverageSalesConfirmation at Avg_Rate). Never tblDCStore cost, never selling price.
    SELECT @TotalCost = ISNULL(SUM(-W.Curr_Amt), 0)
    FROM dbo.tbl_INV_WeightedAvg W WITH (NOLOCK)
    WHERE W.Trans_Type_Id = 4 AND W.Mst_Trans_Id = @InvoiceId AND W.Curr_Qty < 0;

    IF NOT EXISTS (SELECT 1 FROM dbo.tbl_INV_WeightedAvg WITH (NOLOCK) WHERE Trans_Type_Id = 4 AND Mst_Trans_Id = @InvoiceId AND Curr_Qty < 0)
    BEGIN
        -- Step 1 of the pipeline did not run: fail loudly instead of posting 0 / a made-up rate.
        RAISERROR('sp_Sales_DeliveryCOGSPosting: no tbl_INV_WeightedAvg stock-out rows for InvoiceId %d; run sp_Process_WeightedAverageSalesConfirmation first.', 16, 1, @InvoiceId);
        RETURN;
    END;

    IF (@TotalCost > 0)
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK)
            WHERE CommonPK_ID = @InvoiceId AND TableID = 20 AND ActionStatus <> 'Cancelled'
              AND Narration LIKE '%Cost of Goods Sold (COGS)%'
        )
        BEGIN
            DECLARE @yearText NVARCHAR(10) = SUBSTRING(CONVERT(NVARCHAR(10), YEAR(@DeliveryDate)), 3, 2);
            DECLARE @monthText NVARCHAR(10) = RIGHT('0' + CAST(MONTH(@DeliveryDate) AS NVARCHAR(2)), 2);
            DECLARE @dateText NVARCHAR(10) = RIGHT('0' + CAST(DAY(@DeliveryDate) AS NVARCHAR(2)), 2);

            DECLARE @MaxNum INT = 10000;
            SELECT @MaxNum = ISNULL(MAX(VoucherMasterId), 10000) FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WITH (NOLOCK);

            DECLARE @VoucherCode NVARCHAR(50) = 'JVP-' + @yearText + @monthText + @dateText + CAST((@MaxNum % 89999) + 10001 AS NVARCHAR(10));

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherMaster
            (
                VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID, CurrencyId, CurrencyCovRate,
                Narration, PaytoIsSupplier, IsInvoice, TotalAmountBDT, TotalAmount, Remarks, EntryBy, EntryDate, VoucherType,
                ActionStatus, IsCostCenter, IsBuyerCostCenter, IsOtherCostCenter, ApprovedBy, ApprovedDate, CommonPK_ID, TableID, IsInventoryValuation
            )
            VALUES
            (
                @VoucherCode, @CompanyId, @FinancialYearId, @DeliveryDate, 2, 1, 1.00,
                'Cost of Goods Sold (COGS) for Delivery Invoice: ' + @InvoiceNo + ' to ' + @CustomerName + '.',
                0, 0, @TotalCost, @TotalCost, 'Auto COGS Posting', 'System', @DeliveryDate, 'Journal',
                'Approved', 0, 0, 0, 'Auto Process', @DeliveryDate, @InvoiceId, 20, 1
            );

            DECLARE @VoucherMasterId INT = SCOPE_IDENTITY();

            -- DR: Cost of Goods Sold (4162)
            DECLARE @DrGl INT = 0;
            SELECT @DrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1021 AND IsInventory = 1;
            IF ISNULL(@DrGl, 0) = 0 SET @DrGl = 4162;

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmount, CreditAmount, DebitAmountBDT, CreditAmountBDT, Remarks)
            VALUES (@VoucherMasterId, @DrGl, 'Debit', 0.00, 0.00, @TotalCost, 0.00, 'Cost of Goods Sold COGS');

            -- CR: Finished Goods Inventory (4156)
            DECLARE @CrGl INT = 0;
            SELECT @CrGl = GLId FROM CSTL_ERPDB..Table_Integration_AccountsConnectivity WHERE PageId = 20 AND FlagId = 1014 AND IsInventory = 1;
            IF ISNULL(@CrGl, 0) = 0 SET @CrGl = 4156;

            INSERT INTO CSTL_ACCDB..tblDebitCreditVoucherDetail (VoucherMasterId, AccountId, DebitorCredit, DebitAmountBDT, CreditAmountBDT, DebitAmount, CreditAmount, Remarks)
            VALUES (@VoucherMasterId, @CrGl, 'Credit', 0.00, @TotalCost, 0.00, 0.00, 'Finished Goods Inventory Stock Out');
        END;
    END;
END;
GO
