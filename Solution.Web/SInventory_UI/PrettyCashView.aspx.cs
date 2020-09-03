using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.PettyCash_BLL;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_PrettyCashView : System.Web.UI.Page
{

    

   // OrderListBLL aOrderListBll = new OrderListBLL();
    PettyCashBLL aCashBll = new PettyCashBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Gridview();
        }
    }


    protected void proCetegoryAddImageButton_Click(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, this.GetType(),
            "alert",
            "window.location ='PrettyCashEntry.aspx';",
            true);
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private void PopUp(string Id)
    {
        string url = "ManualOrderEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }



    private String GetPrameterList()
    {
        string param = "";


        if (Session["PettyCashmasterId"] != null)
        {
            param = param + "And Mas.PettyCashmasterId='" + Session["PettyCashmasterId"] + "' ";
        }


        if (Session["ActiveStatus"] != null)
        {
            param = param + " And Mas.ActiveStatus='" + Session["ActiveStatus"] + "' ";
        }


        return param;

    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {


        //if (e.CommandName == "EditData")
        //{
        //    int rowindex = Convert.ToInt32(e.CommandArgument);
        //    string divisionId = e.CommandArgument.ToString();

        //    {
        //        Response.Redirect("PrettyCashEntry.aspx?MasID=" + divisionId);
        //    }
        //}



        if (e.CommandName == "ViewData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);

            string id = loadGridView.DataKeys[rowindex][0].ToString();
            Session["PettyCashmasterId"] = id;

            string status = loadGridView.DataKeys[rowindex][1].ToString();
            Session["ActiveStatus"] = status;

            if (status == "Accepted")
            {
                Session["PRAM"] = "";
                Session["PRAM"] = GetPrameterList();

                const string url = "../SInventory_RPTVIEW/PettyCashReportViewer.aspx";
                const string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            else
            {
                showMessageBox("Can Not  Print Data!....");
            }


        }





        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);

            string id = loadGridView.DataKeys[rowindex][0].ToString();
            Session["PettyCashmasterId"] = id;

            string status = loadGridView.DataKeys[rowindex][1].ToString();
            Session["ActiveStatus"] = status;
            if (status == "Posted")
            {
                Response.Redirect("PrettyCashEntry.aspx?MasId=" + id);
            }
            else
            {
                showMessageBox("Can Not  Edit Data!....");
            }
            
        }

        
         

        if (e.CommandName == "DeleteData")
        {

            int rowindex = Convert.ToInt32(e.CommandArgument);

            string id = loadGridView.DataKeys[rowindex][0].ToString();
            Session["PettyCashmasterId"] = id;

            string status = loadGridView.DataKeys[rowindex][1].ToString();
            Session["ActiveStatus"] = status;
            
         
            if (status == "Posted")
            {
                if (aCashBll.DetailsDeleteDataBll(id))
                {
                    if (aCashBll.MAsterDeleteDataBll(id))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                            "alert",
                            "alert('Delete Successfully Done...');window.location ='PrettyCashView.aspx';",
                            true);
                       
                    }

                }
            }
            else
            {
                showMessageBox("Can Not  Delete Data!....");
            }
         



        }


    }

    public void Gridview()
    {
        DataTable dt = aCashBll.GetAllAvailableOrder();
        loadGridView.DataSource = dt;
        loadGridView.DataBind();
    }


    protected void itemdeleteImageButton_Click(object sender, EventArgs e)
    {

    }

    protected void itemViewReportImageButton_Click(object sender, EventArgs e)
    {

    }


    protected void miaTargetNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("PrettyCashEntry.aspx");
    }

   
}