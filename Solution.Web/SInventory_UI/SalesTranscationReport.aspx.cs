using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_WingsWiseSalesReport : System.Web.UI.Page
{
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();

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
            
        }

    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")  
                {
                    aReportDal.LoadDcInfoByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
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

        //if (Session["UserType"].ToString() != "")
        //{
        //    if (Session["UserType"].ToString() != "Admin")
        //    {
        //        if (dcDropDownList.SelectedValue == "")
        //        {
        //            ShowMessageBox("Please Select DC !!");
        //            return false; 
        //        }
        //    }
        //}


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
       

        Session["PRAM"] = "";
        Session["PRAM"] = GetPrameterList();

        //Session["PRAM"] = GetPrameterList();

        string url = "../SInventory_RPTVIEW/SalesTransctionReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private String GetPrameterList()
    {
        string parameter = "WHERE INV.InvoiceNo IS NOT NULL AND INVD.Quantity > 0 ";

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            parameter = parameter + " AND INV.InvoiceDate BETWEEN '" + (fromDateTextBox.Text.Trim()) + "'  AND '" + (toDateTextBox.Text.Trim()) + "'";
        }

        if (dcDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND UNT.ComUnitId = '" + dcDropDownList.SelectedValue + "'";
        }

        if (companyNameDropDownList.SelectedValue != "")
        {
            parameter = parameter + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        parameter = parameter + " AND CI.CompanyId IN ( SELECT DISTINCT CompanyId FROM tblUserCompanyUnit AS CU LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = " + Convert.ToInt32(Session["UserId"]) + ")";

        return parameter;
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
        companyNameDropDownList.Items.Clear();
        dcDropDownList.Items.Clear();
        
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";
    }

    protected void gridButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            DataTable aTable = aReportDal.LoadSalesTranscationInfo(GetPrameterList());

            if (aTable.Rows.Count > 0)
            {
                batchGridView.DataSource = aTable;
                batchGridView.DataBind();
            }
            else
            {
                batchGridView.DataSource = null;
                batchGridView.DataBind();
            }
        }
    }
}