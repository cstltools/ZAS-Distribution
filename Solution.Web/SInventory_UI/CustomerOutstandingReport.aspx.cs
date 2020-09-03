using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_CustomerOutstandingReport : System.Web.UI.Page
{
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    CustomerMasterBLL aCustomerMasterBll = new CustomerMasterBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Session["UserType"].ToString() != "")
            //{
            //    if (Session["UserType"].ToString() == "Admin")
            //    {
            //        aCustomerMasterBll.LoadDcDropDownList(dcDropDownList);
            //    }
            //    else
            //    {
            //        string comUnit = Session["ComUnitId"].ToString();
            //        aCustomerMasterBll.LoadDcDropDownList(dcDropDownList, comUnit);
            //    }
            //}
            //else
            //{
            //    Response.Redirect("Login.aspx");
            //}
            if (!IsPostBack)
            {
                DropDownlist();
            }
        }
    }
    protected void CheckBox1_CheckedChanged1(object sender, EventArgs e)
    {
        if (CheckBox1.Checked)
        {
            salesCenterDropDownList.Enabled = false;
            salesCenterDropDownList.SelectedValue = string.Empty;
        }
        if (CheckBox1.Checked == false)
        {
            salesCenterDropDownList.Enabled = true;
        }
    }
    public void DropDownlist()
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        aOrderInfoBll.LoadSC(salesCenterDropDownList, Session["UserId"].ToString());
        //salesCenterDropDownList.SelectedIndex = 1;
    }
    public void LoadDC()
    {
       // aRequisitionBll.DCLoad(dcDropDownList);
    }
    protected void viewRptButton_Click(object sender, EventArgs e)
    {

        if (CheckBox1.Checked==false)
        {
            Session["PaymentReport"] = "";
            Session["PaymentReport"] = "SC";
            if (fromDateTextBox.Text != "" && todateTextBox.Text != "" && paymentTermsDropdown.SelectedValue != "" && salesCenterDropDownList.SelectedValue != "")
            {
                string fromDate = fromDateTextBox.Text;
                string toDate = todateTextBox.Text;
                string url = "../SInventory_RPTVIEW/CustomerPaymentViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&payment=" + paymentTermsDropdown.SelectedValue + "&salesCenter=" + salesCenterDropDownList.SelectedValue;
                string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            else
            {
                showMessageBox("Select Date Range and Sales Center Name!!");
            }
        }
        else
        {
            Session["PaymentReport"] = "";
            Session["PaymentReport"] = "NAT";
            if (fromDateTextBox.Text != "" && todateTextBox.Text != "" && paymentTermsDropdown.SelectedValue != "" && CheckBox1.Checked ==true)
            {
                string fromDate = fromDateTextBox.Text;
                string toDate = todateTextBox.Text;
                string url = "../SInventory_RPTVIEW/CustomerPaymentViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&payment=" + paymentTermsDropdown.SelectedValue + "&salesCenter=" + salesCenterDropDownList.SelectedValue;
                string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            else
            {
                showMessageBox("Select Date Range and National CheckBox!!");
            }
        }
      
    }
    protected void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        aOrderInfoBll.LoadMarket(marketDropDownList, salesCenterDropDownList.SelectedValue);
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}