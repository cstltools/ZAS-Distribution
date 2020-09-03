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
    public class RSMSetupDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void GetRegionDropdownList(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN
                                     LEFT JOIN tblRSMInfo AS RSM ON RGN.RegionId = RSM.RegionId
                                    WHERE RGN.IsActive = 'True' AND  RGN.RegionId not in (select RegionId from tblRSMInfo where IsActive = 'True' AND CompanyId = '" + companyId + "')";
            aCommonInternalDal.LoadDropDownValue(ddl, "RegionName", "RegionId", queryStr, "SSIDB");
        }

        public void GetEmployeeDropdownList(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo  AS EMP 
                                    WHERE EMP.EmpName != 'Vacant'  AND Emp.CompanyId = '" + companyId  + "' AND EmpInfoId NOT IN (SELECT RSM.EmployeeId FROM tblRSMInfo AS RSM WHERE RSM.IsActive = 'True' AND RSM.CompanyId = '" + companyId + "') UNION SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo  AS EP WHERE EP.EmpName = 'Vacant' ORDER BY EmpInfoId DESC";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }

        public bool SaveRSMInformation(RSMSetupDao aInformationDao)
        {

            string insertQuery = @"INSERT INTO tblRSMInfo (CompanyId,RegionId,EmployeeId,IsActive,ActiveDate,EntryBy,EntryDate) VALUES ('" + aInformationDao.CompanyId + "','" + aInformationDao.RegionId
                                       + "','" + aInformationDao.EmployeeId + "','" + aInformationDao.IsActive + "','" + aInformationDao.ActiveDate + "','" + aInformationDao.EntryBy + "','" + aInformationDao.EntryDate + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable GetGridviewData()
        {
            const string queryStr = @"SELECT * FROM tblRSMInfo AS RSM 
                                      INNER JOIN tblCompanyInfo AS CI ON CI.CompanyId = RSM.CompanyId
                                      INNER JOIN tblRegion AS RGN ON RGN.RegionId = RSM.RegionId
                                      LEFT JOIN tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = RSM.EmployeeId";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetRSMInfoByID(string rsmId)
        {
            string queryStr = @"SELECT * FROM tblRSMInfo AS RSM WHERE RSM.RSMId = '" + rsmId + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public void GetRegionDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN";
            aCommonInternalDal.LoadDropDownValue(ddl, "RegionName", "RegionId", queryStr, "SSIDB");
        }

        public bool UpdateRSMInformation(RSMSetupDao rsmInfo)
        {
            string queryStr = @"UPDATE tblRSMInfo SET InActiveDate = '" + rsmInfo.InActiveDate + "',IsActive = '" + rsmInfo.IsActive + "',InActiveBy = '" + rsmInfo.InActiveBy + "',UpdateBy = '" + rsmInfo.UpdateBy + "',UpdateDate = '" + rsmInfo.UpdateDate + "' WHERE RSMId = '" + rsmInfo.RSMId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(queryStr, "SSIDB");
        }

        public bool GetCheckAssignedOrNot(string companyId, string regionId, int rsmId)
        {
            bool status = false;

            string queryStr = @"SELECT * FROM tblRSMInfo AS RSM WHERE RSM.RSMId != '" + rsmId + "' AND RSM.CompanyId = '" + companyId + "' AND RSM.RegionId = '" + regionId + "' AND RSM.IsActive = 'True'";
            DataTable aTable = aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");

            if (aTable.Rows.Count > 0)
            {
                status = true;
            }

            return status;
        }

        public void GetEmployeeDropDownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo ORDER BY EmpInfoId DESC";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }
    }
}
