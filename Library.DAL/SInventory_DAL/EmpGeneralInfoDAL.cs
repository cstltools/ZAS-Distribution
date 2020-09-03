using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class EmpGeneralInfoDAL
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveEmployeeInfo(EmpGeneralInfo aGeneralInfo)
        {
            string insertQuery = @"insert into tblEmpGeneralInfo (EmpMasterCode,EmpName,ShortName,FatherName,MotherName,Religion,Nationality,DateOfBirth,PlaceOfBirth,BloodGroup,Gender,AddressPresent,AddressPermanent,PreCity,
                                   PerCity,CellNumber,MaritalStatus,NationalIdNo,JoiningDate,DesignationId,DepartmentId,CompanyId,Email,EmrgContactNo,RefName,RefContactNo,EntryBy,EntryDate) 
            values (" + "'" + aGeneralInfo.EmpMasterCode + "','" + aGeneralInfo.EmpName + "','" + aGeneralInfo.ShortName + "','" + aGeneralInfo.FatherName + "','" + aGeneralInfo.MotherName + "','" + aGeneralInfo.Religion + "','" + aGeneralInfo.Nationality + "','" + aGeneralInfo.DateOfBirth + "','" + aGeneralInfo.PlaceOfBirth + "'," +
                                  "'" + aGeneralInfo.BloodGroup + "','" + aGeneralInfo.Gender + "','" + aGeneralInfo.AddressPresent + "','" + aGeneralInfo.AddressPermanent + "','" + aGeneralInfo.PreCity + "','" + aGeneralInfo.PerCity + "','" + aGeneralInfo.CellNumber 
                                   + "','" + aGeneralInfo.MaritalStatus + "','" + aGeneralInfo.NationalIdNo + "','" + aGeneralInfo.JoiningDate + "'," + aGeneralInfo.DesignationId  + "," + aGeneralInfo.DepartmentId  + ",'" + aGeneralInfo.CompanyId + "','" + aGeneralInfo.Email + "','"
                                   + aGeneralInfo.EmrgContactNo + "','" + aGeneralInfo.RefName + "','" + aGeneralInfo.RefContactNo + "','" + aGeneralInfo.EntryBy + "','" + aGeneralInfo.EntryDate + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable LoadEmployeeView()
        {
            const string query = @"SELECT * FROM tblEmpGeneralInfo AS EMP
                                    INNER JOIN tblDesignation AS DSG ON EMP.DesignationId = DSG.DesignationId
                                    INNER JOIN tblDepartment AS DPT ON EMP.DepartmentId = DPT.DeptId
									INNER JOIN tblCompanyInfo AS CI ON EMP.CompanyId = CI.CompanyId
                                    WHERE EMP.EmpName != 'Vacant' ORDER BY EMP.EmpInfoId ASC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadEmpGeneralInformation()
        {
            string query = @"SELECT * FROM tblEmpGeneralInfo ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public void LoadDesignationName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblDesignation";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "DesigName", "DesignationId", queryStr);
        }

        public void LoadDepartmentName(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblDepartment";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "DeptName", "DeptId", queryStr);
        }

       
        public EmpGeneralInfo EmpInfoEditLoad(string employeeId)
        {
            string query = @"SELECT * FROM tblEmpGeneralInfo AS EMP
                            INNER JOIN tblDesignation AS DSG ON EMP.DesignationId = DSG.DesignationId
                            INNER JOIN tblCompanyInfo AS CI ON EMP.CompanyId = CI.CompanyId
                            INNER JOIN tblDepartment AS DPT ON EMP.DepartmentId = DPT.DeptId WHERE EMP.EmpInfoId = '" + employeeId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            EmpGeneralInfo aEmpGeneralInfo = new EmpGeneralInfo();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aEmpGeneralInfo.EmpInfoId = Int32.Parse(dataReader["EmpInfoId"].ToString());
                    aEmpGeneralInfo.CompanyId = Int32.Parse(dataReader["CompanyId"].ToString());
                    aEmpGeneralInfo.EmpMasterCode = dataReader["EmpMasterCode"].ToString();
                    aEmpGeneralInfo.EmpName = dataReader["EmpName"].ToString();
                    aEmpGeneralInfo.ShortName = dataReader["ShortName"].ToString();
                    aEmpGeneralInfo.FatherName = dataReader["FatherName"].ToString();
                    aEmpGeneralInfo.MotherName = dataReader["MotherName"].ToString();
                    aEmpGeneralInfo.Religion = dataReader["Religion"].ToString();
                    aEmpGeneralInfo.Nationality = dataReader["Nationality"].ToString();
                    aEmpGeneralInfo.DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"].ToString());
                    aEmpGeneralInfo.PlaceOfBirth = dataReader["PlaceOfBirth"].ToString();
                    aEmpGeneralInfo.BloodGroup = dataReader["BloodGroup"].ToString();
                    aEmpGeneralInfo.Gender = dataReader["Gender"].ToString();
                    aEmpGeneralInfo.AddressPresent = dataReader["AddressPresent"].ToString();
                    aEmpGeneralInfo.AddressPermanent = dataReader["AddressPermanent"].ToString();
                    aEmpGeneralInfo.PreCity = dataReader["PreCity"].ToString();
                    aEmpGeneralInfo.PerCity = dataReader["PerCity"].ToString();
                    aEmpGeneralInfo.CellNumber = dataReader["CellNumber"].ToString();
                    aEmpGeneralInfo.MaritalStatus = dataReader["MaritalStatus"].ToString();
                    aEmpGeneralInfo.NationalIdNo = dataReader["NationalIdNo"].ToString();
                    
                    aEmpGeneralInfo.DepartmentId = Int32.Parse(dataReader["DepartmentId"].ToString());
                    aEmpGeneralInfo.DeptName = (dataReader["DepartmentId"].ToString());
                    aEmpGeneralInfo.JoiningDate = Convert.ToDateTime(dataReader["JoiningDate"].ToString());
                    aEmpGeneralInfo.Designation = dataReader["DesigName"].ToString();
                    aEmpGeneralInfo.DesignationId = Convert.ToInt32(dataReader["DesignationId"].ToString());
                    aEmpGeneralInfo.EntryBy = dataReader["EntryBy"].ToString();
                    aEmpGeneralInfo.EntryDate = Convert.ToDateTime(dataReader["EntryDate"].ToString());

                    aEmpGeneralInfo.Email = dataReader["Email"].ToString();
                    aEmpGeneralInfo.RefName = dataReader["RefName"].ToString();
                    aEmpGeneralInfo.RefContactNo = dataReader["RefContactNo"].ToString();
                    aEmpGeneralInfo.EmrgContactNo = dataReader["EmrgContactNo"].ToString();
                }
            }
            return aEmpGeneralInfo;
        }

        public bool UpdateEmployeeInfo(EmpGeneralInfo aEmpGeneralInfo)
        {
            string query = @"UPDATE tblEmpGeneralInfo SET EmpName='" + aEmpGeneralInfo.EmpName + "',ShortName='" + aEmpGeneralInfo.ShortName + "',FatherName='" + aEmpGeneralInfo.FatherName + "',MotherName='" + aEmpGeneralInfo.MotherName + "',Religion='" + aEmpGeneralInfo.Religion + "',Nationality='" + aEmpGeneralInfo.Nationality + "'," +
                           "DateOfBirth='" + aEmpGeneralInfo.DateOfBirth + "',PlaceOfBirth='" + aEmpGeneralInfo.PlaceOfBirth + "',BloodGroup='" + aEmpGeneralInfo.BloodGroup + "',Gender='" + aEmpGeneralInfo.Gender + "',AddressPresent='" + aEmpGeneralInfo.AddressPresent + "',AddressPermanent='" + aEmpGeneralInfo.AddressPermanent + "',PreCity='" + aEmpGeneralInfo.PreCity + "',DesignationId='" + aEmpGeneralInfo.DesignationId +
                           "',PerCity='" + aEmpGeneralInfo.PerCity + "',CellNumber='" + aEmpGeneralInfo.CellNumber + "',MaritalStatus='" + aEmpGeneralInfo.MaritalStatus + "',NationalIdNo='" + aEmpGeneralInfo.NationalIdNo + "',CompanyId='" + aEmpGeneralInfo.CompanyId + "',Email='" + aEmpGeneralInfo.Email + "',EmrgContactNo='" + aEmpGeneralInfo.EmrgContactNo + "',RefName='" + aEmpGeneralInfo.RefName 
                           + "',RefContactNo='" + aEmpGeneralInfo.RefContactNo + "',JoiningDate='" + aEmpGeneralInfo.JoiningDate + "',DepartmentId='" + aEmpGeneralInfo.DepartmentId  + "',UpdateBy='" + aEmpGeneralInfo.UpdateBy + "',UpdateDate='" + aEmpGeneralInfo.UpdateDate + "' WHERE EmpInfoId=" + aEmpGeneralInfo.EmpInfoId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }


        public List<EmpGeneralInfo> ViewAllEmployee()
        {
            List<EmpGeneralInfo> allEmpGeneralInfoList = new List<EmpGeneralInfo>();
            string query = @"select * from tblEmpGeneralInfo";

            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");

            while (dataReader.Read())
            {
                EmpGeneralInfo aGeneralInfo = new EmpGeneralInfo();
                aGeneralInfo.EmpInfoId = Int32.Parse(dataReader["EmpInfoId"].ToString());
                aGeneralInfo.EmpMasterCode = (dataReader["EmpMasterCode"].ToString());
                aGeneralInfo.EmpName = dataReader["EmpName"].ToString();
                allEmpGeneralInfoList.Add(aGeneralInfo);
            }

            return allEmpGeneralInfoList;
        }

        public List<EmpGeneralInfo> ViewEmpName(string employeeId)
        {
            List<EmpGeneralInfo> singleEmpNameList = ViewAllEmployee();
            List<EmpGeneralInfo> singleEmpName = (from EmpGeneralInfo aGeneralInfo in singleEmpNameList
                                                  where aGeneralInfo.EmpMasterCode == employeeId
                                                  select aGeneralInfo).ToList();
            return singleEmpName;
        }

        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        
    }
}
