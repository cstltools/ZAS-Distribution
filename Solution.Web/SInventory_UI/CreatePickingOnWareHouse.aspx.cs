using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_CreatePickingOnWareHouse : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            yearTextBox.Text = DateTime.Now.Year.ToString();
            LoadGrid();
        }
    }
    private void LoadGrid()
    {
        int? year = null;
        int parsedYear;
        if (int.TryParse(yearTextBox.Text.Trim(), out parsedYear))
        {
            year = parsedYear;
        }

        aDataTable = aRequisitionBll.GetAllNonPickReq(year);
        viewReqGridView.DataSource = aDataTable;
        viewReqGridView.DataBind();
    }
    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }
}