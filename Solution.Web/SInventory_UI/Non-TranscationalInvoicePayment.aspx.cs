using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_Non_TranscationalInvoicePayment : System.Web.UI.Page
{

    BatchWiseCollectionReportDal aReportDal = new BatchWiseCollectionReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }

    protected void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (singleCustomerTextBox.Text.Contains(':'))
        {
            string[] data = singleCustomerTextBox.Text.Split(':');
            singleCustomerTextBox.Text = data[1];
            hdCustomerId.Value = data[3];

        }
        
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            DataTable aTable = aReportDal.LoadDueInvoice(GenerateParameter());

            if (aTable.Rows.Count > 0)
            {
                loadGridView.DataSource = aTable;
                loadGridView.DataBind();
            }
            else
            {
                loadGridView.DataSource = null;
                loadGridView.DataBind();
            }
        }
        else
        {
            ShowMessageBox("You should select a date range !!");
        }
    }


    private string GenerateParameter()
    {
        string pram = "";

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            pram = pram + " AND CP.PaymentDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text) + "' AND '" + Convert.ToDateTime(toDateTextBox.Text) + "'";
        }

        if (singleCustomerTextBox.Text != "")
        {
            pram = pram + " AND CustomerCode = '" + singleCustomerTextBox.Text.Trim() + "'";
        }

        return pram;
    }



    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void PopupReport(string paydetailId)
    {

        Session["paydetailId"] = "";
        Session["paydetailId"] = paydetailId;


        string url = "../SInventory_RPTVIEW/BatchWiseCollectionReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
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


    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        var dataKey = loadGridView.DataKeys[rowindex];
        if (dataKey != null)
            PopupReport(dataKey["CustPayDetailId"].ToString());
    }


    private void PopupCollectionSheetReport(string paydetailId)
    {


        string pram = "";
        pram = GenerateParameter2();

        Session["paydetailId"] = "";
        Session["paydetailId"] = pram;

        aReportDal.UpdateIsPrint(pram);

        string url = "../SInventory_RPTVIEW/BatchWiseCollectionSheetReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private string GenerateParameter2()
    {
        string pram = " AND CustPayDetailId IN (";


        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (ChkBoxRows.Checked)
            {
                pram = pram + loadGridView.DataKeys[i]["CustPayDetailId"] + ",";
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

    protected void viewButton_Click(object sender, EventArgs e)
    {
        PopupCollectionSheetReport(1.ToString());
    }
}