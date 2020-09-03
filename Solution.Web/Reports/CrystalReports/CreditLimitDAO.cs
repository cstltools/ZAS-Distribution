using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CreditLimitDAO
    {
        public int CreditLimitId { get; set; }
        public int CompanyId { get; set; }
        public int CustomerMasterId { get; set; }
        public decimal LimitAmount { get; set; }
        public int DayLimit { get; set; }
        public bool IsActive { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ActionStatus { get; set; }

    }
}
