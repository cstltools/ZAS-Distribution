using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.CrystalReports.SInventory_DS;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_AreaWiseMonthlyInventoryReport : System.Web.UI.Page
{

    AreaWiseMonthlyInventoryReportBll areaWise = new AreaWiseMonthlyInventoryReportBll();
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();

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
        }


        //OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        //aOrderInfoBll.LoadSC(branchDropDownList, Session["UserId"].ToString());
    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aReportDal.LoadDcInfoByUser(branchDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadDcInfo(branchDropDownList, companyNameDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }
        else
        {
            branchDropDownList.Items.Clear();
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
        if (branchDropDownList.SelectedValue != "")
        {
            if (fromDateTextBox.Text != "" && todateTextBox.Text != "")
            {
                var fromDate = Convert.ToDateTime(fromDateTextBox.Text.Trim());
                var toDate = Convert.ToDateTime(todateTextBox.Text.Trim());
                var comId = Convert.ToInt32(companyNameDropDownList.SelectedValue);
                var branchId = Convert.ToInt32(branchDropDownList.SelectedValue);

                var url = "../HTMLReport/DCStockHTMLReport.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&branchId=" + branchId + "&DC=" + branchDropDownList.SelectedItem.Text + "&comId=" + comId;
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
            ShowMessageBox("Please Select a DC !!");
        }

        
    }

    protected void viewRptButton_Click(object sender, EventArgs e)
    {
        ReportPopUp();
    }
}