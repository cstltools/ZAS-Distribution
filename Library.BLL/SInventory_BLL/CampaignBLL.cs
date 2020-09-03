using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class CampaignBLL
    {
        CampaignDAL aCampaignDal=new CampaignDAL();
        public int SaveCampaignMast(CampaignDAO aCampaignDao)
        {
            try
            {
                //if (!aAreaDAL.HasAreaName(areaInfo))
                {
                    ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                    //areaInfo.AreaId = aClsPrimaryKeyFind.PrimaryKeyMax("AreaId", "tblArea");
                    //areaInfo.AreaCode = AreaCodeGenerator(areaInfo.AreaId);
                    DataTable dtdata = aCampaignDal.GetMasterCode();
                    aCampaignDao.CampaignCode = dtdata.Rows[0][0].ToString();
                    return aCampaignDal.SaveCampaignMaster(aCampaignDao);
                    
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public int SaveCampaignDetail(CampaignDetailDAO aCampaignDao)
        {
            try
            {
                //if (!aAreaDAL.HasAreaName(areaInfo))
                {
                    ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                    //areaInfo.AreaId = aClsPrimaryKeyFind.PrimaryKeyMax("AreaId", "tblArea");
                    //areaInfo.AreaCode = AreaCodeGenerator(areaInfo.AreaId);

                    return aCampaignDal.SaveCampaignDetail(aCampaignDao);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

    }
}
