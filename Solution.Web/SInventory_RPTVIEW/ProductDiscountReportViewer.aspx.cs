using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using Library.BLL.PettyCash_BLL;

public partial class SInventory_RPTVIEW_ProductDiscountReportViewer : System.Web.UI.Page
{
    ReportDocument rptdoc = new ReportDocument();
    PettyCashBLL aCashBll = new PettyCashBLL();

    protected void Page_Init(object sender, EventArgs e)
    {


        var allDataTable = new DataTable();
          string parameter = Session["PRAM"].ToString();

        allDataTable = aCashBll.LoadProductDistountReport(parameter).Copy();

        var ds = new DataSet();

        if (allDataTable.Rows.Count > 0)
        {
            allDataTable.TableName = "ProductDiscountRPT";
            ds.Tables.Add(allDataTable);

            rptdoc.Load(ReportPath("crpProductDiscountReport.rpt"));
            rptdoc.SetDataSource(ds);
            crvCreditCustomerRpt.ReportSource = rptdoc;
            crvCreditCustomerRpt.DataBind();

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

    protected void crvProductDiscount_Unload(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCreditCustomerRpt.Dispose();
    }
    protected void crvProductDiscount_Disposed(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCreditCustomerRpt.Dispose();
    }

}