using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_SalesHubReport : System.Web.UI.Page
{
    SalesHubBLL aSalesHubBLL = new SalesHubBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
       
        }
    }

    private void ControlDivVisibility(){
        region.Visible = false;
        area.Visible = false;
        territory.Visible = false;
    }

    private void LoadRegionInfo()
    {
        aSalesHubBLL.LoadRegionDropdownList(regionDropDownList);
    }

    private void LoadAreaInfo()
    {
        if (regionDropDownList.SelectedValue != "") {

            aSalesHubBLL.LoadAreaDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
    }

    private void LoadTarritoryInfo()
    {
        if (areaDropDownList.SelectedValue != "")
        {
            aSalesHubBLL.LoadTerritoryDropdownList(territoryDropDownList, areaDropDownList.SelectedValue);
        }
    }

    protected void reportNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ControlDivVisibility();

        if (reportNameDropDownList.SelectedValue == "AREA")
        {
            region.Visible = true;
            LoadRegionInfo();
        }


        if (reportNameDropDownList.SelectedValue == "TERRITORY")
        {
            region.Visible = true;
            LoadRegionInfo();
           
        }

        if (reportNameDropDownList.SelectedValue == "MARKET")
        {
            region.Visible = true;
            LoadRegionInfo();
        }
    }

    protected void regionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (reportNameDropDownList.SelectedValue == "TERRITORY" || reportNameDropDownList.SelectedValue == "MARKET")
        {
            area.Visible = true;
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            LoadAreaInfo();
        }

        //if (regionDropDownList.SelectedIndex == 0)
        //{
        //   //areaDropDownList.SelectedIndex = 0;
        //    areaDropDownList.Text = string.Empty;

        //   areaDropDownList.Enabled = false;

        //   //territoryDropDownList.SelectedIndex = 0;
        //   territoryDropDownList.Text = string.Empty;

        //   territoryDropDownList.Enabled = false;
        //}
    }

    protected void areaDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (reportNameDropDownList.SelectedValue == "MARKET")
        {
            territory.Visible = true;
            LoadTarritoryInfo();
        }

    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (reportNameDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Select Report Name");
            return false;
        }

        return true;
    }


    protected void clearButton_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        reportNameDropDownList.SelectedIndex = 0;       

        region.Visible = false;
        area.Visible = false;
        territory.Visible = false;
        regionDropDownList.Items.Clear();
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();

    }

    protected void submitButton_OnClick(object sender, EventArgs e)
    {
        if (reportNameDropDownList.SelectedValue == "REGION")
        {
            PopupREGIONReport();
        }

        if (reportNameDropDownList.SelectedValue == "AREA")
        {
            PopupREGIONReport();
        }

        if (reportNameDropDownList.SelectedValue == "TERRITORY")
        {
            PopupREGIONReport();
        }


        if (reportNameDropDownList.SelectedValue == "MARKET")
        {
            PopupREGIONReport();
        }
    }


    private void PopupREGIONReport()
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();
        string rptType = reportNameDropDownList.SelectedValue;

        string url = "../SInventory_RPTVIEW/SalesHubReportViewer.aspx?rptType=" + rptType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private string GetPrameterList()
    {
        string parameter = "";

        if (reportNameDropDownList.SelectedValue != "")
        {
            if (reportNameDropDownList.SelectedValue == "REGION")
            {
                parameter = "";
            }

            if (reportNameDropDownList.SelectedValue == "AREA")
            {
                if (regionDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " WHERE REG.RegionId = '" + regionDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }

            }

            if (reportNameDropDownList.SelectedValue == "TERRITORY")
            {
                if (areaDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " WHERE ARE.AreaId = '" + areaDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }
            }

            if (reportNameDropDownList.SelectedValue == "MARKET")
            {
                if (territoryDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " WHERE TER.TerritoryId = '" + territoryDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }

            }
        }

        return parameter;
    }

}