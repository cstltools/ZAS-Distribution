using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class PaymentReverseDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetOrderInformationByOrderNo(string orderNo)
        {
            string query = @"SELECT INV.InvoiceId FROM dbo.tblOrder AS ODR 
                             LEFT JOIN dbo.tblInvoice AS INV ON ODR.OrderId = INV.OrderId
                             WHERE (INV.DeliveryInvoiceStatus IS NOT NULL AND INV.DeliveryInvoiceStatus NOT IN ('Reject')) AND ODR.OrderCode = '" + orderNo + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool ResetInvoicePaymentStatus(int inoiceId)
        {
            string query = @"UPDATE dbo.tblInvoice SET PaymentStatus = NULL,PaymentAmount = NULL WHERE InvoiceId = " + inoiceId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable GetPaymentInfoByInvoiceId(int inoiceId)
        {
            string query = @"SELECT PD.CustPayDetailId,PD.CustPayId FROM dbo.tblCustPayDetail AS PD WHERE PD.InvoiceId = " + inoiceId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DeletePaymentDetail(int payId)
        {
            string query = @"DELETE FROM dbo.tblCustPayDetail WHERE CustPayDetailId = " + payId;
            return aCommonInternalDal.DeleteDataByDeleteCommand(query, "SSIDB");
        }

        public bool DeletePaymentMaster(int payId)
        {
            string query = @"DELETE FROM dbo.tblCustomerPay WHERE CustPayId = " + payId;
            return aCommonInternalDal.DeleteDataByDeleteCommand(query, "SSIDB");
        }

        public bool InsertIntoDeleteLog(int payMaserId, int payDetailId, string user, DateTime date)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@payMaserId", payMaserId));
            aSqlParameterlist.Add(new SqlParameter("@payDetailId", payDetailId));
            aSqlParameterlist.Add(new SqlParameter("@EntryBy", user));
            aSqlParameterlist.Add(new SqlParameter("@EntryDate", date));

            return aCommonInternalDal.UpdateAction("sp_I_PaymentDeleteLog", aSqlParameterlist);
        }
    }
}
