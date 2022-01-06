using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    class IncomeStatementDao
    {
    }

    public class IncomeStatementGlDao
    {
        public string ParticularName { get; set; }
        public decimal SalesValue { get; set; }
        public decimal CollectionValue { get; set; }
        public decimal DueValue { get; set; }
    }
}
