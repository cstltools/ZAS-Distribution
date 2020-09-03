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
     public class StockConditionFreezeBLL
    {
         StockConditionFreezeDAL aConditionFreezeDal = new StockConditionFreezeDAL();


         public void LoadCompanyDropdownList(DropDownList ddl)
         {
             aConditionFreezeDal.GetCompanyDropdownList(ddl);
         }

         public void LoadWhInformation(DropDownList ddl, string companyId)
         {
             aConditionFreezeDal.GetWhInformation(ddl, companyId);
         }

         public void LoadWhInformationByUser(DropDownList ddl, string companyId,string userId)
         {
             aConditionFreezeDal.GetWhInformationByUser(ddl, companyId, userId);
         }

         public DataTable LoadStockDCData(int id)
         {
             return aConditionFreezeDal.LoadStockDCData(id);
         }
         public DataTable LoadStockStockQtyDCData(int id)
         {
             return aConditionFreezeDal.LoadStockStockQtyDCData(id);
         }
         public void LoadStockConditionBll(DropDownList aDownList,string userid)
         {
             aConditionFreezeDal.LoadStockConditionBll(aDownList, userid);
         }

         public DataTable LoadWHData(int id)
         {
             return aConditionFreezeDal.LoadWHData(id);
         }
         public DataTable LoadWHData()
         {
             return aConditionFreezeDal.LoadWHData();
         }
         public int SaveforWH(StockConditionFreezeDAO aStockConditionFreezeDAO)
         {
             try
             {
                 //if (!aCustomerMasterDAL.HasCustomerMastername(CustomerMaster))
                 {
                     ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                     //int MigoMasterID = 0;
                     aStockConditionFreezeDAO.StockConditionFreezeID = aClsPrimaryKeyFind.PrimaryKeyMax("StockConditionFreezeID", "tblStockConditionFreeze");
                     aConditionFreezeDal.SaveforWH(aStockConditionFreezeDAO);
                     return aStockConditionFreezeDAO.StockConditionFreezeID;
                 }
                 //else
                 //{
                 //    return "Company Name already exist";
                 //}
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             { }
         }
         public int SaveforDC(StockConditionFreezeDAO aStockConditionFreezeDAO)
         {
             try
             {
                 //if (!aCustomerMasterDAL.HasCustomerMastername(CustomerMaster))
                 {
                     ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                     //int MigoMasterID = 0;
                     aStockConditionFreezeDAO.StockConditionFreezeID = aClsPrimaryKeyFind.PrimaryKeyMax("StockConditionFreezeID", "tblStockConditionFreeze");
                     aConditionFreezeDal.SaveforDC(aStockConditionFreezeDAO);
                     return aStockConditionFreezeDAO.StockConditionFreezeID;
                 }
                 //else
                 //{
                 //    return "Company Name already exist";
                 //}
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             { }
         }
         public bool UpdateCentralStore(decimal StockQty, int ReceiveId)
         {
             return aConditionFreezeDal.UpdateCentralStore(StockQty, ReceiveId);
         }
         public bool UpdateDCStore(decimal StockQty, int DCStoreId)
         {
             return aConditionFreezeDal.UpdateDCStore(StockQty, DCStoreId);
         }
         //SC Picking Generate Page
         public DataTable LoadInvoice(int Dcid,int ManufId,int marketid,DateTime invDate)
         {
             return aConditionFreezeDal.LoadInvoice(Dcid, ManufId, marketid, invDate);
         }
         public int SaveDCStoreFreeze(DCStoreFreezeDAO aDcStoreFreezeDao)
         {
             try
             {
                 {
                     ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                     aDcStoreFreezeDao.DCStoreFreezeId = aClsPrimaryKeyFind.PrimaryKeyMax("DCStoreFreezeId", "tblDCStoreFreeze");
                     aConditionFreezeDal.SaveDCStoreFreeze(aDcStoreFreezeDao);
                     return aDcStoreFreezeDao.StockConditionFreezeID;
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             { }
         }
         public int SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStoreFreezeDao)
         {
             try
             {
                 {
                     ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                     aDcStoreFreezeDao.DCStoreFreezeId = aClsPrimaryKeyFind.PrimaryKeyMax("DCStoreFreezeId", "tblDCStoreFreeze");
                     aConditionFreezeDal.SaveDCStoreFreeze2(aDcStoreFreezeDao);
                     return aDcStoreFreezeDao.StockConditionFreezeID;
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             { }
         }

         public void PostJournal(int stockConditionFreezeId, decimal quantity)
         {
             aConditionFreezeDal.PostFreezeStockJournal(stockConditionFreezeId,quantity);
         }
    }

}
