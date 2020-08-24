using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
  public  class CoaDAO
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public string AccountName { get; set; }
        public string SSubsidiaryLCode { get; set; }
        public int SubsidiaryLId { get; set; }
        public int? CompanyId { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string CreateBy { get; set; }
        public int? BankAccountId { get; set; }
        public int? CashAccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public bool InBalanceSheet { get; set; }
        public bool InIncomeStatement { get; set; }
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }
    }
}
