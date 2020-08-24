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
    public class CentralStoreDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDataForStockReceive(CentralStore aReceive)
        {
            string insertQuery = @"insert into tblCentralStore (ReceiveId,ProductCode,ProductName,PackSize,BatchNo,Quantity,ExpDate,ReceiveDate,InternalNoteNo,StockInQty,UnitPrice,TotalAmount) 
            values (" + aReceive.ReceiveId + ",'" + aReceive.ProductCode + "','" + aReceive.ProductName + "','" + aReceive.PackSize + "','" + aReceive.BatchNo + "','" + aReceive.Quantity + "','" + aReceive.ExpDate + "','" + aReceive.ReceiveDate + "','" + aReceive.InternalNoteNo + "','" + aReceive.StockInQty + "','" + aReceive.UnitPrice + "','" + aReceive.TotalAmount + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        
        public bool SaveDataForCurrentStock(CurrentStock aReceive)
        {
            string insertQuery = @"insert into tblCurrentStock (StockId,ProductCode,ProductName,PackSize,Quantity,ComUnitId,ComUnitCode) 
            values (" + aReceive.StockId + ",'" + aReceive.ProductCode + "','" + aReceive.ProductName + "','" + aReceive.PackSize + "','" + aReceive.Quantity + "','" + aReceive.ComUnitId + "','" + aReceive.StorageLocation + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasProductName(CurrentStock aReceive)
        {
            string query = "select * from tblCurrentStock where ProductCode = '" + aReceive.ProductCode + "'";
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

        public DataTable CurrentStockQty(CurrentStock aReceive)
        {
            string query = "select * from tblCurrentStock where ProductCode = '" + aReceive.ProductCode + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CurrentStockReport()
        {
            string query = "select ProductCode, ProductName, PackSize, Quantity from tblCurrentStock";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadStockReceiveView()
        {
            string query = @"SELECT * FROM tblCentralStore ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public CentralStore StockReceiveEditLoad(string ReceiveId)
        {
            string query = "select * from tblCentralStore where ReceiveId = '" + ReceiveId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CentralStore aReceive = new CentralStore();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aReceive.ReceiveId = Int32.Parse(dataReader["ReceiveId"].ToString());
                    aReceive.InternalNoteNo = dataReader["InternalNoteNo"].ToString();
                    aReceive.ProductCode = dataReader["ProductCode"].ToString();
                    aReceive.ProductName = dataReader["ProductName"].ToString();
                    aReceive.PackSize = dataReader["PackSize"].ToString();
                    aReceive.BatchNo = dataReader["BatchNo"].ToString();
                    aReceive.Quantity = Convert.ToDecimal(dataReader["Quantity"].ToString());
                    aReceive.ExpDate = Convert.ToDateTime(dataReader["ExpDate"].ToString());
                    aReceive.ReceiveDate = Convert.ToDateTime(dataReader["ReceiveDate"].ToString());
                }
            }
            return aReceive;
        }
       
        public bool UpdateStockReceive(CentralStore aReceive)
        {
            string query = @"UPDATE tblCentralStore SET ProductName='" + aReceive.ProductName + "',ProductCode='" + aReceive.ProductCode + "',PackSize='" + aReceive.PackSize + "',BatchNo='" + aReceive.BatchNo + "',Quantity='" + aReceive.Quantity + "',ExpDate='" + aReceive.ExpDate + "',ReceiveDate='" + aReceive.ReceiveDate + "' WHERE ReceiveId=" + aReceive.ReceiveId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
       
        public DataTable StockCheck(string stockid)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * from tblCurrentStock where StockId = '" + stockid.Trim() + "'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable LoadProduct(string productId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblUnitPrice where IsActive=1 AND ProductCode='" + productId.Trim() + "' ";
            aDataTableEmpInfo= aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
        public void UpdateCurrentStockQuantity(string stockId,string Quantity)
        {
            string updateQuery = @"UPDATE tblCurrentStock SET Quantity='" + Quantity + "' WHERE ProductCode='" + stockId.Trim() + "' ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
        //public bool UpdateCurrentStockQuantity(CurrentStock aReceive)
        //{
        //    string query = @"UPDATE tblCurrentStock SET Quantity='" + aReceive.Quantity + "' WHERE StockId=" + aReceive.StockId + "";
        //    return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        //}
    }
}
