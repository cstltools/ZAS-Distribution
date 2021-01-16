using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerMasterInfoDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void LoadCompanyUnit(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT ComUnitId,ComUnitCode,ComUnitName +':'+ ComUnitCode as ComUnitName FROM dbo.tblCompanyUnit  WHERE ComUnitId IN (SELECT ComUnitId FROM dbo.tblCompanyUnit) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitCode", queryStr);
        }

        public void GetDZSMname(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT RegionId,RegionCode,RegionName +':'+ RegionCode as RegionName FROM dbo.tblRegion  WHERE RegionId IN (SELECT RegionId FROM dbo.tblRegion)";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionCode", queryStr);
        }

        public void LoadCategoryName(RadioButtonList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT CategoryId,CategoryCode,CategoryName FROM dbo.tblCustCategory WHERE CategoryId IN (SELECT CategoryId FROM dbo.tblCustCategory) ";
            aInternalDal.LoadRadioButtonValueWithoutDataBase(ddl, "CategoryName", "CategoryId", queryStr);
        }

        public void GetFEInfo(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT DistrictId,DistrictCode,DistrictName  +':'+ DistrictCode as DistrictName FROM dbo.tblDistrict WHERE DistrictId IN (SELECT DistrictId FROM dbo.tblDistrict) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "DistrictName", "DistrictCode", queryStr);
        }

        public void GetTerritoryInfo(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT AreaId,AreaCode,AreaName +':'+ AreaCode as AreaName FROM dbo.tblArea WHERE AreaId IN (SELECT AreaId FROM dbo.tblArea) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaCode", queryStr);
        }

        public void GetMiaInfo(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT MiaId,MiaCode,MiaName +':'+ MiaCode as MiaName FROM dbo.tblMIAInfo WHERE MiaId IN (SELECT MiaId FROM dbo.tblMIAInfo) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MiaName", "MiaCode", queryStr); ;
        }

        public void GetMaketInfo(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DISTINCT MarketId,MarketCode,MarketName +':'+ MarketCode as MarketName FROM dbo.tblMarket WHERE MarketId IN (SELECT MarketId FROM dbo.tblMarket) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketCode", queryStr);
        }
        public void GetRegion(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT distinct RGN.RegionId, RGN.RegionCode + ' : ' + RGN.RegionName AS RegionName FROM tblRegion AS RGN WHERE RGN.IsActive ='True'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }


        public void GetLedgerHead (DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select GLHeadId, GLName  from ZAS_ACCDB..tblLedgerHead ORDER BY GLName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "GLName", "GLHeadId", queryStr);
        }

        public void LoadControlCode(DropDownList ddl, string LedgerHead)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"	select ControlLId, ControlLName from ZAS_ACCDB..tblControlLedger  with(nolock) 
	WHERE GLHeadId= '" + LedgerHead + "' ORDER BY ControlLName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ControlLName", "ControlLId", queryStr);
        }

        public void LoadSubsidaryCode(DropDownList ddl, string ControlLedger)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"select SubsidiryLId,SubsidiryLName from ZAS_ACCDB..tblSubsidiaryLedger where ControlLId= '" + ControlLedger + "' ORDER BY SubsidiryLName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "SubsidiryLName", "SubsidiryLId", queryStr);
        }

        public void LoadSUBSubsidaryCode(DropDownList ddl, string ControlLedger)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT SubSubsidiaryId, SubSubsidiaryName from ZAS_ACCDB..tblSubsubsidiaryLadger  with(nolock) 
	WHERE SubsidiaryId = '" + ControlLedger + "' ORDER BY SubSubsidiaryName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "SubSubsidiaryName", "SubSubsidiaryId", queryStr);
        }

        public void LoadAccountName(DropDownList ddl, string ControlLedger)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT   AccountName ,  AccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE SubsidiaryLId= '" + ControlLedger + "' ORDER BY AccountName";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AccountName", "AccountId", queryStr);
        }

        public void GetAreaByRegionId(DropDownList ddl, string regionId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT distinct AR.AreaId, AR.AreaCode + ' : ' + AR.AreaName AS AreaName FROM tblArea AS AR 
                                WHERE AR.IsActive ='True' AND AR.RegionId = '" + regionId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", queryStr);
        }
        public void GetTerritiryByAreaId(DropDownList ddl,string areaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT distinct TTR.TerritoryId, TTR.TerritoryCode + ' : ' + TTR.TerritoryName AS TerritoryName FROM tblTerritory AS TTR 
                                WHERE TTR.IsActive ='True' AND TTR.AreaId = '"+ areaId +"'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
        }
        public void GetMarketNameByTerritoryId(DropDownList ddl,string terrId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblMarket WHERE IsActive ='True' AND TerritoryId='" + terrId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }
        public DataTable GetDZSMnameById(string dzsmId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT RegionId,RegionName FROM dbo.tblRegion WHERE RegionCode = '" + dzsmId + "'";
            return aInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetFEnameById(string feId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT DistrictId,DistrictName FROM dbo.tblDistrict WHERE DistrictCode = '" + feId + "'";
            return aInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetTeritorynameById(string teritoryId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT AreaId,AreaName FROM dbo.tblArea WHERE AreaCode = '" + teritoryId + "'";
            return aInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetMiaNameById(string miaId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT MiaId,MiaName FROM dbo.tblMIAInfo WHERE MiaCode = '" + miaId + "'";
            return aInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetMarketNameById(string marketId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT MarketId,MarketName FROM dbo.tblMarket WHERE MarketCode = '" + marketId + "'";
            return aInternalDal.DataContainerDataTable(queryStr);
        }
       
        public bool SaveCustometMasterInfo(CustomerMaster aCustomerMaster)
        {
            string insertQuery = @"INSERT INTO dbo.tblCustMaster
                        ( 
                          CustomerCode ,
                          CategoryId ,
                          AITGLId,
                          CustomerName ,
                          Address ,
                          CellNo ,
                          MarketId,
                          Addrees2 ,
                          City ,
                          ConPerson ,
                          ShippingCond ,
                          
                          TermOfPayment ,
                          
                            CreateBy,
                            CreateDate,
                          FixedCustomer,IsVatApplicable,IsActive
                        )
                VALUES  ( '" + aCustomerMaster.CustomerCode + "' ,  " +
                        "  '" + aCustomerMaster.CategoryId + "' ,  " +
                        "  '" + aCustomerMaster.AITGLId + "' ,  " +
                        "  '" + aCustomerMaster.CustomerName + "' ,  " +
                        "  '" + aCustomerMaster.Address + "' ," +
                        "  '" + aCustomerMaster.CellNo + "' ," +
                        "  '" + aCustomerMaster.MarketId + "' ," +
                        "  '" + aCustomerMaster.Addrees2 + "' ," +
                        "  '" + aCustomerMaster.City + "' , " +
                        "  '" + aCustomerMaster.ConPerson + "' , " +
                        "  '" + aCustomerMaster.ShippingCond + "' ,  " +
                        
                        "  '" + aCustomerMaster.TermOfPayment + "' ,  " +
                        
                        "  '" + aCustomerMaster.CreateBy + "' ,  " +
                        "  '" + aCustomerMaster.CreateDate + "' ,  " +
                        "  '" + aCustomerMaster.FixedCustomer + "' , " +
                        "  '" + aCustomerMaster.IsVatApplicable + "' , " +
                        "  '" + aCustomerMaster.IsActive + "'  " +
                       " )";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool CreateGlOfCustomer(int supplierId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@CustomerId", supplierId));
            aSqlParameterlist.Add(new SqlParameter("@SubsidiaryLId", 36));

            return aCommonInternalDal.UpdateAction("sp_I_CustomerGl", aSqlParameterlist);
        }

        public bool CheckDuplicate(CustomerMaster aCompanyInfo)
        {
            string query = "SELECT * FROM tblCustMaster WHERE CustomerName  = '" + aCompanyInfo.CustomerName + "' AND CustomerMasterId NOT IN (" + aCompanyInfo.CustomerMasterId + ")";
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


        public bool SaveCOAMasterInfo(CoaDAO aCustomerMaster)
        {



            string insertQuery = @"INSERT INTO ZAS_ACCDB..tblChartOfAccounts
                        ( 
                          AccountName ,
                          AccountCode ,
                          SubsidiaryLId ,
                          CustomerId ,
                          CreateBy ,
                          CreateDate,
                          IsActive ,
CompanyId
                        )
                VALUES  ( '" + aCustomerMaster.AccountName + "' ,  " +
                        "  '" + aCustomerMaster.AccountCode + "' ,  " +
                        "  '" + aCustomerMaster.SubsidiaryLId + "' ,  " +
                        "  '" + aCustomerMaster.CustomerId + "' ," +
                        "  '" + aCustomerMaster.CreateBy + "' ," +
                        "  '" + aCustomerMaster.CreateDate + "' ," +
                        "  '" + aCustomerMaster.IsActive + "' , " +
                          "  '" + aCustomerMaster.CompanyId + "'  " +
                       
                       " )";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public CustomerMaster CustomerMasterEditLoad(string cusmasId)
        {
            string query = "select * from tblCustMaster where CustomerMasterId = '" + cusmasId + "'";
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

                    if (dataReader["MarketId"].ToString() != "")
                    {
                        aCustomerMaster.MarketId = Convert.ToInt32(dataReader["MarketId"].ToString());
                    }

                    aCustomerMaster.AITGLId = Convert.ToInt32(dataReader["AITGLId"].ToString());
                    
                    
                    aCustomerMaster.TermOfPayment = dataReader["TermOfPayment"].ToString();
                    aCustomerMaster.IsVatApplicable = Convert.ToBoolean(dataReader["IsVatApplicable"]);
                    //aCustomerMaster.CustomerCodeOld = dataReader["CustomerCodeOld"].ToString();
                    //aCustomerMaster.UploadDate = Convert.ToDateTime(dataReader["UploadDate"].ToString());
                    //aCustomerMaster.FixedCustomer = (bool) dataReader["FixedCustomer"];
                    //aCustomerMaster.ExcelUpload = (bool)dataReader["ExcelUpload"];
                }

            }
            return aCustomerMaster;
        }
        public DataTable LoadCustomerMasterViewParameter(string parameter)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster
            LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblArea.RegionId
            LEFT JOIN dbo.tblCustCategory ON tblCustCategory.CategoryId = tblCustMaster.CategoryId "+parameter+"";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool HasCustomerMastername(CustomerMaster aMaster)
        {
            string query = "select * from tblCustMaster where CustomerName = '" + aMaster.CustomerName + "' OR CustomerCode = '" + aMaster.CustomerCode + "'";
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

        //View

        public DataTable LoadCustomerMasterView(string cust)
        {
            string query = @"SELECT * FROM dbo.View_CustomerMaster" + cust;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadAllMyMarket(string id)
        {
            string query = @"select * FROM dbo.tblMarket
            LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            LEFT JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            LEFT JOIN dbo.tblRegion ON tblRegion.RegionId = tblArea.RegionId
            WHERE MarketId='"+id+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        //Edit

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

        public bool UpdateCustomerMasterInfo(CustomerMaster aCustomerMaster)
        {
            string query = @"UPDATE tblCustMaster SET  CategoryId='" + aCustomerMaster.CategoryId + "' ,  CustomerName='" + aCustomerMaster.CustomerName + "' ,  AITGLId='" + aCustomerMaster.AITGLId + "' ,Address='" + aCustomerMaster.Address + "' " +
                           " ,  CellNo='"+aCustomerMaster.CellNo+"' , MarketId='"+aCustomerMaster.MarketId+"',Addrees2='"+aCustomerMaster.Addrees2+"' , City='"+aCustomerMaster.City+"' ,  ConPerson='"+aCustomerMaster.ConPerson+"' , " +
                           "  ShippingCond='" + aCustomerMaster.ShippingCond + "' ,   TermOfPayment='" + aCustomerMaster.TermOfPayment + "' ,UpdateBy='" + aCustomerMaster.UpdateBy + "' ,IsVatApplicable='" + aCustomerMaster.IsVatApplicable + "',UpdateDate='" + aCustomerMaster.UpdateDate + "'   WHERE CustomerMasterId=" + aCustomerMaster.CustomerMasterId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
            //List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            //aSqlParameterlist.Add(new SqlParameter("@CategoryId", aCustomerMaster.CategoryId));
            //aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aCustomerMaster.CustomerMasterId));
            //aSqlParameterlist.Add(new SqlParameter("@CustomerCode", aCustomerMaster.CustomerCode));
            //aSqlParameterlist.Add(new SqlParameter("@CustomerName", aCustomerMaster.CustomerName));
            //aSqlParameterlist.Add(new SqlParameter("@Address", aCustomerMaster.Address));
            //aSqlParameterlist.Add(new SqlParameter("@CellNo", aCustomerMaster.CellNo));
            //aSqlParameterlist.Add(new SqlParameter("@Addrees2", aCustomerMaster.Addrees2));
            //aSqlParameterlist.Add(new SqlParameter("@City", aCustomerMaster.City));
            //aSqlParameterlist.Add(new SqlParameter("@ShippingCond", aCustomerMaster.ShippingCond));
            //aSqlParameterlist.Add(new SqlParameter("@MarketCode", aCustomerMaster.MarketCode));
            //aSqlParameterlist.Add(new SqlParameter("@MarketName", aCustomerMaster.MarketName));
            //aSqlParameterlist.Add(new SqlParameter("@MIACode", aCustomerMaster.MIACode));
            //aSqlParameterlist.Add(new SqlParameter("@MiaName", aCustomerMaster.MiaName));
            //aSqlParameterlist.Add(new SqlParameter("@AreaCode", aCustomerMaster.AreaCode));
            //aSqlParameterlist.Add(new SqlParameter("@DisCode", aCustomerMaster.DisCode));
            //aSqlParameterlist.Add(new SqlParameter("@FEName", aCustomerMaster.FEName));
            //aSqlParameterlist.Add(new SqlParameter("@ComUnitCode", aCustomerMaster.ComUnitCode));
            //aSqlParameterlist.Add(new SqlParameter("@ComUnitName", aCustomerMaster.ComUnitName));
            //aSqlParameterlist.Add(new SqlParameter("@RegionCode", aCustomerMaster.RegionCode));
            //aSqlParameterlist.Add(new SqlParameter("@DZSMName", aCustomerMaster.DZSMName));
            //aSqlParameterlist.Add(new SqlParameter("@TermOfPayment", aCustomerMaster.TermOfPayment));
            //aSqlParameterlist.Add(new SqlParameter("@FixedCustomer", aCustomerMaster.FixedCustomer));
            //aSqlParameterlist.Add(new SqlParameter("@LoginName", HttpContext.Current.Session["LoginName"].ToString()));

            //return aCommonInternalDal.UpdateAction("UD_CustomerMaster", aSqlParameterlist);
        }

        public DataTable GetCustomerMasterId()
        {
            string query = @"SELECT TOP 1 CustomerCode FROM dbo.tblCustMaster ORDER BY CustomerMasterId DESC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        //API Customer
        public DataTable LoadNewCustomer()
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tbltempCustMaster WHERE AddtoMainCustomer = 'False' ";
            return aInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetMaxCode()
        {
            string query = @"SELECT CONVERT(NVARCHAR(50), MAX(CustomerCode) + 1) AS Code FROM tblCustMaster";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateCusstomerGl(int customerMasterId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@CustomerId", customerMasterId));
            aSqlParameterlist.Add(new SqlParameter("@SubsidiaryLId", 36));

            return aCommonInternalDal.UpdateAction("sp_UD_CustomerGl", aSqlParameterlist);
        }

        public void LoadAIT(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT AccountId,AccountName FROM ZAS_ACCDB..tblChartOfAccounts AS GL WHERE GL.SubsidiaryLId = 52";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AccountName", "AccountId", queryStr);
        }
    }
}
