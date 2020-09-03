using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.PettyCashDao
{
   public class PettyCashDetailsDao
    {
        public int PettyCashDetailsId { get; set; }
        public int PettyCashmasterId { get; set; }
        public int PerticulerId { get; set; }
        public string CashReason { get; set; }
        public decimal Amount { get; set; }
    
    }
}
