using System;
using System.Collections.Generic;
using System.Data;
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
                //GridView();
                
            }
        }
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
            aTable = aOrderInfoBll.LoadOrderForOrderCreation(Session["MarketId"].ToString());
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

    public void GridView(string parametr)
    {
        DataTable aTable = aOrderInfoBll.LoadOrderForOrderCreation(parametr);

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

    public string GenerateParameter()
    {
        string param = "";

        if (manufacDropDownList.SelectedIndex != 0)
        {
            param = param + "  AND UNT.CompanyId='" + manufacDropDownList.SelectedValue + "' ";

        }
        if (salesCenterDropDownList.SelectedIndex > 0)
        {
            param = param + " AND UNT.ComUnitId='" + salesCenterDropDownList.SelectedValue + "'";
        }

        if (areaDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ODR.AreaId='" + areaDropDownList.SelectedValue + "'";
        }
       
        if (territoryDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ODR.TerritoryId='" + territoryDropDownList.SelectedValue + "'";
        }

        if (marketDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ODR.MarketId='" + marketDropDownList.SelectedValue + "'";
        }

        if (orderDateTextBox.Text !=string.Empty)
        {
            param = param + " AND ODR.SubmissionDate='" + orderDateTextBox.Text + "'";
        }

        if (custDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ODR.CustomerMasterId='" + custDropDownList.SelectedValue + "'";
        }


//        param = param + @" AND (
//    CASE 
//        WHEN TermOfPayment = 'Credit' THEN
//            CASE 
//                WHEN ISNULL(LimitAmount, 0) = 0 THEN 0  -- No limit set No permission
//                WHEN 
//                    (ISNULL(GrossValue, 0) + ISNULL(Due, 0) <= ISNULL(LimitAmount, 0)) AND
//                    (ISNULL(DayLimit, 0) = 0 OR DATEDIFF(day, InvoiceDate, GETDATE()) <= ISNULL(DayLimit, 0))
//                THEN 1
//                ELSE 0
//            END
//        ELSE 1
//    END
//) = 1";

        

        return param;
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

        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserType"].ToString() != "Admin")
            {
                if (orderDateTextBox.Text != "")
                {
                    if (manufacDropDownList.SelectedValue != "")
                    {
                        if (salesCenterDropDownList.SelectedValue != "")
                        {
                            GridView(GenerateParameter());
                        }
                        else
                        {
                            ShowMessageBox("Please select DC !!!");
                        }
                    }
                    else
                    {
                        ShowMessageBox("Please select company !!!");
                    }
                }
                else
                {
                    ShowMessageBox("Order date is required !!!");
                }
            }
            else
            {
                GridView(GenerateParameter());
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }


        
       
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
