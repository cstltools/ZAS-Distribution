using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ProUnitPriceEntry : System.Web.UI.Page
{
    UnitPriceBLL aUnitPriceBLL = new UnitPriceBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UnitPrice"] !=null)
            {
                unitPriceIdHiddenField.Value = Session["UnitPrice"].ToString();
                ProductEditLoad(unitPriceIdHiddenField.Value);
                Session["UnitPrice"] = null;
            }
        }
    }

    private void GetProductInfo(string productCode)
    {
        if (!string.IsNullOrEmpty(productCode))
        {
            DataTable aDataTable = new DataTable();
            if (!string.IsNullOrEmpty(productCode))
            {
                aDataTable = aUnitPriceBLL.ProductInfo(productCode);
                if (aDataTable.Rows.Count > 0)
                {
                    productNameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();
                    packSizeTextBox.Text = aDataTable.Rows[0]["PackSizeName"].ToString();
                    productIdHiddenField.Value = aDataTable.Rows[0]["ProductId"].ToString();
                }
                else
                {
                    Clear();
                    MessageLabel.Text = "Product Information Not Found!!";
                }
            }
        }
    }

    private void Clear()
    {
        productCodeTextBox.Text = string.Empty;
        productNameTextBox.Text = string.Empty;
        packSizeTextBox.Text = string.Empty;
        costPriceTextBox.Text = string.Empty;
        unitPriceTextBox.Text = string.Empty;
        productIdHiddenField.Value = null;
        vatperTextBox.Text = string.Empty;
        vatAmountTextBox.Text = string.Empty;
        RadioButtonList1.Items[0].Selected = false;
        RadioButtonList1.Items[1].Selected = false;
        @new.Visible = false;
        unitPriceIdHiddenField.Value = string.Empty;
        activeDtTextBox.Text = string.Empty;
        inactiveTextBox.Text = string.Empty;
        newCostPriceTextBox.Text = string.Empty;
        newUnitPriceTextBox.Text = string.Empty;
        newactiveDtTextBox.Text = string.Empty;
        newvatAmountTextBox.Text = string.Empty;
        newvatPerceTextBox.Text = string.Empty;
        mrpPriceTextBox.Text = "";
        newMRPTextBox.Text = "";
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (productCodeTextBox.Text == "")
        {
            ShowMessageBox("Please Input Product Code!!");
            return false;
        }
        if (costPriceTextBox.Text == "")
        {
            ShowMessageBox("Please Input Cost Price!!");
            return false;
        }

        if (unitPriceTextBox.Text == "")
        {
            ShowMessageBox("Please Input Unit Price!!");
            return false;
        }

        if (vatperTextBox.Text == "")
        {
            ShowMessageBox("Please Input Vat!!");
            return false;
        }

        if (vatAmountTextBox.Text == "")
        {
            ShowMessageBox("Please Input Vat Amount!!");
            return false;
        }

        if (activeDtTextBox.Text == "")
        {
            ShowMessageBox("Please active date!!");
            return false;
        }

        if (mrpPriceTextBox.Text == "")
        {
            ShowMessageBox("MRP Price is Required!!");
            return false;
        }

        if (unitPriceIdHiddenField.Value != "")
        {
            if (inactiveTextBox.Text == "")
            {
                ShowMessageBox("Please insert inactive date!!");
                return false;
            }

            if (newCostPriceTextBox.Text == "")
            {
                ShowMessageBox("Please Input New Cost Price!!");
                return false;
            }

            if (unitPriceTextBox.Text == "")
            {
                ShowMessageBox("Please Input New Unit Price!!");
                return false;
            }

            if (vatperTextBox.Text == "")
            {
                ShowMessageBox("Please Input New Vat!!");
                return false;
            }

            if (vatAmountTextBox.Text == "")
            {
                ShowMessageBox("Please Input Vat Amount!!");
                return false;
            }

            if (newactiveDtTextBox.Text == "")
            {
                ShowMessageBox("Please new active date!!");
                return false;
            }

            if (newMRPTextBox.Text == "")
            {
                ShowMessageBox("New MRP Price is required!!");
                return false;
            }
        }

        return true;
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (unitPriceIdHiddenField.Value == null || string.IsNullOrEmpty(unitPriceIdHiddenField.Value))
            {

                var aProductUnitPrice = new ProductUnitPrice()
                {
                    ProductCode = productCodeTextBox.Text,
                    ProductName = productNameTextBox.Text,
                    PackSize = packSizeTextBox.Text,
                    CostPrice = Convert.ToDecimal(costPriceTextBox.Text),
                    UnitPrice = Convert.ToDecimal(unitPriceTextBox.Text),
                    ProductId = Convert.ToInt32(productIdHiddenField.Value),
                    VATPercentage = Convert.ToDecimal(vatperTextBox.Text),
                    VATAmountPerUnit = Convert.ToDecimal(vatAmountTextBox.Text),
                    IsActive = true,
                    ActiveDate = Convert.ToDateTime(activeDtTextBox.Text),
                    EntryBy = Session["LoginName"].ToString(),
                    EntryDate = DateTime.Now,
                    ActionStatus = "Posted",
                    MRPPrice = Convert.ToDecimal(mrpPriceTextBox.Text)
                };

                var aUnitPriceBLL = new UnitPriceBLL();

                if (aUnitPriceBLL.SaveUnitPrice(aProductUnitPrice))
                {
                    ShowMessageBox("Data Save Successfully  Product Name  is :  " + aProductUnitPrice.ProductName +
                                   " And  Product Price is :" + aProductUnitPrice.UnitPrice);
                    Clear();
                }
                else
                {
                    ShowMessageBox("Product Price Already Exist !!");
                }
            }
            else
            {
                aUnitPriceBLL.UpdateActive(Convert.ToDateTime(inactiveTextBox.Text), unitPriceIdHiddenField.Value,Session["LoginName"].ToString(),DateTime.Now);
                var aProductUnitPrice = new ProductUnitPrice()
                {
                    ProductCode = productCodeTextBox.Text,
                    ProductName = productNameTextBox.Text,
                    PackSize = packSizeTextBox.Text,
                    CostPrice = Convert.ToDecimal(newCostPriceTextBox.Text),
                    UnitPrice = Convert.ToDecimal(newUnitPriceTextBox.Text),
                    ProductId = Convert.ToInt32(productIdHiddenField.Value),
                    VATPercentage = Convert.ToDecimal(newvatPerceTextBox.Text),
                    VATAmountPerUnit = Convert.ToDecimal(newvatAmountTextBox.Text),
                    IsActive = true,
                    ActiveDate = Convert.ToDateTime(newactiveDtTextBox.Text),
                    EntryBy = Session["LoginName"].ToString(),
                    EntryDate = DateTime.Now,
                    ActionStatus = "Posted",
                    MRPPrice = Convert.ToDecimal(newMRPTextBox.Text)

                };

                
                if (aUnitPriceBLL.SaveUnitPriceEdit(aProductUnitPrice))
                {
                    ShowMessageBox("Data Save Successfully  Product Name  is :  " + aProductUnitPrice.ProductName +
                                   " And  Product Price is :" + aProductUnitPrice.UnitPrice);
                    Clear();
                }
                else
                {
                    ShowMessageBox("Product Price Already Exist !!");
                }

            }
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }

    private void ProductEditLoad(string productId)
    {
        ProductUnitPrice aProductUnitPrice = new ProductUnitPrice();
        aProductUnitPrice = aUnitPriceBLL.ProductUnitPriceEditLoad(productId);
        productCodeTextBox.Text = aProductUnitPrice.ProductCode;
        productNameTextBox.Text = aProductUnitPrice.ProductName;
        packSizeTextBox.Text = aProductUnitPrice.PackSize;
        costPriceTextBox.Text = aProductUnitPrice.CostPrice.ToString();
        unitPriceTextBox.Text = aProductUnitPrice.UnitPrice.ToString();
        productIdHiddenField.Value = aProductUnitPrice.ProductId.ToString();
        vatperTextBox.Text = aProductUnitPrice.VATPercentage.ToString();
        vatAmountTextBox.Text = aProductUnitPrice.VATAmountPerUnit.ToString();
        activeDtTextBox.Text = aProductUnitPrice.ActiveDate.ToString("dd-MMM-yyyy");
        mrpPriceTextBox.Text = aProductUnitPrice.MRPPrice.ToString();
    }
    
    protected void productImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProUnitPriceView.aspx");
    }
    protected void productCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string productCode = productCodeTextBox.Text.Trim();
        GetProductInfo(productCode);
        
    }
    protected void productNameTextBox_TextChanged(object sender, EventArgs e)
    {
        string productName = productNameTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(productName))
        {
            if (productName.Contains(':'))
            {
                string[] productInfo = productName.Split(':');
                productCodeTextBox.Text = productInfo[0];
                string productCode = productCodeTextBox.Text.Trim();
                GetProductInfo(productCode);
            }
        }
    }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonList1.Items[1].Selected==true)
        {
            if (!string.IsNullOrEmpty(unitPriceIdHiddenField.Value))
            {
                @new.Visible = true;    
            }
            else
            {
                RadioButtonList1.Items[0].Selected = false;
                RadioButtonList1.Items[1].Selected = false;

                ShowMessageBox("Use Edit Mode to use that function ");
                
            }
            
        }
        else
        {
            @new.Visible = false;
        }
    }

    protected void clearButton_Click(object sender, EventArgs e)
    {
        Clear();
    }
}