using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class InvoiceBatchBLL
    {
        InvoiceBatchDAL  aInvoiceBatchDal=new InvoiceBatchDAL();
        public int SaveInvoiceBatch(InvoiceBatchDAO aInvoiceBatchDao)
        {
            DataTable dtdata = GetBatchNo(aInvoiceBatchDao.CompanyId.ToString(),
                aInvoiceBatchDao.DeliveryManId.ToString(), aInvoiceBatchDao.Date.ToString());
            aInvoiceBatchDao.BatchNo = Convert.ToInt32(dtdata.Rows[0][0].ToString());
            return aInvoiceBatchDal.SaveInvoiceBatch(aInvoiceBatchDao);
        }
        public void LoadManufac(DropDownList ddl)
        {
            aInvoiceBatchDal.LoadManufac(ddl);
        }
        public void LoadDeliveryMan(DropDownList aDropDownList)
        {
            aInvoiceBatchDal.LoadDeliveryMan(aDropDownList);
        }
        //public DataTable LoadInvoice(string companyId, string delManId, string invDate)
        //{
        //    return aInvoiceBatchDal.LoadInvoice(companyId, delManId, invDate);
        //}

        public DataTable LoadInvoice(string delManId, string invDate)
        {
            return aInvoiceBatchDal.LoadInvoice(delManId, invDate);
        }


        public DataTable LoadInvoiceNotNull(string delManId, string invDate, string batchId)
        {
            return aInvoiceBatchDal.LoadInvoiceNotNul(delManId, invDate, batchId);
        }

        public DataTable GetBatchNo(string companyId, string delManId, string invDate)
        {
            return aInvoiceBatchDal.GetBatchNo(companyId, delManId, invDate);
        }
        public void UpdateInvoiceBatchId(string invoiceId, string batchId)
        {
            aInvoiceBatchDal.UpdateInvoiceBatchId(invoiceId,batchId);
            
        }

        public DataTable LoadBatchInfo(string batchId)
        {
           return aInvoiceBatchDal.GetInvoiceBatchId(batchId);
        }

        public DataTable LoadInvoiceByUser(string delManId, string invDate, string userId)
        {
            return aInvoiceBatchDal.LoadInvoiceByUser(delManId, invDate,userId);
        }
    }
}
