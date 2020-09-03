using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class CustomerReturnDetailDAO
    {
        public int CustReturnDetailId { get; set; }
        public int InvoiceDetailId { get; set; }
        public int ReturnQty { get; set; }
        public decimal ReturnAmount { get; set; }
        public int CustReturnId { get; set; }
    }
}
