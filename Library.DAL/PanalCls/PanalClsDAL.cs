using System;
using System.Data;
using Library.DAL.InternalCls;

namespace Library.DAL.PanalCls
{
    public class PanalClsDAL
    {
        private ClsCommonInternalDAL _aCommonInternalDal;

        public DataTable Login(string loginName, string password)
        {
            _aCommonInternalDal = new ClsCommonInternalDAL();
            string queryString = @" select * from dbo.tblUser U left join tblUserCompanyUnit UC on U.UserId=UC.UserId  where LoginName='" + loginName + "' and Password='" + password + "' and UserStatus='active'";
            return _aCommonInternalDal.DataContainerDataTable(queryString);
        }

        public DataTable MainMenu()
        {
            _aCommonInternalDal = new ClsCommonInternalDAL();
            DataTable aTableMainMenu = new DataTable();
            string queryString = "select * from tblMainMenu where ParantId is null or ParantId=''";
            aTableMainMenu= _aCommonInternalDal.DataContainerDataTable(queryString);
            return aTableMainMenu;
        }
        public bool LoginLog(string userId,string LoginName,DateTime loginTime)
        {
            _aCommonInternalDal = new ClsCommonInternalDAL();
            string queryString = "INSERT INTO dbo.tblLoginLog ( UserId, LoginName, LoginTime ) VALUES  ( '"+userId+"', '"+LoginName+"', '"+loginTime+"')";
            _aCommonInternalDal.SaveDataByInsertCommand(queryString);
            return true;
        }
        public DataTable MainMenu(int userId)
        {
            _aCommonInternalDal = new ClsCommonInternalDAL();
            DataTable aTableMainMenu = new DataTable();
            string queryString = @"select tblMainMenu.* from tblMainMenu "+
                                    " INNER JOIN dbo.tblMenuDistribution ON dbo.tblMainMenu.SL = dbo.tblMenuDistribution.MenuSL " +
                                    " WHERE UserId='" + userId + "' AND (ParantId is null or ParantId='')";
            aTableMainMenu = _aCommonInternalDal.DataContainerDataTable(queryString);
            return aTableMainMenu;
        }
        public DataTable SubItem(string Id)
        {
            DataTable aDataTableSubItem = new DataTable();
            _aCommonInternalDal = new ClsCommonInternalDAL();
            string queryString = "select * from tblMainMenu where ParantId='" + Id + "'";
            aDataTableSubItem = _aCommonInternalDal.DataContainerDataTable(queryString);
            return aDataTableSubItem;
        }
        public DataTable SubItem(string Id, int userId)
        {
            DataTable aDataTableSubItem = new DataTable();
            _aCommonInternalDal = new ClsCommonInternalDAL();

            string queryString = @"select tblMainMenu.* from tblMainMenu " +
                                   " INNER JOIN dbo.tblMenuDistribution ON dbo.tblMainMenu.SL = dbo.tblMenuDistribution.MenuSL " +
                                   " WHERE UserId='" + userId + "' AND ParantId='" + Id + "'";

            
            aDataTableSubItem = _aCommonInternalDal.DataContainerDataTable(queryString);
            return aDataTableSubItem;
        }
        public DataTable SubSubItem(string Id)
        {
            DataTable aDataSubSubItem = new DataTable();
            _aCommonInternalDal = new ClsCommonInternalDAL();
            string queryString = "select * from tblMainMenu where ParantId='" + Id + "'";
            aDataSubSubItem = _aCommonInternalDal.DataContainerDataTable(queryString);
            return aDataSubSubItem;
        }
        public DataTable SubSubItem(string Id, int userId)
        {
            DataTable aDataSubSubItem = new DataTable();
            _aCommonInternalDal = new ClsCommonInternalDAL();
            string queryString = @"select tblMainMenu.* from tblMainMenu " +
                                   " INNER JOIN dbo.tblMenuDistribution ON dbo.tblMainMenu.SL = dbo.tblMenuDistribution.MenuSL " +
                                   " WHERE UserId='" + userId + "' AND ParantId='" + Id + "'";
            aDataSubSubItem = _aCommonInternalDal.DataContainerDataTable(queryString);
            return aDataSubSubItem;
        }
        public DataTable SubSubChildItem(string Id)
        {
            DataTable aDataSubSubChildItem = new DataTable();
            _aCommonInternalDal = new ClsCommonInternalDAL();
            string queryString = "select * from tblMainMenu where ParantId='" + Id + "'";
            aDataSubSubChildItem = _aCommonInternalDal.DataContainerDataTable(queryString);
            return aDataSubSubChildItem;
        }
        public DataTable SubSubChildItem(string Id, int userId)
        {
            DataTable aDataSubSubChildItem = new DataTable();
            _aCommonInternalDal = new ClsCommonInternalDAL();
            string queryString = @"select tblMainMenu.* from tblMainMenu " +
                                    " INNER JOIN dbo.tblMenuDistribution ON dbo.tblMainMenu.SL = dbo.tblMenuDistribution.MenuSL " +
                                    " WHERE UserId='" + userId + "' AND ParantId='" + Id + "'";
            aDataSubSubChildItem = _aCommonInternalDal.DataContainerDataTable(queryString);
            return aDataSubSubChildItem;
        }

    }
}
