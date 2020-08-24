using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_SalesHubReport2 : System.Web.UI.Page
{
    SalesHubBLL aSalesHubBLL = new SalesHubBLL();
    DeliveryInvoiceReportDal aDal = new DeliveryInvoiceReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownlist();
        }
    }
    private void DropDownlist()
    {
        aDal.LoadManufac(manufacDropDownList);
    }

    protected void manufacDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        salesCenterDropDownList.Items.Clear();
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        

        if (manufacDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aDal.LoadDCByUser(salesCenterDropDownList, manufacDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aDal.LoadDC(salesCenterDropDownList, manufacDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }
        else
        {
            salesCenterDropDownList.Items.Clear();
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
           
        }
    }

    protected void salesCenterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        

        if (salesCenterDropDownList.SelectedValue != "")
        {
            aDal.AreaLoad(areaDropDownList, salesCenterDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
            
        }
    }

    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
       

        if (areaDropDownList.SelectedValue != "")
        {
            aDal.TerritoryLoad(territoryDropDownList, areaDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
            marketDropDownList.Items.Clear();
            
        }
    }

    protected void territoryDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        marketDropDownList.Items.Clear();
        

        if (territoryDropDownList.SelectedValue != "")
        {
            aDal.MarketLoad(marketDropDownList, territoryDropDownList.SelectedValue);
        }
        else
        {
            marketDropDownList.Items.Clear();
            
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private string GenerateParameter()
    {
        string parameter = "";

        if (reportNameDropDownList.SelectedValue != "")
        {
            if (reportNameDropDownList.SelectedValue == "REGION")
            {
                parameter = "";
            }

            if (reportNameDropDownList.SelectedValue == "AREA")
            {
                if (salesCenterDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " WHERE DCA.DCId = '" + salesCenterDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }

            }

            if (reportNameDropDownList.SelectedValue == "TERRITORY")
            {

                parameter = " WHERE TER.IsActive = 1 ";

                if (areaDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " AND ARE.AreaId = '" + areaDropDownList.SelectedValue + "'";
                }
                else
                {
                    parameter = "";
                }
            }

            if (reportNameDropDownList.SelectedValue == "MARKET")
            {

                parameter = " WHERE MAR.IsActive = 1 ";

                if (areaDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " AND ARE.AreaId = '" + areaDropDownList.SelectedValue + "'";
                }
               

                if (territoryDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " AND TER.TerritoryId = '" + territoryDropDownList.SelectedValue + "'";
                }

                if (marketDropDownList.SelectedValue != "")
                {
                    parameter = parameter + " AND MAR.MarketId = '" + marketDropDownList.SelectedValue + "'";
                }
               

            }

            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    parameter = parameter + " AND DCA.DCId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + Session["UserId"].ToString() + ")";
                }
            }
        }

        return parameter;
    }

    private void PopupReport()
    {

        Session["PRAM"] = "";
        Session["PRAM"] = GenerateParameter();

        string rptType = reportNameDropDownList.SelectedValue;

        string url = "../SInventory_RPTVIEW/SalesHubReportViewer.aspx?rptType=" + rptType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    protected void reportButton_Click(object sender, EventArgs e)
    {
        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserType"].ToString() != "Admin")
            {
                if (reportNameDropDownList.SelectedValue != "")
                {
                    if (manufacDropDownList.SelectedValue != "")
                    {
                        if (salesCenterDropDownList.SelectedValue != "")
                        {
                            PopupReport();
                        }
                        else
                        {
                            ShowMessageBox("Please Select DC name !!!");
                        }
                    }
                    else
                    {
                        ShowMessageBox("Please Select a company !!!");
                    }
                }
                else
                {
                    ShowMessageBox("Please Select Report name !!!");
                }

            }
            else
            {
                PopupReport();
            }
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {


        manufacDropDownList.SelectedValue = "";

        salesCenterDropDownList.Items.Clear();
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        
    }
}