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
    public class MiaTargetDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();


        public void LoadGroupInfo(DropDownList groupDropDownList)
        {
            string query = @"SELECT GroupId, GroupName FROM tblGroupInformation";
            aCommonInternalDal.LoadDropDownValue(groupDropDownList, "GroupName", "GroupId", query, "SSIDB");
        }

        public void LoadCompanyInfo(DropDownList ddl, string groupId)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo WHERE GroupId = " + groupId;
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        public void LoadMIOInformation(DropDownList ddl, string companyId)
        {
            string query = @"SELECT MIO.MIOId,TTR.TerritoryCode + ':' + EGI.EmpName AS MIOName FROM dbo.tblMIOInfo AS MIO
                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                            INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId WHERE MIO.IsActive = 1 AND MIO.CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "MIOName", "MIOId", query, "SSIDB");
        }


        public bool SaveMiaTarget(MiaTarget aMiaTarget)
        {

            string insertQuery = @"INSERT INTO dbo.tblMIATarget (MiaTargetId,MiaId,MiaTargetAmount,Period, Year,CompanyId) VALUES ( " + aMiaTarget.MiaTargetId + ",'" + aMiaTarget.MiaId + "','" + aMiaTarget.MiaTargetAmount + "','" + aMiaTarget.Period + "','" + aMiaTarget.Year + "','" + aMiaTarget.CompanyId + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasMiaName(MiaTarget aMiaTarget)
        {
            string query = "select * from tblMIATarget where MiaName = '" + aMiaTarget.MiaName + "'";
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

        public DataTable LoadMiaTargetView()
        {
            string query = @"SELECT MT.MiaTargetId,CI.CompanyName,EGI.EmpMasterCode AS MiaCode, EGI.EmpName AS MiaName,MT.MiaTargetAmount,MT.Period from tblMIATarget AS MT
                             LEFT JOIN dbo.tblMIOInfo AS MIO ON MT.MiaId = MIO.MIOId
                             LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON MT.CompanyId  = CI.CompanyId";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public MiaTarget MiaTargetEditLoad(string MiaTargetId)
        {
            string query = "select * from tblMIATarget where MiaTargetId = '" + MiaTargetId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            MiaTarget aMiaTarget = new MiaTarget();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aMiaTarget.MiaTargetId = Int32.Parse(dataReader["MiaTargetId"].ToString());
                    aMiaTarget.MiaId = Convert.ToInt32(dataReader["MiaId"]);
                    aMiaTarget.CompanyId = Convert.ToInt32(dataReader["CompanyId"]);
                    aMiaTarget.Period = dataReader["Period"].ToString();
                    aMiaTarget.MiaTargetAmount = Convert.ToDecimal(dataReader["MiaTargetAmount"].ToString());
                }
            }
            return aMiaTarget;
        }

        public bool UpdateMiaTarget(MiaTarget aMiaTarget)
        {
            string query = @"UPDATE tblMIATarget SET MiaId='" + aMiaTarget.MiaId + "',MiaTargetAmount='" + aMiaTarget.MiaTargetAmount + "',Period='" + aMiaTarget.Period + "',Year='" + aMiaTarget.Year + "',CompanyId='" + aMiaTarget.CompanyId + "' WHERE MiaTargetId=" + aMiaTarget.MiaTargetId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable LoadMiaInfo(string miaInfoId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblMIAInfo where MiaCode='" + miaInfoId.Trim() + "' ";
            aDataTableEmpInfo = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
            return aDataTableEmpInfo;
        }
    }
}
