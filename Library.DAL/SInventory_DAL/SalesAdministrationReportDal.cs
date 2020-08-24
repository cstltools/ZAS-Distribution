using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class SalesAdministrationReportDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public DataTable GetRSMInformation(string parameter)
        {
            string queryStr = @"SELECT dbo.tblEmpGeneralInfo.EmpMasterCode, dbo.tblEmpGeneralInfo.EmpName, dbo.tblCompanyInfo.CompanyName, dbo.tblRSMInfo.IsActive, dbo.tblRegion.RegionName, dbo.tblRSMInfo.ActiveDate, dbo.tblRSMInfo.InActiveDate, dbo.tblRSMInfo.EntryBy, dbo.tblRSMInfo.EntryDate, dbo.tblRSMInfo.UpdateBy, dbo.tblRSMInfo.UpdateDate
                          FROM  dbo.tblRSMInfo 
                        INNER JOIN dbo.tblCompanyInfo ON dbo.tblRSMInfo.CompanyId = dbo.tblCompanyInfo.CompanyId 
                        INNER JOIN dbo.tblRegion ON dbo.tblRSMInfo.RegionId = dbo.tblRegion.RegionId 
                        INNER JOIN dbo.tblEmpGeneralInfo ON dbo.tblRSMInfo.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId WHERE dbo.tblRSMInfo.IsActive = 'True'" + parameter;
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetASMInformation(string parameter)
        {
            string queryStr = @"SELECT DISTINCT CI.CompanyCode,CI.CompanyName, EGI.EmpMasterCode AS ASMCode, EGI.EmpName AS ASMNName,RGN.RegionCode, RGN.RegionName, AR.AreaCode, AR.AreaName,
                                    ASM.IsActive, ASM.ActiveDate FROM tblASMInfo AS ASM 
                                    INNER JOIN tblCompanyInfo AS CI ON CI.CompanyId = ASM.CompanyId                                      
                                    INNER JOIN tblArea AS AR ON ASM.AreaId = AR.AreaId
                                    INNER JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = AR.AreaId
                                    INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                    INNER JOIN tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = ASM.EmployeeId WHERE ASM.IsActive = 'True'" + parameter;
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetMIOInformation(string parameter)
        {
            string queryStr = @"SELECT DISTINCT CI.CompanyCode,CI.CompanyName,EGI.EmpMasterCode AS MIOCode, EGI.EmpName AS MIOName,RGN.RegionCode, RGN.RegionName, AR.AreaCode, 
                                AR.AreaName,TTR.TerritoryCode, TTR.TerritoryName,MIO.IsActive, MIO.ActiveDate 
                                FROM tblMIOInfo AS MIO  
                                INNER JOIN tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId
                                INNER JOIN tblCompanyInfo AS CI ON CI.CompanyId = MIO.CompanyId                                      
                                INNER JOIN tblArea AS AR ON TTR.AreaId = AR.AreaId
                                INNER JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = AR.AreaId
                                INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId
                                INNER JOIN tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = MIO.EmployeeId WHERE MIO.IsActive = 'True'" + parameter;
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

    
    }
}
