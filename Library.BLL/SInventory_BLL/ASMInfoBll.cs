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
    public class ASMInfoBll
    {
        ASMInfoDal aDal = new ASMInfoDal();

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

        public bool LoadRSMInfoForSave(ASMInfoDao aInformationDao)
        {
            return aDal.SaveASMInformation(aInformationDao);
        }

        public DataTable LoadGridviewData()
        {
            return aDal.GetGridviewData();
        }

        public DataTable LoadASMInfoByID(string rsmId)
        {
            return aDal.GetASMInfoByID(rsmId);
        }

        public void LoadRegionDropDownList(DropDownList ddl)
        {
            aDal.GetRegionDropdownList(ddl);
        }

        public bool UpdateAsmInfo(ASMInfoDao asmInfo)
        {
            return aDal.UpdateASMInformation(asmInfo);
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

        public void LoadEmployeeDropDownList(DropDownList ddl)
        {
            aDal.GetEmployeeDropdownList(ddl);
        }
    }
}
