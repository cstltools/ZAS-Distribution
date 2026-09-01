using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class AreaDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public bool SaveAreaInfo(AreaInfo areaInfo)
        {
            string insertQuery = @"insert into tblArea (AreaCode,AreaName,AreaShortName,RegionId,IsActive,Description,Remarks,EntryBy,EntryDate) 
            values (" + "'" + areaInfo.AreaCode + "','" + areaInfo.AreaName + "','" + areaInfo.AreaShortName + "','" + areaInfo.RegionId + "','" + areaInfo.IsActive + "','" + areaInfo.Description + "','" + areaInfo.Remarks + "','" + areaInfo.EntryBy + "','" + areaInfo.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool HasAreaName(AreaInfo areaInfo)
        {
            string query = "select * from tblArea where AreaName = '" + areaInfo.AreaName + "'";
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
            const string query = @"SELECT * FROM tblArea AS AR 
                                   INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public AreaInfo AreaEditLoad(string areaId)
        {
            string query = @"SELECT * FROM tblArea AS AR 
                            WHERE AR.AreaId = '" + areaId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");

            AreaInfo areaInfo = new AreaInfo();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    areaInfo.AreaId = Int32.Parse(dataReader["AreaId"].ToString());
                    areaInfo.AreaName = dataReader["AreaName"].ToString();
                    areaInfo.AreaCode = dataReader["AreaCode"].ToString();
                    areaInfo.AreaShortName = dataReader["AreaShortName"].ToString();
                    areaInfo.RegionId = Convert.ToInt32(dataReader["RegionId"].ToString());
                    areaInfo.Remarks = dataReader["Remarks"].ToString();
                    areaInfo.Description = dataReader["Description"].ToString();
                    areaInfo.IsActive = Convert.ToBoolean(dataReader["IsActive"].ToString());
                }
            }
            return areaInfo;
        }

        public void LoadDDLDal_Zas(DropDownList ddl)
        {
            ClsCommonInternalDAL.LoadDropDownValue2(ddl, "ProductName", "ProductId", "sp_GET_AllItemDropDown_Zas", "SSIDB");
        }

        public void LoadUnitByPermissionByModule_Zas(DropDownList ddl, Int32 userId, Int32 moduleId)
        {
            List<SqlParameter> gSqlParameterList = new List<SqlParameter>();

            gSqlParameterList.Add(new SqlParameter("@UserId", userId));
            gSqlParameterList.Add(new SqlParameter("@ModuleId", moduleId));

            ClsCommonInternalDAL.LoadDropDownListData(ddl, "UnitName", "UnitId", "sp_Common_GetUnitByPermissionByModule", gSqlParameterList);
        }

        public DataTable GetStockValuationReport_Zas(string param, string forOpeningParam, string forClosingParam)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            //string formattedParam = string.IsNullOrWhiteSpace(param) ? "" : " AND 1=1";

            aSqlParameterlist.Add(new SqlParameter("@Param", param));
            aSqlParameterlist.Add(new SqlParameter("@OpenParam", forOpeningParam));
            aSqlParameterlist.Add(new SqlParameter("@CloseParam", forClosingParam));

            return aCommonInternalDal.GetDataTableAction("sp_INV_WeightedAverage_Report", aSqlParameterlist);
        }
        public bool UpdateAreaInfo(AreaInfo areaInfo)
        {
            string query = @"UPDATE tblArea SET AreaName='" + areaInfo.AreaName + "',AreaCode='" + areaInfo.AreaCode + "',RegionId='" + areaInfo.RegionId + "',Description='" + areaInfo.Description + "',AreaShortName='" + areaInfo.AreaShortName + "',Remarks='" + areaInfo.Remarks + "',IsActive='" + areaInfo.IsActive + "',UpdateBy='" + areaInfo.UpdateBy + "',UpdateDate='" + areaInfo.UpdateDate + "' WHERE AreaId=" + areaInfo.AreaId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public void LoadMiaName(DropDownList ddl,string distId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblMIAInfo WHERE DistrictId='"+distId+"'";
            aInternalDal.LoadDropDownValue(ddl, "MiaName", "MiaId", queryStr,"SSIDB");
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

        public void LoadUnitWiseLocation_Zas(DropDownList ddl, string businessUnitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            List<SqlParameter> gSqlParameterList = new List<SqlParameter>();

            gSqlParameterList.Add(new SqlParameter("@CompanyId", businessUnitId));
            ClsCommonInternalDAL.LoadDropDownListData(ddl, "UnitName", "UnitId", "sp_Common_GetUnitWiseLocation_Zas", gSqlParameterList);
        }

        public DataTable CodeExistOrNot(string code)
        {
            string queryStr = "select * from tblArea where AreaCode='" + code + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable NameExistOrNot(string name)
        {
            string queryStr = "select * from tblArea where AreaName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable ShortNameExistOrNot(string name)
        {
            string queryStr = "select * from tblArea where AreaShortName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }
    }
}
