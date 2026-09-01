using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class CustomerMasterInfoBll
    {
        CustomerMasterInfoDal aCustomerMasterDAL = new CustomerMasterInfoDal();

        public void LoadCategoryName(RadioButtonList ddl)
        {
            aCustomerMasterDAL.LoadCategoryName(ddl);
        }

        public void LoadDistributionCenterName(DropDownList ddl)
        {
            aCustomerMasterDAL.LoadCompanyUnit(ddl);
        }

        public void LoadDZSMInfo(DropDownList ddl)
        {
            aCustomerMasterDAL.GetDZSMname(ddl);
        }

        public void LoadFEInfo(DropDownList ddl)
        {
            aCustomerMasterDAL.GetFEInfo(ddl);
        }

        public void LoadTerritoryInfo(DropDownList ddl)
        {
            aCustomerMasterDAL.GetTerritoryInfo(ddl);
        }
        public void GetRegion(DropDownList ddl)
        {
            aCustomerMasterDAL.GetRegion(ddl);
        }

        public void GetLedgerHead(DropDownList ddl)
        {
            aCustomerMasterDAL.GetLedgerHead(ddl);
        }

        public void LoadControlCode(DropDownList ddl, string LedgerHead)
        {
            aCustomerMasterDAL.LoadControlCode(ddl, LedgerHead);
        }

        public void LoadSubsidaryCode(DropDownList ddl, string ControlLedger)
        {
            aCustomerMasterDAL.LoadSubsidaryCode(ddl, ControlLedger);
        }

        public void LoadSUBSubsidaryCode(DropDownList ddl, string ControlLedger)
        {
            aCustomerMasterDAL.LoadSUBSubsidaryCode(ddl, ControlLedger);
        }

        public void LoadAccountName(DropDownList ddl, string ControlLedger)
        {
            aCustomerMasterDAL.LoadAccountName(ddl, ControlLedger);
        }

        public void GetAreaByRegionId(DropDownList ddl, string regionId)
        {
            aCustomerMasterDAL.GetAreaByRegionId(ddl,regionId);
        }
        public void GetTerritiryByAreaId(DropDownList ddl, string areaId)
        {
            aCustomerMasterDAL.GetTerritiryByAreaId(ddl,areaId);
        }
        public void GetMarketNameByTerritoryId(DropDownList ddl, string terrId)
        {
            aCustomerMasterDAL.GetMarketNameByTerritoryId(ddl,terrId);
        }
        public void LoadMiaInfo(DropDownList ddl)
        {
            aCustomerMasterDAL.GetMiaInfo(ddl);
        }

        public void LoadMaketInfo(DropDownList ddl)
        {
            aCustomerMasterDAL.GetMaketInfo(ddl);
        }

        public DataTable LoadDZSMName(string dzsmId)
        {
            return  aCustomerMasterDAL.GetDZSMnameById(dzsmId);
        }

        public DataTable LoadFEName(string feId)
        {
            return aCustomerMasterDAL.GetFEnameById(feId);
        }

        public DataTable LoadTeritoryName(string teritoryId)
        {
            return aCustomerMasterDAL.GetTeritorynameById(teritoryId);
        }

        public DataTable LoadMiaName(string miaId)
        {
            return aCustomerMasterDAL.GetMiaNameById(miaId);
        }

        public DataTable LoadMarketName(string marketId)
        {
            return aCustomerMasterDAL.GetMarketNameById(marketId);
        }
        private string CustCodeGenerator(int id)
        {
            string code = string.Empty;

           DataTable aTable = aCustomerMasterDAL.GetMaxCode();

            //string Id = id.ToString();
           
            //if (Id.Length == 1)
            //{
            //    Id = "1000" + Id;
            //}
            //if (Id.Length == 2)
            //{
            //    Id = "100" + Id;
            //}
            //if (Id.Length == 3)
            //{
            //    Id = "10" + Id;
            //}
            //if (Id.Length == 4)
            //{
            //    Id = "1" + Id;
            //}

            if (aTable.Rows.Count > 0)
            {
                if (aTable.Rows[0].Field<string>("Code") != "")
                {
                    code = aTable.Rows[0].Field<string>("Code");
                }

            }
            else
            {
                code = "CUST-00001";
            }

            
            return code;
        }
        public bool SaveCustMasterInfo(CustomerMaster CustomerMaster)
        {
            try
            {
                if (!aCustomerMasterDAL.HasCustomerMastername(CustomerMaster))
                {
                    ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                    CustomerMaster.CustomerMasterId = aClsPrimaryKeyFind.PrimaryKeyMax("CustomerMasterId", "tblCustMaster");
                    CustomerMaster.CustomerCode = CustCodeGenerator(CustomerMaster.CustomerMasterId);
                    aCustomerMasterDAL.SaveCustometMasterInfo(CustomerMaster);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }



        public bool SaveCoaMasterInfo(int customerId)
        {
            try
            {

                aCustomerMasterDAL.CreateGlOfCustomer(customerId);
                    return true;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        public DataTable LoadAllMyMarket(string id)
        {
            return aCustomerMasterDAL.LoadAllMyMarket(id);
        }

        public CustomerMaster CustomerMasterEditLoad(string CustomerMasterId)
        {
            return aCustomerMasterDAL.CustomerMasterEditLoad(CustomerMasterId);
        }

        public DataTable LoadCustomerMaster(string cust)
        {
            return aCustomerMasterDAL.LoadCustomerMasterView(cust);
        }

        public DataTable LoadCustomerMasterViewParameter(string parameter)
        {
            return aCustomerMasterDAL.LoadCustomerMasterViewParameter(parameter);
        }

        public DataTable Customer(string customerId)
        {
            return aCustomerMasterDAL.Customer(customerId);
        }

        public DataTable CustomerPayment(string customerId)
        {
            return aCustomerMasterDAL.CustomerPayment(customerId);
        }

        public bool UpdateDataForCustomerMaster(CustomerMaster aCustomerMaster)
        {
            //if (!aCustomerMasterDAL.HasCustomerMastername(aCustomerMaster   ))
            {
                return aCustomerMasterDAL.UpdateCustomerMasterInfo(aCustomerMaster);
            }
            //else
            //{
            //    return false;
            //}


        }

        public DataTable LoadCustomerMasterId()
        {
            return aCustomerMasterDAL.GetCustomerMasterId();
        }

        //API Customer
        public DataTable LoadNewCustomer()
        {
           return aCustomerMasterDAL.LoadNewCustomer();
        }

        public bool UpdateCusstomerGl(int customerMasterId)
        {
            return aCustomerMasterDAL.UpdateCusstomerGl(customerMasterId)
            ;
        }

        public void LoadAIT(DropDownList ddl)
        {
            aCustomerMasterDAL.LoadAIT(ddl);
        }
    }
}
