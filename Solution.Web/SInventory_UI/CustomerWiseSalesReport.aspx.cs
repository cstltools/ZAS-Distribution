using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerWiseSalesReport : System.Web.UI.Page
{
    DeliveryInvoiceReportDal aDal = new DeliveryInvoiceReportDal();

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

        if (fromDateTextBox.Text.Trim() != "")
        {
            if (toDateTextBox.Text.Trim() != "")
            {
                param = param + " AND INV.InvoiceDate BETWEEN '" + fromDateTextBox.Text.Trim() + "' AND '" +
                        toDateTextBox.Text.Trim() + "'";
            }
        }

        if (manufacDropDownList.SelectedIndex  > 0)
        {
            param = param + "  AND CI.CompanyId = '" + manufacDropDownList.SelectedValue + "' ";
        }

        if (salesCenterDropDownList.SelectedIndex > 0)
        {
            param = param + " AND INV.ComUnitId = '" + salesCenterDropDownList.SelectedValue + "'";
        }

        if (areaDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ARA.AreaId = '" + areaDropDownList.SelectedValue + "'";
        }

        if (territoryDropDownList.SelectedIndex > 0)
        {
            param = param + " AND TTR.TerritoryId = '" + territoryDropDownList.SelectedValue + "'";
        }

        if (marketDropDownList.SelectedIndex > 0)
        {
            param = param + " AND MKT.MarketId = '" + marketDropDownList.SelectedValue + "'";
        }

        if (custDropDownList.SelectedIndex > 0)
        {
            param = param + " AND INV.CustomerMasterId = '" + custDropDownList.SelectedValue + "'";
        }

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
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aDal.LoadDCByUser(salesCenterDropDownList, manufacDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aDal.LoadDC(salesCenterDropDownList, manufacDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }  
            
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
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        PopupReport(invoiceGridView.DataKeys[rowindex]["InvoiceNo"].ToString());
    }

    private void PopupReport(string invoiceNo)
    {
        string url = "../SInventory_RPTVIEW/DelivaryInvoiceReturnViewer.aspx?InvNo=" + Server.UrlEncode(invoiceNo);
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" +
                         url +
                         "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
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


    private void PopupReport()
    {

        Session["PRAM"] = "";
        Session["PRAM"] = GenerateParameter();

        Session["RPT"] = "";

        Session["FromDate"] = "";
        Session["FromDate"] = fromDateTextBox.Text.Trim();

        Session["ToDate"] = "";
        Session["ToDate"] = toDateTextBox.Text.Trim();

        string url = "../SInventory_RPTVIEW/CustomerWiseSalesReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    protected void reportButton_Click(object sender, EventArgs e)
    {
        if (fromDateTextBox.Text != "")
        {
            if (toDateTextBox.Text != "")
            {
                if (Session["UserType"].ToString() != "")
                {
                    if (Session["UserType"].ToString() != "Admin")
                    {
                        if (manufacDropDownList.SelectedValue != "")
                        {
                            if (salesCenterDropDownList.SelectedValue != "")
                            {
                                PopupReport();
                            }
                            else
                            {
                                ShowMessageBox("Please Select DC name !!!");
                            }
                        }
                        else
                        {
                            ShowMessageBox("Please Select a company !!!");
                        }

                    }
                    else
                    {
                        PopupReport();
                    }
                }
            }
            else
            {
                ShowMessageBox("Please Select to date !!!");
            }
        }
        else
        {
            ShowMessageBox("Please Select from date !!!");
        }
        

        
    }
}