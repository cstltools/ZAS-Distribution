using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_BatchWiseCollectionReport : System.Web.UI.Page
{

    BatchWiseCollectionReportDal aReportDal = new BatchWiseCollectionReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fromDateTextBox.Text = DateTime.Today.ToString("dd-MMM-yyyy");
            toDateTextBox.Text = DateTime.Today.ToString("dd-MMM-yyyy");
            DropDownList();
        }
    }

    private void DropDownList()
    {
        ddlMoneyReceipt.Items.Clear();
        aReportDal.LoadMoneyReceipt(ddlMoneyReceipt, Convert.ToDateTime(fromDateTextBox.Text), Convert.ToDateTime(toDateTextBox.Text), Session["UserId"].ToString());
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void delDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        FindBatchInfo();
    }

    private void FindBatchInfo()
    {
        //if (manufacDropDownList.SelectedValue != "")
        //{
        //if (delDropDownList.SelectedValue != "")
        //{
        //    if (dateTextBox.Text != "")
        //    {
        //        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        //        batchDropDownList.Items.Clear();

        //        if (Session["UserType"].ToString() != "")
        //        {
        //            if (Session["UserType"].ToString() != "Admin")
        //            {
        //                aOrderInfoBll.BatchNoLoadByUser(batchDropDownList, dateTextBox.Text, delDropDownList.SelectedValue, Session["UserId"].ToString());
        //            }
        //            else
        //            {
        //                aOrderInfoBll.BatchNoLoad(batchDropDownList, dateTextBox.Text, delDropDownList.SelectedValue);
        //                //aOrderInfoBll.BatchNoLoad(batchDropDownList, manufacDropDownList.SelectedValue, dateTextBox.Text, delDropDownList.SelectedValue);
        //            }
        //        }                
        //    }
        //    else
        //    {
        //        //dateTextBox.Text = "";
        //        ShowMessageBox("Batch Creation Date Is Required !!!!");
        //    }
        //}
        //else
        //{
        //    ShowMessageBox("Please Select Delivery Man !!!!");
        //}
        //}
        //else
        //{
        //    ShowMessageBox("Please Select company !!!!");
        //}
    }

    

    private void PopupReport(string paydetailId)
    {
        Session["paydetailId"] = "";
        Session["paydetailId"] = paydetailId;

        string url = "../SInventory_RPTVIEW/BatchWiseCollectionReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void dateTextBox_TextChanged(object sender, EventArgs e)
    {
        FindBatchInfo();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        //delDropDownList.SelectedValue = "";
        //dateTextBox.Text = "";
        //batchDropDownList.Items.Clear();

        nameTextBox.Text = "";
        customerCheckBox.Checked = false;
        singleCustomerTextBox.Text = "";
        hdCustomerId.Value = "";
        singleCustomerTextBox.Enabled = false;
    }


    protected void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (singleCustomerTextBox.Text.Contains(':'))
        {
            string[] data = singleCustomerTextBox.Text.Split(':');
            singleCustomerTextBox.Text = data[1];
            hdCustomerId.Value = data[0];
        }
        
    }

    protected void customerCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        singleCustomerTextBox.Text = "";

        if (customerCheckBox.Checked)
        {
            singleCustomerTextBox.Enabled = true;
        }

        else
        {
            singleCustomerTextBox.Enabled = false;
            hdCustomerId.Value = "";
            nameTextBox.Text = "";
            loadGridView.DataSource = null;
            loadGridView.DataBind();
        }
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        DataTable aTable = new DataTable();

        //if (batchDropDownList.SelectedValue != "" && !customerCheckBox.Checked)
        //{
        //    aTable = aReportDal.BatchWiseCollection(Convert.ToInt32(batchDropDownList.SelectedValue));
        //}

        //if (customerCheckBox.Checked)
        //{
            
        //}

        aTable = aReportDal.GetMoneyReceiptWisePayment(ddlMoneyReceipt.SelectedValue);


        if (aTable.Rows.Count > 0)
        {
            loadGridView.DataSource = aTable;
            loadGridView.DataBind();
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
            ShowMessageBox("No Data Found !");
        }

        CalculateTotalReceiveable();
    }


    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string userType = Session["UserType"].ToString();
            string LoginName = Session["LoginName"].ToString();
    
            // Access the DataRowView
            DataRowView drv = (DataRowView)e.Row.DataItem;
    
            // Safely extract the PaymentDate
            DateTime? paymentDate = drv["PaymentDate"] != DBNull.Value ? (DateTime?)drv["PaymentDate"] : null;
    
            // Get the delete button
            LinkButton deleteBtn = (LinkButton)e.Row.FindControl("invoiceButton");
    
            if (deleteBtn != null)
            {
                //if (userType == "Admin")
                if (LoginName == "rezaul")
                {
                    deleteBtn.Visible = true;
                }
                //else if (paymentDate.HasValue && paymentDate.Value.Date == DateTime.Today)
                //{
                //    deleteBtn.Visible = true;
                //}
                else
                {
                    deleteBtn.Visible = false;
                }
            }
        }
    }


    private void CalculateTotalReceiveable()
    {
        decimal value = 0;

        if (loadGridView.Rows.Count > 0)
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                value = value + Convert.ToDecimal(loadGridView.Rows[i].Cells[6].Text.Trim());
            }
        }

        totalReceiveableLabel.Text = "TK. " + value.ToString(CultureInfo.InvariantCulture);
    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        try
        {
            var button = sender as LinkButton;
            if (button == null) return;

            var currentRow = (GridViewRow)button.NamingContainer;
            int rowIndex = currentRow.RowIndex;

            var custPayId = Convert.ToInt32(loadGridView.DataKeys[rowIndex]["CustPayId"]);
            var invoiceId = Convert.ToInt32(loadGridView.DataKeys[rowIndex]["InvoiceId"]);

            // Call the journal posting method
            aReportDal.PaymentJournalPosting(custPayId, invoiceId);

            // Optional: Provide user feedback
            ShowMessageBox("Reverse Journal posted successfully.");
            searchButton_Click(null, null);

        }
        catch (Exception ex)
        {
            // Optional: Log the error
            ShowMessageBox("Error during Reverse Journal Posting.");
        }
    }

    private void PopupReportNew(string moneyReceiptId)
    {
        Session["MoneyReceiptId"] = "";
        Session["MoneyReceiptId"] = moneyReceiptId;

        string url = "../SInventory_RPTVIEW/BatchWiseMoneyReceiptReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    protected void printButton_ClickNew(object sender, EventArgs e)
    {

        if (ddlMoneyReceipt.SelectedValue != "")
        {
            PopupReportNew(ddlMoneyReceipt.SelectedValue);
        }
        else
        {
            ShowMessageBox("Please Select Money Receipt");
        }
        
    }

    protected void fromDateTextBox_OnTextChanged(object sender, EventArgs e)
    {

        if (fromDateTextBox.Text.Trim() != "")
        {
            if (toDateTextBox.Text.Trim() != "")
            {
                DropDownList();
            }
            else
            {
                ShowMessageBox("Please slect To Date");
            }
        }
        else
        {
            ShowMessageBox("Please slect From Date");
        }
        
    }

    protected void toDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (fromDateTextBox.Text.Trim() != "")
        {
            if (toDateTextBox.Text.Trim() != "")
            {
                DropDownList();
            }
            else
            {
                ShowMessageBox("Please slect To Date");
            }
        }
        else
        {
            ShowMessageBox("Please slect From Date");
        }
    }

    protected void ddlMoneyReceipt_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        searchButton_Click(null,null);
    }
}