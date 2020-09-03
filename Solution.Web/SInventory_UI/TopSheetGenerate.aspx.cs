using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Microsoft.VisualBasic;

public partial class SInventory_UI_TopSheetGenerate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
            //dateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    public void LoadDropDown()
    {
        OtherStockActionBLL aOtherStockActionBLL = new OtherStockActionBLL();
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        //aOtherStockActionBLL.LoadmanufacturerName(manufacturerDropDownList);
        //aOtherStockActionBLL.DCLoad(dcDropDownList1, Session["UserId"].ToString());
        //manufacturerDropDownList.SelectedIndex = 1;

        aOrderInfoBll.LoadManufac(manufacDropDownList);
        aOrderInfoBll.LoadDeliveryManList(delDropDownList);

    }

    protected void dateTextBox_TextChanged(object sender, EventArgs e)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        if (dateTextBox.Text != "")
        {
            //aOrderInfoBll.BatchNoLoad(batchDropDownList, manufacDropDownList.SelectedValue, dateTextBox.Text, delDropDownList.SelectedValue);
            aOrderInfoBll.BatchNoLoad(batchDropDownList, manufacDropDownList.SelectedValue, dateTextBox.Text, delDropDownList.SelectedValue);
        }
        
    }

    protected void dcDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        aOrderInfoBll.LoadMarket(MarketDropDownList1, dcDropDownList1.SelectedValue);
        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }
    protected void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }
    protected void MarketDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        if (dcDropDownList1.SelectedValue != "" && manufacturerDropDownList.SelectedValue != "" && MarketDropDownList1.SelectedValue != "" && InvoiceDateTextBox.Text.Trim() != "")
        {
            StockConditionFreezeBLL aStockConditionFreezeBll = new StockConditionFreezeBLL();
            DataTable dt = new DataTable();
            dt = aStockConditionFreezeBll.LoadInvoice(Convert.ToInt32(dcDropDownList1.SelectedValue), Convert.ToInt32(manufacturerDropDownList.SelectedValue), Convert.ToInt32(MarketDropDownList1.SelectedValue), Convert.ToDateTime(InvoiceDateTextBox.Text.Trim()));

            if (dt.Rows.Count>0)
            {
                loadGridView.DataSource = dt;
            loadGridView.DataBind();
            }
            else
            {
                showMessageBox("No Data Found!!");
                loadGridView.DataSource = null;
                loadGridView.DataBind();
            }
            
        }
        else
        {
            showMessageBox("Please Select all Parameters");
        }

    }
    protected void gotoinvoiceButton_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        GridViewRow currentRow = (GridViewRow)button.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        //Session["InvoiceNo"] = ""
        //Session["InvoiceNo"] = loadGridView.DataKeys[rowindex]["InvoiceNo"].ToString();


        string url = "../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(loadGridView.DataKeys[rowindex]["InvoiceNo"].ToString());
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

    }
    protected void opsheetButton_Click(object sender, EventArgs e)
    {
        Session["ProformaTopSheet"] = "";
        Session["ProformaTopSheet"] = 0;

        Session["dcDropDownList1"] = "";
        Session["manufacturerDropDownList"] = "";
        Session["MarketDropDownList1"] = "";
        Session["InvoiceDateTextBox"] = "";
        Session["dcDropDownList1"] = Convert.ToInt32(dcDropDownList1.SelectedValue);
        Session["manufacturerDropDownList"] = Convert.ToInt32(manufacturerDropDownList.SelectedValue);
        Session["MarketDropDownList1"] = Convert.ToInt32(MarketDropDownList1.SelectedValue);
        Session["InvoiceDateTextBox"] = Convert.ToDateTime(InvoiceDateTextBox.Text.Trim());
        string url = "../SInventory_RPTVIEW/TopSheetReportViewer.aspx?InvNo=" + Server.UrlEncode(Session["InvoiceDateTextBox"].ToString());
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void viewRptButton_Click(object sender, EventArgs e)
    {
        //Session["ProformaTopSheet"] = "";
        //Session["ProformaTopSheet"] = 1;
        //Session["dcDropDownList1"] = "";
        //Session["manufacturerDropDownList"] = "";
        //Session["MarketDropDownList1"] = "";
        //Session["InvoiceDateTextBox"] = "";

        Session["CompanyId"] = Convert.ToInt32(manufacDropDownList.SelectedValue);
        Session["DelManId"] = Convert.ToInt32(delDropDownList.SelectedValue);
        Session["BatchId"] = Convert.ToInt32(batchDropDownList.SelectedValue);
        Session["InvoiceDateTextBox"] = Convert.ToDateTime(dateTextBox.Text.Trim());

        string url = "../SInventory_RPTVIEW/TopSheetReportViewer.aspx";

        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void manufacDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        dateTextBox.Text = "";
        batchDropDownList.Items.Clear();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        manufacDropDownList.SelectedValue = "";
        delDropDownList.SelectedValue = "";
        //dateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        batchDropDownList.Items.Clear();

    }

    protected void delDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        dateTextBox.Text = "";
        batchDropDownList.Items.Clear();
    }

    
}