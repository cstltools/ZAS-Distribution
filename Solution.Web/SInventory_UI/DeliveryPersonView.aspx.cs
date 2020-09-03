using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_DeliveryPersonView : System.Web.UI.Page
{
    DeliveryPersonBLL aDeliveryPersonBll=new DeliveryPersonBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    public void LoadData()
    {
        DataTable dt = aDeliveryPersonBll.LoadDelivaryMan();
        loadGridView.DataSource=dt;
        loadGridView.DataBind();


    }
    
    protected void departmentNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("DeliveryPersonEntry.aspx");
    }
    protected void deptReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
    }
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string id= loadGridView.DataKeys[rowindex][0].ToString();
            Session["DeliveryManId"] = id;
            Response.Redirect("DeliveryPersonEntry.aspx");
            
        }

    }
}