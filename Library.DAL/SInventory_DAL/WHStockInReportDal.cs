using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class WHStockInReportDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public DataTable GetWarehouseStockInData(string stockInDate)
        {
            string query = @"SELECT * FROM dbo.tblWHStockInMaster AS WSHM WHERE WSHM.Status = 'Approved' AND WSHM.WHStockInDate = '" + stockInDate + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetWarehouseStockInMasterData(string reqId)
        {
//            string query = @"SELECT * FROM dbo.tblWHStockInMaster AS WHSM
//            INNER JOIN dbo.tblManufacturer AS MNCF ON MNCF.ManufacId = WHSM.ManufacId
//            WHERE WHSM.WHStockInMasterID ='" + reqId + "'";

            string query = @"SELECT Ci.CompanyId AS ManufacId, CI.CompanyName AS ManufacName ,* FROM dbo.tblWHStockInMaster AS WHSM
                            INNER JOIN tblWearhouse AS WH ON WHSM.WarehouseId = WH.WearhouseId
                            INNER JOIN tblCompanyInfo AS CI ON WH.CompanyId = CI.CompanyId
                            WHERE WHSM.WHStockInMasterID = '" + reqId + "'";

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetWarehouseStockInDetailData(string reqId)
        {
            string query = @"SELECT * FROM dbo.tblWHStockInDetail AS WHSD 
            INNER JOIN dbo.tblProduct AS PD ON PD.ProductId = WHSD.ProductId
            WHERE WHSD.WHStockInMasterID = '" + reqId + "'";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable CompanyInformation()
        {
            string query = @"SELECT CompanyName , Address ,ContactNo ,  FaxNo  FROM tblCompanyInfo WHERE CompanyId='1' ";
            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void GetProductInformation(DropDownList ddl)
        {
            string queryStr = "SELECT * FROM dbo.tblProduct ORDER	BY ProductName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ProductName", "ProductCode", queryStr);
        }
    }
}
