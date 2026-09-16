-- Loads the pending-order grid for InvoiceCreationByOrder.aspx.
-- Replaces the dynamic string-concatenated WHERE clause built in
-- InvoiceCreationByOrder.aspx.cs (GenerateParameter) with parameterized filters.
-- All filter parameters are optional; pass NULL to skip a filter.

IF OBJECT_ID('dbo.sp_LoadOrderForOrderCreation', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_LoadOrderForOrderCreation
GO

CREATE PROCEDURE dbo.sp_LoadOrderForOrderCreation
    @CompanyId        NVARCHAR(50) = NULL,
    @ComUnitId        NVARCHAR(50) = NULL,
    @AreaId           NVARCHAR(50) = NULL,
    @TerritoryId      NVARCHAR(50) = NULL,
    @MarketId         NVARCHAR(50) = NULL,
    @SubmissionDate   NVARCHAR(50) = NULL,
    @CustomerMasterId NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON

    SELECT ODR.ComUnitId, ManufacId, OrderId, ODR.CustomerMasterId, UNT.ComUnitName, OrderCode, SubmissionDate, ODR.CustomerCode,
        ODR.CustomerName, MarketName, EGI.EmpMasterCode + ' : ' + EGI.EmpName AS MIOCode, ISNULL(GrossValue,0) GrossValue, ISNULL(Due,0) Due,
        ISNULL(GrossValue,0) + ISNULL(Due,0) AS ReceiveAble, ODR.Remarks,
        ISNULL(LimitAmount,0) LimitAmount, ISNULL(DayLimit,0) DayLimit, ISNULL(LD.InvoiceNo,'N/A') InvoiceNo, ISNULL(LD.InvoiceDate,GETDATE()) InvoiceDate,
        ISNULL(DATEDIFF(day, InvoiceDate, GETDATE()), 0) AS DS,
        CASE
            WHEN TermOfPayment = 'Credit' THEN
                CASE
                    WHEN ISNULL(LimitAmount, 0) = 0 THEN 0
                    WHEN (ISNULL(GrossValue, 0) + ISNULL(Due, 0) <= ISNULL(LimitAmount, 0)) AND (ISNULL(DATEDIFF(day, InvoiceDate, GETDATE()), 0) <= ISNULL(DayLimit, 0)) THEN 1
                    ELSE 0
                END
            ELSE 1
        END AS Permission
    FROM tblOrder AS ODR
        LEFT JOIN tblCompanyUnit AS UNT ON ODR.ComUnitId = UNT.ComUnitId
        INNER JOIN tblCustMaster AS C ON ODR.CustomerMasterId = C.CustomerMasterId
        LEFT JOIN tblMarket AS MKT ON MKT.MarketId = ODR.MarketId
        LEFT JOIN tblMIOInfo AS MIO ON ODR.MIOId = MIO.MIOId
        LEFT JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId

        LEFT JOIN (SELECT CustomerMasterId, CompanyId, SUM(ISNULL(DeliveryTpGrandTotal,0)) AS TotalDelivery, SUM(ISNULL(PaymentAmount,0)) AS ActualPayment, SUM(ISNULL(AIT,0)) AIT, SUM(ISNULL(DiscountOnPayment,0)) DiscountAmount,
                (SUM(ISNULL(PaymentAmount,0)) + SUM(ISNULL(AIT,0)) + SUM(ISNULL(DiscountOnPayment,0))) AS TotalPayment,
                SUM(ISNULL(DeliveryTpGrandTotal,0)) - (SUM(ISNULL(PaymentAmount,0)) + SUM(ISNULL(AIT,0)) + SUM(ISNULL(DiscountOnPayment,0))) - ISNULL(ReturnAmount,0) AS Due FROM tblInvoice AS INV
                LEFT JOIN (SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount FROM CSTL_ACCDB..tblDebitCreditVoucherMaster AS M
                    LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
                    LEFT JOIN tblInvoice AS INV ON M.CommonPK_ID = INV.InvoiceId
                    WHERE ISNULL(M.IsInventoryValuation,0) = 0 AND M.TableID = 2018 GROUP BY M.CommonPK_ID) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
                LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                WHERE ISNULL(IsBadDue,0) = 0 GROUP BY CustomerMasterId, CompanyId, ReturnAmount
                HAVING SUM(ISNULL(DeliveryTpGrandTotal,0)) > 0 AND SUM(ISNULL(DeliveryTpGrandTotal,0)) - (SUM(ISNULL(PaymentAmount,0)) + SUM(ISNULL(AIT,0)) + SUM(ISNULL(DiscountOnPayment,0))) - ISNULL(ReturnAmount,0) > 1
            ) AS TS ON ODR.CustomerMasterId = TS.CustomerMasterId AND UNT.CompanyId = TS.CompanyId

        LEFT JOIN (SELECT CompanyId, CustomerMasterId, SUM(ISNULL(LimitAmount,0)) LimitAmount, SUM(ISNULL(DayLimit,0)) DayLimit FROM tblCustomerCreditLimit
            WHERE IsActive = 1 AND ActionStatus = 'Accepted' GROUP BY CompanyId, CustomerMasterId) AS LM ON ODR.CustomerMasterId = LM.CustomerMasterId AND UNT.CompanyId = LM.CompanyId

        LEFT JOIN (SELECT CustomerMasterId, CompanyId, MIN(InvoiceNo) AS InvoiceNo, MIN(InvoiceDate) AS InvoiceDate FROM tblInvoice AS INV
                LEFT JOIN tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
                LEFT JOIN (SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount FROM CSTL_ACCDB..tblDebitCreditVoucherMaster AS M
                    LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
                    LEFT JOIN tblInvoice AS INV ON M.CommonPK_ID = INV.InvoiceId
                    WHERE ISNULL(M.IsInventoryValuation,0) = 0 AND M.TableID = 2018 GROUP BY M.CommonPK_ID) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
                WHERE ISNULL(IsBadDue,0) = 0 AND ISNULL(DeliveryTpGrandTotal,0) > 0
                    AND (ISNULL(DeliveryTpGrandTotal,0) - (ISNULL(PaymentAmount,0) + ISNULL(AIT,0) + ISNULL(DiscountOnPayment,0))) - ISNULL(ReturnAmount,0) > 1
                GROUP BY CustomerMasterId, CompanyId) AS LD ON ODR.CustomerMasterId = LD.CustomerMasterId AND UNT.CompanyId = LD.CompanyId
    WHERE IsInvoice = 0
        AND (@CompanyId IS NULL OR UNT.CompanyId = @CompanyId)
        AND (@ComUnitId IS NULL OR UNT.ComUnitId = @ComUnitId)
        AND (@AreaId IS NULL OR ODR.AreaId = @AreaId)
        AND (@TerritoryId IS NULL OR ODR.TerritoryId = @TerritoryId)
        AND (@MarketId IS NULL OR ODR.MarketId = @MarketId)
        AND (@SubmissionDate IS NULL OR ODR.SubmissionDate = @SubmissionDate)
        AND (@CustomerMasterId IS NULL OR ODR.CustomerMasterId = @CustomerMasterId)
    ORDER BY SubmissionDate DESC
END
GO
