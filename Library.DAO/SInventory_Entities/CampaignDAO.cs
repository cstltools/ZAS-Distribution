using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CampaignDAO
    {
        public int CampgainMasterId { get; set; }
        public string CampaignCode { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public int CompanyId { get; set; }

    }
}
