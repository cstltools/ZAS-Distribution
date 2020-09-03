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
    public class ProductDiscountDAL
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
        public bool SaveProductDiscount(ProductDiscount aProductDiscount)
        {
            string insertQuery = @"insert into tblProductDiscount (DiscountId,ProductId,CustomerMasterId,DiscountPercentage,Status,ActiveDate,InactiveDate,IsActive,EntryBy,EntryDate,ActionStatus) 
            values (" + aProductDiscount.DiscountId + ",'" + aProductDiscount.ProductId + "','" + aProductDiscount.CustomerMasterId + "','" + aProductDiscount.DiscountPercentage + "'," +
                                 " '" + aProductDiscount.Status + "','" + aProductDiscount.ActiveDate + "','" + aProductDiscount.InactiveDate + "','True','" + aProductDiscount.EntryBy + "','" + aProductDiscount.EntryDate + "','" + aProductDiscount.ActionStatus + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasProductDiscountName(ProductDiscount aProductDiscount)
        {
            string query = "select * from tblProductDiscount where ProductId = '" + aProductDiscount.ProductId + "'";
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

        public DataTable LoadProductDiscount()
        {
            string query = @"SELECT * from tblProductDiscount
            LEFT JOIN dbo.tblCustMaster ON dbo.tblProductDiscount.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            LEFT JOIN dbo.tblProduct ON dbo.tblProductDiscount.ProductId=dbo.tblProduct.ProductId";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool ApprovalUpdateDAL(ProductDiscount aProductDiscount)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@DiscountId", aProductDiscount.DiscountId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aProductDiscount.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aProductDiscount.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aProductDiscount.ApprovedDate));

            string query = @"UPDATE tblProductDiscount SET ActionStatus=@ActionStatus,ApprovedBy=@ApprovedBy,ApprovedDate=@ApprovedDate WHERE DiscountId=@DiscountId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        }
        public DataTable LoadProductDiscount(string actionstatus)
        {
            string query = @"SELECT * from tblProductDiscount
            LEFT JOIN dbo.tblCustMaster ON dbo.tblProductDiscount.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            LEFT JOIN dbo.tblProduct ON dbo.tblProductDiscount.ProductId=dbo.tblProduct.ProductId WHERE ActionStatus='"+actionstatus+"'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadProductDiscount(string fromdate,string todate)
        {
            string query = @"SELECT (MIACode+'-'+MIAName)MIAName,* from tblProductDiscount
            LEFT JOIN dbo.tblCustMaster ON dbo.tblProductDiscount.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            LEFT JOIN dbo.tblProduct ON dbo.tblProductDiscount.ProductCode=dbo.tblProduct.ProductCode WHERE ActiveDate BETWEEN '" + fromdate + "' AND '" + todate + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetCustomerInfo(string custId)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster WHERE CustomerMasterId='" + custId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetProduct(string productId)
        {
            string query = @"SELECT * FROM dbo.tblProduct WHERE ProductId='"+productId+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetProductDiscount()
        {
            string query = @"SELECT * FROM dbo.tblProductDiscount
            LEFT JOIN dbo.tblProduct ON tblProduct.ProductId = tblProductDiscount.ProductId
            LEFT JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblProductDiscount.CustomerMasterId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetProductDiscountApproval(string actionstatus)
        {
            string query = @"SELECT * FROM dbo.tblProductDiscount
            LEFT JOIN dbo.tblProduct ON tblProduct.ProductId = tblProductDiscount.ProductId
            LEFT JOIN dbo.tblCustMaster ON dbo.tblCustMaster.CustomerMasterId=dbo.tblProductDiscount.CustomerMasterId
            WHERE ActionStatus='"+actionstatus+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public void LoadCustomerMaster(DropDownList ddl,string marketId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCustMaster WHERE CustomerMasterId IN (SELECT DISTINCT CustomerMasterId FROM dbo.View_CustomerMaster WHERE MarketId='"+marketId+"')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", queryStr);
        }
        public void LoadSalesCenter(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        public void LoadArea(DropDownList ddl,string comUnitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblArea WHERE AreaId IN (SELECT AreaId FROM dbo.View_CustomerMaster WHERE ComUnitId='"+comUnitId+"')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }
        public void LoadMarket(DropDownList ddl, string areaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblMarket WHERE MarketId IN (SELECT DISTINCT MarketId FROM dbo.View_CustomerMaster WHERE AreaId='"+areaId+"')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }
        public void DCLoad(DropDownList aDownList, string companyId)
        {
            string dc = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit WHERE CompanyId='" + companyId + "'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "Com", "ComUnitId", dc, "SSIDB");
        }
        public void AreaLoad(DropDownList aDownList, string dcId)
        {
            string dc = @"SELECT * FROM dbo.tblDcWiseAreaInfo
            INNER JOIN dbo.tblArea ON tblArea.AreaId = tblDcWiseAreaInfo.AreaId
            WHERE DCId='" + dcId + "' AND tblDcWiseAreaInfo.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "AreaName", "AreaId", dc, "SSIDB");
        }
        public void MIOLoad(DropDownList aDownList, string areaId)
        {
            string dc = @"SELECT DISTINCT (EmpName+'('+EmpMasterCode+')')Name,EmpInfoId FROM dbo.tblMIOInfo
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMIOInfo.TerritoryId
            LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
            WHERE AreaId='" + areaId + "' AND tblMIOInfo.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "Name", "EmpInfoId", dc, "SSIDB");
        }
        public void TerritoryLoad(DropDownList aDownList, string mioId)
        {
            string dc = @"
            SELECT * FROM dbo.tblTerritory
            INNER JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId
            WHERE EmployeeId='" + mioId + "' AND tblTerritory.IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "TerritoryName", "TerritoryId", dc, "SSIDB");
        }
        public void LoadmanufacturerName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }
        public void MarketLoad(DropDownList aDownList, string territoryId)
        {
            string dc = @"SELECT * FROM dbo.tblMarket
            WHERE TerritoryId='" + territoryId + "' AND IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "MarketName", "MarketId", dc, "SSIDB");
        }
        public void MIOLoad(DropDownList aDownList)
        {
            string dc = @"SELECT * FROM dbo.tblMIOInfo WITH (NOLOCK )
	   LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblEmpGeneralInfo.EmpInfoId=dbo.tblMIOInfo.EmployeeId WHERE IsActive='1'";
            aCommonInternalDal.LoadDropDownValue(aDownList, "EmpName", "MIOId", dc, "SSIDB");
        }

        public ProductDiscount ProductDiscountEditLoad(string ID)
        {
            string query = "select * from tblProductDiscount where DiscountId = '" + ID + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            ProductDiscount aProductDiscount = new ProductDiscount();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aProductDiscount.DiscountId = Int32.Parse(dataReader["DiscountId"].ToString());
                    aProductDiscount.ProductId = Convert.ToInt32(dataReader["ProductId"].ToString());
                    aProductDiscount.CustomerMasterId = Convert.ToInt32(dataReader["CustomerMasterId"].ToString());
                    aProductDiscount.DiscountPercentage = Convert.ToDecimal(dataReader["DiscountPercentage"].ToString());
                    aProductDiscount.Status = dataReader["Status"].ToString();
                    aProductDiscount.ActiveDate = Convert.ToDateTime(dataReader["ActiveDate"].ToString());
                    aProductDiscount.InactiveDate = Convert.ToDateTime(dataReader["InactiveDate"].ToString());
                }
            }
            return aProductDiscount;
        }

        public bool UpdateProductDiscountInfo(ProductDiscount aProductDiscount)
        {
            string query = @"UPDATE tblProductDiscount SET ProductId='" + aProductDiscount.ProductId + "',CustomerMasterId='" + aProductDiscount.CustomerMasterId + "',DiscountPercentage='" + aProductDiscount.DiscountPercentage + "',ActiveDate='" + aProductDiscount.ActiveDate + "',InactiveDate='" + aProductDiscount.InactiveDate + "',UpdateBy='" + aProductDiscount.UpdateBy + "',UpdateDate='" + aProductDiscount.UpdateDate + "' WHERE DiscountId=" + aProductDiscount.DiscountId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
