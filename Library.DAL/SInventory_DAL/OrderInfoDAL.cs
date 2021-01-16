using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class OrderInfoDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        DB_Manager aDbManager = new DB_Manager();
        public DataTable LoadOrder(string comunitId,string manufacId,string marketId)
        {
            string query = @"SELECT * FROM dbo.tblOrder
            LEFT JOIN dbo. View_CustomerMaster V ON dbo.tblOrder.CustomerCode = V.CustomerCode
          LEFT JOIN dbo.tblCompanyUnit ON dbo.tblOrder.ComUnitCode=dbo.tblCompanyUnit.ComUnitCode
            LEFT JOIN dbo.tblMarket ON V.MarketCode = dbo.tblMarket.MarketCode  WHERE V.ComUnitId='" + comunitId + "' AND ManufacId='" + manufacId + "' AND dbo.tblMarket.MarketId='" + marketId + "' AND IsInvoice =0 ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void LoadMarketByInvoice(DropDownList ddl, string comunitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT Distinct tblMarket.MarketId,tblMarket.MarketCode,tblMarket.MarketName FROM dbo.tblOrder
            inner JOIN View_CustomerMaster C ON dbo.tblOrder.CustomerCode = C.CustomerCode
            inner JOIN dbo.tblCompanyUnit ON dbo.tblOrder.ComUnitCode=dbo.tblCompanyUnit.ComUnitCode
            inner JOIN dbo.tblMarket ON C.MarketId = dbo.tblMarket.MarketId
            inner JOIN dbo.tblInvoice ON dbo.tblOrder.OrderId=dbo.tblInvoice.OrderId 
          WHERE  TpGrandTotal>0 AND (DelivaryInvoiceNo IS NULL OR DelivaryInvoiceNo='') and tblOrder.ComUnitId='" + comunitId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }
        public DataTable LoadOrderForOrderCreation(string comunitId, string manufacId, string marketId)
        {
            string query = @"SELECT * FROM dbo.tblOrder
            inner JOIN dbo. View_CustomerMaster V ON dbo.tblOrder.CustomerCode = V.CustomerCode
            inner JOIN dbo.tblCompanyUnit ON dbo.tblOrder.ComUnitCode=dbo.tblCompanyUnit.ComUnitCode
            inner JOIN dbo.tblMarket ON V.MarketCode = dbo.tblMarket.MarketCode  
            WHERE IsInvoice = 0 and   V.ComUnitId='" + comunitId + "' AND ManufacId='" + manufacId + "' AND dbo.tblMarket.MarketId='" + marketId + "' AND IsInvoice =0 ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadOrderForOrderCreation(string parameter)
        {
            string query = @"SELECT *,CASE WHEN ((ISNULL(D.DUE,0) > ISNULL(CR.LimitAmount,0)) OR (ISNULL(DATEDIFF(day, InvoiceDate,GETDATE()),0) > ISNULL(CR.DayLimit,0))) AND TermOfPayment = 'Credit'  THEN 0 ELSE 1 END AS Permission 
                            ,ISNULL(DATEDIFF(day, InvoiceDate,GETDATE()),0) AS Ds, ISNULL(CR.DayLimit,0) AS D FROM tblOrder
                            INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblOrder.CustomerMasterId
                            INNER JOIN dbo.tblMarket ON tblMarket.MarketId = tblOrder.MarketId                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
                            LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.MIOId = tblOrder.MIOId
                            INNER JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = dbo.tblOrder.ComUnitId
                            INNER JOIN tblCompanyInfo AS CI ON CI.CompanyId = tblCompanyUnit.CompanyId
                            LEFT JOIN ( SELECT CompanyId,CreditLimitId,CustomerMasterId,LimitAmount,DayLimit FROM tblCustomerCreditLimit 
                             WHERE ActionStatus = 'Accepted') AS CR ON tblCustMaster.CustomerMasterId = CR.CustomerMasterId AND CR.CompanyId =  CI.CompanyId 
                            LEFT JOIN (SELECT  CI.CompanyId,CustomerMasterId,SUM(ISNULL(DeliveryTpGrandTotal,0) - (ISNULL(PaymentAmount,0) + ISNULL(DiscountOnPayment,0) + ISNULL(AIT,0)))  AS DUE FROM tblInvoice 
                            LEFT JOIN tblCompanyUnit AS UNT ON UNT.ComUnitId = tblInvoice.ComUnitId
                            LEFT JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                            WHERE DeliveryInvoiceStatus IN ('Full','Partial') GROUP BY CustomerMasterId,CI.CompanyId ) 
                            AS D ON tblOrder.CustomerMasterId = D.CustomerMasterId AND D.CompanyId = CI.CompanyId
                            LEFT JOIN (SELECT DISTINCT CustomerMasterId,MIN(InvoiceDate) AS InvoiceDate FROM tblInvoice
                            WHERE DeliveryInvoiceStatus IN ('Full','Partial') AND ISNULL(DeliveryTpGrandTotal,0) - (ISNULL(PaymentAmount,0) + ISNULL(DiscountOnPayment,0) + ISNULL(AIT,0)) > 0
                            GROUP BY CustomerMasterId ) AS DD ON tblOrder.CustomerMasterId = DD.CustomerMasterId
                             WHERE IsInvoice = 0 AND ActionStatus = 'Accepted'" + parameter + "";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadOrderWithInvoice(string batchId)
        {
            string query = @"              
            SELECT * FROM dbo.tblOrder
            INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblOrder.CustomerMasterId
            INNER JOIN dbo.tblMarket ON tblMarket.MarketId = tblOrder.MarketId
            INNER JOIN dbo.tblMIOInfo ON tblMIOInfo.MIOId = tblOrder.MIOId
            INNER JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = dbo.tblOrder.ComUnitId
            INNER JOIN dbo.tblInvoice ON dbo.tblInvoice.OrderId=dbo.tblOrder.OrderId
            WHERE  TpGrandTotal>0 and BatchId='" + batchId + "'  AND DelivaryInvoiceNo IS NULL OR DelivaryInvoiceNo=''";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadInvoice(string invoiceId)
        {
            string query = @"SELECT *,UnitVatAmount AS UnitVAT,'0'SL,'0'StockQty,UnitPrice,UnitVatAmount,TotalQuantity AS Quantity,TotalPrice,TotalPriceVatAmount AS VAT,DiscountPercentage,DiscountAmount,NetAmount AS NetPrice,BonusQuantity AS BonusQty,TotalQuantity AS TotalQty FROM dbo.tblInvoice
            LEFT JOIN dbo.tblInvoiceDetail ON dbo.tblInvoice.InvoiceId = dbo.tblInvoiceDetail.InvoiceId
            LEFT JOIN dbo.tblProduct ON dbo.tblInvoiceDetail.ProductCode = dbo.tblProduct.ProductCode
            LEFT JOIN dbo.tblCustMaster ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            LEFT JOIN dbo.tblDeliveryManInfo ON tblDeliveryManInfo.DeliveryManId = tblInvoice.DeliveryManId
            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId
            WHERE dbo.tblInvoice.InvoiceId='" + invoiceId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadDelivaryInvoice(string invoiceno,string comunitId)
        {
            string query = @"SELECT UnitVatAmount AS UnitVAT,'0'SL,'0'StockQty,UnitPrice,UnitVatAmount,DeliveryQuantity AS Quantity,DeliveryTotalPrice AS TotalPrice,DeliveryTotalPriceVatAmount AS VAT,DiscountPercentage,DeliveryDiscountAmount AS DiscountAmount,DeliveryNetAmount AS NetPrice,'0'BonusQty,DeliveryTotalQuantity AS TotalQty,tblInvoiceDetail.DelivarySpecialAmount as SpecialAmount,* FROM dbo.tblInvoice
            LEFT JOIN dbo.tblInvoiceDetail ON dbo.tblInvoice.InvoiceId = dbo.tblInvoiceDetail.InvoiceId
            LEFT JOIN dbo.tblProduct ON dbo.tblInvoiceDetail.ProductCode = dbo.tblProduct.ProductCode
            LEFT JOIN dbo.tblCustMaster ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId WHERE DelivaryInvoiceNo='" +invoiceno+"' AND ComUnitId='"+comunitId+"'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetQty(string invoicedetailId)
        {
            string query = @"SELECT SUM(TotalQuantity) AS Qty FROM dbo.tblReturnInvoiceDetail WHERE InvoiceDetailId='"+invoicedetailId+"'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadOrderWithDetail(string orderid)
        {
            string query = @"SELECT tblCompanyUnit.ComUnitCode + ':' +  tblCompanyUnit.ComUnitName  AS ComUnitName,* FROM dbo.tblOrder
            
            LEFT JOIN dbo.tblCustMaster ON dbo.tblOrder.CustomerCode = dbo.tblCustMaster.CustomerCode
            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblOrder.ComUnitCode=dbo.tblCompanyUnit.ComUnitCode
            LEFT JOIN dbo.tblMarket ON dbo.tblCustMaster.MarketId = dbo.tblMarket.MarketId
            LEFT JOIN dbo.tblOrderDetail ON dbo.tblOrder.OrderId=dbo.tblOrderDetail.OrderId
            inner JOIN dbo.tblProduct ON dbo.tblOrderDetail.ProductId = dbo.tblProduct.ProductId    
               
            WHERE tblOrder.OrderId='" + orderid + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadDeliveryMan(string deliveryId)
        {
            string query = @"SELECT * FROM dbo.tblDeliveryManInfo
            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId WHERE DeliveryManId='" + deliveryId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadOffer(string InvoiceNo)
        {
            string query = @"select InvoiceId,InvoiceNo,ProductOffer,OldTradePolicy from tblInvoice WHERE tblInvoice.InvoiceNo='" + InvoiceNo + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadOrderExistsDal(string orderid)
        {
            string query = @"SELECT * FROM dbo.tblInvoice
            WHERE tblInvoice.OrderId='" + orderid + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetTradeTerm(string amount)
        {
            string query = @"SELECT * FROM dbo.tblTradePolicyNew WHERE '" + amount + "' BETWEEN MinAmount AND MaxAmount";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetTradeTermOld(string amount)
        {
            string query = @"SELECT * FROM dbo.tblTradePolicy WHERE '" + amount + "' BETWEEN MinAmount AND MaxAmount";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetFixedCustomer(string customerCode)
        {
            string query = @"select FixedCustomer from tblCustMaster WHERE tblCustMaster.CustomerCode='" + customerCode + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetFixedCustomerfromInvoiceTable(string invoiceid)
        {
            string query = @"select 'False'FixedCustomer from dbo.tblInvoice WHERE tblInvoice.InvoiceId='" + invoiceid + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ProductVat(string productcode)
        {
            string query = @"SELECT * FROM dbo.tblUnitPrice WHERE ProductCode='"+productcode+"'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ProductDiscount(string productcode, string customerId, string invoicedate)
        {
            string query = @"SELECT * FROM dbo.tblProductDiscount INNER JOIN dbo.tblProduct ON tblProduct.ProductId = tblProductDiscount.ProductId WHERE IsActive=1 AND CustomerMasterId='" + customerId + "' AND ProductCode='" + productcode + "' AND ('" + invoicedate + "' BETWEEN tblProductDiscount.ActiveDate AND tblProductDiscount.InactiveDate)";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

       
        public void LoadSC(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        public void LoadSC(DropDownList ddl,string userId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
//            string queryStr = @"SELECT * FROM dbo.tblCompanyUnit
//                                LEFT JOIN dbo.tblUserCompanyUnit ON dbo.tblCompanyUnit.ComUnitId=dbo.tblUserCompanyUnit.CompanyUnitId
//                                WHERE UserId='"+userId+"'";


            string queryStr = "select ComUnitId, ComUnitName  from tblCompanyUnit WHERE " +
                               " ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId='" + userId.Trim() + "')";


            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        public void LoadDC(DropDownList ddl, string companyId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            //            string queryStr = @"SELECT * FROM dbo.tblCompanyUnit
            //                                LEFT JOIN dbo.tblUserCompanyUnit ON dbo.tblCompanyUnit.ComUnitId=dbo.tblUserCompanyUnit.CompanyUnitId
            //                                WHERE UserId='"+userId+"'";


            string queryStr = "SELECT * FROM dbo.tblCompanyUnit WHERE CompanyId='"+companyId+"' ";


            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void GetDcInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT ComUnitId, ComUnitCode + ' : ' + ComUnitName AS UnitName FROM tblCompanyUnit WHERE CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDal.LoadDropDownValue(ddl, "UnitName", "ComUnitId", queryStr, "SSIDB");
        }

        public void LoadManufac(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }
        //public bool UpdateInvoiceStatus(string id)
        //{
        //    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //    aSqlParameterlist.Add(new SqlParameter("@OrderId", id));

        //    return aDbManager.UpdateAction("sp_UD_InvoiceDetail", aSqlParameterlist);
        //}
        public bool UpdateInvoiceStatus(string id)
        {
            string query = @"UPDATE dbo.tblOrder SET IsInvoice='True' WHERE OrderId='" + id + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public void LoadMarket(DropDownList ddl,string comunitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT DISTINCT CM.MarketId,CM.MarketCode,CM.MarketName FROM dbo.tblCompanyUnit Cu
                inner JOIN dbo.View_CustomerMaster CM ON CM.ComUnitCode = Cu.ComUnitCode
                inner JOIN dbo.tblMarket ON tblMarket.MarketCode = CM.MarketCode
                WHERE CU.ComUnitId='" + comunitId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }
      
//        public void LoadMarketOrderWise(DropDownList ddl, string comunitId)
//        {
//            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
//            string queryStr = @"SELECT DISTINCT tblMarket.MarketId,tblMarket.MarketCode,tblMarket.MarketName FROM dbo.tblOrder
//            inner JOIN dbo. View_CustomerMaster V ON dbo.tblOrder.CustomerCode = V.CustomerCode
//            inner JOIN dbo.tblCompanyUnit ON dbo.tblOrder.ComUnitCode=dbo.tblCompanyUnit.ComUnitCode
//            inner JOIN dbo.tblMarket ON V.MarketCode = dbo.tblMarket.MarketCode  
//            WHERE IsInvoice = 0 and   V.ComUnitId='" + comunitId + "' AND IsInvoice =0 ";
//            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
//        }
        public void LoadMarketOrderWise(DropDownList ddl, string comunitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT * FROM dbo.tblMarket
            INNER JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            INNER JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            INNER JOIN dbo.tblDcWiseAreaInfo ON tblDcWiseAreaInfo.AreaId = tblArea.AreaId
            WHERE DCId='"+comunitId+"' AND MarketId IN (SELECT MarketId FROM dbo.tblOrder WHERE IsInvoice=0) AND tblMarket.IsActive=1";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }
        public void AreaLoad(DropDownList aDownList, string dcId)
        {
            string dc = @"SELECT * FROM dbo.tblDcWiseAreaInfo
            INNER JOIN dbo.tblArea ON tblArea.AreaId = tblDcWiseAreaInfo.AreaId
            WHERE DCId='" + dcId + "' AND tblDcWiseAreaInfo.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "AreaName", "AreaId", dc, "SSIDB");
        }
        public void TerritoryLoad(DropDownList aDownList, string areaId)
        {
            string dc = @"SELECT * FROM dbo.tblTerritory WHERE AreaId='"+areaId+"'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "TerritoryName", "TerritoryId", dc, "SSIDB");
        }
        public void MarketLoad(DropDownList aDownList, string territoryId)
        {
            string dc = @"SELECT * FROM dbo.tblMarket
            WHERE TerritoryId='" + territoryId + "' AND IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "MarketName", "MarketId", dc, "SSIDB");
        }
        public void BatchNoLoad(DropDownList aDownList, string companyId, string date, string delmanId)
        {
            string dc = @"SELECT * FROM dbo.tblInvoiceBatch WHERE CompanyId='" + companyId + "' AND Date='" + date + "' AND DeliveryManId='" + delmanId + "'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "BatchNo", "BatchId", dc, "SSIDB");
        }

        public void BatchNoLoadByUser(DropDownList aDownList, string date, string delmanId,string userId)
        {
            string dc = @"SELECT DISTINCT tblInvoiceBatch.BatchId,BatchNo FROM dbo.tblInvoiceBatch 
                          INNER JOIN dbo.tblInvoice ON tblInvoice.BatchId = tblInvoiceBatch.BatchId WHERE Date='" + date + "' AND tblInvoiceBatch.DeliveryManId = " + delmanId + " AND tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDal.LoadDropDownValue(aDownList, "BatchNo", "BatchId", dc, "SSIDB");
        }


        public void BatchNoLoad(DropDownList aDownList, string date, string delmanId)
        {
            string dc = @"SELECT * FROM dbo.tblInvoiceBatch WHERE Date='" + date + "' AND DeliveryManId='" + delmanId + "'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "BatchNo", "BatchId", dc, "SSIDB");
        }

        public void CustomerLoad(DropDownList aDownList, string marketId)
        {
            string dc = @"SELECT * FROM dbo.tblCustMaster WHERE MarketId='"+marketId+"'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "CustomerName", "CustomerMasterId", dc, "SSIDB");
        }
        public void LoadSalesCenter(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        public void LoadArea(DropDownList ddl, string comUnitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblArea WHERE AreaId IN (SELECT AreaId FROM dbo.View_CustomerMaster WHERE ComUnitId='" + comUnitId + "')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }
        public void LoadMarketbyArea(DropDownList ddl, string areaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblMarket WHERE MarketId IN (SELECT DISTINCT MarketId FROM dbo.View_CustomerMaster WHERE AreaId='" + areaId + "')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }
        public DataTable LoadInvoice(string ComUnitId, string ManufId, string marketid, DateTime invDate)
        {
            string query = @"SELECT  * 				
               FROM tblInvoice I
                   INNER JOIN (SELECT DISTINCT D.InvoiceId, ManufacId FROM dbo.tblInvoice I
            INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId
            INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode WHERE  ManufacId='" + ManufId + "' AND UpdateDate='" + invDate + "' " +
            " ) as tblD ON I.InvoiceId = tblD.InvoiceId  " +
          " INNER JOIN dbo.View_CustomerMaster C ON I.CustomerMasterId = C.CustomerMasterId " +
             " where I.ComUnitId= '" + ComUnitId + "' and tblD.ManufacId='" + ManufId + "' and MarketId='" + marketid + "' and UpdateDate='" + invDate + "' AND  I.DeliveryInvoiceStatus IN ('Full','Partial') order by OrderNo";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadDeliveryManList(DropDownList ddl)
        {
            string query = @"SELECT *,EmpName as DeliveryManName FROM dbo.tblDeliveryManInfo
            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId";
            aCommonInternalDal.LoadDropDownValue(ddl, "DeliveryManName", "DeliveryManId", query, "SSIDB");
        }

        public void LoadDeliveryMan(DropDownList aDropDownList)
        {
            string query = @"SELECT *,EmpName as DeliveryManName FROM dbo.tblDeliveryManInfo
            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "DeliveryManName", "DeliveryManId", query, "SSIDB");
        }

        public DataTable GetCustomerFixedDiscount(string custCode)
        {
            string query = @"SELECT * FROM dbo.tblCustomerFixedDiscount AS CSD 
                            LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSD.CustomerMasterId = CSTMR.CustomerMasterId
                            WHERE CSD.IsActive = 1 AND CSTMR.CustomerCode = " + custCode;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerDueHistory(string customerMasterId)
        {
            string query = @"SELECT VC.CustomerMasterId, CI.CompanyName,UNT.ComUnitName,InvoiceNo,InvoiceDate,ISNULL(SUM(DeliveryTpGrandTotal-(ISNULL(PaymentAmount,0) + ISNULL(DiscountOnPayment,0) + ISNULL(AIT,0))),0) AS DueAmount FROM tblInvoice AS INV 
                            LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId 
                            LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                            LEFT JOIN dbo.View_CustomerMaster AS VC ON INV.CustomerMasterId = VC.CustomerMasterId
                            WHERE INV.CustomerMasterId  = " + customerMasterId + "GROUP BY CI.CompanyName,UNT.ComUnitName,InvoiceNo,InvoiceDate,VC.CustomerMasterId HAVING ISNULL(SUM(DeliveryTpGrandTotal-(ISNULL(PaymentAmount,0) + ISNULL(DiscountOnPayment,0) + ISNULL(AIT,0))),0) > 0 ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CheckCreditStatus(int comId, string customerCode)
        {
            throw new NotImplementedException();
        }
    }
}
