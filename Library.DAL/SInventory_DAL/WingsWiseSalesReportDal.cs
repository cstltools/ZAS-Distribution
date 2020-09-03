using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class WingsWiseSalesReportDal
    {
        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public void LoadGroupInfo(DropDownList groupDropDownList)
        {
            string query = @"SELECT GroupId, GroupName FROM tblGroupInformation";
            aCommonInternalDAL.LoadDropDownValue(groupDropDownList, "GroupName", "GroupId", query, "SSIDB");
        }

        public void LoadCompanyInfo(DropDownList ddl, string groupId)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo WHERE GroupId = " + groupId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        public void LoadMIOInformation(DropDownList ddl, string companyId)
        {
            string query = @"SELECT MIO.MIOId,TTR.TerritoryCode + ':' + EGI.EmpName AS MIOName FROM dbo.tblMIOInfo AS MIO
                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                            INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId WHERE MIO.IsActive = 1 AND MIO.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "MIOName", "MIOId", query, "SSIDB");
        }

         public void LoadMIOInformationByUser(DropDownList ddl, string companyId,string userId)
        {
            string query = @"SELECT DISTINCT MIO.MIOId,TTR.TerritoryCode + ' : ' + EGI.EmpName AS MIOName FROM dbo.tblMIOInfo AS MIO
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                             INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId 
                             INNER JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId
                             LEFT JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARA.AreaId WHERE MIO.IsActive = 1 AND DCA.DCId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId +") AND MIO.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "MIOName", "MIOId", query, "SSIDB");
        }

        public DataTable LoadCustomerPaymentInfo(string parameter)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpMasterCode AS MIOCode,
                             EGI.EmpName AS MIOName, INV.InvoiceNo, INV.InvoiceDate,CP.PayType,INV.PaymentStatus,CPD.PaymentAmount,CP.PaymentDate FROM dbo.tblCustomerPay AS CP WITH (nolock)
                             INNER JOIN dbo.tblCustPayDetail AS CPD ON CPD.CustPayId = CP.CustPayId
                             INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId
                             INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
                             INNER JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = CP.CustomerMasterId
                             WHERE DeliveryTpGrandTotal > 0 AND CPD.InvoiceId NOT IN (SELECT CRM.InvoiceId FROM dbo.tblCustomerReturnMaster AS CRM) " + parameter + " ORDER BY CSTMR.CustomerCode ";
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable LoadSalesReportInfo(string parameter)
        {
            string query = @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,INV.OrderNo,CONVERT(VARCHAR,INV.OrderDate,103) as OrderDate,INV.InvoiceNo,
                            CONVERT(VARCHAR,INV.InvoiceDate,103)  InvoiceDate,INV.DelivaryInvoiceNo, CONVERT(VARCHAR,INV.UpdateDate,103) UpdateDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.DeliveryQuantity,DeliveryNetAmount,
                            DeliveryTotalPriceVatAmount,DeliveryDiscountAmount,ID.DelivarySpecialAmount,C.AreaCode, C.AreaName, C.RegionCode, C.RegionName,C.TerritoryCode, C.TerritoryName, C.MarketCode, C.MarketName, REGI.EmpName AS RSM,AEGI.EmpName AS ASM,MEGI.EmpName AS MIO   FROM tblInvoice AS INV 
                            INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = INV.InvoiceId 
                            INNER JOIN dbo.tblOrder AS ORD ON ORD.OrderId = INV.OrderId
                            INNER JOIN dbo.tblRSMInfo AS RSM ON RSM.RSMId = ORD.RSMId
                            INNER JOIN dbo.tblEmpGeneralInfo AS REGI ON RSM.EmployeeId = REGI.EmpInfoId
                            INNER JOIN dbo.tblASMInfo AS ASM ON ASM.ASMId = ORD.ASMId
                            INNER JOIN dbo.tblEmpGeneralInfo AS AEGI ON ASM.EmployeeId = AEGI.EmpInfoId
                            INNER JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = ORD.MIOId
                            INNER JOIN dbo.tblEmpGeneralInfo AS MEGI ON MIO.EmployeeId = MEGI.EmpInfoId
                            INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = INV.ComUnitId
                            INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = INV.CustomerMasterId
                            INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId 
                            WHERE ID.DeliveryStatus IN ('Full','Partial')" + parameter;
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


        public void LoadDcInfoByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");
        }

        public void GetWhInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        //public DataTable LoadWingsSalesInfo(string parameter)
        //{
