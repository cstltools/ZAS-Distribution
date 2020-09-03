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

public partial class SInventory_UI_TerritoryInformationEdit : System.Web.UI.Page
{
    TerritoryInfoBll areaBLL = new TerritoryInfoBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            territoryInfoIdHiddenField.Value = Request.QueryString["ID"];
            AreaInfoLoad(territoryInfoIdHiddenField.Value);
        }
    }

    private void LoadDropdownList()
    {
        areaBLL.LoadRegionDropdownList(regionDropDownList);
    }

    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            areaBLL.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
        }
    }

    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (territoryNameTextBox.Text != "" && areaDropDownList.SelectedValue != "" && territoryCodeTextBox.Text != "" )
        {
            var territoryInfo = new TerritoryInfoDao()
            {
                TerritoryId = Convert.ToInt32(territoryInfoIdHiddenField.Value),
                TerritoryCode = territoryCodeTextBox.Text.Trim(),
                TerritoryName = territoryNameTextBox.Text.Trim(),
                AreaId = Convert.ToInt32(areaDropDownList.SelectedValue),
                TerShortName = shortNameTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now
            };


            if (!areaBLL.UpdateDataForAreaInfo(territoryInfo))
            {
                MessageLabel.Text = "Data Not Update!!!";
                MessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                MessageLabel.Text = "Data Update Successfully!!! Please Reload";
                MessageLabel.ForeColor = System.Drawing.Color.Green;
            }
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }

    private void AreaInfoLoad(string areaId)
    {
        var areaInfo = new TerritoryInfoDao();

        areaInfo = areaBLL.AreaInfoEditLoad(areaId);

        regionDropDownList.SelectedValue = areaInfo.RegionId.ToString(CultureInfo.InvariantCulture);

        if (regionDropDownList.SelectedValue != "")
        {
            areaBLL.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
            areaDropDownList.SelectedValue = areaInfo.AreaId.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            areaDropDownList.Items.Clear();
        }
        
        territoryNameTextBox.Text = areaInfo.TerritoryName;
        territoryCodeTextBox.Text = areaInfo.TerritoryCode;
        shortNameTextBox.Text = areaInfo.TerShortName;
        remarksTextBox.Text = areaInfo.Remarks;
        descriptionTextBox.Text = areaInfo.Description;
        isActiveCheckBox.Checked = areaInfo.IsActive;

        codeHiddenField.Value = areaInfo.TerritoryCode;
        nameHiddenField.Value = areaInfo.TerritoryName;
        shortNameHiddenField.Value = areaInfo.TerShortName;

    }

    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void territoryCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBLL.CodeExistOrNot(territoryCodeTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, territoryCodeTextBox,codeHiddenField ,"Territory Code already Exist !!!");
    }

    protected void territoryNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBLL.NameExistOrNot(territoryNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, territoryNameTextBox,nameHiddenField ,"Territory Name already Exist !!!");
    }

    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBLL.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, shortNameTextBox, shortNameHiddenField,"Territory Short Name already Exist !!!");
    }

    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("TerritoryId") != Convert.ToInt32(territoryInfoIdHiddenField.Value))
                {
                    count++;
                }

                if (count > 0)
                {
                    textBox.Text = presentData.Value;
                    ShowMessageBox(msg);
                    break;
                }
            }

        }
    }
}