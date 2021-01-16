using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class DirectStockOutDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public DataTable StockInformationByCode(string payId)
        {
            string query = @"SELECT ComUnitName,StockOutCode,StockOutType,DCS.ProductCode,DCS.ProductName,PackSize,StockOutDate,
                             ChalanNo,ChalanDate,StockOutQty,StockOutReason FROM tblDirectStockOut AS DCS
                             LEFT JOIN tblCompanyUnit AS UNT ON DCS.DCId = UNT.ComUnitId
                             LEFT JOIN tblDCStore AS DC ON DCS.DCStoreId = DC.DCStoreId
                             WHERE StockOutCode = " + payId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
