using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class UserCredentialChangeDal
    {
        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();
        public DataTable GetUserCredentialLog(string pram)
        {
            string query = @"SELECT UR.UserName AS CurrentUseName,USRL.* FROM tblUser AS UR 
LEFT JOIN (SELECT UL.UserId,UL.UserName,UL.UserCode,UL.LoginName,UL.Password,UL.IsAppsUser,UL.IsDepotDashboard,USR.UserName AS UpdateBy,UL.UpdateDate FROM tblUserLog AS UL
LEFT JOIN tblUser AS USR ON UL.UpdateBy = USR.UserId) AS USRL ON UR.UserId = USRL.UserId
WHERE UR.UserId IS NOT NULL " + pram + " ORDER BY USRL.UpdateDate DESC";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
