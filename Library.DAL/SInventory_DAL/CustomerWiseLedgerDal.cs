using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerWiseLedgerDal
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

        

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");


        }

        public DataTable GetMioWiseProductSales(string parameter)
        {

            string query = @"SELECT CI.CompanyName,EmpName AS MIO,ARA.AreaName,TTR.TerritoryName,tblProduct.ProductCode,tblProduct.ProductName,tblProduct.PackSize,ISNULL(SUM(DeliveryTotalQuantity),0)AS Qty,ISNULL(SUM(DeliveryTotalQuantity * UnitPrice),0)AS Value FROM dbo.tblInvoice
                             LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
                             LEFT JOIN dbo.tblProduct ON tblProduct.ProductCode = tblInvoiceDetail.ProductCode
                             LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=dbo.tblInvoice.MiaId
                             LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = tblMIOInfo.TerritoryId
                             LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId
                             LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = tblMIOInfo.CompanyId
                             " + parameter + " GROUP BY CI.CompanyName,EmpName,TTR.TerritoryName,ARA.AreaName,UnitPrice,tblProduct.ProductCode,tblProduct.PackSize,tblProduct.ProductName HAVING ISNULL(SUM(DeliveryTotalQuantity),0) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }


        public void LoadCustomerInfoInfo(DropDownList ddl, string dcId, string companyId)
        {
            string query = @"SELECT DISTINCT CSTMR.CustomerMasterId,CSTMR.CustomerName FROM dbo.tblDcWiseAreaInfo AS DCA 
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON DCA.DCId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             LEFT JOIN dbo.tblArea AS ARA ON DCA.AreaId = ARA.AreaId 
                             LEFT JOIN dbo.tblTerritory AS TTR ON ARA.AreaId = TTR.AreaId 
                             LEFT JOIN dbo.tblMarket AS MKT ON TTR.TerritoryId = MKT.TerritoryId
                             LEFT JOIN dbo.tblCustMaster AS CSTMR ON MKT.MarketId = CSTMR.MarketId WHERE CSTMR.CustomerMasterId IS NOT NULL AND " + "CI.CompanyId = " + companyId + " AND DCA.DCId = " + dcId + " ORDER BY CSTMR.CustomerName";
            aCommonInternalDAL.LoadDropDownValue(ddl, "CustomerName", "CustomerMasterId", query, "SSIDB");
        }

        public DataTable GetCustomerWiseReturn(string parameter)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName AS Depot,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode,INV.InvoiceNo,INV.InvoiceDate,
                             INV.TpGrandTotal AS InvoiceValue,ISNULL((CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END),0) AS SaleValue,INV.TpGrandTotal-(CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END) AS ReturnValue FROM dbo.tblInvoice AS INV 
                             LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId
                             LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             " + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode,INV.InvoiceNo,INV.InvoiceDate,INV.TpGrandTotal,INV.DeliveryTpGrandTotal,INV.TpGrandTotal HAVING INV.TpGrandTotal-(CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END) > 0 ORDER BY CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode,INV.InvoiceNo,INV.InvoiceDate";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerWiseReturnByProduct(string parameter)
        {
            string query = @"SELECT CI.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,tblProduct.ProductCode,tblProduct.ProductName,tblProduct.PackSize,ISNULL(SUM(TotalQuantity - DeliveryTotalQuantity),0)AS Qty,ISNULL(SUM((TotalQuantity - DeliveryTotalQuantity) * UnitPrice),0)AS Value FROM dbo.tblInvoice
                             LEFT JOIN dbo.tblInvoiceDetail ON tblInvoiceDetail.InvoiceId = tblInvoice.InvoiceId
                             LEFT JOIN dbo.tblProduct ON tblProduct.ProductCode = tblInvoiceDetail.ProductCode
                             LEFT JOIN dbo.tblMIOInfo ON dbo.tblMIOInfo.MIOId=dbo.tblInvoice.MiaId
                             LEFT JOIN dbo.tblCustMaster AS CSTMR ON  tblInvoice.CustomerMasterId = CSTMR.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON tblInvoice.ComUnitId = UNT.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                             " + parameter + " GROUP BY CI.CompanyName,CSTMR.CustomerCode,CSTMR.CustomerName,tblProduct.ProductCode,tblProduct.PackSize,tblProduct.ProductName HAVING ISNULL(SUM(TotalQuantity - DeliveryTotalQuantity),0) > 0";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerWiseLedger(string fromDate, string toDate, string parameter)
        {
            string query = @"SELECT CI.CompanyCode + ' : ' + CI.CompanyName AS UnitName,VC.CustomerCode + ' : ' + VC.CustomerName AS Customer,InvoiceNo,InvoiceDate,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate,"
                +"ISNULL(ActualSales,0) ActualSales,ISNULL(ReturnAmount,0) ReturnAmount,ISNULL(PMNT.TotalPay,0) TotalPay,ISNULL(PMNT.TotalAit,0) TotalAit,"
                +"ISNULL(PMNT.TotalDiscount,0) TotalDiscount,(ISNULL(ActualSales,0) - (ISNULL(PMNT.TotalPay,0) + ISNULL(PMNT.TotalAit,0) + ISNULL(PMNT.TotalDiscount,0))) AS DueAmount,LastPayDate FROM tblInvoice AS INV WITH(NOLOCK)"
                +"LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                +"LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
                +"LEFT JOIN dbo.View_CustomerMaster AS VC ON INV.CustomerMasterId = VC.CustomerMasterId "
                +"LEFT JOIN (SELECT InvoiceId,SUM(PD.PaymentAmount) TotalPay,SUM(PD.AIT) TotalAit,SUM(Discount) TotalDiscount "
                +"FROM tblCustPayDetail AS PD WITH(NOLOCK) GROUP BY InvoiceId) AS PMNT ON INV.InvoiceId = PMNT.InvoiceId "
                +"LEFT JOIN (SELECT InvoiceId,SUM(ISNULL(DeliveryNetAmount,0)) AS ActualSales FROM tblInvoiceDetail WITH(NOLOCK) "
                +"WHERE DeliveryStatus IN ('Partial','Full') GROUP BY InvoiceId) AS ACS ON INV.InvoiceId = ACS.InvoiceId "
                +"LEFT JOIN (SELECT InvoiceId,SUM(ISNULL(NetAmount,0) - ISNULL(DeliveryNetAmount,0)) AS ReturnAmount FROM tblInvoiceDetail WITH(NOLOCK) "
                +"WHERE DeliveryStatus IN ('Reject','Partial') GROUP BY InvoiceId) AS RTS ON INV.InvoiceId = RTS.InvoiceId "
                +"LEFT JOIN (SELECT CP.InvoiceId,MAX(CM.PaymentDate) AS LastPayDate FROM tblCustPayDetail AS CP "
                +"LEFT JOIN tblCustomerPay AS CM ON CP.CustPayId = CM.CustPayId GROUP BY CP.InvoiceId) AS  LSP ON INV.InvoiceId = LSP.InvoiceId "
                +"WHERE INV.InvoiceId IS NOT NULL " + parameter + " ORDER BY InvoiceNo DESC ";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
            //string query = "";

            //if (customer == "")
            //{
            //    query = @"SELECT CI.CompanyName,UNT.ComUnitName AS Depot,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode,INV.InvoiceNo,INV.InvoiceDate,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate,CSTMR.Address,"
            //    + " INV.TpGrandTotal AS InvoiceValue,ISNULL((CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END),0) AS SaleValue, "
            //    + " INV.TpGrandTotal-(CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END) AS ReturnValue,ISNULL(CPD.PaymentAmount,0) AS SalesCollection,CPAY.PaymentDate AS CollectionDate,"
            //    + " ISNULL((CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal - ISNULL(CPD.PaymentAmount,0) ELSE ISNULL(CPD.PaymentAmount,0)-0 END),0) AS Dues, ISNULL((CASE WHEN (OP.Total > 0 OR OP.Total IS NULL) THEN OP.Total ELSE 0 END),0) AS OpeningBalance FROM dbo.tblInvoice AS INV "
            //    + " LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId "
            //    + " LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId "
            //    + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId "
            //    + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
            //    + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
            //    + " LEFT JOIN dbo.tblCustPayDetail AS CPD ON INV.InvoiceId = CPD.InvoiceId "
            //    + " LEFT JOIN dbo.tblCustomerPay AS CPAY ON CPD.CustPayId = CPAY.CustPayId "
            //    + " JOIN ( SELECT CSTMR.CustomerMasterId,SUM(ISNULL((CASE WHEN tblInvoice.DeliveryTpGrandTotal > 0 THEN tblInvoice.DeliveryTpGrandTotal - ISNULL(tblInvoice.PaymentAmount,0) ELSE ISNULL(tblInvoice.PaymentAmount,0)-0 END),0)) AS Total FROM tblInvoice "
            //    + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON tblInvoice.CustomerMasterId = CSTMR.CustomerMasterId "
            //    + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON tblInvoice.ComUnitId = UNT.ComUnitId "
            //    + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
            //    + " WHERE tblInvoice.InvoiceDate < '" + Convert.ToDateTime(fromDate) + "' AND DeliveryInvoiceStatus IN ('Partial','Full') " + parameter  +
            //    " GROUP BY CSTMR.CustomerMasterId ) AS OP ON INV.CustomerMasterId = OP.CustomerMasterId WHERE INV.InvoiceDate BETWEEN '" + Convert.ToDateTime(fromDate) + "' AND '" + Convert.ToDateTime(toDate) + "'" + parameter + " ORDER BY INV.InvoiceNo";
            //}
            //else
            //{
            //    query = @"SELECT CI.CompanyName,UNT.ComUnitName AS Depot,CSTMR.CustomerCode, CSTMR.CustomerName,EGI.EmpMasterCode,INV.InvoiceNo,INV.InvoiceDate,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate,CSTMR.Address,"
            //                + " INV.TpGrandTotal AS InvoiceValue,ISNULL((CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END),0) AS SaleValue, "
            //                + " INV.TpGrandTotal-(CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal ELSE 0 END) AS ReturnValue,ISNULL(CPD.PaymentAmount,0) AS SalesCollection,CPAY.PaymentDate AS CollectionDate,"
            //                + " ISNULL((CASE WHEN INV.DeliveryTpGrandTotal > 0 THEN INV.DeliveryTpGrandTotal - ISNULL(CPD.PaymentAmount,0) ELSE ISNULL(CPD.PaymentAmount,0)-0 END),0) AS Dues, ISNULL((CASE WHEN (OP.Total > 0 OR OP.Total IS NULL) THEN OP.Total ELSE 0 END),0) AS OpeningBalance FROM dbo.tblInvoice AS INV "
            //                + " LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId "
            //                + " LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId "
            //                + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId "
            //                + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
            //                + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
            //                + " LEFT JOIN dbo.tblCustPayDetail AS CPD ON INV.InvoiceId = CPD.InvoiceId "
            //                + " LEFT JOIN dbo.tblCustomerPay AS CPAY ON CPD.CustPayId = CPAY.CustPayId "
            //                + " JOIN ( SELECT CSTMR.CustomerMasterId,SUM(ISNULL((CASE WHEN tblInvoice.DeliveryTpGrandTotal > 0 THEN tblInvoice.DeliveryTpGrandTotal - ISNULL(tblInvoice.PaymentAmount,0) ELSE ISNULL(tblInvoice.PaymentAmount,0)-0 END),0)) AS Total FROM tblInvoice "
            //                + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON tblInvoice.CustomerMasterId = CSTMR.CustomerMasterId "
            //                + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON tblInvoice.ComUnitId = UNT.ComUnitId "
            //                + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId "
            //                + " WHERE tblInvoice.InvoiceDate < '" + Convert.ToDateTime(fromDate) + "' AND DeliveryInvoiceStatus IN ('Partial','Full') AND CSTMR.CustomerMasterId IN ('" + customer + "')" + parameter +
            //                " GROUP BY CSTMR.CustomerMasterId ) AS OP ON INV.CustomerMasterId = OP.CustomerMasterId WHERE INV.InvoiceDate BETWEEN '" + Convert.ToDateTime(fromDate) + "' AND '" + Convert.ToDateTime(toDate) + "' AND CSTMR.CustomerMasterId IN ('" + customer + "')" + parameter +" ORDER BY INV.InvoiceNo";
            //}



           
        }
    }
}
