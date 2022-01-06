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
    DuplicatePaymentCorrectionDal aInvoiceDal = new DuplicatePaymentCorrectionDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadErrorInvoiceList();
        }
    }

    private void LoadErrorInvoiceList()
    {
        DataTable aTable = aInvoiceDal.GetDuplicatePaymentList();

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
        
        if (invoiceGridView.Rows.Count > 0)
        {
            

            for (int i = 0; i < invoiceGridView.Rows.Count; i++)
            {
                string invoiceNo = "";

                var dataKey = invoiceGridView.DataKeys[i];
                if (dataKey != null) invoiceNo = dataKey[0].ToString();

                aInvoiceDal.DeleteDuplicatePayment(invoiceNo.Trim());
            }

            ShowMessageBox("Payment Deleted Successfully !!");
        }

        LoadErrorInvoiceList();
        
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void refreshButton_Click(object sender, EventArgs e)
    {
       LoadErrorInvoiceList();
    }
}