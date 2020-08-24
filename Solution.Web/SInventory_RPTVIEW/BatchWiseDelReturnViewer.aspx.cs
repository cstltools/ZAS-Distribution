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

public partial class SInventory_RPTVIEW_BatchWiseDelReturnViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    BatchWiseTopSheetBll aSheetBll = new BatchWiseTopSheetBll();

    protected void Page_Init(object sender, EventArgs e)
    {
        string rptType = Request.QueryString["rptType"];

        //int companyId = Convert.ToInt32(Session["CompanyId"]);
        int delManId = Convert.ToInt32(Session["DelManId"]);
        int batchId = Convert.ToInt32(Session["BatchId"]);
        DateTime invDate = Convert.ToDateTime(Session["InvoiceDateTextBox"]);

        //DataTable mainTable = aSheetBll.BatchWiseTopSheet(companyId, delManId, batchId, invDate).Copy();
        //DataTable detailTable = aSheetBll.BatchWiseTopSheetDetail(companyId, delManId, batchId, invDate).Copy();

        DataTable mainTable = aSheetBll.BatchWiseDeliveryReturnTopSheet(delManId, batchId, invDate).Copy();
        var ds = new DataSet();


        if (mainTable.Rows.Count > 0)
        {
            mainTable.TableName = "BatchWiseDeliveryReturnSheet";
            ds.Tables.Add(mainTable);

            rptdoc.Load(ReportPath("crpBatchWiseReturnTopSheet.rpt"));
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