using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class PaymentTranscationDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public bool SaveCustPayment(PaymentTranscationMasterDao aCustPayment, List<PaymentTranscationDetailDao> aCustPaymentDetails)
        {

            bool status = false;

            ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
            aCustPayment.CustPayId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblPaymentTranscationMaster");


            if (SaveCustPayment(aCustPayment))
            {
               
                foreach (var custPaymentDetail in aCustPaymentDetails)
                {
                    custPaymentDetail.CustPayId = aCustPayment.CustPayId;
                    if (SaveCustDetail(custPaymentDetail))
                    {
                        status = true;
                    }
                    else
                    {
                        DeletePaymentMasterInfo(aCustPayment.CustPayId);
                    }
                }
            }

            return status;
        }

        private bool DeletePaymentMasterInfo(int custPayId)
        {
            string query = @"DELETE FROM tblPaymentTranscationMaster WHERE CustPayId = " + custPayId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public bool SaveCustPayment(PaymentTranscationMasterDao aCustPayment)
        {
            string insertQuery = @"INSERT INTO dbo.tblPaymentTranscationMaster
        (         
          CustomerMasterId ,            
          PaymentDate ,
          PaymentAmount ,
          PayType ,
          InvPaymentAmount,
          PaymentStatus,
          RefNo ,
          RefDate ,
          CreateBy ,
          CreateDate,
          IsProcess,
          IsReject
          ) values (" + aCustPayment.CustomerMasterId + "," +

                    "'" + aCustPayment.PaymentDate + "'," +
                    "'" + aCustPayment.PaymentAmount + "'," +
                    "'" + aCustPayment.PayType + "'," +

                    "'" + aCustPayment.InvPaymentAmount + "'," +
                    "'" + aCustPayment.PaymentStatus + "'," +

                    "'" + aCustPayment.RefNo + "'," +
                    "'" + (object)(aCustPayment.RefDate ?? (object)DBNull.Value) + "'," +
                    "'" + aCustPayment.CreateBy + "'," +
                    "'" + aCustPayment.CreateDate + "'," +
                    "'0'" + "," +
                    "'0'" +
                //"'" + aCustPayment.UpdateBy + "'," +
                //"'" + aCustPayment.UpdateDate + "'" +

                    ")";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool SaveCustDetail(PaymentTranscationDetailDao aCustPaymentDetail)
        {
            string insertQuery = @"INSERT INTO dbo.tblPaymentTranscationDetail
        ( 
          InvoiceId ,
          PaymentAmount ,
          CustPayId,
          BankAccId,
          CashAccId,AIT,Discount,
          PaymentRemarks
        )
            values (" +
                   "'" + aCustPaymentDetail.InvoiceId + "'," +
                    "" + aCustPaymentDetail.PaymentAmount + "," +
                    "" + aCustPaymentDetail.CustPayId + "," +
                    "" + aCustPaymentDetail.BankAccId + "," +
                    "" + aCustPaymentDetail.CashAccId + "," +
                    "" + aCustPaymentDetail.AIT + "," +
                    "" + aCustPaymentDetail.Discount + "," +
                    "'" + aCustPaymentDetail.Remarks + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable LoadPaymentData(string userID)
        {
            string query = @"SELECT tblInvoice.InvoiceId,CustPayDetailId,tblCustMaster.CustomerMasterId,CustomerName,InvoiceNo,CustomerCode,CustomerName,PaymentDate,
                             InvoiceNo,ComUnitName,tblPaymentTranscationDetail.PaymentAmount,* FROM dbo.tblPaymentTranscationMaster 
                             INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId
                             LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = tblPaymentTranscationDetail.InvoiceId
                             LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
                             WHERE IsProcess = 0 AND IsReject = 0 AND dbo.tblInvoice.ComUnitId IN (
                             SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userID +" )";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetPaymentProcess(string payId)
        {
            string query = @"SELECT tblPaymentTranscationMaster.PaymentAmount,dbo.tblPaymentTranscationDetail.PaymentAmount AS DetailPaymentAmount,AIT,Discount,*,CASE WHEN BankAccId IS NOT NULL AND BankAccId > 0 THEN 'Bank' ELSE 'Cash' END AS PayType2 FROM dbo.tblPaymentTranscationMaster
                             INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId
                             WHERE tblPaymentTranscationMaster.CustPayId = " + payId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ValidationDoble(string invoiceiD)
        {
            string query = @"SELECT * FROM dbo.tblPaymentTranscationMaster
LEFT JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE (CreateDate BETWEEN DATEADD(MINUTE,-5,GETDATE()) AND  DATEADD(MINUTE,5,GETDATE())) AND InvoiceId='" + invoiceiD + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool UpdatePaymentTranscationStatus(string payId)
        {
            string query = @"UPDATE dbo.tblPaymentTranscationMaster SET IsProcess = 1 WHERE CustPayId = " + payId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public bool UpdatePaymentTranscation(string custPayId)
        {
            string query = @"UPDATE dbo.tblPaymentTranscationMaster SET IsReject = 1 WHERE CustPayId = " + custPayId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable IsExist(string custPayId)
        {
            string query = @"SELECT TransctionDetailId FROM tblCustPayDetail WHERE TransctionDetailId = " + custPayId ;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
