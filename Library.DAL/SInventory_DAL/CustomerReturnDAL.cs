using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerReturnDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public int SaveCustomerReturn(CustomerReturnMasterDAO aCustomerReturnMasterDao) 
        {
             List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

             aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aCustomerReturnMasterDao.InvoiceId));
             aSqlParameterlist.Add(new SqlParameter("@TotalQty", aCustomerReturnMasterDao.TotalQty));
             aSqlParameterlist.Add(new SqlParameter("@TotalAmount", aCustomerReturnMasterDao.TotalAmount));
             aSqlParameterlist.Add(new SqlParameter("@EntryBy", aCustomerReturnMasterDao.EntryBy));
             aSqlParameterlist.Add(new SqlParameter("@EntryDate", aCustomerReturnMasterDao.EntryDate));
             aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aCustomerReturnMasterDao.ActionStatus));
            
            string insertQuery = @"INSERT INTO dbo.tblCustomerReturnMaster
                                    (
                                        InvoiceId,
                                        TotalQty,
                                        TotalAmount,
                                        EntryBy,
                                        EntryDate,ActionStatus                                        
                                    )
                                    VALUES
                                    (   @InvoiceId,
                                        @TotalQty,
                                        @TotalAmount,
                                        @EntryBy,
                                        @EntryDate,@ActionStatus
                                    )";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterlist, "SSIDB");
        }
        public int SaveCustomerReturnDetail(CustomerReturnDetailDAO aCustomerReturnDetailDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@InvoiceDetailId", aCustomerReturnDetailDao.InvoiceDetailId));
            aSqlParameterlist.Add(new SqlParameter("@ReturnQty", aCustomerReturnDetailDao.ReturnQty));
            aSqlParameterlist.Add(new SqlParameter("@ReturnAmount", aCustomerReturnDetailDao.ReturnAmount));
            aSqlParameterlist.Add(new SqlParameter("@CustReturnId", aCustomerReturnDetailDao.CustReturnId));
            

            string insertQuery = @"INSERT INTO dbo.tblCustomerReturnDetail
                                    (
                                        InvoiceDetailId,
                                        ReturnQty,
                                        ReturnAmount,
                                        CustReturnId
                                    )
                                    VALUES
                                    (   @InvoiceDetailId,
                                        @ReturnQty,
                                        @ReturnAmount,
                                        @CustReturnId
                                    )";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterlist, "SSIDB");
        }

        public DataTable LoadInvoiceNew(string delinvno)
        {
            string query = @"SELECT ComUnitName,ROUND(DeliveryNetAmount/Quantity, 4) AS UnitPrice,*,(DeliveryTotalQuantity-ISNULL(tbltemp.Qty,0))AvialableQty,PackSize,NetAmount,tblInvoice.InvoiceId FROM dbo.tblInvoice
            LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
            LEFT JOIN tblCompanyUnit AS UNT ON tblInvoice.ComUnitId = UNT.ComUnitId
            LEFT JOIN (SELECT InvoiceDetailId,ISNULL(SUM(ReturnQty),0)Qty FROM dbo.tblCustomerReturnDetail GROUP BY InvoiceDetailId)AS  tbltemp
            ON tbltemp.InvoiceDetailId = tblInvoiceDetail.InvoiceDetailId
            LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount)Amnt FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp2 ON tbltemp2.InvoiceId = tblInvoice.InvoiceId
            WHERE DelivaryInvoiceNo='" + delinvno + "' AND ISNULL(IsDirect,0) = 0  AND tblInvoice.InvoiceId NOT IN (SELECT DISTINCT A.InvoiceId FROM dbo.tblCustomerReturnMaster A)";

            //AND ISNULL(PaymentAmount,0) = 0 

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadInvoiceNewConsumer(string delinvno)
        {
            string query = @"SELECT  *,(TotalQuantity-ISNULL(tbltemp.Qty,0))AvialableQty,PackSize,NetAmount,tblInvoice.InvoiceId FROM dbo.tblInvoice
            LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
            LEFT JOIN (SELECT InvoiceDetailId,ISNULL(SUM(ReturnQty),0)Qty FROM dbo.tblCustomerReturnDetail GROUP BY InvoiceDetailId)AS  tbltemp
            ON tbltemp.InvoiceDetailId = tblInvoiceDetail.InvoiceDetailId
            LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount)Amnt FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp2 ON tbltemp2.InvoiceId = tblInvoice.InvoiceId
             WHERE DelivaryInvoiceNo='" + delinvno + "' AND IsDirect=1";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool ApprovalUpdateDAL(CustomerReturnMasterDAO aCustomerReturnMasterDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CustReturnId", aCustomerReturnMasterDao.CustReturnId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aCustomerReturnMasterDao.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aCustomerReturnMasterDao.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aCustomerReturnMasterDao.ApprovedDate));

            string query = @"UPDATE tblCustomerReturnMaster SET ActionStatus=@ActionStatus,ApprovedBy=@ApprovedBy,ApprovedDate=@ApprovedDate WHERE CustReturnId=@CustReturnId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        } 
        public bool UpdateDCStoreQty(string dcstoreId,int qty)
        {
            string query = @"UPDATE dbo.tblDCStore SET StockQty=StockQty+" + qty + " WHERE DCStoreId='" + dcstoreId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public DataTable LoadCustomerReturnApproval(string actionstatus)
        {
            string query = @"SELECT InvoiceNo,TotalQty,TotalAmount,EntryBy,EntryDate,* FROM dbo.tblCustomerReturnMaster
LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = tblCustomerReturnMaster.InvoiceId
LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
            WHERE ActionStatus='" +actionstatus+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadInvoiceDataApproval(string id)
        {
            string query = @"SELECT * FROM dbo.tblCustomerReturnMaster
            LEFT JOIN dbo.tblCustomerReturnDetail ON tblCustomerReturnDetail.CustReturnId = tblCustomerReturnMaster.CustReturnId
            LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceDetailId = tblCustomerReturnDetail.InvoiceDetailId
            WHERE tblCustomerReturnMaster.CustReturnId='"+id+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ClsApprovalAction approvalAction = new ClsApprovalAction();

        public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        {
            approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        }
        public string LoadForApprovalConditionDAL(string pageName, string userName)
        {
            return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        }

        public bool PostDeliverySalesReturn(int custReturnId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@ReturnId", custReturnId));

            return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesReturnPosting", aSqlParameterList);
        }
    }
}
