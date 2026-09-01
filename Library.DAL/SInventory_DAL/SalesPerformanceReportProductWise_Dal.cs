using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DAL.MAIN_FUNCTION;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;

namespace Library.DAL.SInventory_DAL
{
    public class SalesPerformanceReportProductWise_Dal
    {

        DB_Manager aDbManager = new DB_Manager();

        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        // Create a connection to the specified database
        public void CreateConnection(string dbName)
        {
            aDbManager.CreateConnection(dbName);
        }

        // Default connection to SALESDB
        public void CreateConnection()
        {
            aDbManager.CreateConnection(DB_Names.SALESDB);
        }

        // Close all open connections
        public void CloseAllConnection()
        {
            aDbManager.CloseConnection();
        }


        public DataTable GetSalesVsTargetReport_ProductWise(
    int? companyId, 
    int? comUnitId, 
    int? regionId, 
    int? areaId, 
    int? territoryId, 
    int? mioId, 
    string period, 
    int year)
{
    List<SqlParameter> prms = new List<SqlParameter>
    {
        new SqlParameter("@CompanyId", (object)companyId ?? DBNull.Value),
        new SqlParameter("@ComUnitId", (object)comUnitId ?? DBNull.Value),
        new SqlParameter("@RegionId", (object)regionId ?? DBNull.Value),
        new SqlParameter("@AreaId", (object)areaId ?? DBNull.Value),
        new SqlParameter("@TerritoryId", (object)territoryId ?? DBNull.Value),
        new SqlParameter("@MioId", (object)mioId ?? DBNull.Value),
        new SqlParameter("@Period", (object)period ?? DBNull.Value),
        new SqlParameter("@Year", (object)year ?? DBNull.Value)
    };

    return aDbManager.GetDataTableAction("GetSalesVsTargetReport_ProductWise", prms);
}


    }
}

