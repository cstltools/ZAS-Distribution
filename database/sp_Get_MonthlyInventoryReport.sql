-- =============================================
-- sp_Get_MonthlyInventoryReport — reformatted only (SalesDisDB_SMC_NEWDB)
-- Restructured into the same CTE style as sp_DCBinCard. NO logic change:
-- every derived table became a CTE with its ORIGINAL alias name, all
-- predicates, expressions, column order and ORDER BY are byte-for-byte
-- equivalent to the old body. Unused joins from the original
-- (vTblFreez, currentStock, tblfreez, TblInvActual) are kept as-is.
-- Original body backed up before this was applied.
-- =============================================
ALTER PROCEDURE [dbo].[sp_Get_MonthlyInventoryReport]

	@fromDate DATETIME,
	@toDate DATETIME,
	@CiD NVARCHAR(MAX),
	@ProTypId NVARCHAR(MAX)

AS
BEGIN

	;WITH ProductBase AS
(
    SELECT
        P.ProductCode,
        P.ProductName,
        P.PackSize
    FROM dbo.tblProduct P WITH (NOLOCK)
    WHERE P.ProductGroupId = COALESCE(NULLIF(@ProTypId, 0), P.ProductGroupId)
),

-- Opening balance (snapshot rows stamped exactly @fromDate)
vTblOB AS
(
    SELECT
        ProductCode,
        SUM(StockQty) AS Quantity
    FROM dbo.tblDCStore_OpeningBalance WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND DCOpeningBalanceDate = @fromDate
    GROUP BY
        ProductCode
),

-- Opening balance of frozen stock
vTblOBfreez AS
(
    SELECT
        ProductCode,
        SUM(StockQty) AS Quantity
    FROM dbo.tblDCStoreFreeze WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND Opening = @fromDate
    GROUP BY
        ProductCode
),

-- Sub-invoice returns (old DB: SalesDisDB_SMC)
tblDRTsub AS
(
    SELECT
        (SUM(ID.Quantity) - SUM(ID.DeliveryQuantity)) AS DelQty,
        ID.ProductCode,
        ((SUM(ID.NetAmount) - SUM(ID.TotalPriceVatAmount))
            - SUM(ID.DeliveryNetAmount - ID.DeliveryTotalPriceVatAmount)) AS SumofNetReturnAmount,
        COUNT(DISTINCT I.DelivaryInvoiceNo) AS NumberofReturnInvoice,
        SUM(ID.TotalPriceVatAmount) - SUM(DeliveryTotalPriceVatAmount) AS TotalPriceVatAmount
    FROM SalesDisDB_SMC..tblSubInvoiceMaster I WITH (NOLOCK)
    INNER JOIN SalesDisDB_SMC..tblSubInvoiceDetail ID WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 0
        AND ID.DeliveryStatus IN ('Reject', 'Partial')
        AND I.UpdateDate BETWEEN @fromDate AND @toDate
        AND I.TpGrandTotal > 0
    GROUP BY
        ID.ProductCode
),

