using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Library.BLL.SInventory_BLL;
using Library.CrystalReports.SInventory_RPT;

public partial class SInventory_RPTVIEW_SalesHubReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    SalesHubBLL aSalesHubBLL = new SalesHubBLL();

    protected void Page_Init(object sender, EventArgs e)
    {
        string rptType = Request.QueryString["rptType"];
        

        if (rptType != "")
        {
            if (rptType == "REGION")
            {
                var regionDataTable = new DataTable();
                string parameter = Session["PRAM"].ToString();
                regionDataTable = aSalesHubBLL.LoadREGIONHub(parameter).Copy();

                var ds = new DataSet();

                if (regionDataTable.Rows.Count > 0)
                {
                    regionDataTable.TableName = "RegionDataSet";
                    ds.Tables.Add(regionDataTable);

                    rptdoc.Load(ReportPath("rptRegionReport.rpt"));
                    rptdoc.SetDataSource(ds);

                    crvCustMasterRpt.ReportSource = rptdoc;
                    crvCustMasterRpt.DataBind();

                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }


            if (rptType == "AREA")
            {
                var areaDataTable = new DataTable();
                string parameter = Session["PRAM"].ToString();
                areaDataTable = aSalesHubBLL.LoadAreaHub(parameter).Copy();

                var ds = new DataSet();

                if (areaDataTable.Rows.Count > 0)
                {
                    areaDataTable.TableName = "AreaDataSet";
                    ds.Tables.Add(areaDataTable);

                    rptdoc.Load(ReportPath("rptAreaReport.rpt"));
                    rptdoc.SetDataSource(ds);

                    crvCustMasterRpt.ReportSource = rptdoc;
                    crvCustMasterRpt.DataBind();

                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }


            if (rptType == "TERRITORY")
            {
                var territoryDataTable = new DataTable();
                string parameter = Session["PRAM"].ToString();
                territoryDataTable = aSalesHubBLL.LoadTerritoryHub(parameter).Copy();

                var ds = new DataSet();

                if (territoryDataTable.Rows.Count > 0)
                {
                    territoryDataTable.TableName = "TerritoryDataSet";
                    ds.Tables.Add(territoryDataTable);

                    rptdoc.Load(ReportPath("rptTerritoryReport.rpt"));
                    rptdoc.SetDataSource(ds);

                    crvCustMasterRpt.ReportSource = rptdoc;
                    crvCustMasterRpt.DataBind();

                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }

            if (rptType == "MARKET")
            {
                var marketDataTable = new DataTable();
                string parameter = Session["PRAM"].ToString();
                marketDataTable = aSalesHubBLL.LoadMarketHub(parameter).Copy();

                var ds = new DataSet();

                if (marketDataTable.Rows.Count > 0)
                {
                    marketDataTable.TableName = "MarketDataSet";
                    ds.Tables.Add(marketDataTable);

                    rptdoc.Load(ReportPath("rptMarketReport.rpt"));
                    rptdoc.SetDataSource(ds);

                    crvCustMasterRpt.ReportSource = rptdoc;
                    crvCustMasterRpt.DataBind();

                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }
    
        }
    }
    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }
   
    protected void crvCustMasterRpt_Unload(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCustMasterRpt.Dispose();
    }
    protected void crvCustMasterRpt_Disposed(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCustMasterRpt.Dispose();
    }
}

