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

public partial class SInventory_RPTVIEW_B2BReceiveViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    ChalanBLL aRequisitionBll = new ChalanBLL();
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    ComUnitSalesReportBLL aComUnitSalesReportBll = new ComUnitSalesReportBLL();

    protected void Page_Init(object sender, EventArgs e)
    {
        string reqId = Request.QueryString["ChalanId"];
        DataTable dcDataTable = aRequisitionBll.DCStoreReport(reqId).Copy();

        var ds = new DataSet();


        if (dcDataTable.Rows.Count > 0)
        {
            dcDataTable.TableName = "B2BStockReceive";
            ds.Tables.Add(dcDataTable);

            rptdoc.Load(ReportPath("crpB2BStockReceive.rpt"));
            rptdoc.SetDataSource(ds);

            crvCustMasterRpt.ReportSource = rptdoc;
            crvCustMasterRpt.DataBind();
        }
        else
        {
            MessageLabel.Text = "No Data Found !!";
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
   
    protected void crvCustMasterRpt_Unload(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCustMasterRpt.Dispose();
    }
    protected void crvCustMasterRpt_Disposed(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCustMasterRpt.Dispose();
    }
}