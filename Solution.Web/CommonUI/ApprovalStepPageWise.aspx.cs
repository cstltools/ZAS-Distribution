using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.Panal_BLL;
using Library.DAO.Panal_Entities;

public partial class CommonUI_ApprovalStepPageWise : System.Web.UI.Page
{
    PanalBLL aPanalBll = new PanalBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            aPanalBll.MainMenuDropdown(ddlMMenuNameMPanal);
        }
    }
    protected void ddlMMenuNameMPanal_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }

    private void LoadData()
    {
        DataTable aTable = aPanalBll.ApprovalMenuLoadBLL(ddlMMenuNameMPanal.SelectedValue);
        appGridView.DataSource = null;
        appGridView.DataBind();
        appGridView.DataSource = aTable;
        appGridView.DataBind();
        if (appGridView.Rows.Count > 0)
        {
            LoadActionStepDropDown(appGridView);
        }
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private void LoadActionStepDropDown(GridView gridView)
    {
        int manuSL = 0;
        for (int i = 0; i < gridView.Rows.Count; i++)
        {
            DropDownList dropDownList = (DropDownList) gridView.Rows[i].Cells[1].FindControl("asDropDownList");
            aPanalBll.ActionStepDropDownBLL(dropDownList);
            manuSL = Convert.ToInt32(gridView.DataKeys[i][0].ToString());
            DataTable dt = aPanalBll.GetApprovalPageStepsBLL(manuSL);

            if (dt.Rows.Count>0)
            {
                dropDownList.SelectedValue = dt.Rows[0]["ASId"].ToString();
            }
            else
            {
                appGridView.Rows[i].BackColor = System.Drawing.Color.DarkOrange;
            }
        }

    }
    protected void submitButton_Click(object sender, EventArgs e)
    {
        int manuSL = 0;
        for (int i = 0; i < appGridView.Rows.Count; i++)
        {
            manuSL = Convert.ToInt32(appGridView.DataKeys[i][0].ToString());

            ObjActionPageWiseStep actionPageWise;
            DropDownList dropDownList = (DropDownList)appGridView.Rows[i].Cells[1].FindControl("asDropDownList");
            if (!string.IsNullOrEmpty(dropDownList.SelectedValue))
            {
                actionPageWise = new ObjActionPageWiseStep();
                actionPageWise.ManuSL = manuSL;
                actionPageWise.ASId = Convert.ToInt32(dropDownList.SelectedValue);
                aPanalBll.ApprovalPageStepsDeleteBLL(actionPageWise.ManuSL);
                
                    if (aPanalBll.ApprovalPageStepsSaveBLL(actionPageWise))
                    {
                        showMessageBox("Step Save Successfully");
                    }
                
                
               
            }
            else
            {
                actionPageWise = new ObjActionPageWiseStep();
                actionPageWise.ManuSL = manuSL;
                if (aPanalBll.ApprovalPageStepsDeleteBLL(actionPageWise.ManuSL))
                {
                    showMessageBox("Step Remove Successfully");
                }
                
            }
            

        }
        LoadData();
    }
}