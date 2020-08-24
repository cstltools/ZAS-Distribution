using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerPaymentReportDal
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

        public DataTable LoadCustomerPaymentInfo(string parameter,string fromDate, string toDate)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,EGI.EmpMasterCode AS MIOCode,
                             EGI.EmpName AS MIOName, INV.InvoiceNo, INV.InvoiceDate,CP.PayType,INV.PaymentStatus,CPD.PaymentAmount,CP.PaymentDate,'" + fromDate + "' AS FromDate, '" + toDate + "' AS ToDate FROM dbo.tblCustomerPay AS CP WITH (nolock)"
                             + " LEFT JOIN dbo.tblCustPayDetail AS CPD ON CPD.CustPayId = CP.CustPayId"
                             + " LEFT JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = CPD.InvoiceId"
                             + " LEFT JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId"
                             + " LEFT JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId"
                             + " LEFT JOIN dbo.tblMIOInfo AS MIO ON INV.MiaId = MIO.MIOId"
                             + " LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId"
                             + " LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = CP.CustomerMasterId"
                             + " WHERE DeliveryTpGrandTotal > 0 AND CPD.InvoiceId NOT IN (SELECT CRM.InvoiceId FROM dbo.tblCustomerReturnMaster AS CRM) " + parameter + " ORDER BY CSTMR.CustomerCode ";
           return  aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
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
                            WHERE ID.DeliveryStatus IN ('Full','Partial')" + parameter ;
            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");

             
        }
    }
}
