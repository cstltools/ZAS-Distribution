using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class Manufacturer
    {
        public int ManufacId { get; set; }
        public string ManufacName { get; set; }
        public string  ManufacAddress { get; set; }
        public string ManufacCode { get; set; }

        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
