using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class DCUserPermissionBll
    {
        DCUserPermissionDal aDcUserPermissionDal = new DCUserPermissionDal();

        public void LoadUserInfo(DropDownList ddl)
        {
            aDcUserPermissionDal.GetUserInfo(ddl);
        }

        public DataTable LoadDCList()
        {
            return aDcUserPermissionDal.GetDCList();
        }

        public bool SaveDCUserPermissions(int userId, List<int> companyUnitIds)
        {
            return aDcUserPermissionDal.SaveDCUserPermissions(userId, companyUnitIds);
        }

        public DataTable LoadDCUserPermissionById(string userId)
        {
            return aDcUserPermissionDal.GetDCUserPermissionById(userId);
        }
    }
}
