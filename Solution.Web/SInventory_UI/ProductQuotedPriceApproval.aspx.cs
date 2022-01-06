using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;
using Microsoft.VisualBasic.CompilerServices;

public partial class SInventory_UI_ManualOrderApproval : System.Web.UI.Page
{
    OrderInfoBLL aOrderInfoBll=new OrderInfoBLL();
    OrderListBLL aOrderListBll=new OrderListBLL();
    ProductDiscountBLL aProductDiscountBll=new ProductDiscountBLL();
    DataTable aDataTable=new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ManualOrderLoad();
        }
    }
    protected void btnSubmit0_Click(object sender, EventArgs e)
    {
        SubmitApproval();
        ManualOrderLoad();
    }
    public void ManualOrderLoad()
    {
        string filename = Path.GetFileName(Request.Path);
        string userName = Session["LoginName"].ToString();
        ManualOrderCondition(filename, userName);
        aProductDiscountBll.LoadApprovalControlBLL(actionRadioButtonList, filename, userName);

    }
    private void ManualOrderCondition(string pageName, string user)
    {
        string ActionStatus = aProductDiscountBll.LoadForApprovalConditionBLL(pageName, user);
        aDataTable = aProductDiscountBll.GetProductQuotedPriceApproval(ActionStatus);
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    private void SubmitApproval()
    {

        if (actionRadioButtonList.SelectedValue == "")
        {
            ShowMessageBox("Please Choose any of the operation !!");
        }

        else
        {

            if (Validation())
            {
                for (int i = 0; i < loadGridView.Rows.Count; i++)
                {

                    var cb = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

                    if (cb.Checked)
                    {
                        var dataKey = loadGridView.DataKeys[i];
                        if (dataKey != null)
                        {
                            var aOrderInfoMaster = new QuotedPriceSetup()
                            {
                                QuotedPriceId = Convert.ToInt32(dataKey[0].ToString()),
                                ActionStatus = actionRadioButtonList.SelectedItem.Text,
                                ApprovedBy = Session["LoginName"].ToString(),
                                ApprovedDate = DateTime.Today,
                            };


                            if (actionRadioButtonList.SelectedValue != null)
                            {
                                aProductDiscountBll.QuotedPriceApproval(aOrderInfoMaster);
                            }
                        }
                    }
                }
                ShowMessageBox("Your " + actionRadioButtonList.SelectedItem.Text + " is Successfully Done !!");
            }
            else
            {
                ShowMessageBox("You have to sslect at least one row !!!");
            }
            
        }

    }

    private bool Validation()
    {
        int count = 0;
         
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox) loadGridView.Rows[i].FindControl("chkSelect");

            if (cb.Checked)
            {
                count ++;
            }

            if (count > 0)
            {
                break;
            }

        }

        if (count > 0)
        {
            return true;
        }

        return false;
    }


    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
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

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        var dataKey = loadGridView.DataKeys[rowindex];
        //if (dataKey != null)
            //PopupReport(dataKey["CustPayDetailId"].ToString());
        ModalPopupExtender10.Show();

        LoadDueHistory(dataKey["CustomerMasterId"].ToString());

    }

    private void LoadDueHistory(string customerMasterId)
    {
        DataTable aTable = aOrderInfoBll.LoadCustomerDueHistory(customerMasterId);

        if (aTable.Rows.Count > 0)
        {
            dueGridView.DataSource = aTable;
            dueGridView.DataBind();
        }
        else
        {
            dueGridView.DataSource = null;
            dueGridView.DataBind();
        }
    }

    protected void Button41_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender10.Hide();
    }
}