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
   public class ProformaOrInvoiceReturnBLL
    {
       ProformaOrInvoiceReturn aInvoiceReturn = new ProformaOrInvoiceReturn();
       public bool chkProforma(string Proforma)
       {
           try
           {
               if (aInvoiceReturn.HasProforma(Proforma))
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           { }
       }
       public bool chkInvoice(string Invoice)
       {
           try
           {
               if (aInvoiceReturn.HasInvoice(Invoice))
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           { }
       }
       public int DeleteProforma(string Invoice, string userId)
       {
           return aInvoiceReturn.DeleteProformaDal(Invoice,userId);
       }
       public int DeleteDeliveyInvoice(string Invoice, string userId)
       {
           return aInvoiceReturn.DeleteDeliveyInvoiceDal(Invoice, userId);
       }
       public DataTable LoadDetailID(string Invoice)
       {
           return aInvoiceReturn.LoadDetailID(Invoice);
       }
       public DataTable LoadStock(string Invoice)
       {
           return aInvoiceReturn.LoadStock(Invoice);
       }

       public bool ProformaReturnPosting(string invoiceNo)
       {
           return aInvoiceReturn.ProformaReturnPosting(invoiceNo);
       }

       public bool InvoiceDeletePosting(string invoiceNo)
       {
           return aInvoiceReturn.InvoiceDeletePosting(invoiceNo);
       }

       public bool CheckWHStockIn(string whStockInCode)
       {
           try
           {
               if (aInvoiceReturn.HasWHStockIn(whStockInCode))
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           { }
       }

       public bool CheckSampleInvoice(string sampleinvoiceCode)
       {
           try
           {
               if (aInvoiceReturn.CheckSampleInvoice(sampleinvoiceCode))
               {
                   return true;
               }
               else
               {
                   return false;
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
