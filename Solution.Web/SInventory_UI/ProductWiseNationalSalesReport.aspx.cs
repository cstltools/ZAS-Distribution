using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_ProductWiseNationalSalesReport : System.Web.UI.Page
{
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();

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

    


    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            PopUpCustomerPaymentReport();
        }
    }

    private bool Validation()
    {

        if (groupDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select group Name !!");
            return false;
        }

        if (fromDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select invoice from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select invoice To date !!");
            return false;
        }

        return true;
    }

    private void PopUpCustomerPaymentReport()
    {
        Session["ComId"] = "";
        Session["FDate"] = "";
        Session["TDate"] = "";

        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();


        if (companyNameDropDownList.SelectedValue != "")
        {
            Session["ComId"] = companyNameDropDownList.SelectedValue;
        }

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            Session["FDate"] = fromDateTextBox.Text.Trim();
            Session["TDate"] = toDateTextBox.Text.Trim();
        }

        //Session["PRAM"] = GetPrameterList();

        string url = "../HTMLReport/ProductWiseNationalSalesHTMLReport.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    //private String GetPrameterList()
    //{
    //    string parameter = "";


    //    if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
    //    {
    //        parameter = parameter + " AND INV.InvoiceDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AND '" + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "'";
    //    }

    //    return parameter;

    //}

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    private String GetPrameterList()
    {
        string parameter = "  ";

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            parameter = parameter + " WHERE INV.InvoiceDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AND ' " + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "'";
        }

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }


        return parameter;

    }

    private void Clear()
    {
        groupDropDownList.SelectedValue = "";
        companyNameDropDownList.Items.Clear();

        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
        LoadDropdownList();
    }
}