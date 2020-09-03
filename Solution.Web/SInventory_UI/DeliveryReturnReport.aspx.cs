using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_DeliveryReturnReport : System.Web.UI.Page
{
    MioWiseCustomerDal aReportDal = new MioWiseCustomerDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();

            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    CheckBox1.Enabled = false;
                }
                else
                {
                    CheckBox1.Enabled = true;
                }
            }
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    public void LoadDropDown()
    {
        dcDropDownList1.Items.Clear();
        aReportDal.LoadGroupInfo(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);

        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
            dcDropDownList1.Items.Clear();
            
        }

        //OtherStockActionBLL aOtherStockActionBLL = new OtherStockActionBLL();
        //aOtherStockActionBLL.DCLoad(dcDropDownList1);
    }


    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aReportDal.LoadWhInformationByUser(dcDropDownList1, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadDcInfo(dcDropDownList1, companyNameDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            } 

            
        }
        else
        {
            dcDropDownList1.Items.Clear();
        }
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        Session["DeliveryReturnRpt"] = "";
        Session["DeliveryReturnRpt"] = 0;

        if (InvoiceDateTextBox.Text != "" && todateTextBox.Text != "" && dcDropDownList1.SelectedValue != "")
        {
            if (todateTextBox.Text == "")
            {
                InvoiceDateTextBox.Text = todateTextBox.Text;
            }

            string fromDate = InvoiceDateTextBox.Text;
            string toDate = todateTextBox.Text;
            string districtId = dcDropDownList1.SelectedValue;

            //string url = "../SInventory_RPTVIEW/DeliveryReturnReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&districtId=" + districtId;
            //// string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            //string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            string url = "../HTMLReport/DelivaryReturnHTMLReport.aspx?fromDate=" + fromDate + "&toDate=" + toDate +
                         "&districtId=" + districtId;
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL =
                "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
                "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof (string), "OPEN_WINDOW", fullURL, true);
        }
        else
        {
            showMessageBox("Please ensure date range (From-TO) & DC !!!!");
        }
        if (CheckBox1.Checked && todateTextBox.Text != "" && InvoiceDateTextBox.Text != "")
        {
            string fromDate = InvoiceDateTextBox.Text;
            string toDate = todateTextBox.Text;
            string districtId = dcDropDownList1.SelectedValue;
            int i = 1;

            //string url = "../SInventory_RPTVIEW/DeliveryReturnReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&NationalReport=" + 1;
            //// string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            //string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            string url = "../HTMLReport/DelivaryReturnHTMLReport.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&NationalReport=" + 1;
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
    protected void dcDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void CheckBox1_CheckedChanged1(object sender, EventArgs e)
    {
        if (CheckBox1.Checked)
        {
            dcDropDownList1.Enabled = false;
            companyNameDropDownList.Enabled = false;
        }
        if (CheckBox1.Checked == false)
        {
            dcDropDownList1.Enabled = true;
            companyNameDropDownList.Enabled = true;
        }
    }

    protected void viewRptButton_Click(object sender, EventArgs e)
    {

        Session["DeliveryReturnRpt"] = "";
        Session["DeliveryReturnRpt"] = 1;

        if (InvoiceDateTextBox.Text != "" && todateTextBox.Text != "" && dcDropDownList1.SelectedValue != "")
        {
            if (todateTextBox.Text == "")
            {
                InvoiceDateTextBox.Text = todateTextBox.Text;
            }

            string fromDate = InvoiceDateTextBox.Text;
            string toDate = todateTextBox.Text;
            string districtId = dcDropDownList1.SelectedValue;

            string url = "../SInventory_RPTVIEW/DeliveryReturnReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&districtId=" + districtId;
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        if (CheckBox1.Checked && todateTextBox.Text != "" && InvoiceDateTextBox.Text != "")
        {
            string fromDate = InvoiceDateTextBox.Text;
            string toDate = todateTextBox.Text;
            string districtId = dcDropDownList1.SelectedValue;
            int i = 1;
            string url = "../SInventory_RPTVIEW/DeliveryReturnReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&NationalReport=" + 1;
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        LoadDropDown();
        InvoiceDateTextBox.Text = "";
        todateTextBox.Text = "";
        CheckBox1.Checked = false;
    }
}