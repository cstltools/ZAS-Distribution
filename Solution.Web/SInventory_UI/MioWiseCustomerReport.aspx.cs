using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_MioWiseCustomerReport : System.Web.UI.Page
{
    MioWiseCustomerDal aReportDal = new MioWiseCustomerDal();

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
            dcDropDownList.Items.Clear();
            mioDropDownList.Items.Clear();
        }


    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            aReportDal.LoadMIOInformation(mioDropDownList, companyNameDropDownList.SelectedValue);
            aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            mioDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();
        }
    }


   

    protected void submitButton_Click(object sender, EventArgs e)
    {
        
        PopUpCustomerReport();
        
    }



    private void PopUpCustomerReport()
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        const string url = "../SInventory_RPTVIEW/MioWiseCustomerReportViewer.aspx";
        const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private String GetPrameterList()
    {
        string parameter = " WHERE MIO.IsActive = 1 ";


        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        //if (dcDropDownList.SelectedValue != "")
        //{
        //    parameter = parameter + " AND tblInvoice.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
        //}

        if (mioDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND MIO.MIOId = '" + mioDropDownList.SelectedValue + "'";
        }

       


        return parameter;

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

    private void Clear()
    {
        LoadDropdownList();
        dcDropDownList.Items.Clear();
        mioDropDownList.Items.Clear();
        //fromDateTextBox.Text = "";
        //toDateTextBox.Text = "";
    }
}