using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_ErrorInvoiceList : System.Web.UI.Page
{
    ErrorInvoiceDal aInvoiceDal = new ErrorInvoiceDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadErrorInvoiceList();
        }
    }

    private void LoadErrorInvoiceList()
    {
        DataTable aTable = aInvoiceDal.GetErrorInvoiceList();

        if (aTable.Rows.Count > 0)
        {
            invoiceGridView.DataSource = aTable;
            invoiceGridView.DataBind();
        }
        else
        {
            invoiceGridView.DataSource = null;
            invoiceGridView.DataBind();
        }
    }

    protected void removeButton_Click(object sender, EventArgs e)
    {
        ProformaOrInvoiceReturnBLL aInvoiceReturnBll = new ProformaOrInvoiceReturnBLL();
        
        // Journal Posting

        if (invoiceGridView.Rows.Count > 0)
        {
            

            for (int i = 0; i < invoiceGridView.Rows.Count; i++)
            {
                string invoiceNo = "";

                var dataKey = invoiceGridView.DataKeys[i];
                if (dataKey != null) invoiceNo = dataKey[0].ToString();

                aInvoiceReturnBll.ProformaReturnPosting(invoiceNo.Trim());
                aInvoiceReturnBll.DeleteProforma(invoiceNo.Trim(), Session["LoginName"].ToString());
            }

            ShowMessageBox("Invoice Canceled Successfully !!");
        }

        LoadErrorInvoiceList();
        

        
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void refreshButton_Click(object sender, EventArgs e)
    {
       LoadErrorInvoiceList();
    }
}