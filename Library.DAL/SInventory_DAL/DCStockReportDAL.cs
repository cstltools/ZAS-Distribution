using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class DCStockReportDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public void LoadCompanyUnit(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as ComInfo  from tblCompanyUnit ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComInfo", "ComUnitId", queryStr);
        }

        public void LoadCompanyUnit(DropDownList ddl,string userid)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as ComInfo  from tblCompanyUnit WHERE " +
                              " ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId='" + userid.Trim() + "')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComInfo", "ComUnitId", queryStr);
        }
        
        public DataTable DCReportMainDataDAL(string comUnitId)
        {
            string query = @"select * from tblCompanyUnit where ComUnitId='"+comUnitId.Trim()+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable DCReportDetailDataDAL(string comUnitId)
        {
            string query = "";
            if (comUnitId !="")
            {
                query = @"

           SELECT  
               --vTblTotal.ComUnitId,
               Cu.ComUnitCode,Cu.ComUnitName,vTblTotal.ProductCode ,
                vTblTotal.ProductName ,
                vTblTotal.PackSize ,
                vTblTotal.BatchNo ,
                vTblTotal.MfgDate ,
                vTblTotal.ExpDate ,
                SUM(vTblTotal.AvailableQty)AQty ,
                SUM(vTblTotal.BookFDel)BookFDel ,
                SUM(vTblTotal.Tqty)Tqty ,
                SUM(vTblTotal.RQty)RQty ,
                SUM(vTblTotal.BQty)BQty ,
                SUM(vTblTotal.ReturnRcvQty)ReturnRcvQty ,
 				SUM(vTblTotal.AvailableQty * vTblTotal.Price) AS AvialableValue,
 				SUM(vTblTotal.RQty * vTblTotal.Price) AS RestrictedValue,
 				SUM(vTblTotal.Tqty * vTblTotal.Price) AS TransitValue,
 				SUM(vTblTotal.BQty * vTblTotal.Price) AS BlockedValue,
                SUM(vTblTotal.ReturnRcvQty * vTblTotal.Price) AS ReturnRcvValue,
 				(SUM(vTblTotal.RQty * vTblTotal.Price) + SUM(vTblTotal.BQty * vTblTotal.Price) + SUM(vTblTotal.AvailableQty * vTblTotal.Price)) AS GrandTotal

                FROM 
                (
                       SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Available' AND UP.IsActive=1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblChalanDetail cd ON dcs.ChalanDetailsId=CD.ChalanDetailsId
                --INNER JOIN tblDCStore dc ON CD.DCStoreId=DC.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
               -- INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = dc.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Available' AND UP.IsActive=1
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,ID.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.TotalQuantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                I.ComUnitId,0 AS ReturnRcvQty
                FROM dbo.tblInvoice I 
                INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                --INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE I.DelivaryInvoiceNo IS NULL AND UP.IsActive=1
                UNION ALL
                  SELECT p.ProductCode,p.ProductName,P.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.Quantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                cu.ComUnitId,0 AS ReturnRcvQty
                FROM dbo.tblChalanInfo I 
                INNER JOIN dbo.tblChalanDetail ID ON ID.ChalanId = I.ChalanId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN   dbo.tblCompanyUnit cu ON i.FromComUnitCode = cU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE I.IsDeliver IS NULL AND UP.IsActive=1
                
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCS.StockQty AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Restricted' AND UP.IsActive=1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,DCS.StockQty AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty 
                FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Blocked' AND UP.IsActive=1
                UNION ALL
                
                
                SELECT p.ProductCode,p.ProductName,ST.PackSize,ST.
                BatchNo,ST.MfgDate,ST.ExpDate,0 AS AvailableQty,0 AS BookFDel,ST.Quantity AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                R.ComUnitId,0 AS ReturnRcvQty FROM dbo.tblRequisition R 
                INNER JOIN dbo.tblStockInTransfar ST ON ST.ReqId = R.ReqId
                INNER JOIN dbo.tblProduct p ON p.ProductCode = ST.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                WHERE  R.ReceiveIssue IS NULL AND UP.IsActive=1
                UNION ALL

                SELECT p.ProductCode,p.ProductName,D.PackSize,D.
                                BatchNo,D.MfgDate,D.ExpDate,0 AS AvailableQty,0 AS BookFDel,CD.Quantity AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                                CU.ComUnitId,0 AS ReturnRcvQty FROM dbo.tblChalanInfo C 
                INNER JOIN dbo.tblChalanDetail CD ON CD.ChalanId = C.ChalanId
                INNER JOIN dbo.tblDCStore D ON D.DCStoreId = CD.DCStoreId
                INNER JOIN dbo.tblCompanyUnit CU ON C.ToComUnitCode=CU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON p.ProductCode = CD.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                WHERE C.IsDeliver ='False' AND UP.IsActive=1
                
                UNION ALL
                SELECT p.ProductCode, p.ProductName, DCS.PackSize, DCS.BatchNo, DCS.MfgDate, DCS.ExpDate,
                       DCS.StockQty AS AvailableQty, 0 AS BookFDel, 0 AS Tqty, 0 AS RQty, 0 AS BQty,
                       (ISNULL(NTD.Price, 0) + ISNULL(NTD.VAT, 0)) AS Price, DCS.ComUnitId, DCS.StockQty AS ReturnRcvQty
                FROM dbo.tblDCStore DCS
                INNER JOIN dbo.tblProduct p ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(p.ProductCode)))
                INNER JOIN dbo.tblNonTranscationalInvoiceDetail NTD ON NTD.InvReturnDetailID = DCS.NonTranscationalInvReturnDetailID
                WHERE DCS.StockQty > 0
                  AND DCS.StockCondition = 'Available'
                  AND ISNULL(DCS.NonTranscationalInvReturnDetailID, 0) > 0
                  AND ISNULL(DCS.ReceiveId, 0) = 0
                  AND ISNULL(DCS.TempReceiveId, 0) = 0) AS vTblTotal
				
				INNER JOIN dbo.tblCompanyUnit Cu ON Cu.ComUnitId = vTblTotal.ComUnitId

                where vTblTotal.ComUnitId='" + comUnitId + "'GROUP BY vTblTotal.ComUnitId,Cu.ComUnitCode,Cu.ComUnitName,vTblTotal.ProductCode, vTblTotal.ProductName ,vTblTotal.PackSize ,vTblTotal.BatchNo ,vTblTotal.MfgDate ,vTblTotal.ExpDate ORDER BY vTblTotal.ProductCode "; 
                

            }
            else
            {
                query = @"

                SELECT  
               --vTblTotal.ComUnitId,
               Cu.ComUnitCode,Cu.ComUnitName,vTblTotal.ProductCode ,
                vTblTotal.ProductName ,
                vTblTotal.PackSize ,
                vTblTotal.BatchNo ,
                vTblTotal.MfgDate ,
                vTblTotal.ExpDate ,
                SUM(vTblTotal.AvailableQty)AQty ,
                SUM(vTblTotal.BookFDel)BookFDel ,
                SUM(vTblTotal.Tqty)Tqty ,
                SUM(vTblTotal.RQty)RQty ,
                SUM(vTblTotal.BQty)BQty ,
                SUM(vTblTotal.ReturnRcvQty)ReturnRcvQty ,
 				SUM(vTblTotal.AvailableQty * vTblTotal.Price) AS AvialableValue,
 				SUM(vTblTotal.RQty * vTblTotal.Price) AS RestrictedValue,
 				SUM(vTblTotal.Tqty * vTblTotal.Price) AS TransitValue,
 				SUM(vTblTotal.BQty * vTblTotal.Price) AS BlockedValue,
                SUM(vTblTotal.ReturnRcvQty * vTblTotal.Price) AS ReturnRcvValue,
 				(SUM(vTblTotal.RQty * vTblTotal.Price) + SUM(vTblTotal.BQty * vTblTotal.Price) + SUM(vTblTotal.AvailableQty * vTblTotal.Price)) AS GrandTotal

                FROM 
                (
                       SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Available' AND UP.IsActive=1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblChalanDetail cd ON dcs.ChalanDetailsId=CD.ChalanDetailsId
                --INNER JOIN tblDCStore dc ON CD.DCStoreId=DC.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
               -- INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = dc.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Available' AND UP.IsActive=1
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,ID.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.TotalQuantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                I.ComUnitId,0 AS ReturnRcvQty
                FROM dbo.tblInvoice I 
                INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                --INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE I.DelivaryInvoiceNo IS NULL AND UP.IsActive=1
                UNION ALL
                  SELECT p.ProductCode,p.ProductName,P.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.Quantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                cu.ComUnitId,0 AS ReturnRcvQty
                FROM dbo.tblChalanInfo I 
                INNER JOIN dbo.tblChalanDetail ID ON ID.ChalanId = I.ChalanId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN   dbo.tblCompanyUnit cu ON i.FromComUnitCode = cU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE I.IsDeliver IS NULL AND UP.IsActive=1
                
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCS.StockQty AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Restricted' AND UP.IsActive=1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,DCS.StockQty AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS ReturnRcvQty 
                FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Blocked' AND UP.IsActive=1
                UNION ALL
                
                
                SELECT p.ProductCode,p.ProductName,ST.PackSize,ST.
                BatchNo,ST.MfgDate,ST.ExpDate,0 AS AvailableQty,0 AS BookFDel,ST.Quantity AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                R.ComUnitId,0 AS ReturnRcvQty FROM dbo.tblRequisition R 
                INNER JOIN dbo.tblStockInTransfar ST ON ST.ReqId = R.ReqId
                INNER JOIN dbo.tblProduct p ON p.ProductCode = ST.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                WHERE  R.ReceiveIssue IS NULL AND UP.IsActive=1
                UNION ALL

                SELECT p.ProductCode,p.ProductName,D.PackSize,D.
                                BatchNo,D.MfgDate,D.ExpDate,0 AS AvailableQty,0 AS BookFDel,CD.Quantity AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                                CU.ComUnitId,0 AS ReturnRcvQty FROM dbo.tblChalanInfo C 
                INNER JOIN dbo.tblChalanDetail CD ON CD.ChalanId = C.ChalanId
                INNER JOIN dbo.tblDCStore D ON D.DCStoreId = CD.DCStoreId
                INNER JOIN dbo.tblCompanyUnit CU ON C.ToComUnitCode=CU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON p.ProductCode = CD.ProductCode
 				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                WHERE C.IsDeliver ='False' AND UP.IsActive=1
                
                UNION ALL
                SELECT p.ProductCode, p.ProductName, DCS.PackSize, DCS.BatchNo, DCS.MfgDate, DCS.ExpDate,
                       DCS.StockQty AS AvailableQty, 0 AS BookFDel, 0 AS Tqty, 0 AS RQty, 0 AS BQty,
                       (ISNULL(NTD.Price, 0) + ISNULL(NTD.VAT, 0)) AS Price, DCS.ComUnitId, DCS.StockQty AS ReturnRcvQty
                FROM dbo.tblDCStore DCS
                INNER JOIN dbo.tblProduct p ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(p.ProductCode)))
                INNER JOIN dbo.tblNonTranscationalInvoiceDetail NTD ON NTD.InvReturnDetailID = DCS.NonTranscationalInvReturnDetailID
                WHERE DCS.StockQty > 0
                  AND DCS.StockCondition = 'Available'
                  AND ISNULL(DCS.NonTranscationalInvReturnDetailID, 0) > 0
                  AND ISNULL(DCS.ReceiveId, 0) = 0
                  AND ISNULL(DCS.TempReceiveId, 0) = 0) AS vTblTotal
				
				INNER JOIN dbo.tblCompanyUnit Cu ON Cu.ComUnitId = vTblTotal.ComUnitId
                 GROUP BY vTblTotal.ComUnitId,Cu.ComUnitCode,Cu.ComUnitName,vTblTotal.ProductCode, vTblTotal.ProductName ,vTblTotal.PackSize ,vTblTotal.BatchNo ,vTblTotal.MfgDate ,vTblTotal.ExpDate ORDER BY vTblTotal.ProductCode"; 																
																		               
            }
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable DCWiseCountryReportDetailDataDAL()
        {
            string query = @"select * from View_ProDCStock order by ProductCode";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable WHReportDetailDataDAL()
        {
            string query = "";
            {
                query = @"

                  
                  SELECT
	vTblTotal.WearhouseCode,vTblTotal.WearhouseName,
   vTblTotal.ProductCode ,
       vTblTotal.ProductName ,
       vTblTotal.PackSize ,
       vTblTotal.BatchNo ,
       vTblTotal.MfgDate ,
       vTblTotal.ExpDate 
     ,
       SUM(vTblTotal.AvailableQty)AQty ,
       SUM(vTblTotal.BookFDel)BookFDel ,
       SUM(vTblTotal.Tqty)Tqty ,
       SUM(vTblTotal.RQty)RQty ,
       SUM(vTblTotal.BQty)BQty 
    from

  (SELECT CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,CS.Quantity AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName
 FROM dbo.tblCentralStore CS 
  INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
  INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
where CS.Quantity>0
       UNION ALL
          


SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,ST.PickingQty AS Tqty,0 AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName
 FROM dbo.tblCentralStore CS 
 INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblStockInTransfar ST ON ST.ReceiveId = CS.ReceiveId

INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
WHERE  ST.IsTransfared is null and ST.IsIssue='OK'

 UNION ALL
SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,ST.PickingQty AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName FROM dbo.tblCentralStore CS 
INNER JOIN dbo.tblStockInTransfar ST ON ST.ReceiveId = CS.ReceiveId
INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
WHERE ST.IsIssue IS NULL and ST.IsTransfared IS NULL 
UNION ALL

SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCF.StockQty AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName FROM 
dbo.tblWhStoreFreeze DCF 
INNER JOIN 
dbo.tblCentralStore CS ON CS.ReceiveId = DCF.ReceiveId
INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
WHERE DCF.StockCondition='Restricted' and DCF.StockQty>0


UNION ALL

SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,DCF.StockQty AS BQty,WH.WearhouseCode,
	WH.WearhouseName FROM 
dbo.tblWhStoreFreeze DCF  INNER JOIN 
dbo.tblCentralStore CS ON CS.ReceiveId = DCF.ReceiveId
INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
       WHERE DCF.StockCondition='Blocked'  and DCF.StockQty>0
) AS vTblTotal
                
       GROUP BY vTblTotal.ProductCode ,
       vTblTotal.ProductName ,
       vTblTotal.PackSize ,
       vTblTotal.BatchNo ,
       vTblTotal.MfgDate ,
       vTblTotal.ExpDate,
	   vTblTotal.WearhouseCode,
	   vTblTotal.WearhouseName
HAVING (SUM(vTblTotal.AvailableQty) > 0 OR SUM(vTblTotal.BookFDel) > 0 OR SUM(vTblTotal.Tqty) > 0 OR SUM(vTblTotal.RQty) > 0 OR SUM(vTblTotal.BQty) > 0)"
                    ;
            }
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable WHReportDetailDataDAL(string Pcode)
        {
            string query = "";
            {
                query = @"

                  
                 SELECT
	vTblTotal.WearhouseCode,vTblTotal.WearhouseName,vTblTotal.WearhouseId,
   vTblTotal.ProductCode ,
       vTblTotal.ProductName ,
       vTblTotal.PackSize ,
       vTblTotal.BatchNo ,
       vTblTotal.MfgDate ,
       vTblTotal.ExpDate 
     ,
       SUM(vTblTotal.AvailableQty)AQty ,
       SUM(vTblTotal.BookFDel)BookFDel ,
       SUM(vTblTotal.Tqty)Tqty ,
       SUM(vTblTotal.RQty)RQty ,
       SUM(vTblTotal.BQty)BQty 
    from

  (SELECT CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,CS.Quantity AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName,WH.WearhouseId
 FROM dbo.tblCentralStore CS 
  INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
  INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
where CS.Quantity>0
       UNION ALL
          


SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,ST.PickingQty AS Tqty,0 AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName,WH.WearhouseId
 FROM dbo.tblCentralStore CS 
 INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblStockInTransfar ST ON ST.ReceiveId = CS.ReceiveId

INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
WHERE  ST.IsTransfared is null and ST.IsIssue='OK'

 UNION ALL
SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,ST.PickingQty AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName,WH.WearhouseId FROM dbo.tblCentralStore CS 
INNER JOIN dbo.tblStockInTransfar ST ON ST.ReceiveId = CS.ReceiveId
INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
WHERE ST.IsIssue IS NULL and ST.IsTransfared IS NULL 
UNION ALL

SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCF.StockQty AS RQty,0 AS BQty,WH.WearhouseCode,
	WH.WearhouseName,WH.WearhouseId FROM 
dbo.tblWhStoreFreeze DCF 
INNER JOIN 
dbo.tblCentralStore CS ON CS.ReceiveId = DCF.ReceiveId
INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
WHERE DCF.StockCondition='Restricted' and DCF.StockQty>0


UNION ALL

SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,DCF.StockQty AS BQty,WH.WearhouseCode,
	WH.WearhouseName,WH.WearhouseId FROM 
dbo.tblWhStoreFreeze DCF  INNER JOIN 
dbo.tblCentralStore CS ON CS.ReceiveId = DCF.ReceiveId
INNER JOIN dbo.tblWearhouse AS WH ON CS.WarehouseId = WH.WearhouseId
INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
       WHERE DCF.StockCondition='Blocked'  and DCF.StockQty>0
) AS vTblTotal " + Pcode + @" GROUP BY vTblTotal.ProductCode ,vTblTotal.ProductName ,vTblTotal.PackSize ,vTblTotal.BatchNo ,vTblTotal.MfgDate ,vTblTotal.ExpDate,vTblTotal.WearhouseCode,vTblTotal.WearhouseName,vTblTotal.WearhouseId
HAVING (SUM(vTblTotal.AvailableQty) > 0 OR SUM(vTblTotal.BookFDel) > 0 OR SUM(vTblTotal.Tqty) > 0 OR SUM(vTblTotal.RQty) > 0 OR SUM(vTblTotal.BQty) > 0)"
                    ;


//            string query = "";
//            {
//                query = @"
//
//                SELECT
//
//   vTblTotal.ProductCode ,
//       vTblTotal.ProductName ,
//       vTblTotal.PackSize ,
//       vTblTotal.BatchNo ,
//       vTblTotal.MfgDate ,
//       vTblTotal.ExpDate 
//     ,
//       SUM(vTblTotal.AvailableQty)AQty ,
//       SUM(vTblTotal.BookFDel)BookFDel ,
//       SUM(vTblTotal.Tqty)Tqty ,
//       SUM(vTblTotal.RQty)RQty ,
//       SUM(vTblTotal.BQty)BQty 
//    from
//
//  (SELECT CS.ProductCode,CS.ProductName,CS.PackSize,CS.
//BatchNo,MD.MfgDate,CS.ExpDate,CS.Quantity AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty
// FROM dbo.tblCentralStore CS 
//  INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
//where CS.Quantity>0
//       UNION ALL
//          
//
//
//SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
//BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,ST.PickingQty AS Tqty,0 AS RQty,0 AS BQty FROM dbo.tblCentralStore CS 
//INNER JOIN dbo.tblStockInTransfar ST ON ST.ReceiveId = CS.ReceiveId
//
//INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
//WHERE  ST.IsTransfared is null and ST.IsIssue='OK'
//
// UNION ALL
//SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
//BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,ST.PickingQty AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty FROM dbo.tblCentralStore CS 
//INNER JOIN dbo.tblStockInTransfar ST ON ST.ReceiveId = CS.ReceiveId
//
//INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
//WHERE ST.IsIssue IS NULL and ST.IsTransfared IS NULL 
//
//UNION ALL
//
//SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
//BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCF.StockQty AS RQty,0 AS BQty FROM 
//dbo.tblWhStoreFreeze DCF 
//INNER JOIN 
//dbo.tblCentralStore CS ON CS.ReceiveId = DCF.ReceiveId
//INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
//WHERE DCF.StockCondition='Restricted' and DCF.StockQty>0
//
//
//UNION ALL
//
//SELECT  CS.ProductCode,CS.ProductName,CS.PackSize,CS.
//BatchNo,MD.MfgDate,CS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,DCF.StockQty AS BQty FROM 
//dbo.tblWhStoreFreeze DCF  INNER JOIN 
//dbo.tblCentralStore CS ON CS.ReceiveId = DCF.ReceiveId
//INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
//       WHERE DCF.StockCondition='Blocked'  and DCF.StockQty>0
//
//) AS vTblTotal
//
// where vTblTotal.ProductCode='" + Pcode + "'GROUP BY vTblTotal.ProductCode ,vTblTotal.ProductName ,vTblTotal.PackSize ,vTblTotal.BatchNo ,vTblTotal.MfgDate ,vTblTotal.ExpDate ";
            }
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

    }

}
