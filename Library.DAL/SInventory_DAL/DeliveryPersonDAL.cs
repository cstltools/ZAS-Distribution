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
    public class DeliveryPersonDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDeliveryPerson(DeliveryPersonDAO aDeliveryPersonDao)
        {
            string insertQuery = @"INSERT INTO tblDeliveryManInfo (EmpInfoId,Remarks,IsActive,EntryBy,EntryDate) VALUES (" + "'" + aDeliveryPersonDao.EmpInfoId + "','" + aDeliveryPersonDao.Remarks + "','" + aDeliveryPersonDao.IsActive + "','" + aDeliveryPersonDao.EntryBy + "','" + aDeliveryPersonDao.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public DataTable LoadDelivaryMan()
        {
            const string query = @"SELECT * FROM dbo.tblDeliveryManInfo AS DVM
                            INNER JOIN tblEmpGeneralInfo AS EGI ON DVM.EmpInfoId = EGI.EmpInfoId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadDelivaryManById(string delId)
        {
            string query = @"SELECT * FROM dbo.tblDeliveryManInfo AS DVM
                            INNER JOIN tblEmpGeneralInfo AS EGI ON DVM.EmpInfoId = EGI.EmpInfoId
                            INNER JOIN tblCompanyInfo AS CI ON EGI.CompanyId = CI.CompanyId WHERE DVM.DeliveryManId = '" + delId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool UpdateDeliveryInfo(DeliveryPersonDAO aDeliveryPersonDao)
        {
            string query = @"UPDATE tblDeliveryManInfo SET EmpInfoId ='" + aDeliveryPersonDao.EmpInfoId + "',Remarks ='" + aDeliveryPersonDao.Remarks + "',IsActive='" + aDeliveryPersonDao.IsActive + "',UpdateBy='" + aDeliveryPersonDao.UpdateBy + "',UpdateDate='" + aDeliveryPersonDao.UpdateDate + "' WHERE DeliveryManId = " + aDeliveryPersonDao.DeliveryManId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public void EmpLoad(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo WHERE CompanyId = '" + companyId + "' ORDER BY EmpInfoId DESC";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public DataTable GetEmpInfoById(string empId)
        {
            string queryStr = @"SELECT * FROM tblEmpGeneralInfo WHERE EmpInfoId = '" + empId + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetSRInfoById(string empId)
        {
            string queryStr = @"SELECT * FROM tblDeliveryManInfo WHERE EmpInfoId = '" + empId + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr);
        }

        public void EmpLoad(DropDownList ddl)
        {
            string queryStr = @"SELECT EmpInfoId,EmpMasterCode + ' : ' + EmpName AS EmpName FROM tblEmpGeneralInfo WHERE EmpName != 'Vacant'";
            aCommonInternalDal.LoadDropDownValue(ddl, "EmpName", "EmpInfoId", queryStr, "SSIDB");
        }
    }
}
