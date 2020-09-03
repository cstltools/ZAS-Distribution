using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ProductEdit : System.Web.UI.Page
{
    ProductBLL ProductBLL = new ProductBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProductCategory();
            productIdHiddenField.Value = Request.QueryString["ID"];
            ProductEditLoad(productIdHiddenField.Value);
        }
    }

    private void LoadProductCategory()
    {
        ProductBLL.LoadCompanyDropdownList(companyNameDropDownList);
        ProductBLL.LoadProductCategory(categoryNameDropDownList);
        ProductBLL.LoadManufac(manufacDropDownList);
        ProductBLL.LoadPackSize(packSizeDropDownList);
        ProductBLL.LoadStockUOM(stockUOMDropDownList);
        ProductBLL.LoadType(typeDropDownList);
        ProductBLL.LoadShippingCartonSize(shippingCartonSizeDropDownList);
        ProductBLL.LoadProductSQ(productSQDropDownList);
    }


    private void showMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            showMessageBox("Please select a company!!");
            return false;
        }

        if (productNameTextBox.Text == "")
        {
            showMessageBox("Please Input Product Name!!");
            return false;
        }
        if (productCodeTextBox.Text == "")
        {
            showMessageBox("Please Input Product Code!!");
            return false;
        }
        if (descriptionTextBox.Text == "")
        {
            showMessageBox("Please Input Description!!");
            return false;
        }

        if (packSizeDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Input Pack Size!!");
            return false;
        }
        if (manufacDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Input Manufacturer!!");
            return false;
        }
        if (stockUOMDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Input StockUOM!!");
            return false;
        }
        if (typeDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Input Type!!");
            return false;
        }

        if (productSQDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Input ProductSQ!!");
            return false;
        }
        if (shippingCartonSizeDropDownList.SelectedIndex == 0)
        {
            showMessageBox("Please Input Shipping Carton Size!!");
            return false;
        }

        return true;
    }

    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            Product aProduct = new Product()
            {
                ProductId = Convert.ToInt32(productIdHiddenField.Value),
                ProductCode = productCodeTextBox.Text,
                ProductName = productNameTextBox.Text,
                Description = descriptionTextBox.Text,
                PackSize = packSizeDropDownList.SelectedItem.Text,
                CategoryId = Convert.ToInt32(categoryNameDropDownList.SelectedValue),
                CategoryName = categoryNameDropDownList.SelectedItem.Text,
                ManufacId = Convert.ToInt32(manufacDropDownList.SelectedValue),
                PackSizeId = Convert.ToInt32(packSizeDropDownList.SelectedValue),
                StockUOMId = Convert.ToInt32(stockUOMDropDownList.SelectedValue),
                ProTypeId = Convert.ToInt32(typeDropDownList.SelectedValue),
                ProductBrandId = Convert.ToInt32(productSQDropDownList.SelectedValue),
                CaseId = Convert.ToInt32(shippingCartonSizeDropDownList.SelectedValue),
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue)
            };
            
            ProductBLL aProductBLL = new ProductBLL();

            if (!aProductBLL.UpdateDataForProduct(aProduct))
            {
                ShowMessageBox("Data Not Update!!!");
                
            }
            else
            {
                ShowMessageBox("Data Update Successfully!!! Please Reload");
                
            }
        }
        else
        {
            ShowMessageBox("Please input data in all Textbox");
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void ProductEditLoad(string productId)
    {
        var Product = new Product();
        Product = ProductBLL.ProductEditLoad(productId);
        productNameTextBox.Text = Product.ProductName;
        descriptionTextBox.Text = Product.Description;
        categoryNameDropDownList.SelectedValue = Product.CategoryId.ToString();
        manufacDropDownList.SelectedValue = Product.ManufacId.ToString();
        packSizeDropDownList.SelectedValue = Product.PackSizeId.ToString();
        typeDropDownList.SelectedValue = Product.ProTypeId.ToString();
        productSQDropDownList.SelectedValue = Product.ProductBrandId.ToString();
        shippingCartonSizeDropDownList.SelectedValue = Product.CaseId.ToString();
        stockUOMDropDownList.SelectedValue = Product.StockUOMId.ToString();
        productCodeTextBox.Text = Product.ProductCode;
        companyNameDropDownList.SelectedValue = Convert.ToString(Product.CompanyId);

        codeHiddenField.Value = Product.ProductCode;
        nameHiddenField.Value = Product.ProductName;
    }
    
    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }

    protected void productCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = ProductBLL.CodeExistOrNot(productCodeTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, productCodeTextBox,codeHiddenField ,"Product Code already Exist !!!");
    }

    protected void productNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = ProductBLL.NameExistOrNot(productNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, productNameTextBox,nameHiddenField ,"Product Name already Exist !!!");
    }


    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("ProductId") != Convert.ToInt32(productIdHiddenField.Value))
                {
                    count++;
                }

                if (count > 0)
                {
                    textBox.Text = presentData.Value;
                    ShowMessageBox(msg);
                    break;
                }
            }

        }
    }



}