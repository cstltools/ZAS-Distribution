using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using System.Web.UI.WebControls;
using System.Data;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
   public class DcWiseAreaAloDal
    {
       private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
       ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

       public void LoadCompanyName(DropDownList ddl)
       {
          // ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
           string queryStr = "SELECT CompanyId, CompanyName FROM tblCompanyInfo";
           aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
       }

       public void LoadDC(DropDownList ddl, string companyId)
       {
           //ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
           string queryStr = "select * from tblCompanyUnit where CompanyId='"+ companyId +"'";
           aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
       }

       public DataTable LoadAreaView()
       {
           const string query = @"SELECT * FROM tblArea ";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }

       // Cheek in Cheekbox 

       public DataTable LoadDcWiseAreaAloByDc(string DCId)
       {
           string query = @"SELECT * FROM dbo.tblDcWiseAreaInfo WHERE IsActive = 'True' AND DCId = '" + DCId + "'";
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }


       public DataTable GetDcWiseAreaAloInfo(int DCId)
       {
           string query = @"SELECT * FROM dbo.tblDcWiseAreaInfo WHERE IsActive = 'True' AND DCId = " + DCId;
           return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
       }
       

       public bool SaveDcWiseAreaAlo(DcWiseAreaAloDao dcWiseAreaAlo)
       {
           string insertQuery = @"insert into tblDcWiseAreaInfo (DCId,AreaId,IsActive,EntryBy,EntryDate) 
            values (" + "'" + dcWiseAreaAlo.DCId + "','" + dcWiseAreaAlo.AreaId + "','" + dcWiseAreaAlo.IsActive + "','" + dcWiseAreaAlo.EntryBy + "','" + dcWiseAreaAlo.EntryDate + "')";
           return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
       }


       public bool UpdateDcWiseAreaAlo(DcWiseAreaAloDao areaAlo)
       {
           string query = @"UPDATE tblDcWiseAreaInfo SET IsActive = '" + areaAlo.IsActive + "', UpdateBy = '" + areaAlo.UpdateBy + "', UpdateDate = '" + areaAlo.UpdateDate + "' WHERE DcWiseAreaId = '" + areaAlo.DcWiseAreaId + "'";
           return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
       }
    }
}
