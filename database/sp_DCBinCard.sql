-- =============================================
-- sp_DCBinCard — corrected version
-- Source of original body: docs/database/stored-procedures-part1.md (recovered from SalesrollDB_ZAS)
--
-- Fixes applied:
--   1. StoReceive / ChallanReceive: Value was UnitPrice * StockQty (StockQty = remaining
--      stock TODAY, so historical receive value shrank as the batch sold). Now uses
--      TotalQuantity, same base as the quantity column (matches StockReceive CTE).
--   2. OB: DCOpeningBalanceDate = @FromDate was an exact datetime match; the snapshot job
--      stamps GETDATE() with time, so opening stock silently came back 0. Now compares
--      date-only.
--   3. ClosingStockValue: ABS() removed — a negative value (issued > received) is a real
--      signal and must show as negative, not be silently flipped positive.
--   4. Final filter: ClosingStock / ClosingStockValue compared with <> 0 instead of > 0
--      so rows that are only negative still appear.
--
-- NOT changed (needs data verification first):
--   * SampleIssue: SUM(ST.Quantity * -1) — correct only if tblSampleIssueTranscation.Quantity
--     is stored negative. Verify: SELECT TOP 5 Quantity FROM tblSampleIssueTranscation.
--   * ProductBonus (OrderDetailsId = 0) overlaps Sales/SalesInTransit for delivered bonus
--     lines — possible double count in TotalIssue; confirm against a known invoice.
-- =============================================
ALTER PROCEDURE [dbo].[sp_DCBinCard]

	@FromDate DATETIME,
	@ToDate DATETIME,
	@DcId INT,
	@DCName NVARCHAR(MAX),
	@CompanyId INT

AS
BEGIN

	-- FIX 5: report dates are day-granular; make the range inclusive of the whole last day
	SET @FromDate = CAST(CAST(@FromDate AS DATE) AS DATETIME);
	SET @ToDate = DATEADD(MILLISECOND, -3, DATEADD(DAY, 1, CAST(CAST(@ToDate AS DATE) AS DATETIME)));

	;WITH ProductBase AS
(
    SELECT
        P.ProductCode,
        P.ProductName,
        P.PackSize
    FROM dbo.tblProduct P WITH (NOLOCK)
    WHERE P.CompanyId = @CompanyId
),

OB AS
(
    SELECT
        OB.ProductCode,
        SUM(ISNULL(OB.StockQty, 0)) AS Quantity,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(OB.StockQty, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(OB.StockQty, 0))
        ) AS Value
    FROM dbo.tblDCStore_OpeningBalance OB WITH (NOLOCK)
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON OB.TempReceiveId = CTRS.ReceiveId
    WHERE
        OB.ComUnitId = @DcId
        -- FIX 2: date-only compare (snapshot job stamps GETDATE() with time)
        AND CAST(OB.DCOpeningBalanceDate AS DATE) = CAST(@FromDate AS DATE)
    GROUP BY
        OB.ProductCode
),

StoReceive AS
(
    SELECT
        DCS.ProductCode,
        SUM(ISNULL(DCS.TotalQuantity, 0)) AS TotalStockReceiveQty,
        -- FIX 1: value on TotalQuantity (received qty), not StockQty (remaining qty)
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(DCS.TotalQuantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(DCS.TotalQuantity, 0))
        ) AS Value
    FROM dbo.tblDCStore DCS WITH (NOLOCK)
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        DCS.ComUnitId = @DcId
        AND DCS.ChalanDetailsId IS NULL
        AND DCS.StockRcvDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        DCS.ProductCode
),

NonTransReceive AS
(
    SELECT
        DCS.ProductCode,
        SUM(ISNULL(DCS.TotalQuantity, 0)) AS TotalStockReceiveQty,
        SUM(
            (ISNULL(NTD.Price, 0) * ISNULL(DCS.TotalQuantity, 0))
            + (ISNULL(NTD.VAT, 0) * ISNULL(DCS.TotalQuantity, 0))
        ) AS Value
    FROM dbo.tblDCStore DCS WITH (NOLOCK)
    INNER JOIN dbo.tblNonTranscationalInvoiceDetail NTD WITH (NOLOCK)
        ON NTD.InvReturnDetailID = DCS.NonTranscationalInvReturnDetailID
    WHERE
        DCS.ComUnitId = @DcId
        AND ISNULL(DCS.NonTranscationalInvReturnDetailID, 0) > 0
        AND ISNULL(DCS.ReceiveId, 0) = 0
        AND ISNULL(DCS.TempReceiveId, 0) = 0
        AND DCS.StockRcvDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        DCS.ProductCode
),

