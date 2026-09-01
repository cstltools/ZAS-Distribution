using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using Library.DAL.SInventory_DAL;

public partial class HTMLReport_SalesPerformanceReport : System.Web.UI.Page
{
    SalesPerformanceReportProductWise_Dal aDal = new SalesPerformanceReportProductWise_Dal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int companyId = ToInt(Request.QueryString["CompanyId"]);
            int? comUnitId = ToNullableInt(Request.QueryString["ComUnitId"]);

            DateTime? fromDate = ToNullableDate(Request.QueryString["FromDate"]);
            DateTime? toDate = ToNullableDate(Request.QueryString["ToDate"]);

            if (companyId <= 0 || fromDate == null || toDate == null)
            {
                Response.Write("<div style='text-align:center;color:red;font-family:Arial;'>Invalid parameters. Required: CompanyId, FromDate, ToDate.</div>");
                return;
            }

            if (fromDate.Value.Date > toDate.Value.Date)
            {
                Response.Write("<div style='text-align:center;color:red;font-family:Arial;'>FromDate cannot be greater than ToDate.</div>");
                return;
            }

            // SP is month/year based
            if (fromDate.Value.Month != toDate.Value.Month || fromDate.Value.Year != toDate.Value.Year)
            {
                Response.Write("<div style='text-align:center;color:red;font-family:Arial;'>This report supports Date Range within same Month & Year only. Please select a single month range.</div>");
                return;
            }

            string period = fromDate.Value.ToString("MMMM", CultureInfo.InvariantCulture);
            int year = fromDate.Value.Year;

            int? regionId = null;
            int? areaId = null;
            int? territoryId = null;
            int? mioId = null;

            aDal.CreateConnection();

            DataTable dt = aDal.GetSalesVsTargetReport_ProductWise(
                companyId,
                comUnitId,
                regionId,
                areaId,
                territoryId,
                mioId,
                period,
                year
            );

            aDal.CloseAllConnection();

            if (dt == null || dt.Rows.Count == 0)
            {
                Response.Write("<div style='text-align:center;color:red;font-family:Arial;'>No data found.</div>");
                return;
            }

