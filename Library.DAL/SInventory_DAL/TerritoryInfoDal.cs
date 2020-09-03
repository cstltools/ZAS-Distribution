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
    public class TerritoryInfoDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public bool SaveAreaInfo(TerritoryInfoDao territoryInfo)
        {
            string insertQuery = @"insert into tblTerritory (TerritoryCode,TerritoryName,TerShortName,AreaId,IsActive,Description,Remarks,EntryBy,EntryDate) 
            values (" + "'" + territoryInfo.TerritoryCode + "','" + territoryInfo.TerritoryName + "','" + territoryInfo.TerShortName + "','" + territoryInfo.AreaId + "','" + territoryInfo.IsActive + "','" + territoryInfo.Description + "','" + territoryInfo.Remarks + "','" + territoryInfo.EntryBy + "','" + territoryInfo.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasAreaName(TerritoryInfoDao territoryInfo)
        {
            string query = "SELECT * FROM tblTerritory WHERE TerritoryName = '" + territoryInfo.TerritoryName + "'";
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

        public DataTable LoadAreaView()
        {
            const string query = @"SELECT * FROM tblTerritory AS TTR 
                                  INNER JOIN tblArea AS AR ON TTR.AreaId = AR.AreaId
                                  INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public TerritoryInfoDao AreaEditLoad(string territoryId)
        {
            string query = @"SELECT * FROM tblTerritory AS TTR 
                                  INNER JOIN tblArea AS AR ON TTR.AreaId = AR.AreaId
                                  INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId WHERE TTR.TerritoryId = '" + territoryId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");

            var areaInfo = new TerritoryInfoDao();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    areaInfo.TerritoryId = Int32.Parse(dataReader["TerritoryId"].ToString());
                    areaInfo.TerritoryName = dataReader["TerritoryName"].ToString();
                    areaInfo.TerritoryCode = dataReader["TerritoryCode"].ToString();
                    areaInfo.TerShortName = dataReader["TerShortName"].ToString();
                    areaInfo.AreaId = Convert.ToInt32(dataReader["AreaId"].ToString());
                    areaInfo.Remarks = dataReader["Remarks"].ToString();
                    areaInfo.Description = dataReader["Description"].ToString();
                    areaInfo.IsActive = Convert.ToBoolean(dataReader["IsActive"].ToString());
                    areaInfo.RegionId = Int32.Parse(dataReader["RegionId"].ToString());
                }
            }
            return areaInfo;
        }

        public bool UpdateAreaInfo(TerritoryInfoDao territoryInfo)
        {
            string query = @"UPDATE tblTerritory SET TerritoryName='" + territoryInfo.TerritoryName + "',TerritoryCode='" + territoryInfo.TerritoryCode + "',AreaId='" + territoryInfo.AreaId + "',Description='" + territoryInfo.Description + "',TerShortName='" + territoryInfo.TerShortName + "',Remarks='" + territoryInfo.Remarks + "',IsActive='" + territoryInfo.IsActive + "',UpdateBy='" + territoryInfo.UpdateBy + "',UpdateDate='" + territoryInfo.UpdateDate + "' WHERE TerritoryId = '" + territoryInfo.TerritoryId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public void LoadMiaName(DropDownList ddl, string distId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblMIAInfo WHERE DistrictId='" + distId + "'";
            aInternalDal.LoadDropDownValue(ddl, "MiaName", "MiaId", queryStr, "SSIDB");
        }
        public void LoadDistrictName(DropDownList ddl, string regionId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblDistrict where RegionId='" + regionId + "'";
            aInternalDal.LoadDropDownValue(ddl, "DistrictName", "DistrictId", queryStr, "SSIDB");
        }
        public void LoadRegionName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            const string queryStr = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive ='True'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }

        public void GetAreaOnDropdownList(DropDownList ddl, string regionId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                WHERE AR.IsActive ='True' AND AR.RegionId = '" + regionId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }

        public DataTable CodeExistOrNot(string code)
        {
            string query = @"SELECT * FROM tblTerritory WHERE TerritoryCode = '" + code + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable NameExistOrNot(string name)
        {
            string query = @"SELECT * FROM tblTerritory WHERE TerritoryName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ShortNameExistOrNot(string shortname)
        {
            string query = @"SELECT * FROM tblTerritory WHERE TerShortName = '" + shortname + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
