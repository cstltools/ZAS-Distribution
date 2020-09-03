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
    public class SlaveInformationDal
    {

        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public bool SaveSlaveInformation(SlaveInformation aInformation)
        {
            string insertQuery = @"insert into tblSlaveInfo (CompanyId,MaxAmmount,MinAmmount,DisParcentage,ActionStatus,IsActive,ActiveDate,EntryBy,EntryDate) 
            values (" + "'" + aInformation.CompanyId + "','" + aInformation.MaxAmmount + "','" + aInformation.MinAmmount + "','" + aInformation.DisParcentage + "','" + aInformation.ActionStatus + "','" + aInformation.IsActive + "','" + aInformation.ActiveDate + "','" + aInformation.EntryBy + "','" + aInformation.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool UpdateSlaveInformation(SlaveInformation aInformation)
        {
            string queryStr = @"UPDATE tblSlaveInfo SET InActiveDate = '" + aInformation.InActiveDate + "',IsActive = '" + aInformation.IsActive + "',InActiveBy = '" + aInformation.InActiveBy + "',UpdateBy = '" + aInformation.UpdateBy + "',UpdateDate = '" + aInformation.UpdateDate + "' WHERE SlaveId = '" + aInformation.SlaveId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(queryStr, "SSIDB");
        }

        public DataTable GetSlaveInformation()
        {
            const string queryStr = @"SELECT * FROM tblSlaveInfo AS SLVI 
                                      INNER JOIN tblCompanyInfo AS CI ON SLVI.CompanyId = CI.CompanyId WHERE SLVI.IsActive = 'True'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetSlaveInformationById(string slaveId)
        {
            string queryStr = @"SELECT * FROM tblSlaveInfo AS SLVI 
                                      INNER JOIN tblCompanyInfo AS CI ON SLVI.CompanyId = CI.CompanyId WHERE SLVI.SlaveId = '" + slaveId + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetSlaveInformationByCompanyId(string companyId)
        {
            string queryStr = @"SELECT * FROM tblSlaveInfo AS SLVI WHERE SLVI.IsActive = 'True' AND SLVI.CompanyId = '" + companyId + "'";
            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }
    }
}
