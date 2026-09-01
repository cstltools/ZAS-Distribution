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
    public class ChalanDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDataForChalanInfo(ChalanInfo aChalanInfo)
        {
            string insertQuery = @"insert into tblChalanInfo (ChalanId,ChalanDate,ChalanNo,TrackNo,DriverName,FromComUnitCode,FromComUnitName,FromComUnitAddress,ToComUnitCode,ToComUnitName,ToComUnitAddress,TotalValue,TotalVat,GrandTotal,ManufacId,ChalanBy,FromComUnitId,IsDeliver) 
            values (" + aChalanInfo.ChalanId + ",'" + aChalanInfo.ChalanDate + "','" + aChalanInfo.ChalanNo + "','" + aChalanInfo.TrackNo + "','" + aChalanInfo.DriverName + "','" + aChalanInfo.FromComUnitCode + "','" + aChalanInfo.FromComUnitName + "','" + aChalanInfo.FromComUnitAddress + "','" + aChalanInfo.ToComUnitCode + "','" + aChalanInfo.ToComUnitName + "','" + aChalanInfo.ToComUnitAddress + "','" + aChalanInfo.TotalValue + "','" + aChalanInfo.TotalVat + "','" + aChalanInfo.GrandTotal + "','" + aChalanInfo.ManufacId +  "','" + aChalanInfo.ChalanBy + "','" + aChalanInfo.FromComUnitId + "','" + false + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable CheckDetailsExistOrNot(ChalanDetail aDetailDao)
        {
            string query = @"SELECT * FROM tblChalanDetail WHERE ChalanId = '" + aDetailDao.ChalanId +
                "' AND LTRIM(RTRIM(UPPER(ProductCode))) = LTRIM(RTRIM(UPPER('" + aDetailDao.ProductCode + "')))  AND LTRIM(RTRIM(UPPER(BatchNo))) = LTRIM(RTRIM(UPPER('" + aDetailDao.BatchNo +
                "'))) AND DCStoreId = '" + aDetailDao.DCStoreId + "' AND Quantity = " + aDetailDao.Quantity;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveDataForChalanDetail(ChalanDetail aChalanDetail)
        {
            string insertQuery = @"insert into tblChalanDetail (ChalanDetailsId,ProductCode,ProductName,Quantity,BatchNo,UnitPrice,Value,Vat,ValueWVat,ChalanId,DCStoreId) 
            values (" + aChalanDetail.ChalanDetailId + ",'" + aChalanDetail.ProductCode + "','" + aChalanDetail.ProductName + "','" + aChalanDetail.Quantity + "','" + aChalanDetail.BatchNo + "','" + aChalanDetail.UnitPrice + "','" + aChalanDetail.Value + "','" + aChalanDetail.Vat + "','" + aChalanDetail.ValueWVat + "'," + aChalanDetail.ChalanId + ",'" + aChalanDetail.DCStoreId + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool ChalanUpdate(string chalanId)
        {
            string query = @"UPDATE dbo.tblChalanInfo SET IsDeliver='True' WHERE ChalanId='"+chalanId+"'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStoreFreezeDao)
        {
            string insertQuery = @"insert into tblDCStoreFreeze (DCStoreFreezeId,StorageLocation,TotalQuantity,ProductCode,ProductName,PackSize,BatchNo,ExpDate,ReceiveDate,ChalanNo,ChalanDate,StockQty,DamageQty,StockRcvDate,StockCondition,ComUnitId,DCStoreId,ChalanDetailsId) 
            values (" + aDcStoreFreezeDao.DCStoreFreezeId + ",'" + aDcStoreFreezeDao.StorageLocation + "','" + aDcStoreFreezeDao.TotalQuantity + "','" + aDcStoreFreezeDao.ProductCode + "','" + aDcStoreFreezeDao.ProductName + "','" + aDcStoreFreezeDao.PackSize + "','" + aDcStoreFreezeDao.BatchNo + "','" + aDcStoreFreezeDao.ExpDate + "','" + aDcStoreFreezeDao.ReceiveDate + "','" + aDcStoreFreezeDao.ChalanNo + "','" + aDcStoreFreezeDao.ChalanDate + "','" + aDcStoreFreezeDao.StockQty + "','" + aDcStoreFreezeDao.DamageQty + "','" + aDcStoreFreezeDao.StockRcvDate + "','" + aDcStoreFreezeDao.StockCondition + "'," + aDcStoreFreezeDao.ComUnitId + "," + aDcStoreFreezeDao.DCStoreId + "," + aDcStoreFreezeDao.ChalanDetailsId + ")";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }


        public DataTable CheckB2BExistOrNotInDcStore(DCStockNew aDetailDao)
        {
            string query = @"SELECT * FROM tblDCStore WHERE CONVERT(DATE,ReceiveDateTime) = CONVERT(DATE,GETDATE()) AND  ChalanDetailsId = '" + aDetailDao.ChalanDetailsId +
                "' AND LTRIM(RTRIM(UPPER(ProductCode))) = LTRIM(RTRIM(UPPER('" + aDetailDao.ProductCode + "')))  AND LTRIM(RTRIM(UPPER(BatchNo))) = LTRIM(RTRIM(UPPER('" + aDetailDao.BatchNo + "'))) AND TotalQuantity = " + aDetailDao.TotalQuantity;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


       
        public bool DCStockInDALMain(DCStockNew aDcStockNew)
        {
            string query = @"INSERT INTO dbo.tblDCStore " +
       "  ( DCStoreId , " +
         "    StorageLocation , " +
         "    ProductCode , " +
         "    ProductName , " +
         "    PackSize , " +
         "    BatchNo , " +
         "    TotalQuantity , " +
         "    ExpDate , " +
          "    MfgDate , " +
         "    ReceiveDate , " +
         "    ChalanNo , " +
         "    ChalanDate , " +
          "   ComUnitId , " +
         "    StockQty , " +
         "    DamageQty , " +
         "    StockRcvDate , " +
         "    ChalanDetailsId , " +      
          "    StockCondition ," +
          "    ReceiveDateTime " + "   ) " +
        "   VALUES  ( '" + aDcStockNew.DCStoreId + "' , " +
        "     '" + aDcStockNew.StorageLocation + "' , " +
        "     '" + aDcStockNew.ProductCode + "' ,  " +
        "    '" + aDcStockNew.ProductName + "' , " +
        "    '" + aDcStockNew.PackSize + "' , " +
        "    '" + aDcStockNew.BatchNo + "' , " +
         "    '" + aDcStockNew.TotalQuantity + "' ,  " +
         "    '" + aDcStockNew.ExpDate + "' ,  " +
            "    '" + aDcStockNew.mfgdate + "' ,  " +
        "     '" + aDcStockNew.ReceiveDate + "' , " +
        "    '" + aDcStockNew.ChalanNo + "' , " +
        "    '" + aDcStockNew.ChalanDate + "', " +
        "    '" + aDcStockNew.ComUnitId + "' , " +
         "    '" + aDcStockNew.StockQty + "', " +
         "    '" + aDcStockNew.DamageQty + "' , " +
         "   '" + aDcStockNew.StockRcvDate + "' , " +
         "   '" + aDcStockNew.ChalanDetailsId + "' , " +        
           "    'Available', '" + aDcStockNew.ReceiveDateTime + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
        }
        public DataTable DCStoreReport(string reqId)
        {
            string query = @"SELECT tblUnitPrice.UnitPrice,tblUnitPrice.VATAmountPerUnit,DS.MfgDate,DS.ExpDate,tblProduct.ProductName,* FROM dbo.tblDCStore
                            LEFT JOIN dbo.tblChalanDetail ON dbo.tblDCStore.ChalanDetailsId = dbo.tblChalanDetail.ChalanDetailsId
                            LEFT JOIN dbo.tblDCStore DS ON dbo.tblChalanDetail.DCStoreId = DS.DCStoreId
                            LEFT JOIN dbo.tblStockInTransfar ON DS.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
                            LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
                            LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode
                            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblDCStore.ComUnitId = dbo.tblCompanyUnit.ComUnitId 
                            LEFT JOIN dbo.tblUnitPrice ON dbo.tblDCStore.ProductCode = dbo.tblUnitPrice.ProductCode 
                            WHERE tblUnitPrice.IsActive=1 AND tblDCStore.ChalanNo='" + reqId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool DCStockInDAL(DCStockNew aDcStockNew)
        {
            string query = @"INSERT INTO dbo.tblDCStoreFreeze " +
       "  ( DCStoreId , " +
         "    DCStoreFreezeId, " +
         "    ProductCode , " +
         "    ProductName , " +
         "    PackSize , " +
         "    BatchNo , " +
         "    TotalQuantity , " +
         "    ExpDate , " +
         "    ChalanNo , " +
         "    ChalanDate , " +
          "   ComUnitId , " +
         "    StockQty , " +
         "    DamageQty , " +
         "    StockCondition " +
        "   ) " +
        "   VALUES  ( '" + aDcStockNew.DCStoreId + "' , " +
        "     '" + aDcStockNew.DCStoreFreezeId + "' , " +
        "     '" + aDcStockNew.ProductCode + "' ,  " +
        "    '" + aDcStockNew.ProductName + "' , " +
        "    '" + aDcStockNew.PackSize + "' , " +
        "    '" + aDcStockNew.BatchNo + "' , " +
         "    '" + aDcStockNew.TotalQuantity + "' ,  " +
         "    '" + aDcStockNew.ExpDate + "' ,  " +
        "    '" + aDcStockNew.ChalanNo + "' , " +
        "    '" + aDcStockNew.ChalanDate + "', " +
        "    '" + aDcStockNew.ComUnitId + "' , " +
         "    '" + aDcStockNew.StockQty + "', " +
         "    '" + aDcStockNew.DamageQty + "' , " +
       "      'StockInTransfer'  " +
      "     )";
            return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
        }
        public DataTable DCInfoWithDCId(string dcstoreId)
        {
            string query = "SELECT * FROM dbo.tblDCStore WHERE DCStoreId='" + dcstoreId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void LoadManufac(DropDownList aDropDownList)
        {
            string query = @"SELECT * FROM dbo.tblManufacturer ";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "ManufacName", "ManufacId", query, "SSIDB");
        }
        public bool HasProductcode(ChalanDetail aChalanDetail)
        {
            string query = "select * from tblDCStock where ProductCode = '" + aChalanDetail.ProductCode + "' and BatchNo='"+aChalanDetail.BatchNo+"'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    return true;
                }
            }
            return false;
        }

        public DataTable LoadChalanView()
        {
            string query = @"SELECT *  FROM tblChalanInfo ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable ChalanLoadInReceive(string comunitId)
        {
            string query = @"SELECT * FROM dbo.tblChalanInfo
            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblCompanyUnit.ComUnitCode=dbo.tblChalanInfo.ToComUnitCode
            WHERE ComUnitId='"+comunitId+"' AND (IsDeliver='False' OR IsDeliver IS NULL)";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable ChalanLoadInReceiveReport(string comunitId, string param)
        {
            string query = @"SELECT * FROM dbo.tblChalanInfo
            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblCompanyUnit.ComUnitCode=dbo.tblChalanInfo.ToComUnitCode
            WHERE ComUnitId='" + comunitId + "'" + param;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetChalanReceieve(string id)
        {
            string query = @"SELECT tblProduct.ProductName,*,tblChalanInfo.ChalanDate AS ReceiveDate FROM dbo.tblChalanInfo
            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblCompanyUnit.ComUnitCode=dbo.tblChalanInfo.ToComUnitCode
            LEFT JOIN dbo.tblChalanDetail ON dbo.tblChalanInfo.ChalanId = dbo.tblChalanDetail.ChalanId
            LEFT JOIN dbo.tblProduct ON dbo.tblChalanDetail.ProductCode = dbo.tblProduct.ProductCode
            LEFT JOIN dbo.tblDCStore ON dbo.tblChalanDetail.DCStoreId=dbo.tblDCStore.DCStoreId
            WHERE tblChalanInfo.IsDeliver='False' and tblChalanInfo.ChalanId='" + id + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadChalanById(string id)
        {
            string query = @"SELECT * FROM dbo.tblChalanInfo WHERE ChalanId='"+id+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
//        public DataTable ChalanReport(string id)
//        {
//            string query = @"SELECT * FROM dbo.tblChalanInfo
//            LEFT JOIN dbo.tblChalanDetail ON dbo.tblChalanInfo.ChalanId = dbo.tblChalanDetail.ChalanId WHERE ChalanNo='"+id+"'";
//            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
//        }
        public DataTable LoadComunit(string comunitCode)
        {
            string query = @"SELECT * FROM dbo.tblCompanyUnit WHERE ComUnitCode='"+comunitCode+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CheckEntryWithinlastMinute(ChalanInfo aStockInMasterDao)
        {

            string validationQuery = "SELECT 1 FROM tblChalanInfo WHERE ChalanBy IS NOT NULL AND LTRIM(RTRIM(UPPER(ChalanBy))) = LTRIM(RTRIM(UPPER('" + aStockInMasterDao.ChalanBy + "'))) AND DATEDIFF(SECOND, EntryDate, GETDATE()) < 60";
            return aCommonInternalDal.DataContainerDataTable(validationQuery, "SSIDB");

        }
        public ChalanInfo ChalanEditLoad(string ChalanId)
        {
            string query = "select * from tblChalanInfo where ChalanId = '" + ChalanId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ChalanInfo aChalanInfo = new ChalanInfo();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aChalanInfo.ChalanId = Int32.Parse(dataReader["ChalanId"].ToString());
                    aChalanInfo.ChalanDate = Convert.ToDateTime(dataReader["ChalanDate"].ToString());
                    aChalanInfo.ChalanNo = dataReader["ChalanNo"].ToString();
                    aChalanInfo.TrackNo = dataReader["TrackNo"].ToString();
                    aChalanInfo.DriverName = dataReader["DriverName"].ToString();
                    aChalanInfo.ToComUnitCode = dataReader["ToComUnitCode"].ToString();
                    aChalanInfo.ToComUnitName = dataReader["ToComUnitName"].ToString();
                    aChalanInfo.ToComUnitAddress = dataReader["ToComUnitAddress"].ToString();
                    aChalanInfo.FromComUnitCode = dataReader["FromComUnitCode"].ToString();
                    aChalanInfo.FromComUnitName = dataReader["FromComUnitName"].ToString();
                    aChalanInfo.FromComUnitAddress = dataReader["FromComUnitAddress"].ToString();
                    aChalanInfo.TotalValue = Convert.ToDecimal(dataReader["TotalValue"].ToString());
                    aChalanInfo.TotalVat = Convert.ToDecimal(dataReader["TotalVat"].ToString());
                    aChalanInfo.GrandTotal = Convert.ToDecimal(dataReader["GrandTotal"].ToString());
                    
                }
            }
            return aChalanInfo;
        }
         public ChalanDetail ChalanDetailEditLoad(string ChalanDetailId)
        {
            string query = "select * from tblChalanDetail where ChalanDetailId = '" + ChalanDetailId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ChalanDetail aChalanDetail = new ChalanDetail();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aChalanDetail.ChalanDetailId = Int32.Parse(dataReader["ChalanDetailId"].ToString());
                    aChalanDetail.ProductCode = dataReader["ProductCode"].ToString();
                    aChalanDetail.ProductName = dataReader["ProductName"].ToString();
                    aChalanDetail.Quantity = Convert.ToDecimal(dataReader["Quantity"].ToString());
                    aChalanDetail.BatchNo = dataReader["BatchNo"].ToString();
                    aChalanDetail.UnitPrice = Convert.ToDecimal(dataReader["UnitPrice"].ToString());
                    aChalanDetail.Value = Convert.ToDecimal(dataReader["Value"].ToString());
                    aChalanDetail.Vat = Convert.ToDecimal(dataReader["Vat"].ToString());
                    aChalanDetail.ValueWVat = Convert.ToDecimal(dataReader["ValueWVat"].ToString());
                }
            }
            return aChalanDetail;
        }
        
        public bool UpdateaChalanInfo(ChalanInfo aChalanInfo)
        {
            string query = @"UPDATE tblChalanInfo SET ChalanDate='" + aChalanInfo.ChalanDate + "',ChalanNo='" + aChalanInfo.ChalanNo + "',FromComUnitCode='" + aChalanInfo.FromComUnitCode + "',FromComUnitName='" + aChalanInfo.FromComUnitName + "', FromComUnitAddress='" + aChalanInfo.FromComUnitAddress + "',ToComUnitCode='" + aChalanInfo.ToComUnitCode + "',ToComUnitName='" + aChalanInfo.ToComUnitName + "', ToComUnitAddress='" + aChalanInfo.ToComUnitAddress + "', TotalValue='" + aChalanInfo.TotalValue + "',TotalVat='" + aChalanInfo.TotalVat + "',GrandTotal='" + aChalanInfo.GrandTotal + "' WHERE ChalanId=" + aChalanInfo.ChalanId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool UpdateaChalanDetail(ChalanDetail aChalanDetail)
        {
            string query = @"UPDATE tblChalanInfo SET ProductCode='" + aChalanDetail.ProductCode + "',ProductName='" + aChalanDetail.ProductName + "',Quantity='" + aChalanDetail.Quantity + "',BatchNo='" + aChalanDetail.BatchNo + "',UnitPrice='" + aChalanDetail.UnitPrice + "',Value='" + aChalanDetail.Value + "',Vat'" + aChalanDetail.Vat + "',ValueWVat='" + aChalanDetail.ValueWVat + "' WHERE ChalanDetailId=" + aChalanDetail.ChalanDetailId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable LoadProduct(string productId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblProduct where ProductCode='" + productId.Trim() + "' ";
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
        public DataTable GetProductDcStore(string productCode,string comunitId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT ISNULL(VATAmountPerUnit,0)VATAmountPerUnit,ISNULL(VATPercentage,0)VATPercentage,UnitPrice,*,
            tblProduct.ProductCode AS PCode , tblProduct.ProductName AS PName 
            FROM dbo.tblDCStore
            LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode 
            LEFT JOIN dbo.tblCompanyUnit ON dbo.tblDCStore.ComUnitId = dbo.tblCompanyUnit.ComUnitId 
            LEFT JOIN dbo.tblUnitPrice ON dbo.tblDCStore.ProductCode=dbo.tblUnitPrice.ProductCode
            WHERE  ComUnitCode='" + comunitId + "' AND tblDCStore.ProductCode='" + productCode + "' AND tblUnitPrice.IsActive='True' AND StockQty>0 ";
            //tblDCStore.StockInTransfarId is not null AND
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
        public DataTable LoadComUnit(string ComUnitId)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM tblCompanyUnit where ComUnitCode='" + ComUnitId.Trim() + "' ";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTable;
        }

        public DataTable CentralStoreQuantity(CentralStore aCentralStore)
        {
            string query = "select * from tblCentralStore where ProductCode = '" + aCentralStore.ProductCode + "' and BatchNo='" + aCentralStore.BatchNo + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void UpdateDCStockQuantity(string stockId, string Quantity)
        {
            string updateQuery = @"UPDATE tblDCStore SET StockQty='" + Quantity + "' WHERE DCStoreId='" + stockId.Trim() + "'   ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
      
public DataTable ChalanReport(string id)
        {
            string query = @"SELECT tblDCStore.MfgDate,dbo.tblProduct.ProductName,* 
FROM dbo.tblChalanInfo
LEFT JOIN dbo.tblChalanDetail ON dbo.tblChalanInfo.ChalanId = dbo.tblChalanDetail.ChalanId
LEFT JOIN dbo.tblDCStore ON dbo.tblChalanDetail.DCStoreId = dbo.tblDCStore.DCStoreId
LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode 
LEFT JOIN dbo.tblStockInTransfar ON dbo.tblDCStore.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
WHERE tblChalanInfo.ChalanNo='" + id + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ChalanReceiveReport(string comunitId, string parameter)
        {
            string query = @"SELECT DISTINCT CLN.ChalanId,DCS.ChalanNo,DCS.ChalanDate,CLN.FromComUnitName,CLN.ToComUnitName,CLN.DriverName FROM dbo.tblDCStore AS DCS 
                             INNER JOIN dbo.tblChalanInfo AS CLN ON CLN.ChalanNo = DCS.ChalanNo
                             WHERE DCS.ChalanDetailsId IS NOT NULL AND DCS.ComUnitId = '" + comunitId + "' AND (CLN.IsDeliver='True')" + parameter;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable DcChalanReceiveReport(string comunitId, string parameter)
        {
            string query = @"SELECT RQ.ReqId,RQ.ReqNo, RQ.ReqDate,ComUnitName,IssueChalanNo,IssuChalanDate, PickingNo,PickingDate, TruckNo,DriverName  FROM tblRequisition AS RQ
                            WHERE ReceiveIssue IN ('OK') AND ComUnitId  = '" + comunitId + "' " + parameter;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool ProcessWeightedAverage(int dcStoreId)
       {
           var aSqlParameterList = new List<SqlParameter>();

           aSqlParameterList.Add(new SqlParameter("@DCStoreId", dcStoreId));

           return aCommonInternalDal.UpdateAction("sp_Process_WeightedAverage", aSqlParameterList);
       }


        public bool ProcessWeightedAverageFreezeStock(int DCStoreFreezeId)
        {
            var aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@DCStoreFreezeId", DCStoreFreezeId));
            return aCommonInternalDal.UpdateAction("sp_Process_WeightedAverageFreezeStock", aSqlParameterList);
        }

        public bool ProcessWeightedAverageB2BTransfer(int chalanDetailId)
        {
            var aSqlParameterList = new List<SqlParameter>();

           aSqlParameterList.Add(new SqlParameter("@ChalanDetailsId", chalanDetailId));

           return aCommonInternalDal.UpdateAction("sp_Process_WeightedAverageB2BTransfer", aSqlParameterList);
        }
    }
}

