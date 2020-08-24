using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerLedgerDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public DataTable CustomerLedgerDal(int CustomerID)
        {
            string query = @"SELECT 
            M.CustomerCode,M.CustomerName,I.OrderNo,I.OrderDate,I.InvoiceNo,I.InvoiceDate,M.ComUnitCode,M.ComUnitName,M.MarketCode,M.MarketName,I.TpGrandTotal,I.DeliveryTpGrandTotal,I.DelivaryInvoiceNo,I.UpdateDate,I.PaymentAmount,I.PaymentStatus
            FROM dbo.tblInvoice I
            INNER JOIN dbo.View_CustomerMaster M ON I.CustomerMasterId = M.CustomerMasterId
             WHERE M.CustomerCode= '" + CustomerID + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
