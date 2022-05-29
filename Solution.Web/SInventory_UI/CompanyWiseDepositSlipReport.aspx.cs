using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CompanyWiseDepositSlipReport : System.Web.UI.Page
{
    DepositSlipEntryDal aDal = new DepositSlipEntryDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Loaddropdownlist();
        }
    }

    private void Loaddropdownlist()
    {
        aDal.LoadManufac(companyNameDropDownList);
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aDal.GetDcInformationByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());

                }
                else
                {
                    aDal.GetDCInformation(dcDropDownList, companyNameDropDownList.SelectedValue);
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

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Loaddropdownlist();
        dcDropDownList.Items.Clear();
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
    }

    protected void Button_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            PopUpDepositReport();
        }
    }

    private void PopUpDepositReport()
    {
        Session["ComId"] = "";
        Session["UnitId"] = "";
        Session["FDate"] = "";
        Session["TDate"] = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            Session["ComId"] = companyNameDropDownList.SelectedValue;
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


        string url = "../SInventory_RPTVIEW/WingsWiseDepositReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private bool Validation()
    {
        //if (companyNameDropDownList.SelectedValue == "")
        //{
        //    ShowMessageBox("Please select company !!");
        //    return false;
        //}

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
            ShowMessageBox("Please select from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please select to date !!");
            return false;
        }

        return true;
    }
}