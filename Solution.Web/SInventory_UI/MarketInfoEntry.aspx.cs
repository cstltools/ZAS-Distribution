using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.ReportSource;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_MarketInfoEntry : System.Web.UI.Page
{
    MarketInfoBLL aMarketInfoBll = new MarketInfoBLL();

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
        aMarketInfoBll.LoadRegionDropdownList(regionDropDownList);
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
        marketnameTextBox.Text = string.Empty;
        regionDropDownList.SelectedValue = "";
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketCodeTextBox.Text = string.Empty;
        descriptionTextBox.Text = "";
        remarksTextBox.Text = "";
        shortNameTextBox.Text = "";
        SetActiveStatus();
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

   
    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (territoryDropDownList.SelectedValue != "" && marketnameTextBox.Text != "" && marketCodeTextBox.Text != "")
        {
            var aMarketInfo = new MarketInfo()
            {
                MarketName = marketnameTextBox.Text.Trim(),
                MarketCode = marketCodeTextBox.Text.Trim(),
                MktShortName = shortNameTextBox.Text.Trim(),
                TerritoryId = Convert.ToInt32(territoryDropDownList.SelectedValue),
                Description = descriptionTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                IsActive = isActiveCheckBox.Checked,
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now
            };

            var aInfoBll = new MarketInfoBLL();

            ShowMessageBox(aInfoBll.SaveMarket(aMarketInfo));
            Clear();
        }
        else
        {
            ShowMessageBox("Please input data in all Textbox");
        }
    }
    protected void areaImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("MarketInfoView.aspx");
    }


    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            aMarketInfoBll.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
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
            aMarketInfoBll.LoadTerritoryOnDropdownList(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
        }
    }

    protected void marketnameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aMarketInfoBll.NameExistOrNot(marketnameTextBox.Text.Trim());
        GotoPermissionSetUp(info, marketnameTextBox, "Market Name already Exist !!!");

    }

    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            ShowMessageBox(msg);
        }
    }

    protected void marketCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aMarketInfoBll.CodeExistOrNot(marketCodeTextBox.Text.Trim());
        GotoPermissionSetUp(info, marketCodeTextBox, "Market Code already Exist !!!");
    }

    protected void shortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aMarketInfoBll.ShortNameExistOrNot(shortNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, shortNameTextBox, "Market Short Name already Exist !!!");
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }
}