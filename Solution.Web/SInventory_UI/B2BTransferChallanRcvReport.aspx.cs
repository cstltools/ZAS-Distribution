using System;
using System.Activities.Expressions;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_TransferStockReceiveByDC : Page
{
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    ChalanBLL aChalanBll=new ChalanBLL();
    DataTable aDataTable = new DataTable();

    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            LoadDropdownList();
            
            
            //if (Session["UserType"].ToString()!="")
            //{
            //    if (Session["UserType"].ToString() == "Admin")
            //    {
            //        aRequisitionBll.DCLoad(dcDropDownList);
            //    }
            //    else
            //    {
            //        string comUnit = Session["ComUnitId"].ToString();
            //        aRequisitionBll.DCLoad(dcDropDownList, Session["UserId"].ToString()); 
            //    }
            //}
            //else
            //{
            //    Response.Redirect("Login.aspx");
            //}  
        }
    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aReportDal.LoadDcInfoByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        else
        {
            dcDropDownList.Items.Clear();
        }
    }

    private void LoadDropdownList()
    {

        aReportDal.LoadGroupInfo(groupDropDownList);

        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);


        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();

        }

    }
  
    private void LoadGrid(string comUnitId)
    {
        string parameter = "";

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            parameter = parameter + " AND CLN.ChalanDate BETWEEN '" + fromDateTextBox.Text.Trim() + "' AND '" +
                        toDateTextBox.Text.Trim() + "'";
        }


        aDataTable = aChalanBll.ChalanReceiveReport(comUnitId,parameter);

        if (aDataTable.Rows.Count>0)
        {
            stockInTraGridView.DataSource = null;
            stockInTraGridView.DataBind();
            stockInTraGridView.DataSource = aDataTable;
            stockInTraGridView.DataBind();
        }
        else
        {
            stockInTraGridView.DataSource = null;
            stockInTraGridView.DataBind();
            showMessageBox("No Data Found!!");
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        if (dcDropDownList.SelectedValue != "")
        {
            LoadGrid(dcDropDownList.SelectedValue);
        }
        else
        {
            showMessageBox("Please select DC !!!");
            stockInTraGridView.DataSource = null;
            stockInTraGridView.DataBind();
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {

        LoadDropdownList();
        companyNameDropDownList.SelectedValue = "";
        dcDropDownList.Items.Clear();
    }


    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        string url = "../SInventory_RPTVIEW/TransferStockReceiveReportViewer.aspx?ChalanId=" + stockInTraGridView.DataKeys[rowindex]["ChalanNo"];
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
}