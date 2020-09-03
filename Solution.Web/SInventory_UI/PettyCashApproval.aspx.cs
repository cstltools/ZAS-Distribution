using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.PettyCash_BLL;
using Library.DAO.PettyCashDao;


public partial class SInventory_UI_PettyCashApprovalaspx : System.Web.UI.Page
{

    PettyCashBLL aCashBll = new PettyCashBLL();

    DataTable aDataTable = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PettyCashLoad();
        }
    }
    //
    protected void btnSubmit0_Click(object sender, EventArgs e)
    {
        SubmitApproval();
        PettyCashLoad();
    }
    public void PettyCashLoad()
    {
        string filename = Path.GetFileName(Request.Path);
        string userName = Session["LoginName"].ToString();
        PettyCashLoadrCondition(filename, userName);
        aCashBll.LoadApprovalControlBLL(actionRadioButtonList, filename, userName);

    }
    private void PettyCashLoadrCondition(string pageName, string user)
    {
        string ActionStatus = aCashBll.LoadForApprovalConditionBLL(pageName, user);
        aDataTable = aCashBll.GetSpecialApproveOrder(ActionStatus);
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    private void SubmitApproval()
    {

        if (actionRadioButtonList.SelectedValue == "")
        {
            ShowMessageBox("Please Choose any of the operation");
        }

        else
        {

            if (Validation())
            {
                for (int i = 0; i < loadGridView.Rows.Count; i++)
                {

                    var cb = (CheckBox)loadGridView.Rows[i].Cells[8].FindControl("chkSelect");

                    if (cb.Checked)
                    {
                        var dataKey = loadGridView.DataKeys[i];
                        if (dataKey != null)
                        {
                           

                            var aPettycashInfoMaster = new PettyCashMasterDao();
                            {

                                aPettycashInfoMaster.PettyCashmasterId = Convert.ToInt32(dataKey[0].ToString());
                                aPettycashInfoMaster.ActiveStatus = actionRadioButtonList.SelectedItem.Text;
                                aPettycashInfoMaster.ApprovedBy = Session["LoginName"].ToString();
                                aPettycashInfoMaster.ApprovedDate = DateTime.Today;
                            };


                            if (actionRadioButtonList.SelectedValue != null)
                            {
                                aCashBll.ApprovalUpdateDAL(aPettycashInfoMaster);
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
            CheckBox cb = (CheckBox) loadGridView.Rows[i].Cells[8].FindControl("chkSelect");

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
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[13].FindControl("chkSelect");
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

    //

  
}