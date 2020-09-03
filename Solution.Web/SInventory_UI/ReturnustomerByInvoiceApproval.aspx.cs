using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ReturnustomerByInvoiceApproval : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    CustomerReturnBLL aCustomerReturnBll= new CustomerReturnBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ReturnCustLoad();
        }
    }

    public void ReturnCustLoad()
    {
        string filename = Path.GetFileName(Request.Path);
        
        string userName = Session["LoginName"].ToString();
        ReturnCustCondition(filename, userName);
        aCustomerReturnBll.LoadApprovalControlBLL(actionRadioButtonList, filename, userName);

    }
    private void ReturnCustCondition(string pageName, string user)
    {
        string ActionStatus = aCustomerReturnBll.LoadForApprovalConditionBLL(pageName, user);
        aDataTable = aCustomerReturnBll.LoadCustomerReturnApproval(ActionStatus);
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[5].FindControl("chkSelect");
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
    protected void btnSubmit0_Click(object sender, EventArgs e)
    {
        SubmitApproval();
        ReturnCustLoad();
    }
    private void SubmitApproval()
    {
        if (Validation())
        {
            if (actionRadioButtonList.SelectedValue == "")
            {
                showMessageBox("Please Choose any of the operation");
            }

            else
            {
                for (int i = 0; i < loadGridView.Rows.Count; i++)
                {

                    CheckBox cb = (CheckBox)loadGridView.Rows[i].Cells[5].FindControl("chkSelect");
                    if (cb.Checked == true)
                    {
                        CustomerReturnMasterDAO aCustomerReturnMasterDao = new CustomerReturnMasterDAO()
                        {
                            CustReturnId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()),
                            ActionStatus = actionRadioButtonList.SelectedItem.Text,
                            ApprovedBy = Session["LoginName"].ToString(),
                            ApprovedDate = DateTime.Today,
                        };


                        if (actionRadioButtonList.SelectedValue != null)
                        {
                            if (actionRadioButtonList.SelectedItem.Text == "Accepted")
                            {


                                DataTable dtdata =
                                    aCustomerReturnBll.LoadInvoiceDataApproval(loadGridView.DataKeys[i][0].ToString());
                                for (int j = 0; j < dtdata.Rows.Count; j++)
                                {
                                    InvoiceBLL aInvoiceBll = new InvoiceBLL();
                                    DataTable dtdcinfo = aInvoiceBll.DCInfoWithDCId(dtdata.Rows[j]["DCStoreId"].ToString());
                                    DCStockNew aDcStockNew = new DCStockNew();
                                    {
                                        aDcStockNew.DCStoreId = Convert.ToInt32(dtdata.Rows[j]["DCStoreId"].ToString());
                                        aDcStockNew.InvoiceDetailId =
                                            Convert.ToInt32(dtdata.Rows[j]["InvoiceDetailId"].ToString());
                                        aDcStockNew.StorageLocation = dtdcinfo.Rows[0]["StorageLocation"].ToString();
                                        aDcStockNew.ProductCode = dtdcinfo.Rows[0]["ProductCode"].ToString();
                                        aDcStockNew.ProductName = dtdcinfo.Rows[0]["ProductName"].ToString();
                                        aDcStockNew.PackSize = dtdcinfo.Rows[0]["PackSize"].ToString();
                                        aDcStockNew.BatchNo = dtdcinfo.Rows[0]["BatchNo"].ToString();
                                        aDcStockNew.TotalQuantity = Convert.ToDecimal(dtdata.Rows[0]["TotalQty"].ToString());
                                        aDcStockNew.ExpDate = Convert.ToDateTime(dtdcinfo.Rows[0]["ExpDate"].ToString());
                                        aDcStockNew.ReceiveDate =
                                            Convert.ToDateTime(dtdcinfo.Rows[0]["ReceiveDate"].ToString());
                                        aDcStockNew.ChalanNo = dtdcinfo.Rows[0]["ChalanNo"].ToString();
                                        aDcStockNew.ChalanDate =
                                            Convert.ToDateTime(dtdcinfo.Rows[0]["ChalanDate"].ToString());
                                        aDcStockNew.ComUnitId = Convert.ToInt32(dtdcinfo.Rows[0]["ComUnitId"].ToString());
                                        aDcStockNew.StockQty = Convert.ToInt32(dtdata.Rows[0]["TotalQty"].ToString());
                                        aDcStockNew.TotalQuantity = Convert.ToInt32(dtdata.Rows[0]["TotalQty"].ToString());
                                        aDcStockNew.DamageQty = 0;
                                        aDcStockNew.StockRcvDate =
                                            Convert.ToDateTime(dtdcinfo.Rows[0]["StockRcvDate"].ToString());
                                        if (!string.IsNullOrEmpty(dtdcinfo.Rows[0]["ReqId"].ToString()))
                                        {
                                            aDcStockNew.ReqId = string.IsNullOrEmpty(dtdcinfo.Rows[0]["ReqId"].ToString())
                                                ? 0
                                                : Convert.ToInt32(dtdcinfo.Rows[0]["ReqId"].ToString());
                                        }
                                        if (!string.IsNullOrEmpty(dtdcinfo.Rows[0]["ReqChildId"].ToString()))
                                        {
                                            aDcStockNew.ReqChildId =
                                                Convert.ToInt32(dtdcinfo.Rows[0]["ReqChildId"].ToString());
                                        }
                                        if (!string.IsNullOrEmpty(dtdcinfo.Rows[0]["StockInTransfarId"].ToString()))
                                        {
                                            aDcStockNew.StockInTransfarId =
                                                Convert.ToInt32(dtdcinfo.Rows[0]["StockInTransfarId"].ToString());
                                        }
                                        if (!string.IsNullOrEmpty(dtdcinfo.Rows[0]["ChalanDetailsId"].ToString()))
                                        {
                                            aDcStockNew.ChalanDetailsId =
                                                Convert.ToInt32(dtdcinfo.Rows[0]["ChalanDetailsId"].ToString());
                                        }
                                    }
                                    ;

                                    {
                                        aInvoiceBll.SaveDCStoreFreeze(aDcStockNew);
                                    }
                                }
                            }
                            aCustomerReturnBll.ApprovalUpdateDAL(aCustomerReturnMasterDao);

                            // Journal Posting
                            aCustomerReturnBll.PostDeliverySalesReturn(aCustomerReturnMasterDao.CustReturnId);
                        }

                    }
                }
                showMessageBox("Your " + actionRadioButtonList.SelectedItem.Text + " is Successfully Done");
            }
        }
    }

    private bool Validation()
    {
        InvoiceBLL aInvoiceBll = new InvoiceBLL();
        DataTable aTable = aInvoiceBll.GetFinancialYearInfo(DateTime.Today.ToString(CultureInfo.InvariantCulture), loadGridView.DataKeys[0][2].ToString());

        if (aTable.Rows.Count == 0)
        {
            showMessageBox("You should declare financial year !!");
            return false;
        }


        DataTable aTable1 = aInvoiceBll.CheckChartOfAccount(loadGridView.DataKeys[0][3].ToString());

        if (aTable1.Rows.Count == 0)
        {
            showMessageBox("Customer does not exist in chart of accounts !!");
            return false;
        }


        return true;

    }
}