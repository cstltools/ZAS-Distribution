using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class WebServiceDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetCustomerInfo(string prefixText)
        {
            string query = @"SELECT CSTMR.CustomerName + ' ( Region: ' + ISNULL(RGN.RegionName, '')   + '; Area: ' +  ISNULL(AR.AreaName, '')  + '; Territory:' + ISNULL(TTR.TerritoryName, '')  + '; Market: ' + ISNULL(MKT.MarketName, '')  + ' )' AS CustomerName FROM tblCustMaster AS CSTMR 
                                    LEFT JOIN tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
                                    LEFT JOIN tblTerritory AS TTR On MKT.TerritoryId = TTR.TerritoryId
                                    LEFT JOIN tblArea AS AR On TTR.AreaId = AR.AreaId
                                    LEFT JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId WHERE CSTMR.CustomerName like '%" + prefixText + "%'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetProductInfoByCompany(string prefixText, int companyId)
        {
            string query = @"SELECT PD.ProductId,PD.ProductCode + ':' + PD.ProductName AS PRODUCT FROM tblProduct AS PD 
                           INNER JOIN tblUnitPrice AS PDP ON PD.ProductId = PDP.ProductId
                           WHERE PD.ProductCode + ':' + PD.ProductName LIKE  '%" + prefixText + "%' AND PD.CompanyId = '" + companyId + "' AND PDP.ActionStatus = 'Accepted' AND PDP.IsActive = 1";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerInfoByCode(string prefixText)
        {
            string query = @"SELECT CSTMR.CustomerCode+':'+CSTMR.CustomerName + ':( Region- ' + RGN.RegionName + '; Area - ' + AR.AreaName + '; Territory - ' + TTR.TerritoryName + '; Market - ' +MKT.MarketName +' )'  +':'+CONVERT(NVARCHAR(MAX),CSTMR.CustomerMasterId) AS CustomerName FROM tblCustMaster AS CSTMR 
                                    LEFT JOIN tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
                                    LEFT JOIN tblTerritory AS TTR On MKT.TerritoryId = TTR.TerritoryId
                                    LEFT JOIN tblArea AS AR On TTR.AreaId = AR.AreaId
                                    LEFT JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId WHERE CSTMR.CustomerName like '%" + prefixText + "%'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetMoneyReceipt(string prefixText)
        {
            string query = @"SELECT CONVERT(NVARCHAR,MoneyReceiptId) + ':' + MoneyReceiptNo  AS MoneyReceiptNo FROM tblMoneyReceipt WHERE MoneyReceiptNo like '%" + prefixText + "%'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
