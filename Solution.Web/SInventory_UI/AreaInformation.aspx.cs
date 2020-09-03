using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_AreaInformation : System.Web.UI.Page
{
    AreaBLL areaBll = new AreaBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            SetActiveStatus();
        }
    }

    private void LoadDropdownList()
    {
        areaBll.LoadRegionDropdownList(regionDropDownList);
    }

    private void Clear()
    {
        areaNameTextBox.Text = string.Empty;
        areaCodeTextBox.Text = string.Empty;
        regionDropDownList.SelectedValue = "";
        remarksTextBox.Text = "";
        descriptionTextBox.Text = "";
        shortNameTextBox.Text = "";
        SetActiveStatus();

    }

    private void SetActiveStatus()
    {
        if (!isActiveCheckBox.Checked)
        {
            isActiveCheckBox.Checked = true;
        }
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
        if (regionDropDownList.SelectedValue != "" && areaNameTextBox.Text != "" && areaCodeTextBox.Text != "")
        {
            var areaInfo = new AreaInfo()
            {
                AreaCode = areaCodeTextBox.Text.Trim(),
                AreaName = areaNameTextBox.Text.Trim(),
                RegionId = Convert.ToInt32(regionDropDownList.SelectedValue),
                AreaShortName = shortNameTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now
            };

            string msg = areaBll.SaveArea(areaInfo);
            ShowMessageBox(msg);
            Clear();
        }
        else
        {
            ShowMessageBox("Please input data in all Textbox");
        }
    }

    protected void areaImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("AreaInformationView.aspx");
    }

    protected void areaCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBll.CodeExistOrNot(areaCodeTextBox.Text.Trim());
        GotoPermissionSetUp(info, areaCodeTextBox, "Area Code already Exist !!!");
    }


    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            ShowMessageBox(msg);
        }
    }

    protected void areaNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBll.NameExistOrNot(areaNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, areaNameTextBox, "Area Name already Exist !!!");
    }

    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBll.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, shortNameTextBox, "Area Short Name already Exist !!!");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }
}