using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public  class OtherStockActionDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void GetWhInformation(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void GetDcInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public DataTable LoadData(int unitID, int companyId)
        {
            string query = @"SELECT F.DCStoreFreezeId,F.DCStoreId,F.ProductCode,F.ProductName,F.BatchNo,F.ExpDate,F.StockQty,INV.InvoiceNo,INV.DelivaryInvoiceNo,
                           (U.UnitPrice*F.StockQty) AS Amount,F.StockCondition,F.Remarks
                           FROM dbo.tblDCStoreFreeze F
                           INNER JOIN dbo.tblUnitPrice U ON F.ProductCode = U.ProductCode
                           INNER JOIN dbo.tblProduct P ON F.ProductCode = P.ProductCode
                           LEFT JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceDetailId = F.InvoiceDetailId
						   LEFT JOIN dbo.tblInvoice AS INV  ON INV.InvoiceId = INVD.InvoiceId
                           WHERE StockQty>0 AND U.IsActive = 1 AND U.ActionStatus = 'Accepted' AND F.ComUnitId ='" + unitID + "'ORDER BY F.ExpDate ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void LoadmanufacturerName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }
        public DataTable LoadStockData(int DCStoreFreezeId)
        {
            string query = @"SELECT StockQty FROM dbo.tblDCStoreFreeze where DCStoreFreezeId='" + DCStoreFreezeId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadStockDCStockData(int DCStoreId)
        {
            string query = @"SELECT StockQty FROM dbo.tblDCStore where DCStoreId='" + DCStoreId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void LoadSC(DropDownList ddl, string userId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            //            string queryStr = @"SELECT * FROM dbo.tblCompanyUnit
            //                                LEFT JOIN dbo.tblUserCompanyUnit ON dbo.tblCompanyUnit.ComUnitId=dbo.tblUserCompanyUnit.CompanyUnitId
            //                                WHERE UserId='"+userId+"'";


            string queryStr = "select ComUnitId, ComUnitName  from tblCompanyUnit WHERE " +
                               " ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId='" + userId.Trim() + "')";


            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void DCLoad(DropDownList aDownList, string userId)
        {
            //string dc = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit";
            //aCommonInternalDal.LoadDropDownValue(aDownList, "Com", "ComUnitId", dc, "SSIDB");
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT * FROM dbo.tblCompanyUnit
                                            LEFT JOIN dbo.tblUserCompanyUnit ON dbo.tblCompanyUnit.ComUnitId=dbo.tblUserCompanyUnit.CompanyUnitId
                                            WHERE UserId='" + userId + "'";

            aInternalDal.LoadDropDownValueWithoutDataBase(aDownList, "ComUnitName", "ComUnitId", queryStr);
        }
        public void DCLoad(DropDownList aDownList)
        {
            string dc = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit";
            aCommonInternalDal.LoadDropDownValue(aDownList, "Com", "ComUnitId", dc, "SSIDB");
//            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
//            string queryStr = @"SELECT * FROM dbo.tblCompanyUnit
//                                            LEFT JOIN dbo.tblUserCompanyUnit ON dbo.tblCompanyUnit.ComUnitId=dbo.tblUserCompanyUnit.CompanyUnitId
//                                            WHERE UserId='" + userId + "'";

          //  aInternalDal.LoadDropDownValueWithoutDataBase(aDownList, "ComUnitName", "ComUnitId", queryStr);
        }
        public void MarketLoad(DropDownList aDownList)
        {
            string dc = "select * from dbo.tblMarket";
            aCommonInternalDal.LoadDropDownValue(aDownList, "MarketName", "MarketId", dc, "SSIDB");
        }
        public bool UpdateDCStoreFreezeStock(decimal StockQty, int DCStoreFreezeId)
        {
            string query = @"UPDATE tblDCStoreFreeze SET StockQty='" + StockQty + "' WHERE DCStoreFreezeId=" + DCStoreFreezeId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool UpdateDCStock(decimal StockQty, int DCStoreId)
        {
            string query = @"UPDATE tblDCStore SET StockQty='" + StockQty + "' WHERE DCStoreId=" + DCStoreId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable GetCheckPermission(string condition, string userId)
        {
            string query = @"SELECT * FROM dbo.tblStockConditionPermission
                            INNER JOIN dbo.tblStockCondition ON tblStockCondition.StockConId = tblStockConditionPermission.StockConId
                            WHERE UserId = " + userId + " AND StockCondition = '" + condition + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool FreezeStockReleasePosting(int dcStoreFreezeId, decimal returnQty)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@FreezeId", dcStoreFreezeId));
            aSqlParameterList.Add(new SqlParameter("@Quantity", returnQty));

            return aCommonInternalDal.UpdateAction("sp_FreezeStockReleasePosting", aSqlParameterList);
        }
    }
}
