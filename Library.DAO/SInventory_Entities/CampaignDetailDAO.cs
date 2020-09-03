using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CampaignDetailDAO
    
    {
        public int CampaignDetailId { get; set; }
        public int CampgainMasterId { get; set; }
        public string PointProductCode { get; set; }
        public int Quantity { get; set; }
        public string BonusProductCode { get; set; }
        public int BonusQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime FromDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan ToTime { get; set; }
        public bool IsSame { get; set; }

    }

}
