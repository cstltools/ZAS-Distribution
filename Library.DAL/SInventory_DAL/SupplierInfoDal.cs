using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class SupplierInfoDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();


        public bool SaveDataForCompanyInfo(SupplierInformation aCompanyInfo)
        {
            string insertQuery = @"insert into tblSupplierInformation (SupplierCode,SupplierName,SupplierAddress,ContactNo,Entryby,EntryDate) 
            values ('" + aCompanyInfo.CompanyCode + "','" + aCompanyInfo.SupplierName + "','" + aCompanyInfo.Address + "'," +
                                 "'" + aCompanyInfo.ContactNo + "','" + aCompanyInfo.EntryBy + "','" + aCompanyInfo.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable LoadSupplierInfo()
        {
            string query = @"SELECT * FROM tblSupplierInformation";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool HasCompanyName(SupplierInformation aCompanyInfo)
        {
            string query = "select * from tblSupplierInformation where SupplierName = '" + aCompanyInfo.SupplierName + "'";
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


        public Int32 SaveCompanyInfoData(SupplierInformation aCompanyInfo)
        {
            try
            {
                int supplierId = 0;

                if (!HasCompanyName(aCompanyInfo))
                {
                    ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                    aCompanyInfo.SupplierId = aClsPrimaryKeyFind.PrimaryKeyMax("SupplierId", "tblSupplierInformation");
                    aCompanyInfo.CompanyCode = CompanyCodeGenerator(aCompanyInfo.SupplierId);
                    SaveDataForCompanyInfo(aCompanyInfo);
                    supplierId = aCompanyInfo.SupplierId;
                    return supplierId;
                }
                else
                {
                    return supplierId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        public string CompanyCodeGenerator(int id)
        {
            string code = string.Empty;

            string Id = id.ToString();

            if (Id.Length == 1)
            {
                Id = "00" + Id;
            }

            if (Id.Length == 2)
            {
                Id = "0" + Id;
            }

            code = "SPLR-" + Id;

            return code;
        }

        public DataTable LoadSupplierInfoById(int supplierid)
        {
            string query = @"SELECT * FROM tblSupplierInformation WHERE SupplierId = " + supplierid;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateCompanyInfoData(SupplierInformation aCompanyInfo)
        {
            string query = @"UPDATE tblSupplierInformation SET SupplierName='" + aCompanyInfo.SupplierName + "',Address='" + aCompanyInfo.Address + "',ContactNo='" + aCompanyInfo.ContactNo + "',Updateby='" + aCompanyInfo.UpdateBy + "',UpdateDate='" + aCompanyInfo.UpdateDate + "' WHERE SupplierId=" + aCompanyInfo.SupplierId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public bool CheckDuplicate(SupplierInformation aCompanyInfo)
        {
            string query = "select * from tblSupplierInformation where SupplierName = '" + aCompanyInfo.SupplierName + "' AND SupplierId NOT IN (" + aCompanyInfo.SupplierId + ")";
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

        public bool CreateGlOfSupplier(int supplierId)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@SupplierId", supplierId));
            aSqlParameterlist.Add(new SqlParameter("@SubsidiaryLId", 11));

            return aCommonInternalDal.UpdateAction("sp_I_SupplierGl", aSqlParameterlist);
        }
    }
}
