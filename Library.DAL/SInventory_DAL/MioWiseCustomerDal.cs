using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class MioWiseCustomerDal
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

        public void LoadMIOInformation(DropDownList ddl, string companyId)
        {
            string query = @"SELECT MIO.MIOId,TTR.TerritoryCode + ':' + EGI.EmpName AS MIOName FROM dbo.tblMIOInfo AS MIO
                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                            INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId WHERE MIO.IsActive = 1 AND MIO.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "MIOName", "MIOId", query, "SSIDB");
        }

        public void LoadWhInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");


        }

        public DataTable GetMioWiseCustomer(string parameter)
        {

            string query = @"SELECT DISTINCT CI.CompanyName AS Company,CV.CustomerCode AS Code,CV.CustomerName AS Customer,CV.MarketCode,CV.Address,
                             CV.MarketName AS Market,CV.TerritoryCode,CV.TerritoryName AS Territory,CV.AreaCode,CV.AreaName AS Area,CV.RegionCode,
                             CV.RegionName AS Region,MIO.MIOId,EGI.EmpMasterCode AS MIOCode, EGI.EmpName AS MIO FROM [dbo].[View_CustomerMaster] AS CV
                             LEFT JOIN dbo.tblMIOInfo AS MIO ON CV.TerritoryId = MIO.TerritoryId
                             LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON  MIO.CompanyId = CI.CompanyId
                             " + parameter + " ORDER BY CI.CompanyName,CV.CustomerCode,CV.RegionCode, CV.AreaCode,CV.TerritoryCode,CV.MarketCode,CV.Address";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        

    }
}
