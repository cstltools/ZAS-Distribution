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
    public class DistributionCenterInfoDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public bool SaveDataForSalesCenter(CompanyUnit aCompanyUnit)
        {
            string insertQuery = @"insert into tblCompanyUnit (CompanyId,DcTypeId,ComUnitCode,ComUnitName,ShortName,Address,PhoneNo,MobileNo,FaxNo,CashGlId,EntryBy,EntryDate) 
            values ('" + aCompanyUnit.CompanyId + "','" + aCompanyUnit.DcTypeId + "','" + aCompanyUnit.ComUnitCode + "','" + aCompanyUnit.ComUnitName + "','" + aCompanyUnit.ShortName + "','" + aCompanyUnit.Address + "'," +
                                 "'" + aCompanyUnit.PhoneNo + "','" + aCompanyUnit.MobileNo + "','" + aCompanyUnit.FaxNo + "','" + aCompanyUnit.CashGlId + "','" + aCompanyUnit.EntryBy + "','" + aCompanyUnit.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasComUnitName(CompanyUnit aCompanyUnit)
        {
            string query = "select * from tblCompanyUnit where ComUnitName = '" + aCompanyUnit.ComUnitName + "'";
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

        public DataTable LoadSalesCenter()
        {
            const string query = @"SELECT * FROM tblCompanyUnit AS CU 
                             INNER JOIN tblCompanyInfo AS CI ON CU.CompanyId = CI.CompanyId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public CompanyUnit SalesCenterEditLoad(string ComUnitId)
        {
            string query = @"SELECT CASE WHEN CashGlId IS NULL THEN 0 ELSE CashGlId END CashGlId ,* FROM tblCompanyUnit AS CU 
                            INNER JOIN tblCompanyInfo AS CI ON CU.CompanyId = CI.CompanyId
                            WHERE CU.ComUnitId = '" + ComUnitId + "'";

            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");

            var aCompanyUnit = new CompanyUnit();

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCompanyUnit.ComUnitId = Int32.Parse(dataReader["ComUnitId"].ToString());
                    aCompanyUnit.CashGlId = Int32.Parse(dataReader["CashGlId"].ToString());
                    aCompanyUnit.ComUnitCode = dataReader["ComUnitCode"].ToString();
                    aCompanyUnit.ShortName = dataReader["ShortName"].ToString();
                    aCompanyUnit.ComUnitName = dataReader["ComUnitName"].ToString();
                    aCompanyUnit.PhoneNo = dataReader["PhoneNo"].ToString();
                    aCompanyUnit.Address = dataReader["Address"].ToString();
                    aCompanyUnit.CompanyId = Convert.ToInt32(dataReader["CompanyId"].ToString());
                    aCompanyUnit.DcTypeId = Convert.ToInt32(dataReader["DcTypeId"].ToString());
                    aCompanyUnit.MobileNo = dataReader["MobileNo"].ToString();
                    aCompanyUnit.FaxNo = dataReader["FaxNo"].ToString();

                }
            }
            return aCompanyUnit;
        }

        public bool UpdateSalesCenter(CompanyUnit aCompanyUnit)
        {
            string query = @"UPDATE tblCompanyUnit SET ComUnitName='" + aCompanyUnit.ComUnitName + "',CashGlId='" + aCompanyUnit.CashGlId + "',CompanyId='" + aCompanyUnit.CompanyId + "',DcTypeId=" + aCompanyUnit.DcTypeId + ",ShortName='" + aCompanyUnit.ShortName + "',Address='" + aCompanyUnit.Address + "',MobileNo='" + aCompanyUnit.MobileNo + "',PhoneNo='" + aCompanyUnit.PhoneNo + "',FaxNo='" + aCompanyUnit.FaxNo + "',UpdateBy='" + aCompanyUnit.UpdateBy + "',UpdateDate='" + aCompanyUnit.UpdateDate + "' WHERE ComUnitId=" + aCompanyUnit.ComUnitId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public void LoadCompanyName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void LoadRegionName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            const string queryStr = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }

        public DataTable DCInfoExistOrNot(string dcName)
        {
            string query = @"SELECT * FROM tblCompanyUnit AS CU WHERE CU.ComUnitName = '" + dcName + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable DCShortNameExistOrNot(string shortName)
        {
            string query = @"SELECT * FROM tblCompanyUnit AS CU WHERE CU.ShortName = '" + shortName + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool CheckCodeExistOrNot(string comUnitCode)
        {
            bool status = false;
            var aTable = new DataTable();

            string queryStr = @"SELECT * FROM tblCompanyUnit AS CU WHERE CU.ComUnitCode = '" + comUnitCode + "'";
            aTable = aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");

            if (aTable.Rows.Count > 0)
            {
                status = true;
            }

            return status;
        }

        public void LoadDcType(DropDownList ddl)
        {
            string queryStr = "SELECT DcTypeId,DcTypeName FROM tblDistributionType";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "DcTypeName", "DcTypeId", queryStr);
        }

        public void LoadCashAccount(DropDownList ddl, int companyId)
        {
            //            string query = @"SELECT CA.CashAccountId,AccountName FROM ZAS_ACCDB..tblCashAccount AS CA
            //                             LEFT JOIN ZAS_ACCDB..tblMultipleCompanySameCashAccount CASC ON CA.CashAccountId = CASC.CashAccountId
            //                             WHERE CA.CashAccountId IN (
            //                             SELECT CashAccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE CashAccountId IS NOT NULL )
            //                             AND CASC.CompanyId = " + companyId;

            string query = @"SELECT CA.CashAccountId,AccountName FROM CSTL_ACCDB..tblCashAccount AS CA
                             LEFT JOIN CSTL_ACCDB..tblMultipleCompanySameCashAccount CASC ON CA.CashAccountId = CASC.CashAccountId
                             WHERE CA.CashAccountId IN (
                             SELECT CashAccountId FROM CSTL_ACCDB..tblChartOfAccounts WHERE CashAccountId IS NOT NULL )
                             AND CASC.CompanyId = " + companyId;

            aCommonInternalDal.LoadDropDownValue(ddl, "AccountName", "CashAccountId", query, "SSIDB");
        }
    }
}
