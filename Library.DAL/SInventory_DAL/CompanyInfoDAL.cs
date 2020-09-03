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
    public class CompanyInfoDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDataForCompanyInfo(CompanyInformation aCompanyInfo)
        {
            string insertQuery = @"insert into tblCompanyInfo (CmpShortName,CompanyCode,GroupId,CompanyName,Address,ContactNo,FaxNo,Remarks,EntryBy,EntryDate) 
            values ('" + aCompanyInfo.CmpShortName + "','" + aCompanyInfo.CompanyCode + "','" + aCompanyInfo.GroupId + "','" + aCompanyInfo.CompanyName + "','" + aCompanyInfo.Address + "'," +
                                 "'" + aCompanyInfo.ContactNo + "','" + aCompanyInfo.FaxNo + "','" + aCompanyInfo.Remarks + "','" + aCompanyInfo.EntryBy + "','" + aCompanyInfo.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasCompanyName(CompanyInformation aCompanyInfo)
        {
            string query = "select * from tblCompanyInfo where CompanyName = '" + aCompanyInfo.CompanyName + "'";
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


        public DataTable LoadCompanyInfo()
        {
            string query = @"SELECT tblCompanyInfo.CompanyId, tblCompanyInfo.CmpShortName,tblCompanyInfo.CompanyName,tblCompanyInfo.Address,tblCompanyInfo.ContactNo,tblCompanyInfo.FaxNo,tblCompanyInfo.Remarks FROM tblCompanyInfo ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public CompanyInformation CompanyInfoEditLoad(string companyInfoId)
        {
            string query = "select * from tblCompanyInfo where CompanyId = '" + companyInfoId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CompanyInformation aCompanyInfo = new CompanyInformation();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCompanyInfo.CompanyId = Int32.Parse(dataReader["CompanyId"].ToString());
                    aCompanyInfo.CmpShortName = dataReader["CmpShortName"].ToString();
                    aCompanyInfo.CompanyCode = dataReader["CompanyCode"].ToString();
                    aCompanyInfo.CompanyName = dataReader["CompanyName"].ToString();
                    aCompanyInfo.ContactNo = dataReader["ContactNo"].ToString();
                    aCompanyInfo.Address = dataReader["Address"].ToString();
                    aCompanyInfo.FaxNo = dataReader["FaxNo"].ToString();
                    aCompanyInfo.Remarks = dataReader["Remarks"].ToString();

                }
            }
            return aCompanyInfo;
        }

        public bool UpdateCompanyInfo(CompanyInformation aCompanyInfo)
        {
            string query = @"UPDATE tblCompanyInfo SET CompanyName='" + aCompanyInfo.CompanyName + "',Address='" + aCompanyInfo.Address + "',GroupId='" + aCompanyInfo.GroupId + "',ContactNo='" + aCompanyInfo.ContactNo + "',FaxNo='" + aCompanyInfo.FaxNo + "',Remarks='" + aCompanyInfo.Remarks + "',UpdateBy='" + aCompanyInfo.UpdateBy + "',UpdateDate='" + aCompanyInfo.UpdateDate + "' WHERE CompanyId=" + aCompanyInfo.CompanyId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public void GetGroupDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT GroupId,GroupName FROM tblGroupInformation";
            aCommonInternalDal.LoadDropDownValue(ddl, "GroupName", "GroupId", queryStr, "SSIDB");
        }

       
        public bool CheckCompanyExistOrNot(string companyCode, string cmpShortName, string companyName)
        {
            bool status = false;

            var aTable = new DataTable();
            string queryStr = @"SELECT * FROM tblCompanyInfo WHERE CompanyCode = '" + companyCode + "' AND CompanyName = '" + cmpShortName + "' AND CmpShortName = '" + companyName + "'";

            aTable = aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");

            if (aTable.Rows.Count > 0)
            {
                status = true;
            }

            return status;
        }

        public DataTable GetCompanyInfo(string companyName)
        {
            string queryStr = @"SELECT * FROM tblCompanyInfo WHERE CompanyName = '" + companyName + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetCompanyShortNameInfo(string cmpShortName)
        {
            string queryStr = @"SELECT * FROM tblCompanyInfo WHERE CmpShortName = '" + cmpShortName + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public bool CompanyCodeExistorNot(string companyCode)
        {
            bool status = false;
            var aTable = new DataTable();

            string queryStr = @"SELECT * FROM tblCompanyInfo WHERE CompanyCode = '" + companyCode + "'";
            aTable = aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");

            if (aTable.Rows.Count > 0)
            {
                status = true;
            }

            return status;

        }
    }
}
