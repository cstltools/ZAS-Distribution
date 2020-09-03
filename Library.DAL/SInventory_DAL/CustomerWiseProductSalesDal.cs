using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerWiseProductSalesDal
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

        public DataTable GetMioWiseProductSales(string parameter,string fromDate,string toDate)
        {

            string query = @"SELECT CI.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,EmpName AS MIO,ARA.AreaName,TTR.TerritoryName,INVD.ProductCode,INVD.ProductName,INVD.PackSize,ISNULL(SUM(DeliveryTotalQuantity),0) AS Qty,
                             SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) AS Value,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM dbo.tblInvoice AS INV" 
                             + " LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId"
                             + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON  INV.CustomerMasterId = CSTMR.CustomerMasterId"
                             + " LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=INV.MiaId"
                             + " LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId"
                             + " LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId"
                             + " LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId"
                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId"
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId"
                             + parameter + " GROUP BY  CI.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,EmpName,ARA.AreaName,TTR.TerritoryName,INVD.ProductCode,INVD.ProductName,INVD.PackSize"
                             + " HAVING SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GetMioWiseCustomerReturn(string parameter)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName AS Depot,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpMasterCode AS MIOCode,EGI.EmpName AS MIO,EGI.EmpName AS MIO,TTR.TerritoryCode,
                          INV.InvoiceNo,INV.InvoiceDate,ISNULL(SUM(INVD.NetAmount),0) AS InvoiceValue,
                          ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0) AS SaleValue,
                          ISNULL(SUM(INVD.NetAmount),0)-ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0) 
                          AS ReturnValue FROM tblInvoice AS INV 
                          LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                          LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId
                          LEFT JOIN dbo.tblTerritory AS TTR ON MIO.TerritoryId = TTR.TerritoryId
                          LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                          LEFT JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
                          LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                          LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                          " + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpMasterCode,INV.InvoiceNo,INV.InvoiceDate,EGI.EmpName,TTR.TerritoryCode "
                          + " HAVING ISNULL(SUM(INVD.NetAmount),0)-ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
