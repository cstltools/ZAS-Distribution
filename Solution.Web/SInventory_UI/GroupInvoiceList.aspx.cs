using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_GroupInvoiceList : System.Web.UI.Page
{
    InvoiceReportDal aDal = new InvoiceReportDal();
    ManualOrderCreationDal aOrderCreationDal = new ManualOrderCreationDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownlist();
        }
    }


    private void DropDownlist()
    {
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
            DataTable aTable = aDal.GetGroupInvoiceData(GenerateParameter());

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


    protected void invoiceGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string groupInvoiceNo = invoiceGridView.DataKeys[e.Row.RowIndex]["GroupInvoiceNo"] == null
                ? ""
                : invoiceGridView.DataKeys[e.Row.RowIndex]["GroupInvoiceNo"].ToString();

            CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");

            HyperLink downloadExcelHyperLink = (HyperLink)e.Row.FindControl("downloadExcelHyperLink");
            if (downloadExcelHyperLink != null)
            {
                downloadExcelHyperLink.NavigateUrl = "../SInventory_RPTVIEW/GroupInvoiceReport.aspx?groupInvoiceNo=" +
                    Server.UrlEncode(groupInvoiceNo) + "&format=excel";
            }

            if (!string.IsNullOrEmpty(groupInvoiceNo))
            {
                // Row already grouped
                e.Row.BackColor = Color.LightGray;

                if (chk != null)
                    chk.Visible = false;
            }
            else
            {
                // Available for selection
                e.Row.BackColor = Color.Transparent;

                if (chk != null)
                    chk.Visible = true;
            }
        }
    }

    private string GenerateParameter()
    {
        string param = " ";

        if (fromDateTextBox.Text.Trim() != "")
        {
            if (toDateTextBox.Text.Trim() != "")
            {
                param = param + " WHERE GroupInvoiceNo IS NOT NULL AND (INV.DeliveryInvoiceStatus = 'Full' OR INV.DeliveryInvoiceStatus = 'Partial' OR INV.DeliveryInvoiceStatus IS NULL) AND CONVERT(DATE,INV.GroupInvoiceDate) BETWEEN  CONVERT(DATE,'" + fromDateTextBox.Text.Trim() + "') AND CONVERT(DATE,'" +
                        toDateTextBox.Text.Trim() + "')";
            }
        }

        if (custIdHiddenField.Value != "")
        {
            param = param + " AND CSTMR.CustomerMasterId = " + custIdHiddenField.Value;
        }

        if (delDropDownList.SelectedValue != "")
        {
            param = param + " AND DLV.DeliveryManId = " + delDropDownList.SelectedValue;
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



    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        // Cast the sender to LinkButton
        var button = (LinkButton)sender;

        // Access the GridViewRow containing the clicked button
        var currentRow = (GridViewRow)button.NamingContainer;

        // Get the row index
        int rowindex = currentRow.RowIndex;

        // Retrieve the values of InvoiceNo and InvoiceId from the DataKeys of the GridView
        string GroupInvoiceNo = invoiceGridView.DataKeys[rowindex]["GroupInvoiceNo"].ToString();

        // Open the HTML-based Group Invoice report (replaces the old Crystal Report popup)
        string url = "../SInventory_RPTVIEW/GroupInvoiceReport.aspx?groupInvoiceNo=" + Server.UrlEncode(GroupInvoiceNo);
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
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
        custIdHiddenField.Value = "";
        custCodeTextBox.Text = "";

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
                        //aDal.LoadBatch(batchDropDownList, Convert.ToDateTime(fromDateTextBox.Text),
                        //    Convert.ToDateTime(toDateTextBox.Text), Convert.ToInt32(delDropDownList.SelectedValue),
                        //    Convert.ToInt32(Session["UserId"].ToString()));

                    }
                    else
                    {
                        //aDal.LoadBatch(batchDropDownList, Convert.ToDateTime(fromDateTextBox.Text),
                        //    Convert.ToDateTime(toDateTextBox.Text), Convert.ToInt32(delDropDownList.SelectedValue), 0);
                    }
                }


            }
        }
        else
        {
            ShowMessageBox("Please Select Date Ramge !!");
        }
    }

    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        if (custCodeTextBox.Text.Contains(':'))
        {
            custIdHiddenField.Value = "";
            string[] data = custCodeTextBox.Text.Split(':');
            custCodeTextBox.Text = data[0];
            custIdHiddenField.Value = data[3];

        }
        else
        {
            if (custCodeTextBox.Text != "")
            {
                custIdHiddenField.Value = "";
                DataTable aTable = aOrderCreationDal.GetCustomerInfoByCode(custCodeTextBox.Text.Trim());

                if (aTable.Rows.Count > 0)
                {
                    custIdHiddenField.Value =
                        aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    custIdHiddenField.Value = "";
                    custCodeTextBox.Text = "";
                    ShowMessageBox("Customer does not exist or not contain enough information !!!!");
                }
            }
        }
    }

   

    private string GenerateParameter3()
    {
        List<string> invoiceList = new List<string>();

        for (int i = 0; i < invoiceGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox)invoiceGridView.Rows[i].FindControl("chkSelect");

            if (chkBoxRows != null && chkBoxRows.Checked)
            {
                invoiceList.Add("'" + invoiceGridView.DataKeys[i]["InvoiceNo"].ToString() + "'");
            }
        }

        return string.Join(",", invoiceList);
    }

    protected void unitPriceNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GroupInvoiceEntry.aspx");
    }

    protected void unitPriceReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GroupInvoiceList.aspx");
    }
}