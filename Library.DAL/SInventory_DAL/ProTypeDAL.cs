using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class ProTypeDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveProType(ProType aProType)
        {
            string insertQuery = @"insert into tblProType (ProTypeId,ProTypeName) 
            values (" + aProType.ProTypeId + ",'" + aProType.ProTypeName + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasProTypeName(ProType aProType)
        {
            string query = "select * from tblProType where ProTypeName = '" + aProType.ProTypeName + "'";
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

        public DataTable LoadProType()
        {
            string query = @"SELECT * from tblProType ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public ProType ProTypeEditLoad(string ID)
        {
            string query = "select * from tblProType where ProTypeId = '" + ID + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ProType aProType = new ProType();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aProType.ProTypeId = Int32.Parse(dataReader["ProTypeId"].ToString());
                    aProType.ProTypeName = dataReader["ProTypeName"].ToString();
                }
            }
            return aProType;
        }

        public bool UpdateProTypeInfo(ProType aProType)
        {

            string query = @"UPDATE tblProType SET ProTypeName='" + aProType.ProTypeName + "' WHERE ProTypeId=" + aProType.ProTypeId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
