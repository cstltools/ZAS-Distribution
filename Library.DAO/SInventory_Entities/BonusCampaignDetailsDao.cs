using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class BonusCampaignDetailsDao
    {
        public int CampaignDetailId { get; set; }

        public int? CampgainMasterId { get; set; }

        public int? PointProductId { get; set; }

        public decimal? Quantity { get; set; }
    }
}
