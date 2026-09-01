using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_SalesPerformanceReportProductWise : System.Web.UI.Page
{
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();
    SalesPerformanceReportProductWise_Dal aDal = new SalesPerformanceReportProductWise_Dal();

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

        // Head user auto company set
        if (Session["IsHead"] != null && Session["IsHead"].ToString() == "True")
        {
            DataTable aTable = aReportDal.GetUserCompanyById(Session["UserId"].ToString());

            if (aTable != null && aTable.Rows.Count > 0)
            {
                ddlCompany.SelectedValue =
                    aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);

                ddlCompany_SelectedIndexChanged(null, null);
            }
        }
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue != "")
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "")
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

    private bool Validation()
    {
        if (groupDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select group Name !!");
            return false;
        }

        if (Session["UserType"] != null && Session["UserType"].ToString() != "")
        {
            // Only non-admin AND non-head must select DC
            if (Session["UserType"].ToString() != "Admin" &&
                (Session["IsHead"] == null || Session["IsHead"].ToString() != "True"))
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
            ShowMessageBox("Please Select invoice from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select invoice To date !!");
            return false;
        }

        // Date format check
        DateTime fromDate, toDate;
        if (!DateTime.TryParseExact(fromDateTextBox.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out fromDate))
        {
            ShowMessageBox("Invalid From Date format. Use dd-MMM-yyyy.");
            return false;
        }

        if (!DateTime.TryParseExact(toDateTextBox.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out toDate))
        {
            ShowMessageBox("Invalid To Date format. Use dd-MMM-yyyy.");
            return false;
        }

        if (fromDate > toDate)
        {
            ShowMessageBox("From Date cannot be greater than To Date.");
            return false;
        }

        return true;
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (!Validation()) return;

        // ✅ CompanyId
        int companyId = Convert.ToInt32(ddlCompany.SelectedValue);

        // ✅ DC / ComUnitId handling:
        // - If Head user: allow blank => pass NULL (or 0)
        // - Admin: can choose, optional (you decide)
        // - Normal user: must choose (already validated)
        int? comUnitId = null;

        if (!string.IsNullOrEmpty(dcDropDownList.SelectedValue))
        {
            comUnitId = Convert.ToInt32(dcDropDownList.SelectedValue);
        }
        else
        {
            // keep NULL for head/admin if not selected
            comUnitId = null;
        }

        // ✅ Dates
        DateTime fromDate = DateTime.ParseExact(fromDateTextBox.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture);
        DateTime toDate = DateTime.ParseExact(toDateTextBox.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture);

        // ✅ Build report URL (open in new tab)
        // SalesPerformanceReport.aspx is your HTML report page
        // You will read these querystrings there and call SP.
        string url = "../HTMLReport/SalesPerformanceReport.aspx"
                     + "?CompanyId=" + HttpUtility.UrlEncode(companyId.ToString())
                     + "&ComUnitId=" + HttpUtility.UrlEncode(comUnitId.HasValue ? comUnitId.Value.ToString() : "")
                     + "&FromDate=" + HttpUtility.UrlEncode(fromDate.ToString("dd-MMM-yyyy"))
                     + "&ToDate=" + HttpUtility.UrlEncode(toDate.ToString("dd-MMM-yyyy"));

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "openReport",
            "window.open('" + url + "', '_blank');",
            true
        );
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("SalesPerformanceReportProductWise.aspx");
    }
}
