using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class BatchWiseTopSheetBll
    {
        BatchWiseTopSheetDal aSheetDal = new BatchWiseTopSheetDal();

        public void LoadManufac(DropDownList ddl)
        {
            aSheetDal.LoadManufac(ddl);
        }

        public void LoadDeliveryManList(DropDownList ddl)
        {
            aSheetDal.LoadDeliveryManList(ddl);
        }

        //public DataTable BatchWiseTopSheet(int companyId, int delManId, int batchId, DateTime invDate)
        //{
        //    return aSheetDal.GetBatchWiseTopSheet(companyId, delManId, batchId, invDate);
        //}

        //public DataTable BatchWiseTopSheetDetail(int companyId, int delManId, int batchId, DateTime invDate)
        //{
        //    return aSheetDal.GetBatchWiseTopSheetDetail(companyId, delManId, batchId, invDate);
        //}


        public DataTable BatchWiseTopSheet(int delManId, int batchId, DateTime invDate)
        {
            return aSheetDal.GetBatchWiseTopSheet(delManId, batchId, invDate);
        }

        public DataTable BatchWiseDeliveryTopSheet(int delManId, int batchId, DateTime invDate)
        {
            return aSheetDal.BatchWiseDeliveryTopSheet(delManId, batchId, invDate);
        }

        public DataTable BatchWiseTopSheetDetail(int delManId, int batchId, DateTime invDate)
        {
            return aSheetDal.GetBatchWiseTopSheetDetail(delManId, batchId, invDate);
        }

        public DataTable ReturnOnDeliveryReport(int delManId, int batchId, DateTime invDate)
        {
            return aSheetDal.ReturnOnDeliveryReport(delManId, batchId, invDate);
        }

        public DataTable BatchWiseDeliveryReturnTopSheet(int delManId, int batchId, DateTime invDate)
        {
            return aSheetDal.BatchWiseDeliveryReturnTopSheet(delManId, batchId, invDate);
        }
    }
}
