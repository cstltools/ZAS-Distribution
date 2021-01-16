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
    public class ProductUnitPriceDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        ClsApprovalAction approvalAction = new ClsApprovalAction();

        public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        {
            approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        }
        public string LoadForApprovalConditionDAL(string pageName, string userName)
        {
            return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        }
        public bool SaveProductUnitPrice(ProductUnitPrice aProductUnitPrice)
        {
            string insertQuery = @"insert into tblUnitPrice (UnitPriceId,ProductId,ProductCode,ProductName,PackSize,CostPrice,UnitPrice,VATPercentage,VATAmountPerUnit,IsActive,ActiveDate,MRPPrice,ActionStatus,EntryBy,EntryDate) 
            values (" + aProductUnitPrice.UnitPriceId + "," + aProductUnitPrice.ProductId + ",'" + aProductUnitPrice.ProductCode + "','" + aProductUnitPrice.ProductName + "','" + aProductUnitPrice.PackSize + "','" + aProductUnitPrice.CostPrice + "'," +
                                 "'" + aProductUnitPrice.UnitPrice + "','" + aProductUnitPrice.VATPercentage + "','" + aProductUnitPrice.VATAmountPerUnit + "','" + aProductUnitPrice.IsActive + "','" + aProductUnitPrice.ActiveDate + "','" + aProductUnitPrice.MRPPrice + "'," +
                                 "'"+aProductUnitPrice.ActionStatus+"','"+aProductUnitPrice.EntryBy+"','"+aProductUnitPrice.EntryDate+"')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public bool SaveProductUnitPriceUpdate(ProductUnitPrice aProductUnitPrice)
        {
            string insertQuery = @"insert into tblUnitPriceUpdate (UnitPriceUpdateId,UnitPriceId,ProductId,ProductCode,ProductName,PackSize,CostPrice,UnitPrice,VATPercentage,VATAmountPerUnit,ActiveDate) 
            values ('" + aProductUnitPrice.UnitPriceUpdateId + "','" + aProductUnitPrice.UnitPriceId + "'," + aProductUnitPrice.ProductId + ",'" + aProductUnitPrice.ProductCode + "','" + aProductUnitPrice.ProductName + "','" + aProductUnitPrice.PackSize + "','" + aProductUnitPrice.CostPrice + "','" + aProductUnitPrice.UnitPrice + "','" + aProductUnitPrice.VATPercentage + "','" + aProductUnitPrice.VATAmountPerUnit + "','" + aProductUnitPrice.ActiveDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasProductName(ProductUnitPrice aCategory)
        {
            string query = "select * from tblUnitPrice where ProductId = '" + aCategory.ProductId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
//        public DataTable LoadUnitPriceView()
//        {
//            string query = @"SELECT (UnitPrice+VATAmountPerUnit)TPVat ,* from tblUnitPrice 
//                LEFT JOIN dbo.tblProduct ON dbo.tblUnitPrice.ProductId = dbo.tblProduct.ProductId
//                LEFT JOIN dbo.tblProductSQ ON dbo.tblProduct.ProductBrandId = dbo.tblProductSQ.ProductBrandId
//                LEFT JOIN dbo.tblPackSize ON dbo.tblProduct.PackSizeId=dbo.tblPackSize.PackSizeId
//                LEFT JOIN dbo.tblProType ON dbo.tblProType.ProTypeId=dbo.tblProduct.ProTypeId
//                LEFT JOIN dbo.tblProCategory ON dbo.tblProduct.CategoryId = dbo.tblProCategory.CategoryId
//                LEFT JOIN dbo.tblManufacturer ON dbo.tblProduct.ManufacId=dbo.tblManufacturer.ManufacId
//                LEFT JOIN dbo.tblStockUOM ON dbo.tblProduct.StockUOMId=dbo.tblStockUOM.StockUOMId
//                LEFT JOIN dbo.tblProductCase ON dbo.tblProductCase.CaseId=dbo.tblProduct.CaseId where IsActive='1'";
//            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
//        }
        public DataTable LoadUnitPriceView()
        {
            string query = @"SELECT * from tblUnitPrice 
                            INNER JOIN dbo.tblProduct ON dbo.tblUnitPrice.ProductId = dbo.tblProduct.ProductId 
                            INNER JOIN dbo.tblPackSize ON dbo.tblProduct.PackSizeId=dbo.tblPackSize.PackSizeId
                            INNER JOIN tblCompanyInfo AS CI ON dbo.tblProduct.CompanyId = CI.CompanyId
                            where IsActive='1' AND (ActionStatus = 'Accepted' OR ActionStatus = 'Cancel')" ;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetProductPriceReportInfo()
        {
            string query = @"SELECT [ProductCode],[ProductName],[PackSize],[CostPrice],[UnitPrice],[VATPercentage],[VATAmountPerUnit],[IsActive],[ActiveDate],[InActiveDate] from tblUnitPrice WITH(NOLOCK) ORDER BY IsActive DESC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GetProductPriceReportInfo(string parameter)
        {
            string query = @"SELECT Ci.CompanyName,PD.ProductCode,PD.ProductName,PD.PackSize,UnitPrice,MRPPrice, ISNULL(TPVat, 0) AS Vat,IsActive,ActiveDate,InActiveDate from tblUnitPrice WITH(NOLOCK)
                             INNER JOIN dbo.tblProduct AS PD ON PD.ProductId = tblUnitPrice.ProductId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = PD.CompanyId " + parameter + " ORDER BY IsActive DESC";
            
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadProUnitPrice(string ActionStatus)
        {
            string query = @"SELECT * FROM dbo.tblUnitPrice WHERE ActionStatus='" + ActionStatus + "' and IsActive=1   order by tblUnitPrice.UnitPriceId asc ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool ApprovalUpdateDAL(ProductUnitPrice aProductUnitPrice)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@UnitPriceId", aProductUnitPrice.UnitPriceId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aProductUnitPrice.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aProductUnitPrice.ApproveBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aProductUnitPrice.ApproveDate));

            string query = @"UPDATE tblUnitPrice SET ActionStatus=@ActionStatus,ApproveBy=@ApprovedBy,ApproveDate=@ApprovedDate WHERE UnitPriceId=@UnitPriceId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        }

        /// /////////////////////////////////////////////////////////////////////////
        
        public ProductUnitPrice ProductUnitPriceEditLoad(string UnitPriceId)
        {
            string query = "select * from tblUnitPrice where UnitPriceId = '" + UnitPriceId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ProductUnitPrice aCategory = new ProductUnitPrice();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCategory.UnitPriceId = Int32.Parse(dataReader["UnitPriceId"].ToString());
                    aCategory.ProductId = Int32.Parse(dataReader["ProductId"].ToString());
                    aCategory.ProductCode = dataReader["ProductCode"].ToString();
                    aCategory.ProductName = dataReader["ProductName"].ToString();
                    aCategory.VATAmountPerUnit = Convert.ToDecimal(dataReader["VATAmountPerUnit"].ToString());
                    aCategory.VATPercentage = Convert.ToDecimal(dataReader["VATPercentage"].ToString());
                    aCategory.PackSize = dataReader["PackSize"].ToString();
                    aCategory.CostPrice = Convert.ToDecimal(dataReader["CostPrice"].ToString());
                    aCategory.UnitPrice = Convert.ToDecimal(dataReader["UnitPrice"].ToString());
                    aCategory.ActiveDate = Convert.ToDateTime(dataReader["ActiveDate"].ToString());
                    aCategory.MRPPrice = Convert.ToDecimal(dataReader["MRPPrice"].ToString());
                }
            }
            return aCategory;
        }
        public ProductUnitPrice ProductUnitPriceEditLoadProduct(string productId)
        {
            string query = "select * from tblUnitPrice where ProductId = '" + productId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ProductUnitPrice aCategory = new ProductUnitPrice();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCategory.UnitPriceId = Int32.Parse(dataReader["UnitPriceId"].ToString());
                    aCategory.ProductId = Int32.Parse(dataReader["ProductId"].ToString());
                    aCategory.ProductCode = dataReader["ProductCode"].ToString();
                    aCategory.ProductName = dataReader["ProductName"].ToString();
                    aCategory.VATAmountPerUnit = Convert.ToDecimal(dataReader["VATAmountPerUnit"].ToString());
                    aCategory.VATPercentage = Convert.ToDecimal(dataReader["VATPercentage"].ToString());
                    aCategory.PackSize = dataReader["PackSize"].ToString();
                    aCategory.CostPrice = Convert.ToDecimal(dataReader["CostPrice"].ToString());
                    aCategory.UnitPrice = Convert.ToDecimal(dataReader["UnitPrice"].ToString());
                }
            }
            return aCategory;
        }
        
        public bool UpdateCustCategoryInfo(ProductUnitPrice aCategory)
        {
            string query = @"UPDATE tblUnitPrice SET ProductName='" + aCategory.ProductName + "',CostPrice='" + aCategory.CostPrice + "',UnitPrice='" + aCategory.UnitPrice + "',PackSize='" + aCategory.PackSize + "',ProductCode='" + aCategory.ProductCode + "',VATPercentage='" + aCategory.VATPercentage + "',VATAmountPerUnit='"+aCategory.VATAmountPerUnit+"' WHERE UnitPriceId=" + aCategory.UnitPriceId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool UpdateActive(DateTime inactivedate,string unitpriceId,string updateBy,DateTime updatedate)
        {
            string query = @"UPDATE tblUnitPrice SET IsActive='False',InActiveDate='" + inactivedate + "',UpdateBy='" + updateBy + "',UpdateDate='"+updatedate+"' WHERE UnitPriceId=" + unitpriceId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool DeleteProduct(string productId)
        {
            string query = @"DELETE FROM dbo.tblUnitPrice WHERE ProductId='"+productId+"'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public DataTable LoadProduct(string productId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblProduct LEFT JOIN tblPackSize ON dbo.tblProduct.PackSizeId=tblPackSize.PackSizeId where tblProduct.ProductCode='" + productId.Trim() + "' ";
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }

        public DataTable TotalUnitPriceReport()
        {
            string query = @"select * from tblUnitPrice";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetMaterialMasterReport(string parameter)
        {
            string query = @"SELECT CI.CompanyName,PD.ProductCode,PD.ProductName,PD.Description,PD.PackSize,
                            BND.ProductSQName AS Brand,TYP.ProTypeName AS Type,CTG.CategoryName AS Category FROM dbo.tblProduct AS PD 
                            INNER JOIN dbo.tblProductSQ AS BND ON BND.ProductBrandId = PD.ProductBrandId
                            INNER JOIN dbo.tblProType AS TYP ON TYP.ProTypeId = PD.ProTypeId
                            INNER JOIN dbo.tblProCategory AS CTG ON CTG.CategoryId = PD.CategoryId
                            INNER JOIN dbo.tblStockUOM AS UOM ON UOM.StockUOMId = PD.StockUOMId
                            INNER JOIN dbo.tblProductCase AS CS ON CS.CaseId = PD.CaseId
                            INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = PD.CompanyId " + parameter;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadUnitPriceViewInfo(string parameter)
        {
            string query = @"SELECT * from tblUnitPrice 
                            INNER JOIN dbo.tblProduct ON dbo.tblUnitPrice.ProductId = dbo.tblProduct.ProductId 
                            INNER JOIN dbo.tblPackSize ON dbo.tblProduct.PackSizeId=dbo.tblPackSize.PackSizeId
                            INNER JOIN tblCompanyInfo AS CI ON dbo.tblProduct.CompanyId = CI.CompanyId
                            where IsActive='1' AND (ActionStatus = 'Accepted' OR ActionStatus = 'Cancel')" + parameter;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool CreateProductGl(int productId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            return aCommonInternalDal.UpdateAction("sp_I_ProductGl", aSqlParameterlist);
        }
    }
}
