using System;

namespace Library.DAO.SInventory_Entities
{
    public class TradeMaterialTransferDao
    {
        public int TradeMaterialTransferId { get; set; }

        public int FromCompanyId { get; set; }
        public int FromLocationId { get; set; }
        public int FromProductId { get; set; }
        public string FromProductCode { get; set; }
        public string FromProductName { get; set; }
        public decimal FromCurrentStockQty { get; set; }

        public int ToCompanyId { get; set; }
        public int ToLocationId { get; set; }
        public int ToItemDescriptionId { get; set; }
        public string ToItemDescriptionCode { get; set; }
        public string ToItemDescription { get; set; }
        public decimal ToCurrentStockQty { get; set; }

        public decimal TransferQty { get; set; }
        public int FgIssueTransactionTypeId { get; set; }
        public int RmReceiveTransactionTypeId { get; set; }
        public string FgIssueTransactionType { get; set; }
        public string RmReceiveTransactionType { get; set; }

        public string Remarks { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
