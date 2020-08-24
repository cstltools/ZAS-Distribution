using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
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
        aDataTable = aChalanBll.ChalanLoadInReceive(comUnitId);

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
}