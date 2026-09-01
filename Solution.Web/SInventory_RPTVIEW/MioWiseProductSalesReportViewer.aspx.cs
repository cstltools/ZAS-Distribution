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
using Library.CrystalReports.SInventory_RPT;
using Library.DAL.SInventory_DAL;

public partial class SInventory_RPTVIEW_MioWiseProductSalesReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    MioWiseProductSalesDal aReportDal = new MioWiseProductSalesDal();

    protected void Page_Init(object sender, EventArgs e)
    {
        //string rptType = Request.QueryString["rptType"];
        //string reportType = Request.QueryString["reportType"];

        
        var asmDataTable = new DataTable();
        string parameter = Session["PRAM"].ToString();
        string parameter2 = Session["PRAM2"].ToString();
        string parameter3 = Session["PRAM3"].ToString();

        string parameter4 = Session["PRAM4"].ToString();
        Int32 comId = Convert.ToInt32(Session["COMID"].ToString());

        string fromDate = Session["FromDate"].ToString();
        string toDate = Session["ToDate"].ToString();

        DateTime currentDate = Convert.ToDateTime(fromDate);
        DateTime endDate = Convert.ToDateTime(toDate);

        List<string> monthNames = new List<string>();

        while (currentDate <= endDate)
        {
            string monthName = currentDate.ToString("MMMM", CultureInfo.InvariantCulture);

            if (!monthNames.Contains(monthName))
            {
                monthNames.Add(monthName);
            }
            currentDate = currentDate.AddMonths(1);
        }

        string monthNamePram = " AND Period IN ('";
        string monthNamePram2 = " AND TGRT.Period IN ('";

        foreach (var monthName in monthNames)
        {
            monthNamePram = monthNamePram + monthName + "','";
            monthNamePram2 = monthNamePram2 + monthName + "','";
        }

        monthNamePram = monthNamePram.Substring(0, monthNamePram.Length - 2) + ")";
        monthNamePram2 = monthNamePram2.Substring(0, monthNamePram2.Length - 2) + ")";

        asmDataTable = aReportDal.GetMioWiseProductSales(parameter, fromDate, toDate, parameter2, comId, parameter3, monthNamePram, monthNamePram2, parameter4).Copy();

        var ds = new DataSet();

        if (asmDataTable.Rows.Count > 0)
        {
            asmDataTable.TableName = "MioWiseProductSaleTable";
            ds.Tables.Add(asmDataTable);


            rptdoc.Load(ReportPath("crpMioWiseProductSales.rpt"));
            rptdoc.SetDataSource(ds);
            crvCustMasterRpt.ReportSource = rptdoc;
            crvCustMasterRpt.DataBind();
            

            //if (reportType == "exl") 
            //{
            //    rptdoc.Load(ReportPath("ASMReportExcel.rpt"));
            //    rptdoc.SetDataSource(ds);
            //    crvCustMasterRpt.ReportSource = rptdoc;
            //    crvCustMasterRpt.DataBind();
            //    rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "ASM_Report");
            //}
        }
        else
        {
            MessageLabel.Text = "No Data Found!!";
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