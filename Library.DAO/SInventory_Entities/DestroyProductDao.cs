using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class DestroyProductDao
    {
        public Int32 DestroyId { get; set; }
        public Int32 DcFreezeId { get; set; }
        public Int32 StockOutBy { get; set; }
        public decimal StockOutQty { get; set; }
        public decimal StockOutAmount { get; set; }
        public DateTime StockOutDate { get; set; } 
        public string Reason { get; set; }
        public string ActionStatus { get; set; }        
        public bool IsApproved { get; set; }
        public string ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
