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
    public class MIOSetupBll
    {

        MIOSetupDal aDal = new MIOSetupDal();

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            aDal.GetCompanyDropdownList(ddl);
        }

        public void LoadEmployeeDropDownList(DropDownList ddl, string companyId)
        {
            aDal.GetEmployeeDropdownList(ddl, companyId);
        }

        public void LoadRegionDropdownList(DropDownList ddl, string companyId)
        {
            aDal.GetRegionDropdownList(ddl, companyId);
        }

        public void LoadEmployeeDropDownList(DropDownList ddl)
        {
            aDal.GetEmployeeDropdownList(ddl);
        }

        public bool LoadMIOInfoForSave(MIOSetupDao aInformationDao)
        {
            return aDal.SaveMIOInformation(aInformationDao);
        }

        public DataTable LoadGridviewData()
        {
            return aDal.GetGridviewData();
        }

        public DataTable LoadMIOInfoByID(string rsmId)
        {
            return aDal.GetMIOInfoByID(rsmId);
        }

        public void LoadRegionDropDownList(DropDownList ddl)
        {
            aDal.GetRegionDropdownList(ddl);
        }

        public bool UpdateMIOInfo(MIOSetupDao mioInfo)
        {
            return aDal.UpdateMIOInformation(mioInfo);
        }

        public bool CheckAssignedOrNot(string companyId, string areaId, int asmId)
        {
            return aDal.GetCheckAssignedOrNot(companyId, areaId, asmId);
        }

        public void LoadAreaDropDownList(DropDownList ddl, string regionId, string companyId)
        {
            aDal.GetAreaDropdownList(ddl,regionId ,companyId);
        }

        public void LoadAreaDropDownList(DropDownList ddl, string regionId)
        {
            aDal.GetAreaDropdownList(ddl, regionId);
        }

        public void LoadTerritoryDropDownList(DropDownList ddl, string areaId, string companyId)
        {
            aDal.GetTerritoryDropdownList(ddl, areaId, companyId);
        }

        public void LoadTerritoryDropDownList(DropDownList ddl, string areaId)
        {
            aDal.GetTerritoryDropdownList(ddl, areaId);
        }
    }
}
