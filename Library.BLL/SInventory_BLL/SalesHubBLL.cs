using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Library.DAL.SInventory_DAL;
using System.Web.UI.WebControls;

namespace Library.BLL.SInventory_BLL
{

   public class SalesHubBLL
    {
       SalesHubDAL aSalesHubDAL = new SalesHubDAL();


       public DataTable LoadREGIONHub(string parameter)
       {
           return aSalesHubDAL.GetRegionHub(parameter);
       }

       public void LoadRegionDropdownList(DropDownList regionDropDownList)
       {
           aSalesHubDAL.GetRegionDropdownList(regionDropDownList);
       }

       public DataTable LoadAreaHub(string parameter)
       {
           return aSalesHubDAL.GetAreaHub(parameter);
       }


       public void LoadAreaDropdownList(DropDownList areaDropDownList, string regionDropDownList)
       {
           aSalesHubDAL.GetAreaDropdownList(areaDropDownList, regionDropDownList);
       }

       public void LoadTerritoryDropdownList(DropDownList territoryDropDownList, string areaDropDownList)
       {
           aSalesHubDAL.GetTerritoryList(territoryDropDownList, areaDropDownList);
       }

       public DataTable LoadTerritoryHub(string parameter)
       {
           return aSalesHubDAL.GetTerritoryHub(parameter);
       }

       public DataTable LoadMarketHub(string parameter)
       {
           return aSalesHubDAL.GetMarketHub(parameter);
       }
    }
}
