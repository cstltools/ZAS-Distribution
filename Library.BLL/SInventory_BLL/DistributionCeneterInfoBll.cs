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
    public class DistributionCeneterInfoBll
    {
        DistributionCenterInfoDal aDal = new DistributionCenterInfoDal();

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            aDal.GetCompanyDropdownList(ddl);
        }

        public bool SaveSalesCenter(CompanyUnit companyUnit)
        {
            try
            {
                var aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                companyUnit.ComUnitId = aClsPrimaryKeyFind.PrimaryKeyMax("ComUnitId", "tblCompanyUnit");
                companyUnit.ComUnitCode = SalesCodeGenerator(companyUnit.ComUnitId);

                if (!aDal.CheckCodeExistOrNot(companyUnit.ComUnitCode))
                {
                    aDal.SaveDataForSalesCenter(companyUnit);
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



        private string SalesCodeGenerator(int id)
        {
            string code = string.Empty;
            string Id = id.ToString();
            if (Id.Length == 1)
            {
                Id = "0" + Id;
            }
            if (Id.Length == 2)
            {
                Id = "" + Id;
            }
            code = "BD" + Id;
            return code;
        }


        public bool UpdateDataForSalesCenter(CompanyUnit aCompanyUnit)
        {
            return aDal.UpdateSalesCenter(aCompanyUnit);
        }

        public DataTable LoadSalesCenter()
        {
            return aDal.LoadSalesCenter();
        }

        public CompanyUnit SalesCenterEditLoad(string ComUnitId)
        {
            return aDal.SalesCenterEditLoad(ComUnitId);
        }

        public void LoadCompanyName(DropDownList ddl)
        {
            aDal.LoadCompanyName(ddl);
        }

        public void LoadRegionDropdownList(DropDownList ddl)
        {
            aDal.LoadRegionName(ddl);
        }

        public DataTable CheckDCInfoExistOrNot(string dcName)
        {
            return aDal.DCInfoExistOrNot(dcName);
        }

        public DataTable CheckDCShortNameExistOrNot(string shortName)
        {
            return aDal.DCShortNameExistOrNot(shortName);
        }

        public void LoadDcType(DropDownList ddl)
        {
            aDal.LoadDcType(ddl);
        }

        public void LoadCashAccount(DropDownList ddlCashAccount, int companyId)
        {
            aDal.LoadCashAccount(ddlCashAccount, companyId);
        }
    }
}
