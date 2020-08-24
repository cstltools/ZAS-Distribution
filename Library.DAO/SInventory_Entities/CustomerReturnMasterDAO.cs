using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CustomerReturnMasterDAO
    {
        public int CustReturnId { get; set; }
        public int InvoiceId { get; set; }
        public int TotalQty { get; set; }
        public decimal TotalAmount { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ActionStatus { get; set; }
    }
}
