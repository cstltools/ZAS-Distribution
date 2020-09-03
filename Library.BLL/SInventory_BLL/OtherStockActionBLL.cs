using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class OtherStockActionBLL
    {
        OtherStockActionDAL aOtherStockActionDAL =new OtherStockActionDAL();

        public DataTable LoadData(int DCID,int ManuID)
        {
            return aOtherStockActionDAL.LoadData(DCID, ManuID);
        }

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            aOtherStockActionDAL.GetCompanyDropdownList(ddl);
        }

        public void LoadDcInformationByUser(DropDownList ddl, string companyId, string userId)
         {
             aOtherStockActionDAL.GetDcInformationByUser(ddl, companyId, userId);
         }

        public void LoadWhInformation(DropDownList ddl, string companyId)
        {
            aOtherStockActionDAL.GetWhInformation(ddl, companyId);
        }

        public void LoadmanufacturerName(DropDownList ddl)
        {
            aOtherStockActionDAL.LoadmanufacturerName(ddl);
        }
        public void DCLoad(DropDownList aDownList, string userId)
        {
            aOtherStockActionDAL.DCLoad(aDownList, userId);

        }
        public void LoadSC(DropDownList ddl, string userId)
        {
            aOtherStockActionDAL.LoadSC(ddl, userId);
        }
        public void DCLoad(DropDownList aDownList)
        {
            aOtherStockActionDAL.DCLoad(aDownList);

        }
        public void MarketLoad(DropDownList aDownList)
        {
            aOtherStockActionDAL.MarketLoad(aDownList);

        }
        public bool UpdateDCStoreFreezeStock(decimal StockQty, int DCStoreFreezeId)
        {
            return aOtherStockActionDAL.UpdateDCStoreFreezeStock(StockQty, DCStoreFreezeId);
        }
        public DataTable LoadStockData(int DCStoreFreezeId)
        {
            return aOtherStockActionDAL.LoadStockData(DCStoreFreezeId);
        }
        public DataTable LoadStockDCStockData(int DCStoreId)
        {
            return aOtherStockActionDAL.LoadStockDCStockData(DCStoreId);
        }

        public bool UpdateDCStock(decimal StockQty, int DCStoreId)
        {
            return aOtherStockActionDAL.UpdateDCStock(StockQty, DCStoreId);
        }

        public DataTable CheckPermission(string condition, string userId)
        {
            return aOtherStockActionDAL.GetCheckPermission(condition, userId);
        }

        public bool FreezeStockReleasePosting(int dcStoreFreezeId, decimal returnQty)
        {
            return aOtherStockActionDAL.FreezeStockReleasePosting(dcStoreFreezeId, returnQty);
        }
    }
}
