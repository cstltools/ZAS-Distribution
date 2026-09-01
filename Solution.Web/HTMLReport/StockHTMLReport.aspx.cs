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

public partial class HTMLReport_ProformaHTMLReport : System.Web.UI.Page
{
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    DCStockReportBLL aDcStockReportBll = new DCStockReportBLL();
    ReportDocument rptdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        string ComUnitId = Request.QueryString["comUnitId"];
        int i = Convert.ToInt32(Session["DC"]);
        int j = Convert.ToInt32(Session["SDC"]);
        if (i == 1)
        {
            
            DataTable comUnitDetailDataTable = new DataTable();

            if (ComUnitId != "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL((ComUnitId)).Copy();
            }
            if (ComUnitId == "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL("").Copy();
            }
            if (comUnitDetailDataTable.Rows.Count > 0)
            {
                DataTable comUnitMainDataTable =
                    aDcStockReportBll.DCReportMainDataDAL(ComUnitId).Copy();

                DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();

                loadGridView.DataSource = comUnitDetailDataTable;
                loadGridView.DataBind();
            }
            else
            {
                
            }
        }
        if (i == 0 && j==1)
        {
            DataTable comUnitDetailDataTable = new DataTable();

            if (ComUnitId == "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL("").Copy();
            }
            if (comUnitDetailDataTable.Rows.Count > 0)
            {

                DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();

                GridView1.DataSource = comUnitDetailDataTable;
                GridView1.DataBind();

            }
            else
            {
                
            }
        }
        if (i == 0 && j==0)
        {
            DataTable comUnitDetailDataTable = new DataTable();
            if (ComUnitId == "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL("").Copy();
            }
            if (comUnitDetailDataTable.Rows.Count > 0)
            {

                DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();

                GridView2.DataSource = comUnitDetailDataTable;
                GridView2.DataBind();

            }

        }
    }
    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        int i = Convert.ToInt32(Session["DC"]);
        int j = Convert.ToInt32(Session["SDC"]);
        string ComUnitId = Request.QueryString["comUnitId"];
        
        if (i == 1)
        {
            
            DataTable comUnitDetailDataTable = new DataTable();
            if (ComUnitId != "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL((ComUnitId)).Copy();
            }
            if (ComUnitId == "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL("").Copy();
            }
            if (comUnitDetailDataTable.Rows.Count > 0)
            {
                DataTable comUnitMainDataTable =
                    aDcStockReportBll.DCReportMainDataDAL(ComUnitId).Copy();

                DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();

                DataSet Ds = new DataSet();

                comUnitMainDataTable.TableName = "comUnitMainDataTable";
                comUnitDetailDataTable.TableName = "comUnitDetailDataTable";
                companyInfoDataTable.TableName = "companyInfoDataTable";

                Ds.Tables.Add(comUnitMainDataTable);
                Ds.Tables.Add(comUnitDetailDataTable);
                Ds.Tables.Add(companyInfoDataTable);
               
                rptdoc.Load(ReportPath("rptDCStock.rpt"));
                rptdoc.SetDataSource(Ds);

                rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
                "Stock_Report");
            }
            else
            {
                
            }
        }
        if (i == 0 && j == 1)
        {
            DataTable comUnitDetailDataTable = new DataTable();
            if (ComUnitId == "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL("").Copy();
            }
            if (comUnitDetailDataTable.Rows.Count > 0)
            {

                DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();

                DataSet Ds = new DataSet();

                comUnitDetailDataTable.TableName = "comUnitDetailDataTable";
                companyInfoDataTable.TableName = "companyInfoDataTable";

                Ds.Tables.Add(comUnitDetailDataTable);
                Ds.Tables.Add(companyInfoDataTable);
                
                //rptDCStock aRptDcStock = new rptDCStock();

                //aRptDcStock.SetDataSource(Ds);
                //crvDCStockRpt.ReportSource = aRptDcStock;


                rptdoc.Load(ReportPath("rptWHDCStock.rpt"));
                rptdoc.SetDataSource(Ds);


                //crvDCStockRpt.ReportSource = rptdoc;
                //crvDCStockRpt.DataBind();
                rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
               "WHStock_Report");

            }
            else
            {
                
            }
        }
        if (i == 0 && j == 0)
        {
            DataTable comUnitDetailDataTable = new DataTable();
            if (ComUnitId == "")
            {
                comUnitDetailDataTable = aDcStockReportBll.DCReportDetailDataDAL("").Copy();
            }
            if (comUnitDetailDataTable.Rows.Count > 0)
            {

                DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();

                DataSet Ds = new DataSet();

                comUnitDetailDataTable.TableName = "comUnitDetailDataTable";
                companyInfoDataTable.TableName = "companyInfoDataTable";

                Ds.Tables.Add(comUnitDetailDataTable);
                Ds.Tables.Add(companyInfoDataTable);
                //rptDCStock aRptDcStock = new rptDCStock();

                //aRptDcStock.SetDataSource(Ds);
                //crvDCStockRpt.ReportSource = aRptDcStock;


                rptdoc.Load(ReportPath("rptWHDCStockSum.rpt"));
                rptdoc.SetDataSource(Ds);


                //crvDCStockRpt.ReportSource = rptdoc;
                //crvDCStockRpt.DataBind();
                rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
               "WHStock_ReportSum");

            }
            else
            {

            }
        }
    }
}