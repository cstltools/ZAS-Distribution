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
using Library.DAL.SInventory_DAL;

public partial class HTMLReport_NationalStockReportHTMLReport : System.Web.UI.Page
{
    WhStockConditionFreezeDal aReportDal = new WhStockConditionFreezeDal();
    ReportDocument rptdoc = new ReportDocument();

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["PRAM"] == null)
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
            return;
        }

        string parameter = Session["PRAM"].ToString();
        string companyName = Session["CompanyName"] != null ? Session["CompanyName"].ToString() : "All Companies";

        companyTextBox.Text = companyName;

        DataTable mainDataTable = aReportDal.LoadNationalStockInfo(parameter).Copy();

        if (mainDataTable.Rows.Count > 0)
        {
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();

            // Calculate Sums
            decimal totalAQty = 0;
            decimal totalBookFDel = 0;
            decimal totalTqty = 0;
            decimal totalRQty = 0;
            decimal totalBQty = 0;
            decimal totalWHQty = 0;
            decimal totalFZQty = 0;
            decimal totalReturnRcvQty = 0;
            decimal totalTotalQty = 0;

            decimal totalAvailableValue = 0;
            decimal totalRestrictedValue = 0;
            decimal totalTransitValue = 0;
            decimal totalBlockedValue = 0;
            decimal totalWhStockValue = 0;
            decimal totalReturnRcvValue = 0;
            decimal totalGrandTotal = 0;

            foreach (DataRow row in mainDataTable.Rows)
            {
                totalAQty += row["AQty"] != DBNull.Value ? Convert.ToDecimal(row["AQty"]) : 0;
                totalBookFDel += row["BookFDel"] != DBNull.Value ? Convert.ToDecimal(row["BookFDel"]) : 0;
                totalTqty += row["Tqty"] != DBNull.Value ? Convert.ToDecimal(row["Tqty"]) : 0;
                totalRQty += row["RQty"] != DBNull.Value ? Convert.ToDecimal(row["RQty"]) : 0;
                totalBQty += row["BQty"] != DBNull.Value ? Convert.ToDecimal(row["BQty"]) : 0;

                if (row["WHQty"] != DBNull.Value && row["WHQty"].ToString().Trim() != "")
                {
                    decimal whVal = 0;
                    if (decimal.TryParse(row["WHQty"].ToString(), out whVal))
                    {
                        totalWHQty += whVal;
                    }
                }

                totalFZQty += row["FZQty"] != DBNull.Value ? Convert.ToDecimal(row["FZQty"]) : 0;
                totalReturnRcvQty += row["ReturnRcvQty"] != DBNull.Value ? Convert.ToDecimal(row["ReturnRcvQty"]) : 0;
                totalTotalQty += row["TotalQty"] != DBNull.Value ? Convert.ToDecimal(row["TotalQty"]) : 0;

                totalAvailableValue += row["AvialableValue"] != DBNull.Value ? Convert.ToDecimal(row["AvialableValue"]) : 0;
                totalRestrictedValue += row["RestrictedValue"] != DBNull.Value ? Convert.ToDecimal(row["RestrictedValue"]) : 0;
                totalTransitValue += row["TransitValue"] != DBNull.Value ? Convert.ToDecimal(row["TransitValue"]) : 0;
                totalBlockedValue += row["BlockedValue"] != DBNull.Value ? Convert.ToDecimal(row["BlockedValue"]) : 0;
                totalWhStockValue += row["WhStockValue"] != DBNull.Value ? Convert.ToDecimal(row["WhStockValue"]) : 0;
                totalReturnRcvValue += row["ReturnRcvValue"] != DBNull.Value ? Convert.ToDecimal(row["ReturnRcvValue"]) : 0;
                totalGrandTotal += row["GrandTotal"] != DBNull.Value ? Convert.ToDecimal(row["GrandTotal"]) : 0;
            }

            // Assign Sums to GridView Footer
            loadGridView.FooterRow.Cells[1].Text = "Total";
            loadGridView.FooterRow.Cells[1].Font.Bold = true;

            loadGridView.FooterRow.Cells[3].Text = totalAQty.ToString("N0");
            loadGridView.FooterRow.Cells[4].Text = totalBookFDel.ToString("N0");
            loadGridView.FooterRow.Cells[5].Text = totalTqty.ToString("N0");
            loadGridView.FooterRow.Cells[6].Text = totalRQty.ToString("N0");
            loadGridView.FooterRow.Cells[7].Text = totalBQty.ToString("N0");
            loadGridView.FooterRow.Cells[8].Text = totalWHQty.ToString("N0");
            loadGridView.FooterRow.Cells[9].Text = totalFZQty.ToString("N0");
            loadGridView.FooterRow.Cells[10].Text = totalReturnRcvQty.ToString("N0");
            loadGridView.FooterRow.Cells[11].Text = totalTotalQty.ToString("N0");

            loadGridView.FooterRow.Cells[12].Text = totalAvailableValue.ToString("N4");
            loadGridView.FooterRow.Cells[13].Text = totalRestrictedValue.ToString("N4");
            loadGridView.FooterRow.Cells[14].Text = totalTransitValue.ToString("N4");
            loadGridView.FooterRow.Cells[15].Text = totalBlockedValue.ToString("N4");
            loadGridView.FooterRow.Cells[16].Text = totalWhStockValue.ToString("N4");
            loadGridView.FooterRow.Cells[17].Text = totalReturnRcvValue.ToString("N4");
            loadGridView.FooterRow.Cells[18].Text = "-";
            loadGridView.FooterRow.Cells[19].Text = totalGrandTotal.ToString("N4");

            foreach (TableCell cell in loadGridView.FooterRow.Cells)
            {
                cell.Font.Bold = true;
            }
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
        }
    }

    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["PRAM"] == null) return;

        string parameter = Session["PRAM"].ToString();
        DataTable mainDataTable = aReportDal.LoadNationalStockInfo(parameter).Copy();

        var ds = new DataSet();
        mainDataTable.TableName = "NationalStockReportData";
        ds.Tables.Add(mainDataTable);

        rptdoc.Load(ReportPath("crpNationalStockReport.rpt"));
        rptdoc.SetDataSource(ds);

        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "NationalStockReport");
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        if (rptdoc != null)
        {
            rptdoc.Close();
            rptdoc.Dispose();
        }
    }
}
