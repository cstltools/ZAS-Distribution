using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class DashboardInfoDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public DataTable LoadUnitWiseMonthlySales(string companyId)
        {
            string queryStr = @"SELECT ISNULL(FLOOR(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*INVD.DeliveryQuantity))),0) AS NhietDo, MONTH(INV.InvoiceDate) AS MonthName
		FROM dbo.tblInvoice AS INV 
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
		WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial')  AND INV.ComUnitId = "+ companyId +" AND YEAR(INV.InvoiceDate) = '2019' GROUP BY MONTH(INV.InvoiceDate),YEAR(INV.InvoiceDate)";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }
    }
}
