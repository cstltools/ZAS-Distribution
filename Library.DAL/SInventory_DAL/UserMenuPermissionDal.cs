using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class UserMenuPermissionDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
        public void LoadUser(DropDownList ddl)
        {
            string queryStr = "SELECT UserId,UserName FROM tblUser WHERE UserId != 1";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "UserName", "UserId", queryStr);
        }

        public DataTable LoadMenuPermissionList(string parameter)
        {
            string query = @"SELECT USR.UserName,MDLE.ManuName Parent,MN.ManuName FROM tblMenuDistribution AS MD
                             LEFT JOIN tblUser AS USR ON MD.UserId = USR.UserId
                             LEFT JOIN tblMainMenu AS MN ON MD.MenuSL = MN.SL
                             LEFT JOIN tblMainMenu AS MDLE ON MN.ParantId = MDLE.SL
                             WHERE USR.UserId IS NOT NULL AND MDLE.ManuName IS NOT NULL " + parameter + " ORDER BY USR.UserName";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