ChallanReceive AS
(
    SELECT
        DCS.ProductCode,
        SUM(ISNULL(DCS.TotalQuantity, 0)) AS TotalStockReceiveQty,
        -- FIX 1: value on TotalQuantity (received qty), not StockQty (remaining qty)
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(DCS.TotalQuantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(DCS.TotalQuantity, 0))
        ) AS Value
    FROM dbo.tblDCStore DCS WITH (NOLOCK)
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        DCS.ComUnitId = @DcId
        AND DCS.ChalanDetailsId IS NOT NULL
        AND DCS.StockRcvDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        DCS.ProductCode
),

StockReceive AS
(
    SELECT
        DCS.ProductCode,
        SUM(ISNULL(DCS.TotalQuantity, 0)) AS TotalStockReceiveQty,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(DCS.TotalQuantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(DCS.TotalQuantity, 0))
        ) AS Value
    FROM dbo.tblDCStore DCS WITH (NOLOCK)
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        DCS.ComUnitId = @DcId
        AND DCS.StockRcvDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        DCS.ProductCode
),

Sales AS
(
    SELECT
        ID.ProductCode,
        SUM(ISNULL(ID.DeliveryQuantity, 0)) AS Sales,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(ID.DeliveryQuantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(ID.DeliveryQuantity, 0))
        ) AS Value
    FROM dbo.tblInvoiceDetail ID WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice I WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = ID.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        I.ComUnitId = @DcId
        AND ID.DeliveryStatus IN ('Full', 'Partial')
        AND I.InvoiceDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        ID.ProductCode
),

Bonus AS
(
    SELECT
        ID.ProductCode,
        SUM(ISNULL(ID.BonusQuantity, 0)) AS BonusQty,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(ID.BonusQuantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(ID.BonusQuantity, 0))
        ) AS Value
    FROM dbo.tblInvoiceDetail ID WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice I WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = ID.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        I.ComUnitId = @DcId
        AND (ID.DeliveryStatus IS NULL OR ID.DeliveryStatus IN ('Full', 'Partial'))
        AND I.InvoiceDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        ID.ProductCode
),

ProductBonus AS
(
    SELECT
        ID.ProductCode,
        SUM(ISNULL(ID.DeliveryQuantity, 0)) AS Sales,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(ID.DeliveryQuantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(ID.DeliveryQuantity, 0))
        ) AS Value
    FROM dbo.tblInvoiceDetail ID WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice I WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = ID.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        I.ComUnitId = @DcId
        AND ID.OrderDetailsId = 0
        AND I.InvoiceDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        ID.ProductCode
),

SalesInTransit AS
(
    SELECT
        ID.ProductCode,
        SUM(ISNULL(ID.TotalQuantity, 0)) AS Sales,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(ID.TotalQuantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(ID.TotalQuantity, 0))
        ) AS Value
    FROM dbo.tblInvoiceDetail ID WITH (NOLOCK)
    INNER JOIN dbo.tblInvoice I WITH (NOLOCK)
        ON ID.InvoiceId = I.InvoiceId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = ID.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        I.ComUnitId = @DcId
        AND ID.DeliveryStatus IS NULL
        AND I.InvoiceDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        ID.ProductCode
),

ChallanIssue AS
(
    SELECT
        CD.ProductCode,
        SUM(ISNULL(CD.Quantity, 0)) AS Challan,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(CD.Quantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(CD.Quantity, 0))
        ) AS Value
    FROM dbo.tblChalanDetail CD WITH (NOLOCK)
    INNER JOIN dbo.tblChalanInfo CI WITH (NOLOCK)
        ON CD.ChalanId = CI.ChalanId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = CD.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        CI.FromComUnitId = @DcId
        AND CI.ChalanDate BETWEEN @FromDate AND @ToDate
        AND CI.IsDeliver = 'True'
    GROUP BY
        CD.ProductCode
),

ChallanTransit AS
(
    SELECT
        CD.ProductCode,
        SUM(ISNULL(CD.Quantity, 0)) AS Challan,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(CD.Quantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(CD.Quantity, 0))
        ) AS Value
    FROM dbo.tblChalanDetail CD WITH (NOLOCK)
    INNER JOIN dbo.tblChalanInfo CI WITH (NOLOCK)
        ON CD.ChalanId = CI.ChalanId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = CD.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        CI.FromComUnitId = @DcId
        AND CI.ChalanDate BETWEEN @FromDate AND @ToDate
        AND CI.IsDeliver = 'False'
    GROUP BY
        CD.ProductCode
),

