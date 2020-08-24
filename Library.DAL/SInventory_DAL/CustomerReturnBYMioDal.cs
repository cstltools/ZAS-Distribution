using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerReturnBYMioDal
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

        public void LoadMIOInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string query = @"SELECT DISTINCT MIO.MIOId,TTR.TerritoryCode + ' : ' + EGI.EmpName AS MIOName FROM dbo.tblMIOInfo AS MIO
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                             INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId 
                             INNER JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId
                             LEFT JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARA.AreaId WHERE MIO.IsActive = 1 AND DCA.DCId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ") AND MIO.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "MIOName", "MIOId", query, "SSIDB");
        }

        public void GetWhInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
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

        public DataTable GetMioWiseProductSales(string parameter)
        {

            string query = @"SELECT CI.CompanyName,EmpName AS MIO,ARA.AreaName,TTR.TerritoryName,tblProduct.ProductCode,tblProduct.ProductName,tblProduct.PackSize,ISNULL(SUM(DeliveryTotalQuantity),0)AS Qty,ISNULL(SUM(DeliveryTotalQuantity * UnitPrice),0)AS Value FROM dbo.tblInvoice
                             LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
                             LEFT JOIN dbo.tblProduct ON tblProduct.ProductCode = tblInvoiceDetail.ProductCode
                             LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=dbo.tblInvoice.MiaId
                             LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId
                             LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId
                             LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = tblMIOInfo.CompanyId
                             " + parameter + " GROUP BY CI.CompanyName,EmpName,TTR.TerritoryName,ARA.AreaName,UnitPrice,tblProduct.ProductCode,tblProduct.PackSize,tblProduct.ProductName HAVING ISNULL(SUM(DeliveryTotalQuantity),0) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


    }
}
