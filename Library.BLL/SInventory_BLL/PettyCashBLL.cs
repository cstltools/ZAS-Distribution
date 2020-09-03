using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.PettyCash_DaL;
using Library.DAO.PettyCashDao;

namespace Library.BLL.PettyCash_BLL
{
  public class PettyCashBLL
    {
      PettyCashDaL aCashDaL = new PettyCashDaL();



      public void LoadCompany(DropDownList ddl)
      {
          aCashDaL.LoadCompanydropdown(ddl);
      }

      public void LoadCashAcc(DropDownList ddl, string id)
      {
          aCashDaL.LoadCashAccDal(ddl, id);
      }

      //delete Action master

      public bool MAsterDeleteDataBll(string Id)
      {
          return aCashDaL.MasterDeleteDataDal(Id);
      }

      //delete Action Details
      public bool DetailsDeleteDataBll(string Id)
      {
          return aCashDaL.DetailsDeleteDataDal(Id);
      }



      public bool UpdateDataBll(PettyCashMasterDao aCashMasterDao)
      {
          return aCashDaL.UpdateMasterDataDal(aCashMasterDao);
         // return aCashDaL.SavePrettyCashMasterid(aCashMasterDao);
      }

 

      public Int32 SaveDataBll(PettyCashMasterDao aCashMasterDao)
      {
          return aCashDaL.SavePettyCashMaster(aCashMasterDao);
      }


      public Int32 SaveDataForDetails(List<PettyCashDetailsDao> aDirectStockInDaoList)
      {
          Int32 ID = 0;
          foreach (PettyCashDetailsDao aDirectStockInDao in aDirectStockInDaoList)
          {
              ID = aCashDaL.SaveDataForDetail(aDirectStockInDao);
          }
          return ID;
      }




      public bool UpdateDetailsDataBll(List<PettyCashDetailsDao> aDirectStockInDaoList)
      {
          bool Id = false;
          foreach (PettyCashDetailsDao aDirectStockInDao in aDirectStockInDaoList)
          {
            Id=  aCashDaL.UpdateDetailsDataDal(aDirectStockInDao);
          }

          return Id;
        
      }

      



      public DataTable GetAllAvailableOrder()
      {
          return aCashDaL.GetAllAvailablePrettyCash();
      }

      //product discount check
      public DataTable CheckProductDisCount()
      {
          return aCashDaL.CheckProductDisCountdal();
      }

      //get master
      public DataTable GetDataMasterByIdBll(int id)
      {
          return aCashDaL.GetMasterInfoByIdDal(id);
      }

   
      //get details
      public DataTable GetDetailsInfoByIdBll(int id)
      {
          return aCashDaL.GetDetailsInfoByIdDal(id);
      }


      public bool ApprovalUpdateDAL( PettyCashMasterDao aCashMasterDao)
      {
          return aCashDaL.ApprovalUpdateDAL(aCashMasterDao);

      }


      public void LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)
      {
          aCashDaL.LoadApprovalControlDAL(rdl, pageName, userName);
      }



      public DataTable GetSpecialApproveOrder(string actionstatus)
      {
          return aCashDaL.GetSpecialApproveOrder(actionstatus);
      }

      public string LoadForApprovalConditionBLL(string pageName, string userName)
      {
          return aCashDaL.LoadForApprovalConditionDAL(pageName, userName);
      }


      public DataTable LoadPettyCash(string parameter)
      {
          return aCashDaL.GetAllEmployeeInformation(parameter);
      }



      public DataTable LoadCreditCustomerreport(string parameter)
      {
          return aCashDaL.LoadCreditCustomerReport(parameter);
      }

      //Load Product distount report All
      public DataTable LoadProductDistountReport( )
      {
          return aCashDaL.LoadProductDiscountReportAlldal();
      }


      //Load Product discount report
      public DataTable LoadProductDistountReport(string parameter)
      {
          return aCashDaL.LoadProductDiscountReportdal(parameter);
      }



      public DataTable LoadCreditCustomerreportAll()
      {
          return aCashDaL.LoadCreditCustomerReportAll();
      }

    }
}
