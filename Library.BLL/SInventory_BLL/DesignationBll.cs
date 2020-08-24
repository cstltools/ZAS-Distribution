using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class DesignationBll
    {
        DesignationDal aDepartmentDal = new DesignationDal();

        public string SaveDataForDesignation(Designation aDepartment)
        {
            try
            {
                if (!aDepartmentDal.HasDeptName(aDepartment))
                {
                    aDepartmentDal.SaveDepartmentInfo(aDepartment);
                    return  "Designation Name is : " + aDepartment.DesignationName;
                }
                else
                {
                    return "Designation Name already exist";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        public string DepartmentCodeGenerator(int id)
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
            code = "DEPT-" + Id;
            return code;
        }

        public bool UpdateDataForDesignation(Designation aDepartment)
        {
            return aDepartmentDal.UpdateDepartmentInfo(aDepartment);
        }

        public DataTable LoadDesignationInfo()
        {
            return aDepartmentDal.LoadDesignation();
        }

        public Designation DesignationEditLoad(string designationId)
        {
            return aDepartmentDal.DesignationEditLoad(designationId);
        }

        public DataTable NameExistOrNot(string name)
        {
            return aDepartmentDal.NameExistOrNot(name);
        }
    }
}

