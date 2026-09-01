using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Library.DAL.SInventory_DAL;

public partial class HTMLReport_SalesSummeryForAccHtmlReport : System.Web.UI.Page
{
    SalesSummeryReportForAcc aReportBll = new SalesSummeryReportForAcc();
    ReportDocument rptdoc = new ReportDocument();

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["UnitId"] == null || Session["ComId"] == null || Session["FDate"] == null || Session["TDate"] == null)
        {
            Response.Write("<h3 style='color:red;font-family:sans-serif;padding:20px;'>Session expired or invalid request. Please load the report from the Sales/Accounts menu.</h3>");
            Response.End();
            return;
        }

        string branchId = Session["UnitId"].ToString();
        string comId = Session["ComId"].ToString();
        string fromDate = Session["FDate"].ToString();
        string toDate = Session["TDate"].ToString();

        FromTextBox.Text = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
        toTextBox.Text = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");

        DataTable mainDataTable = aReportBll.LoadSalesSummeryByPram(fromDate, toDate, branchId, comId).Copy();

        decimal value = 0;

        if (mainDataTable.Rows.Count > 0)
        {
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind();
        }

        //totalReceiveableLabel.Text = "TK. " + value.ToString(CultureInfo.InvariantCulture);

        // Report-related commented code – no issue
    }



    decimal totalOpeningDue = 0, totalSales = 0, totalReturn = 0, totalPayment = 0, totalAIT = 0, totalDiscount = 0, totalClosingDue = 0, totalSample = 0;

    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            totalOpeningDue += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OpeningDue") ?? 0);
            totalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SalesAmount") ?? 0);
            totalReturn += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ReturnAmount") ?? 0);
            totalPayment += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PaymentAmount") ?? 0);
            totalAIT += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AIT") ?? 0);
            totalDiscount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Discount") ?? 0);
            totalClosingDue += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ClosingDue") ?? 0);
            totalSample += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SampleExpense") ?? 0);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = "Total:";
            e.Row.Cells[4].Text = totalOpeningDue.ToString("N2");
            e.Row.Cells[5].Text = totalSales.ToString("N2");
            e.Row.Cells[6].Text = totalReturn.ToString("N2");
            e.Row.Cells[7].Text = totalPayment.ToString("N2");
            e.Row.Cells[8].Text = totalAIT.ToString("N2");
            e.Row.Cells[9].Text = totalDiscount.ToString("N2");
            e.Row.Cells[10].Text = totalClosingDue.ToString("N2");
            e.Row.Cells[11].Text = totalSample.ToString("N2");

            // Optional: Make footer bold
            e.Row.Font.Bold = true;
        }
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
        if (Session["UnitId"] == null || Session["ComId"] == null || Session["FDate"] == null || Session["TDate"] == null)
        {
            Response.Write("<script>alert('Session expired. Please reload the report.');</script>");
            return;
        }

        string branchId = Session["UnitId"].ToString();
        string comId = Session["ComId"].ToString();
        string fromDate = Session["FDate"].ToString();
        string toDate = Session["TDate"].ToString();

        FromTextBox.Text = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
        toTextBox.Text = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");

        DataTable mainDataTable = aReportBll.LoadSalesSummeryByPram(fromDate, toDate, branchId, comId).Copy();

        var ds = new DataSet();

        mainDataTable.TableName = "SalesSummeryForAccounts";
        ds.Tables.Add(mainDataTable);

        rptdoc.Load(ReportPath("rptSalesSummeryForAcc.rpt"));
        rptdoc.SetDataSource(ds);

        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
        "SalesrollSummery_Report");
    }
}