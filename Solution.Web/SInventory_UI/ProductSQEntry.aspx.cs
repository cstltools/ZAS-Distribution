using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ProductSQEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownList();
        }
    }

    public void DropDownList()
    {
        ProductSQBLL aProductSQBll = new ProductSQBLL();
        aProductSQBll.LoadIngrident(ingridentDropDownList);
    }
    private void Clear()
    {
        productSQTextBox.Text = string.Empty;
        ingridentDropDownList.SelectedValue = string.Empty;

    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (productSQTextBox.Text != "")
        {
            ProductSQ aProductSQ= new ProductSQ()
            {
                ProductSQName = productSQTextBox.Text,
                //IngridentsId = Convert.ToInt32(ingridentDropDownList.SelectedValue)

                
            };
            ProductSQBLL aProductSQBll = new ProductSQBLL();
            MessageLabel.Text = aProductSQBll.SaveProductSQ(aProductSQ);
            Clear();
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }
    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProductSQView.aspx");
    }
}