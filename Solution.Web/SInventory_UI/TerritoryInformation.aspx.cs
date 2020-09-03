using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_TerritoryInformation : System.Web.UI.Page
{
    TerritoryInfoBll areaBll = new TerritoryInfoBll();
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
        regionDropDownList.SelectedValue = "";
        areaDropDownList.Items.Clear();
        territoryNameTextBox.Text = string.Empty;
        territoryCodeTextBox.Text = string.Empty;
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
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (territoryNameTextBox.Text != "" && areaDropDownList.SelectedValue != "" && territoryCodeTextBox.Text != "")
        {
            var territoryInfo = new TerritoryInfoDao()
            {
                TerritoryCode = territoryCodeTextBox.Text.Trim(),
                TerritoryName = territoryNameTextBox.Text.Trim(),
                AreaId = Convert.ToInt32(areaDropDownList.SelectedValue),
                TerShortName = shortNameTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now
            };

            string msg = areaBll.SaveArea(territoryInfo);
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
        Response.Redirect("TerritoryInformationView.aspx");
    }

    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            areaBll.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
        }
    }

    protected void territoryCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBll.CodeExistOrNot(territoryCodeTextBox.Text.Trim());
        GotoPermissionSetUp(info, territoryCodeTextBox, "Territory Code already Exist !!!");
    }

    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            ShowMessageBox(msg);
        }
    }


    protected void territoryNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBll.NameExistOrNot(territoryNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, territoryNameTextBox, "Territory Name already Exist !!!");
    }

    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBll.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, shortNameTextBox, "Territory Short Name already Exist !!!");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }
}