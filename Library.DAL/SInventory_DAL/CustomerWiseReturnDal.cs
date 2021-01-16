using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerWiseReturnDal
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

        public void GetWhInformationByUser(DropDownList ddl, string companyId, string userId)
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

        public DataTable GetCustomerWiseReturn(string parameter,string fromDate,string toDate)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName AS Depot,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpMasterCode AS MIOId,
                             INV.InvoiceNo,INV.InvoiceDate,ISNULL(SUM(INVD.NetAmount),0) AS InvoiceValue,
                             ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0) AS SalesValue,
                             ISNULL(SUM(INVD.NetAmount),0)-ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0) 
                             AS ReturnValue,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM tblInvoice AS INV "
                             +" LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId"
                             +" LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId"
                             +" LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId"
                             +" LEFT JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId"
                             +" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId"
                             +" LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId"
                             + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpMasterCode,INV.InvoiceNo,INV.InvoiceDate "
                             + " HAVING ISNULL(SUM(INVD.NetAmount),0)-ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerWiseReturnByProduct(string parameter,string fromDate,string toDate)
        {
            string query = @"SELECT OrderNo,CI.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,INVD.ProductCode,INVD.ProductName,INVD.PackSize,ISNULL(SUM(TotalQuantity - DeliveryTotalQuantity),0)AS Qty,
                             ISNULL(SUM(INVD.TotalPrice),0)-ISNULL(SUM(CASE WHEN INVD.DeliveryTotalPrice < 0 THEN 0 ELSE INVD.DeliveryTotalPrice END),0) AS Value,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM dbo.tblInvoice AS INV" 
                             +" LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId" 
                             +" LEFT JOIN dbo.tblCustMaster AS CSTMR ON  INV.CustomerMasterId = CSTMR.CustomerMasterId" 
                             +" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId"
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId"
                            + parameter + " GROUP BY CI.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,INVD.ProductCode,INVD.ProductName,INVD.PackSize,OrderNo "
                            + " HAVING SUM(ISNULL(INVD.NetAmount,0) - ISNULL(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END,0)) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


     
        public void LoadCustomerInfoByUser(DropDownList ddl, string dcId, string companyId, string userId)
        {
            string query = @"SELECT DISTINCT CSTMR.CustomerMasterId,CSTMR.CustomerName FROM dbo.tblDcWiseAreaInfo AS DCA 
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON DCA.DCId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             LEFT JOIN dbo.tblArea AS ARA ON DCA.AreaId = ARA.AreaId 
                             LEFT JOIN dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId 
                             LEFT JOIN dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId
                             LEFT JOIN dbo.tblCustMaster AS CSTMR ON MKT.MarketId = CSTMR.MarketId WHERE CSTMR.CustomerMasterId IS NOT NULL AND " + "CI.CompanyId = " + companyId + " AND DCA.DCId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ") ORDER BY CSTMR.CustomerName";
            aCommonInternalDAL.LoadDropDownValue(ddl, "CustomerName", "CustomerMasterId", query, "SSIDB");
        }
    }
}
