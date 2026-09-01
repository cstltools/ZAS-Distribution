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
    public class OrderListBLL
    {
        ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
        OrderListDAL aOrderListDal=new OrderListDAL();
        public bool SaveOrderMaster(OrderInfoMaster aListMasterDao,out int maxorderId)
        {
            maxorderId = OrderId();
            aListMasterDao.OrderId = maxorderId;
            aListMasterDao.OrderCode = OrdNo();
            return aOrderListDal.SaveOrderMaster(aListMasterDao);
        }
        private int OrderId()
        {
            int ReqId = 0;
            ReqId = aClsPrimaryKeyFind.PrimaryKeyMax("OrderId", "tblOrder", "SSIDB");
            return ReqId;
        }
        public string OrdNo()
        {
            string ordNo = string.Empty;

            ordNo = OrdnNoGenerator(aOrderListDal.OrderManualId());

            return ordNo;
        }

        public bool ApprovalUpdateDAL(OrderInfoMaster aOrderInfoMaster)
        {
            return aOrderListDal.ApprovalUpdateDAL(aOrderInfoMaster);

        }
        public DataTable GetSpecialApproveOrder(string actionstatus)
        {
            return aOrderListDal.GetSpecialApproveOrder(actionstatus);
        }

        public void LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)
        {
            aOrderListDal.LoadApprovalControlDAL(rdl, pageName, userName);
        }
        public string LoadForApprovalConditionBLL(string pageName, string userName)
        {
            return aOrderListDal.LoadForApprovalConditionDAL(pageName, userName);
        }
        private string OrdnNoGenerator(int id)
        {
            string code = string.Empty;
            string Id = id.ToString();
            if (Id.Length == 1)
            {
                Id = "000000" + Id;
            }
            if (Id.Length == 2)
            {
                Id = "00000" + Id;
            }
            if (Id.Length == 3)
            {
                Id = "0000" + Id;
            }
            if (Id.Length == 4)
            {
                Id = "000" + Id;
            }
            if (Id.Length == 5)
            {
                Id = "00" + Id;
            }
            if (Id.Length == 6)
            {
                Id = "0" + Id;
            }
            code = "ORD-" + Id;
            return code;
        }

        public DataTable CustomerInfoAll(string custCode)
        {
            return aOrderListDal.CustomerInfoAll(custCode);
        }

        public DataTable GetAllAvailableOrder()
        {
            return aOrderListDal.GetAllAvailableOrder();
        }

        public DataTable CustomerInfo(string custCode)
        {
            return aOrderListDal.CustomerInfo(custCode);
        }

        public void LoadDCByArea(DropDownList ddl, string id)
        {
            aOrderListDal.LoadDCByArea(ddl,id);
        }

        public DataTable GetCompanybydc(string id)
        {
            return aOrderListDal.GetCompanybydc(id);
        }

        public DataTable GetCustomerCreditLimit(string customerId)
        {
            return aOrderListDal.GetCustomerCreditLimit(customerId);

        }

        public DataTable GetCustomerUsedCreditLimit(string customerId)
        {
            return aOrderListDal.GetCustomerUsedCreditLimit(customerId);

        }

        public DataTable GetCustomerEditData(string id)
        {
            return aOrderListDal.GetCustomerEditData(id);
        }

        public bool DeleteDetail(string id)
        {
            return aOrderListDal.DeleteDetail(id);
        }

        public bool UpdateOrderMaster(OrderInfoMaster aOrderInfoMaster)
        {
            return aOrderListDal.UpdateOrderMaster(aOrderInfoMaster);
        }

        public string SaveOrderDetail(List<OrderInfoDetail> aOrderListDetailDaoList)
        {
            foreach (var aOrderListDetailDao in aOrderListDetailDaoList)
            {
                aOrderListDal.SaveOrderDetail(aOrderListDetailDao);
            }
            return "Data Saved Successfully";
        }
        public void LoadmanufacturerName(DropDownList ddl)
        {
            aOrderListDal.LoadmanufacturerName(ddl);
        }

        public void LoadAreaByRegion(DropDownList ddl, string id)
        {
            aOrderListDal.LoadAreaByRegion(ddl,id);
        }

        public void LoadRegion(DropDownList ddl)
        {
            aOrderListDal.LoadRegion(ddl);
        }

        public void DCLoad(DropDownList aDownList,string companyId)
        {
            aOrderListDal.DCLoad(aDownList,companyId);

        }

        public void MIOLoad(DropDownList aDownList, string areaId)
        {
            aOrderListDal.MIOLoad(aDownList,areaId);
        }

        public DataTable GetMIOIDByTerritory(string terId)
        {
            return aOrderListDal.GetMIOIDByTerritory(terId);
        }

        public void MarketLoad(DropDownList aDownList, string territoryId)
        {
            aOrderListDal.MarketLoad(aDownList,territoryId);
        }

        public void TerritoryLoad(DropDownList aDownList, string mioId)
        {
            aOrderListDal.TerritoryLoad(aDownList,mioId);
        }

        public void AreaLoad(DropDownList aDownList, string DcId)
        {
            aOrderListDal.AreaLoad(aDownList,DcId);
        }

        public void MIOLoad(DropDownList aDownList)
        {
            aOrderListDal.MIOLoad(aDownList);
        }

        public bool UpdateDataForCompanyInfo(OrderInfoMaster aOrderInfoMaster)
        {
            return aOrderListDal.UpdateCompanyInfo(aOrderInfoMaster);
        }

        public DataTable GetCustomerPaymentInfo(int id)
        {
            return aOrderListDal.GetCustomerPaymentInfo(id);
        }

        public DataTable LoadMioByCompany(string terittoryId, string companyId)
        {
            return aOrderListDal.GetMioByCompany(terittoryId, companyId);
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            return aOrderListDal.GetCustomerInfoByCode(customerCode);
        }
    }
}
