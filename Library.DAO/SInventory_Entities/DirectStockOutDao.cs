using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class DirectStockOutDao
    {

        public int DCStockOutId { get; set; }
        public int DCId { get; set; }
        public string Code { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime StockOutDate { get; set; }
        public int DCStoreId { get; set; }
        public decimal StockOutQty { get; set; }
        public decimal StockOutValue { get; set; }
        public string StockOutReason { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
