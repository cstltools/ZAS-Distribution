using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CustomerPaymentProcess : System.Web.UI.Page
{
    PaymentTranscationDal aTranscationDal = new PaymentTranscationDal();
    private CustPaymentBLL aCustPaymentBll = new CustPaymentBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
            LoadPaymentData();
        }
    }

    private void LoadDropDownList()
    {
        InvoiceBatchBLL aInvoiceBatchBll = new InvoiceBatchBLL();
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        aOrderInfoBll.LoadManufac(manufacDropDownList);
        aInvoiceBatchBll.LoadDeliveryMan(delDropDownList);
        aCustPaymentBll.PaymentTypeLoadBLL(payTypeDDL);

    }

    protected void CustMasterNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CustomerPaymentList.aspx");
    }

    protected void customerTextBox_TextChanged(object sender, EventArgs e)
    {
        CustomerMaster aCustomerMaster;
        var CustomerID = CustomerId(out aCustomerMaster);
        DataTable dt = aCustPaymentBll.LoadInvoiceNew(batchDropDownList.SelectedValue, CustomerID.ToString());

        if (dt.Rows.Count > 0)
        {
            loadGridView.DataSource = dt;
            loadGridView.DataBind();
        }

        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
            ShowMessageBox("No Payment Found !!");
        }
    }

    protected void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (singleCustomerTextBox.Text.Contains(':'))
        {
            string[] data = singleCustomerTextBox.Text.Split(':');
            singleCustomerTextBox.Text = data[1];
            hdCustomerId.Value = data[3];

            if (payTypeDDL.SelectedValue == 2.ToString())
            {
                SetBalanceInfo();
            }
        }
        else
        {

            if (singleCustomerTextBox.Text != "")
            {
                hdCustomerId.Value = "";
                DataTable aTable = aCustPaymentBll.GetCustomerInfoByCode(singleCustomerTextBox.Text.Trim());

                if (aTable.Rows.Count > 0)
                {
                    hdCustomerId.Value =
                        aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);

                    if (payTypeDDL.SelectedValue == 2.ToString())
                    {
                        SetBalanceInfo();
                    }
                }
                else
                {
                    hdCustomerId.Value = "";
                    singleCustomerTextBox.Text = "";
                    ShowMessageBox("Invalid Customer Info !!");
                }
            }
        }
    }

    private void SetBalanceInfo()
    {

        if (hdCustomerId.Value != "" && payTypeDDL.SelectedValue == 2.ToString(CultureInfo.InvariantCulture))
        {
            DataTable aTable = aCustPaymentBll.GetCreditBalanceInfo(hdCustomerId.Value);

            if (aTable.Rows.Count > 0)
            {
                totalBalance.Text = aTable.Rows[0].Field<decimal>("Balance").ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                totalBalance.Text = "0.00";
            }
        }

    }

    protected void customerCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        // singleCustomerTextBox.Text = "";

        if (customerCheckBox.Checked)
        {
            singleCustomerTextBox.Enabled = true;
            delDropDownList.SelectedValue = "";
            delDropDownList.Enabled = false;
            batchDropDownList.Items.Clear();
        }

        else
        {
            hdCustomerId.Value = "";
            singleCustomerTextBox.Enabled = false;
            delDropDownList.Enabled = true;
        }
    }

    protected void payTypeDDL_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        totalBalance.Text = "";

        if (manufacDropDownList.SelectedValue != "")
        {
            if (payTypeDDL.SelectedValue == 2.ToString(CultureInfo.InvariantCulture))
            {
                customerCheckBox.Checked = true;
                singleCustomerTextBox.Enabled = true;

                delDropDownList.SelectedValue = "";
                delDropDownList.Enabled = false;

                dateTextBox.Text = "";
                dateTextBox.Enabled = false;
                batchDropDownList.Enabled = false;
                batchDropDownList.Items.Clear();

                // ddl Bank

                ddlCash.Items.Clear();
                aCustPaymentBll.LoadBankAccount(ddlBank, manufacDropDownList.SelectedValue);
            }
            else
            {
                customerCheckBox.Checked = false;
                singleCustomerTextBox.Enabled = false;

                delDropDownList.SelectedValue = "";
                delDropDownList.Enabled = true;

                dateTextBox.Text = "";
                dateTextBox.Enabled = true;
                batchDropDownList.Enabled = true;
                batchDropDownList.Items.Clear();

                ddlBank.Items.Clear();

                aCustPaymentBll.LoadCashAccount(ddlCash, manufacDropDownList.SelectedValue);
            }
        }
        else
        {
            ShowMessageBox("You should Select Company !!!");
        }

    }

    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        OtherStockActionBLL aOtherStockActionBLL = new OtherStockActionBLL();

        if (manufacDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aOtherStockActionBLL.LoadDcInformationByUser(dcDropDownList, manufacDropDownList.SelectedValue, Session["UserId"].ToString());
                    payTypeDDL.SelectedValue = "";
                    ddlBank.Items.Clear();
                    ddlCash.Items.Clear();
                }
                else
                {
                    aOtherStockActionBLL.LoadWhInformation(dcDropDownList, manufacDropDownList.SelectedValue);
                    payTypeDDL.SelectedValue = "";
                    ddlBank.Items.Clear();
                    ddlCash.Items.Clear();
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        else
        {
            dcDropDownList.Items.Clear();
            payTypeDDL.SelectedValue = "";
            ddlBank.Items.Clear();
            ddlCash.Items.Clear();
        }
    }

    protected void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dcDropDownList.SelectedValue != "")
        {
            customerCheckBox.Checked = true;
            customerCheckBox_OnCheckedChanged(null, null);

        }
        else
        {
            customerCheckBox.Checked = false;
            customerCheckBox_OnCheckedChanged(null, null);
        }
    }

    private int CustomerId(out CustomerMaster aCustomerMaster)
    {
        int CustomerID = 0;
        aCustomerMaster = new CustomerMaster();
        aCustomerMaster = aCustPaymentBll.CustomerLoad(customerTextBox.Text.Trim());
        CustomerID = aCustomerMaster.CustomerMasterId;
        return CustomerID;
    }

    protected void delDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        FindBatchInfo();
    }

    protected void customerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void batchDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (batchDropDownList.SelectedValue != "")
        {
            aCustPaymentBll.LoadCustomer(customerDropDownList, batchDropDownList.SelectedValue);
        }
        else
        {
            customerDropDownList.Items.Clear();
        }

    }

    protected void dateTextBox_TextChanged(object sender, EventArgs e)
    {
        FindBatchInfo();
    }

    private void FindBatchInfo()
    {
        //if (manufacDropDownList.SelectedValue != "")
        //{
        if (delDropDownList.SelectedValue != "")
        {
            if (dateTextBox.Text != "")
            {
                OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

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

                //aOrderInfoBll.BatchNoLoad(batchDropDownList, manufacDropDownList.SelectedValue, dateTextBox.Text, delDropDownList.SelectedValue);
                //aOrderInfoBll.BatchNoLoad(batchDropDownList, dateTextBox.Text, delDropDownList.SelectedValue);
            }
            else
            {
                dateTextBox.Text = "";
                ShowMessageBox("Batch Creation Date Is Required !!!!");
            }
        }
        else
        {
            ShowMessageBox("Please Select Delivery Man !!!!");
        }
        //}
        //else
        //{
        //    ShowMessageBox("Please Select company !!!!");
        //}
    }

    private void LoadPaymentData()
    {
        paymentBatchId.Value = "";

        DataTable aTable = aTranscationDal.LoadPaymentData(Session["UserId"].ToString(), GenerateParameter());

        if (aTable.Rows.Count > 0)
        {
            loadGridView.DataSource = aTable;
            loadGridView.DataBind();

            if (loadGridView.Rows.Count > 0)
            {
                decimal value = 0;
                for (int i = 0; i < loadGridView.Rows.Count; i++)
                {
                    CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

                   // ChkBoxRows.Checked = true;

                    if (ChkBoxRows.Checked)
                    {
                        value = value + Convert.ToDecimal(loadGridView.Rows[i].Cells[4].Text.Trim());
                    }
                }

                totalReceiveableLabel.Text = "TK. " + value.ToString(CultureInfo.InvariantCulture);
            }
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();

            totalReceiveableLabel.Text = null;
        }

        CheckBatchUpdate();

    }

    public string GenerateParameter()
    {
        string pram = "";

        pram = pram + " AND PaymentDate = CONVERT(DATE,GETDATE())";

        if (batchDropDownList.SelectedValue != "")
        {
            pram = pram + " AND tblInvoice.BatchId = " + batchDropDownList.SelectedValue;
        }
        
        if (delDropDownList.SelectedValue != "")
        {
            pram = pram + " AND tblInvoice.DeliveryManId = " + delDropDownList.SelectedValue;
        }

        if (hdCustomerId.Value != "")
        {
            pram = pram + " AND tblInvoice.CustomerMasterId = " + hdCustomerId.Value;
        }

        return pram;
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
            
            if (ChkBoxHeader.Checked)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
                //ChkBoxRows.Checked = true;
            }
        }

        CalculateTotalReceiveable();
    }


    private void CalculateTotalReceiveable()
    {
        decimal value = 0;

        if (loadGridView.Rows.Count > 0)
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
               // ChkBoxRows.Checked = true;
                if (ChkBoxRows.Checked)
                {
                    value = value + Convert.ToDecimal(loadGridView.Rows[i].Cells[6].Text.Trim());
                }
                
            }
        }

        totalReceiveableLabel.Text = "TK. " + value.ToString(CultureInfo.InvariantCulture);
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        CustPaymentBLL aCustPaymentBll = new CustPaymentBLL();
        bool status = false;

        //CustPaymentDAL aCustPaymentDal = new CustPaymentDAL();
        //int Id = aCustPaymentDal.SaveCode();

        if (Validation())
        {
            List<CustPaymentDetail> aCustPaymentDetails;

            int count = 0;

            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {

                count++;

                CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
                
                if (ChkBoxRows.Checked)
                {
                    DataTable iSExist = aTranscationDal.IsExist(loadGridView.DataKeys[i]["CustPayDetailId"].ToString());

                    if (iSExist.Rows.Count == 0)
                    {
                        DataTable aTable = aTranscationDal.GetPaymentProcess(loadGridView.DataKeys[i]["CustPayId"].ToString());

                        for (int j = 0; j < aTable.Rows.Count; j++)
                        {
                            if (aTable.Rows.Count > 0)
                            {

                                // Check if any entry exist within 5 miniutes

                                CustPaymentDAL aPaymentDal = new CustPaymentDAL();
                                DataTable paymentCountTable = new DataTable();
                                paymentCountTable =
                                    aPaymentDal.GetInvoiceExistOrNot(aTable.Rows[j].Field<int>("InvoiceId"));

                                if (paymentCountTable.Rows.Count == 0)
                                {
                                    CustPayment aCustPayment = new CustPayment();

                                    var dataKey = loadGridView.DataKeys[i];
                                    if (dataKey != null)
                                        aCustPayment.CustomerMasterId = Convert.ToInt32(dataKey[1].ToString());
                                    aCustPayment.PaymentDate = aTable.Rows[j].Field<DateTime>("PaymentDate");
                                    aCustPayment.PaymentAmount = aTable.Rows[j].Field<decimal>("PaymentAmount");
                                    aCustPayment.PayType = aTable.Rows[j].Field<string>("PayType");
                                    aCustPayment.RefNo = aTable.Rows[j].Field<string>("RefNo");
                                    aCustPayment.RefDate = aTable.Rows[j].Field<DateTime>("RefDate");
                                    aCustPayment.CreateBy = Session["LoginName"].ToString();
                                    aCustPayment.CreateDate = DateTime.Now;

                                    aCustPaymentDetails = new List<CustPaymentDetail>();

                                    CustPaymentDetail aCustPaymentDetail = new CustPaymentDetail();
                                    {
                                        aCustPaymentDetail.InvoiceId =
                                            Convert.ToInt32(aTable.Rows[j].Field<int>("InvoiceId"));
                                        aCustPaymentDetail.TransctionDetailId =
                                            Convert.ToInt32(aTable.Rows[j].Field<int>("CustPayDetailId"));
                                        aCustPaymentDetail.PaymentAmount =
                                            aTable.Rows[j].Field<decimal>("DetailPaymentAmount");
                                        aCustPaymentDetail.AIT = aTable.Rows[j].Field<decimal>("AIT");
                                        aCustPaymentDetail.Discount = aTable.Rows[j].Field<decimal>("Discount");
                                        aCustPaymentDetail.Remarks = aTable.Rows[j].Field<String>("PaymentRemarks");
                                        aCustPaymentDetail.ProcessTime = DateTime.Now;

                                        if (aTable.Rows[j].Field<string>("PayType2") == "Bank")
                                        {
                                            aCustPaymentDetail.BankAccId = aTable.Rows[j].Field<int>("BankAccId");
                                        }

                                        if (aTable.Rows[j].Field<string>("PayType2") == "Cash")
                                        {
                                            aCustPaymentDetail.CashAccId = aTable.Rows[j].Field<int>("CashAccId");
                                        }

                                    }

                                    aCustPaymentDetails.Add(aCustPaymentDetail);


                                    //if (count == 1)
                                    //{
                                    //    CustPaymentDAL aCustPaymentDal = new CustPaymentDAL();
                                    //    int Id = aCustPaymentDal.SaveCode(aCustPayment);
                                    //}

                                    status = aCustPaymentBll.SaveCustPayment(aCustPayment, aCustPaymentDetails);

                                    if (status)
                                    {
                                        string amount =
                                            aTable.Rows[j].Field<decimal>("InvPaymentAmount")
                                                .ToString(CultureInfo.InvariantCulture);
                                        string paymentStatus = aTable.Rows[j].Field<string>("PaymentStatus");

                                        aCustPaymentBll.UpdateInvoicePaymentAmount(amount, paymentStatus,
                                            aTable.Rows[j].Field<int>("InvoiceId").ToString());

                                        aTranscationDal.UpdatePaymentTranscationStatus(
                                            loadGridView.DataKeys[i]["CustPayId"].ToString());
                                    }
                                }

                            }

                        }


                    }
                    else
                    {
                        // Set Is process
                        aTranscationDal.UpdatePaymentTranscationStatus(loadGridView.DataKeys[i]["CustPayId"].ToString());
                    }

                }
                else
                {
                    //aTranscationDal.UpdatePaymentTranscation(loadGridView.DataKeys[i]["CustPayId"].ToString());
                }

            }

            LoadPaymentData();
            ShowMessageBox("Data Processed Successfullly !!!");
        }
    }

    private bool Validation()
    {

        int count = 0;

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (ChkBoxRows.Checked)
            {
                count++;

                if (count > 0)
                {
                    break;
                }
            }
        }

        if (count == 0)
        {
            
            ShowMessageBox("You should select at least one item !!!");
            return false;
        }

        return true;
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        LoadPaymentData();
    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        var dataKey = loadGridView.DataKeys[rowindex];

        UpdatePaymentTranscation(dataKey["CustPayId"].ToString(), rowindex);

    }

    private void UpdatePaymentTranscation(string custPayId, int rowindex)
    {
        if (aTranscationDal.UpdatePaymentTranscation(custPayId))
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[rowindex].Cells[0].FindControl("chkSelect");
            LinkButton rejectButton = (LinkButton)loadGridView.Rows[rowindex].Cells[0].FindControl("invoiceButton");

            ChkBoxRows.Visible = false;
            rejectButton.Visible = false;

            ShowMessageBox("Payment Reject Successfully !!!");
        }
        LoadPaymentData();
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        CalculateTotalReceiveable();
    }

    protected void Button1_Click2(object sender, EventArgs e)
    {
        LoadPaymentData();
    }

    public void CheckBatchUpdate()
    {

        if (loadGridView.Rows.Count > 0)
        {
            CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
                LinkButton rejectButton = (LinkButton)loadGridView.Rows[i].Cells[0].FindControl("invoiceButton");

                var dataKey = loadGridView.DataKeys[i];

                if (dataKey[3].ToString() == "Yes")
                {
                    ChkBoxHeader.Enabled = true;
                    ChkBoxRows.Enabled = true;
                    rejectButton.Visible = true;

                    System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#F2E7FE");
                    loadGridView.Rows[i].BackColor = col;
                }
                else
                {
                    ChkBoxHeader.Enabled = false;
                    ChkBoxRows.Enabled = false;
                    rejectButton.Visible = false;
                }

            }
        }
        
        
    }

    protected void saveBatchButton_Click(object sender, EventArgs e)
    {
         //Generate Payment Transcation Batch

         //Batch Master

        PaymentBachMasterDao aMasterDao = new PaymentBachMasterDao();

        aMasterDao.DeliveryManId = delDropDownList.SelectedValue != ""
            ? Convert.ToInt32(delDropDownList.SelectedValue)
            : 0;

        aMasterDao.PayCustomerId = hdCustomerId.Value != ""
           ? Convert.ToInt32(hdCustomerId.Value)
           : 0;
        aMasterDao.EntryBy = Convert.ToInt32(Session["UserId"].ToString());

        // Batch Detail

        List<PaymentBatchDetailDao> detailList = new List<PaymentBatchDetailDao>();

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            var custPayDetailId = loadGridView.DataKeys[i]["CustPayDetailId"].ToString();

            DataTable iSExist = aTranscationDal.CheckExistOrNot(custPayDetailId);

            if (iSExist.Rows.Count == 0)
            {
                PaymentBatchDetailDao _DAO = new PaymentBatchDetailDao();

                _DAO.TranscationDetailId = Convert.ToInt32(custPayDetailId);

                detailList.Add(_DAO);
            }

        }

        if (detailList.Count > 0)
        {
            var batchId = aTranscationDal.GenerateBatch(aMasterDao, detailList);
            paymentBatchId.Value = batchId.ToString();
        }

        LoadPaymentData();
        CheckBatchUpdate();
    }
}