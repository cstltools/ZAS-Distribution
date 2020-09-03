using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_DirectStockOutView : System.Web.UI.Page
{
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
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

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserId"].ToString() == 2.ToString(CultureInfo.InvariantCulture))
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

    protected void searchButton_Click(object sender, EventArgs e)
    {
        if (dcDropDownList.SelectedValue != "")
        {
            DataTable aTable = aReportDal.GetDCStockOutInfo(GetPrameterList());

            if (aTable.Rows.Count > 0)
            {
                orderGridView.DataSource = aTable;
                orderGridView.DataBind();
            }
            else
            {
                orderGridView.DataSource = null;
                orderGridView.DataBind();
            }
        }
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (dcDropDownList.SelectedValue != "")
        {
            parameter = parameter + "AND DCId = '" + dcDropDownList.SelectedValue + "'";
        }

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            parameter = parameter + "AND StockOutDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AND '" + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "'";
        }


        return parameter;

    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
 
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}