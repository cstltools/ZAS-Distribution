using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_RegionInformationEdit : System.Web.UI.Page
{
    RegionInfoBLL RegionInfoBLL = new RegionInfoBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            regionInfoIdHiddenField.Value = Request.QueryString["ID"];
            RegionInfoLoad(regionInfoIdHiddenField.Value);

        }

    }


    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (regionnameTextBox.Text != "" && regioncodeTextBox.Text != "")
        {
            var RegionInfo = new RegionInfo()
            {
                RegionId = Convert.ToInt32(regionInfoIdHiddenField.Value),
                RegionName = regionnameTextBox.Text.Trim(),
                RegionCode = regioncodeTextBox.Text.Trim(),
                RgnShortName = shortNameTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now

            };


            if (!RegionInfoBLL.UpdateDataForRegionInfo(RegionInfo))
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

    private void RegionInfoLoad(string marketId)
    {
        var RegionInfo = new RegionInfo();

        RegionInfo = RegionInfoBLL.RegionInfoEditLoad(marketId);

        regionnameTextBox.Text = RegionInfo.RegionName;
        regioncodeTextBox.Text = RegionInfo.RegionCode;
        shortNameTextBox.Text = RegionInfo.RgnShortName;
        remarksTextBox.Text = RegionInfo.Remarks;
        descriptionTextBox.Text = RegionInfo.Description;
        isActiveCheckBox.Checked = RegionInfo.IsActive;

        codeHiddenField.Value = RegionInfo.RegionCode;
        nameHiddenField.Value = RegionInfo.RegionName;
        shortNameHiddenField.Value = RegionInfo.RgnShortName;

    }


    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);

    }

    protected void regioncodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = RegionInfoBLL.CheckRegionCodeExistOrNot(regioncodeTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, regioncodeTextBox, codeHiddenField, "Region Code already Exist !!!");

    }

    protected void regionnameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = RegionInfoBLL.NameExistOrNot(regionnameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, regionnameTextBox, nameHiddenField, "Region Name already Exist !!!");

    }


    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = RegionInfoBLL.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, shortNameTextBox,shortNameHiddenField ,"Region Short Name already Exist !!!");
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
                if (aTable.Rows[i].Field<Int32>("RegionId") != Convert.ToInt32(regionInfoIdHiddenField.Value))
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