using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_MiaTargetEntry : System.Web.UI.Page
{
    MiaTargetBLL aMiaTargetBLL = new MiaTargetBLL();
    MiaTargetDAL aReportDal = new MiaTargetDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {

        aReportDal.LoadGroupInfo(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);

        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
            mioDropDownList.Items.Clear();
        }


    }
    
    private void Clear()
    {
        companyNameDropDownList.SelectedValue = "";
        periodDropDownList.SelectedValue = "";
        mioDropDownList.Items.Clear();
        miaTargetAmountTextBox.Text = string.Empty;

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

        if (companyNameDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Company!!");
            return false;
        }

        if (mioDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select MIA!!");
            return false;
        }

        if (miaTargetAmountTextBox.Text == "")
        {
            showMessageBox("Please Input Target Amount!!");
            return false;
        }

        return true;
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            MiaTarget aMiaTarget = new MiaTarget()
            {
                MiaId = Convert.ToInt32(mioDropDownList.SelectedValue),
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                MiaTargetAmount = Convert.ToDecimal(miaTargetAmountTextBox.Text.Trim()),
                Period = periodDropDownList.SelectedItem.Text.Trim(),
                Year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)
            };

            MiaTargetBLL aMiaTargetBLL = new MiaTargetBLL();

            if (aMiaTargetBLL.SaveMiaTarget(aMiaTarget))
            {
                showMessageBox("Data Save Successfully !!!");
            }
            Clear();
        }
        else
        {
            showMessageBox("Please input data in all Textbox !!!");
        }
    }

   
    protected void miaViewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("MiaTargetView.aspx");
    }
    
    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            aReportDal.LoadMIOInformation(mioDropDownList, companyNameDropDownList.SelectedValue);          
        }
        else
        {
            mioDropDownList.Items.Clear();
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }
}