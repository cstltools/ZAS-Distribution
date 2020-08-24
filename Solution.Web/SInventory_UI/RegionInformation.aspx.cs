using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_RegionInformation : System.Web.UI.Page
{
    RegionInfoBLL RegionInfoBLL = new RegionInfoBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetActiveStatus();
        }
    }

    private void SetActiveStatus()
    {
        if (!isActiveCheckBox.Checked)
        {
            isActiveCheckBox.Checked = true;
        }
    }

    private void Clear()
    {
        regionnameTextBox.Text = string.Empty;
        regioncodeTextBox.Text = string.Empty;
        remarksTextBox.Text = "";
        descriptionTextBox.Text = "";
        shortNameTextBox.Text = "";
        SetActiveStatus();
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    
    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (regionnameTextBox.Text != "" && regioncodeTextBox.Text != "")
        {       
            var RegionInfo = new RegionInfo()
            {
                //CompanyName = companyNameDropDownList.SelectedItem.Text,
                //CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),

                RegionName = regionnameTextBox.Text.Trim(),
                RegionCode = regioncodeTextBox.Text.Trim(),
                RgnShortName = shortNameTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now

            };

            string message = RegionInfoBLL.SaveRegion(RegionInfo);
            ShowMessageBox(message);
            Clear();
        }
        else
        {
            ShowMessageBox("Please input data in all Textbox");
        }
    }

    protected void regionViewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("RegionInformationViw.aspx");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void regioncodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = RegionInfoBLL.CheckRegionCodeExistOrNot(regioncodeTextBox.Text.Trim());
        GotoPermissionSetUp(info, regioncodeTextBox, "Region Code already Exist !!!");

    }


    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            ShowMessageBox(msg);
        }
    }


    protected void regionnameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = RegionInfoBLL.NameExistOrNot(regionnameTextBox.Text.Trim());
        GotoPermissionSetUp(info, regionnameTextBox, "Region Name already Exist !!!");

    }


    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = RegionInfoBLL.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, shortNameTextBox, "Region Short Name already Exist !!!");
    }

    
}