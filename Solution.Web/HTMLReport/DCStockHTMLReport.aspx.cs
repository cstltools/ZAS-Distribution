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
using Library.BLL.SInventory_BLL;

public partial class HTMLReport_ProformaHTMLReport : System.Web.UI.Page
{
    AreaWiseMonthlyInventoryReportBll aReportBll = new AreaWiseMonthlyInventoryReportBll();
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

        DataTable mainDataTable = aReportBll.LoadAreaWiseMonthlyInventoryReport(fromDate, toDate, branchId,DC,comId).Copy();

        decimal value = 0;

        if (mainDataTable.Rows.Count > 0)
        {
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();

            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                value = value + Convert.ToDecimal(loadGridView.Rows[i].Cells[18].Text.Trim());
            }
        }
        else
        {
            loadGridView.DataSource = null;
            loadGridView.DataBind(); 
        }


        totalReceiveableLabel.Text = "TK. " + value.ToString(CultureInfo.InvariantCulture);

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
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];
        string DC = Request.QueryString["DC"];
        string comId = Request.QueryString["comId"];

        DataTable mainDataTable = aReportBll.LoadAreaWiseMonthlyInventoryReport(fromDate, toDate, branchId,DC,comId).Copy();

        var ds = new DataSet();

        mainDataTable.TableName = "AreaWiseMonthlyInventoryReportDataTable";
        ds.Tables.Add(mainDataTable);

        rptdoc.Load(ReportPath("rptAreaWiseMonthlyInventoryReport.rpt"));
        rptdoc.SetDataSource(ds);

        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
        "DCStock_Report");
    }
}