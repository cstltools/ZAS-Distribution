using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_InvoiceBatchEntry : System.Web.UI.Page
{
    InvoiceBatchBLL aInvoiceBatchBll=new InvoiceBatchBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        // UpdatePanel async postback default is 90s; IIS request default is 110s
        ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 600;
        Server.ScriptTimeout = 600;

        if (!IsPostBack)
        {
            DropDownList();
            ViewReportButton(0);
        }
    }

    private void DropDownList()
    {
        aInvoiceBatchBll.LoadManufac(comDropDownList);
        aInvoiceBatchBll.LoadDeliveryMan(delManDropDownList);
    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[1].FindControl("chkSelect");
           
            if (ChkBoxHeader.Checked)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    public bool Validation()
    {
        int count = 0;
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[1].FindControl("chkSelect");
            if (ChkBoxRows.Checked==false)
            {
                count++;
            }
        }

        if (dateTextBox.Text == "")
        {
            ShowMessageBox("Date Is Required !!!");
            return false;
        }

        if (count==loadGridView.Rows.Count)
        {
            ShowMessageBox("Check At least 1 Invoice !!!");
            return false;
        }
        return true;

    }

    private void Clear()
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();
        comDropDownList.SelectedIndex = 0;
        delManDropDownList.SelectedIndex = 0;
        dateTextBox.Text = string.Empty;

        ViewReportButton(0);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            InvoiceBatchDAO aBatchDao = new InvoiceBatchDAO()
            {
                //CompanyId = Convert.ToInt32(comDropDownList.SelectedValue),
                Date = Convert.ToDateTime(dateTextBox.Text),
                DeliveryManId = Convert.ToInt32(delManDropDownList.SelectedValue)
            };

            List<int> selectedIds = new List<int>();
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[1].FindControl("chkSelect");
                if (ChkBoxRows.Checked)
                {
                    selectedIds.Add(Convert.ToInt32(loadGridView.DataKeys[i][0]));
                }
            }

            int id;
            try
            {
                // batch + batch code + invoices: one transaction, all or nothing
                id = aInvoiceBatchBll.SaveInvoiceBatchWithInvoices(aBatchDao, selectedIds);
            }
            catch (Exception ex)
            {
                ShowMessageBox("Save failed, nothing was saved. " + ex.Message.Replace("'", " ").Replace("\r", " ").Replace("\n", " "));
                return;
            }

            if (id > 0)
            {
                ViewReportButton(id);
                batchIdHiddenField.Value = id.ToString(CultureInfo.InvariantCulture);
                dateHiddenField.Value = dateTextBox.Text.Trim();
                ShowMessageBox("Data Saved Successfully");
                Clear();
            }
        }
    }

    private void ViewReportButton(int id)
    {
        if (id != 0)
        {
            printButton.Visible = true;
        }
        else
        {
            printButton.Visible = false;
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //DataTable dt = aInvoiceBatchBll.LoadInvoice(comDropDownList.SelectedValue, delManDropDownList.SelectedValue,dateTextBox.Text);

        if (delManDropDownList.SelectedValue != "")
        {
            if (dateTextBox.Text != "")
            {
                 if (Session["UserType"].ToString() != "")
                 {
                     if (Session["UserType"].ToString() != "Admin")
                     {
                         DataTable dt = aInvoiceBatchBll.LoadInvoiceByUser(delManDropDownList.SelectedValue, dateTextBox.Text, Session["UserId"].ToString());

                         if (dt.Rows.Count > 0)
                         {
                             loadGridView.DataSource = dt;
                             loadGridView.DataBind();
                         }
                         else
                         {
                             loadGridView.DataSource = null;
                             loadGridView.DataBind();
                             ShowMessageBox("No Invoice Found !!!");
                         }
                        
                     }
                     else
                     {
                         DataTable dt = aInvoiceBatchBll.LoadInvoice(delManDropDownList.SelectedValue, dateTextBox.Text);
                         
                         if (dt.Rows.Count > 0)
                         {
                             loadGridView.DataSource = dt;
                             loadGridView.DataBind();
                         }
                         else
                         {
                             loadGridView.DataSource = null;
                             loadGridView.DataBind();
                             ShowMessageBox("No Invoice Found !!!");
                         }
                     }
                 }
                 else
                 {
                     Response.Redirect("Login.aspx");
                 }
            }
            else
            {
                ShowMessageBox("Invoice date is required !!!");
            }
        }
        else
        {
            ShowMessageBox("Delivery man is required !!!");
        }
        
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void printButton_Click(object sender, EventArgs e)
    {

        if (batchIdHiddenField.Value != "" && dateHiddenField.Value != "")
        {
            DataTable aTable = aInvoiceBatchBll.LoadBatchInfo(batchIdHiddenField.Value);

            Session["CompanyId"] = "";
            Session["DelManId"] = "";
            Session["BatchId"] = "";
            Session["InvoiceDateTextBox"] = "";

            Session["CompanyId"] = aTable.Rows[0].Field<Int32>("CompanyId");
            Session["DelManId"] = aTable.Rows[0].Field<Int32>("DeliveryManId");
            Session["BatchId"] = aTable.Rows[0].Field<Int32>("BatchId");
            Session["InvoiceDateTextBox"] = Convert.ToDateTime(dateHiddenField.Value);

            string url = "../SInventory_RPTVIEW/BatchWiseTopSheetReportViewer.aspx";

            // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
            string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        
    }

    protected void delManDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }
}