            string html = BuildReportHtml(dt, companyId, comUnitId, fromDate.Value, toDate.Value, period, year);
            Response.Write(html);
        }
    }

    private string BuildReportHtml(DataTable dt, int companyId, int? comUnitId, DateTime fromDate, DateTime toDate, string period, int year)
    {
        // ---- Sort (grouping friendly) ----
        DataView dv = dt.DefaultView;
        dv.Sort = "RegionName, AreaName, TerritoryName, RsmName, AsmName, MioName, ProductName";
        DataTable sdt = dv.ToTable();

        // ---- Merge duplicates per (Region/Area/Territory/RSM/ASM/MIO/Product) ----
        if (HasCol(sdt, "ProductCode"))
        {
            var merged = sdt.AsEnumerable()
                .GroupBy(r => new
                {
                    CompanyId = SafeStr(GetAny(r, "CompanyId")),
                    WingsName = SafeStr(GetAny(r, "WingsName")),
                    ComUnitId = SafeStr(GetAny(r, "ComUnitId")),
                    ComUnitName = SafeStr(GetAny(r, "ComUnitName")),

                    RegionId = SafeStr(GetAny(r, "RegionId")),
                    RegionName = SafeStr(GetAny(r, "RegionName")),

                    RsmName = SafeStr(GetAny(r, "RsmName")),

                    AreaId = SafeStr(GetAny(r, "AreaId")),
                    AreaName = SafeStr(GetAny(r, "AreaName")),

                    AsmName = SafeStr(GetAny(r, "AsmName")),

                    TerritoryId = SafeStr(GetAny(r, "TerritoryId")),
                    TerritoryName = SafeStr(GetAny(r, "TerritoryName")),

                    MioName = SafeStr(GetAny(r, "MioName")),
                    IsMioActive = SafeStr(GetAny(r, "IsMioActive")),

                    ProductCode = SafeStr(GetAny(r, "ProductCode")),
                    ProductName = SafeStr(GetAny(r, "ProductName")),
                    PackSize = SafeStr(GetAny(r, "PackSize"))
                })
                .Select(g =>
                {
                    DataRow n = sdt.NewRow();
                    DataRow f = g.First();

                    foreach (DataColumn c in sdt.Columns)
                        n[c.ColumnName] = f[c.ColumnName];

                    n["TargetQty"] = g.Sum(x => ToDec(GetAny(x, "TargetQty")));
                    n["SaleQty"] = g.Sum(x => ToDec(GetAny(x, "SaleQty")));
                    n["SalesValue"] = g.Sum(x => ToDec(GetAny(x, "SalesValue")));
                    n["SalesTarget"] = g.Sum(x => ToDec(GetAny(x, "SalesTarget")));

                    // keep DB fields as-is:
                    // Achievement, Sales_Target_Status, Average_Up

                    return n;
                });

            DataTable mergedTable = sdt.Clone();
            foreach (var r in merged) mergedTable.Rows.Add(r.ItemArray);
            sdt = mergedTable;
        }

        StringBuilder sb = new StringBuilder();

        sb.Append(@"
<!DOCTYPE html>
<html>
<head>
<title>Sales Performance - Product Wise</title>
<style>
    body{font-family:Arial;font-size:12px;margin:0;padding:10px;background:#fff;color:#000;}
    .title{font-size:18px;font-weight:800;text-align:center;margin:0;}
    .sub{font-size:12px;text-align:center;margin:6px 0 12px 0;}
    .meta{width:98%;margin:0 auto 10px auto;display:flex;justify-content:space-between;gap:10px;flex-wrap:wrap;}
    .meta .box{border:1px solid #bbb;padding:8px 12px;border-radius:6px;min-width:280px;}
    .meta b{display:inline-block;min-width:100px;}

    table{width:98%;margin:0 auto;border-collapse:collapse;table-layout:fixed;}
    th,td{border:1px solid #999;padding:6px 6px;vertical-align:middle;}
    th{background:#f2f2f2;text-align:center;font-weight:800;white-space:nowrap;}
    td{background:#fff;}
    .left{text-align:left;}
    .right{text-align:right;}
    .center{text-align:center;}
    .wrap{white-space:normal;word-break:break-word;}

    /* Subtotal rows only */
    .subMIO td{background:#FFF3E0;font-weight:900;}
    .subASM td{background:#E8F5E9;font-weight:900;}
    .subRSM td{background:#E1F5FE;font-weight:900;}
    .subTerr td{background:#FFF7E6;font-weight:900;}
    .subArea td{background:#E9F7EF;font-weight:900;}
    .subRegion td{background:#E8F4FF;font-weight:900;}
    .grand td{background:#f0ad4e;color:#fff;font-weight:900;}
</style>
</head>
<body>
");

        sb.Append("<div class='title'>Sales Performance Report (Product Wise)</div>");
        sb.AppendFormat(
            "<div class='sub'><b>Month:</b> {0} {1} &nbsp; | &nbsp; <b>Date Range:</b> {2} to {3}</div>",
            Html(period), year,
            fromDate.ToString("dd-MMM-yyyy"),
            toDate.ToString("dd-MMM-yyyy")
        );

        string wing = SafeStr(HasCol(sdt, "WingsName") ? sdt.Rows[0]["WingsName"] : "");
        string unitName = SafeStr(HasCol(sdt, "ComUnitName") ? sdt.Rows[0]["ComUnitName"] : "");

        sb.Append("<div class='meta'>");
        sb.Append("<div class='box'>");
        sb.AppendFormat("<div><b>CompanyId</b>: {0}</div>", companyId);
        sb.AppendFormat("<div><b>WingsName</b>: {0}</div>", Html(string.IsNullOrEmpty(wing) ? "-" : wing));
        sb.Append("</div>");

        sb.Append("<div class='box'>");
        sb.AppendFormat("<div><b>ComUnitId</b>: {0}</div>", comUnitId.HasValue ? comUnitId.Value.ToString() : "All");
        sb.AppendFormat("<div><b>ComUnitName</b>: {0}</div>", Html(string.IsNullOrEmpty(unitName) ? "-" : unitName));
        sb.Append("</div>");
        sb.Append("</div>");

        // ---- Table Header (use your headers) ----
        sb.Append("<table>");
        sb.Append(@"
<thead>
<tr>
    <th style='width:40px;'>SL</th>
    <th style='width:70px;'>CompanyId</th>
    <th style='width:190px;'>WingsName</th>
    <th style='width:70px;'>ComUnitId</th>
    <th style='width:190px;'>ComUnitName</th>
    <th style='width:70px;'>RegionId</th>
    <th style='width:140px;'>RegionName</th>
    <th style='width:160px;'>RsmName</th>
    <th style='width:70px;'>AreaId</th>
    <th style='width:140px;'>AreaName</th>
    <th style='width:160px;'>AsmName</th>
    <th style='width:80px;'>TerritoryId</th>
    <th style='width:160px;'>TerritoryName</th>
    <th style='width:160px;'>MioName</th>
    <th style='width:70px;'>IsMioActive</th>
    <th style='width:90px;'>ProductCode</th>
    <th style='width:260px;'>ProductName</th>
    <th style='width:80px;'>PackSize</th>
    <th style='width:80px;'>TargetQty</th>
    <th style='width:80px;'>SaleQty</th>
    <th style='width:110px;'>SalesValue</th>
    <th style='width:90px;'>Achievement</th>
    <th style='width:160px;'>Sales_Target_Status</th>
    <th style='width:90px;'>Average_Up</th>
    <th style='width:110px;'>SalesTarget</th>
</tr>
</thead>
<tbody>
");

        int sl = 0;

        decimal grandTargetQty = 0m, grandSaleQty = 0m, grandSalesValue = 0m, grandSalesTarget = 0m;

        var regionGroups = sdt.AsEnumerable().GroupBy(r => SafeStr(GetAny(r, "RegionId", "RegionName")));
        foreach (var regionGroup in regionGroups)
        {
            var areaGroups = regionGroup.GroupBy(r => SafeStr(GetAny(r, "AreaId", "AreaName")));
            foreach (var areaGroup in areaGroups)
            {
                var terrGroups = areaGroup.GroupBy(r => SafeStr(GetAny(r, "TerritoryId", "TerritoryName")));
                foreach (var terrGroup in terrGroups)
                {
                    var rsmGroups = terrGroup.GroupBy(r => SafeStr(GetAny(r, "RsmName")));
                    foreach (var rsmGroup in rsmGroups)
                    {
                        var asmGroups = rsmGroup.GroupBy(r => SafeStr(GetAny(r, "AsmName")));
                        foreach (var asmGroup in asmGroups)
                        {
                            var mioGroups = asmGroup.GroupBy(r => SafeStr(GetAny(r, "MioName")));
                            foreach (var mioGroup in mioGroups)
                            {
                                foreach (DataRow row in mioGroup)
                                {
                                    sl++;

                                    sb.Append("<tr>");
                                    sb.AppendFormat("<td class='center'>{0}</td>", sl);

                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "CompanyId"))));
                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "WingsName"))));
                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "ComUnitId"))));
                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "ComUnitName"))));

                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "RegionId"))));
                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "RegionName"))));

                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "RsmName"))));

                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "AreaId"))));
                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "AreaName"))));

                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "AsmName"))));

                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "TerritoryId"))));
                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "TerritoryName"))));

                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "MioName"))));
                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "IsMioActive"))));

                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "ProductCode"))));
                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(row, "ProductName"))));
                                    sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(row, "PackSize"))));

                                    decimal targetQty = ToDec(GetAny(row, "TargetQty"));
                                    decimal saleQty = ToDec(GetAny(row, "SaleQty"));
                                    decimal salesValue = ToDec(GetAny(row, "SalesValue"));
                                    decimal salesTarget = ToDec(GetAny(row, "SalesTarget"));

                                    decimal achDb = ToDec(GetAny(row, "Achievement"));
                                    string status = SafeStr(GetAny(row, "Sales_Target_Status"));
                                    decimal avgUp = ToDec(GetAny(row, "Average_Up"));

                                    sb.AppendFormat("<td class='right'>{0}</td>", targetQty.ToString("0"));
                                    sb.AppendFormat("<td class='right'>{0}</td>", saleQty.ToString("0"));
                                    sb.AppendFormat("<td class='right'>{0}</td>", salesValue.ToString("0.00"));
                                    sb.AppendFormat("<td class='right'>{0}</td>", achDb.ToString("0.00"));
                                    sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(status));
                                    sb.AppendFormat("<td class='right'>{0}</td>", avgUp.ToString("0.00"));
                                    sb.AppendFormat("<td class='right'>{0}</td>", salesTarget.ToString("0.00"));

                                    sb.Append("</tr>");

                                    grandTargetQty += targetQty;
                                    grandSaleQty += saleQty;
                                    grandSalesValue += salesValue;
                                    grandSalesTarget += salesTarget;
                                }

                                // MIO Subtotal
                                AppendSubtotalRow(sb, "subMIO", "Subtotal (MIO)", mioGroup.First(), mioGroup);
                            }

                            // ASM Subtotal
                            AppendSubtotalRow(sb, "subASM", "Subtotal (ASM)", asmGroup.First(), asmGroup);
                        }

                        // RSM Subtotal
                        AppendSubtotalRow(sb, "subRSM", "Subtotal (RSM)", rsmGroup.First(), rsmGroup);
                    }

                    // Territory Subtotal
                    AppendSubtotalRow(sb, "subTerr", "Subtotal (Territory)", terrGroup.First(), terrGroup);
                }

                // Area Subtotal
                AppendSubtotalRow(sb, "subArea", "Subtotal (Area)", areaGroup.First(), areaGroup);
            }

            // Region Subtotal
            AppendSubtotalRow(sb, "subRegion", "Subtotal (Region)", regionGroup.First(), regionGroup);
        }

        // Grand Total
        sb.Append("<tr class='grand'>");
        sb.Append("<td class='center'></td>");
        sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(sdt.Rows[0], "CompanyId"))));
        sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(sdt.Rows[0], "WingsName"))));
        sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(sdt.Rows[0], "ComUnitId"))));
        sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(sdt.Rows[0], "ComUnitName"))));
        sb.Append("<td></td><td class='left'><b>GRAND TOTAL</b></td>");
        sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
        sb.Append("<td></td><td class='left'></td><td></td>");

        decimal grandAch = grandSalesTarget <= 0m ? 0m : Math.Round((grandSalesValue * 100m) / grandSalesTarget, 2);

        sb.AppendFormat("<td class='right'>{0}</td>", grandTargetQty.ToString("0"));
        sb.AppendFormat("<td class='right'>{0}</td>", grandSaleQty.ToString("0"));
        sb.AppendFormat("<td class='right'>{0}</td>", grandSalesValue.ToString("0.00"));
        sb.AppendFormat("<td class='right'>{0}</td>", grandAch.ToString("0.00"));
        sb.Append("<td class='left wrap'></td>");
        sb.Append("<td class='right'></td>");
        sb.AppendFormat("<td class='right'>{0}</td>", grandSalesTarget.ToString("0.00"));
        sb.Append("</tr>");

        sb.Append("</tbody></table>");
        sb.Append("</body></html>");
        return sb.ToString();
    }

    /// <summary>
    /// Subtotal row prints SAME columns (your headers) and keeps non-sum fields blank except IDs/names from sampleRow.
    /// </summary>
    private void AppendSubtotalRow(StringBuilder sb, string cssClass, string label, DataRow sampleRow, IEnumerable<DataRow> rows)
    {
        decimal sumTargetQty = rows.Sum(r => ToDec(GetAny(r, "TargetQty")));
        decimal sumSaleQty = rows.Sum(r => ToDec(GetAny(r, "SaleQty")));
        decimal sumSalesValue = rows.Sum(r => ToDec(GetAny(r, "SalesValue")));
        decimal sumSalesTarget = rows.Sum(r => ToDec(GetAny(r, "SalesTarget")));

        decimal ach = sumSalesTarget <= 0m ? 0m : Math.Round((sumSalesValue * 100m) / sumSalesTarget, 2);

        sb.AppendFormat("<tr class='{0}'>", cssClass);

        sb.Append("<td class='center'></td>");
        sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(sampleRow, "CompanyId"))));
        sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(sampleRow, "WingsName"))));
        sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(sampleRow, "ComUnitId"))));
        sb.AppendFormat("<td class='left wrap'>{0}</td>", Html(SafeStr(GetAny(sampleRow, "ComUnitName"))));

        sb.AppendFormat("<td class='center'>{0}</td>", Html(SafeStr(GetAny(sampleRow, "RegionId"))));
        sb.AppendFormat("<td class='left wrap'><b>{0}</b></td>", Html(label));

        sb.Append("<td></td>"); // RsmName
        sb.Append("<td></td>"); // AreaId
        sb.Append("<td></td>"); // AreaName
        sb.Append("<td></td>"); // AsmName
        sb.Append("<td></td>"); // TerritoryId
        sb.Append("<td></td>"); // TerritoryName
        sb.Append("<td></td>"); // MioName
        sb.Append("<td></td>"); // IsMioActive
        sb.Append("<td></td>"); // ProductCode
        sb.Append("<td></td>"); // ProductName
        sb.Append("<td></td>"); // PackSize

        sb.AppendFormat("<td class='right'>{0}</td>", sumTargetQty.ToString("0"));
        sb.AppendFormat("<td class='right'>{0}</td>", sumSaleQty.ToString("0"));
        sb.AppendFormat("<td class='right'>{0}</td>", sumSalesValue.ToString("0.00"));
        sb.AppendFormat("<td class='right'>{0}</td>", ach.ToString("0.00"));

        sb.Append("<td class='left wrap'></td>"); // Sales_Target_Status
        sb.Append("<td class='right'></td>");     // Average_Up
        sb.AppendFormat("<td class='right'>{0}</td>", sumSalesTarget.ToString("0.00"));

        sb.Append("</tr>");
    }

    // ---------------- Helpers ----------------

    private int ToInt(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return 0;
        s = s.Trim();
        if (string.Equals(s, "NULL", StringComparison.OrdinalIgnoreCase)) return 0;
        return Convert.ToInt32(s, CultureInfo.InvariantCulture);
    }

    private int? ToNullableInt(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        s = s.Trim();
        if (string.Equals(s, "NULL", StringComparison.OrdinalIgnoreCase)) return null;

        int v;
        if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
            return v;
        return null;
    }

    private DateTime? ToNullableDate(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        s = s.Trim();

        DateTime d;
        if (DateTime.TryParseExact(s, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            return d;

        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            return d;

        return null;
    }

    private decimal ToDec(object v)
    {
        if (v == null || v == DBNull.Value) return 0m;
        string s = Convert.ToString(v).Trim();
        if (s == "") return 0m;

        decimal d;
        if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
            return d;

        return Convert.ToDecimal(v);
    }

    private string SafeStr(object v)
    {
        if (v == null || v == DBNull.Value) return "";
        return Convert.ToString(v).Trim();
    }

    private string Html(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        return Server.HtmlEncode(s);
    }

    private bool HasCol(DataTable dt, string col)
    {
        return dt != null && dt.Columns.Contains(col);
    }

    private object GetAny(DataRow r, params string[] cols)
    {
        foreach (var c in cols)
        {
            if (r.Table.Columns.Contains(c)) return r[c];
        }
        return DBNull.Value;
    }
}
