using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class DeliveryPersonDAO
    {
        public int DeliveryManId { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int EmpInfoId { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
