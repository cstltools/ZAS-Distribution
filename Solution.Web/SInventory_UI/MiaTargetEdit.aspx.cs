using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_MiaTargetEdit : System.Web.UI.Page
{
    MiaTargetBLL MiaTargetBLL = new MiaTargetBLL();
    MiaTargetDAL aReportDal = new MiaTargetDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            LoadDropdownList();
            miaTargetInfoIdHiddenField.Value = Request.QueryString["ID"];
            MiaTargetLoad(miaTargetInfoIdHiddenField.Value);           
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
    
    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "" && periodDropDownList.SelectedValue != "" && mioDropDownList.SelectedValue != "" && miaTargetAmountTextBox.Text != "")
        {
            MiaTarget aMiaTarget = new MiaTarget()
            {
                MiaTargetId = Convert.ToInt32(miaTargetInfoIdHiddenField.Value),
                MiaId = Convert.ToInt32(mioDropDownList.SelectedValue),
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                MiaTargetAmount = Convert.ToDecimal(miaTargetAmountTextBox.Text.Trim()),
                Period = periodDropDownList.SelectedItem.Text.Trim(),
                Year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)
            };

            MiaTargetBLL aMiaTargetBLL = new MiaTargetBLL();

            if (!aMiaTargetBLL.UpdateDataForMiaTarget(aMiaTarget))
            {
                MessageLabel.Text = "Data Not Update!!!";
                MessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                MessageLabel.Text = "Data Update Successfully!!! Please Reload";
                MessageLabel.ForeColor = System.Drawing.Color.Green;
            }
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }

    private void MiaTargetLoad(string miaId)
    {
        MiaTarget MiaTarget = new MiaTarget();
        MiaTarget = MiaTargetBLL.MiaTargetEditLoad(miaId);
        companyNameDropDownList.SelectedValue = MiaTarget.CompanyId.ToString(CultureInfo.InvariantCulture);

        if (companyNameDropDownList.SelectedValue != "")
        {
            aReportDal.LoadMIOInformation(mioDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            mioDropDownList.Items.Clear();
        }

        mioDropDownList.SelectedValue = MiaTarget.MiaId.ToString(CultureInfo.InvariantCulture);
        miaTargetAmountTextBox.Text = MiaTarget.MiaTargetAmount.ToString(CultureInfo.InvariantCulture);
        periodDropDownList.SelectedItem.Text = MiaTarget.Period;
    }


    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);

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

}