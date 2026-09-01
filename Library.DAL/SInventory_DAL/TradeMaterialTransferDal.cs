using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class TradeMaterialTransferDal
    {
        private readonly ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void LoadBusinessUnit(DropDownList ddl, int userId, int moduleId)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@UserId", userId));
            aSqlParameters.Add(new SqlParameter("@ModuleId", moduleId));

            ClsCommonInternalDAL.LoadDropDownListData(ddl, "UnitName", "UnitId", "sp_Common_GetUnitByPermissionByModule", aSqlParameters);
        }

        public void LoadLocation(DropDownList ddl, string companyId)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@CompanyId", companyId));

            ClsCommonInternalDAL.LoadDropDownListData(ddl, "UnitName", "UnitId", "sp_Common_GetUnitWiseLocation_Zas", aSqlParameters);
        }

        public void LoadToLocation(DropDownList ddl, string companyId)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@CompanyId", companyId));

            ClsCommonInternalDAL.LoadDropDownListData(ddl, "UnitName", "UnitId", "sp_Common_GetUnitWiseLocation_FromErp", aSqlParameters);
        }

        

        public void LoadFgByCompany(DropDownList ddl, int companyId)
        {
            const string queryStr = @"SELECT PD.ProductId,
            PD.ProductCode + ' : ' + PD.ProductName AS ProductName
            FROM tblProduct AS PD
            LEFT JOIN tblUnitPrice AS UP ON PD.ProductId = UP.ProductId
            WHERE UP.IsActive = 1
            AND PD.CompanyId = @CompanyId
            ORDER BY PD.ProductName";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@CompanyId", companyId));

            DataTable aDataTable = aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
            ddl.DataSource = aDataTable;
            ddl.DataTextField = "ProductName";
            ddl.DataValueField = "ProductId";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }

        public DataTable GetFgCurrentStock(int productId, int comUnitId)
        {
            const string queryStr = @"SELECT PD.ProductCode, SUM(DCS.StockQty) CurrentStock
            FROM tblDCStore AS DCS
            LEFT JOIN tblProduct AS PD ON DCS.ProductCode = PD.ProductCode
            WHERE PD.ProductId = @ProductId
            AND DCS.ComUnitId = @ComUnitId
            GROUP BY PD.ProductCode
            HAVING SUM(StockQty) > 0";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@ProductId", productId));
            aSqlParameters.Add(new SqlParameter("@ComUnitId", comUnitId));

            return aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
        }

        public DataTable GetFgInfo(int productId)
        {
            const string queryStr = @"SELECT TOP 1 ProductId, ProductCode, ProductName
            FROM tblProduct
            WHERE ProductId = @ProductId";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@ProductId", productId));

            return aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
        }

        public string[] SearchRawMaterial(string prefixText)
        {
            const string queryStr = @"SELECT TOP 15 ItemDescriptionCode + ':' + ItemDescription AS RawMaterial
            FROM CSTL_ERPDB..tblItemDescription
            WHERE ItemDescriptionCode LIKE @PrefixText
            OR ItemDescription LIKE @PrefixText
            ORDER BY ItemDescription";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@PrefixText", "%" + prefixText + "%"));

            DataTable aDataTable = aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
            string[] items = new string[aDataTable.Rows.Count];

            for (int i = 0; i < aDataTable.Rows.Count; i++)
            {
                items[i] = aDataTable.Rows[i]["RawMaterial"].ToString();
            }

            return items;
        }

        public DataTable GetRawMaterialInfo(string rawMaterialText)
        {
            const string queryStr = @"SELECT TOP 1 ItemDescriptionId, ItemDescriptionCode, ItemDescription
            FROM CSTL_ERPDB..tblItemDescription
            WHERE LTRIM(RTRIM(ItemDescriptionCode)) = LTRIM(RTRIM(@RawMaterialText))
            OR LTRIM(RTRIM(ItemDescription)) = LTRIM(RTRIM(@RawMaterialText))
            OR LTRIM(RTRIM(ItemDescriptionCode)) + ':' + LTRIM(RTRIM(ItemDescription)) = LTRIM(RTRIM(@RawMaterialText))";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@RawMaterialText", rawMaterialText));

            return aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
        }

        public DataTable GetRawMaterialCurrentStock(int companyId, int warehouseId, int itemDescriptionId)
        {
            const string queryStr = @"SELECT TOP 1
            ISNULL(Close_Qty, 0) AS CurrentStockQty,
            ISNULL(Avg_Rate, 0) AS AverageRate
            FROM dbo.tbl_INV_WeightedAvg
            WHERE Company_Id = @CompanyId
            AND Warehouse_Id = @WarehouseId
            AND Item_Id = @ItemDescriptionId
            ORDER BY Trans_Date DESC, Weighted_Avg_Id DESC";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@CompanyId", companyId));
            aSqlParameters.Add(new SqlParameter("@WarehouseId", warehouseId));
            aSqlParameters.Add(new SqlParameter("@ItemDescriptionId", itemDescriptionId));

            return aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
        }

        public int InsertTradeMaterialTransfer(TradeMaterialTransferDao aTradeMaterialTransferDao)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@FromCompanyId", aTradeMaterialTransferDao.FromCompanyId));
            aSqlParameters.Add(new SqlParameter("@FromLocationId", aTradeMaterialTransferDao.FromLocationId));
            aSqlParameters.Add(new SqlParameter("@FromProductId", aTradeMaterialTransferDao.FromProductId));
            aSqlParameters.Add(new SqlParameter("@FromProductCode", aTradeMaterialTransferDao.FromProductCode ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@FromProductName", aTradeMaterialTransferDao.FromProductName ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@FromCurrentStockQty", aTradeMaterialTransferDao.FromCurrentStockQty));
            aSqlParameters.Add(new SqlParameter("@ToCompanyId", aTradeMaterialTransferDao.ToCompanyId));
            aSqlParameters.Add(new SqlParameter("@ToLocationId", aTradeMaterialTransferDao.ToLocationId));
            aSqlParameters.Add(new SqlParameter("@ToItemDescriptionId", aTradeMaterialTransferDao.ToItemDescriptionId));
            aSqlParameters.Add(new SqlParameter("@ToItemDescriptionCode", aTradeMaterialTransferDao.ToItemDescriptionCode ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@ToItemDescription", aTradeMaterialTransferDao.ToItemDescription ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@ToCurrentStockQty", aTradeMaterialTransferDao.ToCurrentStockQty));
            aSqlParameters.Add(new SqlParameter("@TransferQty", aTradeMaterialTransferDao.TransferQty));
            aSqlParameters.Add(new SqlParameter("@Remarks", aTradeMaterialTransferDao.Remarks ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@EntryBy", aTradeMaterialTransferDao.EntryBy ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@EntryDate", aTradeMaterialTransferDao.EntryDate));

            return aInternalDal.SaveAction("spInsertTradeMaterialTransfer", aSqlParameters, "@TradeMaterialTransferId");
        }

        public bool UpdateTradeMaterialTransfer(TradeMaterialTransferDao aTradeMaterialTransferDao)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@TradeMaterialTransferId", aTradeMaterialTransferDao.TradeMaterialTransferId));
            aSqlParameters.Add(new SqlParameter("@FromCompanyId", aTradeMaterialTransferDao.FromCompanyId));
            aSqlParameters.Add(new SqlParameter("@FromLocationId", aTradeMaterialTransferDao.FromLocationId));
            aSqlParameters.Add(new SqlParameter("@FromProductId", aTradeMaterialTransferDao.FromProductId));
            aSqlParameters.Add(new SqlParameter("@FromProductCode", aTradeMaterialTransferDao.FromProductCode ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@FromProductName", aTradeMaterialTransferDao.FromProductName ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@FromCurrentStockQty", aTradeMaterialTransferDao.FromCurrentStockQty));
            aSqlParameters.Add(new SqlParameter("@ToCompanyId", aTradeMaterialTransferDao.ToCompanyId));
            aSqlParameters.Add(new SqlParameter("@ToLocationId", aTradeMaterialTransferDao.ToLocationId));
            aSqlParameters.Add(new SqlParameter("@ToItemDescriptionId", aTradeMaterialTransferDao.ToItemDescriptionId));
            aSqlParameters.Add(new SqlParameter("@ToItemDescriptionCode", aTradeMaterialTransferDao.ToItemDescriptionCode ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@ToItemDescription", aTradeMaterialTransferDao.ToItemDescription ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@ToCurrentStockQty", aTradeMaterialTransferDao.ToCurrentStockQty));
            aSqlParameters.Add(new SqlParameter("@TransferQty", aTradeMaterialTransferDao.TransferQty));
            aSqlParameters.Add(new SqlParameter("@Remarks", aTradeMaterialTransferDao.Remarks ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@UpdateBy", aTradeMaterialTransferDao.UpdateBy ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@UpdateDate", aTradeMaterialTransferDao.UpdateDate));

            return aInternalDal.UpdateAction("spUpdateTradeMaterialTransfer", aSqlParameters);
        }

        public string SaveTradeMaterialTransfer(TradeMaterialTransferDao aTradeMaterialTransferDao)
        {
            if (aTradeMaterialTransferDao.TradeMaterialTransferId > 0)
            {
                bool status = UpdateTradeMaterialTransfer(aTradeMaterialTransferDao);
                return status
                    ? "Trade material transfer updated successfully."
                    : "Trade material transfer update failed.";
            }

            int tradeMaterialTransferId = InsertTradeMaterialTransfer(aTradeMaterialTransferDao);

            return tradeMaterialTransferId > 0
                ? "Trade material transfer saved successfully."
                : "Trade material transfer save failed.";
        }

        public DataTable GetTradeMaterialTransferById(int tradeMaterialTransferId)
        {
            const string queryStr = @"SELECT TOP 1 TMT.*,
            FCI.CompanyName AS FromCompanyName,
            FLOC.ComUnitName AS FromLocationName,
            TCI.CompanyName AS ToCompanyName,
            TLOC.UnitName AS ToLocationName
            FROM dbo.tblTradeMaterialTransfer AS TMT
            LEFT JOIN dbo.tblCompanyInfo AS FCI ON TMT.FromCompanyId = FCI.CompanyId
            LEFT JOIN dbo.tblCompanyUnit AS FLOC ON TMT.FromLocationId = FLOC.ComUnitId
            LEFT JOIN dbo.tblCompanyInfo AS TCI ON TMT.ToCompanyId = TCI.CompanyId
            LEFT JOIN CSTL_ERPDB..tblCompanyUnit AS TLOC ON TMT.ToLocationId = TLOC.UnitId
            WHERE TMT.TradeMaterialTransferId = @TradeMaterialTransferId";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@TradeMaterialTransferId", tradeMaterialTransferId));

            return aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
        }

        public DataTable GetTradeMaterialTransferList(string fromDate, string toDate, string fromCompanyId, string toCompanyId, string rawMaterialText)
        {
            const string queryStr = @"SELECT TMT.TradeMaterialTransferId,
            TMT.TransferCode,
            FCI.CompanyName AS FromCompanyName,
            FLOC.ComUnitName AS FromLocationName,
            TMT.FromProductCode,
            TMT.FromProductName,
            TCI.CompanyName AS ToCompanyName,
            TLOC.UnitName AS ToLocationName,
            TMT.ToItemDescriptionCode,
            TMT.ToItemDescription,
            TMT.TransferQty,
            TMT.Remarks,
            TMT.EntryBy,
            TMT.EntryDate,
            TMT.UpdateBy,
            TMT.UpdateDate,
            TMT.ApprovalStatus,
            TMT.ApprovedBy,
            TMT.ApprovedDate
            FROM dbo.tblTradeMaterialTransfer AS TMT
            LEFT JOIN dbo.tblCompanyInfo AS FCI ON TMT.FromCompanyId = FCI.CompanyId
            LEFT JOIN dbo.tblCompanyUnit AS FLOC ON TMT.FromLocationId = FLOC.ComUnitId
            LEFT JOIN dbo.tblCompanyInfo AS TCI ON TMT.ToCompanyId = TCI.CompanyId
            LEFT JOIN CSTL_ERPDB..tblCompanyUnit AS TLOC ON TMT.ToLocationId = TLOC.UnitId
            WHERE (@FromDate = '' OR CONVERT(DATETIME, CONVERT(NVARCHAR(11), TMT.EntryDate, 106)) >= CONVERT(DATETIME, @FromDate))
            AND (@ToDate = '' OR CONVERT(DATETIME, CONVERT(NVARCHAR(11), TMT.EntryDate, 106)) <= CONVERT(DATETIME, @ToDate))
            AND (@FromCompanyId = '' OR CONVERT(NVARCHAR(50), TMT.FromCompanyId) = @FromCompanyId)
            AND (@ToCompanyId = '' OR CONVERT(NVARCHAR(50), TMT.ToCompanyId) = @ToCompanyId)
            AND (@RawMaterialText = ''
                 OR TMT.ToItemDescriptionCode LIKE @RawMaterialLike
                 OR TMT.ToItemDescription LIKE @RawMaterialLike
                 OR TMT.ToItemDescriptionCode + ':' + TMT.ToItemDescription LIKE @RawMaterialLike)
            ORDER BY TMT.TradeMaterialTransferId DESC";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@FromDate", fromDate ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@ToDate", toDate ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@FromCompanyId", fromCompanyId ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@ToCompanyId", toCompanyId ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@RawMaterialText", rawMaterialText ?? string.Empty));
            aSqlParameters.Add(new SqlParameter("@RawMaterialLike", (rawMaterialText ?? string.Empty) + "%"));

            return aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
        }

        public bool DeleteTradeMaterialTransfer(int tradeMaterialTransferId)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@TradeMaterialTransferId", tradeMaterialTransferId));

            return aInternalDal.DeleteAction("spDeleteTradeMaterialTransfer", aSqlParameters);
        }

        public bool ApproveTradeMaterialTransfer(int tradeMaterialTransferId, string approvedBy)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@TradeMaterialTransferId", tradeMaterialTransferId));
            aSqlParameters.Add(new SqlParameter("@ApprovedBy", approvedBy));

            return aInternalDal.UpdateAction("sp_Process_TradeMaterialTransferFullProcess", aSqlParameters);
        }

        public DataTable GetPendingTradeMaterialTransferList()
        {
            const string queryStr = @"SELECT TMT.TradeMaterialTransferId,
            TMT.TransferCode,
            TMT.FromCompanyId, TMT.FromLocationId, TMT.FromProductCode, TMT.FromProductName,
            TMT.ToCompanyId, TMT.ToLocationId, TMT.ToItemDescriptionCode, TMT.ToItemDescription,
            TMT.TransferQty, TMT.Remarks, TMT.EntryBy, TMT.EntryDate, TMT.ApprovalStatus,
            FCI.CompanyName AS FromCompanyName,
            FLOC.ComUnitName AS FromLocationName,
            TCI.CompanyName AS ToCompanyName,
            TLOC.UnitName AS ToLocationName
            FROM dbo.tblTradeMaterialTransfer AS TMT
            LEFT JOIN dbo.tblCompanyInfo AS FCI ON TMT.FromCompanyId = FCI.CompanyId
            LEFT JOIN dbo.tblCompanyUnit AS FLOC ON TMT.FromLocationId = FLOC.ComUnitId
            LEFT JOIN dbo.tblCompanyInfo AS TCI ON TMT.ToCompanyId = TCI.CompanyId
            LEFT JOIN CSTL_ERPDB..tblCompanyUnit AS TLOC ON TMT.ToLocationId = TLOC.UnitId
            WHERE ISNULL(TMT.ApprovalStatus, '') <> 'Approved'
            ORDER BY TMT.TradeMaterialTransferId DESC";

            return aInternalDal.DataContainerDataTable(queryStr, new List<SqlParameter>(), "SSIDB");
        }

        public DataTable GetPendingTradeMaterialTransferListByStatus(string status)
        {
            const string queryStr = @"SELECT TMT.TradeMaterialTransferId,
            TMT.TransferCode,
            TMT.FromCompanyId, TMT.FromLocationId, TMT.FromProductCode, TMT.FromProductName,
            TMT.ToCompanyId, TMT.ToLocationId, TMT.ToItemDescriptionCode, TMT.ToItemDescription,
            TMT.TransferQty, TMT.Remarks, TMT.EntryBy, TMT.EntryDate, TMT.ApprovalStatus,
            FCI.CompanyName AS FromCompanyName,
            FLOC.ComUnitName AS FromLocationName,
            TCI.CompanyName AS ToCompanyName,
            TLOC.UnitName AS ToLocationName
            FROM dbo.tblTradeMaterialTransfer AS TMT
            LEFT JOIN dbo.tblCompanyInfo AS FCI ON TMT.FromCompanyId = FCI.CompanyId
            LEFT JOIN dbo.tblCompanyUnit AS FLOC ON TMT.FromLocationId = FLOC.ComUnitId
            LEFT JOIN dbo.tblCompanyInfo AS TCI ON TMT.ToCompanyId = TCI.CompanyId
            LEFT JOIN CSTL_ERPDB..tblCompanyUnit AS TLOC ON TMT.ToLocationId = TLOC.UnitId
            WHERE ISNULL(TMT.ApprovalStatus, '') = @Status
            ORDER BY TMT.TradeMaterialTransferId DESC";

            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@Status", status ?? string.Empty));

            return aInternalDal.DataContainerDataTable(queryStr, aSqlParameters, "SSIDB");
        }
    }
}
