using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_PickingSlipGenerate : System.Web.UI.Page
{
    OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();

    InvoiceBatchBLL aInvoiceBatchBll = new InvoiceBatchBLL();

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
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    public void DropDownlist()
    {

        InvoiceBatchBLL aInvoiceBatchBll = new InvoiceBatchBLL();
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

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[6].FindControl("chkSelect");
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
        DataTable dt = aInvoiceBatchBll.LoadInvoiceNotNull(delDropDownList.SelectedValue, dateTextBox.Text.Trim(),batchDropDownList.SelectedValue);
        loadGridView.DataSource = dt;
        loadGridView.DataBind();

        //Session["DelMarketId"] = delDropDownList.SelectedValue;
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

        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }

    public string GetParameter()
    {
        string parameter = " AND INV.InvoiceId IN (";

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {

            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[6].FindControl("chkSelect");
            if (ChkBoxRows.Checked)
            {
                parameter = parameter + "'" + loadGridView.DataKeys[i][0].ToString() + "',";
            }
        }

        parameter = parameter.TrimEnd(',');
        parameter = parameter + ")";
        return parameter;

    }

    protected void ViewButton_Click(object sender, EventArgs e)
    {
        int ch = 0;
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            if (cb.Checked)
            {
                ch = ch + 1;
            }
        }

        if (ch > 0)
        {
            Session["MrktRptParameter"] = GetParameter();
            //Session["Market"] = "";
            //Session["Manufac"] = "";
            //Session["invoicedate"] = "";

            Session["DelMan"] = "";
            Session["DelMan"] = delDropDownList.SelectedValue;

            //Session["Market"] = MarketDropDownList1.SelectedValue;
            //Session["Manufac"] = manufacturerDropDownList.SelectedValue;
            //Session["invoicedate"] = dateTextBox.Text.Trim();

            string url = "../SInventory_RPTVIEW/PickingSlipGenerateViewer.aspx?SC=" + Server.UrlEncode(batchDropDownList.SelectedValue);
            //string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }
        else
        {
            showMessageBox("Please Select Invoice From List!!");
        }

    }
}