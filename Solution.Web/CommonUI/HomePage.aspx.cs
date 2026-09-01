using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class CommonUI_HomePage : System.Web.UI.Page
{
    DashboardDal aDal = new DashboardDal();
    OrderInfoDAL aOrderInfoDal = new OrderInfoDAL();
    OrderInfoBLL aOrderInfoBll = new OrderInfoBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadExpireProduct();
            //LoadPendingInvoice();
            SetOrderDateVisibility();

        }
    }

    private void SetOrderDateVisibility()
    {
        if (Session["UserId"] != null && Session["UserId"].ToString() != "")
        {
            DataTable dtTable = aDal.GetIsHeadInfo(Session["UserId"].ToString());

            order.Visible = dtTable.Rows.Count > 0;
            LoadPendingInvoice();
        }
    }

    protected void btnDue_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    private void LoadPendingInvoice()
    {
        if (Session["UserId"] != null && Session["UserId"].ToString() != "")
        {
            DataTable dtTable = aDal.GetIsHeadInfo(Session["UserId"].ToString());

            if (dtTable.Rows.Count > 0)
            {
                DataTable aTable = aOrderInfoDal.LoadOrderForOrderCreation(GenerateParameter());
                if (aTable.Rows.Count > 0)
                {
                    orderGridView.DataSource = aTable;
                    orderGridView.DataBind();

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
                            gotoinvoiceButton.ToolTip = "Credit limit extention required !!!";
                        }
                    }
                }
                else
                {
                    orderGridView.DataSource = null;
                    orderGridView.DataBind();
                }
            }
        }
    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        var dataKey = orderGridView.DataKeys[rowindex];
        //if (dataKey != null)
        //PopupReport(dataKey["CustPayDetailId"].ToString());
        ModalPopupExtender1.Show();

        LoadDueHistory(dataKey["CustomerMasterId"].ToString());

    }


    protected void OrderButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        var dataKey = orderGridView.DataKeys[rowindex];
        //if (dataKey != null)
        //PopupReport(dataKey["CustPayDetailId"].ToString());
        ModalPopupExtender2.Show();

        DataTable aTable = aOrderInfoBll.LoadOrderDetails(dataKey["OrderId"].ToString());

        if (aTable.Rows.Count > 0)
        {
            GridView1.DataSource = aTable;
            GridView1.DataBind();
            GridView1.Columns[2].HeaderStyle.Width = 20;
        }
        else
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
    }


    private void LoadDueHistory(string customerMasterId)
    {
        DataTable aTable = aOrderInfoBll.LoadCustomerDueHistory(customerMasterId);

        if (aTable.Rows.Count > 0)
        {
            dueGrid.DataSource = aTable;
            dueGrid.DataBind();

            dueGrid.Columns[2].HeaderStyle.Width = 20;
        }
        else
        {
            dueGrid.DataSource = null;
            dueGrid.DataBind();
        }
    }

    protected void gotoinvoiceButton_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        GridViewRow currentRow = (GridViewRow)button.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        Session["OrderId"] = orderGridView.DataKeys[rowindex]["OrderId"].ToString();
        Response.Redirect("~/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx");

    }

    public string GenerateParameter()
    {
        string param = "";

        if (orderDateTextBox.Text != string.Empty)
        {
            param = param + " AND ODR.SubmissionDate='" + orderDateTextBox.Text + "'";
        }

        if (Session["UserId"] != null && Session["UserId"].ToString() != "")
        {
            param = param + "  AND UNT.CompanyId IN (SELECT DISTINCT CI.CompanyId FROM tblUserCompanyUnit AS UNT LEFT JOIN tblCompanyUnit AS DC ON UNT.CompanyUnitId = DC.ComUnitId LEFT JOIN tblCompanyInfo AS CI ON DC.CompanyId = CI.CompanyId WHERE UserId = " + Session["UserId"].ToString() + ")";

        }

        param = param + " AND TermOfPayment = 'Credit' AND CASE WHEN TermOfPayment = 'Credit' THEN CASE WHEN CASE WHEN ISNULL(LimitAmount,0) > 0 AND ISNULL(Due,0) > 0 AND (ISNULL(GrossValue,0) + ISNULL(Due,0) >  ISNULL(LimitAmount,0)) THEN 0 ELSE 1 END = 1 AND CASE WHEN ISNULL(DayLimit,0) > 0 AND (ISNULL(DATEDIFF(day, InvoiceDate, GETDATE()), 0) > ISNULL(DayLimit,0)) THEN 0 ELSE 1 END = 1 THEN 1 ELSE 0 END ELSE 1 END = 0";
       

        return param;
    }

    private void LoadExpireProduct()
    {

        if (Session["UserId"] != null && Session["UserId"].ToString() != "")
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



    protected void Button42_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        LoadPendingInvoice();
    }
}