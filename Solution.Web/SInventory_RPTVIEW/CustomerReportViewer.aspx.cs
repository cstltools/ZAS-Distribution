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

public partial class SInventory_RPTVIEW_CustomerReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    CustomerReportDal aReportDal = new CustomerReportDal();

    protected void Page_Init(object sender, EventArgs e)
    {
        string parameter = Session["PRAM"].ToString();
        DataTable areaDataTable = aReportDal.LoadCustomerInfo(parameter).Copy();

        var ds = new DataSet();

        if (areaDataTable.Rows.Count > 0)
        {
            areaDataTable.TableName = "CustomerDataSet";
            ds.Tables.Add(areaDataTable);

            rptdoc.Load(ReportPath("crpCustomerMasterReport.rpt"));
            rptdoc.SetDataSource(ds);

            //crvCustMasterRpt.ReportSource = rptdoc;
            //crvCustMasterRpt.DataBind();

            rptdoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true,"CustomerMasterReport - ( Date: " + DateTime.Now + ")");

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

