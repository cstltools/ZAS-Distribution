using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;

namespace Library.DAL.SInventory_DAL
{
    public class ZASMonitoringDal
    {

        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        DB_Manager aDbManager = new DB_Manager();



        public DataTable DCStock()
        {
            string query = @"SELECT DCS.DCStoreId,ProductCode,ProductName,ReceiveDate,TotalQuantity,StockQty AS AvilableStock,ISNULL(B2BS.Challan,0) B2bIssue, ISNULL(SoldQuantity,0) SoldQuantity, ISNULL(BookForDelivery,0) BookForDelivery,	 ISNULL(SampleQty,0) SampleQty, ISNULL(FRZ.DcFreeze,0) ReturnStock, ISNULL(FRZR.DcFreeze,0) Restricted, ISNULL(FRZB.DcFreeze,0) Blocked, ISNULL(DOT.DirectStockOutQty,0) DirectStockOutQty,	 ISNULL(Destroy,0) Destroy, ISNULL(B2BS.Challan,0) + ISNULL(SoldQuantity,0) + ISNULL(BookForDelivery,0) 	 + ISNULL(SampleQty,0) + ISNULL(DOT.DirectStockOutQty,0) + ISNULL(Destroy,0) AS TotalIssue,		 TotalQuantity - (ISNULL(B2BS.Challan,0) + ISNULL(SoldQuantity,0) + ISNULL(BookForDelivery,0) 	 + ISNULL(SampleQty,0) + ISNULL(FRZ.DcFreeze,0) + ISNULL(FRZR.DcFreeze,0) + ISNULL(FRZB.DcFreeze,0) + ISNULL(DOT.DirectStockOutQty,0) + ISNULL(Destroy,0)) AS Closing,	 TotalQuantity - (StockQty + ISNULL(B2BS.Challan,0) + ISNULL(SoldQuantity,0) + ISNULL(BookForDelivery,0) 	 + ISNULL(SampleQty,0) + ISNULL(FRZ.DcFreeze,0) + ISNULL(FRZR.DcFreeze,0) + ISNULL(FRZB.DcFreeze,0) + ISNULL(DOT.DirectStockOutQty,0) + ISNULL(Destroy,0)) Error		FROM tblDCStore AS DCS with(nolock) 	LEFT JOIN tblCompanyUnit AS UNT ON DCS.ComUnitId = UNT.ComUnitId		-- B2B Stock Out		LEFT JOIN (SELECT CLD.DcStoreId,SUM(Quantity) Challan FROM dbo.tblChalanDetail AS CLD  with(nolock) GROUP BY CLD.DcStoreId) AS B2BS ON DCS.DcStoreId = B2BS.DcStoreId 		-- Sold Qty		LEFT JOIN (SELECT INVD.DcStoreId,SUM(INVD.DeliveryQuantity) AS SoldQuantity FROM tblInvoiceDetail AS INVD with(nolock) 	WHERE INVD.DeliveryStatus IN ('Full','Partial') GROUP BY INVD.DcStoreId) AS DLQ ON DCS.DcStoreId = DLQ.DCStoreId 		-- Book for delivery		LEFT JOIN (SELECT INVD.DcStoreId,SUM(Quantity) AS BookForDelivery FROM tblInvoiceDetail AS INVD with(nolock) 	LEFT JOIN tblDCStore AS IDC with(nolock)  ON INVD.DCStoreId = IDC.DcStoreId	LEFT JOIN tblInvoice AS INV ON INVD.InvoiceId = INV.Invoiceid	WHERE INVD.DeliveryStatus IS NULL GROUP BY INVD.DcStoreId) AS IBD 	ON DCS.DcStoreId = IBD.DcStoreId 		-- Sample Issue		LEFT JOIN (SELECT ST.DCStoreId,SUM(ST.Quantity)*-1 AS SampleQty FROM tblSampleIssueDetail AS D
	LEFT JOIN tblSampleIssueTranscation AS ST ON D.OrderDetailId = ST.IssueDetailId
	LEFT JOIN tblSampleIssue AS M ON D.OrderId = M.OrderId
	GROUP BY ST.DCStoreId) AS SMP ON DCS.DCStoreId = SMP.DCStoreId			-- Direct Stock Out		LEFT JOIN (SELECT DCStoreId,SUM(StockOutQty) AS DirectStockOutQty FROM tblDirectStockOut  GROUP BY DCStoreId) AS DOT ON DCS.DCStoreId = DOT.DCStoreId			-- Freeze Stock (Delivery Confirm Return, B2B Return, STO Retun)		LEFT JOIN (SELECT DISTINCT FZ.DcStoreId,SUM(FZ.StockQty) AS DcFreeze FROM tblDCStoreFreeze AS FZ	LEFT JOIN tblDCStore AS FZDC with(nolock)  ON FZ.DCStoreId = FZDC.DcStoreId 	WHERE FZ.StockCondition IS NULL OR FZ.StockCondition = 'ReturnStock' GROUP BY FZ.DcStoreId) AS FRZ ON DCS.DcStoreId = FRZ.DcStoreId 		-- Freeze Stock (Restricted)		LEFT JOIN (SELECT DISTINCT FZ.DcStoreId,SUM(FZ.StockQty) AS DcFreeze FROM tblDCStoreFreeze AS FZ	LEFT JOIN tblDCStore AS FZDC with(nolock)  ON FZ.DCStoreId = FZDC.DcStoreId 	WHERE FZ.StockCondition = 'Restricted' GROUP BY FZ.DcStoreId) AS FRZR ON DCS.DcStoreId = FRZR.DcStoreId 		--Freeze Stock (Blocked)		LEFT JOIN (SELECT DISTINCT FZ.DcStoreId,SUM(FZ.StockQty) AS DcFreeze FROM tblDCStoreFreeze AS FZ	LEFT JOIN tblDCStore AS FZDC with(nolock)  ON FZ.DCStoreId = FZDC.DcStoreId 	WHERE FZ.StockCondition = 'Blocked' GROUP BY FZ.DcStoreId) AS FRZB ON DCS.DcStoreId = FRZB.DcStoreId 		-- Destroy		LEFT JOIN (SELECT DCStoreId,SUM(StockOutQty) AS Destroy FROM tblDestroyProduct AS DT	LEFT JOIN tblDCStoreFreeze AS DFZ ON DT.DcFreezeId = DFZ.DCStoreFreezeId 	GROUP BY DCStoreId) AS DST ON DCS.DcStoreId = DST.DcStoreId 			WHERE  TotalQuantity - (StockQty + ISNULL(B2BS.Challan,0) + ISNULL(SoldQuantity,0) + ISNULL(BookForDelivery,0) 	 + ISNULL(SampleQty,0) + ISNULL(FRZ.DcFreeze,0) + ISNULL(FRZR.DcFreeze,0) + ISNULL(FRZB.DcFreeze,0) + ISNULL(DOT.DirectStockOutQty,0) + ISNULL(Destroy,0)) > 0		ORDER BY ReceiveDate";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable AccountsIntegration()
        {
            string query = @"SELECT INV.InvoiceId,INV.InvoiceNo, SUM(INVD.TotalPrice) AS ProformaValue,VCR.ProformaId,VCR.PostingValue ,VoucherMasterId, 'Proforma' VType,VoucherCode
FROM tblInvoice AS INV 
LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
LEFT JOIN (
    SELECT DVB.VoucherMasterId,DVB.VoucherCode ,DVB.ProformaId, SUM(DVD.DebitAmountBDT) AS PostingValue 
    FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster AS DVB
    LEFT JOIN CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail AS DVD ON DVB.VoucherMasterId = DVD.VoucherMasterId
    WHERE PostingDate > '2024-07-01'  AND Narration LIKE '%Proforma Invoice%' GROUP BY DVB.ProformaId,DVB.VoucherMasterId, DVB.VoucherCode
) AS VCR ON INV.InvoiceId = VCR.ProformaId
WHERE InvoiceDate > '2024-07-01'  AND CompanyId = 16
GROUP BY INV.InvoiceId,INV.InvoiceNo, VCR.ProformaId, VCR.PostingValue,VoucherMasterId, VoucherCode
HAVING VCR.ProformaId IS NULL OR SUM(INVD.TotalPrice) != VCR.PostingValue

UNION ALL

SELECT INV.InvoiceId,DelivaryInvoiceNo ,SUM(INVD.DeliveryTotalPrice) AS ProformaValue,VCR.ProformaId,VCR.PostingValue,VoucherMasterId, 'Sales' VType,VoucherCode
FROM tblInvoice AS INV 
LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
LEFT JOIN (
   SELECT DVB.VoucherMasterId,DVB.ProformaId,DVB.VoucherCode,SUM(DVD.DebitAmountBDT) AS PostingValue FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster AS DVB
LEFT JOIN CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail AS DVD ON DVB.VoucherMasterId = DVD.VoucherMasterId
WHERE PostingDate > '2024-07-01'  AND (Narration LIKE '%Delivery Sales Confirmation Full%' OR Narration LIKE '%Delivery Sales Confirmation Partial%')
GROUP BY DVB.ProformaId,DVB.TotalAmountBDT,DVB.VoucherMasterId, DVB.VoucherCode
) AS VCR ON INV.InvoiceId = VCR.ProformaId
WHERE InvoiceDate > '2024-07-01'  AND DelivaryInvoiceNo IS NOT NULL AND CompanyId = 16
GROUP BY INV.InvoiceId,DelivaryInvoiceNo, VCR.ProformaId,DelivaryInvoiceNo, VCR.PostingValue,VoucherMasterId,VoucherCode
HAVING SUM(INVD.DeliveryTotalPrice) > 0 AND (VCR.ProformaId IS NULL OR SUM(INVD.DeliveryTotalPrice) != ISNULL(VCR.PostingValue,0))

UNION ALL

SELECT CPD.CustPayDetailId,CP.PaymentCode,CPD.PaymentAmount+CPD.Discount+CPD.AIT AS PaymentAmount,VCR.CustPayDetailId,VCR.PostingValue,VoucherMasterId,  'Payment' VType, VoucherCode FROM tblCustPayDetail AS CPD 
LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId
LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId
LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId

LEFT JOIN (SELECT DVB.VoucherMasterId,DVB.VoucherCode,DVB.CustPayDetailId,SUM(DVD.DebitAmountBDT) AS PostingValue FROM CSTL_ERP_ACCDB..tblDebitCreditVoucherMaster AS DVB
LEFT JOIN CSTL_ERP_ACCDB..tblDebitCreditVoucherDetail AS DVD ON DVB.VoucherMasterId = DVD.VoucherMasterId
WHERE PostingDate > '2024-07-01'  AND Narration LIKE '%Customer Payment%' AND CompanyInfoId = 16
GROUP BY DVB.VoucherMasterId,DVB.CustPayDetailId,DVB.TotalAmountBDT,DVB.VoucherCode) AS VCR ON VCR.CustPayDetailId = CPD.CustPayDetailId

WHERE PaymentDate > '2024-07-01'  AND CompanyId = 16 
AND (VCR.CustPayDetailId IS NULL OR (CPD.PaymentAmount+CPD.Discount+CPD.AIT) != VCR.PostingValue) ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
