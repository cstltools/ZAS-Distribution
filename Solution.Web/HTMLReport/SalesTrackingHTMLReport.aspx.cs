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
    ReportDocument rptdoc = new ReportDocument();
    NationalSalesProductWiseDal aReportDal = new NationalSalesProductWiseDal();

    protected void Page_Init(object sender, EventArgs e)
    {
       

        string parameter = Session["PRAM"].ToString();
        DataTable mainDataTable = aReportDal.GetSalesTrackingReport(parameter).Copy();


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
        string parameter = Session["PRAM"].ToString();
        DataTable mainDataTable = aReportDal.GetSalesTrackingReport(parameter).Copy();

        var ds = new DataSet();

        mainDataTable.TableName = "SalesTrackingData";
        ds.Tables.Add(mainDataTable);

        rptdoc.Load(ReportPath("crpSalesTrackingReport.rpt"));
        rptdoc.SetDataSource(ds);

        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
        "SalesTrackingReport");
    }
}