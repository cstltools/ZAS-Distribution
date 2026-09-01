using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_WingsWiseSalesReport : System.Web.UI.Page
{
    SalesSummeryReportForAcc aReportDal = new SalesSummeryReportForAcc();

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
            aReportDal.LoadCompanyInfo(ddlCompany, groupDropDownList.SelectedValue);
        }
        else
        {
            ddlCompany.Items.Clear();
            dcDropDownList.Items.Clear();
            
        }


        if (Session["IsHead"].ToString() == "True")
        {
            DataTable aTable = aReportDal.GetUserCompanyById(Session["UserId"].ToString());

            if (aTable.Rows.Count > 0)
            {
                ddlCompany.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);
                ddlCompany_SelectedIndexChanged(null, null);
            }
        }

        

        //if (Session["UserType"].ToString() != "Admin")
        //{
        //    ddlCompany.Enabled = false;
        //}
        //else
        //{
        //    ddlCompany.Enabled = true;
        //}

    }



    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {

        string userType = Session["UserId"].ToString();

        if (ddlCompany.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")  
                {
                    aReportDal.LoadDcInfoByUser(dcDropDownList, ddlCompany.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadDcInfo(dcDropDownList, ddlCompany.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            } 
            
        }
        else
        {
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
       
        if (groupDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select group Name !!");
            return false;
        }


        string var = Session["IsHead"].ToString();

        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserType"].ToString() != "Admin" && Session["IsHead"].ToString() != "True")
            {
                if (dcDropDownList.SelectedValue == "")
                {
                    ShowMessageBox("Please Select DC !!");
                    return false;
                }
            }
        }


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
        Session["ComId"] = "";
        Session["UnitId"] = "";
        Session["FDate"] = "";
        Session["TDate"] = "";

        if (ddlCompany.SelectedValue != "")
        {
            Session["ComId"] = ddlCompany.SelectedValue;
        }

       
        if (dcDropDownList.SelectedValue != "")
        {
            Session["UnitId"] = dcDropDownList.SelectedValue;
        }

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            Session["FDate"] = fromDateTextBox.Text.Trim();
            Session["TDate"] = toDateTextBox.Text.Trim();
        }

        

        //Session["PRAM"] = GetPrameterList();

        string url = "../HTMLReport/SalesSummeryForAccHtmlReport.aspx";
        string fullURL =
            "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
            "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
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

    private void Clear()
    {
        groupDropDownList.SelectedValue = "";
        ddlCompany.Items.Clear();
        dcDropDownList.Items.Clear();
        
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
    }
}