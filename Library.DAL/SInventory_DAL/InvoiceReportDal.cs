using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class InvoiceReportDal
    {

        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void LoadManufac(DropDownList ddl)
        {
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public DataTable GetInvoiceData(string parameter)
        {
            const string orderBy = " ORDER BY CSTMR.CustomerCode";

            string query = @"SELECT GroupInvoiceNo, INV.InvoiceId, CUNT.ComUnitCode + '' + CUNT.ComUnitName AS Wings,CSTMR.CustomerCode + ':' + CSTMR.CustomerName AS Customer,INV.InvoiceNo, INV.InvoiceDate,INV.TpGrandTotal,EGI.EmpName AS DeliveryMan FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
                              " + parameter + orderBy;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        
        
        public DataTable GetGroupInvoiceData(string parameter)
        {
            const string orderBy = " ORDER BY CSTMR.CustomerCode";

            string query = @"SELECT GroupInvoiceNo,CSTMR.CustomerCode + ':' + CSTMR.CustomerName AS Customer, CONVERT(DATE,INV.GroupInvoiceDate,107) InvoiceDate,SUM(INV.TpGrandTotal) TpGrandTotal,EGI.EmpName AS DeliveryMan FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             INNER JOIN dbo.tblOrder AS ODR ON INV.OrderId = ODR.OrderId
                              " + parameter + " GROUP BY GroupInvoiceNo,CSTMR.CustomerCode,CSTMR.CustomerName,CONVERT(DATE,INV.GroupInvoiceDate,107),EGI.EmpName " + orderBy;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


       

        public void LoadDC(DropDownList ddl, string companyId)
        {
            string queryStr = "SELECT * FROM dbo.tblCompanyUnit WHERE CompanyId='" + companyId + "' ";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
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
            string dc = @"SELECT * FROM dbo.tblTerritory WHERE AreaId='" + areaId + "'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "TerritoryName", "TerritoryId", dc, "SSIDB");
        }

        public void MarketLoad(DropDownList aDownList, string territoryId)
        {
            string dc = @"SELECT * FROM dbo.tblMarket
            WHERE TerritoryId='" + territoryId + "' AND IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "MarketName", "MarketId", dc, "SSIDB");
        }

        public void LoadCustomer(DropDownList aDownList, string marketId)
        {
            string dc = @"SELECT  CSTMR.CustomerMasterId,CSTMR.CustomerName FROM dbo.tblCustMaster AS CSTMR
                          WHERE CSTMR.MarketId = '" + marketId + "' AND CSTMR.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "CustomerName", "CustomerMasterId", dc, "SSIDB");
        }
        public void LoadDeliveryManList(DropDownList ddl)
        {
            string query = @"SELECT *,EmpName as DeliveryManName FROM dbo.tblDeliveryManInfo
            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId";
            aCommonInternalDal.LoadDropDownValue(ddl, "DeliveryManName", "DeliveryManId", query, "SSIDB");
        }
        
        
        public void LoadBatch(DropDownList ddl,DateTime fromDate, DateTime toDate,int deliveryManId,int userId)
        {
            string query = "";

            if (userId > 0)
            {
                query = @"SELECT BatchId,(CONVERT(varchar, BatchNo) + ' [' + CONVERT(varchar, Date, 101) + ']') AS BatchNo FROM tblInvoiceBatch 
                             WHERE Date BETWEEN '" + fromDate + "' AND '" + toDate + "' AND DeliveryManId = " + deliveryManId + " AND tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            }
            else
            {
                query = @"SELECT BatchId,(CONVERT(varchar, BatchNo) + ' [' + CONVERT(varchar, Date, 101) + ']') AS BatchNo FROM tblInvoiceBatch 
                             WHERE Date BETWEEN '" + fromDate + "' AND '" + toDate + "' AND DeliveryManId = " + deliveryManId;
            }

            aCommonInternalDal.LoadDropDownValue(ddl, "BatchNo", "BatchId", query, "SSIDB");
        }
    }
}
