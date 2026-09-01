using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class DashboardDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetUserPermissionInfo(string userId)
        {
            string query = @"SELECT * FROM dbo.tblUser WHERE IsExpProduct = 1 AND UserId = " + userId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadExpireProductInfo()
        {
            string query = @"SELECT ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo,SUM(StockQty) AS StockQty FROM dbo.tblDCStore 
                             INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = tblDCStore.ComUnitId
                             WHERE GETDATE() BETWEEN DATEADD(MONTH, -3, CAST(ExpDate AS DATETIME)) AND ExpDate
                             GROUP BY ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo
                             HAVING SUM(StockQty) > 0";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetIsHeadInfo(string userId)
        {
            string query = @"SELECT * FROM dbo.tblUser WHERE IsHead = 1 AND UserId = " + userId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetIsCreditApprovalInfo(string userId)
        {
            string query = @"SELECT * FROM dbo.tblUser WHERE IsCreditApproval = 1 AND UserId = " + userId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DeleteCredit(string creditId)
        {
            string query = @"DELETE FROM tblCustomerCreditLimit WHERE CreditLimitId = " + creditId;
            return aCommonInternalDal.DeleteDataByCommand(query, "SSIDB");
        }
    }
}
