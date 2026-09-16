using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerPaymentDelete : System.Web.UI.Page
{
    private readonly CustPaymentBLL aBll = new CustPaymentBLL();
    private readonly PaymentReverseDal aPaymentReverseDal = new PaymentReverseDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            aBll.LoadSC(depoDropDownList);
        }
    }

    protected void depoDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        routeDropDownList.Items.Clear();
        invoiceDropDownList.Items.Clear();
        ClearGrid();

        if (depoDropDownList.SelectedValue != "")
        {
            aBll.LoadMarket(routeDropDownList, depoDropDownList.SelectedValue);
        }
    }

    protected void routeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        invoiceDropDownList.Items.Clear();
        ClearGrid();

        if (routeDropDownList.SelectedValue != "")
        {
            aBll.LoadInvoiceForPaymentDelete(invoiceDropDownList, depoDropDownList.SelectedValue, routeDropDownList.SelectedValue);
        }
    }

    protected void invoiceDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPaymentGrid();
    }

    private void LoadPaymentGrid()
    {
        if (invoiceDropDownList.SelectedValue != "")
        {
            DataTable dt = aBll.GetPaymentDetailListByInvoiceId(invoiceDropDownList.SelectedValue);
            paymentGridView.DataSource = dt;
            paymentGridView.DataBind();
        }
        else
        {
            ClearGrid();
        }
    }

    private void ClearGrid()
    {
        paymentGridView.DataSource = null;
        paymentGridView.DataBind();
    }

    protected void paymentGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteData")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int custPayDetailId = Convert.ToInt32(paymentGridView.DataKeys[rowIndex]["CustPayDetailId"]);
            int custPayId = Convert.ToInt32(paymentGridView.DataKeys[rowIndex]["CustPayId"]);
            int invoiceId = Convert.ToInt32(invoiceDropDownList.SelectedValue);

            DeletePaymentRow(custPayDetailId, custPayId, invoiceId);
            LoadPaymentGrid();
            ShowMessageBox("Payment Deleted Successfully !!");
        }
    }

    private void DeletePaymentRow(int custPayDetailId, int custPayId, int invoiceId)
    {
        aPaymentReverseDal.InsertIntoDeleteLog(custPayId, custPayDetailId, Session["LoginName"].ToString(), DateTime.Now);
        aPaymentReverseDal.DeletePaymentDetail(custPayDetailId);

        DataTable remaining = aBll.GetPaymentDetailCountByCustPayId(custPayId.ToString());
        if (remaining.Rows.Count > 0 && Convert.ToInt32(remaining.Rows[0]["Cnt"]) == 0)
        {
            aPaymentReverseDal.DeletePaymentMaster(custPayId);
        }

        aBll.UpdateInvoicePaymentAmount("", "", invoiceId.ToString());
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}
