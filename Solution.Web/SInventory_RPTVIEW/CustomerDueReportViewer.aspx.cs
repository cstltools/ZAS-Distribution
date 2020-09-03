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
using Library.DAL.SInventory_DAL;

public partial class SInventory_RPTVIEW_CustomerDueReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    CustomerDueReportDAL aReportDal = new CustomerDueReportDAL();

    protected void Page_Init(object sender, EventArgs e)
    {
        //string rptType = Request.QueryString["rptType"];
        //string reportType = Request.QueryString["reportType"];

        
        var asmDataTable = new DataTable();
        string parameter = Session["PRAM"].ToString();
        string toDate = Session["TODT"].ToString();

        string rpt = Session["RPT"].ToString();

        if (rpt == "")
        {
            string companyId = Request.QueryString["company"];
            asmDataTable = aReportDal.LoadCustomerDueReport(parameter,companyId, toDate).Copy();

            var ds = new DataSet();

            if (asmDataTable.Rows.Count > 0)
            {
                asmDataTable.TableName = "CustomerDueReportData";
                ds.Tables.Add(asmDataTable);

                rptdoc.Load(ReportPath("crpCustomerDueReport.rpt"));
                rptdoc.SetDataSource(ds);

                crvCustMasterRpt.ReportSource = rptdoc;
                crvCustMasterRpt.DataBind();

                //rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
                //      "Sales_Report");

            }
            else
            {
                MessageLabel.Text = "No Data Found!!";
            }
        }


         if (rpt != "")
        {
            asmDataTable = aReportDal.LoadCustomerTotalDueReport(parameter,toDate).Copy();

            var ds = new DataSet();

            if (asmDataTable.Rows.Count > 0)
            {
                asmDataTable.TableName = "CustomerTotalDueReportData";
                ds.Tables.Add(asmDataTable);

                rptdoc.Load(ReportPath("crpCustomerTotalDueReport.rpt"));
                rptdoc.SetDataSource(ds);

                crvCustMasterRpt.ReportSource = rptdoc;
                crvCustMasterRpt.DataBind();

                //rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
                //      "Sales_Report");

            }
            else
            {
                MessageLabel.Text = "No Data Found!!";
            }
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