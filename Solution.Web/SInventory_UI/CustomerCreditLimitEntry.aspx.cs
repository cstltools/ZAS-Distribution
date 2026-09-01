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

public partial class SInventory_UI_CustomerCreditLimitEntry : System.Web.UI.Page
{
    CreditLimitDAL aCreditLimitDal=new CreditLimitDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownList();

            if (Session["CreditId"] != null)
            {
                GetOneRecord(Session["CreditId"].ToString());
                Session["CreditId"] = null;
            }
        }
    }

    private void GetOneRecord(string creditId)
    {
        DataTable aTable = aCreditLimitDal.GetCustomerCreditInfoById(creditId);

        if (aTable.Rows.Count > 0)
        {
            creditHiddenField.Value = aTable.Rows[0].Field<Int32>("CreditLimitId").ToString(CultureInfo.InvariantCulture);
            companyDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);
            customerTextBox.Text = aTable.Rows[0].Field<String>("CustomerName").ToString(CultureInfo.InvariantCulture);
            customeridHiddenField.Value = aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);
            limitAmntTextBox.Text = aTable.Rows[0].Field<Decimal>("LimitAmount").ToString(CultureInfo.InvariantCulture);
            limitDaysTextBox.Text = aTable.Rows[0].Field<Int32>("DayLimit").ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(aTable.Rows[0]["IsActive"].ToString()))
            {
                if (Convert.ToBoolean(aTable.Rows[0]["IsActive"]))
                {
                    isActiveCheckBox.Checked = true;
                }

            }
            else
            {
                isActiveCheckBox.Checked = false;
            }
            
            if (!string.IsNullOrEmpty(aTable.Rows[0]["StartDate"].ToString()))
            {
                fromDateTextBox.Text = aTable.Rows[0].Field<DateTime>("StartDate").ToString("dd-MMM-yyyy");
            }

            if (!string.IsNullOrEmpty(aTable.Rows[0]["EndDate"].ToString()))
            {
                toDateTextBox.Text = aTable.Rows[0].Field<DateTime>("EndDate").ToString("dd-MMM-yyyy");
            }

            Button1.Text = "Update";

            if (Session["LoginName"] != null && string.Equals(Session["LoginName"].ToString().Trim(), "ZAS643", StringComparison.OrdinalIgnoreCase))
            {
                customerTextBox.Enabled = false;
            }
        }
    }

    private void Clear()
    {
        creditHiddenField.Value = "";
        customerTextBox.Text = string.Empty;
        customeridHiddenField.Value = string.Empty;
        companyDropDownList.SelectedIndex = 0;
        limitAmntTextBox.Text = string.Empty;
        perdayLimitTextBox.Text = string.Empty;
        limitDaysTextBox.Text = string.Empty;
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
        Button1.Text = "Save";
        customerTextBox.Enabled = true;
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
            ShowMessageBox("Please select Customer Credit Limit");
            return false;
        }

        if (limitDaysTextBox.Text == string.Empty)
        {
            ShowMessageBox("Please select Customer Day Limit");
            return false;
        }

        //if (fromDateTextBox.Text == "")
        //{
        //    ShowMessageBox("Please start date");
        //    return false;
        //}

        //if (toDateTextBox.Text == "")
        //{
        //    ShowMessageBox("Please to date");
        //    return false;
        //}

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

            if (creditHiddenField.Value == "")
            {
                CreditLimitDAO aCreditLimitDao = new CreditLimitDAO();
                //{
                aCreditLimitDao.CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue);
                aCreditLimitDao.CustomerMasterId = Convert.ToInt32(customeridHiddenField.Value);
                aCreditLimitDao.LimitAmount = Convert.ToDecimal(limitAmntTextBox.Text.Trim());
                aCreditLimitDao.DayLimit = Convert.ToInt32(limitDaysTextBox.Text.Trim());
                aCreditLimitDao.EntryBy = Session["LoginName"].ToString();
                aCreditLimitDao.EntryDate = DateTime.Now;
                aCreditLimitDao.IsActive = isActiveCheckBox.Checked;

                    //StartDate = Convert.ToDateTime(fromDateTextBox.Text),
                    //EndDate = Convert.ToDateTime(toDateTextBox.Text)

                //};

                DataTable exisTable = 
                aCreditLimitDal.CheckExistorNot(Convert.ToInt32(companyDropDownList.SelectedValue),
                    Convert.ToInt32(customeridHiddenField.Value));

                if (exisTable.Rows.Count == 0)
                {
                    bool status = aCreditLimitDal.SaveCreditLimit(aCreditLimitDao);

                    if (status)
                    {
                        ShowMessageBox("Data Saved Successfully");
                        Clear();
                    }
                }
                else
                {
                    ShowMessageBox("Data Already Exist !");
                }
            }
            else
            {
                var aCreditLimitDao = new CreditLimitDAO()
                {
                    CreditLimitId = Convert.ToInt32(creditHiddenField.Value),
                    CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue),
                    CustomerMasterId = Convert.ToInt32(customeridHiddenField.Value),
                    LimitAmount = Convert.ToDecimal(limitAmntTextBox.Text),
                    DayLimit = Convert.ToInt32(limitDaysTextBox.Text.Trim()),
                    IsActive = isActiveCheckBox.Checked,
                    UpdateBy = Session["LoginName"].ToString(),
                    UpdateDate = DateTime.Now,
                    //StartDate = Convert.ToDateTime(fromDateTextBox.Text),
                    //EndDate = Convert.ToDateTime(toDateTextBox.Text)

                };
                

                DataTable exisTable = aCreditLimitDal.CheckExistorNotUpdate(Convert.ToInt32(companyDropDownList.SelectedValue),
                   Convert.ToInt32(customeridHiddenField.Value), Convert.ToInt32(creditHiddenField.Value));


                if (exisTable.Rows.Count == 0)
                {
                    bool status = aCreditLimitDal.UpdateCreditLimit(aCreditLimitDao);

                    if (status)
                    {
                        ShowMessageBox("Data Updated Successfully");
                        Clear();
                    }
                }
                else
                {
                    ShowMessageBox("Data Already Exist !");
                }
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

    protected void custMasterListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CustomerCreditView.aspx");
    }
}