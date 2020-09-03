using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class BatchWiseTransitReportDal
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

        

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");


        }

        public void GetWhInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void LoadCustomerInfoInfo(DropDownList ddl, string dcId, string companyId)
        {
            string query = @"SELECT DISTINCT CSTMR.CustomerMasterId,CSTMR.CustomerName FROM dbo.tblDcWiseAreaInfo AS DCA 
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON DCA.DCId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             LEFT JOIN dbo.tblArea AS ARA ON DCA.AreaId = ARA.AreaId 
                             LEFT JOIN dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId 
                             LEFT JOIN dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId
                             LEFT JOIN dbo.tblCustMaster AS CSTMR ON MKT.MarketId = CSTMR.MarketId WHERE CSTMR.CustomerMasterId IS NOT NULL AND " + "CI.CompanyId = " + companyId + " AND DCA.DCId = " + dcId + " ORDER BY CSTMR.CustomerName";
            aCommonInternalDAL.LoadDropDownValue(ddl, "CustomerName", "CustomerMasterId", query, "SSIDB");
        }



        public DataTable GetBatchWiseTransitInfo(string parameter)
        {
            string query = @"SELECT DISTINCT INVB.BatchId,DM.DeliveryManId,CI.CompanyName,UNT.ComUnitName,INV.InvoiceDate,Invb.BatchNo,INVB.Date,EGI.EmpMasterCode AS DCode,EGI.EmpName AS Dname FROM dbo.tblInvoiceBatch AS INVB
                             LEFT JOIN dbo.tblInvoice AS INV ON INVB.BatchId = INV.BatchId
                             LEFT JOIN dbo.tblDeliveryManInfo AS DM ON INVB.DeliveryManId = DM.DeliveryManId
                             LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON DM.EmpInfoId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             WHERE INV.DeliveryInvoiceStatus IS NULL AND CI.CompanyName IS NOT NULL AND UNT.ComUnitName IS NOT NULL 
                             " + parameter + " ORDER BY INVB.Date, CI.CompanyName";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
