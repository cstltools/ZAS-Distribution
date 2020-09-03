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

public partial class SInventory_UI_ProUnitPriceApprove : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    UnitPriceBLL aUnitPriceBll=new UnitPriceBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProPriceLoad();
        }
    }
    public void ProPriceLoad()
    {
        string filename = Path.GetFileName(Request.Path);
        string userName = Session["LoginName"].ToString();
        ProPriceCondition(filename, userName);
        aUnitPriceBll.LoadApprovalControlBLL(actionRadioButtonList, filename, userName);

    }
    private void ProPriceCondition(string pageName, string user)
    {
        string ActionStatus = aUnitPriceBll.LoadForApprovalConditionBLL(pageName, user);
        aDataTable = aUnitPriceBll.LoadProUnitPrice(ActionStatus);
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
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[4].FindControl("chkSelect");
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
        ProPriceLoad();
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

                    CheckBox cb = (CheckBox)loadGridView.Rows[i].Cells[7].FindControl("chkSelect");
                    if (cb.Checked == true)
                    {
                        ProductUnitPrice aProductUnitPrice = new ProductUnitPrice();
                        {
                            aProductUnitPrice.UnitPriceId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());
                            aProductUnitPrice.ActionStatus = actionRadioButtonList.SelectedItem.Text;
                            aProductUnitPrice.ApproveBy = Session["LoginName"].ToString();
                            aProductUnitPrice.ApproveDate = System.DateTime.Today;
                        };


                        if (actionRadioButtonList.SelectedValue != null)
                        {
                            aUnitPriceBll.ApprovalUpdateDAL(aProductUnitPrice);
                        }

                    }
                }
                showMessageBox("Your " + actionRadioButtonList.SelectedItem.Text + " is Successfully Done");
            }


        }
        else
        {
            showMessageBox("You have to select at least one product !!");
        }
       

        
    }

    private bool Validation()
    {
        Int32 count = 0;
        bool status = false;

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)loadGridView.Rows[i].Cells[7].FindControl("chkSelect");
            
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
            status = true;
        }

        return status;
    }
}