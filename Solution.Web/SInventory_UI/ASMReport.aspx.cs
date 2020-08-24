using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_ASMReport : System.Web.UI.Page
{
    SalesAdministrationReportBll administrationReport = new SalesAdministrationReportBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
        administrationReport.LoadCompanyDropdownList(companyNameDropDownList);
    }
    protected void clearButton_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        reportNameDropDownList.SelectedIndex = 0;
        companyNameDropDownList.SelectedIndex = 0;
    }

    protected void submitButton_OnClick(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (reportNameDropDownList.SelectedValue == "RSM")
            {
                PopupRSMReport("crp");
            }

            if (reportNameDropDownList.SelectedValue == "ASM")
            {
                PopupASMReport("crp");
            }

            if (reportNameDropDownList.SelectedValue == "MIO")
            {
                PopupMIOReport("crp");
            }

        
        }
    }

    protected void excelButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (reportNameDropDownList.SelectedValue == "RSM")
            {
                PopupRSMReport("exl");
            }

            if (reportNameDropDownList.SelectedValue == "ASM")
            {
                PopupASMReport("exl");
            }

            if (reportNameDropDownList.SelectedValue == "MIO")
            {
                PopupMIOReport("exl");
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

    private bool Validation()
    {
        if (reportNameDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Select Report Type");
            return false;
        }

        return true;
    }

    private void PopupRSMReport(string reportType)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();
        string rptType = reportNameDropDownList.SelectedValue;

        string url = "../SInventory_RPTVIEW/SalesAdministrationReportViewer.aspx?rptType=" + rptType + "&reportType=" + reportType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private void PopupASMReport(string reportType)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();
        string rptType = reportNameDropDownList.SelectedValue;

        string url = "../SInventory_RPTVIEW/SalesAdministrationReportViewer.aspx?rptType=" + rptType + "&reportType=" + reportType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private void PopupMIOReport(string reportType)
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();
        string rptType = reportNameDropDownList.SelectedValue;

        string url = "../SInventory_RPTVIEW/SalesAdministrationReportViewer.aspx?rptType=" + rptType + "&reportType=" + reportType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private string GetPrameterList()
    {
        string parameter = "";

        if (reportNameDropDownList.SelectedValue != "")
        {
            if (reportNameDropDownList.SelectedValue == "RSM")
            {
                if (companyNameDropDownList.SelectedValue != "")
                {
                    parameter = parameter + "AND dbo.tblRSMInfo.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }
            }

            if (reportNameDropDownList.SelectedValue == "ASM")
            {
                if (companyNameDropDownList.SelectedValue != "")
                {
                    parameter = parameter + "AND ASM.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }
            }


            if (reportNameDropDownList.SelectedValue == "MIO")
            {
                if (companyNameDropDownList.SelectedValue != "")
                {
                    parameter = parameter + "AND MIO.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }
            }

            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    parameter = parameter + " AND DCA.DCId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + Session["UserId"].ToString() + ")";
                }
            }

        }

        return parameter;
    }

    protected void reportNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (reportNameDropDownList.SelectedValue != "")
        {
            company.Visible = true;
        }
        else
        {
            company.Visible = false;
        }
    }

}