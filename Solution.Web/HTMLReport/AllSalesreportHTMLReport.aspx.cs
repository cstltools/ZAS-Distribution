using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class HTMLReport_AllSalesreportHTMLReport : System.Web.UI.Page
{
    private CompanySalesReportBLL aCompanySalesReportBll = new CompanySalesReportBLL();

    // Summary Totals
    private decimal totalInvoicedQty = 0;
    private decimal totalInvoiceGross = 0;
    private decimal totalInvoiceTradeDisc = 0;
    private decimal totalInvoiceSpecialDisc = 0;
    private decimal totalInvoiceVAT = 0;
    private decimal totalInvoiceNet = 0;
    private decimal totalInTransitQty = 0;
    private decimal totalDeliveredQty = 0;
    private decimal totalDeliveredGross = 0;
    private decimal totalDeliveryTradeDisc = 0;
    private decimal totalDeliverySpecialDisc = 0;
    private decimal totalDeliveryVAT = 0;
    private decimal totalDeliveredNet = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadReport();
        }
    }

    private void LoadReport()
    {
        string fromDate = Request.QueryString["fromDate"] ?? string.Empty;
        string toDate = Request.QueryString["toDate"] ?? string.Empty;
        string districtId = Request.QueryString["districtId"] ?? string.Empty;
        string companyId = Request.QueryString["companyId"] ?? string.Empty;
        string parameter = Session["Param"] != null ? Session["Param"].ToString() : string.Empty;

        if (string.IsNullOrEmpty(parameter))
        {
            if (!string.IsNullOrEmpty(companyId) && companyId != "0")
            {
                parameter += " AND CU.CompanyId = " + companyId;
            }
            if (!string.IsNullOrEmpty(districtId) && districtId != "0")
            {
                parameter += " AND I.ComUnitId = " + districtId;
            }
        }

        DateTime dtFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Today.AddDays(-30) : Convert.ToDateTime(fromDate);
        DateTime dtTo = string.IsNullOrEmpty(toDate) ? DateTime.Today : Convert.ToDateTime(toDate);

        FromTextBox.Text = dtFrom.ToString("dd-MMM-yyyy");
        toTextBox.Text = dtTo.ToString("dd-MMM-yyyy");

        DataTable dt = aCompanySalesReportBll.SalesReportDAl(parameter, dtFrom, dtTo);

        if (dt != null && dt.Rows.Count > 0)
        {
            lblTotalRecords.Text = dt.Rows.Count.ToString("N0");
            loadGridView.DataSource = dt;
            loadGridView.DataBind();
        }
        else
        {
            lblTotalRecords.Text = "0";
            loadGridView.DataSource = new DataTable();
            loadGridView.DataBind();
        }
    }

    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            totalInvoicedQty += drv["InvoicedQty"] != DBNull.Value ? Convert.ToDecimal(drv["InvoicedQty"]) : 0;
            totalInvoiceGross += drv["InvoiceGrossAmount"] != DBNull.Value ? Convert.ToDecimal(drv["InvoiceGrossAmount"]) : 0;
            totalInvoiceTradeDisc += drv["InvoiceTradeDiscount"] != DBNull.Value ? Convert.ToDecimal(drv["InvoiceTradeDiscount"]) : 0;
            totalInvoiceSpecialDisc += drv["InvoiceSpecialDiscount"] != DBNull.Value ? Convert.ToDecimal(drv["InvoiceSpecialDiscount"]) : 0;
            totalInvoiceVAT += drv["InvoiceVATAmount"] != DBNull.Value ? Convert.ToDecimal(drv["InvoiceVATAmount"]) : 0;
            totalInvoiceNet += drv["InvoiceNetAmount"] != DBNull.Value ? Convert.ToDecimal(drv["InvoiceNetAmount"]) : 0;
            totalInTransitQty += drv["InTransitQty"] != DBNull.Value ? Convert.ToDecimal(drv["InTransitQty"]) : 0;
            totalDeliveredQty += drv["DeliveredQty"] != DBNull.Value ? Convert.ToDecimal(drv["DeliveredQty"]) : 0;
            totalDeliveredGross += drv["DeliveryGrossAmount"] != DBNull.Value ? Convert.ToDecimal(drv["DeliveryGrossAmount"]) : 0;
            totalDeliveryTradeDisc += drv["DeliveryTradeDiscount"] != DBNull.Value ? Convert.ToDecimal(drv["DeliveryTradeDiscount"]) : 0;
            totalDeliverySpecialDisc += drv["DeliverySpecialDiscount"] != DBNull.Value ? Convert.ToDecimal(drv["DeliverySpecialDiscount"]) : 0;
            totalDeliveryVAT += drv["DeliveryVATAmount"] != DBNull.Value ? Convert.ToDecimal(drv["DeliveryVATAmount"]) : 0;
            totalDeliveredNet += drv["DeliveredNetAmount"] != DBNull.Value ? Convert.ToDecimal(drv["DeliveredNetAmount"]) : 0;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[1].Font.Bold = true;
            e.Row.Cells[1].CssClass = "text-left";

            // Invoiced Totals (Columns 25 to 30)
            e.Row.Cells[25].Text = totalInvoicedQty.ToString("N0");
            e.Row.Cells[25].CssClass = "text-right";
            e.Row.Cells[26].Text = totalInvoiceGross.ToString("N2");
            e.Row.Cells[26].CssClass = "text-right";
            e.Row.Cells[27].Text = totalInvoiceTradeDisc.ToString("N2");
            e.Row.Cells[27].CssClass = "text-right";
            e.Row.Cells[28].Text = totalInvoiceSpecialDisc.ToString("N2");
            e.Row.Cells[28].CssClass = "text-right";
            e.Row.Cells[29].Text = totalInvoiceVAT.ToString("N2");
            e.Row.Cells[29].CssClass = "text-right";
            e.Row.Cells[30].Text = totalInvoiceNet.ToString("N2");
            e.Row.Cells[30].CssClass = "text-right";

            // In-Transit Total (Column 33)
            e.Row.Cells[33].Text = totalInTransitQty.ToString("N0");
            e.Row.Cells[33].CssClass = "text-right";

            // Delivered Totals (Columns 36 to 41)
            e.Row.Cells[36].Text = totalDeliveredQty.ToString("N0");
            e.Row.Cells[36].CssClass = "text-right";
            e.Row.Cells[37].Text = totalDeliveredGross.ToString("N2");
            e.Row.Cells[37].CssClass = "text-right";
            e.Row.Cells[38].Text = totalDeliveryTradeDisc.ToString("N2");
            e.Row.Cells[38].CssClass = "text-right";
            e.Row.Cells[39].Text = totalDeliverySpecialDisc.ToString("N2");
            e.Row.Cells[39].CssClass = "text-right";
            e.Row.Cells[40].Text = totalDeliveryVAT.ToString("N2");
            e.Row.Cells[40].CssClass = "text-right";
            e.Row.Cells[41].Text = totalDeliveredNet.ToString("N2");
            e.Row.Cells[41].CssClass = "text-right";

            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Font.Bold = true;
            }
        }
    }

    public string GetStatusClass(object statusObj)
    {
        if (statusObj == null) return "status-transit";
        string status = statusObj.ToString().Trim().ToLowerInvariant();

        switch (status)
        {
            case "full":
            case "delivered":
                return "status-full";
            case "partial":
                return "status-partial";
            case "reject":
                return "status-reject";
            default:
                return "status-transit";
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        string fromDate = Request.QueryString["fromDate"] ?? string.Empty;
        string toDate = Request.QueryString["toDate"] ?? string.Empty;
        string districtId = Request.QueryString["districtId"] ?? string.Empty;
        string companyId = Request.QueryString["companyId"] ?? string.Empty;
        string parameter = Session["Param"] != null ? Session["Param"].ToString() : string.Empty;

        if (string.IsNullOrEmpty(parameter))
        {
            if (!string.IsNullOrEmpty(companyId) && companyId != "0")
            {
                parameter += " AND CU.CompanyId = " + companyId;
            }
            if (!string.IsNullOrEmpty(districtId) && districtId != "0")
            {
                parameter += " AND I.ComUnitId = " + districtId;
            }
        }

        DateTime dtFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Today.AddDays(-30) : Convert.ToDateTime(fromDate);
        DateTime dtTo = string.IsNullOrEmpty(toDate) ? DateTime.Today : Convert.ToDateTime(toDate);

        DataTable dt = aCompanySalesReportBll.SalesReportDAl(parameter, dtFrom, dtTo);

        if (dt == null || dt.Rows.Count == 0)
        {
            return;
        }

        Response.Clear();
        Response.Buffer = true;
        string filename = string.Format("All_Sales_Report_{0}.xls", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.Charset = "UTF-8";
        Response.ContentType = "application/vnd.ms-excel";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\">");
        sb.AppendLine("<head>");
        sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
        sb.AppendLine("<!--[if gte mso 9]>");
        sb.AppendLine("<xml>");
        sb.AppendLine("  <x:ExcelWorkbook>");
        sb.AppendLine("    <x:ExcelWorksheets>");
        sb.AppendLine("      <x:ExcelWorksheet>");
        sb.AppendLine("        <x:Name>All Sales Report</x:Name>");
        sb.AppendLine("        <x:WorksheetOptions>");
        sb.AppendLine("          <x:DisplayGridlines/>");
        sb.AppendLine("        </x:WorksheetOptions>");
        sb.AppendLine("      </x:ExcelWorksheet>");
        sb.AppendLine("    </x:ExcelWorksheets>");
        sb.AppendLine("  </x:ExcelWorkbook>");
        sb.AppendLine("</xml>");
        sb.AppendLine("<![endif]-->");
        sb.AppendLine("<style>");
        sb.AppendLine("  body { font-family: Calibri, Arial, sans-serif; }");
        sb.AppendLine("  table { border-collapse: collapse; }");
        sb.AppendLine("  .th-hdr { background-color: #1e293b; color: #ffffff; font-size: 11pt; font-weight: bold; text-align: center; border: 0.5pt solid #000000; padding: 6px 8px; }");
        sb.AppendLine("  .td-code { mso-number-format: \"\\@\"; font-size: 10pt; text-align: center; border: 0.5pt solid #d4d4d4; padding: 4px 6px; }");
        sb.AppendLine("  .td-text { mso-number-format: \"\\@\"; font-size: 10pt; text-align: left; border: 0.5pt solid #d4d4d4; padding: 4px 6px; }");
        sb.AppendLine("  .td-date { mso-number-format: \"yyyy-mm-dd\"; font-size: 10pt; text-align: center; border: 0.5pt solid #d4d4d4; padding: 4px 6px; }");
        sb.AppendLine("  .td-qty { mso-number-format: \"\\#\\,\\#\\#0\"; font-size: 10pt; text-align: right; border: 0.5pt solid #d4d4d4; padding: 4px 6px; }");
        sb.AppendLine("  .td-amt { mso-number-format: \"\\#\\,\\#\\#0\\.00\"; font-size: 10pt; text-align: right; border: 0.5pt solid #d4d4d4; padding: 4px 6px; }");
        sb.AppendLine("  .ftr-lbl { font-size: 11pt; font-weight: bold; background-color: #e2e8f0; text-align: left; border-top: 1.5pt solid #000000; border-bottom: 1.5pt solid #000000; border-left: 0.5pt solid #d4d4d4; border-right: 0.5pt solid #d4d4d4; padding: 6px; }");
        sb.AppendLine("  .ftr-code { font-size: 11pt; font-weight: bold; background-color: #e2e8f0; text-align: center; border-top: 1.5pt solid #000000; border-bottom: 1.5pt solid #000000; border-left: 0.5pt solid #d4d4d4; border-right: 0.5pt solid #d4d4d4; padding: 6px; }");
        sb.AppendLine("  .ftr-qty { mso-number-format: \"\\#\\,\\#\\#0\"; font-size: 11pt; font-weight: bold; background-color: #e2e8f0; text-align: right; border-top: 1.5pt solid #000000; border-bottom: 1.5pt solid #000000; border-left: 0.5pt solid #d4d4d4; border-right: 0.5pt solid #d4d4d4; padding: 6px; }");
        sb.AppendLine("  .ftr-amt { mso-number-format: \"\\#\\,\\#\\#0\\.00\"; font-size: 11pt; font-weight: bold; background-color: #e2e8f0; text-align: right; border-top: 1.5pt solid #000000; border-bottom: 1.5pt solid #000000; border-left: 0.5pt solid #d4d4d4; border-right: 0.5pt solid #d4d4d4; padding: 6px; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<table>");
        
        // Table Headers
        sb.AppendLine("<thead>");
        sb.AppendLine("<tr>");
        sb.AppendLine("<th class=\"th-hdr\">Depot Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">Depot Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">Order No</th>");
        sb.AppendLine("<th class=\"th-hdr\">Order Date</th>");
        sb.AppendLine("<th class=\"th-hdr\">Customer Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">Customer Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">Customer Type</th>");
        sb.AppendLine("<th class=\"th-hdr\">Market Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">Market Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">Territory Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">Territory Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">Area Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">Area Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">Region Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">Region Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">MIO Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">MIO Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">Product Code</th>");
        sb.AppendLine("<th class=\"th-hdr\">Product Name</th>");
        sb.AppendLine("<th class=\"th-hdr\">Pack Size</th>");
        sb.AppendLine("<th class=\"th-hdr\">Batch No</th>");
        sb.AppendLine("<th class=\"th-hdr\">Exp Date</th>");
        sb.AppendLine("<th class=\"th-hdr\">Unit Price</th>");
        sb.AppendLine("<th class=\"th-hdr\">Invoice No</th>");
        sb.AppendLine("<th class=\"th-hdr\">Invoice Date</th>");
        sb.AppendLine("<th class=\"th-hdr\">Invoiced Qty</th>");
        sb.AppendLine("<th class=\"th-hdr\">Invoice Gross</th>");
        sb.AppendLine("<th class=\"th-hdr\">Inv Trade Disc</th>");
        sb.AppendLine("<th class=\"th-hdr\">Inv Special Disc</th>");
        sb.AppendLine("<th class=\"th-hdr\">Inv VAT</th>");
        sb.AppendLine("<th class=\"th-hdr\">Inv Net Amount</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delivery Status</th>");
        sb.AppendLine("<th class=\"th-hdr\">Transit Aging (Days)</th>");
        sb.AppendLine("<th class=\"th-hdr\">In-Transit Qty</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delivery Inv No</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delivery Conf. Date</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delivered Qty</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delivered Gross</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delv Trade Disc</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delv Special Disc</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delv VAT</th>");
        sb.AppendLine("<th class=\"th-hdr\">Delivered Net Sales</th>");
        sb.AppendLine("</tr>");
        sb.AppendLine("</thead>");

        // Table Body
        sb.AppendLine("<tbody>");
        decimal expTotalInvoicedQty = 0;
        decimal expTotalInvoiceGross = 0;
        decimal expTotalInvoiceTradeDisc = 0;
        decimal expTotalInvoiceSpecialDisc = 0;
        decimal expTotalInvoiceVAT = 0;
        decimal expTotalInvoiceNet = 0;
        decimal expTotalInTransitQty = 0;
        decimal expTotalDeliveredQty = 0;
        decimal expTotalDeliveredGross = 0;
        decimal expTotalDeliveryTradeDisc = 0;
        decimal expTotalDeliverySpecialDisc = 0;
        decimal expTotalDeliveryVAT = 0;
        decimal expTotalDeliveredNet = 0;

        foreach (DataRow r in dt.Rows)
        {
            decimal invQty = r["InvoicedQty"] != DBNull.Value ? Convert.ToDecimal(r["InvoicedQty"]) : 0;
            decimal invGross = r["InvoiceGrossAmount"] != DBNull.Value ? Convert.ToDecimal(r["InvoiceGrossAmount"]) : 0;
            decimal invTradeDisc = r["InvoiceTradeDiscount"] != DBNull.Value ? Convert.ToDecimal(r["InvoiceTradeDiscount"]) : 0;
            decimal invSpecDisc = r["InvoiceSpecialDiscount"] != DBNull.Value ? Convert.ToDecimal(r["InvoiceSpecialDiscount"]) : 0;
            decimal invVat = r["InvoiceVATAmount"] != DBNull.Value ? Convert.ToDecimal(r["InvoiceVATAmount"]) : 0;
            decimal invNet = r["InvoiceNetAmount"] != DBNull.Value ? Convert.ToDecimal(r["InvoiceNetAmount"]) : 0;
            decimal transitQty = r["InTransitQty"] != DBNull.Value ? Convert.ToDecimal(r["InTransitQty"]) : 0;
            decimal delvQty = r["DeliveredQty"] != DBNull.Value ? Convert.ToDecimal(r["DeliveredQty"]) : 0;
            decimal delvGross = r["DeliveryGrossAmount"] != DBNull.Value ? Convert.ToDecimal(r["DeliveryGrossAmount"]) : 0;
            decimal delvTradeDisc = r["DeliveryTradeDiscount"] != DBNull.Value ? Convert.ToDecimal(r["DeliveryTradeDiscount"]) : 0;
            decimal delvSpecDisc = r["DeliverySpecialDiscount"] != DBNull.Value ? Convert.ToDecimal(r["DeliverySpecialDiscount"]) : 0;
            decimal delvVat = r["DeliveryVATAmount"] != DBNull.Value ? Convert.ToDecimal(r["DeliveryVATAmount"]) : 0;
            decimal delvNet = r["DeliveredNetAmount"] != DBNull.Value ? Convert.ToDecimal(r["DeliveredNetAmount"]) : 0;
            decimal unitPrice = r["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(r["UnitPrice"]) : 0;
            int aging = r["TransitAgingDays"] != DBNull.Value ? Convert.ToInt32(r["TransitAgingDays"]) : 0;

            expTotalInvoicedQty += invQty;
            expTotalInvoiceGross += invGross;
            expTotalInvoiceTradeDisc += invTradeDisc;
            expTotalInvoiceSpecialDisc += invSpecDisc;
            expTotalInvoiceVAT += invVat;
            expTotalInvoiceNet += invNet;
            expTotalInTransitQty += transitQty;
            expTotalDeliveredQty += delvQty;
            expTotalDeliveredGross += delvGross;
            expTotalDeliveryTradeDisc += delvTradeDisc;
            expTotalDeliverySpecialDisc += delvSpecDisc;
            expTotalDeliveryVAT += delvVat;
            expTotalDeliveredNet += delvNet;

            sb.AppendLine("<tr>");
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["DepotCode"] != DBNull.Value ? r["DepotCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["DepotName"] != DBNull.Value ? r["DepotName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["OrderNo"] != DBNull.Value ? r["OrderNo"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-date\">{0}</td>", HttpUtility.HtmlEncode(r["OrderDate"] != DBNull.Value ? r["OrderDate"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["CustomerCode"] != DBNull.Value ? r["CustomerCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["CustomerName"] != DBNull.Value ? r["CustomerName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["CustomerType"] != DBNull.Value ? r["CustomerType"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["MarketCode"] != DBNull.Value ? r["MarketCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["MarketName"] != DBNull.Value ? r["MarketName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["TerritoryCode"] != DBNull.Value ? r["TerritoryCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["TerritoryName"] != DBNull.Value ? r["TerritoryName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["AreaCode"] != DBNull.Value ? r["AreaCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["AreaName"] != DBNull.Value ? r["AreaName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["RegionCode"] != DBNull.Value ? r["RegionCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["RegionName"] != DBNull.Value ? r["RegionName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["MiaCode"] != DBNull.Value ? r["MiaCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["MiaName"] != DBNull.Value ? r["MiaName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["ProductCode"] != DBNull.Value ? r["ProductCode"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-text\">{0}</td>", HttpUtility.HtmlEncode(r["ProductName"] != DBNull.Value ? r["ProductName"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["PackSize"] != DBNull.Value ? r["PackSize"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["BatchNo"] != DBNull.Value ? r["BatchNo"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-date\">{0}</td>", HttpUtility.HtmlEncode(r["ExpDate"] != DBNull.Value ? r["ExpDate"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", unitPrice.ToString("F2"));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["InvoiceNo"] != DBNull.Value ? r["InvoiceNo"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-date\">{0}</td>", HttpUtility.HtmlEncode(r["InvoiceDate"] != DBNull.Value ? r["InvoiceDate"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-qty\">{0}</td>", invQty.ToString("F0"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", invGross.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", invTradeDisc.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", invSpecDisc.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", invVat.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", invNet.ToString("F2"));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["DeliveryStatus"] != DBNull.Value ? r["DeliveryStatus"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-qty\">{0}</td>", aging);
            sb.AppendFormat("<td class=\"td-qty\">{0}</td>", transitQty.ToString("F0"));
            sb.AppendFormat("<td class=\"td-code\">{0}</td>", HttpUtility.HtmlEncode(r["DeliveryInvoiceNo"] != DBNull.Value ? r["DeliveryInvoiceNo"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-date\">{0}</td>", HttpUtility.HtmlEncode(r["DeliveryConfirmationDate"] != DBNull.Value ? r["DeliveryConfirmationDate"].ToString() : ""));
            sb.AppendFormat("<td class=\"td-qty\">{0}</td>", delvQty.ToString("F0"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", delvGross.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", delvTradeDisc.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", delvSpecDisc.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", delvVat.ToString("F2"));
            sb.AppendFormat("<td class=\"td-amt\">{0}</td>", delvNet.ToString("F2"));
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</tbody>");

        // Table Footer
        sb.AppendLine("<tfoot>");
        sb.AppendLine("<tr>");
        sb.AppendLine("<td class=\"ftr-code\"></td>");
        sb.AppendLine("<td class=\"ftr-lbl\">Total</td>");
        for (int i = 2; i <= 24; i++)
        {
            sb.AppendLine("<td class=\"ftr-code\"></td>");
        }
        sb.AppendFormat("<td class=\"ftr-qty\">{0}</td>", expTotalInvoicedQty.ToString("F0"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalInvoiceGross.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalInvoiceTradeDisc.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalInvoiceSpecialDisc.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalInvoiceVAT.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalInvoiceNet.ToString("F2"));
        sb.AppendLine("<td class=\"ftr-code\"></td>"); // Delivery Status
        sb.AppendLine("<td class=\"ftr-code\"></td>"); // Aging
        sb.AppendFormat("<td class=\"ftr-qty\">{0}</td>", expTotalInTransitQty.ToString("F0"));
        sb.AppendLine("<td class=\"ftr-code\"></td>"); // Delivery Invoice No
        sb.AppendLine("<td class=\"ftr-code\"></td>"); // Delivery Conf Date
        sb.AppendFormat("<td class=\"ftr-qty\">{0}</td>", expTotalDeliveredQty.ToString("F0"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalDeliveredGross.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalDeliveryTradeDisc.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalDeliverySpecialDisc.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalDeliveryVAT.ToString("F2"));
        sb.AppendFormat("<td class=\"ftr-amt\">{0}</td>", expTotalDeliveredNet.ToString("F2"));
        sb.AppendLine("</tr>");
        sb.AppendLine("</tfoot>");

        sb.AppendLine("</table>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        Response.Output.Write(sb.ToString());
        Response.Flush();
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Required for GridView export to Excel
    }
}