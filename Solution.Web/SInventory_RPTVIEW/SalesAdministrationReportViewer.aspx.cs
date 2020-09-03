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

public partial class SInventory_RPTVIEW_SalesAdministrationReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    SalesAdministrationReportBll administrationBll = new SalesAdministrationReportBll();

    protected void Page_Init(object sender, EventArgs e)
    {
        string rptType = Request.QueryString["rptType"];
        string reportType = Request.QueryString["reportType"];

        if (rptType != "")
        {
            if (rptType == "ASM")
            {
                var asmDataTable = new DataTable();
                string parameter = Session["PRAM"].ToString();
                asmDataTable = administrationBll.LoadASMInformation(parameter).Copy();

                var ds = new DataSet();

                if (asmDataTable.Rows.Count > 0)
                {
                    asmDataTable.TableName = "ASMInformationTable";
                    ds.Tables.Add(asmDataTable);

                    if (reportType == "crp") 
                    {
                        rptdoc.Load(ReportPath("ASMReport.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                    }

                    if (reportType == "exl") 
                    {
                        rptdoc.Load(ReportPath("ASMReportExcel.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "ASM_Report");
                    }
                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }

            if (rptType == "RSM")
            {
                var rsmDataTable = new DataTable();
                string parameter = Session["PRAM"].ToString();
                rsmDataTable = administrationBll.LoadRSMInformation(parameter).Copy();

                var ds = new DataSet();

                if (rsmDataTable.Rows.Count > 0)
                {
                    rsmDataTable.TableName = "RSMInformationTable";
                    ds.Tables.Add(rsmDataTable);

                    if (reportType == "crp") 
                    {
                        rptdoc.Load(ReportPath("crpRSMInformation.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                    }

                    if (reportType == "exl") 
                    {
                        rptdoc.Load(ReportPath("crpRSMInformationExcel.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "RSM_Report");
                    }
                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }

            if (rptType == "MIO")
            {
                var mioDataTable = new DataTable();
                string parameter = Session["PRAM"].ToString();
                mioDataTable = administrationBll.LoadMIOInformation(parameter).Copy();

                var ds = new DataSet();

                if (mioDataTable.Rows.Count > 0)
                {
                    mioDataTable.TableName = "MIOInformationTable";
                    ds.Tables.Add(mioDataTable);

                    if (reportType == "crp") 
                    {
                        rptdoc.Load(ReportPath("MIOReport.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                    
                    }
                    if (reportType == "exl") 
                    {
                        rptdoc.Load(ReportPath("MIOReportExcel.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "MIO_Report");
                    }
                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
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