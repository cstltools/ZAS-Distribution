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
    ReportDocument rptdoc = new ReportDocument();
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        int National = Convert.ToInt32(Request.QueryString["NationalReport"]);
        string districtId = Request.QueryString["districtId"];
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];
        if (National != 1)
        {

            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aInvoiceBll.ProformaReportBLL(districtId, FromDate, ToDate);
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();
        }
        if (National == 1)
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aInvoiceBll.ProformaReportBLL(FromDate, ToDate);
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();

        }
    }
    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        int National = Convert.ToInt32(Request.QueryString["NationalReport"]);
        string districtId = Request.QueryString["districtId"];
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];
        if (National != 1)
        {

            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aInvoiceBll.ProformaReportBLL(districtId, FromDate, ToDate).Copy();
            DataSet Ds = new DataSet();

            mainDataTable.TableName = "proformaDataTable";
            Ds.Tables.Add(mainDataTable);
            rptdoc.Load(ReportPath("rptProformaReport.rpt"));
            rptdoc.SetDataSource(Ds);
            //crvInvoiceReport.ReportSource = rptdoc;
            //crvInvoiceReport.DataBind();
            rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
              "Proforma_Report");
        }
        if (National == 1)
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aInvoiceBll.ProformaReportBLL(FromDate, ToDate).Copy();
            DataSet Ds = new DataSet();

            mainDataTable.TableName = "proformaDataTable";
            Ds.Tables.Add(mainDataTable);
            rptdoc.Load(ReportPath("rptProformaReport.rpt"));
            rptdoc.SetDataSource(Ds);
            //crvInvoiceReport.ReportSource = rptdoc;
            //crvInvoiceReport.DataBind();
            rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
              "Proforma_Report");
        }
    }
}