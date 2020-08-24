using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
   public class ProformaOrInvoiceReturn
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public bool HasProforma(string Proforma)
        {
            string query = "select InvoiceNo from tblInvoice where DelivaryInvoiceNo is null and InvoiceNo = '" + Proforma + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    return true;
                }
            }
            return false;
        }
        //public bool HasInvoice(string Invoice)
        //{
        //    string query = "select DelivaryInvoiceNo from tblInvoice where DelivaryInvoiceNo = '" + Invoice + "'";
        //    IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
        //    if (dataReader != null)
        //    {
        //        while (dataReader.Read())
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public bool HasInvoice(string Invoice)
        {
            string query = "select DelivaryInvoiceNo from tblInvoice where DelivaryInvoiceNo = '" + Invoice + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    return true;
                }
            }
            return false;
        }
        public int DeleteProformaDal(string Invoice)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceCode", Invoice));
            return aCommonInternalDal.RunStoreProcedure("sp_Delete_ProformaInvoice", aSqlParameterList, "SSIDB");
        }
        public int DeleteDeliveyInvoiceDal(string Invoice)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@DelivaryInvoiceNo", Invoice));
            return aCommonInternalDal.RunStoreProcedure("sp_DeleteDeliveryInvoice", aSqlParameterList, "SSIDB");
        }
        public DataTable LoadDetailID(string Invoice)
        {
            DataTable InvoiceDetail = new DataTable();
            string query = @"select InvoiceDetailId from tblInvoice I
            inner join tblInvoiceDetail D on I.InvoiceId = D.InvoiceId
            where DelivaryInvoiceNo='" + Invoice.Trim() + "' ";
            InvoiceDetail = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return InvoiceDetail;
        }
        public DataTable LoadStock(string InvoiceDetailID)
        {
            DataTable InvoiceDetail = new DataTable();
            string query = @" select * from tblDCStoreFreeze where InvoiceDetailId='" + InvoiceDetailID + "' and StockQty<>TotalQuantity AND StockQty > 0";
            InvoiceDetail = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return InvoiceDetail;
        }

       public bool ProformaReturnPosting(string invoiceNo)
       {
           List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
           aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceNo));
           return aCommonInternalDal.UpdateAction("sp_ProformaReturnPosting", aSqlParameterList);
       }

       public bool InvoiceDeletePosting(string invoiceNo)
       {
           List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
           aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceNo));
           return aCommonInternalDal.UpdateAction("sp_DeliveryInvoiceDeletePosting", aSqlParameterList);
       }
    }
}
