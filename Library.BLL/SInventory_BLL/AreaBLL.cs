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
    public class AreaBLL
    {
        AreaDAL aAreaDAL = new AreaDAL();
        public string SaveArea(AreaInfo areaInfo)
        {
            try
            {
                if (!aAreaDAL.HasAreaName(areaInfo))
                {
                    ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                    //areaInfo.AreaId = aClsPrimaryKeyFind.PrimaryKeyMax("AreaId", "tblArea");
                    //areaInfo.AreaCode = AreaCodeGenerator(areaInfo.AreaId);

                    aAreaDAL.SaveAreaInfo(areaInfo);
                    return "Data Save Successfully Area Code  is :  " + areaInfo.AreaCode + " And Area Name is :" + areaInfo.AreaName;
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


        public bool UpdateDataForAreaInfo(AreaInfo aAreaInfo)
        {
            return aAreaDAL.UpdateAreaInfo(aAreaInfo);
        }

        public DataTable LoadAreaInfo()
        {
            return aAreaDAL.LoadAreaView();
        }
        public void LoadMiaName(DropDownList ddl,string distId)
        {
            aAreaDAL.LoadMiaName(ddl,distId);
        }
        public AreaInfo AreaInfoEditLoad(string AreaInfoId)
        {
            return aAreaDAL.AreaEditLoad(AreaInfoId);
        }
        public void LoadDistrictName(DropDownList ddl,string regionId)
        {
            aAreaDAL.LoadDistrictName(ddl,regionId);
        }
        public void LoadDDLBll_Zas(DropDownList ddl)
        {
            aAreaDAL.LoadDDLDal_Zas(ddl);
        }

        public void LoadRegionDropdownList(DropDownList ddl)
        {
            aAreaDAL.LoadRegionName(ddl);
        }

        public DataTable CodeExistOrNot(string code)
        {
            return aAreaDAL.CodeExistOrNot(code);
        }

        public DataTable NameExistOrNot(string name)
        {
            return aAreaDAL.NameExistOrNot(name);
        }

        public DataTable ShortNameExistOrNot(string name)
        {
            return aAreaDAL.ShortNameExistOrNot(name);
        }
    }
}
