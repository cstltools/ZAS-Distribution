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
    public class SalesSummeryReportForAcc
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

        public DataTable LoadSalesSummeryByPram(string fromDate,string toDate, string branchId, string comId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@CompanyId", comId ?? (object)DBNull.Value));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitId", branchId ?? (object)DBNull.Value));
            aSqlParameterlist.Add(new SqlParameter("@fDate", fromDate != null ? Convert.ToDateTime(fromDate) : (object)DBNull.Value));
            aSqlParameterlist.Add(new SqlParameter("@tdate", toDate != null ? Convert.ToDateTime(toDate) : (object)DBNull.Value));

            return aCommonInternalDAL.GetDataTableAction("sp_Summery_SalesSummerReportForAccounts", aSqlParameterlist, "SSIDB");
        }
    }
}
