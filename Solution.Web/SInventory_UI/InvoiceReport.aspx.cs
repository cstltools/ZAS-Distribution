using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_InvoiceReport : System.Web.UI.Page
{
    InvoiceReportDal aDal = new InvoiceReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownlist();
        }
    }


    private void DropDownlist()
    {  
        aDal.LoadManufac(manufacDropDownList);
        aDal.LoadDeliveryManList(delDropDownList);
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)invoiceGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < invoiceGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)invoiceGridView.Rows[i].Cells[0].FindControl("chkSelect");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

    protected void Button_Click(object sender, EventArgs e)
    {

        if (Validation())
        {
            DataTable aTable = aDal.GetInvoiceData(GenerateParameter());

            if (aTable.Rows.Count > 0)
            {
                invoiceGridView.DataSource = aTable;
                invoiceGridView.DataBind();


               
            }
            else
            {
                invoiceGridView.DataSource = null;
                invoiceGridView.DataBind();
                ShowMessageBox("No Invoice found !!!");
            }
        }
    }

    private string GenerateParameter()
    {
        string param = "";

        if (fromDateTextBox.Text.Trim() != "" )
        {
            if (toDateTextBox.Text.Trim() != "")
            {
                param = param + " WHERE (INV.DeliveryInvoiceStatus = 'Full' OR INV.DeliveryInvoiceStatus = 'Partial' OR INV.DeliveryInvoiceStatus IS NULL) AND INV.InvoiceDate BETWEEN '" + fromDateTextBox.Text.Trim() + "' AND '" +
                        toDateTextBox.Text.Trim() + "'";
            }
        }

        if (manufacDropDownList.SelectedIndex != 0)
        {
            param = param + "  AND ODR.ManufacId = '" + manufacDropDownList.SelectedValue + "' ";
        }

        if (salesCenterDropDownList.SelectedIndex > 0)
        {
            param = param + " AND INV.ComUnitId = '" + salesCenterDropDownList.SelectedValue + "'";
        }

        if (areaDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ODR.AreaId = '" + areaDropDownList.SelectedValue + "'";
        }

        if (territoryDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ODR.TerritoryId ='" + territoryDropDownList.SelectedValue + "'";
        }

        if (marketDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ODR.MarketId ='" + marketDropDownList.SelectedValue + "'";
        }
        
        if (custDropDownList.SelectedIndex > 0)
        {
            param = param + " AND INV.CustomerMasterId = '" + custDropDownList.SelectedValue + "'";
        }

        if (batchDropDownList.SelectedIndex > 0)
        {
            param = param + " AND INV.BatchId = '" + batchDropDownList.SelectedValue + "'";
        }

        param = param + " AND CUNT.CompanyId IN ( SELECT DISTINCT CompanyId FROM tblUserCompanyUnit AS CU LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = " + Convert.ToInt32(Session["UserId"]) + ")";
        //param = param + " AND CUNT.ComUnitId IN ( SELECT DISTINCT CU.CompanyUnitId FROM tblUserCompanyUnit AS CU LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = " + Convert.ToInt32(Session["UserId"]) + ")"; 

        return param;
    }

    private bool Validation()
    {
        if (fromDateTextBox.Text == "" || toDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select date range !!!");
            return false;
        }
        else
        {
            invoiceGridView.DataSource = null;
            invoiceGridView.DataBind();
        }

        return true;
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void manufacDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        salesCenterDropDownList.Items.Clear();
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        custDropDownList.Items.Clear();

        if (manufacDropDownList.SelectedValue != "")
        {
            aDal.LoadDC(salesCenterDropDownList, manufacDropDownList.SelectedValue);
        }
        else
        {
            salesCenterDropDownList.Items.Clear();
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
            custDropDownList.Items.Clear();
        }
    }

    protected void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        custDropDownList.Items.Clear();

        if (salesCenterDropDownList.SelectedValue != "")
        {
            aDal.AreaLoad(areaDropDownList, salesCenterDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
            custDropDownList.Items.Clear();
        }
    }

    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        custDropDownList.Items.Clear();

        if (areaDropDownList.SelectedValue != "")
        {
            aDal.TerritoryLoad(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {          
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
            custDropDownList.Items.Clear();
        }
    }

    protected void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        marketDropDownList.Items.Clear();
        custDropDownList.Items.Clear();

        if (territoryDropDownList.SelectedValue != "")
        {
            aDal.MarketLoad(marketDropDownList, territoryDropDownList.SelectedValue);
        }
        else
        {
            marketDropDownList.Items.Clear();
            custDropDownList.Items.Clear();
        }
    }

    protected void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        custDropDownList.Items.Clear();

        if (marketDropDownList.SelectedValue != "")
        {
            aDal.LoadCustomer(custDropDownList, marketDropDownList.SelectedValue);
        }
        else
        {
            custDropDownList.Items.Clear();
        }
    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        // Cast the sender to LinkButton
        var button = (LinkButton)sender;

        // Access the GridViewRow containing the clicked button
        var currentRow = (GridViewRow)button.NamingContainer;

        // Get the row index
        int rowindex = currentRow.RowIndex;

        // Retrieve the values of InvoiceNo and InvoiceId from the DataKeys of the GridView
        string invoiceNo = invoiceGridView.DataKeys[rowindex]["InvoiceNo"].ToString();
        string invoiceId = invoiceGridView.DataKeys[rowindex]["InvoiceId"].ToString();

        // Call the method to show the report popup
        PopupReport(invoiceNo, invoiceId);
    }

    private void PopupReport(string invoiceNo, string invoiceId)
    {
        // Create the URL for the report viewer page with encoded parameters
        string url = "../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=" + Server.UrlEncode(invoiceNo) + "&invoiceId=" + Server.UrlEncode(invoiceId);

        // JavaScript code for opening the popup window with customized size and position
        string fullURL = "var Mleft = (screen.width / 2) - (1150 / 2);" +
                         "var Mtop = (screen.height / 2) - (700 / 2);" +
                         "window.open('" + url + "', null, 'height=700,width=1150,status=yes,toolbar=no,addressbar=no,scrollbars=yes,menubar=no,location=no,top=' + Mtop + ',left=' + Mleft + '');";

        // Register the script to open the popup window
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";

        manufacDropDownList.SelectedValue = "";

        salesCenterDropDownList.Items.Clear();
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        custDropDownList.Items.Clear();

        invoiceGridView.DataSource = null;
        invoiceGridView.DataBind();
    }

    private string GenerateParameter2()
    {
        string pram = " WHERE IV.InvoiceNo IN (";


        for (int i = 0; i < invoiceGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)invoiceGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (ChkBoxRows.Checked)
            {
                pram = pram + "'" + invoiceGridView.DataKeys[i]["InvoiceNo"] + "',";
            }
        }



        if (pram != "")
        {
            pram = pram.Substring(0, pram.Length - 1);
            pram = pram + ")";
        }
        else
        {
            pram = "";
        }
        return pram;
    }

    protected void viewRptButton_Click(object sender, EventArgs e)
    {

        string pram = "";
        pram = GenerateParameter2();

        Session["paydetailId"] = "";
        Session["paydetailId"] = pram;


        string url = "../SInventory_RPTVIEW/ProformaReportPrintViewer.aspx";
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void delDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            if (delDropDownList.SelectedValue != "")
            {

                if (Session["UserType"].ToString() != "")
                {
                    if (Session["UserType"].ToString() != "Admin")
                    {
                        aDal.LoadBatch(batchDropDownList, Convert.ToDateTime(fromDateTextBox.Text),
                            Convert.ToDateTime(toDateTextBox.Text), Convert.ToInt32(delDropDownList.SelectedValue),
                            Convert.ToInt32(Session["UserId"].ToString()));

                    }
                    else
                    {
                        aDal.LoadBatch(batchDropDownList, Convert.ToDateTime(fromDateTextBox.Text),
                            Convert.ToDateTime(toDateTextBox.Text), Convert.ToInt32(delDropDownList.SelectedValue), 0);
                    }
                }


            }
        }
        else
        {
            ShowMessageBox("Please Select Date Ramge !!");
        }
    }
}