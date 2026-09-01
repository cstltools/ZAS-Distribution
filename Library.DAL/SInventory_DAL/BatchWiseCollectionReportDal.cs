using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class BatchWiseCollectionReportDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void LoadDeliveryMan(DropDownList aDropDownList)
        {
            string query = @"SELECT *,EmpName as DeliveryManName FROM dbo.tblDeliveryManInfo
            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "DeliveryManName", "DeliveryManId", query, "SSIDB");
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            string query = @"SELECT CSTMR.CustomerName,INV.CustomerMasterId FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
                             WHERE CSTMR.CustomerCode ='" + customerCode + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable BatchWiseCollection(int batchId, DateTime batchCreationDate)
        {
            string query = @"SELECT 'M-' + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 120),'-','') AS MRNo,INV.InvoiceNo,INV.InvoiceDate,BCH.BatchNo,BCH.Date AS BatchCreationDate,CP.PayType,
                            SUM(CPD.PaymentAmount) AS TotalPaid,(INV.DeliveryTpGrandTotal - SUM(CPD.PaymentAmount)) AS Due ,CSTMR.CustomerName
                            FROM dbo.tblCustPayDetail AS CPD 
                            INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                            INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = CPD.CustPayId
                            INNER JOIN dbo.tblInvoiceBatch AS BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            WHERE INV.BatchId = " + batchId + " AND INV.PaymentStatus != 'Full' GROUP BY INV.InvoiceNo,INV.DeliveryTpGrandTotal,BCH.BatchNo,BCH.Date,INV.InvoiceDate,CP.PayType,CSTMR.CustomerName";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable BatchWiseCollection(int batchId)
        {
            string query = @"SELECT  CPD.CustPayDetailId,INV.InvoiceNo,INV.InvoiceDate,BCH.BatchNo,BCH.Date AS BatchCreationDate,CP.PayType,CP.PaymentDate,
                            CPD.PaymentAmount AS PaidAmount ,CSTMR.CustomerName
                            FROM dbo.tblCustPayDetail AS CPD 
                            INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                            INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = CPD.CustPayId
                            INNER JOIN dbo.tblInvoiceBatch AS BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            WHERE INV.BatchId = " + batchId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable BatchWiseCollectionById(string payId)
        {
            string query = @"SELECT 'M-' + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 120),'-','') AS MRNo,INV.InvoiceNo,INV.InvoiceDate,BCH.BatchNo,BCH.Date AS BatchCreationDate,CP.PayType,CP.PaymentDate,
                            CPD.PaymentAmount AS PaidAmount ,CSTMR.CustomerName,PaymentRemarks,EGM.EmpName
                            FROM dbo.tblCustPayDetail AS CPD 
                            INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                            INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = CPD.CustPayId
                            LEFT JOIN dbo.tblInvoiceBatch AS BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            LEFT JOIN tblDeliveryManInfo AS DM ON INV.DeliveryManId = DM.DeliveryManId
							LEFT JOIN tblEmpGeneralInfo AS EGM ON DM.EmpInfoId = EGM.EmpInfoId
                            WHERE CPD.CustPayDetailId = " + payId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        } 
        
        
        public DataTable MoneyReceiptById(string moneyReceitId)
        {
            string query = @"SELECT 
                               
                                MR.MoneyReceiptId,
								CSTMR.CustomerName + '(' + CSTMR.CustomerCode + ')' AS CustomerName,
								UNT.ComUnitName CompanyName,
                                MR.MoneyReceiptNo AS MRNo,
                                MR.MonyReceiptDate,
                                MR.CreateDate AS MoneyReceiptCreateDate,
                                INV.InvoiceNo,
                                INV.InvoiceDate,
								CONVERT(NVARCHAR(10), BCH.Date, 120) + ' (' +  CONVERT(NVARCHAR(10),BCH.BatchNo) + ')' AS BatchNo,
                                BCH.Date AS BatchCreationDate,
                                CP.PayType,
                                CP.PaymentDate,
                                CPD.PaymentAmount AS PaidAmount,
								CPD.AIT,
								CPD.Discount,
								Inv.PaymentStatus,
                                CP.PaymentRemarks,
                                CSTMR.CustomerName,
                                EGM.EmpName,
                                ISNULL(UEGM.EmpName,USR.UserName) AS CreateBy,
								FORMAT(MR.CreateDate, 'dd-MMM-yyyy hh:mm:ss tt') CreateDate
                            FROM tblMoneyReceipt MR
                            INNER JOIN tblCustPayDetail CPD ON ',' + MR.CustPayDetailsId + ',' LIKE '%,' + CAST(CPD.CustPayDetailId AS VARCHAR) + ',%'
                            INNER JOIN tblCustomerPay CP ON CP.CustPayId = CPD.CustPayId
                            INNER JOIN tblInvoice INV ON INV.InvoiceId = CPD.InvoiceId
                            LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                            LEFT JOIN tblInvoiceBatch BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN tblCustMaster CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            LEFT JOIN tblDeliveryManInfo DM ON INV.DeliveryManId = DM.DeliveryManId
                            LEFT JOIN tblEmpGeneralInfo EGM ON DM.EmpInfoId = EGM.EmpInfoId
                            LEFT JOIN tblUser AS USR ON CP.CreateBy = USR.LoginName
							LEFT JOIN tblEmpGeneralInfo UEGM ON USR.EmpInfoId = UEGM.EmpInfoId
                            WHERE MoneyReceiptId IS NOT NULL AND MoneyReceiptId = " + moneyReceitId + "  ORDER BY MR.MoneyReceiptId, CPD.CustPayDetailId";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable BatchWiseCollectionSheet(string pram)
        {
            string query = @"SELECT  tblCustPayDetail.CustPayDetailId,INV.InvoiceId,INV.InvoiceNo,INV.InvoiceDate,BCH.BatchNo,BCH.Date AS BatchCreationDate,CP.PayType,CP.PaymentDate,
                            tblCustPayDetail.PaymentAmount AS PaidAmount,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpName AS DeliveryMan
                            FROM dbo.tblCustPayDetail 
                            INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = tblCustPayDetail.InvoiceId
                            INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = tblCustPayDetail.CustPayId
                            LEFT JOIN dbo.tblInvoiceBatch AS BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
							LEFT JOIN dbo.tblDeliveryManInfo AS DEL ON DEL.DeliveryManId = INV.DeliveryManId
							LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DEL.EmpInfoId
                            WHERE CustPayDetailId IS NOT NULL " + pram;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable BatchWiseCollectionByBatchId(string batchId)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpName AS MIO,INV.InvoiceNo,INV.InvoiceDate,INVB.BatchNo,INVB.Date,ISNULL(INV.DeliveryTpGrandTotal,0) AS DeliveryTpGrandTotal,ISNULL(INV.PaymentAmount,0) AS PaymentAmount,
ISNULL(INV.PaymentStatus,'No Payment') AS PaymentStatus FROM tblInvoice AS INV 
                             LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId
                             LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblInvoiceBatch AS INVB ON INV.BatchId = INVB.BatchId
                             LEFT JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             WHERE INV.BatchId = " + batchId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable BatchWiseCollectionSingle(string customerCode)
        {
            string query = @"SELECT  CPD.CustPayDetailId,INV.InvoiceNo,INV.InvoiceDate,BCH.BatchNo,BCH.Date AS BatchCreationDate,CP.PayType,CP.PaymentDate,
                            CPD.PaymentAmount AS PaidAmount ,CSTMR.CustomerName
                            FROM dbo.tblCustPayDetail AS CPD 
                            INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                            INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = CPD.CustPayId
                            INNER JOIN dbo.tblInvoiceBatch AS BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            WHERE CSTMR.CustomerCode = " + customerCode;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable BatchWiseCollectionSheet(int batchId)
        {
            string query = @"SELECT CPD.CustPayDetailId,INV.InvoiceId,INV.InvoiceNo,INV.InvoiceDate,BCH.BatchNo,BCH.Date AS BatchCreationDate,CP.PayType,CP.PaymentDate,
                            CPD.PaymentAmount AS PaidAmount ,CSTMR.CustomerName
                            FROM dbo.tblCustPayDetail AS CPD 
                            INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                            INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = CPD.CustPayId
                            INNER JOIN dbo.tblInvoiceBatch AS BCH ON BCH.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            WHERE IsPrint IN ('False',NULL) AND INV.BatchId = " + batchId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateIsPrint(string pram)
        {
            string query = @"UPDATE dbo.tblCustPayDetail SET IsPrint = 'True' WHERE CustPayDetailId IS NOT NULL " + pram;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable LoadDueInvoice(string parameter)
        {
            string query = @"SELECT CustPayDetailId,InvoiceNo,tblInvoice.InvoiceId,InvoiceDate,CP.PaymentDate,CP.PaymentAmount AS PaidAmount, '' AS BatchNo, '' AS BatchCreationDate, 
							CustomerName,CustomerCode
							FROM dbo.tblCustPayDetail 
							INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = tblCustPayDetail.CustPayId
							INNER JOIN dbo.tblInvoice  ON tblInvoice.InvoiceId = tblCustPayDetail.InvoiceId
							INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
							WHERE Remarks = 'Due' AND DeliveryTpGrandTotal > 0  AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL 
                            " + parameter;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadNonTranscationalInvoice(string parameter)
        {
            string query = @"SELECT INVM.InvReturnMasterID,CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceNo,INVM.ReturnCode,INVM.ReturnDate,INVM.TotalReturn,INVM.TotalValue
                             FROM dbo.tblNonTranscationalInvoiceMaster AS INVM 
                             LEFT JOIN tblInvoice AS INV ON INV.InvoiceId = INVM.InvoiceId
                             INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INVM.ComUnitId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INVM.CustomerMasterId
                             WHERE INVM.ActionStatus IN ('Accepted') " + parameter;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable NTInvoiceReturnReport(string parameter)
        {
            string query = @"SELECT INVM.InvReturnMasterID,CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceNo,INVM.ReturnCode,INVM.ReturnDate,
                             PD.ProductCode,PD.ProductName,INVD.Qty,INVD.Price,INVD.Batch,INVD.ExpDate,INVD.MfgDate,INVD.VAT,INVD.TotalAmount FROM dbo.tblNonTranscationalInvoiceMaster AS INVM 
                             INNER JOIN dbo.tblNonTranscationalInvoiceDetail AS INVD ON INVD.InvReturnMasterID = INVM.InvReturnMasterID
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INVM.CustomerMasterId
                             LEFT JOIN tblInvoice AS INV ON INV.InvoiceId = INVM.InvoiceId
                             INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INVM.ComUnitId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
                             INNER JOIN dbo.tblProduct AS PD ON PD.ProductId = INVD.ProductId
                             INNER JOIN dbo.tblPackSize AS PKS ON PKS.PackSizeId = PD.PackSizeId WHERE INVM.InvReturnMasterID = " + parameter;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable AitInformationById(string payId)
        {
            string query = @"SELECT 'AIT-' + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 120),'-','') AS MRNo,CompanyName,UNT.ComUnitName,CustPayDetailId,INV.InvoiceNo,INV.InvoiceDate,CP.PayType,CP.PaymentDate,
                             CPD.AIT AS PaidAmount,CSTMR.CustomerCode ,CSTMR.CustomerName,PaymentRemarks
                             FROM dbo.tblCustPayDetail AS CPD 
                             INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                             LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
                             INNER JOIN dbo.tblCustomerPay AS CP ON CP.CustPayId = CPD.CustPayId
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             WHERE CPD.CustPayDetailId = " + payId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetMoneyReceiptWisePayment(string moneyReceiptId)
        {
            string query = @"SELECT 
    MR.MoneyReceiptId,
    MR.MoneyReceiptNo AS MRNo,
    MR.MonyReceiptDate,
    MR.CreateDate AS MoneyReceiptCreateDate,
    INV.InvoiceNo,
    INV.InvoiceDate,
    BCH.BatchNo,
    BCH.Date AS BatchCreationDate,
    CP.PayType,
    CP.PaymentDate,
    CPD.PaymentAmount AS PaidAmount,
    CPD.PaymentRemarks,
    CSTMR.CustomerName,
    EGM.EmpName,
	CPD.InvoiceId,CPD.CustPayDetailId, ISNULL(CPD.AIT,0) AIT, ISNULL(CPD.Discount,0) Discount,CP.CustPayId
FROM tblMoneyReceipt MR
INNER JOIN tblCustPayDetail CPD ON ',' + MR.CustPayDetailsId + ',' LIKE '%,' + CAST(CPD.CustPayDetailId AS VARCHAR) + ',%'
INNER JOIN tblCustomerPay CP ON CP.CustPayId = CPD.CustPayId
INNER JOIN tblInvoice INV ON INV.InvoiceId = CPD.InvoiceId
LEFT JOIN tblInvoiceBatch BCH ON BCH.BatchId = INV.BatchId
INNER JOIN tblCustMaster CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
LEFT JOIN tblDeliveryManInfo DM ON INV.DeliveryManId = DM.DeliveryManId
LEFT JOIN tblEmpGeneralInfo EGM ON DM.EmpInfoId = EGM.EmpInfoId
WHERE MoneyReceiptId IS NOT NULL AND MoneyReceiptId = " + moneyReceiptId + "ORDER BY MR.MoneyReceiptId, CPD.CustPayDetailId";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public bool PaymentJournalPosting(int custPayId, int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CustPayId", custPayId));
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            return aCommonInternalDal.UpdateAction("sp_Sales_CustomerPaymentReversePosting", aSqlParameterList);
            //return aCommonInternalDal.UpdateAction("sp_CustomerPaymentPosting", aSqlParameterList);
        }

        public void LoadMoneyReceipt(DropDownList ddl, DateTime fromDate, DateTime toDate, string userId)
        {
            string query = @"
SELECT DISTINCT
    MR.MoneyReceiptId,
    MR.MoneyReceiptNo
FROM tblMoneyReceipt MR
CROSS APPLY dbo.SplitString(MR.CustPayDetailsId, ',') AS Split
INNER JOIN tblCustPayDetail CPD 
    ON Split.Value = CPD.CustPayDetailId
INNER JOIN tblInvoice INV 
    ON CPD.InvoiceId = INV.InvoiceId
WHERE 
    MR.MonyReceiptDate >= '" + fromDate + @"' 
    AND MR.MonyReceiptDate < DATEADD(DAY, 1, '" + toDate + @"')
    AND ComUnitId IN (
        SELECT CompanyUnitId 
        FROM dbo.tblUserCompanyUnit 
        WHERE UserId = " + userId + @"
    )
    AND MR.MoneyReceiptNo IS NOT NULL;";

            aCommonInternalDal.LoadDropDownValue(ddl, "MoneyReceiptNo", "MoneyReceiptId", query, "SSIDB");

        }
    }
}
