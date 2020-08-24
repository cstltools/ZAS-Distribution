using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CompanyInformationEdit : System.Web.UI.Page
{
    CompanyInfoBLL aCompanyInfoBll = new CompanyInfoBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadDropDownList();

        if (!IsPostBack)
        {
            companyInfoIdHiddenField.Value = Request.QueryString["ID"];
            CompanyInfoLoad(companyInfoIdHiddenField.Value);
        }

    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private void LoadDropDownList()
    {
        aCompanyInfoBll.LoadGroupDropdownList(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);
    }

    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (companyNameTextBox.Text != "" && cmpShortNameTextBox.Text != "" && addressNameTextBox.Text != "" && contactNoTextBox.Text != "")
        {

            var aCompanyInfo = new CompanyInformation()
            {
                CompanyId = Convert.ToInt32(companyInfoIdHiddenField.Value),
                GroupId = Convert.ToInt32(groupDropDownList.SelectedValue),
                CompanyName = companyNameTextBox.Text.Trim(),
                CmpShortName = cmpShortNameTextBox.Text.Trim(),
                Address = addressNameTextBox.Text.Trim(),
                ContactNo = contactNoTextBox.Text.Trim(),
                FaxNo = faxNoTextBox.Text.Trim(),
                Remarks = remarksTextBox.Text.Trim(),
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now

            };

            if (!aCompanyInfoBll.UpdateDataForCompanyInfo(aCompanyInfo))
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

    private void CompanyInfoLoad(string comapnyInfoId)
    {
        var aCompanyInfo = new CompanyInformation();

        aCompanyInfo = aCompanyInfoBll.CompanyInfoEditLoad(comapnyInfoId);
        cmpShortNameTextBox.Text = aCompanyInfo.CmpShortName;
        companyNameTextBox.Text = aCompanyInfo.CompanyName;
        addressNameTextBox.Text = aCompanyInfo.Address;
        contactNoTextBox.Text = aCompanyInfo.ContactNo;
        faxNoTextBox.Text = aCompanyInfo.FaxNo;
        remarksTextBox.Text = aCompanyInfo.Remarks;

        cmpNameHiddenField.Value = aCompanyInfo.CompanyName;
        cmpShortHiddenField.Value = aCompanyInfo.CmpShortName;

    }


    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);

    }

    protected void companyNameTextBox_OnTextChanged(object sender, EventArgs e)
    {

        if (cmpNameHiddenField.Value != companyNameTextBox.Text.Trim())
        {
            DataTable companyInfo = aCompanyInfoBll.CheckCompanyInfoExistOrNot(companyNameTextBox.Text.Trim());
            GotoUpdatePermissionSetUp(companyInfo, companyNameTextBox, cmpNameHiddenField, "Company Name already Exist !!!");
        }
       
    }

    protected void cmpShortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable companyInfo = aCompanyInfoBll.CheckCompanyShortNameExistOrNot(cmpShortNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(companyInfo, cmpShortNameTextBox,cmpShortHiddenField ,"Company short Name already Exist !!!");
    }

    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("CompanyId") != Convert.ToInt32(companyInfoIdHiddenField.Value))
                {
                    count ++;
                }

                if (count > 0)
                {
                    textBox.Text = presentData.Value;
                    ShowMessageBox(msg);
                    break;
                }
            }
           
        }
    }
}