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
            const string query = @"SELECT * FROM dbo.tblWHStockInMaster AS WHSM
                            INNER JOIN tblWearhouse AS WH ON WHSM.WarehouseId = WH.WearhouseId WHERE WHSM.Status = 'Posted'";
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

        public void SaveWHStockIntoCenterlWarehouse(WHStockInToCenteralStore aCenteralStore)
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

            aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
    }
}
