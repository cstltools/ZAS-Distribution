using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_ProductDiscountView : System.Web.UI.Page
{
    ProductDiscountBLL aProductDiscountBll = new ProductDiscountBLL();
    CreditLimitDAL aCashBll = new CreditLimitDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoaddropdownList();
            ProductdiscountLoad();
        }
    }

    private void LoaddropdownList()
    {

        aCashBll.GetCompanyDropdownList(CompanyDropDown);
        CompanyDropDown.SelectedValue = 2.ToString();

    }



    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {

        string Perticular = custCodeTextBox.Text.Trim();


        if (Perticular.Contains(':'))
        {
            string[] emp = Perticular.Split(':');
            //  TextBox txtPerticular = (TextBox)itemGridView.Rows[rowindex].Cells[1].FindControl("txtPerticular");
            custCodeTextBox.Text = emp[1];

            CustomerIdHiddenField.Value = emp[3];



        }
        else
        {

            custCodeTextBox.Text = "";
            CustomerIdHiddenField.Value = "";
            //showMessageBox("Input Correct Data !");
        }

        ProductdiscountLoad();
    }

    private String GenerateParam()
    {
        string param = "";


        if (CompanyDropDown.SelectedValue != "")
        {
            param = param + "And tblProduct.CompanyId ='" + CompanyDropDown.SelectedValue + "' ";
        }


        if (CustomerIdHiddenField.Value != "")
        {
            param = param + " And ProDis.CustomerMasterId ='" + CustomerIdHiddenField.Value + "' ";
        }

        return param;
    }

    private void ProductdiscountLoad()
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();

        if (CompanyDropDown.SelectedValue != "")
        {

            if (CustomerIdHiddenField.Value != "")
            {
                DataTable aDataTable = aProductDiscountBll.ProductQuotedPriceView(GenerateParam());
                loadGridView.DataSource = aDataTable;
                loadGridView.DataBind();
            }
            else
            {
                showMessageBox("Please Select Customer !!");
            }
        }
        else
        {
            showMessageBox("Please Select Company !!");
        }

        
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

            if (aProductDiscountBll.DeleteProductQuotedprice(productDiscountId))
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
        Response.Redirect("ProductQuotedPriceSetup.aspx");
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

    protected void CompanyDropDown_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ProductdiscountLoad();
    }
}