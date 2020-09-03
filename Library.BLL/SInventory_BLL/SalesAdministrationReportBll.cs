using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class SalesAdministrationReportBll
    {
        SalesAdministrationReportDal administrationDal = new SalesAdministrationReportDal();

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            administrationDal.GetCompanyDropdownList(ddl);
        }

        public DataTable LoadASMInformation(string parameter)
        {
            return administrationDal.GetASMInformation(parameter);
        }

        public DataTable LoadMIOInformation(string parameter)
        {
            return administrationDal.GetMIOInformation(parameter);
        }

        public DataTable LoadRSMInformation(string parameter)
        {
            return administrationDal.GetRSMInformation(parameter);

        }
    }
}
