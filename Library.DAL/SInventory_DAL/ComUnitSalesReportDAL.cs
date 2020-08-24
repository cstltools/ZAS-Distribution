using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class ComUnitSalesReportDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();


        public void LoadComUnit(DropDownList dropDownList)
        {
            string query = @"SELECT * from tblCompanyUnit ";
            aCommonInternalDal.LoadDropDownValue(dropDownList, "ComUnitName", "ComUnitId", query, "SSIDB");
        }

        public void LoadComUnit(DropDownList dropDownList,string comUnitId)
        {
            string query = @"SELECT * from tblCompanyUnit where ComUnitId='"+comUnitId+"'";
            aCommonInternalDal.LoadDropDownValue(dropDownList, "ComUnitName", "ComUnitId", query, "SSIDB");
        }
        
        public DataTable ComUnitReportMainDataDAL(string comunitId, DateTime fromDate, DateTime toDate)
        {
            string query = @"select (ComUnitCode+':'+ComUnitName) as ComUnitDetail,'" + fromDate + "' as FromDate, '" +
                           toDate + "' as ToDate from tblCompanyUnit where ComUnitId='" + comunitId.Trim() + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ComUnitReportDetailDataDAL(string comUnitId, DateTime fromDate, DateTime toDate)
        {
            string query =
                @"select ProductCode,Product, sum(TotalQuantity) as TotalQty ,sum(Price) as TotalAmount from View_MIAWiseSalesReport  where ComUnitId='" + comUnitId.Trim() + "' and InvoiceDate between '" + fromDate + "' and '" + toDate + "' group by ProductCode,Product";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
