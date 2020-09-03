using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_DesignationView : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
   DesignationBll aDesignationBll = new DesignationBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DepartmentLoad();
        }
    }

    private void DepartmentLoad()
    {
        aDataTable = aDesignationBll.LoadDesignationInfo();
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    protected void deptReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        DepartmentLoad();
    }

    protected void departmentNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("DesignationInfo.aspx");
    }
    
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string departmentId = loadGridView.DataKeys[rowindex][0].ToString();
            
            Session["DESGId"] = "";
            Session["DESGId"] = departmentId;

            Response.Redirect("DesignationInfo.aspx");
        }

    }
}