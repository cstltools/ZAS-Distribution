using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_WingsWiseSalesReport : System.Web.UI.Page
{
    private readonly WHStockValuationReprtDal aReportDal = new WHStockValuationReprtDal();

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

        // default group = 1 (if exists)
        if (groupDropDownList.Items.FindByValue("1") != null)
            groupDropDownList.SelectedValue = "1";

        if (!string.IsNullOrEmpty(groupDropDownList.SelectedValue))
        {
            aReportDal.LoadCompanyInfo(ddlCompany, groupDropDownList.SelectedValue);
        }
        else
        {
            ddlCompany.Items.Clear();
        }

        // Head user default company set
        if (Session["IsHead"] != null && Session["IsHead"].ToString() == "True")
        {
            DataTable aTable = aReportDal.GetUserCompanyById(Convert.ToString(Session["UserId"]));
            if (aTable.Rows.Count > 0)
            {
                ddlCompany.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId")
                    .ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    protected void groupDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        // If group changes, reload companies
        ddlCompany.Items.Clear();

        if (!string.IsNullOrEmpty(groupDropDownList.SelectedValue))
        {
            aReportDal.LoadCompanyInfo(ddlCompany, groupDropDownList.SelectedValue);
        }
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            PopUpWHStockValuationReport();
        }
    }

    private bool Validation()
    {
        if (string.IsNullOrEmpty(groupDropDownList.SelectedValue))
        {
            ShowMessageBox("Please Select Group Name !!");
            return false;
        }

        // Company optional? If you want mandatory, uncomment this block
        //if (string.IsNullOrEmpty(ddlCompany.SelectedValue))
        //{
        //    ShowMessageBox("Please Select Company !!");
        //    return false;
        //}

        if (string.IsNullOrWhiteSpace(fromDateTextBox.Text))
        {
            ShowMessageBox("Please Select From Date !!");
            return false;
        }

        if (string.IsNullOrWhiteSpace(toDateTextBox.Text))
        {
            ShowMessageBox("Please Select To Date !!");
            return false;
        }

        DateTime fd, td;
        if (!DateTime.TryParse(fromDateTextBox.Text.Trim(), out fd))
        {
            ShowMessageBox("Invalid From Date !!");
            return false;
        }

        if (!DateTime.TryParse(toDateTextBox.Text.Trim(), out td))
        {
            ShowMessageBox("Invalid To Date !!");
            return false;
        }

        if (fd > td)
        {
            ShowMessageBox("From Date cannot be greater than To Date !!");
            return false;
        }

        return true;
    }

    private void PopUpWHStockValuationReport()
    {
        // reset
        Session["ComId"] = null;
        Session["FDate"] = null;
        Session["TDate"] = null;
        Session["ProductId"] = null;
        Session["CompanyName"] = null;
        Session["ProductName"] = null;
        Session["WarehouseName"] = null;

        // Company (optional)
        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            Session["ComId"] = ddlCompany.SelectedValue;
            Session["CompanyName"] = ddlCompany.SelectedItem != null ? ddlCompany.SelectedItem.Text : "";
        }
        else
        {
            Session["ComId"] = ""; // keep blank if your report expects blank for "All"
            Session["CompanyName"] = "All";
        }

        // Date range
        Session["FDate"] = fromDateTextBox.Text.Trim();
        Session["TDate"] = toDateTextBox.Text.Trim();

        // Product (optional) - add dropdown in UI later if needed
        // Example if you add ddlProduct:
        // if (!string.IsNullOrEmpty(ddlProduct.SelectedValue))
        // {
        //     Session["ProductId"] = ddlProduct.SelectedValue;
        //     Session["ProductName"] = ddlProduct.SelectedItem.Text;
        // }
        // else
        // {
        //     Session["ProductId"] = "";
        //     Session["ProductName"] = "All";
        // }

        // Warehouse (optional) - add dropdown in UI later if needed
        // Session["WarehouseName"] = ddlWarehouse.SelectedItem.Text;

        string url = "../HTMLReport/WHStockValuationHtmlReport.aspx";
        string fullURL =
            "var Mleft=(screen.width/2)-(950/2);var Mtop=(screen.height/2)-(700/2);" +
            "window.open('" + url +
            "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no,scrollbars=yes,menubar=no,location=no,top='+Mtop+',left='+Mleft);";

        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private void ShowMessageBox(string message)
    {
        message = (message ?? "").Replace("'", "\\'");
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
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";

        // If you add product/warehouse dropdowns later, clear them here too
        // ddlProduct.Items.Clear();
        // ddlWarehouse.Items.Clear();
    }
}
