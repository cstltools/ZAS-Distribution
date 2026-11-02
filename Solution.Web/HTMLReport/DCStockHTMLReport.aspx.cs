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
            decimal salesValue = 0;
            foreach (DataRow row in mainDataTable.Rows)
            {
                value = value + Convert.ToDecimal(row["ClosingStockValue"]);
                salesValue = salesValue + Convert.ToDecimal(row["IssuedToSalesValue"]);
            }

            // footer totals sit directly under their columns
            foreach (DataControlField col in loadGridView.Columns)
            {
                var f = col as BoundField;
                if (f == null) continue;
                if (f.DataField == "ProductName") f.FooterText = "Total";
                else if (f.DataField == "IssuedToSalesValue") f.FooterText = salesValue.ToString("N2", CultureInfo.InvariantCulture);
                else if (f.DataField == "ClosingStockValue") f.FooterText = value.ToString("N2", CultureInfo.InvariantCulture);
            }

            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();
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
        // Export the on-screen grid (already bound in Page_Init) so Excel has every column the page shows.
        // The old Crystal .rpt had a fixed, outdated column set.
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=DCStock_Report.xls");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "";
        using (var sw = new System.IO.StringWriter())
        using (var hw = new HtmlTextWriter(sw))
        {
            loadGridView.RenderControl(hw);
            Response.Write("<html><head><meta charset='utf-8'></head><body>" + sw + "</body></html>");
        }
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control) { }
}