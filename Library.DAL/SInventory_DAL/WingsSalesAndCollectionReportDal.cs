using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class WingsSalesAndCollectionReportDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetGroupDropdownList(DropDownList ddl)
        {
            string queryStr = @"SELECT GroupId,GroupName FROM dbo.tblGroupInformation";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "GroupName", "GroupId", queryStr);
        }

        public void LoadCompanyName(DropDownList ddl, string groupId)
        {
            string queryStr = @"SELECT CI.CompanyId,CI.CompanyName FROM dbo.tblCompanyInfo AS CI WHERE CI.GroupId = " + groupId;
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void LoadDC(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT UNT.ComUnitId,UNT.ComUnitName FROM dbo.tblCompanyUnit AS UNT WHERE UNT.CompanyId =  " + companyId;
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public DataTable GetReportInfo(string parameter, DateTime fromDate, DateTime toDate)
        {
            string queryStr = @"SELECT CI.CompanyName,UNT.ComUnitName,TTR.TerritoryName,EGI.EmpMasterCode AS MIOCode,EGI.EmpName AS MIO,ISNULL(SUM(INV.TpGrandTotal),0) AS Amount,'Transit' AS ValueType FROM tblInvoice AS INV 
                                INNER JOIN dbo.tblOrder AS ODR ON ODR.OrderId = INV.OrderId
                                INNER JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = INV.MiaId
                                INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId
                                INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = ODR.ManufacId
                                INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
                                INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = MIO.EmployeeId
                                WHERE INV.DeliveryInvoiceStatus IS NULL AND MIO.IsActive = 1 " + parameter
                                + " GROUP BY INV.MiaId,EGI.EmpName,CI.CompanyName,UNT.ComUnitName,EGI.EmpMasterCode,TTR.TerritoryName " 
                                + " UNION SELECT CI.CompanyName,UNT.ComUnitName,TTR.TerritoryName,EGI.EmpMasterCode AS MIOCode,EGI.EmpName AS MIO,ISNULL(SUM(INV.DeliveryTpGrandTotal),0) AS Amount,'Actual Sales' AS ValueType FROM tblInvoice AS INV " 
                                + " INNER JOIN dbo.tblOrder AS ODR ON ODR.OrderId = INV.OrderId " 
                                + " INNER JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = INV.MiaId " 
                                + " INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId " 
                                + " INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId =MIO.EmployeeId " 
                                + " INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = ODR.ManufacId " 
                                + " INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId "
                                + " WHERE (INV.DeliveryInvoiceStatus IS NOT NULL OR INV.DeliveryInvoiceStatus != 'Reject') AND MIO.IsActive = 1 " + parameter
                                + " GROUP BY INV.MiaId,CI.CompanyName,UNT.ComUnitName,EGI.EmpMasterCode,EGI.EmpName,TTR.TerritoryName"
                                + " UNION SELECT CI.CompanyName,UNT.ComUnitName,TTR.TerritoryName,EGI.EmpMasterCode AS MIOCode,EGI.EmpName AS MIO,ISNULL(SUM(INV.PaymentAmount),0) AS Amount,'Collection' AS ValueType FROM dbo.tblInvoice AS INV  " 
                                + " INNER JOIN dbo.tblOrder AS ODR ON ODR.OrderId = INV.OrderId "
                                + " INNER JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = INV.MiaId "
                                + " INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId "
                                + " INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = MIO.EmployeeId "
                                + " LEFT JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = ODR.ManufacId "
                                + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId "
                                + " WHERE (INV.DeliveryInvoiceStatus IS NOT NULL OR INV.DeliveryInvoiceStatus != 'Reject') AND MIO.IsActive = 1 " + parameter
                                + " GROUP BY INV.MiaId,CI.CompanyName,UNT.ComUnitName,EGI.EmpMasterCode,EGI.EmpName,TTR.TerritoryName " 
                                + " UNION SELECT CI.CompanyName,UNT.ComUnitName,TTR.TerritoryName,EGI.EmpMasterCode AS MIOCode,EGI.EmpName AS MIO,ISNULL(ISNULL(SUM(INV.DeliveryTpGrandTotal),0) - ISNULL(SUM(INV.PaymentAmount),0),0) AS Amount,'Due' AS ValueType FROM dbo.tblInvoice AS INV "
                                + " INNER JOIN dbo.tblOrder AS ODR ON ODR.OrderId = INV.OrderId "
                                + " INNER JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = INV.MiaId "
                                + " INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId "
                                + " INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = MIO.EmployeeId "
                                + " LEFT JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = ODR.ManufacId "
                                + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId "
                                + " WHERE (INV.DeliveryInvoiceStatus IS NOT NULL OR INV.DeliveryInvoiceStatus != 'Reject') AND MIO.IsActive = 1 " + parameter
                                + " GROUP BY INV.MiaId,CI.CompanyName,UNT.ComUnitName,EGI.EmpMasterCode,EGI.EmpName,TTR.TerritoryName "
                                + " ORDER BY EGI.EmpName,ValueType";
            return aCommonInternalDal.DataContainerDataTable(queryStr);
        }
    }
}
