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
    public class ProductDiscountBLL
    {
        ProductDiscountDAL aProductDiscountDal=new ProductDiscountDAL();
        public bool SaveProductDiscount(ProductDiscount aProductDiscount)
        {
            try
            {
                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                aProductDiscount.DiscountId = aClsPrimaryKeyFind.PrimaryKeyMax("DiscountId", "tblProductDiscount");
                aProductDiscountDal.SaveProductDiscount(aProductDiscount);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }


        public bool SaveQuotedPrice(QuotedPriceSetup aProductDiscount)
        {
            try
            {
                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                aProductDiscount.QuotedPriceId = aClsPrimaryKeyFind.PrimaryKeyMax("QuotedPriceId", "tblProductQuotedPrice");

                // Inactive Existing

                //aProductDiscountDal.InactiveQuotedPrice(aProductDiscount);

                // Save New

                aProductDiscountDal.SaveQuotedPrice(aProductDiscount);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        public DataTable ProductDiscountView(string pram)
        {
            return aProductDiscountDal.ProductDiscountViewDal(pram);
        }

        public DataTable ProductQuotedPriceView(string pram)
        {
            return aProductDiscountDal.ProductQuotedprice(pram);
        }


        public bool DeleteProductDiscount(string id)
        {
            return aProductDiscountDal.DeleteProductdiscountDal(id);
        }

        public bool DeleteProductQuotedprice(string id)
        {
            return aProductDiscountDal.DeleteProductQuotedprice(id);
        }

        public bool HasProductDiscountName(ProductDiscount aProductDiscount)
        {
            return aProductDiscountDal.HasProductDiscountName(aProductDiscount);
        }

        public DataTable LoadProductDiscount()
        {
            return aProductDiscountDal.LoadProductDiscount();
        }

        public DataTable GetCustomerInfo(string custId)
        {
            return aProductDiscountDal.GetCustomerInfo(custId);

        }

        public DataTable GetProductDiscount()
        {
            return aProductDiscountDal.GetProductDiscount();
        }

        public DataTable LoadProductDiscount(string actionstatus)
        {
            return aProductDiscountDal.LoadProductDiscount(actionstatus);

        }

        public bool ApprovalUpdateDAL(ProductDiscount aProductDiscount)
        {
            return aProductDiscountDal.ApprovalUpdateDAL(aProductDiscount);
        }

        public bool QuotedPriceApproval(QuotedPriceSetup aProductDiscount)
        {
            return aProductDiscountDal.QuotedPriceApprovalDAL(aProductDiscount);
        }


        public DataTable GetProduct(string productId)
        {
            return aProductDiscountDal.GetProduct(productId);

        }
        public void LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)
        {
            aProductDiscountDal.LoadApprovalControlDAL(rdl, pageName, userName);
        }
        public string LoadForApprovalConditionBLL(string pageName, string userName)
        {
            return aProductDiscountDal.LoadForApprovalConditionDAL(pageName, userName);
        }
        public ProductDiscount ProductDiscountEditLoad(string ID)
        {
            return aProductDiscountDal.ProductDiscountEditLoad(ID);
        }

        public DataTable GetProductDiscountApproval(string actionstatus)
        {
            return aProductDiscountDal.GetProductDiscountApproval(actionstatus);
        }

        public DataTable GetProductQuotedPriceApproval(string actionstatus)
        {
            return aProductDiscountDal.GetProductQuotedPriceApproval(actionstatus);
        }

        public DataTable LoadProductDiscount(string fromdate, string todate)
        {
            return aProductDiscountDal.LoadProductDiscount(fromdate, todate);
        }
        public bool UpdateProductDiscountInfo(ProductDiscount aProductDiscount)
        {
            return aProductDiscountDal.UpdateProductDiscountInfo(aProductDiscount);
        }
        public void LoadCustomerMaster(DropDownList ddl,string marketId)
        {
            aProductDiscountDal.LoadCustomerMaster(ddl,marketId);
        }
        public void LoadSalesCenter(DropDownList ddl)
        {
            aProductDiscountDal.LoadSalesCenter(ddl);
        }
        public void LoadArea(DropDownList ddl, string comUnitId)
        {
            aProductDiscountDal.LoadArea(ddl,comUnitId);
        }
        public void LoadMarket(DropDownList ddl, string areaId)
        {
            aProductDiscountDal.LoadMarket(ddl,areaId);
        }
        public void LoadmanufacturerName(DropDownList ddl)
        {
            aProductDiscountDal.LoadmanufacturerName(ddl);
        }

        public void DCLoad(DropDownList aDownList, string companyId)
        {
            aProductDiscountDal.DCLoad(aDownList, companyId);

        }
        public void MarketLoad(DropDownList aDownList, string territoryId)
        {
            aProductDiscountDal.MarketLoad(aDownList, territoryId);
        }

        public void TerritoryLoad(DropDownList aDownList, string mioId)
        {
            aProductDiscountDal.TerritoryLoad(aDownList, mioId);
        }

        public void AreaLoad(DropDownList aDownList, string DcId)
        {
            aProductDiscountDal.AreaLoad(aDownList, DcId);
        }

        public void MIOLoad(DropDownList aDownList, string areaId)
        {
            aProductDiscountDal.MIOLoad(aDownList, areaId);
        }

        public DataTable CheckDiscountExistOrNot(int customerId, string productCode)
        {
            return aProductDiscountDal.CheckDiscountExistOrNot(customerId, productCode);
        }

        public DataTable CheckQuotedPricetExistOrNot(int customerId, string productCode)
        {
            return aProductDiscountDal.CheckQuotedPricetExistOrNot(customerId, productCode);
        }
    }
}
