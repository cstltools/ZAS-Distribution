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
    public class OrderListDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        ClsApprovalAction approvalAction = new ClsApprovalAction();

        public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        {
            approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        }
        public string LoadForApprovalConditionDAL(string pageName, string userName)
        {
            return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        }
        public bool SaveOrderMaster(OrderInfoMaster aListMasterDao)
        {
            string insertQuery = @"INSERT
                             INTO dbo.tblOrder
                                     ( OrderCode ,
                                       ComUnitId ,
                                       ComUnitCode ,
                                       ComUnitName ,
                                       MIOCode ,
                                       MIOName ,
                                       ManufacId ,
                                       CustomerCode ,
                                       CustomerName ,
                                       GrossValue ,
                                       SubmissionDate ,
                                       IsManual,IsInvoice,RegionId,AreaId,TerritoryId,MarketId,CustomerMasterId,RSMId,ASMId,MIOId,EntryBy,EntryDate,ActionStatus,IsSpecialApproval
                                     ) VALUES  ( 
                                    '" + aListMasterDao.OrderCode+"'," +
                                 "'"+aListMasterDao.ComUnitId+"',"+
                                 "'"+aListMasterDao.ComUnitCode+"',"+
                                 "'"+aListMasterDao.ComUnitName+"',"+
                                 "'"+aListMasterDao.MIOCode+"',"+
                                 "'"+aListMasterDao.MIOName+"',"+
                                 "'"+aListMasterDao.ManufacId+"',"+
                                 "'"+aListMasterDao.CustomerCode+"',"+
                                 "'"+aListMasterDao.CustomerName+"',"+
                                 "'"+aListMasterDao.GrossValue+"',"+
                                 "'"+aListMasterDao.SubmissionDate+"',"+
                                 "'" + aListMasterDao.IsManual + "','False','" + aListMasterDao.RegionId + "','" + aListMasterDao.AreaId + "','" + aListMasterDao.TerritoryId + "'" +
                                 ",'" + aListMasterDao.MarketId + "','" + aListMasterDao.CustomerMasterId + "','" + aListMasterDao.RSMId + "','" + aListMasterDao.ASMId + "','" + aListMasterDao.MIOId + "','" + aListMasterDao.EntryBy + "','" + aListMasterDao.EntryDate + "','Posted','" + aListMasterDao.IsSpecialApproval + "' " +

                                     ")";
                                        return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool UpdateOrderMaster(OrderInfoMaster aOrderInfoMaster)
        {
            string query = @"UPDATE tblOrder SET  ComUnitId='"+aOrderInfoMaster.ComUnitId+"' ,ComUnitCode='"+aOrderInfoMaster.ComUnitCode+"' ,ComUnitName='"+aOrderInfoMaster.ComUnitName+"' ,MIOCode='"+aOrderInfoMaster.MIOCode+"' ,MIOName='"+aOrderInfoMaster.MIOName+"' ,ManufacId='"+aOrderInfoMaster.ManufacId+"' ,CustomerCode='"+aOrderInfoMaster.CustomerCode+"' ,CustomerName='"+aOrderInfoMaster.CustomerName+"' ,GrossValue='"+aOrderInfoMaster.GrossValue+"' ,SubmissionDate='"+aOrderInfoMaster.SubmissionDate+"' ,"+
                                       " IsManual='" + aOrderInfoMaster.IsManual + "',IsInvoice='" + aOrderInfoMaster.IsInvoice + "',RegionId='" + aOrderInfoMaster.RegionId + "',AreaId='" + aOrderInfoMaster.AreaId + "',TerritoryId='" + aOrderInfoMaster.TerritoryId + "',MarketId='" + aOrderInfoMaster.MarketId + "',CustomerMasterId='" + aOrderInfoMaster.CustomerMasterId + "',RSMId='" + aOrderInfoMaster.RSMId + "',ASMId='" + aOrderInfoMaster.ASMId + "',MIOId='" + aOrderInfoMaster.MIOId + "',UpdateBy='" + aOrderInfoMaster.UpdateBy + "',UpdateDate='" + aOrderInfoMaster.UpdateDate + "' WHERE OrderId=" + aOrderInfoMaster.OrderId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool SaveOrderDetail(OrderInfoDetail aOrderListDetailDao)
        {
            string insertQuery = @"INSERT INTO dbo.tblOrderDetail
                                ( ProductId ,
                                  ProductCode ,
                                  ProductName ,
                                  Quantity ,
                                  TradePrice ,
                                  TotalTradePrice ,
                                  OrderId 
                                  
                                  
                                ) VALUES  ( 
                                    '"+aOrderListDetailDao.ProductId+"'," +
                                 "'"+aOrderListDetailDao.ProductCode+"',"+
                                 "'"+aOrderListDetailDao.ProductName+"',"+
                                 "'"+aOrderListDetailDao.Quantity+"',"+
                                 "'"+aOrderListDetailDao.TradePrice+"',"+
                                 "'"+aOrderListDetailDao.TotalTradePrice+"',"+
                                 "'"+aOrderListDetailDao.OrderId+"'"+
                                 
                                 
                                ")";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public void LoadmanufacturerName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }
        public void LoadRegion(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblRegion";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }
        public void LoadAreaByRegion(DropDownList ddl,string id)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblArea WHERE RegionId='"+id+"'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }
        public void LoadDCByArea(DropDownList ddl, string id)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT ComUnitId, (ComUnitCode+':'+ComUnitName) as Com FROM dbo.tblDcWiseAreaInfo
            INNER JOIN dbo.tblCompanyUnit ON dbo.tblDcWiseAreaInfo.DCId=dbo.tblCompanyUnit.ComUnitId 
            WHERE AreaId='" +id+"'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "Com", "ComUnitId", queryStr);
        }
        public DataTable CustomerInfo(string custCode)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM dbo.tblCustMaster
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId 
			LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
             WHERE CustomerCode='" + custCode + "'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable GetCompanybydc(string id)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM dbo.tblCompanyUnit WHERE ComUnitId='"+id+"'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable GetCustomerCreditLimit(string customerId)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM dbo.tblCustomerCreditLimit
            LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
            WHERE tblCustMaster.CustomerMasterId='"+customerId+"'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable GetCustomerUsedCreditLimit(string customerId)
        {
            DataTable aDataTable = new DataTable();
            string query = @"
            SELECT SUM(tbltemp.RemainAmount)Amount FROM (SELECT ISNULL(SUM(GrossValue),0)RemainAmount FROM dbo.tblOrder WHERE (IsInvoice IS NULL OR IsInvoice='0') AND CustomerMasterId='"+customerId+"' "+
            " UNION ALL "+
            " SELECT SUM(DeliveryTpGrandTotal-ISNULL(tbltemp.Amount,0))RemainAmount FROM dbo.tblInvoice "+
            " LEFT JOIN  "+
            " (SELECT InvoiceId,ISNULL(SUM(PaymentAmount),0)Amount FROM dbo.tblCustPayDetail GROUP BY InvoiceId) AS tbltemp ON tbltemp.InvoiceId = tblInvoice.InvoiceId "+
            " WHERE CustomerMasterId='"+customerId+"' AND PaymentStatus<>'Full')AS tbltemp";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable CustomerInfoAll(string custCode)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM dbo.tblCustMaster
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId
            LEFT JOIN dbo.tblArea ON dbo.tblArea.AreaId=dbo.tblTerritory.AreaId
            LEFT JOIN dbo.tblASMInfo ON dbo.tblArea.AreaId=dbo.tblASMInfo.AreaId
            LEFT JOIN dbo.tblRegion ON dbo.tblArea.RegionId=dbo.tblRegion.RegionId
            LEFT JOIN dbo.tblRSMInfo ON dbo.tblRegion.RegionId=dbo.tblRSMInfo.RSMId  WHERE  CustomerCode='" + custCode + "'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable GetAllAvailableOrder()
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT OrderCode,SubmissionDate,CustomerName,MIOName,MarketName,TerritoryName,RegionName,AreaName,SUM(Quantity)QTY,tblOrder.OrderId FROM dbo.tblOrder
            LEFT JOIN dbo.tblOrderDetail ON tblOrderDetail.OrderId = tblOrder.OrderId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblOrder.RegionId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblOrder.AreaId
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblOrder.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblOrder.TerritoryId
            LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.MIOId = tblOrder.MIOId
            WHERE (IsInvoice IS NULL OR IsInvoice=0)
            GROUP BY OrderCode,SubmissionDate,CustomerName,MIOName,MarketName,TerritoryName,RegionName,AreaName,tblOrder.OrderId";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable GetSpecialApproveOrder(string actionstatus)
        {
            DataTable aDataTable = new DataTable();

            string query = @"SELECT OrderCode,tblOrder.CustomerMasterId,SubmissionDate,CustomerName,MIOName,MarketName,TerritoryName,RegionName,AreaName,SUM(Quantity)QTY,tblOrder.OrderId,D.Due,CompanyName,tblCompanyUnit.ComUnitName FROM dbo.tblOrder
            LEFT JOIN dbo.tblOrderDetail ON tblOrderDetail.OrderId = tblOrder.OrderId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblOrder.RegionId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblOrder.AreaId
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblOrder.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblOrder.TerritoryId
            LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.MIOId = tblOrder.MIOId
			LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblOrder.ComUnitId
			LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
            LEFT JOIN (SELECT CustomerMasterId,ISNULL(SUM(DeliveryTpGrandTotal-(ISNULL(PaymentAmount,0) + ISNULL(DiscountOnPayment,0) + ISNULL(AIT,0))),0) AS Due FROM dbo.tblInvoice 
            WHERE (DeliveryInvoiceStatus IS NOT NULL AND DeliveryInvoiceStatus != 'Reject') GROUP BY CustomerMasterId) AS D ON D.CustomerMasterId = tblOrder.CustomerMasterId
            WHERE (IsInvoice IS NULL OR IsInvoice=0) AND ActionStatus='" + actionstatus + "' AND IsSpecialApproval='1' GROUP BY OrderCode,SubmissionDate,CustomerName,MIOName,MarketName,TerritoryName,RegionName,AreaName,tblOrder.OrderId,D.Due,tblOrder.CustomerMasterId,CompanyName,tblCompanyUnit.ComUnitName";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public bool ApprovalUpdateDAL(OrderInfoMaster aOrderInfoMaster)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@OrderId", aOrderInfoMaster.OrderId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aOrderInfoMaster.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aOrderInfoMaster.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aOrderInfoMaster.ApprovedDate));

            string query = @"UPDATE tblOrder SET ActionStatus=@ActionStatus,ApprovedBy=@ApprovedBy,ApprovedDate=@ApprovedDate WHERE OrderId=@OrderId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        }
        public DataTable GetMIOIDByTerritory(string terId)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM dbo.tblMIOInfo WHERE TerritoryId='"+terId+"'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable GetCustomerEditData(string id)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT *,VATAmountPerUnit as VAT FROM dbo.tblOrder
            LEFT JOIN dbo.tblOrderDetail ON tblOrderDetail.OrderId = tblOrder.OrderId
            LEFT JOIN dbo.tblDeliveryManInfo ON dbo.tblOrder.DeliveryPersonId=dbo.tblDeliveryManInfo.DeliveryManId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblOrder.RegionId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblOrder.AreaId
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblOrder.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblOrder.TerritoryId
            LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.MIOId = tblOrder.MIOId
            LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
            LEFT JOIN dbo.tblProduct ON tblProduct.ProductId = tblOrderDetail.ProductId
            LEFT JOIN dbo.tblUnitPrice ON tblUnitPrice.ProductId = tblProduct.ProductId AND tblUnitPrice.IsActive=1 AND tblUnitPrice.ActionStatus='Accepted'
            LEFT JOIN dbo.tblPackSize ON tblPackSize.PackSizeId = tblProduct.PackSizeId
            
                            WHERE tblOrder.OrderId='" + id+"'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public int OrderManualId()
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT (isnull(MAX(SUBSTRING(OrderCode,5,15)),0)+1) as PKMaxNo FROM dbo.tblOrder WHERE IsManual='True'";
            return Convert.ToInt32(aCommonInternalDal.DataContainerDataTable(query, "SSIDB").Rows[0][0].ToString());

        }
        public void DCLoad(DropDownList aDownList,string companyId)
        {
            string dc = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit WHERE CompanyId='"+companyId+"'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "Com", "ComUnitId", dc, "SSIDB");
        }
        public void AreaLoad(DropDownList aDownList, string dcId)
        {
            string dc = @"SELECT * FROM dbo.tblDcWiseAreaInfo
            INNER JOIN dbo.tblArea ON tblArea.AreaId = tblDcWiseAreaInfo.AreaId
            WHERE DCId='" + dcId + "' AND tblDcWiseAreaInfo.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "AreaName", "AreaId", dc, "SSIDB");
        }
        public void MIOLoad(DropDownList aDownList, string areaId)
        {
            string dc = @"SELECT DISTINCT (EmpName+'('+EmpMasterCode+')')Name,MIOId FROM dbo.tblMIOInfo
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMIOInfo.TerritoryId
            LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
            WHERE AreaId='" + areaId + "' AND tblMIOInfo.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "Name", "MIOId", dc, "SSIDB");
        }
        public void TerritoryLoad(DropDownList aDownList, string mioId)
        {
            string dc = @"
            SELECT * FROM dbo.tblTerritory
            INNER JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId
            WHERE EmployeeId='" + mioId + "' AND tblTerritory.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "TerritoryName", "TerritoryId", dc, "SSIDB");
        }
        public void MarketLoad(DropDownList aDownList, string territoryId)
        {
            string dc = @"SELECT * FROM dbo.tblMarket
            WHERE TerritoryId='"+territoryId+"' AND IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "MarketName", "MarketId", dc, "SSIDB");
        }
        public void MIOLoad(DropDownList aDownList)
        {
            string dc = @"SELECT * FROM dbo.tblMIOInfo WITH (NOLOCK )
	   LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblEmpGeneralInfo.EmpInfoId=dbo.tblMIOInfo.EmployeeId WHERE IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "EmpName", "MIOId", dc, "SSIDB");
        }
        public bool UpdateCompanyInfo(OrderInfoMaster aOrderInfoMaster)
        {
            string query = @"UPDATE tblOrder SET ComUnitId='" + aOrderInfoMaster.ComUnitId + "',ComUnitCode='" + aOrderInfoMaster.ComUnitCode + "',ComUnitName='" + aOrderInfoMaster.ComUnitName + "' WHERE OrderId=" + aOrderInfoMaster.OrderId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool DeleteDetail(string id)
        {
            string query = @"DELETE FROM dbo.tblOrderDetail WHERE OrderId='"+id+"'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable GetCustomerPaymentInfo(int id)
        {
            string query = @"SELECT * FROM dbo.tblInvoice AS INV 
                             WHERE INV.CustomerMasterId = '" + id + "' AND INV.PaymentStatus IS NULL OR INV.PaymentStatus = 'Partial'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

        }

        public DataTable GetMioByCompany(string terittoryId, string companyId)
        {
            string query = @"SELECT MIO.EmployeeId FROM dbo.tblMIOInfo AS MIO WHERE MIO.TerritoryId = " + terittoryId + "  AND MIO.CompanyId = " + companyId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster AS CSTMR
			 LEFT JOIN dbo.tblMarket AS MKT ON MKT.MarketId = CSTMR.MarketId
			 LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MKT.TerritoryId
			 LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.TerritoryId = TTR.TerritoryId
			 WHERE CSTMR.MarketId IS NOT NULL AND MIO.MIOId IS NOT NULL AND CSTMR.CustomerCode = '" + customerCode + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
