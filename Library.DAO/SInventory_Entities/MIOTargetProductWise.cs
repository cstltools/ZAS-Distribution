using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class MIOTargetProductWise
    {
        public int MiaTargetId { get; set; }
        public int CompanyId { get; set; }
        public int MiaId { get; set; }
        public string MiaName { get; set; }
        public decimal TargetQty { get; set; }
        public string Period { get; set; }
        public string Year { get; set; }
        public int ProductId { get; set; }
    }
}
