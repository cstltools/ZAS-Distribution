using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

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

        public DataTable LoadWarehouseStockInStatement(string pram)
        {
            string query = @"SELECT CI.CompanyName,WHStockInDate,ChallanNo,ChallanDate,ReferenceNo,ReferenceDate,
                             PD.ProductCode,PD.ProductName,PKS.PackSizeName,Batch,ExpDate,MfgDate,Qty,WHM.Status FROM tblWHStockInMaster AS WHM
                             LEFT JOIN tblWHStockInDetail AS WHD ON WHM.WHStockInMasterID = WHD.WHStockInMasterID
                             LEFT JOIN tblProduct AS PD ON PD.ProductId = WHD.ProductId
                             LEFT JOIN tblPackSize AS PKS ON PD.PackSizeId = PKS.PackSizeId
                             LEFT JOIN tblWearhouse AS WH ON WHM.WarehouseId = WH.WearhouseId
                             LEFT JOIN tblCompanyInfo AS CI ON WH.CompanyId = CI.CompanyId
                             WHERE WHM.Status = 'Approved' " + pram;

            return aInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public int DeleteWHStockIn(string whStockInCode, string userId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@WhStockInCode", whStockInCode));
            aSqlParameterList.Add(new SqlParameter("@userId", userId));
            return aInternalDal.RunStoreProcedure("sp_Delete_WHStockIn", aSqlParameterList, "SSIDB");
        }
    }
}
