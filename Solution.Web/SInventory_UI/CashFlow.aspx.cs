using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;
using Microsoft.VisualBasic;

public partial class SInventory_UI_Dashboard : System.Web.UI.Page
{
    static SalesDashboardDal aDashboardDal = new SalesDashboardDal();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)
    {

        List<CompanyWiseSalesDao> aList = new List<CompanyWiseSalesDao>();


        DataTable aTable = aDashboardDal.GetCompanyWiseSales(companyId, year, month);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new CompanyWiseSalesDao();

                DataTable dueTable = aDashboardDal.GetDueUpto(Convert.ToDateTime(DR["InvoiceDate"]), companyId);

                aData.DueValue = dueTable.Rows.Count > 0 ? dueTable.Rows[0].Field<Decimal>("DueAmount") : Convert.ToDecimal(0);

                aData.InvoiceDate = DR["InvoiceDate2"].ToString();                
                aData.SalesValue = Convert.ToDecimal(DR["SalesValue"].ToString());
                aData.CollectionValue = Convert.ToDecimal(DR["Collection"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }


    [WebMethod(EnableSession = true)]
    public static FinancialYearDao[] GetFinancialYearDate(string companyId, string finyearId)
    {

        List<FinancialYearDao> aList = new List<FinancialYearDao>();


        DataTable aTable = aDashboardDal.GetFinancialYearDate(companyId, finyearId);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow dr in aTable.Rows)
            {
                var aData = new FinancialYearDao();

                aData.FinancialYearId = dr["FinancialYearId"].ToString();
                aData.StartDate = Convert.ToDateTime(dr["StartDate"].ToString()).ToString("yyyy-MM-dd");
                aData.EndDate = Convert.ToDateTime(dr["EndDate"].ToString()).ToString("yyyy-MM-dd");

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }
    [WebMethod(EnableSession = true)]
    public static CashFlowDao[] GetCashflowData(string companyId, string finyearId, string startdate, string enddate)
    {

        List<CashFlowDao> aList = new List<CashFlowDao>();


        DataTable aTable = aDashboardDal.GetCashFlowBDTDAL(finyearId,companyId,startdate,enddate);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow dr in aTable.Rows)
            {
                var aData = new CashFlowDao();

                
                aData.StartDate = Convert.ToDateTime(dr["StartDate"].ToString()).ToString("d");
                aData.EndDate = Convert.ToDateTime(dr["EndDate"].ToString()).ToString("d");
                aData.SubsidiryLCode = (dr["SubsidiryLCode"].ToString());
                aData.SubsidiryLName = (dr["SubsidiryLName"].ToString());
                aData.AccountCode = (dr["AccountCode"].ToString());
                aData.AccountName = (dr["AccountName"].ToString());
                aData.AmountBDT = Convert.ToDecimal((dr["AmountBDT"].ToString()));
                aData.CompanyName = (dr["CompanyName"].ToString());
                aData.Category = (dr["Category"].ToString());
                aData.FinancialYearDesc = (dr["FinancialYearDesc"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }

    [WebMethod(EnableSession = true)]
    public static CompanyWiseUnitSalesDao[] GetCompanyWiseDcSales(string companyId, string fromDate, string toDate)
    {

        List<CompanyWiseUnitSalesDao> aList = new List<CompanyWiseUnitSalesDao>();


        DataTable aTable = aDashboardDal.GetCompanyWiseDcSales(companyId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new CompanyWiseUnitSalesDao();

                aData.CompanyName = DR["CompanyName"].ToString();
                aData.UnitName = DR["UnitName"].ToString();
                aData.SalesValue = Convert.ToDecimal(DR["SalesValue"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }

    


    [WebMethod(EnableSession = true)]
    public static GroupSalesCompanyWiseDao[] GetGroupSalesCompanyWise(string fromDate, string toDate)
    {

        List<GroupSalesCompanyWiseDao> aList = new List<GroupSalesCompanyWiseDao>();


        DataTable aTable = aDashboardDal.GetGroupSalesCompanyWise(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new GroupSalesCompanyWiseDao();

                aData.CompanyName = DR["CompanyName"].ToString();
                aData.SalesValue = Convert.ToDecimal(DR["SalesValue"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }

    [WebMethod(EnableSession = true)]
    public static WingsSalesTrend[] GetWingsSalesTrend(string year)
    {

        List<WingsSalesTrend> aList = new List<WingsSalesTrend>();


        DataTable aTable = aDashboardDal.GetWingsSalesTrend(year);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new WingsSalesTrend();

                aData.MonthName = DR["SalesMonth"].ToString();
                aData.CompanyName = DR["CompanyName"].ToString();
                aData.SalesValue = Convert.ToDecimal(DR["SalesValue"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }



    [WebMethod(EnableSession = true)]
    public static SalesDashboardDao[] GetDashBoardCardInfo(string companyId)
    {

        var aList = new List<SalesDashboardDao>();

        string fromDate ="";
        string toDate ="";


        DataTable aTable = aDashboardDal.GetDashBoardCardInfo(companyId, fromDate, toDate, Convert.ToDateTime("2020-09-1 00:00:00.000"));

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new SalesDashboardDao();

                aData.NoOfOrder = Convert.ToDecimal(DR["NoOfOrder"].ToString());
                aData.NoOfInvoice = Convert.ToDecimal(DR["NoOfInvoice"].ToString());
                aData.DeliveryConfirmed = Convert.ToDecimal(DR["DeliveryConfirmed"].ToString());
                aData.ActualSales = Convert.ToDecimal(DR["ActualSales"].ToString());
                aData.TotalCollection = Convert.ToDecimal(DR["TotalCollection"].ToString());
                aData.TotalDue = Convert.ToDecimal(DR["TotalDue"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }


    [WebMethod(EnableSession = true)]
    public static FinancialYearDao[] GetFinancialYear(string companyId)
    {

        List<FinancialYearDao> aList = new List<FinancialYearDao>();


        DataTable aTable = aDashboardDal.GetFinancialYearWithId(companyId);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow dr in aTable.Rows)
            {
                var aData = new FinancialYearDao();

                aData.FinancialYearId = dr["FinancialYearId"].ToString();
                aData.FinancialYear = dr["FinancialYear"].ToString();

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }


    [WebMethod(EnableSession = true)]
    public static CompanyDao[] GetCompanyInfo()
    {

        List<CompanyDao> aList = new List<CompanyDao>();


        DataTable aTable = aDashboardDal.GetCompanyInfo();

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new CompanyDao();

                aData.CompanyId = Convert.ToInt32(DR["CompanyId"].ToString());
                aData.CompanyName = DR["CompanyName"].ToString();

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }

    [WebMethod(EnableSession = true)]
    public static AglingReportDao[] GetAglingReport(string companyId, string timePeriod, string year, string month)
    {

        string parameter = "";


        if (companyId != 0.ToString())
        {
            parameter = parameter + " AND CI.CompanyId = " + companyId;
        }

        if (timePeriod == "Month")
        {
            parameter = parameter + " AND YEAR(INV.InvoiceDate) = '" + year + "' AND MONTH(INV.InvoiceDate) = '" + month + "'";
        }
        else
        {
            parameter = parameter + " AND INV.InvoiceDate < GETDATE()";
        }

        var aList = new List<AglingReportDao>();


        DataTable aTable = aDashboardDal.GetAglingReport(parameter);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow dr in aTable.Rows)
            {
                var aData = new AglingReportDao();

                aData.RegionName = dr["RegionName"].ToString();
                aData.Oneto10 = Convert.ToDecimal(dr["D10"].ToString());
                aData.Tento20 = Convert.ToDecimal(dr["D20"].ToString());
                aData.Twentyto30 = Convert.ToDecimal(dr["D30"].ToString());
                aData.Thirtyto40 = Convert.ToDecimal(dr["D40"].ToString());
                aData.Fortyto50 = Convert.ToDecimal(dr["D50"].ToString());
                aData.Fiftyto60 = Convert.ToDecimal(dr["D60"].ToString());
                aData.SixtyPlus = Convert.ToDecimal(dr["D61"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }
}