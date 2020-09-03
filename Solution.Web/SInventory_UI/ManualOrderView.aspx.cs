using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_ManualOrderView : System.Web.UI.Page
{
    OrderListBLL aOrderListBll=new OrderListBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }
    private void PopUp(string Id)
    {
        string url = "ManualOrderEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string id = loadGridView.DataKeys[rowindex][0].ToString();
            Session["OrderId"] = id;
            Response.Redirect("ManualOrderCreation.aspx");
        }
    }
    public void LoadData()
    {
        DataTable dt = aOrderListBll.GetAllAvailableOrder();
        loadGridView.DataSource = dt;
        loadGridView.DataBind();
    }
    protected void custCetegoryAddImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ManualOrderCreation.aspx");
    }
    protected void custCetegoryReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
    }
}