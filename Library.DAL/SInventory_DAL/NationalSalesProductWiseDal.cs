using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class NationalSalesProductWiseDal
    {
        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public DataTable GetProductWiseNationalSales(string parameter)
        {
            string query = @"SELECT CI.CompanyCode ,CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,EmpName AS RSM,RGN.RegionName,INVD.ProductCode,INVD.ProductName,INVD.PackSize,ISNULL(SUM(DeliveryTotalQuantity),0)AS Qty,
                             SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) AS Value FROM dbo.tblInvoice AS INV 
                             LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                             LEFT JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
                             LEFT JOIN dbo.tblRSMInfo AS RSM ON RSM.RSMId = ODR.RSMId
                             LEFT JOIN dbo.tblEmpGeneralInfo ON RSM.EmployeeId = dbo.tblEmpGeneralInfo.EmpInfoId
                             LEFT JOIN dbo.tblRegion AS RGN ON RGN.RegionId = RSM.RegionId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
                             + parameter + " GROUP BY  CI.CompanyCode ,CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,EmpName,RGN.RegionName,INVD.ProductCode,INVD.ProductName,INVD.PackSize "
                             + " HAVING SUM((CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END)) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetProductWiseNationalSales(string fDate, string tdate, string comId)
        {
            string parameter = " AND PD.CompanyId IN ( SELECT DISTINCT CompanyId FROM tblUserCompanyUnit AS CU LEFT JOIN tblCompanyUnit AS CI ON CU.CompanyUnitId = CI.ComUnitId WHERE UserId = " + HttpContext.Current.Session["UserId"] + ")";

            if (comId != "")
            {
                parameter = parameter + " AND PD.CompanyId = '" + comId + "'";
                
            }

            string query = @"SELECT NTP.ProductCode,PD.ProductName,PD.PackSize,SUM(NTP.DeliveryQuantity) AS SalesQuantity,SUM(NTP.Value) AS SalesValue FROM tblProduct AS PD 
LEFT JOIN (SELECT INVD.ProductCode,INVD.DeliveryQuantity,(CASE WHEN INVD.DeliveryNetAmount > 0 THEN  INVD.DeliveryNetAmount  ELSE 0 END) Value FROM dbo.tblInvoice AS INV 
LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId 
WHERE INVD.DeliveryStatus IN ('Full','Partial') AND INVD.Quantity > 0 AND  INV.InvoiceDate BETWEEN '" + fDate + "' AND '" + tdate + "') AS NTP ON NTP.ProductCode = PD.ProductCode WHERE PD.CompanyId IS NOT NULL  " + parameter + " GROUP BY NTP.ProductCode,PD.PackSize,PD.ProductName HAVING NTP.ProductCode IS NOT NULL ORDER BY NTP.ProductCode";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetSalesTrackingReport(string parameter)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceNo,INV.InvoiceDate,ISNULL(SUM(INVD.UnitPrice*INVD.Quantity),INV.DeliveryTpGrandTotal) AS InvoiceValue,  
                            CAST(ISNULL(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*INVD.DeliveryQuantity)),INV.DeliveryTpGrandTotal)  AS DECIMAL (16,2)) ActualSales,
                            CAST(ISNULL(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity))),0) AS DECIMAL (16,2)) ReturnValue,
                            CAST(ISNULL( SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity),0) AS DECIMAL (16,2)) Discount, ISNULL(TNS.InTransit,0) InTransit, ISNULL(INV.PaymentAmount,0) AS PaymentAmount, 
                            INV.DeliveryTpGrandTotal - (ISNULL(INV.PaymentAmount,0) + ISNULL(INV.AIT,0) + ISNULL(INV.DiscountOnPayment,0)) AS Due,ISNULL(INV.AIT,0) AIT,ISNULL(INV.DiscountOnPayment,0) DOP ,EGI.EmpName AS MIO,ISNULL((ISNULL(SUM(INVD.UnitPrice*INVD.Quantity),INV.DeliveryTpGrandTotal) - 
                            (CAST(ISNULL( SUM((INVD.DelivarySpecialAmount / INVD.Quantity) * INVD.DeliveryQuantity),0) AS DECIMAL (16,2)) + 
                            (CAST(ISNULL(SUM((((((INVD.UnitPrice*INVD.Quantity) - INVD.DelivarySpecialAmount))/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity)) + ((INVD.DelivarySpecialAmount/INVD.Quantity)*(INVD.Quantity-INVD.DeliveryQuantity))),0) AS DECIMAL (16,2))) + INV.DeliveryTpGrandTotal - ISNULL(INV.PaymentAmount,0))),0) AS PrevousCollection
                            FROM dbo.tblInvoice AS INV
                            LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INV.InvoiceId = INVD.InvoiceId
                            LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = INV.MiaId
                            LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId  
                            LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId  
                            LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                            LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId 
                            LEFT JOIN (
                            	SELECT tblInvoice.InvoiceId,SUM(tblInvoiceDetail.NetAmount) AS InTransit FROM tblInvoice 
                            	LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
                            	WHERE tblInvoice.DeliveryInvoiceStatus IS NULL
                            	GROUP BY tblInvoice.InvoiceId
                            ) AS TNS ON TNS.InvoiceId = INV.InvoiceId " + parameter
                            + " GROUP BY CI.CompanyName,UNT.ComUnitCode,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceNo,INV.InvoiceDate,DeliveryTpGrandTotal,TNS.InTransit,INV.PaymentAmount,EGI.EmpName,INV.AIT,INV.DiscountOnPayment ORDER BY EGI.EmpName,CSTMR.CustomerCode";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
