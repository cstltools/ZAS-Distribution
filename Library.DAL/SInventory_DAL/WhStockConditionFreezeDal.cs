using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class WhStockConditionFreezeDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

         public void GetWhInfoOnDropDownList(DropDownList ddl)
        {
            string query = @"SELECT WearhouseId, WearhouseCode + ':' + WearhouseName AS WearhouseName  FROM tblWearhouse";
            aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", query, "SSIDB");
        }

         public void GetCompanyDropdownList(DropDownList ddl)
         {
             const string queryStr = @"SELECT CompanyId,CompanyName AS CompanyName FROM tblCompanyInfo";
             aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
         }

         public void GetWhInformation(DropDownList ddl, string companyId)
         {
             string queryStr = @"SELECT WearhouseId, WearhouseCode + ' : ' + WearhouseName AS WearhouseName FROM tblWearhouse WHERE CompanyId = " + companyId;
             aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", queryStr, "SSIDB");
         }

        public DataTable GetWhStockInformation(string warehouseId)
        {
            string query = @"SELECT DISTINCT tblCentralStore.*,ReceiveId AS nomanslandID,tblCentralStore.ProductCode,tblCentralStore.ProductName,tblCentralStore.BatchNo,ExpDate,ReceiveDate,StockInQty AS TotalQuantity,Quantity AS StockQty,StockCondition
                           FROM dbo.tblCentralStore where tblCentralStore.WarehouseId = '" + warehouseId + "' AND StockCondition = 'Available'  AND  Quantity > 0  ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable LoadWHData(int ReceiveId)
        {
            string query = @"SELECT  tblCentralStore.*,ReceiveId AS nomanslandID,tblCentralStore.ProductCode,tblCentralStore.ProductName,tblCentralStore.BatchNo,ExpDate,ReceiveDate,StockInQty AS TotalQuantity,Quantity AS StockQty,StockCondition
                           FROM dbo.tblCentralStore where StockCondition = 'Available' AND ReceiveId= '" + ReceiveId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public void LoadStockConditionBll(DropDownList aDownList, string userid)
        {
            string StockCondition = "select * from tblStockCondition WHERE StockCondition<>'Available' AND StockConId IN (SELECT StockConId FROM tblStockConditionPermission where Permission=1 AND UserId='" + userid + "')";
            aCommonInternalDal.LoadDropDownValue(aDownList, "StockCondition", "StockConId", StockCondition, "SSIDB");
        }

        public bool SaveforWh(WhStockConditionFreezeDao aConditionFreezeDao)
        {
            string insertQuery = @"insert into tblWhStockConditionFreeze (WhStockConditionFreezeID,ReceiveId,WarehouseId,FreezeQty,EntryBy,EntryDate) 
            values (" + aConditionFreezeDao.WhStockConditionFreezeID + "," + aConditionFreezeDao.ReceiveId + ",'" + aConditionFreezeDao.WarehouseId + "','" + aConditionFreezeDao.FreezeQty + "','" + aConditionFreezeDao.EntryBy + "','" + aConditionFreezeDao.EntryDate + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool SaveWhStoreFreeze(WhStoreFreezeDao aWhStoreFreezeDao)
        {
            string insertQuery = @"insert into tblWhStoreFreeze (TotalQuantity,ProductId,ProductName,PackSize,BatchNo,ExpDate,ReceiveDate,StockQty,DamageQty,StockRcvDate,StockCondition,Remarks,ReceiveId,WhStockConditionFreezeID) 
            values (" + "'" + aWhStoreFreezeDao.TotalQuantity + "','" + aWhStoreFreezeDao.ProductId + "','" + aWhStoreFreezeDao.ProductName + "','" + aWhStoreFreezeDao.PackSize + "','" + aWhStoreFreezeDao.BatchNo + "','" + aWhStoreFreezeDao.ExpDate + "','" + aWhStoreFreezeDao.ReceiveDate  + "','" + aWhStoreFreezeDao.StockQty + "','" + aWhStoreFreezeDao.DamageQty + "','" + aWhStoreFreezeDao.StockRcvDate + "','" + aWhStoreFreezeDao.StockCondition + "','" + aWhStoreFreezeDao.Remarks + "','" + aWhStoreFreezeDao.ReceiveId + "','" + aWhStoreFreezeDao.WhStockConditionFreezeID + "'" + ")";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool UpdateCentralStore(decimal StockQty, int ReceiveId)
        {
            string query = @"UPDATE tblCentralStore SET Quantity='" + StockQty + "' WHERE ReceiveId=" + ReceiveId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable LoadNationalStockInfo(string parameter)
        {
            string query = @"SELECT  
				CASE 
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '371' THEN 'ZAS Dermal'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '372' THEN 'ZAS Critical Care'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '373' THEN 'ZAS Ophthalmic'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '379' THEN 'ZAS Active' 
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '375' THEN 'ZAS Dental ( Clinic )'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '381' THEN 'ZAS Dental ( Lab  )'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '377' THEN 'ZAS Gloves & Instruments'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '376' THEN 'ZAS Gyno'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '378' THEN 'ZAS Nutrition' 
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '374' THEN 'ZAS Onco'
				WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '382' THEN 'ZAS OTC'
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '383' THEN 'ZAS NOC'
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '384' THEN 'ZAS Baby Care'
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '385' THEN 'ZAS Neurology'
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '386' THEN 'ZAS Hematology'
                WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '387' THEN 'Aqvida Bangladesh'
				ELSE ''  
				END AS CompanyName,
                vTblTotal.ProductCode ,
                vTblTotal.ProductName ,
                vTblTotal.PackSize ,                
                SUM(vTblTotal.AvailableQty)AQty ,
                SUM(vTblTotal.BookFDel)BookFDel ,
                SUM(vTblTotal.Tqty)Tqty ,
                SUM(vTblTotal.RQty)RQty ,
                SUM(vTblTotal.BQty)BQty ,
				SUM(vTblTotal.WHQty)WHQty ,
                SUM(vTblTotal.WHQty+vTblTotal.AvailableQty+vTblTotal.RQty+vTblTotal.BQty)TotalQty ,
				SUM(vTblTotal.AvailableQty * vTblTotal.Price) AS AvialableValue,
				SUM(vTblTotal.RQty * vTblTotal.Price) AS RestrictedValue,
				SUM(vTblTotal.Tqty * vTblTotal.Price) AS TransitValue,
				SUM(vTblTotal.BQty * vTblTotal.Price) AS BlockedValue,
				SUM(vTblTotal.WHQty * vTblTotal.Price) AS WhStockValue,
				(SUM(vTblTotal.RQty * vTblTotal.Price) + SUM(vTblTotal.BQty * vTblTotal.Price) + SUM(vTblTotal.AvailableQty * vTblTotal.Price) + SUM(vTblTotal.WHQty * vTblTotal.Price)) AS GrandTotal

                FROM 
                (SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS WHQty  FROM dbo.tblDCStore DCS 
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE DCS.StockQty>0 AND DCS.StockCondition='Available' AND UP.IsActive=1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,DCS.MfgDate,DCS.ExpDate,DCS.StockQty AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS WHQty  FROM dbo.tblDCStore DCS 
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
                I.ComUnitId,0 AS WHQty
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
                cu.ComUnitId,0 AS WHQty
                FROM dbo.tblChalanInfo I 
                INNER JOIN dbo.tblChalanDetail ID ON ID.ChalanId = I.ChalanId 
                INNER JOIN 
                dbo.tblDCStore DCS ON DCS.DCStoreId = ID.DCStoreId
                INNER JOIN dbo.tblCompanyUnit cu ON i.FromComUnitCode = cU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON DCS.ProductCode = p.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN tblStockInTransfar ST ON ST.StockInTransfarId = DCS.StockInTransfarId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                INNER JOIN dbo.tblWHStockInDetail MD ON MD.WHStockInDetailID = CS.MigoDetailID
                WHERE I.IsDeliver IS NULL AND UP.IsActive=1
                
                
                UNION ALL
                SELECT p.ProductCode,p.ProductName,DCS.PackSize,DCS.
                BatchNo,MD.MfgDate,DCS.ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,DCS.StockQty AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                DCS.ComUnitId,0 AS WHQty FROM dbo.tblDCStoreFreeze DCS 
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
                DCS.ComUnitId,0 AS WHQty 
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
                R.ComUnitId,0 AS WHQty FROM dbo.tblRequisition R 
                INNER JOIN dbo.tblStockInTransfar ST ON ST.ReqId = R.ReqId
                INNER JOIN dbo.tblProduct p ON p.ProductCode = ST.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                INNER JOIN dbo.tblCentralStore CS ON CS.ReceiveId = ST.ReceiveId
                WHERE  R.ReceiveIssue IS NULL AND UP.IsActive=1
                UNION ALL
                SELECT p.ProductCode,p.ProductName,D.PackSize,D.
                                BatchNo,D.MfgDate,D.ExpDate,0 AS AvailableQty,0 AS BookFDel,CD.Quantity AS Tqty,0 AS RQty,0 AS BQty,UP.UnitPrice AS Price,
                                CU.ComUnitId,0 AS WHQty FROM dbo.tblChalanInfo C 
                INNER JOIN dbo.tblChalanDetail CD ON CD.ChalanId = C.ChalanId
                INNER JOIN dbo.tblDCStore D ON D.DCStoreId = CD.DCStoreId
                INNER JOIN dbo.tblCompanyUnit CU ON C.ToComUnitCode=CU.ComUnitCode
                INNER JOIN dbo.tblProduct p ON p.ProductCode = CD.ProductCode
				INNER JOIN dbo.tblUnitPrice AS UP ON P.ProductId = UP.ProductId
                WHERE C.IsDeliver ='False' AND UP.IsActive=1
				UNION ALL
				SELECT ProductCode,ProductName,PackSize,
                BatchNo,MfgDate,ExpDate,0 AS AvailableQty,0 AS BookFDel,0 AS Tqty,0 AS RQty,0 AS BQty,UnitPrice AS Price,
                0 AS ComUnitId,SUM(Quantity) AS WHQty FROM dbo.tblCentralStore WHERE QUantity > 0 GROUP BY ProductCode,ProductName,PackSize,BatchNo,MfgDate,ExpDate,UnitPrice
				) AS vTblTotal				
				LEFT JOIN dbo.tblCompanyUnit Cu ON Cu.ComUnitId = vTblTotal.ComUnitId " + parameter + " GROUP BY vTblTotal.ProductCode, vTblTotal.ProductName ,vTblTotal.PackSize";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
