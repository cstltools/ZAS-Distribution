using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.PettyCash_BLL;

public partial class SInventory_UI_CreditCustomerReport : System.Web.UI.Page
{

    PettyCashBLL aCashBll = new PettyCashBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoaddropdownList();
        }
    }

    

    private void LoaddropdownList()
    {

        aCashBll.LoadCompany(CompanyDropDown);
        
    }



    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {

        string Perticular = custCodeTextBox.Text.Trim();


        if (Perticular.Contains(':'))
        {
            string[] emp = Perticular.Split(':');
            //  TextBox txtPerticular = (TextBox)itemGridView.Rows[rowindex].Cells[1].FindControl("txtPerticular");
            custCodeTextBox.Text = emp[1];

            CustomerIdHiddenField.Value = emp[3];



        }
        else
        {
            
            custCodeTextBox.Text = "";
            CustomerIdHiddenField.Value = "";
            //showMessageBox("Input Correct Data !");
        }
    }


    private String GenerateParam()
    {
        string param = "";


        if (CompanyDropDown.SelectedValue != "")
        {
            param = param + "And tblCustomerCreditLimit.CompanyId ='" + CompanyDropDown.SelectedValue + "' ";
        }


        if (CustomerIdHiddenField.Value != "")
        {
            param = param + " And tblCustomerCreditLimit.CustomerMasterId ='" + CustomerIdHiddenField.Value + "' ";
        }

        return param;
    }

    
    protected void viewRptButton_Click(object sender, EventArgs e)
    {


       
            Session["PRAM"] = "";
            Session["PRAM"] = GenerateParam();

            const string url = "../SInventory_RPTVIEW/CreditCustomerViewer.aspx";
            const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            
        
       
    }

    protected void clearButton_Click(object sender, EventArgs e)
    {
        LoaddropdownList();
        custCodeTextBox.Text = "";
        CustomerIdHiddenField.Value = "";
    }




}