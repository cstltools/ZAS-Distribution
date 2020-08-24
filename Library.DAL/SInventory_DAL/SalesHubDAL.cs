using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Library.DAL.InternalCls;
using System.Web.UI.WebControls;

namespace Library.DAL.SInventory_DAL
{
   public class SalesHubDAL
    {
       ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public DataTable GetRegionHub(string parameter)
        {
            string queryStr = @"SELECT * from tblRegion" + parameter;
            return aCommonInternalDAL.DataContainerDataTable(queryStr, "SSIDB");
        }

        public void GetRegionDropdownList(DropDownList regionDropDownList)
        {
             string queryStr = @"SELECT RegionId, RegionName FROM tblRegion";
             aCommonInternalDAL.LoadDropDownValue(regionDropDownList, "RegionName", "RegionId", queryStr, "SSIDB");
        }
    
        public DataTable GetAreaHub(string parameter)
        {
            string queryStr = @"SELECT DISTINCT DCA.DCId,REG.RegionCode, REG.RegionName, ARE.AreaCode, ARE.AreaName, ARE.AreaShortName, ARE.IsActive, ARE.Description, ARE.Remarks
                                   FROM tblArea AS ARE 
                                   INNER JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARE.AreaId
                                   INNER JOIN  tblRegion AS REG  ON ARE.RegionId = REG.RegionId " + parameter;
        
            return aCommonInternalDAL.DataContainerDataTable(queryStr, "SSIDB");
        }


        public void GetAreaDropdownList(DropDownList areaDropDownList, string regionDropDownList)
        {
            string queryStr = @"SELECT AreaId, AreaName FROM tblArea WHERE  RegionId = '" + regionDropDownList + "'";
            aCommonInternalDAL.LoadDropDownValue(areaDropDownList, "AreaName", "AreaId", queryStr, "SSIDB");
        }

        public void GetTerritoryList(DropDownList territoryDropDownList, string areaDropDownList)
        {
            string queryStr = @"SELECT TerritoryId, TerritoryName FROM tblTerritory WHERE  AreaId = '" + areaDropDownList + "'";
            aCommonInternalDAL.LoadDropDownValue(territoryDropDownList, "TerritoryName", "TerritoryId", queryStr, "SSIDB");
        }

        public DataTable GetTerritoryHub(string parameter)
        {
            string queryStr = @"SELECT DISTINCT REG.RegionCode, REG.RegionName, ARE.AreaCode, ARE.AreaName, TER.TerritoryCode, TER.TerritoryName,TER.TerShortName,TER.IsActive,TER.Description, TER.Remarks
                                FROM tblTerritory AS TER
								INNER JOIN tblArea AS ARE ON TER.AreaId=ARE.AreaId
                                INNER JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARE.AreaId
                                INNER JOIN  tblRegion AS REG  ON ARE.RegionId=REG.RegionId" + parameter;
          return  aCommonInternalDAL.DataContainerDataTable(queryStr,"SSIDB");
        }

        public DataTable GetMarketHub(string parameter)
        {
            string queryStr = @"SELECT DISTINCT REG.RegionCode, REG.RegionName, ARE.AreaCode, ARE.AreaName, TER.TerritoryCode, TER.TerritoryName, MAR.MarketCode AS MarketId, MAR.MarketName,MAR.MktShortName,MAR.IsActive, MAR.Description,MAR.Remarks
                                FROM tblMarket AS MAR
								INNER JOIN tblTerritory AS TER ON MAR.TerritoryId=TER.TerritoryId
								INNER JOIN tblArea AS ARE ON TER.AreaId=ARE.AreaId
                                INNER JOIN dbo.tblDcWiseAreaInfo AS DCA ON DCA.AreaId = ARE.AreaId
                                INNER JOIN  tblRegion AS REG  ON ARE.RegionId=REG.RegionId" + parameter;
            return aCommonInternalDAL.DataContainerDataTable(queryStr, "SSIDB");
        }
    }
}