-- Receive from central warehouse (no challan reference)
vTblStoReceive AS
(
    SELECT
        ProductCode,
        SUM(TotalQuantity) AS TotalStockReceiveQty
    FROM dbo.tblDCStore WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND SChalanDetailsId IS NULL
        AND ChalanDetailsId IS NULL
        AND StockRcvDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Receive via inter-office challan
vTblChallanReceive AS
(
    SELECT
        ProductCode,
        SUM(TotalQuantity) AS TotalStockReceiveQty
    FROM dbo.tblDCStore WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ChalanDetailsId IS NOT NULL
        AND StockRcvDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- All receives in period
vTblStockReceive AS
(
    SELECT
        ProductCode,
        SUM(TotalQuantity) AS TotalStockReceiveQty
    FROM dbo.tblDCStore WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND StockRcvDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Invoiced sales (non-gift)
vTblsales AS
(
    SELECT
        ProductCode,
        SUM(tblInvoiceDetail.Quantity) AS Sales
    FROM dbo.tblInvoiceDetail WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice
        ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 0
        AND InvoiceDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Sales returns / undelivered (current DB, non-gift)
tblD AS
(
    SELECT
        (SUM(ID.Quantity) - SUM(ID.DeliveryQuantity)) AS DelQty,
        ID.ProductCode,
        ((SUM(ID.NetAmount) - SUM(ID.TotalPriceVatAmount))
            - SUM(ID.DeliveryNetAmount - ID.DeliveryTotalPriceVatAmount)) AS SumofNetReturnAmount,
        COUNT(DISTINCT I.DelivaryInvoiceNo) AS NumberofReturnInvoice,
        SUM(ID.TotalPriceVatAmount) - SUM(DeliveryTotalPriceVatAmount) AS TotalPriceVatAmount
    FROM dbo.tblInvoice I WITH (NOLOCK)
    INNER JOIN dbo.tblInvoiceDetail ID WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 0
        AND ID.DeliveryStatus IN ('Reject', 'Partial')
        AND I.UpdateDate BETWEEN @fromDate AND @toDate
        AND I.TpGrandTotal > 0
    GROUP BY
        ID.ProductCode
),

-- Booked for delivery (invoice created, not yet delivered)
tblBookforDeliveryQty AS
(
    SELECT
        ProductCode,
        SUM(quantity) AS BookforDeliveryQty
    FROM tblinvoice
    INNER JOIN tblInvoiceDetail
        ON tblinvoice.invoiceid = tblInvoiceDetail.invoiceid
    WHERE
        DelivaryInvoiceNo IS NULL
        AND updatedate IS NULL
        AND invoicedate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Gift / product bonus issued
vTblProductBonus AS
(
    SELECT
        ProductCode,
        SUM(TotalQuantity) AS Sales
    FROM dbo.tblInvoiceDetail WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice
        ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 1
        AND InvoiceDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Gift/bonus returns (current DB)
tblreturnBonus AS
(
    SELECT
        (SUM(ID.TotalQuantity) - SUM(ID.DeliveryQuantity)) AS DelQty,
        ID.ProductCode,
        ((SUM(ID.NetAmount) - SUM(ID.TotalPriceVatAmount))
            - SUM(ID.DeliveryNetAmount - ID.DeliveryTotalPriceVatAmount)) AS SumofNetReturnAmount,
        COUNT(DISTINCT I.DelivaryInvoiceNo) AS NumberofReturnInvoice,
        SUM(ID.TotalPriceVatAmount) - SUM(DeliveryTotalPriceVatAmount) AS TotalPriceVatAmount
    FROM dbo.tblInvoice I WITH (NOLOCK)
    INNER JOIN dbo.tblInvoiceDetail ID WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 1
        AND ID.DeliveryStatus IN ('Reject', 'Partial')
        AND I.UpdateDate BETWEEN @fromDate AND @toDate
        AND I.TpGrandTotal > 0
    GROUP BY
        ID.ProductCode
),

-- Gift/bonus returns (old DB: SalesDisDB_SMC)
tblreturnBonusold AS
(
    SELECT
        (SUM(ID.TotalQuantity) - SUM(ID.DeliveryQuantity)) AS DelQty,
        ID.ProductCode,
        ((SUM(ID.NetAmount) - SUM(ID.TotalPriceVatAmount))
            - SUM(ID.DeliveryNetAmount - ID.DeliveryTotalPriceVatAmount)) AS SumofNetReturnAmount,
        COUNT(DISTINCT I.DelivaryInvoiceNo) AS NumberofReturnInvoice,
        SUM(ID.TotalPriceVatAmount) - SUM(DeliveryTotalPriceVatAmount) AS TotalPriceVatAmount
    FROM SalesDisDB_SMC..tblInvoice I WITH (NOLOCK)
    INNER JOIN SalesDisDB_SMC..tblInvoiceDetail ID WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 1
        AND ID.DeliveryStatus IN ('Reject', 'Partial')
        AND I.UpdateDate BETWEEN @fromDate AND @toDate
        AND I.TpGrandTotal > 0
    GROUP BY
        ID.ProductCode
),

-- Inter-office challan issued out
vTblChallan AS
(
    SELECT
        ProductCode,
        SUM(Quantity) AS Challan
    FROM dbo.tblChalanDetail WITH (NOLOCK)
    INNER JOIN dbo.tblChalanInfo
        ON tblChalanDetail.ChalanId = tblChalanInfo.ChalanId
    WHERE
        FromComUnitId = COALESCE(NULLIF(@CiD, 0), FromComUnitId)
        AND ChalanDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Freeze in period (kept from original; not referenced in output)
vTblFreez AS
(
    SELECT
        ProductCode,
        (SUM(StockQty) + SUM(DamageQty)) AS Freeze
    FROM dbo.tblDCStoreFreeze WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND StockRcvDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Blocked stock in period
vTblFreez2 AS
(
    SELECT
        ProductCode,
        (SUM(StockQty) + SUM(DamageQty)) AS Freeze
    FROM dbo.tblDCStoreFreeze WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND [StockCondition] = 'Blocked'
        AND ReceiveDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Current stock (kept from original; not referenced in output)
currentStock AS
(
    SELECT
        SUM(StockQty) AS Closingstock,
        ProductCode
    FROM dbo.tblDCStore WITH (NOLOCK)
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
    GROUP BY
        ProductCode
),

-- Depot → warehouse return challan
vTblChallantoWH AS
(
    SELECT
        ProductCode,
        SUM(Quantity) AS qty
    FROM dbo.tblDepotToWHChalanInfo WITH (NOLOCK)
    INNER JOIN dbo.tblDepotToWHChalanDetail
        ON tblDepotToWHChalanInfo.SChalanId = tblDepotToWHChalanDetail.SChalanId
    INNER JOIN dbo.tblCompanyUnit
        ON tblDepotToWHChalanInfo.FromComUnitCode = tblCompanyUnit.ComUnitCode
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ChalanDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Sub-depot transfer out
vTblSubdeportTransfer AS
(
    SELECT
        ProductCode,
        SUM(Quantity) AS qty1
    FROM dbo.tblSubDepotChalanInfo WITH (NOLOCK)
    INNER JOIN dbo.tblSubDepotChalanDetail
        ON tblSubDepotChalanInfo.SChalanId = tblSubDepotChalanDetail.SChalanId
    INNER JOIN dbo.tblCompanyUnit
        ON tblSubDepotChalanInfo.FromComUnitCode = tblCompanyUnit.ComUnitCode
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ChalanDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Sub-depot return in
vTblSubdeportReturn AS
(
    SELECT
        ProductCode,
        SUM(Quantity) AS qty2
    FROM dbo.tblSubDepotChalanReturnInfo WITH (NOLOCK)
    INNER JOIN dbo.tblSubDepotChalanRetuenDetail
        ON tblSubDepotChalanReturnInfo.SChalanId = tblSubDepotChalanRetuenDetail.SChalanId
    INNER JOIN dbo.tblCompanyUnit
        ON tblSubDepotChalanReturnInfo.FromComUnitCode = tblCompanyUnit.ComUnitCode
    WHERE
        IsDeliver = 'True'
        AND ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ChalanDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Direct stock out / adjustment voucher
vTblDirectStockOut AS
(
    SELECT
        ProductCode,
        SUM(StackOutQty) AS StockOutQty
    FROM dbo.tblDeStockOutMaster WITH (NOLOCK)
    INNER JOIN dbo.tblDeStockOutDetails
        ON tblDeStockOutMaster.DcStockOutMasterId = tblDeStockOutDetails.DcStockOutMasterId
    INNER JOIN dbo.tblCompanyUnit
        ON tblDeStockOutMaster.ComUnitId = tblCompanyUnit.ComUnitId
    WHERE
        tblDeStockOutMaster.ComUnitId = COALESCE(NULLIF(@CiD, 0), tblDeStockOutMaster.ComUnitId)
        AND ApprovedDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Freeze without unit filter (kept from original; not referenced in output)
tblfreez AS
(
    SELECT
        ProductCode,
        SUM(StockQty) AS StockQty
    FROM dbo.tblDCStoreFreeze WITH (NOLOCK)
    WHERE
        StockRcvDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Sales returns / undelivered (old DB: SalesDisDB_SMC, non-gift)
tblDRT AS
(
    SELECT
        (SUM(ID.Quantity) - SUM(ID.DeliveryQuantity)) AS DelQty,
        ID.ProductCode,
        ((SUM(ID.NetAmount) - SUM(ID.TotalPriceVatAmount))
            - SUM(ID.DeliveryNetAmount - ID.DeliveryTotalPriceVatAmount)) AS SumofNetReturnAmount,
        COUNT(DISTINCT I.DelivaryInvoiceNo) AS NumberofReturnInvoice,
        SUM(ID.TotalPriceVatAmount) - SUM(DeliveryTotalPriceVatAmount) AS TotalPriceVatAmount
    FROM SalesDisDB_SMC..tblInvoice I WITH (NOLOCK)
    INNER JOIN SalesDisDB_SMC..tblInvoiceDetail ID WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 0
        AND ID.DeliveryStatus IN ('Reject', 'Partial')
        AND I.UpdateDate BETWEEN @fromDate AND @toDate
        AND I.TpGrandTotal > 0
    GROUP BY
        ID.ProductCode
),

-- Second return (partial second-return settlement)
tbl2ndrtn AS
(
    SELECT
        P.ProductCode,
        SUM((ISNULL(tblInvoiceDetailReturn.PreviousQuantity, 0)))
            - SUM((ISNULL(tblInvoiceDetailReturn.sndReturnQuantity, 0))) AS Quantity
    FROM tblInvoice iv WITH (NOLOCK)
    INNER JOIN tblInvoiceDetail ivD WITH (NOLOCK)
        ON iv.InvoiceId = ivD.InvoiceId
    LEFT JOIN tblProduct P WITH (NOLOCK)
        ON P.ProductCode = ivD.ProductCode
    LEFT JOIN tblStockUOM WITH (NOLOCK)
        ON tblStockUOM.StockUOMId = P.StockUOMId
    LEFT JOIN tblOrder O WITH (NOLOCK)
        ON O.OrderId = iv.OrderId
    LEFT JOIN tblEmpGeneralInfo
        ON tblEmpGeneralInfo.EmpMasterCode = O.OrderSenderCode
    LEFT JOIN tblCompanyUnit U WITH (NOLOCK)
        ON O.ComUnitId = U.ComUnitId
    LEFT JOIN tblUnitPrice
        ON tblUnitPrice.ProductCode = P.ProductCode
    LEFT JOIN tblArea
        ON O.AreaId = tblArea.AreaId
    LEFT JOIN tblRegion
        ON O.RegionId = tblRegion.RegionId
    LEFT JOIN tblDCStore
        ON tblDCStore.DCStoreId = ivD.DCStoreId
    INNER JOIN tblInvoiceDetailReturn
        ON tblInvoiceDetailReturn.InvoiceDetailId = ivD.InvoiceDetailId
    WHERE
        tblInvoiceDetailReturn.PreviousQuantity <> tblInvoiceDetailReturn.sndReturnQuantity
        AND iv.SndReturnPaymentDate BETWEEN @fromDate AND @toDate
        AND iv.ComUnitId = COALESCE(NULLIF(@CiD, 0), iv.ComUnitId)
    GROUP BY
        P.ProductCode
),

-- Invoiced actual qty (kept from original; not referenced in output)
TblInvActual AS
(
    SELECT
        ProductCode,
        SUM(tblInvoiceDetail.Quantity) AS InvActualQty
    FROM dbo.tblInvoiceDetail WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice
        ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
    WHERE
        ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND ISGiftProduct = 0
        AND InvoiceDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

-- Payment-stage return (delivered but not paid in full)
tblreturn AS
(
    SELECT
        ProductCode,
        SUM(tblInvoiceDetail.DeliveryQuantity) - SUM(tblInvoiceDetail.PaymentQuantity) AS ReturnQty
    FROM dbo.tblInvoiceDetail WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice
        ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
    WHERE
        PaymentInvoiceNo IS NOT NULL
        AND DeliveryQuantity <> PaymentQuantity
        AND ComUnitId = COALESCE(NULLIF(@CiD, 0), ComUnitId)
        AND tblInvoice.PaymentDate BETWEEN @fromDate AND @toDate
    GROUP BY
        ProductCode
),

RawData AS
(
    SELECT
        PB.ProductCode,
        PB.ProductName,
        PB.PackSize AS BaseUnit,

        ISNULL(vTblOB.Quantity, 0) AS OBQty,
        ISNULL(vTblOBfreez.Quantity, 0) AS OBFreezeQty,
        ISNULL(vTblStoReceive.TotalStockReceiveQty, 0) AS StoReceiveQty,
        ISNULL(vTblChallanReceive.TotalStockReceiveQty, 0) AS ChallanReceiveQty,
        ISNULL(vTblStockReceive.TotalStockReceiveQty, 0) AS StockReceiveQty,

        ISNULL(vTblsales.Sales, 0) AS SalesQty,
        ISNULL(tblD.DelQty, 0) AS InvReturnQty,
        ISNULL(tblDRT.DelQty, 0) AS InvReturnOldQty,
        ISNULL(tblDRTsub.DelQty, 0) AS SubInvReturnQty,

        ISNULL(vTblProductBonus.Sales, 0) AS ProductBonusQty,
        ISNULL(tblreturnBonus.DelQty, 0) AS BonusReturnQty,
        ISNULL(tblreturnBonusold.DelQty, 0) AS BonusReturnOldQty,

        ISNULL(vTblChallan.Challan, 0) AS ChallanIssueQty,
        ISNULL(vTblFreez2.Freeze, 0) AS BlockedQty,
        ISNULL(vTblChallantoWH.qty, 0) AS WHReturnQty,
        ISNULL(vTblSubdeportTransfer.qty1, 0) AS SubDepotTransferQty,
        ISNULL(vTblSubdeportReturn.qty2, 0) AS SubDepotReturnQty,
        ISNULL(vTblDirectStockOut.StockOutQty, 0) AS DirectStockOutQty,

        ISNULL(tblBookforDeliveryQty.BookforDeliveryQty, 0) AS BookforDeliveryQty,
        ISNULL(tblreturn.ReturnQty, 0) AS PaymentReturnQty,
        ISNULL(tbl2ndrtn.Quantity, 0) AS SecondReturnQty
    FROM ProductBase PB
    LEFT JOIN vTblOB
        ON vTblOB.ProductCode = PB.ProductCode
    LEFT JOIN vTblOBfreez
        ON vTblOBfreez.ProductCode = PB.ProductCode
    LEFT JOIN tblDRTsub
        ON tblDRTsub.ProductCode = PB.ProductCode
    LEFT JOIN vTblStoReceive
        ON vTblStoReceive.ProductCode = PB.ProductCode
    LEFT JOIN vTblChallanReceive
        ON vTblChallanReceive.ProductCode = PB.ProductCode
    LEFT JOIN vTblStockReceive
        ON vTblStockReceive.ProductCode = PB.ProductCode
    LEFT JOIN vTblsales
        ON vTblsales.ProductCode = PB.ProductCode
    LEFT JOIN tblD
        ON tblD.ProductCode = PB.ProductCode
    LEFT JOIN tblBookforDeliveryQty
        ON tblBookforDeliveryQty.ProductCode = PB.ProductCode
    LEFT JOIN vTblProductBonus
        ON vTblProductBonus.ProductCode = PB.ProductCode
    LEFT JOIN tblreturnBonus
        ON tblreturnBonus.ProductCode = PB.ProductCode
    LEFT JOIN tblreturnBonusold
        ON tblreturnBonusold.ProductCode = PB.ProductCode
    LEFT JOIN vTblChallan
        ON vTblChallan.ProductCode = PB.ProductCode
    LEFT JOIN vTblFreez
        ON vTblFreez.ProductCode = PB.ProductCode
    LEFT JOIN vTblFreez2
        ON vTblFreez2.ProductCode = PB.ProductCode
    LEFT JOIN currentStock
        ON currentStock.ProductCode = PB.ProductCode
    LEFT JOIN vTblChallantoWH
        ON vTblChallantoWH.ProductCode = PB.ProductCode
    LEFT JOIN vTblSubdeportTransfer
        ON vTblSubdeportTransfer.ProductCode = PB.ProductCode
    LEFT JOIN vTblSubdeportReturn
        ON vTblSubdeportReturn.ProductCode = PB.ProductCode
    LEFT JOIN vTblDirectStockOut
        ON vTblDirectStockOut.ProductCode = PB.ProductCode
    LEFT JOIN tblfreez
        ON tblfreez.ProductCode = PB.ProductCode
    LEFT JOIN tblDRT
        ON tblDRT.ProductCode = PB.ProductCode
    LEFT JOIN tbl2ndrtn
        ON tbl2ndrtn.ProductCode = PB.ProductCode
    LEFT JOIN TblInvActual
        ON TblInvActual.ProductCode = PB.ProductCode
    LEFT JOIN tblreturn
        ON tblreturn.ProductCode = PB.ProductCode
),

FinalData AS
(
    SELECT
        BookforDeliveryQty,

        PaymentReturnQty + SecondReturnQty AS ReturnQty,

        ISNULL((SELECT ComUnitName FROM tblCompanyUnit WHERE ComUnitId = @CiD), '') AS ComUnitName,
        CONVERT(VARCHAR, @fromDate, 6) AS fromDate,
        CONVERT(VARCHAR, @toDate, 6) AS toDate,

        ProductCode,
        ProductName,
        BaseUnit,

        OBQty + OBFreezeQty AS OpeningStock,
        StoReceiveQty AS ReceiveFromCentralWarehouse,
        ChallanReceiveQty AS ReceiveFromAreaOfficeInterTransfer,

        StockReceiveQty + OBQty + SubDepotReturnQty + OBFreezeQty AS TotalReceived,

        SalesQty - (InvReturnQty + InvReturnOldQty + SubInvReturnQty) AS IssuedToSales,

        (ProductBonusQty - BonusReturnQty) - BonusReturnOldQty AS IssuedToProductBonus,

        ChallanIssueQty AS IssuedToAreaOfficeInterTransfer,
        0 AS IssuedToDamageAndOthers,
        BlockedQty AS Blocked,

        (
            (OBQty + OBFreezeQty + StoReceiveQty + ChallanReceiveQty)
            - (SalesQty - (InvReturnQty + InvReturnOldQty + SubInvReturnQty))
            - (ProductBonusQty - (BonusReturnQty + BonusReturnOldQty))
            - DirectStockOutQty
            - ChallanIssueQty
            - WHReturnQty
            + PaymentReturnQty
        ) + SecondReturnQty AS ClosingStock,

        WHReturnQty AS WHReturn,
        SubDepotTransferQty AS SubdepoTransfer,
        SubDepotReturnQty AS Subdeporeturn,
        DirectStockOutQty AS StockOutQty
    FROM RawData
)

SELECT *
FROM FinalData
ORDER BY
    ProductName;

END
