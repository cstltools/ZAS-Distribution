using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAL.InternalCls;

public partial class SInventory_UI_TradeMaterialTransferApproval : System.Web.UI.Page
{
    private TradeMaterialTransferDal aTradeMaterialTransferDal = new TradeMaterialTransferDal();
    private ClsApprovalAction approvalAction = new ClsApprovalAction();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginName"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            ManualOrderLoad();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (actionRadioButtonList.SelectedValue == "")
        {
            ShowMessageBox("Please choose any action first.");
            return;
        }

        if (Validation())
        {
            string approvedBy = Session["LoginName"].ToString();
            int successCount = 0;
            int totalChecked = 0;

            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

                if (cb != null && cb.Checked)
                {
                    totalChecked++;
                    int tradeMaterialTransferId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());

                    if (aTradeMaterialTransferDal.ApproveTradeMaterialTransfer(tradeMaterialTransferId, approvedBy))
                    {
                        successCount++;
                    }
                }
            }

            if (successCount == totalChecked)
            {
                ShowMessageBox("Selected trade material transfers approved and stock adjusted successfully!");
            }
            else if (successCount > 0)
            {
                ShowMessageBox(string.Format("{0} out of {1} transfers approved successfully.", successCount, totalChecked));
            }
            else
            {
                ShowMessageBox("Approval failed.");
            }

            ManualOrderLoad();
        }
        else
        {
            ShowMessageBox("You have to select at least one row!");
        }
    }

    public void ManualOrderLoad()
    {
        string filename = System.IO.Path.GetFileName(Request.Path);
        string userName = Session["LoginName"].ToString();
        
        // Dynamically load the action control based on user permissions
        approvalAction.LoadActionControlByUser(actionRadioButtonList, filename, userName);
        
        // Load the approval status condition for this user
        string actionStatus = approvalAction.LoadForApprovalByUserCondition(filename, userName);
        
        if (string.IsNullOrEmpty(actionStatus))
        {
            // If user has no approval step configured for this page, load empty grid
            loadGridView.DataSource = new DataTable();
            loadGridView.DataBind();
        }
        else
        {
            // Load pending transfers matching this workflow step status (e.g. 'Posted')
            DataTable dt = aTradeMaterialTransferDal.GetPendingTradeMaterialTransferListByStatus(actionStatus);
            loadGridView.DataSource = dt;
            loadGridView.DataBind();
        }
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        int count = 0;

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

            if (cb != null && cb.Checked)
            {
                count++;
                break;
            }
        }

        return count > 0;
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            if (ChkBoxRows != null)
            {
                ChkBoxRows.Checked = ChkBoxHeader.Checked;
            }
        }
    }
}
