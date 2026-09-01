using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SInventory_UI_UserPasswordChangeReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            UserCredentialChangeReport();
        }
    }

    private void UserCredentialChangeReport()
    {
        Session["FDate"] = "";
        Session["TDate"] = "";
        Session["UserId"] = "";

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            Session["FDate"] = fromDateTextBox.Text.Trim();
            Session["TDate"] = toDateTextBox.Text.Trim();
        }

        //Session["PRAM"] = GetPrameterList();

        string url = "../SInventory_RPTVIEW/UserCredentialsReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    private bool Validation()
    {

        if (fromDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please Select To date !!");
            return false;
        }



        return true;
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}