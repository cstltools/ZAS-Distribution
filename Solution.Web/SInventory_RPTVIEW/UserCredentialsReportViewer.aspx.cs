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

public partial class SInventory_RPTVIEW_WingsWiseSalesReportViewer : System.Web.UI.Page
{
    StockTransportOrderReportBLL aOrderReportBll = new StockTransportOrderReportBLL();
    ReportDocument rptdoc = new ReportDocument();
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();
    UserCredentialChangeDal aDal = new UserCredentialChangeDal();

    protected void Page_Init(object sender, EventArgs e)
    {        
        var asmDataTable = new DataTable();

        string FDate = Session["FDate"].ToString();
        string TDate = Session["TDate"].ToString();
        string UserId = Session["UserId"].ToString();

        string parameter = "";

        if (FDate != "" && TDate != "")
        {
            parameter = parameter + " AND USRL.UpdateDate BETWEEN '" + FDate + "' AND '" + TDate + "'";
        }

        if (UserId != "")
        {
            parameter = parameter + " AND UR.UserId = '" + UserId + "'";
        }

        

        DataTable companyInfoDataTable = aOrderReportBll.CompanyInfoBLL().Copy();
        asmDataTable = aDal.GetUserCredentialLog(parameter).Copy();

        //asmDataTable = aReportDal.LoadWingsSalesInfo(parameter).Copy();
        

        var ds = new DataSet();

        if (asmDataTable.Rows.Count > 0)
        {
            asmDataTable.TableName = "CredentialDataTable";
            ds.Tables.Add(asmDataTable);

            companyInfoDataTable.TableName = "companyInfoDataTable";
            ds.Tables.Add(companyInfoDataTable);

            rptdoc.Load(ReportPath("crpUserCredentialLog.rpt"));
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