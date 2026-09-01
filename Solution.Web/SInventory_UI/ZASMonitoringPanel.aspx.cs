using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_ZASMonitoringPanel : System.Web.UI.Page
{

    ZASMonitoringDal aMonitoringDal = new ZASMonitoringDal();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            
        }

    }

    protected void viewRptButton_Click(object sender, EventArgs e)
    {

        loadStockGridView.DataSource = null;
        loadStockGridView.DataBind();

        loadAccountsIntegration.DataSource = null;
        loadAccountsIntegration.DataBind();

        if (ddlType.SelectedValue == "Stock")
        {
            DataTable aTable = aMonitoringDal.DCStock();

            if (aTable.Rows.Count > 0)
            {
                loadStockGridView.DataSource = aTable;
                loadStockGridView.DataBind();
            }

        }
        else
        {
            DataTable aTable = aMonitoringDal.AccountsIntegration();

            if (aTable.Rows.Count > 0)
            {
                loadAccountsIntegration.DataSource = aTable;
                loadAccountsIntegration.DataBind();
            }
        }

        
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("ZASMonitoringPanel.aspx");
    }
}