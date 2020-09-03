using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;
using System.Web.UI.WebControls;
using System.Data;
using Library.DAL.InternalCls;

namespace Library.BLL.SInventory_BLL
{
   public class DcWiseAreaAloBll
    {

       DcWiseAreaAloDal aDcWiseDAL = new DcWiseAreaAloDal();

       public void LoadCompanyDropdownList(DropDownList ddl)
       {
           aDcWiseDAL.LoadCompanyName(ddl);
       }

       public void LoadDC(DropDownList ddl, string companyId)
       {
           aDcWiseDAL.LoadDC(ddl,companyId);
       }

       public DataTable LoadAreaView()
       {
           return aDcWiseDAL.LoadAreaView();
       }


       public DataTable LoadDcWiseAreaAloByDc(string DcWiseAreaID)
       {
           return aDcWiseDAL.LoadDcWiseAreaAloByDc(DcWiseAreaID);
       }

     //////

       public DataTable LoadDcWiseAreaAloInfo(int DCId)
       {
           return aDcWiseDAL.GetDcWiseAreaAloInfo(DCId);
       }

 
       public string LoadDcWiseAreaAloDataForSave(DcWiseAreaAloDao dcWiseAreaAlo)
       {
           try
           {
               aDcWiseDAL.SaveDcWiseAreaAlo(dcWiseAreaAlo);
               return "Data Save Successfully";
           }
           catch (Exception ex)
           { 
               throw ex;
           }
        
       }

       public bool SaveDcWiseAreaInfo(List<DcWiseAreaAloDao> areaListForSave)
       {
           bool id = true;

           foreach (DcWiseAreaAloDao areaAlo in areaListForSave)
           {
               id = aDcWiseDAL.SaveDcWiseAreaAlo(areaAlo);
           }

           return id;
       }

       public bool UpdateDcWiseAreaInfo(List<DcWiseAreaAloDao> areaListForUpdate)
       {
           bool id = true;

           foreach (DcWiseAreaAloDao areaAlo in areaListForUpdate)
           {
               id = aDcWiseDAL.UpdateDcWiseAreaAlo(areaAlo);
           }

           return id;
       }
    }
}

