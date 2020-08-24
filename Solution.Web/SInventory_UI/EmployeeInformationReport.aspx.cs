using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_EmployeeInformationReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            LoadDropDown();
        }
    }
    private void LoadDropDown()
    {
        EmployeeInformationBLL aEmployeeInformationBLL = new EmployeeInformationBLL();
        aEmployeeInformationBLL.GroupDropDown(groupDropDownList);
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void reportTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
      
        if (reportTypeDropDownList.SelectedValue == "CompanyWise")
        {
            groupCompanyWise.Visible = true;
        }
        else 
        {
            groupCompanyWise.Visible = false;
        }

    }

    protected void groupDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeInformationBLL aEmployeeInformationBLL = new EmployeeInformationBLL();
        aEmployeeInformationBLL.CompanyNameDropDown(companyNameDropDownList);
       
    }

    protected void viewRptButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (reportTypeDropDownList.SelectedValue == "All")
            {
                PupupAllReport("crp");
            }

            if (reportTypeDropDownList.SelectedValue == "CompanyWise")
            {
                PupupAllReport("crp");
            }

            if (reportTypeDropDownList.SelectedValue == "DeliveryPerson")
            {
                PupupAllReport("crp");
            }
        }
    }

    protected void excelButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (reportTypeDropDownList.SelectedValue == "All")
            {
                PupupAllReport("exl");
            }

            if (reportTypeDropDownList.SelectedValue == "CompanyWise")
            {
                PupupAllReport("exl");
            }

            if (reportTypeDropDownList.SelectedValue == "DeliveryPerson")
            {
                PupupAllReport("exl");
            }
        }
    }

    private string GetParameterList()
    {
        string parameter = "";

        if (reportTypeDropDownList.SelectedValue != "")
        {
            if (reportTypeDropDownList.SelectedValue == "All")
            {
                parameter = "";
            }

            if (reportTypeDropDownList.SelectedValue == "CompanyWise")
            {
                if (companyNameDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " WHERE EGI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }
            }

            if (reportTypeDropDownList.SelectedValue == "DeliveryPerson")
            {
                parameter = "";
            }
        }

        return parameter;
    }

    private bool Validation()
    {
        if (reportTypeDropDownList.SelectedIndex == 0) 
        {
            showMessageBox("Please Select Report Type");
            return false;
        }

        if (reportTypeDropDownList.SelectedValue=="CompanyWise")
        {
            if (groupDropDownList.SelectedValue == "")
            {
                showMessageBox("Please Select Group Name");
                return false;
            }

            if (companyNameDropDownList.SelectedValue == "")
            {
                showMessageBox("Please Select Company Name");
                return false;
            }
        }
        return true;
    }

    private void PupupAllReport(string reportType)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetParameterList();
        string rptType = reportTypeDropDownList.SelectedValue;

        string url = "../SInventory_RPTVIEW/EmployeeInformationReportViewer.aspx?rptType=" + rptType + "&reportType=" + reportType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void clearButton_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        reportTypeDropDownList.SelectedIndex = 0;
        groupDropDownList.SelectedIndex = 0;
        companyNameDropDownList.SelectedIndex = 0;
    }
}