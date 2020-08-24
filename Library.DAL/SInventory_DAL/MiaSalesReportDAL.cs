using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
     
    public class MiaSalesReportDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public void LoadMiaByComUnit(DropDownList dropDownList, string comUnitId)
        {
            string query = @"SELECT DISTINCT View_CustomerMaster.MiaId,View_CustomerMaster.MiaName FROM dbo.tblMIAInfo
            LEFT JOIN dbo.View_CustomerMaster ON dbo.tblMIAInfo.MiaId = dbo.View_CustomerMaster.MiaId WHERE ComUnitId='" + comUnitId.Trim() + "'";
            aCommonInternalDal.LoadDropDownValue(dropDownList, "MiaName", "MiaId", query, "SSIDB");
        }
        public void LoadComUnit(DropDownList dropDownList)
        {
            string query = @"SELECT * from tblCompanyUnit ";
            aCommonInternalDal.LoadDropDownValue(dropDownList, "ComUnitName", "ComUnitId", query, "SSIDB");
        }

        public void LoadComUnit(DropDownList dropDownList, string comUnitId)
        {
            string query = @"SELECT * from tblCompanyUnit where ComUnitId='" + comUnitId + "'";
            aCommonInternalDal.LoadDropDownValue(dropDownList, "ComUnitName", "ComUnitId", query, "SSIDB");
        }
        
        public DataTable MiaWiseReportMainDataDAL(string miaId,DateTime fromDate,DateTime toDate,string comunitId)
        {
            string query = @"SELECT DISTINCT MiaId, MiaCode+':'+MiaName AS MIA, DistrictCode+':'+DistrictName AS District, ComUnitCode+':'+ComUnitName AS CompanyUnit, 
'"+fromDate+"' AS FromDate, '"+toDate+"' AS ToDate FROM dbo.View_CustomerMaster  WHERE MiaId='"+miaId+"' AND MiaId IN (SELECT MiaId FROM dbo.tblMIAInfo WHERE MiaId='"+miaId+"') AND ComUnitId='"+comunitId+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable MiaWiseReportDetailDataDAL(string miaId,DateTime fromDate,DateTime toDate)
        {
            string query = @"select InvoiceDate, MiaId,ProductCode,Product,sum(TotalQuantity) as TotalQuantity,sum(Price) as Price from  View_MIAWiseSalesReport where MiaId ='" + miaId + "' and InvoiceDate between '" + fromDate + "' and  '" + toDate + "'  group by InvoiceDate, MiaId,ProductCode,Product";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

    }
}
