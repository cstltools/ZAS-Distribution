using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class InvoiceDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        DB_Manager aDbManager = new DB_Manager();
        public void CreateConnection_DAL()
        {
            aDbManager.CreateConnection("SalesDisDB_New3");
        }
        public void CloseAllConnection_DAL()
        {
            aDbManager.CloseConnection();
        }


        public DataTable InvoiceDetailDataForReport2(string invNo)
        {
            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,tblProduct.Description AS GenericName,SUM(IVD.Quantity) Quantity,SUM(IVD.BonusQuantity) BonusQuantity, IVD.UnitPrice,   
IVD.UnitVatAmount,SUM(IVD.TotalPrice) TotalPrice, IVD.TotalPriceVatAmount, (IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage,SUM((IVD.DiscountAmount+IVD.SpecialAmount)) DiscountAmount,  
SUM(IVD.NetAmount) NetAmount,IVD.PackSize AS InvoiceId FROM dbo.tblInvoice AS IV 
LEFT JOIN dbo.tblInvoiceDetail AS IVD ON IVD.InvoiceId = IV.InvoiceId
LEFT JOIN dbo.tblProduct ON tblProduct.ProductCode = IVD.ProductCode
LEFT JOIN (SELECT CustomerMasterId,ProductId,DiscountPercentage FROM tblProductDiscount WHERE IsActive = 1 AND ActionStatus = 'Accepted' ) PD  ON PD.CustomerMasterId = IV.CustomerMasterId AND PD.ProductId = dbo.tblProduct.ProductId " + invNo.Trim() + " GROUP BY IVD.ProductCode,IVD.ProductName,tblProduct.Description, IVD.UnitPrice,IVD.UnitVatAmount, IVD.TotalPriceVatAmount, IVD.DiscountPercentage, PD.DiscountPercentage,IVD.PackSize ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
public DataTable GroupInvoiceDetailDataForReport2(string invNo)
        {
            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,tblProduct.Description AS GenericName,SUM(IVD.Quantity) Quantity,SUM(IVD.BonusQuantity) BonusQuantity, IVD.UnitPrice,   
IVD.UnitVatAmount,SUM(IVD.TotalPrice) TotalPrice, IVD.TotalPriceVatAmount, (IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage,SUM((IVD.DiscountAmount+IVD.SpecialAmount)) DiscountAmount,  
SUM(IVD.NetAmount) NetAmount,IVD.PackSize AS InvoiceId FROM dbo.tblInvoice AS IV 
LEFT JOIN dbo.tblInvoiceDetail AS IVD ON IVD.InvoiceId = IV.InvoiceId
LEFT JOIN dbo.tblProduct ON tblProduct.ProductCode = IVD.ProductCode
LEFT JOIN (SELECT CustomerMasterId,ProductId,DiscountPercentage FROM tblProductDiscount WHERE IsActive = 1 AND ActionStatus = 'Accepted' ) PD  ON PD.CustomerMasterId = IV.CustomerMasterId AND PD.ProductId = dbo.tblProduct.ProductId  WHERE  GroupInvoiceNo = '" + invNo.Trim() + "' GROUP BY IVD.ProductCode,IVD.ProductName,tblProduct.Description, IVD.UnitPrice,IVD.UnitVatAmount, IVD.TotalPriceVatAmount, IVD.DiscountPercentage, PD.DiscountPercentage,IVD.PackSize ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable InvoiceMainDataForReport2(string invNo)
        {
            string query = @"SELECT GroupInvoiceNo,EGI.EmpName AS DeliveryPersonName,tblEmpGeneralInfo.CellNumber AS DeliveryPersonPhNo,IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,
    IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as 
    CompanyName,  (CU.Address) AS CUAddress,  CM.CustomerCode,CM.CustomerName, (CM.Address) AS 
    CMAddress,CM.Addrees2,CC.CategoryName,PT.PaymentTypeName,  dbo.tblEmpGeneralInfo.EmpMasterCode AS MiaCode,dbo.tblEmpGeneralInfo.EmpName AS MiaName,MarketName as UserName,IV.Remarks,PONumber,U.UserName AS PreparedBy  
    FROM tblInvoice IV 
    LEFT JOIN dbo.tblOrder ON tblOrder.OrderId = IV.OrderId
    LEFT JOIN dbo.tblDeliveryManInfo ON IV.DeliveryManId=dbo.tblDeliveryManInfo.DeliveryManId
     LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId  
     LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId  
     LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = CM.MarketId
     LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId  
     LEFT JOIN dbo.tblMIOInfo MIA ON IV.MiaId=MIA.MIOId  
     LEFT JOIN tblUser U ON IV.UserId=U.UserId  
     LEFT JOIN dbo.tblEmpGeneralInfo ON MIA.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
	 LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = dbo.tblDeliveryManInfo.EmpInfoId
     LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " + invNo.Trim() + " ORDER BY IV.InvoiceId ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GroupInvoiceMainDataForReport2(string invNo)
        {
            string query = @"
        SELECT 
            INV.GroupInvoiceNo,
            CONVERT(DATE, INV.GroupInvoiceDate, 101) AS InvoiceDate,
            CSTMR.CustomerCode,
            CSTMR.CustomerName,
            CSTMR.Address,
            CSTMR.Addrees2,
            CSTMR.TermOfPayment,
            MKT.MarketName,
            EGI.EmpName AS DeliveryPersonName,
            EGI.CellNumber AS DeliveryPersonPhNo,
            INV.Remarks,
            INV.PONumber,
            U.UserName AS PreparedBy,
            INV.InvoiceNo,
            INV.OrderNo,
            CONVERT(DATE, ODR.SubmissionDate, 101) AS OrderDate,
            IVD.ProductCode,
            IVD.ProductName AS Product,
            PD.Description AS GenericName,
            SUM(IVD.Quantity) AS Quantity,
            SUM(IVD.BonusQuantity) AS BonusQuantity,
            SUM(IVD.Quantity) + SUM(IVD.BonusQuantity) AS TotalQuantity,
            IVD.UnitPrice,
            IVD.UnitVatAmount,
            SUM(IVD.TotalPrice) AS TotalPrice,
            IVD.TotalPriceVatAmount,
            (IVD.DiscountPercentage + ISNULL(DS.DiscountPercentage, 0)) AS DiscountPercentage,
            SUM(IVD.DiscountAmount + IVD.SpecialAmount) AS DiscountAmount,
            SUM(IVD.NetAmount) AS NetAmount,
            IVD.PackSize
        FROM tblInvoice AS INV
        LEFT JOIN tblUser AS U 
            ON INV.UserId = U.UserId
        LEFT JOIN tblOrder AS ODR 
            ON INV.OrderId = ODR.OrderId
        LEFT JOIN tblInvoiceDetail AS IVD 
            ON INV.InvoiceId = IVD.InvoiceId
        LEFT JOIN tblProduct AS PD 
            ON IVD.ProductCode = PD.ProductCode
        INNER JOIN dbo.tblDeliveryManInfo AS DLV 
            ON DLV.DeliveryManId = INV.DeliveryManId
        INNER JOIN dbo.tblEmpGeneralInfo AS EGI 
            ON EGI.EmpInfoId = DLV.EmpInfoId
        INNER JOIN dbo.tblCustMaster AS CSTMR 
            ON CSTMR.CustomerMasterId = INV.CustomerMasterId
        LEFT JOIN tblMarket AS MKT 
            ON CSTMR.MarketId = MKT.MarketId
        LEFT JOIN (
            SELECT CustomerMasterId, ProductId, DiscountPercentage
            FROM tblProductDiscount
            WHERE IsActive = 1 
              AND ActionStatus = 'Accepted'
        ) AS DS
            ON DS.CustomerMasterId = INV.CustomerMasterId
           AND DS.ProductId = PD.ProductId
        WHERE INV.GroupInvoiceNo = '" + invNo.Trim() + @"'
        GROUP BY
            INV.GroupInvoiceNo,
            CONVERT(DATE, INV.GroupInvoiceDate, 101),
            CSTMR.CustomerCode,
            CSTMR.CustomerName,
            CSTMR.Address,
            CSTMR.Addrees2,
            CSTMR.TermOfPayment,
            MKT.MarketName,
            EGI.EmpName,
            EGI.CellNumber,
            INV.Remarks,
            INV.PONumber,
            U.UserName,
            INV.InvoiceNo,
            INV.OrderNo,
            CONVERT(DATE, ODR.SubmissionDate, 101),
            IVD.ProductCode,
            IVD.ProductName,
            PD.Description,
            IVD.UnitPrice,
            IVD.UnitVatAmount,
            IVD.TotalPriceVatAmount,
            IVD.DiscountPercentage,
            DS.DiscountPercentage,
            IVD.PackSize
        ORDER BY 
            INV.InvoiceNo,
            IVD.ProductCode";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public int GenerateGroupInvoiceNo(string InvoiceNo)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));

            return aCommonInternalDal.RunStoreProcedure("sp_Generate_InvoiceNo", aSqlParameterList, "SSIDB");
        }

        public int SaveFullInvoice(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationFull", aSqlParameterList, "SSIDB");
        }

        public int SaveFullProformaInvoice(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationFull", aSqlParameterList, "SSIDB");
        }
        public int SaveRejectInvoice(string InvoiceNo, string updateby, string updatedate, string reason)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            aSqlParameterList.Add(new SqlParameter("@ReturnReason", reason));
            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationReject", aSqlParameterList, "SSIDB");
        }
        //public Int32 SaveDataForInvoice(Invoice aInvoice)
        //{
        //    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aInvoice.InvoiceId));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceNo", aInvoice.InvoiceNo));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceDate", aInvoice.InvoiceDate));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderNo", aInvoice.OrderNo));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderDate", aInvoice.OrderDate));
        //    aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aInvoice.CustomerMasterId));
        //    aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aInvoice.ComUnitId));
        //    aSqlParameterlist.Add(new SqlParameter("@MiaId", aInvoice.MiaId));
        //    aSqlParameterlist.Add(new SqlParameter("@PaymentTypeId", aInvoice.PaymentTypeId));
        //    aSqlParameterlist.Add(new SqlParameter("@TpTotal", aInvoice.TpTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@TpDiscount", aInvoice.TpDiscount));
        //    aSqlParameterlist.Add(new SqlParameter("@TpVat", aInvoice.TpVat));
        //    aSqlParameterlist.Add(new SqlParameter("@TpGrandTotal", aInvoice.TpGrandTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@UserId", aInvoice.UserId));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderId", aInvoice.OrderId));
        //    aSqlParameterlist.Add(new SqlParameter("@TotalSpecialAmount", aInvoice.TotalSpecialAmount));
        //    aSqlParameterlist.Add(new SqlParameter("@ProductOffer", aInvoice.ProductOffer));

        //    return aDbManager.SaveAction("sp_I_InvoiceMaster", aSqlParameterlist, "@InvoiceId");
        //}
        //public Int32 SaveDataForInvoiceDetails(InvoiceDetail aInvoiceDetail)
        //{
        //    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aInvoice.InvoiceId));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceNo", aInvoice.InvoiceNo));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceDate", aInvoice.InvoiceDate));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderNo", aInvoice.OrderNo));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderDate", aInvoice.OrderDate));
        //    aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aInvoice.CustomerMasterId));
        //    aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aInvoice.ComUnitId));
        //    aSqlParameterlist.Add(new SqlParameter("@MiaId", aInvoice.MiaId));
        //    aSqlParameterlist.Add(new SqlParameter("@PaymentTypeId", aInvoice.PaymentTypeId));
        //    aSqlParameterlist.Add(new SqlParameter("@TpTotal", aInvoice.TpTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@TpDiscount", aInvoice.TpDiscount));
        //    aSqlParameterlist.Add(new SqlParameter("@TpVat", aInvoice.TpVat));
        //    aSqlParameterlist.Add(new SqlParameter("@TpGrandTotal", aInvoice.TpGrandTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@UserId", aInvoice.UserId));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderId", aInvoice.OrderId));
        //    aSqlParameterlist.Add(new SqlParameter("@TotalSpecialAmount", aInvoice.TotalSpecialAmount));
        //    aSqlParameterlist.Add(new SqlParameter("@ProductOffer", aInvoice.ProductOffer));

        //    return aDbManager.SaveAction("sp_I_InvoiceMaster", aSqlParameterlist, "@InvoiceId");
        //}

        public bool SaveDataForInvoice(Invoice aInvoice)
        {
            string insertQuery = @" INSERT INTO dbo.tblInvoice " +
                " (    InvoiceId , " +
     "     InvoiceNo , " +
       "    InvoiceDate , " +
       "    OrderNo , " +
       "    OrderDate , " +
       "    CustomerMasterId , " +
       "    ComUnitId , " +
        "   MiaId , " +
        "   PaymentTypeId , " +
        "   TpTotal , " +
        "   TpDiscount , " +
        "   TpVat , " +
        "   TpGrandTotal , " +
        "   UserId, " +
        "   OrderId, " +
        "   TotalSpecialAmount, " +
         "   OldTradePolicy, " +
          "   ProductOffer,Remarks,DeliveryManId" +
      "   ) " +
      " VALUES  ( '" + aInvoice.InvoiceId + "' , " +
       "    '" + aInvoice.InvoiceNo + "' , " +
      "    '" + aInvoice.InvoiceDate + "' , " +
      "     '" + aInvoice.OrderNo + "' , " +
      "     '" + aInvoice.OrderDate + "' , " +
      "     '" + aInvoice.CustomerMasterId + "' , " +
       "    '" + aInvoice.ComUnitId + "' , " +
       "    '" + aInvoice.MiaId + "' , " +
      "     '" + aInvoice.PaymentTypeId + "' , " +
      "    '" + aInvoice.TpTotal + "' , " +
      "     '" + aInvoice.TpDiscount + "' , " +
      "     '" + aInvoice.TpVat + "' , " +
       "    '" + aInvoice.TpGrandTotal + "' , " +
       "    '" + aInvoice.UserId + "' , " +
       "    '" + aInvoice.OrderId + "' , " +
        "    '" + aInvoice.TotalSpecialAmount + "' , " +
          "    '" + aInvoice.OldTradePolicy + "' , " +
       "    '" + aInvoice.ProductOffer + "','" + aInvoice.Remarks + "','" + aInvoice.DeliveryManId + "'" +
      "   ) ";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool SaveDataForInvoiceConsumer(Invoice aInvoice)
        {
            string insertQuery = @" INSERT INTO dbo.tblInvoice " +
                " (    InvoiceId , " +
     "     InvoiceNo , " +
       "    InvoiceDate , " +
       "    OrderNo , " +
       "    OrderDate , " +
       "    CustomerMasterId , " +
       "    ComUnitId , " +
     //   "   MiaId , " +
        "   PaymentTypeId , " +
        "   TpTotal , " +
        "   TpDiscount , " +
        "   TpVat , " +
        "   TpGrandTotal , " +
        "   UserId, " +
        "   OrderId, " +
        "   TotalSpecialAmount " +
      //   "   OldTradePolicy, " +
        //  "   ProductOffer,Remarks,DeliveryManId" +
      "   ) " +
      " VALUES  ( '" + aInvoice.InvoiceId + "' , " +
       "    '" + aInvoice.InvoiceNo + "' , " +
      "    '" + aInvoice.InvoiceDate + "' , " +
      "     '" + aInvoice.OrderNo + "' , " +
      "     '" + aInvoice.OrderDate + "' , " +
      "     '" + aInvoice.CustomerMasterId + "' , " +
       "    '" + aInvoice.ComUnitId + "' , " +
      // "    '" + aInvoice.MiaId + "' , " +
      "     '" + aInvoice.PaymentTypeId + "' , " +
      "    '" + aInvoice.TpTotal + "' , " +
      "     '" + aInvoice.TpDiscount + "' , " +
      "     '" + aInvoice.TpVat + "' , " +
       "    '" + aInvoice.TpGrandTotal + "' , " +
       "    '" + aInvoice.UserId + "' , " +
       "    '" + aInvoice.OrderId + "' , " +
        "    '" + aInvoice.TotalSpecialAmount + "'  " +
     //     "    '" + aInvoice.OldTradePolicy + "' , " +
       //"    '" + aInvoice.ProductOffer + "','" + aInvoice.Remarks + "','" + aInvoice.DeliveryManId + "'" +
      "   ) ";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool SaveDataForReturnInvoice(Invoice aInvoice)
        {
            string insertQuery = @" INSERT INTO dbo.tblReturnInvoice " +
                " (    ReturnInvoiceId , " +
     "     ReturnInvoiceNo , " +
       "    ReturnInvoiceDate , " +
       "    OrderNo , " +
       "    OrderDate , " +
       "    CustomerMasterId , " +
       "    ComUnitId , " +
        "   MiaId , " +
        "   PaymentTypeId , " +
        "   TpTotal , " +
        "   TpDiscount , " +
        "   TpVat , " +
        "   TpGrandTotal , " +
        "   UserId, " +
        "   OrderId,InvoiceId,TotalSpecialAmount " +
      "   ) " +
      " VALUES  ( '" + aInvoice.InvoiceId + "' , " +
       "    '" + aInvoice.InvoiceNo + "' , " +
      "    '" + aInvoice.InvoiceDate + "' , " +
      "     '" + aInvoice.OrderNo + "' , " +
      "     '" + aInvoice.OrderDate + "' , " +
      "     '" + aInvoice.CustomerMasterId + "' , " +
       "    '" + aInvoice.ComUnitId + "' , " +
       "    '" + aInvoice.MiaId + "' , " +
      "     '" + aInvoice.PaymentTypeId + "' , " +
      "    '" + aInvoice.TpTotal + "' , " +
      "     '" + aInvoice.TpDiscount + "' , " +
      "     '" + aInvoice.TpVat + "' , " +
       "    '" + aInvoice.TpGrandTotal + "' , " +
       "    '" + aInvoice.UserId + "' , " +
       "    '" + aInvoice.OrderId + "',  " +
       "    '" + aInvoice.ReturnInvoiceid + "' , " +
       "    '" + aInvoice.TotalSpecialAmount + "'  " +
      "   ) ";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool SaveDataForInvoiceDetails(InvoiceDetail aInvoiceDetail)
        {
            string insertQuery = @" INSERT INTO dbo.tblInvoiceDetail "+
     "   ( ProductCode , "+
     "        ProductName , "+
      "       PackSize , "+
       "      BatchNo , "+
        "     ReceiveDate , "+
        "     ExpDate , "+
         "    CostPrice , "+
         "    UnitPrice , "+
         "    UnitVatAmount , "+
         "    Quantity , "+
         "    BonusQuantity ,"+
         "    TotalQuantity ,"+
         "    TotalPrice , "+
        "     TotalPriceVatAmount , "+
        "     DiscountPercentage , "+
        "     DiscountAmount , "+
        "     NetAmount , "+
        "     InvoiceId, "+
        "     DCStoreId, " +
        "     OrderDetailsId ," +
        "     SpecialAmount " +
       "    ) "+
 "  VALUES  ( '" + aInvoiceDetail.ProductCode + "' , " +
         "   '" + aInvoiceDetail.ProductName + "' , " +
         "    '" + aInvoiceDetail.PackSize + "' , " +
         "    '" + aInvoiceDetail.BatchNo + "' , " +
         "    '" + aInvoiceDetail.ReceiveDate + "' , " +
         "    '" + aInvoiceDetail.ExpDate + "' ,  " +
         "    '" + aInvoiceDetail.CostPrice + "' , " +
         "    '" + aInvoiceDetail.UnitPrice + "' , " +
         "    '" + aInvoiceDetail.UnitVatAmount + "' , " +
         "    '" + aInvoiceDetail.Quantity + "' , " +
         "    '" + aInvoiceDetail.BonusQuantity + "' , " +
         "    '" + aInvoiceDetail.TotalQuantity + "' , " +
         "    '" + aInvoiceDetail.TotalPrice + "' , " +
         "    '" + aInvoiceDetail.TotalPriceVatAmount + "' , " +
         "    '" + aInvoiceDetail.DiscountPercentage + "' , " +
         "    '" + aInvoiceDetail.DiscountAmount + "' , " +
       "      '" + aInvoiceDetail.NetAmount + "' , " +
       "      '" + aInvoiceDetail.InvoiceId + "' , " +
       "      '" + aInvoiceDetail.DCStoreId + "' , " +
       "      '" + aInvoiceDetail.OrderDetailsId + "' , " +
       "      '" + aInvoiceDetail.SpecialAmount + "'  " +
       
      "     )";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool SaveDataForInvoiceDetailsConsumer(InvoiceDetail aInvoiceDetail)
        {
            string insertQuery = @" INSERT INTO dbo.tblInvoiceDetail " +
     "   ( InvoiceDetailId , " +
        "     ProductCode , " +
     "        ProductName , " +
      "       PackSize , " +
       "      BatchNo , " +
        "     ReceiveDate , " +
        "     ExpDate , " +
         "    CostPrice , " +
         "    UnitPrice , " +
         "    UnitVatAmount , " +
         "    Quantity , " +
         "    BonusQuantity ," +
         "    TotalQuantity ," +
         "    TotalPrice , " +
        "     TotalPriceVatAmount , " +
        "     DiscountPercentage , " +
        "     DiscountAmount , " +
        "     NetAmount , " +
        "     InvoiceId, " +
        "     DCStoreId, " +
        "     OrderDetailsId ," +
        "     SpecialAmount " +
       "    ) " +
 "  VALUES  ( '" + aInvoiceDetail.InvoiceDetailId + "' , " +
          "   '" + aInvoiceDetail.ProductCode + "' , " +
         "   '" + aInvoiceDetail.ProductName + "' , " +
         "    '" + aInvoiceDetail.PackSize + "' , " +
         "    '" + aInvoiceDetail.BatchNo + "' , " +
         "    '" + aInvoiceDetail.ReceiveDate + "' , " +
         "    '" + aInvoiceDetail.ExpDate + "' ,  " +
         "    '" + aInvoiceDetail.CostPrice + "' , " +
         "    '" + aInvoiceDetail.UnitPrice + "' , " +
         "    '" + aInvoiceDetail.UnitVatAmount + "' , " +
         "    '" + aInvoiceDetail.Quantity + "' , " +
         "    '" + aInvoiceDetail.BonusQuantity + "' , " +
         "    '" + aInvoiceDetail.TotalQuantity + "' , " +
         "    '" + aInvoiceDetail.TotalPrice + "' , " +
         "    '" + aInvoiceDetail.TotalPriceVatAmount + "' , " +
         "    '" + aInvoiceDetail.DiscountPercentage + "' , " +
         "    '" + aInvoiceDetail.DiscountAmount + "' , " +
       "      '" + aInvoiceDetail.NetAmount + "' , " +
       "      '" + aInvoiceDetail.InvoiceId + "' , " +
       "      '" + aInvoiceDetail.DCStoreId + "' , " +
       "      '" + aInvoiceDetail.OrderDetailsId + "' , " +
       "      '" + aInvoiceDetail.SpecialAmount + "'  " +

      "     )";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool SaveConsumerCustomer(ConsumerCustomerDAO aConsumerCustomerDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@ConsumerCustId", aConsumerCustomerDao.ConsumerCustId));
            aSqlParameterlist.Add(new SqlParameter("@CustomerCode", aConsumerCustomerDao.CustomerCode));
            aSqlParameterlist.Add(new SqlParameter("@CustomerName", aConsumerCustomerDao.CustomerName));
            aSqlParameterlist.Add(new SqlParameter("@CustomerPhoneNo", aConsumerCustomerDao.CustomerPhoneNo));
            aSqlParameterlist.Add(new SqlParameter("@CustomerAddress", aConsumerCustomerDao.CustomerAddress));

            string insertQuery = @"INSERT INTO dbo.tblConsumerCustomer
                                    (ConsumerCustId,
                                        CustomerCode,
                                        CustomerName,
                                        CustomerPhoneNo,
                                        CustomerAddress
                                    )
                                    VALUES
                                    ( @ConsumerCustId,
                                        @CustomerCode,
                                        @CustomerName,
                                        @CustomerPhoneNo,
                                        @CustomerAddress
                                    )";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, aSqlParameterlist, "SSIDB");
        }
        public int SaveOrderMaster(OrderInfoMaster aOrderInfoMaster)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@OrderCode", aOrderInfoMaster.OrderCode));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aOrderInfoMaster.ComUnitId));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitCode", aOrderInfoMaster.ComUnitCode));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitName", aOrderInfoMaster.ComUnitName));
            aSqlParameterlist.Add(new SqlParameter("@ManufacId", aOrderInfoMaster.ManufacId));
            aSqlParameterlist.Add(new SqlParameter("@CustomerName", aOrderInfoMaster.CustomerName));
            aSqlParameterlist.Add(new SqlParameter("@CustomerCode   ", aOrderInfoMaster.CustomerCode));
            aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aOrderInfoMaster.CustomerMasterId));
            aSqlParameterlist.Add(new SqlParameter("@GrossValue", aOrderInfoMaster.GrossValue));
            aSqlParameterlist.Add(new SqlParameter("@SubmissionDate", aOrderInfoMaster.SubmissionDate));
            aSqlParameterlist.Add(new SqlParameter("@IsInvoice", aOrderInfoMaster.IsInvoice));
            aSqlParameterlist.Add(new SqlParameter("@IsManual", aOrderInfoMaster.IsManual));
            aSqlParameterlist.Add(new SqlParameter("@IsSpecialApproval", aOrderInfoMaster.IsSpecialApproval));
            aSqlParameterlist.Add(new SqlParameter("@EntryBy", aOrderInfoMaster.EntryBy));
            aSqlParameterlist.Add(new SqlParameter("@EntryDate", aOrderInfoMaster.EntryDate));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aOrderInfoMaster.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aOrderInfoMaster.ApprovedDate));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aOrderInfoMaster.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@IsDirect", aOrderInfoMaster.IsDirect));


            string insertQuery = @"INSERT INTO dbo.tblOrder
                                    (
                                        OrderCode,
                                        ComUnitId,
                                        ComUnitCode,
                                        ComUnitName,
    
                                        ManufacId,
                                        CustomerCode,
                                        CustomerName,
                                        CustomerMasterId,
                                        GrossValue,
                                        SubmissionDate,
                                        IsInvoice,
                                        IsManual,
    
                                        IsSpecialApproval,
                                        EntryBy,
                                        EntryDate,
    
                                        ApprovedBy,
                                        ApprovedDate,
                                        ActionStatus,IsDirect
                                    )
                                    VALUES
                                    (   @OrderCode,
                                        @ComUnitId,
                                        @ComUnitCode,
                                        @ComUnitName,
    
                                        @ManufacId,
    
                                        @CustomerCode,
                                        @CustomerName,
                                        @CustomerMasterId,
                                        @GrossValue,
                                        @SubmissionDate,
                                        @IsInvoice,
                                        @IsManual,
    
                                        @IsSpecialApproval,
                                        @EntryBy,
                                        @EntryDate,
    
                                        @ApprovedBy,
                                        @ApprovedDate,
                                        @ActionStatus,@IsDirect
                                    )";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterlist, "SSIDB");
        }
        public int SaveOrderDetail(OrderInfoDetail aOrderInfoDetail)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@ProductId", aOrderInfoDetail.ProductId));
            aSqlParameterlist.Add(new SqlParameter("@ProductCode", aOrderInfoDetail.ProductCode));
            aSqlParameterlist.Add(new SqlParameter("@ProductName", aOrderInfoDetail.ProductName));
            aSqlParameterlist.Add(new SqlParameter("@Quantity", aOrderInfoDetail.Quantity));
            aSqlParameterlist.Add(new SqlParameter("@TradePrice", aOrderInfoDetail.TradePrice));
            aSqlParameterlist.Add(new SqlParameter("@TotalTradePrice", aOrderInfoDetail.TotalTradePrice));
            aSqlParameterlist.Add(new SqlParameter("@OrderId", aOrderInfoDetail.OrderId));
            aSqlParameterlist.Add(new SqlParameter("@OrderListDetailId", aOrderInfoDetail.OrderListDetailId));
            aSqlParameterlist.Add(new SqlParameter("@Status", aOrderInfoDetail.Status));



            string insertQuery = @"INSERT INTO dbo.tblOrderDetail
                                    (
                                        ProductId,
                                        ProductCode,
                                        ProductName,
                                        Quantity,
                                        TradePrice,
                                        TotalTradePrice,
                                        OrderId,
                                        OrderListDetailId,
                                        Status
                                    )
                                    VALUES
                                    (   @ProductId,
                                        @ProductCode,
                                        @ProductName,
                                        @Quantity,
                                        @TradePrice,
                                        @TotalTradePrice,
                                        @OrderId,
                                        @OrderListDetailId,
                                        @Status
                                    )";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterlist, "SSIDB");
        }

        public bool SaveDataForReturnInvoiceDetails(InvoiceDetail aInvoiceDetail)
        {
            string insertQuery = @" INSERT INTO dbo.tblReturnInvoiceDetail " +
     "   ( ReuturnInvoiceDetailId , " +
        "     ProductCode , " +
     "        ProductName , " +
      "       PackSize , " +
       "      BatchNo , " +
        "     ReceiveDate , " +
        "     ExpDate , " +
         "    CostPrice , " +
         "    UnitPrice , " +
         "    UnitVatAmount , " +
         "    Quantity , " +
         "    BonusQuantity ," +
         "    TotalQuantity ," +
         "    TotalPrice , " +
        "     TotalPriceVatAmount , " +
        "     DiscountPercentage , " +
        "     DiscountAmount , " +
        "     NetAmount , " +
        "     ReturnInvoiceId, " +
        "     DCStoreId, " +
        "     OrderDetailsId,InvoiceDetailId,SpecialAmount " +
       "    ) " +
 "  VALUES  ( '" + aInvoiceDetail.InvoiceDetailId + "' , " +
          "   '" + aInvoiceDetail.ProductCode + "' , " +
         "   '" + aInvoiceDetail.ProductName + "' , " +
         "    '" + aInvoiceDetail.PackSize + "' , " +
         "    '" + aInvoiceDetail.BatchNo + "' , " +
         "    '" + aInvoiceDetail.ReceiveDate + "' , " +
         "    '" + aInvoiceDetail.ExpDate + "' ,  " +
         "    '" + aInvoiceDetail.CostPrice + "' , " +
         "    '" + aInvoiceDetail.UnitPrice + "' , " +
         "    '" + aInvoiceDetail.UnitVatAmount + "' , " +
         "    '" + aInvoiceDetail.Quantity + "' , " +
         "    '" + aInvoiceDetail.BonusQuantity + "' , " +
         "    '" + aInvoiceDetail.TotalQuantity + "' , " +
         "    '" + aInvoiceDetail.TotalPrice + "' , " +
         "    '" + aInvoiceDetail.TotalPriceVatAmount + "' , " +
         "    '" + aInvoiceDetail.DiscountPercentage + "' , " +
         "    '" + aInvoiceDetail.DiscountAmount + "' , " +
       "      '" + aInvoiceDetail.NetAmount + "' , " +
       "      '" + aInvoiceDetail.InvoiceId + "' , " +
       "      '" + aInvoiceDetail.DCStoreId + "' , " +
       "      '" + aInvoiceDetail.OrderDetailsId + "',  " +
       "      '" + aInvoiceDetail.ReturnDetailsId + "' , " +
       "      '" + aInvoiceDetail.SpecialAmount + "'  " +
      "     )";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasProductcode(DCStore aReceive)
        {
            string query = "select * from tblDCStock where ProductCode = '" + aReceive.ProductCode + "' and BatchNo='"+aReceive.BatchNo+"'";
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

        public DataTable LoadInvoiceView()
        {
            string query = @"SELECT *  FROM tblInvoice ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ProductInfoDAL(string comUnitId, string productCode)
        {

             string query = @"   SELECT P.ProductCode,(P.ProductName+':'+P.PackSize) as  ProductName,P.PackSize, " +
            " ISNULL(UP.UnitPrice,0) AS UnitPrice,ISNULL(VCS.TotalQty,0) AS StockQty, " +
            " (UP.VATAmountPerUnit) AS VAT, ISNULL(UP.CostPrice,0) AS CostPrice, " +
            " ISNULL(UP.VATPercentage,0)VATPercentage,MRPPrice,P.ProductId  FROM " +
            " dbo.tblProduct P  " +
            " LEFT JOIN dbo.tblUnitPrice UP ON P.ProductCode = UP.ProductCode  " +
            " LEFT JOIN (select ComUnitId,ProductCode, TotalQty from View_DCStoreCurrentStock WHERE ComUnitId='" + comUnitId.Trim() + "' AND ProductCode='" + productCode.Trim() + "') VCS  " +
            " ON P.ProductCode=VCS.ProductCode   where  UP.ActionStatus = 'Accepted' AND UP.IsActive = 'TRUE' AND P.ProductCode='" + productCode.Trim() + "' ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ProductInfoDAL(string comUnitId, string productCode,string customerMasterId)
        {

            string query = @"SELECT P.ProductCode,(P.ProductName+':'+P.PackSize) as  ProductName,P.PackSize, " +
           " CASE WHEN QTD.QuotedPrice IS NULL THEN ISNULL(UP.UnitPrice,0) ELSE ISNULL(QTD.QuotedPrice,0) END AS UnitPrice,ISNULL(VCS.TotalQty,0) AS StockQty, " +
           " (UP.VATAmountPerUnit) AS VAT, ISNULL(UP.CostPrice,0) AS CostPrice, " +
           " ISNULL(UP.VATPercentage,0)VATPercentage,MRPPrice,P.ProductId  FROM " +
           " dbo.tblProduct P  " +
           " LEFT JOIN dbo.tblUnitPrice UP ON P.ProductCode = UP.ProductCode  " +
           " LEFT JOIN (SELECT ProductId,QuotedPrice FROM tblProductQuotedPrice AS Q LEFT JOIN tblCustMaster AS C ON Q.CustomerMasterId = C.CustomerMasterId WHERE '" + DateTime.Today + "' <= Q.InactiveDate AND ActionStatus = 'Accepted' AND Q.IsActive = 1 AND CustomerCode = '" + customerMasterId.Trim() + "') AS QTD ON QTD.ProductId = P.ProductId" +
           " LEFT JOIN (select ComUnitId,ProductCode, TotalQty from View_DCStoreCurrentStock WHERE ComUnitId='" + comUnitId.Trim() + "' AND ProductCode='" + productCode.Trim() + "') VCS  " +
           " ON P.ProductCode=VCS.ProductCode   where  UP.ActionStatus = 'Accepted' AND UP.IsActive = 'TRUE' AND P.ProductCode='" + productCode.Trim() + "' ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ProductFocBonusQtyDAL(string invoiceDate, string productCode,int Qty)
        {

            string query = @"select * from [dbo].[tblFocMaster] M " +
                           " inner join [dbo].[tblFocDetails] D on M.FocId=D.FocId " +
                           " where ProductCode='" + productCode.Trim() + "' and  ('" + invoiceDate.Trim() + "' between [FocFromDate] and [FocToDate]) " +
                           " and IsActive=1 and (" + Qty + " between [RangeFrom] and RangeTo) ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadProductQty(string orderid, string productCode)
        {
            string query = @"SELECT SUM(Quantity)Qty FROM dbo.tblOrder
LEFT JOIN dbo.tblOrderDetail ON dbo.tblOrder.OrderId = dbo.tblOrderDetail.OrderId WHERE dbo.tblOrder.OrderId='" + orderid + "' AND ProductCode='" + productCode + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
       
        public DataTable LoadProduct(string productId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblProduct where ActionStatus = 'Accepted' AND IsActive = 'TRUE' AND ProductCode='" + productId.Trim() + "' ";
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
        //public DataTable LoadCustomerMaster(string CustomerMasterId)
        //{
        //    DataTable aDataTableEmpInfo = new DataTable();
        //    string query = @"SELECT * FROM dbo.View_CustomerMaster   WHERE CustomerCode='" + CustomerMasterId.Trim() + "' ";
        //    aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //    return aDataTableEmpInfo;
        //}
        public DataTable LoadCustomerMaster(string CustomerMasterId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM dbo.tblCustMaster
            INNER JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            INNER JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            INNER JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId
            INNER JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
            INNER JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            INNER JOIN dbo.tblDcWiseAreaInfo ON tblDcWiseAreaInfo.AreaId = tblArea.AreaId
            INNER JOIN dbo.tblCompanyUnit ON dbo.tblDcWiseAreaInfo.DCId=dbo.tblCompanyUnit.ComUnitId
            INNER JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
            INNER JOIN dbo.tblCustCategory ON tblCustCategory.CategoryId = tblCustMaster.CategoryId
            WHERE CustomerCode='" + CustomerMasterId.Trim() + "' ";
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
        public DataTable LoadConsumerCustomer(string CustomerMasterId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM dbo.tblConsumerCustomer WHERE ConsumerCustId='"+CustomerMasterId+"'";
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
        public DataTable GetDcStoreId(string invoicedetailId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM dbo.tblInvoiceDetail WHERE InvoiceDetailId='"+invoicedetailId+"'";
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
        public DataTable DCStockQuantity(DCStore aReceive)
        {
            string query = "select * from tblDCStock where ProductCode = '" + aReceive.ProductCode + "' and BatchNo='"+aReceive.BatchNo+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DCInfoWithDCId(string dcstoreId)
        {
            string query = "SELECT * FROM dbo.tblDCStore WHERE DCStoreId='"+dcstoreId+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void UpdateDCStoreQuantity(string dCStoreId, decimal Quantity)
        {
            string updateQuery = @"UPDATE tblDCStore SET StockQty='" + Quantity + "' WHERE DCStoreId='" + dCStoreId.Trim() + "'  ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
      //  public bool DCStockInDAL(DCStockNew aDcStockNew)
      //  {
      //      string query = @"INSERT INTO dbo.tblDCStoreFreeze " +
      // "  ( DCStoreId , " +
      //   "    DCStoreFreezeId, " +
      //   "    InvoiceDetailId, " +
      //   "    StorageLocation , " +
      //   "    ProductCode , " +
      //   "    ProductName , " +
      //   "    PackSize , " +
      //   "    BatchNo , " +
      //   "    TotalQuantity , " +
      //   "    ExpDate , " +
      //   "    ReceiveDate , " +
      //   "    ChalanNo , " +
      //   "    ChalanDate , " +
      //    "   ComUnitId , " +
      //   "    StockQty , " +
      //   "    DamageQty , " +
      //   "    StockRcvDate , " +
      //   "    ReqId , " +
      //   "    ReqChildId , " +
      //   "    StockInTransfarId, " +
      //   "    StockCondition " +
      //  "   ) " +
      //  "   VALUES  ( '" + aDcStockNew.DCStoreId + "' , " +
      //  "     '" + aDcStockNew.DCStoreFreezeId + "' , " +
      //  "     '" + aDcStockNew.InvoiceDetailId + "' , " +
      //  "     '" + aDcStockNew.StorageLocation + "' , " +
      //  "     '" + aDcStockNew.ProductCode + "' ,  " +
      //  "    '" + aDcStockNew.ProductName + "' , " +
      //  "    '" + aDcStockNew.PackSize + "' , " +
      //  "    '" + aDcStockNew.BatchNo + "' , " +
      //   "    '" + aDcStockNew.TotalQuantity + "' ,  " +
      //   "    '" + aDcStockNew.ExpDate + "' ,  " +
      //  "     '" + aDcStockNew.ReceiveDate + "' , " +
      //  "    '" + aDcStockNew.ChalanNo + "' , " +
      //  "    '" + aDcStockNew.ChalanDate + "', " +
      //  "    '" + aDcStockNew.ComUnitId + "' , " +
      //   "    '" + aDcStockNew.StockQty + "', " +
      //   "    '" + aDcStockNew.DamageQty + "' , " +
      //   "   '" + aDcStockNew.StockRcvDate + "' , " +
      //   "   '" + aDcStockNew.ReqId + "' , " +
      //  "    '" + aDcStockNew.ReqChildId + "', " +
      // "      '" + aDcStockNew.StockInTransfarId + "',  " +
      // "      'ReturnStock'  " +
      //"     )";
      //      return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
      //  }

        public void UpdateInvoice(Invoice aInvoice)
        {
            string updateQuery = @"UPDATE tblInvoice SET DeliveryTpTotal='" + aInvoice.TpTotal + "',DeliveryTpDiscount='" + aInvoice.TpDiscount + "',DeliveryTpVat='" + aInvoice.TpVat + "'," +
                                 "DeliveryTpGrandTotal='" + aInvoice.TpGrandTotal + "',DeliveryInvoiceStatus='" + aInvoice.DeliveryInvoiceStatus + "',DelivaryInvoiceNo='" + aInvoice.DelivaryInvoiceNo + "',DelivarySpecialAmount='" + aInvoice.TotalSpecialAmount + "',UpdateBy='" + aInvoice.UpdateBy + "',UpdateDate='" + aInvoice.InvoiceDate + "' WHERE InvoiceId='" + aInvoice.InvoiceId + "'  ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
        public void UpdateInvoiceDetail(InvoiceDetail  aInvoiceDetail)
        {
            string updateQuery = @"UPDATE tblInvoiceDetail SET DeliveryQuantity='" + aInvoiceDetail.Quantity + "',DeliveryBonusQuantity='" + aInvoiceDetail.BonusQuantity + "',DeliveryTotalQuantity='" + aInvoiceDetail.TotalQuantity+ "'," +
                                 "DeliveryTotalPrice='" + aInvoiceDetail.TotalPrice + "',DeliveryTotalPriceVatAmount='" + aInvoiceDetail.TotalPriceVatAmount + "',DeliveryDiscountPercentage='" + aInvoiceDetail.DiscountPercentage + "',DeliveryDiscountAmount='" + aInvoiceDetail.DiscountAmount + "',DeliveryNetAmount='" + aInvoiceDetail.NetAmount + "',DeliveryStatus='" + aInvoiceDetail.DeliveryStatus + "',DelivarySpecialAmount='" + aInvoiceDetail.SpecialAmount + "',ReturnReason='" + aInvoiceDetail.ReturnReason + "' WHERE InvoiceDetailId='" + aInvoiceDetail.InvoiceDetailId + "'  ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
        public void UpdateInvoiceDetailConsumer(string invoiceID)
        {
            string updateQuery = @"UPDATE tblInvoiceDetail SET DeliveryQuantity=Quantity,DeliveryBonusQuantity=BonusQuantity,DeliveryTotalQuantity=TotalQuantity," +
                                 "DeliveryTotalPrice=TotalPrice,DeliveryTotalPriceVatAmount=TotalPriceVatAmount,DeliveryDiscountPercentage=DiscountPercentage,DeliveryDiscountAmount=DiscountAmount,DeliveryNetAmount=NetAmount,DeliveryStatus='Full',DelivarySpecialAmount=SpecialAmount,ReturnReason='' WHERE InvoiceId='" + invoiceID + "'  ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
        public void UpdateInvoiceConsumer(string invoiceID)
        {
            string updateQuery = @"UPDATE tblInvoice SET IsDirect='True' WHERE InvoiceId='" + invoiceID + "'  ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
        public void PaymentTypeLoad(DropDownList aDropDownList)
        {
            string query = @"select * from tblPaymentType";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "PaymentTypeName", "PaymentTypeId", query, "SSIDB");
        }
        public void LoadCompany(DropDownList aDropDownList)
        {
            string query = @"SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "CompanyName", "CompanyId", query, "SSIDB");
        }
        public void LoadDC(DropDownList aDropDownList,string companyId)
        {
            string query = @"SELECT ComUnitId,
       
       (ComUnitCode+':'+ComUnitName)ComUnit
        FROM dbo.tblCompanyUnit WHERE CompanyId='"+companyId+"'";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "ComUnit", "ComUnitId", query, "SSIDB");
        }
        public void LoadDeliveryMan(DropDownList aDropDownList)
        {
//            string query = @"SELECT *,EmpName + CASE WHEN EmpMasterCode IS NOT NULL THEN ' (' + EmpMasterCode + ')' END  as DeliveryManName FROM dbo.tblDeliveryManInfo
//                            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId ORDER BY EmpName "; 


            string query = @"SELECT DL.DeliveryManId,ISNULL( HRGI.EmpName + ' (' + JobLocationName + ')', EGI.EmpName) DeliveryManName FROM tblDeliveryManInfo AS DL
            INNER JOIN tblEmpGeneralInfo AS EGI ON DL.EmpInfoId = EGI.EmpInfoId
            LEFT JOIN ZASHRIS..tblEmpGeneralInfo AS HRGI ON EGI.EmpMasterCode = HRGI.EmpMasterCode
            LEFT JOIN ZASHRIS..tblJobLocation AS JL ON HRGI.JobLocationisionId = JL.JobLocationisionId
            WHERE DL.IsActive = 1 ORDER BY JobLocationName ASC ";


            aCommonInternalDal.LoadDropDownValue(aDropDownList, "DeliveryManName", "DeliveryManId", query, "SSIDB");
        }
        public void ReturnReason(DropDownList aDropDownList)
        {
            string query = @"SELECT * FROM tblReturnReason";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "ReaturnReason", "ReturndReasonId", query, "SSIDB");
        }
        public DataTable GetReason()
        {
            string query = @"SELECT ReaturnReason,ReturndReasonId FROM tblReturnReason";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
       
        public DataTable GetConsumerCustomerInfo(string custId)
        {
            string query = @"SELECT * FROM dbo.tblConsumerCustomer WHERE ConsumerCustId='"+custId+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        //public DataTable DiscountofProduct(string productCode,string qty)
        //{
        //    string query = @"SELECT * FROM dbo.tblProductDiscount WHERE ProductCode='" + productCode.Trim() + "' AND ('" + qty.Trim() + "' BETWEEN MinQty AND MaxQty) AND Status='Active'";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        public DataTable DiscountofProduct(string productCode,string customerId,string date)
        {
            string query = @"SELECT * FROM dbo.tblProductDiscount
            LEFT JOIN dbo.tblProduct ON tblProduct.ProductId = tblProductDiscount.ProductId
            WHERE ProductCode='" + productCode + "' AND CustomerMasterId='" + customerId + "' AND IsActive='1' AND ActionStatus='Accepted'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable BatchWiseProductQty(string productCode, string comUnitId)
        {
            string query = @"SELECT * FROM tblDCStore WHERE ProductCode='" + productCode.Trim() + "' AND ComUnitId='" + comUnitId.Trim() + "' AND StockQty>0 ORDER BY ExpDate ASC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        //public DataTable InvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName , " +
        //                 " (CU.Address +' ' +',M-'+CU.MobileNo+',P-'+CU.PhoneNo+',F-'+CU.FaxNo) AS CUAddress, "+
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address+' '+CM.CellNo) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, "+
        //                " MIA.MiaCode,MIA.MiaName,U.UserName "+
        //                " FROM tblInvoice IV "+
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId "+
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId "+
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId "+
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId "+
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId "+
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        //public DataTable InvoiceDetailDataForReport(string invNo)
        //{
        //    //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
        //    //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
        //    //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //    //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //    //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

        //    string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
        //                    "  IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, " +
        //                    "  (IVD.DiscountAmount+IVD.SpecialAmount)DiscountAmount,IVD.NetAmount,IV.InvoiceId  " +
        //                    "   FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //             "  INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId " +
        //            "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //              " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        public DataTable InvoiceDetailDataForReport(int Dcid, int ManufId, int marketid, DateTime invDate)
        {
            string query = @"    SELECT  * 				
FROM tblInvoice I
INNER JOIN (SELECT DISTINCT D.InvoiceId, ManufacId FROM dbo.tblInvoice I
            INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId
            INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode
            ) as tblD ON I.InvoiceId = tblD.InvoiceId  
 INNER JOIN dbo.View_CustomerMaster C ON I.CustomerMasterId = C.CustomerMasterId
 INNER JOIN dbo.tblMarket ON C.MarketCode=dbo.tblMarket.MarketCode     
where I.ComUnitId= '" + Dcid + "' and tblD.ManufacId='" + ManufId + "' and tblMarket.MarketId='" + marketid + "' and InvoiceDate='" + invDate + "' order by OrderNo";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ReturnInvoiceMainDataForReport(string invNo)
        {
            string query = @"SELECT IV.InvoiceId,IV.ReturnInvoiceNo as InvoiceNo,ReturnInvoiceId as InvoiceId ,IV.ReturnInvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,IV.TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName , " +
                         " (CU.Address +' ' +',M-'+CU.MobileNo+',P-'+CU.PhoneNo+',F-'+CU.FaxNo) AS CUAddress, " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address+' '+CM.CellNo) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,U.UserName " +
                        " FROM tblReturnInvoice IV " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ReturnInvoiceDetailDataForReport(string invNo)
        {
            string query = @"SELECT IV.ReturnInvoiceNo as InvoiceNo,IV.ReturnInvoiceId as InvoiceId,IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
                            " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.ReturnInvoiceId " +
                             " FROM dbo.tblReturnInvoiceDetail IVD LEFT JOIN dbo.tblReturnInvoice IV ON IVD.ReturnInvoiceId = IV.ReturnInvoiceId " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable AllInvoiceForPrintingDAL(string ComUnitId, DateTime InvoiceDate)
        {
            string query = @"SELECT * FROM dbo.tblInvoice I LEFT JOIN dbo.tblCustMaster C ON I.CustomerMasterId = C.CustomerMasterId "+
                            " WHERE I.ComUnitId='"+ComUnitId.Trim()+"' AND I.InvoiceDate='"+InvoiceDate+"' ORDER BY I.InvoiceId DESC" ;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable InvoiceForDCPickingDAL(string ComUnitId, DateTime InvoiceDate)
        {
            string query = @"SELECT * FROM dbo.tblInvoice I LEFT JOIN dbo.tblCustMaster C ON I.CustomerMasterId = C.CustomerMasterId " +
                            " WHERE I.ComUnitId='" + ComUnitId.Trim() + "' AND I.InvoiceDate='" + InvoiceDate + "' AND I.InvoiceNo NOT IN (SELECT InvoiceNo FROM dbo.tblDCPickingDetail)  ORDER BY I.InvoiceId DESC";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public bool DcPickingSaveDAL(DCPicking aDcPicking)
        {
            string insertQuery = @" INSERT INTO dbo.tblDCPicking "+
                          "  ( DCPicId , "+
                           "    DCPicNo , "+
                          "     DCPicDate , "+
                           "    ComUnitId ,"+
                           "    AreaId " +
                          "   ) "+
                   "  VALUES  ( '" + aDcPicking.DCPicId + "' ,  " +
                         "      '" + aDcPicking.DCPicNo + "' , " +
                        "       '" + aDcPicking.DCPicDate + "' , " +
                       "        '" + aDcPicking.ComUnitId + "'  ," +
                        "        '" + aDcPicking.AreaId + "'  " +
                        "     ) ";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool UpdateOrder(string  status,string id)
        {
            string insertQuery = @"UPDATE dbo.tblOrderDetail SET Status='"+status+"' WHERE OrderDetailId='"+id+"'";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool DcPickingDetailSaveDAL(DCPickingDetail aDcPickingDetail)
        {
            string insertQuery = @"INSERT INTO dbo.tblDCPickingDetail "+
                         "   ( DCPicDetailId, InvoiceNo, DCPicId ) "+
                   "    VALUES  ( '" + aDcPickingDetail.DCPicDetailId + "', " +
                         "       '" + aDcPickingDetail.InvoiceNo + "', " +
                         "        '" + aDcPickingDetail.DCPicId + "' " +
                         "        )";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }


        public DataTable AllPickingForReportList(string comUnitId,DateTime pickDate)
        {
            string query = @"select * from tblDCPicking where DCPicDate='" + pickDate + "' and ComUnitId='" + comUnitId.Trim() + "' order by DCPicId  desc ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable DCPickingReportMainDataDAL(string dcPickingNo)
        {
            string query = @"SELECT P.DCPicNo,P.DCPicDate,CU.ComUnitCode,CU.ComUnitName,CU.Address,A.AreaCode,A.AreaName FROM tblDCPicking P LEFT JOIN dbo.tblCompanyUnit CU ON P.ComUnitId = CU.ComUnitId LEFT JOIN dbo.tblArea A ON P.AreaId=A.AreaId " +
                            " WHERE P.DCPicNo='" + dcPickingNo.Trim() + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable  DCPickingReportDetailDataDAL(string dcPickingNo)
        {

            string query = @"SELECT IND.ProductCode,IND.ProductName,IND.BatchNo, SUM(TotalQuantity) AS TotalPickQty FROM dbo.tblInvoiceDetail IND LEFT JOIN dbo.tblInvoice I ON IND.InvoiceId = I.InvoiceId  "+
                            " WHERE I.InvoiceNo IN (SELECT InvoiceNo FROM dbo.tblDCPickingDetail LEFT JOIN dbo.tblDCPicking "+
                            " ON dbo.tblDCPickingDetail.DCPicId = dbo.tblDCPicking.DCPicId WHERE tblDCPicking.DCPicNo='" + dcPickingNo.Trim() + "')  " +
                            " GROUP BY IND.ProductCode,IND.ProductName,IND.BatchNo ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void AreaDropDownLoad(DropDownList dropDownList, string comUnitId)
        {
            string query = @"SELECT A.* FROM dbo.tblArea A LEFT JOIN dbo.tblDistrict D ON A.DistrictId = D.DistrictId "+
                                " LEFT JOIN dbo.tblCompanyUnit C ON D.ComUnitId = C.ComUnitId "+
                                " WHERE C.ComUnitId='" + comUnitId.Trim() + "'";
            aCommonInternalDal.LoadDropDownValue(dropDownList, "AreaName", "AreaId", query, "SSIDB");

        }
        public DataTable InvoiceNoCount(string comUnitId)
        {
            //string query = @"SELECT count(InvoiceNo) CountNo FROM dbo.tblInvoice WHERE ComUnitId ='" + comUnitId.Trim() + "'";

            string query = @"SELECT  CASE WHEN ComUnitId > 99  THEN (ISNULL(MAX(CAST((SUBSTRING(InvoiceNo,11,12)) AS INT)),0)+1) 
                             ELSE (ISNULL(MAX(CAST((SUBSTRING(InvoiceNo,10,11)) AS INT)),0)+1) END CountNo FROM dbo.tblInvoice WHERE ComUnitId ='" + comUnitId.Trim() + "' AND Remarks != 'Due' GROUP BY ComUnitId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ReturnInvoiceNoCount(string comUnitId)
        {
            string query = @"SELECT count(ReturnInvoiceNo) CountNo FROM dbo.tblReturnInvoice WHERE ComUnitId ='" + comUnitId.Trim() + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DcPickingNoCount(string comUnitId)
        {
            string query = @"SELECT count(DCPicNo) CountNo FROM dbo.tblDCPicking WHERE ComUnitId ='" + comUnitId.Trim() + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable MarketPickinReport(string SC, int MarketID, int ManufacID, DateTime InvDate)
        {
            string query = @" SELECT tblMarket.MarketName,M.MiaName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,SUM(D.Quantity) AS Quantity " +
                        " FROM dbo.tblInvoice I " +
                           " INNER JOIN View_CustomerMaster C  ON I.CustomerMasterId = C.CustomerMasterId " +
                        " INNER JOIN dbo.tblMIAInfo M ON C.MiaId = M.MiaId " +
                        " INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId " +
                        " INNER JOIN dbo.tblMarket ON C.MarketId=dbo.tblMarket.MarketId  " +
                        " INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode  " +
                       " WHERE  I.ComUnitId='" + SC + "' and p.ManufacId='" + ManufacID + "' and InvoiceDate='" + InvDate + "' and tblMarket.MarketId='" + MarketID + "' GROUP BY tblMarket.MarketName,M.MiaName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize ";
            //  " I.ComUnitId= '2' AND p.ManufacId='1' AND tblMarket.MarketId='9' AND InvoiceDate='7/31/2017 12:00:00 AM'  ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }




        public DataTable DeliveryInvoiceMainDataForReportDAL(string invNo)
        {
            string query = @"SELECT IV.InvoiceId,IV.DelivaryInvoiceNo AS InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal AS TpTotal,IV.DeliveryTpVat AS TpVat,(IV.DeliveryTpDiscount  +isnull(IV.DelivarySpecialAmount ,0))TpDiscount,IV.DeliveryTpGrandTotal AS TpGrandTotal,CU.ComUnitCode,CU.ComUnitName , " +
                         "        (CU.Address +' ' +',M-'+CU.MobileNo+',P-'+CU.PhoneNo+',F-'+CU.FaxNo) AS CUAddress,  " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address+' '+CM.CellNo) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,U.UserName  " +
                        " FROM tblInvoice IV  " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
               
                       " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DeliveryInvoiceDetailDataForReportDAL(string invNo)
        {
            string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.DeliveryQuantity AS Quantity,IVD.UnitPrice, IVD.UnitVatAmount,IVD.DeliveryTotalPrice,IVD.DeliveryTotalPriceVatAmount,(IVD.DeliveryDiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, (IVD.DeliveryDiscountAmount+IVD.DelivarySpecialAmount)DiscountAmount,IVD.DeliveryNetAmount AS NetAmount,IV.InvoiceId   " +
                            " LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
                            " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ProformaReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        {

//            string query =
//                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,
//CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,
//CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount,I.AreaCode
//,I.RegionCode as MiaCode,I.DisCode as DistrictCode,I.MarketCode
//,I.MarketName
//FROM dbo.tblInvoice I with(nolock)
//INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
//INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
//INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
//INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId
//        where CU.ComUnitId='" + districtId.Trim() + "' and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";

            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,
CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,
CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount, C.TerritoryName
,MEGI.EmpName AS MIO
FROM dbo.tblInvoice I with(nolock)
INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId
INNER JOIN dbo.tblOrder AS ORD ON ORD.OrderId = I.OrderId
LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = I.MiaId
LEFT JOIN dbo.tblEmpGeneralInfo AS MEGI ON MIO.EmployeeId = MEGI.EmpInfoId
LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = ORD.TerritoryId
        where I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ProformaReportDAl( DateTime fromDate, DateTime toDate)
        {

//            string query =
//                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,
//CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,
//CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount,I.AreaCode
//,I.RegionCode as MiaCode,I.DisCode as DistrictCode,I.MarketCode
//,I.MarketName
//FROM dbo.tblInvoice I with(nolock)
//INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
//INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
//INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
//INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId
//       where I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";


            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,
CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,
CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount, C.TerritoryName
,MEGI.EmpName AS MIO
FROM dbo.tblInvoice I with(nolock)
INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId
INNER JOIN dbo.tblOrder AS ORD ON ORD.OrderId = I.OrderId
LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.MIOId = I.MiaId
LEFT JOIN dbo.tblEmpGeneralInfo AS MEGI ON MIO.EmployeeId = MEGI.EmpInfoId
LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = ORD.TerritoryId
        where I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable InvoiceMainDataForReport(string invNo, string invoiceId)
        {
            string query = @"SELECT EGI.EmpName AS DeliveryPersonName,tblEmpGeneralInfo.CellNumber AS DeliveryPersonPhNo,IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,tblOrder.EntryDate OrderDate,
    IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as 
    CompanyName,  (CU.Address) AS CUAddress,  CM.CustomerCode,CM.CustomerName, (CM.Address) AS 
    CMAddress,CM.Addrees2,CompanyName CategoryName,PT.PaymentTypeName,  dbo.tblEmpGeneralInfo.EmpMasterCode AS MiaCode,dbo.tblEmpGeneralInfo.EmpName AS MiaName,MarketName as UserName,IV.Remarks,PONumber,U.UserName AS PreparedBy,
   CONVERT(VARCHAR,BatchNo) + ' - ( ' + CONVERT(varchar,Date,107) + ')' AS BatchNo 
    FROM tblInvoice IV 
    LEFT JOIN dbo.tblOrder ON tblOrder.OrderId = IV.OrderId
    LEFT JOIN dbo.tblDeliveryManInfo ON IV.DeliveryManId=dbo.tblDeliveryManInfo.DeliveryManId
LEFT JOIN  tblInvoiceBatch AS B ON IV.BatchId = B.BatchId
     LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId  
LEFT JOIN tblCompanyInfo AS COM ON CU.CompanyId = COM.CompanyId
     LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId  
     LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = CM.MarketId
     LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId  
     LEFT JOIN dbo.tblMIOInfo MIA ON IV.MiaId=MIA.MIOId  
     LEFT JOIN tblUser U ON IV.UserId=U.UserId  
     LEFT JOIN dbo.tblEmpGeneralInfo ON MIA.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
	 LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = dbo.tblDeliveryManInfo.EmpInfoId
     LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId WHERE IV.InvoiceId = " + invoiceId  + " AND IV.InvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        //public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
        //                 " (CU.Address) AS CUAddress, " +
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
        //                " MIA.MiaCode,MIA.MiaName,U.UserName " +
        //                " FROM tblInvoice IV " +
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable InvoiceDetailDataForReport(string invNo, string invoiceId)
        {
            //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
            //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
            //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
            //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
            //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,tblProduct.Description AS GenericName,SUM(IVD.Quantity) Quantity,SUM(IVD.BonusQuantity) BonusQuantity, IVD.UnitPrice,   
IVD.UnitVatAmount,SUM(IVD.TotalPrice) TotalPrice,SUM(IVD.TotalPriceVatAmount) TotalPriceVatAmount, (IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage,SUM((IVD.DiscountAmount+IVD.SpecialAmount)) DiscountAmount,  
SUM(IVD.NetAmount) NetAmount,IVD.PackSize AS InvoiceId FROM dbo.tblInvoice AS IV 
LEFT JOIN dbo.tblInvoiceDetail AS IVD ON IVD.InvoiceId = IV.InvoiceId
LEFT JOIN dbo.tblProduct ON tblProduct.ProductCode = IVD.ProductCode
LEFT JOIN (SELECT CustomerMasterId,ProductId,DiscountPercentage FROM tblProductDiscount WHERE IsActive = 1 AND ActionStatus = 'Accepted' ) PD  ON PD.CustomerMasterId = IV.CustomerMasterId AND PD.ProductId = dbo.tblProduct.ProductId WHERE IV.InvoiceId = " + invoiceId + " AND IV.InvoiceNo in (" + invNo.Trim() + ") GROUP BY IVD.ProductCode,IVD.ProductName,tblProduct.Description, IVD.UnitPrice,IVD.UnitVatAmount, IVD.DiscountPercentage, PD.DiscountPercentage,IVD.PackSize ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable ReturnReturnInvoiceMainDataForReport(string invNo)
        {
            string query = @"SELECT IV.ReturnInvoiceId as InvoiceId,IV.ReturnInvoiceNo as InvoiceNo,IV.ReturnInvoiceDate as InvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName , " +
                         " (CU.Address ) AS CUAddress, " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,U.UserName " +
                        " FROM tblReturnInvoice IV " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
                //" WHERE IV.ReturnInvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable ReturnReturnInvoiceDetailDataForReport(string invNo)
        {
            string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
                            "  IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, " +
                            "  (IVD.DiscountAmount+IVD.SpecialAmount)DiscountAmount,IVD.NetAmount,IV.ReturnInvoiceId as  InvoiceId  " +
                            "   FROM dbo.tblReturnInvoiceDetail IVD LEFT JOIN dbo.tblReturnInvoice IV ON IVD.ReturnInvoiceId = IV.ReturnInvoiceId " +
                     "  INNER JOIN dbo.tblReturnInvoice I ON I.ReturnInvoiceId = IV.ReturnInvoiceId " +
                    "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
                //" WHERE IV.ReturnInvoiceNo='" + invNo.Trim() + "'";
                      " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        //public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.DelivaryInvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal,IV.DeliveryTpVat,(IV.DeliveryTpDiscount+isnull(IV.DelivarySpecialAmount,0))TpDiscount,IV.DeliveryTpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
        //                 " (CU.Address) AS CUAddress, " +
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
        //                " MIA.MiaCode,MIA.MiaName,U.UserName " +
        //                " FROM tblInvoice IV " +
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        //public DataTable DelivaryInvoiceDetailDataForReport(string invNo)
        //{
        //    //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
        //    //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
        //    //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //    //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //    //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

        //    string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.DeliveryQuantity,IVD.UnitPrice, " +
        //                    "  IVD.UnitVatAmount,IVD.DeliveryTotalPrice,IVD.DeliveryTotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DeliveryDiscountPercentage,0))DiscountPercentage, " +
        //                    "  (IVD.DeliveryDiscountAmount+IVD.DelivarySpecialAmount)DiscountAmount,IVD.DeliveryNetAmount,IV.InvoiceId  " +
        //                    "   FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //             "  INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId " +
        //            "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //              " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") ";

        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        //public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.DelivaryInvoiceNo as InvoiceNo,IV.UpdateDate as InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal,IV.DeliveryTpVat,(IV.DeliveryTpDiscount+isnull(IV.DelivarySpecialAmount,0))TpDiscount,IV.DeliveryTpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
        //                 " (CU.Address) AS CUAddress, " +
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
        //                " MIA.MiaCode,MIA.MiaName,U.UserName " +
        //                " FROM tblInvoice IV " +
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ")   ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        public DataTable MarketPickinReport(string SC, int MarketID, int ManufacID, DateTime InvDate, string parameter)
        {
            string query = @" SELECT tblMarket.MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,SUM(D.Quantity) AS Quantity " +
                        " FROM dbo.tblInvoice I " +
                           " INNER JOIN View_CustomerMaster C  ON I.CustomerMasterId = C.CustomerMasterId " +
                        " INNER JOIN dbo.tblMIAInfo M ON C.MiaId = M.MiaId " +
                        " INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId " +
                        " INNER JOIN dbo.tblMarket ON C.MarketId=dbo.tblMarket.MarketId  " +
                        " INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode  " +
                       " WHERE  I.ComUnitId='" + SC + "' and p.ManufacId='" + ManufacID + "' and InvoiceDate='" + InvDate + "' and tblMarket.MarketId='" + MarketID + "' " + parameter + " GROUP BY tblMarket.MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize ";
            //  " I.ComUnitId= '2' AND p.ManufacId='1' AND tblMarket.MarketId='9' AND InvoiceDate='7/31/2017 12:00:00 AM'  ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DelivaryInvoiceDetailDataForReport(string invNo)
        {

            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,IVD.BatchNo,
                             IVD.PackSize as BonusQuantity,IVD.DeliveryTotalQuantity as Quantity,IVD.UnitPrice,   
                             IVD.UnitVatAmount,IVD.DeliveryTotalPrice as TotalPrice,IVD.DeliveryTotalPriceVatAmount as TotalPriceVatAmount,
                             DeliveryDiscountPercentage DiscountPercentage,   
                             CASE WHEN IVD.Quantity > 0 THEN (IVD.DeliveryDiscountAmount+((IVD.DelivarySpecialAmount/IVD.Quantity)*IVD.DeliveryQuantity)) ELSE 0 END DiscountAmount,IVD.DeliveryNetAmount as NetAmount,IV.InvoiceId FROM tblInvoiceDetail AS  IVD 
                             LEFT JOIN tblInvoice AS IV ON IVD.InvoiceId = IV.InvoiceId
                             LEFT JOIN tblProduct AS PD ON IVD.ProductCode = PD.ProductCode 
                             WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") and IVD.DeliveryStatus IN ('Full','Partial') ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        {
            string query = @"SELECT DL.EmpName AS DeliveryPersonName,tblEmpGeneralInfo.CellNumber AS DeliveryPersonPhNo,IV.InvoiceId,IV.DelivaryInvoiceNo as InvoiceNo,IV.UpdateDate as InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal as TpTotal,IV.DeliveryTpVat as TpVat,(IV.DeliveryTpDiscount+isnull(IV.DelivarySpecialAmount,0))TpDiscount,IV.DeliveryTpGrandTotal as TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as 
    CompanyName,  (CU.Address) AS CUAddress,  CM.CustomerCode,CM.CustomerName, (CM.Address) AS 
    CMAddress,CM.Addrees2,CC.CategoryName,PT.PaymentTypeName,  tblEmpGeneralInfo.EmpMasterCode AS MiaCode,tblEmpGeneralInfo.EmpName AS MiaName,MarketName as UserName  

    FROM tblInvoice IV 
    LEFT JOIN dbo.tblOrder ON tblOrder.OrderId = IV.OrderId
    LEFT JOIN dbo.tblDeliveryManInfo ON IV.DeliveryManId=dbo.tblDeliveryManInfo.DeliveryManId
	LEFT JOIN dbo.tblEmpGeneralInfo AS DL ON tblDeliveryManInfo.EmpInfoId=DL.EmpInfoId
     LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId  
     LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId  
     LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = CM.MarketId
     LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId  
     LEFT JOIN dbo.tblMIOInfo MIA ON IV.MiaId=MIA.MIOId  
     LEFT JOIN tblUser U ON IV.UserId=U.UserId  
     LEFT JOIN dbo.tblEmpGeneralInfo ON MIA.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
     LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId  WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DelivaryInvoiceMainDataForReport2(string invNo)
        {
            string query = @"SELECT IV.DeliveryPersonName,IV.DeliveryPersonPhNo,IV.InvoiceId,IV.DelivaryInvoiceNo as InvoiceNo,IV.UpdateDate as InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal as TpTotal,IV.DeliveryTpVat as TpVat,(IV.DeliveryTpDiscount+isnull(IV.DelivarySpecialAmount,0))TpDiscount,IV.DeliveryTpGrandTotal as TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
                         " (CU.Address) AS CUAddress, " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CM.Addrees2,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,CM.MarketName as UserName " +
                        " FROM tblInvoice IV " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ")   ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CheckReturnExistOrNotInFreezeStore(DCStockNew aDetailDao)
        {
            string query = "";

            if (aDetailDao.InvoiceDetailId > 0) // For Delivery Confirmation 
            {
                query = @"SELECT * FROM tblDCStoreFreeze WHERE CONVERT(DATE,FreezeRcvDate) = CONVERT(DATE,GETDATE()) AND  InvoiceDetailId = '" +
                    aDetailDao.InvoiceDetailId +
                    "' AND LTRIM(RTRIM(UPPER(ProductCode))) = LTRIM(RTRIM(UPPER('" + aDetailDao.ProductCode +
                    "')))  AND LTRIM(RTRIM(UPPER(BatchNo))) = LTRIM(RTRIM(UPPER('" + aDetailDao.BatchNo +
                    "'))) AND TotalQuantity = " + aDetailDao.TotalQuantity;
            }

            else if (aDetailDao.CustReturnDetailId > 0) // For Sales Return 
            {
                query = @"SELECT * FROM tblDCStoreFreeze WHERE CONVERT(DATE,FreezeRcvDate) = CONVERT(DATE,GETDATE()) AND  CustReturnDetailId = '" +
                   aDetailDao.CustReturnDetailId +
                   "' AND LTRIM(RTRIM(UPPER(ProductCode))) = LTRIM(RTRIM(UPPER('" + aDetailDao.ProductCode +
                   "')))  AND LTRIM(RTRIM(UPPER(BatchNo))) = LTRIM(RTRIM(UPPER('" + aDetailDao.BatchNo +
                   "'))) AND TotalQuantity = " + aDetailDao.TotalQuantity;
            }


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DCStockInDAL(DCStockNew aDcStockNew)
        {
            string query = @"INSERT INTO dbo.tblDCStoreFreeze " +
       "  ( DCStoreId , " +
         "    DCStoreFreezeId, " +
         "    InvoiceDetailId, " +
         "    StorageLocation , " +
         "    ProductCode , " +
         "    ProductName , " +
         "    PackSize , " +
         "    BatchNo , " +
         "    TotalQuantity , " +
         "    ExpDate , " +
         "    ReceiveDate , " +
         "    ChalanNo , " +
         "    ChalanDate , " +
          "   ComUnitId , " +
         "    StockQty , " +
         "    DamageQty , " +
         "    StockRcvDate , " +
         "    ReqId , " +
         "    ReqChildId , " +
         "    StockInTransfarId, " +
         "    CustReturnDetailId, " +
         "    StockCondition,ChalanDetailsId " +
        "   ) " +
        "   VALUES  ( '" + aDcStockNew.DCStoreId + "' , " +
        "     '" + aDcStockNew.DCStoreFreezeId + "' , " +
        "     '" + aDcStockNew.InvoiceDetailId + "' , " +
        "     '" + aDcStockNew.StorageLocation + "' , " +
        "     '" + aDcStockNew.ProductCode + "' ,  " +
        "    '" + aDcStockNew.ProductName + "' , " +
        "    '" + aDcStockNew.PackSize + "' , " +
        "    '" + aDcStockNew.BatchNo + "' , " +
         "    '" + aDcStockNew.TotalQuantity + "' ,  " +
         "    '" + aDcStockNew.ExpDate + "' ,  " +
        "     '" + aDcStockNew.ReceiveDate + "' , " +
        "    '" + aDcStockNew.ChalanNo + "' , " +
        "    '" + aDcStockNew.ChalanDate + "', " +
        "    '" + aDcStockNew.ComUnitId + "' , " +
         "    '" + aDcStockNew.StockQty + "', " +
         "    '" + aDcStockNew.DamageQty + "' , " +
         "   '" + aDcStockNew.StockRcvDate + "' , " +
         "   '" + (aDcStockNew.ReqId ?? Convert.DBNull) + "' , " +
        "    '" + (aDcStockNew.ReqChildId ?? Convert.DBNull) + "', " +
       "      '" + (aDcStockNew.StockInTransfarId ?? Convert.DBNull) + "',  " +
       "      '" + (aDcStockNew.CustReturnDetailId ?? Convert.DBNull) + "',  " +
       "      'ReturnStock','" + (aDcStockNew.ChalanDetailsId ?? Convert.DBNull) + "'  " +
      "     )";
            return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
        }

        public DataTable IntransitReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        {
            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,
CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,
I.TpGrandTotal AS NetAmount,I.TpVat AS TotalPriceVatAmount,I.TpDiscount AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode AS DistrictCode
,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) IntransitDay,I.MIACode as MainMIOCODE,I.MIAName as MainMIONAME
FROM dbo.tblInvoice I WITH(nolock)
INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
where I.TpTotal>0 AND I.DelivaryInvoiceNo IS NULL  and CU.ComUnitId='" + districtId.Trim() + "' and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable IntransitReportDAl(DateTime fromDate, DateTime toDate)
        {
            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,
CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,
I.TpGrandTotal AS NetAmount,I.TpVat AS TotalPriceVatAmount,I.TpDiscount AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode AS DistrictCode
,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) IntransitDay,I.MIACode as MainMIOCODE,I.MIAName as MainMIONAME
FROM dbo.tblInvoice I WITH(nolock)
INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
where I.TpTotal>0 AND I.DelivaryInvoiceNo IS NULL    and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public int DeleteInvoice(string Invoice)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@OrderID", Invoice));
            return aCommonInternalDal.RunStoreProcedure("sp_Deletenvoice", aSqlParameterList, "SSIDB");
        }

        public DataTable GetBatchWiseTopSheet(int companyId, int delManId, int batchId, DateTime invDate)
        {
            string query = @"SELECT INV.InvoiceDate, INV.InvoiceNo,INV.OrderId,SUM(INVD.Quantity) AS OrderQty,INV.InvoiceId,SUM(ORDD.Quantity) AS ProformaQty, 
                            INV.TpGrandTotal AS TotalAmount FROM dbo.tblInvoiceBatch AS INVB
                            INNER JOIN dbo.tblInvoice AS INV ON INV.BatchId = INVB.BatchId
                            INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
                            INNER JOIN dbo.tblOrder AS ORD ON ORD.OrderId = INV.OrderId
                            INNER JOIN dbo.tblOrderDetail AS ORDD ON ORDD.OrderId = ORD.OrderId
                            WHERE INV.DeliveryManId = '" + delManId +"' AND INVB.CompanyId = '" + companyId + "' AND INVB.BatchId = '" + batchId + "' AND INVB.Date = '" + invDate + "' GROUP BY INV.OrderId,INV.InvoiceDate,INV.InvoiceNo, ORD.OrderId, INV.InvoiceId, INV.TpGrandTotal";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
           // return aCommonInternalDal.RunStoreProcedure("sp_Deletenvoice", aSqlParameterList, "SSIDB");
        }

        public DataTable DelManPickinReport(string SC, int delman, string parameter)
        {
            string query = @"SELECT BatchCode,EGI.EmpName AS MarketName,INV.InvoiceDate,INVD.ProductCode, INVD.ProductName, INVD.PackSize,INVD.BatchNo,SUM(INVD.TotalQuantity) AS Quantity FROM dbo.tblInvoiceDetail AS INVD 
                             INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = INVD.InvoiceId
                             INNER JOIN dbo.tblInvoiceBatch AS INVB ON INVB.BatchId = INV.BatchId
                             INNER JOIN dbo.tblDeliveryManInfo AS DMI ON DMI.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DMI.EmpInfoId
                           WHERE INVB.BatchId = '" + SC + "' AND INV.DeliveryManId = '" + delman + "' " + parameter + " GROUP BY BatchCode,EGI.EmpName,INV.InvoiceDate,INVD.ProductCode, INVD.ProductName, INVD.PackSize,INVD.BatchNo";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetInvoiceData(DateTime fromDate, DateTime toDate)
        {
            string query = @"SELECT CUNT.ComUnitCode + '' + CUNT.ComUnitName AS Wings,CSTMR.CustomerCode + ':' + CSTMR.CustomerName AS Customer,INV.InvoiceNo, INV.InvoiceDate,INV.TpGrandTotal,EGI.EmpName AS DeliveryMan FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = INV.ComUnitId
                             INNER JOIN dbo.tblDeliveryManInfo AS DLV ON DLV.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLV.EmpInfoId
                             WHERE INV.InvoiceDate BETWEEN '" + fromDate + "' AND '" + toDate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadManufac(DropDownList ddl)
        {
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public DataTable LoadCustomerPayType(string value)
        {
            string query = @"SELECT TermOfPayment FROM dbo.tblCustMaster WHERE CustomerMasterId = " + value;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable InvoiceNumberDAL(string SC, int delman, string parameter)
        {
            string query = @" select
                                     distinct  
                                     stuff((
                                     select ',' + INV.InvoiceNo
                                     from tblInvoice INV
                                      where INV.InvoiceNo = InvoiceNo  " + parameter + " order by INV.InvoiceNo for xml path('') ),1,1,'') as InvoiceNO from tblInvoice   group by InvoiceNo ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCompanyIdByInvoiceId(string invoiceId)
        {
            string query = @"SELECT CI.CompanyId FROM dbo.tblInvoice AS INV
                            LEFT JOIN dbo.tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
                            LEFT JOIN dbo.tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
                            WHERE INV.InvoiceId = " + invoiceId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveCreditAdjustmentInfo(CreditAdjustmentDao adjustmentDao)
        {
            string insertQuery = @"INSERT INTO tblCreditAdjustment " +
                         " ( CompanyId, CustomerMasterId, InvoiceId,Amount,ReturnDate,EntryBy,EntryDate) " +
                         " VALUES  ( '" + adjustmentDao.CompanyId + "', " + "'" + adjustmentDao.CustomerMasterId + "', " + " '" + adjustmentDao.InvoiceId + "', " + " '" + adjustmentDao.Amount + "', " + " '" + adjustmentDao.ReturnDate + "', " + " '" + adjustmentDao.EntryBy + "', " + " '" + adjustmentDao.EntryDate + "' " + " )";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable GetFinancialYearInfo(string invoiceDate, string unitId)
        {
            string query = @"SELECT DISTINCT FinancialYearId FROM dbo.tblCompanyUnit AS UNT
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
                             INNER JOIN CSTL_ACCDB..tblFinancialYear ON tblFinancialYear.CompanyId = CI.CompanyId
                             WHERE UNT.ComUnitId = " + unitId + " AND Status IN ('Active') AND '" + invoiceDate + "' BETWEEN StartDate AND EndDate";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool PostJournal(int invId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invId));
            return aCommonInternalDal.UpdateAction("sp_ProformaPosting", aSqlParameterList);
        }

        public bool PostProformaReturnJournal(int invId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invId));
            return aCommonInternalDal.UpdateAction("sp_ProformaFullReturnPosting", aSqlParameterList);
        }

        public bool PostDeliveryConfFullJournal(int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            //return aCommonInternalDal.UpdateAction("sp_DeliveryConfirmationFullPosting", aSqlParameterList);
            //return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesPosting", aSqlParameterList);
            return aCommonInternalDal.UpdateAction("sp_Sales_ExecuteDeliveryPipeline", aSqlParameterList);
        }

        public DataTable CheckChartOfAccount(string customerCode)
        {
            string query = @"SELECT AccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE CustomerId IS NOT NULL AND CustomerId IN (
                             SELECT CustomerMasterId FROM dbo.tblCustMaster WHERE CustomerCode = '" + customerCode + "')";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool PostDeliveryConfPartialJournal(int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            //return aCommonInternalDal.UpdateAction("sp_DeliveryConfirmationPartiallPosting", aSqlParameterList);
            return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesPosting", aSqlParameterList);
        }
        public void UpdateInvoiceNull(Invoice aInvoice)
        {
            string updateQuery = @"UPDATE tblInvoice SET DeliveryTpTotal=NULL,DeliveryTpDiscount=NULL,DeliveryTpVat=NULL," +
                                 "DeliveryTpGrandTotal=NULL,DeliveryInvoiceStatus=NULL,DelivaryInvoiceNo=NULL,DelivarySpecialAmount=NULL,UpdateBy=NULL,UpdateDate=NULL WHERE InvoiceId='" + aInvoice.InvoiceId + "'  ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
        public void UpdateInvoiceDetailNull(InvoiceDetail aInvoiceDetail)
        {
            string updateQuery = @"UPDATE tblInvoiceDetail SET DeliveryQuantity=NULL,DeliveryBonusQuantity=NULL,DeliveryTotalQuantity=NULL," +
                                 "DeliveryTotalPrice=NULL,DeliveryTotalPriceVatAmount=NULL,DeliveryDiscountPercentage=NULL,DeliveryDiscountAmount=NULL,DeliveryNetAmount=NULL,DeliveryStatus=NULL,DelivarySpecialAmount=NULL,ReturnReason=NULL WHERE InvoiceDetailId='" + aInvoiceDetail.InvoiceDetailId + "'  ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }

        public bool UpdateDeliveryInformation(int invId)
        {
            string updateQuery = @"UPDATE tblInvoice SET DeliveryTpTotal=NULL,DeliveryTpDiscount=NULL,DeliveryTpVat=NULL," +
                                 "DeliveryTpGrandTotal=NULL,DeliveryInvoiceStatus=NULL,DelivaryInvoiceNo=NULL,DelivarySpecialAmount=NULL,UpdateBy=NULL,UpdateDate=NULL WHERE InvoiceId='" + invId + "'  ";
            return aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }

        public DataTable GetBonusCampaignInfo(string productCode, DateTime invoiceDate, TimeSpan time,Int32 Qty)
        {
            string query = @"SELECT *, FLOOR(" + Qty +"/Quantity) AS BonusQty FROM tblBonusCampgainDetail " +
                " LEFT JOIN dbo.tblProduct ON BonusProductCode = ProductCode" +
" WHERE PointProductCode = '" + productCode + "'  AND ( '" + invoiceDate + "' BETWEEN FromDate + CAST(FromTime AS DATETIME) AND ToDate + CAST(ToTime AS DATETIME))  AND IsActive = 1";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadAdjustableInvoiceNo(DropDownList ddl, string customerCode)
        {
            string query = @"SELECT A.* FROM dbo.tblArea A LEFT JOIN dbo.tblDistrict D ON A.DistrictId = D.DistrictId " +
                               " LEFT JOIN dbo.tblCompanyUnit C ON D.ComUnitId = C.ComUnitId " +
                               " WHERE C.ComUnitId='" + customerCode.Trim() + "'";
            aCommonInternalDal.LoadDropDownValue(ddl, "AreaName", "AreaId", query, "SSIDB");
        }

        public DataTable GetFixedDiscount(string customerId, string unitId)
        {
            string query = @"SELECT DISTINCT DiscountParcentage FROM tblCustomerFixedDiscount AS FDC 
                             LEFT JOIN tblCompanyInfo AS CI ON FDC.CompanyId = CI.CompanyId
                             LEFT JOIN tblCompanyUnit AS UT ON UT.CompanyId = CI.CompanyId
                             WHERE CustMasterId = " + customerId + " AND UT.ComUnitId = " + unitId + " AND IsActive = 1 AND ActionStatus = 'Accepted'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public int SaveInvoiceMater(Invoice aInvoice)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@InvoiceDate", aInvoice.InvoiceDate));
            aSqlParameterlist.Add(new SqlParameter("@OrderNo", aInvoice.OrderNo));
            aSqlParameterlist.Add(new SqlParameter("@OrderDate", aInvoice.OrderDate));
            aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aInvoice.CustomerMasterId));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aInvoice.ComUnitId));
            aSqlParameterlist.Add(new SqlParameter("@MiaId", aInvoice.MiaId));
            aSqlParameterlist.Add(new SqlParameter("@PaymentTypeId", aInvoice.PaymentTypeId));
            aSqlParameterlist.Add(new SqlParameter("@TpTotal", aInvoice.TpTotal));
            aSqlParameterlist.Add(new SqlParameter("@TpDiscount", aInvoice.TpDiscount));
            aSqlParameterlist.Add(new SqlParameter("@TpVat", aInvoice.TpVat));
            aSqlParameterlist.Add(new SqlParameter("@TpGrandTotal", aInvoice.TpGrandTotal));
            aSqlParameterlist.Add(new SqlParameter("@UserId", aInvoice.UserId));
            aSqlParameterlist.Add(new SqlParameter("@OrderId", aInvoice.OrderId));
            aSqlParameterlist.Add(new SqlParameter("@TotalSpecialAmount", aInvoice.TotalSpecialAmount));
            aSqlParameterlist.Add(new SqlParameter("@ProductOffer", aInvoice.ProductOffer));
            aSqlParameterlist.Add(new SqlParameter("@OldTradePolicy", aInvoice.OldTradePolicy));
            aSqlParameterlist.Add(new SqlParameter("@Remarks", aInvoice.Remarks));
            aSqlParameterlist.Add(new SqlParameter("@DeliveryManId", aInvoice.DeliveryManId));
            aSqlParameterlist.Add(new SqlParameter("@UnitCode", aInvoice.ComUnitCode));
            aSqlParameterlist.Add(new SqlParameter("@PONumber", aInvoice.PONumber));

            return aCommonInternalDal.SaveAction("sp_I_InvoiceMaster", aSqlParameterlist, "@InvoiceId");
        }

        public DataTable GetInvoiceNoById(int invId)
        {
            string query = @"SELECT InvoiceNo,InvoiceId FROM tblInvoice WHERE InvoiceId  = " + invId ;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetBonusCampaignInfoByCompany(string productCode, DateTime invDate, string orderId)
        {
//            string query = @"SELECT CPM.CampgainMasterId,CPM.BonusProductQuantity,ProductCode AS BonusProductCode,ProductName + ':' + PackSize ProductName FROM Table_CPM_BonusCampaignMaster AS CPM
//                             LEFT JOIN tblCompanyInfo AS CI ON CPM.CompanyId = CI.CompanyId
//                             LEFT JOIN tblProduct AS PD ON CPM.BonusProductId = PD.ProductId
//                             WHERE ProductCode = " + productCode + @" AND CPM.CompanyId IN (SELECT DISTINCT CompanyId FROM tblOrder AS ODR LEFT JOIN tblCompanyUnit 
//                             AS UNT ON ODR.ComUnitId = UNT.ComUnitId WHERE OrderId = " + orderId + ") AND GETDATE() BETWEEN CPM.FromDate AND CPM.Todate";


            string query = @" SELECT 
    CPM.CampgainMasterId,
    CPM.BonusProductQuantity,
    PD.ProductCode AS BonusProductCode,
    PD.ProductName + ':' + PD.PackSize AS ProductName
FROM 
    Table_CPM_BonusCampaignMaster AS CPM
INNER JOIN Table_CPM_BonusCampaignDetail AS CPD 
    ON CPM.CampgainMasterId = CPD.CampgainMasterId
LEFT JOIN tblProduct AS PD 
    ON CPM.BonusProductId = PD.ProductId
WHERE 
    CPD.PointProductId = (SELECT ProductId FROM tblProduct WHERE ProductCode = '" + productCode + @"')
    AND CPM.CompanyId IN (
        SELECT DISTINCT CompanyId
        FROM tblOrder AS ODR 
        LEFT JOIN tblCompanyUnit AS UNT ON ODR.ComUnitId = UNT.ComUnitId 
        WHERE OrderId = " + orderId + @" ) AND GETDATE() BETWEEN CPM.FromDate AND CPM.ToDate";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetOrderDetailsByOrderId(int orderId)
        {
            string query = @"SELECT * FROM tblOrderDetail WHERE OrderId =" + orderId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCampaignDetailsByCampaignId(int campaignId)
        {
            string query = @"SELECT * FROM Table_CPM_BonusCampaignDetail AS CPD
            LEFT JOIN tblProduct AS PD ON CPD.PointProductId = PD.ProductId WHERE CampgainMasterId = " + campaignId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetGroupInvoiceNo(string invoiceNo)
        {
            string query = @"SELECT DISTINCT GroupInvoiceNo FROM tblInvoice WHERE InvoiceNo IN (" + invoiceNo + ")";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
