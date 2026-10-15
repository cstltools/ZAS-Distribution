using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;

namespace Library.DAL.SInventory_DAL
{
    public class MaterialConversionReceiveDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        DB_Manager aDbManager = new DB_Manager();
        public void CreateConnection_DAL()
        {
            aDbManager.CreateConnection("SalesDisDB_New3");
        }
        public void CloseAllConnection_DAL()
        {
            aDbManager.CloseConnection();
        }

        public DataTable LoadMaterialConversionList(string param)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@Param", param));

            string queryStr = @"SELECT  DISTINCT CD.MaterialConversionDetailId,CM.MaterialConversionMasterId,FG.ProductCode,FG.ProductId,FG.ProductName,FG.PackSize,CM.CostPerUnit, 0 AS Vat,
            CAST(BS.Trans_Qty*-1 AS INT) FinalOutputQuantityInt,CAST(BS.Trans_Qty*-1 AS INT) * CM.CostPerUnit AS TotalCostOfThisBatch,MAP.RawMaterialId,Batch_No, Mfg_Date, Exp_Date  FROM CSTL_ERPDB..tbl_MC_MaterialConversionMaster AS CM
			LEFT JOIN CSTL_ERPDB..tbl_MC_ProductionUnit AS PUNT ON CM.ProductionUnitId = PUNT.ProductionUnitId
            INNER JOIN CSTL_ERPDB..tbl_MC_RecipeMaster AS RCM ON CM.RecipeSetupMasterId = RCM.RecipeSetupMasterId
            LEFT JOIN CSTL_ERPDB..tblCompanyInfo AS CI ON CM.BusinessUnitId = CI.CompanyId

			 INNER JOIN tbl_MC_ProductRawMaterialDetails AS MAP ON CM.FGItemId = MAP.ProductId
            
            INNER JOIN CSTL_ERPDB..tbl_MC_MaterialConversionDetails AS CD ON CM.MaterialConversionMasterId = CD.MaterialConversionMasterId
            INNER JOIN CSTL_ERPDB..tbl_INV_BatchWiseStock AS BS ON CD.MaterialConversionDetailId = BS.MaterialConversionDetailId

			 LEFT JOIN (SELECT DISTINCT MaterialConversionMasterId FROM tblWHStockInMaster AS WM
            LEFT JOIN tblWHStockInDetail AS WD ON WM.WHStockInMasterID = WD.WHStockInMasterID
            WHERE MaterialConversionMasterId IS NOT NULL AND LTRIM(RTRIM(Upper(Status))) != LTRIM(RTRIM(Upper('Reject')))) AS CS 
			ON CM.MaterialConversionMasterId = CS.MaterialConversionMasterId

			 LEFT JOIN tblProduct AS FG ON CM.FGItemId = FG.ProductId
            LEFT JOIN tblPackSize AS PKS ON FG.PackSizeId = PKS.PackSizeId
            WHERE CS.MaterialConversionMasterId IS NULL AND ISNULL(FinalOutputQuantity,0) > 0  AND CM.MaterialConversionMasterId IS NOT NULL " + param + " ORDER BY CM.MaterialConversionMasterId DESC";

            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public int SaveFullInvoice(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));

            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationFull", aSqlParameterList, "SSIDB");
        }


        public void LoadCompany(DropDownList ddl)
        {
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void GetProductionUnit(DropDownList ddl, string businessUnitId)
        {
            string queryStr = "SELECT ProductionUnitId,ProductionUnitName FROM CSTL_ERPDB..tbl_MC_ProductionUnit WHERE BusinessUnitId = " + businessUnitId;
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ProductionUnitName", "ProductionUnitId", queryStr);
        }
    }
}
