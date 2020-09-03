using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_MioWiseProductSalesReport : System.Web.UI.Page
{
    MioWiseProductSalesDal aReportDal = new MioWiseProductSalesDal();

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
            mioDropDownList.Items.Clear();
        }


    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        mioDropDownList.Items.Clear();
        dcDropDownList.Items.Clear();

        if (companyNameDropDownList.SelectedValue != "")
        {

            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                   // aReportDal.LoadMIOInformationByUser(mioDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                    aReportDal.GetWhInformationByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadMIOInformation(mioDropDownList, companyNameDropDownList.SelectedValue);
                    aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            } 
            
        }
        else
        {
            mioDropDownList.Items.Clear();
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
        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserType"].ToString() != "Admin")
            {
                if (companyNameDropDownList.SelectedValue == "")
                {
                    ShowMessageBox("Please Select company !!");
                    return false;
                }

                if (dcDropDownList.SelectedValue == "")
                {
                    ShowMessageBox("Please Select DC !!");
                    return false;
                }
            }
        }

        if (fromDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select  from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select  To date !!");
            return false;
        }

        return true;
    }

    private void PopUpCustomerPaymentReport()
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        Session["FromDate"] = "";
        Session["FromDate"] = fromDateTextBox.Text.Trim();

        Session["ToDate"] = "";
        Session["ToDate"] = toDateTextBox.Text.Trim();

        const string url = "../SInventory_RPTVIEW/MioWiseProductSalesReportViewer.aspx";
        const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            parameter = parameter + " WHERE DeliveryInvoiceStatus IN ('Full','Partial') AND INV.InvoiceDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AND ' " + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "'";
        }

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        if (dcDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND INV.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
        }

        if (mioDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND INV.MiaId = '" + mioDropDownList.SelectedValue + "'";
        }

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
        mioDropDownList.Items.Clear();
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
    }

    protected void dcDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        mioDropDownList.Items.Clear();

        if (dcDropDownList.SelectedValue != "")
        {

            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aReportDal.LoadMIOInformationByUserDC(mioDropDownList, dcDropDownList.SelectedValue, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadMIOInformationByUserDC(mioDropDownList, dcDropDownList.SelectedValue, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }


        }
        else
        {
            mioDropDownList.Items.Clear();
        }
    }

    
}