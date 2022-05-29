using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
     public class NationalBeenCaedDal
    {

        private readonly ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetWhBinCardReport(string fromDate, string toDate, string CiD, string DC, string comId)
        {

            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@FromDate", fromDate));
            aSqlParameterList.Add(new SqlParameter("@ToDate", toDate));
            aSqlParameterList.Add(new SqlParameter("@CompanyId", comId));

            return aCommonInternalDal.GetDataTableAction("sp_NationalBinCard", aSqlParameterList, "SSIDB");

        }
    }
}