FreezeRestricted AS
(
    SELECT
        F.ProductCode,
        SUM(ISNULL(F.StockQty, 0) + ISNULL(F.DamageQty, 0)) AS Freeze,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(F.StockQty, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(F.StockQty, 0))
            + (ISNULL(CTRS.UnitPrice, 0) * ISNULL(F.DamageQty, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(F.DamageQty, 0))
        ) AS Value
    FROM dbo.tblDCStoreFreeze F WITH (NOLOCK)
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = F.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        F.ComUnitId = @DcId
        AND F.StockCondition = 'Restricted'
        AND F.StockRcvDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        F.ProductCode
),

FreezeBlocked AS
(
    SELECT
        F.ProductCode,
        SUM(ISNULL(F.StockQty, 0) + ISNULL(F.DamageQty, 0)) AS Freeze,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(F.StockQty, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(F.StockQty, 0))
            + (ISNULL(CTRS.UnitPrice, 0) * ISNULL(F.DamageQty, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(F.DamageQty, 0))
        ) AS Value
    FROM dbo.tblDCStoreFreeze F WITH (NOLOCK)
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = F.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        F.ComUnitId = @DcId
        AND F.StockCondition = 'Blocked'
        AND F.StockRcvDate BETWEEN @FromDate AND @ToDate
    GROUP BY
        F.ProductCode
),

SampleIssue AS
(
    SELECT
        SMPD.ProductCode,
        -- ponytail: sign kept as-is; correct only if ST.Quantity is stored negative —
        -- verify against data before touching (see header note)
        SUM(ISNULL(ST.Quantity, 0) * -1) AS SampleIssue,
        SUM(
            (ISNULL(CTRS.UnitPrice, 0) * ISNULL(SMPD.Quantity, 0))
            + (ISNULL(CTRS.VATPerUnit, 0) * ISNULL(SMPD.Quantity, 0))
        ) AS Value
    FROM dbo.tblSampleIssue SMPI WITH (NOLOCK)
    LEFT JOIN dbo.tblSampleIssueDetail SMPD WITH (NOLOCK)
        ON SMPI.OrderId = SMPD.OrderId
    LEFT JOIN dbo.tblSampleIssueTranscation ST WITH (NOLOCK)
        ON ST.IssueDetailId = SMPD.OrderDetailId
    INNER JOIN dbo.tblDCStore DCS WITH (NOLOCK)
        ON DCS.DCStoreId = ST.DCStoreId
    INNER JOIN dbo.tblCentralStore CTRS WITH (NOLOCK)
        ON DCS.TempReceiveId = CTRS.ReceiveId
    WHERE
        SMPI.ActionStatus = 'Accepted'
        AND SMPI.ApprovedDate BETWEEN @FromDate AND @ToDate
        AND SMPI.ComUnitId = @DcId
    GROUP BY
        SMPD.ProductCode
),

RawData AS
(
    SELECT
        PB.ProductCode,
        PB.ProductName,
        PB.PackSize AS BaseUnit,

        ISNULL(OB.Quantity, 0) AS OpeningStock,
        ISNULL(SR.TotalStockReceiveQty, 0) AS ReceiveFromCentralWarehouse,
        ISNULL(NTR.TotalStockReceiveQty, 0) AS ReturnQty,
        ISNULL(CR.TotalStockReceiveQty, 0) AS ReceiveFromAreaOfficeInterTransfer,
        ISNULL(STR.TotalStockReceiveQty, 0) AS BaseTotalReceived,

        ISNULL(CT.Challan, 0) AS IntransitWHB2B,
        ISNULL(SIT.Sales, 0) AS SalesInTransit,
        ISNULL(SL.Sales, 0) AS IssuedToSales,
        ISNULL(BN.BonusQty, 0) AS IssuedToBonus,
        ISNULL(PBNS.Sales, 0) AS IssuedToProductBonus,
        ISNULL(CI.Challan, 0) AS IssuedToAreaOfficeInterTransfer,
        ISNULL(FR.Freeze, 0) AS IssuedToDamageAndOthers,
        ISNULL(FB.Freeze, 0) AS Blocked,
        ISNULL(SI.SampleIssue, 0) AS SampleIssue,

        ISNULL(OB.Value, 0) AS OBValue,
        ISNULL(SR.Value, 0) AS ReceiveValue,
        ISNULL(NTR.Value, 0) AS NonTransReceiveValue,
        ISNULL(CR.Value, 0) AS ChallanReceiveValue,
        ISNULL(CT.Value, 0) AS ChallanTransitValue,

        ISNULL(SL.Value, 0) AS SalesValue,
        ISNULL(PBNS.Value, 0) AS ProductBonusValue,
        ISNULL(CI.Value, 0) AS ChallanIssueValue,
        ISNULL(SI.Value, 0) AS SampleIssueValue,
        ISNULL(BN.Value, 0) AS BonusValue,
        ISNULL(SIT.Value, 0) AS SalesTransitValue
    FROM ProductBase PB
    LEFT JOIN OB
        ON OB.ProductCode = PB.ProductCode
    LEFT JOIN StoReceive SR
        ON SR.ProductCode = PB.ProductCode
    LEFT JOIN NonTransReceive NTR
        ON NTR.ProductCode = PB.ProductCode
    LEFT JOIN ChallanReceive CR
        ON CR.ProductCode = PB.ProductCode
    LEFT JOIN StockReceive STR
        ON STR.ProductCode = PB.ProductCode
    LEFT JOIN Sales SL
        ON SL.ProductCode = PB.ProductCode
    LEFT JOIN Bonus BN
        ON BN.ProductCode = PB.ProductCode
    LEFT JOIN ProductBonus PBNS
        ON PBNS.ProductCode = PB.ProductCode
    LEFT JOIN SalesInTransit SIT
        ON SIT.ProductCode = PB.ProductCode
    LEFT JOIN ChallanIssue CI
        ON CI.ProductCode = PB.ProductCode
    LEFT JOIN ChallanTransit CT
        ON CT.ProductCode = PB.ProductCode
    LEFT JOIN FreezeRestricted FR
        ON FR.ProductCode = PB.ProductCode
    LEFT JOIN FreezeBlocked FB
        ON FB.ProductCode = PB.ProductCode
    LEFT JOIN SampleIssue SI
        ON SI.ProductCode = PB.ProductCode
),

