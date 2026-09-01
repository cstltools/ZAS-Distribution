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
    ReportDocument rptdoc = new ReportDocument();
    UserMenuPermissionDal aReportDal = new UserMenuPermissionDal();

    protected void Page_Init(object sender, EventArgs e)
    {        
        var asmDataTable = new DataTable();

        string pram = Session["Pram"].ToString();

        string parameter = "";

       
        asmDataTable = aReportDal.LoadMenuPermissionList(parameter).Copy();
        
        var ds = new DataSet();

        if (asmDataTable.Rows.Count > 0)
        {
            asmDataTable.TableName = "DepositListDataTable";
            ds.Tables.Add(asmDataTable);


            rptdoc.Load(ReportPath("crpUserMenuPermission.rpt"));
            rptdoc.SetDataSource(ds);
            crvCustMasterRpt.ReportSource = rptdoc;
            crvCustMasterRpt.DataBind();
            
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