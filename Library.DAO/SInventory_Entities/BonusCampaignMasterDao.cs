using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class BonusCampaignMasterDao
    {
        public int CampgainMasterId { get; set; }

        public string CampaignCode { get; set; }

        public int? CompanyId { get; set; }
        public int? BonusProductId { get; set; }
        public decimal? BonusProductQuantity { get; set; }

        public string CampaignName { get; set; }

        public string CampaignDesc { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? Todate { get; set; }

        public bool? IsActive { get; set; }

        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
