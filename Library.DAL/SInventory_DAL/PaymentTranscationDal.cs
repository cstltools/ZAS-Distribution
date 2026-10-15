using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DAL.MAIN_FUNCTION;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class PaymentTranscationDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();


        DB_Manager aDbManager = new DB_Manager();
        public void CreateConnection(string dbName)
        {
            aDbManager.CreateConnection(dbName);
        }
        public void CreateConnection()
        {
            aDbManager.CreateConnection(DB_Names.SALESDB);
        }
        public void CloseAllConnection()
        {
            aDbManager.CloseConnection();
        }
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

        private bool DeletePaymentMasterInfoNew(int custPayId)
        {
            //string query = @"DELETE FROM tblCustomerPay WHERE CustPayId = " + custPayId;
            //return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@CustPayId", custPayId));
            return aInternalDal.DeleteAction("sp_MRCP_DeletePayment", aSqlParameters);

        }

        public bool PaymentJournalPosting(int custPayId, int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CustPayId", custPayId));
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            return aCommonInternalDal.UpdateAction("sp_Sales_CustomerPaymentPosting", aSqlParameterList);
            //return aCommonInternalDal.UpdateAction("sp_CustomerPaymentPosting", aSqlParameterList);
        }


        // Whole-submission atomicity (P1-1/P1-3): Master + Detail + Journal
        // Posting for every selected invoice line, plus the single Money
        // Receipt insert covering them all, run inside ONE shared connection
        // and transaction. Any hard failure rolls back everything -- there is
        // no scenario where invoice 1 stays committed while invoice 2 fails,
        // and no scenario where the payment/journal commit but the receipt
        // does not. A duplicate-guard "skip" for an individual invoice line
        // (its master insert returns 0, no exception) is NOT a hard failure
        // and does not abort the submission -- that is existing, preserved
        // business behavior. Returns 0 only when nothing new was submitted
        // (every line was a duplicate-skip); throws on any genuine failure so
        // the caller never reports success for a payment that was rolled back.
        public Int32 SaveCustPaymentNew2(CustPayment aCustPayment, List<CustPaymentDetail> aCustPaymentDetails)
        {
            Int32 status = 0;
            List<int> savedDetailIds = new List<int>();

            aDbManager.CreateConnectionByConnectionStringKey("SolutionConnectionStringSSIDB");

            try
            {
                foreach (var custPaymentDetail in aCustPaymentDetails)
                {
                    // Save a new Master for each Detail

                    aCustPayment.InvoiceIdForValidation = custPaymentDetail.InvoiceId;

                    aCustPayment.CustomerMasterId = custPaymentDetail.CustomerMasterId;
                    aCustPayment.BatchId = custPaymentDetail.BatchId;
                    aCustPayment.PayType = custPaymentDetail.PayType;

                    aCustPayment.PaymentAmount = (custPaymentDetail.PaymentAmount != null ? custPaymentDetail.PaymentAmount : 0) +
                                                (custPaymentDetail.AIT != null ? custPaymentDetail.AIT : 0) +
                                                (custPaymentDetail.Discount != null ? custPaymentDetail.Discount : 0);

                    int custPayId = SaveCustomerPaymentMasterTx(aCustPayment);

                    if (custPayId <= 0)
                    {
                        // Duplicate-payment guard fired for this invoice line --
                        // existing behavior: skip it, keep processing the rest.
                        continue;
                    }

                    // Assign the new master to the detail

                    custPaymentDetail.CustPayId = custPayId;

                    int detailResult = SaveCustomerPaymentDetailTx(custPaymentDetail);

                    if (detailResult <= 0)
                    {
                        // Genuine failure (or an exact-duplicate detail row) --
                        // abort the entire submission. A rollback undoes the
                        // master insert above; no compensating delete needed.
                        throw new InvalidOperationException(
                            "Payment detail could not be saved for InvoiceId " + custPaymentDetail.InvoiceId + ".");
                    }

                    // Journal Posting -- any real failure throws (the stored
                    // procedure now wraps its body in TRY/CATCH + RAISERROR),
                    // which aborts the whole submission below.
                    PaymentJournalPostingTx(custPayId, custPaymentDetail.InvoiceId);

                    // Track detail for receipt
                    savedDetailIds.Add(detailResult);
                }

                if (savedDetailIds.Count > 0)
                {
                    string detailIdsCsv = string.Join(",", savedDetailIds);

                    var parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@MoneyReceiptDate", DateTime.Today),
                       // new SqlParameter("@MoneyReceiptDate", aCustPayment.PaymentDate != DateTime.MinValue ? (object)aCustPayment.PaymentDate : DateTime.Now),
                        new SqlParameter("@BatchId", aCustPayment.BatchId > 0 ? (object)aCustPayment.BatchId : DBNull.Value),
                        new SqlParameter("@CustomerMasterId", aCustPayment.CustomerMasterId > 0 ? (object)aCustPayment.CustomerMasterId : DBNull.Value),
                        new SqlParameter("@CustPayDetailsId", detailIdsCsv),
                        new SqlParameter("@CreateBy", aCustPayment.CreateBy)
                    };

                    int moneyReceiptId = aDbManager.SaveActionStrict("sp_MRCP_MoneyReceiptMasterInsert", parameters, "@MoneyReceiptID");

                    if (moneyReceiptId <= 0)
                    {
                        // The receipt insert's own duplicate guard was removed
                        // (see the P1 migration) since it is now redundant with
                        // the payment-level guards above -- so getting 0 here
                        // means something genuinely went wrong. Do not report a
                        // successful payment with no receipt.
                        throw new InvalidOperationException("Money receipt could not be created for this payment.");
                    }

                    status = moneyReceiptId;
                }

                aDbManager.CloseConnection();
            }
            catch
            {
                aDbManager.RollbackAndClose();
                throw;
            }

            return status;
        }

        private int SaveCustomerPaymentMasterTx(CustPayment aPayment)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@CustomerMasterId", aPayment.CustomerMasterId > 0 ? aPayment.CustomerMasterId : 0));
            aSqlParameters.Add(new SqlParameter("@BatchId", aPayment.BatchId > 0 ? aPayment.BatchId : 0));
            aSqlParameters.Add(new SqlParameter("@PaymentDate", aPayment.PaymentDate != DateTime.MinValue ? (object)aPayment.PaymentDate : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@PaymentAmount", aPayment.PaymentAmount > 0 ? (object)aPayment.PaymentAmount : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@PayType", !string.IsNullOrEmpty(aPayment.PayType) ? (object)aPayment.PayType : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@PaymentRemarks", !string.IsNullOrEmpty(aPayment.PaymentRemarks) ? (object)aPayment.PaymentRemarks : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@RefNo", !string.IsNullOrEmpty(aPayment.RefNo) ? (object)aPayment.RefNo : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@RefDate", aPayment.RefDate.HasValue ? (object)aPayment.RefDate.Value : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@CreateBy", aPayment.CreateBy));
            aSqlParameters.Add(new SqlParameter("@InvoiceIdForValidation", aPayment.InvoiceIdForValidation));

            return aDbManager.SaveActionStrict("sp_PMNT_CustomerPaymentMasterInsert", aSqlParameters, "@CustPayId");
        }

        private int SaveCustomerPaymentDetailTx(CustPaymentDetail aPaymentDetail)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CustPayId", aPaymentDetail.CustPayId));
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", aPaymentDetail.InvoiceId));
            aSqlParameterList.Add(new SqlParameter("@PaymentAmount", aPaymentDetail.PaymentAmount > 0 ? (object)aPaymentDetail.PaymentAmount : 0));
            aSqlParameterList.Add(new SqlParameter("@BankAccId", aPaymentDetail.BankAccId > 0 ? (object)aPaymentDetail.BankAccId : 0));
            aSqlParameterList.Add(new SqlParameter("@CashAccId", aPaymentDetail.CashAccId > 0 ? (object)aPaymentDetail.CashAccId : 0));
            aSqlParameterList.Add(new SqlParameter("@Remarks", !string.IsNullOrEmpty(aPaymentDetail.Remarks) ? (object)aPaymentDetail.Remarks : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@AIT", aPaymentDetail.AIT > 0 ? (object)aPaymentDetail.AIT : 0));
            aSqlParameterList.Add(new SqlParameter("@Discount", aPaymentDetail.Discount > 0 ? (object)aPaymentDetail.Discount : 0));

            return aDbManager.SaveActionStrict("sp_PMNT_CustomerPaymentDetailInsert", aSqlParameterList, "@CustPayDetailId");
        }

        private void PaymentJournalPostingTx(int custPayId, int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CustPayId", custPayId));
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            aDbManager.ExecuteActionStrict("sp_Sales_CustomerPaymentPosting", aSqlParameterList);
        }


        public Int32 SaveCustPaymentNew(CustPayment aCustPayment, List<CustPaymentDetail> aCustPaymentDetails)
        {
            Int32 status = 0;
            bool allSaved = true;

            List<int> savedDetailIds = new List<int>();

            foreach (var custPaymentDetail in aCustPaymentDetails)
            {
                // Save a new Master for each Detail

                aCustPayment.InvoiceIdForValidation = custPaymentDetail.InvoiceId;

                aCustPayment.PaymentAmount = (custPaymentDetail.PaymentAmount != null ? custPaymentDetail.PaymentAmount : 0) +
                                            (custPaymentDetail.AIT != null ? custPaymentDetail.AIT : 0) +
                                            (custPaymentDetail.Discount != null ? custPaymentDetail.Discount : 0);


                int custPayId = SaveCustomerPaymentMaster(aCustPayment);

                if (custPayId > 0)
                {
                    // Assign the new master to the detail

                    custPaymentDetail.CustPayId = custPayId;

                    int detailResult = SaveCustomerPaymentDetail(custPaymentDetail);

                    if (detailResult <= 0)
                    {
                        // Rollback just this pair

                        DeletePaymentMasterInfoNew(custPayId);
                        allSaved = false;
                        break;
                    }

                    // Journal Posting
                    PaymentJournalPosting(custPayId, custPaymentDetail.InvoiceId);

                    // Track detail for receipt
                    savedDetailIds.Add(detailResult);
                }

                
            }

            if (savedDetailIds.Count > 0)
            {
                string detailIdsCsv = string.Join(",", savedDetailIds);

                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@MoneyReceiptDate", aCustPayment.PaymentDate != DateTime.MinValue ? (object)aCustPayment.PaymentDate : DateTime.Now),
                    new SqlParameter("@BatchId", aCustPayment.BatchId > 0 ? (object)aCustPayment.BatchId : DBNull.Value),
                    new SqlParameter("@CustomerMasterId", aCustPayment.CustomerMasterId > 0 ? (object)aCustPayment.CustomerMasterId : DBNull.Value),
                    new SqlParameter("@CustPayDetailsId", detailIdsCsv),
                    new SqlParameter("@CreateBy", aCustPayment.CreateBy)
                };

                int moneyReceiptId = aInternalDal.SaveAction("sp_MRCP_MoneyReceiptMasterInsert", parameters, "@MoneyReceiptID");

                if (moneyReceiptId > 0)
                {
                    status = moneyReceiptId;
                }
            }

            return status;
        }




        public int SaveCustomerPaymentMaster(CustPayment aPayment)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@CustomerMasterId", aPayment.CustomerMasterId > 0 ? aPayment.CustomerMasterId : 0));
            aSqlParameters.Add(new SqlParameter("@BatchId", aPayment.BatchId > 0 ? aPayment.BatchId : 0));
            aSqlParameters.Add(new SqlParameter("@PaymentDate", aPayment.PaymentDate != DateTime.MinValue ? (object)aPayment.PaymentDate : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@PaymentAmount", aPayment.PaymentAmount > 0 ? (object)aPayment.PaymentAmount : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@PayType", !string.IsNullOrEmpty(aPayment.PayType) ? (object)aPayment.PayType : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@PaymentRemarks", !string.IsNullOrEmpty(aPayment.PaymentRemarks) ? (object)aPayment.PaymentRemarks : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@RefNo", !string.IsNullOrEmpty(aPayment.RefNo) ? (object)aPayment.RefNo : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@RefDate",aPayment.RefDate.HasValue? (object)aPayment.RefDate.Value: DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@CreateBy", aPayment.CreateBy));
            

            //For Validation

            aSqlParameters.Add(new SqlParameter("@InvoiceIdForValidation", aPayment.InvoiceIdForValidation));



            return aInternalDal.SaveAction("sp_PMNT_CustomerPaymentMasterInsert", aSqlParameters, "@CustPayId");
        }


        public int SaveCustomerPaymentDetail(CustPaymentDetail aPaymentDetail)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CustPayId", aPaymentDetail.CustPayId));
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", aPaymentDetail.InvoiceId));
            aSqlParameterList.Add(new SqlParameter("@PaymentAmount", aPaymentDetail.PaymentAmount > 0 ? (object)aPaymentDetail.PaymentAmount : 0));
            aSqlParameterList.Add(new SqlParameter("@BankAccId", aPaymentDetail.BankAccId > 0 ? (object)aPaymentDetail.BankAccId : 0));
            aSqlParameterList.Add(new SqlParameter("@CashAccId", aPaymentDetail.CashAccId > 0 ? (object)aPaymentDetail.CashAccId : 0));
            aSqlParameterList.Add(new SqlParameter("@Remarks", !string.IsNullOrEmpty(aPaymentDetail.Remarks) ? (object)aPaymentDetail.Remarks : DBNull.Value));
            //aSqlParameterList.Add(new SqlParameter("@TransctionDetailId", aPaymentDetail.TransctionDetailId > 0 ? (object)aPaymentDetail.TransctionDetailId : DBNull.Value));
            //aSqlParameterList.Add(new SqlParameter("@ProcessTime", aPaymentDetail.ProcessTime != DateTime.MinValue ? (object)aPaymentDetail.ProcessTime : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@AIT", aPaymentDetail.AIT > 0 ? (object)aPaymentDetail.AIT : 0));
            aSqlParameterList.Add(new SqlParameter("@Discount", aPaymentDetail.Discount > 0 ? (object)aPaymentDetail.Discount : 0));


            return aInternalDal.SaveAction("sp_PMNT_CustomerPaymentDetailInsert", aSqlParameterList, "@CustPayDetailId");
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
            DataTable aTable = new DataTable();

            string query2 = @"SELECT * FROM tblPaymentTranscationDetail AS D
