using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{

    public class WingsSalesAndCollectionReportBll
    {
        WingsSalesAndCollectionReportDal andCollectionReportDal = new WingsSalesAndCollectionReportDal();
        
        public void LoadGroupDropdownList(DropDownList ddl)
        {
            andCollectionReportDal.GetGroupDropdownList(ddl);
        }

        public void LoadCompanyDropdownList(DropDownList ddl, string groupId)
        {
            andCollectionReportDal.LoadCompanyName(ddl, groupId);
        }

        public void LoadDdlDc(DropDownList ddl, string companyId)
        {
            andCollectionReportDal.LoadDC(ddl,companyId);
        }

        public DataTable LoadReportInfo(string parameter, DateTime fromDate, DateTime toDate)
        {
            return andCollectionReportDal.GetReportInfo(parameter, fromDate, toDate);
        }
    }
}
