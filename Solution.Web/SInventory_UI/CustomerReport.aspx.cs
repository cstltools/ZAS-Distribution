using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerReport : System.Web.UI.Page
{

    CustomerReportDal aReportDal = new CustomerReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
        aReportDal.LoadRegionName(regionDropDownList);
    }

    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            aReportDal.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
        }
    }

    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (areaDropDownList.SelectedValue != "")
        {
            aReportDal.LoadTerritoryOnDropdownList(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
        }
    }

    protected void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (territoryDropDownList.SelectedValue != "")
        {
            aReportDal.LoadMarketOnDropdownList(marketDropDownList, territoryDropDownList.SelectedValue);
        }
        else
        {
            marketDropDownList.Items.Clear();
        }
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        PopupReport();
    }

    private void PopupReport()
    {

        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        string url = "../SInventory_RPTVIEW/CustomerReportViewer.aspx";
        string fullURL =
            "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
            "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
     }

    private string GetPrameterList()
    {
        string parameter = "";

         
        if (regionDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND tblRegion.RegionId = " + regionDropDownList.SelectedValue ;
        }   

        
        if (areaDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND tblArea.AreaId = " + areaDropDownList.SelectedValue;
        }
        
        
        if (territoryDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND tblTerritory.TerritoryId = " + territoryDropDownList.SelectedValue;
        }

        if (marketDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND tblMarket.MarketId = " + marketDropDownList.SelectedValue;
        }
              
            
        return parameter;
    }


    protected void cancelButton_Click(object sender, EventArgs e)
    {
        regionDropDownList.SelectedValue = "";
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
    }
}