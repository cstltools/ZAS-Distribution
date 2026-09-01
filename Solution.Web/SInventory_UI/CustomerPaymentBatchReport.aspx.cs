using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerPaymentBatchReport : System.Web.UI.Page
{

    PaymentTranscationDal aTranscationDal = new PaymentTranscationDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
            dateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy");
            LoadPaymentConfirmationBatch();
        }
    }

    private void LoadDropDownList()
    {
        InvoiceBatchBLL aInvoiceBatchBll = new InvoiceBatchBLL();
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        aInvoiceBatchBll.LoadDeliveryMan(delDropDownList);

    }

    protected void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (singleCustomerTextBox.Text.Contains(':'))
        {
            string[] data = singleCustomerTextBox.Text.Split(':');
            singleCustomerTextBox.Text = data[1];
            hdCustomerId.Value = data[3];

        }
        else
        {
            singleCustomerTextBox.Text = "";
            hdCustomerId.Value = "";
        }
    }

    private void LoadPaymentConfirmationBatch()
    {
        DataTable aTable = aTranscationDal.GetCustomerPayment(GetPrameterList());

        loadGridView.DataSource = null;
        loadGridView.DataBind();

        if (aTable.Rows.Count > 0)
        {
            loadGridView.DataSource = aTable;
            loadGridView.DataBind();

            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                var closeButton = (LinkButton)loadGridView.Rows[i].Cells[1].FindControl("closeButton");

                if (loadGridView.DataKeys[i]["TotalPending"].ToString() != "0")
                {
                   
                    if (Session["LoginName"].ToString() == "rezaul")
                    {
                        closeButton.Visible = true;
                        loadGridView.Rows[i].BackColor = Color.Red;
                    }
                    else
                    {
                        closeButton.Visible = false;
                    } 

                }
                
            }


            
        }
    }


    private void PopupReport()
    {

        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        string url = "../SInventory_RPTVIEW/CustomerReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (Session["UserType"].ToString() != "Admin")
        {
            parameter = parameter + "AND M.EntryBy = '" + Convert.ToInt32(Session["UserId"].ToString()) + "'";
        }

        if (delDropDownList.SelectedValue != "")
        {
            parameter = parameter + "AND M.DeliveryManId = '" + delDropDownList.SelectedValue + "'";
        }

        if (hdCustomerId.Value != "")
        {
            parameter = parameter + "AND M.PayCustomerId = '" + hdCustomerId.Value + "'";
        }

        if (dateTextBox.Text != "")
        {
            parameter = parameter + "AND DATEADD(dd, 0, DATEDIFF(dd, 0, M.BatchDate)) = DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dateTextBox.Text.Trim()  + "'))";
        }

        return parameter;

    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        PopupReport(loadGridView.DataKeys[rowindex]["BatchMasterId"].ToString());
    }

    private void PopupReport(string invoiceNo)
    {
        string url = "../SInventory_RPTVIEW/PaymentReportViewer.aspx?InvNo=" + Server.UrlEncode(invoiceNo);
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(1150/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=1150,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        LoadPaymentConfirmationBatch();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("CustomerPaymentBatchReport.aspx");
    }

    protected void closeButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        var BatchMasterId = loadGridView.DataKeys[rowindex]["BatchMasterId"].ToString();

        aTranscationDal.LockProcess(loadGridView.DataKeys[rowindex]["BatchMasterId"].ToString());

        LoadPaymentConfirmationBatch();
    }
}