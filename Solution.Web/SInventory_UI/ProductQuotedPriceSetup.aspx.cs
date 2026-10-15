using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ProductQuotedPriceSetup : System.Web.UI.Page
{
    ProductDiscountBLL aProductDiscountBll = new ProductDiscountBLL();
    private CustPaymentBLL aCustPaymentBll = new CustPaymentBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownLoad();
            if (Session["DiscountId"] != null)
            {
                discountIdHiddenField.Value = Session["DiscountId"].ToString();
                GetData(Session["DiscountId"].ToString());
                Session["DiscountId"] = null;

            }
        }
    }
    private void Clear()
    {
        productCodeTextBox.Text = string.Empty;
        customerTextBox.Text = string.Empty;
        quotedPriceTextBox.Text = string.Empty;
        activeDtTextBox.Text = string.Empty;
        inactiveDtTextBox.Text = string.Empty;
        customerNameTextBox1.Text = string.Empty;
        productNameTextBox2.Text = string.Empty;
        MessageLabel.Text = "";
        txtIsactive.Text = string.Empty;
    }

    private void GetData(string id)
    {
        ProductDiscount aProductDiscount = aProductDiscountBll.ProductDiscountEditLoad(id);
        custIdHiddenField.Value = aProductDiscount.CustomerMasterId.ToString();
        productIdHiddenField.Value = aProductDiscount.ProductId.ToString();
        quotedPriceTextBox.Text = aProductDiscount.DiscountPercentage.ToString("F");
        activeDtTextBox.Text = aProductDiscount.ActiveDate.ToString("dd-MMM-yyyy");
        inactiveDtTextBox.Text = aProductDiscount.InactiveDate.ToString("dd-MMM-yyyy");
        DataTable dtcust = aProductDiscountBll.GetCustomerInfo(custIdHiddenField.Value);
        if (dtcust.Rows.Count > 0)
        {
            customerTextBox.Text = dtcust.Rows[0]["CustomerCode"].ToString();
            customerNameTextBox1.Text = dtcust.Rows[0]["CustomerName"].ToString();
        }
        DataTable dtproduct = aProductDiscountBll.GetProduct(productIdHiddenField.Value);
        if (dtproduct.Rows.Count > 0)
        {
            productNameTextBox2.Text = dtproduct.Rows[0]["ProductName"].ToString();
            productCodeTextBox.Text = dtproduct.Rows[0]["ProductCode"].ToString();
        }

    }
    private void DropDownLoad()
    {
        aProductDiscountBll.LoadmanufacturerName(manufacturerDropDownList);

        AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productNameTextBox2.FindControl("productNameTextBox_AutoCompleteExtender");
        modal.ContextKey = manufacturerDropDownList.SelectedValue;
    }

    public void SaveData()
    {



        if (productCodeTextBox.Text != "" && customerTextBox.Text != "" && activeDtTextBox.Text != "" && inactiveDtTextBox.Text != ""
            && quotedPriceTextBox.Text != "")
        {
            if (CheckDiscountExistOrNot())
            {
                CustomerMaster aCustomerMaster;

                QuotedPriceSetup aProductDiscount = new QuotedPriceSetup()
                {
                    ProductId = Convert.ToInt32(productIdHiddenField.Value.Trim()),
                    CustomerMasterId = CustomerId(out aCustomerMaster),
                    QuotedPrice = Convert.ToDecimal(quotedPriceTextBox.Text),
                    Status = "Active",
                    ActiveDate = Convert.ToDateTime(activeDtTextBox.Text),
                    InactiveDate = Convert.ToDateTime(inactiveDtTextBox.Text),
                    EntryBy = Session["LoginName"].ToString(),
                    EntryDate = DateTime.Now,
                    ActionStatus = "Posted"

                };

                if (aProductDiscountBll.SaveQuotedPrice(aProductDiscount))
                {
                    showMessageBox("Data Saved Successfully !!");
                }

            }
            else
            {
                showMessageBox("Quoted Price already exist !!!");
            }
            Clear();
        }
        else
        {
            showMessageBox("Please input data in all Textbox !!!");
        }
    }
    public void UpdateData()
    {
        if (productCodeTextBox.Text != "" && customerTextBox.Text != "" && activeDtTextBox.Text != "" && inactiveDtTextBox.Text != ""
            && quotedPriceTextBox.Text != "")
        {
            CustomerMaster aCustomerMaster;
            ProductDiscount aProductDiscount = new ProductDiscount()
            {
                DiscountId = Convert.ToInt32(discountIdHiddenField.Value),
                ProductId = Convert.ToInt32(productIdHiddenField.Value.Trim()),
                CustomerMasterId = CustomerId(out aCustomerMaster),
                DiscountPercentage = Convert.ToDecimal(quotedPriceTextBox.Text),
                Status = "Active",
                ActiveDate = Convert.ToDateTime(activeDtTextBox.Text),
                InactiveDate = Convert.ToDateTime(inactiveDtTextBox.Text),
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now

            };
            if (aProductDiscountBll.UpdateProductDiscountInfo(aProductDiscount))
            {
                showMessageBox("Data Update Successfully");
            }
            Clear();
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (discountIdHiddenField.Value == string.Empty)
        {
            SaveData();
        }
        else
        {
            UpdateData();
        }

    }
    //protected void customerTextBox_TextChanged(object sender, EventArgs e)
    //{
    //    CustomerMaster aCustomerMaster = new CustomerMaster();
    //    aCustomerMaster = aCustPaymentBll.DetailCustomerLoad(customerTextBox.Text.Trim());
    //    customerNameTextBox1.Text = aCustomerMaster.CustomerName;

    //}
    private int CustomerId(out CustomerMaster aCustomerMaster)
    {
        int CustomerID = 0;
        aCustomerMaster = new CustomerMaster();
        aCustomerMaster = aCustPaymentBll.CustomerLoad(customerTextBox.Text.Trim());
        CustomerID = aCustomerMaster.CustomerMasterId;
        return CustomerID;
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void productCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        Product aProduct = new Product();
        aProduct = aCustPaymentBll.DetailProductLoad(productCodeTextBox.Text.Trim());
        productNameTextBox2.Text = aProduct.ProductName;

        GetProductInGrid(productCodeTextBox.Text);

    }
    public void GenerateParameter()
    {
        string param = "";
        if (manufacturerDropDownList.SelectedIndex != 0)
        {
            param = param + "  AND tblCompanyUnit.CompanyId='" + manufacturerDropDownList.SelectedValue + "' ";

        }
        if (dcDropDownList.SelectedIndex > 0)
        {
            param = param + " AND ComUnitId='" + dcDropDownList.SelectedValue + "'";
        }
        if (areaDropDownList.SelectedIndex > 0)
        {
            param = param + " AND tblArea.AreaId='" + areaDropDownList.SelectedValue + "'";
        }
        if (mioDropDownList.SelectedIndex > 0)
        {
            param = param + " AND EmpInfoId='" + mioDropDownList.SelectedValue + "'";
        }
        if (territoryDropDownList.SelectedIndex > 0)
        {
            param = param + " AND tblTerritory.TerritoryId='" + territoryDropDownList.SelectedValue + "'";
        }
        if (marketDropDownList.SelectedIndex > 0)
        {
            param = param + " AND tblMarket.MarketId='" + marketDropDownList.SelectedValue + "'";
        }

        Session["MOParam"] = param;
    }

    protected void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aProductDiscountBll.DCLoad(dcDropDownList, manufacturerDropDownList.SelectedValue);
        GenerateParameter();
        AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productNameTextBox2.FindControl("productNameTextBox_AutoCompleteExtender");
        modal.ContextKey = manufacturerDropDownList.SelectedValue;
    }
    protected void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aProductDiscountBll.AreaLoad(areaDropDownList, dcDropDownList.SelectedValue);
        GenerateParameter();
    }

    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        aProductDiscountBll.MIOLoad(mioDropDownList, areaDropDownList.SelectedValue);
        GenerateParameter();
    }
    protected void mioDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aProductDiscountBll.TerritoryLoad(territoryDropDownList, mioDropDownList.SelectedValue);
        GenerateParameter();
    }
    protected void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        aProductDiscountBll.MarketLoad(marketDropDownList, territoryDropDownList.SelectedValue);
        GenerateParameter();
    }
    protected void marketDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateParameter();
    }
    protected void customerNameTextBox1_TextChanged(object sender, EventArgs e)
    {
        if (customerNameTextBox1.Text.Contains(':'))
        {
            string[] cust = customerNameTextBox1.Text.Split(':');
            customerNameTextBox1.Text = cust[1];
            customerTextBox.Text = cust[0];
            custIdHiddenField.Value = cust[cust.Length - 1]; // GetCustomernew returns Code:Name:(Region...):Id
        }


    }
    protected void productNameTextBox2_TextChanged(object sender, EventArgs e)
    {
        if (productNameTextBox2.Text.Contains(':'))
        {
            string[] product = productNameTextBox2.Text.Split(':');
            //txtIsactive.Text = product[2];
            productNameTextBox2.Text = product[1];
            productCodeTextBox.Text = product[0];

            GetProductInGrid(productCodeTextBox.Text);
        }
    }

    private bool CheckDiscountExistOrNot()
    {
        bool status = false;


        DataTable aTable = aProductDiscountBll.CheckQuotedPricetExistOrNot(Convert.ToInt32(customerTextBox.Text), productCodeTextBox.Text.Trim());

        if (aTable.Rows.Count == 0)
        {
            status = true;
        }



        return status;
    }

    private void GetProductInGrid(string productCode)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(productCode))
        {

            aDataTable = _aDcStockReceiveBll.ProductInfo(productCode, manufacturerDropDownList.SelectedValue);
            if (aDataTable.Rows.Count > 0)
            {
                productIdHiddenField.Value = aDataTable.Rows[0]["ProductId"].ToString();
                txtIsactive.Text = aDataTable.Rows[0]["UnitPrice"].ToString(); ;
            }
        }
    }
    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProductQuotedPriceView.aspx");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void discountAmountTextBox_OnTextChanged(object sender, EventArgs e)
    {
        AmountOrPercantageCalculation("amount");
    }

    protected void discountPerTextBox_OnTextChanged(object sender, EventArgs e)
    {
        AmountOrPercantageCalculation("percentage");
    }

    private void AmountOrPercantageCalculation(string flag)
    {
        decimal amount = 0;
        decimal percentage = 0;

        if (flag == "amount")
        {
            percentage = percentage + ((Convert.ToDecimal(quotedPriceTextBox.Text) * 100) / Convert.ToDecimal(txtIsactive.Text));
            quotedPriceTextBox.Text = System.Math.Round(percentage, 2).ToString(CultureInfo.InvariantCulture);
        }

        if (flag == "percentage")
        {
            amount = amount + ((Convert.ToDecimal(quotedPriceTextBox.Text) / 100) * Convert.ToDecimal(txtIsactive.Text));
            quotedPriceTextBox.Text = System.Math.Round(amount, 2).ToString(CultureInfo.InvariantCulture);
        }
    }
}