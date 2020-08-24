using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_SlaveInformationView : System.Web.UI.Page
{
    SlaveinformationBll aBll = new SlaveinformationBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadSlaveInformation();
        }
    }


    private void LoadSlaveInformation()
    {
        DataTable aTable = aBll.LoadSlaveInformation();

        loadGridView.DataSource = aTable;
        loadGridView.DataBind();
    }


    protected void unitPriceNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("SlaveInformation.aspx");
    }

    protected void unitPriceReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        LoadSlaveInformation();
    }


    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            var dataKey = loadGridView.DataKeys[rowindex];

            if (dataKey != null)
            {
                string asmId = dataKey[0].ToString();

                Session["SlaveId"] = "";
                Session["SlaveId"] = asmId;

                Response.Redirect("SlaveInformation.aspx");
            }
        }
    }
}