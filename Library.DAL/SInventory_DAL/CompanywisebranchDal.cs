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
    public class CompanywisebranchDal
    {
        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public void LoadCompany(DropDownList ddl)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo";
            aCommonInternalDAL.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        public void LoadBank(DropDownList ddl)
        {
            string query = @"SELECT BankId,BankName FROM tblBankInfo";
            aCommonInternalDAL.LoadDropDownValue(ddl, "BankName", "BankId", query, "SSIDB");
        }

        public void LoadBranch(DropDownList ddl, string bankId)
        {
            string query = @"SELECT BranchId,BranchName FROM tblBranchInfo WHERE BankId = " + bankId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "BranchName", "BranchId", query, "SSIDB");
        }

        public Int32 SaveDepositInfo(CompanyWiseDepositDao aDepositDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@CompanyId", aDepositDao.CompanyId));
            aSqlParameterlist.Add(new SqlParameter("@BranchId", aDepositDao.BranchId));
            aSqlParameterlist.Add(new SqlParameter("@Amount", aDepositDao.Amount));
            aSqlParameterlist.Add(new SqlParameter("@EntryBy", aDepositDao.EntryBy));
            aSqlParameterlist.Add(new SqlParameter("@EntryDate", aDepositDao.EntryDate));
            aSqlParameterlist.Add(new SqlParameter("@DepositDate", aDepositDao.DepositDate));
            aSqlParameterlist.Add(new SqlParameter("@IsDelete", aDepositDao.IsDelete));

            string insertQuery = @"INSERT INTO tblCompanyWiseDeposit(CompanyId,BranchId,Amount,EntryBy,EntryDate,DepositDate,IsDelete) VALUES (@CompanyId,@BranchId,@Amount,@EntryBy,@EntryDate,@DepositDate,@IsDelete)";
            return aCommonInternalDAL.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterlist, "SSIDB");
        }

        public DataTable LoadDepositList(string parameter)
        {
             string queryStr = @"SELECT DP.DepositId,CI.CompanyName,BNK.BankName,BN.BranchName,DP.DepositDate,DP.Amount FROM tblCompanyWiseDeposit AS DP
                                      INNER JOIN tblCompanyInfo  AS CI ON DP.CompanyId = CI.CompanyId
                                      INNER JOIN tblBranchInfo AS BN ON DP.BranchId = BN.BranchId
                                      INNER JOIN tblBankInfo AS BNK ON BN.BankId = BNK.BankId WHERE DP.IsDelete = 0 " + parameter;
            return aCommonInternalDAL.DataContainerDataTable(queryStr, "SSIDB");
        }

        public bool UpdateDepositInfo(CompanyWiseDepositDao aDepositDao)
        {
            //List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            //aSqlParameterlist.Add(new SqlParameter("@DepositId", aDepositDao.DepositId));
            //aSqlParameterlist.Add(new SqlParameter("@IsDelete", aDepositDao.IsDelete));
            //aSqlParameterlist.Add(new SqlParameter("@DeleteBy", aDepositDao.DeleteBy));
            //aSqlParameterlist.Add(new SqlParameter("@DeleteDate", aDepositDao.DeleteDate));

            string queryStr = @"UPDATE tblCompanyWiseDeposit SET IsDelete = '" + aDepositDao.IsDelete + "',DeleteBy = '" + aDepositDao.DeleteBy + "',DeleteDate = '" + aDepositDao.DeleteDate + "' WHERE DepositId = " + aDepositDao.DepositId ;
            return aCommonInternalDAL.UpdateDataByUpdateCommand(queryStr ,"SSIDB");
        }
    }
}
