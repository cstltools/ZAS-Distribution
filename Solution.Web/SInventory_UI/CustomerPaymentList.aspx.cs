using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerPaymentList : System.Web.UI.Page
{

    DataTable aDataTable = new DataTable();


    CustPaymentDAL aCustPaymentDal = new CustPaymentDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {
       // aCustomerMasterBLL.LoadRegionDropdownList(regionDropDownList);
    }

    private void CustomerMasterLoad()
    {
        string parameter = "";

        parameter = GenerateParameter();

        if (parameter != "")
        {
            aDataTable = aCustPaymentDal.LoadCoustomerPaymentMaster(parameter);
            loadGridView.DataSource = aDataTable;
            loadGridView.DataBind();
        }
    }

    private string GenerateParameter()
    {

        string param = "";

        if (paymentFromdate.Text.Trim() != "" && paymentTodate.Text.Trim() !="")
        {
            param = param  + " And PaymentDate Between '"+paymentFromdate.Text+"' And '"+paymentTodate.Text+"'";
        }

        if (paymentFromdate.Text.Trim() != "" && paymentTodate.Text.Trim() =="" )
        {
            param = param + " And PaymentDate Between '" + paymentFromdate.Text + "' And '" + DateTime.Now + "'";
        }

        if (paymentFromdate.Text.Trim() == "" && paymentTodate.Text.Trim() != "")
        {
            param = param + " And PaymentDate Between '" + DateTime.Now + "' And '" + paymentTodate.Text + "'";
        }

        return param;
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        if (e.CommandName == "viewData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string customermasterid = loadGridView.DataKeys[rowindex][0].ToString();

            Session["jkdj"] = "";
            Session["jkdj"] = customermasterid;
            PopUp(customermasterid);
        }
    }

    private void PopUp(string Id)
    {
        string url = "../SInventory_RPTVIEW/CusTPaymentReportViewer.aspx";
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void CustMasterNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CustomerPaymentProcess.aspx");
    }
    protected void CustMasterReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        CustomerMasterLoad();
    }

    protected void rptImageButton_Click(object sender, ImageClickEventArgs e)
    {
        string url = "../SInventory_RPTVIEW/CustomerMasterViewer.aspx";
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        CustomerMasterLoad();
    }


    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        
        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }
}