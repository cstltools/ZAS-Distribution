using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.Panal_BLL;

public partial class Login : System.Web.UI.Page
{
    DataTable aTableLogin = new DataTable();
    PanalBLL aPanalBll = new PanalBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void loginButton_Click(object sender, EventArgs e)
    {
        msgLabel.Text = "";
        string loginName = string.Empty;
        string passwordText = string.Empty;
        if (!string.IsNullOrEmpty(userNameTextBox.Text.Trim()))
        {
            loginName = userNameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(passwordTextBox.Text.Trim()))
            {
                passwordText = passwordTextBox.Text.Trim();
                aTableLogin = aPanalBll.Login(loginName, passwordText);
                if (aTableLogin.Rows.Count > 0)
                {
                    Session["UserId"] = aTableLogin.Rows[0]["UserId"].ToString().Trim();
                    Session["LoginName"] = aTableLogin.Rows[0]["LoginName"].ToString().Trim();
                    Session["UserType"] = aTableLogin.Rows[0]["UserType"].ToString().Trim();
                    Session["CentralWareHouse"] = aTableLogin.Rows[0]["CentralWareHouse"].ToString().Trim();
                    if (aTableLogin.Rows[0]["UserType"].ToString().Trim()!="Admin")
                    {
                        Session["ComUnitId"] = aTableLogin.Rows[0]["CompanyUnitId"].ToString().Trim();
                    }
                    Session["UserTime"] = DateTime.Now.ToString("f");
                    aPanalBll.LoginLog(Session["UserId"].ToString(), Session["LoginName"].ToString(), DateTime.Now);
                    Response.Redirect("CommonUI/HomePage.aspx");
                }
                else
                {
                    msgLabel.Text = "User name or password incourect !!!!";
                }
            }
            else
            {
                msgLabel.Text = "Input Password Please!!!";
            }
        }
        else
        {
            msgLabel.Text = "Input Use Name Please!!!";
        }
    }
}