using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class DesignationDal
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public bool SaveDepartmentInfo(Designation aDepartment)
        {
            string insertQuery = @"insert into tblDesignation (DesigName) 
            values ('"+ aDepartment.DesignationName + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery,"SSIDB");
        }

        public bool HasDeptName(Designation aDepartment)
        {
            string query = "select * from tblDesignation where DesigName = '" + aDepartment.DesignationName + "'";
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

        public DataTable LoadDesignation()
        {
            string query = @"select * from tblDesignation";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public Designation DesignationEditLoad(string designationId)
        {
            string query = "select * from tblDesignation where DesignationId = '" + designationId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");

            var aDepartment = new Designation();

            //EmpGeneralInfo aGeneralInfo=new EmpGeneralInfo();


            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aDepartment.DesignationId = Int32.Parse(dataReader["DesignationId"].ToString());
                    aDepartment.DesignationName = dataReader["DesigName"].ToString();                    
                }

            }
            return aDepartment;
        }

        public bool UpdateDepartmentInfo(Designation aDepartment)
        {
            string query = @"UPDATE tblDesignation SET DesigName ='" + aDepartment.DesignationName + "' WHERE DesignationId =" + aDepartment.DesignationId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable NameExistOrNot(string name)
        {
            string query = @"select * from tblDesignation WHERE DesigName = '" + name + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
 }

