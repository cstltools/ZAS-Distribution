using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class DeleteInvoiceReportDAL
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void GetSalesCenter(DropDownList ddl)
        {
            string queryStr = "SELECT DISTINCT ComUnitId,ComUnitCode,ComUnitName +':'+ ComUnitCode as ComUnitName FROM dbo.tblCompanyUnit  WHERE ComUnitId IN (SELECT ComUnitId FROM dbo.tblCompanyUnit) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitCode", queryStr);
        }

        public void GetCustomer(DropDownList ddl)
        {
            string queryStr = "SELECT CustomerMasterId ,CustomerCode FROM dbo.tblCustMaster";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerCode", "CustomerMasterId", queryStr); ;
        }

        public DataTable GetDeleteOrderReport(string parameter)
        {
            string queryStr = @"SELECT '' AS ComUnitName, mas.InvoiceNo, RIGHT(mas.DelInvoiceNo, LEN(mas.DelInvoiceNo)-4) DelInvoiceNo, FORMAT(mas.DeleteDateTime,'dd-MM-yyyy') AS  DeleteDateTime, 
FORMAT(dtls.ReceiveDate,'dd-MM-yyyy') ReceiveDate, FORMAT(dtls.ExpDate,'dd-MM-yyyy') ExpDate, * FROM [dbo].[tblInvoiceDeleteLog] mas WITH (NOLOCK)
 
inner   JOIN tblInvoiceDetail_DeleterRecord dtls ON mas.InvoiceId = dtls.InvoiceId
 
WHERE mas.InvoiceId IS NOT NULL " + parameter;
            return aInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetDeleteOrderNationalReport(string parameter)
        {
            string queryStr = @"SELECT '' AS ComUnitName, mas.InvoiceNo, RIGHT(mas.DelInvoiceNo, LEN(mas.DelInvoiceNo)-4) DelInvoiceNo, FORMAT(mas.DeleteDateTime,'dd-MM-yyyy') AS  DeleteDateTime, 
FORMAT(dtls.ReceiveDate,'dd-MM-yyyy') ReceiveDate, FORMAT(dtls.ExpDate,'dd-MM-yyyy') ExpDate, * FROM [dbo].[tblInvoiceDeleteLog] mas WITH (NOLOCK)
 
inner   JOIN tblInvoiceDetail_DeleterRecord dtls ON mas.InvoiceId = dtls.InvoiceId
 
WHERE mas.InvoiceId IS NOT NULL  " + parameter;
            return aInternalDal.DataContainerDataTable(queryStr);
        }
    }
}
