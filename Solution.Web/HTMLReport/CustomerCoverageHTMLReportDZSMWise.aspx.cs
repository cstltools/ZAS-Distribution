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

public partial class HTMLReport_CustomerCoverageHTMLReportDZSMWise : System.Web.UI.Page
{

    CustomerCoverageReportBll aCoverageReportBll = new CustomerCoverageReportBll();
    ReportDocument rptdoc = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string fromDate = Request.QueryString["fromDate"];
            string toDate = Request.QueryString["toDate"];

            DataTable mainDataTable = aCoverageReportBll.LoadDZSMWiseCustomerCoverageInfo(fromDate, toDate);

            if (mainDataTable.Rows.Count > 0)
            {
                loadGridView.DataSource = mainDataTable;
                loadGridView.DataBind();
            }

            else
            {
                messageLabel.Text = "No Data Found!!!";
            }
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        string fromDate = Request.QueryString["fromDate"];
        string toDate = Request.QueryString["toDate"];

        DataTable mainDataTable = aCoverageReportBll.LoadDZSMWiseCustomerCoverageInfo(fromDate, toDate).Copy();
        DataSet Ds = new DataSet();

        if (mainDataTable.Rows.Count > 0)
        {
            mainDataTable.TableName = "MiaWiseCustomerCoverageDataTable";
            Ds.Tables.Add(mainDataTable);

            rptdoc.Load(ReportPath("crpMiaWiseCustomerCoverage.rpt"));
            rptdoc.SetDataSource(Ds);

            rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true,
               "MiaWiseCustomerCoverage_Report");
        }

    }

    private string ReportPath(string rptName)
    {
        return Convert.ToString(Server.MapPath("~\\Reports\\CrystalReports\\" + rptName));
    }
}