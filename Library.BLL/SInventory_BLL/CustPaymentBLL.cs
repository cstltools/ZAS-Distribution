using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class CustPaymentBLL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        CustPaymentDAL aCustPaymentDal=new CustPaymentDAL();
        public bool SaveCustPayment(CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDetailList )
        {
            try
            {

                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                aCustPayment.CustPayId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblCustomerPay");

                aCustPaymentDal.SaveCustPayment(aCustPayment);
               
                
              // aCustPaymentDal.SaveFullInvoice(aCustPayment);
                
                foreach (var custPaymentDetail in aPaymentDetailList)
                {
                    // Check if exist

                    //DataTable aTable = new DataTable();
                    //aTable = aCustPaymentDal.GetInvoiceExistOrNot(custPaymentDetail.InvoiceId);

                    //if (aTable.Rows.Count == 0)
                    //{
                        // Save Master

                        custPaymentDetail.CustPayId = aCustPayment.CustPayId;

                        //Save Detail

                        if (SaveCustDetail(custPaymentDetail))
                        {
                            // Journal Posting

                            aCustPaymentDal.PaymentJournalPosting(aCustPayment.CustPayId, custPaymentDetail.InvoiceId);
                        }
                        else
                        {
                            aCustPaymentDal.DeletePaymentMasterInfo(aCustPayment);
                        }
                    //}


                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        

        public bool SaveCustPayment(CustPayment aCustPayment, CustPaymentDetail custPaymentDetail)
        {
            try
            {

                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                aCustPayment.CustPayId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblCustomerPay");
                aCustPaymentDal.SaveCustPayment(aCustPayment);
                
                    custPaymentDetail.CustPayId = aCustPayment.CustPayId;
                    SaveCustDetail(custPaymentDetail);
                
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public bool UpdateInvoicePaymentAmount(string amount, string status, string id)
        {
            return aCustPaymentDal.UpdateInvoicePaymentAmount(amount, status, id);
        }

        public void LoadCustomer(DropDownList ddl, string batchId)
        {
            aCustPaymentDal.LoadCustomer(ddl,batchId);
        }

        public DataTable GetPrevAmount(string invoiceId)
        {
            return aCustPaymentDal.GetPrevAmount(invoiceId);
        }
        public bool SaveCustDetail(CustPaymentDetail aCustPaymentDetail)
        {
            try
            {

                // Validate Customer Payment by (InvoiceId & Amount & Date)

                DataTable aTable = aCustPaymentDal.CheckCustomerPaymentExistOrNot(aCustPaymentDetail);


                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                if (aTable.Rows.Count == 0)
                {

                    aCustPaymentDetail.CustPayDetailId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayDetailId","tblCustPayDetail");
                    return aCustPaymentDal.SaveCustDetail(aCustPaymentDetail);
                }

                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public void LoadSC(DropDownList ddl)
        {
            aCustPaymentDal.LoadSC(ddl);
        }
        public void LoadSC(DropDownList ddl, string userId)
        {
            aCustPaymentDal.LoadSC(ddl,userId);
        }
        public void LoadManufac(DropDownList ddl)
        {
            aCustPaymentDal.LoadManufac(ddl);
            
        }

        public void LoadDC(DropDownList ddl, string companyId)
        {
            aCustPaymentDal.LoadDC(ddl, companyId);
        }

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            aCustPaymentDal.GetCompanyDropdownList(ddl);
        }

        public void LoadDcWiseAreaAloByDc(DropDownList ddl,string DcWiseAreaID)
        {
            aCustPaymentDal.LoadDcWiseAreaAloByDc(ddl,DcWiseAreaID);
        }

        public void LoadTerritoryOnDropdownList(DropDownList ddl, string areaId)
        {
            aCustPaymentDal.GetTerritoryOnDropdownList(ddl, areaId);
        }
        
        public void LoadCustomerMaster(DropDownList ddl, string marketId)
        {
            aCustPaymentDal.LoadCustomerMaster(ddl,marketId);
        }
        public void LoadMarket(DropDownList ddl, string comunitId)
        {
            aCustPaymentDal.LoadMarket(ddl,comunitId);
        }
        public void PaymentTypeLoadBLL(DropDownList aDropDownList)
        {
            aCustPaymentDal.PaymentTypeLoad(aDropDownList);
        }
        public DataTable LoadInvoice(string comUnitId, string customerId, string marketId)
        {
            return aCustPaymentDal.LoadInvoice(comUnitId, customerId, marketId);
        }
        public DataTable LoadInvoiceNew(string batchId, string customerId)
        {
            return aCustPaymentDal.LoadInvoiceNew(batchId, customerId);
        }

        public DataTable LoadInvoice(string comUnitId, string marketId)
        {
            return aCustPaymentDal.LoadInvoice(comUnitId, marketId);
        }

        public CustomerMaster CustomerLoad(string aCustomerMaster)
        {
            return aCustPaymentDal.CustomerLoad(aCustomerMaster);
        }
        public CustomerMaster DetailCustomerLoad(string aCustomerMaster)
        {
            return aCustPaymentDal.DetailCustomerLoad(aCustomerMaster);
        }
        public Product DetailProductLoad(string aProduct)
        {
            return aCustPaymentDal.DetailProductLoad(aProduct);
        }

        public void GetMarketNameByTerritoryId(DropDownList ddl, string terrId)
        {
            aCustPaymentDal.GetMarketNameByTerritoryId(ddl, terrId);
        }

        public DataTable LoadCustomerPaymentDueNew(string parameter)
        {
            return aCustPaymentDal.CustomerPaymentDueNew(parameter);
        }

        public void LoadDcInfo(DropDownList ddl, string comId)
        {
            aCustPaymentDal.LoadDcInfo(ddl, comId);
        }

        public void CustomerInfoLoad(DropDownList ddl)
        {
            aCustPaymentDal.GetCustomerInfoLoad(ddl);
        }

        public DataTable LoadInvoiceByCustomer(string custmerId, string dcId)
        {
            return aCustPaymentDal.GetInvoiceByCustomer(custmerId,dcId);
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            return aCustPaymentDal.GetCustomerInfoByCode(customerCode);
        }

        public DataTable GetCreditBalanceInfo(string value)
        {
            return aCustPaymentDal.GetCreditBalanceInfo(value);
        }

        public DataTable LoadInvoiceByCustomerByUser(string custmerId, string userId, string dc)
        {
            return aCustPaymentDal.GetInvoiceByCustomer(custmerId,userId,dc);
        }

        public DataTable LoadInvoiceNewByUser(string batchId, string customerId, string userId)
        {
            return aCustPaymentDal.LoadInvoiceNewByUser(batchId, customerId, userId);
        }

        public void LoadBankAccount(DropDownList ddlBank, string companyId)
        {
            aCustPaymentDal.LoadBankAccount(ddlBank,companyId);
        }

        public void LoadCashAccount(DropDownList ddlCash, string companyId)
        {
            aCustPaymentDal.LoadCashAccount(ddlCash,companyId);
        }
    }
}
