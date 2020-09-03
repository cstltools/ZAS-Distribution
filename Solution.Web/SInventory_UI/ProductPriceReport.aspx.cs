using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_ProductPriceReport : System.Web.UI.Page
{

    CustomerPaymentReportDal aReportDal = new CustomerPaymentReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {

        aReportDal.LoadGroupInfo(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);

        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
        }
    }
   
    protected void viewRptButton_Click(object sender, EventArgs e)
    {

        if (reportTypeDropDownList.SelectedValue != "NULL")
        {
            Session["PRAM"] = "";

            if (companyNameDropDownList.SelectedValue != "")
            {
                
                Session["PRAM"] = GetPrameterList();
            }

            string reportType = reportTypeDropDownList.SelectedValue;

            string url = "../SInventory_RPTVIEW/ProductTreadPriceViewer.aspx?reportType=" + reportType;
            string fullURL =
                "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
                "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof (string), "OPEN_WINDOW", fullURL, true);
        }
        else
        {
            ShowMessageBox("Please Select a Report Type !!!");
        }
    }

    private object GetPrameterList()
    {
        string parameter = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND PD.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        return parameter;
    }


    protected void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}