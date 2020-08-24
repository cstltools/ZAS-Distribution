using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_SlaveInfoApproval : System.Web.UI.Page
{
    SlaveApprovalDal approvalDal = new SlaveApprovalDal();
    DataTable aDataTable = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SlaveInformationLoad();
        }
    }

    private void SlaveInformationLoad()
    {
        string filename = Path.GetFileName(Request.Path);
        string userName = Session["LoginName"].ToString();
        CreditLimitCondition(filename, userName);
        approvalDal.LoadApprovalControlDAL(actionRadioButtonList, filename, userName);

    }
    private void CreditLimitCondition(string pageName, string user)
    {
        string ActionStatus = approvalDal.LoadForApprovalConditionDAL(pageName, user);
        aDataTable = approvalDal.LoadSlavePostedInfo(ActionStatus);
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
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
        SlaveInformationLoad();
    }
    private void SubmitApproval()
    {

        if (actionRadioButtonList.SelectedValue == "")
        {
            ShowMessageBox("Please Choose any of the operation");
        }

        else
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {

                CheckBox cb = (CheckBox)loadGridView.Rows[i].Cells[5].FindControl("chkSelect");
                if (cb.Checked == true)
                {
                    SlaveInformation aLimitDao = new SlaveInformation()
                    {
                        SlaveId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()),
                        ActionStatus = actionRadioButtonList.SelectedItem.Text,
                        ApprovedBy = Session["LoginName"].ToString(),
                        ApprovedDate = System.DateTime.Today,
                    };


                    if (actionRadioButtonList.SelectedValue != null)
                    {
                        approvalDal.ApprovalUpdateDAL(aLimitDao);
                    }

                }
            }
            ShowMessageBox("Your " + actionRadioButtonList.SelectedItem.Text + " is Successfully Done");
        }

    }
}