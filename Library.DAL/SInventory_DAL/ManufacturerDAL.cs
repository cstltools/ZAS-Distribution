using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class ManufacturerDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public bool SaveManufacturer(Manufacturer aManufacturer)
        {
            string insertQuery = @"insert into tblManufacturer (ManufacName,ManufacAddress,ManufacCode,EntryBy,EntryDate) 
            values (" + "'" + aManufacturer.ManufacName + "','" + aManufacturer.ManufacAddress + "','" + aManufacturer.ManufacCode + "','" + aManufacturer.EntryBy + "','" + aManufacturer.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasManufacName(Manufacturer aManufacturer)
        {
            string query = "select * from tblManufacturer where ManufacName = '" + aManufacturer.ManufacName + "'";
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

        public DataTable LoadManufacturer()
        {
            string query = @"SELECT * from tblManufacturer ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public Manufacturer ManufacturerEditLoad(string ID)
        {
            string query = "select * from tblManufacturer where ManufacId = '" + ID + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            Manufacturer aManufacturer = new Manufacturer();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aManufacturer.ManufacId = Int32.Parse(dataReader["ManufacId"].ToString());
                    aManufacturer.ManufacName = dataReader["ManufacName"].ToString();
                    aManufacturer.ManufacAddress = dataReader["ManufacAddress"].ToString();
                    aManufacturer.ManufacCode = dataReader["ManufacCode"].ToString();
                }
            }
            return aManufacturer;
        }

        public bool UpdateManufacturerInfo(Manufacturer aManufacturer)
        {

            string query = @"UPDATE tblManufacturer SET ManufacName='" + aManufacturer.ManufacName + "',ManufacAddress='" + aManufacturer.ManufacAddress + "',UpdateBy='" + aManufacturer.UpdateBy + "',UpdateDate='" + aManufacturer.UpdateDate + "' WHERE ManufacId=" + aManufacturer.ManufacId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable NameExistOrNot(string name)
        {
            string query = "select * from tblManufacturer where ManufacName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
