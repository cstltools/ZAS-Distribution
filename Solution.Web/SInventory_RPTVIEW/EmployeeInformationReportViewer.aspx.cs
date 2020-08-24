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

public partial class SInventory_RPTVIEW_EmployeeInformationReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    EmployeeInformationBLL aEmployeeInformationBLL = new EmployeeInformationBLL();

    protected void Page_Init(object sender, EventArgs e)
    {
        string rptType = Request.QueryString["rptType"];
        string reportType = Request.QueryString["reportType"];


        if (rptType != "")
        {
            var allDataTable = new DataTable();
            string parameter = Session["PRAM"].ToString();

            if (rptType == "All")
            {
                allDataTable = null;
                allDataTable = aEmployeeInformationBLL.LoadAllEmployeeInformation(parameter).Copy();

                var ds = new DataSet();

                if (allDataTable.Rows.Count > 0)
                {
                    allDataTable.TableName = "AllEmployeeInformationTable";
                    ds.Tables.Add(allDataTable);

                    if (reportType == "crp")
                    {
                        rptdoc.Load(ReportPath("crpEemployeeInformation.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                    }
                   if(reportType=="exl")
                    {
                        rptdoc.Load(ReportPath("crpEemployeeInformationExcel.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "EmployeeInfo_Report");
                    }
                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }

            if (rptType == "CompanyWise")
            {
                allDataTable = null;
                allDataTable = aEmployeeInformationBLL.LoadAllEmployeeInformation(parameter).Copy();

                var ds = new DataSet();

                if (allDataTable.Rows.Count > 0)
                {
                    allDataTable.TableName = "AllEmployeeInformationTable";
                    ds.Tables.Add(allDataTable);

                   
                    if (reportType == "crp")
                    {
                        rptdoc.Load(ReportPath("crpEemployeeInformation.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                    }
                    if (reportType == "exl") 
                    {
                        rptdoc.Load(ReportPath("crpEemployeeInformationExcel.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "EmployeeInfo_Report");
                    }
                }
                else
                {
                    MessageLabel.Text = "No Data Found!!";
                }
            }

            if (rptType == "DeliveryPerson")
            {
                allDataTable = null;
                allDataTable = aEmployeeInformationBLL.LoadDeliveryPerson(parameter).Copy();
                var ds = new DataSet();

                if (allDataTable.Rows.Count > 0)
                {
                    allDataTable.TableName = "DeliveryPersonTable";
                    ds.Tables.Add(allDataTable);

                    if (reportType == "crp") 
                    {
                        rptdoc.Load(ReportPath("crpDeliveryPerson.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                    }

                    if (reportType == "exl")
                    {
                        rptdoc.Load(ReportPath("crpDeliveryPersonExcel.rpt"));
                        rptdoc.SetDataSource(ds);
                        crvCustMasterRpt.ReportSource = rptdoc;
                        crvCustMasterRpt.DataBind();
                        rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "DeliveryPerson_Report");
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