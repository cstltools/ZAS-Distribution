using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.DAO.SInventory_Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string PackSize { get; set; }
        public int PackSizeId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ManufacId { get; set; }
        public int StockUOMId { get; set; }
        public int ProTypeId { get; set; }
        public int IngridentsId { get; set; }
        public int ProductBrandId { get; set; }
        public int CaseId { get; set; }

        public int CompanyId { get; set; }
    }
}
