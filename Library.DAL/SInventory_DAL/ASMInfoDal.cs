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
    public class ASMInfoDal
    {

        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void GetRegionDropdownList(DropDownList ddl, string companyId)
        {
            const string queryStr = @"SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive = 'True'";
            aCommonInternalDal.LoadDropDownValue(ddl, "RegionName", "RegionId", queryStr, "SSIDB");
        }

        public void GetEmployeeDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo ORDER BY EmpInfoId DESC";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }

        public bool SaveASMInformation(ASMInfoDao aInformationDao)
        {

            string insertQuery = @"INSERT INTO tblASMInfo (CompanyId,AreaId,EmployeeId,IsActive,ActiveDate,EntryBy,EntryDate) VALUES ('" + aInformationDao.CompanyId + "','" + aInformationDao.AreaId
                                       + "','" + aInformationDao.EmployeeId + "','" + aInformationDao.IsActive + "','" + aInformationDao.ActiveDate + "','" + aInformationDao.EntryBy + "','" + aInformationDao.EntryDate + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable GetGridviewData()
        {
            const string queryStr = @"SELECT * FROM tblASMInfo AS ASM 
                                       INNER JOIN tblCompanyInfo AS CI ON CI.CompanyId = ASM.CompanyId                                      
                                       INNER JOIN tblArea AS AR ON ASM.AreaId = AR.AreaId
                                       INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                       LEFT JOIN tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = ASM.EmployeeId";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetASMInfoByID(string asmId)
        {
            string queryStr = @"SELECT * FROM tblASMInfo AS ASM 
                                INNER JOIN tblArea AS AR ON ASM.AreaId = AR.AreaId
                                LEFT JOIN tblRegion AS RGN ON AR.AreaId = RGN.RegionId WHERE ASM.ASMId = '" + asmId + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public void GetRegionDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive = 'True'";
            aCommonInternalDal.LoadDropDownValue(ddl, "RegionName", "RegionId", queryStr, "SSIDB");
        }

        public bool UpdateASMInformation(ASMInfoDao asmInfo)
        {
            string queryStr = @"UPDATE tblASMInfo SET InActiveDate = '" + asmInfo.InActiveDate + "',IsActive = '" + asmInfo.IsActive + "',InActiveBy = '" + asmInfo.InActiveBy + "',UpdateBy = '" + asmInfo.UpdateBy + "',UpdateDate = '" + asmInfo.UpdateDate + "' WHERE ASMId = '" + asmInfo.ASMId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(queryStr, "SSIDB");
        }

        public bool GetCheckAssignedOrNot(string companyId, string areaId, int asmId)
        {
            bool status = false;

            string queryStr = @"SELECT * FROM tblASMInfo AS ASM WHERE ASM.ASMId != '" + asmId + "' AND ASM.CompanyId = '" + companyId + "' AND ASM.AreaId = '" + areaId + "' AND ASM.IsActive = 'True'";
            DataTable aTable = aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");

            if (aTable.Rows.Count > 0)
            {
                status = true;
            }

            return status;
        }

        public void GetAreaDropdownList(DropDownList ddl, string regionId, string companyId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                LEFT JOIN tblASMInfo AS ASM ON AR.AreaId = ASM.AreaId
                                WHERE AR.RegionId = '" + regionId + "' AND AR.IsActive = 'True' AND AR.AreaId not in (SELECT AreaId FROM tblASMInfo WHERE IsActive = 'True' AND CompanyId = '" + companyId + "')";
            aCommonInternalDal.LoadDropDownValue(ddl, "AreaName", "AreaId", queryStr, "SSIDB");
        }

        public void GetEmployeeDropdownList(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo  AS EMP 
                                WHERE EMP.EmpName != 'Vacant' AND Emp.CompanyId = '" + companyId + "' AND EmpInfoId NOT IN (SELECT ASM.EmployeeId FROM tblASMInfo AS ASM WHERE ASM.IsActive = 'True' AND ASM.CompanyId = '" + companyId + "') UNION SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo  AS EP WHERE EP.EmpName = 'Vacant' ORDER BY EmpInfoId DESC";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }

        public void GetAreaDropdownList(DropDownList ddl, string regionId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                LEFT JOIN tblASMInfo AS ASM ON AR.AreaId = ASM.AreaId
                                WHERE RGN.RegionId = '" + regionId + "'";
            aCommonInternalDal.LoadDropDownValue(ddl, "AreaName", "AreaId", queryStr, "SSIDB");
        }
    }
}
