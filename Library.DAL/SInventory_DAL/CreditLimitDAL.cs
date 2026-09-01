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
                                      EntryDate,
                                      ActionStatus
                                    )
                            VALUES  ( '" + aCreditLimitDao.CompanyId+"' ,'"+aCreditLimitDao.CustomerMasterId+"' ,'"+aCreditLimitDao.LimitAmount+"', " +
                                 "   '" + aCreditLimitDao.DayLimit + "' , '" + aCreditLimitDao.IsActive + "' ,'" + aCreditLimitDao.EntryBy + "' ,'" + aCreditLimitDao.EntryDate 
                                 + "','Posted')";

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
        
        public DataTable GetCustomerCreditInfo(string generateParam)
        {
            string query = @"SELECT tblCustomerCreditLimit.ApprovedBy,tblCustomerCreditLimit.ApprovedDate,CreditLimitId,CompanyName,CustomerCode,CustomerName,LimitAmount,StartDate,EndDate,ApprovedBy,tblCustomerCreditLimit.ApprovedDate,
            ActionStatus,InvoiceDate, ISNULL(DayLimit,0) AS Validity,ISNULL(DATEDIFF(day, InvoiceDate,GETDATE()),0) AS Ds,InvoiceNo, CASE WHEN tblCustomerCreditLimit.IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS ActiveStatus,
            (ISNULL(DayLimit,0) - ISNULL(DATEDIFF(day, InvoiceDate,GETDATE()),0)) AS DaysRemaining FROM dbo.tblCustomerCreditLimit WITH(NOLOCK)
            INNER JOIN dbo.tblCompanyInfo WITH(NOLOCK) ON tblCompanyInfo.CompanyId = tblCustomerCreditLimit.CompanyId
            INNER JOIN dbo.tblCustMaster WITH(NOLOCK) ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
            LEFT JOIN (SELECT CustomerMasterId,CompanyId,MIN(InvoiceNo) AS InvoiceNo,MIN(InvoiceDate) AS InvoiceDate FROM tblInvoice AS INV
            LEFT JOIN tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
            LEFT JOIN (SELECT M.CommonPK_ID,SUM(DebitAmountBDT) AS ReturnAmount FROM CSTL_ACCDB..tblDebitCreditVoucherMaster AS M
            			LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
            			LEFT JOIN tblInvoice AS INV ON M.CommonPK_ID = INV.InvoiceId
            			WHERE ISNULL(M.IsInventoryValuation,0) = 0 AND M.TableID = 2018 GROUP BY M.CommonPK_ID) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
            WHERE ISNULL(IsBadDue,0) = 0 AND ISNULL(DeliveryTpGrandTotal,0) > 0 
            AND ISNULL(DeliveryTpGrandTotal,0) - (ISNULL(PaymentAmount,0) + ISNULL(AIT,0) + ISNULL(DiscountOnPayment,0))- ISNULL(ReturnAmount,0) > 1
            GROUP BY CustomerMasterId,CompanyId) AS DD ON tblCustomerCreditLimit.CustomerMasterId = DD.CustomerMasterId AND tblCustomerCreditLimit.CompanyId = DD.CompanyId
            WHERE CreditLimitId IS NOT NULL " + generateParam + " ORDER BY CompanyName,CreditLimitId,CustomerCode";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerCreditInfoById(string creditId)
        {
            string query = @"SELECT ISNULL(DayLimit,0) AS DayLimit,* FROM dbo.tblCustomerCreditLimit
                             INNER JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCustomerCreditLimit.CompanyId
                             INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
                             WHERE CreditLimitId = " + creditId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateCreditLimit(CreditLimitDAO aCreditLimitDao)
        {
            string query = @"UPDATE tblCustomerCreditLimit SET CompanyId='" + aCreditLimitDao.CompanyId + "',CustomerMasterId='" + aCreditLimitDao.CustomerMasterId
                + "',LimitAmount = '" + aCreditLimitDao.LimitAmount + "',DayLimit='" + aCreditLimitDao.DayLimit + "',IsActive='" + aCreditLimitDao.IsActive 
                + "',UpdateBy = '" + aCreditLimitDao.UpdateBy + "',UpdateDate = '" + aCreditLimitDao.UpdateDate + "',ActionStatus = 'Posted' WHERE CreditLimitId = " + aCreditLimitDao.CreditLimitId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable CheckExistorNot(int companyId, int customerid)
        {
            string query = @"SELECT ISNULL(DayLimit,0) AS DayLimit,* FROM dbo.tblCustomerCreditLimit
                             INNER JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCustomerCreditLimit.CompanyId
                             INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
                             WHERE tblCustomerCreditLimit.IsActive = 1 AND  tblCustomerCreditLimit.CompanyId = " + companyId + "AND tblCustomerCreditLimit.CustomerMasterId = " + customerid;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CheckExistorNotUpdate(int companyId, int customerid, int creditId)
        {
            string query = @"SELECT ISNULL(DayLimit,0) AS DayLimit,* FROM dbo.tblCustomerCreditLimit
                             INNER JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCustomerCreditLimit.CompanyId
                             INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
                             WHERE tblCustomerCreditLimit.IsActive = 1 AND tblCustomerCreditLimit.CompanyId = " + companyId + "AND tblCustomerCreditLimit.CustomerMasterId = " + customerid + " AND CreditLimitId != " + creditId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
