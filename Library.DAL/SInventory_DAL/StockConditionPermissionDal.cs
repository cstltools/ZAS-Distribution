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
    public class StockConditionPermissionDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetUserInfoOnDropdownList(DropDownList ddl)
        {
            string query = @"SELECT UserId,UserCode + ':' + UserName AS UserName FROM tblUser";
            aCommonInternalDal.LoadDropDownValue(ddl, "UserName", "UserId", query, "SSIDB");
        }

        public DataTable GetStockConditionList()
        {
            string query = @"SELECT * FROM dbo.tblStockCondition";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetStockConditionByUserId(string userId)
        {
            string query = @"SELECT * FROM dbo.tblStockConditionPermission WHERE UserId = '" + userId + "'" ;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveStockConditionPermissionInfo(StockConditionPermissionDao stockConditionPermissionDao)
        {
            string insertQuery = @"INSERT INTO dbo.tblStockConditionPermission (UserId,StockConId,Permission) VALUES ('" + stockConditionPermissionDao.UserId + "','" + stockConditionPermissionDao.StockConId + "','" + stockConditionPermissionDao.Permission + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable CheckPermissionInfoAlreadyExistOrNot(StockConditionPermissionDao stockConditionPermissionDao)
        {
            string query = @"SELECT * FROM dbo.tblStockConditionPermission WHERE UserId = '" + stockConditionPermissionDao.UserId + "' AND StockConId = '" + stockConditionPermissionDao.StockConId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DeletePermissionInfo(StockConditionPermissionDao stockConditionPermissionDao)
        {
            string query = @"DELETE FROM dbo.tblStockConditionPermission WHERE UserId = '" + stockConditionPermissionDao.UserId + "' AND StockConId = '" + stockConditionPermissionDao.StockConId + "'";
            return aCommonInternalDal.DeleteDataByDeleteCommand(query, "SSIDB");
        }
    }
}
