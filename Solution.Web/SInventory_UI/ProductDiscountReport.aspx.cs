using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.PettyCash_BLL;

public partial class SInventory_UI_ProductDiscountReport : System.Web.UI.Page
{

    PettyCashBLL aCashBll = new PettyCashBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Gridview();
            LoaddropdownList();
        }
    }


    private void LoaddropdownList()
    {

        aCashBll.LoadCompany(CompanyDropDownlist);

    }


    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    public void Gridview()
    {
        DataTable dt = aCashBll.CheckProductDisCount();
        loadGridView.DataSource = dt;
        loadGridView.DataBind();
    }



    private void clear()
    {

        CustomerIdHiddenField.Value = string.Empty;
    }


    protected void viewRptButton_Click(object sender, EventArgs e)
    {
        //int rowindex = Convert.ToInt32(e.ToString());

        //string Status = loadGridView.DataKeys[rowindex][0].ToString();
        //Session["ActionStatus"] = Status;

        //bool IsActive =Convert.ToBoolean( loadGridView.DataKeys[rowindex][1].ToString());
        //Session["IsActive"] = IsActive;

            Session["PRAM"] = "";
            Session["PRAM"] = GenerateParam();

            const string url = "../SInventory_RPTVIEW/ProductDiscountReportViewer.aspx";
            const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

    }


    private String GenerateParam()
    {
        string param = "";

        if (CompanyDropDownlist.SelectedValue != "")
        {
            param = param + "And Pro.companyId ='" + CompanyDropDownlist.SelectedValue + "' ";
        }


        if (CustomerIdHiddenField.Value != "")
        {
            param = param + " And tblProductDiscount.CustomerMasterId ='" + CustomerIdHiddenField.Value + "' ";
        }

        return param;
    }


    protected void clearButton_Click(object sender, EventArgs e)
    {

        custCodeTextBox.Text = "";
        CustomerIdHiddenField.Value = "";
    }




    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        string perticular = custCodeTextBox.Text.Trim();


        if (perticular.Contains(':'))
        {
            string[] emp = perticular.Split(':');
            custCodeTextBox.Text = emp[1];
            CustomerIdHiddenField.Value = emp[3];

        }
        else
        {

            custCodeTextBox.Text = "";
            CustomerIdHiddenField.Value = "";
            
        }
    }
}