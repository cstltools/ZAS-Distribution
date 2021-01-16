using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerDueReportDAL
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

        public DataTable LoadCustomerDueReport(string parameter, string companyId, string toDate)
        {
//            string query = @"SELECT CI.CompanyName, CSTMR.CustomerCode, CSTMR.CustomerName,MKT.MarketName, CSTMR.Address, INV.DeliveryTpGrandTotal AS DueAmount, ISNULL(INV.PaymentAmount,0) AS PreviousPay, (INV.DeliveryTpGrandTotal - ISNULL(INV.PaymentAmount,0)) AS ReceivableDue  FROM dbo.tblInvoice AS INV 
//                            INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = INV.ComUnitId
//                            INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = CU.CompanyId
//                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
//                            LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
//                            WHERE (INV.PaymentStatus IS NULL OR INV.PaymentStatus = 'Partial') AND  Inv.Remarks = 'Due'" + parameter;

//            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,VC.CustomerName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate,EGIM.EmpMasterCode AS MIOCode,
//                            EGIM.EmpName AS MIO,SUM(DeliveryTpGrandTotal)- SUM(ISNULL(PaymentAmount,0)) AS DueAmount FROM dbo.tblInvoice AS INV   
//                            LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId 
//                            LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
//                            LEFT JOIN dbo.View_CustomerMaster AS VC ON INV.CustomerMasterId = VC.CustomerMasterId
//                            LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId  
//                            LEFT JOIN dbo.tblEmpGeneralInfo AS EGIM ON MIO.EmployeeId = EGIM.EmpInfoId  "
//                            + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,VC.CustomerName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate,EGIM.EmpMasterCode,EGIM.EmpName "
//                            + " HAVING SUM(DeliveryTpGrandTotal)- SUM(ISNULL(PaymentAmount,0)) > 0 "
//                            + " ORDER BY CI.CompanyName,UNT.ComUnitName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate DESC";

//            string query = @"SELECT  CI.CompanyName,UNT.ComUnitName,VC.CustomerName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate,TTR.TerritoryName,EGIM.EmpMasterCode AS MIOCode,EGIM.EmpName AS MIO,
//                             SUM(DeliveryTpGrandTotal)- SUM(ISNULL(PaymentAmount,0)) AS DueAmount FROM dbo.tblInvoice AS INV
//                             LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId 
//                             LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
//                             LEFT JOIN dbo.View_CustomerMaster AS VC ON INV.CustomerMasterId = VC.CustomerMasterId
//                             INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = VC.TerritoryId
//                             INNER JOIN dbo.tblMIOInfo AS MIO ON MIO.TerritoryId = VC.TerritoryId
//                             INNER JOIN dbo.tblEmpGeneralInfo AS EGIM ON MIO.EmployeeId = EGIM.EmpInfoId "
//                            + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,VC.CustomerName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate,EGIM.EmpMasterCode,EGIM.EmpName,TTR.TerritoryName "
//                            + " HAVING SUM(DeliveryTpGrandTotal)- SUM(ISNULL(PaymentAmount,0)) > 0 "
//                            + " ORDER BY CI.CompanyName,UNT.ComUnitName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate DESC";



            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,VC.CustomerName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate,INV.MiaId,VC.TerritoryName,MIO.EmpName AS MIO, MIO.EmpMasterCode AS MIOCode,
                             DeliveryTpGrandTotal- (ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0)) AS DueAmount,VC.TermOfPayment,VC.Address,'" + toDate + "' AS UptoDate FROM dbo.tblInvoice AS INV"   
                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId "
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId"
                             + " LEFT JOIN ("
                             + " SELECT InvoiceId,SUM(CPD.PaymentAmount) AS PaymentAmount,SUM(CPD.AIT) AIT, SUM(CPD.Discount) Discount FROM tblCustPayDetail AS CPD "
                             + " LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId"
                             + " WHERE CP.PaymentDate <= '" + toDate + "' GROUP BY InvoiceId "
                            + ") AS PMNT ON PMNT.InvoiceId = inv.InvoiceId "
                            + "LEFT JOIN dbo.View_CustomerMaster AS VC ON INV.CustomerMasterId = VC.CustomerMasterId "
                            + "LEFT JOIN (SELECT MIO.TerritoryId,EGI.EmpName, EGI.EmpMasterCode FROM dbo.tblTerritory AS TTR "
                            + "LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.TerritoryId = TTR.TerritoryId "
                            + "INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId WHERE MIO.CompanyId = " + companyId + " AND MIO.IsActive = 1) AS MIO ON MIO.TerritoryId = VC.TerritoryId "
                            + parameter + " GROUP BY CI.CompanyName,UNT.ComUnitName,VC.CustomerName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate,INV.MiaId,VC.TerritoryName,MIO.EmpName,MIO.EmpMasterCode,VC.TermOfPayment,PMNT.PaymentAmount,VC.Address,PMNT.AIT,PMNT.Discount,DeliveryTpGrandTotal  "
                            + " HAVING DeliveryTpGrandTotal- (ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0)) > 0 "
                            + " ORDER BY CI.CompanyName,UNT.ComUnitName,VC.CustomerCode,VC.MarketName,InvoiceNo,InvoiceDate DESC";


            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadCustomerTotalDueReport(string parameter,string toDate)
        {
            string query = @"SELECT CSTMR.CustomerMasterId, CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,
                             SUM(ISNULL(CASE WHEN INV.DeliveryTpGrandTotal < 0  THEN 0 ELSE INV.DeliveryTpGrandTotal END,0)) AS Receiveable,ISNULL(SUM(PMNT.PaymentAmount),0) AS Collection,SUM(ISNULL(PMNT.AIT,0)) AS AIT,SUM(ISNULL(PMNT.Discount,0)) AS Discount,
                             SUM(DeliveryTpGrandTotal)- SUM((ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0))) AS DUE,'" + toDate + "' AS UptoDate FROM dbo.tblInvoice AS INV "
                            + "LEFT JOIN ( SELECT InvoiceId,SUM(CPD.PaymentAmount) AS PaymentAmount,SUM(CPD.AIT) AIT, SUM(CPD.Discount) Discount FROM tblCustPayDetail AS CPD  "
                            +" LEFT JOIN tblCustomerPay AS CP ON CPD.CustPayId = CP.CustPayId "
                            +" WHERE CP.PaymentDate <= '" +toDate + "' GROUP BY InvoiceId ) AS PMNT ON PMNT.InvoiceId = inv.InvoiceId "
							+" LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId"
							+" LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId"
							+" LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId"
							+" LEFT JOIN dbo.tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId"
                            + " LEFT JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId"
                            + " LEFT JOIN dbo.tblArea AS ARA ON TTR.AreaId = ARA.AreaId" + parameter + " GROUP BY CSTMR.CustomerCode,CSTMR.CustomerName,CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerMasterId having SUM(DeliveryTpGrandTotal)- SUM((ISNULL(PMNT.PaymentAmount,0) + ISNULL(PMNT.AIT,0) + ISNULL(PMNT.Discount,0))) > 0 ORDER BY CSTMR.CustomerCode";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }
    }
}
