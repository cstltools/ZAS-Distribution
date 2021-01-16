using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CustomerFixedDiscount : System.Web.UI.Page
{
    CustomerFixedDiscountDal aDiscountDal = new CustomerFixedDiscountDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            SetStatus();

            if (fixedDiscountIdHiddenField.Value == "")
            {
                isActiveCheckBox.Checked = true;
            }
        }
    }

    private void SetStatus()
    {
        if (fixedDiscountIdHiddenField.Value != "")
        {
            isActiveCheckBox.Checked = true;
        }

        if (isActiveCheckBox.Checked == false && fixedDiscountIdHiddenField.Value != "")
        {
            newDiscount.Visible = true;
        }
        else
        {
            newDiscount.Visible = false;
        }
    }


    private void LoadDropdownList()
    {

        aDiscountDal.LoadGroupInfo(groupDropDownList);

        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);


        if (groupDropDownList.SelectedValue != "")
        {
            aDiscountDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
        }

    }


    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {

        if (custCodeTextBox.Text.Contains(':'))
        {
            custIdHiddenField.Value = "";
            string[] data = custCodeTextBox.Text.Split(':');
            custCodeTextBox.Text = data[0];
            customerNameLabel.Text = data[1];
            custIdHiddenField.Value = data[3];

        }
        else
        {
            if (custCodeTextBox.Text != "")
            {
                custIdHiddenField.Value = "";
                DataTable aTable = aDiscountDal.GetCustomerInfoByCode(custCodeTextBox.Text.Trim());

                if (aTable.Rows.Count > 0)
                {
                    custIdHiddenField.Value =
                        aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);
                    customerNameLabel.Text = aTable.Rows[0].Field<String>("CustomerName");
                }
                else
                {
                    custIdHiddenField.Value = "";
                    custCodeTextBox.Text = "";
                    customerNameLabel.Text = "";
                    ShowMessageBox("Customer does not exist or not contain enough information !!!!");
                }
            }
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (DataValidation())
        {
            CustFixedDiscountDao aDao = new CustFixedDiscountDao();

            aDao.CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue);
            aDao.CustomerMasterId = Convert.ToInt32(custIdHiddenField.Value);
            aDao.DisParcentage = Convert.ToInt32(disPerTextBox.Text.Trim());
            aDao.IsActive = isActiveCheckBox.Checked;
            aDao.EntryBy = Session["LoginName"].ToString();
            aDao.EntryDate = DateTime.Now;

            Int32 id =  aDiscountDal.SaveDiscountInfo(aDao);
            
            if (id > 0)
            {
                ShowMessageBox("Information Saved Successfully !!");
                Clear();
            }

        }
    }

    private bool DataValidation()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select company !!");
            return false;
        }

        if (custIdHiddenField.Value == "")
        {
            ShowMessageBox("Please Select customer !!");
            return false;
        }

        if (disPerTextBox.Text == "")
        {
            ShowMessageBox("Discount Percentage is required !!");
            return false;
        }

        if (fixedDiscountIdHiddenField.Value == "")
        {
            if (!isActiveCheckBox.Checked)
            {
                ShowMessageBox("Is Active status must be true !!");
                return false;
            }

            if (activeDtTextBox.Text == "")
            {
                ShowMessageBox("Active date is required !!");
                return false;
            }
        }
        else
        {
            if (inActiveDateTextBox.Text == "")
            {
                ShowMessageBox("In-Active date is required !!");
                return false;
            }
        }


        return true;
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        LoadDropdownList();
        custCodeTextBox.Text = "";
        customerNameLabel.Text = "";
        fixedDiscountIdHiddenField.Value = "";
        disPerTextBox.Text = "";
        activeDtTextBox.Text = "";

        SetStatus();
        inActiveDateTextBox.Text = "";
        newDisPerTextBox.Text = "";
        newActiveDateTextBox.Text = "";

        

    }

    protected void isActiveCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        SetStatus();
    }
}