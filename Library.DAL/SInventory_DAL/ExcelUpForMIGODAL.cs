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
   public  class ExcelUpForMIGODAL
    {
       private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
       public bool XLDataGridToDbByRow(String ShipToParty, String PONo, DateTime PODate, String ItemNo, String OrderDocNo,
                                                         DateTime OrderDocDate, String DeliveryDocNo, DateTime DeliveryDocDate, String LMID, String LMIDDescription, String Batch, DateTime ExpDate, DateTime MfgDate, decimal Qty, String VATChallan,
                                                        String BilltoParty, String InvoiceNo, DateTime InvoiceDate, String CaseNoofShipper, decimal VAT, decimal Amount, decimal Total, String TransportNo, int MigoMasterID)
       {
           string insertQuery = @"insert into tblMIGODetail ( MigoMasterID, PONo, PODate, ItemNo, OrderDocNo,OrderDocDate, DeliveryDocNo, DeliveryDocDate, LMID, LMIDDescription, Batch, ExpDate, MfgDate, Qty, VATChallan,BilltoParty, InvoiceNo, InvoiceDate, CaseNoofShipper, VAT, Amount, Total, TransportNo,ShipToParty) 
            values (" + MigoMasterID + ",'" + PONo + "','" + PODate + "','" + ItemNo + "','" + OrderDocNo + "','" + OrderDocDate + "','" + DeliveryDocNo + "','" + DeliveryDocDate + "','" + LMID + "','" + LMIDDescription + "','" + Batch + "','" + ExpDate + "','" + MfgDate + "','" + Qty + "','" + VATChallan + "','" + BilltoParty + "','" + InvoiceNo + "','" + InvoiceDate + "','" + CaseNoofShipper + "','" + VAT + "','" + Amount + "','" + Total + "','" + TransportNo + "','" + ShipToParty + "')";
           return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
       }
//       public bool CustomerXLDataGridToDbByRow(String BRANCH, String BRANCHDES, String CustomerCode, String CUSTOMERNAME, String ADDRESS1,
//           String ADDRESS2, String CITY, String CONTACTPERSON, String CONTACTNUMBER, String MIOCode, String MIOName, String TerritoryCode, String FECode, String FEName, String DZSMCode,
//           String DZSMName, String SHIPPINGCOND, String SHIPPINGPOINT, String MarketName, String TERMOFPAYMENT, string Migo)
//        {
//            string insertQuery = @"insert into tblCustomerMasterExcelFileDetail (MasterID, BRANCH, BRANCHDES, CustomerCode, CUSTOMERNAME, ADDRESS1,ADDRESS2, CITY, CONTACTPERSON, CONTACTNUMBER, MIOCode, MIOName, TerritoryCode, FECode, FEName, DZSMCode,DZSMName, SHIPPINGCOND, SHIPPINGPOINT, MarketName, TERMOFPAYMENT,Verifyed) 
//            values (" + Migo + ",'" + BRANCH + "','" + BRANCHDES + "','" + CustomerCode + "','" + CUSTOMERNAME + "','" + ADDRESS1 + "','" + ADDRESS2 + "','" + CITY + "','" + CONTACTPERSON + "','" + CONTACTNUMBER + "','" + MIOCode + "','" + MIOName + "','" + TerritoryCode + "','" + FECode + "','" + FEName + "','" + DZSMCode + "','" + DZSMName + "','" + SHIPPINGCOND + "','" + SHIPPINGPOINT + "','" + MarketName + "','" + TERMOFPAYMENT + "','" + "False" + "')";
//            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
//        }
       public Int32 CustomerXLDataGridToDbByRow(String BRANCH, String BRANCHDES, String CustomerCode, String CUSTOMERNAME, String ADDRESS1,
           String ADDRESS2, String CITY, String CONTACTPERSON, String CONTACTNUMBER, String MIOCode, String MIOName, String TerritoryCode, String FECode, String FEName, String DZSMCode,
           String DZSMName, String SHIPPINGCOND, String SHIPPINGPOINT, String MarketName, String TERMOFPAYMENT, string Migo)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@Migo", Migo));
            aSqlParameterlist.Add(new SqlParameter("@BRANCH", BRANCH));
            aSqlParameterlist.Add(new SqlParameter("@BRANCHDES", BRANCHDES));
            aSqlParameterlist.Add(new SqlParameter("@CustomerCode", CustomerCode));
            aSqlParameterlist.Add(new SqlParameter("@CUSTOMERNAME", CUSTOMERNAME));
            aSqlParameterlist.Add(new SqlParameter("@ADDRESS1", ADDRESS1));
            aSqlParameterlist.Add(new SqlParameter("@ADDRESS2", ADDRESS2));
            aSqlParameterlist.Add(new SqlParameter("@CITY", CITY));
            aSqlParameterlist.Add(new SqlParameter("@CONTACTPERSON", CONTACTPERSON));
            aSqlParameterlist.Add(new SqlParameter("@CONTACTNUMBER", CONTACTNUMBER));
            aSqlParameterlist.Add(new SqlParameter("@MIOCode", MIOCode));
            aSqlParameterlist.Add(new SqlParameter("@MIOName", MIOName));
            aSqlParameterlist.Add(new SqlParameter("@TerritoryCode", TerritoryCode));
            aSqlParameterlist.Add(new SqlParameter("@FECode", FECode));
            aSqlParameterlist.Add(new SqlParameter("@FEName", FEName));
            aSqlParameterlist.Add(new SqlParameter("@DZSMCode", DZSMCode));
            aSqlParameterlist.Add(new SqlParameter("@DZSMName", DZSMName));
            aSqlParameterlist.Add(new SqlParameter("@SHIPPINGCOND", SHIPPINGCOND));
            aSqlParameterlist.Add(new SqlParameter("@SHIPPINGPOINT", SHIPPINGPOINT));
            aSqlParameterlist.Add(new SqlParameter("@MarketName", MarketName));
            aSqlParameterlist.Add(new SqlParameter("@TERMOFPAYMENT", TERMOFPAYMENT));
            aSqlParameterlist.Add(new SqlParameter("@Verifyed", "False"));

            return aCommonInternalDal.SaveAction("sp_I_Customer", aSqlParameterlist, "@DetailID");
        }

       public bool SaveMigoDAL(MigoMasterDAO aMigoMasterDAO)
                                                      
       {
           string insertQuery = @"insert into tblMIGOMaster (MigoMasterID,MogoCode,ManufacId,MogoDocumentDate,StockUpload,EntryBy,EntryDate) 
            values (" + aMigoMasterDAO.MigoMasterID + ",'" + aMigoMasterDAO.MogoCode + "'," + aMigoMasterDAO.ManufacId + ",'" + aMigoMasterDAO.MogoDocumentDate + "','" + aMigoMasterDAO.StockUpload + "','" + aMigoMasterDAO.EntryBy + "'," +
                                "'" + aMigoMasterDAO.EntryDate + "')";
                           
           return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
       }
        public bool SaveCustomerUploadMasterDAL(MigoMasterDAO aMigoMasterDAO)
        {
            string insertQuery = @"insert into tblCustomerMasterExcelFileMaster (CustomerMasterExcelFileMasterID,CustomerMasterExcelFileCode,ManufacId,CustomerMasterExcelFileDocumentDate,Transfer,EntryBy,EntryDate,VerifyedAll) 
            values (" + aMigoMasterDAO.MigoMasterID + ",'" + aMigoMasterDAO.MogoCode + "'," + aMigoMasterDAO.ManufacId + ",'" + aMigoMasterDAO.MogoDocumentDate + "','" + aMigoMasterDAO.StockUpload + "','" + aMigoMasterDAO.EntryBy + "','" + aMigoMasterDAO.EntryDate + "','" + "False" + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool VerifyCustomerDAL(string id)
        {
            string insertQuery = @"exec sp_VerifyCustomer "+id+"";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
       public void LoadmanufacturerName(DropDownList ddl)
       {
           ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
           string queryStr = "select * from tblManufacturer";
           aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ManufacName", "ManufacId", queryStr);
       }
       public DataTable LoadMigo()
       {
           string query = @"SELECT TOP 10 * FROM dbo.tblMIGOMaster
INNER JOIN dbo.tblManufacturer ON tblMIGOMaster.ManufacId = tblManufacturer.ManufacId
where StockUpload=0 
ORDER BY MogoDocumentDate DESC ";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       public DataTable LoadMigobyID(int MigoMasterID)
       {
           string query = @"SELECT StockUpload,MigoMasterID FROM dbo.tblMIGOMaster
           left JOIN dbo.tblManufacturer ON tblMIGOMaster.ManufacId = tblManufacturer.ManufacId
           where MigoMasterID = '" + MigoMasterID + "'"; 

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public DataTable LoadMigoReport(string fromdate, string todate)
       {
           string query = @"SELECT r.ComUnitId,R.ComUnitCode ,R.ComUnitName ,P.ProductCode,P.ProductName ,M.MfgDate,
T.ExpDate,T.PackSize,T.BatchNo,SUM(t.Quantity) AS TotalQuantity,T.UnitPrice,SUM(T.PriceAmount) TotalPriceAmount,UP.VATAmountPerUnit,SUM(T.VATAmount)TotalVATAmount 
,SUM(TotalPriceAmount) AS TotalPriceAmountwithVat,R.ReqNo,R.ReqDate
FROM dbo.tblStockInTransfar T
INNER JOIN dbo.tblProduct P ON T.ProductCode = P.ProductCode
LEFT JOIN dbo.tblRequisition R ON T.ReqId = R.ReqId
LEFT JOIN dbo.tblCentralStore W ON T.ReceiveId = W.ReceiveId
LEFT JOIN dbo.tblWHStockInDetail M ON W.MigoDetailID = M.WHStockInDetailID
INNER JOIN dbo.tblUnitPrice UP ON T.ProductCode = UP.ProductCode
WHERE R.ReqDate BETWEEN '" + fromdate + "' AND '" + todate + "'GROUP BY R.ReqNo,R.ReqDate,r.ComUnitId,R.ComUnitCode  ,R.ComUnitName ,P.ProductCode,P.ProductName ,M.MfgDate,T.ExpDate,T.PackSize,T.BatchNo,T.UnitPrice ,UP.VATAmountPerUnit,T.ReceiveDate";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }


       public DataTable LoadMainMigoReport(string fromdate, string todate)
       {
           string query = @"SELECT pp.ProductName AS LMIDDescription,CONVERT(NVARCHAR,D.PODate,103)PODate,CONVERT(NVARCHAR,D.OrderDocDate,103)OrderDocDate,CONVERT(NVARCHAR,D.DeliveryDocDate,103)DeliveryDocDate,
          CONVERT(NVARCHAR,D.MfgDate,103)MfgDate,CONVERT(NVARCHAR,D.ExpDate,103)ExpDate,CONVERT(NVARCHAR,D.InvoiceDate,103)InvoiceDate,* 
             FROM dbo.tblMIGODetail D
           INNER JOIN dbo.tblMIGOMaster  M ON D.MigoMasterID=M.MigoMasterID
           LEFT JOIN dbo.tblProduct PP ON D.LMID = pp.ProductCode
           WHERE M.MogoDocumentDate BETWEEN  '" + fromdate + "' AND '" + todate + "'";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public DataTable LoadOrderDetail(string fromdate, string todate)
       {
           string query = @"SELECT (MIOCode+'-'+MIOName)MIOCode,(SalesCentre+'-'+SalesCentreName)SalesCentre,* FROM dbo.tblOrderListDetail
            LEFT JOIN dbo.tblOrderListMaster ON dbo.tblOrderListDetail.OrderMasterID = dbo.tblOrderListMaster.OrderMasterID WHERE DocumentDate BETWEEN '" + fromdate + "' AND '" + todate + "' ORDER BY tblOrderListDetail.SalesCentre";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       
       public bool DeleteData(int MigoMasterID)
       {
           string query = @"DELETE FROM dbo.tblMIGOMaster WHERE MigoMasterID = '" + MigoMasterID + "'"; 
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }
       public bool DeleteDetailData(int MigoMasterID)
       {
           string query = @"DELETE FROM dbo.tblMIGODetail WHERE MigoMasterID = '" + MigoMasterID + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }
       public DataTable LoadMigo(string MigoMasterID)
       {
           string query = @"SELECT  * FROM dbo.tblMIGODetail

        WHERE MigoMasterID = '" + MigoMasterID + "'";

           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       public DataTable LoadMigoDate(string parameter)
       {
           string query = @"SELECT  * FROM dbo.tblMIGOMaster  left JOIN dbo.tblManufacturer ON tblMIGOMaster.ManufacId = tblManufacturer.ManufacId Where StockUpload=0 and  " + parameter + "'";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
        public DataTable LoadCustomer(string parameter)
        {
            string query = @"SELECT  * FROM dbo.tblCustomerMasterExcelFileMaster LEFT JOIN dbo.tblManufacturer ON tblCustomerMasterExcelFileMaster.ManufacId = tblManufacturer.ManufacId  Where tblCustomerMasterExcelFileMaster.Transfer=0 and  " + parameter + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadCustomer(int MigoMasterID)
        {
            string query = @"SELECT  * FROM dbo.tblCustomerMasterExcelFileMaster   Where tblCustomerMasterExcelFileMaster.CustomerMasterExcelFileMasterID= '" + MigoMasterID + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
       public int TransfarMigobyID_DAL(int id)
       {
           List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
           aSqlParameterList.Add(new SqlParameter("@MigoMasterID_In", id));
           return aCommonInternalDal.RunStoreProcedure("sp_StockInMIGOtoCentralStore", aSqlParameterList, "SSIDB");
       }


        public bool DeleteCustomerData(int MigoMasterID)
        {
            string query = @"DELETE FROM dbo.tblCustomerMasterExcelFileMaster WHERE CustomerMasterExcelFileMasterID = '" + MigoMasterID + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool DeleteCustomerDetailData(int MigoMasterID)
        {
            string query = @"DELETE FROM dbo.tblCustomerMasterExcelFileDetail WHERE MasterID = '" + MigoMasterID + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public int TransfarCustomer_DAL(int id)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CustomerMasterExcelFileMasterID", id));
            return aCommonInternalDal.RunStoreProcedure("sp_CustomerTransfer", aSqlParameterList, "SSIDB");
        }
        public DataTable GetVerifyedData(string masterId)
        {
            string query = @"SELECT COUNT(*)A FROM dbo.tblCustomerMasterExcelFileDetail WHERE MasterID='" + masterId + "' AND Verifyed='True'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetUnVerifyedData(string masterId)
        {
            string query = @"SELECT COUNT(*)A FROM dbo.tblCustomerMasterExcelFileDetail WHERE MasterID='" + masterId + "' AND Verifyed='False'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ReportVerifyedData(string masterId)
        {
            string query = @"SELECT BRANCH AS ComUnitCode,BRANCHDES AS ComUnitName,CustomerCode AS CustomerCode,CUSTOMERNAME AS CustomerName,ADDRESS1 AS Address,ADDRESS2 AS Addrees2,CITY AS City,CONTACTPERSON AS ConPerson,CONTACTNUMBER AS CellNo,MIOCode AS MiaCode,MIOName AS MiaName,TerritoryCode AS  AreaCode,FECode AS DistrictCode,FEName,DZSMCode AS RegionCode,DZSMName,SHIPPINGCOND AS ShippingCond,SHIPPINGPOINT AS MarketCode,MarketName,TERMOFPAYMENT AS TermOfPayment FROM dbo.tblCustomerMasterExcelFileDetail WHERE MasterID='" + masterId + "' AND Verifyed='True'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ReportUnVerifyedData(string masterId)
        {
            string query = @"SELECT BRANCH AS ComUnitCode,BRANCHDES AS ComUnitName,CustomerCode AS CustomerCode,CUSTOMERNAME AS CustomerName,ADDRESS1 AS Address,ADDRESS2 AS Addrees2,CITY AS City,CONTACTPERSON AS ConPerson,CONTACTNUMBER AS CellNo,MIOCode AS MiaCode,MIOName AS MiaName,TerritoryCode AS  AreaCode,FECode AS DistrictCode,FEName,DZSMCode AS RegionCode,DZSMName,SHIPPINGCOND AS ShippingCond,SHIPPINGPOINT AS MarketCode,MarketName,TERMOFPAYMENT AS TermOfPayment FROM dbo.tblCustomerMasterExcelFileDetail WHERE MasterID='" + masterId + "' AND Verifyed='False'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
