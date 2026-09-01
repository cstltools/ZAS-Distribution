using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class RAN_DetailsDao
    {
        public int RanDetailID { get; set; }
        public int? RanMasterID { get; set; }

        public int? ProductId { get; set; }

        public string Batch { get; set; }

        public DateTime? ExpDate { get; set; }

        public DateTime? MfgDate { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Price { get; set; }

        public decimal? VAT { get; set; }

        public decimal? TotalAmount { get; set; }

    }
}
