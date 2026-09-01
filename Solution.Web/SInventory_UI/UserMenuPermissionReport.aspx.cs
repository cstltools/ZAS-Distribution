using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_InTransitReport : System.Web.UI.Page
{
    UserMenuPermissionDal aPermissionDal = new UserMenuPermissionDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
        }
    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    public void LoadDropDown()
    {
       aPermissionDal.LoadUser(ddlUser);
    }
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        Session["Pram"] = "";
        Session["Pram"] = GenerateParameter();

        string url = "../SInventory_RPTVIEW/UserPermissionReportViewer.aspx";
        // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
       
    }

    private string GenerateParameter()
    {
        string pram = "";

        if (ddlUser.SelectedValue != "")
        {
            pram = pram + " AND MD.UserId = " + ddlUser.SelectedValue ;
        }

        return pram;
    }

    protected void dcDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
   
}