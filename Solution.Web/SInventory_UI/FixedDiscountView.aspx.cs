using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_FixedDiscountView : System.Web.UI.Page
{
    FixedDiscountEntryDal aEntryDal = new FixedDiscountEntryDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {

        aEntryDal.LoadGroupInfo(groupDropDownList);

        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);


        if (groupDropDownList.SelectedValue != "")
        {
            aEntryDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
        }

    }

    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        if (custCodeTextBox.Text.Contains(':'))
        {
            custIdHiddenField.Value = "";
            string[] data = custCodeTextBox.Text.Split(':');
            custCodeTextBox.Text = data[0];
            custIdHiddenField.Value = data[3];
        }
        else
        {
            if (custCodeTextBox.Text != "")
            {
                custIdHiddenField.Value = "";
                DataTable aTable = aEntryDal.GetCustomerInfoByCode(custCodeTextBox.Text.Trim());

                if (aTable.Rows.Count > 0)
                {
                    custIdHiddenField.Value =
                        aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    custIdHiddenField.Value = "";
                    custCodeTextBox.Text = "";
                }
            }
        }

    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        LoadDiscountData();
    }

    private void LoadDiscountData()
    {
        DataTable aTable = aEntryDal.GetDiscountInfo(GetPrameterList());

        if (aTable.Rows.Count > 0)
        {
            invoiceGridView.DataSource = aTable;
            invoiceGridView.DataBind();
        }
        else
        {
            invoiceGridView.DataSource = null;
            invoiceGridView.DataBind();
        }
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (custIdHiddenField.Value != "")
        {
            parameter = parameter + " AND FDC.CustMasterId = '" + custIdHiddenField.Value + "'";
        }

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        return parameter;
    }

    protected void invoiceButton_Click(object sender, EventArgs e)
    {
        var button = (LinkButton)sender;
        var currentRow = (GridViewRow)button.Parent.Parent;

        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        if (aEntryDal.DeleteDiscountInfo(invoiceGridView.DataKeys[rowindex]["DiscountId"].ToString(),Session["LoginName"].ToString()))
        {
            ShowMessageBox("Discount inactive successfully !!!");
        }

        LoadDiscountData();

    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        LoadDiscountData();
    }

    protected void CustMasterNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("FixedDiscountEntry.aspx");
    }
}