using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_DepositSlipEntry : System.Web.UI.Page
{
    DepositSlipEntryDal aDal = new DepositSlipEntryDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
        aDal.LoadManufac(companyNameDropDownList);
        aDal.LoadBank(ddlBank);
    }

    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aDal.GetDcInformationByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());

                }
                else
                {
                    aDal.GetDCInformation(dcDropDownList, companyNameDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        else
        {
            dcDropDownList.Items.Clear();
        }
    }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CheckBoxList1.SelectedValue == "Cash" || CheckBoxList1.SelectedValue == "Online")
        {
            branch.Visible = false;
            chkNo.Visible = false;
            chkDate.Visible = false;
        }
        else
        {

            branch.Visible = true;
            chkNo.Visible = true;
            chkDate.Visible = true;
        }
    }
   

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validattion())
        {
            var aDepositDao = new CompanyWiseDepositDao();

            aDepositDao.DepotId = Convert.ToInt32(dcDropDownList.SelectedValue);

            aDepositDao.Amount = Convert.ToDecimal(amount.Text.Trim());
            aDepositDao.EntryBy = Session["LoginName"].ToString();
            aDepositDao.EntryDate = DateTime.Now;
            aDepositDao.DepositDate = Convert.ToDateTime(dateTextBox.Text.Trim());
            aDepositDao.IsDelete = false;
            aDepositDao.Remarks = remarksTextBox.Text;
            

            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    aDepositDao.DepositType = CheckBoxList1.Items[i].Text.Trim();
                }
            }

            if (aDepositDao.DepositType == "Cash" || aDepositDao.DepositType == "Online")
            {
                aDepositDao.BankId = Convert.ToInt32(ddlBank.SelectedValue);
                aDepositDao.AccountName = accNameTextBox.Text;
            }
            else
            {
                aDepositDao.BankId = Convert.ToInt32(ddlBank.SelectedValue);
                aDepositDao.AccountName = accNameTextBox.Text;
                aDepositDao.BranchName = branchTextBox.Text;
                aDepositDao.CheckNumber = chkTextBox.Text;
                aDepositDao.CheckDate = Convert.ToDateTime(chkDateTextBox.Text.Trim());

            }

            if (aDal.SaveDepositInfo(aDepositDao) > 0)
            {
                ShowMessageBox("Info Saved Successfully !!!");
                Clear();
            }
        }
    }

    private void Clear()
    {
        LoadDropdownList();
        dcDropDownList.Items.Clear();
        ddlBranch.Items.Clear();
        accNameTextBox.Text = "";
        chkTextBox.Text = "";
        chkDateTextBox.Text = "";
        branchTextBox.Text = "";
        amount.Text = "";
        dateTextBox.Text = "";
        remarksTextBox.Text = "";

        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
        {
            if (CheckBoxList1.Items[i].Selected)
            {
                CheckBoxList1.Items[i].Selected = false;
            }
        }

        branch.Visible = true;
        chkNo.Visible = true;
        chkDate.Visible = true;


    }


    private bool Validattion()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("You should select sales center !!");
            return false;
        }

        int count = 0;
        string text = "";

        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
        {
            if (CheckBoxList1.Items[i].Selected)
            {
                count++;
                text = CheckBoxList1.Items[i].Text.Trim();
            }
        }

        if (count == 0)
        {
            ShowMessageBox("You should select deposit type !!!");
            return false;
        }

        if (text != "")
        {
            if (text == "Cash" || text == "Online")
            {
                if (ddlBank.SelectedValue == "")
                {
                    ShowMessageBox("You should select bank !!");
                    return false;
                }

                if (accNameTextBox.Text == "")
                {
                    ShowMessageBox("You should select account name !!!");
                    return false;
                }
            }
            else
            {
                if (ddlBank.SelectedValue == "")
                {
                    ShowMessageBox("You should select bank !!");
                    return false;
                }

                if (accNameTextBox.Text == "")
                {
                    ShowMessageBox("You should select account name !!!");
                    return false;
                }

                if (branchTextBox.Text == "")
                {
                    ShowMessageBox("You should select branch name !!!");
                    return false;
                }

                if (chkTextBox.Text == "")
                {
                    ShowMessageBox("You should select check no !!!");
                    return false;
                }

                if (chkDateTextBox.Text == "")
                {
                    ShowMessageBox("You should select check date !!!");
                    return false;
                }
            }
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


    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }


    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("DepositList.aspx");
    }
}