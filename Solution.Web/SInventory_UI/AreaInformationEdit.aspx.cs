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

public partial class SInventory_UI_AreaInformationEdit : System.Web.UI.Page
{
    AreaBLL areaBLL = new AreaBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            areaInfoIdHiddenField.Value = Request.QueryString["ID"];
            AreaInfoLoad(areaInfoIdHiddenField.Value);
        }
    }

    private void LoadDropdownList()
    {
        areaBLL.LoadRegionDropdownList(regionDropDownList);
    }

    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "" && areaNameTextBox.Text != "" && shortNameTextBox.Text != "" && areaCodeTextBox.Text != "")
        {
            var areaInfo = new AreaInfo()
            {
                AreaId = Convert.ToInt32(areaInfoIdHiddenField.Value),
                AreaCode = areaCodeTextBox.Text,
                AreaName = areaNameTextBox.Text,
                RegionId = Convert.ToInt32(regionDropDownList.SelectedValue),
                AreaShortName = shortNameTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now
            };


            if (!areaBLL.UpdateDataForAreaInfo(areaInfo))
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
        var areaInfo = new AreaInfo();

        areaInfo = areaBLL.AreaInfoEditLoad(areaId);

        regionDropDownList.SelectedValue = areaInfo.RegionId.ToString(CultureInfo.InvariantCulture);
        areaNameTextBox.Text = areaInfo.AreaName;
        areaCodeTextBox.Text = areaInfo.AreaCode;
        shortNameTextBox.Text = areaInfo.AreaShortName;
        remarksTextBox.Text = areaInfo.Remarks;
        descriptionTextBox.Text = areaInfo.Description;
        isActiveCheckBox.Checked = areaInfo.IsActive;

        codeHiddenField.Value = areaInfo.AreaCode;
        nameHiddenField.Value = areaInfo.AreaName;
        shortNameHiddenField.Value = areaInfo.AreaShortName;

    }

    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }

    protected void areaCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBLL.CodeExistOrNot(areaCodeTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, areaCodeTextBox,codeHiddenField ,"Area Code already Exist !!!");
    }

    protected void areaNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBLL.NameExistOrNot(areaNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, areaNameTextBox, nameHiddenField,"Area Name already Exist !!!");
    }

    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = areaBLL.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, shortNameTextBox,shortNameHiddenField ,"Area Short Name already Exist !!!");
    }



    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("AreaId") != Convert.ToInt32(areaInfoIdHiddenField.Value))
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