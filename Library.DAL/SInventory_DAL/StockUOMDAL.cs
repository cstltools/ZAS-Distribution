using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class StockUOMDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        
        public bool SaveStockUOM(StockUOM aStockUOM)
        {
            string insertQuery = @"insert into tblStockUOM (StockUOMName) 
            values (" + "'" + aStockUOM.StockUOMName + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasStockUOMName(StockUOM aStockUOM)
        {
            string query = "select * from tblStockUOM where StockUOMName = '" + aStockUOM.StockUOMName + "'";
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

        public DataTable LoadStockUOM()
        {
            string query = @"SELECT * from tblStockUOM ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public StockUOM StockUOMEditLoad(string ID)
        {
            string query = "select * from tblStockUOM where StockUOMId = '" + ID + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            StockUOM aStockUOM = new StockUOM();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aStockUOM.StockUOMId = Int32.Parse(dataReader["StockUOMId"].ToString());
                    aStockUOM.StockUOMName = dataReader["StockUOMName"].ToString();
                }
            }
            return aStockUOM;
        }

        public bool UpdateStockUOMInfo(StockUOM aStockUOM)
        {
            string query = @"UPDATE tblStockUOM SET StockUOMName='" + aStockUOM.StockUOMName + "' WHERE StockUOMId=" + aStockUOM.StockUOMId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
