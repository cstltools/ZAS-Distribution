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

public partial class HTMLReport_AllSalesreportHTMLReport : System.Web.UI.Page
{
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    CompanySalesReportBLL aCompanySalesReportBll = new CompanySalesReportBLL();
    ReportDocument rptdoc = new ReportDocument();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        int NationalReport = Convert.ToInt32(Request.QueryString["NationalReport"]);
        string districtId = Request.QueryString["districtId"];
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];
        string parameter = Session["Param"].ToString();
        
        if (NationalReport != 1)
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aCompanySalesReportBll.SalesReportDAl(parameter, FromDate, ToDate);
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();

        }
        else
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aCompanySalesReportBll.SalesReportDAl(FromDate, ToDate);

            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();


            //rptdoc.Load(ReportPath("rptInvoiceForCustomer.rpt"));
            //rptdoc.SetDataSource(Ds);

            //rptdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true,
            //    "ProformaInvoice-" + invNo);
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        int NationalReport = Convert.ToInt32(Request.QueryString["NationalReport"]);
        string districtId = Request.QueryString["districtId"];
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];
        string parameter = Session["Param"].ToString();

        if (NationalReport != 1)
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aCompanySalesReportBll.SalesReportDAl(parameter, FromDate, ToDate).Copy();

            DataSet Ds = new DataSet();

            mainDataTable.TableName = "AllsalesDataTable";
            Ds.Tables.Add(mainDataTable);
            rptdoc.Load(ReportPath("crpAllSalesReport.rpt"));
            rptdoc.SetDataSource(Ds);
            rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
               "Sales_Report");
        }
        else
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable = aCompanySalesReportBll.SalesReportDAl(FromDate, ToDate).Copy();

            DataSet Ds = new DataSet();

            mainDataTable.TableName = "AllsalesDataTable";
            Ds.Tables.Add(mainDataTable);
            rptdoc.Load(ReportPath("crpAllSalesReport.rpt"));
            rptdoc.SetDataSource(Ds);

            //crvSalesRpt.ReportSource = rptdoc;
            //crvSalesRpt.DataBind();
            rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
               "Sales_Report");


            //rptdoc.Load(ReportPath("rptInvoiceForCustomer.rpt"));
            //rptdoc.SetDataSource(Ds);

            //rptdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true,
            //    "ProformaInvoice-" + invNo);
        }
    }
    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
}