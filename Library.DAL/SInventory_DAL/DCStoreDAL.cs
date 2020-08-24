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
    public class DCStoreDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDataForDhakaStock(DCStore aReceive)
        {
            string insertQuery = @"insert into tblDCStore (ReceiveId,ProductCode,ProductName,PackSize,BatchNo,Quantity,ExpDate,ReceiveDate,ChalanNo,ChalanDate,ComUnitId,StorageLocation) 
            values (" + aReceive.StockId + ",'" + aReceive.ProductCode + "','" + aReceive.ProductName + "','" + aReceive.PackSize + "','" + aReceive.BatchNo + "','" + aReceive.Quantity + "','" + aReceive.ExpDate + "','" + aReceive.ReceiveDate + "','" + aReceive.ChalanNo + "','" + aReceive.ChalanDate + "','" + aReceive.ComUnitId + "','" + aReceive.StorageLocation + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        
        public bool SaveDataForCurrentStock(CurrentStock aReceive)
        {
            string insertQuery = @"insert into tblCurrentStock (StockId,ProductCode,ProductName,PackSize,Quantity,ComUnitId,ComUnitCode) 
            values (" + aReceive.StockId + ",'" + aReceive.ProductCode + "','" + aReceive.ProductName + "','" + aReceive.PackSize + "','" + aReceive.Quantity + "','" + aReceive.ComUnitId + "','" + aReceive.StorageLocation + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasProductcode(CurrentStock aReceive)
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

        public DataTable LoadCountryStockView()
        {
            string query = @"SELECT * FROM tblDCStore ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DCStore DhakaStockEditLoad(string ReceiveId)
        {
            string query = "select * from tblDCStore where ReceiveId = '" + ReceiveId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            DCStore aReceive = new DCStore();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aReceive.StockId = Int32.Parse(dataReader["ReceiveId"].ToString());
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
       
        public bool UpdateDhakaStock(DCStore aReceive)
        {
            string query = @"UPDATE tblDCStore SET ProductName='" + aReceive.ProductName + "',ProductCode='" + aReceive.ProductCode + "',PackSize='" + aReceive.PackSize + "',BatchNo='" + aReceive.BatchNo + "',Quantity='" + aReceive.Quantity + "',ExpDate='" + aReceive.ExpDate + "',ReceiveDate='" + aReceive.ReceiveDate + "' WHERE ReceiveId=" + aReceive.StockId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
       
        public DataTable StockCheck(string stockid)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * from tblCurrentStock where StockId = '" + stockid.Trim() + "'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }
        public DataTable LoadProductCode(string productCode, string companyId)
        {
            DataTable aDataTable = new DataTable();

            string query = @"SELECT * FROM tblProduct AS PD 
                            INNER JOIN tblUnitPrice AS UP ON PD.ProductId = UP.ProductId
                           WHERE  UP.ActionStatus = 'Accepted' AND UP.IsActive = 'True' AND PD.ProductCode = '" + productCode + "' AND PD.CompanyId = '" + companyId + "'";
            aDataTable= aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTable;
        }
        public DataTable LoadProductCode(string productId)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT * FROM tblProduct left join tblUnitPrice on tblProduct.ProductCode=tblUnitPrice.ProductCode where tblProduct.ProductCode='" + productId.Trim() + "' ";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTable;
        }
        public void UpdateCurrentStockQuantity(string stockId,string Quantity)
        {
            string updateQuery = @"UPDATE tblCurrentStock SET Quantity='" + Quantity + "' WHERE ProductCode='" + stockId.Trim() + "' ";
            aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        }
        
    }
}
