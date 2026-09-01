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
    public class UserDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveUserInformation(UserInformation aInformation)
        {
            string insertQuery = @"insert into tblUser (UserId,UserName,UserType,LoginName,Password,UserStatus,Email,ContactNo,IsCreditApproval,UserCode,IsHead) 
            values (" + aInformation.UserId + ",'" + aInformation.UserName + "','" + aInformation.UserType + "','" + aInformation.LoginName + "'," +
                      "'" + aInformation.Password + "','" + aInformation.UserStatus + "','" + aInformation.Email + "','" + aInformation.ContactNo + "','" + aInformation.IsCreditApproval + "','" + aInformation.UserCode + "','" + aInformation.IsHead + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool SaveUserCompanyUnit(UserCompanyUnit aCompanyUnit)
        {
            string insertQuery = @"insert into tblUserCompanyUnit (UserComId,UserId,CompanyUnitId) 
            values (" + aCompanyUnit.UserComId + "," + aCompanyUnit.UserId + "," + aCompanyUnit.CompanyUnitId + ")";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool HasUserInformationName(UserInformation aUserInformation)
        {
            string query = "select * from tblUser where UserName = '" + aUserInformation.UserName + "'";
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

        public bool HasLoginName(UserInformation aUserInformation)
        {
            string query = "select * from tblUser where LoginName = '" + aUserInformation.LoginName + "'";
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

        public bool HasEmail(UserInformation aUserInformation)
        {
            string query = "select * from tblUser where Email = '" + aUserInformation.Email + "'";
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
       
        public DataTable LoadUserView()
        {
            string query = @"SELECT 
    tblUser.UserId,
    tblUser.UserName,
    tblUser.UserType,
    tblUser.LoginName,
    tblUser.Password,
    tblUser.UserStatus,
    tblUser.Email,
    tblUser.ContactNo 
FROM tblUser 
WHERE UserId != 1 
  AND ISNULL(IsAppsUser,0) = 0";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public UserInformation UserInformationEditLoad(string userId)
        {
            string query = "select * from tblUser where UserId = '" + userId + "'";
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            UserInformation aUserInformation = new UserInformation();
            //EmpGeneralInfo aGeneralInfo=new EmpGeneralInfo();

            
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aUserInformation.UserId = Int32.Parse(dataReader["UserId"].ToString());
                    //aGeneralInfo.EmpMasterCode = dataReader["EmpMasterCode"].ToString();
                    aUserInformation.UserName = dataReader["UserName"].ToString();
                    aUserInformation.UserType = dataReader["UserType"].ToString();
                    aUserInformation.LoginName = dataReader["LoginName"].ToString();
                    aUserInformation.Password = dataReader["Password"].ToString();
                    aUserInformation.UserStatus = dataReader["UserStatus"].ToString();
                    aUserInformation.Email = dataReader["Email"].ToString();
                    aUserInformation.ContactNo = dataReader["ContactNo"].ToString();
                    aUserInformation.IsHead = Convert.ToBoolean(dataReader["IsHead"].ToString());
                    aUserInformation.IsCreditApproval = Convert.ToBoolean(dataReader["IsCreditApproval"].ToString());
                }

            }
            return aUserInformation;
        }

        public bool UpdateUserInfo(UserInformation aUserInformation)
        {

            string query = @"UPDATE tblUser SET UserName='" + aUserInformation.UserName + "',UserType='" + aUserInformation.UserType + "',LoginName='" + aUserInformation.LoginName + "',Password='" + aUserInformation.Password + "',UserStatus='" + aUserInformation.UserStatus + "',Email='" + aUserInformation.Email + "',IsCreditApproval='" + aUserInformation.IsCreditApproval + "',ContactNo='" + aUserInformation.ContactNo + "',IsHead='" + aUserInformation.IsHead + "' WHERE UserId=" + aUserInformation.UserId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public bool DeleteUserInfo(string userId)
        {

            string query = @"Delete from tblUser WHERE UserId=" + userId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public void LoadUserType(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblUserType";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "UserType", "UserTypeId", queryStr);
        }

        public void LoadComUnit(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select ComUnitCode+':'+ComUnitName as ComUnitName,ComUnitId from tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
    }
}
