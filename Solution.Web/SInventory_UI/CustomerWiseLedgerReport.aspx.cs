using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerWiseLedgerReport : System.Web.UI.Page
{
    CustomerWiseReturnDal aReportDal = new CustomerWiseReturnDal();
    ManualOrderCreationDal aOrderCreationDal = new ManualOrderCreationDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }


    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {

        if (custCodeTextBox.Text.Contains(':'))
        {
            custIdHiddenField.Value = "";
            string[] data = custCodeTextBox.Text.Split(':');
            custCodeTextBox.Text = data[0];
            custIdHiddenField.Value = data[3];

        }
        else   
        {
            if (custCodeTextBox.Text != "")
            {
                custIdHiddenField.Value = "";
                DataTable aTable = aOrderCreationDal.GetCustomerInfoByCode(custCodeTextBox.Text.Trim());

                if (aTable.Rows.Count > 0)
                {
                    custIdHiddenField.Value =
                        aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    custIdHiddenField.Value = "";
                    custCodeTextBox.Text = "";
                    Clear();
                    ShowMessageBox("Customer does not exist or not contain enough information !!!!");
                }
            }
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
            customerDropDownList.Items.Clear();
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
            customerDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();
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


        //if (customerDropDownList.SelectedValue == "")
        //{
        //    ShowMessageBox("Please Select Customer !!");
        //    return false;
        //}

        if (fromDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select To date !!");
            return false;
        }



        return true;
    }

    private void PopUpCustomerPaymentReport()
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        string fromDate = fromDateTextBox.Text;
        string toDate = toDateTextBox.Text;

        string customer = "";

        if (custIdHiddenField.Value != "")
        {
            customer = custIdHiddenField.Value;
        }
        else
        {
            customer = "";
        }

        string url = "../SInventory_RPTVIEW/CustomerWiseLedgerReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&customer=" + customer;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        custIdHiddenField.Value = "";
    }

    private String GetPrameterList()
    {
        string parameter = "";

        //if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        //{
        //    parameter = parameter + " WHERE INV.DeliveryInvoiceStatus IN ('Reject','Partial') AND INV.InvoiceDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AND ' " + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "'";
        //}

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        if (dcDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND UNT.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
        }

        //if (customerDropDownList.SelectedValue != "")
        //{
        //    parameter = parameter + " AND INV.CustomerMasterId = '" + customerDropDownList.SelectedValue + "'";
        //}

       
        return parameter;

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
        LoadDropdownList();
        dcDropDownList.Items.Clear();
        customerDropDownList.Items.Clear();
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
        custIdHiddenField.Value = "";
    }

    protected void dcDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (dcDropDownList.SelectedValue != "" && companyNameDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCustomerInfoInfo(customerDropDownList, dcDropDownList.SelectedValue, companyNameDropDownList.SelectedValue);
        }
        else
        {
            customerDropDownList.Items.Clear();
            
        }
    }
}