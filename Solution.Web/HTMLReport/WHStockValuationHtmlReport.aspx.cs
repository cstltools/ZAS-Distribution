using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Library.DAL.SInventory_DAL;

public partial class HTMLReport_WHStockValuationHtmlReport : System.Web.UI.Page
{
    private readonly WHStockValuationReprtDal aReportBll = new WHStockValuationReprtDal();
    private readonly ReportDocument rptdoc = new ReportDocument();

    // Totals (for footer)

    private decimal totalOpeningValue = 0;
    private decimal totalClosingValue = 0;
    private decimal totalInQty = 0;
    private decimal totalOutQty = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            // Session inputs

            string comId = Convert.ToString(Session["ComId"]);     // required
            string fromDate = Convert.ToString(Session["FDate"]); // required (string date)
            string toDate = Convert.ToString(Session["TDate"]);   // required (string date)

            // optional

            string productId = Convert.ToString(Session["ProductId"]); // can be null/empty
            string companyName = Convert.ToString(Session["CompanyName"]);
            string productName = Convert.ToString(Session["ProductName"]);
            string warehouseName = Convert.ToString(Session["WarehouseName"]);

            // show header labels

            FromTextBox.Text = SafeToDateText(fromDate);
            toTextBox.Text = SafeToDateText(toDate);

            if (companyTextBox != null) companyTextBox.Text = string.IsNullOrWhiteSpace(companyName) ? "All" : companyName;
            if (productTextBox != null) productTextBox.Text = string.IsNullOrWhiteSpace(productName) ? "All" : productName;
            if (warehouseTextBox != null) warehouseTextBox.Text = string.IsNullOrWhiteSpace(warehouseName) ? "All" : warehouseName;

            // Load data (DAL should call your SP: dbo.sp_INV_ValuationReport)

            DataTable mainDataTable = aReportBll.LoadWHStockValueationReportByPram(fromDate, toDate, productId, comId).Copy();

            loadGridView.DataSource = mainDataTable.Rows.Count > 0 ? mainDataTable : null;
            loadGridView.DataBind();
        }
        catch
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
        }
    }

    private string SafeToDateText(string dt)
    {
        DateTime d;
        if (DateTime.TryParse(dt, out d))
            return d.ToString("dd-MMM-yyyy");
        return dt ?? "";
    }

    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Columns coming from your Valuation SP output:
            // ProductCode, ProductName, PackSize, OpeningQty, OpeningRate, OpeningValue, InQty, OutQty, ClosingQty, ClosingRate, ClosingValue

            totalOpeningValue += SafeDec(DataBinder.Eval(e.Row.DataItem, "OpeningValue"));
            totalClosingValue += SafeDec(DataBinder.Eval(e.Row.DataItem, "ClosingValue"));
            totalInQty += SafeDec(DataBinder.Eval(e.Row.DataItem, "InQty"));
            totalOutQty += SafeDec(DataBinder.Eval(e.Row.DataItem, "OutQty"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            // Grid columns index (based on your updated ASPX):
            // 0 ProductCode
            // 1 ProductName
            // 2 PackSize
            // 3 OpeningQty
            // 4 OpeningRate
            // 5 OpeningValue
            // 6 InQty
            // 7 OutQty
            // 8 ClosingQty
            // 9 ClosingRate
            // 10 ClosingValue

            e.Row.Cells[2].Text = "Total:";
            e.Row.Cells[5].Text = totalOpeningValue.ToString("N2");
            e.Row.Cells[6].Text = totalInQty.ToString("N3");
            e.Row.Cells[7].Text = totalOutQty.ToString("N3");
            e.Row.Cells[10].Text = totalClosingValue.ToString("N2");

            // Right align numeric footer cells

            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;

            e.Row.Font.Bold = true;

            // Optional label under grid (if exists in ASPX)

            if (lblTotalClosingValue != null)
                lblTotalClosingValue.Text = totalClosingValue.ToString("N2");
        }
    }

    private decimal SafeDec(object val)
    {
        if (val == null || val == DBNull.Value) return 0m;
        decimal d;
        if (decimal.TryParse(Convert.ToString(val), NumberStyles.Any, CultureInfo.InvariantCulture, out d))
            return d;

        // fallback for current culture

        if (decimal.TryParse(Convert.ToString(val), out d))
            return d;
        return 0m;
    }

    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }

    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        string comId = Convert.ToString(Session["ComId"]);
        string fromDate = Convert.ToString(Session["FDate"]);
        string toDate = Convert.ToString(Session["TDate"]);
        string productId = Convert.ToString(Session["ProductId"]); // optional

        FromTextBox.Text = SafeToDateText(fromDate);
        toTextBox.Text = SafeToDateText(toDate);

        DataTable dt = aReportBll.LoadWHStockValueationReportByPram(fromDate, toDate, productId, comId).Copy();

        // if no data
        if (dt == null || dt.Rows.Count == 0)
        {
            // optional: show message
            // ShowMessageBox("No data found!");
            return;
        }

        ExportDataTableToExcel(dt, "WHStockValuation_Report");
    }

    private void ExportDataTableToExcel(DataTable dt, string fileName)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");

        // Excel formatting (keeps leading zeros)
        Response.Write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body>");
        Response.Write("<table border='1' style='border-collapse:collapse; font-family:Segoe UI; font-size:12px;'>");

        // Header
        Response.Write("<tr style='background:#0b74c7;color:#fff;font-weight:bold;'>");
        foreach (DataColumn col in dt.Columns)
        {
            Response.Write("<td>" + HttpUtility.HtmlEncode(col.ColumnName) + "</td>");
        }
        Response.Write("</tr>");

        // Rows
        foreach (DataRow row in dt.Rows)
        {
            Response.Write("<tr>");
            foreach (DataColumn col in dt.Columns)
            {
                string val = row[col] == DBNull.Value ? "" : row[col].ToString();

                // Force Excel to treat as text when needed
                // e.g. ProductCode like 00123
                Response.Write("<td style='mso-number-format:\"\\@\";'>" + HttpUtility.HtmlEncode(val) + "</td>");
            }
            Response.Write("</tr>");
        }

        Response.Write("</table>");
        Response.Write("</body></html>");

        Response.Flush();
        Response.End();
    }

}
