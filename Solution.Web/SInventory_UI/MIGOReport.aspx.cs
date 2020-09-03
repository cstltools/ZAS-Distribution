using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_SpecialDiscountReport : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
        
    }
    
    protected void viewRptButton_Click(object sender, EventArgs e)
    {
        if (fromDateTextBox.Text!="")
        {
            if (toDateTextBox.Text == "")
            {
                toDateTextBox.Text = fromDateTextBox.Text;
            }

                string fromDate = fromDateTextBox.Text;
                string toDate = toDateTextBox.Text;


                string url = "../SInventory_RPTVIEW/MIGOReportViewer.aspx?fromDate=" + fromDate + "&toDate=" + toDate + "&rpttype=" + rptTypeDropDownList.SelectedValue;
                // string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=900,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
                string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
           
        }
        else
        {
            ShowMessageBox("Select From Date");
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}