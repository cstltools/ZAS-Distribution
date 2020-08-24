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
    public class SlaveApprovalDal
    {

        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        
        public bool ApprovalUpdateDAL(SlaveInformation aCreditLimitDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@SlaveId", aCreditLimitDao.SlaveId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aCreditLimitDao.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aCreditLimitDao.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aCreditLimitDao.ApprovedDate));

            const string query = @"UPDATE tblSlaveInfo SET ActionStatus = @ActionStatus,ApprovedBy = @ApprovedBy,ApprovedDate = @ApprovedDate WHERE SlaveId = @SlaveId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        }
        public DataTable LoadSlavePostedInfo(string actionstatus)
        {
            const string query = @"SELECT * FROM tblSlaveInfo AS SLV
                                  INNER JOIN tblCompanyInfo AS CI ON SLV.CompanyId = CI.CompanyId
                                  WHERE SLV.IsActive = 'True' AND SLV.ActionStatus = 'Posted'";
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
        
    }
}
