using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerWiseSalesReportDAL
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

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");

        }

        public void LoadCustomerDropdownList(DropDownList customerDropDownList)
        {
            string queryStr = @"SELECT CustomerMasterId, CustomerCode + ':' + CustomerName AS CustomerName FROM tblCustMaster ORDER BY CustomerName ASC";
            aCommonInternalDAL.LoadDropDownValue(customerDropDownList, "CustomerName", "CustomerMasterId", queryStr, "SSIDB");
        }

        public DataTable LoadCustomerWiseSalesReport(string parameter,string fromDate, string toDate)
        {
            //            string query = @"SELECT CI.CompanyName, CSTMR.CustomerCode, CSTMR.CustomerName,MKT.MarketName, CSTMR.Address, INV.DeliveryTpGrandTotal AS DueAmount, ISNULL(INV.PaymentAmount,0) AS PreviousPay, (INV.DeliveryTpGrandTotal - ISNULL(INV.PaymentAmount,0)) AS ReceivableDue  FROM dbo.tblInvoice AS INV 
            //                            INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = INV.ComUnitId
            //                            INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = CU.CompanyId
            //                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
            //                            LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
            //                            WHERE (INV.PaymentStatus IS NULL OR INV.PaymentStatus = 'Partial') AND  Inv.Remarks = 'Due'" + parameter;

            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,MKT.MarketName,EGI.EmpName AS MIO,CSTMR.CustomerCode,INV.InvoiceNo,INV.InvoiceDate,CSTMR.CustomerName,CSTMR.Address,SUM(INVD.DeliveryNetAmount) Amount, 'ActualSales' AS ValueType,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM tblInvoice AS INV"
                             + " LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId"
                             + " LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId"
                             + " LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId"
                             + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId"
                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON   UNT.CompanyId = CI.CompanyId"
                             + " LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId"
                             + " LEFT JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId"
                             + " LEFT JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId"
                             + " WHERE (INVD.DeliveryStatus = 'Full' OR INVD.DeliveryStatus = 'Partial') " + parameter
                             + " GROUP BY CI.CompanyName,UNT.ComUnitName,MKT.MarketName,EGI.EmpName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceId,INV.InvoiceNo,INV.InvoiceDate,INV.TpGrandTotal,CSTMR.Address" 
                             + " UNION "
                             + " SELECT CI.CompanyName,UNT.ComUnitName,MKT.MarketName,EGI.EmpName AS MIO,CSTMR.CustomerCode,INV.InvoiceNo,INV.InvoiceDate,CSTMR.CustomerName,CSTMR.Address,ISNULL(SUM(INVD.NetAmount-INVD.DeliveryNetAmount),0) Amount, 'Rejected' AS ValueType,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDat FROM tblInvoice AS INV "
                             + " LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId "
                             + " LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId  "
                             + " LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId "
                             + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId "
                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON   UNT.CompanyId = CI.CompanyId "
                             + " LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId  "
                             + " LEFT JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId  "
                             + " LEFT JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId  "
                             + " WHERE (INVD.DeliveryStatus = 'Reject' OR INVD.DeliveryStatus = 'Partial')  " + parameter
                              + " GROUP BY CI.CompanyName,UNT.ComUnitName,MKT.MarketName,EGI.EmpName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceId,INV.InvoiceNo,INV.InvoiceDate,CSTMR.Address   "
                              + " UNION "
                              + " SELECT CI.CompanyName,UNT.ComUnitName,MKT.MarketName,EGI.EmpName AS MIO,CSTMR.CustomerCode,INV.InvoiceNo,INV.InvoiceDate,CSTMR.CustomerName,CSTMR.Address,SUM(CPD.PaymentAmount) AS PaidAmount,'PaymentValue' AS ValueType,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDat FROM dbo.tblCustPayDetail AS CPD   "
                              + " LEFT JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId   "
                              + " LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId  "
                             + " LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId "
                              + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId   "
                              + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId   "
                              + " LEFT JOIN dbo.tblCompanyInfo AS CI ON   UNT.CompanyId = CI.CompanyId  "
                              + " LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId  "
                              + " LEFT JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId  "
                              + " LEFT JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId  "
                              + " WHERE (INV.DeliveryInvoiceStatus != 'Reject')   " + parameter
                              + " GROUP BY CI.CompanyName,UNT.ComUnitName,MKT.MarketName,EGI.EmpName,CSTMR.CustomerCode,CSTMR.CustomerName,INV.InvoiceId,INV.InvoiceNo,INV.InvoiceDate,CSTMR.Address";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadCustomerTotalDueReport(string parameter)
        {
            string query = @"SELECT CSTMR.CustomerMasterId, CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,SUM(ISNULL(INV.DeliveryTpGrandTotal,0) - ISNULL(INV.PaymentAmount,0)) AS DUE FROM dbo.tblInvoice AS INV 
							 LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
							 LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
							 LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
							 LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
							 LEFT JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId
							 LEFT JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId" + parameter + " GROUP BY CSTMR.CustomerCode,CSTMR.CustomerName,CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerMasterId having(SUM(ISNULL(INV.DeliveryTpGrandTotal,0) - ISNULL(INV.PaymentAmount,0))) > 0 ORDER BY CSTMR.CustomerCode";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
