using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class MarketInfo
    {
        public int MarketId { get; set; }
        public int TerritoryId { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string MktShortName { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public int RegionId { get; set; }
        public int AreaId { get; set; }
    }
}
