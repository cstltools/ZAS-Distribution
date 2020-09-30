using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class ErrorInvoiceDal
    {
        private readonly ClsCommonInternalDAL _aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetErrorInvoiceList()
        {
            const string query = @"SELECT UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceId,INV.InvoiceNo,INV.InvoiceDate,INV.OrderNo,INV.OrderDate,INV.TpGrandTotal,INV.PaymentAmount FROM dbo.tblInvoice AS INV 
                                   LEFT JOIN dbo.tblInvoiceDetail AS IVD ON IVD.InvoiceId = INV.InvoiceId
                                   LEFT JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
                                   LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                                   WHERE IVD.InvoiceId IS NULL AND INV.Remarks != 'Due' AND INV.PaymentAmount IS NULL ORDER BY INV.InvoiceDate";
            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
