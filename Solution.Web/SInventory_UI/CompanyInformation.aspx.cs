using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CompanyInformation : System.Web.UI.Page
{
    CompanyInfoBLL aCompanyInfoBll = new CompanyInfoBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
        }
    }

    private void LoadDropDownList()
    {
        aCompanyInfoBll.LoadGroupDropdownList(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void Clear()
    {
        companynameTextBox.Text = string.Empty;
        cmpShortNameTextBox.Text = "";
        companyAddressTextBox.Text = string.Empty;
        contactTextBox.Text = string.Empty;
        faxNoTextBox.Text = string.Empty;
        remarksTextBox.Text = string.Empty;
    }
    private bool Validation()
    {
        if (companynameTextBox.Text == "")
        {
            ShowMessageBox("Please Input Company Name!!");
            return false;
        }

        //if (cmpShortNameTextBox.Text == "")
        //{
        //    ShowMessageBox("Please Input Company Short Name!!");
        //    return false;
        //}

        if (companyAddressTextBox.Text == "")
        {
            ShowMessageBox("Please Input Company Address!!");
            return false;
        }
        if (contactTextBox.Text == "")
        {
            ShowMessageBox("Please Input Company Contact Number!!");
            return false;
        }
        return true;
    }
    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            var aCompanyInfo = new CompanyInformation()
            {
                GroupId = Convert.ToInt32(groupDropDownList.SelectedValue),
                CompanyName = companynameTextBox.Text.Trim(),
                CmpShortName = cmpShortNameTextBox.Text.Trim(),
                Address = companyAddressTextBox.Text.Trim(),
                ContactNo = contactTextBox.Text.Trim(),
                FaxNo = faxNoTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now
            };

            if (aCompanyInfoBll.SaveCompanyInfoData(aCompanyInfo))
            {
                ShowMessageBox("Data Save Successfully Company Code  is :  " + aCompanyInfo.CompanyCode + " And CompanyName is :" + aCompanyInfo.CompanyName);
                Clear();
            }
        }
        else
        {
            ShowMessageBox("Company Code already exist");
        }
    }
    protected void CompanyListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CompanyInformationView.aspx");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void companynameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable companyInfo = aCompanyInfoBll.CheckCompanyInfoExistOrNot(companynameTextBox.Text.Trim());
        GotoPermissionSetUp(companyInfo, companynameTextBox,"Company Name already Exist !!!");

    }

    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            ShowMessageBox(msg);
        }
    }

    protected void cmpShortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable companyInfo = aCompanyInfoBll.CheckCompanyShortNameExistOrNot(cmpShortNameTextBox.Text.Trim());
        GotoPermissionSetUp(companyInfo, cmpShortNameTextBox, "Company short Name already Exist !!!");
    }

}