using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_FieldForceSalesSummery : System.Web.UI.Page
{
    FieldForceSummeryDal aReportDal = new FieldForceSummeryDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            aReportDal.GetAreaOnDropdownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();

        }
    }

    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (areaDropDownList.SelectedValue != "")
        {
            aReportDal.GetTerritoryOnDropdownList(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
        }
    }

    private void LoadDropdownList()
    {

        aReportDal.LoadGroupInfo(groupDropDownList);

        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);


        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(ddlCompany, groupDropDownList.SelectedValue);
        }
        else
        {
            ddlCompany.Items.Clear();
            dcDropDownList.Items.Clear();

        }


        aReportDal.LoadRegionName(regionDropDownList);


        //if (Session["UserType"].ToString() != "Admin")
        //{
        //    ddlCompany.Enabled = false;
        //}
        //else
        //{
        //    ddlCompany.Enabled = true;
        //}

    }



    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {

        string userType = Session["UserId"].ToString();

        if (ddlCompany.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aReportDal.LoadDcInfoByUser(dcDropDownList, ddlCompany.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadDcInfo(dcDropDownList, ddlCompany.SelectedValue);
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


    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            PopUpCustomerPaymentReport();
        }
    }

    private bool Validation()
    {

        if (groupDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select group Name !!");
            return false;
        }


        string var = Session["IsHead"].ToString();

        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserType"].ToString() != "Admin" && Session["IsHead"].ToString() != "True")
            {
                if (dcDropDownList.SelectedValue == "")
                {
                    ShowMessageBox("Please Select DC !!");
                    return false;
                }
            }
        }


        if (fromDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select invoice from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select invoice To date !!");
            return false;
        }



        return true;
    }

    private void PopUpCustomerPaymentReport()
    {
        Session["FDate"] = "";
        Session["TDate"] = "";
        Session["pram"] = "";

        Session["pram"] = GenerateParameter();

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            Session["FDate"] = fromDateTextBox.Text.Trim();
            Session["TDate"] = toDateTextBox.Text.Trim();
        }


        //Session["PRAM"] = GetPrameterList();

        string url = "../SInventory_RPTVIEW/FieldForceSalesSummeryReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private string GenerateParameter()
    {
        string param = "";

        if (fromDateTextBox.Text.Trim() != "")
        {
            if (toDateTextBox.Text.Trim() != "")
            {
                param = param + " AND SLS.InvoiceDate BETWEEN '" + fromDateTextBox.Text.Trim() + "' AND '" +
                        toDateTextBox.Text.Trim() + "'";
            }
        }


        if (ddlCompany.SelectedIndex > 0)
        {
            param = param + "  AND SLS.CompanyId = '" + ddlCompany.SelectedValue + "' ";
        }

        if (dcDropDownList.SelectedIndex > 0)
        {
            param = param + " AND SLS.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
        }

        if (regionDropDownList.SelectedIndex > 0)
        {
            param = param + " AND SLS.RegionId = '" + regionDropDownList.SelectedValue + "'";
        }

        if (areaDropDownList.SelectedIndex > 0)
        {
            param = param + " AND SLS.AreaId = '" + areaDropDownList.SelectedValue + "'";
        }

        if (territoryDropDownList.SelectedIndex > 0)
        {
            param = param + " AND SLS.TerritoryId = '" + territoryDropDownList.SelectedValue + "'";
        }

        

        return param;
    }

    //private String GetPrameterList()
    //{
    //    string parameter = "";


    //    if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
    //    {
    //        parameter = parameter + " AND INV.InvoiceDate BETWEEN '" + Convert.ToDateTime(fromDateTextBox.Text.Trim()) + "' AND '" + Convert.ToDateTime(toDateTextBox.Text.Trim()) + "'";
    //    }

    //    return parameter;

    //}

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
        groupDropDownList.SelectedValue = "";
        ddlCompany.Items.Clear();
        dcDropDownList.Items.Clear();

        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
    }
}