using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_RPTVIEW_GroupInvoiceReport : System.Web.UI.Page
{
    InvoiceBLL aInvoiceBll = new InvoiceBLL();
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();

    DataTable lineData;
    string groupInvoiceNo;
    List<InvoiceSection> sections;

    // One "Reference No" section = one constituent invoice (tblInvoice.InvoiceNo) inside the Group
    // Invoice, exactly as the original Crystal report lays it out: its own header block (only rendered
    // when it actually differs from the previous section's — the reference PDF for INV-GP01-2609020006
    // shows two different customers/orders under the same GroupInvoiceNo, each getting its own "Sold To"
    // block), its own product table, and its own Sub-Total.
    private class InvoiceSection
    {
        public string ReferenceNo;
        public string CustomerCode, CustomerName, Address, TermOfPayment, MarketName,
            DeliveryPersonName, DeliveryPersonPhNo, PONumber, OrderNo, OrderDate, PreparedBy, Remarks;
        public List<DataRow> Lines = new List<DataRow>();
    }

    // Fixed column widths shared by every product table and the Grand Total table on the page, so they
    // all line up visually even though they're rendered as separate <table> elements per section.
    static readonly string[] ColWidths = { "8%", "24%", "6%", "6%", "6%", "6%", "8%", "5%", "8%", "5%", "8%", "10%" };

    protected void Page_Load(object sender, EventArgs e)
    {
        groupInvoiceNo = Request.QueryString["groupInvoiceNo"];

        if (string.IsNullOrWhiteSpace(groupInvoiceNo) || !Regex.IsMatch(groupInvoiceNo, @"^[A-Za-z0-9\-\/]+$"))
        {
            ShowError("No invoice was selected, or the invoice reference is invalid.");
            return;
        }

        lineData = aInvoiceBll.GroupInvoiceHtmlReportDataBLL(groupInvoiceNo);

        if (lineData == null || lineData.Rows.Count == 0)
        {
            ShowError("The requested group invoice could not be found.");
            return;
        }

        sections = BuildSections();

        if (Request.QueryString["format"] == "excel")
        {
            ExportExcel();
            return;
        }

        BindReport();

        if (!IsPostBack)
        {
            pdfFileNameHidden.Value = "GroupInvoice_" + groupInvoiceNo.Replace("/", "-") + ".pdf";
        }
    }

    private void ShowError(string message)
    {
        reportPanel.Visible = false;
        errorLabel.Visible = true;
        errorLabel.Text = message;
    }

    // lineData is already ORDER BY InvoiceNo, ProductCode (see GroupInvoiceHtmlReportData), which for
    // this data happens to reproduce the exact constituent-invoice order Crystal printed them in
    // (verified against the reference PDF: BD159, BD17, BD201, BD76 — a plain string sort). Rows are
    // grouped into contiguous per-InvoiceNo sections here without re-sorting, preserving that order.
    private List<InvoiceSection> BuildSections()
    {
        var result = new List<InvoiceSection>();
        InvoiceSection current = null;

        foreach (DataRow row in lineData.Rows)
        {
            string refNo = Convert.ToString(row["InvoiceNo"]);
            if (current == null || current.ReferenceNo != refNo)
            {
                current = new InvoiceSection
                {
                    ReferenceNo = refNo,
                    CustomerCode = Convert.ToString(row["CustomerCode"]),
                    CustomerName = Convert.ToString(row["CustomerName"]),
                    Address = (Convert.ToString(row["Address"]) + " " + Convert.ToString(row["Addrees2"])).Trim(),
                    TermOfPayment = Convert.ToString(row["TermOfPayment"]),
                    MarketName = Convert.ToString(row["MarketName"]),
                    DeliveryPersonName = Convert.ToString(row["DeliveryPersonName"]),
                    DeliveryPersonPhNo = Convert.ToString(row["DeliveryPersonPhNo"]),
                    PONumber = Convert.ToString(row["PONumber"]),
                    OrderNo = Convert.ToString(row["OrderNo"]),
                    OrderDate = FormatDate(row["OrderDate"]),
                    PreparedBy = Convert.ToString(row["PreparedBy"]),
                    Remarks = Convert.ToString(row["Remarks"]),
                };
                result.Add(current);
            }
            current.Lines.Add(row);
        }
        return result;
    }

    private void BindReport()
    {
        string groupInvoiceNoDisplay = Convert.ToString(lineData.Rows[0]["GroupInvoiceNo"]);
        string invoiceDateDisplay = FormatDate(lineData.Rows[0]["InvoiceDate"]);

        var sb = new StringBuilder();

        // Sold To / Payment / Order header prints once for the whole Group Invoice, not per
        // constituent invoice section — matches ExportExcel's layout below.
        sb.Append(RenderHeaderBlock(sections[0], groupInvoiceNoDisplay, invoiceDateDisplay));

        foreach (var section in sections)
        {
            sb.Append(RenderInvoiceSection(section));
        }

        decimal grandTotalPrice = SumColumn("TotalPrice");
        decimal grandVat = SumColumn("TotalPriceVatAmount");
        decimal grandDiscount = SumColumn("DiscountAmount");
        decimal grandNet = SumColumn("NetAmount");

        sb.Append(RenderGrandTotalTable(grandTotalPrice, grandVat, grandDiscount, grandNet));

        // The original PDF prints "In Words" and "Remarks" once, near the bottom, after the Grand
        // Total. Remarks is a per-invoice column in the source data; the last constituent invoice's
        // value is used since that is the one physically adjacent to these lines in the original layout.
        string remarksValue = sections[sections.Count - 1].Remarks;
        string preparedByValue = sections[sections.Count - 1].PreparedBy;

        sb.Append("<div class='below-total'>");
        sb.Append("<div class='in-words'><b>In Words:</b> " + Encode(BuildAmountInWords(grandNet)) + "</div>");
        sb.Append("<div class='remarks'><b>Remarks:</b> " + Encode(remarksValue) + "</div>");
        sb.Append("</div>");

        sb.Append(RenderSignatureBlock(preparedByValue));

        reportBodyLiteral.Text = sb.ToString();
    }

    private string RenderHeaderBlock(InvoiceSection s, string groupInvoiceNoDisplay, string invoiceDateDisplay)
    {
        var sb = new StringBuilder();
        sb.Append("<div class='invoice-title-wrap'><span class='invoice-title'>Invoice</span></div>");
        sb.Append("<div class='sold-to-block'>");
        sb.Append("<div class='sold-to-label'>Sold To</div>");
        sb.Append("<div class='header-grid'>");

        sb.Append("<div class='col-soldto'>");
        sb.Append("<div>" + Encode(s.CustomerCode) + "</div>");
        sb.Append("<div><b>" + Encode(s.CustomerName) + "</b></div>");
        sb.Append("<div>" + Encode(s.Address) + "</div>");
        sb.Append("</div>");

        sb.Append("<div class='col-mid'>");
        sb.Append("<div><label>Payment Type:</label> " + Encode(s.TermOfPayment) + "</div>");
        sb.Append("<div><label>Market Name:</label> " + Encode(s.MarketName) + "</div>");
        sb.Append("<div><label>Delivery Person:</label> " + Encode(s.DeliveryPersonName) + "</div>");
        sb.Append("<div><label>Mobile No:</label> " + Encode(s.DeliveryPersonPhNo) + "</div>");
        sb.Append("</div>");

        sb.Append("<div class='col-right'>");
        sb.Append("<div><label>Invoice No:</label> " + Encode(groupInvoiceNoDisplay) + "</div>");
        sb.Append("<div><label>Invoice Date:</label> " + Encode(invoiceDateDisplay) + "</div>");
        sb.Append("<div><label>Order No:</label> " + Encode(s.OrderNo) + "</div>");
        sb.Append("<div><label>Order Date:</label> " + Encode(s.OrderDate) + "</div>");
        sb.Append("<div><label>PO Number:</label> " + Encode(s.PONumber) + "</div>");
        sb.Append("</div>");

        sb.Append("</div></div>");
        return sb.ToString();
    }

    private string Colgroup()
    {
        var sb = new StringBuilder("<colgroup>");
        foreach (var w in ColWidths) sb.Append("<col style='width:" + w + "' />");
        sb.Append("</colgroup>");
        return sb.ToString();
    }

    private string RenderInvoiceSection(InvoiceSection section)
    {
        var sb = new StringBuilder();
        sb.Append("<div class='ref-block'>");
        sb.Append("<div class='reference-no'><span class='ref-label'>Reference No:</span> " + Encode(section.ReferenceNo) + "</div>");

        sb.Append("<table class='product-table'>");
        sb.Append(Colgroup());
        sb.Append("<thead>");
        sb.Append("<tr>");
        sb.Append("<th rowspan='2' class='vert'>Product Code</th>");
        sb.Append("<th rowspan='2'>Product &amp; Generic Name</th>");
        sb.Append("<th rowspan='2' class='vert'>Pack</th>");
        sb.Append("<th colspan='3'>Quantity</th>");
        sb.Append("<th colspan='2'>Per Pack</th>");
        sb.Append("<th colspan='2'>Total</th>");
        sb.Append("<th rowspan='2' class='vert'>Discount</th>");
        sb.Append("<th rowspan='2'>Net Amount</th>");
        sb.Append("</tr>");
        sb.Append("<tr><th>RQ</th><th>BQ</th><th>TQ</th><th>Unit Price</th><th class='vert'>Vat</th><th>Total Price</th><th class='vert'>Vat</th></tr>");
        sb.Append("</thead>");
        sb.Append("<tbody>");

        decimal subTotalPrice = 0, subVat = 0, subDiscount = 0, subNet = 0;

        foreach (DataRow row in section.Lines)
        {
            decimal quantity = ToDec(row["Quantity"]);
            decimal bonusQty = ToDec(row["BonusQuantity"]);
            decimal totalQty = ToDec(row["TotalQuantity"]);
            decimal unitPrice = ToDec(row["UnitPrice"]);
            decimal unitVat = ToDec(row["UnitVatAmount"]);
            decimal totalPrice = ToDec(row["TotalPrice"]);
            decimal totalVat = ToDec(row["TotalPriceVatAmount"]);
            decimal discount = ToDec(row["DiscountAmount"]);
            decimal netAmount = ToDec(row["NetAmount"]);

            subTotalPrice += totalPrice;
            subVat += totalVat;
            subDiscount += discount;
            subNet += netAmount;

            sb.Append("<tr>");
            sb.Append("<td class='center'>" + Encode(row["ProductCode"]) + "</td>");
            sb.Append("<td class='prod-name'><b>" + Encode(row["Product"]) + "</b><br/>" + Encode(row["GenericName"]) + "</td>");
            sb.Append("<td class='center vert'>" + Encode(row["PackSize"]) + "</td>");
            sb.Append("<td class='num'>" + quantity.ToString("N2") + "</td>");
            sb.Append("<td class='num'>" + bonusQty.ToString("N2") + "</td>");
            sb.Append("<td class='num'>" + totalQty.ToString("N2") + "</td>");
            sb.Append("<td class='num'>" + unitPrice.ToString("N2") + "</td>");
            sb.Append("<td class='num vert'>" + unitVat.ToString("N2") + "</td>");
            sb.Append("<td class='num'>" + totalPrice.ToString("N2") + "</td>");
            sb.Append("<td class='num vert'>" + totalVat.ToString("N2") + "</td>");
            sb.Append("<td class='num'>" + discount.ToString("N2") + "</td>");
            sb.Append("<td class='num'>" + netAmount.ToString("N2") + "</td>");
            sb.Append("</tr>");
        }

        sb.Append("</tbody>");
        sb.Append("<tfoot><tr class='subtotal-row'>");
        sb.Append("<td colspan='8' class='right'><b>Sub-Total:</b></td>");
        sb.Append("<td class='num'><b>" + subTotalPrice.ToString("N2") + "</b></td>");
        sb.Append("<td class='num'><b>" + subVat.ToString("N2") + "</b></td>");
        sb.Append("<td class='num'><b>" + subDiscount.ToString("N2") + "</b></td>");
        sb.Append("<td class='num'><b>" + subNet.ToString("N2") + "</b></td>");
        sb.Append("</tr></tfoot>");
        sb.Append("</table>");
        sb.Append("</div>");
        return sb.ToString();
    }

    private string RenderGrandTotalTable(decimal totalPrice, decimal vat, decimal discount, decimal netAmount)
    {
        var sb = new StringBuilder();
        sb.Append("<table class='product-table grand-total-table'>");
        sb.Append(Colgroup());
        sb.Append("<tr class='grand-total-row'>");
        sb.Append("<td colspan='8' class='right'><b>Grand Total:</b></td>");
        sb.Append("<td class='num'><b>" + totalPrice.ToString("N2") + "</b></td>");
        sb.Append("<td class='num'><b>" + vat.ToString("N2") + "</b></td>");
        sb.Append("<td class='num'><b>" + discount.ToString("N2") + "</b></td>");
        sb.Append("<td class='num'><b>" + netAmount.ToString("N2") + "</b></td>");
        sb.Append("</tr>");
        sb.Append("</table>");
        return sb.ToString();
    }

    private string RenderSignatureBlock(string preparedByValue)
    {
        var sb = new StringBuilder();
        sb.Append("<div class='signature-block'>");
        sb.Append(SignatureColumn("&nbsp;", "Customer Signature"));
        sb.Append(SignatureColumn(Encode(preparedByValue), "Prepared By"));
        sb.Append(SignatureColumn("&nbsp;", "Delivery By"));
        sb.Append(SignatureColumn("&nbsp;", "Authorised Signature"));
        sb.Append("</div>");
        return sb.ToString();
    }

    private static string SignatureColumn(string valueAboveLine, string label)
    {
        return "<div class='sig-col'><div class='sig-value'>" + valueAboveLine + "</div><div class='sig-line'></div><div class='sig-label'>" + label + "</div></div>";
    }

    // Matches the wording style shown by the original Crystal report for this exact Group Invoice
    // ("In Words: Three Lakhs Thirteen Thousand Four Hundred Eighty Only" for a Net Amount grand total
    // of 313,480.16): the fractional/paisa part is truncated silently, no currency name is appended, and
    // "Lakhs" is used (see NumberToWords.Convert). No parity claim is made beyond this one verified case.
    private static string BuildAmountInWords(decimal grandNetTotal)
    {
        long whole = (long)Math.Truncate(grandNetTotal);
        return NumberToWords.Convert(whole) + " Only";
    }

    private static decimal ToDec(object value)
    {
        return value == null || value == DBNull.Value ? 0 : Convert.ToDecimal(value);
    }

    private static string Encode(object value)
    {
        return HttpUtility.HtmlEncode(Convert.ToString(value));
    }

    private decimal SumColumn(string columnName)
    {
        decimal total = 0;
        foreach (DataRow row in lineData.Rows)
        {
            if (row[columnName] != DBNull.Value)
                total += Convert.ToDecimal(row[columnName]);
        }
        return total;
    }

    private static string FormatDate(object value)
    {
        DateTime date;
        return DateTime.TryParse(Convert.ToString(value), out date) ? date.ToString("dd-MMM-yyyy") : Convert.ToString(value);
    }

    // Cells use the Excel-specific "mso-number-format" style to force text formatting. This is the
    // standard, non-destructive way to stop Excel from evaluating a value beginning with =, +, - or @
    // as a formula when it opens an HTML table saved with an .xls extension: it changes only how Excel
    // *interprets* the cell (always as text), not the text itself, so legitimate product/customer names
    // are shown unmodified. Combined with HttpUtility.HtmlEncode (below), this also blocks the value
    // from being interpreted as markup if the file is ever opened as HTML instead of in Excel.
    // extraAttributes (e.g. a colspan) must be double-quoted: the style attribute itself is single-quoted
    // because its value contains literal double quotes (mso-number-format:"\@"). Reusing single quotes for
    // extraAttributes would terminate the style attribute early and silently drop the colspan — confirmed
    // live via a real Excel export request, where colspan='7' broke the markup this way.
    private static string ExcelTextCell(object value, string extraAttributes = "")
    {
        return "<td style='mso-number-format:\"\\@\";'" + extraAttributes + ">" + HttpUtility.HtmlEncode(Convert.ToString(value)) + "</td>";
    }

    private static string ExcelHeaderRow(params string[] labels)
    {
        var sb = new StringBuilder("<tr>");
        foreach (var l in labels) sb.Append("<th>" + HttpUtility.HtmlEncode(l) + "</th>");
        sb.Append("</tr>");
        return sb.ToString();
    }

    protected void exportExcelButton_Click(object sender, EventArgs e)
    {
        ExportExcel();
    }

    private void ExportExcel()
    {
        // Header block prints once for the whole Group Invoice (not per order/customer section) — the
        // grouping-per-section shown by the earlier version isn't wanted; only Customer, Address, Payment
        // Type, Delivery Person and Mobile No are needed, taken from the first section.
        if (lineData == null || lineData.Rows.Count == 0 || sections == null || sections.Count == 0) return;

        string groupInvoiceNoDisplay = Convert.ToString(lineData.Rows[0]["GroupInvoiceNo"]);
        string invoiceDateDisplay = FormatDate(lineData.Rows[0]["InvoiceDate"]);
        var firstSection = sections[0];

        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=GroupInvoice_" + groupInvoiceNo.Replace("/", "-") + ".xls");

        var sb = new StringBuilder();
        sb.Append("<table border='1'>");
        sb.Append("<tr><td>Group Invoice No</td>" + ExcelTextCell(groupInvoiceNoDisplay, " colspan=\"7\"") + "</tr>");
        sb.Append("<tr><td>Invoice Date</td>" + ExcelTextCell(invoiceDateDisplay, " colspan=\"7\"") + "</tr>");
        sb.Append("<tr><td colspan='9'>&nbsp;</td></tr>");
        sb.Append("<tr><td>Customer</td>" + ExcelTextCell(firstSection.CustomerName + " (" + firstSection.CustomerCode + ")", " colspan=\"7\"") + "</tr>");
        sb.Append("<tr><td>Address</td>" + ExcelTextCell(firstSection.Address, " colspan=\"7\"") + "</tr>");
        sb.Append("<tr><td>Payment Type</td>" + ExcelTextCell(firstSection.TermOfPayment, " colspan=\"7\"") + "</tr>");
        sb.Append("<tr><td>Delivery Person</td>" + ExcelTextCell(firstSection.DeliveryPersonName, " colspan=\"7\"") + "</tr>");
        sb.Append("<tr><td>Mobile No</td>" + ExcelTextCell(firstSection.DeliveryPersonPhNo, " colspan=\"7\"") + "</tr>");
        sb.Append("<tr><td colspan='9'>&nbsp;</td></tr>");

        decimal grandTotalPrice = 0, grandVat = 0, grandDiscount = 0, grandNet = 0;

        sb.Append(ExcelHeaderRow("Product Code", "Product", "Generic Name", "Pack", "TQ", "Unit Price", "Unit VAT", "Total Price", "VAT", "Discount", "Net Amount"));

        foreach (var section in sections)
        {
            decimal subTotalPrice = 0, subVat = 0, subDiscount = 0, subNet = 0;
            foreach (DataRow row in section.Lines)
            {
                decimal totalPrice = ToDec(row["TotalPrice"]);
                decimal totalVat = ToDec(row["TotalPriceVatAmount"]);
                decimal discount = ToDec(row["DiscountAmount"]);
                decimal netAmount = ToDec(row["NetAmount"]);
                subTotalPrice += totalPrice; subVat += totalVat; subDiscount += discount; subNet += netAmount;

                sb.Append("<tr>");
                sb.Append(ExcelTextCell(row["ProductCode"]));
                sb.Append(ExcelTextCell(row["Product"]));
                sb.Append(ExcelTextCell(row["GenericName"]));
                sb.Append(ExcelTextCell(row["PackSize"]));
                sb.Append(ExcelTextCell(ToDec(row["TotalQuantity"]).ToString("N2")));
                sb.Append(ExcelTextCell(ToDec(row["UnitPrice"]).ToString("N2")));
                sb.Append(ExcelTextCell(ToDec(row["UnitVatAmount"]).ToString("N2")));
                sb.Append(ExcelTextCell(totalPrice.ToString("N2")));
                sb.Append(ExcelTextCell(totalVat.ToString("N2")));
                sb.Append(ExcelTextCell(discount.ToString("N2")));
                sb.Append(ExcelTextCell(netAmount.ToString("N2")));
                sb.Append("</tr>");
            }

            grandTotalPrice += subTotalPrice; grandVat += subVat; grandDiscount += subDiscount; grandNet += subNet;
        }

        sb.Append("<tr><td colspan='7'><b>Grand Total</b></td>" + ExcelTextCell(grandTotalPrice.ToString("N2")) + ExcelTextCell(grandVat.ToString("N2")) + ExcelTextCell(grandDiscount.ToString("N2")) + ExcelTextCell(grandNet.ToString("N2")) + "</tr>");
        sb.Append("<tr><td>In Words</td>" + ExcelTextCell(BuildAmountInWords(grandNet), " colspan=\"10\"") + "</tr>");
        sb.Append("<tr><td>Remarks</td>" + ExcelTextCell(sections[sections.Count - 1].Remarks, " colspan=\"10\"") + "</tr>");
        sb.Append("</table>");

        Response.Write(sb.ToString());
        Response.End();
    }
}
