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
    public class SlaveinformationBll
    {

        SlaveInformationDal aDal = new SlaveInformationDal();

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            aDal.GetCompanyDropdownList(ddl);
        }

        public bool LoadSlaveDataForSave(SlaveInformation aInformation)
        {
            return aDal.SaveSlaveInformation(aInformation);
        }

        public bool ChangeStatusToFalse(SlaveInformation aInformation)
        {
            return aDal.UpdateSlaveInformation(aInformation);
        }

        public DataTable LoadSlaveInformation()
        {
            return aDal.GetSlaveInformation();
        }

        public DataTable LoadSlaveInformationById(string slaveId)
        {
            return aDal.GetSlaveInformationById(slaveId);
        }

        public DataTable LoadSlaveInformationByCompanyId(string companyId)
        {
            return aDal.GetSlaveInformationByCompanyId(companyId);
        }
    }
}
