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
    public class BonusCampaignDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        // Load Wings

        public void LoadCompanyInfo(DropDownList ddl)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        // Load Wings Wise Product

        public void LoadProductInfo(DropDownList ddl, int wingsId)
        {
            string query = @"SELECT PD.ProductId, PD.ProductCode + ':' + PD.ProductName AS ProductName  FROM tblProduct AS PD
                             LEFT JOIN tblUnitPrice AS UP ON PD.ProductId = UP.ProductId
                             WHERE UP.IsActive = 1 AND CompanyId = " + wingsId + " ORDER BY PD.ProductName";

            aCommonInternalDal.LoadDropDownValue(ddl, "ProductName", "ProductId", query, "SSIDB");
        }

        // Save


        public bool SaveBonusCampaign(BonusCampaignMasterDao aMasterDao, List<BonusCampaignDetailsDao> aDetailsDaos)
        {
            int masterId;

            // Check if updating existing campaign
            if (aMasterDao.CampgainMasterId > 0)
            {
                // Update master
                bool updateSuccess = UpdateBonusCampaignMaster(aMasterDao);
                if (!updateSuccess)
                    return false;

                masterId = aMasterDao.CampgainMasterId;
            }
            else
            {
                // Insert new master
                masterId = SaveBonusCampaignMaster(aMasterDao);
                if (masterId <= 0)
                    return false;
            }

            // Save all details
            foreach (var aDetailsDao in aDetailsDaos)
            {
                aDetailsDao.CampgainMasterId = masterId;

                int detailResult = SaveBonusCampaignDetail(aDetailsDao);
                if (detailResult <= 0)
                {
                    // Rollback only if this is a new insert
                    if (aMasterDao.CampgainMasterId <= 0)
                    {
                        DeleteBonusCampaignNew(masterId);
                    }
                    return false;
                }
            }

            return true;
        }

        private bool UpdateBonusCampaignMaster(BonusCampaignMasterDao aMasterDao)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@CompanyId", aMasterDao.CompanyId > 0 ? aMasterDao.CompanyId : 0));
            aSqlParameters.Add(new SqlParameter("@CampgainMasterId", aMasterDao.CampgainMasterId > 0 ? aMasterDao.CampgainMasterId : 0));
            aSqlParameters.Add(new SqlParameter("@BonusProductId", aMasterDao.BonusProductId > 0 ? aMasterDao.BonusProductId : 0));
            aSqlParameters.Add(new SqlParameter("@BonusProductQuantity", aMasterDao.BonusProductQuantity > 0 ? aMasterDao.BonusProductQuantity : 0));
            aSqlParameters.Add(new SqlParameter("@CampaignName", !string.IsNullOrEmpty(aMasterDao.CampaignName) ? (object)aMasterDao.CampaignName : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@CampaignDesc", !string.IsNullOrEmpty(aMasterDao.CampaignDesc) ? (object)aMasterDao.CampaignDesc : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@FromDate", aMasterDao.FromDate != DateTime.MinValue ? (object)aMasterDao.FromDate : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@ToDate", aMasterDao.Todate != DateTime.MinValue ? (object)aMasterDao.Todate : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@IsActive", aMasterDao.IsActive));
            aSqlParameters.Add(new SqlParameter("@EntryBy", aMasterDao.EntryBy));

            return aInternalDal.UpdateAction("sp_CPM_BonusCampaigntMasterUpdate", aSqlParameters);
        }


        private bool DeleteBonusCampaignNew(int campaignMasterId)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();
            aSqlParameters.Add(new SqlParameter("@CampaignMasterId", campaignMasterId));

            return aInternalDal.DeleteAction("sp_CPM_DeleteBonusCampaign", aSqlParameters);
        }


        public int SaveBonusCampaignMaster(BonusCampaignMasterDao aMasterDao)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@CompanyId", aMasterDao.CompanyId > 0 ? aMasterDao.CompanyId : 0));
            aSqlParameters.Add(new SqlParameter("@BonusProductId", aMasterDao.BonusProductId > 0 ? aMasterDao.BonusProductId : 0));
            aSqlParameters.Add(new SqlParameter("@BonusProductQuantity", aMasterDao.BonusProductQuantity > 0 ? aMasterDao.BonusProductQuantity : 0));
            aSqlParameters.Add(new SqlParameter("@CampaignName", !string.IsNullOrEmpty(aMasterDao.CampaignName) ? (object)aMasterDao.CampaignName : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@CampaignDesc", !string.IsNullOrEmpty(aMasterDao.CampaignDesc) ? (object)aMasterDao.CampaignDesc : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@FromDate", aMasterDao.FromDate != DateTime.MinValue ? (object)aMasterDao.FromDate : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@ToDate", aMasterDao.Todate != DateTime.MinValue ? (object)aMasterDao.Todate : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@IsActive", aMasterDao.IsActive));
            aSqlParameters.Add(new SqlParameter("@EntryBy", aMasterDao.EntryBy));

            return aInternalDal.SaveAction("sp_CPM_BonusCampaigntMasterInsert", aSqlParameters, "@CampgainMasterId");
        }


        public int SaveBonusCampaignDetail(BonusCampaignDetailsDao aDetailsDao)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CampgainMasterId", aDetailsDao.CampgainMasterId));
            aSqlParameterList.Add(new SqlParameter("@PointProductId", aDetailsDao.PointProductId));
            aSqlParameterList.Add(new SqlParameter("@Quantity", aDetailsDao.Quantity > 0 ? (object)aDetailsDao.Quantity : 0));

            return aInternalDal.SaveAction("sp_CPM_BonusCampaignDetailInsert", aSqlParameterList, "@CampaignDetailId");
        }

        public DataTable LoadBonusCampaignData(string param)
        {
            string query = @"SELECT CampgainMasterId,CampaignCode,CampaignName,CampaignDesc,CompanyName,
                             FORMAT(CPM.FromDate, 'dd-MMM-yyyy hh:mm:ss tt') FromDate, FORMAT(CPM.Todate, 'dd-MMM-yyyy hh:mm:ss tt') Todate,
                             CPM.EntryBy, FORMAT(CPM.EntryDate, 'dd-MMM-yyyy') EntryDate, PD.ProductCode + ' : ' + ProductName AS ProductName, BonusProductQuantity FROM Table_CPM_BonusCampaignMaster AS CPM
                             LEFT JOIN tblCompanyInfo AS CI ON CPM.CompanyId = CI.CompanyId
                             LEFT JOIN tblProduct AS PD ON CPM.BonusProductId = PD.ProductId
                             WHERE CampgainMasterId IS NOT NULL " + param + " ORDER BY CPM.EntryDate DESC";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

        }

        public DataTable LoadBonusCampaignDetails(string id)
        {
            string query = @"SELECT * FROM Table_CPM_BonusCampaignMaster AS CPM
							 LEFT JOIN Table_CPM_BonusCampaignDetail AS CPD ON CPM.CampgainMasterId = CPD.CampgainMasterId
                             LEFT JOIN tblProduct AS PD ON CPD.PointProductId = PD.ProductId
							 WHERE CPM.CampgainMasterId = " + id ;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
