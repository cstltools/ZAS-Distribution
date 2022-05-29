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
    public class DepositSlipEntryDal
    {

        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void LoadManufac(DropDownList ddl)
        {
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void GetDcInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void GetDCInformation(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void LoadBank(DropDownList ddl)
        {
            string query = @"SELECT BankId,BankName FROM tblBankInfo ORDER BY BankName";
            aCommonInternalDal.LoadDropDownValue(ddl, "BankName", "BankId", query, "SSIDB");
        }

        public Int32 SaveDepositInfo(CompanyWiseDepositDao aDepositDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@DepotId", aDepositDao.DepotId));
            aSqlParameterlist.Add(new SqlParameter("@Amount", aDepositDao.Amount));
            aSqlParameterlist.Add(new SqlParameter("@EntryBy", aDepositDao.EntryBy));
            aSqlParameterlist.Add(new SqlParameter("@EntryDate", aDepositDao.EntryDate));
            aSqlParameterlist.Add(new SqlParameter("@DepositDate", aDepositDao.DepositDate));
            aSqlParameterlist.Add(new SqlParameter("@IsDelete", aDepositDao.IsDelete));
            aSqlParameterlist.Add(new SqlParameter("@Remarks", aDepositDao.Remarks));
            aSqlParameterlist.Add(new SqlParameter("@DepositType", aDepositDao.DepositType));
            aSqlParameterlist.Add(new SqlParameter("@BankId", aDepositDao.BankId));
            aSqlParameterlist.Add(new SqlParameter("@CheckNumber", aDepositDao.CheckNumber ?? (object)DBNull.Value));
            aSqlParameterlist.Add(new SqlParameter("@CheckDate", aDepositDao.CheckDate ?? (object)DBNull.Value));
            aSqlParameterlist.Add(new SqlParameter("@BranchName", aDepositDao.BranchName ?? (object)DBNull.Value));
            aSqlParameterlist.Add(new SqlParameter("@AccountName", aDepositDao.AccountName ?? (object)DBNull.Value));


            return aCommonInternalDal.SaveAction("sp_I_CompanyWiseDeposit", aSqlParameterlist, "@DepositId");
        }


        public DataTable LoadDepositList(string parameter)
        {
            string queryStr = @"SELECT DP.DepositId,CI.CompanyName,UNT.ComUnitName,DP.DepositDate,CPT.Collection,DP.Amount,(CPT.Collection - DP.Amount) DueAmount,BNK.BankName,DP.BranchName, AccountName
,CheckNumber,CheckDate,DP.Remarks,DP.DepositType FROM tblCompanyWiseDeposit AS DP
LEFT JOIN tblCompanyUnit AS UNT ON DP.DepotId = UNT.ComUnitId
LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
LEFT JOIN tblBankInfo AS BNK ON DP.BankId = BNK.BankId 
LEFT JOIN (SELECT INV.ComUnitId,SUM(CPD.PaymentAmount) Collection FROM tblCustPayDetail AS CPD
LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId
LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId
WHERE CP.PaymentDate BETWEEN '31-Jan-2022' AND '02-Feb-2022' GROUP BY INV.ComUnitId) AS CPT ON UNT.ComUnitId = CPT.ComUnitId
WHERE DP.IsDelete = 0  " + parameter + " ORDER BY DP.DepositDate";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public bool UpdateDepositInfo(CompanyWiseDepositDao aDepositDao)
        {
            string queryStr = @"UPDATE tblCompanyWiseDeposit SET IsDelete = '" + aDepositDao.IsDelete + "',DeleteBy = '" + aDepositDao.DeleteBy + "',DeleteDate = '" + aDepositDao.DeleteDate + "' WHERE DepositId = " + aDepositDao.DepositId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(queryStr, "SSIDB");
        }

        public DataTable LoadDepositByPram(string parameter, string fromDate, string toDate)
        {
            string queryStr = @"SELECT DP.DepositId,CI.CompanyName,UNT.ComUnitName,DP.DepositDate,CPT.Collection,DP.Amount,(CPT.Collection - DP.Amount) DueAmount,BNK.BankName,DP.BranchName, AccountName
            ,CheckNumber,CheckDate,DP.Remarks, '" + fromDate + "' AS FromDate,'" + toDate + "' AS ToDate,DP.DepositType FROM tblCompanyWiseDeposit AS DP LEFT JOIN tblCompanyUnit AS UNT ON DP.DepotId = UNT.ComUnitId LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId LEFT JOIN tblBankInfo AS BNK ON DP.BankId = BNK.BankId  LEFT JOIN (SELECT INV.ComUnitId,SUM(CPD.PaymentAmount) Collection FROM tblCustPayDetail AS CPD LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId WHERE CP.PaymentDate BETWEEN '" + fromDate + "' AND '" + toDate + "' GROUP BY INV.ComUnitId) AS CPT ON UNT.ComUnitId = CPT.ComUnitId WHERE DP.IsDelete = 0  " + parameter + " ORDER BY DP.DepositDate";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }


        public DataTable LoadDepositReportByPram(string parameter, string fromDate, string toDate)
        {
            //string queryStr = @"SELECT CI.CompanyName,UNT.ComUnitName,CPT.Collection,SUM(DP.Amount) Amount ,(CPT.Collection - SUM(DP.Amount)) DueAmount,'" + fromDate + "' AS FromDate,'" + toDate + "' AS ToDate FROM tblCompanyWiseDeposit AS DP LEFT JOIN tblCompanyUnit AS UNT ON DP.DepotId = UNT.ComUnitId LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId LEFT JOIN tblBankInfo AS BNK ON DP.BankId = BNK.BankId  LEFT JOIN (SELECT INV.ComUnitId,SUM(CPD.PaymentAmount) Collection FROM tblCustPayDetail AS CPD LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId WHERE CP.PaymentDate BETWEEN '" + fromDate + "' AND '" + toDate + "' GROUP BY INV.ComUnitId) AS CPT ON UNT.ComUnitId = CPT.ComUnitId WHERE DP.IsDelete = 0  " + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,CPT.Collection ORDER BY UNT.ComUnitName";
            string queryStr = @"SELECT  CI.CompanyName,UNT.ComUnitName,SUM(CPD.PaymentAmount) Collection,ISNULL(DP.Amount,0) Amount,(SUM(CPD.PaymentAmount)- ISNULL(DP.Amount,0)) AS DueAmount,'" + fromDate + "' AS FromDate,'" + toDate + "' AS ToDate FROM tblCustPayDetail AS CPD LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId LEFT JOIN (SELECT CI.CompanyName,UNT.ComUnitName,SUM(Amount) AS Amount FROM tblCompanyWiseDeposit AS DP LEFT JOIN tblCompanyUnit AS UNT ON DP.DepotId = UNT.ComUnitId LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId WHERE DP.IsDelete = 0 AND DP.DepositDate BETWEEN '" + fromDate + "' AND '" + toDate + "' GROUP BY CI.CompanyName,UNT.ComUnitName) AS DP ON UNT.ComUnitName = DP.ComUnitName WHERE CI.CompanyId IS NOT NULL " + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,DP.Amount";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }
    }
}
