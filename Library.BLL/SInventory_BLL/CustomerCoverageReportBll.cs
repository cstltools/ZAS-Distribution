using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class CustomerCoverageReportBll
    {
        CustomerCoverageReportDal aCoverageReportDal = new CustomerCoverageReportDal();

  
        public DataTable LoadMiaWiseCustomerCoverageInfo(string fromDate, string toDate)
        {
            return aCoverageReportDal.GetMiaWiseCustomerCoverageInfo(fromDate, toDate);
        }

        public DataTable LoadDZSMWiseCustomerCoverageInfo(string fromDate, string toDate)
        {
            return aCoverageReportDal.GetDZSMWiseCustomerCoverageInfo(fromDate, toDate);
        }
    }
}
