using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
   public class DcWiseAreaAloDao
    {
           public int DcWiseAreaId { get; set; }
           public int CompanyId { get; set; }
           public int DCId { get; set; }
           public int AreaId { get; set; }
            public bool IsActive { get; set; }
            public string EntryBy { get; set; }
            public DateTime EntryDate { get; set; }
            public string UpdateBy { get; set; }
            public DateTime UpdateDate { get; set; }
    }
}
