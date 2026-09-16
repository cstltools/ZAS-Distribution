using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_PaymentTranscation : System.Web.UI.Page
{
    private CustPaymentBLL aCustPaymentBll = new CustPaymentBLL();
    PaymentTranscationDal aTranscationDal = new PaymentTranscationDal();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownList();
            paymentDtTextBox.Text = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy");
            //SetPaymentDivStatus();
            SaveButtonVisibility();
        }
    }

    // ponytail: single centralized null-safe Session accessor -- avoids
    // NullReferenceException on every Session[key].ToString() call site
    // (expired/missing session), see Phase 1 item 2.
    private string GetSessionString(string key)
    {
        object value = Session[key];
        return value != null ? value.ToString() : null;
    }

    private void Clear()
    {
        // Clear the main order grid
        orderGridView.DataSource = null;
        orderGridView.DataBind();

        // Clear the selected order grid
        selectedOrderGridView.DataSource = null;
        selectedOrderGridView.DataBind();

        // Clear all dropdowns, textboxes and selections
        manufacDropDownList.SelectedValue = "";
        delDropDownList.SelectedValue = "";
        dateTextBox.Text = "";
        batchDropDownList.Items.Clear();
        customerDropDownList.Items.Clear();
        paymentAmountTextBox.Text = string.Empty;
        refDtTextBox.Text = string.Empty;
        refNameTextBox.Text = string.Empty;
        payTypeDDL.SelectedIndex = 0;
        customerTextBox.Text = string.Empty;

        // Button and logic state
        SaveButtonVisibility();
        paymentAmountTextBox.Text = "";

        // Customer related input fields
        customerCheckBox.Checked = false;
        singleCustomerTextBox.Text = "";
        hdCustomerId.Value = "";
        singleCustomerTextBox.Enabled = false;

        // Clear total balance
        totalBalance.Text = "";
    }



    private void DropDownList()
    {
        InvoiceBatchBLL aInvoiceBatchBll = new InvoiceBatchBLL();
        OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

        aOrderInfoBll.LoadManufac(manufacDropDownList);
        aInvoiceBatchBll.LoadDeliveryMan(delDropDownList);
        aCustPaymentBll.PaymentTypeLoadBLL(payTypeDDL);

    }


    protected void customerTextBox_TextChanged(object sender, EventArgs e)
    {
        CustomerMaster aCustomerMaster;
        var CustomerID = CustomerId(out aCustomerMaster);
        DataTable dt = aCustPaymentBll.LoadInvoiceNew(batchDropDownList.SelectedValue, CustomerID.ToString());

        if (dt.Rows.Count > 0)
        {
            orderGridView.DataSource = dt;
            orderGridView.DataBind();
        }

        else
        {
            orderGridView.DataSource = null;
            orderGridView.DataBind();
            ShowMessageBox("No Invoice Found !!");
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

    protected void btnAddToGrid_Click(object sender, EventArgs e)
    {
        DataTable dtSelected = ViewState["SelectedOrders"] as DataTable;

        if (dtSelected == null)
        {
            dtSelected = new DataTable();
            dtSelected.Columns.Add("InvoiceId", typeof(string));
            dtSelected.Columns.Add("InvoiceNo", typeof(string));
            dtSelected.Columns.Add("PaymentAmount", typeof(decimal));
            dtSelected.Columns.Add("Remarks", typeof(string));
            dtSelected.Columns.Add("BankAccId", typeof(string));
            dtSelected.Columns.Add("BankAccountName", typeof(string));
            dtSelected.Columns.Add("CashAccId", typeof(string));
            dtSelected.Columns.Add("CashAccountName", typeof(string));
            dtSelected.Columns.Add("PayType", typeof(string));
            dtSelected.Columns.Add("AIT", typeof(decimal));
            dtSelected.Columns.Add("Discount", typeof(decimal));
            dtSelected.Columns.Add("BatchId", typeof(string)); // New Column
            dtSelected.Columns.Add("CustomerMasterId", typeof(string)); // New Column
        }

        // Set batchId to "0" if batchDropDownList has no selection; otherwise use the value.
        // Same value applies to every row selected in this one Add-to-Grid click.
        string batchId = string.IsNullOrWhiteSpace(batchDropDownList.SelectedValue) ? "0" : batchDropDownList.SelectedValue;

        // Safely extract CustomerMasterId
        string customerMasterId = string.IsNullOrWhiteSpace(hdCustomerId.Value) ? "0" : hdCustomerId.Value;

        // Phase 2 item 4: sp_MRCP_MoneyReceiptMasterInsert takes a single
        // @BatchId/@CustomerMasterId for the whole submission -- one Money
        // Receipt = one batch/customer. Reject mixing batches/customers
        // across Add-to-Grid clicks in the same submission instead of
        // silently attributing everything to whichever batch/customer was
        // selected last (see PaymentTranscationDal.SaveCustPaymentNew2).
        if (dtSelected.Rows.Count > 0)
        {
            DataRow firstSelectedRow = dtSelected.Rows[0];

            if (firstSelectedRow.Field<string>("BatchId") != batchId ||
                firstSelectedRow.Field<string>("CustomerMasterId") != customerMasterId)
            {
                ShowMessageBox("This payment already has invoices from a different batch/customer. Please submit or clear the current selection before adding invoices from another batch or customer.");
                return;
            }
        }

        // Phase 2 item 5: validate the whole pass into a local buffer first.
        // dtSelected/ViewState is only mutated after every checked row in
        // this click has passed validation, so a mid-pass failure can no
        // longer silently discard rows already validated earlier in the
        // same pass (matches the atomic, stop-and-report pattern the
        // Submit-flow Validation() method already uses on this page).
        List<object[]> rowsToAdd = new List<object[]>();

        foreach (GridViewRow row in orderGridView.Rows)
        {
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;

            if (chk != null && chk.Checked)
            {
                string invoiceId = orderGridView.DataKeys[row.RowIndex]["InvoiceId"].ToString();

                string invoiceNo = row.Cells[2].Text;

                TextBox paymentAmountTextBox = row.FindControl("payAmountTextBox") as TextBox;
                TextBox remarksTextBox = row.FindControl("remarksTextBox") as TextBox;
                DropDownList ddlBank = row.FindControl("ddlBank") as DropDownList;
                DropDownList ddlCash = row.FindControl("ddlCash") as DropDownList;
                CheckBox isBank = row.FindControl("isBank") as CheckBox;
                CheckBox isCash = row.FindControl("isCash") as CheckBox;
                TextBox aitTextBox = row.FindControl("aitTextBox") as TextBox;
                TextBox discountTextBox = row.FindControl("discountTextBox") as TextBox;

                string bankAccId = (ddlBank != null && ddlBank.SelectedValue != null) ? ddlBank.SelectedValue : "0";
                string cashAccId = (ddlCash != null && ddlCash.SelectedValue != null) ? ddlCash.SelectedValue : "0";

                string bankAccountName = (isBank != null && isBank.Checked && ddlBank != null) ? ddlBank.SelectedItem.Text : "";
                string cashAccountName = (isCash != null && isCash.Checked && ddlCash != null) ? ddlCash.SelectedItem.Text : "";

                decimal paymentAmount = 0, ait = 0, discount = 0;

                if (paymentAmountTextBox != null && !string.IsNullOrWhiteSpace(paymentAmountTextBox.Text))
                    decimal.TryParse(paymentAmountTextBox.Text.Trim(), out paymentAmount);

                if (aitTextBox != null && !string.IsNullOrWhiteSpace(aitTextBox.Text))
                    decimal.TryParse(aitTextBox.Text.Trim(), out ait);

                if (discountTextBox != null && !string.IsNullOrWhiteSpace(discountTextBox.Text))
                    decimal.TryParse(discountTextBox.Text.Trim(), out discount);

                string remarks = remarksTextBox != null ? remarksTextBox.Text.Trim() : "";
                string payType = (!string.IsNullOrEmpty(ddlBank.SelectedValue) && ddlBank.SelectedValue != "0") ? "Bank" : "Cash";

                // Validation 1

                if (paymentAmount <= 0 && ait <= 0 && discount <= 0)
                {
                    ShowMessageBox(string.Format("For Invoice No {0}, at least one of Payment Amount, AIT, or Discount must be entered.", invoiceNo));
                    return;
                }

                // Validation 2

                bool isBankSelected = !string.IsNullOrEmpty(bankAccId) && bankAccId != "0" && isBank != null && isBank.Checked;
                bool isCashSelected = !string.IsNullOrEmpty(cashAccId) && cashAccId != "0" && isCash != null && isCash.Checked;

                if (!isBankSelected && !isCashSelected)
                {
                    ShowMessageBox(string.Format("For Invoice No {0}, please select at least one Bank Account or Cash Account and check the corresponding box.", invoiceNo));
                    return;
                }

                // Prevent duplicates

                bool exists = dtSelected.AsEnumerable().Any(r => r.Field<string>("InvoiceId") == invoiceId)
                    || rowsToAdd.Any(r => (string)r[0] == invoiceId);

                if (!exists)
                {
                    rowsToAdd.Add(new object[] { invoiceId, invoiceNo, paymentAmount, remarks,
                        bankAccId, bankAccountName,
                        cashAccId, cashAccountName,
                        payType, ait, discount,
                        batchId, customerMasterId });
                }
            }
        }

        foreach (object[] newRow in rowsToAdd)
        {
            dtSelected.Rows.Add(newRow);
        }

        ViewState["SelectedOrders"] = dtSelected;
        selectedOrderGridView.DataSource = dtSelected;
        selectedOrderGridView.DataBind();

        ClaculatePaymentAmount();
    }




    protected void selectedOrderGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            DataTable dtSelected = ViewState["SelectedOrders"] as DataTable;
            if (dtSelected != null && dtSelected.Rows.Count > rowIndex)
            {
                dtSelected.Rows.RemoveAt(rowIndex);
                ViewState["SelectedOrders"] = dtSelected;

                selectedOrderGridView.DataSource = dtSelected;
                selectedOrderGridView.DataBind();
            }
        }

        ClaculatePaymentAmount();
    }


    protected void customerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)orderGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");
            string dueAmount =  orderGridView.DataKeys[i][5].ToString();
            TextBox amountBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("payAmountTextBox");

            DropDownList ddlBank = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlBank");
            DropDownList ddlCash = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlCash");

            CheckBox cbBank = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("isBank");
            CheckBox cbCash = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("isCash");

            var paymentTypeName = orderGridView.DataKeys[i]["PaymentTypeName"].ToString();

            // Reset All

            cbCash.Checked = false;
            cbBank.Checked = false;

            ddlBank.Items.Clear();
            ddlCash.Items.Clear();

            amountBox.Text = "";


            
            if (ChkBoxHeader.Checked)
            {
                ChkBoxRows.Checked = true;   
                amountBox.Text = dueAmount;

                if (paymentTypeName.Trim() == "Cash")
                {
                    cbCash.Checked = true;
                    cbBank.Checked = false;

                    ddlBank.Items.Clear();

                    SetBankOrCashAccount(i, "Cash");
                }
                else
                {
                    cbCash.Checked = false;
                    cbBank.Checked = true;

                    ddlCash.Items.Clear();

                    SetBankOrCashAccount(i, "Bank");
                }

                

            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }

        ClaculatePaymentAmount();

        SaveButtonVisibility();
    }

    private void SaveButtonVisibility()
    {
        //CheckBox ChkBoxHeader = (CheckBox)orderGridView.HeaderRow.FindControl("chkSelectAll");
        Int32 count = 0;

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (ChkBoxRows.Checked)
            {
                count++;
            }
            if (count > 0)
            {
                break;
            }
        }

        if (count > 0)
        {
            saveButton.Enabled = true;
        }
        else
        {
            saveButton.Enabled = false;
        }
    }

    protected void dateTextBox_TextChanged(object sender, EventArgs e)
    {
        FindBatchInfo();
    }

    private bool Validation()
{
    if (selectedOrderGridView.Rows.Count == 0)
    {
        ShowMessageBox("Please select invoice from the list!");
        return false;
    }

    for (int i = 0; i < selectedOrderGridView.Rows.Count; i++)
    {
        GridViewRow row = selectedOrderGridView.Rows[i];
        var dataKeys = selectedOrderGridView.DataKeys[i];

        string invoiceNo = row.Cells[0].Text.Trim(); // Assuming InvoiceNo is in first cell

        // Extract and validate numeric values

        decimal paymentAmount = Convert.ToDecimal(dataKeys["PaymentAmount"] ?? 0);
        decimal ait = Convert.ToDecimal(dataKeys["AIT"] ?? 0);
        decimal discount = Convert.ToDecimal(dataKeys["Discount"] ?? 0);
        decimal totalAmount = paymentAmount + ait + discount;

        if (totalAmount <= 0)
        {
            ShowMessageBox(string.Format("For Invoice No {0}, total amount must be greater than 0.", invoiceNo));
            return false;
        }

        // Validate PayType

        string payType = dataKeys["PayType"].ToString();
        object bankAcc = dataKeys["BankAccId"];
        object cashAcc = dataKeys["CashAccId"];

        if (string.IsNullOrEmpty(payType))
        {
            ShowMessageBox(string.Format("For Invoice No {0}, please select a payment method.", invoiceNo));
            return false;
        }

        if (payType == "Bank" && (bankAcc == null || string.IsNullOrWhiteSpace(bankAcc.ToString())))
        {
            ShowMessageBox(string.Format("For Invoice No {0}, please select a valid Bank Account.", invoiceNo));
            return false;
        }

        if (payType == "Cash" && (cashAcc == null || string.IsNullOrWhiteSpace(cashAcc.ToString())))
        {
            ShowMessageBox(string.Format("For Invoice No {0}, please select a valid Cash Account.", invoiceNo));
            return false;
        }
    }

    if (string.IsNullOrWhiteSpace(paymentDtTextBox.Text))
    {
        ShowMessageBox("Please select payment date!");
        return false;
    }

    return true;
}





    protected void saveButton_ClickNew(object sender, EventArgs e)
    {

        Int32 moneyReceiptId = 0;

        if (Validation())
        {
            // Master Data

            CustPayment aCustPayment = new CustPayment();

            aCustPayment.CustomerMasterId = !string.IsNullOrEmpty(hdCustomerId.Value)
                ? Convert.ToInt32(hdCustomerId.Value)
                : 0;

            aCustPayment.BatchId = !string.IsNullOrEmpty(batchDropDownList.SelectedValue)
                ? Convert.ToInt32(batchDropDownList.SelectedValue)
                : 0;

            aCustPayment.PaymentDate = Convert.ToDateTime(paymentDtTextBox.Text);
            aCustPayment.PaymentAmount = Convert.ToDecimal(paymentAmountTextBox.Text);
            aCustPayment.PayType = payTypeDDL.SelectedItem.Text.Trim();
            aCustPayment.RefNo = refNameTextBox.Text;
            aCustPayment.PaymentRemarks = tbxRemarks.Text;

            if (!string.IsNullOrWhiteSpace(refDtTextBox.Text))
            {
                // refDtTextBox is free-typed (CalendarExtender only, no format
                // enforcement) -- parse safely instead of Convert.ToDateTime,
                // which throws FormatException on bad input and used to abort
                // the whole postback before the grid state was ever touched.
                DateTime refDate;
                bool validRefDate = DateTime.TryParseExact(
                    refDtTextBox.Text.Trim(),
                    new[] { "dd-MMM-yyyy", "d-MMM-yyyy" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out refDate);

                if (!validRefDate)
                {
                    ShowMessageBox("Please enter a valid Reference Date (format: dd-MMM-yyyy).");
                    return;
                }

                aCustPayment.RefDate = refDate;
            }

            string createBy = GetSessionString("LoginName");

            if (string.IsNullOrEmpty(createBy))
            {
                ShowMessageBox("Your session has expired. Please log in again.");
                return;
            }

            aCustPayment.CreateBy = createBy;


            // Details Data

            List<CustPaymentDetail> aCustPaymentDetails = new List<CustPaymentDetail>();

            for (int i = 0; i < selectedOrderGridView.Rows.Count; i++)
            {
                CustPaymentDetail detail = new CustPaymentDetail();

                // Get the DataKey for current row
                DataKey key = selectedOrderGridView.DataKeys[i];

                // Safe conversions from DataKey values

                detail.InvoiceId = key["InvoiceId"] != null ? Convert.ToInt32(key["InvoiceId"]) : 0;

                detail.CustomerMasterId = !string.IsNullOrWhiteSpace(Convert.ToString(key["CustomerMasterId"]))
                   ? Convert.ToInt32(key["CustomerMasterId"])
                   : 0;

                detail.BatchId = !string.IsNullOrWhiteSpace(Convert.ToString(key["BatchId"]))
                  ? Convert.ToInt32(key["BatchId"])
                  : 0;

                // BankAccId

                detail.BankAccId = !string.IsNullOrWhiteSpace(Convert.ToString(key["BankAccId"]))
                    ? Convert.ToInt32(key["BankAccId"])
                    : 0;

                // CashAccId

                detail.CashAccId = !string.IsNullOrWhiteSpace(Convert.ToString(key["CashAccId"]))
                    ? Convert.ToInt32(key["CashAccId"])
                    : 0;

                detail.PaymentAmount = key["PaymentAmount"] != null ? Convert.ToDecimal(key["PaymentAmount"]) : 0;
                detail.AIT = key["AIT"] != null ? Convert.ToDecimal(key["AIT"]) : 0;
                detail.Discount = key["Discount"] != null ? Convert.ToDecimal(key["Discount"]) : 0;
                detail.Remarks = key["Remarks"] != null ? key["Remarks"].ToString() : string.Empty;

                // Determine PayType based on account presence

                if (detail.BankAccId > 0)
                    detail.PayType = "Bank";
                else if (detail.CashAccId > 0)
                    detail.PayType = "Cash";
                else
                    detail.PayType = null;

                aCustPaymentDetails.Add(detail);
            }



           try
           {
               moneyReceiptId = aTranscationDal.SaveCustPaymentNew2(aCustPayment, aCustPaymentDetails);

               if (moneyReceiptId > 0)
               {

                   // Get Money Receipt Details

                   DataTable aTable = aTranscationDal.GetMoneyReceiptDetails(moneyReceiptId);

                   if (aTable.Rows.Count > 0)
                   {
                       invTextBox.Text = aTable.Rows[0].Field<string>("MoneyReceiptNo");
                       rptInvoiceId.Text = moneyReceiptId.ToString(CultureInfo.InvariantCulture);
                   }

                   ShowMessageBox("✅ Data saved successfully.");
                   Clear();
               }
               else
               {
                   // Nothing new was submitted -- every selected invoice line
                   // matched an existing recent payment (duplicate guard).
                   // No receipt was created, so none is shown here.
                   ShowMessageBox("⚠️ This payment already appears to be recorded (submitted moments ago). No new receipt was created.");
                   Clear();
               }
           }
           catch (Exception)
           {
               // A hard failure occurred and the whole submission was rolled
               // back -- nothing was saved. Leave the entered data on screen
               // so the user can retry instead of losing their input.
               ShowMessageBox("❌ Payment could not be saved due to an error. Nothing was recorded. Please try again.");
           }

        }


        

    }

    protected void payAmountTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox qtyTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)qtyTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        ValidatePaymentInfo(rowindex, qtyTextBox);

        //decimal prevamount = 0;
        //TextBox payAmountTextBox = (TextBox)orderGridView.Rows[rowindex].Cells[7].FindControl("payAmountTextBox");
        //decimal mainamount = string.IsNullOrEmpty(payAmountTextBox.Text) ? 0 : Convert.ToDecimal(payAmountTextBox.Text);
        //decimal delamount = Convert.ToDecimal(orderGridView.Rows[rowindex].Cells[6].Text);

        //if (orderGridView.Rows[rowindex].Cells[6].Text != "&nbsp;")
        //{
        //    prevamount = Convert.ToDecimal(orderGridView.Rows[rowindex].Cells[7].Text);
        //}


        //if ((mainamount + prevamount) > delamount)
        //{
        //    payAmountTextBox.Text = "0";
        //    ShowMessageBox("Cannot Be Greater then Invoice Quantity ");

        //}

        ClaculatePaymentAmount();

    }

    private void ValidatePaymentInfo(int i, TextBox textBox)
    {
        decimal totalamount = 0;

        TextBox payAmountTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("payAmountTextBox");
        TextBox aitAmountTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("aitTextBox");
        TextBox discountAmountTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("discountTextBox");

        if (payAmountTextBox.Text.Trim() != "")
        {
            totalamount = totalamount + Convert.ToDecimal((payAmountTextBox.Text));
        }

        if (aitAmountTextBox.Text.Trim() != "")
        {
            totalamount = totalamount + Convert.ToDecimal((aitAmountTextBox.Text));
        }

        if (discountAmountTextBox.Text.Trim() != "")
        {
            totalamount = totalamount + Convert.ToDecimal((discountAmountTextBox.Text));
        }

        // Read the real underlying Due value from DataKeys ("Due", see the
        // orderGridView DataKeyNames in the .aspx) instead of re-parsing the
        // rendered cell text -- a DB NULL due amount renders as "&nbsp;" and
        // used to throw FormatException on Convert.ToDecimal. NULL due is
        // treated as 0 due (standard convention).
        object dueValue = orderGridView.DataKeys[i]["Due"];
        decimal dueAmount = (dueValue == null || dueValue == DBNull.Value)
            ? 0
            : Convert.ToDecimal(dueValue);

        if (totalamount > dueAmount)
        {
             ShowMessageBox("Total Amount Must Be less than or Equel to due Amount !!");
             textBox.Text = "";
       }

       
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


    protected void Button1_Click1(object sender, EventArgs e)
    {
        CustomerMaster aCustomerMaster;
        var CustomerID = CustomerId(out aCustomerMaster);
        DataTable dt = aCustPaymentBll.LoadInvoiceNew(batchDropDownList.SelectedValue,
         CustomerID.ToString());

        if (dt.Rows.Count > 0)
        {
            orderGridView.DataSource = dt;
            orderGridView.DataBind();

        }

        else
        {
            orderGridView.DataSource = null;
            orderGridView.DataBind();
            ShowMessageBox("No Invoice Found!!");
        }
    }

    protected void Button1_Click2(object sender, EventArgs e)
    {
        //if (customerDropDownList.SelectedValue != "")
        //{

        CustomerMaster aCustomerMaster;
        var CustomerID = CustomerId(out aCustomerMaster);

        DataTable dt = null;


        //if (dcDropDownList.SelectedValue != "")
        //{
            if (customerCheckBox.Checked)
            {
                if (singleCustomerTextBox.Text != "" && hdCustomerId.Value != "")
                {
                    string userType = GetSessionString("UserType");
                    if (!string.IsNullOrEmpty(userType))
                    {
                        if (userType != "Admin")
                        {
                            dt = aCustPaymentBll.LoadInvoiceByCustomerByUser(hdCustomerId.Value,
                                GetSessionString("UserId"), dcDropDownList.SelectedValue);

                            if (dt.Rows.Count > 0)
                            {
                                orderGridView.DataSource = dt;
                                orderGridView.DataBind();

                            }

                            else
                            {
                                orderGridView.DataSource = null;
                                orderGridView.DataBind();
                                ShowMessageBox("No Invoice Found!!");
                            }
                        }
                        else
                        {
                            dt = aCustPaymentBll.LoadInvoiceByCustomer(hdCustomerId.Value, dcDropDownList.SelectedValue);

                            if (dt.Rows.Count > 0)
                            {
                                orderGridView.DataSource = dt;
                                orderGridView.DataBind();

                            }

                            else
                            {
                                orderGridView.DataSource = null;
                                orderGridView.DataBind();
                                ShowMessageBox("No Invoice Found!!");
                            }
                        }
                    }
                    else
                    {
                        ShowMessageBox("You are not an authorised user !!!");
                    }
                }
                else
                {
                    ShowMessageBox("You should select a customer !!!");
                }
            }
            else
            {
                if (batchDropDownList.SelectedValue != "")
                {

                    string userType2 = GetSessionString("UserType");
                    if (!string.IsNullOrEmpty(userType2))
                    {
                        if (userType2 != "Admin")
                        {
                            dt = aCustPaymentBll.LoadInvoiceNewByUser(batchDropDownList.SelectedValue,
                                customerDropDownList.SelectedValue, GetSessionString("UserId"));

                            if (dt.Rows.Count > 0)
                            {
                                orderGridView.DataSource = dt;
                                orderGridView.DataBind();
                            }
                            else
                            {
                                orderGridView.DataSource = null;
                                orderGridView.DataBind();
                                ShowMessageBox("No Invoice Found!!");
                            }
                        }
                        else
                        {
                            dt = aCustPaymentBll.LoadInvoiceNew(batchDropDownList.SelectedValue,
                                customerDropDownList.SelectedValue);

                            if (dt.Rows.Count > 0)
                            {
                                orderGridView.DataSource = dt;
                                orderGridView.DataBind();
                            }
                            else
                            {
                                orderGridView.DataSource = null;
                                orderGridView.DataBind();
                                ShowMessageBox("No Invoice Found!!");
                            }
                        }
                    }
                    else
                    {
                        ShowMessageBox("Your session has expired. Please log in again.");
                    }

                }
            }




        if (orderGridView.Rows.Count > 0)
        {
            for (int i = 0; i < orderGridView.Rows.Count; i++)
            {
                TextBox tbxDiscount = (TextBox)orderGridView.Rows[i].Cells[0].FindControl("discountTextBox");



                if (GetSessionString("UserType") == "Admin")
                {
                    orderGridView.Columns[16].Visible = true;
                }
                else
                {
                    orderGridView.Columns[16].Visible =  false;
                }
            }
        }
        //}
        //else
        //{
        //    ShowMessageBox("You should select DC !!!");
        //}





        //}
        //else
        //{
        //    ShowMessageBox("Please Select Customer !!!");
        //}

        //SetPaymentDivStatus();

    }

    //private void SetPaymentDivStatus()
    //{
    //    if (orderGridView.Rows.Count > 0)
    //    {
    //        Payment.Visible = true;
    //    }
    //    else
    //    {
    //        Payment.Visible = false;
    //    }
    //}

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void delDropDownList_OnTextChanged(object sender, EventArgs e)
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

                string userType3 = GetSessionString("UserType");
                if (!string.IsNullOrEmpty(userType3))
                {
                    if (userType3 != "Admin")
                    {
                        aOrderInfoBll.BatchNoLoadByUser(batchDropDownList, dateTextBox.Text, delDropDownList.SelectedValue, GetSessionString("UserId"));
                    }
                    else
                    {
                        aOrderInfoBll.BatchNoLoad(batchDropDownList, dateTextBox.Text, delDropDownList.SelectedValue);
                    }
                }
                else
                {
                    ShowMessageBox("Your session has expired. Please log in again.");
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

    protected void manufacDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FindBatchInfo();
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        int i = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
        CheckBox cb = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");

        CheckBox ChkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");
        string dueAmount = orderGridView.DataKeys[i][5].ToString();
        TextBox amountBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("payAmountTextBox");

        DropDownList ddlBank = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlBank");
        DropDownList ddlCash = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlCash");

        CheckBox cbBank = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("isBank");
        CheckBox cbCash = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("isCash");

        var paymentTypeName = orderGridView.DataKeys[i]["PaymentTypeName"].ToString();

        if (cb.Checked)
        {
            amountBox.Text = dueAmount.ToString();
        }
        else
        {
            amountBox.Text = "";
        }

        

        if (paymentTypeName.Trim() == "Cash")
        {
            cbCash.Checked = true;
            cbBank.Checked = false;

            ddlBank.Items.Clear();

            SetBankOrCashAccount(i, "Cash");
        }
        else
        {
            cbCash.Checked = false;
            cbBank.Checked = true;

            ddlCash.Items.Clear();

            SetBankOrCashAccount(i, "Bank");
        }

        //cbCash.Checked = true;
        //SetBankOrCashAccount(i, "Cash");

        //amountBox.Text = dueAmount.ToString();
        //if (cb.Checked)
        //{
        ClaculatePaymentAmount();
        //}

        SaveButtonVisibility();


        //Setddl(selRowIndex);
    }

    private void Setddl(int selRowIndex)
    {
        CheckBox cbBank = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isBank");
        CheckBox cbCash = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isCash");


        DropDownList ddlBank = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlBank");
        DropDownList ddlCash = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlCash");

        cbCash.Checked = false;
        cbBank.Checked = false;

        ddlBank.Items.Clear();
        ddlCash.Items.Clear();
    }

    private void ClaculatePaymentAmount()
    {
        decimal paymentAmount = 0;

        for (int i = 0; i < selectedOrderGridView.Rows.Count; i++)
        {
            // Get the DataKey for current row

            DataKey key = selectedOrderGridView.DataKeys[i];

            decimal paidAmount = key["PaymentAmount"] != null ? Convert.ToDecimal(key["PaymentAmount"]) : 0;
            decimal AIT = key["AIT"] != null ? Convert.ToDecimal(key["AIT"]) : 0;
            decimal Discount = key["Discount"] != null ? Convert.ToDecimal(key["Discount"]) : 0;


            paymentAmount = paymentAmount + Convert.ToDecimal(paidAmount);
        }

        if (paymentAmount > 0)
        {
            paymentAmountTextBox.Text = paymentAmount.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            paymentAmountTextBox.Text = 0.ToString(CultureInfo.InvariantCulture);
        }
    }

    protected void singleCustomerTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (singleCustomerTextBox.Text.Contains(':'))
        {
            string[] data = singleCustomerTextBox.Text.Split(':');

            if (data.Length < 4)
            {
                hdCustomerId.Value = "";
                singleCustomerTextBox.Text = "";
                ShowMessageBox("Invalid Customer Info !!");
                return;
            }

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
        //singleCustomerTextBox.Text = "";

        if (customerCheckBox.Checked)
        {
            singleCustomerTextBox.Enabled = true;
            delDropDownList.SelectedValue = "";
            batchDropDownList.Items.Clear();

        }

        else
        {
            singleCustomerTextBox.Enabled = false;
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
            string userType4 = GetSessionString("UserType");
            if (!string.IsNullOrEmpty(userType4))
            {
                if (userType4 != "Admin")
                {
                    aOtherStockActionBLL.LoadDcInformationByUser(dcDropDownList, manufacDropDownList.SelectedValue, GetSessionString("UserId"));
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

    protected void isBank_CheckedChanged(object sender, EventArgs e)
    {
        
        int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
        CheckBox cb = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("chkSelect");

        if (cb.Checked)
        {
            SetBankOrCashAccount(selRowIndex, "Bank");
        }
        else
        {
            CheckBox cbBank = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isBank");
            CheckBox cbCash = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isCash");

            DropDownList ddlBank = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlBank");
            DropDownList ddlCash = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlCash");

            cbBank.Checked = false;
            ddlBank.Items.Clear();

            cbCash.Checked = false;
            ddlCash.Items.Clear();

            ShowMessageBox("You should select row item first !!");
        }
        
        
    }

    private void SetBankOrCashAccount(int selRowIndex,string status)
    {
        CheckBox cbBank = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isBank");
        CheckBox cbCash = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isCash");

        DropDownList ddlBank = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlBank");
        DropDownList ddlCash = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlCash");


        if (status == "Cash")
        {
            cbBank.Checked = false;
            ddlBank.Items.Clear();
        }

        if (status == "Bank")
        {
            cbCash.Checked = false;
            ddlCash.Items.Clear();
        }


        if (cbBank.Checked)
        {

            //ddlCash.Enabled = false;
            //ddlBank.Enabled = true;
            aCustPaymentBll.LoadBankAccount(ddlBank, orderGridView.DataKeys[selRowIndex]["CompanyId"].ToString());
        }

        if (cbCash.Checked)
        {

            //ddlCash.Enabled = false;
            //ddlBank.Enabled = false;

            aCustPaymentBll.LoadCashAccount(ddlCash, orderGridView.DataKeys[selRowIndex]["CompanyId"].ToString());

            DataTable aTable = aTranscationDal.GetCashGlByUnitId(orderGridView.DataKeys[selRowIndex]["ComUnitId"].ToString());

            if (aTable.Rows.Count > 0)
            {

                if (aTable.Rows[0].Field<int>("CashGlId") > 0)
                {
                    ddlCash.SelectedValue = aTable.Rows[0].Field<int>("CashGlId").ToString(CultureInfo.InvariantCulture);
                    //ddlCash.Enabled = false;
                }

            }
        }

    }

    protected void isCash_CheckedChanged(object sender, EventArgs e)
    {
        int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
        CheckBox cb = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("chkSelect");

        if (cb.Checked)
        {
            SetBankOrCashAccount(selRowIndex,"Cash");
        }
        else
        {

            CheckBox cbBank = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isBank");
            CheckBox cbCash = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("isCash");

            DropDownList ddlBank = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlBank");
            DropDownList ddlCash = (DropDownList)orderGridView.Rows[selRowIndex].Cells[0].FindControl("ddlCash");

            cbBank.Checked = false;
            ddlBank.Items.Clear();

            cbCash.Checked = false;
            ddlCash.Items.Clear();

            ShowMessageBox("You should select row item first !!");
        }
    }

    protected void aitTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox aitTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)aitTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        ValidatePaymentInfo(rowindex, aitTextBox);
    }

    protected void discountTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox discountTextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)discountTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        ValidatePaymentInfo(rowindex, discountTextBox);
    }

    private void PopupReport(string moneyReceiptId)
    {
        Session["MoneyReceiptId"] = "";
        Session["MoneyReceiptId"] = moneyReceiptId;

        string url = "../SInventory_RPTVIEW/BatchWiseMoneyReceiptReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    protected void printButton_Click(object sender, EventArgs e)
    {
        PopupReport(rptInvoiceId.Text.Trim());
    }
}