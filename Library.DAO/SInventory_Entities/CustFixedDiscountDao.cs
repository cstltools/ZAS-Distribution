using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CustFixedDiscountDao
    {

        public Int32 CompanyId { get; set; }
        public Int32 CustomerMasterId { get; set; }
        public Int32 DisParcentage { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime InActiveDate { get; set; }
        public String EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public String InActiveBy { get; set; }

    }
}