FinalData AS
(
    SELECT
        ProductCode,
        ProductName,
        @FromDate AS FromDate,
        @ToDate AS ToDate,
        @DCName AS DC,
        BaseUnit,

        OpeningStock,
        ReceiveFromCentralWarehouse,
        ReturnQty,
        ReceiveFromAreaOfficeInterTransfer,

        BaseTotalReceived + ReturnQty AS TotalReceived,

        IntransitWHB2B,
        SalesInTransit,
        IssuedToSales,
        IssuedToBonus,
        IssuedToProductBonus,
        IssuedToAreaOfficeInterTransfer,
        IssuedToDamageAndOthers,
        Blocked,
        SampleIssue,

        (
            OpeningStock
            + ReceiveFromCentralWarehouse
            + ReturnQty
            + ReceiveFromAreaOfficeInterTransfer
            + IntransitWHB2B
        ) AS TotalStock,

        (
            IssuedToSales
            + IssuedToProductBonus
            + IssuedToAreaOfficeInterTransfer
            + SampleIssue
            + IssuedToBonus
            + SalesInTransit
        ) AS TotalIssue,

        (
            OpeningStock
            + ReceiveFromCentralWarehouse
            + ReturnQty
            + ReceiveFromAreaOfficeInterTransfer
            + IntransitWHB2B
        )
        -
        (
            IssuedToSales
            + IssuedToProductBonus
            + IssuedToAreaOfficeInterTransfer
            + SampleIssue
            + IssuedToBonus
            + SalesInTransit
        ) AS ClosingStock,

        (
            SalesValue
            + ProductBonusValue
            + ChallanIssueValue
            + SampleIssueValue
            + BonusValue
            + SalesTransitValue
        ) AS IssuedValue,

        (
            OBValue
            + ReceiveValue
            + NonTransReceiveValue
            + ChallanReceiveValue
            + ChallanTransitValue
        ) AS TotalRcvValue,

        -- FIX 3: ABS removed — negative means issued value exceeded received value,
        -- which the reader must see, not have flipped positive
        (
            (
                OBValue
                + ReceiveValue
                + NonTransReceiveValue
                + ChallanReceiveValue
                + ChallanTransitValue
            )
            -
            (
                SalesValue
                + ProductBonusValue
                + ChallanIssueValue
                + SampleIssueValue
                + BonusValue
                + SalesTransitValue
            )
        ) AS ClosingStockValue
    FROM RawData
)

SELECT *
FROM FinalData
WHERE
    OpeningStock > 0
    OR ReceiveFromCentralWarehouse > 0
    OR ReturnQty > 0
    OR ReceiveFromAreaOfficeInterTransfer > 0
    OR TotalReceived > 0
    OR IntransitWHB2B > 0
    OR SalesInTransit > 0
    OR IssuedToSales > 0
    OR IssuedToBonus > 0
    OR IssuedToProductBonus > 0
    OR IssuedToAreaOfficeInterTransfer > 0
    OR IssuedToDamageAndOthers > 0
    OR Blocked > 0
    OR SampleIssue > 0
    OR TotalStock > 0
    OR TotalIssue > 0
    -- FIX 4: <> 0 so negative-only rows are not hidden
    OR ClosingStock <> 0
    OR IssuedValue > 0
    OR TotalRcvValue > 0
    OR ClosingStockValue <> 0
ORDER BY
    ProductCode;

END
