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
    public class CampaignDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetDdlCompany(DropDownList ddl)
        {
            const string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }
        public DataTable GetMasterCode()
        {
            const string query = @"SELECT 'CMP-'+CONVERT(NVARCHAR(MAX),COUNT(ISNULL(CampgainMasterId,0))+1000+1) FROM dbo.tblBonusCampaignMaster";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadAllData(string parm)
        {
            string query = @"SELECT tblBonusCampaignMaster.CompanyId, * FROM dbo.tblBonusCampaignMaster
            LEFT JOIN dbo.tblCompanyInfo ON dbo.tblCompanyInfo.CompanyId=dbo.tblBonusCampaignMaster.CompanyId    WHERE CampgainMasterId IS NOT NULL " + parm;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadingleData(string id)
        {
            string query = @"SELECT REPLACE(convert(varchar, FromDate, 106),' ','-')AS  FromDate,REPLACE(convert(varchar, ToDate, 106),' ','-')ToDate,*,B.ProductName AS BonusProductName,B.ProductId AS BonusProductId  FROM dbo.tblBonusCampaignMaster
LEFT JOIN dbo.tblBonusCampgainDetail ON tblBonusCampgainDetail.CampgainMasterId = tblBonusCampaignMaster.CampgainMasterId
LEFT JOIN dbo.tblProduct P ON P.ProductCode=dbo.tblBonusCampgainDetail.PointProductCode
LEFT JOIN dbo.tblProduct B ON B.ProductCode=dbo.tblBonusCampgainDetail.BonusProductCode
            WHERE tblBonusCampaignMaster.CampgainMasterId='" +id+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public int SaveCampaignMaster(CampaignDAO aCampaignDao)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CompanyId", aCampaignDao.CompanyId));
            aSqlParameterList.Add(new SqlParameter("@CampaignCode", aCampaignDao.CampaignCode));
            aSqlParameterList.Add(new SqlParameter("@EntryBy", aCampaignDao.EntryBy));
            aSqlParameterList.Add(new SqlParameter("@EntryDate", aCampaignDao.EntryDate));
            string insertQuery = @"INSERT dbo.tblBonusCampaignMaster
                                    (CompanyId,
                                        CampaignCode,
                                        EntryBy,
                                        EntryDate
                                    )
                                    VALUES
                                    (  @CompanyId,
                                        @CampaignCode,
                                        @EntryBy,
                                        @EntryDate)";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery, aSqlParameterList,"SSIDB");
        }
        public bool UpdateCampaignMaster(CampaignDAO aCampaignDao)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CampgainMasterId", aCampaignDao.CampgainMasterId));
            aSqlParameterList.Add(new SqlParameter("@CompanyId", aCampaignDao.CompanyId));
            
            aSqlParameterList.Add(new SqlParameter("@EntryBy", aCampaignDao.EntryBy));
            aSqlParameterList.Add(new SqlParameter("@EntryDate", aCampaignDao.EntryDate));
            string insertQuery = @"UPDATE dbo.tblBonusCampaignMaster SET
                                    CompanyId=@CompanyId,
                                        
                                        EntryBy=@EntryBy,
                                        EntryDate=@EntryDate where CampgainMasterId=@CampgainMasterId
                                    
                                    ";
            return aCommonInternalDal.UpdateDataByUpdateCommand(insertQuery, aSqlParameterList, "SSIDB");
        }
        public bool DeleteCampaignDetail(string id)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CampgainMasterId", id));

            string insertQuery = @"DELETE FROM dbo.tblBonusCampgainDetail WHERE CampgainMasterId=@CampgainMasterId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(insertQuery, aSqlParameterList, "SSIDB");
        }
        public int SaveCampaignDetail(CampaignDetailDAO aCampaignDetailDao)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CampgainMasterId", aCampaignDetailDao.CampgainMasterId));
            aSqlParameterList.Add(new SqlParameter("@PointProductCode", aCampaignDetailDao.PointProductCode));
            aSqlParameterList.Add(new SqlParameter("@Quantity", aCampaignDetailDao.Quantity));
            aSqlParameterList.Add(new SqlParameter("@BonusProductCode", aCampaignDetailDao.BonusProductCode));
            aSqlParameterList.Add(new SqlParameter("@BonusQuantity", aCampaignDetailDao.BonusQuantity));
            aSqlParameterList.Add(new SqlParameter("@IsActive", aCampaignDetailDao.IsActive));
            aSqlParameterList.Add(new SqlParameter("@FromDate", aCampaignDetailDao.FromDate));
            aSqlParameterList.Add(new SqlParameter("@FromTime", aCampaignDetailDao.FromTime));
            aSqlParameterList.Add(new SqlParameter("@ToDate", aCampaignDetailDao.ToDate));
            aSqlParameterList.Add(new SqlParameter("@ToTime", aCampaignDetailDao.ToTime));
            aSqlParameterList.Add(new SqlParameter("@IsSame", aCampaignDetailDao.IsSame));
            
            string insertQuery = @"INSERT INTO dbo.tblBonusCampgainDetail
                                    (
                                        CampgainMasterId,
                                        PointProductCode,
                                        Quantity,
                                        BonusProductCode,
                                        BonusQuantity,
                                        IsActive,
                                        FromDate,
                                        FromTime,
                                        ToDate,
                                        ToTime,
                                        IsSame
                                    )
                                    VALUES
                                    (   @CampgainMasterId,
                                        @PointProductCode,
                                        @Quantity,
                                        @BonusProductCode,
                                        @BonusQuantity,
                                        @IsActive,
                                        @FromDate,
                                        @FromTime,
                                        @ToDate,
                                        @ToTime,
                                        @IsSame)";
            return aCommonInternalDal.SaveDataByInsertCommandWithIdentity(insertQuery,aSqlParameterList, "SSIDB");
        }
    }
}
