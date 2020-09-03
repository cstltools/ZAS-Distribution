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
using Library.DAL.SInventory_DAL;

public partial class HTMLReport_ProformaHTMLReport : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    NationalSalesProductWiseDal aReportDal = new NationalSalesProductWiseDal();

    protected void Page_Init(object sender, EventArgs e)
    {
        string ComId = Session["ComId"].ToString();
        string FDate = Session["FDate"].ToString();
        string TDate = Session["TDate"].ToString();

        FromTextBox.Text = Convert.ToDateTime(FDate).ToString("dd-MMM-yyyy");
        toTextBox.Text = Convert.ToDateTime(TDate).ToString("dd-MMM-yyyy");

        string parameter = Session["PRAM"].ToString();


        DataTable mainDataTable = aReportDal.GetProductWiseNationalSales(FDate,TDate,ComId).Copy();

        decimal value = 0;


        if (mainDataTable.Rows.Count > 0)
        {
            loadGridView.DataSource = mainDataTable;
            loadGridView.DataBind();


            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                value = value + Convert.ToDecimal(loadGridView.Rows[i].Cells[4].Text.Trim());
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
        string ComId = Session["ComId"].ToString();
        string FDate = Session["FDate"].ToString();
        string TDate = Session["TDate"].ToString();

        string parameter = Session["PRAM"].ToString();

        DataTable mainDataTable = aReportDal.GetProductWiseNationalSales(FDate, TDate, ComId).Copy();

        var ds = new DataSet();

        mainDataTable.TableName = "NationalSalesProductWise";
        ds.Tables.Add(mainDataTable);

        rptdoc.Load(ReportPath("crpNationalSalesProductWise.rpt"));
        rptdoc.SetDataSource(ds);

        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
        "National Sales Report Product Wise");
    }
}