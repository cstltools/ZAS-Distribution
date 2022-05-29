using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_WHBeenCardReport : System.Web.UI.Page
{
    WHBeenCardReportDal aDal = new WHBeenCardReportDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
        }
    }

    private void LoadDropDownList()
    {
        aDal.GetCompanyDropdownList(companyNameDropDownList);
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            aDal.GetWhInformation(warehouseDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            warehouseDropDownList.Items.Clear();
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private void ReportPopUp()
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (fromDateTextBox.Text != "" && todateTextBox.Text != "")
            {
                var fromDate = Convert.ToDateTime(fromDateTextBox.Text.Trim());
                var toDate = Convert.ToDateTime(todateTextBox.Text.Trim());
                var comId = Convert.ToInt32(companyNameDropDownList.SelectedValue);
                var branchId = 0;

                var url = "../HTMLReport/NationalBeenCardHTMLReport.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&branchId=" + branchId + "&DC=" + companyNameDropDownList.SelectedItem.Text + "&comId=" + comId;
                var fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            else
            {
                ShowMessageBox("Please Select a date range!!");
            }
        }
        else
        {
            ShowMessageBox("Please Select a Company !!");
        }


    }

    protected void viewRptButton_Click(object sender, EventArgs e)
    {
        ReportPopUp();
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        ReportPopUp();
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        LoadDropDownList();
        fromDateTextBox.Text = "";
        todateTextBox.Text = "";
    }
}