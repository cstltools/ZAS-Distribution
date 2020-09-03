using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerMasterDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveCustometMasterInfo(CustomerMaster aCustomerMaster)
        {
            string insertQuery = @"INSERT INTO dbo.tblCustMaster
                        ( 
                          CustomerCode ,
                          CategoryId ,
                          CustomerName ,
                          Address ,
                          CellNo ,
                          Addrees2 ,
                          City ,
                          ConPerson ,
                          ShippingCond ,
                          MarketCode ,
                          MarketName ,
                          MIACode ,
                          MIAName ,
                          AreaCode ,
                          DisCode ,
                          FEName ,
                          ComUnitCode ,
                          ComUnitName ,
                          RegionCode ,
                          DZSMName ,
                          TermOfPayment ,
                          CustomerCodeOld
                        )
                VALUES  ( '" + aCustomerMaster.CustomerCode + "' ,  " +
                        "  '" + aCustomerMaster.CategoryId + "' ,  " +
                        "  '" + aCustomerMaster.CustomerName + "' ,  " +
                        "  '" + aCustomerMaster.Address + "' ," +
                        "  '" + aCustomerMaster.CellNo + "' ," +
                        "  '" + aCustomerMaster.Addrees2 + "' ," +
                        "  '" + aCustomerMaster.City + "' , " +
                        "  '" + aCustomerMaster.ConPerson + "' , " +
                        "  '" + aCustomerMaster.ShippingCond + "' ,  " +
                        "  '" + aCustomerMaster.MarketCode + "' , " +
                        "  '" + aCustomerMaster.MarketName + "' ,  " +
                        "  '" + aCustomerMaster.MIACode + "' , " +
                        "  '" + aCustomerMaster.MiaName + "' ,  " +
                        "  '" + aCustomerMaster.AreaCode + "' , " +
                        " '" + aCustomerMaster.DisCode + "' ,  " +
                        "  '" + aCustomerMaster.FEName + "' , " +
                        "  '" + aCustomerMaster.ComUnitCode + "' ," +
                        "  '" + aCustomerMaster.ComUnitName + "' ,  " +
                        "  '" + aCustomerMaster.RegionCode + "' , " +
                        "  '" + aCustomerMaster.DZSMName + "' , " +
                        "  '" + aCustomerMaster.TermOfPayment + "' ,  " +
                        "  '" + aCustomerMaster.CustomerCodeOld + "'  " +
                       " )";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasCustomerMastername(CustomerMaster aMaster)
        {
            string query = "select * from tblCustMaster where CustomerCode = '" + aMaster.CustomerCode + "'";
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
       
        //public bool DeleteRequisition(int OrdID)
        //{
        //    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //    aSqlParameterlist.Add(new SqlParameter("@OrdID", OrdID));

        //    return aCommonInternalDal.DeleteAction("sp_DeleteOrder", aSqlParameterlist);
        //}
        public DataTable LoadCustomerMasterView(string parameter)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblArea.RegionId
            LEFT JOIN dbo.tblCustCategory ON tblCustCategory.CategoryId = tblCustMaster.CategoryId " + parameter;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable FixedCustomerMasterReport(bool fixedBusiness)
        {
            string query = @"SELECT (DistrictCode+'-'+DistrictName)DistrictName,(RegionCode+'-'+RegionName)RegionName, (AreaCode+'-'+AreaName)AreaName,(MiaCode+'-'+MiaName)MiaName,(MarketCode+'-'+MarketName)MarketName,* FROM dbo.View_CustomerMaster WHERE FixedCustomer = 'true'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable RegularCustomerMasterReport(bool regularCustomer)
        {
            string query = @"SELECT (DistrictCode+'-'+DistrictName)DistrictName,(RegionCode+'-'+RegionName)RegionName, (AreaCode+'-'+AreaName)AreaName,(MiaCode+'-'+MiaName)MiaName,(MarketCode+'-'+MarketName)MarketName,* FROM dbo.View_CustomerMaster WHERE FixedCustomer = 'false'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadOrderView(string Order)
        {
            string query = @"select * from tblOrder where  IsInvoice=0 and OrderCode='" + Order + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public CustomerMaster CustomerMasterEditLoad(string cusmasId)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblArea.RegionId
            LEFT JOIN dbo.tblCustCategory ON tblCustCategory.CategoryId = tblCustMaster.CategoryId where CustomerMasterId = '" + cusmasId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CustomerMaster aCustomerMaster = new CustomerMaster();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {

                    aCustomerMaster.CustomerMasterId = Int32.Parse(dataReader["CustomerMasterId"].ToString());
                    aCustomerMaster.CustomerName = dataReader["CustomerName"].ToString();
                    aCustomerMaster.CustomerCode = dataReader["CustomerCode"].ToString();
                    aCustomerMaster.Address = dataReader["Address"].ToString();
                    aCustomerMaster.CellNo = dataReader["CellNo"].ToString();
                    aCustomerMaster.CategoryId = Convert.ToInt32(dataReader["CategoryId"].ToString());
                    aCustomerMaster.Addrees2 = dataReader["Addrees2"].ToString();
                    aCustomerMaster.City = dataReader["City"].ToString();
                    aCustomerMaster.ConPerson = dataReader["ConPerson"].ToString();
                    aCustomerMaster.ShippingCond = dataReader["ShippingCond"].ToString();
                    aCustomerMaster.MarketCode = dataReader["MarketCode"].ToString();
                    aCustomerMaster.MarketName = dataReader["MarketName"].ToString();
                    //aCustomerMaster.MIACode = dataReader["MIACode"].ToString();
                    //aCustomerMaster.MiaName = dataReader["MIAName"].ToString();
                    aCustomerMaster.AreaCode = dataReader["AreaCode"].ToString();
                    aCustomerMaster.DisCode = dataReader["DisCode"].ToString();
                    aCustomerMaster.FEName = dataReader["FEName"].ToString();
                    aCustomerMaster.ComUnitCode = dataReader["ComUnitCode"].ToString();
                    aCustomerMaster.ComUnitName = dataReader["ComUnitName"].ToString();
                    aCustomerMaster.RegionCode = dataReader["RegionCode"].ToString();
                    aCustomerMaster.DZSMName = dataReader["DZSMName"].ToString();
                    aCustomerMaster.TermOfPayment = dataReader["TermOfPayment"].ToString();
                    aCustomerMaster.CustomerCodeOld = dataReader["CustomerCodeOld"].ToString();
                    
                }

            }
            return aCustomerMaster;
        }

        public bool UpdateCustomerMasterInfo(CustomerMaster aCustomerMaster)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@CategoryId", aCustomerMaster.CategoryId));
            aSqlParameterlist.Add(new SqlParameter("@CustomerName", aCustomerMaster.CustomerName));
            aSqlParameterlist.Add(new SqlParameter("@CustomerCode", aCustomerMaster.CustomerCode));
            aSqlParameterlist.Add(new SqlParameter("@Address", aCustomerMaster.Address));
            aSqlParameterlist.Add(new SqlParameter("@CellNo", aCustomerMaster.CellNo));
            aSqlParameterlist.Add(new SqlParameter("@Addrees2", aCustomerMaster.Addrees2));
            aSqlParameterlist.Add(new SqlParameter("@City", aCustomerMaster.City));
            aSqlParameterlist.Add(new SqlParameter("@ShippingCond", aCustomerMaster.ShippingCond));
            aSqlParameterlist.Add(new SqlParameter("@MarketCode", aCustomerMaster.MarketCode));
            aSqlParameterlist.Add(new SqlParameter("@MarketName", aCustomerMaster.MarketName));
            aSqlParameterlist.Add(new SqlParameter("@MIACode", aCustomerMaster.MIACode));
            aSqlParameterlist.Add(new SqlParameter("@MiaName", aCustomerMaster.MiaName));
            aSqlParameterlist.Add(new SqlParameter("@AreaCode", aCustomerMaster.AreaCode));
            aSqlParameterlist.Add(new SqlParameter("@DisCode", aCustomerMaster.DisCode));
            aSqlParameterlist.Add(new SqlParameter("@FEName", aCustomerMaster.FEName));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitCode", aCustomerMaster.ComUnitCode));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitName", aCustomerMaster.ComUnitName));
            aSqlParameterlist.Add(new SqlParameter("@RegionCode", aCustomerMaster.RegionCode));
            aSqlParameterlist.Add(new SqlParameter("@DZSMName", aCustomerMaster.DZSMName));
            aSqlParameterlist.Add(new SqlParameter("@TermOfPayment", aCustomerMaster.TermOfPayment));
            aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aCustomerMaster.CustomerMasterId));
            aSqlParameterlist.Add(new SqlParameter("@LoginName", HttpContext.Current.Session["LoginName"].ToString()));

            return aCommonInternalDal.UpdateAction("UD_CustomerMaster2", aSqlParameterlist);
        }
        public void LoadMarketName(DropDownList ddl,string miaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT MarketId,MarketCode,MarketName +':'+ MarketCode as MarketName FROM dbo.tblMarket  WHERE MarketId IN (SELECT MarketId FROM dbo.tblMarket) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketCode", queryStr);
        }

        public void LoadAreaName(DropDownList ddl,string districtId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT AreaId,AreaCode,AreaName +':'+ AreaCode as AreaName FROM dbo.tblArea  WHERE AreaId IN (SELECT AreaId FROM dbo.tblArea) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaCode", queryStr);
        }
        public void LoadCompanyUnit(DropDownList ddl,string regionid)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT ComUnitId,ComUnitCode,ComUnitName +':'+ ComUnitCode as ComUnitName FROM dbo.tblCompanyUnit  WHERE ComUnitId IN (SELECT ComUnitId FROM dbo.tblCompanyUnit) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitCode", queryStr);
        }
        public void LoadCompanyUnitbyID(DropDownList ddl, string regionid)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT ComUnitId,ComUnitCode,ComUnitName +':'+ ComUnitCode as ComUnitName FROM dbo.tblCompanyUnit  WHERE ComUnitId IN (SELECT ComUnitId FROM dbo.tblCompanyUnit) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        
        public void LoadDistrictName(DropDownList ddl,string comUnitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT DistrictId,DistrictCode,DistrictName  +':'+ DistrictCode as DistrictName FROM dbo.tblDistrict  WHERE DistrictId IN (SELECT DistrictId FROM dbo.tblDistrict) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "DistrictName", "DistrictCode", queryStr);
        }
        public void LoadRegionname(DropDownList ddl, string id)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblRegion where CompanyId='"+id+"'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }
        public void LoadRegionname(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT RegionId,RegionCode,RegionName FROM dbo.tblRegion  WHERE RegionId IN (SELECT RegionId FROM dbo.tblRegion)";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionCode", queryStr);
        }
        public void LoadCategoryName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT CategoryId,CategoryCode,CategoryName FROM dbo.tblCustCategory  WHERE CategoryId IN (SELECT CategoryId FROM dbo.tblCustCategory) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CategoryName", "CategoryId", queryStr);
        }
        public void LoadMiaName(DropDownList ddl,string areaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT MiaId,MiaCode,MiaName FROM dbo.tblMIAInfo  WHERE MiaId IN (SELECT MiaId FROM dbo.tblMIAInfo) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MiaName", "MiaCode", queryStr);
        }
        public void LoadDcDropDownList(DropDownList ddl,string custMasterId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyUnit where ComUnitId='" + custMasterId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void LoadDcDropDownList(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        /// <summary>
        /// ////////
        /// </summary>
        /// <param name="comUnitId"></param>
        /// <returns></returns>
        public void LoadMarketNameById(DropDownList ddl, string miaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT MarketId,MarketCode,MarketName FROM dbo.View_CustomerMaster  WHERE MarketId IN (SELECT MarketId FROM dbo.tblMarket) AND dbo.View_CustomerMaster.MiaId='" + miaId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }

        public void LoadAreaNameById(DropDownList ddl, string districtId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT AreaId,AreaCode,AreaName FROM dbo.View_CustomerMaster  WHERE AreaId IN (SELECT AreaId FROM dbo.tblArea) AND dbo.View_CustomerMaster.DistrictId='" + districtId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }
        public void LoadCompanyUnitById(DropDownList ddl, string regionid)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT ComUnitId,ComUnitCode,ComUnitName FROM dbo.View_CustomerMaster  WHERE ComUnitId IN (SELECT ComUnitId FROM dbo.tblCompanyUnit) AND dbo.View_CustomerMaster.RegionId='" + regionid + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void LoadDistrictNameById(DropDownList ddl, string comUnitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT DistrictId,DistrictCode,DistrictName FROM dbo.View_CustomerMaster  WHERE DistrictId IN (SELECT DistrictId FROM dbo.tblDistrict) AND dbo.View_CustomerMaster.ComUnitId='" + comUnitId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "DistrictName", "DistrictId", queryStr);
        }
        public void LoadRegionnameById(DropDownList ddl, string id)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblRegion where CompanyId='" + id + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }
        public void LoadRegionnameById(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT RegionId,RegionCode,RegionName FROM dbo.View_CustomerMaster  WHERE RegionId IN (SELECT RegionId FROM dbo.tblRegion)";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }
        public void LoadCategoryNameById(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT CategoryId,CategoryCode,CategoryName FROM dbo.View_CustomerMaster  WHERE CategoryId IN (SELECT CategoryId FROM dbo.tblCustCategory) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CategoryName", "CategoryId", queryStr);
        }
        public void LoadMiaNameById(DropDownList ddl, string areaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT MiaId,MiaCode,MiaName FROM dbo.View_CustomerMaster  WHERE MiaId IN (SELECT MiaId FROM dbo.tblMIAInfo) AND dbo.View_CustomerMaster.AreaId='" + areaId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MiaName", "MiaId", queryStr);
        }
        public void LoadDcDropDownListById(DropDownList ddl, string custMasterId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyUnit where ComUnitId='" + custMasterId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void LoadDcDropDownListById(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public DataTable CustomerMasterReport(string comUnitId)
        {
            string query = @"SELECT (DistrictCode+'-'+DistrictName)DistrictName,(RegionCode+'-'+RegionName)RegionName, (AreaCode+'-'+AreaName)AreaName,(MiaCode+'-'+MiaName)MiaName,(MarketCode+'-'+MarketName)MarketName,* FROM dbo.View_CustomerMaster WHERE ComUnitId='" + comUnitId.Trim() + "' ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable CustomerMasterReport()
        {
            string query = @"SELECT * FROM dbo.tblCustMaster
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblArea.RegionId
            LEFT JOIN dbo.tblCustCategory ON tblCustCategory.CategoryId = tblCustMaster.CategoryId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable Customer(string customerId)
        {
            string query = @"SELECT * FROM dbo.tblInvoice WHERE TpTotal>0 and DeliveryInvoiceStatus IS NULL AND CustomerMasterId='" + customerId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable CustomerPayment(string customerId)
        {
            string query = @"SELECT * FROM dbo.tblInvoice WHERE (PaymentStatus  IS NULL OR PaymentStatus= 'Partial')  AND DeliveryTpTotal>0  AND CustomerMasterId='" + customerId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable Customerorder(string customerCode)
        {
            string query = @"SELECT * FROM dbo.tblOrder WHERE IsInvoice = 0 AND CustomerCode ='" + customerCode + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool DeleteRequisition(int OrdID)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@OrdID", OrdID));
            aSqlParameterlist.Add(new SqlParameter("@LoginName", HttpContext.Current.Session["LoginName"].ToString()));

            return aCommonInternalDal.DeleteAction("sp_DeleteOrder", aSqlParameterlist);
        }

        public void LoadRegionName(DropDownList ddl)
        {
            const string queryStr = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive ='True'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }

        public void GetAreaOnDropdownList(DropDownList ddl, string regionId)
        {
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                WHERE AR.IsActive ='True' AND AR.RegionId = '" + regionId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }

        public void GetTerritoryOnDropdownList(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT distinct TTR.TerritoryId, TTR.TerritoryCode + ' : ' + TTR.TerritoryName AS TerritoryName FROM tblTerritory AS TTR 
                                WHERE TTR.IsActive ='True' AND TTR.AreaId = '" + areaId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
        }

        public void GetMarketOnDropdownList(DropDownList ddl, string ttrId)
        {
            string queryStr = @"SELECT distinct MKT.MarketId, MKT.MarketCode + ' : ' + MKT.MarketName AS MarketName FROM tblMarket AS MKT 
                                WHERE MKT.IsActive ='True' AND MKT.TerritoryId = '" + ttrId + "'";

            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }
    }
}
