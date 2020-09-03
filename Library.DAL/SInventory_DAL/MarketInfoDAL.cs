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
    public class MarketInfoDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public bool SaveMarketInfo(MarketInfo aMarketInfo)
        {
            string insertQuery = @"insert into tblMarket (MarketCode,MarketName,MktShortName,TerritoryId,IsActive,Description,Remarks,EntryBy,EntryDate) 
            values (" + "'" + aMarketInfo.MarketCode + "','" + aMarketInfo.MarketName + "','" + aMarketInfo.MktShortName + "','" + aMarketInfo.TerritoryId + "','" + aMarketInfo.IsActive + "','" + aMarketInfo.Description + "','" + aMarketInfo.Remarks + "','" + aMarketInfo.EntryBy + "','" + aMarketInfo.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasMarketName(MarketInfo aMarketInfo)
        {
            string query = "select * from tblMarket where MarketName = '" + aMarketInfo.MarketName + "'";
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
            aInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void LoadRegionName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            const string queryStr = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive ='True'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }
        public void LoadComUnit(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyUnit";
            aInternalDal.LoadDropDownValue(ddl, "ComUnitName", "ComUnitId", queryStr, "SSIDB");
        }
        public void LoadDistrictName(DropDownList ddl, string regionId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblDistrict where RegionId='" + regionId + "'";
            aInternalDal.LoadDropDownValue(ddl, "DistrictName", "DistrictId", queryStr, "SSIDB");
        }
        public void LoadArea(DropDownList ddl, string distId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblArea WHERE DistrictId='" + distId + "'";
            aInternalDal.LoadDropDownValue(ddl, "AreaName", "AreaId", queryStr, "SSIDB");
        }

        public DataTable LoadMarketView()
        {
            const string query = @"SELECT * FROM tblMarket AS MKT 
                            INNER JOIN tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId
                            INNER JOIN tblArea AS AR ON TTR.AreaId = AR.AreaId
                            INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public MarketInfo MarketInfoEditLoad(string MarketId)
        {
            string query = @"SELECT * FROM tblMarket AS MKT 
                            INNER JOIN tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId
                            INNER JOIN tblArea AS AR ON TTR.AreaId = AR.AreaId
                            INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId where MarketId = '" + MarketId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");

            var aMarketInfo = new MarketInfo();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aMarketInfo.MarketId = Int32.Parse(dataReader["MarketId"].ToString());
                    aMarketInfo.MarketCode = dataReader["MarketCode"].ToString();
                    aMarketInfo.MarketName = dataReader["MarketName"].ToString();
                    aMarketInfo.MktShortName = dataReader["MktShortName"].ToString();
                    aMarketInfo.AreaId = Convert.ToInt32(dataReader["AreaId"].ToString());
                    aMarketInfo.RegionId = Convert.ToInt32(dataReader["RegionId"].ToString());
                    aMarketInfo.TerritoryId = Convert.ToInt32(dataReader["TerritoryId"].ToString());

                    aMarketInfo.Remarks = dataReader["Remarks"].ToString();
                    aMarketInfo.Description = dataReader["Description"].ToString();
                    aMarketInfo.IsActive = Convert.ToBoolean(dataReader["IsActive"].ToString());
                }
            }
            return aMarketInfo;
        }

        public bool UpdateMarketInfo(MarketInfo aMarketInfo)
        {
            string query = @"UPDATE tblMarket SET MarketName='" + aMarketInfo.MarketName + "',TerritoryId='" + aMarketInfo.TerritoryId + "',MktShortName='" + aMarketInfo.MktShortName + "',MarketCode='" + aMarketInfo.MarketCode + "',Description='" + aMarketInfo.Description + "',Remarks='" + aMarketInfo.Remarks + "',IsActive='" + aMarketInfo.IsActive + "',UpdateBy='" + aMarketInfo.UpdateBy + "',UpdateDate='" + aMarketInfo.UpdateDate + "' WHERE MarketId=" + aMarketInfo.MarketId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public void LoadAreaName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblArea ";
            aInternalDal.LoadDropDownValue(ddl, "AreaName", "AreaId", queryStr, "SSIDB");
        }
        public void LoadCompanyUnitName(DropDownList ddl, string ComUnitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyUnit where ComUnitId='" + ComUnitId + "' ";
            aInternalDal.LoadDropDownValue(ddl, "ComUnitName", "ComUnitId", queryStr, "SSIDB");
        }


        public void GetAreaOnDropdownList(DropDownList ddl, string regionId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                WHERE AR.IsActive ='True' AND AR.RegionId = '" + regionId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }

        public void GetTerritoryOnDropdownList(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT distinct TTR.TerritoryId, TTR.TerritoryCode + ' : ' + TTR.TerritoryName AS TerritoryName FROM tblTerritory AS TTR 
                                WHERE TTR.IsActive ='True' AND TTR.AreaId = '" + areaId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
        }

        public DataTable NameExistOrNot(string name)
        {
            string queryStr = "select * from tblMarket where MarketName ='" + name + "' ";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable CodeExistOrNot(string code)
        {
            string queryStr = "select * from tblMarket where MarketCode ='" + code + "' ";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable ShortNameExistOrNot(string srtName)
        {
            string queryStr = "select * from tblMarket where MktShortName ='" + srtName + "' ";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }
    }
}
