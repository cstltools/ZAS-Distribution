using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class WarehouseStockInApprovalBll
    {
        WarehouseStockInApprovalDal approvalDal = new WarehouseStockInApprovalDal();


        //public void LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)
        //{
        //    approvalDal.LoadApprovalControlDAL(rdl, pageName, userName);
        //}
        //public string LoadForApprovalConditionBLL(string pageName, string userName)
        //{
        //    return approvalDal.LoadForApprovalConditionDAL(pageName, userName);
        //}


        public DataTable LoadWarehouseStockInInformation()
        {
            return approvalDal.GetWarehouseStockInInformation();
        }

        public bool ApproveWarehouseStockInMaster(WarehouseStockInMasterDao aMasterDao)
        {
            return approvalDal.ApproveWarehouseStockInMaster(aMasterDao);
        }

        public string ApprovalUpdateBLL(WarehouseStockInMasterDao aMasterDao)
        {
            approvalDal.ApprovalUpdateDal(aMasterDao);
            return "Weldone! Stock In approved successfully!!!";
        }

        public bool InsertDataIntoCenterlWarehouse(List<WHStockInToCenteralStore> aCenteralStoreList)
        {
            bool status = false;

            foreach (WHStockInToCenteralStore aCenteralStore in aCenteralStoreList)
            {
                // Check Already Exist or Not 

                DataTable aTable = new DataTable();
                aTable = approvalDal.CheckDetailsExistOrNot(aCenteralStore);

                if (aTable.Rows.Count == 0)
                {
                    status = approvalDal.SaveWHStockIntoCenterlWarehouse(aCenteralStore);
                }
  
            }

            return status;
        }

        public DataTable LoadWarehouseStockInInfo(Int32 stockInMasterId)
        {
            return approvalDal.GetLoadWarehouseStockInInfo(stockInMasterId);
        }

        public bool PostJournal(int stockInMasterId)
        {
            return approvalDal.PostJournal(stockInMasterId);
        }
    }
}
