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

public partial class HTMLReport_DelivaryReturnHTMLReport : System.Web.UI.Page
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

        if (NationalReport != 1)
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable =
                aCompanySalesReportBll.DeliveryReturnReportDAl(districtId, FromDate, ToDate);
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();
        }
        else
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable =
                aCompanySalesReportBll.DeliveryReturnReportDAl(FromDate, ToDate);
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        int NationalReport = Convert.ToInt32(Request.QueryString["NationalReport"]);
        string districtId = Request.QueryString["districtId"];
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];

        if (NationalReport != 1)
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable =
                aCompanySalesReportBll.DeliveryReturnReportDAl(districtId, FromDate, ToDate).Copy();
            DataSet Ds = new DataSet();
            mainDataTable.TableName = "deliveryDataTable";
            Ds.Tables.Add(mainDataTable);
            rptdoc.Load(ReportPath("crpDeliveryReturnReport.rpt"));
            rptdoc.SetDataSource(Ds);
            //crvSalesRpt.ReportSource = rptdoc;
            //crvSalesRpt.DataBind();
            rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
               "DeliveryReturn_Report");
        }
        else
        {
            DateTime FromDate = Convert.ToDateTime(fromDate);
            DateTime ToDate = Convert.ToDateTime(toDate);

            DataTable mainDataTable =
                aCompanySalesReportBll.DeliveryReturnReportDAl(FromDate, ToDate).Copy();
            DataSet Ds = new DataSet();
            mainDataTable.TableName = "deliveryDataTable";
            Ds.Tables.Add(mainDataTable);
            rptdoc.Load(ReportPath("crpDeliveryReturnReport.rpt"));
            rptdoc.SetDataSource(Ds);
            //crvSalesRpt.ReportSource = rptdoc;
            //crvSalesRpt.DataBind();
            rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
               "DeliveryReturn_Report");
        }
    }
    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
}