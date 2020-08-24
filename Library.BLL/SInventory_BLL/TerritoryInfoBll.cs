using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class TerritoryInfoBll
    {

        TerritoryInfoDal aAreaDAL = new TerritoryInfoDal();

        public string SaveArea(TerritoryInfoDao territoryInfo)
        {
            try
            {
                if (!aAreaDAL.HasAreaName(territoryInfo))
                {
                    ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                    //areaInfo.AreaId = aClsPrimaryKeyFind.PrimaryKeyMax("AreaId", "tblArea");
                    //areaInfo.AreaCode = AreaCodeGenerator(areaInfo.AreaId);

                    aAreaDAL.SaveAreaInfo(territoryInfo);
                    return "Data Save Successfully Area Code  is :  " + territoryInfo.TerritoryCode + " And Area Name is :" + territoryInfo.TerritoryName;
                }
                else
                {
                    return "Area Name already exist";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public string AreaCodeGenerator(int id)
        {
            string code = string.Empty;
            string Id = id.ToString();
            if (Id.Length == 1)
            {
                Id = "00" + Id;
            }
            if (Id.Length == 2)
            {
                Id = "0" + Id;
            }
            code = "AREA-" + Id;
            return code;
        }


        public bool UpdateDataForAreaInfo(TerritoryInfoDao territoryInfo)
        {
            return aAreaDAL.UpdateAreaInfo(territoryInfo);
        }

        public DataTable LoadAreaInfo()
        {
            return aAreaDAL.LoadAreaView();
        }
        public void LoadMiaName(DropDownList ddl,string distId)
        {
            aAreaDAL.LoadMiaName(ddl,distId);
        }
        public TerritoryInfoDao AreaInfoEditLoad(string territoryId)
        {
            return aAreaDAL.AreaEditLoad(territoryId);
        }
        public void LoadDistrictName(DropDownList ddl,string regionId)
        {
            aAreaDAL.LoadDistrictName(ddl,regionId);
        }


        public void LoadRegionDropdownList(DropDownList ddl)
        {
            aAreaDAL.LoadRegionName(ddl);
        }

        public void LoadAreaOnDropdownList(DropDownList ddl, string regionId)
        {
            aAreaDAL.GetAreaOnDropdownList(ddl, regionId);
        }

        public DataTable CodeExistOrNot(string code)
        {
            return aAreaDAL.CodeExistOrNot(code);
        }

        public DataTable NameExistOrNot(string name)
        {
            return aAreaDAL.NameExistOrNot(name);
        }

        public DataTable ShortNameExistOrNot(string shortNAme)
        {
            return aAreaDAL.ShortNameExistOrNot(shortNAme);
        }
    }
}

