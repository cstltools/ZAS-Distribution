using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CustomerCreditLimitEntry : System.Web.UI.Page
{
    CreditLimitDAL aCreditLimitDal=new CreditLimitDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownList();
        }
    }

    private void Clear()
    {
        customerTextBox.Text = string.Empty;
        customeridHiddenField.Value = string.Empty;
        companyDropDownList.SelectedIndex = 0;
        limitAmntTextBox.Text = string.Empty;
        perdayLimitTextBox.Text = string.Empty;
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (companyDropDownList.SelectedIndex == 0)
        {
            ShowMessageBox("Please Select a Company!!");
            return false;
        }

        if (customeridHiddenField.Value == string.Empty)
        {
            ShowMessageBox("Please Select a Customer!!");
            return false;
        }

        if (limitAmntTextBox.Text == string.Empty)
        {
            ShowMessageBox("Please Customer Credit Limit");
            return false;
        }

        //if (perdayLimitTextBox.Text == string.Empty)
        //{
        //    ShowMessageBox("Please Input Customer Per Day Limit ");
        //    return false;
        //}

        

        return true;
    }


    public void DropDownList()
    {
        aCreditLimitDal.GetCompanyDropdownList(companyDropDownList);
        //aCreditLimitDal.GetCustomerDropdownList(customerComboBox);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            CreditLimitDAO aCreditLimitDao = new CreditLimitDAO()
            {
                CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue),
                CustomerMasterId = Convert.ToInt32(customeridHiddenField.Value),
                LimitAmount = Convert.ToDecimal(limitAmntTextBox.Text),
                //DayLimit = Convert.ToInt32(perdayLimitTextBox.Text),
                DayLimit = 0,
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now,
                IsActive = true

            };
            bool status = aCreditLimitDal.SaveCreditLimit(aCreditLimitDao);
            if (status)
            {
                ShowMessageBox("Data Saved Successfully");    
                Clear();
            }
            else
            {
                ShowMessageBox("Data not saved !!!!!!!!!!!!");
            }
        }
    }
    protected void customerTextBox_TextChanged(object sender, EventArgs e)
    {
        if (customerTextBox.Text.Contains(':'))
        {
            string[] text = customerTextBox.Text.Split(':');
            customerTextBox.Text = text[1];
            customeridHiddenField.Value = text[3];
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }
}