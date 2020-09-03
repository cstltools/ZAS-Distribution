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
    public class ProductSQDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveProductSQ(ProductSQ aProductSQ)
        {
            string insertQuery = @"insert into tblProductSQ (ProductBrandId,ProductSQName,IngridentsId) 
            values (" + aProductSQ.ProductBrandId + ",'" + aProductSQ.ProductSQName + "','" + aProductSQ.IngridentsId + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public void LoadIngrident(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM tblIngridents";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "IngridentsName", "IngridentsId", queryStr);
        }
        public bool HasProductSQName(ProductSQ aProductSQ)
        {
            string query = "select * from tblProductSQ where ProductSQName = '" + aProductSQ.ProductSQName + "'";
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

        public DataTable LoadProductSQ()
        {
            string query = @"SELECT * from tblProductSQ
            LEFT JOIN dbo.tblIngridents ON dbo.tblProductSQ.IngridentsId = dbo.tblIngridents.IngridentsId ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public ProductSQ ProductSQEditLoad(string ID)
        {
            string query = "select * from tblProductSQ where ProductBrandId = '" + ID + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ProductSQ aProductSQ = new ProductSQ();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aProductSQ.ProductBrandId = Int32.Parse(dataReader["ProductBrandId"].ToString());
                    aProductSQ.ProductSQName = dataReader["ProductSQName"].ToString();
                    aProductSQ.IngridentsId = Convert.ToInt32(dataReader["IngridentsId"].ToString());
                }
            }
            return aProductSQ;
        }

        public bool UpdateProductSQInfo(ProductSQ aProductSQ)
        {

            string query = @"UPDATE tblProductSQ SET ProductSQName='" + aProductSQ.ProductSQName + "',IngridentsId='" + aProductSQ.IngridentsId + "' WHERE ProductBrandId=" + aProductSQ.ProductBrandId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool DeleteProductBrand(string id)
        {
            string query = @"DELETE FROM dbo.tblProductSQ WHERE ProductBrandId='"+id+"'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
