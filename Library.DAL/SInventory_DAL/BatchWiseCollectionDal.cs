using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class BatchWiseCollectionDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable BatchWiseCollection(int batchId, DateTime batchCreationDate)
        {
            string query = @"SELECT INV.InvoiceNo,INV.InvoiceDate,BCH.BatchNo,BCH.Date AS BatchCreationDate,CP.PayType,
                            CPD.PaymentAmount AS PaidAmount ,CSTMR.CustomerName
                            FROM dbo.tblCustPayDetail AS CPD 
                            INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                            INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = CPD.CustPayId
                            INNER JOIN dbo.tblInvoiceBatch AS BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            WHERE INV.BatchId = " + batchId ;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
