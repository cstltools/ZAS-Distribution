using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_ProductDiscountView : System.Web.UI.Page
{
    ProductDiscountBLL aProductDiscountBll = new ProductDiscountBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProductdiscountLoad();
        }
    }

    private void ProductdiscountLoad()
    {
        DataTable aDataTable = aProductDiscountBll.ProductDiscountView();
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();
    }

    private void PopUp(string Id)
    {
        string url = "ProductEntry.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string productDiscountId = loadGridView.DataKeys[rowindex][0].ToString();
            PopUp(productDiscountId);
        }

        if (e.CommandName == "DeleteData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string productDiscountId = loadGridView.DataKeys[rowindex][0].ToString();
            Session["DiscountId"] = productDiscountId;

                if (aProductDiscountBll.DeleteProductDiscount(productDiscountId))
                {
                    ProductdiscountLoad();
                }
        }

    }
    protected void miaTargetReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ProductdiscountLoad();
    }
    protected void miaTargetNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProductDiscountEntry.aspx");
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void gotoinvoiceButton_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}