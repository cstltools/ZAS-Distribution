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
using Library.CrystalReports.SInventory_RPT;

public partial class SInventory_RPTVIEW_TopSheetReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    InvoiceBLL aInvoiceBll = new InvoiceBLL();

    protected void Page_Init(object sender, EventArgs e)
    {

        //int rpt = Convert.ToInt32(Session["ProformaTopSheet"]); 

        //string invNo = Server.UrlDecode(Request.QueryString["InvNo"]);
        //string invColl = aInvoiceBll.InvoiceNoCollectionFormate(invNo);

        int companyId = Convert.ToInt32(Session["CompanyId"]);
        int delManId = Convert.ToInt32(Session["DelManId"]);
        int batchId = Convert.ToInt32(Session["BatchId"]);
        DateTime invDate =  Convert.ToDateTime(Session["InvoiceDateTextBox"]) ;

        DataTable detailDataTable = aInvoiceBll.BatchWiseTopSheet(companyId, delManId, batchId, invDate).Copy();

        //DataTable mainDataTable = aInvoiceBll.InvoiceMainDataForReportBLL(invColl).Copy();
        //DataTable detailDataTable = aInvoiceBll.InvoiceDetailDataForReportBLL(companyId, delManId, batchId, invDate).Copy();
        //DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();


        DataSet Ds = new DataSet();

        detailDataTable.TableName = "BatchWiseTopSheet";
        //detailDataTable.TableName = "detailDataTable";
        //companyInfoDataTable.TableName = "companyInfoDataTable";
        //Ds.Tables.Add(mainDataTable);
        Ds.Tables.Add(detailDataTable);
        //Ds.Tables.Add(companyInfoDataTable);


        rptdoc.Load(ReportPath("rptTopSheet.rpt"));
        rptdoc.SetDataSource(Ds);

        crvInvoiceReport.ReportSource = rptdoc;
        crvInvoiceReport.DataBind();


        //if (rpt == 0)
        //{
            //aRptInvoiceForCustomer.SetDataSource(Ds);
            //crvInvoiceReport.ReportSource = aRptInvoiceForCustomer;

            //aRptInvoiceForCustomer.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true,
            //       "TopSheet-" + invNo);

        //    rptdoc.Load(ReportPath("rptTopSheet.rpt"));
        //    rptdoc.SetDataSource(Ds);

        //    crvInvoiceReport.ReportSource = rptdoc;
        //    crvInvoiceReport.DataBind();
        //}

        //aRptInvoiceForCustomer.SetDataSource(Ds);
        //crvInvoiceReport.ReportSource = aRptInvoiceForCustomer;

        //aRptInvoiceForCustomer.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true,
        //       "TopSheet-" + invNo);

        //if (rpt == 1)
        //{
        //    rptdoc.Load(ReportPath("rptTopSheet.rpt"));
        //    rptdoc.SetDataSource(Ds);


        //    //aRptInvoiceForCustomer.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true,
        //    //       "TopSheet-" + invNo);
        //    crvInvoiceReport.ReportSource = rptdoc;
        //    crvInvoiceReport.DataBind();
        //}

        
    }
    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }
   
    protected void crvInvoiceReport_Disposed(object sender, EventArgs e)
    {
        if (this.rptdoc != null)
        {
            rptdoc.Close();
            rptdoc.Dispose();
            crvInvoiceReport.Dispose();
        }
       
    }
    protected void crvInvoiceReport_Unload(object sender, EventArgs e)
    {
        if (this.rptdoc != null)
        {
            rptdoc.Close();
            rptdoc.Dispose();
            crvInvoiceReport.Dispose();
        }
       
    }
}