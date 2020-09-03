using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_ProUnitPriceView : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    UnitPriceBLL aUnitPriceBLL = new UnitPriceBLL();
    WingsSalesAndCollectionReportBll andCollectionReportBll = new WingsSalesAndCollectionReportBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
            UnitPriceLoad();
        }
    }

    private void LoadDropDownList()
    {
        andCollectionReportBll.LoadGroupDropdownList(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);

        companyNameDropDownList.Items.Clear();

        if (groupDropDownList.SelectedValue != "")
        {
            andCollectionReportBll.LoadCompanyDropdownList(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
        }
    }

    private void UnitPriceLoad()
    {
        aDataTable = aUnitPriceBLL.LoadProductUnitPriceInfo(GetPrameterList());
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + "AND tblProduct.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        if (productCodeTextBox.Text != "")
        {
            parameter = parameter + "AND tblProduct.ProductCode = '" + productCodeTextBox.Text.Trim() + "'";
        }

        return parameter;

    }

    private void PopUp(string Id)
    {
        string url = "ProUnitPriceEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string unitPriceId = loadGridView.DataKeys[rowindex][0].ToString();
            Session["UnitPrice"] = unitPriceId;
            Response.Redirect("ProUnitPriceEntry.aspx");
        }

    }
    protected void unitPriceNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProUnitPriceEntry.aspx");
    }
    protected void unitPriceReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        UnitPriceLoad();
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        UnitPriceLoad();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        companyNameDropDownList.SelectedValue = "";
        productCodeTextBox.Text = "";
        UnitPriceLoad();
    }
}