using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_InvoiceCreationByOrder : System.Web.UI.Page
{
    OrderInfoBLL aOrderInfoBll=new OrderInfoBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownlist();
            salesCenterDropDownList_SelectedIndexChanged(sender, e);
            SessionChoose();
           
            if (Session["MarketId"] !=null)
            {
                marketDropDownList.SelectedValue = Session["MarketId"].ToString();
            }
        }
    }

    // showMessages=false: page load e chup chap skip kore, button click e alert dey
    private void LoadGrid(bool showMessages)
    {
        if (Session["UserType"] == null || Session["UserType"].ToString() == "")
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (Session["UserType"].ToString() == "Admin")
        {
            GridView(GenerateParameter());
            return;
        }

        if (orderDateTextBox.Text == "")
        {
            if (showMessages) ShowMessageBox("Order date is required !!!");
            return;
        }
        if (manufacDropDownList.SelectedValue == "")
        {
            if (showMessages) ShowMessageBox("Please select company !!!");
            return;
        }
        if (salesCenterDropDownList.SelectedValue == "")
        {
            if (showMessages) ShowMessageBox("Please select DC !!!");
            return;
        }

        GridView(GenerateParameter());
    }

    public void DropDownlist()
    {
        //aOrderInfoBll.LoadSC(salesCenterDropDownList,Session["UserId"].ToString());
        
        aOrderInfoBll.LoadManufac(manufacDropDownList);


        //manufacDropDownList.SelectedIndex = 1;
        //salesCenterDropDownList.SelectedIndex = 1;
    }
    protected void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aOrderInfoBll.AreaLoad(areaDropDownList,salesCenterDropDownList.SelectedValue);
        //aOrderInfoBll.LoadMarketOrderWise(marketDropDownList,salesCenterDropDownList.SelectedValue);
        orderGridView.DataSource = null;
        orderGridView.DataBind();
    }
    protected void manufacDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (manufacDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aOrderInfoBll.LoadDcInformationByUser(salesCenterDropDownList, manufacDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aOrderInfoBll.LoadDC(salesCenterDropDownList, manufacDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }
        else
        {
            salesCenterDropDownList.Items.Clear();
        }


        
    }
    protected void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aOrderInfoBll.CustomerLoad(custDropDownList,marketDropDownList.SelectedValue);
        //DataTable aTable = new DataTable();
        //aTable = aOrderInfoBll.LoadOrderForOrderCreation(salesCenterDropDownList.SelectedValue, manufacDropDownList.SelectedValue,
        //    marketDropDownList.SelectedValue);
        //orderGridView.DataSource = aTable;
        //orderGridView.DataBind();
    }
    protected void gotoinvoiceButton_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        GridViewRow currentRow = (GridViewRow)button.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        Session["OrderId"] = orderGridView.DataKeys[rowindex]["OrderId"].ToString();
        Response.Redirect("InvoiceCreationForCustomerByOrder.aspx");

    }

    public void SessionChoose()
    {
        DataTable aTable = new DataTable();
        if (Session["MarketId"] !=null)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@CompanyId", DBNull.Value),
                new SqlParameter("@ComUnitId", DBNull.Value),
                new SqlParameter("@AreaId", DBNull.Value),
                new SqlParameter("@TerritoryId", DBNull.Value),
                new SqlParameter("@MarketId", Session["MarketId"].ToString()),
                new SqlParameter("@SubmissionDate", DBNull.Value),
                new SqlParameter("@CustomerMasterId", DBNull.Value)
            };
            aTable = aOrderInfoBll.LoadOrderForOrderCreation(parameters);
            if (aTable.Rows.Count>0)
            {
                marketDropDownList.SelectedValue = Session["MarketId"].ToString();
            }
            else
            {
                marketDropDownList.SelectedIndex = 0;
            }
        }
        
    }

    public void GridView(List<SqlParameter> parameters)
    {
        DataTable aTable = aOrderInfoBll.LoadOrderForOrderCreation(parameters);

        if (aTable.Rows.Count > 0)
        {
            orderGridView.DataSource = aTable;
            orderGridView.DataBind();
            Session["MarketId"] = marketDropDownList.SelectedValue;


            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                var gotoinvoiceButton = (Button)orderGridView.Rows[i].Cells[1].FindControl("gotoinvoiceButton");

                if (orderGridView.DataKeys[i]["Permission"].ToString() == "1")
                {
                    gotoinvoiceButton.Enabled = true;
                    gotoinvoiceButton.Text = "Generate Invoice";
                    gotoinvoiceButton.ToolTip = "";
                }
                else
                {
                    gotoinvoiceButton.Enabled = false;
                    gotoinvoiceButton.BackColor = Color.DarkRed;
                    gotoinvoiceButton.Text = "Unable to Access";
                    gotoinvoiceButton.ToolTip = "Credit limit not set or requires extension.";
                }
            }
        }
        else
        {
            orderGridView.DataSource = null;
            orderGridView.DataBind();
        }
    }

    public List<SqlParameter> GenerateParameter()
    {
        var parameters = new List<SqlParameter>();

        parameters.Add(new SqlParameter("@CompanyId", (object)(manufacDropDownList.SelectedIndex != 0 ? manufacDropDownList.SelectedValue : null) ?? DBNull.Value));
        parameters.Add(new SqlParameter("@ComUnitId", (object)(salesCenterDropDownList.SelectedIndex > 0 ? salesCenterDropDownList.SelectedValue : null) ?? DBNull.Value));
        parameters.Add(new SqlParameter("@AreaId", (object)(areaDropDownList.SelectedIndex > 0 ? areaDropDownList.SelectedValue : null) ?? DBNull.Value));
        parameters.Add(new SqlParameter("@TerritoryId", (object)(territoryDropDownList.SelectedIndex > 0 ? territoryDropDownList.SelectedValue : null) ?? DBNull.Value));
        parameters.Add(new SqlParameter("@MarketId", (object)(marketDropDownList.SelectedIndex > 0 ? marketDropDownList.SelectedValue : null) ?? DBNull.Value));
        parameters.Add(new SqlParameter("@SubmissionDate", (object)(orderDateTextBox.Text != string.Empty ? orderDateTextBox.Text : null) ?? DBNull.Value));
        parameters.Add(new SqlParameter("@CustomerMasterId", (object)(custDropDownList.SelectedIndex > 0 ? custDropDownList.SelectedValue : null) ?? DBNull.Value));

        return parameters;
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        LoadGrid(true);
    }
    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        aOrderInfoBll.TerritoryLoad(territoryDropDownList, areaDropDownList.SelectedValue);
    }
    protected void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        aOrderInfoBll.MarketLoad(marketDropDownList,territoryDropDownList.SelectedValue);
    }
}
