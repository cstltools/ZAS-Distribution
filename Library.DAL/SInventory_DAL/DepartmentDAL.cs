using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class DepartmentDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveDepartmentInfo(Department aDepartment)
        {
            string insertQuery = @"insert into tblDepartment (DeptId,DeptCode,DeptName) 
            values ("+aDepartment.DepartmentId+",'" + aDepartment.DeaprtmentCode +
                                 "','" + aDepartment.DepartmentName + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery,"SSIDB");
        }

        public bool HasDeptName(Department aDepartment)
        {
            string query = "select * from tblDepartment where DeptName = '" + aDepartment.DepartmentName + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            if (dataReader != null)
            {
                if (dataReader.Read())
                {
                    while (dataReader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public DataTable LoadDepartment()
        {
            string query = @"select * from tblDepartment";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public Department DepartmentEditLoad(string DeptId)
        {
            string query = "select * from tblDepartment where DeptId = '" + DeptId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            Department aDepartment = new Department();
            //EmpGeneralInfo aGeneralInfo=new EmpGeneralInfo();


            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aDepartment.DepartmentId = Int32.Parse(dataReader["DeptId"].ToString());
                    aDepartment.DepartmentName = dataReader["DeptName"].ToString();
                    
                }

            }
            return aDepartment;
        }

        public bool UpdateDepartmentInfo(Department aDepartment)
        {
            string query = @"UPDATE tblDepartment SET DeptName='" + aDepartment.DepartmentName + "' WHERE DeptId=" + aDepartment.DepartmentId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable NameExistOrNot(string name)
        {
            string query = @"select * from tblDepartment WHERE DeptName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
