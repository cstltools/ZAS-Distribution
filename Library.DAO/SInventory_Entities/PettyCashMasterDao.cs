using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.PettyCashDao
{
  public class PettyCashMasterDao
    {
        public int PettyCashmasterId { get; set; }
        public int ConpanyId { get; set; }
        public DateTime PostingDate { get; set; }
        public int CashAccId { get; set; }
        public string Remark { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ActiveStatus { get; set; } 
          
    }
}
