using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class PaymentTranscationDetailDao
    {
        public int CustPayDetailId { get; set; }
        public int InvoiceId { get; set; }
        public decimal PaymentAmount { get; set; }
        public int CustPayId { get; set; }

        public int BankAccId { get; set; }
        public int CashAccId { get; set; }

        public string Remarks { get; set; }
    }
}
