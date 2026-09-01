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
    public class WarehouseStockInApprovalDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        //ClsApprovalAction approvalAction = new ClsApprovalAction();

        //public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        //{
        //  //  approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        //}
        //public string LoadForApprovalConditionDAL(string pageName, string userName)
        //{
        //   // return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        //}

        public DataTable GetWarehouseStockInInformation()
        {
            const string query = @"SELECT ISNULL(MaterialConversionCount,0) MaterialConversionCount,* FROM dbo.tblWHStockInMaster AS WHSM
INNER JOIN tblWearhouse AS WH ON WHSM.WarehouseId = WH.WearhouseId 
LEFT JOIN (SELECT WM.WHStockInMasterID, Count(WHStockInDetailID) MaterialConversionCount FROM tblWHStockInDetail AS SD 
LEFT JOIN tblWHStockInMaster AS WM ON SD.WHStockInMasterID = WM.WHStockInMasterID
WHERE ISNULL(SD.MaterialConversionMasterId,0) != 0 GROUP BY WM.WHStockInMasterID) AS MC ON WHSM.WHStockInMasterID = MC.WHStockInMasterID
WHERE WHSM.Status = 'Posted' ORDER BY EntryDate DESC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void ApprovalUpdateDal(WarehouseStockInMasterDao aMasterDao)
        {
            string query = @"UPDATE tblWHStockInMaster SET Status = '" + aMasterDao.Status + "',ApproveBy = '"
                + aMasterDao.ApproveBy + "',ApproveDate = '" + aMasterDao.ApproveDate +
                "' WHERE WHStockInMasterID = " + aMasterDao.WHStockInMasterID + "";
            aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable GetLoadWarehouseStockInInfo(Int32 stockInMasterId)
        {
            string query = @"SELECT * FROM dbo.tblWHStockInMaster AS WHSM 
                    INNER JOIN dbo.tblWHStockInDetail WHSD ON WHSD.WHStockInMasterID = WHSM.WHStockInMasterID
                    INNER JOIN dbo.tblProduct AS PD ON PD.ProductId = WHSD.ProductId 
                    WHERE WHSM.WHStockInMasterID = '" + stockInMasterId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CheckDetailsExistOrNot(WHStockInToCenteralStore aDetailDao)
        {
            string query = @"SELECT * FROM tblCentralStore WHERE LTRIM(RTRIM(UPPER(ProductCode))) = LTRIM(RTRIM(UPPER('" + aDetailDao.ProductCode + "')))  AND StockInQty = " 
                + aDetailDao.StockInQty + " AND MigoDetailID = " + aDetailDao.MigoDetailID;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveWHStockIntoCenterlWarehouse(WHStockInToCenteralStore aCenteralStore)
        {
            string insertQuery = @"INSERT INTO dbo.tblCentralStore
        ( 
          ProductCode ,
          ProductName ,
          WarehouseId,
          PackSize ,
          BatchNo ,
          Quantity ,
          ExpDate ,
          MfgDate, 
          ReceiveDate ,
          ChalanNo ,
          ChalanDate ,
          StockInQty ,
          UnitPrice ,
          VATPerUnit ,
          TotalPrice ,
          TotalVAT ,
          TotalAmount ,
          MigoDetailID,
          ProductStockType,
          ProductId,
          StockCondition 
        )
            VALUES (
                                 '" + aCenteralStore.ProductCode + "'," +
                                 "'" + aCenteralStore.ProductName + "'," +
                                 "'" + aCenteralStore.WarehouseId + "'," +
                                 "'" + aCenteralStore.PackSize + "'," +
                                 "'" + aCenteralStore.BatchNo + "'," +
                                 "'" + aCenteralStore.Quantity + "'," +
                                 "'" + aCenteralStore.ExpDate + "'," +
                                 "'" + aCenteralStore.MfgDate + "'," +
                                 "'" + aCenteralStore.ReceiveDate + "'," +
                                 "'" + aCenteralStore.ChalanNo + "'," +
                                 "'" + aCenteralStore.ChalanDate + "'," +
                                 "'" + aCenteralStore.StockInQty + "'," +
                                 "'" + aCenteralStore.UnitPrice + "'," +
                                 "'" + aCenteralStore.VATPerUnit + "'," +
                                 "'" + aCenteralStore.TotalPrice + "'," +
                                 "'" + aCenteralStore.TotalVAT + "'," +
                                 "'" + aCenteralStore.TotalAmount + "'," +
                                 "'" + aCenteralStore.MigoDetailID + "'," +
                                 "'" + aCenteralStore.ProductStockType + "'," +
                                 "'" + aCenteralStore.ProductId + "'," +
                                 "'" + aCenteralStore.StockCondition + "' " + ")";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool PostJournal(int stockInMasterId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@MasterId", stockInMasterId));

            return aCommonInternalDal.UpdateAction("sp_WHStockInPosting", aSqlParameterList);
        }

       

        public bool ApproveWarehouseStockInMaster(WarehouseStockInMasterDao aMasterDao)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();


            aSqlParameterList.Add(new SqlParameter("@ApprovalStatus", aMasterDao.Status));
            aSqlParameterList.Add(new SqlParameter("@MasterId", aMasterDao.WHStockInMasterID));
            aSqlParameterList.Add(new SqlParameter("@ApprovalBy", aMasterDao.ApproveBy));
            aSqlParameterList.Add(new SqlParameter("@ApprovalDate", aMasterDao.ApproveDate));

            return aCommonInternalDal.UpdateAction("sp_Acc_MaterialConversionReceive", aSqlParameterList);
        }
    }
}
