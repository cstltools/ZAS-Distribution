using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_WingsSalesAndCollectionReport : System.Web.UI.Page
{
    WingsSalesAndCollectionReportBll andCollectionReportBll = new WingsSalesAndCollectionReportBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
        }
    }

    private void LoadDropDownList()
    {
        andCollectionReportBll.LoadGroupDropdownList(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);

        companyNameDropDownList.Items.Clear();
        dcDropDownList.Items.Clear();

        if (groupDropDownList.SelectedValue != "")
        {
            andCollectionReportBll.LoadCompanyDropdownList(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();
        }
    }

    protected void companyNameDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        dcDropDownList.Items.Clear();

        if (companyNameDropDownList.SelectedValue != "")
        {
            andCollectionReportBll.LoadDdlDc(dcDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            dcDropDownList.Items.Clear();
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            PopupReport();
        }
    }

    private void PopupReport()
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        Session["FromDate"] = "";
        Session["FromDate"] = fromDateTextBox.Text;

        Session["ToDate"] = "";
        Session["ToDate"] = toDateTextBox.Text;


        const string url = "../SInventory_RPTVIEW/WingsSalesAndCollectionReportViewer.aspx";
        const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + "AND ODR.ManufacId = '" + companyNameDropDownList.SelectedValue + "'"; 
        }

        if (dcDropDownList.SelectedValue != "")
        {
            parameter = parameter + "AND INV.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
        }

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            parameter = parameter + "AND (INV.InvoiceDate BETWEEN FORMAT(CAST('" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AS DATE),'MM/dd/yy') AND FORMAT(CAST('" + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "' AS DATE),'MM/dd/yy')) ";
        }


        return parameter;

    }

    private bool Validation()
    {


        //if (companyNameDropDownList.SelectedValue == "")
        //{
        //    ShowMessageBox("Company Name is required !!");
        //    return false;
        //}

        //if (dcDropDownList.SelectedValue == "")
        //{
        //    ShowMessageBox("DC Name is required !!");
        //    return false;
        //}

        if (fromDateTextBox.Text == "")
        {
            ShowMessageBox("From date is required !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("To date is required !!");
            return false;
        }

        return true;
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        LoadDropDownList();

        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
    }
}