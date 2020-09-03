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
    public class RegionDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        
        public bool SaveRegionInfo(RegionInfo aRegionInfo)
        {
            string insertQuery = @"insert into tblRegion (RegionCode,RegionName,RgnShortName,IsActive,Description,Remarks,EntryBy,EntryDate) 
            values (" + "'" + aRegionInfo.RegionCode + "','" + aRegionInfo.RegionName + "','" + aRegionInfo.RgnShortName + "','" + aRegionInfo.IsActive + "','" + aRegionInfo.Description + "','" + aRegionInfo.Remarks + "','" + aRegionInfo.EntryBy + "','" + aRegionInfo.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasRegionName(RegionInfo aRegionInfo)
        {
            string query = "select * from tblRegion where RegionName = '" + aRegionInfo.RegionName + "'";
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

        public void LoadCompanyName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyInfo";
            aInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr,"SSIDB");
        }


        public DataTable LoadRegionInfo()
        {
            const string query = @"SELECT * from tblRegion ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public RegionInfo RegionInfoEditLoad(string RegionId)
        {
            string query = "select * from tblRegion where RegionId = '" + RegionId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            RegionInfo aRegionInfo = new RegionInfo();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aRegionInfo.RegionId = Int32.Parse(dataReader["RegionId"].ToString());
                    aRegionInfo.RegionCode = dataReader["RegionCode"].ToString();
                    aRegionInfo.RegionName = dataReader["RegionName"].ToString();
                    aRegionInfo.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                    aRegionInfo.Description = dataReader["Description"].ToString();
                    aRegionInfo.Remarks = dataReader["Remarks"].ToString();
                    aRegionInfo.RgnShortName = dataReader["RgnShortName"].ToString();
                }

            }
            return aRegionInfo;
        }

        public bool UpdateRegionInfo(RegionInfo aRegionInfo)
        {

            string query = @"UPDATE tblRegion SET RegionName='" + aRegionInfo.RegionName + "',RegionCode='" + aRegionInfo.RegionCode + "',RgnShortName='" + aRegionInfo.RgnShortName + "',Description='" + aRegionInfo.Description + "',Remarks='" + aRegionInfo.Remarks + "',IsActive='" + aRegionInfo.IsActive + "',UpdateBy='" + aRegionInfo.UpdateBy + "',UpdateDate='" + aRegionInfo.UpdateDate + "' WHERE RegionId=" + aRegionInfo.RegionId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable CheckRegionCodeExistOrNot(string code)
        {
             string query = @"SELECT * from tblRegion WHERE  RegionCode = '" + code + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable NameExistOrNot(string name)
        {
            string query = @"SELECT * from tblRegion WHERE  RegionName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ShortNameExistOrNot(string shortname)
        {
            string query = @"SELECT * from tblRegion WHERE RgnShortName = '" + shortname + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
