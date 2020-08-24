using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class DCStockInfoReprotDal
    {

        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public void LoadGroupInfo(DropDownList groupDropDownList)
        {
            string query = @"SELECT GroupId, GroupName FROM tblGroupInformation";
            aCommonInternalDAL.LoadDropDownValue(groupDropDownList, "GroupName", "GroupId", query, "SSIDB");
        }

        public void LoadCompanyInfo(DropDownList ddl, string groupId)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo WHERE GroupId = " + groupId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        

        public DataTable LoadDCStockInfo(string parameter)
        {
            string query = @" SELECT  vTblTotal.ComUnitId,Cu.ComUnitCode,Cu.ComUnitName,vTblTotal.ProductCode ,
                vTblTotal.ProductName ,
                vTblTotal.PackSize ,
                vTblTotal.BatchNo ,
                vTblTotal.MfgDate ,
                vTblTotal.ExpDate ,
                SUM(vTblTotal.AvailableQty)AQty ,
                SUM(vTblTotal.BookFDel)BookFDel ,
                SUM(vTblTotal.Tqty)Tqty ,
                SUM(vTblTotal.RQty)RQty ,
                SUM(vTblTotal.BQty)BQty 
                FROM 
                (
                   SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,
                DCS.ComUnitId  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Available'
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,
                DCS.ComUnitId  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblChalanDetail cd ON dcs.ChalanDetailsId=CD.ChalanDetailsId
                --INNER JOIN tblDCStore dc ON CD.DCStoreId=DC.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
               -- INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = dc.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Available'
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,ID.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.TotalQuantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,
                I.ComUnitId
                FROM dbo.tblInvoice I 
                INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
                --INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                --INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                --INNER JOIN dbo.tblWHStockInDetail MD ON MD.MigoDetailID = CS.MigoDetailID
                WHERE I.DelivaryInvoiceNo IS NULL
                UNION ALL
                  SELECT p.ProductCode,p.ProductName,P.PackSize,
                ID.BatchNo,DCS.MfgDate,DCS.ExpDate,0 AS AvailableQty,ID.Quantity AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,
                cu.ComUnitId
                FROM dbo.tblChalanInfo I 
                INNER JOIN dbo.tblChalanDetail ID ON ID.ChalanId = I.ChalanId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN   dbo.tblCompanyUnit cu ON i.FromComUnitCode = cU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE I.IsDeliver IS NULL
                
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCS.StockQty AS RQty,0 AS BQty,
                DCS.ComUnitId  FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Restricted'
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,DCS.StockQty AS BQty,
                DCS.ComUnitId  FROM dbo.tblDCStoreFreeze DCS 
                INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = DCS.DCStoreId
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
                left JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DS.StockInTransfarId
                left JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                left JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Blocked'

                 UNION ALL
                
                
                SELECT p.ProductCode,p.ProductName,ST.PackSize,ST.
                BatchNo,ST.MfgDate,ST.ExpDate,0 AS AvailableQty,0 AS BookFDel,ST.Quantity AS Tqty,0 AS RQty,0 AS BQty,
                R.ComUnitId FROM dbo.tblRequisition R 
                INNER JOIN dbo.tblStockInTransfar ST ON ST.ReqId = R.ReqId
                INNER JOIN dbo.tblProduct p ON p.ProductCode = ST.ProductCode
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                WHERE  R.ReceiveIssue IS NULL
                UNION ALL

                SELECT p.ProductCode,p.ProductName,D.PackSize,D.
                                BatchNo,D.MfgDate,D.ExpDate,0 AS AvailableQty,0 AS BookFDel,CD.Quantity AS Tqty,0 AS RQty,0 AS BQty,
                                CU.ComUnitId FROM dbo.tblChalanInfo C 
                INNER JOIN dbo.tblChalanDetail CD ON CD.ChalanId = C.ChalanId
                INNER JOIN dbo.tblDCStore D ON D.DCStoreId = CD.DCStoreId
                INNER JOIN dbo.tblCompanyUnit CU ON C.ToComUnitCode=CU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON p.ProductCode = CD.ProductCode
                WHERE C.IsDeliver ='False' ) AS vTblTotal
				INNER JOIN dbo.tblCompanyUnit Cu ON Cu.ComUnitId = vTblTotal.ComUnitId
                " + parameter + "  GROUP BY vTblTotal.ComUnitId,Cu.ComUnitCode,Cu.ComUnitName,vTblTotal.ProductCode, vTblTotal.ProductName ,vTblTotal.PackSize ,vTblTotal.BatchNo ,vTblTotal.MfgDate ,vTblTotal.ExpDate ";
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable LoadSalesReportInfo(string parameter)
        {
            string query = @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,INV.OrderNo,CONVERT(VARCHAR,INV.OrderDate,103) as OrderDate,INV.InvoiceNo,
                            CONVERT(VARCHAR,INV.InvoiceDate,103)  InvoiceDate,INV.DelivaryInvoiceNo, CONVERT(VARCHAR,INV.UpdateDate,103) UpdateDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.DeliveryQuantity,DeliveryNetAmount,
                            DeliveryTotalPriceVatAmount,DeliveryDiscountAmount,ID.DelivarySpecialAmount,C.AreaCode, C.AreaName, C.RegionCode, C.RegionName,C.TerritoryCode, C.TerritoryName, C.MarketCode, C.MarketName, REGI.EmpName AS RSM,AEGI.EmpName AS ASM,MEGI.EmpName AS MIO   FROM tblInvoice AS INV 
                            INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = INV.InvoiceId 
                            INNER JOIN dbo.tblOrder AS ORD ON ORD.OrderId = INV.OrderId
                            INNER JOIN dbo.tblRSMInfo AS RSM ON RSM.RSMId = ORD.RSMId
                            INNER JOIN dbo.tblEmpGeneralInfo AS REGI ON RSM.EmployeeId = REGI.EmpInfoId
                            INNER JOIN dbo.tblASMInfo AS ASM ON ASM.ASMId = ORD.ASMId
                            INNER JOIN dbo.tblEmpGeneralInfo AS AEGI ON ASM.EmployeeId = AEGI.EmpInfoId
                            INNER JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = ORD.MIOId
                            INNER JOIN dbo.tblEmpGeneralInfo AS MEGI ON MIO.EmployeeId = MEGI.EmpInfoId
                            INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = INV.ComUnitId
                            INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = INV.CustomerMasterId
                            INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId 
                            WHERE ID.DeliveryStatus IN ('Full','Partial')" + parameter;
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");


        }
    }
}
