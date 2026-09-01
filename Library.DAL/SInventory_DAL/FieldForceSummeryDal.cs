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
    public class FieldForceSummeryDal
    {

        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public void LoadRegionName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            const string queryStr = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive ='True'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }
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

        public void LoadMIOInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string query = @"SELECT DISTINCT MIO.MIOId,TTR.TerritoryCode + ' : ' + EGI.EmpName AS MIOName FROM dbo.tblMIOInfo AS MIO
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                             INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId 
                             INNER JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId
                             LEFT JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARA.AreaId WHERE MIO.IsActive = 1 AND DCA.DCId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ") AND MIO.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "MIOName", "MIOId", query, "SSIDB");
        }

        public void GetAreaOnDropdownList(DropDownList ddl, string regionId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                WHERE AR.IsActive ='True' AND AR.RegionId = '" + regionId + "'";
            aCommonInternalDAL.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }

        public void GetTerritoryOnDropdownList(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT distinct TTR.TerritoryId, TTR.TerritoryCode + ' : ' + TTR.TerritoryName AS TerritoryName FROM tblTerritory AS TTR 
                                WHERE TTR.IsActive ='True' AND TTR.AreaId = '" + areaId + "'";
            aCommonInternalDAL.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
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

        
        public DataTable LoadWingsCollectionInfo(string parameter, string fromDate, string toDate)
        {
            string query = @"SELECT CI.CompanyName,CUNT.ComUnitCode,CUNT.ComUnitName,ISNULL(SUM(CPAYD.PaymentAmount),0) AS Collection,ISNULL(SUM(CPAYD.AIT),0) AS AIT,ISNULL(SUM(CPAYD.Discount),0) AS Discount,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM dbo.tblCustPayDetail AS CPAYD"
                             + " INNER JOIN dbo.tblCustomerPay AS CPAY ON CPAY.CustPayId = CPAYD.CustPayId"
                             + " LEFT JOIN dbo.tblInvoice AS INV ON CPAYD.InvoiceId = INV.InvoiceId"
                             + " LEFT JOIN dbo.tblCompanyUnit AS CUNT ON INV.ComUnitId = CUNT.ComUnitId"
                             + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON CPAY.CustomerMasterId = CSTMR.CustomerMasterId"
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON CUNT.CompanyId = CI.CompanyId " + parameter + " GROUP BY CI.CompanyName,CUNT.ComUnitCode,CUNT.ComUnitName";

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

            return aCommonInternalDAL.GetDataTableAction("sp_GET_WingsSalesReport", aSqlParameterlist, "SSIDB");
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
            aSqlParameters.Add(new SqlParameter("@StockOutType", aListMasterDao.StockOutType));

            return aCommonInternalDAL.UpdateAction("sp_I_DCStockOutInfo", aSqlParameters);
        }

        public DataTable GetDCStockOutInfo(string parameter)
        {
            string query = @"SELECT DISTINCT StockOutCode,StockOutDate,[EntryBy],[EntryDate],StockOutReason,StockOutType FROM tblDirectStockOut WHERE StockOutCode IS NOT NULL " + parameter;
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


        public DataTable GetUserCompanyById(string parameter)
        {
            string query = @"SELECT DISTINCT TOP 1 CompanyId FROM dbo.tblUserCompanyUnit AS UT
	LEFT JOIN tblCompanyUnit AS UNT ON UT.CompanyUnitId = UNT.ComUnitId WHERE UserId =" + parameter;
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GetFieldForceSalesSummery(string parameter, string fDate, string date)
        {
            string query = @"SELECT '" + fDate +"' as FromDate,'" + date +"' as ToDate,SLS.CompanyName,SLS.ComUnitName,Region,Area,Territory,RSM,ASM,MIO,SUM(OrderValue) AS OrderValue,SUM(ProformaValue) ProformaValue,"
+ " SUM(SalesValue) SalesValue,SUM(DeliveryDiscount) DeliveryDiscount,SUM(ReturnValue) ReturnValue,"
+ " CASE WHEN SUM(ReturnValue) > 0 THEN (CAST(((SUM(ReturnValue) * 100)/(SUM(SalesValue) + ISNULL(SUM(ReturnValue),0))) as decimal(16,2))) "
+ " ELSE CAST(0 as decimal(16,2)) END AS ReturnPercentage,SUM(OnDelivery) OnDelivery,SUM(PaymentAmount) PaymentAmount, "
+ " SUM(AIT) AIT, SUM(DiscountOnPayment) DiscountOnPayment,(SUM(DiscountOnPayment) + SUM(DeliveryDiscount)) AS TotalDiscount," 
+ " CASE WHEN (SUM(DiscountOnPayment) + SUM(DeliveryDiscount)) > 0 THEN (CAST((((SUM(DiscountOnPayment) + SUM(DeliveryDiscount)) * 100)/(SUM(SalesValue))) as decimal(16,2))) "
+ " ELSE CAST(0 as decimal(16,2)) END AS DiscountPercentage ,SUM(DueAmount) AS DueAmount,ISNULL(MiaTargetAmount,0) AS MiaTargetAmount,"
+ " CASE WHEN ISNULL(MiaTargetAmount,0) > 0 THEN CAST(((SUM(SalesValue) *100)/MiaTargetAmount) as decimal(16,2))  ELSE CAST(0 as decimal(16,2)) END AS Achivement  "
+ " FROM (SELECT CompanyName,CI.CompanyId,UNT.ComUnitId,Unt.ComUnitName,ODR.SubmissionDate,I.InvoiceDate,I.InvoiceNo,RGN.RegionId,(RGN.RegionCode +' : '+  RGN.RegionName) AS Region, "
+ " ARA.AreaId,ARA.AreaCode +' : '+  ARA.AreaName AS Area,TTR.TerritoryId,TTR.TerritoryCode+ ' : ' +TTR.TerritoryName  AS Territory "
+ " ,RSME.RSMId,RSM.EmpMasterCode +' : '+  RSM.EmpName AS RSM,ASME.ASMId, "
+ " ASM.EmpMasterCode +' : '+  ASM.EmpName AS ASM,MIOE.MIOId,MIO.EmpMasterCode +' : '+  MIO.EmpName AS MIO"
+ " ,ISNULL(OV.OrderValue,0) AS OrderValue,ISNULL(PV.ProformaValue,0) AS ProformaValue,"
+ " ISNULL(IV.InvoiceValue,0) AS SalesValue,ISNULL(DS.DiscountValue,0) AS DeliveryDiscount,ISNULL(ODV.OnDelivery,0) AS OnDelivery, "
+ " ISNULL(RV.ReturnValue,0) ReturnValue,ISNULL(PT.PaymentAmount,0) AS PaymentAmount,ISNULL(AT.AITAmount,0) AS AIT, ISNULL(DT.DiscountAmount,0) AS DiscountOnPayment,"
+ " (ISNULL(IV.InvoiceValue,0) - (ISNULL(PT.PaymentAmount,0) + ISNULL(AT.AITAmount,0) + ISNULL(DT.DiscountAmount,0))) AS DueAmount FROM tblInvoice AS I with (nolock)"
+ " LEFT JOIN tblCompanyUnit AS UNT with (nolock) ON I.ComUnitId = UNT.ComUnitId"
+ " LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
+ " LEFT JOIN tblOrder AS ODR with (nolock) ON I.OrderId = ODR.OrderId"
+ " LEFT JOIN tblRegion AS RGN with (nolock) ON ODR.RegionId = RGN.RegionId"
+ " LEFT JOIN tblArea AS ARA with (nolock) ON ODR.AreaId = ARA.AreaId "
+ " LEFT JOIN tblTerritory AS TTR with (nolock) ON ODR.TerritoryId  = TTR.TerritoryId"
+ " LEFT JOIN tblMIOInfo AS MIOE with (nolock) ON ODR.MIOId = MIOE.MIOId"
+ " LEFT JOIN tblEmpGeneralInfo AS MIO with (nolock) ON MIO.EmpInfoId = MIOE.EmployeeId "
+ " LEFT JOIN tblASMInfo AS ASME with (nolock) ON ODR.ASMId = ASME.ASMId"
+ " LEFT JOIN tblEmpGeneralInfo AS ASM with (nolock) ON ASM.EmpInfoId = ASME.EmployeeId"
+ " LEFT JOIN tblRSMInfo AS RSME with (nolock) ON ODR.RSMId = RSME.RSMId"
+ " LEFT JOIN tblEmpGeneralInfo AS RSM with (nolock) ON RSM.EmpInfoId = RSME.EmployeeId "
+ " LEFT JOIN (SELECT T.InvoiceId,SUM(ODRD.TotalTradePrice) OrderValue FROM tblInvoice AS T "
+ " LEFT JOIN tblInvoiceDetail AS TID ON T.InvoiceId = TID.InvoiceId "
+ " LEFT JOIN tblOrderDetail AS ODRD ON TID.OrderDetailsId = ODRD.OrderDetailId GROUP BY T.InvoiceId) AS OV ON I.InvoiceId = OV.InvoiceId"
+ " LEFT JOIN (SELECT T.InvoiceId,SUM(TID.TotalPrice) AS ProformaValue FROM tblInvoice AS T"
+ " LEFT JOIN tblInvoiceDetail AS TID ON T.InvoiceId = TID.InvoiceId"
+ " LEFT JOIN tblOrder AS TODR ON T.OrderId = TODR.OrderId GROUP BY T.InvoiceId) AS PV ON I.InvoiceId = PV.InvoiceId"
+ " LEFT JOIN (SELECT T.InvoiceId,SUM(TID.DeliveryNetAmount) InvoiceValue,SUM(TID.DeliveryTotalPriceVatAmount) InvoiceVat FROM tblInvoice AS T"
+ " LEFT JOIN tblInvoiceDetail AS TID ON T.InvoiceId = TID.InvoiceId"
+ " LEFT JOIN tblOrder AS TODR ON T.OrderId = TODR.OrderId WHERE DeliveryInvoiceStatus IN ('Full','Partial') GROUP BY T.InvoiceId) AS IV ON I.InvoiceId = IV.InvoiceId"
+ " LEFT JOIN (SELECT T.InvoiceId,SUM(TID.DeliveryDiscountAmount + TID.DelivarySpecialAmount) DiscountValue FROM tblInvoice AS T"
+ " LEFT JOIN tblInvoiceDetail AS TID ON T.InvoiceId = TID.InvoiceId"
+ " LEFT JOIN tblOrder AS TODR ON T.OrderId = TODR.OrderId WHERE DeliveryInvoiceStatus IN ('Full','Partial') GROUP BY T.InvoiceId) AS DS ON I.InvoiceId = DS.InvoiceId"
+ " LEFT JOIN (SELECT T.InvoiceId,SUM(TID.TotalPrice) OnDelivery FROM tblInvoice AS T"
+ " LEFT JOIN tblInvoiceDetail AS TID ON T.InvoiceId = TID.InvoiceId"
+ " LEFT JOIN tblOrder AS TODR ON T.OrderId = TODR.OrderId WHERE DeliveryInvoiceStatus IS NULL GROUP BY T.InvoiceId) AS ODV ON I.InvoiceId = ODV.InvoiceId"
+ " LEFT JOIN (SELECT T.InvoiceId,SUM(TID.TotalPrice-ISNULL(TID.DeliveryTotalPrice,0)) ReturnValue FROM tblInvoice AS T"
+ " LEFT JOIN tblInvoiceDetail AS TID ON T.InvoiceId = TID.InvoiceId"
+ " LEFT JOIN tblOrder AS TODR ON T.OrderId = TODR.OrderId WHERE DeliveryInvoiceStatus IN ('Reject','Partial') GROUP BY T.InvoiceId) AS RV ON I.InvoiceId = RV.InvoiceId"
+ " LEFT JOIN (SELECT CP.InvoiceId,SUM(CP.PaymentAmount) AS PaymentAmount FROM tblCustPayDetail AS CP GROUP BY CP.InvoiceId) AS PT ON I.InvoiceId = PT.InvoiceId"
+ " LEFT JOIN (SELECT CP.InvoiceId,SUM(ISNULL(CP.AIT,0)) AS AITAmount FROM tblCustPayDetail AS CP GROUP BY CP.InvoiceId) AS AT ON I.InvoiceId = AT.InvoiceId"
+ " LEFT JOIN (SELECT CP.InvoiceId,SUM(ISNULL(CP.Discount,0)) AS DiscountAmount FROM tblCustPayDetail AS CP GROUP BY CP.InvoiceId) AS DT ON I.InvoiceId = DT.InvoiceId) AS SLS"
+ " LEFT JOIN (SELECT TG.Period,TG.Year,TG.MiaId,MiaTargetAmount FROM tblMIATarget AS TG) AS TGD ON SLS.MIOId = TGD.MiaId "
+ " AND DATENAME(month,SLS.InvoiceDate) = TGD.Period AND DATENAME(year,SLS.InvoiceDate) = TGD.Year"
+ " WHERE SLS.InvoiceDate IS NOT NULL " + parameter + " GROUP BY SLS.CompanyName,SLS.ComUnitName,Region,Area,Territory,RSM,ASM,MIO,MiaTargetAmount ORDER BY SLS.CompanyName,SLS.ComUnitName,Region,Area,Territory,RSM,ASM,MIO";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
