using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_WHStockInReportList : System.Web.UI.Page
{
    WHStockInReportBll aStockInReportBll = new WHStockInReportBll();
    readonly WhStockConditionFreezeBll _aConditionFreezeBll = new WhStockConditionFreezeBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
        //_aConditionFreezeBll.LoadWhInfoOnDropDownList(whDropDownList);
        _aConditionFreezeBll.LoadCompanyDropdownList(companyNameDropDownList);
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            _aConditionFreezeBll.LoadWhInformation(warehouseDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            warehouseDropDownList.Items.Clear();
        }
    }


    protected void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void LoadGridView()
    {
        DataTable aDataTable = new DataTable();

        aDataTable = aStockInReportBll.LoadWarehouseStockInData(stockInDateTextBox.Text);
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadGridView();
    }

    protected void printButton_Click(object sender, EventArgs e)
    {
        int iRowIndex = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
        string reqId = loadGridView.DataKeys[iRowIndex][0].ToString();

        if (reportDropDownList.SelectedValue == "STD")
        {
            Session["ReportType"] = "";
            Session["ReportType"] = "STD";

            Session["Excel"] = "";
            Session["Excel"] = 0;

            string url = "../SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=" + reqId;
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL =
                "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
                "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof (string), "OPEN_WINDOW", fullURL, true);
        }
        
    }

    protected void reportDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (reportDropDownList.SelectedValue == "STD")
        {
            stockInDate.Visible = true;
            productName.Visible = false;

            reportButton.Visible = false;
            excelButton.Visible = false;

            searchButton.Visible = true;
            fromDate.Visible = false;
            toDate.Visible = false;
        }

        else if (reportDropDownList.SelectedValue == "CSR")
        {
            stockInDate.Visible = false;
            aStockInReportBll.LoadProductInformation(productDropDownList);
            productName.Visible = true;

            reportButton.Visible = true;
            excelButton.Visible = true;
            searchButton.Visible = false;

            fromDate.Visible = false;
            toDate.Visible = false;

            loadGridView.DataSource = null;
            loadGridView.DataBind(); 

        }
        else if (reportDropDownList.SelectedValue == "STI")
        {
            stockInDate.Visible = false;
            productName.Visible = false;
            
            fromDate.Visible = true;
            toDate.Visible = true;

            reportButton.Visible = true;
            excelButton.Visible = true;
            searchButton.Visible = false;

            loadGridView.DataSource = null;
            loadGridView.DataBind();

        }

        else
        {
            stockInDate.Visible = false;
            productName.Visible = false;

            reportButton.Visible = false;
            excelButton.Visible = false;
            searchButton.Visible = true;

            fromDate.Visible = false;
            toDate.Visible = false;

            loadGridView.DataSource = null;
            loadGridView.DataBind(); 
        }
    }

    protected void reportButton_OnClick(object sender, EventArgs e)
    {
        if (reportDropDownList.SelectedValue == "CSR")
        {
            Session["ReportType"] = "";
            Session["ReportType"] = "CSR";
            Session["ProductCode"] = "";
            Session["ProductCode"] = GstParameter();

            //if (productDropDownList.SelectedValue != string.Empty)
            //{
            //    Session["ProductId"] = Conv(productDropDownList.SelectedValue);
            //}

            Session["Excel"] = "";
            Session["Excel"] = 0;

            string url = "../SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=";
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL =
                "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
                "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        if (reportDropDownList.SelectedValue == "STI")
        {
            Session["ReportType"] = "";
            Session["ReportType"] = "STI";
            Session["PRAM"] = "";
            Session["PRAM"] = StiParameter();

            //if (productDropDownList.SelectedValue != string.Empty)
            //{
            //    Session["ProductId"] = Conv(productDropDownList.SelectedValue);
            //}

            Session["Excel"] = "";
            Session["Excel"] = 0;

            string url = "../SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=";
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL =
                "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
                "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }

    private String StiParameter()
    {
        string pram = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            pram = pram + " AND CI.CompanyId = " + Convert.ToInt32(companyNameDropDownList.SelectedValue);
        }

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            pram = pram + " AND WHStockInDate BETWEEN '" + fromDateTextBox.Text.Trim() + "' AND '" + toDateTextBox.Text.Trim() + "'";
        }

        return pram;
    }

    private string GstParameter()
    {
        string pram = "WHERE vTblTotal.WearhouseId IS NOT NULL";

        if (warehouseDropDownList.SelectedValue != "")
        {
            pram = pram + " AND vTblTotal.WearhouseId = " + Convert.ToInt32(warehouseDropDownList.SelectedValue);
        }


        if (productDropDownList.SelectedValue != "")
        {
            pram = pram + " AND vTblTotal.ProductCode = '" + productDropDownList.SelectedValue + "'";
        }

        return pram;
    }

    protected void excelButton_OnClick(object sender, EventArgs e)
    {
        if (reportDropDownList.SelectedValue == "CSR")
        {
            Session["ReportType"] = "";
            Session["ReportType"] = "CSR";
            Session["ProductCode"] = "";
            Session["ProductCode"] = GstParameter();
            //if (productDropDownList.SelectedValue != string.Empty)
            //{
            //    Session["ProductId"] = Convert.ToInt32(productDropDownList.SelectedValue);
            //}
             
            Session["Excel"] = "";
            Session["Excel"] = 1;

            string url = "../SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=";
            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL =
                "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url +
                "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}