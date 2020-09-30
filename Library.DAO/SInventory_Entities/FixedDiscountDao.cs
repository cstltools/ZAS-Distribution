using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class FixedDiscountDao
    {
        public int DiscountId { get; set; }
	    public int CompanyId { get; set; }
	    public int CustMasterId { get; set; }
	    public decimal DiscountParcentage { get; set; }
	    public bool IsActive { get; set; } 
	    public string EntryBy { get; set; }
	    public DateTime EntryDate { get; set; }
        public string ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ActionStatus { get; set; }
        public DateTime ActiveDate { get; set; }
        public string InactiveBy { get; set; }
        public DateTime InactiveDate { get; set; }
    }
}
