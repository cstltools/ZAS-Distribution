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
    
   public class RequisitionDAL
    {
       ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

       public void WareHouseLoad(DropDownList aDownList)
       {
           string wareHouse = "select * from tblWearhouse";
           aCommonInternalDal.LoadDropDownValue(aDownList, "WearhouseName", "WearhouseId", wareHouse, "SSIDB");
       }

       public void GetCompanyDropdownList(DropDownList ddl)
       {
           const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
           aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
       }

       public void GetDcInformation(DropDownList ddl, string companyId)
       {
           string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId ;
           aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
       }

       public void GetWhInformationByUser(DropDownList ddl, string companyId, string userId)
       {
           string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
           aCommonInternalDal.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
       }

       public void GetWhInformation(DropDownList ddl, string companyId)
       {
           string queryStr = @"SELECT WearhouseId, WearhouseCode + ' : ' + WearhouseName AS WearhouseName FROM tblWearhouse WHERE CompanyId = " + companyId;
           aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", queryStr, "SSIDB");
       }

       public void GetWhInformationDC(DropDownList ddl, string companyId)
       {
           string queryStr = @"SELECT DC.ComUnitId AS WearhouseId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS WearhouseName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId;
           aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", queryStr, "SSIDB");
       }

       public void LoadmanufacturerName(DropDownList ddl)
       {
           ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
           string queryStr = "select * from tblManufacturer";
           aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ManufacName", "ManufacId", queryStr);
       }
       public void DCLoad(DropDownList aDownList)
       {
           string dc = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit";
           aCommonInternalDal.LoadDropDownValue(aDownList, "Com", "ComUnitId", dc, "SSIDB");
       }

       public void DCLoad(DropDownList ddl, string companyId)
       {
           string queryStr = @"SELECT ComUnitId, ComUnitCode + ' : ' + ComUnitName AS UnitName FROM tblCompanyUnit WHERE DcTypeId = 2 AND CompanyId = " + companyId;
           aCommonInternalDal.LoadDropDownValue(ddl, "UnitName", "ComUnitId", queryStr, "SSIDB");
       }

       public void AllDCLoad(DropDownList ddl, string companyId)
       {
           string queryStr = @"SELECT ComUnitId, ComUnitCode + ' : ' + ComUnitName AS UnitName FROM tblCompanyUnit WHERE CompanyId = " + companyId;
           aCommonInternalDal.LoadDropDownValue(ddl, "UnitName", "ComUnitId", queryStr, "SSIDB");
       }

       public bool SaveReuqisitionDAL(Requesition aRequesition)
       {
           string query = @" INSERT INTO dbo.tblRequisition "+
     "   ( ReqId , "+
       "    ReqNo , "+
       "    ReqDate , "+
       "    WarehouseId , "+
       "    WearhouseName , "+
      "     ComUnitId , "+
      "     ComUnitCode , " +

       "     CompanyId , " +
        "     EntryBy , " +
         "     EntryDate , " +


       "    ComUnitName " +
      
     "    ) "+
 "   VALUES  ( " + aRequesition.ReqId + " , " +
      "     '" + aRequesition.ReqNo + "' , " +
      "     '" + aRequesition.ReqDate + "' , " +
      "      " + aRequesition.WarehouseId + " , " +
     "       '" + aRequesition.WearhouseName + "' , " +
     "       " + aRequesition.ComUnitId + ", " +
     "      '" + aRequesition.ComUnitCode + "' , " +

         "      " + aRequesition.CompanyId + " , " +
           "      '" + aRequesition.EntryBy + "' , " +
             "      '" + aRequesition.EntryDate + "' , " +
     "       '" + aRequesition.ComUnitName + "' " +
    "     )";


           return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
       }
       public bool DeleteRequisition(string reqId)
       {
           string query = @"DELETE FROM dbo.tblRequisition WHERE ReqId='" + reqId + "'  DELETE FROM dbo.tblRequsitionChild WHERE ReqId='" + reqId + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }

       public bool SaveReuqisitionChildDAL(RequsitionChild aRequsitionChild)
       {
           string query = @" INSERT INTO dbo.tblRequsitionChild "+
      "  ( ReqChildId , "+
        "   ProductCode , "+
        "   ProductName , "+
         "   PackSize , " +
        "   ReqQty , "+
        "   ReqId "+
      "   ) "+
   "   VALUES  ( " + aRequsitionChild.ReqChildId + " , " +
     "      '" + aRequsitionChild.ProductCode + "' , " +
     "      '" + aRequsitionChild.ProductName + "', " +
      "      '" + aRequsitionChild.PackSize + "', " +
     "     " + aRequsitionChild.ReqQty + " , " +
     "      " + aRequsitionChild.ReqId + " " +
     "    )";
           return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
       }
       public DataTable GetAllNonSubmitReqDAL()
       {
           string query = @"select * from tblRequisition LEFT JOIN tblCompanyInfo ON tblRequisition.CompanyId = tblCompanyInfo.CompanyId  where (Submit is null or Submit = '') and (CreatePicking is not null or CreatePicking != '')";
           return aCommonInternalDal.DataContainerDataTable(query,"SSIDB");
       }
       public DataTable GetAllNonPickReqDAL()
       {
           const string query = @"SELECT * FROM tblRequisition AS REQ
                                   INNER JOIN tblCompanyInfo AS CI ON REQ.CompanyId = CI.CompanyId 
                                   INNER JOIN tblCompanyUnit AS CU ON REQ.ComUnitId = CU.ComUnitId
                                   where CreatePicking is null or CreatePicking = ''";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public DataTable GetRequisitionView()
       {
           string query = @"SELECT ReqNo,ReqDate,WearhouseName,ComUnitCode,ComUnitName,SUM(ReqQty)AS Qty,dbo.tblRequisition.ReqId FROM dbo.tblRequisition
LEFT JOIN dbo.tblRequsitionChild ON dbo.tblRequisition.ReqId = dbo.tblRequsitionChild.ReqId
 WHERE CreatePicking IS NULL GROUP BY ReqNo,ReqDate,WearhouseName,ComUnitCode,ComUnitName,tblRequisition.ReqId
";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public DataTable GetRequisitionDetailByReqIdDAL(string reqId)
       {
           //string query = @"SELECT RC.*,ISNULL(VCS.TotalCurrentStockQty,0) AS CurrentStockQty , ISNULL(U.UnitPrice,0) AS ProductUnitPrice FROM dbo.tblRequsitionChild RC " +
           //                 " LEFT JOIN View_CentralStoreCurrentStock VCS ON RC.ProductCode=VCS.ProductCode "+
           //                 " LEFT JOIN dbo.tblUnitPrice U ON RC.ProductCode=U.ProductCode " +
           //                  " where ReqId = '" + reqId + "'";
           string query = @"SELECT * FROM dbo.tblStockInTransfar WHERE ReqId = '" + reqId + "'";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public DataTable GetPickingDataForIssueDAL(string reqId, string warehouseId)
       {
           string query = @"SELECT RC.*,ISNULL(VCS.TotalCurrentStockQty,0) AS CurrentStockQty , ISNULL(U.UnitPrice,0) AS ProductUnitPrice FROM dbo.tblRequsitionChild RC  
                            LEFT JOIN View_CentralStoreCurrentStock VCS ON RC.ProductCode = VCS.ProductCode  
                            LEFT JOIN dbo.tblUnitPrice U ON RC.ProductCode = U.ProductCode  
                            where VCS.StockCondition = 'Available' AND ReqId = '" + reqId + "' AND  U.IsActive != 'False' AND U.ActionStatus = 'Accepted' AND VCS.WarehouseId = '" + warehouseId + "'";
           
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       public DataTable GetRequisitionInfoByReqIdDAL(string reqId)
       {
           string query = @"SELECT * FROM tblRequisition AS RQ
                            INNER JOIN tblCompanyInfo AS CI ON RQ.CompanyId = CI.CompanyId
                            INNER JOIN tblWearhouse AS WR ON RQ.WarehouseId = WR.WearhouseId WHERE RQ.ReqId = '" + reqId + "'";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       public DataTable GetRequisitionInfofromTransferDAL(string reqId)
       {
           string query = @"SELECT ReqId FROM tblStockInTransfar " +

                             " where ReqId = '" + reqId + "'";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       public DataTable ChallanExistsDAL(string reqId)
       {
           string query = @"SELECT Submit FROM tblRequisition  " +

                            // " where Submit = '" + "OK" + "'"; and 

                                   " where Submit = 'OK' and ReqId = '" + reqId + "'";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       public bool UpdateIssueInformationOnRequisitionDAL(Requesition aRequesition)
       {
           string query = @"UPDATE tblRequisition SET  Submit='" + aRequesition.Submit + "',SubmitDate='" + aRequesition.SubmitDate + "',IssueChalanNo='" + aRequesition.IssueChalanNo + "',IssuChalanDate='" + aRequesition.IssuChalanDate + "', " +
                           " TruckNo='" + aRequesition.TruckNo + "',DriverName='" + aRequesition.DriverName + "',TotalPrice='" + aRequesition.TotalPrice + "',TotalVAT='" + aRequesition.TotalVAT + "' ," +
                           " GrandTotalPrice='" + aRequesition.GrandTotalPrice + "' WHERE ReqId='" + aRequesition.ReqId + "'";
          

           return aCommonInternalDal.UpdateDataByUpdateCommand(query,"SSIDB");
       }

       public bool UpdatePickingInformationOnRequisitionDAL(Requesition aRequesition)
       {
           string query = @"UPDATE tblRequisition SET  CreatePicking='" + aRequesition.CreatePicking + "',PickingDate='" + aRequesition.PickingDate + "',PickingNo='" + aRequesition.PickingNo + "', " +
                             " TruckNo='" + aRequesition.TruckNo + "',DriverName='" + aRequesition.DriverName + "',TotalPrice='" + aRequesition.TotalPrice + "',TotalVAT='" + aRequesition.TotalVAT + "' ," +
                             " GrandTotalPrice='" + aRequesition.GrandTotalPrice + "' WHERE ReqId='" + aRequesition.ReqId + "'";
          

           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }

       public bool UpdateIssueInformationOnRequisitionChildDAL(RequsitionChild aRequsitionChild)
       {
           string query = @" UPDATE tblRequsitionChild SET IssueQty='" + aRequsitionChild.IssueQty + "',UnitPrice='" + aRequsitionChild.UnitPrice + "',PriceAmount='" + aRequsitionChild.PriceAmount + "', " +
                          " VATAmount='" + aRequsitionChild.VATAmount + "',TotalPrice='" + aRequsitionChild.TotalPrice + "',IsPicking='" + aRequsitionChild.IsPicking + "',CaseQty='" + aRequsitionChild.CaseQty + "',MusakVATAmount='" + aRequsitionChild.MusakVATAmount + "',MusakTotalPrice='" + aRequsitionChild.MusakTotalPrice + "'   WHERE ReqChildId='" + aRequsitionChild.ReqChildId + "'";

           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }

       public DataTable GetCurrentStockofCentralStock(string productCode)
       {
           string query = @" SELECT * FROM dbo.tblCentralStore WHERE ProductCode='" + productCode.Trim() + "' AND Quantity>0 ORDER BY ExpDate ";
           return aCommonInternalDal.DataContainerDataTable(query,"SSIDB");
       }

       public bool UpdateCentralStockStockOut(decimal quantity, string receiveId)
       {
           string query = @"UPDATE dbo.tblCentralStore SET Quantity='" + quantity + "' WHERE ReceiveId='" + receiveId + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }

       public bool StockInTransfarInsertDAL(StockInTransfar aStockInTransfar)
       {

           string query = @"INSERT INTO dbo.tblStockInTransfar "+
    "    ( StockInTransfarId , "+
        "    ReqId , "+
        "    ReqChildId , "+
        "    ProductCode , "+
        "    ProductName , "+
         "   PackSize , "+
          "   BatchNo , "+
          "  Quantity , "+
         "   UnitPrice , "+
         "   PriceAmount , "+
         "   VATAmount , "+
          "  TotalPriceAmount , "+
          "  ExpDate , "+
          "  MfgDate , " +
         "   ReceiveDate ,"+
         "   PickingQty,ReceiveId " +
        "  ) "+
      " VALUES  ( '" + aStockInTransfar.StockInTransfarId + "' , " +
        "    '" + aStockInTransfar.ReqId + "'  ,  " +
        "    '" + aStockInTransfar.ReqChildId + "' ,  " +
       "     '" + aStockInTransfar.ProductCode + "'  ,  " +
       "    '" + aStockInTransfar.ProductName + "'  ,  " +
       "     '" + aStockInTransfar.PackSize + "'  , " +
       "    '" + aStockInTransfar.BatchNo + "' , " +
       "     '" + aStockInTransfar.Quantity + "'  ,  " +
       "     '" + aStockInTransfar.UnitPrice + "'  ,  " +
        "    '" + aStockInTransfar.PriceAmount + "'  ,  " +
        "    '" + aStockInTransfar.VATAmount + "'  ,  " +
         "   '" + aStockInTransfar.TotalPriceAmount + "'  ,  " +
        "    '" + aStockInTransfar.ExpDate + "'  , " +
          "    '" + aStockInTransfar.MfgDate + "'  , " +
        "    '" + aStockInTransfar.ReceiveDate + "'   , " +
         "    '" + aStockInTransfar.PickingQty + "'  ,  " +
         "    '" + aStockInTransfar.ReceiveId + "'    " +
       "   )";

           return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
       }

       public bool ReqDetailUpdateAfterIssue(int rcId, decimal totalQuantity, decimal totalPriceAmount, decimal totalVATAmount, decimal totalPriceAmountSum)
       {
           string query = @"UPDATE dbo.tblRequsitionChild SET IssueQty='" + totalQuantity + "',PriceAmount='" + totalPriceAmount + "',VATAmount='" + totalVATAmount + "',TotalPrice='" + totalPriceAmountSum + "' WHERE ReqChildId='" + rcId.ToString() + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }
       public bool UpdateStockTransfarInfoUpdateDAL(StockInTransfar aStockInTransfar)
       {

           string query = @"UPDATE dbo.tblStockInTransfar SET Quantity='" + aStockInTransfar.Quantity + "', PriceAmount='" + aStockInTransfar.PriceAmount + "',VATAmount='" + aStockInTransfar.VATAmount + "', TotalPriceAmount='" + aStockInTransfar.TotalPriceAmount + "', IsIssue='" + aStockInTransfar.IsIssue + "' WHERE StockInTransfarId='" + aStockInTransfar.StockInTransfarId + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }

       public bool UpdateReqDetailIssueStatusDAL(StockInTransfar aStockInTransfar)
       {
           string query = @"UPDATE dbo.tblRequsitionChild SET IsIssue='OK' WHERE ReqChildId='" + aStockInTransfar.ReqChildId + "' ";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }

       public DataTable GetAllStockRcvByDcDAL(string comUnitId)
       {
           string query = @"SELECT * FROM dbo.tblRequisition WHERE Submit='OK' AND (ReceiveIssue IS NULL OR ReceiveIssue ='') AND ComUnitId='" + comUnitId + "'";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public DataTable GetStockInTransfarByReqIdDAL(string reqId)
       {
           string query = @"SELECT * FROM dbo.tblStockInTransfar WHERE IsTransfared is null and ReqId='" + reqId.Trim() + "'";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public bool DCStockInDAL(DCStockNew aDcStockNew)
       {
           string query = @"INSERT INTO dbo.tblDCStore "+
      "  ( DCStoreId , "+
        "    StorageLocation , "+
        "    ProductCode , "+
        "    ProductName , "+
        "    PackSize , "+
        "    BatchNo , "+
        "    TotalQuantity , "+
        "    ExpDate , "+
          "    MfgDate , " +
        "    ReceiveDate , "+
        "    ChalanNo , "+
        "    ChalanDate , "+
         "   ComUnitId , "+
        "    StockQty , "+
        "    DamageQty , "+
        "    StockRcvDate , "+
        "    ReqId , "+
        "    ReqChildId , "+
         "    StockCondition, " +
        "    StockInTransfarId, "+
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
        "   '" + aDcStockNew.ReqId + "' , " +
       "    '" + aDcStockNew.ReqChildId + "', " +
          "    'Available', " +
      "      '" + aDcStockNew.StockInTransfarId + "' , '" + aDcStockNew.ReceiveDateTime + "')";
           return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
       }

       public bool StockInTransfarStatusUpdate(string stockInTransfarId)
       {
           string query = @"UPDATE dbo.tblStockInTransfar SET IsTransfared='OK' WHERE StockInTransfarId='" + stockInTransfarId.Trim() + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }
       public bool UpdateReceiveIssueStatus(string reqId,DateTime rcvDate)
       {
           string query = @"UPDATE dbo.tblRequisition SET ReceiveIssue='OK',ReceiveIssueDate='" + rcvDate + "' WHERE ReqId='" + reqId.Trim() + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }


       //Report Submit='OK' AND SubmitDate='" + date + "' order by ReqId desc";
       public DataTable StockTransportOrderGridDataDAL(DateTime date)
       {
           string query = @"SELECT * FROM dbo.tblRequisition WHERE  ReqDate='" + date + "' order by ReqId desc";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       public DataTable ChallanReportDAL(DateTime date)
       {
           string query = @"SELECT * FROM dbo.tblRequisition WHERE  Submit='OK' AND SubmitDate='" + date + "' order by ReqId desc";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public bool SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStoreFreezeDao)
       {
           string insertQuery = @"insert into tblDCStoreFreeze (DCStoreFreezeId,StorageLocation,TotalQuantity,ProductCode,ProductName,PackSize,BatchNo,ExpDate,ReceiveDate,ChalanNo,ChalanDate,StockQty,DamageQty,StockRcvDate,StockCondition,ComUnitId,DCStoreId) 
            values (" + aDcStoreFreezeDao.DCStoreFreezeId + ",'" + aDcStoreFreezeDao.StorageLocation + "','" + aDcStoreFreezeDao.TotalQuantity + "','" + aDcStoreFreezeDao.ProductCode + "','" + aDcStoreFreezeDao.ProductName + "','" + aDcStoreFreezeDao.PackSize + "','" + aDcStoreFreezeDao.BatchNo + "','" + aDcStoreFreezeDao.ExpDate + "','" + aDcStoreFreezeDao.ReceiveDate + "','" + aDcStoreFreezeDao.ChalanNo + "','" + aDcStoreFreezeDao.ChalanDate + "','" + aDcStoreFreezeDao.StockQty + "','" + aDcStoreFreezeDao.DamageQty + "','" + aDcStoreFreezeDao.StockRcvDate + "','" + aDcStoreFreezeDao.StockCondition + "'," + aDcStoreFreezeDao.ComUnitId + "," + aDcStoreFreezeDao.DCStoreId  + ")";

           return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
       }
       ///////////////////////////////////////////////////////////////////////////////
       public DataTable DCStoreReport(string reqId)
       {
           string query = @"SELECT tblUnitPrice.UnitPrice,tblUnitPrice.VATAmountPerUnit,tblProduct.ProductName,* FROM dbo.tblDCStore
LEFT JOIN dbo.tblChalanDetail ON dbo.tblDCStore.ChalanDetailsId = dbo.tblChalanDetail.ChalanDetailsId
LEFT JOIN dbo.tblDCStore DS ON dbo.tblChalanDetail.DCStoreId = DS.DCStoreId
LEFT JOIN dbo.tblStockInTransfar ON DS.StockInTransfarId = dbo.tblStockInTransfar.StockInTransfarId
LEFT JOIN dbo.tblCentralStore ON dbo.tblStockInTransfar.ReceiveId = dbo.tblCentralStore.ReceiveId
--LEFT JOIN dbo.tblMIGODetail ON dbo.tblCentralStore.MigoDetailID = dbo.tblMIGODetail.MigoDetailID
LEFT JOIN dbo.tblProduct ON dbo.tblDCStore.ProductCode = dbo.tblProduct.ProductCode
LEFT JOIN dbo.tblCompanyUnit ON dbo.tblDCStore.ComUnitId = dbo.tblCompanyUnit.ComUnitId 
LEFT JOIN dbo.tblUnitPrice ON dbo.tblDCStore.ProductCode = dbo.tblUnitPrice.ProductCode 
WHERE tblUnitPrice.IsActive=1 AND tblDCStore.ReqId='" + reqId + "'";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       public DataTable GetStockInTransfer(string reqId)
       {
           string query = @"SELECT * FROM dbo.tblStockInTransfar WHERE ReqId='" + reqId + "'";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

        ///////////////////////////////////////////////////////////////////////////////
       public void GetDcInformationByUser(DropDownList ddl, string companyId, string userId)
       {
           string queryStr = @"SELECT ComUnitId, ComUnitCode + ' : ' + ComUnitName AS UnitName FROM tblCompanyUnit WHERE CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
           aCommonInternalDal.LoadDropDownValue(ddl, "UnitName", "ComUnitId", queryStr, "SSIDB");
       }

       public DataTable CheckDuplicateEntry(int? id)
       {
           string query = @"SELECT * FROM tblDCStore WHERE ReceiveDateTime BETWEEN DATEADD(MINUTE,-5,GETDATE()) AND  DATEADD(MINUTE,5,GETDATE()) AND (StockInTransfarId = " + id + " OR ChalanDetailsId = " + id + ")";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public bool UpdateTempReceiveId()
       {
           var aSqlParameterList = new List<SqlParameter>();

           return aCommonInternalDal.UpdateAction("sp_UD_ReceiveIdInDc", aSqlParameterList);
       }
    }
}
