using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_AIT_Report : System.Web.UI.Page
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
            dcDropDownList.Items.Clear();
        }


    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            dcDropDownList.Items.Clear();
        }
    }


   

    protected void submitButton_Click(object sender, EventArgs e)
    {
        invoiceGridView.DataSource = null;
        invoiceGridView.DataBind();

        DataTable aTable = aReportDal.LoadAitInvoice(GetPrameterList());

        if (aTable.Rows.Count > 0)
        {
            invoiceGridView.DataSource = aTable;
            invoiceGridView.DataBind();
        }
        else
        {
            invoiceGridView.DataSource = null;
            invoiceGridView.DataBind();
        }        
    }

    private bool Validation()
    {
        if (groupDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select group Name !!");
            return false;
        }

        if (fromDateTextBox.Text != "")
        {
            if (toDateTextBox.Text == "")
            {
                ShowMessageBox("Please Select delivery To date !!");
                return false;
            }

        }

        if (toDateTextBox.Text != "")
        {
            if (fromDateTextBox.Text == "")
            {
                ShowMessageBox("Please Select delivery from date !!");
                return false;
            }

        }

        return true;
    }

    private void PopUpCustomerPaymentReport(string id)
    {
        Session["paydetailId"] = "";
        Session["paydetailId"] = id;

        const string url = "../SInventory_RPTVIEW/AITReceiptViewer.aspx";
        const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + "AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        if (dcDropDownList.SelectedValue != "")
        {
            parameter = parameter + "AND INV.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
        }


        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            parameter = parameter + "AND CP.PaymentDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AND ' " + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "'";
        }


        return parameter;

    }

    protected void customerNameTextBox1_TextChanged(object sender, EventArgs e)
    {
        if (customerNameTextBox1.Text.Contains(':'))
        {
            string[] cust = customerNameTextBox1.Text.Split(':');
            customerNameTextBox1.Text = cust[0];
        }


    }

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

    private void Clear()
    {
        groupDropDownList.SelectedValue = "";
        companyNameDropDownList.Items.Clear();
        dcDropDownList.Items.Clear();
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {

        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        PopUpCustomerPaymentReport(invoiceGridView.DataKeys[rowindex]["CustPayDetailId"].ToString());
    }
}