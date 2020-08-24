using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class FreezeStockReportDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public DataTable LoadFreezeStockInfo(string parameter)
        {
            string query = @"SELECT CUNT.ComUnitName,FSTK.ProductCode,FSTK.ProductName,FSTK.BatchNo,FSTK.StockQty,FSTK.StockCondition FROM dbo.tblDCStoreFreeze AS FSTK 
                             INNER JOIN dbo.tblCompanyUnit AS CUNT ON CUNT.ComUnitId = FSTK.ComUnitId
                             WHERE FSTK.StockQty > 0 " + parameter ;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
