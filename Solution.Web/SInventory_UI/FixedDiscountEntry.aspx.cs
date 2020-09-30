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

public partial class SInventory_UI_FixedDiscountEntry : System.Web.UI.Page
{
    FixedDiscountEntryDal aEntryDal = new FixedDiscountEntryDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetDefaultValue();
            LoadDropdownList();
        }
    }

    private void SetDefaultValue()
    {
        isActiveCheckBox.Checked = true;
    }

    private void LoadDropdownList()
    {

        aEntryDal.LoadGroupInfo(groupDropDownList);

        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);


        if (groupDropDownList.SelectedValue != "")
        {
            aEntryDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
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
            custIdHiddenField.Value = data[3];
            custNameLabel.Text = data[1];
        }
        else
        {
            if (custCodeTextBox.Text != "")
            {
                custIdHiddenField.Value = "";
                DataTable aTable = aEntryDal.GetCustomerInfoByCode(custCodeTextBox.Text.Trim());

                if (aTable.Rows.Count > 0)
                {
                    custIdHiddenField.Value =
                        aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);
                    custNameLabel.Text = aTable.Rows[0].Field<String>("CustomerName");
                }
                else
                {
                    custIdHiddenField.Value = "";
                    custCodeTextBox.Text = "";
                    custNameLabel.Text = "";
                    ShowMessageBox("Customer does not exist or not contain enough information !!!!");
                }
            }
        }

        CheckDuplicate();


    }

    private void CheckDuplicate()
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (custIdHiddenField.Value != "")
            {
                DataTable aTable = aEntryDal.CheckDuplicate(custIdHiddenField.Value,
                    companyNameDropDownList.SelectedValue);

                if (aTable.Rows.Count > 0)
                {
                    companyNameDropDownList.SelectedValue = "";
                    custIdHiddenField.Value = "";
                    ShowMessageBox("Discount already exist !!!");
                }
            }
            else
            {
                ShowMessageBox("Please select customer !!");
            }
        }
        else
        {
            ShowMessageBox("Please select company !!!");
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
        if (Validation())
        {
            var aDao = new FixedDiscountDao();

            aDao.CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue);
            aDao.CustMasterId = Convert.ToInt32(custIdHiddenField.Value);
            aDao.DiscountParcentage = Convert.ToDecimal(tbxDiscount.Text);
            aDao.IsActive = true;
            aDao.ActionStatus = "Posted";
            aDao.EntryBy = Session["LoginName"].ToString();
            aDao.EntryDate = DateTime.Now;

            if (aEntryDal.SaveDiscount(aDao))
            {
                ShowMessageBox("Info saved successfully !!!");
                Clear();
            }
        }
    }

    private void Clear()
    {
        SetDefaultValue();
        LoadDropdownList();
        custIdHiddenField.Value = "";
        tbxDiscount.Text = "";
        custNameLabel.Text = "";
        custCodeTextBox.Text = "";
    }

    private bool Validation()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("You should select company !!!");
            return false;
        }

        if (custIdHiddenField.Value == "")
        {
            ShowMessageBox("You should select customer !!!");
            return false;
        }

        if (tbxDiscount.Text == "")
        {
            ShowMessageBox("You should select discount percentage !!!");
            return false;
        }

        return true;
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CheckDuplicate();
    }

    protected void CustMasterNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("FixedDiscountView.aspx");
    }
}