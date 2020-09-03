using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerReportDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void LoadRegionName(DropDownList ddl)
        {
            
            const string queryStr = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive ='True'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }

        public void LoadAreaOnDropdownList(DropDownList ddl, string regionId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                WHERE AR.IsActive ='True' AND AR.RegionId = '" + regionId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }

        public void LoadTerritoryOnDropdownList(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT distinct TTR.TerritoryId, TTR.TerritoryCode + ' : ' + TTR.TerritoryName AS TerritoryName FROM tblTerritory AS TTR 
                                WHERE TTR.IsActive ='True' AND TTR.AreaId = '" + areaId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
        }

        public void LoadMarketOnDropdownList(DropDownList ddl, string territoryId)
        {
            string queryStr = @" SELECT MKT.MarketId, MKT.MarketCode + ' : ' + MKT.MarketName AS Market FROM tblMarket AS MKT
                                WHERE MKT.IsActive ='True' AND MKT.TerritoryId = '" + territoryId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "Market", "MarketId", queryStr);
        }

        public DataTable LoadCustomerInfo(string parameter)
        {
            string queryStr = @"SELECT CustomerCode,CustomerName,ConPerson,Address,CellNo,CategoryName ,tblMarket.MarketCode + ' : ' + tblMarket.MarketName AS Market, 
                                tblTerritory.TerritoryCode + ' : ' + tblTerritory.TerritoryName AS Territory, tblArea.AreaCode + ' : ' + tblArea.AreaName AS Area,
                                tblRegion.RegionCode + ' : ' + tblRegion.RegionName AS Region  FROM dbo.tblCustMaster
                                LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
                                LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
                                LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
                                LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblArea.RegionId
                                LEFT JOIN dbo.tblCustCategory ON tblCustCategory.CategoryId = tblCustMaster.CategoryId
                                WHERE tblCustMaster.IsActive = 1" + parameter;

            return aInternalDal.DataContainerDataTable(queryStr);
        }
    }
}
