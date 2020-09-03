using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_DestroyProductList : System.Web.UI.Page
{

    DestroyProductDal aDal = new DestroyProductDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
            LoadProductList();
        }
    }

    private void LoadProductList()
    {
        DataTable aTable = aDal.GetFreezeStockDataByCondition(GenerateParameter());

        if (aTable.Rows.Count > 0)
        {
            loadGridView.DataSource = aTable;
            loadGridView.DataBind();
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
        }
    }

    private void LoadDropDownList()
    {
        aDal.LoadManufac(manufacDropDownList);
        aDal.LoadStockCondition(stockConditionDropDownList);
        stockConditionDropDownList.SelectedValue = 3.ToString(CultureInfo.InvariantCulture);
        stockConditionDropDownList.Enabled = false;
    }

    protected void stockConditionDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductList();
    }

    private string GenerateParameter()
    {
        string param = "";

        
        if (manufacDropDownList.SelectedValue != "")
        {
            param = param + "  AND IUNT.CompanyId = '" + manufacDropDownList.SelectedValue + "' ";
        }

        if (stockConditionDropDownList.SelectedValue != "")
        {
            param = param + " AND F.StockCondition LIKE '%" + stockConditionDropDownList.SelectedItem.Text.Trim() + "%'";
        }

        return param;
    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        var dataKey = loadGridView.DataKeys[rowindex];

        DataTable aTable = aDal.CheckFreezeIdExistOrNot(dataKey["DCStoreFreezeId"].ToString());

        if (aTable.Rows.Count == 0)
        {
            Session["DcFreezeId"] = "";
            Session["DcFreezeId"] = dataKey["DCStoreFreezeId"].ToString();

            if (Session["DcFreezeId"] != null)
            {
                Response.Redirect("DestroyProduct.aspx");
            }
        }
        else
        {
            ShowMessageBox("Already Exist On Destroy List !!!!");
        }

        

    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void manufacDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductList();
    }

}