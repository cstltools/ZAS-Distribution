using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using Library.BLL.PettyCash_BLL;

public partial class SInventory_RPTVIEW_CreditReportAllViewer : System.Web.UI.Page
{
    


    
    ReportDocument rptdoc = new ReportDocument();
    //   EmployeeInformationBLL aEmployeeInformationBLL = new EmployeeInformationBLL();
    PettyCashBLL aCashBll = new PettyCashBLL();

    protected void Page_Init(object sender, EventArgs e)
    {


        var allDataTable = new DataTable();


        allDataTable = null;
        allDataTable = aCashBll.LoadCreditCustomerreportAll().Copy();

        var ds = new DataSet();

        if (allDataTable.Rows.Count > 0)
        {
            allDataTable.TableName = "CreditCustomerTable";
            ds.Tables.Add(allDataTable);

            rptdoc.Load(ReportPath("crpCreditCustomerReport.rpt"));
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

    protected void crvCustMasterRpt_Unload(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCreditCustomerRpt.Dispose();
    }
    protected void crvCustMasterRpt_Disposed(object sender, EventArgs e)
    {
        rptdoc.Close();
        rptdoc.Dispose();
        crvCreditCustomerRpt.Dispose();
    }



    protected void closeButton_Click(object sender, EventArgs e)
    {

    }


}