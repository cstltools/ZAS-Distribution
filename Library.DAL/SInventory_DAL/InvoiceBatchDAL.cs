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
    public class InvoiceBatchDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public int SaveInvoiceBatch(InvoiceBatchDAO aInvoiceBatchDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            
            aSqlParameterlist.Add(new SqlParameter("@BatchNo", aInvoiceBatchDao.BatchNo));
            aSqlParameterlist.Add(new SqlParameter("@CompanyId", aInvoiceBatchDao.CompanyId));
            aSqlParameterlist.Add(new SqlParameter("@Date", aInvoiceBatchDao.Date));
            aSqlParameterlist.Add(new SqlParameter("@DeliveryManId", aInvoiceBatchDao.DeliveryManId));
            
            string insertQuery = @"INSERT INTO dbo.tblInvoiceBatch
        ( BatchNo ,
          CompanyId ,
          Date ,
          DeliveryManId
        )
            values (@BatchNo ,
          @CompanyId ,
          @Date ,
          @DeliveryManId)";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery,aSqlParameterlist, "SSIDB");
        }
        public void LoadManufac(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }
        public void LoadDeliveryMan(DropDownList aDropDownList)
        {
            string query = @"SELECT DL.DeliveryManId,ISNULL( HRGI.EmpName + ' (' + JobLocationName + ')', EGI.EmpName) DeliveryManName FROM tblDeliveryManInfo AS DL
INNER JOIN tblEmpGeneralInfo AS EGI ON DL.EmpInfoId = EGI.EmpInfoId
LEFT JOIN ZASHRIS..tblEmpGeneralInfo AS HRGI ON EGI.EmpMasterCode = HRGI.EmpMasterCode
LEFT JOIN ZASHRIS..tblJobLocation AS JL ON HRGI.JobLocationisionId = JL.JobLocationisionId
WHERE DL.IsActive = 1 ORDER BY JobLocationName ASC";


            aCommonInternalDal.LoadDropDownValue(aDropDownList, "DeliveryManName", "DeliveryManId", query, "SSIDB");
        }
//        public DataTable LoadInvoice(string companyId, string delManId, string invDate)
//        {
//            string query = @"SELECT * FROM dbo.tblInvoice 
//            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
//			LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
//            WHERE InvoiceDate='" +invDate+"' AND ( BatchId IS NULL) AND DeliveryManId='"+delManId+"' AND CompanyId='"+companyId+"'";

//            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
//        }

        public DataTable LoadInvoice(string delManId, string invDate)
        {
            string query = @"SELECT * FROM dbo.tblInvoice 
            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
			LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
            WHERE InvoiceDate='" + invDate + "' AND ( BatchId IS NULL) AND DeliveryManId='" + delManId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadInvoiceNotNul(string delManId, string invDate, string batchId)
        {
            string query = @"SELECT * FROM dbo.tblInvoice 
            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
			LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
            WHERE InvoiceDate='" + invDate + "' AND BatchId = '" + batchId + "' AND DeliveryManId='" + delManId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetBatchNo(string companyId, string delManId, string invDate)
        {
            string query = @"SELECT (COUNT(*)+1)No FROM dbo.tblInvoiceBatch WHERE Date='"+invDate+"' AND  DeliveryManId='"+delManId+"' AND CompanyId='"+companyId+"'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void UpdateInvoiceBatchId(string invoiceId,string batchId)
        {
            string updateQuery = @"UPDATE tblInvoice SET BatchId='" + batchId + "' WHERE InvoiceId='" + invoiceId + "' AND ISNULL(BatchId,0) = 0";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }

        public DataTable GetInvoiceBatchId(string batchId)
        {
            string updateQuery = @"SELECT * FROM dbo.tblInvoiceBatch WHERE	BatchId = '" + batchId + "'";
            return aCommonInternalDal.DataContainerDataTable(updateQuery, "SSIDB");
        }

        public DataTable LoadInvoiceByUser(string delManId, string invDate, string userId)
        {
            string query = @"SELECT * FROM dbo.tblInvoice 
            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
			LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
            WHERE InvoiceDate='" + invDate + "' AND ( BatchId IS NULL) AND DeliveryManId= " + delManId + " AND tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateBachCodeById(int batchId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            
            aSqlParameterlist.Add(new SqlParameter("@BatchId", batchId));

            return aCommonInternalDal.UpdateAction("sp_Save_BatchCode",aSqlParameterlist); 
        }
    }
}
