using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class CustomerReturnBLL
    {
        CustomerReturnDAL aCustomerReturnDal=new CustomerReturnDAL();
        public int SaveCustomerReturn(CustomerReturnMasterDAO aCustomerReturnMasterDao)
        {
            return aCustomerReturnDal.SaveCustomerReturn(aCustomerReturnMasterDao);
        }
        public int SaveCustomerReturnDetail(CustomerReturnDetailDAO aCustomerReturnDetailDao)
        {
            return aCustomerReturnDal.SaveCustomerReturnDetail(aCustomerReturnDetailDao);
        }
        public DataTable LoadInvoiceNew(string delinvno)
        {
            return aCustomerReturnDal.LoadInvoiceNew(delinvno);
        }

        public bool UpdateDCStoreQty(string dcstoreId, int qty)
        {
            return aCustomerReturnDal.UpdateDCStoreQty(dcstoreId, qty);

        }

        public DataTable LoadInvoiceNewConsumer(string delinvno)
        {
            return aCustomerReturnDal.LoadInvoiceNewConsumer(delinvno);

        }

        public bool ApprovalUpdateDAL(CustomerReturnMasterDAO aCustomerReturnMasterDao)
        {
            return aCustomerReturnDal.ApprovalUpdateDAL(aCustomerReturnMasterDao);
        }
        public DataTable LoadCustomerReturnApproval(string actionstatus)
        {
            return aCustomerReturnDal.LoadCustomerReturnApproval(actionstatus);
        }

        public DataTable LoadInvoiceDataApproval(string id)
        {
            return aCustomerReturnDal.LoadInvoiceDataApproval(id);
        }

        public void LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)
        {
            aCustomerReturnDal.LoadApprovalControlDAL(rdl, pageName, userName);
        }
        public string LoadForApprovalConditionBLL(string pageName, string userName)
        {
            return aCustomerReturnDal.LoadForApprovalConditionDAL(pageName, userName);
        }

        public bool PostDeliverySalesReturn(int custReturnId)
        {
            return aCustomerReturnDal.PostDeliverySalesReturn(custReturnId);
        }
    }
}
