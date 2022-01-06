using System;
using System.Activities.Expressions;
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
    public static Top50CustomerDao[] GetTop50Customer(string companyId, string fromDate, string toDate)
    {

        List<Top50CustomerDao> aList = new List<Top50CustomerDao>();


        DateTime date = DateTime.Today;
        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        DataTable aTable = aDashboardDal.GetTop50Customer(companyId, fromDate == "" ? firstDayOfMonth : Convert.ToDateTime(fromDate), toDate == "" ? lastDayOfMonth : Convert.ToDateTime(toDate));

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new Top50CustomerDao();

                aData.CustomerCode = DR["CustomerCode"].ToString();
                aData.CustomerName = DR["CustomerName"].ToString();
                aData.ActualSales = Convert.ToDecimal(DR["ActualSales"].ToString());
                aData.Due = Convert.ToDecimal(DR["Due"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }


    [WebMethod(EnableSession = true)]
    public static TopPriorityProductSalesDao[] GetTopPriorityProductsales(string companyId, string regionId, string fromDate, string toDate)
    {

        List<TopPriorityProductSalesDao> aList = new List<TopPriorityProductSalesDao>();


        DateTime date = DateTime.Today;
        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        DataTable aTable = aDashboardDal.GetTopPriorityProductsales(companyId, regionId, fromDate == "" ? firstDayOfMonth : Convert.ToDateTime(fromDate), toDate == "" ? lastDayOfMonth : Convert.ToDateTime(toDate));

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new TopPriorityProductSalesDao();

                aData.ProductCode = DR["ProductCode"].ToString();
                aData.ProductName = DR["ProductName"].ToString();
                aData.TargetQty = Convert.ToDecimal(DR["TargetQty"].ToString());
                aData.SalesQty = Convert.ToDecimal(DR["SalesQty"].ToString());
                aData.Achivment = Convert.ToDecimal(DR["Achivment"].ToString());
                aData.TimePass = Convert.ToDecimal(DR["TimePass"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }
    
    
    [WebMethod(EnableSession = true)]
    public static ExpireProductDao[] GetCompanyWiseExpireProduct(string companyId, string expireIn)
    {

        List<ExpireProductDao> aList = new List<ExpireProductDao>();


        DataTable aTable = aDashboardDal.GetCompanyWiseExpireProduct(companyId, Convert.ToInt32(expireIn));

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new ExpireProductDao();

                aData.ProductCode = DR["ProductCode"].ToString();
                aData.ProductName = DR["ProductName"].ToString();
                aData.PackSize = DR["PackSize"].ToString();
                aData.MfgDate = DR["MfgDate"].ToString();
                aData.ExpDate = DR["ExpDate"].ToString();
                aData.BatchNo = DR["BatchNo"].ToString();
                aData.StockQty = Convert.ToDecimal(DR["StockQty"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }

    [WebMethod(EnableSession = true)]
    public static CompanyWiseProductSalesDao[] GetCompanyWiseProductSales(string companyId, string fromDate, string toDate)
    {

        List<CompanyWiseProductSalesDao> aList = new List<CompanyWiseProductSalesDao>();



        string pram = "";

        if (fromDate == "" || toDate == "")
        {
            pram = pram + " AND YEAR(INV.InvoiceDate) = YEAR('" + DateTime.Today +
                   "') AND MONTH(INV.InvoiceDate) = MONTH('" + DateTime.Today + "')";
        }
        else
        {
            pram = pram + "AND  INV.InvoiceDate BETWEEN '" + fromDate + "' AND '" + toDate + "'";
        }


        DataTable aTable = aDashboardDal.GetCompanyWiseProductSales(companyId, pram);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new CompanyWiseProductSalesDao();

                aData.ProductName = DR["Product"].ToString();
                aData.SalesValue = Convert.ToDecimal(DR["SalesValue"].ToString());
                aData.SalesQuantity = Convert.ToDecimal(DR["SalesQuantity"].ToString());

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
    public static RegionDao[] GetRegionInfo()
    {

        List<RegionDao> aList = new List<RegionDao>();


        DataTable aTable = aDashboardDal.GetRegion();

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow DR in aTable.Rows)
            {
                var aData = new RegionDao();

                aData.RegionName = DR["RegionName"].ToString();
                aData.RegionId = Convert.ToInt32(DR["RegionId"].ToString());

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


        DataTable aTable = aDashboardDal.GetDashBoardCardInfo(companyId, DateTime.Today);

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


        DataTable aTable = aDashboardDal.GetFinancialYear(companyId);

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


        if (timePeriod == "Day")
        {
            parameter = parameter + " AND INV.InvoiceDate < GETDATE()";
            
        }
        else
        {

            string parm = "";
            parm = parm + year + "-" + month + "-01 00:00:00.000";

            parameter = parameter + " AND INV.InvoiceDate < '" + parm + "'";
            
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

    [WebMethod(EnableSession = true)]
    public static BusinessSummeryReportDao[] GetBusinessSummeryReport(string companyId, string timePeriod, string year, string month)
    {

        string parameter1 = "";
        string parameter2 = "";
        string parameter3 = "";
        string parameter4 = "";

        if (companyId != 0.ToString())
        {
            parameter1 = parameter1 + " AND CI.CompanyId = " + companyId;
            parameter2 = parameter2 + " AND CI.CompanyId = " + companyId;
            parameter3 = parameter3 + " AND CI.CompanyId = " + companyId;
            parameter4 = parameter4 + " AND CI.CompanyId = " + companyId;
        }

        if (timePeriod == "Day")
        {
            parameter1 = parameter1 + " AND INV.InvoiceDate < GETDATE()";
            parameter2 = parameter2 + " AND INV.InvoiceDate = CAST( GETDATE() AS Date )";

            parameter3 = parameter3 + " AND PaymentDate = CAST( GETDATE() AS Date ) AND INV.InvoiceDate = CAST( GETDATE() AS Date )";
            parameter4 = parameter4 + " AND PaymentDate = CAST( GETDATE() AS Date ) AND INV.InvoiceDate < CAST( GETDATE() AS Date )";
        }
        else
        {

            string parm = "";
            parm = parm + year + "-" + month + "-01 00:00:00.000";

            parameter1 = parameter1 + " AND INV.InvoiceDate < '" + parm + "'";
            parameter2 = parameter2 + " AND MONTH(INV.InvoiceDate) = MONTH('" + parm + "') AND YEAR(INV.InvoiceDate) = YEAR('" + parm + "') ";

            parameter3 = parameter3 + " AND PaymentDate = CAST( GETDATE() AS Date ) AND MONTH(INV.InvoiceDate) = MONTH('" + parm + "') AND YEAR(INV.InvoiceDate) = YEAR('" + parm + "')";
            parameter4 = parameter4 + " AND PaymentDate = CAST( GETDATE() AS Date ) AND INV.InvoiceDate < '" + parm + "'";
        }


        

        var aList = new List<BusinessSummeryReportDao>();


        DataTable aTable = aDashboardDal.GetBusinesSummeryReport(parameter1,parameter2,parameter3,parameter4);

        if (aTable.Rows.Count > 0)
        {

            foreach (DataRow dr in aTable.Rows)
            {
                var aData = new BusinessSummeryReportDao();

                aData.RegionName = dr["RegionName"].ToString();
                aData.PreviousDue = Convert.ToDecimal(dr["PreviousDue"].ToString());
                aData.TodaysDue = Convert.ToDecimal(dr["TodaysDue"].ToString());
                aData.TotalDue = Convert.ToDecimal(dr["TotalDue"].ToString());
                aData.PrevousDueCollection = Convert.ToDecimal(dr["PrevousDueCollection"].ToString());
                aData.TodaysCollection = Convert.ToDecimal(dr["TodaysCollection"].ToString());
                aData.TotalCollection = Convert.ToDecimal(dr["TotalCollection"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }


        return aList.ToArray();

    }
}