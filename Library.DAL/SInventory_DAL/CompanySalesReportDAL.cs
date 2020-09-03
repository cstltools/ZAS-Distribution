using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CompanySalesReportDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public DataTable CompanyReportMainDataDAL(DateTime fromDate, DateTime toDate)
        {
            string query = @"select '" + fromDate + "' as FromDate, '" + toDate + "'as ToDate";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CompanyReportDetailDataDAL(DateTime fromDate, DateTime toDate)
        {
            string query =@"select ProductCode,Product, sum(TotalQuantity) as TotalQty ,sum(Price) as TotalAmount from View_MIAWiseSalesReport  where InvoiceDate between '" + fromDate + "' and '" + toDate + "' group by ProductCode,Product";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        // Sales Report 
        public DataTable SalesReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        {

            string query = @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) as OrderDate,I.InvoiceNo,
                         CONVERT(VARCHAR,I.InvoiceDate,103)  InvoiceDate,I.DelivaryInvoiceNo, CONVERT(VARCHAR,I.InvoiceDate,103) UpdateDate,ID.ProductCode,ID.ProductName,
                         ID.PackSize,ID.BatchNo,CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.DeliveryQuantity,DeliveryNetAmount,
                         DeliveryTotalPriceVatAmount,DeliveryDiscountAmount,ID.DelivarySpecialAmount,C.AreaCode,C.AreaName as DistrictCode,C.TerritoryCode, C.TerritoryName,EGI.EmpMasterCode AS MiaCode , C.RegionCode,C.MarketCode,C.MarketName
                         FROM dbo.tblInvoice I  with(nolock)
                         INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                         INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
                         INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
                         INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId 
						 INNER JOIN dbo.tblMIOInfo AS MIO ON I.MiaId = MIO.MIOId
						 INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                         WHERE ID.DeliveryStatus IN ('Full','Partial')
                         AND I.ComUnitId = '" + districtId.Trim() + "' and I.InvoiceDate BETWEEN '" + fromDate + "' and '" + toDate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        // Sales Report 
        public DataTable SalesReportDAl(DateTime fromDate, DateTime toDate)
        {

            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) as OrderDate,I.InvoiceNo,
                         CONVERT(VARCHAR,I.InvoiceDate,103)  InvoiceDate,I.DelivaryInvoiceNo, CONVERT(VARCHAR,I.InvoiceDate,103) UpdateDate,ID.ProductCode,ID.ProductName,
                         ID.PackSize,ID.BatchNo,CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.DeliveryQuantity,DeliveryNetAmount,
                         DeliveryTotalPriceVatAmount,DeliveryDiscountAmount,ID.DelivarySpecialAmount,C.AreaCode,C.AreaName as DistrictCode,C.TerritoryCode, C.TerritoryName,EGI.EmpMasterCode AS MiaCode , C.RegionCode,C.MarketCode,C.MarketName
                         FROM dbo.tblInvoice I  with(nolock)
                         INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                         INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
                         INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
                         INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId 
						 INNER JOIN dbo.tblMIOInfo AS MIO ON I.MiaId = MIO.MIOId
						 INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                         WHERE ID.DeliveryStatus IN ('Full','Partial')
                         AND I.InvoiceDate BETWEEN '" + fromDate + "' and '" + toDate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        // Delivery Return Report
        public DataTable DeliveryReturnReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        {

            string query =
                       @"  SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,Convert(varchar,I.OrderDate,103) as OrderDate,I.InvoiceNo,
                           Convert(varchar,I.InvoiceDate,103)	as InvoiceDate,I.DelivaryInvoiceNo,Convert(varchar,I.UpdateDate,103)	as UpdateDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,Convert(varchar,DS.ExpDate,103) as ExpDate,
                           (ID.Quantity-ID.DeliveryQuantity)Quantity,(ID.NetAmount-ID.DeliveryNetAmount)Amount,
                           (ID.TotalPriceVatAmount-ID.DeliveryTotalPriceVatAmount)VatAmount,C.AreaCode,C.TerritoryCode, C.TerritoryName,EGI.EmpMasterCode AS MiaCode,C.AreaName as DistrictCode , C.RegionCode,ReturnReason,
                           Convert(varchar,I.UpdateDate,103)	as UpdateDate ,C.MarketCode,C.MarketName
                           FROM dbo.tblInvoice I with(nolock)
                           INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                           INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId 
                           INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId 
                           INNER JOIN dbo.tblMIOInfo AS MIO ON I.MiaId = MIO.MIOId
                           INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                           INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId 
                            where ID.DeliveryStatus IN ('Reject','Partial') and CU.ComUnitId='" + districtId.Trim() + "'  and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DeliveryReturnReportDAl(DateTime fromDate, DateTime toDate)
        {

            string query =
                       @" SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,Convert(varchar,I.OrderDate,103) as OrderDate,I.InvoiceNo,
                          Convert(varchar,I.InvoiceDate,103)	as InvoiceDate,I.DelivaryInvoiceNo,Convert(varchar,I.UpdateDate,103)	as UpdateDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,Convert(varchar,DS.ExpDate,103) as ExpDate,
                          (ID.Quantity-ID.DeliveryQuantity)Quantity,(ID.NetAmount-ID.DeliveryNetAmount)Amount,
                          (ID.TotalPriceVatAmount-ID.DeliveryTotalPriceVatAmount)VatAmount,C.AreaCode,C.TerritoryCode, C.TerritoryName,EGI.EmpMasterCode AS MiaCode,C.AreaName as DistrictCode , C.RegionCode,ReturnReason,
                          Convert(varchar,I.UpdateDate,103)	as UpdateDate ,C.MarketCode,C.MarketName
                          FROM dbo.tblInvoice I with(nolock)
                          INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                          INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId 
                          INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId 
                          INNER JOIN dbo.tblMIOInfo AS MIO ON I.MiaId = MIO.MIOId
                          INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                          INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId 
                           where ID.DeliveryStatus IN ('Reject','Partial') and  I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CustomerPaymentDAl(string PaymentStatus, DateTime fromDate, DateTime toDate, string salesCenter)
        {
            string query = "";
            //Partial Payment
            if (PaymentStatus =="0")
            {
                 query =
                    @"SELECT tblCompanyUnit.ComUnitCode,tblCompanyUnit.ComUnitName,CustomerCode,CustomerName,InvoiceNo,InvoiceDate as InvoiceDate,DelivaryInvoiceNo,tblInvoice.UpdateDate as  DelivaryInvoiceDate,DeliveryTpGrandTotal,IsNull(tblCustPayDetail.PaymentAmount,0)PaymentAmount,PaymentStatus,dbo.tblCustomerPay.PaymentDate
,tblInvoice.MarketCode,tblInvoice.MarketName,tblInvoice.AreaCode,tblInvoice.MiaCode,tblInvoice.DisCode as DistrictCode , tblInvoice.RegionCode,tblInvoice.MIAName
FROM dbo.tblInvoice WITH (nolock) 
 INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = tblInvoice.ComUnitId
inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
left JOIN dbo.tblCustPayDetail ON dbo.tblInvoice.InvoiceId = dbo.tblCustPayDetail.InvoiceId
left JOIN dbo.tblCustomerPay ON dbo.tblCustPayDetail.CustPayId=dbo.tblCustomerPay.CustPayId
inner JOIN tblCompanyUnit ON dbo.tblInvoice.ComUnitId=dbo.tblCompanyUnit.ComUnitId

WHERE  (DeliveryInvoiceStatus='Full' or DeliveryInvoiceStatus= 'Partial') and DeliveryTpGrandTotal > 0 AND DeliveryTpGrandTotal > ISNULL(tblInvoice.PaymentAmount,0) and CU.ComUnitId='" + salesCenter.Trim() + "' and tblInvoice.UpdateDate between  '" + fromDate + "' and '" + toDate + "'";
            }
            //Full Payment
            if (PaymentStatus == "1")
            {
                 query =
                     @"SELECT tblCompanyUnit.ComUnitCode,tblCompanyUnit.ComUnitName,DelivaryInvoiceNo, CustomerCode,CustomerName,InvoiceNo,InvoiceDate as InvoiceDate,tblInvoice.UpdateDate as  DelivaryInvoiceDate,DeliveryTpGrandTotal,IsNull(dbo.tblCustPayDetail.PaymentAmount,0)PaymentAmount,PaymentStatus
,tblCustomerPay.PaymentDate
,tblInvoice.MarketCode,tblInvoice.MarketName,tblInvoice.AreaCode,tblInvoice.MiaCode,tblInvoice.DisCode as DistrictCode , tblInvoice.RegionCode,tblInvoice.MIAName
FROM dbo.tblInvoice WITH (nolock)
 INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = tblInvoice.ComUnitId
inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
left JOIN dbo.tblCustPayDetail ON dbo.tblInvoice.InvoiceId = dbo.tblCustPayDetail.InvoiceId
LEFT JOIN tblCustomerPay ON dbo.tblCustPayDetail.CustPayId=tblCustomerPay.CustPayId
inner JOIN tblCompanyUnit ON dbo.tblInvoice.ComUnitId=dbo.tblCompanyUnit.ComUnitId
WHERE DeliveryTpGrandTotal > 0
and DeliveryTpGrandTotal = tblInvoice.PaymentAmount and CU.ComUnitId='" + salesCenter.Trim() + "'and tblCustomerPay.PaymentDate between '" + fromDate + "' and '" + toDate + "' order by PaymentDate ";
            }

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CustomerPaymentDAl(string PaymentStatus, DateTime fromDate, DateTime toDate)
        {
            string query = "";
            //Partial Payment
            if (PaymentStatus == "0")
            {
                query =
                   @"SELECT tblCompanyUnit.ComUnitCode,tblCompanyUnit.ComUnitName,CustomerCode,CustomerName,InvoiceNo,InvoiceDate as InvoiceDate,DelivaryInvoiceNo,tblInvoice.UpdateDate as  DelivaryInvoiceDate,DeliveryTpGrandTotal,IsNull(tblCustPayDetail.PaymentAmount,0)PaymentAmount,PaymentStatus,dbo.tblCustomerPay.PaymentDate
,tblInvoice.MarketCode,tblInvoice.MarketName,tblInvoice.AreaCode,tblInvoice.MiaCode,tblInvoice.DisCode as DistrictCode , tblInvoice.RegionCode,tblInvoice.MIAName
FROM dbo.tblInvoice WITH (nolock) 
 INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = tblInvoice.ComUnitId
inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
left JOIN dbo.tblCustPayDetail ON dbo.tblInvoice.InvoiceId = dbo.tblCustPayDetail.InvoiceId
left JOIN dbo.tblCustomerPay ON dbo.tblCustPayDetail.CustPayId=dbo.tblCustomerPay.CustPayId
inner JOIN tblCompanyUnit ON dbo.tblInvoice.ComUnitId=dbo.tblCompanyUnit.ComUnitId

WHERE  (DeliveryInvoiceStatus='Full' or DeliveryInvoiceStatus= 'Partial') and DeliveryTpGrandTotal > 0 AND DeliveryTpGrandTotal > ISNULL(tblInvoice.PaymentAmount,0)   and tblInvoice.UpdateDate between  '" + fromDate + "' and '" + toDate + "'";
            }
            //Full Payment
            if (PaymentStatus == "1")
            {
                query =
                    @"SELECT  tblCompanyUnit.ComUnitCode,tblCompanyUnit.ComUnitName,DelivaryInvoiceNo, CustomerCode,CustomerName,InvoiceNo,InvoiceDate as InvoiceDate,tblInvoice.UpdateDate as  DelivaryInvoiceDate,DeliveryTpGrandTotal,IsNull(dbo.tblCustPayDetail.PaymentAmount,0)PaymentAmount,PaymentStatus
,tblCustomerPay.PaymentDate
,tblInvoice.MarketCode,tblInvoice.MarketName,tblInvoice.AreaCode,tblInvoice.MiaCode,tblInvoice.DisCode as DistrictCode , tblInvoice.RegionCode,tblInvoice.MIAName
FROM dbo.tblInvoice WITH (nolock)
 INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = tblInvoice.ComUnitId
inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
left JOIN dbo.tblCustPayDetail ON dbo.tblInvoice.InvoiceId = dbo.tblCustPayDetail.InvoiceId
LEFT JOIN tblCustomerPay ON dbo.tblCustPayDetail.CustPayId=tblCustomerPay.CustPayId
inner JOIN tblCompanyUnit ON dbo.tblInvoice.ComUnitId=dbo.tblCompanyUnit.ComUnitId

                        WHERE DeliveryTpGrandTotal > 0
                        and DeliveryTpGrandTotal = tblInvoice.PaymentAmount and  tblCustomerPay.PaymentDate between '" + fromDate + "' and '" + toDate + "' order by PaymentDate ";
            }
            //            if (PaymentStatus == "2")
            //            {
            //                query =
            //                    @"SELECT CustomerCode,CustomerName,InvoiceNo,CONVERT(VARCHAR(10),InvoiceDate,110) as InvoiceDate,DelivaryInvoiceNo,CONVERT(VARCHAR(10),tblInvoice.UpdateDate,110) as  DelivaryInvoiceDate,DeliveryTpGrandTotal,IsNull(tblCustPayDetail.PaymentAmount,0)PaymentAmount,PaymentStatus
            //                        ,PaymentDate,RefNo,PayType,tblCompanyUnit.ComUnitCode,tblCompanyUnit.ComUnitName,tblMarket.MarketCode,tblMarket.MarketName
            //                        FROM dbo.tblInvoice 
            //                        left JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
            //                        left JOIN dbo.tblCustPayDetail ON dbo.tblInvoice.InvoiceId= dbo.tblCustPayDetail.InvoiceId
            //                        left JOIN dbo.tblCustomerPay ON dbo.tblCustPayDetail.CustPayId=dbo.tblCustomerPay.CustPayId
            //                        left JOIN dbo.tblCompanyUnit ON dbo.tblCustomerPay.ComUnitId=dbo.tblCompanyUnit.ComUnitId
            //                        left JOIN dbo.tblMarket ON dbo.tblCustomerPay.MarketId=dbo.tblMarket.MarketId
            //
            //                       WHERE DeliveryTpGrandTotal > 0  and tblInvoice.UpdateDate between '" + fromDate + "' and '" + toDate + "'";
            // }
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        // Sales Rejection 
        public DataTable SalesReectionReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        {

            string query =
                       @"SELECT up.UnitPrice AS Batch,up.VATAmountPerUnit AS VatAmount,U.ComUnitCode,U.ComUnitName,C.MarketCode,C.MarketName,C.CustomerCode,C.CustomerName
                        ,O.OrderCode,CONVERT(NVARCHAR,O.SubmissionDate,103)SubmissionDate
                        ,D.ProductCode,PP.ProductName,I.InvoiceNo,CONVERT(NVARCHAR,I.InvoiceDate,103)InvoiceDate,D.Quantity AS RejectQuantity
                        ,D.TotalTradePrice AS NetRejectionAmount , CONVERT(NVARCHAR,I.InvoiceDate,103) AS DateofRejection,C.RegionCode AS DZSMCode,C.DistrictCode AS FECode , C.MiaCode AS MIOCode,C.AreaCode AS TerritoryCode
                        FROM dbo.tblOrder O
                        INNER JOIN dbo.tblOrderDetail D ON O.OrderId = D.OrderId
                        left JOIN dbo.tblCompanyUnit U ON O.ComUnitId = U.ComUnitId
                         left JOIN dbo.tblProduct PP ON D.ProductCode = PP.ProductCode
                        left JOIN dbo.View_CustomerMaster C ON O.CustomerCode = C.CustomerCode
                        left JOIN dbo.tblInvoice I ON O.OrderCode = I.OrderNo
                        left JOIN dbo.tblUnitPrice UP ON D.ProductCode = UP.ProductCode
                        WHERE Status='Undelivered' AND I.InvoiceNo IS NOT NULL
                        and O.ComUnitId='" + districtId.Trim() + "' and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable SalesReectionReportDAl(DateTime fromDate, DateTime toDate)
        {

            string query =
                       @"SELECT up.UnitPrice AS Batch,up.VATAmountPerUnit AS VatAmount,U.ComUnitCode,U.ComUnitName,C.MarketCode,C.MarketName,C.CustomerCode,C.CustomerName
                        ,O.OrderCode,CONVERT(NVARCHAR,O.SubmissionDate,103)SubmissionDate
                        ,D.ProductCode,PP.ProductName,I.InvoiceNo,CONVERT(NVARCHAR,I.InvoiceDate,103)InvoiceDate,D.Quantity AS RejectQuantity
                        ,D.TotalTradePrice AS NetRejectionAmount , CONVERT(NVARCHAR,I.InvoiceDate,103) AS DateofRejection,C.RegionCode AS DZSMCode,C.DistrictCode AS FECode , C.MiaCode AS MIOCode,C.AreaCode AS TerritoryCode
                        FROM dbo.tblOrder O
                        INNER JOIN dbo.tblOrderDetail D ON O.OrderId = D.OrderId
                        left JOIN dbo.tblCompanyUnit U ON O.ComUnitId = U.ComUnitId
                         left JOIN dbo.tblProduct PP ON D.ProductCode = PP.ProductCode
                        left JOIN dbo.View_CustomerMaster C ON O.CustomerCode = C.CustomerCode
                        left JOIN dbo.tblInvoice I ON O.OrderCode = I.OrderNo
                        left JOIN dbo.tblUnitPrice UP ON D.ProductCode = UP.ProductCode
                        WHERE Status='Undelivered' AND I.InvoiceNo IS NOT NULL
                        AND I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable SalesReportDAlParameter(DateTime fromDate, DateTime toDate, string parameter)
        {

            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) as OrderDate,I.InvoiceNo,
                      CONVERT(VARCHAR,I.InvoiceDate,103)  InvoiceDate,I.DelivaryInvoiceNo, CONVERT(VARCHAR,I.UpdateDate,103) UpdateDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.DeliveryQuantity,DeliveryNetAmount,
                         DeliveryTotalPriceVatAmount,DeliveryDiscountAmount,ID.DelivarySpecialAmount,A.AreaCode,MI.MiaCode,DIS.DistrictCode , R.RegionCode
                        FROM dbo.tblInvoice I 
                       INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId 
                       INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
                       INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
                       INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId 
                        LEFT JOIN dbo.tblArea A ON A.AreaId = C.AreaId
                      LEFT JOIN dbo.tblMIAInfo MI ON MI.MiaId =C.MiaId 
                       LEFT JOIN dbo.tblDistrict DIS ON DIS.DistrictId = C.DistrictId
                        INNER JOIN dbo.tblRegion R ON R.RegionId = C.RegionId
                      where ID.DeliveryStatus IN ('Full','Partial') 
                       and I.UpdateDate between '" + fromDate + "' and '" + toDate + "' " + parameter + "";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
