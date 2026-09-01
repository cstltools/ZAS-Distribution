using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class WarehouseStockInDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void GetWhInformation(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT WearhouseId, WearhouseCode + ' : ' + WearhouseName AS WearhouseName FROM tblWearhouse WHERE CompanyId = " + companyId;
            aInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", queryStr, "SSIDB");
        }


        public void LoadSupplierName(DropDownList ddl)
        {
            const string queryStr = @"SELECT * FROM tblSupplierInformation 
                                WHERE SupplierId IN (
                                SELECT SupplierId FROM ZAS_ACCDB..tblChartOfAccounts
                                WHERE SupplierId IS NOT NULL
                                )
                                ORDER BY SupplierName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "SupplierName", "SupplierId", queryStr);
        }

        public void LoadmanufacturerName(DropDownList ddl)
        { 
            string queryStr = "select * from tblManufacturer";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ManufacName", "ManufacId", queryStr);
        }

        public DataTable LoadProductCode(string productCode, string companyId)
        {

            string query = @"SELECT * FROM tblProduct AS PD 
                           WHERE PD.ProductCode = '" + productCode + "' AND PD.CompanyId = '" + companyId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
             
        }


        public string GetNextReturnSerialNo(int comUnitId, DateTime returnDate)
        {
            string date = returnDate.ToString("yyyy-MM-dd");

            string query = @"
        SELECT 
            ISNULL(MAX(TRY_CAST(RIGHT(ReturnCode, 8) AS INT)), 0) + 1 AS NextSerial
        FROM dbo.tblNonTranscationalInvoiceMaster
        WHERE ComUnitId = " + comUnitId + @"
          AND CAST(ReturnDate AS DATE) = '" + date + @"'
          AND ReturnCode LIKE 'RTN-INV-%'
    ";

            DataTable dt = aInternalDal.DataContainerDataTable(query, "SSIDB");

            int nextSerial = 1;

            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["NextSerial"] != DBNull.Value)
            {
                nextSerial = Convert.ToInt32(dt.Rows[0]["NextSerial"]);
            }

            return nextSerial.ToString("00000000");
        }

        public DataTable CheckEntryWithinlastMinute(WarehouseStockInMasterDao aStockInMasterDao)
        {

            string validationQuery = "SELECT 1 FROM tblWHStockInMaster WHERE LTRIM(RTRIM(UPPER(EntryBy))) = LTRIM(RTRIM(UPPER('" + aStockInMasterDao.EntryBy + "'))) AND DATEDIFF(SECOND, EntryDate, GETDATE()) < 60";
            return aInternalDal.DataContainerDataTable(validationQuery, "SSIDB");

        }

        public bool SaveStockInMasterInformation(WarehouseStockInMasterDao aStockInMasterDao)
        {


            string insertQuery = @"INSERT INTO dbo.tblWHStockInMaster
        ( WHStockInMasterID,
          WHStockInCode,
          WarehouseId ,
          WHStockInDate ,
          TotalQuantity ,
          TotalVat ,
          TotalValue ,
          ChallanNo,
          ChallanDate,
          ReferenceNo,
          ReferenceDate,
          Remarks ,
          Status ,   
          EntryBy ,
          EntryDate,
          SupplierId
        )
        VALUES  (   
                                 '" + aStockInMasterDao.WHStockInMasterID + "'," + 
                                 "'" + aStockInMasterDao.WHStockInCode + "'," +
                                 "'" + aStockInMasterDao.WarehouseId + "'," + 
                                 "'" + aStockInMasterDao.WHStockInDate + "'," +
                                 "'" + aStockInMasterDao.TotalQuantity + "'," +
                                 "'" + aStockInMasterDao.TotalVat + "'," +
                                 "'" + aStockInMasterDao.TotalValue + "'," +
                                 "'" + aStockInMasterDao.ChallanNo + "'," +
                                 "'" + aStockInMasterDao.ChallanDate + "'," +
                                 "'" + aStockInMasterDao.ReferenceNo + "'," +
                                 "'" + aStockInMasterDao.ReferenceDate + "'," +
                                 "'" + aStockInMasterDao.Remarks + "'," +
                                 "'" + aStockInMasterDao.Status + "'," +
                                 "'" + aStockInMasterDao.EntryBy + "'," +
                                 "GETDATE(), " + aStockInMasterDao.SupplierId + ")";

            return aInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }


        public int SaveNonTranscationalMaster(tblNonTranscationalInvoiceMasterDao aStockInMasterDao)
        {

            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aStockInMasterDao.InvoiceId));
            aSqlParameterlist.Add(new SqlParameter("@ReturnDate", aStockInMasterDao.ReturnDate));
            aSqlParameterlist.Add(new SqlParameter("@SalesDate", aStockInMasterDao.SalesDate));
            aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aStockInMasterDao.CustomerMasterId));
            aSqlParameterlist.Add(new SqlParameter("@ReturnCode", aStockInMasterDao.ReturnCode));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aStockInMasterDao.ComUnitId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aStockInMasterDao.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@TotalReturn", aStockInMasterDao.TotalReturn));
            aSqlParameterlist.Add(new SqlParameter("@EntryBy", aStockInMasterDao.EntryBy));
            aSqlParameterlist.Add(new SqlParameter("@EntryDate", aStockInMasterDao.EntryDate));
            aSqlParameterlist.Add(new SqlParameter("@TotalValue", aStockInMasterDao.TotalValue));

            string insertQuery = @"INSERT INTO dbo.tblNonTranscationalInvoiceMaster ( InvoiceId,
          ReturnDate,
          ReturnCode,
          SalesDate,
          CustomerMasterId ,
          ComUnitId ,
          CreateBy ,
          CreateDate,
          ActionStatus,
          TotalReturn,TotalValue)
          VALUES  (@InvoiceId,@ReturnDate,@ReturnCode,@SalesDate,@CustomerMasterId,@ComUnitId,@EntryBy,@EntryDate,@ActionStatus,@TotalReturn,@TotalValue)";

            return aInternalDal.SaveDataByInsertCommandById(insertQuery,aSqlParameterlist,"SSIDB");
        }


        public bool SaveNonTranscationalMasterDetail(List<NonTranscationalInvoiceDetailDao> aDetailList)
        {

            bool status = false;

            foreach (NonTranscationalInvoiceDetailDao aDetailDao in aDetailList)
            {
                string insertQuery = @"INSERT INTO dbo.tblNonTranscationalInvoiceDetail
                ( 
                    InvReturnMasterID,ProductId,Batch, ExpDate, MfgDate, Qty, Price, VAT, TotalAmount 
                )
                VALUES  ('" + aDetailDao.InvReturnMasterID + "'," +
                 "'" + aDetailDao.ProductId + "'," +
                 "'" + aDetailDao.Batch + "'," +
                 "'" + aDetailDao.ExpDate + "'," +
                 "'" + aDetailDao.MfgDate + "'," +
                 "'" + aDetailDao.Qty + "'," +
                 "'" + aDetailDao.Price + "'," +
                 "'" + aDetailDao.VAT + "'," +
                 "'" + aDetailDao.TotalAmount + "' " + ")";

                status = aInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
            }

            return status;

        }

        public DataTable CheckDetailsExistOrNot(WharehouseStockInDetailsDao aDetailDao)
        {
            string query = @"SELECT * FROM tblWHStockInDetail WHERE WHStockInMasterID = '" + aDetailDao.WHStockInMasterID +
                "' AND ProductId = '" + aDetailDao.ProductId + "' AND LTRIM(RTRIM(UPPER(Batch))) = LTRIM(RTRIM(UPPER('" + aDetailDao.Batch +
                "'))) AND ExpDate = '" + aDetailDao.ExpDate + "' AND Qty = " + aDetailDao.Qty + " AND TotalAmount = " + aDetailDao.TotalAmount;

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public bool SaveWarehouseStockInDetail(WharehouseStockInDetailsDao aDetailDao)
        {
            string insertQuery = @"INSERT INTO dbo.tblWHStockInDetail
        ( 
          WHStockInMasterID,ProductId,Batch, ExpDate, MfgDate, Qty, Price, VAT, MaterialConversionMasterId,TotalAmount 
        )
        VALUES  ('" + aDetailDao.WHStockInMasterID + "'," + 
                                 "'" + aDetailDao.ProductId + "'," +
                                 "'" + aDetailDao.Batch + "'," +
                                 "'" + aDetailDao.ExpDate + "'," +
                                 "'" + aDetailDao.MfgDate + "'," +
                                 "'" + aDetailDao.Qty + "'," +
                                 "'" + aDetailDao.Price + "'," +
                                 "'" + aDetailDao.VAT + "'," +
                                 "'" + aDetailDao.MaterialConversionMasterId + "'," +
                                 "'" + aDetailDao.TotalAmount + "' " + ")";

            return aInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public Int32 WarehouseStockInCodeId()
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT (isnull(MAX(SUBSTRING(WHStockInCode,5,15)),0)+1) as PKMaxNo FROM dbo.tblWHStockInMaster";
            return Convert.ToInt32(aInternalDal.DataContainerDataTable(query, "SSIDB").Rows[0][0].ToString());
        }

        public DataTable GetWarehouseStockInData()
        {
            string query = @"SELECT * FROM dbo.tblWHStockInMaster AS WSHM 
            WHERE WSHM.Status = 'Posted' OR WSHM.Status = 'Reject'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetWarehouseStockInMasterInfo(string stockInId)
        {
            string query = @"SELECT * FROM dbo.tblWHStockInMaster AS MGM 
                             INNER JOIN tblWearhouse AS WH ON MGM.WarehouseId = WH.WearhouseId WHERE MGM.WHStockInMasterID = '" + stockInId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetWarehouseStockInDetailInfo(string stockInId)
        {
            string query = @"SELECT WHSD.WHStockInDetailID ,
       WHSD.WHStockInMasterID ,
       WHSD.ProductId ,
       WHSD.Batch ,
       WHSD.ExpDate ,
       WHSD.MfgDate ,
       WHSD.Qty AS Quantity ,
       WHSD.Price ,
       WHSD.VAT ,
       WHSD.TotalAmount ,
       PD.ProductId ,
       PD.ProductCode ,
       PD.ProductName ,
       PD.Description ,
       PD.ProductBrandId ,
       PD.PackSizeId ,
       PD.PackSize ,
       PD.ProTypeId ,
       PD.CategoryId ,
       PD.ManufacId ,
       PD.StockUOMId ,
       PD.CaseId FROM dbo.tblWHStockInDetail AS WHSD
            INNER JOIN dbo.tblProduct AS PD ON PD.ProductId = WHSD.ProductId
            WHERE WHSD.WHStockInMasterID =  '" + stockInId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateStockInMasterInfo(WarehouseStockInMasterDao aStockInMasterDao)
        {
            string insertQuery = @"UPDATE dbo.tblWHStockInMaster SET WarehouseId ='"
                + aStockInMasterDao.WarehouseId + "',WHStockInDate = '" + aStockInMasterDao.WHStockInDate
                + "' ,ChallanNo = '" + aStockInMasterDao.ChallanNo
                + "' ,ChallanDate = '" + aStockInMasterDao.ChallanDate
                + "' ,ReferenceNo = '" + aStockInMasterDao.ReferenceNo
                + "' ,ReferenceDate = '" + aStockInMasterDao.ReferenceDate
                + "' ,TotalQuantity = '" + aStockInMasterDao.TotalQuantity
                + "' ,TotalValue = '" + aStockInMasterDao.TotalValue
                + "' ,TotalVat = '" + aStockInMasterDao.TotalVat
                + "' ,Status = '" + aStockInMasterDao.Status 
                + "',UpdateBy ='" + aStockInMasterDao.UpdateBy 
                + "',UpdateDate ='" + aStockInMasterDao.UpdateDate
                + "',Remarks ='" + aStockInMasterDao.Remarks
                + "',SupplierId ='" + aStockInMasterDao.SupplierId
                + "' WHERE WHStockInMasterID = '" + aStockInMasterDao.WHStockInMasterID + "'";

            return aInternalDal.UpdateDataByUpdateCommand(insertQuery, "SSIDB");
        }


        public void DeleteWhStockInDetailById(int stockInId)
        {
            string insertQuery = @"DELETE FROM dbo.tblWHStockInDetail WHERE WHStockInMasterID = '" + stockInId + "'";
            
            aInternalDal.DeleteDataByDeleteCommand(insertQuery, "SSIDB");
        }

        public void DeleteWhStockInInfoById(string stockInId)
        {
            string insertQuery1 = @"DELETE FROM dbo.tblWHStockInMaster WHERE WHStockInMasterID = '" + stockInId + "'";
            aInternalDal.DeleteDataByDeleteCommand(insertQuery1, "SSIDB");

            string insertQuery2 = @"DELETE FROM dbo.tblWHStockInDetail WHERE WHStockInMasterID = '" + stockInId + "'";
            aInternalDal.DeleteDataByDeleteCommand(insertQuery2, "SSIDB");
        }

       
    }
}
