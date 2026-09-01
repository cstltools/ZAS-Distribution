using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class PaymentBachMasterDao
    {
        public Int32 BatchMasterId { get; set; }
        public Int32 DeliveryManId { get; set; }
        public Int32 PayCustomerId { get; set; }
        public string BatchNo { get; set; }
        public string BatchDate { get; set; }
        public int EntryBy { get; set; }
    }
}
