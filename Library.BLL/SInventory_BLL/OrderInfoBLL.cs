using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class OrderInfoBLL
    {
        OrderInfoDAL aOrderInfoDal=new OrderInfoDAL();
        public DataTable LoadOrder(string comunitId, string manufacId, string marketId)
        {
            return aOrderInfoDal.LoadOrder(comunitId, manufacId, marketId);
        }
        public void LoadMarketByInvoice(DropDownList ddl, string comunitId)
        {
            aOrderInfoDal.LoadMarketByInvoice(ddl, comunitId);
        }
        public DataTable LoadOrderForOrderCreation(string comunitId, string manufacId, string marketId)
        {
            return aOrderInfoDal.LoadOrderForOrderCreation(comunitId, manufacId, marketId);
        }

        public DataTable LoadOrderForOrderCreation(string parameter)
        {
            return aOrderInfoDal.LoadOrderForOrderCreation(parameter);
        }

        public void LoadMarketOrderWise(DropDownList ddl, string comunitId)
        {
            aOrderInfoDal.LoadMarketOrderWise(ddl,comunitId);
        }

        public void AreaLoad(DropDownList aDownList, string dcId)
        {
            aOrderInfoDal.AreaLoad(aDownList,dcId);
        }

        public void TerritoryLoad(DropDownList aDownList, string areaId)
        {
            aOrderInfoDal.TerritoryLoad(aDownList,areaId);
        }

        public void CustomerLoad(DropDownList aDownList, string marketId)
        {
            aOrderInfoDal.CustomerLoad(aDownList,marketId);
        }

        public void BatchNoLoad(DropDownList aDownList, string companyId, string date, string delmanId)
        {
            aOrderInfoDal.BatchNoLoad(aDownList, companyId, date, delmanId);

        }

        public void BatchNoLoadByUser(DropDownList aDownList, string date, string delmanId, string userId)
        {
            aOrderInfoDal.BatchNoLoadByUser(aDownList, date, delmanId,userId);

        }

        public void BatchNoLoad(DropDownList aDownList, string date, string delmanId)
        {
            aOrderInfoDal.BatchNoLoad(aDownList, date, delmanId);

        }

        public void MarketLoad(DropDownList aDownList, string territoryId)
        {
            aOrderInfoDal.MarketLoad(aDownList,territoryId);
        }

        public void LoadSC(DropDownList ddl)
        {
            aOrderInfoDal.LoadSC(ddl);
        }
        public void LoadArea(DropDownList ddl, string comUnitId)
        {
            aOrderInfoDal.LoadArea(ddl, comUnitId);
        }
        public void LoadSC(DropDownList ddl,string userId)
        {
            aOrderInfoDal.LoadSC(ddl,userId);
        }
        public void LoadManufac(DropDownList ddl)
        {
            aOrderInfoDal.LoadManufac(ddl);
        }

        public void LoadDC(DropDownList ddl, string companyId)
        {
            aOrderInfoDal.LoadDC(ddl,companyId);
        }

        public DataTable LoadOrderWithInvoice(string batchId)
        {
            return aOrderInfoDal.LoadOrderWithInvoice(batchId);
        }

        public DataTable LoadInvoice(string invoiceId)
        {
            return aOrderInfoDal.LoadInvoice(invoiceId);
        }

        public DataTable LoadDelivaryInvoice(string invoiceno, string comunitId)
        {
            return aOrderInfoDal.LoadDelivaryInvoice(invoiceno, comunitId);
        }

        public DataTable GetQty(string invoicedetailId)
        {
            return aOrderInfoDal.GetQty(invoicedetailId);
        }

        public DataTable LoadOrderWithDetail(string orderid)
        {
            return  aOrderInfoDal.LoadOrderWithDetail(orderid);
        }

        public DataTable LoadDeliveryMan(string deliveryId)
        {
            return aOrderInfoDal.LoadDeliveryMan(deliveryId);

        }

        public DataTable LoadOffer(string InvoiceId)
        {
            return aOrderInfoDal.LoadOffer(InvoiceId);
        }
        public DataTable GetTradeTerm(string amount)
        {
            return aOrderInfoDal.GetTradeTerm(amount);
        }
        public DataTable GetTradeTermOld(string amount)
        {
            return aOrderInfoDal.GetTradeTermOld(amount);
        }
        public DataTable GetFixedCustomer(string customercode)
        {
            return aOrderInfoDal.GetFixedCustomer(customercode);
        }
        public DataTable GetFixedCustomerfromInvoiceTable(string invoiceid)
        {
            return aOrderInfoDal.GetFixedCustomerfromInvoiceTable(invoiceid);
        }
        public bool UpdateInvoiceStatus(string id)
        {
            return aOrderInfoDal.UpdateInvoiceStatus(id);
        }
        public DataTable ProductVat(string productcode)
        {
            return aOrderInfoDal.ProductVat(productcode);
        }
        public DataTable ProductDiscount(string productcode, string customerId, string invoicedate)
        {
            return aOrderInfoDal.ProductDiscount(productcode, customerId, invoicedate);
        }
        public void LoadMarketbyArea(DropDownList ddl, string areaId)
        {
            aOrderInfoDal.LoadMarketbyArea(ddl, areaId);
        }
        public void LoadMarket(DropDownList ddl, string comunitId)
        {
            aOrderInfoDal.LoadMarket(ddl, comunitId);
        }


        public DataTable LoadInvoice(string ComUnitId, string ManufId, string marketid, DateTime invDate)
        {
            return aOrderInfoDal.LoadInvoice(ComUnitId, ManufId, marketid, invDate);
        }
        public DataTable LoadOrderExistsBll(string orderid)
        {
            return aOrderInfoDal.LoadOrderExistsDal(orderid);
        }

        public void LoadDeliveryManList(DropDownList ddl)
        {
            aOrderInfoDal.LoadDeliveryManList(ddl);
        }
        public void LoadDeliveryMan(DropDownList aDropDownList)
        {
            aOrderInfoDal.LoadDeliveryMan(aDropDownList);
        }

        public DataTable GetCustomerFixedDiscount(string custCode)
        {
            return aOrderInfoDal.GetCustomerFixedDiscount(custCode);
        }

        public void LoadDcInformationByUser(DropDownList ddl, string companyId, string userId)
        {
            aOrderInfoDal.GetDcInformationByUser(ddl, companyId, userId);
        }

        public DataTable LoadCustomerDueHistory(string customerMasterId)
        {
            return aOrderInfoDal.GetCustomerDueHistory(customerMasterId);
        }
    }
}
