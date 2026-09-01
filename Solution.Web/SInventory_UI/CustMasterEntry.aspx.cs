using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CustMasterEntry : System.Web.UI.Page
{
    CustomerMasterInfoBll aMasterInfoBll = new CustomerMasterInfoBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
            SetNA();
            //SetShipingCondition();
            // SetCustomerCode();
        }
    }

    private void SetNA()
    {
        addressTextBox2.Text = "N/A";
        contactTextBox.Text = "N/A";
        cityTextBox.Text = "N/A";
        contactPersonTextBox.Text = "N/A";
    }

    private void SetCustomerCode()
    {
        DataTable customerData = new DataTable();
        customerData = aMasterInfoBll.LoadCustomerMasterId();
        Int32 custCode = 0;

        if (customerData.Rows.Count > 0)
        {
            custCode = Convert.ToInt32(customerData.Rows[0].Field<String>("CustomerCode")) + 1;
            //customerCodeTextBox.Text = custCode.ToString();
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
        //aMasterInfoBll.GetLedgerHead(ddlLedgerHead);
        //aMasterInfoBll.LoadAIT(ddlAIT);
    }

    private void Clear()
    {
        customernameTextBox.Text = string.Empty;
        addressTextBox.Text = string.Empty;
        contactTextBox.Text = string.Empty;
        paymentTypeDropDownList.SelectedValue = null;
       
        categoryNameDropDownList.SelectedValue = null;
        addressTextBox2.Text = string.Empty;
        cityTextBox.Text = string.Empty;
        contactPersonTextBox.Text = string.Empty;
        tbxTradeLicenseNo.Text = string.Empty;
        tbxTradeLicenseExpiryDate.Text = string.Empty;
        //shippingConditionTextBox.Text = string.Empty;

        regionDropDownList.SelectedValue = "";
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();

        ddlAIT.SelectedValue = "";

        SetNA();

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

        //if (contactTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Mobail Number!!");
        //    return false;
        //}
        
        //if (categoryNameDropDownList.SelectedIndex == 0)
        //{
        //    showMessageBox("Please Input Category  !!");
        //    return false;
        //}
       
        //if (customernameTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Customer Name  !!");
        //    return false;
        //}
        //if (addressTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Customer Address  !!");
        //    return false;
        //}
        //if (addressTextBox2.Text == "")
        //{
        //    showMessageBox("Please Input Customer Address Second  !!");
        //    return false;
        //}
        //if (contactTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Customer Contact No  !!");
        //    return false;
        //}
        //if (cityTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Customer City  !!");
        //    return false;
        //}
        //if (contactPersonTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Contact Person  !!");
        //    return false;
        //}
        //if (shippingConditionTextBox.Text == "")
        //{
        //    showMessageBox("Please Input Shipping Condition  !!");
        //    return false;
        //}


        if (regionDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Region Name!!");
            return false;
        }

        if (areaDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Area Name!!");
            return false;
        }

        if (territoryDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Territory Name!!");
            return false;
        }

        if (marketDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Market Name!!");
            return false;
        }

        if (categoryNameDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Category Name!!");
            return false;
        }

        if (paymentTypeDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Payment Type!!");
            return false;
        }
        
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

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            
            bool fixedCustomer = false;

            if (fbDropDownList.SelectedValue == "1")
            {
                fixedCustomer = true;
            }

            //string[] MarketName = marketNameDropDownList.SelectedItem.Text.Split(':');

            CustomerMaster aCompanyInfo = new CustomerMaster()
            {
                //CustomerCode = customerCodeTextBox.Text,
                CustomerName = customernameTextBox.Text,

                CategoryId = Convert.ToInt32(categoryNameDropDownList.SelectedValue),
                Address = addressTextBox.Text,
                Addrees2 = addressTextBox2.Text,
                CellNo = contactTextBox.Text,
                City = cityTextBox.Text,
                TradelicenseNo = tbxTradeLicenseNo.Text,
                ConPerson = contactPersonTextBox.Text,
                ShippingCond = shippingConditionTextBox.Text,
                MarketId = Convert.ToInt32(marketDropDownList.SelectedValue),
                TermOfPayment = paymentTypeDropDownList.SelectedValue,
                FixedCustomer = fixedCustomer,
                CreateBy = Session["LoginName"].ToString(),
                CreateDate = DateTime.Now,
                IsActive = true,
                IsVatApplicable = isVatApplicable.Checked,
                AITGLId = ddlAIT.SelectedValue != "" ? Convert.ToInt32(ddlAIT.SelectedValue) : 0,
                TradeLicenseExpiryDate = ParseNullableDate(tbxTradeLicenseExpiryDate.Text)

            };

            //aCompanyInfo.IsVatApplicable = isVatApplicable.Checked;
            

            CustomerMasterBLL aCustomerMasterBLL = new CustomerMasterBLL();
            if (aMasterInfoBll.SaveCustMasterInfo(aCompanyInfo))
            {
                int MasterIdd = aCompanyInfo.CustomerMasterId;
                string Code = aCompanyInfo.CustomerCode;
                CoaDAO aCoaMaster = new CoaDAO();
                //{
                //    AccountName = customernameTextBox.Text,
                //    CompanyId = 0,
                //    AccountCode = Code,
                //    SubsidiaryLId = Convert.ToInt32(ddlSubSubsidiaryLedger.SelectedValue),
                //    CustomerId = Convert.ToInt32(MasterIdd),
                //    CreateBy = Session["LoginName"].ToString(),
                //    CreateDate = DateTime.Now,
                //    IsActive = true
                //};

                if (aMasterInfoBll.SaveCoaMasterInfo(MasterIdd))
                {
                    showMessageBox("Data Save Successfully Customer Master Code  is :  " + aCompanyInfo.CustomerCode + " And Customer Master Name is :" + aCompanyInfo.CustomerName);
                 
                }
 


                Clear();
                //SetCustomerCode();
            }
            else
            {
                showMessageBox(" Customer Code already exist");
            }
        }
        else
        {
            showMessageBox("Please input data in all Textbox");
        }
    }
    protected void custMasterListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CustMasterView.aspx");
    }

    protected void regionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (regionDropDownList.SelectedValue != "")
        {
            aMasterInfoBll.GetAreaByRegionId(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
        }
        
    }
    protected void areaDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (areaDropDownList.SelectedValue != "")
        {
            aMasterInfoBll.GetTerritiryByAreaId(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
        }
        
    }
    protected void territoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (territoryDropDownList.SelectedValue != "")
        {
            aMasterInfoBll.GetMarketNameByTerritoryId(marketDropDownList, territoryDropDownList.SelectedValue);
        }
        else
        {
      
            marketDropDownList.Items.Clear();
        }

        
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void ddlLedgerHead_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlControlLedger.Items.Clear();
        ddlSubsidiaryLedger.Items.Clear();
        ddlSubSubsidiaryLedger.Items.Clear();
        ddlAccountName.Items.Clear();
        if (ddlLedgerHead.SelectedValue!="")
        {
            aMasterInfoBll.LoadControlCode(ddlControlLedger, ddlLedgerHead.SelectedValue);   
        }
        else
        {
            ddlControlLedger.Items.Clear();
            ddlSubsidiaryLedger.Items.Clear();
            ddlSubSubsidiaryLedger.Items.Clear();
            ddlAccountName.Items.Clear();
            
        }
    }

    protected void ddlControlLedger_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubsidiaryLedger.Items.Clear();
        ddlSubSubsidiaryLedger.Items.Clear();
        ddlAccountName.Items.Clear();
        if (ddlControlLedger.SelectedValue != "")
        {
            aMasterInfoBll.LoadSubsidaryCode(ddlSubsidiaryLedger, ddlControlLedger.SelectedValue);
        }
        else
        {
            ddlSubsidiaryLedger.Items.Clear();
            ddlSubSubsidiaryLedger.Items.Clear();
            ddlAccountName.Items.Clear();
        }
    }

    protected void ddlSubsidiaryLedger_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubSubsidiaryLedger.Items.Clear();
        ddlAccountName.Items.Clear();
        if (ddlSubsidiaryLedger.SelectedValue != "")
        {
            aMasterInfoBll.LoadSUBSubsidaryCode(ddlSubSubsidiaryLedger, ddlSubsidiaryLedger.SelectedValue);
        }
        else
        {
            ddlSubSubsidiaryLedger.Items.Clear();
            ddlAccountName.Items.Clear();
        }
    }

    protected void ddlSubSubsidiaryLedger_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubSubsidiaryLedger.SelectedValue != "")
        {
            aMasterInfoBll.LoadAccountName(ddlAccountName, ddlSubSubsidiaryLedger.SelectedValue);
        }
        else
        {
            ddlAccountName.Items.Clear();
        }
    }
}
