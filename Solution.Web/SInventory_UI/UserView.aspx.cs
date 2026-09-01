using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class HRM_UI_TDLUserView : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    UserBLL aUserBll = new UserBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserLoad();
        }
    }

    private void UserLoad(string searchText = "")
    {
        aDataTable = aUserBll.LoadUserInfo();
        if (!string.IsNullOrEmpty(searchText))
        {
            DataView dv = aDataTable.DefaultView;
            dv.RowFilter = string.Format("UserName LIKE '%{0}%' OR LoginName LIKE '%{0}%' OR Email LIKE '%{0}%'", searchText.Replace("'", "''"));
            loadGridView.DataSource = dv.ToTable();
        }
        else
        {
            loadGridView.DataSource = aDataTable;
        }
        loadGridView.DataBind();
    }

    private void PopUp(string Id)
    {
        string url = "UserEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void addImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("UserEntry.aspx");
    }
    protected void reloadLinkButton_Click(object sender, ImageClickEventArgs e)
    {
        txtSearch.Text = "";
        UserLoad();
    }
    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            string userId = e.CommandArgument.ToString();
            PopUp(userId);
        }
        if (e.CommandName == "DeleteData")
        {
            string userId = e.CommandArgument.ToString();
            bool ststus = aUserBll.DeleteUserInfo(userId);
            if (ststus == true)
            {
                ShowMessageBox("User Delete Successfully");
                UserLoad(txtSearch.Text.Trim());
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        UserLoad(txtSearch.Text.Trim());
    }

    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        UserLoad();
    }
}