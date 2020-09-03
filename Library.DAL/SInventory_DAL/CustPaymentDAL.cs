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
    public class CustPaymentDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveCustPayment(CustPayment aCustPayment)
        {
            string insertQuery = @"INSERT INTO dbo.tblCustomerPay
        ( CustPayId ,
          
          CustomerMasterId ,
            
          PaymentDate ,
          PaymentAmount ,
          PayType ,
          RefNo ,
          RefDate ,
          CreateBy ,
          CreateDate 
          
        )

            values (" + aCustPayment.CustPayId + "," +
                   
                    "" + aCustPayment.CustomerMasterId + "," +
                 
                    "'" + aCustPayment.PaymentDate + "'," +
                    "'" + aCustPayment.PaymentAmount + "'," +
                    "'" + aCustPayment.PayType + "'," +
                    "'" + aCustPayment.RefNo + "'," +
                    "'" + (object)(aCustPayment.RefDate ?? (object)DBNull.Value) + "'," +
                    "'" + aCustPayment.CreateBy + "'," +
                    "'" + aCustPayment.CreateDate + "'" +
                    //"'" + aCustPayment.UpdateBy + "'," +
                    //"'" + aCustPayment.UpdateDate + "'" +
                                 
                    ")";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool SaveCustDetail(CustPaymentDetail aCustPaymentDetail)
        {
            string insertQuery = @"INSERT INTO dbo.tblCustPayDetail
        ( CustPayDetailId ,
          InvoiceId ,
          PaymentAmount ,
          CustPayId,
          BankAccId,
          CashAccId,
          TransctionDetailId,
          ProcessTime, 
          PaymentRemarks 
        )
            values (" + aCustPaymentDetail.CustPayDetailId+ "," +
                   "'" + aCustPaymentDetail.InvoiceId + "'," +
                    "" + aCustPaymentDetail.PaymentAmount + "," +
                    "" + aCustPaymentDetail.CustPayId + "," +
                    "" + aCustPaymentDetail.BankAccId + "," +
                    "" + aCustPaymentDetail.CashAccId + "," +
                    "" + aCustPaymentDetail.TransctionDetailId + "," +
                    "'" + aCustPaymentDetail.ProcessTime + "'," +
                    "'" + aCustPaymentDetail.Remarks + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public void LoadSC(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void LoadDC(DropDownList ddl, string companyId)
        {
            string queryStr = "select * from tblCompanyUnit where CompanyId='" + companyId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void LoadDcWiseAreaAloByDc(DropDownList ddl,string DCId)
        {
            string query = @"SELECT DCRA.AreaId, AR.AreaShortName + ' : ' + AR.AreaName AS AreaName FROM dbo.tblDcWiseAreaInfo AS DCRA
                            INNER JOIN tblArea AS AR ON DCRA.AreaId = AR.AreaId
                            WHERE DCRA.IsActive =  'True' AND DCRA.DCId = '" + DCId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", query);
        }


        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void LoadSC(DropDownList ddl, string userId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            //            string queryStr = @"SELECT * FROM dbo.tblCompanyUnit
            //                                LEFT JOIN dbo.tblUserCompanyUnit ON dbo.tblCompanyUnit.ComUnitId=dbo.tblUserCompanyUnit.CompanyUnitId
            //                                WHERE UserId='"+userId+"'";


            string queryStr = "select ComUnitId, ComUnitName  from tblCompanyUnit WHERE " +
                               " ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId='" + userId.Trim() + "')";


            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        public void LoadManufac(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblManufacturer";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ManufacName", "ManufacId", queryStr);
        }
        public bool UpdateInvoicePaymentAmount(string amount,string status,string id)
        {
            string query = @"UPDATE dbo.tblInvoice SET PaymentAmount='" + amount + "',PaymentStatus='"+status+"' WHERE InvoiceId='" + id + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        
        public void LoadCustomerMaster(DropDownList ddl, string marketId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCustMaster WHERE CustomerMasterId IN (SELECT DISTINCT CustomerMasterId FROM dbo.View_CustomerMaster WHERE MarketId='" + marketId + "')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", queryStr);
        }
        public void PaymentTypeLoad(DropDownList aDropDownList)
        {
            string query = @"select * from tblPaymentType";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "PaymentTypeName", "PaymentTypeId", query, "SSIDB");
        }

        public void LoadMarket(DropDownList ddl, string comunitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT DISTINCT CM.MarketId,CM.MarketCode,CM.MarketName FROM dbo.tblCompanyUnit Cu
                inner JOIN dbo.View_CustomerMaster CM ON CM.ComUnitCode = Cu.ComUnitCode
                inner JOIN dbo.tblMarket ON tblMarket.MarketCode = CM.MarketCode
                WHERE CU.ComUnitId='" + comunitId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }

        public void LoadCustomer(DropDownList ddl, string batchId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT DISTINCT tblCustMaster.CustomerMasterId,CustomerName FROM dbo.tblInvoice
INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
WHERE BatchId='"+batchId+"' AND DeliveryTpGrandTotal > 0 AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", queryStr);
        }
        public DataTable LoadInvoiceNew(string batchId, string customerId)
        {
//            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
//            INNER JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
//            WHERE  DeliveryTpGrandTotal > 0 AND BatchId='" + batchId + "' AND dbo.tblInvoice.CustomerMasterId='" + customerId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL ";
            
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due,
                             tblCompanyInfo.CompanyId,tblCustMaster.CustomerMasterId FROM dbo.tblInvoice           
                             LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
                             WHERE  DeliveryTpGrandTotal > 0 AND BatchId='" + batchId
                             + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL AND InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (DeliveryTpGrandTotal-isnull(PaymentAmount,0)) > 0";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

        }
        public DataTable LoadInvoice(string comUnitId,string customerId,string marketId)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice
            
            inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
            WHERE  DeliveryTpGrandTotal > 0 AND MarketId='" + marketId + "' AND dbo.tblInvoice.ComUnitId='" + comUnitId + "' AND dbo.tblInvoice.CustomerMasterId='" + customerId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadInvoice(string comUnitId, string marketId)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice
            
            inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
            WHERE  DeliveryTpGrandTotal > 0 AND MarketId='" + marketId + "' AND dbo.tblInvoice.ComUnitId='" + comUnitId + "'  AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND (DelivaryInvoiceNo IS NOT NULL) AND (DeliveryInvoiceStatus ='Partial' or DeliveryInvoiceStatus ='Full')";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetPrevAmount(string invoiceId)
        {
            string query = @"SELECT PaymentAmount, 
         FROM dbo.tblInvoice WHERE InvoiceId='" + invoiceId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetCustomerInfo(string custId)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster WHERE CustomerMasterId='"+custId+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public CustomerMaster CustomerLoad(string CustomerCode)
        {
            string query = @"SELECT CustomerMasterId FROM dbo.tblCustMaster WHERE CustomerCode= '" + CustomerCode + "'";
                             
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CustomerMaster aCustomerMaster = new CustomerMaster();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCustomerMaster.CustomerMasterId = Int32.Parse(dataReader["CustomerMasterId"].ToString());
                }
            }
            return aCustomerMaster;
        }
        public CustomerMaster DetailCustomerLoad(string CustomerCode)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster WHERE CustomerCode= '" + CustomerCode + "'";

            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CustomerMaster aCustomerMaster = new CustomerMaster();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCustomerMaster.CustomerMasterId = Int32.Parse(dataReader["CustomerMasterId"].ToString());
                    aCustomerMaster.CustomerCode = (dataReader["CustomerCode"].ToString());
                    aCustomerMaster.CustomerName = (dataReader["CustomerName"].ToString());
                }
            }
            return aCustomerMaster;
        }
        public Product DetailProductLoad(string ProductCode)
        {
            string query = @"SELECT * FROM dbo.tblProduct WHERE ProductCode= '" + ProductCode + "'";

            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            Product aProduct = new Product();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aProduct.ProductId = Int32.Parse(dataReader["ProductId"].ToString());
                    aProduct.ProductCode = (dataReader["ProductCode"].ToString());
                    aProduct.ProductName = (dataReader["ProductName"].ToString());
                }
            }
            return aProduct;
        }


        public void GetTerritoryOnDropdownList(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT distinct TTR.TerritoryId, TTR.TerritoryCode + ' : ' + TTR.TerritoryName AS TerritoryName FROM tblTerritory AS TTR 
                                WHERE TTR.IsActive ='True' AND TTR.AreaId = '" + areaId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
        }

        public void GetMarketNameByTerritoryId(DropDownList ddl, string terrId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT MKT.MarketId ,MKT.MarketCode + ' : ' + MKT.MarketName AS MarketName FROM dbo.tblMarket AS MKT WHERE MKT.IsActive ='True' AND TerritoryId = '" + terrId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }

        public DataTable CustomerPaymentDueNew(string parameter)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
            LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            WHERE  DeliveryTpGrandTotal > 0 AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL " + parameter;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");


        }

        public void GetCustomerInfoLoad(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT CSTMR.CustomerMasterId,CAST(CSTMR.CustomerCode AS NVARCHAR(50)) + ':' + CSTMR.CustomerName + ' ( Region: ' + ISNULL(RGN.RegionName, '')   + '; Area: ' +  ISNULL(AR.AreaName, '')  + '; Territory:' + ISNULL(TTR.TerritoryName, '')  + '; Market: ' + ISNULL(MKT.MarketName, '')  + ' )' AS CustomerName FROM tblCustMaster AS CSTMR 
                                    INNER JOIN tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
                                    INNER JOIN tblTerritory AS TTR On MKT.TerritoryId = TTR.TerritoryId
                                    INNER JOIN tblArea AS AR On TTR.AreaId = AR.AreaId
                                    INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", queryStr);
        }

        public DataTable GetInvoiceByCustomer(string custmerId,string dcId)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
            LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
            LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
            WHERE  DeliveryTpGrandTotal > 0 AND tblCustMaster.CustomerMasterId='" + custmerId
            + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL AND InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (DeliveryTpGrandTotal-isnull(PaymentAmount,0)) > 0 ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GetInvoiceByCustomer(string custmerId, string userId, string dcId)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
            LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
            LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
            WHERE  DeliveryTpGrandTotal > 0 AND tblCustMaster.CustomerMasterId='" + custmerId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL AND tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId
            + " ) AND InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (DeliveryTpGrandTotal-isnull(PaymentAmount,0)) > 0 ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            string query = @"SELECT INV.CustomerMasterId FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
                             WHERE CSTMR.CustomerCode ='" + customerCode + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCreditBalanceInfo(string customerId)
        {
            string query = @"SELECT CDJ.CustomerMasterId,SUM(CDJ.Amount) AS Balance FROM tblCreditAdjustment AS CDJ
                             WHERE CDJ.CustomerMasterId = '" + customerId + "' GROUP BY CDJ.CustomerMasterId";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadInvoiceNewByUser(string batchId, string customerId, string userId)
        {
            //            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
            //            INNER JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            //            WHERE  DeliveryTpGrandTotal > 0 AND BatchId='" + batchId + "' AND dbo.tblInvoice.CustomerMasterId='" + customerId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL ";

            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due,
                             tblCompanyInfo.CompanyId,tblCustMaster.CustomerMasterId FROM dbo.tblInvoice           
                             LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
                             WHERE  DeliveryTpGrandTotal > 0 AND BatchId='" + batchId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL AND tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId
                             + ") AND InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (DeliveryTpGrandTotal-isnull(PaymentAmount,0)) > 0"; 

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadBankAccount(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CB.BankAccountId,BankName + ' (' + AccountName + ' - ' + AccountNo + ')' AS BankAcount FROM ZAS_ACCDB..tblBankAccount AS CB
                             LEFT JOIN ZAS_ACCDB..tblBankInformation AS B ON b.BankId = CB.BankId
                             LEFT JOIN ZAS_ACCDB..tblMultipleCompanySameBankAccount AS CABC ON CABC.BankAccountId = CB.BankAccountId
                             WHERE CB.BankAccountId IN (
                             SELECT BankAccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE BankAccountId IS NOT NULL )
                             AND CABC.CompanyId = " + companyId;

            aCommonInternalDal.LoadDropDownValue(ddl, "BankAcount", "BankAccountId", query, "SSIDB");
        }


        public void LoadCashAccount(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CA.CashAccountId,AccountName FROM ZAS_ACCDB..tblCashAccount AS CA
                             LEFT JOIN ZAS_ACCDB..tblMultipleCompanySameCashAccount CASC ON CA.CashAccountId = CASC.CashAccountId
                             WHERE CA.CashAccountId IN (
                             SELECT CashAccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE CashAccountId IS NOT NULL )
                             AND CASC.CompanyId = " + companyId;

            aCommonInternalDal.LoadDropDownValue(ddl, "AccountName", "CashAccountId", query, "SSIDB");
        }

        public bool PaymentJournalPosting(int custPayId, int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CustPayId", custPayId));
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            return aCommonInternalDal.UpdateAction("sp_CustomerPaymentPosting", aSqlParameterList);
        }

        //private bool DeletePaymentMasterInfo(CustPayment aCustPayment)
        //{
        //    string query = @"DELETE FROM tblCustomerPay WHERE CustPayId =  " + aCustPayment.CustPayId;
        //    return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        //}
        public bool DeletePaymentMasterInfo(CustPayment aCustPayment)
        {
            string query = @"DELETE FROM tblCustomerPay WHERE CustPayId =  " + aCustPayment.CustPayId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
