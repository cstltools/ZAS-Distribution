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
    public class StockConditionFreezeDAL
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();


        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void GetWhInformation(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void GetWhInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }


        public DataTable LoadStockDCData(int ComUnitId)
        {
            string query = @"SELECT tblDCStore.*,DCStoreId AS nomanslandID, tblDCStore.ProductCode,tblDCStore.ProductName,tblDCStore.BatchNo,ExpDate,ReceiveDate,TotalQuantity,StockQty,tblUnitPrice.UnitPrice*StockQty AS Amount,StockCondition FROM dbo.tblDCStore
                              INNER JOIN dbo.tblUnitPrice ON tblDCStore.ProductCode = tblUnitPrice.ProductCode        
                                where  StockQty>0 and   StockCondition = 'Available' AND ComUnitId= '" + ComUnitId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadStockStockQtyDCData(int DCStoreId)
        {
            string query = @"SELECT tblDCStore.*,DCStoreId AS nomanslandID, tblDCStore.ProductCode,tblDCStore.ProductName,tblDCStore.BatchNo,ExpDate,ReceiveDate,TotalQuantity,StockQty,tblUnitPrice.UnitPrice*StockQty AS Amount,StockCondition FROM dbo.tblDCStore
                              INNER JOIN dbo.tblUnitPrice ON tblDCStore.ProductCode = tblUnitPrice.ProductCode        
                                where StockCondition = 'Available' AND DCStoreId= '" + DCStoreId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void LoadStockConditionBll(DropDownList aDownList,string userid)
        {
            string StockCondition = "select * from tblStockCondition WHERE StockCondition<>'Available'";
            //string StockCondition = "select * from tblStockCondition WHERE StockCondition<>'Available' AND StockConId IN (SELECT StockConId FROM tblStockConditionPermission where Permission=1 AND UserId='" + userid + "')";
            aCommonInternalDal.LoadDropDownValue(aDownList, "StockCondition", "StockConId", StockCondition, "SSIDB");
        }
        public DataTable LoadWHData()
        {
            string query = @"SELECT  tblCentralStore.*,ReceiveId AS nomanslandID,tblCentralStore.ProductCode,tblCentralStore.ProductName,tblCentralStore.BatchNo,ExpDate,ReceiveDate,StockInQty AS TotalQuantity,Quantity AS StockQty,tblUnitPrice.UnitPrice*Quantity AS Amount,StockCondition
                           FROM dbo.tblCentralStore INNER JOIN dbo.tblUnitPrice ON tblCentralStore.ProductCode = tblUnitPrice.ProductCode        
                           where StockCondition = 'Available' ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadWHData(int ReceiveId)
        {
            string query = @"SELECT  tblCentralStore.*,ReceiveId AS nomanslandID,tblCentralStore.ProductCode,tblCentralStore.ProductName,tblCentralStore.BatchNo,ExpDate,ReceiveDate,StockInQty AS TotalQuantity,Quantity AS StockQty,tblUnitPrice.UnitPrice*Quantity AS Amount,StockCondition
                           FROM dbo.tblCentralStore INNER JOIN dbo.tblUnitPrice ON tblCentralStore.ProductCode = tblUnitPrice.ProductCode        
                           where StockCondition = 'Available' AND ReceiveId= '" + ReceiveId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveforWH(StockConditionFreezeDAO aStockConditionFreezeDAO)
        {
            string insertQuery = @"insert into tblStockConditionFreeze (StockConditionFreezeID,ReceiveId,ManufacId,FreezeQty,EntryBy,EntryDate) 
            values (" + aStockConditionFreezeDAO.StockConditionFreezeID + "," + aStockConditionFreezeDAO.ReceiveId + ",'" + aStockConditionFreezeDAO.ManufacId + "','" + aStockConditionFreezeDAO.FreezeQty + "','" + aStockConditionFreezeDAO.EntryBy + "','" + aStockConditionFreezeDAO.EntryDate + "')";
                
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool SaveforDC(StockConditionFreezeDAO aStockConditionFreezeDAO)
        {
            string insertQuery = @"insert into tblStockConditionFreeze (StockConditionFreezeID,DCStoreId,ManufacId,FreezeQty,EntryBy,EntryDate) 
            values (" + aStockConditionFreezeDAO.StockConditionFreezeID + "," + aStockConditionFreezeDAO.DCStoreId + ",'" + aStockConditionFreezeDAO.ManufacId + "','" + aStockConditionFreezeDAO.FreezeQty + "','" + aStockConditionFreezeDAO.EntryBy + "','" + aStockConditionFreezeDAO.EntryDate + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool UpdateCentralStore(decimal StockQty, int ReceiveId)
        {
            string query = @"UPDATE tblCentralStore SET Quantity='" + StockQty + "' WHERE ReceiveId=" + ReceiveId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool UpdateDCStore(decimal StockQty, int DCStoreId)
        {
            string query = @"UPDATE tblDCStore SET StockQty='" + StockQty + "' WHERE DCStoreId=" + DCStoreId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        //SC Picking Generate Page

        public DataTable LoadInvoice(int ComUnitId, int ManufId, int marketid, DateTime invDate)
        {
                    string query = @"SELECT  * 				
        FROM tblInvoice I
        INNER JOIN (SELECT DISTINCT D.InvoiceId, ManufacId FROM dbo.tblInvoice I
                    INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId
                    INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode
                    ) as tblD ON I.InvoiceId = tblD.InvoiceId  
         INNER JOIN dbo.View_CustomerMaster C ON I.CustomerMasterId = C.CustomerMasterId
         INNER JOIN dbo.tblMarket ON C.MarketCode=dbo.tblMarket.MarketCode        
        where I.ComUnitId= '" + ComUnitId + "' and tblD.ManufacId='" + ManufId + "' and tblMarket.MarketId='" + marketid + "' and InvoiceDate='" + invDate + "' order by OrderNo";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveDCStoreFreeze(DCStoreFreezeDAO aDcStoreFreezeDao)
        {
            string insertQuery = @"insert into tblDCStoreFreeze (DCStoreFreezeId,StorageLocation,TotalQuantity,ProductCode,ProductName,PackSize,BatchNo,ExpDate,ReceiveDate,ChalanNo,ChalanDate,StockQty,DamageQty,StockRcvDate,StockCondition,Remarks,ReceiveId,StockConditionFreezeID) 
            values (" + aDcStoreFreezeDao.DCStoreFreezeId + ",'" + aDcStoreFreezeDao.StorageLocation + "','" + aDcStoreFreezeDao.TotalQuantity + "','" + aDcStoreFreezeDao.ProductCode + "','" + aDcStoreFreezeDao.ProductName + "','" + aDcStoreFreezeDao.PackSize + "','" + aDcStoreFreezeDao.BatchNo + "','" + aDcStoreFreezeDao.ExpDate + "','" + aDcStoreFreezeDao.ReceiveDate + "','" + aDcStoreFreezeDao.ChalanNo + "','" + aDcStoreFreezeDao.ChalanDate + "','" + aDcStoreFreezeDao.StockQty + "','" + aDcStoreFreezeDao.DamageQty + "','" + aDcStoreFreezeDao.StockRcvDate + "','" + aDcStoreFreezeDao.StockCondition + "'," + aDcStoreFreezeDao.remarks + "'," + aDcStoreFreezeDao.ReceiveId + "," + aDcStoreFreezeDao.StockConditionFreezeID + ")";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStoreFreezeDao)
        {
            string insertQuery = @"insert into tblDCStoreFreeze (DCStoreFreezeId,StorageLocation,TotalQuantity,ProductCode,ProductName,PackSize,BatchNo,ExpDate,ReceiveDate,ChalanNo,ChalanDate,StockQty,DamageQty,StockRcvDate,StockCondition,Remarks,ComUnitId,DCStoreId,StockConditionFreezeID) 
            values (" + aDcStoreFreezeDao.DCStoreFreezeId + ",'" + aDcStoreFreezeDao.StorageLocation + "','" + aDcStoreFreezeDao.TotalQuantity + "','" + aDcStoreFreezeDao.ProductCode + "','" + aDcStoreFreezeDao.ProductName + "','" + aDcStoreFreezeDao.PackSize + "','" + aDcStoreFreezeDao.BatchNo + "','" + aDcStoreFreezeDao.ExpDate + "','" + aDcStoreFreezeDao.ReceiveDate + "','" + aDcStoreFreezeDao.ChalanNo + "','" + aDcStoreFreezeDao.ChalanDate + "','" + aDcStoreFreezeDao.StockQty + "','" + aDcStoreFreezeDao.DamageQty + "','" + aDcStoreFreezeDao.StockRcvDate + "','" + aDcStoreFreezeDao.StockCondition + "','" + aDcStoreFreezeDao.remarks + "'," + aDcStoreFreezeDao.ComUnitId + "," + aDcStoreFreezeDao.DCStoreId + "," + aDcStoreFreezeDao.StockConditionFreezeID + ")";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool PostFreezeStockJournal(int stockConditionFreezeId, decimal quantity)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@DcStockConditionId", stockConditionFreezeId));
            aSqlParameterList.Add(new SqlParameter("@Quantity", quantity));

            return aCommonInternalDal.UpdateAction("sp_FreezeStockPosting", aSqlParameterList);
        }
    }
}
