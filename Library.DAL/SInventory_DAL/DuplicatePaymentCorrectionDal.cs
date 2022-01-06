using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class DuplicatePaymentCorrectionDal
    {
        private readonly ClsCommonInternalDAL _aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetDuplicatePaymentList()
        {
            const string query = @"SELECT DISTINCT UNT.ComUnitName,C.CustomerCode,C.Customername,INV.InvoiceId,InvoiceNo,InvoiceDate,DelivaryInvoiceNo,
                                   CPD.PaymentAmount,CP.PaymentDate FROM tblCustPayDetail AS CPD 
                                   LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId
                                   LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId 
                                   LEFT JOIN tblCustMaster AS C ON INV.CustomerMasterId = C.CustomerMasterId
                                   LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComunitId
                                   WHERE TransctionDetailId IN (SELECT TransctionDetailId FROM tblCustPayDetail
                                   GROUP BY TransctionDetailId HAVING COUNT(TransctionDetailId) > 1) ORDER BY UNT.ComUnitName";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public int DeleteDuplicatePayment(string Invoice)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", Invoice));
            return _aCommonInternalDal.RunStoreProcedure("sp_DEL_DuplicatePayment", aSqlParameterList, "SSIDB");
        }
    }
}
