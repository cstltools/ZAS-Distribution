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
    public class CustomerFixedDiscountDal
    {
        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public void LoadGroupInfo(DropDownList groupDropDownList)
        {
            string query = @"SELECT GroupId, GroupName FROM tblGroupInformation";
            aCommonInternalDAL.LoadDropDownValue(groupDropDownList, "GroupName", "GroupId", query, "SSIDB");
        }      

        public void LoadCompanyInfo(DropDownList ddl, string groupId)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo WHERE GroupId = " + groupId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster AS CSTMR
			 LEFT JOIN dbo.tblMarket AS MKT ON MKT.MarketId = CSTMR.MarketId
			 LEFT JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MKT.TerritoryId
			 LEFT JOIN dbo.tblMIOInfo AS MIO ON MIO.TerritoryId = TTR.TerritoryId
			 WHERE CSTMR.MarketId IS NOT NULL AND MIO.MIOId IS NOT NULL AND CSTMR.CustomerCode = '" + customerCode + "'";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public Int32 SaveDiscountInfo(CustFixedDiscountDao aDao)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@CompanyId", aDao.CompanyId));
            aSqlParameters.Add(new SqlParameter("@CustomerMasterId", aDao.CustomerMasterId));
            aSqlParameters.Add(new SqlParameter("@DisParcentage", aDao.DisParcentage));
            aSqlParameters.Add(new SqlParameter("@IsActive", aDao.IsActive));
            aSqlParameters.Add(new SqlParameter("@ActiveDate", aDao.ActiveDate));
            aSqlParameters.Add(new SqlParameter("@EntryBy", aDao.EntryBy));
            aSqlParameters.Add(new SqlParameter("@EntryDate", aDao.EntryDate));

            string query = @"INSERT INTO dbo.tblCustomerFixedDiscount (CompanyId,CustomerMasterId,DisParcentage,IsActive,ActiveDate,EntryBy,EntryDate)
                             VALUES (@CompanyId,@CustomerMasterId,@DisParcentage,@IsActive,@ActiveDate,@EntryBy,@EntryDate)";
            return aCommonInternalDAL.SaveDataByInsertCommandById(query,aSqlParameters,"SSIDB");
        }
    }
}
