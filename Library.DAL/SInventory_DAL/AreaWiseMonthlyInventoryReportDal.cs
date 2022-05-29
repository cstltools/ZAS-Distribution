using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class AreaWiseMonthlyInventoryReportDal
    {
        private readonly ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

//        public DataTable GetAreaWiseMonthlyInventoryReport(string fromDate, string toDate, string branchId)
//        {

//            const string query = @"SELECT ProductCode ,ProductName ,StorageLocation AS BaseUnit,PackSize AS OpeningStock ,BatchNo AS ReceiveFromCentralWarehouse,
//                                  Quantity AS ReceiveFromAreaOfficeInterTransfer,ExpDate AS TotalReceived,ReceiveDate AS IssuedToSales,ChalanNo AS IssuedToProductBonus,
//ChalanDate AS IssuedToAreaOfficeInterTransfer,
//                                  InternalNoteNo AS IssuedToCentralWarehouse,StockInQty AS IssuedToDamageAndOthers,UnitPrice AS TotalIssued,MfgDate AS ClosingStock FROM dbo.tblCentralStore";

//            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
//        }


        public DataTable GetAreaWiseMonthlyInventoryReport(string fromDate, string toDate, string CiD, string DC, string comId)
        {
            
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@FromDate", fromDate));
            aSqlParameterList.Add(new SqlParameter("@ToDate", toDate));
            aSqlParameterList.Add(new SqlParameter("@CompanyId", comId));
            aSqlParameterList.Add(new SqlParameter("@DcId", CiD));
            aSqlParameterList.Add(new SqlParameter("@DCName", DC));

            return aCommonInternalDal.GetDataTableAction("sp_DCBinCard", aSqlParameterList, "SSIDB");
            
            
           
  //          string query = "SELECT P.ProductCode as ProductCode,P.ProductName as ProductName ,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate,'" + DC + "' AS DC,P.PackSize as BaseUnit,ISNULL(vTblOB.Quantity,0)OpeningStock   ,ISNULL(vTblStoReceive.TotalStockReceiveQty,0) AS ReceiveFromCentralWarehouse   ,ISNULL(vTblChallanReceive.TotalStockReceiveQty,0) "
  //+" AS ReceiveFromAreaOfficeInterTransfer    ,ISNULL(vTblStockReceive.TotalStockReceiveQty,0) AS TotalReceived,ISNULL(vTblChallanTransit.Challan,0) AS IntransitWHB2B,ISNULL(vTblsalesInTransit.Sales,0) AS SalesInTransit  ,ISNULL(vTblsales.Sales,0) AS IssuedToSales, ISNULL(vbBonus.BonusQty,0) AS IssuedToBonus   ,ISNULL(vTblProductBonus.Sales,0) AS IssuedToProductBonus   ,ISNULL(vTblChallan.Challan,0) AS IssuedToAreaOfficeInterTransfer    ,ISNULL(vTblFreez.Freeze,0) AS IssuedToDamageAndOthers,    ISNULL(vTblFreez2.Freeze,0) AS Blocked, ISNULL(vTblSample.SampleIssue,0) AS SampleIssue,   "
  //+" ((ISNULL(vTblOB.Quantity,0)+ISNULL(vTblStoReceive.TotalStockReceiveQty,0)+ISNULL(vTblChallanReceive.TotalStockReceiveQty,0)+ ISNULL(vTblChallanTransit.Challan,0))-(ISNULL(vTblsales.Sales,0) +ISNULL(vTblProductBonus.Sales,0)+ISNULL(vTblChallan.Challan,0)+ ISNULL(vTblSample.SampleIssue,0)+ ISNULL(vbBonus.BonusQty,0) + ISNULL(vTblsalesInTransit.Sales,0)))  as ClosingStock ,"
  //+" (ISNULL(vTblOB.Quantity,0)+ISNULL(vTblStoReceive.TotalStockReceiveQty,0)+ISNULL(vTblChallanReceive.TotalStockReceiveQty,0) + "
  //+" ISNULL(vTblChallanTransit.Challan,0)) AS TotalStock,(ISNULL(vTblsales.Sales,0) +ISNULL(vTblProductBonus.Sales,0)+ISNULL(vTblChallan.Challan,0) + "
  //+" ISNULL(vTblSample.SampleIssue,0) + ISNULL(vbBonus.BonusQty,0) + ISNULL(vTblsalesInTransit.Sales,0)) AS TotalIssue, "
  //+" (ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0)) AS IssuedValue,"
  //+" (ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)) AS TotalRcvValue,"
  //+" CASE WHEN (ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0)) > (ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)) THEN ((ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0))-(ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0))) ELSE ((ISNULL(vTblOB.Value,0)+ISNULL(vTblStoReceive.Value,0)+ISNULL(vTblChallanReceive.Value,0)+ ISNULL(vTblChallanTransit.Value,0)) - (ISNULL(vTblsales.Value,0) +ISNULL(vTblProductBonus.Value,0)+ISNULL(vTblChallan.Value,0)+ ISNULL(vTblSample.Value,0)+ ISNULL(vbBonus.Value,0) + ISNULL(vTblsalesInTransit.Value,0))) END as ClosingStockValue "
  //+" FROM dbo.tblProduct P with(nolock) "    
  //+" LEFT JOIN (SELECT tblDCStore_OpeningBalance.ProductCode,SUM(StockQty)Quantity, "
  //+" SUM(((CTRS.UnitPrice*tblDCStore_OpeningBalance.StockQty) + (CTRS.VATPerUnit*tblDCStore_OpeningBalance.StockQty))) AS Value FROM dbo.tblDCStore_OpeningBalance  with(nolock) "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = tblDCStore_OpeningBalance.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId "
  //+" WHERE tblDCStore_OpeningBalance.ComUnitId="+ CiD + " AND DCOpeningBalanceDate='" + fromDate + "' GROUP BY tblDCStore_OpeningBalance.ProductCode) vTblOB ON vTblOB.ProductCode = P.ProductCode "      
  //+" LEFT JOIN (SELECT DCS.ProductCode,SUM(TotalQuantity)TotalStockReceiveQty, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty))) AS Value FROM dbo.tblDCStore AS DCS with(nolock) "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId WHERE ComUnitId="+ CiD + "AND ChalanDetailsId IS  NULL AND StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY DCS.ProductCode) vTblStoReceive ON vTblStoReceive.ProductCode = P.ProductCode  "   
  //+" LEFT JOIN (SELECT DCS.ProductCode,SUM(TotalQuantity)TotalStockReceiveQty, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty))) AS Value FROM dbo.tblDCStore AS DCS with(nolock) "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId  " 
  //+" WHERE DCS.ComUnitId = "+ CiD + " AND ChalanDetailsId IS NOT NULL AND StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY DCS.ProductCode) vTblChallanReceive ON vTblChallanReceive.ProductCode = P.ProductCode "     
  //+" LEFT JOIN (SELECT DCS.ProductCode,SUM(TotalQuantity)TotalStockReceiveQty, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblDCStore DCS  with(nolock)  " 
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId WHERE ComUnitId="+ CiD + " AND StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY DCS.ProductCode) "
  //+" vTblStockReceive ON vTblStockReceive.ProductCode = P.ProductCode  "    
  //+" LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(DeliveryQuantity)Sales, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock) "
  //+" INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId   "
  //+" WHERE tblInvoice.ComUnitId = "+ CiD + " AND DeliveryStatus IN ('Full','Partial') AND "
  //+" InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY tblInvoiceDetail.ProductCode) "
  //+" vTblsales ON vTblsales.ProductCode = P.ProductCode  "     
  //+" LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(BonusQuantity) BonusQty, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock) "
  //+" INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId "  
  //+" WHERE tblInvoice.ComUnitId = "+ CiD + " AND DeliveryStatus IN (NULL,'Full','Partial') AND "
  //+" InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY tblInvoiceDetail.ProductCode) "
  //+" AS vbBonus ON vbBonus.ProductCode = P.ProductCode   "  
  //+" LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(DeliveryQuantity)Sales,"
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock) "
  //+" INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId   "
  //+" WHERE tblInvoice.ComUnitId = "+ CiD + " AND OrderDetailsId=0 and InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY tblInvoiceDetail.ProductCode) vTblProductBonus ON vTblProductBonus.ProductCode = P.ProductCode   "   
  //+" LEFT JOIN (SELECT tblInvoiceDetail.ProductCode,SUM(tblInvoiceDetail.TotalQuantity)Sales, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblInvoiceDetail  with(nolock) "
  //+" INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId   "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblInvoiceDetail.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId "
  //+" WHERE tblInvoice.ComUnitId = "+ CiD + " AND "
  //+" DeliveryStatus IS NULL and InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY tblInvoiceDetail.ProductCode) "
  //+" vTblsalesInTransit ON vTblsalesInTransit.ProductCode = P.ProductCode "      
  //+" LEFT JOIN (SELECT tblChalanDetail.ProductCode,SUM(tblChalanDetail.Quantity)Challan, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblChalanDetail  with(nolock) "
  //+" INNER JOIN dbo.tblChalanInfo ON tblChalanDetail.ChalanId=tblChalanInfo.ChalanId "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblChalanDetail.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId " 
  //+" WHERE FromComUnitId = "+ CiD + " AND  tblChalanInfo.ChalanDate between '" + fromDate + "' and '" + toDate + "' AND IsDeliver = 'True' GROUP BY tblChalanDetail.ProductCode) vTblChallan ON vTblChallan.ProductCode = P.ProductCode  "    
  //+" LEFT JOIN (SELECT tblChalanDetail.ProductCode,SUM(tblChalanDetail.Quantity)Challan,SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblChalanDetail  with(nolock) "
  //+" INNER JOIN dbo.tblChalanInfo ON tblChalanDetail.ChalanId=tblChalanInfo.ChalanId "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblChalanDetail.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId  "   
  //+" WHERE FromComUnitId = "+ CiD + " AND  tblChalanInfo.ChalanDate between '" + fromDate + "' and '" + toDate + "' AND IsDeliver = 'False' GROUP BY tblChalanDetail.ProductCode) vTblChallanTransit ON vTblChallanTransit.ProductCode = P.ProductCode "   
  //+" LEFT JOIN (SELECT tblDCStoreFreeze.ProductCode,(SUM(tblDCStoreFreeze.StockQty)+SUM(tblDCStoreFreeze.DamageQty))Freeze, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblDCStoreFreeze  with(nolock)  "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblDCStoreFreeze.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId   "
  //+" WHERE tblDCStoreFreeze.ComUnitId ="+ CiD + " and  tblDCStoreFreeze.[StockCondition]='Restricted' and tblDCStoreFreeze.StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY tblDCStoreFreeze.ProductCode) vTblFreez ON vTblFreez.ProductCode = P.ProductCode  " 
  //+" LEFT JOIN (SELECT tblDCStoreFreeze.ProductCode,(SUM(tblDCStoreFreeze.StockQty)+SUM(tblDCStoreFreeze.DamageQty))Freeze, "
  //+" SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblDCStoreFreeze  with(nolock) "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = tblDCStoreFreeze.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId   "
  //+" WHERE tblDCStoreFreeze.ComUnitId ="+ CiD + " and  tblDCStoreFreeze.[StockCondition]='Blocked' and tblDCStoreFreeze.StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY tblDCStoreFreeze.ProductCode) vTblFreez2 ON vTblFreez2.ProductCode = P.ProductCode "
  //+" LEFT JOIN (SELECT SMPD.ProductCode,SUM(SMPD.Quantity) SampleIssue,SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROm tblSampleIssue AS SMPI "
  //+" LEFT JOIN tblSampleIssueDetail AS SMPD ON SMPI.OrderId = SMPD.OrderId "
  //+" LEFT JOIN dbo.tblSampleIssueTranscation AS ST ON ST.IssueDetailId = SMPD.OrderDetailId "
  //+" INNER JOIN tblDCStore AS DCS ON DCS.DCStoreId = ST.DCStoreId "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+" INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId WHERE ActionStatus IN ('Accepted') "
  //+" AND ApprovedDate BETWEEN '" + fromDate + "' and '" + toDate + "' AND SMPI.ComUnitId = 1 GROUP BY SMPD.ProductCode) "
  //+" vTblSample ON vTblSample.ProductCode = P.ProductCode "   
  //+" LEFT JOIN (SELECT sum(StockQty)Closingstock,DCS.ProductCode,SUM(((CTRS.UnitPrice*DCS.StockQty) + (CTRS.VATPerUnit*DCS.StockQty) )) AS Value FROM dbo.tblDCStore AS DCS  with(nolock) "
  //+" INNER JOIN tblStockInTransfar AS TNS ON TNS.StockInTransfarId = DCS.StockInTransfarId "
  //+ " INNER JOIN tblCentralStore AS CTRS ON TNS.ReceiveId = CTRS.ReceiveId  " 
  //+" WHERE ComUnitId="+ CiD + " group by DCS.ProductCode) currentStock ON currentStock.ProductCode = P.ProductCode WHERE P.CompanyId =" + comId + " ORDER BY P.ProductCode";

     //       string query = "SELECT P.ProductCode as ProductCode,P.ProductName as ProductName ,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate,'" + DC + "' AS DC,P.PackSize as BaseUnit,ISNULL(vTblOB.Quantity,0)OpeningStock " +
     //    "  ,ISNULL(vTblStoReceive.TotalStockReceiveQty,0) AS ReceiveFromCentralWarehouse " +
     //    "  ,ISNULL(vTblChallanReceive.TotalStockReceiveQty,0) AS ReceiveFromAreaOfficeInterTransfer " +
     //   "   ,ISNULL(vTblStockReceive.TotalStockReceiveQty,0) AS TotalReceived,ISNULL(vTblChallanTransit.Challan,0) AS IntransitWHB2B,ISNULL(vTblsalesInTransit.Sales,0) AS SalesInTransit" +
     //    "  ,ISNULL(vTblsales.Sales,0) AS IssuedToSales, ISNULL(vbBonus.BonusQty,0) AS IssuedToBonus " +
     //    "  ,ISNULL(vTblProductBonus.Sales,0) AS IssuedToProductBonus " +
     //    "  ,ISNULL(vTblChallan.Challan,0) AS IssuedToAreaOfficeInterTransfer " +
     //   "   ,ISNULL(vTblFreez.Freeze,0) AS IssuedToDamageAndOthers,    ISNULL(vTblFreez2.Freeze,0) AS Blocked, ISNULL(vTblSample.SampleIssue,0) AS SampleIssue," +
     //   "   ((ISNULL(vTblOB.Quantity,0)+ISNULL(vTblStoReceive.TotalStockReceiveQty,0)+ISNULL(vTblChallanReceive.TotalStockReceiveQty,0) + ISNULL(vTblChallanTransit.Challan,0))-(ISNULL(vTblsales.Sales,0) +ISNULL(vTblProductBonus.Sales,0)+ISNULL(vTblChallan.Challan,0)+ ISNULL(vTblSample.SampleIssue,0)+ ISNULL(vbBonus.BonusQty,0) + ISNULL(vTblsalesInTransit.Sales,0)))  as ClosingStock " +
     //   ",(ISNULL(vTblOB.Quantity,0)+ISNULL(vTblStoReceive.TotalStockReceiveQty,0)+ISNULL(vTblChallanReceive.TotalStockReceiveQty,0) + ISNULL(vTblChallanTransit.Challan,0)) AS TotalStock,(ISNULL(vTblsales.Sales,0) +ISNULL(vTblProductBonus.Sales,0)+ISNULL(vTblChallan.Challan,0) + ISNULL(vTblSample.SampleIssue,0) + ISNULL(vbBonus.BonusQty,0) + ISNULL(vTblsalesInTransit.Sales,0)) AS TotalIssue  " +
     //   "    FROM dbo.tblProduct P  with(nolock) " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(StockQty)Quantity FROM dbo.tblDCStore_OpeningBalance  with(nolock) WHERE ComUnitId='" + CiD + "' AND DCOpeningBalanceDate='" + fromDate + "' " +
     //   "    GROUP BY ProductCode) vTblOB ON vTblOB.ProductCode = P.ProductCode  " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(TotalQuantity)TotalStockReceiveQty  " +
     //   "    FROM dbo.tblDCStore  with(nolock) WHERE ComUnitId='" + CiD + "' " +
     //   "    AND ChalanDetailsId IS  NULL AND StockRcvDate between '" + fromDate + "' and '" + toDate + "'  " +
     //   "    GROUP BY ProductCode) vTblStoReceive ON vTblStoReceive.ProductCode = P.ProductCode  " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(TotalQuantity)TotalStockReceiveQty FROM dbo.tblDCStore  with(nolock) " +
     //   "    WHERE ComUnitId = '" + CiD + "' AND ChalanDetailsId IS NOT NULL AND StockRcvDate between '" + fromDate + "' and '" + toDate + "' " +
     //   "    GROUP BY ProductCode) vTblChallanReceive ON vTblChallanReceive.ProductCode = P.ProductCode " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(TotalQuantity)TotalStockReceiveQty FROM dbo.tblDCStore  with(nolock) WHERE ComUnitId='" + CiD + "' AND StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY ProductCode) vTblStockReceive ON vTblStockReceive.ProductCode = P.ProductCode " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(DeliveryQuantity)Sales FROM dbo.tblInvoiceDetail  with(nolock) INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId   WHERE ComUnitId = '" + CiD + "' AND DeliveryStatus IN ('Full','Partial') AND InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY ProductCode) vTblsales ON vTblsales.ProductCode = P.ProductCode " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(BonusQuantity) BonusQty FROM dbo.tblInvoiceDetail  with(nolock) INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId   WHERE ComUnitId = '" + CiD + "' AND DeliveryStatus IN (NULL,'Full','Partial') AND InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY ProductCode) AS vbBonus ON vbBonus.ProductCode = P.ProductCode " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(DeliveryQuantity)Sales FROM dbo.tblInvoiceDetail  with(nolock) INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId   WHERE ComUnitId = '" + CiD + "' AND OrderDetailsId=0 and InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY ProductCode) vTblProductBonus ON vTblProductBonus.ProductCode = P.ProductCode " +
     //"    LEFT JOIN (SELECT ProductCode,SUM(tblInvoiceDetail.TotalQuantity)Sales FROM dbo.tblInvoiceDetail  with(nolock) INNER JOIN dbo.tblInvoice ON tblInvoiceDetail.InvoiceId=tblInvoice.InvoiceId   WHERE ComUnitId = '" + CiD + "' AND DeliveryStatus IS NULL and InvoiceDate between '" + fromDate + "' and '" + toDate + "' GROUP BY ProductCode) vTblsalesInTransit ON vTblsalesInTransit.ProductCode = P.ProductCode " +
     //   "    LEFT JOIN (SELECT ProductCode,SUM(Quantity)Challan FROM dbo.tblChalanDetail  with(nolock) INNER JOIN dbo.tblChalanInfo ON tblChalanDetail.ChalanId=tblChalanInfo.ChalanId   WHERE FromComUnitId = '" + CiD + "' AND  ChalanDate between '" + fromDate + "' and '" + toDate + "' AND IsDeliver = 'True' GROUP BY ProductCode) vTblChallan ON vTblChallan.ProductCode = P.ProductCode" +
     //           "    LEFT JOIN (SELECT ProductCode,SUM(Quantity)Challan FROM dbo.tblChalanDetail  with(nolock) INNER JOIN dbo.tblChalanInfo ON tblChalanDetail.ChalanId=tblChalanInfo.ChalanId   WHERE FromComUnitId = '" + CiD + "' AND  ChalanDate between '" + fromDate + "' and '" + toDate + "' AND IsDeliver = 'False' GROUP BY ProductCode) vTblChallanTransit ON vTblChallanTransit.ProductCode = P.ProductCode" +
     //   "    LEFT JOIN (SELECT ProductCode,(SUM(StockQty)+SUM(DamageQty))Freeze FROM dbo.tblDCStoreFreeze  with(nolock)    WHERE ComUnitId='" + CiD + "' and  [StockCondition]='Restricted' and StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY ProductCode) vTblFreez ON vTblFreez.ProductCode = P.ProductCode" +
     //    "   LEFT JOIN (SELECT ProductCode,(SUM(StockQty)+SUM(DamageQty))Freeze FROM dbo.tblDCStoreFreeze  with(nolock)    WHERE ComUnitId='" + CiD + "' and  [StockCondition]='Blocked' and StockRcvDate between '" + fromDate + "' and '" + toDate + "' GROUP BY ProductCode) vTblFreez2 ON vTblFreez2.ProductCode = P.ProductCode" +
     //    " LEFT JOIN (SELECT SMPD.ProductCode,SUM(SMPD.Quantity) SampleIssue FROm tblSampleIssue AS SMPI" +
     //     " LEFT JOIN tblSampleIssueDetail AS SMPD ON SMPI.OrderId = SMPD.OrderId" +
     //     " WHERE ActionStatus IN ('Accepted') AND ApprovedDate BETWEEN '" + fromDate + "' and '" + toDate + "' AND ComUnitId = " + CiD + 
     //     "GROUP BY SMPD.ProductCode) vTblSample ON vTblSample.ProductCode = P.ProductCode " +
     //    "   LEFT JOIN (SELECT sum(StockQty)Closingstock,ProductCode FROM dbo.tblDCStore  with(nolock)    WHERE ComUnitId='" + CiD + "' group by ProductCode) currentStock ON currentStock.ProductCode = P.ProductCode WHERE P.CompanyId = " + comId + " ORDER BY P.ProductCode";

            //return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public void GetBranchList(DropDownList ddl)
        {
            const string query = @"SELECT ComUnitId,ComUnitName FROM dbo.tblCompanyUnit";
            aCommonInternalDal.LoadDropDownValue(ddl, "ComUnitName", "ComUnitId", query, "SSIDB");
        }
    }
}
