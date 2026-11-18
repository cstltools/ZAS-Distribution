using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_NationalStockReport : System.Web.UI.Page
{
    readonly WhStockConditionFreezeBll _aConditionFreezeBll = new WhStockConditionFreezeBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
        //_aConditionFreezeBll.LoadWhInfoOnDropDownList(whDropDownList);
        _aConditionFreezeBll.LoadCompanyDropdownList(companyNameDropDownList);
    }


    protected void reportButton_OnClick(object sender, EventArgs e)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GstParameter();

        string url = "../SInventory_RPTVIEW/NationalStockReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void htmlReportButton_OnClick(object sender, EventArgs e)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GstParameter();
        Session["CompanyName"] = companyNameDropDownList.SelectedItem.Text;

        string url = "../HTMLReport/NationalStockReportHTMLReport.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW_HTML", fullURL, true);
    }

    private string GstParameter()
    {
        string pram = "WHERE vTblTotal.ProductCode IS NOT NULL";

        if (companyNameDropDownList.SelectedValue != "")
        {
            int companyId;
            if (!int.TryParse(companyNameDropDownList.SelectedValue, out companyId)) companyId = 0;
            pram = pram + " AND vTblTotal.ProductCode IN (SELECT ProductCode FROM tblProduct WHERE CompanyId = " + companyId + ")";
        }

        return pram;
    }
}