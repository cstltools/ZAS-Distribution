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

public partial class SInventory_UI_DistributionCenterInfoEdit : System.Web.UI.Page
{
    DistributionCeneterInfoBll _companyUnitBll = new DistributionCeneterInfoBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
            salesCenterInfoIdHiddenField.Value = Request.QueryString["ID"];
            ComUnitLoad(salesCenterInfoIdHiddenField.Value);
        }
    }



    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (salesCenternameTextBox.Text != ""  && addressTextBox.Text != ""  && companyNameDropDownList.SelectedValue != "")
        {
            var aCompanyUnit = new CompanyUnit()
            {
                ComUnitId = Convert.ToInt32(salesCenterInfoIdHiddenField.Value),
                ComUnitName = salesCenternameTextBox.Text.Trim(),
                ShortName = dcShortNameTextBox.Text,
                Address = addressTextBox.Text,
                PhoneNo = phoneNoTextBox.Text,
                MobileNo = mobileNoTextBox.Text,
                FaxNo = faxNoTextBox.Text,
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                DcTypeId = Convert.ToInt32(ddlDcType.SelectedValue),
                UpdateBy = Session["LoginName"].ToString(),
                UpdateDate = DateTime.Now
            };


            if (!_companyUnitBll.UpdateDataForSalesCenter(aCompanyUnit))
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

    private void ComUnitLoad(string salesCenterId)
    {
        var companyUnit = new CompanyUnit();

        companyUnit = _companyUnitBll.SalesCenterEditLoad(salesCenterId);

        salesCenternameTextBox.Text = companyUnit.ComUnitName;
        dcShortNameTextBox.Text = companyUnit.ShortName;
        addressTextBox.Text = companyUnit.Address;
        phoneNoTextBox.Text = companyUnit.PhoneNo;
        mobileNoTextBox.Text = companyUnit.MobileNo;
        faxNoTextBox.Text = companyUnit.FaxNo;
        companyNameDropDownList.SelectedValue = companyUnit.CompanyId.ToString(CultureInfo.InvariantCulture);
        ddlDcType.SelectedValue = companyUnit.DcTypeId.ToString(CultureInfo.InvariantCulture);
        nameHiddenField.Value = companyUnit.ComUnitName;
        shortNameHiddenField.Value = companyUnit.ShortName;

    }

    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }

    private void LoadDropDown()
    {
        _companyUnitBll.LoadCompanyName(companyNameDropDownList);
        _companyUnitBll.LoadDcType(ddlDcType);
    }

    protected void salesCenternameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dcInfo = _companyUnitBll.CheckDCInfoExistOrNot(salesCenternameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(dcInfo, salesCenternameTextBox, nameHiddenField, "DC Name already Exist !!!");
    }


    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("ComUnitId") != Convert.ToInt32(salesCenterInfoIdHiddenField.Value))
                {
                    count++;
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

    protected void dcShortNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = _companyUnitBll.CheckDCShortNameExistOrNot(dcShortNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, dcShortNameTextBox,shortNameHiddenField ,"DC short Name already Exist !!!");
    }
}