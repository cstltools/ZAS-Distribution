using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class PackSizeDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SavePackSize(PackSize aPackSize)
        {
            string insertQuery = @"insert into tblPackSize (PackSizeName) 
            values ('" + aPackSize.PackSizeName + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasPackSizeName(PackSize aPackSize)
        {
            string query = "select * from tblPackSize where PackSizeName = '" + aPackSize.PackSizeName + "'";
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

        public DataTable LoadPackSize()
        {
            string query = @"SELECT * from tblPackSize ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public PackSize PackSizeEditLoad(string ID)
        {
            string query = "select * from tblPackSize where PackSizeId = '" + ID + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            PackSize aPackSize = new PackSize();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aPackSize.PackSizeId = Int32.Parse(dataReader["PackSizeId"].ToString());
                    aPackSize.PackSizeName = dataReader["PackSizeName"].ToString();
                }
            }
            return aPackSize;
        }

        public bool UpdatePackSizeInfo(PackSize aPackSize)
        {

            string query = @"UPDATE tblPackSize SET PackSizeName='" + aPackSize.PackSizeName + "' WHERE PackSizeId=" + aPackSize.PackSizeId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
