using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class MIOWiseProformaReportDAL
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

        public DataTable LoadCustomerPaymentInfo(string parameter)
        {
            string query = @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,
                            CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,
                            ID.ProductName,ID.PackSize,ID.BatchNo,
                            CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount, C.TerritoryName
                            ,MEGI.EmpName AS MIO
                            FROM dbo.tblInvoice I with(nolock)
                            INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
                            INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
                            INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
                            INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId
                            INNER JOIN dbo.tblOrder AS ORD ON ORD.OrderId = I.OrderId
                            LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = ORD.MIOId
                            LEFT JOIN dbo.tblEmpGeneralInfo AS MEGI ON MIO.EmployeeId = MEGI.EmpInfoId
                            LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = ORD.TerritoryId" + parameter;
           return  aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");

             
        }
    }
}
