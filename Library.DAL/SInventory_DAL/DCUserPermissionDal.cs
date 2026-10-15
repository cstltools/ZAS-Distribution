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
    public class DCUserPermissionDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetUserInfo(DropDownList ddl)
        {
            string query = @"SELECT UserId,ISNULL(EmpMasterCode,UserCode) + CASE WHEN EmpName IS NULL THEN ':' + UserName ELSE EmpName END AS UserName FROM tblUser AS USR
                             LEFT JOIN tblEmpGeneralInfo AS EGI ON USR.EmpInfoId = EGI.EmpInfoId";
            aCommonInternalDal.LoadDropDownValue(ddl, "UserName", "UserId", query, "SSIDB");
        }

        public DataTable GetDCList()
        {
            string query = @"SELECT ComUnitId, ComUnitCode + ':'+ ComUnitName AS CompanyUnit FROM tblCompanyUnit UNION ALL SELECT 15,WearhouseCode + ':' + WearhouseName AS WearhouseName FROM tblWearhouse";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        // One atomic batch: removes unticked DCs and adds newly ticked ones. Existing rows are
        // left untouched so their CWHPermission/NationalReportPermission flags survive a re-save.
        public bool SaveDCUserPermissions(int userId, List<int> companyUnitIds)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@UserId", userId) };
            var names = new List<string>();
            for (int i = 0; i < companyUnitIds.Count; i++)
            {
                names.Add("@Unit" + i);
                parameters.Add(new SqlParameter("@Unit" + i, companyUnitIds[i]));
            }

            string idList = string.Join(",", names);
            string valueRows = string.Join(",", names.Select(n => "(" + n + ")"));

            string query = @"SET XACT_ABORT ON;
                BEGIN TRAN;
                DELETE FROM tblUserCompanyUnit WHERE UserId = @UserId AND CompanyUnitId NOT IN (" + idList + @");
                DELETE u FROM tblUserCompanyUnit u WHERE u.UserId = @UserId AND EXISTS
                    (SELECT 1 FROM tblUserCompanyUnit d WHERE d.UserId = u.UserId AND d.CompanyUnitId = u.CompanyUnitId AND d.UserComId < u.UserComId);
                INSERT INTO tblUserCompanyUnit (UserId, CompanyUnitId, CWHPermission, NationalReportPermission)
                SELECT @UserId, v.Id, 0, 0 FROM (VALUES " + valueRows + @") AS v(Id)
                WHERE NOT EXISTS (SELECT 1 FROM tblUserCompanyUnit u WHERE u.UserId = @UserId AND u.CompanyUnitId = v.Id);
                COMMIT;";

            aCommonInternalDal.SaveDataByInsertCommand(query, parameters, "SSIDB");
            return true; // zero affected rows means nothing changed, still a successful save; failures throw
        }

        public DataTable GetDCUserPermissionById(string userId)
        {
            string query = @"SELECT * FROM tblUserCompanyUnit WHERE UserId = @UserId";
            var parameters = new List<SqlParameter> { new SqlParameter("@UserId", userId) };
            return aCommonInternalDal.DataContainerDataTable(query, parameters, "SSIDB");
        }
    }
}
