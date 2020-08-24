using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using System.Data;

namespace Library.BLL.SInventory_BLL
{
  public  class EmployeeInformationBLL
    {
      EmployeeInformationDAL aEmployeeInformationDAL = new EmployeeInformationDAL();

      public void GroupDropDown(DropDownList groupDropDownList)
        {
            aEmployeeInformationDAL.GroupDropDown(groupDropDownList);
        }

      public void CompanyNameDropDown(DropDownList companyNameDropDownList)
      {
          aEmployeeInformationDAL.CompanyNameDropDown(companyNameDropDownList);
      }

      public DataTable LoadAllEmployeeInformation(string parameter)
      {
          return aEmployeeInformationDAL.GetAllEmployeeInformation(parameter);
      }


      public DataTable LoadDeliveryPerson(string parameter)
      {
          return aEmployeeInformationDAL.GetDeliveryPerson(parameter);
      }
    }
}
