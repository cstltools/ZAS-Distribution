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
            string query = @"SELECT MIO.MIOId,EGI.EmpName + ' (' + TTR.TerritoryName + ')' AS MIOName FROM dbo.tblMIOInfo AS MIO
                            INNER JOIN dbo.tblEmpGeneralInfo AS EGI ON  MIO.EmployeeId = EGI.EmpInfoId
                            INNER JOIN dbo.tblTerritory AS TTR ON TTR.TerritoryId = MIO.TerritoryId WHERE MIO.IsActive = 1 AND MIO.CompanyId = " + companyId + " ORDER BY EGI.EmpName";
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
            string query = @"SELECT MT.MiaTargetId,Year,CI.CompanyName,EGI.EmpMasterCode AS MiaCode, EGI.EmpName AS MiaName,MT.MiaTargetAmount,MT.Period from tblMIATarget AS MT
                             LEFT JOIN dbo.tblMIOInfo AS MIO ON MT.MiaId = MIO.MIOId
                             LEFT JOIN dbo.tblEmpGeneralInfo AS EGI ON MIO.EmployeeId = EGI.EmpInfoId
                             LEFT JOIN dbo.tblCompanyInfo AS CI ON MT.CompanyId  = CI.CompanyId 
							 ORDER BY Year DESC,MT.Period,CI.CompanyName DESC";
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

        public DataTable GetActiveProduct(string companyId)
        {
            string query = @"SELECT PD.ProductId,PD.ProductCode,PD.ProductName,PD.PackSize, AVG(UP.UnitPrice) AvgUnitPrice FROM tblProduct AS PD 
                             LEFT JOIN tblUnitPrice AS UP ON PD.ProductId = UP.ProductId
                             WHERE UP.IsActive = 1 AND PD.CompanyId = " + companyId + " GROUP BY PD.ProductId,PD.ProductCode,PD.ProductName,PD.PackSize ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool SaveMiaTargetProductWise(MIOTargetProductWise aMiaTarget)
        {
            string insertQuery = @"INSERT INTO dbo.tblMIATargetProductWise (ProductId,MiaId,TargetQty,Period, Year,CompanyId, TargetAmount) VALUES ( "
                + aMiaTarget.ProductId + ",'" + aMiaTarget.MiaId + "','" + aMiaTarget.TargetQty + "','" + aMiaTarget.Period + "','" + aMiaTarget.Year + "','" + aMiaTarget.CompanyId + "','" + aMiaTarget.TargetAmount + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable GetExistingData(string year, string month,string mioId)
        {
            string query = @"SELECT PDT.ProductId,TargetQty,TargetAmount FROM tblMIATargetProductWise AS PDT
                             WHERE PDT.MiaId = " + mioId + " AND PDT.Period = '" + month + "' AND PDT.Year = '" + year + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool DeleteExistingData(string year, string month, string mioId)
        {
            string query = @"DELETE FROM tblMIATargetProductWise
                             WHERE MiaId = " + mioId + " AND Period = '" + month + "' AND Year = '" + year + "'";

            return aCommonInternalDal.DeleteDataByCommand(query, "SSIDB");
        }
    }
}
