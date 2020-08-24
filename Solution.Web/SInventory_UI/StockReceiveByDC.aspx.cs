using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_StockReceiveByDC : System.Web.UI.Page
{
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    DataTable aDataTable = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

            aRequisitionBll.LoadCompanyDropdownList(companyNameDropDownList);
        }
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aRequisitionBll.LoadDcInformationByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aRequisitionBll.LoadDcInformation(dcDropDownList, companyNameDropDownList.SelectedValue); 
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

  
    private void LoadGrid(string comUnitId)
    {
        aDataTable = aRequisitionBll.GetAllStockRcvByDc(comUnitId);
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
            ShowMessageBox("No Data Found!!");
        }
    }

    private void ShowMessageBox(string message)
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
            stockInTraGridView.DataSource = null;
            stockInTraGridView.DataBind();
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        companyNameDropDownList.SelectedValue = "";

        if (companyNameDropDownList.SelectedValue == "")
        {
            dcDropDownList.Items.Clear();
        }
    }
}