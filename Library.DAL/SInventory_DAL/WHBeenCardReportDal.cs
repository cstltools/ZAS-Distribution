using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class WHBeenCardReportDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetWhInformation(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT WearhouseId, WearhouseCode + ' : ' + WearhouseName AS WearhouseName FROM tblWearhouse WHERE CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", queryStr, "SSIDB");
        }

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId,CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }
    }
}
