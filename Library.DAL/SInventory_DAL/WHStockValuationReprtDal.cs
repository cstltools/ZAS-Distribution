using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class WHStockValuationReprtDal
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


        public DataTable GetUserCompanyById(string parameter)
        {
            string query = @"SELECT DISTINCT TOP 1 CompanyId FROM dbo.tblUserCompanyUnit AS UT
	            LEFT JOIN tblCompanyUnit AS UNT ON UT.CompanyUnitId = UNT.ComUnitId WHERE UserId =" + parameter;
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadDcInfoByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");
        }

        public DataTable LoadWHStockValueationReportByPram(string fromDate, string toDate, string productId, string comId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            // ProductId (INT, nullable)

            aSqlParameterlist.Add(new SqlParameter("@ProductId",
                string.IsNullOrWhiteSpace(productId) ? (object)DBNull.Value : Convert.ToInt32(productId)));

            // CompanyId (INT, required in your SP; if you still want nullable, handle it)

            aSqlParameterlist.Add(new SqlParameter("@CompanyId",
                string.IsNullOrWhiteSpace(comId) ? (object)DBNull.Value : Convert.ToInt32(comId)));

            // FromDate (DATE)

            aSqlParameterlist.Add(new SqlParameter("@FromDate",
                string.IsNullOrWhiteSpace(fromDate) ? (object)DBNull.Value : Convert.ToDateTime(fromDate)));

            // ToDate (DATE)

            aSqlParameterlist.Add(new SqlParameter("@ToDate",
                string.IsNullOrWhiteSpace(toDate) ? (object)DBNull.Value : Convert.ToDateTime(toDate)));

            return aCommonInternalDAL.GetDataTableAction("sp_INV_WHStockValuationReport", aSqlParameterlist, "SSIDB");

        }



        public DataTable LoadDCStockValuationReportByParam(
    string fromDate,
    string toDate,
    string productId,
    string warehouseId,
    string comId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            // ProductId (INT, nullable)
            aSqlParameterlist.Add(new SqlParameter("@ProductId",
                string.IsNullOrWhiteSpace(productId)
                    ? (object)DBNull.Value
                    : Convert.ToInt32(productId)));

            // CompanyId (INT, required)
            aSqlParameterlist.Add(new SqlParameter("@CompanyId",
                string.IsNullOrWhiteSpace(comId)
                    ? (object)DBNull.Value
                    : Convert.ToInt32(comId)));

            // FromDate (DATETIME)
            aSqlParameterlist.Add(new SqlParameter("@FromDate",
                string.IsNullOrWhiteSpace(fromDate)
                    ? (object)DBNull.Value
                    : Convert.ToDateTime(fromDate)));

            // ToDate (DATETIME)
            aSqlParameterlist.Add(new SqlParameter("@ToDate",
                string.IsNullOrWhiteSpace(toDate)
                    ? (object)DBNull.Value
                    : Convert.ToDateTime(toDate)));

            // WarehouseId (INT, nullable)
            aSqlParameterlist.Add(new SqlParameter("@WarehouseId",
                string.IsNullOrWhiteSpace(warehouseId)
                    ? (object)DBNull.Value
                    : Convert.ToInt32(warehouseId)));

            return aCommonInternalDAL.GetDataTableAction(
                "sp_INV_DCStockValuationReport",
                aSqlParameterlist,
                "SSIDB");   // ⚠ Ensure DB name matches SP
        }



    }
}
