using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_DCStockInfoReport : System.Web.UI.Page
{
    DCStockInfoReprotDal aReportDal = new DCStockInfoReprotDal();

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
        LoadCompanyList();


    }

    private void LoadCompanyList()
    {
        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();

        }
    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            dcDropDownList.Items.Clear();
        }
    }


   

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            PopUpDCStocktReport();
        }
    }

    private bool Validation()
    {
        if (groupDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select group Name !!");
            return false;
        }

        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select company Name !!");
            return false;
        }

        //if (dcDropDownList.SelectedValue == "")
        //{
        //    ShowMessageBox("Please Select DC Name !!");
        //    return false;
        //}

        return true;
    }

    private void PopUpDCStocktReport()
    {
        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        const string url = "../SInventory_RPTVIEW/DCStockInfoReportViewer.aspx";
        const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private String GetPrameterList()
    {
        string parameter = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND Cu.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        if (dcDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND vTblTotal.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
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
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);
        LoadCompanyList();
        dcDropDownList.Items.Clear();
    }
}