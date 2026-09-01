using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ProductEntry : System.Web.UI.Page
{
    ProductBLL aProductBLL = new ProductBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProductCategory();
            LoadRawMaterial();
        }
    }

    private void LoadRawMaterial()
    {
        aProductBLL.LoadRawMaterial(ddlRawMaterials);
    }

    private void LoadProductCategory()
    {
        aProductBLL.LoadCompanyDropdownList(companyNameDropDownList);
        aProductBLL.LoadProductCategory(categoryNameDropDownList);
        aProductBLL.LoadManufac(manufacDropDownList);
        aProductBLL.LoadPackSize(packSizeDropDownList);
        aProductBLL.LoadStockUOM(stockUOMDropDownList);
        aProductBLL.LoadType(typeDropDownList);
        aProductBLL.LoadShippingCartonSize(shippingCartonSizeDropDownList);
        aProductBLL.LoadProductSQ(productSQDropDownList);
    }

    private void Clear()
    {
        companyNameDropDownList.SelectedValue = "";
        productNameTextBox.Text = "";
        categoryNameDropDownList.SelectedValue = "";
        descriptionTextBox.Text = "";
        packSizeDropDownList.SelectedValue = "";
        manufacDropDownList.SelectedValue = "";
        stockUOMDropDownList.SelectedValue = "";
        typeDropDownList.SelectedValue = "";
        productCodeTextBox.Text = "";
        productSQDropDownList.SelectedValue = "";
        shippingCartonSizeDropDownList.SelectedValue = "";

    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
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

        //if (cbxTradeOrRecipe.SelectedValue == "Trade")
        //{
        //    if (ddlRawMaterials.SelectedIndex == 0)
        //    {
        //        showMessageBox("Please select Raw Material Name !!!");
        //        return false;
        //    }
        //}

        return true;
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            var aProduct = new Product()
            {
                ProductCode = productCodeTextBox.Text.Trim(),
                ProductName = productNameTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                PackSize = packSizeDropDownList.SelectedItem.Text.Trim(),
                CategoryId = Convert.ToInt32(categoryNameDropDownList.SelectedValue),
                CategoryName = categoryNameDropDownList.SelectedItem.Text.Trim(),
                ManufacId = Convert.ToInt32(manufacDropDownList.SelectedValue),
                PackSizeId = Convert.ToInt32(packSizeDropDownList.SelectedValue),
                StockUOMId = Convert.ToInt32(stockUOMDropDownList.SelectedValue),
                ProTypeId = Convert.ToInt32(typeDropDownList.SelectedValue),
                ProductBrandId = Convert.ToInt32(productSQDropDownList.SelectedValue),
                CaseId= Convert.ToInt32(shippingCartonSizeDropDownList.SelectedValue),
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                IsTrade = cbxTradeOrRecipe.SelectedValue == "Trade" ? true: false,
                IsRecipe = cbxTradeOrRecipe.SelectedValue != "Trade" ? true : false

            };

            var aProductBLL = new ProductBLL();

            if (aProductBLL.SaveProduct(aProduct))
            {
                // Save

                int ItemDescriptionId = aProductBLL.CretaeItem(aProduct);

                // Save Raw Material Details

                if (cbxTradeOrRecipe.SelectedValue == "Trade")
                {
                    DataTable aTable = aProductBLL.GetPoductByCode(productCodeTextBox.Text.Trim());

                    if (aTable != null && aTable.Rows.Count > 0)
                    {
                        object productIdObj = aTable.Rows[0]["ProductId"];

                        if (productIdObj != null && productIdObj != DBNull.Value)
                        {
                            int productId = Convert.ToInt32(productIdObj);

                            int rawMaterialId = 0;

                            if (!string.IsNullOrEmpty(ddlRawMaterials.SelectedValue))
                            {
                                rawMaterialId = Convert.ToInt32(ddlRawMaterials.SelectedValue);
                            }
                            else if (ItemDescriptionId > 0)
                            {
                                rawMaterialId = ItemDescriptionId;
                            }

                            // Only save if RawMaterialId is valid (> 0)

                            if (rawMaterialId > 0)
                            {
                                var productRawMaterialDetailsDao = new ProductRawMaterialDetailsDao
                                {
                                    RawMaterialId = rawMaterialId,
                                    ProductId = productId
                                };

                                aProductBLL.SaveRawMaterialDetails(productRawMaterialDetailsDao);
                            }
                        }
                    }
                }

                showMessageBox("Data Saved Successfully !!!");
                Clear();
            }

            else
            {
                showMessageBox("Product Code Already Exist !!!");
            }
            
        }
        else
        {
            showMessageBox("Please input data in all Textbox");
        }
    }

    
    protected void productImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProductView.aspx");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
       Clear();
    }

    protected void productCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aProductBLL.CodeExistOrNot(productCodeTextBox.Text.Trim());
        GotoPermissionSetUp(info, productCodeTextBox, "Product Code already Exist !!!");
    }

    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            showMessageBox(msg);
        }
    }

    protected void productNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aProductBLL.NameExistOrNot(productNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, productNameTextBox, "Product Name already Exist !!!");
    }


    protected void cbxTradeOrRecipe_Clicked(object sender, EventArgs e)
    {
        if (cbxTradeOrRecipe.SelectedValue == "Trade")
        {
            ddlRawMaterials.Enabled = true; // Enable the dropdown
        }
        else
        {
            ddlRawMaterials.SelectedValue = "";
            ddlRawMaterials.Enabled = false; // Disable the dropdown
        }
    }
}