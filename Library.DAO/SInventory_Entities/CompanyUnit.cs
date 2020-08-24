using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CompanyUnit
    {
        public int ComUnitId { get; set; }
        public int CompanyId { get; set; }
        public string ShortName { get; set; }
        public string ComUnitName { get; set; }
        public string ComUnitCode { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int DcTypeId { get; set; }
    }
}
