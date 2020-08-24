using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class InvoiceBatchDAO
    {
        public int BatchId { get; set; }
        public int BatchNo { get; set; }
        public int CompanyId { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryManId { get; set; }
    }
}
