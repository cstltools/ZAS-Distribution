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
    public class ManualOrderCreationDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void GetDdlCompany(DropDownList ddl)
        {
            const string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void GetDdlRegion(DropDownList ddl)
        {
            const string queryStr = "SELECT * FROM dbo.tblRegion";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster AS CSTMR
			 LEFT JOIN dbo.tblMarket AS MKT ON MKT.MarketId = CSTMR.MarketId
			 LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MKT.TerritoryId
			 LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.TerritoryId = TTR.TerritoryId
			 WHERE CSTMR.MarketId IS NOT NULL AND MIO.MIOId IS NOT NULL AND CSTMR.CustomerCode = '" + customerCode + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerInfo(string customerCode)
        {
            string query = @"SELECT * FROM tblCustMaster AS CSTMR 
                             INNER JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
                             INNER JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId
                             INNER JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId 
                             INNER JOIN dbo.tblRegion AS RGN ON ARA.RegionId = RGN.RegionId
                             WHERE CSTMR.MarketId IS NOT NULL AND CSTMR.CustomerCode = '" + customerCode + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void GetDdlArea(DropDownList ddl, string regionId)
        {
            string queryStr = "SELECT * FROM dbo.tblArea WHERE RegionId='" + regionId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }

        public void GetDdlTeritory(DropDownList ddl, string areaId)
        {
            string queryStr = "SELECT TTR.TerritoryId,TTR.TerritoryName FROM dbo.tblTerritory AS TTR WHERE TTR.AreaId = '" + areaId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
        }

        public void GetDdlDc(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT UNT.ComUnitId,(ComUnitCode+':'+ComUnitName) AS ComUnitName FROM dbo.tblDcWiseAreaInfo AS DCA 
                                INNER JOIN dbo.tblCompanyUnit AS UNT ON DCA.DCId = UNT.ComUnitId
                                WHERE DCA.AreaId = '" + areaId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);

        }

        public void GetDdlDcByUser(DropDownList ddl, string areaId, string userId)
        {
            string queryStr = @"SELECT UNT.ComUnitId,(ComUnitCode+':'+ComUnitName) AS ComUnitName FROM dbo.tblDcWiseAreaInfo AS DCA 
                                INNER JOIN dbo.tblCompanyUnit AS UNT ON DCA.DCId = UNT.ComUnitId
                                WHERE DCA.AreaId = " + areaId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);

        }

        public void GetDdlMarket(DropDownList ddl, string territoriId)
        {
            string queryStr = "SELECT MKT.MarketId, MKT.MarketName + ':' + MKT.MarketCode AS MarketName FROM tblMarket AS MKT WHERE MKT.TerritoryId =  '" + territoriId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }

        public void GetDdlMio(DropDownList ddl, string territoriId)
        {
            string queryStr = @"SELECT MIO.MIOId, (EGI.EmpName + ':' + EGI.EmpMasterCode) AS MioName FROM dbo.tblMIOInfo AS MIO
                                INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId WHERE MIO.TerritoryId = '" + territoriId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MioName", "MIOId", queryStr);
        }

        public DataTable GetCompanyByDc(string unitId)
        {
            string query = @"SELECT UNT.CompanyId FROM dbo.tblCompanyUnit AS UNT WHERE UNT.ComUnitId = '" + unitId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetMioByCompany(string companyId, string territoryId)
        {
            string query = @"SELECT MIO.MIOId FROM dbo.tblMIOInfo AS MIO WHERE MIO.IsActive = 1 AND MIO.CompanyId ='" + companyId + "' AND MIO.TerritoryId = '" + territoryId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public int OrderManualId()
        {
            DataTable aDataTable = new DataTable();

            string query = @"SELECT (isnull(MAX(SUBSTRING(OrderCode,5,15)),0)+1) as PKMaxNo FROM dbo.tblOrder WHERE IsManual='True' OR IsManual=1";
            return Convert.ToInt32(aInternalDal.DataContainerDataTable(query, "SSIDB").Rows[0][0].ToString());

        }

        public DataTable GetCustomerCreditLimit(string customerId)
        {
            DataTable aDataTable = new DataTable();

            string query = @"SELECT * FROM dbo.tblCustomerCreditLimit
            LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
            WHERE tblCustMaster.CustomerMasterId='" + customerId + "'";
            aDataTable = aInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }


        public DataTable GetCustomerUsedCreditLimit(string customerId)
        {
            DataTable aDataTable = new DataTable();

            string query = @"
            SELECT SUM(tbltemp.RemainAmount)Amount FROM (SELECT ISNULL(SUM(GrossValue),0)RemainAmount FROM dbo.tblOrder WHERE (IsInvoice IS NULL OR IsInvoice='0') AND CustomerMasterId='" + customerId + "' " +
            " UNION ALL " +
            " SELECT SUM(DeliveryTpGrandTotal-ISNULL(tbltemp.Amount,0))RemainAmount FROM dbo.tblInvoice " +
            " LEFT JOIN  " +
            " (SELECT InvoiceId,ISNULL(SUM(PaymentAmount),0)Amount FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp ON tbltemp.InvoiceId = tblInvoice.InvoiceId " +
            " WHERE CustomerMasterId='" + customerId + "' AND PaymentStatus<>'Full')AS tbltemp";
            aDataTable = aInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }

        public DataTable GetAsmInfo(string areeaId, string companyId)
        {
            string query = @"SELECT ASM.ASMId FROM dbo.tblASMInfo AS ASM WHERE ASM.IsActive = 1	AND ASM.AreaId = '" + areeaId + "'  AND ASM.CompanyId = '" + companyId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetRsmInfo(string areeaId, string companyId)
        {
            string query = @"SELECT RSM.RSMId FROM dbo.tblRSMInfo AS RSM WHERE RSM.IsActive = 1 AND	RSM.RegionId = '" + areeaId + "'  AND RSM.CompanyId = '" + companyId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public string GenerateAutoNumber(string table, string column, DateTime date)
        {
            try
            {
                string query = @"SELECT (((SUBSTRING((CONVERT(NVARCHAR(5),YEAR(GETDATE()))),3,2)+(CASE WHEN LEN(MONTH(GETDATE()))=1 
		                        THEN  '0'+CONVERT(NVARCHAR(5),MONTH(GETDATE())) ELSE CONVERT(NVARCHAR(5),MONTH(GETDATE())) END)+
		                        (CASE WHEN LEN(DAY(GETDATE()))=1 THEN  '0'+CONVERT(NVARCHAR(5),DAY(GETDATE())) ELSE CONVERT(NVARCHAR(5),DAY(GETDATE())) END)))+
		                        CONVERT(NVARCHAR(5),(ISNULL((MAX(CONVERT(INT,(SUBSTRING( " + column + " ,10,4))))),1000)+1))) as AutoNumber FROM  " + table + "  WHERE  CONVERT(NVARCHAR(11),EntryDate,106)= CONVERT(NVARCHAR(11),'" + (System.DateTime.Now).ToString("dd-MMM-yyyy").Replace("-", " ") + "',106)";
               
                DataTable dt = aInternalDal.DataContainerDataTable(query, "SSIDB");
                Decimal asd = Convert.ToDecimal(dt.Rows[0][0].ToString()); ;
                Decimal asd1 = asd + 1;

                return dt.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public int SaveOrderInformation(OrderInfoMaster aListMasterDao)
        {

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            //aSqlParameters.Add(new SqlParameter("@OrderNo", aListMasterDao.OrderCode));
            aSqlParameters.Add(new SqlParameter("@ComUnitId", aListMasterDao.ComUnitId));
            aSqlParameters.Add(new SqlParameter("@ComUnitName", aListMasterDao.ComUnitName));
            aSqlParameters.Add(new SqlParameter("@ComUnitCode", aListMasterDao.ComUnitCode));
            aSqlParameters.Add(new SqlParameter("@MIOCode", aListMasterDao.MIOCode));
                                                 
            aSqlParameters.Add(new SqlParameter("@MIOId", aListMasterDao.MIOId));
            aSqlParameters.Add(new SqlParameter("@MIOName", aListMasterDao.MIOName));
            aSqlParameters.Add(new SqlParameter("@ManufacId", aListMasterDao.ManufacId));
            aSqlParameters.Add(new SqlParameter("@CustomerCode", aListMasterDao.CustomerCode));
            aSqlParameters.Add(new SqlParameter("@CustomerName", aListMasterDao.CustomerName));
                                                 
            aSqlParameters.Add(new SqlParameter("@CustomerMasterId", aListMasterDao.CustomerMasterId));
            aSqlParameters.Add(new SqlParameter("@GrossValue", aListMasterDao.GrossValue));
            aSqlParameters.Add(new SqlParameter("@SubmissionDate", aListMasterDao.SubmissionDate));
            aSqlParameters.Add(new SqlParameter("@IsManual", aListMasterDao.IsManual));
            aSqlParameters.Add(new SqlParameter("@IsInvoice", aListMasterDao.IsInvoice));
                                                 
            aSqlParameters.Add(new SqlParameter("@RegionId", aListMasterDao.RegionId));
            aSqlParameters.Add(new SqlParameter("@RSMId", aListMasterDao.RSMId));
            aSqlParameters.Add(new SqlParameter("@AreaId", aListMasterDao.AreaId));
            aSqlParameters.Add(new SqlParameter("@ASMId", aListMasterDao.ASMId));
            aSqlParameters.Add(new SqlParameter("@TerritoryId", aListMasterDao.TerritoryId));
            aSqlParameters.Add(new SqlParameter("@IsSpecialApproval", aListMasterDao.IsSpecialApproval));
                                                 
            aSqlParameters.Add(new SqlParameter("@ActionStatus", aListMasterDao.ActionStatus));
            //aSqlParameters.Add(new SqlParameter("@IsDirect", aListMasterDao.IsDirect));
            aSqlParameters.Add(new SqlParameter("@ApprovedBy", aListMasterDao.ApprovedBy));
            aSqlParameters.Add(new SqlParameter("@ApprovedDate", aListMasterDao.ApprovedDate));
                                                 
            aSqlParameters.Add(new SqlParameter("@EntryBy", aListMasterDao.EntryBy));
            aSqlParameters.Add(new SqlParameter("@EntryDate", aListMasterDao.EntryDate));

            aSqlParameters.Add(new SqlParameter("@MarketId", aListMasterDao.MarketId));

            return aInternalDal.SaveAction("sp_I_OrderMaster", aSqlParameters, "@OrderId");


//            string insertQuery = @"INSERT
//                                       INTO dbo.tblOrder
//                                     ( OrderCode ,
//                                       ComUnitId ,
//                                       ComUnitCode ,
//                                       ComUnitName ,
//                                       MIOCode ,
//                                       MIOName ,
//                                       ManufacId ,
//                                       CustomerCode ,
//                                       CustomerName ,
//                                       GrossValue ,
//                                       SubmissionDate ,
//                                       IsManual,
//                                       IsInvoice,
//                                       RegionId,
//                                       AreaId,
//                                       TerritoryId,
//                                       MarketId,
//                                       CustomerMasterId,
//                                       RSMId,
//                                       ASMId,
//                                       MIOId,
//                                       EntryBy,
//                                       EntryDate,
//                                       ActionStatus,
//                                       IsSpecialApproval,
//                                       ApprovedBy,
//                                       ApprovedDate
//                                     ) VALUES  ( 
//
//                                       @OrderCode ,
//                                       @ComUnitId ,
//                                       @ComUnitCode ,
//                                       @ComUnitName ,
//                                       @MIOCode ,
//                                       @MIOName ,
//                                       @ManufacId ,
//                                       @CustomerCode ,
//                                       @CustomerName ,
//                                       @GrossValue ,
//                                       @SubmissionDate ,
//                                       @IsManual,
//                                       @IsInvoice,
//                                       @RegionId,
//                                       @AreaId,
//                                       @TerritoryId,
//                                       @MarketId,
//                                       @CustomerMasterId,
//                                       @RSMId,
//                                       @ASMId,
//                                       @MIOId,
//                                       @EntryBy,
//                                       @EntryDate,
//                                       @ActionStatus,
//                                       @IsSpecialApproval,
//                                       @ApprovedBy,
//                                       @ApprovedDate)";


            //return aInternalDal.SaveDataByInsertCommandById(insertQuery, aSqlParameters, "SSIDB");

            
        }

        public int SaveOrderDetailInfo(OrderInfoDetail aListMasterDao)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@ProductId", aListMasterDao.ProductId));
            aSqlParameters.Add(new SqlParameter("@ProductCode", aListMasterDao.ProductCode));
            aSqlParameters.Add(new SqlParameter("@ProductName", aListMasterDao.ProductName));
            aSqlParameters.Add(new SqlParameter("@Quantity", aListMasterDao.Quantity));
            aSqlParameters.Add(new SqlParameter("@TradePrice", aListMasterDao.TradePrice));

            aSqlParameters.Add(new SqlParameter("@TotalTradePrice", aListMasterDao.TotalTradePrice));
            aSqlParameters.Add(new SqlParameter("@OrderId", aListMasterDao.OrderId));


            string insertQuery = @"INSERT INTO dbo.tblOrderDetail
                                ( ProductId ,
                                  ProductCode ,
                                  ProductName ,
                                  Quantity ,
                                  TradePrice ,
                                  TotalTradePrice ,
                                  OrderId                                     
                                ) VALUES (                                     
                                  @ProductId ,
                                  @ProductCode ,
                                  @ProductName ,
                                  @Quantity ,
                                  @TradePrice ,
                                  @TotalTradePrice ,
                                  @OrderId 
                               )";

            return aInternalDal.SaveDataByInsertCommandById(insertQuery, aSqlParameters, "SSIDB");
        }

        public DataTable GetDataSaveStatus(int masterId)
        {
            string query = @"SELECT * FROM dbo.tblOrderDetail WHERE OrderId = " + masterId;
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DeleteOrderMasterInfo(int masterId)
        {
            string query = @"DELETE FROM dbo.tblOrder WHERE OrderId = " + masterId;
            return aInternalDal.DeleteDataByDeleteCommand(query);
        }

        public DataTable GetCustomerDueInfo(string customerId)
        {
            string query = @"SELECT ISNULL(SUM(DeliveryTpGrandTotal-ISNULL(PaymentAmount,0)),0) AS Due FROM dbo.tblInvoice WHERE (DeliveryInvoiceStatus IS NOT NULL OR DeliveryInvoiceStatus != 'Reject') AND CustomerMasterId = " + customerId;
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadInvoiceInfo(DropDownList ddl, string customerId, string dcId)
        {
            string queryStr =
                @"SELECT InvoiceId,(InvoiceNo + ' (' + CONVERT(NVARCHAR(50),tblInvoice.DeliveryTpGrandTotal))+ ' : ' + CONVERT(NVARCHAR(50),ISNULL(PaymentAmount,0)) + ')' AS InvoiceNo FROM dbo.tblInvoice WHERE CustomerMasterId = " +
                customerId + " AND Remarks = 'Due' AND DeliveryTpGrandTotal > 0 AND ComUnitId = " + dcId;
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "InvoiceNo", "InvoiceId", queryStr);
        }

        public bool SampleStockIssue(OrderInfoDetail aListMasterDao, int id)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@ProductCode", aListMasterDao.ProductCode));
            aSqlParameters.Add(new SqlParameter("@Quantity", aListMasterDao.Quantity));
            aSqlParameters.Add(new SqlParameter("@MasterId", aListMasterDao.OrderId));
            aSqlParameters.Add(new SqlParameter("@OrderDetailsId", id));
       
            return aInternalDal.UpdateAction("sp_UD_SampleStockIssue", aSqlParameters);
        }

        public DataTable GetIsVatApplicable(string customerId)
        {
            string query = @"SELECT IsVatApplicable FROM tblCustMaster WHERE IsActive = 1 AND CustomerMasterId = " + customerId;
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetOrderCode(int masterId)
        {
            string query = @"SELECT OrderCode FROM dbo.tblOrder WHERE OrderId = " + masterId;
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
