using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CompanyInformation
    {
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CmpShortName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string FaxNo { get; set; }
        public string Remarks { get; set; }

        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
