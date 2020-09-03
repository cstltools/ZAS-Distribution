using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class BatchWiseTopSheetDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void LoadManufac(DropDownList ddl)
        {            
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void LoadDeliveryManList(DropDownList ddl)
        {
            string query = @"SELECT *,EmpName as DeliveryManName FROM dbo.tblDeliveryManInfo
            inner join tblEmpGeneralInfo on tblDeliveryManInfo.EmpInfoId=tblEmpGeneralInfo.EmpInfoId";
            aInternalDal.LoadDropDownValue(ddl, "DeliveryManName", "DeliveryManId", query, "SSIDB");
        }

//        public DataTable GetBatchWiseTopSheet(int companyId, int delManId, int batchId, DateTime invDate)
//        {
//        string query = @"SELECT Cu.ComUnitName,
//       INV.InvoiceNo,
//       INV.InvoiceDate,
//       INV.OrderNo,
//       INV.OrderDate,
//	   INV.BatchId,
//       EGI.EmpMasterCode,
//       EGI.EmpName,
//       EGI.CellNumber  FROM dbo.tblInvoice AS INV
//	   INNER JOIN dbo.tblCompanyUnit AS CU ON CU.ComUnitId = INV.ComUnitId
//       INNER JOIN dbo.tblDeliveryManInfo AS DLMAN ON DLMAN.DeliveryManId = INV.DeliveryManId
//       INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON EGI.EmpInfoId = DLMAN.EmpInfoId
//       WHERE INV.DeliveryManId = '" + delManId + "' AND INV.ComUnitId = '" + companyId + "' AND INV.BatchId = '" + batchId + "' AND INV.InvoiceDate = '" + invDate + "'";

//       return aInternalDal.DataContainerDataTable(query, "SSIDB");

//        }



        public DataTable GetBatchWiseTopSheet( int delManId, int batchId, DateTime invDate)
        {
            string query = @"SELECT CSTMR.CustomerName,CI.ComUnitName AS CompanyName,INV.OrderNo,INV.OrderDate, INV.InvoiceNo, INV.InvoiceDate,EGI.EmpMasterCode,EGI.EmpName,EGI.CellNumber, INVB.BatchNo,
                            INVD.ProductCode, INVD.ProductName,  INVD.PackSize,ORD.Quantity AS OrderQty, INVD.TotalQuantity AS InvQty, INVD.UnitPrice, INVD.UnitVatAmount,
                            INVD.SpecialAmount, INVD.TotalPrice ,INVD.TotalPriceVatAmount, INVD.NetAmount FROM dbo.tblInvoice AS INV 
                            INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
                            INNER JOIN dbo.tblOrderDetail AS ORD ON ORD.OrderDetailId = INVD.OrderDetailsId

                            INNER JOIN dbo.tblDeliveryManInfo AS DELI ON DELI.DeliveryManId = INV.DeliveryManId
                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON DELI.EmpInfoId = EGI.EmpInfoId
                            INNER JOIN dbo.tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
                            INNER JOIN dbo.tblInvoiceBatch AS INVB ON INVB.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            WHERE INV.DeliveryManId = '" + delManId + "' AND INV.BatchId = '" + batchId + "' AND INV.InvoiceDate = '" + invDate + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");

        }

        public DataTable BatchWiseDeliveryTopSheet(int delManId, int batchId, DateTime invDate)
        {
            string query = @"SELECT  CSTMR.CustomerName ,CI.ComUnitName AS CompanyName,INV.OrderNo,INV.OrderDate, INV.InvoiceNo, INV.InvoiceDate,EGI.EmpMasterCode,EGI.EmpName,EGI.CellNumber, INVB.BatchNo,
                            INVD.ProductCode, INVD.ProductName,  INVD.PackSize,ORD.Quantity AS OrderQty, INVD.Quantity AS InvQty,INVD.DeliveryQuantity, INVD.UnitPrice, INVD.UnitVatAmount,
                            INVD.DelivarySpecialAmount, INVD.DeliveryTotalPrice ,INVD.DeliveryTotalPriceVatAmount, INVD.DeliveryNetAmount FROM dbo.tblInvoice AS INV 
                            INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
                            INNER JOIN dbo.tblOrderDetail AS ORD ON ORD.OrderDetailId = INVD.OrderDetailsId
                            
                            INNER JOIN dbo.tblDeliveryManInfo AS DELI ON DELI.DeliveryManId = INV.DeliveryManId
                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON DELI.EmpInfoId = EGI.EmpInfoId
                            INNER JOIN dbo.tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
                            INNER JOIN dbo.tblInvoiceBatch AS INVB ON INVB.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                            WHERE INVD.DeliveryStatus IS NOT NULL AND INV.DeliveryManId = '" + delManId + "' AND INV.BatchId = '" + batchId + "' AND INV.InvoiceDate = '" + invDate + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");

        }

//        public DataTable GetBatchWiseTopSheetDetail(int companyId, int delManId, int batchId, DateTime invDate)
//        {
//            string query = @"SELECT INVD.ProductCode, INVD.ProductName,  INVD.PackSize, INVD.Quantity, INVD.UnitPrice, INVD.UnitVatAmount,   INVD.SpecialAmount, INVD.TotalPrice ,INVD.TotalPriceVatAmount, INVD.NetAmount
//                FROM dbo.tblInvoice AS INV 
//                INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
//                WHERE INV.DeliveryManId = '" + delManId + "' AND INV.ComUnitId = '" + companyId + "' AND INV.BatchId = '" + batchId + "' AND INV.InvoiceDate = '" + invDate + "'";

//            return aInternalDal.DataContainerDataTable(query, "SSIDB");
//        }

        public DataTable GetBatchWiseTopSheetDetail(int delManId, int batchId, DateTime invDate)
        {
            string query = @"SELECT CI.CompanyName,INV.OrderNo,INV.OrderDate, INV.InvoiceNo, INV.InvoiceDate,EGI.EmpMasterCode,EGI.EmpName,EGI.CellNumber, INVB.BatchNo,
INVD.ProductCode, INVD.ProductName,  INVD.PackSize,ORD.Quantity AS OrderQty, INVD.Quantity AS InvQty, INVD.UnitPrice, INVD.UnitVatAmount,
INVD.SpecialAmount, INVD.TotalPrice ,INVD.TotalPriceVatAmount, INVD.NetAmount FROM dbo.tblInvoice AS INV 
INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
INNER JOIN dbo.tblOrderDetail AS ORD ON ORD.OrderDetailId = INVD.OrderDetailsId

INNER JOIN dbo.tblDeliveryManInfo AS DELI ON DELI.DeliveryManId = INV.DeliveryManId
INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON DELI.EmpInfoId = EGI.EmpInfoId
INNER JOIN dbo.tblCompanyInfo AS CI ON INV.ComUnitId = CI.CompanyId
INNER JOIN dbo.tblInvoiceBatch AS INVB ON INVB.BatchId = INV.BatchId
                WHERE INV.DeliveryManId = '" + delManId  + "' AND INV.BatchId = '" + batchId + "' AND INV.InvoiceDate = '" + invDate + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ReturnOnDeliveryReport(int delManId, int batchId, DateTime invDate)
        {
            string query = @"SELECT CI.CompanyName,INV.OrderNo,INV.OrderDate, INV.InvoiceNo, INV.InvoiceDate,EGI.EmpMasterCode,EGI.EmpName,EGI.CellNumber, INVB.BatchNo,
                             INVD.ProductCode, INVD.ProductName,  INVD.PackSize,ORD.Quantity AS OrderQty, INVD.Quantity AS InvQty, INVD.UnitPrice, INVD.UnitVatAmount,
                             INVD.SpecialAmount, INVD.TotalPrice ,INVD.TotalPriceVatAmount, INVD.NetAmount FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
                             INNER JOIN dbo.tblOrderDetail AS ORD ON ORD.OrderDetailId = INVD.OrderDetailsId
                             
                             INNER JOIN dbo.tblDeliveryManInfo AS DELI ON DELI.DeliveryManId = INV.DeliveryManId
                             INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON DELI.EmpInfoId = EGI.EmpInfoId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON INV.ComUnitId = CI.CompanyId
                             INNER JOIN dbo.tblInvoiceBatch AS INVB ON INVB.BatchId = INV.BatchId
                WHERE INV.DeliveryManId = '" + delManId + "' AND INV.BatchId = '" + batchId + "' AND INV.InvoiceDate = '" + invDate + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable BatchWiseDeliveryReturnTopSheet(int delManId, int batchId, DateTime invDate)
        {
            string query = @"SELECT CSTMR.CustomerCode,CSTMR.CustomerName,CI.ComUnitName AS Depot, C.CompanyName,INV.OrderNo,INV.OrderDate, 
                            INV.InvoiceNo, INV.InvoiceDate,EGI.EmpMasterCode,EGI.EmpName,EGI.CellNumber, INVB.BatchNo,INVD.UnitPrice,
                            INVD.ProductCode, INVD.ProductName,  INVD.PackSize,(INVD.Quantity-INVD.DeliveryQuantity)Quantity,
                            (INVD.NetAmount-INVD.DeliveryNetAmount)Amount, INVD.ReturnReason FROM dbo.tblInvoice AS INV 
                            INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
                            INNER JOIN dbo.tblOrderDetail AS ORD ON ORD.OrderDetailId = INVD.OrderDetailsId
                            INNER JOIN dbo.tblDeliveryManInfo AS DELI ON DELI.DeliveryManId = INV.DeliveryManId
                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON DELI.EmpInfoId = EGI.EmpInfoId
                            INNER JOIN dbo.tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
                            INNER JOIN dbo.tblCompanyInfo AS C ON CI.CompanyId = C.CompanyId
                            INNER JOIN dbo.tblInvoiceBatch AS INVB ON INVB.BatchId = INV.BatchId
                            INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                             WHERE INVD.DeliveryStatus IN ('Reject','Partial') AND INV.DeliveryManId = '" + delManId + "' AND INV.BatchId = '" + batchId + "' AND INV.InvoiceDate = '" + invDate + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
