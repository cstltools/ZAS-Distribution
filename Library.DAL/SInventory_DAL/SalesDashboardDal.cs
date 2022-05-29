using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class SalesDashboardDal
    {
        private ClsCommonInternalDAL _aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetCompanyWiseSales(string companyId, string year, string month)
        {
            //string[] dateString = year.Split('|');

            //DateTime fromDate = Convert.ToDateTime(dateString[0]);
            //DateTime toDate = Convert.ToDateTime(dateString[1]);


            string query = @"SELECT INV.InvoiceDate,CONVERT(varchar, INV.InvoiceDate, 7)  + ' : '+ LEFT(DATENAME(DW,INV.InvoiceDate),3) AS InvoiceDate2 ,
                            SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) AS SalesValue, ISNULL(PM.Collection,0) Collection FROM  dbo.tblInvoice AS INV 
                            LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                            LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                            LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                            LEFT JOIN (SELECT PaymentDate,SUM(CPD.PaymentAmount) AS Collection FROM tblCustPayDetail AS CPD
                            LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId
                            LEFT JOIN tblInvoice AS INV ON CPD.InvoiceId = INV.InvoiceId
                            LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                            LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                            WHERE MONTH(PaymentDate) = " + month + " AND YEAR(PaymentDate) = '" + year + "' AND CI.CompanyId = " + companyId + " GROUP BY PaymentDate) AS PM ON INV.InvoiceDate = PM.PaymentDate WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0 AND CI.CompanyId = " + companyId + " AND MONTH(INV.InvoiceDate) = " + month + " AND YEAR(INV.InvoiceDate) = '" + year + "' GROUP BY InvoiceDate,ISNULL(PM.Collection,0) ORDER BY INV.InvoiceDate";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            
            
        }


        public DataTable GetCompanyWiseDcSales(string companyId, DateTime fromDate, DateTime toDate)
        {
            string query = @"SELECT CompanyName,UNT.ComUnitName AS UnitName,SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) AS SalesValue
	                         FROM dbo.tblInvoice AS INV 
	                         LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
	                         LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
	                         LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
	                         WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  
	                         AND INV.InvoiceDate BETWEEN '" + fromDate + "' AND '" + toDate + "' GROUP BY UNT.ComUnitName,CompanyName  ORDER BY CompanyName,UNT.ComUnitName";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetGroupSalesCompanyWise(DateTime fromDate, DateTime toDate)
        {
            string query = @"SELECT STUFF(CompanyName, 1, 4, '') CompanyName,SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) AS SalesValue
                             FROM dbo.tblInvoice AS INV 
                             LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  
                             AND MONTH(INV.InvoiceDate) = MONTH(GETDATE()) AND Year(INV.InvoiceDate) = Year(GETDATE()) GROUP BY CompanyName ORDER BY SalesValue DESC";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCompanyWiseProductSales(string companyId, string prameter)
        {
            string query = @"SELECT NTP.ProductCode + ':' + PD.ProductName AS Product,SUM(NTP.DeliveryQuantity) AS SalesQuantity,SUM(NTP.Value) AS SalesValue FROM tblProduct AS PD 
	                         LEFT JOIN (SELECT INVD.ProductCode,INVD.DeliveryQuantity,(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) Value FROM dbo.tblInvoice AS INV 
	                         LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId 
	                         WHERE INVD.DeliveryStatus IN ('Full','Partial') AND INVD.Quantity > 0 " + prameter + ") AS NTP ON NTP.ProductCode = PD.ProductCode WHERE PD.CompanyId = '" + companyId + "' GROUP BY NTP.ProductCode,PD.PackSize,PD.ProductName HAVING NTP.ProductCode IS NOT NULL ORDER BY SalesQuantity DESC";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetWingsSalesTrend(string year)
        {

            //string[] dateString = year.Split('|');

            //DateTime fromDate = Convert.ToDateTime(dateString[0]);
            //DateTime toDate = Convert.ToDateTime(dateString[1]);

            string query = @"SELECT STUFF(CompanyName, 1, 4, '') CompanyName,MONTH(INV.InvoiceDate) AS MonthNo,YEAR(INV.InvoiceDate) AS YearName ,
                             CAST(DATENAME(month, INV.InvoiceDate) AS CHAR(3)) AS SalesMonth,SUM(CASE WHEN INVD.DeliveryNetAmount > 0 
                             THEN  INVD.DeliveryNetAmount  ELSE 0 END) AS SalesValue
                             FROM dbo.tblInvoice AS INV 
                             LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  
                             AND YEAR(INV.InvoiceDate) = '" + year + "' GROUP BY CI.CompanyName,YEAR(INV.InvoiceDate),MONTH(INV.InvoiceDate),CAST(DATENAME(month, INV.InvoiceDate) AS CHAR(3))  ORDER BY YearName,MonthNo ";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetDueUpto(DateTime toDateTime, string companyId)
        {
            string query = @"SELECT SUM(DeliveryTpGrandTotal- (ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0))) AS DueAmount FROM dbo.tblInvoice AS INV 
LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  
LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
LEFT JOIN ( SELECT InvoiceId,SUM(CPD.PaymentAmount) AS PaymentAmount,SUM(CPD.AIT) AIT, SUM(CPD.Discount) Discount FROM tblCustPayDetail AS CPD  
LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId 
WHERE CP.PaymentDate <= '" + toDateTime + "' GROUP BY InvoiceId ) AS PMNT ON PMNT.InvoiceId = inv.InvoiceId LEFT JOIN dbo.View_CustomerMaster AS VC ON INV.CustomerMasterId = VC.CustomerMasterId WHERE DeliveryInvoiceStatus IN ('Full','Partial') AND INV.InvoiceDate <= '" + toDateTime + "'  AND CI.CompanyId = '" + companyId + "'  ";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetDashBoardCardInfo(string companyId,string fromDate, string toDate,DateTime currentDate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CurrentDate", currentDate));
            aSqlParameterList.Add(new SqlParameter("@CompanyId", companyId));
            aSqlParameterList.Add(new SqlParameter("@FromDate", fromDate == "" ?DateTime.Today : Convert.ToDateTime(fromDate)));
            aSqlParameterList.Add(new SqlParameter("@ToDate", toDate == "" ? DateTime.Today : Convert.ToDateTime(toDate)));

            return _aCommonInternalDal.GetDataTableAction("sp_GET_DashboardCardInfo", aSqlParameterList, "SSIDB");
        }

        public DataTable GetFinancialYear(string companyId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CompanyId", companyId));

            return _aCommonInternalDal.GetDataTableAction("sp_GET_FinancialYear", aSqlParameterList, "SSIDB");
        }

        public DataTable GetMIOInfo(string companyId)
        {
            string query = @"SELECT MIOId,EmpName FROM tblMIOInfo AS MIO 
                             LEFT JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             WHERE MIO.CompanyId = '" + companyId + "' AND IsActive = 1 ORDER BY EmpName";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCompanyInfo()
        {
            return _aCommonInternalDal.GetDataTableAction("sp_GET_CompanyInfo", "SSIDB");
        }

        public DataTable GetAglingReport(string parameter)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@Parameter", parameter));

            return _aCommonInternalDal.GetDataTableAction("sp_DSB_AglingReport", aSqlParameterList, "SSIDB");
        }


        public DataTable GetBusinesSummeryReport(string parameter1, string parameter2, string parameter3, string parameter4)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@Parameter1", parameter1));
            aSqlParameterList.Add(new SqlParameter("@Parameter2", parameter2));
            aSqlParameterList.Add(new SqlParameter("@Parameter3", parameter3));
            aSqlParameterList.Add(new SqlParameter("@Parameter4", parameter4));

            return _aCommonInternalDal.GetDataTableAction("sp_DSB_BusinessSummery", aSqlParameterList, "SSIDB");
        }

        public DataTable GetFinancialYearWithId(string companyId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CompanyId", companyId));

            return _aCommonInternalDal.GetDataTableAction("sp_GET_FinancialYearWithId", aSqlParameterList, "SSIDB");
        }
        public DataTable GetFinancialYearDate(string companyId, string finyearId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CompanyId", companyId));
            aSqlParameterList.Add(new SqlParameter("@FinYearId", finyearId));

            return _aCommonInternalDal.GetDataTableAction("sp_GET_FinancialYearDate", aSqlParameterList, "SSIDB");
        }

        public DataTable GetCashFlowBDTDAL(string FinancialYearID, string CompanyID, string fdate, string todate)
        {

            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@FinancialYearId", FinancialYearID));
            aSqlParameterlist.Add(new SqlParameter("@CompanyInfoId", CompanyID));
            aSqlParameterlist.Add(new SqlParameter("@fdate", fdate));
            aSqlParameterlist.Add(new SqlParameter("@todate", todate));
            DataTable dt = _aCommonInternalDal.GetDataTableAction("sp_GET_CashFlowSummaryBDTCurrency", aSqlParameterlist, "ACCDB");

            return dt;
        }

        public DataTable GetIncomeStatementMasterMap()
        {
            return _aCommonInternalDal.GetDataTableAction("sp_GET_IncomeStatementMapMaster", "ACCDB");
        }

        public DataTable GetIncomeStatementDetail(string masterId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@MasterId", masterId));

            return _aCommonInternalDal.GetDataTableAction("sp_GET_IncomeStatementMapDetailById", aSqlParameterlist, "ACCDB");
        }

        public DataTable GerIncomeStatementBalance(string pram, string pram2)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@Parameter", pram));
            aSqlParameterlist.Add(new SqlParameter("@Parameter2", pram2));

            return _aCommonInternalDal.GetDataTableAction("sp_GET_IncomeStatementBalance", aSqlParameterlist, "ACCDB");
        }

        public DataTable GetCompanyWiseExpireProduct(string companyId, Int32 expireIn)
        {
            string query = @"SELECT ProductCode,ProductName,PackSize,CONVERT(varchar, MfgDate, 7) MfgDate,CONVERT(varchar, ExpDate, 7) ExpDate,BatchNo,SUM(StockQty) AS StockQty FROM dbo.tblDCStore 
                             INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = tblDCStore.ComUnitId
                             WHERE ProductCode IS NOT NULL AND (GETDATE() BETWEEN DATEADD(MONTH, " + (expireIn * -1) + ", CAST(ExpDate AS DATETIME)) AND ExpDate) " +
                           " AND CompanyId = '" + companyId + "' GROUP BY ProductCode,ProductName,PackSize,MfgDate,ExpDate,BatchNo HAVING SUM(StockQty) > 0";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetTop50Customer(string companyId, DateTime fromDate, DateTime toDate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CompanyId", Convert.ToInt32(companyId)));
            aSqlParameterList.Add(new SqlParameter("@fromDate", fromDate));
            aSqlParameterList.Add(new SqlParameter("@toDate", toDate));


//            string query = @"SELECT TOP 50 MST.CustomerCode,MST.CustomerName,SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) ActualSales,ISNULL(DD.DUE,0) AS Due
//		FROM dbo.tblInvoice AS INV 
//		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
//		LEFT JOIN tblCustMaster AS MST ON INV.CustomerMasterId = MST.CustomerMasterId
//		LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
//		LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
//		LEFT JOIN (SELECT INV.CustomerMasterId,
//        SUM(DeliveryTpGrandTotal)- SUM((ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0))) AS DUE FROM dbo.tblInvoice AS INV 
//        LEFT JOIN ( SELECT InvoiceId,SUM(CPD.PaymentAmount) AS PaymentAmount,SUM(CPD.AIT) AIT, SUM(CPD.Discount) Discount FROM tblCustPayDetail AS CPD   
//        LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId  WHERE CP.PaymentDate <= '" + toDate + "' GROUP BY InvoiceId ) AS PMNT ON PMNT.InvoiceId = inv.InvoiceId " 
//        + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId "
//        + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
//        + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
//        + " LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId "
//        + " LEFT JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId "
//        + " LEFT JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId  WHERE (INV.DeliveryInvoiceStatus IS NOT NULL AND INV.DeliveryInvoiceStatus != 'Reject')  AND CI.CompanyId = " + companyId 
//        + " AND INV.InvoiceDate <= '" + toDate + "' GROUP BY INV.CustomerMasterId"
//        + " HAVING SUM(DeliveryTpGrandTotal)- SUM((ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0))) > 0 ) AS DD ON DD.CustomerMasterId = INV.CustomerMasterId"
//        + " WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') AND INVD.Quantity > 0  "
//        + " AND INV.InvoiceDate BETWEEN '" + fromDate + "' AND '" + toDate + "' AND CI.CompanyId = " + companyId 
//        + " GROUP BY MST.CustomerName, MST.CustomerCode,ISNULL(DD.DUE,0) ORDER BY SUM(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) DESC ";

            return _aCommonInternalDal.GetDataTableAction("sp_Dashboard_Top50Customer",aSqlParameterList,"SSIDB");
        }

        public DataTable GetTopPriorityProductsales(string companyId, string depotId, DateTime fromDate, DateTime toDate)
        {
            string query = "";

            if (depotId != null && depotId != "Select ..")
            {
                 query = @"SELECT INVD.ProductCode,INVD.ProductName,ISNULL(TGT.TargetQty,0) TargetQty,ISNULL(SUM(DeliveryTotalQuantity),0) AS SalesQty,
                CASE WHEN TGT.TargetQty IS NOT NULL THEN (ISNULL(SUM(DeliveryTotalQuantity),0) * 100) / NULLIF(TGT.TargetQty,0) 
                ELSE 0 END  AS Achivment,CASE WHEN MAX(InvoiceDate) IS NOT NULL THEN (ISNULL(DATEDIFF(DAY, '" + fromDate +
                               "', MAX(InvoiceDate)),0) * 100) / NULLIF(DATEDIFF(DAY, '" + fromDate + "', '" + toDate +
                               "'),0)"
                               + " ELSE 0 END  AS TimePass FROM tblInvoice AS INV "
                               + " LEFT JOIN tblInvoiceDetail AS INVD ON INV.invoiceId = INVD.InvoiceId "
                               + " LEFT JOIN tblOrder AS ODR ON ODR.OrderId = INV.OrderId "
                               + " LEFT JOIN tblRegion AS RGN ON ODR.RegionId = RGN.RegionId "
                               + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                               + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
                               +
                               " LEFT JOIN (SELECT RGN.RegionName,RSM.EmpName AS RSM,ProductCode,ProductName,SUM(TargetQty) TargetQty  FROM tblMIATargetProductWise AS MTP "
                               + " LEFT JOIN tblProduct AS PD ON MTP.ProductId = PD.ProductId "
                               + " LEFT JOIN tblMIOInfo AS MIO ON MTP.MiaId = MIO.MIOId "
                               + " LEFT JOIN tblTerritory AS TTR ON MIO.TerritoryId = TTR.TerritoryId "
                               + " LEFT JOIN tblArea AS ARA ON ARA.AreaId = TTR.AreaId "
                               + " LEFT JOIN tblRegion AS RGN ON RGN.RegionId = ARA.RegionId "
                               + " LEFT JOIN (SELECT EGI.EmpName,RegionId FROM tblRSMInfo AS RSM "
                               + " LEFT JOIN tblEmpGeneralInfo AS EGI ON RSM.EmployeeId = EGI.EmpInfoId "
                               + " WHERE RSM.CompanyId = " + companyId +
                               ") AS RSM ON RSM.RegionId = RGN.RegionId WHERE MTP.Year = DATENAME(year,'" + fromDate +
                               "') "
                               + " AND (MTP.Period = DATENAME(month,'" + fromDate + "'))"
                               +
                               " GROUP BY RGN.RegionName,RSM.EmpName,ProductCode,ProductName) AS TGT ON INVD.ProductCode = TGT.ProductCode AND RGN.RegionName = TGT.RegionName"
                               + " WHERE DeliveryInvoiceStatus IN ('Full','Partial') AND INV.InvoiceDate BETWEEN '" +
                               fromDate + "' AND '" + toDate + "' AND UNT.ComUnitId = " + depotId
                               + " AND CI.CompanyId = " + companyId +
                               " GROUP BY INVD.ProductCode,INVD.ProductName,TGT.TargetQty  ORDER BY ProductCode";
            }
            else
            {
                 query = @"SELECT INVD.ProductCode,INVD.ProductName,ISNULL(TGT.TargetQty,0) TargetQty,ISNULL(SUM(DeliveryTotalQuantity),0) AS SalesQty,
                CASE WHEN TGT.TargetQty IS NOT NULL THEN (ISNULL(SUM(DeliveryTotalQuantity),0) * 100) / NULLIF(TGT.TargetQty,0) 
                ELSE 0 END  AS Achivment,CASE WHEN MAX(InvoiceDate) IS NOT NULL THEN (ISNULL(DATEDIFF(DAY, '" + fromDate +
                              "', MAX(InvoiceDate)),0) * 100) / NULLIF(DATEDIFF(DAY, '" + fromDate + "', '" + toDate +
                              "'),0)"
                              + " ELSE 0 END  AS TimePass FROM tblInvoice AS INV "
                              + " LEFT JOIN tblInvoiceDetail AS INVD ON INV.invoiceId = INVD.InvoiceId "
                              + " LEFT JOIN tblOrder AS ODR ON ODR.OrderId = INV.OrderId "
                              + " LEFT JOIN tblRegion AS RGN ON ODR.RegionId = RGN.RegionId "
                              + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                              + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
                              +
                              " LEFT JOIN (SELECT RGN.RegionName,RSM.EmpName AS RSM,ProductCode,ProductName,SUM(TargetQty) TargetQty  FROM tblMIATargetProductWise AS MTP "
                              + " LEFT JOIN tblProduct AS PD ON MTP.ProductId = PD.ProductId "
                              + " LEFT JOIN tblMIOInfo AS MIO ON MTP.MiaId = MIO.MIOId "
                              + " LEFT JOIN tblTerritory AS TTR ON MIO.TerritoryId = TTR.TerritoryId "
                              + " LEFT JOIN tblArea AS ARA ON ARA.AreaId = TTR.AreaId "
                              + " LEFT JOIN tblRegion AS RGN ON RGN.RegionId = ARA.RegionId "
                              + " LEFT JOIN (SELECT EGI.EmpName,RegionId FROM tblRSMInfo AS RSM "
                              + " LEFT JOIN tblEmpGeneralInfo AS EGI ON RSM.EmployeeId = EGI.EmpInfoId "
                              + " WHERE RSM.CompanyId = " + companyId +
                              ") AS RSM ON RSM.RegionId = RGN.RegionId WHERE MTP.Year = DATENAME(year,'" + fromDate +
                              "') "
                              + " AND (MTP.Period = DATENAME(month,'" + fromDate + "'))"
                              +
                              " GROUP BY RGN.RegionName,RSM.EmpName,ProductCode,ProductName) AS TGT ON INVD.ProductCode = TGT.ProductCode AND RGN.RegionName = TGT.RegionName"
                              + " WHERE DeliveryInvoiceStatus IN ('Full','Partial') AND INV.InvoiceDate BETWEEN '" +
                              fromDate + "' AND '" + toDate + "' AND CI.CompanyId = " + companyId +
                              " GROUP BY INVD.ProductCode,INVD.ProductName,TGT.TargetQty  ORDER BY ProductCode";
            }
            

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetRegion()
        {
            string query = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive ='True'";
            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetDepot(string companyId)
        {
            string query = "SELECT ComUnitId,ComUnitCode + ' : ' + ComUnitName AS UnitName FROM tblCompanyUnit AS UNT WHERE UNT.CompanyId = " + companyId;
            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GetCompanyInvoice(string companyId, DateTime fromDate, DateTime toDate)
        {
            string query = @"SELECT UNT.CompanyId,CONVERT(varchar, INV.InvoiceDate, 7) InvoiceDate,COUNT(InvoiceNo) AS NoOfInvoice FROM tblInvoice AS INV 
                             LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             WHERE InvoiceDate BETWEEN '" + fromDate + "' AND '" + toDate + "' AND UNT.CompanyId = " + companyId + " GROUP BY UNT.CompanyId,INV.InvoiceDate ORDER BY InvoiceDate";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetMioWiseInvoice(string companyId, DateTime fromDate, DateTime toDate, string mioId)
        {
            string query = @"SELECT UNT.CompanyId,MIO.MIOId,EmpName,CONVERT(varchar, INV.InvoiceDate, 7) InvoiceDate,COUNT(InvoiceNo) AS NoOfInvoice,COUNT(DelivaryInvoiceNo) AS NoOfDelivaryInvoice FROM tblInvoice AS INV 
                             LEFT JOIN tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId
                             LEFT JOIN tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             WHERE InvoiceDate BETWEEN '" + fromDate + "' AND '" + toDate + "' AND UNT.CompanyId = " + companyId + " AND MIO.MIOId = " + mioId + " GROUP BY UNT.CompanyId,MIO.MIOId,EmpName,InvoiceDate ORDER BY InvoiceDate";

            return _aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetDashBoardStockCardInfo(string companyId, string fromDate, string toDate, DateTime today)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CompanyId", companyId));
            aSqlParameterList.Add(new SqlParameter("@FromDate", fromDate == "" ? DateTime.Today : Convert.ToDateTime(fromDate)));
            aSqlParameterList.Add(new SqlParameter("@ToDate", toDate == "" ? DateTime.Today : Convert.ToDateTime(toDate)));

            return _aCommonInternalDal.GetDataTableAction("sp_NationalBinCardPrice", aSqlParameterList, "SSIDB");
        }
    }
}
