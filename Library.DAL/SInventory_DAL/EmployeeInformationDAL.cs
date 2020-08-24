using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using System.Web.UI.WebControls;
using System.Data;

namespace Library.DAL.SInventory_DAL
{
   public class EmployeeInformationDAL
    {

       ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public void GroupDropDown(DropDownList groupDropDownList)
        {
            string ei = @"SELECT GroupId, GroupName FROM tblGroupInformation";
            aCommonInternalDAL.LoadDropDownValue(groupDropDownList, "GroupName", "GroupId", ei, "SSIDB");
        }

        public void CompanyNameDropDown(DropDownList companyNameDropDownList)
        {
            string cn = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo";
            aCommonInternalDAL.LoadDropDownValue(companyNameDropDownList, "CompanyName", "CompanyId", cn, "SSIDB");
        }

        public DataTable GetAllEmployeeInformation(string parameter)
        {
            string queryStr = @"SELECT CI.CompanyName,EGI.EmpMasterCode,EGI.EmpName,EGI.ShortName,EGI.FatherName,EGI.MotherName,EGI.Religion,EGI.Nationality,EGI.DateOfBirth,EGI.PlaceOfBirth,EGI.BloodGroup,EGI.Gender,EGI.AddressPresent,EGI.AddressPermanent,EGI.NationalIdNo,EGI.CellNumber,EGI.Email,EGI.EmrgContactNo,EGI.MaritalStatus,EGI.RefName,EGI.RefContactNo,DG.DesigName,DEP.DeptName,EGI.JoiningDate,EGI.EntryBy,EGI.EntryDate,EGI.UpdateBy,EGI.UpdateDate
                                        FROM tblEmpGeneralInfo as EGI
                                        JOIN tblCompanyInfo as CI ON CI.CompanyId=EGI.CompanyId
                                        JOIN tblDesignation as DG ON DG.DesignationId=EGI.DesignationId
                                        JOIN tblDepartment as DEP ON DEP.DeptId=EGI.DepartmentId" + parameter;
            return aCommonInternalDAL.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable GetDeliveryPerson(string parameter)
        {
            string queryStr = @"SELECT EGI.EmpMasterCode,EGI.EmpName,EGI.NationalIdNo,EGI.CellNumber,EGI.AddressPresent, DMI.IsActive, DMI.Remarks
                                FROM tblDeliveryManInfo as DMI 
                                JOIN tblEmpGeneralInfo as EGI ON EGI.EmpInfoId= DMI.DeliveryManId WHERE EGI.EmpName !='Vacant'";
            
            return aCommonInternalDAL.DataContainerDataTable(queryStr, "SSIDB");
        }
    }
}
