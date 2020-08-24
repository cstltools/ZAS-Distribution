using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CreditAdjustmentDao
    {

        public Int32 CrditAdjustmentId { get; set; }
        public Int32 CompanyId { get; set; }
        public Int32 CustomerMasterId { get; set; }
        public Int32 InvoiceId { get; set; }
        public Decimal Amount { get; set; }
        public DateTime ReturnDate { get; set; }
        public String EntryBy { get; set; }
        public DateTime EntryDate { get; set; }

    }
}
