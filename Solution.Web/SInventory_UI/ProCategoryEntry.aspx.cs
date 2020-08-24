using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ProCategoryEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void Clear()
    {
        proCategoryNameTextBox.Text = string.Empty;
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (proCategoryNameTextBox.Text != "")
        {
            ProductCategory aProductCategory = new ProductCategory()
            {
                CategoryName = proCategoryNameTextBox.Text
            };
            ProductCategoriesBLL aProductCategoriesBll = new ProductCategoriesBLL();
            MessageLabel.Text = aProductCategoriesBll.SaveProductCategory(aProductCategory);
            Clear();
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }
    protected void EmpCetegoryListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProCategoryView.aspx");
    }
}