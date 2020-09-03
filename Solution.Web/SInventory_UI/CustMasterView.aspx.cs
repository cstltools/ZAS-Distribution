using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_CustMasterView : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    CustomerMasterBLL aCustomerMasterBLL = new CustomerMasterBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
        aCustomerMasterBLL.LoadRegionDropdownList(regionDropDownList);
    }

    private void CustomerMasterLoad()
    {


        string parameter = "";

        parameter = GenerateParameter();

        if (parameter != "")
        {
            aDataTable = aCustomerMasterBLL.LoadCustomerMaster(parameter);
            loadGridView.DataSource = aDataTable;
            loadGridView.DataBind();
        }

       
    }

    private string GenerateParameter()
    {
        string parameter = "WHERE";

        if (custcodenameTextBox.Text != "")
        {
            parameter = parameter + "  tblCustMaster.CustomerCode =  '" + custcodenameTextBox.Text.Trim() + "' AND ";
        }

        if (customerNameTextBox.Text != "")
        {
            parameter = parameter + "  tblCustMaster.CustomerName =  '" + customerNameTextBox.Text.Trim() + "' AND ";
        }

        if (regionDropDownList.SelectedValue != "")
        {
            parameter = parameter + "  tblRegion.RegionId =  '" + regionDropDownList.SelectedValue + "' AND ";
        }

        if (areaDropDownList.SelectedValue != "")
        {
            parameter = parameter + "  tblArea.AreaId =  '" + areaDropDownList.SelectedValue + "' AND ";
        }

        if (territoryDropDownList.SelectedValue != "")
        {
            parameter = parameter + "  tblTerritory.TerritoryId =  '" + territoryDropDownList.SelectedValue + "' AND ";
        }

        if (marketDropDownList.SelectedValue != "")
        {
            parameter = parameter + "  tblMarket.MarketId =  '" + marketDropDownList.SelectedValue + "' AND ";
        }

        if (paymentTypeDropDownList.SelectedValue != "")
        {
            parameter = parameter + " tblCustMaster.TermOfPayment =  '" + paymentTypeDropDownList.SelectedItem.Text + "' AND ";
        }

        string finalPrm = null;

        if (parameter != "")
        {
            finalPrm = parameter.Remove(parameter.Length - 5);
            //finalPrm = parameter.Substring(parameter.Length - 3);
        }

        return finalPrm;
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string customermasterid = loadGridView.DataKeys[rowindex][0].ToString();
            PopUp(customermasterid);
        }
        if (e.CommandName == "viewData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string customermasterid = loadGridView.DataKeys[rowindex][0].ToString();
            PopUp(customermasterid);
        }
    }

    private void PopUp(string Id)
    {
        string url = "CustMasterEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void CustMasterNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CustMasterEntry.aspx");
    }
    protected void CustMasterReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        CustomerMasterLoad();
    }

    protected void rptImageButton_Click(object sender, ImageClickEventArgs e)
    {
        string url = "../SInventory_RPTVIEW/CustomerMasterViewer.aspx";
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        CustomerMasterLoad();
    }

    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            aCustomerMasterBLL.LoadAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
        }
    }


    protected void territoryDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (territoryDropDownList.SelectedValue != "")
        {
            aCustomerMasterBLL.LoadMarketOnDropdownList(marketDropDownList, territoryDropDownList.SelectedValue);
        }
        else
        {
            marketDropDownList.Items.Clear();
        }
    }

    protected void areaDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (areaDropDownList.SelectedValue != "")
        {
            aCustomerMasterBLL.LoadTerritoryOnDropdownList(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
        }
    }

    protected void customerNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if(customerNameTextBox.Text != "")
        {
            string customerName = customerNameTextBox.Text.Trim();
            string[] productItem = customerName.Split('(');
            customerNameTextBox.Text = productItem[0];
        }
    }


    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        custcodenameTextBox.Text = "";
        customerNameTextBox.Text = "";

        regionDropDownList.SelectedValue = "";
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();

        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }
}