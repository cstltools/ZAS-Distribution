using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class WhFreezeStockReleaseDal
    {
        private  ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable GetWhFreezeStockData(string warehouseId)
        {
             string query = @"SELECT WSF.WhStoreFreezeId, WSCF.WarehouseId, WSF.ReceiveId,P.ProductCode,WSF.ProductName,WSF.BatchNo,WSF.ExpDate,WSF.StockQty,WSF.StockCondition,WSF.Remarks From dbo.tblWhStoreFreeze AS WSF
                                   INNER JOIN tblWhStockConditionFreeze AS WSCF ON WSF.ReceiveId = WSCF.ReceiveId
                                  INNER JOIN dbo.tblProduct P ON WSF.ProductId = P.ProductId WHERE WSF.StockQty > 0 AND WSCF.WarehouseId = '" + warehouseId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void GetWhInformation(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT WearhouseId, WearhouseCode + ' : ' + WearhouseName AS WearhouseName FROM tblWearhouse WHERE CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", queryStr, "SSIDB");
        }

        public DataTable GetStockReleaseData(int id)
        {
            string query = @"SELECT WSF.WhStoreFreezeId,P.ProductCode,WSF.ProductName,WSF.BatchNo,WSF.ExpDate,WSF.StockQty,WSF.StockCondition,WSF.Remarks From dbo.tblWhStoreFreeze AS WSF
                            INNER JOIN dbo.tblProduct P ON WSF.ProductId = P.ProductId WHERE WSF.WhStoreFreezeId = '" + id + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateWhStoreFreezeStockQuantity(WhStoreFreezeDao aConditionFreezeDao)
        {
            string query = @"UPDATE tblWhStoreFreeze SET StockQty ='" + aConditionFreezeDao.StockQty + "' WHERE WhStoreFreezeId = " + aConditionFreezeDao.WhStoreFreezeId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable GetCentralStoreDataByReceiveId(int receiveId)
        {
            string query = @"SELECT * FROM dbo.tblCentralStore WHERE ReceiveId = '" + receiveId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateCentalStoreQuantity(decimal quantity, int receiveId)
        {
            string query = @"UPDATE tblCentralStore SET Quantity ='" + quantity + "' WHERE ReceiveId = " + receiveId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public void GetWhInfoOnDropDownList(DropDownList ddl)
        {
            string query = @"SELECT WearhouseId, WearhouseCode + ':' + WearhouseName AS WearhouseName  FROM tblWearhouse";
            aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", query, "SSIDB");
        }
    }
}
