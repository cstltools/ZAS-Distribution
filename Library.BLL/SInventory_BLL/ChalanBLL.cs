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
    public class ChalanBLL
    {
        ChalanInfo aChalanInfo = new ChalanInfo();
        ChalanDAL aChalanDal = new ChalanDAL();
        ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

        public bool SaveDataForChalanInfo(ChalanInfo aChalanInfo, out int ChalanId)
        {
            DataTable aTable = new DataTable();
            aTable = aChalanDal.CheckEntryWithinlastMinute(aChalanInfo);

            if (aTable.Rows.Count != 1) // 1 means exist
            {
                ChalanId = aClsPrimaryKeyFind.PrimaryKeyMax("ChalanId", "tblChalanInfo");
                aChalanInfo.ChalanId = ChalanId;
                aChalanInfo.ChalanNo = ChalanNoGenerator(aChalanInfo.ChalanId);
                return aChalanDal.SaveDataForChalanInfo(aChalanInfo);           
            }
            else
            {
                ChalanId = 0;
                return false;
            }
     
        }

        public int DCStockIn2(DCStockNew aDcStockNew)
        {
            string msg = "Data Save Successfully!!";
            int DCStoreId = 0;

            // Check Duplicate 

            DataTable aTable = new DataTable();
            aTable = aChalanDal.CheckB2BExistOrNotInDcStore(aDcStockNew);

            if (aTable.Rows.Count == 0)
            {
                DCStoreId = aDcStockNew.DCStoreId = aClsPrimaryKeyFind.PrimaryKeyMax("DCStoreId", "tblDCStore", "SSIDB");
                aChalanDal.DCStockInDALMain(aDcStockNew);
                aChalanDal.ChalanUpdate(aDcStockNew.ChalanId.ToString());

                // Insert Into Weighted Average 

                if (DCStoreId > 0)
                {
                    // Update CentralStoreId in DCStore
                    RequisitionDAL aRequisitionDal = new RequisitionDAL();

                    aRequisitionDal.UpdateTempReceiveId();

                    // process Weighted Average (Disabled as per user request to not maintain DC Store WAC)

                    // aChalanDal.ProcessWeightedAverage(DCStoreId);
                }

            }

            
            return DCStoreId;
        }
        public string SaveDCStoreFreeze2(DCStoreFreezeDAO aDcStockNew)
        {
            string msg = "Data Save Successfully!!";

            ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
            aDcStockNew.DCStoreFreezeId = aClsPrimaryKeyFind.PrimaryKeyMax("DCStoreFreezeId","tblDCStoreFreeze");
            aChalanDal.SaveDCStoreFreeze2(aDcStockNew);


            // Insert Into Weighted Average (Disabled as per user request to not maintain DC Store WAC)

            // if (aDcStockNew.DCStoreFreezeId > 0)
            // {
            //     aChalanDal.ProcessWeightedAverageFreezeStock(aDcStockNew.DCStoreFreezeId);
            // }

            return msg;
        }
        public void UpdateDCStockQuantity(string stockId, string Quantity)
        {
            aChalanDal.UpdateDCStockQuantity(stockId,Quantity);
        }
        public DataTable LoadChalanById(string id)
        {
            return aChalanDal.LoadChalanById(id);
        }
        public DataTable ChalanLoadInReceive(string comunitId)
        {
            return aChalanDal.ChalanLoadInReceive(comunitId);
        }

        public DataTable ChalanLoadInReceiveReport(string comunitId, string param)
        {
            return aChalanDal.ChalanLoadInReceiveReport(comunitId, param);
        }

        public DataTable GetChalanReceieve(string id)
        {
            return aChalanDal.GetChalanReceieve(id);
        }

        public DataTable ChalanReport(string id)
        {
            return aChalanDal.ChalanReport(id);
        }
        public void LoadManufac(DropDownList aDropDownList)
        {
            aChalanDal.LoadManufac(aDropDownList);
        }

        public bool SaveDataForChalanDetail(List<ChalanDetail> aIChalanList)
        {
            bool status = false;

            foreach (var chalanDetail in aIChalanList)
            {
                // Check Already Exist or not

                DataTable  aTable = new DataTable();

                aTable = aChalanDal.CheckDetailsExistOrNot(chalanDetail);

                if (aTable.Rows.Count == 0)
                {
                    chalanDetail.ChalanDetailId = aClsPrimaryKeyFind.PrimaryKeyMax("ChalanDetailsId", "tblChalanDetail");
                    status = aChalanDal.SaveDataForChalanDetail(chalanDetail);

                    DataTable dtdcinfo = DCInfoWithDCId(chalanDetail.DCStoreId.ToString());
                    UpdateDCStockQuantity(chalanDetail.DCStoreId.ToString(), (Convert.ToDecimal(dtdcinfo.Rows[0]["StockQty"].ToString()) - chalanDetail.Quantity).ToString());

                    // Insert Into Weighted Average (Disabled as per user request to not maintain DC Store WAC)

                    // if (chalanDetail.ChalanDetailId > 0)
                    // {
                    //     aChalanDal.ProcessWeightedAverageB2BTransfer(chalanDetail.ChalanDetailId);
                    // }
                }

            }

            return status;
        }
        public DataTable DCStoreReport(string reqId)
        {
            return aChalanDal.DCStoreReport(reqId);
        }
        public DataTable LoadComunit(string comunitCode)
        {
            return aChalanDal.LoadComunit(comunitCode);
        }
        public bool DCStockInDAL(DCStockNew aDcStockNew)
        {
            return aChalanDal.DCStockInDAL(aDcStockNew);
        }
        public DataTable DCInfoWithDCId(string dcstoreId)
        {
            return aChalanDal.DCInfoWithDCId(dcstoreId);
        }
        public string ChalanNoForShow()
        {
            string ChalanNo = "";
            ChalanNo = ChalanNoGenerator(aClsPrimaryKeyFind.PrimaryKeyMax("ChalanId", "tblChalanInfo"));
            return ChalanNo;
        }

        public string ChalanNoGenerator(int id)
        {
            string code = string.Empty;
            string Id = id.ToString();
            if (Id.Length == 1)
            {
                Id = "00000" + Id;
            }
            if (Id.Length == 2)
            {
                Id = "0000" + Id;
            }
            if (Id.Length == 3)
            {
                Id = "000" + Id;
            }
            code = "IDT" + Id;
            return code;
        }
       
        //public bool UpdateDataForInvoice(ChalanInfo aInvoice)
        //{
        //    return aInvoiceDal.UpdateaInvoice(aInvoice);
        //}

        public DataTable LoadstockReceive()
        {
            return aChalanDal.LoadChalanView();
        }
        public DataTable ComUnit(string ComUnitId)
        {
            return aChalanDal.LoadComUnit(ComUnitId);
        }

        public DataTable ProductInfo(string productId)
        {
            return aChalanDal.LoadProduct(productId);
        }

        public DataTable GetProductDcStore(string productCode, string comunitId)
        {
            return aChalanDal.GetProductDcStore(productCode, comunitId);
        }

        public DataTable ChalanReceiveReport(string comunitId, string parameter)
        {
            return aChalanDal.ChalanReceiveReport(comunitId, parameter);
        }

        public DataTable DcChalanReceiveReport(string comunitId, string parameter)
        {
            return aChalanDal.DcChalanReceiveReport(comunitId, parameter);
        }
    }
}
