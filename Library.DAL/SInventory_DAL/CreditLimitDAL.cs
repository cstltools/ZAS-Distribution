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
    public class CreditLimitDAL
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveCreditLimit(CreditLimitDAO aCreditLimitDao)
        {

            string insertQuery = @"INSERT INTO dbo.tblCustomerCreditLimit
                                    ( CompanyId ,
                                      CustomerMasterId ,
                                      LimitAmount ,
                                      DayLimit ,
                                      IsActive ,
                                      EntryBy ,
                                      EntryDate ,ActionStatus
                                    )
                            VALUES  ( '"+aCreditLimitDao.CompanyId+"' ,'"+aCreditLimitDao.CustomerMasterId+"' ,'"+aCreditLimitDao.LimitAmount+"', " +
                                 "   '"+aCreditLimitDao.DayLimit+"' , '"+aCreditLimitDao.IsActive+"' ,'"+aCreditLimitDao.EntryBy+"' ,'"+aCreditLimitDao.EntryDate+"','Posted')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool ApprovalUpdateDAL(CreditLimitDAO aCreditLimitDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CreditLimitId", aCreditLimitDao.CreditLimitId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aCreditLimitDao.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aCreditLimitDao.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aCreditLimitDao.ApprovedDate));

            string query = @"UPDATE tblCustomerCreditLimit SET ActionStatus=@ActionStatus,ApprovedBy=@ApprovedBy,ApprovedDate=@ApprovedDate WHERE CreditLimitId=@CreditLimitId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        } 
        public DataTable LoadCustomerCreditLimitApproval(string actionstatus)
        {
            const string query = @"SELECT * FROM dbo.tblCustomerCreditLimit
            INNER JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCustomerCreditLimit.CompanyId
            INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
            WHERE ActionStatus='Posted'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
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
        //public void GetCustomerDropdownList(ComboBox ddl)
        //{
        //    const string queryStr = @"SELECT * FROM dbo.tblCustMaster";
        //    aCommonInternalDal.LoadComboboxValue(ddl, "CustomerName", "CustomerMasterId", queryStr, "SSIDB");
        //}
    }
}