LEFT JOIN tblPaymentTranscationMaster AS M ON D.CustPayId = M.CustPayId
WHERE IsReject = 0 AND PaymentDate BETWEEN CONVERT(DATE,GETDATE()) AND CONVERT(DATE,GETDATE()) AND InvoiceId = " + aCustPaymentDetail.InvoiceId + " AND D.PaymentAmount = " + aCustPaymentDetail.PaymentAmount + " AND D.AIT = " + aCustPaymentDetail.AIT + " AND D.Discount = " + aCustPaymentDetail.Discount;

            aTable = aCommonInternalDal.DataContainerDataTable(query2, "SSIDB");


            if (aTable.Rows.Count == 0)
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
            else
            {
                // Delete Payment Transcation Master


                string query = @"DELETE FROM tblPaymentTranscationMaster WHERE CustPayId = " + aCustPaymentDetail.CustPayId;
                return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
            }


            
        }

        public DataTable LoadPaymentData(string userID,string pram)
        {
            string query = @"SELECT CASE WHEN PB.TranscationDetailId IS NULL THEN 'No' ELSE 'Yes' END AS IsBatchCreated,tblInvoice.InvoiceId,CustPayDetailId,tblCustMaster.CustomerMasterId,CustomerName,InvoiceNo,CustomerCode,CustomerName,PaymentDate,
                             InvoiceNo,ComUnitName,tblPaymentTranscationDetail.PaymentAmount,EGI.EmpMasterCode + ' : ' + EGI.EmpName AS DeliveryMan,CONVERT(NVARCHAR(MAX),IVB.Date,107) + ' (' + CONVERT(NVARCHAR(MAX),IVB.BatchNo) +')' AS InvoiceBatch,* FROM dbo.tblPaymentTranscationMaster 
                             INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId
                             LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = tblPaymentTranscationDetail.InvoiceId
                             LEFT JOIN (SELECT TranscationDetailId FROM PaymentBatchDetails AS PBD) AS PB ON tblPaymentTranscationDetail.CustPayDetailId = PB.TranscationDetailId
                             LEFT JOIN tblInvoiceBatch AS IVB ON tblInvoice.BatchId = IVB.BatchId
                             LEFT JOIN tblDeliveryManInfo AS DM ON tblInvoice.DeliveryManId = DM.DeliveryManId
                             LEFT JOIN tblEmpGeneralInfo AS EGI ON DM.EmpInfoId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
                             WHERE IsProcess = 0 AND IsReject = 0 " + pram + " AND dbo.tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userID + " )";

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


        public DataTable GetCashGlByUnitId(string unitId)
        {
            string query = @"SELECT ISNULL(CashGlId,0) AS CashGlId FROM tblCompanyUnit WHERE ComUnitId = @ComUnitId";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@ComUnitId", unitId));

            return aCommonInternalDal.DataContainerDataTable(query, aSqlParameters, "SSIDB");
        }


        public Int32 GenerateBatch(PaymentBachMasterDao master, List<PaymentBatchDetailDao> detailsList)
        {
            int pk = 0;

            try
            {
                CreateConnection();
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                DateTime entryDtae = DateTime.Now;

                gSqlParameterList.Add(new SqlParameter("@PayCustomerId", master.PayCustomerId));
                gSqlParameterList.Add(new SqlParameter("@DeliveryManId", master.DeliveryManId));
                gSqlParameterList.Add(new SqlParameter("@EntryBy", master.EntryBy));

                pk = aDbManager.SaveAction("sp_Save_PaymentBatchMaster", gSqlParameterList, "@BatchMasterId");
                
                if (pk > 0)
                {
                    foreach (var item in detailsList)
                    {
                        List<SqlParameter> aSQL = new List<SqlParameter>();

                        aSQL.Add(new SqlParameter("@BatchMasterId", pk));
                        aSQL.Add(new SqlParameter("@TranscationDetailId", item.TranscationDetailId));

                        aDbManager.SaveAction("sp_Save_PaymentBatchDetail", aSQL);

                    }
                }

            }
            catch (Exception exception)
            {
               CloseAllConnection();

                throw exception;
            }
            finally
            {

                CloseAllConnection();
            }

            return pk;
        }


        public DataTable CheckExistOrNot(string custPayDetailId)
        {
            string query = @"SELECT * FROM PaymentBatchDetails WHERE TranscationDetailId  = " + custPayDetailId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerPayment(string getPrameterList)
        {
            string query = @"SELECT M.BatchMasterId,BatchNo,USR.UserName,BatchDate,PP.totalPay,EGI.EmpMasterCode + ' : ' + EGI.EmpName AS DeliveryMan,CS.CustomerCode + ' : ' + CS.CustomerName AS Customer,ISNULL(TotalPending,0) TotalPending FROM PaymentBachMaster AS M
LEFT JOIN tblUser AS USR ON M.EntryBy = USR.UserId 
LEFT JOIN tblCustMaster AS CS ON M.PayCustomerId = CS.CustomerMasterId
LEFT JOIN tblDeliveryManInfo AS DL ON M.DeliveryManId = DL.DeliveryManId
LEFT JOIN tblEmpGeneralInfo AS EGI ON DL.EmpInfoId = EGI.EmpInfoId
LEFT JOIN (SELECT BD.BatchMasterId,SUM(PD.PaymentAmount) AS totalPay FROM PaymentBatchDetails AS BD
LEFT JOIN tblPaymentTranscationDetail AS PD ON BD.TranscationDetailId = PD.CustPayDetailId
LEFT JOIN tblPaymentTranscationMaster AS PM ON PD.CustPayId = PM.CustPayId
WHERE PM.IsReject = 0 GROUP BY BD.BatchMasterId) AS PP ON M.BatchMasterId = PP.BatchMasterId
LEFT JOIN (SELECT BatchMasterId,COUNT(BD.BatchDetailId) TotalPending FROM PaymentBatchDetails AS BD
LEFT JOIN tblPaymentTranscationDetail AS PD ON BD.TranscationDetailId = PD.CustPayDetailId
LEFT JOIN tblPaymentTranscationMaster AS PM ON PD.CustPayId = PM.CustPayId
WHERE PM.IsReject = 0 AND IsProcess = 0 GROUP BY BD.BatchMasterId) AS PD ON M.BatchMasterId = PD.BatchMasterId
WHERE M.BatchMasterId IS NOT NULL " + getPrameterList + " ORDER BY M.BatchMasterId DESC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadPaymentReport(string paycode)
        {
            string query = @"SELECT M.BatchMasterId,M.BatchNo,CASE WHEN  M.PayCustomerId != 0 THEN CS2.CustomerCode + ' : ' + CS2.CustomerName ELSE EGI.EmpMasterCode + ' : ' + EGI.EmpName END UserName,BatchDate,
INV.InvoiceDate,CASE WHEN MT.IsReject = 1 THEN 0 ELSE PDT.PaymentAmount END  PaymentAmount, CASE WHEN MT.IsReject = 1 THEN 0 ELSE PDT.AIT END  AITAmount,CASE WHEN MT.IsReject = 1 THEN 0 ELSE PDT.Discount END  DiscountAmount,
CONVERT(VARCHAR(10), INVB.Date, 7) + ' (' + CONVERT(NVARCHAR(MAX),INVB.BatchNo) + ')' AS InvoiceBatch,CASE WHEN CashAccId != 0 THEN 'Cash' ELSE 'Bank' END PayType,
INV.InvoiceNo,CS.CustomerCode + ' : ' + CS.CustomerName AS Customer,ComUnitName,CASE WHEN MT.IsReject = 1 THEN 'Rejected' WHEN  MT.IsReject = 0 AND MT.IsProcess = 0 THEN 'Pending' ELSE  INV.PaymentStatus END PaymentStatus FROM PaymentBachMaster AS M
LEFT JOIN tblUser AS USR ON M.EntryBy = USR.UserId 
LEFT JOIN PaymentBatchDetails AS D ON M.BatchMasterId = D.BatchMasterId
LEFT JOIN tblCustMaster AS CS2 ON M.PayCustomerId = CS2.CustomerMasterId
LEFT JOIN tblDeliveryManInfo AS DL ON M.DeliveryManId = DL.DeliveryManId
LEFT JOIN tblEmpGeneralInfo AS EGI ON DL.EmpInfoId = EGI.EmpInfoId
LEFT JOIN tblPaymentTranscationDetail AS PDT ON D.TranscationDetailId = PDT.CustPayDetailId
LEFT JOIN tblPaymentTranscationMaster AS MT ON PDT.CustPayId = MT.CustPayId
LEFT JOIN tblPaymentType AS PT ON PDT.CustPayId = PT.PaymentTypeId
LEFT JOIN tblInvoice AS INV ON PDT.InvoiceId = INV.InvoiceId
LEFT JOIN tblInvoiceBatch AS INVB ON INV.BatchId = INVB.BatchId
LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
LEFT JOIN tblCustMaster AS CS ON INV.CustomerMasterId = CS.CustomerMasterId
WHERE M.BatchMasterId IS NOT NULL AND M.BatchMasterId = " + paycode;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool LockProcess(string batchMasterId)
        {

            try
            {
                CreateConnection();

                List<SqlParameter> aSQL = new List<SqlParameter>();

                aSQL.Add(new SqlParameter("@BatchMasterId", batchMasterId));
                return aDbManager.SaveAction("sp_PaymentBatchLockProcess", aSQL);

            }
            catch (Exception exception)
            {
                CloseAllConnection();
                throw exception;
            }
            finally
            {

                CloseAllConnection();
            }
           
        }

        public DataTable GetMoneyReceiptDetails(int moneyReceiptId)
        {
            string query = @"SELECT [MoneyReceiptId]
      ,[MoneyReceiptNo]
      ,[MonyReceiptDate]
      ,[BatchId]
      ,[CustMasterId]
      ,[CustPayDetailsId]
      ,[CreateBy]
      ,[CreateDate]
  FROM [dbo].[tblMoneyReceipt] WHERE MoneyReceiptId  = " + moneyReceiptId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetMoneyReceiptByUser(string userName)
        {
            string query = @"SELECT TOP 1 [MoneyReceiptId]
                             ,[MoneyReceiptNo]
                             ,[MonyReceiptDate]
                             ,[BatchId]
                             ,[CustMasterId]
                             ,[CustPayDetailsId]
                             ,[CreateBy]
                             ,[CreateDate]
                         FROM [dbo].[tblMoneyReceipt] WHERE CONVERT(DATE,CreateDate) = CONVERT(DATE,GETDATE()) AND LTRIM(RTRIM(UPPER(CreateBy))) = LTRIM(RTRIM(UPPER('" + userName + "')))  ORDER BY MoneyReceiptId DESC";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