//            string query = @"SELECT CI.CompanyId,CI.CompanyName,UNT.ComUnitName AS Depot,IV.InvoiceValue,SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) AS ActualSales,
//                             ISNULL(TNZ.InTransit, 0) AS InTransit,ISNULL(RJT.Reject, 0) AS Reject
//                             FROM dbo.tblInvoice AS INV 
//                             LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
//                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
//                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
//                             LEFT JOIN (SELECT  CI.CompanyId,CI.CompanyName,UNT.ComUnitName AS Depot,SUM(INVD.NetAmount) InTransit  FROM dbo.tblInvoice AS INV 
//                             LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
//                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
//                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId 
//                             WHERE INV.DeliveryInvoiceStatus IS NULL " + parameter +" GROUP BY CI.CompanyId,CI.CompanyName,UNT.ComUnitName) AS TNZ ON CI.CompanyId = TNZ.CompanyId "
//                             + " LEFT JOIN (SELECT  CI.CompanyId,CI.CompanyName,UNT.ComUnitName AS Depot,ISNULL(SUM(INVD.NetAmount),0)-ISNULL(SUM(CASE WHEN INVD.DeliveryNetAmount < 0 THEN 0 ELSE INVD.DeliveryNetAmount END),0) Reject  FROM dbo.tblInvoice AS INV  "
//                             + " LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId "
//                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
//                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
//                             + " WHERE INV.DeliveryInvoiceStatus IN ('Reject','Partial') " + parameter + " GROUP BY CI.CompanyId,CI.CompanyName,UNT.ComUnitName) AS RJT ON CI.CompanyId = RJT.CompanyId "
//                             + " LEFT JOIN (SELECT  CI.CompanyId,CI.CompanyName,UNT.ComUnitName AS Depot,SUM(INVD.NetAmount) AS InvoiceValue  FROM dbo.tblInvoice AS INV  "
//                             + " LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId "
//                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
//                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId  "
//                             + " WHERE INV.InvoiceNo IS NOT NULL " + parameter + " GROUP BY CI.CompanyId,CI.CompanyName,UNT.ComUnitName) AS IV ON CI.CompanyId = IV.CompanyId "
//                             + " WHERE INV.DeliveryInvoiceStatus IN ('Full','Partial') " + parameter
//                             + " GROUP BY CI.CompanyId,CI.CompanyName,UNT.ComUnitName,TNZ.InTransit,RJT.Reject,IV.InvoiceValue";
        //    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //    aSqlParameterlist.Add(new SqlParameter("@BankId", aBankBranchInformationDAO.BankId));

        //    return aCommonInternalDAL.GetDataTableAction("sp_GET_WingsSalesReport", "SSIDB");
        //}

        public DataTable LoadWingsCollectionInfo(string parameter,string fromDate,string toDate)
        {
            string query = @"SELECT CI.CompanyName,CUNT.ComUnitCode,CUNT.ComUnitName,ISNULL(SUM(CPAYD.PaymentAmount),0) AS Collection,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM dbo.tblCustPayDetail AS CPAYD"
                             +" INNER JOIN dbo.tblCustomerPay AS CPAY ON CPAY.CustPayId = CPAYD.CustPayId"
                             +" LEFT JOIN dbo.tblInvoice AS INV ON CPAYD.InvoiceId = INV.InvoiceId"
                             +" LEFT JOIN dbo.tblCompanyUnit AS CUNT ON INV.ComUnitId = CUNT.ComUnitId"
                             + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON CPAY.CustomerMasterId = CSTMR.CustomerMasterId"
                             +" LEFT JOIN dbo.tblCompanyInfo AS CI ON CUNT.CompanyId = CI.CompanyId " + parameter + " GROUP BY CI.CompanyName,CUNT.ComUnitCode,CUNT.ComUnitName";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadWingsTransitInfo(string parameter)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,ISNULL(SUM(INV.TpGrandTotal),0) AS TotalSales,(0.00)AS  Collection,(0.00)AS  Transit FROM dbo.tblInvoice AS INV 
                            INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
                            LEFT JOIN dbo.tblOrder AS ODR ON ODR.OrderId = INV.OrderId
                            INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = ODR.ManufacId
                            INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
                            WHERE INV.DeliveryInvoiceStatus IS NULL " + parameter + " GROUP BY CI.CompanyName,CUNT.ComUnitCode,CUNT.ComUnitName";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadWingsSalesInfoByPram(string parameter, string fDate, string tdate)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@parameter", parameter));
            aSqlParameterlist.Add(new SqlParameter("@fDate", Convert.ToDateTime(fDate)));
            aSqlParameterlist.Add(new SqlParameter("@tdate", Convert.ToDateTime(tdate)));

            return aCommonInternalDAL.GetDataTableAction("sp_GET_WingsSalesReport",aSqlParameterlist, "SSIDB");
        }

        public DataTable LoadWingsSalesInfoByCompany(string comId, string fDate, string tdate)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@parameter", comId));
            aSqlParameterlist.Add(new SqlParameter("@fDate", Convert.ToDateTime(fDate)));
            aSqlParameterlist.Add(new SqlParameter("@tdate", Convert.ToDateTime(tdate)));

            return aCommonInternalDAL.GetDataTableAction("sp_GET_WingsSalesReportByCompany", aSqlParameterlist, "SSIDB");
        }

        public DataTable LoadWingsSalesInfoByUnit(string unitId, string fDate, string tdate)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@parameter", unitId));
            aSqlParameterlist.Add(new SqlParameter("@fDate", Convert.ToDateTime(fDate)));
            aSqlParameterlist.Add(new SqlParameter("@tdate", Convert.ToDateTime(tdate)));

            return aCommonInternalDAL.GetDataTableAction("sp_GET_WingsSalesReportByUnit", aSqlParameterlist, "SSIDB");
        }

        public DataTable GetDcStockInfo(string dcId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@DcId", dcId));

            return aCommonInternalDAL.GetDataTableAction("sp_GET_DCStockInfo", aSqlParameterlist, "SSIDB");
        }

        public bool SaveDirectStockOutInfo(DirectStockOutDao aListMasterDao)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@DCId", aListMasterDao.DCId));
            aSqlParameters.Add(new SqlParameter("@Code", aListMasterDao.Code));
            aSqlParameters.Add(new SqlParameter("@ProductCode", aListMasterDao.ProductCode));
            aSqlParameters.Add(new SqlParameter("@ProductName", aListMasterDao.ProductName));
            aSqlParameters.Add(new SqlParameter("@DCStoreId", aListMasterDao.DCStoreId));
            aSqlParameters.Add(new SqlParameter("@StockOutQty", aListMasterDao.StockOutQty));
            aSqlParameters.Add(new SqlParameter("@StockOutReason", aListMasterDao.StockOutReason));
            aSqlParameters.Add(new SqlParameter("@EntryBy", aListMasterDao.EntryBy));
            aSqlParameters.Add(new SqlParameter("@EntryDate", aListMasterDao.EntryDate));
            aSqlParameters.Add(new SqlParameter("@StockOutDate", aListMasterDao.StockOutDate));

            return aCommonInternalDAL.UpdateAction("sp_I_DCStockOutInfo", aSqlParameters);
        }

        public DataTable GetDCStockOutInfo(string parameter)
        {
            string query = @"SELECT DISTINCT StockOutCode,StockOutDate,[EntryBy],[EntryDate] FROM tblDirectStockOut WHERE StockOutCode IS NOT NULL " + parameter ;
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GenerateCode()
        {
            const string query = @"SELECT DISTINCT TOP 1 StockOutCode FROM tblDirectStockOut ORDER BY StockOutCode DESC";
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadSalesTranscationInfo(string parameter)
        {
            string query = @"SELECT CI.CompanyCode,CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,INV.InvoiceNo,INV.InvoiceDate,ISNULL(SUM(INVD.UnitPrice*INVD.Quantity),0) AS InvoiceValue,ISNULL(TNS.SalesInTransit,0) AS SalesInTransit,
		CAST(ISNULL(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*INVD.DeliveryQuantity)),0) AS DECIMAL (16,2))  AS ActualSales,
		CAST(ISNULL(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity))),0) AS DECIMAL (16,2))  AS ReturnValue,
		CAST( ISNULL(SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity),0) AS DECIMAL (16,2))  AS Discount FROM tblInvoice AS INV
		LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId 
		LEFT JOIN (
		SELECT SUM(IVD.UnitPrice*IVD.Quantity) AS SalesInTransit,IV.InvoiceNo FROM tblInvoice AS IV 
		LEFT JOIN dbo.tblInvoiceDetail AS IVD ON IVD.InvoiceId = IV.InvoiceId 
		WHERE IV.DeliveryInvoiceStatus IS NULL 
		GROUP BY InvoiceNo
		) AS TNS ON TNS.InvoiceNo = INV.InvoiceNo
		LEFT JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
		LEFT JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId " + parameter + " GROUP BY CI.CompanyCode,CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,INV.InvoiceNo,INV.InvoiceDate,INV.InvoiceNo,TNS.SalesInTransit " 
		+ " HAVING ISNULL(SUM(INVD.UnitPrice*INVD.Quantity),0) > 0 OR ISNULL(TNS.SalesInTransit,0) > 0 OR "
		+ " ISNULL(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*INVD.DeliveryQuantity)),0) > 0 OR "
		+ " ISNULL(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity))),0) > 0 OR"
		+ " ISNULL(SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity),0) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
