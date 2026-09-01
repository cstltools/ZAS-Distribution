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

public partial class HTMLReport_ProformaHTMLReport : System.Web.UI.Page
{
    NationalBeenCaedDal aReportBll = new NationalBeenCaedDal();
    ReportDocument rptdoc = new ReportDocument();

    protected void Page_Init(object sender, EventArgs e)
    {
        string branchId = Request.QueryString["branchId"];
        string comId = Request.QueryString["comId"];
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];
        string DC = Request.QueryString["DC"];

        dcTextBox.Text = DC;
        FromTextBox.Text = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
        toTextBox.Text = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");

        DataTable mainDataTable = aReportBll.GetWhBinCardReport(fromDate, toDate, branchId,DC,comId).Copy();


        if (mainDataTable.Rows.Count > 0)
        {
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();

            // Calculate Sums
            decimal totalWHOpeningStock = 0;
            decimal totalStockInQuantity = 0;
            decimal totalWHReceive = 0;
            decimal totalWHStockIssue = 0;
            decimal totalWHInTransit = 0;
            decimal totalWHFreeze = 0;
            decimal totalWHTotalIssue = 0;
            decimal totalWHStock = 0;
            decimal totalDepotOpeningStock = 0;
            decimal totalDepotReceive = 0;
            decimal totalB2BRcvQty = 0;
            decimal totalB2BTransitQty = 0;
            decimal totalFreezeQty = 0;
            decimal totalDepotTotalReceive = 0;
            decimal totalB2BIssueQty = 0;
            decimal totalSalesQuanty = 0;
            decimal totalBonusQuantity = 0;
            decimal totalSampleIssue = 0;
            decimal totalSalesTransitQty = 0;
            decimal totalDamageQty = 0;
            decimal totalDepotTotalIssue = 0;
            decimal totalDepotStock = 0;
            decimal totalClosingStock = 0;
            decimal totalClosingStockValue = 0;

            foreach (DataRow row in mainDataTable.Rows)
            {
                totalWHOpeningStock += row["WHOpeningStock"] != DBNull.Value ? Convert.ToDecimal(row["WHOpeningStock"]) : 0;
                totalStockInQuantity += row["StockInQuantity"] != DBNull.Value ? Convert.ToDecimal(row["StockInQuantity"]) : 0;
                totalWHReceive += row["WHReceive"] != DBNull.Value ? Convert.ToDecimal(row["WHReceive"]) : 0;
                totalWHStockIssue += row["WHStockIssue"] != DBNull.Value ? Convert.ToDecimal(row["WHStockIssue"]) : 0;
                totalWHInTransit += row["WHInTransit"] != DBNull.Value ? Convert.ToDecimal(row["WHInTransit"]) : 0;
                totalWHFreeze += row["WHFreeze"] != DBNull.Value ? Convert.ToDecimal(row["WHFreeze"]) : 0;
                totalWHTotalIssue += row["WHTotalIssue"] != DBNull.Value ? Convert.ToDecimal(row["WHTotalIssue"]) : 0;
                totalWHStock += row["WHStock"] != DBNull.Value ? Convert.ToDecimal(row["WHStock"]) : 0;
                totalDepotOpeningStock += row["DepotOpeningStock"] != DBNull.Value ? Convert.ToDecimal(row["DepotOpeningStock"]) : 0;
                totalDepotReceive += row["DepotReceive"] != DBNull.Value ? Convert.ToDecimal(row["DepotReceive"]) : 0;
                totalB2BRcvQty += row["B2BRcvQty"] != DBNull.Value ? Convert.ToDecimal(row["B2BRcvQty"]) : 0;
                totalB2BTransitQty += row["B2BTransitQty"] != DBNull.Value ? Convert.ToDecimal(row["B2BTransitQty"]) : 0;
                totalFreezeQty += row["FreezeQty"] != DBNull.Value ? Convert.ToDecimal(row["FreezeQty"]) : 0;
                totalDepotTotalReceive += row["DepotTotalReceive"] != DBNull.Value ? Convert.ToDecimal(row["DepotTotalReceive"]) : 0;
                totalB2BIssueQty += row["B2BIssueQty"] != DBNull.Value ? Convert.ToDecimal(row["B2BIssueQty"]) : 0;
                totalSalesQuanty += row["SalesQuanty"] != DBNull.Value ? Convert.ToDecimal(row["SalesQuanty"]) : 0;
                totalBonusQuantity += row["BonusQuantity"] != DBNull.Value ? Convert.ToDecimal(row["BonusQuantity"]) : 0;
                totalSampleIssue += row["SampleIssue"] != DBNull.Value ? Convert.ToDecimal(row["SampleIssue"]) : 0;
                totalSalesTransitQty += row["SalesTransitQty"] != DBNull.Value ? Convert.ToDecimal(row["SalesTransitQty"]) : 0;
                totalDamageQty += row["DamageQty"] != DBNull.Value ? Convert.ToDecimal(row["DamageQty"]) : 0;
                totalDepotTotalIssue += row["DepotTotalIssue"] != DBNull.Value ? Convert.ToDecimal(row["DepotTotalIssue"]) : 0;
                totalDepotStock += row["DepotStock"] != DBNull.Value ? Convert.ToDecimal(row["DepotStock"]) : 0;
                totalClosingStock += row["ClosingStock"] != DBNull.Value ? Convert.ToDecimal(row["ClosingStock"]) : 0;
                totalClosingStockValue += row["ClosingStockValue"] != DBNull.Value ? Convert.ToDecimal(row["ClosingStockValue"]) : 0;
            }

            // Assign Sums to GridView Footer
            loadGridView.FooterRow.Cells[1].Text = "Total";
            loadGridView.FooterRow.Cells[1].Font.Bold = true;

            loadGridView.FooterRow.Cells[3].Text = totalWHOpeningStock.ToString("N0");
            loadGridView.FooterRow.Cells[4].Text = totalStockInQuantity.ToString("N0");
            loadGridView.FooterRow.Cells[5].Text = totalWHReceive.ToString("N0");
            loadGridView.FooterRow.Cells[6].Text = totalWHStockIssue.ToString("N0");
            loadGridView.FooterRow.Cells[7].Text = totalWHInTransit.ToString("N0");
            loadGridView.FooterRow.Cells[8].Text = totalWHFreeze.ToString("N0");
            loadGridView.FooterRow.Cells[9].Text = totalWHTotalIssue.ToString("N0");
            loadGridView.FooterRow.Cells[10].Text = totalWHStock.ToString("N0");
            loadGridView.FooterRow.Cells[11].Text = totalDepotOpeningStock.ToString("N0");
            loadGridView.FooterRow.Cells[12].Text = totalDepotReceive.ToString("N0");
            loadGridView.FooterRow.Cells[13].Text = totalB2BRcvQty.ToString("N0");
            loadGridView.FooterRow.Cells[14].Text = totalB2BTransitQty.ToString("N0");
            loadGridView.FooterRow.Cells[15].Text = totalFreezeQty.ToString("N0");
            loadGridView.FooterRow.Cells[16].Text = totalDepotTotalReceive.ToString("N0");
            loadGridView.FooterRow.Cells[17].Text = totalB2BIssueQty.ToString("N0");
            loadGridView.FooterRow.Cells[18].Text = totalSalesQuanty.ToString("N0");
            loadGridView.FooterRow.Cells[19].Text = totalBonusQuantity.ToString("N0");
            loadGridView.FooterRow.Cells[20].Text = totalSampleIssue.ToString("N0");
            loadGridView.FooterRow.Cells[21].Text = totalSalesTransitQty.ToString("N0");
            loadGridView.FooterRow.Cells[22].Text = totalDamageQty.ToString("N0");
            loadGridView.FooterRow.Cells[23].Text = totalDepotTotalIssue.ToString("N0");
            loadGridView.FooterRow.Cells[24].Text = totalDepotStock.ToString("N0");
            loadGridView.FooterRow.Cells[25].Text = totalClosingStock.ToString("N0");
            loadGridView.FooterRow.Cells[26].Text = "-";
            loadGridView.FooterRow.Cells[27].Text = totalClosingStockValue.ToString("N4");

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
        

        //var ds = new DataSet();

        //mainDataTable.TableName = "AreaWiseMonthlyInventoryReportDataTable";
        //ds.Tables.Add(mainDataTable);

        //rptdoc.Load(ReportPath("rptAreaWiseMonthlyInventoryReport.rpt"));
        //rptdoc.SetDataSource(ds);

        //loadGridView.DataSource = rptdoc;
        //crvSalesRpt.DataBind();

    }
    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }

    //protected void crvSalesRpt_Unload(object sender, EventArgs e)
    //{
    //    if (this.rptdoc != null)
    //    {
    //        rptdoc.Close();
    //        rptdoc.Dispose();
    //        crvSalesRpt.Dispose();
    //    }
    //}
    //protected void crvSalesRpt_Disposed(object sender, EventArgs e)
    //{
    //    if (this.rptdoc != null)
    //    {
    //        rptdoc.Close();
    //        rptdoc.Dispose();
    //        crvSalesRpt.Dispose();
    //    }
    //}
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        string branchId = Request.QueryString["branchId"];
        string comId = Request.QueryString["comId"];
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];
        string DC = Request.QueryString["DC"];

        dcTextBox.Text = DC;
        FromTextBox.Text = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
        toTextBox.Text = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");

        DataTable mainDataTable = aReportBll.GetWhBinCardReport(fromDate, toDate, branchId, DC, comId).Copy();

        var ds = new DataSet();

        mainDataTable.TableName = "NationalBeenCard";
        ds.Tables.Add(mainDataTable);

        rptdoc.Load(ReportPath("rptNationalBinCardExcel.rpt"));
        rptdoc.SetDataSource(ds);

        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
        "WhBinCard_Report");
    }
}