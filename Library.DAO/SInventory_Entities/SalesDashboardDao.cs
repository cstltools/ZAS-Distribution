using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class SalesDashboardDao
    {
        public decimal NoOfOrder { get; set; }
        public decimal NoOfInvoice { get; set; }
        public decimal DeliveryConfirmed { get; set; }
        public decimal ActualSales { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal TotalDue { get; set; }
        public decimal StockValue { get; set; }
    }


    public class CompanyWiseSalesDao
    {
        public string InvoiceDate { get; set; }
        public decimal SalesValue { get; set; }
        public decimal CollectionValue { get; set; }
        public decimal DueValue { get; set; }
    }

    public class MioWiseInvoiceDao
    {
        public string InvoiceDate { get; set; }
        public decimal NoOfInvoice { get; set; }
        public decimal NoOfDelivaryInvoice { get; set; }
    }

    public class CompanyWiseInvoiceDao
    {
        public string InvoiceDate { get; set; }
        public decimal NoOfInvoice { get; set; }
    }


    public class CompanyWiseUnitSalesDao
    {
        public string CompanyName { get; set; }
        public string UnitName { get; set; }
        public decimal SalesValue { get; set; }
    }

    public class MIODao
    {
        public string MIOId { get; set; }
        public string EmpName { get; set; }
    }

    public class FinancialYearDao
    {
        public string FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class CompanyDao
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public class CompanyWiseProductSalesDao
    {
        public string ProductName { get; set; }
        public decimal SalesValue { get; set; }
        public decimal SalesQuantity { get; set; }
    }

    public class GroupSalesCompanyWiseDao
    {
        public string CompanyName { get; set; }
        public decimal SalesValue { get; set; }
    }
    
    public class RegionDao
    {
        public string RegionName { get; set; }
        public Int32 RegionId { get; set; }
    }

    public class DepotDao
    {
        public string UnitName { get; set; }
        public Int32 UnitId { get; set; }
    }

    public class WingsSalesTrend
    {
        public string MonthName { get; set; }
        public string CompanyName { get; set; }
        public decimal SalesValue { get; set; }
    }

    public class AglingReportDao
    {
        public string RegionName { get; set; }
        public decimal Oneto10 { get; set; }
        public decimal Tento20 { get; set; }
        public decimal Twentyto30 { get; set; }
        public decimal Thirtyto40 { get; set; }
        public decimal Fortyto50 { get; set; }
        public decimal Fiftyto60 { get; set; }
        public decimal SixtyPlus { get; set; }
    }

    public class BusinessSummeryReportDao
    {
        public string RegionName { get; set; }
        public decimal PreviousDue { get; set; }
        public decimal TodaysDue { get; set; }
        public decimal TotalDue { get; set; }
        public decimal PrevousDueCollection { get; set; }
        public decimal TodaysCollection { get; set; }
        public decimal TotalCollection { get; set; }
    }

    public class CashFlowDao
    {

        public string CompanyName { get; set; }
        public string FinancialYearDesc { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Category { get; set; }
        public string SubsidiryLCode { get; set; }
        public string SubsidiryLName { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal AmountBDT { get; set; }
    }


    public class IncomeSatementDao
    {

        public string ParticularName { get; set; }
        public string CssClass { get; set; }
        public decimal Value { get; set; }
        
    }
    public class TrialBalanceDao
    {
        public string GLName { get; set; }
        public string ControllName { get; set; }
        public string SubsidiryName { get; set; }
        public string SubSubsidiryName { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TBCompanyName { get; set; }
        public string TBFinancialYear { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal FinalBalance { get; set; }

    }


    public class ExpireProductDao
    {

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackSize { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string BatchNo { get; set; }
        public decimal StockQty { get; set; }

    }

    public class Top50CustomerDao
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal PrevActualSales { get; set; }
        public decimal ActualSales { get; set; }
        public decimal PrevMonthDue { get; set; }
        public decimal Due { get; set; }

    }

    public class TopPriorityProductSalesDao
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal TargetQty { get; set; }
        public decimal SalesQty { get; set; }
        public decimal Achivment { get; set; }
        public decimal TimePass { get; set; }

    }
}
