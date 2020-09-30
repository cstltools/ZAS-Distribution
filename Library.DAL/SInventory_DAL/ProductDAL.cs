using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class ProductDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDataForProduct(Product aProduct)
        {
            string insertQuery = @"insert into tblProduct (CompanyId,ProductCode,ProductName,Description,PackSize,CategoryId,ManufacId,StockUOMId,ProTypeId,ProductBrandId,CaseId,PackSizeId) 
            values ('" + aProduct.CompanyId + "','" + aProduct.ProductCode + "','" + aProduct.ProductName + "','" + aProduct.Description + "'," +
                                 "'" + aProduct.PackSize + "'," + aProduct.CategoryId + ",'" + aProduct.ManufacId + "','" + aProduct.StockUOMId + "','" + aProduct.ProTypeId + "','" + aProduct.ProductBrandId + "','" + aProduct.CaseId + "','" + aProduct.PackSizeId + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasProductName(Product aProduct)
        {
            string query = "select * from tblProduct where ProductCode = '" + aProduct.ProductCode + "'";
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


        public DataTable LoadProduct()
        {
            const string query = @"SELECT * FROM tblProduct AS PDT 
                            INNER JOIN tblCompanyInfo AS CI ON PDT.CompanyId = Ci.CompanyId
                            LEFT JOIN tblProCategory AS PDC ON PDT.CategoryId = PDC.CategoryId
                            LEFT JOIN tblProductSQ AS BND ON PDT.ProductBrandId = BND.ProductBrandId
                            LEFT JOIN tblProType AS PRT ON PDT.ProTypeId = PRT.ProTypeId
                            LEFT JOIN tblManufacturer AS MNF ON PDT.ManufacId = MNF.ManufacId
                            LEFT JOIN tblProductCase AS SCZ ON PDT.CaseId = SCZ.CaseId
                            LEFT JOIN tblStockUOM AS UOM ON PDT.StockUOMId = UOM.StockUOMId
                            LEFT JOIN tblPackSize AS PKZ On PDT.PackSizeId = PKZ.PackSizeId";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public Product ProductEditLoad(string ProductId)
        {
            string query = "select * from tblProduct where ProductId = '" + ProductId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            Product aProduct = new Product();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aProduct.ProductId = Int32.Parse(dataReader["ProductId"].ToString());
                    aProduct.ProductCode = dataReader["ProductCode"].ToString();
                    aProduct.ProductName = dataReader["ProductName"].ToString();
                    aProduct.PackSize = dataReader["PackSize"].ToString();
                    aProduct.Description = dataReader["Description"].ToString();
                    aProduct.CategoryId = Convert.ToInt32(dataReader["CategoryId"].ToString());
                    
                    aProduct.ManufacId = Convert.ToInt32(dataReader["ManufacId"].ToString());
                    aProduct.StockUOMId = Convert.ToInt32(dataReader["StockUOMId"].ToString());
                    aProduct.ProTypeId = Convert.ToInt32(dataReader["ProTypeId"].ToString());
                    aProduct.ProductBrandId = Convert.ToInt32(dataReader["ProductBrandId"].ToString());
                    aProduct.CaseId = Convert.ToInt32(dataReader["CaseId"].ToString());
                    aProduct.PackSizeId = Convert.ToInt32(dataReader["PackSizeId"].ToString());

                    aProduct.CompanyId = Convert.ToInt32(dataReader["CompanyId"].ToString());

                }
            }
            return aProduct;
        }

        public bool UpdateProduct(Product aProduct)
        {
            string query = @"UPDATE tblProduct SET ProductName='" + aProduct.ProductName + "',CompanyId='" + aProduct.CompanyId + "',ProductCode='" + aProduct.ProductCode + "',Description='" + aProduct.Description + "',PackSize='" + aProduct.PackSize + "',CategoryId=" + aProduct.CategoryId + ",ManufacId='" + aProduct.ManufacId + "',StockUOMId='" + aProduct.StockUOMId + "',ProTypeId='" + aProduct.ProTypeId + "',ProductBrandId='" + aProduct.ProductBrandId + "',PackSizeId='" + aProduct.PackSizeId + "' WHERE ProductId=" + aProduct.ProductId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }


        public void LoadCategoryName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblProCategory ORDER BY CategoryName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CategoryName", "CategoryId", queryStr);
        }
        public void LoadManufac(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblManufacturer ORDER BY ManufacName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ManufacName", "ManufacId", queryStr);
        }
        public void LoadPackSize(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblPackSize ORDER BY PackSizeName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "PackSizeName", "PackSizeId", queryStr);
        }
        public void LoadStockUOM(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblStockUOM ORDER BY StockUOMName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "StockUOMName", "StockUOMId", queryStr);
        }
        public void LoadType(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblProType ORDER BY ProTypeName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ProTypeName", "ProTypeId", queryStr);
        }
        public void LoadIngrident(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblIngridents";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "IngridentsName", "IngridentsId", queryStr);
        }
        public void LoadProductSQ(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblProductSQ ORDER BY ProductSQName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ProductSQName", "ProductBrandId", queryStr);
        }
        public void LoadShippingCartonSize(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            const string queryStr = "select * from tblProductCase ORDER BY PcsPerCase";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "PcsPerCase", "CaseId", queryStr);
        }
        public DataTable ProductPriceDetailWithCase(string productCode)
        {
            string query = @"SELECT UP.*,PC.CaseQty,PC.PcsPerCase FROM dbo.tblUnitPrice UP "+
                            " LEFT JOIN dbo.tblProduct P ON UP.ProductCode = P.ProductCode  " +
                             " LEFT JOIN dbo.tblProductCase PC ON p.CaseId = PC.CaseId   " +
                            " WHERE UP.ActionStatus = 'Accepted' AND UP.IsActive = 1 AND UP.ProductCode='" + productCode.Trim() + "' ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public DataTable CodeExistOrNot(string code)
        {
            string query = @"SELECT * FROM tblProduct WHERE tblProduct.ProductCode = '" + code + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable NameExistOrNot(string name)
        {
            string query = @"SELECT * FROM tblProduct WHERE tblProduct.ProductName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
