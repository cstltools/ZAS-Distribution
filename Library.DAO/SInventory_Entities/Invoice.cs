using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
       
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerMasterId { get; set; }

        public int ComUnitId { get; set; }
        public int MiaId { get; set; }
        public int PaymentTypeId { get; set; }
       
        public decimal TpTotal { get; set; }
        public decimal TpVat { get; set; }
        public decimal TpDiscount { get; set; }
        public decimal TpGrandTotal { get; set; }
        public int UserId { get; set; }
        public string ComUnitCode { get; set; }
        public int OrderId { get; set; }
        public int ReturnInvoiceid { get; set; }
        public string DeliveryInvoiceStatus { get; set; }
        public string DelivaryInvoiceNo { get; set; }
        public decimal TotalSpecialAmount { get; set; }
        public int DeliveryManId { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ProductOffer { get; set; }
        public bool OldTradePolicy { get; set; }
        public string Remarks { get; set; }
        public string MIACode { get; set; }
        public string MIAName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string AreaCode { get; set; }
        public string DisCode { get; set; }
        public string FEName { get; set; }
        public string RegionCode { get; set; }
        public string DZSMName { get; set; }
        public bool FixedCustomer { get; set; }

        public string DpNAme { get; set; }
        public string DpMob { get; set; }
       
    }
}
