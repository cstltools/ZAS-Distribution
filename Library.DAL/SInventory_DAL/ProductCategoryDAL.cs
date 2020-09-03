using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class ProductCategoryDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveProductCategory(ProductCategory aProductCategory)
        {
            string insertQuery = @"insert into tblProCategory (CategoryId,CategoryCode,CategoryName) 
            values (" + aProductCategory.CategoryId + ",'" + aProductCategory.CategoryCode + "','" + aProductCategory.CategoryName + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasCustCategoryName(ProductCategory aCategory)
        {
            string query = "select * from tblProCategory where CategoryName = '" + aCategory.CategoryName + "'";
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

        public DataTable LoadCategoryView()
        {
            string query = @"SELECT * from tblProCategory ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public ProductCategory ProductCategoryEditLoad(string CategoryId)
        {
            string query = "select * from tblProCategory where CategoryId = '" + CategoryId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ProductCategory aCategory = new ProductCategory();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCategory.CategoryId = Int32.Parse(dataReader["CategoryId"].ToString());
                    aCategory.CategoryCode = dataReader["CategoryCode"].ToString();
                    aCategory.CategoryName = dataReader["CategoryName"].ToString();
                }
            }
            return aCategory;
        }

        public bool UpdateProCategoryInfo(ProductCategory aCategory)
        {

            string query = @"UPDATE tblProCategory SET CategoryName='" + aCategory.CategoryName + "' WHERE CategoryId=" + aCategory.CategoryId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
