using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_DistributionCenterInformation : System.Web.UI.Page
{
    DistributionCeneterInfoBll aCompanyUnitBll = new DistributionCeneterInfoBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
        }
    }

    private void LoadDropDown()
    {
        aCompanyUnitBll.LoadCompanyDropdownList(companyNameDropDownList);
        aCompanyUnitBll.LoadDcType(ddlDcType);

    }


    private void Clear()
    {
        salesCenternameTextBox.Text = string.Empty;
        addressTextBox.Text = string.Empty;
        phoneNoTextBox.Text = string.Empty;
        mobileNoTextBox.Text = string.Empty;
        faxNoTextBox.Text = string.Empty;
        LoadDropDown();
        dcShortNameTextBox.Text = string.Empty;

    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {

        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please select a company!!");
            return false;
        }

        if (ddlDcType.SelectedValue == "")
        {
            ShowMessageBox("You should select DC Type !!");
            return false;
        }

        if (salesCenternameTextBox.Text == "")
        {
            ShowMessageBox("Please Input DC Name!!");
            return false;
        }

        if (addressTextBox.Text == "")
        {
            ShowMessageBox("Please Input Address!!");
            return false;
        }

        return true;
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            var aCompanyUnit = new CompanyUnit()
            {
                //ComUnitCode = comUnitCodeTextBox.Text,
                ComUnitName = salesCenternameTextBox.Text,
                ShortName = dcShortNameTextBox.Text,
                Address = addressTextBox.Text,
                PhoneNo = phoneNoTextBox.Text,
                MobileNo = mobileNoTextBox.Text,
                FaxNo = faxNoTextBox.Text,
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                CashGlId = ddlCashAccount.SelectedValue != "" ? Convert.ToInt32(ddlCashAccount.SelectedValue) : 0,
                DcTypeId = Convert.ToInt32(ddlDcType.SelectedValue),
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now
            };

            if (aCompanyUnitBll.SaveSalesCenter(aCompanyUnit))
            {
                ShowMessageBox("Data Save Successfully  CompanyUnit Code  is :  " + aCompanyUnit.ComUnitCode +
                               " And  CompanyUnit Name is :" + aCompanyUnit.ComUnitName);
                Clear();
            }
            else
            {
                ShowMessageBox("DC Code already exist!!!");
            }
        }
        else
        {
            ShowMessageBox("Please input data in all Textbox");
        }
    }

    protected void unitViewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("DistributionCenterInfoView.aspx");
    }

    protected void salesCenternameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dcInfo = aCompanyUnitBll.CheckDCInfoExistOrNot(salesCenternameTextBox.Text.Trim());
        GotoPermissionSetUp(dcInfo, salesCenternameTextBox, "DC Name already Exist !!!");
    }


    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            ShowMessageBox(msg);
        }
    }


    protected void dcShortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aCompanyUnitBll.CheckDCShortNameExistOrNot(dcShortNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, dcShortNameTextBox, "DC short Name already Exist !!!");
    }


    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            aCompanyUnitBll.LoadCashAccount(ddlCashAccount, Convert.ToInt32(companyNameDropDownList.SelectedValue));
        }

        
    }
}