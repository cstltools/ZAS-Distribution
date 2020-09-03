using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CompanyWiseDepositDao
    {
        public int DepositId { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public decimal Amount { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime DepositDate { get; set; }
        public bool IsDelete { get; set; }
        public string DeleteBy { get; set; }
        public DateTime DeleteDate { get; set; }

    }
}
