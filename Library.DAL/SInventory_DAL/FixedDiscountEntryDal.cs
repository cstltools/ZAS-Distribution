using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class FixedDiscountEntryDal
    {
        readonly ClsCommonInternalDAL _aInternalDal = new ClsCommonInternalDAL();
        ClsApprovalAction approvalAction = new ClsApprovalAction();
        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster AS CSTMR
			 LEFT JOIN dbo.tblMarket AS MKT ON MKT.MarketId = CSTMR.MarketId
			 LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MKT.TerritoryId
			 LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.TerritoryId = TTR.TerritoryId
			 WHERE CSTMR.MarketId IS NOT NULL AND MIO.MIOId IS NOT NULL AND CSTMR.CustomerCode = '" + customerCode + "'";

            return _aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadGroupInfo(DropDownList groupDropDownList)
        {
            string query = @"SELECT GroupId, GroupName FROM tblGroupInformation";
            _aInternalDal.LoadDropDownValue(groupDropDownList, "GroupName", "GroupId", query, "SSIDB");
        }

        public void LoadCompanyInfo(DropDownList ddl, string groupId)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo WHERE GroupId = " + groupId;
            _aInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        public DataTable CheckDuplicate(string customerId, string companyId)
        {
            string query = @"SELECT * FROM tblCustomerFixedDiscount AS FDC WHERE FDC.CompanyId = " + companyId + " AND CustMasterId = " + customerId + " AND IsActive = 1";

            return _aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveDiscount(FixedDiscountDao aInformationDao)
        {
            string insertQuery = @"INSERT INTO tblCustomerFixedDiscount (CompanyId,CustMasterId,DiscountParcentage,IsActive,ActionStatus,EntryBy,EntryDate) VALUES ('" + aInformationDao.CompanyId + "','" + aInformationDao.CustMasterId
                                       + "','" + aInformationDao.DiscountParcentage + "','" + aInformationDao.IsActive + "','" + aInformationDao.ActionStatus + "','" + aInformationDao.EntryBy + "','" + aInformationDao.EntryDate + "')";

            return _aInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable GetDiscountInfo(string getPrameterList)
        {
            string query = @"SELECT DiscountId,Ci.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,FDC.DiscountParcentage FROM tblCustomerFixedDiscount AS FDC 
                             LEFT JOIN tblCompanyInfo AS CI ON FDC.CompanyId = CI.CompanyId
                             LEFT JOIN tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = FDC.CustMasterId
                             WHERE ActionStatus IN ('Accepted')	AND FDC.IsActive = 1 " + getPrameterList;

            return _aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DeleteDiscountInfo(string discountId, string userName)
        {
            string query = @"UPDATE tblCustomerFixedDiscount SET IsActive = 0,ActionStatus = 'Delete',InactiveDate = '" + DateTime.Now + "',InactiveBy = " + userName + " WHERE DiscountId = " + discountId;
            return _aInternalDal.DeleteDataByCommand(query, "SSIDB");
        }

        public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        {
            approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        }

        public string LoadForApprovalConditionDAL(string pageName, string userName)
        {
            return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        }

        public DataTable GetCustomerFixedDiscount(string actionStatus,string userId)
        {
            string query = @"SELECT DiscountId,Ci.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,FDC.DiscountParcentage FROM tblCustomerFixedDiscount AS FDC 
LEFT JOIN tblCompanyInfo AS CI ON FDC.CompanyId = CI.CompanyId
LEFT JOIN tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = FDC.CustMasterId
WHERE ActionStatus IN ('" + actionStatus + "') AND FDC.IsActive = 1 AND FDC.CompanyId IN (SELECT DISTINCT C.CompanyId FROM tblUserCompanyUnit UC LEFT JOIN tblCompanyInfo AS C ON UC.CompanyUnitId = C.CompanyId WHERE UserId = " + userId + " AND CompanyId IS NOT NULL)";

            return _aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateDiscount(FixedDiscountDao aOrderInfoMaster)
        {
            string query = @"UPDATE tblCustomerFixedDiscount SET ActionStatus = '" + aOrderInfoMaster.ActionStatus + "',ApproveBy = '" + aOrderInfoMaster.ApproveBy + "',ApproveDate = " + aOrderInfoMaster.ApproveDate + " WHERE DiscountId = " + aOrderInfoMaster.DiscountId;
            return _aInternalDal.DeleteDataByCommand(query, "SSIDB");
        }
    }
}
