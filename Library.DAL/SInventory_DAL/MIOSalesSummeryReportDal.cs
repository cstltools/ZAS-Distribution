using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class MIOSalesSummeryReportDal
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

        public DataTable GetMioSalesSummery(string parameter, string fromDate, string toDate)
        {

            string query = @"SELECT CI.CompanyName,UNT.ComUnitName AS Depot,INV.MiaId,EGIM.EmpMasterCode AS MIOId,
                            EGIM.EmpName AS MIO,EGIR.EmpMasterCode AS RSMId,EGIR.EmpName AS RSM,EGIA.EmpMasterCode AS ASMId,EGIA.EmpName AS ASM, 
                            SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) AS Sales,
                            ISNULL(TNZ.InTransit,0) AS InTransit,ISNULL(RTN.ReturnSales,0) AS ReturnSales,ISNULL(PMNT.SalesCollection,0) AS SalesCollection,ISNULL(D.Dues,0) AS Dues,
                            RGN.RegionName,ARA.AreaName,TTR.TerritoryName,'"+ fromDate +"' AS FromDate, '"+ toDate +"' AS ToDate FROM dbo.tblInvoice AS INV " 
                            +" LEFT JOIN tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId " 
                            +" LEFT JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId" 
                            +" LEFT JOIN dbo.tblRSMInfo AS RSM ON ODR.RSMId = RSM.RSMId" 
                            +" LEFT JOIN dbo.tblEmpGeneralInfo AS EGIR ON RSM.EmployeeId = EGIR.EmpInfoId " 
                            +" LEFT JOIN dbo.tblRegion AS RGN ON RSM.RegionId = RGN.RegionId" 
                            +" LEFT JOIN dbo.tblASMInfo AS ASM ON ODR.ASMId = ASM.ASMId" 
                            +" LEFT JOIN dbo.tblEmpGeneralInfo AS EGIA ON ASM.EmployeeId = EGIA.EmpInfoId " 
                            +" LEFT JOIN dbo.tblArea AS ARA ON ASM.AreaId = ARA.AreaId" 
                            +" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId" 
                            +" LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId" 
                            +" LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId "  
                            +" LEFT JOIN dbo.tblEmpGeneralInfo AS EGIM ON MIO.EmployeeId = EGIM.EmpInfoId"   
                            +" LEFT JOIN dbo.tblTerritory AS TTR ON MiO.TerritoryId = TTR.TerritoryId" 
                            +" LEFT JOIN (SELECT INV.MiaId," 
                            +" ISNULL(SUM(INVD.NetAmount),0)-ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0)  ReturnSales  FROM dbo.tblInvoice AS INV "   
                            +" LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId "  
                            +" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                            + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "   
                            +" WHERE INV.DeliveryInvoiceStatus IN ('Reject','Partial') " + parameter + " GROUP BY INV.MiaId) AS RTN ON INV.MiaId = RTN.MiaId "
                            + " LEFT JOIN (SELECT INV.MiaId,SUM(INVD.NetAmount) InTransit  FROM dbo.tblInvoice AS INV  "
                            +" LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId "
                            +" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                            +" LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId  "
                            + " WHERE INV.DeliveryInvoiceStatus IS NULL " + parameter + " GROUP BY INV.MiaId) AS TNZ ON INV.MiaId = TNZ.MiaId  " 
                            +" LEFT JOIN (SELECT INV.MiaId,SUM(ISNULL(PaymentAmount,0)) SalesCollection FROM dbo.tblInvoice AS INV  "
                            +" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  "
                            +" LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId  WHERE DeliveryInvoiceStatus IN ('Full','Partial') "
                            + parameter + " GROUP BY INV.MiaId) AS PMNT ON INV.MiaId = PMNT.MiaId  " 
                            +" LEFT JOIN (SELECT INV.MiaId,SUM(DeliveryTpGrandTotal)- SUM(ISNULL(PaymentAmount,0)) AS Dues FROM dbo.tblInvoice AS INV  "
                            +" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                            +" LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId  WHERE DeliveryInvoiceStatus IN ('Full','Partial') "
                            + parameter + " GROUP BY INV.MiaId) AS D ON INV.MiaId = D.MiaId "
                            + " WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial')  " + parameter 
                            +" GROUP BY CI.CompanyName,UNT.ComUnitName,INV.MiaId,EGIM.EmpMasterCode,EGIM.EmpName,EGIR.EmpMasterCode,EGIR.EmpName, "
                            +" EGIA.EmpMasterCode,EGIA.EmpName,RTN.ReturnSales,PMNT.SalesCollection,D.Dues,TNZ.InTransit,RGN.RegionName,ARA.AreaName,TTR.TerritoryName "
                            +" ORDER BY CI.CompanyName,UNT.ComUnitName,INV.MiaId";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GetMioWiseCustomerReturn(string parameter)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName AS Depot,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode AS MIOCode,
                             EGI.EmpName AS MIO,TTR.TerritoryCode,INV.InvoiceNo,
                             INV.InvoiceDate,INV.TpGrandTotal AS InvoiceValue,
                             ISNULL((CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END),0) AS SaleValue,
                             INV.TpGrandTotal-(CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END) AS ReturnValue FROM dbo.tblInvoice AS INV 
                             LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId
                             LEFT JOIN dbo.tblTerritory AS TTR ON MIO.TerritoryId = TTR.TerritoryId
                             LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             " + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode,EGI.EmpName,TTR.TerritoryCode,INV.InvoiceNo,INV.InvoiceDate,INV.TpGrandTotal,INV.DeliveryTpGrandTotal,INV.TpGrandTotal HAVING INV.TpGrandTotal-(CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END) > 0 ORDER BY CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode,EGI.EmpName,INV.InvoiceNo,INV.InvoiceDate";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadMIOInformationByUserDC(DropDownList ddl, string dcId, string comId, string userId)
        {
            string query = @"SELECT DISTINCT MIO.MIOId,TTR.TerritoryCode + ' : ' + EGI.EmpName AS MIOName FROM dbo.tblMIOInfo AS MIO
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                             INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId 
                             INNER JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId
                             LEFT JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARA.AreaId 
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON DCA.DCId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             WHERE MIO.IsActive = 1 AND 
                             DCA.DCId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ") AND MIO.CompanyId = " + comId + " AND UNT.ComUnitId = " + dcId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "MIOName", "MIOId", query, "SSIDB");
        }

        public void GetWhInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }
    }
}
