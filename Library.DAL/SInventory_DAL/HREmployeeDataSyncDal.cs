using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;

namespace Library.DAL.SInventory_DAL
{
    public class HREmployeeDataSyncDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        DB_Manager aDbManager = new DB_Manager();


        public DataTable GetEmployeeDataFromHr(string parameter)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@Parameter", parameter));

            return aCommonInternalDal.GetDataTableAction("sp_Employee_EmployeeListFromHR", aSqlParameterList, "SSIDB");
        }


        public bool SyncEmployeById(string empMasterCode)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@EmployeeMasterCode", empMasterCode));

            return aCommonInternalDal.UpdateAction("sp_Employee_EmployeeListSyncedWithDistibution", aSqlParameterlist);
        }

        public bool SyncCompanyEmployeById(string empMasterCode)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@EmployeeMasterCode", empMasterCode));

            return aCommonInternalDal.UpdateAction("sp_Employee_EmployeeCompanySyncedWithDistibution", aSqlParameterlist);
        }
    }
}
