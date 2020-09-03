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

public partial class SInventory_UI_MarketInfoEdit : System.Web.UI.Page
{
    MarketInfoBLL MarketInfoBLL = new MarketInfoBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            marketInfoIdHiddenField.Value = Request.QueryString["ID"];
            MarketInfoLoad(marketInfoIdHiddenField.Value);           
        }

    }

    private void LoadDropdownList()
    {
        MarketInfoBLL.LoadRegionDropdownList(regionDropDownList);
    }
    

    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (territoryDropDownList.SelectedValue != "" && marketnameTextBox.Text != "" && marketCodeTextBox.Text != "")
        {
            var MarketInfo = new MarketInfo()
            {
                MarketId = Convert.ToInt32(marketInfoIdHiddenField.Value),
                MarketName = marketnameTextBox.Text.Trim(),
                MarketCode = marketCodeTextBox.Text.Trim(),
                MktShortName = shortNameTextBox.Text.Trim(),
                TerritoryId = Convert.ToInt32(territoryDropDownList.SelectedValue),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now

            };

            MarketInfoBLL aMarketInfoBLL = new MarketInfoBLL();

            if (!aMarketInfoBLL.UpdateDataForMarketInfo(MarketInfo))
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

    private void MarketInfoLoad(string marketId)
    {
        MarketInfo MarketInfo = new MarketInfo();

        MarketInfo = MarketInfoBLL.MarketInfoEditLoad(marketId);


        regionDropDownList.SelectedValue = MarketInfo.RegionId.ToString(CultureInfo.InvariantCulture);

        if (regionDropDownList.SelectedValue != "")
        {
            MarketInfoBLL.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
            areaDropDownList.SelectedValue = MarketInfo.AreaId.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            areaDropDownList.Items.Clear();
        }


        if (areaDropDownList.SelectedValue != "")
        {
            MarketInfoBLL.LoadTerritoryOnDropdownList(territoryDropDownList, areaDropDownList.SelectedValue);
            territoryDropDownList.SelectedValue = MarketInfo.TerritoryId.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            territoryDropDownList.Items.Clear();
        }

        marketnameTextBox.Text = MarketInfo.MarketName;
        marketCodeTextBox.Text = MarketInfo.MarketCode;
        shortNameTextBox.Text = MarketInfo.MktShortName;
        remarksTextBox.Text = MarketInfo.Remarks;
        descriptionTextBox.Text = MarketInfo.Description;
        isActiveCheckBox.Checked = MarketInfo.IsActive;

        codeHiddenField.Value = MarketInfo.MarketCode;
        nameHiddenField.Value = MarketInfo.MarketName;

    }


    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            MarketInfoBLL.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
        }
    }

    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (areaDropDownList.SelectedValue != "")
        {
            MarketInfoBLL.LoadTerritoryOnDropdownList(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
        }
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

    protected void marketnameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = MarketInfoBLL.NameExistOrNot(marketnameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, marketnameTextBox,nameHiddenField ,"Market Name already Exist !!!");

    }

    protected void marketCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = MarketInfoBLL.CodeExistOrNot(marketCodeTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, marketCodeTextBox,codeHiddenField ,"Market Code already Exist !!!");
    }

    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("MarketId") != Convert.ToInt32(marketInfoIdHiddenField.Value))
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

    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = MarketInfoBLL.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, shortNameTextBox, shortNameHiddenField, "Market Short Name already Exist !!!");
    }
}