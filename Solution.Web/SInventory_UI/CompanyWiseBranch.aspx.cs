using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CompanyWiseBranch : System.Web.UI.Page
{
    CompanywisebranchDal aDal = new CompanywisebranchDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Loaddropdownlist();
        }
    }

    private void Loaddropdownlist()
    {
        aDal.LoadCompany(companyNameDropDownList);
        aDal.LoadBank(ddlBank);

    }


    protected void ddlBank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBank.SelectedValue != "")
        {
            aDal.LoadBranch(ddlBranch, ddlBank.SelectedValue);
        }
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validattion())
        {
            var aDepositDao = new CompanyWiseDepositDao
            {
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                BranchId = Convert.ToInt32(ddlBranch.SelectedValue),
                Amount = Convert.ToDecimal(amount.Text.Trim()),
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now,
                DepositDate = Convert.ToDateTime(dateTextBox.Text.Trim()),
                IsDelete = false
            };

            if (aDal.SaveDepositInfo(aDepositDao) > 0)
            {
                ShowMessageBox("Info saved successfully !!!");
                Clear();
            }
        }
    }

    private bool Validattion()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("You should select company !!");
            return false;
        }

        if (ddlBranch.SelectedValue == "")
        {
            ShowMessageBox("You should select branch !!");
            return false;
        }

        if (dateTextBox.Text == "")
        {
            ShowMessageBox("You should select deposit date !!");
            return false;
        }

        if (amount.Text == "")
        {
            ShowMessageBox("You should select amount !!");
            return false;
        }

        return true;

    }

    protected void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
       Clear();
    }

    private void Clear()
    {
        Loaddropdownlist();
        ddlBranch.Items.Clear();
        amount.Text = "";
        dateTextBox.Text = "";
    }
}