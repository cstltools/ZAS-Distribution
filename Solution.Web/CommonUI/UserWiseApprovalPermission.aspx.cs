using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.Panal_BLL;
using Library.DAO.Panal_Entities;

public partial class CommonUI_UserWiseApprovalPermission : System.Web.UI.Page
{
    PanalBLL aPanalBll = new PanalBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            aPanalBll.UserDdl(userDropDownList);
           
        }
    }

    private void LoadAction(string ManuSL)
    {
        DataTable dt = aPanalBll.GetActionPageWiseStepBLL(ManuSL);
        aPanalBll.LoadActionBLL(actionRadioButtonList);
        for (int i = 0; i < actionRadioButtonList.Items.Count; i++)
        {
            if (dt.Rows.Count>0)
            {
                if (actionRadioButtonList.Items[i].Text == "Cancel")
                {
                    actionRadioButtonList.Items[i].Enabled = false;

                }

                if (dt.Rows[0]["ASId"].ToString()=="1")
                {
                    if (actionRadioButtonList.Items[i].Text == "Verified")
                    {
                        actionRadioButtonList.Items[i].Enabled = false;

                    }
                }
            }
            else
            {
                actionRadioButtonList.Items[i].Enabled = false;
            }
            
            
        }
    }

    protected void userDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aPanalBll.MainMenu(mainMenuDropDownList, userDropDownList.SelectedValue);
    }
  
    private void MenuLoad(string userId, string parantId)
    {
        aPanalBll.ApprovalManuDropDownBLL(appMenuDropDownList, userId, parantId);
    }
   
    protected void mainMenuDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        MenuLoad(userDropDownList.SelectedValue, mainMenuDropDownList.SelectedValue);

        
    }

    private bool Validation()
    {
        if (string.IsNullOrEmpty(userDropDownList.SelectedValue))
        {
            showMessageBox("Select User");
            return false;
        }
        if (string.IsNullOrEmpty(mainMenuDropDownList.SelectedValue))
        {
            showMessageBox("Select Manu");
            return false;
        }
        if (string.IsNullOrEmpty(appMenuDropDownList.SelectedValue))
        {
            showMessageBox("Select Approval Manu");
            return false;
        }
        int actionRadioButton = 0;
        for (int i = 0; i < actionRadioButtonList.Items.Count; i++)
        {
            if (actionRadioButtonList.Items[i].Selected==true)
            {
                actionRadioButton++;
            }
        }
        if (actionRadioButton==0)
        {
            showMessageBox("Select Action");
            return false;
        }
        return true;
    }
    private void Save()
    {
        ObjUserWiseApprovalPermission approvalPermission = new ObjUserWiseApprovalPermission();
        approvalPermission.UserId = Convert.ToInt32(userDropDownList.SelectedValue);
        approvalPermission.LoginName = userDropDownList.SelectedItem.Text.ToLower();
        approvalPermission.ManuSL = Convert.ToInt32(appMenuDropDownList.SelectedValue);
        approvalPermission.ActionId = Convert.ToInt32(actionRadioButtonList.SelectedValue);
        if (aPanalBll.UserWiseApprovalPermissionBLL(approvalPermission))
        {
            showMessageBox("Save Action Successfully");
            Clear();
        }
       
    }
    private void Clear()
    {
        userDropDownList.SelectedIndex = 0;
        mainMenuDropDownList.SelectedIndex = 0;
        appMenuDropDownList.SelectedIndex = 0;
        lblMsg.Text = "";
        //LoadAction();
    }
    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation()==true)
        {
            Save();
            
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void appMenuDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        lblMsg.Text = "";
        if (!string.IsNullOrEmpty(appMenuDropDownList.SelectedValue))
        {
            LoadAction(appMenuDropDownList.SelectedValue);
             ObjUserWiseApprovalPermission approvalPermission = new ObjUserWiseApprovalPermission();
            approvalPermission.UserId = Convert.ToInt32(userDropDownList.SelectedValue);
            approvalPermission.ManuSL = Convert.ToInt32(appMenuDropDownList.SelectedValue);

            DataTable dt = aPanalBll.GetManuWiseSelectedActionBLL(approvalPermission);

            if (dt.Rows.Count>0)
            {
                actionRadioButtonList.SelectedValue = dt.Rows[0]["ActionId"].ToString();
                lblMsg.Text = "One Action Selected";
                lblMsg.ForeColor = System.Drawing.Color.DarkGreen;
            }
            else
            {
                for (int i = 0; i < actionRadioButtonList.Items.Count; i++)
                {
                    actionRadioButtonList.Items[i].Selected = false;
                    lblMsg.Text = "No Action Selected";
                    lblMsg.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
        }
        else
        {
            for (int i = 0; i < actionRadioButtonList.Items.Count; i++)
            {
                actionRadioButtonList.Items[i].Selected = false;
                lblMsg.Text = "No Action Selected";
                lblMsg.ForeColor = System.Drawing.Color.DarkRed;
            }
        }
    }
}