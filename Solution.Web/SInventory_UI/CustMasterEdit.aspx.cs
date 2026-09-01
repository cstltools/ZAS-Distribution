using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CustMasterEdit : System.Web.UI.Page
{

    CustomerMasterInfoBll aMasterInfoBll = new CustomerMasterInfoBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
            SetShipingCondition();
            custMastIdHiddenField.Value = Request.QueryString["ID"];
            CompanyInfoLoad(custMastIdHiddenField.Value);
        }
    }

    private void SetShipingCondition()
    {
        shippingConditionTextBox.Text = "N/A";
    }

    public void LoadDropDown()
    {
        aMasterInfoBll.GetRegion(regionDropDownList);

        aMasterInfoBll.LoadCategoryName(categoryNameDropDownList);
        aMasterInfoBll.LoadAIT(ddlAIT);
    }

    private void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (customernameTextBox.Text == "")
        {
            showMessageBox("Please Input Customer Name!!");
            return false;
        }
        if (addressTextBox.Text == "")
        {
            showMessageBox("Please Input Address!!");
            return false;
        }

        if (contactTextBox.Text == "")
        {
            showMessageBox("Please Input Mobail Number!!");
            return false;
        }
       
        //if (paymentTypeDropDownList.SelectedIndex == 0)
        //{
        //    showMessageBox("Please Input Payment Type !!");
        //    return false;
        //}
        //if (regionNameDropDownList.SelectedIndex == 0)
        //{
        //    showMessageBox("Please Input Region  !!");
        //    return false;
        //}
        
       
        
        if (categoryNameDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Input Category  !!");
            return false;
        }
      
        if (customernameTextBox.Text == "")
        {
            showMessageBox("Please Input Customer Name  !!");
            return false;
        }
        if (addressTextBox.Text == "")
        {
            showMessageBox("Please Input Customer Address  !!");
            return false;
        }
        if (addressTextBox2.Text == "")
        {
            showMessageBox("Please Input Customer Address Second  !!");
            return false;
        }
        if (contactTextBox.Text == "")
        {
            showMessageBox("Please Input Customer Contact No  !!");
            return false;
        }
        if (cityTextBox.Text == "")
        {
            showMessageBox("Please Input Customer City  !!");
            return false;
        }
        if (contactPersonTextBox.Text == "")
        {
            showMessageBox("Please Input Contact Person  !!");
            return false;
        }
        //if (shippingConditionTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Shipping Condition  !!");
        //    return false;
        //}
      
        return true;
    }

    private DateTime? ParseNullableDate(string text)
    {
        DateTime parsedDate;
        if (DateTime.TryParse(text, out parsedDate))
        {
            return parsedDate;
        }

        return null;
    }

    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (Validation() == true)
        {
             //string[] CUnit = comUnitNameDropDownList.SelectedItem.Text.Split(':');
            //string[] MarketName = marketNameDropDownList.SelectedItem.Text.Split(':');

             bool fixedCustomer = false;

             if (fbDropDownList.SelectedValue == "1")
             {
                 fixedCustomer = true;
             }

            CustomerMaster aCustomerMaster = new CustomerMaster()
            {
                CustomerMasterId = Convert.ToInt32(custMastIdHiddenField.Value),
                CustomerName = customernameTextBox.Text,

                CategoryId = Convert.ToInt32(categoryNameDropDownList.SelectedValue),
                Address = addressTextBox.Text,
                Addrees2 = addressTextBox2.Text,

                CellNo = contactTextBox.Text,
                City = cityTextBox.Text,
                TradelicenseNo = tbxTradeLicenseNo.Text,

                ConPerson = contactPersonTextBox.Text,
                ShippingCond = null,

                MarketId = Convert.ToInt32(marketDropDownList.SelectedValue),

                TermOfPayment = paymentTypeDropDownList.SelectedValue,
                FixedCustomer = fixedCustomer,
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now,
                IsVatApplicable = isVatApplicable.Checked,
                AITGLId = ddlAIT.SelectedValue != "" ? Convert.ToInt32(ddlAIT.SelectedValue) : 0,
                TradeLicenseExpiryDate = ParseNullableDate(tbxTradeLicenseExpiryDate.Text)
            };
            //CustomerMasterBLL aCustomerMasterBLL = new CustomerMasterBLL();

           // DataTable dtcustomerProformaButNoDelevery = aMasterInfoBll.Customer(custMastIdHiddenField.Value);
          //  DataTable dtcustomerPaymentNotFull = aMasterInfoBll.CustomerPayment(custMastIdHiddenField.Value);

            //DataTable dtcustomerOrderSubmitted = aCustomerMasterBLL.Customerorder(customerCodeTextBox.Text);
           // if (dtcustomerOrderSubmitted.Rows.Count < 1)
            //{
               // if (dtcustomerProformaButNoDelevery.Rows.Count < 1)
                //{
                //    if (dtcustomerPaymentNotFull.Rows.Count < 1)
                    {
                        if (!aMasterInfoBll.UpdateDataForCustomerMaster(aCustomerMaster))
                        {
                            showMessageBox("Data Not Update!!!");
                        }
                        else
                        {
                            aMasterInfoBll.SaveCoaMasterInfo(aCustomerMaster.CustomerMasterId);
                            showMessageBox("Data Update Successfully!!! Please Reload");
                        }
                    }
                //    else
                //    {
                //        showMessageBox("Payment Pending !!");
                //    }
                //}
                //else
                //{
                //    showMessageBox("Delivery Pending !!");
                //}
            //}
            //else
            //{
            //    showMessageBox("New Order is Submitted For this Customer !!");
            //}
        }
        else
        {
            showMessageBox("Please input data in all Textbox");
        }
    }

    private void CompanyInfoLoad(string custMastId)
    {
        CustomerMaster aCustomerMaster = new CustomerMaster();

        aCustomerMaster = aMasterInfoBll.CustomerMasterEditLoad(custMastId);
       
        customernameTextBox.Text = aCustomerMaster.CustomerName;
        
        

        if (aCustomerMaster.MarketId != 0)
        {
             DataTable dt = aMasterInfoBll.LoadAllMyMarket(aCustomerMaster.MarketId.ToString());
             regionDropDownList.SelectedValue = dt.Rows[0]["RegionId"].ToString();
             aMasterInfoBll.GetAreaByRegionId(areaDropDownList, regionDropDownList.SelectedValue);
             areaDropDownList.SelectedValue = dt.Rows[0]["AreaId"].ToString();
             aMasterInfoBll.GetTerritiryByAreaId(territoryDropDownList, areaDropDownList.SelectedValue);
             territoryDropDownList.SelectedValue = dt.Rows[0]["TerritoryId"].ToString();
             aMasterInfoBll.GetMarketNameByTerritoryId(marketDropDownList, territoryDropDownList.SelectedValue);
             marketDropDownList.SelectedValue = aCustomerMaster.MarketId.ToString();
        }

        if (aCustomerMaster.AITGLId != 0)
        {
            ddlAIT.SelectedValue = aCustomerMaster.AITGLId.ToString();
        }

        isVatApplicable.Checked = aCustomerMaster.IsVatApplicable;
        addressTextBox.Text = aCustomerMaster.Address;
        contactTextBox.Text = aCustomerMaster.CellNo;
        tbxTradeLicenseNo.Text = aCustomerMaster.TradelicenseNo;
        tbxTradeLicenseExpiryDate.Text = aCustomerMaster.TradeLicenseExpiryDate.HasValue
            ? aCustomerMaster.TradeLicenseExpiryDate.Value.ToString("yyyy-MM-dd")
            : string.Empty;
        paymentTypeDropDownList.SelectedValue = aCustomerMaster.PaymentType;
  

        categoryNameDropDownList.SelectedValue = aCustomerMaster.CategoryId.ToString();
        addressTextBox2.Text = aCustomerMaster.Addrees2;
        cityTextBox.Text = aCustomerMaster.City;
        contactPersonTextBox.Text = aCustomerMaster.ConPerson;
        shippingConditionTextBox.Text = aCustomerMaster.ShippingCond;
        paymentTypeDropDownList.SelectedValue = aCustomerMaster.TermOfPayment;
        fbDropDownList.SelectedValue = fbDropDownList.Items.FindByText(aCustomerMaster.FixedCustomer.ToString()).Value;

        string loginUserName = Session["LoginName"].ToString();

        if (loginUserName.Trim() == "rezaul")
        {
            divCategoryType.Visible = true;
            divpaymentType.Visible = true;
        }
        else
        {
            divCategoryType.Visible = false;
            divpaymentType.Visible = false;
        }


    }


    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }
    protected void custMasterListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CustomerMasterView.aspx");
    }

    protected void regionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aMasterInfoBll.GetAreaByRegionId(areaDropDownList, regionDropDownList.SelectedValue);
    }
    protected void areaDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aMasterInfoBll.GetTerritiryByAreaId(territoryDropDownList, areaDropDownList.SelectedValue);
    }
    protected void territoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aMasterInfoBll.GetMarketNameByTerritoryId(marketDropDownList, territoryDropDownList.SelectedValue);
    }
    
}
