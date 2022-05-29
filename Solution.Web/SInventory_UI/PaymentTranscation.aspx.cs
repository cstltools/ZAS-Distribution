using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
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

    private void Clear()
    {
        orderGridView.DataSource = null;
        orderGridView.DataBind();

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

        //SetPaymentDivStatus();
        SaveButtonVisibility();
        paymentAmountTextBox.Text = "";

        customerCheckBox.Checked = false;
        singleCustomerTextBox.Text = "";
        hdCustomerId.Value = "";

        singleCustomerTextBox.Enabled = false;

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
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
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

        InvoiceBLL aInvoiceBll = new InvoiceBLL();

        //DataTable aTable = aInvoiceBll.GetFinancialYearInfo(DateTime.Today.ToString(CultureInfo.InvariantCulture), orderGridView.DataKeys[0][2].ToString());

        //if (aTable.Rows.Count == 0)
        //{
        //    ShowMessageBox("You should declare financial year !!");
        //    return false;
        //}

        //if (payTypeDDL.SelectedValue == "")
        //{
        //    ShowMessageBox("Please Select Payment Type!!");
        //    return false;
        //}

        //if (payTypeDDL.SelectedValue != "")
        //{
        //    if (payTypeDDL.SelectedItem.Text.Trim() == "Cash")
        //    {
        //        if (ddlCash.SelectedValue == "")
        //        {
        //            ShowMessageBox("You should select Cash acount!!");
        //            return false;
        //        }
        //    }

        //    if (payTypeDDL.SelectedItem.Text.Trim() == "Bank")
        //    {
        //        if (ddlBank.SelectedValue == "")
        //        {
        //            ShowMessageBox("You should select Bank acount!!");
        //            return false;
        //        }
        //    }
        //}

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox cbReject = (CheckBox)orderGridView.Rows[i].FindControl("chkSelect");

            if (cbReject.Checked)
            {
                DataTable aTable1 = aInvoiceBll.CheckChartOfAccount(orderGridView.DataKeys[i][2].ToString());

                if (aTable1.Rows.Count == 0)
                {
                    ShowMessageBox("Customer does not exist in chart of accounts !!");
                    return false;
                }
            }
        }




        int CK = 0;

        for (int j = 0; j < orderGridView.Rows.Count; j++)
        {
            CheckBox cbReject = (CheckBox)orderGridView.Rows[j].FindControl("chkSelect");
            if (cbReject.Checked)
            {
                CK = CK + 1;
            }
        }

        if (CK == 0)
        {
            ShowMessageBox("Please Select Invoice from List!!");
            return false;
        }

        //int count = 0;

        //if (orderGridView.Rows.Count > 0)
        //{
        //    for (int i = 0; i < orderGridView.Rows.Count; i++)
        //    {

        //        if (((CheckBox)orderGridView.Rows[i].Cells[1].FindControl("chkSelect")).Checked)
        //        {
        //            if (((TextBox)orderGridView.Rows[i].FindControl("payAmountTextBox")).Text == "")
        //            {
        //                ShowMessageBox("Please fill out Pay Amount !!");
        //                return false;
        //            }
        //            count++;
        //        }
        //    }
        //}

        //if (paymentAmountTextBox.Text.Trim() == "")
        //{
        //    ShowMessageBox("Please Select Payment Amount !!");
        //    return false;
        //}

        
        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            decimal totalamount = 0;

            CheckBox cbReject = (CheckBox)orderGridView.Rows[i].FindControl("chkSelect");

            if (cbReject.Checked)
            {
                TextBox payAmountTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("payAmountTextBox");
                TextBox aitAmountTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("aitTextBox");
                TextBox discountAmountTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("discountTextBox");

                if (payAmountTextBox.Text != "")
                {
                    totalamount = totalamount + Convert.ToDecimal((payAmountTextBox.Text));
                }

                if (aitAmountTextBox.Text != "")
                {
                    totalamount = totalamount + Convert.ToDecimal((aitAmountTextBox.Text));
                }

                if (discountAmountTextBox.Text != "")
                {
                    totalamount = totalamount + Convert.ToDecimal((discountAmountTextBox.Text));
                }

                if (totalamount == 0)
                {   payAmountTextBox.Focus();
                    ShowMessageBox("Payment Amount must be greater then 0 !!");
                    return false;
                }
                
            }

        }

        

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox cbReject = (CheckBox)orderGridView.Rows[i].FindControl("chkSelect");

            if (cbReject.Checked)
            {
                CheckBox cbBank = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("isBank");
                CheckBox cbCash = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("isCash");

                DropDownList ddlBank = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlBank");
                DropDownList ddlCash = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlCash");


                if (!cbBank.Checked && !cbCash.Checked)
                {
                    ShowMessageBox("You should Select Payment Method !!!");
                    return false;
                }
                else
                {
                    if (cbBank.Checked)
                    {
                        if (ddlBank.SelectedValue == "")
                        {
                            ddlBank.Focus();
                            ShowMessageBox("You should Select Bank account !!!");
                            return false;
                        }
                    }

                    if (cbCash.Checked)
                    {
                        if (ddlCash.SelectedValue == "")
                        {
                            ddlCash.Focus();
                            ShowMessageBox("You should Select Cash account !!!");
                            return false;
                        }
                    }
                }
            }

        }




        if (paymentDtTextBox.Text == "")
        {
            ShowMessageBox("Please Select Payment Date!!");
            return false;
        }

        //if (customerDropDownList.SelectedIndex == 0)
        //{
        //    ShowMessageBox("Please Select Customer!!");
        //    return false;
        //}



        return true;
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            CustomerMaster aCustomerMaster;

            //CustPayment aCustPayment = new CustPayment();
            //aCustPayment.CustomerMasterId = Convert.ToInt32(customerDropDownList.SelectedValue); 

            //aCustPayment.PaymentDate = Convert.ToDateTime(paymentDtTextBox.Text);
            //aCustPayment.PaymentAmount = Convert.ToDecimal(paymentAmountTextBox.Text);
            //aCustPayment.PayType = payTypeDDL.SelectedItem.Text;
            //aCustPayment.RefNo = refNameTextBox.Text;
            //if (refDtTextBox.Text!="")
            //{
            //   aCustPayment.RefDate = Convert.ToDateTime(refDtTextBox.Text);
            //}
            //aCustPayment.CreateBy = Session["LoginName"].ToString();
            //aCustPayment.CreateDate = DateTime.Now;

            bool status = false;


            List<PaymentTranscationDetailDao> aCustPaymentDetails;

            for (int i = 0; i < orderGridView.Rows.Count; i++)
            {



                CheckBox ChkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");
                TextBox payAmountTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("payAmountTextBox");
                TextBox remarksTextBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("remarksTextBox");

                decimal prevamount = 0;
                if (orderGridView.Rows[i].Cells[7].Text != "&nbsp;")
                {
                    prevamount = Convert.ToDecimal(orderGridView.Rows[i].Cells[7].Text);
                }

                if (ChkBoxRows.Checked)
                {

                    DataTable dtdata = aTranscationDal.ValidationDoble(orderGridView.DataKeys[i]["InvoiceId"].ToString());

                    if (dtdata.Rows.Count > 0)
                    {
                        ShowMessageBox("Wait 5 minute to entry payment against same invoice !!!");

                    }
                    else
                    {
                        PaymentTranscationMasterDao aCustPayment = new PaymentTranscationMasterDao();

                        if (payTypeDDL.SelectedValue == 2.ToString(CultureInfo.InvariantCulture))
                        {
                            CreditAdjustmentDao adjustmentDao = new CreditAdjustmentDao();

                            InvoiceBLL aInvoiceBll = new InvoiceBLL();



                            DataTable companyInfo = aInvoiceBll.GetCompanyIdByInvoice(orderGridView.DataKeys[i]["InvoiceId"].ToString());

                            if (companyInfo.Rows.Count > 0)
                            {
                                adjustmentDao.CustomerMasterId = Convert.ToInt32(orderGridView.DataKeys[i]["CustomerMasterId"].ToString());
                                adjustmentDao.CompanyId = Convert.ToInt32(companyInfo.Rows[0]["CompanyId"]);
                                adjustmentDao.InvoiceId = Convert.ToInt32(orderGridView.DataKeys[i]["InvoiceId"].ToString());
                                if ((Convert.ToDecimal(payAmountTextBox.Text) + prevamount) ==
                                Convert.ToDecimal(orderGridView.Rows[i].Cells[5].Text))
                                {
                                    decimal totalamount = 0;
                                    totalamount = (Convert.ToDecimal(payAmountTextBox.Text));
                                    adjustmentDao.Amount = totalamount * -1;
                                }
                                else
                                {
                                    decimal totalamount = 0;
                                    totalamount = (Convert.ToDecimal(payAmountTextBox.Text));
                                    adjustmentDao.Amount = totalamount * -1;
                                }

                                adjustmentDao.ReturnDate = Convert.ToDateTime(paymentDtTextBox.Text);
                                adjustmentDao.EntryBy = Session["LoginName"].ToString();
                                adjustmentDao.EntryDate = DateTime.Now;
                            }

                            aInvoiceBll.SaveCreditAdjustmentInfo(adjustmentDao);

                        }

                        //aCustPayment.CustomerMasterId = Convert.ToInt32(customerDropDownList.SelectedValue);
                        var dataKey = orderGridView.DataKeys[i];
                        if (dataKey != null)
                            aCustPayment.CustomerMasterId = Convert.ToInt32(dataKey[1].ToString());
                        aCustPayment.PaymentDate = Convert.ToDateTime(paymentDtTextBox.Text);
                        aCustPayment.PaymentAmount = Convert.ToDecimal(paymentAmountTextBox.Text);
                        //aCustPayment.PayType = payTypeDDL.SelectedItem.Text;
                        aCustPayment.RefNo = refNameTextBox.Text;
                        if (refDtTextBox.Text != "")
                        {
                            aCustPayment.RefDate = Convert.ToDateTime(refDtTextBox.Text);
                        }
                        aCustPayment.CreateBy = Session["LoginName"].ToString();
                        aCustPayment.CreateDate = DateTime.Now;


                        if (payAmountTextBox.Text.Trim() != "")
                        {
                            if ((Convert.ToDecimal(payAmountTextBox.Text) + prevamount) ==
                            Convert.ToDecimal(orderGridView.Rows[i].Cells[7].Text))
                            {
                                decimal totalamount = 0;
                                totalamount = (Convert.ToDecimal(payAmountTextBox.Text) + prevamount);

                                aCustPayment.InvPaymentAmount = totalamount;
                                aCustPayment.PaymentStatus = "Full";
                                //aCustPaymentBll.UpdateInvoicePaymentAmount(totalamount.ToString(), "Full",
                                //    orderGridView.DataKeys[i][0].ToString());
                            }
                            else
                            {
                                decimal totalamount = 0;
                                totalamount = (Convert.ToDecimal(payAmountTextBox.Text) + prevamount);

                                aCustPayment.InvPaymentAmount = totalamount;
                                aCustPayment.PaymentStatus = "Partial";

                                //aCustPaymentBll.UpdateInvoicePaymentAmount(totalamount.ToString(), "Partial",
                                //    orderGridView.DataKeys[i][0].ToString());
                            }
                        }

                        

                        aCustPaymentDetails = new List<PaymentTranscationDetailDao>();

                        PaymentTranscationDetailDao aCustPaymentDetail = new PaymentTranscationDetailDao();
                        {
                            aCustPaymentDetail.InvoiceId = Convert.ToInt32(orderGridView.DataKeys[i]["InvoiceId"].ToString());

                            if (payAmountTextBox.Text.Trim() != "")
                            {
                                aCustPaymentDetail.PaymentAmount = Convert.ToDecimal(payAmountTextBox.Text);
                            }
                            else
                            {
                                aCustPaymentDetail.PaymentAmount = 0;
                            }
                            
                            aCustPaymentDetail.Remarks = remarksTextBox.Text;


                            DropDownList ddlBank = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlBank");
                            DropDownList ddlCash = (DropDownList)orderGridView.Rows[i].Cells[0].FindControl("ddlCash");

                            TextBox aiTextBox = (TextBox)orderGridView.Rows[i].Cells[0].FindControl("aitTextBox");
                            TextBox disTextBox = (TextBox)orderGridView.Rows[i].Cells[0].FindControl("discountTextBox");

                            if (ddlBank.SelectedValue != "")
                            {
                                aCustPaymentDetail.BankAccId = Convert.ToInt32(ddlBank.SelectedValue);
                                aCustPayment.PayType = "Bank";
                            }

                            if (ddlCash.SelectedValue != "")
                            {
                                aCustPaymentDetail.CashAccId = Convert.ToInt32(ddlCash.SelectedValue);
                                aCustPayment.PayType = "Cash";
                            }

                            if (aiTextBox.Text != "")
                            {
                                aCustPaymentDetail.AIT = Convert.ToDecimal(aiTextBox.Text.Trim());
                            }

                            if (disTextBox.Text != "")
                            {
                                aCustPaymentDetail.Discount = Convert.ToDecimal(disTextBox.Text.Trim());
                            }


                        };

                        aCustPaymentDetails.Add(aCustPaymentDetail);

                        status = aTranscationDal.SaveCustPayment(aCustPayment, aCustPaymentDetails);
                    }
                }

               
            }

            if (status)
            {
                ShowMessageBox("Data Saved Successfully !!!!!");
                Clear();
            }

            Thread.Sleep(2000);         
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

        string amount = orderGridView.Rows[i].Cells[8].Text.Trim();

        if (totalamount > Convert.ToDecimal(amount))
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
                    if (Session["UserType"].ToString() != "")
                    {
                        if (Session["UserType"].ToString() != "Admin")
                        {
                            dt = aCustPaymentBll.LoadInvoiceByCustomerByUser(hdCustomerId.Value,
                                Session["UserId"].ToString(), dcDropDownList.SelectedValue);

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

                    if (Session["UserType"].ToString() != "")
                    {
                        if (Session["UserType"].ToString() != "Admin")
                        {
                            dt = aCustPaymentBll.LoadInvoiceNewByUser(batchDropDownList.SelectedValue,
                                customerDropDownList.SelectedValue, Session["UserId"].ToString());

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

    protected void manufacDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FindBatchInfo();
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
        CheckBox cb = (CheckBox)orderGridView.Rows[selRowIndex].Cells[0].FindControl("chkSelect");
        //if (cb.Checked)
        //{
        ClaculatePaymentAmount();
        //}

        SaveButtonVisibility();


        Setddl(selRowIndex);
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

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");

            TextBox amountBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("payAmountTextBox");
            TextBox aitBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("aitTextBox");
            TextBox discountBox = (TextBox)orderGridView.Rows[i].Cells[7].FindControl("discountTextBox");

            decimal amount = 0;

            if (ChkBoxRows.Checked)
            {
                if (amountBox.Text != "")
                {
                    paymentAmount = paymentAmount + Convert.ToDecimal(amountBox.Text.Trim());
                }
                else
                {
                    amountBox.Focus();
                    ShowMessageBox("Payment amount is required !!");
                }
            }
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
            

            aCustPaymentBll.LoadBankAccount(ddlBank, orderGridView.DataKeys[selRowIndex]["CompanyId"].ToString());
        }

        if (cbCash.Checked)
        {
          

            aCustPaymentBll.LoadCashAccount(ddlCash, orderGridView.DataKeys[selRowIndex]["CompanyId"].ToString());
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
}