using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CustomerMaster
    {
        public int CustomerMasterId { get; set; }
        public int CategoryId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string CellNo { get; set; }
        public int MarketId { get; set; }
        public string Addrees2 { get; set; }
        public string City { get; set; }
        public string ConPerson { get; set; }
        public string ShippingCond { get; set; }
        public string PaymentType { get; set; }
        public string TermOfPayment { get; set; }
        public string CustomerCodeOld { get; set; }
        public DateTime UploadDate { get; set; }
        public bool ExcelUpload { get; set; }
        public bool FixedCustomer { get; set; }
        public bool IsVatApplicable { get; set; }

        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public string MarketCode { get; set; }
        public string MIACode { get; set; }
        public string AreaCode   { get; set; }
        public string DisCode { get; set; }
        public string FEName { get; set; }
        public string ComUnitCode { get; set; }
        public string ComUnitName { get; set; }
        public string RegionCode { get; set; }
        public string DZSMName { get; set; }
        
        
        public string MarketName { get; set; }
        public string MiaName { get; set; }


        public bool IsActive { get; set; }

        public int AITGLId { get; set; }
        
    }
}
