
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class OrderInfoMaster
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public int ComUnitId { get; set; }
        public string ComUnitCode { get; set; }
        public string ComUnitName { get; set; }
        public string MIOCode { get; set; }
        public string MIOName { get; set; }
        public int ManufacId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal GrossValue { get; set; }
        public DateTime SubmissionDate { get; set; }
        public bool IsInvoice { get; set; }
        public bool IsManual { get; set; }
        public int DeliveryPersonId { get; set; }
        public int MIOId { get; set; }
        public int ASMId { get; set; }
        public int RSMId { get; set; }
        public int CustomerMasterId { get; set; }
        public int MarketId { get; set; }
        public int TerritoryId { get; set; }
        public int AreaId { get; set; }
        public int RegionId { get; set; }
        public bool IsDirect { get; set; }
        
        public bool IsSpecialApproval { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ActionStatus { get; set; }


    }
}
