using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_ProductDiscountView : System.Web.UI.Page
{
    ProductDiscountBLL aProductDiscountBll = new ProductDiscountBLL();
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

    private void ProductdiscountLoad()
    {
        DataTable aDataTable = aProductDiscountBll.ProductDiscountView(GetPrameterList());
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

    private String GetPrameterList()
    {
        string parameter = "";

        if (custIdHiddenField.Value != "")
        {
            parameter = parameter + " AND ProDis.CustomerMasterId = '" + custIdHiddenField.Value + "'";
        }

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND tblProduct.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        return parameter;
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        ProductdiscountLoad();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        ProductdiscountLoad();
    }
}