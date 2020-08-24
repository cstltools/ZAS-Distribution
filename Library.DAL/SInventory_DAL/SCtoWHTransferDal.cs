using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class SCtoWHTransferDal
    {

        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void LoadManufac(DropDownList ddl)
        {
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public DataTable LoadComUnit(string ComUnitId)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM tblCompanyUnit where ComUnitCode='" + ComUnitId.Trim() + "' ";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTable;
        }

        public void DCLoad(DropDownList aDownList, string comUnitId)
        {

            string queryStr = "select ComUnitId, ComUnitName  from tblCompanyUnit WHERE " +
                                       " ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId='" + comUnitId.Trim() + "')";

            aCommonInternalDal.LoadDropDownValueWithoutDataBase(aDownList, "ComUnitName", "ComUnitId", queryStr);
        }

        public void DCLoad(DropDownList aDownList)
        {
            string dc = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit";
            aCommonInternalDal.LoadDropDownValue(aDownList, "Com", "ComUnitId", dc, "SSIDB");
        }
        

        
    }
}
