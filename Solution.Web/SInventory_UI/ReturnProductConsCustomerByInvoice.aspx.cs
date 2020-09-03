using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;
using System.Drawing;

public partial class SInventory_UI_ReturnProductCustomerByInvoice : System.Web.UI.Page
{
    private RequisitionBLL aRequisitionBll = new RequisitionBLL();
    private InvoiceBLL aInvoiceBll = new InvoiceBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Session["InvoiceId"] != null)
            //{
            //    LoadInvoice(Session["InvoiceId"].ToString());
            //    Session["InvoiceId"] = null;
            //}
            //payTypeDDL.SelectedIndex = 1;
            //aInvoiceBll.PaymentTypeLoadBLL(payTypeDDL);

            Todate();
            //InitialGrid();
        }
    }

    public void Todate()
    {
        invDateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy");
    }

    private void GetCustInfo(string custCode)
    {
        if (!string.IsNullOrEmpty(custCode))
        {
            custCodeTextBox.Text = custCode;
            DataTable aDataTable = new DataTable();
            aDataTable = aInvoiceBll.LoadConsumerCustomer(custCode);
            if (aDataTable.Rows.Count > 0)
            {
                //hdComUnitId.Value = aDataTable.Rows[0]["ComUnitId"].ToString();
                hdCustomerMasterId.Value = aDataTable.Rows[0]["ConsumerCustId"].ToString();
                custCodeTextBox.Text = aDataTable.Rows[0]["CustomerCode"].ToString();
                custNameTextBox.Text = aDataTable.Rows[0]["CustomerName"].ToString();
                custAddressTextBox.Text = aDataTable.Rows[0]["CustomerAddress"].ToString();
                //districtNameTextBox.Text = aDataTable.Rows[0]["DistrictName"].ToString();
                //areaNameTextBox.Text = aDataTable.Rows[0]["AreaName"].ToString();
                //comUnitNameTextBox.Text = aDataTable.Rows[0]["ComUnitCode"].ToString() + ":" + aDataTable.Rows[0]["ComUnitName"].ToString();
                //miaCodeTextBox.Text = aDataTable.Rows[0]["EmpMasterCode"].ToString();
                //hdMiaId.Value = aDataTable.Rows[0]["MIOId"].ToString();
                //marketNameTextBox.Text = aDataTable.Rows[0]["MarketName"].ToString();
                //miaNameTextBox.Text = aDataTable.Rows[0]["EmpName"].ToString();
                //custCategoryTextBox.Text = aDataTable.Rows[0]["CategoryName"].ToString();
                //hdMiaId.Value = aDataTable.Rows[0]["MIOId"].ToString();
            }
            else
            {
            }
        }
    }

    public void LoadInvoice(string invoiceId)
    {
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();
        DataTable dtinvoice = aOrderInfoBll.LoadInvoice(invoiceId);
        if (dtinvoice.Rows.Count > 0)
        {
            GetCustInfo(dtinvoice.Rows[0]["CustomerMasterId"].ToString());
            //orderNoTextBox.Text = dtinvoice.Rows[0]["OrderNo"].ToString();
            //deliverypersonNameTextBox.Text = dtinvoice.Rows[0]["DeliveryManName"].ToString();
            //deliverypersonMobileTextBox.Text = dtinvoice.Rows[0]["PhoneNo"].ToString();
            //remarksTextBox.Text = dtinvoice.Rows[0]["Remarks"].ToString();
            invoiceNoHiddenField.Value = dtinvoice.Rows[0]["InvoiceNo"].ToString();
            //orderIdHiddenField.Value = dtinvoice.Rows[0]["OrderId"].ToString();
            //orderDateTextBox.Text = Convert.ToDateTime(dtinvoice.Rows[0]["OrderDate"].ToString())
            //    .ToString("dd-MMM-yyyy");
            invoiceHiddenField.Value = invoiceId;
            //gridLineItemGridView.DataSource = dtinvoice;
            //gridLineItemGridView.DataBind();
            //TotalValueCalculation();
        }
     
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        CustomerReturnMasterDAO aCustomerReturnMasterDao = new CustomerReturnMasterDAO()
        {
           InvoiceId = Convert.ToInt32(invoiceHiddenField.Value),
           ActionStatus = "Posted",
           EntryBy = Session["LoginName"].ToString(),
           EntryDate = DateTime.Now,
           ApprovedBy = Session["LoginName"].ToString(),
           ApprovedDate = DateTime.Now,
           
        };
        decimal totalamnt = 0;
        int totalqty = 0;
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            totalqty += string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text) ? 0 :
                Convert.ToInt32(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text);
            totalamnt += string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text) ? 0 :
                Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text);

        }
        aCustomerReturnMasterDao.TotalAmount = totalamnt;
        aCustomerReturnMasterDao.TotalQty = totalqty;
        CustomerReturnBLL aCustomerReturnBll=new CustomerReturnBLL();
        int id = 0;
        if (totalqty>0)
        {
            id = aCustomerReturnBll.SaveCustomerReturn(aCustomerReturnMasterDao);   
        }
         
        if (id>0)
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CustomerReturnDetailDAO aCustomerReturnDetailDao = new CustomerReturnDetailDAO()
                {
                    CustReturnId = id,
                    InvoiceDetailId = Convert.ToInt32(loadGridView.DataKeys[i][1].ToString()),
                    ReturnAmount = string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text) ? 0 :
                Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text),
                    ReturnQty = string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text) ? 0 :
                Convert.ToInt32(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text)
                    
                };
                
                if (aCustomerReturnDetailDao.ReturnQty>0)
                {
                    aCustomerReturnBll.SaveCustomerReturnDetail(aCustomerReturnDetailDao);
                    
                }
                DataTable dtdcstoreid = aInvoiceBll.GetDcStoreId(loadGridView.DataKeys[i][1].ToString());
                if (dtdcstoreid.Rows.Count>0)
                {
                    aCustomerReturnBll.UpdateDCStoreQty(dtdcstoreid.Rows[0]["DCStoreId"].ToString(),
                        aCustomerReturnDetailDao.ReturnQty);

                }

            }
            aCustomerReturnMasterDao.ActionStatus = "Accepted";
            aCustomerReturnMasterDao.CustReturnId = id;

            aCustomerReturnBll.ApprovalUpdateDAL(aCustomerReturnMasterDao);


            Clear();
            showMessageBox("Data Saved Successfully");
        }
        

    }
    private void Clear()
    {
        
        hdComUnitId.Value = "";
        hdCustomerMasterId.Value = "";
        custNameTextBox.Text = "";
        custAddressTextBox.Text = "";
        //districtNameTextBox.Text = "";
        //areaNameTextBox.Text = "";
        //comUnitNameTextBox.Text = "";
        //miaCodeTextBox.Text = "";
        //marketNameTextBox.Text = "";
        //miaNameTextBox.Text = "";
        //custCategoryTextBox.Text = "";
        hdMiaId.Value = "";
        custCodeTextBox.Text = "";
        //orderNoTextBox.Text = "";
        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        CustomerReturnBLL aCustomerReturnBll=new CustomerReturnBLL();
        DataTable dt = aCustomerReturnBll.LoadInvoiceNewConsumer(delInvNoTextBox0.Text);
        if (dt.Rows.Count>0)
        {
            uiel.Visible = true;
            payamntTextBox.Text = dt.Rows[0]["Amnt"].ToString();
            LoadInvoice(dt.Rows[0]["InvoiceId"].ToString());
            loadGridView.DataSource = dt;
            loadGridView.DataBind();
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        CustomerReturnBLL aCustomerReturnBll = new CustomerReturnBLL();
        DataTable dt = aCustomerReturnBll.LoadInvoiceNew(delInvNoTextBox0.Text);
        if (dt.Rows.Count > 0)
        {
            uiel.Visible = true;
            payamntTextBox.Text = dt.Rows[0]["Amnt"].ToString();
            LoadInvoice(dt.Rows[0]["InvoiceId"].ToString());
            loadGridView.DataSource = dt;
            loadGridView.DataBind();
        }
    }

    protected void retQtyTextBox_OnTextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            
            decimal qty = string.IsNullOrEmpty(((TextBox) loadGridView.Rows[i].FindControl("retQtyTextBox")).Text)?0:
                Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("retQtyTextBox")).Text);
            if (qty>Convert.ToDecimal(loadGridView.Rows[i].Cells[3].Text))
            {
                showMessageBox("Cannot be greater then Avialable Quantity");
                ((TextBox) loadGridView.Rows[i].FindControl("retQtyTextBox")).Text = string.Empty;
            }
            else
            {
                decimal tp = Convert.ToDecimal(loadGridView.DataKeys[i][2].ToString()) +
                         Convert.ToDecimal(loadGridView.DataKeys[i][3].ToString());
                ((TextBox)loadGridView.Rows[i].FindControl("retamountTextBox")).Text = (qty * tp).ToString();    
            }
            
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}