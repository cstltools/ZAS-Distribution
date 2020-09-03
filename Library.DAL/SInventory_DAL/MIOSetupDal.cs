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
    public class MIOSetupDal
    {

        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }
        public void GetEmployeeDropdownList(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo WHERE CompanyId = '" + companyId + "' UNION SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo WHERE EmpName = 'Vacant' ORDER BY EmpInfoId DESC";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }

        public void GetRegionDropdownList(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN
                                LEFT JOIN tblRSMInfo AS RSM ON RGN.RegionId = RSM.RegionId
                                WHERE RGN.IsActive = 'True' AND  RGN.RegionId in (select RegionId from tblRSMInfo where IsActive = 'True' AND CompanyId = '" + companyId + "')";
            aCommonInternalDal.LoadDropDownValue(ddl, "RegionName", "RegionId", queryStr, "SSIDB");
        }

        public void GetEmployeeDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo ORDER BY EmpInfoId DESC";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }

        public bool SaveMIOInformation(MIOSetupDao aInformationDao)
        {

            string insertQuery = @"INSERT INTO tblMIOInfo (CompanyId,TerritoryId,EmployeeId,IsActive,ActiveDate,EntryBy,EntryDate) VALUES ('" + aInformationDao.CompanyId + "','" + aInformationDao.TerritoryId
                                       + "','" + aInformationDao.EmployeeId + "','" + aInformationDao.IsActive + "','" + aInformationDao.ActiveDate + "','" + aInformationDao.EntryBy + "','" + aInformationDao.EntryDate + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable GetGridviewData()
        {
            const string queryStr = @"SELECT * FROM tblMIOInfo AS MIO 
                                      INNER JOIN tblCompanyInfo AS CI ON CI.CompanyId = MIO.CompanyId   
                                      INNER JOIN tblTerritory AS TTR ON MIO.TerritoryId = TTR.TerritoryId                                   
                                      INNER JOIN tblArea AS AR ON TTR.AreaId = AR.AreaId
                                      INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                      LEFT JOIN tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = MIO.EmployeeId";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetMIOInfoByID(string mioId)
        {
            string queryStr = @"SELECT * FROM tblMIOInfo AS MIO 
                                INNER JOIN tblCompanyInfo AS CI ON CI.CompanyId = MIO.CompanyId   
                                INNER JOIN tblTerritory AS TTR ON MIO.TerritoryId = TTR.TerritoryId                                   
                                INNER JOIN tblArea AS AR ON TTR.AreaId = AR.AreaId
                                INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                LEFT JOIN tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = MIO.EmployeeId
									   WHERE MIO.MIOId ='" + mioId + "'";

            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public void GetRegionDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive = 'True'";
            aCommonInternalDal.LoadDropDownValue(ddl, "RegionName", "RegionId", queryStr, "SSIDB");
        }

        public bool UpdateMIOInformation(MIOSetupDao mioSetupDao)
        {
            string queryStr = @"UPDATE tblMIOInfo SET InActiveDate = '" + mioSetupDao.InActiveDate + "',IsActive = '" + mioSetupDao.IsActive + "',InActiveBy = '" + mioSetupDao.InActiveBy + "',UpdateBy = '" + mioSetupDao.UpdateBy + "',UpdateDate = '" + mioSetupDao.UpdateDate + "' WHERE MIOId = '" + mioSetupDao.MIOId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(queryStr, "SSIDB");
        }

        public bool GetCheckAssignedOrNot(string companyId, string territoryId, int mioId)
        {
            bool status = false;

            string queryStr = @"SELECT * FROM tblMIOInfo AS MIO WHERE MIO.MIOId != '" + mioId + "' AND MIO.CompanyId = '" + companyId + "' AND MIO.TerritoryId = '" + territoryId + "' AND MIO.IsActive = 'True'";
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
                                WHERE AR.RegionId = '" + regionId + "' AND AR.AreaId in (SELECT AreaId FROM tblASMInfo WHERE CompanyId = '" + companyId + "')";
            aCommonInternalDal.LoadDropDownValue(ddl, "AreaName", "AreaId", queryStr, "SSIDB");
        }

        public void GetAreaDropdownList(DropDownList ddl, string regionId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                LEFT JOIN tblASMInfo AS ASM ON AR.AreaId = ASM.AreaId
                                WHERE RGN.RegionId = '" + regionId + "'";
            aCommonInternalDal.LoadDropDownValue(ddl, "AreaName", "AreaId", queryStr, "SSIDB");
        }

        public void GetTerritoryDropdownList(DropDownList ddl, string areaId, string companyId)
        {
            string queryStr = @"SELECT distinct TR.TerritoryId, TR.TerritoryCode + ' : ' + TR.TerritoryName AS TerritoryName FROM tblTerritory AS TR 
                                INNER JOIN tblArea AS AR ON TR.AreaId = AR.AreaId
                                INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                LEFT JOIN tblMioInfo AS MIO ON TR.TerritoryId = MIO.TerritoryId
                                WHERE TR.AreaId = '" + areaId + "' AND TR.IsActive = 'True' AND TR.TerritoryId not in (SELECT TerritoryId FROM tblMIOInfo WHERE IsActive = 'True' AND CompanyId = '" + companyId + "')";
            aCommonInternalDal.LoadDropDownValue(ddl, "TerritoryName", "TerritoryId", queryStr, "SSIDB");
        }

        public void GetTerritoryDropdownList(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT distinct TR.TerritoryId, TR.TerritoryCode + ' : ' + TR.TerritoryName AS TerritoryName FROM tblTerritory AS TR 
                                INNER JOIN tblArea AS AR ON TR.AreaId = AR.AreaId
                                INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                LEFT JOIN tblMioInfo AS MIO ON TR.TerritoryId = MIO.TerritoryId
                                WHERE AR.AreaId = '" + areaId + "'";
            aCommonInternalDal.LoadDropDownValue(ddl, "TerritoryName", "TerritoryId", queryStr, "SSIDB");
        }
    }
}
