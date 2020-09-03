using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class tblNonTranscationalInvoiceMasterDao
    {

        public int InvReturnMasterID { get; set; }

        public int InvoiceId { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime SalesDate { get; set; }
        public int CustomerMasterId { get; set; }
        public string ReturnCode { get; set; }
        public decimal TotalReturn { get; set; }
        public decimal TotalValue { get; set; }
        public int ComUnitId { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ActionStatus { get; set; }
    }
}
