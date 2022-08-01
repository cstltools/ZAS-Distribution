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

public partial class SInventory_UI_DestroyProductApproval : Page
{

    DestroyProductDal aDal = new DestroyProductDal();
    DataTable aDataTable = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDestroyList();
        }
        
    }

    protected void   btnSubmit0_Click(object sender, EventArgs e)
    {
        SubmitApproval();
        LoadDestroyList();
    }

    public void LoadDestroyList()
    {
        string filename = Path.GetFileName(Request.Path);
        string userName = Session["LoginName"].ToString();
        ManualOrderCondition(filename, userName);
        aDal.LoadApprovalControlDAL(RadioButtonList, filename, userName);

    }
    private void ManualOrderCondition(string pageName, string user)
    {
        string ActionStatus = aDal.LoadForApprovalConditionDAL(pageName, user);
        
        aDataTable = aDal.GetSpecialApproveOrder(ActionStatus);
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

        if (RadioButtonList.SelectedValue == "")
        {
            ShowMessageBox("Please Choose any of the operation !!!");
        }
        else
        {
            if (Validation())
            {
                for (int i = 0; i < loadGridView.Rows.Count; i++)
                {

                    var cb = (CheckBox)loadGridView.Rows[i].Cells[7].FindControl("chkSelect");

                    if (cb.Checked)
                    {
                        var aOrderInfoMaster = new DestroyProductDao()
                        {
                            DcFreezeId = Convert.ToInt32(loadGridView.DataKeys[i][1].ToString()),
                            DestroyId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()),
                            ActionStatus = RadioButtonList.SelectedItem.Text,
                            ApproveBy = Session["LoginName"].ToString(),
                            ApproveDate = DateTime.Today,
                        };


                        if (RadioButtonList.SelectedValue != "")
                        {
                            if (aDal.ApprovalUpdateDAL(aOrderInfoMaster))
                            {

                                if (RadioButtonList.SelectedItem.Text.Trim() != "Cancel")
                                {
                                    DataTable aTable = aDal.FetchDcFreezeQty(aOrderInfoMaster.DcFreezeId);

                                    decimal qty = 0;

                                    if (aTable.Rows.Count > 0)
                                    {
                                        Label lblName = (Label)loadGridView.Rows[i].FindControl("StockOutQty");
                                        decimal dt = aTable.Rows[0].Field<Decimal>("StockQty");

                                        qty = dt - Convert.ToDecimal(loadGridView.DataKeys[i][2].ToString());

                                        aDal.UpdateFreezeStockQty(aOrderInfoMaster.DcFreezeId, qty);

                                        //Journal Posting

                                        aDal.PostJournal(Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()));
                                    }

                                }
                                
                                
                            }
                           
                        }

                    }
                }


                ShowMessageBox("Your " + RadioButtonList.SelectedItem.Text + " is Successfully Done !!");
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
            CheckBox cb = (CheckBox)loadGridView.Rows[i].Cells[7].FindControl("chkSelect");

            if (cb.Checked)
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
            return true;
        }

        return false;
    }


    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[9].FindControl("chkSelect");
            
            if (ChkBoxHeader.Checked)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

  

   
}