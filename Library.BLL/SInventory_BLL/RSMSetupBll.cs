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
    public class RSMSetupBll
    {
        RSMSetupDal aDal = new RSMSetupDal();

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            aDal.GetCompanyDropdownList(ddl);
        }

        public void LoadRegionDropdownList(DropDownList ddl, string companyId)
        {
            aDal.GetRegionDropdownList(ddl, companyId);
        }

        public void LoadEmployeeDropDownList(DropDownList ddl, string companyId)
        {
            aDal.GetEmployeeDropdownList(ddl, companyId);
        }

        public bool LoadRSMInfoForSave(RSMSetupDao aInformationDao)
        {
            return aDal.SaveRSMInformation(aInformationDao);
        }

        public DataTable LoadGridviewData()
        {
            return aDal.GetGridviewData();
        }

        public DataTable LoadRSMInfoByID(string rsmId)
        {
            return aDal.GetRSMInfoByID(rsmId);
        }

        public void LoadRegionDropDownList(DropDownList ddl)
        {
           aDal.GetRegionDropdownList(ddl);
        }

        public bool UpdateRsmInfo(RSMSetupDao rsmInfo)
        {
            return aDal.UpdateRSMInformation(rsmInfo);
        }

        public bool CheckAssignedOrNot(string companyId, string regionId, int rsmId)
        {
            return aDal.GetCheckAssignedOrNot(companyId, regionId, rsmId);
        }

        public void LoadEmployeeDropDownList(DropDownList ddl)
        {
            aDal.GetEmployeeDropDownList(ddl);
        }
    }
}
