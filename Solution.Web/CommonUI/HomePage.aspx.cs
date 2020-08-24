using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class CommonUI_HomePage : System.Web.UI.Page
{
    DashboardDal aDal = new DashboardDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadExpireProduct();
        }
    }

    private void LoadExpireProduct()
    {

        if (Session["UserId"] != null || Session["UserId"].ToString() != "")
        {
            DataTable dtTable = aDal.GetUserPermissionInfo(Session["UserId"].ToString());

            if (dtTable.Rows.Count > 0)
            {
                DataTable aTable = aDal.LoadExpireProductInfo();

                if (aTable.Rows.Count > 0)
                {
                    ModalPopupExtender10.Show();
                    dueGridView.DataSource = aTable;
                    dueGridView.DataBind();
                }
                else
                {
                    dueGridView.DataSource = null;
                    dueGridView.DataBind();
                }
            }
        }
    }

    protected void Button41_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender10.Hide();
    }

}