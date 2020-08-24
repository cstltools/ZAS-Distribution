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
    public class DCUserPermissionDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetUserInfo(DropDownList ddl)
        {
            string query = @"SELECT UserId,UserCode + ':' + UserName AS UserName FROM tblUser";
            aCommonInternalDal.LoadDropDownValue(ddl, "UserName", "UserId", query, "SSIDB");
        }

        public DataTable GetDCList()
        {
            string query = @"SELECT ComUnitId, ComUnitCode + ':'+ ComUnitName AS CompanyUnit FROM tblCompanyUnit UNION ALL SELECT 15,WearhouseCode + ':' + WearhouseName AS WearhouseName FROM tblWearhouse";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveDCUserPermissionInfo(UserCompanyUnitDao aUserCompanyUnitDao)
        {
            string query = @"INSERT INTO tblUserCompanyUnit (UserId,CompanyUnitId,CWHPermission,NationalReportPermission) VALUES ('" + aUserCompanyUnitDao.UserId + "' , '"
                + aUserCompanyUnitDao.CompanyUnitId + "','" + aUserCompanyUnitDao.CWHPermission + "','" + aUserCompanyUnitDao.NationalReportPermission + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
        }

        public DataTable CheckPermissionInfoAlreadyExistOrNot(UserCompanyUnitDao aUserCompanyUnitDao)
        {
            string query = @"SELECT * FROM tblUserCompanyUnit WHERE UserId = '" + aUserCompanyUnitDao.UserId + "' AND CompanyUnitId = '" + aUserCompanyUnitDao.CompanyUnitId + "' AND CWHPermission = '" + aUserCompanyUnitDao.CWHPermission + "' AND NationalReportPermission = '" + aUserCompanyUnitDao.NationalReportPermission + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DeletePermissionInfo(string userId)
        {
            string query = @"DELETE FROM tblUserCompanyUnit WHERE UserId = '" + userId + "'";
            return aCommonInternalDal.DeleteDataByDeleteCommand(query, "SSIDB");
        }

        public DataTable GetDCUserPermissionById(string userId)
        {
            string query = @"SELECT * FROM tblUserCompanyUnit WHERE UserId = '" + userId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
