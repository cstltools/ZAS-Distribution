using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class TerritoryInfoDao
    {
        public int TerritoryId { get; set; }
        public int AreaId { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string TerShortName { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public int RegionId { get; set; }
    }
}
