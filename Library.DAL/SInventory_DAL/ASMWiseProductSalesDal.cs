using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class ASMWiseProductSalesDal
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

        public void LoadASMInformation(DropDownList ddl, string companyId)
        {
            string query = @"SELECT ASM.ASMId,ARA.AreaCode + ' : ' + EGI.EmpName AS ASM FROM dbo.tblASMInfo AS ASM
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  ASM.EmployeeId = EGI.EmpInfoId
                             INNER JOIN dbo.tblArea AS ARA ON ARA.AreaId = ASM.AreaId WHERE ASM.IsActive = 1 AND ASM.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "ASM", "ASMId", query, "SSIDB");
        }

        public void LoadASMInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string query = @"SELECT DISTINCT ASM.ASMId,ARA.AreaCode + ' : ' + EGI.EmpName AS ASM FROM dbo.tblASMInfo AS ASM
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  ASM.EmployeeId = EGI.EmpInfoId
                             INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.CompanyId = ASM.CompanyId
                             INNER JOIN dbo.tblArea AS ARA ON ARA.AreaId = ASM.AreaId 
                             WHERE ASM.IsActive = 1 AND ASM.CompanyId = " + companyId +
                           " AND UNT.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "ASM", "ASMId", query, "SSIDB");
        }
       
        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");
        }
              
        public void GetDCByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public DataTable GetASMWiseProductSales(string parameter, string fromDate, string toDate)
        {
            string query = @"SELECT CI.CompanyCode ,CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,EmpName AS ASM,ARA.AreaName,INVD.ProductCode,INVD.ProductName,INVD.PackSize,ISNULL(SUM(DeliveryTotalQuantity),0)AS Qty,
                             SUM(INVD.DeliveryNetAmount) AS Value,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM dbo.tblInvoice AS INV "
                             + " LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId"
                             + " LEFT JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId"
                             + " LEFT JOIN dbo.tblASMInfo AS ASM ON  ODR.ASMId = ASM.ASMId"
                             + " LEFT JOIN dbo.tblEmpGeneralInfo ON ASM.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId"
                             + " LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = ASM.AreaId"
                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId"
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId"
                             + parameter + " GROUP BY  CI.CompanyCode ,CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,EmpName,ARA.AreaName,INVD.ProductCode,INVD.ProductName,INVD.PackSize "
                             + " HAVING SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) > 0";
            
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadASMInformationByUserDC(DropDownList ddl, string dcId, string companyId, string userId)
        {
            string query = @"SELECT DISTINCT ASM.ASMId,ARA.AreaCode + ' : ' + EGI.EmpName AS ASM FROM dbo.tblASMInfo AS ASM
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  ASM.EmployeeId = EGI.EmpInfoId
							 INNER JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ASM.AreaId
							 INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = DCA.DCId
							 INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
							 INNER JOIN dbo.tblArea AS ARA ON ARA.AreaId = ASM.AreaId 
                             WHERE ASM.IsActive = 1 AND ASM.CompanyId = " + companyId + "  AND UNT.ComUnitId = " + dcId +
                            " AND UNT.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "ASM", "ASMId", query, "SSIDB");
        }
    }
}
