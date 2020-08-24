using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HTMLReport_InvoicePrintPreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            printjob();
        }
    }

    public void printjob()
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "printpr()", true);
    }
}