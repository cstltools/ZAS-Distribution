using System;
using System.Collections.Generic;
using System.Data;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadPaymentData();
        }
    }

    private void LoadPaymentData()
    {
        DataTable aTable = aTranscationDal.LoadPaymentData(Session["UserId"].ToString());

        if (aTable.Rows.Count > 0)
        {
            loadGridView.DataSource = aTable;
            loadGridView.DataBind();
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
        }
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

                if (ChkBoxRows.Checked)
                {
                    value = value + Convert.ToDecimal(loadGridView.Rows[i].Cells[4].Text.Trim());
                }
                
            }
        }

        totalReceiveableLabel.Text = "TK. " + value.ToString(CultureInfo.InvariantCulture);
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        CustPaymentBLL aCustPaymentBll = new CustPaymentBLL();
        bool status = false;

        if (Validation())
        {
            List<CustPaymentDetail> aCustPaymentDetails;
            

            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
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
                                    aCustPaymentDetail.InvoiceId = Convert.ToInt32(aTable.Rows[j].Field<int>("InvoiceId"));
                                    aCustPaymentDetail.TransctionDetailId = Convert.ToInt32(aTable.Rows[j].Field<int>("CustPayDetailId"));
                                    aCustPaymentDetail.PaymentAmount = aTable.Rows[j].Field<decimal>("DetailPaymentAmount");
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
                                status = aCustPaymentBll.SaveCustPayment(aCustPayment, aCustPaymentDetails);

                                if (status)
                                {
                                    string amount = aTable.Rows[j].Field<decimal>("InvPaymentAmount").ToString(CultureInfo.InvariantCulture);
                                    string paymentStatus = aTable.Rows[j].Field<string>("PaymentStatus");

                                    aCustPaymentBll.UpdateInvoicePaymentAmount(amount, paymentStatus,
                                        aTable.Rows[j].Field<int>("InvoiceId").ToString());


                                    aTranscationDal.UpdatePaymentTranscationStatus(loadGridView.DataKeys[i]["CustPayId"].ToString());
                                }

                            }


                        }
                    }
                    
                        //Set Is process
                        aTranscationDal.UpdatePaymentTranscationStatus(loadGridView.DataKeys[i]["CustPayId"].ToString());
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

        UpdatePaymentTranscation(dataKey["CustPayId"].ToString());
    }

    private void UpdatePaymentTranscation(string custPayId)
    {
        if (aTranscationDal.UpdatePaymentTranscation(custPayId))
        {
            ShowMessageBox("Payment Reject Successfully !!!");
        }
        LoadPaymentData();
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        CalculateTotalReceiveable();
    }
}