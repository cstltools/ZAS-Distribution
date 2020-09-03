using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_DelivaryInvoiceCreationAuto : System.Web.UI.Page
{
    OrderInfoBLL aOrderInfoBll=new OrderInfoBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownlist();
            //salesCenterDropDownList_SelectedIndexChanged(sender, e);
            SessionChoose();
            if (Session["DelMarketId"] != null)
            {
                delDropDownList.SelectedValue = Session["DelMarketId"].ToString();
                GridView();

            }
            //for (int i = 0; i < orderGridView.Rows.Count; i++)
            //{
                
            //    aInvoiceBll.ReturnReason(
            //        ((DropDownList)orderGridView.Rows[i].FindControl("reasonReturnDropDownList")));
            //}
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void DropDownlist()
    {

        InvoiceBatchBLL aInvoiceBatchBll=new InvoiceBatchBLL();
        aOrderInfoBll.LoadManufac(manufacDropDownList);
        manufacDropDownList.SelectedIndex = 1;
        aInvoiceBatchBll.LoadDeliveryMan(delDropDownList);
        //aOrderInfoBll.LoadDeliveryMan(delDropDownList)
       

    }
    //protected void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    aOrderInfoBll.LoadMarketByInvoice(delDropDownList,salesCenterDropDownList.SelectedValue);
    //    orderGridView.DataSource = null;
    //    orderGridView.DataBind();
    //}
    protected void manufacDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //aOrderInfoBll.LoadDC(salesCenterDropDownList, manufacDropDownList.SelectedValue);

        dateTextBox.Text = "";
        batchDropDownList.Items.Clear();
    }
    protected void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataTable aTable = new DataTable();
        //aTable = aOrderInfoBll.LoadOrderWithInvoice(salesCenterDropDownList.SelectedValue, manufacDropDownList.SelectedValue,
        //    marketDropDownList.SelectedValue);
        //orderGridView.DataSource = aTable;
        //orderGridView.DataBind();

        dateTextBox.Text = "";
        batchDropDownList.Items.Clear();
    }
    protected void gotoinvoiceButton_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        GridViewRow currentRow = (GridViewRow)button.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        DropDownList statusDropDownList = ((DropDownList)orderGridView.Rows[rowindex].FindControl("statusDropDownList"));
        
        if (statusDropDownList.SelectedItem.Text=="Partial")
        {
            Session["InvoiceId"] = orderGridView.DataKeys[rowindex]["InvoiceId"].ToString();
            Response.Redirect("DelivaryInvoiceCreationForCustomerByOrderAuto.aspx");    
        }
        else if (statusDropDownList.SelectedItem.Text=="Full")
        {

            DataTable aTable = aInvoiceBll.CheckChartOfAccount(orderGridView.Rows[rowindex].Cells[2].Text.Trim());

            if (aTable.Rows.Count > 0)
            {
                int status = aInvoiceBll.SaveFullInvoice(orderGridView.Rows[rowindex].Cells[7].Text,
                Session["LoginName"].ToString(), DateTime.Now.ToString("dd-MMM-yyyy"));

                // Journal Posting

                if (aInvoiceBll.PostDeliveryConfFullJournal(Convert.ToInt32(orderGridView.DataKeys[rowindex]["InvoiceId"])))
                {
                    ShowMessageBox("Delivery Invoice Save Successfully !!");
                    GridView();
                }
            }
            else
            {
                ShowMessageBox("Delivery Confirmation is not pssible because Customer does not exist in chart of accounts !!");
            }

            
           // if (status==1)
           
        }
        else 
        {
            //if (((DropDownList) orderGridView.Rows[rowindex].FindControl("reasonReturnDropDownList")).SelectedIndex != 0)
            //{


                int status = aInvoiceBll.SaveRejectInvoice(orderGridView.Rows[rowindex].Cells[7].Text,Session["LoginName"].ToString(), DateTime.Now.ToString("dd-MMM-yyyy"),
                    ((DropDownList) orderGridView.Rows[rowindex].FindControl("reasonReturnDropDownList")).SelectedItem.Text);
                
            //if (status == 1)
                {
                    // Journal Posting

                    if (aInvoiceBll.PostProformaReturnJournal(Convert.ToInt32(orderGridView.DataKeys[rowindex]["InvoiceId"])))
                    {
                        ShowMessageBox("Delivery Invoice Reject Successfully");
                        GridView();
                    }
                    
                    //for (int i = 0; i < orderGridView.Rows.Count; i++)
                    //{

                    //    aInvoiceBll.ReturnReason(
                    //        ((DropDownList)orderGridView.Rows[i].FindControl("reasonReturnDropDownList")));
                    //}
                }
            //}
            //else
            //{
            //    showMessageBox("Please Choose Reason !!!");
            //}

        }
        //Session["OrderId"] = orderGridView.DataKeys[rowindex]["OrderId"].ToString();
        

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GridView();
        //for (int i = 0; i < orderGridView.Rows.Count; i++)
        //{

        //    aInvoiceBll.ReturnReason(
        //        ((DropDownList)orderGridView.Rows[i].FindControl("reasonReturnDropDownList")));
        //}
    }

    public void GridView()
    {
        DataTable aTable = new DataTable();
        aTable = aOrderInfoBll.LoadOrderWithInvoice(batchDropDownList.SelectedValue);
        orderGridView.DataSource = aTable;
        orderGridView.DataBind();
        Session["DelMarketId"] = delDropDownList.SelectedValue;
    }
    
    public void SessionChoose()
    {
        DataTable aTable = new DataTable();
        if (Session["DelMarketId"] != null)
        {
            aTable = aOrderInfoBll.LoadOrderWithInvoice(batchDropDownList.SelectedValue);
            if (aTable.Rows.Count > 0)
            {
                delDropDownList.SelectedValue = Session["DelMarketId"].ToString();
            }
            else
            {
                delDropDownList.SelectedIndex = 0;
            }
        }

    }

    protected void statusDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        DropDownList DropDownList = (DropDownList)sender;
        GridViewRow currentRow = (GridViewRow)DropDownList.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        DropDownList statusDropDownList = ((DropDownList)orderGridView.Rows[rowindex].FindControl("statusDropDownList"));
        DropDownList reasonReturnDropDownList = ((DropDownList)orderGridView.Rows[rowindex].FindControl("reasonReturnDropDownList"));
        if (statusDropDownList.SelectedItem.Text=="Reject")
        {
            reasonReturnDropDownList.Visible = true;

        }
        else
        {
            reasonReturnDropDownList.Visible = false;
        }

    }
    protected void dateTextBox_TextChanged(object sender, EventArgs e)
    {
        //aOrderInfoBll.BatchNoLoad(batchDropDownList,manufacDropDownList.SelectedValue,dateTextBox.Text,delDropDownList.SelectedValue);

        batchDropDownList.Items.Clear();

        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserType"].ToString() != "Admin")
            {
                aOrderInfoBll.BatchNoLoadByUser(batchDropDownList, dateTextBox.Text, delDropDownList.SelectedValue, Session["UserId"].ToString());
            }
            else
            {
                aOrderInfoBll.BatchNoLoad(batchDropDownList, dateTextBox.Text, delDropDownList.SelectedValue);
            }
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        manufacDropDownList.SelectedValue = "";
        delDropDownList.SelectedValue = "";
        dateTextBox.Text = "";

        if (dateTextBox.Text == "")
        {
           batchDropDownList.Items.Clear(); 
        }

        orderGridView.DataSource = null;
        orderGridView.DataBind();
    }
}