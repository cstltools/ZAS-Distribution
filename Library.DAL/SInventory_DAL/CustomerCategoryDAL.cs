using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;


namespace Library.DAL.SInventory_DAL
{
    public class CustomerCategoryDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveCustomerCategory(CustomerCategory aCustomerCategory)
        {
            string insertQuery = @"insert into tblCustCategory (CategoryId,CategoryCode,CategoryName) 
            values (" + aCustomerCategory.CategoryId + ",'" + aCustomerCategory.CategoryCode + "','" + aCustomerCategory.CategoryName + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasCustCategoryName(CustomerCategory aCategory)
        {
            string query = "select * from tblCustCategory where CategoryName = '" + aCategory.CategoryName + "'";
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

        public DataTable LoadCustCategoryView()
        {
            string query = @"SELECT * from tblCustCategory ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public CustomerCategory CustomerCategoryEditLoad(string categoryId)
        {
            string query = "select * from tblCustCategory where CategoryId = '" + categoryId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CustomerCategory aCategory = new CustomerCategory();
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

        public bool UpdateCustCategoryInfo(CustomerCategory aCategory)
        {

            string query = @"UPDATE tblCustCategory SET CategoryName='" + aCategory.CategoryName + "' WHERE CategoryId=" + aCategory.CategoryId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
